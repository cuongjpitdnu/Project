<?php
require_once('otpcode.php');
require_once('mybinhex.php');
require_once('mypubkey.php');
require_once('myaes.php');


//http会话超时时间
define ('HTTP_TIME_OUT', 60);

//error_reporting(0);
//set_time_limit(90);

class cloudentifysdk 
{
    function __construct()
    {
        //默认应用类型(SDK) 4
        $this->app_type = 4;
        $this->refresh_token = '';

        //2进制16进制
        $this->mybinhex = new mybinhex();
        //产生密钥
        $this->mypubkey = new mypubkey();
        //加解密
        $this->myaes = new myaes();
        $this->myaes->require_pkcs5();

        //curl_init
        $this->curl = curl_init();
    }

    function __destruct()
    {
        //curl_close
        curl_close($this->curl);
    }

    /*
    名字： 写日志 sdk内部使用
    参数： 
        $str            日志字符串
    */
    function write_log($str)
    {
        $len = 0;
        
        $fp = fopen("./cloudentifysdk_debug_log.txt", "a+");
        if (!$fp)
        {
            return -1;
        }

        $len = fwrite($fp, $str);
        if (strpos($str, "\n") == false)
        {
            $len += fwrite($fp, "\n");
        }
        
        fclose($fp);
        
        return $len;
    }

    /*
    名字： 检查返回值 sdk内部使用
    参数： 
        $curl_code      curl返回值
    */
    private function check_code($curl_code)
    {
        $code = OTP_OK;

        switch($curl_code)
        {
        case 0:     //CURLE_OK:
            $code = OTP_OK;
            break;
        case 7:     //CURLE_COULDNT_CONNECT:
            $code = OTP_HTTP_CANTCONNECT;
            break;
        case 28:    //CURLE_OPERATION_TIMEDOUT:
            $code = OTP_HTTP_TIMEOUT;
            break;
        case 22:    //CURLE_HTTP_RETURNED_ERROR:
            $code = OTP_HTTP_FAIL;
            break;
        case 6:     //CURLE_COULDNT_RESOLVE_HOST:
            $code = OTP_HTTP_CANTCONNECT;
            break;
        default :
            $code = OTP_HTTP_FAIL;
            break;
        }
        //print_r('==== ==== curl_code: ' . $curl_code . ', code: ' . $code . '<br>');
        //$this->write_log('==== ==== curl_code: ' . $curl_code . ', code: ' . $code);

        return $code;
    }

    /*
    名字： 进行http请求 sdk内部使用
    参数： 
        $request        http请求
        $resp_data      http回应数据 (out)
        $time_out       http超时时间
    */
    private function do_http($request, &$resp_data, $time_out=60)
    {
        curl_setopt($this->curl, CURLOPT_URL, $request);
        curl_setopt($this->curl, CURLOPT_HTTPGET, 1);
        curl_setopt($this->curl, CURLOPT_SSL_VERIFYPEER, 0);
        curl_setopt($this->curl, CURLOPT_FOLLOWLOCATION, 1);
        curl_setopt($this->curl, CURLOPT_TIMEOUT, $time_out);
        curl_setopt($this->curl, CURLOPT_RETURNTRANSFER, 1);   

        $resp_data = curl_exec($this->curl);
        //print_r('==== ==== resp: ' . $resp_data . '<br>');
        //$this->write_log('==== ==== resp: ' . $resp_data);

        $curl_code = curl_errno($this->curl);
        $code = $this->check_code($curl_code);

        return $code;
    }

    /*
    名字： 进行http请求 sdk内部使用
    参数： 
        $request        http请求
        $data_post      待post数据
        $resp_data      http回应数据 (out)
        $time_out       http超时时间
    */
    private function do_http_post($request, $data_post, &$resp_data, $time_out=60)
    {
        curl_setopt($this->curl, CURLOPT_URL, $request);
        curl_setopt($this->curl, CURLOPT_POST, 1);
        curl_setopt($this->curl, CURLOPT_POSTFIELDS, $data_post);
        curl_setopt($this->curl, CURLOPT_SSL_VERIFYPEER, 0);
        curl_setopt($this->curl, CURLOPT_FOLLOWLOCATION, 1);
        curl_setopt($this->curl, CURLOPT_TIMEOUT, $time_out);
        curl_setopt($this->curl, CURLOPT_RETURNTRANSFER, 1);   

        $resp_data = curl_exec($this->curl);
        //print_r('==== ==== resp: ' . $resp_data . '<br>');
        //$this->write_log('==== ==== resp: ' . $resp_data);

        $curl_code = curl_errno($this->curl);
        $code = $this->check_code($curl_code);

        return $code;
    }

    /*
    名字： 获取密文哈希 sdk内部使用
    参数： 
        $ciper          密文
        $ciper_hash_hex 密文哈希16进制 (out)
    */
    private function get_ciper_hash($ciper, &$ciper_hash_hex)
    {
        $ciper_hash_hex = sha1($this->key . $ciper);
    }

    /*
    名字： 获取数据密文 sdk内部使用
    参数： 
        $data_plain     数据明文
        $data_ciper     数据密文 (out)
    */
    private function get_data_ciper($data_plain, &$data_ciper)
    {
        //明文加密
        $data_ciper = $this->myaes->encrypt($data_plain);
    }

    /*
    名字： 获取json明文 sdk内部使用
    参数： 
        $resp_data      http回应数据
        $json_plain     json明文 (out)
    */
    private function get_json_plain($resp_data, &$json_plain)
    {
        if (null == $resp_data)
        {
            return OTP_JSON_PROCESS_FAILED;
        }
        $objs = json_decode($resp_data);
        if (null == $objs)
        {
            return OTP_JSON_PROCESS_FAILED;
        }

        $mac_hex = $objs->mac;
        if (empty($mac_hex))
        {
            //出错时为明文
            $json_plain = json_encode($objs->data);
        }
        else
        {
            //数据密文16进制
            $data_ciper_hex = $objs->data;
            if (null == $data_ciper_hex)
            {
                return OTP_JSON_PROCESS_FAILED;
            }
            //print_r('==== ==== receive data: ' . $data_ciper_hex . '<br>');
            //print_r('==== ==== receive mac: ' . $mac_hex . '<br>');
            //转为2进制
            $data_ciper = $this->mybinhex->hex2bin($data_ciper_hex);
            //获取密文哈希
            $this->get_ciper_hash($data_ciper, $hash_hex);
            //print_r('==== ==== computer hash_hex: ' . $hash_hex . '<br>');
            if ($hash_hex != $mac_hex)
            {
                return OTP_DATA_HASH_ERR;
            }
            //密文解密
            $json_plain = $this->myaes->decrypt($data_ciper);
        }
        //print_r('==== ==== json_plain: ' . $json_plain . '<br>');
        //$this->write_log('==== ==== json_plain: ' . $json_plain . "\n\n");

        return OTP_OK;
    }

    /*
    名字： 请求字符串处理 sdk内部使用
    参数： 
        $data_plain     数据明文
        $request        http请求
        $request_new    http请求 (out)
    */
    private function compose_request($data_plain, $request, &$request_new)
    {
        if (empty($data_plain))
        {
            $request_new = $request . '&data=' . '&mac=';
        }
        else
        {
            //加密数据
            //print_r('==== ==== data_plain: ' . $data_plain . '<br>');
            //$this->write_log('==== ==== data_plain: ' . $data_plain);
            $this->get_data_ciper($data_plain, $data_ciper);
            //print_r('==== ==== data_ciper_hex: ' . $this->mybinhex->bin2hex($data_ciper) . '<br>');
            $this->get_ciper_hash($data_ciper, $data_ciper_hash_hex);
            //print_r('==== ==== data_ciper_hash_hex: ' . $data_ciper_hash_hex . '<br>');
            //拼接请求
            $request_new = $request . '&data=' . $this->mybinhex->bin2hex($data_ciper) . '&mac=' . $data_ciper_hash_hex;
        }
        //print_r('==== ==== req: ' . $request_new. '<br>');
        //$this->write_log('==== ==== req: ' . $request_new);
    }

    /*
    名字： http处理 sdk内部使用
    参数： 
        $data_plain     数据明文
        $request        http请求
        $json_plain     json明文 (out)
        $time_out       http超时时间
    */
    private function deal_http($data_plain, $request, &$json_plain, $time_out=HTTP_TIME_OUT)
    {
        //新地址
        $this->compose_request($data_plain, $request, $request_new);

        //进行http请求
        $code = $this->do_http($request_new, $resp_data, $time_out);
        if (OTP_OK != $code)
        {
            return $code;
        }
        //获取json明文
        $code = $this->get_json_plain($resp_data, $json_plain);

        return $code;
    }

