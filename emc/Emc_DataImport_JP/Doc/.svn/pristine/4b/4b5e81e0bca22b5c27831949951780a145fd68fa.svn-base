<?php
header("Content-type:text/html;charset=utf-8");

require_once('cloudentifysdk.php');



// $face_regist_data = '{"face":[{"authid":"",' . '"face_data":"' . 'face+data+test' . '"}],' . '"result":"' . '1' . '"}';
// $data_encode = urlencode($face_regist_data);
// echo"data before encode: " . $face_regist_data . "<br>";
// echo"data after  encode: " . $data_encode . "<br>";

// $face_auth_data = '{"face_s":"' . 0 . '",' . '"face_data":"' . 'face+data+test' . '",' . '"result":"' . '1' . '"}';
// $data_encode = urlencode($face_auth_data);
// echo"data before encode: " . $face_auth_data . "<br>";
// echo"data after  encode: " . $data_encode . "<br>";
// return;

// $version = '{"version":"","os":0}';
// $data_encode = urlencode($version);
// echo"data before encode: " . $version . "<br>";
// echo"data after  encode: " . $data_encode . "<br>";
// return;



echo"Test type: <br>";
echo"   1   check user exist or not by user's name.<br>";
echo"   2   add user.<br>";

echo"   3   bind user with one token.<br><br>";

echo"   4   auth otp by user's name.<br>";
echo"   5   auth otp by token number.<br>";
echo"   6   sync otp by user's name.<br>";
echo"   7   sync otp by token number.<br><br>";

echo"   8   disable one user.<br>";
echo"   9   enable one user.<br>";
echo"   10  lock user.<br>";
echo"   11  unlock user.<br><br>";

echo"   12  disable one token.<br>";
echo"   13  enable one token.<br>";
echo"   14  lock one token.<br>";
echo"   15  unlock one token.<br>";
echo"   16  unbind one token of user.<br><br>";

echo"   17  check user information(if user is not exist, return new user auth policy).<br><br>";

echo"   18  add user's phone number.<br>";
echo"   19  check mobile token is actived or not.<br>";
echo"   20  active mobile token.<br><br>";

echo"   21  auth otp by user's name and phone number(mobile token).<br><br>";

echo"   22  push auth by user's name(mobile token).<br><br>";

echo"   23  push auth with user's voice(mobile token).<br><br>";

echo"   24  get QR code for scan auth(mobile token).<br>";
echo"   25  scan auth by scan QR code(mobile token).<br><br>";

echo"   26  get register QR code for active face token.<br>";
echo"   27  get the active state of user's face token.<br>";
echo"   28  push auth with user's face(face token).<br><br>";

echo"   29  send SMS OTP to user's phone by user's name and user's phone number.<br>";
echo"   30  get SMS OTP by user's name.<br>";
echo"   31  SMS push auth.<br><br>";

echo"   32  check wechat token is actived or not.<br>";
echo"   33  get wechat token active code.<br>";
echo"   34  active wechat token.<br><br>";

echo"   35  push auth by user's name(wechat token).<br><br>";

echo"   36 push auth with user's voice(wechat token).<br><br>";

echo"   37  get QR code for scan auth(wechat token).<br>";
echo"   38  scan auth by scan QR code(wechat token).<br><br>";

echo"   39  send response for push auth(wechat token).<br>";
echo"   40  send response for voice push auth(wechat token).<br>";
echo"   41  send response for scan auth(wechat token).<br><br>";

echo"   42  get user's information(wechat token).<br>";
echo"   43  get wechat OTP by user's name(wechat token).<br><br>";


echo"   44  face engine regiest .<br>";
echo"   45  face engine auth.<br>";
echo"   46  get face process result.<br><br>";

echo"   47  get face auth id from cloudentify server.<br>";
echo"   48  face register to cloudentify server.<br>";
echo"   49  face auth to cloudentify server.<br><br>";

echo"   50  get mobile token's activedata.<br><br>";


$server_url = "https://auth.rakunin.co.jp";
$app_id = "4C73655CF4A9BCE79CA4";
$app_secret = "D4041B4B105301A8B3B079790DD6118E6542440B";

$user_name = "test";
$token_sn = "1000000031";

$otp = "702686";
$retry = 0;
$next_otp = "123456";

$req_id = "test_scan_auth_input_req_id";

$openid = "test_wechat_token_openid";

$result = 1;

$qr_code = "test_scan_auth_input_qr_code";

$phone_sn = '13012345678';

