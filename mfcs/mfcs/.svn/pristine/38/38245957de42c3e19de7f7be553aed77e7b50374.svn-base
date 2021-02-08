<?php
/*
 * @OrgController.php
 * 職制マスタコントローラーファイル
 *
 * @create 2020/07/09 KBS K.Yoshihara
 *
 * @update 2020/07/21 KBS K.Yoshihara POST の GET 化対応
 *
 * @update 2020/08/25 Cuong
 * @update 2020/10/22 Cuong isEdit=trueの場合 update PID
 * @update 2020/10/22 Cuong 排他ロックが解除されていないかの確認
 */

namespace App\Http\Controllers\Mst;

use DB;
use App\Http\Controllers\Controller;
use App\Http\Requests\Mst\OrgIndexDateRequest;
use App\Http\Requests\Mst\OrgIndexSelectRequest;
use App\Http\Requests\Mst\OrgContentsRequest;
use App\Librarys\FuncCommon;
use App\Librarys\MstOrgCommon;
use App\Models\SystemLog;
use App\Models\MstOrg;
use Illuminate\Http\Request;
use Illuminate\Database\QueryException;
use DateTime;
use Carbon\Carbon;

/*
 * 職制マスタコントローラー
 *
 * @create 2020/07/09 KBS K.Yoshihara
 *
 * @update
 */
class OrgController extends Controller
{
	/**
	 * GET 職制マスタトップ画面アクション
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/07/09　K.Yoshihara
	 * @update 2020/07/22　K.Yoshihara リファクタリング
	 */
	public function index(Request $request)
	{
		return $this->initialize($request);
	}

	/**
	 * GET 職制マスタトップ画面日付変更アクション
	 *
	 * @param OrgIndexDateRequest 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/07/09　K.Yoshihara
	 * @update 2020/07/22　K.Yoshihara リファクタリング
	 */
	public function changeDate(OrgIndexDateRequest $request)
	{
		return $this->initialize($request);
	}