    /*
    名字： POST数据加密 sdk内部使用
    参数： 
        $data_plain     数据明文
        $data_ciper_hex 密文16进制
    */
    private function encrypt_post($data_plain, &$data_ciper_hex)
    {
        print_r('==== ==== before decode: ' . $data_plain. '<br>');
        //$data_ciper_hex = $data_plain;
        //return;

        //decode
        $data_plain_decode = urldecode($data_plain);
        //print_r('==== ==== after  decode: ' . $data_plain_decode. '<br>');

        //加密数据
        $this->get_data_ciper($data_plain_decode, $data_post_ciper);

        //转为16进制
        $data_ciper_hex = $this->mybinhex->bin2hex($data_post_ciper);
        //print_r('==== ==== after encrypt: ' . $data_ciper_hex. '<br>');
    }

    /*
    名字： http处理 sdk内部使用
    参数： 
        $data_plain     数据明文
        $request        http请求
        $data_post      待post数据
        $json_plain     json明文 (out)
        $time_out       http超时时间
    */
    private function deal_http_post($data_plain, $request, $data_post, &$json_plain, $time_out=HTTP_TIME_OUT)
    {
        //新地址
        $this->compose_request($data_plain, $request, $request_new);

        //进行http请求
        $code = $this->do_http_post($request_new, $data_post, $resp_data, $time_out);
        if (OTP_OK != $code)
        {
            return $code;
        }
        //获取json明文
        $code = $this->get_json_plain($resp_data, $json_plain);

        return $code;
    }

    /*
    名字： http处理 sdk内部使用
    参数： 
        $data_plain     数据明文
        $request        http请求
        $json_objs      json数据对象 (out)
        $time_out       http超时时间
        $time_wait      执行等待时间
        $time_max       执行最大时间
    */
    private function deal_http_lots($data_plain, $request, &$json_objs, $time_out=HTTP_TIME_OUT, $time_wait=2, $time_max=50)
    {
        $this->compose_request($data_plain, $request, $request_new);

        $t_start = time();
        while (1)
        {
            sleep($time_wait);

            //进行http请求
            $code = $this->do_http($request_new, $resp_data, $time_out);
            if (OTP_OK != $code)
            {
                break;
            }
            //print_r('==== ==== code: ' . $code. ' do_http <br>');

            //获取json明文
            $code = $this->get_json_plain($resp_data, $json_plain);
            if (OTP_OK != $code)
            {
                break;
            }
            //print_r('==== ==== code: ' . $code. ' get_json_plain <br>');

            //解析json数据
            $json_objs = json_decode($json_plain);
            if (null == $json_objs)
            {
                return OTP_JSON_PROCESS_FAILED;
            }
            $code = $json_objs->code;

            //“正在处理”、“超时”
            if (OTP_PUSHAUTH_PROCESS == $code || OTP_HTTP_TIMEOUT == $code)
            {
                //计算用时
                $t_end = time();
                $t_use = $t_end - $t_start;

                //判断是否到了最大时间
                if ($t_use + $time_wait < $time_max)
                {
                    continue;
                }
                //返回超时
                else
                {
                    $code = OTP_HTTP_TIMEOUT;
                    break;
                }
            }
            //其他返回值，直接返回
            else
            {
                break;
            }
        }

        return $code;
    }

    /*
    名字： 获取随机数
    参数： 
    */
    private function get_random()
    {
        return rand(0, 999999);
    }

    /*
    名字： 获取访问令牌 sdk内部使用
    参数： 
        $server_url     认证服务的URL
        $app_id         应用的标示
        $app_secret     应用密钥
    */
    private function get_token($server_url, $app_id, $app_secret)
    {
        if (empty($this->refresh_token))
        {
            $request = $this->server_url . '?method=access_token&response_type=token&client_id=' . $this->app_id . '&apptype=' . $this->app_type . '&redirect_uri=cloudentify_php_sdk_v3.6';
        }
        else
        {
            $request = $this->server_url . '?method=access_token&response_type=token&client_id=' . $this->app_id . '&apptype=' . $this->app_type . '&redirect_uri=cloudentify_php_sdk_v3.6' . '&refresh_token=' . $this->refresh_token;
        }
        $state = strval($this->get_random());
        $request = $request . '&state=' . $state;
        //print_r('==== ==== req: ' . $request . '<br>');

        //进行http请求
        $code = $this->do_http($request, $resp_data);
        if (OTP_OK != $code)
        {
            return $code;
        }
        //获取json明文
        $code = $this->get_json_plain($resp_data, $json_plain);
        if (OTP_OK != $code)
        {
            return $code;
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return OTP_JSON_PROCESS_FAILED;
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
            $this->get_time = time();
            $this->access_token = $objs->access_token;
            $this->refresh_token = $objs->refresh_token;
            $this->expires_in = $objs->expires_in;
        }

        return $code;
    }

    /*
    名字： 检查有效时间 sdk内部使用
    参数： 
    */
    private function check_expire()
    {
        if (0 == $this->init)
        {
            return OTP_NOT_INIT;
        }

        $code = OTP_OK;
        $cur_time = time();
        if ($this->get_time+$this->expires_in < $cur_time)
        {
            $file = 'cloudentifysdk_temp.txt';
            $fp = fopen($file, 'a');
            if (flock($fp, LOCK_EX))
            {
                if ($this->get_time+$this->expires_in < $cur_time)
                {
                    //print_r('==== ==== get token aggin.' . '<br>');
                    $code = $this->get_token($this->server_url, $this->app_id, $this->app_secret);
                    flock($fp, LOCK_UN);
                }
            }
            fclose($fp);
        }  

        return $code;
    }

    /*
    名字： 获取push认证结果
    参数： 
        $req_id         请求ID
    */
    private function get_push_auth_result($req_id)
    {
        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"reqid":"' . $req_id . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=queryauthresult&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http_lots($data_plain, $request, $objs, HTTP_TIME_OUT);
        if (null != $objs)
        {
            if (OTP_OK == $code)
            {
                if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
                {
                    $code = OTP_INVALID_STATE;
                }
            }
        }

        return $code;
    }

    /*
    名字： 处理二维码
    参数： 
        $qr_code        二维码字符串
        $req_id         请求ID (out)
    */
    private function deal_qr_code($qr_code, &$req_id)
    {
        //起始字符
        if (0 != strncasecmp($qr_code, "cloud", 5))
        {
            return OTP_INVALID_QR_CODE_FORMAT;
        }

        //分割成数组
        $arr_str = explode('/',$qr_code);

        //格式检查
        $len = count($arr_str);
        if ($len < 5)
        {
            return OTP_INVALID_QR_CODE_FORMAT;
        }
        if (0 != strcmp($arr_str[$len - 2], "02"))
        {
            return OTP_INVALID_QR_CODE_FORMAT;
        }

        //最后的字符串
        if (empty($arr_str[$len - 1]))
        {
            return OTP_INVALID_QR_CODE_FORMAT;
        }
        $req_id = $arr_str[$len - 1];

        return OTP_OK;
    }

