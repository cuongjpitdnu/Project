<?php
/*
* @session_check.php
*
* @create 2020/03/11 KBS S.Tasaki
* @update 2020/03/12 KBS S.Tasaki fncSessionTimeOutCheck関数を修正
*/

/**
* セッションタイムアウトを確認する。
*
* @create 2020/03/11 KBS S.Tasaki
* @update 2020/03/12 KBS S.Tasaki ajax通信でも画面遷移が可能になるよう修正
* @param なし
* @return なし
*/
function fncSessionTimeOutCheck($strFlagProc = '')
{
	//セッションが存在するか確認
	if(!isset($_SESSION["SES_TIME"])){
		header("location:login.php");
		exit;
	}

	//セッションがタイムアウトしていないか確認
	$intSesTime = strtotime($_SESSION["SES_TIME"]);
	$intNowTime = strtotime( date( "Y/m/d H:i:s", time() ) );

	$intTotalTime = $intNowTime - $intSesTime;
	if($intTotalTime > SESSION_TIMEOUT_TIME){
		$_SESSION['LOGIN_ERROR'] = PUBLIC_MSG_044_JPN .'/' .PUBLIC_MSG_044_ENG;

		//-----------------------------------------
		//  2020/03/12 KBS S.Tasaki
		//  ajax通信でも画面遷移が可能になるよう修正
		//-----------------------------------------
		//header("location:login.php");
		// echo '<script>window.location.href="login.php";</script>';
		// 2020/03/12 KBS S.Tasaki ajax通信でも画面遷移が可能になるよう修正

		//-----------------------------------------
		//  2020/03/18 AKB Chien
		//  new version for all case of action ajax
		//-----------------------------------------
		if($strFlagProc == '') {
			echo '<script>window.location.href="login.php";</script>';
		} else {
			echo 'window.location.href="login.php";';
		}
		// 2020/03/18 AKB Chien new version for all case of action ajax

		exit;
	}

	//現在日時を再度セット
	$_SESSION["SES_TIME"] = date( "Y/m/d H:i:s", time() );

}
?>
