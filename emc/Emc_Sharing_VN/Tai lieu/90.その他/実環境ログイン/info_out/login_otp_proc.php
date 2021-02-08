<?php
/*
*	@login_otp_proc.php
*
*	@create	2020/04/14 KBS Tam
*	@update
*/

require_once('C:/inetpub/wwwroot/information_sharing/common/common.php');
require_once('C:/inetpub/wwwroot/information_sharing/common/Rakunin/cloudentifysdk.php');

header('Content-type: text/html; charset=utf-8');
header('X-FRAME-OPTIONS: DENY');

const DISPLAY_NAME = 'ログイン(OTP認証) ';
//define array translate text message
$arrMSG = [
    'LOGIN_MSG_007',
    'LOGIN_MSG_008',
    'LOGIN_MSG_009',
    'LOGIN_MSG_010',
    'LOGIN_MSG_011',
    'LOGIN_MSG_012',
    'LOGIN_MSG_013',
    'LOGIN_MSG_014',
    'LOGIN_MSG_015',
];

$strDisable = '';
//DB connection
if(fncConnectDB() == false){
	$_SESSION['LOGIN_OTP_ERROR'] = 'DB接続に失敗しました。';
	header('Location: login.php');
	exit;
}

// check if isset txtUserId to process, if don't have continue load view login_otp.php
if(isset($_POST['txtUserId'])){
    //Input user ID
    $strUserID = $_POST['txtUserId'];
    //Input password
    $strPassword = $_POST['txtPassword'];
    $strMode = $_POST['mode'];

    //Log contents
    $strLogBase = '（ユーザID = '.$strUserID;
    $strLogBase .= ' , パスワード = '.$strPassword;
    $strLogBase .= ':PARAM';
    $strLogBase .= ' , IPアドレス =' .$_SERVER["REMOTE_ADDR"] .' ） ';
    $strLogBase .= $_SERVER["HTTP_USER_AGENT"];

    //No user ID entered
    if(mb_strlen($strUserID) == 0){
        $_SESSION['LOGIN_OTP_ERROR'] = LOGIN_MSG_001_JPN .' / '.LOGIN_MSG_001_ENG.'<br>';
    }

    //Password not entered
    if(mb_strlen($strPassword) == 0){
        if(isset($_SESSION['LOGIN_OTP_ERROR'])){
            $_SESSION['LOGIN_OTP_ERROR'] .= LOGIN_MSG_002_JPN .' / '.LOGIN_MSG_002_ENG.'<br>';
        }else{
            $_SESSION['LOGIN_OTP_ERROR'] = LOGIN_MSG_002_JPN .' / '.LOGIN_MSG_002_ENG.'<br>';
        }
    }

    // If have error, break to page view login_otp.php
    if(isset($_SESSION['LOGIN_OTP_ERROR'])){
        $strLogBase = str_replace(':PARAM', "", $strLogBase);
        $strErrorLog = DISPLAY_NAME.' ログイン入力エラー '.$strLogBase;
        fncWriteLog(LogLevel['Error'] , LogPattern['Error'], $strErrorLog);
        header('Location: login.php');
        exit;
    }

    //User data acquisition
    $arrUserData = fncLogin($strUserID, $strPassword, DISPLAY_NAME);
    if($arrUserData){
        fncValideUser($arrUserData,$strLogBase);
        $intMode = $_POST['mode'];
        //mode 1 show login form, mode 2 show type OTP form.
        if($intMode == 1){
            $intMode = 2;
            $intType = 1;
            //check user exists with type 1
            $intResult = fncRakuService($strUserID, $strPassword,1);
            // at here, if $intResult == 0 is success, if != 0 is error code
            if($intResult == OTP_OK){
                //user exist, return data is 0 status, continue call service send OTP with type 4
                $_SESSION['OTP_SESSION_TIME'] = NULL;
                $intType = 4;
                $intResult = fncRakuService($strUserID, $strPassword,4);
            }
            // if $intResult not null is send OTP fail
            if($intResult){
                //$intResult != 0 write error to log with error code
                $strError = fncErrorSendSMS($intResult,$intType);
            }else{
                $_SESSION['OTP_SESSION_TIME'] = date('Y-m-d H:i:s');
                $intMode = 2;
                $strDisable = 'disabled';
            }
        }else{
            // Mode = 2 show type OTP form
            $strDisable = 'disabled';
            $strCode = $_POST['txtOTP'];

            // check session OTP time out
            if(date('Y-m-d H:i:s',strtotime($_SESSION['OTP_SESSION_TIME']) + OTP_TIME_OUT) < date("Y-m-d H:i:s")){
                $_SESSION['LOGIN_OTP_ERROR'] = LOGIN_MSG_015_JPN.' / '.LOGIN_MSG_015_ENG;
                $strLogBase = str_replace(':PARAM', ", OTP = ".$strCode, $strLogBase);
                $strErrorLog = DISPLAY_NAME.' 有効期限エラー'.$strLogBase;
                fncWriteLog(LogLevel['Error'] , LogPattern['Error'], $strErrorLog);
                header('Location: login.php');
                exit;
            }
            //call service otp_auth with type 6
            $intResult = fncRakuService($strUserID, $strPassword,6,$strCode);
            // if result == 0 => success, else 0 is error code
            if($intResult){
                $strError = fncErrorSendSMS($intResult,6);
                $_SESSION['LOGIN_OTP_ERROR'] .= $strError;
            }else{
                // otp_auth success process login
                fncProcessLogin($arrUserData);
            }
        }
    }else{
        $strLogBase = str_replace(':PARAM', "", $strLogBase);
        $strErrorLog = DISPLAY_NAME.'ログイン入力エラー '.$strLogBase;
        fncWriteLog(LogLevel['Error'] , LogPattern['Error'], $strErrorLog);

        //No user who matches the user ID and password
        $_SESSION['LOGIN_OTP_ERROR'] = LOGIN_MSG_003_JPN. ' / ' .LOGIN_MSG_003_ENG;
        header('Location: login.php');
        exit;
    }

    $strCsrf = $_SESSION['csrf'];
}

