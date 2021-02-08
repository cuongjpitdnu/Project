<?php
// -------------------------------------------------------------------------
//	function	: ユーザのログイン情報を検証する
//	create		: 2020/02/17 KBS Tamnv
//	update		:
// ------------------------------------------------------------------------
require_once('common/common.php');
header('Content-type: text/html; charset=utf-8');
header('X-FRAME-OPTIONS: DENY');

if(!isset($_SESSION))
{
    session_start();
}

//DB接続.
if(fncConnectDB() == false){
    $_SESSION['LOGIN_ERROR'] = 'DB接続に失敗しました。';
    header('Location: login.php');
    exit;
}

//DB接続.
if(unserialize(@$_SESSION['LOGINUSER_INFO']) == NULL){
    $alertMsg = PUBLIC_MSG_008_JPN.'/ '.PUBLIC_MSG_008_ENG;
    echo "
    <script>
        alert('".$alertMsg."');
        window.location.href = 'login.php';
    </script>
    ";
    exit;
}

if(!isset($objLoginUserInfo)){
    $objLoginUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);
}

//check user language type to set end language
if($objLoginUserInfo->intLanguageType){
    $strEndLang = '_ENG'; // $endLang
    $intMaxInputChat = 200;
}else{
    $strEndLang = '_JPN';
    $intMaxInputChat = 100;
}

$_SESSION['SQL_GET_DATA_ERROR'] = NULL;

/*if($objLoginUserInfo-> != 1){
    $_SESSION['LOGIN_ERROR'] = PUBLIC_MSG_009_ENG;
    header('Location: login.php');
    exit;
}*/


//ログインユーザ情報を取得


?>