<?php
    /*
     * @announce_edit_proc.php
     *
     * @create 2020/03/13 AKB Chien
     * @update
     */
    require_once('common/common.php');
    require_once('common/session_check.php');

    header('Content-type: text/html; charset=utf-8');
    header('X-FRAME-OPTIONS: DENY');

    define('DISPLAY_TITLE', 'お知らせ登録編集画面');

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

    // check timeout if direct access this file
    fncSessionTimeOutCheck(1);

    // ログインユーザ情報を取得
    $objLoginUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);

    $intLanguageType = $objLoginUserInfo->intLanguageType;

    $arrTitleMsg = array(
        'ANNOUNCE_EDIT_MSG_003',

        'PUBLIC_MSG_021',
        'PUBLIC_MSG_022',
        'PUBLIC_MSG_023',
        'PUBLIC_MSG_024',
        'PUBLIC_MSG_025',
        'PUBLIC_MSG_026',
        'PUBLIC_MSG_027',
        'PUBLIC_MSG_028',
        'PUBLIC_MSG_029',
        'PUBLIC_MSG_030',

        'PUBLIC_MSG_010',
        'PUBLIC_MSG_041',

        // msg check file
        'PUBLIC_MSG_011',
        'PUBLIC_MSG_012',
        'PUBLIC_MSG_013',
        'PUBLIC_MSG_014',
        'PUBLIC_MSG_015',
        'PUBLIC_MSG_016',
        'PUBLIC_MSG_017',
        'PUBLIC_MSG_018',
        'PUBLIC_MSG_019',

        'PUBLIC_MSG_031',
        'PUBLIC_MSG_032',
        'PUBLIC_MSG_033',
        'PUBLIC_MSG_034',
        'PUBLIC_MSG_035',
        'PUBLIC_MSG_036',
        'PUBLIC_MSG_037',
        'PUBLIC_MSG_038',
        'PUBLIC_MSG_039',
        'PUBLIC_MSG_040',

        'PUBLIC_MSG_002',

        'PUBLIC_MSG_001',
        'PUBLIC_MSG_003',

        // 2020/03/26 AKB Chien - start - update document 2020/03/26
        'ANNOUNCE_EDIT_TEXT_003',
        'PUBLIC_MSG_050',
        'PUBLIC_MSG_049',
        // 2020/03/26 AKB Chien - end - update document 2020/03/26

        // 2020/04/15 - start
        'PUBLIC_MSG_051',
        // 2020/04/15 - end
    );

    define('LENGTH_TITLE_JPN_JE', 30);      // length title jpn ja -> eng
    define('LENGTH_TITLE_JPN_EJ', 30);      // length title jpn eng -> jp
    define('LENGTH_TITLE_ENG_JE', 150);     // length title eng ja -> eng
    define('LENGTH_TITLE_ENG_EJ', 150);     // length title eng eng -> jp

    define('LENGTH_CONTENTS_JPN_JE', 1000); // length contents jpn ja -> eng
    define('LENGTH_CONTENTS_JPN_EJ', 1000); // length contents jpn eng -> jp
    define('LENGTH_CONTENTS_ENG_JE', 5000); // length contents eng ja -> eng
    define('LENGTH_CONTENTS_ENG_EJ', 5000); // length contents eng eng -> jp

    define('LENGTH_TITLE_JPN_TO_ENG', 30);  // title jpn to eng
    define('LENGTH_TITLE_ENG_TO_JPN', 150); // title eng to jpn
    define('LENGTH_CONTENT_JPN_TO_ENG', 1000);  // content jpn to eng
    define('LENGTH_CONTENT_ENG_TO_JPN', 5000);  // content eng to jpn

    define('LENGTH_FILE_NAME', 100);    // file name length
    define('TOTAL_FILE_SIZE', 10485760); // 10 * 1024 * 1024

    $intAutoTranslate = 0;  // default = 0

    // get list text(header, title, msg) with languague_type of user logged
    $arrTxtTrans = getListTextTranslate($arrTitleMsg, $intLanguageType);

    // 2020/03/26 AKB Chien - start - update document 2020/03/26
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


    // check permission when access page
    if($objLoginUserInfo->intAnnounceRegPerm != 1) {
        echo 900;
        exit();

    }

    // check if ajax -> do something | access this file directly -> stop
    if(!(isset($_POST['mode']) && $_POST['mode'] == 99) &&
        !(!empty($_SERVER['HTTP_X_REQUESTED_WITH'])
        && strtolower($_SERVER['HTTP_X_REQUESTED_WITH']) == 'xmlhttprequest')) {
        exit;
    }

    if(isset($_POST)) {
        $_SESSION['ANNOUNCE_EDIT_ERROR'] = array();

        // 2020/03/26 AKB Chien - start - update document 2020/03/26
        if(isset($_POST['mode'])) {
            if($_POST['mode'] == 2) {
                // write log
                $strLogBtnPost = DISPLAY_TITLE.'　完了(ユーザID = '.$objLoginUserInfo->strUserID.')';
                fncWriteLog(LogLevel['Info'], LogPattern['Button'], $strLogBtnPost);

                // 完了していないか確認する。
                $strSQL = ' UPDATE t_announce SET t_announce.comp_date = CURRENT_TIMESTAMP, 
                                                  t_announce.up_user_no = :up_user_no,
                                                  t_announce.up_date = CURRENT_TIMESTAMP';
                $strSQL .= ' WHERE t_announce.announce_no = :announceNo ';
                $GLOBALS['g_dbaccess']->funcBeginTransaction();
                try {
                    $strQuery = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
                    $strQuery->bindParam(':announceNo', $_POST['announceNo']);
                    $strQuery->bindParam(':up_user_no', $objLoginUserInfo->intUserNo);

                    // write log
                    $strLogSql = $strSQL;
                    $strLogSql = str_replace(':announceNo', $_POST['announceNo'], $strLogSql);
                    $strLogSql = str_replace(':up_user_no', $objLoginUserInfo->intUserNo, $strLogSql);
                    fncWriteLog(LogLevel['Info'], LogPattern['Sql'], DISPLAY_TITLE.'  '.$strLogSql);

                    $strQuery->execute();
                    //2020/04/24 T.Masuda 更新対象のお知らせが削除されていた場合、画面を閉じる
                    if(!is_object($strQuery) || $strQuery->rowCount() <= 0){
                        $GLOBALS['g_dbaccess']->funcRollback();
                        fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                            DISPLAY_TITLE.' '.$arrTxtTrans['PUBLIC_MSG_003']);
                        echo 2;
                        exit;
                    }
                    //2020/04/24 T.Masuda
                    $GLOBALS['g_dbaccess']->funcCommit();

                    echo 1;
                } catch (\Exception $e) {
                    $GLOBALS['g_dbaccess']->funcRollback();
                    // <エラー発生時>
                    fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                        $arrTxtTrans['PUBLIC_MSG_003']);
                    $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_003'];
                    echo 0;
                }
            }

            // event download
            if($_POST['mode'] == 99) {
                $strFilePath = SHARE_FOLDER.'/'.ANNOUNCE_ATTACHMENT_FOLDER;
                $strFilePath .= '/'.base64_decode($_POST['path']);

                // download file
                fncDownloadFile($strFilePath);
            }
        }
        // 2020/03/26 AKB Chien - end - update document 2020/03/26

        if(isset($_POST['action'])) {
            // event check file exist
            if($_POST['action'] == 'checkFile') {
                $strFilePath = SHARE_FOLDER.'/'.ANNOUNCE_ATTACHMENT_FOLDER.'/'
                             . base64_decode($_POST['file']);
                // check is file exist
                if(is_file($strFilePath)) {
                    echo base64_encode($strFilePath);
                } else {
                    echo 0;
                }
                exit;
            }

            $arrResultToView = array(
                'trans-error' => '',
                'error' => '',
                'success' => '',
                'confirm' => '',
                'auto' => '',
            );

            // 「翻訳」ボタンクリック後の処理
            if($_POST['action'] == 'translate') {
                $intTypeTranslate = @$_POST['cmbTranslation']
                                        ? ((trim($_POST['cmbTranslation']) == 'ja') ? 0 : 1) : '';
                $strTitleOriginal = @$_POST['txtTitleOriginal']
                                        ? trim($_POST['txtTitleOriginal']) : '';
                $strContentOriginal = @$_POST['txtContentOriginal']
                                        ? trim($_POST['txtContentOriginal']) : '';
                $strChkBoxManualTranslate = @$_POST['chkManualInput']
                                        ? trim($_POST['chkManualInput']) : '';
                $strTitleTranslate = @$_POST['txtTitleTranslation']
                                        ? trim($_POST['txtTitleTranslation']) : '';
                $strContentTranslate = @$_POST['txtContentTranslation']
                                        ? trim($_POST['txtContentTranslation']) : '';

                // chekc box translate checked
                if($strChkBoxManualTranslate == 'on') {
                    $arrResultToView['success'] = array(
                        'titleTranslate' => $strTitleTranslate,
                        'contentTranslate' => $strContentTranslate,
                    );
                    echo json_encode($arrResultToView);
                    exit;
                }

                // chekc box translate unchecked
                if($strChkBoxManualTranslate == 'off') {
                    if($strTitleTranslate != '' || $strContentTranslate != '') {
                        $arrResultToView['success'] = array(
                            'titleTranslate'   => $strTitleTranslate,
                            'contentTranslate' => $strContentTranslate,
                        );
                        echo json_encode($arrResultToView);
                        exit;
                    }
                }

                //2020/04/24 T.Masuda 入力チェック文字数変更
                $_SESSION['ANNOUNCE_EDIT_ERROR']
                    = array_merge($_SESSION['ANNOUNCE_EDIT_ERROR'],
                        fncTranInputCheck($strTitleOriginal, $strContentOriginal,
                                          '','',$intTypeTranslate,'',0,
                                          $arrTxtTrans));

                // validate original title + original content
                //validateDataOriginal($strTitleOriginal, $strContentOriginal,
                //                    $intTypeTranslate, $arrTxtTrans);
                //2020/04/24 T.Masuda

                // if has msg error
                if(count($_SESSION['ANNOUNCE_EDIT_ERROR']) > 0) {
                    $strHtmlError = '';
                    foreach($_SESSION['ANNOUNCE_EDIT_ERROR'] as $error) {
                        $strHtmlError .= '<div>'.$error.'</div>';
                    }
                    $arrResultToView['error'] = $strHtmlError;
                    $arrResultToView['success'] = array(
                        'titleTranslate' => '',
                        'contentTranslate' => '',
                    );
                    echo json_encode($arrResultToView);
                    exit;
                } else {
                    $strTitleTranslate = '';
                    $strContentTranslate = '';
                    try {
                        // if amazon key has defined
                        if(defined ('AWS_ACCESS_KEY') && defined ('AWS_SECRET_KEY')) {
                            $strTitleTranslate = tranAmazon($strTitleOriginal, $intTypeTranslate);
                            $strContentTranslate = tranAmazon($strContentOriginal, $intTypeTranslate);
                        }

                        // translate has error
                        if((is_array($strTitleTranslate) && $strTitleTranslate['error'] == 1)
                            || (is_array($strContentTranslate) && $strContentTranslate['error'] == 1)
                            || !defined ('AWS_ACCESS_KEY') || !defined ('AWS_SECRET_KEY')
                            || $strTitleTranslate == '' || $strContentTranslate == '') {
                            $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_010'];
                        }

                        // if has error msg
                        if(count($_SESSION['ANNOUNCE_EDIT_ERROR']) > 0) {
                            $strHtmlError = '';
                            foreach($_SESSION['ANNOUNCE_EDIT_ERROR'] as $error) {
                                $strHtmlError .= '<div>'.$error.'</div>';
                            }
                            $arrResultToView['error'] = $strHtmlError;
                            $arrResultToView['trans-error'] = 'error';
                            echo json_encode($arrResultToView);
                            exit;
                        }

                        $arrResultToView['success'] = array(
                            'titleTranslate'   => $strTitleTranslate,
                            'contentTranslate' => $strContentTranslate,
                        );
                        echo json_encode($arrResultToView);
                        exit;
                    } catch (\Exception $e) {
                        // write log
                        fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                            $arrTxtTrans['PUBLIC_MSG_010']);
                        $arrResultToView['error'] = $arrTxtTrans['PUBLIC_MSG_010'];
                        $arrResultToView['trans-error'] = 'error';
                        $arrResultToView['success'] = array(
                            'titleTranslate'   => '',
                            'contentTranslate' => '',
                        );
                        echo json_encode($arrResultToView);
                        exit;
                    }
                }
            }

            // 「投稿」ボタンクリック後の処理 - step 1
            if($_POST['action'] == 'pre-update' || $_POST['action'] == 'pre-insert') {
                $intAnnounceNo = @$_POST['announceNo'] ? trim($_POST['announceNo']) : '';

                // write log
                $strLogBtnPost = DISPLAY_TITLE.'　登録(ユーザID = '.$objLoginUserInfo->strUserID.')';
                if($intAnnounceNo != '') {
                    $strLogBtnPost = DISPLAY_TITLE.'　更新(ユーザID = '.$objLoginUserInfo->strUserID.')';
                }
                fncWriteLog(LogLevel['Info'], LogPattern['Button'], $strLogBtnPost);

                $intTypeTranslate = @$_POST['cmbTranslation']
                                        ? ((trim($_POST['cmbTranslation']) == 'ja') ? 0 : 1) : '';
                $strTitleOriginal = @$_POST['txtTitleOriginal']
                                        ? trim($_POST['txtTitleOriginal']) : '';
                $strContentOriginal = @$_POST['txtContentOriginal']
                                        ? trim($_POST['txtContentOriginal']) : '';
                $strChkBoxManualTranslate = @$_POST['chkManualInput']
                                        ? trim($_POST['chkManualInput']) : '';

                $strTitleTranslate = @$_POST['txtTitleTranslation']
                                        ? trim($_POST['txtTitleTranslation']) : '';
                $strContentTranslate = @$_POST['txtContentTranslation']
                                        ? trim($_POST['txtContentTranslation']) : '';

                // check translate has text -> set data auto_translate
                $blnFlagHasTransUncheck = false;
                if($strTitleTranslate != '' || $strContentTranslate != '') {
                    $blnFlagHasTransUncheck = true;
                }

                //2020/04/24 T.Masuda 確認メッセージの前に入力チェック
                $intManualTranslate = $strChkBoxManualTranslate == 'off' ? 0 : 1;

                //入力チェック
                $_SESSION['ANNOUNCE_EDIT_ERROR']
                     = array_merge($_SESSION['ANNOUNCE_EDIT_ERROR'],
                                   fncTranInputCheck($strTitleOriginal, $strContentOriginal,
                                                     $strTitleTranslate,$strContentTranslate,
                                                     $intTypeTranslate,$intManualTranslate,1,
                                                     $arrTxtTrans));
                 //2020/04/24 T.Masuda

                if(count($_SESSION['ANNOUNCE_EDIT_ERROR']) > 0) {
                    $strHtmlError = '';
                    foreach($_SESSION['ANNOUNCE_EDIT_ERROR'] as $error) {
                        $strHtmlError .= '<div>'.$error.'</div>';
                    }
                    $arrResultToView['error'] = $strHtmlError;
                    $arrResultToView['auto'] = 0;
                    echo json_encode($arrResultToView);
                    exit;
                }

                // 1. validate title orginal + content original
                //validateDataOriginal($strTitleOriginal, $strContentOriginal,
                //                        $intTypeTranslate, $arrTxtTrans);

                // if has error msg
                //if(count($_SESSION['ANNOUNCE_EDIT_ERROR']) > 0) {
                //    $strHtmlError = '';
                //    foreach($_SESSION['ANNOUNCE_EDIT_ERROR'] as $error) {
                /*         $strHtmlError .= '<div>'.$error.'</div>';
                    }
                    $arrResultToView['error'] = $strHtmlError;
                    $arrResultToView['auto'] = 0;
                    echo json_encode($arrResultToView);
                    exit;
                } */

                // check box translate unchecked
                if($strChkBoxManualTranslate == 'off') {
                    $blnFlagHadTrans = false;
                    // 2. uncheck + titleTranslate = null && contentTranslate = null
                    if($strTitleTranslate == '' && $strContentTranslate == '') {
                        try {
                            // if amazon key has defined
                            if(defined ('AWS_ACCESS_KEY') && defined ('AWS_SECRET_KEY')) {
                                $strTitleTranslate = tranAmazon($strTitleOriginal, $intTypeTranslate);
                                $strContentTranslate = tranAmazon($strContentOriginal, $intTypeTranslate);
                            }

                            // if translate has error
                            if((is_array($strTitleTranslate) && $strTitleTranslate['error'] == 1)
                                || (is_array($strContentTranslate) && $strContentTranslate['error'] == 1)
                                || !defined ('AWS_ACCESS_KEY') || !defined ('AWS_SECRET_KEY')
                                || $strTitleTranslate == '' || $strContentTranslate == '') {
                                $arrResultToView['confirm'] = $arrTxtTrans['PUBLIC_MSG_041'];
                            }

                            // if has error msg
                            if(count($_SESSION['ANNOUNCE_EDIT_ERROR']) > 0) {
                                $strHtmlError = '';
                                foreach($_SESSION['ANNOUNCE_EDIT_ERROR'] as $error) {
                                    $strHtmlError .= '<div>'.$error.'</div>';
                                }
                                $arrResultToView['error'] = $strHtmlError;
                            }

                            $arrResultToView['success'] = array(
                                'titleTranslate' => $strTitleTranslate,
                                'contentTranslate' => $strContentTranslate,
                            );
                            $intAutoTranslate = 0;
                            $blnFlagHadTrans = true;
                        } catch (\Exception $e) {
                            // write log
                            fncWriteLog(LogLevel['Error'] , LogPattern['Error'],
                                        $arrTxtTrans['PUBLIC_MSG_010']);
                            $arrResultToView['error'] = $arrTxtTrans['PUBLIC_MSG_010'];
                            $arrResultToView['trans-error'] = 'error';
                            $arrResultToView['auto'] = 0;
                            echo json_encode($arrResultToView);
                            exit;
                        }
                    }

                    // 4. not error in translate
                    if(!is_array($strTitleTranslate) && !is_array($strContentTranslate)
                        && $strTitleTranslate != '' && $strContentTranslate != '') {
                        $arrResultToView['confirm'] = $arrTxtTrans['ANNOUNCE_EDIT_MSG_003'];
                        $arrResultToView['success'] = array(
                            'titleTranslate' => $strTitleTranslate,
                            'contentTranslate' => $strContentTranslate,
                        );
                        $intAutoTranslate = 0;
                        if($blnFlagHasTransUncheck) {
                            $intAutoTranslate = 1;
                        }
                    } else {
                        // 3. titleTranslate = null || contentTranslate = null
                        if((!is_array($strTitleTranslate) && $strTitleTranslate == '')
                            || (!is_array($strContentTranslate) && $strContentTranslate == '')) {
                            $arrResultToView['confirm'] = $arrTxtTrans['PUBLIC_MSG_041'];
                            if(!$blnFlagHadTrans) {
                                $arrResultToView['confirm'] = $arrTxtTrans['ANNOUNCE_EDIT_MSG_003'];
                            }
                        }
                        $arrResultToView['success'] = array(
                            'titleTranslate' => (!is_array($strTitleTranslate))
                                ? (($strTitleTranslate == '') ? '' : $strTitleTranslate) : '',
                            'contentTranslate' => (!is_array($strContentTranslate))
                                ? (($strContentTranslate == '') ? '' : $strContentTranslate) : '',
                        );

                        $intAutoTranslate = 1;
                        if($blnFlagHadTrans) {
                            $intAutoTranslate = 0;
                        }
                    }
                } else {
                    $arrResultToView['success'] = array(
                        'titleTranslate' => $strTitleTranslate,
                        'contentTranslate' => $strContentTranslate,
                    );
                    $arrResultToView['confirm'] = $arrTxtTrans['ANNOUNCE_EDIT_MSG_003'];
                    $intAutoTranslate = 1;
                }
                $arrResultToView['auto'] = $intAutoTranslate;
                echo json_encode($arrResultToView);
                exit;
            }

            // 「投稿」ボタンクリック後の処理 - step 2 (to finish)
            if($_POST['action'] == 'update' || $_POST['action'] == 'insert') {
                $arrFiles = @$_FILES ? $_FILES : array();
                $arrFileUpload = array();

                if($_POST['mode'] == 1) {
                    // prepare data to insert/update
                    $intAnnounceNo = isset($_POST['id'])
                                        ? trim($_POST['id']) : '';
                    $strCmbTranslation = isset($_POST['cmbTranslation'])
                                        ? trim($_POST['cmbTranslation']) : '';
                    $strTxtTitleOriginal = isset($_POST['txtTitleOriginal'])
                                        ? trim($_POST['txtTitleOriginal']) : '';
                    $strTxtContentOriginal = isset($_POST['txtContentOriginal'])
                                        ? trim($_POST['txtContentOriginal']) : '';
                    $strTxtTitleTranslation = isset($_POST['txtTitleTranslation'])
                                        ? trim($_POST['txtTitleTranslation']) : '';
                    $strTxtContentTranslation = isset($_POST['txtContentTranslation'])
                                        ? trim($_POST['txtContentTranslation']) : '';
                    $intChkManualInput = isset($_POST['chkManualInput'])
                                        ? ((trim($_POST['chkManualInput']) == 'on') ? 1 : 0) : '';
                    $intLanguageType = ($strCmbTranslation != '')
                                        ? (($strCmbTranslation == 'ja') ? 0 : 1) : '';
                    $intChkDelete1 = isset($_POST['chkDelete1'])
                                        ? (($_POST['chkDelete1'] == -1) ? 0 : 1) : '';
                    $intChkDelete2 = isset($_POST['chkDelete2'])
                                        ? (($_POST['chkDelete2'] == -1) ? 0 : 1) : '';
                    $intChkDelete3 = isset($_POST['chkDelete3'])
                                        ? (($_POST['chkDelete3'] == -1) ? 0 : 1) : '';
                    $intChkDelete4 = isset($_POST['chkDelete4'])
                                        ? (($_POST['chkDelete4'] == -1) ? 0 : 1) : '';
                    $intChkDelete5 = isset($_POST['chkDelete5'])
                                        ? (($_POST['chkDelete5'] == -1) ? 0 : 1) : '';
                    $intAutoTrans = isset($_POST['auto'])
                                        ? $_POST['auto'] : '';

                    // get info of announce_no(in case update)
                    $arrOldData = ($_POST['action'] == 'update')
                                    ? getInfoAnnounce($intAnnounceNo, DISPLAY_TITLE) : array();

                    // if get data has error
                    if($arrOldData == 0) {
                        $strMsgError = $arrTxtTrans['PUBLIC_MSG_002'];
                        if($_POST['action'] == 'update') {
                            $strMsgError = $arrTxtTrans['PUBLIC_MSG_003'];
                        }
                        $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $strMsgError;
                        if(count($_SESSION['ANNOUNCE_EDIT_ERROR']) > 0) {
                            $strHtmlError = '';
                            foreach($_SESSION['ANNOUNCE_EDIT_ERROR'] as $error) {
                                $strHtmlError .= '<div>'.$error.'</div>';
                            }
                            $arrResultToView['error'] = $strHtmlError;
                            echo json_encode($arrResultToView);
                            exit;
                        }
                        die();
                    }

                    //2020/04/24 T.Masuda ファイルチェックを関数化
                    //登録時は「0」、更新時は「1」
                    $intActKind = $intAnnounceNo != '' ? 0 : 1;
                    //2020/04/24 T.Masuda

                    //添付ファイルチェック
                    $_SESSION['ANNOUNCE_EDIT_ERROR']
                            = array_merge($_SESSION['ANNOUNCE_EDIT_ERROR'],
                                fncFileCheck($arrFiles, $arrOldData,
                                             $intActKind,0,$arrTxtTrans));

                    // has error msg
                    if(count($_SESSION['ANNOUNCE_EDIT_ERROR']) > 0) {
                            $strHtmlError = '';
                            foreach($_SESSION['ANNOUNCE_EDIT_ERROR'] as $error) {
                                $strHtmlError .= '<div>'.$error.'</div>';
                            }
                            $arrResultToView['error'] = $strHtmlError;
                            echo json_encode($arrResultToView);
                            exit;
                        }
                    }
                    foreach($arrFiles as $strInputName => $objFile) {
                        $strFileName = basename($objFile["name"]);
                        $intNum = substr($strInputName, -1);
                        $arrFileUpload[$intNum] = array($strFileName, $objFile['tmp_name']);
                    }

                    $arrFileToDB = array();
                    $arrFileCheck = array();
                    for($i = 1; $i <= 5; $i++) {
                        $intTemp = isset($_POST['chkDelete'.$i])
                                    ? (($_POST['chkDelete'.$i] == -1) ? 0 : 1) : '';

                        // get file name to array before insert/update to DB
                        $arrFileToDB['file_name'.$i] = getStrFileName($intTemp, $i
                                                                    , $arrFileUpload, $arrOldData);
                    }

                    // check file duplicate
                    if(count($arrFileToDB) > 0) {
                        foreach($arrFileToDB as $strFN) {
                            $strTempFN = trim($strFN);
                            if(count($arrFileCheck) == 0) {
                                if(mb_strwidth($strTempFN) > 0) {
                                    $arrFileCheck[] = $strTempFN;
                                }
                            } else {
                                if(in_array($strTempFN, $arrFileCheck)) {
                                    $strErrMsg = $arrTxtTrans['PUBLIC_MSG_051'];
                                    $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $strErrMsg;
                                    break;
                                } else {
                                    if(mb_strwidth($strTempFN) > 0) {
                                        $arrFileCheck[] = $strTempFN;
                                    }
                                }
                            }
                        }
                    }

                    // if has error msg
                    if(count($_SESSION['ANNOUNCE_EDIT_ERROR']) > 0) {
                        $strHtmlError = '';
                        foreach($_SESSION['ANNOUNCE_EDIT_ERROR'] as $error) {
                            $strHtmlError .= '<div>'.$error.'</div>';
                        }
                        $arrResultToView['error'] = $strHtmlError;
                        echo json_encode($arrResultToView);
                        exit;
                    }

                    // prepare data to insert
                    $arrData = array(
                        // 'announce_no' => $intAnnounceNo,
                        'title_jpn' => ($strCmbTranslation != '')
                                                ? (($strCmbTranslation == 'ja') ? $strTxtTitleOriginal
                                                                             : $strTxtTitleTranslation) : null,
                        'title_eng' => ($strCmbTranslation != '')
                                                ? (($strCmbTranslation == 'en') ? $strTxtTitleOriginal
                                                                             : $strTxtTitleTranslation) : null,
                        'contents_jpn' => ($strCmbTranslation != '')
                                                ? (($strCmbTranslation == 'ja') ? $strTxtContentOriginal
                                                                             : $strTxtContentTranslation): null,
                        'contents_eng' => ($strCmbTranslation != '')
                                                ? (($strCmbTranslation == 'en') ? $strTxtContentOriginal
                                                                             : $strTxtContentTranslation): null,
                        'language_type' => ($strCmbTranslation != '')
                                                ? (($strCmbTranslation == 'ja') ? 0 : 1) : null,
                        'correction_flag' => $intChkManualInput,
                        'language_type' => $intLanguageType,
                        'file_name1' => $arrFileToDB['file_name1'],
                        'file_name2' => $arrFileToDB['file_name2'],
                        'file_name3' => $arrFileToDB['file_name3'],
                        'file_name4' => $arrFileToDB['file_name4'],
                        'file_name5' => $arrFileToDB['file_name5'],
                    );

                    //2020/04/24 T.Masuda 確認メッセージの前に入力チェック
                    // mode=1 / 以下の登録処理を実行する。/ ①	入力チェックを行う。
                    // no auto translate
                    /* if($intAutoTrans == 1) {
                        // eng -> jp
                        if($intLanguageType == 1) {
                            // title_jpn
                            // check length title_jpn
                            if(mb_strwidth($arrData['title_jpn']) == 0) {
                                $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_031'];
                            }
                            // check length title_jpn
                            if(mb_strwidth($arrData['title_jpn']) > LENGTH_TITLE_JPN_EJ) {
                                $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_032'];
                            }

                            // contents_jpn
                            // check length contents_jpn
                            if(mb_strwidth($arrData['contents_jpn']) == 0) {
                                $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_036'];
                            }
                            // check length contents_jpn
                            if(mb_strwidth($arrData['contents_jpn']) > LENGTH_CONTENTS_JPN_EJ) {
                                $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_037'];
                            }
                        } else {
                            // jp -> eng
                            if($intLanguageType == 0) {
                                // title_eng
                                // check length title_eng
                                if(mb_strwidth($arrData['title_eng']) == 0) {
                                    $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_031'];
                                }
                                // check title_eng has non-ASCII char
                                if(!mb_detect_encoding($arrData['title_eng'], 'ASCII', true)) {
                                    $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_033'];
                                }
                                // check length title_eng
                                if(mb_strwidth($arrData['title_eng']) > LENGTH_TITLE_ENG_JE) {
                                    $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_034'];
                                }

                                // contents_eng
                                // check length contents_eng
                                if(mb_strwidth($arrData['contents_eng']) == 0) {
                                    $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_036'];
                                }
                                // check contents_eng has non-ASCII char
                                if(!mb_detect_encoding($arrData['contents_eng'], 'ASCII', true)) {
                                    $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_038'];
                                }
                                // check length contents_eng
                                if(mb_strwidth($arrData['contents_eng']) > LENGTH_CONTENTS_ENG_JE) {
                                    $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_039'];
                                }
                            }
                        }
                    } else {
                        // auto translate
                        // jp -> eng
                        if($intLanguageType == 0) {
                            // title_jpn
                            // check length title_jpn
                            if(mb_strwidth($arrData['title_jpn']) == 0) {
                                $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_021'];
                            }
                            // check length title_jpn
                            if(mb_strwidth($arrData['title_jpn']) > LENGTH_TITLE_JPN_JE) {
                                $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_022'];
                            }

                            // contents_jpn
                            // check length contents_jpn
                            if(mb_strwidth($arrData['contents_jpn']) == 0) {
                                $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_026'];
                            }
                            // check length contents_jpn
                            if(mb_strwidth($arrData['contents_jpn']) > LENGTH_CONTENTS_JPN_JE) {
                                $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_027'];
                            }
                        } else {
                            // eng -> jp
                            if($intLanguageType == 1) {
                                // title_eng
                                // check length title_eng
                                if(mb_strwidth($arrData['title_eng']) == 0) {
                                    $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_021'];
                                }
                                // check title_eng has non-ASCII char
                                if(!mb_detect_encoding($arrData['title_eng'], 'ASCII', true)) {
                                    $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_023'];
                                }
                                // check length title_eng
                                if(mb_strwidth($arrData['title_eng']) > LENGTH_TITLE_ENG_EJ) {
                                    $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_024'];
                                }

                                // contents_eng
                                // check length contents_eng
                                if(mb_strwidth($arrData['contents_eng']) == 0) {
                                    $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_026'];
                                }
                                // check contents_eng has non-ASCII char
                                if(!mb_detect_encoding($arrData['contents_eng'], 'ASCII', true)) {
                                    $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_028'];
                                }
                                // check length contents_eng
                                if(mb_strwidth($arrData['contents_eng']) > LENGTH_CONTENTS_ENG_EJ) {
                                    $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_029'];
                                }
                            }
                        }
                    } */
                    //2020/04/24 T.Masuda


                    // get list mail of user
                    $arrUser = getDataMUserSendMail('ANNOUNCE_MAIL', DISPLAY_TITLE);
                    if($arrUser == 0) {
                        $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_001'];
                    }

                    // if has error msg
                    if(count($_SESSION['ANNOUNCE_EDIT_ERROR']) > 0) {
                        $strHtmlError = '';
                        foreach($_SESSION['ANNOUNCE_EDIT_ERROR'] as $error) {
                            $strHtmlError .= '<div>'.$error.'</div>';
                        }
                        $arrResultToView['error'] = $strHtmlError;
                        echo json_encode($arrResultToView);
                        exit;
                    }

                    // update data
                    if($_POST['action'] == 'update') {
                        $GLOBALS['g_dbaccess']->funcBeginTransaction();
                        try {
                            // 以下の条件でお知らせテーブル[t_announce]を更新する。登録要領は以下の通りとする。
                            $strSQL = " UPDATE t_announce SET "
                                    . " title_jpn = :title_jpn "
                                    . " , title_eng = :title_eng "
                                    . " , contents_jpn = :contents_jpn "
                                    . " , contents_eng = :contents_eng "
                                    . " , language_type = :language_type "
                                    . " , correction_flag = :correction_flag "
                                    . " , file_name1 = :file_name1 "
                                    . " , file_name2 = :file_name2 "
                                    . " , file_name3 = :file_name3 "
                                    . " , file_name4 = :file_name4 "
                                    . " , file_name5 = :file_name5 "
                                    . " , untranslated = 1 "
                                    // . " , data_type = 0 "
                                    . " , up_user_no = :up_user_no "
                                    . " , up_date = CURRENT_TIMESTAMP "
                                    . " WHERE announce_no = :announce_no; ";

                            $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
                            $objStmt->bindParam(':title_jpn', $arrData['title_jpn']);
                            $objStmt->bindParam(':title_eng', $arrData['title_eng']);
                            $objStmt->bindParam(':contents_jpn', $arrData['contents_jpn']);
                            $objStmt->bindParam(':contents_eng', $arrData['contents_eng']);
                            $objStmt->bindParam(':language_type', $arrData['language_type']);
                            $objStmt->bindParam(':correction_flag', $arrData['correction_flag']);
                            $objStmt->bindParam(':file_name1', $arrData['file_name1']);
                            $objStmt->bindParam(':file_name2', $arrData['file_name2']);
                            $objStmt->bindParam(':file_name3', $arrData['file_name3']);
                            $objStmt->bindParam(':file_name4', $arrData['file_name4']);
                            $objStmt->bindParam(':file_name5', $arrData['file_name5']);
                            $objStmt->bindParam(':up_user_no', $objLoginUserInfo->intUserNo);
                            $objStmt->bindParam(':announce_no', $intAnnounceNo);

                            $strLogSql = DISPLAY_TITLE.' '.$strSQL;
                            $strLogSql = str_replace(':title_jpn', $arrData['title_jpn'], $strLogSql);
                            $strLogSql = str_replace(':title_eng', $arrData['title_eng'], $strLogSql);
                            $strLogSql = str_replace(':contents_jpn', $arrData['contents_jpn'], $strLogSql);
                            $strLogSql = str_replace(':contents_eng', $arrData['contents_eng'], $strLogSql);
                            $strLogSql = str_replace(':language_type'
                                                    , $arrData['language_type'], $strLogSql);
                            $strLogSql = str_replace(':correction_flag'
                                                    , $arrData['correction_flag'], $strLogSql);
                            $strLogSql = str_replace(':file_name1', $arrData['file_name1'], $strLogSql);
                            $strLogSql = str_replace(':file_name2', $arrData['file_name2'], $strLogSql);
                            $strLogSql = str_replace(':file_name3', $arrData['file_name3'], $strLogSql);
                            $strLogSql = str_replace(':file_name4', $arrData['file_name4'], $strLogSql);
                            $strLogSql = str_replace(':file_name5', $arrData['file_name5'], $strLogSql);
                            $strLogSql = str_replace(':up_user_no'
                                                    , $objLoginUserInfo->intUserNo, $strLogSql);
                            $strLogSql = str_replace(':announce_no', $intAnnounceNo, $strLogSql);
                            // write log
                            fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);
                            $objStmt->execute();

                            //2020/04/24 T.Masuda 更新対象のお知らせが削除されていた場合、画面を閉じる
                            if(!is_object($objStmt) || $objStmt->rowCount() <= 0){
                                $GLOBALS['g_dbaccess']->funcRollback();
                                fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                                    DISPLAY_TITLE.' '.$arrTxtTrans['PUBLIC_MSG_003']);
                                $arrRes['error'] = 'alreadyDelError';
                                echo json_encode($arrRes);
                                exit;
                            }
                            //2020/04/24 T.Masuda


                            $GLOBALS['g_dbaccess']->funcCommit();
                        } catch (\Exception $e) {
                            // write log
                            fncWriteLog(LogLevel['Error'], LogPattern['Error']
                                        , $arrTxtTrans['PUBLIC_MSG_003']);
                            $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_003'];
                            $GLOBALS['g_dbaccess']->funcRollback();
                            // if has error msg
                            if(count($_SESSION['ANNOUNCE_EDIT_ERROR']) > 0) {
                                $strHtmlError = '';
                                foreach($_SESSION['ANNOUNCE_EDIT_ERROR'] as $error) {
                                    $strHtmlError .= '<div>'.$error.'</div>';
                                }
                                $arrResultToView['error'] = $strHtmlError;
                                echo json_encode($arrResultToView);
                                exit;
                            }
                        }
                    }

                    // insert data
                    if($_POST['action'] == 'insert') {
                        // お知らせテーブルのシーケンス[announce_sequence]から採番したキーを取得する。
                        $intNextAnnounceNo = getNextAnnouce();

                        // if has error msg
                        if($intNextAnnounceNo == 0) {
                            fncWriteLog(LogLevel['Error'], LogPattern['Error']
                                        , $arrTxtTrans['PUBLIC_MSG_002']);
                            $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_002'];
                            if(count($_SESSION['ANNOUNCE_EDIT_ERROR']) > 0) {
                                $strHtmlError = '';
                                foreach($_SESSION['ANNOUNCE_EDIT_ERROR'] as $error) {
                                    $strHtmlError .= '<div>'.$error.'</div>';
                                }
                                $arrResultToView['error'] = $strHtmlError;
                                echo json_encode($arrResultToView);
                                exit;
                            }
                        }

                        $GLOBALS['g_dbaccess']->funcBeginTransaction();
                        try {
                            // 以下の条件でお知らせテーブル[t_announce]を更新する。登録要領は以下の通りとする。
                            $strSQL = " INSERT INTO t_announce "
                                    . " ( "
                                    . "     announce_no, "
                                    . "     title_jpn, "
                                    . "     title_eng, "
                                    . "     contents_jpn, "
                                    . "     contents_eng, "
                                    . "     language_type, "
                                    . "     correction_flag, "
                                    . "     file_name1, "
                                    . "     file_name2, "
                                    . "     file_name3, "
                                    . "     file_name4, "
                                    . "     file_name5, "
                                    . "     untranslated, "
                                    . "     data_type, "
                                    . "     reg_user_no, "
                                    . "     reg_date "
                                    // . "     , up_user_no, "
                                    // . "     up_date "
                                    . " ) VALUES ( "
                                    . "     :announce_no, "
                                    . "     :title_jpn, "
                                    . "     :title_eng, "
                                    . "     :contents_jpn, "
                                    . "     :contents_eng, "
                                    . "     :language_type, "
                                    . "     :correction_flag, "
                                    . "     :file_name1, "
                                    . "     :file_name2, "
                                    . "     :file_name3, "
                                    . "     :file_name4, "
                                    . "     :file_name5, "
                                    . "     1, "
                                    . "     0, "
                                    . "     :reg_user_no, "
                                    . "     CURRENT_TIMESTAMP "
                                    // . "    , :up_user_no, "
                                    // . "     CURRENT_TIMESTAMP "
                                    . " ); ";
                            $intAnnounceNo = $intNextAnnounceNo[0]['announce_no'];
                            $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
                            $objStmt->bindParam(':announce_no', $intAnnounceNo);
                            $objStmt->bindParam(':title_jpn', $arrData['title_jpn']);
                            $objStmt->bindParam(':title_eng', $arrData['title_eng']);
                            $objStmt->bindParam(':contents_jpn', $arrData['contents_jpn']);
                            $objStmt->bindParam(':contents_eng', $arrData['contents_eng']);
                            $objStmt->bindParam(':language_type', $arrData['language_type']);
                            $objStmt->bindParam(':correction_flag', $arrData['correction_flag']);
                            $objStmt->bindParam(':file_name1', $arrData['file_name1']);
                            $objStmt->bindParam(':file_name2', $arrData['file_name2']);
                            $objStmt->bindParam(':file_name3', $arrData['file_name3']);
                            $objStmt->bindParam(':file_name4', $arrData['file_name4']);
                            $objStmt->bindParam(':file_name5', $arrData['file_name5']);
                            $objStmt->bindParam(':reg_user_no', $objLoginUserInfo->intUserNo);
                            // $objStmt->bindParam(':up_user_no', $objLoginUserInfo->intUserNo);

                            $strLogSql = DISPLAY_TITLE.' '.$strSQL;
                            $strLogSql = str_replace(':announce_no', $intAnnounceNo, $strLogSql);
                            $strLogSql = str_replace(':title_jpn', $arrData['title_jpn'], $strLogSql);
                            $strLogSql = str_replace(':title_eng', $arrData['title_eng'], $strLogSql);
                            $strLogSql = str_replace(':contents_jpn', $arrData['contents_jpn'], $strLogSql);
                            $strLogSql = str_replace(':contents_eng', $arrData['contents_eng'], $strLogSql);
                            $strLogSql = str_replace(':language_type'
                                                    , $arrData['language_type'], $strLogSql);
                            $strLogSql = str_replace(':correction_flag'
                                                    , $arrData['correction_flag'], $strLogSql);
                            $strLogSql = str_replace(':file_name1', $arrData['file_name1'], $strLogSql);
                            $strLogSql = str_replace(':file_name2', $arrData['file_name2'], $strLogSql);
                            $strLogSql = str_replace(':file_name3', $arrData['file_name3'], $strLogSql);
                            $strLogSql = str_replace(':file_name4', $arrData['file_name4'], $strLogSql);
                            $strLogSql = str_replace(':file_name5', $arrData['file_name5'], $strLogSql);
                            $strLogSql = str_replace(':reg_user_no'
                                                    , $objLoginUserInfo->intUserNo, $strLogSql);
                            // $strLogSql = str_replace(':up_user_no', $objLoginUserInfo->intUserNo, $strLogSql);
                            fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);
                            $objStmt->execute();
                            $GLOBALS['g_dbaccess']->funcCommit();
                        } catch (\Exception $e) {
                            // write log
                            fncWriteLog(LogLevel['Error'], LogPattern['Error']
                                        , $arrTxtTrans['PUBLIC_MSG_002']);
                            $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_002'];
                            $GLOBALS['g_dbaccess']->funcRollback();
                            if(count($_SESSION['ANNOUNCE_EDIT_ERROR']) > 0) {
                                $strHtmlError = '';
                                foreach($_SESSION['ANNOUNCE_EDIT_ERROR'] as $error) {
                                    $strHtmlError .= '<div>'.$error.'</div>';
                                }
                                $arrResultToView['error'] = $strHtmlError;
                                echo json_encode($arrResultToView);
                                exit;
                            }
                        }
                    }

                    // prepare data file to upload
                    $arrFolderRemove = array();
                    if($intChkDelete1 == 1) {
                        $arrFolderRemove[] = 1;
                    }
                    if($intChkDelete2 == 1) {
                        $arrFolderRemove[] = 2;
                    }
                    if($intChkDelete3 == 1) {
                        $arrFolderRemove[] = 3;
                    }
                    if($intChkDelete4 == 1) {
                        $arrFolderRemove[] = 4;
                    }
                    if($intChkDelete5 == 1) {
                        $arrFolderRemove[] = 5;
                    }
                    // update file
                    uploadFile($intAnnounceNo, $arrFolderRemove, $arrFiles);

                    // send mail
                    // if has error msg
                    if($arrUser == 0) {
                        $_SESSION['ANNOUNCE_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_001'];
                        // has error msg
                        if(count($_SESSION['ANNOUNCE_EDIT_ERROR']) > 0) {
                            $strHtmlError = '';
                            foreach($_SESSION['ANNOUNCE_EDIT_ERROR'] as $error) {
                                $strHtmlError .= '<div>'.$error.'</div>';
                            }
                            $arrResultToView['error'] = $strHtmlError;
                            echo json_encode($arrResultToView);
                            exit;
                        }
                    } else {
                        //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
                        $arrMail = array(
                            'jpn' => array(),
                            'eng' => array(),
                            'jpn_ext' => array(),
                            'eng_ext' => array(),
                        );
                        //▲2020/06/11 KBS S.Tasaki

                        $arrTempMailJP = array();
                        $arrTempMailEN = array();
                        //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
                        $arrTempExtMailJP = array();
                        $arrTempExtMailEN = array();
                        //▲2020/06/11 KBS S.Tasaki
                        foreach($arrUser as $user) {
                            //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
                            if($user['admin_flag'] == 1){
	                            // if user is jpn
	                            if($user['language_type'] == 0) {
	                                if(count($arrTempMailJP) == 0) {
	                                    array_push($arrTempMailJP, $user['mail_address']);
	                                    $arrMail['jpn'][] = array(
	                                        'USER_NAME' => $user['user_name'],
	                                        'MAIL_ADDRESS' => $user['mail_address'],
	                                    );
	                                } else {
	                                    // if email not exist in list prepare to send
	                                    if(!in_array($user['mail_address'], $arrTempMailJP)) {
	                                        array_push($arrTempMailJP, $user['mail_address']);
	                                        $arrMail['jpn'][] = array(
	                                            'USER_NAME' => $user['user_name'],
	                                            'MAIL_ADDRESS' => $user['mail_address'],
	                                        );
	                                    }
	                                }
	                            } else {
	                                // if user is eng
	                                if(count($arrTempMailEN) == 0) {
	                                    array_push($arrTempMailEN, $user['mail_address']);
	                                    $arrMail['eng'][] = array(
	                                        'USER_NAME' => $user['user_name'],
	                                        'MAIL_ADDRESS' => $user['mail_address'],
	                                    );
	                                } else {
	                                    // if email not exist in list prepare to send
	                                    if(!in_array($user['mail_address'], $arrTempMailEN)) {
	                                        array_push($arrTempMailEN, $user['mail_address']);
	                                        $arrMail['eng'][] = array(
	                                            'USER_NAME' => $user['user_name'],
	                                            'MAIL_ADDRESS' => $user['mail_address'],
	                                        );
	                                    }
	                                }
	                            }
                            }else{
	                            if($user['language_type'] == 0) {
	                                if(count($arrTempExtMailJP) == 0) {
	                                    array_push($arrTempExtMailJP, $user['mail_address']);
	                                    $arrMail['jpn_ext'][] = array(
	                                        'USER_NAME' => $user['user_name'],
	                                        'MAIL_ADDRESS' => $user['mail_address'],
	                                    );
	                                } else {
	                                    // if email not exist in list prepare to send
	                                    if(!in_array($user['mail_address'], $arrTempExtMailJP)) {
	                                        array_push($arrTempExtMailJP, $user['mail_address']);
	                                        $arrMail['jpn_ext'][] = array(
	                                            'USER_NAME' => $user['user_name'],
	                                            'MAIL_ADDRESS' => $user['mail_address'],
	                                        );
	                                    }
	                                }
	                            } else {
	                                // if user is eng
	                                if(count($arrTempExtMailEN) == 0) {
	                                    array_push($arrTempExtMailEN, $user['mail_address']);
	                                    $arrMail['eng_ext'][] = array(
	                                        'USER_NAME' => $user['user_name'],
	                                        'MAIL_ADDRESS' => $user['mail_address'],
	                                    );
	                                } else {
	                                    // if email not exist in list prepare to send
	                                    if(!in_array($user['mail_address'], $arrTempExtMailEN)) {
	                                        array_push($arrTempExtMailEN, $user['mail_address']);
	                                        $arrMail['eng_ext'][] = array(
	                                            'USER_NAME' => $user['user_name'],
	                                            'MAIL_ADDRESS' => $user['mail_address'],
	                                        );
	                                    }
	                                }
	                            }
	                        }
                            //▲2020/06/11 KBS S.Tasaki
                        }

                        //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
                        $arrMailSend = array(
                            'jpn' => array(),
                            'eng' => array(),
                            'jpn_ext' => array(),
                            'eng_ext' => array(),
                        );
                        //▲2020/06/11 KBS S.Tasaki

                        // divide array mail with MAIL_SUBMIT_NUMBER
                        if(count($arrMail['jpn']) > 0) {
                            $arrMailSend['jpn'] = array_chunk($arrMail['jpn'], MAIL_SUBMIT_NUMBER);
                        }

                        // divide array mail with MAIL_SUBMIT_NUMBER
                        if(count($arrMail['eng']) > 0) {
                            $arrMailSend['eng'] = array_chunk($arrMail['eng'], MAIL_SUBMIT_NUMBER);
                        }
                        
                        //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
                        if(count($arrMail['jpn_ext']) > 0) {
                            $arrMailSend['jpn_ext'] = array_chunk($arrMail['jpn_ext'], MAIL_SUBMIT_NUMBER);
                        }

                        if(count($arrMail['eng_ext']) > 0) {
                            $arrMailSend['eng_ext'] = array_chunk($arrMail['eng_ext'], MAIL_SUBMIT_NUMBER);
                        }
                        //▲2020/06/11 KBS S.Tasaki

                        $arrSubject = array(
                            'jpn' => MAIL_SUBMIT_TITLE_JPN,
                            'eng' => MAIL_SUBMIT_TITLE_ENG,
                        );

                        //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
                        $arrBody = array(
                            // check file exist to get content
                            'jpn' => is_file('common/mail_temp_jpn.txt')
                                        ? nl2br(file_get_contents("common/mail_temp_jpn.txt")) : '',
                            // check file exist to get content
                            'eng' => is_file('common/mail_temp_eng.txt')
                                        ? nl2br(file_get_contents("common/mail_temp_eng.txt")) : '',
                            // check file exist to get content
                            'jpn_ext' => is_file('common/mail_temp_ext_jpn.txt')
                                        ? nl2br(file_get_contents("common/mail_temp_ext_jpn.txt")) : '',
                            // check file exist to get content
                            'eng_ext' => is_file('common/mail_temp_ext_eng.txt')
                                        ? nl2br(file_get_contents("common/mail_temp_ext_eng.txt")) : '',
                        );
                        //▲2020/06/11 KBS S.Tasaki

                        // check body mail jpn not empty -> get body mail jpn
                        if($arrBody['jpn'] != '') {
                            $arrBody['jpn'] = str_replace ('%0%',
                                date('m月d日H時i分'), $arrBody['jpn']);
                            $arrBody['jpn'] = str_replace ('%1%',
                                ANNOUNCE_EDIT_TEXT_003_JPN, $arrBody['jpn']);
                        }

                        // check body mail eng not empty -> get body mail eng
                        if($arrBody['eng'] != '') {
                            $arrBody['eng'] = str_replace ('%0%',
                                date('H:i, d M'), $arrBody['eng']);
                            $arrBody['eng'] = str_replace ('%1%',
                                ANNOUNCE_EDIT_TEXT_003_ENG, $arrBody['eng']);
                        }
                        
                        //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
                        // check body mail jpn not empty -> get body mail jpn
                        if($arrBody['jpn_ext'] != '') {
                            $arrBody['jpn_ext'] = str_replace ('%0%',
                                date('m月d日H時i分'), $arrBody['jpn_ext']);
                            $arrBody['jpn_ext'] = str_replace ('%1%',
                                ANNOUNCE_EDIT_TEXT_003_JPN, $arrBody['jpn_ext']);
                        }

                        // check body mail eng not empty -> get body mail eng
                        if($arrBody['eng_ext'] != '') {
                            $arrBody['eng_ext'] = str_replace ('%0%',
                                date('H:i, d M'), $arrBody['eng_ext']);
                            $arrBody['eng_ext'] = str_replace ('%1%',
                                ANNOUNCE_EDIT_TEXT_003_ENG, $arrBody['eng_ext']);
                        }
                        //▲2020/06/11 KBS S.Tasaki


                        $blnFlagHasBccMailJPN = false;
                        $blnFlagHasBccMailENG = false;
                        //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
                        $blnFlagHasBccExtMailJPN = false;
                        $blnFlagHasBccExtMailENG = false;
                        //▲2020/06/11 KBS S.Tasaki

                        // check ammount mail
                        if(count($arrMailSend['jpn']) > 0) {
                            $blnFlagHasBccMailJPN = true;
                        }

                        // check ammount mail
                        if(count($arrMailSend['eng']) > 0) {
                            $blnFlagHasBccMailENG = true;
                        }
                        
                        //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
                        if(count($arrMailSend['jpn_ext']) > 0) {
                            $blnFlagHasBccExtMailJPN = true;
                        }
                        
                        if(count($arrMailSend['eng_ext']) > 0) {
                            $blnFlagHasBccExtMailENG = true;
                        }
                        //▲2020/06/11 KBS S.Tasaki

                        //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
                        if(!$blnFlagHasBccMailJPN && !$blnFlagHasBccMailENG && !$blnFlagHasBccExtMailJPN && !$blnFlagHasBccExtMailENG) {
                            // send mail
                            fncSendMail(array(), $arrSubject['jpn'], $arrBody['jpn'], '');
                        } else {
                            foreach($arrMailSend as $strLang => $arrListMail) {
                                $strSubjectSend = '';
                                $strBodySend = '';
                                if($strLang == 'jpn'){
                                    $strSubjectSend = $arrSubject['jpn'];
                                    $strBodySend = $arrBody['jpn'];
                                }else if($strLang == 'eng'){
                                    $strSubjectSend = $arrSubject['eng'];
                                    $strBodySend = $arrBody['eng'];
                                }else if($strLang == 'jpn_ext'){
                                    $strSubjectSend = $arrSubject['jpn'];
                                    $strBodySend = $arrBody['jpn_ext'];
                                }else{
                                    $strSubjectSend = $arrSubject['eng'];
                                    $strBodySend = $arrBody['eng_ext'];
                                }
                                
                                if(count($arrListMail) > 0) {
                                    foreach($arrListMail as $group) {
                                        // send mail
                                        fncSendMail($group, $strSubjectSend, $strBodySend, '');
                                    }
                                }
                            }
                        }
                        //▲2020/06/11 KBS S.Tasaki

                    }

                    $arrResultToView['success'] = 1;
                    echo json_encode($arrResultToView);
                    exit;
                }
            }
        }


    /**
     * get next value announce_no
     *
     * @create 2020/03/13 AKB Chien
     * @update
     * @param integer $intId           announce_no update
     * @param array $arrRemoveFolder   old folder need replace or remove
     * @param array $arrFileUpload     new folder to replace old folder or upload new
     * @return boolean true:成功, false:失敗
     */
    function uploadFile($intId, $arrRemoveFolder, $arrFileUpload) {
        $strPath = SHARE_FOLDER.'/'.ANNOUNCE_ATTACHMENT_FOLDER.'/'.$intId;
        try {
            // delete
            if(count($arrRemoveFolder) > 0) {
                foreach($arrRemoveFolder as $strFolderName) {
                    $strPathOldFolder = $strPath.'/'.$strFolderName;
                    // check path exist
                    if(is_dir($strPathOldFolder)) {
                        $arrFileFolder = glob($strPathOldFolder.'/*');	// get all file and folder
                        if(count($arrFileFolder) > 0) {
                            foreach($arrFileFolder as $arrItem) {
                                // if it isn't file
                                if(!is_file($arrItem)) {
                                    $strPathTemp = glob($arrItem.'/*');	// get all file and folder
                                    // remove folder file_name1 -> 5
                                    foreach($strPathTemp as $objFile) { // iterate files
                                        // if is file
                                        if(is_file($objFile)) {
                                            unlink($objFile); // delete file
                                        } else {
                                            // if is folder
                                            $strPathTemp_2 = glob($objFile.'/*');	// get all file and folder
                                            foreach($strPathTemp_2 as $objFile_2) {
                                                if(is_file($objFile_2)) {
                                                    unlink($objFile_2);
                                                }
                                            }
                                            rmdir($objFile);
                                        }
                                    }
                                    rmdir($arrItem);
                                } else {
                                    unlink($arrItem); // delete file
                                }
                            }
                            rmdir($strPathOldFolder);
                        }
                    }
                }
            }
            // upload
            if(count($arrFileUpload) > 0) {
                foreach($arrFileUpload as $strKey => $objFile) {
                    $strIdFile = substr($strKey, -1);
                    $strPathUpload = $strPath.'/'.$strIdFile;
                    $strFileName = $strPathUpload."/".basename($objFile["name"]);
                    if(!is_dir($strPathUpload)) {
                        // create folder 1 -> 5
                        if (mkdir($strPathUpload, 0755, true)) {
                            // upload file to folder 1 -> 5
                            move_uploaded_file($objFile["tmp_name"], $strFileName);
                        }
                    } else {
                        // upload file
                        move_uploaded_file($objFile["tmp_name"], $strFileName);
                    }
                }
            }
            return true;
        } catch (\Exception $e) {
            // write log
            fncWriteLog(LogLevel['Error'], LogPattern['Error'], $e->getMessage());
            return false;
        }
        return true;
    }

    /**
     * get next value announce_no
     *
     * @create 2020/03/13 AKB Chien
     * @update
     * @param
     * @return array $arrResult || 0: exception
     */
    function getNextAnnouce() {
        try {
            // sql get next announce in sequence
            $strSQL = ' SELECT NEXT VALUE FOR announce_sequence AS announce_no ';
            $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
            $strLogSql = DISPLAY_TITLE.' '.$strSQL;
            // write log
            fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);
            $objStmt->execute();
            $arrResult = $objStmt->fetchAll(PDO::FETCH_ASSOC);
            return $arrResult;
        } catch (\Exception $e) {
            // write log
            fncWriteLog(LogLevel['Error'], LogPattern['Error'], $e->getMessage());
            return 0;
        }
    }

    /**
     * get string file name replace or keep
     *
     * @create 2020/03/13 AKB Chien
     * @update
     * @param boolean $intFlag          flag check file remove or replace
     * @param integer $intFileNumber    file number 1 -> 5
     * @param array $arrFile            list file upload
     * @param array $arrOldData         list file in database
     * @return string $strFileName      file name return
     */
    function getStrFileName($intFlag, $intFileNumber, $arrFile, $arrOldData) {
        $strFileName = null;
        if($intFlag == 1 || $intFlag == '') {
            if(isset($arrFile[$intFileNumber])) {
                $strFileName = $arrFile[$intFileNumber][0];
            } else {
                if(is_array($arrOldData) && count($arrOldData) > 0
                    && $intFlag == '') {
                    $strFileName = $arrOldData[0]['FILE_NAME'.$intFileNumber];
                }
            }
        }
        return $strFileName;
    }
?>