<?php
/*
 * @ReCalcController.php
 * 中日程再計算条件設定画面コントローラーファイル
 *
 * @create 2020/11/23 Chien
 *
 * @update 2021/01/08 Chien
 */

namespace App\Http\Controllers\Schem;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use Illuminate\Database\QueryException;
use Illuminate\Pagination\LengthAwarePaginator;
use App\Http\Requests\Schem\ReCalcRequest;
use App\Librarys\FuncCommon;
use App\Librarys\MenuInfo;
use App\Librarys\MissingUpdateException;
use App\Librarys\CustomException;
use App\Librarys\TimeTrackerCommon;
use App\Librarys\TimeTrackerFuncSchem;
use App\Models\MstProject;
use App\Models\MstOrderNo;
use App\Models\Cyn_TosaiData;
use App\Models\Cyn_BlockKukaku;
use App\Models\Cyn_C_BlockKukaku;
use App\Models\Cyn_Plan;
use App\Models\Cyn_C_Plan;
use App\Models\Cyn_mstKotei;
use Exception;

/*
 * 中日程再計算条件設定画面コントローラー
 *
 * @create 2020/11/23 Chien
 *
 * @update
 */
class ReCalcController extends Controller
{
	/**
	 * 中日程再計算条件設定画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/11/23 Chien
	 * @update
	 */
	public function index(Request $request) {
		return $this->initialize($request);
	}

