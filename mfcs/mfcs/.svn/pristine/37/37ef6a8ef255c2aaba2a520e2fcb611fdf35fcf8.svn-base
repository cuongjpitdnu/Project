<?php
/*
 * @OrderController.php
 * Order Controller file
 *
 * @create 2020/08/03 Thang
 *
 * @update 2020/08/10 Thang fix bug + use DB::transaction()
 *
 * @update 2020/08/24 Cuong Update/Insert Up_User
 * @update 2020/09/19	Cuong	pageunit use system_const
 * @update 2020/12/23　Cuong update process DB:transaction
 */
namespace App\Http\Controllers\Mst;

use App\Http\Controllers\Controller;
use App\Http\Requests\Mst\OrderContentsRequest;
use App\Librarys\TimeTrackerFuncMst;
use App\Librarys\CustomException;
use App\Repositories\MstOrderNo\MstOrderNoRepositoryInterface;
use App\Models\MstOrderNo;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use Illuminate\Database\QueryException;
use Illuminate\Pagination\LengthAwarePaginator;
use Carbon\Carbon;
use Exception;

/*
 * OrderController class
 *
 * @create 2020/08/03 Thang
 *
 * @update
 */
class OrderController extends Controller
{
	protected $orderObject;
	/**
	 * construct
	 * @param MstOrderNoRepositoryInterface
	 * @return mixed
	 * @create 2020/08/03　Thang
	 * @update
	 */
	public function __construct(MstOrderNoRepositoryInterface $orderObject){
		//create MstOrderNoRepository instance
		$this->orderObject = $orderObject;
		// 初期処理
	}
	/**
	 * GET order list
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/08/03　Thang
	 * @update
	 */
	public function index(Request $request){
		return $this->initialize($request);
	}
	/**
	 * GET order show button action
	 *
	 * @param Illuminate\Http\Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/08/04　Thang
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
		$orderData = [];
		$orderNo = valueUrlDecode($request->val1);
		$data = $this->orderObject->find($orderNo, 'OrderNo');
		if($data){
			$orderData = $data->toArray();
			foreach($orderData as $key => $value){
				//format date
				if (strpos($key, 'Date') !== false) {
					$orderData[$key] = $value ? Carbon::parse($value)->format('Y/m/d') : null;
				}
			}
		}
		//prepare data
		$this->data['orderData'] = $orderData;
		$this->data['request'] = $request;
		$this->data['menuInfo'] = $menuInfo;
		// ビューを表示
		return view('mst/order/show', $this->data);
	}
	/**
	 * GET order edit button action
	 *
	 * @param Illuminate\Http\Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/08/04　Thang
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
		//initialize $orderData and $originalError
		$orderData = [];
		$originalError = [];
		if (isset($request->err1)) {
			//error occured
			$originalError[] = valueUrlDecode($request->err1);
			$orderData['OrderNo'] = valueUrlDecode($request->val1);
			$orderData['BLDDIST'] = valueUrlDecode($request->val2);
			$orderData['CLASS'] = valueUrlDecode($request->val3);
			$orderData['TYPE'] = valueUrlDecode($request->val4);
			$orderData['STYLE'] = valueUrlDecode($request->val5);
			$orderData['NAME'] = valueUrlDecode($request->val6);
			$orderData['TP_Date'] = valueUrlDecode($request->val7);
			$orderData['KG_Date'] = valueUrlDecode($request->val8);
			$orderData['OG_Date'] = valueUrlDecode($request->val9);
			$orderData['SG_Date'] = valueUrlDecode($request->val10);
			$orderData['LD_Date'] = valueUrlDecode($request->val11);
			$orderData['S_SDate'] = valueUrlDecode($request->val12);
			$orderData['PE_SDate'] = valueUrlDecode($request->val13);
			$orderData['ST_Date'] = valueUrlDecode($request->val14);
			$orderData['L_Date'] = valueUrlDecode($request->val15);
			$orderData['O_Date'] = valueUrlDecode($request->val16);
			$orderData['PI_Date'] = valueUrlDecode($request->val17);
			$orderData['D_Date'] = valueUrlDecode($request->val18);
			$orderData['Sgts_Flag'] = valueUrlDecode($request->val19);
			$orderData['Is_Dummy'] = valueUrlDecode($request->val20);
			$orderData['Is_Kantei'] = valueUrlDecode($request->val21);
			$orderData['DispFlag'] = valueUrlDecode($request->val22);
			$orderData['WBSCode'] = valueUrlDecode($request->val23);
			$orderData['PreOrderNo'] = valueUrlDecode($request->val24);
			$orderData['Note'] = valueUrlDecode($request->val25);
			$orderData['Updated_at'] = valueUrlDecode($request->val26);
		}else{
			//get orderNo
			$order = valueUrlDecode($request->val1);
			//find the order
			$data = $this->orderObject->find($order);
			if($data){
				//assign returned data to the array
				$orderData = $data->toArray();
				foreach($orderData as $key => $value){
					//format date
					if (strpos($key, 'Date') !== false) {
						$orderData[$key] = $value ? Carbon::parse($value)->format('Y/m/d') : null;
					}
				}
			}
			$originalError = [];
		}
		//prepare data
		$this->data['originalError'] = $originalError;
		$this->data['orderData'] = $orderData;
		$this->data['request'] = $request;
		$this->data['menuInfo'] = $menuInfo;
		//return view with all data
		return view('Mst/Order/edit', $this->data);
	}
	/**
	 * GET order create button action
	 *
	 * @param Illuminate\Http\Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/08/04　Thang
	 * @update 2020/08/31　K.Yoshihara 権限チェックに関する全画面への改修
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
		//initialize $orderData
		$orderData = [];
		//initialize $originalError
		$originalError = [];

		if (isset($request->err1)) {
			//error occured
			$originalError[] = valueUrlDecode($request->err1);
			$orderData['OrderNo'] = valueUrlDecode($request->val1);
			$orderData['BLDDIST'] = valueUrlDecode($request->val2);
			$orderData['CLASS'] = valueUrlDecode($request->val3);
			$orderData['TYPE'] = valueUrlDecode($request->val4);
			$orderData['STYLE'] = valueUrlDecode($request->val5);
			$orderData['NAME'] = valueUrlDecode($request->val6);
			$orderData['TP_Date'] = valueUrlDecode($request->val7);
			$orderData['KG_Date'] = valueUrlDecode($request->val8);
			$orderData['OG_Date'] = valueUrlDecode($request->val9);
			$orderData['SG_Date'] = valueUrlDecode($request->val10);
			$orderData['LD_Date'] = valueUrlDecode($request->val11);
			$orderData['S_SDate'] = valueUrlDecode($request->val12);
			$orderData['PE_SDate'] = valueUrlDecode($request->val13);
			$orderData['ST_Date'] = valueUrlDecode($request->val14);
			$orderData['L_Date'] = valueUrlDecode($request->val15);
			$orderData['O_Date'] = valueUrlDecode($request->val16);
			$orderData['PI_Date'] = valueUrlDecode($request->val17);
			$orderData['D_Date'] = valueUrlDecode($request->val18);
			$orderData['Sgts_Flag'] = valueUrlDecode($request->val19);
			$orderData['Is_Dummy'] = valueUrlDecode($request->val20);
			$orderData['Is_Kantei'] = valueUrlDecode($request->val21);
			$orderData['DispFlag'] = valueUrlDecode($request->val22);
			$orderData['WBSCode'] = valueUrlDecode($request->val23);
			$orderData['PreOrderNo'] = valueUrlDecode($request->val24);
			$orderData['Note'] = valueUrlDecode($request->val25);
		}
		//prepare data
		$this->data['request'] = $request;
		$this->data['menuInfo'] = $menuInfo;
		$this->data['orderData'] = $orderData;
		$this->data['originalError'] = $originalError;
		//return view with all data
		return view('Mst/Order/create', $this->data);
	}

	const TIME_TRACKER_ERROR = 'TimeNXError';

	/**
	 * POST save button(create or edit)
	 *
	 * @param OrderContentsRequest
	 * @return View ビュー
	 *
	 * @create 2020/08/04　Thang
	 * @update 2020/08/10 Thang リファクタリング
	 *
	 * @update 2020/08/24 Cuong Update/Insert Up_User
	 * @update 2020/08/31　K.Yoshihara 権限チェックに関する全画面への改修
	 * @update 2020/12/23　Cuong update process DB:transaction
	 */
	public function save(OrderContentsRequest $request){
		// // 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		//validate form
		$validated = $request->validated();
		if($request->method == 'edit'){
			//edit Order
			$order['OrderNo'] = $validated['val1'];
			$order['BLDDIST'] = $validated['val2'];
			$order['CLASS'] = $validated['val3'];
			$order['TYPE'] = $validated['val4'];
			$order['STYLE'] = $validated['val5'];
			$order['NAME'] = $validated['val6'];
			$order['TP_Date'] = $validated['val7'];
			$order['KG_Date'] = $validated['val8'];
			$order['OG_Date'] = $validated['val9'];
			$order['SG_Date'] = $validated['val10'];
			$order['LD_Date'] = $validated['val11'];
			$order['S_SDate'] = $validated['val12'];
			$order['PE_SDate'] = $validated['val13'];
			$order['ST_Date'] = $validated['val14'];
			$order['L_Date'] = $validated['val15'];
			$order['O_Date'] = $validated['val16'];
			$order['PI_Date'] = $validated['val17'];
			$order['D_Date'] = $validated['val18'];
			$order['Sgts_Flag'] = $validated['val19'];
			$order['Is_Dummy'] = $validated['val20'];
			$order['Is_Kantei'] = $validated['val21'];
			$order['DispFlag'] = $validated['val22'];
			$order['WBSCode'] = $validated['val23'];
			$order['PreOrderNo'] = $validated['val24'];
			$order['Note'] = $validated['val25'];
			$order['Up_User'] = $menuInfo->UserID;
			//process update order
			$result = MstOrderNo::query()
				->where('OrderNo', $validated['val1'])
				->where('Updated_at', $request->val26)
				->update($order);
			if(!$result){
				//update failed
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
				for($i=1; $i<=26; $i++){
					$key = 'val'.$i;
					$url .= '&val'.$i.'=' . valueUrlEncode($request->$key);
				}
				$url .= '&err1=' . valueUrlEncode($originalError);
				//redirect to $url
				return redirect($url);
			}
		}
		elseif($request->method == 'create'){
			//process create order
			$order = new MstOrderNo;
			$order->ViewColor = null;
			$order->DrawPattern = 0;
			try {
				//begin a transaction
				$result = null;
				DB::transaction(function () use ($order, $validated, $menuInfo, $request, &$result) {
					$order->OrderNo = $validated['val1'];
					$order->BLDDIST = $validated['val2'];
					$order->CLASS = $validated['val3'];
					$order->TYPE= $validated['val4'];
					$order->STYLE = $validated['val5'];
					$order->NAME = $validated['val6'];
					$order->TP_Date = $validated['val7'];
					$order->KG_Date = $validated['val8'];
					$order->OG_Date = $validated['val9'];
					$order->SG_Date = $validated['val10'];
					$order->LD_Date = $validated['val11'];
					$order->S_SDate = $validated['val12'];
					$order->PE_SDate = $validated['val13'];
					$order->ST_Date = $validated['val14'];
					$order->L_Date = $validated['val15'];
					$order->O_Date = $validated['val16'];
					$order->PI_Date = $validated['val17'];
					$order->D_Date = $validated['val18'];
					$order->Sgts_Flag = $validated['val19'];
					$order->Is_Dummy = $validated['val20'];
					$order->Is_Kantei = $validated['val21'];
					$order->DispFlag = $validated['val22'];
					$order->WBSCode = $validated['val23'];
					$order->PreOrderNo = $validated['val24'];
					$order->Note = $validated['val25'];
					$order['Up_User'] = $menuInfo->UserID;
					$order->save();
					//new TimeTrackerFuncMst instance
					$mstTracker = new TimeTrackerFuncMst;

					$result = $mstTracker->mstOrder($request->val1, $request->val1, $request->val7, $request->val18);
					if(!is_null($result)){
						throw new CustomException(self::TIME_TRACKER_ERROR);
					}
				});
				
			} catch (QueryException $e) {
				if ($request->method == 'create' && $e->getCode() == '23000'){
					//insert failed, return error to view
					$originalError = config('message.msg_cmn_db_003');
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
					for($i=1; $i<=25; $i++){
						$key = 'val'.$i;
						$url .= '&val'.$i.'=' . valueUrlEncode($request->$key);
					}
					$url .= '&err1=' . valueUrlEncode($originalError);
					return redirect($url);
				}
				throw $e;
			}catch(CustomException $ex) {
				if($ex->getMessage() == self::TIME_TRACKER_ERROR) {
					if(!is_null($result)){
						//timeTracker error
						$originalError = $result;
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
						for($i=1; $i<=25; $i++){
							$key = 'val'.$i;
							$url .= '&val'.$i.'=' . valueUrlEncode($request->$key);
						}
						$url .= '&err1=' . valueUrlEncode($originalError);
						//redirect to the $url
						return redirect($url);
					}
				}
				throw $ex;
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

		//everything is ok, return to mst/order/index
		return redirect($url);
	}
	/**
	 * Order list screen initial display processing
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/08/04　Thang
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
		//condition array
		$condition = [];
		//pageunit != 10 -> pageunit = 10
		if(isset($request->pageunit) && in_array($request->pageunit, [config('system_const.displayed_results_1'),
																		config('system_const.displayed_results_2'),
																		config('system_const.displayed_results_3')])){
			$pageunit = $request->pageunit;
		}else{
			$pageunit = config('system_const.displayed_results_1');
		}

		// Handling sort
		// update rev2
		$direction = (isset($request->direction) && $request->direction != '') ?  $request->direction : 'asc';

		$sort = ['fld1'];
		if (isset($request->sort) && $request->sort != '' && trim($request->sort) != 'fld1') {
			$sort = [$request->sort, 'fld1'];
		}
		$query = MstOrderNo::select('OrderNo as fld1', 'TYPE as fld2', 
										'STYLE as fld3', 'TP_Date as fld4',
										'L_Date as fld5', 'D_Date as fld6', 'DispFlag as fld7')
										->get();
		$rows = $this->sortAndPagination($query, $sort, $direction, $pageunit, $request);

		//get list of orders
		$this->data['orders'] = $rows;

		//request
		$this->data['request'] = $request;
		//return view with all data
		return view('Mst/Order/index', $this->data);
	}
}
