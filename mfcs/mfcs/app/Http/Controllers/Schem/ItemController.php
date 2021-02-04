<?php
/*
 * @ItemController.php
 * 項目定義条件設定画面コントローラーファイル
 *
 * @create 2020/10/22 Chien
 *
 * @update
 */

namespace App\Http\Controllers\Schem;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use Illuminate\Database\QueryException;
use Illuminate\Pagination\LengthAwarePaginator;
use App\Http\Requests\Schem\ItemIndexContentsRequest;
use App\Http\Requests\Schem\ItemContentsRequest;
use App\Http\Requests\Schem\RequestItemCreate;
use App\Http\Requests\Schem\ItemPContentsRequest;
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
use App\Models\Cyn_mstKotei;
use App\Models\MstFloor;
use App\Models\MstBDCode;
use Exception;

/*
 * 項目定義条件設定画面コントローラー
 *
 * @create 2020/10/22 Chien
 *
 * @update
 */
class ItemController extends Controller
{
	/**
	 * construct
	 * @param
	 * @return mixed
	 * @create 2020/10/22 Chien
	 * @update
	 */
	public function __construct() {
	}

	/**
	 * 項目定義条件設定画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/10/22 Chien
	 * @update
	 */
	public function index(Request $request) {
		return $this->initialize($request);
	}

	/**
	 * init & prepare data to show 項目定義条件設定画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/10/22 Chien
	 * @update
	 */
	private function initialize(Request $request) {
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$this->data['menuInfo'] = $menuInfo;

		$originalError = array();
		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
		}

		$data['val1'] = (isset($request->val1)) ? valueUrlDecode($request->val1) : ((trim(old('val1')) != '') ?
							valueUrlDecode(old('val1')) : config('system_const.c_kind_chijyo'));
		$data['val2'] = (isset($request->val2)) ? valueUrlDecode($request->val2) : ((trim(old('val2')) != '') ?
							valueUrlDecode(old('val2')) : config('system_const.displayed_results_1'));

