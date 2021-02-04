<?php
/*
 * @BDataController.php
 * Order Controller file
 *
 * @create 2020/08/06 Thang
 *
 * @update 2020/08/24 Cuong Update/Insert Up_User
 * @update 2020/09/19	Cuong	pageunit use system_const
 */
namespace App\Http\Controllers\Mst;

use App\Http\Controllers\Controller;
use App\Http\Requests\Mst\BDataContentsRequest;
use App\Repositories\MstBDCode\MstBDCodeRepositoryInterface;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use App\Models\MstBDCode;
use Illuminate\Database\QueryException;
use Illuminate\Pagination\LengthAwarePaginator;
/*
 * BDataController class
 *
 * @create 2020/08/06 Thang
 * @update
 */
class BDataController extends Controller
{
	protected $repo;
	/**
	 * construct
	 * @param MstBDCodeRepositoryInterface
	 * @return mixed
	 * @create 2020/08/06　Thang
	 * @update
	 */
	public function __construct(MstBDCodeRepositoryInterface $repo){

		//create Repository instance
		$this->repo = $repo;
	}

	/**
	 * GET bData list
	 *
	 * @param Request
	 * @return View
	 *
	 * @create 2020/08/06　Thang
	 * @update
	 */
	public function index(Request $request){
		return $this->initialize($request);
	}
	/**
	 * bData list screen initial display processing
	 *
	 * @param Request
	 * @return View
	 *
	 * @create 2020/08/06　Thang
	 * @update 2020/08/31　K.Yoshihara 権限チェックに関する全画面への改修
	 * @update 2020/09/19	Cuong	pageunit use system_const
	 */
	private function initialize(Request $request){
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$this->data['menuInfo'] = $menuInfo;

		//pageunit != 10 -> pageunit = 10
		if(isset($request->pageunit) && in_array($request->pageunit, [config('system_const.displayed_results_1'),
																	config('system_const.displayed_results_2'),
																	config('system_const.displayed_results_3')])){
			$pageunit = $request->pageunit;
		}else{
			$pageunit = config('system_const.displayed_results_1');
		}
		
		$direction = (isset($request->direction) && $request->direction != '') ?  $request->direction : 'asc';

		// update rev2
		$sort = ['fld1'];
		if (isset($request->sort) && $request->sort != '' && trim($request->sort) != 'fld1') {
			$sort = [$request->sort, 'fld1'];
		}

		$query = MstBDCode::select('Code as fld1', 'Name as fld2', 'Nick as fld3', 'ViewFlag as fld4')->get();
		$rows = $this->sortAndPagination($query, $sort, $direction, $pageunit, $request);
		
		//get list of bDatas
		$this->data['bDatas'] = $rows;
		//request
		$this->data['request'] = $request;
		//return view with all data
		return view('Mst/BData/index', $this->data);
	}
	/**
	 * GET bData create button action
	 *
	 * @param Illuminate\Http\Request
	 * @return View
	 *
	 * @create 2020/08/06　Thang
	 * @update 2020/08/31　K.Yoshihara 権限チェックに関する全画面への改修
	 */
	public function create(Request $request)
	{

		// // 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		//create $bData array
		$bData = [];
		//create $originalErrror array
		$originalError = [];

		if (isset($request->err1)) {
			//error occured
			$originalError[] = valueUrlDecode($request->err1);
			$bData['Code'] = valueUrlDecode($request->val1);
			$bData['Name'] = valueUrlDecode($request->val2);
			$bData['Nick'] = valueUrlDecode($request->val3);
			$bData['ViewFlag'] = valueUrlDecode($request->val4);
		}
		//prepare data
		$this->data['request'] = $request;
		$this->data['menuInfo'] = $menuInfo;
		$this->data['bData'] = $bData;
		$this->data['originalError'] = $originalError;
		//return view with all data
		return view('Mst/BData/create', $this->data);
	}
	/**
	 * GET bData show button action
	 *
	 * @param Illuminate\Http\Request
	 * @return View
	 *
	 * @create 2020/08/06　Thang
	 * @update 2020/08/31　K.Yoshihara 権限チェックに関する全画面への改修
	 */
	public function show(Request $request)
	{
		// // 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_readonly'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		//create $bData array
		$bData = [];
		//get $bDataCode
		$bDataCode = valueUrlDecode($request->val1);
		$data = $this->repo->find($bDataCode, 'Code');
		if($data){
			//assign returned data to the array
			$bData = $data->toArray();
		}
		//prepare all data
		$this->data['bData'] = $bData;
		$this->data['request'] = $request;
		$this->data['menuInfo'] = $menuInfo;
		// return view with all data
		return view('mst/BData/show', $this->data);
	}
	/**
	 * GET bData edit button action
	 *
	 * @param Illuminate\Http\Request
	 * @return View
	 *
	 * @create 2020/08/06　Thang
	 * @update 2020/08/31　K.Yoshihara 権限チェックに関する全画面への改修
	 */
	public function edit(Request $request)
	{
		// // 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		//create $bData array
		$bData = [];
		//create $originalError array
		$originalError = [];
		if (isset($request->err1)) {
			//error occured
			$originalError[] = valueUrlDecode($request->err1);
			$bData['Code'] = valueUrlDecode($request->val1);
			$bData['Name'] = valueUrlDecode($request->val2);
			$bData['Nick'] = valueUrlDecode($request->val3);
			$bData['ViewFlag'] = valueUrlDecode($request->val4);
			$bData['Updated_at'] = valueUrlDecode($request->val5);
		}else{
			//get $bDataCode
			$bDataCode = valueUrlDecode($request->val1);
			$data = $this->repo->find($bDataCode);
			if($data){
				//assign returned data to the array
				$bData = $data->toArray();
			}
		}
		//prepare data
		$this->data['originalError'] = $originalError;
		$this->data['bData'] = $bData;
		$this->data['request'] = $request;
		$this->data['menuInfo'] = $menuInfo;
		//return view with all data
		return view('Mst/BData/edit', $this->data);
	}

