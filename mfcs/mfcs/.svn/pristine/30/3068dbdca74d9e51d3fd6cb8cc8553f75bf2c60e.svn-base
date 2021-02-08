<?php
/*
 * @KoteiController.php
 * 工程定義管理画面コントローラーファイル
 *
 * @create 2020/09/03 Dung
 * @update 2020/10/14 Dung changed according to Rev2
 * @update 2020/10/15 Dung changed from $pageUnit to $pageunit
 * @update 2020/1/04 Dung fixbug No 185 buglist 01
 */
namespace App\Http\Controllers\Schem;

use App\Http\Controllers\Controller;
use App\Http\Requests\Schem\KoteiContentsRequest;
use App\Repositories\Cyn_mstKotei\Cyn_mstKoteiRepositoryInterface;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use Illuminate\Database\QueryException;
use Illuminate\Pagination\LengthAwarePaginator;
use App\Librarys\FuncCommon;
use App\Librarys\MenuInfo;
use App\Librarys\MissingUpdateException;
use App\Models\Cyn_mstKotei;

/*
 * 工程定義管理画面コントローラー
 *
 * @create 2020/09/03 Dung
 */
class KoteiController extends Controller
{
	protected $mobjSchemCynMstKotei;
	/**
	 * construct
	 * @param Cyn_mstKoteiRepositoryInterface
	 * @return mixed
	 * @create 2020/09/03 Dung
	 */
	public function __construct(Cyn_mstKoteiRepositoryInterface $repository){
		//create MstFloorRepository instance
		$this->mobjSchemCynMstKotei = $repository;
	}
	/**
	 * 工程定義管理画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/09/03 Dung
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
	 * @create 2020/09/03　Dung
	 * @update 2020/10/14 Dung changed according to Rev2
	 * @update 2020/10/15 Dung changed from $pageUnit to $pageunit
	 */
	private function initialize(Request $request) {
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		$this->data['menuInfo'] = $menuInfo;
		//condition array
		$condition = [];
		$query = Cyn_mstKotei::select('Code as fld1', 'Name as fld2', 'Nick as fld3',
										'CKind as fld4', 'DelFlag as fld5')->get();

		$sort = ['fld1', 'fld4'];
		if (isset($request->sort) && $request->sort != '') {
			if (trim($request->sort) == 'fld1') {
				$sort = ['fld1', 'fld4'];
			} else if (trim($request->sort) == 'fld4') {
				$sort = ['fld4', 'fld1'];
			} else {
				$sort = [$request->sort, 'fld1', 'fld4'];
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
		return view('Schem/Kotei/index', $this->data);
	}
	/**
	 * GET itemData create button action
	 *
	 * @param Illuminate\Http\Request
	 * @return View
	 *
	 * @create 2020/09/04　Dung
	 */
	public function create(Request $request) {
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		//initialize $itemData
		$itemData = [];
		//initialize $originalError
		$originalError = [];

		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
			$itemData['val101'] = valueUrlDecode($request->val101);
			$itemData['val102'] = valueUrlDecode($request->val102);
			$itemData['val103'] = valueUrlDecode($request->val103);
			$itemData['val104'] = valueUrlDecode($request->val104);
			$itemData['val105'] = valueUrlDecode($request->val105);
		}

		$this->data['request'] = $request;
		$this->data['menuInfo'] = $menuInfo;
		$this->data['originalError'] = $originalError;
		$this->data['itemData'] = $itemData;

		return view('Schem/Kotei/create', $this->data);
	}

	/**
	 * GET itemData edit button action
	 *
	 * @param Illuminate\Http\Request
	 * @return View
	 *
	 * @create 2020/09/07　Dung
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
		//initialize $itemData
		$itemData = [];
		//initialize $originalError
		$originalError = [];

		if (isset($request->err1)) {
			//error occured
			$originalError[] = valueUrlDecode($request->err1);
			$itemData['val101'] = valueUrlDecode($request->val101);
			$itemData['val102'] = valueUrlDecode($request->val102);
			$itemData['val103'] = valueUrlDecode($request->val103);
			$itemData['val104'] = valueUrlDecode($request->val104);
			$itemData['val105'] = valueUrlDecode($request->val105);
			$itemData['val106'] = valueUrlDecode($request->val106);
		} else {
			//get $itemDataCode
			$val1 = (isset($request->val1)) ? $request->val1 : $request->val101;
			$val4 = (isset($request->val4)) ? $request->val4 : $request->val104;

			$code = valueUrlDecode($request->val1);
			$cKind = valueUrlDecode($request->val4);
			$data = $this->mobjSchemCynMstKotei->findWithMultiKey($code,$cKind);
			if(!is_null($data)) {
				$itemData['val101'] = $data->Code;
				$itemData['val102'] = $data->Name;
				$itemData['val103'] = $data->Nick;
				$itemData['val104'] = $data->CKind;
				$itemData['val105'] = $data->DelFlag;
				$itemData['val106'] = $data->Updated_at;
			}
		}
		//prepare data
		$this->data['originalError'] = $originalError;
		$this->data['itemData'] = $itemData;
		$this->data['request'] = $request;
		$this->data['menuInfo'] = $menuInfo;

		return view('Schem/Kotei/edit', $this->data);
	}
	/**
	 * GET itemData show button action
	 *
	 * @param Illuminate\Http\Request
	 * @return View
	 *
	 * @create 2020/09/07　Dung
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

		//initialize $itemData
		$itemData = [];
		//initialize $originalError
		$originalError = [];
		$code = valueUrlDecode($request->val1);		// Code
		$cKind = valueUrlDecode($request->val4);	// CKind
		$data = $this->mobjSchemCynMstKotei->findWithMultiKey($code, $cKind);
		if(!is_null($data)) {
			$itemData['val101'] = $data->Code;
			$itemData['val102'] = $data->Name;
			$itemData['val103'] = $data->Nick;
			$itemData['val104'] = $data->CKind;
			$itemData['val105'] = $data->DelFlag;
			$itemData['val106'] = $data->Updated_at;
		}
		$this->data['request'] = $request;
		$this->data['originalError'] = $originalError;
		$this->data['itemData'] = $itemData;
		return view('Schem/Kotei/show', $this->data);
	}
	/**
	 * POST save button(create or edit)
	 *
	 * @param KoteiContentsRequest
	 * @return View
	 *
	 * @create 2020/09/04　Dung
	 */
	public function save(KoteiContentsRequest $request) {
		// // 初期処理
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
				$itemData = new Cyn_mstKotei;
				//beginTransaction
				DB::transaction(function() use ($itemData, $validated) {
					$itemData->Code = $validated['val101'];
					$itemData->Name = $validated['val102'];
					$itemData->Nick = $validated['val103'];
					$itemData->CKind = $validated['val104'];
					$itemData->DelFlag = $validated['val105'];
					$itemData->save();
				});
			} catch(QueryException $ex) {
				if ($request->method == 'create' && $ex->getCode() == '23000') {
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
					//encode val101 -> val105
					for ($i = 1; $i <= 5; $i++) {
						$key = 'val10'.$i;
						$url .= '&val10'.$i.'=' . valueUrlEncode($request->$key);
					}
					$url .= '&err1=' . valueUrlEncode($originalError);
					//redirect to $url
					return redirect($url);
				}
				//throw exception
				throw $e;
			}
		}

		if ($request->method == 'edit') {
			try {
				//assign validated data to array
				$koteiObjData['Name'] = $validated['val102'];
				$koteiObjData['Nick'] = $validated['val103'];
				$koteiObjData['DelFlag'] = $validated['val105'];

				$result = Cyn_mstKotei::query()
					->Where('Code', $validated['val101'])
					->where('CKind', $validated['val104'])
					->Where('Updated_at', valueUrlDecode($request->val106))
					->update($koteiObjData);
				if (!$result) {
					//update failed, show error
					throw new MissingUpdateException(config('message.msg_cmn_db_002'));
				}
			} catch (MissingUpdateException  $ex) {
				//update failed, show error
				$originalError = $ex->returnMessage;;
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
				for($i = 1; $i <= 6; $i++) {
					$key = 'val10'.$i;
					if ($i == 6) {
						$url .= '&val10'.$i.'=' . $request->$key;
					} else {
						$url .= '&val10'.$i.'=' . valueUrlEncode($request->$key);
					}
				}
				$url .= '&err1=' . valueUrlEncode($originalError);

				return redirect($url);
				//throw exception
				throw $e;
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


}