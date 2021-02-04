<?php
/*
 * @FuncCommon.php
 * 共通処理用ファイル
 *
 * @create 2020/06/01 KBS T.Nishida
 *
 * @update
 */

namespace App\Librarys;

use App\Librarys\GetUserErrorException;
use App\Librarys\UnregisteredUserException;
use App\Models\MstUser;
use App\Models\MstSysKind;
use App\Models\MstSysMenu;
use Illuminate\Database\Eloquent\ModelNotFoundException;

/**
 * 共通処理クラス
 *
 * @create 2020/06/01　T.Nishida
 * @update
 */
class FuncCommon
{

	/**
	 * ログインユーザIDを取得
	 *
	 * @param
	 * @return string ユーザID
	 *
	 * @create 2020/06/01　T.Nishida
	 * @update 2020/07/08　K.Yoshihara staticに変更、ダミーの中身を本物に変更、戻り値を void に変更
	 */
	static function getUserID()
	{
		//$userID = $_SERVER['LOGON_USER'];
		//$userID = $_SERVER['AUTH_USER'];
		//$userID = $_SERVER['REMOTE_USER'];
		$userID = 'MHI\\' . gethostbyaddr($_SERVER['REMOTE_ADDR']);

		//ドメイン名の除去
		return ltrim(strstr($userID, '\\'), '\\');
	}

	/**
	 * ログイン（ユーザ情報をセッションへ）
	 *
	 * @param
	 * @return void
	 *
	 * @create 2020/06/01　T.Nishida
	 * @update 2020/07/08　K.Yoshihara staticに変更、ダミーの中身を本物に変更、戻り値を void に変更
	 * @update 2020/07/09　K.Yoshihara リファクタリング、及びログインユーザの取得に失敗とユーザマスタにない場合の独自例外スローを実装
	 * @update 2020/07/15  T.Nishida MstUserテーブルにユーザIDがない場合、ゲストユーザで実行できるように追加
	 * @update 2020/07/28  T.Nishida 既にログイン済みの場合は何もせず抜けるように変更
	 * @update 2020/07/31  K.Yoshihara 既にログイン済みかどうかのチェックで同じユーザーIDか見るように変更
	 */
	static function runLogin()
	{
		// ログインユーザIDを取得
		$userID = self::getUserID();

		// 取得成功か？
		if (empty($userID) && $userID != '0') {
			// ログインユーザの取得に失敗した場合は例外を投げる
			throw new GetUserErrorException();
		}

		$loginUserInfo = session('LOGINUSER_INFO');

		//既にログイン済みなら抜ける
		if (isset($loginUserInfo) && $loginUserInfo['userid'] == $userID) {
			return;
		}

		// セッションスタート
		session()->regenerate();

		$mstUser = null;
		$guestID = null;
		try {
			// ユーザマスタから対象のレコードを取得
			$mstUser = MstUser::where('UserID', $userID)->firstOrFail();;
		} catch (ModelNotFoundException $e) {
			try {
				//見つからない場合はゲストユーザを探す
				$mstUser = MstUser::where('UserID', config('system_const.guest_user'))->firstOrFail();;
				$guestID = $userID;
			} catch (ModelNotFoundException $e) {
				// 見つからない場合は例外を投げなおす
				throw new UnregisteredUserException($userID);
			}
		}

		// 変数初期化
		$sysKinds = array(); // システム種類マスタ
		$sysMenus = array(); // システムメニューマスタ

		if ($mstUser->SysKindID != null) {

			// カンマ区切りのシステム種類IDを配列に直す
			$sysKindIDs = explode(',', $mstUser->SysKindID);

			// システム種類マスタの読込
			$sysKinds = self::getSys($sysKindIDs);

			// システムメニューマスタの読込
			$sysMenus = self::getMenu($sysKindIDs, $mstUser);
		}

		// セッション変数に入れる配列を作成
		$loginUserInfo = array(
			'userid' => $mstUser->UserID, // ユーザーID
			'username' => $mstUser->UserName, // ユーザー名
			'syskind' => $sysKinds, // システム種類マスタ
			'sysmenu' => $sysMenus, // システムメニューマスタ
			'guestid' => $guestID, // ゲストユーザーの場合のWindowsログインユーザーID
		);

		// セッション変数に入れる
		session(['LOGINUSER_INFO' => $loginUserInfo]);
	}

