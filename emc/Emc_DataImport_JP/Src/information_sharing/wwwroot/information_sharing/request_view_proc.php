<?php
    /*
     * @request_view_proc.php
     *
     * @create 2020/03/23 AKB Thang
     * @update
     */
    require_once('common/common.php');
    require_once('common/session_check.php');

    header('Content-type: text/html; charset=utf-8');
    header('X-FRAME-OPTIONS: DENY');
    //failed to connect database
    if(fncConnectDB() == false){
        $_SESSION['LOGIN_ERROR'] = 'DB接続に失敗しました。';
        header('Location: login.php');
        exit;
    }
    //not login
    if(!isset($_SESSION['LOGINUSER_INFO'])) {
        $strShow = 'alert("'.PUBLIC_MSG_008_JPN.' / '.PUBLIC_MSG_008_ENG.'");';
        $strShow .= ' window.location.href="login.php";';
        echo $strShow;
        exit;
    }
    //session timeout check
    fncSessionTimeOutCheck(1);
    // check if ajax -> do something | access this file directly -> stop
    if(!(!empty($_SERVER['HTTP_X_REQUESTED_WITH'])
        && strtolower($_SERVER['HTTP_X_REQUESTED_WITH']) == 'xmlhttprequest')) {
        //if not for downloading file, stop    
        if(!isset($_POST['mode']))
        exit;
    }

    //check file download

    if(isset($_POST)) {
        if(isset($_POST['action'])) {
            // ①のファイルが存在するか確認する。
            if($_POST['action'] == 'checkFile') {
                
                $strFilePath = base64_decode($_POST['file']);
                // check file exist
                if(is_file($strFilePath)) {
                    echo base64_encode($strFilePath);
                } else {
                    echo 0;
                }
            }
        }

        if(isset($_POST['mode']) && $_POST['mode'] == 99) {
            
            $strFilePath = base64_decode($_POST['path']);
            //call function to download file
            fncDownloadFile($strFilePath);
            
        }
    }
?>
