<?php
/*
* @info_form_content.php
*
* @create 2020/02/20 KBS Tam.nv
* @update
*/

require_once ('portal_proc.php');
fncSessionUp('ポータル画面');
//if have data will loop data and show it
if(count($arrInfo) >0){
    foreach ($arrInfo as $arrItem){
        $strTilte = $arrItem['TITLE'.$strEndLang];
        // if text of this language null, set text with text of other language
        if(!$strTilte){
            $intUntra = 1;
            //check language type
            if(!$objLoginUserInfo->intLanguageType){
                $strTilte = $arrItem['TITLE_ENG'];
            }else{
                $strTilte = $arrItem['TITLE_JPN'];
            }
        }
        ?>
        <div class="row-no-gutters clearfix">
            <div class="t-col-md-3">
                <?php echo date('Y/m/d H:i',strtotime($arrItem['REG_DATE']))?>
            </div>
            <div class="t-col-md-60">
                <a href="#" onclick="loadModalAnonView(<?php echo $arrItem['ANNOUNCE_NO'] ?>,1)">
                    <?php echo fncHtmlSpecialChars($strTilte)?>
                </a>
            </div>
        </div>
        <?php
    }
}
?>

