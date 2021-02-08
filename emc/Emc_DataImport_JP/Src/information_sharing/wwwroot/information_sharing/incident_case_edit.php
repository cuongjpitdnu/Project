<?php
/*
 * @JCMG事案登録編集画面
 *
 * @create 2020/03/19 KBS T.Masuda
 * @update 2020/03/26 KBS T.Masuda  仕様変更のため
 */

    require_once('common/common.php');

    header('Content-type: text/html; charset=utf-8');
    header('X-FRAME-OPTIONS: DENY');

    if(fncConnectDB() == false) {
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

    fncSessionTimeOutCheck();

    const SCREEN_NAME = 'JCMG事案登録編集画面';

    //ログインユーザ情報を取得
    $objLoginUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);

    //ログインユーザの言語タイプ
    $intLanguageType = $objLoginUserInfo->intLanguageType;

    //表示テキスト・メッセージ
    $arrTitleMsg =  array(
        'PUBLIC_MSG_049',

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
        'PUBLIC_BUTTON_007',
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

        'PUBLIC_MSG_003',
        'PUBLIC_MSG_001',
        'INCIDENT_CASE_EDIT_MSG_006',
    );

    //言語タイプに応じたテキスト・メッセージ
    $arrTextTranslate = getListTextTranslate($arrTitleMsg, $intLanguageType);

    //URLを直接指定した場合
    if($_SERVER['REQUEST_METHOD'] == 'GET'){
        echo '<script>alert("'.$arrTextTranslate['PUBLIC_MSG_049'].'");
                       history.back();</script>';
        exit;
    }

    //redirect if dont have permission
    if($objLoginUserInfo->intIncidentCaseRegPerm != 1){
        echo "
        <script>
            alert('".$arrTextTranslate['PUBLIC_MSG_009']."');
            window.location.href = 'login.php';
        </script>
        ";
        exit();
    }

    //VIEWログ内容
    $strLogView = SCREEN_NAME.'　表示'.
                  '(ユーザID = '.$objLoginUserInfo->strUserID.') '.
                   (isset($_SERVER['HTTP_REFERER']) ? $_SERVER['HTTP_REFERER'] : null);

    //表示ログ登録
    fncWriteLog(LogLevel['Info'], LogPattern['View'],$strLogView );

    //2020/04/21 T.Masuda 遷移元画面を取得
    $strScreenRef = @$_POST['screen'] ? $_POST['screen'] : '';
    //2020/04/21 T.Masuda

    //編集時の対象JCMG事案No
    $intId = @$_POST['id'] ? $_POST['id'] : '';

    //編集時の表示JCMG事案情報
    $arrDataIncident = fncGetInfoIncident($intId, SCREEN_NAME);

    //データ取得失敗時
    if($arrDataIncident == 0 ){
        $strAlert = '<script>alert("'.$arrTextTranslate['PUBLIC_MSG_001'].'");
                      setTimeout(function() {
                        window.location.reload();
                     }, 300);';
        $strAlert.= "$('#myModal').modal('hide');</script>";
        echo $strAlert;
        exit;
    }

    //編集時JCMG事案情報格納
    $arrRes = array(
        'INCIDENT_CASE_NO' => '',
        'S_DATE' => '',
        'TITLE_ORIGINAL' => '',
        'TITLE_TRANSLATE' => '',
        'CONTENTS_ORIGINAL' => '',
        'CONTENTS_TRANSLATE' => '',
        'LANGUAGE_TYPE' => '',
        'CORRECTION_FLAG' => '',
        'COMP_DATE' => '',
    );
    //編集の場合対象のJCMG事案情報を格納
    if($arrDataIncident != 0 && count($arrDataIncident) > 0) {
        //タイトル（原文）
        $strTitleOriginal = '';
        //タイトル（翻訳）
        $strTitleTranslate = '';
        //内容（原文）
        $strContentsOriginal = '';
        //内容（翻訳）
        $strContentsTranslate = '';
        //開始日
        $dtmStartDate = New DateTime($arrDataIncident[0]['S_DATE']);
        //言語タイプが日本語か英語
        if($arrDataIncident[0]['LANGUAGE_TYPE'] == 0) {
            $strTitleOriginal = @$arrDataIncident[0]['TITLE_JPN']
                               ? fncHtmlSpecialChars(trim($arrDataIncident[0]['TITLE_JPN'])) : '';
            $strTitleTranslate = @$arrDataIncident[0]['TITLE_ENG']
                                ? fncHtmlSpecialChars(trim($arrDataIncident[0]['TITLE_ENG'])) : '';
            $strContentsOriginal = @$arrDataIncident[0]['CONTENTS_JPN']
                                  ? fncHtmlSpecialChars(trim($arrDataIncident[0]['CONTENTS_JPN'])) : '';
            $strContentsTranslate = @$arrDataIncident[0]['CONTENTS_ENG']
                                   ? fncHtmlSpecialChars(trim($arrDataIncident[0]['CONTENTS_ENG'])) : '';
        } else {
            $strTitleOriginal = @$arrDataIncident[0]['TITLE_ENG']
                               ? fncHtmlSpecialChars(trim($arrDataIncident[0]['TITLE_ENG'])) : '';
            $strTitleTranslate = @$arrDataIncident[0]['TITLE_JPN']
                                ? fncHtmlSpecialChars(trim($arrDataIncident[0]['TITLE_JPN'])) : '';
            $strContentsOriginal = @$arrDataIncident[0]['CONTENTS_ENG']
                                  ? fncHtmlSpecialChars(trim($arrDataIncident[0]['CONTENTS_ENG'])) : '';
            $strContentsTranslate = @$arrDataIncident[0]['CONTENTS_JPN']
                                   ? fncHtmlSpecialChars(trim($arrDataIncident[0]['CONTENTS_JPN'])) : '';
        }

        //表示するJCMGデータを配列に格納
        $arrRes = array(
            'INCIDENT_CASE_NO' => $arrDataIncident[0]['INCIDENT_CASE_NO'],
            'S_DATE' => $dtmStartDate->format('Y/m/d H:i'),
            'TITLE_ORIGINAL' => $strTitleOriginal,
            'TITLE_TRANSLATE' => $strTitleTranslate,
            'CONTENTS_ORIGINAL' => $strContentsOriginal,
            'CONTENTS_TRANSLATE' => $strContentsTranslate,
            'LANGUAGE_TYPE' => $arrDataIncident[0]['LANGUAGE_TYPE'],
            'CORRECTION_FLAG' => $arrDataIncident[0]['CORRECTION_FLAG'],
            'COMP_DATE' => $arrDataIncident[0]['COMP_DATE'],
        );

    //削除されたJCMG事案だった場合、アラート表示し、画面を閉じる
    }else if(count($arrDataIncident) == 0 && ! empty($intId)){
        $strAlert = '<script>alert("'.$arrTextTranslate['INCIDENT_CASE_EDIT_MSG_001'].'");
                      setTimeout(function() {
                        window.location.reload();
                     }, 300);';
        $strAlert.= "$('#myModal').modal('hide');</script>";
        echo $strAlert;
        exit;
    }

