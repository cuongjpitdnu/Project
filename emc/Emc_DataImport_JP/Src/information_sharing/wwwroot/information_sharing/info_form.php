<?php
/*
* @info_form.php
*
* @create 2020/02/20 KBS Tam.nv
* @update
*/
?>
<div class="clearfix">
    <div class="in-line float-left title text-yellow">
        <div><?php echo $arrText['PORTAL_DAILY_TEXT_001'] ?></div>
    </div>
    <div class="in-line col-right">
        <?php
            //check permission to show data
            $strDisplayStyle = "display: none;";
            if($objLoginUserInfo->intAnnounceRegPerm){
                $strDisplayStyle = "";
            }
        ?>

        <input type="button" class="tbtn tbtn-defaut right-20 load-modal"
                   href="announce_edit.php" data-id="" name="btnRegAnnounce"
                   value="<?php echo $arrText['PUBLIC_BUTTON_001']?>" style="<?php echo $strDisplayStyle?>" />

        <div class="select-container">
            <select class="select-150 selAnnounceTime" data-id="1" name="cmbAnnounceYears" id="selTimeAnnouce1">
                <option value="">All</option>
                <?php
                for ($i = 0; $i <= 11; $i++) {
                    $option = date("Y/m", strtotime( date( "Y-m-01" )." -$i months"));
                    echo '<option value="'.$option.'">'.$option.'</option>';
                }
                ?>

            </select>
        </div>
    </div>
</div>
<div class="cont-table ">
    <div class="row-no-gutters clearfix">
        <div class="t-col-md-3"><?php echo $arrText['PORTAL_DAILY_TEXT_003']?></div>
        <div class="t-col-md-3"><?php echo $arrText['PORTAL_DAILY_TEXT_004']?></div>
    </div>
    <div class="info-cont" id="info-cont-1">
        <?php require_once('info_form_content.php')?>
    </div>
</div>