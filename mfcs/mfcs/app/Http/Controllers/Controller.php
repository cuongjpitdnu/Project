<?php
/*
 * @Controller.php
 * ベースコントローラーファイル
 *
 * @create 2020/07/09 KBS K.Yoshihara
 *
 * @update
 */

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Foundation\Auth\Access\AuthorizesRequests;
use Illuminate\Foundation\Bus\DispatchesJobs;
use Illuminate\Foundation\Validation\ValidatesRequests;
use Illuminate\Routing\Controller as BaseController;
use Illuminate\Pagination\LengthAwarePaginator;

use DB;
use App\Librarys\FuncCommon;
use App\Librarys\MenuInfo;
use App\Models\SystemLog;
use App\Models\SystemLock;
use App\Models\MstUser;

/*
 * ベースコントローラークラス
 *
 * @create 2020/07/09 KBS K.Yoshihara
 * @update
 */
class Controller extends BaseController
{
	use AuthorizesRequests, DispatchesJobs, ValidatesRequests;
	/**
	 * @create 2020/08/03 Thang
	 */
	public $data;
	public $controller;//use controller's functions in blade view
	public function __construct(Request $request)
	{
		$this->data['controller'] = $this;
	}
	/**
	 * ログイン状態のチェックとメニュー表示用情報の作成、操作ログの出力
	 *
	 * @param Request 呼び出し元のリクエストオブジェクト
	 * @param int 呼び出し元がどのような権限を必要とするか
	 *                 system_const.authority_all = 読み取り専用と書き込み権限どちらでも可。
	 *                 system_const.authority_readonly = 読み取り専用の権限が無いとトップ画面に戻る。
	 *                 system_const.authority_editable = 書き込み権限が無いとトップ画面に戻る。
	 * @return mix MenuInfo RedirectResponse
	 *              メニュー表示用情報 ただし、エラーが起きた場合はリダイレクト先の Illuminate\\Http\\RedirectResponse オブジェクト
	 *
	 * @create 2020/07/09　K.Yoshihara
	 * @update 2020/07/21  K.Yoshihara POST の GET 化対応、キャッシュ切れ対策追加
	 * @update 2020/08/28  K.Yoshihara URL手打ちで権限がないものを表示出来ていたのを修正
	 * @update 2020/08/31  K.Yoshihara 引数 authority を追加
	 */
	protected function checkLogin(Request $request, $authority)
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

		// システムメニューマスタレコードの配列から該当する項目を取得
		$menus = $loginUserInfo['sysmenu'];
		$menuID = valueUrlDecode($request->cmn2);
		foreach ($menus as $menuLoop) {
			if ($menuLoop['id'] == $menuID) {
				$menuItem = $menuLoop;
				break;
			}
		}

		if (!isset($menuItem)) {
			// システムメニューマスタレコードが見つからない(操作権限が無い or システム未定義の画面に対し、URLが直打ちで画面遷移された)
			return redirect('/index?err1=' . valueUrlEncode(config('message.msg_cmn_auth_003')));
		}

		$urlArray = explode('/', $_SERVER['REQUEST_URI']);
		if (count($urlArray) < 4) {
			// URLが変
			return redirect('/index?err1=' . valueUrlEncode(config('message.msg_cmn_auth_003')));
		}

		if ($urlArray[3] != $kindItem['url'] || $urlArray[4] != $menuItem['url']) {
			// URLがcmn1 cmn2と矛盾
			return redirect('/index?err1=' . valueUrlEncode(config('message.msg_cmn_auth_003')));
		}

		// 操作権限が有るか？
		$isReadOnly = null;
		if (!FuncCommon::isPermissionMenu($kindItem['url'], $menuItem['url'], $isReadOnly)) {
			// 無いならトップページにリダイレクト
			return redirect('/index?err1=' . valueUrlEncode(config('message.msg_cmn_auth_002')));
		}

