<?php
/*
* @incident_request_form.php
*
* @create 2020/02/20 KBS Tam.nv
* @update
*/
    require_once ('portal_function.php');
    
    $arrRequest = get_request();
    
    //▼2020/06/01 KBS S.Tasaki JCMGボード権限がない場合は取得データを表示しない。
    if($objLoginUserInfo->intJcmgTabPerm != 1){
        $arrRequest = array();
    }
    //▲2020/06/01 KBS S.Tasaki
    
    //if have data will loop data and show it
    if($arrRequest){
        foreach ($arrRequest as $arrItem){
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
            echo '<div class="row-no-gutters clearfix">';
                echo '<div class="col-md-2">'
                    .date('Y/m/d H:i',strtotime($arrItem['REG_DATE'])).'　</div>';
                echo '<div class="col-md-8 ">
                <a href="#" onclick=\'loadView("request_view.php",'
                    .$arrItem['REQUEST_NO'].')\' >' . fncHtmlSpecialChars($strTilte).' </a></div>';
                echo '<div class="col-md-2 ">'.fncHtmlSpecialChars($arrItem['ABBREVIATIONS'.$strEndLang]).'</div>';
            echo '</div>';
        }
    }
?>

