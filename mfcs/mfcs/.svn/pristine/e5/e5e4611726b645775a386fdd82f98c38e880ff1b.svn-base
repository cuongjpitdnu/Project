<?php
/*
 * @NitteiReflectController.php
 *
 *
 * @create 2021/01/18 Anh
 * @update
 */
namespace App\Http\Controllers\Schem;

use App\Http\Controllers\Controller;
use App\Http\Requests\Schem\NitteiReflectRequest;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use App\Librarys\MenuInfo;
use App\Librarys\TimeTrackerCommon;
use App\Librarys\TimeTrackerFuncSchem;
use App\Models\MstProject;
use App\Models\MstOrderNo;
use App\Models\S_JobData;
use App\Models\Cyn_TosaiData;
use App\Models\Cyn_BlockKukaku;
use App\Models\Cyn_C_BlockKukaku;
use App\Models\Cyn_Plan;
use App\Models\Cyn_C_Plan;
use App\Models\Cyn_mstKotei;

/*
 *
 *
 * @create 2021/01/18 Anh
 * @update
 */
class NitteiReflectController extends Controller
{
	/**
	 * construct
	 * @param
	 * @return mixed
	 * @create 2021/01/18 Anh
	 */
	public function __construct(){
	}
	/**
	 * 工程定義管理画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2021/01/18 Anh
	 */
	public function index(Request $request)
	{
		return $this->initialize($request);
	}
	/**
	 * itemData list screen initial display processing
	 *
	 * @param Request
	 * @return View
	 *
	 * @create 2021/01/18 Anh
	 *
	 * @update
	 */
	private function initialize(Request $request) {
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		$this->data['menuInfo'] = $menuInfo;
		$originalError = [];
		$itemShow = array(
			'val1' => isset($request->val1) ? valueUrlDecode($request->val1) :
						((trim(old('val1')) != '') ? valueUrlDecode(old('val1')) : ''),
			'val2' => isset($request->val2) ? valueUrlDecode($request->val2) :
						((trim(old('val2')) != '') ? valueUrlDecode(old('val2')) : ''),
			'val3' => isset($request->val3) ? valueUrlDecode($request->val3) :
						((trim(old('val3')) != '') ? valueUrlDecode(old('val3')) : config('system_const.c_kind_chijyo')),
			'val4' => isset($request->val4) ? valueUrlDecode($request->val4) :
						((trim(old('val4')) != '') ? valueUrlDecode(old('val4')) : ''),
		);

		// data 1 for val 1
		$data1 = $this->getDataVal(false);
		$this->data['dataView']['data_1'] = $data1;
		$this->data['dataView']['data_1_all'] = $this->getDataVal(true);

		// data 2 for val 2
		$data2 = $this->getDataVal2(false);
		$this->data['dataView']['data_2'] = $data2;
		$this->data['dataView']['data_2_all'] = $this->getDataVal2(true);

		// data 4 for val 4
		$data4 = $this->getDataVal(false, valueUrlDecode($request->cmn1));
		$this->data['dataView']['data_4'] = $data4;
		$this->data['dataView']['data_4_all'] = $this->getDataVal(true, valueUrlDecode($request->cmn1));

		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
		}
		$itemShow['val1'] = valueUrlEncode($itemShow['val1']);
		$itemShow['val2'] = valueUrlEncode($itemShow['val2']);
		$itemShow['val3'] = valueUrlEncode($itemShow['val3']);
		$itemShow['val4'] = valueUrlEncode($itemShow['val4']);

