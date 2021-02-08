<?php
/*
* @category_detail.php
*
* @create 2020/02/27 KBS Tam.nv
* @update
*/

require_once('category_detail_proc.php') ?>
<!DOCTYPE html>
<html lang="en">
<head>
    <title>
        <?php echo $arrText['PORTAL_DAILY_TEXT_009'].
            @$arrCategory[0]['LINK_CATEGORY_NAME_JPN'].'/ '.
            @$arrCategory[0]['LINK_CATEGORY_NAME_ENG'] ?>
    </title>
    <link rel="stylesheet" type="text/css" href="css/style.css">
</head>
<body>
<div class="main-content">
    <div class="main-form">
        <div class="form-title">
            <?php
            //if have data will show it
            if(@$arrCategory[0]['LINK_CATEGORY_NAME_JPN']){
                $strTitle = ' ('. @$arrCategory[0]['LINK_CATEGORY_NAME_JPN'].
                '/ '. @$arrCategory[0]['LINK_CATEGORY_NAME_ENG'].')';
            }
            echo $arrText['PORTAL_DAILY_TEXT_009'].@$strTitle ?>
        </div>
        <div class="form-body-90 top-20">
            <?php

                foreach ($arrCategory as $arrItem){
                    $strImgLink = SHARE_FOLDER.'/'.LINK_EDIT_FOLDER.'/'.$arrItem["LINK_NO"].'/1/'.$arrItem["BURNER_FILE_NAME1"];
                    //check file, if exists show file with source, if null show image with empty source
                    if(file_exists($strImgLink) && $arrItem["BURNER_FILE_NAME1"]){
                        $objImg = file_get_contents($strImgLink);
                        $objEncImg = base64_encode($objImg);
                        $arrImginfo = getimagesize('data:application/octet-stream;base64,' . $objEncImg);
                        $strImg = '<a target="_blank" href="'.$arrItem['URL'].'" ><img src="data:' . $arrImginfo['mime'] . ';base64,'.$objEncImg.'"></a>';
                    }else{
                        $strImg = '<img src="">';
                    }
                    echo '<div class="div-middle-line">';
                    echo '<label class="lb-left" style="width:30%;">';
                    echo '<a target="_blank" href="'.$arrItem['URL'].'">'.$arrItem["LINK_NAME".$strEndLang].'</a>';
                    echo '</label>';
                    echo $strImg;
                    echo '<span class="ip-span">';
                    echo '<a href="'.$arrItem['URL'].'">';
                    echo '</a></span>';
                    echo '</div>';
                }
            ?>
            <div class="text-right">
                <button type="submit" class="tbtn-cancel tbtn-defaut " id="close" data-dismiss="modal"><?php echo $arrText["PUBLIC_BUTTON_003"] ?></button>
            </div>
        </div>


    </div>
</div>
</body>
</html>