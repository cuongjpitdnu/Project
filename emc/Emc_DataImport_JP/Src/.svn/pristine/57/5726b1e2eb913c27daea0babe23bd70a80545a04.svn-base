<?php
    /*
     * @company_mng.php
     *
     * @create 2020/03/13 AKB Thang
     */
    require_once('common/common.php');
    require_once('common/session_check.php');

    header('Content-type: text/html; charset=utf-8');
    header('X-FRAME-OPTIONS: DENY');
    //failed to connect database
    if(fncConnectDB() == false) {
        $_SESSION['LOGIN_ERROR'] = 'DB接続に失敗しました。';
        header('Location: login.php');
        exit;
    }
    // if not login, rediect to login page
    if(!isset($_SESSION['LOGINUSER_INFO'])) {
        echo "
        <script>
            alert('".PUBLIC_MSG_008_JPN . "/" . PUBLIC_MSG_008_ENG."');
            window.location.href = 'login.php';
        </script>
        ";
        exit();
    }
    //get object user login
    $objUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);
    $arrTitle =  array(
        'COMPANY_MNG_TEXT_001',
        'COMPANY_MNG_TEXT_002',
        'COMPANY_MNG_TEXT_003',
        'COMPANY_MNG_TEXT_004',
        'COMPANY_MNG_TEXT_005',
        'PUBLIC_BUTTON_006',
        'COMPANY_MNG_MSG_001',
        'PUBLIC_MSG_001',
        'PUBLIC_MSG_009',
        'PUBLIC_MSG_049'
    );
    // get list text(header, title, msg) with languague_type of user logged
    $arrTextTranslate = getListTextTranslate(
        $arrTitle,
        $objUserInfo->intLanguageType
    );
    //redirect if dont have permission
    if($objUserInfo->intMenuPerm != 1) {
        echo "
        <script>
            alert('".$arrTextTranslate['PUBLIC_MSG_009']."');
            window.location.href = 'login.php';
        </script>
        ";
    }

    //sesstion check
    fncSessionTimeOutCheck();

    //constant
    define('SCREEN_NAME', '会社情報管理画面');


    //check get request
    fncGetRequestCheck($arrTextTranslate);

    // check request directly
    if(!isset($_SERVER['HTTP_REFERER'])){
        echo '<script type="text/javascript">
                function goBack() {
                    // Check to see if override is needed here
                    // If no override needed, call history.back()
                    history.go(-1);
                    return false;
                }
                alert("'.$arrTextTranslate['PUBLIC_MSG_049'].'");
                goBack();
            </script>';
        die();
    }
    // check page first time or refresh
    if (!isset($_SESSION["VISITS"])) {
        $_SESSION["VISITS"] = 0;
    }
    $_SESSION["VISITS"] = $_SESSION["VISITS"] + 1;

    if ($_SESSION["VISITS"] > 1) {
        //you refreshed the page!
    } else {
        // init session of screen
        $_SESSION['txtCompanyName'] = '';
        $_SESSION['txtAbbreviations'] = '';
        $_SESSION['cmbInstCategory'] = '';
        $_SESSION['cmbGroup'] = '';
        $_SESSION['COMPANY_MNG_PAGE'] = 1;
        //log view
        $strViewLog = SCREEN_NAME . '　画面表示(ユーザID = '.$objUserInfo->strUserID.') '
        .(isset($_SERVER['HTTP_REFERER']) ? $_SERVER['HTTP_REFERER'] : null);

        // write log view screen
        fncWriteLog(LogLevel['Info'] , LogPattern['View'], $strViewLog);
    }

    //get inst category list
    $strSQL = "SELECT INST_CATEGORY_NO,";
    $strSQL .= iff(
        $objUserInfo->intLanguageType,
        " INST_CATEGORY_NAME_ENG as INST_CATEGORY_NAME,",
        " INST_CATEGORY_NAME_JPN as INST_CATEGORY_NAME,"
    );
    $strSQL .= " INST_CATEGORY_NAME_ENG,";
    $strSQL .= " INST_CATEGORY_NAME_JPN,";

    $strSQL .= " SORT_NO";
    $strSQL .= " FROM m_inst_category ORDER BY SORT_NO ASC";

    $arrInstCategoryList = fncSelectData($strSQL, [], 1, false, SCREEN_NAME);
    //$blncheck = true if there is an error
    $blnCheck = false;
    if(!is_array($arrInstCategoryList)){
        //failed to get inst category list
        $blnCheck = true;
    }
    //get group list
    $strSQL = "SELECT GROUP_NO,";
    $strSQL .= iff(
        $objUserInfo->intLanguageType,
        " GROUP_NAME_ENG as GROUP_NAME,",
        " GROUP_NAME_JPN as GROUP_NAME,"
    );
    $strSQL .= " GROUP_NAME_ENG,";
    $strSQL .= " GROUP_NAME_JPN,";

    $strSQL .= " SORT_NO";
    $strSQL .= " FROM m_group ORDER BY SORT_NO ASC";

    $arrGroupList = fncSelectData($strSQL, [], 1, false, SCREEN_NAME);
    if(!is_array($arrGroupList)){
        //failed to get group list
        $blnCheck = true;
    }
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="csrf-token" content="<?php echo $strCsrf; ?>">
    <title><?php echo $arrTextTranslate['COMPANY_MNG_TEXT_001']; ?></title>
    <link rel="stylesheet" type="text/css" href="css/style.css">
    <link rel="stylesheet" type="text/css" href="css/table.css">


    <script type="text/javascript" src="js/jquery.min.js"></script>
    <link rel="stylesheet" href="css/bootstrap.min.css">
    <script src="js/bootstrap.min.js"></script>
    <!-- <script src="js/nextpage.js"></script> -->
    <script src="js/common.js"></script>

