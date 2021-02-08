<?php
// -------------------------------------------------------------------------
//	function	: ログイン画面 (仮)ページ
//	create		: 2020/01/17 KBS S.Tasaki
//	update		:
// -------------------------------------------------------------------------

require_once('common/common.php');

header('Content-type: text/html; charset=utf-8');
header('X-FRAME-OPTIONS: DENY');

//セッションスタート
session_start();

//エラーメッセージ確認
$strLoginError = '';
if(isset($_SESSION['LOGIN_ERROR'])){
	$strLoginError = $_SESSION['LOGIN_ERROR'];
	
	session_unset();
}

?>
<!DOCTYPE html> 
<html>
	<head>
		<title><?php echo HEADER_PAGE_TITLE;?></title>
		<script language="javascript">
		<!--
			//初期フォーカス
			function fncLoad(){
				document.frmLogin.user_id.focus();
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
<?php
if($strLoginError != ''){
?>
		<font color="red"><?php echo $strLoginError;?></font>
<?php
}
?>
		<form id="frmLogin" name="frmLogin" onsubmit="return false;" method="post">
			ユーザID：
			<input type="text" id="txtUserId" name="txtUserId" onkeydown="fncInputId(event.keyCode);" />
			<br />
			パスワード
			<input type="password" id="txtPassword" name="txtPassword" onkeydown="fncInputPass(event.keyCode);" />
			<br />
			<input type="button" value="ログイン" onClick="fncLogin();">
		</form>
	</body>
</html>
