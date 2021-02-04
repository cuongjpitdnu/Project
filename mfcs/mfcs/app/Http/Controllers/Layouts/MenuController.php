<?php
/*
 * @MenuController.php
 * メニューコントローラーファイル
 *
 * @create 2020/07/09 KBS K.Yoshihara
 * @update 2020/07/21 KBS K.Yoshihara POST の GET 化対応
 * @update 2020/10/20 KBS K.Yoshihara フラッシュメッセージ対応
 * @update 2020/12/10 KBS K.Yoshihara 不安定ソート対策
 * @update 2020/12/23 KBS K.Yoshihara 列ヘッダークリック時のソート基準を変更
 */

namespace App\Http\Controllers\Layouts;

use DB;
use App\Http\Controllers\Controller;
use App\Librarys\GetUserErrorException;
use App\Librarys\UnregisteredUserException;
use App\Librarys\FuncCommon;
use App\Librarys\MenuInfo;
use App\Models\Information;
use App\Models\SystemLog;
use App\Models\T_ImportHistory;
use Illuminate\Http\Request;
use Illuminate\Pagination\LengthAwarePaginator;


/*
 * メニューコントローラー
 *
 * @create 2020/07/09 KBS K.Yoshihara
 * @update 2020/07/21 KBS K.Yoshihara POST の GET 化対応
 * @update 2020/10/20 KBS K.Yoshihara フラッシュメッセージ対応
 * @update 2020/12/10 KBS K.Yoshihara 不安定ソート対策
 * @update 2020/12/23 KBS K.Yoshihara 列ヘッダークリック時のソート基準を変更
 */
class MenuController extends Controller
{

	/**
	 * GET メインメニュー(兼 ログイン)アクション
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return view ビュー
	 *
	 * @create 2020/07/09 K.Yoshihara
	 * @update 2020/10/20 K.Yoshihara フラッシュメッセージ対応
	 */
	public function main(Request $request)
	{

		// エラーメッセージが指定されている場合、リダイレクトしてフラッシュメッセージを表示する
		if (isset($request->err1)) {
			return redirect('/index')->with('menu_index_message', valueUrlDecode($request->err1));
		}

		$loginUserInfo = null; // ログイン情報
		$userID = null; // ユーザーID
		$guestID = null; // ゲストユーザーの場合のWindowsログインユーザーID
		$action2 = null; // 動作2
		try {

			// ログイン処理
			FuncCommon::runLogin();

			// セッション変数からログイン情報を取得
			$loginUserInfo = session('LOGINUSER_INFO');

			// ログインに成功したユーザーIDを取得
			$userID = $loginUserInfo['userid'];
			$guestID = $loginUserInfo['guestid'];

			// 設定から動作2を取得
			if (isset($guestID)) {
				$action2 = config('system_const.syslog_action2_unregistered');
			}else{
				$action2 = config('system_const.syslog_action2_registered');
			}

		} catch (GetUserErrorException $ex) {

			// 設定から動作2を取得
			$action2 = config('system_const.syslog_action2_getusererror');

			// ログインユーザーIDの取得に失敗した場合は専用のエラーページに飛ばす
			return redirect('errors/getusererror');
		} catch (UnregisteredUserException $ex) {

			// ログインに失敗したユーザーIDを取得
			$userID = $ex->UserID;

			// 設定から動作2を取得
			$action2 = config('system_const.syslog_action2_unregistered');

			// ログインユーザーIDに一致するレコードがユーザーマスタに存在しない場合は専用のエラーページに飛ばす
			return redirect('errors/unregistereduser');
		}
		finally{
			// ログを出力するか
			if (config('system_config.log_system_flag') == '1') {
				// システムログを出力
				DB::transaction(function () use ($userID, $action2, $guestID) {
					$systemLog = new SystemLog();
					// ユーザーID
					if (isset($guestID)) {
						$systemLog->UserID = $guestID;
					}
					else{
						$systemLog->UserID = $userID;
					}
					$systemLog->Action1 = config('system_const.syslog_action1_login'); // 動作1
					$systemLog->Action2 = $action2; // 動作2
					$systemLog->save(); // 保存実行
				});
			}
		}

		//ロック削除
		$this->deleteLockAll(session()->getId());

		// お知らせのレコードを取得
		$informations = $this->getRecords($request);

		// メニュー表示用の変数を作成
		$menuInfo = new MenuInfo();
		$menuInfo->UserName = $loginUserInfo['username'];
		$menuInfo->Kinds = $loginUserInfo['syskind'];
		$menuInfo->Menus = $loginUserInfo['sysmenu'];

		// ビューを表示
		return view('layouts/mainmenu/index')->with([
			'menuInfo' => $menuInfo,
			'informations' => $informations,
		]);
	}