		//request
		$this->data['request'] = $request;
		$this->data['originalError'] = $originalError;
		$this->data['itemShow'] = $itemShow;
		//return view with all data
		return view('Schem/Nitteireflect/index', $this->data);
	}

	/**
	 * get data value
	 *
	 * @param String $val2, $val3
	 * @return Object mixed
	 *
	 * @create 2021/01/18 Anh
	 *
	 * @update
	 */
	private function getDataVal($loadAll = false, $cmn1 = '') {
		$data = MstProject::select('ID as val', 'ProjectName as valName', 'ListKind')
				->where('SysKindID', '=', ($cmn1 !== '') ? $cmn1 : config('system_config.SysKindID_Sches'));
		$data = $data->orderBy('ProjectName')->get();

		if (count($data) > 0) {
			foreach ($data as &$row) {
				$row->val = valueUrlEncode($row->val);
				$row->ListKind = valueUrlEncode($row->ListKind);
				$row->valName = ($loadAll) ? htmlentities($row->valName) : $row->valName;
			}
		}
		return $data;
	}

	/**
	 * get data value 2
	 *
	 * @param String $val1
	 * @return Object mixed
	 *
	 * @create 2021/01/18 Anh
	 *
	 * @update
	 */
	private function getDataVal2($loadAll = false) {
		$data = MstOrderNo::select('mstOrderNo.OrderNo as val2', 'S_JobData.ProjectID', 'S_JobData.CKind')
				->join('S_JobData', 'mstOrderNo.OrderNo', '=', 'S_JobData.OrderNo')->where('DispFlag', '=', 0);
		$data = $data->orderBy('mstOrderNo.OrderNo')->get();
		if (count($data) > 0) {
			foreach ($data as &$row) {
				$row->NameShow = ($loadAll) ? htmlentities($row->val2) : $row->val2;
				$row->val2 = valueUrlEncode($row->val2);
				$row->CKind = valueUrlEncode($row->CKind);
				$row->ProjectID = valueUrlEncode($row->ProjectID);
			}
		}
		return $data;
	}

	/**
	 * action OK clicked
	 *
	 * @param NitteiReflectRequest 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2021/01/18 Anh
	 *
	 * @update
	 */
	public function reflect(NitteiReflectRequest $request) {
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		//validate form
		$validated = $request->validated();

		/* url redirect */
		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/index';
		$url .= '?cmn1=' . $request->cmn1;
		$url .= '&cmn2=' . $request->cmn2;
		for ($i = 1; $i <= 4; $i++) {
			$key = 'val'. $i;
			$url .= '&val'. $i . '=' . valueUrlEncode($request->$key);
		}

		$objTimeTrackerCommon = new TimeTrackerCommon();
		$objTimeTrackerFuncSchem = new TimeTrackerFuncSchem();

		// 小日程・検討ケースの実働期間を取得する。
		$smallProjectCalendar = $objTimeTrackerCommon->getCalendar($validated['val1']);	// $小日程プロジェクトカレンダー
		if (is_string($smallProjectCalendar)) {
			// error
			$url .= '&err1=' . valueUrlEncode($smallProjectCalendar);
			return redirect($url);
		}

		// 中日程・検討ケースの実働期間を取得する。
		$mediumProjectCalendar = $objTimeTrackerCommon->getCalendar($validated['val4']);	// $中日程プロジェクトカレンダー
		if (is_string($mediumProjectCalendar)) {
			// error
			$url .= '&err1=' . valueUrlEncode($mediumProjectCalendar);
			return redirect($url);
		}

		// 小日程、中日程のプロジェクトカレンダーを比較する。
		$checkCalendar = $objTimeTrackerCommon->checkCalendar($smallProjectCalendar, $mediumProjectCalendar);
		if (!is_null($checkCalendar)) {
			// error
			$url .= '&err1=' . valueUrlEncode($checkCalendar);
			return redirect($url);
		}

		// 反映元対象データの取得
		$condition = S_JobData::select(
									'S_JobData.WorkItemID as KeyWorkItemID',
									'S_JobData.DispName1',
									'S_JobData.BKumiku',
									'S_JobData.KKumiku',
									'S_JobData.Kotei',
									'S_JobData.AcFloor',
									'SJD2.WorkItemID'
								)
								->join('S_JobData as SJD2', function($join){
									$join->on('S_JobData.ProjectID', '=', 'SJD2.ProjectID')
										->on('S_JobData.OrderNo', '=', 'SJD2.OrderNo')
										->on('S_JobData.ID', '=', 'SJD2.PID');
								})
								->where('S_JobData.ProjectID', '=', $validated['val1'])
								->where('S_JobData.OrderNo', '=', $validated['val2'])
								->where('S_JobData.CKind', '=', $validated['val3'])
								->where('S_JobData.Level_Job', '=', 2)
								->where('S_JobData.IsOriginal', '=', 0)
								->where('SJD2.Level_Job', '=', 3)
								->where('SJD2.IsOriginal ', '=', 0)
								->orderBy('S_JobData.ProjectID')
								->orderBy('S_JobData.OrderNo')
								->orderBy('S_JobData.BKumiku')
								->orderBy('S_JobData.KKumiku')
								->orderBy('S_JobData.Kotei')
								->orderBy('S_JobData.AcFloor')
								->get();
		if (count($condition) == 0) {
			// error
			$url .= '&err1=' . valueUrlEncode(config('message.msg_schem_nitteireflect_001'));
			return redirect($url);
		}

		$smallScheduleInfor = array(); // $小日程情報
		$smallListWorkItemID = array(); // $小日程WorkItemIDリスト
		foreach ($condition as $value) {
			// 取得した[KeyWorkItemID]をキーに、連想配列としメモリ「$小日程情報」する。
			$smallScheduleInfor[$value->KeyWorkItemID] = [
				'DispName1' => $value->DispName1,
				'BKumiku' => $value->BKumiku,
				'KKumiku' => $value->KKumiku,
				'Kotei' => $value->Kotei,
				'AcFloor' => $value->AcFloor,
				'SDate' => null,
				'EDate' => null,
			];

			// 取得した[KeyWorkItemID]をキーに、[WorkItemID]を配列としメモリ「$WorkItemIDリスト」する。
			if (array_key_exists($value->KeyWorkItemID, $smallListWorkItemID)) {
				array_push($smallListWorkItemID[$value->KeyWorkItemID], $value->WorkItemID);
			} else {
				$smallListWorkItemID[$value->KeyWorkItemID] = [$value->WorkItemID];
			}
		}

		// 小日程の最小「着工日」、最大「完工日」の算出
		foreach ($smallListWorkItemID as $keyWorkItemID => $smallWorkItemID) {
			$dataGetKoteiRange = $objTimeTrackerCommon->getKoteiRange($smallWorkItemID, false, $smallProjectCalendar);
			if(is_string($dataGetKoteiRange)) {
				// error
				$url .= '&err1=' . valueUrlEncode($dataGetKoteiRange);
				return redirect($url);
			}
			$sDate = $dataGetKoteiRange[$smallWorkItemID[0]]['plannedStartDate'];
			$eDate = $dataGetKoteiRange[$smallWorkItemID[0]]['plannedFinishDate'];
			foreach ($dataGetKoteiRange as $koteiRange) {
				if (strtotime($koteiRange['plannedStartDate']) < strtotime($sDate)) {
					$sDate = $koteiRange['plannedStartDate'];
				}
				if (strtotime($koteiRange['plannedFinishDate']) > strtotime($eDate)) {
					$eDate = $koteiRange['plannedFinishDate'];
				}
			}
			$smallScheduleInfor[$keyWorkItemID]['SDate'] = $sDate;
			$smallScheduleInfor[$keyWorkItemID]['EDate'] = $eDate;
		}

		// 小日程情報を中日程情報に反映
		foreach ($smallScheduleInfor as $scheduleInfor) {
			$condition_1 = Cyn_TosaiData::select(
												'Cyn_Plan.WorkItemID',
												'Cyn_Plan.OrderNo',
												'Cyn_BlockKukaku.T_Name',
												'Cyn_BlockKukaku.T_BKumiku',
												'Cyn_BlockKukaku.Name',
												'Cyn_BlockKukaku.BKumiku',
											)
											->join('Cyn_BlockKukaku', function($join){
												$join->on('Cyn_TosaiData.ProjectID', '=', 'Cyn_BlockKukaku.ProjectID')
													->on('Cyn_TosaiData.OrderNo', '=', 'Cyn_BlockKukaku.OrderNo')
													->on('Cyn_TosaiData.Name', '=', 'Cyn_BlockKukaku.T_Name')
													->on('Cyn_TosaiData.BKumiku', '=', 'Cyn_BlockKukaku.T_BKumiku');
											})
											->join('Cyn_Plan', function($join){
												$join->on('Cyn_BlockKukaku.ProjectID', '=', 'Cyn_Plan.ProjectID')
													->on('Cyn_BlockKukaku.OrderNo', '=', 'Cyn_Plan.OrderNo')
													->on('Cyn_BlockKukaku.No', '=', 'Cyn_Plan.No');
											})
											->where('Cyn_TosaiData.ProjectID', '=', $validated['val4'])
											->where('Cyn_TosaiData.OrderNo', '=', $validated['val2'])
											->where('Cyn_TosaiData.CKind', '=', $validated['val3'])
											->where('Cyn_TosaiData.WorkItemID', '<>', 0)
											->where('Cyn_BlockKukaku.CKind', '=', $validated['val3'])
											->where('Cyn_BlockKukaku.Name', '=', $scheduleInfor['DispName1'])
											->where('Cyn_BlockKukaku.BKumiku', '=', $scheduleInfor['BKumiku'])
											->whereNull('Cyn_BlockKukaku.Del_Date')
											->where('Cyn_Plan.Kotei', '=', $scheduleInfor['Kotei'])
											->where('Cyn_Plan.KKumiku', '=', $scheduleInfor['KKumiku'])
											->where('Cyn_Plan.Floor', '=', $scheduleInfor['AcFloor'])
											->whereNull('Cyn_Plan.Del_Date');
			$condition_2 = Cyn_BlockKukaku::select(
												'Cyn_C_Plan.WorkItemID',
												'Cyn_C_Plan.OrderNo',
												'Cyn_C_BlockKukaku.T_Name',
												'Cyn_C_BlockKukaku.T_BKumiku',
												'Cyn_C_BlockKukaku.Name',
												'Cyn_C_BlockKukaku.BKumiku'
											)
											->join('Cyn_C_BlockKukaku', function($join){
												$join->on('Cyn_BlockKukaku.ProjectID', '=', 'Cyn_C_BlockKukaku.ProjectID')
													->on('Cyn_BlockKukaku.OrderNo', '=', 'Cyn_C_BlockKukaku.OrderNo')
													->on('Cyn_BlockKukaku.Name', '=', 'Cyn_C_BlockKukaku.T_Name')
													->on('Cyn_BlockKukaku.BKumiku', '=', 'Cyn_C_BlockKukaku.T_BKumiku');
											})
											->join('Cyn_C_Plan', function($join){
												$join->on('Cyn_C_BlockKukaku.ProjectID', '=', 'Cyn_C_Plan.ProjectID')
													->on('Cyn_C_BlockKukaku.OrderNo', '=', 'Cyn_C_Plan.OrderNo')
													->on('Cyn_C_BlockKukaku.No', '=', 'Cyn_C_Plan.No');
											})
											->where('Cyn_BlockKukaku.ProjectID', '=', $validated['val4'])
											->where('Cyn_BlockKukaku.OrderNo', '=', $validated['val2'])
											->where('Cyn_BlockKukaku.CKind', '=', $validated['val3'])
											->where('Cyn_C_BlockKukaku.CKind', '=', $validated['val3'])
											->where('Cyn_C_BlockKukaku.Name', '=', $scheduleInfor['DispName1'])
											->where('Cyn_C_BlockKukaku.BKumiku', '=', $scheduleInfor['BKumiku'])
											->whereNull('Cyn_C_BlockKukaku.Del_Date')
											->where('Cyn_C_Plan.Kotei', '=', $scheduleInfor['Kotei'])
											->where('Cyn_C_Plan.KKumiku', '=', $scheduleInfor['KKumiku'])
											->where('Cyn_C_Plan.Floor', '=', $scheduleInfor['AcFloor'])
											->whereNull('Cyn_C_Plan.Del_Date');
			$result_union = $condition_1->union($condition_2)
										->orderBy('T_Name')
										->orderBy('T_BKumiku')
										->orderBy('Name')
										->orderBy('BKumiku')
										->get();
			if (count($result_union) == 0) {
				continue;
			} elseif (count($result_union) > 1) {
				// error
				$url .= '&err1=' . valueUrlEncode(config('message.msg_schem_nitteireflect_002'));
				return redirect($url);
			} else {
				$result = $result_union->first();
				$checkInsertPlan = $objTimeTrackerFuncSchem->insertPlan($validated['val4'], $result->OrderNo, null, $result->WorkItemID,
									$scheduleInfor['SDate'], $scheduleInfor['EDate'], null, $mediumProjectCalendar);
				if(is_string($checkInsertPlan)) {
					// error
					$url .= '&err1=' . valueUrlEncode($checkInsertPlan);
					return redirect($url);
				}
			}
		}

		// redirect screen
		return redirect($url);
	}
}