    /*
    名字： 获取所有错误信息(中文) sdk内部使用
    参数： 
        $err_no         错误码
    */
    private function get_err_msg_all_cn($err_no)
    {
        $err_msg = "";

        switch ($err_no)
        {
        case OTP_OK:
            $err_msg = "业务成功响应";
            break;

        case OTP_PARAM_TYPE_ERR:
            $err_msg = "业务请求参数缺少或参数类型不正确";
            break;
        case OTP_DEAL_REQ_FAILED:
            $err_msg = "处理请求失败，未知错误原因";
            break;
        case OTP_DEAL_DATA_ERR:
            $err_msg = "处理数据解析错误，无效的请求";
            break;
        case OTP_REQ_NOT_SUPPORT:
            $err_msg = "请求不支持，无效的请求";
            break;
        case OTP_REQ_INVALID:
            $err_msg = "无效的客户端请求";
            break;
        case OTP_DATA_DECRPT_FAILED:
            $err_msg = "数据解密失败，请检查安全密钥是否填写错误或已被服务器端更新";
            break;

        case OTP_APP_ID_INVALID:
            $err_msg = "无效应用标识，请检查是否已经添加应用或应用标识是否正确";
            break;
        case OTP_ACCESSTOKEN_INVALID:
            $err_msg = "无效的会话标识";
            break;
        case OTP_ACCESSTOKEN_EXPIRED:
            $err_msg = "会话标识已过期";
            break;
        case OTP_URI_INVALID:
            $err_msg = "无效的回调URI";
            break;

        case OTP_USER_NOT_EXIST:
            $err_msg = "用户不存在，请检查您输入的用户帐号是否正确";
            break;
        case OTP_USER_DISABLED:
            $err_msg = "用户未启用";
            break;
        case OTP_USER_LOCKED:
            $err_msg = "用户已锁定，请稍后重试";
            break;
        case OTP_USER_NO_TOKEN:
            $err_msg = "用户未绑定令牌";
            break;
        case OTP_USER_BIND_TOKEN_FAILED:
            $err_msg = "用户绑定令牌失败";
            break;
        case OTP_USER_NOT_HAVE_PERMISSION:
            $err_msg = "用户没有应用的访问权限";
            break;
        case OTP_USER_PHONE_NUBER_EXSIT:
            $err_msg = "手机号已存在";
            break;
        case OTP_USER_PHONE_TOKEN_FAILED:
            $err_msg = "手机令牌激活码生成失败";
            break;
        case OTP_USER_EXSIT:
            $err_msg = "用户已存在";
            break;
        case OTP_USER_NO_FACEE_TOKNE:
            $err_msg = "用户未激活脸谱令牌";
            break;
        case OTP_USER_NO_MOBILE_TOKNE:
            $err_msg = "用户未激活手机令牌";
            break;
        case OTP_USER_NO_WECHAT_TOKNE:
            $err_msg = "用户未激活微信令牌";
            break;
        case OTP_USER_NO_VOICE_TOKNE:
            $err_msg = "用户未激活语音令牌";
            break;
        case OTP_USER_NO_GESTURE_TOKNE:
            $err_msg = "用户未激活手势令牌";
            break;

        case OTP_TOKEN_NOT_EXIST:
            $err_msg = "令牌不存在";
            break;
        case OTP_TOKEN_DISABLED:
            $err_msg = "令牌未启用";
            break;
        case OTP_TOKEN_LOCKED:
            $err_msg = "令牌已锁定，请稍后重试";
            break;
        case OTP_TOKEN_IS_BINDED:
            $err_msg = "令牌已经被绑定";
            break;
        case OTP_TOKEN_NOT_OWNED:
            $err_msg = "该企业下不存在该令牌";
            break;
        case OTP_TOKEN_DECRPT_FAILED:
            $err_msg = "解密令牌数据失败";
            break;
        case OTP_TOKEN_EXPIRED:
            $err_msg = "令牌已过期";
            break;

        case OTP_INVALID:
            $err_msg = "动态口令验证失败";
            break;
        case OTP_HAVE_USED:
            $err_msg = "动态口令已经被使用过，请输入新的动态口令";
            break;
        case OTP_TOKEN_NEED_SYNC:
            $err_msg = "云信令时间需要校准";
            break;
        case OTP_AUTH_POLICY_INVALID:
            $err_msg = "无效用户/用户组认证策略";
            break;
        case OTP_AUTH_POLICY_ACCEPT:
            $err_msg = "认证策略为：不进行动态口令认证，直接返回成功";
            break;
        case OTP_AUTH_POLICY_REJECT:
            $err_msg = "认证策略为：不进行动态口令认证，直接返回失败";
            break;
        case OTP_TOKEN_SYNC_FAILED:
            $err_msg = "云信令时间校准失败";
            break;
        case OTP_AUTH_POLICY_REJECT_WIFI:
            $err_msg = "WIFI应用不能使用不进行动态口令认证，直接返回成功的认证策略";
            break;
        case OTP_WIFI_SECRET_EMPTY:
            $err_msg = "公网WIFI密钥为空";
            break;
        case OTP_APP_TYPE_INVALID:
            $err_msg = "应用集成类型与代理类型不匹配，请检查应用类型选择是否正确";
            break;
        case OTP_TWO_FACTOR_AUTH:
            $err_msg = "认证策略为：进行动态口令认证";
            break;
        case OTP_PUSH_AUTH_REJECT:
            $err_msg = "推送认证请求选择了拒绝的处理方式";
            break;
        case OTP_AUTH_TIMEOUT:
            $err_msg = "推送认证请求超时，请重新进行推送";
            break;
        case OTP_PUSH_AUTH_NO_TOKEN:
            $err_msg = "未激活手机云信令或微信云信令，不能进行推送认证";
            break;
        case OTP_PUSH_AUTH_NO_TERMINAL:
            $err_msg = "没有可推送的终端，请检查您的手机云信令是否已经开启";
            break;
        case OTP_SCAN_AUTH_FAILED:
            $err_msg = "扫码认证失败";
            break;
        case OTP_USER_MATCH_FAILED:
            $err_msg = "用户账号不匹配";
            break;
        case OTP_REQID_EXPIERD_FAILED:
            $err_msg = "扫码认证请求已过期";
            break;
        case OTP_FACE_AUTH_FAILED:
            $err_msg = "人脸认证失败";
            break;
        case OTP_PUSH_CERT_NOT_EXIST:
            $err_msg = "没有推送证书";
            break;
        case OTP_NO_TOKEN_TYPE_TO_SYNC:
            $err_msg = "没有可同步的令牌类型";
            break;
        case OTP_PUSHAUTH_PROCESS:
            $err_msg = "认证处理中";
            break;
        case OTP_APP_PARAM_INVALID:
            $err_msg = "云信令客户端参数错误";
            break;
        case OTP_CLIENT_TOKEN_DELETED:
            $err_msg = "客户端令牌己删除";
            break;
        case OTP_FACE_DETECT_FAILED:
            $err_msg = "人脸认证活体检测失败";
            break;
        case OTP_POLICY_GRUOP_INVALID:
            $err_msg = "无效认证策略，用户选择使用组策略，但不属于任何组";
            break;
        case OTP_DEVICE_NOT_SUPPORT:
            $err_msg = "不支持此设备类型";
            break;
        case OTP_VOICE_TOKEN_DISABLED:
            $err_msg = "未启用语音令";
            break;
        case OTP_VOICE_AUTH_FAILED:
            $err_msg = "语音认证失败";
            break;
        case OTP_GESTURE_TOKEN_DISABLED:
            $err_msg = "未启用手势令";
            break;
        case OTP_GESTURE_AUTH_FAILED:
            $err_msg = "手势认证失败";
            break;
        case OTP_U2F_ADDR_NOT_MATCH:
            $err_msg = "U2F受保护地址不区配";
            break;
        case OTP_PSUH_TOO_MUCH:
            $err_msg = "推送太频繁，请稍后重试";
            break;
        case FACE_ENGINE_NOT_MATCHED:
            $err_msg = "人脸引擎类型不匹配";
            break;
        case FACE_SERVICE_NOT_MATCHED:
            $err_msg = "人脸服务类型不匹配";
            break;
        case FACE_REGIST_FAILED:
            $err_msg = "人脸注册失败";
            break;
        case OTP_PSUH_USER_NOT_MATCHED:
            $err_msg = "推送和响应用户不匹配";
            break;
        case FACE_SERVICE_CONN_FAILED:
            $err_msg = "无法连接脸谱服务器";
            break;
        case FACE_SERVICE_PROCESS_FAILED:
            $err_msg = "脸谱服务供应商处理失败";
            break;
        case PSUH_SERVICE_CONN_FAILED:
            $err_msg = "无法连接推送服务器";
            break;
        case COMPANY_LOCKED:
            $err_msg = "企业已锁定";
            break;
        case APP_LOCKED:
            $err_msg = "应用已锁定";
            break;
        case USER_NUM_UPTO_MAX:
            $err_msg = "用户数已达上限";
            break;
        case ACCOUNT_INSUFFICIENT:
            $err_msg = "企业账号余额不足";
            break;

        case OTP_USER_ADD_FAILED:
            $err_msg = "添加用户信息失败";
            break;
        case OTP_PHONE_ADD_FAILED:
            $err_msg = "添加手机号失败";
            break;
        case OTP_PHONE_NOT_EXIST:
            $err_msg = "手机号不存在";
            break;

        case OTP_SEND_SMS_FAILED:
            $err_msg = "发送短信验证码失败，请重新发送";
            break;
        case OTP_SEND_SMS_TOO_MUCH:
            $err_msg = "发送短信验证码失败，您的操作过于频繁！";
            break;

        case OTP_PARAM_INVALID:
            $err_msg = "SDK客户端参数不对";
            break;
        case OTP_NOT_INIT:
            $err_msg = "SDK客户端未初始化";
            break;
        case OTP_CONF_NOT_EXIST:
            $err_msg = "配置文件不存在";
            break;
        case OTP_CONF_READ_ERR:
            $err_msg = "读取配置文件失败";
            break;
        case OTP_DATA_HASH_ERR:
            $err_msg = "SDK客户端解密数据失败";
            break;
        case OTP_JSON_PROCESS_FAILED:
            $err_msg = "SDK客户端JSON处理失败";
            break;
        case OTP_INVALID_QR_CODE_FORMAT:
            $err_msg = "无效二维码格式";
            break;
        case OTP_INVALID_STATE:
            $err_msg = "状态检查不一致";
            break;

        case OTP_HTTP_TIMEOUT:
            $err_msg = "SDK客户端请求超时";
            break;
        case OTP_HTTP_CANTCONNECT:
            $err_msg = "SDK客户端不能连接";
            break;
        case OTP_HTTP_FAIL:
            $err_msg = "SDK客户端HTTP请求异常";
            break;

        default:
            $err_msg = "未知的错误码";
            break;
        }

        return $err_msg;
    }

