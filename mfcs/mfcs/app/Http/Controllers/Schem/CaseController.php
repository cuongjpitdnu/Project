<?php
/*
 * @CaseController.php
 * 検討ケース作成画面コントローラーファイル
 *
 * @create 2020/11/20 Chien
 *
 * @update 2021/01/08 Chien
 */

namespace App\Http\Controllers\Schem;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Http\Requests\Schem\CaseCreateRequest;
use App\Http\Requests\Schem\CaseCopyRequest;
use App\Http\Requests\Schem\CaseDeleteRequest;
use Illuminate\Support\Facades\DB;
use Illuminate\Database\QueryException;
use Illuminate\Pagination\LengthAwarePaginator;
use App\Librarys\FuncCommon;
use App\Librarys\MenuInfo;
use App\Librarys\TimeTrackerCommon;
use App\Librarys\TimeTrackerFuncSchem;
use App\Librarys\CustomException;
use App\Models\MstProject;
use App\Models\MstOrderNo;
use App\Models\Cyn_TosaiData;
use App\Models\Cyn_BlockKukaku;
use App\Models\Cyn_C_BlockKukaku;
use App\Models\Cyn_Plan;
use App\Models\Cyn_C_Plan;
use App\Models\Cyn_History_Copy;
use App\Models\Cyn_LogData_Copy;
use Exception;

/*
 * 検討ケース作成画面コントローラー
 *
 * @create 2020/11/20 Chien
 *
 * @update
 */
class CaseController extends Controller
{
	/**
	 * 検討ケース作成画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/11/20 Chien
	 * @update
	 */
	public function index(Request $request) {
		return $this->initialize($request);
	}

	/**
	 * init & prepare data to show 検討ケース作成画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/11/20 Chien
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

		//request
		$this->data['request'] = $request;
		//return view with all data
		return view('Schem/case/index', $this->data);
	}

	/**
	 * 新規ケース登録画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/11/20 Chien
	 * @update
	 */
	public function create(Request $request) {
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$this->data['menuInfo'] = $menuInfo;

		//initialize $originalError
		$originalError = [];
		$data = [];
		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
			$data['ListKind'] = valueUrlDecode($request->val101);
			$data['ProjectName'] = valueUrlDecode($request->val102);
		} else {
			$data['ListKind'] = config('system_const.c_kind_chijyo');
			$data['ProjectName'] = '';
		}