</head>
<body>
    <div class="main-content">
        <div class="main-form">
            <div class="form-title">
                <?php echo $arrTextTranslate['COMPANY_MNG_TEXT_001']; ?>
            </div>
            <?php if(isset($_SESSION['COMPANY_MNG_ERROR'])) {
                //show error message
                echo '<font color="red" class="error">'
                .$_SESSION['COMPANY_MNG_ERROR'].'</font>';
				unset($_SESSION['COMPANY_MNG_ERROR']);
			}else{
				echo '<font color="red" class="error"></font>';
			}
			?>
            <div class="form-body">
                <div class="cont-title">
                <form class="search-form" name="searchForm" autocomplete="off">
                    <input type="hidden" name="loadList" value="1">
                    <div class=" in-line">
                        <div class="label-80" style="width: 100px;">
                            <?php echo $arrTextTranslate['COMPANY_MNG_TEXT_002']; ?>
                        </div>
                        <input type="text" class="t-input t-input-125" name="txtCompanyName" value="<?php
                        echo (isset($_SESSION['txtCompanyName']) && $_SESSION['txtCompanyName'] != '' ? $_SESSION['txtCompanyName'] : ''); ?>">
                    </div>
                    <div class="in-line">
                        <div class="label-80" style="width: 130px;">
                            <?php echo $arrTextTranslate['COMPANY_MNG_TEXT_003']; ?>
                        </div>
                        <input type="text" class="t-input t-input-125" name="txtAbbreviations" value="<?php
                            echo (isset($_SESSION['txtAbbreviations']) && $_SESSION['txtAbbreviations'] != '' ? $_SESSION['txtAbbreviations'] : ''); ?>">
                    </div>
                    <div class="in-line">
                        <div class="label-100" style="width: 130px;">
                            <?php echo $arrTextTranslate['COMPANY_MNG_TEXT_004']; ?>
                        </div>
                        <div class="select-container">
                            <select name="cmbInstCategory">
                                <option value="">ALL</option>
                                <?php
                                    //inst category list
                                    if(isset($arrInstCategoryList) && is_array($arrInstCategoryList))
                                    foreach($arrInstCategoryList as $arrInstCategory) {
                                        $strSelect = '';
                                        if($_SESSION['cmbInstCategory'] == $arrInstCategory['INST_CATEGORY_NO']){
                                            $strSelect = 'selected';
                                        }
                                        echo '<option value="'.$arrInstCategory['INST_CATEGORY_NO'].'" '.$strSelect.'>'
                                                .fncHtmlSpecialChars($arrInstCategory['INST_CATEGORY_NAME']).'</option>';
                                    }
                                ?>
                            </select>
                        </div>
                    </div>
                    <div class="in-line">
                        <div class="label-80">
                            <?php echo $arrTextTranslate['COMPANY_MNG_TEXT_005']; ?>
                        </div>
                        <div class="select-container">
                            <select name="cmbGroup">
                                <option value="">ALL</option>
                                <?php
                                    //group list
                                    if(isset($arrGroupList) && is_array($arrGroupList))
                                    foreach($arrGroupList as $arrGroup){
                                        $strSelect = '';
                                        if($_SESSION['cmbGroup'] == $arrGroup['GROUP_NO']){
                                            $strSelect = 'selected';
                                        }
                                        echo '<option value="'.$arrGroup['GROUP_NO'].'"'.$strSelect.'>'
                                                .fncHtmlSpecialChars($arrGroup['GROUP_NAME']).'</option>';
                                    }
                                ?>
                            </select>
                        </div>
                    </div>
                    <div class="in-line col-right">
                        <input type="button" class="tbtn tbtn-defaut btn-search" style="margin-top:3px;"
                        value="<?php echo $arrTextTranslate['PUBLIC_BUTTON_006']; ?>">
                    </div>
                    <div style="clear:both"></div>
                </form>
                </div>

                <div class="ajax-content">

                </div>
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
    var confirmDelMsg = '<?php echo $arrTextTranslate['COMPANY_MNG_MSG_001']; ?>';
    var csrf = '<?php echo $strCsrf; ?>';
    var ajaxUrl = 'company_mng_proc.php';


    $(function() {
        function loadPageNew() {
            $.ajax({
                url: ajaxUrl,
                type: 'post',
                data: [{name: 'X-CSRF-TOKEN', value: csrf}],
                success: function(result) {
                    $(".ajax-content").html(result);
                }
            });
        }

		function loadPage() {
			$('.error').html('');
			var html = $('.ajax-content').html();
			var arr = $('.search-form').serializeArray();
			arr.push({name: 'originalSearch', value: 0});

			$.ajax({
				url: ajaxUrl,
				type: 'post',
				data: arr,
				success: function(result){
					$(".ajax-content").html(result);
				}
			});
		}
<?php
if($_SESSION['VISITS'] > 1){
?>
		loadPage();
<?php
}else{
?>
		loadPageNew();
<?php
}
?>
    });
    <?php if($blnCheck){
        //$blnCheck == true, show error message
        echo "$('.error').html('".$arrTextTranslate['PUBLIC_MSG_001']."')";
    }
    ?>
</script>

</html>