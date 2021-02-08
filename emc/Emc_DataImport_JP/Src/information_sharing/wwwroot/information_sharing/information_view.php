<?php
    /*
     * @information_view.php
     *
     * @create 2020/04/09 AKB Chien
     * @update
     */
    require_once('common/common.php');
    header('Content-type: text/html; charset=utf-8');
    header('X-FRAME-OPTIONS: DENY');

    //DB接続
    if(fncConnectDB() == false) {
        $_SESSION['LOGIN_ERROR'] = 'DB接続に失敗しました。';
        header('Location: login.php');
        exit;
    }

    // Check if the user logged in or not
    if(!isset($_SESSION['LOGINUSER_INFO'])) {
        echo "
        <script>
            alert('".PUBLIC_MSG_008_JPN . "/" . PUBLIC_MSG_008_ENG."');
            window.location.href = 'login.php';
        </script>
        ";

        exit();
    }

    // check timeout if direct access this file
    fncSessionTimeOutCheck();

    $_SESSION['errStr'] = '';

    define('SCREEN_NAME', '情報表示画面');
    define('SCREEN_NAME_VIEW', '情報表示画面　表示');

    $objUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);

    // get list text translate
    $arrTitle =  array(
        'INFORMATION_VIEW_TEXT_001',
        'PUBLIC_TEXT_017',
        'INFORMATION_VIEW_TEXT_002',
        'INFORMATION_VIEW_TEXT_003',
        'INFORMATION_VIEW_TEXT_004',
        'INFORMATION_VIEW_TEXT_005',
        'INFORMATION_VIEW_TEXT_006',
        'INFORMATION_VIEW_TEXT_007',
        'INFORMATION_VIEW_MSG_001',
        'INFORMATION_VIEW_MSG_002',
        'INFORMATION_VIEW_MSG_003',
        'PUBLIC_BUTTON_002',
        'PUBLIC_BUTTON_010',
        'PUBLIC_BUTTON_003',

        'PUBLIC_MSG_001',
        'PUBLIC_MSG_004',
        'PUBLIC_MSG_049',
        'PUBLIC_MSG_016',
        'PUBLIC_MSG_009'
    );

    // get list text(header, title, msg) with languague_type of user logged
    $arrTextTranslate = getListTextTranslate(
        $arrTitle,
        $objUserInfo->intLanguageType
    );

    // 2020/04/15 AKB Chien - start - update document 2020/04/15
    // GET通信にて遷移してきた場合、以下のメッセージをアラート表示し、遷移元画面に戻す。
    fncGetRequestCheck($arrTextTranslate);
    // 2020/04/15 AKB Chien - end - update document 2020/04/15

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

    //▼2020/05/27 KBS T.Masuda Jcmgタブ権限が無いユーザはログイン画面に遷移
    if($objUserInfo->intJcmgTabPerm != 1){
        echo '<script>alert("'.$arrTextTranslate['PUBLIC_MSG_009'].'");
                      window.location.href="login.php";</script>';
        exit;
    }
    //▲2020/05/27 KBS T.Masuda

    // check if ajax -> do something | access this file directly -> stop
    if(!(!empty($_SERVER['HTTP_X_REQUESTED_WITH'])
        && strtolower($_SERVER['HTTP_X_REQUESTED_WITH']) == 'xmlhttprequest')) {
        exit;
    }

    // write log when access this screen
    fncWriteLog(
        LogLevel['Info'],
        LogPattern['View'] ,
        SCREEN_NAME_VIEW . ' (ユーザID = '.$objUserInfo->strUserID.') '
        .(isset($_SERVER['HTTP_REFERER']) ? $_SERVER['HTTP_REFERER'] : null)
    );

    $arrOneRequest = array();
    // if $_POST has id
    if(isset($_POST['id'])) {
        //2020/04/22 T.Masuda 会社名表示を日本語/英語に変更
        //$strSuffixes = ($objUserInfo->intLanguageType == 0) ? '_JPN' : '_ENG';
        //$strCompanyNameSelect = 'company_name'.$strSuffixes;
        //2020/04/22 T.Masuda

        // get data of information_no (check exist)
        $arrOneRequest = fncSelectOne(
            " SELECT
                ti.*".
                //, mc.".$strCompanyNameSelect." AS COMPANY_NAME
               ", mc.COMPANY_NAME_JPN
                , mc.COMPANY_NAME_ENG
                , mic.INST_CATEGORY_NAME_JPN
                , mic.INST_CATEGORY_NAME_ENG
                , mifc.INFO_CATEGORY_NAME_JPN
                , mifc.INFO_CATEGORY_NAME_ENG
            FROM t_information AS ti
            LEFT OUTER JOIN m_company AS mc
                ON ti.COMPANY_NO = mc.COMPANY_NO
            LEFT OUTER JOIN m_inst_category AS mic
                ON mc.INST_CATEGORY_NO = mic.INST_CATEGORY_NO
            LEFT OUTER JOIN m_info_category AS mifc
                ON ti.INFO_CATEGORY_NO = mifc.INFO_CATEGORY_NO
            WHERE ti.information_no = ? ",
            [$_POST['id']],
            SCREEN_NAME
        );

        // if has error
        if(!is_array($arrOneRequest) || count($arrOneRequest) == 0) {
            //log error
            fncWriteLog(
                LogLevel['Error'],
                LogPattern['Error'],
                SCREEN_NAME . ' ' . $arrTextTranslate['INFORMATION_VIEW_MSG_002']
            );
            //request not exist
?>
<script>
    alert('<?php echo $arrTextTranslate['INFORMATION_VIEW_MSG_002']; ?>');
    $('#myModal').modal('hide');
    setTimeout(function() {
        window.location.reload();
    }, 300);
</script>
<?php
        exit();
    }

    // get info of user with information_no
    $arrUserLogin = fncSelectOne(
        " SELECT ti.COMPANY_NO, mg.ADMIN_FLAG FROM t_information AS ti
            INNER JOIN m_user AS mu ON ti.REG_USER_NO = mu.USER_NO
            INNER JOIN m_company AS mc ON mu.COMPANY_NO = mc.COMPANY_NO
            INNER JOIN m_group AS mg ON mc.GROUP_NO = mg.GROUP_NO
        WHERE ti.INFORMATION_NO = ? ",
        [$arrOneRequest['INFORMATION_NO']],
        SCREEN_NAME
    );

    // if has error
    if(!is_array($arrUserLogin) || count($arrUserLogin) == 0) {
?>
<script>
    alert('<?php echo $arrTextTranslate['INFORMATION_VIEW_MSG_002']; ?>');
    $('#myModal').modal('hide');
    setTimeout(function() {
        window.location.reload();
    }, 300);
</script>
<?php
        exit();
    }

    // check condition to show btn Edit
    $blnEditInformation = false;
    // 2020/04/20 AKB Chien - start - update document 2020/04/20
    // if($objUserInfo->intInformationRegPerm == 1 &&
    //     $objUserInfo->intCompanyNo == $arrUserLogin['COMPANY_NO'] &&
    //     $arrUserLogin['ADMIN_FLAG'] == 1) {
    //     $blnEditInformation = true;
    // }
    if(($objUserInfo->intInformationRegPerm == 1 &&
        $objUserInfo->intCompanyNo == $arrUserLogin['COMPANY_NO'])
        || $objUserInfo->intMenuPerm == 1) {
        $blnEditInformation = true;
    }
    // 2020/04/20 AKB Chien - end - update document 2020/04/20
?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta name="csrf-token" content="<?php
        echo (isset($_POST['X-CSRF-TOKEN']) ? $_POST['X-CSRF-TOKEN'] : '');
    ?>">
    <meta charset="UTF-8">
    <meta name="format-detection" content="telephone=no">
    <title><?php
        echo $arrTextTranslate['INFORMATION_VIEW_TEXT_001'];
    ?></title>
    <link rel="stylesheet" type="text/css" href="css/style.css">
    <style>
        .style_real_display {
            white-space: pre-wrap;
            white-space: -moz-pre-wrap;
            white-space: -pre-wrap;
            white-space: -o-pre-wrap;
            word-wrap: break-word;
        }
        legend{
            color: white;
        }
        
        <?php
        	if($objUserInfo->intLanguageType == 1) {
        		echo '.info-com{ width: 130px !important;}';
        	}
        ?>
    </style>
</head>
<body>
    <div class="main-content">
        <div class="main-form">
            <div class="form-title"><?php
                echo $arrTextTranslate['INFORMATION_VIEW_TEXT_001'];
            ?></div>
            <div class="view-error" style="color:red;"></div>
            <div class="form-body">
            <?php if($arrOneRequest['CORRECTION_FLAG'] == 0) { ?>
                <div class="error-messeage">
                    <div><?php echo PUBLIC_TEXT_001_JPN; ?></div>
                    <div><?php echo PUBLIC_TEXT_001_ENG; ?></div>
                </div>
            <?php } ?>
                <div class="cont-title">
                    <p style="background-color:#4169e1">
                        <legend><?php
                            echo PUBLIC_TEXT_004_JPN; ?>/<?php
                            echo PUBLIC_TEXT_004_ENG;
                        ?></legend>
                    </p>

                    <div class="info-left">
                        <div class="line">
                            <div class="in-line tlabel style_real_display">(<?php
                                echo PUBLIC_TEXT_002_JPN; ?>/<?php
                                echo PUBLIC_TEXT_002_ENG;
                            ?>)</div>
                            <div class="in-line text-input text-bold"><?php
                                if($arrOneRequest['LANGUAGE_TYPE'] == 0) {
                                    echo fncHtmlSpecialChars($arrOneRequest['TITLE_JPN']);
                                } else {
                                    echo fncHtmlSpecialChars($arrOneRequest['TITLE_ENG']);
                                }
                            ?></div>
                        </div>
                        <div class="line">
                            <div class="in-line tlabel style_real_display">(<?php
                                echo PUBLIC_TEXT_003_JPN; ?>/<?php
                                echo PUBLIC_TEXT_003_ENG;
                            ?>)</div>
                            <div class="in-line text-input text-bold"><?php
                                if($arrOneRequest['LANGUAGE_TYPE'] == 0) {
                                    echo fncHtmlSpecialChars($arrOneRequest['TITLE_ENG']);
                                } else {
                                    echo fncHtmlSpecialChars($arrOneRequest['TITLE_JPN']);
                                }
                            ?></div>
                        </div>
                    </div>
                </div>

				<p style="background-color:#4169e1">
                    <legend><?php
                        echo PUBLIC_TEXT_005_JPN; ?>/<?php
                        echo PUBLIC_TEXT_005_ENG;
                    ?></legend>
                </p>

                <div class="info">
                    <div class="line">
                        <div>(<?php
                            echo PUBLIC_TEXT_002_JPN; ?>/<?php
                            echo PUBLIC_TEXT_002_ENG;
                        ?>)</div>
                        <div class="text-input text-bold style_real_display"><?php
                            if($arrOneRequest['LANGUAGE_TYPE'] == 0) {
                                echo fncHtmlSpecialChars($arrOneRequest['CONTENTS_JPN']);
                            } else {
                                echo fncHtmlSpecialChars($arrOneRequest['CONTENTS_ENG']);
                            }
                        ?></div>
                    </div>
                </div>

                <div class="info">
                    <div class="line">
                        <div>(<?php
                            echo PUBLIC_TEXT_003_JPN; ?>/<?php
                            echo PUBLIC_TEXT_003_ENG;
                        ?>)</div>
                        <div class="text-input text-bold style_real_display"><?php
                            if($arrOneRequest['LANGUAGE_TYPE'] == 0) {
                                echo fncHtmlSpecialChars($arrOneRequest['CONTENTS_ENG']);
                            } else {
                                echo fncHtmlSpecialChars($arrOneRequest['CONTENTS_JPN']);
                            }
                        ?></div>
                    </div>
                </div>
                <p style="background-color:#4169e1">
                    <legend><?php
                        echo INFORMATION_VIEW_TEXT_008_JPN; ?>/<?php
                        echo INFORMATION_VIEW_TEXT_008_ENG;
                    ?></legend>
                </p>
                <div class="link info">
                <?php for($intLoop = 1; $intLoop <= 5; $intLoop++) { ?>
                    <?php if(isset($arrOneRequest['TMP_FILE_NAME'.$intLoop])
                    && trim($arrOneRequest['TMP_FILE_NAME'.$intLoop]) != '') {
                        $strPathTmp = SHARE_FOLDER . '/' . INFORMATION_ATTACHMENT_FOLDER . '/'
                        . $arrOneRequest['INFORMATION_NO'] . '/'.$intLoop.'/'
                        . $arrOneRequest['TMP_FILE_NAME'.$intLoop];
                    ?>
                    <div style="word-wrap: break-word;">
                        <a <?php echo 'href="#"'
                        . (!file_exists(SHARE_FOLDER . '/' . INFORMATION_ATTACHMENT_FOLDER
                        . '/' . $arrOneRequest['INFORMATION_NO'] . '/'.$intLoop.'/'
                        . $arrOneRequest['TMP_FILE_NAME'.$intLoop])
                        ? ' class="no-file"'
                        : '  data-id="'.base64_encode($strPathTmp).'" class="download-file"')
                        ?> ><?php
                            echo $arrOneRequest['TMP_FILE_NAME'.$intLoop];
                        ?></a><?php
                            if(isset($arrOneRequest['PUBIC_FLAG'.$intLoop])) {
                                if(trim($arrOneRequest['PUBIC_FLAG'.$intLoop]) == 1) {
                        ?>
                        &nbsp;&nbsp;<font size="2" color="red"><?php
                            echo $arrTextTranslate['PUBLIC_TEXT_017'];
                        ?></font>
                        <?php
                                }
                            }
                        ?>
                    </div>
                <?php } } ?>
                </div>
                <div class="text-link info">
                    <div><?php
                        echo $arrTextTranslate['INFORMATION_VIEW_TEXT_002'].'　'.
                        date_format(date_create($arrOneRequest['CONFIRM_DATE']), 'Y/n/j H:i').'（'.
                        $arrTextTranslate['INFORMATION_VIEW_TEXT_003'].'　'.
                        date_format(date_create($arrOneRequest['REG_DATE']), 'Y/n/j H:i').'）';
                    ?></div>
                    <div><p x-ms-format-detection="none"><?php
                        echo $arrTextTranslate['INFORMATION_VIEW_TEXT_004'].'　'.
                        fncHtmlSpecialChars($arrOneRequest['CONTACT_INFO']);
                    ?></p></div>
                </div>
                <div class="text-link info">
                    <div class="line">
                        <div class="info-com tlabel"><?php
                            echo $arrTextTranslate['INFORMATION_VIEW_TEXT_005'];
                        ?></div><div class="in-line text-input text-bold style_real_display"><?php 
                        	echo fncHtmlSpecialChars($arrOneRequest['COMPANY_NAME_JPN']).'<br>'.
                            	 '<div class="info-com tlabel"></div>'.
                                 fncHtmlSpecialChars($arrOneRequest['COMPANY_NAME_ENG']) ;
                        ?></div>
                    </div>
                    <div class="line">
                        <div class="in-line tlabel"><?php
                            echo $arrTextTranslate['INFORMATION_VIEW_TEXT_006'];
                        ?></div>
                        <div class="in-line text-input text-bold style_real_display"><?php
                            echo fncHtmlSpecialChars($arrOneRequest['INST_CATEGORY_NAME_JPN']).'/'.
                            fncHtmlSpecialChars($arrOneRequest['INST_CATEGORY_NAME_ENG']);
                        ?></div>
                    </div>
                    <div class="line">
                        <div class="in-line tlabel"><?php
                            echo $arrTextTranslate['INFORMATION_VIEW_TEXT_007'];
                        ?></div>
                        <div class="in-line text-input text-bold style_real_display"><?php
                            echo fncHtmlSpecialChars($arrOneRequest['INFO_CATEGORY_NAME_JPN']).'/'.
                            fncHtmlSpecialChars($arrOneRequest['INFO_CATEGORY_NAME_ENG']);
                        ?></div>
                    </div>
                </div>

                <div class="form-footer text-right right-20">
                <?php if($blnEditInformation) { ?>
                    <button type="button" class="tbtn tbtn-defaut load-modal" id="btnEdit"
                        href="information_edit.php" data-id="<?php echo $_POST['id']; ?>"><?php
                        echo $arrTextTranslate['PUBLIC_BUTTON_002']; ?></button>
                    <button type="button" class="tbtn tbtn-defaut" id="btnDelete"
                        data-id="<?php echo $_POST['id']; ?>"><?php
                        echo $arrTextTranslate['PUBLIC_BUTTON_010'];
                    ?></button>
                <?php } ?>
                    <button type="button" class="tbtn tbtn-defaut"
                        id="btnClose" data-dismiss="modal"><?php
                        echo $arrTextTranslate['PUBLIC_BUTTON_003'];
                    ?></button>
                </div>
            </div>
        </div>
    </div>
    <form action="information_view_proc.php" method="post" id="formDownload">
        <input type="hidden" name="X-CSRF-TOKEN" value="<?php
            if(isset($_SESSION['csrf'])) echo $_SESSION['csrf'];
        ?>" />
        <input type="hidden" name="path" value="">
        <input type="hidden" name="mode" value="99">
    </form>
    <script>
        var fileNotExistMessage = '<?php
        echo $arrTextTranslate['INFORMATION_VIEW_MSG_001'] ?>';

        $('.no-file').click(function(e) {
            e.preventDefault();
            $('.view-error').html(fileNotExistMessage);
        });

        $('.download-file').on('click', function(e) {
            e.preventDefault();
            $('.view-error').html('');
            var path = $(this).attr('data-id');
            var arr = [
                { name: 'X-CSRF-TOKEN',
                    value: '<?php if(isset($_SESSION['csrf'])) echo $_SESSION['csrf']; ?>'},
                { name: 'file', value: $(this).attr('data-id') },
                { name: 'action', value: 'checkFile' }
            ];
            var blnCanDownload = false;
            $.ajax({
                url: 'information_view_proc.php',
                type: 'POST',
                async: false,
                data: arr,
                success: function(result) {
                    if($.trim(result) == 'window.location.href="login.php";') {
                        window.location.href="login.php";
                        return;
                    }
                    if(result == 0) {
                        $('span.error').html('<div><?php
                            echo $arrTextTranslate['PUBLIC_MSG_016'];
                        ?></div>');
                    } else {
                        blnCanDownload = true;
                    }
                    if(blnCanDownload) {
                        $('input[name=path]').val(path);

                        $('form#formDownload').submit();
                    }
                },
                error: function(e) {
                    console.log(e);
                }
            });
            return;
        });

        $('#btnClose').on('click', function(e) {
            loadPortalClose();
            $('#myModal').modal('hide');
        });

        $('#btnDelete').off().on('click', function(e) {
            e.preventDefault();
            if(confirm('<?php
                echo $arrTextTranslate['INFORMATION_VIEW_MSG_003']; ?>')) {
                $('.error').html('');
                var arr = [
                    { name: 'X-CSRF-TOKEN',
                        value: '<?php if(isset($_SESSION['csrf'])) echo $_SESSION['csrf']; ?>'},
                    { name: 'id', value: $(this).attr('data-id') },
                    { name: 'mode', value: 1 },
                    { name: 'COMPANY_NO', value: <?php echo $arrUserLogin['COMPANY_NO'] ?> }
                ];

                $.ajax({
                    url: 'information_view_proc.php',
                    type: 'POST',
                    async: false,
                    data: arr,
                    success: function(result) {
                        if($.trim(result) == 'window.location.href="login.php";') {
                            window.location.href="login.php";
                            return;
                        }

                        if(result == 900){
    						alert('<?php echo $arrTextTranslate['PUBLIC_MSG_009'] ?>');
    						window.location.href="login.php";
    	                    return;
    					}

                        if($.trim(result) == 0) {
                            $('.view-error').html('<?php echo $arrTextTranslate['PUBLIC_MSG_004']; ?>');
                            return;
                        } else {
                            if($.trim(result) == 1) {
                            	loadPortalClose();
                                $('#myModal').modal('hide');
                            }
                        }
                    },
                    error: function(e) {
                        console.log(e);
                    }
                });
                return;
            }
        });
    </script>
</body>
</html>
<?php
    } else {
        echo "
        <script>
            alert('".PUBLIC_MSG_008_JPN."/".PUBLIC_MSG_008_ENG."');
            window.location.href = 'login.php';
        </script>
        ";
        exit();
    }
?>