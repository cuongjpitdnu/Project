<?php
/*
 * @MemberController.php
 * Member Controller file
 *
 * @create 2020/09/01 Cuong
 *
 * @update 2020/09/19	Cuong	pageunit use system_const - change baseDate get by sqlserer (getdate())
 * @update 2020/12/23	Cuong	update process DB::transaction
 */
namespace App\Http\Controllers\Mst;

use DB;
use App\Http\Controllers\Controller;
use App\Librarys\MstOrgCommon;
use App\Librarys\MissingUpdateException;
use App\Http\Requests\Mst\MemberIndexRequest;
use App\Http\Requests\Mst\MemberIndexDateRequest;
use App\Http\Requests\Mst\HistoryMemberContentsRequest;
use App\Http\Requests\Mst\MemberCreateRequest;
use App\Http\Requests\Mst\MemberEditRequest;
use Illuminate\Http\Request;
use Illuminate\Pagination\LengthAwarePaginator;
use Illuminate\Database\QueryException;
use App\Models\MstMember;
use App\Models\MEMHist;
use App\Models\MstSyokusyu;
use App\Models\SystemLock;
use App\Librarys\FuncCommon;
use App\Librarys\CustomException;
use Carbon\Carbon;
use Exception;

/*
 * MemberController class
 *
 * @create 2020/09/01 Cuong
 *
 * @update
 */
class MemberController extends Controller
{
	/**
	 * Member screen initial display processing
	 *
	 * @param Request
	 * @return View
	 *
	 * @create 2020/08/20　Cuong
	 * @update 
	 */
	public function index(Request $request)
	{
		return $this->initialize($request);
	}

	/**
	 * Change date processing
	 *
	 * @param MemberIndexDateRequest Caller request object
	 * @return View ビュー
	 *
	 * @create 2020/09/01　Cuong
	 * @update 
	 */
	public function changeDate(MemberIndexDateRequest $request)
	{
		return $this->initialize($request);
	}

	/**
	 * Initial display processing
	 *
	 * @param Request 
	 * @return View 
	 *
	 * @create 2020/09/01　Cuong
	 * @update
	 */
	private function initialize(Request $request)
	{
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		//check request->val1 
		if (!$request->has('val1')) {
			// set $request->val1 = sql getdate
			$request->val1 = DB::selectOne('SELECT CONVERT(DATE, getdate()) AS sysdate')->sysdate;
			$request->val1 = str_replace('-', '/', $request->val1);
		}

		//create object MstOrgCommon
		$mstOrgCommon = new MstOrgCommon($request->val1);

		// check if has validator fails
		if (isset($request->validator) && $request->validator->fails()) {
			//set $grpID
			$grpID = $request->val2; 
		}else{
			$grpID = $request->val2; 
		}
		
		$grpName='';
		if(!is_null($grpID)){
			//set $grpName by return data of $mstOrgCommon->getGrpName()
			$grpName = $mstOrgCommon->getGrpName(valueUrlDecode($grpID));
		}

		if($grpName == config('system_const.org_null_name')) {
			$grpID = null; 
		}
		
		//create arrayarrKanrenID
		$arrKanrenID = $mstOrgCommon->getKanrenID();
		
		// ビューを表示
		//prepare all data
		$this->data['menuInfo'] = $menuInfo;
		$this->data['grpID'] = $grpID;
		$this->data['grpName'] = $grpName;
		$this->data['request'] = $request;
		$this->data['mstOrgCommon'] = $mstOrgCommon;
		$this->data['arrKanrenID'] = $arrKanrenID;
		// return view with all data
		// if validator->fails() retrun view with all data and with errors
		if (isset($request->validator) && $request->validator->fails()) {
			return view('mst/member/index', $this->data)->withErrors($request->validator);
		}
		
		return view('mst/member/index', $this->data);
	}

	/**
	 * GET member serach button action
	 *
	 * @param MemberIndexRequest
	 * @return View
	 *
	 * @create 2020/09/07　Cuong
	 * @update 2020/09/19	Cuong	pageunit use system_const
	 */
	public function search(MemberIndexRequest $request)
	{
		if (isset($request->validator) && $request->validator->fails()) {
			//set $grpID
			return $this->initialize($request);
		}
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		//選択している日付
		$dateNow = DB::selectOne('SELECT CONVERT(DATE, getdate()) AS sysdate')->sysdate;
		$dateNow = str_replace('-', '/', $dateNow);

		//get data with condition search
		$query = $this->getDataSearch($dateNow, $request);

		// 親の[mstOrg].[Name]を全て連結して表示する
		$mstOrgCommon = new MstOrgCommon();
		foreach ($query as &$k) {
			$grpId = (int)($k->fld3);
			$k->fld3 = $mstOrgCommon->getFullName($grpId);
		}

		// Handling sort
		// update rev8
		$sort = ['fld2', 'ID'];
		if (isset($request->sort) && $request->sort != '') {
			$sort = [$request->sort, 'ID'];
		}
		$direction = (isset($request->direction) && $request->direction != '') ?  $request->direction : 'asc';
		//pageunit != 10 -> pageunit = 10
		if(isset($request->pageunit) && in_array(valueUrlDecode($request->pageunit), [config('system_const.displayed_results_1'),
																	config('system_const.displayed_results_2'),
																	config('system_const.displayed_results_3')])){
			$pageunit = valueUrlDecode($request->pageunit);
		}else{
			$pageunit = config('system_const.displayed_results_1');
		}

		$rows = $this->sortAndPagination($query, $sort, $direction, $pageunit, $request);

		// ビューを表示
		//prepare all data
		$this->data['menuInfo'] = $menuInfo;
		$this->data['rows'] = $rows;
		$this->data['request'] = $request;
		// return view with all data
		return view('mst/member/search', $this->data);

	}

