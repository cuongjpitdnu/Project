<?php
    /*
     * @incident_case_mng.php
     *
     * @create 2020/04/07 AKB Chien
     * @update
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
    fncSessionTimeOutCheck();

    // ログインユーザ情報を取得
    $objLoginUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);

    $intLanguageType = $objLoginUserInfo->intLanguageType;

    // check page first time or refresh
    if (!isset($_SESSION["VISITS"])) {
        $_SESSION["VISITS"] = 0;
    }
    $_SESSION["VISITS"] = $_SESSION["VISITS"] + 1;

    if ($_SESSION["VISITS"] > 1) {
        //you refreshed the page!
    } else {
        $_SESSION['INCIDENT_MNG_ERROR'] = array();
        $_SESSION['INCIDENT_CASE_MNG_SEARCH_TITLE'] = null;
        $_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_START'] = null;
        $_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_END'] = null;
        $_SESSION['INCIDENT_CASE_MNG_SEARCH_COMP_CHECK'] = 0;
        $_SESSION['INCIDENT_CASE_MNG_PAGE'] = 1;

        // write log
        $strLog = DISPLAY_TITLE.'　画面表示(ユーザID = '.$objLoginUserInfo->strUserID.') ';
        $strLog .= isset($_SERVER['HTTP_REFERER']) ? $_SERVER['HTTP_REFERER'] : null;
        fncWriteLog(LogLevel['Info'], LogPattern['View'], $strLog);
    }

    $arrLoginError = (isset($_SESSION['INCIDENT_MNG_ERROR']))
                        ? $_SESSION['INCIDENT_MNG_ERROR'] : array();

    $arrTitleMsg =  array(
        'PUBLIC_MSG_001',
        'INCIDENT_CASE_MNG_TEXT_001',
        'INCIDENT_CASE_MNG_TEXT_002',
        'INCIDENT_CASE_MNG_TEXT_003',
        'INCIDENT_CASE_MNG_TEXT_004',
        'INCIDENT_CASE_MNG_TEXT_005',
        'INCIDENT_CASE_MNG_TEXT_006',
        'INCIDENT_CASE_MNG_TEXT_007',
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
        'PUBLIC_MSG_007',
        // 2020/03/26 AKB Chien - start - update document 2020/03/26
        'PUBLIC_MSG_049',
        'PUBLIC_MSG_009',
        // 2020/03/26 AKB Chien - end - update document 2020/03/26

        //2020/04/23 T.Masuda 既に削除されていた場合のメッセージ
        'INCIDENT_CASE_MNG_MSG_007',
        //2020/04/23 T.Masuda

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
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta name="csrf-token" content="<?php
        echo (isset($strCsrf) ? $strCsrf : ''); ?>">
    <meta charset="UTF-8">
    <title><?php echo $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_001']; ?></title>
    <link rel="stylesheet" type="text/css" href="css/style.css">
    <link rel="stylesheet" type="text/css" href="css/table.css">
    <link type="text/css"
        href="css/smoothness/jquery-ui-1.10.4.custom.css"
        rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.min.js"></script>
    <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.js"></script>
    <script type="text/javascript" src="js/jquery.ui.datepicker-ja.js"></script>

    <link rel="stylesheet" href="css/bootstrap.min.css">
    <script src="js/bootstrap.min.js"></script>
    <script src="js/common.js"></script>
    <?php
        // permission 各種メニュー権限[intMenuPerm]
        if($objLoginUserInfo->intMenuPerm == 0) { ?>
        <script type="text/javascript">
            alert('<?php echo $arrTxtTrans['PUBLIC_MSG_009']; ?>');
            window.location.href = 'login.php';
        </script>
    <?php exit; } ?>
    <style>
    <?php if($intLanguageType == 1) { ?>
        .btnDone { padding: 0 12px; }
        .btn-95 { width: 95px !important; }
    <?php } ?>
    </style>
</head>
<body>
<div class="main-content">
    <div class="main-form">
        <div class="form-title"><?php
            echo $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_001'];
        ?></div>
        <font color="red" class="error">
            <?php
                if(count($arrLoginError) > 0) {
                    $html = '';
                    foreach($arrLoginError as $value) {
                        $html .= '<div>'.$value.'</div>';
                    }
                    echo $html;
                }
            ?>
        </font>
        <div class="form-body">
            <form class="search-form" name="search-form" method="POST"  autocomplete="off">
                <input type="hidden" name="loadList" value="1">
                <input type="hidden" name="currentPage" value="1">
                <div class="cont-title">
                    <div class=" in-line">
                        <div class="label-80"><?php
                            echo $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_002']; ?></div>
                        <input type="text" class="t-input" name="txtTitle" id="txtTitle"
                            value="<?php
                                $strValue = '';
                                if(isset($_SESSION['INCIDENT_CASE_MNG_SEARCH_TITLE'])) {
                                    $strValue = $_SESSION['INCIDENT_CASE_MNG_SEARCH_TITLE'];
                                }
                                echo $strValue;
                            ?>" />
                    </div>
                    <div class="in-line">
                        <div class="label-80">
                            <?php echo $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_003']; ?>
                        </div>
                        <input type="text" class="t-input" name="txtDateStart" id="txtDateStart"
                            value="<?php
                                $strValue = '';
                                if(isset($_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_START'])) {
                                    $strValue = $_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_START'];
                                }
                                echo $strValue;
                            ?>" /> ~
                        <input type="text" class="t-input" name="txtDataEnd" id="txtDataEnd"
                            value="<?php
                                $strValue = '';
                                if(isset($_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_END'])) {
                                    $strValue = $_SESSION['INCIDENT_CASE_MNG_SEARCH_DATE_END'];
                                }
                                echo $strValue;
                            ?>" />
                    </div>
                    <div class="in-line">
                        <label class="t-container"><?php
                            echo $arrTxtTrans['INCIDENT_CASE_MNG_TEXT_004']; ?>
                            <input type="checkbox" name="chkDone" id="chkDone"
                            <?php
                                $strChecked = '';
                                if(isset($_SESSION['INCIDENT_CASE_MNG_SEARCH_COMP_CHECK'])) {
                                    if($_SESSION['INCIDENT_CASE_MNG_SEARCH_COMP_CHECK'] == 1) {
                                        $strChecked = 'checked';
                                    }
                                }
                                echo $strChecked;
                            ?> />
                            <span class="checkmark"></span>
                        </label>
                    </div>
                    <div class="in-line col-right">
                        <input type="button" class="tbtn tbtn-defaut btn-search-request"
                            value="<?php echo $arrTxtTrans['PUBLIC_BUTTON_006']; ?>" />
                    </div>
                </div>
            </form>

            <div class="ajax-content"></div>
        </div>
    </div>
</div>
</body>

<!-- Modal HTML -->
<div id="myModal" class="modal fade">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div id="modal-body">

            </div>
        </div>
    </div>
</div>

<script type="text/javascript">
    var ajaxUrl = '';
    var pageWasRefreshed = '<?php echo $_SESSION["VISITS"]; ?>';
    $(function() {
        $('#txtDateStart').datepicker({
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            dateFormat: 'yy/mm/dd'
        });

        $('#txtDataEnd').datepicker({
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            dateFormat: 'yy/mm/dd'
        });
    });

    $(document).ready(function() {
        if(pageWasRefreshed > 1) {
            loadData(1, 0);
        } else {
            loadData(0, 0);
        }

        $('.btn-search-request').on('click', function(e) {
            $('.error').html('');
            e.preventDefault();
            loadData(1, 1);
        });

        $('#chkDone').on('change', function(e) {
            $('.error').html('');
            if($(this).is(':checked')) {
                $('.btnDataReduct').prop('disabled', false);
            } else {
                $('.btnDataReduct').prop('disabled', true);
            }
            loadData(1, 1);
        });

        $(document).on('click', '.btnDone', function(e) {
            e.preventDefault();
            $('.error').html('');
            if(confirm('<?php
                echo $arrTxtTrans['INCIDENT_CASE_MNG_MSG_003']; ?>')) {
                var arr = [
                    { name: 'X-CSRF-TOKEN', value: '<?php
                        echo (isset($strCsrf) ? $strCsrf : ''); ?>'},
                    { name: 'incident_case_no', value: $(this).attr('data-id') },
                    { name: 'mode', value: 1 }
                ];
                $.ajax({
                    url: 'incident_case_mng_proc.php',
                    type: 'POST',
                    async: false,
                    data: arr,
                    success: function(result) {
                        if($.trim(result) == 'window.location.href="login.php";') {
                            window.location.href="login.php";
                            return;
                        }
                        if(result == 2) {
                            $('.error').html('<div><?php
                                echo $arrTxtTrans['PUBLIC_MSG_001']; ?></div>');
                        }
                        if(result == 0) {
                            $('.error').html('<div><?php
                                echo $arrTxtTrans['PUBLIC_MSG_003']; ?></div>');
                        }

                        if(result == 3) {
                            $('.error').html('<div><?php
                                echo $arrTxtTrans['INCIDENT_CASE_MNG_MSG_007']; ?></div>');
                        }
                        loadData(1, 0);
                    },
                    error: function(e) {
                        console.log(e);
                    }
                });
            }
        });

        $(document).on('click', '.btnCancel', function(e) {
            e.preventDefault();
            $('.error').html('');
            if(confirm('<?php
                echo $arrTxtTrans['INCIDENT_CASE_MNG_MSG_005']; ?>')) {
                var arr = [
                    { name: 'X-CSRF-TOKEN', value: '<?php
                        echo (isset($strCsrf) ? $strCsrf : ''); ?>'},
                    { name: 'incident_case_no', value: $(this).attr('data-id') },
                    { name: 'mode', value: 3 }
                ];
                $.ajax({
                    url: 'incident_case_mng_proc.php',
                    type: 'POST',
                    async: false,
                    data: arr,
                    success: function(result) {
                        if($.trim(result) == 'window.location.href="login.php";') {
                            window.location.href="login.php";
                            return;
                        }
                        if(result == 2) {
                            $('.error').html('<div><?php
                                echo $arrTxtTrans['INCIDENT_CASE_MNG_MSG_006']; ?></div>');
                        }
                        if(result == 0) {
                            $('.error').html('<div><?php
                                echo $arrTxtTrans['PUBLIC_MSG_003']; ?></div>');
                        }
                        if(result == 3) {
                            $('.error').html('<div><?php
                                echo $arrTxtTrans['INCIDENT_CASE_MNG_MSG_007']; ?></div>');
                        }

                        loadData(1, 0);
                    },
                    error: function(e) {
                        console.log(e);
                    }
                });
            }
        });

        $(document).on('click', '.btnDelete', function(e) {
            e.preventDefault();
            $('.error').html('');
            if(confirm('<?php
                echo $arrTxtTrans['INCIDENT_CASE_MNG_MSG_004']; ?>')) {
                var arr = [
                    { name: 'X-CSRF-TOKEN', value: '<?php
                        echo (isset($strCsrf) ? $strCsrf : ''); ?>'},
                    { name: 'incident_case_no', value: $(this).attr('data-id') },
                    { name: 'mode', value: 2 }
                ];
                $.ajax({
                    url: 'incident_case_mng_proc.php',
                    type: 'POST',
                    async: false,
                    data: arr,
                    success: function(result) {
                        if($.trim(result) == 'window.location.href="login.php";') {
                            window.location.href="login.php";
                            return;
                        }
                        if(result == 0 || result == 2) {
                            $('.error').html('<div><?php
                                echo $arrTxtTrans['PUBLIC_MSG_004']; ?></div>');
                        }
                        loadData(1, 0);
                    },
                    error: function(e) {
                        console.log(e);
                    }
                });
            }
        });

        $(document).on('click', '.btnOutput', function(e) {
            e.preventDefault();
            $('.error').html('');
            $('[name=mode]').val(4);
            $('[name=incident_case_no]').val($(this).attr('data-id'));
            var data = $('.search-form').serialize();
            $('[name=searchData]').val(data);
            $("#formCSV").submit();
        });

        $(document).on('click', '.btnDataReduct', function(e) {
            e.preventDefault();
            $('.error').html('');
            checkNumDataReduct();
        });
    });

    function loadData(condition, originalSearch) {
        var data = $('.search-form').serializeArray();
        data.push({ name: 'event', value: condition });
        data.push({ name: 'originalSearch', value: originalSearch});
        if(!$('input[name=chkDone]').is(':checked')) {
            data.push({ name: 'chkDone', value: 0 });
        }
        $.ajax({
            url: 'incident_case_mng_proc.php',
            type: 'POST',
            async: false,
            data: data,
            success: function(result) {
                if($.trim(result) == 'window.location.href="login.php";') {
                    window.location.href="login.php";
                    return;
                }
                if(result == 0) {
                    $('.error').html('<div><?php
                        echo $arrTxtTrans['PUBLIC_MSG_001']; ?></div>');
                }
                $(".ajax-content").html(result);
            },
            error: function(e) {
                console.log(e);
            }
        });
    }

    function checkNumDataReduct() {
        var arr = [
            { name: 'X-CSRF-TOKEN', value: '<?php
                echo (isset($strCsrf) ? $strCsrf : ''); ?>' },
            { name: 'action', value: 'checkNumDataReduct' }
        ];
        $.ajax({
            url: 'incident_case_mng_proc.php',
            type: 'POST',
            async: false,
            data: arr,
            success: function(result) {
                if($.trim(result) == 'window.location.href="login.php";') {
                    window.location.href="login.php";
                    return;
                }
                if(result == -1) {
                    $('.error').html('<div><?php
                        echo $arrTxtTrans['PUBLIC_MSG_007']; ?></div>');
                    return false;
                } else {
                    if(result > 0) {
                    	lockScreen();
                        if(confirm('<?php echo $arrTxtTrans['PUBLIC_MSG_006']; ?>'.replace("%0%", result))) {
                            $('[name=mode]').val(5);
                            var data = $('.search-form').serialize();
                            $('[name=searchData]').val(data);
                            $.ajax({
                                url: './incident_case_mng_proc.php',
                                type:'post',
                                async: false,
                                data: $('#formCSV').serializeArray(),
                                success:function(data) {
                                    console.log(data);
                                    if(data != '') {
                                        $('.error').html('<div>'+data+'</div>');
                                    }
                                    unlockScreen();
                                    loadData(1, 0);
                                }
                            });
                        }else{
                        	unlockScreen();
                        }
                    }
                }
            },
            error: function(e) {
                console.log(e);
            }
        });
    }
    
    function lockScreen() {
		var element = document.createElement('div');
		element.id = "screenLock";
		element.style.height = '100%'; 
		element.style.left = '0px'; 
		element.style.position = 'fixed';
		element.style.top = '0px';
		element.style.width = '100%';
		element.style.zIndex = '9999';
		element.style.backgroundColor = 'gray';
		element.style.opacity = '0.3';
		
		var objBody = document.getElementsByTagName("body").item(0); 
		objBody.appendChild(element);
	}
	
	
	function unlockScreen() {
		$("#screenLock").remove();
	}
</script>
</html>