	/**
	 * init & prepare data to show 中日程再計算条件設定画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/11/23 Chien
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

		//initialize $originalError
		$originalError = [];

		$itemShow = array(
			'val1' => isset($request->val1) ? valueUrlDecode($request->val1) :
						((trim(old('val1')) != '') ? valueUrlDecode(old('val1')) : 0),
			'val2' => isset($request->val2) ? valueUrlDecode($request->val2) :
						((trim(old('val2')) != '') ? valueUrlDecode(old('val2')) : ''),
			'val3' => isset($request->val3) ? valueUrlDecode($request->val3) :
						((trim(old('val3')) != '') ? valueUrlDecode(old('val3')) : ''),
			'val4' => isset($request->val4) ? valueUrlDecode($request->val4) :
						((trim(old('val4')) != '') ? valueUrlDecode(old('val4')) : ''),
			'val5' => isset($request->val5) ? valueUrlDecode($request->val5) :
						((trim(old('val5')) != '') ? valueUrlDecode(old('val5')) : ''),
		);

		$dataSelect = $this->getInitData($menuInfo->KindID, $itemShow['val1'],
										$itemShow['val2'], $itemShow['val3'], $itemShow['val4']);
		$dataSelectAll = $this->getInitData($menuInfo->KindID, '', '', '', '', true);
		if (count($dataSelect['val2']) > 0) {
			$arrUnique = array();
			foreach ($dataSelect['val2'] as $key => &$itemVal2) {
				if (count($arrUnique) == 0) {
					$arrUnique[] = $itemVal2->ProjectName;
				} else {
					if (!in_array($itemVal2->ProjectName, $arrUnique)) {
						$arrUnique[] = $itemVal2->ProjectName;
					} else {
						unset($dataSelect['val2'][$key]);
					}
				}
			}
		}
		if (count($dataSelect['val3']) > 0) {
			$arrUnique = array();
			foreach ($dataSelect['val3'] as $key => &$itemVal3) {
				if (count($arrUnique) == 0) {
					$arrUnique[] = $itemVal3->NameShow;
				} else {
					if (!in_array($itemVal3->NameShow, $arrUnique)) {
						$arrUnique[] = $itemVal3->NameShow;
					} else {
						unset($dataSelect['val3'][$key]);
					}
				}
			}
		}
		if (count($dataSelect['val4']) > 0) {
			$arrUnique = array();
			foreach ($dataSelect['val4'] as $key => &$itemVal4) {
				if (count($arrUnique) == 0) {
					$arrUnique[] = $itemVal4->Name;
				} else {
					if (!in_array($itemVal4->Name, $arrUnique)) {
						$arrUnique[] = $itemVal4->Name;
					} else {
						unset($dataSelect['val4'][$key]);
					}
				}
			}
		}
		if (count($dataSelect['val5']) > 0) {
			$arrUnique = array();
			foreach ($dataSelect['val5'] as $key => &$itemVal5) {
				if (count($arrUnique) == 0) {
					$arrUnique[] = $itemVal5->NameShow;
				} else {
					if (!in_array($itemVal5->NameShow, $arrUnique)) {
						$arrUnique[] = $itemVal5->NameShow;
					} else {
						unset($dataSelect['val5'][$key]);
					}
				}
			}
		}

		$this->data['dataSelect'] = array(
			'val2' => $dataSelect['val2'],
			'val3' => $dataSelect['val3'],
			'val4' => $dataSelect['val4'],
			'val5' => $dataSelect['val5'],
			'val2LoadAll' => $dataSelectAll['val2'],
			'val3LoadAll' => $dataSelectAll['val3'],
			'val4LoadAll' => $dataSelectAll['val4'],
			'val5LoadAll' => $dataSelectAll['val5'],
		);

		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
		}
		$itemShow['val1'] = valueUrlEncode($itemShow['val1']);
		$itemShow['val2'] = valueUrlEncode($itemShow['val2']);
		$itemShow['val3'] = valueUrlEncode($itemShow['val3']);
		$itemShow['val4'] = valueUrlEncode($itemShow['val4']);
		$itemShow['val5'] = valueUrlEncode($itemShow['val5']);

		$this->data['originalError'] = $originalError;
		//request
		$this->data['request'] = $request;
		$this->data['itemData'] = $itemShow;
		//return view with all data
		return view('Schem/recalc/index', $this->data);
	}

	/**
	 * init & prepare data to show combobox on screen
	 *
	 * @param String $kindID
	 * @param String $listKind
	 * @param String $cKind
	 * @param String $projectID
	 * @param String $orderNo
	 * @param String $kotei
	 * @return Array mixed
	 *
	 * @create 2020/11/23 Chien
	 * @update
	 */
	private function getInitData($kindID = '', $cKind = '', $projectID = '', $orderNo = '', $kotei = '', $loadAll = false) {
		// Data A2
		$dataA2 = MstProject::select('ID', 'ProjectName', 'ListKind')->where('SysKindID', '=', $kindID);
		$dataA2 = ($cKind !== '' && is_numeric($cKind)) ? $dataA2->where('ListKind', '=', $cKind) : $dataA2;
		$dataA2 = $dataA2->orderBy('ProjectName')->distinct()->get();
		if (count($dataA2) > 0) {
			foreach ($dataA2 as &$itemA2) {
				$itemA2->ID = valueUrlEncode($itemA2->ID);
				$itemA2->ListKind = valueUrlEncode($itemA2->ListKind);
				$itemA2->ProjectName = ($loadAll) ? htmlentities($itemA2->ProjectName) : $itemA2->ProjectName;
			}
		}

		// tempProjectID if load default
		$tempProjectID = (count($dataA2) > 0) ? valueUrlDecode($dataA2->first()->ID) : '';

		// Data A3
		$dataA3 = MstOrderNo::select('mstOrderNo.OrderNo', 'Cyn_TosaiData.ProjectID', 'Cyn_TosaiData.CKind')
							->join('Cyn_TosaiData', 'mstOrderNo.OrderNo', '=', 'Cyn_TosaiData.OrderNo');
		$dataA3 = ($projectID !== '' && is_numeric($projectID)) ? $dataA3->where('Cyn_TosaiData.ProjectID', '=', $projectID) :
									(($tempProjectID != '' && !$loadAll) ? $dataA3->where('Cyn_TosaiData.ProjectID', '=', $tempProjectID) : $dataA3);
		$dataA3 = $dataA3->where('MstOrderNo.DispFlag', '=', 0)->orderBy('mstOrderNo.OrderNo')->distinct()->get();
		if (count($dataA3) > 0) {
			foreach ($dataA3 as &$itemA3) {
				$itemA3->NameShow = ($loadAll) ? htmlentities($itemA3->OrderNo) : $itemA3->OrderNo;
				$itemA3->OrderNo = valueUrlEncode($itemA3->OrderNo);
				$itemA3->ProjectID = valueUrlEncode($itemA3->ProjectID);
				$itemA3->CKind = valueUrlEncode($itemA3->CKind);
			}
		}

		// tempOrderNo if load default
		$tempOrderNo = (count($dataA3) > 0) ? valueUrlDecode($dataA3->first()->OrderNo) : '';

		// Data A4
		// #1
		$data_1 = Cyn_BlockKukaku::select(
									'Cyn_BlockKukaku.ProjectID', 'Cyn_BlockKukaku.OrderNo', 'Cyn_BlockKukaku.CKind',
									'Cyn_Plan.Kotei',
									'Cyn_mstKotei.Code', 'Cyn_mstKotei.Name'
								)
								->join('Cyn_Plan', function($join) {
									$join->on('Cyn_BlockKukaku.ProjectID', '=', 'Cyn_Plan.ProjectID')
										->on('Cyn_BlockKukaku.OrderNo', '=', 'Cyn_Plan.OrderNo');
								})
								->join('Cyn_mstKotei', function($join) {
									$join->on('Cyn_BlockKukaku.CKind', '=', 'Cyn_mstKotei.CKind')
										->on('Cyn_Plan.Kotei', '=', 'Cyn_mstKotei.Code');
								});
		$data_1 = ($cKind !== '' && is_numeric($cKind)) ? $data_1->where('Cyn_BlockKukaku.CKind', '=', $cKind) : $data_1;
		$data_1 = ($projectID !== '' && is_numeric($projectID)) ? $data_1->where('Cyn_BlockKukaku.ProjectID', '=', $projectID) :
									(($tempProjectID != '' && !$loadAll) ? $data_1->where('Cyn_BlockKukaku.ProjectID', '=', $tempProjectID) : $data_1);
		$data_1 = ($orderNo !== '') ? $data_1->where('Cyn_BlockKukaku.OrderNo', '=', $orderNo) :
									(($tempOrderNo != '' && !$loadAll) ? $data_1->where('Cyn_BlockKukaku.OrderNo', '=', $tempOrderNo) : $data_1);
		// #2
		$data_2 = Cyn_C_BlockKukaku::select(
									'Cyn_C_BlockKukaku.ProjectID', 'Cyn_C_BlockKukaku.OrderNo', 'Cyn_C_BlockKukaku.CKind',
									'Cyn_C_Plan.Kotei',
									'Cyn_mstKotei.Code', 'Cyn_mstKotei.Name'
								)
								->join('Cyn_C_Plan', function($join) {
									$join->on('Cyn_C_BlockKukaku.ProjectID', '=', 'Cyn_C_Plan.ProjectID')
										->on('Cyn_C_BlockKukaku.OrderNo', '=', 'Cyn_C_Plan.OrderNo');
								})
								->join('Cyn_mstKotei', function($join) {
									$join->on('Cyn_C_BlockKukaku.CKind', '=', 'Cyn_mstKotei.CKind')
										->on('Cyn_C_Plan.Kotei', '=', 'Cyn_mstKotei.Code');
								});
		$data_2 = ($cKind !== '' && is_numeric($cKind)) ? $data_2->where('Cyn_C_BlockKukaku.CKind', '=', $cKind) : $data_2;
		$data_2 = ($projectID !== '' && is_numeric($projectID)) ? $data_2->where('Cyn_C_BlockKukaku.ProjectID', '=', $projectID) :
									(($tempProjectID != '' && !$loadAll) ? $data_2->where('Cyn_C_BlockKukaku.ProjectID', '=', $tempProjectID) : $data_2);
		$data_2 = ($orderNo !== '') ? $data_2->where('Cyn_C_BlockKukaku.OrderNo', '=', $orderNo) :
									(($tempOrderNo != '' && !$loadAll) ? $data_2->where('Cyn_C_BlockKukaku.OrderNo', '=', $tempOrderNo) : $data_2);
		// UNION #1 & #2
		$dataA4 = $data_1->union($data_2)->orderBy('Name')->get();
		if (count($dataA4) > 0) {
			foreach ($dataA4 as &$itemA4) {
				$itemA4->Code = valueUrlEncode($itemA4->Code);
				$itemA4->ProjectID = valueUrlEncode($itemA4->ProjectID);
				$itemA4->OrderNo = valueUrlEncode($itemA4->OrderNo);
				$itemA4->CKind = valueUrlEncode($itemA4->CKind);
				$itemA4->Name = ($loadAll) ? htmlentities($itemA4->Name) : $itemA4->Name;
			}
		}

		// tempKotei if load default
		$tempKotei = (count($dataA4) > 0) ? valueUrlDecode($dataA4->first()->Code) : '';

		// Data A5
		// #1
		$data_1 = Cyn_BlockKukaku::select(
									'Cyn_BlockKukaku.ProjectID', 'Cyn_BlockKukaku.OrderNo', 'Cyn_BlockKukaku.CKind',
									'Cyn_Plan.Kotei', 'Cyn_Plan.KKumiku'
								)
								->join('Cyn_Plan', function($join) {
									$join->on('Cyn_BlockKukaku.ProjectID', '=', 'Cyn_Plan.ProjectID')
										->on('Cyn_BlockKukaku.OrderNo', '=', 'Cyn_Plan.OrderNo');
								})
								->join('Cyn_mstKotei', function($join) {
									$join->on('Cyn_BlockKukaku.CKind', '=', 'Cyn_mstKotei.CKind')
										->on('Cyn_Plan.Kotei', '=', 'Cyn_mstKotei.Code');
								});
		$data_1 = ($cKind !== '' && is_numeric($cKind)) ? $data_1->where('Cyn_BlockKukaku.CKind', '=', $cKind) : $data_1;
		$data_1 = ($projectID !== '' && is_numeric($projectID)) ? $data_1->where('Cyn_BlockKukaku.ProjectID', '=', $projectID) :
									(($tempProjectID != '' && !$loadAll) ? $data_1->where('Cyn_BlockKukaku.ProjectID', '=', $tempProjectID) : $data_1);
		$data_1 = ($orderNo !== '') ? $data_1->where('Cyn_BlockKukaku.OrderNo', '=', $orderNo) :
									(($tempOrderNo != '' && !$loadAll) ? $data_1->where('Cyn_BlockKukaku.OrderNo', '=', $tempOrderNo) : $data_1);
		$data_1 = ($kotei !== '') ? $data_1->where('Cyn_Plan.Kotei', '=', $kotei) :
									(($tempKotei != '' && !$loadAll) ? $data_1->where('Cyn_Plan.Kotei', '=', $tempKotei) : $data_1);
		// #2
		$data_2 = Cyn_C_BlockKukaku::select(
										'Cyn_C_BlockKukaku.ProjectID', 'Cyn_C_BlockKukaku.OrderNo', 'Cyn_C_BlockKukaku.CKind',
										'Cyn_C_Plan.Kotei', 'Cyn_C_Plan.KKumiku'
									)
									->join('Cyn_C_Plan', function($join) {
										$join->on('Cyn_C_BlockKukaku.ProjectID', '=', 'Cyn_C_Plan.ProjectID')
											->on('Cyn_C_BlockKukaku.OrderNo', '=', 'Cyn_C_Plan.OrderNo');
									})
									->join('Cyn_mstKotei', function($join) {
										$join->on('Cyn_C_BlockKukaku.CKind', '=', 'Cyn_mstKotei.CKind')
											->on('Cyn_C_Plan.Kotei', '=', 'Cyn_mstKotei.Code');
									});
		$data_2 = ($cKind !== '' && is_numeric($cKind)) ? $data_2->where('Cyn_C_BlockKukaku.CKind', '=', $cKind) : $data_2;
		$data_2 = ($projectID !== '' && is_numeric($projectID)) ? $data_2->where('Cyn_C_BlockKukaku.ProjectID', '=', $projectID) :
									(($tempProjectID != '' && !$loadAll) ? $data_2->where('Cyn_C_BlockKukaku.ProjectID', '=', $tempProjectID) : $data_2);
		$data_2 = ($orderNo !== '') ? $data_2->where('Cyn_C_BlockKukaku.OrderNo', '=', $orderNo) :
									(($tempOrderNo != '' && !$loadAll) ? $data_2->where('Cyn_C_BlockKukaku.OrderNo', '=', $tempOrderNo) : $data_2);
		$data_2 = ($kotei !== '') ? $data_2->where('Cyn_C_Plan.Kotei', '=', $kotei) :
									(($tempKotei != '' && !$loadAll) ? $data_2->where('Cyn_C_Plan.Kotei', '=', $tempKotei) : $data_2);
		// UNION #1 & #2
		$dataA5 = $data_1->union($data_2)->orderBy('KKumiku')->get();
		if (count($dataA5) > 0) {
			foreach ($dataA5 as &$itemA5) {
				$data = FuncCommon::getKumikuData($itemA5->KKumiku);
				$itemA5->NameShow = (is_array($data) && count($data) > 0) ? (($loadAll) ? htmlentities($data[2]) : $data[2]) : '';
				$itemA5->KKumiku = valueUrlEncode($itemA5->KKumiku);
				$itemA5->ProjectID = valueUrlEncode($itemA5->ProjectID);
				$itemA5->OrderNo = valueUrlEncode($itemA5->OrderNo);
				$itemA5->CKind = valueUrlEncode($itemA5->CKind);
				$itemA5->Kotei = valueUrlEncode($itemA5->Kotei);
			}
		}

		return array(
			'val2' => $dataA2,
			'val3' => $dataA3,
			'val4' => $dataA4,
			'val5' => $dataA5,
		);
	}

