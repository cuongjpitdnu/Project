<?php
/*
 * @JCMG事案登録編集画面
 *
 * @create 2020/03/19 KBS T.Masuda
 * @update 2020/03/26 KBS T.Masuda  仕様変更のため
 *         2020/04/01 KBS T.Masuda  メール対象者に失敗した場合、ロールバック処理を追加
 *                                  AWSキーの定数名が不正の場合、確認メッセージが表示されるように
 */

    require_once('common/common.php');

    header('Content-type: text/html; charset=utf-8');
    header('X-FRAME-OPTIONS: DENY');

    if(fncConnectDB() == false){
        $_SESSION['LOGIN_ERROR'] = 'DB接続に失敗しました。';
        header('Location: login.php');
        exit;
    }

    //ログインしていない場合、ログイン画面を表示する
    if(!isset($_SESSION['LOGINUSER_INFO'])) {
        echo '<script>alert("'.PUBLIC_MSG_008_JPN.' / '.PUBLIC_MSG_008_ENG.'");
                      window.location.href="login.php";</script>';
        exit;
    }

    //ajax通信ではない場合、処理を終了する。
    if(!(!empty($_SERVER['HTTP_X_REQUESTED_WITH'])
        && strtolower($_SERVER['HTTP_X_REQUESTED_WITH']) == 'xmlhttprequest')) {
        exit;
    }

    fncSessionTimeOutCheck(1);

    const SCREEN_NAME = 'JCMG事案登録編集画面';

    //ログインユーザ情報
    $objLoginUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);

    //ログインユーザ言語タイプ
    $intLanguageType = $objLoginUserInfo->intLanguageType;

    //表示テキスト・メッセージ
    $arrTitleMsg =  array(
        'INCIDENT_CASE_EDIT_TEXT_001',
        'INCIDENT_CASE_EDIT_TEXT_002',
        'PUBLIC_TEXT_002',
        'PUBLIC_TEXT_004',
        'PUBLIC_TEXT_005',
        'PUBLIC_TEXT_008',
        'PUBLIC_BUTTON_004',
        'PUBLIC_TEXT_003',
        'PUBLIC_TEXT_004',
        'PUBLIC_TEXT_005',
        'PUBLIC_BUTTON_005',
        'PUBLIC_BUTTON_003',
        'PUBLIC_TEXT_010',
        'PUBLIC_MSG_020',

        'PUBLIC_MSG_009',
        'INCIDENT_CASE_EDIT_MSG_001',
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
        'INCIDENT_CASE_EDIT_MSG_002',

        'INCIDENT_CASE_EDIT_MSG_003',
        'INCIDENT_CASE_EDIT_MSG_004',
        'INCIDENT_CASE_EDIT_MSG_005',

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
        'PUBLIC_MSG_003',
        'PUBLIC_MSG_001',
        'PUBLIC_MSG_006',
        'PUBLIC_MSG_049',
    );



    //タイトル（日本語）入力チェック文字数
    define('LENGTH_TITLE_JPN_TO_ENG', 30);
    //タイトル（英語）入力チェック文字数
    define('LENGTH_TITLE_ENG_TO_JPN', 150);
    //内容（日本語）入力チェック文字数
    define('LENGTH_CONTENT_JPN_TO_ENG', 1000);
    //内容（英語）入力チェック文字数
    define('LENGTH_CONTENT_ENG_TO_JPN', 5000);

    //言語タイプに応じたテキスト・メッセージ
    $arrTextTranslate = getListTextTranslate($arrTitleMsg, $intLanguageType);

    //URLを直接指定した場合
    if($_SERVER['REQUEST_METHOD'] == 'GET'){
        echo '<script>alert("'.$arrTextTranslate['PUBLIC_MSG_049'].'");
                       history.back();</script>';
        exit;
    }

    // check permission when access page
    if($objLoginUserInfo->intIncidentCaseRegPerm != 1) {
        echo 900;
        exit();

    }

    //POSTが存在するか
    if(isset($_POST)) {
        $_SESSION['INCIDENT_CASE_EDIT_ERROR'] = array();

        if(isset($_POST['action'])) {
            //メッセージ格納
            $arrRes = array(
                'trans-error' => '',
                'error'       => '',
                'success'     => '',
                'confirm'     => '',
            );

            //翻訳ボタン処理
            if($_POST['action'] == 'translate') {
                //翻訳言語タイプ
                $strTranslate = @$_POST['cmbTranslation']
                              ? ((trim($_POST['cmbTranslation']) == 'ja') ? 0 : 1) : '';
                //タイトル（原文）
                $strTitleOriginal = @$_POST['txtTitleOriginal']
                                  ? trim($_POST['txtTitleOriginal']) : '';
                //内容（原文）
                $strContentOriginal = @$_POST['txtContentOriginal']
                                    ? trim($_POST['txtContentOriginal']) : '';
                //翻訳を手入力するのチェック状態
                $strManualTranslate = @$_POST['chkManualInput']
                                    ? trim($_POST['chkManualInput']) : '';
                //タイトル（翻訳）
                $strTitleTranslate = @$_POST['txtTitleTranslation']
                                   ? trim($_POST['txtTitleTranslation']) : '';
                //内容（翻訳）
                $strContentTranslate = @$_POST['txtContentTranslation']
                                     ? trim($_POST['txtContentTranslation']) : '';

                //翻訳を手入力するにチェック有り
                if($strManualTranslate == 'on') {
                    //入力された内容（翻訳）、タイトル（翻訳）を格納
                    $arrRes['success'] = array(
                        'titleTranslate' => $strTitleTranslate,
                        'contentTranslate' => $strContentTranslate,
                    );
                    echo json_encode($arrRes);
                    exit;
                }

                //翻訳を手入力するにチェック無し
                if($strManualTranslate == 'off') {
                    //タイトル（翻訳）と内容（翻訳）どちらともに入力がある場合、格納する
                    if($strTitleTranslate != '' || $strContentTranslate != '') {
                        $arrRes['success'] = array(
                            'titleTranslate' => $strTitleTranslate,
                            'contentTranslate' => $strContentTranslate,
                        );
                        echo json_encode($arrRes);
                        exit;
                    }
                }

                // 入力チェック
                $_SESSION['INCIDENT_CASE_EDIT_ERROR']
                        = fncTranInputCheck($strTitleOriginal, $strContentOriginal,'','',
                                            $strTranslate,'',0, $arrTextTranslate);

                //入力チェックエラーが存在する場合、表示する
                if(count($_SESSION['INCIDENT_CASE_EDIT_ERROR']) > 0) {
                    $strHtmlError = '';
                    foreach($_SESSION['INCIDENT_CASE_EDIT_ERROR'] as $strError) {
                        $strHtmlError .= '<div>'.$strError.'</div>';
                    }
                    $arrRes['error'] = $strHtmlError;
                    $arrRes['success'] = array(
                        'titleTranslate'   => '',
                        'contentTranslate' => '',
                    );
                    echo json_encode($arrRes);
                    exit;
                } else {
                    $strTitleTranslate   = '';
                    $strContentTranslate = '';
                    try {
                        //AWSが未設定、定数名が間違っていた場合は翻訳を行わない
                        if(defined ('AWS_ACCESS_KEY') && defined ('AWS_SECRET_KEY')){
                            //タイトルの翻訳
                            $strTitleTranslate   = tranAmazon($strTitleOriginal, $strTranslate);
                            //内容の翻訳
                            $strContentTranslate = tranAmazon($strContentOriginal, $strTranslate);
                        }
                        //翻訳失敗時、翻訳を行わなかった時にエラーを格納
                        if((is_array($strTitleTranslate) && $strTitleTranslate['error'] == 1)
                            || (is_array($strContentTranslate) && $strContentTranslate['error'] == 1)
                            || $strTitleTranslate == '' || $strContentTranslate == ''
                            || ! defined ('AWS_ACCESS_KEY') || ! defined ('AWS_SECRET_KEY')) {
                            $_SESSION['INCIDENT_CASE_EDIT_ERROR'][] =  $arrTextTranslate['PUBLIC_MSG_010'];
                        }

                        //翻訳エラーが存在する場合、表示する
                        if(count($_SESSION['INCIDENT_CASE_EDIT_ERROR']) > 0) {
                            $strHtmlError = '';
                            foreach($_SESSION['INCIDENT_CASE_EDIT_ERROR'] as $strError) {
                                $strHtmlError .= '<div>'.$strError.'</div>';
                            }
                            $arrRes['error']       = $strHtmlError;
                            $arrRes['trans-error'] = 'error';
                            echo json_encode($arrRes);
                            exit;
                        }

                        //翻訳結果を格納
                        $arrRes['success'] = array(
                            'titleTranslate'   => $strTitleTranslate,
                            'contentTranslate' => $strContentTranslate,
                        );
                        echo json_encode($arrRes);
                        exit;

                    } catch (\Exception $e) {
                        //翻訳失敗ログを登録
                        fncWriteLog(LogLevel['Error'] , LogPattern['Error'],
                                    $arrTextTranslate['PUBLIC_MSG_010']);
                        $arrRes['error'] = $arrTextTranslate['PUBLIC_MSG_010'];
                        $arrRes['trans-error'] = 'error';
                        $arrRes['success'] = array(
                            'titleTranslate'   => '',
                            'contentTranslate' => '',
                        );
                        echo json_encode($arrRes);
                        exit;
                    }
                }
            }

            //投稿時の自動翻訳処理
            if($_POST['action'] == 'pre-update' || $_POST['action'] == 'pre-insert') {
                //登録・更新ログ内容
                $strEdtLog = SCREEN_NAME.'　更新(ユーザID = '.$objLoginUserInfo->strUserID.')';
                $strRegLog = SCREEN_NAME.'　登録(ユーザID = '.$objLoginUserInfo->strUserID.')';
                $strLogBtnPost = ($_POST['action'] == 'pre-update') ? $strEdtLog : $strRegLog;
                fncWriteLog(LogLevel['Info'] , LogPattern['Button'], $strLogBtnPost);

                //JCMGNo
                $intIncidentCaseNo = @$_POST['incidentCaseNo']
                                   ? trim($_POST['incidentCaseNo']) : '';
                //開始日
                $dtmStartDate = @$_POST['dtStartDate'] ? trim($_POST['dtStartDate']) : '';
                //翻訳言語
                $strTranslate = @$_POST['cmbTranslation']
                              ? ((trim($_POST['cmbTranslation']) == 'ja') ? 0 : 1) : '';
                //タイトル（原文）
                $strTitleOriginal = @$_POST['txtTitleOriginal']
                                  ? trim($_POST['txtTitleOriginal']) : '';
                //内容（原文）
                $strContentOriginal = @$_POST['txtContentOriginal']
                                    ? trim($_POST['txtContentOriginal']) : '';
                //翻訳を手入力するのチェック状態
                $intManualTranslate = @$_POST['chkManualInput']
                                    ? ((trim($_POST['chkManualInput']) == 'on') ? 1 : 0) : '';
                //タイトル（翻訳）
                $strTitleTranslate   = @$_POST['txtTitleTranslation']
                                     ? trim($_POST['txtTitleTranslation']) : '';
                //内容（翻訳）
                $strContentTranslate = @$_POST['txtContentTranslation']
                                     ? trim($_POST['txtContentTranslation']) : '';

                //新規登録時に開始日の入力チェック
               if($intIncidentCaseNo == ''){
                     //開始日未入力時
                    if(empty($dtmStartDate)){
                         $_SESSION['INCIDENT_CASE_EDIT_ERROR'][]
                                    = $arrTextTranslate['INCIDENT_CASE_EDIT_MSG_003'];
                    }else{
                        $blnFlag = true;

                        $arrTmpDate = explode('/', $dtmStartDate);

						$arrCheckData = explode(' ', $arrTmpDate[2]);

						if(count($arrTmpDate) == 3) {
    						preg_match(
                                '/([1-9]\d{3}\/([1-9]|0[1-9]|1[0-2])\/([1-9]|0[1-9]|[12]\d|3[01]))/',
                                $dtmStartDate,
                                $arrCheckFormatDate
                            );
                            if(count($arrCheckFormatDate) == 0) {
                                $blnFlag = false;
                            }

                            $blnCheckDate = checkdate($arrTmpDate[1], $arrCheckData[0], $arrTmpDate[0]);
                            if(!$blnCheckDate) {
                                //the date is not valid date
                                $blnFlag = false;
                            } else {
                                $strHis = $arrCheckData[1];
                                if(trim($strHis) != '') {
                                    //check hour minute format: H:i
                                    preg_match(
                                        '/^(([0-1]?[0-9])|([2][0-3])):([0-5]?[0-9])$/',
                                        $strHis,
                                        $arrCheckHis
                                    );
                                    //format input is H:i:s exactly (has 1 ":")
                                    if(substr_count($strHis, ':') != 1) {
                                         $blnFlag = false;
                                    }
                                    if(count($arrCheckHis) == 0) {
                                        $blnFlag = false;
                                    }
                                }
							}
						}else {
                            $blnFlag = false;
                        }
                         //入力日付が不正の場合
                         if(!$blnFlag){
                             $_SESSION['INCIDENT_CASE_EDIT_ERROR'][]
                                = $arrTextTranslate['INCIDENT_CASE_EDIT_MSG_004'];
                         }

                    }
               }

                //入力チェック
                $_SESSION['INCIDENT_CASE_EDIT_ERROR']
                     = array_merge($_SESSION['INCIDENT_CASE_EDIT_ERROR'],
                                   fncTranInputCheck($strTitleOriginal, $strContentOriginal,
                                                     $strTitleTranslate,$strContentTranslate,
                                                     $strTranslate,$intManualTranslate,1,
                                                     $arrTextTranslate));

                //入力チェックエラーが存在する場合は、表示する。
                 if(count($_SESSION['INCIDENT_CASE_EDIT_ERROR']) > 0) {
                     $strHtmlError = '';
                     foreach($_SESSION['INCIDENT_CASE_EDIT_ERROR'] as $strError) {
                         $strHtmlError .= '<div>'.$strError.'</div>';
                     }
                     $arrRes['error'] = $strHtmlError;
                     echo json_encode($arrRes);
                     exit;
                 }

                //翻訳を手入力するにチェック無し
                 if($intManualTranslate == 0) {
                    //タイトル（翻訳）、内容（翻訳）がnullの場合
                    if($strTitleTranslate == '' && $strContentTranslate == '') {
                        try {
                                //AWSが未設定、定数名が間違っていた場合は自動翻訳を行わない
                                if(defined ('AWS_ACCESS_KEY') && defined ('AWS_SECRET_KEY')){
                                    $strTitleTranslate   = tranAmazon($strTitleOriginal, $strTranslate);
                                    $strContentTranslate = tranAmazon($strContentOriginal, $strTranslate);
                                }

                                //翻訳失敗時、翻訳処理を行わなかった場合
                                if((is_array($strTitleTranslate) && $strTitleTranslate['error'] == 1)
                                    || (is_array($strContentTranslate) && $strContentTranslate['error'] == 1)
                                    || $strTitleTranslate == '' || $strContentTranslate == ''
                                    || ! defined ('AWS_ACCESS_KEY') || ! defined ('AWS_SECRET_KEY')) {
                                    $arrRes['confirm'] = $arrTextTranslate['PUBLIC_MSG_041'];
                                }else{
                                    //自動翻訳結果を格納
                                    $arrRes['success'] = array(
                                        'titleTranslate' => $strTitleTranslate,
                                        'contentTranslate' => $strContentTranslate,
                                    );
                                }

                        } catch (\Exception $e) {
                            //自動翻訳失敗ログを登録
                            fncWriteLog(LogLevel['Error'] , LogPattern['Error'],
                                        $arrTextTranslate['PUBLIC_MSG_010']);
                            $arrRes['error'] = $arrTextTranslate['PUBLIC_MSG_010'];
                            $arrRes['trans-error'] = 'error';
                            echo json_encode($arrRes);
                            exit;
                        }
                    }


                    //自動翻訳に成功時
                    if($strTitleTranslate != '' && $strContentTranslate != ''
                       && $arrRes['confirm'] == '') {
                        $arrRes['confirm'] = $arrTextTranslate['INCIDENT_CASE_EDIT_MSG_002'];
                        $arrRes['success'] = array(
                            'titleTranslate' => $strTitleTranslate,
                            'contentTranslate' => $strContentTranslate,
                        );
                    } else {
                        //自動翻訳に失敗時
                        if($strTitleTranslate == '' || $strContentTranslate == '') {
                            $arrRes['confirm'] = $arrTextTranslate['PUBLIC_MSG_041'];
                        }
                        $arrRes['success'] = array(
                            'titleTranslate'   => '',
                            'contentTranslate' => '',
                        );
                    }

                //翻訳を手入力するにチェック有り
                } else {
                    $arrRes['success'] = array(
                        'titleTranslate'   => $strTitleTranslate,
                        'contentTranslate' => $strContentTranslate,
                    );
                    $arrRes['confirm'] = $arrTextTranslate['INCIDENT_CASE_EDIT_MSG_002'];
                }
                echo json_encode($arrRes);
                exit;
            }

            //投稿時の登録処理、更新処理
            if($_POST['action'] == 'update' || $_POST['action'] == 'insert') {

                //modeが1の場合
                if($_POST['mode'] == 1) {
                    //更新対象のJCMG事案No
                    $intIncidentNo = @$_POST['id'] ? trim($_POST['id']) : '';
                    //開始日
                    $dtmStartDate = new DateTime(@$_POST['dtStartDate']
                                                   ? trim($_POST['dtStartDate']) : '');
                    $dtmStartDate = $dtmStartDate->format('Y-m-d H:i');
                    //翻訳言語タイプ選択値
                    $strTranslation = @$_POST['cmbTranslation']
                                    ? trim($_POST['cmbTranslation']) : '';
                    //タイトル（原文）
                    $strTitleOriginal = @$_POST['txtTitleOriginal']
                                      ? trim($_POST['txtTitleOriginal']) : '';
                    //内容（原文）
                    $strContentOriginal = @$_POST['txtContentOriginal']
                                        ? trim($_POST['txtContentOriginal']) : '';
                    //タイトル（翻訳）
                    $strTitleTranslation = @$_POST['txtTitleTranslation']
                                         ? trim($_POST['txtTitleTranslation']) : '';
                    //内容（翻訳）
                    $strContentTranslation = @$_POST['txtContentTranslation']
                                           ? trim($_POST['txtContentTranslation']) : '';
                    //翻訳を手入力するチェック状態
                    $intManualInput = @$_POST['chkManualInput']
                                    ? ((trim($_POST['chkManualInput']) == 'on') ? 1 : 0) : '';
                    //JCMG事案の言語タイプ
                    $intLanguageType = ($strTranslation != '')
                                     ? (($strTranslation == 'ja') ? 0 : 1) : '';

                    // 入力データを配列に格納
                    $arrData = array(
                        's_date'          => $dtmStartDate,
                        'title_jpn'       => ($strTranslation != '') ? (($strTranslation == 'ja')
                                            ? $strTitleOriginal : $strTitleTranslation) : null,
                        'title_eng'       => ($strTranslation != '') ? (($strTranslation == 'en')
                                            ? $strTitleOriginal : $strTitleTranslation) : null,
                        'contents_jpn'    => ($strTranslation != '') ? (($strTranslation == 'ja')
                                            ? $strContentOriginal : $strContentTranslation): null,
                        'contents_eng'    => ($strTranslation != '') ? (($strTranslation == 'en')
                                            ? $strContentOriginal : $strContentTranslation): null,
                        'language_type'   => ($strTranslation != '') ? (($strTranslation == 'ja')
                                            ? 0 : 1) : null,
                        'correction_flag' => $intManualInput,
                        'language_type'   => $intLanguageType
                    );

                    //JCMG事案更新
                    if($_POST['action'] == 'update') {
                        try {
                            //トランザクション開始
                            $GLOBALS['g_dbaccess']->funcBeginTransaction();

                            //JCMG事案更新SQL
                            $strSQL = " UPDATE t_incident_case SET "
                                    . " title_jpn = :title_jpn "
                                    . " , title_eng = :title_eng "
                                    . " , contents_jpn = :contents_jpn "
                                    . " , contents_eng = :contents_eng "
                                    . " , language_type = :language_type "
                                    . " , correction_flag = :correction_flag "
                                    . " , up_user_no = :up_user_no "
                                    . " , up_date = CURRENT_TIMESTAMP "
                                    . " WHERE incident_case_no = :incident_case_no; ";

                            $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
                            $objStmt->bindParam(':title_jpn', $arrData['title_jpn']);
                            $objStmt->bindParam(':title_eng', $arrData['title_eng']);
                            $objStmt->bindParam(':contents_jpn', $arrData['contents_jpn']);
                            $objStmt->bindParam(':contents_eng', $arrData['contents_eng']);
                            $objStmt->bindParam(':language_type', $arrData['language_type']);
                            $objStmt->bindParam(':correction_flag', $arrData['correction_flag']);
                            $objStmt->bindParam(':up_user_no', $objLoginUserInfo->intUserNo);
                            $objStmt->bindParam(':incident_case_no', $intIncidentNo);

                            //SQLログ内容
                            $strLogSql = SCREEN_NAME.$strSQL;
                            $strLogSql = str_replace(':title_jpn', $arrData['title_jpn'], $strLogSql);
                            $strLogSql = str_replace(':title_eng', $arrData['title_eng'], $strLogSql);
                            $strLogSql = str_replace(':contents_jpn', $arrData['contents_jpn'], $strLogSql);
                            $strLogSql = str_replace(':contents_eng', $arrData['contents_eng'], $strLogSql);
                            $strLogSql = str_replace(':language_type',
                                                      $arrData['language_type'], $strLogSql);
                            $strLogSql = str_replace(':correction_flag',
                                                     $arrData['correction_flag'], $strLogSql);
                            $strLogSql = str_replace(':up_user_no',
                                                     $objLoginUserInfo->intUserNo, $strLogSql);
                            $strLogSql = str_replace(':incident_case_no', $intIncidentNo, $strLogSql);
                            fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);
                            $objStmt->execute();
                            //2020/04/21 T.Masuda 完了対象のJCMG事案が削除されていた場合、画面を閉じる
                            if(!is_object($objStmt) || $objStmt->rowCount() <= 0){
                                $GLOBALS['g_dbaccess']->funcRollback();
                                fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                                        SCREEN_NAME.' '.$arrTextTranslate['PUBLIC_MSG_003']);
                                $arrRes['error'] = 'alreadyDelError';
                                echo json_encode($arrRes);
                                exit;
                            }
                            //2020/04/21 T.Masuda

                        } catch (\Exception $e) {
                            //ロールバック処理
                            $GLOBALS['g_dbaccess']->funcRollback();
                            //更新失敗時のログを登録
                            fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                                        SCREEN_NAME.' '.$arrTextTranslate['PUBLIC_MSG_003']);
                            $_SESSION['INCIDENT_CASE_EDIT_ERROR'][] = $arrTextTranslate['PUBLIC_MSG_003'];
                            //エラーを表示
                            if(count($_SESSION['INCIDENT_CASE_EDIT_ERROR']) > 0) {
                                $strHtmlError = '';
                                foreach($_SESSION['INCIDENT_CASE_EDIT_ERROR'] as $strError) {
                                    $strHtmlError .= '<div>'.$strError.'</div>';
                                }
                                $arrRes['error'] = $strHtmlError;
                                echo json_encode($arrRes);
                                exit;
                            }
                        }
                    }

                    //JCMG事案登録
                    if($_POST['action'] == 'insert') {
                        //完了していないJCMG事案を取得
                        $arrCompData = funGetCompIncident();
                        //完了していないJCMG事案がある場合、登録処理は行わない
                        if(is_array($arrCompData) && count($arrCompData) > 0) {
                            $_SESSION['INCIDENT_CASE_EDIT_ERROR'][]
                                = $arrTextTranslate['INCIDENT_CASE_EDIT_MSG_005'];
                            //エラーを表示
                            if(count($_SESSION['INCIDENT_CASE_EDIT_ERROR']) > 0) {
                                $strHtmlError = '';
                                foreach($_SESSION['INCIDENT_CASE_EDIT_ERROR'] as $strError) {
                                    $strHtmlError .= '<div>'.$strError.'</div>';
                                }
                                $arrRes['error'] = $strHtmlError;
                                echo json_encode($arrRes);
                                exit;
                            }
                        }
                        //シーケンス（JCMG事案No）を取得
                        $arrNextIncidentNo = getNextIncident();

                        //Jシーケンス（JCMG事案No）の取得失敗時、完了していないJCMG事案の取得失敗時
                        if($arrNextIncidentNo == 0 || $arrCompData == 0) {
                            //取得失敗エラーをログに登録
                            fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                                        SCREEN_NAME.' '.$arrTextTranslate['PUBLIC_MSG_002']);
                            $_SESSION['INCIDENT_CASE_EDIT_ERROR'][] = $arrTextTranslate['PUBLIC_MSG_002'];
                            //エラーを表示
                            if(count($_SESSION['INCIDENT_CASE_EDIT_ERROR']) > 0) {
                                $strHtmlError = '';
                                foreach($_SESSION['INCIDENT_CASE_EDIT_ERROR'] as $strError) {
                                    $strHtmlError .= '<div>'.$strError.'</div>';
                                }
                                $arrRes['error'] = $strHtmlError;
                                echo json_encode($arrRes);
                                exit;
                            }
                        }

                        try {
                            //トランザクション開始
                            $GLOBALS['g_dbaccess']->funcBeginTransaction();

                            //JCMG事案新規登録SQL
                            $strSQL = " INSERT INTO t_incident_case "
                                    . " ( "
                                    . "     incident_case_no, "
                                    . "     s_date, "
                                    . "     title_jpn, "
                                    . "     title_eng, "
                                    . "     contents_jpn, "
                                    . "     contents_eng, "
                                    . "     language_type, "
                                    . "     correction_flag, "
                                    . "     reg_user_no, "
                                    . "     reg_date "
                                    . " ) VALUES ( "
                                    . "     :incident_case_no, "
                                    . "     :s_date, "
                                    . "     :title_jpn, "
                                    . "     :title_eng, "
                                    . "     :contents_jpn, "
                                    . "     :contents_eng, "
                                    . "     :language_type, "
                                    . "     :correction_flag, "
                                    . "     :reg_user_no, "
                                    . "     CURRENT_TIMESTAMP "
                                    . " ); ";
                            $intIncidentNo = $arrNextIncidentNo[0]['incident_case_no'];
                            $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
                            $objStmt->bindParam(':incident_case_no', $intIncidentNo);
                            $objStmt->bindParam(':s_date', $arrData['s_date']);
                            $objStmt->bindParam(':title_jpn', $arrData['title_jpn']);
                            $objStmt->bindParam(':title_eng', $arrData['title_eng']);
                            $objStmt->bindParam(':contents_jpn', $arrData['contents_jpn']);
                            $objStmt->bindParam(':contents_eng', $arrData['contents_eng']);
                            $objStmt->bindParam(':language_type', $arrData['language_type']);
                            $objStmt->bindParam(':correction_flag', $arrData['correction_flag']);
                            $objStmt->bindParam(':reg_user_no', $objLoginUserInfo->intUserNo);

                            //SQLログ内容
                            $strLogSql = SCREEN_NAME.$strSQL;
                            $strLogSql = str_replace(':incident_case_no', $intIncidentNo, $strLogSql);
                            $strLogSql = str_replace(':s_date', $arrData['s_date'], $strLogSql);
                            $strLogSql = str_replace(':title_jpn', $arrData['title_jpn'], $strLogSql);
                            $strLogSql = str_replace(':title_eng', $arrData['title_eng'], $strLogSql);
                            $strLogSql = str_replace(':contents_jpn', $arrData['contents_jpn'], $strLogSql);
                            $strLogSql = str_replace(':contents_eng', $arrData['contents_eng'], $strLogSql);
                            $strLogSql = str_replace(':language_type',
                                                     $arrData['language_type'], $strLogSql);
                            $strLogSql = str_replace(':correction_flag',
                                                     $arrData['correction_flag'], $strLogSql);
                            $strLogSql = str_replace(':reg_user_no',
                                                     $objLoginUserInfo->intUserNo, $strLogSql);
                            fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);
                            $objStmt->execute();
                        } catch (\Exception $e) {
                            //ロールバック処理
                            $GLOBALS['g_dbaccess']->funcRollback();
                            fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                                        SCREEN_NAME.' '.$arrTextTranslate['PUBLIC_MSG_002']);
                            $_SESSION['INCIDENT_CASE_EDIT_ERROR'][] = $arrTextTranslate['PUBLIC_MSG_002'];
                            //エラーを表示
                            if(count($_SESSION['INCIDENT_CASE_EDIT_ERROR']) > 0) {
                                $strHtmlError = '';
                                foreach($_SESSION['INCIDENT_CASE_EDIT_ERROR'] as $strError) {
                                    $strHtmlError .= '<div>'.$strError.'</div>';
                                }
                                $arrRes['error'] = $strHtmlError;
                                echo json_encode($arrRes);
                                exit;
                            }
                        }
                    }

                    //JCMG事案メール送信者を取得
                    $arrUser = getDataMUserSendMail2('INCIDENT_CASE_MAIL', SCREEN_NAME);
                    //JCMG事案メール送信者の取得失敗時
                    if($arrUser == 0) {
                        //ロールバック
                        $GLOBALS['g_dbaccess']->funcRollback();
                        fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                            SCREEN_NAME.' '.$arrTextTranslate['PUBLIC_MSG_001']);
                        $_SESSION['INCIDENT_CASE_EDIT_ERROR'][] = $arrTextTranslate['PUBLIC_MSG_001'];
                        //エラーを表示
                        if(count($_SESSION['INCIDENT_CASE_EDIT_ERROR']) > 0) {
                            $strHtmlError = '';
                            foreach($_SESSION['INCIDENT_CASE_EDIT_ERROR'] as $strError) {
                                $strHtmlError .= '<div>'.$strError.'</div>';
                            }
                            $arrRes['error'] = $strHtmlError;
                            echo json_encode($arrRes);
                            exit;
                        }
                    } else {
                        //コミット
                        $GLOBALS['g_dbaccess']->funcCommit();

                        //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
                        //メール送信対象者（日本語ユーザ）（英語ユーザ）格納配列
                        $arrMail = array(
                            'jpn' => array(),
                            'eng' => array(),
                            'jpn_ext' => array(),
                            'eng_ext' => array(),
                        );
                        //▲2020/06/11 KBS S.Tasaki

                        //メール送信対象ユーザ数、日本語ユーザ、英語ユーザ
                        $intMailJpn = 0;
                        $intMailEng = 0;
                        //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
                        $intExtMailJpn = 0;
                        $intExtMailEng = 0;
                        //▲2020/06/11 KBS S.Tasaki

                        $arrTempMailJP = array();
                        $arrTempMailEN = array();
                        //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
                        $arrTempExtMailJP = array();
                        $arrTempExtMailEN = array();
                        //▲2020/06/11 KBS S.Tasaki

                        //メール件名
                        $arrSubject = array(
                            'jpn' => '',
                            'eng' => '',
                        );

                        //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
                        //メール本文
                        $arrBody = array(
                            'jpn' => '',
                            'eng' => '',
                            'jpn_ext' => '',
                            'eng_ext' => '',
                        );
                        //▲2020/06/11 KBS S.Tasaki

                        //メール本文テンプレートファイル
                        $strFileNameJpn = 'common/mail_temp_jpn.txt';
                        $strFileNameEng = 'common/mail_temp_eng.txt';
                        //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
                        $strExtFileNameJpn = 'common/mail_temp_ext_jpn.txt';
                        $strExtFileNameEng = 'common/mail_temp_ext_eng.txt';
                        //▲2020/06/11 KBS S.Tasaki

                        //メールの件名（日本語、英語）
                        $arrSubject['jpn'] = trim(MAIL_SUBMIT_TITLE_JPN);
                        $arrSubject['eng'] = trim(MAIL_SUBMIT_TITLE_ENG);

                        //日本語日時表示
                        $strBodyJpn = str_replace('%0%',date("m月d日 H時i分"),
                                                  file_get_contents($strFileNameJpn));

                        //JCMG事案専用の本文（日本語）
                        $strBodyJpn = str_replace('%1%',INCIDENT_CASE_EDIT_TEXT_003_JPN,$strBodyJpn);

                        //英語日時表示
                        $strBodyEng = str_replace('%0%',date("H:i,d M"),
                                                  file_get_contents($strFileNameEng));

                        //JCMG事案専用の本文（英語）
                        $strBodyEng = str_replace('%1%',INCIDENT_CASE_EDIT_TEXT_003_ENG,$strBodyEng);

                        //日本語ユーザ用のメール内容を格納
                        $arrBody['jpn'] = fncHtmlSpecialChars($strBodyJpn);
                        $arrBody['jpn'] = str_replace(array("\r\n", "\r","\n"),'<br>',$arrBody['jpn']);

                        //英語ユーザ用のメール内容を格納
                        $arrBody['eng'] = fncHtmlSpecialChars($strBodyEng);
                        $arrBody['eng'] = str_replace(array("\r\n", "\r","\n"),'<br>',$arrBody['eng']);
                        
                        //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
                        //日本語日時表示
                        $strExtBodyJpn = str_replace('%0%',date("m月d日 H時i分"),
                                                  file_get_contents($strExtFileNameJpn));

                        //JCMG事案専用の本文（日本語）
                        $strExtBodyJpn = str_replace('%1%',INCIDENT_CASE_EDIT_TEXT_003_JPN,$strExtBodyJpn);

                        //英語日時表示
                        $strExtBodyEng = str_replace('%0%',date("H:i,d M"),
                                                  file_get_contents($strExtFileNameEng));

                        //JCMG事案専用の本文（英語）
                        $strExtBodyEng = str_replace('%1%',INCIDENT_CASE_EDIT_TEXT_003_ENG,$strExtBodyEng);

                        //日本語ユーザ用のメール内容を格納
                        $arrBody['jpn_ext'] = fncHtmlSpecialChars($strExtBodyJpn);
                        $arrBody['jpn_ext'] = str_replace(array("\r\n", "\r","\n"),'<br>',$arrBody['jpn_ext']);

                        //英語ユーザ用のメール内容を格納
                        $arrBody['eng_ext'] = fncHtmlSpecialChars($strExtBodyEng);
                        $arrBody['eng_ext'] = str_replace(array("\r\n", "\r","\n"),'<br>',$arrBody['eng_ext']);
                        //▲2020/06/11 KBS S.Tasaki
                        

                        //2020/04/25 T.Masuda メール送信者が0人の場合
                        if(count($arrUser) == 0){
                            fncSendMail_2(array(), $arrSubject['jpn'], $arrBody['jpn'], '');
                        }
                        //2020/04/25 T.Masuda

                        //メール対象者の言語タイプにて内容を変更
                        foreach($arrUser as $user) {
                            //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
                            if($user['admin_flag'] == 1){
	                            //日本語ユーザ
	                            if($user['language_type'] == 0) {
	                                //日本語ユーザ、一人目の場合
	                                if(count($arrTempMailJP) == 0) {
	                                    array_push($arrTempMailJP, $user['mail_address']);
	                                    //日本語ユーザ名、メールアドレス格納
	                                    $arrMail['jpn'][] = array(
	                                        'USER_NAME' => $user['user_name'],
	                                        'MAIL_ADDRESS' => $user['mail_address'],
	                                    );
	                                    $intMailJpn = $intMailJpn + 1;
	                                } else {
	                                    if(!in_array($user['mail_address'], $arrTempMailJP)) {
	                                        array_push($arrTempMailJP, $user['mail_address']);
	                                        //日本語ユーザ名、メールアドレス格納
	                                        $arrMail['jpn'][] = array(
	                                            'USER_NAME' => $user['user_name'],
	                                            'MAIL_ADDRESS' => $user['mail_address'],
	                                        );
	                                        $intMailJpn = $intMailJpn + 1;
	                                    }
	                                }
	                            } else {
	                                //英語ユーザ、一人目の場合
	                                if(count($arrTempMailEN) == 0) {
	                                    array_push($arrTempMailEN, $user['mail_address']);
	                                    //英語ユーザ名、メールアドレス格納
	                                    $arrMail['eng'][] = array(
	                                        'USER_NAME' => $user['user_name'],
	                                        'MAIL_ADDRESS' => $user['mail_address'],
	                                    );
	                                    $intMailEng = $intMailEng + 1;
	                                } else {
	                                    if(!in_array($user['mail_address'], $arrTempMailEN)) {
	                                        array_push($arrTempMailEN, $user['mail_address']);
	                                        //英語ユーザ名、メールアドレス格納
	                                        $arrMail['eng'][] = array(
	                                            'USER_NAME' => $user['user_name'],
	                                            'MAIL_ADDRESS' => $user['mail_address'],
	                                        );
	                                        $intMailEng = $intMailEng + 1;
	                                    }
	                                }
	                            }
                            }else{
	                            //日本語ユーザ
	                            if($user['language_type'] == 0) {
	                                //日本語ユーザ、一人目の場合
	                                if(count($arrTempExtMailJP) == 0) {
	                                    array_push($arrTempExtMailJP, $user['mail_address']);
	                                    //日本語ユーザ名、メールアドレス格納
	                                    $arrMail['jpn_ext'][] = array(
	                                        'USER_NAME' => $user['user_name'],
	                                        'MAIL_ADDRESS' => $user['mail_address'],
	                                    );
	                                    $intExtMailJpn = $intExtMailJpn + 1;
	                                } else {
	                                    if(!in_array($user['mail_address'], $arrTempExtMailJP)) {
	                                        array_push($arrTempExtMailJP, $user['mail_address']);
	                                        //日本語ユーザ名、メールアドレス格納
	                                        $arrMail['jpn_ext'][] = array(
	                                            'USER_NAME' => $user['user_name'],
	                                            'MAIL_ADDRESS' => $user['mail_address'],
	                                        );
	                                        $intExtMailJpn = $intExtMailJpn + 1;
	                                    }
	                                }
	                            } else {
	                                //英語ユーザ、一人目の場合
	                                if(count($arrTempExtMailEN) == 0) {
	                                    array_push($arrTempExtMailEN, $user['mail_address']);
	                                    //英語ユーザ名、メールアドレス格納
	                                    $arrMail['eng_ext'][] = array(
	                                        'USER_NAME' => $user['user_name'],
	                                        'MAIL_ADDRESS' => $user['mail_address'],
	                                    );
	                                    $intExtMailEng = $intExtMailEng + 1;
	                                } else {
	                                    if(!in_array($user['mail_address'], $arrTempExtMailEN)) {
	                                        array_push($arrTempExtMailEN, $user['mail_address']);
	                                        //英語ユーザ名、メールアドレス格納
	                                        $arrMail['eng_ext'][] = array(
	                                            'USER_NAME' => $user['user_name'],
	                                            'MAIL_ADDRESS' => $user['mail_address'],
	                                        );
	                                        $intExtMailEng = $intExtMailEng + 1;
	                                    }
	                                }
	                            }
                            }
                            //▲2020/06/11 KBS S.Tasaki
                            

                            //日本語ユーザの一斉送信数を超えた場合、送信する
                            if($intMailJpn >= MAIL_SUBMIT_NUMBER){
                                //日本語送信ユーザ数を初期化する
                                $intMailJpn = 0;
                                //メール送信
                                fncSendMail_2($arrMail['jpn'], $arrSubject['jpn'], $arrBody['jpn'], '');
                                $arrMail['jpn'] = array();
                            }

                            //英語ユーザの一斉送信数を超えた場合、送信する
                            if($intMailEng >= MAIL_SUBMIT_NUMBER){
                                //英語送信ユーザ数を初期化する
                                $intMailEng = 0;
                                //メール送信
                                fncSendMail_2($arrMail['eng'], $arrSubject['eng'], $arrBody['eng'], '');
                                $arrMail['eng'] = array();
                            }
                            
                            //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
                            //日本語ユーザの一斉送信数を超えた場合、送信する
                            if($intExtMailJpn >= MAIL_SUBMIT_NUMBER){
                                //日本語送信ユーザ数を初期化する
                                $intExtMailJpn = 0;
                                //メール送信
                                fncSendMail_2($arrMail['jpn_ext'], $arrSubject['jpn'], $arrBody['jpn_ext'], '');
                                $arrMail['jpn_ext'] = array();
                            }

                            //英語ユーザの一斉送信数を超えた場合、送信する
                            if($intExtMailEng >= MAIL_SUBMIT_NUMBER){
                                //英語送信ユーザ数を初期化する
                                $intExtMailEng = 0;
                                //メール送信
                                fncSendMail_2($arrMail['eng_ext'], $arrSubject['eng'], $arrBody['eng_ext'], '');
                                $arrMail['eng_ext'] = array();
                            }
                            //▲2020/06/11 KBS S.Tasaki
                            
                        }

                        //送信されていない日本語ユーザが存在した場合、メールの送信を行う
                        if(count($arrMail['jpn']) > 0){
                            fncSendMail_2($arrMail['jpn'], $arrSubject['jpn'], $arrBody['jpn'], '');
                        }

                        //送信されていない英語ユーザが存在した場合、メールの送信を行う
                        if(count($arrMail['eng']) > 0){
                            fncSendMail_2($arrMail['eng'], $arrSubject['eng'], $arrBody['eng'], '');
                        }
                        
                        //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
                        //送信されていない日本語ユーザが存在した場合、メールの送信を行う
                        if(count($arrMail['jpn_ext']) > 0){
                            fncSendMail_2($arrMail['jpn_ext'], $arrSubject['jpn'], $arrBody['jpn_ext'], '');
                        }

                        //送信されていない英語ユーザが存在した場合、メールの送信を行う
                        if(count($arrMail['eng_ext']) > 0){
                            fncSendMail_2($arrMail['eng_ext'], $arrSubject['eng'], $arrBody['eng_ext'], '');
                        }
                        //▲2020/06/11 KBS S.Tasaki

                    }

                    $arrRes['success'] = 1;
                    echo json_encode($arrRes);
                    exit;
                }
            }

            //完了ボタンの処理
            if($_POST['action'] == 'done') {
                //完了ボタンログを登録
                $strLog = SCREEN_NAME.'　完了(ユーザID = '.$objLoginUserInfo->strUserID.')';
                fncWriteLog(LogLevel['Info'] , LogPattern['Button'], $strLog);

                //JCMG事案の完了SQL
                $strSQL = ' UPDATE t_incident_case '
                        . ' SET t_incident_case.comp_date = CURRENT_TIMESTAMP, '
                             . 't_incident_case.up_user_no = :up_user_no,'   
                             . 't_incident_case.up_date = CURRENT_TIMESTAMP '  
                        . ' WHERE t_incident_case.incident_case_no = :icn ';

                //トランザクション開始
                $GLOBALS['g_dbaccess']->funcBeginTransaction();
                try {
                    $objQuery = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
                    $objQuery->bindParam(':icn', $_POST['id']);
                    $objQuery->bindParam(':up_user_no', $objLoginUserInfo->intUserNo);

                    //完了SQLをログに登録する
                    $strLogSql = $strSQL;
                    $strLogSql = str_replace(':icn', $_POST['id'], $strLogSql);
                    $strLogSql = str_replace(':up_user_no', $objLoginUserInfo->intUserNo, $strLogSql);
                    fncWriteLog(LogLevel['Info'], LogPattern['Sql'], SCREEN_NAME.' '.$strLogSql);
                    $objQuery->execute();

                    //2020/04/21 T.Masuda 完了対象のJCMG事案が削除されていた場合、画面を閉じる
                    if(!is_object($objQuery) || $objQuery->rowCount() <= 0){
                        $GLOBALS['g_dbaccess']->funcRollback();
                        fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                                SCREEN_NAME.' '.$arrTextTranslate['PUBLIC_MSG_003']);
                        $arrRes['error'] = 'alreadyDelError';
                        echo json_encode($arrRes);
                        exit;
                    }
                    //2020/04/21 T.Masuda

                    //コミット
                    $GLOBALS['g_dbaccess']->funcCommit();

                    $arrRes['success'] = 1;
                    echo json_encode($arrRes);
                    exit;
                } catch (\Exception $e) {
                    //ロールバック
                    $GLOBALS['g_dbaccess']->funcRollback();
                    fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                                SCREEN_NAME.' '.$arrTextTranslate['PUBLIC_MSG_003']);
                    $_SESSION['INCIDENT_CASE_EDIT_ERROR'][] = $arrTextTranslate['PUBLIC_MSG_003'];
                    //エラーを表示
                    if(count($_SESSION['INCIDENT_CASE_EDIT_ERROR']) > 0) {
                        $strHtmlError = '';
                        foreach($_SESSION['INCIDENT_CASE_EDIT_ERROR'] as $strError) {
                            $strHtmlError .= '<div>'.$strError.'</div>';
                        }
                        $arrRes['error'] = $strHtmlError;
                        echo json_encode($arrRes);
                        exit;
                    }
                }
            }
        }
    }


    /**
    *   function	: JCMG事案Noシーケンス取得
    *
    *   @params	    :
    *   @return	    : $arrResult    JCMG事案Noシーケンス　　取得失敗時：0
    *   @create	    : 2020/03/13 T.Masuda
    *   @update	    :
    */
        function getNextIncident() {
        try {
            //シーケンス取得SQL
            $strSQL = ' SELECT NEXT VALUE FOR incident_case_sequence AS incident_case_no ';
            $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
            //SQLログを登録
            $strLogSql = SCREEN_NAME.$strSQL;
            fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);
            $objStmt->execute();
            $arrResult = $objStmt->fetchAll(PDO::FETCH_ASSOC);
            return $arrResult;
        } catch (\Excoption $e) {
            fncWriteLog(LogLevel['Error'], LogPattern['Error'], $e->getMessage());
            return 0;
        }
    }


    /**
     *   function	    : JCMG事案完了していないデータ取得
     *
     *   @params	    :
     *   @return	    : $arrResult    JCMG事案完了データ　　取得失敗時：0
     *   @create	    : 2020/03/19 T.Masuda
     *   @update	    :
     */
    function funGetCompIncident() {
        try {
            //完了していないJCMG事案を取得
            $strSQL = ' SELECT * FROM t_incident_case WHERE comp_date IS NULL ';
            $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
            //SQLログを登録
            $strLogSql = SCREEN_NAME.$strSQL;
            fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);
            $objStmt->execute();
            $arrResult = $objStmt->fetchAll(PDO::FETCH_ASSOC);
            return $arrResult;
        } catch (\Excoption $e) {
            fncWriteLog(LogLevel['Error'], LogPattern['Error'], $e->getMessage());
            return 0;
        }
    }
