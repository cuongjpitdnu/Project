<?php
header("Content-type:text/html;charset=utf-8");

require_once('cloudentifysdk.php');


echo"Test type: <br>";
echo"   1   check user exist or not by user's name.<br>";
echo"   2   add user.<br><br>";

echo"   3   check wechat token is actived or not.<br>";
echo"   4   get wechat token active code.<br>";
echo"   5   active wechat token.<br><br>";

echo"   6   push auth by user's name(wechat token).<br><br>";

echo"   7   push auth with user's voice(wechat token).<br><br>";

echo"   8   get QR code for scan auth(wechat token).<br>";
echo"   9   scan auth by scan QR code(wechat token).<br><br>";

echo"   10  send response for push auth(wechat token).<br>";
echo"   11  send response for voice push auth(wechat token).<br>";
echo"   12  send response for scan auth(wechat token).<br><br>";

echo"   13  get user's information(wechat token).<br>";
echo"   14  get wechat OTP by user's name(wechat token).<br><br>";


$server_url = "https://api-testingdfserv.cloudentify.com";
$app_id = "B31BDD89789C382E7D03";
$app_secret = "AE8E18498834B540153382EEE8BB1F9BA28B5C12";

$user_name = "test";

$otp = "702686";
$retry = 0;


if (isset($_GET['wechat_ac']))
{
    $wechat_ac = $_GET['wechat_ac'];
}
else
{
    $wechat_ac = "wechat_ac_test";
}

if (isset($_GET['req_id']))
{
    $req_id = $_GET['req_id'];
}
else
{
    $req_id = "test_scan_auth_input_req_id";
}

$openid = "oHeGAs1TvDx2n1sOgj3s3bDQR2EU";

$ret = OTP_OK;


if (isset($_GET['test_type']))
{
    $test_type = $_GET['test_type'];
}
else
{
    echo "<br>invalid test type ! <br>";
    return -1;
}
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
    case 4:
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
    case 5:
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

    case 6:
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

    case 7:
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

    case 8:
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
    case 9:
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

    case 10:
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
    case 11:
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
    case 12:
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

    case 13:
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
    case 14:
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

    default:
        echo "<br>Error test type!<br>";
        break;
}

$cloudentifysdk->uninit();

?>
