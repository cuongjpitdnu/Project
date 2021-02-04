<?php
/*
 * @ExcelImportController.php
 * 日程取込の各画面コントローラーファイル
 *
 * @create 2020/10/13 Dung
 *
 * @update 2021/2/3 update Rev7
 */

namespace App\Http\Controllers\Schem;

use App\Http\Controllers\Controller;
use App\Http\Requests\Schem\ExcelImportOutputRequest;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use Illuminate\Database\QueryException;
use Illuminate\Pagination\LengthAwarePaginator;
use App\Librarys\FuncCommon;
use App\Librarys\MenuInfo;
use App\Librarys\MissingUpdateException;
use App\Librarys\TimeTrackerFuncSchem;
use App\Librarys\CustomException;
use App\Librarys\TimeTrackerCommon;
use App\Models\MstProject;
use App\Models\MstOrderNo;
use App\Models\Cyn_TosaiData;
use App\Models\Cyn_mstKotei_STR_P;
use App\Models\Cyn_mstKotei_STR_C;
use App\Models\Cyn_mstKotei;
use App\Models\Cyn_BlockKukaku;
use App\Models\Cyn_C_BlockKukaku;
use App\Models\Cyn_Plan;
use App\Models\Cyn_C_Plan;
use App\Models\mstFloor;
use App\Models\mstBDCode;
use App\Models\Cyn_Temp_Excel_LogData;
use App\Models\WorkItemIDList;
use App\Models\Cyn_Excel_History;
use App\Models\Cyn_Excel_LogData;
use PhpOffice\PhpSpreadsheet\IOFactory;
use PhpOffice\PhpSpreadsheet\Style\Border;
use PhpOffice\PhpSpreadsheet\Style\Font;
use PhpOffice\PhpSpreadsheet\Reader\IReader;
use PhpOffice\PhpSpreadsheet\Writer\IWriter;
use PhpOffice\PhpSpreadsheet\Cell\Coordinate;
use PhpOffice\PhpSpreadsheet\Shared\Date;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use PhpOffice\PhpSpreadsheet\Style\Alignment;

/*
 * 日程取込の各画面コントローラー
 *
 * @create 2020/10/13 Dung
 *
 * @update
 */
