<?php
    /*
     * @announce_view.php
     *
     * @create 2020/03/13 AKB Chien
     * @update
     */
    require_once('common/common.php');
    require_once('common/session_check.php');

    header('Content-type: text/html; charset=utf-8');
    header('X-FRAME-OPTIONS: DENY');

    define('DISPLAY_TITLE', 'お知らせ表示画面');

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
        'ANNOUNCE_VIEW_MSG_001',
        'ANNOUNCE_VIEW_TEXT_001',
        'ANNOUNCE_VIEW_MSG_002',
        'ANNOUNCE_VIEW_TEXT_002',
        'PUBLIC_TEXT_001',
        'PUBLIC_TEXT_004',
        'PUBLIC_TEXT_002',
        'PUBLIC_TEXT_003',
        'PUBLIC_TEXT_005',
        'PUBLIC_BUTTON_002',
        'PUBLIC_BUTTON_003',
         // 2020/03/26 AKB Chien - start - update document 2020/03/26
        'PUBLIC_MSG_049',
         // 2020/03/26 AKB Chien - end - update document 2020/03/26
    );

    // write log when access this screen
    $strLog = DISPLAY_TITLE.'　表示(ユーザID = '.$objLoginUserInfo->strUserID.') ';
    $strLog .= isset($_SERVER['HTTP_REFERER']) ? $_SERVER['HTTP_REFERER'] : null;
    fncWriteLog(LogLevel['Info'], LogPattern['View'], $strLog);

    // get list text(header, title, msg) with languague_type of user logged
    $arrTxtTrans = getListTextTranslate($arrTitleMsg, $intLanguageType);

    $intIsFromPortalPage = (@$_POST['isAjax']) ? 1 : -1;

    // announce_no send to to this view
    $intId = @$_POST['id'] ? $_POST['id'] : '';

    //2020/04/14 T.Masuda 画面遷移元を調べる 1:ポータル　0:管理画面
    $intType = $_POST['type'];
    $strRefScreen = $_POST['screen'];
    //2020/04/14 T.Masuda

    // 以下のお知らせ情報が存在するか確認する。
    $arrDataDetail = getInfoAnnounce($intId, DISPLAY_TITLE);

    $arrResult = array(
        'ANNOUNCE_NO' => '',
        'REG_DATE' => '',
        'TITLE_ORIGINAL' => '',
        'TITLE_TRANSLATE' => '',
        'CONTENTS_ORIGINAL' => '',
        'CONTENTS_TRANSLATE' => '',
        'CORRECTION_FLAG' => '',
        'FILES' => array(
            '1' => '',
            '2' => '',
            '3' => '',
            '4' => '',
            '5' => ''
        )
    );
    // prepare data to show
    if($arrDataDetail != 0 && count($arrDataDetail) > 0) {
        $strTitleOriginal = '';
        $strTitleTranslate = '';
        $strContentsOriginal = '';
        $strContentsTranslate =  '';
        if($arrDataDetail[0]['LANGUAGE_TYPE'] == 0) {
            $strTitleOriginal = @$arrDataDetail[0]['TITLE_JPN']
                                    ? fncHtmlSpecialChars(trim($arrDataDetail[0]['TITLE_JPN'])) : '';
            $strTitleTranslate = @$arrDataDetail[0]['TITLE_ENG']
                                    ? fncHtmlSpecialChars(trim($arrDataDetail[0]['TITLE_ENG'])) : '';
            $strContentsOriginal = @$arrDataDetail[0]['CONTENTS_JPN']
                                    ? fncHtmlSpecialChars(trim($arrDataDetail[0]['CONTENTS_JPN'])) : '';
            $strContentsTranslate = @$arrDataDetail[0]['CONTENTS_ENG']
                                    ? fncHtmlSpecialChars(trim($arrDataDetail[0]['CONTENTS_ENG'])) : '';
        } else {
            $strTitleOriginal = @$arrDataDetail[0]['TITLE_ENG']
                                    ? fncHtmlSpecialChars(trim($arrDataDetail[0]['TITLE_ENG'])) : '';
            $strTitleTranslate = @$arrDataDetail[0]['TITLE_JPN']
                                    ? fncHtmlSpecialChars(trim($arrDataDetail[0]['TITLE_JPN'])) : '';
            $strContentsOriginal = @$arrDataDetail[0]['CONTENTS_ENG']
                                    ? fncHtmlSpecialChars(trim($arrDataDetail[0]['CONTENTS_ENG'])) : '';
            $strContentsTranslate = @$arrDataDetail[0]['CONTENTS_JPN']
                                    ? fncHtmlSpecialChars(trim($arrDataDetail[0]['CONTENTS_JPN'])) : '';
        }

        $arrResult = array(
            'ANNOUNCE_NO' => $arrDataDetail[0]['ANNOUNCE_NO'],
            'REG_DATE' => $arrDataDetail[0]['REG_DATE'],
            'TITLE_ORIGINAL' => $strTitleOriginal,
            'TITLE_TRANSLATE' => $strTitleTranslate,
            'CONTENTS_ORIGINAL' => $strContentsOriginal,
            'CONTENTS_TRANSLATE' => $strContentsTranslate,
            'CORRECTION_FLAG' => $arrDataDetail[0]['CORRECTION_FLAG'],
            //2020/4/14 T.Masuda データ種類を取得
            'DATA_TYPE' => $arrDataDetail[0]['DATA_TYPE'],
            //2020/4/14 T.Masuda
            'FILES' => array(
                '1' => @$arrDataDetail[0]['FILE_NAME1']
                        ? fncHtmlSpecialChars(trim($arrDataDetail[0]['FILE_NAME1'])) : '',
                '2' => @$arrDataDetail[0]['FILE_NAME2']
                        ? fncHtmlSpecialChars(trim($arrDataDetail[0]['FILE_NAME2'])) : '',
                '3' => @$arrDataDetail[0]['FILE_NAME3']
                        ? fncHtmlSpecialChars(trim($arrDataDetail[0]['FILE_NAME3'])) : '',
                '4' => @$arrDataDetail[0]['FILE_NAME4']
                        ? fncHtmlSpecialChars(trim($arrDataDetail[0]['FILE_NAME4'])) : '',
                '5' => @$arrDataDetail[0]['FILE_NAME5']
                        ? fncHtmlSpecialChars(trim($arrDataDetail[0]['FILE_NAME5'])) : ''
            )
        );
    }

    // get string csrf token
    $strCSRF = isset($_POST['X-CSRF-TOKEN']) ? $_POST['X-CSRF-TOKEN'] : '';

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
?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta name="csrf-token" content="<?php
        echo (isset($strCsrf) ? $strCsrf : ''); ?>">
    <meta charset="UTF-8">
    <title><?php echo $arrTxtTrans['ANNOUNCE_VIEW_TEXT_001']; ?></title>
    <link rel="stylesheet" type="text/css" href="css/style.css">
    <style>
        .style_real_display {
            white-space: pre-wrap;
            white-space: -moz-pre-wrap;
            white-space: -pre-wrap;
            white-space: -o-pre-wrap;
            word-break: break-all;
        }
    </style>