	/**
	 * 職制マスタトップ画面初期表示処理
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/07/22　K.Yoshihara
	 * @update 2020/08/31　K.Yoshihara 権限チェックに関する全画面への改修
	 * @update 2020/11/04　K.Yoshihara 職制を選択した後に基準日を再選択した時、基準日に一致する職制が無い場合は選択したIDをnullに戻すように変更。
	 *                                 基準日をyyyy/mm/ddにフォーマットする処理を追加。
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

		// 基準日が日付でない場合nullに戻す
		$baseDate = $request->val2;
		if (isset($baseDate)) {
			try {
				list($Y, $m, $d) = explode('/', $baseDate);
				if (!checkdate($m, $d, $Y)) {
					$baseDate = null;
				}
			} catch (\exception $e) {
				$baseDate = null;
			}
		}

		if (isset($baseDate)) {
			$baseDate = date('Y/m/d', strtotime($baseDate));
		}
		else{
			$baseDate = DB::selectOne('SELECT CONVERT(DATE, getdate()) AS sysdate')->sysdate;
			$baseDate = str_replace('-', '/', $baseDate);
		}

		$mstOrgCommon = new MstOrgCommon($baseDate);

		$activeOrgID = valueUrlDecode($request->val1);

		$activeHasChild = '';
		if (isset($activeOrgID)) {
			$ret = $mstOrgCommon->getChildID($activeOrgID);
			if (isset($ret) && count($ret) > 0) {
				$activeHasChild = '1';
			}
			$orgName = $mstOrgCommon->getGrpName($activeOrgID);
			if ($orgName == config('system_const.org_null_name')) {
				$activeOrgID = null;
			}
		}

		$originalError = array();

		if(!$menuInfo->IsReadOnly) {
			$lockError = $this->tryLock($menuInfo->KindID, $menuInfo->MenuID, $menuInfo->UserID, $menuInfo->SessionID);
			if (isset($lockError)){
				$menuInfo->IsReadOnly = true;
				$originalError[] = $lockError;
			}
		}

		$activeSDate = valueUrlDecode($request->val3);

		// ビューを表示
		return view('mst/org/index')->with([
			'menuInfo' => $menuInfo,
			'baseDate' => $baseDate,
			'activeOrgID' => $activeOrgID,
			'mstOrgCommon' => $mstOrgCommon,
			'originalError' => $originalError,
			'activeHasChild' => $activeHasChild,
			'activeSDate' => $activeSDate,
		]);

	}

	/**
	 * GET 職制マスタ参照ボタンアクション
	 *
	 * @param OrgIndexSelectRequest 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/07/17　K.Yoshihara
	 * @update 2020/08/31　K.Yoshihara 権限チェックに関する全画面への改修
	 * @update 2020/12/10 Cuong formatDecToText
	 */
	public function show(OrgIndexSelectRequest $request)
	{

		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$activeOrgID = valueUrlDecode($request->val1);

		$baseDate = $request->val2;
		$mstOrgCommon = new MstOrgCommon($baseDate);
		$data = $mstOrgCommon->getDataFromID($activeOrgID);

		$mstOrg = array();
		$mstOrg['val101'] = $data['pid'];
		$mstOrg['val102'] = $data['syokuseicode'];
		$mstOrg['val103'] = $data['name'];
		$mstOrg['val104'] = $data['nick'];
		$mstOrg['val105'] = $data['outinflag'];
		$mstOrg['val106'] = $data['buoutinflag'];
		$mstOrg['val107'] = $data['outpid'];
		$mstOrg['val108'] = $data['outtype'];
		$mstOrg['val109'] = $data['vendercode'];
		$mstOrg['val110'] = $data['folderflag'];
		$mstOrg['val111'] = FuncCommon::formatDecToText($data['sortno']);
		$mstOrg['val112'] = !empty($data['sdate']) ? Carbon::parse($data['sdate'])->format('Y/m/d') : null;
		$mstOrg['val113'] = !empty($data['edate']) ? Carbon::parse($data['edate'])->format('Y/m/d') : null;
		$mstOrg['val114'] = $data['updated_at'];

		if($mstOrg['val101'] == 0){
			$parentName  = config('system_const.org_root_name');
		}else{
			$parentName = $mstOrgCommon->getGrpName($mstOrg['val101']);
		}

		$arrKanren = $mstOrgCommon->getKanrenID();
		
		if(count($arrKanren) > 0) {
			foreach($arrKanren as &$item) {
				$item['name'] = htmlentities($item['name']);
			}
		}
		//prepare data
		$this->data['menuInfo'] = $menuInfo;
		$this->data['request'] = $request;
		$this->data['mstOrg'] = $mstOrg;
		$this->data['parentName'] = $parentName;
		$this->data['mstOrgCommon'] = $mstOrgCommon;
		$this->data['arrKanren'] = $arrKanren;
		//return view with all data
		return view('mst/org/show', $this->data);
	}

