<?php
/*
* @portal.php
*
* @create 2020/02/17 KBS Tam.nv
* @update
*/
require_once('portal_proc.php');

fncSessionUp('ポータル画面');
// chien add - reset times visit page
$_SESSION["VISITS"] = 0;
?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="csrf-token" content="<?php echo (isset($strCsrf) ? $strCsrf : ''); ?>">
    <title><?php echo $arrText['PORTAL_TEXT_005'] ?></title>
    <link rel="stylesheet" type="text/css" href="css/style.css">
    <link rel="stylesheet" type="text/css" href="css/tabs.css">
    <link rel="stylesheet" type="text/css" href="css/table-info.css">
    <link rel="stylesheet" type="text/css" href="css/menu.css">
    <link rel="stylesheet" href="css/bootstrap.min.css">
    <!--<script type="text/javascript" src="js/jquery.min.js"></script>-->
    <script src="js/jquery-3.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/menu.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="js/common.js"></script>
    <script src="js/nextpage.js"></script>
    <script src="js/incident.js"></script>

    <!--Map-->
    <link rel="stylesheet" href="js/OpenLayers-6.1.1/ol.css" type="text/css">
    <script src="js/OpenLayers-6.1.1/ol.js" type="text/javascript"></script>
    <script src="../mapFunction.js" type="text/javascript"></script>
    <style type="text/css">
        #map .ol-zoom {
			left: auto;
			right: 1.0em;
		}
		#map .ol-zoomslider {
			background-color: transparent;
			top: 2.8em;
			left: auto;
			right: 1.0em;
			height: 200px;
		}
		#map .ol-scale-line {
			top: auto;
			bottom: 0.1em;
			left: auto;
			right: 4.0em;
		}
		#map .ol-zoom .ol-zoom-out {
			margin-top: 208px;
		}
		#map .ol-rotate {
			visibility:hidden;
			top: auto;
			bottom: 2.0em;
			left: auto;
			right: 1.0em;
		}
		#map .ol-control button:focus, .ol-control button:hover {
			background-color: rgba(0,60,136,.5);
		}
		#map .ol-control button {
			font-size: 1.5em;
		}
		#map .ol-zoomslider button {
			height: 14px;
		}
    </style>

<?php
    //AKB Thang -- comment by Tam.nv
    //check session tab value
    if(isset($_SESSION['tab']) && $_SESSION['tab'] == 1){
    if($intIncident == 0){
    	$_SESSION['tab'] = 0;
    }
    //check http referer
    if(isset($_SERVER['HTTP_REFERER'])){
        $refer = $_SERVER['HTTP_REFERER'];
        $tmpArr = explode('/', $refer);
        if($tmpArr[count($tmpArr)-1] != 'portal.php'){
            //incident tab select
            ?>
            <script>
                $(function(){
                    $('.tabbs').eq(<?php echo $_SESSION['tab']; ?>).click();
                });
            </script>

            <?php
            }
        }

    }

    //AKB Thang
?>

