<?php
/*
 * @DistController.php
 * 工程定義管理画面コントローラーファイル
 *
 * @create 2020/10/09 Chien
 *
 * @update
 */

namespace App\Http\Controllers\Sches;

use App\Http\Controllers\Controller;
use App\Repositories\MstDist\MstDistRepositoryInterface;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use Illuminate\Database\QueryException;
use Illuminate\Pagination\LengthAwarePaginator;
use App\Http\Requests\Sches\DistContentsRequest;
use App\Librarys\FuncCommon;
use App\Librarys\MenuInfo;
use App\Librarys\MissingUpdateException;
use App\Models\MstDist;

/*
 * 工程定義管理画面コントローラー
 *
 * @create 2020/10/09 Chien
 *
 * @update
 */
class DistController extends Controller
{
	protected $mobjSchesMstDist;

	/**
	 * construct
	 * @param MstDistRepositoryInterface
	 * @return mixed
	 * @create 2020/10/09 Chien
	 * @update
	 */
	public function __construct(MstDistRepositoryInterface $repositoryMstDist) {
		//create MstFloorRepository instance
		$this->mobjSchesMstDist = $repositoryMstDist;
	}

	/**
	 * 工程定義管理画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/10/09 Chien
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
	 * @create 2020/10/09 Chien
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
		//initialize $originalError
		$originalError = [];

		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
		}

		//get list of pattern
		$query = MstDist::select('Name as fld2', 'Nick as fld3', 'Updated_at as fld4')->selectRaw("LTRIM(RTRIM(Code)) AS fld1 ")->get();

		$sort = ['fld1'];
		if (isset($request->sort) && $request->sort != '' && trim($request->sort) != 'fld1') {
			$sort = [$request->sort, 'fld1'];
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
		$this->data['originalError'] = $originalError;
		//return view with all data
		return view('Sches/Dist/index', $this->data);
	}

	/**
	 * GET 工程定義登録画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/10/09　Chien
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

		//initialize $dataMstDist
		$dataMstDist = [];
		//initialize $originalError
		$originalError = [];

		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
			$dataMstDist['Code'] = valueUrlDecode($request->val1);
			$dataMstDist['Name'] = valueUrlDecode($request->val2);
			$dataMstDist['Nick'] = valueUrlDecode($request->val3);
		}

		$this->data['request'] = $request;
		$this->data['originalError'] = $originalError;
		$this->data['itemData'] = $dataMstDist;

		return view('Sches/Dist/create', $this->data);
	}

	/**
	 * GET 工程定義編集画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/10/09　Chien
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

		//initialize $dataMstDist
		$dataMstDist = [];
		//initialize $originalError
		$originalError = [];

		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
			$dataMstDist['Code'] = valueUrlDecode($request->val1);
			$dataMstDist['Name'] = valueUrlDecode($request->val2);
			$dataMstDist['Nick'] = valueUrlDecode($request->val3);
			$dataMstDist['Updated_at'] = valueUrlDecode($request->val4);
		} else {
			$code = valueUrlDecode($request->val1);	// Code

			$data = $this->mobjSchesMstDist->find($code);
			if (!is_null($data)) {
				$dataMstDist = $data->toArray();
			}
		}

		$this->data['request'] = $request;
		$this->data['originalError'] = $originalError;
		$this->data['itemData'] = $dataMstDist;

		return view('Sches/Dist/edit', $this->data);
	}

	/**
	 * GET 工程定義詳細画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/10/09　Chien
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

		//initialize $dataMstDist
		$dataMstDist = [];
		//initialize $originalError
		$originalError = [];

		$code = valueUrlDecode($request->val1);		// Code

		$data = $this->mobjSchesMstDist->find($code);
		if (!is_null($data)) {
			$dataMstDist = $data->toArray();
		}

		$this->data['request'] = $request;
		$this->data['originalError'] = $originalError;
		$this->data['itemData'] = $dataMstDist;

		return view('Sches/Dist/show', $this->data);
	}

	/**
	 * POST 工程定義マスター削除ボタンアクション
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View 工程パターン共通化画面
	 *
	 * @create 2020/10/09 Chien
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

		$code = valueUrlDecode($request->fld1);
		$updateAt = valueUrlDecode($request->fld4);

		$deleteFlag = MstDist::where('Code', '=', $code)
							->where('Updated_at', '=', $updateAt)
							->delete();
		if ($deleteFlag == 0) {
			$url .= '&err1=' . valueUrlEncode(config('message.msg_cmn_db_002'));
		}

		return redirect($url);
	}

	/**
	 * POST パターンマスターの保存ボタンアクション
	 *
	 * @param PatternContentsRequest 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/10/09　Chien
	 * @update
	 */
	public function save(DistContentsRequest $request) {
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
				$obj = new MstDist;
				//beginTransaction
				DB::transaction(function() use ($obj, $validated) {
					$obj->Code = $validated['val1'];
					$obj->Name = $validated['val2'];
					$obj->Nick = $validated['val3'];
					$obj->ForeColor = 0;
					$obj->BackColor = 0;
					$obj->save();
				});
			} catch (QueryException $e) {
				if ($request->method == 'create' && $e->getCode() == '23000') {
					//error code is 23000
					$originalError = config('message.msg_cmn_db_004');
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
					for ($i = 1; $i <= 3; $i++) {
						$key = 'val'.$i;
						$url .= '&val'.$i.'=' . valueUrlEncode($request->$key);
					}
					$url .= '&err1=' . valueUrlEncode($originalError);
					//redirect to $url
					return redirect($url);
				}
			}
		}

		if ($request->method == 'edit') {
			try {
				DB::transaction(function() use ($request, $validated) {
					//assign validated data to array
					$objData['Name'] = $validated['val2'];
					$objData['Nick'] = $validated['val3'];

					$result = MstDist::query()
						->Where('Code', $validated['val1'])
						->Where('Updated_at', valueUrlDecode($request->val4))
						->update($objData);

					if (!$result) {
						throw new MissingUpdateException(config('message.msg_cmn_db_002'));
					}
				});
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
				for ($i = 1; $i <= 4; $i++) {
					$key = 'val'.$i;
					if ($i == 4) {
						$url .= '&val'.$i.'=' . $request->$key;
					} else {
						$url .= '&val'.$i.'=' . valueUrlEncode($request->$key);
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
}