/**
 * Validate Object user
 *
 * @create 2020/04/15 KBS Tam.nv
 * @update
 * @param $arrUserData user data array
 * @param $strLogBase string log
 * @return void
 * @throws Exception
 */
function fncValideUser($arrUserData,$strLogBase){

    //Existence of permission setting
    if($arrUserData[0]['perm_flag'] == 0){
        $strLogBase = str_replace(':PARAM', "", $strLogBase);
        $strErrorLog = DISPLAY_NAME.'ログイン入力エラー '.$strLogBase;
        fncWriteLog(LogLevel['Error'] , LogPattern['Error'], $strErrorLog);

        $_SESSION['LOGIN_OTP_ERROR'] = LOGIN_MSG_004_JPN. ' / ' .LOGIN_MSG_004_ENG;
        header('Location: login.php');
        exit;
    }

    //Expiration date start
        $dtmExpirationDateStart = new DateTime($arrUserData[0]['expiration_date_s']);
    //Expired
        $dtmExpirationDateEnd = new DateTime($arrUserData[0]['expiration_date_e']);
    //Password update date
        $dtmPassUpDate = new DateTime($arrUserData[0]['password_up_date']);

    //Whether the user has expired
        if($dtmExpirationDateStart->format('Y/m/d') > date("Y/m/d")
            || $dtmExpirationDateEnd->format('Y/m/d') < date("Y/m/d")){
            $_SESSION['LOGIN_OTP_ERROR'] = LOGIN_MSG_005_JPN. ' / ' .LOGIN_MSG_005_ENG;

            $strLogBase = str_replace(':PARAM', "", $strLogBase);
            $strErrorLog = DISPLAY_NAME.'ログイン入力エラー '.$strLogBase;
            fncWriteLog(LogLevel['Error'] , LogPattern['Error'], $strErrorLog);

            header('Location: login.php');
            exit;
        }

    //Password expiration
    $dtmPassUpDate = $dtmPassUpDate->modify('+'.PASSWORD_PERIOD_DAY.' days');

    //If the password has expired, transition to the password change screen
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

}

