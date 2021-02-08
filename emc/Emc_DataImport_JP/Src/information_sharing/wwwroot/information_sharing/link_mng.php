<?php
    /*
     * @link_mng.php
     *
     * @create 2020/03/13 AKB Thang
     */

    require_once('common/common.php');
    require_once('common/session_check.php');
    header('Content-type: text/html; charset=utf-8');
    header('X-FRAME-OPTIONS: DENY');
    //can not connect to database, redirect to login page
    if(fncConnectDB() == false) {
        $_SESSION['LOGIN_ERROR'] = 'DB接続に失敗しました。';
        header('Location: login.php');
        exit;
    }
    $arrTitle =  array(
        'LINK_MNG_TEXT_001',
        'LINK_MNG_TEXT_002',
        'PUBLIC_BUTTON_006',
        'LINK_MNG_MSG_001',
        'PUBLIC_MSG_009',
        'PUBLIC_MSG_049',
    );
    $arrTextTranslate = getListTextTranslate($arrTitle);

    //if not login
    if(!isset($_SESSION['LOGINUSER_INFO'])) {
        echo "
        <script>
            alert('".PUBLIC_MSG_008_JPN . "/" . PUBLIC_MSG_008_ENG."');
            window.location.href = 'login.php';
        </script>
        ";
        exit();
    }
    //sesstion check
    fncSessionTimeOutCheck();
    $objUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);
    //define constant
    define('SCREEN_NAME', 'リンク情報管理画面');

    $arrTextTranslate = getListTextTranslate(
        $arrTitle,
        $objUserInfo->intLanguageType
    );
    //check get request
    fncGetRequestCheck($arrTextTranslate);
    // check request directly
    if(!isset($_SERVER['HTTP_REFERER'])) {
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
    //check user permission, if menu_perm !=1 -> alert then redirect to login page
    if($objUserInfo->intMenuPerm != 1) {
        echo "
        <script>
            alert('".$arrTextTranslate['PUBLIC_MSG_009']."');
            window.location.href = 'login.php';
        </script>
        ";
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
        $_SESSION['catNO'] = '';

        //log view
        $strViewLog = SCREEN_NAME . '　画面表示(ユーザID = '
            .$objUserInfo->strUserID.') '
                .(isset($_SERVER['HTTP_REFERER']) ? $_SERVER['HTTP_REFERER'] : null);
                fncWriteLog(LogLevel['Info'] , LogPattern['View'], $strViewLog);
    }


    //get link categories
    $strSQL = "SELECT LINK_CATEGORY_NO,";
    $strSQL .= iff(
        $objUserInfo->intLanguageType,
        " LINK_CATEGORY_NAME_ENG as LINK_CATEGORY_NAME,",
        " LINK_CATEGORY_NAME_JPN as LINK_CATEGORY_NAME,"
    );
    $strSQL .= " LINK_CATEGORY_NAME_JPN,";
    $strSQL .= " LINK_CATEGORY_NAME_ENG,";

    $strSQL .= " SORT_NO";
    $strSQL .= " FROM m_link_category ORDER BY SORT_NO ASC";

    $arrLinkCategoryList = fncSelectData($strSQL, [], 1, false, SCREEN_NAME);

?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="csrf-token" content="<?php echo $strCsrf; ?>">
    <title><?php echo $arrTextTranslate['LINK_MNG_TEXT_001']; ?></title>
    <link rel="stylesheet" type="text/css" href="css/style.css">
    <link rel="stylesheet" type="text/css" href="css/table.css">
    <link rel="stylesheet"
        href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <script type="text/javascript" src="js/jquery.min.js"></script>
    <link rel="stylesheet" href="css/bootstrap.min.css">
    <script src="js/bootstrap.min.js"></script>
    <!-- <script src="js/nextpage.js"></script> -->
    <script src="js/common.js"></script>
    <style>
        table.blueTable td{
            padding: 9px 10px !important;
        }
    </style>
</head>
<body>
    <div class="main-content">
        <div class="main-form">
            <div class="form-title">
                <?php echo $arrTextTranslate['LINK_MNG_TEXT_001']; ?>
            </div>
            <?php
            //show error
            if(isset($_SESSION['LINK_MNG_ERROR'])) {
				echo '<font color="red" class="error">'.$_SESSION['LINK_MNG_ERROR'].'</font>';
			}else{
				echo '<font color="red" class="error"></font>';
			}
			?>
            <div class="form-body">
                <div class="cont-title col-right">
                <form class="search-form" name="searchForm" autocomplete="off">
                    <input type="hidden" name="loadList" value="1">
                    <div class="in-line">
                        <div class="label-80">
                            <?php echo $arrTextTranslate['LINK_MNG_TEXT_002']; ?>
                        </div>
                        <div class="select-container">
                            <select name="catNO">
                            <option value="">ALL</option>
                            <?php
                                //show list of link category
                                if(is_array($arrLinkCategoryList)) {
                                    foreach($arrLinkCategoryList as $arrLinkCategory) {
                                        $strSelect = '';
                                        if($_SESSION['catNO'] == $arrLinkCategory['LINK_CATEGORY_NO']){
                                            $strSelect = 'selected';
                                        }
                                        ?>
                                <option value="<?php
                                    echo $arrLinkCategory['LINK_CATEGORY_NO']
                                    ?>" <?php echo $strSelect ?>>
                                <?php echo fncHtmlSpecialChars($arrLinkCategory['LINK_CATEGORY_NAME']); ?>
                            </option>
                            <?php } } ?>
                            </select>
                        </div>
                    </div>
                    <div class="in-line">
                        <input type="button" class="tbtn tbtn-defaut btn-search" value="<?php
                            echo $arrTextTranslate['PUBLIC_BUTTON_006'];
                        ?>">
                    </div>
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
    var confirmMsg = '<?php echo $arrTextTranslate['LINK_MNG_MSG_001']; ?>';
    var confirmDelMsg = '<?php echo $arrTextTranslate['LINK_MNG_MSG_001']; ?>';
    var csrf = '<?php echo $strCsrf; ?>';
    var ajaxUrl = 'link_mng_proc.php';
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
</script>
</html>