	/**
	 * function get data with condition search 
	 *
	 * @param dateTemp 選択している日付
	 * @param request
	 * @return mixed
	 *
	 * @create 2020/09/01　Cuong
	 * @update 2020/10/22　Cuong Add condition
	 * @update 2020/11/25　Cuong update condition
	 * @update 2020/12/21　Cuong update condition : valueUrlDecode($request->val3) != 0
	 * @update 2021/01/19　Cuong update code check condition : $request->val4, val5 (010602_rev9)
	 */
	private function getDataSearch($dateTemp, $request){
		//get data with condition search
		$query = DB::table('mstMember as t1')
		->leftJoin('MEMHist as t2', function($join) use($dateTemp)
		{
			$join->on('t2.MEM_ID', '=', 't1.ID')
				->whereDate('t2.Sdate', '<=', $dateTemp)
				->where(function($query) use($dateTemp) {
					$query->whereDate('t2.Edate', '>=', $dateTemp)
						->orWhereNull('t2.Edate');
				});
		})
		->leftJoin('mstOrg as t3', function($join) use($dateTemp)
		{
			$join->on('t3.ID', '=', 't2.GroupID')
				->whereDate('t3.Sdate', '<=', $dateTemp)
				->where(function($query) use($dateTemp) {
					$query->whereDate('t3.Edate', '>=', $dateTemp)
						->orWhereNull('t3.Edate');
				});
		})
		->leftJoin('mstOrg as t4', function($join) use($dateTemp)
		{
			$join->on('t4.ID', '=', 't2.COM_ID')
				->whereDate('t4.Sdate', '<=', $dateTemp)
				->where(function($query) use($dateTemp) {
					$query->whereDate('t4.Edate', '>=', $dateTemp)
						->orWhereNull('t4.Edate');
				});
		})
		->select('t2.WorkerNo as fld1', 't1.Name as fld2', 't3.ID as fld3', 't4.Name as fld4', 't1.ID', 't1.Updated_at');

		//condition seach
		if(isset($request->val2) && $request->val2 !== ''){
		$query->where('t2.GroupID', '=', valueUrlDecode($request->val2))
				->where('t2.Sdate','<=',$request->val1)
				->where(function($query) use($request) {
					$query->whereDate('t2.Edate', '>=', $request->val1)
						->orWhereNull('t2.Edate');
				});
		};

		if(isset($request->val3) && valueUrlDecode($request->val3) != 0){
		$query->where('t2.COM_ID', '=', valueUrlDecode($request->val3))
			->where('t2.Sdate','<=',$request->val1)
			->where(function($query) use($request) {
				$query->whereDate('t2.Edate', '>=', $request->val1)
					->orWhereNull('t2.Edate');
			});
		};

		if(isset($request->val4) && $request->val4 !== ''){
		$query->whereIn('t1.ID', function ($querylike) use($request)
			{
				$querylike->select('MEMHist.MEM_ID')
				->from('MEMHist')
				->where('MEMHist.WorkerNo','LIKE', $request->val4."%");
			});
		};

		if(isset($request->val5) && $request->val5 !== ''){
			$query->where('t1.Name','LIKE', "%".$request->val5."%");
		};
		
		return $query->get();
	}

	/**
	 * GET member history button action
	 *
	 * @param Request
	 * @return View
	 *
	 * @create 2020/09/10　Cuong
	 * @update 2020/09/19	Cuong	pageunit use system_const
	 */
	public function history(Request  $request)
	{
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		//process when has err1
		$originalError = array();
		if(isset($request->err1)){
			$originalError[] = valueUrlDecode($request->err1);
		}

		$memID =(int) valueUrlDecode($request->val101);		//member id
		//get object member with $memID
		$objMember = MstMember::find($memID);
		$memberName = $objMember->Name;
		//選択している日付
		$dateNow = DB::selectOne('SELECT CONVERT(DATE, getdate()) AS sysdate')->sysdate;
		$dateNow = str_replace('-', '/', $dateNow);

		// get data member history
		$query = $this->getDataMemberHistory($dateNow, $memID);

		// 親の[mstOrg].[Name]を全て連結して表示する
		$mstOrgCommon = new MstOrgCommon();
		foreach ($query as &$k) {
			$grpId = (int)($k->fld4);
			$k->fld4 = $mstOrgCommon->getFullName($grpId);
		}

		// update rev5
		$sort = ['fld1'];
		if (isset($request->sort) && $request->sort != '' && trim($request->sort) != 'fld1') {
			$sort = [$request->sort, 'fld1'];
		}
		$direction = (isset($request->direction) && $request->direction != '') ?  $request->direction : 'desc';

		//pageunit != 10 -> pageunit = 10
		if(isset($request->pageunit) && in_array($request->pageunit, [config('system_const.displayed_results_1'),
																	config('system_const.displayed_results_2'),
																	config('system_const.displayed_results_3')])){
			$pageunit = $request->pageunit;
		}else{
			$pageunit = config('system_const.displayed_results_1');
		}

		$rows = $this->sortAndPagination($query, $sort, $direction, $pageunit, $request);

		// ビューを表示
		//prepare all data
		$this->data['menuInfo'] = $menuInfo;
		$this->data['rows'] = $rows;
		$this->data['request'] = $request;
		$this->data['memberName'] = $memberName;
		$this->data['originalError'] = $originalError;
		// return view with all data
		return view('mst/member/history', $this->data);

	}

	/**
	 * function get data member history
	 *
	 * @param dateTemp 選択している日付
	 * @param memID
	 * @return mixed
	 *
	 * @create 2020/09/01　Cuong
	 * @update
	 */
	private function getDataMemberHistory($dateTemp, $memID){
		//get history of member
		$query = DB::table('MEMHist as t1')
		->leftJoin('mstOrg as t2', function($join) use($dateTemp)
		{
			$join->on('t2.ID', '=', 't1.COM_ID')
				->whereDate('t2.Sdate', '<=', $dateTemp)
				->where(function($query) use($dateTemp) {
					$query->whereDate('t2.Edate', '>=', $dateTemp)
						->orWhereNull('t2.EDate');
				});
		})
		->leftJoin('mstOrg as t3', function($join) use($dateTemp)
		{
			$join->on('t3.ID', '=', 't1.GroupID')
				->whereDate('t3.Sdate', '<=', $dateTemp)
				->where(function($query) use($dateTemp) {
					$query->whereDate('t3.Edate', '>=', $dateTemp)
						->orWhereNull('t3.EDate');
				});
		})
		->select('t1.Sdate as fld1', 't1.Edate as fld2', 't2.Name as fld3', 't1.GroupID as fld4', 't1.Updated_at');

		$query->where('t1.MEM_ID', '=', $memID);
		return $query->get();
	}

