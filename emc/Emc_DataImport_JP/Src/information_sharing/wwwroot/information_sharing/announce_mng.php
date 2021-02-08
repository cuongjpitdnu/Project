<?php
    /*
     * @announce_mng.php
     *
     * @create 2020/03/13 AKB Chien
     * @update
     */
    require_once('common/common.php');
    require_once('common/session_check.php');

    header('Content-type: text/html; charset=utf-8');
    header('X-FRAME-OPTIONS: DENY');

    define('DISPLAY_TITLE', 'お知らせ管理画面');

    // check connection
    if(fncConnectDB() == false) {
        $_SESSION['LOGIN_ERROR'] = 'DB接続に失敗しました。';
        header('Location: login.php');
        exit;
    }

    // Check if the user logged in or not
    if(!isset($_SESSION['LOGINUSER_INFO'])) {
        $strShow = '<script>alert("'.PUBLIC_MSG_008_JPN.' / '.PUBLIC_MSG_008_ENG.'"); ';
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
        // init session of screen
        $_SESSION['ANNOUNCE_MNG_ERROR'] = array();
        $_SESSION['ANNOUNCE_MNG_SEARCH_TITLE'] = null;
        $_SESSION['ANNOUNCE_MNG_SEARCH_DATE_START'] = null;
        $_SESSION['ANNOUNCE_MNG_SEARCH_DATE_END'] = null;
        $_SESSION['ANNOUNCE_MNG_SEARCH_COMP_CHECK'] = 0;
        $_SESSION['ANNOUNCE_MNG_PAGE'] = 1;

        // <初期画面表示時>
        $strLog = DISPLAY_TITLE.'　画面表示(ユーザID = '.$objLoginUserInfo->strUserID.') ';
        $strLog .= isset($_SERVER['HTTP_REFERER']) ? $_SERVER['HTTP_REFERER'] : null;
        fncWriteLog(LogLevel['Info'], LogPattern['View'], $strLog);
    }

    // init session error of screen
    $arrLoginError = (isset($_SESSION['ANNOUNCE_MNG_ERROR']))
                        ? $_SESSION['ANNOUNCE_MNG_ERROR'] : array();

    $arrTitleMsg =  array(
        'PUBLIC_MSG_009',
        'ANNOUNCE_MNG_TEXT_001',
        'ANNOUNCE_MNG_TEXT_002',
        'ANNOUNCE_MNG_TEXT_003',
        'ANNOUNCE_MNG_TEXT_004',
        'PUBLIC_BUTTON_006',
        // get data grid fail
        'PUBLIC_MSG_001',
        // msg confirm when click btnDone
        'ANNOUNCE_MNG_MSG_001',
        'ANNOUNCE_MNG_MSG_002',

        'PUBLIC_MSG_003',
        'PUBLIC_MSG_004',
        'PUBLIC_MSG_006',
        'PUBLIC_MSG_007',
        // 2020/03/26 AKB Chien - start - update document 2020/03/26
        'PUBLIC_MSG_049',
        // 2020/03/26 AKB Chien - end - update document 2020/03/26
        'ANNOUNCE_MNG_MSG_005'
    );

    // get list text(header, title, msg) with languague_type of user logged
    $arrTextTranslate = getListTextTranslate($arrTitleMsg, $intLanguageType);

    // 2020/03/26 AKB Chien - start - update document 2020/03/26
    // GET通信にて遷移してきた場合、以下のメッセージをアラート表示し、遷移元画面に戻す。
    fncGetRequestCheck($arrTextTranslate);
    // 2020/03/26 AKB Chien - end - update document 2020/03/26

    // 2020/04/01 AKB Chien - start - update document 2020/04/01
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
    // 2020/04/01 AKB Chien - end - update document 2020/04/01
?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta name="csrf-token" content="<?php
        echo (isset($strCsrf) ? $strCsrf : '');
    ?>">
    <meta charset="UTF-8">
    <title><?php echo $arrTextTranslate['ANNOUNCE_MNG_TEXT_001']; ?></title>
    <link rel="stylesheet" type="text/css" href="css/style.css">
    <link rel="stylesheet" type="text/css" href="css/table.css">
    <link type="text/css"
        href="css/smoothness/jquery-ui-1.10.4.custom.css"
        rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.min.3.14.js"></script>
    <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.js"></script>
    <script type="text/javascript" src="js/jquery.ui.datepicker-ja.js"></script>

    <link rel="stylesheet" href="css/bootstrap.min.css">
    <script src="js/bootstrap.min.js"></script>
    <script src="js/common.js"></script>
    <?php
        // permission 各種メニュー権限[intMenuPerm]
        if($objLoginUserInfo->intMenuPerm == 0) { ?>
        <script type="text/javascript">
            alert('<?php echo $arrTextTranslate['PUBLIC_MSG_009']; ?>');
            window.location.href = 'login.php';
        </script>
    <?php exit; } ?>