/**
 * Login process
 *
 * @create 2020/04/15 KBS Tam.nv
 * @update
 * @param $arrUserData user data array
 * @return void
 * @throws Exception
 */

function fncProcessLogin($arrUserData){
    //Expiration date start
        $dtmExpirationDateStart = new DateTime($arrUserData[0]['expiration_date_s']);
    //Expired
        $dtmExpirationDateEnd = new DateTime($arrUserData[0]['expiration_date_e']);
    //Password update date
        $dtmPassUpDate = new DateTime($arrUserData[0]['password_up_date']);

    //Whether the user has expired
        if($dtmExpirationDateStart->format('Y/m/d') > date("Y/m/d")
            || $dtmExpirationDateEnd->format('Y/m/d') < date("Y/m/d")){
            $_SESSION['LOGIN_OTP_ERROR'] = LOGIN_MSG_005_JPN. ' / ' .LOGIN_MSG_005_ENG;
            header('Location: login.php');
            exit;
        }

    //Password expiration
    $dtmPassUpDate = $dtmPassUpDate->modify('+'.PASSWORD_PERIOD_DAY.' days');

    //If the password has expired, transition to the password change screen
    if($dtmPassUpDate->format('Y/m/d') < date("Y/m/d")){
        $_SESSION['csrf'] =fncGetCsrfToken();
        $strCsrf = $_SESSION['csrf'];
        ?>
        <form action="password_chg.php" method="post" name="passchange">
            <input type="hidden" name="user_no" value="<?php echo $arrUserData[0]['user_no']?>">
            <input type="hidden" name="X-CSRF-TOKEN" value="<?php echo $strCsrf;?>">
        </form>
        <script>document.passchange.submit();</script>
        <?php
        exit;
    }

    //Generate new session information
    session_regenerate_id(true);

    //Store login user information in session
    $objLoginUserInfo = new clsLoginUserInfo();
    $objLoginUserInfo->intUserNo = $arrUserData[0]['user_no'];
    $objLoginUserInfo->strUserID = $arrUserData[0]['user_id'];
    $objLoginUserInfo->intLanguageType = $arrUserData[0]['language_type'];
    $objLoginUserInfo->intCompanyNo = $arrUserData[0]['company_no'];
    //check language type to set name follow by language
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


//Log contents
    $strLog = DISPLAY_NAME.'（ユーザID = '.$objLoginUserInfo->strUserID;
    $strLog .= ', IPアドレス =' .$_SERVER["REMOTE_ADDR"] .' ） ';
    $strLog .= $_SERVER["HTTP_USER_AGENT"];
//Log write
    fncWriteLog(LogLevel['Info'] , LogPattern['Login'], $strLog);

//Transition to portal screen
    header('Location: info_src/portal.php');
    exit();
}


/**
 * check error when call Rakunin Service
 *
 * @create 2020/04/15 KBS Tam.nv
 * @update
 * @param $intCodeError user_id
 * @param $intType input password
 * @return string error
 */
function fncErrorSendSMS($intCodeError,$intType){
    switch ($intCodeError)
    {
        case 203:
            //Transmission failed because the user ID is locked
            $strError = LOGIN_MSG_008_JPN.' / '.LOGIN_MSG_008_ENG;
            break;
        case 506:
            //Transmission failed because the mobile phone number is not linked to the user ID
            $strError = LOGIN_MSG_009_JPN.' / '.LOGIN_MSG_009_ENG;
            break;

        case 206:
            //Transmission failed because the application is not linked to the user ID
            $strError = LOGIN_MSG_010_JPN.' / '.LOGIN_MSG_010_ENG;
            break;

        case 7:
            //The transmission failed because the type of application
            // authentication method "SMS authentication" is not set for the user ID
            $strError = LOGIN_MSG_011_JPN.' / '.LOGIN_MSG_011_ENG;
            break;
        case 602:
            //Failed to send SMS verification code. Please wait and try again.
            $strError = LOGIN_MSG_016_JPN.' / '.LOGIN_MSG_016_ENG;
            break;


        default:
            //service type: 1 - check user info, 4 send OTP, 6 OTP auth
            if($intType == 1){
                //The target user has not been registered
                $strError = LOGIN_MSG_007_JPN.' / '.LOGIN_MSG_007_ENG;
            }else if($intType == 4){
                //Failed to send OTP authentication password
                $strError = LOGIN_MSG_012_JPN.' / '.LOGIN_MSG_012_ENG;
            }else{
                //Password for Rakunin value and OTP authentication did not match
                //Type = 6
                $strError = LOGIN_MSG_013_JPN.' / '.LOGIN_MSG_013_ENG;
            }
            break;
    }
    // check if status different 602 and type different 6 will write log message
    if($intCodeError !== 602 && $intType != 6){
            $strError .= '<br>'.LOGIN_MSG_014_JPN.' / '.LOGIN_MSG_014_ENG.' <br>'.OTP_ERROR_MAILADDRESS;
    }
    // with type different 6 will return to login_otp.php. if type 6 will return error only
    if($intType != 6){
        $_SESSION['LOGIN_OTP_ERROR'] = $strError;
        header('Location: login.php');
        exit;
    }else{
        return $strError;
    }
}


