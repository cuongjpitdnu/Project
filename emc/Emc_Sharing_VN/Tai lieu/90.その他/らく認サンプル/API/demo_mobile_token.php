<?php
header("Content-type:text/html;charset=utf-8");

require_once('cloudentifysdk.php');


echo"Test type: <br>";
echo"   1   check user exist or not by user's name.<br>";
echo"   2   add user.<br><br>";

echo"   3   add user's phone number.<br>";
echo"   4   check mobile token is actived or not.<br>";
echo"   5   active mobile token.<br><br>";

echo"   6   push auth by user's name(mobile token).<br><br>";

echo"   7   get QR code for scan auth(mobile token).<br>";
echo"   8   scan auth by scan QR code(mobile token).<br><br>";

echo"   9   push auth with user's voice(mobile token).<br><br>";

echo"   10  auth otp by user's name.<br>";
echo"   11  auth otp by user's name and phone number(mobile token).<br><br>";

echo"   12  get mobile token's activedata.<br><br>";


$server_url = "https://auth.rakunin.co.jp";
$app_id = "4C73655CF4A9BCE79CA4";
$app_secret = "D4041B4B105301A8B3B079790DD6118E6542440B";

$user_name = "test";
$token_sn = "1000000031";

$otp = "702686";
$retry = 0;
$next_otp = "123456";

$req_id = "test_scan_auth_input_req_id";

$phone_sn = '13012345678';

$version = urlencode('{"version":"4.0","os":1}');

$ret = OTP_OK;


$test_type = 1;
echo "<br>current test type: ", $test_type, "<br>";


$cloudentifysdk = new cloudentifysdk();

$ret = $cloudentifysdk->init($server_url, $app_id, $app_secret);
if (OTP_OK == $ret)
{
    echo "<br>init() ok.<br>";
}
else
{
    echo "<br>init() err, ret: ", $ret, "<br>";
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
    case 4:
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
    case 5:
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

    case 6:
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

    case 7:
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
    case 8:
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
        
    case 9:
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
        
    case 10:
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
    case 11:
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
    case 12:
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

$cloudentifysdk->uninit();

?>
