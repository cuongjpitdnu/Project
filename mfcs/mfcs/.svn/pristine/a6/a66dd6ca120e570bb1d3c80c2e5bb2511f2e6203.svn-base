<?php
/*
 * @MissingUpdateException.php
 *
 *
 * @create 2020/09/11 Chien
 *
 * @update
 */

namespace App\Librarys;

use Exception;

/*
 * exception when update
 *
 * @create 2020/09/11 Chien
 * @update
 */
class MissingUpdateException extends Exception {

    public $returnMessage;
	/**
	 * コンストラクタ
	 *
	 * @param string ユーザーID
	 * @param string メッセージ
	 * @param int コード
	 * @param Exception 内包例外
	 * @return void
	 *
	 * @create 2020/09/11
	 * @update
	 */
	public function __construct($message = null, $code = 0, Exception $previous = null) {
		$this->returnMessage = $message;
		parent::__construct($message, $code, $previous);
	}

}
