<?php
/*
 * @CaseController.php
 * 検討ケース作成画面コントローラーファイル
 *
 * @create 2020/09/21 Chien
 *
 * @update 2020/10/28 Chien
 * @update 2021/01/08 Chien
 */

namespace App\Http\Controllers\Schet;

use App\Http\Controllers\Controller;
use App\Http\Requests\Schet\CaseCreateRequest;
use App\Http\Requests\Schet\CaseCopyRequest;
use App\Http\Requests\Schet\CaseDeleteRequest;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use Illuminate\Database\QueryException;
use Illuminate\Pagination\LengthAwarePaginator;
use App\Librarys\FuncCommon;
use App\Librarys\MenuInfo;
use App\Librarys\TimeTrackerFuncSchet;
use App\Librarys\TimeTrackerCommon;
use App\Librarys\CustomException;
use App\Models\MstProject;
use App\Models\MstOrderNo;
use App\Models\T_Tosai;
use App\Models\T_Sogumi;
use App\Models\T_Kyokyu;
use Exception;

/*
 * 検討ケース作成画面コントローラー
 *
 * @create 2020/09/21 Chien
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
	 * @create 2020/09/21 Chien
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
	 * @create 2020/09/21 Chien
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
		return view('Schet/Case/index', $this->data);
	}

	/**
	 * 新規ケース登録画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/09/21 Chien
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
			$data['ProjectName'] = valueUrlDecode($request->val1);
		} else {
			$data['ProjectName'] = '';
		}

		$this->data['originalError'] = $originalError;
		$this->data['request'] = $request;
		$this->data['itemData'] = $data;
		//return view with all data
		return view('Schet/Case/create', $this->data);
	}

	/**
	 * process save action screen 020302
	 *
	 * @param CaseCreateRequest 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/09/21 Chien
	 * @update 2020/11/10 Chien
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

		$cstrProjectListkindTosai = config('system_const.project_listkind_tosai');

		$data = MstProject::selectRaw('MAX(SerialNo) as MaxSerialNo')
			->where('SysKindID', '=', $menuInfo->KindID)
			->where('ListKind', '=', $cstrProjectListkindTosai)
			->first();
		$serialNo = 1;
		if ($data != null) {
			$serialNo = $data->MaxSerialNo + 1;
		}

		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/create';
		$url .= '?cmn1=' . $request->cmn1;
		$url .= '&cmn2=' . $request->cmn2;
		$url .= '&val1=' . valueUrlEncode($request->val1);

		try {
			$projectName = $validated['val1'];
			$projectCode = $menuInfo->KindID . '-' . config('system_const.project_listkind_tosai') . '-' . $serialNo;

			$objMstProject = new MstProject;
			$flagRollback = false;
			$msgTimeTrackerErr = '';

			//beginTransaction
			DB::transaction(function () use (
				$request,
				$objMstProject,
				$projectName,
				$projectCode,
				$serialNo,
				$menuInfo,
				$cstrProjectListkindTosai
			) {
				// get next val sequence project id
				$seq_mstProject = DB::select('SELECT NEXT VALUE FOR seq_mstProject as projectID');
				$projectID = $seq_mstProject[0]->projectID;

				$objMstProject->Up_User = $menuInfo->UserID;
				$objMstProject->ID = $projectID;
				$objMstProject->SysKindID = $menuInfo->KindID;
				$objMstProject->ListKind = $cstrProjectListkindTosai;
				$objMstProject->SerialNo = $serialNo;
				$objMstProject->ProjectName = $projectName;
				$objMstProject->save();

				/* func addCase of TimeTrackerFuncSchet */
				$objTimeTrackerFuncSchet = new TimeTrackerFuncSchet();
				$dataTimeTrackerFuncSchet = $objTimeTrackerFuncSchet->addCase($projectName, $projectCode);
				if ($dataTimeTrackerFuncSchet != null) {
					throw new CustomException($dataTimeTrackerFuncSchet);
				}
			});
		} catch (QueryException $e) {
			if ($e->getCode() == '23000') {
				//error code is 23000
				$url .= '&err1=' . valueUrlEncode(config('message.msg_cmn_db_015'));
				//redirect to $url
				return redirect($url);
			}
			throw $e;
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
	 * コピーケース登録画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/09/21 Chien
	 * @update 2020/10/28 Chien
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

		// data 1
		$data_1 = MstProject::where('SysKindID', '=', $menuInfo->KindID)
			->where('ListKind', '=', config('system_const.project_listkind_tosai'))
			->orderBy('ProjectName')
			->get();
		if (count($data_1) > 0) {
			foreach ($data_1 as &$row) {
				$row->ID = valueUrlEncode($row->ID);
			}
		}

		$this->data['dataView']['data_1'] = (count($data_1) > 0) ? $data_1 : array();

		$itemShow = array(
			'val1' => isset($request->val1) ? valueUrlDecode($request->val1) : ((trim(old('val1')) != '') ? valueUrlDecode(old('val1')) : 0),
			'val2' => isset($request->val2) ? valueUrlDecode($request->val2) : ((trim(old('val2')) != '') ? valueUrlDecode(old('val2')) : ''),
			'val3' => isset($request->val3) ? valueUrlDecode($request->val3) : ((trim(old('val3')) != '') ? valueUrlDecode(old('val3')) : ''),
			'val4' => isset($request->val4) ? valueUrlDecode($request->val4) : ((trim(old('val4')) != '') ? valueUrlDecode(old('val4')) : ''),
			'val5' => isset($request->val5) ? valueUrlDecode($request->val5) : ((trim(old('val5')) != '') ? valueUrlDecode(old('val5')) : 0),
		);

		// data 2
		$data_2 = MstOrderNo::select('OrderNo as value', 'OrderNo as name')->where('DispFlag', '=', 0)->orderBy('OrderNo')->distinct()->get();
		if (count($data_2) > 0) {
			foreach ($data_2 as &$row) {
				$row->value = valueUrlEncode($row->value);
			}
		}
		$this->data['dataView']['data_2'] = (count($data_2) > 0) ? $data_2 : array();

		// data 3
		$data_3 = $this->getDataVal2($itemShow['val1']);
		if (count($data_3) > 0) {
			$arrUnique = array();
			foreach ($data_3 as $key => &$item) {
				if (count($arrUnique) == 0) {
					$arrUnique[] = $item->val2Name;
				} else {
					if (!in_array($item->val2Name, $arrUnique)) {
						$arrUnique[] = $item->val2Name;
					} else {
						unset($data_3[$key]);
					}
				}
			}
		}
		$this->data['dataView']['data_3'] = $data_3;
		$this->data['dataView']['data_3_all'] = $this->getDataVal2('', true);

		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
			$itemShow = array(
				'val1' => isset($request->val1) ? $request->val1 : valueUrlEncode(0),
				'val2' => isset($request->val2) ? $request->val2 : '',
				'val3' => isset($request->val3) ? $request->val3 : '',
				'val4' => isset($request->val4) ? $request->val4 : '',
				'val5' => isset($request->val5) ? valueUrlDecode($request->val5) : ((trim(old('val5')) != '') ? valueUrlDecode(old('val5')) : 0),
			);
		}

		$this->data['originalError'] = $originalError;
		$this->data['request'] = $request;
		$this->data['itemShow'] = $itemShow;

		//return view with all data
		return view('Schet/Case/copy', $this->data);
	}

	/**
	 * get data value 2
	 *
	 * @param String $value ~ val1
	 * @return Object mixed
	 *
	 * @create 2020/09/24 Chien
	 * @update 2020/10/28 Chien
	 */
	private function getDataVal2($value = 0, $loadAll = false) {
		$data = T_Tosai::select('T_Tosai.ProjectID as target', 'T_Tosai.OrderNo as val2')
						->join('MstOrderNo', 'T_Tosai.OrderNo', '=', 'MstOrderNo.OrderNo')
						->where('MstOrderNo.DispFlag', '=', 0);
		$data = (trim($value) !== '') ? $data->where('T_Tosai.ProjectID', '=', $value) : $data;
		$result = $data->orderBy('val2')->get();
		if (count($result) > 0) {
			foreach ($result as &$row) {
				$row->val2Name = ($loadAll) ? htmlentities($row->val2) : $row->val2;
				$row->val2 = valueUrlEncode($row->val2);
				$row->target = valueUrlEncode($row->target);
			}
		}
		return $result;
	}

	/**
	 * process save action screen 020303
	 *
	 * @param CaseCopyRequest 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/09/21 Chien
	 * @update 2020/11/10 Chien
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

		/* url redirect */
		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/copy';
		$url .= '?cmn1=' . $request->cmn1;
		$url .= '&cmn2=' . $request->cmn2;
		for ($i = 1; $i <= 5; $i++) {
			$key = 'val' . $i;
			$url .= '&val' . $i . '=' . valueUrlEncode($request->$key);
		}

		// process lock
		$resultProcessBlock = $this->tryLock(
			$menuInfo->KindID,
			config('system_const_schet.syslock_menuid_schet'),
			$menuInfo->UserID,
			$menuInfo->SessionID,
			valueUrlDecode($request->val3),
			false
		);

		if (!is_null($resultProcessBlock)) {
			$url .= '&err1=' . valueUrlEncode($resultProcessBlock);
			return redirect($url);
		}

		$objTimeTrackerFuncSchet = new TimeTrackerFuncSchet();
		$objTimeTrackerCommon = new TimeTrackerCommon();

		// getCalendar - update 2020/11/10 - start
		$orderNoSource = ($validated['val1'] == config('system_const.projectid_production')) ? $validated['val2'] : null;
		$valueGetCalendar_source = $objTimeTrackerCommon->getCalendar($validated['val1'], $orderNoSource);
		if ($valueGetCalendar_source != '' && is_string($valueGetCalendar_source)) {
			// delete lock
			$this->deleteLock(
				$menuInfo->KindID,
				config('system_const_schet.syslock_menuid_schet'),
				$menuInfo->SessionID,
				valueUrlDecode($request->val3)
			);
			// error
			$url .= '&err1=' . valueUrlEncode($valueGetCalendar_source);
			return redirect($url);
		}
		$valueGetCalendar_destination = $objTimeTrackerCommon->getCalendar($validated['val3']);
		if ($valueGetCalendar_destination != '' && is_string($valueGetCalendar_destination)) {
			// delete lock
			$this->deleteLock(
				$menuInfo->KindID,
				config('system_const_schet.syslock_menuid_schet'),
				$menuInfo->SessionID,
				valueUrlDecode($request->val3)
			);
			// error
			$url .= '&err1=' . valueUrlEncode($valueGetCalendar_destination);
			return redirect($url);
		}
		$valueCheckCalendar = $objTimeTrackerCommon->checkCalendar($valueGetCalendar_source, $valueGetCalendar_destination);
		if ($valueCheckCalendar != null) {
			// delete lock
			$this->deleteLock(
				$menuInfo->KindID,
				config('system_const_schet.syslock_menuid_schet'),
				$menuInfo->SessionID,
				valueUrlDecode($request->val3)
			);
			// error
			$url .= '&err1=' . valueUrlEncode($valueCheckCalendar);
			return redirect($url);
		}
		// getCalendar - update 2020/11/10 - end

		/* check data original exist in T_Tosai, T_Sogumi, T_Kyokyu with val1 & val2 */
		$lstOriginalTTosai = T_Tosai::where('ProjectID', '=', $validated['val1'])
									->where('OrderNo', '=', $validated['val2'])
									->get();
		$lstOriginalTSogumi = T_Sogumi::where('ProjectID', '=', $validated['val1'])
									->where('OrderNo', '=', $validated['val2'])
									->get();
		$lstOriginalTKyokyu = T_Kyokyu::where('ProjectID', '=', $validated['val1'])
									->where('OrderNo', '=', $validated['val2'])
									->get();

		if (($lstOriginalTTosai->first() == null && $lstOriginalTSogumi->first() == null && $lstOriginalTKyokyu->first() == null) ||
			($lstOriginalTTosai->first() == null && $lstOriginalTSogumi->first() != null) ||
			($lstOriginalTSogumi->first() == null && $lstOriginalTKyokyu->first() != null)
		) {
			// delete lock
			$this->deleteLock(
				$menuInfo->KindID,
				config('system_const_schet.syslock_menuid_schet'),
				$menuInfo->SessionID,
				valueUrlDecode($request->val3)
			);
			$url .= '&err1=' . valueUrlEncode(config('message.msg_cmn_db_001'));
			return redirect($url);
		}

		try {
			$newWorkItemID = 0;
			$WorkItemIDTosai = array();

			/* delete in original */
			DB::transaction(function () use ($validated, &$newWorkItemID, $objTimeTrackerFuncSchet) {
				/* delete record will copy next time if exist in T_Tosai, T_Sogumi, T_Kyokyu */
				$delTosai = T_Tosai::where('ProjectID', '=', $validated['val3'])
					->where('OrderNo', '=', $validated['val4'])
					->delete();
				$delSogumi = T_Sogumi::where('ProjectID', '=', $validated['val3'])
					->where('OrderNo', '=', $validated['val4'])
					->delete();
				$delKyokyu = T_Kyokyu::where('ProjectID', '=', $validated['val3'])
					->where('OrderNo', '=', $validated['val4'])
					->delete();

				$checkDeleteOrderTimeTracker = $objTimeTrackerFuncSchet->deleteOrder($validated['val3'], $validated['val4']);
				if ($checkDeleteOrderTimeTracker != null) {
					throw new CustomException($checkDeleteOrderTimeTracker);
				}

				// コピー先のワークアイテムID ~ $newWorkItemID
				$newWorkItemID = $objTimeTrackerFuncSchet->createOrder($validated['val3'], $validated['val4']);
				if ($newWorkItemID != '' && !is_int($newWorkItemID)) {
					throw new CustomException($newWorkItemID);
				}
			});

			/* copy to destination */
			if (count($lstOriginalTTosai) > 0) {
				foreach ($lstOriginalTTosai as $tosaiRow) {
					DB::transaction(function () use (
						$validated,
						$tosaiRow,
						$lstOriginalTSogumi,
						$lstOriginalTKyokyu,
						$valueGetCalendar_destination,
						&$WorkItemIDTosai,
						$newWorkItemID,
						$objTimeTrackerFuncSchet
					) {
						/* dot 1 - getShiftDate */
						$checkGetShiftDateTimeTracker = $objTimeTrackerFuncSchet->getShiftDate($tosaiRow->WorkItemID, $validated['val5'],
																								$valueGetCalendar_destination);
						if ($checkGetShiftDateTimeTracker != '' && is_string($checkGetShiftDateTimeTracker)) {
							throw new CustomException($checkGetShiftDateTimeTracker);
						}

						/* dot 2 - copyItem (deleted) -> insertKotei */
						$valCopyItemTosai = $objTimeTrackerFuncSchet->insertKotei($validated['val3'], $validated['val4'], array(
							'parentid' => $newWorkItemID,
							'blockname' => $tosaiRow->BlockName,
							'kumiku' => $tosaiRow->BlockKumiku,
							'name' => config('system_const_timetracker.koteiname_schet_tosai'),
							'sdate' => $checkGetShiftDateTimeTracker['sDate'],
							'edate' => $checkGetShiftDateTimeTracker['eDate'],
							'parentflag' => true,
						), $valueGetCalendar_destination);
						if ($valCopyItemTosai != '' && is_string($valCopyItemTosai)) {
							$WorkItemIDTosai[] = $valCopyItemTosai['parentid'];
							throw new CustomException($valCopyItemTosai);
						}
						// save in memory to ready delete if error
						$WorkItemIDTosai[] = $valCopyItemTosai['parentid'];

						// dot 3 - check [T_Tosai].[BlockKumiku] = kumiku_code_sogumi(config/system_const.php) ?
						if ($tosaiRow->BlockKumiku == config('system_const.kumiku_code_sogumi')) {
							// dot 3 - 1
							$dataTSogumi = $lstOriginalTSogumi->where('BlockName', '=', $tosaiRow->BlockName)
															->where('BlockKumiku', '=', $tosaiRow->BlockKumiku)
															->first();

							if ($dataTSogumi != null) {
								// dot 3 - 2
								$checkGetShiftDateTimeTracker = $objTimeTrackerFuncSchet->getShiftDate($dataTSogumi->WorkItemID, $validated['val5'],
																										$valueGetCalendar_destination);
								if ($checkGetShiftDateTimeTracker != '' && is_string($checkGetShiftDateTimeTracker)) {
									throw new CustomException($checkGetShiftDateTimeTracker);
								}

								// dot 3 - 3 -> insertKotei
								$valCopyItemSogumi = $objTimeTrackerFuncSchet->insertKotei($validated['val3'], $validated['val4'], array(
									'parentid' => $valCopyItemTosai['parentid'],
									'blockname' => $dataTSogumi->BlockName,
									'kumiku' => $dataTSogumi->BlockKumiku,
									'name' => config('system_const_timetracker.koteiname_schet_sogumi'),
									'sdate' => $checkGetShiftDateTimeTracker['sDate'],
									'edate' => $checkGetShiftDateTimeTracker['eDate'],
									'parentflag' => false,
								), $valueGetCalendar_destination);
								if ($valCopyItemSogumi != '' && is_string($valCopyItemSogumi)) {
									throw new CustomException($valCopyItemSogumi);
								}

								// dot 3 - 4 - get data T_Kyokyu
								$dataTKyokyu = $lstOriginalTKyokyu->where('K_BlockName', '=', $dataTSogumi->BlockName)
																->where('K_BlockKumiku', '=', $dataTSogumi->BlockKumiku);
								if (count($dataTKyokyu) > 0) {
									foreach ($dataTKyokyu as $row) {
										$checkGetShiftDateTimeTracker = $objTimeTrackerFuncSchet->getShiftDate($row->WorkItemID, $validated['val5'],
																												$valueGetCalendar_destination);
										// dot 3 - 5
										if ($checkGetShiftDateTimeTracker != '' && is_string($checkGetShiftDateTimeTracker)) {
											throw new CustomException($checkGetShiftDateTimeTracker);
										}
										// dot 3 - 6 -> insertKotei
										$valCopyItemKyokyu = $objTimeTrackerFuncSchet->insertKotei($validated['val3'], $validated['val4'], array(
											'parentid' => $valCopyItemTosai['parentid'],
											'blockname' => $row->BlockName,
											'kumiku' => $row->BlockKumiku,
											'name' => config('system_const_timetracker.koteiname_schet_kyokyu'),
											'sdate' => $checkGetShiftDateTimeTracker['sDate'],
											'edate' => $checkGetShiftDateTimeTracker['eDate'],
											'parentflag' => true,
										), $valueGetCalendar_destination);
										if ($valCopyItemKyokyu != '' && is_string($valCopyItemKyokyu)) {
											throw new CustomException($valCopyItemKyokyu);
										}

										// insert new copy T_Kyokyu
										$objKyokyu = new T_Kyokyu;
										$objKyokyu->ProjectID = $validated['val3'];
										$objKyokyu->OrderNo = $validated['val4'];
										$objKyokyu->BlockName = $row->BlockName;
										$objKyokyu->BlockKumiku = $row->BlockKumiku;
										$objKyokyu->K_BlockName = $row->K_BlockName;
										$objKyokyu->K_BlockKumiku = $row->K_BlockKumiku;
										$objKyokyu->WorkItemID = $valCopyItemKyokyu['workItemid'];
										$objKyokyu->save();
									}

									// insert new copy T_Sogumi
									$objSogumi = new T_Sogumi;
									$objSogumi->ProjectID = $validated['val3'];
									$objSogumi->OrderNo = $validated['val4'];
									$objSogumi->BlockName = $dataTSogumi->BlockName;
									$objSogumi->BlockKumiku = $dataTSogumi->BlockKumiku;
									$objSogumi->WorkItemID = $valCopyItemSogumi['workItemid'];
									$objSogumi->save();
								}
							}
						}

						// insert new copy T_Tosai
						$objTosai = new T_Tosai;
						$objTosai->ProjectID = $validated['val3'];
						$objTosai->OrderNo = $validated['val4'];
						$objTosai->BlockName = $tosaiRow->BlockName;
						$objTosai->BlockKumiku = $tosaiRow->BlockKumiku;
						$objTosai->WorkItemID = $valCopyItemTosai['workItemid'];
						$objTosai->save();
					});
				}	// end foreach t_tosai
			}
		} catch (QueryException $e) {
			if (count($WorkItemIDTosai) > 0) {
				$objTimeTrackerCommon->deleteItem($WorkItemIDTosai);
			}
			$originalError = valueUrlEncode(config('message.msg_schet_case_001'));
			$url .= '&err1=' . $originalError;
			return redirect($url);
		} catch (CustomException $ex) {
			if (count($WorkItemIDTosai) > 0) {
				$objTimeTrackerCommon->deleteItem($WorkItemIDTosai);
			}
			// error
			$url .= '&err1=' . valueUrlEncode($ex->getMessage());
			return redirect($url);
		} finally {
			// delete lock
			$this->deleteLock(
				$menuInfo->KindID,
				config('system_const_schet.syslock_menuid_schet'),
				$menuInfo->SessionID,
				valueUrlDecode($request->val3)
			);
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
	 * ケース削除画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/09/21 Chien
	 * @update 2020/10/28 Chien
	 */
	public function delete(Request $request) {
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$this->data['menuInfo'] = $menuInfo;

		//initialize $originalError
		$originalError = [];

		// data 1
		$data_1 = MstProject::where('SysKindID', '=', $menuInfo->KindID)
			->where('ListKind', '=', config('system_const.project_listkind_tosai'))
			->orderBy('ProjectName')
			->get();
		if (count($data_1) > 0) {
			foreach ($data_1 as &$row) {
				$row->ID = valueUrlEncode($row->ID);
			}
		}
		$this->data['dataView']['data_1'] = (count($data_1) > 0) ? $data_1 : array();

		$itemShow = array(
			'val1' => isset($request->val1) ? valueUrlDecode($request->val1) : ((trim(old('val1')) != '') ?
				valueUrlDecode(old('val1')) : ((count($data_1) > 0) ? valueUrlDecode($data_1->first()->ID) : '')),
			'val2' => isset($request->val2) ? valueUrlDecode($request->val2) : ((trim(old('val2')) != '') ? valueUrlDecode(old('val2')) : ''),
		);

		// data 2
		$data_2 = $this->getDataVal2($itemShow['val1']);
		if (count($data_2) > 0) {
			$arrUnique = array();
			foreach ($data_2 as $key => &$item) {
				if (count($arrUnique) == 0) {
					$arrUnique[] = $item->val2Name;
				} else {
					if (!in_array($item->val2Name, $arrUnique)) {
						$arrUnique[] = $item->val2Name;
					} else {
						unset($data_2[$key]);
					}
				}
			}
		}
		$this->data['dataView']['data_2'] = $data_2;
		$this->data['dataView']['data_3_all'] = $this->getDataVal2('', true);

		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
			$itemShow = array(
				'val1' => isset($request->val1) ? $request->val1 : '',
				'val2' => isset($request->val2) ? $request->val2 : '',
			);
		}

		$this->data['originalError'] = $originalError;
		$this->data['request'] = $request;
		$this->data['itemShow'] = $itemShow;

		//return view with all data
		return view('Schet/Case/delete', $this->data);
	}

	/**
	* process save action screen 020304
	*
	* @param CaseDeleteRequest 呼び出し元リクエストオブジェクト
	* @return View ビュー
	*
	* @create 2020/09/21 Chien
	* @update 2020/11/10 Chien
	*/
	public function deletesave(CaseDeleteRequest $request)
	{
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		//validate form
		$validated = $request->validated();

		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/delete';
		$url .= '?cmn1=' . $request->cmn1;
		$url .= '&cmn2=' . $request->cmn2;
		$url .= '&val1=' . valueUrlEncode($request->val1);
		$url .= '&val2=' . valueUrlEncode($request->val2);

		// process lock
		$resultProcessBlock = $this->tryLock(
			$menuInfo->KindID,
			config('system_const_schet.syslock_menuid_schet'),
			$menuInfo->UserID,
			$menuInfo->SessionID,
			valueUrlDecode($request->val1),
			false
		);

		if (!is_null($resultProcessBlock)) {
			$url .= '&err1=' . valueUrlEncode($resultProcessBlock);
			return redirect($url);
		}

		try {
			DB::transaction(function () use ($validated) {
				$deleteTTosai = T_Tosai::where('ProjectID', '=', $validated['val1']);
				if ($validated['val2'] != null || $validated['val2'] != '') {
					$deleteTTosai = $deleteTTosai->where('OrderNo', '=', $validated['val2']);
				}
				$deleteTTosai = $deleteTTosai->delete();

				$deleteTSogumi = T_Sogumi::where('ProjectID', '=', $validated['val1']);
				if ($validated['val2'] != null || $validated['val2'] != '') {
					$deleteTSogumi = $deleteTSogumi->where('OrderNo', '=', $validated['val2']);
				}
				$deleteTSogumi = $deleteTSogumi->delete();

				$deleteTKyokyu = T_Kyokyu::where('ProjectID', '=', $validated['val1']);
				if ($validated['val2'] != null || $validated['val2'] != '') {
					$deleteTKyokyu = $deleteTKyokyu->where('OrderNo', '=', $validated['val2']);
				}
				$deleteTKyokyu = $deleteTKyokyu->delete();

				$objTimeTrackerFuncSchet = new TimeTrackerFuncSchet();
				$checkDeleteOrderTimeTracker = $objTimeTrackerFuncSchet->deleteOrder($validated['val1'], $validated['val2']);
				if ($checkDeleteOrderTimeTracker != null) {
					// error - update 020304 Rev2
					throw new CustomException($checkDeleteOrderTimeTracker);
				}
			});
		} catch (CustomException $ex) {
			// error
			$url .= '&err1=' . valueUrlEncode($ex->getMessage());
			return redirect($url);
		} catch (QueryException $e) {
			$url .= '&err1=' . valueUrlEncode(config('message.msg_schet_case_002'));
			return redirect($url);
		} finally {
			// delete lock
			$this->deleteLock(
				$menuInfo->KindID,
				config('system_const_schet.syslock_menuid_schet'),
				$menuInfo->SessionID,
				valueUrlDecode($request->val1)
			);
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
}
