<?php
/*
    微信令牌示例
    2015 飞天诚信云信事业部
*/
require_once('cloudentifysdk.php');

header("Content-type:text/html;charset=utf-8");
date_default_timezone_set ('Asia/Shanghai');


//微信Token
define("TOKEN", "weixin");

//微信应用信息
define('APP_ID', 'wx4e8676f6957e0353');
define('APP_SECRET', 'a29614a32a2b5bb1711f886aad3731ce');

//云信应用信息
define('Cloudentify_Server_URL', 'http://192.168.16.101:1880');
define('Cloudentify_APP_ID', '8E1E72680E21EBF0C28D');
define('Cloudentify_APP_SECRET', 'E14B0351099B6CABB9616FF40963943993B75C06');

//公众号类对象
$wechat_serv = new wechatService(APP_ID, APP_SECRET);
$wechat_serv->initCloudentify(Cloudentify_Server_URL, Cloudentify_APP_ID, Cloudentify_APP_SECRET);

//验证签名
if (isset($_GET['echostr']))
{
    $wechat_serv->valid();
}
//创建公众号目录
else if (isset($_GET['createmenu']))
{
    $ret = $wechat_serv->getAccessToken($access_token);
    print_r($access_token . "<br>");
    $result = $wechat_serv->createMenu($access_token);
    print_r("createmenu() result: " . $result . "<br>");
}
//云信推送处理
else if (isset($_GET['data']))
{
    $wechat_serv->logger("processCloudentify() begin");
    $result = $wechat_serv->processCloudentify();
    print_r("processCloudentify() result: " . $result . "<br>");
}
//云信推送认证响应消息
else if (isset($_GET['req_type']))
{
    $wechat_serv->logger("responsePushAuth() begin");
    $result = $wechat_serv->responsePushAuth();
    print_r("responsePushAuth() result: " . $result . "<br>");
}
//云信扫码登陆响应消息
else if (isset($_GET['scan_login']))
{
    $wechat_serv->logger("responseScanLogin()  begin");
    $result = $wechat_serv->responseScanLogin();
    print_r("responseScanLogin() result: " . $result . "<br>");
}
//公众号响应消息
else
{
    $wechat_serv->logger("responseMsg() begin");
    $wechat_serv->responseMsg();
}


//微信服务
class wechatService
{
    private $app_id; 
    private $app_secret; 

    private $cloudentify_sdk; 

    //构造函数
    public function __construct($app_id, $app_secret) 
    { 
        //微信应用信息
        $this->app_id = $app_id; 
        $this->app_secret = $app_secret; 

        $this->cloudentify_sdk = new cloudentifysdk();
    } 
    //析构函数
    public function __destruct()
    {
        $this->cloudentify_sdk->uninit();
    }

    //初始化云信
    public function initCloudentify($server_url, $app_id, $app_secret) 
    {
        $ret = OTP_OK;
        //使用云信应用信息，进行云信SDK初始化
        $ret = $this->cloudentify_sdk->init($server_url, $app_id, $app_secret);
        return $ret;
    }

    //验证签名
	public function valid() {
		$echoStr = $_GET["echostr"];
		if($this->checkSignature()){
			echo $echoStr;
			exit;
		}
	}
    //验证签名
	private function checkSignature() {
		$signature = $_GET["signature"];
		$timestamp = $_GET["timestamp"];
		$nonce = $_GET["nonce"];
		$token = TOKEN;
		$tmpArr = array($token, $timestamp, $nonce);
		sort($tmpArr);
		$tmpStr = implode( $tmpArr );
		$tmpStr = sha1( $tmpStr );
		if( $tmpStr == $signature ) {
			return true;
		} else {
			return false;
		}
	}

