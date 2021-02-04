<?php
/*
 * @UnregisteredUserException.php
 * ユーザーがユーザーマスタに見つからない例外ファイル
 *
 * @create 2020/07/09 KBS K.Yoshihara
 *
 * @update
 */

namespace App\Librarys;

use Exception;

/*
 * ユーザーがユーザーマスタに見つからない例外
 *
 * @create 2020/07/09 KBS K.Yoshihara
 * @update
 */
class UnregisteredUserException extends Exception
{

	public $UserID;

	/**
	 * コンストラクタ
	 *
	 * @param string ユーザーID
	 * @param string メッセージ
	 * @param int コード
	 * @param Exception 内包例外
	 * @return void
	 *
	 * @create 2020/07/09　K.Yoshihara
	 * @update
	 */
	public function __construct($userID, $message = null, $code = 0, Exception $previous = null) {
		$this->UserID = $userID;
		parent::__construct($message, $code, $previous);
	}

}