</head>
<body>
    <div class="main-content">
        <div class="main-form">
            <div class="form-title"
                ><?php echo $arrTextTranslate['ANNOUNCE_MNG_TEXT_001']; ?>
            </div>
            <font color="red" class="error">
                <?php
                    // if has error msg -> show
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
                        <div class="in-line ">
                            <div class="label-80"><?php
                                echo $arrTextTranslate['ANNOUNCE_MNG_TEXT_002']; ?></div>
                            <input type="text" class="t-input" id="txtTitle" name="txtTitle"
                                value="<?php
                                    $strValue = '';
                                    if(isset($_SESSION['ANNOUNCE_MNG_SEARCH_TITLE'])) {
                                        $strValue = $_SESSION['ANNOUNCE_MNG_SEARCH_TITLE'];
                                    }
                                    echo $strValue;
                                ?>" />
                        </div>
                        <div class="in-line ">
                            <div class="label-80"><?php
                                echo $arrTextTranslate['ANNOUNCE_MNG_TEXT_003']; ?></div>
                            <input type="text" class="t-input" id="txtDateStart" name="txtDateStart"
                                value="<?php
                                    $strValue = '';
                                    if(isset($_SESSION['ANNOUNCE_MNG_SEARCH_DATE_START'])) {
                                        $strValue = $_SESSION['ANNOUNCE_MNG_SEARCH_DATE_START'];
                                    }
                                    echo $strValue;
                                ?>" /> ~
                            <input type="text" class="t-input" id="txtDataEnd" name="txtDataEnd"
                                value="<?php
                                    $strValue = '';
                                    if(isset($_SESSION['ANNOUNCE_MNG_SEARCH_DATE_END'])) {
                                        $strValue = $_SESSION['ANNOUNCE_MNG_SEARCH_DATE_END'];
                                    }
                                    echo $strValue;
                                ?>" />
                        </div>
                        <div class="in-line ">
                            <label class="t-container"><?php
                                echo $arrTextTranslate['ANNOUNCE_MNG_TEXT_004']; ?>
                                <input type="checkbox" id="chkDone" name="chkDone"
                                    <?php
                                        $strChecked = '';
                                        if(isset($_SESSION['ANNOUNCE_MNG_SEARCH_COMP_CHECK'])) {
                                            if($_SESSION['ANNOUNCE_MNG_SEARCH_COMP_CHECK'] == 1) {
                                                $strChecked = 'checked';
                                            }
                                        }
                                        echo $strChecked;
                                    ?>
                                />
                                <span class="checkmark"></span>
                            </label>
                        </div>
                        <div class="in-line col-right">
                            <input type="button" class="tbtn tbtn-defaut btn-search-announce"
                                value="<?php echo $arrTextTranslate['PUBLIC_BUTTON_006']; ?>" />
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

<script>
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

    var ajaxUrl = '';
    var msgReduct = '';
    var pageWasRefreshed = '<?php echo $_SESSION["VISITS"]; ?>';
    $(document).ready(function() {
        if(pageWasRefreshed > 1) {
            loadData(1, 0);
        } else {
            loadData(0, 0);
        }

        $('.btn-search-announce').on('click', function(e) {
            $('.error').html('');
            e.preventDefault();
            loadData(1, 1);
        });

        $(document).on('click', '.btnDone', function(e) {
            e.preventDefault();
            $('.error').html('');
            if(confirm('<?php echo $arrTextTranslate['ANNOUNCE_MNG_MSG_001']; ?>')) {
                var arr = [
                    { name: 'X-CSRF-TOKEN', value: '<?php
                        echo (isset($strCsrf) ? $strCsrf : ''); ?>'},
                    { name: 'announce_no', value: $(this).attr('data-id') },
                    { name: 'mode', value: 1 }
                ];
                $.ajax({
                    url: 'announce_mng_proc.php',
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
                                echo $arrTextTranslate['PUBLIC_MSG_001']; ?></div>');
                        }
                        if(result == 0) {
                            $('.error').html('<div><?php
                                echo $arrTextTranslate['PUBLIC_MSG_003']; ?></div>');
                        }
                        if(result == 3){
                            $('.error').html('<div><?php
                                echo $arrTextTranslate['ANNOUNCE_MNG_MSG_005']; ?></div>');    
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
                echo $arrTextTranslate['ANNOUNCE_MNG_MSG_002']; ?>')) {
                var arr = [
                    { name: 'X-CSRF-TOKEN', value: '<?php
                        echo (isset($strCsrf) ? $strCsrf : ''); ?>'},
                    { name: 'announce_no', value: $(this).attr('data-id') },
                    { name: 'mode', value: 2 }
                ];
                $.ajax({
                    url: 'announce_mng_proc.php',
                    type: 'POST',
                    async: false,
                    data: arr,
                    success: function(result) {
                        if($.trim(result) == 'window.location.href="login.php";') {
                            window.location.href="login.php";
                            return;
                        }
                        if(result == 0) {
                            $('.error').html('<div><?php
                                echo $arrTextTranslate['PUBLIC_MSG_004']; ?></div>');
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
            $('[name=mode]').val(3);
            var data = $('.search-form').serialize();
            $('[name=searchData]').val(data);
            $("#formCSV").submit();
        });

        $(document).on('click', '.btnDataReduct', function(e) {
            e.preventDefault();
            $('.error').html('');
            checkNumDataReduct();
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
    });

    function loadData(condition, originalSearch) {
        var data = $('.search-form').serializeArray();
        data.push({ name: 'event', value: condition });
        data.push({ name: 'originalSearch', value: originalSearch});
        if(!$('input[name=chkDone]').is(':checked')) {
            data.push({ name: 'chkDone', value: 0 });
        }
        $.ajax({
            url: 'announce_mng_proc.php',
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
                        echo $arrTextTranslate['PUBLIC_MSG_001']; ?></div>');
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
            url: 'announce_mng_proc.php',
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
                        echo $arrTextTranslate['PUBLIC_MSG_007']; ?></div>');
                    return false;
                } else {
                    if(result > 0) {
                        lockScreen();
                        if(confirm('<?php echo $arrTextTranslate['PUBLIC_MSG_006']; ?>'.replace("%0%", result))) {
                            $('[name=mode]').val(4);
                            var data = $('.search-form').serialize();
                            $('[name=searchData]').val(data);
                            $.ajax({
                                url: './announce_mng_proc.php',
                                type:'post',
                                data: $('#formCSV').serializeArray(),
                                success:function(data) {
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