    /*
    名字： 获取所有错误信息 sdk内部使用
    参数： 
        $err_no         错误码
    */
    private function get_err_msg_all($err_no)
    {
        $err_msg = "";

        switch($err_no)
        {
        case OTP_OK: 
            $err_msg = "success";
            break;

        case OTP_PARAM_TYPE_ERR: 
            $err_msg = "params count or type invalid";
            break;
        case OTP_DEAL_REQ_FAILED:
            $err_msg = "process request failed, unknown error";
            break;
        case OTP_DEAL_DATA_ERR:
            $err_msg = "parse data failed, invalid request";
            break;
        case OTP_REQ_NOT_SUPPORT:
            $err_msg = "request not support, invalid request";
            break;
        case OTP_REQ_INVALID:
            $err_msg = "client request invalid";
            break;
        case OTP_DATA_DECRPT_FAILED:
            $err_msg = "data decrypt failed, please check app secret was spell wrong or was updated";
            break;

        case OTP_APP_ID_INVALID:
            $err_msg = "app id invalid";
            break;
        case OTP_ACCESSTOKEN_INVALID:
            $err_msg = "access token invalid";
            break;
        case OTP_ACCESSTOKEN_EXPIRED:
            $err_msg = "access token has expired";
            break;
        case OTP_URI_INVALID:
            $err_msg = "URI invalid";
            break;

        case OTP_USER_NOT_EXIST:
            $err_msg = "user not exist";
            break;
        case OTP_USER_DISABLED:
            $err_msg = "user is disabled";
            break;
        case OTP_USER_LOCKED:
            $err_msg = "user is locked";
            break;
        case OTP_USER_NO_TOKEN:
            $err_msg = "user not bind token";
            break;
        case OTP_USER_BIND_TOKEN_FAILED:
            $err_msg = "user bind token failed";
            break;
        case OTP_USER_NOT_HAVE_PERMISSION:
            $err_msg = "user not have privileges";
            break;
        case OTP_USER_PHONE_NUBER_EXSIT:
            $err_msg = "phone number not exist";
            break;
        case OTP_USER_PHONE_TOKEN_FAILED:
            $err_msg = "create mobile token active code failed";
            break;
        case OTP_USER_EXSIT:
            $err_msg = "user has exist";
            break;
        case OTP_USER_NO_FACEE_TOKNE:
            $err_msg = "user not active face token";
            break;
        case OTP_USER_NO_MOBILE_TOKNE:
            $err_msg = "user not active mobile token";
            break;
        case OTP_USER_NO_WECHAT_TOKNE:
            $err_msg = "user not active wechat token";
            break;
        case OTP_USER_NO_VOICE_TOKNE:
            $err_msg = "user not active voice token";
            break;
        case OTP_USER_NO_GESTURE_TOKNE:
            $err_msg = "user not active gesture token";
            break;

        case OTP_TOKEN_NOT_EXIST:
            $err_msg = "token not exist";
            break;
        case OTP_TOKEN_DISABLED:
            $err_msg = "token is disabled";
            break;
        case OTP_TOKEN_LOCKED:
            $err_msg = "token is locked";
            break;
        case OTP_TOKEN_IS_BINDED:
            $err_msg = "token has been bind";
            break;
        case OTP_TOKEN_NOT_OWNED:
            $err_msg = "enterprise is not own token";
            break;
        case OTP_TOKEN_DECRPT_FAILED:
            $err_msg = "decrypt token data failed";
            break;
        case OTP_TOKEN_EXPIRED:
            $err_msg = "data has expired";
            break;

        case OTP_INVALID:
            $err_msg = "otp invalid";
            break;
        case OTP_HAVE_USED:
            $err_msg = "otp has used";
            break;
        case OTP_TOKEN_NEED_SYNC:
            $err_msg = "token need synchronoused";
            break;
        case OTP_AUTH_POLICY_INVALID:
            $err_msg = "auth policy invalid";
            break;
        case OTP_AUTH_POLICY_ACCEPT:
            $err_msg = "auth policy allowed";
            break;
        case OTP_AUTH_POLICY_REJECT:
            $err_msg = "auth policy prohibited";
            break;
        case OTP_TOKEN_SYNC_FAILED:
            $err_msg = "token synchronoused failed";
            break;
        case OTP_AUTH_POLICY_REJECT_WIFI:
            $err_msg = "WIFI app not support auth policy allowed";
            break;
        case OTP_WIFI_SECRET_EMPTY:
            $err_msg = "WIFI secret is empty";
            break;
        case OTP_APP_TYPE_INVALID:
            $err_msg = "app type different from agent type";
            break;
        case OTP_TWO_FACTOR_AUTH:
            $err_msg = "auth policy is double factor";
            break;
        case OTP_PUSH_AUTH_REJECT:
            $err_msg = "push authentication rejected";
            break;
        case OTP_AUTH_TIMEOUT:
            $err_msg = "authentication timeout";
            break;
        case OTP_PUSH_AUTH_NO_TOKEN:
            $err_msg = "not exist token for push";
            break;
        case OTP_PUSH_AUTH_NO_TERMINAL:
            $err_msg = "not exist terminal for push";
            break;
        case OTP_SCAN_AUTH_FAILED:
            $err_msg = "scan authentication failed";
            break;
        case OTP_USER_MATCH_FAILED:
            $err_msg = "user not matched";
            break;
        case OTP_REQID_EXPIERD_FAILED:
            $err_msg = "scan authentication request has expired";
            break;
        case OTP_FACE_AUTH_FAILED:
            $err_msg = "face authentication failed";
            break;
        case OTP_PUSH_CERT_NOT_EXIST:
            $err_msg = "not exist certificate for push";
            break;
        case OTP_NO_TOKEN_TYPE_TO_SYNC:
            $err_msg = "no token type to synchronoused";
            break;
        case OTP_PUSHAUTH_PROCESS:
            $err_msg = "push authentication processing";
            break;
        case OTP_APP_PARAM_INVALID:
            $err_msg = "cloundentify client params invalid";
            break;
        case OTP_CLIENT_TOKEN_DELETED:
            $err_msg = "client token has been deleted";
            break;
        case OTP_FACE_DETECT_FAILED:
            $err_msg = "face active detect failed";
            break;
        case OTP_POLICY_GRUOP_INVALID:
            $err_msg = "user auth policy invalid";
            break;
        case OTP_DEVICE_NOT_SUPPORT:
            $err_msg = "device type not supported";
            break;
        case OTP_VOICE_TOKEN_DISABLED:
            $err_msg = "voice token disabled";
            break;
        case OTP_VOICE_AUTH_FAILED:
            $err_msg = "voice auth failed";
            break;
        case OTP_GESTURE_TOKEN_DISABLED:
            $err_msg = "gesture token disabled";
            break;
        case OTP_GESTURE_AUTH_FAILED:
            $err_msg = "gesture auth failed";
            break;
        case OTP_U2F_ADDR_NOT_MATCH:
            $err_msg = "U2F protected address not mathced";
            break;
        case OTP_PSUH_TOO_MUCH:
            $err_msg = "push too much, please wait";
            break;
        case FACE_ENGINE_NOT_MATCHED:
            $err_msg = "face engine not matched";
            break;
        case FACE_SERVICE_NOT_MATCHED:
            $err_msg = "face service not matched";
            break;
        case FACE_REGIST_FAILED:
            $err_msg = "face regist failed";
            break;
        case OTP_PSUH_USER_NOT_MATCHED:
            $err_msg = "push user not matched";
            break;
        case FACE_SERVICE_CONN_FAILED:
            $err_msg = "connect face server failed";
            break;
        case FACE_SERVICE_PROCESS_FAILED:
            $err_msg = "face service provider process failed";
            break;
        case PSUH_SERVICE_CONN_FAILED:
            $err_msg = "connect push server failed";
            break;
        case COMPANY_LOCKED:
            $err_msg = "your company has locked";
            break;
        case APP_LOCKED:
            $err_msg = "your app has locked";
            break;
        case USER_NUM_UPTO_MAX:
            $err_msg = "user's num has up to max";
            break;
        case ACCOUNT_INSUFFICIENT:
            $err_msg = "account of company is insufficient";
            break;

        case OTP_USER_ADD_FAILED:
            $err_msg = "add user failed";
            break;
        case OTP_PHONE_ADD_FAILED:
            $err_msg = "add phone number failed";
            break;
        case OTP_PHONE_NOT_EXIST:
            $err_msg = "phone number not exist";
            break;

        case OTP_SEND_SMS_FAILED:
            $err_msg = "send sms failed, please try";
            break;
        case OTP_SEND_SMS_TOO_MUCH:
            $err_msg = "send sms failed, too much, please wait";
            break;

        case OTP_PARAM_INVALID:
            $err_msg = "client params invalid";
            break;
        case OTP_NOT_INIT:
            $err_msg = "client not init";
            break;
        case OTP_CONF_NOT_EXIST:
            $err_msg = "configure files not exist";
            break;
        case OTP_CONF_READ_ERR:
            $err_msg = "read configure files failed";
            break;
        case OTP_DATA_HASH_ERR:
            $err_msg = "hash of data invalid";
            break;
        case OTP_JSON_PROCESS_FAILED:
            $err_msg = "process json failed";
            break;
        case OTP_INVALID_STATE:
            $err_msg = "state not matched";
            break;

        case OTP_INVALID_QR_CODE_FORMAT:
            $err_msg = "qrcode invalid";
            break;
        case OTP_HTTP_TIMEOUT:
            $err_msg = "http session timeout";
            break;
        case OTP_HTTP_CANTCONNECT:
            $err_msg = "http connect failed";
            break;
        case OTP_HTTP_FAIL:
            $err_msg = "http request failed";
            break;
        default:
            $err_msg = "unknown error number!";
            break;
        }

        return $err_msg;
    }

