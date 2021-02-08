<?php
/*
* @query_view.php
*
* @create 2020/02/27 KBS Tam.nv
* @update
*/
?>
<script type="text/javascript" src="js/query_chat.js"></script>
<style>
    .mesgs {
        border: 1px solid;
    }
</style>
<?php require('query_view_proc.php'); ?>
<div class="main-form">
    <div class="form-title"><?php echo $arrText['QUERY_VIEW_TEXT_001'] ?></div>
    <div class="form-body">
        <div class="mesgs" id="mesgs">
            <?php require_once('msg_history.php'); ?>
        </div>
        <div class="error-messeage" style="margin-top: 20px">
            <div style="display: none" id="er-source"></div>
            <div style="display: none" id="er-character"></div>
            <div style="display: none" id="er-target"></div>
        </div>
        <div class=" top-20">
            <?php echo $arrText['PUBLIC_TEXT_005']?>
            (<span class="txt-red"><?php echo $arrText['PUBLIC_TEXT_002']?></span>)
        </div>
        <textarea name="txt-source" id="txt-source" class="form-control" rows="2" cols="50"
                  ></textarea>
        <div class="in-line tlabel">
        	<label>
            <input type="checkbox" name="manually" value="manual" id="ckeck-manunal" >
            <?php echo $arrText['PUBLIC_TEXT_008']?>
            </label>
        </div>
        <input type="hidden" name="csrf" id="csrf" value="<?php echo @$strCsrf?>" >
        <input type="hidden" name="permError" id="permError" value="<?php echo $arrText['PUBLIC_MSG_009'] ?>" >
        <div class="in-line col-right">
            <div class="select-container">
                <select name="sle-tran" id="sle-tran">
                    <?php
                        //check permission to show dropdown list by language type
                        if($objLoginUserInfo->intLanguageType){
                            echo '<option value="ej">'.fncHtmlSpecialChars(PUBLIC_TEXT_010_ENG).'</option>
                                    <option value="je">'.fncHtmlSpecialChars(PUBLIC_TEXT_010_JPN).'</option>';
                        }else{
                            echo '<option value="je">'.fncHtmlSpecialChars(PUBLIC_TEXT_010_JPN).'</option>
                                    <option value="ej">'.fncHtmlSpecialChars(PUBLIC_TEXT_010_ENG).'</option>';
                        }
                    ?>
                </select>
            </div>
            <button type="button" class="tbtn tbtn-defaut pad-left-20" id="btn-tran">
                <?php echo $arrText['PUBLIC_BUTTON_004']?>
            </button>
        </div>
        <div class="mar-tl-10"><?php echo $arrText['PUBLIC_TEXT_005']?>
            (<span class="txt-red"><?php echo $arrText['PUBLIC_TEXT_003']?></span>)
        </div>
        <textarea name="txt-target" id="txt-target"
                  class="form-control top-20" rows="2" cols="50"
                  disabled ></textarea>

        <div class="form-footer top-20">
            <div class="in-line">
                <button type="submit" class="tbtn tbtn-defaut " onclick="postQuery()">
                    <?php echo $arrText['PUBLIC_BUTTON_005']?>
                </button>
            </div>
            <div class="in-line text-right" style="float: right">
                <button type="submit" class="tbtn-cancel tbtn-defaut " id="close"
                        data-dismiss="modal">
                    <?php echo $arrText['PUBLIC_BUTTON_003']?>
                </button>
            </div>
        </div>
    </div>
</div>



