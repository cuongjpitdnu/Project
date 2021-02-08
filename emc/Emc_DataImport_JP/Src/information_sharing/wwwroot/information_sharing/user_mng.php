<?php
    /*
     * @user_mng.php
     *
     * @create 2020/03/13 AKB Chien
     * @update
     */
    require_once('common/common.php');
    require_once('common/session_check.php');

    header('Content-type: text/html; charset=utf-8');
    header('X-FRAME-OPTIONS: DENY');

    define('DISPLAY_TITLE', 'ユーザ管理画面');

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
        $_SESSION['USER_MNG_ERROR'] = array();
        $_SESSION['USER_MNG_SEARCH_USERID'] = null;
        $_SESSION['USER_MNG_SEARCH_COMPANY'] = null;
        $_SESSION['USER_MNG_SEARCH_GROUP'] = null;
        $_SESSION['USER_MNG_SEARCH_USER_NAME'] = null;
        $_SESSION['USER_MNG_PAGE'] = 1;

        // write log
        $strLog = DISPLAY_TITLE.'　画面表示(ユーザID = '.$objLoginUserInfo->strUserID.') ';
        $strLog .= isset($_SERVER['HTTP_REFERER']) ? $_SERVER['HTTP_REFERER'] : null;
        fncWriteLog(LogLevel['Info'], LogPattern['View'], $strLog);
    }

    $arrTitleMsg =  array(
        // msg do not have permission
        'PUBLIC_MSG_009',
        'USER_MNG_TEXT_001',
        'USER_MNG_TEXT_002',
        'USER_MNG_TEXT_003',
        'USER_MNG_TEXT_004',
        'USER_MNG_TEXT_005',
        'PUBLIC_BUTTON_006',
        // get data grid = false
        'PUBLIC_MSG_001',

        // 2020/03/30 AKB Chien - start - update document 2020/03/30
        'PUBLIC_MSG_049'
        // 2020/03/30 AKB Chien - end - update document 2020/03/30
    );

    // get list text(header, title, msg) with languague_type of user logged
    $arrTxtTrans = getListTextTranslate($arrTitleMsg, $intLanguageType);

    //「グループ」セレクトボックスを作成
    $arrMGroup = getMGroup($intLanguageType);

    /**
     * get data m_group
     *
     * @create 2020/03/13 AKB Chien
     * @update
     * @param integer $intLang
     * @return array $arrResult
     */
    function getMGroup($intLang) {
        $strSuffixes = ($intLang == 0) ? '_JPN' : '_ENG';
        $strGroupName = 'group_name'.$strSuffixes;

        $strSQL = ' SELECT '
                . '     group_no, '
                . $strGroupName.' AS group_name '
                . ' FROM '
                . '     m_group '
                . ' ORDER BY sort_no ASC';

        try {
            $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);

            // write log
            $strLogSql = $strSQL;
            fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], DISPLAY_TITLE.' '.$strLogSql);

            // execute SQL and get data
            $objStmt->execute();
            $arrResult = $objStmt->fetchAll(PDO::FETCH_ASSOC);
            return $arrResult;
        } catch (\Exception $e) {
            // write log
            fncWriteLog(LogLevel['Error'], LogPattern['Error'], $e->getMessage());
            return 0;
        }
    }

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

    $arrLoginError = (isset($_SESSION['USER_MNG_ERROR']))
                        ? $_SESSION['USER_MNG_ERROR'] : array();
?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta name="csrf-token" content="<?php
        echo (isset($strCsrf) ? $strCsrf : ''); ?>">
    <meta charset="UTF-8">
    <title><?php echo $arrTxtTrans['USER_MNG_TEXT_001']; ?></title>
    <link rel="stylesheet" type="text/css" href="css/style.css">
    <link rel="stylesheet" type="text/css" href="css/table.css">

    <link rel="stylesheet" href="css/bootstrap.min.css">
    <link type="text/css"
        href="css/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.min.js"></script>
    <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.js"></script>
    <script type="text/javascript" src="js/jquery.ui.datepicker-ja.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="js/common.js"></script>
    <?php
        // permission 各種メニュー権限[intMenuPerm]
        if($objLoginUserInfo->intMenuPerm == 0) {
    ?>
        <script type="text/javascript">
            alert('<?php echo $arrTxtTrans['PUBLIC_MSG_009']; ?>');
            window.location.href = 'login.php';
        </script>
    <?php exit; } ?>
    <style>

    <?php if($intLanguageType == 1) { ?>
        .label-105 { width: 105px; text-align: right; }
    <?php } ?>
    </style>
