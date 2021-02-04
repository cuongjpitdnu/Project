<?php
/*
 * @PatternController.php
 * 工程パターン管理画面コントローラーファイル
 *
 * @create 2020/09/01 Chien
 *
 * @update
 */

namespace App\Http\Controllers\Schem;

use App\Http\Controllers\Controller;
use App\Repositories\SchemCyn_mstKotei_STR_P\SchemCyn_mstKotei_STR_PRepositoryInterface;
use App\Repositories\SchemCyn_mstKotei_STR_C\SchemCyn_mstKotei_STR_CRepositoryInterface;
use Illuminate\Http\Request;
use App\Http\Requests\Schem\PatternContentsRequest;
use App\Http\Requests\Schem\PatternDetailContentsRequest;
use Illuminate\Support\Facades\DB;
use Illuminate\Database\QueryException;
use Illuminate\Pagination\LengthAwarePaginator;
use App\Librarys\FuncCommon;
use App\Librarys\MenuInfo;
use App\Librarys\MissingUpdateException;
use App\Models\Cyn_mstKotei_STR_P;
use App\Models\Cyn_mstKotei_STR_C;
use App\Models\Cyn_mstKotei;
use App\Models\MstFloor;
use App\Models\MstBDCode;

/*
 * 工程パターン管理画面コントローラー
 *
 * @create 2020/09/01 Chien
 *
 * @update
 */
class PatternController extends Controller
{
	protected $mobjSchemCynMstKoteiSTRP;
	protected $mobjSchemCynMstKoteiSTRC;
	/**
	 * construct
	 * @param SchemCyn_mstKotei_STR_PRepositoryInterface
	 * @param SchemCyn_mstKotei_STR_CRepositoryInterface
	 * @return mixed
	 * @create 2020/09/01 Chien
	 * @update
	 */
	public function __construct(
		SchemCyn_mstKotei_STR_PRepositoryInterface $repositoryP,
		SchemCyn_mstKotei_STR_CRepositoryInterface $repositoryC) {
		//create MstFloorRepository instance
		$this->mobjSchemCynMstKoteiSTRP = $repositoryP;
		$this->mobjSchemCynMstKoteiSTRC = $repositoryC;
	}

	/**
	 * 工程パターン管理画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/09/01 Chien
	 * @update
	 */
	public function index(Request $request) {
		return $this->initialize($request);
	}

	private function initialize(Request $request) {
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$this->data['menuInfo'] = $menuInfo;

		//get list of pattern
		$query = Cyn_mstKotei_STR_P::select('Code as fld1', 'Name as fld2', 'CKind as fld3', 'DelFlag as fld4')->get();

		$sort = ['fld1', 'fld3'];
		if (isset($request->sort) && $request->sort != '') {
			if (trim($request->sort) == 'fld1') {
				$sort = ['fld1', 'fld3'];
			} else if (trim($request->sort) == 'fld3') {
				$sort = ['fld3', 'fld1'];
			} else {
				$sort = [$request->sort, 'fld1', 'fld3'];
			}
		}
		$direction = (isset($request->direction) && trim($request->direction) != '') ? $request->direction : 'asc';
		//pageunit != 10 -> pageunit = 10
		$pageunit = in_array($request->pageunit, [config('system_const.displayed_results_1'),
												config('system_const.displayed_results_2'),
												config('system_const.displayed_results_3')])
					? $request->pageunit : config('system_const.displayed_results_1');

		$rows = $this->sortAndPagination($query, $sort, $direction, $pageunit, $request);

		$this->data['rows'] = $rows;

		//request
		$this->data['request'] = $request;
		//return view with all data
		return view('Schem/Pattern/index', $this->data);
	}

	/**
	 * GET 工程定義登録画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/09/14 Chien
	 * @update
	 */
	public function create(Request $request) {
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$this->data['menuInfo'] = $menuInfo;

		//initialize $koteiPatternData
		$koteiPatternData = [];
		//initialize $originalError
		$originalError = [];

		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
			$koteiPatternData['val101'] = valueUrlDecode($request->val101);
			$koteiPatternData['val102'] = valueUrlDecode($request->val102);
			$koteiPatternData['val103'] = valueUrlDecode($request->val103);
			$koteiPatternData['val104'] = valueUrlDecode($request->val104);
		}