	/**
	 * POST 人員マスタ履歴 削除ボタンアクション
	 *
	 * @param Request
	 * @return View ビュー
	 *
	 * @create 2020/09/10	Cuong
	 * @update 
	 */
	public function historydelete(Request $request)
	{
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$memID = valueUrlDecode($request->val101); 	//Member ID
		$sDate = valueUrlDecode($request->val201 );
		$updatedAt = valueUrlDecode($request->val202);
		$errorMessage='';

		//process delete 
		DB::transaction(function() use ($memID, $sDate, $updatedAt,&$errorMessage){
			$deleteFlag = MEMHist::where('MEM_ID', '=', $memID)
			->whereDate('Sdate', '=', $sDate)
			->where('Updated_at', '=', $updatedAt)
			->delete();
			//when not record delete 
			if($deleteFlag == 0){
				$errorMessage = valueUrlEncode (config('message.msg_cmn_db_002'));
			}
		});

		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/history';
		$url .= '?cmn1=' . $request->cmn1;
		$url .= '&cmn2=' . $request->cmn2;
		$url .= '&pageunit=' . $request->pageunit;
		$url .= '&searchpage=' . $request->searchpage;
		$url .= '&searchsort=' . $request->searchsort;
		$url .= '&searchdirection=' . $request->searchdirection;
		$url .= '&page=' . $request->page;
		$url .= '&sort=' . $request->sort;
		$url .= '&direction=' . $request->direction;
		$url .= '&val1=' . $request->val1;
		$url .= '&val2=' . $request->val2;
		$url .= '&val3=' . $request->val3;
		$url .= '&val4=' . $request->val4;
		$url .= '&val5=' . $request->val5;
		$url .= '&val101=' . $request->val101;
		//when has error message set err1 to url
		if (!empty($errorMessage)) { 
			$url .= '&err1=' . $errorMessage; 
		}
		return redirect($url);
	}

		/**
	 * GET member history create button action
	 *
	 * @param Request
	 * @return View
	 *
	 * @create 2020/09/10　Cuong
	 * @update 2020/12/10　Cuong formatDecToText
	 */
	public function historycreate(Request $request)
	{
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$originalError = array();
		$memHist = array();
		$memHist['val304'] = null;
		//when has err1
		if(isset($request->err1)){
			//set err1 to $originalError[]
			$originalError[] = valueUrlDecode($request->err1);
			$memHist['val301'] = FuncCommon::formatDecToText(valueUrlDecode($request->val301)); 
			$memHist['val302'] = valueUrlDecode($request->val302); 
			$memHist['val303'] = valueUrlDecode($request->val303); 
			$memHist['val304'] = valueUrlDecode($request->val304); 
			$memHist['val305'] = valueUrlDecode($request->val305); 
			$memHist['val306'] = valueUrlDecode($request->val306); 
			$memHist['val307'] = valueUrlDecode($request->val307); 
			$memHist['val308'] = valueUrlDecode($request->val308); 
			$memHist['val309'] = valueUrlDecode($request->val309); 
			$memHist['val310'] = FuncCommon::formatDecToText(valueUrlDecode($request->val310)); 
			$memHist['val311'] = valueUrlDecode($request->val311); 
		}else {
			$memID = valueUrlDecode($request->val101);

			$dataMEMHist = MEMHist::where('MEM_ID', '=', $memID)
							->get()->toArray();
			if(count($dataMEMHist) > 0) {
				$arrSDate = array_column($dataMEMHist, 'Sdate');
				$maxSdate = max($arrSDate);
				$datas = array_filter($dataMEMHist, function($item) use($maxSdate){
					return $item['Sdate'] == $maxSdate;
				});

				if(count($datas) > 0) {
					foreach($datas as $data) {
						$memHist['val305'] = !is_null($data['Edate']) ? Carbon::parse($data['Edate'])->addDay()->format('Y/m/d') : null;
						$memHist['val301'] = FuncCommon::formatDecToText($data['WorkerNo']);
					}
				}
			}
		}

		$baseDate = DB::selectOne('SELECT CONVERT(DATE, getdate()) AS sysdate')->sysdate;
		$baseDate = str_replace('-', '/', $baseDate);
		
		$mstOrgCommon = new MstOrgCommon($baseDate);
		if (is_null($memHist['val304'])) {
			$grpName = config('system_const.org_null_name');
		}else {
			$grpName = $mstOrgCommon->getGrpName($memHist['val304']);
		}
		//get data mstSyokysyu
		$mstSyokusyu = MstSyokusyu::all()->sortBy('Code');

		$arrKanren = $mstOrgCommon->getKanrenID();
		if(count($arrKanren) > 0) {
			foreach($arrKanren as &$item) {
				$item['name'] = htmlentities($item['name']);
			}
		}
		// ビューを表示
		//prepare all data
		$this->data['menuInfo'] = $menuInfo;
		$this->data['memHist'] = $memHist;
		$this->data['grpName'] = $grpName;
		$this->data['request'] = $request;
		$this->data['mstOrgCommon'] = $mstOrgCommon;
		$this->data['mstSyokusyu'] = $mstSyokusyu;
		$this->data['originalError'] = $originalError;
		$this->data['arrKanren'] = $arrKanren;
		// return view with all data
		return view('mst/member/historycreate', $this->data);

	}