	/**
	 * action OK clicked
	 *
	 * @param ReCalcRequest 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/11/24 Chien
	 * @update
	 */
	public function recalc(ReCalcRequest $request) {
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
		for ($i = 1; $i <= 5; $i++) {
			$key = 'val'. $i;
			$url .= '&val'. $i . '=' . valueUrlEncode($request->$key);
		}

		$objTimeTrackerCommon = new TimeTrackerCommon();
		$objTimeTrackerFuncSchem = new TimeTrackerFuncSchem();

		// 検討ケースの実働期間を取得する。
		$dataGetCalendar = $objTimeTrackerCommon->getCalendar($validated['val2']);	// $プロジェクトカレンダー
		if ($dataGetCalendar != '' && is_string($dataGetCalendar)) {
			// error
			$url .= '&err1=' . valueUrlEncode($dataGetCalendar);
			return redirect($url);
		}

		$hasTableType0 = false;
		$hasTableType1 = false;
		// 親データの種別を確認する。
		$condition_1 = Cyn_TosaiData::selectRaw('0 as TableType')
									->join('Cyn_BlockKukaku', function($join) {
										$join->on('Cyn_TosaiData.ProjectID', '=', 'Cyn_BlockKukaku.ProjectID')
											->on('Cyn_TosaiData.OrderNo', '=', 'Cyn_BlockKukaku.OrderNo')
											->on('Cyn_TosaiData.CKind', '=', 'Cyn_BlockKukaku.CKind')
											->on('Cyn_TosaiData.Name', '=', 'Cyn_BlockKukaku.T_Name')
											->on('Cyn_TosaiData.BKumiku', '=', 'Cyn_BlockKukaku.T_BKumiku');
									})
									->where('Cyn_TosaiData.ProjectID', '=', $validated['val2'])
									->where('Cyn_TosaiData.OrderNo', '=', $validated['val3'])
									->where('Cyn_TosaiData.CKind', '=', $validated['val1'])
									->where('Cyn_TosaiData.WorkItemID ', '<>', 0);
		$condition_2 = Cyn_BlockKukaku::selectRaw('1 as TableType')
									->join('Cyn_C_BlockKukaku', function($join) {
										$join->on('Cyn_BlockKukaku.ProjectID', '=', 'Cyn_C_BlockKukaku.ProjectID')
											->on('Cyn_BlockKukaku.OrderNo', '=', 'Cyn_C_BlockKukaku.OrderNo')
											->on('Cyn_BlockKukaku.CKind', '=', 'Cyn_C_BlockKukaku.CKind')
											->on('Cyn_BlockKukaku.Name', '=', 'Cyn_C_BlockKukaku.T_Name')
											->on('Cyn_BlockKukaku.BKumiku', '=', 'Cyn_C_BlockKukaku.T_BKumiku');
									})
									->where('Cyn_BlockKukaku.ProjectID', '=', $validated['val2'])
									->where('Cyn_BlockKukaku.OrderNo', '=', $validated['val3'])
									->where('Cyn_BlockKukaku.CKind', '=', $validated['val1']);
		$result_union = $condition_1->union($condition_2)->get();
		$tableType = array();
		if (count($result_union) > 0) {
			$tableType = $result_union;
			foreach ($tableType as $item) {
				if ($item->TableType == 0) {
					$hasTableType0 = true;
				}
				if ($item->TableType == 1) {
					$hasTableType1 = true;
				}
			}
		}

		// 再計算対象データの取得
		$condition_1 = Cyn_TosaiData::select(
										'Cyn_BlockKukaku.No as B_No',
										'Cyn_Plan.WorkItemID as WorkItemID',
										'Cyn_Plan.No',
										'Cyn_Plan.Kotei',
										'Cyn_Plan.KoteiNo',
										'Cyn_Plan.KKumiku',
										'Cyn_Plan.Days',
										'Cyn_Plan.N_KoteiNo',
										'Cyn_Plan.N_Link',
										'Cyn_BlockKukaku.T_Name',
										'Cyn_BlockKukaku.T_BKumiku',
										'Cyn_BlockKukaku.Name',
										'Cyn_BlockKukaku.BKumiku',
									)
									->selectRaw('0 as TableType')
									->leftJoin('Cyn_BlockKukaku', function($join) {
										$join->on('Cyn_TosaiData.ProjectID', '=', 'Cyn_BlockKukaku.ProjectID')
											->on('Cyn_TosaiData.OrderNo', '=', 'Cyn_BlockKukaku.OrderNo')
											->on('Cyn_TosaiData.CKind', '=', 'Cyn_BlockKukaku.CKind')
											->on('Cyn_TosaiData.Name', '=', 'Cyn_BlockKukaku.T_Name')
											->on('Cyn_TosaiData.BKumiku', '=', 'Cyn_BlockKukaku.T_BKumiku');
									})
									->leftJoin('Cyn_Plan', function($join) {
										$join->on('Cyn_BlockKukaku.ProjectID', '=', 'Cyn_Plan.ProjectID')
											->on('Cyn_BlockKukaku.OrderNo', '=', 'Cyn_Plan.OrderNo')
											->on('Cyn_BlockKukaku.No', '=', 'Cyn_Plan.No');
									})
									->where('Cyn_TosaiData.ProjectID', '=', $validated['val2'])
									->where('Cyn_TosaiData.OrderNo', '=', $validated['val3'])
									->where('Cyn_TosaiData.CKind', '=', $validated['val1'])
									->where('Cyn_TosaiData.WorkItemID', '<>', 0)
									->whereNull('Cyn_BlockKukaku.Del_Date')
									->whereNull('Cyn_Plan.Del_Date');
		$condition_2 = Cyn_BlockKukaku::select(
										'Cyn_C_BlockKukaku.No as B_No',
										'Cyn_C_Plan.WorkItemID as WorkItemID',
										'Cyn_C_Plan.No',
										'Cyn_C_Plan.Kotei',
										'Cyn_C_Plan.KoteiNo',
										'Cyn_C_Plan.KKumiku',
										'Cyn_C_Plan.Days',
										'Cyn_C_Plan.N_KoteiNo',
										'Cyn_C_Plan.N_Link',
										'Cyn_C_BlockKukaku.T_Name',
										'Cyn_C_BlockKukaku.T_BKumiku',
										'Cyn_C_BlockKukaku.Name',
										'Cyn_C_BlockKukaku.BKumiku',
									)
									->selectRaw('1 as TableType')
									->leftJoin('Cyn_C_BlockKukaku', function($join) {
										$join->on('Cyn_BlockKukaku.ProjectID', '=', 'Cyn_C_BlockKukaku.ProjectID')
											->on('Cyn_BlockKukaku.OrderNo', '=', 'Cyn_C_BlockKukaku.OrderNo')
											->on('Cyn_BlockKukaku.CKind', '=', 'Cyn_C_BlockKukaku.CKind')
											->on('Cyn_BlockKukaku.Name', '=', 'Cyn_C_BlockKukaku.T_Name')
											->on('Cyn_BlockKukaku.BKumiku', '=', 'Cyn_C_BlockKukaku.T_BKumiku');
									})
									->leftJoin('Cyn_C_Plan', function($join) {
										$join->on('Cyn_C_BlockKukaku.ProjectID', '=', 'Cyn_C_Plan.ProjectID')
											->on('Cyn_C_BlockKukaku.OrderNo', '=', 'Cyn_C_Plan.OrderNo')
											->on('Cyn_C_BlockKukaku.No', '=', 'Cyn_C_Plan.No');
									})
									->where('Cyn_BlockKukaku.ProjectID', '=', $validated['val2'])
									->where('Cyn_BlockKukaku.OrderNo', '=', $validated['val3'])
									->where('Cyn_BlockKukaku.CKind', '=', $validated['val1'])
									->whereNull('Cyn_C_BlockKukaku.Del_Date')
									->whereNull('Cyn_C_Plan.Del_Date');
		$result_union = $condition_1->union($condition_2)
									->orderBy('T_Name')
									->orderBy('T_BKumiku')
									->orderBy('Name')
									->orderBy('BKumiku')
									->get();

		// $工程情報
		$processInformation = array();

		// ルートオブジェクトの取得
		$dataGetOrderRoot = $objTimeTrackerCommon->getOrderRoot($validated['val2'], $validated['val3']);	// $ルートオブジェクトのWorkItemID
		if ($dataGetOrderRoot != '' && is_string($dataGetOrderRoot)) {
			// error
			$url .= '&err1=' . valueUrlEncode($dataGetOrderRoot);
			return redirect($url);
		}

		// ルートオブジェクト配下すべてのWorkItemIDを取得する。
		$dataGetChildWorkItem = $objTimeTrackerCommon->getChildWorkItem($dataGetOrderRoot);	// $すべてのWorkItemID
		if ($dataGetChildWorkItem != '' && is_string($dataGetChildWorkItem)) {
			// error
			$url .= '&err1=' . valueUrlEncode($dataGetChildWorkItem);
			return redirect($url);
		}

		// list workItemID in $すべてのWorkItemID
		$listChildWorkItem = array();
		foreach ($dataGetChildWorkItem as $key => $value) {
			if (!in_array($key, $listChildWorkItem)) {
				$listChildWorkItem[] = $key;
			}
		}

		// $ブロックリスト
		$blockList = array();
		if (count($result_union) > 0) {
			foreach ($result_union as $row) {
				if (trim($row->B_No) != '') {
					if (!in_array($row->B_No, $blockList)) {
						$blockList[] = trim($row->B_No);
					}
					if (trim($row->Kotei) != '' && trim($row->KKumiku) != '') {
						$processInformation[trim($row->B_No)][] = array(
							'WorkItemID' => $row->WorkItemID,
							'No' => $row->No,
							'KoteiNo' => $row->KoteiNo,
							'Kotei' => $row->Kotei,
							'KKumiku' => $row->KKumiku,
							'Days' => $row->Days,
							'N_KoteiNo' => $row->N_KoteiNo,
							'N_Link' => $row->N_Link,
							'TableType' => $row->TableType,
						);
					}
				}

				if (trim($row->WorkItemID) !== '') {
					// 存在確認 / 一致するデータが存在しない場合
					if (!in_array($row->WorkItemID, $listChildWorkItem)) {
						// error
						$url .= '&err1=' . valueUrlEncode(sprintf(config('message.msg_cmn_db_028'), $row->WorkItemID));
						return redirect($url);
					}
				}
			}

			if (count($blockList) > 0) {
				try {
					DB::transaction(function () use ($blockList, $objTimeTrackerCommon, $processInformation,
													$objTimeTrackerFuncSchem, $dataGetCalendar, $validated) {
						foreach ($blockList as $row) {
							if (isset($processInformation[$row])) {
								if (count($processInformation[$row]) > 0) {
									// 「$キー工程」の取得
									$arrKeyProcess = array();
									foreach ($processInformation[$row] as $child) {
										// 一致するデータが取得できた場合
										if ($child['Kotei'] == $validated['val4'] && $child['KKumiku'] == $validated['val5']) {
											// has only 1 data could found
											$arrKeyProcess['KoteiNo'] = $child['KoteiNo'];			// $キー工程No
											$arrKeyProcess['N_KoteiNo'] = $child['N_KoteiNo'];		// $キー次工程No
											$arrKeyProcess['N_Link'] = $child['N_Link'];			// $キー次工程とのリンク日数
											$arrKeyProcess['WorkItemID'] = $child['WorkItemID'];	// $キー工程のWorkItemID
											$arrKeyProcess['TableType'] = $child['TableType'];		// $TableType - update rev5
										}
									}

									if (count($arrKeyProcess) > 0) {
										// 「$キー着工日」、「$キー完工日」の取得
										$dataGetKoteiRange = $objTimeTrackerCommon->getKoteiRange(array($arrKeyProcess['WorkItemID']), true, $dataGetCalendar);
										if ($dataGetKoteiRange != '' && is_string($dataGetKoteiRange)) {
											throw new CustomException($dataGetKoteiRange);
										}

										// $キー着工日
										$plannedStartDate = $dataGetKoteiRange[$arrKeyProcess['WorkItemID']]['plannedStartDate'];
										// $キー完工日
										$plannedFinishDate = $dataGetKoteiRange[$arrKeyProcess['WorkItemID']]['plannedFinishDate'];

										// update rev5
										$objUpdateDays['Days'] = $dataGetKoteiRange[$arrKeyProcess['WorkItemID']]['workDays'];
										if ($arrKeyProcess['TableType'] == 0) {
											//「$TableType」が「0」の場合、[Cyn_Plan]のデータ更新を行う。
											$tempUpdate = Cyn_Plan::where('WorkItemID', '=', $arrKeyProcess['WorkItemID'])->update($objUpdateDays);
										}
										if ($arrKeyProcess['TableType'] == 1) {
											//「$TableType」が「1」の場合、[Cyn_C_Plan]のデータ更新を行う。
											$tempUpdate = Cyn_C_Plan::where('WorkItemID', '=', $arrKeyProcess['WorkItemID'])->update($objUpdateDays);
										}

										$previous = $this->processRecursivePrevious($processInformation[$row], $arrKeyProcess, $plannedStartDate,
																			[$objTimeTrackerCommon, $objTimeTrackerFuncSchem], $validated, $dataGetCalendar);
										if (is_string($previous)) {
											throw new CustomException($previous);
										}

										$next = $this->processRecursiveNext($processInformation[$row], $arrKeyProcess,
																					$plannedFinishDate, $arrKeyProcess['N_Link'],
																					[$objTimeTrackerCommon, $objTimeTrackerFuncSchem],
																					$validated, $dataGetCalendar);
										if (is_string($next)) {
											throw new CustomException($next);
										}
									} else {
										// 一致するデータが取得できない場合
										continue;
									}
								}
							}
						}
					});
				} catch (CustomException $ex) {
					// error
					$url .= '&err1=' . valueUrlEncode($ex->getMessage());
					return redirect($url);
				}
			}
		}

		// redirect screen
		return redirect($url);
	}

