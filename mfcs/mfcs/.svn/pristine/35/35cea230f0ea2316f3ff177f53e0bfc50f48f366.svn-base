<?php
/*
 * @FloorController.php
 * Floor Controller file
 *
 * @create 2020/08/20 Cuong
 *
 * @update 2020/09/19	Cuong	pageunit use system_const
 */
namespace App\Http\Controllers\Mst;

use App\Http\Controllers\Controller;
use App\Http\Requests\Mst\FloorContentsRequest;
use App\Repositories\MstFloor\MstFloorRepositoryInterface;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use Illuminate\Database\QueryException;
use App\Models\MstFloor;
use App\Librarys\FuncCommon;
use Illuminate\Pagination\LengthAwarePaginator;

/*
 * FloorController class
 *
 * @create 2020/08/20 Cuong
 *
 * @update
 */
class FloorController extends Controller
{
	protected $mobjFloor;
	/**
	 * construct
	 * @param MstFloorRepositoryInterface
	 * @return mixed
	 * @create 2020/08/20 Cuong
	 * @update
	 */
	public function __construct(MstFloorRepositoryInterface $repo){
		//create MstFloorRepository instance
		$this->mobjFloor = $repo;
	}

	/**
	 * GET floor list
	 *
	 * @param Request
	 * @return View
	 *
	 * @create 2020/08/20 Cuong
	 * @update
	 */
	public function index(Request $request){
		return $this->initialize($request);
	}

