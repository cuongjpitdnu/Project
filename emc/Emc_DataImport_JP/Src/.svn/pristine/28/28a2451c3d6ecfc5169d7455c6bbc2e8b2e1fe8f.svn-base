<?php
// -------------------------------------------------------------------------
//	function	: ログイン処理画面 (仮)ページ
//	create		: 2020/01/17 KBS S.Tasaki
//	update		:
// -------------------------------------------------------------------------

require_once('common/common.php');

header('Content-type: text/html; charset=utf-8');

//セッションスタート
session_start();

//DB接続
if(fncConnectDB() == false){
	$_SESSION['LOGIN_ERROR'] = 'DB接続に失敗しました。';
	header('Location: login.php');
	exit;
}

$strUserID = $_POST['txtUserId'];
$strPassword = $_POST['txtPassword'];

$arrUserData = fncLogin($strUserID, $strPassword);

if(count($arrUserData) > 0){
	//新たなセッション情報を生成
	session_regenerate_id(true);
	
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
	$objLoginUserInfo->intAnnounceRegPerm = $arrUserData[0]['announce_reg_perm'];
	$objLoginUserInfo->intBulletinBoardRegPerm = $arrUserData[0]['bulletin_board_reg_perm'];
	$objLoginUserInfo->intQueryRegPerm = $arrUserData[0]['query_reg_perm'];
	$objLoginUserInfo->intIncidentCaseRegPerm = $arrUserData[0]['incident_case_reg_perm'];
	$objLoginUserInfo->intRequestRegPerm = $arrUserData[0]['request_reg_perm'];
	$objLoginUserInfo->intInformationRegPerm = $arrUserData[0]['information_reg_perm'];
	$objLoginUserInfo->intMenuPerm = $arrUserData[0]['menu_perm'];
	
	$_SESSION['LOGINUSER_INFO'] = serialize($objLoginUserInfo);
	
	//ログ書き込み
	fncWriteLog(LogLevel['Info'] , LogPattern['Login'], 'ログイン（ユーザID = ' .$objLoginUserInfo->strUserID .', IPアドレス =' .$_SERVER["REMOTE_ADDR"] .' ） ' .$_SERVER["HTTP_USER_AGENT"]);
	//ポータル画面へ遷移
	header('Location: portal.php');
}else{
	$_SESSION['LOGIN_ERROR'] = 'ログインに失敗しました。';
	header('Location: login.php');
	exit;
}

?>