	/**
	 * POST save button(create or edit)
	 *
	 * @param BDataContentsRequest
	 * @return View
	 *
	 * @create 2020/08/06　Thang
	 * @update 2020/08/24 Cuong  Update/Insert Up_User
	 * @update 2020/08/31　K.Yoshihara 権限チェックに関する全画面への改修
	 */
	public function save(BDataContentsRequest $request){
		// // 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		$validated = $request->validated();
		if($request->method == 'edit'){
			//assign validated data to array
			$bDataItem['Code'] = $validated['val1'];
			$bDataItem['Name'] = $validated['val2'];
			$bDataItem['Nick'] = $validated['val3'];
			$bDataItem['ViewFlag'] = $validated['val4'];
			$bDataItem['Up_User'] = $menuInfo->UserID;
			//update bData
			$result = MstBDCode::query()
				->where('Code', $validated['val1'])
				->where('Updated_at', $request->val5)
				->update($bDataItem);
			if(!$result){
				//update failed, show error
				$originalError = config('message.msg_cmn_db_002');
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
				//encode val1 -> val5
				for($i=1; $i<=5; $i++){
					$key = 'val'.$i;
					$url .= '&val'.$i.'=' . valueUrlEncode($request->$key);
				}
				$url .= '&err1=' . valueUrlEncode($originalError);

				return redirect($url);
			}
		}
		elseif($request->method == 'create'){
			$bDataItem = new MstBdCode;
			try {
				//beginTransaction
				DB::transaction(function() use ($bDataItem, $menuInfo, $validated){
					$bDataItem->Code = $validated['val1'];
					$bDataItem->Name = $validated['val2'];
					$bDataItem->Nick = $validated['val3'];
					$bDataItem->ViewFlag = $validated['val4'];
					$bDataItem['Up_User'] = $menuInfo->UserID;

					$bDataItem->save();
				});
			} catch (QueryException $e) {
				if ($request->method == 'create' && $e->getCode() == '23000'){
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
					//encode val1 -> val4
					for($i=1; $i<=4; $i++){
						$key = 'val'.$i;
						$url .= '&val'.$i.'=' . valueUrlEncode($request->$key);
					}
					$url .= '&err1=' . valueUrlEncode($originalError);
					//redirect to $url
					return redirect($url);
				}
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

		//everything is ok, return to bData list
		return redirect($url);
	}
}