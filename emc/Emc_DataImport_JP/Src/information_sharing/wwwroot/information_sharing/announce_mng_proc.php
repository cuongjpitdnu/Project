<?php
    /*
     * @announce_mng_proc.php
     *
     * @create 2020/03/13 AKB Chien
     * @update 2020/04/14 KBS T.Mausda  execを使用しフォルダ削除に変更
     */
    require_once('common/common.php');
    require_once('common/session_check.php');

    header('Content-type: text/html; charset=utf-8');
    header('X-FRAME-OPTIONS: DENY');

    define('DISPLAY_TITLE', 'お知らせ管理画面');

    // check connection
    if(fncConnectDB() == false){
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
        || (isset($_POST['mode']) && $_POST['mode'] == 4)) {
        fncSessionTimeOutCheck(1);
    } else {
        fncSessionTimeOutCheck();
    }

    // ログインユーザ情報を取得
    $objLoginUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);

    $intLanguageType = $objLoginUserInfo->intLanguageType;

    $arrTitleMsg =  array(
        // 2020/03/25 AKB Chien - start - update document 2020/03/25
        'ANNOUNCE_MNG_TEXT_017',
        'ANNOUNCE_MNG_TEXT_018',
        'ANNOUNCE_MNG_TEXT_019',
        'ANNOUNCE_MNG_TEXT_020',
        'ANNOUNCE_MNG_TEXT_021',
        'ANNOUNCE_MNG_TEXT_022',
        'ANNOUNCE_MNG_TEXT_023',
        'ANNOUNCE_MNG_TEXT_024',
        'ANNOUNCE_MNG_TEXT_025',
        'ANNOUNCE_MNG_TEXT_026',
        'ANNOUNCE_MNG_TEXT_027',
        'ANNOUNCE_MNG_TEXT_028',
        'PUBLIC_MSG_049',
        // 2020/03/25 AKB Chien - end - update document 2020/03/25

        'ANNOUNCE_MNG_TEXT_011',
        'ANNOUNCE_MNG_TEXT_005',
        'ANNOUNCE_MNG_TEXT_006',
        'ANNOUNCE_MNG_TEXT_007',
        'ANNOUNCE_MNG_TEXT_008',
        'ANNOUNCE_MNG_TEXT_009',
        'PUBLIC_BUTTON_007',
        'PUBLIC_BUTTON_010',
        'PUBLIC_TEXT_016',
        'PUBLIC_BUTTON_011',
        'PUBLIC_BUTTON_012',
        'PUBLIC_BUTTON_015',
        'ANNOUNCE_MNG_MSG_003',
        'ANNOUNCE_MNG_MSG_004',
        // get data grid fail
        'PUBLIC_MSG_001',
        'PUBLIC_MSG_003',
        'PUBLIC_MSG_004',
        'PUBLIC_MSG_005',
        'PUBLIC_MSG_007',
        // csv title
        'ANNOUNCE_MNG_TEXT_012',
        'ANNOUNCE_MNG_TEXT_013',
        'ANNOUNCE_MNG_TEXT_014',
        'ANNOUNCE_MNG_TEXT_015',
        'ANNOUNCE_MNG_TEXT_016',
        'PUBLIC_TEXT_011',
        'PUBLIC_TEXT_012',
        'PUBLIC_TEXT_013',
        'PUBLIC_TEXT_014',
        'PUBLIC_TEXT_015',

        // 2020/04/20 AKB Chien - start - update document 2020/04/20
        'ANNOUNCE_MNG_TEXT_029',
        'ANNOUNCE_MNG_TEXT_010',
        // 2020/04/20 AKB Chien - end - update document 2020/04/20
        'ANNOUNCE_MNG_MSG_005',

        //2020/04/22 T.Masuda 期間（From）と期間（To）
        'ANNOUNCE_MNG_TEXT_030',
        'ANNOUNCE_MNG_TEXT_031'
        //2020/04/22 T.Masuda
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

    // check if ajax or export csv -> do something | access this file directly->stop
    if(!(isset($_POST['mode']) && $_POST['mode'] == 3) &&
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
     * @param string $strChkDone
     * @param integer $intLang
     * @param integer $intPage
     * @param boolean $blnPaging
     * @param boolean $blnGetAll
     * @return array $arrResult
     */
    function fncGetData($strTitle = '', $strSDate = '',
                        $strEDate = '', $strChkDone = '',
                        $intLang, $intPage,
                        $blnPaging = false, $blnGetAll = false) {
        try {
            $strSuffixes = ($intLang == 0) ? '_JPN' : '_ENG';
            $strTitleSelect = 'title'.$strSuffixes;

            // get data show on screen
            if(!$blnGetAll) {
                $strSQL = ' SELECT '
                        . '     ta.announce_no, '
                        . '     ta.reg_date, '
                        . '     ta.comp_date, '
                        . $strTitleSelect.' AS title, '
                        . '     ta.title_jpn, '
                        . '     ta.title_eng, '
                        // 2020/03/25 AKB Chien - start - update document 2020/03/25
                        . '     ta.data_type, '
                        . '     ta.correction_flag, '
                        . '     ta.up_date, '
                        // 2020/03/25 AKB Chien - end - update document 2020/03/25
                        . '     mu.user_name '
                        . ' FROM '
                        . '     t_announce AS ta ';
            } else {
                // export csv
                $strSQL = ' SELECT '
                        . '     ta.*, '
                        . '     mu.user_name '
                        . ' FROM '
                        . '     t_announce AS ta ';
            }

            $strSQL .= ' INNER JOIN m_user AS mu ON ta.reg_user_no = mu.user_no ';

            $arrCondition = array();
            $blnHaveAnd = false;
            // if title not empty
            if(trim($strTitle) != '') {
                // $strSQL .= ' WHERE ta.'.$strTitleSelect.' LIKE ? ';
                $strSQL .= ' WHERE CASE WHEN ta.title_jpn IS NOT NULL ';
                $strSQL .= ' AND ta.title_eng IS NOT NULL THEN ta.'.$strTitleSelect;
                $strSQL .= ' WHEN ta.title_eng IS NULL THEN ta.title_jpn ';
                $strSQL .= ' ELSE ta.title_eng END LIKE ? ESCAPE \'!\' ';
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
                $strSQL .= ' CAST(ta.reg_date AS DATE) >= ? ';
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
                $strSQL .= ' CAST(ta.comp_date AS DATE) <= ? ';
                $arrCondition[] = $strEDate;
                $blnHaveAnd = true;
            }
            // if checkDone checked
            if(trim($strChkDone) != '' && trim($strChkDone) == 1) {
                if($blnHaveAnd) {
                    $strSQL .= ' AND ';
                } else {
                    $strSQL .= ' WHERE ';
                }
                $strSQL .= ' ta.comp_date IS NOT NULL ';
                $blnHaveAnd = true;
            }

            $strSQL .= ' ORDER BY ta.reg_date DESC ';
            // execute SQL and get data
            $arrResult = fncSelectData($strSQL, $arrCondition, $intPage,
                                        $blnPaging, DISPLAY_TITLE);
            return $arrResult;
        } catch (\Exception $e) {
            // <エラー発生時>
            fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                $arrTxtTrans['PUBLIC_MSG_001']);
            $_SESSION['ANNOUNCE_MNG_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_001'];
            return 0;
        }
    }

    if(isset($_POST)) {
        if(isset($_POST['mode'])) {
            $strFPAttachmentRoot = SHARE_FOLDER.'\\'.ANNOUNCE_ATTACHMENT_FOLDER;

            // set Done data
            if($_POST['mode'] == 1) {
                $strLogButton = DISPLAY_TITLE.'　完了(ユーザID = '.$objLoginUserInfo->strUserID.')';
                fncWriteLog(LogLevel['Info'], LogPattern['Button'], $strLogButton);
                // 完了していないか確認する。
                $strSQL = ' UPDATE t_announce SET t_announce.comp_date = CURRENT_TIMESTAMP,
                                                  t_announce.up_user_no = :up_user_no,
                                                  t_announce.up_date = CURRENT_TIMESTAMP';
                $strSQL .= ' WHERE t_announce.announce_no = :announceNo ';
                //2020/04/22 T.Masuda 削除されたデータのみ0件とするため
                //$strSQL .= ' AND t_announce.comp_date IS NULL ';
                //2020/04/22 T.Masuda
                $GLOBALS['g_dbaccess']->funcBeginTransaction();
                try {
                    $query = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
                    $query->bindParam(':announceNo', $_POST['announce_no']);
                    $query->bindParam(':up_user_no', $objLoginUserInfo->intUserNo);

                    $strLogSql = $strSQL;
                    $strLogSql = str_replace(':announceNo', $_POST['announce_no'], $strLogSql);
                    $strLogSql = str_replace(':up_user_no', $objLoginUserInfo->intUserNo, $strLogSql);

                    // write log
                    fncWriteLog(LogLevel['Info'], LogPattern['Sql'], DISPLAY_TITLE.' '.$strLogSql);

                    $query->execute();

                    //2020/04/22 T.Masuda 完了対象のお知らせが存在しなかった場合
                    if(!is_object($query) || $query->rowCount() <= 0){
                        $GLOBALS['g_dbaccess']->funcRollback();
                        fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                                DISPLAY_TITLE.' '.$arrTxtTrans['PUBLIC_MSG_003']);
                        $_SESSION['ANNOUNCE_MNG_ERROR'][] = $arrTxtTrans['ANNOUNCE_MNG_MSG_005'];
                        echo 3;
                        return;
                    }
                    //2020/04/22 T.Masuda

                    $GLOBALS['g_dbaccess']->funcCommit();

                    echo 1;
                } catch (\Exception $e) {
                    $GLOBALS['g_dbaccess']->funcRollback();
                    // <エラー発生時>
                    fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                        $arrTxtTrans['PUBLIC_MSG_003']);
                    $_SESSION['ANNOUNCE_MNG_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_003'];
                    echo 0;
                }
            }

            // delete data
            if($_POST['mode'] == 2) {
                // 以下の条件で[t_announce]から削除(DELETE)する。
                $strSQL = ' DELETE FROM t_announce WHERE t_announce.announce_no = :announceNo ';
                $GLOBALS['g_dbaccess']->funcBeginTransaction();
                try {
                    $query = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
                    $query->bindParam(':announceNo', $_POST['announce_no']);

                    // write log
                    $strLogSql = $strSQL;
                    $strLogSql = str_replace(':announceNo', $_POST['announce_no'], $strLogSql);
                    fncWriteLog(LogLevel['Info'], LogPattern['Button'],
                        DISPLAY_TITLE.'　削除(ユーザID = '.$objLoginUserInfo->strUserID.')');
                    fncWriteLog(LogLevel['Info'], LogPattern['Sql'], DISPLAY_TITLE.' '.$strLogSql);

                    $query->execute();
                    $GLOBALS['g_dbaccess']->funcCommit();

                    // delete folder announce attachment with announce_no
                    $strPathDelete = $strFPAttachmentRoot.'/'.$_POST['announce_no'];
                    deleteFolderWithPath($strPathDelete);

                    echo 1;
                } catch (\Exception $e) {
                    $GLOBALS['g_dbaccess']->funcRollback();
                    // <エラー発生時>
                    fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                                    $arrTxtTrans['PUBLIC_MSG_004']);
                    $_SESSION['ANNOUNCE_MNG_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_004'];
                    echo 0;
                }
            }

            // export CSV || backup data
            if($_POST['mode'] == 3 || $_POST['mode'] == 4) {
                $strTitleSearch    = $_SESSION['ANNOUNCE_MNG_SEARCH_TITLE'];
                $strRegDateSearch  = $_SESSION['ANNOUNCE_MNG_SEARCH_DATE_START'];
                $strCompDateSearch = $_SESSION['ANNOUNCE_MNG_SEARCH_DATE_END'];
                $intChkDoneSearch  = ($_POST['mode'] == 4) ? 1
                                        : $_SESSION['ANNOUNCE_MNG_SEARCH_COMP_CHECK'];

                $strBtn = ($_POST['mode'] == 3) ? 'CSV出力' : '一括削除';

                $strMsgError = ($_POST['mode'] == 3) ? $arrTxtTrans['PUBLIC_MSG_005']
                                                     : $arrTxtTrans['PUBLIC_MSG_007'];

                // <CSV出力時> / <一括削除時>
                $intChkDoneLog = ($intChkDoneSearch == 1) ? 1 : 0;
                $strLog = DISPLAY_TITLE.'　'.$strBtn.'「タイトル = '.$strTitleSearch.',';
                $strLog .= '期間（開始）= '. $strRegDateSearch.',';
                $strLog .= '期間（終了）= '. $strCompDateSearch.',';
                $strLog .= '完了を表示 = '.$intChkDoneLog.'」';
                $strLog .= '(ユーザID = '.$objLoginUserInfo->strUserID.')';
                fncWriteLog(LogLevel['Info'], LogPattern['Button'], $strLog);

                // get data to export
                $arrDataCSV = fncGetData($strTitleSearch, $strRegDateSearch,
                                        $strCompDateSearch, $intChkDoneLog,
                                        $intLanguageType, 1, false, true);
                $arrDataContent = array();

                if($arrDataCSV == 0) {
                    // <エラー発生時>
                    fncWriteLog(LogLevel['Error'], LogPattern['Error'], $strMsgError);
                    $_SESSION['ANNOUNCE_MNG_ERROR'][] = $strMsgError;
                    if($_POST['mode'] == 3) {
                        header('Location: announce_mng.php');
                        exit;
                    } else {
                        echo $strMsgError;
                        exit;
                    }
                }

                if(count($arrDataCSV) > 0) {
                    $dtmNowDate = date("Ymd-His");
                    $strFileName = ($_POST['mode'] == 3) ? 'お知らせ_'.$dtmNowDate.'.csv'
                                                         : $dtmNowDate.'.csv';
                    // $strFilePathBackup = SHARE_FOLDER.'/'.ORGANIZE_ANNOUNCE_FOLDER.'/'.$dtmNowDate;
                    $strFilePathBackup = SHARE_FOLDER_ORGANIZE.'\\'.ORGANIZE_ANNOUNCE_FOLDER.'\\'.$dtmNowDate;

                    $intNumber = 0;
                    $arrHasFile = array();

                    // prepare data to export CSV / backup data
                    foreach($arrDataCSV as $arrItem) {
                        $intNumber++;

                        // get title jp to creata folder name
                        $strTitleExport = $arrItem['TITLE_JPN'];
                        if($strTitleExport == '') {
                            $strTitleExport = $arrItem['TITLE_ENG'];
                        }

                        $strFolderKey = $arrItem['ANNOUNCE_NO'].'--';
                        $strFolderKey .= getNumberIncrementWithZero($intNumber).'-'.$strTitleExport;

                        $strTitleExport = preg_replace('/([\/\\\:*?"><|]*)/', '', $strTitleExport);
                        // remove all dot in last of string
                        $strTitleExport = preg_replace('/\.+$/', '', $strTitleExport);

                        $strFolderKeyInsert = getNumberIncrementWithZero($intNumber).'-'.$strTitleExport;
                        $strFp = ($_POST['mode'] == 3) ? $strFPAttachmentRoot.'\\'.$arrItem['ANNOUNCE_NO']
                                                       : $strFilePathBackup.'\\'.$strFolderKeyInsert;

                        $strRegDateTemp = date_format(date_create($arrItem['REG_DATE']), 'Y/n/j H:i');
                        //2020/04/22 T.Masuda 期間（From）に変更のため
                        //$strRegDateTemp .= '～';
                        //2020/04/22 T.Masuda
                        $strCompDateTemp = '';
                        // 2020/04/20 AKB Chien - start - update document 2020/04/20
                        $strDoneTextCSV = '';
                        // 2020/04/20 AKB Chien - end - update document 2020/04/20
                        if(!is_null($arrItem['COMP_DATE'])) {
                            $strCompDateTemp = date_format(date_create($arrItem['COMP_DATE']), 'Y/n/j H:i');
                            // 2020/04/20 AKB Chien - start - update document 2020/04/20
                            $strDoneTextCSV = $arrTxtTrans['ANNOUNCE_MNG_TEXT_010'];
                            // 2020/04/20 AKB Chien - end - update document 2020/04/20
                        }

                        // 2020/03/25 AKB Chien - start - update document 2020/03/25
                        $strDataType = '';
                        $strUpdate = '';
                        $strCorrectionFlag = '';
                        if($arrItem['DATA_TYPE'] == 0) {
                            $strDataType = $arrTxtTrans['ANNOUNCE_MNG_TEXT_026'];
                        }
                        if($arrItem['DATA_TYPE'] == 1) {
                            $strDataType = $arrTxtTrans['ANNOUNCE_MNG_TEXT_027'];
                        }

                        if(!is_null($arrItem['UP_DATE'])) {
                            $strUpdate = date_format(date_create($arrItem['UP_DATE']), 'Y/n/j H:i');
                        }

                        if($arrItem['CORRECTION_FLAG'] == 0) {
                            $strCorrectionFlag = $arrTxtTrans['ANNOUNCE_MNG_TEXT_028'];
                        }
                        // 2020/03/25 AKB Chien - end - update document 2020/03/25


                        $arrDataContent[] = [
                            $strDataType,   // 2020/03/25 AKB Chien - update document 2020/03/25
                            iff(is_null($arrItem['TITLE_JPN']), '', trim($arrItem['TITLE_JPN'])),
                            iff(is_null($arrItem['TITLE_ENG']), '', trim($arrItem['TITLE_ENG'])),
                            iff(is_null($arrItem['CONTENTS_JPN']), '', trim($arrItem['CONTENTS_JPN'])),
                            iff(is_null($arrItem['CONTENTS_ENG']), '', trim($arrItem['CONTENTS_ENG'])),
                            //2020/04/22 T.Masuda 表示期間→期間（Froｍ）と期間（To）に変更
                            $strRegDateTemp,
                            $strCompDateTemp,
                            //2020/04/22 T.Masuda
                            iff(is_null($arrItem['user_name']), '', $arrItem['user_name']),

                            // 2020/03/25 AKB Chien - start - update document 2020/03/25
                            $strUpdate,
                            $strCorrectionFlag,
                            // 2020/03/25 AKB Chien - end - update document 2020/03/25

                            // 2020/04/20 AKB Chien - start - update document 2020/04/20
                            $strDoneTextCSV,
                            // 2020/04/20 AKB Chien - end - update document 2020/04/20

                            iff(is_null($arrItem['FILE_NAME1']), '', $strFp.'\\1\\'.$arrItem['FILE_NAME1']),
                            iff(is_null($arrItem['FILE_NAME2']), '', $strFp.'\\2\\'.$arrItem['FILE_NAME2']),
                            iff(is_null($arrItem['FILE_NAME3']), '', $strFp.'\\3\\'.$arrItem['FILE_NAME3']),
                            iff(is_null($arrItem['FILE_NAME4']), '', $strFp.'\\4\\'.$arrItem['FILE_NAME4']),
                            iff(is_null($arrItem['FILE_NAME5']), '', $strFp.'\\5\\'.$arrItem['FILE_NAME5']),
                        ];
                        

				if($arrItem['FILE_NAME1'] != '' || $arrItem['FILE_NAME2'] != ''
				   || $arrItem['FILE_NAME3'] != '' || $arrItem['FILE_NAME4'] != ''
			         || $arrItem['FILE_NAME5'] != ''){

	                        $arrHasFile[$strFolderKey] = array();
	                        if($arrItem['FILE_NAME1'] != '') {
	                            $arrHasFile[$strFolderKey][1] = $arrItem['FILE_NAME1'];
	                        }
	                        if($arrItem['FILE_NAME2'] != '') {
	                            $arrHasFile[$strFolderKey][2] = $arrItem['FILE_NAME2'];
	                        }
	                        if($arrItem['FILE_NAME3'] != '') {
	                            $arrHasFile[$strFolderKey][3] = $arrItem['FILE_NAME3'];
	                        }
	                        if($arrItem['FILE_NAME4'] != '') {
	                            $arrHasFile[$strFolderKey][4] = $arrItem['FILE_NAME4'];
	                        }
	                        if($arrItem['FILE_NAME5'] != '') {
	                            $arrHasFile[$strFolderKey][5] = $arrItem['FILE_NAME5'];
	                        }
	                  }
                    }


                    // header in csv file
                    $arrHeaderTitle = array([
                        // 2020/03/25 AKB Chien - update document 2020/03/25
                        $arrTxtTrans['ANNOUNCE_MNG_TEXT_023'],

                        $arrTxtTrans['ANNOUNCE_MNG_TEXT_011'],
                        $arrTxtTrans['ANNOUNCE_MNG_TEXT_012'],
                        $arrTxtTrans['ANNOUNCE_MNG_TEXT_013'],
                        $arrTxtTrans['ANNOUNCE_MNG_TEXT_014'],

                        //2020/04/22 T.Masuda 表示期間→期間（Froｍ）と期間（To）に変更
                        //$arrTxtTrans['ANNOUNCE_MNG_TEXT_015'],
                        $arrTxtTrans['ANNOUNCE_MNG_TEXT_030'],
                        $arrTxtTrans['ANNOUNCE_MNG_TEXT_031'],
                        //2020/04/22 T.Masuda

                        $arrTxtTrans['ANNOUNCE_MNG_TEXT_016'],

                        // 2020/03/25 AKB Chien - start - update document 2020/03/25
                        $arrTxtTrans['ANNOUNCE_MNG_TEXT_024'],
                        $arrTxtTrans['ANNOUNCE_MNG_TEXT_025'],
                        // 2020/03/25 AKB Chien - end - update document 2020/03/25

                        // 2020/04/20 AKB Chien - start - update document 2020/04/20
                        $arrTxtTrans['ANNOUNCE_MNG_TEXT_029'],
                        // 2020/04/20 AKB Chien - end - update document 2020/04/20

                        $arrTxtTrans['PUBLIC_TEXT_011'],
                        $arrTxtTrans['PUBLIC_TEXT_012'],
                        $arrTxtTrans['PUBLIC_TEXT_013'],
                        $arrTxtTrans['PUBLIC_TEXT_014'],
                        $arrTxtTrans['PUBLIC_TEXT_015'],
                    ]);
                    

                    $arrDataExport = array_merge($arrHeaderTitle, $arrDataContent);

                    // export CSV
                    if($_POST['mode'] == 3) {
                        try {
                            if(fncArray2Csv($arrDataExport, $strFileName, 1)) {
                                exit();
                            }
                        } catch (\Exception $e) {
                            // <エラー発生時>
                            fncWriteLog(LogLevel['Error'], LogPattern['Error'], DISPLAY_TITLE.' '.$strMsgError);
                            $_SESSION['ANNOUNCE_MNG_ERROR'][] = $strMsgError;
                            header('Location: announce_mng.php');
                            exit;
                        }
                    } else {
                        // backup data to file csv in folder YmdHis
                        if(fncArray2Csv($arrDataExport, $strFileName, 0, $strFilePathBackup)) {
                            /* copy file from attachment folder to backup folder
                             * & delete in attachment folder with announce_no */
                            if(backupData($arrHasFile, $strFPAttachmentRoot, $strFilePathBackup)) {
                                $GLOBALS['g_dbaccess']->funcBeginTransaction();
                                try {
                                    // 一括削除対象のお知らせ情報データと添付ファイルを削除する。
                                    $strSQL = ' DELETE FROM t_announce WHERE t_announce.announce_no = :announceNo ';
                                    foreach($arrDataCSV as $arrItem) {
                                        $query = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
                                        $query->bindParam(':announceNo', $arrItem['ANNOUNCE_NO']);

                                        // write log
                                        $strSqlLog = str_replace(':announceNo', $arrItem['ANNOUNCE_NO'], $strSQL);
                                        fncWriteLog(LogLevel['Info'], LogPattern['Sql'], DISPLAY_TITLE.' '.$strSqlLog);

                                        $query->execute();
                                    }
                                    $GLOBALS['g_dbaccess']->funcCommit();

                                    // delete root folder
                                    foreach($arrDataCSV as $arrItem) {
                                        deleteFolderWithPath($strFPAttachmentRoot.'/'.$arrItem['ANNOUNCE_NO']);
                                    }

                                    exit;
                                } catch (\Exception $e) {
                                    header('Content-Type: text/html; charset=utf-8');
			                        header("Content-Transfer-Encoding: utf-8");
                                    deleteFolderWithPath($strFilePathBackup);
                                    $GLOBALS['g_dbaccess']->funcRollback();
                                    // <エラー発生時>
                                    fncWriteLog(LogLevel['Error'], LogPattern['Error'], DISPLAY_TITLE.' '.$strMsgError);
                                    $_SESSION['ANNOUNCE_MNG_ERROR'][] = $strMsgError;
                                    echo $strMsgError;
                                    exit;
                                }
                            }
                        }
                    }
                } else {
                    // <エラー発生時>
                    fncWriteLog(LogLevel['Error'], LogPattern['Error'], DISPLAY_TITLE.' '.$strMsgError);
                    $_SESSION['ANNOUNCE_MNG_ERROR'][] = $strMsgError;
                    if($_POST['mode'] == 3) {
                        header('Location: announce_mng.php');
                    } else {
                        header('Content-Type: text/html; charset=utf-8');
                        header("Content-Transfer-Encoding: utf-8");
                        echo $strMsgError;
                    }
                    exit;
                }
            }
        }

        if(isset($_POST['loadList'])) {
            $strEvent = null;
            if(isset($_POST['event']) && trim($_POST['event']) != '') {
                $strEvent = trim($_POST['event']);
            }
            if(isset($_POST['page'])) $_SESSION['ANNOUNCE_MNG_PAGE'] = $_POST['page'];
            $GLOBALS['currentPage'] = $_SESSION['ANNOUNCE_MNG_PAGE'];

            $strLog = '';
            $strLogPattern = '';

            $_SESSION['ANNOUNCE_MNG_ERROR'] = array();
            if($strEvent == 0) {

            } else {
                $strLogPattern = 'Button';
                // save to session
                $strTxtTitle = $_SESSION['ANNOUNCE_MNG_SEARCH_TITLE'];
                if(isset($_POST['txtTitle'])) {
                    $_SESSION['ANNOUNCE_MNG_SEARCH_TITLE'] = trim($_POST['txtTitle']);
                }

                $blnFlagSDate = true;
                $blnFlagEDate = true;
                $strSDate = $_SESSION['ANNOUNCE_MNG_SEARCH_DATE_START'];
                $strEDate = $_SESSION['ANNOUNCE_MNG_SEARCH_DATE_END'];

                if(isset($_POST['txtDateStart'])) {
                    $strDate = trim($_POST['txtDateStart']);
                    if($strDate != '') {
                        // check format date
                        if(!checkFormatDateTimeInput($strDate)) {
                            $strDate = $strSDate;
                            $blnFlagSDate = false;
                        }
                    }
                    $_SESSION['ANNOUNCE_MNG_SEARCH_DATE_START'] = $strDate;
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
                    $_SESSION['ANNOUNCE_MNG_SEARCH_DATE_END'] = $strDate;
                }

                $blnFlagAfterBefore = true;
                if($_SESSION['ANNOUNCE_MNG_SEARCH_DATE_START'] != ''
                    && $_SESSION['ANNOUNCE_MNG_SEARCH_DATE_END'] != '') {
                    $strSDatePost = trim($_SESSION['ANNOUNCE_MNG_SEARCH_DATE_START']);
                    $strEDatePost = trim($_SESSION['ANNOUNCE_MNG_SEARCH_DATE_END']);

                    if($blnFlagSDate && $blnFlagEDate) {
                        if(strtotime($strSDatePost) > strtotime($strEDatePost)) {
                            $_SESSION['ANNOUNCE_MNG_ERROR'][] = $arrTxtTrans['ANNOUNCE_MNG_MSG_004'];
                            $_SESSION['ANNOUNCE_MNG_SEARCH_DATE_START'] = $strSDate;
                            $_SESSION['ANNOUNCE_MNG_SEARCH_DATE_END'] = $strEDate;
                            $blnFlagAfterBefore = false;
                        }
                    }
                }

                $strChkDone = $_SESSION['ANNOUNCE_MNG_SEARCH_COMP_CHECK'];
                if(isset($_POST['chkDone'])) {
                    $_SESSION['ANNOUNCE_MNG_SEARCH_COMP_CHECK'] = (trim($_POST['chkDone'])
                                                && trim($_POST['chkDone']) == 'on') ? 1 : 0;
                }

                if(!$blnFlagSDate || !$blnFlagEDate || (!$blnFlagSDate && !$blnFlagEDate)) {
                    $_SESSION['ANNOUNCE_MNG_ERROR'][] = $arrTxtTrans['ANNOUNCE_MNG_MSG_003'];
                    $_SESSION['ANNOUNCE_MNG_SEARCH_COMP_CHECK'] = $strChkDone;
                    $_SESSION['ANNOUNCE_MNG_SEARCH_TITLE'] = $strTxtTitle;
                    $_SESSION['ANNOUNCE_MNG_SEARCH_DATE_START'] = $strSDate;
                    $_SESSION['ANNOUNCE_MNG_SEARCH_DATE_END'] = $strEDate;
                }

                if(!$blnFlagAfterBefore) {
                    $_SESSION['ANNOUNCE_MNG_SEARCH_COMP_CHECK'] = $strChkDone;
                    $_SESSION['ANNOUNCE_MNG_SEARCH_TITLE'] = $strTxtTitle;
                    $_SESSION['ANNOUNCE_MNG_SEARCH_DATE_START'] = $strSDate;
                    $_SESSION['ANNOUNCE_MNG_SEARCH_DATE_END'] = $strEDate;
                }

                if($blnFlagSDate && $blnFlagEDate
                    && count($_SESSION['ANNOUNCE_MNG_ERROR']) == 0 && $blnFlagAfterBefore) {
                    // <お知らせ情報検索時>
                    if(isset($_POST['originalSearch']) && $_POST['originalSearch'] == 1) {
	                    $intChkDoneLog = $_SESSION['ANNOUNCE_MNG_SEARCH_COMP_CHECK'];
	                    $strLog = DISPLAY_TITLE.' 検索を実施';
	                    $strLog .= '「タイトル = '.$_SESSION['ANNOUNCE_MNG_SEARCH_TITLE'].',';
	                    $strLog .= '期間（開始）= '. $_SESSION['ANNOUNCE_MNG_SEARCH_DATE_START'].',';
	                    $strLog .= '期間（終了）= '. $_SESSION['ANNOUNCE_MNG_SEARCH_DATE_END'].',';
	                    $strLog .= '完了を表示 = '.$intChkDoneLog.'」';
	                    $strLog .= '(ユーザID = '.$objLoginUserInfo->strUserID.')';
	                    fncWriteLog(LogLevel['Info'], LogPattern[$strLogPattern], $strLog);

	                    $GLOBALS['currentPage'] = 1;
	                    $_SESSION['ANNOUNCE_MNG_PAGE'] = 1;
	               }
                }
            }

            // 検索処理
            $arrData = fncGetData($_SESSION['ANNOUNCE_MNG_SEARCH_TITLE'],
                                  $_SESSION['ANNOUNCE_MNG_SEARCH_DATE_START'],
                                  $_SESSION['ANNOUNCE_MNG_SEARCH_DATE_END'],
                                  $_SESSION['ANNOUNCE_MNG_SEARCH_COMP_CHECK'],
                                  $intLanguageType, $GLOBALS['currentPage'],
                                  true);
?>
            <table class="blueTable">
                <thead>
                    <tr>
                        <?php // 2020/03/25 AKB Chien - start - update document 2020/03/25 ?>
                        <th class="text-th"><?php
                            echo $arrTxtTrans['ANNOUNCE_MNG_TEXT_017']; ?></th>
                        <?php // 2020/03/25 AKB Chien - end - update document 2020/03/25 ?>
                        <th class="text-th width-550"><?php
                            echo $arrTxtTrans['ANNOUNCE_MNG_TEXT_005']; ?></th>
                        <th class="text-th"><?php
                            echo $arrTxtTrans['ANNOUNCE_MNG_TEXT_006']; ?></th>
                        <th class="text-th"><?php
                            echo $arrTxtTrans['ANNOUNCE_MNG_TEXT_007']; ?></th>
                        <?php // 2020/03/25 AKB Chien - start - update document 2020/03/25 ?>
                        <th class="text-th"><?php
                            echo $arrTxtTrans['ANNOUNCE_MNG_TEXT_018']; ?></th>
                        <th class="text-th"><?php
                            echo $arrTxtTrans['ANNOUNCE_MNG_TEXT_019']; ?></th>
                        <?php // 2020/03/25 AKB Chien - end - update document 2020/03/25 ?>
                        <th class="text-th"><?php
                            echo $arrTxtTrans['ANNOUNCE_MNG_TEXT_008']; ?></th>
                        <th class="text-th"><?php
                            echo $arrTxtTrans['ANNOUNCE_MNG_TEXT_009']; ?></th>
                    </tr>
                </thead>
                <tfoot>
                    <tr>
                    	<td colspan="8">
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
											<?php fncViewPagination('announce_mng_proc.php'); ?>
										</div>
                    				</td>
                    				<td style="width:30%;">
										<div class="in-line" style="float: right">
											<form  action="announce_mng_proc.php" method="post" id="formCSV">
												<input type="hidden" name="searchData" value="" />
												<input type="hidden" name="X-CSRF-TOKEN" value="<?php echo $_POST['X-CSRF-TOKEN']; ?>" />
												<input type="hidden" name="mode" value="3" />
												<button type="submit" class="tbtn tbtn-defaut btnOutput">
													<?php echo $arrTxtTrans['PUBLIC_BUTTON_011']; ?>
												</button>
												<button type="submit" class="tbtn tbtn-defaut btnDataReduct" id="close" disabled>
													<?php echo $arrTxtTrans['PUBLIC_BUTTON_012'];?>
												</button>
												<button type="submit" class="tbtn tbtn-defaut tbn-btn-return">
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
                            <?php // 2020/03/25 AKB Chien - start - update document 2020/03/25 ?>
                            <td class="text-center"><?php
                                if($value['data_type'] == 1) {
                                    echo fncHtmlSpecialChars($arrTxtTrans['ANNOUNCE_MNG_TEXT_021']);
                                } else {
                                    echo fncHtmlSpecialChars($arrTxtTrans['ANNOUNCE_MNG_TEXT_020']);
                                }
                            ?></td>
                            <?php // 2020/03/25 AKB Chien - end - update document 2020/03/25 ?>
                            <td class="text-center">
                                <a href="announce_view.php" class="load-modal-announce"
                                    data-id="<?php echo $value['announce_no']; ?>"
                                    data-screen="announce_mng" type="0"><?php
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
                                echo date_format(date_create($value['reg_date']), 'Y/n/j H:i').'～';
                                if($value['comp_date'] != '') {
                                    echo date_format(date_create($value['comp_date']), 'Y/n/j H:i');
                                }
                            ?></td>
                            <td class="text-center"><?php
                                echo fncHtmlSpecialChars($value['user_name']); ?></td>
                            <?php // 2020/03/25 AKB Chien - start - update document 2020/03/25 ?>
                            <td class="text-center"><?php
                                if($value['up_date'] != '') {
                                    echo date_format(date_create($value['up_date']), 'Y/n/j H:i');
                                }
                            ?></td>
                            <td class="text-center"><?php
                                if($value['correction_flag'] == 0) {
                                    echo fncHtmlSpecialChars($arrTxtTrans['ANNOUNCE_MNG_TEXT_022']);
                                }
                            ?></td>
                            <?php // 2020/03/25 AKB Chien - end - update document 2020/03/25 ?>
                            <td class="text-center"><?php
                                if($value['comp_date'] != '') {
                                    echo $arrTxtTrans['PUBLIC_BUTTON_007'];
                                }
                            ?></td>
                            <td class="text-center">
                                <div class="btn-50">
                                    <?php if($value['comp_date'] == '') { ?>
                                        <button class="tbl-btn tbtn-green btnDone"
                                            data-id="<?php echo $value['announce_no']; ?>">
                                            <?php echo $arrTxtTrans['PUBLIC_BUTTON_007']; ?>
                                        </button>
                                    <?php } ?>
                                </div>
                                <button class="tbl-btn tbtn-red btnDelete" data-id="<?php
                                    echo $value['announce_no'];
                                ?>"><?php
                                    echo $arrTxtTrans['PUBLIC_BUTTON_010'];
                                ?></button>
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
            $arrErrorMsg = array();
            if(isset($_SESSION['ANNOUNCE_MNG_ERROR'])) {
                $arrErrorMsg = $_SESSION['ANNOUNCE_MNG_ERROR'];
            }
            if($arrData == 0) {
                // <エラー発生時>
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
                        if('<?php echo $_SESSION['ANNOUNCE_MNG_SEARCH_COMP_CHECK']; ?>' == 0) {
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
                $strTitleSearch = $_SESSION['ANNOUNCE_MNG_SEARCH_TITLE'];
                $strRegDateSearch = $_SESSION['ANNOUNCE_MNG_SEARCH_DATE_START'];
                $strCompDateSearch = $_SESSION['ANNOUNCE_MNG_SEARCH_DATE_END'];
                $intChkDoneSearch = 1;
                $arrDataCSV = fncGetData($strTitleSearch, $strRegDateSearch,
                                         $strCompDateSearch, $intChkDoneSearch,
                                         $intLanguageType, 1, false, true);

                if(!is_array($arrDataCSV) && $arrDataCSV == 0) {
                    // <エラー発生時>
                    fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                        $arrTxtTrans['PUBLIC_MSG_007']);
                    $_SESSION['ANNOUNCE_MNG_ERROR'][] = $arrTxtTrans['PUBLIC_MSG_007'];
                    echo -1;
                    exit;
                }

                // 上記のメッセージを下記で置換し、表示する。
                echo count($arrDataCSV);
                exit;
            }
        }
    }

    /**
     * backup all file in root path to folder backup & del all file in root path
     *
     * @create 2020/03/13 AKB Chien
     * @update 2020/04/14 KBS T.Mausda  execを使用しフォルダ削除に変更
     * @param array $arrData
     * @param string $strPathRootFolder
     * @param string $strPathBackupFolder
     * @return boolean true:成功、false:失敗
     */
    function backupData($arrData, $strPathRootFolder,
                        $strPathBackupFolder, $blnFlagDeleteRoot = false) {
		$strPathRoot = $strPathRootFolder;
		$strPathExportRoot = $strPathBackupFolder;

		try {
			if (!is_dir($strPathExportRoot) && !mkdir($strPathExportRoot, 0755, true)) {
				return false;
			}
			$blnFlagCheckFalse = false;
			// copy -> backup data
			foreach ($arrData as $strFolderKey => $arrFile) {
                $arrKey = explode("--", $strFolderKey);
                $intId = $arrKey[0];
                $strFolderBackup = $arrKey[1];

                // remove special char in folder name
                $strFolderBackup = preg_replace('/([\/\\\:*?"><|]*)/', '', $strFolderBackup);
                $strFolderBackup = preg_replace('/\.+$/', '', $strFolderBackup);

                $strPath = $strPathExportRoot.'/'.$strFolderBackup;
                // make folder 0xxx-titlename
				if (!mkdir($strPath, 0755, true)) {
					$blnFlagCheckFalse = true;
				} else {
					if(count($arrFile) > 0) {
						foreach ($arrFile as $strFolder => $strFile) {
                            // remove special char in folder name
                            $strFolder = preg_replace('/([\/\\\:*?"><|]*)/', '', $strFolder);
                            $strFolder = preg_replace('/\.+$/', '', $strFolder);
                            // make folder 1 -> 5 to copy
							if (!mkdir($strPath.'/'.$strFolder, 0755, true)) {
								$blnFlagCheckFalse = true;
							} else {
								$strOldFile = $strPathRoot.'/'.$intId.'/'.$strFolder.'/'.$strFile;
								$strNewFile = $strPath.'/'.$strFolder.'/'.$strFile;

								if (file_exists($strOldFile)) {
                                    // copy -> backup folder
									if (!copy($strOldFile, $strNewFile)) {
										$blnFlagCheckFalse = true;
                                    }
                                }

                                if($blnFlagDeleteRoot) {
                                    $strPathFolderFileOfAnnounce = $strPathRoot.'/'.$intId.'/'.$strFolder;
                                    if(is_dir($strPathFolderFileOfAnnounce)) {
                                        // get all file names
                                        $arrFiles = glob($strPathFolderFileOfAnnounce.'/*');
                                        foreach($arrFiles as $objFile) { // iterate files
                                            // check if is file -> delete
                                            if(is_file($objFile)) {
                                                unlink($objFile); // delete file
                                            } else {
                                                // get all file and folder
                                                $arrTempFolderFile = glob($objFile.'/*');
                                                // iterate files
                                                foreach($arrTempFolderFile as $objFileInFolder) {
                                                    if(is_file($objFileInFolder)) {
                                                        unlink($objFileInFolder); // delete file
                                                    }
                                                }
                                                if(is_dir($arrTempFolderFile)) {
                                                    rmdir($arrTempFolderFile);
                                                }
                                            }
                                        }
                                        if(is_dir($strPathFolderFileOfAnnounce)) {
                                            // remove folder file_name1 -> 5
                                            rmdir($strPathFolderFileOfAnnounce);
                                        }
                                    }
                                }
							}
                        }
                        if($blnFlagDeleteRoot) {
                            if(is_dir($strPathRoot.'/'.$intId)) {
                                // remove folder announce_no
                                rmdir($strPathRoot.'/'.$intId);
                            }
                        }
					}
				}
			}
		} catch (\Exception $e) {
			return false;
		}
		return true;
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
            //if(is_dir($strPath)) {
                // get all file and folder
            //    $arrFileFolder = glob($strPath.'/*');
            //    if(count($arrFileFolder) > 0) {
            //        foreach($arrFileFolder as $arrItem) {
            //            if(!is_file($arrItem)) {
                            // get all file and folder
            //               $pathTemp = glob($arrItem.'/*');
                            // remove folder file_name1 -> 5
            //                foreach($pathTemp as $objFile) { // iterate files
            //                    if(is_file($objFile)) {
            //                        unlink($objFile); // delete file
            //                    } else {
            //                        	// get all file and folder
            //                        $pathTemp_2 = glob($objFile.'/*');
            //                        foreach($pathTemp_2 as $objFile_2) {
            //                            if(is_file($objFile_2)) {
                                            // delete each file in folder
            //                                unlink($objFile_2);
            //                            }
            //                        }
            //                        rmdir($objFile);    // delete folder
            //                    }
            //                }
            //                rmdir($arrItem);    // delete folder
            //            } else {
            //                unlink($arrItem);   // delete file
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