	/**
	 * GET member history edit button action
	 *
	 * @param Request
	 * @return View
	 *
	 * @create 2020/09/10　Cuong
	 * @update 2020/12/10　Cuong formatDecToText
	 */
	public function historyedit(Request $request)
	{
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$originalError = array();
		$memHist = array();
		$memHist['val304'] = null;

		//when has err1
		if(isset($request->err1)){
			//set message err to $originalError[]
			$originalError[] = valueUrlDecode($request->err1);
			$memHist['val301'] = FuncCommon::formatDecToText(valueUrlDecode($request->val301)); 
			$memHist['val302'] = valueUrlDecode($request->val302); 
			$memHist['val303'] = valueUrlDecode($request->val303); 
			$memHist['val304'] = valueUrlDecode($request->val304); 
			$memHist['val305'] = valueUrlDecode($request->val305); 
			$memHist['val306'] = valueUrlDecode($request->val306); 
			$memHist['val307'] = valueUrlDecode($request->val307); 
			$memHist['val308'] = valueUrlDecode($request->val308); 
			$memHist['val309'] = valueUrlDecode($request->val309); 
			$memHist['val310'] = FuncCommon::formatDecToText(valueUrlDecode($request->val310)); 
			$memHist['val311'] = valueUrlDecode($request->val311); 
		}else {
			$memID = valueUrlDecode($request->val101);
			$sDate = valueUrlDecode($request->val201); 
			$data = MEMHist::where('MEM_ID', '=', $memID)
							->where('Sdate', '=', $sDate)->first();
			if(!is_null($data)) {
				$memHist['val301'] = FuncCommon::formatDecToText($data->WorkerNo);
				$memHist['val302'] = $data->OutInFlag; 
				$memHist['val303'] = $data->COM_ID; 
				$memHist['val304'] = $data->GroupID; 
				$memHist['val305'] = !is_null($data['Sdate']) ? Carbon::parse($data['Sdate'])->format('Y/m/d') : null; 
				$memHist['val306'] = !is_null($data['Edate']) ? Carbon::parse($data['Edate'])->format('Y/m/d') : null; 
				$memHist['val307'] = $data->OutType; 
				$memHist['val308'] = $data->DistCode; 
				$memHist['val309'] = $data->Is_Proper; 
				$memHist['val310'] = FuncCommon::formatDecToText($data->SortNo); 
				$memHist['val311'] = $data->Updated_at;
			}
		}

		$baseDate = DB::selectOne('SELECT CONVERT(DATE, getdate()) AS sysdate')->sysdate;
		$baseDate = str_replace('-', '/', $baseDate);

		$mstOrgCommon = new MstOrgCommon($baseDate);

		//set value $grpName
		if (is_null($memHist['val304'])) {
			$grpName = config('system_const.org_null_name');
		}else {
			$grpName = $mstOrgCommon->getGrpName($memHist['val304']);
		}

		//get data MstSyokusyu
		$mstSyokusyu = MstSyokusyu::all()->sortBy('Code');

		$arrKanren = $mstOrgCommon->getKanrenID();
		if(count($arrKanren) > 0) {
			foreach($arrKanren as &$item) {
				$item['name'] = htmlentities($item['name']);
			}
		}
		// ビューを表示
		//prepare all data
		$this->data['menuInfo'] = $menuInfo;
		$this->data['memHist'] = $memHist;
		$this->data['grpName'] = $grpName;
		$this->data['request'] = $request;
		$this->data['mstOrgCommon'] = $mstOrgCommon;
		$this->data['mstSyokusyu'] = $mstSyokusyu;
		$this->data['originalError'] = $originalError;
		$this->data['arrKanren'] = $arrKanren;

		// return view with all data
		return view('mst/member/historyedit', $this->data);

	}
	/**
	 * GET member history show button action
	 *
	 * @param Request
	 * @return View
	 *
	 * @create 2020/09/10　Cuong
	 * @update 2020/12/10　Cuong formatDecToText
	 */
	public function historyshow(Request  $request)
	{
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_readonly'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$memHist = array();
		$memID = valueUrlDecode($request->val101);
		$sDate = valueUrlDecode($request->val201); 
		$data = MEMHist::where('MEM_ID', '=', $memID)
						->where('Sdate', '=', $sDate)->first();
		
		$memHist['val301'] = FuncCommon::formatDecToText($data->WorkerNo); 
		$memHist['val302'] = $data->OutInFlag; 
		$memHist['val303'] = $data->COM_ID; 
		$memHist['val304'] = $data->GroupID; 
		$memHist['val305'] = !is_null($data['Sdate']) ? Carbon::parse($data['Sdate'])->format('Y/m/d') : null; 
		$memHist['val306'] = !is_null($data['Edate']) ? Carbon::parse($data['Edate'])->format('Y/m/d') : null; 
		$memHist['val307'] = $data->OutType; 
		$memHist['val308'] = $data->DistCode; 
		$memHist['val309'] = $data->Is_Proper; 
		$memHist['val310'] = FuncCommon::formatDecToText($data->SortNo); 
		$memHist['val311'] = $data->Updated_at;

		$originalError = array();

		$baseDate = DB::selectOne('SELECT CONVERT(DATE, getdate()) AS sysdate')->sysdate;
		$baseDate = str_replace('-', '/', $baseDate);
		$mstOrgCommon = new MstOrgCommon($baseDate);

		//set value $grpName
		if (is_null($memHist['val304'])) {
			$grpName = config('system_const.org_null_name');
		}else {
			$grpName = $mstOrgCommon->getGrpName($memHist['val304']);
		}

		//get data MstSyokusyu
		$mstSyokusyu = MstSyokusyu::all()->sortBy('Code');
			
		$arrKanren = $mstOrgCommon->getKanrenID();
		if(count($arrKanren) > 0) {
			foreach($arrKanren as &$item) {
				$item['name'] = htmlentities($item['name']);
			}
		}

		// ビューを表示
		//prepare all data
		$this->data['menuInfo'] = $menuInfo;
		$this->data['memHist'] = $memHist;
		$this->data['grpName'] = $grpName;
		$this->data['request'] = $request;
		$this->data['mstOrgCommon'] = $mstOrgCommon;
		$this->data['mstSyokusyu'] = $mstSyokusyu;
		$this->data['originalError'] = $originalError;
		$this->data['arrKanren'] = $arrKanren;
		// return view with all data
		return view('mst/member/historyshow', $this->data);

	}