?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="csrf-token" content="<?php echo (isset($strCsrf) ? $strCsrf : ''); ?>">
    <title><?php echo $arrTextTranslate['INCIDENT_CASE_EDIT_TEXT_001']; ?></title>
    <link rel="stylesheet" type="text/css" href="css/style.css">
    <link rel="stylesheet" type="text/css" href="css/table-upload.css">
</head>
<body>
    <div class="main-content">
        <div class="main-form">
            <div class="form-title"><?php echo $arrTextTranslate['INCIDENT_CASE_EDIT_TEXT_001']; ?></div>
            <div class="form-body">
            	<div><font color="red" class="error-edit"></font></div>
            	<form action="" method="POST" name="incident_case-edit" autocomplete="off">
                    <div class="cont-title">
                        <div class="in-line"><?php echo $arrTextTranslate['INCIDENT_CASE_EDIT_TEXT_002']; ?></div>
                        <div class="in-line">
                        <input type="text" id="dtStartDate" name="dtStartDate" value="<?php if($arrRes['INCIDENT_CASE_NO'] == ''){echo date('Y/m/d H:i').'"';}else{echo $arrRes['S_DATE'].'" disabled';}?>></div>
                    </div>
                    <div class="text-right right-20">
                        <div class="select-container">
                            <select name="cmbTranslation">
                                <option value="ja" <?php if($arrRes['LANGUAGE_TYPE'] == 0 || ($arrRes['INCIDENT_CASE_NO'] == '' && $intLanguageType == 0)) { echo 'selected'; } ?>><?php echo fncHtmlSpecialChars(PUBLIC_TEXT_010_JPN); ?></option>
                                <option value="en" <?php if($arrRes['LANGUAGE_TYPE'] == 1 || ($arrRes['INCIDENT_CASE_NO'] == '' && $intLanguageType == 1)) { echo 'selected'; } ?>><?php echo fncHtmlSpecialChars(PUBLIC_TEXT_010_ENG); ?></option>
                            </select>
                        </div>
                    </div>
                    <br>
                    	<p style="background-color:#4169e1">
                        	<legend><?php echo $arrTextTranslate['PUBLIC_TEXT_002']; ?></legend>
                        </p>

                        <div ><?php echo fncHtmlSpecialChars(PUBLIC_TEXT_004_JPN).'/'.fncHtmlSpecialChars(PUBLIC_TEXT_004_ENG); ?> </div>
                        <input type="text" name="txtTitleOriginal" class="form-control" value="<?php echo $arrRes['TITLE_ORIGINAL']; ?>">
                        <div ><?php echo fncHtmlSpecialChars(PUBLIC_TEXT_005_JPN).'/'.fncHtmlSpecialChars(PUBLIC_TEXT_005_ENG); ?> </div>
                        <textarea name="txtContentOriginal" class="form-control" rows="2" cols="50"><?php echo $arrRes['CONTENTS_ORIGINAL']; ?></textarea>


                    <div class="clearfix mar-tl-10">
                        <div class="in-line">
                        	<label for="chkManualInput">
                            	<input type="checkbox" name="chkManualInput" id="chkManualInput" <?php if($arrRes['CORRECTION_FLAG'] == 1) { echo 'checked'; } ?>><?php echo $arrTextTranslate['PUBLIC_TEXT_008'];?>
                        	</label>
                        </div>
                        <div class="in-line col-right">
                            <button class="tbtn tbtn-defaut btnTranIncident" name="btnTranslation" <?php if($arrRes['CORRECTION_FLAG'] == 1) { echo 'disabled'; } ?>><?php echo $arrTextTranslate['PUBLIC_BUTTON_004']; ?></button>
                        </div>
                    </div>


                    	<p style="background-color:#4169e1">
                        <legend><?php echo $arrTextTranslate['PUBLIC_TEXT_003']; ?></legend>
                        </p>
                        <div ><?php echo fncHtmlSpecialChars(PUBLIC_TEXT_004_JPN).'/'.fncHtmlSpecialChars(PUBLIC_TEXT_004_ENG); ?> </div>
                        <input type="text" name="txtTitleTranslation" class="form-control"<?php if($arrRes['CORRECTION_FLAG'] == 0) { echo 'disabled'; } ?> value="<?php echo $arrRes['TITLE_TRANSLATE']; ?>">
                        <div ><?php echo fncHtmlSpecialChars(PUBLIC_TEXT_005_JPN).'/'.fncHtmlSpecialChars(PUBLIC_TEXT_005_ENG); ?> </div>
                        <textarea name="txtContentTranslation" class="form-control" rows="2" cols="50"<?php if($arrRes['CORRECTION_FLAG'] == 0) { echo 'disabled'; } ?>><?php echo $arrRes['CONTENTS_TRANSLATE']; ?></textarea>


					<input type="hidden" name="incidentCaseNo" value="<?php echo $arrRes['INCIDENT_CASE_NO']; ?>">

                    <div class="form-footer mar-tl-10">
                        <div class="in-line">
                            <button type="button" class="tbtn tbtn-defaut" name="btnPost" data-id="<?php echo $arrRes['INCIDENT_CASE_NO']; ?>"><?php echo $arrTextTranslate['PUBLIC_BUTTON_005']; ?></button>
                            <?php  if($arrRes['COMP_DATE'] == null && ! empty($intId)){?>
                            <button type="button" class="tbtn tbtn-defaut" name="btnDone" data-id="<?php echo $arrRes['INCIDENT_CASE_NO']; ?>"><?php echo $arrTextTranslate['PUBLIC_BUTTON_007']; ?></button>
                        	<?php }?>
                        </div>
                        <div class="in-line text-right" style="float: right">
                            <button type="button" class="tbtn-cancel tbtn-defaut " name="btnClose" id="close"><?php echo $arrTextTranslate['PUBLIC_BUTTON_003']; ?></button>
                        </div>
                    </div>
                </div>
             </form>
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
                $('input[name=txtTitleTranslation], textarea[name=txtContentTranslation]').prop('disabled', false);
                if(!flagShowMsgCheckTranslateManual && ($.trim($('input[name=txtTitleTranslation]').val()) != '' || $.trim($('textarea[name=txtContentTranslation]').val()) != '')) {
                    alert('<?php echo $arrTextTranslate['PUBLIC_MSG_020']; ?>');
                    flagShowMsgCheckTranslateManual = true;
                }
                flagCheckTranslateManual = true;

                $('.btnTranIncident').prop('disabled', true);
            } else {
                $('input[name=txtTitleTranslation], textarea[name=txtContentTranslation]').prop('disabled', true);
                $('.btnTranIncident').prop('disabled', false);
            }
        });


        $('button[name=btnTranslation]').off().on('click', function(e) {
            e.preventDefault();
            $('.error-edit').html('');
            setTimeout(function(){
            var arr = $('form[name=incident_case-edit]').serializeArray();
            arr.push({ name: 'action', value: 'translate' });
            if(!$('input[name=chkManualInput]').is(':checked')) {
                arr.push({ name: 'chkManualInput', value: 'off' });
                arr.push({ name: 'txtTitleTranslation', value: $('input[name=txtTitleTranslation]').val() });
                arr.push({ name: 'txtContentTranslation', value: $('textarea[name=txtContentTranslation]').val() });
            }
            $.ajax({
                url: 'incident_case_edit_proc.php',
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
    						alert('<?php echo $arrTextTranslate['PUBLIC_MSG_009'] ?>');
    						window.location.href="login.php";
    	                    return;
    					}
                        var res = getData(result);
                        if(typeof res != 'string') {
                            if(res['error'] != '') {
                                if(res['trans-error'] == 'error') {
                                    alert('<?php echo $arrTextTranslate['PUBLIC_MSG_010']; ?>');
                                } else {
                                    $('.error-edit').html(res['error']);
                                    $('#myModal').animate({ scrollTop: 0 }, '10');
                                }
                            }
                            if(res['success'] != '') {
                                $('input[name=txtTitleTranslation]').val(res['success']['titleTranslate']);
                                $('textarea[name=txtContentTranslation]').val(res['success']['contentTranslate']);
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

        $('button[name=btnPost]').off().on('click', function(e) {
            e.preventDefault();
            $('.error-edit').html('');
            setTimeout(function(){
            var id = $('button[name=btnPost]').attr('data-id');
            var arr = $('form[name=incident_case-edit]').serializeArray();
            arr.push({ name: 'action', value: (id == '') ? 'pre-insert' : 'pre-update' });
            arr.push({ name: 'id', value: id });
            arr.push({ name: 'dtStartDate', value: $('input[name=dtStartDate]').val() });
            if(!$('input[name=chkManualInput]').is(':checked')) {
                arr.push({ name: 'chkManualInput', value: 'off' });
                arr.push({ name: 'txtTitleTranslation', value: $('input[name=txtTitleTranslation]').val() });
                arr.push({ name: 'txtContentTranslation', value: $('textarea[name=txtContentTranslation]').val() });
            }

            var flagStop = false;
            var msgConfirm = '';
            var titleTranslate = '';
            var contentTranslate = '';

            $.ajax({
                url: 'incident_case_edit_proc.php',
                type: 'POST',
                data: arr,
                async: false,
                success: function(result) {
                    if(result != '') {
                    	if(result == 900){
    						alert('<?php echo $arrTextTranslate['PUBLIC_MSG_009'] ?>');
    						window.location.href="login.php";
    	                    return;
    					}
                        var res = getData(result);
                        if(typeof res != 'string') {
                            if(res['error'] != '') {
                                if(res['trans-error'] == 'error') {
                                    alert('<?php echo $arrTextTranslate['PUBLIC_MSG_010']; ?>');
                                } else {
                                    $('.error-edit').html(res['error']);
                                    $('#myModal').animate({ scrollTop: 0 }, '10');
                                }
                                flagStop = true;
                            }
                        }
                        if($.trim(result) == 'window.location.href="login.php";') {
                            window.location.href="login.php";
                            return;
                        }
                        if(res['success'] != '') {
                            titleTranslate = res['success']['titleTranslate'];
                            contentTranslate = res['success']['contentTranslate'];
                        }
                        msgConfirm = res['confirm'];
                    }
                },
                error: function(e) {
                    console.log(e);
                }
            });

            if(!flagStop) {
                if(msgConfirm != '' && confirm(msgConfirm)) {
                    var formData = new FormData();
                    var data = $('form[name=incident_case-edit]').serializeArray();
                    data.push({ name: 'action', value: (id == '') ? 'insert' : 'update' });
                    data.push({ name: 'id', value: id });
                    arr.push({ name: 'dtStartDate', value: $('input[name=dtStartDate]').val() });
                    if(!$('input[name=chkManualInput]').is(':checked')) {
                        data.push({ name: 'chkManualInput', value: 'off' });
                    }
                    data.push({ name: 'txtTitleTranslation', value: titleTranslate });
                    data.push({ name: 'txtContentTranslation', value: contentTranslate });
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

                    if(listMsg.length > 0) {
                        $.each(listMsg, function(i, e) {
                            $('.error-edit').append('<div>'+e+'</div>');
                        });
                        $('#myModal').animate({ scrollTop: 0 }, '10');
                        return false;
                    }

                    $.each(data, function(index, obj) {
                        formData.append(obj.name, obj.value);
                    });

                    var flagHasError = false;
                    $.ajax({
                        url: 'incident_case_edit_proc.php',
                        type: 'POST',
                        data: formData,
                        cache: false,
                        contentType: false,
                        processData: false,
                        async: false,
                        success: function(result) {
                            if(result != '') {
                            	if(result == 900){
            						alert('<?php echo $arrTextTranslate['PUBLIC_MSG_009'] ?>');
            						window.location.href="login.php";
            	                    return;
            					}
                                var res = getData(result);
                                if(res['error'] != '') {
                                    if(res['error'] == 'alreadyDelError') {
                                        alert('<?php echo $arrTextTranslate['INCIDENT_CASE_EDIT_MSG_001'] ?>');
                                        if('<?php echo $strScreenRef; ?>' == 'portal.php'){
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
                        var id = '<?php echo $arrRes['INCIDENT_CASE_NO'] ?>';
                        var key = '<?php echo $_POST['screen'] ?>';
                        if(id != '') {
                            var a = $("<button>")
                                    .attr('class', 'load-modal')
                                    .attr("href", 'incident_case_view.php')
                                    .attr("data-id", id)
                                    .attr("data-screen", key)
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

        $('button[name=btnDone]').off().on('click', function(e) {
            e.preventDefault();
            $('.error-edit').html('');
            setTimeout(function(){
            var id = $('button[name=btnDone]').attr('data-id');
            var arr = $('form[name=incident_case-edit]').serializeArray();
            var key = '<?php echo $_POST['screen'] ?>';
            if(confirm('<?php echo $arrTextTranslate['INCIDENT_CASE_EDIT_MSG_006']; ?>')) {
            	arr.push({name: 'X-CSRF-TOKEN', value: '<?php echo (isset($_POST['X-CSRF-TOKEN']) ? $_POST['X-CSRF-TOKEN'] : ''); ?>' });
            	arr.push({ name: 'id', value: id });
            	arr.push({ name: 'action', value: 'done' });
                $.ajax({
                    url: 'incident_case_edit_proc.php',
                    type: 'POST',
                    data: arr,
                    async: false,
                    success: function(result) {
                        if(result != '') {
                        	if(result == 900){
        						alert('<?php echo $arrTextTranslate['PUBLIC_MSG_009'] ?>');
        						window.location.href="login.php";
        	                    return;
        					}
                            var res = getData(result);

                            if($.trim(result) == 'window.location.href="login.php";') {
                                window.location.href="login.php";
                                return;
                            }

                            if(res['error'] != '') {
                                if(res['error'] == 'alreadyDelError'){
                                    alert('<?php echo $arrTextTranslate['INCIDENT_CASE_EDIT_MSG_001']; ?>');
                                    if('<?php echo $strScreenRef; ?>' == 'portal.php'){
                                        loadPortalClose();
                                        $('#myModal').modal('hide');
                                    }else{
                                        setTimeout(function() {
                                            window.location.reload();
                                        }, 300);
                                        $('#myModal').modal('hide');
                                    }
                                }else{
                            	    $('.error-edit').html(res['error']);
                                    $('#myModal').animate({ scrollTop: 0 }, '10');
                                }
                            }else{
                            	var a = $("<button>")
                        				.attr('class', 'load-modal')
                        				.attr("href", 'incident_case_view.php')
                        				.attr("data-id", id)
                        				.attr("data-screen", key)
                        				.appendTo("body");
                				a[0].click();
                				a.remove();
                				$('#myModal').modal('show');
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

    });


    $('button[name=btnClose]').off().on('click', function(e) {
        e.preventDefault();
        var id = '<?php echo $arrRes['INCIDENT_CASE_NO'] ?>';
        var key = '<?php echo $_POST['screen'] ?>';
        if(id != '') {
            var a = $("<button>")
                    .attr('class', 'load-modal')
                    .attr("href", 'incident_case_view.php')
                    .attr("data-id", id)
                    .attr("data-screen", key)
                    .appendTo("body");
            a[0].click();
            a.remove();
            $('#myModal').modal('show');
        } else {
            loadPortalClose();
            $('#myModal').modal('hide');
        }
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