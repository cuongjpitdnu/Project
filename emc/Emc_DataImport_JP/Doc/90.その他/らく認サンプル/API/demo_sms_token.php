<?php
header("Content-type:text/html;charset=utf-8");

require_once('cloudentifysdk.php');


echo"Test type: <br>";
echo"   1   check user exist or not by user's name.<br>";
echo"   2   add user.<br><br>";

echo"   3   add user's phone number.<br><br>";

echo"   4   send SMS OTP to user's phone by user's name and user's phone number.<br>";
echo"   5   get SMS OTP by user's name.<br><br>";

echo"   6   auth otp by user's name.<br><br>";

echo"   7   SMS push auth.<br><br>";


$server_url = "https://auth.rakunin.co.jp";
$app_id = "4C73655CF4A9BCE79CA4";
$app_secret = "D4041B4B105301A8B3B079790DD6118E6542440B";

$user_name = "test";

$otp = "702686";
$retry = 0;

$phone_sn = '13012345678';

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
    case 5:
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

    case 6:
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
        
    case 7:
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
    default:
        echo "<br>Error test type!<br>";
        break;
}

$cloudentifysdk->uninit();

?>