</head>
<body >
<div class="main-content">
    <input type="hidden" id="reloadTimeInfoBoad" value="<?php echo  PORTAL_RELOAD_TIME ?>">
    <input type="hidden" id="queryReloadTimePortal" value="<?php echo  QUERY_RELOAD_TIME_PORTAL ?>">
    <input type="hidden" name="csrf" id="csrf" value="<?php echo @$strCsrf?>" >
    <div class="header-boad">
        <div class="in-line right-100"><img src="img/logo.jpg" alt=""></div>
        <label name="lblViewTimeJST"><span id="timeJST"><?php echo $strJstTime ?></span> JST　</label>
        <label name="lblViewTimeUTC"><span id="timeUTC"><?php echo $strUtcTime ?></span> UTC</label>
        <div class="in-line mark-btn">
            　　<span class="text-red"><?php echo $arrText['PORTAL_TEXT_017']?></span>
            <span id="span-btnSearch">
                <button class="tbtn tbtn-defaut jcmg-btn" onclick="loadModal('incident_case_edit.php')"><?php echo $arrText['PORTAL_DAILY_BUTTON_001']  ?></button>
            </span>
        </div>
        <div class="in-line">
        <div class="dropdown">
            <button onclick="myFunction()" class="dropbtn"><?php echo $arrText['PORTAL_TEXT_004']?>　▼</button>
            <?php
                //check permission to show data
                if($objLoginUserInfo->intMenuPerm) {
            ?>
                <div id="myDropdown" class="dropdown-content select-gray">
                    <div class="t-outgroup"><?php echo $arrText['PORTAL_TEXT_007']?></div>
                    <a href="announce_mng.php"><?php echo $arrText['PORTAL_TEXT_008']?></a>
                    <a href="bulletin_board_mng.php"><?php echo $arrText['PORTAL_TEXT_009']?></a>
                    <div class="t-outgroup"><?php echo $arrText['PORTAL_TEXT_010']?></div>
                    <a href="incident_case_mng.php"><?php echo $arrText['PORTAL_TEXT_011']?></a>
                    <div class="t-outgroup"><?php echo $arrText['PORTAL_TEXT_012']?></div>
                    <a href="company_mng.php"><?php echo $arrText['PORTAL_TEXT_013']?></a>
                    <a href="user_mng.php"><?php echo $arrText['PORTAL_TEXT_014']?></a>
                    <a href="link_mng.php"><?php echo $arrText['PORTAL_TEXT_015']?></a>
                    <a href="user_setting.php" class="load-modal" data-id="" >
                        <?php echo $arrText['PORTAL_TEXT_016']?>
                    </a>
                </div>
            <?php
                } else {
            ?>
                    <div id="myDropdown" class="dropdown-content select-gray">
                        <div class="t-outgroup"><?php echo $arrText['PORTAL_TEXT_012']?></div>
                        <a href="user_setting.php" class="load-modal" data-id="" ><?php echo $arrText['PORTAL_TEXT_016']?></a>
                    </div>
            <?php
                }
            ?>

        </div>

        <div class="in-line col-right right-100">
            <div class="in-line">
                <div class="right-20"><label name="lblUserID"><?php echo $arrText['PORTAL_TEXT_001']?>: <?php echo fncHtmlSpecialChars($objLoginUserInfo->strUserID)?></label></div>
                <div class="right-20"><label name="lblCompanyName"><?php echo $arrText['PORTAL_TEXT_002']?>: <?php echo fncHtmlSpecialChars($objLoginUserInfo->strCompanyName)?></label></div>
                <div class="right-20"><label name="lblUserName"><?php echo $arrText['PORTAL_TEXT_003']?>: <?php echo fncHtmlSpecialChars($objLoginUserInfo->strUserName)?></label></div>
            </div>
        </div>
    </div>


    <div class = "tabinator">
        <input type = "radio" id = "tab1" class="input-tab" name = "tabs" checked>
        <label for = "tab1" class="title tabbs"><?php echo $arrText['PORTAL_TEXT_005'] ?></label>
        <input type = "radio" id = "tab2" class="input-tab" name = "tabs">
        <label for = "tab2" class="title tabbs el-tab-2"><?php echo $arrText['PORTAL_TEXT_006'] ?></label>
        <input type="hidden" id="reloadTimeInfoBoad" value="<?php echo PORTAL_RELOAD_TIME ?>">
        <input type="hidden" name="csrf" id="csrf" value="<?php echo (isset($strCsrf) ? $strCsrf : ''); ?>" >
        <div id = "content1" class="bg-content">
            <?php require_once('daily_boad.php'); ?>
        </div>
        <div id = "content2" class="bg-content">
            <div class="incident_boad"></div>
            <?php require_once('incident_boad.php'); ?>
        </div>
    </div>
</div>

<!-- Modal HTML -->
<div id="myModal" class="modal fade">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div id="modal-body">

            </div>
        </div>
    </div>
</div>
<style>
    body{
        overflow: hidden;
    }
</style>

<script>
    $( document ).ready(function() {
        var strUntrTitle = '<?php echo @$intUntra; ?>';
        if(strUntrTitle == 1){
            var mesUnTitle = '<?php echo $arrText['PUBLIC_MSG_042']?>';
            alert(mesUnTitle);
        }

        var checkSQLError =  '<?php echo @$_SESSION['SQL_GET_DATA_ERROR'] ?>';
        if(checkSQLError.length > 0){
            alert(<?php echo @$arrText[$_SESSION['SQL_GET_DATA_ERROR']]?>);
        }

        var intTabSession = <?php if(isset($_SESSION['tab'])) {
            echo $_SESSION['tab'];
        } else { echo 0; } ?>;
        if(intTabSession != '')  {
            intTabSession += 1;
        }
        $('#tab'+intTabSession).click();
    });
    var intInciBoadShow = '<?php echo @$intIncident; ?>';
    if(intInciBoadShow == 0){
        $('.el-tab-2').hide();
    }else{
        $('.el-tab-2').show();
    }

    var intInciShowBtn = '<?php echo $intShowBtn; ?>';
    if(intInciShowBtn !== '1'){
        $('#span-btnSearch').hide();
    }

</script>


</body>
</html>