	/**
	 * お知らせレコードの取得
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return Information お知らせレコード
	 *
	 * @create 2020/07/13 K.Yoshihara
	 * @update 2020/12/10 K.Yoshihara 不安定ソート対策
	 * @update 2020/12/23 K.Yoshihara 列ヘッダークリック時のソート基準を変更
	 */
	private function getRecords(Request $request)
	{

		if(isset($request->pageunit) && in_array($request->pageunit, [config('system_const.displayed_results_1'),
																	  config('system_const.displayed_results_2'),
																	  config('system_const.displayed_results_3')])){
			$pageunit = $request->pageunit;
		}else{
			$pageunit = config('system_const.displayed_results_1');
		}

		$sort = ['fld1','fld2'];
		if (isset($request->sort) && $request->sort != '') {
			if (trim($request->sort) == 'fld2') {
				$sort = ['fld2','fld1'];
			} else if (trim($request->sort) == 'fld1') {
				$sort = ['fld1', 'fld2'];
			} else {
				$sort = [$request->sort, 'fld1', 'fld2'];
			}
		}

		$direction = (isset($request->direction) && $request->direction != '') ?  $request->direction : 'asc';

		$query = Information::select('Updated_at as fld1', 'Message as fld2')
		->whereRaw(
			'      (sdate IS NULL OR (CONVERT(date, sdate) <= CONVERT(date, GETDATE())))'
			. '　AND (edate IS NULL OR (CONVERT(date, edate) >= CONVERT(date, GETDATE())))'
			)
		->get();

		$informations = $this->sortAndPagination($query, $sort, $direction, $pageunit, $request);

		return $informations;

	}

	/**
	 * 搭載日程取得履歴の取得
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return array 搭載日程取得履歴
	 *
	 * @create 2020/08/25 T.Nishida
	 * @update
	 */
	private function getT_ImportHistory(Request $request)
	{
		$getLists = T_ImportHistory::from('T_ImportHistory as A');
		$getLists = $getLists->select(
			'A.ID as fld1'
			, 'A.Import_Date as fld2'
			, 'A.ProjectID as fld3'
			, 'B.ProjectName as fld4'
			, 'A.OrderNo as fld5'
			, 'A.LinkFlag as fld6'
			, 'A.StatusFlag as fld7'
		);
		$getLists = $getLists->leftjoin('mstProject as B', 'A.ProjectID', '=', 'B.ID');
		if (!isset($request->sort)) {
			//初回表示
			$getLists = $getLists->orderby('A.Import_Date', 'desc');
			$getLists = $getLists->sortable();
		}else{
			$getLists = $getLists->sortable(['fld2', 'fld4', 'fld5', 'fld6', 'fld7']);
		}
		if (!isset($request->pageunit)) {
			//初回表示
			$getLists = $getLists->paginate(config('system_const.displayed_results_1'));
		}else{
			$getLists = $getLists->paginate($request->pageunit);
		}
		return $getLists;
	}

	/**
	 * 搭載日程取得履歴(エラー)の取得
	 *
	 * @param
	 * @return array 搭載日程取得履歴(エラー)
	 *
	 * @create 2020/08/25 T.Nishida
	 * @update
	 */
	private function getT_ImportHistoryErr()
	{
		$subQuery = T_ImportHistory::select(
				'ProjectID'
				, 'OrderNo'
				, DB::raw('max(Updated_at) as Updated_at')
			)
			->groupby('ProjectID', 'OrderNo');

		$getErrLists = T_ImportHistory::from('T_ImportHistory as A')
			->select(
				'A.ID as ID'
				, 'A.Import_Date as Import_Date'
				, 'A.ProjectID as ProjectID'
				, 'C.ProjectName as ProjectName'
				, 'A.OrderNo as OrderNo'
				, 'A.LinkFlag as LinkFlag'
				, 'A.StatusFlag as StatusFlag'
			)
			->joinsub($subQuery, 'B', function($join) {
					$join->on('A.ProjectID', '=', 'B.ProjectID');
					$join->on('A.OrderNo', '=', 'B.OrderNo');
					$join->on('A.Updated_at', '=', 'B.Updated_at');
				})
			->leftjoin('mstProject as C', 'A.ProjectID', '=', 'C.ID')
			->wherein('A.StatusFlag', [config('system_const_schet.schet_import_status_error'), config('system_const_schet.schet_import_status_running')])
			->orderby('A.Updated_at', 'asc')
			->get();

		return $getErrLists;
	}

