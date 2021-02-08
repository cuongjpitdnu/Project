<?php
    /*
     * @announce_edit.php
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
    fncSessionTimeOutCheck();

    // ログインユーザ情報を取得
    $objLoginUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);

    $intLanguageType = $objLoginUserInfo->intLanguageType;

    // <初期画面表示時>
    $strLog = DISPLAY_TITLE.'　表示(ユーザID = '.$objLoginUserInfo->strUserID.') ';
    $strLog .= isset($_SERVER['HTTP_REFERER']) ? $_SERVER['HTTP_REFERER'] : null;
    fncWriteLog(LogLevel['Info'], LogPattern['View'], $strLog);

    $arrTitleMsg =  array(
        'ANNOUNCE_EDIT_TEXT_001',
        'ANNOUNCE_EDIT_MSG_002',
        'PUBLIC_TEXT_002',
        'PUBLIC_TEXT_004',
        'PUBLIC_TEXT_005',
        'PUBLIC_TEXT_008',
        'PUBLIC_BUTTON_004',
        'PUBLIC_TEXT_003',
        'ANNOUNCE_EDIT_TEXT_002',
        'ANNOUNCE_EDIT_MSG_003',
        'PUBLIC_TEXT_010',
        'PUBLIC_TEXT_009',
        'PUBLIC_BUTTON_005',
        'PUBLIC_BUTTON_003',
        'PUBLIC_MSG_020',

        'PUBLIC_MSG_010',

        // msg check file
        'PUBLIC_MSG_011',
        'PUBLIC_MSG_012',
        'PUBLIC_MSG_013',
        'PUBLIC_MSG_014',
        'PUBLIC_MSG_015',
        'PUBLIC_MSG_016',

        // 2020/03/26 AKB Chien - start - update document 2020/03/26
        'PUBLIC_BUTTON_007',
        'ANNOUNCE_EDIT_MSG_017',
        'PUBLIC_MSG_003',
        'PUBLIC_MSG_009',
        'PUBLIC_MSG_049',
        // 2020/03/26 AKB Chien - end - update document 2020/03/26

        'ANNOUNCE_EDIT_MSG_001',
        'PUBLIC_MSG_050',

        //2020/04/24　T.Mausda　ファイル容量チェックをJavascriptで行う
        'PUBLIC_MSG_019',
        //2020/04/24　T.Mausda
    );

    // get list text(header, title, msg) with languague_type of user logged
    $arrTxtTrans = getListTextTranslate($arrTitleMsg, $intLanguageType);

    $intId = @$_POST['id'] ? $_POST['id'] : '';

    //2020/04/24　T.Mausda　ファイル容量チェックをJavascriptで行う
    define('TOTAL_FILE_SIZE', 10485760); // 10 * 1024 * 1024
    //2020/04/24　T.Mausda

    // get info of announce_no
    $arrDataDetail = getInfoAnnounce($intId, DISPLAY_TITLE);

    $arrResult = array(
        'ANNOUNCE_NO' => '',
        'COMP_DATE' => '',
        'TITLE_ORIGINAL' => '',
        'TITLE_TRANSLATE' => '',
        'CONTENTS_ORIGINAL' => '',
        'CONTENTS_TRANSLATE' => '',
        'LANGUAGE_TYPE' => '',
        'CORRECTION_FLAG' => '',
        'UNTRANSLATED' => '',
        //2020/4/15 データ種類を取得
        'DATA_TYPE' => '',
        //2020/4/15
        'FILES' => array(
            '1' => '',
            '2' => '',
            '3' => '',
            '4' => '',
            '5' => ''
        )
    );

    $intRefScreen = $_POST['screen'] == 'portal' ? 1 : 0;

    // has data -> prepare data to show
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
            'COMP_DATE' => @$arrDataDetail[0]['COMP_DATE']
                            ? fncHtmlSpecialChars(trim($arrDataDetail[0]['COMP_DATE'])) : '',
            'TITLE_ORIGINAL' => $strTitleOriginal,
            'TITLE_TRANSLATE' => $strTitleTranslate,
            'CONTENTS_ORIGINAL' => $strContentsOriginal,
            'CONTENTS_TRANSLATE' => $strContentsTranslate,
            'LANGUAGE_TYPE' => $arrDataDetail[0]['LANGUAGE_TYPE'],
            'CORRECTION_FLAG' => $arrDataDetail[0]['CORRECTION_FLAG'],
            'UNTRANSLATED' => $arrDataDetail[0]['UNTRANSLATED'],
            //2020/4/15 データ種類を取得
            'DATA_TYPE' => $arrDataDetail[0]['DATA_TYPE'],
            //2020/4/15
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
    <title><?php echo $arrTxtTrans['ANNOUNCE_EDIT_TEXT_001']; ?></title>
    <link rel="stylesheet" type="text/css" href="css/style.css">
    <link rel="stylesheet" type="text/css" href="css/table-upload.css">
    <?php
        // check permission when access page
        if($objLoginUserInfo->intAnnounceRegPerm != 1) {
    ?>
        <script>
            alert('<?php echo $arrTxtTrans['PUBLIC_MSG_009']; ?>');
            window.location.href="login.php";
        </script>
    <?php die(); } ?>
    <?php
        // check if announce_no has been delete
        if($arrDataDetail != 0 && count($arrDataDetail) == 0 && $intId != '') {
    ?>
        <script>
            alert('<?php echo $arrTxtTrans['ANNOUNCE_EDIT_MSG_001']; ?>');
            $('#myModal').modal('hide');
            setTimeout(function() {
                window.location.reload();
            }, 300);
        </script>
    <?php exit; } ?>
    <style>
        .style_real_display {
            white-space: pre-wrap;
            white-space: -moz-pre-wrap;
            white-space: -pre-wrap;
            white-space: -o-pre-wrap;
            word-wrap: break-word;
        }
        .fl-w-700 {
            float: left;
            width: 700px;
        }
        fieldset {
            padding: 10px !important;
            border-style: groove !important;
            border-color: threedface !important;
            border-image: initial !important;
        }
    </style>
</head>
<body>
    <div class="main-content">
        <div class="main-form">
            <div class="form-title"><?php
                echo $arrTxtTrans['ANNOUNCE_EDIT_TEXT_001'];
            ?></div>
            <div class="form-body">
                <div><font color="red" class="error-edit"></font></div>
                <form action="" method="POST" name="announce-edit"  autocomplete="off">
                    <div class="text-right right-20">
                        <div class="select-container">
                            <select name="cmbTranslation">
                                <option value="ja"
                                    <?php
                                        if($arrResult['LANGUAGE_TYPE'] == 0
                                            || ($arrResult['ANNOUNCE_NO'] == '' && $intLanguageType == 0)) {
                                                echo 'selected';
                                        }
                                    ?>>
                                    <?php echo fncHtmlSpecialChars(PUBLIC_TEXT_010_JPN); ?>
                                </option>
                                <option value="en"
                                    <?php
                                        if($arrResult['LANGUAGE_TYPE'] == 1
                                            || ($arrResult['ANNOUNCE_NO'] == '' && $intLanguageType == 1)) {
                                                echo 'selected';
                                        }
                                    ?>>
                                    <?php echo fncHtmlSpecialChars(PUBLIC_TEXT_010_ENG); ?>
                                </option>
                            </select>
                        </div>
                    </div>
                    <br>
                    <p style="background-color:#4169e1">
                        <legend><?php echo $arrTxtTrans['PUBLIC_TEXT_002']; ?></legend>
                    </p>
                    <fieldset class="fi-yellow">
                        <div><?php
                            $strHtml = fncHtmlSpecialChars(PUBLIC_TEXT_004_JPN).'/';
                            $strHtml .= fncHtmlSpecialChars(PUBLIC_TEXT_004_ENG);
                            echo $strHtml;
                        ?></div>
                        <input type="text" name="txtTitleOriginal" class="form-control"
                            value="<?php echo $arrResult['TITLE_ORIGINAL']; ?>"
                            <?php
                                if($arrResult['DATA_TYPE'] == 1) {
                                    echo 'disabled';
                                }
                            ?>
                        />
                        <div><?php
                            $strHtml = fncHtmlSpecialChars(PUBLIC_TEXT_005_JPN).'/';
                            $strHtml .= fncHtmlSpecialChars(PUBLIC_TEXT_005_ENG);
                            echo $strHtml;
                        ?></div>
                        <textarea name="txtContentOriginal" class="form-control"
                            <?php if($arrResult['DATA_TYPE'] == 1) { echo 'disabled'; } ?>
                            rows="2" cols="50"><?php echo $arrResult['CONTENTS_ORIGINAL']; ?></textarea>
                    </fieldset>

                    <div class="clearfix mar-tl-10">
                        <div class="in-line">
                            <label for="chkManualInput">
                                <input type="checkbox" id="chkManualInput" name="chkManualInput"
                                    <?php
                                        if($arrResult['CORRECTION_FLAG'] == 1) {
                                            echo 'checked';
                                        }
                                    ?>
                                /><?php echo $arrTxtTrans['PUBLIC_TEXT_008']; ?>
                            </label>
                        </div>
                        <div class="in-line col-right">
                            <button class="tbtn tbtn-defaut" name="btnTranslation"
                            <?php
                                if($arrResult['CORRECTION_FLAG'] == 1) {
                                    echo 'disabled';
                                }
                            ?>><?php
                                echo $arrTxtTrans['PUBLIC_BUTTON_004'];
                            ?></button>
                        </div>
                    </div>

                    <p style="background-color:#4169e1">
                        <legend><?php
                            echo $arrTxtTrans['PUBLIC_TEXT_003'];
                        ?></legend>
                    </p>
                    <fieldset class="fi-pink">
                        <div><?php
                                $strHtml = fncHtmlSpecialChars(PUBLIC_TEXT_004_JPN).'/';
                                $strHtml .= fncHtmlSpecialChars(PUBLIC_TEXT_004_ENG);
                                echo $strHtml;
                        ?></div>
                        <input type="text" name="txtTitleTranslation" class="form-control"
                            <?php
                                if($arrResult['CORRECTION_FLAG'] == 0) {
                                    echo 'disabled';
                                }
                            ?> value="<?php echo $arrResult['TITLE_TRANSLATE']; ?>" />
                        <div><?php
                                $strHtml = fncHtmlSpecialChars(PUBLIC_TEXT_005_JPN).'/';
                                $strHtml .= fncHtmlSpecialChars(PUBLIC_TEXT_005_ENG);
                                echo $strHtml;
                        ?></div>
                        <textarea name="txtContentTranslation" class="form-control" rows="2" cols="50"
                            <?php if($arrResult['CORRECTION_FLAG'] == 0) { echo 'disabled'; } ?>
                        ><?php echo $arrResult['CONTENTS_TRANSLATE']; ?></textarea>
                    </fieldset>

                    <br>
                    <p style="background-color:#4169e1">
                        <legend><?php echo $arrTxtTrans['ANNOUNCE_EDIT_TEXT_002']; ?></legend>
                    </p>
                    <table class="blueTable-modal">
                        <?php
                            foreach($arrResult['FILES'] as $folder => $file) {
                                if($file != '') {
                        ?>
                            <tr>
                                <td rowspan="2" width="60px" align="center"><?php echo $folder; ?></td>
                                <td>
                                    <div class="in-line">
                                        <a href="javascript:void(0)"
                                            data-id="<?php
                                                echo base64_encode($arrResult['ANNOUNCE_NO'].'/'.$folder.'/'.$file);
                                            ?>"
                                            class="download-file fl-w-700 style_real_display"
                                            name="lnkAttachment<?php echo $folder; ?>"><?php
                                                echo $file;
                                        ?></a>
                                    </div>
                                    <div class="in-line col-right right-20">
                                        <label for="chkDelete<?php echo $folder; ?>">
                                            <input type="checkbox" class="chkDelete" id="chkDelete<?php echo $folder; ?>"
                                                name="chkDelete<?php echo $folder; ?>" />
                                            <?php echo $arrTxtTrans['PUBLIC_TEXT_009']; ?>
                                        </label>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="file" class="t-input-file"  style="width:80%;" id="file<?php echo $folder; ?>"
                                        name="file<?php echo $folder; ?>" disabled />
                                </td>
                            </tr>
                        <?php   } else { ?>
                            <tr>
                                <td  width="60px" align="center"><?php echo $folder; ?></td>
                                <td>
                                    <input type="file" class="t-input-file"  style="width:80%;" id="file<?php echo $folder; ?>"
                                        name="file<?php echo $folder; ?>" />
                                </td>
                            </tr>
                        <?php
                                }
                            }
                        ?>
                    </table>

                    <input type="hidden" name="announceNo"
                            value="<?php echo $arrResult['ANNOUNCE_NO']; ?>">

                    <div class="form-footer top-20">
                        <div class="in-line">
                            <button type="button" class="tbtn tbtn-defaut" name="btnPost"
                                data-id="<?php echo $arrResult['ANNOUNCE_NO']; ?>">
                                <?php echo $arrTxtTrans['PUBLIC_BUTTON_005']; ?>
                            </button>
                            <?php if($arrResult['ANNOUNCE_NO'] != '' && $arrResult['COMP_DATE'] == '') { ?>
                            <button type="button" id="btnDone" class="tbtn tbtn-defaut"
                                data-id="<?php echo $arrResult['ANNOUNCE_NO']; ?>"><?php
                                echo $arrTxtTrans['PUBLIC_BUTTON_007'];
                            ?></button>
                            <?php } ?>
                        </div>
                        <div class="in-line text-right" style="float: right">
                            <button type="button" class="tbtn-cancel tbtn-defaut" name="btnClose"
                                    id="close">
                                <?php echo $arrTxtTrans['PUBLIC_BUTTON_003']; ?>
                            </button>
                        </div>
                    </div>
                </form>

                <form action="announce_edit_proc.php" method="post" id="formDownload">
                    <input type="hidden" name="path" value="">
                    <input type="hidden" name="mode" value="99">
                </form>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var csrf = $('meta[name="csrf-token"]').attr('content');
        var inputToken = '<input type="hidden" name="X-CSRF-TOKEN" value="'+csrf+'">';
        $('form').append(inputToken);

        var flagShowMsgCheckTranslateManual = false;
        var flagCheckTranslateManual = false;

        $(document).ready(function() {
            $('input[name=chkManualInput]').on('change', function(e) {
                e.preventDefault();
                if($(this).is(':checked')) {
                    $('input[name=txtTitleTranslation], textarea[name=txtContentTranslation]')
                    .prop('disabled', false);
                    if(!flagShowMsgCheckTranslateManual
                        && ($.trim($('input[name=txtTitleTranslation]').val()) != ''
                            || $.trim($('textarea[name=txtContentTranslation]').val()) != '')) {
                        alert('<?php echo $arrTxtTrans['PUBLIC_MSG_020']; ?>');
                        flagShowMsgCheckTranslateManual = true;
                    }
                    flagCheckTranslateManual = true;
                    $('button[name=btnTranslation]').prop('disabled', true);
                } else {
                    $('input[name=txtTitleTranslation], textarea[name=txtContentTranslation]')
                    .prop('disabled', true);
                    $('button[name=btnTranslation]').prop('disabled', false);
                }
            });

            $('button[name=btnTranslation]').off().on('click', function(e) {
                e.preventDefault();
                $('.error-edit').html('');
                
                setTimeout(function(){
                if(window.navigator.onLine == false) {
                    alert('<?php echo $arrTxtTrans['PUBLIC_MSG_010']; ?>');
                    return;
                }

                var arr = $('form[name=announce-edit]').serializeArray();
                arr.push({ name: 'action', value: 'translate' });
                if(!$('input[name=chkManualInput]').is(':checked')) {
                    arr.push({
                        name: 'chkManualInput',
                        value: 'off'
                    });

                    arr.push({
                        name: 'txtTitleTranslation',
                        value: $('input[name=txtTitleTranslation]').val()
                    });
                    arr.push({
                        name: 'txtContentTranslation',
                        value: $('textarea[name=txtContentTranslation]').val()
                    });
                }

                arr.push({
                    name: 'txtTitleOriginal',
                    value: $('input[name=txtTitleOriginal]').val()
                });
                arr.push({
                    name: 'txtContentOriginal',
                    value: $('textarea[name=txtContentOriginal]').val()
                });


                $.ajax({
                    url: 'announce_edit_proc.php',
                    type: 'POST',
                    data: arr,
                    async: false,
                    success: function(result) {
                        if(result != '') {
                            if($.trim(result) == 'window.location.href="login.php";') {
                                window.location.href="login.php";
                                return;
                            }
                            if(result == 900){
        						alert('<?php echo $arrTxtTrans['PUBLIC_MSG_009'] ?>');
        						window.location.href="login.php";
        	                    return;
        					}
                            var res = getData(result);
                            if(typeof res != 'string') {
                                if(res['error'] != '') {
                                    if(res['trans-error'] == 'error') {
                                        alert('<?php echo $arrTxtTrans['PUBLIC_MSG_010']; ?>');
                                    } else {
                                        $('.error-edit').html(res['error']);
                                        $('#myModal').animate({ scrollTop: 0 }, '10');
                                    }
                                }
                                if(res['success'] != '') {
                                    $('input[name=txtTitleTranslation]').val(res['success']['titleTranslate']);
                                    $('textarea[name=txtContentTranslation]')
                                    .val(res['success']['contentTranslate']);
                                }
                            }
                            return;
                        }
                    },
                    error: function(e) {
                        console.log(e);
                    }
                });
                return;
            },15);
            });

            $('.chkDelete').on('change', function(e) {
                var name = $(this).attr('name');
                var key = name.substring(name.length - 1, name.length);
                if($(this).is(':checked')) {
                    $('input[name=file'+key+']').prop('disabled', false);
                } else {
                    $('input[name=file'+key+']').prop('disabled', true).val('');
                }
            });

            $('button[name=btnPost]').off().on('click', function(e) {
                e.preventDefault();
                $('.error-edit').html('');
                setTimeout(function(){
                var id = $('button[name=btnPost]').attr('data-id');
                var arr = $('form[name=announce-edit]').serializeArray();
                arr.push({ name: 'action', value: (id == '') ? 'pre-insert' : 'pre-update' });
                arr.push({ name: 'id', value: id });
                if(!$('input[name=chkManualInput]').is(':checked')) {
                    if(window.navigator.onLine == false) {
                        alert('<?php echo $arrTxtTrans['PUBLIC_MSG_010']; ?>');
                        return;
                    }

                    arr.push({
                        name: 'chkManualInput',
                        value: 'off'
                    });

                    arr.push({
                        name: 'txtTitleTranslation',
                        value: $('input[name=txtTitleTranslation]').val()
                    });
                    arr.push({
                        name: 'txtContentTranslation',
                        value: $('textarea[name=txtContentTranslation]').val()
                    });
                }
                arr.push({
                    name: 'txtTitleOriginal',
                    value: $('input[name=txtTitleOriginal]').val()
                });
                arr.push({
                    name: 'txtContentOriginal',
                    value: $('textarea[name=txtContentOriginal]').val()
                });

                var total_size = 0;
                $('input[type=file]').each(function(){
                    if($(this).val()){
                        var file = $(this).prop('files')[0];
                        total_size = total_size + file.size;
                    }
                });

                if( total_size > <?php echo TOTAL_FILE_SIZE ; ?>){
                    $('.error-edit').html('<div><?php echo $arrTxtTrans['PUBLIC_MSG_019']; ?></div>');
                    $('#myModal').animate({ scrollTop: 0 }, '10');
                    return;
                }

                var flagStop = false;
                var msgConfirm = '';
                var titleTranslate = '';
                var contentTranslate = '';
                var intAuto = 0;

                $.ajax({
                    url: 'announce_edit_proc.php',
                    type: 'POST',
                    data: arr,
                    async: false,
                    success: function(result) {
                        if(result != '') {
                            if($.trim(result) == 'window.location.href="login.php";') {
                                window.location.href="login.php";
                                return;
                            }
                            if(result == 900){
        						alert('<?php echo $arrTxtTrans['PUBLIC_MSG_009'] ?>');
        						window.location.href="login.php";
        	                    return;
        					}
                            var res = getData(result);
                            if(typeof res != 'string') {
                                if(res['error'] != '') {
                                    if(res['trans-error'] == 'error') {
                                        alert('<?php echo $arrTxtTrans['PUBLIC_MSG_010']; ?>');
                                    } else {
                                        $('.error-edit').html(res['error']);
                                        $('#myModal').animate({ scrollTop: 0 }, '10');
                                    }
                                    flagStop = true;
                                }
                            }
                            if(res['success'] != '') {
                                titleTranslate = res['success']['titleTranslate'];
                                contentTranslate = res['success']['contentTranslate'];
                            }
                            msgConfirm = res['confirm'];
                            intAuto = res['auto'];
                        }
                    },
                    error: function(e) {
                        console.log(e);
                    }
                });

                if(!flagStop) {
                    if(msgConfirm != '' && confirm(msgConfirm)) {
                        var formData = new FormData();
                        var data = $('form[name=announce-edit]').serializeArray();
                        data.push({ name: 'action', value: (id == '') ? 'insert' : 'update' });
                        data.push({ name: 'id', value: id });
                        if(!$('input[name=chkManualInput]').is(':checked')) {
                            data.push({ name: 'chkManualInput', value: 'off' });
                        }
                        data.push({ name: 'txtTitleOriginal', value: $('input[name=txtTitleOriginal]').val() });
                        data.push({ name: 'txtContentOriginal', value: $('textarea[name=txtContentOriginal]').val() });
                        data.push({ name: 'txtTitleTranslation', value: titleTranslate });
                        data.push({ name: 'txtContentTranslation', value: contentTranslate });
                        data.push({ name: 'auto', value: intAuto });
                        data.push({ name: 'mode', value: 1 });

                        $('.chkDelete').each(function(i, e) {
                            var name = $(this).attr('name');
                            if($(this).is(':checked'))  {
                                $.each(data, function(index, obj) {
                                    if(obj.name == name) {
                                        obj.value = 1;
                                    }
                                });
                            } else {
                                data.push({ name: name, value: -1 });
                            }
                        });

                        var listMsg  = [];
                        $('input[name^=file]').each(function(i, e) {
                            if($(this).val() != '') {
                                var name = $(this).attr('name');
                                var file = $(this).prop('files')[0];
                                formData.append(name, file);
                                checkFileExist(name, file, listMsg);
                            }
                        });

                        if(listMsg.length > 0) {
                            $.each(listMsg, function(i, e) {
                                $('.error-edit').append('<div>'+e+'</div>');
                                $('#myModal').animate({ scrollTop: 0 }, '10');
                            });
                            return false;
                        }

                        $.each(data, function(index, obj) {
                            formData.append(obj.name, obj.value);
                        });

                        var flagHasError = false;
                        $.ajax({
                            url: 'announce_edit_proc.php',
                            type: 'POST',
                            data: formData,
                            cache: false,
                            contentType: false,
                            processData: false,
                            async: false,
                            success: function(result) {
                                if(result != '') {
                                    if($.trim(result) == 'window.location.href="login.php";') {
                                        window.location.href="login.php";
                                        return;
                                    }
                                    if(result == 900){
                						alert('<?php echo $arrTxtTrans['PUBLIC_MSG_009'] ?>');
                						window.location.href="login.php";
                	                    return;
                					}
                                    var res = getData(result);
                                    if(res['error'] != '') {
                                        if(res['error'] == 'alreadyDelError') {
                                        	alert('<?php echo $arrTxtTrans['ANNOUNCE_EDIT_MSG_001'] ?>');
                                            if('<?php echo $intRefScreen; ?>' != '0'){
                                                loadPortalClose();
                                                $('#myModal').modal('hide');
                                            }else{
                                                setTimeout(function() {
                                                    window.location.reload();
                                                }, 300);
                                                $('#myModal').modal('hide');
                                            }
                                            flagHasError = true;
                                            return;
                                        }else{
                                            $('.error-edit').html(res['error']);
                                            $('#myModal').animate({ scrollTop: 0 }, '10');
                                            flagHasError = true;
                                            return;
                                        }
                                    }
                                }
                            },
                            error: function(e) {
                                console.log(e);
                            }
                        });
                        if(!flagHasError) {
                            var id = '<?php echo $arrResult['ANNOUNCE_NO'] ?>';
                            var key = '<?php if(isset($_POST['screen'])) {
                                                if($_POST['screen'] == '1') {
                                                    echo "portal";
                                                } else {
                                                    echo "announce_mng";}
                                            } else { echo -1; } ?>';
                            if(id != '') {
                                var a = $("<button>")
                                        .attr('class', 'load-modal-announce')
                                        .attr("href", 'announce_view.php')
                                        .attr("data-id", id)
                                        .attr("data-screen", key)
                                        .attr("type", '<?php echo $intRefScreen ?>')
                                        .appendTo("body");
                                a[0].click();
                                a.remove();
                            } else {
                            	loadPortalClose();
                                $('#myModal').modal('hide');
                            }
                            return;
                        }
                    }
                }
        },15);
            });

            $('button#btnDone').off().on('click', function(e) {
                e.preventDefault();
                $('.error-edit').html('');
                setTimeout(function(){
                if(confirm('<?php
                    echo $arrTxtTrans['ANNOUNCE_EDIT_MSG_017']; ?>')) {
                    var arr = $('form[name=announce-edit]').serializeArray();
                    arr.push({ name: 'mode', value: 2 });
                    $.ajax({
                        url: 'announce_edit_proc.php',
                        type: 'POST',
                        data: arr,
                        async: false,
                        success: function(result) {
                            if(result != '') {
                                if($.trim(result) == 'window.location.href="login.php";') {
                                    window.location.href="login.php";
                                    return;
                                }
                                if(result == 900){
            						alert('<?php echo $arrTxtTrans['PUBLIC_MSG_009'] ?>');
            						window.location.href="login.php";
            	                    return;
            					}

                                if(result == 2) {
                                	alert('<?php echo $arrTxtTrans['ANNOUNCE_EDIT_MSG_001'] ?>');
                                    if('<?php echo $intRefScreen; ?>' != '0'){
                                        loadPortalClose();
                                        $('#myModal').modal('hide');
                                    }else{
                                        setTimeout(function() {
                                            window.location.reload();
                                        }, 300);
                                        $('#myModal').modal('hide');
                                    }
                                    flagHasError = true;
                                    return;
                                }

                                if(result == 0) {
                                    $('.error-edit').html('<div><?php
                                    echo $arrTxtTrans['PUBLIC_MSG_003']; ?></div>');
                                    $('#myModal').animate({ scrollTop: 0 }, 10);
                                    return;
                                }
                                if(result == 1) {
                                    var id = '<?php echo $arrResult['ANNOUNCE_NO'] ?>';
                                    var key = '<?php if(isset($_POST['screen'])) {
                                                    if($_POST['screen'] == '0') {
                                                        echo "portal";
                                                    } else {
                                                        echo "announce_mng";}
                                                } else { echo -1; } ?>';
                                    if(id != '') {
                                        var a = $("<button>")
                                                .attr('class', 'load-modal-announce')
                                                .attr("href", 'announce_view.php')
                                                .attr("data-id", id)
                                                .attr("data-screen", key)
                                                .attr("type", '<?php echo $intRefScreen ?>')
                                                .appendTo("body");
                                        a[0].click();
                                        a.remove();
                                    } else {
                                        $('#myModal').modal('hide');
                                        setTimeout(function() {
                                            window.location.reload();
                                        }, 300);
                                    }
                                    return;
                                }
                            }
                        },
                        error: function(e) {
                            console.log(e);
                        }
                    });
                }
            },15);
            });

            $('.download-file').off().on('click', function(e) {
                e.preventDefault();
                $('.error-edit').html('');
                var arr = [
                    {
                        name: 'X-CSRF-TOKEN',
                        value: '<?php echo (isset($_POST['X-CSRF-TOKEN'])
                                            ? $_POST['X-CSRF-TOKEN'] : ''); ?>'
                    },{
                        name: 'file',
                        value: $(this).attr('data-id')
                    },{
                        name: 'action',
                        value: 'checkFile'
                    }
                ];
                var blnCanDownload = false;
                $.ajax({
                    url: 'announce_edit_proc.php',
                    type: 'POST',
                    data: arr,
                    async: false,
                    success: function(result) {
                        if($.trim(result) == 'window.location.href="login.php";') {
                            window.location.href="login.php";
                            return;
                        }
                        if(result == 900){
    						alert('<?php echo $arrTxtTrans['PUBLIC_MSG_009'] ?>');
    						window.location.href="login.php";
    	                    return;
    					}
                        if(result == 0) {
                            $('.error-edit').html('<div><?php
                                echo $arrTxtTrans['ANNOUNCE_EDIT_MSG_002']; ?></div>');
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
            });

            $('button[name=btnClose]').off().on('click', function(e) {
                e.preventDefault();
                var id = '<?php echo $arrResult['ANNOUNCE_NO'] ?>';
                var key = '<?php if(isset($_POST['screen'])) {
                                    if($_POST['screen'] == '1') {
                                        echo "portal";
                                    } else {
                                        echo "announce_mng";}
                                    } else {
                                        echo -1; } ?>';
                if(id != '') {
                    var a = $("<button>")
                            .attr('class', 'load-modal-announce')
                            .attr("href", 'announce_view.php')
                            .attr("data-id", id)
                            .attr("data-screen", key)
                            .attr("type", '<?php echo $intRefScreen ?>')
                            .appendTo("body");
                    a[0].click();
                    a.remove();
                    $('#myModal').modal('show');
                } else {
                    loadPortalClose();
                    $('#myModal').modal('hide');
                }
            });
        });

        function getData(str) {
            var res;
            try {
                res = JSON.parse(str);
            } catch (e) {
                res = str;
            }
            return res;
        }

        function checkFileExist(name, file, listMsg) {
            var arrMsg = {
                'file1': '<?php
                            echo $arrTxtTrans['PUBLIC_MSG_011'].$arrTxtTrans['PUBLIC_MSG_016']; ?>',
                'file2': '<?php
                            echo $arrTxtTrans['PUBLIC_MSG_012'].$arrTxtTrans['PUBLIC_MSG_016']; ?>',
                'file3': '<?php
                            echo $arrTxtTrans['PUBLIC_MSG_013'].$arrTxtTrans['PUBLIC_MSG_016']; ?>',
                'file4': '<?php
                            echo $arrTxtTrans['PUBLIC_MSG_014'].$arrTxtTrans['PUBLIC_MSG_016']; ?>',
                'file5': '<?php
                            echo $arrTxtTrans['PUBLIC_MSG_015'].$arrTxtTrans['PUBLIC_MSG_016']; ?>'
            }
            if(file['size'] == 0) {
                listMsg.push(arrMsg[name]);
                return false;
            }
            return true;
        }
    </script>
</body>
</html>