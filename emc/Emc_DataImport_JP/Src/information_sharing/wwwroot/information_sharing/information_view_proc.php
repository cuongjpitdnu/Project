<?php
    /*
     * @information_view_proc.php
     *
     * @create 2020/04/09 AKB Chien
     * @update
     */
    require_once('common/common.php');
    require_once('common/session_check.php');

    header('Content-type: text/html; charset=utf-8');
    header('X-FRAME-OPTIONS: DENY');

    define('SCREEN_NAME', '情報表示画面');

    // check connection
    if(fncConnectDB() == false) {
        $_SESSION['LOGIN_ERROR'] = 'DB接続に失敗しました。';
        header('Location: login.php');
        exit;
    }

    // Check if the user logged in or not
    if(!isset($_SESSION['LOGINUSER_INFO'])) {
        $strShow = 'alert("'.PUBLIC_MSG_008_JPN.' / '.PUBLIC_MSG_008_ENG.'");';
        $strShow .= ' window.location.href="login.php";';
        echo $strShow;
        exit;
    }

    // check timeout if direct access this file
    fncSessionTimeOutCheck(1);

    // check if ajax -> do something | access this file directly -> stop
    if(!(!empty($_SERVER['HTTP_X_REQUESTED_WITH'])
        && strtolower($_SERVER['HTTP_X_REQUESTED_WITH']) == 'xmlhttprequest')) {
        if(!isset($_POST['mode']))
        exit;
    }

    $objUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);

    // get list text translate
	$arrTitle =  array(
		'PUBLIC_MSG_004',
		'PUBLIC_MSG_049',
    );

    // get list text(header, title, msg) with languague_type of user logged
	$arrTextTranslate = getListTextTranslate(
		$arrTitle,
		$objUserInfo->intLanguageType
	);

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

        if(isset($_POST['mode'])) {
            // mode download file
            if($_POST['mode'] == 99) {
                $strFilePath = base64_decode($_POST['path']);

                // download file with path
                fncDownloadFile($strFilePath);
            }

            // delete information
            if($_POST['mode'] == 1) {
                // write log
                $strLog = SCREEN_NAME.'　削除 (ユーザID = '.$objUserInfo->strUserID.') ';
                fncWriteLog(LogLevel['Info'] , LogPattern['Button'], $strLog);

                if(($objUserInfo->intInformationRegPerm == 1 &&
                    $objUserInfo->intCompanyNo == $_POST['COMPANY_NO'])
                    || $objUserInfo->intMenuPerm == 1) {

                    }else{
                        echo 900;
                        exit();
                    }

                try{
                    // sql delete information
                    $strSQL = '';
                    $strSQL .= 'DELETE FROM t_information
                                WHERE INFORMATION_NO = :INFORMATION_NO';
                    $objQuery = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
                    $objQuery->bindParam(':INFORMATION_NO', $_POST['id']);
                    //sql log
                    $strSqlLog = str_replace(':INFORMATION_NO', $_POST['id'], $strSQL);

                    // write log sql
                    fncWriteLog(
                        LogLevel['Info'] ,
                        LogPattern['Sql'],
                        SCREEN_NAME . ' '.$strSqlLog
                    );

                    $objQuery->execute();

                    // delete folder of information_no
                    $strPath = SHARE_FOLDER.'/'.INFORMATION_ATTACHMENT_FOLDER.'/'.$_POST['id'];
                    deleteFolderWithPath($strPath);

                    echo 1;
                    exit();
                }catch(\Exception $e) {
                    // write error log
                    fncWriteLog(
                        LogLevel['Error'],
                        LogPattern['Error'],
                        SCREEN_NAME . ' ' . $e->getMessage()
                    );
                    echo 0;
                    exit();
                }
            }
        }
    }

    /**
     * delete all file and folder in path
     *
     * @create 2020/03/13 AKB Chien
     * @update
     * @param string $strPath
     * @return boolean true:成功、false:失敗
     */
    function deleteFolderWithPath($strPath) {
        try {
            // check string input is path?
            if(is_dir($strPath)) {
                // get all file and folder
                $arrFileFolder = glob($strPath.'/*');
                if(count($arrFileFolder) > 0) {
                    foreach($arrFileFolder as $arrItem) {
                        if(!is_file($arrItem)) {
                            // get all file and folder
                            $strPathTemp = glob($arrItem.'/*');
                            // remove folder file_name1 -> 5
                            foreach($strPathTemp as $objFile) { // iterate files
                                if(is_file($objFile)) {
                                    unlink($objFile); // delete file
                                } else {
                                    // get all file and folder
                                    $strPathTemp_2 = glob($objFile.'/*');
                                    foreach($strPathTemp_2 as $objFile_2) {
                                        if(is_file($objFile_2)) {
                                            // delete each file in folder
                                            unlink($objFile_2);
                                        }
                                    }
                                    rmdir($objFile);    // delete folder
                                }
                            }
                            rmdir($arrItem);    // delete folder
                        } else {
                            unlink($arrItem);   // delete file
                        }
                    }
                    rmdir($strPath);
                }
            }
            return true;
        } catch (\Exception $e) {
            return false;
        }
        return true;
    }
?>