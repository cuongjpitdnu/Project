<?php
/*
 * @VersionController.php
 * システム情報コントローラーファイル
 *
 * @create 2020/11/13 KBS T.Nishida
 *
 * @update 
 */

namespace App\Http\Controllers\System;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;

/*
 * システム情報コントローラー
 *
 * @create 2020/11/13 KBS T.Nishida
 *
 * @update
 */
class VersionController extends Controller
{

	/**
	 * GET システム情報トップ画面アクション
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/11/13 KBS T.Nishida
	 * @update
	 */
	public function index(Request $request)
	{
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));
		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		//ビューを表示
		return view('system/version/index')->with([
			'request' => $request,
			'menuInfo' => $menuInfo,
		]);
	}
}