<?php
    /*
     * @bulletin_board_view.php
     *
     * @create 2020/03/24 AKB Chien
     * @update
     */
    require_once('common/common.php');
    require_once('common/session_check.php');

    header('Content-type: text/html; charset=utf-8');
    header('X-FRAME-OPTIONS: DENY');

    define('DISPLAY_TITLE', '掲示板表示画面');

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
    fncSessionTimeOutCheck();

    // ログインユーザ情報を取得
    $objLoginUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);

    $intLanguageType = $objLoginUserInfo->intLanguageType;

    $arrTitleMsg =  array(
        'PUBLIC_MSG_001',
        'BULLETIN_BOARD_VIEW_TEXT_001',
        'PUBLIC_TEXT_001',
        'BULLETIN_BOARD_VIEW_TEXT_002',
        'BULLETIN_BOARD_VIEW_TEXT_003',
        'PUBLIC_TEXT_002',
        'PUBLIC_TEXT_003',
        'BULLETIN_BOARD_VIEW_TEXT_004',
        'PUBLIC_TEXT_002',
        'PUBLIC_TEXT_003',
        'BULLETIN_BOARD_VIEW_TEXT_005',
        'PUBLIC_TEXT_002',
        'PUBLIC_TEXT_003',
        'PUBLIC_BUTTON_002',
        'PUBLIC_BUTTON_003',
        'BULLETIN_BOARD_VIEW_MSG_001',

        // 2020/03/30 AKB Chien - start - update document 2020/03/30
        'PUBLIC_MSG_049',
        // 2020/03/30 AKB Chien - end - update document 2020/03/30

        // 2020/04/01 AKB Chien - start - update document 2020/04/01
        'PUBLIC_MSG_009',
        // 2020/04/01 AKB Chien - end - update document 2020/04/01

        // 2020/04/14 KBS T.Masuda 画面変更のため
        'BULLETIN_BOARD_VIEW_TEXT_002'
        // 2020/04/14 KBS T.Masuda
    );

    // write log when access this screen
    $strLog = DISPLAY_TITLE.'　表示(ユーザID = '.$objLoginUserInfo->strUserID.') ';
    $strLog .= isset($_SERVER['HTTP_REFERER']) ? $_SERVER['HTTP_REFERER'] : null;
    fncWriteLog(LogLevel['Info'], LogPattern['View'], $strLog);

    // get list text translate
    $arrTxtTrans = getListTextTranslate($arrTitleMsg, $intLanguageType);

    $intIsFromPortalPage = (@$_POST['isAjax']) ? 1 : -1;

    // check has bulletin_board_no
    $intId = @$_POST['id'] ? $_POST['id'] : '';

    //2020/04/17 T.Masuda 遷移元画面名を取得
    $strScreen = @$_POST['screen'] ? $_POST['screen'] : '';
    //2020/04/17 T.Masuda

    $arrResult = array(
        'BULLETIN_BOARD_NO' => '',
        'OCCURRENCE_DATE' => '',
        'CORRECTION_FLAG' => '',
        'BUSINESS_NAME' => '',
        'BUSINESS_NAME_ENG' => '',
        'INCIDENT_NAME_JPN' => '',
        'INCIDENT_NAME_ENG' => '',
        'PLACE_NAME_JPN' => '',
        'PLACE_NAME_ENG' => '',
    );

    // get data infor of bulletin_board_no
    $arrDataDetail = getInfoBullentin($intId);
    // prepare data to show
    if($arrDataDetail != 0 && count($arrDataDetail) > 0) {
        $arrResult = array(
            'BULLETIN_BOARD_NO' => isset($arrDataDetail[0]['BULLETIN_BOARD_NO'])
                                ? $arrDataDetail[0]['BULLETIN_BOARD_NO'] : '',
            'OCCURRENCE_DATE' => isset($arrDataDetail[0]['OCCURRENCE_DATE'])
                                ? fncHtmlSpecialChars(trim($arrDataDetail[0]['OCCURRENCE_DATE'])) : '',
            'CORRECTION_FLAG' => isset($arrDataDetail[0]['CORRECTION_FLAG'])
                                ? fncHtmlSpecialChars(trim($arrDataDetail[0]['CORRECTION_FLAG'])) : '',
            'BUSINESS_NAME' => isset($arrDataDetail[0]['BUSINESS_NAME'])
                                ? fncHtmlSpecialChars(trim($arrDataDetail[0]['BUSINESS_NAME'])) : '',
            'BUSINESS_NAME_ENG' => isset($arrDataDetail[0]['BUSINESS_NAME_ENG'])
                                ? fncHtmlSpecialChars(trim($arrDataDetail[0]['BUSINESS_NAME_ENG'])) : '',
            'INCIDENT_NAME_JPN' => isset($arrDataDetail[0]['INCIDENT_NAME_JPN'])
                                ? fncHtmlSpecialChars(trim($arrDataDetail[0]['INCIDENT_NAME_JPN'])) : '',
            'INCIDENT_NAME_ENG' => isset($arrDataDetail[0]['INCIDENT_NAME_ENG'])
                                ? fncHtmlSpecialChars(trim($arrDataDetail[0]['INCIDENT_NAME_ENG'])) : '',
            'PLACE_NAME_JPN' => isset($arrDataDetail[0]['PLACE_NAME_JPN'])
                                ? fncHtmlSpecialChars(trim($arrDataDetail[0]['PLACE_NAME_JPN'])) : '',
            'PLACE_NAME_ENG' => isset($arrDataDetail[0]['PLACE_NAME_ENG'])
                                ? fncHtmlSpecialChars(trim($arrDataDetail[0]['PLACE_NAME_ENG'])) : '',
        );
    }

    /**
     *	function: インシデント事案取得
     *
     *	@create	2020/03/24 Chien AKB
     *	@update
     *	@params	$intId      bulletin_board_no
     *	@return $arrResult  array data info of bulletin_board_no
     */
    function getInfoBullentin($intId) {
        $arrResult = array();
        if($intId == '') {
            return $arrResult;
        }
        $strSQL = ' SELECT '
                . '     tbb.BULLETIN_BOARD_NO '
                . '     , tbb.OCCURRENCE_DATE '
                . '     , tbb.CORRECTION_FLAG '
                . '     , tbb.BUSINESS_NAME '
                . '     , mbn.BUSINESS_NAME_ENG '
                . '     , tbb.INCIDENT_NAME_JPN '
                . '     , tbb.INCIDENT_NAME_ENG '
                . '     , mpn.PLACE_NAME_JPN '
                . '     , mpn.PLACE_NAME_ENG '
                . ' FROM '
                . '     t_bulletin_board AS tbb '
                . '     LEFT OUTER JOIN m_business_name AS mbn '
                . '         ON tbb.BUSINESS_NAME = mbn.BUSINESS_NAME_JPN '
                . '     LEFT OUTER JOIN m_place_name AS mpn '
                . '         ON tbb.PLACE3_ID = mpn.PLACE3_ID '
                . ' WHERE '
                . '     tbb.BULLETIN_BOARD_NO = ? ';
        try {
            // execute SQL and get data
            $arrResult = fncSelectData($strSQL, array($intId),
                                        1, false, DISPLAY_TITLE);
            return $arrResult;
        } catch (\Exceoption $e) {
            // write log
            fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                DISPLAY_TITLE.' '.$e->getMessage());
            return 0;
        }
    }

    // get string csrf token
    $strCSRF = isset($_POST['X-CSRF-TOKEN']) ? $_POST['X-CSRF-TOKEN'] : '';

    // 2020/03/30 AKB Chien - start - update document 2020/03/30
    // GET通信にて遷移してきた場合、以下のメッセージをアラート表示し、遷移元画面に戻す。
    fncGetRequestCheck($arrTxtTrans);
    // 2020/03/30 AKB Chien - end - update document 2020/03/30

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
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta name="csrf-token" content="<?php
        echo (isset($strCsrf) ? $strCsrf : '');
    ?>">
    <meta charset="UTF-8">
    <title><?php echo $arrTxtTrans['BULLETIN_BOARD_VIEW_TEXT_001']; ?></title>
    <link rel="stylesheet" type="text/css" href="css/style.css">
    <style>
        .style_real_display {
            white-space: pre-wrap;
            white-space: -moz-pre-wrap;
            white-space: -pre-wrap;
            white-space: -o-pre-wrap;
            word-wrap: break-word;
        }
    </style>
