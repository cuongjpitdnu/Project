<?php
    /*
     * @bulletin_board_edit_proc.php
     *
     * @create 2020/03/13 AKB Chien
     * @update
     */
    require_once('common/common.php');
    require_once('common/session_check.php');

    header('Content-type: text/html; charset=utf-8');
    header('X-FRAME-OPTIONS: DENY');

    define('DISPLAY_TITLE', '掲示板編集画面');

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
        'PUBLIC_MSG_001',
        'PUBLIC_MSG_049',
        'PUBLIC_MSG_010',
        'PUBLIC_MSG_041',
        'BULLETIN_BOARD_EDIT_MSG_002',
        'BULLETIN_BOARD_EDIT_MSG_003',
        'BULLETIN_BOARD_EDIT_MSG_004',
        'BULLETIN_BOARD_EDIT_MSG_005',
        'PUBLIC_MSG_003',
    );

    define('LENGTH_INCIDENT_JPN_TO_ENG', 1000);  // content jpn to eng

    $intAutoTranslate = 0;  // default = 0

    // get list text translate
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

    // check if ajax -> do something | access this file directly -> stop
    if(!(!empty($_SERVER['HTTP_X_REQUESTED_WITH'])
        && strtolower($_SERVER['HTTP_X_REQUESTED_WITH']) == 'xmlhttprequest')) {
        exit;
    }

    if(isset($_POST)) {
        $_SESSION['BULLETIN_BOARD_EDIT_ERROR'] = array();

        // init data
        $arrResultToView = array(
            'trans-error' => '',
            'error' => '',
            'success' => '',
            'confirm' => '',
            'auto' => '',
        );

        if(isset($_POST['action'])) {
            // event click button translate
            if($_POST['action'] == 'translate') {
                $intTypeTranslate = 0; // ja -> eng
                $strChkBoxManualTranslate = @$_POST['chkManualInput']
                                        ? trim($_POST['chkManualInput']) : '';
                $strIncidentOriginal = @$_POST['txtIncidentOriginal']
                                        ? trim($_POST['txtIncidentOriginal']) : '';
                $strIncidentTranslate = @$_POST['txtIncidentTranslation']
                                        ? trim($_POST['txtIncidentTranslation']) : '';

                // if checkbox translate checked
                if($strChkBoxManualTranslate == 'on') {
                    $arrResultToView['success'] = array(
                        'incidentTranslate' => $strIncidentTranslate
                    );
                    echo json_encode($arrResultToView);
                    exit;
                }

                // if checkbox translate unchecked
                if($strChkBoxManualTranslate == 'off') {
                    if($strIncidentTranslate != '') {
                        $arrResultToView['success'] = array(
                            'incidentTranslate' => $strIncidentTranslate
                        );
                        echo json_encode($arrResultToView);
                        exit;
                    }
                }

                $strIncidentTranslate = '';
                // translate
                try {
                    // if amazon key has defined
                    if(defined ('AWS_ACCESS_KEY') && defined ('AWS_SECRET_KEY')) {
                        $strIncidentTranslate = tranAmazon($strIncidentOriginal, $intTypeTranslate);
                    }

                    // if translate has error
                    if((is_array($strIncidentTranslate) && $strIncidentTranslate['error'] == 1)
                        || $strIncidentTranslate == '' || $strIncidentTranslate == null
                        || !defined ('AWS_ACCESS_KEY') || !defined ('AWS_SECRET_KEY')) {
                        $_SESSION['BULLETIN_BOARD_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_010'];
                    }

                    // if has msg error
                    if(count($_SESSION['BULLETIN_BOARD_EDIT_ERROR']) > 0) {
                        $strHtmlError = '';
                        foreach($_SESSION['BULLETIN_BOARD_EDIT_ERROR'] as $error) {
                            $strHtmlError .= '<div>'.$error.'</div>';
                        }
                        $arrResultToView['error'] = $strHtmlError;
                        $arrResultToView['trans-error'] = 'error';
                        echo json_encode($arrResultToView);
                        exit;
                    }

                    $arrResultToView['success'] = array(
                        'incidentTranslate' => $strIncidentTranslate
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
                        'incidentTranslate' => ''
                    );
                    echo json_encode($arrResultToView);
                    exit;
                }
            }

            // click post
            if($_POST['action'] == 'pre-update') {
                // write log
                $strLogBtnPost = DISPLAY_TITLE.'　更新(ユーザID = '.$objLoginUserInfo->strUserID.')';
                fncWriteLog(LogLevel['Info'] , LogPattern['Button'], $strLogBtnPost);

                $intTypeTranslate = 0; // ja -> eng
                $strChkBoxManualTranslate = @$_POST['chkManualInput']
                                        ? trim($_POST['chkManualInput']) : '';
                $strIncidentOriginal = @$_POST['txtIncidentOriginal']
                                        ? trim($_POST['txtIncidentOriginal']) : '';
                $strIncidentTranslate = @$_POST['txtIncidentTranslation']
                                        ? trim($_POST['txtIncidentTranslation']) : '';

                // check if translate has text -> prepare to set value for auto_translate
                $blnFlagHasTransUncheck = false;
                if($strIncidentTranslate != '') {
                    $blnFlagHasTransUncheck = true;
                }


                //2020/04/24 T.Mausda 確認メッセージの前に入力チェックを行う                    /
                if($strChkBoxManualTranslate == 'on' || $strIncidentTranslate != '') {
                    // check length text translate
                    if(mb_strlen($strIncidentTranslate) == 0) {
                        $_SESSION['BULLETIN_BOARD_EDIT_ERROR'][] = $arrTxtTrans['BULLETIN_BOARD_EDIT_MSG_003'];
                    }
                    //▼2020/05/27 KBS T.Masuda 翻訳欄の文字数チェックを行わない
                    // check length text translate
                    //if(mb_strlen($strIncidentTranslate) > LENGTH_INCIDENT_JPN_TO_ENG) {
                    //    $_SESSION['BULLETIN_BOARD_EDIT_ERROR'][] = $arrTxtTrans['BULLETIN_BOARD_EDIT_MSG_004'];
                    //}
                    //▲2020/05/27 KBS T.Masuda
                    
                    // check text translate has non-ASCII char
                    //▼2020/05/29 KBS S.Tasaki 半角英数チェックから全角ダブルクォーテーションを除外
                    $strCheckIncidentTranslate = $strIncidentTranslate;
                    $strCheckIncidentTranslate = str_replace('“', '', $strCheckIncidentTranslate);
                    $strCheckIncidentTranslate = str_replace('”', '', $strCheckIncidentTranslate);
                    if(!mb_detect_encoding($strCheckIncidentTranslate, 'ASCII', true)) {
                        $_SESSION['BULLETIN_BOARD_EDIT_ERROR'][] = $arrTxtTrans['BULLETIN_BOARD_EDIT_MSG_005'];
                    }
                    //▲2020/05/29 KBS S.Tasaki
                }

                // if has error msg
                if(count($_SESSION['BULLETIN_BOARD_EDIT_ERROR']) > 0) {
                    $strHtmlError = '';
                    foreach($_SESSION['BULLETIN_BOARD_EDIT_ERROR'] as $error) {
                        $strHtmlError .= '<div>'.$error.'</div>';
                    }
                    $arrResultToView['error'] = $strHtmlError;
                    echo json_encode($arrResultToView);
                    exit;
                }
                //2020/04/24 T.Mausda


                // if checkbox translate unchecked
                if($strChkBoxManualTranslate == 'off') {
                    $blnFlagHadTrans = false;
                    // if text translate is empty
                    if($strIncidentTranslate == '') {
                        // translate
                        try {
                            // if amazon key has defined
                            if(defined ('AWS_ACCESS_KEY') && defined ('AWS_SECRET_KEY')) {
                                $strIncidentTranslate = tranAmazon($strIncidentOriginal, $intTypeTranslate);
                            }

                            // if translate has error
                            if((is_array($strIncidentTranslate) && $strIncidentTranslate['error'] == 1)
                                || $strIncidentTranslate == '' || $strIncidentTranslate == null
                                || !defined ('AWS_ACCESS_KEY') || !defined ('AWS_SECRET_KEY')) {
                                    $arrResultToView['confirm'] = $arrTxtTrans['PUBLIC_MSG_041'];
                            }

                            // if has msg error
                            if(count($_SESSION['BULLETIN_BOARD_EDIT_ERROR']) > 0) {
                                $strHtmlError = '';
                                foreach($_SESSION['BULLETIN_BOARD_EDIT_ERROR'] as $error) {
                                    $strHtmlError .= '<div>'.$error.'</div>';
                                }
                                $arrResultToView['error'] = $strHtmlError;
                                $arrResultToView['trans-error'] = 'error';
                                echo json_encode($arrResultToView);
                                exit;
                            }

                            $arrResultToView['confirm'] = $arrTxtTrans['BULLETIN_BOARD_EDIT_MSG_002'];
                            $arrResultToView['success'] = array(
                                'incidentTranslate' => $strIncidentTranslate
                            );
                            $intAutoTranslate = 0;
                            $blnFlagHadTrans = true;
                        } catch (\Exception $e) {
                            // write log
                            fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                                $arrTxtTrans['PUBLIC_MSG_010']);
                            $arrResultToView['error'] = $arrTxtTrans['PUBLIC_MSG_010'];
                            $arrResultToView['trans-error'] = 'error';
                            $arrResultToView['success'] = array(
                                'incidentTranslate' => ''
                            );
                            echo json_encode($arrResultToView);
                            exit;
                        }
                    }

                    // Not error in translate
                    if(!is_array($strIncidentTranslate)) {
                        if($strIncidentTranslate != '') {
                            $arrResultToView['confirm'] = $arrTxtTrans['BULLETIN_BOARD_EDIT_MSG_002'];
                            $arrResultToView['success'] = array(
                                'incidentTranslate' => $strIncidentTranslate
                            );
                            $intAutoTranslate = 0;
                            if($blnFlagHasTransUncheck) {
                                $intAutoTranslate = 1;
                            }
                        } else {
                            // if translate text is empty
                            if($strIncidentTranslate == '') {
                                $arrResultToView['confirm'] = $arrTxtTrans['PUBLIC_MSG_041'];
                                if(!$blnFlagHadTrans) {
                                    $arrResultToView['confirm'] = $arrTxtTrans['BULLETIN_BOARD_EDIT_MSG_002'];
                                }
                            }
                            $arrResultToView['success'] = array(
                                'incidentTranslate' => (!is_array($strIncidentTranslate))
                                ? ((trim($strIncidentTranslate) == '') ? '' : $strIncidentTranslate) : '',
                            );
                            $intAutoTranslate = 0;
                        }
                    } else {
                        $arrResultToView['confirm'] = $arrTxtTrans['PUBLIC_MSG_041'];
                        $arrResultToView['success'] = array(
                            'incidentTranslate' => '',
                        );
                        $intAutoTranslate = 0;
                    }
                } else {
                    $arrResultToView['confirm'] = $arrTxtTrans['BULLETIN_BOARD_EDIT_MSG_002'];
                    $arrResultToView['success'] = array(
                        'incidentTranslate' => $strIncidentTranslate
                    );
                    $intAutoTranslate = 1;
                }
                $arrResultToView['auto'] = $intAutoTranslate;
                echo json_encode($arrResultToView);
                exit;
            }

            // action update after confirm msg
            if($_POST['action'] == 'update') {
                if($_POST['mode'] == 1) {
                    // prepare data to insert/update
                    $intBulletinBoardNo = isset($_POST['id'])
                                        ? trim($_POST['id']) : '';
                    $chkManualInput = isset($_POST['chkManualInput'])
                                        ? ((trim($_POST['chkManualInput']) == 'on') ? 1 : 0) : '';
                    $strIncidentOriginal = @$_POST['txtIncidentOriginal']
                                        ? trim($_POST['txtIncidentOriginal']) : '';
                    $strIncidentTranslate = @$_POST['txtIncidentTranslation']
                                        ? trim($_POST['txtIncidentTranslation']) : '';
                    $intAutoTrans = isset($_POST['auto']) ? $_POST['auto'] : '';

                    //2020/04/24 T.Masuda 確認メッセージの前に入力チェックを行う
                    // mode=1 / 以下の登録処理を実行する。/ ①	入力チェックを行う。
                    /* if($intAutoTrans == 1) {
                        // check length text translate
                        if(mb_strwidth($strIncidentTranslate) == 0) {
                            $_SESSION['BULLETIN_BOARD_EDIT_ERROR'][] = $arrTxtTrans['BULLETIN_BOARD_EDIT_MSG_003'];
                        }
                        // check length text translate
                        if(mb_strwidth($strIncidentTranslate) > LENGTH_INCIDENT_JPN_TO_ENG) {
                            $_SESSION['BULLETIN_BOARD_EDIT_ERROR'][] = $arrTxtTrans['BULLETIN_BOARD_EDIT_MSG_004'];
                        }
                        // check text translate has non-ASCII char
                        if(!mb_detect_encoding($strIncidentTranslate, 'ASCII', true)) {
                            $_SESSION['BULLETIN_BOARD_EDIT_ERROR'][] = $arrTxtTrans['BULLETIN_BOARD_EDIT_MSG_005'];
                        }
                    } */
                    //2020/04/24 T.Masuda
                    
                    // get list mail of user
                    $arrUser = getDataMUserSendMail('BULLETIN_BOARD_MAIL', DISPLAY_TITLE);
                    if($arrUser == 0) {
                        $_SESSION['BULLETIN_BOARD_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_001'];
                    }
                                        
                    // if has error msg
                    if(count($_SESSION['BULLETIN_BOARD_EDIT_ERROR']) > 0) {
                        $strHtmlError = '';
                        foreach($_SESSION['BULLETIN_BOARD_EDIT_ERROR'] as $error) {
                            $strHtmlError .= '<div>'.$error.'</div>';
                        }
                        $arrResultToView['error'] = $strHtmlError;
                        echo json_encode($arrResultToView);
                        exit;
                    }

                    // update
                    if($_POST['action'] == 'update') {
                        try {
                            // ②	以下の条件で掲示板テーブル[t_bulletin_board]を更新する。更新要領は以下の通りとする。
                            $strSQL = " UPDATE t_bulletin_board SET "
                                    . " incident_name_eng = :incident_name_eng "
                                    . " , correction_flag = :correction_flag "
                                    . " , untranslated = 1 "
                                    . " , up_user_no = :up_user_no "
                                    . " , up_date = CURRENT_TIMESTAMP "
                                    . " WHERE bulletin_board_no = :bulletin_board_no; ";

                            $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
                            $objStmt->bindParam(':incident_name_eng', $strIncidentTranslate);
                            $objStmt->bindParam(':correction_flag', $chkManualInput);
                            $objStmt->bindParam(':up_user_no', $objLoginUserInfo->intUserNo);
                            $objStmt->bindParam(':bulletin_board_no', $intBulletinBoardNo);

                            $strLogSql = DISPLAY_TITLE.' '.$strSQL;
                            $strLogSql = str_replace(':incident_name_eng', $strIncidentTranslate, $strLogSql);
                            $strLogSql = str_replace(':correction_flag', $chkManualInput, $strLogSql);
                            $strLogSql = str_replace(':up_user_no', $objLoginUserInfo->intUserNo, $strLogSql);
                            $strLogSql = str_replace(':bulletin_board_no', $intBulletinBoardNo, $strLogSql);
                            fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);
                            $objStmt->execute();

                            //2020/04/24 T.Masuda 更新対象の掲示板が削除されていた場合、画面を閉じる
                            if(!is_object($objStmt) || $objStmt->rowCount() <= 0){
                                fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                                    DISPLAY_TITLE.' '.$arrTxtTrans['PUBLIC_MSG_003']);
                                $arrResultToView['error'] = 'alreadyDelError';
                                echo json_encode($arrResultToView);
                                exit;
                            }
                            //2020/04/24 T.Masuda

                        } catch (\Exception $e) {
                            // write log
                            fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                                $arrTxtTrans['PUBLIC_MSG_003']);
                            $_SESSION['BULLETIN_BOARD_EDIT_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_003'];
                            // if has error msg
                            if(count($_SESSION['BULLETIN_BOARD_EDIT_ERROR']) > 0) {
                                $strHtmlError = '';
                                foreach($_SESSION['BULLETIN_BOARD_EDIT_ERROR'] as $error) {
                                    $strHtmlError .= '<div>'.$error.'</div>';
                                }
                                $arrResultToView['error'] = $strHtmlError;
                                echo json_encode($arrResultToView);
                                exit;
                            }
                        }
                        
                        // send mail
	                    // if has error msg
	                    if($arrUser == 0) {
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
		                            // if user is jpn
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
	                        // divide array mail with MAIL_SUBMIT_NUMBER
	                        if(count($arrMail['jpn_ext']) > 0) {
	                            $arrMailSend['jpn_ext'] = array_chunk($arrMail['jpn_ext'], MAIL_SUBMIT_NUMBER);
	                        }

	                        // divide array mail with MAIL_SUBMIT_NUMBER
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
	                                BULLETIN_BOARD_MAIL_TEXT_001_JPN, $arrBody['jpn']);
	                        }

	                        // check body mail eng not empty -> get body mail eng
	                        if($arrBody['eng'] != '') {
	                            $arrBody['eng'] = str_replace ('%0%',
	                                date('H:i, d M'), $arrBody['eng']);
	                            $arrBody['eng'] = str_replace ('%1%',
	                                BULLETIN_BOARD_MAIL_TEXT_001_ENG, $arrBody['eng']);
	                        }
	                        
	                        //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
	                        // check body mail jpn not empty -> get body mail jpn
	                        if($arrBody['jpn_ext'] != '') {
	                            $arrBody['jpn_ext'] = str_replace ('%0%',
	                                date('m月d日H時i分'), $arrBody['jpn_ext']);
	                            $arrBody['jpn_ext'] = str_replace ('%1%',
	                                BULLETIN_BOARD_MAIL_TEXT_001_JPN, $arrBody['jpn_ext']);
	                        }

	                        // check body mail eng not empty -> get body mail eng
	                        if($arrBody['eng_ext'] != '') {
	                            $arrBody['eng_ext'] = str_replace ('%0%',
	                                date('H:i, d M'), $arrBody['eng_ext']);
	                            $arrBody['eng_ext'] = str_replace ('%1%',
	                                BULLETIN_BOARD_MAIL_TEXT_001_ENG, $arrBody['eng_ext']);
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
	                        // check ammount mail
	                        if(count($arrMailSend['jpn_ext']) > 0) {
	                            $blnFlagHasBccExtMailJPN = true;
	                        }

	                        // check ammount mail
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
        }
    }
?>