    /*
    名字： 获取SDK错误信息 sdk内部使用
    参数： 
        $err_no         错误码
    */
    private function get_err_msg_sdk($err_no)
    {
        $err_msg = "";

        switch($err_no)
        {
        case OTP_PARAM_INVALID:
            $err_msg = "client params invalid";
            break;
        case OTP_NOT_INIT:
            $err_msg = "client not init";
            break;
        case OTP_CONF_NOT_EXIST:
            $err_msg = "configure files not exist";
            break;
        case OTP_CONF_READ_ERR:
            $err_msg = "read configure files failed";
            break;
        case OTP_DATA_HASH_ERR:
            $err_msg = "hash of data invalid";
            break;
        case OTP_INVALID_STATE:
            $err_msg = "state not matched";
            break;

        case OTP_JSON_PROCESS_FAILED:
            $err_msg = "process json failed";
            break;
        case OTP_INVALID_QR_CODE_FORMAT:
            $err_msg = "qrcode invalid";
            break;
        case OTP_HTTP_TIMEOUT:
            $err_msg = "http session timeout";
            break;
        case OTP_HTTP_CANTCONNECT:
            $err_msg = "http connect failed";
            break;
        case OTP_HTTP_FAIL:
            $err_msg = "http request failed";
            break;
        default:
            $err_msg = "unknown error number!";
            break;
        }

        return $err_msg;
    }

    /*
    名字： 上传错误 sdk内部使用
    参数： 
        $err_no         错误码
        $func_name      函数名
    */
    private function submit_err($err_no, $func_name)
    {
        //print_r("==== ==== submit_err() err_no: " . $err_no . "<br>");
        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $code;
        }