    //GET获取https页面 
    private function https_get($url, &$result)
    {
        $ch = curl_init();

        curl_setopt($ch, CURLOPT_URL, $url);
        curl_setopt($ch, CURLOPT_REFERER, $url);
        curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, FALSE);
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, TRUE);
        curl_setopt($ch, CURLOPT_HEADER, false);
        curl_setopt($ch, CURLOPT_FOLLOWLOCATION, true);
        $result = curl_exec($ch);
        //print_r('==== ==== result: ' . $result . "<br>");

        $curl_code = curl_errno($ch);
        //$curl_str = 'Errno' . curl_error($ch);
        //print_r('==== ==== curl_code: ' . $curl_code . "<br>");

        curl_close($ch);
        
        return $curl_code;
    }

    //POST获取https
    private function https_post($url, $data, &$result)
    {
        $ch = curl_init();

        curl_setopt($ch, CURLOPT_URL, $url); 
        curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, FALSE);
        curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, FALSE);
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
        curl_setopt($ch, CURLOPT_POST, 1);
        curl_setopt($ch, CURLOPT_POSTFIELDS, $data);
        $result = curl_exec($ch);
        //print_r('==== ==== result: ' . $result . "<br>");

        $curl_code = curl_errno($ch);
        //$curl_str = 'Errno' . curl_error($ch);
        //print_r('==== ==== curl_code: ' . $curl_code . "<br>");

        curl_close($ch);

        return $curl_code;

        // $ch = curl_init(); 
        // curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, FALSE);
        // curl_setopt($ch,CURLOPT_URL,$url); 
        // curl_setopt($ch,CURLOPT_RETURNTRANSFER,1); 
        // curl_setopt($ch,CURLOPT_POST,1); 
        // curl_setopt($ch,CURLOPT_POSTFIELDS,$jsondata); 
        // $result = curl_exec($ch); 
        // $curl_code = curl_errno($ch);
        // print_r('==== ==== curl_code: ' . $curl_code . "<br>");
        // curl_close($ch);
    }

    //获取access_token 
    public function getAccessToken(&$access_token) 
    { 
        $url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" . $this->app_id . "&secret=" . $this->app_secret; 

        $ret = $this->https_get($url, $result);

        $objs = json_decode($result,true);
        if ($objs['access_token'])
        {
            $access_token = $objs['access_token']; 
        }

        return $ret;
    }

    //创建菜单 
    public function createMenu($access_token) 
    { 
        $url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" . $access_token; 
        $arr = array(  
            'button' =>array( 
                array( 
                    'name'=>urlencode("扫码登录"), 
                    'type'=>'scancode_waitmsg', 
                    'key'=>'SCAN_SCAN_LOGIN'
                ), 
                array( 
                    'name'=>urlencode("获取口令"), 
                    'type'=>'click', 
                    'key'=>'CLICK_GET_OTP'
                ), 
                array( 
                    'name'=>urlencode("服务"), 
                    'sub_button'=>array( 
                        array( 
                            'name'=>urlencode("关于"), 
                            'type'=>'click', 
                            'key'=>'CLICK_ABOUT' 
                        ), 
                        array( 
                            'name'=>urlencode("账号信息"), 
                            'type'=>'click', 
                            'key'=>'CLICK_ACCOUNT_INFO' 
                        ),
                        array( 
                            'name'=>urlencode("开通服务"), 
                            'type'=>'scancode_waitmsg', 
                            'key'=>'SCAN_OPEN_SERVICE' 
                        ),                         
                    ) 
                ) 
            ) 
        );

        $jsondata = urldecode(json_encode($arr));

        $curl_code = $this->https_post($url, $jsondata, $result);

        return $result;
    }

    //云信推送处理
    public function processCloudentify()
    {
        $data = $_GET['data'];

        $objs = json_decode($data);

        $req_type = $objs->reqtype;

        $req_id = $objs->reqid;
        $openid = $objs->openid;
        $time = $objs->time;

        $user_name = $objs->userid;
        $app_name = $objs->appname;

        //传递参数
        $url_base = 'http://' . $_SERVER['HTTP_HOST'] . $_SERVER['PHP_SELF'];
        $url_base .= "?req_id=" . $req_id;
        $url_base .= "&openid=" . $openid;
        $url_base .= "&req_type=" . $req_type;
        //确认、取消
        $url_accept = $url_base . "&result=1";
        $url_reject = $url_base . "&result=0";
        //多图文信息
        $content = array();
        $content[] = array(
                        "Title"=>$user_name . "[推送认证]" . $app_name . "，请确认是否本人登录:", 
                        "Description"=>"", 
                        "PicUrl"=>"http://weixin.cloudentify.com/wxtoken/image/1.jpg", 
                        "Url" =>""
                        );
        $content[] = array(
                        "Title"=>"确认", 
                        "Description"=>"", 
                        "PicUrl"=>"", 
                        "Url" =>"$url_accept"
                        );
        $content[] = array(
                        "Title"=>"取消", 
                        "Description"=>"", 
                        "PicUrl"=>"", 
                        "Url" =>"$url_reject"
                        ); 

        $ret = $this->sendNews($openid, $content);

        return $ret;
    }

    //云信推送认证响应消息
    public function responsePushAuth()
    {
        $req_type = $_GET['req_type'];

        $openid = $_GET['openid'];

        $req_id = $_GET['req_id'];
        $result = $_GET['result'];

        $str = "";
        $ret = $this->cloudentify_sdk->wechat_push_auth_response($req_id, $openid, $result);
        if (OTP_OK == $ret)
        {
            $str = "云信推送认证响应成功！";
        }
        else
        {
            $str = "req_id: " . $req_id . "\n出错了，返回值：" . $ret;
        }

        //发送提示
        if ($result == "1")
        {
            $content = "您选择了[确认]\n";
        }
        else
        {
            $content = "您选择了[取消]\n";
        }
        $content .= $str;
        //$this->sendText($openid, $content);

        return $str;
    }

    //公众号响应消息
    public function responseMsg()
    {
        $postStr = $GLOBALS["HTTP_RAW_POST_DATA"];
        if (!empty($postStr))
        {
            $this->logger("responseMsg() postStr: ".$postStr);

            $postObj = simplexml_load_string($postStr, 'SimpleXMLElement', LIBXML_NOCDATA);
            $RX_TYPE = trim($postObj->MsgType);

            //消息类型分离
            switch ($RX_TYPE)
            {
                case "event":
                    $result = $this->receiveEvent($postObj);
                    break;
                case "text":
                    $result = $this->receiveText($postObj);
                    break;
                default:
                    $result = "not support msg type: ".$RX_TYPE;
                    break;
            }

            $this->logger("responseMsg() result: " . $result);
            echo $result;
        }
        else
        {
            echo "";
            exit;
        }
    }

    //接收事件消息
    private function receiveEvent($object)
    {
        $content = "";
        switch ($object->Event)
        {
            case "subscribe":
                $content = "欢迎关注\n飞天诚信云信事业部";
                $content .= (!empty($object->EventKey))? ("\n来自二维码场景 " . str_replace("qrscene_", "", $object->EventKey)) : "";
                break;
            case "unsubscribe":
                $content = "取消关注";
                break;

            case "scancode_waitmsg":
                $this->dealScancodeWaitmsg($object, $content);
                break;

            case "CLICK":
                $this->dealClick($object, $content);
                break;

            default:
                $content = "receive a new event: ".$object->Event;
                break;
        }

        if(is_array($content))
        {
            $result = $this->transmitNews($object, $content);
        }
        else
        {
            $result = $this->transmitText($object, $content);
        }

        return $result;
    }

    //扫码推事件
    private function dealScancodeWaitmsg($object, &$content)    
    {
        switch ($object->EventKey)
        {
            case "SCAN_OPEN_SERVICE":
                $openid = $object->FromUserName;
                $ScanCodeInfo = $object->ScanCodeInfo;
                $wechat_ac = $ScanCodeInfo->ScanResult;
                $ret = $this->cloudentify_sdk->active_wechat_token($openid, $wechat_ac);
                if (OTP_OK == $ret)
                {
                    $content = "开通成功!";
                }
                else
                {
                    $content = "openid: " . $object->FromUserName . "\nwechat_ac:" . $wechat_ac . "\n出错了，返回值：" . $ret;
                }
                break;
            case "SCAN_SCAN_LOGIN":
                $openid = $object->FromUserName;
                $ScanCodeInfo = $object->ScanCodeInfo;
                $qr_code = $ScanCodeInfo->ScanResult;
                $ret = $this->cloudentify_sdk->get_wechat_scan_auth_info($openid, $qr_code, $user_name, $app_name, $req_id);
                if (OTP_OK == $ret)
                {
                    //传递参数
                    $url_base = 'http://' . $_SERVER['HTTP_HOST'] . $_SERVER['PHP_SELF'];
                    $url_base .= "?req_id=" . $req_id . "&user_name=" . $user_name;
                    $url_base .= "&openid=" . $openid;
                    $url_base .= "&scan_login=1";
                    //确认、取消
                    $url_accept = $url_base . "&result=1";
                    $url_reject = $url_base . "&result=0";
                    //多图文信息
                    $content = array();
                    $content[] = array(
                                    "Title"=>$user_name . "[扫码登录]" . $app_name . "，请确认是否本人登录:", 
                                    "Description"=>"", 
                                    "PicUrl"=>"http://weixin.cloudentify.com/wxtoken/image/1.jpg", 
                                    "Url" =>""
                                    );
                    $content[] = array(
                                    "Title"=>"确认", 
                                    "Description"=>"", 
                                    "PicUrl"=>"", 
                                    "Url" =>"$url_accept"
                                    );
                    $content[] = array(
                                    "Title"=>"取消", 
                                    "Description"=>"", 
                                    "PicUrl"=>"", 
                                    "Url" =>"$url_reject"
                                    );
                }
                else
                {
                    $content = "openid: " . $object->FromUserName . "\nqr_code:" . $qr_code . "\n出错了，返回值：" . $ret;
                }
                break;
            default:
                $content = "扫描场景：" . $object->EventKey . "\n飞天诚信云信事业部";
                break;
        }
    }

    //点击事件
    private function dealClick($object, &$content)
    {
        switch ($object->EventKey)
        {
            case "CLICK_GET_OTP":
                $openid = $object->FromUserName;
                $wechat_otp = "";
                $ret = $this->cloudentify_sdk->get_wechat_otp($openid, $wechat_otp);
                if (OTP_OK == $ret)
                {
                    $content = $wechat_otp . "[有效期60秒]";
                }
                else
                {
                    $content = "openid: " . $object->FromUserName . "\n出错了，返回值：" . $ret;
                }
                break;
            case "CLICK_ACCOUNT_INFO":
                $openid = $object->FromUserName;
                $ret = $this->cloudentify_sdk->get_userinfo_by_wechat($openid, $user_name, $token_sn, $company_name);
                if (OTP_OK == $ret)
                {
                    $content = "企业名称：" . $company_name;
                    $content .= "\n用户名称：" . $user_name;
                    $content .= "\n令 牌 号：" . $token_sn;
                }
                else
                {
                    $content = "openid: " . $object->FromUserName . "\n出错了，返回值：" . $ret;
                }
                break;
            case "CLICK_ABOUT":
                $content = "微信令牌示例\n飞天诚信云信事业部";
                break;
            default:
                $content = "点击菜单：" . $object->EventKey . "\n飞天诚信云信事业部";
                break;
        }
    }

    //云信扫码登陆响应消息
    public function responseScanLogin()
    {
        $scan_login = $_GET['scan_login'];
        $openid = $_GET['openid'];

        $req_id = $_GET['req_id'];
        $user_name = $_GET['user_name'];
        $result = $_GET['result'];

        $str = "";
        $ret = $this->cloudentify_sdk->wechat_scan_auth_response($req_id, $user_name, $result);
        if (OTP_OK == $ret)
        {
            $str = "云信扫码登陆响应成功！";
        }
        else
        {
            $str = "req_id: " . $req_id . "\n出错了，返回值：" . $ret;
        }

        //发送提示
        if ($result == "1")
        {
            $content = "您选择了[确认]\n";
        }
        else
        {
            $content = "您选择了[取消]\n";
        }
        $content .= $str;
        //$this->sendText($openid, $content);

        return $str;
    }

    //发送文本消息
    private function sendText($openid, $content)
    {
        $data = '{
"touser":"' .$openid . '",
"msgtype":"text",
"text":{
"content":"' . $content . '"
}}';

        $ret = $this->getAccessToken($access_token);
        $url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" . $access_token;

        $curl_code = $this->https_post($url, $data, $result);
        
        $this->logger("sendText() data: " . $data);
        $this->logger("sendText() result: " . $result);

        return $curl_code;
    }

    //发送图文消息
    private function sendNews($openid, $content)
    {
        $itemTpl = '{
"title":"%s",
"description":"%s",
"picurl":"%s",
"url":"%s"
},';
        $item_str = "";
        foreach ($content as $item){
            $item_str .= sprintf($itemTpl, $item['Title'], $item['Description'], $item['PicUrl'], $item['Url']);
        }

        $data = '{