	/**
	 * システム種類マスタの読込
	 *
	 * @param string[] システム種類IDの配列
	 * @return string[] システム種類マスタのレコード一覧
	 *
	 * @create 2020/07/09　K.Yoshihara
	 * @update
	 */
	private static function getSys($sysKindIDs)
	{

		// データベースからレコードを検索
		$mstSysKinds = MstSysKind::whereIn('id', $sysKindIDs)->orderBy('SortNo', 'asc')->get();

		$sysKinds = array();

		// 配列に入れなおす
		foreach ($mstSysKinds as $mstSysKind) {
			// 1行分を作成
			$sysKind = array(
				'id' => $mstSysKind->ID, // システム種類ID
				'sysname' => $mstSysKind->SysName, // システム名
				'sysnick' => $mstSysKind->SysNick, // 略称
				'url' => $mstSysKind->URL, // URL
			);
			// 配列に追加
			$sysKinds[] = $sysKind;
		}

		return $sysKinds;
	}

	/**
	 * システムメニューマスタの読込
	 *
	 * @param string[] システム種類IDの配列
	 * @param MstUser ユーザーマスタのレコード
	 * @return string[] システムメニューマスタのレコード一覧
	 *
	 * @create 2020/07/09　K.Yoshihara
	 * @update
	 */
	private static function getMenu($sysKindIDs, $mstUser)
	{

		// データベースからレコードを検索
		$mstSysMenus = MstSysMenu::whereIn('SysKindID', $sysKindIDs)->orderBy('SortNo', 'asc')->get();

		// 書き込み権限が有る画面IDを配列に直す
		$updateSysMenuIDs = array();
		if ($mstUser->SysMenuID_Update != null) {
			$updateSysMenuIDs = explode(',', $mstUser->SysMenuID_Update);
		}

		// 読み取り権限が有る画面IDを配列に直す
		$readSysMenuIDs = array();
		if ($mstUser->SysMenuID_Read != null) {
			$readSysMenuIDs = explode(',', $mstUser->SysMenuID_Read);
		}

		$sysMenus = array();
		foreach ($mstSysMenus as $mstSysMenu) {

			// 権限チェック
			$readonly = null;
			if (in_array($mstSysMenu->ID, $updateSysMenuIDs)) {
				$readonly = false;
			} elseif (in_array($mstSysMenu->ID, $readSysMenuIDs)) {
				$readonly = true;
			} else {
				// 読み取り権限も書き込み権限もないなら表示対象ではないので配列に追加しない
				continue;
			}

			// 1行分を作成
			$sysMenu = array(
				'id' => $mstSysMenu->ID, // メニューID
				'syskindid' => $mstSysMenu->SysKindID, // システム種類ID
				'menuname' => $mstSysMenu->MenuName, // メニュー名
				'menunick' => $mstSysMenu->MenuNick, // 略称
				'url' => $mstSysMenu->URL, // URL
				'readonly' => $readonly, // true:閲覧のみ、false:更新可
			);
			// 配列に追加
			$sysMenus[] = $sysMenu;
		}

		return $sysMenus;
	}