		if ((!$isReadOnly && $authority == config('system_const.authority_readonly')) ||
		    ($isReadOnly && $authority == config('system_const.authority_editable'))) {
			// 呼び出し元で指定された権限が無いならトップページにリダイレクト
			return redirect('/index?err1=' . valueUrlEncode(config('message.msg_cmn_auth_002')));
		}

		// ログを出力するか
		$menuURL = url('/') . '/' . $kindItem['url'] . '/' . $menuItem['url'] .
			'/index?cmn1=' . $request->cmn1 . '&cmn2=' . $request->cmn2;
		$url = 'http://' . $_SERVER['HTTP_HOST'] . $_SERVER['REQUEST_URI'];
		if ($menuURL == $url) {
			if (config('system_config.log_system_flag') == '1') {
				// システムログを出力
				DB::transaction(function () use ($loginUserInfo, $request, $kindItem, $menuItem) {
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
					$systemLog->Action3 = $menuItem['menuname']; // 動作3
					$systemLog->save(); // 保存実行
				});
			}

			//ロック削除
			$this->deleteLockAll(session()->getId());
		}

		// メニュー表示用の変数を作成
		$menuInfo = new MenuInfo();
		$menuInfo->SessionID = $request->session()->getId(); // セッションID
		$menuInfo->UserID = $loginUserInfo['userid']; // ユーザーID
		$menuInfo->GuestID = $loginUserInfo['guestid']; // ゲストユーザーの場合のWindowsログインユーザーID
		$menuInfo->UserName = $loginUserInfo['username']; // ユーザー名
		$menuInfo->Kinds = $kinds; // システム種類マスタレコードの配列
		$menuInfo->Menus = $menus; // システムメニューマスタレコードの配列
		$menuInfo->IsReadOnly = $isReadOnly; // メニューに書き込み権限が有るか
		$menuInfo->KindID = $kindItem['id']; // システム種類ID
		$menuInfo->KindURL = $kindItem['url']; // システムURL
		$menuInfo->MenuID = $menuItem['id']; // システムメニューマスタID
		$menuInfo->MenuURL = $menuItem['url']; // メニューURL
		$menuInfo->MenuNick = $menuItem['menunick']; // メニュー略称

