<?php
/*
 * @ImportController.php
 * 日程表取込画面コントローラー
 *
 * @create 2020/09/24 Cuong
 *
 * @update 2020/10/26 Cuong update condition delete of function deleteDataImportOld24h()
 * @update 2020/10/27 Cuong update insert data to T_Import
 */

namespace App\Http\Controllers\Schet;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Pagination\LengthAwarePaginator;
use App\Librarys\TimeTrackerFuncSchet;
use App\Librarys\TimeTrackerCommon;
use App\Librarys\CustomException;
use App\Http\Requests\Schet\ImportContentsRequest;
use App\Models\MstProject;
use App\Models\MstOrderNo;
use App\Models\T_Tosai;
use App\Models\T_Kyokyu;
use App\Models\T_Sogumi;
use App\Models\T_ImportData;
use App\Models\T_ImportHistory;
use App\Models\T_ImportLog;
use DB;
use Carbon\Carbon;
use Exception;
/*
 * 日程表取込画面コントローラー
 *
 * @create 2020/09/24 Cuong
 *
 * @update
 */
class ImportController extends Controller
{
	private $mtosaiRowStart = null;	//tosai : row start
	private $mtosaiRowEnd = null;	//tosai : row end
	private $mtosaiColStart = null;	//tosai : col start
	private $mtosaiColEnd = null;	//tosai : col end
	private $mtosaiDayRow = null;	//tosai : row day
	private $msogumiRowStart = null; //sogumi : row start
	private $msogumiRowEnd = null;	//sogumi : row end
	private $msogumiColStart = null;	//sogumi : col start
	private $msogumiColEnd = null;	//sogumi : col end
	private $msogumiDayRow = null;	//sogumi : row day
	private $marrSymbolTosai= array();	//tosai : symbol
	private $marrSymbolSogumi= array();	//kumiku : symbol