	/**
	 * POST member history save button action
	 *
	 * @param Request
	 * @return View
	 *
	 * @create 2020/09/10　Cuong
	 * @update
	 */
	public function historySave(HistoryMemberContentsRequest  $request)
	{
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		$validated = $request->validated();

		$urlErr = url('/');
		$urlErr .= '/' . $menuInfo->KindURL;
		$urlErr .= '/' . $menuInfo->MenuURL;
		if($request->method == 'create') { $urlErr .= '/historycreate'; }
		if($request->method == 'edit') { $urlErr .= '/historyedit'; }
		$urlErr .= '?cmn1=' . $request->cmn1;
		$urlErr .= '&cmn2=' . $request->cmn2;
		$urlErr .= '&pageunit=' . $request->pageunit;
		$urlErr .= '&searchpage=' . $request->searchpage;
		$urlErr .= '&searchsort=' . $request->searchsort;
		$urlErr .= '&searchdirection=' . $request->searchdirection;
		$urlErr .= '&page=' . $request->page;
		$urlErr .= '&sort=' . $request->sort;
		$urlErr .= '&direction=' . $request->direction;
		$urlErr .= '&val1=' . $request->val1;
		$urlErr .= '&val2=' . $request->val2;
		$urlErr .= '&val3=' . $request->val3;
		$urlErr .= '&val4=' . $request->val4;
		$urlErr .= '&val5=' . $request->val5;
		$urlErr .= '&val101=' . $request->val101;
		if($request->method == 'edit'){$urlErr .= '&val201=' . $request->val201;}
		for($i=301; $i<=310; $i++){
			$key = 'val'.$i;
			$urlErr .= '&val'.$i.'=' . valueUrlEncode($request->$key);
		}

		//process block
		$resultProcessBlock = $this->tryLock($menuInfo->KindID, $menuInfo->MenuID, $menuInfo->UserID, $menuInfo->SessionID, 
		valueUrlDecode($request->val101), false);
		if (!is_null($resultProcessBlock)) {
			$originalError = $resultProcessBlock;
			$urlErr .= '&err1=' . valueUrlEncode($originalError);
			return redirect($urlErr);
		}

		$sDate = $request->input('val305');		//input start date
		$eDate = $request->input('val306');		//input end date
		$isCheck = false;
		//process insert member history when method is create
		if ($request->method == 'create') {
			$resultInsert = $this->insertMemberHistory($request, $menuInfo, $sDate, $eDate ,$isCheck, $urlErr);
			if($resultInsert != 1) {
				return redirect($resultInsert);
			}
		}
		//process insert member history when method is edit
		if ($request->method == 'edit') {
			$resultUpdate = $this->updateMemberHistory($request, $menuInfo, $sDate, $eDate ,$isCheck, $urlErr);
			if($resultUpdate != 1) {
				return redirect($resultUpdate);
			}
		}

		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/history';
		$url .= '?cmn1=' . $request->cmn1;
		$url .= '&cmn2=' . $request->cmn2;
		$url .= '&pageunit=' . $request->pageunit;
		$url .= '&searchpage=' . $request->searchpage;
		$url .= '&searchsort=' . $request->searchsort;
		$url .= '&searchdirection=' . $request->searchdirection;
		$url .= '&page=' . $request->page;
		$url .= '&sort=' . $request->sort;
		$url .= '&direction=' . $request->direction;
		$url .= '&val1=' . $request->val1;
		$url .= '&val2=' . $request->val2;
		$url .= '&val3=' . $request->val3;
		$url .= '&val4=' . $request->val4;
		$url .= '&val5=' . $request->val5;
		$url .= '&val101=' . $request->val101;
		return redirect($url);
	}

	const EXCEPTION_ERROR = 'ThrowExceptionError';	//const message exception error