		$this->data['request'] = $request;
		$this->data['originalError'] = $originalError;
		$this->data['itemData'] = $koteiPatternData;

		return view('Schem/Pattern/create', $this->data);
	}

	/**
	 * GET 工程定義編集画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/09/14 Chien
	 * @update
	 */
	public function edit(Request $request) {
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$this->data['menuInfo'] = $menuInfo;

		//initialize $koteiPatternData
		$koteiPatternData = [];
		//initialize $originalError
		$originalError = [];

		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
			$koteiPatternData['val101'] = valueUrlDecode($request->val101);
			$koteiPatternData['val102'] = valueUrlDecode($request->val102);
			$koteiPatternData['val103'] = valueUrlDecode($request->val103);
			$koteiPatternData['val104'] = valueUrlDecode($request->val104);
			$koteiPatternData['val105'] = valueUrlDecode($request->val105);
		} else {
			$val1 = (isset($request->val1)) ? $request->val1 : $request->val101;
			$val3 = (isset($request->val3)) ? $request->val3 : $request->val103;
			$cKind = valueUrlDecode($val3);	// CKind
			$code = valueUrlDecode($val1);	// Code

			$data = $this->mobjSchemCynMstKoteiSTRP->findWithMultiKey($cKind, $code);
			if (!is_null($data)) {
				// $koteiPatternData = $data->toArray();
				$koteiPatternData['val101'] = $data->Code;
				$koteiPatternData['val102'] = $data->Name;
				$koteiPatternData['val103'] = $data->CKind;
				$koteiPatternData['val104'] = $data->DelFlag;
				$koteiPatternData['val105'] = $data->Updated_at;
			}
		}

		$this->data['request'] = $request;
		$this->data['originalError'] = $originalError;
		$this->data['itemData'] = $koteiPatternData;

		return view('Schem/Pattern/edit', $this->data);
	}

	/**
	 * GET 工程定義詳細画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/09/14 Chien
	 * @update
	 */
	public function show(Request $request) {
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_readonly'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$this->data['menuInfo'] = $menuInfo;

		//initialize $koteiPatternData
		$koteiPatternData = [];
		//initialize $originalError
		$originalError = [];

		$cKind = valueUrlDecode($request->val3);	// CKind
		$code = valueUrlDecode($request->val1);		// Code

		$data = $this->mobjSchemCynMstKoteiSTRP->findWithMultiKey($cKind, $code);
		if (!is_null($data)) {
			$koteiPatternData['val101'] = $data->Code;
			$koteiPatternData['val102'] = $data->Name;
			$koteiPatternData['val103'] = $data->CKind;
			$koteiPatternData['val104'] = $data->DelFlag;
			$koteiPatternData['val105'] = $data->Updated_at;
		}

		$this->data['request'] = $request;
		$this->data['originalError'] = $originalError;
		$this->data['itemData'] = $koteiPatternData;

		return view('Schem/Pattern/show', $this->data);
	}

	/**
	 * POST パターンマスターの保存ボタンアクション
	 *
	 * @param PatternContentsRequest 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/09/14 Chien
	 * @update
	 */
	public function save(PatternContentsRequest $request) {
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		//validate form
		$validated = $request->validated();

		if ($request->method == 'create') {
			try {
				$objPattern = new Cyn_mstKotei_STR_P;
				//beginTransaction
				DB::transaction(function() use ($objPattern, $validated) {
					$objPattern->CKind = $validated['val103'];
					$objPattern->Code = $validated['val101'];
					$objPattern->Name = $validated['val102'];
					$objPattern->DelFlag = $validated['val104'];
					$objPattern->save();
				});
			} catch (QueryException $e) {
				if ($request->method == 'create' && $e->getCode() == '23000') {
					//error code is 23000
					$originalError = config('message.msg_cmn_db_009');
					$url = url('/');
					$url .= '/' . $menuInfo->KindURL;
					$url .= '/' . $menuInfo->MenuURL;
					$url .= '/create';
					$url .= '?cmn1=' . $request->cmn1;
					$url .= '&cmn2=' . $request->cmn2;
					$url .= '&page=' . $request->page;
					$url .= '&pageunit=' . $request->pageunit;
					$url .= '&sort=' . $request->sort;
					$url .= '&direction=' . $request->direction;
					//encode val101 -> val104
					for ($i = 1; $i <= 4; $i++) {
						$key = 'val10'.$i;
						$url .= '&val10'.$i.'=' . valueUrlEncode($request->$key);
					}
					$url .= '&err1=' . valueUrlEncode($originalError);
					//redirect to $url
					return redirect($url);
				}
				throw $e;
			}
		}

		if ($request->method == 'edit') {
			try {
				//assign validated data to array
				$koteiPatternObjData['Name'] = $validated['val102'];
				$koteiPatternObjData['DelFlag'] = $validated['val104'];

				$result = Cyn_mstKotei_STR_P::where('CKind', $validated['val103'])
											->Where('Code', $validated['val101'])
											->Where('Updated_at', valueUrlDecode($request->val105))
											->update($koteiPatternObjData);

				if (!$result) {
					throw new MissingUpdateException(config('message.msg_cmn_db_002'));
				}
			} catch (MissingUpdateException $e) {
				//update failed, show error
				$originalError = $e->returnMessage;
				$url = url('/');
				$url .= '/' . $menuInfo->KindURL;
				$url .= '/' . $menuInfo->MenuURL;
				$url .= '/edit';
				$url .= '?cmn1=' . $request->cmn1;
				$url .= '&cmn2=' . $request->cmn2;
				$url .= '&page=' . $request->page;
				$url .= '&pageunit=' . $request->pageunit;
				$url .= '&sort=' . $request->sort;
				$url .= '&direction=' . $request->direction;
				//encode val1 -> val4
				for ($i = 1; $i <= 5; $i++) {
					$key = 'val10'.$i;
					if ($i == 5) {
						$url .= '&val10'.$i.'=' . $request->$key;
					} else {
						$url .= '&val10'.$i.'=' . valueUrlEncode($request->$key);
					}
				}
				$url .= '&err1=' . valueUrlEncode($originalError);
				return redirect($url);
			}
		}

		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/index';
		$url .= '?cmn1=' . $request->cmn1;
		$url .= '&cmn2=' . $request->cmn2;
		$url .= '&page=' . $request->page;
		$url .= '&pageunit=' . $request->pageunit;
		$url .= '&sort=' . $request->sort;
		$url .= '&direction=' . $request->direction;

		//everything is ok
		return redirect($url);
	}

	/**
	 * 工程パターン共通化画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View 工程パターン共通化画面
	 *
	 * @create 2020/09/14 Chien
	 * @update 2020/12/31
	 */
	public function indexDetail(Request $request) {
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$this->data['menuInfo'] = $menuInfo;

		//initialize $koteiPatternData
		$koteiSTRPData = [];
		$koteiSTRCData = [];
		//initialize $originalError
		$originalError = [];

		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
		}

		$val1 = (isset($request->val1)) ? $request->val1 : $request->val101;
		$val3 = (isset($request->val3)) ? $request->val3 : $request->val103;
		$cKind = valueUrlDecode($val3);	// CKind
		$code = valueUrlDecode($val1);	// Code

		$koteiSTRPData = $this->mobjSchemCynMstKoteiSTRP->findWithMultiKey($cKind, $code);

		// query builder
		$koteiSTRCData = Cyn_mstKotei_STR_C::select(
												'Cyn_mstKotei_STR_C.KKumiku as fld1',
												'Cyn_mstKotei_STR_C.Kotei as fld2',
												'Cyn_mstKotei_STR_C.Days as fld3',
												'Cyn_mstKotei_STR_C.Floor as fld4',
												'Cyn_mstKotei_STR_C.BD_Code as fld5',
												'Cyn_mstKotei_STR_C.N_KKumiku as fld6',
												'Cyn_mstKotei_STR_C.N_Kotei as fld7',
												'Cyn_mstKotei_STR_C.No',
												'Cyn_mstKotei_STR_C.Updated_at',
												'Cyn_mstKotei.Name as mstKoteiName',
												'mstFloor.Name as mstFloorName',
												'mstBDCode.Name as mstBDCodeName',
											)
											->leftJoin('Cyn_mstKotei', function($join) {
												$join->on('Cyn_mstKotei.Code', '=', 'Cyn_mstKotei_STR_C.Kotei')
													->on('Cyn_mstKotei.CKind', '=', 'Cyn_mstKotei_STR_C.CKind');
											})
											->leftJoin('mstFloor', 'mstFloor.Code', '=', 'Cyn_mstKotei_STR_C.Floor')
											->leftJoin('mstBDCode', 'mstBDCode.Code', '=', 'Cyn_mstKotei_STR_C.BD_Code')
											->where('Cyn_mstKotei_STR_C.CKind', '=', $cKind)
											->where('Cyn_mstKotei_STR_C.Code', '=', $code);

		$koteiSTRCData = $koteiSTRCData->get();

		if ($koteiSTRCData != null) {
			foreach ($koteiSTRCData as &$row) {
				// KKumiku as fld1
				if ($row->fld1 != "") {
					$data = FuncCommon::getKumikuData($row->fld1);
					$row->fld1 = is_array($data) ? $data[2] : '';
				}

				// Kotei as fld2
				$row->fld2 = $row->fld2.config('system_const.code_name_separator').(($row->mstKoteiName != "") ?
																					$row->mstKoteiName : '');

				// Days as fld3
				$row->fld3 = FuncCommon::formatDecToChar($row->fld3, 0);

				// Floor as fld4
				if ($row->fld4 == '') {
					$row->fld4 = '';
				} else {
					$row->fld4 = $row->fld4.config('system_const.code_name_separator').(($row->mstFloorName != "") ?
																						$row->mstFloorName : '-');
				}

				// BD_Code as fld5
				if ($row->fld5 == '') {
					$row->fld5 = '';
				} else {
					$row->fld5 = $row->fld5.config('system_const.code_name_separator').(($row->mstBDCodeName != "") ?
																						$row->mstBDCodeName : '-');
				}

				// N_KKumiku as fld6
				if ($row->fld6 != "") {
					$data = FuncCommon::getKumikuData($row->fld6);
					$row->fld6 = is_array($data) ? $data[2] : '';
				}

				// N_Kotei as fld7
				if ($row->fld7 == '') {
					$row->fld7 = '';
				} else {
					// get Name
					$queryMstKotei = Cyn_mstKotei::select('Cyn_mstKotei.Name')
									->leftJoin('Cyn_mstKotei_STR_C', function($join) {
										$join->on('Cyn_mstKotei.Code', '=', 'Cyn_mstKotei_STR_C.N_Kotei')
											->on('Cyn_mstKotei.CKind', '=', 'Cyn_mstKotei_STR_C.CKind');
									})
									->where('Cyn_mstKotei.CKind', '=', trim($cKind))
									->where('Cyn_mstKotei.Code', '=', trim($row->fld7))->first();
					$row->fld7 = $row->fld7.config('system_const.code_name_separator').(($queryMstKotei != null)
								? (($queryMstKotei->Name != '') ? $queryMstKotei->Name : '-') : '-');
				}
			}
		}

		// Handling sort
		// update rev7
		$sort = ['No'];
		if (isset($request->sort) && $request->sort != '') {
			$sort = [$request->sort, 'No'];
		}
		$direction = (isset($request->direction)) ?  $request->direction : 'asc';
		$pageunit = in_array($request->pageunit, [config('system_const.displayed_results_1'),
											config('system_const.displayed_results_2'),
											config('system_const.displayed_results_3')])
								? $request->pageunit : config('system_const.displayed_results_1');

		$rows = $this->sortAndPagination($koteiSTRCData, $sort, $direction, $pageunit, $request);

		$this->data['request'] = $request;
		$this->data['originalError'] = $originalError;
		$this->data['parent'] = $koteiSTRPData;
		$this->data['rows'] = $rows;

		return view('Schem/Pattern/indexdetail', $this->data);
	}

	/**
	 * POST パターン詳細マスター削除ボタンアクション
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View 工程パターン共通化画面
	 *
	 * @create 2020/09/14 Chien
	 * @update
	 */
	public function deleteDetail(Request $request) {
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
		$url .= '/indexdetail';
		$url .= '?cmn1=' . $request->cmn1;
		$url .= '&cmn2=' . $request->cmn2;
		$url .= '&page=' . $request->page;
		$url .= '&pageunit=' . $request->pageunit;
		$url .= '&sort=' . $request->sort;
		$url .= '&direction=' . $request->direction;
		$url .= '&val1=' . $request->val1;
		$url .= '&val3=' . $request->val3;
		$url .= '&val101=' . $request->val101;
		$url .= '&val102=' . $request->val102;

		$originalError = '';
		try {
			$code = valueUrlDecode($request->val1);
			$cKind = valueUrlDecode($request->val3);
			$no = valueUrlDecode($request->val101);
			$updatedAt = valueUrlDecode($request->val102);

			DB::transaction(function() use ($code, $cKind, $no, $updatedAt, &$errorMessage) {
				$deleteFlag = Cyn_mstKotei_STR_C::where('Code', '=', $code)
												->where('CKind', '=', $cKind)
												->where('No', '=', $no)
												->where('Updated_at', '=', $updatedAt)
												->delete();
				if ($deleteFlag == 0) {
					throw new MissingUpdateException(valueUrlEncode(config('message.msg_cmn_db_002')));
				}
			});
		} catch (MissingUpdateException $e) {
			$originalError = $e->returnMessage;
			$url .= '&err1=' . $originalError;
		}
		return redirect($url);
	}

	/**
	 * 工程パターン詳細登録画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View 工程パターン詳細登録画面
	 *
	 * @create 2020/09/14 Chien
	 * @update
	 */
	public function createDetail(Request $request) {
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$this->data['menuInfo'] = $menuInfo;

		//initialize $dataMstKoteiSTRC
		$dataMstKoteiSTRC = [];
		//initialize $originalError
		$originalError = [];

		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
			$dataMstKoteiSTRC['val201'] = valueUrlDecode($request->val201);
			$dataMstKoteiSTRC['val202'] = FuncCommon::formatDecToText(valueUrlDecode($request->val202));
			$dataMstKoteiSTRC['val203'] = valueUrlDecode($request->val203);
			$dataMstKoteiSTRC['val204'] = valueUrlDecode($request->val204);
			$dataMstKoteiSTRC['val205'] = valueUrlDecode($request->val205);
			$dataMstKoteiSTRC['val206'] = valueUrlDecode($request->val206);
			$dataMstKoteiSTRC['val207'] = valueUrlDecode($request->val207);
			$dataMstKoteiSTRC['val208'] = valueUrlDecode($request->val208);
		}

		$cKind = valueUrlDecode($request->val3);		// CKind
		$code = valueUrlDecode($request->val1);			// Code
		$dataSelect = $this->loadDataSelect($request, $cKind, $code);

		$this->data['request'] = $request;
		$this->data['originalError'] = $originalError;
		$this->data['dataSelect'] = array(
			'val201' => $dataSelect['val201'],	// val201
			'val203' => $dataSelect['val203'],	// val203
			'val204' => $dataSelect['val204'],	// val204
			'val205' => $dataSelect['val205'],	// val205
			'val206' => $dataSelect['val206'],	// val206
			'val208' => $dataSelect['val208'],	// val208
		);
		$this->data['itemData'] = $dataMstKoteiSTRC;

		return view('Schem/Pattern/createdetail', $this->data);
	}

	/**
	 * 工程パターン詳細編集画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View 工程パターン詳細編集画面
	 *
	 * @create 2020/09/14 Chien
	 * @update
	 */
	public function editDetail(Request $request) {
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$this->data['menuInfo'] = $menuInfo;

		//initialize $dataMstKoteiSTRC
		$dataMstKoteiSTRC = [];
		//initialize $originalError
		$originalError = [];

		$cKind = valueUrlDecode($request->val3);		// CKind
		$code = valueUrlDecode($request->val1);			// Code
		$no = valueUrlDecode($request->val101);			// No
		$updatedAt = valueUrlDecode($request->val102);	// Updated_at

		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
			$dataMstKoteiSTRC['val201'] = valueUrlDecode($request->val201);
			$dataMstKoteiSTRC['val202'] = FuncCommon::formatDecToText(valueUrlDecode($request->val202));
			$dataMstKoteiSTRC['val203'] = valueUrlDecode($request->val203);
			$dataMstKoteiSTRC['val204'] = valueUrlDecode($request->val204);
			$dataMstKoteiSTRC['val205'] = valueUrlDecode($request->val205);
			$dataMstKoteiSTRC['val206'] = valueUrlDecode($request->val206);
			$dataMstKoteiSTRC['val207'] = valueUrlDecode($request->val207);
			$dataMstKoteiSTRC['val208'] = valueUrlDecode($request->val208);
		} else {
			$data = $this->mobjSchemCynMstKoteiSTRC->findWithMultiKey($cKind, $code, $no);
			if ($data != null) {
				$data = $data->first();
				$dataMstKoteiSTRC['val201'] = $data->KKumiku;
				$dataMstKoteiSTRC['val202'] = $data->Days;
				$dataMstKoteiSTRC['val203'] = $data->Kotei;
				$dataMstKoteiSTRC['val204'] = $data->Floor;
				$dataMstKoteiSTRC['val205'] = $data->BD_Code;
				$dataMstKoteiSTRC['val206'] = $data->N_KKumiku;
				$dataMstKoteiSTRC['val207'] = $data->N_Link;
				$dataMstKoteiSTRC['val208'] = $data->N_Kotei;
			}
		}

		$dataSelect = $this->loadDataSelect($request, $cKind, $code);

		$this->data['request'] = $request;
		$this->data['originalError'] = $originalError;
		$this->data['dataSelect'] = array(
			'val201' => $dataSelect['val201'],	// val201
			'val203' => $dataSelect['val203'],	// val203
			'val204' => $dataSelect['val204'],	// val204
			'val205' => $dataSelect['val205'],	// val205
			'val206' => $dataSelect['val206'],	// val206
			'val208' => $dataSelect['val208'],	// val208
		);
		$this->data['itemData'] = $dataMstKoteiSTRC;

		return view('Schem/Pattern/editdetail', $this->data);
	}

	/**
	 * 工程パターン詳細閲覧画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View 工程パターン詳細閲覧画面
	 *
	 * @create 2020/09/14 Chien
	 * @update
	 */
	public function showDetail(Request $request) {
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_readonly'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$this->data['menuInfo'] = $menuInfo;

		//initialize $koteiPatternData
		$koteiPatternData = [];
		//initialize $originalError
		$originalError = [];

		$cKind = valueUrlDecode($request->val3);		// CKind
		$code = valueUrlDecode($request->val1);			// Code
		$no = valueUrlDecode($request->val101);			// No

		$data = $this->mobjSchemCynMstKoteiSTRC->findWithMultiKey($cKind, $code, $no);
		if ($data != null) {
			$data = $data->first();
			$dataMstKoteiSTRC['val201'] = $data->KKumiku;
			$dataMstKoteiSTRC['val202'] = $data->Days;
			$dataMstKoteiSTRC['val203'] = $data->Kotei;
			$dataMstKoteiSTRC['val204'] = $data->Floor;
			$dataMstKoteiSTRC['val205'] = $data->BD_Code;
			$dataMstKoteiSTRC['val206'] = $data->N_KKumiku;
			$dataMstKoteiSTRC['val207'] = $data->N_Link;
			$dataMstKoteiSTRC['val208'] = $data->N_Kotei;
		}

		$dataSelect = $this->loadDataSelect($request, $cKind, $code);

		$this->data['request'] = $request;
		$this->data['originalError'] = $originalError;
		$this->data['dataSelect'] = array(
			'val201' => $dataSelect['val201'],	// val201
			'val203' => $dataSelect['val203'],	// val203
			'val204' => $dataSelect['val204'],	// val204
			'val205' => $dataSelect['val205'],	// val205
			'val206' => $dataSelect['val206'],	// val206
			'val208' => $dataSelect['val208'],	// val208
		);
		$this->data['itemData'] = $dataMstKoteiSTRC;

		return view('Schem/Pattern/showdetail', $this->data);
	}

	/**
	 * POST パターン詳細マスター保存ボタンアクション
	 *
	 * @param PatternDetailContentsRequest 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/09/14 Chien
	 * @update 2020/10/15 Chien
	 */
	public function saveDetail(PatternDetailContentsRequest $request) {
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		//validate form
		$validated = $request->validated();

		$cKind = valueUrlDecode($request->val3);	// CKind
		$code = valueUrlDecode($request->val1);		// Code

		$nNo = 0;
		if ($request->val206 != '') {
			$data = Cyn_mstKotei_STR_C::query()
				->where('Cyn_mstKotei_STR_C.CKind', '=', $cKind)
				->where('Cyn_mstKotei_STR_C.Code', '=', $code)
				->where('Cyn_mstKotei_STR_C.Kotei', '=', $request->val208)
				->where('Cyn_mstKotei_STR_C.KKumiku', '=', $request->val206)
				->first();

			if ($data != null) {
				$nNo = $data->No;
			}
		}

		if ($request->method == 'create') {
			$noNew = 1;
			// get MAX No
			$data = Cyn_mstKotei_STR_C::selectRaw('MAX(No) as MaxNo')
					->where('Cyn_mstKotei_STR_C.CKind', '=', $cKind)
					->where('Cyn_mstKotei_STR_C.Code', '=', $code)
					->first();
			if ($data != null) {
				$noNew = $data->MaxNo + 1;
			}

			try {
				$objPattern = new Cyn_mstKotei_STR_C;
				//beginTransaction
				DB::transaction(function() use ($objPattern, $validated, $request, $cKind, $code, $noNew, $nNo) {
					$objPattern->CKind = $cKind;
					$objPattern->Code = $code;
					$objPattern->No = $noNew;
					$objPattern->Kotei = $validated['val203'];
					$objPattern->KKumiku = $validated['val201'];
					if ($validated['val202'] != null) {
						$objPattern->Days = $validated['val202'];
					}
					$objPattern->Floor = $validated['val204'];
					$objPattern->BD_Code = $validated['val205'];
					$objPattern->N_No = $nNo;
					$objPattern->N_Kotei = $validated['val208'];
					$objPattern->N_KKumiku = $validated['val206'];
					if ($validated['val207'] != null) {
						$objPattern->N_Link = $validated['val207'];
					}
					$objPattern->save();
				});
			} catch (QueryException $e) {
				if ($request->method == 'create' && $e->getCode() == '23000') {
					//error code is 23000
					$originalError = config('message.msg_cmn_db_002');
					$url = url('/');
					$url .= '/' . $menuInfo->KindURL;
					$url .= '/' . $menuInfo->MenuURL;
					$url .= '/createdetail';
					$url .= '?cmn1=' . $request->cmn1;
					$url .= '&cmn2=' . $request->cmn2;
					$url .= '&page=' . $request->page;
					$url .= '&pageunit=' . $request->pageunit;
					$url .= '&sort=' . $request->sort;
					$url .= '&direction=' . $request->direction;
					$url .= '&val1=' . $request->val1;
					$url .= '&val3=' . $request->val3;
					//encode val101 -> val104
					for ($i = 1; $i <= 8; $i++) {
						$key = 'val20'.$i;
						$url .= '&val20'.$i.'=' . valueUrlEncode($request->$key);
					}
					$url .= '&err1=' . valueUrlEncode($originalError);
					//redirect to $url
					return redirect($url);
				}
			}
		}

		if ($request->method == 'edit') {
			try {
				//assign validated data to array
				$koteiPatternObjData['KKumiku'] = $validated['val201'];
				$koteiPatternObjData['Days'] = ($validated['val202'] != null) ? $validated['val202'] : 1;
				$koteiPatternObjData['Kotei'] = $validated['val203'];
				$koteiPatternObjData['Floor'] = $validated['val204'];
				$koteiPatternObjData['BD_Code'] = $validated['val205'];
				$koteiPatternObjData['N_KKumiku'] = $validated['val206'];
				$koteiPatternObjData['N_Link'] = ($validated['val207'] != null) ? $validated['val207'] : 0;
				$koteiPatternObjData['N_Kotei'] = $validated['val208'];

				$result = Cyn_mstKotei_STR_C::query()
					->where('CKind', $cKind)
					->Where('Code', $code)
					->Where('No', valueUrlDecode($request->val101))
					->Where('Updated_at', valueUrlDecode($request->val102))
					->update($koteiPatternObjData);

				if (!$result) {
					throw new MissingUpdateException(config('message.msg_cmn_db_002'));
				}
			} catch (MissingUpdateException $e) {
				//update failed, show error
				$originalError = $e->returnMessage;
				$url = url('/');
				$url .= '/' . $menuInfo->KindURL;
				$url .= '/' . $menuInfo->MenuURL;
				$url .= '/editdetail';
				$url .= '?cmn1=' . $request->cmn1;
				$url .= '&cmn2=' . $request->cmn2;
				$url .= '&page=' . $request->page;
				$url .= '&pageunit=' . $request->pageunit;
				$url .= '&sort=' . $request->sort;
				$url .= '&direction=' . $request->direction;
				$url .= '&val1=' . $request->val1;
				$url .= '&val3=' . $request->val3;
				$url .= '&val101=' . $request->val101;
				$url .= '&val102=' . $request->val102;
				//encode val201 -> val208
				for ($i = 1; $i <= 8; $i++) {
					$key = 'val20'.$i;
					$url .= '&val20'.$i.'=' . valueUrlEncode($request->$key);
				}
				$url .= '&err1=' . valueUrlEncode($originalError);
				return redirect($url);
			}
		}

		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/indexdetail';
		$url .= '?cmn1=' . $request->cmn1;
		$url .= '&cmn2=' . $request->cmn2;
		$url .= '&page=' . $request->page;
		$url .= '&pageunit=' . $request->pageunit;
		$url .= '&sort=' . $request->sort;
		$url .= '&direction=' . $request->direction;
		$url .= '&val1=' . $request->val1;
		$url .= '&val3=' . $request->val3;

		//everything is ok
		return redirect($url);
	}

	/**
	 * get all data form master data to select option
	 *
	 * @param Request $request
	 * @param Integer $cKind
	 * @param String $code
	 * @return array
	 *
	 * @create 2020/09/14 Chien
	 * @update
	 */
	private function loadDataSelect($request, $cKind = 0, $code = '') {
		// val201
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
			$arrKumiku[$kumiku] = is_array($data) ? $data[2] : '';
		}

		// val203
		$kotei = Cyn_mstKotei::select('Code')
							->selectRaw("Code+'".config('system_const.code_name_separator')."'+Name as Name")
							->where('Cyn_mstKotei.DelFlag', '=', 0)
							->where('Cyn_mstKotei.CKind', '=', $cKind)
							->orderBy('Code')
							->get();

		// val204
		$floor = MstFloor::select('SortNo', 'Code')
						->selectRaw("Code+'".config('system_const.code_name_separator')."'+Name as Name")
						->where('MstFloor.ViewFlag', '=', 1)
						->orderBy('SortNo')
						->orderBy('Code')
						->get();

		// val205
		$BD_Code = MstBDCode::select('Code')
							->selectRaw("Code+'".config('system_const.code_name_separator')."'+Name as Name")
							->where('MstBDCode.ViewFlag', '=', 1)
							->orderBy('Code')
							->get();

		// val206
		$N_KKumiku = Cyn_mstKotei_STR_C::select('KKumiku')
										->where('Cyn_mstKotei_STR_C.CKind', '=', $cKind)
										->where('Cyn_mstKotei_STR_C.Code', '=', $code)
										->orderBy('KKumiku')
										->distinct()->get();
		if ($N_KKumiku != null) {
			foreach ($N_KKumiku as &$obj) {
				$data = FuncCommon::getKumikuData($obj->KKumiku);
				$obj->Name = is_array($data) ? $data[2] : '';
			}
		}

		// val208
		$N_Kotei = Cyn_mstKotei_STR_C::select('Cyn_mstKotei.Code')
									->selectRaw("Cyn_mstKotei.Code+'".config('system_const.code_name_separator')."'+Cyn_mstKotei.Name as Name")
									->join('Cyn_mstKotei', function($join) {
										$join->on('Cyn_mstKotei.Code', '=', 'Cyn_mstKotei_STR_C.Kotei')
											->on('Cyn_mstKotei.CKind', '=', 'Cyn_mstKotei_STR_C.CKind');
									})
									->where('Cyn_mstKotei_STR_C.CKind', '=', $cKind)
									->where('Cyn_mstKotei_STR_C.Code', '=', $code)
									->where('Cyn_mstKotei.DelFlag', '=', 0)
									->orderBy('Cyn_mstKotei.Code')
									->distinct()->get();

		return array(
			'val201' => $arrKumiku,	// val201
			'val203' => $kotei,		// val203
			'val204' => $floor,		// val204
			'val205' => $BD_Code,	// val205
			'val206' => $N_KKumiku,	// val206
			'val208' => $N_Kotei,	// val208
		);
	}
}