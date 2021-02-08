<?php
/*
*	@login.php
*
*	@create	2020/03/11 KBS T.Mausda
*	@update
*/

require_once('C:/inetpub/wwwroot/information_sharing/common/common.php');
require_once('login_otp_proc.php');

header('Content-type: text/html; charset=utf-8');
header('X-FRAME-OPTIONS: DENY');

//エラーメッセージ確認
$strLoginOtpError = '';
if(isset($_SESSION['LOGIN_OTP_ERROR'])){
	$strLoginOtpError = $_SESSION['LOGIN_OTP_ERROR'];
	unset($_SESSION['LOGIN_OTP_ERROR']);

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
	    <script type="text/javascript" src="info_src/js/jquery.min.js"></script>
	    <link rel="stylesheet" href="info_src/css/bootstrap.min.css">
	    <script src="info_src/js/bootstrap.min.js"></script>
		<script src="info_src/js/common.js"></script>
		<script language="javascript">
		<!--
			//初期フォーカス
			window.onload = function(){
				document.frmLoginOtp.txtUserId.focus();
			}

			//IDの入力を確認.
			function fncInputId(keyCode){
				if(keyCode == 13){
					if(document.frmLoginOtp.txtUserId.value != ""){
						document.frmLoginOtp.txtPassword.focus();
					}
				}
			}

			//パスワードの入力を確認.
			function fncInputPass(keyCode){
				if(keyCode == 13){
					if(document.frmLoginOtp.txtPassword.value != ""){
						fncLogin();
					}
				}
			}

			//ログイン
			function fncLogin(){
                document.frmLoginOtp.action = "login.php";
                document.frmLoginOtp.submit();
			}
		-->
	</script>
	</head>
	<body>
	<div class="main-content">
        <div class="main-form">
			<div class="form-title"><?php echo LOGIN_OTP_TEXT_001?></div>
            <div id="error-content" class="error-messeage-otp">
                <?php if(isset($strLoginOtpError)) {
                    echo '<span class="error">'.$strLoginOtpError.'</span>';
                    unset($strLoginOtpError);
                }
                ?>
            </div>
            <form id="frmLoginOtp" name="frmLoginOtp" onsubmit="return false;" method="post" autocomplete="off">
                <div class="form-body-300 top-20">
                        <div class="lb-left-long" ><?php echo LOGIN_OTP_TEXT_002?>:</div>
                        <span class="ip-span"><input type="text" id="txtUserId" name="txtUserId" value="<?php echo @$strUserID?>"  class="t-input" onkeydown="fncInputId(event.keyCode);" <?php echo $strDisable; ?> ></span>
                        <div class="lb-left-long" ><?php echo LOGIN_OTP_TEXT_003?>：</div>
                        <span class="ip-span"><input type="password" id="txtPassword" name="txtPassword" value="<?php echo @$strPassword?>" class="t-input" onkeydown="fncInputPass(event.keyCode);" <?php echo $strDisable ?> ></span>
                        <?php if(isset($intMode)){
                            if($intMode == 2){
                                echo '<input type="hidden" value="'.@$strUserID.'" name="txtUserId">';
                                echo '<input type="hidden" value="'.@$strPassword.'" name="txtPassword">';
                            }
                        ?>
                        <div class="lb-left-long" ><?php echo LOGIN_OTP_TEXT_004?>：</div>
                        <span class="ip-span"><input type="text" id="otp" name="txtOTP"  class="t-input" onkeydown="fncInputPass(event.keyCode);" ></span>
                        <?php } ?>
                        <input type="hidden" value="<?php echo (isset($intMode)) ? $intMode : 1 ?>" name="mode" id="mode">

                 </div>
               	 <div class="form-footer top-20 text-right text-center">
                	<input type="button" value="<?php echo LOGIN_OTP_BUTTON_001?>" class="tbtn tbtn-defaut" onClick="fncLogin();">
                 </div>
        	</form>
        </div>
    </div>
	</body>
</html>
