<?php
/*
*	@login.php
*
*	@create	2020/03/11 KBS T.Mausda
*	@update
*/
require_once ('password_chg_proc.php');

header('Content-type: text/html; charset=utf-8');
header('X-FRAME-OPTIONS: DENY');

?>
<!DOCTYPE html>
<html>
	<head>
		<meta charset="UTF-8">
		<meta name="csrf-token" content="<?php echo $strCsrf; ?>">
		<title><?php echo HEADER_PAGE_TITLE;?></title>
		<link rel="stylesheet" type="text/css" href="css/style.css">
	    <link rel="stylesheet" type="text/css" href="css/table.css">
	    <link type="text/css" href="css/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" />
	    <script type="text/javascript" src="js/jquery.min.js"></script>
	    <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.js"></script>
	    <script type="text/javascript" src="js/jquery.ui.datepicker-ja.js"></script>

	    <link rel="stylesheet" href="css/bootstrap.min.css">
	    <script src="js/bootstrap.min.js"></script>
		<script src="js/common.js"></script>
		<script language="javascript">
		<!--
			//初期フォーカス
			window.onload = function(){
				document.frmLogin.txtPasword.focus();
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
                $('.error').html('');
                setTimeout(function(){
                if ( confirm('<?php echo $arrText['PASSWORD_CHG_MSG_001'] ?>') == false ) {
                    return false ;
                } else {
                    document.frmLogin.action = "password_chg.php";
                    document.frmLogin.submit();
                }
                },15);
			}
		-->
	</script>
	</head>
	<body>
	<div class="main-content">
        <div class="main-form">
			<div class="form-title"><?php echo $arrText['PASSWORD_CHG_TEXT_001']?></div>
			<?php 
			    echo '<div class="pad-left-10 top-10">';
                echo '<span class="txt-red">'.$_SESSION['PASSWORD_CHG_EVER_MSG'].'</span>';
                echo '<span class="txt-red error">'.$_SESSION['PASSWORD_CHG_MSG'].'</span>';
                echo '</div>';
			    unset($_SESSION['PASSWORD_CHG_MSG']);
			?>
            <form id="frmLogin" name="frmLogin" onsubmit="return false;" method="post" autocomplete="off">
                <div class="form-body-400 top-20">
                        <input type="hidden" name="user_no" value="<?php echo @$_SESSION['user_no']?>">
                        <input type="hidden" name="X-CSRF-TOKEN" id="csrf" value="<?php echo $strCsrf ?>">
                        <div class="" ><?php echo $arrText['PASSWORD_CHG_TEXT_003']?></div>
                        <span class="ip-span"><input type="password" id="txtPasword" name="txtPasword"  class="t-input" onkeydown="fncInputId(event.keyCode);" ></span>
                        <div class="" ><?php echo $arrText['PASSWORD_CHG_TEXT_004']?></div>
                        <span class="ip-span"><input type="password" id="txtNewPassword" name="txtNewPassword"  class="t-input" onkeydown="fncInputPass(event.keyCode);" ></span>
                        <div class="text-blue"><?php echo $arrText['PASSWORD_CHG_TEXT_005']?></div>
                        <div class="" ><?php echo $arrText['PASSWORD_CHG_TEXT_006']?></div>
                        <span class="ip-span"><input type="password" id="txtNewPassword2" name="txtNewPassword2"  class="t-input" onkeydown="fncInputPass(event.keyCode);" ></span>
                 </div>
               	 <div class="form-footer top-20 text-right text-center">
                	<input type="button" value="<?php echo $arrText['PUBLIC_BUTTON_017']?>" class="tbtn tbtn-defaut" onClick="fncLogin();">
                    <a href="login.php" class="tbtn tbtn-defaut pad-left-20"><?php echo $arrText['PUBLIC_BUTTON_018']?></a>
                 </div>
        	</form>
        </div>
    </div>
	</body>
</html>