	/**
	* function process insert member history
	*
	* @param Request
	* @param menuInfo
	* @param sDate
	* @param eDate
	* @param isCheck
	* @param urlErr
	* @return mix
	*
	* @create 2020/09/21　Cuong
	* @update 2020/10/22　Cuong when error return url, success return 1
	* @update 2020/12/23　Cuong update process DB::transaction 
	*/
	private function insertMemberHistory($request, $menuInfo, $sDate, $eDate ,$isCheck, $urlErr){
		try {
			DB::transaction(function() use($request, $menuInfo, $sDate, $eDate ,$isCheck){

				if (is_null($eDate)) {
					//when input end date is null
					$isCheck = MEMHist::where('MEM_ID', '=', valueUrlDecode($request->val101))
							->where(function($query) use($sDate) {
								$query->whereDate('MEMHist.Edate', '>=', $sDate)
									->orWhereNull('MEMHist.Edate');
							})->exists();
				
				}else{
					$isCheck = MEMHist::where('MEM_ID', '=', valueUrlDecode($request->val101))
										->where(function($query1) use($sDate, $eDate) {
											$query1->where(function($query2) use($sDate, $eDate) {
													$query2->where('MEMHist.Sdate', '<=', $sDate)
														->where(function($query3) use($sDate, $eDate) {
															$query3->whereDate('MEMHist.Edate', '>=', $sDate)
																->orWhereNull('MEMHist.EDate');
														});

													})
													->orWhere(function($query2) use($sDate, $eDate) {
														$query2->where('MEMHist.Sdate', '<=', $eDate)
															->where(function($query3) use($sDate, $eDate) {
																$query3->whereDate('MEMHist.Edate', '>=', $eDate)
																	->orWhereNull('MEMHist.EDate');
															});

													})
													->orWhere(function($query2) use($sDate, $eDate) {
														$query2->whereDate('MEMHist.Sdate', '>=', $sDate)
														->whereDate('MEMHist.Edate', '<=', $eDate);
													});
										})->exists();  
				}

				//if $isCheck is true
				if($isCheck){
					throw new CustomException(self::EXCEPTION_ERROR);
				}
				//new object MEMHist 
				$objMEMHist = new MEMHist;
				$objMEMHist->MEM_ID = valueUrlDecode($request->val101);
				$objMEMHist->WorkerNo = $request->val301;
				$objMEMHist->OutInFlag = $request->val302;
				if(isset($request->val303) && !is_null($request->val303)) {
					$objMEMHist->COM_ID = $request->val303;
				}

				if(isset($request->val304) && !is_null($request->val304)) {
					$objMEMHist->GroupID = $request->val304;
				}
				$objMEMHist->Sdate = $request->val305;
				$objMEMHist->Edate = $request->val306;
				$objMEMHist->OutType = $request->val307;
				$objMEMHist->DistCode = $request->val308;
				$objMEMHist->Is_Proper = $request->val309;
				$objMEMHist->SortNo = $request->val310;
				$objMEMHist->Up_User = $menuInfo->UserID;
				$objMEMHist->save();
			});

		} catch (QueryException $e) {
			if ($e->getCode() == '23000') {
				$originalError = config('message.msg_cmn_db_008');
				$urlErr .= '&err1=' . valueUrlEncode($originalError);
				return $urlErr;
			}
			//throw exception
			throw $e;
		}catch(CustomException $ex) {
			if($ex->getMessage() == self::EXCEPTION_ERROR) {
				$originalError = config('message.msg_cmn_db_005');
				$urlErr .= '&err1=' . valueUrlEncode($originalError);
				return $urlErr;
			}
			//throw exception
			throw $ex;

		}finally {
			$this->deleteLock($menuInfo->KindID, $menuInfo->MenuID, $menuInfo->SessionID, valueUrlDecode($request->val101));
		}

		return 1;
	}

	/**
	* function process update member history
	*
	* @param Request
	* @param menuInfo
	* @param sDate
	* @param eDate
	* @param isCheck
	* @param urlErr
	* @return mix
	*
	* @create 2020/09/21　Cuong
	* @update 2020/10/22　Cuong when error return url, success return 1
	* @update 2020/12/23  Cuong update process DB::transaction
	*/
	private function updateMemberHistory($request, $menuInfo, $sDate, $eDate ,$isCheck, $urlErr){
		try {
			DB::transaction(function() use($request, $menuInfo, $sDate, $eDate ,$isCheck){
				if (is_null($eDate)) {
					$isCheck = MEMHist::where('MEM_ID', '=', valueUrlDecode($request->val101))
							->where('Sdate', '!=', valueUrlDecode($request->val201))
							->where(function($query) use($sDate) {
								$query->whereDate('MEMHist.Edate', '>=', $sDate)
									->orWhereNull('MEMHist.Edate');
							})->exists();
				
				}else{
					$isCheck = MEMHist::where('MEM_ID', '=', valueUrlDecode($request->val101))
										->where('Sdate', '!=', valueUrlDecode($request->val201))
										->where(function($query1) use($sDate, $eDate) {
											$query1->where(function($query2) use($sDate, $eDate) {
													$query2->where('MEMHist.Sdate', '<=', $sDate)
														->where(function($query3) use($sDate, $eDate) {
															$query3->whereDate('MEMHist.Edate', '>=', $sDate)
																->orWhereNull('MEMHist.Edate');
														});
													})
													->orWhere(function($query2) use($sDate, $eDate) {
														$query2->where('MEMHist.Sdate', '<=', $eDate)
															->where(function($query3) use($sDate, $eDate) {
																$query3->whereDate('MEMHist.Edate', '>=', $eDate)
																	->orWhereNull('MEMHist.Edate');
															});

													})
													->orWhere(function($query2) use($sDate, $eDate) {
														$query2->whereDate('MEMHist.Sdate', '>=', $sDate)
														->whereDate('MEMHist.Edate', '<=', $eDate);
													});
									})->exists();  
				}
				//if $isCheck is true throw exception;
				if($isCheck){
					throw new CustomException(self::EXCEPTION_ERROR);
				}

				$objMEMHist['MEM_ID'] = valueUrlDecode($request->val101);
				$objMEMHist['WorkerNo'] =  $request->val301;
				$objMEMHist['OutInFlag'] = $request->val302;
				if(isset($request->val303) && !is_null($request->val303)) {
					$objMEMHist['COM_ID'] = $request->val303;
				}
				if(isset($request->val304) && !is_null($request->val304)) {
					$objMEMHist['GroupID'] = $request->val304;
				}
				$objMEMHist['Sdate'] =  $request->val305;
				$objMEMHist['Edate'] = $request->val306;
				$objMEMHist['OutType'] = $request->val307;
				$objMEMHist['DistCode'] = $request->val308;
				$objMEMHist['Is_Proper'] = $request->val309;
				$objMEMHist['SortNo'] =  $request->val310;
				$objMEMHist['Up_User'] = $menuInfo->UserID;

				$result = MEMHist::where('MEM_ID', '=', valueUrlDecode($request->val101))
									->where('Sdate', '=', valueUrlDecode($request->val201))
									->where('Updated_at', '=', valueUrlDecode($request->val202))
									->update($objMEMHist);

				if ($result == 0) {
					throw new MissingUpdateException(config('message.msg_cmn_db_002'));
				}
			});

		}catch (MissingUpdateException $e) {
			$originalError = config('message.msg_cmn_db_002');
			$urlErr .= '&err1=' . valueUrlEncode($originalError);
			return $urlErr;
		}catch (QueryException  $e) {
			if ($e->getCode() == '23000') {
				$originalError = config('message.msg_cmn_db_008');
				$urlErr .= '&err1=' . valueUrlEncode($originalError);
				return $urlErr;
			}

			//throw exception
			throw $e;
		}catch (CustomException $ex) {
			if($ex->getMessage() == self::EXCEPTION_ERROR) {
				$originalError = config('message.msg_cmn_db_005');
				$urlErr .= '&err1=' . valueUrlEncode($originalError);
				return $urlErr;
			}
		
			//throw exception
			throw $ex;
		}finally {
			$this->deleteLock($menuInfo->KindID, $menuInfo->MenuID, $menuInfo->SessionID, valueUrlDecode($request->val101));
		}

		return 1;
	}