/**
 * Call Reaknin service
 *
 * @create 2020/04/15 KBS Tam.nv
 * @update
 * @param $strUserID user_id
 * @param $strPassword input password
 * @param $test_type type of service
 * @param $otp string otp
 * @param $phone_sn string phone numb?
 * @return boolean
 */

function fncRakuService($strUserID, $strPassword,$test_type,$otp = NULL,$phone_sn = ''){
    $server_url = "https://auth.rakunin.co.jp";
    $app_id = RAKUNIN_APP_ID;
    $app_secret = RAKUNIN_APP_SECRET;
    $retry = 0;
    $user_name = $strUserID;

    $cloudentifysdk = new cloudentifysdk();
    $ret = $cloudentifysdk->init($server_url, $app_id, $app_secret);
    switch ($test_type)
    {
        case 1:
            //is_user_exist
            $ret = $cloudentifysdk->is_user_exist($user_name);
            break;
        case 2:
            //add_user
            $ret = $cloudentifysdk->add_user($user_name);
            break;

        case 3:
            //add_user_phone
            $ret = $cloudentifysdk->add_user_phone($user_name, $phone_sn);
            break;

        case 4:
            //send_smsotp
            $ret = $cloudentifysdk->send_smsotp($user_name, $phone_sn);
            break;
        case 5:
            //get_smsotp
            $ret = $cloudentifysdk->get_smsotp($user_name, $smsotp);
            break;

        case 6:
            //user_otp_auth
            $ret = $cloudentifysdk->user_otp_auth($user_name, $otp, $retry);
            break;

        case 7:
            //sms_push_auth
            $ret = $cloudentifysdk->sms_push_auth($user_name, $phone_sn);
            break;
        default:
            echo "<br>Error test type!<br>";
            break;
    }

    $cloudentifysdk->uninit();
    // check status, if status different 0 is error. we will write error to log.
    if($ret){
        //Log contents
        if($otp){
            $strLogRakuninOTP = ' ,OTP = '.$otp;
        }
        $strLogRakunin = '（ユーザID = '.$strUserID;
        $strLogRakunin .= ' , パスワード = '.$strPassword;
        $strLogRakunin .= @$strLogRakuninOTP;
        $strLogRakunin .= ' , らく認エラーコード = '.$ret;
        $strLogRakunin .= ', IPアドレス =' .$_SERVER["REMOTE_ADDR"] .' ） ';
        $strLogRakunin .= $_SERVER["HTTP_USER_AGENT"];
        // check type
        if($test_type == 1){
            fncWriteLog(LogLevel['Error'], LogPattern['Error'], DISPLAY_NAME.' らく認ユーザーエラー '.$strLogRakunin);
        }else if($test_type == 4){
            fncWriteLog(LogLevel['Error'], LogPattern['Error'], DISPLAY_NAME.' OTP送信エラー '.$strLogRakunin);
        }else{
            fncWriteLog(LogLevel['Error'], LogPattern['Error'], DISPLAY_NAME.' OTP認証エラー  '.$strLogRakunin);
        }
    }

    return $ret;
}

?>
