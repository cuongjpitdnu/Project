<?php
    /*
     * @bulletin_board_edit.php
     *
     * @create 2020/03/31 AKB Chien
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
    fncSessionTimeOutCheck();

    // ログインユーザ情報を取得
    $objLoginUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);

    $intLanguageType = $objLoginUserInfo->intLanguageType;

    $arrTitleMsg =  array(
        'BULLETIN_BOARD_EDIT_TEXT_001',
        'BULLETIN_BOARD_EDIT_TEXT_003',
        'PUBLIC_TEXT_002',
        'PUBLIC_TEXT_003',
        'BULLETIN_BOARD_EDIT_TEXT_004',
        'PUBLIC_TEXT_002',
        'PUBLIC_TEXT_008',
        'BULLETIN_BOARD_EDIT_TEXT_004',
        'PUBLIC_TEXT_003',
        'BULLETIN_BOARD_EDIT_TEXT_005',
        'PUBLIC_TEXT_002',
        'PUBLIC_TEXT_003',
        'PUBLIC_BUTTON_004',
        'PUBLIC_BUTTON_014',
        'PUBLIC_BUTTON_003',
        'PUBLIC_MSG_049',
        'PUBLIC_MSG_001',
        'BULLETIN_BOARD_EDIT_MSG_002',
        'PUBLIC_MSG_020',
        'PUBLIC_MSG_010',
        'PUBLIC_MSG_009',

        // 2020/04/14 T.Masuda 画面変更のため
        'BULLETIN_BOARD_EDIT_TEXT_002',
        // 2020/04/14 T.Masuda

        // 2020/04/15 start
        'BULLETIN_BOARD_EDIT_MSG_007'
        // 2020/04/15 end
    );

    // get list text(header, title, msg) with languague_type of user logged
    $arrTxtTrans = getListTextTranslate($arrTitleMsg, $intLanguageType);

    // GET通信にて遷移してきた場合、以下のメッセージをアラート表示し、遷移元画面に戻す。
    fncGetRequestCheck($arrTxtTrans);

    // 2020/04/15 AKB Chien - start - update document 2020/04/15
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
    // 2020/04/15 AKB Chien - end - update document 2020/04/15

    // write log when access this screen
    $strLog = DISPLAY_TITLE.'　表示(ユーザID = '.$objLoginUserInfo->strUserID.') ';
    $strLog .= isset($_SERVER['HTTP_REFERER']) ? $_SERVER['HTTP_REFERER'] : null;
    fncWriteLog(LogLevel['Info'], LogPattern['View'], $strLog);

    // get bulletin_board_no
    $intId = @$_POST['id'] ? $_POST['id'] : '';

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

    // get info of bulletin_board_no
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
     *	@create	2020/03/31 Chien AKB
     *	@update
     *	@params	$intId      bulletin_no
     *	@return $arrResult  array data info of bulletin_no
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

    $strCSRF = isset($_POST['X-CSRF-TOKEN']) ? $_POST['X-CSRF-TOKEN'] : '';
?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title><?php
        echo $arrTxtTrans['BULLETIN_BOARD_EDIT_TEXT_001'];
    ?></title>
    <link rel="stylesheet" type="text/css" href="css/style.css">
    <?php if($objLoginUserInfo->intMenuPerm != 1) { ?>
        <script>
            alert('<?php echo $arrTxtTrans['PUBLIC_MSG_009']; ?>');
            window.location.href="login.php";
        </script>
    <?php die(); } ?>
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
            alert('<?php echo $arrTxtTrans['BULLETIN_BOARD_EDIT_MSG_007']; ?>');
            $('#myModal').modal('hide');
            setTimeout(function() {
                window.location.reload();
            }, 300);
        </script>
    <?php
            }
        }
    ?>
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
                echo $arrTxtTrans['BULLETIN_BOARD_EDIT_TEXT_001'];
            ?></div>
            <div class="form-body">
                <div><font color="red" class="error-edit"></font></div>
                <form action="" method="POST" name="bulletin-edit" autocomplete="off">
                    <div class="cont-title">
                        <div class="in-line"><?php
                            echo $arrTxtTrans['BULLETIN_BOARD_EDIT_TEXT_002']; ?></div>
                        <div class="in-line"><?php
                            echo date('Y/m/d H:i', strtotime($arrResult['OCCURRENCE_DATE']));
                        ?></div>
                    </div>
                    <!-- <br/> -->
                    <div>
                        <p style="background-color:#4169e1">
                            <legend><?php
                                echo BULLETIN_BOARD_EDIT_TEXT_003_JPN; ?>/<?php
                                echo BULLETIN_BOARD_EDIT_TEXT_003_ENG;
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

                    <p style="background-color:#4169e1">
                        <legend><?php
                            echo BULLETIN_BOARD_EDIT_TEXT_004_JPN; ?>/<?php
                            echo BULLETIN_BOARD_EDIT_TEXT_004_ENG;
                        ?></legend>
                    </p>
                    <div class="info">
                        <div class="line">
                            <div >(<?php
                                echo PUBLIC_TEXT_002_JPN; ?>/<?php
                                echo PUBLIC_TEXT_002_ENG;
                            ?>)</div>
                            <div class="text-input text-bold style_real_display"
                                name="txtIncidentOriginal"><?php
                                echo $arrResult['INCIDENT_NAME_JPN'];
                            ?></div>
                        </div>
                    </div>

                    <div>
                        <div class="in-line tlabel">
                           <label for="chkTran">
                            <input type="checkbox" name="chkManualInput" id="chkTran" <?php
                                if($arrResult['CORRECTION_FLAG'] == 1) {
                                    echo 'checked';
                                }
                            ?>><?php
                                echo $arrTxtTrans['PUBLIC_TEXT_008'];
                            ?>
                            </label>
                        </div>
                        <div class="in-line text-input col-right">
                            <button type="submit" class="tbtn-cancel tbtn-defaut"
                                name="btnTranslation" <?php
                                if($arrResult['CORRECTION_FLAG'] == 1) {
                                    echo 'disabled';
                                }
                                ?>><?php
                                echo $arrTxtTrans['PUBLIC_BUTTON_004'];
                            ?></button>
                        </div>
                    </div>

                    <div class="top-20">
                        <div class="title"><?php
                            echo BULLETIN_BOARD_EDIT_TEXT_004_JPN; ?>/<?php
                            echo BULLETIN_BOARD_EDIT_TEXT_004_ENG;
                        ?> （<?php echo $arrTxtTrans['PUBLIC_TEXT_003']; ?>）</div>
                        <input type="text" class="form-control" name="txtIncidentTranslation" <?php
                            if($arrResult['CORRECTION_FLAG'] == 0) { echo 'disabled'; }
                        ?> value="<?php
                            echo $arrResult['INCIDENT_NAME_ENG'];
                        ?>" />
                    </div>

                    <br/>

                    <div class="col-left">
                        <p style="background-color:#4169e1">
                            <legend><?php
                                echo BULLETIN_BOARD_EDIT_TEXT_005_JPN; ?>/<?php
                                echo BULLETIN_BOARD_EDIT_TEXT_005_ENG;
                            ?> </legend>
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

                    <div class="form-footer top-20">
                        <div class="in-line">
                            <button type="button" class="tbtn tbtn-defaut" name="btnPost"
                                data-id="<?php echo $arrResult['BULLETIN_BOARD_NO']; ?>"><?php
                                echo $arrTxtTrans['PUBLIC_BUTTON_014'];
                            ?></button>　
                        </div>
                        <div class="in-line text-right" style="float: right">
                            <button type="button" class="tbtn-cancel tbtn-defaut"
                                name="btnClose" id="close" data-dismiss="modal"><?php
                                echo $arrTxtTrans['PUBLIC_BUTTON_003'];
                            ?></button>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    </div>

    <script>
        var csrf = $('meta[name="csrf-token"]').attr('content');
        var inputToken = '<input type="hidden" name="X-CSRF-TOKEN" value="'+csrf+'">';
        $('form').append(inputToken);

        var flagShowMsgCheckTranslateManual = false;
        var flagCheckTranslateManual = false;

        $(document).ready(function() {
            $('input[name=chkManualInput]').on('change', function(e) {
                e.preventDefault();
                if($(this).is(':checked')) {
                    $('input[name=txtIncidentTranslation]').prop('disabled', false);
                    if(!flagShowMsgCheckTranslateManual
                        && ($.trim($('input[name=txtIncidentTranslation]').val()) != '')) {
                        alert('<?php echo $arrTxtTrans['PUBLIC_MSG_020']; ?>');
                        flagShowMsgCheckTranslateManual = true;
                    }
                    flagCheckTranslateManual = true;
                    $('button[name=btnTranslation]').prop('disabled', true);
                } else {
                    $('input[name=txtIncidentTranslation]').prop('disabled', true);
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

                var arr = $('form[name=bulletin-edit]').serializeArray();
                arr.push({ name: 'action', value: 'translate' });
                arr.push({
                    name: 'txtIncidentOriginal',
                    value: $.trim($('div[name=txtIncidentOriginal]').text())
                });
                if(!$('input[name=chkManualInput]').is(':checked')) {
                    arr.push({
                        name: 'chkManualInput',
                        value: 'off'
                    });
                    arr.push({
                        name: 'txtIncidentTranslation',
                        value: $('input[name=txtIncidentTranslation]').val()
                    });
                }

                $.ajax({
                    url: 'bulletin_board_edit_proc.php',
                    type: 'POST',
                    data: arr,
                    async: false,
                    success: function(result) {
                        if(result != '') {
                            if($.trim(result) == 'window.location.href="login.php";') {
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
                                    }
                                }
                                if(res['success'] != '') {
                                    $('input[name=txtIncidentTranslation]').val(res['success']['incidentTranslate']);
                                }
                            }
                        }
                    },
                    error: function(e) {
                        console.log(e);
                    }
                });
                },15);
            });

            $('button[name=btnPost]').off().on('click', function(e) {
                e.preventDefault();
                $('.error-edit').html('');
                setTimeout(function(){

                var id = $('button[name=btnPost]').attr('data-id');
                var arr = $('form[name=bulletin-edit]').serializeArray();
                arr.push({ name: 'action', value: 'pre-update' });
                arr.push({
                    name: 'txtIncidentOriginal',
                    value: $.trim($('div[name=txtIncidentOriginal]').text())
                });
                if(!$('input[name=chkManualInput]').is(':checked')) {
                    arr.push({
                        name: 'chkManualInput',
                        value: 'off'
                    });
                    arr.push({
                        name: 'txtIncidentTranslation',
                        value: $('input[name=txtIncidentTranslation]').val()
                    });
                }

                var flagStop = false;
                var msgConfirm = '';
                var incidentTranslate = '';
                var intAuto = 0;

                $.ajax({
                    url: 'bulletin_board_edit_proc.php',
                    type: 'POST',
                    data: arr,
                    async: false,
                    success: function(result) {
                        if(result != '') {
                            if($.trim(result) == 'window.location.href="login.php";') {
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
                                    }
                                    flagStop = true;
                                }
                            }
                            if(res['success'] != '') {
                                console.log(res);
                                incidentTranslate = res['success']['incidentTranslate'];
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
                        $('input[name=txtIncidentTranslation]').val(incidentTranslate);
                        var data = $('form[name=bulletin-edit]').serializeArray();
                        data.push({ name: 'action', value: 'update' });
                        data.push({ name: 'id', value: id });
                        data.push({
                            name: 'txtIncidentOriginal',
                            value: $.trim($('div[name=txtIncidentOriginal]').text())
                        });
                        if(!$('input[name=chkManualInput]').is(':checked')) {
                            data.push({ name: 'chkManualInput', value: 'off' });
                        }
                        data.push({
                            name: 'txtIncidentTranslation',
                            value: $('input[name=txtIncidentTranslation]').val()
                        });
                        data.push({ name: 'auto', value: intAuto });
                        data.push({ name: 'mode', value: 1 });

                        var flagHasError = false;
                        $.ajax({
                            url: 'bulletin_board_edit_proc.php',
                            type: 'POST',
                            data: data,
                            cache: false,
                            async: false,
                            success: function(result) {
                                if(result != '') {
                                    var res = getData(result);
                                    if(res['error'] != '') {
                                    	if(res['error'] == 'alreadyDelError') {
                                    		alert('<?php echo $arrTxtTrans['BULLETIN_BOARD_EDIT_MSG_007']; ?>');
                                    		setTimeout(function() {
                                                window.location.reload();
                                            }, 300);
                                            $('#myModal').modal('hide');
                                            flagHasError = true;
                                            return;
                                    	}else{
                                            $('.error-edit').html(res['error']);
                                            $('input[name=txtIncidentTranslation]').val('');
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
                            var id = '<?php echo $arrResult['BULLETIN_BOARD_NO'] ?>';
                            if(id != '') {
                                var a = $("<button>")
                                        .attr('class', 'load-modal')
                                        .attr("href", 'bulletin_board_view.php')
                                        .attr("data-id", id)
                                        .attr("data-screen", 'bulletin_mng')
                                        .appendTo("body");
                                a[0].click();
                                a.remove();
                            } else {
                                $('#myModal').modal('hide');
                                setTimeout(function() {
                                    window.location.reload();
                                }, 300);
                            }
                        }
                    }
                }
                },15);
            });

            $('button[name=btnClose]').off().on('click', function(e) {
                e.preventDefault();
                var id = '<?php echo $arrResult['BULLETIN_BOARD_NO'] ?>';
                var key = '<?php if(isset($_POST['screen'])) {
                                    if($_POST['screen'] == -1) {
                                        echo "bulletin_edit";
                                    } else { echo -1;} } else { echo -1; } ?>';
                console.log(key);
                if(id != '') {
                    var a = $("<button>")
                            .attr('class', 'load-modal')
                            .attr("href", 'bulletin_board_view.php')
                            .attr("data-id", id)
                            .attr("data-screen", 'bulletin_mng')
                            .appendTo("body");
                    a[0].click();
                    a.remove();
                    $('#myModal').modal('show');
                } else {
                    $('#myModal').modal('hide');
                    setTimeout(function() {
                        window.location.reload();
                    }, 300);
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
    </script>
</body>
</html>