	/**
	 * 許可されたメニューか判定
	 *
	 * @param  string 第一階層URL
	 * @param  string 第二階層URL
	 * @param  boolean true:閲覧権限、false:更新権限
	 * @return boolean true:許可されている、false:許可されていない
	 *
	 * @create 2020/06/01　T.Nishida
	 * @update 2020/07/08　K.Yoshihara staticに変更
	 * @update 2020/07/09　K.Yoshihara 第二階層URLが省略された場合は第一階層URLのみで判断するように変更
	 */
	static function isPermissionMenu($url1, $url2, &$readOnlyFlg)
	{
		//セッションから取り出し
		$tmpArr = session('LOGINUSER_INFO');

		//セッションから取り出し
		$sysMenuArr = $tmpArr['syskind'];
		$sysSubArr = $tmpArr['sysmenu'];

		//メインメニューを確認
		$menuFlg = false;
		foreach ($sysMenuArr as $tmpData) {
			if (strtolower($url1) == strtolower($tmpData['url'])) {
				$menuFlg = true;
				break;
			}
		}

		//メインメニューの権限なし
		if ($menuFlg == false) {
			return false;
		}

		if ($url2 == null) {
			//メインメニューのみで判断する場合はここで終わり
			return true;
		}

		//サブメニューを確認
		foreach ($sysSubArr as $tmpData) {
			if (strtolower($url2) == strtolower($tmpData['url'])) {
				$readOnlyFlg = $tmpData['readonly'];
				return true;
			}
		}

		//サブメニューの権限なし
		return false;
	}

	/**
	 * 組区から名称等を返す
	 *
	 * @param  string 組区
	 * @return array [0]=組区、[1]=名称、[2]=組区と名称が区切り文字で仕切られた文字（組区：名称）
	 *
	 * @create 2020/10/09　T.Nishida
	 * @update
	 */
	static function getKumikuData($kumiku)
	{
		$return = array();

		switch ($kumiku) {
			case config('system_const.kumiku_code_kogumi'):
				$return[0] = config('system_const.kumiku_code_kogumi');
				$return[1] = config('system_const.kumiku_name_kogumi');
				$return[2] = config('system_const.kumiku_code_kogumi') .
					config('system_const.code_name_separator') .
					config('system_const.kumiku_name_kogumi');
				break;

			case config('system_const.kumiku_code_naicyu'):
				$return[0] = config('system_const.kumiku_code_naicyu');
				$return[1] = config('system_const.kumiku_name_naicyu');
				$return[2] = config('system_const.kumiku_code_naicyu') .
					config('system_const.code_name_separator') .
					config('system_const.kumiku_name_naicyu');
				break;

			case config('system_const.kumiku_code_kumicyu'):
				$return[0] = config('system_const.kumiku_code_kumicyu');
				$return[1] = config('system_const.kumiku_name_kumicyu');
				$return[2] = config('system_const.kumiku_code_kumicyu') .
					config('system_const.code_name_separator') .
					config('system_const.kumiku_name_kumicyu');
				break;

			case config('system_const.kumiku_code_ogumi'):
				$return[0] = config('system_const.kumiku_code_ogumi');
				$return[1] = config('system_const.kumiku_name_ogumi');
				$return[2] = config('system_const.kumiku_code_ogumi') .
					config('system_const.code_name_separator') .
					config('system_const.kumiku_name_ogumi');
				break;

			case config('system_const.kumiku_code_sogumi'):
				$return[0] = config('system_const.kumiku_code_sogumi');
				$return[1] = config('system_const.kumiku_name_sogumi');
				$return[2] = config('system_const.kumiku_code_sogumi') .
					config('system_const.code_name_separator') .
					config('system_const.kumiku_name_sogumi');
				break;

			case config('system_const.kumiku_code_kyocyu'):
				$return[0] = config('system_const.kumiku_code_kyocyu');
				$return[1] = config('system_const.kumiku_name_kyocyu');
				$return[2] = config('system_const.kumiku_code_kyocyu') .
					config('system_const.code_name_separator') .
					config('system_const.kumiku_name_kyocyu');
				break;

			default:
				break;
		}

		return $return;
	}

	/**
	 * コードと名称を区切った文字を分けて返す
	 *
	 * @param  string 区切られた文字
	 * @return array [0]=コード、[1]=名称
	 *
	 * @create 2020/10/09　T.Nishida
	 * @update
	 */
	static function getSplitChar($char)
	{
		$return = array();

		$tmp = explode(config('system_const.code_name_separator'), $char);
		if (count($tmp) == 2) {
			$return = $tmp;
		}

		return $return;
	}