		$this->data['originalError'] = $originalError;
		$this->data['request'] = $request;
		$this->data['itemData'] = $data;
		//return view with all data
		return view('Schem/Case/create', $this->data);
	}

	/**
	 * process save action screen 030902
	 *
	 * @param CaseCreateRequest 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/12/11 Chien
	 * @update
	 */
	public function newsave(CaseCreateRequest $request) {
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		//validate form
		$validated = $request->validated();

		// init url
		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/create';
		$url .= '?cmn1=' . $request->cmn1;
		$url .= '&cmn2=' . $request->cmn2;
		$url .= '&val101=' . valueUrlEncode($request->val101);
		$url .= '&val102=' . valueUrlEncode($request->val102);

		// 重複確認を行う。
		$checkDUplicate = MstProject::where('SysKindID', '=', $menuInfo->KindID)
									->where('ListKind', '=', $validated['val101'])
									->where('ProjectName', '=', $validated['val102'])
									->first();
		// レコードが存在した場合
		if ($checkDUplicate != null) {
			$url .= '&err1=' . valueUrlEncode(config('message.msg_cmn_db_015'));
			//redirect to $url
			return redirect($url);
		}

		//beginTransaction
		try {
			DB::transaction(function () use ($validated, $menuInfo) {
				// 検討ケースを[mstProject]に登録する。
				$data = MstProject::selectRaw('MAX(SerialNo) as MaxSerialNo')
									->where('SysKindID', '=', $menuInfo->KindID)
									->where('ListKind', '=', $validated['val101'])
									->first();
				$serialNo = 1;
				if ($data != null) {
					$serialNo = $data->MaxSerialNo + 1;
				}

				// [mstProject]のデータ登録を行う。
				$seq_mstProject = DB::select('SELECT NEXT VALUE FOR seq_mstProject as projectID');
				$projectID = $seq_mstProject[0]->projectID;

				$objMstProject = new MstProject();
				$objMstProject->Up_User = $menuInfo->UserID;
				$objMstProject->ID = $projectID;
				$objMstProject->SysKindID = $menuInfo->KindID;
				$objMstProject->ListKind = $validated['val101'];
				$objMstProject->SerialNo = $serialNo;
				$objMstProject->ProjectName = $validated['val102'];
				$objMstProject->save();

				// 検討ケースをTimeTrackerNXに登録する。
				$objTimeTrackerFuncSchem = new TimeTrackerFuncSchem();
				$dataAddCase = $objTimeTrackerFuncSchem->addCase(
					$validated['val102'],
					$menuInfo->KindID.'-'.$validated['val101'].'-'.$serialNo,
					$validated['val101']
				);
				if ($dataAddCase != null) {
					throw new CustomException($dataAddCase);
				}
			});
		} catch (CustomException $ex) {
			// error
			$url .= '&err1=' . valueUrlEncode($ex->getMessage());
			return redirect($url);
		}

		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/index';
		$url .= '?cmn1=' . $request->cmn1;
		$url .= '&cmn2=' . $request->cmn2;

		//everything is ok
		return redirect($url);
	}

	/**
	 * コピー画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/12/15 Chien
	 * @update
	 */
	public function copy(Request $request) {
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
			'val201' => isset($request->val201) ? valueUrlDecode($request->val201) :
						((trim(old('val201')) != '') ? valueUrlDecode(old('val201')) : config('system_const.c_kind_chijyo')),
			'val202' => isset($request->val202) ? valueUrlDecode($request->val202) :
						((trim(old('val202')) != '') ? valueUrlDecode(old('val202')) : 0),
			'val203' => isset($request->val203) ? valueUrlDecode($request->val203) :
						((trim(old('val203')) != '') ? valueUrlDecode(old('val203')) : ''),
			'val204' => isset($request->val204) ? valueUrlDecode($request->val204) :
						((trim(old('val204')) != '') ? valueUrlDecode(old('val204')) : ''),
			'val205' => isset($request->val205) ? valueUrlDecode($request->val205) :
						((trim(old('val205')) != '') ? valueUrlDecode(old('val205')) : ''),
			'val206' => isset($request->val206) ? valueUrlDecode($request->val206) :
						((trim(old('val206')) != '') ? valueUrlDecode(old('val206')) : 0),
		);

		$dataSelect = $this->getInitData($menuInfo->KindID, $itemShow['val201'], $itemShow['val202']);
		$dataSelectAll = $this->getInitData($menuInfo->KindID, '', '', true);
		if (count($dataSelect['val202']) > 0) {
			$arrUnique = array();
			foreach ($dataSelect['val202'] as $key => &$itemVal202) {
				if (count($arrUnique) == 0) {
					$arrUnique[] = $itemVal202->ProjectName;
				} else {
					if (!in_array($itemVal202->ProjectName, $arrUnique)) {
						$arrUnique[] = $itemVal202->ProjectName;
					} else {
						unset($dataSelect['val202'][$key]);
					}
				}
			}
		}
		if (count($dataSelect['val203']) > 0) {
			$arrUnique = array();
			foreach ($dataSelect['val203'] as $key => &$itemVal203) {
				if (count($arrUnique) == 0) {
					$arrUnique[] = $itemVal203->NameShow;
				} else {
					if (!in_array($itemVal203->NameShow, $arrUnique)) {
						$arrUnique[] = $itemVal203->NameShow;
					} else {
						unset($dataSelect['val203'][$key]);
					}
				}
			}
		}
		if (count($dataSelect['val204']) > 0) {
			$arrUnique = array();
			foreach ($dataSelect['val204'] as $key => &$itemVal204) {
				if (count($arrUnique) == 0) {
					$arrUnique[] = $itemVal204->ProjectName;
				} else {
					if (!in_array($itemVal204->ProjectName, $arrUnique)) {
						$arrUnique[] = $itemVal204->ProjectName;
					} else {
						unset($dataSelect['val204'][$key]);
					}
				}
			}
		}
		if (count($dataSelect['val205']) > 0) {
			$arrUnique = array();
			foreach ($dataSelect['val205'] as $key => &$itemVal205) {
				if (count($arrUnique) == 0) {
					$arrUnique[] = $itemVal205->NameShow;
				} else {
					if (!in_array($itemVal205->NameShow, $arrUnique)) {
						$arrUnique[] = $itemVal205->NameShow;
					} else {
						unset($dataSelect['val205'][$key]);
					}
				}
			}
		}

		$this->data['dataSelect'] = array(
			'val202' => $dataSelect['val202'],
			'val203' => $dataSelect['val203'],
			'val204' => $dataSelect['val204'],
			'val205' => $dataSelect['val205'],
			'val202LoadAll' => $dataSelectAll['val202'],
			'val203LoadAll' => $dataSelectAll['val203'],
			'val204LoadAll' => $dataSelectAll['val204'],
			'val205LoadAll' => $dataSelectAll['val205'],
		);

		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
		}
		$itemShow['val202'] = $dataSelect['projectSource'];
		$itemShow['val201'] = valueUrlEncode($itemShow['val201']);
		$itemShow['val202'] = valueUrlEncode($itemShow['val202']);
		$itemShow['val203'] = valueUrlEncode($itemShow['val203']);
		$itemShow['val204'] = valueUrlEncode($itemShow['val204']);
		$itemShow['val205'] = valueUrlEncode($itemShow['val205']);

		$this->data['originalError'] = $originalError;
		$this->data['request'] = $request;
		$this->data['itemData'] = $itemShow;
		//return view with all data
		return view('Schem/Case/copy', $this->data);
	}

	/**
	 * init & prepare data to show combobox on screen
	 *
	 * @param String $kindID
	 * @param String $cKind
	 * @param String $projectID_source
	 * @param Bool $loadAll
	 * @return Array mixed
	 *
	 * @create 2020/12/15 Chien
	 * @update
	 */
	private function getInitData($kindID = '', $cKind = '', $projectID_source = '', $loadAll = false) {
		// コピー元 ~ source
		// A2 ~ val202 + A4 ~ val204
		$dataA2A4 = MstProject::select('ID', 'ProjectName', 'ListKind')->where('SysKindID', '=', $kindID);
		$dataA2A4 = ($cKind !== '' && is_numeric($cKind)) ? $dataA2A4->where('ListKind', '=', $cKind) : $dataA2A4;
		$dataA2A4 = $dataA2A4->orderBy('ProjectName')->distinct()->get();
		if (count($dataA2A4) > 0) {
			foreach ($dataA2A4 as &$itemA2A4) {
				$itemA2A4->ID = valueUrlEncode($itemA2A4->ID);
				$itemA2A4->ListKind = valueUrlEncode($itemA2A4->ListKind);
				$itemA2A4->ProjectName = ($loadAll) ? htmlentities($itemA2A4->ProjectName) : $itemA2A4->ProjectName;
			}
		}
		// tempProjectID if load default
		$tempProjectID = (count($dataA2A4) > 0) ? valueUrlDecode($dataA2A4->first()->ID) : '';

		// A3 ~ val203
		$dataA3 = MstOrderNo::select('mstOrderNo.OrderNo', 'Cyn_TosaiData.ProjectID', 'Cyn_TosaiData.CKind')
							->join('Cyn_TosaiData', 'mstOrderNo.OrderNo', '=', 'Cyn_TosaiData.OrderNo');
		$dataA3 = ($projectID_source !== '' && is_numeric($projectID_source) && $projectID_source != 0) ?
									$dataA3->where('Cyn_TosaiData.ProjectID', '=', $projectID_source) :
									(($tempProjectID != '' && !$loadAll) ? $dataA3->where('Cyn_TosaiData.ProjectID', '=', $tempProjectID) : $dataA3);
		$dataA3 = (!$loadAll) ? (($cKind !== '' && is_numeric($cKind)) ?
									$dataA3->where('Cyn_TosaiData.CKind', '=', $cKind) : $dataA3) : $dataA3;
		$dataA3 = $dataA3->where('MstOrderNo.DispFlag', '=', 0)->orderBy('mstOrderNo.OrderNo')->distinct()->get();
		if (count($dataA3) > 0) {
			foreach ($dataA3 as &$itemA3) {
				$itemA3->NameShow = ($loadAll) ? htmlentities($itemA3->OrderNo) : $itemA3->OrderNo;
				$itemA3->OrderNo = valueUrlEncode($itemA3->OrderNo);
				$itemA3->ProjectID = valueUrlEncode($itemA3->ProjectID);
				$itemA3->CKind = valueUrlEncode($itemA3->CKind);
			}
		}

		// コピー先 ~ destination
		// A5 ~ val205
		$dataA5 = MstOrderNo::select('mstOrderNo.OrderNo', 'Cyn_TosaiData.ProjectID', 'Cyn_TosaiData.CKind')
							->join('Cyn_TosaiData', 'mstOrderNo.OrderNo', '=', 'Cyn_TosaiData.OrderNo');
		// $dataA5 = (!$loadAll) ? (($cKind !== '' && is_numeric($cKind)) ? $dataA5->where('Cyn_TosaiData.CKind', '=', $cKind) : $dataA5) : $dataA5;
		$dataA5 = $dataA5->where('MstOrderNo.DispFlag', '=', 0)->orderBy('mstOrderNo.OrderNo')->distinct()->get();
		if (count($dataA5) > 0) {
			foreach ($dataA5 as &$itemA5) {
				$itemA5->NameShow = ($loadAll) ? htmlentities($itemA5->OrderNo) : $itemA5->OrderNo;
				$itemA5->OrderNo = valueUrlEncode($itemA5->OrderNo);
				$itemA5->ProjectID = valueUrlEncode($itemA5->ProjectID);
				$itemA5->CKind = valueUrlEncode($itemA5->CKind);
			}
		}

		return array(
			'projectSource' => ($projectID_source !== 0) ? $projectID_source : $tempProjectID,
			'val202' => $dataA2A4,
			'val203' => (count($dataA2A4) > 0) ? $dataA3 : array(),
			'val204' => $dataA2A4,
			'val205' => (count($dataA2A4) > 0) ? $dataA5 : array(),
		);
	}

	/**
	 * process save action screen 030903
	 *
	 * @param CaseCopyRequest 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/12/16 Chien
	 * @update
	 */
	public function copysave(CaseCopyRequest $request) {
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		// validate form
		$validated = $request->validated();

		$url = url('/');
		$url .= '/'.$menuInfo->KindURL;
		$url .= '/'.$menuInfo->MenuURL;
		$url .= '/copy';
		$url .= '?cmn1='.(isset($request->cmn1) ? $request->cmn1 : '');
		$url .= '&cmn2='.(isset($request->cmn2) ? $request->cmn2 : '');
		//encode val201 -> val206
		for ($i = 1; $i <= 6; $i++) {
			$key = 'val20'.$i;
			$url .=  '&val20'.$i.'='.valueUrlEncode($request->$key);
		}

		// 入力値の比較
		if ($validated['val202'] == $validated['val204'] && $validated['val203'] == $validated['val205']) {
			$url .= '&err1='.valueUrlEncode(config('message.msg_schem_case_001'));
			return redirect($url);
		}

		// コピー元に紐付くTimeTrackerNXのデータを取得する。
		// コピー元検討ケースの実働期間を取得する。
		// TimeTrackerCommon【getCalendar】を実行する。
		$objTimeTrackerCommon = new TimeTrackerCommon();
		$objTimeTrackerFuncSchem = new TimeTrackerFuncSchem();

		$param_2 = ($validated['val202'] == 0) ? $validated['val203'] : null;
		$dataGetCalendar_source = $objTimeTrackerCommon->getCalendar($validated['val202'], $param_2);	// $コピー元プロジェクトカレンダー
		if ($dataGetCalendar_source != '' && is_string($dataGetCalendar_source)) {
			// error
			$url .= '&err1=' . valueUrlEncode($dataGetCalendar_source);
			return redirect($url);
		}

		//「コピー元検討ケース」、「コピー元オーダ」を元にルートオブジェクトのWorkItemIDを取得する。
		// TimeTrackerCommon【getOrderRoot】を実行する。
		// $コピー元ルートオブジェクトのWorkItemID
		$dataGetOrderRoot_source = $objTimeTrackerCommon->getOrderRoot($validated['val202'], $validated['val203']);
		if ($dataGetOrderRoot_source != '' && is_string($dataGetOrderRoot_source)) {
			// error
			$url .= '&err1=' . valueUrlEncode($dataGetOrderRoot_source);
			return redirect($url);
		}

		// ルートオブジェクト配下すべてのWorkItemIDを取得する。
		// TimeTrackerCommon【getChildWorkItem】を実行する。
		$dataGetChildWorkItem_source = $objTimeTrackerCommon->getChildWorkItem($dataGetOrderRoot_source);	// $すべてのWorkItem
		if ($dataGetChildWorkItem_source != '' && is_string($dataGetChildWorkItem_source)) {
			// error
			$url .= '&err1=' . valueUrlEncode($dataGetChildWorkItem_source);
			return redirect($url);
		}

		// コピー先に紐付くTimeTrackerNXのデータを取得する。
		// コピー先検討ケースの実働期間を取得する。
		// TimeTrackerCommon【getCalendar】を実行する。
		$dataGetCalendar_destination = $objTimeTrackerCommon->getCalendar($validated['val204'], null);	// $コピー先プロジェクトカレンダー
		if ($dataGetCalendar_destination != '' && is_string($dataGetCalendar_destination)) {
			// error
			$url .= '&err1=' . valueUrlEncode($dataGetCalendar_destination);
			return redirect($url);
		}

		//「コピー先検討ケース」、「コピー先オーダ」を元にルートオブジェクトのWorkItemIDを取得する。
		// TimeTrackerCommon【getOrderRoot】を実行する。
		// $コピー先ルートオブジェクトのWorkItemID
		$dataGetOrderRoot_destination = $objTimeTrackerCommon->getOrderRoot($validated['val204'], $validated['val205']);
		if ($dataGetOrderRoot_destination != '' && is_string($dataGetOrderRoot_destination)) {
			// error
			$url .= '&err1=' . valueUrlEncode($dataGetOrderRoot_destination);
			return redirect($url);
		}

		// コピー元、コピー先のプロジェクトカレンダーを比較する。
		// コピー元、コピー先のプロジェクトカレンダーの比較。
		// TimeTrackerCommon【checkCalendar】を実行する。
		$dataCheckCalendar = $objTimeTrackerCommon->checkCalendar($dataGetCalendar_source, $dataGetCalendar_destination);
		if ($dataCheckCalendar != null) {
			// error
			$url .= '&err1=' . valueUrlEncode('コピー元とコピー先の'.$dataCheckCalendar);
			return redirect($url);
		}

		// 履歴データの登録。
		//【Cyn_History_Copy】
		$newID = 1;	// $履歴ID
		$dataCynHistoryCopy = Cyn_History_Copy::selectRaw('MAX(ID) as MaxID')->first();
		if ($dataCynHistoryCopy != null) {
			$newID = $dataCynHistoryCopy->MaxID + 1;
		}
		$objCynHistoryCopy = new Cyn_History_Copy;
		$objCynHistoryCopy->ID = $newID;
		$objCynHistoryCopy->Import_User = $menuInfo->UserID;
		$objCynHistoryCopy->Import_Date = now();
		$objCynHistoryCopy->ProjectID_C = $validated['val202'];
		$objCynHistoryCopy->OrderNo_C = $validated['val203'];
		$objCynHistoryCopy->ProjectID_P = $validated['val204'];
		$objCynHistoryCopy->OrderNo_P = $validated['val205'];
		$objCynHistoryCopy->StatusFlag = 0;
		$objCynHistoryCopy->save();

		try {
			DB::transaction(function() use ($validated, $request, $objTimeTrackerCommon, $objTimeTrackerFuncSchem, $newID,
											$dataGetCalendar_destination, $dataGetOrderRoot_destination, $dataGetChildWorkItem_source) {
				// コピー元の搭載情報を取得する。
				//「中日程区分」、「コピー元検討ケース」、「コピー元オーダ」の選択値を元に、対象データの取得を行う。
				$checkDataCynTosai_source = Cyn_TosaiData::select('Name', 'BKumiku')->where('CKind', '=', $validated['val201']);
				$checkDataCynTosai_source = ($validated['val202'] == 0) ? $checkDataCynTosai_source->where('ProjectID', '=', 0) :
																$checkDataCynTosai_source->where('ProjectID', '=', $validated['val202']);
				$checkDataCynTosai_source = $checkDataCynTosai_source->where('OrderNo', '=', $validated['val203'])
																	->where('IsOriginal', '=', 0)->get();	// $コピー元データ

				// コピー先の搭載情報を取得する。
				// 「中日程区分」、「コピー先検討ケース」、「コピー先オーダ」の選択値を元に、対象データの取得を行う。
				$checkDataCynTosai_destination = Cyn_TosaiData::select('Name', 'BKumiku')->where('CKind', '=', $validated['val201'])
																->where('ProjectID', '=', $validated['val204'])
																->where('OrderNo', '=', $validated['val205'])
																->where('IsOriginal', '=', 0)->get();	// $コピー先データ

				// コピー元データを元にコピー先データとの比較を行う。
				// メモリ「$履歴ログID」に0を設定する。
				$historyLogID = 0;	// $履歴ログID
				// メモリ「$コピー元データ」のデータ数分、下記処理を行う。
				foreach ($checkDataCynTosai_source as $keySource => &$row_source) {
					$flagCase003 = (count($checkDataCynTosai_destination) > 0) ? false : true;
					foreach ($checkDataCynTosai_destination as $keyDes => &$row_destination) {
						//「$コピー元データ」[Name]、[BKumiku]の値と一致するデータが「$コピー先データ」に存在した場合
						if ($row_source->Name == $row_destination->Name && $row_source->BKumiku == $row_destination->BKumiku) {
							// ①	コピー元TbaleTypeを取得
							$copySourceTableType = array();
							// ※条件１
							$condition_1 = Cyn_TosaiData::selectRaw('0 as TableType')
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
														->where('Cyn_TosaiData.ProjectID', '=', $validated['val202'])
														->where('Cyn_TosaiData.OrderNo', '=', $validated['val203'])
														->where('Cyn_TosaiData.CKind', '=', $validated['val201'])
														->where('Cyn_TosaiData.Name', '=', $row_source->Name)
														->where('Cyn_TosaiData.BKumiku', '=', $row_source->BKumiku)
														->where('Cyn_TosaiData.WorkItemID', '<>', 0)
														->where('Cyn_TosaiData.IsOriginal', '=', 0)
														->whereNull('Cyn_BlockKukaku.Del_Date')
														->whereNull('Cyn_Plan.Del_Date');

							// ※条件２
							$condition_2 = DB::table('Cyn_TosaiData')
											->selectRaw('1 as TableType')
											->leftJoin('Cyn_BlockKukaku AS CynB', function($join) {
												$join->on('Cyn_TosaiData.ProjectID', '=', 'CynB.ProjectID')
													->on('Cyn_TosaiData.OrderNo', '=', 'CynB.OrderNo')
													->on('Cyn_TosaiData.CKind', '=', 'CynB.CKind')
													->on('Cyn_TosaiData.Name', '=', 'CynB.T_Name')
													->on('Cyn_TosaiData.BKumiku', '=', 'CynB.T_BKumiku');
											})
											->leftJoin('Cyn_C_BlockKukaku as CynCB', function($join) {
												$join->on('CynB.ProjectID', '=', 'CynCB.ProjectID')
													->on('CynB.OrderNo', '=', 'CynCB.OrderNo')
													->on('CynB.CKind', '=', 'CynCB.CKind')
													->on('CynB.Name', '=', 'CynCB.T_Name')
													->on('CynB.BKumiku', '=', 'CynCB.T_BKumiku');
											})
											->leftJoin('Cyn_C_Plan as CynCP', function($join) {
												$join->on('CynCB.ProjectID', '=', 'CynCP.ProjectID')
													->on('CynCB.OrderNo', '=', 'CynCP.OrderNo')
													->on('CynCB.No', '=', 'CynCP.No');
											})
											->where('CynB.ProjectID', '=', $validated['val202'])
											->where('CynB.OrderNo', '=', $validated['val203'])
											->where('CynB.CKind', '=', $validated['val201'])
											->where('CynB.Name', '=', $row_source->Name)
											->where('CynB.BKumiku', '=', $row_source->BKumiku)
											->whereNull('CynB.Del_Date')
											->whereNull('CynCB.Del_Date')
											->whereNull('CynCP.Del_Date')
											->where('Cyn_TosaiData.WorkItemID', '=', 0)
											->where('Cyn_TosaiData.IsOriginal', '=', 0);

							$copySourceTableType = $condition_1->union($condition_2)->get();

							$arrCopySourceTableType = array();	// $コピー元TableType
							if (count($copySourceTableType) > 0) {
								foreach ($copySourceTableType as $item) {
									if (!in_array($item->TableType, $arrCopySourceTableType)) {
										$arrCopySourceTableType[] = $item->TableType;
									}
								}
							}

							// ②	コピー先の削除対象WorkItemID、コピー先TbaleTypeを取得
							$copyDestinationTableType = array();
							// ※条件１
							$condition_1 = Cyn_TosaiData::select('Cyn_TosaiData.WorkItemID')
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
														->where('Cyn_TosaiData.ProjectID', '=', $validated['val204'])
														->where('Cyn_TosaiData.OrderNo', '=', $validated['val205'])
														->where('Cyn_TosaiData.CKind', '=', $validated['val201'])
														->where('Cyn_TosaiData.Name', '=', $row_source->Name)
														->where('Cyn_TosaiData.BKumiku', '=', $row_source->BKumiku)
														->where('Cyn_TosaiData.WorkItemID', '<>', 0)
														->where('Cyn_TosaiData.IsOriginal', '=', 0)
														->whereNull('Cyn_BlockKukaku.Del_Date')
														->whereNull('Cyn_Plan.Del_Date');

							// ※条件２
							$condition_2 = DB::table('Cyn_TosaiData')
											->select('CynB.WorkItemID')
											->selectRaw('1 as TableType')
											->leftJoin('Cyn_BlockKukaku as CynB', function($join) {
												$join->on('Cyn_TosaiData.ProjectID', '=', 'CynB.ProjectID')
													->on('Cyn_TosaiData.OrderNo', '=', 'CynB.OrderNo')
													->on('Cyn_TosaiData.CKind', '=', 'CynB.CKind')
													->on('Cyn_TosaiData.Name', '=', 'CynB.T_Name')
													->on('Cyn_TosaiData.BKumiku', '=', 'CynB.T_BKumiku');
											})
											->leftJoin('Cyn_C_BlockKukaku as CynCB', function($join) {
												$join->on('CynB.ProjectID', '=', 'CynCB.ProjectID')
													->on('CynB.OrderNo', '=', 'CynCB.OrderNo')
													->on('CynB.CKind', '=', 'CynCB.CKind')
													->on('CynB.Name', '=', 'CynCB.T_Name')
													->on('CynB.BKumiku', '=', 'CynCB.T_BKumiku');
											})
											->leftJoin('Cyn_C_Plan', function($join) {
												$join->on('CynCB.ProjectID', '=', 'Cyn_C_Plan.ProjectID')
													->on('CynCB.OrderNo', '=', 'Cyn_C_Plan.OrderNo')
													->on('CynCB.No', '=', 'Cyn_C_Plan.No');
											})
											->where('CynB.ProjectID', '=', $validated['val204'])
											->where('CynB.OrderNo', '=', $validated['val205'])
											->where('CynB.CKind', '=', $validated['val201'])
											->where('CynB.Name', '=', $row_source->Name)
											->where('CynB.BKumiku', '=', $row_source->BKumiku)
											->whereNull('CynB.Del_Date')
											->whereNull('CynCB.Del_Date')
											->whereNull('Cyn_C_Plan.Del_Date')
											->where('Cyn_TosaiData.WorkItemID', '=', 0)
											->where('Cyn_TosaiData.IsOriginal', '=', 0);

							$copyDestinationTableType = $condition_1->union($condition_2)->get();

							$arrDeleteWorkItemID = array();			// $削除WorkItemID
							$arrCopyDestinationTableType = array();	// $コピー先TableType
							if (count($copyDestinationTableType) > 0) {
								foreach ($copyDestinationTableType as $item) {
									if (!in_array($item->WorkItemID, $arrDeleteWorkItemID) && ($item->WorkItemID != '' || $item->WorkItemID != null)) {
										$arrDeleteWorkItemID[] = $item->WorkItemID;
									}
									if (!in_array($item->TableType, $arrCopyDestinationTableType)) {
										$arrCopyDestinationTableType[] = $item->TableType;
									}
								}
							}

							$dateNow = DB::selectOne('SELECT CONVERT(DATE, getdate()) AS sysdate')->sysdate;
							$dateNow = str_replace('-', '/', $dateNow);
							$objDelDate['Del_Date'] = $dateNow;
							// ③	検討ケースに紐付く工程情報を更新する。
							// 工程情報を更新する。
							// 下記条件に該当するレコードを更新（論理削除）する。
							$updateCynPlan = Cyn_Plan::join('Cyn_BlockKukaku', function($join) {
															$join->on('Cyn_BlockKukaku.ProjectID', '=', 'Cyn_Plan.ProjectID')
																->on('Cyn_BlockKukaku.OrderNo', '=', 'Cyn_Plan.OrderNo')
																->on('Cyn_BlockKukaku.No', '=', 'Cyn_Plan.No');
														})
														->join('Cyn_TosaiData', function($join) {
															$join->on('Cyn_TosaiData.ProjectID', '=', 'Cyn_BlockKukaku.ProjectID')
																->on('Cyn_TosaiData.OrderNo', '=', 'Cyn_BlockKukaku.OrderNo')
																->on('Cyn_TosaiData.CKind', '=', 'Cyn_BlockKukaku.CKind')
																->on('Cyn_TosaiData.Name', '=', 'Cyn_BlockKukaku.T_Name')
																->on('Cyn_TosaiData.BKumiku', '=', 'Cyn_BlockKukaku.T_BKumiku');
														})
														->where('Cyn_BlockKukaku.ProjectID', '=', $validated['val204'])
														->where('Cyn_BlockKukaku.OrderNo', '=', $validated['val205'])
														->where('Cyn_BlockKukaku.T_Name', '=', $row_source->Name)
														->where('Cyn_BlockKukaku.T_BKumiku', '=', $row_source->BKumiku)
														->whereNull('Cyn_Plan.Del_Date')
														->where('Cyn_TosaiData.IsOriginal', '=', 0)
														->update($objDelDate);

							// 工程情報(中日程を親とした中日程レベルデータ)を更新する。
							// 下記条件に該当するレコードを更新（論理削除）する。
							$updateCynCPlan = Cyn_C_Plan::join('Cyn_C_BlockKukaku', function($join) {
															$join->on('Cyn_C_BlockKukaku.ProjectID', '=', 'Cyn_C_Plan.ProjectID')
																->on('Cyn_C_BlockKukaku.OrderNo', '=', 'Cyn_C_Plan.OrderNo')
																->on('Cyn_C_BlockKukaku.No', '=', 'Cyn_C_Plan.No');
														})
														->join('Cyn_TosaiData', function($join) {
															$join->on('Cyn_TosaiData.ProjectID', '=', 'Cyn_C_BlockKukaku.ProjectID')
																->on('Cyn_TosaiData.OrderNo', '=', 'Cyn_C_BlockKukaku.OrderNo')
																->on('Cyn_TosaiData.CKind', '=', 'Cyn_C_BlockKukaku.CKind')
																->on('Cyn_TosaiData.Name', '=', 'Cyn_C_BlockKukaku.T_Name')
																->on('Cyn_TosaiData.BKumiku', '=', 'Cyn_C_BlockKukaku.T_BKumiku');
														})
														->where('Cyn_C_BlockKukaku.ProjectID', '=', $validated['val204'])
														->where('Cyn_C_BlockKukaku.OrderNo', '=', $validated['val205'])
														->where('Cyn_C_BlockKukaku.T_Name', '=', $row_source->Name)
														->where('Cyn_C_BlockKukaku.T_BKumiku', '=', $row_source->BKumiku)
														->whereNull('Cyn_C_Plan.Del_Date')
														->where('Cyn_TosaiData.IsOriginal', '=', 0)
														->update($objDelDate);

							// ④	検討ケースに紐付くブロック/区画情報を更新する。
							// ブロック/区画情報を更新する。
							// 下記条件に該当するレコードを更新（論理削除）する。
							$updateCynBlockKukaku = Cyn_BlockKukaku::join('Cyn_TosaiData', function($join) {
																		$join->on('Cyn_TosaiData.ProjectID', '=', 'Cyn_BlockKukaku.ProjectID')
																			->on('Cyn_TosaiData.OrderNo', '=', 'Cyn_BlockKukaku.OrderNo')
																			->on('Cyn_TosaiData.CKind', '=', 'Cyn_BlockKukaku.CKind')
																			->on('Cyn_TosaiData.Name', '=', 'Cyn_BlockKukaku.T_Name')
																			->on('Cyn_TosaiData.BKumiku', '=', 'Cyn_BlockKukaku.T_BKumiku');
																	})
																	->where('Cyn_BlockKukaku.ProjectID', '=', $validated['val204'])
																	->where('Cyn_BlockKukaku.OrderNo', '=', $validated['val205'])
																	->where('Cyn_BlockKukaku.T_Name', '=', $row_source->Name)
																	->where('Cyn_BlockKukaku.T_BKumiku', '=', $row_source->BKumiku)
																	->whereNull('Cyn_BlockKukaku.Del_Date')
																	->where('Cyn_TosaiData.IsOriginal', '=', 0)
																	->update($objDelDate);

							// ブロック/区画情報(中日程を親とした中日程レベルデータ)を更新する。
							// 下記条件に該当するレコードを更新（論理削除）する。
							$updateCynCBlockKukaku = Cyn_C_BlockKukaku::join('Cyn_TosaiData', function($join) {
																			$join->on('Cyn_TosaiData.ProjectID', '=', 'Cyn_C_BlockKukaku.ProjectID')
																				->on('Cyn_TosaiData.OrderNo', '=', 'Cyn_C_BlockKukaku.OrderNo')
																				->on('Cyn_TosaiData.CKind', '=', 'Cyn_C_BlockKukaku.CKind')
																				->on('Cyn_TosaiData.Name', '=', 'Cyn_C_BlockKukaku.T_Name')
																				->on('Cyn_TosaiData.BKumiku', '=', 'Cyn_C_BlockKukaku.T_BKumiku');
																		})
																		->where('Cyn_C_BlockKukaku.ProjectID', '=', $validated['val204'])
																		->where('Cyn_C_BlockKukaku.OrderNo', '=', $validated['val205'])
																		->where('Cyn_C_BlockKukaku.T_Name', '=', $row_source->Name)
																		->where('Cyn_C_BlockKukaku.T_BKumiku', '=', $row_source->BKumiku)
																		->whereNull('Cyn_C_BlockKukaku.Del_Date')
																		->where('Cyn_TosaiData.IsOriginal', '=', 0)
																		->update($objDelDate);

							// ⑤	検討ケースに紐付く搭載情報を削除する。
							// 搭載情報を削除する。
							// 下記条件に該当するレコードを削除する。
							$updateCynTosaiData = Cyn_TosaiData::where('ProjectID', '=', $validated['val204'])
																->where('OrderNo', '=', $validated['val205'])
																->where('Name', '=', $row_source->Name)
																->where('BKumiku', '=', $row_source->BKumiku)
																->where('IsOriginal', '=', 0)
																->delete();

							// ⑥	TimeTrackerNXのデータを削除する。
							// TimeTrackerCommon【deleteItem】を実行する。
							$timeTrackerDeleteItem = $objTimeTrackerCommon->deleteItem($arrDeleteWorkItemID);
							if (!is_null($timeTrackerDeleteItem)) {
								throw new CustomException($timeTrackerDeleteItem);
							}

							// ⑦	コピーする対象のWorkItemIDを取得する。
							// コピー対象データのWorkItemIDの取得を行う。
							// 下記条件１，２で、データを取得、結果を結合（UNION）する。
							// ※条件１
							$condition_1 = Cyn_TosaiData::select(
															'Cyn_TosaiData.WorkItemID as T_WorkItemID',
															'Cyn_BlockKukaku.WorkItemID as B_WorkItemID',
															'Cyn_BlockKukaku.BKumiku',
															'Cyn_Plan.WorkItemID as P_WorkItemID',
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
														->where('Cyn_TosaiData.ProjectID', '=', $validated['val202'])
														->where('Cyn_TosaiData.OrderNo', '=', $validated['val203'])
														->where('Cyn_TosaiData.CKind', '=', $validated['val201'])
														->where('Cyn_TosaiData.Name', '=', $row_source->Name)
														->where('Cyn_TosaiData.BKumiku', '=', $row_source->BKumiku)
														->where('Cyn_TosaiData.WorkItemID', '<>', 0)
														->where('Cyn_TosaiData.IsOriginal', '=', 0);

							// ※条件２
							$condition_2 = DB::table('Cyn_TosaiData')
											->select(
												'CynB.WorkItemID as T_WorkItemID',
												'CynCB.WorkItemID as B_WorkItemID',
												'CynCB.BKumiku',
												'CynCP.WorkItemID as P_WorkItemID',
											)
											->leftJoin('Cyn_BlockKukaku as CynB', function($join) {
												$join->on('Cyn_TosaiData.ProjectID', '=', 'CynB.ProjectID')
													->on('Cyn_TosaiData.OrderNo', '=', 'CynB.OrderNo')
													->on('Cyn_TosaiData.CKind', '=', 'CynB.CKind')
													->on('Cyn_TosaiData.Name', '=', 'CynB.T_Name')
													->on('Cyn_TosaiData.BKumiku', '=', 'CynB.T_BKumiku');
											})
											->leftJoin('Cyn_C_BlockKukaku as CynCB', function($join) {
												$join->on('CynB.ProjectID', '=', 'CynCB.ProjectID')
													->on('CynB.OrderNo', '=', 'CynCB.OrderNo')
													->on('CynB.CKind', '=', 'CynCB.CKind')
													->on('CynB.Name', '=', 'CynCB.T_Name')
													->on('CynB.BKumiku', '=', 'CynCB.T_BKumiku');
											})
											->leftJoin('Cyn_C_Plan as CynCP', function($join) {
												$join->on('CynCB.ProjectID', '=', 'CynCP.ProjectID')
													->on('CynCB.OrderNo', '=', 'CynCP.OrderNo')
													->on('CynCB.No', '=', 'CynCP.No');
											})
											->where('CynB.ProjectID', '=', $validated['val202'])
											->where('CynB.OrderNo', '=', $validated['val203'])
											->where('CynB.CKind', '=', $validated['val201'])
											->where('CynB.Name', '=', $row_source->Name)
											->where('CynB.BKumiku', '=', $row_source->BKumiku)
											->whereNull('CynB.Del_Date')
											->whereNull('CynCB.Del_Date')
											->whereNull('CynCP.Del_Date')
											->where('Cyn_TosaiData.WorkItemID', '=', 0)
											->where('Cyn_TosaiData.IsOriginal', '=', 0);
							$dataUnion = $condition_1->union($condition_2)->get();

							$lstT_WorkItemID = array();	// $T_WorkItemIDリスト
							$lstB_WorkItemID = array();	// $B_WorkItemIDリスト
							$lstP_WorkItemID = array();	// $P_WorkItemIDリスト
							if (count($dataUnion) > 0) {
								foreach ($dataUnion as $itemUnion) {
									if ($itemUnion->T_WorkItemID != '') {
										// 取得した[T_WorkItemID]をキーに、連想配列でメモリ「$T_WorkItemIDリスト」する。
										if (!isset($lstT_WorkItemID[$itemUnion->T_WorkItemID])) {
											$lstT_WorkItemID[$itemUnion->T_WorkItemID] = array(
												'WorkItemID' => $itemUnion->T_WorkItemID,
												'NewWorkItemID' => null,
											);
										}
										if ($itemUnion->B_WorkItemID != '') {
											// 取得した[T_WorkItemID]、をキーに、連想配列でメモリ「$B_WorkItemIDリスト」する。
											if (!isset($lstB_WorkItemID[$itemUnion->T_WorkItemID])) {
												$lstB_WorkItemID[$itemUnion->T_WorkItemID][] = array(
													'WorkItemID' => $itemUnion->B_WorkItemID,
													'NewWorkItemID' => null,
													'ParentWorkItemID' => $itemUnion->T_WorkItemID,
													'BKumiku' => $itemUnion->BKumiku,
												);
											} else {
												foreach ($lstB_WorkItemID[$itemUnion->T_WorkItemID] as $itemRow) {
													if (!($itemRow['WorkItemID'] == $itemUnion->B_WorkItemID &&
														$itemRow['ParentWorkItemID'] == $itemUnion->T_WorkItemID)) {
															$lstB_WorkItemID[$itemUnion->T_WorkItemID][] = array(
																'WorkItemID' => $itemUnion->B_WorkItemID,
																'NewWorkItemID' => null,
																'ParentWorkItemID' => $itemUnion->T_WorkItemID,
																'BKumiku' => $itemUnion->BKumiku,
															);
															break;
													}
												}
											}

											if ($itemUnion->P_WorkItemID != '') {
												// 取得した[T_WorkItemID]、[B_WorkItemID]をキーに、連想配列でメモリ「$P_WorkItemIDリスト」する。
												if (!isset($lstP_WorkItemID[$itemUnion->T_WorkItemID.'_'.$itemUnion->B_WorkItemID])) {
													$lstP_WorkItemID[$itemUnion->T_WorkItemID.'_'.$itemUnion->B_WorkItemID][] = array(
														'WorkItemID' => $itemUnion->P_WorkItemID,
														'NewWorkItemID' => null,
														'ParentWorkItemID' => $itemUnion->B_WorkItemID,
													);
												} else {
													foreach ($lstP_WorkItemID[$itemUnion->T_WorkItemID.'_'.$itemUnion->B_WorkItemID] as $itemRow) {
														if (!($itemRow['WorkItemID'] == $itemUnion->P_WorkItemID &&
															$itemRow['ParentWorkItemID'] == $itemUnion->B_WorkItemID)) {
															$lstP_WorkItemID[$itemUnion->T_WorkItemID.'_'.$itemUnion->B_WorkItemID][] = array(
																'WorkItemID' => $itemUnion->P_WorkItemID,
																'NewWorkItemID' => null,
																'ParentWorkItemID' => $itemUnion->B_WorkItemID,
															);
															break;
														}
													}
												}
											}
										}
									}
								}

								// ⑧	TimeTrackerNXにデータを登録する。
								//「$T_WorkItemIDリスト」数分、以下の処理を行う。
								//・「$T_WorkItemIDリスト」[WorkItemID]と「$すべてのWorkItem」[Id]が一致するデータを取得する。
								if (count($dataGetChildWorkItem_source) > 0 && count($lstT_WorkItemID) > 0) {
									foreach ($lstT_WorkItemID as &$itemTWIID) {
										if (!isset($dataGetChildWorkItem_source[$itemTWIID['WorkItemID']])) {
											// (ア)	一致するデータが存在しない場合
											throw new CustomException(config('message.msg_cmn_db_001'));
										}

										// (イ)	一致するデータが存在した場合
										// a.	手番シフト後の着工日、完工日を算出（「手番シフト」に「0」以外が入力されている場合のみ）
										// ・シフト後の着工日算出
										// TimeTrackerCommon【shiftDate】を実行する。
										if($validated['val206'] != 0) {
											$dataShiftDateSDate = $objTimeTrackerCommon->shiftDate(
												$dataGetChildWorkItem_source[$itemTWIID['WorkItemID']]['plannedStartDate'],
												$validated['val206'],
												$dataGetCalendar_destination
											);
											if(is_string($dataShiftDateSDate) && $dataShiftDateSDate == '') {
												throw new CustomException($dataShiftDateSDate);
											}
											$dataGetChildWorkItem_source[$itemTWIID['WorkItemID']]['plannedStartDate'] = $dataShiftDateSDate;
										}

										// ・シフト後の完工日算出
										// TimeTrackerCommon【shiftDate】を実行する。
										if($validated['val206'] != 0) {
											$dataShiftDateEDate = $objTimeTrackerCommon->shiftDate(
												$dataGetChildWorkItem_source[$itemTWIID['WorkItemID']]['plannedFinishDate'],
												$validated['val206'],
												$dataGetCalendar_destination
											);
											if(is_string($dataShiftDateEDate) && $dataShiftDateEDate == '') {
												throw new CustomException($dataShiftDateEDate);
											}
											$dataGetChildWorkItem_source[$itemTWIID['WorkItemID']]['plannedFinishDate'] = $dataShiftDateEDate;
										}

										// コピー先にデータを追加する。
										// TimeTrackerFuncSchem【insertTosai】を実行する。
										$dataInsertTosai = $objTimeTrackerFuncSchem->insertTosai(
											$validated['val204'],
											$validated['val205'],
											array(
												"parent" => $dataGetOrderRoot_destination,
												"itemTypeId" => $dataGetChildWorkItem_source[$itemTWIID['WorkItemID']]['itemTypeId'],
												"statusTypeId" => $dataGetChildWorkItem_source[$itemTWIID['WorkItemID']]['statusTypeId'],
												"name" => $dataGetChildWorkItem_source[$itemTWIID['WorkItemID']]['name'],
												"plannedStartDate" => $dataGetChildWorkItem_source[$itemTWIID['WorkItemID']]['plannedStartDate'],
												"plannedFinishDate" => $dataGetChildWorkItem_source[$itemTWIID['WorkItemID']]['plannedFinishDate'],
												"bKumiku" => $dataGetChildWorkItem_source[$itemTWIID['WorkItemID']]['c_BKumiku']
											),
											$dataGetCalendar_destination
										);

										// 戻り値が配列以外の場合
										// 処理を中断し、エラーメッセージ「msg_cmn_db_016 (config\message.php)」を表示する。
										if (!is_array($dataInsertTosai)) {
											throw new CustomException(config('message.msg_cmn_db_016'));
										}
										$itemTWIID['NewWorkItemID'] = $dataInsertTosai['id'];

										if (count($lstB_WorkItemID) > 0 && isset($lstB_WorkItemID[$itemTWIID['WorkItemID']])) {
											foreach ($lstB_WorkItemID[$itemTWIID['WorkItemID']] as &$itemBWIID) {
												// b.	一致するデータが存在した場合
												if (!isset($dataGetChildWorkItem_source[$itemBWIID['WorkItemID']])) {
													// a.	一致するデータが存在しない場合
													throw new CustomException(config('message.msg_cmn_db_001'));
												}
												//「$B_WorkItemIDリスト」[ParentWorkItemID]と「$T_WorkItem」[WorkItemID]が一致するデータを取得する。
												// コピー先にデータを追加する。
												// TimeTrackerFuncSchem【insertBlock】を実行する。
												if ($itemTWIID['WorkItemID'] == $itemBWIID['ParentWorkItemID']) {
													$dataInsertBlock = $objTimeTrackerFuncSchem->insertBlock(
														$validated['val204'],
														$validated['val205'],
														$itemTWIID['NewWorkItemID'],
														null,
														$dataGetChildWorkItem_source[$itemTWIID['WorkItemID']]['name'],
														$itemBWIID['BKumiku']	// update Rev3
													);
													if ($dataInsertBlock != '' && is_string($dataInsertBlock)) {
														throw new CustomException(config('message.msg_cmn_db_016'));
													}
													// 戻り値が数値の場合
													$itemBWIID['NewWorkItemID'] = $dataInsertBlock;
												}

												if (count($lstP_WorkItemID) > 0 && isset($lstP_WorkItemID[$itemTWIID['WorkItemID'].'_'.$itemBWIID['WorkItemID']])) {
													foreach ($lstP_WorkItemID[$itemTWIID['WorkItemID'].'_'.$itemBWIID['WorkItemID']] as &$itemPWIID) {
														if (!isset($dataGetChildWorkItem_source[$itemPWIID['WorkItemID']])) {
															// i.	一致するデータが存在しない場合
															throw new CustomException(config('message.msg_cmn_db_001'));
														}

														// c.	「$T_WorkItemID」[WorkItemID]と「$B_WorkItemID」[WorkItemID]をキーに持つ
														//「$P_WorkItemIDリスト」数分、以下の処理を行う。
														//「$P_WorkItemIDリスト」[WorkItemID]と「$すべてのWorkItem」[Id]が一致するデータを取得する。
														// ii.	一致するデータが存在した場合

														//「$P_WorkItemIDリスト」[ParentWorkItemID]と「$B_WorkItem」[WorkItemID]が一致するデータを取得する。
														// コピー先にデータを追加する。
														// TimeTrackerFuncSchem【insertPlan】を実行する。
														if ($itemBWIID['WorkItemID'] == $itemPWIID['ParentWorkItemID']) {
															// 	手番シフト後の着工日、完工日を算出（「手番シフト」に「0」以外が入力されている場合のみ）
															// 手番シフトを行い、シフト後の着工日、完工日を算出する。

															// ・シフト後の着工日算出
															// TimeTrackerCommon【shiftDate】を実行する。
															if($validated['val206'] != 0) {
																$dataShiftDateSDate = $objTimeTrackerCommon->shiftDate(
																	$dataGetChildWorkItem_source[$itemPWIID['WorkItemID']]['plannedStartDate'],
																	$validated['val206'],
																	$dataGetCalendar_destination
																);
																if(is_string($dataShiftDateSDate) && $dataShiftDateSDate == '') {
																	throw new CustomException($dataShiftDateSDate);
																}
																$dataGetChildWorkItem_source[$itemPWIID['WorkItemID']]['plannedStartDate'] = $dataShiftDateSDate;
															}

															// ・シフト後の完工日算出
															// TimeTrackerCommon【shiftDate】を実行する。
															if($validated['val206'] != 0) {
																$dataShiftDateEDate = $objTimeTrackerCommon->shiftDate(
																	$dataGetChildWorkItem_source[$itemPWIID['WorkItemID']]['plannedFinishDate'],
																	$validated['val206'],
																	$dataGetCalendar_destination
																);
																if(is_string($dataShiftDateEDate) && $dataShiftDateEDate == '') {
																	throw new CustomException($dataShiftDateEDate);
																}
																$dataGetChildWorkItem_source[$itemPWIID['WorkItemID']]['plannedFinishDate'] = $dataShiftDateEDate;
															}

															$dataInsertPlan = $objTimeTrackerFuncSchem->insertPlan(
																$validated['val204'],
																$validated['val205'],
																$itemBWIID['NewWorkItemID'],
																null,
																$dataGetChildWorkItem_source[$itemPWIID['WorkItemID']]['plannedStartDate'],
																$dataGetChildWorkItem_source[$itemPWIID['WorkItemID']]['plannedFinishDate'],
																$dataGetChildWorkItem_source[$itemPWIID['WorkItemID']]['name'],
																$dataGetCalendar_destination
															);
															if ($dataInsertPlan != '' && is_string($dataInsertPlan)) {
																// 戻り値が数値以外の場合
																throw new CustomException(config('message.msg_cmn_db_016'));
															}
															// 戻り値が数値の場合
															$itemPWIID['NewWorkItemID'] = $dataInsertPlan;
														}
													}
												}
											}
										}
									}
								}
							}

							// ⑨	コピー先にデータを登録する。
							if (count($arrCopySourceTableType) > 0 && count($arrCopyDestinationTableType) > 0) {
								//「$コピー元TableType」が0、かつ「$コピー先TableType」が0の場合
								if ($arrCopySourceTableType[0] == 0 && $arrCopyDestinationTableType[0] == 0) {
									// (ア)	 [Cyn_TosaiData]から[Cyn_TosaiData]にデータをコピーする。
									//「$T_WorkItemIDリスト」数分、以下の処理を行う。
									if (count($lstT_WorkItemID) > 0) {
										// Cyn_TosaiData】の登録
										$arrCynTosaiDataInsert = array();
										foreach ($lstT_WorkItemID as $objTWI) {
											// [Cyn_TosaiData].[WorkItemID] = 「$T_WorkItemIDリスト」[WorkItemID]で取得
											$dataCynTosaiData = Cyn_TosaiData::where('WorkItemID', '=', $objTWI['WorkItemID'])->first();

											$obj = $this->setDataCynTosaiData_0_0($validated, $objTWI, $dataCynTosaiData);
											$arrCynTosaiDataInsert[] = $obj;

											// a.	[Cyn_BlockKukaku]から[Cyn_BlockKukaku]にデータをコピーする。
											//「$T_WorkItemID」[WorkItemID]をキーに持つ「$B_WorkItemIDリスト」数分、以下の処理を行う。
											$lstNextSeqNo = array();	// $シーケンスNo
											if (isset($lstB_WorkItemID[$objTWI['WorkItemID']]) && count($lstB_WorkItemID[$objTWI['WorkItemID']]) > 0) {
												//【Cyn_BlockKukaku】の登録
												$arrCynBlockKukaku = array();
												foreach ($lstB_WorkItemID[$objTWI['WorkItemID']] as $objBWI) {
													// [Cyn_BlockKukakaku].[WorkItemID] = 「$B_WorkItemIDリスト」[WorkItemID]で取得
													$dataCynBlockKukaku = Cyn_BlockKukaku::where('WorkItemID', '=', $objBWI['WorkItemID'])->first();
													$nextNo = $this->returnValueSeqProjectOrder('Cyn_BlockKukaku', $validated['val204'], $validated['val205']);
													$lstNextSeqNo[$objBWI['WorkItemID']] = $nextNo;

													$obj = $this->setDataCynBlockKukaku_0_0($validated, $objBWI, $nextNo, $dataCynBlockKukaku);
													$arrCynBlockKukaku[] = $obj;

													// i.	[Cyn_Plan]から[Cyn_Plan]にデータをコピーする。
													//「$T_WorkItemID」[WorkItemID]と「$B_WorkItemID」[WorkItemID]をキーに持つ、「$P_WorkItemIDリスト」数分、以下の処理を行う
													if (isset($lstP_WorkItemID[$objTWI['WorkItemID'].'_'.$objBWI['WorkItemID']]) &&
														count($lstP_WorkItemID[$objTWI['WorkItemID'].'_'.$objBWI['WorkItemID']]) > 0) {
														$newKoteiNo = 0;	//「$KoteiNo連番」に0を設定する。
														//【Cyn_Plan】の登録
														$arrCynPlan = array();
														foreach ($lstP_WorkItemID[$objTWI['WorkItemID'].'_'.$objBWI['WorkItemID']] as $key => $objPWI) {
															// [Cyn_Plan].[WorkItemID] = 「$P_WorkItemIDリスト」[WorkItemID]で取得
															$dataCynPlan = Cyn_Plan::where('WorkItemID', '=', $objPWI['WorkItemID'])->first();

															$newKoteiNo += 1;
															$no = (isset($lstNextSeqNo[$objPWI['ParentWorkItemID']])) ? $lstNextSeqNo[$objPWI['ParentWorkItemID']] : 0;

															$obj = $this->setDataCynPlan_0_0($validated, $objPWI, $no, $newKoteiNo, $dataCynPlan);
															$arrCynPlan[] = $obj;
														}
														$this->insertDB('Cyn_Plan', $arrCynPlan, 90);
													}
												}
												$this->insertDB('Cyn_BlockKukaku', $arrCynBlockKukaku, 77);
											}
										}
										$this->insertDB('Cyn_TosaiData', $arrCynTosaiDataInsert, 120);
									}
								}

								//「$コピー元TableType」が0、かつ「$コピー先TableType」が1の場合
								if ($arrCopySourceTableType[0] == 0 && $arrCopyDestinationTableType[0] == 1) {
									// (ア)	[Cyn_TosaiData]から[Cyn_TosaiData]、[Cyn_BlockKukaku]にデータをコピーする。
									//「$T_WorkItemIDリスト」数分、以下の処理を行う。
									$lstNextSeqNo = array();	// $シーケンスNo
									if (count($lstT_WorkItemID) > 0) {
										$arrCynTosaiDataInsert = array();
										$arrCynBlockKukaku = array();
										foreach ($lstT_WorkItemID as $objTWI) {
											// [Cyn_TosaiData].[WorkItemID] = 「$T_WorkItemIDリスト」[WorkItemID]で取得
											$dataCynTosaiData = Cyn_TosaiData::where('WorkItemID', '=', $objTWI['WorkItemID'])->first();
											$nextNo = $this->returnValueSeqProjectOrder('Cyn_BlockKukaku', $validated['val204'], $validated['val205']);
											$lstNextSeqNo[$objTWI['WorkItemID']] = $nextNo;

											$objCynTosai = $this->setDataCynTosaiData_0_1($validated, $dataCynTosaiData);
											$arrCynTosaiDataInsert[] = $objCynTosai;

											$objCynBlock = $this->setDataCynBlockKukaku_0_1($validated, $objTWI, $nextNo, $dataCynTosaiData);
											$arrCynBlockKukaku[] = $objCynBlock;

											// a.	[Cyn_BlockKukaku]から[Cyn_C_BlockKukaku]にデータをコピーする。
											// 「$T_WorkItemID」[WorkItemID]をキーに持つ「$B_WorkItemIDリスト」数分、以下の処理を行う。
											$lstNextSeqCNo = array();	// $シーケンスCNo
											if (isset($lstB_WorkItemID[$objTWI['WorkItemID']]) && count($lstB_WorkItemID[$objTWI['WorkItemID']]) > 0) {
												//【Cyn_C_BlockKukaku】の登録
												$arrCynCBlockKukaku = array();
												foreach ($lstB_WorkItemID[$objTWI['WorkItemID']] as $objBWI) {
													// [Cyn_BlockKukakaku].[WorkItemID] = 「$B_WorkItemIDリスト」[WorkItemID]で取得
													$dataCynBlockKukaku = Cyn_BlockKukaku::where('WorkItemID', '=', $objBWI['WorkItemID'])->first();
													$nextNo = $this->returnValueSeqProjectOrder('Cyn_C_BlockKukaku', $validated['val204'], $validated['val205']);
													$lstNextSeqCNo[$objBWI['WorkItemID']] = $nextNo;

													// using $シーケンスNo below
													$pNo = (isset($lstNextSeqNo[$objBWI['ParentWorkItemID']])) ? $lstNextSeqNo[$objBWI['ParentWorkItemID']] : 0;

													$obj = $this->setDataCynCBlockKukaku_0_1($validated, $objBWI, $pNo, $nextNo, $dataCynBlockKukaku);
													$arrCynCBlockKukaku[] = $obj;

													// i.	[Cyn_Plan]から[Cyn_Plan]にデータをコピーする。
													// 「$KoteiNo連番」に0を設定する。
													// 「$T_WorkItemID」[WorkItemID]と「$B_WorkItemID」[WorkItemID]をキーに持つ、「$P_WorkItemIDリスト」数分、以下の処理を行う。
													if (isset($lstP_WorkItemID[$objTWI['WorkItemID'].'_'.$objBWI['WorkItemID']]) &&
														count($lstP_WorkItemID[$objTWI['WorkItemID'].'_'.$objBWI['WorkItemID']]) > 0) {
														$newKoteiNo = 0;	//「$KoteiNo連番」に0を設定する。
														//【Cyn_Plan】の登録
														$arrCynPlan = array();
														foreach ($lstP_WorkItemID[$objTWI['WorkItemID'].'_'.$objBWI['WorkItemID']] as $key => $objPWI) {
															// [Cyn_Plan].[WorkItemID] = 「$P_WorkItemIDリスト」[WorkItemID]で取得
															$dataCynPlan = Cyn_Plan::where('WorkItemID', '=', $objPWI['WorkItemID'])->first();

															$newKoteiNo += 1;
															$no = (isset($lstNextSeqCNo[$objPWI['ParentWorkItemID']])) ? $lstNextSeqCNo[$objPWI['ParentWorkItemID']] : 0;

															$obj = $this->setDataCynPlan_0_1($validated, $objPWI, $no, $newKoteiNo, $dataCynPlan);
															$arrCynPlan[] = $obj;
														}
														$this->insertDB('Cyn_Plan', $arrCynPlan, 90);
													}
												}
												$this->insertDB('Cyn_C_BlockKukaku', $arrCynCBlockKukaku, 70);
											}
										}
										//【Cyn_TosaiData】の登録
										$this->insertDB('Cyn_TosaiData', $arrCynTosaiDataInsert, 120);

										//【Cyn_BlockKukaku】の登録
										$this->insertDB('Cyn_BlockKukaku', $arrCynBlockKukaku, 200);
									}
								}

								//「$コピー元TableType」が1、かつ「$コピー先TableType」が0の場合
								if ($arrCopySourceTableType[0] == 1 && $arrCopyDestinationTableType[0] == 0) {
									// (ア)	 [Cyn_BlockKukaku]から[Cyn_TosaiData]にデータをコピーする。
									//「$T_WorkItemIDリスト」数分、以下の処理を行う。
									if (count($lstT_WorkItemID) > 0) {
										//【Cyn_TosaiData】の登録
										$arrCynTosaiDataInsert = array();
										foreach ($lstT_WorkItemID as $objTWI) {
											// [Cyn_BlockKukaku].[WorkItemID] = 「$T_WorkItemIDリスト」[WorkItemID]で取得
											$dataCynBlockKukaku = Cyn_BlockKukaku::where('WorkItemID', '=', $objTWI['WorkItemID'])->first();

											$obj = $this->setDataCynTosaiData_1_0($validated, $objTWI, $dataCynBlockKukaku);
											$arrCynTosaiDataInsert[] = $obj;

											// a.	[Cyn_C_BlockKukaku]から[Cyn_BlockKukaku]にデータをコピーする。
											//「$T_WorkItemID」[WorkItemID]をキーに持つ「$B_WorkItemIDリスト」数分、以下の処理を行う。
											$lstNextSeqNo = array();	// $シーケンスNo
											if (isset($lstB_WorkItemID[$objTWI['WorkItemID']]) && count($lstB_WorkItemID[$objTWI['WorkItemID']]) > 0) {
												// 【Cyn_BlockKukaku】の登録
												$arrCynBlockKukaku = array();
												foreach ($lstB_WorkItemID[$objTWI['WorkItemID']] as $objBWI) {
													// [Cyn_C_BlockKukakaku].[WorkItemID] = 「$B_WorkItemIDリスト」[WorkItemID]で取得
													$dataCynCBlockKukaku = Cyn_C_BlockKukaku::where('WorkItemID', '=', $objBWI['WorkItemID'])->first();
													$nextNo = $this->returnValueSeqProjectOrder('Cyn_BlockKukaku', $validated['val204'], $validated['val205']);
													$lstNextSeqNo[$objBWI['WorkItemID']] = $nextNo;

													$obj = $this->setDataCynBlockKukaku_1_0($validated, $objBWI, $nextNo, $dataCynCBlockKukaku);
													$arrCynBlockKukaku[] = $obj;

													// i.	[Cyn_C_Plan]から[Cyn_Plan]にデータをコピーする。
													//「$T_WorkItemID」[WorkItemID]と「$B_WorkItemID」[WorkItemID]をキーに持つ、「$P_WorkItemIDリスト」数分、以下の処理を行う。
													if (isset($lstP_WorkItemID[$objTWI['WorkItemID'].'_'.$objBWI['WorkItemID']]) &&
														count($lstP_WorkItemID[$objTWI['WorkItemID'].'_'.$objBWI['WorkItemID']]) > 0) {
														$newKoteiNo = 0;	//「$KoteiNo連番」に0を設定する。
														//【Cyn_Plan】の登録
														$arrCynPlan = array();
														foreach ($lstP_WorkItemID[$objTWI['WorkItemID'].'_'.$objBWI['WorkItemID']] as $key => $objPWI) {
															// [Cyn_C_Plan].[WorkItemID] = 「$P_WorkItemIDリスト」[WorkItemID]で取得
															$dataCynCPlan = Cyn_C_Plan::where('WorkItemID', '=', $objPWI['WorkItemID'])->first();

															$newKoteiNo += 1;
															$no = (isset($lstNextSeqNo[$objPWI['ParentWorkItemID']])) ? $lstNextSeqNo[$objPWI['ParentWorkItemID']] : 0;

															$obj = $this->setDataCynPlan_1_0($validated, $objPWI, $no, $newKoteiNo, $dataCynCPlan);
															$arrCynPlan[] = $obj;
														}
														$this->insertDB('Cyn_Plan', $arrCynPlan, 90);
													}
												}
												$this->insertDB('Cyn_BlockKukaku', $arrCynBlockKukaku, 70);
											}
										}
										$this->insertDB('Cyn_TosaiData', $arrCynTosaiDataInsert, 300);
									}
								}

								//「$コピー元TableType」が1、かつ「$コピー先TableType」が1の場合
								if ($arrCopySourceTableType[0] == 1 && $arrCopyDestinationTableType[0] == 1) {
									// (ア)	[Cyn_BlockKukaku]から[Cyn_TosaiData]、[Cyn_BlockKukaku]にデータをコピーする。
									//「$T_WorkItemIDリスト」数分、以下の処理を行う。
									$lstNextSeqNo = array();	// $シーケンスNo
									if (count($lstT_WorkItemID) > 0) {
										$arrCynTosaiDataInsert = array();
										$arrCynBlockKukaku = array();
										foreach ($lstT_WorkItemID as $objTWI) {
											// [Cyn_BlockKukaku].[WorkItemID] = 「$T_WorkItemIDリスト」[WorkItemID]で取得
											$dataCynBlockKukaku = Cyn_BlockKukaku::where('WorkItemID', '=', $objTWI['WorkItemID'])->first();
											$nextNo = $this->returnValueSeqProjectOrder('Cyn_BlockKukaku', $validated['val204'], $validated['val205']);
											$lstNextSeqNo[$objTWI['WorkItemID']] = $nextNo;

											$objCynTosai = $this->setDataCynTosaiData_1_1($validated, $dataCynBlockKukaku);
											$arrCynTosaiDataInsert[] = $objCynTosai;

											$objCynBlock = $this->setDataCynBlockKukaku_1_1($validated, $objTWI, $nextNo, $dataCynBlockKukaku);
											$arrCynBlockKukaku[] = $objCynBlock;

											// [Cyn_C_BlockKukaku]から[Cyn_C_BlockKukaku]にデータをコピーする。
											//「$T_WorkItemID」[WorkItemID]をキーに持つ「$B_WorkItemIDリスト」数分、以下の処理を行う。
											$lstNextSeqCNo = array();	// $シーケンスCNo
											if (isset($lstB_WorkItemID[$objTWI['WorkItemID']]) && count($lstB_WorkItemID[$objTWI['WorkItemID']]) > 0) {
												//【Cyn_C_BlockKukaku】の登録
												$arrCynCBlockKukaku = array();
												foreach ($lstB_WorkItemID[$objTWI['WorkItemID']] as $objBWI) {
													// [Cyn_C_BlockKukakaku].[WorkItemID] = 「$B_WorkItemIDリスト」[WorkItemID]で取得
													$dataCynCBlockKukaku = Cyn_C_BlockKukaku::where('WorkItemID', '=', $objBWI['WorkItemID'])->first();
													$nextNo = $this->returnValueSeqProjectOrder('Cyn_C_BlockKukaku', $validated['val204'], $validated['val205']);
													$lstNextSeqCNo[$objBWI['WorkItemID']] = $nextNo;

													// using $シーケンスNo below
													$pNo = (isset($lstNextSeqNo[$objBWI['ParentWorkItemID']])) ? $lstNextSeqNo[$objBWI['ParentWorkItemID']] : 0;

													$obj = $this->setDataCynCBlockKukaku_1_1($validated, $objBWI, $pNo, $nextNo, $dataCynCBlockKukaku);
													$arrCynCBlockKukaku[] = $obj;

													// i.	[Cyn_C_Plan]から[Cyn_C_Plan]にデータをコピーする。
													//「$T_WorkItemID」[WorkItemID]と「$B_WorkItemID」[WorkItemID]をキーに持つ、「$P_WorkItemIDリスト」数分、以下の処理を行う
													if (isset($lstP_WorkItemID[$objTWI['WorkItemID'].'_'.$objBWI['WorkItemID']]) &&
														count($lstP_WorkItemID[$objTWI['WorkItemID'].'_'.$objBWI['WorkItemID']]) > 0) {
														$newKoteiNo = 0;	//「$KoteiNo連番」に0を設定する。
														//【Cyn_C_Plan】の登録
														$arrCynCPlan = array();
														foreach ($lstP_WorkItemID[$objTWI['WorkItemID'].'_'.$objBWI['WorkItemID']] as $key => $objPWI) {
															// [Cyn_C_Plan].[WorkItemID] = 「$P_WorkItemIDリスト」[WorkItemID]で取得
															$dataCynCPlan = Cyn_C_Plan::where('WorkItemID', '=', $objPWI['WorkItemID'])->first();

															$newKoteiNo += 1;
															$obj = array();

															$no = (isset($lstNextSeqCNo[$objPWI['ParentWorkItemID']])) ? $lstNextSeqCNo[$objPWI['ParentWorkItemID']] : 0;

															$obj = $this->setDataCynCPlan_1_1($validated, $objPWI, $no, $newKoteiNo, $dataCynCPlan);
															$arrCynCPlan[] = $obj;
														}
														$this->insertDB('Cyn_C_Plan', $arrCynCPlan, 90);
													}
												}
												$this->insertDB('Cyn_C_BlockKukaku', $arrCynCBlockKukaku, 70);
											}
										}
										//【Cyn_TosaiData】の登録
										$this->insertDB('Cyn_TosaiData', $arrCynTosaiDataInsert, 300);

										//【Cyn_BlockKukaku】の登録
										$this->insertDB('Cyn_BlockKukaku', $arrCynBlockKukaku, 80);
									}
								}
							}

							// ⑩	履歴ログの登録
							// 【Cyn_LogData_Copy】
							$this->writeLog(config('message.msg_schem_case_002'), $row_source, $newID, $historyLogID);

							unset($checkDataCynTosai_source[$keySource]);
							unset($checkDataCynTosai_destination[$keyDes]);
							continue 2;
						} else {
							$flagCase003 = true;
						}
					}
					if ($flagCase003) {
						$this->writeLog(config('message.msg_schem_case_003'), $row_source, $newID, $historyLogID);
					}
				}
				foreach ($checkDataCynTosai_destination as $itemDes) {
					$this->writeLog(config('message.msg_schem_case_004'), $itemDes, $newID, $historyLogID);
				}
			});
		} catch (CustomException $ex) {
			// error
			$url .= '&err1=' . valueUrlEncode($ex->getMessage());
			return redirect($url);
		}

		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/index';
		$url .= '?cmn1=' . $request->cmn1;
		$url .= '&cmn2=' . $request->cmn2;

		//everything is ok
		return redirect($url);
	}

	/**
	 * create object insert Cyn_TosaiData
	 *
	 * @param Array $validated
	 * @param Array $objTWI
	 * @param Object $dataCynTosaiData
	 * @return
	 *
	 * @create 2021/01/22 Chien
	 * @update
	 */
	private function setDataCynTosaiData_0_0($validated, $objTWI, $dataCynTosaiData) {
		$obj = array();
		$obj['ProjectID'] = $validated['val204'];
		$obj['OrderNo'] = $validated['val205'];
		$obj['CKind'] = $validated['val201'];
		$obj['WorkItemID'] = $objTWI['NewWorkItemID'];
		$obj['Name'] = ($dataCynTosaiData != null) ? $dataCynTosaiData->Name : '';
		$obj['BKumiku'] = ($dataCynTosaiData != null) ? $dataCynTosaiData->BKumiku : '';
		$obj['IsOriginal'] = ($dataCynTosaiData != null) ? $dataCynTosaiData->IsOriginal : 0;
		$obj['T_Date'] = ($dataCynTosaiData != null) ? $dataCynTosaiData->T_Date : null;
		$obj['SG_Date'] = ($dataCynTosaiData != null) ? $dataCynTosaiData->SG_Date : null;
		$obj['PlSDate'] = ($dataCynTosaiData != null) ? $dataCynTosaiData->PlSDate : null;
		$obj['SG_Days'] = ($dataCynTosaiData != null) ? $dataCynTosaiData->SG_Days : null;
		$obj['NxtName'] = ($dataCynTosaiData != null) ? $dataCynTosaiData->NxtName : null;
		$obj['NxtBKumiku'] = ($dataCynTosaiData != null) ? $dataCynTosaiData->NxtBKumiku : null;
		$obj['WorkItemID_T'] = ($dataCynTosaiData != null) ? $dataCynTosaiData->WorkItemID_T : null;
		$obj['WorkItemID_K'] = ($dataCynTosaiData != null) ? $dataCynTosaiData->WorkItemID_K : null;
		$obj['WorkItemID_S'] = ($dataCynTosaiData != null) ? $dataCynTosaiData->WorkItemID_S : null;
		return $obj;
	}

	/**
	 * create object insert Cyn_BlockKukaku
	 *
	 * @param Array $validated
	 * @param Array $objBWI
	 * @param Integer $nextNo
	 * @param Object $dataCynBlockKukaku
	 * @return
	 *
	 * @create 2021/01/22 Chien
	 * @update
	 */
	private function setDataCynBlockKukaku_0_0($validated, $objBWI, $nextNo, $dataCynBlockKukaku) {
		$obj = array();
		$obj['ProjectID'] = $validated['val204'];
		$obj['OrderNo'] = $validated['val205'];
		$obj['CKind'] = $validated['val201'];
		$obj['WorkItemID'] = $objBWI['NewWorkItemID'];
		$obj['T_Name'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->T_Name : '';
		$obj['T_BKumiku'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->T_BKumiku : '';
		$obj['No'] = $nextNo;
		$obj['Name'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Name : '';
		$obj['BKumiku'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->BKumiku : '';
		$obj['N_No'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->N_No : 0;
		$obj['N_Name'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->N_Name : null;
		$obj['N_BKumiku'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->N_BKumiku : null;
		$obj['Struct'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Struct : null;
		$obj['Category'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Category : null;
		$obj['Width'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Width : null;
		$obj['Length'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Length : null;
		$obj['Height'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Height : null;
		$obj['Weight'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Weight : null;
		$obj['Zu_No'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Zu_No : null;
		$obj['KG_Weight'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->KG_Weight : null;
		$obj['True_Weight'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->True_Weight : 0;
		$obj['Is_Magari'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Is_Magari : 0;
		$obj['Del_Date'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Del_Date : null;
		$obj['O_CKind'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->O_CKind : null;
		return $obj;
	}

	/**
	 * create object insert Cyn_Plan
	 *
	 * @param Array $validated
	 * @param Array $objPWI
	 * @param Integer $no
	 * @param Integer $newKoteiNo
	 * @param Object $dataCynPlan
	 * @return
	 *
	 * @create 2021/01/22 Chien
	 * @update
	 */
	private function setDataCynPlan_0_0($validated, $objPWI, $no, $newKoteiNo, $dataCynPlan) {
		$obj = array();
		$obj['ProjectID'] = $validated['val204'];
		$obj['OrderNo'] = $validated['val205'];
		$obj['WorkItemID'] = $objPWI['NewWorkItemID'];
		$obj['No'] = $no;
		$obj['KoteiNo'] = $newKoteiNo;
		$obj['Kotei'] = ($dataCynPlan != null) ? $dataCynPlan->Kotei : '';
		$obj['KKumiku'] = ($dataCynPlan != null) ? $dataCynPlan->KKumiku : '';
		$obj['Floor'] = ($dataCynPlan != null) ? $dataCynPlan->Floor : null;
		$obj['BD_Code'] = ($dataCynPlan != null) ? $dataCynPlan->BD_Code : null;
		$obj['BData'] = ($dataCynPlan != null) ? $dataCynPlan->BData : null;
		$obj['HC'] = ($dataCynPlan != null) ? $dataCynPlan->HC : null;
		$obj['Days'] = ($dataCynPlan != null) ? $dataCynPlan->Days : 1;
		$obj['N_KoteiNo'] = ($dataCynPlan != null) ? $dataCynPlan->N_KoteiNo : 0;
		$obj['N_Kotei'] = ($dataCynPlan != null) ? $dataCynPlan->N_Kotei : null;
		$obj['N_KKumiku'] = ($dataCynPlan != null) ? $dataCynPlan->N_KKumiku : null;
		$obj['Del_Date'] = ($dataCynPlan != null) ? $dataCynPlan->Del_Date : null;
		$obj['B_PlSDate'] = ($dataCynPlan != null) ? $dataCynPlan->B_PlSDate : null;
		$obj['B_SG_Date'] = ($dataCynPlan != null) ? $dataCynPlan->B_SG_Date : null;
		$obj['B_T_Date'] = ($dataCynPlan != null) ? $dataCynPlan->B_T_Date : null;
		$obj['Jyoban'] = ($dataCynPlan != null) ? $dataCynPlan->Jyoban : null;
		return $obj;
	}

	/**
	 * create object insert Cyn_TosaiData
	 *
	 * @param Array $validated
	 * @param Object $dataCynTosaiData
	 * @return
	 *
	 * @create 2021/01/22 Chien
	 * @update
	 */
	private function setDataCynTosaiData_0_1($validated, $dataCynTosaiData) {
		$obj = array();
		$obj['ProjectID'] = $validated['val204'];
		$obj['OrderNo'] = $validated['val205'];
		$obj['CKind'] = $validated['val201'];
		$obj['WorkItemID'] = 0;
		$obj['Name'] = ($dataCynTosaiData != null) ? $dataCynTosaiData->Name : '';
		$obj['BKumiku'] = ($dataCynTosaiData != null) ? $dataCynTosaiData->BKumiku : '';
		$obj['IsOriginal'] = ($dataCynTosaiData != null) ? $dataCynTosaiData->IsOriginal : 0;
		$obj['T_Date'] = ($dataCynTosaiData != null) ? $dataCynTosaiData->T_Date : null;
		$obj['SG_Date'] = ($dataCynTosaiData != null) ? $dataCynTosaiData->SG_Date : null;
		$obj['PlSDate'] = ($dataCynTosaiData != null) ? $dataCynTosaiData->PlSDate : null;
		$obj['SG_Days'] = ($dataCynTosaiData != null) ? $dataCynTosaiData->SG_Days : null;
		$obj['NxtName'] = ($dataCynTosaiData != null) ? $dataCynTosaiData->NxtName : null;
		$obj['NxtBKumiku'] = ($dataCynTosaiData != null) ? $dataCynTosaiData->NxtBKumiku : null;
		$obj['WorkItemID_T'] = ($dataCynTosaiData != null) ? $dataCynTosaiData->WorkItemID_T : null;
		$obj['WorkItemID_K'] = ($dataCynTosaiData != null) ? $dataCynTosaiData->WorkItemID_K : null;
		$obj['WorkItemID_S'] = ($dataCynTosaiData != null) ? $dataCynTosaiData->WorkItemID_S : null;
		return $obj;
	}

	/**
	 * create object insert Cyn_BlockKukaku
	 *
	 * @param Array $validated
	 * @param Array $objTWI
	 * @param Integer $nextNo
	 * @param Object $dataCynTosaiData
	 * @return
	 *
	 * @create 2021/01/22 Chien
	 * @update
	 */
	private function setDataCynBlockKukaku_0_1($validated, $objTWI, $nextNo, $dataCynTosaiData) {
		$obj = array();
		$obj['ProjectID'] = $validated['val204'];
		$obj['OrderNo'] = $validated['val205'];
		$obj['CKind'] = $validated['val201'];
		$obj['WorkItemID'] = $objTWI['NewWorkItemID'];
		$obj['T_Name'] = ($dataCynTosaiData != null) ? $dataCynTosaiData->Name : '';
		$obj['T_BKumiku'] = ($dataCynTosaiData != null) ? $dataCynTosaiData->BKumiku : '';
		$obj['No'] = $nextNo;
		$obj['Name'] = ($dataCynTosaiData != null) ? $dataCynTosaiData->Name : '';
		$obj['BKumiku'] = ($dataCynTosaiData != null) ? $dataCynTosaiData->BKumiku : '';
		return $obj;
	}

	/**
	 * create object insert Cyn_C_BlockKukaku
	 *
	 * @param Array $validated
	 * @param Array $objBWI
	 * @param Integer $pNo
	 * @param Integer $nextNo
	 * @param Object $dataCynBlockKukaku
	 * @return
	 *
	 * @create 2021/01/22 Chien
	 * @update
	 */
	private function setDataCynCBlockKukaku_0_1($validated, $objBWI, $pNo, $nextNo, $dataCynBlockKukaku) {
		$obj = array();
		$obj['ProjectID'] = $validated['val204'];
		$obj['OrderNo'] = $validated['val205'];
		$obj['CKind'] = $validated['val201'];
		$obj['WorkItemID'] = $objBWI['NewWorkItemID'];
		$obj['P_ProjectID'] = $validated['val204'];
		$obj['P_OrderNo'] = $validated['val205'];
		$obj['P_CKind'] = $validated['val201'];
		$obj['P_No'] = $pNo;
		$obj['T_Name'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->T_Name : '';
		$obj['T_BKumiku'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->T_BKumiku : '';
		$obj['No'] = $nextNo;
		$obj['Name'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Name : '';
		$obj['BKumiku'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->BKumiku : '';
		$obj['N_No'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->N_No : 0;
		$obj['N_Name'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->N_Name : null;
		$obj['N_BKumiku'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->N_BKumiku : null;
		$obj['Struct'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Struct : null;
		$obj['Category'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Category : null;
		$obj['Width'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Width : null;
		$obj['Length'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Length : null;
		$obj['Height'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Height : null;
		$obj['Weight'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Weight : null;
		$obj['Zu_No'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Zu_No : null;
		$obj['KG_Weight'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->KG_Weight : null;
		$obj['True_Weight'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->True_Weight : 0;
		$obj['Is_Magari'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Is_Magari : 0;
		$obj['Del_Date'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Del_Date : null;
		return $obj;
	}

	/**
	 * create object insert Cyn_Plan
	 *
	 * @param Array $validated
	 * @param Array $objPWI
	 * @param Integer $no
	 * @param Integer $newKoteiNo
	 * @param Object $dataCynPlan
	 * @return
	 *
	 * @create 2021/01/22 Chien
	 * @update
	 */
	private function setDataCynPlan_0_1($validated, $objPWI, $no, $newKoteiNo, $dataCynPlan) {
		$obj = array();
		$obj['ProjectID'] = $validated['val204'];
		$obj['OrderNo'] = $validated['val205'];
		$obj['WorkItemID'] = $objPWI['NewWorkItemID'];
		$obj['No'] = $no;
		$obj['KoteiNo'] = $newKoteiNo;
		$obj['Kotei'] = ($dataCynPlan != null) ? $dataCynPlan->Kotei : '';
		$obj['KKumiku'] = ($dataCynPlan != null) ? $dataCynPlan->KKumiku : '';
		$obj['Floor'] = ($dataCynPlan != null) ? $dataCynPlan->Floor : null;
		$obj['BD_Code'] = ($dataCynPlan != null) ? $dataCynPlan->BD_Code : null;
		$obj['BData'] = ($dataCynPlan != null) ? $dataCynPlan->BData : null;
		$obj['HC'] = ($dataCynPlan != null) ? $dataCynPlan->HC : null;
		$obj['Days'] = ($dataCynPlan != null) ? $dataCynPlan->Days : 1;
		$obj['N_KoteiNo'] = ($dataCynPlan != null) ? $dataCynPlan->N_KoteiNo : 0;
		$obj['N_Kotei'] = ($dataCynPlan != null) ? $dataCynPlan->N_Kotei : null;
		$obj['N_KKumiku'] = ($dataCynPlan != null) ? $dataCynPlan->N_KKumiku : null;
		$obj['Del_Date'] = ($dataCynPlan != null) ? $dataCynPlan->Del_Date : null;
		$obj['B_PlSDate'] = ($dataCynPlan != null) ? $dataCynPlan->B_PlSDate : null;
		$obj['B_SG_Date'] = ($dataCynPlan != null) ? $dataCynPlan->B_SG_Date : null;
		$obj['B_T_Date'] = ($dataCynPlan != null) ? $dataCynPlan->B_T_Date : null;
		$obj['Jyoban'] = ($dataCynPlan != null) ? $dataCynPlan->Jyoban : null;
		return $obj;
	}

	/**
	 * create object insert Cyn_TosaiData
	 *
	 * @param Array $validated
	 * @param Array $objTWI
	 * @param Object $dataCynBlockKukaku
	 * @return
	 *
	 * @create 2021/01/22 Chien
	 * @update
	 */
	private function setDataCynTosaiData_1_0($validated, $objTWI, $dataCynBlockKukaku) {
		$obj = array();
		$obj['ProjectID'] = $validated['val204'];
		$obj['OrderNo'] = $validated['val205'];
		$obj['CKind'] = $validated['val201'];
		$obj['WorkItemID'] = $objTWI['NewWorkItemID'];
		$obj['Name'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Name : '';
		$obj['BKumiku'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->BKumiku : '';
		return $obj;
	}

	/**
	 * create object insert Cyn_BlockKukaku
	 *
	 * @param Array $validated
	 * @param Array $objBWI
	 * @param Integer $nextNo
	 * @param Object $dataCynCBlockKukaku
	 * @return
	 *
	 * @create 2021/01/22 Chien
	 * @update
	 */
	private function setDataCynBlockKukaku_1_0($validated, $objBWI, $nextNo, $dataCynCBlockKukaku) {
		$obj = array();
		$obj['ProjectID'] = $validated['val204'];
		$obj['OrderNo'] = $validated['val205'];
		$obj['CKind'] = $validated['val201'];
		$obj['WorkItemID'] = $objBWI['NewWorkItemID'];
		$obj['T_Name'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->T_Name : '';
		$obj['T_BKumiku'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->T_BKumiku : '';
		$obj['No'] = $nextNo;
		$obj['Name'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->Name : '';
		$obj['BKumiku'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->BKumiku : '';
		$obj['N_No'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->N_No : 0;
		$obj['N_Name'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->N_Name : null;
		$obj['N_BKumiku'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->N_BKumiku : null;
		$obj['Struct'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->Struct : null;
		$obj['Category'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->Category : null;
		$obj['Width'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->Width : null;
		$obj['Length'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->Length : null;
		$obj['Height'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->Height : null;
		$obj['Weight'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->Weight : null;
		$obj['Zu_No'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->Zu_No : null;
		$obj['KG_Weight'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->KG_Weight : null;
		$obj['True_Weight'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->True_Weight : 0;
		$obj['Is_Magari'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->Is_Magari : 0;
		$obj['Del_Date'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->Del_Date : null;
		return $obj;
	}

	/**
	 * create object insert Cyn_Plan
	 *
	 * @param Array $validated
	 * @param Array $objPWI
	 * @param Integer $no
	 * @param Integer $newKoteiNo
	 * @param Object $dataCynCPlan
	 * @return
	 *
	 * @create 2021/01/22 Chien
	 * @update
	 */
	private function setDataCynPlan_1_0($validated, $objPWI, $no, $newKoteiNo, $dataCynCPlan) {
		$obj = array();
		$obj['ProjectID'] = $validated['val204'];
		$obj['OrderNo'] = $validated['val205'];
		$obj['WorkItemID'] = $objPWI['NewWorkItemID'];
		$obj['No'] = $no;
		$obj['KoteiNo'] = $newKoteiNo;
		$obj['Kotei'] = ($dataCynCPlan != null) ? $dataCynCPlan->Kotei : '';
		$obj['KKumiku'] = ($dataCynCPlan != null) ? $dataCynCPlan->KKumiku : '';
		$obj['Floor'] = ($dataCynCPlan != null) ? $dataCynCPlan->Floor : null;
		$obj['BD_Code'] = ($dataCynCPlan != null) ? $dataCynCPlan->BD_Code : null;
		$obj['BData'] = ($dataCynCPlan != null) ? $dataCynCPlan->BData : null;
		$obj['HC'] = ($dataCynCPlan != null) ? $dataCynCPlan->HC : null;
		$obj['Days'] = ($dataCynCPlan != null) ? $dataCynCPlan->Days : 1;
		$obj['N_KoteiNo'] = ($dataCynCPlan != null) ? $dataCynCPlan->N_KoteiNo : 0;
		$obj['N_Kotei'] = ($dataCynCPlan != null) ? $dataCynCPlan->N_Kotei : null;
		$obj['N_KKumiku'] = ($dataCynCPlan != null) ? $dataCynCPlan->N_KKumiku : null;
		$obj['Del_Date'] = ($dataCynCPlan != null) ? $dataCynCPlan->Del_Date : null;
		$obj['B_PlSDate'] = ($dataCynCPlan != null) ? $dataCynCPlan->B_PlSDate : null;
		$obj['B_SG_Date'] = ($dataCynCPlan != null) ? $dataCynCPlan->B_SG_Date : null;
		$obj['B_T_Date'] = ($dataCynCPlan != null) ? $dataCynCPlan->B_T_Date : null;
		$obj['Jyoban'] = ($dataCynCPlan != null) ? $dataCynCPlan->Jyoban : null;
		return $obj;
	}

	/**
	 * create object insert Cyn_TosaiData
	 *
	 * @param Array $validated
	 * @param Object $dataCynBlockKukaku
	 * @return
	 *
	 * @create 2021/01/22 Chien
	 * @update
	 */
	private function setDataCynTosaiData_1_1($validated, $dataCynBlockKukaku) {
		$obj = array();
		$obj['ProjectID'] = $validated['val204'];
		$obj['OrderNo'] = $validated['val205'];
		$obj['CKind'] = $validated['val201'];
		$obj['WorkItemID'] = 0;
		$obj['Name'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Name : '';
		$obj['BKumiku'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->BKumiku : '';
		return $obj;
	}

	/**
	 * create object insert Cyn_BlockKukaku
	 *
	 * @param Array $validated
	 * @param Array $objTWI
	 * @param Integer $nextNo
	 * @param Object $dataCynBlockKukaku
	 * @return
	 *
	 * @create 2021/01/22 Chien
	 * @update
	 */
	private function setDataCynBlockKukaku_1_1($validated, $objTWI, $nextNo, $dataCynBlockKukaku) {
		$obj = array();
		$obj['ProjectID'] = $validated['val204'];
		$obj['OrderNo'] = $validated['val205'];
		$obj['CKind'] = $validated['val201'];
		$obj['WorkItemID'] = $objTWI['NewWorkItemID'];
		$obj['T_Name'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->T_Name : '';
		$obj['T_BKumiku'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->T_BKumiku : '';
		$obj['No'] = $nextNo;
		$obj['Name'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Name : '';
		$obj['BKumiku'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->BKumiku : '';
		$obj['N_No'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->N_No : 0;
		$obj['N_Name'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->N_Name : null;
		$obj['N_BKumiku'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->N_BKumiku : null;
		$obj['Struct'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Struct : null;
		$obj['Category'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Category : null;
		$obj['Width'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Width : null;
		$obj['Length'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Length : null;
		$obj['Height'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Height : null;
		$obj['Weight'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Weight : null;
		$obj['Zu_No'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Zu_No : null;
		$obj['KG_Weight'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->KG_Weight : null;
		$obj['True_Weight'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->True_Weight : 0;
		$obj['Is_Magari'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Is_Magari : 0;
		$obj['Del_Date'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->Del_Date : null;
		$obj['O_CKind'] = ($dataCynBlockKukaku != null) ? $dataCynBlockKukaku->O_CKind : null;
		return $obj;
	}

	/**
	 * create object insert Cyn_C_BlockKukaku
	 *
	 * @param Array $validated
	 * @param Array $objBWI
	 * @param Integer $pNo
	 * @param Integer $nextNo
	 * @param Object $dataCynBlockKukaku
	 * @return
	 *
	 * @create 2021/01/22 Chien
	 * @update
	 */
	private function setDataCynCBlockKukaku_1_1($validated, $objBWI, $pNo, $nextNo, $dataCynCBlockKukaku) {
		$obj = array();
		$obj['ProjectID'] = $validated['val204'];
		$obj['OrderNo'] = $validated['val205'];
		$obj['CKind'] = $validated['val201'];
		$obj['WorkItemID'] = $objBWI['NewWorkItemID'];
		$obj['P_ProjectID'] = $validated['val204'];
		$obj['P_OrderNo'] = $validated['val205'];
		$obj['P_CKind'] = $validated['val201'];
		$obj['P_No'] = $pNo;
		$obj['T_Name'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->T_Name : '';
		$obj['T_BKumiku'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->T_BKumiku : '';
		$obj['No'] = $nextNo;
		$obj['Name'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->Name : '';
		$obj['BKumiku'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->BKumiku : '';
		$obj['N_No'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->N_No : 0;
		$obj['N_Name'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->N_Name : null;
		$obj['N_BKumiku'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->N_BKumiku : null;
		$obj['Struct'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->Struct : null;
		$obj['Category'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->Category : null;
		$obj['Width'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->Width : null;
		$obj['Length'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->Length : null;
		$obj['Height'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->Height : null;
		$obj['Weight'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->Weight : null;
		$obj['Zu_No'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->Zu_No : null;
		$obj['KG_Weight'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->KG_Weight : null;
		$obj['True_Weight'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->True_Weight : 0;
		$obj['Is_Magari'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->Is_Magari : 0;
		$obj['Del_Date'] = ($dataCynCBlockKukaku != null) ? $dataCynCBlockKukaku->Del_Date : null;
		return $obj;
	}

	/**
	 * create object insert Cyn_C_Plan
	 *
	 * @param Array $validated
	 * @param Array $objPWI
	 * @param Integer $no
	 * @param Integer $newKoteiNo
	 * @param Object $dataCynCPlan
	 * @return
	 *
	 * @create 2021/01/22 Chien
	 * @update
	 */
	private function setDataCynCPlan_1_1($validated, $objPWI, $no, $newKoteiNo, $dataCynCPlan) {
		$obj = array();
		$obj['ProjectID'] = $validated['val204'];
		$obj['OrderNo'] = $validated['val205'];
		$obj['WorkItemID'] = $objPWI['NewWorkItemID'];
		$obj['No'] = $no;
		$obj['KoteiNo'] = $newKoteiNo;
		$obj['Kotei'] = ($dataCynCPlan != null) ? $dataCynCPlan->Kotei : '';
		$obj['KKumiku'] = ($dataCynCPlan != null) ? $dataCynCPlan->KKumiku : '';
		$obj['Floor'] = ($dataCynCPlan != null) ? $dataCynCPlan->Floor : null;
		$obj['BD_Code'] = ($dataCynCPlan != null) ? $dataCynCPlan->BD_Code : null;
		$obj['BData'] = ($dataCynCPlan != null) ? $dataCynCPlan->BData : null;
		$obj['HC'] = ($dataCynCPlan != null) ? $dataCynCPlan->HC : null;
		$obj['Days'] = ($dataCynCPlan != null) ? $dataCynCPlan->Days : 1;
		$obj['N_KoteiNo'] = ($dataCynCPlan != null) ? $dataCynCPlan->N_KoteiNo : 0;
		$obj['N_Kotei'] = ($dataCynCPlan != null) ? $dataCynCPlan->N_Kotei : null;
		$obj['N_KKumiku'] = ($dataCynCPlan != null) ? $dataCynCPlan->N_KKumiku : null;
		$obj['Del_Date'] = ($dataCynCPlan != null) ? $dataCynCPlan->Del_Date : null;
		$obj['B_PlSDate'] = ($dataCynCPlan != null) ? $dataCynCPlan->B_PlSDate : null;
		$obj['B_SG_Date'] = ($dataCynCPlan != null) ? $dataCynCPlan->B_SG_Date : null;
		$obj['B_T_Date'] = ($dataCynCPlan != null) ? $dataCynCPlan->B_T_Date : null;
		$obj['Jyoban'] = ($dataCynCPlan != null) ? $dataCynCPlan->Jyoban : null;
		return $obj;
	}

	/**
	 * insert log to db
	 *
	 * @param String $type
	 * @param Object $obj
	 * @param Integer $newID
	 * @param Integer $historyLogID
	 * @return
	 *
	 * @create 2020/12/31 Chien
	 * @update
	 */
	private function writeLog($msg, $obj, $newID, &$historyLogID) {
		$historyLogID += 1;
		$objCynLogDataCopy = new Cyn_LogData_Copy;
		$objCynLogDataCopy->HistoryID = $newID;
		$objCynLogDataCopy->ID = $historyLogID;
		$objCynLogDataCopy->Name = $obj->Name;
		$objCynLogDataCopy->BKumiku = $obj->BKumiku;
		$temp = FuncCommon::getKumikuData($obj->BKumiku);
		$bKumikuName = (count($temp) > 0 && is_array($temp)) ? $temp[2] : '';
		$strLog = sprintf($msg, $obj->Name, $bKumikuName);
		$objCynLogDataCopy->Log = $strLog;
		$objCynLogDataCopy->save();
	}

	/**
	 * insert data to DB
	 *
	 * @param String $table
	 * @param Array $data
	 * @param Integer $length
	 * @return
	 *
	 * @create 2020/12/31 Chien
	 * @update
	 */
	private function insertDB($table, $data, $length) {
		if (count($data) > 0) {
			$arrChunk = array_chunk($data, $length);
			if ($table == 'Cyn_TosaiData') {
				foreach ($arrChunk as $arrData) {
					Cyn_TosaiData::insert($arrData);
				}
			}
			if ($table == 'Cyn_BlockKukaku') {
				foreach ($arrChunk as $arrData) {
					Cyn_BlockKukaku::insert($arrData);
				}
			}
			if ($table == 'Cyn_C_BlockKukaku') {
				foreach ($arrChunk as $arrData) {
					Cyn_C_BlockKukaku::insert($arrData);
				}
			}
			if ($table == 'Cyn_Plan') {
				foreach ($arrChunk as $arrData) {
					Cyn_Plan::insert($arrData);
				}
			}
			if ($table == 'Cyn_C_Plan') {
				foreach ($arrChunk as $arrData) {
					Cyn_C_Plan::insert($arrData);
				}
			}
		}
	}

	/**
	* init & prepare data to show combobox on screen
	*
	* @param String $kindID
	* @param String $cKind
	* @param String $projectID
	* @param Bool $loadAll
	* @return Array mixed
	*
	* @create 2020/12/15 Chien
	* @update
	*/
	private function getInitDataDelete($kindID = '', $cKind = '', $projectID = '', $loadAll = false) {
		//
		// A2 ~ val302
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

		// A3 ~ val303
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

		return array(
			'val302' => $dataA2,
			'val303' => (count($dataA2) > 0) ? $dataA3 : array(),
		);
	}

	/**
	* ケース削除画面
	*
	* @param Request 呼び出し元リクエストオブジェクト
	* @return View ビュー
	*
	* @create 2020/12/22 Chien
	* @update
	*/
	public function delete(Request $request) {
		// 初期処理
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
			'val301' => isset($request->val301) ? valueUrlDecode($request->val301) :
						((trim(old('val301')) != '') ? valueUrlDecode(old('val301')) : config('system_const.c_kind_chijyo')),
			'val302' => isset($request->val302) ? valueUrlDecode($request->val302) :
						((trim(old('val302')) != '') ? valueUrlDecode(old('val302')) : ''),
			'val303' => isset($request->val303) ? valueUrlDecode($request->val303) :
						((trim(old('val303')) != '') ? valueUrlDecode(old('val303')) : ''),
		);

		$dataSelect = $this->getInitDataDelete($menuInfo->KindID, $itemShow['val301'], $itemShow['val302']);
		$dataSelectAll = $this->getInitDataDelete($menuInfo->KindID, '', '', true);
		if (count($dataSelect['val302']) > 0) {
			$arrUnique = array();
			foreach ($dataSelect['val302'] as $key => &$itemVal302) {
				if (count($arrUnique) == 0) {
					$arrUnique[] = $itemVal302->ProjectName;
				} else {
					if (!in_array($itemVal302->ProjectName, $arrUnique)) {
						$arrUnique[] = $itemVal302->ProjectName;
					} else {
						unset($dataSelect['val302'][$key]);
					}
				}
			}
		}
		if (count($dataSelect['val303']) > 0) {
			$arrUnique = array();
			foreach ($dataSelect['val303'] as $key => &$itemVal303) {
				if (count($arrUnique) == 0) {
					$arrUnique[] = $itemVal303->NameShow;
				} else {
					if (!in_array($itemVal303->NameShow, $arrUnique)) {
						$arrUnique[] = $itemVal303->NameShow;
					} else {
						unset($dataSelect['val303'][$key]);
					}
				}
			}
		}

		$this->data['dataSelect'] = array(
			'val302' => $dataSelect['val302'],
			'val303' => $dataSelect['val303'],
			'val302LoadAll' => $dataSelectAll['val302'],
			'val303LoadAll' => $dataSelectAll['val303'],
		);

		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
		}
		$itemShow['val301'] = valueUrlEncode($itemShow['val301']);
		$itemShow['val302'] = valueUrlEncode($itemShow['val302']);
		$itemShow['val303'] = valueUrlEncode($itemShow['val303']);

		$this->data['originalError'] = $originalError;
		$this->data['request'] = $request;
		$this->data['itemData'] = $itemShow;
		//return view with all data
		return view('Schem/Case/delete', $this->data);
	}

	/**
	 * process save action screen 030904
	 *
	 * @param CaseDeleteRequest 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/12/22 Chien
	 * @update
	 */
	public function deletesave(CaseDeleteRequest $request) {
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		// validate form
		$validated = $request->validated();

		$url = url('/');
		$url .= '/'.$menuInfo->KindURL;
		$url .= '/'.$menuInfo->MenuURL;
		$url .= '/delete';
		$url .= '?cmn1='.(isset($request->cmn1) ? $request->cmn1 : '');
		$url .= '&cmn2='.(isset($request->cmn2) ? $request->cmn2 : '');
		//encode val201 -> val206
		for ($i = 1; $i <= 3; $i++) {
			$key = 'val30'.$i;
			$url .=  '&val30'.$i.'='.valueUrlEncode($request->$key);
		}

		try {
			DB::transaction(function () use ($validated, $menuInfo) {
				// TimeTrackerNXのデータを削除する。
				// ※条件１
				$condition_1 = Cyn_TosaiData::select('Cyn_TosaiData.WorkItemID')
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
											->where('Cyn_TosaiData.CKind', '=', $validated['val301'])
											->where('Cyn_TosaiData.ProjectID', '=', $validated['val302'])
											->where('Cyn_TosaiData.WorkItemID', '<>', 0)
											->whereNotNull('Cyn_TosaiData.WorkItemID');
				$condition_1 = (isset($validated['val303']) && $validated['val303'] !== '') ?
									$condition_1->where('Cyn_TosaiData.OrderNo', '=', $validated['val303']) : $condition_1;

				// ※条件２
				$condition_2 = Cyn_BlockKukaku::select('Cyn_BlockKukaku.WorkItemID')
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
											->where('Cyn_BlockKukaku.CKind', '=', $validated['val301'])
											->where('Cyn_BlockKukaku.ProjectID', '=', $validated['val302']);
				$condition_2 = (isset($validated['val303']) && $validated['val303'] !== '') ?
									$condition_2->where('Cyn_BlockKukaku.OrderNo', '=', $validated['val303']) : $condition_2;
				$condition_2 = $condition_2->whereNull('Cyn_BlockKukaku.Del_Date')
											->whereNotNull('Cyn_BlockKukaku.WorkItemID');

				$dataUnion = $condition_1->union($condition_2)->get();	// $削除対象データ

				if (count($dataUnion) > 0) {
					$arrWorkItemID = array_column($dataUnion->toArray(), 'WorkItemID');
					// ・	「$削除対象データ」に値が設定されている場合、TimeTrackerNXのデータ削除を行う。
					// TimeTrackerCommon【deleteItem】を実行する。
					$objTimeTrackerCommon = new TimeTrackerCommon();
					$resultDeleteItem = $objTimeTrackerCommon->deleteItem($arrWorkItemID);
					if($resultDeleteItem != null) {
						throw new CustomException($resultDeleteItem);
					}
				}

				// 検討ケースに紐付く工程情報を更新する。
				// ・	工程情報を更新する。
				// 下記条件に該当するレコードを更新（論理削除）する。
				$dateNow = DB::selectOne('SELECT CONVERT(DATE, getdate()) AS sysdate')->sysdate;
				$dateNow = str_replace('-', '/', $dateNow);
				$obj['Del_Date'] = $dateNow;

				//【Cyn_Plan】
				$updateCynPlan = Cyn_Plan::where('ProjectID', '=', $validated['val302'])->whereNull('Del_Date');
				$updateCynPlan = (isset($validated['val303']) && $validated['val303'] !== '') ?
									$updateCynPlan->where('OrderNo', '=', $validated['val303']) : $updateCynPlan;
				$updateCynPlan = $updateCynPlan->update($obj);

				// ・	工程情報(中日程を親とした中日程レベルデータ)を更新する。
				// 下記条件に該当するレコードを更新（論理削除）する。
				//【Cyn_C_Plan】
				$updateCynCPlan = Cyn_C_Plan::where('ProjectID', '=', $validated['val302'])->whereNull('Del_Date');
				$updateCynCPlan = (isset($validated['val303']) && $validated['val303'] !== '') ?
									$updateCynCPlan->where('OrderNo', '=', $validated['val303']) : $updateCynCPlan;
				$updateCynCPlan = $updateCynCPlan->update($obj);

				// 検討ケースに紐付くブロック/区画情報を更新する。
				// ・	ブロック/区画情報を更新する。
				// 下記条件に該当するレコードを更新（論理削除）する。
				//【Cyn_BlockKukaku】
				$updateCynBlockKukaku = Cyn_BlockKukaku::where('ProjectID', '=', $validated['val302'])->whereNull('Del_Date');
				$updateCynBlockKukaku = (isset($validated['val303']) && $validated['val303'] !== '') ?
									$updateCynBlockKukaku->where('OrderNo', '=', $validated['val303']) : $updateCynBlockKukaku;
				$updateCynBlockKukaku = $updateCynBlockKukaku->update($obj);

				// ・	ブロック/区画情報(中日程を親とした中日程レベルデータ)を更新する。
				// 下記条件に該当するレコードを更新（論理削除）する。
				// Cyn_C_BlockKukaku】
				$updateCynCBlockKukaku = Cyn_C_BlockKukaku::where('ProjectID', '=', $validated['val302'])->whereNull('Del_Date');
				$updateCynCBlockKukaku = (isset($validated['val303']) && $validated['val303'] !== '') ?
									$updateCynCBlockKukaku->where('OrderNo', '=', $validated['val303']) : $updateCynCBlockKukaku;
				$updateCynCBlockKukaku = $updateCynCBlockKukaku->update($obj);

				// 検討ケースに紐付く搭載情報を削除する。
				// ・	搭載情報を削除する。
				// 下記条件に該当するレコードを削除する。
				//【Cyn_TosaiData】
				$updateCynTosaiData = Cyn_TosaiData::where('ProjectID', '=', $validated['val302']);
				$updateCynTosaiData = (isset($validated['val303']) && $validated['val303'] !== '') ?
									$updateCynTosaiData->where('OrderNo', '=', $validated['val303']) : $updateCynTosaiData;
				$updateCynTosaiData = $updateCynTosaiData->delete();
			});
		} catch (CustomException $ex) {
			// error
			$url .= '&err1=' . valueUrlEncode($ex->getMessage());
			return redirect($url);
		}

		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/index';
		$url .= '?cmn1=' . $request->cmn1;
		$url .= '&cmn2=' . $request->cmn2;

		//everything is ok
		return redirect($url);
	}

	/**
	* function sequence
	*
	* @param String $projectID
	* @param String $orderNo
	* @return mixed
	*
	* @create 2020/12/18 Chien
	* @update
	*/
	private function returnValueSeqProjectOrder($tblSeqName, $projectID, $orderNo) {
		$seqName = sprintf('seq_%s_%s_%s', $tblSeqName, $projectID, $orderNo);
		$sqlSeq = sprintf('SELECT NEXT VALUE FOR %s as SeqValue', $seqName);
		$flagGetValue = false;
		$value = config('system_const.seq_start_with');
		try {
			$seq_mstProject = DB::select($sqlSeq);
			if (count($seq_mstProject) > 0) {
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
			if (!$flagGetValue) {
				$seq_mstProject = DB::select($sqlSeq);
				if (count($seq_mstProject) > 0) {
					$value = $seq_mstProject[0]->SeqValue;
				}
			}
		}
		return (int)$value;
	}
}