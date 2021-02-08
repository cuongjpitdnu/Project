<?php
/*
* @bull_form.php
*
* @create 2020/02/20 KBS Tam.nv
* @update
*/
?>
<div>
    <?php
    require_once ('portal_proc.php');
    $arrPlaceId = array();
    /** @var array $arrBulletin */
    //if have data will loop data and show it
    if(!empty(@$arrBulletin)){
        foreach ($arrBulletin as $arrItem){
            $strName = $arrItem['INCIDENT_NAME'.$strEndLang];
            $strBusName = $arrItem['BUSINESS_NAME'.$strEndLang];
            // if text of this language null, set text with BUSINESS_NAME
            if(!$strBusName){
                $strBusName = $arrItem['BUSINESS_NAME'];
            }
            // if text of this language null, set text with text of other language
            if(!$strName){
                $intUntra = 1;
                //check language type
                if(!$objLoginUserInfo->intLanguageType){
                    $strName = $arrItem['INCIDENT_NAME_ENG'];
                }else{
                    $strName = $arrItem['INCIDENT_NAME_JPN'];
                }
            }

            ?>
            <div class="row-no-gutters clearfix">
                <div class="col-md-3">
                    <?php echo date('Y/m/d H:i',strtotime($arrItem['OCCURRENCE_DATE']))?>
                </div>
                <div class="col-md-3">
                    <?php echo fncHtmlSpecialChars($strBusName)?>
                </div>
                <div class="t-col-md-45">
                    <a href="#" onclick="
                            loadViewBull(<?php echo $arrItem['BULLETIN_BOARD_NO'] ?>,'portal')">
                        <?php echo fncHtmlSpecialChars($strName)?>
                    </a>
                </div>
            </div>
            <?php
            $arrPlaceId[] = $arrItem['MAP_ID'];
        }
    }
    ?>
</div>
<script>
    $( document ).ready(function() {
        var isAjax = '<?php echo $intAjax ?>';
        if (isAjax == 0){
            initMap('../');
            changeSearchLocationMode();
            localStorage.removeItem("cookie");
        }
        var locationIdArray = <?php echo json_encode($arrPlaceId); ?>;
        viewLocation(locationIdArray);

        if (isAjax == 1){
            var arrIds = JSON.parse(localStorage.getItem("cookie"));
            if(arrIds != null){
                for (var i=0; i < arrIds.length; i++) {
                    highlightLocation(arrIds[i]);
                }
            }
        }
    });

</script>