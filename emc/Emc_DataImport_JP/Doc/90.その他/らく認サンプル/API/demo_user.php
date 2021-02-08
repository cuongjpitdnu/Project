<?php
header("Content-type:text/html;charset=utf-8");

require_once('cloudentifysdk.php');


echo"Test type: <br>";
echo"   1   check user exist or not by user's name.<br>";
echo"   2   add user.<br><br>";

echo"   3   disable one user.<br>";
echo"   4   enable one user.<br>";
echo"   5   lock user.<br>";
echo"   6   unlock user.<br><br>";

echo"   7  check user information(if user is not exist, return new user auth policy).<br><br>";


$server_url = "https://auth.rakunin.co.jp";
$app_id = "4C73655CF4A9BCE79CA4";
$app_secret = "D4041B4B105301A8B3B079790DD6118E6542440B";

$user_name = "test";

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
    case 4:
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
    case 5:
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
    case 6:
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

    case 7:
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

    default:
        echo "<br>Error test type!<br>";
        break;
}

$cloudentifysdk->uninit();

?>