</head>
<body>
<div class="main-content">
    <div class="main-form">
        <div class="form-title"><?php
            echo $arrTxtTrans['ANNOUNCE_VIEW_TEXT_001'];
        ?></div>
        <div class="form-body">
            <div class="error-messeage">
                <?php
                    // check correction_flag of announce_no to show text
                    if($arrResult['CORRECTION_FLAG'] == 0) {
                ?>
                <div><?php echo PUBLIC_TEXT_001_JPN; ?></div>
                <div><?php echo PUBLIC_TEXT_001_ENG; ?></div>
                <?php } ?>
                <?php
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
                        if(count($arrDataDetail) == 0) {
                ?>
                    <script>
                        alert('<?php echo $arrTxtTrans['ANNOUNCE_VIEW_MSG_002']; ?>');
                        $('#myModal').modal('hide');
                        setTimeout(function() {
                            window.location.reload();
                        }, 300);
                    </script>
                <?php
                        }
                    }
                ?>
                <span class="error"></span>
            </div>
            <div class="col-right in-lineblock right-20">
                <?php
                    // check condition to show button edit
                    if(@$objLoginUserInfo->intAnnounceRegPerm
                        && $objLoginUserInfo->intAnnounceRegPerm == 1
                        && ($intType == 0
                            || ($intType == 1 && $arrResult['DATA_TYPE'] == '0'))) {
                ?>
                    <button type="button" class="tbtn tbtn-defaut load-modal"
                        href="announce_edit.php"
                        data-id="<?php echo $arrResult['ANNOUNCE_NO']; ?>"
                        data-screen="<?php echo fncHtmlSpecialChars($intType); ?>" id="btn-edit">
                        <?php echo $arrTxtTrans['PUBLIC_BUTTON_002']; ?>
                    </button>
                <?php } ?>
            </div>
            <div class="cont-title">
                <div class="in-line"><?php
                    echo $arrTxtTrans['ANNOUNCE_VIEW_TEXT_002'];
                ?></div>
                <div class="in-line "><?php
                    echo date_format(date_create($arrResult['REG_DATE']), 'Y/n/j H:i');
                ?></div>
            </div>
            <br/>
            <div>
                <p style="background-color:#4169e1">
                    <legend><?php
                        echo PUBLIC_TEXT_004_JPN ?>/<?php
                        echo PUBLIC_TEXT_004_ENG
                    ?></legend>
                </p>
                <div class="info-left">
                    <div class="line">
                        <div class="in-line tlabel">(<?php
                            echo PUBLIC_TEXT_002_JPN ?>/<?php
                            echo PUBLIC_TEXT_002_ENG
                        ?>)</div>
                        <div class="in-line text-input text-bold style_real_display"><?php
                            echo $arrResult['TITLE_ORIGINAL'];
                        ?></div>
                    </div>
                    <div class="line">
                        <div class="in-line tlabel">(<?php
                            echo PUBLIC_TEXT_003_JPN ?>/<?php echo PUBLIC_TEXT_003_ENG
                        ?>)</div>
                        <div class="in-line text-input text-bold style_real_display"><?php
                            echo $arrResult['TITLE_TRANSLATE'];
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
                        echo PUBLIC_TEXT_002_JPN ?>/<?php
                        echo PUBLIC_TEXT_002_ENG
                    ?>)</div>
                    <div class="text-input text-bold style_real_display"><?php
                        echo $arrResult['CONTENTS_ORIGINAL'];
                    ?></div>
                </div>
            </div>

            <div class="info">
                <div class="line">
                    <div>(<?php
                        echo PUBLIC_TEXT_003_JPN ?>/<?php
                        echo PUBLIC_TEXT_003_ENG
                    ?>)</div>
                    <div class="text-input text-bold style_real_display"><?php
                        echo $arrResult['CONTENTS_TRANSLATE'];
                    ?></div>
                </div>
            </div>

            <p style="background-color:#4169e1">
                <legend><?php
                    echo ANNOUNCE_VIEW_TEXT_003_JPN ?>/<?php
                    echo ANNOUNCE_VIEW_TEXT_003_ENG
                ?></legend>
			</p>
            <div class="link info">
                <?php
                    foreach($arrResult['FILES'] as $folder => $file) {
                        if($file != '') {
                ?>
                    <div style="word-break: break-all;">
                        <a href="javscript:void(0)" class="download-file"
                            data-id="<?php
                                echo base64_encode($_POST['id'].'/'.$folder.'/'.$file);
                        ?>"><?php
                            echo $file;
                        ?></a>
                    </div>
                <?php
                        }
                    }
                ?>
            </div>
            <div class="form-footer text-right right-20">
                <button type="submit" class="tbtn-cancel tbtn-defaut closeModal"
                    id="close" data-dismiss="modal"><?php
                    echo $arrTxtTrans['PUBLIC_BUTTON_003'];
                ?></button>
            </div>

            <form action="announce_view_proc.php" method="post" id="formDownload">
                <input type="hidden" name="X-CSRF-TOKEN" value="<?php
                    echo $strCSRF; ?>" />
                <input type="hidden" name="path" value="">
                <input type="hidden" name="mode" value="99">
            </form>
        </div>
    </div>
</div>
<script>
    $(document).ready(function() {
        $('.download-file').off().on('click', function(e) {
            e.preventDefault();
            $('.error').html('');
            var arr = [
                { name: 'X-CSRF-TOKEN', value: '<?php echo $strCSRF; ?>'},
                { name: 'file', value: $(this).attr('data-id') },
                { name: 'action', value: 'checkFile' }
            ];
            var blnCanDownload = false;
            $.ajax({
                url: 'announce_view_proc.php',
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
                            echo $arrTxtTrans['ANNOUNCE_VIEW_MSG_001'];
                        ?></div>');
                    } else {
                        blnCanDownload = true;
                    }
                },
                error: function(e) {
                    console.log(e);
                }
            });
            if(blnCanDownload) {
                $('input[name=path]').val($(this).attr('data-id'));
                $('form#formDownload').submit();
            }
            return;
        });

        $('.closeModal').on('click', function(e) {
            <?php if( @$intType == 1 || @$strRefScreen == 'portal'){ ?>
                $('#myModal').modal('hide');
                loadPortalClose();
            <?php }else{ ?>
                setTimeout(function() {
                    window.location.reload();
                }, 300);
            <?php } ?>
        });
    });
</script>
</body>
</html>