"touser":"' . $openid . '",
"msgtype":"news",
"news":{
"articles": [' . $item_str . 
']}}';

        $ret = $this->getAccessToken($access_token);
        $url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" . $access_token;

        $curl_code = $this->https_post($url, $data, $result);

        $this->logger("sendNews() data: " . $data);
        $this->logger("sendNews() result: " . $result);

        return $curl_code;
    }

    //接收文本消息
    private function receiveText($object)
    {
        $content = date("Y-m-d H:i:s", time()) . "\n" . $object->FromUserName . "\n您好，飞天诚信云信事业部";
        $result = $this->transmitText($object, $content);

        return $result;
    }

    //回复文本消息
    private function transmitText($object, $content)
    {
        $xmlTpl = "<xml>
<ToUserName><![CDATA[%s]]></ToUserName>
<FromUserName><![CDATA[%s]]></FromUserName>
<CreateTime>%s</CreateTime>
<MsgType><![CDATA[text]]></MsgType>
<Content><![CDATA[%s]]></Content>
</xml>";
        $result = sprintf($xmlTpl, $object->FromUserName, $object->ToUserName, time(), $content);
        return $result;
    }

    //回复图文消息
    private function transmitNews($object, $newsArray)
    {
        if(!is_array($newsArray)){
            return;
        }
        $itemTpl = "<item>
<Title><![CDATA[%s]]></Title>
<Description><![CDATA[%s]]></Description>
<PicUrl><![CDATA[%s]]></PicUrl>
<Url><![CDATA[%s]]></Url>
</item>";
        $item_str = "";
        foreach ($newsArray as $item){
            $item_str .= sprintf($itemTpl, $item['Title'], $item['Description'], $item['PicUrl'], $item['Url']);
        }
        $xmlTpl = "<xml>
<ToUserName><![CDATA[%s]]></ToUserName>
<FromUserName><![CDATA[%s]]></FromUserName>
<CreateTime>%s</CreateTime>
<MsgType><![CDATA[news]]></MsgType>
<ArticleCount>%s</ArticleCount>
<Articles>$item_str</Articles>
</xml>";

        $result = sprintf($xmlTpl, $object->FromUserName, $object->ToUserName, time(), count($newsArray));
        return $result;
    }

    //日志记录
    public function logger($log_content)
    {
        $max_size = 10000;
        $log_filename = "log.xml";
        if(file_exists($log_filename) and (abs(filesize($log_filename)) > $max_size))
        {
            unlink($log_filename);
        }
        file_put_contents($log_filename, date('H:i:s') . " " . $log_content . "\r\n", FILE_APPEND);
    }
}
?>