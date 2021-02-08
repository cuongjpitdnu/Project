<?php
/*
* @incident_case_form.php
*
* @create 2020/02/20 KBS Tam.nv
* @update
*/
    require_once ('portal_function.php');
    
    $arrIncident = get_incident_case();
    
    //▼2020/06/01 KBS S.Tasaki JCMGボード権限がない場合は取得データを表示しない。
    if($objLoginUserInfo->intJcmgTabPerm != 1){
        $arrIncident = array();
    }
    //▲2020/06/01 KBS S.Tasaki
    
    echo '<div class="incident-reg-content">';
        //if have data will loop data and show it
        if($arrIncident){
            foreach ($arrIncident as $arrItem){
                $strTitle = $arrItem['TITLE'.$strEndLang];
                //if not exist text, set text with other language type
                if(!$strTitle){
                    $intUntra = 1;
                    // check language type
                    if(!$objLoginUserInfo->intLanguageType){
                        $strTitle = $arrItem['TITLE_ENG'];
                    }else{
                        $strTitle = $arrItem['TITLE_JPN'];
                    }
                }
                $strContent = $arrItem['CONTENTS'.$strEndLang];

                //if not exist text, set text with other language type
                if(!$strContent){
                    $intUntra = 1;
                    //check language type
                    if(!$objLoginUserInfo->intLanguageType){
                        $strContent = $arrItem['CONTENTS_ENG'];
                    }else{
                        $strContent = $arrItem['CONTENTS_JPN'];
                    }
                }

                echo '<div>'.$arrText['PORTAL_INCIDENT_TEXT_003'].': ' . date('Y/m/d H:i',strtotime($arrItem['S_DATE'])).'</div>';
                echo '<div>'.$arrText['PORTAL_INCIDENT_TEXT_004'].': <a href="#" onclick=\'loadModalIncident('.$arrItem['INCIDENT_CASE_NO'].')\' >' . fncHtmlSpecialChars($strTitle).' </a></div>';
                echo '<div>'.$arrText['PORTAL_INCIDENT_TEXT_005'].': ' . fncHtmlSpecialChars($strContent).'</div>';
            }
        }
    echo '</div>';
?>

<div class="row-no-gutters mar-tl-10">
    <div class="col-md-6">
        <div class="title text-yellow">
            <?php echo $arrText['PORTAL_INCIDENT_TEXT_008']?>
        </div>
    </div>
    <div class="col-md-6 text-right " style="margin-bottom: 10px;">
        <input type="hidden" value="<?php echo @$arrIncident[0]['INCIDENT_CASE_NO']?>" class="hideRegRequest">
        <input type="hidden" value="<?php echo @$objLoginUserInfo->intInformationRegPerm ?>" class="infoBtnView">
        <input type="hidden" value="<?php echo @$objLoginUserInfo->intAnnounceRegPerm ?>" class="annoBtnView">
        <input type="hidden" value="<?php echo @$objLoginUserInfo->intQueryRegPerm ?>" class="queryBtnView">
        <?php
        //check permission to show data
        if($objLoginUserInfo->intRequestRegPerm){
            ?>
            <button class="tbtn tbtn-defaut btnRegRequest" name="btnRegRequest"
                    href="request_edit.php" onclick="
                loadModalReqOnEdit(0,<?php echo @$arrIncident[0]['INCIDENT_CASE_NO']?>)" >
                <?php echo $arrText['PUBLIC_BUTTON_001']?>
            </button>
            <?php
        }
        ?>
    </div>
</div>