	/**
	 * GET 職制マスタ新規ボタンアクション
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/07/17　K.Yoshihara
	 * 2020/08/31　K.Yoshihara 権限チェックに関する全画面への改修
	 * @update 2020/08/25 Cuong
	 * @update 2020/10/22 Cuong 排他ロックが解除されていないかの確認
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

		// 排他ロックが解除されていないかの確認
		$existLog = $this->existsLock($menuInfo->KindID, $menuInfo->MenuID,
		$menuInfo->SessionID);
		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($existLog)) {
			// エラーが起きたのでリダイレクト
			return $existLog;
		}

		$activeOrgID = valueUrlDecode($request->val1);
		$baseDate = $request->val2;
		$mstOrg = array();
		$mstOrg['val101'] = 0;
		$mstOrgCommon = new MstOrgCommon($baseDate);
		if(!empty($activeOrgID)){
			$activeOrg = $mstOrgCommon->getDataFromID($activeOrgID);

			while (!is_null($activeOrg)) {
				if ($activeOrg['folderflag'] == 1) {
					$mstOrg['val101'] = $activeOrg['id'];
					break;
				}
				if ($activeOrg['pid'] == 0) {
					break;
				}else{
					$activeOrg = $mstOrgCommon->getDataFromID($activeOrg['pid']);
				}
			}
		}

		if ($mstOrg['val101'] == 0) {
			$parentName = config('system_const.org_root_name');
		}else{
			$parentName = $mstOrgCommon->getGrpName($mstOrg['val101']);
		}
		$errorMessage = $this->tryLock($menuInfo->KindID, $menuInfo->MenuID, $menuInfo->UserID, $menuInfo->SessionID);
		if(!empty($errorMessage)){
			$url = url('/');
			$url .= '/' . $menuInfo->KindURL;
			$url .= '/' . $menuInfo->MenuURL;
			$url .= '/index';
			$url .= '?cmn1=' . $request->cmn1;
			$url .= '&cmn2=' . $request->cmn2;
			$url .= '&val1=' . $request->val1;
			$url .= '&val2=' . $request->val2;
			$url .= '&val3=' . $request->val3;
			$url .= '&err1=' . valueUrlEncode($errorMessage);
			return redirect($url);
		}

		$arrKanren = $mstOrgCommon->getKanrenID();
		
		if(count($arrKanren) > 0) {
			foreach($arrKanren as &$item) {
				$item['name'] = htmlentities($item['name']);
			}
		}

		//prepare data
		$this->data['request'] = $request;
		$this->data['menuInfo'] = $menuInfo;
		$this->data['mstOrg'] = $mstOrg;
		$this->data['parentName'] = $parentName;
		$this->data['mstOrgCommon'] = $mstOrgCommon;
		$this->data['arrKanren'] = $arrKanren;
		//return view with all data
		return view('mst/org/create', $this->data);
	}

	/**
	 * GET 職制マスタ編集ボタンアクション
	 *
	 * @param OrgIndexSelectRequest 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/07/17　K.Yoshihara
	 * @update 2020/08/31　K.Yoshihara 権限チェックに関する全画面への改修
	 * @update 2020/08/25 Cuong
	 * @update 2020/10/22 Cuong 排他ロックが解除されていないかの確認
	 * @update 2020/12/10 Cuong formatDecToText
	 */
	public function edit(OrgIndexSelectRequest $request)
	{
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		// 排他ロックが解除されていないかの確認
		$existLog = $this->existsLock($menuInfo->KindID, $menuInfo->MenuID,
		$menuInfo->SessionID);
		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($existLog)) {
			// エラーが起きたのでリダイレクト
			return $existLog;
		}

		$activeOrgID = valueUrlDecode($request->val1);
		$baseDate = $request->val2;
		$mstOrgCommon = new MstOrgCommon($baseDate);
		$data = $mstOrgCommon->getDataFromID($activeOrgID);

		$mstOrg = array();
		$mstOrg['val101'] = $data['pid'];
		$mstOrg['val102'] = $data['syokuseicode'];
		$mstOrg['val103'] = $data['name'];
		$mstOrg['val104'] = $data['nick'];
		$mstOrg['val105'] = $data['outinflag'];
		$mstOrg['val106'] = $data['buoutinflag'];
		$mstOrg['val107'] = $data['outpid'];
		$mstOrg['val108'] = $data['outtype'];
		$mstOrg['val109'] = $data['vendercode'];
		$mstOrg['val110'] = $data['folderflag'];
		$mstOrg['val111'] = FuncCommon::formatDecToText($data['sortno']);
		$mstOrg['val112'] = !empty($data['sdate']) ? Carbon::parse($data['sdate'])->format('Y/m/d') : null;
		$mstOrg['val113'] = !empty($data['edate']) ? Carbon::parse($data['edate'])->format('Y/m/d') : null;
		$mstOrg['val114'] = $data['updated_at'];
		$mstOrg['val115'] = $data['lv_no'];