	/**
	* GET member create button action
	*
	* @param Request
	* @return View
	*
	* @create 2020/09/21　Cuong
	* @update 2020/10/19 Cuong change baseDate get by sqlserer (getdate()) - remove val310,val311
	* @update 2020/12/10　Cuong formatDecToText
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

		$originalError = array();
		$mstMember = array();
		$memHist = array();
		$memHist['val304'] = null;

		if(isset($request->err1)){
			$originalError[] = valueUrlDecode($request->err1);
			$mstMember['val401'] = valueUrlDecode($request->val401); 
			$mstMember['val402'] = valueUrlDecode($request->val402); 
			$mstMember['val403'] = valueUrlDecode($request->val403); 
			$mstMember['val404'] = valueUrlDecode($request->val404); 

			$memHist['val301'] = FuncCommon::formatDecToText(valueUrlDecode($request->val301)); 
			$memHist['val302'] = valueUrlDecode($request->val302); 
			$memHist['val303'] = valueUrlDecode($request->val303); 
			$memHist['val304'] = valueUrlDecode($request->val304); 
			$memHist['val305'] = valueUrlDecode($request->val305); 
			$memHist['val306'] = valueUrlDecode($request->val306); 
			$memHist['val307'] = valueUrlDecode($request->val307); 
			$memHist['val308'] = valueUrlDecode($request->val308); 
			$memHist['val309'] = valueUrlDecode($request->val309); 
		}else {
			$memID = valueUrlDecode($request->val101);
		}

		$dateNow = DB::selectOne('SELECT CONVERT(DATE, getdate()) AS sysdate')->sysdate;
		$dateNow = str_replace('-', '/', $dateNow);
		$baseDate = $dateNow;

		$mstOrgCommon = new MstOrgCommon($baseDate);
		if (is_null($memHist['val304'])) {
			$grpName = config('system_const.org_null_name');
		}else {
			$grpName = $mstOrgCommon->getGrpName($memHist['val304']);
		}
		$mstSyokusyu = MstSyokusyu::all()->sortBy('Code');
		
		$arrKanren = $mstOrgCommon->getKanrenID();
		if(count($arrKanren) > 0) {
			foreach($arrKanren as &$item) {
				$item['name'] = htmlentities($item['name']);
			}
		}

		// ビューを表示
		//prepare all data
		$this->data['menuInfo'] = $menuInfo;
		$this->data['mstMember'] = $mstMember;
		$this->data['memHist'] = $memHist;
		$this->data['grpName'] = $grpName;
		$this->data['request'] = $request;
		$this->data['mstOrgCommon'] = $mstOrgCommon;
		$this->data['mstSyokusyu'] = $mstSyokusyu;
		$this->data['originalError'] = $originalError;
		$this->data['arrKanren'] = $arrKanren;
		// return view with all data
		return view('mst/member/create', $this->data);

	}

	/**
	* GET member edit button action
	*
	* @param Request
	* @return View
	*
	* @create 2020/09/21　Cuong
	* @update
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

		$originalError = array();
		$mstMember = array();
		if(isset($request->err1)){
			$originalError[] = valueUrlDecode($request->err1);
			$mstMember['val401'] = valueUrlDecode($request->val401); 
			$mstMember['val402'] = valueUrlDecode($request->val402); 
			$mstMember['val403'] = valueUrlDecode($request->val403); 
			$mstMember['val404'] = valueUrlDecode($request->val404); 
		}else {
			$memID = valueUrlDecode($request->val101);
			$data = MstMember::find($memID);

			$mstMember['val401'] = $data->Name; 
			$mstMember['val402'] = $data->Yomi; 
			$mstMember['val403'] = $data->Nick; 
			$mstMember['val404'] = !is_null($data->RetireDate) ? Carbon::parse($data->RetireDate)->format('Y/m/d') : null;
		}
		
		$mstSyokusyu = MstSyokusyu::all()->sortBy('Code');
		
		$mstOrgCommon = new MstOrgCommon();
		$arrKanren = array();
		// ビューを表示
		//prepare all data
		$this->data['menuInfo'] = $menuInfo;
		$this->data['mstMember'] = $mstMember;
		$this->data['request'] = $request;
		$this->data['mstSyokusyu'] = $mstSyokusyu;
		$this->data['originalError'] = $originalError;
		$this->data['mstOrgCommon'] = $mstOrgCommon;
		$this->data['arrKanren'] = $arrKanren;
		// return view with all data
		return view('mst/member/edit', $this->data);

	}

	/**
	* GET member show button action
	*
	* @param Request
	* @return View
	*
	* @create 2020/09/21　Cuong
	* @update
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

		$originalError = array();
		$mstMember = array();

		$memID = valueUrlDecode($request->val101);
		$data = MstMember::find($memID);

		$mstMember['val401'] = $data->Name; 
		$mstMember['val402'] = $data->Yomi; 
		$mstMember['val403'] = $data->Nick; 
		$mstMember['val404'] = !is_null($data->RetireDate) ? Carbon::parse($data->RetireDate)->format('Y/m/d') : null;
		
		$mstSyokusyu = MstSyokusyu::all()->sortBy('Code');
		$mstOrgCommon = new MstOrgCommon();
		$arrKanren = array();
		// ビューを表示
		//prepare all data
		$this->data['menuInfo'] = $menuInfo;
		$this->data['mstMember'] = $mstMember;
		$this->data['request'] = $request;
		$this->data['mstSyokusyu'] = $mstSyokusyu;
		$this->data['mstOrgCommon'] = $mstOrgCommon;
		$this->data['arrKanren'] = $arrKanren;
		// return view with all data
		return view('mst/member/show', $this->data);

	}
	/**
	* check before insert button action
	*
	* @param Request
	* @return mix
	*
	* @create 2020/09/21　Cuong
	* @update
	*/
	public function checkInsert(Request $request)
	{
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			$json = '';
			$json .= '{"status":"' . config("system_const.json_status_ng") . '"';
			$json .= ',"message":"' . config("message.json_session_error") . '"}';

			return $json;
		}
		