		//request
		$this->data['request'] = $request;
		$this->data['itemShow'] = $data;
		//return view with all data
		return view('Schem/Item/index', $this->data);
	}

	/**
	 * 項目定義管理画面
	 *
	 * @param ItemIndexContentsRequest 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/10/22 Chien
	 * @update 2020/12/04 Chien
	 */
	public function manage(ItemIndexContentsRequest $request) {
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$this->data['menuInfo'] = $menuInfo;

		//validate form
		$validated = $request->validated();

		$originalError = array();
		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
		}

		$dataA2 = MstProject::select('ID', 'ProjectName')
							->where('SysKindID', '=', $menuInfo->KindID)
							->where('ListKind', '=', $validated['val1'])
							->orderBy('ProjectName')
							->get();

		$val101 = (isset($request->val101)) ? $request->val101 : ((count($dataA2) > 0) ? $dataA2->first()->ID : '');

		$dataA3 = $this->getDataVal102($validated['val1'], $val101);

		$val102 = (isset($request->val102) && $request->val102 != '') ?
					$request->val102 : ((count($dataA3) > 0) ? valueUrlDecode($dataA3->first()->val102) : '');

		// data 1
		$data_1 = Cyn_TosaiData::select(
					'Cyn_BlockKukaku.No',
					'Cyn_BlockKukaku.Name',
					'Cyn_BlockKukaku.BKumiku',
					'Cyn_BlockKukaku.N_Name',
					'Cyn_BlockKukaku.N_BKumiku',
					'Cyn_BlockKukaku.Struct',
					'Cyn_BlockKukaku.Category',
					'Cyn_BlockKukaku.Width',
					'Cyn_BlockKukaku.Length',
					'Cyn_BlockKukaku.Height',
					'Cyn_BlockKukaku.Weight',
					'Cyn_BlockKukaku.Zu_No',
					'Cyn_BlockKukaku.KG_Weight',
					'Cyn_BlockKukaku.True_Weight',
					'Cyn_BlockKukaku.Is_Magari',
				)
				->selectRaw('\'0\' as GroupData')
				->join('Cyn_BlockKukaku', function($join) {
					$join->on('Cyn_TosaiData.ProjectID', '=', 'Cyn_BlockKukaku.ProjectID')
						->on('Cyn_TosaiData.OrderNo', '=', 'Cyn_BlockKukaku.OrderNo')
						->on('Cyn_TosaiData.Name', '=', 'Cyn_BlockKukaku.T_Name')
						->on('Cyn_TosaiData.BKumiku', '=', 'Cyn_BlockKukaku.T_BKumiku');
					})
				->where('Cyn_TosaiData.CKind', '=', $validated['val1'])
				->where('Cyn_TosaiData.WorkItemID', '<>', 0)
				->whereNull('Cyn_BlockKukaku.Del_Date');
		$data_1 = (trim($val101) != '') ? $data_1->where('Cyn_TosaiData.ProjectID', '=', $val101) : $data_1;
		$data_1 = (trim($val102) != '') ? $data_1->where('Cyn_TosaiData.OrderNo', '=', $val102) : $data_1;

		// data 2
		$data_2 = Cyn_TosaiData::select(
					'Cyn_C_BlockKukaku.No',
					'Cyn_C_BlockKukaku.Name',
					'Cyn_C_BlockKukaku.BKumiku',
					'Cyn_C_BlockKukaku.N_Name',
					'Cyn_C_BlockKukaku.N_BKumiku',
					'Cyn_C_BlockKukaku.Struct',
					'Cyn_C_BlockKukaku.Category',
					'Cyn_C_BlockKukaku.Width',
					'Cyn_C_BlockKukaku.Length',
					'Cyn_C_BlockKukaku.Height',
					'Cyn_C_BlockKukaku.Weight',
					'Cyn_C_BlockKukaku.Zu_No',
					'Cyn_C_BlockKukaku.KG_Weight',
					'Cyn_C_BlockKukaku.True_Weight',
					'Cyn_C_BlockKukaku.Is_Magari',
				)
				->selectRaw('\'1\' as GroupData')
				->join('Cyn_BlockKukaku', function($join) {
					$join->on('Cyn_TosaiData.ProjectID', '=', 'Cyn_BlockKukaku.ProjectID')
						->on('Cyn_TosaiData.OrderNo', '=', 'Cyn_BlockKukaku.OrderNo')
						->on('Cyn_TosaiData.Name', '=', 'Cyn_BlockKukaku.T_Name')
						->on('Cyn_TosaiData.BKumiku', '=', 'Cyn_BlockKukaku.T_BKumiku');
				})
				->join('Cyn_C_BlockKukaku', function($join) {
					$join->on('Cyn_BlockKukaku.ProjectID', '=', 'Cyn_C_BlockKukaku.ProjectID')
						->on('Cyn_BlockKukaku.OrderNo', '=', 'Cyn_C_BlockKukaku.OrderNo')
						->on('Cyn_BlockKukaku.CKind', '=', 'Cyn_C_BlockKukaku.CKind')
						->on('Cyn_BlockKukaku.Name', '=', 'Cyn_C_BlockKukaku.T_Name')
						->on('Cyn_BlockKukaku.BKumiku', '=', 'Cyn_C_BlockKukaku.T_BKumiku');
				})
				->where('Cyn_TosaiData.CKind', '=', $validated['val1'])
				->where('Cyn_TosaiData.WorkItemID', '=', 0)
				->whereNull('Cyn_BlockKukaku.Del_Date')
				->whereNull('Cyn_C_BlockKukaku.Del_Date');
		$data_2 = (trim($val101) != '') ? $data_2->where('Cyn_BlockKukaku.ProjectID', '=', $val101) : $data_2;
		$data_2 = (trim($val102) != '') ? $data_2->where('Cyn_BlockKukaku.OrderNo', '=', $val102) : $data_2;

		$data = $data_1->unionAll($data_2)->orderBy('Name')->orderBy('BKumiku')->get();

		// has data
		$flagCheckVal103 = false;
		if (count($data) > 0) {
			foreach ($data as &$row) {
				// No
				$row->No = valueUrlEncode($row->No);

				// GroupData
				if ($row->GroupData == 1 && !$flagCheckVal103) {
					$flagCheckVal103 = true;
				}
				$row->GroupData = valueUrlEncode($row->GroupData);

				// BKumiku
				if ($row->BKumiku != "") {
					$temp = FuncCommon::getKumikuData($row->BKumiku);
					$row->BKumiku = (count($temp) > 0 && is_array($temp)) ? $temp[2] : '';
				}

				// N_BKumiku
				if ($row->N_BKumiku != "") {
					$temp = FuncCommon::getKumikuData($row->N_BKumiku);
					$row->N_BKumiku = (count($temp) > 0 && is_array($temp)) ? $temp[2] : '';
				}

				// Width
				$row->Width = FuncCommon::formatDecToChar($row->Width, 1);
				// Length
				$row->Length = FuncCommon::formatDecToChar($row->Length, 1);
				// Height
				$row->Height = FuncCommon::formatDecToChar($row->Height, 1);
				// Weight
				$row->Weight = FuncCommon::formatDecToChar($row->Weight, 1);
				// KG_Weight
				$row->KG_Weight = FuncCommon::formatDecToChar($row->KG_Weight, 1);

				// True_Weight
				if ($row->True_Weight != "") {
					$row->True_Weight = ($row->True_Weight == 1) ? '確定' : ($row->True_Weight == 0 ? '未確定' : $row->True_Weight);
				}

				// Is_Magari
				if ($row->Is_Magari != "") {
					$row->Is_Magari = ($row->Is_Magari == 1) ? '曲がり' : ($row->Is_Magari == 0 ? '曲がりなし' : $row->Is_Magari);
				}
			}
		}

		// Handling sort
		// update rev4
		$sort = ['Name', 'BKumiku'];
		if (isset($request->sort) && $request->sort != '') {
			if (trim($request->sort) == 'Name') {
				$sort = ['Name', 'BKumiku'];
			} else if (trim($request->sort) == 'BKumiku') {
				$sort = ['BKumiku', 'Name'];
			} else {
				$sort = [$request->sort, 'Name', 'BKumiku'];
			}
		}
		$direction = (isset($request->direction) && trim($request->direction) != '') ? $request->direction : 'asc';
		$pageUnit = (isset($request->val2) && trim($request->val2) != '' &&
					in_array($request->val2, [config('system_const.displayed_results_1'),
											config('system_const.displayed_results_2'),
											config('system_const.displayed_results_3')]))
					? $request->val2 : config('system_const.displayed_results_1');

		$dataA4 = $this->sortAndPagination($data, $sort, $direction, $pageUnit, $request);

		if (count($dataA2) > 0) {
			foreach ($dataA2 as &$itemA2) {
				$itemA2->ID = valueUrlEncode($itemA2->ID);
			}
		}

		//request
		$this->data['request'] = $request;
		$this->data['selectVal101'] = $dataA2;
		$this->data['selectVal102'] = $dataA3;
		$this->data['originalError'] = $originalError;
		$this->data['dataA4'] = $dataA4;
		$this->data['val103'] = $flagCheckVal103 ? 1 : 0;
		return view('Schem/Item/manage', $this->data);
	}

	/**
	 * get data value 102
	 *
	 * @param String $valA1
	 * @param String $val101
	 * @return Object mixed
	 *
	 * @create 2020/10/23 Chien
	 * @update
	 */
	private function getDataVal102($valA1 = '', $val101 = '') {
		$data_1 = MstOrderNo::select('MstOrderNo.OrderNo as val102')
							->join('Cyn_TosaiData', 'MstOrderNo.OrderNo', '=', 'Cyn_TosaiData.OrderNo')
							->where('MstOrderNo.DispFlag', '<>', 1)
							->where('Cyn_TosaiData.CKind', '=', $valA1);
		$data_1 = (trim($val101) != '') ? $data_1->where('Cyn_TosaiData.ProjectId', '=', $val101) : $data_1;

		$data_2 = MstOrderNo::select('MstOrderNo.OrderNo as val102')
							->join('Cyn_BlockKukaku', 'MstOrderNo.OrderNo', '=', 'Cyn_BlockKukaku.OrderNo')
							->where('MstOrderNo.DispFlag', '<>', 1)
							->where('Cyn_BlockKukaku.CKind', '=', $valA1);
		$data_2 = (trim($val101) != '') ? $data_2->where('Cyn_BlockKukaku.ProjectId', '=', $val101) : $data_2;

		// union
		$dataVal102 = $data_1->union($data_2)->orderBy('val102')->distinct()->get();

		if (count($dataVal102) > 0) {
			foreach ($dataVal102 as &$row) {
				$row->val102Name = $row->val102;
				$row->val102 = valueUrlEncode($row->val102);
			}
		}

		return ($val101 !== '') ? $dataVal102 : array();
	}

	/**
	 * delete button click
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/10/22 Chien
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

		$projectID = (isset($request->val101) && trim($request->val101) != '') ? valueUrlDecode($request->val101) : '';
		$orderNo = (isset($request->val102) && trim($request->val102) != '') ? valueUrlDecode($request->val102) : '';
		$TimeTrackerID = (isset($request->val103) && trim($request->val103) != '') ? valueUrlDecode($request->val103) : '';
		$no = (isset($request->val104) && trim($request->val104) != '') ? valueUrlDecode($request->val104) : '';

		// init url to redirect
		$url = url('/');
		$url .= '/'.$menuInfo->KindURL;
		$url .= '/'.$menuInfo->MenuURL;
		$url .= '/manage';
		$url .= '?cmn1='.(isset($request->cmn1) ? $request->cmn1 : '');
		$url .= '&cmn2='.(isset($request->cmn2) ? $request->cmn2 : '');
		$url .= '&page='.(isset($request->page) ? $request->page : '');
		$url .= '&sort='.(isset($request->sort) ? $request->sort : '');
		$url .= '&direction='.(isset($request->direction) ? $request->direction : '');
		$url .= '&val1='.(isset($request->val1) ? $request->val1 : '');
		$url .= '&val2='.(isset($request->val2) ? $request->val2 : '');
		$url .= '&val101='.(isset($request->val101) ? $request->val101 : '');
		$url .= '&val102='.(isset($request->val102) ? $request->val102 : '');

		if (trim($projectID) != '' && trim($orderNo) != '' & trim($no) != '') {
			// find in Cyn_BlockKukaku or Cyn_C_BlockKukaku
			$obj = null;
			if ($TimeTrackerID == 0) {
				$obj = Cyn_BlockKukaku::where('ProjectID', '=', $projectID)
										->where('OrderNo', '=', $orderNo)
										->where('No', '=', $no)
										->first();
			}
			if ($TimeTrackerID == 1) {
				$obj = Cyn_C_BlockKukaku::where('ProjectID', '=', $projectID)
										->where('OrderNo', '=', $orderNo)
										->where('No', '=', $no)
										->first();
			}

			$workItemID = $obj->WorkItemID;

			//process block
			$resultProcessBlock = $this->tryLock(
				$menuInfo->KindID,
				config('system_const_schem.sys_menu_id_plan'),
				$menuInfo->UserID,
				$menuInfo->SessionID,
				valueUrlDecode($request->val1),
				false
			);

			if (!is_null($resultProcessBlock)) {
				$url .= '&err1='.valueUrlEncode($resultProcessBlock);
				return redirect($url);
			}

			try {
				DB::transaction(function() use ($TimeTrackerID, $projectID, $orderNo, $no, $workItemID) {
					$dateNow = DB::selectOne('SELECT CONVERT(DATE, getdate()) AS sysdate')->sysdate;
					$dateNow = str_replace('-', '/', $dateNow);
					$obj['Del_Date'] = $dateNow;
					if ($TimeTrackerID == 0) {
						$result = Cyn_BlockKukaku::where('ProjectID', '=', $projectID)
												->where('OrderNo', '=', $orderNo)
												->where('No', '=', $no)
												->update($obj);
					}
					if ($TimeTrackerID == 1) {
						$result = Cyn_C_BlockKukaku::where('ProjectID', '=', $projectID)
													->where('OrderNo', '=', $orderNo)
													->where('No', '=', $no)
													->update($obj);
					}

					$objTimeTrackerCommon = new TimeTrackerCommon();
					$timeTrackerDeleteItem = $objTimeTrackerCommon->deleteItem(array($workItemID));
					if (!is_null($timeTrackerDeleteItem)) {
						throw new CustomException($timeTrackerDeleteItem);
					}
				});
			} catch (CustomException $ex) {
				// error
				$url .= '&err1=' . valueUrlEncode($ex->getMessage());
				return redirect($url);
			} finally {
				$this->deleteLock(
					$menuInfo->KindID,
					config('system_const_schem.sys_menu_id_plan'),
					$menuInfo->SessionID,
					valueUrlDecode($request->val1)
				);
			}
		}

		return redirect($url);
	}

	/**
	 * 項目定義登録画面
	 *
	 * @param RequestItemCreate 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/11/02 Chien
	 * @update
	 */
	public function create(RequestItemCreate $request) {
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$this->data['menuInfo'] = $menuInfo;

		//initialize $originalError
		$originalError = [];
		$projectID = (isset($request->val101) && trim($request->val101) != '') ? valueUrlDecode($request->val101) : '';
		$orderNo = (isset($request->val102) && trim($request->val102) != '') ? valueUrlDecode($request->val102) : '';
		$group = (isset($request->val103) && trim($request->val103) != '') ? valueUrlDecode($request->val103) : '';

		$dataShow = array(
			'T_Name' => '',
			'T_BKumiku' => '',
			'Name' => '',
			'BKumiku' => '',
			'N_Name' => '',
			'N_BKumiku' => '',
			'Struct' => '',
			'Category' => '',
			'Width' => '',
			'Length' => '',
			'Height' => '',
			'Weight' => '',
			'Zu_No' => '',
			'KG_Weight' => '',
			'True_Weight' => '',
			'Is_Magari' => '',
			'Updated_at' => '',
		);
		$dataShow['T_Name'] = isset($request->val201) ? valueUrlDecode($request->val201) :
															((old('val201') != '') ? old('val201') : '');
		$dataShow['T_BKumiku'] = isset($request->val202) ? valueUrlDecode($request->val202) :
															((old('val202') != '') ? old('val202') : '');

		$dataSelect = $this->loadInitData($group, $projectID, $orderNo, $dataShow['T_Name'], valueUrlDecode($dataShow['T_BKumiku']));
		$dataSelectLoadAll = $this->loadInitData($group, $projectID, $orderNo, '', '', true);

		// remove duplicate val205
		if (count($dataSelect['val205']) > 0) {
			$arrUnique = array();
			foreach ($dataSelect['val205'] as $key => &$item) {
				if (count($arrUnique) == 0) {
					$arrUnique[] = $item->Name;
				} else {
					if (!in_array($item->Name, $arrUnique)) {
						$arrUnique[] = $item->Name;
					} else {
						unset($dataSelect['val205'][$key]);
					}
				}
			}
		}
		// remove duplicate val206
		if (count($dataSelect['val206']) > 0) {
			$arrUnique = array();
			foreach ($dataSelect['val206'] as $key => &$item) {
				if (count($arrUnique) == 0) {
					$arrUnique[] = $item->BKumiku;
				} else {
					if (!in_array($item->BKumiku, $arrUnique)) {
						$arrUnique[] = $item->BKumiku;
					} else {
						unset($dataSelect['val206'][$key]);
					}
				}
			}
		}

		$this->data['dataSelect'] = array(
			'val202' => $dataSelect['val202204'],	// val202
			'val204' => $dataSelect['val202204'],	// val204
			'val205' => $dataSelect['val205'],	// val205
			'val206' => $dataSelect['val206'],	// val206
			'val205LoadAll' => $dataSelectLoadAll['val205'],
			'val206LoadAll' => $dataSelectLoadAll['val206'],
		);

		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
			$dataShow = array(
				'T_Name' => valueUrlDecode($request->val201),
				'T_BKumiku' => $request->val202,
				'Name' => valueUrlDecode($request->val203),
				'BKumiku' => $request->val204,
				'N_Name' => $request->val205,
				'N_BKumiku' => $request->val206,
				'Struct' => valueUrlDecode($request->val207),
				'Category' => valueUrlDecode($request->val208),
				'Width' => FuncCommon::formatDecToText(valueUrlDecode($request->val209)),
				'Length' => FuncCommon::formatDecToText(valueUrlDecode($request->val210)),
				'Height' => FuncCommon::formatDecToText(valueUrlDecode($request->val211)),
				'Weight' => FuncCommon::formatDecToText(valueUrlDecode($request->val212)),
				'Zu_No' => valueUrlDecode($request->val213),
				'KG_Weight' => FuncCommon::formatDecToText(valueUrlDecode($request->val214)),
				'True_Weight' => valueUrlDecode($request->val215),
				'Is_Magari' => valueUrlDecode($request->val216),
				'Updated_at' => '',
			);
		}

		//request
		$this->data['request'] = $request;
		$this->data['originalError'] = $originalError;
		$this->data['itemData'] = $dataShow;
		//return view with all data
		return view('Schem/Item/create', $this->data);
	}

	/**
	 * 項目定義編集画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/11/02 Chien
	 * @update
	 */
	public function edit(Request $request) {
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$this->data['menuInfo'] = $menuInfo;

		//initialize $originalError
		$originalError = [];
		$projectID = (isset($request->val101) && trim($request->val101) != '') ? valueUrlDecode($request->val101) : '';
		$orderNo = (isset($request->val102) && trim($request->val102) != '') ? valueUrlDecode($request->val102) : '';
		$group = (isset($request->val103) && trim($request->val103) != '') ? valueUrlDecode($request->val103) : '';
		$no = (isset($request->val104) && trim($request->val104) != '') ? valueUrlDecode($request->val104) : '';

		$dataShow = array(
			'T_Name' => '',
			'T_BKumiku' => '',
			'Name' => '',
			'BKumiku' => '',
			'N_Name' => '',
			'N_BKumiku' => '',
			'Struct' => '',
			'Category' => '',
			'Width' => '',
			'Length' => '',
			'Height' => '',
			'Weight' => '',
			'Zu_No' => '',
			'KG_Weight' => '',
			'True_Weight' => '',
			'Is_Magari' => '',
			'Updated_at' => '',
		);
		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
			$dataShow = array(
				'T_Name' => valueUrlDecode($request->val201),
				'T_BKumiku' => $request->val202,
				'Name' => valueUrlDecode($request->val203),
				'BKumiku' => $request->val204,
				'N_Name' => $request->val205,
				'N_BKumiku' => $request->val206,
				'Struct' => valueUrlDecode($request->val207),
				'Category' => valueUrlDecode($request->val208),
				'Width' => FuncCommon::formatDecToText(valueUrlDecode($request->val209)),
				'Length' => FuncCommon::formatDecToText(valueUrlDecode($request->val210)),
				'Height' => FuncCommon::formatDecToText(valueUrlDecode($request->val211)),
				'Weight' => FuncCommon::formatDecToText(valueUrlDecode($request->val212)),
				'Zu_No' => valueUrlDecode($request->val213),
				'KG_Weight' => FuncCommon::formatDecToText(valueUrlDecode($request->val214)),
				'True_Weight' => valueUrlDecode($request->val215),
				'Is_Magari' => valueUrlDecode($request->val216),
				'Updated_at' => '',
			);
		} else {
			$dataShow = $this->getDataShow($projectID, $orderNo, $group, $no);
		}

		$dataSelect = $this->loadInitData($group, $projectID, $orderNo, $dataShow['T_Name'],
														valueUrlDecode($dataShow['T_BKumiku']));
		$dataSelectLoadAll = $this->loadInitData($group, $projectID, $orderNo, '', '', true);

		// remove duplicate val205
		if (count($dataSelect['val205']) > 0) {
			$arrUnique = array();
			foreach ($dataSelect['val205'] as $key => &$item) {
				if (count($arrUnique) == 0) {
					$arrUnique[] = $item->Name;
				} else {
					if (!in_array($item->Name, $arrUnique)) {
						$arrUnique[] = $item->Name;
					} else {
						unset($dataSelect['val205'][$key]);
					}
				}
			}
		}
		// remove duplicate val206
		if (count($dataSelect['val206']) > 0) {
			$arrUnique = array();
			foreach ($dataSelect['val206'] as $key => &$item) {
				if (count($arrUnique) == 0) {
					$arrUnique[] = $item->BKumiku;
				} else {
					if (!in_array($item->BKumiku, $arrUnique)) {
						$arrUnique[] = $item->BKumiku;
					} else {
						unset($dataSelect['val206'][$key]);
					}
				}
			}
		}

		$this->data['dataSelect'] = array(
			'val202' => $dataSelect['val202204'],	// val202
			'val204' => $dataSelect['val202204'],	// val204
			'val205' => $dataSelect['val205'],	// val205
			'val206' => $dataSelect['val206'],	// val206
			'val205LoadAll' => $dataSelectLoadAll['val205'],
			'val206LoadAll' => $dataSelectLoadAll['val206'],
		);

		//request
		$this->data['request'] = $request;
		$this->data['originalError'] = $originalError;
		$this->data['itemData'] = $dataShow;
		//return view with all data
		return view('Schem/Item/edit', $this->data);
	}

	/**
	 * 項目定義詳細画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/11/02 Chien
	 * @update
	 */
	public function show(Request $request) {
		$menuInfo = $this->checkLogin($request, config('system_const.authority_readonly'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$this->data['menuInfo'] = $menuInfo;

		$projectID = (isset($request->val101) && trim($request->val101) != '') ? valueUrlDecode($request->val101) : '';
		$orderNo = (isset($request->val102) && trim($request->val102) != '') ? valueUrlDecode($request->val102) : '';
		$group = (isset($request->val103) && trim($request->val103) != '') ? valueUrlDecode($request->val103) : '';
		$no = (isset($request->val104) && trim($request->val104) != '') ? valueUrlDecode($request->val104) : '';

		$dataSelect = $this->loadInitData($group, $projectID, $orderNo, '', '', true);
		$this->data['dataSelect'] = array(
			'val202' => $dataSelect['val202204'],	// val202
			'val204' => $dataSelect['val202204'],	// val204
			'val205' => $dataSelect['val205'],	// val205
			'val206' => $dataSelect['val206'],	// val206
			'val205LoadAll' => $dataSelect['val205'],	// val205
			'val206LoadAll' => $dataSelect['val206'],	// val206
		);

		$dataShow = $this->getDataShow($projectID, $orderNo, $group, $no);

		//request
		$this->data['request'] = $request;
		$this->data['itemData'] = $dataShow;
		//return view with all data
		return view('Schem/Item/show', $this->data);
	}

	/**
	 * load init data to combobox
	 *
	 * @param String $group
	 * @param String $projectID
	 * @param String $orderNo
	 * @param String $T_Name
	 * @param String $T_BKumiku
	 * @param Boolean $laodAll
	 * @return Array mixed
	 *
	 * @create 2020/11/02 Chien
	 * @update
	 */
	private function loadInitData($group = '', $projectID = '', $orderNo = '',
								$T_Name = '', $T_BKumiku = '', $loadAll = false) {
		// val202 + val204
		$arrKumiku = $this->getListKumiku();

		$lstDataVal205 = array();
		$lstDataVal206 = array();
		if ($group != '' && $projectID != '' && $orderNo != '') {
			// Cyn_BlockKukaku
			if ($group == 0) {
				// val205
				$lstDataVal205 = Cyn_BlockKukaku::select('Name', 'T_Name', 'T_BKumiku')
												->where('ProjectID', '=', $projectID)
												->where('OrderNo', '=', $orderNo)
												->whereNull('Del_Date');
				$lstDataVal205 = (trim($T_Name) != '') ? $lstDataVal205->where('T_Name', '=', $T_Name) : $lstDataVal205;
				$lstDataVal205 = (trim($T_BKumiku) != '') ? $lstDataVal205->where('T_BKumiku', '=', $T_BKumiku) : $lstDataVal205;
				$lstDataVal205 = $lstDataVal205->orderBy('Name')->distinct()->get();
				// val206
				$lstDataVal206 = Cyn_BlockKukaku::select('BKumiku', 'T_Name', 'T_BKumiku')
												->where('ProjectID', '=', $projectID)
												->where('OrderNo', '=', $orderNo)
												->whereNull('Del_Date');
				$lstDataVal206 = (trim($T_Name) != '') ? $lstDataVal206->where('T_Name', '=', $T_Name) : $lstDataVal206;
				$lstDataVal206 = (trim($T_BKumiku) != '') ? $lstDataVal206->where('T_BKumiku', '=', $T_BKumiku) : $lstDataVal206;
				$lstDataVal206 = $lstDataVal206->orderBy('BKumiku')->distinct()->get();
			}
			// Cyn_C_BlockKukaku
			if ($group == 1) {
				// val205
				$lstDataVal205 = Cyn_C_BlockKukaku::select('Name', 'T_Name', 'T_BKumiku')
												->where('ProjectID', '=', $projectID)
												->where('OrderNo', '=', $orderNo)
												->whereNull('Del_Date');
				$lstDataVal205 = (trim($T_Name) != '') ? $lstDataVal205->where('T_Name', '=', $T_Name) : $lstDataVal205;
				$lstDataVal205 = (trim($T_BKumiku) != '') ? $lstDataVal205->where('T_BKumiku', '=', $T_BKumiku) : $lstDataVal205;
				$lstDataVal205 = $lstDataVal205->orderBy('Name')->distinct()->get();
				// val206
				$lstDataVal206 = Cyn_C_BlockKukaku::select('BKumiku', 'T_Name', 'T_BKumiku')
												->where('ProjectID', '=', $projectID)
												->where('OrderNo', '=', $orderNo)
												->whereNull('Del_Date');
				$lstDataVal206 = (trim($T_Name) != '') ? $lstDataVal206->where('T_Name', '=', $T_Name) : $lstDataVal206;
				$lstDataVal206 = (trim($T_BKumiku) != '') ? $lstDataVal206->where('T_BKumiku', '=', $T_BKumiku) : $lstDataVal206;
				$lstDataVal206 = $lstDataVal206->orderBy('BKumiku')->distinct()->get();
			}

			if (count($lstDataVal205) > 0) {
				$nbsp = html_entity_decode('&nbsp;');
				foreach ($lstDataVal205 as &$item) {
					$tempName = $item->Name;
					$item->NameShow = ($loadAll) ? htmlentities(str_replace(" ", $nbsp, $tempName)) : str_replace(" ", $nbsp, $tempName);
					$item->Name = valueUrlEncode($item->Name);
					$item->T_BKumiku = valueUrlEncode($item->T_BKumiku);
				}
			}

			if (count($lstDataVal206) > 0) {
				foreach ($lstDataVal206 as &$item) {
					$data = FuncCommon::getKumikuData($item->BKumiku);
					$item->BKumikuName = (count($data) > 0 && is_array($data)) ? (($loadAll) ? htmlentities($data[2]) : $data[2]) : '';
					$item->BKumiku = valueUrlEncode($item->BKumiku);
					$item->T_BKumiku = valueUrlEncode($item->T_BKumiku);
				}
			}
		}

		return array(
			'val202204' => $arrKumiku,	// val202 + val204
			'val205' => $lstDataVal205,
			'val206' => $lstDataVal206,
		);
	}

	/**
	 * get list kumiku with format
	 *
	 * @param
	 * @return Array
	 *
	 * @create 2020/11/02 Chien
	 * @update
	 */
	private function getListKumiku() {
		$lstKumikuCode = array(
			config('system_const.kumiku_code_kogumi'),
			config('system_const.kumiku_code_naicyu'),
			config('system_const.kumiku_code_kumicyu'),
			config('system_const.kumiku_code_ogumi'),
			config('system_const.kumiku_code_sogumi'),
			config('system_const.kumiku_code_kyocyu'),
		);
		$arrKumiku = array();
		foreach ($lstKumikuCode as $kumiku) {
			$data = FuncCommon::getKumikuData($kumiku);
			$arrKumiku[$kumiku] = (count($data) > 0 && is_array($data)) ? $data[2] : '';
		}

		return $arrKumiku;
	}

	/**
	 * get data of No in 2 table Cyn_BlockKukaku or Cyn_C_BlockKukaku
	 *
	 * @param string project id
	 * @param string order no
	 * @param string group
	 * @param string no
	 * @return array data
	 *
	 * @create 2020/11/02 Chien
	 * @update
	 */
	private function getDataShow($projectID, $orderNo, $group, $no) {
		// init data
		$dataShow = array(
			'T_Name' => '',
			'T_BKumiku' => '',
			'Name' => '',
			'BKumiku' => '',
			'N_Name' => '',
			'N_BKumiku' => '',
			'Struct' => '',
			'Category' => '',
			'Width' => '',
			'Length' => '',
			'Height' => '',
			'Weight' => '',
			'Zu_No' => '',
			'KG_Weight' => '',
			'True_Weight' => '',
			'Is_Magari' => '',
			'Updated_at' => '',
		);

		if ($group != '' && $no != '') {
			// Cyn_BlockKukaku
			$data = null;
			if ($group == 0) {
				// find data to show
				$data = Cyn_BlockKukaku::where('ProjectID', '=', $projectID)
										->where('OrderNo', '=', $orderNo)
										->where('No', '=', $no)
										->first();
			}
			// Cyn_C_BlockKukaku
			if ($group == 1) {
				// find data to show
				$data = Cyn_C_BlockKukaku::where('ProjectID', '=', $projectID)
										->where('OrderNo', '=', $orderNo)
										->where('No', '=', $no)
										->first();
			}
			if ($data != null) {
				$dataShow = array(
					'T_Name' => $data->T_Name,
					'T_BKumiku' => ($data->T_BKumiku != '') ? valueUrlEncode($data->T_BKumiku) : '',
					'Name' => $data->Name,
					'BKumiku' => ($data->BKumiku != '') ? valueUrlEncode($data->BKumiku) : '',
					'N_Name' => ($data->N_Name != '') ? valueUrlEncode($data->N_Name) : '',
					'N_BKumiku' => ($data->N_BKumiku != '') ? valueUrlEncode($data->N_BKumiku) : '',
					'Struct' => $data->Struct,
					'Category' => $data->Category,
					'Width' => FuncCommon::formatDecToText($data->Width),
					'Length' => FuncCommon::formatDecToText($data->Length),
					'Height' => FuncCommon::formatDecToText($data->Height),
					'Weight' => FuncCommon::formatDecToText($data->Weight),
					'Zu_No' => $data->Zu_No,
					'KG_Weight' => FuncCommon::formatDecToText($data->KG_Weight),
					'True_Weight' => $data->True_Weight,
					'Is_Magari' => $data->Is_Magari,
					'Updated_at' => $data->Updated_at,
				);
			}
		}
		return $dataShow;
	}

	/**
	 * POST 項目定義共通化画面保存ボタンアクション
	 *
	 * @param ItemContentsRequest 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/11/14 Chien
	 *
	 */
	public function save(ItemContentsRequest $request) {
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		//validate form
		$validated = $request->validated();

		// initial url preparing to redirect when success or failed
		$url = url('/');
		$url .= '/'.$menuInfo->KindURL;
		$url .= '/'.$menuInfo->MenuURL;
		$url .= ($request->method == 'create') ? '/create' : '/edit';
		$url .= '?cmn1='.(isset($request->cmn1) ? $request->cmn1 : '');
		$url .= '&cmn2='.(isset($request->cmn2) ? $request->cmn2 : '');
		$url .= '&page='.(isset($request->page) ? $request->page : '');
		$url .= '&sort='.(isset($request->sort) ? $request->sort : '');
		$url .= '&direction='.(isset($request->direction) ? $request->direction : '');
		$url .= '&val1='.(isset($request->val1) ? $request->val1 : '');
		$url .= '&val2='.(isset($request->val2) ? $request->val2 : '');
		$url .= '&val101='.(isset($request->val101) ? $request->val101 : '');
		$url .= '&val102='.(isset($request->val102) ? $request->val102 : '');
		$url .= '&val103='.(isset($request->val103) ? $request->val103 : '');
		$url .= '&val104='.(isset($request->val104) ? $request->val104 : '');
		//encode val201 -> val216
		for ($i = 1; $i <= 16; $i++) {
			$key = ($i < 10) ? 'val20'.$i : 'val2'.$i;
			$url .=  ($i < 10) ? '&val20'.$i : '&val2'.$i;
			$url .= '='.valueUrlEncode($request->$key);
		}

		$timeTrackerFuncSchem = new TimeTrackerFuncSchem();
		$timeTrackerFuncCommon = new TimeTrackerCommon();
		$parentWorkItemID = null;

		if ($request->method == 'create') {
			// ルートID
			$parentWorkItemID = $timeTrackerFuncCommon->getOrderRoot(
				valueUrlDecode($request->val101),
				valueUrlDecode($request->val102)
			);
			if ($parentWorkItemID != '' && !is_int($parentWorkItemID) && is_string($parentWorkItemID)) {
				// error
				$url .= '&err1='.valueUrlEncode($parentWorkItemID);
				return redirect($url);
			}

			//process block
			$resultProcessBlock = $this->tryLock(
				$menuInfo->KindID,
				config('system_const_schem.sys_menu_id_plan'),
				$menuInfo->UserID,
				$menuInfo->SessionID,
				valueUrlDecode($request->val1),
				false
			);
			if (!is_null($resultProcessBlock)) {
				$url .= '&err1='.valueUrlEncode($resultProcessBlock);
				return redirect($url);
			}

			try {
				$queryDataTosai = Cyn_TosaiData::select('WorkItemID')
												->where('ProjectID', '=', valueUrlDecode($request->val101))
												->where('OrderNo', '=', valueUrlDecode($request->val102))
												->where('Name', '=', $request->val201)
												->where('BKumiku', '=', $request->val202)
												->first();
				// 親ID
				$workItemID = ($queryDataTosai != null) ? $queryDataTosai->WorkItemID : null;

				DB::transaction(function() use ($request, $parentWorkItemID, $workItemID,
												$queryDataTosai, $timeTrackerFuncSchem) {
					// 親ID
					$dataInsertBlock_1 = $timeTrackerFuncSchem->insertBlock(
						valueUrlDecode($request->val101),
						valueUrlDecode($request->val102),
						$parentWorkItemID,
						$workItemID,
						$request->val201,
						$request->val202
					);
					if ($dataInsertBlock_1 != '' && is_string($dataInsertBlock_1)) {
						throw new CustomException($dataInsertBlock_1);
					}

					// アイテムID
					$dataInsertBlock_2 = $timeTrackerFuncSchem->insertBlock(
						valueUrlDecode($request->val101),
						valueUrlDecode($request->val102),
						$dataInsertBlock_1,
						null,
						$request->val204,
						$request->val205
					);
					if ($dataInsertBlock_2 != '' && is_string($dataInsertBlock_2)) {
						throw new CustomException($dataInsertBlock_2);
					}

					// Cyn_TosaiData
					if ($queryDataTosai != null) {
						$objCynTosaiData['WorkItemID'] = (valueUrlDecode($request->val103) == 0) ? $dataInsertBlock_1 : 0;
						$result = Cyn_TosaiData::where('ProjectID', valueUrlDecode($request->val101))
												->Where('OrderNo', valueUrlDecode($request->val102))
												->Where('Name', $request->val201)
												->Where('BKumiku', $request->val202)
												->update($objCynTosaiData);
					} else {
						// create new Cyn_TosaiData
						$objCynTosaiData = new Cyn_TosaiData;
						$objCynTosaiData->ProjectID = valueUrlDecode($request->val101);
						$objCynTosaiData->OrderNo = valueUrlDecode($request->val102);
						$objCynTosaiData->CKind = valueUrlDecode($request->val1);
						$objCynTosaiData->WorkItemID = (valueUrlDecode($request->val103) == 0) ? $dataInsertBlock_1 : 0;
						$objCynTosaiData->Name = $request->val201;
						$objCynTosaiData->BKumiku = $request->val202;
						$objCynTosaiData->IsOriginal = 1;
						$objCynTosaiData->save();
					}

					// Cyn_BlockKukaku
					if (valueUrlDecode($request->val103) == 0) {
						// get max
						$data = Cyn_BlockKukaku::selectRaw('MAX(No) as MaxNo')
												->where('ProjectID', '=', valueUrlDecode($request->val101))
												->where('OrderNo', '=', valueUrlDecode($request->val102))
												->first();

						$noNew = ($data != null) ? ($data->MaxNo + 1) : 1;

						$N_No = 0;
						if ($request->val205 != '' && $request->val206 != '') {
							$checkNNo = Cyn_BlockKukaku::where('ProjectID', '=', valueUrlDecode($request->val101))
														->where('OrderNo', '=', valueUrlDecode($request->val102))
														->where('T_Name', '=', $request->val201)
														->where('T_BKumiku', '=', $request->val202)
														->where('Name', '=', $request->val205)
														->where('BKumiku', '=', $request->val206)
														->whereNull('Del_Date')->first();
							if ($checkNNo != null) {
								$N_No = $checkNNo->No;
							}
						}

						// insert new to Cyn_BlockKukaku
						$objCynBlockKukaku = new Cyn_BlockKukaku;
						$objCynBlockKukaku->ProjectID = valueUrlDecode($request->val101);
						$objCynBlockKukaku->OrderNo = valueUrlDecode($request->val102);
						$objCynBlockKukaku->WorkItemID = $dataInsertBlock_2;
						$objCynBlockKukaku->CKind = valueUrlDecode($request->val1);
						$objCynBlockKukaku->T_Name = $request->val201;
						$objCynBlockKukaku->T_BKumiku = $request->val202;
						$objCynBlockKukaku->No = $noNew;
						$objCynBlockKukaku->Name = $request->val203;	// update rev11
						$objCynBlockKukaku->BKumiku = $request->val204;	// update rev11

						// fix bug 171 file bug list 03
						$objCynBlockKukaku->N_No = $N_No;
						// val205 N_Name
						$objCynBlockKukaku->N_Name = $request->val205;
						// val206 N_BKumiku
						$objCynBlockKukaku->N_BKumiku = $request->val206;
						// val207 Struct
						$objCynBlockKukaku->Struct = $request->val207;
						// val208 Category
						$objCynBlockKukaku->Category = $request->val208;
						// val209 Width
						$objCynBlockKukaku->Width = $request->val209;
						// val210 Length
						$objCynBlockKukaku->Length = $request->val210;
						// val211 Height
						$objCynBlockKukaku->Height = $request->val211;
						// val212 Weight
						$objCynBlockKukaku->Weight = $request->val212;
						// val213 Zu_No
						$objCynBlockKukaku->Zu_No = $request->val213;
						// val214 KG_Weight
						$objCynBlockKukaku->KG_Weight = $request->val214;
						// val215 True_Weight
						$objCynBlockKukaku->True_Weight = $request->val215;
						// val216 Is_Magari
						$objCynBlockKukaku->Is_Magari = $request->val216;

						$objCynBlockKukaku->save();
					}

					if (valueUrlDecode($request->val103) == 1) {
						// check exists in Cyn_BlockKukaku
						$checkExistsCynBlockKukaku = Cyn_BlockKukaku::where('ProjectID', '=', valueUrlDecode($request->val101))
																	->where('OrderNo', '=', valueUrlDecode($request->val102))
																	->where('T_Name', '=', $request->val201)
																	->where('T_BKumiku', '=', $request->val202)
																	->where('Name', '=', $request->val201)		// update rev11
																	->where('BKumiku', '=', $request->val202)	// update rev11 ~ fix bug 285
																	->whereNull('Del_Date')->first();

						$pNoNew = 1;
						if ($checkExistsCynBlockKukaku != null) {
							// update
							$pNoNew = $checkExistsCynBlockKukaku->No;

							// processing update
							$obj['WorkItemID'] = $dataInsertBlock_1;
							$result = Cyn_BlockKukaku::where('ProjectID', valueUrlDecode($request->val101))
													->Where('OrderNo', valueUrlDecode($request->val102))
													->Where('No', $checkExistsCynBlockKukaku->No)
													->update($obj);
						} else {
							// get max P_No
							$data = Cyn_BlockKukaku::selectRaw('MAX(No) as MaxNo')
													->where('ProjectID', '=', valueUrlDecode($request->val101))
													->where('OrderNo', '=', valueUrlDecode($request->val102))
													->first();

							$pNoNew = ($data != null) ? ($data->MaxNo + 1) : 1;

							// insert new to Cyn_BlockKukaku
							$objCynBlockKukaku = new Cyn_BlockKukaku;
							$objCynBlockKukaku->ProjectID = valueUrlDecode($request->val101);
							$objCynBlockKukaku->OrderNo = valueUrlDecode($request->val102);
							$objCynBlockKukaku->WorkItemID = $dataInsertBlock_1;
							$objCynBlockKukaku->CKind = valueUrlDecode($request->val1);
							$objCynBlockKukaku->T_Name = $request->val201;
							$objCynBlockKukaku->T_BKumiku = $request->val202;
							$objCynBlockKukaku->No = $pNoNew;
							$objCynBlockKukaku->Name = $request->val201;	// update rev11
							$objCynBlockKukaku->BKumiku = $request->val202;	// update rev11
							$objCynBlockKukaku->save();
						}

						$checkExistsCynCBlockKukaku = Cyn_C_BlockKukaku::selectRaw('MAX(No) as MaxNo')
																		->where('ProjectID', '=', valueUrlDecode($request->val101))
																		->where('OrderNo', '=', valueUrlDecode($request->val102))->first();

						$noNewCynC = ($checkExistsCynCBlockKukaku != null) ? ($checkExistsCynCBlockKukaku->MaxNo + 1) : 1;

						$N_No = 0;
						if ($request->val205 != '' && $request->val206 != '') {
							$checkExistsCynBlockKukaku = Cyn_C_BlockKukaku::where('ProjectID', '=', valueUrlDecode($request->val101))
																			->where('OrderNo', '=', valueUrlDecode($request->val102))
																			->where('T_Name', '=', $request->val201)
																			->where('T_BKumiku', '=', $request->val202)
																			->where('Name', '=', $request->val205)
																			->where('BKumiku', '=', $request->val206)
																			->whereNull('Del_Date')->first();
							if ($checkExistsCynBlockKukaku != null) {
								$N_No = $checkExistsCynBlockKukaku->No;
							}
						}

						// insert new to Cyn_C_BlockKukaku
						$objCynCBlockKukaku = new Cyn_C_BlockKukaku;
						$objCynCBlockKukaku->ProjectID = valueUrlDecode($request->val101);
						$objCynCBlockKukaku->OrderNo = valueUrlDecode($request->val102);
						$objCynCBlockKukaku->CKind = valueUrlDecode($request->val1);
						$objCynCBlockKukaku->T_Name = $request->val201;
						$objCynCBlockKukaku->T_BKumiku = $request->val202;
						$objCynCBlockKukaku->No = $noNewCynC;
						$objCynCBlockKukaku->WorkItemID = $dataInsertBlock_2;
						$objCynCBlockKukaku->P_ProjectID = valueUrlDecode($request->val101);
						$objCynCBlockKukaku->P_OrderNo = valueUrlDecode($request->val102);
						$objCynCBlockKukaku->P_CKind = valueUrlDecode($request->val1);
						$objCynCBlockKukaku->P_No = $pNoNew;
						$objCynCBlockKukaku->N_No = $N_No;

						// val203 Name
						$objCynCBlockKukaku->Name = $request->val203;
						// val204 BKumiku
						$objCynCBlockKukaku->BKumiku = $request->val204;

						// val205 N_Name
						$objCynCBlockKukaku->N_Name = $request->val205;
						// val206 N_BKumiku
						$objCynCBlockKukaku->N_BKumiku = $request->val206;
						// val207 Struct
						$objCynCBlockKukaku->Struct = $request->val207;
						// val208 Category
						$objCynCBlockKukaku->Category = $request->val208;
						// val209 Width
						$objCynCBlockKukaku->Width = $request->val209;
						// val210 Length
						$objCynCBlockKukaku->Length = $request->val210;
						// val211 Height
						$objCynCBlockKukaku->Height = $request->val211;
						// val212 Weight
						$objCynCBlockKukaku->Weight = $request->val212;
						// val213 Zu_No
						$objCynCBlockKukaku->Zu_No = $request->val213;
						// val214 KG_Weight
						$objCynCBlockKukaku->KG_Weight = $request->val214;
						// val215 True_Weight
						$objCynCBlockKukaku->True_Weight = $request->val215;
						// val216 Is_Magari
						$objCynCBlockKukaku->Is_Magari = $request->val216;

						$objCynCBlockKukaku->save();
					}
				});
			} catch (CustomException $ex) {
				// error
				$url .= '&err1=' . valueUrlEncode($ex->getMessage());
				return redirect($url);
			} finally {
				$this->deleteLock(
					$menuInfo->KindID,
					config('system_const_schem.sys_menu_id_plan'),
					$menuInfo->SessionID,
					valueUrlDecode($request->val1)
				);
			}
		}

		if ($request->method == 'edit') {
			//process block
			$resultProcessBlock = $this->tryLock(
				$menuInfo->KindID,
				config('system_const_schem.sys_menu_id_plan'),
				$menuInfo->UserID,
				$menuInfo->SessionID,
				valueUrlDecode($request->val1),
				false
			);
			if (!is_null($resultProcessBlock)) {
				$url .= '&err1='.valueUrlEncode($resultProcessBlock);
				return redirect($url);
			}

			try {
				DB::transaction(function() use ($request, $timeTrackerFuncSchem) {
					$parentID = '';
					$workItemID = '';

					$dataTempWorkItemID = null;
					$dataTempParentID = null;
					if (valueUrlDecode($request->val103) == 0) {
						// find $workItemID
						$dataTempWorkItemID =  Cyn_BlockKukaku::select('WorkItemID')
																->where('ProjectID', '=', valueUrlDecode($request->val101))
																->where('OrderNo', '=', valueUrlDecode($request->val102))
																->where('No', '=', valueUrlDecode($request->val104))
																->first();
						// find 親ID
						$dataTempParentID = Cyn_TosaiData::select('WorkItemID')
															->where('ProjectID', '=', valueUrlDecode($request->val101))
															->where('OrderNo', '=', valueUrlDecode($request->val102))
															->where('Name', '=', $request->val201)
															->where('BKumiku', '=', $request->val202)
															->first();
					}
					if (valueUrlDecode($request->val103) == 1) {
						// find $workItemID
						$dataTempWorkItemID =  Cyn_C_BlockKukaku::select('WorkItemID')
																->where('ProjectID', '=', valueUrlDecode($request->val101))
																->where('OrderNo', '=', valueUrlDecode($request->val102))
																->where('No', '=', valueUrlDecode($request->val104))
																->first();

						// find 親ID
						$dataTempParentID = Cyn_BlockKukaku::select('WorkItemID')
															->where('ProjectID', '=', valueUrlDecode($request->val101))
															->where('OrderNo', '=', valueUrlDecode($request->val102))
															->where('Name', '=', $request->val201)
															->where('BKumiku', '=', $request->val202)
															->whereNull('Del_Date')
															->first();
					}
					if ($dataTempParentID != null) {
						$parentID = $dataTempParentID->WorkItemID;
					}
					if ($dataTempWorkItemID != null) {
						$workItemID = $dataTempWorkItemID->WorkItemID;
					}
					// アイテムID
					$dataInsertBlock = $timeTrackerFuncSchem->insertBlock(
						valueUrlDecode($request->val101),
						valueUrlDecode($request->val102),
						$parentID,
						$workItemID,
						$request->val204,
						$request->val205
					);
					if ($dataInsertBlock != '' && is_string($dataInsertBlock)) {
						throw new CustomException($dataInsertBlock);
					}

					$obj['Name'] = $request->val203;
					$obj['BKumiku'] = $request->val204;
					$obj['N_Name'] = $request->val205;
					$obj['N_BKumiku'] = $request->val206;
					$obj['Struct'] = $request->val207;
					$obj['Category'] = $request->val208;
					$obj['Width'] = $request->val209;
					$obj['Length'] = $request->val210;
					$obj['Height'] = $request->val211;
					$obj['Weight'] = $request->val212;
					$obj['Zu_No'] = $request->val213;
					$obj['KG_Weight'] = $request->val214;
					$obj['True_Weight'] = $request->val215;
					$obj['Is_Magari'] = $request->val216;
					$N_No = 0;
					if ($request->val205 != '' && $request->val206 != '') {
						$checkExistsCynBlockKukaku = null;
						if (valueUrlDecode($request->val103) == 0) {
							$checkExistsCynBlockKukaku = Cyn_BlockKukaku::where('ProjectID', '=', valueUrlDecode($request->val101))
																		->where('OrderNo', '=', valueUrlDecode($request->val102))
																		->where('T_Name', '=', $request->val201)
																		->where('T_BKumiku', '=', $request->val202)
																		->where('Name', '=', $request->val205)
																		->where('BKumiku', '=', $request->val206)
																		->whereNull('Del_Date')->first();
						}
						if (valueUrlDecode($request->val103) == 1) {
							$checkExistsCynBlockKukaku = Cyn_C_BlockKukaku::where('ProjectID', '=', valueUrlDecode($request->val101))
																			->where('OrderNo', '=', valueUrlDecode($request->val102))
																			->where('T_Name', '=', $request->val201)
																			->where('T_BKumiku', '=', $request->val202)
																			->where('Name', '=', $request->val205)
																			->where('BKumiku', '=', $request->val206)
																			->whereNull('Del_Date')->first();
						}
						if ($checkExistsCynBlockKukaku != null) {
							$N_No = $checkExistsCynBlockKukaku->No;
						}
					}
					$obj['N_No'] = $N_No;
					if (valueUrlDecode($request->val103) == 0) {
						// Cyn_BlockKukaku
						$result = Cyn_BlockKukaku::where('ProjectID', valueUrlDecode($request->val101))
												->Where('OrderNo', valueUrlDecode($request->val102))
												->Where('No', valueUrlDecode($request->val104))
												->update($obj);
					}
					if (valueUrlDecode($request->val103) == 1) {
						// Cyn_C_BlockKukaku
						$result = Cyn_C_BlockKukaku::where('ProjectID', valueUrlDecode($request->val101))
													->Where('OrderNo', valueUrlDecode($request->val102))
													->Where('No', valueUrlDecode($request->val104))
													->update($obj);
					}
				});
			} catch (CustomException $ex) {
				// error
				$url .= '&err1=' . valueUrlEncode($ex->getMessage());
				return redirect($url);
			} finally {
				$this->deleteLock(
					$menuInfo->KindID,
					config('system_const_schem.sys_menu_id_plan'),
					$menuInfo->SessionID,
					valueUrlDecode($request->val1)
				);
			}
		}

		$url = url('/');
		$url .= '/'.$menuInfo->KindURL;
		$url .= '/'.$menuInfo->MenuURL;
		$url .= '/manage';
		$url .= '?cmn1='.(isset($request->cmn1) ? $request->cmn1 : '');
		$url .= '&cmn2='.(isset($request->cmn2) ? $request->cmn2 : '');
		$url .= '&page='.(isset($request->page) ? $request->page : '');
		$url .= '&sort='.(isset($request->sort) ? $request->sort : '');
		$url .= '&direction='.(isset($request->direction) ? $request->direction : '');
		$url .= '&val1='.(isset($request->val1) ? $request->val1 : '');
		$url .= '&val2='.(isset($request->val2) ? $request->val2 : '');
		$url .= '&val101='.(isset($request->val101) ? $request->val101 : '');
		$url .= '&val102='.(isset($request->val102) ? $request->val102 : '');
		return redirect($url);
	}

	/**
	 * 項目定義詳細管理画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/11/09 Chien
	 * @update
	 */
	public function pmanage(Request $request) {
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$this->data['menuInfo'] = $menuInfo;

		$cKind = (isset($request->val1) && trim($request->val1) != '') ? valueUrlDecode($request->val1) : '';
		$projectID = (isset($request->val101) && trim($request->val101) != '') ? valueUrlDecode($request->val101) : '';
		$orderNo = (isset($request->val102) && trim($request->val102) != '') ? valueUrlDecode($request->val102) : '';
		$group = (isset($request->val103) && trim($request->val103) != '') ? valueUrlDecode($request->val103) : '';
		$no = (isset($request->val104) && trim($request->val104) != '') ? valueUrlDecode($request->val104) : '';

		$dataShow = array(
			'T_Name' => '',
			'T_BKumiku' => '',
			'Name' => '',
			'BKumiku' => '',
			'N_Name' => '',
			'N_BKumiku' => '',
			'Struct' => '',
			'Category' => '',
			'Width' => '',
			'Length' => '',
			'Height' => '',
			'Weight' => '',
			'Zu_No' => '',
			'KG_Weight' => '',
			'True_Weight' => '',
			'Is_Magari' => '',
			'Updated_at' => '',
		);
		$dataShow = $this->getDataShow($projectID, $orderNo, $group, $no);
		if ($dataShow != null) {
			if ($dataShow['BKumiku'] != '') {
				$data = FuncCommon::getKumikuData(valueUrlDecode($dataShow['BKumiku']));
				$dataShow['BKumiku'] = (count($data) > 0 && is_array($data)) ? $data[2] : '';
			}
		}

		// get data A3
		$dataA3 = array();
		if ($group == 0) {
			// Cyn_Plan
			$dataA3 = Cyn_TosaiData::select(
										'Cyn_Plan.KoteiNo',
										'Cyn_Plan.KKumiku as KKumiku',
										'Cyn_Plan.Kotei as Kotei',
										'Cyn_Plan.Floor',
										'Cyn_Plan.BD_Code',
										'Cyn_Plan.BData',
										'Cyn_Plan.HC',
										'Cyn_Plan.Days',
										'Cyn_Plan.N_KKumiku',
										'Cyn_Plan.N_Kotei',
										'Cyn_Plan.Jyoban',
										'Cyn_mstKotei.Name as CynMstKoteiName',
									)
									->join('Cyn_BlockKukaku', function($join) {
										$join->on('Cyn_TosaiData.ProjectID', '=', 'Cyn_BlockKukaku.ProjectID')
											->on('Cyn_TosaiData.OrderNo', '=', 'Cyn_BlockKukaku.OrderNo')
											->on('Cyn_TosaiData.Name', '=', 'Cyn_BlockKukaku.T_Name')
											->on('Cyn_TosaiData.BKumiku', '=', 'Cyn_BlockKukaku.T_BKumiku');
									})
									->join('Cyn_Plan', function($join) {
										$join->on('Cyn_BlockKukaku.ProjectID', '=', 'Cyn_Plan.ProjectID')
											->on('Cyn_BlockKukaku.OrderNo', '=', 'Cyn_Plan.OrderNo')
											->on('Cyn_BlockKukaku.No', '=', 'Cyn_Plan.No');
									})
									->leftjoin('Cyn_mstKotei', function($join) {
										$join->on('Cyn_mstKotei.Code', '=', 'Cyn_Plan.Kotei')
											->on('Cyn_mstKotei.CKind', '=', 'Cyn_BlockKukaku.CKind');
									})
									->leftjoin('mstFloor', 'mstFloor.Code', '=', 'Cyn_Plan.Floor')
									->leftjoin('mstBDCode', 'mstBDCode.Code', '=', 'Cyn_Plan.BD_Code')
									->where('Cyn_Plan.ProjectID', '=', $projectID)
									->where('Cyn_Plan.OrderNo', '=', $orderNo)
									->where('Cyn_Plan.No', '=', $no)
									->whereNull('Cyn_Plan.Del_Date')
									->get();
		} else {
			// Cyn_C_Plan
			$dataA3 = Cyn_BlockKukaku::select(
										'Cyn_C_Plan.KoteiNo',
										'Cyn_C_Plan.KKumiku as KKumiku',
										'Cyn_C_Plan.Kotei as Kotei',
										'Cyn_C_Plan.Floor',
										'Cyn_C_Plan.BD_Code',
										'Cyn_C_Plan.BData',
										'Cyn_C_Plan.HC',
										'Cyn_C_Plan.Days',
										'Cyn_C_Plan.N_KKumiku',
										'Cyn_C_Plan.N_Kotei',
										'Cyn_C_Plan.Jyoban',
										'Cyn_mstKotei.Name as CynMstKoteiName',
									)
									->join('Cyn_C_BlockKukaku', function($join) {
										$join->on('Cyn_BlockKukaku.ProjectID', '=', 'Cyn_C_BlockKukaku.ProjectID')
											->on('Cyn_BlockKukaku.OrderNo', '=', 'Cyn_C_BlockKukaku.OrderNo')
											->on('Cyn_BlockKukaku.CKind', '=', 'Cyn_C_BlockKukaku.CKind')
											->on('Cyn_BlockKukaku.Name', '=', 'Cyn_C_BlockKukaku.T_Name')
											->on('Cyn_BlockKukaku.BKumiku', '=', 'Cyn_C_BlockKukaku.T_BKumiku');
									})
									->join('Cyn_C_Plan', function($join) {
										$join->on('Cyn_C_BlockKukaku.ProjectID', '=', 'Cyn_C_Plan.ProjectID')
											->on('Cyn_C_BlockKukaku.OrderNo', '=', 'Cyn_C_Plan.OrderNo')
											->on('Cyn_C_BlockKukaku.No', '=', 'Cyn_C_Plan.No');
									})
									->leftjoin('Cyn_mstKotei', function($join) {
										$join->on('Cyn_mstKotei.Code', '=', 'Cyn_C_Plan.Kotei')
											->on('Cyn_mstKotei.CKind', '=', 'Cyn_C_BlockKukaku.CKind');
									})
									->leftjoin('mstFloor', 'mstFloor.Code', '=', 'Cyn_C_Plan.Floor')
									->leftjoin('mstBDCode', 'mstBDCode.Code', '=', 'Cyn_C_Plan.BD_Code')
									->where('Cyn_C_Plan.ProjectID', '=', $projectID)
									->where('Cyn_C_Plan.OrderNo', '=', $orderNo)
									->where('Cyn_C_Plan.No', '=', $no)
									->whereNull('Cyn_C_Plan.Del_Date')
									->get();
		}
		if (count($dataA3) > 0) {
			foreach ($dataA3 as &$row) {
				if ($row->KKumiku != '') {
					$data = FuncCommon::getKumikuData($row->KKumiku);
					$row->KKumiku = (count($data) > 0 && is_array($data)) ? $data[2] : '';
				}

				if ($row->Kotei != '') {
					$row->Kotei = $row->Kotei.config('system_const.code_name_separator').((
									$row->CynMstKoteiName != '') ? $row->CynMstKoteiName : '');
				}

				if ($row->Floor != '') {
					$queryMstFloor = null;
					$queryMstFloor = MstFloor::select('mstFloor.Name');
					if ($group == 0) {
						$queryMstFloor = $queryMstFloor->leftJoin('Cyn_Plan', 'mstFloor.Code', '=', 'Cyn_Plan.Floor');
					} else {
						$queryMstFloor = $queryMstFloor->leftJoin('Cyn_C_Plan', 'mstFloor.Code', '=', 'Cyn_C_Plan.Floor');
					}
					$queryMstFloor = $queryMstFloor->where('mstFloor.Code', '=', $row->Floor)->first();
					$row->Floor = $row->Floor.config('system_const.code_name_separator').(($queryMstFloor != null)
								? (($queryMstFloor->Name != '') ? $queryMstFloor->Name : '') : '');
				}

				if ($row->BD_Code != '') {
					$queryMstBDCode = null;
					$queryMstBDCode = MstBDCode::select('mstBDCode.Name');
					if ($group == 0) {
						$queryMstBDCode = $queryMstBDCode->leftJoin('Cyn_Plan', 'mstBDCode.Code', '=', 'Cyn_Plan.BD_Code');
					} else {
						$queryMstBDCode = $queryMstBDCode->leftJoin('Cyn_C_Plan', 'mstBDCode.Code', '=', 'Cyn_C_Plan.BD_Code');
					}
					$queryMstBDCode = $queryMstBDCode->where('mstBDCode.Code', '=', $row->BD_Code)->first();
					$row->BD_Code = $row->BD_Code.config('system_const.code_name_separator').(($queryMstBDCode != null)
								? (($queryMstBDCode->Name != '') ? $queryMstBDCode->Name : '') : '');
				}

				if ($row->N_KKumiku != '') {
					$data = FuncCommon::getKumikuData($row->N_KKumiku);
					$row->N_KKumiku = (count($data) > 0 && is_array($data)) ? $data[2] : '';
				}

				if ($row->N_Kotei != '') {
					$queryMstKotei = null;
					$queryMstKotei = Cyn_mstKotei::select('Cyn_mstKotei.Name');
					if ($group == 0) {
						$queryMstKotei = $queryMstKotei->leftJoin('Cyn_Plan', 'Cyn_mstKotei.Code', '=', 'Cyn_Plan.N_Kotei')
														->join('Cyn_BlockKukaku', function($join) {
															$join->on('Cyn_BlockKukaku.ProjectID', '=', 'Cyn_Plan.ProjectID')
																->on('Cyn_BlockKukaku.OrderNo', '=', 'Cyn_Plan.OrderNo')
																->on('Cyn_BlockKukaku.No', '=', 'Cyn_Plan.No')
																->on('Cyn_mstKotei.CKind', '=', 'Cyn_BlockKukaku.CKind');
														});
					} else {
						$queryMstKotei = $queryMstKotei->leftJoin('Cyn_C_Plan', 'Cyn_mstKotei.Code', '=', 'Cyn_C_Plan.N_Kotei')
														->join('Cyn_BlockKukaku', function($join) {
															$join->on('Cyn_BlockKukaku.ProjectID', '=', 'Cyn_C_Plan.ProjectID')
																->on('Cyn_BlockKukaku.OrderNo', '=', 'Cyn_C_Plan.OrderNo')
																->on('Cyn_BlockKukaku.No', '=', 'Cyn_C_Plan.No')
																->on('Cyn_mstKotei.CKind', '=', 'Cyn_BlockKukaku.CKind');
														});
					}
					$queryMstKotei = $queryMstKotei->where('Cyn_mstKotei.CKind', '=', $cKind)
													->where('Cyn_mstKotei.Code', '=', $row->N_Kotei)->first();
					$row->N_Kotei = $row->N_Kotei.config('system_const.code_name_separator').(($queryMstKotei != null)
								? (($queryMstKotei->Name != '') ? $queryMstKotei->Name : '') : '');
				}

				// BData
				$row->BData = FuncCommon::formatDecToChar($row->BData, 2);
				// HC
				$row->HC = FuncCommon::formatDecToChar($row->HC, 2);
			}
		}

		// Handling sort
		// update rev2
		$sort = ['KKumiku', 'Kotei'];
		if (isset($request->sort) && $request->sort != '') {
			if (trim($request->sort) == 'KKumiku') {
				$sort = ['KKumiku', 'Kotei'];
			} else if (trim($request->sort) == 'Kotei') {
				$sort = ['Kotei', 'KKumiku'];
			} else {
				$sort = [$request->sort, 'KKumiku', 'Kotei'];
			}
		}
		$direction = (isset($request->direction) && $request->direction != '') ?  $request->direction : 'asc';
		$pageUnit = (isset($request->val201) && $request->val201 != '' &&
					in_array(valueUrlDecode($request->val201), [config('system_const.displayed_results_1'),
												config('system_const.displayed_results_2'),
												config('system_const.displayed_results_3')]))
								? valueUrlDecode($request->val201) : config('system_const.displayed_results_1');

		$rows = $this->sortAndPagination($dataA3, $sort, $direction, $pageUnit, $request);

		//request
		$this->data['request'] = $request;
		$this->data['itemData'] = $dataShow;
		$this->data['dataA3'] = $rows;

		return view('Schem/Item/pmanage', $this->data);
	}

	/**
	* 項目定義詳細編集画面
	*
	* @param Request 呼び出し元リクエストオブジェクト
	* @return View ビュー
	*
	* @create 2020/11/14 Chien
	* @update
	*/
	public function pedit(Request $request) {
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$this->data['menuInfo'] = $menuInfo;

		//initialize $originalError
		$originalError = [];
		$cKind = (isset($request->val1) && trim($request->val1) != '') ? valueUrlDecode($request->val1) : '';
		$projectID = (isset($request->val101) && trim($request->val101) != '') ? valueUrlDecode($request->val101) : '';
		$orderNo = (isset($request->val102) && trim($request->val102) != '') ? valueUrlDecode($request->val102) : '';
		$group = (isset($request->val103) && trim($request->val103) != '') ? valueUrlDecode($request->val103) : '';
		$no = (isset($request->val104) && trim($request->val104) != '') ? valueUrlDecode($request->val104) : '';
		$koteiNo = (isset($request->val202) && trim($request->val202) != '') ? valueUrlDecode($request->val202) : '';

		$dataShow = array(
			'Kotei' => '',
			'KKumiku' => '',
			'Floor' => '',
			'BD_Code' => '',
			'BData' => '',
			'HC' => '',
			'Days' => '',
			'N_Kotei' => '',
			'N_KKumiku' => '',
			'Jyoban' => '',
		);
		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
			$dataShow = array(
				'Kotei' => $request->val301,
				'KKumiku' => $request->val302,
				'Floor' => $request->val303,
				'BD_Code' => $request->val304,
				'BData' => FuncCommon::formatDecToText(valueUrlDecode($request->val305)),
				'HC' => FuncCommon::formatDecToText(valueUrlDecode($request->val306)),
				'Days' => valueUrlDecode($request->val307),
				'N_Kotei' => $request->val308,
				'N_KKumiku' => $request->val309,
				'Jyoban' => valueUrlDecode($request->val310),
			);
		} else {
			$dataShow = $this->getDetailPContent($projectID, $orderNo, $group, $no, $koteiNo);
		}

		$initSelectData = $this->loadInitDataSelectPScreen($cKind, $projectID, $orderNo, $group, $no);

		//request
		$this->data['request'] = $request;
		$this->data['originalError'] = $originalError;
		$this->data['dataSelect'] = $initSelectData;
		$this->data['itemData'] = $dataShow;
		//return view with all data
		return view('Schem/Item/pedit', $this->data);
	}

	/**
	* load init data to combo box P's screen
	*
	* @param String $val1
	* @param String $val101
	* @param String $val102
	* @param String $val103
	* @param String $val104
	* @return Array mixed
	*
	* @create 2020/11/02 Chien
	* @update
	*/
	private function loadInitDataSelectPScreen($val1 = '', $val101 = '', $val102 = '', $val103 = '', $val104 = '') {
		// dataA1
		$dataA1 = Cyn_mstKotei::where('CKind', '=', $val1)
								->where('DelFlag', '=', 0)
								->orderBy('Code')
								->get();
		if (count($dataA1) > 0) {
			foreach ($dataA1 as &$itemA1) {
				$itemA1->NameShow = $itemA1->Code.config('system_const.code_name_separator').$itemA1->Name;
				$itemA1->Code = valueUrlEncode($itemA1->Code);
			}
		}

		// dataA2
		$dataA2 = $this->getListKumiku();

		// dataA3
		$dataA3 = MstFloor::where('ViewFlag', '=', 1)->orderBy('SortNo')->orderBy('Code')->get();
		if (count($dataA3) > 0) {
			foreach ($dataA3 as &$itemA3) {
				$itemA3->NameShow = $itemA3->Code.config('system_const.code_name_separator').$itemA3->Name;
				$itemA3->Code = valueUrlEncode($itemA3->Code);
			}
		}

		// dataA4
		$dataA4 = MstBDCode::where('ViewFlag', '=', 1)->orderBy('Code')->get();
		if (count($dataA4) > 0) {
			foreach ($dataA4 as &$itemA4) {
				$itemA4->NameShow = $itemA4->Code.config('system_const.code_name_separator').$itemA4->Name;
				$itemA4->Code = valueUrlEncode($itemA4->Code);
			}
		}

		$dataA8 = array();
		$dataA9 = array();
		if ($val103 == 0) {
			// Cyn_Plan
			// dataA8
			$dataA8 = Cyn_Plan::select('Cyn_mstKotei.Code', 'Cyn_mstKotei.Name')
								->join('Cyn_mstKotei', 'Cyn_mstKotei.Code', '=', 'Cyn_Plan.Kotei')
								->join('Cyn_BlockKukaku', function($join) {
									$join->on('Cyn_BlockKukaku.ProjectID', '=', 'Cyn_Plan.ProjectID')
										->on('Cyn_BlockKukaku.OrderNo', '=', 'Cyn_Plan.OrderNo')
										->on('Cyn_BlockKukaku.No', '=', 'Cyn_Plan.No')
										->on('Cyn_mstKotei.CKind', '=', 'Cyn_BlockKukaku.CKind');
								})
								->where('Cyn_BlockKukaku.CKind', '=', $val1)
								->where('Cyn_Plan.ProjectID', '=', $val101)
								->where('Cyn_Plan.OrderNo', '=', $val102)
								->where('Cyn_Plan.No', '=', $val104)
								->whereNull('Cyn_Plan.Del_Date')
								->orderBy('Cyn_mstKotei.Code')
								->distinct()->get();
			// dataA9
			$dataA9 = Cyn_Plan::select('Cyn_Plan.KKumiku')
								->join('Cyn_BlockKukaku', function($join) {
									$join->on('Cyn_BlockKukaku.ProjectID', '=', 'Cyn_Plan.ProjectID')
										->on('Cyn_BlockKukaku.OrderNo', '=', 'Cyn_Plan.OrderNo')
										->on('Cyn_BlockKukaku.No', '=', 'Cyn_Plan.No');
								})
								->where('Cyn_BlockKukaku.CKind', '=', $val1)
								->where('Cyn_Plan.ProjectID', '=', $val101)
								->where('Cyn_Plan.OrderNo', '=', $val102)
								->where('Cyn_Plan.No', '=', $val104)
								->whereNull('Cyn_Plan.Del_Date')
								->orderBy('Cyn_Plan.KKumiku')
								->distinct()->get();
		}
		if ($val103 == 1) {
			// Cyn_C_Plan
			// dataA8
			$dataA8 = Cyn_C_Plan::select('Cyn_mstKotei.Code', 'Cyn_mstKotei.Name')
								->join('Cyn_mstKotei', 'Cyn_mstKotei.Code', '=', 'Cyn_C_Plan.Kotei')
								->join('Cyn_C_BlockKukaku', function($join) {
									$join->on('Cyn_C_BlockKukaku.ProjectID', '=', 'Cyn_C_Plan.ProjectID')
										->on('Cyn_C_BlockKukaku.OrderNo', '=', 'Cyn_C_Plan.OrderNo')
										->on('Cyn_C_BlockKukaku.No', '=', 'Cyn_C_Plan.No')
										->on('Cyn_mstKotei.CKind', '=', 'Cyn_C_BlockKukaku.CKind');
								})
								->where('Cyn_C_BlockKukaku.CKind', '=', $val1)
								->where('Cyn_C_Plan.ProjectID', '=', $val101)
								->where('Cyn_C_Plan.OrderNo', '=', $val102)
								->where('Cyn_C_Plan.No', '=', $val104)
								->whereNull('Cyn_C_Plan.Del_Date')
								->orderBy('Cyn_mstKotei.Code')
								->distinct()->get();
			// dataA9
			$dataA9 = Cyn_C_Plan::select('Cyn_C_Plan.KKumiku')
								->join('Cyn_C_BlockKukaku', function($join) {
									$join->on('Cyn_C_BlockKukaku.ProjectID', '=', 'Cyn_C_Plan.ProjectID')
										->on('Cyn_C_BlockKukaku.OrderNo', '=', 'Cyn_C_Plan.OrderNo')
										->on('Cyn_C_BlockKukaku.No', '=', 'Cyn_C_Plan.No');
								})
								->where('Cyn_C_BlockKukaku.CKind', '=', $val1)
								->where('Cyn_C_Plan.ProjectID', '=', $val101)
								->where('Cyn_C_Plan.OrderNo', '=', $val102)
								->where('Cyn_C_Plan.No', '=', $val104)
								->whereNull('Cyn_C_Plan.Del_Date')
								->orderBy('Cyn_C_Plan.KKumiku')
								->distinct()->get();
		}
		if (count($dataA8) > 0) {
			foreach ($dataA8 as &$itemA8) {
				$itemA8->NameShow = $itemA8->Code.config('system_const.code_name_separator').$itemA8->Name;
				$itemA8->Code = valueUrlEncode($itemA8->Code);
			}
		}
		if (count($dataA9) > 0) {
			foreach ($dataA9 as &$itemA9) {
				if ($itemA9->KKumiku != '') {
					$data = FuncCommon::getKumikuData($itemA9->KKumiku);
					$itemA9->KKumikuShow = (count($data) > 0 && is_array($data)) ? $data[2] : '';
					$itemA9->KKumiku = valueUrlEncode($itemA9->KKumiku);
				}
			}
		}

		return array(
			'val301' => $dataA1,
			'val302' => $dataA2,
			'val303' => $dataA3,
			'val304' => $dataA4,
			'val308' => $dataA8,
			'val309' => $dataA9,
		);;
	}

	/**
	* get data Cyn_Plan or Cyn_C_Plan
	*
	* @param String $projectID
	* @param String $orderNo
	* @param String $group
	* @param String $no
	* @param String $koteiNo
	* @return Array mixed
	*
	* @create 2020/11/02 Chien
	* @update
	*/
	private function getDetailPContent($projectID = '', $orderNo = '', $group = '', $no = '', $koteiNo = '') {
		$result = array(
			'Kotei' => '',
			'KKumiku' => '',
			'Floor' => '',
			'BD_Code' => '',
			'BData' => '',
			'HC' => '',
			'Days' => '',
			'N_Kotei' => '',
			'N_KKumiku' => '',
			'Jyoban' => '',
		);
		if ($projectID != '' && $orderNo != '' && $no != '' && $koteiNo != '') {
			// Cyn_Plan
			$data = null;
			if ($group == 0) {
				// find data to show
				$data = Cyn_Plan::where('ProjectID', '=', $projectID)
										->where('OrderNo', '=', $orderNo)
										->where('No', '=', $no)
										->where('KoteiNo', '=', $koteiNo)
										->first();
			}
			// Cyn_C_Plan
			if ($group == 1) {
				// find data to show
				$data = Cyn_C_Plan::where('ProjectID', '=', $projectID)
										->where('OrderNo', '=', $orderNo)
										->where('No', '=', $no)
										->where('KoteiNo', '=', $koteiNo)
										->first();
			}
			if ($data != null) {
				$result = array(
					'Kotei' => ($data->Kotei != '') ? valueUrlEncode($data->Kotei) : '',
					'KKumiku' => ($data->KKumiku != '') ? valueUrlEncode($data->KKumiku) : '',
					'Floor' => ($data->Floor != '') ? valueUrlEncode($data->Floor) : '',
					'BD_Code' => ($data->BD_Code != '') ? valueUrlEncode($data->BD_Code) : '',
					'BData' => FuncCommon::formatDecToText($data->BData),
					'HC' => FuncCommon::formatDecToText($data->HC),
					'Days' => $data->Days,
					'N_Kotei' => ($data->N_Kotei != '') ? valueUrlEncode($data->N_Kotei) : '',
					'N_KKumiku' => ($data->N_KKumiku != '') ? valueUrlEncode($data->N_KKumiku) : '',
					'Jyoban' => $data->Jyoban,
				);
			}
		}
		return $result;
	}

	/**
	* POST 項目定義共通化画面保存ボタンアクション
	*
	* @param ItemPContentsRequest 呼び出し元リクエストオブジェクト
	* @return View ビュー
	*
	* @create 2020/11/16 Chien
	*
	*/
	public function psave(ItemPContentsRequest $request) {
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
		$url .= '/'.$menuInfo->KindURL;
		$url .= '/'.$menuInfo->MenuURL;
		$url .= '/pedit';
		$url .= '?cmn1='.(isset($request->cmn1) ? $request->cmn1 : '');
		$url .= '&cmn2='.(isset($request->cmn2) ? $request->cmn2 : '');
		$url .= '&bpage='.(isset($request->bpage) ? $request->bpage : '');
		$url .= '&bsort='.(isset($request->bsort) ? $request->bsort : '');
		$url .= '&bdirection='.(isset($request->bdirection) ? $request->bdirection : '');
		$url .= '&page='.(isset($request->page) ? $request->page : '');
		$url .= '&sort='.(isset($request->sort) ? $request->sort : '');
		$url .= '&direction='.(isset($request->direction) ? $request->direction : '');
		$url .= '&val1='.(isset($request->val1) ? $request->val1 : '');
		$url .= '&val2='.(isset($request->val2) ? $request->val2 : '');
		$url .= '&val101='.(isset($request->val101) ? $request->val101 : '');
		$url .= '&val102='.(isset($request->val102) ? $request->val102 : '');
		$url .= '&val103='.(isset($request->val103) ? $request->val103 : '');
		$url .= '&val104='.(isset($request->val104) ? $request->val104 : '');
		$url .= '&val201='.(isset($request->val201) ? $request->val201 : '');
		$url .= '&val202='.(isset($request->val202) ? $request->val202 : '');
		//encode val301 -> val310
		for ($i = 1; $i <= 10; $i++) {
			$key = ($i < 10) ? 'val30'.$i : 'val3'.$i;
			$url .=  ($i < 10) ? '&val30'.$i : '&val3'.$i;
			$url .= '='.valueUrlEncode($request->$key);
		}

		$timeTrackerFuncSchem = new TimeTrackerFuncSchem();

		//process block
		$resultProcessBlock = $this->tryLock(
			$menuInfo->KindID,
			config('system_const_schem.sys_menu_id_plan'),
			$menuInfo->UserID,
			$menuInfo->SessionID,
			valueUrlDecode($request->val1),
			false
		);

		if (!is_null($resultProcessBlock)) {
			$url .= '&err1='.valueUrlEncode($resultProcessBlock);
			return redirect($url);
		}

		// begin Transaction
		try {
			DB::transaction(function() use ($validated, $request, $timeTrackerFuncSchem) {
				$data = null;
				if (valueUrlDecode($request->val103) == 0) {
					// find data to show
					$data = Cyn_Plan::where('ProjectID', '=', valueUrlDecode($request->val101))
									->where('OrderNo', '=', valueUrlDecode($request->val102))
									->where('No', '=', valueUrlDecode($request->val104))
									->where('KoteiNo', '=', valueUrlDecode($request->val202))
									->first();
				}
				// Cyn_C_Plan
				if (valueUrlDecode($request->val103) == 1) {
					// find data to show
					$data = Cyn_C_Plan::where('ProjectID', '=', valueUrlDecode($request->val101))
										->where('OrderNo', '=', valueUrlDecode($request->val102))
										->where('No', '=', valueUrlDecode($request->val104))
										->where('KoteiNo', '=', valueUrlDecode($request->val202))
										->first();
				}
				$workItemID = '';
				if ($data != null) {
					$workItemID = $data->WorkItemID;
				}

				$dataA1 = Cyn_mstKotei::where('CKind', '=', valueUrlDecode($request->val1))
										->where('Code', '=', $validated['val301'])
										->where('DelFlag', '=', 0)
										->first();
				$nameShowA1 = '';
				if ($dataA1 != null) {
					$nameShowA1 = $dataA1->Code.config('system_const.code_name_separator').$dataA1->Name;
				}

				$checkUpdatePlan = $timeTrackerFuncSchem->updatePlan(
					valueUrlDecode($request->val101),
					valueUrlDecode($request->val102),
					$workItemID,
					$request->val307,
					$nameShowA1
				);
				if ($checkUpdatePlan != null) {
					throw new CustomException($checkUpdatePlan);
				}

				// update
				$obj['Kotei'] = $request->val301;
				$obj['KKumiku'] = $request->val302;
				$obj['Floor'] = $request->val303;
				$obj['BD_Code'] = $request->val304;
				$obj['BData'] = $request->val305;
				$obj['HC'] = $request->val306;
				$obj['Days'] = $request->val307;

				// check N_KoteiNo
				$nKoteiNo = 0;
				if (isset($request->val308) && isset($request->val309)) {
					if ($request->val308 == '' && $request->val309 == '') {
						$nKoteiNo = 0;
					}
					if ($request->val308 != '' && $request->val309 != '') {
						$dataCheck = null;
						if (valueUrlDecode($request->val103) == 0) {
							// find data
							$dataCheck = Cyn_Plan::where('ProjectID', '=', valueUrlDecode($request->val101))
											->where('OrderNo', '=', valueUrlDecode($request->val102))
											->where('No', '=', valueUrlDecode($request->val104))
											->where('Kotei', '=', $request->val308)
											->where('KKumiku', '=', $request->val309)
											->whereNull('Del_Date')
											->orderBy('KoteiNo')
											->first();
						}
						// Cyn_C_Plan
						if (valueUrlDecode($request->val103) == 1) {
							// find data
							$dataCheck = Cyn_C_Plan::where('ProjectID', '=', valueUrlDecode($request->val101))
											->where('OrderNo', '=', valueUrlDecode($request->val102))
											->where('No', '=', valueUrlDecode($request->val104))
											->where('Kotei', '=', $request->val308)
											->where('KKumiku', '=', $request->val309)
											->whereNull('Del_Date')
											->orderBy('KoteiNo')
											->first();
						}
						if ($dataCheck != null) {
							$nKoteiNo = $dataCheck->KoteiNo;
						}
					}
				}

				$obj['N_KoteiNo'] = $nKoteiNo;
				$obj['N_Kotei'] = $request->val308;
				$obj['N_KKumiku'] = $request->val309;
				$obj['Jyoban'] = $request->val310;

				if (valueUrlDecode($request->val103) == 0) {
					// Cyn_Plan
					$result = Cyn_Plan::where('ProjectID', '=', valueUrlDecode($request->val101))
										->where('OrderNo', '=', valueUrlDecode($request->val102))
										->where('No', '=', valueUrlDecode($request->val104))
										->where('KoteiNo', '=', valueUrlDecode($request->val202))
										->update($obj);
				}
				if (valueUrlDecode($request->val103) == 1) {
					// Cyn_C_Plan
					$result = Cyn_C_Plan::where('ProjectID', '=', valueUrlDecode($request->val101))
										->where('OrderNo', '=', valueUrlDecode($request->val102))
										->where('No', '=', valueUrlDecode($request->val104))
										->where('KoteiNo', '=', valueUrlDecode($request->val202))
										->update($obj);
				}
			});
		} catch (CustomException $ex) {
			// error
			$url .= '&err1=' . valueUrlEncode($ex->getMessage());
			return redirect($url);
		} finally {
			$this->deleteLock(
				$menuInfo->KindID,
				config('system_const_schem.sys_menu_id_plan'),
				$menuInfo->SessionID,
				valueUrlDecode($request->val1)
			);
		}

		// redirect
		$url = url('/');
		$url .= '/'.$menuInfo->KindURL;
		$url .= '/'.$menuInfo->MenuURL;
		$url .= '/pmanage';
		$url .= '?cmn1='.(isset($request->cmn1) ? $request->cmn1 : '');
		$url .= '&cmn2='.(isset($request->cmn2) ? $request->cmn2 : '');
		$url .= '&bpage='.(isset($request->bpage) ? $request->bpage : '');
		$url .= '&bsort='.(isset($request->bsort) ? $request->bsort : '');
		$url .= '&bdirection='.(isset($request->bdirection) ? $request->bdirection : '');
		$url .= '&page='.(isset($request->page) ? $request->page : '');
		$url .= '&sort='.(isset($request->sort) ? $request->sort : '');
		$url .= '&direction='.(isset($request->direction) ? $request->direction : '');
		$url .= '&val1='.(isset($request->val1) ? $request->val1 : '');
		$url .= '&val2='.(isset($request->val2) ? $request->val2 : '');
		$url .= '&val101='.(isset($request->val101) ? $request->val101 : '');
		$url .= '&val102='.(isset($request->val102) ? $request->val102 : '');
		$url .= '&val103='.(isset($request->val103) ? $request->val103 : '');
		$url .= '&val104='.(isset($request->val104) ? $request->val104 : '');
		$url .= '&val201='.(isset($request->val201) ? $request->val201 : '');
		return redirect($url);
	}

	/**
	* 項目定義詳細閲覧画面
	*
	* @param Request 呼び出し元リクエストオブジェクト
	* @return View ビュー
	*
	* @create 2020/11/14 Chien
	* @update
	*/
	public function pshow(Request $request) {
		$menuInfo = $this->checkLogin($request, config('system_const.authority_readonly'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$this->data['menuInfo'] = $menuInfo;

		$cKind = (isset($request->val1) && trim($request->val1) != '') ? valueUrlDecode($request->val1) : '';
		$projectID = (isset($request->val101) && trim($request->val101) != '') ? valueUrlDecode($request->val101) : '';
		$orderNo = (isset($request->val102) && trim($request->val102) != '') ? valueUrlDecode($request->val102) : '';
		$group = (isset($request->val103) && trim($request->val103) != '') ? valueUrlDecode($request->val103) : '';
		$no = (isset($request->val104) && trim($request->val104) != '') ? valueUrlDecode($request->val104) : '';
		$koteiNo = (isset($request->val202) && trim($request->val202) != '') ? valueUrlDecode($request->val202) : '';

		$initSelectData = $this->loadInitDataSelectPScreen($cKind, $projectID, $orderNo, $group, $no);

		$dataShow = $this->getDetailPContent($projectID, $orderNo, $group, $no, $koteiNo);

		//request
		$this->data['request'] = $request;
		$this->data['dataSelect'] = $initSelectData;
		$this->data['itemData'] = $dataShow;
		//return view with all data
		return view('Schem/Item/pshow', $this->data);
	}
}