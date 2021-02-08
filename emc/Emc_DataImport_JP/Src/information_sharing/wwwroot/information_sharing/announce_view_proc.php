<?php
    /*
     * @announce_view_proc.php
     *
     * @create 2020/03/13 AKB Chien
     * @update
     */

    require_once('common/common.php');
    require_once('common/session_check.php');

    header('Content-type: text/html; charset=utf-8');
    header('X-FRAME-OPTIONS: DENY');

    // check timeout if direct access this file
    fncSessionTimeOutCheck(1);

    // check connection
    if(fncConnectDB() == false) {
        $_SESSION['LOGIN_ERROR'] = 'DB接続に失敗しました。';
        header('Location: login.php');
        exit;
    }

    // Check if the user logged in or not
    if(!isset($_SESSION['LOGINUSER_INFO'])) {
        $strShow = '<script>alert("'.PUBLIC_MSG_008_JPN.' / '.PUBLIC_MSG_008_ENG.'");';
        $strShow .= ' window.location.href="login.php";</script> ';
        echo $strShow;
        exit;
    }

    // 2020/03/26 AKB Chien - start - update document 2020/03/26
    // ログインユーザ情報を取得
    $objLoginUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);
    $intLanguageType = $objLoginUserInfo->intLanguageType;

    $arrTitleMsg = array('PUBLIC_MSG_049');

    // get list text(header, title, msg) with languague_type of user logged
    $arrTxtTrans = getListTextTranslate($arrTitleMsg, $intLanguageType);
    // GET通信にて遷移してきた場合、以下のメッセージをアラート表示し、遷移元画面に戻す。
    fncGetRequestCheck($arrTxtTrans);
    // 2020/03/26 AKB Chien - end - update document 2020/03/26

    // 2020/04/01 AKB Chien - start - update document 2020/04/01
    // GET通信にて遷移してきた場合、以下のメッセージをアラート表示し、遷移元画面に戻す。
    if(!isset($_SERVER['HTTP_REFERER'])) {
        echo '<script type="text/javascript">
                function goBack() {
                    history.go(-1);
                    return false;
                }
                alert("'.$arrTxtTrans['PUBLIC_MSG_049'].'");
                goBack();
            </script>';
        die();
    }
    // 2020/04/01 AKB Chien - end - update document 2020/04/01

    // check if ajax -> do something | access this file directly -> stop
    if(!(isset($_POST['mode']) && $_POST['mode'] == 99) &&
        !(!empty($_SERVER['HTTP_X_REQUESTED_WITH'])
        && strtolower($_SERVER['HTTP_X_REQUESTED_WITH']) == 'xmlhttprequest')) {
        exit;
    }

    if(isset($_POST)) {
        if(isset($_POST['action'])) {
            // ①のファイルが存在するか確認する。
            if($_POST['action'] == 'checkFile') {
                $strFilePath = SHARE_FOLDER.'/'.ANNOUNCE_ATTACHMENT_FOLDER;
                $strFilePath .= '/'.base64_decode($_POST['file']);
                // check file exist
                if(is_file($strFilePath)) {
                    echo base64_encode($strFilePath);
                } else {
                    echo 0;
                }
            }
        }

        if(isset($_POST['mode'])) {
            // mode download
            if($_POST['mode'] == 99) {
                $strFilePath = SHARE_FOLDER.'/'.ANNOUNCE_ATTACHMENT_FOLDER;
                $strFilePath .= '/'.base64_decode($_POST['path']);

                // download file with path
                fncDownloadFile($strFilePath);
            }
        }
    }
?>
