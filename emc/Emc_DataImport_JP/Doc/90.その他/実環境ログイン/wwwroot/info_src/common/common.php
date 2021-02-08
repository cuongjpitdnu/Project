<?php
	// -------------------------------------------------------------------------
	//	function	: データベース処理共通関数
	//	create		: 2020/01/17 KBS S.Tasaki
	//	update		:
	// -------------------------------------------------------------------------

	require_once('config.php');
	require_once('display_text_config.php');
	require_once('msg_config.php');
	require_once('sqlsrv_common.php');
	require_once('session_check.php');

	//Amazon Service
	require 'C:/inetpub/wwwroot/information_sharing/vendor/autoload.php';
	use Aws\Translate\TranslateClient;
	use Aws\Exception\AwsException;

	//DB接続クラスを設定.
	$g_dbaccess = null;		//DB接続クラスを格納
	$g_dbaccess = new sqlsrv_dbaccess();

	date_default_timezone_set('Asia/Tokyo');

	// ************************************************************************************************************************************************************
	//	CONST
	// ************************************************************************************************************************************************************
	//ログレベル
	const LogLevel = array(
		'Error' => 0,
		'Info' => 1
	);
	//ログパターン
	const LogPattern = array(
		'Error' => 0,
		'Login'=> 1,
		'View' => 2,
		'Button' => 3,
		'Sql' => 4
	);
	//Thang
	ini_set("error_reporting", E_ALL & ~E_NOTICE & ~E_DEPRECATED);
	session_start();
	if(count($_POST)) {
		if(!isset($_POST['X-CSRF-TOKEN']) || !isset($_SESSION['csrf'])) {
			echo "<script>alert('".PUBLIC_MSG_008_JPN."/".PUBLIC_MSG_008_ENG."');window.location.href='login.php';</script>";
			exit();
		} else if($_POST['X-CSRF-TOKEN'] != $_SESSION['csrf']) {
			echo "<script>window.location.href='login.php';</script>";
			exit();
		}
		// unset($_POST['X-CSRF-TOKEN']);
	} else {
		if(isset($_SESSION['csrf']) && $_SESSION['csrf'] != '') {
			$strCsrf = $_SESSION['csrf'];
		} else {
			$_SESSION['csrf'] =fncGetCsrfToken();
			$strCsrf = $_SESSION['csrf'];
		}

	}

	//end Thang

	// ************************************************************************************************************************************************************
	//	CLASS
	// ************************************************************************************************************************************************************

	/**
	*	ログインユーザー情報格納クラス
	*
	*	@create	2020/01/17 KBS S.Tasaki
	*	@update
	*/
	class clsLoginUserInfo {
		public $intUserNo;					//ユーザNo
		public $strUserID;					//ユーザID
		public $intLanguageType;			//言語タイプ
		public $intCompanyNo;				//会社No
		public $strCompanyName;				//会社名
		public $intGroupNo;					//グループNo
		public $strUserName;				//ユーザ名
		public $intJcmgTabPerm;				//JCMGタブ権限
		public $intAnnounceRegPerm;			//お知らせ登録権限
		public $intBulletinBoardRegPerm;	//掲示板登録権限
		public $intQueryRegPerm;			//問い合わせ登録権限
		public $intIncidentCaseRegPerm;		//インシデント事案登録権限
		public $intRequestRegPerm;			//依頼事項登録権限
		public $intInformationRegPerm;		//情報登録権限
		public $intMenuPerm;				//各種メニュー権限
		public $strMailAddress;
		public $intAnnounceMail;
		public $intBulletinBoardMail;
		public $intIncidentCaseMail;
		public $intRequestContentsMail;
	}

	// ************************************************************************************************************************************************************
	//	DB CONNECT
	// ************************************************************************************************************************************************************

	/**
	*	function	: データベース接続
	*
	*	@create	2020/01/17 KBS S.Tasaki
	*	@update
	*	@params
	*	@return	boolean true:接続成功、false接続失敗
	*/
	function fncConnectDB(){
		return $GLOBALS['g_dbaccess']->fncDbConnect();
	}

	// ************************************************************************************************************************************************************
	//	DB SELECT
	// ************************************************************************************************************************************************************

	/**
	*	function	: ログインユーザ情報取得
	*
	*	@create	2020/01/17 KBS S.Tasaki
	*	@update	2020/03/26 KBS T.Masuda
	*	@params	$strUserID		ユーザID
	*			$strPassword	パスワード
	*	@return	array			ログインユーザ情報
	*/
	//-----------------------------------------
	//  2020/03/26 KBS T.Masuda　
	//  JCMGタブ権限を追加
	//　掲示板登録権限を削除
	//-----------------------------------------

	function fncLogin($strUserID, $strPassword, $strDisplay = 'ログイン'){

		try{
			$strSQL = '';	//SQL文
			$strSQL .= ' SELECT ';
			$strSQL .= '        m_user.user_no ';
			$strSQL .= '      , m_user.user_id ';
            $strSQL .= '      , m_user.password  ';
			$strSQL .= '      , m_user.password_up_date ';
			$strSQL .= '      , m_user.expiration_date_s ';
			$strSQL .= '      , m_user.expiration_date_e ';
			$strSQL .= '      , m_user.language_type ';
			$strSQL .= '      , m_user.company_no ';
			$strSQL .= '      , m_company.company_name_jpn ';
			$strSQL .= '      , m_company.company_name_eng ';
			$strSQL .= '      , m_company.group_no ';
			$strSQL .= '      , m_user.user_name ';
			$strSQL .= '      , m_user.perm_flag ';
			$strSQL .= '      , m_user.jcmg_tab_perm ';
			$strSQL .= '      , m_user.announce_reg_perm ';
			//$strSQL .= '      , m_user.bulletin_board_reg_perm ';
			$strSQL .= '      , m_user.query_reg_perm ';
			$strSQL .= '      , m_user.incident_case_reg_perm ';
			$strSQL .= '      , m_user.request_reg_perm ';
			$strSQL .= '      , m_user.information_reg_perm ';
			$strSQL .= '      , m_user.menu_perm ';

			$strSQL .= '      , m_user.mail_address ';
			$strSQL .= '      , m_user.announce_mail ';
			$strSQL .= '      , m_user.bulletin_board_mail ';
			$strSQL .= '      , m_user.incident_case_mail ';
			$strSQL .= '      , m_user.request_contents_mail ';

			$strSQL .= '   FROM m_user ';
			$strSQL .= '  INNER JOIN m_company ';
			$strSQL .= '          ON m_user.company_no = m_company.company_no ';
			$strSQL .= '  WHERE m_user.user_id = :user_id ';
			$strSQL .= '    AND m_user.password = :password ';
			$strSQL .= '    AND m_company.del_flag = 0 ';
			//$strSQL .= '    AND m_user.delete_flag = 0 ';

			$objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);	//DBステートメント格納
			$objStmt->bindParam(':user_id', $strUserID);
			$objStmt->bindParam(':password', $strPassword);

			$strLogSql = $strSQL;	//ログ出力内容
			$strLogSql = str_replace(':user_id', $strUserID, $strLogSql);
			$strLogSql = str_replace(':password', $strPassword, $strLogSql);
			fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strDisplay.$strLogSql);

			$objStmt->execute();
			$arrResult = $objStmt->fetchAll(PDO::FETCH_ASSOC);	//取得結果
			return $arrResult;
		}catch (\Exception $e){
			fncWriteLog(LogLevel['Error'] , LogPattern['Error'], $strDisplay.$e->getMessage());
			return null;
		}
	}

	// ************************************************************************************************************************************************************
	//	FUNCTION
	// ************************************************************************************************************************************************************

	/**
	*	function	: エスケープ処理
	*
	*	@create	2020/01/22 KBS S.Tasaki
	*	@update
	*	@params	$item	エスケープ処理対象文字列
	*	@return	string	エスケープした文字列
	*/
	function fncHtmlSpecialChars($strItem){
		return htmlspecialchars($strItem, ENT_QUOTES, 'UTF-8');
	}


	/**
	*	function	: トークン作成処理
	*
	*	@create	2020/01/22 KBS S.Tasaki
	*	@update
	*	@params
	*	@return	string 作成したトークン
	*/
	function fncGetCsrfToken() {
		$TOKEN_LENGTH = 16;
		$bytes = openssl_random_pseudo_bytes($TOKEN_LENGTH);
		return bin2hex($bytes);
	}

	// ************************************************************************************************************************************************************
	//	ログ出力
	// ************************************************************************************************************************************************************

	/**
	*	function	: ログの出力を行う
	*
	*	@create	2020/01/17 KBS S.Tasaki
	*	@update
	*	@params	$intLogLevel	ログレベル
	*			$intLogPattern	ログパターン
	*			$strMsg			メッセージ
	*	@return
	*/
	function fncWriteLog($intLogLevel, $intLogPattern, $strMsg){
		//DBにログ内容を出力する
		fncWriteLogToDb($intLogLevel, $intLogPattern, $strMsg);
	}

	/**
	*	function	: Write Log to DB
	*
	*	@create	2020/01/17 KBS Tamnv
	*	@update
	*	@params	$intLogLevel	ログレベル
	*			$intLogPattern	ログパターン
	*			$strMsg			メッセージ
	*	@return
	*/
	function fncWriteLogToDb($intLogLevel, $intLogPattern, $strMsg){
		//ログパターン確認
		if($intLogPattern == LogPattern['Login']){
			if(LOG_LOGIN_FLAG != 1){
				return;
			}
		}else if($intLogPattern == LogPattern['View']){
			if(LOG_DISPLAY_VIEW_FLAG != 1){
				return;
			}
		}else if($intLogPattern == LogPattern['Button']){
			if(LOG_BUTTON_FLAG != 1){
				return;
			}
		}else if($intLogPattern == LogPattern['Sql']){
			if(LOG_SQL_FLAG != 1){
				return;
			}
		}

		//ログレベルを確認
		$strLogLevel = 'INFO';	//ログレベル文字列
		if($intLogLevel == 0){
			$strLogLevel = 'ERROR';
		}
		$strLogMsg = date('Y/m/d H:i:s') .' ' .$strLogLevel .' ' .$strMsg ."\r\n";	//ログメッセージ
		$regDate = date('Y-m-d');	//現在日付

		$strSQL = '';	//SQL文
		$strSQL .= ' INSERT INTO t_log ';
		$strSQL .= ' VALUES(';
		$strSQL .= ' :reg_date';
		$strSQL .= ' ,(SELECT ISNULL(MAX( LOGID ), 0)+ 1  FROM t_log WHERE REG_DATE = :reg_d) ';
		$strSQL .= ' ,:content)';
		$objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);	//DBステートメント
		$objStmt->bindParam(':reg_date', $regDate);
		$objStmt->bindParam(':reg_d', $regDate);
		//get language type and set max length
		if(@unserialize($_SESSION["LOGINUSER_INFO"])->intLanguageType){
			$intMaxLen = 8000;	//最大サイズ
		}else{
			$intMaxLen = 4000;
		}
		if($strSQL>$intMaxLen){
			$output = str_split($strSQL, $intMaxLen);
			foreach ($output as $key => $str){
				$strContent = 'PART_'.$key.' - '.$str;	//出力内容
				$objStmt->bindParam(':content',$strContent);
				$objStmt->execute();
			}
		}else{
			$objStmt->bindParam(':content', $strLogMsg);
			$objStmt->execute();
		}
	}

	// ************************************************************************************************************************************************************
	//	Amazonからの翻訳
	// ************************************************************************************************************************************************************
	// -------------------------------------------------------------------------
	//	function	: Amazonからの翻訳
	//	value		:
	//	params		: 文字列翻訳
	//				: 翻訳タイプ
	//				:
	//	memo		:
	//	create		: 2020/02/21 KBS Tamnv
	//	update		:
	// -------------------------------------------------------------------------

	function tranAmazon($textTran, $type = 0){ // Type = 0 transalte from JPN -> ENG

		$client = new Aws\Translate\TranslateClient([
			//'profile' => 'default',
			'region' => 'us-west-2',
			'version' => '2017-07-01',
			'credentials' => [
				'key' => AWS_ACCESS_KEY,
				'secret' => AWS_SECRET_KEY,
			],
		]);

		if(!$type){
			$currentLanguage= 'ja';
			$targetLanguage= 'en';
		}else{
			$currentLanguage= 'en';
			$targetLanguage= 'ja';
		}

		try {
			$result = $client->translateText([
				'SourceLanguageCode' => $currentLanguage,
				'TargetLanguageCode' => $targetLanguage,
				'Text' => $textTran,
			]);
			return $result['TranslatedText'];
		}catch (AwsException $e) {
			// output error message if fails
			// echo $e->getMessage();
			// echo "\n";
			$data['error'] = 1;
			return $data;
		}
	}

	/**
	 * Update or Create data
	 * @create 2020/02/13 AKB Thang
	 * @update
	 * @create
	 * @param array $data
	 * @param string $tableName
	 * @param string $condition
	 * @param array $conditionArray
	 * @param string $strScreenName
	 * @return object or string
	 */
	function fncProcessData(array $data, string $tableName, string $condition = '', $conditionArray = array(), $strScreenName = '') {
		try{
			if($condition == '') {
				$strSQL = '';
				$strSQL .= ' INSERT INTO '. $tableName;
				$strSQL .= ' (';
				$tempArr = [];
				$tempArrBind = [];
				$tempData = [];
				foreach($data as $key => $value) {
					$tempArr[] = $key;
					$tempArrBind[] = '?';
					$tempData[] = $value;
				}
				$strSQL .= implode(', ', $tempArr);
				$strSQL .= ')';
				$strSQL .= ' VALUES (';
				$strSQL .= implode(', ', $tempArrBind);
				$strSQL .= ')';
				$query = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
				$query->execute($tempData);
			} else {
				$strSQL = '';
				$strSQL .= ' UPDATE '. $tableName;
				$strSQL .= ' SET ';
				$count = 0;
				$tempArr = [];
				foreach($data as $key => $value) {
					$tempArr[] = $value;
					if($count == count($data) - 1) {
						$strSQL .= " $key=?";
					} else {
						$strSQL .= " $key=?,";
					}
					$count++;
				}
				$strSQL .= ' WHERE '.$condition;

				$query = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
				$tempData = array_merge($tempArr, $conditionArray);
				$query->execute($tempData);

			}

			foreach($tempData as $item){
				$from = '/'.preg_quote('?', '/').'/';

				$strSQL = preg_replace($from, $item, $strSQL, 1);
			}
			fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strScreenName.' '.$strSQL);
			return $query;
		} catch(\Exception $e) {
			fncWriteLog(LogLevel['Error'] , LogPattern['Error'], $strScreenName.' '.$e->getMessage());
			return $e->getMessage();
		}
	}

	/**
	 * get list string of const value in system with key
	 *
	 * @create 2020/02/21 AKB Thang
	 * @update
	 * @param array $arrTitle
	 * @param integer $intLang = 0 (default)
	 * @return array $resArrTitle
	 */
	function getListTextTranslate($arrTitle = array(), $intLang = 0) {
		if(count($arrTitle) == 0) {
			return array();
		}

		$suffixes = ($intLang == 0) ? '_JPN' : '_ENG';

		$arrAllConst = get_defined_constants();

		$resArrTitle = array();
		foreach ($arrTitle as $strTitile) {
			$key = $strTitile.$suffixes;
			if(array_key_exists($key, $arrAllConst)) {
				$resArrTitle[$strTitile] = fncHtmlSpecialChars($arrAllConst[$key]);
			} else {
				$resArrTitle[$strTitile] = '';
			}
		}

		return $resArrTitle;
	}

	/**
	 * Select Data list
	 * @create 2020/02/14 AKB Thang
	 * @select
	 * @param string $strSql
	 * @param array $conditionArray
	 * @param int $page
	 * @param boolean $pagi
	 * @param string $strScreenName
	 * @param int $listViewNum
	 * @return array or 0
	 */
	$totalPage = 0;
	$currentPage = 1;
	$totalRecord = 0;
	function fncSelectData($strSQL, $conditionArray = array(), $page = 1, $pagi = true, $strScreenName = '', $listViewNum = LIST_VIEW_NUM) {
		try {
			$query = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);

			$query->execute($conditionArray);

			if($listViewNum > 0 && $pagi) {
				$result = $query->fetchAll(PDO::FETCH_ASSOC);
				$total = count($result);
				$GLOBALS['totalRecord'] = $total;
				$GLOBALS['totalPage'] = ceil($total/$listViewNum);
				if(!is_numeric($page)) $page = 1;
				if(!is_numeric($listViewNum)) $listViewNum = 1;
				if($page=='' || !is_numeric($page) || $page<1) $page=1;
				if($page > $GLOBALS['totalPage']){
					if($GLOBALS['totalPage'] > 0){
						$page = $GLOBALS['totalPage'];
					}else{
						$page = 1;
					}
				}
				$GLOBALS['currentPage'] = $page;
				$offset = ($page-1)*$listViewNum;
				$strSQL .= ' OFFSET '.$offset.' ROWS FETCH NEXT '.$listViewNum.' ROWS ONLY;';
				$query = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
				$query->execute($conditionArray);
			}

			foreach($conditionArray as $item) {
				$from = '/'.preg_quote('?', '/').'/';

				$strSQL = preg_replace($from, $item, $strSQL, 1);
			}

			//write log
			fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strScreenName.' '.$strSQL);
			return $query->fetchAll(PDO::FETCH_ASSOC);
		} catch (\Exception $e) {
			fncWriteLog(LogLevel['Error'] , LogPattern['Error'], $strScreenName.' '.$e->getMessage());
			return 0;
		}
	}

	/**
	 * Ajax pagination
	 *
	 * @create 2020/02/14 AKB Thang
	 * @pagination
	 * @param string $link
	 * @return string
	 */
	function fncViewPagination($link) {
		if($GLOBALS['totalPage'] > 1) {
			// <div style="float: left">
			// <font size="5">
			// 	'.$GLOBALS['totalRecord'].'
			// </font>
			// </div>
			echo '
			<div class="links in-line">
			<a href="'.$link.'" class="pagi'.iff( $GLOBALS['currentPage']==1, ' disabled', ' first').'" data="1">&laquo;</a>
			<a href="'.$link.'" class="pagi'.iff( $GLOBALS['currentPage']==1, ' disabled', '').'" data="'.iff( $GLOBALS['currentPage']>1, ($GLOBALS['currentPage']-1), 1).'">&lsaquo;</a>';

			$page = $GLOBALS['currentPage'];
			$begin = $page - 2;
			$end = $page + 2;
			if($begin < 1) $begin = 1;
			if($end > $GLOBALS['totalPage']) $end = $GLOBALS['totalPage'];
			if($end <= 5) {
				$begin = 1;
				$end = $GLOBALS['totalPage']<=5 ? $GLOBALS['totalPage'] : 5;
			} else {
				$begin = $end-4;
			}
			for($i = $begin; $i <= $end; $i++) {
				echo ' <a href="'.$link.'" '.iff($i == $page, 'class="active pagi"', 'class="pagi"').' data="'.$i.'">'.$i.'</a>';
			}
				echo ' <a href="'.$link.'" class="pagi'.iff( $GLOBALS['currentPage']==$GLOBALS['totalPage'], ' disabled', '').'" data="'.iff( $GLOBALS['currentPage']<$GLOBALS['totalPage'], ($GLOBALS['currentPage']+1), $GLOBALS['totalPage']).'">&rsaquo;</a>
				<a href="'.$link.'" class="pagi'.iff( $GLOBALS['currentPage']==$GLOBALS['totalPage'], ' disabled', ' last').'" data="'.$GLOBALS['totalPage'].'">&raquo;</a>

				</div>
				';
		}
	}

	/**
	 * iff
	 *
	 * @create 2020/02/14 AKB Thang
	 * @iff
	 * @param $a
	 * @param $b
	 * @return $a $b
	 */
	function iff($condition, $a, $b) {
		if($condition) return $a;
		else return $b;
	}

	/**
	 * Export csv
	 *
	 * @create 2020/02/14 AKB Thang
	 * @export
	 * @param array $array
	 * @param string $filename
	 * @param boolean $download
	 * @param string $path
	 * @return boolean 0 1
	 */
	function fncArray2Csv($array, $filename = null, $download = 1, $path = null) {
		$filename = mb_convert_encoding($filename, 'UTF-8');
		if (stripos( $_SERVER['HTTP_USER_AGENT'], 'OS X') !== false) {

		} else {
			$filename = urlencode($filename);
		}
		try {
			if($download == 1) {
				header("Content-Type: application/force-download");
				header("Content-Type: application/octet-stream");
				header("Content-Type: application/download");
				header('Content-Type: text/csv; charset=Shift_JIS');

				// disposition / encoding on response body
				header("Content-Disposition: attachment;filename={$filename}");
				header("Content-Transfer-Encoding: binary");
				header('Expires: 0');
				header('Cache-Control: must-revalidate, post-check=0, pre-check=0');
				header('Pragma: public');
				$new_csv = fopen('php://output', 'w');
			} else {
				header('Content-Type: text/csv; charset=Shift_JIS');
				header("Content-Transfer-Encoding: binary");
				header('Expires: 0');
				header('Cache-Control: must-revalidate, post-check=0, pre-check=0');
				header('Pragma: public');
				if (!file_exists($path)) {
					mkdir($path, 0755, true);
				}
				$new_csv = fopen($path.'/'.$filename, 'w+');
			}
			// foreach($array as $row) {
			// 	if(!fputcsv($new_csv, $row)) {
			// 		return 0;
			// 	}
			// }
			// fclose($new_csv);

			fwrite($new_csv, arr2csv($array));
			fclose($new_csv);
			return 1;
		} catch (\Exception $e) {
			fncWriteLog(LogLevel['Error'] , LogPattern['Error'], $e->getMessage());
			return 0;
		}
	}

	/**
	 * create csv file temp and convert to Shift_JIS
	 *
	 * @create 2020/02/21 AKB Thang
	 * @update
	 * @param array $arrRows
	 * @return array
	 */
	function arr2csv($arrRows) {
		$fp = fopen('php://temp', 'r+b');
		foreach($arrRows as $fields) {
			fputcsv($fp, $fields);
		}
		rewind($fp);
		// Convert CRLF
		$tmp = str_replace(PHP_EOL, "\r\n", stream_get_contents($fp));
		fclose($fp);
		return mb_convert_encoding($tmp, 'SJIS', 'UTF-8');
	}

	/**
	 * Select One Record
	 *
	 * @create 2020/02/14 AKB Thang
	 * @select
	 * @param array $arrCondition
	 * @param string $strSql
	 * @param string $strScreenName
	 * @return array or string
	 */
	function fncSelectOne($strSQL, $arrCondition, $strScreenName = ''){
		try {
			$query = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);

			$query->execute($arrCondition);

			foreach($arrCondition as $item){
				$from = '/'.preg_quote('?', '/').'/';

				$strSQL = preg_replace($from, $item, $strSQL, 1);
			}
			//write log
			fncWriteLog(LogLevel['Info'], LogPattern['Sql'], $strScreenName.' '.$strSQL);
			return $query->fetch();
		} catch (\Exception $e) {
			fncWriteLog(LogLevel['Error'], LogPattern['Error'], $strScreenName.' '.$e->getMessage());
			return $e->getMessage();
		}
	}

	// ************************************************************************************************************************************************************
	//	ログ出力
	// ************************************************************************************************************************************************************
	// -------------------------------------------------------------------------
	//	function	: fncGetText
	//	value		:
	//	params		: getText
	//	params		: $key
	//				: $userLanguageType
	//	memo		: get text translate by Key
	//	create		: 2020/02/24 AKB Thang
	//	update		:
	// -------------------------------------------------------------------------
	function fncGetText($key, $userLanguageType = 0){
		$suffixes = ($userLanguageType == 0) ? '_JPN' : '_ENG';

		$arrAllConst = get_defined_constants();
		$key = $key.$suffixes;
		if(array_key_exists($key, $arrAllConst)) {
			$resArrTitle = fncHtmlSpecialChars($arrAllConst[$key]);
		} else {
			$resArrTitle = '';
		}
		return $resArrTitle;
	}

	/**
	 * send Mail
	 *
	 * @create 2020/02/21 AKB Thang
	 * @update
	 * @param array $arrUser
	 * @param string $strTitile
	 * @param string $strHtmlBody
	 * @param string $noneHtmlBody
	 * @return
	 */
	use PHPMailer\PHPMailer\PHPMailer;
	use PHPMailer\PHPMailer\Exception;
	use PHPMailer\PHPMailer\SMTP;

	require 'PHPMailer/Exception.php';
	require 'PHPMailer/PHPMailer.php';
	require 'PHPMailer/SMTP.php';

	function fncSendMail($arrUser = array(), $strTitile = '', $strHtmlBody = '', $noneHtmlBody = '') {
		if((defined('MAIL_DESTINATION_ADMIN_ADDRESS') && MAIL_DESTINATION_ADMIN_ADDRESS == '') || $strTitile == '') {
			return true;
		}
		$mail = new PHPMailer(true);
		try {
			//Server settings
			$mail->CharSet     = 'UTF-8';
			$mail->SMTPDebug   = 0;
			// $mail->SMTPDebug   = SMTP::DEBUG_SERVER;                      // Enable verbose debug output
			$mail->isSMTP();                                            // Send using SMTP
			$mail->Host        = MAIL_SMTP_HOST;                    // Set the SMTP server to send through
			$mail->SMTPAuth    = true;                                   // Enable SMTP authentication
			$mail->Username    = MAIL_SMTP_USER;                     // SMTP username
			$mail->Password    = MAIL_SMTP_PASS;                               // SMTP password
			// $mail->SMTPSecure  = PHPMailer::ENCRYPTION_STARTTLS ;         // Enable TLS encryption; `PHPMailer::ENCRYPTION_SMTPS` also accepted
			$mail->Port        = MAIL_SMTP_PORT;                                    // TCP port to connect to
			$mail->SMTPAutoTLS = true;

			//Recipients
			$mail->setFrom(MAIL_FROM_ADDRESS, 'Administrator');
			$mail->addAddress(MAIL_DESTINATION_ADMIN_ADDRESS);
			if(count($arrUser) > 0) {
				foreach($arrUser as $user) {
					$mail->addBCC(trim($user['MAIL_ADDRESS']), trim($user['USER_NAME']));     // Add a recipient
				}
			}
			// $mail->addAddress('ellen@example.com');               // Name is optional
			// $mail->addReplyTo('thangvv@akb.com.vn', 'MrThang');
			// $mail->addCC('cc@example.com');
			// $mail->addBCC('bcc@example.com');
			// $mail->SMTPDebug = 4;
			// Attachments
			// $mail->addAttachment('/var/tmp/file.tar.gz');         // Add attachments
			// $mail->addAttachment('/tmp/image.jpg', 'new.jpg');    // Optional name

			// Content
			$mail->isHTML(true);                                  // Set email format to HTML
			$mail->Subject = $strTitile;
			$mail->Body    = $strHtmlBody;
			// $mail->AltBody = $noneHtmlBody;

			$mail->send();
		} catch (Exception $e) {
			fncWriteLog(LogLevel['Error'] , LogPattern['Error'], $mail->ErrorInfo);
			// echo "Message could not be sent. Mailer Error: {$mail->ErrorInfo}";
		}
	}

	/**
	 * fncCheckEngText
	 *
	 * @create 2020/02/21 AKB Thang
	 * @update
	 * @param string $strField
	 * @return boolean true: , false:
	 */
	function fncCheckEngText($string){
		// $pos = strpos($string, ',');
		// if($pos !== false){
		// 	return false;
		// }

		return (strlen($string) != strlen(utf8_decode($string))) ? false : true;
		// for($i=0; $i<strlen($string); $i++){
		// 	$t = mb_strwidth($string[$i]);
		// 	if($t==0) return false;
		// }
		// return true;
	}

	/**
	 * get list user email
	 *
	 * @create 2020/03/13 AKB Chien
	 * @update 2020/03/26
	 * @param array $strField
	 * @param string (optional) $strScreenName
	 * @return array $arrResult
	 */
	function getDataMUserSendMail($strField, $strScreenName = '') {
		try {
			$strSQL = ' SELECT '
					. '     mu.user_no, '
					. '     mu.user_id, '
					. '     mu.user_name, '
					. '     mu.language_type, '
					. '     mu.mail_address '
					. ' FROM '
					. '     m_user AS mu '
					. ' WHERE ';
			$strField = strtoupper($strField);
			switch ($strField) {
				case 'ANNOUNCE_MAIL':
					$strSQL .= ' mu.ANNOUNCE_MAIL = 1 ';
					break;
				case 'BULLETIN_BOARD_MAIL':
					$strSQL .= ' mu.BULLETIN_BOARD_MAIL = 1 ';
					break;
				case 'INCIDENT_CASE_MAIL':
					$strSQL .= ' mu.INCIDENT_CASE_MAIL = 1 ';
					break;
				case 'REQUEST_CONTENTS_MAIL':
					$strSQL .= ' mu.REQUEST_CONTENTS_MAIL = 1 ';
					break;
			}
			$strSQL .= ' AND CAST(mu.expiration_date_s AS DATE) <= CAST(GETDATE() AS DATE) ';
			$strSQL .= ' AND CAST(mu.expiration_date_e AS DATE) >= CAST(GETDATE() AS DATE) ';
			$arrResult = fncSelectData($strSQL, array(), 1, false, $strScreenName);
			return $arrResult;
		} catch (\Exception $e) {
			fncWriteLog(LogLevel['Error'], LogPattern['Error'], $e->getMessage());
			return 0;
		}
	}

	/**
	 * remove item duplicate in array
	 *
	 * @create 2020/03/13 AKB Chien
	 * @update
	 * @param array $arrInput
	 * @return array $arrResult
	 */
	function removeDuplicateArray($arrInput) {
		if ($this->fncIsNullOrEmptyArray($arrInput)) {
			return array();
		}

		$arrRst = array_map("unserialize",
			array_unique(array_map("serialize", $arrInput)));
		return $arrRst;
	}

	/**
	 * get detail of announce
	 *
	 * @create 2020/03/13 AKB Chien
	 * @update
	 * @param integer $intNumber
	 * @param string (optional) $strScreenName
	 * @return array $arrResult
	 */
	function getInfoAnnounce($id, $strScreenName = '') {
		$arrResult = array();
		if($id == '') {
			return $arrResult;
		}
		try {
			$strSQL = ' SELECT * FROM t_announce WHERE t_announce.announce_no = ? ';
			$arrResult = fncSelectData($strSQL, array($id), 1, false, $strScreenName);
			return $arrResult;
		} catch (\Exception $e) {
			fncWriteLog(LogLevel['Error'], LogPattern['Error'], $e->getMessage());
			return 0;
		}
	}

	/**
	 *	function	: インシデント事案取得
	*
	*	@create	2020/03/13 KBS T.Masuda
	*	@update
	*	@params	  $id		        インシデント事案No
	*			  $strScreenName	画面名
	*	@return  $arrResult			インシデント事案情報
	*/
	function fncGetInfoIncident($id, $strScreenName = '') {
		$arrResult = array();
		if($id == '') {
			return $arrResult;
		}
		try {
			$strSQL = ' SELECT * FROM t_incident_case WHERE t_incident_case.incident_case_no = ? ';
			$arrResult = fncSelectData($strSQL, array($id), 1, false, $strScreenName);
			return $arrResult;
		} catch (\Exceoption $e) {
			fncWriteLog(LogLevel['Error'], LogPattern['Error'], $e->getMessage());
			return 0;
		}
	}

	/**
	 * set prefix zero with number
	 *
	 * @create 2020/03/13 AKB Chien
	 * @update
	 * @param integer $intNumber
	 * @return string $intNUmber
	 */
	function getNumberIncrementWithZero($intNumber) {
		if($intNumber == '') {
			return '0000';
		}

		if($intNumber < 10) {
			return '000'.$intNumber;
		}

		if($intNumber >= 10 && $intNumber < 100) {
			return '00'.$intNumber;
		}

		if($intNumber >= 100 && $intNumber < 1000) {
			return '0'.$intNumber;
		}

		if($intNumber >= 1000) {
			return $intNumber;
		}
	}

	/**
	 * check format datetime input (Ymd His or Ymd)
	 *
	 * @create 2020/03/13 AKB Chien
	 * @update
	 * @param integer $intNumber
	 * @return boolean true: , false:
	 */
	function checkFormatDateTimeInput($strDate) {
		$blnFlag = true;
		$arrTmpDate = explode('/', $strDate);
		if(count($arrTmpDate) == 3) {
			$arrCheckkHasHis = explode(' ', $arrTmpDate[2]);
			if(count($arrCheckkHasHis) == 1) {
				if(!ctype_digit($arrTmpDate[0]) || !ctype_digit($arrTmpDate[1]) || !ctype_digit($arrTmpDate[2])) {
					$blnFlag = false;
				} else {
					preg_match('/([1-9]\d{3}\/([1-9]|0[1-9]|1[0-2])\/([1-9]|0[1-9]|[12]\d|3[01]))/', $strDate, $arrCheckFormatDate);
					if(count($arrCheckFormatDate) == 0) {
						$blnFlag = false;
					} else {
						$blnCheckDate = checkdate($arrTmpDate[1], $arrTmpDate[2], $arrTmpDate[0]);
						if(!$blnCheckDate) {
							$blnFlag = false;
						}
					}
				}
			} else {
				if(!ctype_digit($arrTmpDate[0]) || !ctype_digit($arrTmpDate[1]) || !ctype_digit($arrCheckkHasHis[0])) {
					$blnFlag = false;
				} else {
					preg_match('/([1-9]\d{3}\/(0[1-9]|1[0-2])\/(0[1-9]|[12]\d|3[01]))/', $strDate, $arrCheckFormatDate);
					if(count($arrCheckFormatDate) == 0) {
						$blnFlag = false;
					}
					$blnCheckDate = checkdate($arrTmpDate[1], $arrCheckkHasHis[0], $arrTmpDate[0]);
					if(!$blnCheckDate) {
						$blnFlag = false;
					} else {
						$strHis = $arrCheckkHasHis[1];
						if(trim($strHis) != '') {
							preg_match('/^(([0-1]?[0-9])|([2][0-3])):([0-5]?[0-9])(:([0-5]?[0-9]))?$/', $strHis, $arrCheckHis);
							//format input is H:i:s exactly (has 2 ":")
							if(substr_count($strHis, ':') != 2) {
								$blnFlag = false;
							}
							if(count($arrCheckHis) == 0) {
								$blnFlag = false;
							}
						}
					}
				}
			}
		} else {
			$blnFlag = false;
		}
		return $blnFlag;
	}

    /**
    *   function	: 投稿ボタン後の入力チェック
    *
    *   @params	    : $strTitleOriginal        タイトル（原文）
    *                 : $strContentOriginal      内容（原文）
    *                 : $strTitleTranslation     タイトル（翻訳）
    *                 : $strContentTranslation   内容（翻訳）
    *                 : $strTranslate            翻訳言語         0:日本語⇒英語  1:英語⇒日本語
    *                 : $intManualInput          翻訳を手入力する 0：チェック無し 1:チェック有り
    *                 : $intButtonKind           0:翻訳ボタン  1:投稿ボタン
    *                 : $arrTextTranslate        表示メッセージ (ログインユーザにて切り替え後）
    *   @return	    : $arrErrorMsg             入力チェックエラーメッセージ
    *   @create	    : 2020/03/18 T.Masuda
    *   @update	    : 2020/03/27 T.Masuda	「,」を入力チェック対象外に変更
    *               : 2020/03/31 T.Masuda   原文に「,」を入力チェック対象外に変更
    */
    function fncTranInputCheck($strTitleOriginal, $strContentOriginal,
                                $strTitleTranslation,$strContentTranslation,
                                $strTranslate, $intManualInput,$intButtonKind,
                                $arrTextTranslate) {

        //入力チェックエラーメッセージ格納
        $arrErrorMsg = [];

        //タイトル（原文）未入力
        if($strTitleOriginal == '') {
            $arrErrorMsg[] = $arrTextTranslate['PUBLIC_MSG_021'];
        }
        //内容（原文）未入力
        if($strContentOriginal == '') {
            $arrErrorMsg[] = $arrTextTranslate['PUBLIC_MSG_026'];
        }

        // タイトル原文文字数
        $intOrignalTitleLength = ($strTranslate == 0) ? LENGTH_TITLE_JPN_TO_ENG
                                                      : LENGTH_TITLE_ENG_TO_JPN;
        // 内容原文文字数
        $intOrignalContentLength = ($strTranslate == 0) ? LENGTH_CONTENT_JPN_TO_ENG
                                                        : LENGTH_CONTENT_ENG_TO_JPN;

        if(mb_strlen($strTitleOriginal) > $intOrignalTitleLength) {
            $arrErrorMsg[] = ($strTranslate == 0) ? $arrTextTranslate['PUBLIC_MSG_022']
                                                  : $arrTextTranslate['PUBLIC_MSG_024'];
        }
        if(mb_strlen($strContentOriginal) > $intOrignalContentLength) {
            $arrErrorMsg[] = ($strTranslate == 0) ? $arrTextTranslate['PUBLIC_MSG_027']
                                                  : $arrTextTranslate['PUBLIC_MSG_029'];
        }

        //英語⇒日本語の翻訳時の原文の入力チェック
        if($strTranslate == 1) {

            if(!mb_detect_encoding($strTitleOriginal, 'ASCII', true)) {
                $arrErrorMsg[] =  $arrTextTranslate['PUBLIC_MSG_023'];
            }
            // content has non-ASCII char
            if(!mb_detect_encoding($strContentOriginal, 'ASCII', true)) {
                $arrErrorMsg[] =  $arrTextTranslate['PUBLIC_MSG_028'];
            }
        }

        //手入力にチェックまたは、手入力にチェックが無く、翻訳に入力があった場合に翻訳欄のチェック
        if($intButtonKind == 1 && ($intManualInput == 1 || ($intManualInput == 0
                                    && ( !empty($strTitleTranslation)
                                    || !empty($strContentTranslation))))){
             //タイトル（翻訳）未入力
             if($strTitleTranslation == '') {
                 $arrErrorMsg[] = $arrTextTranslate['PUBLIC_MSG_031'];
             }
             //内容（翻訳）未入力
             if($strContentTranslation == '') {
                 $arrErrorMsg[] = $arrTextTranslate['PUBLIC_MSG_036'];
             }

             // タイトル（翻訳）文字数
             $intTranTitleLength = ($strTranslate == 0) ? LENGTH_TITLE_ENG_TO_JPN
                                                        : LENGTH_TITLE_JPN_TO_ENG;
             // 内容（翻訳）文字数
             $intTranContentLength = ($strTranslate == 0) ? LENGTH_CONTENT_ENG_TO_JPN
                                                          : LENGTH_CONTENT_JPN_TO_ENG;

             //タイトル（翻訳）文字数チェック
             if(mb_strlen($strTitleTranslation) > $intTranTitleLength) {
                 $arrErrorMsg[] = ($strTranslate == 0)? $arrTextTranslate['PUBLIC_MSG_034']
                                                      : $arrTextTranslate['PUBLIC_MSG_032'];
             }
             //内容（翻訳）文字数チェック
             if(mb_strlen($strContentTranslation) > $intTranContentLength) {
                 $arrErrorMsg[] = ($strTranslate == 0) ? $arrTextTranslate['PUBLIC_MSG_039']
                                                       : $arrTextTranslate['PUBLIC_MSG_037'];
             }

             //日本語⇒英語の翻訳時の原文の入力チェック
             if($strTranslate == 0) {
                 //タイトル（翻訳）
                 if(!mb_detect_encoding($strTitleTranslation, 'ASCII', true)){
                        $arrErrorMsg[] =  $arrTextTranslate['PUBLIC_MSG_033'];
                 }
                 // 内容（翻訳）
                 if(!mb_detect_encoding($strContentTranslation, 'ASCII', true)){
                         $arrErrorMsg[] =  $arrTextTranslate['PUBLIC_MSG_038'];
                 }
             }
        }
        return $arrErrorMsg;
    }

	/**
	 * メール送信益田専用
	 *
	 * @create 2020/03/26 KBS T.Masuda
	 * @update
	 * @param array $arrUser
	 * @param string $strTitile
	 * @param string $strHtmlBody
	 * @param string $noneHtmlBody
	 * @return
	 */

	function fncSendMail_2($arrUser = array(), $strTitile = '', $strHtmlBody = '', $noneHtmlBody = '') {
	    if((defined('MAIL_DESTINATION_ADMIN_ADDRESS') && MAIL_DESTINATION_ADMIN_ADDRESS == '') || $strTitile == '') {
	        return true;
	    }
		$mail = new PHPMailer(true);
		try {
			//Server settings
			$mail->CharSet    = 'UTF-8';
			$mail->SMTPDebug  = 0;
			// $mail->SMTPDebug = SMTP::DEBUG_SERVER;                      // Enable verbose debug output
			$mail->isSMTP();                                            // Send using SMTP
			$mail->Host       = MAIL_SMTP_HOST;                    // Set the SMTP server to send through
			$mail->SMTPAuth   = true;                                   // Enable SMTP authentication
			$mail->Username   = MAIL_SMTP_USER;                     // SMTP username
			$mail->Password   = MAIL_SMTP_PASS;                               // SMTP password
			// $mail->SMTPSecure = PHPMailer::ENCRYPTION_STARTTLS ;         // Enable TLS encryption; `PHPMailer::ENCRYPTION_SMTPS` also accepted
			$mail->Port       = MAIL_SMTP_PORT;                                    // TCP port to connect to
			$mail->SMTPAutoTLS = false;

			//Recipients
			$mail->setFrom(MAIL_FROM_ADDRESS, 'Administrator');
			//foreach($arrUser as $user) {
			//	$mail->addAddress(trim($user['MAIL_ADDRESS']), trim($user['USER_NAME']));      Add a recipient
			//}
			$mail->addAddress(MAIL_DESTINATION_ADMIN_ADDRESS);               // Name is optional
			//$mail->addReplyTo(MAIL_DESTINATION_ADMIN_ADDRESS, 'MrThang');
			// $mail->addCC('cc@example.com');
			if(count($arrUser) > 0){
    			foreach($arrUser as $user) {
    				$mail->addBCC(trim($user['MAIL_ADDRESS']));
    			}
			}
			// $mail->SMTPDebug = 4;
			// Attachments
			// $mail->addAttachment('/var/tmp/file.tar.gz');         // Add attachments
			// $mail->addAttachment('/tmp/image.jpg', 'new.jpg');    // Optional name

			// Content
			$mail->isHTML(true);                                  // Set email format to HTML
			$mail->Subject = $strTitile;
			$mail->Body    = $strHtmlBody;
			// $mail->AltBody = $noneHtmlBody;

			$mail->send();
		} catch (Exception $e) {
			fncWriteLog(LogLevel['Error'] , LogPattern['Error'], $mail->ErrorInfo);
			// echo "Message could not be sent. Mailer Error: {$mail->ErrorInfo}";
		}
	}

	/**
	 * get list user email
	 *
	 * @create 2020/03/26 KBS T.Masuda
	 * @update
	 * @param array $strField
	 * @param string (optional) $strScreenName
	 * @return array $arrResult
	 */
	function getDataMUserSendMail2($strField, $strScreenName = '') {
		try {
			$strSQL = ' SELECT '
					. '     mu.user_no, '
					. '     mu.user_id, '
					. '     mu.user_name, '
					. '     mu.language_type, '
					. '     mu.mail_address '
					. ' FROM '
					. '     m_user AS mu '
					. ' WHERE ';
			$strField = strtoupper($strField);
			switch ($strField) {
				case 'ANNOUNCE_MAIL':
					$strSQL .= ' mu.ANNOUNCE_MAIL = 1 ';
					break;
				case 'BULLETIN_BOARD_MAIL':
					$strSQL .= ' mu.BULLETIN_BOARD_MAIL = 1 ';
					break;
				case 'INCIDENT_CASE_MAIL':
					$strSQL .= ' mu.INCIDENT_CASE_MAIL = 1 ';
					break;
				case 'REQUEST_CONTENTS_MAIL':
					$strSQL .= ' mu.REQUEST_CONTENTS_MAIL = 1 ';
					break;
			}

			$strSQL .= "AND '".date("Y/m/d")."' BETWEEN FORMAT(mu.expiration_date_s,'yyyy/MM/dd') AND FORMAT(mu.expiration_date_e,'yyyy/MM/dd')";

			$arrResult = fncSelectData($strSQL, array(), 1, false, $strScreenName);
			return $arrResult;
		} catch (\Exception $e) {
			fncWriteLog(LogLevel['Error'], LogPattern['Error'], $e->getMessage());
			return 0;
		}
	}

	/**
	 * 改行を反映
	 *
	 * @create 2020/03/27 KBS T.Masuda
	 * @update
	 * @param string $strText	対象の文字列
	 * @return string 		改行反映後の文字列
	 */
	function fncBreakAll($strText){
		return str_replace(array("\r\n", "\r","\n"), '<br>', $strText);
	}

	/*
	* @get_request_check.php
	*
	* @create 2020/03/26 AKB Thang
	* @param $arrTextTranslate
	* @return
	*/
	function fncGetRequestCheck($arrTextTranslate) {
		//check get request
		if(count($_GET) > 0){
			// $strFullUrl = (isset($_SERVER['HTTPS']) && $_SERVER['HTTPS'] === 'on'
			// ? "https" : "http") . "://$_SERVER[HTTP_HOST]$_SERVER[REQUEST_URI]";
			// $strFullUrl = strtok($strFullUrl, '?');
			// if(isset($_SERVER['HTTP_REFERER'])){
			// 	$strRedirectUrl = $_SERVER['HTTP_REFERER'];
			// }else{
			// 	$strRedirectUrl = $strFullUrl;
			// }
			// echo "
			// <script>
			// 	alert('".$arrTextTranslate['PUBLIC_MSG_049']."');
			// 	window.location.href = '".$strRedirectUrl."';
			// </script>
			// ";
			echo '<script type="text/javascript">
					function goBack() {
						// Check to see if override is needed here
						// If no override needed, call history.back()
						history.go(-1);
						return false;
					}
					alert("'.$arrTextTranslate['PUBLIC_MSG_049'].'");
					goBack();
				</script>';
			die();
		}
	}

	/*
	 * download file with file path
	 *
	 * @create 2020/04/08 Chien
	 * @param $strFilePath
	 * @return
	 */
	function fncDownloadFile($strFilePath) {
		$strFileName = mb_convert_encoding(basename($strFilePath), 'UTF-8');
		if (stripos( $_SERVER['HTTP_USER_AGENT'], 'OS X') !== false) {
		} else {
			$strFileName = urlencode(basename($strFilePath));
		}

		header('Content-Description: File Transfer');
		header('Content-Type: application/octet-stream');
		header('Content-Disposition: attachment; filename='.$strFileName);
		header('Content-Transfer-Encoding: binary');
		header('Expires: 0');
		header('Cache-Control: must-revalidate, post-check=0, pre-check=0');
		header('Pragma: public');
		header('Content-Length: ' . filesize($strFilePath));
		ob_clean();
		flush();
		readfile($strFilePath);
		exit;
	}


	/**
	 * 添付ファイルチェック
	 *
	 * @create 2020/04/24 KBS T.Masuda
	 * @update
	 * @param array $arrFiles          選択されている添付ファイル
	 *         array $arrOldData       既に登録されているデータ(更新時のみ）
	 *         integer $intActKind      0：登録、1：更新
	 *         integer $intKindScreen   0：お知らせ、1：依頼事項、2：情報
	 *         array $arrTextTranslate  表示するエラーメッセージ
	 * @return array $arrErrorMsg      エラーメッセージ
	 */

	function fncFileCheck($arrFiles,$arrOldData,$intActKind,
	                       $intKindScreen,$arrTextTranslate){
	    //入力チェックエラーメッセージ格納
	    $arrErrorMsg = [];

	    // 添付ファイルが選択されているか
	    if(count($arrFiles) > 0) {
	        $strHtmlError = '';
	        $intTotalFileSize = 0;

	        // 禁止拡張子を取得
	        $strSQL = ' SELECT * FROM m_err_extension ';
	        $arrResult = fncSelectData($strSQL, array(), 1, false, DISPLAY_TITLE);
	        // データ取得に失敗時
	        if($arrResult == 0) {
	            if($intActKind == 1) {
	                $arrErrorMsg[] = $arrTextTranslate['PUBLIC_MSG_003'];
	            }else{
	                $arrErrorMsg[] = $arrTextTranslate['PUBLIC_MSG_002'];
	            }
	            return $arrErrorMsg;
	        }

	        $arrExtensionError = array();
	        // 禁止拡張子を格納する
	        if(count($arrResult) > 0) {
	            foreach($arrResult as $objExt) {
	                $arrExtensionError[] = strtolower($objExt['ERR_EXTENSION']);
	            }
	        }

	        foreach($arrFiles as $strInputName => $objFile) {
	            $strFileName = basename($objFile["name"]);
	            $strImageFileType = strtolower(pathinfo($strFileName, PATHINFO_EXTENSION));
	            $intNum = substr($strInputName, -1);

	            // check extension file
	            if(in_array($strImageFileType, $arrExtensionError)
	                || $strImageFileType == '') {
	                $strError = $arrTextTranslate['PUBLIC_MSG_01'.$intNum];
	                $strError .= $arrTextTranslate['PUBLIC_MSG_017'];
	                $arrErrorMsg[] = $strError;
	            }

	            // ファイル名超過
	            if(mb_strwidth($strFileName) > LENGTH_FILE_NAME) {
	                $strError = $arrTextTranslate['PUBLIC_MSG_01'.$intNum];
	                $strError .= $arrTextTranslate['PUBLIC_MSG_018'];
	                $arrErrorMsg[] = $strError;
	            }

	            // 「\」マークチェック
	            if(preg_match('/[\\\]/u',$strFileName)){
	                $strErrMsg = $arrTextTranslate['PUBLIC_MSG_01'.$intNum]
	                .$arrTextTranslate['PUBLIC_MSG_050'];
	                $arrErrorMsg[] = $strErrMsg;
	            }

	            // 添付ファイル禁止文字が入っているか
	            preg_match('/[\\/\:\*\?\"\<\>\|\&\#\%\']+|((COM|com)[0-9])|(AUX|aux)|(CON|con)|((LPT|lpt)[0-9])|(NUL|nul)|(PRN|prn)/',
	                $strFileName, $strFileNameErr);
	            if(count($strFileNameErr) > 0) {
	                $strErrMsg = $arrTextTranslate['PUBLIC_MSG_01'.$intNum]
	                .$arrTextTranslate['PUBLIC_MSG_050'];
	                $arrErrorMsg[] = $strErrMsg;
	            }

	            // check file size
	            if($objFile['size'] > 0) {
	                $intTotalFileSize += $objFile['size'];
	            } else {
	                // if file has size > upload_max_filesize || post_max_size in php.ini
	                $strErrMsg = $arrTextTranslate['PUBLIC_MSG_01'.$intNum]
	                .$arrTextTranslate['PUBLIC_MSG_016'];
	                $arrErrorMsg[] = $strErrMsg;
	            }

	            $arrFileUpload[$intNum] = array($strFileName, $objFile['tmp_name']);
	        }

	        // 既に登録されている添付ファイルの容量を取得
	        if(count($arrOldData) > 0) {
	            for($i = 1; $i <= 5; $i++) {
	                if(isset($arrOldData[0]['FILE_NAME'.$i])
	                    && $arrOldData[0]['FILE_NAME'.$i] != '') {
	                        if(!isset($arrFileUpload[$i])) {
	                            $intTemp = isset($_POST['chkDelete'.$i])
	                            ? (($_POST['chkDelete'.$i] == -1) ? 0 : 1) : '';

	                            if($intTemp != 1) {
	                                if($intKindScreen == 0){
	                                   $strFilePath = SHARE_FOLDER.'\\'.ANNOUNCE_ATTACHMENT_FOLDER;
	                                }
	                                $strFilePath .= '\\'.$intAnnounceNo.'\\'.$i.'\\'.$arrOldData[0]['FILE_NAME'.$i];
	                                if(is_file($strFilePath)) {
	                                    $intTotalFileSize += filesize($strFilePath);
	                                }
	                            }
	                        }
	                    }
	            }
	        }

	        // 添付ファイルの合計容量チェック
	        if($intTotalFileSize > TOTAL_FILE_SIZE) {
	            $arrErrorMsg[] = $arrTextTranslate['PUBLIC_MSG_019'];
	        }
	    }
	    return $arrErrorMsg;
	}

	/**
	 * セッション情報を更新
	 *
	 * @create 2020/04/24 KBS T.Masuda
	 * @update
	 * @param integer $intUserNo          更新対象のユーザNo
	 *         string  $strDisplay         呼び出し元画面名
	 * @return true：成功      false：失敗
	 *  */

	function fncSessionUp($strDisplay){
	    $objLoginUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);
	    $intUserNo = $objLoginUserInfo->intUserNo;

	    //対象のユーザの情報を取得
	    $strSQL = '';	//SQL文
	    $strSQL .= ' SELECT ';
	    $strSQL .= '        m_user.user_no ';
	    $strSQL .= '      , m_user.user_id ';
	    $strSQL .= '      , m_user.password_up_date ';
	    $strSQL .= '      , m_user.expiration_date_s ';
	    $strSQL .= '      , m_user.expiration_date_e ';
	    $strSQL .= '      , m_user.language_type ';
	    $strSQL .= '      , m_user.company_no ';
	    $strSQL .= '      , m_company.company_name_jpn ';
	    $strSQL .= '      , m_company.company_name_eng ';
	    $strSQL .= '      , m_company.group_no ';
	    $strSQL .= '      , m_user.user_name ';
	    $strSQL .= '      , m_user.perm_flag ';
	    $strSQL .= '      , m_user.jcmg_tab_perm ';
	    $strSQL .= '      , m_user.announce_reg_perm ';
	    //$strSQL .= '      , m_user.bulletin_board_reg_perm ';
	    $strSQL .= '      , m_user.query_reg_perm ';
	    $strSQL .= '      , m_user.incident_case_reg_perm ';
	    $strSQL .= '      , m_user.request_reg_perm ';
	    $strSQL .= '      , m_user.information_reg_perm ';
	    $strSQL .= '      , m_user.menu_perm ';

	    $strSQL .= '      , m_user.mail_address ';
	    $strSQL .= '      , m_user.announce_mail ';
	    $strSQL .= '      , m_user.bulletin_board_mail ';
	    $strSQL .= '      , m_user.incident_case_mail ';
	    $strSQL .= '      , m_user.request_contents_mail ';

	    $strSQL .= '   FROM m_user ';
	    $strSQL .= '  INNER JOIN m_company ';
	    $strSQL .= '          ON m_user.company_no = m_company.company_no ';
	    $strSQL .= '  WHERE m_user.user_no= :user_no ';
	    $strSQL .= '    AND m_company.del_flag = 0 ';

	    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);	//DBステートメント格納
	    $objStmt->bindParam(':user_no', $intUserNo);

	    $strLogSql = $strSQL;	//ログ出力内容
	    $strLogSql = str_replace(':user_no', $intUserNo, $strLogSql);
	    fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strDisplay.$strLogSql);
	    try{
	        $objStmt->execute();
	        $arrUserData = $objStmt->fetchAll(PDO::FETCH_ASSOC);	//取得結果
	    }catch (\Exception $e){
	        fncWriteLog(LogLevel['Error'] , LogPattern['Error'], $strDisplay.$e->getMessage());
	        return false;
	    }
	    //セッションにログインユーザ情報を格納
	    $objLoginUserInfo = new clsLoginUserInfo();
	    $objLoginUserInfo->intUserNo = $arrUserData[0]['user_no'];
	    $objLoginUserInfo->strUserID = $arrUserData[0]['user_id'];
	    $objLoginUserInfo->intLanguageType = $arrUserData[0]['language_type'];
	    $objLoginUserInfo->intCompanyNo = $arrUserData[0]['company_no'];

	    if($objLoginUserInfo->intLanguageType == 0){
	        $objLoginUserInfo->strCompanyName = $arrUserData[0]['company_name_jpn'];
	    }else{
	        $objLoginUserInfo->strCompanyName = $arrUserData[0]['company_name_eng'];
	    }

	    $objLoginUserInfo->intGroupNo = $arrUserData[0]['group_no'];
	    $objLoginUserInfo->strUserName = $arrUserData[0]['user_name'];
	    $objLoginUserInfo->intJcmgTabPerm = $arrUserData[0]['jcmg_tab_perm'];
	    $objLoginUserInfo->intAnnounceRegPerm = $arrUserData[0]['announce_reg_perm'];
	    //$objLoginUserInfo->intBulletinBoardRegPerm = $arrUserData[0]['bulletin_board_reg_perm'];
	    $objLoginUserInfo->intBulletinBoardRegPerm = 1;
	    $objLoginUserInfo->intQueryRegPerm = $arrUserData[0]['query_reg_perm'];
	    $objLoginUserInfo->intIncidentCaseRegPerm = $arrUserData[0]['incident_case_reg_perm'];
	    $objLoginUserInfo->intRequestRegPerm = $arrUserData[0]['request_reg_perm'];
	    $objLoginUserInfo->intInformationRegPerm = $arrUserData[0]['information_reg_perm'];
	    $objLoginUserInfo->intMenuPerm = $arrUserData[0]['menu_perm'];


	    $objLoginUserInfo->strMailAddress = $arrUserData[0]['mail_address'];
	    $objLoginUserInfo->intAnnounceMail = $arrUserData[0]['announce_mail'];
	    $objLoginUserInfo->intBulletinBoardMail = $arrUserData[0]['bulletin_board_mail'];
	    $objLoginUserInfo->intIncidentCaseMail = $arrUserData[0]['incident_case_mail'];
	    $objLoginUserInfo->intRequestContentsMail = $arrUserData[0]['request_contents_mail'];

	    $_SESSION['LOGINUSER_INFO'] = serialize($objLoginUserInfo);
	    return true;
	}


?>
