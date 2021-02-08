<?php
/*
* @msg_history.php
*
* @create 2020/02/20 KBS Tam.nv
* @update
*/
?>
<link rel="stylesheet" type="text/css" href="css/chat.css">
<div class="msg_history" id="load_msg_history">
    <?php require_once('msg_history_form.php'); ?>
</div>
<input type="hidden" id="reloadTime"
       value="<?php echo QUERY_RELOAD_TIME ?>">

<script>

    setHeighQuery();

    function setHeighQuery() {
        var msg = 'msg_history';
        var vph = screen.height;
        console.log(vph);
        if(vph < 900 && 1050< vph ){
            doResize(msg,64.2);
        }else if(vph < 900 && 768< vph){
            doResize(msg,62.5);
        }else if(768 >= vph ){
            doResize(msg,62.5);
        }else{
            doResize(msg,67.3);
        }
    }

    // allow 1px inaccuracy by adding 1
    $('.msg_history').stop().animate({
        scrollTop: $('.msg_history')[0].scrollHeight
    }, 1);
</script>