<?php
    /*
     * @link_edit.php
     *
     * @create 2020/03/13 AKB Thang
     */


    require_once('common/common.php');
    require_once('common/session_check.php');
    header('Content-type: text/html; charset=utf-8');
    header('X-FRAME-OPTIONS: DENY');
    //DB接続
    if(fncConnectDB() == false){
        $_SESSION['LOGIN_ERROR'] = 'DB接続に失敗しました。';
        header('Location: login.php');
        exit;
    }
    //user not login, redirect to login.php
    if(!isset($_SESSION['LOGINUSER_INFO'])){
        echo "
        <script>
            alert('".PUBLIC_MSG_008_JPN . "/" . PUBLIC_MSG_008_ENG."');
            window.location.href = 'login.php';
        </script>
        ";
        exit();
    }

    //sesstion check
    fncSessionTimeOutCheck();


    $_SESSION['LINK_EDIT_MSG_ERROR_INPUT'] = '';
    define('SCREEN_NAME', 'リンク情報登録編集画面');
    define('SCREEN_NAME_VIEW', 'リンク情報登録編集画面 表示');
    $objUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);
    //log view
    fncWriteLog(
        LogLevel['Info'],
        LogPattern['View'],
        SCREEN_NAME_VIEW . ' (ユーザID = '.$objUserInfo->strUserID.')'
    );

    // get list text translate
    $arrTitle =  array(
        'LINK_EDIT_TEXT_001',
        'LINK_EDIT_TEXT_002',
        'LINK_EDIT_TEXT_003',
        'LINK_EDIT_TEXT_004',
        'LINK_EDIT_TEXT_005',
        'LINK_EDIT_TEXT_006',
        'LINK_EDIT_MSG_001',
        'PUBLIC_TEXT_009',
        'PUBLIC_BUTTON_003',
        'PUBLIC_BUTTON_013',
        'PUBLIC_MSG_009',
        'PUBLIC_MSG_049',

        'LINK_EDIT_MSG_010',
        'LINK_EDIT_MSG_011',
        'LINK_EDIT_MSG_014'

    );
    $arrTextTranslate = getListTextTranslate(
        $arrTitle,
        $objUserInfo->intLanguageType
    );
    //check get request
    fncGetRequestCheck($arrTextTranslate);

    // check request directly
    if(!isset($_SERVER['HTTP_REFERER'])){
        echo '<script type="text/javascript">
                function goBack() {
                    // Check to see if override is needed here
                    // If no override needed, call history.back()
                    history.go(-1);
                    return false;
                }
                alert("'.$arrTextTranslate['PUBLIC_MSG_049'].'");
                goBack();
            </script>';
        die();
    }

    //redirect if dont have permission
    if($objUserInfo->intMenuPerm == 0){
        echo "
        <script>
            alert('".$arrTextTranslate['PUBLIC_MSG_009']."');
            window.location.href = 'login.php';
        </script>
        ";
    }


    if(isset($_POST['id'])){
        //$_POST['id'] == 0, add new link
        if($_POST['id'] != 0){
            //process edit link
            $arrOneLink = fncSelectOne(
                "SELECT * FROM t_link WHERE LINK_NO=?",
                [$_POST['id']],
                SCREEN_NAME
            );
            if(!is_array($arrOneLink) || count($arrOneLink) == 0){
                //link not exist, return error msg
                $errMsg = $arrTextTranslate['LINK_EDIT_MSG_014'];

?>
<script>
	alert ("<?php echo $errMsg; ?>");
    $('#myModal').modal('hide');
    setTimeout(function() {
    	window.location.reload();
	}, 300);
</script>
<?php
                exit();
            }
            //escape html char
            foreach($arrOneLink as $key => $value){
                $arrOneLink[$key] = fncHtmlSpecialChars($value);
            }
        }
    //continue to edit link

    //get link cats
    $strSQL = "SELECT LINK_CATEGORY_NO,";
    $strSQL .= iff(
        $objUserInfo->intLanguageType,
        " LINK_CATEGORY_NAME_ENG as LINK_CATEGORY_NAME,",
        " LINK_CATEGORY_NAME_JPN as LINK_CATEGORY_NAME,"
    );
    $strSQL .= " SORT_NO";
    $strSQL .= " FROM m_link_category ORDER BY SORT_NO ASC";

    $arrLinkCategoryList= fncSelectData($strSQL, [], 1, false, SCREEN_NAME);
?>
<div class="main-content">
    <div class="main-form">
    <form class="formEdit" action="link_edit_proc.php" enctype="multipart/form-data" autocomplete="off">
        <input type="hidden" name="mode" class="t-input t-input-250" value="1">
        <input type="hidden" name="LINK_NO" class="t-input t-input-250" value="<?php
            //link no
            if(isset($arrOneLink['LINK_NO'])) echo $arrOneLink['LINK_NO'];
        ?>">
        <input type="hidden" name="X-CSRF-TOKEN" value="<?php
            //csrf token
            if(isset($_SESSION['csrf'])) echo $_SESSION['csrf'];
        ?>">
        <div class="form-title">
            <?php echo $arrTextTranslate['LINK_EDIT_TEXT_001']; ?>
        </div>
        <div class="edit-error" style="color:red;"></div>
        <div class="form-body-90 top-20">
            <div class="lb-left-long">
                <?php echo $arrTextTranslate['LINK_EDIT_TEXT_002']; ?>
            </div>
            <div class="select-container" style="margin-left: 15px">
                <select name="LINK_CATEGORY_NO">
                    <?php echo !isset($arrOneLink['LINK_CATEGORY_NO']) ? '<option value="">' : '' ?>
                    <?php foreach($arrLinkCategoryList as $arrLinkCategory){
                        //show list category link
                        echo '<option value="'.$arrLinkCategory['LINK_CATEGORY_NO'].'" '
                        .(isset($arrOneLink['LINK_CATEGORY_NO'])
                        && $arrOneLink['LINK_CATEGORY_NO']==$arrLinkCategory['LINK_CATEGORY_NO']
                        ? 'selected': '').'>'
                        .$arrLinkCategory['LINK_CATEGORY_NAME']
                        .'</option>';
                    } ?>
                </select>
            </div><br/>

            <div class="lb-left-long">
                <?php echo $arrTextTranslate['LINK_EDIT_TEXT_003']; ?>
            </div>
            <span class="ip-span">
                <input type="text" name="LINK_NAME_JPN" class="t-input" value="<?php
                    echo isset($arrOneLink['LINK_NAME_JPN']) ? $arrOneLink['LINK_NAME_JPN'] : '';
                ?>">
            </span>

            <div class="lb-left-long">
                <?php echo $arrTextTranslate['LINK_EDIT_TEXT_004']; ?>
            </div>
            <span class="ip-span">
                <input type="text" name="LINK_NAME_ENG" class="t-input" value="<?php
                echo isset($arrOneLink['LINK_NAME_ENG']) ? $arrOneLink['LINK_NAME_ENG'] : '';
                ?>">
            </span>

            <div class="lb-left-long">
                <?php echo $arrTextTranslate['LINK_EDIT_TEXT_005']; ?>
            </div>
            <span class="ip-span">
                <input type="text" name="URL" class="t-input" value="<?php
                    echo isset($arrOneLink['URL']) ? $arrOneLink['URL'] : '';
                ?>">
            </span>

            <div class="lb-left-long">
                <?php echo $arrTextTranslate['LINK_EDIT_TEXT_006']; ?>
            </div>
            <div class="upload-file ip-span">

                <?php if(
                    isset($arrOneLink['BURNER_FILE_NAME1'])
                    && $arrOneLink['BURNER_FILE_NAME1'] != ''
                ){
                    //file path
                    $strPathTmp = SHARE_FOLDER . '/' . LINK_EDIT_FOLDER . '/'
                    . $arrOneLink['LINK_NO'] . '/1/' . $arrOneLink['BURNER_FILE_NAME1'];
                ?>
                <div class="singe-line" style="border-bottom: none;">
                    <div class="in-line">
                        <a <?php echo (mb_strlen($arrOneLink['BURNER_FILE_NAME1']) >=60
                        ? 'style="word-wrap:break-word;"' : ''); ?>
                        <?php echo isset($arrOneLink['BURNER_FILE_NAME1'])
                        && $arrOneLink['BURNER_FILE_NAME1'] != ''
                        ? 'href="#"'.(!file_exists(
                                SHARE_FOLDER.'/'.LINK_EDIT_FOLDER.'/'
                                .$arrOneLink['LINK_NO'] . '/1/' . $arrOneLink['BURNER_FILE_NAME1']
                            ) ? ' class="no-file"'
                            : ' data-id="'.base64_encode($strPathTmp).'" class="download-file"')
                        : '' ?> class="link"
                        ><?php echo isset($arrOneLink['BURNER_FILE_NAME1'])
                        ? $arrOneLink['BURNER_FILE_NAME1'] : ''; ?></a>
                    </div>


                    <div class="in-line col-right right-20">
                        <input type="checkbox" name="chkDelete">
                        <?php echo $arrTextTranslate['PUBLIC_TEXT_009']; ?>
                    </div>
                    <div style="clear:both;"></div>

                </div>
                <?php } ?>
                <div class="singe-line">
                    <input type="file" name="file" id="file-upload" style="width:100%"
                    class="t-input" <?php
                        echo isset($arrOneLink['BURNER_FILE_NAME1'])
                        && $arrOneLink['BURNER_FILE_NAME1']!='' ? 'disabled' : '';
                        ?>>
                </div>
            </div>

            <div class="form-footer top-20 text-right">
            <button type="submit" class="tbtn-cancel tbtn-defaut btn-save-edit-file">
                <?php echo $arrTextTranslate['PUBLIC_BUTTON_013']; ?>
            </button>
            <button type="submit" class="tbtn-cancel tbtn-defaut"
            id="close-setting" data-dismiss="modal">
                <?php echo $arrTextTranslate['PUBLIC_BUTTON_003']; ?>
            </button>
            </div>

        </div>
    </form>
    </div>
</div>
<canvas id="canvas1" style="display: none;"></canvas>
<form action="link_edit_proc.php" method="post" id="formDownload">
    <input type="hidden" name="X-CSRF-TOKEN" value="<?php
            //csrf token
            if(isset($_SESSION['csrf'])) echo $_SESSION['csrf'];
        ?>" />
    <input type="hidden" name="path" value="">
    <input type="hidden" name="mode" value="99">
</form>
<div class="edit-error2" style="display:none"></div>
<style>
    table.blueTable {
        width: 100%;
    }
</style>
<script>
    $('[name="chkDelete"]').click(function(){
        var t = $(this).prop('checked');
        if(t){
            $('[name="file"]').attr('disabled', false);
        }else{
            $('[name="file"]').attr('disabled', true);
        }
    });

    $('#close-setting').on('click', function(e) {
        setTimeout(function() {
            window.location.reload();
        }, 300);
    });

    var confirmMsg = '<?php echo $arrTextTranslate['LINK_EDIT_MSG_001']; ?>';
    var fileNotExistMessage = '<?php
    echo $arrTextTranslate['LINK_EDIT_MSG_010']; ?>';
    $('.no-file').click(function(e){
        e.preventDefault();
        $('.edit-error').html(fileNotExistMessage);

    });

    $('.btn-save-edit-file').click(function(e){
        $('.edit-error').html('');
        $('.edit-error2').html('');
		e.preventDefault();
        setTimeout(function(){
		t = confirm(confirmMsg);
		if(!t){
			return;
		}
		var formElement = document.querySelector(".formEdit");
		var formData = new FormData(formElement);

        var url = $('.formEdit').attr('action');

        var listMsg  = [];

		$('input[name^=file]').each(function(i, e) {
			if($(this).val() != '') {
				var name = $(this).attr('name');
				var file = $(this).prop('files')[0];
				checkFileExist(name, file, listMsg);
			}
		});

		if(listMsg.length > 0) {
			$.each(listMsg, function(i, e) {
				$('.edit-error2').append('<div>'+e+'</div>');
            });
            formData = $('.formEdit').serializeArray();
            formData.push({name: 'errFile', value: '1'});
            $.ajax({
                url: url,
                type: 'post',
                data: formData,
                success: function(result){
                    try{
                        eval(result);
                    }catch(err){

                    }

                    $('.edit-error').append($('.edit-error2').html());
                }
            });
		}else{
            $.ajax({
                url: url,
                type: 'post',
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                async: false,
                success: function(result){
                    if(result!=0 && result != 1){
                        eval(result);
                    }else{
                        window.location.reload();
                        return;
                    }
                }
            });
        }

        },15);

    });
    function checkFileExist(name, file, listMsg) {
        var arrMsg = {
            'file': '<?php echo $arrTextTranslate['LINK_EDIT_MSG_010']; ?>'
        }
        if(file['size'] == 0) {
            listMsg.push(arrMsg[name]);
            return false;
        }
        return true;
    }
    $('.download-file').on('click', function(e) {
        e.preventDefault();
        $('.error').html('');
        var path = $(this).attr('data-id');
        var arr = [
            { name: 'X-CSRF-TOKEN',
                value: '<?php if(isset($_SESSION['csrf'])) echo $_SESSION['csrf']; ?>'},
            { name: 'file', value: $(this).attr('data-id') },
            { name: 'action', value: 'checkFile' }
        ];
        var blnCanDownload = false;
        $.ajax({
            url: 'link_edit_proc.php',
            type: 'POST',
            async: false,
            data: arr,
            success: function(result) {
                console.log(result);
                if($.trim(result) == 'window.location.href="login.php";') {
                    window.location.href="login.php";
                    return;
                }
                if(result == 0) {
                    $('.edit-error').html('<?php
                        echo $arrTextTranslate['LINK_EDIT_MSG_010'];
                    ?>');
                } else {
                    blnCanDownload = true;
                }
                if(blnCanDownload) {
                    $('input[name=path]').val(path);

                    $('form#formDownload').submit();
                }
            },
            error: function(e) {
                console.log(e);
            }
        });

        return;
    });
</script>
<?php
    }else{
        echo "
        <script>
            alert('".PUBLIC_MSG_008_JPN."/".PUBLIC_MSG_008_ENG."');
            window.location.href = 'login.php';
        </script>
        ";
        exit();
    }
?>