	/**
	* Process previous kotei recursive
	*
	* @param Array $totalDataBlock
	* @param Array $arrKeyProcess
	* @param String $planDate
	* @param Array $arrTimeTracker
	* @param Array $validatedData
	* @param Array $dataGetCalendar
	* @return Mixed boolean || string
	*
	* @create 2020/12/10 Chien
	* @update
	*/
	private function processRecursivePrevious($totalDataBlock, $arrKeyProcess, &$planDate, $arrTimeTracker = array(), $validatedData, $dataGetCalendar) {
		if (count($totalDataBlock) > 0) {
			$filter = array_values(array_filter($totalDataBlock, function($child) use ($arrKeyProcess) {
				return $child['N_KoteiNo'] == $arrKeyProcess['KoteiNo'];
			}));
			if (count($filter) > 0) {
				// 一致するデータが存在した場合
				$newArrKeyProcess_1 = array();
				$newArrKeyProcess_1['WorkItemID'] = $filter[0]['WorkItemID'];	// $前工程のWorkItemID
				$newArrKeyProcess_1['N_Link'] = $filter[0]['N_Link'];			// $前工程のN_Link
				$newArrKeyProcess_1['KoteiNo'] = $filter[0]['KoteiNo'];			// $キー工程No
				$newArrKeyProcess_1['Days'] = $filter[0]['Days'];				// $変更前の工期
				$newArrKeyProcess_1['TableType'] = $filter[0]['TableType'];		// TableType

				// 「$前工程の工期」を取得する。
				$dataGetKoteiRange = $arrTimeTracker[0]->getKoteiRange(
					array($newArrKeyProcess_1['WorkItemID']),
					true,
					$dataGetCalendar
				);
				if ($dataGetKoteiRange != '' && is_string($dataGetKoteiRange)) {
					return $dataGetKoteiRange;
				}

				// $前工程の工期
				$workDays = (isset($dataGetKoteiRange[$newArrKeyProcess_1['WorkItemID']]['workDays'])) ?
									$dataGetKoteiRange[$newArrKeyProcess_1['WorkItemID']]['workDays'] : 0;

				// 「$前工程の完工日」を算出する。
				$completeDatePreviousProcess = $arrTimeTracker[0]->shiftDate(
					$planDate,
					($newArrKeyProcess_1['N_Link'] * -1),
					$dataGetCalendar
				);	// $前工程の完工日
				if (is_string($completeDatePreviousProcess) && $completeDatePreviousProcess == '') {
					return $completeDatePreviousProcess;
				}

				// 「$前工程の着工日」を算出する。
				$startDatePreviousProcess = $arrTimeTracker[0]->shiftDate(
					$completeDatePreviousProcess,
					($workDays * -1),
					$dataGetCalendar
				);	// 前工程の着工日
				if (is_string($startDatePreviousProcess) && $startDatePreviousProcess == '') {
					return $startDatePreviousProcess;
				}

				// 戻り値をメモリ「$キー着工日」する。
				$planDate = $startDatePreviousProcess;

				// TimeTrackerNXのデータ更新を行う。
				// 「③」、「④」、「⑤」で取得した値を元にTimeTrackerNXを更新する。
				$dataInsertPlan = $arrTimeTracker[1]->insertPlan(
					$validatedData['val2'],
					$validatedData['val3'],
					null,
					$newArrKeyProcess_1['WorkItemID'],
					$startDatePreviousProcess,
					$completeDatePreviousProcess,
					null,
					$dataGetCalendar
				);

				if ($dataInsertPlan != '' && is_string($dataInsertPlan)) {
					return $dataInsertPlan;
				}

				// 既存データ更新を行う。
				if ($newArrKeyProcess_1['Days'] != $workDays) {
					$objUpdate['Days'] = $workDays;
					if ($newArrKeyProcess_1['TableType'] == 0) {
						// 「$TableType」が「0」の場合
						$result = Cyn_Plan::query()
											->where('WorkItemID', '=', $newArrKeyProcess_1['WorkItemID'])
											->update($objUpdate);
					}
					if ($newArrKeyProcess_1['TableType'] == 1) {
						// 「$TableType」が「1」の場合
						$result = Cyn_C_Plan::query()
											->where('WorkItemID', '=', $newArrKeyProcess_1['WorkItemID'])
											->update($objUpdate);
					}
				}
				$next = $this->processRecursivePrevious($totalDataBlock, $newArrKeyProcess_1, $planDate, $arrTimeTracker, $validatedData, $dataGetCalendar);
				if (is_string($next)) {
					return $next;
				}
			}
		} else {
			return true;
		}
	}