		if($mstOrg['val101'] == 0){
			$parentName  = config('system_const.org_root_name');
		}else{
			$parentName = $mstOrgCommon->getGrpName($mstOrg['val101']);
		}
		$errorMessage = $this->tryLock($menuInfo->KindID, $menuInfo->MenuID, $menuInfo->UserID, $menuInfo->SessionID);
		if(!empty($errorMessage)){
			$url = url('/');
			$url .= '/' . $menuInfo->KindURL;
			$url .= '/' . $menuInfo->MenuURL;
			$url .= '/index';
			$url .= '?cmn1=' . $request->cmn1;
			$url .= '&cmn2=' . $request->cmn2;
			$url .= '&val1=' . $request->val1;
			$url .= '&val2=' . $request->val2;
			$url .= '&val3=' . $request->val3;
			$url .= '&err1=' . valueUrlEncode($errorMessage);
			return redirect($url);
		}

		$arrKanren = $mstOrgCommon->getKanrenID();
		
		if(count($arrKanren) > 0) {
			foreach($arrKanren as &$item) {
				$item['name'] = htmlentities($item['name']);
			}
		}

		//prepare data
		$this->data['menuInfo'] = $menuInfo;
		$this->data['request'] = $request;
		$this->data['mstOrg'] = $mstOrg;
		$this->data['parentName'] = $parentName;
		$this->data['mstOrgCommon'] = $mstOrgCommon;
		$this->data['arrKanren'] = $arrKanren;

