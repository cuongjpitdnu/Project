<?php
/*
*	@login.php
*
*	@create	2020/03/11 KBS T.Mausda
*	@update
*/

require_once('C:/inetpub/wwwroot/information_sharing/common/common.php');

header('Content-type: text/html; charset=utf-8');
header('X-FRAME-OPTIONS: DENY');

//エラーメッセージ確認
$strLoginError = '';
if(isset($_SESSION['LOGIN_ERROR'])){
	$strLoginError = $_SESSION['LOGIN_ERROR'];
    
	unset($_SESSION['LOGIN_ERROR']);

}
if(isset($_SESSION['LOGINUSER_INFO'])){
    unset($_SESSION['LOGINUSER_INFO']);
}
if(isset($_SESSION['tab'])){
    unset($_SESSION['tab']);
}
//-----------------------------------------
//  2020/03/18 KBS S.Tasaki
//  セッションタイムアウト用のセッションがあれば削除する処理を追加
//-----------------------------------------
if(isset($_SESSION['SES_TIME'])){
    unset($_SESSION['SES_TIME']);
}
//  2020/03/18 KBS S.Tasaki セッションタイムアウト用のセッションがあれば削除する処理を追加

//2020/04/24 T.Masuda 他社を表示しないセッションを削除
if(isset($_SESSION['checkWithoutCom'])){
    unset($_SESSION['checkWithoutCom']);
}
//2020/04/24 T.Masuda

?>
<!DOCTYPE html>
<html>
	<head>
		<meta charset="UTF-8">
		<meta name="csrf-token" content="<?php echo $strCsrf; ?>">
		<title><?php echo HEADER_PAGE_TITLE;?></title>
		<link rel="stylesheet" type="text/css" href="info_src/css/style.css">
	    <link rel="stylesheet" type="text/css" href="info_src/css/table.css">
	    <link type="text/css" href="info_src/css/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" />
	    <script type="text/javascript" src="info_src/js/jquery.min.js"></script>
	    <script type="text/javascript" src="info_src/js/jquery-ui-1.10.4.custom.js"></script>
	    <script type="text/javascript" src="info_src/js/jquery.ui.datepicker-ja.js"></script>

	    <link rel="stylesheet" href="info_src/css/bootstrap.min.css">
	    <script src="info_src/js/bootstrap.min.js"></script>
		<script src="info_src/js/common.js"></script>
		<script language="javascript">
		<!--
			//初期フォーカス
			window.onload = function(){
				document.frmLogin.txtUserId.focus();
			}

			//IDの入力を確認.
			function fncInputId(keyCode){
				if(keyCode == 13){
					if(document.frmLogin.txtUserId.value != ""){
						document.frmLogin.txtPassword.focus();
					}
				}
			}

			//パスワードの入力を確認.
			function fncInputPass(keyCode){
				if(keyCode == 13){
					if(document.frmLogin.txtPassword.value != ""){
						fncLogin();
					}
				}
			}

			//ログイン
			function fncLogin(){
				document.frmLogin.action = "login_proc.php";
				document.frmLogin.submit();
			}

		-->
	</script>
	</head>
	<body>
	<div class="main-content">
        <div class="main-form">
			<div class="form-title">LOGIN</div>
			<?php if(isset($strLoginError)) {
			    echo '<font color="red" class="error">'.$strLoginError.'</font>';
			    unset($strLoginError);
			}else{
				echo '<font color="red" class="error"></font>';
			}
			?>
            <form id="frmLogin" name="frmLogin" onsubmit="return false;" method="post" autocomplete="off">
                <div class="form-body-300 top-20">
                        <div class="lb-left" >User ID:</div>
                        <span class="ip-span"><input type="text" id="txtUserId" name="txtUserId"  class="t-input" onkeydown="fncInputId(event.keyCode);" ></span>
                        <div class="lb-left" >Password：</div>
                        <span class="ip-span"><input type="password" id="txtPassword" name="txtPassword"  class="t-input" onkeydown="fncInputPass(event.keyCode);" ></span>
                 </div>
               	 <div class="form-footer top-20 text-right text-center">
                	<input type="button" value="Login" class="tbtn tbtn-defaut" onClick="fncLogin();">
                 </div>
        	</form>
        </div>
    </div>
	</body>
</html>
