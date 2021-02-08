<?php
require_once('query_view_proc.php');
foreach ($listQuery as $que){
    if($objLoginUserInfo->intLanguageType){
        $mes = $que['CONTENTS_ENG'];
    }else{
        $mes = $que['CONTENTS_JPN'];
    }
    if(in_array($que['QUERY_USER_NO'],$mapArrIds)){
        ?>
        <div class="incoming_msg">
            <div class="received_msg">
                <div class="received_withd_msg">
                    <span class="time_date"><?php echo date('Y-m-d H:i:s',strtotime($que['QUERY_DATE']))?></span>
                    <p><?php echo $mes?></p>
                </div>
            </div>
        </div>
    <?php
    }else{
    ?>
        <div class="outgoing_msg">
            <div class="sent_msg">
                <span class="time_date"><?php echo date('Y-m-d H:i:s',strtotime($que['QUERY_DATE']))?></span>
                <p><?php echo $mes?></p>
            </div>
        </div>
    <?php
    }
    ?>



    <?php
}
?>
<script>
    // allow 1px inaccuracy by adding 1
    $('.msg_history').stop().animate({
        scrollTop: $('.msg_history')[0].scrollHeight
    }, 1);


</script>