</head>
<body>
    <div class="main-content">
        <div class="main-form">
            <div class="form-title"><?php
                echo $arrTxtTrans['USER_MNG_TEXT_001'];
            ?></div>
            <font color="red" class="error">
                <?php
                    // if has error msg
                    if(count($arrLoginError) > 0) {
                        $strHtml = '';
                        foreach($arrLoginError as $value) {
                            $strHtml .= '<div>'.fncHtmlSpecialChars($value).'</div>';
                        }
                        echo $strHtml;
                    }
                    // if get info group has error msg
                    if($arrMGroup == 0) {
                        echo '<div>'.$arrTxtTrans['PUBLIC_MSG_001'].'</div>';
                    }
                ?>
            </font>
            <div class="form-body">
                <form class="search-form" name="searchForm" action="" method="post" autocomplete="off">
                    <input type="hidden" name="loadList" value="1">
                    <input type="hidden" name="currentPage" value="1">
                    <div class="cont-title">
                        <div class=" in-line">
                            <div class="label-80"><?php
                                echo $arrTxtTrans['USER_MNG_TEXT_002'];
                            ?></div>
                            <input type="text" class="t-input t-input-125" name="userId"
                                value="<?php
                                    $strValue = '';
                                    if(isset($_SESSION['USER_MNG_SEARCH_USERID'])) {
                                        $strValue = $_SESSION['USER_MNG_SEARCH_USERID'];
                                    }
                                    echo $strValue;
                                ?>" />
                        </div>
                        <div class="in-line">
                            <div class="label-80 label-105"><?php
                                echo $arrTxtTrans['USER_MNG_TEXT_003'];
                            ?></div>
                            <input type="text" class="t-input t-input-125" name="companyName"
                                value="<?php
                                    $strValue = '';
                                    if(isset($_SESSION['USER_MNG_SEARCH_COMPANY'])) {
                                        $strValue = $_SESSION['USER_MNG_SEARCH_COMPANY'];
                                    }
                                    echo $strValue;
                                ?>" />
                        </div>
                        <div class="in-line">
                            <div class="label-80"><?php
                                echo $arrTxtTrans['USER_MNG_TEXT_004'];
                            ?></div>
                            <div class="select-container">
                                <select class="t-select select-150" name="groupNo">
                                    <?php
                                        $strHtml = '';
                                        $strHtml .= '<option value="">All</option>';
                                        if($arrMGroup != 0 && count($arrMGroup) > 0) {
                                            foreach ($arrMGroup as $value) {
                                                $strSelected = '';
                                                if(isset($_SESSION['USER_MNG_SEARCH_GROUP'])) {
                                                    if($_SESSION['USER_MNG_SEARCH_GROUP'] == $value['group_no']) {
                                                        $strSelected = 'selected';
                                                    } else {
                                                        $strSelected = '';
                                                    }
                                                }
                                                $strHtml .= '<option value="'.$value['group_no'].'" '.$strSelected.'>';
                                                $strHtml .= fncHtmlSpecialChars($value['group_name']);
                                                $strHtml .= '</option>';
                                            }
                                        }
                                        echo $strHtml;
                                    ?>
                                </select>
                            </div>
                        </div>
                        <div class="in-line">
                            <div class="label-80 label-105"><?php
                                echo $arrTxtTrans['USER_MNG_TEXT_005'];
                            ?></div>
                            <input type="text" class="t-input t-input-125" name="userName"
                                value="<?php
                                    $strValue = '';
                                    if(isset($_SESSION['USER_MNG_SEARCH_USER_NAME'])) {
                                        $strValue = $_SESSION['USER_MNG_SEARCH_USER_NAME'];
                                    }
                                    echo $strValue;
                                ?>"
                            />
                        </div>
                        <div class="in-line col-right">
                            <input type="button" class="tbtn tbtn-defaut btn-search-user"
                                value="<?php echo $arrTxtTrans['PUBLIC_BUTTON_006']; ?>" />
                        </div>
                    </div>
                </form>

                <div class="ajax-content"></div>
            </div>
        </div>
    </div>

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
        $(document).ready(function() {
            if(pageWasRefreshed > 1) {
                loadData(1, 0);
            } else {
                loadData(0, 0);
            }

            $('.btn-search-user').on('click', function(e) {
                $('.error').html('');
                e.preventDefault();
	        	loadData(1, 1);
            });

            $(document).on('click', '.btnOutput', function(e) {
                e.preventDefault();
                $('.error').html('');
                $('[name=mode]').val(1);
                var data = $('.search-form').serialize();
                $('[name=searchData]').val(data);
                $("#formCSV").submit();
            });
        });

        function loadData(condition, originalSearch) {
            var data = $('.search-form').serializeArray();
            data.push({ name: 'event', value: condition });
            data.push({ name: 'originalSearch', value: originalSearch});
            $.ajax({
                url: 'user_mng_proc.php',
                type: 'POST',
                async: false,
                data: data,
                success: function(result) {
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
    </script>
</body>
</html>
