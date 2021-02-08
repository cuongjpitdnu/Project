<?php
header("Content-type:text/html;charset=utf-8");

require_once('cloudentifysdk.php');


echo"Test type: <br>";
echo"   1   check user exist or not by user's name.<br>";
echo"   2   add user.<br><br>";

echo"   3   bind user with one token.<br><br>";

echo"   4   auth otp by user's name.<br>";
echo"   5   auth otp by token number.<br>";
echo"   6   sync otp by user's name.<br>";
echo"   7   sync otp by token number.<br><br>";

echo"   8   disable one token.<br>";
echo"   9   enable one token.<br>";
echo"   10  lock one token.<br>";
echo"   11  unlock one token.<br>";
echo"   12  unbind one token of user.<br><br>";


$server_url = "https://auth.rakunin.co.jp";
$app_id = "4C73655CF4A9BCE79CA4";
$app_secret = "D4041B4B105301A8B3B079790DD6118E6542440B";

$user_name = "test";
$token_sn = "1000000031";

$otp = "702686";
$retry = 0;
$next_otp = "123456";

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
    case 9:
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
    case 10:
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
    case 11:
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
    case 12:
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

    default:
        echo "<br>Error test type!<br>";
        break;
}

$cloudentifysdk->uninit();

?>
