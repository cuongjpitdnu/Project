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

//DB接続クラスを設定.
$g_dbaccess = null;
$g_dbaccess = new sqlsrv_dbaccess();

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


// ************************************************************************************************************************************************************
//	CLASS
// ************************************************************************************************************************************************************

//ログインユーザー情報格納クラス
class clsLoginUserInfo{
	public $intUserNo;					//ユーザNo
	public $strUserID;					//ユーザID
	public $intLanguageType;			//言語タイプ
	public $intCompanyNo;				//会社No
	public $strCompanyName;				//会社名
	public $intGroupNo;					//グループNo
	public $strUserName;				//ユーザ名
	public $intAnnounceRegPerm;			//お知らせ登録権限
	public $intBulletinBoardRegPerm;	//掲示板登録権限
	public $intQueryRegPerm;			//問い合わせ登録権限
	public $intIncidentCaseRegPerm;		//インシデント事案登録権限
	public $intRequestRegPerm;			//依頼事項登録権限
	public $intInformationRegPerm;		//情報登録権限
	public $intMenuPerm;				//各種メニュー権限
}


// ************************************************************************************************************************************************************
//	DB CONNECT
// ************************************************************************************************************************************************************


// -------------------------------------------------------------------------
//	function	: データベース接続
//	value		:
//	params		:
//	memo		:
//	create		: 2020/01/17 KBS S.Tasaki
//	update		:
// -------------------------------------------------------------------------
function fncConnectDB(){
	return $GLOBALS['g_dbaccess']->fncDbConnect();
}


// ************************************************************************************************************************************************************
//	DB SELECT
// ************************************************************************************************************************************************************

// -------------------------------------------------------------------------
//	function	: ログイン
//	value		:
//	params		:
//	memo		:
//	create		: 2020/01/17 KBS S.Tasaki
//	update		:
// -------------------------------------------------------------------------
function fncLogin($strUserID, $strPassword){
	
	$strSQL = '';
	$strSQL .= ' SELECT ';
	$strSQL .= '        m_user.user_no ';
	$strSQL .= '      , m_user.user_id ';
	$strSQL .= '      , m_user.language_type ';
	$strSQL .= '      , m_user.company_no ';
	$strSQL .= '      , m_company.company_name_jpn ';
	$strSQL .= '      , m_company.company_name_eng ';
	$strSQL .= '      , m_company.group_no ';
	$strSQL .= '      , m_user.user_name ';
	$strSQL .= '      , m_user.announce_reg_perm ';
	$strSQL .= '      , m_user.bulletin_board_reg_perm ';
	$strSQL .= '      , m_user.query_reg_perm ';
	$strSQL .= '      , m_user.incident_case_reg_perm ';
	$strSQL .= '      , m_user.request_reg_perm ';
	$strSQL .= '      , m_user.information_reg_perm ';
	$strSQL .= '      , m_user.menu_perm ';
	$strSQL .= '   FROM m_user ';
	$strSQL .= '  INNER JOIN m_company ';
	$strSQL .= '          ON m_user.company_no = m_company.company_no ';
	$strSQL .= '  WHERE m_user.user_id = :user_id ';
	$strSQL .= '    AND m_user.password = :password ';
	
	$objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
	$objStmt->bindParam(':user_id', $strUserID);
	$objStmt->bindParam(':password', $strPassword);
	
	$strLogSql = $strSQL;
	$strLogSql = str_replace(':user_id', $strUserID, $strLogSql);
	$strLogSql = str_replace(':password', $strPassword, $strLogSql);
	fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);
	
	$objStmt->execute();
	$arrResult = $objStmt->fetchAll(PDO::FETCH_ASSOC);
	return $arrResult;
}



// ************************************************************************************************************************************************************
//	FUNCTION
// ************************************************************************************************************************************************************
// -------------------------------------------------------------------------
//	function	: エスケープ処理
//	value		:
//	params		:
//	memo		:
//	create		: 2020/01/22 KBS S.Tasaki
//	update		:
// -------------------------------------------------------------------------
function fncHtmlSpecialChars($item){
	return htmlspecialchars($item, ENT_QUOTES, 'UTF-8');
}

// -------------------------------------------------------------------------
//	function	: トークン作成処理
//	value		:
//	params		:
//	memo		:
//	create		: 2020/01/22 KBS S.Tasaki
//	update		:
// -------------------------------------------------------------------------
function fncGetCsrfToken() {
	$TOKEN_LENGTH = 16;
	$bytes = openssl_random_pseudo_bytes($TOKEN_LENGTH);
	return bin2hex($bytes);
}




// ************************************************************************************************************************************************************
//	ログ出力
// ************************************************************************************************************************************************************
// -------------------------------------------------------------------------
//	function	: ログの出力を行う
//	value		: 
//	params		: ログレベル
//				: ログパターン
//				: 出力メッセージ
//	memo		: 
//	create		: 2020/01/17 KBS S.Tasaki
//	update		: 
// -------------------------------------------------------------------------
function fncWriteLog($intLogLevel, $intLogPattern, $strMsg){
	
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
	$strLogLevel = 'INFO';
	if($intLogLevel == 0){
		$strLogLevel = 'ERROR';
	}
	
	$strLogMsg = '';
	$strLogMsg = date('Y/m/d H:i:s') .' ' .$strLogLevel .' ' .$strMsg ."\r\n";

	
	//ファイル名、パスを作成
	$strFileName = date('md') .'.log';
	$strLogPath = LOG_FOLDER;
	
	//フルパスを作成
	$strFilePath = $strLogPath .'\\' .$strFileName;
	
	if(!file_exists($strFilePath)){
		//指定ファイルが存在しない場合は、ファイルを作成する.
		touch($strFilePath);
	}
	
	//ファイルを追記モードでオープン
	$fno = fopen($strFilePath, 'a');
	
	//文字列を書き出す.
	fwrite($fno, $strLogMsg);

	//ファイルをクローズ
	fclose($fno);
    fncWriteLogToDb($intLogLevel, $intLogPattern, $strMsg);
	
}


// ************************************************************************************************************************************************************
//	ログ出力
// ************************************************************************************************************************************************************
// -------------------------------------------------------------------------
//	function	: Write Log to DB
//	value		:
//	params		: ログレベル
//				: ログパターン
//				: 出力メッセージ
//	memo		:
//	create		: 2020/02/06 KBS Tamnv
//	update		:
// -------------------------------------------------------------------------

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
	$strLogLevel = 'INFO';
	if($intLogLevel == 0){
		$strLogLevel = 'ERROR';
	}

	$strLogMsg = date('Y/m/d H:i:s') .' ' .$strLogLevel .' ' .$strMsg ."\r\n";
    $regDate = date('Y-m-d');

    $strSQL = '';
    $strSQL .= ' INSERT INTO t_log ';
    $strSQL .= ' VALUES(';
    $strSQL .= ' :reg_date';
    $strSQL .= ' ,(SELECT ISNULL(MAX( LOGID ), 0)+ 1  FROM t_log WHERE REG_DATE = :reg_d) ';
    $strSQL .= ' ,:content)';

    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);

    $objStmt->bindParam(':reg_date', $regDate);
    $objStmt->bindParam(':reg_d', $regDate);
    $objStmt->bindParam(':content', $strLogMsg);
    $objStmt->execute();
}


?>