		$memberName = $request->val401; 
		$objMember = MstMember::query()->whereRaw("REPLACE(Name, ' ', '') = ?", preg_replace('/\s+/', '', $memberName))
					->first();
		
		if(!is_null($objMember)){

			$json = '';
			$json .= '{"status":"' . config("system_const.json_status_ng") . '"';
			$json .= ',"message":null}';

			return $json;
		}

		$json = '';
		$json .= '{"status":"' . config("system_const.json_status_ok") . '"';
		$json .= ',"message":null}';
		return $json;
	}

	/**
	* POST insert member button action
	*
	* @param Request
	* @return View
	*
	* @create 2020/09/21　Cuong
	* @update 2020/04/02　Cuong 仕様書010611をRev10に更新
	*/
	public function insert(MemberCreateRequest $request)
	{
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		$validated = $request->validated();

		DB::transaction(function() use ($request, $menuInfo){
			//get id member nextval sequence
			$seq_mstMember = DB::select('SELECT NEXT VALUE FOR seq_mstMember as memberID');
			$memberID = $seq_mstMember[0]->memberID;

			//create instance MstMember
			$objMember = new MstMember;
			$objMember->Name = $request->val401;
			$objMember->Yomi = $request->val402;
			$objMember->Nick = $request->val403;
			$objMember->RetireDate = $request->val404;
			$objMember->Up_User = $menuInfo->UserID;
			$objMember->ID = $memberID;
			$objMember->save();

			//new object MEMHist 
			$objMEMHist = new MEMHist;
			$objMEMHist->MEM_ID = $objMember->ID;
			$objMEMHist->WorkerNo = $request->val301;
			$objMEMHist->OutInFlag = $request->val302;
			if(isset($request->val303) && !is_null($request->val303)) {
				$objMEMHist->COM_ID = $request->val303;
			}

			if(isset($request->val304) && !is_null($request->val304)) {
				$objMEMHist->GroupID = $request->val304;
			}
			$objMEMHist->Sdate = $request->val305;
			$objMEMHist->Edate = $request->val306;
			$objMEMHist->OutType = $request->val307;
			$objMEMHist->DistCode = $request->val308;
			$objMEMHist->Is_Proper = $request->val309;
			$objMEMHist->Up_User = $menuInfo->UserID;
			$objMEMHist->save();
		});

		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/index';
		$url .= '?cmn1=' . $request->cmn1;
		$url .= '&cmn2=' . $request->cmn2;

		return redirect($url);
	}
	/**
	* POST update member button action
	*
	* @param Request
	* @return View
	*
	* @create 2020/09/21　Cuong
	* @update
	*/
	public function update(MemberEditRequest $request)
	{
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		$validated = $request->validated();
		
		try {
			DB::transaction(function() use ($request, $menuInfo){
				$memberDataItem['Name'] = $request->val401;
				$memberDataItem['Yomi'] = $request->val402;
				$memberDataItem['Nick'] = $request->val403;
				$memberDataItem['RetireDate'] = $request->val404;
				$memberDataItem['Up_User'] = $menuInfo->UserID;
				//create instance MstMember
				$result = MstMember::where('ID', '=', valueUrlDecode($request->val101))
										->where('Updated_at', valueUrlDecode($request->val102))
										->update($memberDataItem);
										
				if($result == 0) {
					throw new MissingUpdateException(config('message.msg_cmn_db_002'));
				}
			});
		} catch (MissingUpdateException $ex) {
			$originalError = config('message.msg_cmn_db_002');

			$urlErr = url('/');
			$urlErr .= '/' . $menuInfo->KindURL;
			$urlErr .= '/' . $menuInfo->MenuURL;
			$urlErr .= '/edit';
			$urlErr .= '?cmn1=' . $request->cmn1;
			$urlErr .= '&cmn2=' . $request->cmn2;
			$urlErr .= '&pageunit=' . $request->pageunit;
			$urlErr .= '&page=' . $request->page;
			$urlErr .= '&sort=' . $request->sort;
			$urlErr .= '&direction=' . $request->direction;
			$urlErr .= '&val1=' . $request->val1;
			$urlErr .= '&val2=' . $request->val2;
			$urlErr .= '&val3=' . $request->val3;
			$urlErr .= '&val4=' . $request->val4;
			$urlErr .= '&val5=' . $request->val5;
			$urlErr .= '&val101=' . $request->val101;
			$urlErr .= '&val102=' . $request->val102;
			$urlErr .= '&val401=' . valueUrlEncode($request->val401);
			$urlErr .= '&val402=' . valueUrlEncode($request->val402);
			$urlErr .= '&val403=' . valueUrlEncode($request->val403);
			$urlErr .= '&val404=' . valueUrlEncode($request->val404);
			$urlErr .= '&err1=' . valueUrlEncode($originalError);
			return redirect($urlErr);
		}

		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/search';
		$url .= '?cmn1=' . $request->cmn1;
		$url .= '&cmn2=' . $request->cmn2;
		$url .= '&pageunit=' . $request->pageunit;
		$url .= '&page=' . $request->page;
		$url .= '&sort=' . $request->sort;
		$url .= '&direction=' . $request->direction;
		$url .= '&val1=' . $request->val1;
		$url .= '&val2=' . $request->val2;
		$url .= '&val3=' . $request->val3;
		$url .= '&val4=' . $request->val4;
		$url .= '&val5=' . $request->val5;
		$url .= '&val101=' . $request->val101;
		return redirect($url);
	}
}
