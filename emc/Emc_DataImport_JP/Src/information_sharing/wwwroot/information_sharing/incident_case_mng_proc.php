<?php
    /*
     * @incident_case_mng_proc.php
     *
     * @create 2020/03/13 AKB Chien
     * @update 2020/04/14 KBS T.Mausda  execを使用しフォルダ削除に変更
     */
    require_once('common/common.php');
    require_once('common/session_check.php');

    header('Content-type: text/html; charset=utf-8');
    header('X-FRAME-OPTIONS: DENY');

    define('DISPLAY_TITLE', 'JCMG事案管理画面');

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
    if((isset($_POST['action']) && $_POST['action'] == 'checkNumDataReduct')
        || (isset($_POST['mode']) && $_POST['mode'] == 1)
        || (isset($_POST['mode']) && $_POST['mode'] == 2)
        || (isset($_POST['mode']) && $_POST['mode'] == 3)
        || (isset($_POST['mode']) && $_POST['mode'] == 5)) {
        fncSessionTimeOutCheck(1);
    } else {
        fncSessionTimeOutCheck();
    }

    // ログインユーザ情報を取得
    $objLoginUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);

    $intLanguageType = $objLoginUserInfo->intLanguageType;

    $arrTitleMsg =  array(
        'PUBLIC_MSG_001',
        'INCIDENT_CASE_MNG_TEXT_001',
        'INCIDENT_CASE_MNG_TEXT_002',
        'INCIDENT_CASE_MNG_TEXT_003',
        'INCIDENT_CASE_MNG_TEXT_004',
        'INCIDENT_CASE_MNG_TEXT_005',
        'INCIDENT_CASE_MNG_TEXT_006',
        'INCIDENT_CASE_MNG_TEXT_007',

        // 2020/03/26 AKB Chien - start - update document 2020/03/26
        'INCIDENT_CASE_MNG_TEXT_036',
        'INCIDENT_CASE_MNG_TEXT_037',
        'INCIDENT_CASE_MNG_TEXT_038',
        'INCIDENT_CASE_MNG_TEXT_039',
        'INCIDENT_CASE_MNG_TEXT_040',
        'INCIDENT_CASE_MNG_TEXT_041',
        'PUBLIC_MSG_049',
        // 2020/03/26 AKB Chien - end - update document 2020/03/26

        'INCIDENT_CASE_MNG_TEXT_008',
        'INCIDENT_CASE_MNG_TEXT_009',
        'PUBLIC_BUTTON_006',
        'PUBLIC_BUTTON_007',
        'PUBLIC_BUTTON_010',
        'PUBLIC_BUTTON_008',
        'PUBLIC_BUTTON_011',
        'PUBLIC_BUTTON_012',
        'PUBLIC_BUTTON_015',
        'PUBLIC_TEXT_016',

        'INCIDENT_CASE_MNG_MSG_001',
        'INCIDENT_CASE_MNG_MSG_002',
        'INCIDENT_CASE_MNG_MSG_003',
        'INCIDENT_CASE_MNG_MSG_004',
        'INCIDENT_CASE_MNG_MSG_005',
        'INCIDENT_CASE_MNG_MSG_006',

        'PUBLIC_MSG_006',
        'PUBLIC_MSG_003',
        'PUBLIC_MSG_004',

        // csv
        'PUBLIC_MSG_005',
        // t_incident_case
        'INCIDENT_CASE_MNG_TEXT_011',

        'INCIDENT_CASE_MNG_TEXT_012',
        'INCIDENT_CASE_MNG_TEXT_013',
        'INCIDENT_CASE_MNG_TEXT_014',
        'INCIDENT_CASE_MNG_TEXT_015',
        'INCIDENT_CASE_MNG_TEXT_016',
        'INCIDENT_CASE_MNG_TEXT_017',

        // t_request
        'INCIDENT_CASE_MNG_TEXT_018',

        'INCIDENT_CASE_MNG_TEXT_019',
        'INCIDENT_CASE_MNG_TEXT_020',
        'INCIDENT_CASE_MNG_TEXT_021',
        'INCIDENT_CASE_MNG_TEXT_022',
        'INCIDENT_CASE_MNG_TEXT_023',
        'PUBLIC_TEXT_011',
        'PUBLIC_TEXT_012',
        'PUBLIC_TEXT_013',
        'PUBLIC_TEXT_014',
        'PUBLIC_TEXT_015',
        'PUBLIC_MSG_007',

        // t_information
        'INCIDENT_CASE_MNG_TEXT_024',

        'INCIDENT_CASE_MNG_TEXT_025',
        'INCIDENT_CASE_MNG_TEXT_026',
        'INCIDENT_CASE_MNG_TEXT_027',
        'INCIDENT_CASE_MNG_TEXT_028',
        'INCIDENT_CASE_MNG_TEXT_029',
        'INCIDENT_CASE_MNG_TEXT_030',
        'INCIDENT_CASE_MNG_TEXT_031',
        'INCIDENT_CASE_MNG_TEXT_032',
        'INCIDENT_CASE_MNG_TEXT_033',
        'INCIDENT_CASE_MNG_TEXT_034',
        'INCIDENT_CASE_MNG_TEXT_035',
        'PUBLIC_TEXT_011',
        'PUBLIC_TEXT_012',
        'PUBLIC_TEXT_013',
        'PUBLIC_TEXT_014',
        'PUBLIC_TEXT_015',

        // 2020/04/20 AKB Chien - start - update document 2020/04/20
        'INCIDENT_CASE_MNG_TEXT_042',
        'INCIDENT_CASE_MNG_TEXT_043',
        'INCIDENT_CASE_MNG_TEXT_044',
        'INCIDENT_CASE_MNG_TEXT_045',
        'INCIDENT_CASE_MNG_TEXT_046',
        'INCIDENT_CASE_MNG_TEXT_047',
        // 2020/04/20 AKB Chien - end - update document 2020/04/20

        'INCIDENT_CASE_MNG_MSG_007',
    );

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

    // check if ajax or export csv -> do something | access this file directly -> stop
    if(!(isset($_POST['mode']) && $_POST['mode'] == 4) &&
       !(!empty($_SERVER['HTTP_X_REQUESTED_WITH']) &&
       strtolower($_SERVER['HTTP_X_REQUESTED_WITH']) == 'xmlhttprequest')) {
        exit;
    }

    /**
     * get data search
     *
     * @create 2020/03/13 AKB Chien
     * @update
     * @param string $strTitle
     * @param string $strSDate
     * @param string $strEDate
     * @param string $intChkDone
     * @param integer $intLang
     * @param integer $intPage
     * @param boolean $blnPaging
     * @param boolean $blnGetAll
     * @return array $arrResult
     */
    function fncGetData($strTitle = '', $strSDate = '', $strEDate = '',
                        $intChkDone = '', $intLang, $intPage, $blnPaging = false, $blnGetAll = false) {
        try {
            $suffixes = ($intLang == 0) ? '_JPN' : '_ENG';
            $strTitleSelect = 'title'.$suffixes;

            // get data to show on screen
            if(!$blnGetAll) {
                $strSQL = ' SELECT '
                        . '     tic.incident_case_no, '
                        . '     tic.s_date, '
                        . '     tic.comp_date, '
                        . $strTitleSelect.' AS title, '
                        . '     tic.title_jpn, '
                        . '     tic.title_eng, '
                        . '     tic.up_date, '
                        . '     tic.correction_flag, '
                        . '     mu.user_name '
                        . ' FROM '
                        . '     t_incident_case AS tic ';
            } else {
                // if export csv
                $strSQL = ' SELECT '
                        . '     tic.*, '
                        . '     mu.USER_NAME '
                        . ' FROM '
                        . '     t_incident_case AS tic ';
            }

            $strSQL .= '     INNER JOIN m_user AS mu ON tic.reg_user_no = mu.user_no ';

            $arrCondition = array();
            $blnHaveAnd = false;
            // if title not empty
            if(trim($strTitle) != '') {
                // $strSQL .= ' WHERE tic.'.$strTitleSelect.' LIKE ? ';
                $strSQL .= ' WHERE CASE WHEN tic.title_jpn IS NOT NULL ';
                $strSQL .= ' AND tic.title_eng IS NOT NULL THEN tic.'.$strTitleSelect;
                $strSQL .= ' WHEN tic.title_eng IS NULL THEN tic.title_jpn ';
                $strSQL .= ' ELSE tic.title_eng END LIKE ? ESCAPE \'!\' ';
                $strTitle = str_replace('!', '!!', $strTitle);
                $strTitle = str_replace('%', '!%%', $strTitle);
                $strTitle = str_replace('[', '![', $strTitle);
                $strTitle = str_replace(']', '!]', $strTitle);
                $strTitle = str_replace('_', '!_', $strTitle);
                $arrCondition[] = '%'.$strTitle.'%';
                $blnHaveAnd = true;
            }
            // if s_date not empty
            if(trim($strSDate) != '') {
                if($blnHaveAnd) {
                    $strSQL .= ' AND ';
                } else {
                    $strSQL .= ' WHERE ';
                }
                $strSQL .= ' CAST(tic.s_date AS DATE) >= ? ';
                $arrCondition[] = $strSDate;
                $blnHaveAnd = true;
            }
            // if e_date not empty
            if(trim($strEDate) != '') {
                if($blnHaveAnd) {
                    $strSQL .= ' AND ';
                } else {
                    $strSQL .= ' WHERE ';
                }
                $strSQL .= ' CAST(tic.comp_date AS DATE) <= ? ';
                $arrCondition[] = $strEDate;
                $blnHaveAnd = true;
            }
            // if checkDone checked
            if(trim($intChkDone) != '' && trim($intChkDone) == 1) {
                if($blnHaveAnd) {
                    $strSQL .= ' AND ';
                } else {
                    $strSQL .= ' WHERE ';
                }
                $strSQL .= ' tic.comp_date IS NOT NULL ';
                $blnHaveAnd = true;
            }

            $strSQL .= ' ORDER BY tic.s_date DESC ';

            // execute SQL and get data
            $arrResult = fncSelectData($strSQL, $arrCondition,
                            $intPage, $blnPaging, DISPLAY_TITLE);
            return $arrResult;
        } catch (\Exception $e){
            fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                $arrTxtTrans['PUBLIC_MSG_001']);
            $_SESSION['INCIDENT_MNG_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_001'];
            return 0;
        }
    }

    if(isset($_POST)) {
        if(isset($_POST['mode'])) {
            $strFPRqAttachRoot = SHARE_FOLDER.'\\'.REQUEST_ATTACHMENT_FOLDER;
            $strFPInforAttachRoot = SHARE_FOLDER.'\\'.INFORMATION_ATTACHMENT_FOLDER;

            // Done
            if($_POST['mode'] == 1) {
                // write log
                $strLog = DISPLAY_TITLE.'　完了(ユーザID = '.$objLoginUserInfo->strUserID.')';
                fncWriteLog(LogLevel['Info'] , LogPattern['Button'], $strLog);

                $strSQL = ' UPDATE t_incident_case '
                        . ' SET t_incident_case.comp_date = CURRENT_TIMESTAMP, '
                             . 't_incident_case.up_user_no = :up_user_no,'   
                             . 't_incident_case.up_date = CURRENT_TIMESTAMP '
                        . ' WHERE t_incident_case.incident_case_no = :icn ';
                $GLOBALS['g_dbaccess']->funcBeginTransaction();
                try {
                    $objQuery = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
                    $objQuery->bindParam(':icn', $_POST['incident_case_no']);
                    $objQuery->bindParam(':up_user_no', $objLoginUserInfo->intUserNo);

                    // write log
                    $strLogSql = $strSQL;
                    $strLogSql = str_replace(':icn', $_POST['incident_case_no'], $strLogSql);
                    $strLogSql = str_replace(':up_user_no', $objLoginUserInfo->intUserNo, $strLogSql);
                    fncWriteLog(LogLevel['Info'], LogPattern['Sql'],
                        DISPLAY_TITLE.' '.$strLogSql);

                    $objQuery->execute();
                    //2020/04/23 T.Masuda 完了対象のJCMG事案が存在しなかった場合
                        if(!is_object($objQuery) || $objQuery->rowCount() <= 0){
                            $GLOBALS['g_dbaccess']->funcRollback();
                            fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                                    DISPLAY_TITLE.' '.$arrTxtTrans['PUBLIC_MSG_003']);
                            $_SESSION['INCIDENT_MNG_ERROR'][] = $arrTxtTrans['INCIDENT_CASE_MNG_MSG_007'];
                            echo 3;
                            exit;
                        }
                    //2020/04/23 T.Masuda
                    $GLOBALS['g_dbaccess']->funcCommit();

                    echo 1;
                } catch (\Exception $e) {
                    $GLOBALS['g_dbaccess']->funcRollback();
                    fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                        $arrTxtTrans['PUBLIC_MSG_006']);
                    $_SESSION['INCIDENT_MNG_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_003'];
                    echo 0;
                }
            }

            // Delete
            if($_POST['mode'] == 2) {
                // write log
                $strLog = DISPLAY_TITLE.'　削除(ユーザID = '.$objLoginUserInfo->strUserID.')';
                fncWriteLog(LogLevel['Info'] , LogPattern['Button'], $strLog);

                $strSQLDelIncidentCase = ' DELETE FROM t_incident_case '
                                       . ' WHERE t_incident_case.incident_case_no = :icn ';
                $strSQLDelRequest      = ' DELETE FROM t_request '
                                       . ' WHERE t_request.incident_case_no = :icn ';
                $strSQLDelInformation  = ' DELETE FROM t_information '
                                       . ' WHERE t_information.incident_case_no = :icn ';

                $GLOBALS['g_dbaccess']->funcBeginTransaction();
                try {
                    // get list file
                    // t_request
                    $arrResultRq = array();
                    $arrResultInfo = array();

                    $strSQL = ' SELECT * FROM t_request WHERE t_request.incident_case_no = ? ';
                    // execute SQL and get data
                    $arrResultRq = fncSelectData($strSQL, array($_POST['incident_case_no']),
                                                1, false, DISPLAY_TITLE);
                    $blnCheckHasFalse = false;
                    // if has error
                    if($arrResultRq == 0) {
                        $GLOBALS['g_dbaccess']->funcRollback();
                        $_SESSION['ANNOUNCE_MNG_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_004'];
                        echo 0;
                        die();
                    }

                    // t_information
                    $strSQL = ' SELECT * FROM t_information '
                            . ' WHERE t_information.incident_case_no = ? ';
                    // execute SQL and get data
                    $arrResultInfo = fncSelectData($strSQL, array($_POST['incident_case_no']),
                                                1, false, DISPLAY_TITLE);
                    $blnCheckHasFalse = false;
                    // if has error
                    if($arrResultInfo == 0) {
                        $GLOBALS['g_dbaccess']->funcRollback();
                        $_SESSION['ANNOUNCE_MNG_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_004'];
                        echo 0;
                        die();
                    }

                    // t_incident_case
                    $objQuery = $GLOBALS['g_dbaccess']->funcPrepare($strSQLDelIncidentCase);
                    $objQuery->bindParam(':icn', $_POST['incident_case_no']);

                    // write log
                    $strLogSql = $strSQLDelIncidentCase;
                    $strLogSql = str_replace(':icn', $_POST['incident_case_no'], $strLogSql);
                    fncWriteLog(LogLevel['Info'], LogPattern['Sql'],
                        DISPLAY_TITLE.' '.$strLogSql);

                    $objQuery->execute();

                    // t_request
                    $objQuery = $GLOBALS['g_dbaccess']->funcPrepare($strSQLDelRequest);
                    $objQuery->bindParam(':icn', $_POST['incident_case_no']);

                    // write log
                    $strLogSql = $strSQLDelRequest;
                    $strLogSql = str_replace(':icn', $_POST['incident_case_no'], $strLogSql);
                    fncWriteLog(LogLevel['Info'], LogPattern['Sql'],
                        DISPLAY_TITLE.' '.$strLogSql);

                    $objQuery->execute();

                    // t_information
                    $objQuery = $GLOBALS['g_dbaccess']->funcPrepare($strSQLDelInformation);
                    $objQuery->bindParam(':icn', $_POST['incident_case_no']);

                    // write log
                    $strLogSql = $strSQLDelInformation;
                    $strLogSql = str_replace(':icn', $_POST['incident_case_no'], $strLogSql);
                    fncWriteLog(LogLevel['Info'], LogPattern['Sql'],
                        DISPLAY_TITLE.' '.$strLogSql);

                    $objQuery->execute();

                    $GLOBALS['g_dbaccess']->funcCommit();

                    // no error -> delete file attachment
                    // t_request
                    if($arrResultRq != 0 && count($arrResultRq) > 0) {
                        foreach($arrResultRq as $row) {
                            deleteFolderWithPath($strFPRqAttachRoot.'/'.$row['REQUEST_NO']);
                        }
                    }
                    // t_information
                    if($arrResultInfo != 0 && count($arrResultInfo) > 0) {
                        foreach($arrResultInfo as $row) {
                            deleteFolderWithPath($strFPInforAttachRoot.'/'.$row['INFORMATION_NO']);
                        }
                    }

                    echo 1;
                    exit;
                }  catch (\Exception $e) {
                    $GLOBALS['g_dbaccess']->funcRollback();
                    // write log
                    fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                        $arrTxtTrans['PUBLIC_MSG_004']);
                    $_SESSION['ANNOUNCE_MNG_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_004'];
                    echo 0;
                    exit;
                }
            }

            // cancel
            if($_POST['mode'] == 3) {
                fncWriteLog(LogLevel['Info'] , LogPattern['Button'],
                    DISPLAY_TITLE.'　取消(ユーザID = '.$objLoginUserInfo->strUserID.')');

                // 1
                $strSQLCheck = ' SELECT * FROM t_incident_case '
                             . ' WHERE t_incident_case.comp_date IS NULL '
                             . ' AND t_incident_case.incident_case_no <> ? ';
                // execute SQL and get data
                $arrResult = fncSelectData($strSQLCheck, array($_POST['incident_case_no']), 1,
                                            false, DISPLAY_TITLE);

                // if has error
                if($arrResult == 0) {
                    echo 0;
                    exit;
                }

                // if 1 have data -> show msg 3
                if(count($arrResult) > 0) {
                    // 3
                    echo 2;
                    exit;
                } else {
                    // 2
                    $strSQL = ' UPDATE t_incident_case SET t_incident_case.comp_date = NULL, '
                                    . 't_incident_case.up_user_no = :up_user_no,'   
                                    . 't_incident_case.up_date = CURRENT_TIMESTAMP '  
                            . ' WHERE t_incident_case.incident_case_no = :icn ';
                    $GLOBALS['g_dbaccess']->funcBeginTransaction();
                    try {
                        $objQuery = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
                        $objQuery->bindParam(':icn', $_POST['incident_case_no']);
                        $objQuery->bindParam(':up_user_no', $objLoginUserInfo->intUserNo);

                        // write log
                        $strLogSql = $strSQL;
                        $strLogSql = str_replace(':icn', $_POST['incident_case_no'], $strLogSql);
                        $strLogSql = str_replace(':up_user_no', $objLoginUserInfo->intUserNo, $strLogSql);
                        fncWriteLog(LogLevel['Info'], LogPattern['Sql'],
                            DISPLAY_TITLE.' '.$strLogSql);

                        $objQuery->execute();
                        //2020/04/23 T.Masuda 取消対象のJCMG事案が存在しなかった場合
                        if(!is_object($objQuery) || $objQuery->rowCount() <= 0){
                            $GLOBALS['g_dbaccess']->funcRollback();
                            fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                                    DISPLAY_TITLE.' '.$arrTxtTrans['PUBLIC_MSG_003']);
                            $_SESSION['INCIDENT_MNG_ERROR'][] = $arrTxtTrans['INCIDENT_CASE_MNG_MSG_007'];
                            echo 3;
                            exit;
                        }
                        //2020/04/23 T.Masuda
                        $GLOBALS['g_dbaccess']->funcCommit();
                        echo 1;
                    } catch (\Exception $e) {
                        $GLOBALS['g_dbaccess']->funcRollback();
                        // write log
                        fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                            $arrTxtTrans['PUBLIC_MSG_003']);
                        $_SESSION['INCIDENT_MNG_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_003'];
                        echo 0;
                        exit;
                    }
                }
            }

            // export CSV
            if($_POST['mode'] == 4) {
                $strTitleSearch = $_SESSION['INCIDENT_CASE_MNG_SEARCH_TITLE'];
                $strRegDateSearch = $_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_START'];
                $strCompDateSearch = $_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_END'];
                $intChkDoneSearch = $_SESSION['INCIDENT_CASE_MNG_SEARCH_COMP_CHECK'];

                $intChkDoneLog = ($intChkDoneSearch == 1) ? 1 : 0;

                // write log
                $strLog = DISPLAY_TITLE.'　CSV出力　「JCMG件名 = '.$strTitleSearch.',';
                $strLog .= '期間（開始）= '. $strRegDateSearch.',';
                $strLog .= '期間（終了）= '. $strCompDateSearch.',';
                $strLog .= '完了を表示 = '.$intChkDoneLog.'」';
                $strLog .= '(ユーザID = '.$objLoginUserInfo->strUserID.')';
                fncWriteLog(LogLevel['Info'], LogPattern['Button'], $strLog);

                // t_incident_case
                $strSQL = ' SELECT     '
                        . '     tic.* '
                        . '     , mu.USER_NAME '
                        . ' FROM '
                        . '     t_incident_case AS tic '
                        . '     INNER JOIN m_user AS mu ON (tic.reg_user_no = mu.user_no) '
                        . ' WHERE '
                        . '     tic.incident_case_no = ? ';
                // execute SQL and get data
                $arrResultIncident = fncSelectData($strSQL, array($_POST['incident_case_no']),
                                                    1, false, DISPLAY_TITLE);
                // if has error
                if($arrResultIncident == 0) {
                    fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                        $arrTxtTrans['PUBLIC_MSG_005']);
                    $_SESSION['INCIDENT_MNG_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_005'];
                    header('Location: incident_case_mng.php');
                    exit;
                }

                // t_request
                $strSQL = ' SELECT '
                        . '     t_request.* '
                        . ' FROM '
                        . '     t_request '
                        . ' WHERE '
                        . '     t_request.incident_case_no = ? '
                        . ' ORDER BY t_request.reg_date DESC ';
                // execute SQL and get data
                $arrResultRequest = fncSelectData($strSQL, array($_POST['incident_case_no']),
                                                    1, false, DISPLAY_TITLE);
                // if has error
                if($arrResultRequest == 0) {
                    fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                        $arrTxtTrans['PUBLIC_MSG_005']);
                    $_SESSION['INCIDENT_MNG_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_005'];
                    header('Location: incident_case_mng.php');
                    exit;
                }

                // t_information
                $strSQL = ' SELECT '
                        . '     ti.INFORMATION_NO  '
                        . '     , ti.REG_DATE '
                        . '     , ti.TITLE_JPN '
                        . '     , ti.TITLE_ENG '
                        . '     , ti.CONTENTS_JPN '
                        . '     , ti.CONTENTS_ENG '
                        . '     , ti.CONFIRM_DATE '
                        . '     , ti.CONTACT_INFO '
                        . '     , mc.COMPANY_NAME_JPN '
                        . '     , mc.COMPANY_NAME_ENG '
                        . '     , mic.INFO_CATEGORY_NAME_JPN '
                        . '     , mic.INFO_CATEGORY_NAME_ENG '
                        . '     , minstc.INST_CATEGORY_NAME_JPN '
                        . '     , minstc.INST_CATEGORY_NAME_ENG '
                        . '     , ti.TMP_FILE_NAME1 '
                        . '     , ti.TMP_FILE_NAME2 '
                        . '     , ti.TMP_FILE_NAME3 '
                        . '     , ti.TMP_FILE_NAME4 '
                        . '     , ti.TMP_FILE_NAME5 '
                        . ' FROM t_information AS ti '
                        . '     INNER JOIN m_company AS mc ON ti.COMPANY_NO = mc.COMPANY_NO '
                        . '     INNER JOIN m_info_category AS mic '
                        . '                  ON ti.INFO_CATEGORY_NO = mic.INFO_CATEGORY_NO '
                        . '     INNER JOIN m_inst_category AS minstc '
                        . '                  ON mc.INST_CATEGORY_NO = minstc.INST_CATEGORY_NO '
                        . ' WHERE '
                        . '     ti.incident_case_no = ? '
                        . ' ORDER BY ti.REG_DATE DESC ';
                // execute SQL and get data
                $arrResultInformation = fncSelectData($strSQL, array($_POST['incident_case_no']),
                                                        1, false, DISPLAY_TITLE);
                // if has error
                if($arrResultInformation == 0) {
                    fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                        $arrTxtTrans['PUBLIC_MSG_005']);
                    $_SESSION['INCIDENT_MNG_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_005'];
                    header('Location: incident_case_mng.php');
                    exit;
                }

                $arrResData = array();

                // t_incident_case
                $arrRow1 = array([
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_011']
                ]);
                $arrHead = array([
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_012'],
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_013'],
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_014'],
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_015'],
                    // 2020/04/20 AKB Chien - start - update document 2020/04/20
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_042'],
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_043'],
                    // 2020/04/20 AKB Chien - end - update document 2020/04/20
                    // $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_016'],
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_017'],
                    // 2020/03/26 AKB Chien - start - update document 2020/03/26
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_039'],
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_040'],
                    // 2020/03/26 AKB Chien - end - update document 2020/03/26
                    // 2020/04/20 AKB Chien - start - update document 2020/04/20
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_044'],
                    // 2020/04/20 AKB Chien - end - update document 2020/04/20
                ]);
                $arrDataContent = array();
                foreach($arrResultIncident as $arrItem) {
                    // 2020/03/26 AKB Chien - start - update document 2020/03/26
                    // $strSDateTemp = date_format(date_create($arrItem['S_DATE']), 'Y/n/j H:i').'～';
                    $strSDateTemp = date_format(date_create($arrItem['S_DATE']), 'Y/n/j H:i');
                    $strCompDateTemp = '';
                    // 2020/04/20 AKB Chien - start - update document 2020/04/20
                    $strDoneTextCSV = '';
                    // 2020/04/20 AKB Chien - end - update document 2020/04/20
                    if(!is_null($arrItem['COMP_DATE'])) {
                        $strCompDateTemp = date_format(date_create($arrItem['COMP_DATE']), 'Y/n/j H:i');
                        // 2020/04/20 AKB Chien - start - update document 2020/04/20
                        $strDoneTextCSV = $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_045'];
                        // 2020/04/20 AKB Chien - end - update document 2020/04/20
                    }

                    $strUpdate = '';
                    $strCorrectionFlag = '';
                    if(!is_null($arrItem['UP_DATE'])) {
                        $strUpdate = date_format(date_create($arrItem['UP_DATE']), 'Y/n/j H:i');
                    }

                    if($arrItem['CORRECTION_FLAG'] == 0) {
                        $strCorrectionFlag = $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_041'];
                    }
                    // 2020/03/26 AKB Chien - end - update document 2020/03/26

                    $arrDataContent[] = [
                        iff(is_null($arrItem['TITLE_JPN']), '', trim($arrItem['TITLE_JPN'])),
                        iff(is_null($arrItem['TITLE_ENG']), '', trim($arrItem['TITLE_ENG'])),
                        iff(is_null($arrItem['CONTENTS_JPN']), '', trim($arrItem['CONTENTS_JPN'])),
                        iff(is_null($arrItem['CONTENTS_ENG']), '', trim($arrItem['CONTENTS_ENG'])),
                        $strSDateTemp,
                        $strCompDateTemp,
                        iff(is_null($arrItem['USER_NAME']), '', trim($arrItem['USER_NAME'])),
                        $strUpdate,
                        $strCorrectionFlag,
                        $strDoneTextCSV
                    ];
                }

                $arrResData = array_merge($arrRow1, $arrHead, $arrDataContent);

                // t_request
                $arrRow1 = array([
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_018']
                ]);
                // header csv t_request
                $arrHead = array([
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_019'],
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_020'],
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_021'],
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_022'],
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_023'],
                    $arrTxtTrans['PUBLIC_TEXT_011'],
                    $arrTxtTrans['PUBLIC_TEXT_012'],
                    $arrTxtTrans['PUBLIC_TEXT_013'],
                    $arrTxtTrans['PUBLIC_TEXT_014'],
                    $arrTxtTrans['PUBLIC_TEXT_015'],

                ]);
                $arrDataContent = array();
                foreach($arrResultRequest as $arrItem) {
                    $strFp = $strFPRqAttachRoot.'\\'.$arrItem['REQUEST_NO'];
                    $arrDataContent[] = [
                        date_format(date_create($arrItem['REG_DATE']), 'Y/n/j H:i'),
                        iff(is_null($arrItem['TITLE_JPN']), '', trim($arrItem['TITLE_JPN'])),
                        iff(is_null($arrItem['TITLE_ENG']), '', trim($arrItem['TITLE_ENG'])),
                        iff(is_null($arrItem['CONTENTS_JPN']), '', trim($arrItem['CONTENTS_JPN'])),
                        iff(is_null($arrItem['CONTENTS_ENG']), '', trim($arrItem['CONTENTS_ENG'])),
                        
                        iff(trim($arrItem['TMP_FILE_NAME1']) == '', '',
                            $strFp.'\\1\\'.$arrItem['TMP_FILE_NAME1']),
                        iff(trim($arrItem['TMP_FILE_NAME2']) == '', '',
                            $strFp.'\\2\\'.$arrItem['TMP_FILE_NAME2']),
                        iff(trim($arrItem['TMP_FILE_NAME3']) == '', '',
                            $strFp.'\\3\\'.$arrItem['TMP_FILE_NAME3']),
                        iff(trim($arrItem['TMP_FILE_NAME4']) == '', '',
                            $strFp.'\\4\\'.$arrItem['TMP_FILE_NAME4']),
                        iff(trim($arrItem['TMP_FILE_NAME5']) == '', '',
                            $strFp.'\\5\\'.$arrItem['TMP_FILE_NAME5']),


                    ];
                }

                $arrResData = array_merge($arrResData, $arrRow1, $arrHead, $arrDataContent);

                // t_information
                $arrRow1 = array([
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_024']
                ]);
                // header csv t_information
                $arrHead = array([
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_025'],
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_026'],
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_027'],
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_028'],
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_029'],
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_030'],
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_031'],
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_032'],
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_033'],
                    // 2020/04/20 AKB Chien - start - update document 2020/04/20
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_046'],
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_047'],
                    // 2020/04/20 AKB Chien - end - update document 2020/04/20
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_034'],
                    $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_035'],
                    $arrTxtTrans['PUBLIC_TEXT_011'],
                    $arrTxtTrans['PUBLIC_TEXT_012'],
                    $arrTxtTrans['PUBLIC_TEXT_013'],
                    $arrTxtTrans['PUBLIC_TEXT_014'],
                    $arrTxtTrans['PUBLIC_TEXT_015'],

                ]);
                $arrDataContent = array();
                foreach($arrResultInformation as $arrItem) {
                    $strFp = $strFPInforAttachRoot.'\\'.$arrItem['INFORMATION_NO'];
                    $arrDataContent[] = [
                        date_format(date_create($arrItem['REG_DATE']), 'Y/n/j H:i'),
                        iff(is_null($arrItem['TITLE_JPN']), '', trim($arrItem['TITLE_JPN'])),
                        iff(is_null($arrItem['TITLE_ENG']), '', trim($arrItem['TITLE_ENG'])),
                        iff(is_null($arrItem['CONTENTS_JPN']), '', trim($arrItem['CONTENTS_JPN'])),
                        iff(is_null($arrItem['CONTENTS_ENG']), '', trim($arrItem['CONTENTS_ENG'])),
                        iff(is_null($arrItem['CONFIRM_DATE']), ''
                            , date_format(date_create($arrItem['CONFIRM_DATE']), 'Y/n/j H:i')),
                        iff(is_null($arrItem['CONTACT_INFO']), '', trim($arrItem['CONTACT_INFO'])),
                        iff(is_null($arrItem['COMPANY_NAME_JPN']), '',
                            trim($arrItem['COMPANY_NAME_JPN'])),
                        iff(is_null($arrItem['COMPANY_NAME_ENG']), '',
                            trim($arrItem['COMPANY_NAME_ENG'])),
                        // 2020/04/20 AKB Chien - start - update document 2020/04/20
                        iff(is_null($arrItem['INST_CATEGORY_NAME_JPN']), '',
                            trim($arrItem['INST_CATEGORY_NAME_JPN'])),
                        iff(is_null($arrItem['INST_CATEGORY_NAME_ENG']), '',
                            trim($arrItem['INST_CATEGORY_NAME_ENG'])),
                        // 2020/04/20 AKB Chien - end - update document 2020/04/20
                        iff(is_null($arrItem['INFO_CATEGORY_NAME_JPN']), '',
                            trim($arrItem['INFO_CATEGORY_NAME_JPN'])),
                        iff(is_null($arrItem['INFO_CATEGORY_NAME_ENG']), '',
                            trim($arrItem['INFO_CATEGORY_NAME_ENG'])),
                            
                        iff(trim($arrItem['TMP_FILE_NAME1']) == '', '',
                            $strFp.'\\1\\'.$arrItem['TMP_FILE_NAME1']),
                        iff(trim($arrItem['TMP_FILE_NAME2']) == '', '',
                            $strFp.'\\2\\'.$arrItem['TMP_FILE_NAME2']),
                        iff(trim($arrItem['TMP_FILE_NAME3']) == '', '',
                            $strFp.'\\3\\'.$arrItem['TMP_FILE_NAME3']),
                        iff(trim($arrItem['TMP_FILE_NAME4']) == '', '',
                            $strFp.'\\4\\'.$arrItem['TMP_FILE_NAME4']),
                        iff(trim($arrItem['TMP_FILE_NAME5']) == '', '',
                            $strFp.'\\5\\'.$arrItem['TMP_FILE_NAME5']),

                    ];
                }

                $arrResData = array_merge($arrResData, $arrRow1, $arrHead, $arrDataContent);

                try {
                    // export csv
                    if(fncArray2Csv($arrResData, 'JCMG事案_'.date("Ymd-His").'.csv', 1)) {
                        exit;
                    }
                } catch (\Exception $e) {
                    header('Content-Type: text/html; charset=utf-8');
                    header("Content-Transfer-Encoding: utf-8");
                    fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                        $arrTxtTrans['PUBLIC_MSG_005']);
                    $_SESSION['INCIDENT_MNG_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_005'];
                    header('Location: incident_case_mng.php');
                    exit;
                }
            }

            // backup Data
            if($_POST['mode'] == 5) {
                $strTitleSearch = $_SESSION['INCIDENT_CASE_MNG_SEARCH_TITLE'];
                $strRegDateSearch = $_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_START'];
                $strCompDateSearch = $_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_END'];
                $intChkDoneSearch = 1;

                // write log
                $strLog = DISPLAY_TITLE.'　一括削除「JCMG件名 = '.$strTitleSearch.',';
                $strLog .= '期間（開始）= '. $strRegDateSearch.',';
                $strLog .= '期間（終了）= '. $strCompDateSearch.',';
                $strLog .= '完了を表示 = '.$intChkDoneSearch.'」';
                $strLog .= '(ユーザID = '.$objLoginUserInfo->strUserID.')';
                fncWriteLog(LogLevel['Info'], LogPattern['Button'], $strLog);

                // get data incident
                $arrData = fncGetData($strTitleSearch, $strRegDateSearch, $strCompDateSearch,
                                        $intChkDoneSearch, $intLanguageType, 1, false, true);

                // if has error
                if($arrData == 0) {
                    fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                        $arrTxtTrans['PUBLIC_MSG_007']);
                    $_SESSION['INCIDENT_MNG_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_007'];
                    echo $arrTxtTrans['PUBLIC_MSG_007'];
                    exit;
                } else {
                    if(count($arrData) > 0) {
                        $dtmDateNow = date("Ymd-His");
                        $strFileName = $dtmDateNow.'.csv';
                        // $strFilePathBackup = SHARE_FOLDER.'\\'.ORGANIZE_INCIDENT_FOLDER.'\\'.$dtmDateNow;
                        $strFilePathBackup = SHARE_FOLDER_ORGANIZE.'\\'.ORGANIZE_INCIDENT_FOLDER.'\\'.$dtmDateNow;

                        $strSQLRequest = ' SELECT '
                                       . '      t_request.* '
                                       . ' FROM '
                                       . '      t_request '
                                       . ' WHERE '
                                       . '      t_request.incident_case_no = ? '
                                       . ' ORDER BY t_request.reg_date DESC ';
                        $strSQLInformation = ' SELECT '
                                           . '     ti.INFORMATION_NO  '
                                           . '     , ti.REG_DATE '
                                           . '     , ti.TITLE_JPN '
                                           . '     , ti.TITLE_ENG '
                                           . '     , ti.CONTENTS_JPN '
                                           . '     , ti.CONTENTS_ENG '
                                           . '     , ti.CONFIRM_DATE '
                                           . '     , ti.CONTACT_INFO '
                                           . '     , mc.COMPANY_NO '
                                           . '     , mc.COMPANY_NAME_JPN '
                                           . '     , mc.COMPANY_NAME_ENG '
                                           . '     , mc.ABBREVIATIONS_JPN '
                                           . '     , mc.ABBREVIATIONS_ENG '
                                           . '     , mic.INFO_CATEGORY_NAME_JPN '
                                           . '     , mic.INFO_CATEGORY_NAME_ENG '
                                           . '     , minstc.INST_CATEGORY_NAME_JPN '
                                           . '     , minstc.INST_CATEGORY_NAME_ENG '
                                           . '     , ti.TMP_FILE_NAME1 '
                                           . '     , ti.TMP_FILE_NAME2 '
                                           . '     , ti.TMP_FILE_NAME3 '
                                           . '     , ti.TMP_FILE_NAME4 '
                                           . '     , ti.TMP_FILE_NAME5 '
                                           . ' FROM t_information AS ti '
                                           . '     INNER JOIN t_incident_case AS tic '
                                           . '                  ON ti.INCIDENT_CASE_NO = tic.INCIDENT_CASE_NO '
                                           . '     INNER JOIN m_company AS mc ON ti.COMPANY_NO = mc.COMPANY_NO '
                                           . '     INNER JOIN m_info_category AS mic '
                                           . '                  ON ti.INFO_CATEGORY_NO = mic.INFO_CATEGORY_NO '
                                           . '     INNER JOIN m_inst_category AS minstc '
                                           . '                  ON mc.INST_CATEGORY_NO = minstc.INST_CATEGORY_NO '
                                           . ' WHERE '
                                           . '     ti.incident_case_no = ? '
                                           . ' ORDER BY ti.REG_DATE DESC ';

                        $arrResExport = array();

                        $intNumber = 0;
                        foreach($arrData as $obj) {
                            $intNumber++;

                            // get title jp to creata folder name
                            $strTitleExport = $obj['TITLE_JPN'];
                            if($strTitleExport == '') {
                                $strTitleExport = $obj['TITLE_ENG'];
                            }

                            $strFolderKey = getNumberIncrementWithZero($intNumber).'-'.$strTitleExport;

                            // t_request
                            // execute SQL and get data
                            $arrResultRequest = fncSelectData($strSQLRequest,
                                array($obj['INCIDENT_CASE_NO']), 1, false, DISPLAY_TITLE);
                            // if has error
                            if($arrResultRequest == 0) {
                                fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                                    $arrTxtTrans['PUBLIC_MSG_007']);
                                $_SESSION['INCIDENT_MNG_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_007'];
                                echo $arrTxtTrans['PUBLIC_MSG_007'];
                                die();
                            }

                            // t_information
                            // execute SQL and get data
                            $arrResultInformation = fncSelectData($strSQLInformation,
                                array($obj['INCIDENT_CASE_NO']), 1, false, DISPLAY_TITLE);
                            // if has error
                            if($arrResultInformation == 0) {
                                fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                                    $arrTxtTrans['PUBLIC_MSG_007']);
                                $_SESSION['INCIDENT_MNG_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_007'];
                                echo $arrTxtTrans['PUBLIC_MSG_007'];
                                die();
                            }

                            $arrResData = array();
                            // t_incident_case
                            $arrRow1 = array([
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_011']
                            ]);
                            // header file csv t_incident_case
                            $arrHead = array([
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_012'],
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_013'],
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_014'],
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_015'],
                                // 2020/04/20 AKB Chien - start - update document 2020/04/20
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_042'],
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_043'],
                                // 2020/04/20 AKB Chien - end - update document 2020/04/20
                                // $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_016'],
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_017'],
                                // 2020/03/26 AKB Chien - start - update document 2020/03/26
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_039'],
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_040'],
                                // 2020/03/26 AKB Chien - end - update document 2020/03/26
                                // 2020/04/20 AKB Chien - start - update document 2020/04/20
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_044'],
                                // 2020/04/20 AKB Chien - end - update document 2020/04/20
                            ]);
                            $arrDataContent = array();

                            // 2020/03/26 AKB Chien - start - update document 2020/03/26
                            // $strSDateTemp = date_format(date_create($obj['S_DATE']), 'Y/n/j H:i').'～';
                            $strSDateTemp = date_format(date_create($obj['S_DATE']), 'Y/n/j H:i');
                            $strCompDateTemp = '';
                            // 2020/04/20 AKB Chien - start - update document 2020/04/20
                            $strDoneTextCSV = '';
                            // 2020/04/20 AKB Chien - end - update document 2020/04/20
                            if(!is_null($obj['COMP_DATE'])) {
                                $strCompDateTemp = date_format(date_create($obj['COMP_DATE']), 'Y/n/j H:i');
                                // 2020/04/20 AKB Chien - start - update document 2020/04/20
                                $strDoneTextCSV = $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_045'];
                                // 2020/04/20 AKB Chien - end - update document 2020/04/20
                            }

                            $strUpdate = '';
                            $strCorrectionFlag = '';
                            if(!is_null($obj['UP_DATE'])) {
                                $strUpdate = date_format(date_create($obj['UP_DATE']), 'Y/n/j H:i');
                            }

                            if($obj['CORRECTION_FLAG'] == 0) {
                                $strCorrectionFlag = $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_041'];
                            }
                            // 2020/03/26 AKB Chien - end - update document 2020/03/26

                            $arrDataContent[] = [
                                iff(is_null($obj['TITLE_JPN']), '', trim($obj['TITLE_JPN'])),
                                iff(is_null($obj['TITLE_ENG']), '', trim($obj['TITLE_ENG'])),
                                iff(is_null($obj['CONTENTS_JPN']), '', trim($obj['CONTENTS_JPN'])),
                                iff(is_null($obj['CONTENTS_ENG']), '', trim($obj['CONTENTS_ENG'])),
                                $strSDateTemp,
                                $strCompDateTemp,
                                iff(is_null($obj['USER_NAME']), '', trim($obj['USER_NAME'])),
                                $strUpdate,
                                $strCorrectionFlag,
                                $strDoneTextCSV
                            ];

                            $arrResData = array_merge($arrRow1, $arrHead, $arrDataContent);

                            // t_request
                            $arrRow1 = array([
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_018']
                            ]);
                            // header file csv t_request
                            $arrHead = array([
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_019'],
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_020'],
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_021'],
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_022'],
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_023'],
                                $arrTxtTrans['PUBLIC_TEXT_011'],
                                $arrTxtTrans['PUBLIC_TEXT_012'],
                                $arrTxtTrans['PUBLIC_TEXT_013'],
                                $arrTxtTrans['PUBLIC_TEXT_014'],
                                $arrTxtTrans['PUBLIC_TEXT_015'],
                            ]);
                            $arrDataContent = array();
                            $arrRequestFolderBackup = array();
                            foreach($arrResultRequest as $arrItem) {
                                // get title jp to creata folder name
                                $strTitleExport = $arrItem['TITLE_JPN'];
                                if($strTitleExport == '') {
                                    $strTitleExport = $arrItem['TITLE_ENG'];
                                }

                                $strRequestTitle = $arrItem['REQUEST_NO'].'--'.$strTitleExport;

                                $strFpExport = $strFPRqAttachRoot.'\\'.$arrItem['REQUEST_NO'];
                                $arrDataContent[] = array(
                                    date_format(date_create($arrItem['REG_DATE']), 'Y/n/j H:i'),
                                    iff(is_null($arrItem['TITLE_JPN']), '', trim($arrItem['TITLE_JPN'])),
                                    iff(is_null($arrItem['TITLE_ENG']), '', trim($arrItem['TITLE_ENG'])),
                                    iff(is_null($arrItem['CONTENTS_JPN']), '', trim($arrItem['CONTENTS_JPN'])),
                                    iff(is_null($arrItem['CONTENTS_ENG']), '', trim($arrItem['CONTENTS_ENG'])),

                                    $arrItem['REQUEST_NO'].'-RQ-TMP_FILE_NAME1' => iff(
                                        trim($arrItem['TMP_FILE_NAME1']) == '', '',
                                            $strFpExport.'\\1\\'.$arrItem['TMP_FILE_NAME1']),
                                    $arrItem['REQUEST_NO'].'-RQ-TMP_FILE_NAME2' => iff(
                                        trim($arrItem['TMP_FILE_NAME2']) == '', '',
                                            $strFpExport.'\\2\\'.$arrItem['TMP_FILE_NAME2']),
                                    $arrItem['REQUEST_NO'].'-RQ-TMP_FILE_NAME3' => iff(
                                        trim($arrItem['TMP_FILE_NAME3']) == '', '',
                                            $strFpExport.'\\3\\'.$arrItem['TMP_FILE_NAME3']),
                                    $arrItem['REQUEST_NO'].'-RQ-TMP_FILE_NAME4' => iff(
                                        trim($arrItem['TMP_FILE_NAME4']) == '', '',
                                            $strFpExport.'\\4\\'.$arrItem['TMP_FILE_NAME4']),
                                    $arrItem['REQUEST_NO'].'-RQ-TMP_FILE_NAME5' => iff(
                                        trim($arrItem['TMP_FILE_NAME5']) == '', '',
                                            $strFpExport.'\\5\\'.$arrItem['TMP_FILE_NAME5']),
                                );

                                if($arrItem['TMP_FILE_NAME1'] != '' || $arrItem['TMP_FILE_NAME2'] != ''
								   || $arrItem['TMP_FILE_NAME3'] != '' || $arrItem['TMP_FILE_NAME4'] != ''
							         || $arrItem['TMP_FILE_NAME5'] != ''){
	                                $arrRequestFolderBackup[$strRequestTitle] = array();
	                                if(trim($arrItem['TMP_FILE_NAME1']) != '') {
	                                    $arrRequestFolderBackup[$strRequestTitle][1] = $arrItem['TMP_FILE_NAME1'];
	                                }
	                                if(trim($arrItem['TMP_FILE_NAME2']) != '') {
	                                    $arrRequestFolderBackup[$strRequestTitle][2] = $arrItem['TMP_FILE_NAME2'];
	                                }
	                                if(trim($arrItem['TMP_FILE_NAME3']) != '') {
	                                    $arrRequestFolderBackup[$strRequestTitle][3] = $arrItem['TMP_FILE_NAME3'];
	                                }
	                                if(trim($arrItem['TMP_FILE_NAME4']) != '') {
	                                    $arrRequestFolderBackup[$strRequestTitle][4] = $arrItem['TMP_FILE_NAME4'];
	                                }
	                                if(trim($arrItem['TMP_FILE_NAME5']) != '') {
	                                    $arrRequestFolderBackup[$strRequestTitle][5] = $arrItem['TMP_FILE_NAME5'];
	                                }
	                            }
                            }

                            $arrResData = array_merge($arrResData, $arrRow1, $arrHead, $arrDataContent);

                            // t_information
                            $arrRow1 = array([
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_024']
                            ]);
                            // header file csv t_information
                            $arrHead = array([
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_025'],
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_026'],
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_027'],
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_028'],
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_029'],
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_030'],
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_031'],
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_032'],
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_033'],
                                // 2020/04/20 AKB Chien - start - update document 2020/04/20
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_046'],
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_047'],
                                // 2020/04/20 AKB Chien - end - update document 2020/04/20
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_034'],
                                $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_035'],
                                $arrTxtTrans['PUBLIC_TEXT_011'],
                                $arrTxtTrans['PUBLIC_TEXT_012'],
                                $arrTxtTrans['PUBLIC_TEXT_013'],
                                $arrTxtTrans['PUBLIC_TEXT_014'],
                                $arrTxtTrans['PUBLIC_TEXT_015'],
                            ]);
                            $arrDataContent = array();
                            $arrCompanyName = array();  // array company_name

                            foreach($arrResultInformation as $arrItem) {
                                $strFpExport = $strFPInforAttachRoot.'\\'.$arrItem['INFORMATION_NO'];

                                $arrDataContent[] = array(
                                    date_format(date_create($arrItem['REG_DATE']), 'Y/n/j H:i'),
                                    iff(is_null($arrItem['TITLE_JPN']), '', trim($arrItem['TITLE_JPN'])),
                                    iff(is_null($arrItem['TITLE_ENG']), '', trim($arrItem['TITLE_ENG'])),
                                    iff(is_null($arrItem['CONTENTS_JPN']), '', trim($arrItem['CONTENTS_JPN'])),
                                    iff(is_null($arrItem['CONTENTS_ENG']), '', trim($arrItem['CONTENTS_ENG'])),
                                    iff(is_null($arrItem['CONFIRM_DATE']), ''
                                    	, date_format(date_create($arrItem['CONFIRM_DATE']), 'Y/n/j H:i')),
                                    iff(is_null($arrItem['CONTACT_INFO']), '', trim($arrItem['CONTACT_INFO'])),
                                    iff(is_null($arrItem['COMPANY_NAME_JPN']),
                                        '', trim($arrItem['COMPANY_NAME_JPN'])),
                                    iff(is_null($arrItem['COMPANY_NAME_ENG']),
                                        '', trim($arrItem['COMPANY_NAME_ENG'])),
                                    // 2020/04/20 AKB Chien - start - update document 2020/04/20
                                    iff(is_null($arrItem['INST_CATEGORY_NAME_JPN']), '',
                                        trim($arrItem['INST_CATEGORY_NAME_JPN'])),
                                    iff(is_null($arrItem['INST_CATEGORY_NAME_ENG']), '',
                                        trim($arrItem['INST_CATEGORY_NAME_ENG'])),
                                    // 2020/04/20 AKB Chien - end - update document 2020/04/20
                                    iff(is_null($arrItem['INFO_CATEGORY_NAME_JPN']),
                                        '', trim($arrItem['INFO_CATEGORY_NAME_JPN'])),
                                    iff(is_null($arrItem['INFO_CATEGORY_NAME_ENG']),
                                        '', trim($arrItem['INFO_CATEGORY_NAME_ENG'])),

                                    $arrItem['INFORMATION_NO'].'-IF-TMP_FILE_NAME1' => iff(
                                        trim($arrItem['TMP_FILE_NAME1']) == '',
                                            '', $strFpExport.'\\1\\'.$arrItem['TMP_FILE_NAME1']),
                                    $arrItem['INFORMATION_NO'].'-IF-TMP_FILE_NAME2' =>
                                        iff(trim($arrItem['TMP_FILE_NAME2']) == '',
                                            '', $strFpExport.'\\2\\'.$arrItem['TMP_FILE_NAME2']),
                                    $arrItem['INFORMATION_NO'].'-IF-TMP_FILE_NAME3' =>
                                        iff(trim($arrItem['TMP_FILE_NAME3']) == '',
                                            '', $strFpExport.'\\3\\'.$arrItem['TMP_FILE_NAME3']),
                                    $arrItem['INFORMATION_NO'].'-IF-TMP_FILE_NAME4' =>
                                        iff(trim($arrItem['TMP_FILE_NAME4']) == '',
                                            '', $strFpExport.'\\4\\'.$arrItem['TMP_FILE_NAME4']),
                                    $arrItem['INFORMATION_NO'].'-IF-TMP_FILE_NAME5' =>
                                        iff(trim($arrItem['TMP_FILE_NAME5']) == '',
                                            '', $strFpExport.'\\5\\'.$arrItem['TMP_FILE_NAME5']),
                                );

                                // list file
                                $strTitleExport = $arrItem['TITLE_JPN'];
                                if($strTitleExport == '') {
                                    $strTitleExport = $arrItem['TITLE_ENG'];
                                }

	                            if(trim($arrItem['TMP_FILE_NAME1']) != '' || trim($arrItem['TMP_FILE_NAME2']) != ''
								   || trim($arrItem['TMP_FILE_NAME3']) != '' || trim($arrItem['TMP_FILE_NAME4']) != ''
							       || trim($arrItem['TMP_FILE_NAME5']) != ''){
							       
	                                $strInforTitle = $arrItem['INFORMATION_NO'].'--'.$strTitleExport;
	                                $arrCompanyName[$arrItem['ABBREVIATIONS_JPN']][$strInforTitle] = array();

	                                if(trim($arrItem['TMP_FILE_NAME1']) != '') {
	                                    $arrCompanyName[$arrItem['ABBREVIATIONS_JPN']][$strInforTitle][1] = $arrItem['TMP_FILE_NAME1'];
	                                }
	                                if(trim($arrItem['TMP_FILE_NAME2']) != '') {
	                                    $arrCompanyName[$arrItem['ABBREVIATIONS_JPN']][$strInforTitle][2] = $arrItem['TMP_FILE_NAME2'];
	                                }
	                                if(trim($arrItem['TMP_FILE_NAME3']) != '') {
	                                    $arrCompanyName[$arrItem['ABBREVIATIONS_JPN']][$strInforTitle][3] = $arrItem['TMP_FILE_NAME3'];
	                                }
	                                if(trim($arrItem['TMP_FILE_NAME4']) != '') {
	                                    $arrCompanyName[$arrItem['ABBREVIATIONS_JPN']][$strInforTitle][4] = $arrItem['TMP_FILE_NAME4'];
	                                }
	                                if(trim($arrItem['TMP_FILE_NAME5']) != '') {
	                                    $arrCompanyName[$arrItem['ABBREVIATIONS_JPN']][$strInforTitle][5] = $arrItem['TMP_FILE_NAME5'];
	                                }
	                            }
                            }

                            $arrResData = array_merge($arrResData, $arrRow1, $arrHead, $arrDataContent);

                            // create data to export
                            // data csv
                            $arrResExport[$strFolderKey]['csv'] = $arrResData;
                            // information
                            $arrResExport[$strFolderKey]['information'] = $arrCompanyName;
                            // 依頼事項
                            $arrResExport[$strFolderKey]['request'] = $arrRequestFolderBackup;
                        }

                        if(count($arrResExport) > 0) {
                            $blnFlagHasFailure = false;

                            // backup file
                            if(mkdir($strFilePathBackup, 0755, true)) {
                                foreach($arrResExport as $strFolderIC => $obj) {
                                    // remove special char in folder name
                                    $strFolderICNew = preg_replace('/([\/\\\:*?"><|]*)/', '', $strFolderIC);
                                    $strFolderICNew = preg_replace('/\.+$/', '', $strFolderICNew);

                                    $strPathIncidentFolder = $strFilePathBackup.'\\'.$strFolderICNew;
                                    if(mkdir($strPathIncidentFolder, 0755, true)) {
                                        // create each folder company in folder incident
                                        if(count($obj['information']) > 0) {
                                            foreach($obj['information'] as $strCompanyName => $arrInfoOfCompany) {
                                                // remove special char in folder name
                                                $strFolderCompanyNameNew = preg_replace('/([\/\\\:*?"><|]*)/', '', $strCompanyName);
                                                $strFolderCompanyNameNew = preg_replace('/\.+$/', '', $strFolderCompanyNameNew);

                                                $strPathCompanyFolder = $strPathIncidentFolder.'\\'.$strFolderCompanyNameNew;
                                                // create folder company name
                                                if(mkdir($strPathCompanyFolder, 0755, true)) {
                                                    if(count($arrInfoOfCompany) > 0) {
                                                        $intNumInfor = 0;
                                                        foreach($arrInfoOfCompany as $keyFolder => $objFolderInfo) {
                                                            $intNumInfor++;

                                                            $arrKey = explode("--", $keyFolder);
                                                            $intInformationNo = $arrKey[0];
                                                            $strFolderInfoBackup = $arrKey[1];

                                                            $strFolderInfoBackupNew = preg_replace('/([\/\\\:*?"><|]*)/', '', $strFolderInfoBackup);
                                                            $strFolderInfoBackupNew = preg_replace('/\.+$/', '', $strFolderInfoBackup);

                                                            $strFolderInfoBackupNew = getNumberIncrementWithZero($intNumInfor).'-'.$strFolderInfoBackupNew;
                                                            $strPathFolderInformation = $strPathCompanyFolder.'\\'.$strFolderInfoBackupNew;
                                                            // create each folder information name
                                                            if(mkdir($strPathFolderInformation, 0755, true)) {
                                                                // make file path for request to export csv
                                                                if(count($objFolderInfo) > 0) {
                                                                    foreach($objFolderInfo as $keyTempFile => $strInfoFileName) {
                                                                        foreach($obj['csv'] as &$objValue) {
                                                                            if(count($objValue) > 0) {
                                                                                $strTempKeyCSVFile = $intInformationNo.'-IF-TMP_FILE_NAME'.$keyTempFile;

                                                                                $strPathOld = SHARE_FOLDER.'\\'.INFORMATION_ATTACHMENT_FOLDER.'\\'.$intInformationNo;
                                                                                // path export information file
                                                                                $strPathBackup = $strPathFolderInformation.'\\'.$keyTempFile;

                                                                                if(isset($objValue[$strTempKeyCSVFile])) {
                                                                                    // old file path
                                                                                    $strOldFile = $strPathOld.'\\'.$keyTempFile.'\\'.$strInfoFileName;

                                                                                    // set path to export - request
                                                                                    $strNewFile = $strPathBackup.'\\'.$strInfoFileName;
                                                                                    $objValue[$strTempKeyCSVFile] = $strNewFile;

                                                                                    // copy -> backup folder
                                                                                    if(!is_dir($strPathFolderInformation.'/'.$keyTempFile)) {
                                                                                        if(mkdir($strPathFolderInformation.'/'.trim($keyTempFile), 0755, true)) {
                                                                                            if (file_exists($strOldFile)) {
                                                                                                if (!copy($strOldFile, $strNewFile)) {
                                                                                                    $blnFlagChkFalse = true;
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }

                                                                                    // delete old file + old folder
                                                                                    // deleteFolderWithPath($strPathOld);
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        // create folder request
                                        if(count($obj['request']) > 0) {
                                            $strPathReqFolder = $strPathIncidentFolder.'\\依頼事項';
                                            // create folder 依頼事項
                                            if(mkdir($strPathReqFolder, 0755, true)) {
                                                $intNumReq = 0;
                                                foreach($obj['request'] as $keyFolder => $objFolderInfo) {
                                                    $intNumReq++;

                                                    $arrKey = explode("--", $keyFolder);
                                                    $intRequestNo = $arrKey[0];
                                                    $strFolderReqBackup = $arrKey[1];

                                                    $strFolderReqBackupNew = preg_replace('/([\/\\\:*?"><|]*)/', '', $strFolderReqBackup);
                                                    $strFolderReqBackupNew = preg_replace('/\.+$/', '', $strFolderReqBackupNew);
                                                    $strFolderReqBackupNew = getNumberIncrementWithZero($intNumReq).'-'.$strFolderReqBackupNew;
                                                    $strPathFolderReq = $strPathReqFolder.'\\'.$strFolderReqBackupNew;
                                                    // create each folder request name
                                                    if(mkdir($strPathFolderReq, 0755, true)) {
                                                        // make file path for request to export csv
                                                        if(count($objFolderInfo) > 0) {
                                                            foreach($objFolderInfo as $keyTempFile => $strReqFileName) {
                                                                foreach($obj['csv'] as &$objValue) {
                                                                    if(count($objValue) > 0) {
                                                                        $strTempKeyCSVFile = $intRequestNo.'-RQ-TMP_FILE_NAME'.$keyTempFile;

                                                                        $strPathOld = SHARE_FOLDER.'\\'.REQUEST_ATTACHMENT_FOLDER.'\\'.$intRequestNo;
                                                                        // path export request file
                                                                        $strPathBackup = $strPathFolderReq.'\\'.$keyTempFile;
                                                                        if(isset($objValue[$strTempKeyCSVFile])) {
                                                                            // old file path
                                                                            $strOldFile = $strPathOld.'\\'.$keyTempFile.'\\'.$strReqFileName;

                                                                            // set path to export - request
                                                                            $strNewFile = $strPathBackup.'\\'.$strReqFileName;
                                                                            $objValue[$strTempKeyCSVFile] = $strNewFile;

                                                                            // copy -> backup folder
                                                                            if(!is_dir($strPathFolderReq.'/'.$keyTempFile)) {
                                                                                if(mkdir($strPathFolderReq.'/'.$keyTempFile, 0755, true)) {
                                                                                    if (file_exists($strOldFile)) {
                                                                                        if (!copy($strOldFile, $strNewFile)) {
                                                                                            $blnFlagChkFalse = true;
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }

                                                                            // delete old file + old folder
                                                                            // deleteFolderWithPath($strPathOld);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        // create file csv of each incident_case_no
                                        $intCheckCSV = fncArray2Csv($obj['csv'], $strFileName, 0, $strPathIncidentFolder);
                                        if($intCheckCSV == 0) {
                                            $blnFlagHasFailure = true;
                                            break;
                                        }
                                    }
                                }
                            }

                            if(!$blnFlagHasFailure) {
                                $GLOBALS['g_dbaccess']->funcBeginTransaction();
                                try {
                                    $strSQLDelIncidentCase = ' DELETE FROM t_incident_case '
                                                           . ' WHERE t_incident_case.incident_case_no = :icn ';
                                    $strSQLDelRequest      = ' DELETE FROM t_request '
                                                           . ' WHERE t_request.incident_case_no = :icn ';
                                    $strSQLDelInformation  = ' DELETE FROM t_information '
                                                           . ' WHERE t_information.incident_case_no = :icn ';

                                    $arrReqNo = array();
                                    $arrInforNo = array();
                                    foreach($arrData as $obj) {
                                        // delete file attachment
                                        // t_request
                                        $strSQL = ' SELECT * FROM t_request '
                                                . ' WHERE t_request.incident_case_no = ? ';
                                        // execute SQL and get data
                                        $arrResult = fncSelectData($strSQL, array($obj['INCIDENT_CASE_NO']),
                                                                    1, false, DISPLAY_TITLE);
                                        $blnCheckHasFalse = false;
                                        // if has error
                                        if($arrResult == 0) {
                                            header('Content-Type: text/html; charset=utf-8');
                                            header("Content-Transfer-Encoding: utf-8");
                                            // delete folder backup
                                            deleteFolderBackup($strFilePathBackup);
                                            fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                                                $arrTxtTrans['PUBLIC_MSG_007']);
                                            $_SESSION['INCIDENT_MNG_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_007'];
                                            echo $arrTxtTrans['PUBLIC_MSG_007'];
                                            break;
                                            die();
                                        }
                                        if($arrResult != 0 && count($arrResult) > 0) {
                                            foreach($arrResult as $row) {
                                                $arrReqNo[] = $row['REQUEST_NO'];
                                            }
                                        }

                                        // t_information
                                        $strSQL = ' SELECT * FROM t_information '
                                                . ' WHERE t_information.incident_case_no = ? ';
                                        // execute SQL and get data
                                        $arrResult = fncSelectData($strSQL, array($obj['INCIDENT_CASE_NO']),
                                                                    1, false, DISPLAY_TITLE);
                                        $blnCheckHasFalse = false;
                                        // if has error
                                        if($arrResult == 0) {
                                            header('Content-Type: text/html; charset=utf-8');
                                            header("Content-Transfer-Encoding: utf-8");
                                            // delete folder backup
                                            deleteFolderBackup($strFilePathBackup);
                                            fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                                                $arrTxtTrans['PUBLIC_MSG_007']);
                                            $_SESSION['INCIDENT_MNG_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_007'];
                                            echo $arrTxtTrans['PUBLIC_MSG_007'];
                                            break;
                                            die();
                                        }
                                        // get
                                        if($arrResult != 0 && count($arrResult) > 0) {
                                            foreach($arrResult as $row) {
                                                $arrInforNo[] = $row['INFORMATION_NO'];
                                            }
                                        }

                                        // t_incident_case
                                        $objQuery = $GLOBALS['g_dbaccess']->funcPrepare($strSQLDelIncidentCase);
                                        $objQuery->bindParam(':icn', $obj['INCIDENT_CASE_NO']);

                                        // write log
                                        $strLogSql = $strSQLDelIncidentCase;
                                        $strLogSql = str_replace(':icn', $obj['INCIDENT_CASE_NO'], $strLogSql);
                                        fncWriteLog(LogLevel['Info'], LogPattern['Sql'],
                                            DISPLAY_TITLE.' '.$strLogSql);

                                        $objQuery->execute();

                                        // t_request
                                        $objQuery = $GLOBALS['g_dbaccess']->funcPrepare($strSQLDelRequest);
                                        $objQuery->bindParam(':icn', $obj['INCIDENT_CASE_NO']);

                                        // write log
                                        $strLogSql = $strSQLDelRequest;
                                        $strLogSql = str_replace(':icn', $obj['INCIDENT_CASE_NO'], $strLogSql);
                                        fncWriteLog(LogLevel['Info'], LogPattern['Sql'],
                                            DISPLAY_TITLE.' '.$strLogSql);

                                        $objQuery->execute();

                                        // t_information
                                        $objQuery = $GLOBALS['g_dbaccess']->funcPrepare($strSQLDelInformation);
                                        $objQuery->bindParam(':icn', $obj['INCIDENT_CASE_NO']);

                                        // write log
                                        $strLogSql = $strSQLDelInformation;
                                        $strLogSql = str_replace(':icn', $obj['INCIDENT_CASE_NO'], $strLogSql);
                                        fncWriteLog(LogLevel['Info'], LogPattern['Sql'],
                                            DISPLAY_TITLE.' '.$strLogSql);

                                        $objQuery->execute();
                                    }
                                    $GLOBALS['g_dbaccess']->funcCommit();

                                    // delete root folder
                                    if(count($arrReqNo) > 0) {
                                        foreach($arrReqNo as $intNo) {
                                            $strPathOld = SHARE_FOLDER.'/'.REQUEST_ATTACHMENT_FOLDER.'/'.$intNo;
                                            deleteFolderWithPath($strPathOld);
                                        }
                                    }

                                    if(count($arrInforNo) > 0) {
                                        foreach($arrInforNo as $intNo) {
                                            $strPathOld = SHARE_FOLDER.'/'.INFORMATION_ATTACHMENT_FOLDER.'/'.$intNo;
                                            deleteFolderWithPath($strPathOld);
                                        }
                                    }

                                    exit;
                                } catch(\Exception $e) {
                                    header('Content-Type: text/html; charset=utf-8');
                                    header("Content-Transfer-Encoding: utf-8");
                                    $GLOBALS['g_dbaccess']->funcRollback();
                                    // delete folder backup
                                    deleteFolderBackup($strFilePathBackup);
                                    // write log
                                    fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                                        $arrTxtTrans['PUBLIC_MSG_007']);
                                    $_SESSION['INCIDENT_MNG_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_007'];
                                    echo $arrTxtTrans['PUBLIC_MSG_007'];
                                    exit;
                                }
                            } else {
                                header('Content-Type: text/html; charset=utf-8');
                                header("Content-Transfer-Encoding: utf-8");
                                // write log
                                fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                                    $arrTxtTrans['PUBLIC_MSG_007']);
                                $_SESSION['INCIDENT_MNG_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_007'];
                                echo $arrTxtTrans['PUBLIC_MSG_007'];
                                die();
                            }
                        }
                        exit;
                    }
                }
            }
        }

        if(isset($_POST['loadList'])) {
            $strEvent = (isset($_POST['event']) && trim($_POST['event']) != '')
                        ? trim($_POST['event']) : null;
            if(isset($_POST['page'])) $_SESSION['INCIDENT_CASE_MNG_PAGE'] = $_POST['page'];
            $GLOBALS['currentPage'] = $_SESSION['INCIDENT_CASE_MNG_PAGE'];

            $strLog = '';
            $strLogPattern = '';

            $_SESSION['INCIDENT_MNG_ERROR'] = array();
            if($strEvent == 0) {

            } else {
                // save to session
                $strTxtTitle = $_SESSION['INCIDENT_CASE_MNG_SEARCH_TITLE'];
                if(isset($_POST['txtTitle'])) {
                    $_SESSION['INCIDENT_CASE_MNG_SEARCH_TITLE'] = trim($_POST['txtTitle']);
                }

                $blnFlagSDate = true;
                $blnFlagEDate = true;
                $strSDate = $_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_START'];
                $strEDate = $_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_END'];

                if(isset($_POST['txtDateStart'])) {
                    $strDate = trim($_POST['txtDateStart']);
                    if($strDate != '') {
                        // check format date
                        if(!checkFormatDateTimeInput($strDate)) {
                            $strDate = $strSDate;
                            $blnFlagSDate = false;
                        }
                    }
                    $_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_START'] = $strDate;
                }

                if(isset($_POST['txtDataEnd'])) {
                    $strDate = trim($_POST['txtDataEnd']);
                    if($strDate != '') {
                        // check format date
                        if(!checkFormatDateTimeInput($strDate)) {
                            $strDate = $strEDate;
                            $blnFlagEDate = false;
                        }
                    }
                    $_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_END'] = $strDate;
                }

                $blnFlagAfterBefore = true;
                if($_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_START'] != ''
                    && $_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_END'] != '') {
                    $strSDatePost = trim($_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_START']);
                    $strEDatePost = trim($_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_END']);

                    if($blnFlagSDate && $blnFlagEDate) {
                        if(strtotime($strSDatePost) > strtotime($strEDatePost)) {
                            $_SESSION['INCIDENT_MNG_ERROR'][] = $arrTxtTrans['INCIDENT_CASE_MNG_MSG_002'];
                            $_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_START'] = $strSDate;
                            $_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_END'] = $strEDate;
                            $blnFlagAfterBefore = false;
                        }
                    }
                }

                $intChkDone = $_SESSION['INCIDENT_CASE_MNG_SEARCH_COMP_CHECK'];
                if(isset($_POST['chkDone'])) {
                    $_SESSION['INCIDENT_CASE_MNG_SEARCH_COMP_CHECK'] = (trim($_POST['chkDone'])
                        && trim($_POST['chkDone']) == 'on') ? 1 : 0;
                }

                if(!$blnFlagSDate || !$blnFlagEDate || (!$blnFlagSDate && !$blnFlagEDate)) {
                    $_SESSION['INCIDENT_MNG_ERROR'][] = $arrTxtTrans['INCIDENT_CASE_MNG_MSG_001'];
                    $_SESSION['INCIDENT_CASE_MNG_SEARCH_COMP_CHECK'] = $intChkDone;
                    $_SESSION['INCIDENT_CASE_MNG_SEARCH_TITLE'] = $strTxtTitle;
                    $_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_START'] = $strSDate;
                    $_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_END'] = $strEDate;
                }

                if(!$blnFlagAfterBefore) {
                    $_SESSION['INCIDENT_CASE_MNG_SEARCH_COMP_CHECK'] = $intChkDone;
                    $_SESSION['INCIDENT_CASE_MNG_SEARCH_TITLE'] = $strTxtTitle;
                    $_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_START'] = $strSDate;
                    $_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_END'] = $strEDate;
                }

                if($blnFlagSDate && $blnFlagEDate && count($_SESSION['INCIDENT_MNG_ERROR']) == 0) {
                    if(isset($_POST['originalSearch']) && $_POST['originalSearch'] == 1) {
                        $intChkDoneLog = $_SESSION['INCIDENT_CASE_MNG_SEARCH_COMP_CHECK'];
                        $strLog = DISPLAY_TITLE.'　検索を実施';
                        $strLog .= '「タイトル = '.$_SESSION['INCIDENT_CASE_MNG_SEARCH_TITLE'].',';
                        $strLog .= '期間（開始）= '. $_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_START'].',';
                        $strLog .= '期間（終了）= '. $_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_END'].',';
                        $strLog .= '完了を表示 = '.$intChkDoneLog.'」';
                        $strLog .= '(ユーザID = '.$objLoginUserInfo->strUserID.')';
                        fncWriteLog(LogLevel['Info'], LogPattern['Button'], $strLog);

                        $GLOBALS['currentPage'] = 1;
                        $_SESSION['INCIDENT_CASE_MNG_PAGE'] = 1;
                    }

                }
            }

            // get data of incident_case_no
            $arrData = fncGetData($_SESSION['INCIDENT_CASE_MNG_SEARCH_TITLE'],
                                  $_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_START'],
                                  $_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_END'],
                                  $_SESSION['INCIDENT_CASE_MNG_SEARCH_COMP_CHECK'],
                                  $intLanguageType, $GLOBALS['currentPage'], true);
?>
            <table class="blueTable">
                <thead>
                    <tr>
                        <th class="text-th width-550"><?php
                            echo $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_005'];
                        ?></th>
                        <th class="text-th"><?php
                            echo $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_006'];
                        ?></th>
                        <th class="text-th"><?php
                            echo $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_007'];
                        ?></th>
                        <?php // 2020/03/26 AKB Chien - start - update document 2020/03/26 ?>
                        <th class="text-th"><?php
                            echo $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_036'];
                        ?></th>
                        <th class="text-th"><?php
                            echo $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_037'];
                        ?></th>
                        <?php // 2020/03/26 AKB Chien - end - update document 2020/03/26 ?>
                        <th class="text-th"><?php
                            echo $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_008'];
                        ?></th>
                        <th class="text-th"><?php
                            echo $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_009'];
                        ?></th>
                    </tr>
                </thead>
                <tfoot>
                    <tr>
                        <td colspan="7">
                        	<table width="100%;">
                        		<tr>
                        			<td style="width:30%;">
                                    <?php
										echo str_replace('%1%', $GLOBALS['totalRecord'],
											$arrTxtTrans['PUBLIC_TEXT_016']);
                                    ?>
                        			</td>
									<td style="width:40%;text-align:center;">
										<div class="links in-line">
											<?php fncViewPagination('incident_case_mng_proc.php'); ?>
										</div>
									</td>
                        			<td style="width:30%;">
										<div class="in-line" style="float: right">
										    <form  action="incident_case_mng_proc.php" method="post" id="formCSV">
												<input type="hidden" name="searchData" value="" />
												<input type="hidden" name="X-CSRF-TOKEN" value="<?php echo $_POST['X-CSRF-TOKEN']; ?>" />
												<input type="hidden" name="mode" value="" />
												<input type="hidden" name="incident_case_no" value="" />
												<button type="button" class="tbtn tbtn-defaut btnDataReduct" id="close">
													<?php echo $arrTxtTrans['PUBLIC_BUTTON_012']; ?>
												</button>
												<button type="button" class="tbtn tbtn-defaut tbn-btn-return">
													<?php echo $arrTxtTrans['PUBLIC_BUTTON_015']; ?>
												</button>
										    </form>
										</div>
                        			</td>
                        		</tr>
                        	</table>
                        </td>
                    </tr>
                </tfoot>
                <tbody>
                    <?php
                        if($arrData != 0 && count($arrData) > 0) {
                            foreach ($arrData as $value) {
                    ?>
                        <tr>
                            <td class="text-center" style="word-break: break-all;">
                                <a href="incident_case_view.php" class="load-modal" data-id="<?php
                                    echo $value['incident_case_no']; ?>" data-screen="incident_case_mng"><?php
                                        $strTitle = '';
                                        if($intLanguageType == 0) {
                                            $strTitle = fncHtmlSpecialChars($value['title_jpn']);
                                            if($strTitle == '') {
                                                $strTitle = fncHtmlSpecialChars($value['title_eng']);
                                            }
                                        } else {
                                            $strTitle = fncHtmlSpecialChars($value['title_eng']);
                                            if($strTitle == '') {
                                                $strTitle = fncHtmlSpecialChars($value['title_jpn']);
                                            }
                                        }
                                        echo $strTitle;
                                ?></a>
                            </td>

                            <td class="text-center"><?php
                                    echo date_format(date_create($value['s_date']), 'Y/n/j H:i').'～';
                                    if($value['comp_date'] != '') {
                                        echo date_format(date_create($value['comp_date']), 'Y/n/j H:i');
                                    }
                            ?></td>

                            <td class="text-center"><?php
                                echo fncHtmlSpecialChars($value['user_name']);
                            ?></td>

                            <?php // 2020/03/26 AKB Chien - start - update document 2020/03/26 ?>
                            <td class="text-center"><?php
                                if($value['up_date'] != '') {
                                    echo date_format(date_create($value['up_date']), 'Y/n/j H:i');
                                }
                            ?></td>

                            <td class="text-center"><?php
                                if($value['correction_flag'] == 0) {
                                    echo fncHtmlSpecialChars($arrTxtTrans['INCIDENT_CASE_MNG_TEXT_038']);
                                }
                            ?></td>
                            <?php // 2020/03/26 AKB Chien - end - update document 2020/03/26 ?>

                            <td class="text-center"><?php if($value['comp_date'] != '') {
                                echo $arrTxtTrans['PUBLIC_BUTTON_007']; }
                            ?></td>

                            <td class="text-center">
                                <?php if($value['comp_date'] == '') { ?>
                                    <button class="tbl-btn tbtn-green btnDone" data-id="<?php
                                        echo $value['incident_case_no']; ?>"><?php
                                            echo $arrTxtTrans['PUBLIC_BUTTON_007']; ?></button>
                                <?php } else { ?>
                                    <button class="tbl-btn tbtn-primary btnCancel" data-id="<?php
                                        echo $value['incident_case_no']; ?>"><?php
                                            echo $arrTxtTrans['PUBLIC_BUTTON_008']; ?></button>
                                <?php } ?>
                                <div class="btn-75 btn-95"><?php
                                    if($value['comp_date'] != '') { ?>
                                        <button class="tbl-btn tbtn-orange btnOutput" data-id="<?php
                                            echo $value['incident_case_no']; ?>"><?php
                                                echo $arrTxtTrans['PUBLIC_BUTTON_011']; ?></button>
                                    <?php } ?>
                                </div>
                                <button class="tbl-btn tbtn-red btnDelete" data-id="<?php
                                    echo $value['incident_case_no']; ?>"><?php
                                        echo $arrTxtTrans['PUBLIC_BUTTON_010']; ?></button>
                            </td>
                        </tr>
                    <?php
                            }
                        }
                    ?>
                </tbody>
                </tr>
            </table>
            <script>
                $(function() {
<?php
            $arrErrorMsg = (isset($_SESSION['INCIDENT_MNG_ERROR']))
                            ? $_SESSION['INCIDENT_MNG_ERROR'] : array();
            if($arrData == 0) {
                fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                    $arrTxtTrans['PUBLIC_MSG_001']);
                $arrErrorMsg[] = $arrTxtTrans['PUBLIC_MSG_001'];
            }
            $strHtml = '';
            if(count($arrErrorMsg) > 0) {
                $strHtml = '';
                foreach($arrErrorMsg as $value) {
                    $strHtml .= '<div>'.$value.'</div>';
                }
?>
                    $('.error').append('<?php echo $strHtml; ?>');
<?php       } ?>
                    if($('#chkDone').is(':checked')) {
                        $('.btnDataReduct').prop('disabled', false);
                    } else {
                        $('.btnDataReduct').prop('disabled', true);
                    }

                    var contentError = $.trim($('.error').html());
                    if(contentError.length > 0) {
                        if('<?php
                            echo $_SESSION['INCIDENT_CASE_MNG_SEARCH_COMP_CHECK']; ?>' == 0) {
                            $('#chkDone').prop('checked', false);
                            $('.btnDataReduct').prop('disabled', true);
                        } else {
                            $('#chkDone').prop('checked', true);
                            $('.btnDataReduct').prop('disabled', false);
                        }
                    }
                });
            </script>
<?php
        }

        if(isset($_POST['action'])) {
            if($_POST['action'] == 'checkNumDataReduct') {
                $strTitleSearch = $_SESSION['INCIDENT_CASE_MNG_SEARCH_TITLE'];
                $strRegDateSearch = $_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_START'];
                $strCompDateSearch = $_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_END'];
                $intChkDoneSearch = 1;
                $dataCSV = fncGetData($strTitleSearch, $strRegDateSearch,
                            $strCompDateSearch, $intChkDoneSearch, $intLanguageType, 1, false, true);

                if(!is_array($dataCSV) && $dataCSV == 0) {
                    fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                        $arrTxtTrans['PUBLIC_MSG_007']);
                    $_SESSION['INCIDENT_MNG_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_007'];
                    echo -1;
                    exit;
                }

                echo count($dataCSV);
                exit;
            }
        }
    }

    /**
     * delete all file and folder in path
     *
     * @create 2020/03/13 AKB Chien
     * @update 2020/04/14 KBS T.Mausda  execを使用しフォルダ削除に変更
     * @param string $strPath   string path folder
     * @return boolean true:成功、false:失敗
     */
    function deleteFolderWithPath($strPath) {
        try {
            //if(is_dir($strPath)) {
                // get all file and folder
            //    $arrFileFolder = glob($strPath.'/*');
            //    if(count($arrFileFolder) > 0) {
            //        foreach($arrFileFolder as $arrItem) {
            //            if(!is_file($arrItem)) {
                            	// get all file and folder
            //                $pathTemp = glob($arrItem.'/*');
            //                // remove folder file_name1 -> 5
            //                foreach($pathTemp as $objFile) { // iterate files
            //                    if(is_file($objFile)) {
            //                        unlink($objFile); // delete file
            //                    } else {
                                    // get all file and folder
            //                        $pathTemp_2 = glob($objFile.'/*');
            //                        foreach($pathTemp_2 as $objFile_2) {
            //                            if(is_file($objFile_2)) {
            //                                unlink($objFile_2);
            //                            }
            //                        }
            //                        rmdir($objFile);
            //                    }
            //                }
            //                rmdir($arrItem);
            //            } else {
            //                unlink($arrItem); // delete file
            //            }
            //        }
            //        rmdir($strPath);
            //    }
            //}

            $strPath = str_replace("/", "\\", $strPath);

            exec("rd /s /q " . $strPath);

            return true;
        } catch (\Exception $e) {
            return false;
        }
        return true;
    }

    /**
     * delete all file and folder in path backup
     *
     * @create 2020/03/13 AKB Chien
     * @update 2020/04/14 KBS T.Mausda  execを使用しフォルダ削除に変更
     * @param string $strPath   string path folder
     * @return boolean true:成功、false:失敗
     */
    function deleteFolderBackup($strPath) {
        try {
            //if(is_dir($strPath)) {
                // get all file and folder
            //    $arrFileFolder = glob($strPath.'/*');
            //    if(count($arrFileFolder) > 0) {
            //        foreach($arrFileFolder as $arrItem) {
            //            if(!is_file($arrItem)) {
            //                // get all file and folder
            //                $pathTemp = glob($arrItem.'/*');
            //                // remove folder file_name1 -> 5
            //                foreach($pathTemp as $objFile) { // iterate files
            //                    if(is_file($objFile)) {
            //                        unlink($objFile); // delete file
            //                    } else {
            //                        // get all file and folder
            //                        $pathTemp_2 = glob($objFile.'/*');
            //                        foreach($pathTemp_2 as $objFile_2) {
            //                            if(is_file($objFile_2)) {
            //                                unlink($objFile_2);
            //                            } else {
                                            // get all file and folder
            //                                $pathTemp_3 = glob($objFile_2.'/*');
            //                                foreach($pathTemp_3 as $objFile_3) {
            //                                    if(is_file($objFile_3)) {
            //                                        unlink($objFile_3);
            //                                    } else {
            //                                        // get all file and folder
            //                                        $pathTemp_4 = glob($objFile_3.'/*');
            //                                        foreach($pathTemp_4 as $objFile_4) {
            //                                            if(is_file($objFile_4)) {
            //                                                unlink($objFile_4);
            //                                            }
            //                                        }
            //                                        rmdir($objFile_3);
            //                                    }
            //                                }
            //                                rmdir($objFile_2);
            //                            }
            //                        }
            //                        rmdir($objFile);
            //                    }
            //                }
            //                rmdir($arrItem);
            //            } else {
            //                unlink($arrItem); // delete file
            //            }
            //        }
            //        rmdir($strPath);
            //    }
            //}

            $strPath = str_replace("/", "\\", $strPath);

            exec("rd /s /q " . $strPath);

            return true;
        } catch (\Exception $e) {
            return false;
        }
        return true;
    }
?>