<?php
/*
 * @topController.php
 * 搭載日程トップ画面コントローラーファイル
 *
 * @create 2020/08/27 T.Nishida
 *
 * @update
 */

namespace App\Http\Controllers\Schet;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Librarys\FuncCommon;
use App\Librarys\MenuInfo;
use App\Models\T_ImportLog;

/*
 * 搭載日程トップ画面コントローラー
 *
 * @create 2020/08/27 T.Nishida
 *
 * @update
 */
class topController extends Controller
{
	/**
	 * 搭載日程トップ画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/08/27 T.Nishida
	 * @update
	 */
	public function index(Request $request)
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

		// メニュー表示用の変数を作成
		$menuInfo = new MenuInfo();
		$menuInfo->UserName = $loginUserInfo['username']; // ユーザー名
		$menuInfo->Kinds = $loginUserInfo['syskind']; // システム種類マスタレコードの配列
		$menuInfo->Menus = $loginUserInfo['sysmenu']; // システムメニューマスタレコードの配列
		$menuInfo->KindID = $kindID; // システム種類ID
		$menuInfo->KindURL = $kindItem['url']; //システムURL

		//表示データを取得
		$listDatas = $this->getT_ImportLog($request);

		$this->data['menuInfo'] = $menuInfo;
		$this->data['request'] = $request;
		$this->data['listDatas'] = $listDatas;
		return view('Schet/index', $this->data);
	}

	/**
	 * 搭載日程取得ログの取得
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return array 搭載日程取得ログ
	 *
	 * @create 2020/08/27 T.Nishida
	 * @update
	 */
	private function getT_ImportLog(Request $request)
	{
		$getLists = T_ImportLog::select(
			'Category as fld1'
			, 'BlockName as fld2'
			, 'BlockKumiku as fld3'
			, 'Log as fld4'
		);
		$getLists = $getLists->where('HistoryID', '=', valueUrlDecode($request->val1));
		if (!isset($request->sort)) {
			//初回表示
			$getLists = $getLists->orderby('Category', 'asc');
			$getLists = $getLists->orderby('BlockKumiku', 'asc');
			$getLists = $getLists->orderby('BlockName', 'asc');
			$getLists = $getLists->sortable();
		}else{
			$getLists = $getLists->sortable(['fld1', 'fld2', 'fld3', 'fld4']);
		}
		if (!isset($request->pageunit)) {
			//初回表示
			$getLists = $getLists->paginate(config('system_const.displayed_results_1'));
		}else{
			$getLists = $getLists->paginate($request->pageunit);
		}
		return $getLists;
	}

}