		return $menuInfo;

	}

	/**
	 * checkLogin or existsLock の戻り値でリダイレクトが必要と指定されているかを判定して返す
	 *
	 * @param mix checkLogin or existsLock の戻り値
	 * @return boolean true = リダイレクトが必要 / false = リダイレクトの必要なし
	 *
	 * @create 2020/07/10　K.Yoshihara
	 * @update 2020/10/20　K.Yoshihara existsLock に対応
	 */
	protected function isRedirectMenuInfo($menuInfo)
	{
		// データ型をチェック
		if (!is_null($menuInfo) && get_class($menuInfo) == "Illuminate\\Http\\RedirectResponse") {
			// リダイレクトが必要
			return true;
		}
		return false;
	}

	/**
	 * 排他ロックを試行し、エラーが有ればメッセージを返す
	 *
	 * @param string SysKindID フィールド。
	 * @param string SysMenuID フィールド。複数機能をまたがるロックの場合、マイナスの数値を 定数で渡す。
	 * @param string UserID フィールド。
	 * @param string SessionID フィールド。
	 * @param string OptionKey フィールド。省略可能。省略時は config('system_const.lock_option_key_general')
	 * @param boolean HeartBeatを使用するか。省略可能。省略時は true (true = HeartBeatを使用する / false = HeartBeatを使用しない)
	 * @return boolean エラーメッセージ。エラーが無い場合はnull
	 *
	 * @create 2020/07/29　K.Yoshihara
	 * @update 2020/08/12　K.Yoshihara 排他制御を共有ロックを使った悲観ロックに変更
	 * @update 2020/09/16　K.Yoshihara レコードが無い場合は Insert するように変更
	 *                                 引数 $menuInfo を削除、$kindID $menuID $userID $sessionID $optionKey $useHeartBeat を追加
	 * @update 2020/10/12　K.Yoshihara 引数 $optionKey を配列で渡せるように変更
	 */
	protected function tryLock($kindID, $menuID, $userID, $sessionID, $optionKey = null, $useHeartBeat = true)
	{

		$sysDate = DB::selectOne('SELECT getdate() AS sysdate')->sysdate;

		if ($optionKey == null) {
			$optionKey = config('system_const.lock_option_key_general');
		}

		$optionKeys = null;
		if (is_array($optionKey)) {
			$optionKeys = $optionKey;
		}
		else{
			$optionKeys = array();
			$optionKeys[] = $optionKey;
		}

		$message = DB::transaction(function () use($kindID, $menuID, $optionKeys, $userID, $sessionID, $useHeartBeat, $sysDate) {

			foreach ($optionKeys as $optionKey) {

				$systemLock = SystemLock::
				select(
					'Updated_at'
					,'BeginTime'
					,'UserID'
					,'SessionID'
				)
				->where('SysKindID', $kindID)
				->where('SysMenuID', $menuID)
				->where('OptionKey', $optionKey)
				->sharedLock()
				->first();

				if ($systemLock){
					$isUpdate = false;
					if (!isset($systemLock->SessionID)){
						// システム導入当時はIDが入っていない
						$isUpdate = true;
					}elseif ($systemLock->SessionID == $sessionID){
						// 自分自身が排他ロックしているならロックを上書き可能
						$isUpdate = true;
					}elseif ($useHeartBeat && (strtotime($sysDate) - strtotime($systemLock->Updated_at)) >
						(int)config('system_const.lock_heart_beat_lifetime_sec')){
						// 最後のハートビートからタイムアウト期間が経過したらいつでもロック横取り可能
						$isUpdate = true;
					}elseif ((strtotime($sysDate) - strtotime($systemLock->BeginTime)) >
						(int)config('system_const.lock_open_lifetime_sec')){
						// ページを開いてから長時間放置したらいつでもロック横取り可能
						$isUpdate = true;
					}
					if (!$isUpdate) {
						// 排他ロック不可能
						if ($systemLock->UserID == $userID && $systemLock->SessionID != $sessionID){
							return config('message.msg_lock_002');
						}else{
							$mstUser = MstUser::where('UserID', $systemLock->UserID)->first();
							$userName = null;
							if (isset($mstUser)) {
								$userName = $mstUser->UserName;
							}
							return sprintf(config('message.msg_lock_001'), $systemLock->UserID, $userName);
						}
					}
					SystemLock::
					where('SysKindID', $kindID)
					->where('SysMenuID', $menuID)
					->where('OptionKey', $optionKey)
					->update([
					'UserID' => $userID,
					'SessionID' => $sessionID,
					'BeginTime' => $sysDate,
					]);
				}
				else{
					try {
						SystemLock::insert([
							'SysKindID' => $kindID,
							'SysMenuID' => $menuID,
							'OptionKey' => $optionKey,
							'UserID' => $userID,
							'SessionID' => $sessionID,
							'BeginTime' => $sysDate,
							]);
					} catch (QueryException $e) {
						if ($e->getCode() == '23000'){

							// わずかなタイミングの間に排他ロックされた

							// 排他ロックで割り込んできたロックの情報を取得
							$systemLock = SystemLock::
							select(
								'UserID'
								,'SessionID'
							)
							->where('SysKindID', $kindID)
							->where('SysMenuID', $menuID)
							->where('OptionKey', $optionKey)
							->first();

							if (!$systemLock){
								if ($systemLock->UserID == $userID && $systemLock->SessionID != $sessionID) {
									// 同一ユーザーの別セッション
									return config('message.msg_lock_004');
								}
								else{
									// 別ユーザー
									$mstUser = MstUser::where('UserID', $systemLock->UserID)->first();
									$userName = null;
									if (isset($mstUser)) {
										$userName = $mstUser->UserName;
									}
									return sprintf(config('message.msg_lock_003'), $systemLock->UserID, $userName);
								}
							}
						}
						throw $e;
					}
				}
			}

			// 排他ロック成功
			return null;

		});

		// 排他ロック結果を返す
		return $message;

	}

	/**
	 * 排他ロックを解除
	 *
	 * @param string SysKindID フィールド。
	 * @param string SysMenuID フィールド。複数機能をまたがるロックの場合、マイナスの数値を 定数で渡す。
	 * @param string SessionID フィールド。
	 * @param string OptionKey フィールド。省略可能。省略時は config('system_const.lock_option_key_general')
	 * @return 無し
	 *
	 * @create 2020/09/16　K.Yoshihara
	 * @update 2020/10/12　K.Yoshihara 引数 $optionKey を配列で渡せるように変更
	 */
	protected function deleteLock($kindID, $menuID, $sessionID, $optionKey = null)
	{
		if ($optionKey == null) {
			$optionKey = config('system_const.lock_option_key_general');
		}
		if (is_array($optionKey)) {
			SystemLock::where('SysKindID', $kindID)
			->where('SysMenuID', $menuID)
			->whereIn('OptionKey', $optionKey)
			->where('SessionID', $sessionID)
			->delete();
		}
		else {
			SystemLock::where('SysKindID', $kindID)
			->where('SysMenuID', $menuID)
			->where('OptionKey', $optionKey)
			->where('SessionID', $sessionID)
			->delete();
		}
	}

	/**
	 * 排他ロックを解除(開いているセッションの全て)
	 *
	 * @param string SessionID フィールド。
	 * @return 無し
	 *
	 * @create 2020/10/02　T.Nishida
	 * @update
	 */
	protected function deleteLockAll($sessionID)
	{
		SystemLock::where('SessionID', $sessionID)
			->delete();
	}

	/**
	 * POST 排他情報更新クション
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/07/29　K.Yoshihara
	 * @update 2020/08/12　K.Yoshihara 排他制御を共有ロックを使った悲観ロックに変更
	 * @update 2020/09/16　K.Yoshihara 引数 $optionKey を追加
	 * @update 2020/09/23　K.Yoshihara 引数 $optionKey を削除、requestの引数名を変更
	 * @update 2020/10/12　K.Yoshihara 引数 $request の optionKey をカンマ区切りで複数渡せるように変更
	 */
	protected function heartBeat(Request $request)
	{

		// セッション変数からログイン情報を取得
		$loginUserInfo = session('LOGINUSER_INFO');

		if (!isset($loginUserInfo)) {
			// キャッシュ切れ(まず無いと思うが)
			return response()->json(['status' => config('system_const.json_status_ng'),
				'message' => config('message.msg_json_001')]);
		}

		$userID = $loginUserInfo['userid'];
		$sysKindID = valueUrlDecode($request->sys_kind_id);
		$sysMenuID = valueUrlDecode($request->sys_menu_id);
		$optionKey = valueUrlDecode($request->option_key);
		if ($optionKey == null) {
			$optionKey = config('system_const.lock_option_key_general');
		}

		$optionKeys = explode(',', $optionKey);

		$json = DB::transaction(function () use($request, $userID, $sysKindID, $sysMenuID, $optionKeys) {

			foreach ($optionKeys as $optionKey) {

				$systemLock = SystemLock::
				select(
					'UserID'
					,'SessionID'
				)
				->where('SysKindID', $sysKindID)
				->where('SysMenuID', $sysMenuID)
				->where('OptionKey', $optionKey)
				->sharedLock()
				->first();

				$isUpdate = false;

				$sessionID = $request->session()->getId();

				if (!isset($systemLock->SessionID)){
					// システム導入当時はIDが入っていない
					$isUpdate = true;
				}elseif ($systemLock->SessionID == $sessionID){
					// 自分自身が排他ロックしているならロックを上書き可能
					$isUpdate = true;
				}

				if (!$isUpdate) {
					// いつの間にか排他ロックされていた
					if ($systemLock->UserID == $userID && $systemLock->SessionID != $sessionID) {
						$msg = config('message.msg_lock_004');
					}
					else{
						$mstUser = MstUser::where('UserID', $systemLock->UserID)->first();
						$userName = null;
						if (isset($mstUser)) {
							$userName = $mstUser->UserName;
						}
						$msg = sprintf(config('message.msg_lock_003'), $systemLock->UserID, $userName);
					}
					return response()->json(['status' => config('system_const.json_status_ng'), 'message' => $msg]);
				}

				SystemLock::
				where('SysKindID', $sysKindID)
				->where('SysMenuID', $sysMenuID)
				->where('OptionKey', $optionKey)
				->update([
					'UserID' => $userID,
					'SessionID' => $sessionID,
					]);

			}

			// 排他ロック成功
			return response()->json(['status' => config('system_const.json_status_ok'), 'message' => null]);

		});

		return $json;

	}

	/**
	 * 排他ロックされているかをチェックし、されていない場合はリダイレクト先を返す
	 *
	 * @param string SysKindID フィールド。
	 * @param string SysMenuID フィールド。複数機能をまたがるロックの場合、マイナスの数値を 定数で渡す。
	 * @param string SessionID フィールド。
	 * @param string OptionKey フィールド。省略可能。省略時は config('system_const.lock_option_key_general')
	 * @return boolean リダイレクト先。排他ロックされていない場合はnull
	 *
	 * @create 2020/10/20　K.Yoshihara
	 * @update
	 */
	protected function existsLock($kindID, $menuID, $sessionID, $optionKey = null)
	{

		if ($optionKey == null) {
			$optionKey = config('system_const.lock_option_key_general');
		}

		$optionKeys = null;
		if (is_array($optionKey)) {
			$optionKeys = $optionKey;
		}
		else{
			$optionKeys = array();
			$optionKeys[] = $optionKey;
		}

		foreach ($optionKeys as $optionKey) {

			$exists = SystemLock::
			where('SysKindID', $kindID)
			->where('SysMenuID', $menuID)
			->where('OptionKey', $optionKey)
			->where('SessionID', $sessionID)
			->exists();

			if (!$exists){
				// 排他ロック無し
				return redirect('/index?err1=' . valueUrlEncode(config('message.msg_cmn_auth_003')));
			}
		}

		// 排他ロック有り
		return null;

	}

	/**
	 * function sort and pagination member
	 *
	 * @param query
	 * @param string sort
	 * @param string direction
	 * @param int pageUnit
	 * @param request
	 * @return mixed
	 *
	 * @create 2020/12/28　Cuong
	 * @update 
	 */
	protected function sortAndPagination($query, $sort, $direction, $pageUnit, $request) {
		if(is_array($sort)) {
			if(count($sort) > 0) {
				if($direction == 'desc') {
					$query = $query->sortByDesc(function($obj) use ($sort) {
						$temp = [];
						foreach($sort as $field => $dir) {
							if(is_string($field)) {
								$temp[] = $obj->$field;
							} else {
								$temp[] = $obj->$dir;
							}
						}
						return $temp;
					});
				} else {
					$query = $query->sortBy(function($obj) use ($sort) {
						$temp = [];
						foreach($sort as $field => $dir) {
							if(is_string($field)) {
								$temp[] = $obj->$field;
							} else {
								$temp[] = $obj->$dir;
							}
						}
						return $temp;
					});
				}
			}
			$rows = $query->toArray();
		} else {
			if ($direction == 'desc') {
				$rows = $query->sortByDesc($sort)->toArray();
			} else {
				$rows = $query->sortBy($sort)->toArray();
			}
		}

		// Handling pagination
		$currentPage = LengthAwarePaginator::resolveCurrentPage();
		$currentItems = array_slice($rows, $pageUnit * ($currentPage - 1), $pageUnit);
		// new LengthAwarePaginator
		return new LengthAwarePaginator($currentItems, count($rows), $pageUnit, $currentPage,[
			'path'  => $request->url(),
			'query' => $request->query(),
		]);
	}
}