	/**
	 * POST サブメニューアクション
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/07/09　K.Yoshihara
	 * @update 2020/07/21  K.Yoshihara POST の GET 化対応に伴い、URL直打ち対策追加
	 */
	public function sub(Request $request)
	{

		// セッション変数からログイン情報を取得
		$loginUserInfo = session('LOGINUSER_INFO');

		if (!isset($loginUserInfo)) {
			// キャッシュ切れ
			return redirect('/index?err1=' . valueUrlEncode(config('message.msg_cmn_auth_001')));
		}

		// システム種類マスタレコードの配列から該当する項目を取得
		$kinds = $loginUserInfo['syskind'];
		$kindID = valueUrlDecode($request->cmn1);
		foreach ($kinds as $kindLoop) {
			if ($kindLoop['id'] == $kindID) {
				$kindItem = $kindLoop;
				break;
			}
		}

		if (!isset($kindItem)) {
			// システム種類マスタレコードが見つからない(操作権限が無い or システム未定義の画面に対し、URLが直打ちで画面遷移された)
			return redirect('/index?err1=' . valueUrlEncode(config('message.msg_cmn_auth_003')));
		}

		// 操作権限が有るか？
		$isReadOnly = null;
		if (!FuncCommon::isPermissionMenu($kindItem['url'], null, $isReadOnly))
		{
			// 無いならトップページにリダイレクト
			return redirect('/index?err1=' . valueUrlEncode(config('message.msg_cmn_auth_002')));
		}

		// ログを出力するか
		if (config('system_config.log_system_flag') == '1') {
			// システムログを出力
			DB::transaction(function () use ($loginUserInfo, $request, $kindItem) {
				$systemLog = new SystemLog();
				// ユーザーID
				if (isset($loginUserInfo['guestid'])) {
					$systemLog->UserID = $loginUserInfo['guestid'];
				}
				else{
					$systemLog->UserID = $loginUserInfo['userid'];
				}
				$systemLog->Action1 = config('system_const.syslog_action1_menu'); // 動作1
				$systemLog->Action2 = $kindItem['sysname']; // 動作2
				$systemLog->save(); // 保存実行
			});
		}

		//ロック削除
		$this->deleteLockAll(session()->getId());

		// メニュー表示用の変数を作成
		$menuInfo = new MenuInfo();
		$menuInfo->UserName = $loginUserInfo['username']; // ユーザー名
		$menuInfo->Kinds = $loginUserInfo['syskind']; // システム種類マスタレコードの配列
		$menuInfo->Menus = $loginUserInfo['sysmenu']; // システムメニューマスタレコードの配列
		$menuInfo->KindID = $kindID; // システム種類ID
		$menuInfo->KindURL = $kindItem['url']; //システムURL

		//各メニュー毎のメイン画面に出すデータを取得
		$listDatas = array();
		$originalError = array();
		switch ($kindItem['url']) {
			case 'schet':
				//取込履歴を取得
				$listDatas = $this->getT_ImportHistory($request);
				$listErrDatas = $this->getT_ImportHistoryErr();

				//DBからシステム日付を取得
				$sysDate = strtotime(DB::selectOne('SELECT getdate() AS sysdate')->sysdate);

				//エラー履歴を設定
				foreach ($listErrDatas as $listErrData) {
					switch ($listErrData['StatusFlag']) {
						case config('system_const_schet.schet_import_status_running'):
							$overTime = strtotime($listErrData['Import_Date'] . '+60 minute');
							if ($sysDate > $overTime) {
								$originalError[] = config('message.msg_notice_schet_004')
									. '[検討ケース：' . $listErrData['ProjectName'] . ']'
									. '[オーダ：' . $listErrData['OrderNo'] . ']';
							}
							break;
						case config('system_const_schet.schet_import_status_error'):
							$originalError[] = config('message.msg_notice_schet_005')
									. '[検討ケース：' . $listErrData['ProjectName'] . ']'
									. '[オーダ：' . $listErrData['OrderNo'] . ']';
							break;
						default:
							break;
					}
				}

				break;

			default:
				break;
		}

		// ビューを表示
		return view('layouts/submenu/' . $kindItem['url'])->with([
			'menuInfo' => $menuInfo,
			'request' => $request,
			'listDatas' => $listDatas,
			'originalError' => $originalError,
		]);
	}

}
