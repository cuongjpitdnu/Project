<?php
/*
*	@login_proc.php
*
*	@create	2020/03/11 KBS T.Mausda
*	@update	2020/03/26 KBS T.Mausda
*/
require_once('C:/inetpub/wwwroot/information_sharing/common/common.php');

header('Content-type: text/html; charset=utf-8');
header('X-FRAME-OPTIONS: DENY');

//DB接続
if(fncConnectDB() == false){
	$_SESSION['LOGIN_ERROR'] = 'DB接続に失敗しました。';
	header('Location: login.php');
	exit;
}
//入力ユーザID
$strUserID = $_POST['txtUserId'];
//入力パスワード
$strPassword = $_POST['txtPassword'];

//ユーザID未入力
if(mb_strlen($strUserID) == 0){
    $_SESSION['LOGIN_ERROR'] = LOGIN_MSG_001_JPN . '/ '.LOGIN_MSG_001_ENG . '<br>';
}

//パスワード未入力
if(mb_strlen($strPassword) == 0){
    if(isset($_SESSION['LOGIN_ERROR'])){
        $_SESSION['LOGIN_ERROR'] .= LOGIN_MSG_002_JPN .'/ '.LOGIN_MSG_002_ENG . '<br>';
    }else{
        $_SESSION['LOGIN_ERROR'] = LOGIN_MSG_002_JPN .'/ '.LOGIN_MSG_002_ENG . '<br>';
    }
}

if(isset($_SESSION['LOGIN_ERROR'])){
    //2020/04/18 T.Masuda 入力エラーログ追加
    $strLog  = 'ログイン ログイン入力エラー';
    $strLog .= '（ユーザID = '.$strUserID;
    $strLog .= ', パスワード = '.$strPassword;
    $strLog .= ', IPアドレス =' .$_SERVER["REMOTE_ADDR"] .' ） ';
    $strLog .= $_SERVER["HTTP_USER_AGENT"];
    fncWriteLog(LogLevel['Error'] , LogPattern['Error'], $strLog);
    //2020/04/18 T.Masuda
    header('Location: login.php');
    exit;
}

//ユーザデータ取得
$arrUserData = fncLogin($strUserID, $strPassword);

//ユーザID、パスワードに一致したユーザがいない
if(count($arrUserData) == 0){
    $_SESSION['LOGIN_ERROR'] = LOGIN_MSG_003_JPN. ' / ' .LOGIN_MSG_003_ENG;
    //2020/04/18 T.Masuda 入力エラーログ追加
    $strLog  = 'ログイン ログイン入力エラー';
    $strLog .= '（ユーザID = '.$strUserID;
    $strLog .= ', パスワード = '.$strPassword;
    $strLog .= ', IPアドレス =' .$_SERVER["REMOTE_ADDR"] .' ） ';
    $strLog .= $_SERVER["HTTP_USER_AGENT"];
    fncWriteLog(LogLevel['Error'] , LogPattern['Error'], $strLog);
    //2020/04/18 T.Masuda
    header('Location: login.php');
    exit;
}

//権限設定の有無
if($arrUserData[0]['perm_flag'] == 0){
    $_SESSION['LOGIN_ERROR'] = LOGIN_MSG_004_JPN. ' / ' .LOGIN_MSG_004_ENG;
    //2020/04/18 T.Masuda 権限未設定エラーログ追加
    $strLog  = 'ログイン 未権限設定エラー';
    $strLog .= '（ユーザID = '.$strUserID;
    $strLog .= ', パスワード = '.$strPassword;
    $strLog .= ', IPアドレス =' .$_SERVER["REMOTE_ADDR"] .' ） ';
    $strLog .= $_SERVER["HTTP_USER_AGENT"];
    fncWriteLog(LogLevel['Error'] , LogPattern['Error'], $strLog);
    //2020/04/18 T.Masuda
    header('Location: login.php');
    exit;
}

//有効期限開始
$dtmExpirationDateStart = new DateTime($arrUserData[0]['expiration_date_s']);
//有効期限終了
$dtmExpirationDateEnd = new DateTime($arrUserData[0]['expiration_date_e']);
//パスワード更新日
$dtmPassUpDate = new DateTime($arrUserData[0]['password_up_date']);

//ユーザの有効期限が切れているか
if($dtmExpirationDateStart->format('Y/m/d') > date("Y/m/d")
    || $dtmExpirationDateEnd->format('Y/m/d') < date("Y/m/d")){
    $_SESSION['LOGIN_ERROR'] = LOGIN_MSG_005_JPN. ' / ' .LOGIN_MSG_005_ENG;
    //2020/04/18 T.Masuda 有効期限外エラーログ追加
    $strLog  = 'ログイン 有効期限エラー';
    $strLog .= '（ユーザID = '.$strUserID;
    $strLog .= ', パスワード = '.$strPassword;
    $strLog .= ', IPアドレス =' .$_SERVER["REMOTE_ADDR"] .' ） ';
    $strLog .= $_SERVER["HTTP_USER_AGENT"];
    fncWriteLog(LogLevel['Error'] , LogPattern['Error'], $strLog);
    //2020/04/18 T.Masuda
    header('Location: login.php');
    exit;
}

//パスワード有効期限
$dtmPassUpDate = $dtmPassUpDate->modify('+'.PASSWORD_PERIOD_DAY.' days');

//パスワードの有効期限が切れている場合、パスワード変更画面に遷移
if($dtmPassUpDate->format('Y/m/d') < date("Y/m/d")){
	$_SESSION['csrf'] =fncGetCsrfToken();
	$strCsrf = $_SESSION['csrf'];
?>
	<form action="info_src/password_chg.php" method="post" name="passchange">
    	<input type="hidden" name="user_no" value="<?php echo $arrUserData[0]['user_no']?>">
    	<input type="hidden" name="X-CSRF-TOKEN" value="<?php echo $strCsrf;?>">
	</form>
	<script>document.passchange.submit();</script>
<?php
	exit;
}

//新たなセッション情報を生成
session_regenerate_id(true);

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

$_SESSION["SES_TIME"] = date( "Y/m/d H:i:s", time() );

//ログ内容
$strLog  = 'ログイン ';
$strLog .= '（ユーザID = '.$objLoginUserInfo->strUserID;
$strLog .= ', IPアドレス =' .$_SERVER["REMOTE_ADDR"] .' ） ';
$strLog .= $_SERVER["HTTP_USER_AGENT"];
//ログ書き込み
fncWriteLog(LogLevel['Info'] , LogPattern['Login'], $strLog);
//ポータル画面へ遷移
header('Location: info_src/portal.php');
?>