	/**
	 * 引数で指定された小数・整数を、表示用の文字列に変換して返す。
	 *
	 * @param number 小数・整数
	 * @param integer 区切られた文字
	 * @return string ・nullの場合はnullを返す。
	 *			      ・整数部が0の場合でも必ず0を省略せずに返す。
	 *			      ・引数で指定された小数部桁数分は必ず表示する。
	 *			      ・小数部は、指定された桁数を超えている場合、指定された桁数+1で四捨五入する。
	 *			      ・整数部は3桁毎にカンマを入れる。
	 * @create 2020/12/04 Chien
	 * @update 2020/12/09 Chien
	 */
	static function formatDecToChar($number, $type)
	{
		if ($number === '' || $number === null) {
			return null;
		}

		switch ($type) {
			case 0:
				$number = number_format(round($number));
				break;
			default:
				$number = number_format(round($number, $type), $type, ".", ",");
				break;
		}

		return $number;
	}

	/**
	 * 引数で指定された小数・整数を、テキストボックス用の文字列に変換して返す。
	 *
	 * @param number 小数・整数
	 * @return string ・nullの場合はnullを返す。
	 *				  ・整数部が0の場合でも必ず0を省略せずに返す。
	 *				  ・カンマは入れない。
	 * @create 2020/12/07 Chien
	 * @update 2020/12/09 Chien
	 * @update 2020/12/14 Chien
	 */
	static function formatDecToText($number)
	{
		if ($number === '' || $number === null) {
			return null;
		}

		$res = $number;
		if (strpos($number, '.') > -1) {
			$strSplit = explode('.', $number);
			if (count($strSplit) >= 2) {
				if ($strSplit[0] == '') {
					if ((int)$strSplit[1] == 0) {
						$res = 0;
					} else {
						$lastChar = mb_substr($strSplit[1], -1);
						if ($lastChar == 0) {
							$res = '0'.mb_substr($number, 0, (mb_strlen($number) - 1));
						} else {
							$res = '0.'.$strSplit[1];
						}
					}
				} else {
					if ((int)$strSplit[1] == 0) {
						$res = (int)$strSplit[0];
					} else {
						$lastChar = mb_substr($strSplit[1], -1);
						if ($lastChar == 0) {
							$res = mb_substr($number, 0, (mb_strlen($number) - 1));
						}
					}
				}
			}
		}
		return $res;
	}

	/**
	 * 日付フォーマットチェック
	 *
	 * @param date 日付
	 * @return array 結果(boolean)、エラーメッセージ(string)
	 * @create 2020/12/22 KBS S.Tanaka
	 * 
	 * @update 
	 */
	static function checkFormatDate($value){
		$formats = array("Y/m/d","Y/n/j","Y/n/d","Y/m/j");
		$arrSplit = explode( '/', $value);
		if(count($arrSplit) == 3) {
			if (mb_strlen($arrSplit[0]) == 4) {
				foreach($formats as $format) {
					$parsed = date_parse_from_format($format, $value);
					if ($parsed['error_count'] === 0 && $parsed['warning_count'] === 0) {
						return [true, null];
					}
				}
			}else{
				//年月日に半角数字以外の値が入っているかチェック
				$result = \preg_match('/\D/', $arrSplit[0].$arrSplit[1].$arrSplit[2]);
				if(!$result && $arrSplit[1] <= 12 && $arrSplit[2] <= 31) {
					return [false, config("message.msg_validation_015")];
				}
			}
		}
		return [false, config("message.msg_validation_014")];
	}

	/**
	 * 配列の値をカンマ区切りの文字列に変換する
	 *
	 * @param array 配列
	 * @param integer カンマで区切る値の一つ当たりの最大数
	 * @return array 配列の値をカンマ区切りにした文字列を入れた配列
	 * @create 2021/01/27 KBS S.Tanaka
	 * 
	 * @update 
	 */
	static function delimitCommaData($array, $maxNum){
		//配列の初期化
		$arr = [];
		if(is_int($maxNum) && $maxNum > 0){
			$arrays = array_chunk($array, $maxNum);
			foreach($arrays as $array){
				$arr[] = implode(',', $array);
			}
		}
		return $arr;
	}
}
