<?php
/*
 * @ImportController.php
 * @Import Controller file
 *
 * @create 2020/09/14 Dung
 *
 * @update 2020/10/12 Dung 
 * @update 2020/10/19 Dung changed according to Rev6 screen 030402 , fix bug
 * @update 2020/10/20 Dung changed according to Rev7 screen 030402 , fix bug
 * @update 2020/10/21 Dung changed according to Rev8 screen 030402
 * @update 2020/10/23 Dung fix bug
 * @update 2020/10/27 Dung fix bug
 * @update 2020/10/28 Dung fix bug
 * @update 2020/10/29 Dung fix bug
 * @update 2020/10/30 Dung fix bug
 * @update 2020/11/04 Dung fix bug
 */
namespace App\Http\Controllers\Schem;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use Illuminate\Database\QueryException;
use App\Http\Requests\Schem\ImportContentsRequest;
use App\Librarys\FuncCommon;
use App\Librarys\TimeTrackerFuncSchem;
use App\Librarys\TimeTrackerCommon;
use App\Librarys\MenuInfo;
use App\Models\MstProject;
use App\Models\MstOrderNo;
use App\Models\T_Tosai;
use App\Models\T_Kyokyu;
use App\Models\T_Sogumi;
use App\Models\Cyn_TosaiData;
use App\Models\Cyn_History_Tosai;
use App\Models\Cyn_LogData_Tosai;
use App\Models\Cyn_Temp_LogData_Tosai;
use App\Models\SystemLock;
use Carbon\Carbon;
use DateTime;
/*
 * ImportController class
 *
 * @create 2020/09/14 Dung
 * @update 2020/10/12 Dung 
 * @update 2020/10/23 Dung  fix bug
 */
class ImportController extends Controller
{
	const EXCEPT_CHAR_INDEX = 13; // get the 13th character
	const GEN_P = 'P'; // 13th character is P
	const GEN_S = 'S'; // 13th character is S
	const GEN_C = 'C'; // 13th character is C

