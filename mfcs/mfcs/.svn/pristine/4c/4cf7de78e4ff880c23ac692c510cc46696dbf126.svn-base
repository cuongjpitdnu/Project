<?php
/*
 * @PatternController.php
 *
 *
 * @create 2021/01/25 Anh
 * @update
 */
namespace App\Http\Controllers\Sches;

use App\Http\Controllers\Controller;
use App\Http\Requests\Sches\PatternContentsRequest;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use App\Librarys\MenuInfo;
use App\Librarys\MstOrgCommon;
use App\Librarys\FuncCommon;
use App\Librarys\TimeTrackerFuncSches;
use App\Models\S_mstDataPattern;
use App\Models\S_mstDataDetail;
use App\Models\Cyn_mstKotei;
use App\Models\MstFloor;
use App\Models\MstProject;
use App\Models\MstOrg;
/*
 *
 *
 * @create 2021/01/25 Anh
 * @update
 */
class PatternController extends Controller
{
	/**
	 * construct
	 * @param
	 * @return mixed
	 * @create 2021/01/25 Anh
	 */
	public function __construct(){
	}
	/**
	 *
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2021/01/25 Anh
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
	 * @create 2021/01/25 Anh
	 *
	 * @update
	 */
	private function initialize(Request $request) {
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		//create object MstOrgCommon
		$mstOrgCommon = new MstOrgCommon();

		$this->data['menuInfo'] = $menuInfo;
		$this->data['orgName'] = config('system_const.org_null_name');
		$this->data['val1'] = valueUrlEncode(0);
		$this->data['mstOrgCommon'] = $mstOrgCommon;

		//return view with all data
		return view('Sches/Pattern/index', $this->data);
	}

	/**
	 *
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2021/01/25 Anh
	 */
	public function manage(Request $request)
	{
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$mstOrgCommon = new MstOrgCommon();

		$originalError = [];
		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
		}

		$query = S_mstDataPattern::select(
										'S_mstDataPattern.ID as fld1',
										'S_mstDataPattern.Name as fld2',
										'S_mstDataPattern.BKumiku as fld3',
										'S_mstDataPattern.Kotei as fld4',
										'S_mstDataPattern.KKumiku as fld5',
										'S_mstDataPattern.Floor as fld6',
										'S_mstDataPattern.BaseData as fld7',
										'S_mstDataPattern.Updated_at as fld8',
										'Cyn_mstKotei.Name as fld41',
										'mstFloor.Name as fld61',
									)
									->leftJoin('Cyn_mstKotei', function($join) {
										$join->on('S_mstDataPattern.Kotei', '=', 'Cyn_mstKotei.Code')
											->on('S_mstDataPattern.CKind', '=', 'Cyn_mstKotei.CKind');
									})
									->leftJoin('mstFloor', 'S_mstDataPattern.Floor', '=', 'mstFloor.Code')
									->where('S_mstDataPattern.GroupID', '=', valueUrlDecode($request->val1))
									->get();

		$sort = ['fld1'];
		if (isset($request->sort) && $request->sort != '') {
			if (trim($request->sort) == 'fld1') {
				$sort = ['fld1'];
			} else {
				$sort = [$request->sort, 'fld1'];
			}
		}

		$direction = (isset($request->direction) && $request->direction != '') ?  $request->direction : 'asc';

		//pageunit != 10 -> pageunit = 10
		if(isset($request->pageunit) && in_array($request->pageunit, [config('system_const.displayed_results_1'),
																	config('system_const.displayed_results_2'),
																	config('system_const.displayed_results_3')])){
			$pageunit = $request->pageunit;
		}else{
			$pageunit = config('system_const.displayed_results_1');
		}

