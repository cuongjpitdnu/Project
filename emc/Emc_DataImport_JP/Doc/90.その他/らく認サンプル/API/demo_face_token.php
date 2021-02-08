<?php
header("Content-type:text/html;charset=utf-8");

require_once('cloudentifysdk.php');



// $face_regist_data = '{"face":[{"authid:"",' . '"face_data":"' . 'face+data+test' . '"}],' . '"result":"' . '1' . '"}';
// $data_encode = urlencode($face_regist_data);
// echo"data before encode: " . $face_regist_data . "<br>";
// echo"data after  encode: " . $data_encode . "<br>";

// $face_auth_data = '{"face_s":"' . 0 . '",' . '"face_data":"' . 'face+data+test' . '",' . '"result":"' . '1' . '"}';
// $data_encode = urlencode($face_auth_data);
// echo"data before encode: " . $face_auth_data . "<br>";
// echo"data after  encode: " . $data_encode . "<br>";
// return;



echo"Test type: <br>";
echo"   1   check user exist or not by user's name.<br>";
echo"   2   add user.<br><br>";

echo"   3   add user's phone number.<br><br>";

echo"   4   get register QR code for active face token.<br>";
echo"   5   get the active state of user's face token.<br>";
echo"   6   push auth with user's face(face token).<br><br>";

echo"   7   face engine register user.<br>";
echo"   8   face engine user auth.<br>";
echo"   9   face engine get auth result.<br><br>";

echo"   10  get face auth id from cloudentify server.<br>";
echo"   11  face register to cloudentify server.<br>";
echo"   12  face auth to cloudentify server.<br><br>";


$server_url = "https://auth.rakunin.co.jp";
$app_id = "4C73655CF4A9BCE79CA4";
$app_secret = "D4041B4B105301A8B3B079790DD6118E6542440B";

$user_name = "test";

$otp = "702686";
$retry = 0;

$req_id = "test_scan_auth_input_req_id";

$phone_sn = '13012345678';

$face_regist_data = "%7B%22face%22%3A%5B%7B%22authid%3A%22%22%2C%22face_data%22%3A%22face%2Bdata%2Btest%22%7D%5D%2C%22result%22%3A%221%22%7D";
$face_auth_data = "%7B%22face_s%22%3A%220%22%2C%22face_data%22%3A%22face%2Bdata%2Btest%22%2C%22result%22%3A%221%22%7D";

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
    case 5:
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
    case 6:
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

    case 7:
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
    case 8:
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
    case 9:
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
    case 10:
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
    case 11:
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
    case 12:
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

    default:
        echo "<br>Error test type!<br>";
        break;
}

$cloudentifysdk->uninit();

?>