		//return view with all data
		return view('mst/org/edit', $this->data);
	}

	/**
	 * POST 職制マスタ保存ボタンアクション
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/07/27　K.Yoshihara
	 * @update 2020/08/31　K.Yoshihara 権限チェックに関する全画面への改修
	 * @update 2020/08/27 Cuong
	 * @update 2020/10/22 Cuong isEdit=trueの場合 update PID
	 * @update 2020/10/22 Cuong 排他ロックが解除されていないかの確認
	 * @update 2020/10/22 Cuong isEdit=trueの場合 update LV_No
	 */
	public function save(OrgContentsRequest $request)
	{
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		// 排他ロックが解除されていないかの確認
		$existLog = $this->existsLock($menuInfo->KindID, $menuInfo->MenuID,
		$menuInfo->SessionID);
		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($existLog)) {
			// エラーが起きたのでリダイレクト
			return $existLog;
		}
		//validate form
		$validated = $request->validated();

		$errorMessage = $this->tryLock($menuInfo->KindID, $menuInfo->MenuID, $menuInfo->UserID, $menuInfo->SessionID);

		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/index';
		$url .= '?cmn1=' . $request->cmn1;
		$url .= '&cmn2=' . $request->cmn2;
		$url .= '&val1=' . $request->val1;
		$url .= '&val2=' . $request->val2;
		$url .= '&val3=' . $request->val3;

		if(!empty($errorMessage)){
			$url .= '&err1=' . valueUrlEncode($errorMessage);
			return redirect($url);
		}

		$activeOrgID = valueUrlDecode($request->val1);
		$baseDate = $request->val2;
		$mstOrgCommon = new MstOrgCommon($baseDate);
		$sDate = $request->val112;

		$PID = $request->val101;
		$lvNo = 1;
		if ($PID != 0) {
			$lvNo = count($mstOrgCommon->getPIDAll($PID)) + 2;
		}
		$isEdit = false;

		if ($request->method == 'edit' && $sDate == $baseDate) {
			$isEdit = true;
		}

		if (!$isEdit) {
			
			DB::transaction(function () use ($baseDate, $sDate, $lvNo, $activeOrgID, $request) {
				
				if ($request->method =='edit') {
					$result = MstOrg::query()
					->where('ID', $activeOrgID)
					->where('Sdate', Carbon::parse($sDate))
					->update(['Edate' => Carbon::parse($baseDate)->subDay()]);
				}

				$org = new MstOrg();
				if ($request->method == 'create') {
					$seq_mstOrg = DB::select('SELECT NEXT VALUE FOR seq_mstOrg as orgID');
					$orgID = $seq_mstOrg[0]->orgID;
					$org->ID = $orgID;
				}else{
					$org->ID = $activeOrgID;
				}

				$org->PID = $request->val101;
				$org->SyokuseiCode = $request->val102;
				$org->Name = $request->val103;
				$org->Nick = $request->val104;
				$org->OutInFlag = $request->val105;
				$org->BuOutInFlag = $request->val106;
				$org->OutPID = $request->val107;
				$org->OutType = $request->val108;
				$org->VenderCode = $request->val109;
				$org->FolderFlag = $request->val110;
				$org->SortNo = !is_null($request->val111)? $request->val111 : 0;
				$org->Sdate = $baseDate;
				$org->Edate = $request->val113;
				$org->LV_No = $lvNo;
				$org->Up_User = session('LOGINUSER_INFO')['userid'];
				$org->save();

			});
		}
		else {
			DB::transaction(function () use ($activeOrgID, $baseDate, $validated, $request, $lvNo) {

				$dataOrg['SyokuseiCode'] = $request->val102;
				$dataOrg['Name'] = $request->val103;
				$dataOrg['Nick'] = $request->val104;
				$dataOrg['OutInFlag'] = $request->val105;
				$dataOrg['BuOutInFlag'] = $request->val106;
				$dataOrg['OutPID'] = $request->val107;
				$dataOrg['OutType'] = $request->val108;
				$dataOrg['VenderCode'] = $request->val109;
				$dataOrg['SortNo'] = !is_null($request->val111)? $request->val111 : 0;
				$dataOrg['Up_User'] = session('LOGINUSER_INFO')['userid'];
				$dataOrg['PID'] = $request->val101;
				$dataOrg['LV_No'] = $lvNo;
				$result = MstOrg::query()
					->where('ID', $activeOrgID)
					->where('Sdate', Carbon::parse($baseDate))
					->update($dataOrg);
			});
		}
		return redirect($url);
	}

	/**
	 * POST 職制マスタ削除ボタンアクション
	 *
	 * @param OrgIndexSelectRequest 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/07/17　K.Yoshihara
	 * @update 2020/08/31　K.Yoshihara 権限チェックに関する全画面への改修
	 */
	public function delete(OrgIndexSelectRequest $request)
	{

		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		// 排他ロックが有効かチェック
		$ret = $this->existsLock($menuInfo->KindID, $menuInfo->MenuID, $menuInfo->SessionID);
		if ($this->isRedirectMenuInfo($ret)) {
			// エラーが起きたのでリダイレクト
			return $ret;
		}

		$result = $this->tryLock($menuInfo->KindID, $menuInfo->MenuID, $menuInfo->UserID, $menuInfo->SessionID);

		if(!is_null($result)){
			$errorMessage = $result;

			$url = url('/');
			$url .= '/' . $menuInfo->KindURL;
			$url .= '/' . $menuInfo->MenuURL;
			$url .= '/index';
			$url .= '?cmn1=' . $request->cmn1;
			$url .= '&cmn2=' . $request->cmn2;
			$url .= '&val1=' . $request->val1;
			$url .= '&val2=' . $request->val2;
			$url .= '&val3=' . $request->val3;
			$url .= '&err1=' . valueUrlEncode($errorMessage);

			return redirect($url);
		}

		$activeOrgID = valueUrlDecode($request->val1);
		$baseDate = $request->val2;

		$result = MstOrg::where('ID', '=', $activeOrgID)
					->whereDate('Sdate', '=', valueUrlDecode($request->val3))
					->get();

		$pID = $result[0]->PID;

		//変数初期化
		$deleteItems = array();
		$editItems = array();

		$this->getTargetDeleteEdit($activeOrgID, $pID, $baseDate, $deleteItems, $editItems);

		DB::transaction(function() use ($deleteItems, $editItems, $baseDate, $menuInfo){
			foreach ($deleteItems as $deleteItem) {
				MstOrg::where('ID', '=', $deleteItem->ID)
					->whereDate('Sdate', '=', $deleteItem->Sdate)
					->delete();
			}
			$dayBefore = date("Y/m/d", strtotime("$baseDate -1 day"));
			foreach ($editItems as $editItem) {
				MstOrg::where('ID', '=', $editItem->ID)
					->whereDate('Sdate', '=', $editItem->Sdate)
					->update(['Up_User' => $menuInfo->UserID,
								'Edate' => $dayBefore]);
			}
		});

		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/index';
		$url .= '?cmn1=' . $request->cmn1;
		$url .= '&cmn2=' . $request->cmn2;
		$url .= '&val2=' . $request->val2;

		return redirect($url);
	}

	/**
	 * 削除編集対象取得処理
	 *
	 * @param grpID
	 * @param baseDate
	 * @param deleteItems
	 * @param editItems
	 *
	 * @create 2020/08/04　S.Tanaka
	 * @update
	 */
	private function getTargetDeleteEdit($grpID, $pID, $baseDate, &$deleteItems, &$editItems)
	{

		$orgItems = MstOrg::where('ID', '=', $grpID)
						->where('PID', '=', $pID)
						->get();

		foreach($orgItems as $orgItem){
			$date = new DateTime($orgItem->Sdate);
			$sDate = $date->format('Y/m/d');
			if(is_null($orgItem->Edate)){
				$eDate = $orgItem->Edate;
			}else{
				$date = new DateTime($orgItem->Edate);
				$eDate = $date->format('Y/m/d');
			}

			if($sDate >= $baseDate){
				$deleteItems[] = $orgItem;
			}elseif($sDate < $baseDate && ($eDate == NULL || $eDate >= $baseDate)){
				$editItems[] = $orgItem;
			}
		}

		$childs = MstOrg::where('PID', '=', $grpID)
						->distinct()
						->pluck('ID');

		foreach($childs as $childID){
			$this->getTargetDeleteEdit($childID, $grpID, $baseDate, $deleteItems, $editItems);
		}
	}

	/**
	 * POST 職制マスタ削除前確認処理
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/08/04　S.Tanaka
	 * @update 2020/08/31　K.Yoshihara 権限チェックに関する全画面への改修
	 */
	public function checkDelete(Request $request)
	{

		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト

			$json = '';
			$json .= '{"status":"' . config("system_const.json_status_ng") . '"';
			$json .= ',"message":"' . config("message.msg_json_001") . '"}';

			return $json;
		}

		$value = $this->tryLock($menuInfo->KindID, $menuInfo->MenuID, $menuInfo->UserID, $menuInfo->SessionID);
		if(!is_null($value)){

			$json = '';
			$json .= '{"status":"' . config("system_const.json_status_ng") . '"';
			$json .= ',"message":"' . $value . '"}';

			return $json;
		}

		$activeOrgID = valueUrlDecode($request->val1);

		$baseDate = $request->val2;

		$sDate = valueUrlDecode($request->val3);

		$active = MstOrg::where('ID', '=', $activeOrgID)
						->whereDate('Sdate', '=', $sDate)
						->first();

		if(is_null($active)){

			$json = '';
			$json .= '{"status":"' . config("system_const.json_status_ng") . '"';
			$json .= ',"message":"' . config("message.msg_cmn_db_002") . '"}';

			return $json;
		}

		$existsFuture = MstOrg::where('ID', '=', $activeOrgID)
							->whereDate('Sdate', '>', $active->Sdate)
							->exists();

		$status = 'null';
		if($existsFuture){
			$status = config("system_const.json_status_ng");
		}

		$existsChild = MstOrg::where('PID', '=', $activeOrgID)
							->where(function ($query) use ($baseDate){
								$query->whereNull('Edate')
									->orwhere('Edate', '>=', $baseDate);
							})
							->exists();

		$has_child = 'null';
		if($existsChild){
			$has_child = config('system_const.json_return_true');
		}

		$json = '';
		$json .= '{"status":"' . $status . '"';
		$json .= ',"has_child":"' . $has_child . '"';
		$json .= ',"message":null}';

		return $json;
	}
}