</head>
<body>
    <div class="main-content">
        <div class="main-form">
            <div class="form-title"><?php
                echo $arrTxtTrans['BULLETIN_BOARD_VIEW_TEXT_001'];
            ?></div>
            <div class="form-body">
                <div class="error-messeage">
                    <?php if($arrResult['CORRECTION_FLAG'] == 0) { ?>
                    <div><?php echo PUBLIC_TEXT_001_JPN; ?></div>
                    <div><?php echo PUBLIC_TEXT_001_ENG; ?></div>
                    <?php } ?>
                    <?php
                        // if get data has error
                        if($arrDataDetail == 0) {
                    ?>
                        <script>
                            alert('<?php echo $arrTxtTrans['PUBLIC_MSG_001']; ?>');
                            $('#myModal').modal('hide');
                            setTimeout(function() {
                                window.location.reload();
                            }, 300);
                        </script>
                    <?php
                        } else {
                            // if get non-data -> bulletin_board_no not exist or has been deleted
                            if(count($arrDataDetail) == 0) {
                    ?>
                        <script>
                            alert('<?php echo $arrTxtTrans['BULLETIN_BOARD_VIEW_MSG_001']; ?>');
                            $('#myModal').modal('hide');
                            setTimeout(function() {
                                window.location.reload();
                            }, 300);
                        </script>
                    <?php
                            }
                        }
                    ?>
                </div>
                <div class="cont-title">
                    <div class="col-right in-lineblock right-20">
                        <?php
                            // check condition to show button edit
                            if(@$objLoginUserInfo->intMenuPerm
                                && $objLoginUserInfo->intMenuPerm == 1
                                && $strScreen == 'bulletin_mng') {
                        ?>
                        <button type="button" class="tbtn tbtn-defaut load-modal" href="bulletin_board_edit.php"
                            data-id="<?php echo $arrResult['BULLETIN_BOARD_NO']; ?>"
                            data-screen="<?php echo $intIsFromPortalPage; ?>"
                            id="btn-edit"><?php
                            echo $arrTxtTrans['PUBLIC_BUTTON_002']; ?></button>
                        <?php } ?>
                    </div>
                    <div class="cont-title">
                        <div class="in-line"><?php
                            echo $arrTxtTrans['BULLETIN_BOARD_VIEW_TEXT_002']; ?>
                        </div>
                        <div class="in-line"><?php
                            echo date('Y/m/d H:i', strtotime($arrResult['OCCURRENCE_DATE']));
                        ?></div>
                    </div>
                </div>
                <!-- <br/> -->
                <div>
                    <p style="background-color:#4169e1">
                        <legend><?php
                            echo BULLETIN_BOARD_VIEW_TEXT_003_JPN; ?>/<?php
                            echo BULLETIN_BOARD_VIEW_TEXT_003_ENG;
                        ?></legend>
                    </p>
                    <div class="info-left">
                        <div class="line">
                            <div class="in-line tlabel">(<?php
                                echo PUBLIC_TEXT_002_JPN; ?>/<?php
                                echo PUBLIC_TEXT_002_ENG;
                            ?>)</div>
                            <div class="in-line text-input text-bold style_real_display"><?php
                                echo $arrResult['BUSINESS_NAME'];
                            ?></div>
                        </div>
                        <div class="line">
                            <div class="in-line tlabel">(<?php
                                echo PUBLIC_TEXT_003_JPN; ?>/<?php
                                echo PUBLIC_TEXT_003_ENG;
                            ?>)</div>
                            <div class="in-line text-input text-bold style_real_display"><?php
                                echo $arrResult['BUSINESS_NAME_ENG'];
                            ?></div>
                        </div>
                    </div>
                </div>

                <div>
                    <p style="background-color:#4169e1">
                        <legend><?php
                            echo BULLETIN_BOARD_VIEW_TEXT_004_JPN; ?>/<?php
                            echo BULLETIN_BOARD_VIEW_TEXT_004_ENG;
                        ?></legend>
                	</p>
                    <div class="info-left">
                        <div class="line">
                            <div class="in-line tlabel">(<?php
                                echo PUBLIC_TEXT_002_JPN; ?>/<?php
                                echo PUBLIC_TEXT_002_ENG;
                            ?>)</div>
                            <div class="in-line text-input text-bold style_real_display"><?php
                                echo $arrResult['INCIDENT_NAME_JPN'];
                            ?></div>
                        </div>
                        <div class="line">
                            <div class="in-line tlabel">(<?php
                                echo PUBLIC_TEXT_003_JPN; ?>/<?php
                                echo PUBLIC_TEXT_003_ENG;
                            ?>)</div>
                            <div class="in-line text-input text-bold style_real_display"><?php
                                echo $arrResult['INCIDENT_NAME_ENG'];
                            ?></div>
                        </div>
                    </div>
                </div>

                <div class="col-left">
                    <p style="background-color:#4169e1">
                        <legend><?php
                            echo BULLETIN_BOARD_VIEW_TEXT_005_JPN; ?>/<?php
                            echo BULLETIN_BOARD_VIEW_TEXT_005_ENG;
                        ?></legend>
                	</p>
                    <div class="info-left">
                        <div class="line">
                            <div class="in-line tlabel">(<?php
                                echo PUBLIC_TEXT_002_JPN; ?>/<?php
                                echo PUBLIC_TEXT_002_ENG;
                            ?>)</div>
                            <div class="in-line text-input text-bold style_real_display"><?php
                                echo $arrResult['PLACE_NAME_JPN'];
                            ?></div>
                        </div>
                        <div class="line">
                            <div class="in-line tlabel">(<?php
                                echo PUBLIC_TEXT_003_JPN; ?>/<?php
                                echo PUBLIC_TEXT_003_ENG;
                            ?>)</div>
                            <div class="in-line text-input text-bold style_real_display"><?php
                                echo $arrResult['PLACE_NAME_ENG'];
                            ?></div>
                        </div>
                    </div>
                </div>

                <div class="form-footer text-right right-20">
                    <button type="submit" class="tbtn-cancel tbtn-defaut closeModal"
                        id="close"
                        data-dismiss="modal"><?php
                        echo $arrTxtTrans['PUBLIC_BUTTON_003'];
                    ?></button>
                </div>
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function() {
            $('.closeModal').on('click', function(e) {
                if('<?php echo $strScreen ?>' == 'portal'){
                    $('#myModal').modal('hide');
                    clearHighlightWhenClose();
                    loadPortalClose();
                }else{
                    setTimeout(function() {
                        window.location.reload();
                    }, 300);
                }
            });
        });
    </script>
</body>
</html>