<?php
/*
* @incident_boad_info_long_form.php
*
* @create 2020/02/20 KBS Tam.nv
* @update
*/
require_once ('incident_boad_ajax.php');

//▼2020/06/01 KBS S.Tasaki JCMGボード権限がない場合は取得データを表示しない。
if($objLoginUserInfo->intJcmgTabPerm != 1){
    $arrInfoByCateData = array();
}
//▲2020/06/01 KBS S.Tasaki

//▼2020/06/01 KBS S.Tasaki リクエストの改ざんによりデータが正常に取得できたかった場合の対策
if(!is_array($arrInfoCategory)){
    $arrInfoCategory = array();
}
//▲2020/06/01 KBS S.Tasaki


foreach ($arrInfoCategory as $key => $cat){
    echo '<td>';
    echo '<div class="bg-white"><span class="col-md-4">'.$arrText['PORTAL_INCIDENT_TEXT_016'].
        '</span><span class="col-md-4">'.$arrText['PORTAL_INCIDENT_TEXT_014'].
        '</span><span class="col-md-4">'.$arrText['PORTAL_INCIDENT_TEXT_015'].'</span></div>';
    echo '<div class="ticker-2 bg-white" >';
    //if have data will loop data and show it
    if(@$arrInfoByCateData[$cat['INFO_CATEGORY_NO']]){
        foreach ($arrInfoByCateData[$cat['INFO_CATEGORY_NO']] as $intCatNo =>  $arrItem){
            foreach ($arrItem as $arrInfoOfCom){
                $strTitle = $arrInfoOfCom['TITLE'.$strEndLang];
                //if not exist text, set text with other language type
                if(!$strTitle){
                    $intUntra = 1;
                    //check language typpe
                    if(!$objLoginUserInfo->intLanguageType){
                        $strTitle = $arrInfoOfCom['TITLE_ENG'];
                    }else{
                        $strTitle = $arrInfoOfCom['TITLE_JPN'];
                    }
                }
                $dtmDateInfo = date('m/d H:i',strtotime($arrInfoOfCom['UP_DATE']));
                echo '<div class="row">';
                echo '<span class="col-md-4 text-abbre" data-fhd="'.$arrText['FHD_COMPANY_INFO'].'" data-hd="'.$arrText['COMPANY_INFO'].'" 
                title="'.fncHtmlSpecialChars($arrInfoOfCom['ABBREVIATIONS'.$strEndLang]).'">'.fncHtmlSpecialChars($arrInfoOfCom['ABBREVIATIONS'.$strEndLang]).'</span>';
                echo '<span class="col-md-4">'.$dtmDateInfo.'</span>';
                echo '<span class="col-md-4">';
                echo '<a href="#" title="'.fncHtmlSpecialChars($strTitle).'"
                                        data-fhd="'.fncHtmlSpecialChars($arrText['FHD_TITLE_INFO']).'" data-hd="'.fncHtmlSpecialChars($arrText['TITLE_INFO']).'"
                                        onclick="loadView(\'information_view.php\','.$arrInfoOfCom['INFORMATION_NO'].')"
                                        class="tl-red-boad title-info1">'.fncHtmlSpecialChars($strTitle).'</a>';
                echo '</span>';
                echo '</div>';
            }
        }
    }
    echo '</div>';
    echo '</td>';
}
?>

<script>
    showTitle('text-abbre');
    showTitle('title-info1');

    setHeighIncidentInfo();

    function setHeighIncidentInfo() {
        var infoRight = 'info-cont';
        var ticker2 = 'ticker-2';
        doResize(infoRight,15);
        doResizeTable(ticker2,48);
    }


    window.onresize = function(event) {
        setHeigh();
        setHeighIncidentInfo();
        setHeighQuery();
        heighMapAndBull();
    }

    $(function () {
        $('[data-toggle="tooltip"]').tooltip();
    });

</script>