	/**
	* Process next Kotei recursive
	*
	* @param Array $totalDataBlock
	* @param Array $arrKeyProcess
	* @param String $planDate
	* @param String $nextLink
	* @param Array $arrTimeTracker
	* @param Array $validatedData
	* @param Array $dataGetCalendar
	* @return Mixed boolean || string
	*
	* @create 2020/12/10 Chien
	* @update
	*/
	private function processRecursiveNext($totalDataBlock, $arrKeyProcess, &$planDate, &$nextLink, $arrTimeTracker = array(), $validatedData, $dataGetCalendar) {
		if (count($totalDataBlock) > 0) {
			$filter = array_values(array_filter($totalDataBlock, function($child) use ($arrKeyProcess) {
				return $child['KoteiNo'] == $arrKeyProcess['N_KoteiNo'];
			}));
			if (count($filter) > 0) {
				// 後工程の着工日、完工日を再計算する。
				$newArrKeyProcess_2 = array();
				$newArrKeyProcess_2['WorkItemID'] = $filter[0]['WorkItemID'];	// $後工程のWorkItemID
				$newArrKeyProcess_2['N_KoteiNo'] = $filter[0]['N_KoteiNo'];		// $キー次工程No
				$newArrKeyProcess_2['N_Link'] = $filter[0]['N_Link'];			// N_Link
				$newArrKeyProcess_2['Days'] = $filter[0]['Days'];				// $変更前の工期
				$newArrKeyProcess_2['TableType'] = $filter[0]['TableType'];		// TableType

				// 「$後工程の工期」を取得する。
				$dataGetKoteiRange = $arrTimeTracker[0]->getKoteiRange(
					array($newArrKeyProcess_2['WorkItemID']),
					true,
					$dataGetCalendar
				);
				if ($dataGetKoteiRange != '' && is_string($dataGetKoteiRange)) {
					return $dataGetKoteiRange;
				}

				// $後工程の工期
				$workDays = (isset($dataGetKoteiRange[$newArrKeyProcess_2['WorkItemID']]['workDays'])) ?
									$dataGetKoteiRange[$newArrKeyProcess_2['WorkItemID']]['workDays'] : 0;

				// 「$後工程の着工日」を算出する。
				$startDatePostProcess = $arrTimeTracker[0]->shiftDate(
					$planDate,
					$nextLink,
					$dataGetCalendar
				);	// $後工程の着工日
				if (is_string($startDatePostProcess) && $startDatePostProcess == '') {
					return $startDatePostProcess;
				}

				// 「$後工程の完工日」を算出する。
				$completeDatePostProcess = $arrTimeTracker[0]->shiftDate(
					$startDatePostProcess,
					$workDays,
					$dataGetCalendar
				);	// $後工程の完工日
				if (is_string($completeDatePostProcess) && $completeDatePostProcess == '') {
					return $completeDatePostProcess;
				}

				// 戻り値をメモリ「$キー完工日」する。
				$planDate = $completeDatePostProcess;

				// 「②」で一致したデータ「$工程情報」[N_Link]をメモリ「$キー次工程とのリンク日数」する。
				$nextLink = $newArrKeyProcess_2['N_Link'];	// $キー次工程とのリンク日数

				// TimeTrackerNXのデータ更新を行う。
				// 「③」、「④」、「⑤」で取得した値を元にTimeTrackerNXを更新する。
				$dataInsertPlan = $arrTimeTracker[1]->insertPlan(
					$validatedData['val2'],
					$validatedData['val3'],
					null,
					$newArrKeyProcess_2['WorkItemID'],
					$startDatePostProcess,
					$completeDatePostProcess,
					null,
					$dataGetCalendar
				);

				if ($dataInsertPlan != '' && is_string($dataInsertPlan)) {
					return $dataInsertPlan;
				}

				// 既存データ更新を行う。
				if ($newArrKeyProcess_2['Days'] != $workDays) {
					$objUpdate['Days'] = $workDays;
					if ($newArrKeyProcess_2['TableType'] == 0) {
						// 「$TableType」が「0」の場合
						$result = Cyn_Plan::query()
											->where('WorkItemID', '=', $newArrKeyProcess_2['WorkItemID'])
											->update($objUpdate);
					}
					if ($newArrKeyProcess_2['TableType'] == 1) {
						// 「$TableType」が「1」の場合
						$result = Cyn_C_Plan::query()
											->where('WorkItemID', '=', $newArrKeyProcess_2['WorkItemID'])
											->update($objUpdate);
					}
				}

				$next = $this->processRecursiveNext($totalDataBlock, $newArrKeyProcess_2, $planDate, $nextLink, $arrTimeTracker, $validatedData, $dataGetCalendar);
				if (is_string($next)) {
					return $next;
				}
			}
		} else {
			return true;
		}
	}
}
