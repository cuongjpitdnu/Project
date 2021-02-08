<?php
/*
* @incident_boad.php
*
* @create 2020/02/20 KBS Tam.nv
* @update
*/
require('portal_incident_boad_proc.php');

$arrIncidentInfo = get_incident_case();

?>
<div class="">
    <div class="col-md-6">
        <?php require('info_form.php')?>
        <div class="title text-yellow mar-tl-10">
            <?php echo $arrText['PORTAL_INCIDENT_TEXT_011']?>
        </div>
        <div class="t-row">
            <div class="select-container right-100 in-line">
                <select class="sl-boad">
                    <option value="boad1"><?php echo $arrText['INFORMATION_EDIT_TEXT_005']?></option>
                    <option value="boad2"><?php echo $arrText['INFORMATION_EDIT_TEXT_006']?></option>
                </select>
            </div>
            <div class="in-line">
                <span class="text-yellow">
                <?php echo $arrText['PORTAL_INCIDENT_TEXT_012']?> </span>
                <input type="checkbox" class="right-100" name="without_com" id="without_com" <?php echo @$strCheckWithoutCom ?> >
            </div>
            <div class="in-line col-right">
                <?php
                    //check permission to show data
                    //check permission to show data
                    $strDisplayStyle = "display: none;";
                    if($objLoginUserInfo->intInformationRegPerm){
                        $strDisplayStyle = "";
                    }
                ?>
                        <button class="tbtn tbtn-defaut btnRegInfo" name="btnRegInfo" style="<?php echo $strDisplayStyle?>" onclick="
                                loadModalInfoEdit(0,<?php echo @$arrIncidentInfo[0]['INCIDENT_CASE_NO']?>)" >
                            <?php echo $arrText['PORTAL_INCIDENT_TEXT_013']?>
                        </button>

            </div>
        </div>
    </div>

    <div class="col-md-6">
        <div class="title text-yellow">
            <?php echo $arrText['PORTAL_INCIDENT_TEXT_002']?>
        </div>
        <div class="incident-reg">
            <?php require('incident_case_form.php') ?>
        </div>
        <div class="cont-requests ">
            <div class="row-no-gutters clearfix">
                <div class="col-md-2 title"><?php echo $arrText['PORTAL_INCIDENT_TEXT_010']?></div>
                <div class="col-md-8 title"><?php echo $arrText['PORTAL_INCIDENT_TEXT_009']?></div>
                <div class="col-md-2 title"><?php echo $arrText['PORTAL_INCIDENT_TEXT_021']?></div>
            </div>
            <div class="info-request">
                <?php require ('incident_request_form.php')?>
            </div>
        </div>
    </div>
</div>
<div class="cont-boad1 col-md-12">
    <table class="blueTable-incident">
        <thead>
        <tr>
            <?php
            foreach ($arrInsCategory as $intKey => $arrCat){
                echo '<th class="bg-'.$arrColorBgArr[$intKey].'">'
                    .$arrCat['INST_CATEGORY_NAME'.$strEndLang].'</th>';
            }
            ?>
        </tr>
        </thead>
        <tbody id="content-info">
            <?php require ('incident_boad_info_form.php')?>
        </tbody>
    </table>
</div>
<div class="cont-boad2 col-md-12">
    <table class="blueTable-incident">
        <thead>
        <tr>
            <?php
            foreach ($arrInfoCategory as $intKey => $arrCat){
                echo '<th class="bg-'.$arrColorBgArr[$intKey].'">'
                    .$arrCat['INFO_CATEGORY_NAME'.$strEndLang].'</th>';
            }
            ?>
        </tr>
        </thead>
        <tbody>
        <tr id="long_form">
            <?php
            require ('incident_boad_info_long_form.php');
            ?>
        </tr>
        </tbody>
    </table>
</div>
<script>
    $('.cont-boad2').hide();

    $('.sl-boad').on('change', function() {

        if(this.value == 'boad1'){
            $('.cont-boad1').show();
            $('.cont-boad2').hide();
            
            var checkWithoutCom = $('#without_com').is(':checked');
            loadInfoInCident(checkWithoutCom);
            
        }else{
            $('.cont-boad1').hide();
            $('.cont-boad2').show();
        }
    });





</script>
