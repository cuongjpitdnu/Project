<?php
/*
* @msg_history_form.php
*
* @create 2020/02/20 KBS Tam.nv
* @update
*/
require_once('query_view_proc.php');
//if have data will loop data and show it
if(!empty($arrQuery)){
    foreach ($arrQuery as $que){
        $mes = $que['CONTENTS'.$strEndLang];
        // if text of this language null, set text with text of other language
        if(!$mes){
            $intUntra = 1;
            // check language type
            if(!$objLoginUserInfo->intLanguageType){
                $mes = $que['CONTENTS_ENG'];
            }else{
                $mes = $que['CONTENTS_JPN'];
            }
        }
        // check user no in array ID, if in array show data
        if(in_array($que['QUERY_USER_NO'],$mapArrIds)){
            ?>
            <div class="incoming_msg">
                <div class="received_msg">
                    <div class="received_withd_msg">
                        <span class="time_date"><?php echo $arrUserName[$que['QUERY_USER_NO']]?>
                            <?php echo date('m/d H:i',strtotime($que['QUERY_DATE']))?>
                        </span>
                        <p><?php echo fncHtmlSpecialChars($mes)?></p>
                    </div>
                </div>
            </div>
            <?php
        }else{
            ?>
            <div class="outgoing_msg">
                <div class="sent_msg">
                    <span class="time_date_sent"><?php echo $arrUserName[$que['QUERY_USER_NO']]?>
                        <?php echo date('m/d H:i',strtotime($que['QUERY_DATE']))?>
                    </span>
                    <p><?php echo fncHtmlSpecialChars($mes)?></p>
                </div>
            </div>
            <?php
        }
    }
}
?>