		$rows = $this->sortAndPagination($query, $sort, $direction, $pageunit, $request);
		//get list of pattern
		$rows->getCollection()->transform(function ($value) {
			$value['fld3'] = FuncCommon::getKumikuData($value['fld3'])[2];
			$value['fld4'] .= config('system_const.code_name_separator') . $value['fld41'];
			$value['fld5'] = FuncCommon::getKumikuData($value['fld5'])[2];
			$value['fld6'] .= config('system_const.code_name_separator') . $value['fld61'];
			switch ($value['fld7']) {
				case config('system_const_sches.basedata_code_cyn'):
					$value['fld7'] .= config('system_const.code_name_separator') . config('system_const_sches.basedata_name_cyn');
					break;
				case config('system_const_sches.basedata_code_kukaku'):
					$value['fld7'] .= config('system_const.code_name_separator') . config('system_const_sches.basedata_name_kukaku');
					break;
				case config('system_const_sches.basedata_code_syoku'):
					$value['fld7'] .= config('system_const.code_name_separator') . config('system_const_sches.basedata_name_syoku');
					break;
				case config('system_const_sches.basedata_code_kogumi'):
					$value['fld7'] .= config('system_const.code_name_separator') . config('system_const_sches.basedata_name_kogumi');
					break;
				case config('system_const_sches.basedata_code_nc'):
					$value['fld7'] .= config('system_const.code_name_separator') . config('system_const_sches.basedata_name_nc');
					break;
				case config('system_const_sches.basedata_code_kako_mage'):
					$value['fld7'] .= config('system_const.code_name_separator') . config('system_const_sches.basedata_name_kako_mage');
					break;
				case config('system_const_sches.basedata_code_kako_setsudan'):
					$value['fld7'] .= config('system_const.code_name_separator') . config('system_const_sches.basedata_name_kako_setsudan');
					break;
				default:
					break;
			}
			return $value;
		});

		$this->data['listPattern'] = $rows;
		$this->data['val1'] = $mstOrgCommon->getFullName(valueUrlDecode($request->val1));
		$this->data['menuInfo'] = $menuInfo;
		$this->data['originalError'] = $originalError;
		$this->data['request'] = $request;

