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
 * @update 2020/11/20 Dung fix bug
 * @update 2020/11/25 Dung changed according to Rev21 screen 030401 , fix bug
 * @update 2021/1/5 Dung changed according to Rev26 screen 030401 , fix bug
 * @update 2021/1/5 Dung changed according to Rev19 screen 030402 , fix bug
 * @update 2021/1/14 Dung fix bug
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
use App\Librarys\CustomException;
use App\Librarys\MenuInfo;
use App\Models\MstProject;
use App\Models\MstOrderNo;
use App\Models\T_Tosai;
use App\Models\T_Kyokyu;
use App\Models\T_Sogumi;
use App\Models\Cyn_TosaiData;
use App\Models\Cyn_History_Tosai;
use App\Models\Cyn_LogData_Tosai;
use App\Models\WorkItemIDList;
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
				$project->ListKind = valueUrlEncode($project->ListKind);
				$project->ProjectName = htmlentities($project->ProjectName);
			}
		}
		$orders = MstOrderNo::select('mstOrderNo.OrderNo')
								->where('DispFlag','=',0)
								->orderBy('OrderNo','asc')
								->get();
		$itemShow = array(
			'val1' => isset($request->val1) ? valueUrlDecode($request->val1) :
						((trim(old('val1')) != '') ? valueUrlDecode(old('val1')) : ''),
			'val2' => isset($request->val2) ? valueUrlDecode($request->val2) :
						((trim(old('val2')) != '') ? valueUrlDecode(old('val2')) : 
						config('system_const.c_name_chijyo')),
			'val3' => isset($request->val3) ? valueUrlDecode($request->val3) :
						((trim(old('val3')) != '') ? valueUrlDecode(old('val3')) : ''),
			'val4' => isset($request->val4) ? valueUrlDecode($request->val4) :
						((trim(old('val4')) != '') ? valueUrlDecode(old('val4')) :
						 config('system_const.displayed_results_1')),
		);
		//error
		$originalError = array();
		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
		}
		$itemShow['val1'] = valueUrlEncode($itemShow['val1']);
		$itemShow['val2'] = valueUrlEncode($itemShow['val2']);
		$itemShow['val3'] = valueUrlEncode($itemShow['val3']);
		$itemShow['val4'] = valueUrlEncode($itemShow['val4']);

		$this->data['menuInfo'] = $menuInfo;
		$this->data['request'] = $request;
		$this->data['projects'] = $projects;
		$this->data['orders'] = $orders;
		$this->data['itemShow'] = $itemShow;
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
	 * @update 2020/11/20 Dung - Fix bug
	 * @update 2020/11/25 Dung changed according to Rev21 screen 030401 , fix bug
	 */
	public function import(ImportContentsRequest $request) {
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));
		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		$projectID = $request->val3;
		$orderNo = $request->val1;
		$val2 = $request->val2;
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
		// $tosaisと$sogumisを、[BlockName][BlockKumiku]をキーとしてマージし、新しい配列$mergesを作る。
		$sogumis = T_Sogumi::select('T_Sogumi.ProjectID',
									'T_Sogumi.OrderNo',
									'T_Sogumi.BlockName',
									'T_Sogumi.BlockKumiku',
									'T_Sogumi.WorkItemID',
									'WorkItemIDList.ID as ListWorkItemID'
					, DB::raw('Substring(T_Sogumi.BlockName, 0, len(T_Sogumi.BlockName)) as BLockNameNotGen')
					, DB::raw('Substring(T_Sogumi.BlockName, len(T_Sogumi.BlockName), 1) as Gen'))
					->join('WorkItemIDList', function($join) {
							$join->on('T_Sogumi.WorkItemID', '=', 'WorkItemIDList.WorkItemID');
					})
					->where('T_Sogumi.OrderNo','=',$orderNo)
					->where('T_Sogumi.ProjectID','=', 0);
		$merges = T_Tosai::select('T_Tosai.BlockName','T_Tosai.BlockKumiku','T_Tosai.WorkItemID',
									'WorkItemIDList.ID as ListWorkItemID',
									'sogumis.WorkItemID as S_WorkItemID'
							, DB::raw('Substring(T_Tosai.BlockName, 0, len(T_Tosai.BlockName)) as BLockNameNotGen')
							, DB::raw('Substring(T_Tosai.BlockName, len(T_Tosai.BlockName), 1) as Gen'))
							->leftJoinSub($sogumis, 'sogumis', function($join1) {
								$join1->on('T_Tosai.ProjectID', '=', 'sogumis.ProjectID')
										->on('T_Tosai.OrderNo', '=', 'sogumis.OrderNo')
										->on('T_Tosai.BlockName', '=', 'sogumis.BlockName')
										->on('T_Tosai.BlockKumiku', '=', 'sogumis.BlockKumiku');
							})
							->join('WorkItemIDList', function($join) {
									$join->on('T_Tosai.WorkItemID', '=', 'WorkItemIDList.WorkItemID');
								})
							->where('T_Tosai.OrderNo','=',$orderNo)
							->where('T_Tosai.ProjectID','=', 0)
							->get()->toArray();
		// モデルT_Kyokyu.phpを使用し、[OrderNo]= $orderNo AND [ProjectID] = 0の条件でレコード取得し、変数$kyokyusに入れる。
		$kyokyus = T_Kyokyu::select('T_Kyokyu.BlockName','T_Kyokyu.BlockKumiku',
									'T_Kyokyu.K_BlockName','T_Kyokyu.K_BlockKumiku',
									'T_Kyokyu.WorkItemID' , 'WorkItemIDList.ID as ListWorkItemID'
									, DB::raw('Substring(T_Kyokyu.BlockName, 0, len(T_Kyokyu.BlockName)) as BLockNameNotGen')
									, DB::raw('Substring(T_Kyokyu.BlockName, len(T_Kyokyu.BlockName), 1) as Gen'))
									->join('WorkItemIDList', function($join) {
										$join->on('T_Kyokyu.WorkItemID', '=', 'WorkItemIDList.WorkItemID');
									})
									->join('T_Tosai', function($join) {
											$join->on('T_Kyokyu.K_BlockName', '=', 'T_Tosai.BlockName')
												->on('T_Kyokyu.K_BlockKumiku', '=', 'T_Tosai.BlockKumiku')
												->on('T_Tosai.ProjectID', '=', 'T_Kyokyu.ProjectID')
												->on('T_Tosai.OrderNo', '=', 'T_Kyokyu.OrderNo');
										})
									->where('T_Kyokyu.OrderNo','=',$orderNo)
									->where('T_Kyokyu.ProjectID','=', 0)
									->get()->toArray();				
		// $tosais・$kyokyus・$sogumisから[WorkItemID]を抜き出し、配列$workItemIDsに入れる
		$workItemIDs = array();
		// add WorkItemID from $toais to $workItemIDs
		if(!is_null($merges)){
			foreach ($merges as $merge) {
				array_push($workItemIDs, array('workItemID' => $merge['WorkItemID'] , 'listID' => $merge['ListWorkItemID']));
			}
		}
		if(!is_null($merges)){
			foreach ($merges as $merge) {
				array_push($workItemIDs, array('workItemID'=> $merge['S_WorkItemID'] , 'listID' => $merge['ListWorkItemID']));
			}
		}
		// add WorkItemID from $kyokyus to $workItemIDs
		if(!is_null($kyokyus)){
			foreach ($kyokyus as $kyokyu) {
				array_push($workItemIDs, array('workItemID' => $kyokyu['WorkItemID'] , 'listID' => $kyokyu['ListWorkItemID']));
			}
		}
		// getKoteiRangeメソッドの引数に$workItemIDsを指定して呼び出し、戻り値を$rangesに入れる
		$timeTrackerCommon = new TimeTrackerCommon();
		$ranges = $timeTrackerCommon->getKoteiRange($workItemIDs);
		//TryLock
		if(!is_array($ranges)) {
			$originalError = $ranges;
		}
		else{
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
			$urlErr .= '&val5=' . valueUrlEncode($request->val5);
			$urlErr .= '&err1=' . valueUrlEncode($originalError);
			return redirect($urlErr);
		}
		$importData = array();
		$escapeAddToimportData = array();
		// push $merges data into ImportData
		$this->makeImportData($importData, $escapeAddToimportData, $merges, 0, $ranges, $olds, $oldLogs);
		// push kyokyu data into ImportData
		$this->makeImportData($importData, $escapeAddToimportData, $kyokyus, 1, $ranges, $olds, $oldLogs);
		//Loop $olds
		$importDataBeforeDelete = $importData;
		$addMore = array();
		// $oldsでループ
		foreach($olds as $old){
			//$import
			$matchGroupInImportData = array_filter($importDataBeforeDelete, function($item) use ($old){
				return ($item['BlockNameNotGen'] == $old['NameNotGen']
						&& $item['BKumiku'] == $old['BKumiku']);
			});
			$matchEscapeData = array_filter($escapeAddToimportData, function($item) use ($old){
				return ($item['NameNotGen'] == $old['NameNotGen']
						&& $item['BKumiku'] == $old['BKumiku']);
			});
			if (count($matchGroupInImportData) == 0 && count($matchEscapeData) == 0)
			{
				$data['AMDFlag'] = 2;
				$data['Log'] = config('message.msg_schem_imp_005');
				$this->pushDataIntoImportData($importData, $old, null, $data);
			}
		}
		$idCynTemLogDataTosai = Cyn_Temp_LogData_Tosai::selectRaw('MAX(ID) as MaxID')
											->where('OrderNo','=',$orderNo)
											->where('CKind','=',$val2)
											->first();
		$lastID  = is_null($idCynTemLogDataTosai) ? 0 : $idCynTemLogDataTosai->MaxID;
		// add data from $ImportData to table Cyn_Temp_LogData_Tosai
		DB::transaction(function () use($request, $importData, $lastID) {
			$result = Cyn_Temp_LogData_Tosai::where('OrderNo', '=', $request->val1 )
												->where('CKind', '=', $request->val2)
												->delete();
			foreach ($importData as $item) {
				$objLogDataTosai = new Cyn_Temp_LogData_Tosai;
				$objLogDataTosai->OrderNo = $request->val1;
				$objLogDataTosai->CKind = valueUrlDecode($request->val2);
				$objLogDataTosai->ID =	$lastID + 1;
				$objLogDataTosai->BlockName = $item['BlockName'];
				$objLogDataTosai->OldBlockName =!array_key_exists('OldBlockName',$item) ? null : $item['OldBlockName'];
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
		});
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
	/**
	 * function function process insert target data to $ImportData
	 *
	 * @param importData
	 * @param inputArrData	import対象データ
	 * @param dataGroup　0：tosai + sogumi, 1: kyokyu
	 * @param ranges Timetrackerの戻りデータのArray
	 * @param olds
	 * @param oldLogs
	 * @return mixed
	 *
	 * @create 2020/11/13　Dung
	 */
	public function makeImportData(&$importData, &$escapeAddToimportData, $inputArrData, $dataGroup,
										$ranges, $olds, $oldLogs) {
		foreach ($inputArrData as $input) {
			$data = array();
			$isChangeDate = false;
			$isContainSogumi = false;
			// Importデータ値を設定する
			$data['BlockName'] 	= $input['BlockName'];
			$data['BKumiku'] 	= $input['BlockKumiku'];
			$data['BlockNameNotGen'] =	 $input['BLockNameNotGen'];
			$data['Gen'] 		= $input['Gen'];
			$data['NxtName'] 	= ($dataGroup == 1) ? $input['K_BlockName'] : null;
			$data['NxtBKumiku'] = ($dataGroup == 1) ? $input['K_BlockKumiku'] : null;
			$data['TDate'] = null;
			$data['KDate'] = null;
			$data['SG_SDate'] = null;
			$data['SG_EDate'] = null;
			if($dataGroup == 0)
			{
				// Tosai's data
				$TDate = new Datetime($ranges[$input['WorkItemID']]['plannedStartDate']);
				$data['TDate'] = $TDate->format('Y-m-d');

				if (isset($input['S_WorkItemID']))
				{
					//Sogumi's date
					$isContainSogumi = true;
					$SG_SDate = new Datetime($ranges[$input['S_WorkItemID']]['plannedStartDate']);
					$data['SG_SDate'] = $SG_SDate->format('Y-m-d');

					$SG_EDate = new Datetime($ranges[$input['S_WorkItemID']]['plannedFinishDate']);
					$data['SG_EDate'] = $SG_EDate->format('Y-m-d');
				}
			}
			else
			{
				$KDate = new Datetime($ranges[$input['WorkItemID']]['plannedStartDate']);
				$data['KDate'] = $KDate->format('Y-m-d');
			}

			// $oldsに、Gen＝Pのレコードを取得
			$genPInOlds = array_values(array_filter($olds, function($item)  use ($input){
										return ($item['Gen'] == self::GEN_P
												&& $item['NameNotGen'] == $input['BLockNameNotGen']
												&& $item['BKumiku'] == $input['BlockKumiku']);
							}));

			// $oldsに、Gen＝Sのレコードを取得
			$genSInOlds = array_values(array_filter($olds, function($item)  use ($input){
										return ($item['Gen'] == self::GEN_S
											&& $item['NameNotGen'] == $input['BLockNameNotGen']
											&& $item['BKumiku'] == $input['BlockKumiku']);
									}));

			// $oldsに、Gen＝Cのレコードを取得
			$genCInOlds = array_values(array_filter($olds, function($item)  use ($input){
										return ($item['Gen'] == self::GEN_C
											&& $item['NameNotGen'] == $input['BLockNameNotGen']
											&& $item['BKumiku'] == $input['BlockKumiku']);
									}));

			// 新リストに、Gen＝Sのレコードを取得
			$genSInInputs = array_values(array_filter($inputArrData, function($item) use ($input){
											return ($item['Gen'] == self::GEN_S
													&& $item['BLockNameNotGen'] == $input['BLockNameNotGen']
													&& $item['BlockKumiku'] == $input['BlockKumiku']);
										}));

			// 新リストに、Gen＝Pのレコードを取得
			$genPInInputs = array_values(array_filter($inputArrData, function($item) use ($input){
											return ($item['Gen'] == self::GEN_P
													&& $item['BLockNameNotGen'] == $input['BLockNameNotGen']
													&& $item['BlockKumiku'] == $input['BlockKumiku']);
										}));
			// 新リストに、Gen＝Cのレコードを取得
			$genCInInputs = array_values(array_filter($inputArrData, function($item) use ($input){
											return ($item['Gen'] == self::GEN_C
													&& $item['BLockNameNotGen'] == $input['BLockNameNotGen']
													&& $item['BlockKumiku'] == $input['BlockKumiku']);
										}));

			//Before Gen = C [8> conditions 1,2,3,4,19]
			if(count($genCInOlds) > 0){
				$oldData = $genCInOlds[0];

				$SG_EDate = $this->getSGEDate($oldLogs, $oldData['Name'], $oldData['BKumiku']);

				// 日付変更チェック
				$isChangeDate = ($dataGroup == 1 && $oldData['SG_Date'] != $data['KDate'])
								|| ($dataGroup == 0
								&& ($oldData['T_Date'] != $data['TDate'] ||
									($isContainSogumi && (($oldData['PlSDate'] != $data['SG_SDate'])
													|| ($SG_EDate != $data['SG_EDate'])))));

				//After Gen = P & S [8> conditions 1]
				if(count($genSInInputs) > 0 && count($genPInInputs) > 0)
				{
					if ($input['Gen'] == self::GEN_P)
					{
						$data['AMDFlag'] = 1;
						$data['Log'] = $isChangeDate ? config('message.msg_schem_imp_002')
											. "\r\n" . config('message.msg_schem_imp_003')
											: config('message.msg_schem_imp_002');
					}
					if ($input['Gen'] == self::GEN_S){
						$data['AMDFlag'] = 0;
						$data['Log'] = config('message.msg_schem_imp_004');
					}
					$this->pushDataIntoImportData($importData, $oldData, $input, $data);
				}
				//After Gen = P || S [8> conditions 2,3]
				else if((count($genPInInputs) > 0 || count($genSInInputs) > 0) )
				{
					// If the date is changes
					$data['AMDFlag'] = 1;
					$data['Log'] = $isChangeDate ? config('message.msg_schem_imp_002')
										. "\r\n" . config('message.msg_schem_imp_003')
										: config('message.msg_schem_imp_002');
					$this->pushDataIntoImportData($importData, $oldData, $input, $data);

				}
				//After Gen = C [8> conditions 19]
				else if(count($genCInInputs) > 0)
				{
					if ($isChangeDate)
					{
						$data['AMDFlag'] = 1;
						$data['Log'] = config('message.msg_schem_imp_003');
						$this->pushDataIntoImportData($importData, $oldData, $input, $data);
					}
					else
					{
						//array_push($escapeAddToimportData, $oldData);
						$data['AMDFlag'] = null;
						$data['Log'] = config('message.msg_schem_imp_008');
						$this->pushDataIntoImportData($importData, $oldData, $input, $data);
					}
				}
			}
			//Before Gen = P && before Gen = S [8> conditions 5,6,7,8]
			else if (count($genSInOlds) > 0 && count($genPInOlds) > 0)
			{
				//After Gen = C [8> conditions 5]
				if(count($genCInInputs) > 0){
					// Befor Gen = SのデータをImportに削除対象として追加する
					$data['AMDFlag'] = 2;
					$data['Log'] = config('message.msg_schem_imp_005');
					$this->pushDataIntoImportData($importData, $genSInOlds[0], $input, $data);
					// Befor Gen = PのデータをImportに追加する
					$SG_EDate = $this->getSGEDate($oldLogs, $genPInOlds[0]['Name'], $genPInOlds[0]['BKumiku']);

					$isChangeDate = ($dataGroup == 1 && $genPInOlds[0]['SG_Date'] != $data['KDate'])
									|| ($dataGroup == 0
									&& ($genPInOlds[0]['T_Date'] != $data['TDate'] ||
									($isContainSogumi && (($genPInOlds[0]['PlSDate'] != $data['SG_SDate'])
									|| ($SG_EDate != $data['SG_EDate'])))));


					$data['AMDFlag'] = 1;
					$data['Log'] = $isChangeDate ? config('message.msg_schem_imp_001')
												. "\r\n" . config('message.msg_schem_imp_003')
												: config('message.msg_schem_imp_001');
					$this->pushDataIntoImportData($importData, $genPInOlds[0], $input, $data);
				}
				else if(count($genPInInputs) > 0 && count($genSInInputs) > 0){
					if($input['Gen'] == self::GEN_P)
					{
						$SG_EDate = $this->getSGEDate($oldLogs, $genPInOlds[0]['Name'], $genPInOlds[0]['BKumiku']);
						$isChangeDate = ($dataGroup == 1 && $genPInOlds[0]['SG_Date'] != $data['KDate'])
										|| ($dataGroup == 0
										&& ($genPInOlds[0]['T_Date'] != $data['TDate'] ||
										($isContainSogumi && (($genPInOlds[0]['PlSDate'] != $data['SG_SDate'])
															|| ($SG_EDate != $data['SG_EDate'])))));

						if ($isChangeDate)
						{
							//After Gen = P  [8> conditions 17]
							// 日付が変更された場合
							$data['AMDFlag'] = 1;
							$data['Log'] = config('message.msg_schem_imp_003');
							$this->pushDataIntoImportData($importData, $genPInOlds[0], $input, $data);
						}
						else
						{
							//array_push($escapeAddToimportData, $genPInOlds[0]);
							$data['AMDFlag'] = null;
							$data['Log'] = config('message.msg_schem_imp_008');
							$this->pushDataIntoImportData($importData, $genPInOlds[0], $input, $data);
						}
					}
					else if($input['Gen'] == self::GEN_S)
					{
						// Befor Gen = SのデータをImportに追加する
						$SG_EDate = $this->getSGEDate($oldLogs, $genSInOlds[0]['Name'], $genSInOlds[0]['BKumiku']);
						$isChangeDate = ($dataGroup == 1 && $genSInOlds[0]['SG_Date'] != $data['KDate'])
										|| ($dataGroup == 0
										&& ($genSInOlds[0]['T_Date'] != $data['TDate'] ||
										($isContainSogumi && (($genSInOlds[0]['PlSDate'] != $data['SG_SDate'])
															|| ($SG_EDate != $data['SG_EDate'])))));

						if ($isChangeDate)
						{
							//After Gen = S [8> conditions 18]
							$data['AMDFlag'] = 1;
							$data['Log'] = config('message.msg_schem_imp_003');
							$this->pushDataIntoImportData($importData, $genSInOlds[0], $input, $data);
						}
						else
						{
							//array_push($escapeAddToimportData, $genSInOlds[0]);
							$data['AMDFlag'] = null;
							$data['Log'] = config('message.msg_schem_imp_008');
							$this->pushDataIntoImportData($importData, $genSInOlds[0], $input, $data);
						}
					}
				}
				//After Gen = P [8> conditions 6]
				else if(count($genPInInputs) > 0 && count($genSInInputs) == 0){
					// Befor Gen = SのデータをImportに削除対象として追加する
					$data['AMDFlag'] = 2;
					$data['Log'] = config('message.msg_schem_imp_005');
					$this->pushDataIntoImportData($importData, $genSInOlds[0], $input, $data);

					// Befor Gen = PのデータをImportに追加する
					$SG_EDate = $this->getSGEDate($oldLogs, $genPInOlds[0]['Name'], $genPInOlds[0]['BKumiku']);
					$isChangeDate = ($dataGroup == 1 && $genPInOlds[0]['SG_Date'] != $data['KDate'])
									|| ($dataGroup == 0
									&& ($genPInOlds[0]['T_Date'] != $data['TDate'] ||
									($isContainSogumi && (($genPInOlds[0]['PlSDate'] != $data['SG_SDate'])
									|| ($SG_EDate != $data['SG_EDate'])))));
					if ($isChangeDate)
					{
						$data['AMDFlag'] = 1;
						$data['Log'] = config('message.msg_schem_imp_003');
						$this->pushDataIntoImportData($importData, $genPInOlds[0], $input, $data);
					}
					else
					{
						$data['AMDFlag'] = null;
						$data['Log'] = config('message.msg_schem_imp_008');
						$this->pushDataIntoImportData($importData, $genPInOlds[0], $input, $data);
						//array_push($escapeAddToimportData, $genPInOlds[0]);
					}
				}
				//After Gen = S [8> conditions 7]
				else if(count($genSInInputs) > 0 && count($genPInInputs) == 0){
					// Befor Gen = PのデータをImportに削除対象として追加する
					$data['AMDFlag'] = 2;
					$data['Log'] = config('message.msg_schem_imp_005');
					$this->pushDataIntoImportData($importData, $genPInOlds[0], $input, $data);

					// Befor Gen = SのデータをImportに追加する
					$SG_EDate = $this->getSGEDate($oldLogs, $genSInOlds[0]['Name'], $genSInOlds[0]['BKumiku']);
					$isChangeDate = ($dataGroup == 1 && $genSInOlds[0]['SG_Date'] != $data['KDate'])
									|| ($dataGroup == 0
									&& ($genSInOlds[0]['T_Date'] != $data['TDate'] ||
									($isContainSogumi && (($genSInOlds[0]['PlSDate'] != $data['SG_SDate'])
									|| ($SG_EDate != $data['SG_EDate'])))));
					if ($isChangeDate)
					{
						$data['AMDFlag'] = 1;
						$data['Log'] = config('message.msg_schem_imp_003');
						$this->pushDataIntoImportData($importData, $genSInOlds[0], $input, $data);
					}
					else
					{
						//array_push($escapeAddToimportData, $genSInOlds[0]);
						$data['AMDFlag'] = null;
						$data['Log'] = config('message.msg_schem_imp_008');
						$this->pushDataIntoImportData($importData, $genSInOlds[0], $input, $data);
					}
				}
			}
			//Before Gen = P [8> conditions 9,10, 11,12,17]
			else if(count($genPInOlds) > 0)
			{
				$oldData = $genPInOlds[0];
				$SG_EDate = $this->getSGEDate($oldLogs, $oldData['Name'], $oldData['BKumiku']);

				// 日付変更チェック
				$isChangeDate = ($dataGroup == 1 && $oldData['SG_Date'] != $data['KDate'])
								|| ($dataGroup == 0
								&& ($oldData['T_Date'] != $data['TDate'] ||
								   ($isContainSogumi && (($genPInOlds[0]['PlSDate'] != $data['SG_SDate'])
								   || ($SG_EDate != $data['SG_EDate'])))));
				//After Gen = S && After Gen = P  [8> conditions 10]
				if(count($genSInOlds) == 0 && count($genCInOlds) == 0
						&& (count($genSInInputs) > 0 && count($genPInInputs) > 0) )
				{
					if ($input['Gen'] == self::GEN_P)
					{
						if ($isChangeDate)
						{
							$data['AMDFlag'] = 1;
							$data['Log'] = config('message.msg_schem_imp_003');
							$this->pushDataIntoImportData($importData, $oldData, $input, $data);
						}
						else
						{
							//array_push($escapeAddToimportData, $oldData);
							$data['AMDFlag'] = null;
							$data['Log'] = config('message.msg_schem_imp_008');
							$this->pushDataIntoImportData($importData, $oldData, $input, $data);
						}
					}
					else if ($input['Gen'] == self::GEN_S)
					{
						$data['AMDFlag'] = 0;
						$data['Log'] = config('message.msg_schem_imp_004');
						$this->pushDataIntoImportData($importData, $oldData, $input, $data);
					}

				}
				//After Gen = C [8> conditions 9]
				else if(count($genSInOlds) == 0 && count($genCInOlds) == 0
					&& count($genCInInputs) > 0 && count($genPInInputs) == 0 && count($genSInInputs) == 0)
				{
					$data['AMDFlag'] = 1;
					$data['Log'] = $isChangeDate ? config('message.msg_schem_imp_001')
												. "\r\n" . config('message.msg_schem_imp_003')
												: config('message.msg_schem_imp_001');
					$this->pushDataIntoImportData($importData, $oldData, $input, $data);
				}
				// //After Gen = S  [8> conditions 11]
				else if( count($genSInOlds) == 0 && count($genSInInputs) > 0 && count($genPInInputs) == 0)
				{
					$data['AMDFlag'] = 1;
					$data['Log'] = $isChangeDate ? config('message.msg_schem_imp_007')
												. "\r\n" . config('message.msg_schem_imp_003')
												 : config('message.msg_schem_imp_007');
					$this->pushDataIntoImportData($importData, $oldData, $input, $data);
				}
				// //After Gen = P  [8> conditions 17]
				else if($input['Gen'] == self::GEN_P)
				{
					if ($isChangeDate)
					{
						// 日付が変更された場合
						$data['AMDFlag'] = 1;
						$data['Log'] = config('message.msg_schem_imp_003');
						$this->pushDataIntoImportData($importData, $oldData, $input, $data);
					}
					else
					{
						//array_push($escapeAddToimportData, $oldData);
						$data['AMDFlag'] = null;
						$data['Log'] = config('message.msg_schem_imp_008');
						$this->pushDataIntoImportData($importData, $oldData, $input, $data);
					}
				}
			}
			//Before Gen = S [8> conditions 13,14,15,16,18]
			else if(count($genSInOlds) > 0)
			{
				$oldData = $genSInOlds[0];
				// $olds->PlSDate == SG_SDate && $oldLog->SG_EDate == SG_EDate
				$SG_EDate = $this->getSGEDate($oldLogs, $oldData['Name'], $oldData['BKumiku']);

				// 日付変更チェック
				$isChangeDate = ($dataGroup == 1 && $oldData['SG_Date'] != $data['KDate'])
								|| ($dataGroup == 0
								&& ($oldData['T_Date'] != $data['TDate'] ||
									($isContainSogumi && (($oldData['PlSDate'] != $data['SG_SDate'])
											|| ($SG_EDate != $data['SG_EDate'])))));

				//After Gen = P && After Gen = S [8> conditions 14]
				if(count($genSInInputs) > 0 && count($genPInInputs) > 0)
				{
					if ($input['Gen'] == self::GEN_P)
					{
						$data['AMDFlag'] = 0;
						$data['Log'] = config('message.msg_schem_imp_004');
						$this->pushDataIntoImportData($importData, $oldData, $input, $data);
					}
					if ($input['Gen'] == self::GEN_S)
					{
						if ($isChangeDate)
						{
							$data['AMDFlag'] = 1;
							$data['Log'] = config('message.msg_schem_imp_003');
							$this->pushDataIntoImportData($importData, $oldData, $input, $data);
						}
						else
						{
							//array_push($escapeAddToimportData, $oldData);
							$data['AMDFlag'] = null;
							$data['Log'] = config('message.msg_schem_imp_008');
							$this->pushDataIntoImportData($importData, $oldData, $input, $data);
						}
					}
				}
				//After Gen = C  [8> conditions 13]
				else if( $input['Gen'] == self::GEN_C && count($genCInInputs) > 0)
				{
					$data['AMDFlag'] = 1;
					$data['Log'] = $isChangeDate ? config('message.msg_schem_imp_001')
												. "\r\n" . config('message.msg_schem_imp_003')
												: config('message.msg_schem_imp_001');
					$this->pushDataIntoImportData($importData, $oldData, $input, $data);
				}
				//After Gen = P  [8> conditions 15]
				else if(count($genPInOlds) == 0 && count($genPInInputs) > 0 && count($genSInInputs) == 0)
				{
					$data['AMDFlag'] = 1;
					$data['Log'] = $isChangeDate ? config('message.msg_schem_imp_006')
												. "\r\n" . config('message.msg_schem_imp_003')
												 : config('message.msg_schem_imp_006');
					$this->pushDataIntoImportData($importData, $oldData, $input, $data);
				}
				//After Gen = S [8> conditions 18]
				else if($input['Gen'] == self::GEN_S)
				{
					if ($isChangeDate)
					{
						$data['AMDFlag'] = 1;
						$data['Log'] = config('message.msg_schem_imp_003');
						$this->pushDataIntoImportData($importData, $oldData, $input, $data);
					}
					else
					{
						//array_push($escapeAddToimportData, $oldData);
						$data['AMDFlag'] = null;
						$data['Log'] = config('message.msg_schem_imp_008');
						$this->pushDataIntoImportData($importData, $oldData, $input, $data);
					}
				}
			}
			//Before Gen = '' [8> conditions 20, 21 22]
			else if ((count($genSInOlds) == 0 && count($genPInOlds) == 0 && count($genCInOlds) == 0)
					&& (count($genPInInputs) > 0 || count($genSInInputs) > 0 || count($genCInInputs) > 0 ))
			{
				$data['AMDFlag'] = 0;
				$data['Log'] = config('message.msg_schem_imp_004');
				$this->pushDataIntoImportData($importData, null, $input, $data);
			}
		}
	}
	/**
	 * function getSG_EDate from $oldLogs
	 *
	 * @param oldLogs
	 * @param blockName
	 * @param blockKumiku
	 * @return mixed
	 *
	 * @create 2020/11/13　Dung
	 */
	public function getSGEDate($oldLogs, $blockName, $blockKumiku) {
		$SG_EDate = null;
		// ※注2 $olds には[SG_EDate]が無い為、
		// $oldLogs から　[BlockName]と[BKumiku]が一致するレコードを探し、そのレコードの[SG_EDate]を終了日として扱う
		$matchInOlddata = array_values(array_filter($oldLogs, function($item) use ($blockName, $blockKumiku){
			return ($item['BlockName'] == $blockName && $item['BKumiku'] == $blockKumiku);
		}));
		if(count($matchInOlddata) > 0){
			$SG_EDate = new Datetime($matchInOlddata[0]['SG_EDate']) ;
			$SG_EDate =  $SG_EDate->format('Y-m-d');
		}
		return $SG_EDate;
	}
	/**
	 * function push target data to $ImportData
	 *
	 * @param importData
	 * @param data	import対象データ
	 * @return mixed
	 *
	 * @create 2020/11/13　Dung
	 */
	public function pushDataIntoImportData(&$importData, $oldData, $newData, $data) {

		if ($data['AMDFlag'] == 2)
		{
			// Importデータ値を設定する
			$data['BlockName'] 	= $oldData['Name'];
			$data['BKumiku'] 	= $oldData['BKumiku'];
			$data['Gen'] 		= $oldData['Gen'];
			$data['TDate'] = null;
			$data['NxtName'] 	= null;
			$data['NxtBKumiku'] =  null;
			$data['KDate'] =  null;
			$data['SG_SDate'] = null;
			$data['SG_EDate'] = null;
		}
		else if ($data['AMDFlag'] == 1)
		{
			$data['OldBlockName'] 	= $oldData['Name'];
		}
		array_push($importData, $data);
	}
	/**
	 * GET importData create button action
	 *
	 * @param Request
	 * @return View
	 *
	 * @create 2020/09/28 Dung
	 * @update 2020/10/20 Dung changed according to Rev7
	 * @update 2021/1/4 Dung fixbug No 185 buglist 01
	 * @update 2021/1/5 Dung changed according to Rev19
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
		$originalError = [];
		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
		}
		//sort and pageunit
		if(isset($request->val4) && in_array(valueUrlDecode($request->val4), [config('system_const.displayed_results_1'),
																config('system_const.displayed_results_2'),
																config('system_const.displayed_results_3')])){
			$pageunit = valueUrlDecode($request->val4);
		}else{
			$pageunit = config('system_const.displayed_results_1');
		}
		$sort = ['fld1','fld2','fld3','fld4'];
		if(isset($request->sort) && $request->sort != ''){
			$new = $request->sort;
			array_unshift($sort, $new);
			$sort = array_unique($sort);
		}
		$direction = (isset($request->direction) && $request->direction != '') ?  $request->direction : 'asc';
		$query = Cyn_Temp_LogData_Tosai::select(
					'BlockName as fld1'
					, 'BKumiku as fld2'
					, 'Gen as fld3'
					, 'Log as fld4')
					->where('OrderNo', '=', valueUrlDecode($request->val1))
					->where('CKind','=', valueUrlDecode($request->val2))
					->whereNotNull('AMDFlag')
					->get();
		//update code to REV 19
		$newQuery = collect();
		if(count($query) > 0) {
			foreach($query as $obj) {
				if ($obj->fld4 != "") {
					$tempExplode = explode("\r\n", $obj->fld4);
					if (count($tempExplode) > 1) {
						$obj->fld4 = $tempExplode[0];
						$newQuery->add($obj);
						$temp = new Cyn_Temp_LogData_Tosai;
						$temp->fld1 = $obj->fld1;
						$temp->fld2 = $obj->fld2;
						$temp->fld3 = $obj->fld3;
						$temp->fld4 = $tempExplode[1];
						$newQuery->add($temp);
					} else {
						$newQuery->add($obj);
					}
				}
			}
		}
		// getKumikuData
		if ($newQuery != null) {
			foreach($newQuery as &$row) {
				//Kumiku as fld2
				if($row['fld2'] != "") {
					$data = FuncCommon::getKumikuData($row['fld2']);
					$row['fld2'] = is_array($data) ? $data[2] : '';
				}
			}
		}
		$rows = $this->sortAndPagination($newQuery, $sort, $direction, $pageunit, $request);
		$this->data['rows'] = $rows;
		$this->data['originalError'] = $originalError;
		$this->data['request'] = $request;
		$this->data['menuInfo'] = $menuInfo;
 		//return view with all data
		return view('Schem/Import/create', $this->data);
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
	 * @update 2020/11/11 Dung changed according to Rev11
	 * @update 2020/12/9 Dung changed according to Rev16
	 * @update 2021/1/5 Dung changed according to Rev19
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
		DB::transaction(function () use($request, $menuInfo, $orderNo, $projectID, $historyID ,$dateNow){
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
		$isOK = false;
		try {
			//Time tracker getCalendar projectID = 0
			$baseCalendar = $timeTrackerCommon->getCalendar(0, $orderNo);
			if(is_string($baseCalendar)){
				$return = $this->stopProcessThenGoTo030401($request, $menuInfo, $baseCalendar, $historyID);
				throw new CustomException($return);
			}
			//	Time tracker getCalendar projectID = projectID
			$calendar = $timeTrackerCommon->getCalendar($projectID);
			if(is_string($calendar)){
				$return = $this->stopProcessThenGoTo030401($request, $menuInfo, $calendar, $historyID);
				throw new CustomException($return);
			}
			// Time tracker getCalendar checkCalendar
			$baseCalendar = $timeTrackerCommon->checkCalendar($baseCalendar , $calendar);
			if(is_string($baseCalendar)){
				$return = $this->stopProcessThenGoTo030401($request, $menuInfo, $baseCalendar, $historyID);
				throw new CustomException($return);
			}
			$importData = Cyn_Temp_LogData_Tosai::where('OrderNo' , '=' , $orderNo)->where('CKind', '=', $cKind)->get();
			$olds = Cyn_TosaiData::select('Name','BKumiku','WorkItemID','WorkItemID_T', 'WorkItemID_K', 'WorkItemID_S')
									->where('OrderNo' , '=' , $orderNo)
									->where('ProjectID', '=', $projectID)->get();
			//sequence use funcition returnValueSeqProjOrder
			$value = $this->returnValueSeqProjOrder( $orderNo,  $projectID);
			//総組ブロックのレコードに対する実働日数取得処理
			$koteis = [];
			foreach ($importData as $import) {
				if(!is_null($import->SG_SDate) && !is_null($import->SG_EDate) && in_array($import->AMDFlag, array(0, 1))) {
					$kotei = array(
						'sDate' => $import->SG_SDate,
						'eDate' => $import->SG_EDate,
					);
					$isExits = array_filter($koteis, function($item) use ($import) {
						return $item['sDate'] == $import->SG_SDate && $item['eDate'] == $import->SG_EDate;
					});
					if(count($isExits) == 0) {
						array_push($koteis, $kotei);
					}
				}
			}
			if(count($koteis) >=1) {
				$days = $timeTrackerCommon->getWorkDays($projectID, $orderNo, $koteis, $calendar);
				if(!is_array($days)) {
					$return = $this->stopProcessThenGoTo030401($request, $menuInfo, $days, $historyID);
					throw new CustomException($return);
				}
				foreach($days as &$day) {
					foreach($importData as $import) {
						if ($day['sDate'] == $import->SG_SDate && $day['eDate'] == $import->SG_EDate) {
							$import['SG_Days'] = $day['workDays'];
						}
					}
				}
			}
			// <６．工程検索処理>処理に、引数$importDataに変数$importDataを指定して呼び出し、配列$level1sに戻り値を取得。
			$importData = $importData->toArray();
			$level1s = $this->searchKotei($importData);
			// InsertとUpdateの処理
			$rootWorkItemID = $timeTrackerCommon->getOrderRoot($projectID, $orderNo);
			if(is_string($rootWorkItemID)){
				$return = $this->stopProcessThenGoTo030401($request, $menuInfo, $rootWorkItemID, $historyID);
				throw new CustomException($return);
			}
			// 配列$level1sのデータをループ変数$level1でループし
			$rows = [];
			foreach ($level1s as &$level1) {

				$resultAfterInsertOrUpdate = $this->insertOrUpdate($orderNo, $projectID, $level1 , null, null,
													$rootWorkItemID, $cKind, $newLevel1, $olds->toArray() , $calendar);								
				if(is_string($resultAfterInsertOrUpdate)){
					$return = $this->stopProcessThenGoTo030401($request, $menuInfo, $resultAfterInsertOrUpdate, $historyID);
					throw new CustomException($return);
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
								if(is_string($rowKotei)){
									$return = $this->stopProcessThenGoTo030401($request, $menuInfo, $rowKotei, $historyID);
									throw new CustomException($return);
								}
							}
							$arrayTemp = array();
							$delLists = $this->delList($importData, $row['BlockName'], $row['BKumiku'],$arrayTemp);
							DB::transaction(function () use($request, $delLists, $projectID, $orderNo) {
								foreach ($delLists as $delList) {
									if($delList['AMDFlag'] == 2) {
										$result = Cyn_TosaiData::where('ProjectID', '=', $projectID)
																	->where('orderNo ', '=', $orderNo)
																	->where('Name', '=', $delList['BlockName'])
																	->where('BKumiku', '=', $delList['BKumiku'])
																	->delete();
										}
									}
								}
							);
						}
					}
				}

			}
			//	配列$rowsを$rowでループし、deleteの処理を行う
			$lastID = 1;
			$newQuery = collect();
			if(count($importData) > 0) {
				foreach($importData as $obj) {
					if ($obj['Log'] != "") {
						$tempExplode = explode("\r\n", $obj['Log']);
						if (count($tempExplode) > 1) {
							$obj['Log'] = $tempExplode[0];
							$newQuery->add($obj);
							$temp = new Cyn_Temp_LogData_Tosai;
							$temp->BlockName = $obj['BlockName'];
							$temp->BKumiku = $obj['BKumiku'];
							$temp->Gen = $obj['Gen'];
							$temp->Log = $tempExplode[1];
							$newQuery->add($temp);
						} else {
							$newQuery->add($obj);
						}
					}
				}
			}
			DB::transaction(function () use($request, $newQuery, $orderNo, $cKind, $lastID, $historyID, $menuInfo) {
				foreach ($newQuery as $item) {
					$objLogDataTosai = new Cyn_LogData_Tosai();
					$objLogDataTosai->HistoryID = $historyID;
					$objLogDataTosai->ID = $lastID;
					$objLogDataTosai->BlockName = $item['BlockName'];
					$objLogDataTosai->BKumiku = $item['BKumiku'];
					$objLogDataTosai->Gen = $item['Gen'];
					$objLogDataTosai->Log = $item['Log'];
					$objLogDataTosai->TDate =  is_null($item['TDate']) ? null : $item['TDate'] ; 
					$objLogDataTosai->KDate = is_null($item['KDate']) ? null : $item['KDate'] ; 
					$objLogDataTosai->SG_SDate = is_null($item['SG_SDate']) ? null : $item['SG_SDate'] ; 
					$objLogDataTosai->SG_EDate = is_null($item['SG_EDate']) ? null : $item['SG_EDate'] ; 
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
			$isOK = true;
		} 
		catch (CustomException $ex) {			
			// error
			return redirect($ex->getMessage());
		} finally{
			if(!$isOK){
				Cyn_History_Tosai::where('ID', '=', $historyID )->update(['StatusFlag' => -1]);
			}
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
			$sqlCreateSeq = sprintf('CREATE SEQUENCE %s AS INT START WITH %d MINVALUE %d %s', $seqName,
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
			return empty($item['NxtName']) && empty($item['NxtBKumiku']);
		});
		$cnt = 0;
		if(count($array1) > 0) {
			foreach($array1 as $arr1) {
				$arrayTemp = array();
				array_push($arrayTemp, $arr1);
				$cnt += 1;
				$arrParent = $this->createGroupRecursive($importData, $arr1, $arrayTemp, $cnt);
				array_push($array2, $arrayTemp);
			}
			return $array2;
		} else {
			return array();
		}
	}

	/**
	 * function insert createGroupRecursive
	 *
	 * @param importData
	 * @param objChild
	 * @param arrTemp
	 * @param count
	 * @return mixed
	 *
	 * @create 2020/12/11　Dung
	 */
	private function createGroupRecursive($importData, $objChild, &$arrTemp, &$count) {
		if($count == count($importData)) {
			return true;
		}
		$findParent = array_filter($importData,function($item) use ($objChild){
			return $item['NxtName'] == $objChild['BlockName'] && !empty($item['NxtName']) && !empty($item['NxtBKumiku'])
				&& $item['NxtBKumiku'] == $objChild['BKumiku'];
		});
		if(count($findParent) > 0) {
			foreach($findParent as $parent) {
				$arrTemp[] = $parent;
				$count += 1;
				$next = $this->createGroupRecursive($importData, $parent, $arrTemp, $count);
			}
		}
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
	 * @update 2020/11/26　Dung Fix Bug
	 */
	private function insertOrUpdate($orderNo, $projectID, &$importData, $nxtName, $nxtBKumiku,
									$rootWorkItemID, $cKind, &$newImportData , $olds , $calendar) {
		try {
			DB::transaction(function () use ($orderNo, $projectID, &$importData, $nxtName, $nxtBKumiku,
												$rootWorkItemID, $cKind, $olds, $calendar) {
				// AMDFlag = 1 -> update table Cyn_TosaiData with condition
				$importDataHasAMDFlag1 = array_filter($importData, function($item) {
					return $item['AMDFlag'] == 1 && !is_null($item['AMDFlag']);
				});
				foreach($importDataHasAMDFlag1 as $data) {
					$dataTosai['CKind'] = $cKind;
					$dataTosai['IsOriginal'] = 0;
					$dataTosai['T_Date'] = $data['TDate'];
					$dataTosai['SG_Date'] = $data['KDate'];
					$dataTosai['PlSDate'] = !array_key_exists('SG_SDate',$data) ? null : $data['SG_SDate'];
					$dataTosai['SG_Days'] = !array_key_exists('SG_Days',$data) ? null : $data['SG_Days'];
					$dataTosai['Name'] = $data['BlockName'];
					$dataTosai['NxtName'] = $data['NxtName'];
					$dataTosai['NxtBKumiku'] = $data['NxtBKumiku'];
					$checkUpdate = Cyn_TosaiData::where('ProjectID', '=', $projectID)
											->where('OrderNo', '=', $orderNo)
											->where('Name', '=', $data['OldBlockName'])
											->where('BKumiku', '=', $data['BKumiku'])->update($dataTosai);
				}
				//update WorkItemID to timeTracker
				$processTimeTracker = $this->processToTimetracker($orderNo, $projectID, $importData, null, null, 
																			$rootWorkItemID, $olds , $calendar);
				if(is_string($processTimeTracker)){
					throw new CustomException($processTimeTracker);
				}
				//AMDFlag = 0 add new data for table Cyn_TosaiData
				$importDataHasAMDFlag0 = array_filter($importData, function($item) {
					return $item['AMDFlag'] == 0 && !is_null($item['AMDFlag'])  ;
				});
				foreach($importDataHasAMDFlag0 as $data) {
					$dataOlds = array_filter($olds, function($item) use($data) {
							return $item['Name'] == $data['BlockName'] && $item['BKumiku'] == $data['BKumiku'] ;
						});
					if(count($dataOlds) == 0)
					{
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
					}
				}
			});
		} catch (CustomException $ex) {
			$tempMsg = $ex->getMessage();
			$newIDs = array();
			$data = array_filter($importData, function($item) {
				return is_null($item['NxtName']) && is_null($item['NxtBKumiku']);
			});
			if(count($data) > 0) {
				foreach($data as $item) {
					if(isset($item['WorkItemID']) && !is_null($item['WorkItemID'])) {
						array_push($newIDs, $item['WorkItemID']);
					}
				}
			}
			$timeTrackerCommon = new TimeTrackerCommon();
			$res = $timeTrackerCommon->deleteItem($newIDs);
			if(is_string($res)) {
				$tempMsg = $res;
			}
			return $tempMsg;
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
	 * @update 2020/11/26　Dung changed according to Rev12 + fix bug
	 */
	private function  processToTimetracker($orderNo, $projectID, &$importData, $nxtName,
										$nxtBKumiku, $rootWorkItemID, $olds , $calendar){
		$koteis = array_filter($importData, function($item) use($nxtName, $nxtBKumiku) {
			return $item['NxtName'] == $nxtName && $item['NxtBKumiku'] == $nxtBKumiku;
		});

		if(count($koteis) > 0)
		{
			if(count($koteis) == 1) {
				$temp = array_values($koteis);
				if(in_array($temp[0]['AMDFlag'], array(2, null, ''))) {
					return;
				}
			}
			$newKoteis = array();
			foreach ($koteis as $row) {
				if( $row['AMDFlag'] == 0 || $row['AMDFlag'] == 1 ){
					if(is_null($row['NxtName'])){
						$newKotei['parent'] = $rootWorkItemID;
					}else {
						$hasData = array_filter($importData, function($item) use($row) {
							return $item['BlockName'] == $row['NxtName'] && $item['BKumiku'] == $row['NxtBKumiku'];
						});
						if(isset($hasData)){
							$newKotei['parent'] = $hasData['0']['WorkItemID'];
						}
					}
					$newKotei['tDate']	 	= 	$row['TDate'];
					$newKotei['kDate'] 		= 	$row['KDate'];
					$newKotei['sg_SDate'] 	= 	$row['SG_SDate'];
					$newKotei['sg_EDate'] 	= 	$row['SG_EDate'];
					$newKotei['name'] 		= 	$row['BlockName'];
					$newKotei['oldname'] 	=	$row['OldBlockName'];
					$newKotei['bKumiku'] 	= 	$row['BKumiku'];
					$oldWorkItemID =   array_values(array_filter($olds, function($item) use($row) {
						return $item['Name'] == $row['OldBlockName'] && $item['BKumiku'] == $row['BKumiku'];
					}));
					$newKotei['workItemID'] = (count($oldWorkItemID) > 0) 
																	? (!array_key_exists('WorkItemID',$oldWorkItemID[0])
					 												? null : $oldWorkItemID[0]['WorkItemID']) : null;
					$newKotei['workItemID_T'] = (count($oldWorkItemID) > 0) 
																	? (!array_key_exists('WorkItemID_T',$oldWorkItemID[0]) 
																	? null : $oldWorkItemID[0]['WorkItemID_T']) : null;
					$newKotei['workItemID_K'] = (count($oldWorkItemID) > 0) 
														? (!array_key_exists('WorkItemID_K',$oldWorkItemID[0]) ? null
														: $oldWorkItemID[0]['WorkItemID_K']) : null;
					$newKotei['workItemID_S'] = (count($oldWorkItemID) > 0) 
														? (!array_key_exists('WorkItemID_S',$oldWorkItemID[0]) ? null
														: $oldWorkItemID[0]['WorkItemID_S']) : null;
					array_push($newKoteis, $newKotei);
				}
			}
			
			$newIDs = TimeTrackerFuncSchem::insertKotei($projectID, $orderNo, $newKoteis , $calendar);
			if(!is_array($newIDs)){
				return $newIDs;
			} else {
				for( $i = 0 ; $i < count($newIDs); $i++) {
					foreach($importData as &$importDT){
						if($importDT['BlockName'] == $newKoteis[$i]['name'] && count($newKoteis) > 0
						&&  $importDT['BKumiku'] == $newKoteis[$i]['bKumiku'] ){
							$importDT['WorkItemID'] 	=  $newIDs[$i]['WorkItemID'];
							$importDT['WorkItemID_T'] 	=  $newIDs[$i]['WorkItemID_T'];
							$importDT['WorkItemID_K'] 	=  $newIDs[$i]['WorkItemID_K'];
							$importDT['WorkItemID_S'] 	=  $newIDs[$i]['WorkItemID_S'];
						}
					}
				}
			}
			foreach($koteis as $kotei) {
				$temp = $this->processToTimetracker($orderNo, $projectID, $importData,
									$kotei['BlockName'], $kotei['BKumiku'], $rootWorkItemID , $olds , $calendar );
				if (is_string($temp)) {
					return $temp;
				}
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
	 * function processToTimetracker
	 *
	 * @param originalError
	 * @return mixed
	 *
	 * @create 2021/1/7　Dung
	 * @update
	 */
	private function stopProcessThenGoTo030401($request, $menuInfo, $originalError, $historyID)
	{
		Cyn_History_Tosai::where('ID', '=', $historyID )->update(['StatusFlag' => -1]);
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
		return $url;
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
