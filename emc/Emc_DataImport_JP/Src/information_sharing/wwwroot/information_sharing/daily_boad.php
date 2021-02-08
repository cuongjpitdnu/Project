<?php
/*
* @daily_boad.php
*
* @create 2020/02/20 KBS Tam.nv
* @update
*/
?>
    <div class="col-md-7 ">
        <?php require_once('info_form.php')?>
        <div id="map" class="map top-20 map-img"></div>

        <div class="title text-yellow mar-tl-10">
            <?php echo $arrText['PORTAL_DAILY_TEXT_008']  ?>
        </div>

        <div class="cont-table">
            <div class="row-no-gutters clearfix">
                <div class="col-md-3"><?php echo $arrText['PORTAL_DAILY_TEXT_010'] ?></div>
                <div class="col-md-3"><?php echo $arrText['PORTAL_DAILY_TEXT_011'] ?></div>
                <div class="col-md-3"><?php echo $arrText['PORTAL_DAILY_TEXT_012'] ?></div>
            </div>
            <div class="t-body-table bull_form" id="bull_form">
                <?php require_once('bull_form.php')?>

            </div>
        </div>
    </div>

    <div class="col-md-5">
        <div class="title text-yellow">
            <?php echo $arrText['PORTAL_DAILY_TEXT_002'] ?>
        </div>
        <div class="chat-cont">
            <div class="mesgs">
                <?php include ('msg_history.php')?>
            </div>
            <div class="chat-control mar-tl-10">
                <div class="in-line mar-tl-10">
                    <?php echo $arrText['PORTAL_DAILY_TEXT_006'] ?>
                    <input type="radio" name="gettime" value="0" checked>
                    <span class="pad-left-20"><?php echo $arrText['PORTAL_DAILY_TEXT_007']  ?></span>
                    <input type="radio" name="gettime" value="1">
                </div>
                <div class="in-line col-right">
                    <?php
                        //check permission to show data
                        $strDisplayStyle = "display: none;";
                        if($objLoginUserInfo->intQueryRegPerm){
                            $strDisplayStyle = "";
                        }
                    ?>
                            <button class="tbtn tbtn-defaut btn-chat" onclick="loadChat('query_view.php')" id="btn-chat"
                                    name="btnDelete" style="<?php echo $strDisplayStyle?>"><?php echo $arrText['PORTAL_DAILY_BUTTON_002']?></button>
                </div>
            </div>
        </div>

        <div class="title text-yellow mar-tl-10">
            <?php echo $arrText['PORTAL_DAILY_TEXT_009'] ?>
        </div>
        <div>
            <?php
            foreach ($arrLinkCategory as $item){
                echo '<div class="t-col-md-3">';
                echo '<a href="#" class="tbtn tbtn-defaut" onclick="loadView(\'category_detail.php\','.$item['LINK_CATEGORY_NO'].')">';
                echo fncHtmlSpecialChars($item['LINK_CATEGORY_NAME'.$strEndLang]);
                echo '</a></div>';
            }
            ?>
        </div>
    </div>

<script>
    heighMapAndBull();
    function heighMapAndBull() {
        var msg = 'map-img';
        doResize(msg,39);
        var divBull = 'bull_form';
        doResize(divBull,15);
    }

</script>
