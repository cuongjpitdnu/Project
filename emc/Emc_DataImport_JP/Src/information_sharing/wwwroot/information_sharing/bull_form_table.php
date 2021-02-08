
<?php
/*
* @bull_form.php
*
* @create 2020/02/20 KBS Tam.nv
* @update
*/
?>
<div class="text-center title top-10"><?php echo $arrText['PORTAL_DAILY_TEXT_013'] ?></div>
<div class="text-right right-20">
    <button type="submit" class="tbtn-cancel tbtn-defaut closeModal"
            id="close" onclick="clearHighlightWhenClose()"
            data-dismiss="modal"><?php echo $arrText['PUBLIC_BUTTON_003']; ?>
    </button>
</div>
<div class="clearfix top-20">
    <div class="col-md-3"><?php echo $arrText['PORTAL_DAILY_TEXT_010'] ?></div>
    <div class="col-md-3"><?php echo $arrText['PORTAL_DAILY_TEXT_011'] ?></div>
    <div class="t-col-md-45"><?php echo $arrText['PORTAL_DAILY_TEXT_012'] ?></div>
</div>

<?php
foreach ($arrBulletin as $arrItem){
    $strName = $arrItem['INCIDENT_NAME'.$strEndLang];
    $strBusName = $arrItem['BUSINESS_NAME'.$strEndLang];
    // if text of this language null, set text is BUSINESS_NAME
    if(!$strBusName){
        $strBusName = $arrItem['BUSINESS_NAME'];
    }
    // if text of this language null, set text with text of other language
    if(!$strName){
        $intUntra = 1;
        if(!$objLoginUserInfo->intLanguageType){
            $strName = $arrItem['INCIDENT_NAME_ENG'];
        }else{
            $strName = $arrItem['INCIDENT_NAME_JPN'];
        }
    }
    ?>
    <div class="clearfix">
        <div class="col-md-3">
            <?php echo date('Y-m-d H:i',strtotime($arrItem['OCCURRENCE_DATE']))?>
        </div>
        <div class="col-md-3">
            <?php echo fncHtmlSpecialChars($strBusName)?>
        </div>
        <div class="t-col-md-45">
            <a href="#" onclick="
                loadViewBull(<?php echo $arrItem['BULLETIN_BOARD_NO'] ?>, 'portal')">
                <?php echo fncHtmlSpecialChars($strName)?>
            </a>
        </div>
    </div>
    <?php
}
?>