        //错误信息
        $err_msg = $this->get_err_msg_sdk($err_no);
        //错误内容 json ---> string
        //$logcontent = '{"func":"' . $func_name . '","code":"' . $err_no . '","msg":"' . $err_msg . '"}';
        $logcontent = 'func:' . $func_name . ',code:' . $err_no . ',msg:' . $err_msg;

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"logtype":"11",' . '"logcontent":"' . $logcontent . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=submitexception&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $code;
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return OTP_JSON_PROCESS_FAILED;
        }
        $code = $objs->code;
        //$msg = $objs->msg;

        return $code;
    }

    /*
    名字： 错误处理 sdk内部使用
    参数： 
        $err_no         错误码
        $func_name      函数名
    */
    private function deal_err($err_no, $func_name)
    {
        //print_r("==== ==== deal_err() err_no: " . $err_no . "<br>");
        if (OTP_PARAM_INVALID <= $err_no || -1 == $err_no)
        {
            $this->submit_err($err_no, $func_name);
        }

        return $err_no;
    }

    /*
    名字： 设置应用类型
    参数： 
        $app_type       应用类型
    */
    public function set_apptype($app_type)
    {
        $this->app_type = $app_type;
    }

    /*
    名字： 初始化认证服务
    参数： 
        $server_url     认证服务的URL
        $app_id         应用的标识
        $app_secret     应用密钥
    */
    public function init($server_url, $app_id, $app_secret)
    {
        if (empty($server_url) || empty($app_id) || empty($app_secret))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }
        if (!function_exists("curl_init") || 
            !function_exists("curl_setopt") || 
            !function_exists("curl_exec") || 
            !function_exists("curl_close"))
        {
            $this->init = 0;
            echo 'Need curl support!' . '<br>';
            return $this->deal_err(-1, __function__);
        }

        $this->server_url = $server_url;
        $this->app_id = $app_id;
        $this->app_secret = $app_secret;

        //产生密钥
        $this->mypubkey->gen_pubkey($this->mybinhex->hex2bin($this->app_secret), $this->key);
        //print_r('==== ==== key_hex: ' . $this->mybinhex->bin2hex($this->key) . '<br>');
        //设置密钥
        $this->myaes->set_key($this->key);

        $code = $this->get_token($server_url, $app_id, $app_secret);
        if (OTP_OK != $code)
        {
            $this->init = 0;
        }
        else
        {
            $this->init = 1;
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 反初始化认证服务
    参数： 
    */
    public function uninit()
    {
        $file = 'cloudentifysdk_temp.txt';
        if (file_exists($file))
        {
            unlink($file);
        }
    }


    /*
    名字： 根据用户名认证OTP
    参数： 
        $user_name      用户名
        $otp            令牌的OTP
        $retry          剩余重试次数 (out)
    */
    public function user_otp_auth($user_name, $otp, &$retry)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name) || empty($otp) || false == is_numeric($otp))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }
        $retry = -1;

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"userid":"' . $user_name . '",' . '"otp":"' . $otp . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=otpuserauth&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }
        else
        {
            if ( array_key_exists("retrycnt", $objs) )
            {
                $retry = $objs->retrycnt;
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 根据令牌号认证OTP
    参数： 
        $token_sn       令牌序列号
        $otp            令牌的OTP
        $retry          剩余重试次数 (out)
    */
    public function token_otp_auth($token_sn, $otp, &$retry)
    {
        if (empty($token_sn) || empty($otp) || false == is_numeric($otp))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }
        $retry = -1;

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"tokensn":"' . $token_sn . '",' . '"otp":"' . $otp . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=otptokenauth&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }
        else
        {
            if ( array_key_exists("retrycnt", $objs) )
            {
                $retry = $objs->retrycnt;
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 根据用户名push认证
    参数： 
        $user_name      用户名
        $phone_sn       手机号 (可以为空)
    */
    public function push_auth($user_name, $phone_sn)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        if (empty($phone_sn))
        {
            $data_plain = '{"userid":"' . $user_name . '",' . '"state":"' . $state . '"}';
        }
        else
        {
            $data_plain = '{"userid":"' . $user_name . '",' . '"phonesn":"' . $phone_sn . '",' . '"state":"' . $state . '"}';
        }
        $request = $this->server_url . '?method=pushauth&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain, HTTP_TIME_OUT);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        if (OTP_PUSHAUTH_PROCESS == $code)
        {
            if ( array_key_exists("reqid", $objs) )
            {
                $req_id = $objs->reqid;
                $code = $this->get_push_auth_result($req_id);
            }
            else
            {
                $code = OTP_JSON_PROCESS_FAILED;
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 判断用户是否存在
    参数： 
        $user_name      用户名
    */
    public function is_user_exist($user_name)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"userid":"' . $user_name . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=isuserexist&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 添加用户
    参数： 
        $user_name      用户名
    */
    public function add_user($user_name)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"userid":"' . $user_name . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=adduser&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 绑定令牌
    参数： 
        $user_name      用户名
        $token_sn       令牌序列号
    */
    public function bind_token($user_name, $token_sn)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($token_sn) || empty($token_sn))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"userid":"' . $user_name . '",' . '"tokensn":"' . $token_sn . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=bindtoken&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 解绑令牌
    参数： 
        $user_name      用户名
        $token_sn       令牌序列号
    */
    public function unbind_token($user_name, $token_sn)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name) || empty($token_sn))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"userid":"' . $user_name . '",' . '"tokensn":"' . $token_sn . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=unbindtoken&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 启用令牌
    参数： 
        $token_sn       令牌序列号
    */
    public function enable_token($token_sn)
    {
        if (empty($token_sn))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"tokensn":"' . $token_sn . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=enabletoken&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 停用令牌
    参数： 
        $token_sn       令牌序列号
    */
    public function disable_token($token_sn)
    {
        if (empty($token_sn))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"tokensn":"' . $token_sn . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=unenabletoken&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 启用用户
    参数： 
        $user_name      用户名
    */
    public function enable_user($user_name)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"userid":"' . $user_name . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=enableuser&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 停用用户
    参数： 
        $user_name      用户名
    */
    public function disable_user($user_name)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"userid":"' . $user_name . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=unenableuser&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 锁定令牌
    参数： 
        $token_sn       令牌序列号
    */
    public function lock_token($token_sn)
    {
        if (empty($token_sn))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"tokensn":"' . $token_sn . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=locktoken&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 解锁令牌
    参数： 
        $token_sn       令牌序列号
    */
    public function unlock_token($token_sn)
    {
        if (empty($token_sn))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"tokensn":"' . $token_sn . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=unlocktoken&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 锁定用户
    参数： 
        $user_name      用户名
    */
    public function lock_user($user_name)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"userid":"' . $user_name . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=lockuser&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 解锁用户
    参数： 
        $user_name      用户名
    */
    public function unlock_user($user_name)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"userid":"' . $user_name . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=unlockuser&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 根据用户名同步令牌
    参数： 
        $user_name      用户名
        $otp            当前口令
        $next_otp       下一个口令
    */
    public function user_otp_sync($user_name, $otp, $next_otp)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name) || empty($otp) || false == is_numeric($otp) || 
            empty($next_otp) || false == is_numeric($next_otp))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"userid":"' . $user_name . '",' . '"otp":"' . $otp . '",' . '"nextotp":"' . $next_otp . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=otpusersync&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 根据令牌号同步令牌
    参数： 
        $token_sn   令牌序列号
        $otp        当前口令
        $next_otp   下一个口令
    */
    public function token_otp_sync($token_sn, $otp, $next_otp)
    {
        if (empty($token_sn) ||  empty($otp) || false == is_numeric($otp) || 
            empty($next_otp) || false == is_numeric($next_otp))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"tokensn":"' . $token_sn . '",' . '"otp":"' . $otp . '",' . '"nextotp":"' . $next_otp . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=otptokensync&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 激活手机令牌
    参数： 
        $user_name      用户名
        $phone_sn       手机号 (可以为空)
        $active_code    令牌激活码 (out)
        $auth_code      激活验证码 (out) (非空时为离线激活，请输入app)
    */
    public function active_mobile_token($user_name, $phone_sn, &$active_code, &$auth_code)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        if (empty($phone_sn))
        {
            $data_plain = '{"userid":"' . $user_name . '",' . '"state":"' . $state . '"}';
        }
        else
        {
            $data_plain = '{"userid":"' . $user_name . '",' . '"phonesn":"' . $phone_sn . '",' . '"state":"' . $state . '"}';
        }
        $request = $this->server_url . '?method=activemobiletoken&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }
        else
        {
            return $this->deal_err($code, __function__);
        }

        if ( array_key_exists("activecode", $objs) )
        {
            $active_code = $objs->activecode;
        }
        else
        {
            $code = OTP_JSON_PROCESS_FAILED;
        }
        if ( array_key_exists("authcode", $objs) )
        {
            $auth_code = $objs->authcode;
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 获取微信令牌激活码 (公众号后台调用，公众号关注者触发)
    参数： 
        $user_name      用户名
        $wechat_ac      微信令牌激活码 (out)
    */
    public function get_wechat_token_ac($user_name, &$wechat_ac)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"userid":"' . $user_name . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=getwechattokenac&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }
        else
        {
            return $this->deal_err($code, __function__);
        }

        if ( array_key_exists("wechatac", $objs) )
        {
            $wechat_ac = $objs->wechatac;
        }
        else
        {
            $code = OTP_JSON_PROCESS_FAILED;
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 添加用户的手机号
    参数： 
        $user_name      用户名
        $phone_sn       手机号
    */
    public function add_user_phone($user_name, $phone_sn)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name) || empty($phone_sn))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"userid":"' . $user_name . '",' . '"phonesn":"' . $phone_sn . '",' . '"platform":"0' . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=adduserphone&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 根据用户名、手机号认证OTP
    参数： 
        $user_name      用户名
        $otp            令牌的OTP
        $phone_sn       手机号
        $retry          剩余重试次数 (out)
    */
    public function user_otp_auth_by_mobile($user_name, $otp, $phone_sn, &$retry)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name) ||  empty($otp) || false == is_numeric($otp) || empty($phone_sn))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }
        $retry = -1;

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"userid":"' . $user_name . '",' . '"phonesn":"' . $phone_sn . '",' . '"otp":"' . $otp . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=otpuserauth&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }
        else
        {
            if ( array_key_exists("retrycnt", $objs) )
            {
                $retry = $objs->retrycnt;
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 检查用户 返回用户不存在时，请检查新用户认证策略
    参数： 
        $user_name      用户名
        $user_policy    用户认证策略 (out) ：
                                0需要OTP认证；
                                1不需OTP认证，直接失败；
                                2不需OTP认证，直接成功；
        $newuser_policy 新用户认证策略 (out) ：
                                0新用户需要注册；
                                1新用户直接失败；
                                2新用户直接成功；
    */
    public function check_user($user_name, &$user_policy, &$newuser_policy)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }
        $newuser_policy = -1;
        $user_policy = -1;

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"userid":"' . $user_name . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=checkuser&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
            if ( array_key_exists("authpolicy", $objs) )
            {
                $user_policy = $objs->authpolicy;
            }
            else
            {
                $code = OTP_JSON_PROCESS_FAILED;
            }
        }
        else if (OTP_USER_NOT_EXIST == $code)
        {
            if ( array_key_exists("newuserpolicy", $objs) )
            {
                $newuser_policy = $objs->newuserpolicy;
            }
            else
            {
                $code = OTP_JSON_PROCESS_FAILED;
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 获取二维码数据
    参数： 
        $user_name      用户名 (可以为空)
        $qr_code        二维码字符串 (out)
        $req_id         请求ID (out)
    */
    public function get_qrcode($user_name, &$qr_code, &$req_id)
    {
        $user_name = ltrim(rtrim($user_name));
        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        if (empty($user_name))
        {
            $data_plain = '{"state":"' . $state . '"}';
        }
        else
        {
            $data_plain = '{"userid":"' . $user_name . '",' . '"state":"' . $state . '"}';
        }
        $request = $this->server_url . '?method=getscanqrcode&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }
        else
        {
            return $this->deal_err($code, __function__);
        }

        if ( array_key_exists("reqid", $objs) )
        {
            $req_id = $objs->reqid;
        }
        else
        {
            $code = OTP_JSON_PROCESS_FAILED;
        }
        if ( array_key_exists("qrcodedata", $objs) )
        {
            $qr_code = $objs->qrcodedata;
        }
        else
        {
            $code = OTP_JSON_PROCESS_FAILED;
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 扫码认证
    参数： 
        $req_id     请求ID，通过 get_qrcode() 获取
    */
    public function scan_auth($req_id)
    {
        if (empty($req_id))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        $code = $this->get_push_auth_result($req_id);

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 用户语音认证
    参数： 
        $user_name      用户名
    */
    public function voice_auth($user_name)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"userid":"' . $user_name . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=voicepushauth&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain, HTTP_TIME_OUT);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        if (OTP_PUSHAUTH_PROCESS == $code)
        {
            if ( array_key_exists("reqid", $objs) )
            {
                $req_id = $objs->reqid;
                $code = $this->get_push_auth_result($req_id);
            }
            else
            {
                $code = OTP_JSON_PROCESS_FAILED;
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 获取人脸激活二维码
    参数： 
        $user_name      用户名
        $phone_sn       手机号 (可以为空)
        $reg_qr_code    二维码字符串 (out)
    */
    public function get_face_regist_qrcode($user_name, $phone_sn, &$reg_qr_code)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        if (empty($phone_sn))
        {
            $data_plain = '{"userid":"' . $user_name . '",' . '"state":"' . $state . '"}';
        }
        else
        {
            $data_plain = '{"userid":"' . $user_name . '",' . '"phonesn":"' . $phone_sn . '",' . '"state":"' . $state . '"}';
        }
        $request = $this->server_url . '?method=getfaceregistqrcode&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }
        else
        {
            return $this->deal_err($code, __function__);
        }

        if ( array_key_exists("qrcodedata", $objs) )
        {
            $reg_qr_code = $objs->qrcodedata;
        }
        else
        {
            $code = OTP_JSON_PROCESS_FAILED;
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 获取人脸注册结果
    参数： 
        $user_name      用户名
        $phone_sn       手机号 (可以为空)
    */
    public function get_face_regist_result($user_name, $phone_sn)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        if (empty($phone_sn))
        {
            $data_plain = '{"userid":"' . $user_name . '",' . '"tokentype":"4' . '",' . '"state":"' . $state . '"}';
        }
        else
        {
            $data_plain = '{"userid":"' . $user_name . '",' . '"phonesn":"' . $phone_sn . '",' . '"tokentype":"4' . '",' . '"state":"' . $state . '"}';
        }
        $request = $this->server_url . '?method=getactiveregistresult&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 人脸推送认证
    参数： 
        $user_name      用户名
    */
    public function face_push_auth($user_name)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"userid":"' . $user_name . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=asyncfaceauth&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain, HTTP_TIME_OUT);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        if (OTP_PUSHAUTH_PROCESS == $code)
        {
            if ( array_key_exists("reqid", $objs) )
            {
                $req_id = $objs->reqid;
                $code = $this->get_push_auth_result($req_id);
            }
            else
            {
                $code = OTP_JSON_PROCESS_FAILED;
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 发送短信口令
    参数： 
        $user_name      用户名
        $phone_sn       手机号 (可以为空)
    */
    public function send_smsotp($user_name, $phone_sn)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        if (empty($phone_sn))
        {
            $data_plain = '{"userid":"' . $user_name . '",' . '"state":"' . $state . '"}';
        }
        else
        {
            $data_plain = '{"userid":"' . $user_name . '",' . '"phonesn":"' . $phone_sn . '",' . '"state":"' . $state . '"}';
        }
        $request = $this->server_url . '?method=sendsmsotp&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 获取短信口令
    参数： 
        $user_name      用户名
        $smsotp         短信口令 (out)
    */
    public function get_smsotp($user_name, &$smsotp)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"userid":"' . $user_name . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=getsmsotp&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }
        else
        {
            return $this->deal_err($code, __function__);
        }

        if ( array_key_exists("smsotp", $objs) )
        {
            $smsotp = $objs->smsotp;
        }
        else
        {
            $code = OTP_JSON_PROCESS_FAILED;
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 检查是否已经激活手机令牌
    参数： 
        $user_name      用户名
        $phone_sn       手机号 (可以为空)
    */
    public function is_mobile_token_actived($user_name, $phone_sn)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        if (empty($phone_sn))
        {
            $data_plain = '{"userid":"' . $user_name . '",' . '"tokentype":"1' . '",' . '"state":"' . $state . '"}';
        }
        else
        {
            $data_plain = '{"userid":"' . $user_name . '",' . '"phonesn":"' . $phone_sn . '",' . '"tokentype":"1' . '",' . '"state":"' . $state . '"}';
        }
        $request = $this->server_url . '?method=getactiveregistresult&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 检查是否已经激活微信令牌 (公众号后台调用，公众号关注者触发)
    参数： 
        $user_name      用户名
    */
    public function is_wechat_token_actived($user_name)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"userid":"' . $user_name . '",' . '"tokentype":"2' . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=getactiveregistresult&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 激活微信令牌 (公众号后台调用，公众号关注者触发)
    参数： 
        $openid         微信号
        $wechat_ac      微信令牌激活码
    */
    public function active_wechat_token($openid, $wechat_ac)
    {
        if (empty($openid) || empty($wechat_ac))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"openid":"' . $openid . '",' . '"wechatac":"' . $wechat_ac . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=activewechattoken&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 获取用户信息 (公众号后台调用，公众号关注者触发)
    参数： 
        $openid         微信号
        $user_name      用户名 (out)
        $token_sn       令牌号 (out)
        $company_name   企业名称 (out)

    */
    public function get_userinfo_by_wechat($openid, &$user_name, &$token_sn, &$company_name)
    {
        if (empty($openid))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"openid":"' . $openid . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=getuserinfobywechat&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }
        else
        {
            return $this->deal_err($code, __function__);
        }

        do 
        {
            if ( array_key_exists("userid", $objs) )
            {
                $user_name = $objs->userid;
            }
            else
            {
                $code = OTP_JSON_PROCESS_FAILED;
                break;
            }
            if ( array_key_exists("tokensn", $objs) )
            {
                $token_sn = $objs->tokensn;
            }
            else
            {
                $code = OTP_JSON_PROCESS_FAILED;
                break;
            }
            if ( array_key_exists("companyname", $objs) )
            {
                $company_name = $objs->companyname;
            }
            else
            {
                $code = OTP_JSON_PROCESS_FAILED;
            }
        }
        while(0);

        return $this->deal_err($code, __function__);
    }

    /*
    名字：  获取微信令牌OTP (公众号后台调用，公众号关注者触发)
    参数： 
        $openid         微信号
        $wechat_otp     微信令牌口令 (out)
    */
    public function get_wechat_otp($openid, &$wechat_otp)
    {
        if (empty($openid))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"openid":"' . $openid . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=getwechatotp&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
            if ( array_key_exists("wechatotp", $objs) )
            {
                $wechat_otp = $objs->wechatotp;
            }
            else
            {
                $code = OTP_JSON_PROCESS_FAILED;
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 微信推送认证
    参数： 
        $user_name      用户名
    */
    public function wechat_push_auth($user_name)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"userid":"' . $user_name . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=wechatpushauth&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain, HTTP_TIME_OUT);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        if (OTP_PUSHAUTH_PROCESS == $code)
        {
            if ( array_key_exists("reqid", $objs) )
            {
                $req_id = $objs->reqid;
                $code = $this->get_push_auth_result($req_id);
            }
            else
            {
                $code = OTP_JSON_PROCESS_FAILED;
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 微信推送认证响应 (公众号后台调用)
    参数： 
        $openid         微信号
        $req_id         请求ID，云信服务器推送过来的
        $result         响应结果 ：0拒绝登录 1确认登录
    */
    public function wechat_push_auth_response($req_id, $openid, $result)
    {
        if (empty($req_id) || empty($openid) || $result == "")
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"reqid":"' . $req_id . '",' . '"openid":"' . $openid . '",' . '"result":"' . $result . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=wechatpushauthresponse&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 获取微信扫码认证信息 (公众号后台调用，公众号关注者触发)
    参数： 
        $openid         微信号
        $qr_code        二维码字符串
        $user_name      用户名 (out)
        $app_name       应用名称 (out)
        $req_id         请求ID (out)
    */
    public function get_wechat_scan_auth_info($openid, $qr_code, &$user_name, &$app_name, &$req_id)
    {
        if (empty($openid) || empty($qr_code))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //处理二维码
        $code = $this->deal_qr_code($qr_code, $req_id);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"openid":"' . $openid . '",' . '"reqid":"' . $req_id . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=getwechatscanauthinfo&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        do 
        {
            if ( array_key_exists("userid", $objs) )
            {
                $user_name = $objs->userid;
            }
            else
            {
                $code = OTP_JSON_PROCESS_FAILED;
                break;
            }
            if ( array_key_exists("appname", $objs) )
            {
                $app_name = $objs->appname;
            }
            else
            {
                $code = OTP_JSON_PROCESS_FAILED;
                break;
            }
            if ( array_key_exists("reqid", $objs) )
            {
                $req_id = $objs->reqid;
            }
            else
            {
                $code = OTP_JSON_PROCESS_FAILED;
            }
        }
        while(0);

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 微信扫码认证响应 (公众号后台调用)
    参数： 
        $req_id         请求ID，通过 get_wechat_scan_authinfo() 获取
        $user_name      用户名，通过 get_wechat_scan_authinfo() 获取
        $result         响应结果 ：0拒绝登录 1确认登录
    */
    public function wechat_scan_auth_response($req_id, $user_name, $result)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($req_id) || empty($user_name) || $result == "")
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"reqid":"' . $req_id . '",' . '"userid":"' . $user_name  . '",' . '"result":"' . $result . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=wechatscanauthresponse&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 微信语音推送认证
    参数： 
        $user_name      用户名
    */
    public function wechat_voice_auth($user_name)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"userid":"' . $user_name . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=wechatvoiceauth&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain, HTTP_TIME_OUT);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        if (OTP_PUSHAUTH_PROCESS == $code)
        {
            if ( array_key_exists("reqid", $objs) )
            {
                $req_id = $objs->reqid;
                $code = $this->get_push_auth_result($req_id);
            }
            else
            {
                $code = OTP_JSON_PROCESS_FAILED;
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 微信语音推送认证响应 (公众号后台调用)
    参数： 
        $openid         微信号
        $req_id         请求ID，云信服务器推送过来的
        $result         响应结果 ：0拒绝登录 1确认登录
    */
    public function wechat_voice_auth_response($req_id, $openid, $result)
    {
        if (empty($req_id) || empty($openid) || $result == "")
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"reqid":"' . $req_id . '",' . '"openid":"' . $openid . '",' . '"result":"' . $result . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=wechatpushauthresponse&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 人脸引擎注册
    参数： 
        $user_name      用户名
        $phone_sn       手机号 (可以为空)
    */
    public function face_engine_regist($user_name, $phone_sn)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        if (empty($phone_sn))
        {
            $data_plain = '{"userid":"' . $user_name . '",' . '"state":"' . $state . '"}';
        }
        else
        {
            $data_plain = '{"userid":"' . $user_name . '",' . '"phonesn":"' . $phone_sn . '",' . '"state":"' . $state . '"}';
        }
        $request = $this->server_url . '?method=faceengineregist&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 人脸引擎认证
    参数： 
        $user_name      用户名
        $phone_sn       手机号 (可以为空)
    */
    public function face_engine_auth($user_name, $phone_sn)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        if (empty($phone_sn))
        {
            $data_plain = '{"userid":"' . $user_name . '",' . '"state":"' . $state . '"}';
        }
        else
        {
            $data_plain = '{"userid":"' . $user_name . '",' . '"phonesn":"' . $phone_sn . '",' . '"state":"' . $state . '"}';
        }
        $request = $this->server_url . '?method=faceengineauth&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 获取人脸引擎操作结果
    参数： 
        $user_name      用户名
    */
    public function get_process_result($user_name)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"userid":"' . $user_name . '",' . '"reqid":"' . $user_name . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=queryauthresult&access_token=' . $this->access_token;

        $code = $this->deal_http_lots($data_plain, $request, $objs, HTTP_TIME_OUT);
        if (null != $objs)
        {
            if (OTP_OK == $code)
            {
                if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
                {
                    return $this->deal_err(OTP_INVALID_STATE, __function__);
                }
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 获取人脸认证ID
    参数： 
        $user_name      用户名
        $authid         认证ID (out)
    */
    public function get_face_authid($user_name, &$authid)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"userid":"' . $user_name . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=getfaceauthid&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain, HTTP_TIME_OUT);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
            if ( array_key_exists("authid", $objs) )
            {
                $authid = $objs->authid;
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 人脸注册
    参数： 
        $user_name      用户名
        $phone_sn       手机号
        $face_data      人脸数据 格式: urlEncode({"face":[{"authid":1,"face_data":"base64格式数据"},…],"result":"0失败1成功2活体检测失败"}) 其中的 authid 项可为空
    */
    public function face_regist($user_name, $phone_sn, $face_data)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name) ||  empty($phone_sn) || empty($face_data))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"userid":"' . $user_name . '",' . '"phonesn":"' . $phone_sn . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=faceregist&access_token=' . $this->access_token;

        //POST数据加密
        $this->encrypt_post($face_data, $face_data_ciper_hex);
        $data_post = "facedata=" . $face_data_ciper_hex;
        //$data_post = "facedata=" . $face_data;

        //http处理
        $code = $this->deal_http_post($data_plain, $request, $data_post, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 人脸认证
    参数： 
        $user_name      用户名
        $face_data      人脸数据 格式: urlEncode({"face_s":80,"face_data":"base64格式数据"}) ，其中的 face_s 项可为空

    */
    public function face_auth($user_name, $face_data)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name) || empty($face_data))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"userid":"' . $user_name . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=faceauth&access_token=' . $this->access_token;

        //POST数据加密
        $this->encrypt_post($face_data, $face_data_ciper_hex);
        $data_post = "facedata=" . $face_data_ciper_hex;
        //$data_post = "facedata=" . $face_data;

        //http处理
        $code = $this->deal_http_post($data_plain, $request, $data_post, $json_plain);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 根据用户名进行短信push认证，此接口发送短信链接，由用户点击链接完成认证
    参数： 
        $user_name      用户名
        $phone_sn       手机号 (可以为空)
    */
    public function sms_push_auth($user_name, $phone_sn)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        if (empty($phone_sn))
        {
            $data_plain = '{"userid":"' . $user_name . '",' . '"state":"' . $state . '"}';
        }
        else
        {
            $data_plain = '{"userid":"' . $user_name . '",' . '"phonesn":"' . $phone_sn . '",' . '"state":"' . $state . '"}';
        }
        $request = $this->server_url . '?method=smspushauth&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain, HTTP_TIME_OUT);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
        }

        if (OTP_PUSHAUTH_PROCESS == $code)
        {
            if ( array_key_exists("reqid", $objs) )
            {
                $req_id = $objs->reqid;
                $code = $this->get_push_auth_result($req_id);
            }
            else
            {
                $code = OTP_JSON_PROCESS_FAILED;
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 获取手机令牌激活数据
    参数： 
        $user_name      用户名
        $phone_sn       手机号
        $version        版本信息，不允许为空 格式: urlEncode({"version":"","os":0}) 
        $activedata     激活数据 (out)
    */
    public function get_mobile_token_activedata($user_name, $phone_sn, $version, &$activedata)
    {
        $user_name = ltrim(rtrim($user_name));
        if (empty($user_name) || empty($phone_sn) || empty($version))
        {
            return $this->deal_err(OTP_PARAM_INVALID, __function__);
        }

        //检查有效时间
        $code = $this->check_expire();
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //业务数据
        $state = strval($this->get_random());
        $data_plain = '{"userid":"' . $user_name . '",' . '"phonesn":"' . $phone_sn  . '",' . '"version":"' . $version  . '",' . '"state":"' . $state . '"}';
        $request = $this->server_url . '?method=getmobiletokenactivedata&access_token=' . $this->access_token;

        //http处理
        $code = $this->deal_http($data_plain, $request, $json_plain, HTTP_TIME_OUT);
        if (OTP_OK != $code)
        {
            return $this->deal_err($code, __function__);
        }

        //解析json数据
        $objs = json_decode($json_plain);
        if (null == $objs)
        {
            return $this->deal_err(OTP_JSON_PROCESS_FAILED, __function__);
        }
        $code = $objs->code;
        //$msg = $objs->msg;
        if (OTP_OK == $code)
        {
            if ( !array_key_exists("state", $objs) || 0 != strcmp($state, $objs->state))
            {
                return $this->deal_err(OTP_INVALID_STATE, __function__);
            }
            if ( array_key_exists("activedata", $objs) )
            {
                $activedata = json_encode($objs->activedata);
            }
        }

        return $this->deal_err($code, __function__);
    }

    /*
    名字： 获取错误描述
    参数： 
        $code           错误码
        $err_msg        错误描述  (out)
    */
    public function get_err_msg($code, &$err_msg)
    {
        $err_msg = $this->get_err_msg_all($code);
    }
}
?>