class ExcelImportController extends Controller
{
	/**
	 * construct
	 * @param
	 * @return mixed
	 * @create 2020/10/13 Dung
	 * @update
	 */
	public function __construct() {

	}
	/**
	 * 日程取込条件設定画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/10/13 Dung
	 * @update
	 */
	public function index(Request $request)
	{
		return $this->initialize($request);
	}
	/**
	 * initial display processing
	 *
	 * @param Request
	 * @return View
	 *
	 * @create 2020/10/13 Dung
	 * @update
	 */
	private function initialize(Request $request)
	{
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));
		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		$this->data['menuInfo'] = $menuInfo;
		//initialize $originalError
		$originalError = [];
		//initialize $itemData
		$itemShow = array(
			'val1' => isset($request->val1) ? valueUrlDecode($request->val1) :
											((trim(old('val1')) != '') ? valueUrlDecode(old('val1'))
											: config('system_const.c_kind_chijyo')),
			'val2' => isset($request->val2) ? valueUrlDecode($request->val2) :
											((trim(old('val2')) != '') ? valueUrlDecode(old('val2'))
											: config('system_const_schem.bd_val_export')),
			'val3' => isset($request->val3) ? valueUrlDecode($request->val3) : ((trim(old('val3')) != '')
																		? valueUrlDecode(old('val3')) : ''),
			'val4' => isset($request->val4) ? valueUrlDecode($request->val4) : ((trim(old('val4')) != '')
																		? valueUrlDecode(old('val4')) : ''),
			'val5' => isset($request->val5) ? valueUrlDecode($request->val5) : ((trim(old('val5')) != '')
																		? valueUrlDecode(old('val5')) : ''),
			'val6' => isset($request->val6) ? valueUrlDecode($request->val6) : ((trim(old('val6')) != '')
																		? valueUrlDecode(old('val6')) : 0),
			'val8' => isset($request->val8) ? valueUrlDecode($request->val8) :
							(trim(old('val8') != '') ? old('val8') : config('system_const.displayed_results_1')),
		);
		// data 2 for val 2
		$data_val3 = $this->getDataVal3($menuInfo, $itemShow['val1']);
		if (count($data_val3) > 0) {
			$arrUnique = array();
			foreach($data_val3 as $key => &$item) {
				if (count($arrUnique) == 0) {
					$arrUnique[] = $item->val3Name;
				} else {
					if (!in_array($item->val3Name, $arrUnique)) {
						$arrUnique[] = $item->val3Name;
					} else {
						unset($data_val3[$key]);
					}
				}
			}
		}
		$this->data['dataView']['data_3'] = $data_val3;
		$this->data['dataView']['data_3_all'] = $this->getDataVal3($menuInfo, '', true);
		$tempVal3 = ($itemShow['val3'] == config('system_const.project_listkind_tosai')) ?
					((count($data_val3) > 0) ? valueUrlDecode($data_val3->first()->val3) :
					config('system_const.project_listkind_tosai')) :
					$itemShow['val3'];
		$data_val4 = $this->getDataVal4($itemShow['val1'], $tempVal3);
		if (count($data_val4) > 0) {
			$arrUnique = array();
			foreach($data_val4 as $key => &$item) {
				if (count($arrUnique) == 0) {
					$arrUnique[] = $item->val4Name;
				} else {
					if (!in_array($item->val4Name, $arrUnique)) {
						$arrUnique[] = $item->val4Name;
					} else {
						unset($data_val4[$key]);
					}
				}
			}
		}
		// data 4 for val 4
		$this->data['dataView']['data_4'] = $data_val4;
		$this->data['dataView']['data_4_all'] = $this->getDataVal4('', '', true);
		// data 4 for val 5
		$this->data['dataView']['data_5'] = $this->getDataVal5($itemShow['val1']);
		$this->data['dataView']['data_5_all'] = $this->getDataVal5('', true);
		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
		}
		$itemShow['val1'] = valueUrlEncode($itemShow['val1']);
		$itemShow['val2'] = valueUrlEncode($itemShow['val2']);
		$itemShow['val3'] = valueUrlEncode($itemShow['val3']);
		$itemShow['val4'] = valueUrlEncode($itemShow['val4']);
		$itemShow['val5'] = valueUrlEncode($itemShow['val5']);
		$itemShow['val6'] = valueUrlEncode($itemShow['val6']);
		$itemShow['val8'] = valueUrlEncode($itemShow['val8']);
		//request
		$this->data['request'] = $request;
		$this->data['originalError'] = $originalError;
		$this->data['itemShow'] = $itemShow;
		$this->data['msgTimeOut'] = valueUrlEncode(config('message.msg_cmn_err_002'));
		//return view with all data
		return view('Schem/ExcelImport/index', $this->data);
	}
	/**
	 * export excel and inport data  method
	 * @param Request ExcelImportOutputRequest
	 * @return
	 *
	 * @create 2020/12/31 Dung
	 * @update
	 */
	public function execute(ExcelImportOutputRequest  $request) {
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));
		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		//validate form
		$validated = $request->validated();
		if ($validated['val2'] == 1) {
			/* process export */
			$resExport = $this->ExcelImport($menuInfo, $request, $validated);
			if ($resExport != '') {
				return redirect($resExport);
			}
		}else {
			/* process import */
			$resImport = $this->import($menuInfo, $request, $validated);
			return redirect($resImport);
		}
	}

	/**
	 * get data value 3
	 *
	 * @param String $val3
	 * @return Object mixed
	 *
	 * @create 2020/10/23 Dung
	 * @update
	 */
	private function getDataVal3($menuInfo, $value = 0, $loadAll = false)
	{
		$data = MstProject::select('ID as val3', 'ProjectName as val3Name', 'ListKind')
							->where('SysKindID', '=', $menuInfo->KindID);
		$data = ($value !== '' ) ? $data->where('ListKind', '=', $value) : $data;
		$data = $data->orderBy('ProjectName')->get();
		if (count($data) > 0) {
			foreach ($data as &$row) {
				$row->val3 = valueUrlEncode($row->val3);
				$row->ListKind = valueUrlEncode($row->ListKind);
				$row->val3Name = ($loadAll) ? htmlentities($row->val3Name) : $row->val3Name;
			}
		}
		return $data;
	}
	/* load ajax when change projectId at val3 */
	/**
	 * get data value 4
	 *
	 * @param String $val4
	 * @return Object mixed
	 *
	 * @create 2020/10/23 Dung
	 * @update
	 */
	private function getDataVal4($val1 = 0, $val3 = '', $loadAll = false)
	{
		$data = mstOrderNo::select('mstOrderNo.OrderNo as val4', 'Cyn_TosaiData.CKind', 'Cyn_TosaiData.ProjectID')
								->join('Cyn_TosaiData', 'mstOrderNo.OrderNo', '=', 'Cyn_TosaiData.OrderNo');
		$data = ($val1 !== '') ? $data->where('Cyn_TosaiData.CKind', '=', $val1) : $data;
		$data = ($val3 !== '') ? $data->where('Cyn_TosaiData.ProjectID', '=', $val3) : $data;
		$data = $data->where('mstOrderNo.DispFlag', '=', 0)->orderBy('mstOrderNo.OrderNo')->distinct()->get();
		if (count($data) > 0) {
			foreach ($data as &$row) {
				$row->ProjectID = valueUrlEncode($row->ProjectID);
				$row->CKind = valueUrlEncode($row->CKind);
				$row->val4Name = ($loadAll) ? htmlentities($row->val4) : $row->val4;
				$row->val4 = valueUrlEncode($row->val4);
			}
		}
		return $data;
	}
	 /**
	 * get data value 5
	 *
	 * @param String $val5
	 * @return Object mixed
	 *
	 * @create 2020/10/23 Dung
	 * @update
	 */
	private function getDataVal5($val1 = 0, $loadAll = false)
	{
		$data = Cyn_mstKotei_STR_P::select('Cyn_mstKotei_STR_P.Name as val5',
											'Cyn_mstKotei_STR_P.Code', 'Cyn_mstKotei_STR_P.CKind');
		$data = ($val1 !== ''  && is_numeric($val1)) ? $data->where('Cyn_mstKotei_STR_P.CKind', '=', $val1) : $data;
		$data = $data->where('Cyn_mstKotei_STR_P.DelFlag', '=', 0)->orderBy('Cyn_mstKotei_STR_P.Name')->get();
		if (count($data) > 0) {
			foreach ($data as &$row) {
				$row->CKind = valueUrlEncode($row->CKind);
				$row->Code = valueUrlEncode($row->Code);
				$row->val5Name = ($loadAll) ? htmlentities($row->val5) : $row->val5;
			}
		}
		return $data;
	}
	/**
	 * POST
	 *
	 * @param ExcelImportOutputRequest
	 * @return View
	 *
	 * @create 2020/11/23　Dung
	 * @update
	 */
	public function ExcelImport($menuInfo, $request, $validated)
	{
		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/index';
		$url .= '?cmn1=' . $request->cmn1;
		$url .= '&cmn2=' . $request->cmn2;
		$url .= '&val1=' . valueUrlEncode($request->val1);
		$url .= '&val2=' . valueUrlEncode($request->val2);
		$url .= '&val3=' . valueUrlEncode($request->val3);
		$url .= '&val4=' . valueUrlEncode($request->val4);
		$url .= '&val5=' . valueUrlEncode($request->val5);
		$url .= '&val6=' . valueUrlEncode($request->val6);
		$url .= '&val8=' . valueUrlEncode($request->val8);
		// data header
		$data_header = Cyn_mstKotei_STR_C::select(
										'Cyn_mstKotei_STR_C.Kotei',
										'Cyn_mstKotei_STR_C.KKumiku',
										'Cyn_mstKotei_STR_C.No',
										'Cyn_mstKotei.Name',
									)
									->join('Cyn_mstKotei', function($join) {
										$join->on('Cyn_mstKotei_STR_C.Kotei', '=', 'Cyn_mstKotei.Code')
											->on('Cyn_mstKotei_STR_C.Ckind', '=', 'Cyn_mstKotei.Ckind');
									})
									->where('Cyn_mstKotei_STR_C.Ckind','=',$validated['val1']);
		$data_header = ( trim($request['val5']) != '') ? $data_header
											->where('Cyn_mstKotei_STR_C.Code', '=' ,$validated['val5'])
											: $data_header;
		$header = $data_header->distinct()->orderBy('Cyn_mstKotei_STR_C.No', 'ASC')->get();
		$timeTrackerCommon = new TimeTrackerCommon();
		$projectCalendar = $timeTrackerCommon->getCalendar($validated['val3']);
		// error
		if ($projectCalendar != '' && is_string($projectCalendar)) {
			// has error
			$url .= '&err1=' . valueUrlEncode($projectCalendar);
			return $url;
		}
		// desing header
		$koteiPattern = array();
		$indexColE = Coordinate::columnIndexFromString('E');
		$indexColF = Coordinate::columnIndexFromString('F');
		$indexColG = Coordinate::columnIndexFromString('G');
		$indexColH = Coordinate::columnIndexFromString('H');
		$indexColJ = Coordinate::columnIndexFromString('J');
		$nextCol = $indexColE;
		if (count($header) > 0) {
			foreach ($header as $obj) {
				if (!isset($koteiPattern[$obj->Kotei.'_'.$obj->KKumiku])) {
					$koteiPattern[$obj->Kotei.'_'.$obj->KKumiku] = array(
						'Kotei' => $obj->Kotei,
						'KKumiku' => $obj->KKumiku,
						'Name' => $obj->Name,
						'col' => $nextCol,
					);
				}
				$nextCol += 6;
			}
		}
		$data_1 = Cyn_TosaiData::select(
			'Cyn_TosaiData.Name as T_Name',
			'Cyn_TosaiData.BKumiku as T_BKumiku',
			'Cyn_BlockKukaku.Name',
			'Cyn_BlockKukaku.BKumiku',
			'Cyn_BlockKukaku.No',
			'Cyn_Plan.WorkItemID',
			'Cyn_Plan.Kotei',
			'Cyn_Plan.KKumiku',
			'Cyn_Plan.Floor',
			'Cyn_Plan.BD_Code',
			'Cyn_Plan.KoteiNo',
			'Cyn_Plan.N_KoteiNo',
			'mstFloor.Name as FloorName',
			'mstBDCode.Name as BDName',
			'WorkItemIDList.ID as ListID'
		)
		->join('Cyn_BlockKukaku', function($join) {
			$join->on('Cyn_TosaiData.ProjectID', '=', 'Cyn_BlockKukaku.ProjectID')
				->on('Cyn_TosaiData.OrderNo', '=', 'Cyn_BlockKukaku.OrderNo')
				->on('Cyn_TosaiData.CKind', '=', 'Cyn_BlockKukaku.CKind')
				->on('Cyn_TosaiData.Name', '=', 'Cyn_BlockKukaku.T_Name')
				->on('Cyn_TosaiData.BKumiku', '=', 'Cyn_BlockKukaku.T_BKumiku');
			})
		->join('Cyn_Plan', function($join) {
			$join->on('Cyn_BlockKukaku.ProjectID', '=', 'Cyn_Plan.ProjectID')
				->on('Cyn_BlockKukaku.OrderNo', '=', 'Cyn_Plan.OrderNo')
				->on('Cyn_BlockKukaku.No', '=', 'Cyn_Plan.No');
			})
		->leftJoin('mstFloor','Cyn_Plan.Floor', '=', 'mstFloor.Code')
		->leftJoin('mstBDCode','Cyn_Plan.BD_Code', '=', 'mstBDCode.Code')
		->join('WorkItemIDList','Cyn_Plan.WorkItemID', '=', 'WorkItemIDList.WorkItemID')
		->where('Cyn_TosaiData.ProjectID', '=', $validated['val3'])
		->where('Cyn_TosaiData.OrderNo', '=', $validated['val4'])
		->where('Cyn_TosaiData.CKind', '=', $validated['val1']);

		$data_2 = Cyn_BlockKukaku::select(
			'Cyn_BlockKukaku.Name as T_Name',
			'Cyn_BlockKukaku.BKumiku as T_BKumiku',
			'Cyn_C_BlockKukaku.Name',
			'Cyn_C_BlockKukaku.BKumiku',
			'Cyn_C_BlockKukaku.No',
			'Cyn_C_Plan.WorkItemID',
			'Cyn_C_Plan.Kotei',
			'Cyn_C_Plan.KKumiku',
			'Cyn_C_Plan.Floor',
			'Cyn_C_Plan.BD_Code',
			'Cyn_C_Plan.KoteiNo',
			'Cyn_C_Plan.N_KoteiNo',
			'mstFloor.Name as FloorName',
			'mstBDCode.Name as BDName',
			'WorkItemIDList.ID as ListID',
		)
		->join('Cyn_C_BlockKukaku', function($join) {
			$join->on('Cyn_BlockKukaku.ProjectID', '=', 'Cyn_C_BlockKukaku.ProjectID')
				->on('Cyn_BlockKukaku.OrderNo', '=', 'Cyn_C_BlockKukaku.OrderNo')
				->on('Cyn_BlockKukaku.CKind', '=', 'Cyn_C_BlockKukaku.CKind')
				->on('Cyn_BlockKukaku.Name', '=', 'Cyn_C_BlockKukaku.T_Name')
				->on('Cyn_BlockKukaku.BKumiku', '=', 'Cyn_C_BlockKukaku.T_BKumiku');
		})
		->join('Cyn_C_Plan', function($join) {
			$join->on('Cyn_BlockKukaku.ProjectID', '=', 'Cyn_C_Plan.ProjectID')
				->on('Cyn_BlockKukaku.OrderNo', '=', 'Cyn_C_Plan.OrderNo')
				->on('Cyn_BlockKukaku.No', '=', 'Cyn_C_Plan.No');
		})
		->leftJoin('mstFloor', 'Cyn_C_Plan.Floor', '=', 'mstFloor.Code')
		->leftJoin('mstBDCode', 'Cyn_C_Plan.BD_Code', '=', 'mstBDCode.Code')
		->join('WorkItemIDList','Cyn_C_Plan.WorkItemID', '=', 'WorkItemIDList.WorkItemID')
		->where('Cyn_BlockKukaku.ProjectID', '=', $validated['val3'])
		->where('Cyn_BlockKukaku.OrderNo', '=', $validated['val4'])
		->where('Cyn_BlockKukaku.CKind', '=', $validated['val1']);

		$data = $data_1->union($data_2)
						->orderBy('T_Name', 'asc')
						->orderBy('T_BKumiku', 'asc')
						->orderBy('Name', 'asc')
						->orderBy('BKumiku', 'asc')
						->get();
		// start header
		$blockInformation = array();
		$processInformation = array();
		$nextProcessInformation = array();
		if (count($data) > 0) {
			foreach($data as $record) {
				$nameOfTBKumiku = FuncCommon::getKumikuData($record->T_BKumiku);
				$nameOfBKumiku = FuncCommon::getKumikuData($record->BKumiku);
				$blockInformation[$record->No] = array(
					'A' => $record->T_Name,
					'B' => is_array($nameOfTBKumiku) ? $nameOfTBKumiku[2] : null,
					'C' => $record->Name,
					'D' => is_array($nameOfBKumiku) ? $nameOfBKumiku[2] : null,
				);
				$processInformation[$record->No.'_'.$record->Kotei.'_'.$record->BKumiku] = $record->toArray();
				$nextProcessInformation[$record->No.'_'.$record->KoteiNo] = array(
					'N_KoteiNo' => $record->N_KoteiNo,
					'WorkItemID' => $record->WorkItemID,
				);
			}
		}
		// initial url
		if (count($data) == 0) {
			// has error
			$url .= '&err1=' . valueUrlEncode(config('message.msg_cmn_db_001'));
			return $url;
		}
		// read file excel template
		$inputFileType = 'Xlsx';
		$inputFileName = config('system_const_schem.export_template_path');
		if ($inputFileName != '') {
			$arrPath = explode('/', $inputFileName);
			$arrLength = count($arrPath);

			if ($arrLength > 2) {
				$inputFileName = public_path().'\\'.$arrPath[$arrLength - 2].'\\'.$arrPath[$arrLength - 1];

				if (!file_exists($inputFileName)) {
					// has error
					$url .= '&err1=' . valueUrlEncode(config('message.msg_cmn_db_001'));
					return $url;
				}
				// setting header
				$reader = IOFactory::createReader($inputFileType);
				$spreadsheet = $reader->load($inputFileName);
				$worksheet = $spreadsheet->getActiveSheet();
				//set value header
				$arrRangeBorderHeaderAllBorder = array();
				$arrRangeBorderHeaderHair = array();
				$arrFormatMD = array();
				$firstCol = '';
				$lastCol = '';
				$arrListLastColHeader = array();
				foreach($koteiPattern as $koteiGroup) {
					if ($firstCol == '') {
						$firstCol = $koteiGroup['col'];
					}
					//set value header
					$dataKKumiku = FuncCommon::getKumikuData($koteiGroup['KKumiku']);
					// E1
					$worksheet->setCellValueByColumnAndRow($koteiGroup['col'], 1, '工程');
					// F1
					$worksheet->setCellValueByColumnAndRow(($koteiGroup['col']+1), 1,
					$koteiGroup['Kotei'].config('system_const.code_name_separator').$koteiGroup['Name']);
					// G1
					$worksheet->setCellValueByColumnAndRow(($koteiGroup['col']+2), 1, '工程組区');
					// h1
					$worksheet->setCellValueByColumnAndRow(($koteiGroup['col']+3), 1, $dataKKumiku[2]);
					// E2
					$worksheet->setCellValueByColumnAndRow($koteiGroup['col'], 2, '着工日');
					// F2
					$worksheet->setCellValueByColumnAndRow(($koteiGroup['col']+1), 2, '完工日');
					// G2
					$worksheet->setCellValueByColumnAndRow(($koteiGroup['col']+2), 2, '工期');
					// H2
					$worksheet->setCellValueByColumnAndRow(($koteiGroup['col']+3), 2, 'リンク日数');
					// I2
					$worksheet->setCellValueByColumnAndRow(($koteiGroup['col']+4), 2, '棟');
					// J2
					$worksheet->setCellValueByColumnAndRow(($koteiGroup['col']+5), 2, '物量');
					//set border header
					// E1 -> E2
					$arrRangeBorderHeaderHair[] = Coordinate::stringFromColumnIndex($koteiGroup['col']).'1:'.
												Coordinate::stringFromColumnIndex($koteiGroup['col']).'2';
					// F1 -> F2
					$arrRangeBorderHeaderHair[] = Coordinate::stringFromColumnIndex(($koteiGroup['col']+1)).'1:'.
												Coordinate::stringFromColumnIndex(($koteiGroup['col']+1)).'2';
					// G1 -> G2
					$arrRangeBorderHeaderHair[] = Coordinate::stringFromColumnIndex(($koteiGroup['col']+2)).'1:'.
												Coordinate::stringFromColumnIndex(($koteiGroup['col']+2)).'2';
					// H1 -> H2
					$arrRangeBorderHeaderHair[] = Coordinate::stringFromColumnIndex(($koteiGroup['col']+3)).'1:'.
												Coordinate::stringFromColumnIndex(($koteiGroup['col']+3)).'2';
					// I2 -> I2
					$arrRangeBorderHeaderHair[] = Coordinate::stringFromColumnIndex(($koteiGroup['col']+4)).'1:'.
												Coordinate::stringFromColumnIndex(($koteiGroup['col']+4)).'2';
					// calculator number copy kotei
					$lastCol = $koteiGroup['col']+5;

					$arrListLastColHeader[] = $lastCol;
				}
				$totalData = $data->toArray();
				if (count($blockInformation) > 0) {
					$baseRow = 3;
					$rowIncrement = $baseRow;
					foreach($blockInformation as $no => $objData) {
						// Set data into column A~D
						foreach($objData as $colName => $colData) {
							$worksheet->setCellValue($colName.$rowIncrement, $colData);
						}
						// Get workItemId
						$koteiInfo = array_filter($totalData, function($item) use ($no, $objData) {
							return $item['No'] == $no;
						});
						// get koteiRange
						if (count($koteiInfo) > 0) {
							$arrWorkItemID = array();
							foreach ($koteiInfo as $data) {
								array_push($arrWorkItemID, array('workItemID' => $data['WorkItemID'],
											 'ListID' => $data['ListID']));
							}
							// time tracker getKoteiRange
							$koteiRangeData = $timeTrackerCommon->getKoteiRange($arrWorkItemID, false);
							foreach($koteiInfo as $obj) {
								if (isset($koteiPattern[$obj['Kotei'].'_'.$obj['KKumiku']])) {
									$col = $koteiPattern[$obj['Kotei'].'_'.$obj['KKumiku']]['col'];
									// format date
									$plannedStartDate = Date::PHPToExcel(date("Y-m-d",
													strtotime($koteiRangeData[$obj['WorkItemID']]['plannedStartDate'])));
									$plannedFinishDate = Date::PHPToExcel(date("Y-m-d",
													strtotime($koteiRangeData[$obj['WorkItemID']]['plannedFinishDate'])));
									// N_KoteiNo data
									$N_Kotei = $obj['N_KoteiNo'];
									// workItemID
									$Nkotei_workItemID = (isset($nextProcessInformation[$no.'_'.$N_Kotei])) ?
														$nextProcessInformation[$no.'_'.$N_Kotei]['WorkItemID'] : '';
									// time tracker getDateDiff to G3
									$dataGetDiffDateG3 = $timeTrackerCommon->getDateDiff($projectCalendar,
															$koteiRangeData[$obj['WorkItemID']]['plannedStartDate'],
															$koteiRangeData[$obj['WorkItemID']]['plannedFinishDate']);
									//update Rev7
									// time tracker getDateDiff to H3
									$plannedStartDateNKotei = isset($koteiRangeData[$Nkotei_workItemID]) ?
															$koteiRangeData[$Nkotei_workItemID]['plannedStartDate'] : null;
									$dataGetDiffDateH3 = isset($plannedStartDateNKotei) ?
														$timeTrackerCommon->getDateDiff($projectCalendar,
														$koteiRangeData[$obj['WorkItemID']]['plannedFinishDate'],
														$plannedStartDateNKotei) : 0;
									//update Rev7
									// get first col
									// E3
									$worksheet->setCellValueByColumnAndRow($col, $rowIncrement, $plannedStartDate);
									// F3
									$worksheet->setCellValueByColumnAndRow(($col+1), $rowIncrement, $plannedFinishDate);
									// G3
									$worksheet->setCellValueByColumnAndRow(($col+2), $rowIncrement, ($dataGetDiffDateG3 - 1));
									// H3
									$worksheet->setCellValueByColumnAndRow(($col+3), $rowIncrement, ($dataGetDiffDateH3 - 1));
									// I3
									if($obj['Floor'] != '' && $obj['FloorName'] != ''){
										$worksheet->setCellValueByColumnAndRow(($col+4), $rowIncrement,
										$obj['Floor'].config('system_const.code_name_separator').$obj['FloorName']);
									}
									// J3
									if($obj['BD_Code'] != '' && $obj['BDName'] != ''){
										$worksheet->setCellValueByColumnAndRow(($col+5), $rowIncrement,
										$obj['BD_Code'].config('system_const.code_name_separator').$obj['BDName']);
									}
								}
							}
						}
						$rowIncrement++;
					}
					// format date col E + F
					foreach($koteiPattern as $koteiGroup) {
						$arrFormatMD[] = Coordinate::stringFromColumnIndex($koteiGroup['col']).'3:'.
										Coordinate::stringFromColumnIndex(($koteiGroup['col']+1)).($rowIncrement-1);
					}
				}
				// set style
				// all border header
				if ($firstCol != '' && $lastCol != '') {
					$arrRangeBorderHeaderAllBorder = array(
						Coordinate::stringFromColumnIndex($firstCol).'1:'.Coordinate::stringFromColumnIndex($lastCol).'1',
						Coordinate::stringFromColumnIndex($firstCol).'2:'.Coordinate::stringFromColumnIndex($lastCol).'2',
					);
					foreach($arrRangeBorderHeaderAllBorder as $range) {
						$worksheet->getStyle($range)->applyFromArray([
							'borders' => [
								'outline' => [
									'borderStyle' => Border::BORDER_THIN,
									'color' => ['argb' => '000000'],
								],
							],
						]);
					}
				}
				// set border hair header
				if (count($arrRangeBorderHeaderHair) > 0) {
					foreach($arrRangeBorderHeaderHair as $range) {
						$worksheet->getStyle($range)->applyFromArray([
							'borders' => [
								'right' => [
									'borderStyle' => Border::BORDER_HAIR,
									'color' => ['argb' => '000000'],
								],
							],
						]);
					}
				}
				// fix bug 288
				if (count($koteiPattern) == 0) {
					$lastCol = 4;
				}
				$worksheet->getStyle('A3:'.Coordinate::stringFromColumnIndex($lastCol).($rowIncrement-1))->applyFromArray([
					'borders' => [
						'inside' => [
							'borderStyle' => Border::BORDER_THIN,
							'color' => ['argb' => '000000'],
						],
					],
				]);
				if (count($arrListLastColHeader) > 0) {
					foreach ($arrListLastColHeader as $indexCol) {
						$worksheet->getStyle(
							Coordinate::stringFromColumnIndex($indexCol).'1:'.
							Coordinate::stringFromColumnIndex($indexCol).($rowIncrement-1)
						)->applyFromArray([
							'borders' => [
								'right' => [
									'borderStyle' => Border::BORDER_THIN,
									'color' => ['argb' => '000000'],
								],
							],
						]);
					}
				} else {
					// set last border right
					$worksheet->getStyle(
						Coordinate::stringFromColumnIndex($lastCol).'1:'.
						Coordinate::stringFromColumnIndex($lastCol).($rowIncrement-1)
					)->applyFromArray([
						'borders' => [
							'right' => [
								'borderStyle' => Border::BORDER_THIN,
								'color' => ['argb' => '000000'],
							],
						],
					]);
				}
				// set formatdate M/D
				if (count($arrFormatMD) > 0) {
					foreach($arrFormatMD as $range) {
						$worksheet->getStyle($range)
									->getAlignment()
									->setHorizontal(Alignment::HORIZONTAL_CENTER);// fix bug 288
						$worksheet->getStyle($range)
									->getNumberFormat()
									->setFormatCode('M/D');
					}
				}
				//export excel
				$strFilename = $menuInfo->MenuNick .'.xlsx';
				header("Content-Type: application/force-download");
				header("Content-Type: application/octet-stream");
				header("Content-Type: application/download");
				header('Content-Type:application/octet-stream; charset=Shift_JIS');
				header('Content-Type: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet');
				header('Content-Disposition: attachment; filename="'.$strFilename);
				header("Content-Transfer-Encoding: binary");
				header('Expires: 0');
				header('Cache-Control: no-cache, no-store');
				header('Pragma: no-cache');
				if (isset($_COOKIE['export']) && $_COOKIE['export'] == 0) {
					unset($_COOKIE['export']);
					setcookie("export", 1, -1, '/');
				}
				$writer = IOFactory::createWriter($spreadsheet, "Xlsx");
				ob_end_clean();
				flush();
				$writer->save("php://output");
			}
		}
	}
	/**
	 * POST
	 *
	 * @param Request
	 * @return View
	 *
	 * @create 2020/11/23 Dung
	 * @update
	 */
	public function import($menuInfo,  $request, $validated)
	{
		//try lock
		//Timetracker
		$timeTrackerCommon = new TimeTrackerCommon();
		//TryLock
		$originalError  = $this->tryLock($menuInfo->KindID, config('system_const_schem.sys_menu_id_plan'),
											 $menuInfo->UserID, $menuInfo->SessionID, $request->val1, true);
		$projectID = $request->val3;
		$orderNo = $request->val4;
		$cKind = $request->val1;
		//calendar
		$projectCalendar = $timeTrackerCommon->getCalendar($projectID);
		//set table type
		$data_table_1 = Cyn_TosaiData::selectRaw('\'0\' as TableType')
			->join('Cyn_BlockKukaku', function($join) {
				$join->on('Cyn_TosaiData.ProjectID', '=', 'Cyn_BlockKukaku.ProjectID')
					->on('Cyn_TosaiData.OrderNo', '=', 'Cyn_BlockKukaku.OrderNo')
					->on('Cyn_TosaiData.CKind', '=', 'Cyn_BlockKukaku.CKind')
					->on('Cyn_TosaiData.Name', '=', 'Cyn_BlockKukaku.T_Name')
					->on('Cyn_TosaiData.BKumiku', '=', 'Cyn_BlockKukaku.T_BKumiku');
				})
			->where('Cyn_TosaiData.ProjectID', '=',$projectID)
			->where('Cyn_TosaiData.OrderNo', '=', $orderNo)
			->where('Cyn_TosaiData.CKind', '=', $cKind)
			->where('Cyn_TosaiData.WorkItemID', '<>' , 0);
		$data_table_2 = Cyn_BlockKukaku::selectRaw('\'1\' as TableType')
			->join('Cyn_C_BlockKukaku', function($join) {
				$join->on('Cyn_BlockKukaku.ProjectID', '=', 'Cyn_C_BlockKukaku.ProjectID')
					->on('Cyn_BlockKukaku.OrderNo', '=', 'Cyn_C_BlockKukaku.OrderNo')
					->on('Cyn_BlockKukaku.CKind', '=', 'Cyn_C_BlockKukaku.CKind')
					->on('Cyn_BlockKukaku.Name', '=', 'Cyn_C_BlockKukaku.T_Name')
					->on('Cyn_BlockKukaku.BKumiku', '=', 'Cyn_C_BlockKukaku.T_BKumiku');
			})
			->where('Cyn_BlockKukaku.ProjectID', '=',$projectID)
			->where('Cyn_BlockKukaku.OrderNo', '=', $orderNo)
			->where('Cyn_BlockKukaku.CKind', '=', $cKind);
		$TableType = $data_table_1->union($data_table_2)->first();
		$TableType = isset($TableType) ? $TableType['TableType'] : null;
		//set select data
		$data_1 = Cyn_TosaiData::select(
			'Cyn_TosaiData.Name as T_Name',
			'Cyn_TosaiData.BKumiku as T_BKumiku',
			'Cyn_TosaiData.WorkItemID as T_WorkItemID',

			'Cyn_BlockKukaku.Name',
			'Cyn_BlockKukaku.BKumiku',
			'Cyn_BlockKukaku.No',
			'Cyn_BlockKukaku.WorkItemID as B_WorkItemID',

			'Cyn_Plan.WorkItemID as P_WorkItemID',
			'Cyn_Plan.Kotei',
			'Cyn_Plan.KKumiku',
			'Cyn_Plan.Floor',
			'Cyn_Plan.BD_Code',
			'Cyn_Plan.Days',

			'wList1.ID as T_ID',
			'wList2.ID as B_ID',
			'wList3.ID as P_ID',
			'Cyn_TosaiData.T_Date',
			)
			->leftJoin('Cyn_BlockKukaku', function($join) {
				$join->on('Cyn_TosaiData.ProjectID', '=', 'Cyn_BlockKukaku.ProjectID')
					->on('Cyn_TosaiData.OrderNo', '=', 'Cyn_BlockKukaku.OrderNo')
					->on('Cyn_TosaiData.CKind', '=', 'Cyn_BlockKukaku.CKind')
					->on('Cyn_TosaiData.Name', '=', 'Cyn_BlockKukaku.T_Name')
					->on('Cyn_TosaiData.BKumiku', '=', 'Cyn_BlockKukaku.T_BKumiku')
					->whereNull('Cyn_BlockKukaku.Del_Date');
				})
			->leftJoin('Cyn_Plan', function($join) {
				$join->on('Cyn_BlockKukaku.ProjectID', '=', 'Cyn_Plan.ProjectID')
					->on('Cyn_BlockKukaku.OrderNo', '=', 'Cyn_Plan.OrderNo')
					->on('Cyn_BlockKukaku.No', '=', 'Cyn_Plan.No')
					->whereNull('Cyn_Plan.Del_Date');
				})
			->leftJoin('WorkItemIDList as wList1','Cyn_TosaiData.WorkItemID', '=', 'wList1.WorkItemID')
			->leftJoin('WorkItemIDList as wList2','Cyn_BlockKukaku.WorkItemID', '=', 'wList2.WorkItemID')
			->leftJoin('WorkItemIDList as wList3','Cyn_Plan.WorkItemID', '=', 'wList3.WorkItemID')

			->where('Cyn_TosaiData.ProjectID', '=',$projectID)
			->where('Cyn_TosaiData.OrderNo', '=', $orderNo)
			->where('Cyn_TosaiData.CKind', '=', $cKind)
			->where('Cyn_TosaiData.WorkItemID', '<>' , 0);

		$data_2 = Cyn_BlockKukaku::select(
			'Cyn_BlockKukaku.Name as T_Name',
			'Cyn_BlockKukaku.BKumiku as T_BKumiku',
			'Cyn_BlockKukaku.WorkItemID  as T_WorkItemID',

			'Cyn_C_BlockKukaku.Name',
			'Cyn_C_BlockKukaku.BKumiku',
			'Cyn_C_BlockKukaku.No',
			'Cyn_C_BlockKukaku.WorkItemID as B_WorkItemID',

			'Cyn_C_Plan.WorkItemID as P_WorkItemID',
			'Cyn_C_Plan.Kotei',
			'Cyn_C_Plan.KKumiku',
			'Cyn_C_Plan.Floor',
			'Cyn_C_Plan.BD_Code',
			'Cyn_C_Plan.Days',

			'wList1.ID as T_ID',
			'wList2.ID as B_ID',
			'wList3.ID as P_ID',
			)
			->selectRaw('null as T_Date')
			->leftJoin('Cyn_C_BlockKukaku', function($join) {
				$join->on('Cyn_BlockKukaku.ProjectID', '=', 'Cyn_C_BlockKukaku.ProjectID')
					->on('Cyn_BlockKukaku.OrderNo', '=', 'Cyn_C_BlockKukaku.OrderNo')
					->on('Cyn_BlockKukaku.CKind', '=', 'Cyn_C_BlockKukaku.CKind')
					->on('Cyn_BlockKukaku.Name', '=', 'Cyn_C_BlockKukaku.T_Name')
					->on('Cyn_BlockKukaku.BKumiku', '=', 'Cyn_C_BlockKukaku.T_BKumiku')
					->whereNull('Cyn_C_BlockKukaku.Del_Date');
			})
			->leftJoin('Cyn_C_Plan', function($join) {
				$join->on('Cyn_C_BlockKukaku.ProjectID', '=', 'Cyn_C_Plan.ProjectID')
					->on('Cyn_C_BlockKukaku.OrderNo', '=', 'Cyn_C_Plan.OrderNo')
					->on('Cyn_C_BlockKukaku.No', '=', 'Cyn_C_Plan.No')
					->whereNull('Cyn_C_Plan.Del_Date');
			})
			->leftJoin('WorkItemIDList as wList1','Cyn_BlockKukaku.WorkItemID', '=', 'wList1.WorkItemID')
			->leftJoin('WorkItemIDList as wList2','Cyn_C_BlockKukaku.WorkItemID', '=', 'wList2.WorkItemID')
			->leftJoin('WorkItemIDList as wList3','Cyn_C_Plan.WorkItemID', '=', 'wList3.WorkItemID')

			->where('Cyn_BlockKukaku.ProjectID', '=', $projectID)
			->where('Cyn_BlockKukaku.OrderNo', '=', $orderNo)
			->where('Cyn_BlockKukaku.CKind', '=', $cKind);
		$data = $data_1 ->union($data_2)
						->orderBy('T_Name', 'asc')
						->orderBy('T_BKumiku', 'asc')
						->orderBy('Name', 'asc')
						->orderBy('BKumiku', 'asc')
						->get();
		$workItemID = array();
		$tosai = array();
		$chunittei = array();
		$processInformation = array();
		if (count($data) > 0) {
			foreach($data as $record) {
				$tosai[$record->T_Name.'_'.$record->T_BKumiku] = array(
					'T_Name' => $record->T_Name,
					'T_BKumiku' => $record->T_BKumiku,
					'T_WorkItemID' => $record->T_WorkItemID,
					'T_Date' => $record->T_Date,
				);
				$chunittei[$record->T_Name.'_'.$record->T_BKumiku.'_'.$record->Name.'_'.$record->BKumiku] = array(
					'T_Name' => $record->T_Name,
					'T_BKumiku' => $record->T_BKumiku,
					'Name' => $record->Name,
					'BKumiku' => $record->BKumiku,
					'B_WorkItemID' => $record->B_WorkItemID,
				);
				$processInformation[$record->T_Name.'_'.$record->T_BKumiku.'_'.$record->Name.'_'.$record->BKumiku
											.'_'.$record->Kotei.'_'.$record->KKumiku] = array(
					'T_Name' => $record->T_Name,
					'T_BKumiku' => $record->T_BKumiku,
					'Name' => $record->Name,
					'BKumiku' => $record->BKumiku,
					'Kotei' => $record->Kotei,
					'KKumiku' => $record->KKumiku,
					'Floor' => $record->Floor,
					'BD_Code' => $record->BD_Code,
					'P_WorkItemID' => $record->P_WorkItemID,
				);
				$workItemID[] = array(
					'WorkItemID' => $record->T_WorkItemID,
					'ID' => $record->T_ID,
				);
				$workItemID[] = array(
					'WorkItemID' => $record->B_WorkItemID,
					'ID' => $record->B_ID,
				);
				$workItemID[] = array(
					'WorkItemID' => $record->P_WorkItemID,
					'ID' => $record->P_ID,
				);
			}
		}
		$timeTrackerCommon = new TimeTrackerCommon();
		$workItemID = $timeTrackerCommon->getKoteiRange($workItemID, true, $projectCalendar);
		if (is_string($workItemID) && !empty($workItemID)) {
			//処理を中断する。
			return $this->stopProcessThenGoTo030501($menuInfo, $request, $workItemID);
		}
		//processing file import
		$file = $request->val7;
		$inputFileName = $_FILES['val7']['tmp_name'];
		try {
			/**  Identify the type of $inputFileName  **/
			$inputFileType = IOFactory::identify($inputFileName);
			/**  Create a new Reader of the type that has been identified  **/
			$reader = IOFactory::createReader($inputFileType);
			/**  Load $inputFileName to a Spreadsheet Object  **/
			$spreadsheet = $reader->load($inputFileName);
			$worksheet = $spreadsheet->getActiveSheet();

			// data read file from excel.
			$dataExcel = array();
			/* header  */
			//Read file excel「ヘッダー」
			$koteiPattern = array();
			$startCol = 5;
			//column ６： row 1「工程」
			$col6Row1 = $worksheet->getCellByColumnAndRow($startCol + 1, 1)->getValue();
			//column 8, row 1
			$col8Row1 = $worksheet->getCellByColumnAndRow($startCol + 3, 1)->getValue();
			while(!is_null($col6Row1) && !is_null($col8Row1))
			{
				$koteiCode = FuncCommon::getSplitChar($col6Row1)[0];
				$listBDCode = Cyn_mstKotei::where('CKind','=', $validated['val1'])
											->where('Code','=',$koteiCode)->get()->toArray();
				if (count($listBDCode) == 0) {
					// 処理を中断する。
					return $this->stopProcessThenGoTo030501($menuInfo, $request,
									sprintf(config('message.msg_cmn_db_028'), $koteiCode));
				}
				$koteiKumiku = FuncCommon::getSplitChar($col8Row1)[0];
				$isKKumikuData = FuncCommon::isKKumikuData($koteiKumiku);
				if (!$isKKumikuData) {
					// 処理を中断する。
					return $this->stopProcessThenGoTo030501($menuInfo, $request,
									sprintf(config('message.msg_cmn_db_028'), $koteiKumiku));
				}
				$koteiPattern[$koteiCode.'_'.$koteiKumiku] = array(
					'Kotei' => $koteiCode,
					'KKumiku' => $koteiKumiku,
					'StartCol' => $startCol
				);
				$startCol = $startCol + 6;
				$col6Row1 = $worksheet->getCellByColumnAndRow($startCol + 1, 1)->getValue();
				$col8Row1 = $worksheet->getCellByColumnAndRow($startCol + 3, 1)->getValue();
			}
			// Read file excel「内容」
			$listExcel = array();
			$row = 3;
			$tosaiBlockName = $worksheet->getCellByColumnAndRow(1, $row)->getValue();
			$tosaiBlockKumiku = $worksheet->getCellByColumnAndRow(2, $row)->getValue();
			$chyuNitteiBlockName = $worksheet->getCellByColumnAndRow(3, $row)->getValue();
			$chyuNitteiBlockKumiku = $worksheet->getCellByColumnAndRow(4, $row)->getValue();
			while(!is_null($tosaiBlockName)
				&& !is_null($tosaiBlockKumiku)
				&& !is_null($chyuNitteiBlockName)
				&& !is_null($chyuNitteiBlockKumiku)) {
				$tosaiBlockKumiku = FuncCommon::getSplitChar($tosaiBlockKumiku)[0];
				$isExistTBKumiku = FuncCommon::isKKumikuData($tosaiBlockKumiku);
				if (!$isExistTBKumiku) {
					// 処理を中断する。
					return $this->stopProcessThenGoTo030501($menuInfo, $request,
									sprintf(config('message.msg_cmn_db_028'), $tosaiBlockKumiku));
				}
				$chyuNitteiBlockKumiku = FuncCommon::getSplitChar($chyuNitteiBlockKumiku)[0];
				$isExistCBKumiku = FuncCommon::isKKumikuData($chyuNitteiBlockKumiku);
				if (!$isExistCBKumiku) {
					// 処理を中断する。
					return $this->stopProcessThenGoTo030501($menuInfo, $request,
									sprintf(config('message.msg_cmn_db_028'), $chyuNitteiBlockKumiku));
				}
				//memory「搭載」、「中日程」
				$rowData = array(
					'T_Name' => $tosaiBlockName,
					'T_BKumiku' => $tosaiBlockKumiku,
					'Name' => $chyuNitteiBlockName,
					'BKumiku' => $chyuNitteiBlockKumiku,
				);
				foreach ($koteiPattern as $kotei) {
					//Sdate
					$sDate = $worksheet->getCellByColumnAndRow($kotei['StartCol'] , $row)->getValue();
					$rowData[$kotei['StartCol'] ] =  is_null($sDate) ? null
													: \PhpOffice\PhpSpreadsheet\Shared\Date::excelToDateTimeObject($sDate)
																								->format('Y/m/d');
					//EDate
					$eDate = $worksheet->getCellByColumnAndRow($kotei['StartCol'] + 1, $row)->getValue();
					$rowData[$kotei['StartCol'] + 1] = is_null($eDate) ? null
													: \PhpOffice\PhpSpreadsheet\Shared\Date::excelToDateTimeObject($eDate)
																								->format('Y/m/d');
					$rowData[$kotei['StartCol'] + 2] = $worksheet->getCellByColumnAndRow($kotei['StartCol'] + 2, $row)
																	->getValue();
					$rowData[$kotei['StartCol'] + 3] = $worksheet->getCellByColumnAndRow($kotei['StartCol'] + 3, $row)
																	->getValue();
					//Floor
					$floor = $worksheet->getCellByColumnAndRow($kotei['StartCol'] + 4, $row)->getValue();
					$rowData[$kotei['StartCol'] + 4] = is_null($floor) ? null : FuncCommon::getSplitChar($floor)[0];
					//BD_code
					$bdCode = $worksheet->getCellByColumnAndRow($kotei['StartCol'] + 5, $row)->getValue();
					$rowData[$kotei['StartCol'] + 5] = is_null($bdCode) ? null : FuncCommon::getSplitChar($bdCode)[0];
				}
				$listExcel[$chyuNitteiBlockKumiku.'_'.$tosaiBlockName.'_'.$tosaiBlockKumiku.'_'.$chyuNitteiBlockName]
																											= $rowData;
				$row = $row + 1;
				$tosaiBlockName = $worksheet->getCellByColumnAndRow(1, $row)->getValue();
				$tosaiBlockKumiku = $worksheet->getCellByColumnAndRow(2, $row)->getValue();
				$chyuNitteiBlockName = $worksheet->getCellByColumnAndRow(3, $row)->getValue();
				$chyuNitteiBlockKumiku = $worksheet->getCellByColumnAndRow(4, $row)->getValue();
			}
			//sort key memory「$エクセル」(excel)
			asort($listExcel);
			$tempKoteiData = array();
			$tempData = array();
			foreach ($listExcel as $excel) {
				$checkTossai = $this->trackChangeTempData($excel, $tosai, $tempData, true);
				$checksShedule = $this->trackChangeTempData($excel, $chunittei, $tempData, false);
				if ($request->val6 == 0) {
					//日程計算方式」で「0:着工日・完工日を使用」を選択している場合
					foreach ($koteiPattern as $kotei) {
						$koteiData = array();
						$koteiData['Kotei'] = $kotei['Kotei'];
						$koteiData['KKumiku'] = $kotei['KKumiku'];
						$koteiData['SDate'] = is_null($excel[$kotei['StartCol']]) ? null
												: $excel[$kotei['StartCol']];
						$koteiData['EDate'] = is_null($excel[$kotei['StartCol'] + 1]) ? null
												: $excel[$kotei['StartCol'] + 1];
						$koteiData['Days'] = is_null($excel[$kotei['StartCol'] + 2]) ? null
												: $excel[$kotei['StartCol'] + 2];
						$koteiData['LinkDays'] = 0;
						$koteiData['Floor'] = is_null($excel[$kotei['StartCol'] + 4]) ? null
												: $excel[$kotei['StartCol'] + 4];
						$koteiData['BD_Code'] = is_null($excel[$kotei['StartCol'] + 5]) ? null
												: $excel[$kotei['StartCol'] + 5];
						if (!is_null($koteiData['SDate']) && $this->isRealDate($koteiData['SDate'])
							&& !is_null($koteiData['EDate']) && $this->isRealDate($koteiData['EDate']))
						{
							$koteiData['Days'] = $timeTrackerCommon->getDateDiff($projectCalendar, $koteiData['SDate'],
																				$koteiData['EDate']);
							$loopIndex = 1;
							$nextProcessStartDate = null;
							while (($loopIndex > count($koteiPattern))
								|| !is_null($nextProcessStartDate))
							{
								$nextProcessStartDate = $excel[$kotei['StartCol'] + ($loopIndex*6)];
								$loopIndex = $loopIndex + 1;
							}
							if (!is_null($nextProcessStartDate))
							{
								$koteiData['LinkDays'] = $timeTrackerCommon->getDateDiff($projectCalendar,
																$koteiData['EDate'], $nextProcessStartDate);
							}
						}
						//「$工程情報」に該当データを追加
						$checksShedule = $this->trackChangeKoteiTempData($excel, $processInformation, $tempKoteiData,
																				 $koteiData, $workItemID, $validated);
					}
				} else {
					//「$基準日」を設定する
					if ($TableType == 0) {
						if ($excel['BKumiku'] >= config('system_const.kumiku_code_ogumi')) {
							// ・メモリ「$エクセル」の[T_Name]、[T_BKumiku]をキーにメモリ「$搭載」を検索する。
							if (isset($tosai[$excel['T_Name'] .'_'. $excel['T_BKumiku']])) {
								$dbData = $tosai[$excel['T_Name'] .'_'. $excel['T_BKumiku']];
								// ●メモリ「$搭載」[T_Date]の値がNULL以外の場合
								if (!is_null($dbData['T_Date'])) {
									$baseDate = $dbData['T_Date'];
								} else {
									// 処理を中断する。
									return $this->stopProcessThenGoTo030501($menuInfo, $request,
																		 config('message.msg_schem_excelimport_001'));
								}
							}
						} else {
							// 「$エクセル」をあいまい検索する。
							$sameGroupData = array_filter($listExcel, function($item) use ($excel) {
								return (
									$item['T_Name'] == $excel['T_Name'] &&
									$item['T_BKumiku'] == $excel['T_BKumiku'] &&
									strpos($item['Name'], mb_substr($excel['Name'], 0, 6)) > -1 &&
									$item['BKumiku'] < config('system_const.kumiku_code_ogumi')
								);
							});
							if (isset($sameGroupData) ) {
								if (isset($koteiPattern['20_'. config('system_const.kumiku_code_ogumi')])) {
									$baseDate = $excel[$koteiPattern['20_'.
									config('system_const.kumiku_code_ogumi')]['StartCol']];
								} else {
									// 処理を中断する。
									return $this->stopProcessThenGoTo030501($menuInfo, $request,
																		config('message.msg_schem_excelimport_001'));
								}
							}else{
								// 処理を中断する。
								return $this->stopProcessThenGoTo030501($menuInfo, $request,
																		config('message.msg_schem_excelimport_001'));
							}
						}
						arsort($koteiPattern);
					}else{
						//中日程データに紐付く最後の工程のWorkItemIDを取得する。
						$tosaiData = $tosai[$excel['T_Name'] .'_'. $excel['T_BKumiku']];
						//select data
						$chunitteiWorkItemId = Cyn_BlockKukaku::select('Cyn_Plan.WorkItemID')
								->join('Cyn_Plan', function($join) {
									$join->on('Cyn_BlockKukaku.ProjectID', '=', 'Cyn_Plan.ProjectID')
										->on('Cyn_BlockKukaku.OrderNo', '=', 'Cyn_Plan.OrderNo')
										->on('Cyn_BlockKukaku.No', '=', 'Cyn_Plan.No');
								})
								->where('Cyn_BlockKukaku.ProjectID', '=', $projectID)
								->where('Cyn_BlockKukaku.OrderNo', '=', $orderNo)
								->where('Cyn_BlockKukaku.CKind', '=', $cKind)
								->where('Cyn_BlockKukaku.WorkItemID', '=', $tosaiData['T_WorkItemID'])
								->where('Cyn_Plan.N_KoteiNo', '=', 0)
								->first();
						if (isset($chunitteiWorkItemId) > 0) {
							$baseDate = $timeTrackerCommon->getKoteiRange($chunitteiWorkItemId,
																		false)[0]['plannedStartDate'];
						}
					}
					// 日程計算
					foreach ($koteiPattern as $kotei)
					{
						$koteiData = array();
						$koteiData['Kotei'] = $kotei['Kotei'];
						$koteiData['KKumiku'] = $kotei['KKumiku'];
						$koteiData['Days'] = $excel[$kotei['StartCol'] + 2];
						$koteiData['LinkDays'] = $excel[$kotei['StartCol'] + 3];
						$koteiData['Floor'] = $excel[$kotei['StartCol'] + 4];
						$koteiData['BD_Code'] = $excel[$kotei['StartCol'] + 5];
						if (!is_null($koteiData['Days']) && is_numeric($koteiData['Days'])
							&& !is_null($koteiData['LinkDays']) && is_numeric($koteiData['LinkDays']))
						{
							$koteiData['EDate'] = $timeTrackerCommon->shiftDate($baseDate, -1*$koteiData['LinkDays'], 
																					$projectCalendar);
							$koteiData['SDate'] = $timeTrackerCommon->shiftDate($koteiData['EDate'],
														-1*$koteiData['Days'], $projectCalendar);
						} else {
							continue;
						}
						$checksShedule = $this->trackChangeKoteiTempData($excel, $processInformation, $tempKoteiData,
																				$koteiData, $workItemID , $validated);
						$excel[$kotei['StartCol']] = $koteiData['SDate'];
						$excel[$kotei['StartCol'] + 1] = $koteiData['EDate'];

						$baseDate = $koteiData['SDate'];
					}
				}
			}
			//Delete
			foreach ($processInformation as $process) {
				$checksShedule = $this->trackDeleteData($process, $tempKoteiData, false, true);
			}
			foreach ($chunittei as $dataChunittei) {
				$checksShedule = $this->trackDeleteData($dataChunittei, $tempData, false, false);
			}
			foreach ($tosai as $temData) {
				$checksShedule = $this->trackDeleteData($temData, $tempData, true, false);
			}
			//Delete temporary data
			$delete = Cyn_Temp_Excel_LogData::where('ProjectID', '=', $projectID)
											->where('OrderNo', '=', $orderNo)
											->where('CKind', '=', $cKind)->delete();
			//Register temporary data
			$LogData = 0;
			foreach ($tempKoteiData as $kotei) {
				$this->insertDataTosai($tempData, $kotei, $LogData, $validated);
				$this->insertChyuNittei($tempData, $kotei, $LogData, $validated);
				$this->insertDataKotei($kotei, $LogData, $validated);
			}
			//Conditions 2
			if ($cKind == 0 || $cKind == 1 || $cKind == 2)
			{
				$url = url('/');
				$url .= '/' . $menuInfo->KindURL;
				$url .= '/' . $menuInfo->MenuURL;
				$url .= '/create';
				$url .= '?cmn1=' . $request->cmn1;
				$url .= '&cmn2=' . $request->cmn2;
				$url .= '&val1=' . valueUrlEncode($request['val1']);
				$url .= '&val2=' . valueUrlEncode($request['val2']);
				$url .= '&val3=' . valueUrlEncode($request['val3']);
				$url .= '&val4=' . valueUrlEncode($request['val4']);
				$url .= '&val6=' . valueUrlEncode($request['val6']);
				$url .= '&val8=' . valueUrlEncode($request['val8']);
				return $url;
			}
		} finally {
			// disconnect Worksheets
			$spreadsheet->disconnectWorksheets();
			$spreadsheet->garbageCollect();
		}


	}
	/**
	 * function stopProcessThenGoTo030501
	 *
	 * @param menuInfo
	 * @param request
	 * @param msg
	 * @return mixed
	 *
	 * @create 2020/11/23　Dung
	 */
	private function stopProcessThenGoTo030501($menuInfo, $request, $msg)
	{
		//「9.排他ロック解除処理」を行う。
		$deleteLock = $this->deleteLock($menuInfo->KindID, config('system_const_schem.sys_menu_id_plan'),
						$menuInfo->SessionID, valueUrlDecode($request->val1));
		//redirect url
		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/index';
		$url .= '?cmn1=' . $request->cmn1;
		$url .= '&cmn2=' . $request->cmn2;
		$url .= '&val1=' . valueUrlEncode($request->val1);
		$url .= '&val2=' . valueUrlEncode($request->val2);
		$url .= '&val3=' . valueUrlEncode($request->val3);
		$url .= '&val4=' . valueUrlEncode($request->val4);
		$url .= '&val5=' . valueUrlEncode($request->val5);
		$url .= '&val6=' . valueUrlEncode($request->val6);
		$url .= '&val8=' . valueUrlEncode($request->val8);
		// has error
		$url .= '&err1=' . valueUrlEncode($msg);

		//「030501_日程取込条件設定画面」に遷移する。
		return $url;
	}

	/**
	 * function check is date
	 *
	 * @param date
	 * @return mixed
	 *
	 * @create 2020/11/23 Dung
	 * @update
	 */
	private function isRealDate($strDate)
	{
		$blnFlag = true;
		$arrTmpDate = explode('/', $strDate);
		if (count($arrTmpDate) == 3) {
			$arrCheckkHasHis = explode(' ', $arrTmpDate[2]);
			if (count($arrCheckkHasHis) == 1) {
				if (!ctype_digit($arrTmpDate[0]) || !ctype_digit($arrTmpDate[1]) || !ctype_digit($arrTmpDate[2])) {
					$blnFlag = false;
				} else {
					preg_match('/([1-9]\d{3}\/([1-9]|0[1-9]|1[0-2])\/([1-9]|0[1-9]|[12]\d|3[01]))/', $strDate, $arrCheckFormatDate);
					if (count($arrCheckFormatDate) == 0) {
						$blnFlag = false;
					} else {
						$blnCheckDate = checkdate($arrTmpDate[1], $arrTmpDate[2], $arrTmpDate[0]);
						if (!$blnCheckDate) {
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
	 * function check change koteiTempData
	 *
	 * @param date
	 * @return mixed
	 *
	 * @create 2020/11/23 Dung
	 * @update
	 */
	private function trackChangeKoteiTempData($excel, $referData, &$arrTemp, $koteiData, $workItemID, $validated)
	{
		$data = array();
		$data['T_Name'] = $excel['T_Name'];
		$data['T_BKumiku'] = $excel['T_BKumiku'];
		$data['Name'] = $excel['Name'];
		$data['BKumiku'] = $excel['BKumiku'];
		$data['Gen'] = (mb_strlen($excel['T_Name']) >= 13
						&& in_array(mb_substr($excel['T_Name'], 12, 1), array('P', 'S', 'C')))
						? mb_substr($excel['T_Name'], 12, 1) : null;
		$data['Kotei'] = $koteiData['Kotei'];
		$data['KKumiku'] = $koteiData['KKumiku'];
		$data['SDate'] = is_null($koteiData['SDate']) ? null : $koteiData['SDate'];
		$data['EDate'] = is_null($koteiData['EDate']) ? null : $koteiData['EDate'];
		$data['Days'] = $koteiData['Days'];
		$data['LinkDays'] = $koteiData['LinkDays'];
		$data['Floor'] = is_null( $koteiData['Floor']) ? null :  $koteiData['Floor'];
		$data['BD_Code'] = is_null($koteiData['BD_Code']) ? null :  $koteiData['BD_Code'];
		if (count($referData) > 0) {
			$isDelete = ($validated['val6'] == 0 && (is_null($data['SDate']) || is_null($data['EDate'])))
						|| ($validated['val6'] == 1 && (is_null($data['Days']) || '0' == $data['Days']));

			if (isset($referData[$data['T_Name'] .'_'. $data['T_BKumiku']
							.'_'.$data['Name'] . '_' . $data['BKumiku']
							.'_'.$data['Kotei'] . '_' . $data['KKumiku']])) {
				// 「$工程情報」に該当データが存在する場合
				$dbData = $referData[$data['T_Name'] .'_'. $data['T_BKumiku']
								.'_'.$data['Name'] . '_' . $data['BKumiku']
								.'_'.$data['Kotei'] . '_' . $data['KKumiku']];
				// ・「$着工日」がNULL、または「$完工日」がNULLの場合
				if ($isDelete)
				{
					// 「$工程情報」に該当データが存在しない場合
					$data['WorkItemID'] = $dbData['P_WorkItemID'];
					$data['AMDFlag'] = 2;
					$data['Log'] = sprintf(config('message.msg_schem_excelimport_004'), $data['Name'], $data['Kotei']);
					$arrTemp[$data['T_Name'] .'_'. $data['T_BKumiku'] .'_'. $data['Name'] .'_'
							. $data['BKumiku'] .'_'. $data['Kotei'] .'_'. $data['KKumiku']] = $data;
				}
				else
				{
					$timeTrackerData = $workItemID[$dbData['P_WorkItemID']];
					$changeItemName = null;
					if ($timeTrackerData['plannedStartDate'] != $data['SDate'])
					{
						$changeItemName = '着工日';
					}
					else if ($timeTrackerData['plannedFinishDate'] != $data['EDate'])
					{
						$changeItemName = '完工日';
					}
					else if ($timeTrackerData['workDays'] != $data['Days'])
					{
						$changeItemName = '工期';
					}
					else if ($dbData['Floor'] != $data['Floor'])
					{
						$changeItemName = '施工棟';
					}
					else if ($dbData['BD_Code'] != $data['BD_Code'])
					{
						$changeItemName = '管理物量コード';
					}

					if (isset($changeItemName))
					{
						$data['WorkItemID'] = $dbData['P_WorkItemID'];
						$data['AMDFlag'] = 1;
						$data['Log'] = sprintf(config('message.msg_schem_excelimport_008'), $data['Name'],
											$data['Kotei'], $changeItemName);

						$arrTemp[$data['T_Name'] .'_'. $data['T_BKumiku'] .'_'. $data['Name'] .'_'
								. $data['BKumiku'] .'_'. $data['Kotei'] .'_'. $data['KKumiku']] = $data;
					}
					else
					{
						$data['WorkItemID'] = $dbData['P_WorkItemID'];
						$data['AMDFlag'] = -1;
						$data['Log'] = null;

						$arrTemp[$data['T_Name'] .'_'. $data['T_BKumiku'] .'_'. $data['Name'] .'_'
								. $data['BKumiku'] .'_'. $data['Kotei'] .'_'. $data['KKumiku']] = $data;
					}
				}
			}
			else
			{
				// 「$工程情報」に該当データが存在しない場合
				$data['WorkItemID'] = null;
				$data['AMDFlag'] = 0;
				$data['Log'] = sprintf(config('message.msg_schem_excelimport_007'), $data['Name'], $data['Kotei']);
				$arrTemp[$data['T_Name'] .'_'. $data['T_BKumiku'] .'_'. $data['Name'] .'_'
						. $data['BKumiku'] .'_'. $data['Kotei'] .'_'. $data['KKumiku']] = $data;
			}
		}
	}

	/**
	 * function check change koteiTempData
	 *
	 * @param excel
	 * @param referData
	 * @param arrTemp
	 * @param isTosai
	 * @return mixed
	 *
	 * @create 2020/11/23 Dung
	 * @update
	 */
	private function trackChangeTempData($excel, $referData, &$arrTemp, $isTosai)
	{
		if (count($referData) > 0) {
			$data = array();
			$data['T_Name'] = $excel['T_Name'];
			$data['T_BKumiku'] = $excel['T_BKumiku'];
			$data['Name'] = !$isTosai ? $excel['Name'] : null;
			$data['BKumiku'] = !$isTosai ? $excel['BKumiku'] : null;
			$data['Gen'] = (mb_strlen($excel['T_Name']) >= 13
							&& in_array(mb_substr($excel['T_Name'], 12, 1), array('P', 'S', 'C')))
							? mb_substr($excel['T_Name'], 12, 1) : null;
			$data['Kotei'] = null;
			$data['KKumiku'] = null;
			$data['SDate'] = null;
			$data['EDate'] = null;
			$data['Days'] = null;
			$data['LinkDays'] = null;
			$data['Floor'] = null;
			$data['BD_Code'] = null;
			if ($isTosai) {
				if (isset($referData[$data['T_Name'] .'_'. $data['T_BKumiku']])) {
					$dbData = $referData[$data['T_Name'] .'_'. $data['T_BKumiku']];

					$data['WorkItemID'] = $dbData['T_WorkItemID'];
					$data['AMDFlag'] = -1;
					$data['Log'] = null;
					$arrTemp[$data['T_Name'] . '_' . $data['T_BKumiku'] . '_' . $data['Name'] . '_' . $data['BKumiku']] = $data;
				} else {
					if (!isset($arrTemp[$data['T_Name'] . '_' . $data['T_BKumiku']]))
					{
						$data['WorkItemID'] = null;
						$data['AMDFlag'] = 0;
						$data['Log'] = sprintf(config('message.msg_schem_excelimport_002'), $excel['T_Name']);
						$arrTemp[$data['T_Name'] . '_' . $data['T_BKumiku']] = $data;
					}
				}
			} else {
				if (isset($referData[$data['T_Name'] .'_'. $data['T_BKumiku'].'_'.$data['Name'] . '_' . $data['BKumiku']])) {
					$dbData = $referData[$data['T_Name'] .'_'. $data['T_BKumiku'].'_'.$data['Name'] . '_' . $data['BKumiku']];

					$data['WorkItemID'] = $dbData['B_WorkItemID'];
					$data['AMDFlag'] = -1;
					$data['Log'] = null;

					$arrTemp[$data['T_Name'] . '_' . $data['T_BKumiku'] . '_' . $data['Name'] . '_' . $data['BKumiku']] = $data;
				} else {
					if (!isset($arrTemp[$data['T_Name'] . '_' . $data['T_BKumiku'] . '_' . $data['Name'] . '_' . $data['BKumiku']]))
					{
						$data['WorkItemID'] = null;
						$data['AMDFlag'] = 0;
						$data['Log'] = sprintf(config('message.msg_schem_excelimport_003'), $excel['T_Name'], $excel['Name']);

						$arrTemp[$data['T_Name'] . '_' . $data['T_BKumiku'] . '_' . $data['Name'] . '_' . $data['BKumiku']] = $data;
					}
				}
			}
		}
	}
	/**
	 * function check delete data
	 *
	 * @param referData
	 * @param arrTemp
	 * @param isTosai
	 * @param isKotei
	 * @return mixed
	 *
	 * @create 2020/11/23 Dung
	 * @update
	 */
	private function trackDeleteData($referData, &$arrTemp, $isTosai, $isKotei)
	{
		$data = array();
		$data['T_Name'] = $referData['T_Name'];
		$data['T_BKumiku'] = $referData['T_BKumiku'];
		$data['Name'] = !$isTosai ? $referData['Name'] : null;
		$data['BKumiku'] = !$isTosai ? $referData['BKumiku'] : null;
		$data['Gen'] = (mb_strlen($referData['T_Name']) >= 13
						&& in_array(mb_substr($referData['T_Name'], 12, 1),
						array('P', 'S', 'C'))) ? mb_substr($referData['T_Name'], 12, 1)
						: null;
		$data['Kotei'] = $isKotei ? $referData['Kotei'] : null;
		$data['KKumiku'] = $isKotei ? $referData['KKumiku'] : null;
		$data['SDate'] = null;
		$data['EDate'] = null;
		$data['Days'] = null;
		$data['LinkDays'] = null;
		$data['Floor'] = $isKotei ? $referData['Floor'] : null;
		$data['BD_Code'] = $isKotei ? $referData['BD_Code'] : null;

		if (count($referData) > 0) {
			if ($isKotei) {
				if (!isset($arrTemp[$data['T_Name'] .'_'. $data['T_BKumiku'] .'_'. $data['Name'] .'_'
									. $data['BKumiku'] .'_'. $data['Kotei'] .'_'. $data['KKumiku']])) {
					$data['WorkItemID'] = $referData['P_WorkItemID'];
					$data['AMDFlag'] = 2;
					$data['Log'] = sprintf(config('message.msg_schem_excelimport_004'), $data['Name'], $data['Kotei']);

					$arrTemp[$data['T_Name'] .'_'. $data['T_BKumiku'] .'_'. $data['Name'] .'_'
							. $data['BKumiku'] .'_'. $data['Kotei'] .'_'. $data['KKumiku']] = $data;
				}
			}
			else if ($isTosai) {
				if (!isset($arrTemp[$data['T_Name'] . '_' . $data['T_BKumiku']]))
				{
					$data['WorkItemID'] = $referData['T_WorkItemID'];
					$data['AMDFlag'] = 2;
					$data['Log'] = sprintf(config('message.msg_schem_excelimport_006'), $data['T_Name']);
					$arrTemp[$data['T_Name'] . '_' . $data['T_BKumiku']] = $data;
				}
			} else {
				if (!isset($arrTemp[$data['T_Name'] . '_' . $data['T_BKumiku'] . '_' . $data['Name'] . '_' . $data['BKumiku']]))
				{
					$data['WorkItemID'] = $referData['B_WorkItemID'];
					$data['AMDFlag'] = 2;
					$data['Log'] = sprintf(config('message.msg_schem_excelimport_005'), $data['T_Name'] , $data['Name'] );
				$arrTemp[$data['T_Name'] . '_' . $data['T_BKumiku'] . '_' . $data['Name'] . '_' . $data['BKumiku']] = $data;
				}
			}
		}
	}
	/**
	 * function check insert data
	 *
	 * @param tempData
	 * @param kotei
	 * @param LogData
	 * @param validated
	 * @return mixed
	 *
	 * @create 2020/11/23 Dung
	 * @update
	 */
	private function insertDataTosai (&$tempData, $kotei, &$LogData, $validated)
	{
		$arrData = array_filter($tempData, function($key) use($kotei) {
			return $key == $kotei['T_Name'] . '_' . $kotei['T_BKumiku'];
		}, ARRAY_FILTER_USE_KEY);

		if (count($arrData) > 0) {
			$data1 = array_filter($arrData, function($value) {
				return $value['AMDFlag'] >= 0;
			});
			if (count($data1) > 0) {
				foreach($data1 as $key1=>$data) {
					$LogData ++;
					$obj = new Cyn_Temp_Excel_LogData;
					$obj->ProjectID = $validated['val3'];
					$obj->OrderNo = $validated['val4'];
					$obj->CKind = $validated['val1'];
					$obj->WorkItemID = is_null($data['WorkItemID']) ? null : $data['WorkItemID'] ;
					$obj->ID = $LogData;
					$obj->Log = is_null($data['Log']) ? null : $data['Log'] ;
					$obj->T_Name = is_null($data['T_Name']) ? null : $data['T_Name'] ;
					$obj->T_BKumiku = is_null($data['T_BKumiku']) ? null : $data['T_BKumiku'] ;
					$obj->Name = null;
					$obj->BKumiku = null;
					$obj->Gen = is_null($data['Gen']) ? null : $data['Gen'] ;
					$obj->Kotei = null;
					$obj->KKumiku = null;
					$obj->SDate = is_null($data['SDate']) ? null : $data['SDate'] ;
					$obj->EDate = is_null($data['EDate']) ? null : $data['EDate'] ;
					$obj->Days = is_null($data['Days']) ? null : $data['Days'] ;
					$obj->LinkDays = is_null($data['LinkDays']) ? null : $data['LinkDays'] ;
					$obj->Floor = is_null($data['Floor']) ? null : $data['Floor'];
					$obj->BD_Code = is_null($data['BD_Code']) ? null : $data['BD_Code'];
					$obj->AMDFlag = is_null($data['AMDFlag']) ? null : $data['AMDFlag'] ;
					$obj->save();
					// Set memory「$tempKoteiData」[T_Name]、[T_BKumiku] is key, delete form memory[$tempData]
					unset($tempData[$key1]);
				}
			}
			$data2 = array_filter($arrData, function($value) {
				return $value['AMDFlag'] == -1;
			});

			if (count($data2) > 0) {
				foreach($data2 as $key2=>$data) {
					// Set memory「$tempKoteiData」[T_Name]、[T_BKumiku] is key, delete form memory[$tempData]
					unset($tempData[$key2]);
				}
			}
		}
	}
	/**
	 * function check insert data chunitei
	 *
	 * @param tempData
	 * @param kotei
	 * @param LogData
	 * @param validated
	 * @return mixed
	 *
	 * @create 2020/11/23 Dung
	 * @update
	 */
	private function insertChyuNittei (&$tempData, $kotei, &$LogData, $validated)
	{
		$arrData = array_filter($tempData, function($key) use($kotei) {
			return $key == $kotei['T_Name'] . '_' . $kotei['T_BKumiku'] . '_' .  $kotei['Name'] . '_' . $kotei['BKumiku'];
		}, ARRAY_FILTER_USE_KEY);

		if (count($arrData) > 0) {
			$dataChyu1 = array_filter($arrData, function($value) {
				return $value['AMDFlag'] >= 0;
			});

			if (count($dataChyu1) > 0) {
				foreach($dataChyu1 as $key1=>$data) {
					$LogData ++;
					$obj = new Cyn_Temp_Excel_LogData;
					$obj->ProjectID = $validated['val3'];
					$obj->OrderNo = $validated['val4'];
					$obj->CKind = $validated['val1'];
					$obj->WorkItemID = is_null($data['WorkItemID']) ? null : $data['WorkItemID'] ;
					$obj->ID = $LogData;
					$obj->Log = is_null($data['Log']) ? null : $data['Log'] ;
					$obj->T_Name = is_null($data['T_Name']) ? null : $data['T_Name'] ;
					$obj->T_BKumiku = is_null($data['T_BKumiku']) ? null : $data['T_BKumiku'] ;
					$obj->Name = is_null($data['Name']) ? null : $data['Name'];
					$obj->BKumiku = is_null($data['BKumiku']) ? null : $data['BKumiku'];
					$obj->Gen = is_null($data['Gen']) ? null : $data['Gen'] ;
					$obj->Kotei = null;
					$obj->KKumiku = null;
					$obj->SDate = is_null($data['SDate']) ? null : $data['SDate'] ;
					$obj->EDate = is_null($data['EDate']) ? null : $data['EDate'] ;
					$obj->Days = is_null($data['Days']) ? null : $data['Days'] ;
					$obj->LinkDays = is_null($data['LinkDays']) ? null : $data['LinkDays'] ;
					$obj->Floor = is_null($data['Floor']) ? null : $data['Floor'];
					$obj->BD_Code = is_null($data['BD_Code']) ? null : $data['BD_Code'];
					$obj->AMDFlag = is_null($data['AMDFlag']) ? null : $data['AMDFlag'] ;
					$obj->save();
					// Set memory「$tempKoteiData」[T_Name]、[T_BKumiku] is key, delete form memory[$tempData]
					unset($tempData[$key1]);
				}
			}
			$dataChyu2 = array_filter($arrData, function($value) {
				return $value['AMDFlag'] == -1;
			});
			if (count($dataChyu2) > 0) {
				foreach($dataChyu2 as $key2=>$data) {
					// Set memory「$tempKoteiData」[T_Name]、[T_BKumiku] is key, delete form memory[$tempData]
					unset($tempData[$key2]);
				}
			}
		}
	}
	/**
	 * function check insert data kotei
	 *
	 * @param kotei
	 * @param LogData
	 * @param validated
	 * @return mixed
	 *
	 * @create 2020/11/23 Dung
	 * @update
	 */
	private function insertDataKotei ($kotei, &$LogData, $validated)
	{
		if ($kotei['AMDFlag'] >= 0) {
			$LogData ++;
			$obj = new Cyn_Temp_Excel_LogData;
			$obj->ProjectID = $validated['val3'];
			$obj->OrderNo = $validated['val4'];
			$obj->CKind = $validated['val1'];
			$obj->WorkItemID = is_null($kotei['WorkItemID']) ? null : $kotei['WorkItemID'] ;
			$obj->ID = $LogData;
			$obj->Log = is_null($kotei['Log']) ? null : $kotei['Log'] ;
			$obj->T_Name = is_null($kotei['T_Name']) ? null : $kotei['T_Name'] ;
			$obj->T_BKumiku = is_null($kotei['T_BKumiku']) ? null : $kotei['T_BKumiku'] ;
			$obj->Name = is_null($kotei['Name']) ? null : $kotei['Name'];
			$obj->BKumiku = is_null($kotei['BKumiku']) ? null : $kotei['BKumiku'];
			$obj->Gen = is_null($kotei['Gen']) ? null : $kotei['Gen'] ;
			$obj->Kotei = is_null($kotei['Kotei']) ? null : $kotei['Kotei'];
			$obj->KKumiku = is_null($kotei['KKumiku']) ? null : $kotei['KKumiku'];
			$obj->SDate = is_null($kotei['SDate']) ? null : $kotei['SDate'] ;
			$obj->EDate = is_null($kotei['EDate']) ? null : $kotei['EDate'] ;
			$obj->Days = is_null($kotei['Days']) ? null : $kotei['Days'] ;
			$obj->LinkDays = is_null($kotei['LinkDays']) ? null : $kotei['LinkDays'] ;
			$obj->Floor = is_null($kotei['Floor']) ? null : $kotei['Floor'];
			$obj->BD_Code = is_null($kotei['BD_Code']) ? null : $kotei['BD_Code'];
			$obj->AMDFlag = is_null($kotei['AMDFlag']) ? null : $kotei['AMDFlag'] ;
			$obj->save();
		}
	}
	/**
	 * GET importData create button action
	 *
	 * @param Request
	 * @return View
	 *
	 * @create 2020/12/28 Dung
	 * @update
	 */
	public function create(Request $request)
	{
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		//initialize $originalError
		$originalError = [];
		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
		}
		$rows = Cyn_Temp_Excel_LogData::select( 'Name as fld1', 'BKumiku as fld2', 'Gen as fld3', 'KKumiku as fld4',
												'Kotei as fld5', 'Log as fld6','T_Name as fld7', 'T_BKumiku as fld8'
												)
		->where('ProjectID', '=', valueUrlDecode($request->val3))
		->where('OrderNo', '=', valueUrlDecode($request->val4))
		->where('CKind', '=',valueUrlDecode($request->val1))->orderBy('ID', 'asc')->get();

		$koteiMaster = Cyn_mstKotei::select('Code','Name as fld1')->where('CKind', '=', valueUrlDecode($request->val1))
			->where('DelFlag' ,'=', False)
			->orderBy('Code', 'asc')->get()->toArray();

		if (count($rows) > 0) {
			foreach($rows as $value) {
				$value->fld1 = is_null($value->fld1) ? $value->fld7 : $value->fld1 ;
				$resKumiku1 = FuncCommon::getKumikuData(is_null($value->fld2) ? $value->fld8 : $value->fld2);
				$value->fld2 = isset($resKumiku1[2]) ? $resKumiku1[2] : null;

				$value->fld3 = $value->fld3;

				$resKumiku2 = FuncCommon::getKumikuData($value->fld4);
				$value->fld4 = isset($resKumiku2[2]) ? $resKumiku2[2] : null;

				if (!is_null($value->fld5)) {
					$kotei = array_values(array_filter($koteiMaster, function($item) use($value) {
					return $item['Code'] == $value->fld5;
				}));
				if (count($kotei) > 0) {
					$value->fld5 = $value->fld5.config('system_const.code_name_separator'). $kotei[0]['fld1'];
				}else {
					$value->fld5 = null;
				}
				}else {
					$value->fld5 = null;
				}
				$value->fld6 = $value->fld6;
			}

		}
		//pageunit != 10 -> pageunit = 10
		if (isset($request->val8) && in_array(valueUrlDecode($request->val8), [config('system_const.displayed_results_1'),
																	config('system_const.displayed_results_2'),
																	config('system_const.displayed_results_3')])) {
		$pageunit =valueUrlDecode($request->val8);
		}else{
			$pageunit = config('system_const.displayed_results_1');
		}
		// update rev2
		$sort = ['fld4','fld1'];
		if (isset($request->sort) && $request->sort != '') {
			if (trim($request->sort) == 'fld4') {
				$sort = ['fld4','fld1'];
			} else if (trim($request->sort) == 'fld1') {
				$sort = ['fld1'];
			} else {
				$sort = [$request->sort, 'fld1'];
			}
		}
		$direction = (isset($request->direction) && $request->direction != '') ?  $request->direction : 'asc';
		$query = $this->sortAndPagination($rows, $sort, $direction, $pageunit, $request);
		//get list of floors
		$this->data['rows'] = $query;
		$this->data['menuInfo'] = $menuInfo;
		$this->data['originalError'] = $originalError;
		$this->data['request'] = $request;
		//return view with all data
		return view('Schem/ExcelImport/create', $this->data);
	}

	/**
	 * POST data
	 *
	 * @param Request
	 * @return View
	 *
	 * @create 2020/12/30　Dung
	 * @update
	 */
	public function accept(Request $request)
	{
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		//$originalError = [];
		$projectID = valueUrlDecode($request->val3);
		$orderNo = valueUrlDecode($request->val4);
		$cKind = valueUrlDecode($request->val1);
		$timeTrackerFuncSchem = new TimeTrackerFuncSchem();
		//condition 3
		$historyID = Cyn_Excel_History::selectRaw('MAX(ID) as MaxID')->first();
		$lastID  = is_null($historyID) ? 0 : $historyID->MaxID;
		$timeTrackerCommon = new TimeTrackerCommon();
		//convert GetDate
		$dateNow = DB::selectOne('SELECT CONVERT(DATETIME, getdate()) AS sysdate')->sysdate;
		//insert to Cyn_Excel_History
		DB::transaction(function () use($request, $menuInfo,  $lastID ,$dateNow , $projectID, $orderNo) {
			$objHistory = new Cyn_Excel_History;
			$objHistory->ID = $lastID + 1;
			$objHistory->Import_User = $menuInfo->UserID;
			$objHistory->Import_Date = $dateNow;
			$objHistory->ProjectID = $projectID;
			$objHistory->OrderNo = $orderNo;
			$objHistory->StatusFlag = 0;
			$objHistory->save();
		});
		$projectCalendar = $timeTrackerCommon->getCalendar($projectID);
		$rootWorkItemID = $timeTrackerCommon->getOrderRoot($projectID , $orderNo);
		if (is_string($rootWorkItemID) || !is_numeric($rootWorkItemID)) {
			$originalError = $rootWorkItemID;
			Cyn_Excel_History::where('ID', '=', $lastID)->update(['StatusFlag' => -1]);
			$url = url('/');
			$url .= '/' . $menuInfo->KindURL;
			$url .= '/' . $menuInfo->MenuURL;
			$url .= '/create';
			$url .= '?cmn1=' . $request->cmn1;
			$url .= '&cmn2=' . $request->cmn2;
			$url .= '&val1=' . $request['val1'];
			$url .= '&val2=' . $request['val2'];
			$url .= '&val3=' . $request['val3'];
			$url .= '&val4=' . $request['val4'];
			$url .= '&val6=' . $request['val6'];
			$url .= '&val8=' . $request['val8'];
			$url .= '&err1=' . valueUrlEncode($originalError);
			return redirect($url);
		}
		$itemData1 = Cyn_TosaiData::select(
			'Cyn_TosaiData.Name as T_Name',
			'Cyn_TosaiData.BKumiku as T_BKumiku',
			'Cyn_TosaiData.WorkItemID as T_WorkItemID',

			'Cyn_BlockKukaku.Name',
			'Cyn_BlockKukaku.BKumiku',
			'Cyn_BlockKukaku.WorkItemID as B_WorkItemID',
			)
			->selectRaw('\'0\' as TableType')
			->join('Cyn_BlockKukaku', function($join) {
				$join->on('Cyn_TosaiData.ProjectID', '=', 'Cyn_BlockKukaku.ProjectID')
					->on('Cyn_TosaiData.OrderNo', '=', 'Cyn_BlockKukaku.OrderNo')
					->on('Cyn_TosaiData.CKind', '=', 'Cyn_BlockKukaku.CKind')
					->on('Cyn_TosaiData.Name', '=', 'Cyn_BlockKukaku.T_Name')
					->on('Cyn_TosaiData.BKumiku', '=', 'Cyn_BlockKukaku.T_BKumiku');
				})
			->where('Cyn_TosaiData.ProjectID', '=', $projectID)
			->where('Cyn_TosaiData.OrderNo', '=', $orderNo)
			->where('Cyn_TosaiData.CKind', '=', $cKind)
			->where('Cyn_TosaiData.WorkItemID', '<>' , 0);

		$itemData2 = Cyn_BlockKukaku::select(
			'Cyn_BlockKukaku.Name as T_Name',
			'Cyn_BlockKukaku.BKumiku as T_BKumiku',
			'Cyn_BlockKukaku.WorkItemID as T_WorkItemID',

			'Cyn_C_BlockKukaku.Name',
			'Cyn_C_BlockKukaku.BKumiku',
			'Cyn_C_BlockKukaku.WorkItemID as P_WorkItemID',

			)
			->selectRaw('\'1\' as TableType')
			->join('Cyn_C_BlockKukaku', function($join) {
				$join->on('Cyn_BlockKukaku.ProjectID', '=', 'Cyn_C_BlockKukaku.ProjectID')
					->on('Cyn_BlockKukaku.OrderNo', '=', 'Cyn_C_BlockKukaku.OrderNo')
					->on('Cyn_BlockKukaku.CKind', '=', 'Cyn_C_BlockKukaku.CKind')
					->on('Cyn_BlockKukaku.Name', '=', 'Cyn_C_BlockKukaku.T_Name')
					->on('Cyn_BlockKukaku.BKumiku', '=', 'Cyn_C_BlockKukaku.T_BKumiku');
				})
			->where('Cyn_BlockKukaku.ProjectID', '=', $projectID)
			->where('Cyn_BlockKukaku.OrderNo', '=', $orderNo)
			->where('Cyn_BlockKukaku.CKind', '=', $cKind);

		$itemData = $itemData1->union($itemData2)
						->orderBy('T_Name', 'asc')
						->orderBy('T_BKumiku', 'asc')
						->orderBy('Name', 'asc')
						->orderBy('BKumiku', 'asc')
						->get();

		if (count($itemData) > 0) {
			foreach($itemData as $record) {
				$listWorkItemID[$record->T_Name.'_'.$record->T_BKumiku] = $record->T_WorkItemID;
				$listWorkItemID[$record->T_Name.'_'.$record->T_BKumiku.'_'.$record->Name.'_'.$record->BKumiku]
								= $record->B_WorkItemID;
								$TableType = $record->TableType;
			}
		}

		//set select data
		$dataNo1 = Cyn_TosaiData::select(
			'Cyn_TosaiData.Name as T_Name',
			'Cyn_TosaiData.BKumiku as T_BKumiku',
			'Cyn_BlockKukaku.Name',
			'Cyn_BlockKukaku.BKumiku',
			'Cyn_BlockKukaku.No',
			)
			->selectRaw('\'0\' as T_No')
			->selectRaw("MAX (Cyn_Plan.KoteiNo) OVER (PARTITION BY Cyn_BlockKukaku.No) AS Max_No")
			->join('Cyn_BlockKukaku', function($join) {
				$join->on('Cyn_TosaiData.ProjectID', '=', 'Cyn_BlockKukaku.ProjectID')
					->on('Cyn_TosaiData.OrderNo', '=', 'Cyn_BlockKukaku.OrderNo')
					->on('Cyn_TosaiData.CKind', '=', 'Cyn_BlockKukaku.CKind')
					->on('Cyn_TosaiData.Name', '=', 'Cyn_BlockKukaku.T_Name')
					->on('Cyn_TosaiData.BKumiku', '=', 'Cyn_BlockKukaku.T_BKumiku');
				})
			->leftJoin('Cyn_Plan','Cyn_BlockKukaku.No', '=', 'Cyn_Plan.No')
			->where('Cyn_TosaiData.ProjectID', '=', $projectID)
			->where('Cyn_TosaiData.OrderNo', '=', $orderNo)
			->where('Cyn_TosaiData.CKind', '=', $cKind)
			->where('Cyn_TosaiData.WorkItemID', '<>' , 0);
		$dataNo2 = Cyn_BlockKukaku::select(
			'Cyn_BlockKukaku.Name as T_Name',
			'Cyn_BlockKukaku.BKumiku as T_BKumiku',
			'Cyn_C_BlockKukaku.Name',
			'Cyn_C_BlockKukaku.BKumiku',
			'Cyn_C_BlockKukaku.No',
			'Cyn_BlockKukaku.No as T_No',
			)
			->selectRaw(" MAX (Cyn_C_Plan.KoteiNo) OVER (PARTITION BY Cyn_C_BlockKukaku.No) AS Max_No")
			->join('Cyn_C_BlockKukaku', function($join) {
				$join->on('Cyn_BlockKukaku.ProjectID', '=', 'Cyn_C_BlockKukaku.ProjectID')
					->on('Cyn_BlockKukaku.OrderNo', '=', 'Cyn_C_BlockKukaku.OrderNo')
					->on('Cyn_BlockKukaku.CKind', '=', 'Cyn_C_BlockKukaku.CKind')
					->on('Cyn_BlockKukaku.Name', '=', 'Cyn_C_BlockKukaku.T_Name')
					->on('Cyn_BlockKukaku.BKumiku', '=', 'Cyn_C_BlockKukaku.T_BKumiku');
			})
			->leftJoin('Cyn_C_Plan','Cyn_C_BlockKukaku.No', '=', 'Cyn_C_Plan.No')

			->where('Cyn_BlockKukaku.ProjectID', '=', $projectID)
			->where('Cyn_BlockKukaku.OrderNo', '=', $orderNo)
			->where('Cyn_BlockKukaku.CKind', '=', $cKind);
		$dataNo = $dataNo1->union($dataNo2)
						->orderBy('T_Name')
						->orderBy('T_BKumiku')
						->orderBy('Name')
						->orderBy('BKumiku')
						->get();
		if (count($dataNo) > 0) {
			foreach($dataNo as $record) {
				$listNo[$record->T_Name.'_'.$record->T_BKumiku] = $record->T_No;
				$listNo[$record->T_Name.'_'.$record->T_BKumiku.'_'.$record->Name.'_'.$record->BKumiku] = $record->No;
				$maxNo[$record->T_Name.'_'.$record->T_BKumiku.'_'.$record->Name.'_'.$record->BKumiku] = $record->Max_No;
			}
		}
		$temLogExcelImport = Cyn_Temp_Excel_LogData::select('Cyn_Temp_Excel_LogData.WorkItemID','Cyn_Temp_Excel_LogData.ID',
														'Cyn_Temp_Excel_LogData.T_Name','Cyn_Temp_Excel_LogData.T_BKumiku',
														'Cyn_Temp_Excel_LogData.Name','Cyn_Temp_Excel_LogData.BKumiku',
														'Cyn_Temp_Excel_LogData.Gen','Cyn_Temp_Excel_LogData.Kotei',
														'Cyn_Temp_Excel_LogData.KKumiku','Cyn_Temp_Excel_LogData.Log',
														'Cyn_Temp_Excel_LogData.SDate','Cyn_Temp_Excel_LogData.EDate',
														'Cyn_Temp_Excel_LogData.Days','Cyn_Temp_Excel_LogData.LinkDays',
														'Cyn_Temp_Excel_LogData.Floor','Cyn_Temp_Excel_LogData.BD_Code',
														'Cyn_Temp_Excel_LogData.AMDFlag',
														'Cyn_mstKotei.Name as KoteiName')
													->leftJoin('Cyn_mstKotei', function($join) {
														$join->on('Cyn_Temp_Excel_LogData.Kotei', '=', 'Cyn_mstKotei.Code')
															->on('Cyn_Temp_Excel_LogData.CKind', '=', 'Cyn_mstKotei.CKind');
													})
													->where('Cyn_Temp_Excel_LogData.ProjectID', '=', $projectID)
													->where('Cyn_Temp_Excel_LogData.OrderNo', '=', $orderNo)
													->where('Cyn_Temp_Excel_LogData.CKind', '=', $cKind)
													->orderBy('Cyn_Temp_Excel_LogData.ID')->get();

		$idCynExcelLogData = Cyn_Excel_LogData::selectRaw('MAX(ID) as MaxID')->first();

		$maxExcelLogDataID = is_null($idCynExcelLogData->MaxID) ? 1 : ($idCynExcelLogData->MaxID + 1);

		DB::transaction(function () use($temLogExcelImport, $lastID, $maxExcelLogDataID) {
			foreach ($temLogExcelImport as $data ) {
				$objHistory = new Cyn_Excel_LogData;
				$objHistory->ID = $maxExcelLogDataID;
				$objHistory->HistoryID = $lastID + 1;
				$objHistory->Name = is_null($data->Name) ? $data->T_Name : $data->Name;
				$objHistory->BKumiku = is_null($data->Name) ? $data->T_BKumiku : $data->BKumiku;
				$objHistory->Kotei = is_null($data->Kotei) ? null : $data->Kotei;
				$objHistory->KKumiku = is_null($data->Kotei) ? null : $data->KKumiku;
				$objHistory->SDate = is_null($data->SDate) ? null : $data->SDate;
				$objHistory->Days = is_null($data->Days) ? null : $data->Days;
				$objHistory->LinkDays = is_null($data->LinkDays) ? null : $data->LinkDays;
				$objHistory->Floor = is_null($data->Floor) ? null : $data->Floor;
				$objHistory->BD_Code = is_null($data->BD_Code) ? null : $data->BD_Code;
				$objHistory->Log = is_null($data->Log) ? null : $data->Log;
				$objHistory->save();
				$maxExcelLogDataID++;
			}
		});
		//・[AMDFlag] of 「TemLogExcelImport」 is 「0」（追加）
		$dataHasAMDFlag0 = array_filter($temLogExcelImport->toArray(), function($item) {
			return $item['AMDFlag'] == 0 && !is_null($item['AMDFlag'])  ;
		});
		$maxNoBlockKukaku = Cyn_BlockKukaku::selectRaw('MAX(No) as MaxNo')->first();
		$maxNo  = is_null($maxNoBlockKukaku) ? 1 : ($maxNoBlockKukaku->MaxNo + 1);
		$listMaxNo = array();

		DB::transaction(function () use($dataHasAMDFlag0, $rootWorkItemID, $maxNo, $listMaxNo, $timeTrackerFuncSchem,
										 $projectID, $orderNo, $cKind, $projectCalendar, $TableType, $listWorkItemID) {
			foreach ($dataHasAMDFlag0 as $data ) {
				$parentWorkItemID = 0;
				$name = '';
				// ① select 「$parentWorkItemID」、「$name」
				if (is_null($data['Name']) && is_null($data['Kotei'])) {
					$parentWorkItemID = $rootWorkItemID;
					$name = $data['T_Name'];
				}
				if (!is_null($data['Name']) && is_null($data['Kotei'])) {
					$key =  $data['T_Name'].'_'. $data['T_BKumiku'];
					if (array_key_exists($key, $listWorkItemID)) {

						$parentWorkItemID = $listWorkItemID[$key];
						$name = $data['Name'];
					}
				}
				if (!is_null($data['Kotei'])) {
					$key =  $data['T_Name'].'_'. $data['T_BKumiku'].'_'. $data['Name'].'_'. $data['BKumiku'];
					if (array_key_exists($key, $listWorkItemID)) {
						$parentWorkItemID = $listWorkItemID[$key];
						$name = $data['KoteiName'];
					}
				}
				// ② insert data of TimeTrackerNX
				$resulTimeTK = $timeTrackerFuncSchem->insertPlan($projectID, $orderNo, $parentWorkItemID, NULL,
																$data['SDate'], $data['EDate'], $name, $projectCalendar);
				if (!is_string($resulTimeTK)) {
					if (!is_null($data['Kotei'])) {
						$keycheck =  $data['T_Name'].'_'. $data['T_BKumiku'].'_'.$data['Name'].'_'. $data['BKumiku']
													.'_'.$data['Kotei'].'_'. $data['KKumiku'];
						$listWorkItemID[$keycheck] = $resulTimeTK;
					}elseif (!is_null($data['Name'])) {
						$keycheck =  $data['T_Name'].'_'. $data['T_BKumiku'].'_'.$data['Name'].'_'. $data['BKumiku'];
						$listWorkItemID[$keycheck] = $resulTimeTK;
					}else {
						$keycheck =  $data['T_Name'].'_'. $data['T_BKumiku'];
						$listWorkItemID[$keycheck] = $resulTimeTK;
					}
				}else {
					return redirect($this->stopProcessThenGoTo030501($menuInfo, $request,
																		config('message.msg_schem_excelimport_001')));
				}
				//③	insert data of tosai
				if (is_null($data['Name']) && is_null($data['Kotei'])) {
					$checkWorkItemID =  $data['T_Name'].'_'. $data['T_BKumiku'];
					$newWorkItemID = isset($listWorkItemID[$checkWorkItemID]) ? $listWorkItemID[$checkWorkItemID] : null;
					if ($TableType == 0 ) {
						$objCynTosaiData = new Cyn_TosaiData;
						$objCynTosaiData->ProjectID = $projectID;
						$objCynTosaiData->OrderNo = $orderNo;
						$objCynTosaiData->CKind = $cKind;
						$objCynTosaiData->WorkItemID = $newWorkItemID;
						$objCynTosaiData->Name = $data['T_Name'];
						$objCynTosaiData->BKumiku = $data['T_BKumiku'];
						$objCynTosaiData->IsOriginal = 1;
						$objCynTosaiData->save();
					}
					else if ($TableType == 1 ) {
						$objCynTosaiData = new Cyn_TosaiData;
						$objCynTosaiData->ProjectID = $projectID;
						$objCynTosaiData->OrderNo = $orderNo;
						$objCynTosaiData->CKind = $cKind;
						$objCynTosaiData->WorkItemID = 0;
						$objCynTosaiData->Name = $data['T_Name'];
						$objCynTosaiData->BKumiku = $data['T_BKumiku'];
						$objCynTosaiData->IsOriginal = 1;
						$objCynTosaiData->save();

						$objCynBlockKukaku = new Cyn_BlockKukaku;
						$objCynBlockKukaku->ProjectID = $projectID;
						$objCynBlockKukaku->OrderNo = $orderNo;
						$objCynBlockKukaku->WorkItemID = $newWorkItemID;
						$objCynBlockKukaku->CKind = $cKind;
						$objCynBlockKukaku->T_Name = is_null($data['T_Name']) ? null : $data['T_Name'];
						$objCynBlockKukaku->T_BKumiku = is_null($data['T_BKumiku']) ? null : $data['T_BKumiku'];
						$objCynBlockKukaku->No = $maxNo;
						$objCynBlockKukaku->Name = is_null($data['Name']) ? $data['T_Name'] : $data['Name'];
						$objCynBlockKukaku->BKumiku = is_null($data['BKumiku']) ? $data['T_BKumiku'] : $data['BKumiku'];

						$objCynBlockKukaku->save();
						$maxNo++;
					}
				}
				//④	insert data of 中日程
				if (!is_null($data['Name']) && is_null($data['Kotei'])) {
					$checkListWorkItemID =  $data['T_Name'].'_'. $data['T_BKumiku'].'_'.
											$data['Name'].'_'. $data['BKumiku'];
					$newWorkItemID = isset($listWorkItemID[$checkListWorkItemID])
											? $listWorkItemID[$checkListWorkItemID] : null;
					$checkListNo = $data['T_Name'].'_'. $data['T_BKumiku'];
					$P_No = isset($listNo[$checkListNo]) ? $listNo[$checkListNo] : 0;
					if ($TableType == 0 ) {
						$objCynBlockKukaku = new Cyn_BlockKukaku;
						$objCynBlockKukaku->ProjectID = $projectID;
						$objCynBlockKukaku->OrderNo = $orderNo;
						$objCynBlockKukaku->WorkItemID = $newWorkItemID;
						$objCynBlockKukaku->CKind = $cKind;
						$objCynBlockKukaku->T_Name = is_null($data['T_Name']) ? null : $data['T_Name'];
						$objCynBlockKukaku->T_BKumiku = is_null($data['T_BKumiku']) ? null : $data['T_BKumiku'];
						$objCynBlockKukaku->No = $maxNo;
						$objCynBlockKukaku->Name = $data['Name'];
						$objCynBlockKukaku->BKumiku = is_null($data['BKumiku']) ? null : $data['BKumiku'];
						$objCynBlockKukaku->save();
					}
					else if ($TableType == 1 ) {
						$objCynCBlockKukaku = new Cyn_C_BlockKukaku;
						$objCynCBlockKukaku->ProjectID = $projectID;
						$objCynCBlockKukaku->OrderNo = $orderNo;
						$objCynCBlockKukaku->WorkItemID = $newWorkItemID;
						$objCynCBlockKukaku->P_ProjectID = $projectID;
						$objCynCBlockKukaku->P_OrderNo = $orderNo;
						$objCynCBlockKukaku->P_CKind = $cKind;
						$objCynCBlockKukaku->CKind = $cKind;
						$objCynCBlockKukaku->P_No = $P_No;
						$objCynCBlockKukaku->T_Name = is_null($data['T_Name']) ? null : $data['T_Name'];
						$objCynCBlockKukaku->T_BKumiku = is_null($data['T_BKumiku']) ? null : $data['T_BKumiku'];
						$objCynCBlockKukaku->No = $maxNo;
						$objCynCBlockKukaku->Name = is_null($data['Name']) ? null : $data['Name'];
						$objCynCBlockKukaku->BKumiku = is_null($data['BKumiku']) ? null : $data['BKumiku'];
						$objCynCBlockKukaku->save();

						$listNo[$checkListWorkItemID] = $maxNo;
					}
					$maxNo++;
				}
				//⑤	insert data kotei
				if (!is_null($data['Kotei']))
				{
					$checkListWorkItemID =  $data['T_Name'].'_'. $data['T_BKumiku'].'_'.
											$data['Name'].'_'. $data['BKumiku'].'_'.
											$data['Kotei'].'_'. $data['KKumiku'];
					$newWorkItemID = isset($listWorkItemID[$checkListWorkItemID])
										 ? $listWorkItemID[$checkListWorkItemID] : null;
					$checkListNo = $data['T_Name'].'_'. $data['T_BKumiku'].'_'.
									$data['Name'].'_'. $data['BKumiku'];
					$P_No = isset($listNo[$checkListNo]) ? $listNo[$checkListNo] : 0;

					$checkListMaxNo = $data['T_Name'].'_'. $data['T_BKumiku'].'_'.
									$data['Name'].'_'. $data['BKumiku'];
					$Max_No = (isset($listMaxNo[$checkListMaxNo]) ? $listMaxNo[$checkListMaxNo] : 0) + 1;
					if ($TableType == 0 ) {
						$objCynPlan = new Cyn_Plan;
						$objCynPlan->ProjectID = $projectID;
						$objCynPlan->OrderNo = $orderNo;
						$objCynPlan->WorkItemID = $newWorkItemID;
						$objCynPlan->No = $P_No;
						$objCynPlan->KoteiNo = $Max_No;
						$objCynPlan->Kotei = $data['Kotei'];
						$objCynPlan->KKumiku = $data['KKumiku'];
						$objCynPlan->Floor = is_null($data['Floor']) ? null : $data['Floor'];
						$objCynPlan->BD_Code = is_null($data['BD_Code']) ? null : $data['BD_Code'];
						$objCynPlan->Days = $data['Days'];
						$objCynPlan->N_Link = $data['LinkDays'];
						$objCynPlan->save();

						$listMaxNo[$checkListMaxNo] = $Max_No;
					}else if ($TableType == 1 ) {
						$objCynCPlan = new Cyn_C_Plan;
						$objCynCPlan->ProjectID = $projectID;
						$objCynCPlan->OrderNo = $orderNo;
						$objCynCPlan->WorkItemID = $newWorkItemID;
						$objCynCPlan->No = $P_No;
						$objCynCPlan->KoteiNo = $Max_No;
						$objCynCPlan->Kotei = $data['Kotei'];
						$objCynCPlan->KKumiku = $data['KKumiku'];
						$objCynCPlan->Floor = is_null($data['Floor']) ? null : $data['Floor'];
						$objCynCPlan->BD_Code = is_null($data['BD_Code']) ? null : $data['BD_Code'];
						$objCynCPlan->Days = $data['Days'];
						$objCynCPlan->N_Link = $data['LinkDays'];
						$objCynCPlan->save();

						$listMaxNo[$checkListMaxNo] = $Max_No;

					}
					$Max_No++;
				}
			}
		});
		$dataHasAMDFlag1 = array_filter($temLogExcelImport->toArray(), function($item) {
			return $item['AMDFlag'] == 1 && !is_null($item['AMDFlag'])  ;
		});
		foreach($dataHasAMDFlag1 as $data) {
			$insertPlanTimetracker = $timeTrackerFuncSchem->insertPlan($projectID, $orderNo, null, $data['WorkItemID'],
															$data['SDate'], $data['EDate'], null, $projectCalendar);
			// 2
			if ($TableType == 0) {
				$updateCynPlan = Cyn_Plan::where('WorkItemID' , '=' , $data['WorkItemID'])
										->update(['Floor' => $data['Floor'], 'BD_Code' => $data['BD_Code'],
													'Days' => $data['Days'], 'N_Link' => $data['LinkDays']]);
			}
			// 3
			if ($TableType == 1) {
				$updateCynCPlan = Cyn_C_Plan::where('WorkItemID' , '=' , $data['WorkItemID'])
											->update(['Floor' => $data['Floor'], 'BD_Code' => $data['BD_Code'],
														'Days' => $data['Days'], 'N_Link' => $data['LinkDays']]);
			}
		}

		$dataHasAMDFlag2 = array_filter($temLogExcelImport->toArray(), function($item) {
			return $item['AMDFlag'] == 2 && !is_null($item['AMDFlag'])  ;
		});
		foreach($dataHasAMDFlag2 as $data) {
			$dateNow = DB::selectOne('SELECT CONVERT(DATE, getdate()) AS sysdate')->sysdate;
			$dateNow = str_replace('-', '/', $dateNow);
			$data['Del_Date'] = $dateNow;
			$deleteTimetracker = $timeTrackerCommon->deleteItem($data['WorkItemID']);
			//2
			if (is_null($data['Name']) && is_null($data['Kotei'])) {
				$deleteCynTosai = Cyn_TosaiData::where('WorkItemID' , '=' , $data['WorkItemID'])->delete();
			}
			//3
			if (!is_null($data['Name']) && is_null($data['Kotei'])) {
				$updateCynTosai = Cyn_BlockKukaku::where('WorkItemID' , '=' , $data['WorkItemID'])
												->update(['Del_Date' => $dateNow]);
			}
			//4
			if (!is_null($data['Kotei']) && $TableType == 0) {
				$updateCynPlan = Cyn_Plan::where('WorkItemID' , '=' , $data['WorkItemID'])
										->update(['Del_Date' => $dateNow]);
			}
			//5
			if (!is_null($data['Name']) && is_null($data['Kotei']) && $TableType == 1) {
				$updateCynCBlock = Cyn_C_BlockKukaku::where('WorkItemID' , '=' , $data['WorkItemID'])
													->update(['Del_Date' => $dateNow]);
			}
			//6
			if (!is_null($data['Kotei']) && $TableType == 1) {
				$updateCynCPlan = Cyn_C_Plan::where('WorkItemID' , '=' , $data['WorkItemID'])
											->update(['Del_Date' => $dateNow]);
			}
		}
		$updateData = Cyn_Excel_History::where('ID','=',$lastID + 1)->update(['StatusFlag' => 1]);
		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/index';
		$url .= '?cmn1=' . $request->cmn1;
		$url .= '&cmn2=' . $request->cmn2;
		$url .= '&val1=' . $request->val1;
		$url .= '&val2=' . $request->val2;
		$url .= '&val3=' . $request->val3;
		$url .= '&val4=' . $request->val4;
		$url .= '&val6=' . $request->val6;
		$url .= '&val8=' . $request->val8;
		//「030501_日程取込条件設定画面」に遷移する。
		return redirect($url);
	}

	/**
	 * POST data
	 *
	 * @param Request
	 * @return View
	 *
	 * @create 2020/12/28 Dung
	 */
	public function cancel(Request $request)
	{
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));
		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		// deleteLock
		$deleteLock = $this->deleteLock($menuInfo->KindID, config('system_const_schem.sys_menu_id_plan'),
										$menuInfo->SessionID, valueUrlDecode($request->val1));
		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/index';
		$url .= '?cmn1=' . $request->cmn1;
		$url .= '&cmn2=' . $request->cmn2;
		$url .= '&val1=' . $request->val1;
		$url .= '&val2=' . $request->val2;
		$url .= '&val3=' . $request->val3;
		$url .= '&val4=' . $request->val4;
		$url .= '&val6=' . $request->val6;
		$url .= '&val8=' . $request->val8;
		//「030501_日程取込条件設定画面」に遷移する。
		return redirect($url);
	}

}