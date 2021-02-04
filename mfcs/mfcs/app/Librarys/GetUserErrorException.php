<?php
/*
 * @GetUserErrorException.php
 * ログインユーザーの取得失敗例外ファイル
 *
 * @create 2020/07/09 KBS K.Yoshihara
 *
 * @update
 */

namespace App\Librarys;

use Exception;

/*
 * ログインユーザーの取得失敗例外
 *
 * @create 2020/07/09 KBS K.Yoshihara
 * @update
 */
class GetUserErrorException extends Exception
{
	/**
	 * コンストラクタ
	 *
	 * @param string メッセージ
	 * @param int コード
	 * @param Exception 内包例外
	 * @return void
	 *
	 * @create 2020/07/09　K.Yoshihara
	 * @update
	 */
	public function __construct($message = null, $code = 0, Exception $previous = null) {
		parent::__construct($message, $code, $previous);
	}

}
