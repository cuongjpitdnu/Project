<?php

    /*
     * @link_edit_proc.php
     *
     * @create 2020/03/13 AKB Thang
     */


    require_once('common/common.php');
    require_once('common/session_check.php');
    header('Content-type: text/html; charset=utf-8');
    header('X-FRAME-OPTIONS: DENY');
    //failed to connect database
    if(fncConnectDB() == false) {
        $_SESSION['LOGIN_ERROR'] = 'DB接続に失敗しました。';
        header('Location: login.php');
        exit;
    }
    //not login
    if(!isset($_SESSION['LOGINUSER_INFO'])) {
        echo "
        <script>
            alert('".PUBLIC_MSG_008_JPN . "/" . PUBLIC_MSG_008_ENG."');
            window.location.href = 'login.php';
        </script>
        ";
        exit();
    }

    //sesstion check
    fncSessionTimeOutCheck(1);
    //create session to store error message
    $_SESSION['LINK_EDIT_MSG_ERROR_INPUT'] = '';

    //constant
    define('SCREEN_NAME', 'リンク情報登録編集画面');
    define('SCREEN_NAME_CREATE', 'リンク情報登録編集画面 登録');
    define('SCREEN_NAME_EDIT', 'リンク情報登録編集画面 更新');
    define('LINK_NAME_ENG_LENGTH', 100);
    define('LINK_NAME_JPN_LENGTH', 50);
    define('LINK_URL_LENGTH', 1000);
    define('LINK_FILE_NAME_LENGTH', 100);
    //get object user info
    $objUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);

    // get list text translate
    $arrTitle =  array(
        'LINK_EDIT_MSG_002',
        'LINK_EDIT_MSG_003',
        'LINK_EDIT_MSG_004',
        'LINK_EDIT_MSG_005',
        'LINK_EDIT_MSG_006',
        'LINK_EDIT_MSG_007',
        'LINK_EDIT_MSG_008',
        'LINK_EDIT_MSG_009',
        'LINK_EDIT_MSG_010',
        'LINK_EDIT_MSG_011',
        'LINK_EDIT_MSG_012',
        'LINK_EDIT_MSG_013',
        'PUBLIC_MSG_009',
        'PUBLIC_MSG_003',
        'PUBLIC_MSG_002',
        'PUBLIC_MSG_017',
        'PUBLIC_MSG_011',
        'PUBLIC_MSG_050',

        //2020/04/25 T.Masuda リンク情報が削除されていた場合のメッセージ
        'LINK_EDIT_MSG_014',
        //2020/04/25 T.Masuda
    );

    $arrTextTranslate = getListTextTranslate(
        $arrTitle,
        $objUserInfo->intLanguageType
    );

    //check get request
    fncGetRequestCheck($arrTextTranslate);

    // check request directly
    if(!isset($_SERVER['HTTP_REFERER'])) {
        echo '<script type="text/javascript">
                function goBack() {
                    // Check to see if override is needed here
                    // If no override needed, call history.back()
                    history.go(-1);
                    return false;
                }
                alert("'.$arrTextTranslate['PUBLIC_MSG_049'].'");
                goBack();
            </script>';
        die();
    }
    //redirect if dont have permission
    if($objUserInfo->intMenuPerm != 1) {
        echo "
        <script>
            alert('".$arrTextTranslate['PUBLIC_MSG_009']."');
            window.location.href = 'login.php';
        </script>
        ";
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
        //download file
        if(isset($_POST['mode'])) {
            if($_POST['mode'] == 99) {
                $strFilePath = base64_decode($_POST['path']);

                fncDownloadFile($strFilePath);
            }
        }
    }
    //check input
    if(isset($_POST['mode']) && $_POST['mode'] == 1) {
    if(isset($_POST['LINK_NO'])) {
        if($_POST['LINK_NO'] != '') {

            //log edit button
        	fncWriteLog(
                 LogLevel['Info'],
                 LogPattern['Button'],
                 SCREEN_NAME_EDIT . ' (ユーザID = '.$objUserInfo->strUserID.')'
            );
            //get array link
            $arrOne = fncSelectOne(
                "SELECT LINK_NO, BURNER_FILE_NAME1, LINK_CATEGORY_NO, SORT_NO
                FROM t_link WHERE LINK_NO=?",
                [$_POST['LINK_NO']],
                SCREEN_NAME
            );
        } else {
            //log register button
            fncWriteLog(
                 LogLevel['Info'],
                 LogPattern['Button'],
                 SCREEN_NAME_CREATE. ' (ユーザID = '.$objUserInfo->strUserID.')'
            );

            $arrOne = false;
        }

        if(!$arrOne && $_POST['LINK_NO'] != '') {
            //2020/04/25 T.Masuda 既に削除されていた場合
            fncWriteLog(
                LogLevel['Error'],
                LogPattern['Error'],
                SCREEN_NAME . ' ' . $arrTextTranslate['PUBLIC_MSG_003']
                );
            echo "alert('".$arrTextTranslate['LINK_EDIT_MSG_014']."'); ";
            echo "$('#myModal').modal('hide'); ";
            echo "setTimeout(function() {
                        window.location.reload();
                  }, 300);";
            exit;
            //2020/04/25 T.Masuda
        } else {

            if(!isset($_POST['LINK_CATEGORY_NO']) || $_POST['LINK_CATEGORY_NO']=='') {
                //link category not input
                $_SESSION['LINK_EDIT_MSG_ERROR_INPUT']
                .= $arrTextTranslate['LINK_EDIT_MSG_002'].'<br>';
            }

            if(!isset($_POST['LINK_NAME_JPN']) || $_POST['LINK_NAME_JPN']=='') {
                //link name japanese not input
                $_SESSION['LINK_EDIT_MSG_ERROR_INPUT']
                .= $arrTextTranslate['LINK_EDIT_MSG_003'].'<br>';
            } else {
                //link name japanese length not valid
                if(mb_strlen($_POST['LINK_NAME_JPN'])>LINK_NAME_JPN_LENGTH) {
                    $_SESSION['LINK_EDIT_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['LINK_EDIT_MSG_004'].'<br>';
                }
            }

            if(!isset($_POST['LINK_NAME_ENG']) || $_POST['LINK_NAME_ENG']=='') {
                //link name english not input
                $_SESSION['LINK_EDIT_MSG_ERROR_INPUT']
                .= $arrTextTranslate['LINK_EDIT_MSG_005'].'<br>';
            } else {
                //link name english length not valid
                if(mb_strlen($_POST['LINK_NAME_ENG'])>LINK_NAME_ENG_LENGTH) {
                    $_SESSION['LINK_EDIT_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['LINK_EDIT_MSG_006'].'<br>';
                }
            }

            if(!isset($_POST['URL']) || $_POST['URL']=='') {
                //link url not input
                $_SESSION['LINK_EDIT_MSG_ERROR_INPUT']
                .= $arrTextTranslate['LINK_EDIT_MSG_007'].'<br>';
            } else {
                //link url length not valid
                if(mb_strlen($_POST['URL'])>LINK_URL_LENGTH) {
                    $_SESSION['LINK_EDIT_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['LINK_EDIT_MSG_009'].'<br>';
                }
                //check halfsize & special character
                if(!fncCheckEngText($_POST['URL'])) {
                    $_SESSION['LINK_EDIT_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['LINK_EDIT_MSG_008'].'<br>';
                }

            }
            //check file

            if(isset($_FILES['file'])&&$_FILES['file']['name']!='') {
                if(
                    !isset($_FILES['file']['error'])
                    || is_array($_FILES['file']['error'])
                    || $_FILES['file']['size'] == 0
                ) {
                    //file not exist
                    $_SESSION['LINK_EDIT_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['LINK_EDIT_MSG_010'].'<br>';
                } else {
                    if(!file_exists($_FILES['file']['tmp_name'])) {
                        //file not exist
                        $_SESSION['LINK_EDIT_MSG_ERROR_INPUT']
                        .= $arrTextTranslate['LINK_EDIT_MSG_010'].'<br>';
                    } else {
                        //file name length not valid
                        if(mb_strwidth($_FILES['file']['name']) > LINK_FILE_NAME_LENGTH ) {
                            $_SESSION['LINK_EDIT_MSG_ERROR_INPUT']
                            .= $arrTextTranslate['LINK_EDIT_MSG_012'].'<br>';
                        }
                        //get image file array
                        $arrImageInfo = getimagesize($_FILES['file']['tmp_name']);
                        if(!is_array($arrImageInfo)) {
                            //file is not image
                            $_SESSION['LINK_EDIT_MSG_ERROR_INPUT']
                                .= $arrTextTranslate['LINK_EDIT_MSG_011'].'<br>';
                        } else {
                            $strImageType = $arrImageInfo[2];

                            //check width height
                            if($arrImageInfo[0] > BANNER_WIDTH_SIZE
                                || $arrImageInfo[1] > BANNER_VERTICAL_SIZE) {
                                $_SESSION['LINK_EDIT_MSG_ERROR_INPUT']
                                .= $arrTextTranslate['LINK_EDIT_MSG_013'].'<br>';
                            }

                            //file type must be jpeg and png, gif
                            if(!in_array($strImageType, array(IMAGETYPE_JPEG,IMAGETYPE_PNG,IMAGETYPE_GIF)))
                            {
                                $_SESSION['LINK_EDIT_MSG_ERROR_INPUT']
                                .= $arrTextTranslate['LINK_EDIT_MSG_011'].'<br>';
                            }
                        }
                        //check special char
                        preg_match(
                            '/[\\/\:\*\?\"\<\>\|\&\#\%\']+|((COM|com)[0-9])|(AUX|aux)|(CON|con)|((LPT|lpt)[0-9])|(NUL|nul)|(PRN|prn)/',
                            $_FILES['file']['name'],
                            $strFileNameErr
                        );
                        if(count($strFileNameErr) > 0) {
                            $_SESSION['LINK_EDIT_MSG_ERROR_INPUT'] .= $arrTextTranslate['PUBLIC_MSG_050'];
                        }


                    }

                }

            }

        }
    }
    if($_SESSION['LINK_EDIT_MSG_ERROR_INPUT'] != '') {
        //show error message
?>

$('.edit-error').html('<?php echo $_SESSION['LINK_EDIT_MSG_ERROR_INPUT']; ?>');

<?php
        } else {
            //no error occured, process to edit or create new file
            if(isset($_POST['errFile'])){
                //file not exist error, stop
                exit();
            }
            //get link category
            $arrLinkCategory = fncSelectOne("SELECT LINK_CATEGORY_NO
                FROM m_link_category
                WHERE LINK_CATEGORY_NO=?",
                [$_POST['LINK_CATEGORY_NO']],
                SCREEN_NAME
            );
            if($_POST['LINK_NO']!='') {
                //update link
                //if link category not exist, show error and write log
                if(!$arrLinkCategory) {
                    fncWriteLog(
                        LogLevel['Error'],
                        LogPattern['Error'],
                        SCREEN_NAME . ' ' . $arrTextTranslate['PUBLIC_MSG_003']
                    );
                    echo "$('.edit-error').html('".$arrTextTranslate['PUBLIC_MSG_003']."');";
                    exit();
                }
                $blnCheck = true;
                if(isset($_FILES['file']) && $_FILES['file']['name']!='') {
                    //file upload
                    $strFolderPath = SHARE_FOLDER.'/'.LINK_EDIT_FOLDER.'/'.$arrOne['LINK_NO'].'/1/';
                    if (!file_exists($strFolderPath)) {
                        //create folder path
                        mkdir($strFolderPath, 0777, true);
                    }
                    $files = glob($strFolderPath.'*'); // get all file names
                    foreach($files as $file) { // iterate files
                    if(is_file($file))
                        unlink($file); // delete file
                    }
                    if(
                        !move_uploaded_file(
                            $_FILES["file"]["tmp_name"]
                            ,$strFolderPath.basename($_FILES['file']['name'])
                        )
                    ) {
                        //can not upload file
                        $blnCheck = false;
                    }
                }


                //file upload successfully
                if ($blnCheck) {

                    //get link sort no
                    if($arrOne['LINK_CATEGORY_NO'] != $_POST['LINK_CATEGORY_NO']){
                        //get max sort_no link
                        $arrLastSortLink = fncSelectOne(
                            "SELECT SORT_NO FROM t_link WHERE LINK_CATEGORY_NO=? ORDER BY SORT_NO DESC",
                            [$_POST['LINK_CATEGORY_NO']],
                            SCREEN_NAME
                        );
                        if(is_array($arrLastSortLink) && count($arrLastSortLink) > 0) {
                            //$intSortNo = max sort no + 1
                            $intSortNo = $arrLastSortLink['SORT_NO'] + 1;
                        } else {
                            //there is no link from same category
                            $intSortNo = 1;
                        }
                    }
                    //update link
                    $objResult = fncProcessData([
                            'LINK_CATEGORY_NO' => $_POST['LINK_CATEGORY_NO'],
                            'LINK_NAME_JPN' => $_POST['LINK_NAME_JPN'],
                            'LINK_NAME_ENG' => $_POST['LINK_NAME_ENG'],
                            'URL' => $_POST['URL'],
                            'BURNER_FILE_NAME1' => isset($_POST['chkDelete'])
                            ? $_FILES['file']['name']
                            : (isset($_FILES['file']['name'])
                            ? $_FILES['file']['name'] : $arrOne['BURNER_FILE_NAME1']) ,
                            'UP_USER_NO'    =>  $objUserInfo->intUserNo,
                            'UP_DATE'   => date('Y-m-d H:i:s'),
                            'SORT_NO'   =>  isset($intSortNo) ? $intSortNo : $arrOne['SORT_NO']
                        ],
                        't_link',
                        'LINK_NO=?',
                        array($_POST['LINK_NO']),
                        SCREEN_NAME
                    );

                    if(is_object($objResult) && $objResult->rowCount()>0) {
                        //update successfully
                        echo 0;
                    } else {
                        //update failed, log error
                        fncWriteLog(
                            LogLevel['Error'],
                            LogPattern['Error'],
                            SCREEN_NAME . ' ' . $arrTextTranslate['PUBLIC_MSG_003']
                        );
                        //2020/04/25 T.Masuda 既に削除されていた場合
                        echo "alert('".$arrTextTranslate['LINK_EDIT_MSG_014']."'); ";
                        echo "$('#myModal').modal('hide'); ";
                        echo "setTimeout(function() {
                                window.location.reload();
                            }, 300);";
                        //2020/04/25 T.Masuda
                    }
                } else {
                    //file upload not success, log error and show errror message
                    fncWriteLog(
                        LogLevel['Error'],
                        LogPattern['Error'],
                        SCREEN_NAME . ' ' . $arrTextTranslate['PUBLIC_MSG_003']
                    );
                    echo "$('.edit-error').html('".$arrTextTranslate['PUBLIC_MSG_003']."');";
                }



            } else {
                //add new link

                //if link category not exist, show error and write log
                if(!$arrLinkCategory) {
                    fncWriteLog(
                        LogLevel['Error'],
                        LogPattern['Error'],
                        SCREEN_NAME. ' ' . $arrTextTranslate['PUBLIC_MSG_002']
                    );
                    echo "$('.edit-error').html('".$arrTextTranslate['PUBLIC_MSG_002']."');";
                    exit();
                }
                //get max link_no
                $arrLastLink = fncSelectOne(
                    "SELECT LINK_NO FROM t_link ORDER BY LINK_NO DESC",
                    [],
                    SCREEN_NAME
                );
                if($arrLastLink) {
                    //link_no = max link_no + 1
                    $intLastLinkNo = $arrLastLink['LINK_NO'] + 1;
                } else {
                    //there is no link
                    $intLastLinkNo = 1;
                }
                //get max sort_no
                $arrLastSortLink = fncSelectOne(
                    "SELECT SORT_NO FROM t_link WHERE LINK_CATEGORY_NO=? ORDER BY SORT_NO DESC",
                    [$_POST['LINK_CATEGORY_NO']],
                    SCREEN_NAME
                );
                if($arrLastSortLink) {
                    //sort_no = max sort_no + 1
                    $intSortNo = $arrLastSortLink['SORT_NO'] + 1;
                } else {
                    $intSortNo = 1;
                }
                //upload

                $blnCheck = true;
                if(isset($_FILES['file']) && $_FILES['file']['name']!='') {
                    //upload file
                    $strFolderPath = SHARE_FOLDER.'/'.LINK_EDIT_FOLDER.'/'.$intLastLinkNo.'/1/';
                    if (!file_exists($strFolderPath)) {
                        //create folder if not exist
                        mkdir($strFolderPath, 0777, true);
                    }
                    $files = glob($strFolderPath.'*'); // get all file names
                    foreach($files as $file) { // iterate files
                    if(is_file($file))
                        unlink($file); // delete file
                    }
                    if(
                        !move_uploaded_file(
                            $_FILES["file"]["tmp_name"]
                            ,$strFolderPath.basename($_FILES['file']['name'])
                        )
                    ) {
                        //file upload failed
                        $blnCheck = false;
                    }
                }


                if($blnCheck)
                {
                    //insert new link to database
                    $objResult = fncProcessData([
                            'LINK_NO'   =>  $intLastLinkNo,
                            'SORT_NO'   =>  $intSortNo,
                            'LINK_CATEGORY_NO' => $_POST['LINK_CATEGORY_NO'],
                            'LINK_NAME_JPN' => $_POST['LINK_NAME_JPN'],
                            'LINK_NAME_ENG' => $_POST['LINK_NAME_ENG'],
                            'URL' => $_POST['URL'],
                            'BURNER_FILE_NAME1' => $_FILES['file']['name'],
                            'REG_USER_NO'=> $objUserInfo->intUserNo,
                            'REG_DATE'  => date('Y-m-d H:i:s'),
                            // 'UP_USER_NO'    =>  $objUserInfo->intUserNo,
                            // 'UP_DATE'   => date('Y-m-d H:i:s')
                        ],
                        't_link',
                        '',
                        [],
                        SCREEN_NAME
                    );
                    //insert success
                    if(is_object($objResult) && $objResult->rowCount()>0) {
                        echo 1;
                    } else {
                        //insert failed, write log and show error message
                        fncWriteLog(
                            LogLevel['Error'],
                            LogPattern['Error'],
                            SCREEN_NAME. ' ' . $arrTextTranslate['PUBLIC_MSG_002']
                        );
                        echo "$('.edit-error').html('".$arrTextTranslate['PUBLIC_MSG_002']."');";
                    }
                } else {
                    //file upload failed, write log and show error message
                    fncWriteLog(
                        LogLevel['Error'],
                        LogPattern['Error'],
                        SCREEN_NAME. ' ' . $arrTextTranslate['PUBLIC_MSG_002']
                    );
                    echo "$('.edit-error').html('".$arrTextTranslate['PUBLIC_MSG_002']."');";
                }
            }
        }
    }
?>