	/**
	 * 日程表取込画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/09/24 Cuong
	 * @update
	 */
	public function index(Request $request)
	{
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));
		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
			//select data
		$projects = MstProject::select('ID','ProjectName')
							->where('SysKindID','=',$menuInfo->KindID)
							->where('ListKind', '=', config('system_const.project_listkind_tosai'))
							->orderBy('ProjectName','asc')->get();

		$orders = MstOrderNo::select('OrderNo')
							->where('DispFlag','=', 0)
							->orderBy('OrderNo','asc')->get();

		$originalError = array();
		if (isset($request->err1)) {
			//error
			$originalError[] = valueUrlDecode($request->err1);
		}

		$this->data['request'] = $request;
		$this->data['menuInfo'] = $menuInfo;
		$this->data['projects'] = $projects;
		$this->data['orders'] = $orders;
		$this->data['originalError'] = $originalError;
		return view('Schet/Import/index', $this->data);
	}
	/**
	 * import action button
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/09/29 Cuong
	 * @update 2020/11/20 Cuong Update process data change
	 */
	public function import(ImportContentsRequest $request)
	{
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));
		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		// check file updloaded
		$resultCheck = $this->checkFileUploaded($request , $menuInfo);
		if($resultCheck != ''){
			//when has err
			return redirect($resultCheck);
		}

		/* Check sheet setting and initialize worksheet read sheet data*/
		$worksheet = $this->initializeWorkSheet($request, $menuInfo);
		if(is_string($worksheet)){
			//when sheet seeting has err
			return redirect($worksheet);
		}
		/* Get data from sheetdata file excel */
		$dataExcelToSai = array();
		$dataExcelKyokyu = array();
		$dataExcelSogumi = array();

		// get data tosai from excel
		$this->getTosaiData($worksheet, $dataExcelToSai);

		//get data sogumi and kyokyu from excel
		$this->getSogumiData($worksheet, $dataExcelSogumi, $dataExcelKyokyu);

		// get import id from seq_T_ImportData
		$seq_ImportData = DB::select('SELECT NEXT VALUE FOR seq_T_ImportData as ImportID');
		$importID = $seq_ImportData[0]->ImportID;

		/* Check error data file excel */
		$result = $this->checkErrDataExcel($dataExcelToSai, $dataExcelKyokyu, $dataExcelSogumi, $request, $menuInfo, $importID);
		if(is_string($result)) {
			//when insert database has error
			return redirect($result);
		}
		if(!$result) {
			/* 搭載日程取込データへの登録 */

			//既存のデータを取得する
			$dataTosai = T_Tosai::select('BlockName','BlockKumiku','WorkItemID')
								->selectRaw('SUBSTRING(BlockName, 1, 12) as BlockNameNotGen,
											SUBSTRING(BlockName, 13, 1) as Gen')
								->where('ProjectID', '=', $request->val1)
								->where('OrderNo', '=', $request->val2)
								->get()->toArray();

			$dataKyokyu = T_Kyokyu::select('BlockName','BlockKumiku','WorkItemID', 'K_BlockName','K_BlockKumiku')
								->selectRaw('SUBSTRING(BlockName, 1, 12) as BlockNameNotGen,
											SUBSTRING(BlockName, 13, 1) as Gen,
											SUBSTRING(K_BlockName, 1, 12) as K_BlockNameNotGen,
											SUBSTRING(K_BlockName, 13, 1) as K_Gen')
								->where('ProjectID', '=', $request->val1)
								->where('OrderNo', '=', $request->val2)
								->get()->toArray();

			$dataSogumi  = T_Sogumi::select('BlockName','BlockKumiku','WorkItemID')
								->selectRaw('SUBSTRING(BlockName, 1, 12) as BlockNameNotGen,
											SUBSTRING(BlockName, 13, 1) as Gen')
								->where('ProjectID', '=', $request->val1)
								->where('OrderNo', '=', $request->val2)
								->get()->toArray();

			/* get data TimeTrackerNX */
			$timeTrackerSchet = new TimeTrackerFuncSchet();

			$arrWorkItemIDTosai = array_column($dataTosai, 'WorkItemID');
			$arrWorkItemIDSogumi = array_column($dataSogumi, 'WorkItemID');
			$arrWorkItemIDKyokyu = array_column($dataKyokyu, 'WorkItemID');

			$arrWorkItemID = array_merge($arrWorkItemIDTosai, $arrWorkItemIDSogumi, $arrWorkItemIDKyokyu);
			$resultDataTimeTracker = $timeTrackerSchet->getWorkItemDate($arrWorkItemID);

			if(is_string($resultDataTimeTracker)) {
				$url = url('/');
				$url .= '/' . $menuInfo->KindURL;
				$url .= '/' . $menuInfo->MenuURL;
				$url .= '/index';
				$url .= '?cmn1=' . valueUrlEncode($menuInfo->KindID);
				$url .= '&cmn2=' . valueUrlEncode($menuInfo->MenuID);
				$url .= '&val1=' . valueUrlEncode($request->val1);
				$url .= '&val2=' . valueUrlEncode($request->val2);
				$url .= '&val5=' . valueUrlEncode($request->val5);
				$url .= '&err1=' . valueUrlEncode($resultDataTimeTracker);
				return redirect($url);
			}

			/* process new data */
			$dataAddNew = array();
			$this->processDataNew($dataTosai, $dataExcelToSai, $menuInfo, $dataAddNew);
			$this->processDataNew($dataKyokyu, $dataExcelKyokyu , $menuInfo, $dataAddNew);
			$this->processDataNew($dataSogumi, $dataExcelSogumi, $menuInfo, $dataAddNew);

			/* process delete data */
			$dataDelete = array();
			$kind = config('system_const_schet.import_kind_tosai');
			$this->processDataDelete($dataTosai, $dataExcelToSai, $menuInfo, $kind, $dataDelete);
			$kind = config('system_const_schet.import_kind_kyokyu');
			$this->processDataDelete($dataKyokyu, $dataExcelKyokyu, $menuInfo, $kind, $dataDelete);
			$kind = config('system_const_schet.import_kind_sogumi');
			$this->processDataDelete($dataSogumi, $dataExcelSogumi, $menuInfo, $kind, $dataDelete);

			/* process data change */
			$dataChange = array();
			if(count($dataExcelToSai) > 0 && count($dataTosai) > 0) {
				// group datatosai by blockname not gen and blockkumiku
				$grDataTosai = $this->groupDataOld($dataTosai);
				// group datatosai excel by blockname not gen and blockkumiku
				$grDataTosaiExcel = $this->groupDataExcel($dataExcelToSai);

				$offset = 0;
				$length = count($dataTosai);
				$dataTosaiTimeTracker = array_slice($resultDataTimeTracker, $offset, $length);
				$this->processDataChange($grDataTosai, $grDataTosaiExcel, $dataTosaiTimeTracker, $menuInfo, $dataChange);
			}

			if(count($dataExcelSogumi) > 0 && count($dataSogumi) > 0) {
				// group datasogumi by blockname not gen and blockkumiku
				$grDataSogumi = $this->groupDataOld($dataSogumi);
				// group datasogumi excel by blockname not gen and blockkumiku
				$grDataSogumiExcel = $this->groupDataExcel($dataExcelSogumi);

				$offset = count($dataTosai);
				$length = count($dataSogumi);
				$dataSogumiTimeTracker = array_slice($resultDataTimeTracker, $offset, $length);
				$this->processDataChange($grDataSogumi, $grDataSogumiExcel, $dataSogumiTimeTracker, $menuInfo, $dataChange);
			}

			if(count($dataExcelKyokyu) > 0 && count($dataKyokyu) > 0) {
				// group datakyokyu by blockname not gen and blockkumiku
				$grDataKyokyu = $this->groupDataOld($dataKyokyu);
				// group datakyokyu excel by blockname not gen and blockkumiku
				$grDataKyokyuExcel = $this->groupDataExcel($dataExcelKyokyu);

				if(count($dataSogumi) == 0) {
					$offset = count($dataTosai);
				}else {
					$offset =  count($dataTosai) + count($dataSogumi);
				}
				$length = count($dataKyokyu);
				$dataKyokyuTimeTracker = array_slice($resultDataTimeTracker, $offset, $length);
				$this->processDataChange($grDataKyokyu, $grDataKyokyuExcel, $dataKyokyuTimeTracker, $menuInfo, $dataChange);
			}
			// get import id from seq_T_ImportData
			$res = $this->insertImportData($dataAddNew, $dataDelete, $dataChange, $importID, $menuInfo, $request);
			if($res != 1) {
				return redirect($res);
			}
		}

		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/create';
		$url .= '?cmn1=' . valueUrlEncode($menuInfo->KindID);
		$url .= '&cmn2=' . valueUrlEncode($menuInfo->MenuID);
		$url .= '&val1=' . valueUrlEncode($request->val1);
		$url .= '&val2=' . valueUrlEncode($request->val2);
		$url .= '&val5=' . valueUrlEncode($request->val5);
		$url .= '&val6=' . valueUrlEncode($importID);
		return redirect($url);

	}

	/**
	 * function Check sheet setting and initialize worksheet read sheet data
	 *
	 * @param Request
	 * @param menuInfo
	 * @return mixed
	 *
	 * @create 2020/09/29 Cuong
	 * @update
	 */
	private function initializeWorkSheet ($request, $menuInfo){
		$file = $request->val3;
		$extension = strtolower($file->getClientOriginalExtension()); //extension file uploaded
		$inputFileName = $_FILES['val3']['tmp_name'];

		/* Create a new Xlsx Reader  */
		if($extension == 'xlsx'){
			$reader = new \PhpOffice\PhpSpreadsheet\Reader\Xlsx();
		}
		/* Create a new Xls Reader  */
		if($extension == 'xls'){
			$reader = new \PhpOffice\PhpSpreadsheet\Reader\Xls();
		}

		/** Load $inputFileName to a Spreadsheet Object  **/
		$worksheetData = $reader->listWorksheetInfo($inputFileName);
		/* Validate sheet setting */
		$checkSheetSetting = $this->checkSheetSettingExcel($reader ,$worksheetData, $inputFileName, $request, $menuInfo);
		if(!empty($checkSheetSetting)){
			return $checkSheetSetting;
		}

		/* Read sheet data */
		$sheetData  = $worksheetData[0];
		$reader->setLoadSheetsOnly($sheetData);
		$reader->setReadDataOnly(false);
		$spreadsheet = $reader->load($inputFileName);
		$worksheet = $spreadsheet->getActiveSheet();
		return $worksheet;
	}

	/**
	 * group data by blockname not gen and blockkumiku method
	 *
	 * @param array dataOlds
	 * @return array
	 *
	 * @create 2020/11/20 Cuong
	 * @update
	 */
	private function groupDataOld($dataOlds) {
		$grData = array();
		foreach ($dataOlds as $key => $old) {
			$arr = array_filter($dataOlds, function ($item) use($old){
				return $item['BlockNameNotGen'] == $old['BlockNameNotGen'] && $item['BlockKumiku'] == $old['BlockKumiku'];
			});

			if(count($arr) > 0) {
				$key = $old['BlockKumiku'].'-'.$old['BlockNameNotGen'];
				$grData[$key] = $arr;
			}
		}
		return $grData;
	}


	/**
	 * group data excel by blockname not gen and blockkumiku method
	 *
	 * @param array dataOlds
	 * @return array
	 *
	 * @create 2020/11/20 Cuong
	 * @update
	 */
	private function groupDataExcel($dataExcels) {
		$grData = array();
		foreach ($dataExcels as $key => $dataExcel) {
			$blockNameFulls = $this->convertBlockName($dataExcel['BlockNick']);
			$blockNameNotGen = $this->GetTextExceptCharIndex($blockNameFulls[0], self::EXCEPT_CHAR_INDEX);

			$arr = array_filter($dataExcels, function ($item) use($dataExcel, $blockNameNotGen){
				$blockNameFulls_1 = $this->convertBlockName($item['BlockNick']);
				$blockNameNotGen_1 = $this->GetTextExceptCharIndex($blockNameFulls_1[0], self::EXCEPT_CHAR_INDEX);
				return  $blockNameNotGen == $blockNameNotGen_1 && $item['BlockKumiku'] == $dataExcel['BlockKumiku'];
			});

			if(count($arr) > 0) {
				$key = $dataExcel['BlockKumiku'].'-'.$blockNameNotGen;
				$grData[$key] = $arr;
			}
		}
		return $grData;
	}

	/**
	 * function check file uploaded
	 *
	 * @param Request
	 * @param menuInfo
	 * @return string
	 *
	 * @create 2020/09/29 Cuong
	 * @update 2020/11/18 Cuong update conditions check file xls
	 */
	private function checkFileUploaded($request, $menuInfo)
	{
		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/index';
		$url .= '?cmn1=' . valueUrlEncode($menuInfo->KindID);
		$url .= '&cmn2=' . valueUrlEncode($menuInfo->MenuID);
		$url .= '&val1=' . valueUrlEncode($request->val1);
		$url .= '&val2=' . valueUrlEncode($request->val2);
		$url .= '&val5=' . valueUrlEncode($request->val5);

		if ($request->hasFile('val3')) {

			$file = $request->val3;
			if (is_array($_FILES['val3']['name'])) {
				$originalError = config('message.msg_file_003');
				$url .= '&err1=' . valueUrlEncode($originalError);
				return $url;
			}

			$extension = strtolower($file->getClientOriginalExtension());

			if (!in_array($extension, ['xls', 'xlsx'])) {
				$originalError = config('message.msg_file_001');
				$url .= '&err1=' . valueUrlEncode($originalError);
				return $url;
			}

			if (str_contains($_FILES['val3']['name'], '\\') || str_contains($_FILES['val3']['name'], '\/')) {
				$originalError = config('message.msg_file_003');
				$url .= '&err1=' . valueUrlEncode($originalError);
				return $url;
			}

			if($_FILES['val3']['error'] == 1 || $_FILES['val3']['error'] == 2){
				$originalError = config('message.msg_file_002');
				$url .= '&err1=' . valueUrlEncode($originalError);
				return $url;
			}

			if($_FILES['val3']['error'] != 0 && $_FILES['val3']['error'] != 1  && $_FILES['val3']['error'] != 2){
				$originalError = config('message.msg_file_003') + '(' + $_FILES['val3']['error'] + ')' ;
				$url .= '&err1=' . valueUrlEncode($originalError);
				return $url;
			}

			if(!in_array($_FILES['val3']['type'], ['application/vnd.openxmlformats-officedocument.spreadsheetml.sheet','application/vnd.ms-excel'])){
				$originalError = config('message.msg_file_001');
				$url .= '&err1=' . valueUrlEncode($originalError);
				return $url;
			}

			return '';
		}
	}

	/**
	 * function check file excel
	 *
	 * @param reader
	 * @param worksheetData
	 * @param inputFileName
	 * @param request
	 * @param menuInfo
	 * @return string
	 *
	 * @create 2020/10/01 Cuong
	 * @update 2020/10/26 Cuong get value symbol from excel sheet setting
	 * @update 2020/11/20 Cuong update condition check sheet setting excel
	 */
	private function checkSheetSettingExcel($reader, $worksheetData, $inputFileName, $request, $menuInfo)
	{
		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/index';
		$url .= '?cmn1=' . valueUrlEncode($menuInfo->KindID);
		$url .= '&cmn2=' . valueUrlEncode($menuInfo->MenuID);
		$url .= '&val1=' . valueUrlEncode($request->val1);
		$url .= '&val2=' . valueUrlEncode($request->val2);
		$url .= '&val5=' . valueUrlEncode($request->val5);

		//check first sheet name
		$firstSheetName = $worksheetData[0]['worksheetName'];
		if ($firstSheetName == config('system_const_schet.schet_settingsheet_name')) {
			$originalError = config('message.msg_schet_excel_error_001');
			$url .= '&err1=' . valueUrlEncode($originalError);
			return $url;
		}

		//check 設定シート is exits
		$isCheckExits = array_search(config('system_const_schet.schet_settingsheet_name'), array_column($worksheetData, 'worksheetName'));
		if ($isCheckExits === FALSE) {
			$originalError = config('message.msg_schet_excel_error_002');
			$url .= '&err1=' . valueUrlEncode($originalError);
			return $url;
		}

		//check values exits E4-E32
		$sheetSetting  = $worksheetData[$isCheckExits];
		$reader->setLoadSheetsOnly($sheetSetting);
		$spreadsheet = $reader->load($inputFileName);
		$worksheet = $spreadsheet->getActiveSheet();

		$index = 0;
		$rowStart = preg_replace('/[^0-9]+/', '', config('system_const_schet.import_setting_range_start'));
		$rowEnd = preg_replace('/[^0-9]+/', '', config('system_const_schet.import_setting_range_end'));
		$colName = preg_replace('/[^a-zA-Z]+/', '', config('system_const_schet.import_setting_range_end'));
		$colName = strtoupper($colName);

		for ($i=$rowStart; $i <= $rowEnd; $i++) {
			//check 値が入っていない
			$cellValue = $worksheet->getCell($colName.$i)->getValue();
			$cellValue = mb_convert_kana($cellValue, "KVa");
			if((is_null($cellValue) || $cellValue == "") && $index != config('system_const_schet.tosai_import_column_end')
			&& $index != config('system_const_schet.sogumi_import_column_end')){
				$originalError = config('message.msg_schet_excel_error_003');
				$url .= '&err1=' . valueUrlEncode($originalError);
				return $url;
			}

			//check 数値以外が入っている 0 以下の数値が入っている
			if( $index == config('system_const_schet.tosai_day_row') && ( (filter_var($cellValue, FILTER_VALIDATE_INT) === false ) || $cellValue <=0 ) ){
				$originalError = config('message.msg_schet_excel_error_004');
				$url .= '&err1=' . valueUrlEncode($originalError);
				return $url;
			}

			if( $index == config('system_const_schet.tosai_day_row') ){
				$this->mtosaiDayRow = $cellValue;
			}

			if( $index == config('system_const_schet.tosai_import_row_start') && ( (filter_var($cellValue, FILTER_VALIDATE_INT) === false ) || $cellValue <=0 ) ){
				$originalError = config('message.msg_schet_excel_error_004');
				$url .= '&err1=' . valueUrlEncode($originalError);
				return $url;
			}

			if( $index == config('system_const_schet.tosai_import_row_start')) {
				$this->mtosaiRowStart = $cellValue;
			}

			if( $index == config('system_const_schet.tosai_import_row_end') && ( (filter_var($cellValue, FILTER_VALIDATE_INT) === false ) || $cellValue <=0 ) ){
				$originalError = config('message.msg_schet_excel_error_004');
				$url .= '&err1=' . valueUrlEncode($originalError);
				return $url;
			}

			if( $index == config('system_const_schet.tosai_import_row_end')) {
				$this->mtosaiRowEnd = $cellValue;
			}

			if( $index == config('system_const_schet.tosai_import_column_start') && ( (filter_var($cellValue, FILTER_VALIDATE_INT) === false ) || $cellValue <=0 ) ){
				$originalError = config('message.msg_schet_excel_error_004');
				$url .= '&err1=' . valueUrlEncode($originalError);
				return $url;
			}

			if( $index == config('system_const_schet.tosai_import_column_start')) {
				$this->mtosaiColStart = $cellValue;
			}

			if( $index == config('system_const_schet.tosai_import_column_end') && !is_null($cellValue) && $cellValue != "" && ( (filter_var($cellValue, FILTER_VALIDATE_INT) === false ) || $cellValue <=0 ) ){
				$originalError = config('message.msg_schet_excel_error_004');
				$url .= '&err1=' . valueUrlEncode($originalError);
				return $url;
			}

			if( $index == config('system_const_schet.tosai_import_column_end')) {
				$this->mtosaiColEnd = $cellValue;
			}

			if( $index == config('system_const_schet.sogumi_day_row') && ( (filter_var($cellValue, FILTER_VALIDATE_INT) === false ) || $cellValue <=0 ) ){
				$originalError = config('message.msg_schet_excel_error_004');
				$url .= '&err1=' . valueUrlEncode($originalError);
				return $url;
			}

			if( $index == config('system_const_schet.sogumi_day_row') ){
				$this->msogumiDayRow = $cellValue;
			}

			if( $index == config('system_const_schet.sogumi_import_row_start') && ( (filter_var($cellValue, FILTER_VALIDATE_INT) === false ) || $cellValue <=0 ) ){
				$originalError = config('message.msg_schet_excel_error_004');
				$url .= '&err1=' . valueUrlEncode($originalError);
				return $url;
			}

			if( $index == config('system_const_schet.sogumi_import_row_start')) {
				$this->msogumiRowStart = $cellValue;
			}

			if( $index == config('system_const_schet.sogumi_import_row_end') && ( (filter_var($cellValue, FILTER_VALIDATE_INT) === false ) || $cellValue <=0 ) ){
				$originalError = config('message.msg_schet_excel_error_004');
				$url .= '&err1=' . valueUrlEncode($originalError);
				return $url;
			}

			if( $index == config('system_const_schet.sogumi_import_row_end')) {
				$this->msogumiRowEnd = $cellValue;
			}

			if( $index == config('system_const_schet.sogumi_import_column_start') && ( (filter_var($cellValue, FILTER_VALIDATE_INT) === false ) || $cellValue <=0 ) ){
				$originalError = config('message.msg_schet_excel_error_004');
				$url .= '&err1=' . valueUrlEncode($originalError);
				return $url;
			}

			if( $index == config('system_const_schet.sogumi_import_column_start')) {
				$this->msogumiColStart = $cellValue;
			}

			if( $index == config('system_const_schet.sogumi_import_column_end') && !is_null($cellValue) &&  $cellValue != "" && ( (filter_var($cellValue, FILTER_VALIDATE_INT) === false ) || $cellValue <=0 ) ){
				$originalError = config('message.msg_schet_excel_error_004');
				$url .= '&err1=' . valueUrlEncode($originalError);
				return $url;
			}

			if( $index == config('system_const_schet.sogumi_import_column_end')) {
				$this->msogumiColEnd = $cellValue;
			}

			//get value symbol from excel sheet setting
			$this->getTosaiSymbolExcelSetting($index, $cellValue);
			$this->getSogumiSymbolExcelSetting($index, $cellValue);

			$index ++;
		}

		//check 開始行より終了行の方が小さい
		if( $this->mtosaiRowEnd < $this->mtosaiRowStart || $this->msogumiRowEnd < $this->msogumiRowStart) {
			$originalError = config('message.msg_schet_excel_error_005');
			$url .= '&err1=' . valueUrlEncode($originalError);
			return $url;
		}

		//check 開始行より終了行の方が小さい and 開始列より終了列の方が小さい
		if (is_null($this->mtosaiColEnd) || $this->mtosaiColEnd == "" || is_null($this->msogumiColEnd) || $this->msogumiColEnd== "") {
			//read sheet data
			$sheetData  = $worksheetData[0];
			$reader->setLoadSheetsOnly($sheetData);
			$spreadsheet = $reader->load($inputFileName);
			$worksheet = $spreadsheet->getActiveSheet();
		}

		//終了列に値が入っていない場合は 日付行を検索して 終了列を取得しておくこと
		$this->mtosaiColEnd =  (is_null($this->mtosaiColEnd) ||  $this->mtosaiColEnd == "") ? $this->getColEndByRowDate($worksheet, $this->mtosaiDayRow, $this->mtosaiColStart) - 1  : $this->mtosaiColEnd ;
		$this->msogumiColEnd = (is_null($this->msogumiColEnd) || $this->msogumiColEnd== "") ? $this->getColEndByRowDate($worksheet, $this->msogumiDayRow, $this->msogumiColStart) - 1 : $this->msogumiColEnd ;
		if( $this->mtosaiColEnd < $this->mtosaiColStart || $this->msogumiColEnd < $this->msogumiColStart) {
			$originalError = config('message.msg_schet_excel_error_006');
			$url .= '&err1=' . valueUrlEncode($originalError);
			return $url;
		}

		return '';
	}

	/**
	 * function get tosai symbol from excel sheet setting
	 *
	 * @param index
	 * @param valueCell
	 * @return
	 *
	 * @create 2020/09/29 Cuong
	 * @update
	 */
	private function getTosaiSymbolExcelSetting($index, $valueCell){
		switch ($index) {
			case config('system_const_schet.tosai_kogumi_ps_mark'):
				array_push($this->marrSymbolTosai, $valueCell);
				break;

			case config('system_const_schet.tosai_kogumi_c_mark'):
				array_push($this->marrSymbolTosai, $valueCell);
				break;

			case config('system_const_schet.tosai_naicyu_ps_mark'):
				array_push($this->marrSymbolTosai, $valueCell);
				break;

			case config('system_const_schet.tosai_naicyu_c_mark'):
				array_push($this->marrSymbolTosai, $valueCell);
				break;

			case config('system_const_schet.tosai_ogumi_ps_mark'):
				array_push($this->marrSymbolTosai, $valueCell);
				break;

			case config('system_const_schet.tosai_ogumi_c_mark'):
				array_push($this->marrSymbolTosai, $valueCell);
				break;

			case config('system_const_schet.tosai_sogumi_ps_mark'):
				array_push($this->marrSymbolTosai, $valueCell);
				break;

			case config('system_const_schet.tosai_sogumi_c_mark'):
				array_push($this->marrSymbolTosai, $valueCell);
				break;

			default:
				break;
		}
	}

	/**
	* function get sogumi symbol from excel sheet setting
	*
	* @param index
	* @param valueCell
	* @return
	*
	* @create 2020/09/29 Cuong
	* @update
	*/
	private function getSogumiSymbolExcelSetting($index, $valueCell){
		switch ($index) {
			case config('system_const_schet.sogumi_kogumi_ps_mark'):
				array_push($this->marrSymbolSogumi, $valueCell);
				break;

			case config('system_const_schet.sogumi_kogumi_c_mark'):
				array_push($this->marrSymbolSogumi, $valueCell);
				break;

			case config('system_const_schet.sogumi_naicyu_ps_mark'):
				array_push($this->marrSymbolSogumi, $valueCell);
				break;

			case config('system_const_schet.sogumi_naicyu_c_mark'):
				array_push($this->marrSymbolSogumi, $valueCell);
				break;

			case config('system_const_schet.sogumi_ogumi_ps_mark'):
				array_push($this->marrSymbolSogumi, $valueCell);
				break;

			case config('system_const_schet.sogumi_ogumi_c_mark'):
				array_push($this->marrSymbolSogumi, $valueCell);
				break;

			case config('system_const_schet.sogumi_sogumi_ps_mark'):
				array_push($this->marrSymbolSogumi, $valueCell);
				break;

			case config('system_const_schet.sogumi_sogumi_c_mark'):
				array_push($this->marrSymbolSogumi, $valueCell);
				break;

			default:
				break;
		}
	}

	/**
	* function get Column End By Row Date
	*
	* @param worksheet
	* @param row
	* @param colStart
	* @return int
	*
	* @create 2020/10/01 Cuong
	* @update
	*/
	private function getColEndByRowDate($worksheet, $row, $colStart){
		$cellValue = $worksheet->getCellByColumnAndRow($colStart, $row)->getValue();
		if (!is_null($cellValue)) {
			$colStart++;
			return $this->getColEndByRowDate($worksheet, $row, $colStart);
		}
		return $colStart;
	}

	/**
	* function get tosai data
	*
	* @param worksheet
	* @param arrData
	* @return
	*
	* @create 2020/10/01 Cuong
	* @update
	*/
	private function getTosaiData($worksheet, &$arrData){
		for ($row = $this->mtosaiRowStart; $row <= $this->mtosaiRowEnd; ++$row) {
			for ($col = $this->mtosaiColStart; $col <= $this->mtosaiColEnd; ++$col) {
				$value = $worksheet->getCellByColumnAndRow($col, $row)->getValue();
				$this->processDataTosai($worksheet, $col, $row, $value , $arrData);
			}
		}
	}


	/**
	* function get sogumi data
	*
	* @param worksheet
	* @param arrDataSogumi
	* @param arrDataKyokyu
	* @return
	*
	* @create 2020/10/01 Cuong
	* @update
	*/
	private function getSogumiData($worksheet, &$arrDataSogumi, &$arrDataKyokyu){
		for ($row = $this->msogumiRowStart; $row <= $this->msogumiRowEnd; ++$row)
		{
			$colorFirst = null;
			$colorSeconds = null;
			$sDateColor = null;
			$eDateColor = null;

			$sFirstColorPositon = null;
			$eFirstColorPositon = null;
			$sSecondsColorPositon = null;
			$eSecondsColorPositon = null;

			$k_BlockNick = null;

			for ($col = $this->msogumiColStart; $col <= $this->msogumiColEnd; ++$col)
			{
				$value = $worksheet->getCellByColumnAndRow($col, $row)->getValue();
				$cellCoordinate = $worksheet->getCellByColumnAndRow($col, $row)->getCoordinate();
				$cellColor = $worksheet->getStyle($cellCoordinate)->getFill()->getStartColor()->getRGB();

				if($cellColor != 'FFFFFF' && (is_null($colorFirst))){
					$colorFirst = true;
					$sFirstColorPositon = $col;
					$sDateColor = $this->getSDate($worksheet, $col, $this->msogumiDayRow);
				}elseif($colorFirst === true && $cellColor == 'FFFFFF'){
					$colorFirst = false;
					$colorSeconds = false;
					$eFirstColorPositon = $col -1;
					$eDateColor = $this->getSDate($worksheet, $col-1, $this->msogumiDayRow);
				}

				if($colorSeconds === false && in_array($value, $this->marrSymbolSogumi)){
					$colorSeconds = true;
					$sSecondsColorPositon = $col;
				}
			}

			if(!is_null($sSecondsColorPositon)){
				$value = $worksheet->getCellByColumnAndRow($sSecondsColorPositon, $row)->getValue();
				if(in_array($value, $this->marrSymbolSogumi)){
					$k_BlockNick = $this->getBlockNickSogumi($worksheet, $sSecondsColorPositon, $row);
				}
				$this->processDataSogumi($worksheet, $sSecondsColorPositon, $row, $value, $sDateColor, $eDateColor, $arrDataSogumi);
			}

			for($col = $sFirstColorPositon; $col <= $eFirstColorPositon; ++$col){
				$value = $worksheet->getCellByColumnAndRow($col, $row)->getValue();
				$this->processDataKyokyu($worksheet, $col, $row, $value , $k_BlockNick, $arrDataKyokyu);
			}
		}
	}

	/**
	* function get prcess data tosai by symbol
	*
	* @param worksheet
	* @param col
	* @param row
	* @param value cell
	* @param arrData
	*
	* @create 2020/10/01 Cuong
	* @update
	*/
	private function processDataTosai($worksheet, $col, $row, $value , &$arrData){
		if(in_array($value, $this->marrSymbolTosai)){
			$arrTemp = array(
				'BlockNick' => $this->getBlockNickTosai($worksheet, $col, $row),
				'BlockKumiku' => $this->getBlockKumikuOfTosai($value),
				'K_BlockNick' => null,
				'K_BlockKumiku' => null,
				'Kind' => config('system_const_schet.import_kind_tosai'),
				'SDate' => $this->getSDate($worksheet, $col, $this->mtosaiDayRow),
				'EDate' => null,
				'Row' => $row,
				'Col' => $col,
			);
			array_push($arrData, $arrTemp);
		}
	}

	/**
	* function get process data Sogumi by symbol
	*
	* @param worksheet
	* @param col
	* @param row
	* @param value cell
	* @param sDateColor
	* @param eDateColor
	* @param arrData
	*
	* @create 2020/10/01 Cuong
	* @update
	*/
	private function processDataSogumi($worksheet, $col, $row, $value , $sDateColor, $eDateColor, &$arrData){
		if(in_array($value, $this->marrSymbolSogumi)){
			$arrTemp = array(
				'BlockNick' => $this->getBlockNickSogumi($worksheet, $col, $row),
				'BlockKumiku' => config('system_const.kumiku_code_sogumi'),
				'K_BlockNick' => null,
				'K_BlockKumiku' => null,
				'Kind' => config('system_const_schet.import_kind_sogumi'),
				'SDate' => $sDateColor,
				'EDate' => $eDateColor,
				'Row' => $row,
				'Col' => null,
			);
			array_push($arrData, $arrTemp);
		}
	}

	/**
	* function process get data kyokyu by symbol
	*
	*
	* @param worksheet
	* @param col
	* @param row
	* @param value cell
	* @param string k_BlockNick
	* @param arrData
	*
	* @create 2020/10/01 Cuong
	* @update
	*/
	private function processDataKyokyu($worksheet, $col, $row, $value, $k_BlockNick, &$arrData){
		if(in_array($value, $this->marrSymbolSogumi)){
			$arrTemp = array(
				'BlockNick' => $this->getBlockNickSogumi($worksheet, $col, $row),
				'BlockKumiku' => $this->getBlockKumiku($value),
				'K_BlockNick' => $k_BlockNick,
				'K_BlockKumiku' => config('system_const.kumiku_code_sogumi'),
				'Kind' => config('system_const_schet.import_kind_kyokyu'),
				'SDate' => $this->getSDate($worksheet, $col, $this->msogumiDayRow),
				'EDate' => null,
				'Row' => $row,
				'Col' => $col,
			);
			array_push($arrData, $arrTemp);
		}
	}

	/**
	* function get BlockKumiku by symbol
	*
	* @param value
	* @return string
	*
	* @create 2020/10/01 Cuong
	* @update
	*/
	private function getBlockKumiku($value){
		if($value == $this->marrSymbolSogumi[0]) { return config('system_const.kumiku_code_kogumi'); };
		if($value == $this->marrSymbolSogumi[1]) { return config('system_const.kumiku_code_kogumi'); };
		if($value == $this->marrSymbolSogumi[2]) { return config('system_const.kumiku_code_naicyu'); };
		if($value == $this->marrSymbolSogumi[3]) { return config('system_const.kumiku_code_naicyu'); };
		if($value == $this->marrSymbolSogumi[4]) { return config('system_const.kumiku_code_ogumi'); };
		if($value == $this->marrSymbolSogumi[5]) { return config('system_const.kumiku_code_ogumi'); };
		if($value == $this->marrSymbolSogumi[6]) { return config('system_const.kumiku_code_sogumi'); };
		if($value == $this->marrSymbolSogumi[7]) { return config('system_const.kumiku_code_sogumi'); };
	}

	/**
	* function get BlockKumiku by symbol of Tosai
	*
	* @param value
	* @return string
	*
	* @create 2021/02/04 Cuong
	* @update
	*/
	private function getBlockKumikuOfTosai($value){
		if($value == $this->marrSymbolTosai[0]) { return config('system_const.kumiku_code_kogumi'); };
		if($value == $this->marrSymbolTosai[1]) { return config('system_const.kumiku_code_kogumi'); };
		if($value == $this->marrSymbolTosai[2]) { return config('system_const.kumiku_code_naicyu'); };
		if($value == $this->marrSymbolTosai[3]) { return config('system_const.kumiku_code_naicyu'); };
		if($value == $this->marrSymbolTosai[4]) { return config('system_const.kumiku_code_ogumi'); };
		if($value == $this->marrSymbolTosai[5]) { return config('system_const.kumiku_code_ogumi'); };
		if($value == $this->marrSymbolTosai[6]) { return config('system_const.kumiku_code_sogumi'); };
		if($value == $this->marrSymbolTosai[7]) { return config('system_const.kumiku_code_sogumi'); };
	}

	/**
	* function get blocknick by symbol of tosai
	*
	* @param worksheet
	* @param col
	* @param row
	* @return mix
	*
	* @create 2020/10/01 Cuong
	* @update
	*/
	private function getBlockNickTosai($worksheet, $col, $row){
		// above the symbol
		$aboveSymbol = $worksheet->getCellByColumnAndRow($col, $row-1)->getValue();
		if(!is_null($aboveSymbol) && !in_array($aboveSymbol, $this->marrSymbolTosai)){
			return $blockNick = strtoupper(mb_convert_kana($aboveSymbol, "KVa"));
		}

		// below the symbol
		$belowSymbol = $worksheet->getCellByColumnAndRow($col, $row+1)->getValue();
		if(!is_null($belowSymbol) && !in_array($belowSymbol, $this->marrSymbolTosai)){
			return $blockNick = strtoupper(mb_convert_kana($belowSymbol, "KVa"));
		}

		// left the symbol
		$leftSymbol = $worksheet->getCellByColumnAndRow($col-1, $row)->getValue();
		if(!is_null($leftSymbol) && !in_array($leftSymbol, $this->marrSymbolTosai)){
			return $blockNick = strtoupper(mb_convert_kana($leftSymbol, "KVa"));
		}

		// right the symbol
		$rightSymbol = $worksheet->getCellByColumnAndRow($col+1, $row)->getValue();
		if(!is_null($rightSymbol) && !in_array($rightSymbol, $this->marrSymbolTosai)){
			return $blockNick = strtoupper(mb_convert_kana($rightSymbol, "KVa"));
		}

		return null;
	}

	/**
	* function get blocknick by symbol of sogumi
	*
	* @param worksheet
	* @param col
	* @param row
	* @return mix
	*
	* @create 2020/10/01 Cuong
	* @update
	*/
	private function getBlockNickSogumi($worksheet, $col, $row){
		// above the symbol
		$aboveSymbol = $worksheet->getCellByColumnAndRow($col, $row-1)->getValue();
		if(!is_null($aboveSymbol) && !in_array($aboveSymbol, $this->marrSymbolSogumi)){
			return $blockNick = strtoupper(mb_convert_kana($aboveSymbol, "KVa"));
		}

		// below the symbol
		$belowSymbol = $worksheet->getCellByColumnAndRow($col, $row+1)->getValue();
		if(!is_null($belowSymbol) && !in_array($belowSymbol, $this->marrSymbolSogumi)){
			return $blockNick = strtoupper(mb_convert_kana($belowSymbol, "KVa"));
		}

		// left the symbol
		$leftSymbol = $worksheet->getCellByColumnAndRow($col-1, $row)->getValue();
		if(!is_null($leftSymbol) && !in_array($leftSymbol, $this->marrSymbolSogumi)){
			return $blockNick = strtoupper(mb_convert_kana($leftSymbol, "KVa"));
		}

		// right the symbol
		$rightSymbol = $worksheet->getCellByColumnAndRow($col+1, $row)->getValue();
		if(!is_null($rightSymbol) && !in_array($rightSymbol, $this->marrSymbolSogumi)){
			return $blockNick = strtoupper(mb_convert_kana($rightSymbol, "KVa"));
		}

		return null;
	}

	/**
	* function get sDate of Block
	*
	* @param worksheet
	* @param col
	* @param rowDay
	* @return mix
	*
	* @create 2020/10/01 Cuong
	* @update
	*/
	private function getSDate($worksheet, $col, $rowDay){
		$sDate = $worksheet->getCellByColumnAndRow($col, $rowDay)->getValue();
		if(is_null($sDate)) {
			return null;
		}

		if(is_numeric($sDate)) {
			return \PhpOffice\PhpSpreadsheet\Shared\Date::excelToDateTimeObject($sDate)->format('Y/m/d');
		}

		return $sDate;
	}

	/**
	* function check err of data excel
	*
	* @param array dataToSai
	* @param array dataKyokyu
	* @param array dataSogumi
	* @param Request request
	* @param menuInfo
	*
	* @return string
	*
	* @create 2020/10/01 Cuong
	* @update
	*/
	private function checkErrDataExcel($dataToSai, $dataKyokyu, $dataSogumi, $request, $menuInfo, $importID){
		$flagErr = false;
		$hasDataErr = false;
		$dataErr = array();
		/* Check data tosai */
		if($this->checkDataTosai($dataToSai, $request, $menuInfo, $dataErr)) {
			$flagErr = true;
		}

		/* Check data Kyokyu */
		if($this->checkDataKyokyu($dataKyokyu, $dataSogumi, $request, $menuInfo, $dataErr)) {
			$flagErr = true;
		}
		/* Check data Sogumi */
		if($this->checkDataSogumi($dataSogumi, $dataToSai, $request, $menuInfo, $dataErr)) {
			$flagErr = true;
		}

		/* The block name and kumiku 、 are many in the dataTosai and dataKyokyu groups*/
		$arrData = array_merge($dataToSai, $dataKyokyu);
		foreach($arrData as $data) {
			if(!is_null($data['BlockNick']) && !$this->checkFormatBlockName($data['BlockNick'])) {
				$blockNames = $this->convertBlockName($data['BlockNick']);
				foreach($blockNames as $blockName) {
					$hasData= array_filter($arrData, function ($item) use ($data,$blockName) {
						if(!is_null($item['BlockNick']) && !$this->checkFormatBlockName($item['BlockNick'])) {
							$xblockNames = $this->convertBlockName($item['BlockNick']);
							return $item['BlockKumiku'] == $data['BlockKumiku'] && in_array($blockName, $xblockNames);
						}
					});

					if(count($hasData) > 1) {
						$messageErr = config('message.msg_schet_excel_error_015');
						$data['messageErr'] = $messageErr;
						array_push($dataErr, array($data));
						$flagErr = true;
					}
				}
			}
		}

		if($flagErr) {
			//insert data error to T_ImportData
			$res = $this->insertImportDataErr($request, $dataErr, $menuInfo, $importID);
			if($res != 1) {
				return $res;
			}
			$hasDataErr = true;
		}
		return $hasDataErr;
	}

	/**
	* function check err ブロック名の略称→正式変換方法
	*
	* @param string blockName
	* @return boolean
	*
	* @create 2020/10/01 Cuong
	* @update
	*/
	private function checkFormatBlockName($blockName){
		$messageErr = '';
		$blockNameSplit = explode(" ", $blockName);
		$flag = false;

		if(preg_match('/[^A-Za-z0-9\s]+/', $blockName)) {
			return $flag = true;
		}

		if (!in_array(count($blockNameSplit), [3,4])) {
			return $flag = true;
		}

		if (strlen($blockNameSplit[0]) != 2) {
			return $flag = true;
		}

		if (!in_array(strlen($blockNameSplit[1]), [1,2,3])) {
			return $flag = true;
		}

		if (!in_array(strlen(preg_replace("/[^0-9]/","",$blockNameSplit[1])), [1,2])) {
			return $flag = true;
		}

		if (count($blockNameSplit) == 4) {
			if(!in_array(strlen($blockNameSplit[2]), [1,2,3,4])){
				return $flag = true;
			}
		}

		$gen = null;
		if (count($blockNameSplit) == 4) {
			$gen = $blockNameSplit[3];
		}
		if (count($blockNameSplit) == 3) {
			$gen = $blockNameSplit[2];
		}

		if (!in_array(strlen($gen),[1,2])) {
			return $flag = true;
		}

		if (strlen($gen) == 1 && !in_array($gen, ['P','S','C'])) {
			return $flag = true;
		}

		if (strlen($gen) == 2 && $gen != 'PS') {
			return $flag = true;
		}

		return $flag;
	}

	/**
	* function ブロック名の略称→正式変換方法
	*
	* @param string blockName
	* @return array
	*
	* @create 2020/10/01 Cuong
	* @update
	*/
	private function convertBlockName($blockName){
		$arrBlockNameFull = array();
		$blockNameFull = '';

		$blockNameSplit = explode(" ", $blockName);
		$blockNameFull .= $blockNameSplit[0] . ' ';

		if(strlen($blockNameSplit[1]) == 1){
			$blockNameFull .= str_pad($blockNameSplit[1], 3, " ", STR_PAD_BOTH) . " ";
		}

		if(strlen($blockNameSplit[1]) == 2 && strlen(preg_replace("/[^0-9]/","",$blockNameSplit[1])) == 2){
			$blockNameFull .= str_pad($blockNameSplit[1], 3, " ", STR_PAD_RIGHT) . " ";
		}

		if(strlen($blockNameSplit[1]) == 2 && strlen(preg_replace("/[^0-9]/","",$blockNameSplit[1])) != 2){
			$blockNameFull .= str_pad($blockNameSplit[1], 3, " ", STR_PAD_LEFT) . " ";
		}
		if(strlen($blockNameSplit[1]) == 3){
			$blockNameFull .= $blockNameSplit[1] . " ";
		}

		if (count($blockNameSplit) == 4) {
			$blockNameFull .= str_pad($blockNameSplit[2], 4, " ", STR_PAD_RIGHT) . " ";
		}

		if (count($blockNameSplit) == 3) {
			$blockNameFull = str_pad($blockNameFull, 11, " ", STR_PAD_RIGHT) . " ";
		}

		$gen = null;
		if (count($blockNameSplit) == 4) {
			$gen = $blockNameSplit[3];
		}
		if (count($blockNameSplit) == 3) {
			$gen = $blockNameSplit[2];
		}

		if(strlen($gen) == 2){
			array_push($arrBlockNameFull,$blockNameFull . $gen[0]);
			array_push($arrBlockNameFull,$blockNameFull . $gen[1]);
		}else{
			$blockNameFull .= $gen;
			array_push($arrBlockNameFull,$blockNameFull);
		}

		return $arrBlockNameFull;
	}

	const EXCEPT_CHAR_INDEX = 13;

	/**
	* function split string
	*
	* @param text
	* @param indexExcept
	* @param width
	* @return mixed
	*
	* @create 2020/10/01 Cuong
	* @update
	*/
	private function GetTextExceptCharIndex($text, $indexExcept, $width = 1) {
		$lenght = strlen($text);
		$index = $indexExcept - $width;
		$textExceptChar = $lenght > $index ? substr($text, 0, $index) : '';

		return $textExceptChar;
	}

	/**
	* function cut character
	*
	* @param text
	* @param indexExcept
	* @param width
	* @return mixed
	*
	* @create 2020/10/01 Cuong
	* @update
	*/
	private function GetTextCharIndex($text, $indexGet, $width = 1) {
		$lenght = strlen($text);
		$index = $indexGet - $width;
		$textChar = $lenght > $index ? substr($text, $index, $width) : '';

		return $textChar;
	}

	/**
	* function check is date
	*
	* @param date
	* @return mixed
	*
	* @create 2020/10/01 Cuong
	* @update 2020/11/20 Cuong update condition
	*/
	private function isRealDate($strDate) {
		$blnFlag = true;
		$arrTmpDate = explode('/', $strDate);
		if(count($arrTmpDate) == 3) {
			$arrCheckkHasHis = explode(' ', $arrTmpDate[2]);
			if(count($arrCheckkHasHis) == 1) {
				if(!ctype_digit($arrTmpDate[0]) || !ctype_digit($arrTmpDate[1]) || !ctype_digit($arrTmpDate[2])) {
					$blnFlag = false;
				} else {
					preg_match('/([1-9]\d{3}\/([1-9]|0[1-9]|1[0-2])\/([1-9]|0[1-9]|[12]\d|3[01]))/', $strDate, $arrCheckFormatDate);
					if(count($arrCheckFormatDate) == 0) {
						$blnFlag = false;
					} else {
						$blnCheckDate = checkdate($arrTmpDate[1], $arrTmpDate[2], $arrTmpDate[0]);
						if(!$blnCheckDate) {
							$blnFlag = false;
						}
					}
				}
			}
		} else {
			$blnFlag = false;
		}
		return $blnFlag;
	}

	/**
	* insert data to T_ImportData when has error.
	*
	* @param Request request
	* @param array data
	* @param string messageErr
	* @param menuInfo
	* @return mixed
	*
	* @create 2020/10/05 Cuong
	* @update 2020/11/19 Cuong
	*/
	private function insertImportDataErr($request, $data, $menuInfo, $importID) {
		try {
			/* Delete data old than 24h */
			$this->deleteDataImportOld24h($menuInfo);

			/* Process Insert data */
			DB::transaction(function () use($request, $data, $importID, $menuInfo) {
				foreach ($data as $key => $value) {
					$objImportData = new T_ImportData;
					$objImportData->ID = $key + 1;
					$objImportData->ImportID = $importID;
					$objImportData->UserID = $menuInfo->UserID;
					$objImportData->BlockNick =	is_null($value[0]['BlockNick']) ? null : $value[0]['BlockNick'];
					if(!is_null($value[0]['BlockNick'])) {
						if($this->checkFormatBlockName($value[0]['BlockNick'])) {
							$objImportData->BlockName = null;
						}else {
							$arrBlockNickFull = $this->convertBlockName($value[0]['BlockNick']);
							$objImportData->BlockName =	$this->GetTextExceptCharIndex($arrBlockNickFull[0], self::EXCEPT_CHAR_INDEX);
						}
					}
					$objImportData->BlockKumiku = $value[0]['BlockKumiku'];
					$objImportData->K_BlockNick = is_null($value[0]['K_BlockNick']) ? null : $value[0]['K_BlockNick'];
					if(!is_null($value[0]['K_BlockNick'])) {
						if($this->checkFormatBlockName($value[0]['K_BlockNick'])) {
							$objImportData->K_BlockName = null;
						}else {
							$arrKBlockNickFull = $this->convertBlockName($value[0]['K_BlockNick']);
							$objImportData->K_BlockName = $this->GetTextExceptCharIndex($arrKBlockNickFull[0], self::EXCEPT_CHAR_INDEX);
						}
					}
					$objImportData->K_BlockKumiku = is_null($value[0]['K_BlockKumiku']) ? null : $value[0]['K_BlockKumiku'];
					$objImportData->Kind = $value[0]['Kind'];
					$objImportData->SDate = (is_null($value[0]['SDate']) || !$this->isRealDate($value[0]['SDate'])) ? null : $value[0]['SDate'];
					$objImportData->EDate = (is_null($value[0]['EDate']) || !$this->isRealDate($value[0]['EDate'])) ? null : $value[0]['EDate'];
					$objImportData->Sdate_P = null;
					$objImportData->Edate_P = null;
					$objImportData->Sdate_S = null;
					$objImportData->Edate_S = null;
					$objImportData->Sdate_C = null;
					$objImportData->Edate_C = null;
					$objImportData->WorkItemID_P = null;
					$objImportData->WorkItemID_S = null;
					$objImportData->WorkItemID_C = null;
					$objImportData->K_BlockName_P = null;
					$objImportData->K_BlockKumiku_P = null;
					$objImportData->K_BlockName_S = null;
					$objImportData->K_BlockKumiku_S = null;
					$objImportData->K_BlockName_C = null;
					$objImportData->K_BlockKumiku_C = null;
					$objImportData->ImportFlag = config('system_const_schet.import_flag_err');
					$objImportData->ModifyFlag = null;
					$objImportData->Message = $value[0]['messageErr'];
					$objImportData->Row = is_null($value[0]['Row']) ? null : $value[0]['Row'];
					$objImportData->Col = is_null($value[0]['Col']) ? null : $value[0]['Col'];
					$objImportData->save();
				}
			});
		} catch (Exception $e) {
			$url = url('/');
			$url .= '/' . $menuInfo->KindURL;
			$url .= '/' . $menuInfo->MenuURL;
			$url .= '/index';
			$url .= '?cmn1=' . valueUrlEncode($menuInfo->KindID);
			$url .= '&cmn2=' . valueUrlEncode($menuInfo->MenuID);
			$url .= '&val1=' . valueUrlEncode($request->val1);
			$url .= '&val2=' . valueUrlEncode($request->val2);
			$url .= '&val5=' . valueUrlEncode($request->val5);
			$url .= '&err1=' . valueUrlEncode(config('message.msg_cmn_db_016'));
			return $url;
		}

		return 1;
	}

	/**
	* insert data to T_ImportData .
	*
	* @param array dataNew
	* @param array dataDelete
	* @param array dataChange
	* @param menuInfo
	* @return
	*
	* @create 2020/10/26 Cuong
	* @update 2020/11/19 Cuong additional prameter
	*/
	private function insertImportData($dataNew, $dataDelete, $dataChange, $importID, $menuInfo, $request) {
		try {
			/* Delete data old than 24h */
			$this->deleteDataImportOld24h($menuInfo);

			/* Process Insert data */
			DB::transaction(function () use($dataNew, $dataDelete, $dataChange, $importID, $menuInfo) {
				$index = 0;
				if(count($dataNew) > 0) {
					foreach ($dataNew as $key => $value) {
						$this->insertNew($importID, $index, $value, $menuInfo);
						$index++;
					}
				}

				if(count($dataDelete) > 0) {
					foreach ($dataDelete as $key => $value) {
						$this->insertDelete($importID, $index, $value, $menuInfo);
						$index++;
					}
				}

				if(count($dataChange) > 0) {
					foreach ($dataChange as $key => $value) {
						$this->insertChange($importID, $index, $value, $menuInfo);
						$index++;
					}
				}
			});
		} catch (Exception $e) {
			$url = url('/');
			$url .= '/' . $menuInfo->KindURL;
			$url .= '/' . $menuInfo->MenuURL;
			$url .= '/index';
			$url .= '?cmn1=' . valueUrlEncode($menuInfo->KindID);
			$url .= '&cmn2=' . valueUrlEncode($menuInfo->MenuID);
			$url .= '&val1=' . valueUrlEncode($request->val1);
			$url .= '&val2=' . valueUrlEncode($request->val2);
			$url .= '&val5=' . valueUrlEncode($request->val5);
			$url .= '&err1=' . valueUrlEncode(config('message.msg_cmn_db_016'));
			return $url;
		}
		return 1;
	}

	/**
	* function insert data new.
	*
	* @param int importID
	* @param int lastID
	* @param array data
	* @param menuInfo
	* @return
	*
	* @create 2020/10/26 Cuong
	* @update
	*/
	private function insertNew($importID, $lastID, $data, $menuInfo){

		$objImportData = new T_ImportData;
		$objImportData->ID = $lastID + 1;
		$objImportData->ImportID = $importID;
		$objImportData->UserID = $menuInfo->UserID;
		$objImportData->BlockNick =	is_null($data[0]['BlockNick']) ? null : $data[0]['BlockNick'];

		$arrBlockNickFull = $this->convertBlockName($data[0]['BlockNick']);
		$objImportData->BlockName =	 $this->GetTextExceptCharIndex($arrBlockNickFull[0], self::EXCEPT_CHAR_INDEX);

		$objImportData->BlockKumiku = $data[0]['BlockKumiku'];
		$objImportData->K_BlockNick = is_null($data[0]['K_BlockNick']) ? null : $data[0]['K_BlockNick'];

		if(is_null($data[0]['K_BlockNick'])) {
			$objImportData->K_BlockName = null;
		}else {
			$arrKBlockNickFull = $this->convertBlockName($data[0]['K_BlockNick']);
			$objImportData->K_BlockName = $this->GetTextExceptCharIndex($arrKBlockNickFull[0], self::EXCEPT_CHAR_INDEX);
		}

		$objImportData->K_BlockKumiku = is_null($data[0]['K_BlockKumiku']) ? null : $data[0]['K_BlockKumiku'];

		$objImportData->Kind = $data[0]['Kind'];
		$objImportData->SDate = is_null($data[0]['SDate']) ? null : $data[0]['SDate'];
		$objImportData->EDate = is_null($data[0]['EDate']) ? null : $data[0]['EDate'];
		$objImportData->Sdate_P = null;
		$objImportData->Edate_P = null;
		$objImportData->Sdate_S = null;
		$objImportData->Edate_S = null;
		$objImportData->Sdate_C = null;
		$objImportData->Edate_C = null;
		$objImportData->WorkItemID_P = null;
		$objImportData->WorkItemID_S = null;
		$objImportData->WorkItemID_C = null;
		$objImportData->K_BlockName_P = null;
		$objImportData->K_BlockKumiku_P = null;
		$objImportData->K_BlockName_S = null;
		$objImportData->K_BlockKumiku_S = null;
		$objImportData->K_BlockName_C = null;
		$objImportData->K_BlockKumiku_C = null;
		$objImportData->ImportFlag = config('system_const_schet.import_flag_add');
		$objImportData->ModifyFlag = null;
		$objImportData->Message = null;
		$objImportData->Row = is_null($data[0]['Row']) ? null : $data[0]['Row'];
		$objImportData->Col = is_null($data[0]['Col']) ? null : $data[0]['Col'];
		$objImportData->save();
	}

	/**
	* function insert data delete.
	*
	* @param int importID
	* @param int lastID
	* @param array data
	* @param menuInfo
	* @return
	*
	* @create 2020/10/26 Cuong
	* @update
	*/
	private function insertDelete($importID, $lastID, $data, $menuInfo){
		$objImportData = new T_ImportData;
		$objImportData->ID = $lastID + 1;
		$objImportData->ImportID = $importID;
		$objImportData->UserID = $menuInfo->UserID;
		$objImportData->BlockNick =	$data[0]['BlockName'];
		$objImportData->BlockName =	 $this->GetTextExceptCharIndex($data[0]['BlockName'], self::EXCEPT_CHAR_INDEX);
		$objImportData->BlockKumiku = $data[0]['BlockKumiku'];
		$objImportData->K_BlockNick = null;
		$objImportData->K_BlockName = null;
		$objImportData->K_BlockKumiku = null;
		$objImportData->Kind = $data[0]['Kind'];
		$objImportData->SDate = null;
		$objImportData->EDate = null;
		$objImportData->Sdate_P = null;
		$objImportData->Edate_P = null;
		$objImportData->Sdate_S = null;
		$objImportData->Edate_S = null;
		$objImportData->Sdate_C = null;
		$objImportData->Edate_C = null;
		$objImportData->WorkItemID_P = null;
		$objImportData->WorkItemID_S = null;
		$objImportData->WorkItemID_C = null;
		$objImportData->K_BlockName_P = null;
		$objImportData->K_BlockKumiku_P = null;
		$objImportData->K_BlockName_S = null;
		$objImportData->K_BlockKumiku_S = null;
		$objImportData->K_BlockName_C = null;
		$objImportData->K_BlockKumiku_C = null;
		$objImportData->ImportFlag = config('system_const_schet.import_flag_del');
		$objImportData->ModifyFlag = null;
		$objImportData->Message = null;
		$objImportData->Row = null;
		$objImportData->Col = null;
		$objImportData->save();
	}

	/**
	* function insert data delete.
	*
	* @param int importID
	* @param int lastID
	* @param array data
	* @param menuInfo
	* @return
	*
	* @create 2020/10/26 Cuong
	* @update
	*/
	private function insertChange($importID, $lastID, $data, $menuInfo) {
		$objImportData = new T_ImportData;
		$objImportData->ID = $lastID + 1;
		$objImportData->ImportID = $importID;
		$objImportData->UserID = $menuInfo->UserID;
		$objImportData->BlockNick =	$data[0]['BlockNick'];

		$arrBlockNickFull = $this->convertBlockName($data[0]['BlockNick']);
		$objImportData->BlockName =	 $this->GetTextExceptCharIndex($arrBlockNickFull[0], self::EXCEPT_CHAR_INDEX);

		$objImportData->BlockKumiku = $data[0]['BlockKumiku'];
		$objImportData->K_BlockNick = is_null($data[0]['K_BlockNick']) ? null : $data[0]['K_BlockNick'];

		if(is_null($data[0]['K_BlockNick'])) {
			$objImportData->K_BlockName = null;
		}else {
			$arrKBlockNickFull = $this->convertBlockName($data[0]['K_BlockNick']);
			$objImportData->K_BlockName = $this->GetTextExceptCharIndex($arrKBlockNickFull[0], self::EXCEPT_CHAR_INDEX);
		}

		$objImportData->K_BlockKumiku = is_null($data[0]['K_BlockKumiku']) ? null : $data[0]['K_BlockKumiku'];

		$objImportData->Kind = $data[0]['Kind'];

		$objImportData->SDate = !array_key_exists('SDate',$data[0]) ? null : $data[0]['SDate'];
		$objImportData->EDate = !array_key_exists('EDate',$data[0]) ? null : $data[0]['EDate'];
		$objImportData->Sdate_P = !array_key_exists('Sdate_P',$data[0]) ? null : $data[0]['Sdate_P'];
		$objImportData->Edate_P = !array_key_exists('Edate_P',$data[0]) ? null : $data[0]['Edate_P'];
		$objImportData->Sdate_S = !array_key_exists('Sdate_S',$data[0]) ? null : $data[0]['Sdate_S'];
		$objImportData->Edate_S = !array_key_exists('Edate_S',$data[0]) ? null : $data[0]['Edate_S'];
		$objImportData->Sdate_C = !array_key_exists('Sdate_C',$data[0]) ? null : $data[0]['Sdate_C'];
		$objImportData->Edate_C = !array_key_exists('Edate_C',$data[0]) ? null : $data[0]['Edate_C'];
		$objImportData->WorkItemID_P = !array_key_exists('WorkItemID_P',$data[0]) ? null : $data[0]['WorkItemID_P'];
		$objImportData->WorkItemID_S = !array_key_exists('WorkItemID_S',$data[0]) ? null : $data[0]['WorkItemID_S'];
		$objImportData->WorkItemID_C = !array_key_exists('WorkItemID_C',$data[0]) ? null : $data[0]['WorkItemID_C'];
		$objImportData->K_BlockName_P = !array_key_exists('K_BlockName_P',$data[0]) ? null : $data[0]['K_BlockName_P'];
		$objImportData->K_BlockKumiku_P = !array_key_exists('K_BlockKumiku_P',$data[0]) ? null : $data[0]['K_BlockKumiku_P'];
		$objImportData->K_BlockName_S = !array_key_exists('K_BlockName_S',$data[0]) ? null : $data[0]['K_BlockName_S'];
		$objImportData->K_BlockKumiku_S = !array_key_exists('K_BlockKumiku_S',$data[0]) ? null : $data[0]['K_BlockKumiku_S'];
		$objImportData->K_BlockName_C = !array_key_exists('K_BlockName_C',$data[0]) ? null : $data[0]['K_BlockName_C'];
		$objImportData->K_BlockKumiku_C = !array_key_exists('K_BlockKumiku_C',$data[0]) ? null : $data[0]['K_BlockKumiku_C'];
		$objImportData->ImportFlag = $data[0]['ImportFlag'];
		$objImportData->ModifyFlag = !array_key_exists('ModifyFlag',$data[0]) ? null : $data[0]['ModifyFlag'];
		$objImportData->Message = null;
		$objImportData->Row = $data[0]['Row'];
		$objImportData->Col = $data[0]['Col'];
		$objImportData->save();
	}

	/**
	* function check validate data tosai
	*
	* @param array arrData
	* @param Request request
	* @param menuInfo
	* @return boolean
	*
	* @create 2020/10/05 Cuong
	* @update
	*/
	private function checkDataTosai($dataTosais, $request, $menuInfo, &$dataErr){
		$res = false;
		$messageErr = '';
		foreach ($dataTosais as $key => $data) {
			/* Check BlockNick */
			if (is_null($data['BlockNick'])) {		//Check blocknick null
				$messageErr = config('message.msg_schet_excel_error_007');
				$data['messageErr'] = $messageErr;
				array_push($dataErr, array($data));
				$res = true;
			}else {
				if ($this->checkFormatBlockName($data['BlockNick'])) {		//Check format blocknick
					$messageErr = config('message.msg_schet_excel_error_008');
					$data['messageErr'] = $messageErr;
					array_push($dataErr, array($data));
					$res = true;
				}else {
					if (preg_match('/[^A-Za-z0-9\s]+/', $data['BlockNick'])) {		//記号が含まれている場合
						$messageErr = config('message.msg_schet_excel_error_018');
						$data['messageErr'] = $messageErr;
						array_push($dataErr, array($data));
						$res = true;
					}else {

						/* 略称から正式なブロック名に変換した時の舷以外と、組区が同じものの中で、舷PまたはSがある時に、舷Cがある場合 */
						$arrayGen = array();
						$arrBlockName = $this->convertBlockName($data['BlockNick']);

						//split block name
						$blockNameNotGen = $this->GetTextExceptCharIndex($arrBlockName[0], self::EXCEPT_CHAR_INDEX);
						$tosais = array_filter($dataTosais, function ($item) use ($blockNameNotGen, $data) {
							if(!is_null($item['BlockNick']) && !$this->checkFormatBlockName($item['BlockNick'])) {
								$arrBlockName1 = $this->convertBlockName($item['BlockNick']);
								$blockNameNotGen1 = $this->GetTextExceptCharIndex($arrBlockName1[0], self::EXCEPT_CHAR_INDEX);
								return $blockNameNotGen == $blockNameNotGen1 && $data['BlockKumiku'] == $item['BlockKumiku'];
							}
						});

						if(count($tosais) > 0){
							foreach ($tosais as $tosai) {
								//get Gen in block name
								$arrBlockNameGr = $this->convertBlockName($tosai['BlockNick']);
								foreach($arrBlockNameGr as $blockNameFull) {
									$genTosai = $this->GetTextCharIndex($blockNameFull, self::EXCEPT_CHAR_INDEX);
									array_push($arrayGen, $genTosai);
								}
							}
						}

						if(in_array('C',$arrayGen) && (in_array('P',$arrayGen) || in_array('S',$arrayGen))) {
							$messageErr = config('message.msg_schet_excel_error_009');
							$data['messageErr'] = $messageErr;
							array_push($dataErr, array($data));
							$res = true;
						}

					}
				}

			}

			/* Check SDate */
			if(is_null($data['SDate'])) {
				$messageErr = config('message.msg_schet_excel_error_010');
				$data['messageErr'] = $messageErr;
				array_push($dataErr, array($data));
				$res = true;
				continue;
			}elseif (!$this->isRealDate($data['SDate'])) {
				$messageErr = config('message.msg_schet_excel_error_011');
				$data['messageErr'] = $messageErr;
				array_push($dataErr, array($data));
				$res = true;
				continue;
			}
		}

		return $res;
	}

	/**
	* function check validate data Kyokyu
	*
	* @param array arrData
	* @param array dataSogumi
	* @param Request request
	* @param menuInfo
	* @return boolean
	*
	* @create 2020/10/05 Cuong
	* @update
	*/
	private function checkDataKyokyu($dataKyokyus, $dataSogumi, $request, $menuInfo, &$dataErr){
		$res = false;
		$messageErr = '';
		foreach ($dataKyokyus as $key => $data) {
			/* Check BlockNick */
			if (is_null($data['BlockNick'])) {		//Check blocknick null
				$messageErr = config('message.msg_schet_excel_error_007');
				$data['messageErr'] = $messageErr;
				array_push($dataErr, array($data));
				$res = true;
			}else {
				if ($this->checkFormatBlockName($data['BlockNick'])) {	 //Check format blocknick
					$messageErr = config('message.msg_schet_excel_error_008');
					$data['messageErr'] = $messageErr;
					array_push($dataErr, array($data));
					$res = true;
				}else {
					if (preg_match('/[^A-Za-z0-9\s]+/', $data['BlockNick'])) {	//記号が含まれている場合
						$messageErr = config('message.msg_schet_excel_error_018');
						$data['messageErr'] = $messageErr;
						array_push($dataErr, array($data));
						$res = true;
					}else {

						/* 略称から正式なブロック名に変換した時の舷以外と、組区が同じものの中で、舷PまたはSがある時に、舷Cがある場合 */
						$arrayGen = array();
						$arrBlockName = $this->convertBlockName($data['BlockNick']);

						//split block name
						$blockNameNotGen = $this->GetTextExceptCharIndex($arrBlockName[0], self::EXCEPT_CHAR_INDEX);
						$kyokyus = array_filter($dataKyokyus, function ($item) use ($blockNameNotGen, $data) {
							if(!is_null($item['BlockNick']) && !$this->checkFormatBlockName($item['BlockNick'])) {
								$arrBlockName1 = $this->convertBlockName($item['BlockNick']);
								$blockNameNotGen1 = $this->GetTextExceptCharIndex($arrBlockName1[0], self::EXCEPT_CHAR_INDEX);
								return $blockNameNotGen1 == $blockNameNotGen && $data['BlockKumiku'] == $item['BlockKumiku'];
							}
						});

						if(count($kyokyus) > 0){
							foreach ($kyokyus as $kyokyu) {
								//get Gen in block name
								$blockNameFull_Kyokyus = $this->convertBlockName($kyokyu['BlockNick']);
								foreach($blockNameFull_Kyokyus as $blockNameFull) {
									$genKyokyu = $this->GetTextCharIndex($blockNameFull, self::EXCEPT_CHAR_INDEX);
									array_push($arrayGen, $genKyokyu);
								}
							}
						}

						if(in_array('C',$arrayGen) && (in_array('P',$arrayGen) || in_array('S',$arrayGen))) {
							$messageErr = config('message.msg_schet_excel_error_009');
							$data['messageErr'] = $messageErr;
							array_push($dataErr, array($data));
							$res = true;
						}
					}
				}
			}

			// Check SDate
			if(is_null($data['SDate'])) {
				$messageErr = config('message.msg_schet_excel_error_010');
				$data['messageErr'] = $messageErr;
				array_push($dataErr, array($data));
				$res = true;
			}elseif (!$this->isRealDate($data['SDate'])) {
				$messageErr = config('message.msg_schet_excel_error_011');
				$data['messageErr'] = $messageErr;
				array_push($dataErr, array($data));
				$res = true;
			}

			/* Check K_BlockNick */
			if(is_null($data['K_BlockNick'])) {		//Check k_blocknick null
				$messageErr = config('message.msg_schet_excel_error_012');
				$data['messageErr'] = $messageErr;
				array_push($dataErr, array($data));
				$res = true;
			}elseif($this->checkFormatBlockName($data['K_BlockNick'])) { 	//Check format k_blocknick
				$messageErr = config('message.msg_schet_excel_error_013');
				$data['messageErr'] = $messageErr;
				array_push($dataErr, array($data));
				$res = true;
			}else {
				/* 略称から正式なブロック名に変換した時、同じ正式ブロック名と組区の組み合わせが、[取得データ-総組日程]の中にない場合  */
				//convert K_BlockNick vd: BC 31 P => BC 31       P
				$arrBlockName = $this->convertBlockName($data['K_BlockNick']);

				foreach($arrBlockName as $blockName) {
					$sogumis= array_filter($dataSogumi, function ($item) use ($data,$blockName) {
						if(!is_null($item['BlockNick']) && !$this->checkFormatBlockName($item['BlockNick'])) {
							$arrBlockName1 = $this->convertBlockName($item['BlockNick']);
							return $item['BlockKumiku'] == $data['K_BlockKumiku'] && in_array($blockName, $arrBlockName1);
						}
					});

					if(count($sogumis) == 0) {
						$messageErr = config('message.msg_schet_excel_error_014');
						$data['messageErr'] = $messageErr;
						array_push($dataErr, array($data));
						$res = true;
					}
				}
			}
		}
		return $res;
	}

	/**
	* function check validate data sogumi
	*
	* @param array arrData
	* @param array dataTosai
	* @param Request request
	* @param menuInfo
	* @return boolean
	*
	* @create 2020/10/05 Cuong
	* @update
	*/
	private function checkDataSogumi($dataSogumis, $dataTosai, $request, $menuInfo, &$dataErr){
		$res = false;
		$messageErr = '';
		foreach ($dataSogumis as $key => $data) {
			/* Check BlockNick */
			if (is_null($data['BlockNick'])) {		//Check blocknick null
				$messageErr = config('message.msg_schet_excel_error_007');
				$data['messageErr'] = $messageErr;
				array_push($dataErr, array($data));
				$res = true;
			}else {
				if ($this->checkFormatBlockName($data['BlockNick'])) {		 //Check format blocknick
					$messageErr = config('message.msg_schet_excel_error_008');
					$data['messageErr'] = $messageErr;
					array_push($dataErr, array($data));
					$res = true;
				}else {
					if (preg_match('/[^A-Za-z0-9\s]+/', $data['BlockNick'])) {		//記号が含まれている場合
						$messageErr = config('message.msg_schet_excel_error_018');
						$data['messageErr'] = $messageErr;
						array_push($dataErr, array($data));
						$res = true;
					}else {
						//-------------------
						$arrayGen = array();
						$arrBlockName = $this->convertBlockName($data['BlockNick']);

						/* 略称から正式なブロック名に変換した時の舷以外と、組区が同じものの中で、舷PまたはSがある時に、舷Cがある場合 */
						//split block name
						$blockNameNotGen = $this->GetTextExceptCharIndex($arrBlockName[0], self::EXCEPT_CHAR_INDEX);
						$sogumis = array_filter($dataSogumis, function ($item) use ($blockNameNotGen, $data) {
							if(!is_null($item['BlockNick']) && !$this->checkFormatBlockName($item['BlockNick'])) {
								$arrBlockName1 = $this->convertBlockName($item['BlockNick']);
								$blockNameNotGen1= $this->GetTextExceptCharIndex($arrBlockName1[0], self::EXCEPT_CHAR_INDEX);
								return $blockNameNotGen1 == $blockNameNotGen && $data['BlockKumiku'] == $item['BlockKumiku'];
							}
						});

						if(count($sogumis) > 0){
							foreach ($sogumis as $sogumi) {
								//get Gen in block name
								$arrBlockNameGr = $this->convertBlockName($sogumi['BlockNick']);
								foreach($arrBlockNameGr as $blockNameFull) {
									$genTosai = $this->GetTextCharIndex($blockNameFull, self::EXCEPT_CHAR_INDEX);
									array_push($arrayGen, $genTosai);
								}
							}
						}

						if(in_array('C',$arrayGen) && (in_array('P',$arrayGen) || in_array('S',$arrayGen))) {
							$messageErr = config('message.msg_schet_excel_error_009');
							$data['messageErr'] = $messageErr;
							array_push($dataErr, array($data));
							$res = true;
						}
						//-------------------

						foreach($arrBlockName as $blockName) {
							/* 略称から正式なブロック名に変換した時、同じ正式ブロック名と組区の組み合わせが、[取得データ-総組日程]の中に複数ある場合 */
							$arrSogumi = array_filter($dataSogumis, function ($item) use ($data,$blockName) {
								if(!is_null($item['BlockNick']) && !$this->checkFormatBlockName($item['BlockNick'])) {
									$arrBlockName1 = $this->convertBlockName($item['BlockNick']);
									return $item['BlockKumiku'] == $data['BlockKumiku'] && in_array($blockName, $arrBlockName1);
								}
							});

							if(count($arrSogumi) > 1) {
								$messageErr = config('message.msg_schet_excel_error_015');
								$data['messageErr'] = $messageErr;
								array_push($dataErr, array($data));
								$res = true;
							}

							/* 略称から正式なブロック名に変換した時、同じ正式ブロック名と組区の組み合わせが、[取得データ-搭載日程]の中にない場合 */
							$tosais= array_filter($dataTosai, function ($item) use ($data,$blockName) {
								if(!is_null($item['BlockNick']) && !$this->checkFormatBlockName($item['BlockNick'])) {
									$xblockNames = $this->convertBlockName($item['BlockNick']);
									return $item['BlockKumiku'] == $data['BlockKumiku'] && in_array($blockName, $xblockNames);
								}
							});

							if(count($tosais) == 0) {
								$messageErr = config('message.msg_schet_excel_error_016');
								$data['messageErr'] = $messageErr;
								array_push($dataErr, array($data));
								$res = true;
							}
						}
					}
				}
			}

			/* Check SDate */
			if(is_null($data['SDate'])) {
				$messageErr = config('message.msg_schet_excel_error_010');
				$data['messageErr'] = $messageErr;
				array_push($dataErr, array($data));
				$res = true;
			}elseif (!$this->isRealDate($data['SDate'])) {
				$messageErr = config('message.msg_schet_excel_error_011');
				$data['messageErr'] = $messageErr;
				array_push($dataErr, array($data));
				$res = true;
			}

			/* Check EDate */
			if(is_null($data['EDate'])) {
				$messageErr = config('message.msg_schet_excel_error_010');
				$data['messageErr'] = $messageErr;
				array_push($dataErr, array($data));
				$res = true;
			}elseif (!$this->isRealDate($data['EDate'])) {
				$messageErr = config('message.msg_schet_excel_error_011');
				$data['messageErr'] = $messageErr;
				array_push($dataErr, array($data));
				$res = true;
			}
		}
		return $res;
	}

	/**
	* function delete data old than 24h of T_Import
	*
	* @param
	* @return
	*
	* @create 2020/10/05 Cuong
	* @update 2020/10/26 Cuong update condition delete
	*/
	private function deleteDataImportOld24h($menuInfo) {
		$dateNow = DB::selectOne('SELECT getdate() AS sysdate')->sysdate;
		$datetime = new Carbon($dateNow);
		$dateOld = $datetime->subHours(24);
		$tes = $dateOld->toDateTimeString();
		$result = T_ImportData::where('UserID', '=', $menuInfo->UserID)
							->where('Updated_at', '<', $dateOld->toDateTimeString())->delete();
	}

	/**
	* function 新規データの取得
	*
	* @param array dataOlds is data in database
	* @param array dataExcels
	* @param menuInfo
	* @return
	*
	* @create 2020/10/08 Cuong
	* @update
	*/
	private function processDataNew($dataOlds, $dataExcels, $menuInfo, &$dataInsert) {
		if(count($dataExcels) == 0) {
			// if data excel not data;
			return;
		}

		foreach ($dataExcels as $dataExcel) {
			$arrBlockName = $this->convertBlockName($dataExcel['BlockNick']);
			$blockNameNotGen = $this->GetTextExceptCharIndex($arrBlockName[0], self::EXCEPT_CHAR_INDEX);

			if(count($dataOlds) == 0) {
				array_push($dataInsert, array($dataExcel));
			}else {
				$dataNew = array_filter($dataOlds, function($item) use($blockNameNotGen, $dataExcel){
					return $blockNameNotGen == $item['BlockNameNotGen'] && $dataExcel['BlockKumiku'] == $item['BlockKumiku'];
				});

				if(count($dataNew) == 0) {
					array_push($dataInsert, array($dataExcel));
				}
			}
		}
	}

	/**
	* function 削除データの取得
	*
	* @param array dataOlds is data in database
	* @param array dataExcels
	* @param menuInfo
	* @return
	*
	* @create 2020/10/08 Cuong
	* @update
	*/
	private function processDataDelete($dataOlds, $dataExcels, $menuInfo, $kind, &$dataInsert) {
		if(count($dataOlds) == 0) {
			// if dataOlds not data;
			return;
		}

		foreach ($dataOlds as $data) {
			if(count($dataExcels) == 0) {
				$data['Kind'] = $kind;
				array_push($dataInsert, array($data));
			}else {
				$dataNew = array_filter($dataExcels, function($item) use($data){
					$arrBlockNameExcel = $this->convertBlockName($item['BlockNick']);
					$blockNameNotGenExcel = $this->GetTextExceptCharIndex($arrBlockNameExcel[0], self::EXCEPT_CHAR_INDEX);
					return $data['BlockNameNotGen'] == $blockNameNotGenExcel
							&& $data['BlockKumiku'] == $item['BlockKumiku'];
				});

				if(count($dataNew) == 0) {
					$data['Kind'] = $kind;
					array_push($dataInsert, array($data));
				}
			}
		}
	}

	/**
	* function 新規、削除以外のデータの取得
	*
	* @param array datas
	* @param array dataExcels
	* @param array resultDataTimeTracker
	* @param menuInfo
	* @return
	*
	* @create 2020/10/08 Cuong
	* @update 2020/11/21 Cuong update method
	*/
	private function processDataChange($grDataOlds, $grDataExcels, $resultDataTimeTracker, $menuInfo, &$dataInsert) {
		foreach ($grDataOlds as $keyGroup=>$dataOlds) {
			if(array_key_exists($keyGroup, $grDataExcels)) {
				$isGenPS = false;
				$dataExcels = $grDataExcels[$keyGroup];
				if(count($dataExcels) == 1) {
					$genBlockNicks = $this->getGenFromBlockNick(reset($dataExcels)['BlockNick']);
					if(count($genBlockNicks) == 2) {
						$isGenPS = true;
					}
				}
				//Case: data old has  1 Gen - Excel has 2 Gen . (ex: P->PS, S->PS, C->PS, P->P-S, S->P-S, C->P-S ).
				if( count($dataOlds) == 1 && (count($dataExcels) == 2 || $isGenPS) ) {
					foreach($dataOlds as $key => $dataOld) {	//loop dataOlds
						$genOld = $dataOld['Gen'];		//gen dataOld
						// get data excel has gen same genOld
						$arrDataSameGen = array_filter($dataExcels, function($dataExcel) use($genOld){
							$genBlockNick = $this->getGenFromBlockNick($dataExcel['BlockNick']);
							return in_array($genOld, $genBlockNick);
						});

						if(count($arrDataSameGen) > 0) {
							// Has Gen Same
							if($isGenPS) {
								// case: dataexcel has gen blocknick is: PS
								foreach ($dataExcels as $key1 => $excel) {
									$dataExcelTemp = $excel;
									$sdate = null;
									$edate = null;
									if($resultDataTimeTracker[$key]['errflag'] != -1) {
										$sdate = $resultDataTimeTracker[$key]['sdate'];
										$edate = $resultDataTimeTracker[$key]['edate'];
									}

									if($resultDataTimeTracker[$key]['errflag'] == -1) {
										$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_moderr');
									}

									$dataExcelTemp['WorkItemID_'.$genOld] = $dataOld['WorkItemID'];
									$dataExcelTemp['ModifyFlag'] = config('system_const_schet.modifyflag_gen');
									if(!(array_key_exists('ImportFlag',$dataExcelTemp) && $dataExcelTemp['ImportFlag'] == config('system_const_schet.import_flag_moderr'))) {
										$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_mod');
									}

									if(!is_null($sdate)) {
										$dataExcelTemp['Sdate_'.$genOld] = $sdate;
									}

									if(!is_null($edate)) {
										$dataExcelTemp['Edate_'.$genOld] = $edate;
									}

									if(!is_null($sdate) && $dataExcelTemp['SDate'] != $sdate) {
										$dataExcelTemp['ModifyFlag'] = config('system_const_schet.modifyflag_date').config('system_const_schet.modifyflag_gen');
									}

									if($dataExcelTemp['Kind'] == config('system_const_schet.import_kind_sogumi')) {
										if(!is_null($edate) && $dataExcelTemp['EDate'] != $edate) {
											$dataExcelTemp['ModifyFlag'] = config('system_const_schet.modifyflag_date').config('system_const_schet.modifyflag_gen');
										}
									}

									if($dataExcelTemp['Kind'] == config('system_const_schet.import_kind_kyokyu')) {

										$dataExcelTemp['K_BlockName_'.$genOld] = $dataOld['K_BlockName'];
										$dataExcelTemp['K_BlockKumiku_'.$genOld] = $dataOld['K_BlockKumiku'];

										$arrKBlockNameExcel = $this->convertBlockName($dataExcelTemp['K_BlockNick']);
										$kBlockNameNotGenExcel = $this->GetTextExceptCharIndex($arrKBlockNameExcel[0], self::EXCEPT_CHAR_INDEX);
										if($kBlockNameNotGenExcel != $dataOld['K_BlockNameNotGen']
										|| $dataExcelTemp['K_BlockKumiku'] != $dataOld['K_BlockKumiku']) {
											$dataExcelTemp['ModifyFlag'] = !array_key_exists('ModifyFlag',$dataExcelTemp) ?
											config('system_const_schet.modifyflag_kyokyu') :
											$dataExcelTemp['ModifyFlag'] .config('system_const_schet.modifyflag_kyokyu');

											if(!(array_key_exists('ImportFlag',$dataExcelTemp) && $dataExcelTemp['ImportFlag'] == config('system_const_schet.import_flag_moderr'))) {
												$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_mod');
											}
										}
									}

									if(!array_key_exists('ImportFlag',$dataExcelTemp)) {
										$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_ok');
									}
									array_push($dataInsert, array($dataExcelTemp));
								}
							}else {
								// Case: data excel has: Gen P and Gen S
								foreach ($dataExcels as $key1 => $excel) {
									$dataExcelTemp = $excel;
									if(in_array($excel, $arrDataSameGen)) {
										// process excel has gen same
										$sdate = null;
										$edate = null;
										if($resultDataTimeTracker[$key]['errflag'] != -1) {
											$sdate = $resultDataTimeTracker[$key]['sdate'];
											$edate = $resultDataTimeTracker[$key]['edate'];
										}

										if($resultDataTimeTracker[$key]['errflag'] == -1) {
											$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_moderr');
										}

										$dataExcelTemp['WorkItemID_'.$genOld] = $dataOld['WorkItemID'];


										if(!is_null($sdate)) {
											$dataExcelTemp['Sdate_'.$genOld] = $sdate;
										}

										if(!is_null($edate)) {
											$dataExcelTemp['Edate_'.$genOld] = $edate;
										}

										if(!is_null($sdate) && $dataExcelTemp['SDate'] != $sdate) {
											$dataExcelTemp['ModifyFlag'] = config('system_const_schet.modifyflag_date');
											if(!(array_key_exists('ImportFlag',$dataExcelTemp) && $dataExcelTemp['ImportFlag'] == config('system_const_schet.import_flag_moderr'))) {
												$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_mod');
											}
										}

										if($dataExcelTemp['Kind'] == config('system_const_schet.import_kind_sogumi')) {
											if(!is_null($edate) && $dataExcelTemp['EDate'] != $edate) {
												$dataExcelTemp['ModifyFlag'] = config('system_const_schet.modifyflag_date');
												if(!(array_key_exists('ImportFlag',$dataExcelTemp) && $dataExcelTemp['ImportFlag'] == config('system_const_schet.import_flag_moderr'))) {
													$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_mod');
												}
											}
										}

										if($dataExcelTemp['Kind'] == config('system_const_schet.import_kind_kyokyu')) {

											$dataExcelTemp['K_BlockName_'.$genOld] = $dataOld['K_BlockName'];
											$dataExcelTemp['K_BlockKumiku_'.$genOld] = $dataOld['K_BlockKumiku'];

											$arrKBlockNameExcel = $this->convertBlockName($dataExcelTemp['K_BlockNick']);
											$kBlockNameNotGenExcel = $this->GetTextExceptCharIndex($arrKBlockNameExcel[0], self::EXCEPT_CHAR_INDEX);
											if($kBlockNameNotGenExcel != $dataOld['K_BlockNameNotGen']
											|| $dataExcelTemp['K_BlockKumiku'] != $dataOld['K_BlockKumiku']) {
												$dataExcelTemp['ModifyFlag'] = !array_key_exists('ModifyFlag',$dataExcelTemp) ?
												config('system_const_schet.modifyflag_kyokyu') :
												$dataExcelTemp['ModifyFlag'] .config('system_const_schet.modifyflag_kyokyu');

												if(!(array_key_exists('ImportFlag',$dataExcelTemp) && $dataExcelTemp['ImportFlag'] == config('system_const_schet.import_flag_moderr'))) {
													$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_mod');
												}
											}
										}

										if(!array_key_exists('ImportFlag',$dataExcelTemp)) {
											$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_ok');
										}
										array_push($dataInsert, array($dataExcelTemp));
									}else {
										// process excel has not gen same
										$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_mod');
										$dataExcelTemp['ModifyFlag'] = config('system_const_schet.modifyflag_gen');
										array_push($dataInsert, array($dataExcelTemp));
									}
								}
							}
						}else {
							// data olds has not Gen same in data excel: (C->PS, C->P-S)
							foreach ($dataExcels as $key1 => $excel) {
								$dataExcelTemp = $excel;
								$sdate = null;
								$edate = null;
								if($resultDataTimeTracker[$key]['errflag'] != -1) {
									$sdate = $resultDataTimeTracker[$key]['sdate'];
									$edate = $resultDataTimeTracker[$key]['edate'];
								}

								if($resultDataTimeTracker[$key]['errflag'] == -1) {
									$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_moderr');
								}

								$dataExcelTemp['ModifyFlag'] = config('system_const_schet.modifyflag_gen');
								if(!(array_key_exists('ImportFlag',$dataExcelTemp) && $dataExcelTemp['ImportFlag'] == config('system_const_schet.import_flag_moderr'))) {
									$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_mod');
								}
								$dataExcelTemp['WorkItemID_'.$genOld] = $dataOld['WorkItemID'];


								if(!is_null($sdate)) {
									$dataExcelTemp['Sdate_'.$genOld] = $sdate;
								}

								if(!is_null($edate)) {
									$dataExcelTemp['Edate_'.$genOld] = $edate;
								}

								if(!is_null($sdate) && $dataExcelTemp['SDate'] != $sdate) {
									$dataExcelTemp['ModifyFlag'] = config('system_const_schet.modifyflag_date').config('system_const_schet.modifyflag_gen');
									if(!(array_key_exists('ImportFlag',$dataExcelTemp) && $dataExcelTemp['ImportFlag'] == config('system_const_schet.import_flag_moderr'))) {
										$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_mod');
									}
								}

								if($dataExcelTemp['Kind'] == config('system_const_schet.import_kind_sogumi')) {
									if(!is_null($edate) && $dataExcelTemp['EDate'] != $edate) {
										if(array_key_exists('ModifyFlag',$dataExcelTemp)) {
											$pos = strpos($dataExcelTemp['ModifyFlag'], config('system_const_schet.modifyflag_date'));
											if($pos === false) {
												// modifyflag_date not exists in $dataExcelTemp['ModifyFlag']
												$dataExcelTemp['ModifyFlag'] = config('system_const_schet.modifyflag_date').$dataExcelTemp['ModifyFlag'];
											}
										}else {
											$dataExcelTemp['ModifyFlag'] = config('system_const_schet.modifyflag_date').config('system_const_schet.modifyflag_gen');
										}

										if(!(array_key_exists('ImportFlag',$dataExcelTemp) && $dataExcelTemp['ImportFlag'] == config('system_const_schet.import_flag_moderr'))) {
											$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_mod');
										}
									}
								}

								if($dataExcelTemp['Kind'] == config('system_const_schet.import_kind_kyokyu')) {

									$dataExcelTemp['K_BlockName_'.$genOld] = $dataOld['K_BlockName'];
									$dataExcelTemp['K_BlockKumiku_'.$genOld] = $dataOld['K_BlockKumiku'];

									$arrKBlockNameExcel = $this->convertBlockName($dataExcelTemp['K_BlockNick']);
									$kBlockNameNotGenExcel = $this->GetTextExceptCharIndex($arrKBlockNameExcel[0], self::EXCEPT_CHAR_INDEX);
									if($kBlockNameNotGenExcel != $dataOld['K_BlockNameNotGen']
									|| $dataExcelTemp['K_BlockKumiku'] != $dataOld['K_BlockKumiku']) {
										$dataExcelTemp['ModifyFlag'] = !array_key_exists('ModifyFlag',$dataExcelTemp) ?
										config('system_const_schet.modifyflag_kyokyu') :
										$dataExcelTemp['ModifyFlag'] .config('system_const_schet.modifyflag_kyokyu');

										if(!(array_key_exists('ImportFlag',$dataExcelTemp) && $dataExcelTemp['ImportFlag'] == config('system_const_schet.import_flag_moderr'))) {
											$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_mod');
										}
									}
								}

								if(!array_key_exists('ImportFlag',$dataExcelTemp)) {
									$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_ok');
								}
								array_push($dataInsert, array($dataExcelTemp));
							}
						}
					}
				}

				// Case: data olds has 2 Gen - data Excel has 1 Gen: (P-S->P, P-S->S, P-S->C);
				if( count($dataOlds) == 2 && (count($dataExcels) == 1) && !$isGenPS ) {
					foreach($dataExcels as $key => $dataExcel) {
						$dataExcelTemp = $dataExcel;
						foreach ($dataOlds as $key => $old) {
							$genOld = $old['Gen'];
							$sdate = null;
							$edate = null;
							if($resultDataTimeTracker[$key]['errflag'] != -1) {
								$sdate = $resultDataTimeTracker[$key]['sdate'];
								$edate = $resultDataTimeTracker[$key]['edate'];
							}

							if($resultDataTimeTracker[$key]['errflag'] == -1) {
								$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_moderr');
							}

							$dataExcelTemp['WorkItemID_'.$genOld] = $old['WorkItemID'];
							if(!array_key_exists('ModifyFlag',$dataExcelTemp)) {
								$dataExcelTemp['ModifyFlag'] = config('system_const_schet.modifyflag_gen');
							}

							if(!is_null($sdate)) {
								$dataExcelTemp['Sdate_'.$genOld] = $sdate;
							}

							if(!is_null($edate)) {
								$dataExcelTemp['Edate_'.$genOld] = $edate;
							}

							if(!is_null($sdate) && $dataExcelTemp['SDate'] != $sdate) {
								$dataExcelTemp['ModifyFlag'] = config('system_const_schet.modifyflag_date').config('system_const_schet.modifyflag_gen');
								if(!(array_key_exists('ImportFlag',$dataExcelTemp) && $dataExcelTemp['ImportFlag'] == config('system_const_schet.import_flag_moderr'))) {
									$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_mod');
								}
							}

							if($dataExcelTemp['Kind'] == config('system_const_schet.import_kind_sogumi')) {
								if(!is_null($edate) && $dataExcelTemp['EDate'] != $edate) {
									$dataExcelTemp['ModifyFlag'] = config('system_const_schet.modifyflag_date').config('system_const_schet.modifyflag_gen');
									if(!(array_key_exists('ImportFlag',$dataExcelTemp) && $dataExcelTemp['ImportFlag'] == config('system_const_schet.import_flag_moderr'))) {
										$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_mod');
									}
								}
							}

							if($dataExcelTemp['Kind'] == config('system_const_schet.import_kind_kyokyu')) {

								$dataExcelTemp['K_BlockName_'.$genOld] = $old['K_BlockName'];
								$dataExcelTemp['K_BlockKumiku_'.$genOld] = $old['K_BlockKumiku'];

								$arrKBlockNameExcel = $this->convertBlockName($dataExcelTemp['K_BlockNick']);
								$kBlockNameNotGenExcel = $this->GetTextExceptCharIndex($arrKBlockNameExcel[0], self::EXCEPT_CHAR_INDEX);
								if($kBlockNameNotGenExcel != $old['K_BlockNameNotGen']
								|| $dataExcelTemp['K_BlockKumiku'] != $old['K_BlockKumiku']) {
									$dataExcelTemp['ModifyFlag'] = !array_key_exists('ModifyFlag',$dataExcelTemp) ?
									config('system_const_schet.modifyflag_kyokyu') :
									$dataExcelTemp['ModifyFlag'] .config('system_const_schet.modifyflag_kyokyu');

									if(!(array_key_exists('ImportFlag',$dataExcelTemp) && $dataExcelTemp['ImportFlag'] == config('system_const_schet.import_flag_moderr'))) {
										$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_mod');
									}
								}
							}

							if(!array_key_exists('ImportFlag',$dataExcelTemp)) {
								$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_ok');
							}
						}
						array_push($dataInsert, array($dataExcelTemp));
					}
				}

				// Case: data Excel has 1 Gen - data olds has 1 Gen
				if( count($dataOlds) == 1 && (count($dataExcels) == 1) && !$isGenPS) {
					foreach($dataExcels as $key => $dataExcel) {
						$dataExcelTemp = $dataExcel;
						$arrGenExcel = $this->getGenFromBlockNick($dataExcel['BlockNick']);
						foreach ($dataOlds as $key => $old) {
							$genOld = $old['Gen'];
							$sdate = null;
							$edate = null;
							if($resultDataTimeTracker[$key]['errflag'] != -1) {
								$sdate = $resultDataTimeTracker[$key]['sdate'];
								$edate = $resultDataTimeTracker[$key]['edate'];
							}

							if($resultDataTimeTracker[$key]['errflag'] == -1) {
								$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_moderr');
							}

							$dataExcelTemp['WorkItemID_'.$genOld] = $old['WorkItemID'];
							if(!in_array($old['Gen'], $arrGenExcel)) {
								$dataExcelTemp['ModifyFlag'] = config('system_const_schet.modifyflag_gen');
								if(!(array_key_exists('ImportFlag',$dataExcelTemp) && $dataExcelTemp['ImportFlag'] == config('system_const_schet.import_flag_moderr'))) {
									$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_mod');
								}
							}

							if(!is_null($sdate)) {
								$dataExcelTemp['Sdate_'.$genOld] = $sdate;
							}

							if(!is_null($edate)) {
								$dataExcelTemp['Edate_'.$genOld] = $edate;
							}

							if(!is_null($sdate) && $dataExcelTemp['SDate'] != $sdate) {
								$dataExcelTemp['ModifyFlag'] = array_key_exists('ModifyFlag',$dataExcelTemp) ?
								config('system_const_schet.modifyflag_date').$dataExcelTemp['ModifyFlag'] : config('system_const_schet.modifyflag_date');

								if(!(array_key_exists('ImportFlag',$dataExcelTemp) && $dataExcelTemp['ImportFlag'] == config('system_const_schet.import_flag_moderr'))) {
									$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_mod');
								}
							}

							if($dataExcelTemp['Kind'] == config('system_const_schet.import_kind_sogumi')) {
								if(!is_null($edate) && $dataExcelTemp['EDate'] != $edate) {
									if(array_key_exists('ModifyFlag',$dataExcelTemp)) {
										$pos = strpos($dataExcelTemp['ModifyFlag'], config('system_const_schet.modifyflag_date'));
										if($pos === false) {
											// modifyflag_date not exists in $dataExcelTemp['ModifyFlag']
											$dataExcelTemp['ModifyFlag'] = config('system_const_schet.modifyflag_date').$dataExcelTemp['ModifyFlag'];
										}
									}else {
										$dataExcelTemp['ModifyFlag'] = config('system_const_schet.modifyflag_date');
									}

									if(!(array_key_exists('ImportFlag',$dataExcelTemp) && $dataExcelTemp['ImportFlag'] == config('system_const_schet.import_flag_moderr'))) {
										$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_mod');
									}
								}
							}

							if($dataExcelTemp['Kind'] == config('system_const_schet.import_kind_kyokyu')) {

								$dataExcelTemp['K_BlockName_'.$genOld] = $old['K_BlockName'];
								$dataExcelTemp['K_BlockKumiku_'.$genOld] = $old['K_BlockKumiku'];

								$arrKBlockNameExcel = $this->convertBlockName($dataExcelTemp['K_BlockNick']);
								$kBlockNameNotGenExcel = $this->GetTextExceptCharIndex($arrKBlockNameExcel[0], self::EXCEPT_CHAR_INDEX);
								if($kBlockNameNotGenExcel != $old['K_BlockNameNotGen']
								|| $dataExcelTemp['K_BlockKumiku'] != $old['K_BlockKumiku']) {
									$dataExcelTemp['ModifyFlag'] = !array_key_exists('ModifyFlag',$dataExcelTemp) ?
									config('system_const_schet.modifyflag_kyokyu') :
									$dataExcelTemp['ModifyFlag'] .config('system_const_schet.modifyflag_kyokyu');

									if(!(array_key_exists('ImportFlag',$dataExcelTemp) && $dataExcelTemp['ImportFlag'] == config('system_const_schet.import_flag_moderr'))) {
										$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_mod');
									}
								}
							}

							if(!array_key_exists('ImportFlag',$dataExcelTemp)) {
								$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_ok');
							}
						}
						array_push($dataInsert, array($dataExcelTemp));
					}
				}

				//Case: data Excel has 2 Gen - data olds has 2 Gen (ex: PS ->P-S, P-S -> P-S)
				if( count($dataOlds) == 2 && (count($dataExcels) == 2 || $isGenPS) ) {
					if($isGenPS) {  //(PS ->P-S)
						foreach($dataExcels as $key => $dataExcel) {
							$dataExcelTemp = $dataExcel;
							foreach ($dataOlds as $key => $old) {
								$genOld = $old['Gen'];
								$sdate = null;
								$edate = null;
								if($resultDataTimeTracker[$key]['errflag'] != -1) {
									$sdate = $resultDataTimeTracker[$key]['sdate'];
									$edate = $resultDataTimeTracker[$key]['edate'];
								}

								if($resultDataTimeTracker[$key]['errflag'] == -1) {
									$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_moderr');
								}

								$dataExcelTemp['WorkItemID_'.$genOld] = $old['WorkItemID'];

								if(!is_null($sdate)) {
									$dataExcelTemp['Sdate_'.$genOld] = $sdate;
								}

								if(!is_null($edate)) {
									$dataExcelTemp['Edate_'.$genOld] = $edate;
								}

								if(!is_null($sdate) && $dataExcelTemp['SDate'] != $sdate) {
									if(array_key_exists('ModifyFlag',$dataExcelTemp)) {
										$pos = strpos($dataExcelTemp['ModifyFlag'], config('system_const_schet.modifyflag_date'));
										if($pos === false) {
											// modifyflag_date not exists in $dataExcelTemp['ModifyFlag']
											$dataExcelTemp['ModifyFlag'] = config('system_const_schet.modifyflag_date').$dataExcelTemp['ModifyFlag'];
										}
									}else {
										$dataExcelTemp['ModifyFlag'] = config('system_const_schet.modifyflag_date');
									}

									if(!(array_key_exists('ImportFlag',$dataExcelTemp) && $dataExcelTemp['ImportFlag'] == config('system_const_schet.import_flag_moderr'))) {
										$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_mod');
									}
								}

								if($dataExcelTemp['Kind'] == config('system_const_schet.import_kind_sogumi')) {
									if(!is_null($edate) && $dataExcelTemp['EDate'] != $edate) {
										if(array_key_exists('ModifyFlag',$dataExcelTemp)) {
											$pos = strpos($dataExcelTemp['ModifyFlag'], config('system_const_schet.modifyflag_date'));
											if($pos === false) {
												// modifyflag_date not exists in $dataExcelTemp['ModifyFlag']
												$dataExcelTemp['ModifyFlag'] = config('system_const_schet.modifyflag_date').$dataExcelTemp['ModifyFlag'];
											}
										}else {
											$dataExcelTemp['ModifyFlag'] = config('system_const_schet.modifyflag_date');
										}

										if(!(array_key_exists('ImportFlag',$dataExcelTemp) && $dataExcelTemp['ImportFlag'] == config('system_const_schet.import_flag_moderr'))) {
											$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_mod');
										}
									}
								}

								if($dataExcelTemp['Kind'] == config('system_const_schet.import_kind_kyokyu')) {

									$dataExcelTemp['K_BlockName_'.$genOld] = $old['K_BlockName'];
									$dataExcelTemp['K_BlockKumiku_'.$genOld] = $old['K_BlockKumiku'];

									$arrKBlockNameExcel = $this->convertBlockName($dataExcelTemp['K_BlockNick']);
									$kBlockNameNotGenExcel = $this->GetTextExceptCharIndex($arrKBlockNameExcel[0], self::EXCEPT_CHAR_INDEX);
									if($kBlockNameNotGenExcel != $old['K_BlockNameNotGen']
									|| $dataExcelTemp['K_BlockKumiku'] != $old['K_BlockKumiku']) {
										$dataExcelTemp['ModifyFlag'] = !array_key_exists('ModifyFlag',$dataExcelTemp) ?
										config('system_const_schet.modifyflag_kyokyu') :
										$dataExcelTemp['ModifyFlag'] .config('system_const_schet.modifyflag_kyokyu');

										if(!(array_key_exists('ImportFlag',$dataExcelTemp) && $dataExcelTemp['ImportFlag'] == config('system_const_schet.import_flag_moderr'))) {
											$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_mod');
										}
									}
								}

								if(!array_key_exists('ImportFlag',$dataExcelTemp)) {
									$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_ok');
								}
							}
							array_push($dataInsert, array($dataExcelTemp));
						}
					}else {
						// (ex: P-S -> P-S)
						foreach($dataOlds as $key => $dataOld) {
							$genOld = $dataOld['Gen'];
							$arrDataSameGen = array_filter($dataExcels, function($dataExcel) use($genOld){
								$genBlockNick = $this->getGenFromBlockNick($dataExcel['BlockNick']);
								return in_array($genOld, $genBlockNick);
							});

							if(count($arrDataSameGen) > 0) {
								//has gen same
								foreach ($arrDataSameGen as $key1 => $excel) {
									$dataExcelTemp = $excel;
									$sdate = null;
									$edate = null;
									if($resultDataTimeTracker[$key]['errflag'] != -1) {
										$sdate = $resultDataTimeTracker[$key]['sdate'];
										$edate = $resultDataTimeTracker[$key]['edate'];
									}

									if($resultDataTimeTracker[$key]['errflag'] == -1) {
										$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_moderr');
									}

									$dataExcelTemp['WorkItemID_'.$genOld] = $dataOld['WorkItemID'];


									if(!is_null($sdate)) {
										$dataExcelTemp['Sdate_'.$genOld] = $sdate;
									}

									if(!is_null($edate)) {
										$dataExcelTemp['Edate_'.$genOld] = $edate;
									}

									if(!is_null($sdate) && $dataExcelTemp['SDate'] != $sdate) {
										$dataExcelTemp['ModifyFlag'] = config('system_const_schet.modifyflag_date');
										if(!(array_key_exists('ImportFlag',$dataExcelTemp) && $dataExcelTemp['ImportFlag'] == config('system_const_schet.import_flag_moderr'))) {
											$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_mod');
										}
									}

									if($dataExcelTemp['Kind'] == config('system_const_schet.import_kind_sogumi')) {
										if(!is_null($edate) && $dataExcelTemp['EDate'] != $edate) {
											$dataExcelTemp['ModifyFlag'] = config('system_const_schet.modifyflag_date');
											if(!(array_key_exists('ImportFlag',$dataExcelTemp) && $dataExcelTemp['ImportFlag'] == config('system_const_schet.import_flag_moderr'))) {
												$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_mod');
											}
										}
									}

									if($dataExcelTemp['Kind'] == config('system_const_schet.import_kind_kyokyu')) {

										$dataExcelTemp['K_BlockName_'.$genOld] = $dataOld['K_BlockName'];
										$dataExcelTemp['K_BlockKumiku_'.$genOld] = $dataOld['K_BlockKumiku'];

										$arrKBlockNameExcel = $this->convertBlockName($dataExcelTemp['K_BlockNick']);
										$kBlockNameNotGenExcel = $this->GetTextExceptCharIndex($arrKBlockNameExcel[0], self::EXCEPT_CHAR_INDEX);
										if($kBlockNameNotGenExcel != $dataOld['K_BlockNameNotGen']
										|| $dataExcelTemp['K_BlockKumiku'] != $dataOld['K_BlockKumiku']) {
											$dataExcelTemp['ModifyFlag'] = !array_key_exists('ModifyFlag',$dataExcelTemp) ?
											config('system_const_schet.modifyflag_kyokyu') :
											$dataExcelTemp['ModifyFlag'].config('system_const_schet.modifyflag_kyokyu');

											if(!(array_key_exists('ImportFlag',$dataExcelTemp) && $dataExcelTemp['ImportFlag'] == config('system_const_schet.import_flag_moderr'))) {
												$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_mod');
											}
										}
									}

									if(!array_key_exists('ImportFlag',$dataExcelTemp)) {
										$dataExcelTemp['ImportFlag'] = config('system_const_schet.import_flag_ok');
									}
									array_push($dataInsert, array($dataExcelTemp));

								}
							}
						}
					}
				}
			}
		}
	}

	/**
	* function create action button
	*
	* @param Request request
	* @return view
	*
	* @create 2020/10/29 Cuong
	* @update 2020/12/23 Cuong update condition sort
	*/
	public function create(Request $request) {
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));
		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$originalError = array();
		if (isset($request->err1)) {
			// 1,取得したデータが0件の場合
			$originalError[] = valueUrlDecode($request->err1);
			$datatype = 'error';
		}

		$rows = T_ImportData::where('ImportID', '=', valueUrlDecode($request->val6))
							->orderBy('Kind', 'asc')
							->orderBy('BlockName', 'asc')
							->orderBy('BlockKumiku', 'asc')
							->orderBy('BlockNick', 'asc')
							->get();
		if(count($rows) == 0) {
			$originalError[] = config('message.msg_schet_excel_error_019');
			$datatype = 'error';
			$datas = array();
		}

		if(count($rows) > 0 && $rows[0]->ImportFlag == config('system_const_schet.import_flag_err')) {
			// 2,取得したデータが、エラー時の内容の場合
			$dataDisplays = collect();
			foreach ($rows as $row) {
				$obj = new T_ImportData;
				$obj->fld1 = $row->Kind;
				$obj->fld2 = $row->BlockNick;
				$obj->fld3 = $row->BlockKumiku;
				$obj->fld4 = $row->Message .'[行=' .  $row->Row . ', 列=' . $row->Col . ']';
				$dataDisplays->add($obj);
			}

			$dataDisplaysOrderBy = $dataDisplays->sortBy('fld1')->sortBy('fld2')->sortBy('fld3');

			$keyindex = 0;
			foreach($dataDisplaysOrderBy as $value) {
				$keyindex++;
				$value['fld5'] = $keyindex;
			}

			// Handling sort
			$sort = ['fld1','fld2','fld3'];
			if (isset($request->sort) && $request->sort != '') {
				$sort = [$request->sort, 'fld5'];
			}

			$direction = (isset($request->direction) && !empty($request->direction)) ? $request->direction : 'asc';

			if(isset($request->val5) && in_array(valueUrlDecode($request->val5), [config('system_const.displayed_results_1'),
																	config('system_const.displayed_results_2'),
																	config('system_const.displayed_results_3')])){
				$pageunit = valueUrlDecode($request->val5);
			}else{
				$pageunit = config('system_const.displayed_results_1');
			}

			$datas = $this->sortAndPagination($dataDisplaysOrderBy, $sort, $direction, $pageunit, $request);

			$datas->getCollection()->transform(function ($value) {
				
				$value['fld1'] = $this->getKindName($value['fld1']);
				return $value;
			});

			$datatype = 'error';
		}elseif(count($rows) > 0) {
			/* 1,2以外の場合 */

			$dataDisplays = collect();	// New collection for save data display.
			// process data
			$this->processData($rows, $dataDisplays);

			$dataDisplaysOrderBy = $dataDisplays->sortBy('fld1')->sortBy('fld2')->sortBy('fld3');

			$keyindex = 0;
			foreach($dataDisplaysOrderBy as $value) {
				$keyindex++;
				$value['fld5'] = $keyindex;
			}

			// Handling sort
			$sort = ['fld1','fld2','fld3'];
			if (isset($request->sort) && $request->sort != '') {
				$sort = [$request->sort, 'fld5'];
			}

			$direction = (isset($request->direction) && $request->direction != '') ?  $request->direction : 'asc';

			if(isset($request->val5) && in_array(valueUrlDecode($request->val5), [config('system_const.displayed_results_1'),
																	config('system_const.displayed_results_2'),
																	config('system_const.displayed_results_3')])){
				$pageunit = valueUrlDecode($request->val5);
			}else{
				$pageunit = config('system_const.displayed_results_1');
			}

			$datas = $this->sortAndPagination($dataDisplaysOrderBy, $sort, $direction, $pageunit, $request);

			$datas->getCollection()->transform(function ($value) {
				$value['fld1'] = $this->getKindName($value['fld1']);
				return $value;
			});

			$datatype = 'ok';
		}

		$this->data['request'] = $request;
		$this->data['menuInfo'] = $menuInfo;
		$this->data['rows'] = $datas;
		$this->data['originalError'] = $originalError;
		$this->data['datatype'] = $datatype;
		return view('Schet/Import/create', $this->data);
	}

	/**
	* process data TImport before display method
	*
	* @param mix rows
	* @param collection dataDisplays
	*
	* @create 2020/11/01 Cuong
	* @update
	*/
	private function processData($rows, &$dataDisplays) {
		$dataImport = $rows->toArray();
		if(count($dataImport) > 0) {
			$dataAddAndDelete = array_filter($dataImport, function($dataItem) {
				return $dataItem['ImportFlag'] == config('system_const_schet.import_flag_add') ||
				$dataItem['ImportFlag'] == config('system_const_schet.import_flag_del') ;
			});

			$dataToSai = array_filter($dataAddAndDelete, function($item) {
				return $item['Kind'] == config('system_const_schet.import_kind_tosai');
			});

			$dataSogumi = array_filter($dataAddAndDelete, function($item) {
				return $item['Kind'] == config('system_const_schet.import_kind_sogumi');
			});

			$dataKyokyu = array_filter($dataAddAndDelete, function($item) {
				return $item['Kind'] == config('system_const_schet.import_kind_kyokyu');
			});

			if(count($dataToSai) > 0) {
				$grDataTosai = $this->groupDataImport($dataToSai);
				$this->processDataAddAndDel($grDataTosai, $dataDisplays);
			}

			if(count($dataSogumi) > 0) {
				$grDataSogumi = $this->groupDataImport($dataSogumi);
				$this->processDataAddAndDel($grDataSogumi, $dataDisplays);
			}

			if(count($dataKyokyu) > 0) {
				$grDataKyokyu = $this->groupDataImport($dataKyokyu);
				$this->processDataAddAndDel($grDataKyokyu, $dataDisplays);
			}
			// 取込フラグ[ImportFlag]= import_flag_mod(config\system_const_schet.php) 取込フラグ[ImportFlag]= import_flag_moderr (config\system_const_schet.php)
			$dataImportChange = array_filter($dataImport, function($dataItem) {
				return $dataItem['ImportFlag'] == config('system_const_schet.import_flag_mod') ||
				$dataItem['ImportFlag'] == config('system_const_schet.import_flag_moderr') || $dataItem['ImportFlag'] == config('system_const_schet.import_flag_ok');
			});

			$dataToSai = array_filter($dataImportChange, function($item) {
				return $item['Kind'] == config('system_const_schet.import_kind_tosai');
			});

			$dataSogumi = array_filter($dataImportChange, function($item) {
				return $item['Kind'] == config('system_const_schet.import_kind_sogumi');
			});

			$dataKyokyu = array_filter($dataImportChange, function($item) {
				return $item['Kind'] == config('system_const_schet.import_kind_kyokyu');
			});

			if(count($dataToSai) > 0) {
				$grDataTosai = $this->groupDataImport($dataToSai);
				$this->processDataChanged($grDataTosai, $dataDisplays);
			}

			if(count($dataSogumi) > 0) {
				$grDataSogumi = $this->groupDataImport($dataSogumi);
				$this->processDataChanged($grDataSogumi, $dataDisplays);
			}

			if(count($dataKyokyu) > 0) {
				$grDataKyokyu = $this->groupDataImport($dataKyokyu);
				$this->processDataChanged($grDataKyokyu, $dataDisplays);
			}
		}
	}

	/**
	* process data  method
	*
	* @param array data
	* @param collection dataDisplays
	*
	* @create 2020/11/23 Cuong
	* @update 2021/01/18 Cuong update blockname of gen when modify date.
	*/
	private function processDataChanged($grData, &$dataDisplays) {
		foreach($grData as $datas) {
			$flagLoop = false;
			$genBeforeChange = '';
			foreach($datas as $data) {
				$obj = new T_ImportData;
				$genDatas = $this->getGenFromBlockNick($data['BlockNick']);
				foreach($genDatas as $gen) {
					if(strpos($data['ModifyFlag'],config('system_const_schet.modifyflag_date')) !== false) {
						if(count($genDatas) == 2) {
							$obj = new T_ImportData;
						}
						if(!is_null($data['WorkItemID_'.$gen])) {
							/* [WorkItemID_○] of Gen has data */
							if(is_null($data['SDate_'.$gen])) {
								// TimeTrackerNX not data
								$obj->fld1 = $data['Kind'];
								$obj->fld2 = $data['BlockName'].$gen;
								$obj->fld3 = $data['BlockKumiku'];
								$obj->fld4 = config('message.msg_schet_log_004');
								$dataDisplays->add($obj);
								continue;
							}

							if($data['SDate'] != $data['SDate_'.$gen]) {
								$obj = new T_ImportData;
								$obj->fld1 = $data['Kind'];
								$obj->fld2 = $data['BlockName'].$gen;
								$obj->fld3 = $data['BlockKumiku'];
								$obj->fld4 = sprintf(config('message.msg_schet_log_003'),
								'開始日', Carbon::parse($data['SDate_'.$gen])->format('Y/m/d'),
								Carbon::parse($data['SDate'])->format('Y/m/d'));
								$dataDisplays->add($obj);
							}

							if($data['Kind'] == config('system_const_schet.import_kind_sogumi')) {
								if($data['EDate'] != $data['EDate_'.$gen]) {
									$obj = new T_ImportData;
									$obj->fld1 = $data['Kind'];
									$obj->fld2 = $data['BlockName'].$gen;
									$obj->fld3 = $data['BlockKumiku'];
									$obj->fld4 = sprintf(config('message.msg_schet_log_003'),
									'終了日', Carbon::parse($data['EDate_'.$gen])->format('Y/m/d'),
									Carbon::parse($data['EDate'])->format('Y/m/d'));
									$dataDisplays->add($obj);
								}
							}

						}else {
							/* [WorkItemID_○] of Gen has not data */
								// in group has only 1 data
							if(count($datas) == 1 && count($genDatas) == 1) {
								$dataFirstHasData = false;
								$arrGenAll = array('P','S','C');
								$arrGen = array_diff($arrGenAll, $genDatas);
								foreach($arrGen as $genx) {
									if(!is_null($data['WorkItemID_'.$genx])) {
										if($dataFirstHasData) {
											continue;
										}

										if(is_null($data['SDate_'.$genx])) {
											// TimeTrackerNX not data
											$obj->fld1 = $data['Kind'];
											$obj->fld2 = $data['BlockName'].$gen;
											$obj->fld3 = $data['BlockKumiku'];
											$obj->fld4 = config('message.msg_schet_log_004');
											$dataDisplays->add($obj);
											continue;
										}

										if($data['SDate'] != $data['SDate_'.$genx]) {
											$obj = new T_ImportData;
											$obj->fld1 = $data['Kind'];
											$obj->fld2 = $data['BlockName'].$gen;
											$obj->fld3 = $data['BlockKumiku'];
											$obj->fld4 = sprintf(config('message.msg_schet_log_003'),
											'開始日', Carbon::parse( $data['SDate_'.$genx])->format('Y/m/d'),
											Carbon::parse($data['SDate'])->format('Y/m/d'));
											$dataDisplays->add($obj);
										}

										if($data['Kind'] == config('system_const_schet.import_kind_sogumi')) {
											if($data['EDate'] != $data['EDate_'.$genx]) {
												$obj = new T_ImportData;
												$obj->fld1 = $data['Kind'];
												$obj->fld2 = $data['BlockName'].$gen;
												$obj->fld3 = $data['BlockKumiku'];
												$obj->fld4 = sprintf(config('message.msg_schet_log_003'),
												'終了日', Carbon::parse($data['EDate_'.$genx])->format('Y/m/d'),
												Carbon::parse($data['EDate'])->format('Y/m/d'));
												$dataDisplays->add($obj);
											}
										}

										$dataFirstHasData = true;
									}
								}
							}else {
								// in group has 2 data
								if(!is_null($data['WorkItemID_C'])) {
									if($gen == "P") {
										if(is_null($data['SDate_C'])) {
											// TimeTrackerNX not data
											$obj->fld1 = $data['Kind'];
											$obj->fld2 = $data['BlockName'].$gen;
											$obj->fld3 = $data['BlockKumiku'];
											$obj->fld4 = config('message.msg_schet_log_004');
											$dataDisplays->add($obj);
											continue;
										}

										if($data['SDate'] != $data['SDate_C']) {
											$obj = new T_ImportData;
											$obj->fld1 = $data['Kind'];
											$obj->fld2 = $data['BlockName'].$gen;
											$obj->fld3 = $data['BlockKumiku'];
											$obj->fld4 = sprintf(config('message.msg_schet_log_003'),
											'開始日', Carbon::parse($data['SDate_C'])->format('Y/m/d'),
											Carbon::parse($data['SDate'])->format('Y/m/d'));
											$dataDisplays->add($obj);
										}

										if($data['Kind'] == config('system_const_schet.import_kind_sogumi')) {
											if($data['EDate'] != $data['EDate_C']) {
												$obj = new T_ImportData;
												$obj->fld1 = $data['Kind'];
												$obj->fld2 = $data['BlockName'].$gen;
												$obj->fld3 = $data['BlockKumiku'];
												$obj->fld4 = sprintf(config('message.msg_schet_log_003'),
												'終了日', Carbon::parse($data['EDate_C'])->format('Y/m/d'),
												Carbon::parse($data['EDate'])->format('Y/m/d'));
												$dataDisplays->add($obj);
											}
										}
									}

								}

							}
						}
					}

					if($data['Kind'] == config('system_const_schet.import_kind_kyokyu')
					&& strpos($data['ModifyFlag'],config('system_const_schet.modifyflag_kyokyu')) !== false) {
						if(!is_null($data['K_BlockName_'.$gen]) && !is_null($data['K_BlockKumiku_'.$gen])) {
							if($data['K_BlockName'] != $this->GetTextExceptCharIndex($data['K_BlockName_'.$gen], self::EXCEPT_CHAR_INDEX) ||
							$data['K_BlockKumiku'] != $data['K_BlockKumiku_'.$gen]) {

								$genKBLockNicks = $this->getGenFromBlockNick($data['K_BlockNick']);
								$genKBLockNickDisplay = '';
								foreach($genKBLockNicks as $genBlock) {
									$genKBLockNickDisplay .= $genBlock;
								}
								$obj = new T_ImportData;
								$obj->fld1 = $data['Kind'];
								$obj->fld2 = $data['BlockName'].$gen;
								$obj->fld3 = $data['BlockKumiku'];
								$obj->fld4 = sprintf(config('message.msg_schet_log_003'),
								'供給先', $data['K_BlockName_'.$gen] . '(' . $data['K_BlockKumiku_'.$gen] . ')' ,
								$data['K_BlockName'] . $genKBLockNickDisplay . '(' . $data['K_BlockKumiku'] . ')');
								$dataDisplays->add($obj);
							}
						}else {
							if(count($datas) == 1 && count($genDatas) == 1) {
								$dataFirstHasData = false;
								$arrGenAll = array('P','S','C');
								$arrGen = array_diff($arrGenAll, $genDatas);
								foreach($arrGen as $genz) {
									if(!is_null($data['K_BlockName_'. $genz]) && !is_null($data['K_BlockKumiku_'. $genz])) {
										if($dataFirstHasData) {
											continue;
										}

										if($data['K_BlockName'] != $this->GetTextExceptCharIndex($data['K_BlockName_'. $genz], self::EXCEPT_CHAR_INDEX) ||
										$data['K_BlockKumiku'] != $data['K_BlockKumiku_'. $genz]) {
											$genKBLockNicks = $this->getGenFromBlockNick($data['K_BlockNick']);
											$genKBLockNickDisplay = '';
											foreach($genKBLockNicks as $genBlock) {
												$genKBLockNickDisplay .= $genBlock;
											}
											$obj = new T_ImportData;
											$obj->fld1 = $data['Kind'];
											$obj->fld2 = $data['BlockName'].$gen;
											$obj->fld3 = $data['BlockKumiku'];
											$obj->fld4 = sprintf(config('message.msg_schet_log_003'),
											'供給先', $data['K_BlockName_'.$genz] . '(' . $data['K_BlockKumiku_'.$genz] . ')' ,
											$data['K_BlockName'] . $genKBLockNickDisplay . '(' . $data['K_BlockKumiku'] . ')');

											$dataDisplays->add($obj);
											$dataFirstHasData = true;
										}
									}
								}
							}else {
								// in group has 2 data
								if(!is_null($data['K_BlockName_C']) && !is_null($data['K_BlockKumiku_C'])) {
									if($gen == "P") {
										if($data['K_BlockName'] != $this->GetTextExceptCharIndex($data['K_BlockName_'.$gen], self::EXCEPT_CHAR_INDEX) ||
										$data['K_BlockKumiku'] != $data['K_BlockKumiku_'.$gen]) {
											$genKBLockNicks = $this->getGenFromBlockNick($data['K_BlockNick']);
											$genKBLockNickDisplay = '';
											foreach($genKBLockNicks as $genBlock) {
												$genKBLockNickDisplay .= $genBlock;
											}
											$obj = new T_ImportData;
											$obj->fld1 = $data['Kind'];
											$obj->fld2 = $data['BlockName'].$gen;
											$obj->fld3 = $data['BlockKumiku'];
											$obj->fld4 = sprintf(config('message.msg_schet_log_003'),
											'供給先', $data['K_BlockName_C'] . '(' . $data['K_BlockKumiku_C'] . ')' ,
											$data['K_BlockName'] . $genKBLockNickDisplay . '(' . $data['K_BlockKumiku'] . ')');

											$dataDisplays->add($obj);
										}
									}
								}
							}
						}

					}
				}

				if(!is_null($data['WorkItemID_P'])) {
					if(strpos($genBeforeChange, "P") === false) {
						$genBeforeChange .= 'P';
					}
				}
				if(!is_null($data['WorkItemID_S'])) {
					if(strpos($genBeforeChange, "S") === false) {
						$genBeforeChange .= 'S';
					}
				}
				if(!is_null($data['WorkItemID_C'])) {
					if(strpos($genBeforeChange, "C") === false) {
						$genBeforeChange .= 'C';
					}
				}

				if(strpos($data['ModifyFlag'],config('system_const_schet.modifyflag_gen')) !== false) {
					$genAfterChange = '';
					$flagLoop = true;
					if(count($datas) == 1) {
						foreach($genDatas as $gen) {
							$genAfterChange .= $gen;
						}
					}

					if(count($datas) == 2 || count($genDatas) == 2) {
						$genAfterChange = 'PS';
					}

					if(count($genDatas) == 1 && count($datas) == 1) {
						if($genBeforeChange != $genAfterChange && strpos($genBeforeChange, $genAfterChange) === false) {
							$obj = new T_ImportData;
							$obj->fld1 = $data['Kind'];
							$obj->fld2 = $data['BlockName'].$genAfterChange;
							$obj->fld3 = $data['BlockKumiku'];
							$obj->fld4 = sprintf(config('message.msg_schet_log_003'), '舷', $genBeforeChange, $genAfterChange);
							$dataDisplays->add($obj);
						}

						if(mb_strlen($genBeforeChange) == 2) {
							if($genAfterChange == "P") {
								$genDel = 'S';
							}

							if($genAfterChange == "S") {
								$genDel = 'P';
							}

							if($genAfterChange == "C") {
								$genDel = 'S';
							}

							$objDelete = new T_ImportData;
							$objDelete->fld1 = $data['Kind'];
							$objDelete->fld2 = $data['BlockName'].$genDel;
							$objDelete->fld3 = $data['BlockKumiku'];
							$objDelete->fld4 = config('message.msg_schet_log_002');
							$dataDisplays->add($objDelete);
						}
					}

					if(count($genDatas) == 2) {
						if($genBeforeChange != $genAfterChange && strpos($genAfterChange, $genBeforeChange) === false) {
							$objNew = new T_ImportData;
							$objNew->fld1 = $data['Kind'];
							$objNew->fld2 = $data['BlockName']."P";
							$objNew->fld3 = $data['BlockKumiku'];
							$objNew->fld4 = sprintf(config('message.msg_schet_log_003'), '舷', $genBeforeChange, $genAfterChange);
							$dataDisplays->add($objNew);
						}

						if(mb_strlen($genAfterChange) == 2) {
							if($genBeforeChange == "P") {
								$genAdd = 'S';
							}

							if($genBeforeChange == "S") {
								$genAdd = 'P';
							}

							if($genBeforeChange == "C") {
								$genAdd = 'S';
							}

							$objAdd= new T_ImportData;
							$objAdd->fld1 = $data['Kind'];
							$objAdd->fld2 = $data['BlockName'].$genAdd;
							$objAdd->fld3 = $data['BlockKumiku'];
							$objAdd->fld4 = config('message.msg_schet_log_001');
							$dataDisplays->add($objAdd);
						}
					}

					if(count($datas) == 2) {
						$objNew = new T_ImportData;
						$objNew->fld1 = $data['Kind'];
						$objNew->fld2 = $data['BlockName']."P";
						$objNew->fld3 = $data['BlockKumiku'];
					}
				}
			}

			if($flagLoop && count($datas) == 2) {
				if($genBeforeChange != $genAfterChange && strpos($genAfterChange, $genBeforeChange) === false) {
					$objNew->fld4 = sprintf(config('message.msg_schet_log_003'), '舷', $genBeforeChange, $genAfterChange);
					$dataDisplays->add($objNew);
				}

				if(mb_strlen($genAfterChange) == 2) {
					if($genBeforeChange == "P") {
						$genAdd = 'S';
					}

					if($genBeforeChange == "S") {
						$genAdd = 'P';
					}

					if($genBeforeChange == "C") {
						$genAdd = 'S';
					}

					$objAdd= new T_ImportData;
					$objAdd->fld1 = $data['Kind'];
					$objAdd->fld2 = $data['BlockName'].$genAdd;
					$objAdd->fld3 = $data['BlockKumiku'];
					$objAdd->fld4 = config('message.msg_schet_log_001');
					$dataDisplays->add($objAdd);
				}
			}
		}
	}

	/**
	* process data TImport before display method
	*
	* @param array data
	* @param collection dataDisplays
	*
	* @create 2020/11/23 Cuong
	* @update
	*/
	private function processDataAddAndDel($grDatas, &$dataDisplays) {
			// 追加の場合 && 削除の場合
		foreach($grDatas as $key=>$datas) {
			if(count($datas) == 1) {
				foreach($datas as $data) {
					$gens = $this->getGenFromBlockNick($data['BlockNick']);
					$obj = new T_ImportData;
					$obj->fld1 = $data['Kind'];
					if(count($gens) == 1) {
						$obj->fld2 = $data['BlockName'].$gens['0'];
					}
					if(count($gens) == 2) {
						$obj->fld2 = $data['BlockName'].'PS';
					}

					$obj->fld3 = $data['BlockKumiku'];
					$obj->fld4 = $data['ImportFlag'] == config('system_const_schet.import_flag_add') ?
					config('message.msg_schet_log_001') : config('message.msg_schet_log_002');
					$dataDisplays->add($obj);
				}
			}

			if(count($datas) == 2) {
				$firstData = true;
				foreach($datas as $data) {
					if(!$firstData) {
						continue;
					}

					$gens = $this->getGenFromBlockNick($data['BlockNick']);
					$obj = new T_ImportData;
					$obj->fld1 = $data['Kind'];
					$obj->fld2 = $data['BlockName'].'PS';
					$obj->fld3 = $data['BlockKumiku'];
					$obj->fld4 = $data['ImportFlag'] == config('system_const_schet.import_flag_add') ?
					config('message.msg_schet_log_001') : config('message.msg_schet_log_002');
					$dataDisplays->add($obj);
					$firstData = false;

				}
			}
		}
	}

	/**
	* process data TImport before display method
	*
	* @param mix rows
	* @param collection dataDisplays
	*
	* @create 2020/11/01 Cuong
	* @update
	*/
	private function groupDataImport($data) {
		$grData = array();
		foreach($data as $item) {
			$arrTem = array_filter($data, function($dataItem) use($item) {
				return $dataItem['BlockName'] == $item['BlockName'] && $dataItem['BlockKumiku'] == $item['BlockKumiku'];
			});
			if(count($arrTem) > 0) {
				$key = $item['BlockKumiku'].'-'.$item['BlockName'];
				$grData[$key] = $arrTem;
			}
		}

		return $grData;
	}


	private $mdataAdd = array();			//array data add new
	private $mdataHasChange = array();	//array data 舷変更(ログ用)
	private $mdataDelete = array(); 		//array data delete
	private $mdataGenChangeToroku = array(); //array data 舷変更(登録用)
	private $mdataGenChangeLog = array();	//array data 舷変更(ログ用)
	private $mdataTosaiNotChange = array();	//array data not change

	/**
	* function create action button
	*
	* @param Request requests
	* @return view
	*
	* @create 2020/11/09 Cuong
	* @update 2020/12/08 Cuong update condition get data T_ImportData
	* @update 2020/12/23 Cuong update return mesage error when get data calendar from TimeTrackerNX, update deleteLock
	*/
	public function save(Request $request) {
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));
		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		//process block
		$resultProcessBlock = $this->tryLock($menuInfo->KindID, config('system_const_schet.syslock_menuid_schet'),
		$menuInfo->UserID, $menuInfo->SessionID, valueUrlDecode($request->val1), false);

		/* Get DATA */
		// get datat T_ImportData
		$dataImportData = T_ImportData::where('ImportID', '=', valueUrlDecode($request->val6))
							->where('ImportFlag','!=',config('system_const_schet.import_flag_del'))
							->orderBy('Kind', 'asc')->orderBy('BlockName', 'asc')
							->orderBy('BlockKumiku', 'asc')->orderBy('BlockNick', 'asc')
							->get()->toArray();
		// get datat T_Tosai
		$dataTosai = T_Tosai::select('BlockName','BlockKumiku','WorkItemID')
							->selectRaw('SUBSTRING(BlockName, 1, 12) as BlockNameNotGen,
										SUBSTRING(BlockName, 13, 1) as Gen')
							->where('ProjectID', '=', valueUrlDecode($request->val1))
							->where('OrderNo', '=', valueUrlDecode($request->val2))
							->orderBy('BlockName', 'asc')->orderBy('BlockKumiku', 'asc')
							->get()->toArray();
		// get datat T_Kyokyu
		$dataKyokyu = T_Kyokyu::select('BlockName','BlockKumiku','WorkItemID', 'K_BlockName','K_BlockKumiku')
							->selectRaw('SUBSTRING(BlockName, 1, 12) as BlockNameNotGen,
										SUBSTRING(BlockName, 13, 1) as Gen,
										SUBSTRING(K_BlockName, 1, 12) as K_BlockNameNotGen,
										SUBSTRING(K_BlockName, 13, 1) as K_Gen')
							->where('ProjectID', '=', valueUrlDecode($request->val1))
							->where('OrderNo', '=', valueUrlDecode($request->val2))
							->orderBy('BlockName', 'asc')->orderBy('BlockKumiku', 'asc')
							->get()->toArray();
		// get datat T_Sogumi
		$dataSogumi = T_Sogumi::select('BlockName','BlockKumiku','WorkItemID')
							->selectRaw('SUBSTRING(BlockName, 1, 12) as BlockNameNotGen,
										SUBSTRING(BlockName, 13, 1) as Gen')
							->where('ProjectID', '=', valueUrlDecode($request->val1))
							->where('OrderNo', '=', valueUrlDecode($request->val2))
							->orderBy('BlockName', 'asc')->orderBy('BlockKumiku', 'asc')
							->get()->toArray();

		// データを登録単位に集約する。
		$dataNitteis = array();
		$dataTosaiNittei = array();
		$dataSogumiNittei = array();
		$dataKyokyuNittei  = array();

		$this->getDataTosaiNittei($dataImportData, $dataTosaiNittei, $dataSogumiNittei, $dataKyokyuNittei);

		// group data tosai nittei by blockaname and kumiku
		$groupTosaiNittei = array();
		if(count($dataTosaiNittei) > 0) {
			$this->groupDataNittei($dataTosaiNittei, $groupTosaiNittei);
		}

		// group data sogumi nittei by blockaname and kumiku
		$groupSogumiNittei = array();
		if(count($dataSogumiNittei) > 0) {
			$this->groupDataNittei($dataSogumiNittei, $groupSogumiNittei);
		}

		// group data kyokyu nittei by blockaname and kumiku
		$groupKyokyuNittei = array();
		if(count($dataKyokyuNittei) > 0) {
			$this->groupDataNittei($dataKyokyuNittei, $groupKyokyuNittei);
		}

		// group data by blockaname and kumiku of data tosai
		$groupDataTosai = array();
		$this->groupDataTosai($dataTosai, $groupDataTosai);

		// group data by blockaname and kumiku of data sogumi
		$groupDataSogumi = array();
		$this->groupDataSogumi($dataSogumi, $groupDataSogumi);

		// group data by blockaname and kumiku of data kyokyu
		$groupDataKyokyu = array();
		$this->groupDataKyokyu($dataKyokyu, $groupDataKyokyu);

		// 1.既存データになく、集約データにあるグループは、「追加」となる。
		// $this->getDataAddNew($groupDataNittei, $groupDataTosai, $groupDataSogumi, $groupDataKyokyu);
		$this->getDataAddNew($groupTosaiNittei, $groupDataTosai, $groupSogumiNittei, $groupDataSogumi, $groupKyokyuNittei, $groupDataKyokyu);

		// 2.既存データにあり、集約データにないグループは、「削除」となる。
		// $this->getDataDelete($groupDataNittei, $groupDataTosai, $groupDataSogumi, $groupDataKyokyu);
		$this->getDataDelete($groupTosaiNittei, $groupDataTosai, $groupSogumiNittei, $groupDataSogumi, $groupKyokyuNittei, $groupDataKyokyu);

		// 1,2 以外(同じグループに既存データにも集約データにも1件以上ある) の場合
		// $this->getDataChange($groupDataNittei, $groupDataTosai, $groupDataSogumi, $groupDataKyokyu, $dataSogumi);
		$this->getDataChange($groupTosaiNittei, $groupDataTosai, $groupSogumiNittei, $groupDataSogumi, $groupKyokyuNittei, $groupDataKyokyu);

		// 4 同じグループ内で、舷が変更されたか比較する。
		$this->getDataGenChangeForLog($groupTosaiNittei, $groupDataTosai, $groupSogumiNittei, $groupDataSogumi, $groupKyokyuNittei, $groupDataKyokyu);

	/* コントローラの処理(データの登録) */
		/* get data OrderRoot from TimeTrackerNX */
		$timeTrackerCommon = new TimeTrackerCommon();
		$projectID = valueUrlDecode($request->val1);
		$orderNo = valueUrlDecode($request->val2);
		$orderRoot = $timeTrackerCommon->getOrderRoot($projectID, $orderNo);

		$urlErr = url('/');
		$urlErr .= '/' . $menuInfo->KindURL;
		$urlErr .= '/' . $menuInfo->MenuURL;
		$urlErr .= '/create';
		$urlErr .= '?cmn1=' . valueUrlEncode($menuInfo->KindID);
		$urlErr .= '&cmn2=' . valueUrlEncode($menuInfo->MenuID);
		$urlErr .= '&page=' . $request->page;
		$urlErr .= '&sort=' . $request->sort;
		$urlErr .= '&direction=' . $request->direction;
		$urlErr .= '&val1=' . $request->val1;
		$urlErr .= '&val2=' . $request->val2;
		$urlErr .= '&val5=' . $request->val5;
		$urlErr .= '&val6=' . $request->val6;

		if(is_string($orderRoot)) {			//when get data from TimeTrackerNX error
			// 排他制御を解除する
			$this->deleteLock($menuInfo->KindID, config('system_const_schet.syslock_menuid_schet'),
			$menuInfo->SessionID, $projectID);
			$urlErr .= '&err1=' . valueUrlEncode($orderRoot);
			return redirect($urlErr);
		}

			/* get data calendar from TimeTrackerNX */
		$dataCalendar = $timeTrackerCommon->getCalendar($projectID);
		if(is_string($dataCalendar)) {		//when get data from TimeTrackerNX error
			// 排他制御を解除する
			$this->deleteLock($menuInfo->KindID, config('system_const_schet.syslock_menuid_schet'),
			$menuInfo->SessionID, $projectID);
			$urlErr .= '&err1=' . valueUrlEncode($dataCalendar);
			return redirect($urlErr);
		}

		/* 取込履歴テーブルにデータを登録する */
		$MaxID = T_ImportHistory::selectRaw('MAX(ID) as MaxID')->first()->MaxID;
		$dateNow = DB::selectOne('SELECT CONVERT(DATE, getdate()) AS sysdate')->sysdate;
		$dateNow = str_replace('-', '/', $dateNow);
		$numberLoop = 1;
		while ($numberLoop <= 5) {
			try {
				$MaxID = is_null($MaxID) ? 1 : $MaxID + 1;

				$objImportHistory = new T_ImportHistory;
				$objImportHistory->ID = $MaxID;
				$objImportHistory->Import_User = $menuInfo->UserID;
				$objImportHistory->Import_Date = $dateNow;
				$objImportHistory->ProjectID = $projectID;
				$objImportHistory->OrderNo = $orderNo;
				$objImportHistory->LinkFlag = config('system_const_schet.linkflag_notlink');
				$objImportHistory->StatusFlag = config('system_const_schet.schet_import_status_running');
				$objImportHistory->Save();
				$numberLoop = 10;
			} catch (Exception $e) {
				$numberLoop ++;
				if($numberLoop == 6) {
					// 排他制御の解除
					$this->deleteLock($menuInfo->KindID, config('system_const_schet.syslock_menuid_schet'),
						$menuInfo->SessionID, $projectID);

					// 日程表取込画面(020201)にリダイレクト（GETで遷移するURLを下記の表の内容で作成）
					$urlErr = url('/');
					$urlErr .= '/' . $menuInfo->KindURL;
					$urlErr .= '/' . $menuInfo->MenuURL;
					$urlErr .= '/index';
					$urlErr .= '?cmn1=' . valueUrlEncode($menuInfo->KindID);
					$urlErr .= '&cmn2=' . valueUrlEncode($menuInfo->MenuID);
					$urlErr .= '&val1=' . $request->val1;
					$urlErr .= '&val2=' . $request->val2;
					$urlErr .= '&val5=' . $request->val5;
					$urlErr .= '&err1=' . valueUrlEncode(config('message.msg_cmn_db_016'));
					return redirect($urlErr);
				}
			}
		}

		/* 取込ログテーブルにデータを登録する */
		$this->registerImportLog($MaxID);

		/* 集約データを登録する  */
		$result = $this->registerAggregateData($request, $menuInfo, $MaxID, $projectID, $orderNo, $orderRoot, $dataCalendar);
		if(is_string($result)) {
			// $result is string : error.
			return redirect($result);
		}

	/* 	コントローラの処理(登録完了後の処理) */
			// 搭載日程取込履歴[T_ImportHistory]の状態フラグを更新する
		$this->updateFlagImportHistory($MaxID,config('system_const_schet.schet_import_status_done'));
			// 排他制御を解除する
		$this->deleteLock($menuInfo->KindID, config('system_const_schet.syslock_menuid_schet'), $menuInfo->SessionID, $projectID);

		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/index';
		$url .= '?cmn1=' . valueUrlEncode($menuInfo->KindID);
		$url .= '&cmn2=' . valueUrlEncode($menuInfo->MenuID);
		$url .= '&val1=' . $request->val1;
		$url .= '&val2=' . $request->val2;
		$url .= '&val5=' . $request->val5;

		return redirect($url);
	}

	/**
	* get gen from blocknick method
	*
	* @param string blockNick
	* @return string
	*
	* @create 2020/10/29　Cuong
	* @update
	*/
	private function getGenFromBlockNick($blockNick){
		$arrGen = array();
		if(!empty($blockNick) && is_string($blockNick)) {
			$exploded = explode(' ', $blockNick);
			$gen = end($exploded);
			$arrGen = str_split($gen);
		}
		return $arrGen;
	}

	/**
	* get gen from blocknick method
	*
	* @param int $kind
	* @return string
	*
	* @create 2020/10/29　Cuong
	* @update
	*/
	private function getKindName($kind){
		if($kind == config('system_const_schet.import_kind_tosai')) {
			return config('system_const_schet.import_kindname_tosai');
		}

		if($kind == config('system_const_schet.import_kind_kyokyu')) {
			return config('system_const_schet.import_kindname_kyokyu');
		}

		if($kind == config('system_const_schet.import_kind_sogumi')) {
			return config('system_const_schet.import_kindname_sogumi');
		}
		return '';
	}

	/**
	* get data tosai nittei method
	*
	* @param array dataImportData
	* @param array arrTosai
	* @param array arrSogumi
	* @param array arrKyokyu
	* @return
	*
	* @create 2020/11/11　Cuong
	* @update
	*/
	private function getDataTosaiNittei($dataImportData, &$arrTosai, &$arrSogumi, &$arrKyokyu) {
		$arrayTempData = array();
		foreach($dataImportData as $data) {
			$gens = $this->getGenFromBlockNick($data['BlockNick']);
			if($data['Kind'] != config('system_const_schet.import_kind_kyokyu')){
				foreach($gens as $gen) {
					$temp = array();
					$temp = $data;
					$temp['BlockNameFull'] = $temp['BlockName'].$gen;
					$temp['Gen'] = $gen;

					array_push($arrayTempData,$temp);
				}
			}else {
				$genKBlockNicks = $this->getGenFromBlockNick($data['K_BlockNick']);
				if(count($gens) == 1 && count($genKBlockNicks) == 1) {
					$temp = array();
					$temp = $data;
					$temp['BlockNameFull'] = $temp['BlockName'].$gens[0];
					$temp['Gen'] = $gens[0];
					$temp['KBlockNameFull'] = $temp['K_BlockName'].$genKBlockNicks[0];
					array_push($arrayTempData,$temp);
				}
				if(count($gens) == 1 && count($genKBlockNicks) == 2) {
					if(in_array($genKBlockNicks, $gens[0])) {
						$temp = array();
						$temp = $data;
						$temp['BlockNameFull'] = $temp['BlockName'].$gens[0];
						$temp['Gen'] = $gens[0];
						$temp['KBlockNameFull'] = $temp['K_BlockName'].$gens[0];
					}else {
						$temp = array();
						$temp = $data;
						$temp['BlockNameFull'] = $temp['BlockName'].$gens[0];
						$temp['Gen'] = $gens[0];
						$temp['KBlockNameFull'] = $temp['K_BlockName'].'P';
						array_push($arrayTempData,$temp);
					}
				}

				if(count($gens) == 2 && count($genKBlockNicks) == 1) {
					foreach($gens as $gen) {
						$temp = array();
						$temp = $data;
						$temp['BlockNameFull'] = $temp['BlockName'].$gen;
						$temp['Gen'] = $gen;
						$temp['KBlockNameFull'] = $temp['K_BlockName'].$genKBlockNicks[0];
						array_push($arrayTempData,$temp);
					}
				}

				if(count($gens) == 2 && count($genKBlockNicks) == 2) {
					foreach($gens as $gen) {
						$temp = array();
						$temp = $data;
						$temp['BlockNameFull'] = $temp['BlockName'].$gen;
						$temp['Gen'] = $gen;
						$temp['KBlockNameFull'] = $temp['K_BlockName'].$gen;
						array_push($arrayTempData,$temp);
					}
				}
			}
		}

		$arrTosai = array_filter($arrayTempData, function($data){
			return $data['Kind'] == config('system_const_schet.import_kind_tosai');
		});
		$arrSogumi = array_filter($arrayTempData, function($data){
			return $data['Kind'] == config('system_const_schet.import_kind_sogumi');
		});
		$arrKyokyu = array_filter($arrayTempData, function($data){
			return $data['Kind'] == config('system_const_schet.import_kind_kyokyu');
		});

	}

	/**
	* group data nittei method
	*
	* @param array dataNittei
	* @param array &$groupData
	*
	* @create 2020/11/11　Cuong
	* @update
	*/
	private function groupDataNittei($dataNittei, &$groupData) {
		$dataTemp = $dataNittei;
		// group data by blockaname and kumiku  tosai
		foreach($dataTemp as &$data) {
			$group = array_values(array_filter($dataTemp, function($item) use($data) {
				return $item['BlockName'] == $data['BlockName'] && $item['BlockKumiku'] == $data['BlockKumiku'];
			}));
			$groupData[$data['BlockKumiku'].'-'.$data['BlockName']] = $group;
			foreach($group as $key=>$value) {
				unset($dataTemp[$key]);
			}
		}
	}
	/**
	* group data tosai method
	*
	* @param array data T_Tossai
	* @param array &$groupDataTosai
	*
	* @create 2020/11/11　Cuong
	* @update
	*/
	private function groupDataTosai($dataTosai, &$groupDataTosai) {
		$dataTosaiTemp = $dataTosai;
		// group data by blockaname and kumiku  tosai
		foreach($dataTosaiTemp as &$tosai) {
			$group = array_values(array_filter($dataTosaiTemp, function($item) use($tosai) {
				return $this->GetTextExceptCharIndex($item['BlockName'], self::EXCEPT_CHAR_INDEX) == $this->GetTextExceptCharIndex($tosai['BlockName'], self::EXCEPT_CHAR_INDEX)
				&& $item['BlockKumiku'] == $tosai['BlockKumiku'];
			}));
			$groupDataTosai[$tosai['BlockKumiku'].'-'.$this->GetTextExceptCharIndex($tosai['BlockName'], self::EXCEPT_CHAR_INDEX)] = $group;
			foreach($group as $key=>$value) {
				unset($dataTosaiTemp[$key]);
			}
		}
	}

	/**
	* group data sogumi method
	*
	* @param array data T_Sogumi
	* @param array &$groupDataSogumi
	*
	* @create 2020/11/11　Cuong
	* @update
	*/
	private function groupDataSogumi($dataSogumi, &$groupDataSogumi) {
		$dataSogumiTemp = $dataSogumi;
		// group data by blockaname and kumiku  sogumi
		foreach($dataSogumiTemp as &$sogumi) {
			$group = array_values(array_filter($dataSogumiTemp, function($item) use($sogumi) {
				return $this->GetTextExceptCharIndex($item['BlockName'], self::EXCEPT_CHAR_INDEX) == $this->GetTextExceptCharIndex($sogumi['BlockName'], self::EXCEPT_CHAR_INDEX)
				&& $item['BlockKumiku'] == $sogumi['BlockKumiku'];
			}));
			$groupDataSogumi[$sogumi['BlockKumiku'].'-'.$this->GetTextExceptCharIndex($sogumi['BlockName'], self::EXCEPT_CHAR_INDEX)] = $group;
			foreach($group as $key=>$value) {
				unset($dataSogumiTemp[$key]);
			}
		}
	}

	/**
	* group data kyokyu method
	*
	* @param array data T_Kyokyu
	* @param array &$groupDataKyokyu
	*
	* @create 2020/11/11　Cuong
	* @update
	*/
	private function groupDataKyokyu($dataKyokyu, &$groupDataKyokyu) {
		$dataKyokyuTemp = $dataKyokyu;
		// group data by blockaname and kumiku  kyokyu
		foreach($dataKyokyuTemp as &$kyokyu) {
			$group = array_filter($dataKyokyuTemp, function($item) use($kyokyu) {
				return $this->GetTextExceptCharIndex($item['BlockName'], self::EXCEPT_CHAR_INDEX) == $this->GetTextExceptCharIndex($kyokyu['BlockName'], self::EXCEPT_CHAR_INDEX)
				&& $item['BlockKumiku'] == $kyokyu['BlockKumiku'];
			});
			$groupDataKyokyu[$kyokyu['BlockKumiku'].'-'.$this->GetTextExceptCharIndex($kyokyu['BlockName'], self::EXCEPT_CHAR_INDEX)] = $group;
			foreach($group as $key=>$value) {
				unset($dataKyokyuTemp[$key]);
			}
		}
	}

	/**
	* get data add new method
	*
	* @param array grDataTosaiNittei
	* @param array grDataTosai
	* @param array grDataSogumiNittei
	* @param array grDataSogumi
	* @param array grDataKyokyuNittei
	* @param array grDataKyokyu
	* @return
	*
	* @create 2020/11/11　Cuong
	* @update
	*/
	private function getDataAddNew($grDataTosaiNittei, $grDataTosai, $grDataSogumiNittei, $grDataSogumi, $grDataKyokyuNittei, $grDataKyokyu) {
		$this->checkGroupExistsInDB($grDataTosaiNittei, $grDataTosai);
		$this->checkGroupExistsInDB($grDataSogumiNittei, $grDataSogumi);
		$this->checkGroupExistsInDB($grDataKyokyuNittei, $grDataKyokyu);
	}

	/**
	* check group of data_nittei exists in data db  method
	*
	* @param array groupDataNittei
	* @param array groupData (data db)
	* @return
	*
	* @create 2020/11/25　Cuong
	* @update
	*/
	private function checkGroupExistsInDB($groupDataNittei, $groupData) {
		foreach($groupDataNittei as $group=>$dataNitteis) {
			if(!array_key_exists($group, $groupData)) {
				foreach($dataNitteis as $data) {
					array_push($this->mdataAdd, $data);
				}
			}
		}
	}

	/**
	* check group of data db  exists in data_nittei  method
	*
	* @param array groupDataNittei
	* @param array groupData (data db)
	* @param array kind
	* @return
	*
	* @create 2020/11/25　Cuong
	* @update
	*/
	private function checkGroupExistsInDataNittei($groupData, $groupDataNittei, $kind) {
		foreach($groupData as $group=>$datas) {
			if(!array_key_exists($group, $groupDataNittei)) {
				foreach($datas as $data) {
					$data['Kind'] = $kind;
					array_push($this->mdataDelete, $data);
				}
			}
		}
	}

	/**
	* get data has change method
	*
	* @param array grDataTosaiNittei
	* @param array grDataTosai
	* @param array grDataSogumiNittei
	* @param array grDataSogumi
	* @param array grDataKyokyuNittei
	* @param array grDataKyokyu
	* @return
	*
	* @create 2020/11/11　Cuong
	* @update 2020/11/25 Cuong
	*/
	private function getDataChange($grDataTosaiNittei, $grDataTosai, $grDataSogumiNittei, $grDataSogumi, $grDataKyokyuNittei, $grDataKyokyu) {
		$kind = config('system_const_schet.import_kind_tosai');
		$this->xGetDataChange($kind, $grDataTosaiNittei, $grDataTosai);

		$kind = config('system_const_schet.import_kind_sogumi');
		$this->xGetDataChange($kind, $grDataSogumiNittei, $grDataSogumi);

		$kind = config('system_const_schet.import_kind_kyokyu');
		$this->xGetDataChange($kind, $grDataKyokyuNittei, $grDataKyokyu, $grDataSogumiNittei, $grDataSogumi);
	}

	/**
	* process data change method
	*
	* @param string kind
	* @param array grDataNitteis
	* @param array grDataOlds
	* @param array grDataSogumiNittei
	* @param array grDataSogumi
	* @return
	*
	* @create 2020/11/25 Cuong
	* @update
	*/
	private function xGetDataChange($kind, $grDataNitteis, $grDataOlds, $grDataSogumiNittei = null, $grDataSogumi = null) {
		foreach ($grDataNitteis as $key => $dataNitteis) {
			$remainData = array();
			$remainDataNittei = array();
			$sameData = array();
			$sameDataNittei = array();
			if(array_key_exists($key, $grDataOlds)) {
				foreach($dataNitteis as $ItemDataNittei) {
					$hasSameGen = array_values(array_filter($grDataOlds[$key], function($data) use($ItemDataNittei){
						return $data['BlockName'] == $ItemDataNittei['BlockNameFull'] && $data['BlockKumiku'] == $ItemDataNittei['BlockKumiku'];
					}));

					if(count($hasSameGen) > 0) {
						array_push($sameDataNittei, $ItemDataNittei);
						array_push($sameData, $hasSameGen[0]);
						// Xu ly C
						$dataNittei = $ItemDataNittei;
						$dataDb = $hasSameGen[0];
						$workItemIdOld = null;
						$workItemIdNew = null;

						if($ItemDataNittei['Kind'] == config('system_const_schet.import_kind_kyokyu')) {
							$keyGroupSogumi = $dataDb['K_BlockKumiku'].'-'.$dataDb['K_BlockNameNotGen'];
							if(array_key_exists($keyGroupSogumi, $grDataSogumi)) {
								$groupSogumi = $grDataSogumi[$keyGroupSogumi];
								$sogumi = array_values(array_filter($groupSogumi, function($itemSogumi) use($dataDb){
									return $itemSogumi['BlockName'] == $dataDb['K_BlockName'] &&  $itemSogumi['BlockKumiku'] == $dataDb['K_BlockKumiku'];
								}));

								if(count($sogumi) > 0) {
									$workItemIdOld = $sogumi[0]['WorkItemID'];
								}
							}

							$keyGroupSogumiNittei = $dataNittei['K_BlockKumiku'].'-'.$dataNittei['K_BlockName'];
							if(array_key_exists($keyGroupSogumiNittei, $grDataSogumiNittei)) {
								$groupSogumiNittei = $grDataSogumiNittei[$keyGroupSogumiNittei];
								$sogumiNittei = array_values(array_filter($groupSogumiNittei, function($itemSogumiNittei) use($dataNittei){
									return $itemSogumiNittei['BlockNameFull'] == $dataNittei['KBlockNameFull'] &&  $itemSogumiNittei['BlockKumiku'] == $dataNittei['K_BlockKumiku'];
								}));

								if(count($sogumiNittei) > 0) {
									$workItemIdNew = $sogumiNittei[0]['WorkItemID_'.$dataDb['K_Gen']];
								}
							}
						}
						$this->processDataLinked($dataNittei, $dataDb, true, $workItemIdOld, $workItemIdNew);

					}else {
						array_push($remainDataNittei, $ItemDataNittei);
					}
				}
				// B) in the same group still has data
				if(count($sameData) == 0) {
					if(array_key_exists($key, $grDataOlds)) {
						$remainData = $grDataOlds[$key];
						$this->dataLink($kind ,$remainData, $remainDataNittei, $grDataSogumiNittei, $grDataSogumi);
					}
				}else {
					if(count($sameData) != count($grDataOlds[$key])) {
						$remainData = array_values(array_filter($grDataOlds[$key], function($dataItem) use($sameData){
							return $sameData[0] != $dataItem;
						}));
						$this->dataLink($kind, $remainData, $remainDataNittei, $grDataSogumiNittei, $grDataSogumi);
					}else {
						$this->dataLink($kind, $remainData, $remainDataNittei, $grDataSogumiNittei, $grDataSogumi);
					}
				}
			}
		}
	}

	/**
	* get data delete method
	*
	* @param array grDataTosaiNittei
	* @param array grDataTosai
	* @param array grDataSogumiNittei
	* @param array grDataSogumi
	* @param array grDataKyokyuNittei
	* @param array grDataKyokyu
	* @return
	*
	* @create 2020/11/11　Cuong
	* @update 2020/11/25　Cuong
	*/

	private function getDataDelete($grDataTosaiNittei, $grDataTosai, $grDataSogumiNittei, $grDataSogumi, $grDataKyokyuNittei, $grDataKyokyu) {
		$this->checkGroupExistsInDataNittei($grDataTosai, $grDataTosaiNittei, config('system_const_schet.import_kind_tosai'));
		$this->checkGroupExistsInDataNittei($grDataSogumi, $grDataSogumiNittei, config('system_const_schet.import_kind_sogumi'));
		$this->checkGroupExistsInDataNittei($grDataKyokyu, $grDataKyokyuNittei, config('system_const_schet.import_kind_kyokyu'));
	}

	/**
	* get data change Gen using log method
	*
	* @param array groupDataNittei
	* @param array groupDataTosai
	* @param array groupDataSogumi
	* @param array groupDataKyokyu
	* @return
	*
	* @create 2020/11/11　Cuong
	* @update
	*/
	private function getDataGenChangeForLog($grDataTosaiNittei, $grDataTosai, $grDataSogumiNittei, $grDataSogumi, $grDataKyokyuNittei, $grDataKyokyu) {
		$this->processDataGenChangeForLog($grDataTosaiNittei, $grDataTosai);
		$this->processDataGenChangeForLog($grDataSogumiNittei, $grDataSogumi);
		$this->processDataGenChangeForLog($grDataKyokyuNittei, $grDataKyokyu);
	}

	/**
	* process data change gen for log method
	*
	* @param array grDataNitteis
	* @param array grDataOlds
	* @return
	*
	* @create 2020/11/25　Cuong
	* @update 2020/12/23　Cuong update condition data gen change
	*/
	private function processDataGenChangeForLog($grDataNitteis, $grDataOlds) {

		foreach($grDataNitteis as $group=>$dataNitteis) {
			if(array_key_exists($group, $grDataOlds)) {
				if(count($dataNitteis) == 2) {
					$genDataNittei = 'PS';
				}

				if(count($dataNitteis) == 1) {
					foreach($dataNitteis as $dataNittei) {
						$genDataNittei = $dataNittei['Gen'];
					}
				}

				$dataOlds = $grDataOlds[$group];
				if(count($dataOlds) == 2) {
					$genDataOld = 'PS';
				}
				if(count($dataOlds) == 1) {
					foreach($dataOlds as $dataOld) {
						$genDataOld = $dataOld['Gen'];
					}
				}

				$countGenNittei = strlen($genDataNittei);
				$countGenOld = strlen($genDataOld);

				if($countGenNittei == 1 && $countGenOld == 1) {
					if($genDataNittei != $genDataOld) {
						foreach($dataNitteis as $dataNittei) {
							$dataNittei['Flag'] = 'genLog';
							$dataNittei['messageLog'] = sprintf(config('message.msg_schet_log_003'), '舷', $genDataOld, $genDataNittei);
							array_push($this->mdataGenChangeLog, $dataNittei);
						}
					}
				}

				if($countGenNittei == 2 && $countGenOld == 1) {
					if(strpos($genDataNittei, $genDataOld) === false) {
						foreach($dataNitteis as $dataNittei) {
							$gencheck = $dataNittei['Gen'];
							if($gencheck == 'P') {
								$dataNittei['Flag'] = 'genLog';
								$dataNittei['messageLog'] = sprintf(config('message.msg_schet_log_003'), '舷', $genDataOld, $genDataNittei);
								array_push($this->mdataGenChangeLog, $dataNittei);
							}
						}
					}
				}

				if($countGenNittei == 1  && $countGenOld == 2) {
					if(strpos($genDataOld, $genDataNittei) === false) {
						foreach($dataNitteis as $dataNittei) {
							$dataNittei['Flag'] = 'genLog';
							$dataNittei['messageLog'] = sprintf(config('message.msg_schet_log_003'), '舷', $genDataOld, $genDataNittei);
							array_push($this->mdataGenChangeLog, $dataNittei);
						}
					}
				}
			}
		}
	}

	/**
	* process Data Linked method
	*
	* @param string gen
	* @param array data niitei
	* @param array data db
	* @param bolean isSameGen
	* @param array dataSogumiDBSei
	* @return
	*
	* @create 2020/11/11　Cuong
	* @update
	*/
	private function processDataLinked($data, $datadb, $isSameGen, $workItemIdOld = null, $workItemIdNew = null) {
		// 紐付けできたデータは以下の通り、変更点を調べる。
		$hasChangeDate = false;
		$gen = $datadb['Gen'];

		$data['isSameGen'] = $isSameGen;
		$data['workItemID_Link'] = $datadb['WorkItemID'];
		$data['BlockName_Link'] = $datadb['BlockName'];
		$data['BlockKumiku_Link'] = $datadb['BlockKumiku'];

		if($data['Kind'] == config('system_const_schet.import_kind_kyokyu')) {
			$data['KBlockName_Link'] = $datadb['K_BlockName'];
			$data['KBlockKumiku_Link'] = $datadb['K_BlockKumiku'];
		}
		// check change date
		if(is_null($data['SDate_'.$gen]) || is_null($data['EDate_'.$gen])) {
			$data['Flag'] = 'deleteTT';
			$data['messageLog'] = config('message.msg_schet_log_004');
			array_push($this->mdataHasChange, $data);
			$hasChangeDate = true;

		}else {
			if($data['SDate'] != $data['SDate_'.$gen]) {
				$changeSDate = array();
				$changeSDate = $data;

				$changeSDate['Flag'] = 'date';
				$changeSDate['messageLog'] = sprintf(config('message.msg_schet_log_003'),
				'開始日', Carbon::parse($changeSDate['SDate_'.$gen])->format('Y/m/d') ,
				Carbon::parse($changeSDate['SDate'])->format('Y/m/d'));

				array_push($this->mdataHasChange, $changeSDate);
				$hasChangeDate = true;
			}

			if($data['Kind'] == config('system_const_schet.import_kind_sogumi')) {
				if($data['EDate'] != $data['EDate_'.$gen]) {
					$changeEDate = array();
					$changeEDate = $data;

					$changeEDate['Flag'] = 'date';
					$changeEDate['messageLog'] = sprintf(config('message.msg_schet_log_003'),'終了日',
					Carbon::parse($changeEDate['EDate_'.$gen])->format('Y/m/d') ,
					Carbon::parse($changeEDate['EDate'])->format('Y/m/d'));

					array_push($this->mdataHasChange, $changeEDate);
					$hasChangeDate = true;
				}
			}

			// check change kyokyu
			if($data['Kind'] == config('system_const_schet.import_kind_kyokyu')) {
				$grOld = $datadb['K_BlockName'].'_'. $datadb['K_BlockKumiku'];
				$grNew = $data['KBlockNameFull'].'_'. $data['K_BlockKumiku'];
				if($workItemIdOld != $workItemIdNew || $grOld != $grNew) {
					$changeKyuKyu = array();
					$changeKyuKyu = $data;
					$changeKyuKyu['messageLog'] = sprintf(config('message.msg_schet_log_003'), '供給先',
					$changeKyuKyu['K_BlockName_'.$gen] . '(' . $changeKyuKyu['K_BlockKumiku_'.$gen] . ')' ,
					$changeKyuKyu['KBlockNameFull'] . '(' . $changeKyuKyu['K_BlockKumiku'] . ')');

					array_push($this->mdataHasChange, $changeKyuKyu);
					$hasChangeDate = true;
				}
			}

			if(!$hasChangeDate && !$isSameGen) {
				array_push($this->mdataHasChange, $data);
			}
		}

		if($data['Kind'] == config('system_const_schet.import_kind_tosai')) {
			if(!$hasChangeDate && $isSameGen) {
				array_push($this->mdataTosaiNotChange, $data);
			}
		}
	}

	/**
	* process Data Linked method
	*
	* @param array  remain data of DB
	* @param array  remain data Nittei
	* @param array  data Tsogumi
	* @return
	*
	* @create 2020/11/11　Cuong
	* @update
	*/
	private function dataLink($kind, $remainDB, $remainNittei, $grDataSogumiNittei, $grDataSogumi) {
		// 集約データに残っているものがあり、既存データに残っていない場合、そのデータは「追加」となる
		if(count($remainDB) == 0 && count($remainNittei) > 0) {
			foreach($remainNittei as $nittei) {
				array_push($this->mdataAdd, $nittei);
			}
		}

		// 既存データに残っているものがあり、集約データに残っていない場合、そのデータは「削除」となる
		if(count($remainDB) > 0 && count($remainNittei) == 0) {
			foreach($remainDB as $itemRemainDB) {
				$itemRemainDB['Kind'] = $kind;
				array_push($this->mdataDelete, $itemRemainDB);
			}
		}

		// 集約データと、既存データが1件ずつ残っている場合は、そのデータ同士を紐付ける。
		if(count($remainDB) == 1 && count($remainNittei) == 1) {
			$dataNittei = $remainNittei[0];
			foreach($remainDB as $reDB) {
				$dataDb = $reDB;
			}
			$workItemIdOld = null;
			$workItemIdNew = null;

			if($dataNittei['Kind'] == config('system_const_schet.import_kind_kyokyu')) {
				$keyGroupSogumi = $dataDb['K_BlockKumiku'].'-'.$dataDb['K_BlockNameNotGen'];
				if(array_key_exists($keyGroupSogumi, $grDataSogumi)) {
					$groupSogumi = $grDataSogumi[$keyGroupSogumi];
					$sogumi = array_values(array_filter($groupSogumi, function($itemSogumi) use($dataDb){
						return $itemSogumi['BlockName'] == $dataDb['K_BlockName'] &&  $itemSogumi['BlockKumiku'] == $dataDb['K_BlockKumiku'];
					}));

					if(count($sogumi) > 0) {
						$workItemIdOld = $sogumi[0]['WorkItemID'];
					}
				}

				$keyGroupSogumiNittei = $dataNittei['K_BlockKumiku'].'-'.$dataNittei['K_BlockName'];
				if(array_key_exists($keyGroupSogumiNittei, $grDataSogumiNittei)) {
					$groupSogumiNittei = $grDataSogumiNittei[$keyGroupSogumiNittei];
					$sogumiNittei = array_values(array_filter($groupSogumiNittei, function($itemSogumiNittei) use($dataNittei){
						return $itemSogumiNittei['BlockNameFull'] == $dataNittei['KBlockNameFull'] &&  $itemSogumiNittei['BlockKumiku'] == $dataNittei['K_BlockKumiku'];
					}));

					if(count($sogumiNittei) > 0) {
						$workItemIdNew = $sogumiNittei[0]['WorkItemID_'.$dataDb['K_Gen']];
					}
				}
			}
			$this->processDataLinked($dataNittei, $dataDb, false, $workItemIdOld, $workItemIdNew);
		}

		// 集約データに複数残っており、既存データが1件残っている場合、集約データのP舷と既存データを紐付ける
		if(count($remainDB) == 1 && count($remainNittei) > 1) {

			$dataGenP = array_values(array_filter($remainNittei, function($itemNittei){
				return $itemNittei['Gen'] == 'P';
			}));

			$dataNotGenP = array_values(array_filter($remainNittei, function($itemNittei){
				return $itemNittei['Gen'] != 'P';
			}));

			if(count($dataNotGenP) > 0) {
				array_push($this->mdataAdd, $dataNotGenP[0]);
			}

			$dataNittei = $dataGenP[0];
			foreach($remainDB as $reDB) {
				$dataDb = $reDB;
			}
			$workItemIdOld = null;
			$workItemIdNew = null;

			if($dataNittei['Kind'] == config('system_const_schet.import_kind_kyokyu')) {
				$keyGroupSogumi = $dataDb['K_BlockKumiku'].'-'.$dataDb['K_BlockNameNotGen'];
				if(array_key_exists($keyGroupSogumi, $grDataSogumi)) {
					$groupSogumi = $grDataSogumi[$keyGroupSogumi];
					$sogumi = array_values(array_filter($groupSogumi, function($itemSogumi) use($dataDb){
						return $itemSogumi['BlockName'] == $dataDb['K_BlockName'] &&  $itemSogumi['BlockKumiku'] == $dataDb['K_BlockKumiku'];
					}));

					if(count($sogumi) > 0) {
						$workItemIdOld = $sogumi[0]['WorkItemID'];
					}
				}

				$keyGroupSogumiNittei = $dataNittei['K_BlockKumiku'].'-'.$dataNittei['K_BlockName'];
				if(array_key_exists($keyGroupSogumiNittei, $grDataSogumiNittei)) {
					$groupSogumiNittei = $grDataSogumiNittei[$keyGroupSogumiNittei];
					$sogumiNittei = array_values(array_filter($groupSogumiNittei, function($itemSogumiNittei) use($dataNittei){
						return $itemSogumiNittei['BlockNameFull'] == $dataNittei['KBlockNameFull'] &&  $itemSogumiNittei['BlockKumiku'] == $dataNittei['K_BlockKumiku'];
					}));

					if(count($sogumiNittei) > 0) {
						$workItemIdNew = $sogumiNittei[0]['WorkItemID_'.$dataDb['K_Gen']];
					}
				}
			}
			$this->processDataLinked($dataNittei, $dataDb, false, $workItemIdOld, $workItemIdNew);
		}

		// 既存データに複数残っており、集約データが1件残っている場合、既存データのP舷と集約データを紐付ける。
		if(count($remainDB) > 1 && count($remainNittei) == 1) {
			$gen = 'P';
			$dataGenP = array_values(array_filter($remainDB, function($remainDBItem){
				return $remainDBItem['Gen'] == 'P';
			}));

			$dataNotGenP = array_values(array_filter($remainDB, function($remainDBItem){
				return $remainDBItem['Gen'] != 'P';
			}));


			if(count($dataNotGenP) > 0) {
				$dataNotGenP[0]['Kind'] = $kind;
				array_push($this->mdataDelete, $dataNotGenP[0]);
			}

			$dataNittei = $remainNittei[0];
			$dataDb = $dataGenP[0];
			$workItemIdOld = null;
			$workItemIdNew = null;

			if($dataNittei['Kind'] == config('system_const_schet.import_kind_kyokyu')) {
				$keyGroupSogumi = $dataDb['K_BlockKumiku'].'-'.$dataDb['K_BlockNameNotGen'];
				if(array_key_exists($keyGroupSogumi, $grDataSogumi)) {
					$groupSogumi = $grDataSogumi[$keyGroupSogumi];
					$sogumi = array_values(array_filter($groupSogumi, function($itemSogumi) use($dataDb){
						return $itemSogumi['BlockName'] == $dataDb['K_BlockName'] &&  $itemSogumi['BlockKumiku'] == $dataDb['K_BlockKumiku'];
					}));

					if(count($sogumi) > 0) {
						$workItemIdOld = $sogumi[0]['WorkItemID'];
					}
				}

				$keyGroupSogumiNittei = $dataNittei['K_BlockKumiku'].'-'.$dataNittei['K_BlockName'];
				if(array_key_exists($keyGroupSogumiNittei, $grDataSogumiNittei)) {
					$groupSogumiNittei = $grDataSogumiNittei[$keyGroupSogumiNittei];
					$sogumiNittei = array_values(array_filter($groupSogumiNittei, function($itemSogumiNittei) use($dataNittei){
						return $itemSogumiNittei['BlockNameFull'] == $dataNittei['KBlockNameFull'] &&  $itemSogumiNittei['BlockKumiku'] == $dataNittei['K_BlockKumiku'];
					}));

					if(count($sogumiNittei) > 0) {
						$workItemIdNew = $sogumiNittei[0]['WorkItemID_'.$dataDb['K_Gen']];
					}
				}
			}
			$this->processDataLinked($dataNittei, $dataDb, false, $workItemIdOld, $workItemIdNew);
		}
	}

	/**
	* insert data to T_importLog method
	*
	* @param int  historyID
	* @param int  ID
	* @param array  data
	* @param string  messageLog
	* @return
	*
	* @create 2020/11/11　Cuong
	* @update
	*/
	private function insertImportLog($historyID, $id, $data, $messageLog) {
		$objImportLog = new T_ImportLog;
		$objImportLog->HistoryID = $historyID;
		$objImportLog->ID = $id;
		$objImportLog->Category = $this->getKindName($data['Kind']);
		$objImportLog->BlockName = $data['BlockNameFull'];
		$objImportLog->BlockKumiku = $data['BlockKumiku'];
		$objImportLog->Log = $messageLog;
		$objImportLog->save();
	}

	/**
	* process Data Linked method
	*
	* @param array  remain data
	* @return array remain data Nittei
	*
	* @create 2020/11/11　Cuong
	* @update
	*/
	private function updateFlagImportHistory($id, $statusFlag) {
		T_ImportHistory::where('ID',$id)
						->update(array('StatusFlag' =>$statusFlag));
	}

	/**
	* process Aggregate data before register method
	*
	* @param
	* @return array Aggregate data
	*
	* @create 2020/11/11　Cuong
	* @update 2020/01/07　Cuong remove duplicate value 
	*/
	private function processDataBeforeInsert() {
		
		$resData = array();	 //array data result
		$dataAll = array_merge($this->mdataTosaiNotChange, $this->mdataAdd, $this->mdataHasChange);		//data merge of data add and data change

		// get data tosai in dataAll
		$dataTosais = array_filter($dataAll, function($data) {
			return $data['Kind'] == config('system_const_schet.import_kind_tosai');
		});
		$dataTosais = $this->removeDuplicateVal($dataTosais);

		// get data sogumi in dataAll
		$dataSogumis = array_filter($dataAll, function($data) {
			return $data['Kind'] == config('system_const_schet.import_kind_sogumi');
		});
		$dataSogumis = $this->removeDuplicateVal($dataSogumis);

		// get data kyokyu in dataAll
		$dataKyokyus = array_filter($dataAll, function($data) {
			return $data['Kind'] == config('system_const_schet.import_kind_kyokyu');
		});
		$dataKyokyus = $this->removeDuplicateVal($dataKyokyus);

		// group the data into one 塊
		if(count($dataTosais) > 0) {
			foreach($dataTosais as $tosai) {
				$blockNameFull = $tosai['BlockNameFull'];
				$blockKumiku = $tosai['BlockKumiku'];
				$sogumis = array_filter($dataSogumis, function($data) use ($blockNameFull, $blockKumiku){
					return $blockKumiku ==  $data['BlockKumiku']  && $blockNameFull == $data['BlockNameFull'];
				});

				if(count($sogumis) > 0) {
					foreach($sogumis as $sogumi) {
						$kBlockNameFull = $sogumi['BlockNameFull'];
						$kBlockKumiku = $sogumi['BlockKumiku'];
						$kyokyus = array_filter($dataKyokyus, function($data) use ($kBlockNameFull, $kBlockKumiku){
							$full = $data['KBlockNameFull'];
							return $kBlockKumiku ==  $data['K_BlockKumiku']  && $kBlockNameFull == $data['KBlockNameFull'];
						});
						if(count($kyokyus) == 0) {
							array_push($resData, array('tosai'=>$tosai, 'sogumi'=>$sogumi));
						}else {
							array_push($resData, array('tosai'=>$tosai, 'sogumi'=>$sogumi, 'kyokyu'=>$kyokyus));
							foreach($kyokyus as $key=>$value) {
								unset($dataKyokyus[$key]);
							}
						}
					}

					foreach($sogumis as $key=>$value) {
						unset($dataSogumis[$key]);
					}

				}else {
					array_push($resData, array('tosai'=>$tosai));
				}
			}
		}

		if(count($dataSogumis) > 0) {
			foreach($dataSogumis as $keySogumi=>$valueSogumi) {
				$kBlockNameFull = $valueSogumi['BlockNameFull'];
				$kBlockKumiku = $valueSogumi['BlockKumiku'];
				$arrKyokyu = array_filter($dataKyokyus, function($data) use ($kBlockNameFull, $kBlockKumiku){
					$full = $data['KBlockNameFull'];
					return $kBlockKumiku ==  $data['K_BlockKumiku']  && $kBlockNameFull == $data['KBlockNameFull'];
				});
				if(count($arrKyokyu) == 0) {
					array_push($resData, array('sogumi'=>$valueSogumi));
				}else {
					array_push($resData, array('sogumi'=>$valueSogumi, 'kyokyu'=>$arrKyokyu));
					foreach($arrKyokyu as $key=>$value) {
						unset($dataKyokyus[$key]);
					}
				}
			}
		}

		if(count($dataKyokyus) > 0) {
			foreach($dataKyokyus as $kyokyu){
				$arrData = array_filter($dataKyokyus, function($data) use ($kyokyu){
					return $kyokyu['K_BlockKumiku'] ==  $data['K_BlockKumiku']  && $kyokyu['KBlockNameFull'] == $data['KBlockNameFull'];
				});

				if(count($arrData) > 0) {
					array_push($resData, array('kyokyu'=>$arrData));
					foreach($arrData as $key=>$value) {
						unset($dataKyokyus[$key]);
					}
				}
			}
		}

		return $resData;
	}

	/**
	* remove duplicate value method
	*
	* @param array $data
	* @return array
	*
	* @create 2021/01/11　Cuong
	* @update
	*/
	private function removeDuplicateVal($data = array()) {
		$arrKey = array();
		foreach($data as $key=>$val) {
			$keyData = $val['BlockNameFull'].'_'.$val['BlockKumiku'];
			if(in_array($keyData, $arrKey)) {
				unset($data[$key]);
			}
			array_push($arrKey, $keyData);
		}
		return $data;
	}

	/**
	* register data to ImportLog table
	*
	* @param int MaxID of T_historyimport
	* @return array data
	*
	* @create 2020/11/11　Cuong
	* @update
	*/
	private function registerImportLog($MaxID) {
		DB::transaction(function() use($MaxID){
			$index = 1;
			$historyID = $MaxID;
			$dataChangeLogs = array_filter($this->mdataHasChange, function($item){
				return array_key_exists('messageLog', $item);
			});

			if(count($dataChangeLogs) > 0) {
				foreach ($dataChangeLogs as $key => $value) {
					$messageLog = $value['messageLog'];
					$this->insertImportLog($historyID, $index, $value, $messageLog);
					$index++;
				}
			}
			if(count($this->mdataAdd) > 0) {
				foreach ($this->mdataAdd as $key => $value) {
					$messageLog = config('message.msg_schet_log_001');
					$this->insertImportLog($historyID, $index, $value, $messageLog);
					$index++;
				}
			}
			if(count($this->mdataDelete) > 0) {
				foreach ($this->mdataDelete as $key => $value) {
					$messageLog = config('message.msg_schet_log_002');
					$value['BlockNameFull'] = $value['BlockName'];
					$this->insertImportLog($historyID, $index, $value, $messageLog);
					$index++;
				}
			}

			if(count($this->mdataGenChangeLog) > 0) {
				foreach ($this->mdataGenChangeLog as $key => $value) {
					$messageLog = $value['messageLog'];
					$this->insertImportLog($historyID, $index, $value, $messageLog);
					$index++;
				}
			}
		});
	}

	const TIME_TRACKER_ERROR = 'TimeNXError';

	/**
	* register Aggregate data method
	*
	* @param request request
	* @param menuInfo
	* @param int MaxID
	* @param int projectID
	* @param int orderNo
	* @param int orderRoot
	* @param int dataCalendar
	* @return mix (array/string)
	*
	* @create 2020/11/11　Cuong
	* @update
	*/
	private function registerAggregateData($request, $menuInfo, $MaxID, $projectID, $orderNo, $orderRoot, $dataCalendar) {
		$timeTrackerSchet = new TimeTrackerFuncSchet();
		$urlErr = url('/');
		$urlErr .= '/' . $menuInfo->KindURL;
		$urlErr .= '/' . $menuInfo->MenuURL;
		$urlErr .= '/index';
		$urlErr .= '?cmn1=' . valueUrlEncode($menuInfo->KindID);
		$urlErr .= '&cmn2=' . valueUrlEncode($menuInfo->MenuID);
		$urlErr .= '&val1=' . $request->val1;
		$urlErr .= '&val2=' . $request->val2;
		$urlErr .= '&val5=' . $request->val5;
		$urlErr .= '&err1=' . valueUrlEncode(config('message.msg_cmn_db_016'));

		$dataGeneral = $this->processDataBeforeInsert();
		foreach($dataGeneral as $dataItem) {
			$workItemIDParentTosai = null;
			$workItemIDKoteiTosai = null;
			$workItemIDParentSogumi = null;
			$workItemIDKoteiSogumi = null;
			$workItemIDParentKyokyu = null;
			$workItemIDKoteiKyokyu = null;

			foreach($dataItem as $key=>$data) {
				if($key == 'tosai') {
					try {

						$flagCheckErr = 0;
						// 搭載日程のデータを登録する。(追加、変更)
						if(array_key_exists('isSameGen',$data) && $data['isSameGen'] && !array_key_exists('messageLog',$data)) {
							$arrKoteiItem = array();
							$arrKoteiItem['parentid'] = $orderRoot;
							$arrKoteiItem['blockname'] = $data['BlockNameFull'];
							$arrKoteiItem['kumiku'] = $data['BlockKumiku'];
							$arrKoteiItem['name'] = config('system_const_timetracker.koteiname_schet_tosai');
							$arrKoteiItem['sdate'] = $data['SDate'];
							$arrKoteiItem['edate'] = $data['SDate'];
							$arrKoteiItem['workitemid'] = $data['workItemID_Link'];
							$arrKoteiItem['parentflag'] = true;

							// TimeTrackerNXに登録を行う
							$resultTT = $timeTrackerSchet->updateKotei ($projectID, $orderNo, $arrKoteiItem, $dataCalendar); //array('parentid'=>WorkItemID cha, 'workitemid'=>WorkItemID cua kotei) //err: mess
							if(is_string($resultTT)) {
								/* TimeTrackerNX Error */
								$idMax = T_ImportLog::where('HistoryID','=',$MaxID)->orderBy('ID', 'DESC')->first();
								$index = (!is_null($idMax) > 0) ? ($idMax->ID + 1) : 1;
								$messageLog = $resultTT;
								$this->insertImportLog($MaxID, $index, $data, $messageLog);
								throw new CustomException(self::TIME_TRACKER_ERROR);
							}else {
								$workItemIDParentTosai = $resultTT['parentid'];
								$workItemIDKoteiTosai = $resultTT['workitemid'];
							}
						}elseif(!array_key_exists('isSameGen',$data) && !array_key_exists('messageLog',$data)) {
							// 追加の場合
							$arrKoteiItem = array();
							$arrKoteiItem['parentid'] = $orderRoot;
							$arrKoteiItem['blockname'] = $data['BlockNameFull'];
							$arrKoteiItem['kumiku'] = $data['BlockKumiku'];
							$arrKoteiItem['name'] = config('system_const_timetracker.koteiname_schet_tosai');
							$arrKoteiItem['sdate'] = $data['SDate'];
							$arrKoteiItem['edate'] = $data['SDate'];
							$arrKoteiItem['parentflag'] = true;

							// TimeTrackerNXに登録を行う
							$resultTT = $timeTrackerSchet->insertKotei($projectID, $orderNo , $arrKoteiItem, $dataCalendar); //array('parentid'=>WorkItemID cha, 'workitemid'=>WorkItemID cua kotei) //err: mess

							if(is_string($resultTT)) {
								/* TimeTrackerNX Error*/
								$idMax = T_ImportLog::where('HistoryID','=',$MaxID)->orderBy('ID', 'DESC')->first();
								$index = (!is_null($idMax) > 0) ? ($idMax->ID + 1) : 1;
								$messageLog = $resultTT;
								$this->insertImportLog($MaxID, $index, $data, $messageLog);
								throw new CustomException(self::TIME_TRACKER_ERROR);
							}else {
								$workItemIDParentTosai = $resultTT['parentid'];
								$workItemIDKoteiTosai = $resultTT['workitemid'];

								// データベースに登録を行う
								$objTosai = new T_Tosai;
								$objTosai->ProjectID = $projectID;
								$objTosai->OrderNo = $orderNo;
								$objTosai->BlockName = $data['BlockNameFull'];
								$objTosai->BlockKumiku = $data['BlockKumiku'];
								$objTosai->WorkItemID = $workItemIDKoteiTosai;
								$objTosai->Save();
							}

						}else {
							// 変更の場合
							$flagCheckErr = 1;

							$arrKoteiItem = array();
							$arrKoteiItem['parentid'] = $orderRoot;
							$arrKoteiItem['blockname'] = $data['BlockNameFull'];
							$arrKoteiItem['kumiku'] = $data['BlockKumiku'];
							$arrKoteiItem['name'] = config('system_const_timetracker.koteiname_schet_tosai');
							$arrKoteiItem['sdate'] = $data['SDate'];
							$arrKoteiItem['edate'] = $data['SDate'];
							$arrKoteiItem['workitemid'] = $data['workItemID_Link'];
							$arrKoteiItem['parentflag'] = true;

							// TimeTrackerNXに登録を行う
							$resultTT = $timeTrackerSchet->updateKotei ($projectID, $orderNo, $arrKoteiItem, $dataCalendar); //array('parentid'=>WorkItemID cha, 'workitemid'=>WorkItemID cua kotei) //err: mess
							if(is_string($resultTT)) {
								/* TimeTrackerNX Error */
								$idMax = T_ImportLog::where('HistoryID','=',$MaxID)->orderBy('ID', 'DESC')->first();
								$index = (!is_null($idMax) > 0) ? ($idMax->ID + 1) : 1;
								$messageLog = $resultTT;
								$this->insertImportLog($MaxID, $index, $data, $messageLog);
								throw new CustomException(self::TIME_TRACKER_ERROR);
							}else {
								$workItemIDParentTosai = $resultTT['parentid'];
								$workItemIDKoteiTosai = $resultTT['workitemid'];

								if(!$data['isSameGen']) {
									// ブロック名が違う場合
									// 既存データを削除して
									$res = T_Tosai::where('ProjectID','=',$projectID)
													->where('OrderNo','=',$orderNo)
													->where('BlockName','=',$data['BlockName_Link'])
													->where('BlockKumiku','=',$data['BlockKumiku_Link'])
													->delete();
									// 追加する
									$objTosai = new T_Tosai;
									$objTosai->ProjectID = $projectID;
									$objTosai->OrderNo = $orderNo;
									$objTosai->BlockName = $data['BlockNameFull'];
									$objTosai->BlockKumiku = $data['BlockKumiku'];
									$objTosai->WorkItemID = $workItemIDKoteiTosai;
									$objTosai->Save();
								}else {
									// ブロック名が同じ場合
									if($data['workItemID_Link'] != $workItemIDKoteiTosai) {
										// update
										$objTosai = T_Tosai::where('ProjectID','=',$projectID)
												->where('OrderNo','=',$orderNo)
												->where('BlockName','=',$data['BlockNameFull'])
												->where('BlockKumiku','=',$data['BlockKumiku'])
												->update(array('WorkItemID' =>$workItemIDKoteiTosai));
									}
								}

							}

						}
					} catch (CustomException $e) {
						// Update flag 搭載日程取込履歴[T_ImportHistory]
						$this->updateFlagImportHistory($MaxID,config('system_const_schet.schet_import_status_error'));
						if($e->getMessage() != self::TIME_TRACKER_ERROR &&
						( $flagCheckErr = 0 || ($flagCheckErr = 1 && $data['workItemID_Link'] != $workItemIDKoteiTosai) )) {
							$timeTrackerSchet->deleteKotei(array(['workitemid'=>$workItemIDParentTosai, 'parentflag'=>false]));
						}
						// 排他制御を解除する
						$this->deleteLock($menuInfo->KindID, config('system_const_schet.syslock_menuid_schet'), $menuInfo->SessionID, $projectID);
						return $urlErr;
					}
				}

				if($key == 'sogumi') {
					try {
						$flagCheckErr = 0;
						if(!array_key_exists('isSameGen',$data) && !array_key_exists('messageLog',$data)) {
							// 追加の場合
							$arrKoteiItem = array();
							$arrKoteiItem['parentid'] = $workItemIDParentTosai;
							$arrKoteiItem['blockname'] = $data['BlockNameFull'];
							$arrKoteiItem['kumiku'] = $data['BlockKumiku'];
							$arrKoteiItem['name'] = config('system_const_timetracker.koteiname_schet_sogumi');
							$arrKoteiItem['sdate'] = $data['SDate'];
							$arrKoteiItem['edate'] = $data['EDate'];
							$arrKoteiItem['parentflag'] = false;

							// TimeTrackerNXに登録を行う
							$resultTT = $timeTrackerSchet->insertKotei($projectID, $orderNo , $arrKoteiItem, $dataCalendar); //array('parentid'=>WorkItemID, 'workitemid'=>WorkItemID kotei) //err: mess
							if(is_string($resultTT)) {
								/* TimeTrackerNX Error*/
								$idMax = T_ImportLog::where('HistoryID','=',$MaxID)->orderBy('ID', 'DESC')->first();
								$index = (!is_null($idMax) > 0) ? ($idMax->ID + 1) : 1;
								$messageLog = $resultTT;
								$this->insertImportLog($MaxID, $index, $data, $messageLog);
								throw new CustomException(self::TIME_TRACKER_ERROR);
							}else {
								$workItemIDParentSogumi = $resultTT['parentid'];
								$workItemIDKoteiSogumi = $resultTT['workitemid'];
								// データベースに登録を行う
								$objSogumi = new T_Sogumi;
								$objSogumi->ProjectID = $projectID;
								$objSogumi->OrderNo = $orderNo;
								$objSogumi->BlockName = $data['BlockNameFull'];
								$objSogumi->BlockKumiku = $data['BlockKumiku'];
								$objSogumi->WorkItemID = $workItemIDKoteiSogumi;
								$objSogumi->Save();
							}
						}else {
							$flagCheckErr = 1;
							// 変更の場合
							$arrKoteiItem = array();
							$arrKoteiItem['parentid'] = $workItemIDParentTosai;
							$arrKoteiItem['blockname'] = $data['BlockNameFull'];
							$arrKoteiItem['kumiku'] = $data['BlockKumiku'];
							$arrKoteiItem['name'] = config('system_const_timetracker.koteiname_schet_sogumi');
							$arrKoteiItem['sdate'] = $data['SDate'];
							$arrKoteiItem['edate'] = $data['EDate'];
							$arrKoteiItem['workitemid'] = $data['workItemID_Link'];
							$arrKoteiItem['parentflag'] = false;

							// TimeTrackerNXに登録を行う
							$resultTT = $timeTrackerSchet->updateKotei($projectID, $orderNo, $arrKoteiItem, $dataCalendar); //array('parentid'=>WorkItemID , 'workitemid'=>WorkItemID  kotei) //err: mess
							if(is_string($resultTT)) {
								/* TimeTrackerNX Error*/
								$idMax = T_ImportLog::where('HistoryID','=',$MaxID)->orderBy('ID', 'DESC')->first();
								$index = (!is_null($idMax) > 0) ? ($idMax->ID + 1) : 1;
								$messageLog = $resultTT;
								$this->insertImportLog($MaxID, $index, $data, $messageLog);
								throw new CustomException(self::TIME_TRACKER_ERROR);
							}else {
								$workItemIDParentSogumi = $resultTT['parentid'];
								$workItemIDKoteiSogumi = $resultTT['workitemid'];
								if(!$data['isSameGen']) {
									// ブロック名が違う場合
									// 既存データを削除して
									$res = T_Sogumi::where('ProjectID','=',$projectID)
													->where('OrderNo','=',$orderNo)
													->where('BlockName','=',$data['BlockName_Link'])
													->where('BlockKumiku','=',$data['BlockKumiku_Link'])
													->delete();
									// データベースに登録を行う
									$objSogumi = new T_Sogumi;
									$objSogumi->ProjectID = $projectID;
									$objSogumi->OrderNo = $orderNo;
									$objSogumi->BlockName = $data['BlockNameFull'];
									$objSogumi->BlockKumiku = $data['BlockKumiku'];
									$objSogumi->WorkItemID = $workItemIDKoteiSogumi;
									$objSogumi->Save();
								}else {
									// ブロック名が同じ場合
									if($data['workItemID_Link'] != $workItemIDKoteiSogumi) {
										//  update
										$objSogumi = T_Sogumi::where('ProjectID','=',$projectID)
												->where('OrderNo','=',$orderNo)
												->where('BlockName','=',$data['BlockNameFull'])
												->where('BlockKumiku','=',$data['BlockKumiku'])
												->update(array('WorkItemID' =>$workItemIDKoteiSogumi));
									}
								}
							}

						}
					} catch (CustomException $e) {
						// Update flag 搭載日程取込履歴[T_ImportHistory]
						$this->updateFlagImportHistory($MaxID,config('system_const_schet.schet_import_status_error'));
						if($e->getMessage() != self::TIME_TRACKER_ERROR &&
						( $flagCheckErr = 0 || ($flagCheckErr = 1 && $data['workItemID_Link'] != $workItemIDKoteiSogumi) )) {
							$timeTrackerSchet->deleteKotei(array(['workitemid'=>$workItemIDParentTosai, 'parentflag'=>false]));
						}
						// 排他制御を解除する
						$this->deleteLock($menuInfo->KindID, config('system_const_schet.syslock_menuid_schet'), $menuInfo->SessionID, $projectID);
						return $urlErr;
					}
				}

				if($key == 'kyokyu') {
					try {
						foreach($data as $kyokyu) {
							$flagCheckErr = 0;

							if(!array_key_exists('isSameGen',$kyokyu) && !array_key_exists('messageLog',$kyokyu)) {
								// 追加の場合
								$arrKoteiItem = array();
								$arrKoteiItem['parentid'] = $workItemIDParentTosai;
								$arrKoteiItem['blockname'] = $kyokyu['BlockNameFull'];
								$arrKoteiItem['kumiku'] = $kyokyu['BlockKumiku'];
								$arrKoteiItem['name'] = config('system_const_timetracker.koteiname_schet_kyokyu');
								$arrKoteiItem['sdate'] = $kyokyu['SDate'];
								$arrKoteiItem['edate'] = $kyokyu['SDate'];
								$arrKoteiItem['parentflag'] = true;

								// TimeTrackerNXに登録を行う
								$resultTT = $timeTrackerSchet->insertKotei($projectID, $orderNo, $arrKoteiItem, $dataCalendar); //array('parentid'=>WorkItemID parent, 'workitemid'=>WorkItemID cua kotei) //err: mess
								if(is_string($resultTT)) {
									// TimeTrackerNX error
									$idMax = T_ImportLog::where('HistoryID','=',$MaxID)->orderBy('ID', 'DESC')->first();
									$index = (!is_null($idMax) > 0) ? ($idMax->ID + 1) : 1;
									$messageLog = $resultTT;
									$this->insertImportLog($MaxID, $index, $kyokyu, $messageLog);
									throw new CustomException(self::TIME_TRACKER_ERROR);
								}else {
									$workItemIDParentKyokyu = $resultTT['parentid'];
									$workItemIDKoteiKyokyu = $resultTT['workitemid'];

									// データベースに登録を行う
									$objKyokyu = new T_Kyokyu;
									$objKyokyu->ProjectID = $projectID;
									$objKyokyu->OrderNo = $orderNo;
									$objKyokyu->BlockName = $kyokyu['BlockNameFull'];
									$objKyokyu->BlockKumiku = $kyokyu['BlockKumiku'];
									$objKyokyu->K_BlockName = $kyokyu['KBlockNameFull'];
									$objKyokyu->K_BlockKumiku = $kyokyu['K_BlockKumiku'];
									$objKyokyu->WorkItemID = $workItemIDKoteiKyokyu;
									$objKyokyu->Save();
								}
							}else {
								$flagCheckErr = 1;
								// 変更の場合
								$arrKoteiItem = array();
								$arrKoteiItem['parentid'] = $workItemIDParentTosai;
								$arrKoteiItem['blockname'] = $kyokyu['BlockNameFull'];
								$arrKoteiItem['kumiku'] = $kyokyu['BlockKumiku'];
								$arrKoteiItem['name'] = config('system_const_timetracker.koteiname_schet_kyokyu');
								$arrKoteiItem['sdate'] = $kyokyu['SDate'];
								$arrKoteiItem['edate'] = $kyokyu['SDate'];
								$arrKoteiItem['workitemid'] = $kyokyu['workItemID_Link'];
								$arrKoteiItem['parentflag'] = true;

								// TimeTrackerNXに登録を行う
								$resultTT = $timeTrackerSchet->updateKotei($projectID, $orderNo, $arrKoteiItem, $dataCalendar); //array('parentid'=>WorkItemID parent, 'workitemid'=>WorkItemID cua kotei) //err: mess
								if(is_string($resultTT)) {
									// TimeTrackerNX error
									$idMax = T_ImportLog::where('HistoryID','=',$MaxID)->orderBy('ID', 'DESC')->first();
									$index = (!is_null($idMax) > 0) ? ($idMax->ID + 1) : 1;
									$messageLog = $resultTT;
									$this->insertImportLog($MaxID, $index, $kyokyu, $messageLog);
									throw new CustomException(self::TIME_TRACKER_ERROR);
								}else {
									$workItemIDParentKyokyu = $resultTT['parentid'];
									$workItemIDKoteiKyokyu = $resultTT['workitemid'];

									if(!$kyokyu['isSameGen']) {
										// ブロック名が違う場合
									// 既存データを削除して
										$res = T_Kyokyu::where('ProjectID','=',$projectID)
														->where('OrderNo','=',$orderNo)
														->where('BlockName','=',$kyokyu['BlockName_Link'])
														->where('BlockKumiku','=',$kyokyu['BlockKumiku_Link'])
														->delete();
											// データベースに登録を行う
										$objKyokyu = new T_Kyokyu;
										$objKyokyu->ProjectID = $projectID;
										$objKyokyu->OrderNo = $orderNo;
										$objKyokyu->BlockName = $kyokyu['BlockNameFull'];
										$objKyokyu->BlockKumiku = $kyokyu['BlockKumiku'];
										$objKyokyu->K_BlockName = $kyokyu['KBlockNameFull'];
										$objKyokyu->K_BlockKumiku = $kyokyu['K_BlockKumiku'];
										$objKyokyu->WorkItemID = $workItemIDKoteiKyokyu;
										$objKyokyu->Save();
									}else {
										// ブロック名が同じ場合
										if($kyokyu['workItemID_Link'] != $workItemIDKoteiKyokyu ||
										!($kyokyu['K_BlockName'] == $kyokyu['KBlockName_Link']
										&& $kyokyu['K_BlockKumiku'] == $kyokyu['KBlockKumiku_Link'])) {
											// update
											$objKyokyu = T_Kyokyu::where('ProjectID','=',$projectID)
													->where('OrderNo','=',$orderNo)
													->where('BlockName','=',$kyokyu['BlockNameFull'])
													->where('BlockKumiku','=',$kyokyu['BlockKumiku'])
													->update(
														array(
															'WorkItemID' =>$workItemIDKoteiKyokyu,
															'K_BlockName'=>$kyokyu['KBlockNameFull'],
															'K_BlockKumiku'=>$kyokyu['K_BlockKumiku']
													));
										}
									}

								}
							}
						}
					} catch (CustomException $e) {
						// Update flag 搭載日程取込履歴[T_ImportHistory]
						$this->updateFlagImportHistory($MaxID,config('system_const_schet.schet_import_status_error'));
						if($e->getMessage() != self::TIME_TRACKER_ERROR &&
						( $flagCheckErr = 0 || ($flagCheckErr = 1 && $kyokyu['workItemID_Link'] != $workItemIDKoteiKyokyu) )) {
							$timeTrackerSchet->deleteKotei(array(['workitemid'=>$workItemIDParentTosai, 'parentflag'=>false]));
						}
						// 排他制御を解除する
						$this->deleteLock($menuInfo->KindID, config('system_const_schet.syslock_menuid_schet'), $menuInfo->SessionID, $projectID);
						return $urlErr;
					}
				}
			}
		}

		// 削除処理を行う
		if(count($this->mdataDelete) > 0) {
			foreach($this->mdataDelete as $dataDelete) {
				if($dataDelete['Kind'] == config('system_const_schet.import_kind_tosai')) {
					$timeTrackerSchet->deleteKotei(array(['workitemid'=>$dataDelete['WorkItemID'], 'parentflag'=>true]));
					$resultTosai = T_Tosai::where('ProjectID','=',$projectID)
													->where('OrderNo','=',$orderNo)
													->where('BlockName','=',$dataDelete['BlockName'])
													->where('BlockKumiku','=',$dataDelete['BlockKumiku'])
													->delete();
				}
				if($dataDelete['Kind'] == config('system_const_schet.import_kind_sogumi')) {
					$timeTrackerSchet->deleteKotei(array(['workitemid'=>$dataDelete['WorkItemID'], 'parentflag'=>false]));
					$resultSogumi = T_Sogumi::where('ProjectID','=',$projectID)
													->where('OrderNo','=',$orderNo)
													->where('BlockName','=',$dataDelete['BlockName'])
													->where('BlockKumiku','=',$dataDelete['BlockKumiku'])
													->delete();
				}
				if($dataDelete['Kind'] == config('system_const_schet.import_kind_kyokyu')) {
					$timeTrackerSchet->deleteKotei(array(['workitemid'=>$dataDelete['WorkItemID'], 'parentflag'=>true]));
					$resultSogumi = T_Kyokyu::where('ProjectID','=',$projectID)
													->where('OrderNo','=',$orderNo)
													->where('BlockName','=',$dataDelete['BlockName'])
													->where('BlockKumiku','=',$dataDelete['BlockKumiku'])
													->delete();
				}
			}
		}
		return array();
	}

}