	/**
	 * floor list screen initial display processing
	 *
	 * @param Request
	 * @return View
	 *
	 * @create 2020/08/20　Cuong
	 * @update 2020/08/31　K.Yoshihara 権限チェックに関する全画面への改修
	 * @update 2020/09/19	Cuong	pageunit use system_const
	 * @update 2020/12/10	Cuong	formatDecToChar
	 * @update 2020/12/10	Cuong	change val4 before redirect to view
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

		// update rev2
		$sort = ['fld4','fld1'];
		if (isset($request->sort) && $request->sort != '') {
			if (trim($request->sort) == 'fld4') {
				$sort = ['fld4','fld1'];
			} else if (trim($request->sort) == 'fld1') {
				$sort = ['fld1'];
			} else {
				$sort = [$request->sort, 'fld1'];
			}
		}

		$direction = (isset($request->direction) && $request->direction != '') ?  $request->direction : 'asc';

		$query = MstFloor::select('Code as fld1', 'Name as fld2', 'Nick as fld3', 'SortNo as fld4', 'ViewFlag as fld5')
							->get();
		$rows = $this->sortAndPagination($query, $sort, $direction, $pageunit, $request);

		//get list of floors
		$dataFloors = $rows;
		$dataFloors->getCollection()->transform(function ($value) {
			$value['fld4'] = FuncCommon::formatDecToChar($value['fld4'], 0);
			return $value;
		});
	
		$this->data['floors'] = $dataFloors;
		//request
		$this->data['request'] = $request;
		//return view with all data
		return view('Mst/Floor/index', $this->data);
	}

	/**
	 * GET floor create button action
	 *
	 * @param Illuminate\Http\Request
	 * @return View
	 *
	 * @create 2020/08/20　Cuong
	 * @update 2020/08/31　K.Yoshihara 権限チェックに関する全画面への改修
	 * @update 2020/12/10	Cuong	formatDecToText
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
		//create $floor data array
		$floorData = [];
		//create $originalErrror array
		$originalError = [];

		if (isset($request->err1)) {
			//error occured
			$originalError[] = valueUrlDecode($request->err1);
			$floorData['Code'] = valueUrlDecode($request->val1);
			$floorData['Name'] = valueUrlDecode($request->val2);
			$floorData['Nick'] = valueUrlDecode($request->val3);
			$floorData['Nick1'] = valueUrlDecode($request->val4);
			$floorData['BD_P_D'] = valueUrlDecode($request->val5);
			$floorData['HA_P_D'] = valueUrlDecode($request->val6);
			$floorData['OwnerGroup'] = valueUrlDecode($request->val7);
			$floorData['SortNo'] = FuncCommon::formatDecToText(valueUrlDecode($request->val8));
			$floorData['ViewFlag'] = valueUrlDecode($request->val9);
		}
		//prepare data
		$this->data['request'] = $request;
		$this->data['menuInfo'] = $menuInfo;
		$this->data['floorData'] = $floorData;
		$this->data['originalError'] = $originalError;
		//return view with all data
		return view('Mst/Floor/create', $this->data);
	}

	/**
	 * GET floor show button action
	 *
	 * @param Illuminate\Http\Request
	 * @return View
	 *
	 * @create 2020/08/20　Cuong
	 * @update 2020/08/31　K.Yoshihara 権限チェックに関する全画面への改修
	 * @update 2020/12/10	Cuong	formatDecToText
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
		$floorData = [];
		//get $floorCode
		$floorCode = valueUrlDecode($request->val1);
		$data = $this->mobjFloor->find($floorCode, 'Code');
		if($data){
			//assign returned data to the array
			$floorData = $data->toArray();
			$floorData['BD_P_D'] = FuncCommon::formatDecToText($floorData['BD_P_D']);
			$floorData['HA_P_D'] = FuncCommon::formatDecToText($floorData['HA_P_D']);
			$floorData['SortNo'] = FuncCommon::formatDecToText($floorData['SortNo']);
		}
		//prepare all data
		$this->data['floorData'] = $floorData;
		$this->data['request'] = $request;
		$this->data['menuInfo'] = $menuInfo;
		// return view with all data
		return view('mst/Floor/show', $this->data);
	}

	/**
	 * GET floor edit button action
	 *
	 * @param Illuminate\Http\Request
	 * @return View
	 *
	 * @create 2020/08/20　Cuong
	 * @update 2020/08/31　K.Yoshihara 権限チェックに関する全画面への改修
	 * @update 2020/12/10	Cuong	formatDecToText
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
		//create $floorData array
		$floorData = [];
		//create $originalError array
		$originalError = [];
		if (isset($request->err1)) {
			//error occured
			$originalError[] = valueUrlDecode($request->err1);
			$floorData['Code'] = valueUrlDecode($request->val1);
			$floorData['Name'] = valueUrlDecode($request->val2);
			$floorData['Nick'] = valueUrlDecode($request->val3);
			$floorData['Nick1'] = valueUrlDecode($request->val4);
			$floorData['BD_P_D'] = FuncCommon::formatDecToText(valueUrlDecode($request->val5));
			$floorData['HA_P_D'] = FuncCommon::formatDecToText(valueUrlDecode($request->val6));
			$floorData['OwnerGroup'] = valueUrlDecode($request->val7);
			$floorData['SortNo'] = FuncCommon::formatDecToText(valueUrlDecode($request->val8));
			$floorData['ViewFlag'] = valueUrlDecode($request->val9);
			$floorData['Updated_at'] = valueUrlDecode($request->val10);
		}else{
			//get $floorCode
			$floorCode = valueUrlDecode($request->val1);
			$data = $this->mobjFloor->find($floorCode);
			if($data){
				//assign returned data to the array
				$floorData = $data->toArray();
				$floorData['BD_P_D'] = FuncCommon::formatDecToText($floorData['BD_P_D']);
				$floorData['HA_P_D'] = FuncCommon::formatDecToText($floorData['HA_P_D']);
				$floorData['SortNo'] = FuncCommon::formatDecToText($floorData['SortNo']);
			}
		}
		//prepare data
		$this->data['originalError'] = $originalError;
		$this->data['floorData'] = $floorData;
		$this->data['request'] = $request;
		$this->data['menuInfo'] = $menuInfo;
		//return view with all data
		return view('Mst/Floor/edit', $this->data);
	}

	/**
	 * POST save button(create or edit)
	 *
	 * @param FloorContentsRequest
	 * @return View
	 *
	 * @create 2020/08/20 Cuong
	 * @update 2020/08/31 K.Yoshihara 権限チェックに関する全画面への改修
	 */
	public function save(FloorContentsRequest $request){
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		$validated = $request->validated();
		if($request->method == 'edit'){
			//assign validated data to array
			$floorDataItem['Code'] = $validated['val1'];
			$floorDataItem['Name'] = $validated['val2'];
			$floorDataItem['Nick'] = $validated['val3'];
			$floorDataItem['Nick1'] = $validated['val4'];
			$floorDataItem['BD_P_D'] = $validated['val5'];
			$floorDataItem['HA_P_D'] = $validated['val6'];
			$floorDataItem['OwnerGroup'] = $validated['val7'];
			$floorDataItem['SortNo'] = $validated['val8'];
			$floorDataItem['ViewFlag'] = $validated['val9'];
			$floorDataItem['Up_User'] = $menuInfo->UserID;
			//update floor
			$result = MstFloor::query()
				->where('Code', $validated['val1'])
				->where('Updated_at', $request->val10)
				->update($floorDataItem);
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
				//encode val1 -> val4
				for($i=1; $i<=10; $i++){
					$key = 'val'.$i;
					$url .= '&val'.$i.'=' . valueUrlEncode($request->$key);
				}
				$url .= '&err1=' . valueUrlEncode($originalError);

				return redirect($url);
			}
		}
		elseif($request->method == 'create'){
			$floorDataItem = new MstFloor;
			try {
				//beginTransaction
				DB::transaction(function() use ($floorDataItem, $menuInfo, $validated){
					$floorDataItem->Code = $validated['val1'];
					$floorDataItem->Name = $validated['val2'];
					$floorDataItem->Nick = $validated['val3'];
					$floorDataItem->Nick1 = $validated['val4'];
					$floorDataItem->BD_P_D = $validated['val5'];
					$floorDataItem->HA_P_D = $validated['val6'];
					$floorDataItem->OwnerGroup = $validated['val7'];
					$floorDataItem->SortNo = $validated['val8'];
					$floorDataItem->ViewFlag = $validated['val9'];
					$floorDataItem->Up_User = $menuInfo->UserID;
					$floorDataItem->save();
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
					//encode val1 -> val9
					for($i=1; $i<=9; $i++){
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

		//everything is ok, return to floor list
		return redirect($url);
	}
}