		return view('Sches/Pattern/manage', $this->data);
	}

	/**
	 * コントローラの処理(削除処理)
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2021/01/28 Anh
	 */
	public function delete(Request $request)
	{
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$url = $this->getUrlRedirect('manage', $menuInfo, $request);

		$id = valueUrlDecode($request->fld1);
		$updateAt = valueUrlDecode($request->fld8);

		$deletePattern = S_mstDataPattern::where('ID', '=', $id)
							->where('Updated_at', '=', $updateAt)
							->delete();
		if ($deletePattern == 0) {
			$url .= '&err1=' . valueUrlEncode(config('message.msg_cmn_db_002'));
		}

		return redirect($url);
	}

	/**
	 *
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2021/01/29 Anh
	 */
	public function create(Request $request)
	{
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$originalError = [];

		$dataShow = array(
			'val1' => $request->val1,
			'val2' => $request->val2,
			'val3' => '',
			'val4' => '',
			'val5' => '',
			'val6' => '',
			'val7' => '',
			'val8' => '',
			'val9' => '',
			'val10' => 0,
			'val11' => '',
		);

		$dataShow['val5'] = isset($request->val5) ? valueUrlDecode($request->val5)
			: (old('val5') ? valueUrlDecode(old('val5')) : config('system_const.c_kind_chijyo'));

		$this->data['dataSelect'] = array(
			'data_4' => $this->getDataVal4(),
			'data_5' => $this->getDataVal5(),
			'data_6' => $this->getDataVal6($dataShow['val5'], false),
			'data_6_all' => $this->getDataVal6('', true),
			'data_7' => $this->getDataVal4(),
			'data_8' => $this->getDataVal8(),
			'data_9' => $this->getDataVal9(),
		);

		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
			$dataShow = array(
				'val1' => $request->val1,
				'val2' => $request->val2,
				'val3' => $request->val3,
				'val4' => $request->val4,
				'val5' => $request->val5,
				'val6' => $request->val6,
				'val7' => $request->val7,
				'val8' => $request->val8,
				'val9' => $request->val9,
				'val10' => $request->val10,
				'val11' => $request->val11,
			);
		}

		$this->data['request'] = $request;
		$this->data['itemData'] = $dataShow;
		$this->data['menuInfo'] = $menuInfo;
		$this->data['originalError'] = $originalError;

		return view('Sches/Pattern/create', $this->data);
	}

	/**
	 *
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2021/01/29 Anh
	 */
	public function edit(Request $request)
	{
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$originalError = [];

		$data = S_mstDataPattern::select(
									'ID',
									'Name',
									'GroupID',
									'CKind',
									'BKumiku',
									'Kotei',
									'KKumiku',
									'Floor',
									'BaseData',
									'Updated_at'
								)
								->where('ID', '=', valueUrlDecode($request->val2))
								->first();

		$dataShow = array(
			'val1' => $request->val1,
			'val2' => $request->val2,
			'val3' => $data->Name,
			'val4' => valueUrlEncode($data->BKumiku),
			'val5' => valueUrlEncode($data->CKind),
			'val6' => valueUrlEncode($data->Kotei),
			'val7' => valueUrlEncode($data->KKumiku),
			'val8' => valueUrlEncode($data->Floor),
			'val9' => valueUrlEncode($data->BaseData),
			'val10' => valueUrlEncode(0),
			'val11' => $data->Updated_at,
		);

		$dataVal5 = isset($request->val5) ? valueUrlDecode($request->val5)
								: valueUrlDecode(old('val5', $dataShow['val5']));

		$this->data['dataSelect'] = array(
			'data_4' => $this->getDataVal4(),
			'data_5' => $this->getDataVal5(),
			'data_6' => $this->getDataVal6($dataVal5, false),
			'data_6_all' => $this->getDataVal6('', true),
			'data_7' => $this->getDataVal4(),
			'data_8' => $this->getDataVal8(),
			'data_9' => $this->getDataVal9(),
		);

		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
			$dataShow = array(
				'val1' => $request->val1,
				'val2' => $request->val2,
				'val3' => valueUrlDecode($request->val3),
				'val4' => $request->val4,
				'val5' => $request->val5,
				'val6' => $request->val6,
				'val7' => $request->val7,
				'val8' => $request->val8,
				'val9' => $request->val9,
				'val10' => $request->val10,
				'val11' => $request->val11,
			);
		}

		$this->data['request'] = $request;
		$this->data['itemData'] = $dataShow;
		$this->data['menuInfo'] = $menuInfo;
		$this->data['originalError'] = $originalError;
		return view('Sches/Pattern/edit', $this->data);
	}

	/**
	 *
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2021/01/29 Anh
	 */
	public function show(Request $request)
	{
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_readonly'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$originalError = [];

		$data = S_mstDataPattern::select(
									'ID',
									'Name',
									'GroupID',
									'CKind',
									'BKumiku',
									'Kotei',
									'KKumiku',
									'Floor',
									'BaseData',
									'Updated_at'
								)
								->where('ID', '=', valueUrlDecode($request->val2))
								->first();

		$dataShow = array(
			'val1' => $request->val1,
			'val2' => $request->val2,
			'val3' => $data->Name,
			'val4' => valueUrlEncode($data->BKumiku),
			'val5' => valueUrlEncode($data->CKind),
			'val6' => valueUrlEncode($data->Kotei),
			'val7' => valueUrlEncode($data->KKumiku),
			'val8' => valueUrlEncode($data->Floor),
			'val9' => valueUrlEncode($data->BaseData),
			'val10' => valueUrlEncode(0),
			'val11' => $data->Updated_at,
		);

		$dataVal5 = isset($request->val5) ? valueUrlDecode($request->val5)
								: valueUrlDecode(old('val5', $dataShow['val5']));

		$this->data['dataSelect'] = array(
			'data_4' => $this->getDataVal4(),
			'data_5' => $this->getDataVal5(),
			'data_6' => $this->getDataVal6($dataVal5, false),
			'data_6_all' => $this->getDataVal6('', true),
			'data_7' => $this->getDataVal4(),
			'data_8' => $this->getDataVal8(),
			'data_9' => $this->getDataVal9(),
		);

		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
			$dataShow = array(
				'val1' => $request->val1,
				'val2' => $request->val2,
				'val3' => valueUrlDecode($request->val3),
				'val4' => $request->val4,
				'val5' => $request->val5,
				'val6' => $request->val6,
				'val7' => $request->val7,
				'val8' => $request->val8,
				'val9' => $request->val9,
				'val10' => $request->val10,
				'val11' => $request->val11,
			);
		}

		$this->data['request'] = $request;
		$this->data['itemData'] = $dataShow;
		$this->data['menuInfo'] = $menuInfo;
		$this->data['originalError'] = $originalError;
		return view('Sches/Pattern/show', $this->data);
	}

	/**
	 * get all data value 4
	 *
	 * @return Array mixed
	 *
	 * @create 2021/01/29 Anh
	 *
	 * @update
	 */
	private function getDataVal4(){
		//	select box 組区セレクトボックス
		return array(
			valueUrlEncode(config('system_const.kumiku_code_kogumi')) =>
				FuncCommon::getKumikuData(config('system_const.kumiku_code_kogumi'))[2],
			valueUrlEncode(config('system_const.kumiku_code_naicyu')) =>
				FuncCommon::getKumikuData(config('system_const.kumiku_code_naicyu'))[2],
			valueUrlEncode(config('system_const.kumiku_code_kumicyu')) =>
				FuncCommon::getKumikuData(config('system_const.kumiku_code_kumicyu'))[2],
			valueUrlEncode(config('system_const.kumiku_code_ogumi')) =>
				FuncCommon::getKumikuData(config('system_const.kumiku_code_ogumi'))[2],
			valueUrlEncode(config('system_const.kumiku_code_sogumi')) =>
				FuncCommon::getKumikuData(config('system_const.kumiku_code_sogumi'))[2],
			valueUrlEncode(config('system_const.kumiku_code_kyocyu')) =>
				FuncCommon::getKumikuData(config('system_const.kumiku_code_kyocyu'))[2],
		);
	}

	/**
	 * get all data value 5
	 *
	 * @return Array mixed
	 *
	 * @create 2021/01/29 Anh
	 *
	 * @update
	 */
	private function getDataVal5(){
		//	selectbox 対象区分セレクトボックス
		return array(
			valueUrlEncode(config('system_const.c_kind_chijyo')) => config('system_const.c_name_chijyo'),
			valueUrlEncode(config('system_const.c_kind_gaigyo')) => config('system_const.c_name_gaigyo'),
			valueUrlEncode(config('system_const.c_kind_giso')) => config('system_const.c_name_giso'),
		);
	}

	/**
	 * get all data value 6
	 *
	 * @return Array mixed
	 *
	 * @create 2021/01/29 Anh
	 *
	 * @update
	 */
	private function getDataVal6($cKind = '', $loadAll = false){
		// selectbox 工程セレクトボックス
		$query = Cyn_mstKotei::select('CKind', 'Code', 'Name')
					->where('DelFlag', '=', 0);
		$query = ($cKind !== '') ? $query->where('CKind', '=', $cKind) : $query;
		$query = $query->orderBy('Code')->orderBy('Name')->get();

		foreach ($query as &$row) {
			$name = trim($row['Code'] . config('system_const.code_name_separator') . $row['Name']);
			$row->Name = $loadAll ? htmlentities($name) : $name;
			$row->CKind = valueUrlEncode($row->CKind);
			$row->Code = valueUrlEncode($row->Code);
		}
		return $query;
	}

	/**
	 * get all data value 8
	 *
	 * @return Array mixed
	 *
	 * @create 2021/01/29 Anh
	 *
	 * @update
	 */
	private function getDataVal8(){
		// selectbox 施工棟セレクトボックス
		$query = MstFloor::select('SortNo', 'Code', 'Name')
					->where('ViewFlag', '=', 1)
					->orderBy('SortNo')
					->orderBy('Code')
					->orderBy('Name')
					->get();
		foreach ($query as &$row) {
			$name = trim($row['Code'] . config('system_const.code_name_separator') . $row['Name']);
			$row->Name = $name;
			$row->SortNo = valueUrlEncode($row->SortNo);
			$row->Code = valueUrlEncode($row->Code);
		}
		return $query;
	}

	/**
	 * get all data value 9
	 *
	 * @return Array mixed
	 *
	 * @create 2021/01/29 Anh
	 *
	 * @update
	 */
	private function getDataVal9(){
		// selectbox 基準データセレクトボックス
		return array(
			valueUrlEncode(config('system_const_sches.basedata_code_cyn')) => config('system_const_sches.basedata_code_cyn')
					. config('system_const.code_name_separator') . config('system_const_sches.basedata_name_cyn'),
			valueUrlEncode(config('system_const_sches.basedata_code_kukaku')) => config('system_const_sches.basedata_code_kukaku')
					. config('system_const.code_name_separator') . config('system_const_sches.basedata_name_kukaku'),
			valueUrlEncode(config('system_const_sches.basedata_code_syoku')) => config('system_const_sches.basedata_code_syoku')
					. config('system_const.code_name_separator') . config('system_const_sches.basedata_name_syoku'),
			valueUrlEncode(config('system_const_sches.basedata_code_kogumi')) => config('system_const_sches.basedata_code_kogumi')
					. config('system_const.code_name_separator') . config('system_const_sches.basedata_name_kogumi'),
			valueUrlEncode(config('system_const_sches.basedata_code_nc')) => config('system_const_sches.basedata_code_nc')
					. config('system_const.code_name_separator') . config('system_const_sches.basedata_name_nc'),
			valueUrlEncode(config('system_const_sches.basedata_code_kako_mage')) =>
				config('system_const_sches.basedata_code_kako_mage'). config('system_const.code_name_separator')
				. config('system_const_sches.basedata_name_kako_mage'),
			valueUrlEncode(config('system_const_sches.basedata_code_kako_setsudan')) =>
				config('system_const_sches.basedata_code_kako_setsudan') . config('system_const.code_name_separator')
				. config('system_const_sches.basedata_name_kako_setsudan'),
		);
	}

	/**
	 *
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2021/01/29 Anh
	 */
	public function save(PatternContentsRequest $request)
	{
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$date = date("Y/m/d");

		$val1  = valueUrlDecode($request->val1);

		$objTimeTrackerFuncSches = new TimeTrackerFuncSches();

		$validated = $request->validated();

		$mstOrgData = MstOrg::select('Name')
						->where('ID', '=', $val1)
						->whereDate('Sdate', '<=', $date)
						->whereDate('Edate', '>=', $date)
						->get();
		$nameMstOrg = count($mstOrgData) == 0 ? '' : $mstOrgData[0]->Name;

		// $request->method =create
		if ($request->method == 'create') {
			//beginTransaction
			$idSmstDataPattern = DB::select('SELECT NEXT VALUE FOR seq_S_mstDataPattern as patternID');
			$maxSmstDataPatternID = $idSmstDataPattern[0]->patternID;

			DB::transaction(function() use ($request, $menuInfo, $objTimeTrackerFuncSches, $val1,
											$validated, $maxSmstDataPatternID, $nameMstOrg) {

				$obj = new S_mstDataPattern;
				$obj->ID = $maxSmstDataPatternID;
				$obj->Name = $validated['val3'];
				$obj->GroupID = $val1;
				$obj->CKind = $validated['val5'];
				$obj->BKumiku = $validated['val4'];
				$obj->Kotei = $validated['val6'];
				$obj->KKumiku = $validated['val7'];
				$obj->Floor = $validated['val8'];
				$obj->BaseData = $validated['val9'];
				$obj->save();

				// 以下の表の内容で[mstProject]にデータを登録する
				$checkIsData = MstProject::where('SysKindID', '=', $menuInfo->KindID)->where('ListKind', '=', $val1)->first();
				if (!$checkIsData) {
					$idMstProject = DB::select('SELECT NEXT VALUE FOR seq_mstProject as projectID');
					$maxMstProjectID = $idMstProject[0]->projectID;
					$objProject = new MstProject;
					$objProject->Up_User = $menuInfo->UserID;
					$objProject->ID = $maxMstProjectID;
					$objProject->SysKindID = $menuInfo->KindID;
					$objProject->ListKind = $val1;
					$objProject->SerialNo = 1;
					$objProject->ProjectName = sprintf(config('system_const_sches.case_project_name'), $nameMstOrg);
					$objProject->save();

					$checkUpdateManager = $objTimeTrackerFuncSches->updateManager(
						sprintf(config('system_const_sches.case_project_name'), $nameMstOrg),
						$menuInfo->KindID . '-' . $val1 . '-' . 1, $validated['val5']);
					if (is_string($checkUpdateManager)) {
						// error
						DB::rollback();
						$url = $this->getUrlRedirect('create', $menuInfo, $request, valueUrlEncode($checkUpdateManager));
						return redirect($url);
					}
				} else {
					if ($request->val10 == valueUrlEncode(1)) {
						$checkUpdateManager = $objTimeTrackerFuncSches->updateManager(
							sprintf(config('system_const_sches.case_project_name'), $nameMstOrg),
							$menuInfo->KindID . '-' . $val1 . '-' . 1, $validated['val5']);
						if (is_string($checkUpdateManager)) {
							// error
							DB::rollback();
							$url = $this->getUrlRedirect('create', $menuInfo, $request, valueUrlEncode($checkUpdateManager));
							return redirect($url);
						}
					}
				}
			});
		}

		// $request->method = edit
		if ($request->method == 'edit') {
			//beginTransaction
			DB::transaction(function() use ($request, $menuInfo, $objTimeTrackerFuncSches, $validated, $nameMstOrg, $val1) {
				$val2  = valueUrlDecode($request->val2);
				$val11  = valueUrlDecode($request->val11);
				//assign validated data to array
				$objData['Name'] = $validated['val3'];
				$objData['CKind'] = $validated['val5'];
				$objData['BKumiku'] = $validated['val4'];
				$objData['Kotei'] = $validated['val6'];
				$objData['KKumiku'] = $validated['val7'];
				$objData['Floor'] = $validated['val8'];
				$objData['BaseData'] = $validated['val9'];

				$result = S_mstDataPattern::query()
					->Where('ID', '=', $val2)
					->Where('Updated_at', '=', $val11)
					->update($objData);

				if ($result == 0) {
					$url = $this->getUrlRedirect('edit', $menuInfo, $request, valueUrlEncode(config('message.msg_cmn_db_002')), true);
					return redirect($url);
				}

				if ($request->val10 == valueUrlEncode(1)) {
					$checkUpdateManager = $objTimeTrackerFuncSches->updateManager(
						sprintf(config('system_const_sches.case_project_name'), $nameMstOrg),
						$menuInfo->KindID . '-' . $val1 . '-' . 1, $validated['val5']);
					if (is_string($checkUpdateManager)) {
						// error
						DB::rollback();
						$url = $this->getUrlRedirect('edit', $menuInfo, $request, valueUrlEncode($checkUpdateManager), true);
						return redirect($url);
					}
				}
			});

		}

		$url = $this->getUrlRedirect('manage', $menuInfo, $request);
		return redirect($url);
	}

	/**
	 * get url redirect
	 *
	 * @param string $method, $err
	 * @param array $menuInfo
	 * @param Request
	 * @return string
	 *
	 * @create 2021/01/29 Anh
	 *
	 * @update
	 */
	private function getUrlRedirect($method = 'manage', $menuInfo, Request $request, $err = '', $hasEdit = false){
		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/' . $method;
		$url .= '?cmn1=' . valueUrlEncode($menuInfo->KindID);
		$url .= '&cmn2=' . valueUrlEncode($menuInfo->MenuID);
		$url .= '&page=' . $request->page;
		$url .= '&pageunit=' . $request->pageunit;
		$url .= '&sort=' . $request->sort;
		$url .= '&direction=' . $request->direction;
		$url .= '&val1=' . $request->val1;
		if ($hasEdit) {
			$url .= '&val2=' . $request->val2;
			$url .= '&val11=' . $request->val11;
		}
		if ($method !== 'manage') {
			for ($i = 3; $i <= 10; $i++) {
				$key = 'val'. $i;
				$url .= '&val'. $i . '=' . valueUrlEncode($request->$key);
			};
		}
		$url .= ($err !== '') ? '&err1=' . $err : '';
		return $url;
	}

	/**
	 *
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2021/02/02 Anh
	 */
	public function pmanage(Request $request)
	{
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$mstOrgCommon = new MstOrgCommon();

		$originalError = [];
		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
		}

		$query = S_mstDataDetail::select(
									'S_mstDataDetail.ID',
									'S_mstDataDetail.Name',
									'S_mstDataDetail.DistCode',
									'S_mstDataDetail.AcMac',
									'S_mstDataDetail.Item',
									'S_mstDataDetail.BD_Code',
									'S_mstDataDetail.Kumiku',
									'S_mstDataDetail.KeshiPattern',
									'S_mstDataDetail.KeshiCode',
									'S_mstDataDetail.SortNo',
									'S_mstDataDetail.Updated_at',
									'mstDist.Name as distName',
									'mstMac.Name as macName',
									'mstBDCode.Name as bdCodeName',
								)
								->leftJoin('mstDist', 'S_mstDataDetail.DistCode', '=', 'mstDist.Code')
								->leftJoin('mstMac', 'S_mstDataDetail.AcMac', '=', 'mstMac.Code')
								->leftJoin('mstBDCode', 'S_mstDataDetail.BD_Code', '=', 'mstBDCode.Code')
								->where('S_mstDataDetail.PatternID', '=', valueUrlDecode($request->val2))
								->get();

		$sort = ['SortNo', 'ID'];
		if (isset($request->sort) && $request->sort != '') {
			if (trim($request->sort) == 'ID') {
				$sort = ['ID'];
			} else {
				$sort = [$request->sort, 'ID'];
			}
		}

		$direction = (isset($request->direction) && $request->direction != '') ?  $request->direction : 'asc';

		//pageunit != 10 -> pageunit = 10
		if(isset($request->pageunit) && in_array($request->pageunit, [config('system_const.displayed_results_1'),
																	config('system_const.displayed_results_2'),
																	config('system_const.displayed_results_3')])){
			$pageunit = $request->pageunit;
		}else{
			$pageunit = config('system_const.displayed_results_1');
		}

		$rows = $this->sortAndPagination($query, $sort, $direction, $pageunit, $request);

		//get list of pattern
		$rows->getCollection()->transform(function ($value) {
			$value['Kumiku'] = FuncCommon::getKumikuData($value['Kumiku'])[2];
			$value['DistCode'] .= config('system_const.code_name_separator') . $value['distName'];
			$value['AcMac'] .= config('system_const.code_name_separator') . $value['macName'];
			$value['BD_Code'] .= config('system_const.code_name_separator') . $value['bdCodeName'];
			if ($value['KeshiPattern'] == config('system_const_sches.keshipattern_code_keshikomi')) {
				$value['KeshiPattern'] .= config('system_const.code_name_separator')
					. config('system_const_sches.keshipattern_name_keshikomi');
			} elseif ($value['KeshiPattern'] == config('system_const_sches.keshipattern_code_shintyoku')) {
				$value['KeshiPattern'] .= config('system_const.code_name_separator')
					. config('system_const_sches.keshipattern_name_shintyoku');
			}
			if ($value['KeshiCode'] == config('system_const_sches.keshicode_code_hr')) {
				$value['KeshiCode'] .= config('system_const.code_name_separator')
					. config('system_const_sches.keshicode_name_hr');
			} elseif ($value['KeshiCode'] == config('system_const_sches.keshicode_code_bdata')) {
				$value['KeshiCode'] .= config('system_const.code_name_separator')
					. config('system_const_sches.keshicode_name_bdata');
			}
			return $value;
		});

		$dataPattern = S_mstDataPattern::select('Name')->where('ID', valueUrlDecode($request->val2))->first();

		$this->data['listPattern'] = $rows;
		$this->data['val1'] = $mstOrgCommon->getFullName(valueUrlDecode($request->val1));
		$this->data['patternName'] = (!is_null($dataPattern)) ? $dataPattern->Name : '';
		$this->data['menuInfo'] = $menuInfo;
		$this->data['originalError'] = $originalError;
		$this->data['request'] = $request;

		return view('Sches/Pattern/pmanage', $this->data);
	}

	/**
	 * コントローラの処理(削除処理)
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2021/02/02 Anh
	 */
	public function deletedetail(Request $request)
	{
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/pmanage';
		$url .= '?cmn1=' . valueUrlEncode($menuInfo->KindID);
		$url .= '&cmn2=' . valueUrlEncode($menuInfo->MenuID);
		$url .= '&page=' . $request->page;
		$url .= '&pageunit=' . $request->pageunit;
		$url .= '&sort=' . $request->sort;
		$url .= '&direction=' . $request->direction;
		$url .= '&val1=' . $request->val1;
		$url .= '&val2=' . $request->val2;

		$id = valueUrlDecode($request->ID);
		$updateAt = valueUrlDecode($request->Updated_at);

		$deleteFlag = S_mstDataDetail::where('ID', '=', $id)
							->where('Updated_at', '=', $updateAt)
							->where('PatternID', '=', valueUrlDecode($request->val2))
							->delete();
		if ($deleteFlag == 0) {
			$url .= '&err1=' . valueUrlEncode(config('message.msg_cmn_db_002'));
		}

		return redirect($url);
	}

}