$face_regist_data = "%7B%22face%22%3A%5B%7B%22authid%22%3A%22%22%2C%22face_data%22%3A%22face%2Bdata%2Btest%22%7D%5D%2C%22result%22%3A%221%22%7D";
$face_auth_data = "%7B%22face_s%22%3A%220%22%2C%22face_data%22%3A%22face%2Bdata%2Btest%22%2C%22result%22%3A%221%22%7D";

$version = urlencode('{"version":"4.0","os":1}');
$activedata = '';

$ret = OTP_OK;


$test_type = 1;
echo "<br>current test type: ", $test_type, "<br>";

$err_msg = "";

$cloudentifysdk = new cloudentifysdk();

$ret = $cloudentifysdk->init($server_url, $app_id, $app_secret);
if (OTP_OK == $ret)
{
    echo "<br>init() ok.<br>";
}
else
{
    echo "<br>init() err, ret: ", $ret, "<br>";
    $cloudentifysdk->get_err_msg($ret, $err_msg);
    echo "<br>err_msg: ", $err_msg, "<br>";
    return $ret;
}

switch ($test_type)
{
    case 1:
        $ret = $cloudentifysdk->is_user_exist($user_name);
        if (OTP_OK == $ret)
        {
            echo "<br>is_user_exist() ok.<br>";
        }
        else
        {
            echo "<br>is_user_exist() err, ret: ", $ret, "<br>";
        }
        break;
    case 2:
        $ret = $cloudentifysdk->add_user($user_name);
        if (OTP_OK == $ret)
        {
            echo "<br>add_user() ok.<br>";
        }
        else
        {
            echo "<br>add_user() err, ret: ", $ret, "<br>";
        }
        break;

    case 3:
        $ret = $cloudentifysdk->bind_token($user_name, $token_sn);
        if (OTP_OK == $ret)
        {
            echo "<br>bind_token() ok.<br>";
        }
        else
        {
            echo "<br>bind_token() err, ret: ", $ret, "<br>";
        }
        break;

    case 4:
        $ret = $cloudentifysdk->user_otp_auth($user_name, $otp, $retry);
        if (OTP_OK == $ret)
        {
            echo "<br>user_otp_auth() ok.<br>";
        }
        else
        {
            echo "<br>user_otp_auth() err, ret: ", $ret, ", retry: ", $retry, "<br>";
        }
        break;
    case 5:
        $ret = $cloudentifysdk->token_otp_auth($token_sn, $otp, $retry);
        if (OTP_OK == $ret)
        {
            echo "<br>token_otp_auth() ok.<br>";
        }
        else
        {
            echo "<br>token_otp_auth() err, ret: ", $ret, ", retry: ", $retry, "<br>";
        }
        break;
    case 6:
        $ret = $cloudentifysdk->user_otp_sync($user_name, $otp, $next_otp);
        if (OTP_OK == $ret)
        {
            echo "<br>user_otp_sync() ok.<br>";
        }
        else
        {
            echo "<br>user_otp_sync() err, ret: ", $ret, "<br>";
        }
        break;
    case 7:
        $ret = $cloudentifysdk->token_otp_sync($token_sn, $otp, $next_otp);
        if (OTP_OK == $ret)
        {
            echo "<br>token_otp_sync() ok.<br>";
        }
        else
        {
            echo "<br>token_otp_sync() err, ret: ", $ret, "<br>";
        }
        break;

    case 8:
        $ret = $cloudentifysdk->disable_user($user_name);
        if (OTP_OK == $ret)
        {
            echo "<br>disable_user() ok.<br>";
        }
        else
        {
            echo "<br>disable_user() err, ret: ", $ret, "<br>";
        }
        break;
    case 9:
        $ret = $cloudentifysdk->enable_user($user_name);
        if (OTP_OK == $ret)
        {
            echo "<br>enable_user() ok.<br>";
        }
        else
        {
            echo "<br>enable_user() err, ret: ", $ret, "<br>";
        }
        break;
    case 10:
        $ret = $cloudentifysdk->lock_user($user_name);
        if (OTP_OK == $ret)
        {
            echo "<br>lock_user() ok.<br>";
        }
        else
        {
            echo "<br>lock_user() err, ret: ", $ret, "<br>";
        }
        break;
    case 11:
        $ret = $cloudentifysdk->unlock_user($user_name);
        if (OTP_OK == $ret)
        {
            echo "<br>unlock_user() ok.<br>";
        }
        else
        {
            echo "<br>unlock_user() err, ret: ", $ret, "<br>";
        }
        break;

    case 12:
        $ret = $cloudentifysdk->disable_token($token_sn);
        if (OTP_OK == $ret)
        {
            echo "<br>disable_token() ok.<br>";
        }
        else
        {
            echo "<br>disable_token() err, ret: ", $ret, "<br>";
        }
        break;
    case 13:
        $ret = $cloudentifysdk->enable_token($token_sn);
        if (OTP_OK == $ret)
        {
            echo "<br>enable_token() ok.<br>";
        }
        else
        {
            echo "<br>enable_token() err, ret: ", $ret, "<br>";
        }
        break;
    case 14:
        $ret = $cloudentifysdk->lock_token($token_sn);
        if (OTP_OK == $ret)
        {
            echo "<br>lock_token() ok.<br>";
        }
        else
        {
            echo "<br>lock_token() err, ret: ", $ret, "<br>";
        }
        break;
    case 15:
        $ret = $cloudentifysdk->unlock_token($token_sn);
        if (OTP_OK == $ret)
        {
            echo "<br>unlock_token() ok.<br>";
        }
        else
        {
            echo "<br>unlock_token() err, ret: ", $ret, "<br>";
        }
        break;
    case 16:
        $ret = $cloudentifysdk->unbind_token($user_name, $token_sn);
        if (OTP_OK == $ret)
        {
            echo "<br>unbind_token() ok.<br>";
        }
        else
        {
            echo "<br>unbind_token() err, ret: ", $ret, "<br>";
        }
        break;

    case 17:
        $ret = $cloudentifysdk->check_user($user_name, $user_policy, $newuser_policy);
        if (OTP_OK == $ret)
        {
            echo "<br>get_user_policy() ok, user_policy: ", $user_policy, "<br>";
        }
        else
        {
            echo "<br>get_user_policy() err, ret: ", $ret, ", newuser_policy: ", $newuser_policy, "<br>";
        }
        break;

    case 18:
        $ret = $cloudentifysdk->add_user_phone($user_name, $phone_sn);
        if (OTP_OK == $ret)
        {
            echo "<br>add_user_phone() ok.<br>";
        }
        else
        {
            echo "<br>add_user_phone() err, ret: ", $ret, "<br>";
        }
        break;
    case 19:
        $ret = $cloudentifysdk->is_mobile_token_actived($user_name, $phone_sn);
        if (OTP_OK == $ret)
        {
            echo "<br>is_mobile_token_actived() ok.<br>";
        }
        else
        {
            echo "<br>is_mobile_token_actived() err, ret: ", $ret, "<br>";
        }
        break;
    case 20:
        $ret = $cloudentifysdk->active_mobile_token($user_name, $phone_sn, $active_code, $auth_code);
        if (OTP_OK == $ret)
        {
            echo "<br>active_mobile_token() ok.<br>";
            echo "active_code: ", $active_code, "<br>";
            echo " please transfer QR for show.<br>";
            if (!empty($auth_code))
            {
                echo "auth_code: ", $auth_code, "<br>";
                echo "this token is offline actived, please input app after scan QR.<br>";
            }
        }
        else
        {
            echo "<br>active_mobile_token() err, ret: ", $ret, "<br>";
        }
        break;

    case 21:
        $ret = $cloudentifysdk->user_otp_auth_by_mobile($user_name, $otp, $phone_sn, $retry);
        if (OTP_OK == $ret)
        {
            echo "<br>user_otp_auth_by_mobile() ok.<br>";
        }
        else
        {
            echo "<br>user_otp_auth_by_mobile() err, ret: ", $ret, ", retry: ", $retry, "<br>";
        }
        break;

    case 22:
        $ret = $cloudentifysdk->push_auth($user_name, $phone_sn);
        if (OTP_OK == $ret)
        {
            echo "<br>push_auth() ok.<br>";
        }
        else
        {
            echo "<br>push_auth() err, ret: ", $ret, "<br>";
        }
        break;

    case 23:
        $ret = $cloudentifysdk->voice_auth($user_name);
        if (OTP_OK == $ret)
        {
            echo "<br>voice_auth() ok.<br>";
        }
        else
        {
            echo "<br>voice_auth() err, ret: ", $ret, "<br>";
        }
        break;

    case 24:
    case 37:
        $ret = $cloudentifysdk->get_qrcode($user_name, $qr_code, $req_id);
        if (OTP_OK == $ret)
        {
            echo "<br>get_qrcode() ok <br>";
            echo "qr_code: ", $qr_code, "<br>";
            echo "please transfer QR for show.<br>";
            echo "req_id: ", $req_id, "<br>";
            echo "please call scan_auth() with req_id.<br>";
        }
        else
        {
            echo "<br>get_qrcode() err, ret: ", $ret, "<br>";
        }
        break;
    case 25:
    case 38:
        $ret = $cloudentifysdk->scan_auth($req_id);
        if (OTP_OK == $ret)
        {
            echo "<br>scan_auth() ok.<br>";
        }
        else
        {
            echo "<br>scan_auth() err, ret: ", $ret, "<br>";
        }
        break;

    case 26:
        $ret = $cloudentifysdk->get_face_regist_qrcode($user_name, $phone_sn, $reg_qr_code);
        if (OTP_OK == $ret)
        {
            echo "<br>get_face_regist_qrcode() ok <br>";
            echo "reg_qr_code: ", $reg_qr_code, "<br>";
            echo "please transfer QR for show.<br>";
        }
        else
        {
            echo "<br>get_face_regist_qrcode() err, ret: ", $ret, "<br>";
        }
        break;
    case 27:
        $ret = $cloudentifysdk->get_face_regist_result($user_name, $phone_sn);
        if (OTP_OK == $ret)
        {
            echo "<br>get_face_regist_result() ok <br>";
        }
        else
        {
            echo "<br>get_face_regist_result() err, ret: ", $ret, "<br>";
        }
        break;
    case 28:
        $ret = $cloudentifysdk->face_push_auth($user_name);
        if (OTP_OK == $ret)
        {
            echo "<br>face_push_auth() ok.<br>";
        }
        else
        {
            echo "<br>face_push_auth() err, ret: ", $ret, "<br>";
        }
        break;

    case 29:
        $ret = $cloudentifysdk->send_smsotp($user_name, $phone_sn);
        if (OTP_OK == $ret)
        {
            echo "<br>send_smsotp() ok.<br>";
        }
        else
        {
            echo "<br>send_smsotp() err, ret: ", $ret, "<br>";
        }
        break;
    case 30:
        $ret = $cloudentifysdk->get_smsotp($user_name, $smsotp);
        if (OTP_OK == $ret)
        {
            echo "<br>get_smsotp() ok, SMS OTP : ", $smsotp, "<br>";
        }
        else
        {
            echo "<br>get_smsotp() err, ret: ", $ret, "<br>";
        }
        break;
    case 31:
        $ret = $cloudentifysdk->sms_push_auth($user_name, $phone_sn);
        if (OTP_OK == $ret)
        {
            echo "<br>sms_push_auth() ok.<br>";
        }
        else
        {
            echo "<br>sms_push_auth() err, ret: ", $ret, "<br>";
        }
        break;

   case 32:
        $ret = $cloudentifysdk->is_wechat_token_actived($user_name);
        if (OTP_OK == $ret)
        {
            echo "<br>is_wechat_token_actived() ok.<br>";
        }
        else
        {
            echo "<br>is_wechat_token_actived() err, ret: ", $ret, "<br>";
        }
        break;
    case 33:
        $ret = $cloudentifysdk->get_wechat_token_ac($user_name, $wechat_ac);
        if (OTP_OK == $ret)
        {
            echo "<br>get_wechat_token_ac() ok.<br>";
            echo "wechat_ac: ", $wechat_ac, "<br>";
            echo "please transfer QR for show.<br>";
        }
        else
        {
            echo "<br>get_wechat_token_ac() err, ret: ", $ret, "<br>";
        }
        break;
    case 34:
        $ret = $cloudentifysdk->active_wechat_token($openid, $wechat_ac);
        if (OTP_OK == $ret)
        {
            echo "<br>active_wechat_token() ok.<br>";
        }
        else
        {
            echo "<br>active_wechat_token() err, ret: ", $ret, "<br>";
        }
        break;

    case 35:
        $ret = $cloudentifysdk->wechat_push_auth($user_name);
        if (OTP_OK == $ret)
        {
            echo "<br>wechat_push_auth() ok.<br>";
        }
        else
        {
            echo "<br>wechat_push_auth() err, ret: ", $ret, "<br>";
        }
        break;

    case 36:
        $ret = $cloudentifysdk->wechat_voice_auth($user_name);
        if (OTP_OK == $ret)
        {
            echo "<br>wechat_voice_auth() ok.<br>";
        }
        else
        {
            echo "<br>wechat_voice_auth() err, ret: ", $ret, "<br>";
        }
        break;

    //case 37: //case 24:
    //case 38: //case 25:

    case 39:
        $ret = $cloudentifysdk->wechat_push_auth_response($req_id, $openid, $result);
        if (OTP_OK == $ret)
        {
            echo "<br>wechat_push_auth_response() ok.<br>";
        }
        else
        {
            echo "<br>wechat_push_auth_response() err, ret: ", $ret, "<br>";
        }
        break;
    case 40:
        $ret = $cloudentifysdk->wechat_voice_auth_response($req_id, $openid, $result);
        if (OTP_OK == $ret)
        {
            echo "<br>wechat_voice_auth_response() ok.<br>";
        }
        else
        {
            echo "<br>wechat_voice_auth_response() err, ret: ", $ret, "<br>";
        }
        break;
    case 41:
        $ret = $cloudentifysdk->get_wechat_scan_auth_info($openid, $qr_code, $user_name, $app_name, $req_id);
        if (OTP_OK == $ret)
        {
            echo "<br>get_wechat_scan_auth_info() ok.<br>";
            $ret = $cloudentifysdk->wechat_scan_auth_response($req_id, $user_name, $result);
            if (OTP_OK == $ret)
            {
                echo "<br>wechat_scan_auth_response() ok.<br>";
            }
            else
            {
                echo "<br>wechat_scan_auth_response() err, ret: ", $ret, "<br>";
            }
        }
        else
        {
            echo "<br>get_wechat_scan_auth_info() err, ret: ", $ret, "<br>";
        }
        break;

    case 42:
        $ret = $cloudentifysdk->get_userinfo_by_wechat($openid, $user_name, $token_sn, $company_name);
        if (OTP_OK == $ret)
        {
            echo "<br>get_userinfo_by_wechat() ok <br>";
            echo "user_name: ", $user_name, "<br>";
            echo "token_sn: ", $token_sn, "<br>";
            echo "company_name: ", $company_name, "<br>";
        }
        else
        {
            echo "<br>get_userinfo_by_wechat() err, ret: ", $ret, "<br>";
        }
        break;
    case 43:
        $ret = $cloudentifysdk->get_wechat_otp($openid, $wechat_otp);
        if (OTP_OK == $ret)
        {
            echo "<br>get_wechat_otp() ok, wechat OTP : ", $wechat_otp, "<br>";
        }
        else
        {
            echo "<br>get_wechat_otp() err, ret: ", $ret, "<br>";
        }
        break;

    case 44:
        $ret = $cloudentifysdk->face_engine_regist($user_name);
        if (OTP_OK == $ret)
        {
            echo "<br>face_engine_regist() ok<br>";
        }
        else
        {
            echo "<br>face_engine_regist() err, ret: ", $ret, "<br>";
        }
        break;
    case 45:
        $ret = $cloudentifysdk->face_engine_auth($user_name);
        if (OTP_OK == $ret)
        {
            echo "<br>face_engine_auth() ok<br>";
        }
        else
        {
            echo "<br>face_engine_auth() err, ret: ", $ret, "<br>";
        }
        break;
    case 46:
        $ret = $cloudentifysdk->get_process_result($user_name);
        if (OTP_OK == $ret)
        {
            echo "<br>get_process_result() ok <br>";
        }
        else
        {
            echo "<br>get_process_result() err, ret: ", $ret, "<br>";
        }
        break;
    case 47:
        $ret = $cloudentifysdk->get_face_authid($user_name, $authid);
        if (OTP_OK == $ret)
        {
            echo "<br>get_face_authid() ok, authid : ", $authid, "<br>";
        }
        else
        {
            echo "<br>get_face_authid() err, ret: ", $ret, "<br>";
        }
        break;
    case 48:
        $ret = $cloudentifysdk->face_regist($user_name, $phone_sn, $face_regist_data);
        if (OTP_OK == $ret)
        {
            echo "<br>face_regist() ok <br>";
        }
        else
        {
            echo "<br>face_regist() err, ret: ", $ret, "<br>";
        }
        break;
    case 49:
        $ret = $cloudentifysdk->face_auth($user_name, $face_auth_data);
        if (OTP_OK == $ret)
        {
            echo "<br>face_auth() ok <br>";
        }
        else
        {
            echo "<br>face_auth() err, ret: ", $ret, "<br>";
        }
        break;
    case 50:
        $ret = $cloudentifysdk->get_mobile_token_activedata($user_name, $phone_sn, $version, $activedata);
        if (OTP_OK == $ret)
        {
            echo "<br>get_mobile_token_activedata() ok. activedata: ", $activedata, "<br>";
        }
        else
        {
            echo "<br>get_mobile_token_activedata() err, ret: ", $ret, "<br>";
        }
        break;

    default:
        echo "<br>Error test type!<br>";
        break;
}

$cloudentifysdk->get_err_msg($ret, $err_msg);
echo "<br>err_msg: ", $err_msg, "<br>";

$cloudentifysdk->uninit();

?>
