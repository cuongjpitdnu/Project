<?php
/*
* @SummaryController.php
* Schem Bdata Controller file
*
* @create 2021/02/01 Cuong
*
*
* 
*/
namespace App\Http\Controllers\Report;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Models\R_Summary;
use App\Models\R_SummaryCondition;
use App\Models\R_SummaryTypeMst;

use App\Http\Requests\Report\SummaryRequest;
use DB;
/*
* SummaryController class
*
* @create 2021/02/01 Cuong
* @update
*/
class SummaryController extends Controller
{
	/**
	 * 日程取込条件設定画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2021/02/01 Cuong
	 * @update
	*/
	public function index(Request $request)
	{
		return $this->initialize($request);
	}

	/**
	 * initial display processing
	 *
	 * @param Request
	 * @return View
	 *
	 * @create 2021/02/01 Cuong
	 * @update
	 */
	private function initialize(Request $request) 
	{
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));
		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		/* 初期表示 */
		$dateNow = DB::selectOne('SELECT CONVERT(DATE, getdate()) AS sysdate')->sysdate;
		$dateNow = str_replace('-', '/', $dateNow);

		$itemShow = array(
			'val1' => isset($request->val1) ? $request->val1 : '',
			'val2' => isset($request->val2) ? $request->val2 : '',
			'val3' => isset($request->val3) ? $request->val3 : '',
			'val4' => isset($request->val4) ? $request->val4 : $dateNow,
			'val5' => isset($request->val5) ? $request->val5 : valueUrlEncode(0),
			'val6' => isset($request->val6) ? $request->val6 : valueUrlEncode(0),
		);

		//initialize $originalError
		$originalError = [];
		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
		}

		$val1_All = $this->getDataRSummaryTypeMst();
		if(count($val1_All) > 0) {
			if($itemShow['val1'] == '') {
				$itemShow['val1'] = valueUrlEncode($val1_All->first()->val1);
			}
		}
		
		$this->data['val1_All'] = $val1_All;
		$this->data['val2_All'] = $this->getDataRSummary();
		$this->data['val3_All'] = $this->getDataRSummaryCondition();

		//request
		$this->data['menuInfo'] = $menuInfo;
		$this->data['itemShow'] = $itemShow;
		$this->data['request'] = $request;
		$this->data['originalError'] = $originalError;
		//return view with all data
		return view('Report/summary/index', $this->data);
	}

	public function settings(SummaryRequest $request) {
		if($request->isMethod('post')){
			$res = $this->doPost($request);
			return redirect($res);
		}

		if($request->isMethod('get')){
			$res = $this->doGet($request);
			return $res;
		}
		
	}

	protected function doGet($request)
	{
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));
		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		$this->data['menuInfo'] = $menuInfo;
		// $this->data['itemShow'] = $itemShow;
		$this->data['request'] = $request;
		return view('Report/summary/settings', $this->data);
	}

	protected function doPost( $request)
	{
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));
		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		$res = R_Summary::select('ID')->where('ID','=', $request->val2)
									->where('ViewName', 'like', 'VS_%')->get();
		if(count($res) == 0) {
			$urlErr = url('/');
			$urlErr .= '/' . $menuInfo->KindURL;
			$urlErr .= '/' . $menuInfo->MenuURL;
			$urlErr .= '/index';
			$urlErr .= '?cmn1=' . valueUrlEncode($menuInfo->KindID);
			$urlErr .= '&cmn2=' . valueUrlEncode($menuInfo->MenuID);
			$urlErr .= '&val1=' . valueUrlEncode($request->val1);
			$urlErr .= '&val2=' . valueUrlEncode($request->val2);
			$urlErr .= '&val3=' . valueUrlEncode($request->val3);
			$urlErr .= '&val4=' . $request->val4;
			$urlErr .= '&val5=' . valueUrlEncode($request->val5);
			$urlErr .= '&val6=' . valueUrlEncode($request->val6);
			$urlErr .= '&err1=' . valueUrlEncode(config('message.msg_report_summary_001'));
			return ($urlErr);
		}

		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/settings';
		$url .= '?cmn1=' . valueUrlEncode($menuInfo->KindID);
		$url .= '&cmn2=' . valueUrlEncode($menuInfo->MenuID);
		$url .= '&val1=' . valueUrlEncode($request->val1);
		$url .= '&val2=' . valueUrlEncode($request->val2);
		$url .= '&val3=' . valueUrlEncode($request->val3);
		$url .= '&val4=' . $request->val4;
		$url .= '&val5=' . valueUrlEncode($request->val5);
		$url .= '&val6=' . valueUrlEncode($request->val6);

		return ($url);
	}

	/**
	 * get all data R_SummaryTypeMst table
	 *
	 * @param
	 * @return mix
	 *
	 * @create 2021/02/01 Cuong
	 * @update
	 */
	private function getDataRSummaryTypeMst() {
		$data = R_SummaryTypeMst::select('ID as val1', 'SummaryTypeName as val1Name', 'SortNo')
				->orderBy('SortNo')->get();
		if (count($data) > 0) {
			foreach ($data as &$row) {
				$row->val1 = valueUrlEncode($row->val1);
				$row->val1Name = htmlentities($row->val1Name);
				$row->SortNo = valueUrlEncode($row->SortNo);
			}
		}
		return $data;
	}
	/**
	 * get all data R_Summary table
	 *
	 * @param
	 * @return mix
	 *
	 * @create 2021/02/01 Cuong
	 * @update
	 */
	private function getDataRSummary() {
		$data = R_Summary::select('ID as val2', 'SummaryName as val2Name', 'SummaryType')
				->orderBy('SummaryName')->get();
		if (count($data) > 0) {
			foreach ($data as &$row) {
				$row->val2 = valueUrlEncode($row->val2);
				$row->val2Name = htmlentities($row->val2Name);
				$row->SummaryType = valueUrlEncode($row->SummaryType);
			}
		}
		return $data;
	}
	/**
	 * get all data R_SummaryCondition table
	 *
	 * @param
	 * @return mix
	 *
	 * @create 2021/02/01 Cuong
	 * @update
	 */
	private function getDataRSummaryCondition() {
		$data = R_SummaryCondition::select('ID as val3', 'ItemName as val3Name', 'SummaryID', 'SortNo')
				->where('RequiredFlag', '=', 0)
				->orderBy('SortNo')->get();
		if (count($data) > 0) {
			foreach ($data as &$row) {
				$row->val3 = valueUrlEncode($row->val3);
				$row->val3Name = htmlentities($row->val3Name);
				$row->SummaryID = valueUrlEncode($row->SummaryID);
				$row->SortNo = valueUrlEncode($row->SortNo);
			}
		}
		return $data;
	}
}