	/**
	 * construct
	 *
	 * @param Request
	 * @return View
	 * @create 2020/09/14　Dung
	 * @update 2020/10/12 Dung 
	 */
    public function index(Request $request)
	{
		return $this->initialize($request);
	}
 	/**
	 * ImportData list screen initial display processing
	 *
	 * @param Request
	 * @return View
	 * @create 2020/09/14　Dung
	 * @update 2020/10/12 Dung 
	 */
	private function initialize(Request $request) {
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));
		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
        //select data
		$projects = MstProject::select('mstProject.ID','mstProject.ProjectName','mstProject.ListKind')
								->where('mstProject.SysKindID','=',$menuInfo->KindID)
								->orderBy('ProjectName','asc')
								->get();
		if($projects != null) {
			foreach($projects as & $project) {
				$project->ID = valueUrlEncode($project->ID);
			}
		}
		$orders = MstOrderNo::select('mstOrderNo.OrderNo')
								->where('DispFlag','=',0)
								->orderBy('OrderNo','asc')
								->get();
		//error
		$originalError = array();
        if (isset($request->err1)) {
            $originalError[] = valueUrlDecode($request->err1);
		}
        $this->data['menuInfo'] = $menuInfo;
        $this->data['request'] = $request;
        $this->data['projects'] = $projects;
		$this->data['orders'] = $orders;
		$this->data['originalError'] = $originalError;
        //return view with all data
		return view('Schem/Import/index', $this->data);
	}
	
	
	/**
	 * POST data
	 *
	 * @param ImportContentsRequest
	 * @return View
	 *
	 * @create 2020/09/14　Dung
	 * @update 2020/10/12 Dung changed according to Rev16
	 * @update 2020/10/15 Dung changed according to Rev17
	 * @update 2020/10/20 Dung - Fix bug No35
	 * @update 2020/10/27 Dung - Fix bug
	 */
	public function import(ImportContentsRequest $request) {

		$startImportTime = microtime(true); 

		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));
		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		$projectID = $request->val3;
		$orderNo = $request->val1;
		// $olds  = Cyn_TosaiData::where('OrderNo','=',$orderNo)
		//  							->where('ProjectID','=',$projectID)
		//  							->where('IsOriginal','=',0)->get();
		// $historyID = Cyn_History_Tosai::selectRaw('MAX(ID) as historyID')
		//  							->where('OrderNo','=',$orderNo)
		//  							->where('ProjectID','=',0)->first();
		// if($historyID != null) {
		//  	$oldLogs = Cyn_LogData_Tosai::where('HistoryID','=',$historyID->historyID)->get();
		// }
		// $tosais = T_Tosai::where('OrderNo','=',$orderNo)->where('ProjectID','=', 0)->get();
		// $kyokyus = T_Kyokyu::where('OrderNo','=',$orderNo)->where('ProjectID','=', 0)->get();
		// $sogumis = T_Sogumi::where('OrderNo','=',$orderNo)->where('ProjectID','=', 0)->get();
		

		$olds  = Cyn_TosaiData::select('Name','BKumiku','T_Date','PlSDate', 'SG_Date'
									, DB::raw('Substring(Name, 0, len(Name)) as NameNotGen')
									, DB::raw('Substring(Name, len(Name), 1) as Gen'))
								->where('OrderNo','=',$orderNo)
								->where('ProjectID','=',$projectID)
								->where('IsOriginal','=',0)->get()->toArray();

		$historyID = Cyn_History_Tosai::selectRaw('MAX(ID) as historyID')
									->where('OrderNo','=',$orderNo)
									->where('ProjectID','=',0)->first();		
		if($historyID != null) {
			$oldLogs = Cyn_LogData_Tosai::where('HistoryID','=',$historyID->historyID)->get()->toArray();			
		}
		//$tosais = T_Tosai::where('OrderNo','=',$orderNo)->where('ProjectID','=', 0)->get();
		$tosais = T_Tosai::select('BlockName','BlockKumiku','WorkItemID'
									, DB::raw('Substring(BlockName, 0, len(BlockName)) as BLockNameNotGen')
									, DB::raw('Substring(BlockName, len(BlockName), 1) as Gen'))	
								->where('OrderNo','=',$orderNo)
								->where('ProjectID','=', 0)								
								->get()->toArray();
		//$kyokyus = T_Kyokyu::where('OrderNo','=',$orderNo)->where('ProjectID','=', 0)->get();
		$kyokyus = T_Kyokyu::select('BlockName','BlockKumiku','K_BlockName','K_BlockKumiku','WorkItemID'
									, DB::raw('Substring(BlockName, 0, len(BlockName)) as BLockNameNotGen')
									, DB::raw('Substring(BlockName, len(BlockName), 1) as Gen'))	
									->where('OrderNo','=',$orderNo)
									->where('ProjectID','=', 0)								
									->get()->toArray();
		//$sogumis = T_Sogumi::where('OrderNo','=',$orderNo)->where('ProjectID','=', 0)->get();
		$sogumis = T_Sogumi::select('BlockName','BlockKumiku','WorkItemID'
								, DB::raw('Substring(BlockName, 0, len(BlockName)) as BLockNameNotGen')
								, DB::raw('Substring(BlockName, len(BlockName), 1) as Gen'))	
								->where('OrderNo','=',$orderNo)
								->where('ProjectID','=', 0)								
								->get()->toArray();
		
		//Begin: Tinttp Added
		$caculaSearchData = microtime(true) - $startImportTime;
		if (isset($_COOKIE['1_SearchDataProcess_Time']) && $_COOKIE['1_SearchDataProcess_Time']) {
			unset($_COOKIE['1_SearchDataProcess_Time']);
			setcookie("1_SearchDataProcess_Time", $caculaSearchData, -1, '/');
		}else{
			setcookie("1_SearchDataProcess_Time", $caculaSearchData, -1, '/');
		}
		//End: Tinttp Added

		$pushDataIntoArrayWorkItemIDsSTime = microtime(true);	//Tinttp Added
		$workItemIDs = array();
		// add WorkItemID from $toais to $workItemIDs
		if(!is_null($tosais)){
			foreach ($tosais as $tosai) {
				array_push($workItemIDs, $tosai['WorkItemID']);
			
			}
		}
		// add WorkItemID from $kyokyus to $workItemIDs
		if(!is_null($kyokyus)){
			foreach ($kyokyus as $kyokyu) {
				array_push($workItemIDs, $kyokyu['WorkItemID']);
			}
		}
		// add WorkItemID from $sogumis to $workItemIDs
		if(!is_null($sogumis)){
			foreach ($sogumis as $sogumi) {
				array_push($workItemIDs, $sogumi['WorkItemID']);
			}
		}
		//Begin: Tinttp Added
		$pushDataIntoArrayWorkItemIDsTime = microtime(true) - $pushDataIntoArrayWorkItemIDsSTime;
		if (isset($_COOKIE['9_pushDataIntoArrayWorkItemIDs_Time']) && $_COOKIE['9_pushDataIntoArrayWorkItemIDs_Time']) {
			unset($_COOKIE['9_pushDataIntoArrayWorkItemIDs_Time']);
			setcookie("9_pushDataIntoArrayWorkItemIDs_Time", $pushDataIntoArrayWorkItemIDsTime, -1, '/');
		}else{
			setcookie("9_pushDataIntoArrayWorkItemIDs_Time", $pushDataIntoArrayWorkItemIDsTime, -1, '/');
		}
		//End: Tinttp Added

		$callGetKoteiRangeSTime = microtime(true);	//Tinttp Added
		//Timetracker
		$timeTrackerCommon = new TimeTrackerCommon();
		$ranges = $timeTrackerCommon->getKoteiRange($workItemIDs);
		//TryLock
		if(!is_array($ranges)) {
			$originalError = $ranges ;
		}else{
			$originalError  = $this->tryLock($menuInfo->KindID, config('system_const_schem.sys_menu_id_plan'), 
											 $menuInfo->UserID, $menuInfo->SessionID, $request->val2, true);
		}
		if (!is_null($originalError )) {
			$originalError = $originalError ;
			$urlErr = url('/');
			$urlErr .= '/' . $menuInfo->KindURL;
			$urlErr .= '/' . $menuInfo->MenuURL;
			$urlErr .= '/index';
			$urlErr .= '?cmn1=' . $request->cmn1;
			$urlErr .= '&cmn2=' . $request->cmn2;
			$urlErr .= '&val1=' . valueUrlEncode($request->val1);
			$urlErr .= '&val2=' . valueUrlEncode($request->val2);
			$urlErr .= '&val3=' . valueUrlEncode($request->val3);
			$urlErr .= '&val4=' . valueUrlEncode($request->val4);
			$urlErr .= '&err1=' . valueUrlEncode($originalError);
			return redirect($urlErr);
		}
		//Begin: Tinttp Added
		$callGetKoteiRangeTime = microtime(true) - $callGetKoteiRangeSTime;
		if (isset($_COOKIE['8_callGetKoteiRangeTime_Time']) && $_COOKIE['8_callGetKoteiRangeTime_Time']) {
			unset($_COOKIE['8_callGetKoteiRangeTime_Time']);
			setcookie("8_callGetKoteiRangeTime_Time", $callGetKoteiRangeTime, -1, '/');
		}else{
			setcookie("8_callGetKoteiRangeTime_Time", $callGetKoteiRangeTime, -1, '/');
		}
		//End: Tinttp Added
		$ImportData = array();
		$EscapeOnDeleteDatas = array();

		$pullTosaiToImportDataSTime = microtime(true);	//Tinttp Added
		// push tosai data into ImportData
		$this->makeImportData($ImportData, $EscapeOnDeleteDatas, $tosais, 1, $ranges, $olds, $oldLogs);
		//Begin: Tinttp Added
		$pullTosaiToImportDataTime = microtime(true) - $pullTosaiToImportDataSTime;
		if (isset($_COOKIE['2_pullTosaiToImportData_Time']) && $_COOKIE['2_pullTosaiToImportData_Time']) {
			unset($_COOKIE['2_pullTosaiToImportData_Time']);
			setcookie("2_pullTosaiToImportData_Time", $pullTosaiToImportDataTime, -1, '/');
		}else{
			setcookie("2_pullTosaiToImportData_Time", $pullTosaiToImportDataTime, -1, '/');
		}
		//End: Tinttp Added

		$pullKyoKyuToImportDataSTime = microtime(true);	//Tinttp Added

		// push kyokyu data into ImportData
		$this->makeImportData($ImportData, $EscapeOnDeleteDatas, $kyokyus, 2, $ranges, $olds, $oldLogs);
		//Begin: Tinttp Added
		$pullKyoKyuToImportDataTime = microtime(true) - $pullKyoKyuToImportDataSTime;
		if (isset($_COOKIE['3_pullKyoKyuToImportData_Time']) && $_COOKIE['3_pullKyoKyuToImportData_Time']) {
			unset($_COOKIE['3_pullKyoKyuToImportData_Time']);
			setcookie("3_pullKyoKyuToImportData_Time", $pullKyoKyuToImportDataTime, -1, '/');
		}else{
			setcookie("3_pullKyoKyuToImportData_Time", $pullKyoKyuToImportDataTime, -1, '/');
		}
		//End: Tinttp Added

		$pullSogumiToImportDataSTime = microtime(true);	//Tinttp Added

		// push sogumi data into ImportData
		$this->makeImportData($ImportData, $EscapeOnDeleteDatas, $sogumis, 3, $ranges, $olds, $oldLogs);
		
		//Begin: Tinttp Added
		$pullSogumiToImportDataTime = microtime(true) - $pullSogumiToImportDataSTime;
		if (isset($_COOKIE['4_pullSogumiToImportData_Time']) && $_COOKIE['4_pullSogumiToImportData_Time']) {
			unset($_COOKIE['4_pullSogumiToImportData_Time']);
			setcookie("4_pullSogumiToImportData_Time", $pullSogumiToImportDataTime, -1, '/');
		}else{
			setcookie("4_pullSogumiToImportData_Time", $pullSogumiToImportDataTime, -1, '/');
		}
		//End: Tinttp Added

		//Loop $olds
		$pullDeleteDataToImportDataSTime = microtime(true);	//Tinttp Added
		$ImportDataBeforeDelete = $ImportData;
		$addMore = array();		

		// $oldsでループ		
		foreach($olds as $old){			
			// [BlockName]と[BKumiku]が一致するレコード数を取得			
			$matchInImportData = array_filter($ImportDataBeforeDelete, function($item) use ($old){
				return $item['BlockName'] == $old['Name'] && $item['BKumiku'] == $old['BKumiku'];
			});

			// [BlockName]と[BKumiku]が一致するレコード数を取得			
			$matchInEscapeData = array_filter($EscapeOnDeleteDatas, function($item) use ($old){
				return $item['BlockName'] == $old['Name'] && $item['BKumiku'] == $old['BKumiku'];
			});			
			
			$isChangedToGenC = false;
			$matchPInOldsNotContainGen = array_values(array_filter($olds, function($item) use ($old){
				return ($item['Gen'] == self::GEN_P 
						&& $item['NameNotGen'] == $old['NameNotGen'] 
						&& $item['BKumiku'] == $old['BKumiku']);
			}));

			$matchSInOldsNotContainGen = array_values(array_filter($olds, function($item) use ($old){
				return ($item['Gen'] == self::GEN_S 
						&& $item['NameNotGen'] == $old['NameNotGen'] 
						&& $item['BKumiku'] == $old['BKumiku']);
			}));

			$matchCInImportData = array_filter($ImportDataBeforeDelete, function($item) use ($old){
				return ($item['Gen'] == self::GEN_C 
						&& $item['BlockNameNotGen'] == $old['NameNotGen'] 
						&& $item['BKumiku'] == $old['BKumiku']);
			});

			if (count($matchPInOldsNotContainGen) > 0
				&& count($matchSInOldsNotContainGen) > 0
				&& count($matchCInImportData) > 0)
			{
				$isChangedToGenC = true;
			}

			// [BlockName]と[BKumiku]が一致するレコードが無い場合
			if (count($matchInImportData) == 0 && count($matchInEscapeData) == 0){	
				$deleteData['BlockName'] = $old['Name'];
				$deleteData['BlockNameNotGen'] 	= $old['NameNotGen'];
				$deleteData['BKumiku'] 	= $old['BKumiku'];
				$deleteData['Gen'] = $old['Gen'];				
				$deleteData['AMDFlag'] = 2;
				$deleteData['Log'] = $isChangedToGenC ? config('message.msg_schem_imp_001') . "\r\n" . config('message.msg_schem_imp_005')
														: config('message.msg_schem_imp_005');

				$this->addDataIntoImportData($ImportData, $deleteData, false);
			}

			/*foreach ($ImportData as $data) {
				$dataBKumiku = $data['BKumiku'];
				$dataBlockName =  $data['BlockName'];
				$genOfOld = $this->getTextCharIndex($old['Name'], self::EXCEPT_CHAR_INDEX);
				$tempDataOlds = $this->calculateOldsData($olds, $data['BlockName'], $data['BKumiku']);
				$isEqualFull = $tempDataOlds['isEqualFull'];
				$isEqualFullBlockName = $tempDataOlds['isEqualFullBlockName'];
				$numberEqualFullBKumiku = $tempDataOlds['numberEqualFullBKumiku'];
				$numberGenHasCharP = $tempDataOlds['numberGenHasCharP'];
				$numberGenHasCharS = $tempDataOlds['numberGenHasCharS'];
				$numberGenHasCharC = $tempDataOlds['numberGenHasCharC'];
				$ImportDataMore = array('BlockName' => '','BKumiku' => '','TDate' => null,'Gen' => '','AMDFlag' => '','Log' => '',);

				if($old->Name != $dataBlockName && $old->BKumiku != $dataBKumiku){
					$ImportDataMore = array(
						'BlockName' => $old->Name,
						'BKumiku' => $old->BKumiku,
						'TDate' => null,
						'Gen' => $genOfOld,
						'AMDFlag' => 2,
						'Log' => config('message.msg_schem_imp_005'),
					);
				}else{
					$ImportDataMore = array();
				}
			}
			if(isset($ImportDataMore) && !empty($ImportDataMore)){
				$ImportData[] = $ImportDataMore;
			}
			if (isset($addMore) && !empty($addMore)) {
				$ImportData[] = $addMore;
			}*/
		}		
		//Begin: Tinttp Added
		$pullDeleteDataToImportDataTime = microtime(true) - $pullDeleteDataToImportDataSTime;
		if (isset($_COOKIE['5_pullDeleteDataToImportData_Time']) && $_COOKIE['5_pullDeleteDataToImportData_Time']) {
			unset($_COOKIE['5_pullDeleteDataToImportData_Time']);
			setcookie("5_pullDeleteDataToImportData_Time", $pullDeleteDataToImportDataTime, -1, '/');
		}else{
			setcookie("5_pullDeleteDataToImportData_Time", $pullDeleteDataToImportDataTime, -1, '/');
		}
		//End: Tinttp Added	
		
		$idCynTemLogDataTosai = Cyn_Temp_LogData_Tosai::all()->sortByDesc('ID')->first();
		$lastID  = is_null($idCynTemLogDataTosai) ? 0 : $idCynTemLogDataTosai->ID;
		// add data from $ImportData to table Cyn_Temp_LogData_Tosai
		DB::transaction(function () use($request, $ImportData, $lastID) {

			$deleteCyn_Temp_LogData_TosaiSTime = microtime(true);	//Tinttp Added

			$result = Cyn_Temp_LogData_Tosai::where('OrderNo', '=', $request->val1 )
			->where('CKind', '=', $request->val2)
			->delete();			
			//Begin: Tinttp Added
			$deleteCyn_Temp_LogData_TosaiTime = microtime(true) - $deleteCyn_Temp_LogData_TosaiSTime;
			if (isset($_COOKIE['6_deleteCyn_Temp_LogData_Tosai_Time']) && $_COOKIE['6_deleteCyn_Temp_LogData_Tosai_Time']) {
				unset($_COOKIE['6_deleteCyn_Temp_LogData_Tosai_Time']);
				setcookie("6_deleteCyn_Temp_LogData_Tosai_Time", $deleteCyn_Temp_LogData_TosaiTime, -1, '/');
			}else{
				setcookie("6_deleteCyn_Temp_LogData_Tosai_Time", $deleteCyn_Temp_LogData_TosaiTime, -1, '/');
			}
			//End: Tinttp Added	

			$insertCyn_Temp_LogData_TosaiSTime = microtime(true);	//Tinttp Added
			foreach ($ImportData as $item) {
				$objLogDataTosai = new Cyn_Temp_LogData_Tosai;
				$objLogDataTosai->OrderNo = $request->val1;
				$objLogDataTosai->CKind = valueUrlDecode($request->val2);
				$objLogDataTosai->ID =	$lastID + 1;
				$objLogDataTosai->BlockName = $item['BlockName'];
				$objLogDataTosai->BKumiku = $item['BKumiku'];
				$objLogDataTosai->Gen = $item['Gen'];
				$objLogDataTosai->Log = $item['Log'];
				$objLogDataTosai->AMDFlag = $item['AMDFlag'];
				$objLogDataTosai->TDate = !array_key_exists('TDate',$item) ? null : $item['TDate'];
				$objLogDataTosai->KDate = !array_key_exists('KDate',$item)  ? null : $item['KDate'];
				$objLogDataTosai->SG_SDate = !array_key_exists('SG_SDate',$item) ? null : $item['SG_SDate'];
				$objLogDataTosai->SG_EDate = !array_key_exists('SG_EDate',$item) ? null : $item['SG_EDate'];
				$objLogDataTosai->NxtName =  !array_key_exists('NxtName',$item) ? null : $item['NxtName'];
				$objLogDataTosai->NxtBKumiku = !array_key_exists('NxtBKumiku',$item) ? null : $item['NxtBKumiku'];
				$objLogDataTosai->save();
				$lastID++;
			}						
			//Begin: Tinttp Added
			$insertCyn_Temp_LogData_TosaiTime = microtime(true) - $insertCyn_Temp_LogData_TosaiSTime;
			if (isset($_COOKIE['7_insertCyn_Temp_LogData_Tosai_Time']) && $_COOKIE['7_insertCyn_Temp_LogData_Tosai_Time']) {
				unset($_COOKIE['7_insertCyn_Temp_LogData_Tosai_Time']);
				setcookie("7_insertCyn_Temp_LogData_Tosai_Time", $insertCyn_Temp_LogData_TosaiTime, -1, '/');
			}else{
				setcookie("7_insertCyn_Temp_LogData_Tosai_Time", $insertCyn_Temp_LogData_TosaiTime, -1, '/');
			}
			//End: Tinttp Added	
		});

					
		//Begin: Tinttp Added
		$totalProcessTime = microtime(true) - $startImportTime;
		if (isset($_COOKIE['10_TotalProcess_Time']) && $_COOKIE['10_TotalProcess_Time']) {
			unset($_COOKIE['10_TotalProcess_Time']);
			setcookie("10_TotalProcess_Time", $totalProcessTime, -1, '/');
		}else{
			setcookie("10_TotalProcess_Time", $totalProcessTime, -1, '/');
		}
		//End: Tinttp Added	
		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/create';
		$url .= '?cmn1=' . $request->cmn1;
		$url .= '&cmn2=' . $request->cmn2;
		$url .= '&val1=' . valueUrlEncode($request->val1);
		$url .= '&val2=' . valueUrlEncode($request->val2);
		$url .= '&val3=' . valueUrlEncode($request->val3);
		$url .= '&val4=' . valueUrlEncode($request->val4);
		return redirect($url);
	}

	public function makeImportData(&$ImportData, &$EscapeOnDeleteDatas, $inputArrData, $dataGroup, $ranges, $olds, $oldLogs) {
		foreach ($inputArrData as $input) {
			$data = array();
			$rangeStartDate = new Datetime($ranges[$input['WorkItemID']]['plannedStartDate']);
			$rangeFinishDate = new Datetime($ranges[$input['WorkItemID']]['plannedFinishDate']);
			$rangeStartDate = $rangeStartDate->format('Y-m-d');
			$rangeFinishDate = $rangeFinishDate->format('Y-m-d');			
			$isChangeDate = false;

			$data['BlockName'] 	= $input['BlockName'];
			$data['BKumiku'] 	= $input['BlockKumiku'];
			$data['BlockNameNotGen'] =	 $input['BLockNameNotGen'];
			$data['Gen'] 		= $input['Gen'];
			// Tosai's data
			if($dataGroup == 1)
			{				
				$data['TDate'] = $rangeStartDate;
			}
			// Kyokyu's data
			if($dataGroup == 2)
			{
				$data['NxtName'] 	= $input['K_BlockName'];
				$data['NxtBKumiku'] = $input['K_BlockKumiku'];
				$data['KDate'] = $rangeStartDate;
			}
			// Sogumi's data
			if($dataGroup == 3)
			{				
				$data['SG_SDate'] = $rangeStartDate;
				$data['SG_EDate'] = $rangeFinishDate;
				
				// ※注2 $olds には[SG_EDate]が無い為、
				// $oldLogs から　[BlockName]と[BKumiku]が一致するレコードを探し、そのレコードの[SG_EDate]を終了日として扱う
				$matchInOldlogs = array_values(array_filter($oldLogs, function($item) use ($input){
					return ($item['BlockName'] == $input['BlockName'] && $item['BKumiku'] == $input['BlockKumiku']);
				}));			
				if(count($matchInOldlogs) > 0){
					$SG_EDate = new Datetime($matchInOldlogs[0]['SG_EDate']) ;				
					$SG_EDate =  $SG_EDate->format('Y-m-d');
	
				}				
			}			

			$matchInOlds = array_values(array_filter($olds, function($item) use ($input){
				return ($item['Name'] == $input['BlockName'] && $item['BKumiku'] == $input['BlockKumiku']);
			}));

			if (count($matchInOlds) > 0){
				// $oldsに、[BlockName]と[BKumiku]が一致するレコードが有る
				$isChangeDate = ($dataGroup == 1 && $matchInOlds[0]['T_Date'] != $rangeStartDate) 
								|| ($dataGroup == 2 && $matchInOlds[0]['SG_Date'] != $rangeStartDate) 
								|| ($dataGroup == 3 && ($matchInOlds[0]['PlSDate'] != $rangeStartDate || (isset($SG_EDate) && $SG_EDate == $rangeFinishDate)));

				if ($isChangeDate)
				{
					$data['AMDFlag'] = 1;
					$data['Log'] = config('message.msg_schem_imp_003');
					$this->addDataIntoImportData($ImportData, $data, $dataGroup == 3);
				}
				else
				{					
					$escapeData = array();
					$escapeData['BlockName'] 	= $input['BlockName'];
					$escapeData['BKumiku'] 	= $input['BlockKumiku'];

					array_push($EscapeOnDeleteDatas, $escapeData);
				}
			}
			else
			{
				$data['AMDFlag'] = 0;
				
				$matchInOldsNotContainGen = array_values(array_filter($olds, function($item) use ($input){
					return ($item['NameNotGen'] == $input['BLockNameNotGen'] && $item['BKumiku'] == $input['BlockKumiku']);
				}));				

				if (count($matchInOldsNotContainGen) == 0){
					// $isChangeDate = ($dataGroup == 1 && $matchInOldsNotContainGen[0]['T_Date'] != $rangeStartDate) 
					// 			|| ($dataGroup == 2 && $matchInOldsNotContainGen[0]['SG_Date'] != $rangeStartDate) 
					// 			|| ($dataGroup == 3 && ($matchInOldsNotContainGen[0]['PlSDate'] != $rangeStartDate || $SG_EDate == $rangeFinishDate));

					if ($input['Gen'] == self::GEN_S)
					{
						$data['Log'] = config('message.msg_schem_imp_007');
					}
					elseif ($input['Gen'] == self::GEN_P)
					{
						$data['Log'] =  config('message.msg_schem_imp_006');
					}
					else
					{
						$data['Log'] = config('message.msg_schem_imp_004');
					}
					// $data['Log'] = config('message.msg_schem_imp_004');
					$this->addDataIntoImportData($ImportData, $data, $dataGroup == 3);
				}
				else
				{
					$genPInOlds = array_filter($matchInOldsNotContainGen, function($item){
						return ($item['Gen'] == self::GEN_P);
					});

					$genSInOlds = array_filter($matchInOldsNotContainGen, function($item){
						return ($item['Gen'] == self::GEN_S);
					});

					$genCInOlds = array_filter($matchInOldsNotContainGen, function($item){
						return ($item['Gen'] == self::GEN_C);
					});

					$genSInInputs = array_filter($inputArrData, function($item) use ($input){ 
						return ($item['Gen'] == self::GEN_S 
								&& $item['BLockNameNotGen'] == $input['BLockNameNotGen'] 
								&& $item['BlockKumiku'] == $input['BlockKumiku']);
					});

					$genPInInputs = array_filter($inputArrData, function($item) use ($input){
						return ($item['Gen'] == self::GEN_P 
								&& $item['BLockNameNotGen'] == $input['BLockNameNotGen'] 
								&& $item['BlockKumiku'] == $input['BlockKumiku']);
					});
					
					// $tosaisの[Gen]が「C」
					if($input['Gen'] == self::GEN_C && (count($genSInOlds) > 0 || count($genPInOlds) > 0))
					{

						if(count($matchInOldsNotContainGen) == count($genPInOlds))
						{
							foreach ($genPInOlds as $dataP) {

								$isChangeDate = ($dataGroup == 1 && $dataP['T_Date'] != $rangeStartDate) 
											|| ($dataGroup == 2 && $dataP['SG_Date'] != $rangeStartDate) 
											|| ($dataGroup == 3 && ($dataP['PlSDate'] != $rangeStartDate || (isset($SG_EDate) && $SG_EDate == $rangeFinishDate)));

								$data['Log'] = $isChangeDate ? config('message.msg_schem_imp_001') . "\r\n" . config('message.msg_schem_imp_003') 
															: config('message.msg_schem_imp_001');
								$this->addDataIntoImportData($ImportData, $data, $dataGroup == 3);

							}						
							
						}
						else if (count($matchInOldsNotContainGen) == count($genSInOlds))
						{
							foreach ($genSInOlds as $dataS) {

								$isChangeDate = ($dataGroup == 1 && $dataS['T_Date'] != $rangeStartDate) 
											|| ($dataGroup == 2 && $dataS['SG_Date'] != $rangeStartDate) 
											|| ($dataGroup == 3 && ($dataS['PlSDate'] != $rangeStartDate || (isset($SG_EDate) && $SG_EDate == $rangeFinishDate)));

								$data['Log'] = $isChangeDate ? config('message.msg_schem_imp_001') . "\r\n" . config('message.msg_schem_imp_003') 
															: config('message.msg_schem_imp_001');
								$this->addDataIntoImportData($ImportData, $data, $dataGroup == 3);

							}								
						}
						else
						{
							foreach ($genPInOlds as $dataP) {

								$isChangeDate = ($dataGroup == 1 && $dataP['T_Date'] != $rangeStartDate) 
											|| ($dataGroup == 2 && $dataP['SG_Date'] != $rangeStartDate) 
											|| ($dataGroup == 3 && ($dataP['PlSDate'] != $rangeStartDate || (isset($SG_EDate) && $SG_EDate == $rangeFinishDate)));

								$data['Log'] = $isChangeDate ? config('message.msg_schem_imp_001') . "\r\n" . config('message.msg_schem_imp_003') 
															: config('message.msg_schem_imp_001');
								$this->addDataIntoImportData($ImportData, $data, $dataGroup == 3);

							}

							/*foreach ($genSInOlds as $dataS) {
								$escapeData = array();
								$escapeData['BlockName'] = $dataS['Name'];
								$escapeData['BKumiku'] 	= $dataS['BKumiku'];

								array_push($EscapeOnDeleteDatas, $escapeData);
							}*/
						}
					}
					else if ((count($genPInOlds) == 1 
							|| count($genSInOlds) == 1)
							&& count($matchInOldsNotContainGen) == 1)
					{
						if (count($genSInInputs) > 0 && count($genPInInputs) > 0)
						{
							$isChangeDate = ($dataGroup == 1 && $matchInOldsNotContainGen[0]['T_Date'] != $rangeStartDate) 
											|| ($dataGroup == 2 && $matchInOldsNotContainGen[0]['SG_Date'] != $rangeStartDate) 
											|| ($dataGroup == 3 && ($matchInOldsNotContainGen[0]['PlSDate'] != $rangeStartDate || (isset($SG_EDate) && $SG_EDate == $rangeFinishDate)));
							
							$data['Log'] = $isChangeDate ? config('message.msg_schem_imp_004') . "\r\n" . config('message.msg_schem_imp_003') 
											: config('message.msg_schem_imp_004');

							$this->addDataIntoImportData($ImportData, $data, $dataGroup == 3);
						}
					}
					else if (count($genCInOlds) > 0
							&& (count($genSInInputs) > 0 || count($genPInInputs) > 0))
					{
						// $tosaisに[Gen]が「P」と「S」の2レコード存在する。
						if (count($genSInInputs) > 0 && count($genPInInputs) > 0)
						{
							foreach ($genCInOlds as $dataC) {
								$isChangeDate = ($dataGroup == 1 && $dataC['T_Date'] != $rangeStartDate) 
												|| ($dataGroup == 2 && $dataC['SG_Date'] != $rangeStartDate) 
												|| ($dataGroup == 3 && ($dataC['PlSDate'] != $rangeStartDate || (isset($SG_EDate) && $SG_EDate == $rangeFinishDate)));

								if ($input['Gen'] == self::GEN_S)
								{
									$data['Log'] = config('message.msg_schem_imp_002') . "\r\n" . config('message.msg_schem_imp_004');
								}
								else
								{
									$data['Log'] = $isChangeDate ? config('message.msg_schem_imp_002') . "\r\n" . config('message.msg_schem_imp_003')
																: config('message.msg_schem_imp_002');
								}			

								$this->addDataIntoImportData($ImportData, $data, $dataGroup == 3);
							}
						}
						else
						{
							foreach ($genCInOlds as $dataC) {
								$isChangeDate = ($dataGroup == 1 && $dataC['T_Date'] != $rangeStartDate) 
												|| ($dataGroup == 2 && $dataC['SG_Date'] != $rangeStartDate) 
												|| ($dataGroup == 3 && ($dataC['PlSDate'] != $rangeStartDate || (isset($SG_EDate) && $SG_EDate == $rangeFinishDate)));

								$data['Log'] = $isChangeDate ? config('message.msg_schem_imp_002') . "\r\n" . config('message.msg_schem_imp_003')
															: config('message.msg_schem_imp_002');

								$this->addDataIntoImportData($ImportData, $data, $dataGroup == 3);
							}
						}

					}
				}
			}
		}		
	}

	public function addDataIntoImportData(&$ImportData, $data, $isSogumi = false) {
		if ($isSogumi)
		{
			$isExisted = false;
			foreach ($ImportData as $import) {
				// ※注3 $ImportDataに、[BlockName]と[BKumiku]が一致するレコードが既に有る場合、		
				if ($import['BlockName'] == $data['BlockName'] && $import['BKumiku'] == $data['BKumiku'])
				{
					// 配列に追加せず、SG_SDate とSG_EDateとLogだけを更新する
					$import['SG_SDate'] = $data['SG_SDate'];
					$import['SG_EDate'] = $data['SG_EDate'];
					$import['Log'] = $data['Log']; // TODO: confirm tai lieu va bo sung code					
					$isExisted = true;
				}
			}
			
			if (!$isExisted)
			{
				array_push($ImportData, $data);
			}
		}
		else
		{
			array_push($ImportData, $data);
		}
	}

	/**
	 * GET importData create button action
	 *
	 * @param Request
	 * @return View
	 *
	 * @create 2020/09/28 Dung
	 * @update 2020/10/20 Dung changed according to Rev7
	 */
	public function create(Request $request) {

		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		$existLog = $this->existsLock($menuInfo->KindID, config('system_const_schem.sys_menu_id_plan'), 
										$menuInfo->SessionID, valueUrlDecode($request->val2));

		if ($this->isRedirectMenuInfo($existLog)) {
            return $existLog;
		}
		$rows = $this->get_CynTempLogDataTosai($request);
		$originalError = [];

		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);

		}
		$this->data['rows'] = $rows;
		$this->data['originalError'] = $originalError;
		$this->data['request'] = $request;
		$this->data['menuInfo'] = $menuInfo;
 		//return view with all data
		return view('Schem/Import/create', $this->data);
	}
	/**
	 *
	 *
	 * @param Request
	 * @return array get_CynTempLogDataTosai
	 *
	 * @create 2020/09/29 Dung
	 * @update 2020/10/13 Dung
	 */
	private function get_CynTempLogDataTosai(Request $request)
	{
		$rows = Cyn_Temp_LogData_Tosai::select(
			'BlockName as fld1'
			, 'BKumiku as fld2'
			, 'Gen as fld3'
			, 'Log as fld4'
		);
		$val1= valueUrlDecode($request->val1);
		$val2= valueUrlDecode($request->val2);
		$rows = $rows->where('OrderNo', '=', valueUrlDecode($request->val1))
					 ->where('CKind','=', valueUrlDecode($request->val2));
		if(!isset($request->sort)){
			//初回表示
			$rows = $rows->sortable();
		}else{
			$rows = $rows->sortable($request->sort, $request->direction);
		}
		// paginate
		if (!isset($request->pageunit)) {
			//初回表示
			$rows = $rows->paginate(config('system_const.displayed_results_1'));
		}else{
			$rows = $rows->paginate(valueUrlDecode($request->pageunit), 
									[config('system_const.displayed_results_1'),
									config('system_const.displayed_results_2'),
									config('system_const.displayed_results_3')]);
		}
		// getKumikuData
		if ($rows != null) {
			foreach($rows as &$row) {
				// Kumiku as fld2
				if($row->fld2 != "") {
					$data = FuncCommon::getKumikuData($row->fld2);
					$row->fld2 = is_array($data) ? $data[2] : '';
				}
			}
		}
		return $rows;
	}

	/**
	 * POST data
	 *
	 * @param Request
	 * @return View
	 *
	 * @create 2020/09/30　Dung
	 * @update 2020/10/19 Dung changed according to Rev6
	 * @update 2020/10/20 Dung changed according to Rev7
	 * @update 2020/10/21 Dung changed according to Rev8
	 * @update 2020/10/27 Dung changed according to Rev9 + fix bug
	 * @update 2020/10/28 Dung changed according to Rev10
	 */
	public function accept(Request $request) {
        // 初期処理
        $menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));
        // 戻り値のデータ型をチェック
        if ($this->isRedirectMenuInfo($menuInfo)) {
            // エラーが起きたのでリダイレクト
            return $menuInfo;
		}
		$existLog = $this->existsLock($menuInfo->KindID, config('system_const_schem.sys_menu_id_plan'),
										 $menuInfo->SessionID, valueUrlDecode($request->val2));
		if ($this->isRedirectMenuInfo($existLog)) {
            return $existLog;
		}
        $orderNo = valueUrlDecode($request->val1);
        $cKind = valueUrlDecode($request->val2);
        $projectID = valueUrlDecode($request->val3);
        $idCynHistoryTosai = Cyn_History_Tosai::selectRaw('MAX(ID) as historyID')->first();
		$historyID = is_null($idCynHistoryTosai['historyID']) ? 1 : ($idCynHistoryTosai['historyID'] + 1);
		$timeTrackerCommon = new TimeTrackerCommon();
		//convert GetDate
		$dateNow = DB::selectOne('SELECT CONVERT(DATETIME, getdate()) AS sysdate')->sysdate;
		//insert to Cyn_History_Tosai
		DB::transaction(function () use($request, $menuInfo, $orderNo, $projectID, $historyID ,$dateNow) {
            $objHistoryTosai = new Cyn_History_Tosai;
            $objHistoryTosai->ID = $historyID;
            $objHistoryTosai->Import_User = $menuInfo->UserID;
            $objHistoryTosai->Import_Date = $dateNow;
            $objHistoryTosai->ProjectID = 0;
            $objHistoryTosai->OrderNo = $orderNo;
            $objHistoryTosai->CynProjectID = $projectID;
            $objHistoryTosai->CynOrderNo = $orderNo;
            $objHistoryTosai->StatusFlag = 0;
            $objHistoryTosai->save();
		});

        try {
            $importData = Cyn_Temp_LogData_Tosai::where('OrderNo' , '=' , $orderNo)->where('CKind', '=', $cKind)->get();
			//sequence use funcition returnValueSeqProjOrder
			$value = $this->returnValueSeqProjOrder( $orderNo,  $projectID);
            $koteis = [];
			foreach ($importData as $import) {
                if(!is_null($import->SG_SDate) && !is_null($import->SG_EDate) && in_array($import->AMDFlag, array(0, 1))) {
                    $kotei = array(
                        'sDate' => $import->SG_SDate,
                        'eDate' => $import->SG_EDate,
					);
					if(count($koteis) == 0) {
						array_push($koteis, $kotei);
					} else {
						$isExits = array_filter($koteis, function($item) use ($import) {
							return $item['sDate'] == $import->SG_SDate && $item['eDate'] == $import->SG_EDate;
						});
						if(count($isExits) == 0) {
							array_push($koteis, $kotei);
						}
					}
                }
			}
            if(count($koteis) >=1) {
				$days = $timeTrackerCommon->getWorkDays($orderNo, $projectID, $koteis);
                if(!is_array($days)) {
					$originalError = $days;
                    Cyn_History_Tosai::where('ID', '=', $historyID )->update(['StatusFlag' => -1]);
					$this->deleteLock($menuInfo->KindID, config('system_const_schem.sys_menu_id_plan'),
									  $menuInfo->SessionID, valueUrlDecode($request->val2));
                    $url = url('/');
                    $url .= '/' . $menuInfo->KindURL;
                    $url .= '/' . $menuInfo->MenuURL;
                    $url .= '/create';
                    $url .= '?cmn1=' . $request->cmn1;
                    $url .= '&cmn2=' . $request->cmn2;
                    $url .= '&val1=' .$request->val1;
                    $url .= '&val2=' .$request->val2;
                    $url .= '&val3=' .$request->val3;
                    $url .= '&val4=' .$request->val4;
                    $url .= '&err1=' . valueUrlEncode($originalError);
                    return redirect($url);
				}
                foreach($days as &$day) {
					foreach($importData as $import) {
						if ($day['sDate'] == $import->SG_SDate && $day['eDate'] = $import->SG_EDate) {
							$import['SG_Days'] = $day['workDays'];
						}
					}
				}
			}
			$importData = $importData->toArray();
			$level1s = $this->searchKotei($importData);
			$rootWorkItemID = $timeTrackerCommon->getOrderRoot($orderNo, $projectID);
			if(is_string($rootWorkItemID)){
				$originalError = $rootWorkItemID;
				Cyn_History_Tosai::where('ID', '=', $historyID )->update(['StatusFlag' => -1]);
				$this->deleteLock($menuInfo->KindID, config('system_const_schem.sys_menu_id_plan'),
									$menuInfo->SessionID, valueUrlDecode($request->val2));
				$url = url('/');
				$url .= '/' . $menuInfo->KindURL;
				$url .= '/' . $menuInfo->MenuURL;
				$url .= '/create';
				$url .= '?cmn1=' . $request->cmn1;
				$url .= '&cmn2=' . $request->cmn2;
				$url .= '&val1=' . $request->val1;
				$url .= '&val2=' . $request->val2;
				$url .= '&val3=' . $request->val3;
				$url .= '&val4=' . $request->val4;
				$url .= '&err1=' . valueUrlEncode($originalError);
				return redirect($url);
			}
			foreach ($level1s as &$level1) {
				$resultAfterInsertOrUpdate = $this->insertOrUpdate($orderNo, $projectID, $level1 , null, null,  
																   $rootWorkItemID, $cKind, $newLevel1);
				if(is_string($resultAfterInsertOrUpdate)){
					$originalError = $resultAfterInsertOrUpdate;
					Cyn_History_Tosai::where('ID', '=', $historyID )->update(['StatusFlag' => -1]);
					$this->deleteLock($menuInfo->KindID, config('system_const_schem.sys_menu_id_plan'), 
									  $menuInfo->SessionID, valueUrlDecode($request->val2));
					$url = url('/');
					$url .= '/' . $menuInfo->KindURL;
					$url .= '/' . $menuInfo->MenuURL;
					$url .= '/create';
					$url .= '?cmn1=' . $request->cmn1;
					$url .= '&cmn2=' . $request->cmn2;
					$url .= '&val1=' . $request->val1;
					$url .= '&val2=' . $request->val2;
					$url .= '&val3=' . $request->val3;
					$url .= '&val4=' . $request->val4;
					$url .= '&err1=' . valueUrlEncode($originalError);
					return redirect($url);
				}
				foreach ($level1 as $item) {
					$rows = [];
					if ($item['AMDFlag'] == 2 && is_null($item['NxtName']) && is_null($item['NxtBKumiku'])) {
						$rows[] = $item;
						foreach($rows as $row){
							$datas = Cyn_TosaiData::where('Cyn_TosaiData.ProjectID' , '=', $projectID)
														->where('Cyn_TosaiData.OrderNo', '=', $orderNo)
														->where('Cyn_TosaiData.Name', '=', $row['BlockName'])
														->where('Cyn_TosaiData.BKumiku', '=', $row['BKumiku'])
														->get();	
							if(count($datas) >0) {
								$workItemIDs = array();
								foreach($datas as $item) {
									array_push($workItemIDs,  $item->WorkItemID);
								}
								$rowKotei = $timeTrackerCommon->deleteItem($workItemIDs);
							}	
							if(is_string($rowKotei)){
								$originalError = $rowKotei;
								$url = url('/');
								$url .= '/' . $menuInfo->KindURL;
								$url .= '/' . $menuInfo->MenuURL;
								$url .= '/create';
								$url .= '?cmn1=' . $request->cmn1;
								$url .= '&cmn2=' . $request->cmn2;
								$url .= '&val1=' . $request->val1;
								$url .= '&val2=' . $request->val2;
								$url .= '&val3=' . $request->val3;
								$url .= '&val4=' . $request->val4;
								$url .= '&err1=' . valueUrlEncode($originalError);
								return redirect($url);
							}
							$arrayTemp = array();
							$delLists = $this->delList($importData, $row['BlockName'], $row['BKumiku'],$arrayTemp);
							foreach ($delLists as $delList) {
								DB::transaction(function () use($request, $delList, $projectID, $orderNo) {
									if($delList['AMDFlag'] == 2) {
										$result = Cyn_TosaiData::where('ProjectID', '=', $projectID)
																->where('orderNo ', '=', $orderNo)
																->where('Name', '=', $delList['BlockName'])
																->where('BKumiku', '=', $delList['BKumiku'])
																->delete();
									}
								});
							}
						}
					}
				}
			
			}
            $lastID = 1;
            DB::transaction(function () use($request, $importData, $orderNo, $cKind, $lastID, $historyID, $menuInfo) {
                foreach ($importData as $item) {
                    $objLogDataTosai = new Cyn_LogData_Tosai();
                    $objLogDataTosai->HistoryID = $historyID;
                    $objLogDataTosai->ID = $lastID;
                    $objLogDataTosai->BlockName = $item['BlockName'];
                    $objLogDataTosai->BKumiku = $item['BKumiku'];
                    $objLogDataTosai->Gen = $item['Gen'];
                    $objLogDataTosai->Log = $item['Log'];
                    $objLogDataTosai->TDate = !array_key_exists('TDate',$item) ? null : $item['TDate'];
                    $objLogDataTosai->KDate = !array_key_exists('KDate',$item) ? null : $item['KDate'];
                    $objLogDataTosai->SG_SDate = !array_key_exists('SG_SDate',$item) ? null : $item['SG_SDate'];
					$objLogDataTosai->SG_EDate = !array_key_exists('SG_EDate',$item) ? null : $item['SG_EDate'];
					$objLogDataTosai->NxtName = $item['NxtName'];
                    $objLogDataTosai->NxtBKumiku = $item['NxtBKumiku'];
                    $objLogDataTosai->save();
                    $lastID++;
				}
                Cyn_Temp_LogData_Tosai::where('OrderNo', '=', $orderNo )->where('CKind', '=', $cKind)->delete();
                //deleteLock-----
				$this->deleteLock($menuInfo->KindID, config('system_const_schem.sys_menu_id_plan'), 
								  $menuInfo->SessionID, valueUrlDecode($request->val2));
                Cyn_History_Tosai::where('ID', '=', $historyID )->update(['StatusFlag' => 1]);
            });
        } catch (\Exception $ex){
            Cyn_History_Tosai::where('ID', '=', $historyID )->update(['StatusFlag' => -1]);
			$this->deleteLock($menuInfo->KindID, config('system_const_schem.sys_menu_id_plan'), 
							  $menuInfo->SessionID, valueUrlDecode($request->val2));
			throw $ex;
			return;
        }
		//redirect url ----
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
		return redirect($url);
	}

	/**
	 * function sequen
	 *
	 * @param projectID
	 * @param orderNo
	 * @return mixed
	 *
	 * @create 2020/10/5　Dung
	 * @update
	 */
	private function returnValueSeqProjOrder($orderNo, $projectID) {
		$seqName = sprintf('seq_Cyn_BlockKukaku_%s_%s',$orderNo, $projectID);
		$sqlSeq = sprintf('SELECT NEXT VALUE FOR %s as SeqValue', $seqName);
		$flagGetValue = false;
		$value = config('system_const.seq_start_with');
		try {
			$seq_mstProject = DB::select($sqlSeq);
			if(count($seq_mstProject) > 0) {
				$value = $seq_mstProject[0]->SeqValue;
				$flagGetValue = true;
			}
		} catch (QueryException $e) {
			//$sqlDropSeq = sprintf('DROP SEQUENCE %s', $seqName);			 
			$sqlCreateSeq = sprintf('CREATE SEQUENCE %s START WITH %d MINVALUE %d %s', $seqName, 
								config('system_const.seq_start_with'), 
								config('system_const.seq_start_with'), 
								config('system_const.seq_option')
							);

			$createSqlSeq = DB::connection()->getPdo()->exec($sqlCreateSeq);
		} finally {
			if(!$flagGetValue) {
				$seq_mstProject = DB::select($sqlSeq);
				if(count($seq_mstProject) > 0) {
					$value = $seq_mstProject[0]->SeqValue;
				}
			}
		}
		return (int)$value;
	}
	/**
	 * function search kotei
	 *
	 * @param importData
	 * @return mixed
	 *
	 * @create 2020/10/5　Dung
	 * @update
	 */
	private function searchKotei($importData) {
		$array2 = array();
		$array1 = array_filter($importData,function($item){
			return is_null($item['NxtName']) && is_null($item['NxtBKumiku']);
		});
		foreach($array1 as $arr1) {
			$arrayTemp = array();
			array_push($arrayTemp, $arr1);

			$itemLevel1 = array_filter($importData,function($item) use ($arr1){
				return $item['NxtName'] == $arr1['BlockName'] && !is_null($item['NxtName']);
			});
			if(count($itemLevel1) > 0) {
				foreach ($itemLevel1 as $lv1) {
					array_push($arrayTemp, $lv1);
					$itemLevel2 = array_filter($importData,function($item) use ($lv1){
						return$item['NxtName'] == $lv1['BlockName'];
					});
					ksort($itemLevel2);
					foreach($itemLevel2 as $lv2) {
						array_push($arrayTemp, $lv2);
					}
				}
			}
			array_push($array2, $arrayTemp);
		}
		return $array2;
	}
	/**
	 * function insert update Cyn_TosaiData
	 *
	 * @param orderNo
	 * @param projectID
	 * @param importData
	 * @param rootWorkItemID
	 * @param cKind
	 * @return mixed
	 *
	 * @create 2020/10/5　Dung
	 * @update 2020/10/19　Dung
	 */
	private function insertOrUpdate($orderNo, $projectID, &$importData, $nxtName, $nxtBKumiku, $rootWorkItemID, $cKind, &$newImportData) {
        try {
            DB::transaction(function () use ($orderNo, $projectID, &$importData, $nxtName, $nxtBKumiku, $rootWorkItemID, $cKind) {
				$importDataHasAMDFlag1 = array_filter($importData, function($item) {
					return $item['AMDFlag'] == 1;
				});
				// AMDFlag = 1 -> update table Cyn_TosaiData with condition
				$this->processToTimetracker($orderNo, $projectID, $importData, null, null, $rootWorkItemID);
				foreach($importDataHasAMDFlag1 as $data) {
					$workItemID = null;
					$objTosai = Cyn_TosaiData::where('ProjectID', '=', $projectID)
											->where('OrderNo', '=', $orderNo)
											->where('Name', '=', $data['BlockName'])
											->where('BKumiku', '=', $data['BKumiku'])->first();
					if($objTosai != null) {
						$workItemID = $objTosai->WorkItemID;
					}
					$dataTosai['CKind'] = $cKind;
					$dataTosai['IsOriginal'] = 0;
					$dataTosai['T_Date'] = $data['TDate'];
					$dataTosai['SG_Date'] = $data['KDate'];
					$dataTosai['PlSDate'] = !array_key_exists('SG_SDate',$data) ? null : $data['SG_SDate'];
					$dataTosai['SG_Days'] = !array_key_exists('SG_Days',$data) ? null : $data['SG_Days'];
					$dataTosai['NxtName'] = $data['NxtName'];
					$dataTosai['NxtBKumiku'] = $data['NxtBKumiku'];

					$checkUpdate = Cyn_TosaiData::where('ProjectID', '=', $projectID)
											->where('OrderNo', '=', $orderNo)
											->where('Name', '=', $data['BlockName'])
											->where('BKumiku', '=', $data['BKumiku'])->update($dataTosai);
					foreach($importData as &$lvl1) {
						if($lvl1['OrderNo'] == $orderNo && 
							$lvl1['CKind'] == $cKind && 
							$lvl1['ID'] == $data['ID']) {
							$lvl1['WorkItemID'] = $workItemID;
							break;
						}
					}
				}
				//AMDFlag = 0 add new data for table Cyn_TosaiData
				$importDataHasAMDFlag0 = array_filter($importData, function($item) {
					return $item['AMDFlag'] == 0;
				});
				foreach($importDataHasAMDFlag0 as $data) {
					$objTosai = new Cyn_TosaiData();
					$objTosai->ProjectID = $projectID;
					$objTosai->OrderNo = $orderNo;
					$objTosai->CKind = $cKind;
					$objTosai->WorkItemID = !array_key_exists('WorkItemID',$data) ? null : $data['WorkItemID'];
					$objTosai->Name = $data['BlockName'];
					$objTosai->BKumiku = $data['BKumiku'];
					$objTosai->IsOriginal = 0;
					$objTosai->T_Date = !array_key_exists('TDate',$data) ? null : $data['TDate'];
					$objTosai->SG_Date = !array_key_exists('KDate',$data) ? null : $data['KDate'];
					$objTosai->PlSDate = !array_key_exists('SG_SDate',$data) ? null : $data['SG_SDate'];
					$objTosai->SG_Days = !array_key_exists('SG_Days',$data) ? null : $data['SG_Days'];
					$objTosai->NxtName = !array_key_exists('NxtName',$data) ? null : $data['NxtName'];
					$objTosai->NxtBKumiku = !array_key_exists('NxtBKumiku',$data) ? null : $data['NxtBKumiku'];
					$objTosai->WorkItemID_T = !array_key_exists('WorkItemID_T',$data) ? null : $data['WorkItemID_T'];
					$objTosai->WorkItemID_K = !array_key_exists('WorkItemID_K',$data) ? null : $data['WorkItemID_K'];
					$objTosai->WorkItemID_S = !array_key_exists('WorkItemID_S',$data) ? null : $data['WorkItemID_S'];
					$objTosai->save();
					foreach($importData as &$lvl1) {
						if($lvl1['OrderNo'] == $orderNo && 
							$lvl1['CKind'] == $cKind && 
							$lvl1['ID'] == $data['ID']) {
							$lvl1['WorkItemID'] = $objTosai->WorkItemID;
							break;
						}
					}
				}
			});
        } catch (\Exception $ex) {
			$newIDs = array();
			$data = array_filter($importData, function($item) {
				return is_null($item['NxtName']) && is_null($item['NxtBKumiku']);
			});
			if(count($data) > 0) {
				foreach($data as $item) {
					if(!is_null($item['WorkItemID'])) {
						array_push($newIDs, $item);
					}
				}
			}
			$timeTrackerCommon = new TimeTrackerCommon();
			$res = $timeTrackerCommon->deleteItem($newIDs);
			if(is_string( $res)) {
				return $res;
			}
		}
    }
	/**
	 * function processToTimetracker
	 *
	 * @param orderNo
	 * @param projectID
	 * @param importData
	 * @param nxtName
	 * @param nxtBKumiku
	 * @param rootWorkItemID
	 * @return mixed
	 *
	 * @create 2020/10/5　Dung
	 * @update 2020/10/19　Dung
	 */
	private function  processToTimetracker($orderNo, $projectID, &$importData, $nxtName, $nxtBKumiku, $rootWorkItemID){
		$koteis = array_filter($importData, function($item) use($nxtName, $nxtBKumiku) {
			return ($item['AMDFlag'] == 0 || $item['AMDFlag'] == 1) 
										  && $item['NxtName'] == $nxtName 
										  && $item['NxtBKumiku'] == $nxtBKumiku;
		});
		$newKoteis = array();
		foreach ($koteis as $row) {
			if($row['AMDFlag'] == 0 || $row['AMDFlag'] == 1){
			    if($row['NxtName'] == null){
                    $newKotei['parent'] = $rootWorkItemID;
				}else {
						$hasData = array_filter($importData, function($item) use($row) {
							return $item['BlockName'] == $row['NxtName'] && $item['BKumiku'] == $row['NxtBKumiku'];
						});
						$newKotei['parent'] = $hasData['0']['WorkItemID'];
				}
			}
			$newKotei['tDate']	 	= 	$row['TDate'];
			$newKotei['kDate'] 		= 	$row['KDate'];
			$newKotei['sg_SDate'] 	= 	$row['SG_SDate'];
			$newKotei['sg_EDate'] 	= 	$row['SG_EDate'];
			$newKotei['name'] 		= 	$row['BlockName'];
			$newKotei['bKumiku'] 	= 	$row['BKumiku'];
			$newKotei['workItemID'] =  !array_key_exists('WorkItemID',$row) ? null : $row['WorkItemID'];	
			$newKotei['workItemID_T'] 	=  !array_key_exists('WorkItemID_T',$row) ? null : $row['WorkItemID_T'];	
			$newKotei['workItemID_K'] 	=  !array_key_exists('WorkItemID_K',$row) ? null : $row['WorkItemID_K'];	
			$newKotei['workItemID_S'] 	=  !array_key_exists('WorkItemID_S',$row) ? null : $row['WorkItemID_S'];	
			array_push($newKoteis, $newKotei);
		}
		$newIDs = TimeTrackerFuncSchem::insertKotei($orderNo, $projectID, $newKoteis);
		if(!is_array($newIDs)){
			return $newIDs;
		} else {
            for( $i = 0 ; $i < count($newIDs); $i++) {
                foreach($importData as &$importDT){
                    if($importDT['BlockName'] == $newKoteis[$i]['name'] && $importDT['BKumiku'] == $newKoteis[$i]['bKumiku']){
						$importDT['WorkItemID'] 	=  $newIDs[0]['WorkItemID'];						
						$importDT['WorkItemID_T'] 	=  $newIDs[0]['WorkItemID_T'];
						$importDT['WorkItemID_K'] 	=  $newIDs[0]['WorkItemID_K'];
						$importDT['WorkItemID_S'] 	=  $newIDs[0]['WorkItemID_S'];
                    }
                }
			}
            foreach($koteis as $kotei) {
				$this->processToTimetracker($orderNo, $projectID, $importData, 
											$kotei['BlockName'], $kotei['BKumiku'], $rootWorkItemID, $koteis);
            }
		}
	}
	/**
	 * function processToTimetracker
	 *
	 * @param importData
	 * @param name
	 * @param bKumiku
	 * @param arrayResult
	 * @return mixed
	 *
	 * @create 2020/10/28　Dung
	 * @update 
	 */
	private function delList($importData, $name, $bKumiku ,$arrayResult) {
		$dataFirst = array_filter($importData,function($item) use($name, $bKumiku){
			return $item['BlockName'] == $name && $item['BKumiku'] == $bKumiku;
		});
		if(count($dataFirst) > 0) {
			foreach($dataFirst as $data) {
				if(!in_array($data, $arrayResult)) {
					array_push($arrayResult,$data);
				}
			}
			$array1 = array_filter($importData,function($item) use($name, $bKumiku){
				return $item['NxtName'] == $name && $item['NxtBKumiku'] == $bKumiku;
			});
			if(count($array1) > 0) {
				$obj = null;
				foreach($array1 as $item) {
					array_push($arrayResult,$item);
					$obj = $item;
				}
				return $this->delList($importData, $obj['BlockName'], $obj['BKumiku'],$arrayResult);
			}
		}
		return $arrayResult;
	}

	/**
	 * POST data
	 *
	 * @param Request
	 * @return View
	 *
	 * @create 2020/10/3　Dung
	 */
	public function cancel(Request $request) {
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));
		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		// deleteLock
		$deleteLock = $this->deleteLock($menuInfo->KindID, config('system_const_schem.sys_menu_id_plan'), 
										$menuInfo->SessionID, valueUrlDecode($request->val2));
		//redirect url
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
		return redirect($url);
	}
}
