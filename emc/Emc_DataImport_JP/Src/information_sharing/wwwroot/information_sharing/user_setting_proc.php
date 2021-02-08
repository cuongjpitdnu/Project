<?php
    /*
     * @user_setting_proc.php
     *
     * @create 2020/03/13 AKB Chien
     * @update
     */
    require_once('common/common.php');
    require_once('common/session_check.php');

    header('Content-type: text/html; charset=utf-8');
    header('X-FRAME-OPTIONS: DENY');

    define('DISPLAY_TITLE', 'ユーザ設定画面');

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

    // check if ajax -> do something | access this file directly -> stop
    if(!(!empty($_SERVER['HTTP_X_REQUESTED_WITH'])
        && strtolower($_SERVER['HTTP_X_REQUESTED_WITH']) == 'xmlhttprequest')) {
        exit;
    }

    // ログインユーザ情報を取得
    $objLoginUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);

    $intLanguageType = $objLoginUserInfo->intLanguageType;

    $arrTitleMsg =  array(
        'USER_SETTING_MSG_002',
        'USER_SETTING_MSG_003',
        'USER_SETTING_MSG_004',
        'USER_SETTING_MSG_005',
        'USER_SETTING_MSG_006',
        'USER_SETTING_MSG_007',
        'USER_SETTING_MSG_008',
        'USER_SETTING_MSG_009',
        'USER_SETTING_MSG_010',
        'USER_SETTING_MSG_011',
        'USER_SETTING_MSG_012',
        'USER_SETTING_MSG_013',
        'USER_SETTING_MSG_014',
        'USER_SETTING_MSG_015',
        'USER_SETTING_MSG_016',
        'PUBLIC_MSG_003',

        // 2020/03/30 AKB Chien - start - update document 2020/03/30
        'PUBLIC_MSG_049'
        // 2020/03/30 AKB Chien - end - update document 2020/03/30
    );

    // get list text(header, title, msg) with languague_type of user logged
    $arrTextTranslate = getListTextTranslate($arrTitleMsg, $intLanguageType);

    // 2020/03/30 AKB Chien - start - update document 2020/03/30
    // GET通信にて遷移してきた場合、以下のメッセージをアラート表示し、遷移元画面に戻す。
    fncGetRequestCheck($arrTextTranslate);
    // 2020/03/30 AKB Chien - end - update document 2020/03/30

    // 2020/04/01 AKB Chien - start - update document 2020/04/01
    // GET通信にて遷移してきた場合、以下のメッセージをアラート表示し、遷移元画面に戻す。
    if(!isset($_SERVER['HTTP_REFERER'])) {
        echo '<script type="text/javascript">
                function goBack() {
                    history.go(-1);
                    return false;
                }
                alert("'.$arrTextTranslate['PUBLIC_MSG_049'].'");
                goBack();
            </script>';
        die();
    }
    // 2020/04/01 AKB Chien - end - update document 2020/04/01

    define('LENGTH_ADDRESS', 100);      // max length address
    define('LENGTH_ORGANIZATION', 50);  // max length organization
    define('LENGTH_USER_NAME', 20);     // max length user_name
    define('LENGTH_MAIL_ADDRESS', 50);  // max length mail_address
    define('LENGTH_TEL', 20);           // max length tel
    define('LENGTH_FAX', 20);           // max length fax
    define('LENGTH_MIN_PASSWORD', 12);  // min length password
    define('LENGTH_MAX_PASSWORD', 30);  // max length password

    if(isset($_POST)) {
        $_SESSION['USER_SETTING_ERROR'] = array();

        if(isset($_POST['mode'])) {
            $arrRes = array(
                'error' => '',
                'success' => ''
            );

            // update data user
            if($_POST['mode'] == 1) {
                // <更新処理時>
                $strLog = DISPLAY_TITLE.' 更新 (ユーザID ='.$objLoginUserInfo->strUserID.')';
                fncWriteLog(LogLevel['Info'], LogPattern['Button'], $strLog);

                $strAddress = (@$_POST['txtAddress']) ? trim($_POST['txtAddress']) : '';
                $strOrganization = (@$_POST['txtOrganization'])
                                    ? trim($_POST['txtOrganization']) : '';
                $strName = (@$_POST['txtName']) ? trim($_POST['txtName']) : '';
                $strMail = (@$_POST['txtMail']) ? trim($_POST['txtMail']) : '';
                $strTel = (@$_POST['txtTel']) ? trim($_POST['txtTel']) : '';
                $strFax = (@$_POST['txtFax']) ? trim($_POST['txtFax']) : '';
                $strPassword = (@$_POST['password']) ? trim($_POST['password']) : '';
                $intCmbLanguage = (@$_POST['cmbLanguage'])
                                    ? ((trim($_POST['cmbLanguage']) == 'ja') ? 0 : 1) : '';

                // ①	入力チェックをする。
                if(mb_strlen($strAddress) > LENGTH_ADDRESS) {
                    $_SESSION['USER_SETTING_ERROR'][] = $arrTextTranslate['USER_SETTING_MSG_002'];
                }

                // check length organization
                if(mb_strlen($strOrganization) > LENGTH_ORGANIZATION) {
                    $_SESSION['USER_SETTING_ERROR'][] = $arrTextTranslate['USER_SETTING_MSG_003'];
                }

                // check length user_name
                if(mb_strlen($strName) > LENGTH_USER_NAME) {
                    $_SESSION['USER_SETTING_ERROR'][] = $arrTextTranslate['USER_SETTING_MSG_004'];
                }

                // check length mail
                if(mb_strlen($strMail) == 0) {
                    $_SESSION['USER_SETTING_ERROR'][] = $arrTextTranslate['USER_SETTING_MSG_005'];
                } else {
                    // check mail has non-ASCII char
                    if(!mb_detect_encoding($strMail, 'ASCII', true)) {
                        $_SESSION['USER_SETTING_ERROR'][] = $arrTextTranslate['USER_SETTING_MSG_006'];
                    }
                    if(mb_strlen($strMail) > LENGTH_MAIL_ADDRESS) {
                        $_SESSION['USER_SETTING_ERROR'][] = $arrTextTranslate['USER_SETTING_MSG_007'];
                    }
                }

                // check length tel
                if(mb_strlen($strTel) > LENGTH_TEL) {
                    $_SESSION['USER_SETTING_ERROR'][] = $arrTextTranslate['USER_SETTING_MSG_009'];
                }
                //▼2020/06/05 KBS S.Tasaki 電話番号は数値または「-」のみの入力とする。
                if($strTel != ''){
                    if(!preg_match('/^[-0-9]+$/', $strTel)){
                        $_SESSION['USER_SETTING_ERROR'][] = $arrTextTranslate['USER_SETTING_MSG_008'];
                    }
                }
                //▲2020/06/05 KBS S.Tasaki

                // check length fax
                if(mb_strlen($strFax) > LENGTH_FAX) {
                    $_SESSION['USER_SETTING_ERROR'][] = $arrTextTranslate['USER_SETTING_MSG_011'];
                }
                //▼2020/06/05 KBS S.Tasaki FAXは数値または「-」のみの入力とする。
                if($strFax != ''){
                    if(!preg_match('/^[-0-9]+$/', $strFax)){
                        $_SESSION['USER_SETTING_ERROR'][] = $arrTextTranslate['USER_SETTING_MSG_010'];
                    }
                }
                //▲2020/06/05 KBS S.Tasaki

                // check length password
                if(mb_strlen($strPassword) == 0) {
                    $_SESSION['USER_SETTING_ERROR'][] = $arrTextTranslate['USER_SETTING_MSG_012'];
                } else {
                    // check password has non-ASCII char
                    if(!mb_detect_encoding($strPassword, 'ASCII', true)
                        || strpos($strPassword, ',') !== false) {
                        $_SESSION['USER_SETTING_ERROR'][] = $arrTextTranslate['USER_SETTING_MSG_013'];
                    }
                    // check length password
                    if(mb_strlen($strPassword) < LENGTH_MIN_PASSWORD
                        || mb_strlen($strPassword) > LENGTH_MAX_PASSWORD) {
                        $_SESSION['USER_SETTING_ERROR'][] = $arrTextTranslate['USER_SETTING_MSG_014'];
                    }
                    // check char password must have
                    preg_match('/^.*(?=.*[a-z])(?=.*[A-Z]).*$/', $strPassword, $checkPass);
                    if(count($checkPass) == 0) {
                        $_SESSION['USER_SETTING_ERROR'][] = $arrTextTranslate['USER_SETTING_MSG_015'];
                    }
                }

                // check length combo language
                if(mb_strlen($intCmbLanguage) == 0) {
                    $_SESSION['USER_SETTING_ERROR'][] = $arrTextTranslate['USER_SETTING_MSG_016'];
                }

                // if has error msg -> show
                if(count($_SESSION['USER_SETTING_ERROR']) > 0) {
                    $htmlError = '';
                    foreach($_SESSION['USER_SETTING_ERROR'] as $error) {
                        $htmlError .= '<div>'.$error.'</div>';
                    }
                    $arrRes['error'] = $htmlError;
                    echo json_encode($arrRes);
                    exit;
                }

                // check user exist in database & password change?
                $strSQL = ' SELECT * FROM m_user WHERE m_user.user_no = ? ';
                // execute SQL and get data
                $arrResult = fncSelectData($strSQL, array($objLoginUserInfo->intUserNo),
                    1, false, DISPLAY_TITLE);

                // if has error -> show
                if($arrResult == 0) {
                    // write log
                    fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                        $arrTextTranslate['PUBLIC_MSG_003']);
                    $_SESSION['USER_SETTING_ERROR'][] = $arrTextTranslate['PUBLIC_MSG_003'];
                    // if has error msg
                    if(count($_SESSION['USER_SETTING_ERROR']) > 0) {
                        $htmlError = '';
                        foreach($_SESSION['USER_SETTING_ERROR'] as $error) {
                            $htmlError .= '<div>'.$error.'</div>';
                        }
                        $arrRes['error'] = $htmlError;
                        echo json_encode($arrRes);
                        exit;
                    }
                }

                //flag check password change? false: no | true: change
                $blnFlagChangePass = false;
                // has data
                if(is_array($arrResult) && count($arrResult) > 0) {
                    $strOldPassword = $arrResult[0]['PASSWORD'];
                    if($strOldPassword != $strPassword) {
                        $blnFlagChangePass = true;
                    }
                } else {
                    // write log
                    fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                        $arrTextTranslate['PUBLIC_MSG_003']);
                    $_SESSION['USER_SETTING_ERROR'][] = $arrTextTranslate['PUBLIC_MSG_003'];
                    if(count($_SESSION['USER_SETTING_ERROR']) > 0) {
                        $htmlError = '';
                        foreach($_SESSION['USER_SETTING_ERROR'] as $error) {
                            $htmlError .= '<div>'.$error.'</div>';
                        }
                        $arrRes['error'] = $htmlError;
                        echo json_encode($arrRes);
                        exit;
                    }
                }

                try {
                    // 以下の条件でユーザマスタ[m_user]を更新(UPDATE)する。
                    $strSQL = " UPDATE m_user SET "
                            . " address = :address "
                            . " , organization = :organization "
                            . " , user_name = :user_name "
                            . " , mail_address = :mail_address "
                            . " , tel = :tel "
                            . " , fax = :fax "
                            . " , password = :password "
                            . " , language_type = :language_type "
                    //▼2020/05/27 KBS T.Masuda 更新者、更新日更新
                            . " , up_user_no = :up_user_no "
                            . " , up_date = CURRENT_TIMESTAMP ";
                    //▲2020/05/27 KBS T.Masuda
                    if($blnFlagChangePass) {
                        $strSQL .= " , password_up_date = CURRENT_TIMESTAMP ";
                    }
                    $strSQL .= " WHERE user_no = :user_no; ";

                    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
                    $objStmt->bindParam(':address', $strAddress);
                    $objStmt->bindParam(':organization', $strOrganization);
                    $objStmt->bindParam(':user_name', $strName);
                    $objStmt->bindParam(':mail_address', $strMail);
                    $objStmt->bindParam(':tel', $strTel);
                    $objStmt->bindParam(':fax', $strFax);
                    $objStmt->bindParam(':password', $strPassword);
                    $objStmt->bindParam(':language_type', $intCmbLanguage);
                    $objStmt->bindParam(':up_user_no', $objLoginUserInfo->intUserNo);
                    $objStmt->bindParam(':user_no', $objLoginUserInfo->intUserNo);

                    $strLogSql = DISPLAY_TITLE.$strSQL;
                    $strLogSql = str_replace(':address', $strAddress, $strLogSql);
                    $strLogSql = str_replace(':organization', $strOrganization, $strLogSql);
                    $strLogSql = str_replace(':user_name', $strName, $strLogSql);
                    $strLogSql = str_replace(':mail_address', $strMail, $strLogSql);
                    $strLogSql = str_replace(':tel', $strTel, $strLogSql);
                    $strLogSql = str_replace(':fax', $strFax, $strLogSql);
                    $strLogSql = str_replace(':password', $strPassword, $strLogSql);
                    $strLogSql = str_replace(':language_type', $intCmbLanguage, $strLogSql);
                    $strLogSql = str_replace(':up_user_no', $objLoginUserInfo->intUserNo, $strLogSql);
                    $strLogSql = str_replace(':user_no', $objLoginUserInfo->intUserNo, $strLogSql);
                    fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);
                    $objStmt->execute();

                    /* 更新に成功したら、下記の値をセッション「LOGINUSER_INFO」に格納し、
                     * 画面を閉じる。※親画面の表示を更新する。- 2020/03/18 update */
                    $objLoginUserInfo->strUserName = $strName;
                    $objLoginUserInfo->intLanguageType = $intCmbLanguage;
                    $_SESSION['LOGINUSER_INFO'] = serialize($objLoginUserInfo);

                    $arrRes['success'] = 1;
                    echo json_encode($arrRes);
                    exit;
                } catch (\Exception $e) {
                    // write log
                    fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                        $arrTextTranslate['PUBLIC_MSG_003']);
                    $_SESSION['USER_SETTING_ERROR'][] = $arrTextTranslate['PUBLIC_MSG_003'];
                    if(count($_SESSION['USER_SETTING_ERROR']) > 0) {
                        $htmlError = '';
                        foreach($_SESSION['USER_SETTING_ERROR'] as $error) {
                            $htmlError .= '<div>'.$error.'</div>';
                        }
                        $arrRes['error'] = $htmlError;
                        echo json_encode($arrRes);
                        exit;
                    }
                }
            }
        }
    }
?>