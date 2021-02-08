<?php
    /*
    * @information_edit.php
    *
    * @create 2020/02/20 AKB Thang
    * @update
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
    if(!isset($_SESSION['LOGINUSER_INFO'])){
        echo "
        <script>
            alert('".PUBLIC_MSG_008_JPN . "/" . PUBLIC_MSG_008_ENG."');
            window.location.href = 'login.php';
        </script>
        ";

        exit();
    }

    $_SESSION['INFORMATION_EDIT_MSG_ERROR_INPUT'] = '';

    //constant
    define('SCREEN_NAME', '情報登録編集画面');
    define('SCREEN_NAME_VIEW', '情報登録編集画面 表示');
    define('TOTAL_FILE_SIZE', 10485760);
    //get object login user
    $objUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);


    // get list text translate
    $arrTitle =  array(
        'INFORMATION_EDIT_TEXT_001',
        'INFORMATION_EDIT_TEXT_002',
        'INFORMATION_EDIT_TEXT_003',
        'INFORMATION_EDIT_TEXT_004',
        'INFORMATION_EDIT_TEXT_005',
        'INFORMATION_EDIT_TEXT_006',
        'INFORMATION_EDIT_TEXT_007',
        'PUBLIC_TEXT_004',
        'PUBLIC_TEXT_005',
        'PUBLIC_TEXT_002',
        'PUBLIC_TEXT_008',
        'PUBLIC_TEXT_009',
        'PUBLIC_BUTTON_003',
        'PUBLIC_BUTTON_004',
        'PUBLIC_BUTTON_005',
        'PUBLIC_MSG_009',
        'INFORMATION_EDIT_MSG_002',
        'INFORMATION_EDIT_MSG_003',
        'PUBLIC_TEXT_003',
        'PUBLIC_MSG_020',
        'PUBLIC_MSG_011',
        'PUBLIC_MSG_012',
        'PUBLIC_MSG_013',
        'PUBLIC_MSG_014',
        'PUBLIC_MSG_015',
        'PUBLIC_MSG_016',
        'PUBLIC_MSG_041',
        'INFORMATION_EDIT_MSG_001',
        'PUBLIC_TEXT_017',
        'PUBLIC_MSG_049',

        'PUBLIC_MSG_019',

        //2020/04/27 T.Masuda 既に削除された情報
        'INFORMATION_EDIT_MSG_011',
        //2020/04/27 T.Masuda
    );
    // get list text(header, title, msg) with languague_type of user logged
    $arrTextTranslate = getListTextTranslate(
        $arrTitle,
        $objUserInfo->intLanguageType
    );
    //session check
    fncSessionTimeOutCheck();
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
    if($objUserInfo->intInformationRegPerm == 0 && $objUserInfo->intMenuPerm == 0){
        echo "
        <script>
            alert('".$arrTextTranslate['PUBLIC_MSG_009']."');
            window.location.href = 'login.php';
        </script>
        ";
    }
    //view log
    fncWriteLog(
        LogLevel['Info'],
        LogPattern['View'],
        SCREEN_NAME_VIEW . ' (ユーザID = '.$objUserInfo->strUserID.') '
        .(isset($_SERVER['HTTP_REFERER']) ? $_SERVER['HTTP_REFERER'] : null)
    );
    if(isset($_POST['id'])){
        if($_POST['id'] != 0){
            //process edit information
            $arrOneInfo = fncSelectOne(
                "SELECT * FROM t_information WHERE INFORMATION_NO=?",
                [$_POST['id']],
                SCREEN_NAME
            );
            if(!(is_array($arrOneInfo) && count($arrOneInfo))){
                //information not exist, return error msg

?>
<script>
    alert('<?php echo $arrTextTranslate['INFORMATION_EDIT_MSG_011']; ?>');
    loadPortalClose();
    $('#myModal').modal('hide');

</script>
<?php
            exit();
        }
        //escape html char
        foreach($arrOneInfo as $key => $value){
            $arrOneInfo[$key] = fncHtmlSpecialChars($value);
        }
        //escape html char
        foreach($arrTextTranslate as $key => $value){
            $arrTextTranslate[$key] = fncHtmlSpecialChars($value);
        }
    }
?>

<style>
    input[type=file] {
    display: inline-block;
    }
    .style_real_display {
        white-space: pre-wrap;
        white-space: -moz-pre-wrap;
        white-space: -pre-wrap;
        white-space: -o-pre-wrap;
        word-wrap: break-word;
    }
    .fl-w-630 {
        float: left;
        width: 630px;
    }
</style>
<div class="main-content">
    <div class="main-form">
        <form class="formEdit" action="information_edit_proc.php" enctype="multipart/form-data" autocomplete="off">
        <input type="hidden" name="mode" class="t-input t-input-250" value="1">
        <input type="hidden" name="auto_translation"
        class="t-input t-input-250" value="1">
        <input type="hidden" name="INFORMATION_NO"
        class="t-input t-input-250" value="<?php
            if(isset($arrOneInfo['INFORMATION_NO'])) echo $arrOneInfo['INFORMATION_NO'];
        ?>">
        <input type="hidden" name="INCIDENT_CASE_NO"
        class="t-input t-input-250" value="<?php
            if(isset($_POST['dataId'])) echo $_POST['dataId'];
        ?>">
        <input type="hidden" name="X-CSRF-TOKEN" value="<?php
            if(isset($_SESSION['csrf'])) echo $_SESSION['csrf'];
        ?>">
        <div class="form-title">
            <?php echo $arrTextTranslate['INFORMATION_EDIT_TEXT_001']; ?>
        </div>
        <div class="edit-error" style="color:red;"></div>
        <div class="form-body">
            <div class="text-right" style="margin-bottom: 10px;">
                <div class="select-container" >

                    <select name="LANGUAGE_TYPE">
                        <?php if(isset($arrOneInfo['LANGUAGE_TYPE'])){

                        ?>
                        <option value="ja"
                            <?php echo ($arrOneInfo['LANGUAGE_TYPE'] == 0
                            ? "selected": ""); ?>><?php echo PUBLIC_TEXT_010_JPN; ?>
                        </option>
                        <option value="en"
                            <?php echo ($arrOneInfo['LANGUAGE_TYPE'] == 1
                            ? "selected": ""); ?>><?php echo PUBLIC_TEXT_010_ENG; ?>
                        </option>

                        <?php }
                        else{
                            ?>
                        <option value="ja"
                        <?php echo (!$objUserInfo->intLanguageType ? "selected": ""); ?>>
                            <?php echo PUBLIC_TEXT_010_JPN; ?>
                        </option>
                        <option value="en"
                        <?php echo ($objUserInfo->intLanguageType ? "selected": ""); ?>>
                            <?php echo PUBLIC_TEXT_010_ENG; ?>
                        </option>
                            <?php
                        }
                        ?>
                    </select>

                </div>
            </div>

            <fieldset class="fi-yellow1">
            <p style="background-color:#4169e1">
                 <legend><?php echo $arrTextTranslate['PUBLIC_TEXT_002']; ?></legend>
                </p>

                <div>
                    <?php echo PUBLIC_TEXT_004_JPN . '/' .  PUBLIC_TEXT_004_ENG; ?>
                </div>
                <input type="text" name="titleSource[]" class="form-control" value="<?php
                    if(isset($arrOneInfo['LANGUAGE_TYPE']))
                    echo ($arrOneInfo['LANGUAGE_TYPE']
                    ? $arrOneInfo['TITLE_ENG'] : $arrOneInfo['TITLE_JPN']);
                ?>">
                <div>
                    <?php echo PUBLIC_TEXT_005_JPN . '/' .  PUBLIC_TEXT_005_ENG; ?>
                </div>
                <textarea name="contentSource[]" class="form-control" rows="2" cols="50"><?php
                    if(isset($arrOneInfo['LANGUAGE_TYPE']))
                    echo ($arrOneInfo['LANGUAGE_TYPE']
                    ? $arrOneInfo['CONTENTS_ENG'] : $arrOneInfo['CONTENTS_JPN']);
                ?></textarea>
            </fieldset>

            <div class="clearfix mar-tl-10">
                <div class="in-line">
                	<label for="tranChkBox">
                        <input type="checkbox" name="tranChkBox" id="tranChkBox">
                        <?php echo $arrTextTranslate['PUBLIC_TEXT_008']; ?>
                    </label>
                </div>
                <div class="in-line col-right">
                    <button class="tbtn tbtn-defaut tran-btn">
                        <?php echo $arrTextTranslate['PUBLIC_BUTTON_004']; ?>
                    </button>
                </div>
            </div>

            <fieldset class="fi-pink1">
                <p style="background-color:#4169e1">
                 <legend><?php echo $arrTextTranslate['PUBLIC_TEXT_003']; ?></legend>
                </p>
                <div >
                    <?php echo PUBLIC_TEXT_004_JPN . '/' .  PUBLIC_TEXT_004_ENG; ?>
                </div>
                <input type="text" name="titleSource[]"
                class="form-control trans" value="<?php
                    if(isset($arrOneInfo['LANGUAGE_TYPE']))
                    echo ($arrOneInfo['LANGUAGE_TYPE']
                    ? $arrOneInfo['TITLE_JPN']
                    : $arrOneInfo['TITLE_ENG']); ?>" disabled>

                <div >
                    <?php echo PUBLIC_TEXT_005_JPN . '/' .  PUBLIC_TEXT_005_ENG; ?>
                </div>
                <textarea name="contentSource[]" class="form-control trans"
                rows="2" cols="50" disabled><?php
                    if(isset($arrOneInfo['LANGUAGE_TYPE']))
                    echo ($arrOneInfo['LANGUAGE_TYPE']
                    ? $arrOneInfo['CONTENTS_JPN']
                    : $arrOneInfo['CONTENTS_ENG']); ?></textarea>

            </fieldset>

            <div class="top-20">
                <div for="firstname" class="label-input text-link">
                    <?php echo $arrTextTranslate['INFORMATION_EDIT_TEXT_002']; ?>
                </div>
                <input type="text" class="t-input" name="CONFIRM_DATE"
                style="width:120px;" value="<?php echo isset($arrOneInfo['CONFIRM_DATE'])
                        ? date_format(
                            date_create($arrOneInfo['CONFIRM_DATE']),
                            'Y/m/d H:i'
                        )
                        : date('Y/m/d H:i') ; ?>" /><br/>

                <div for="firstname" class="label-input text-link">
                    <?php echo $arrTextTranslate['INFORMATION_EDIT_TEXT_003']; ?>
                </div>
                <input type="text" style="width:120px;" class="t-input" name="CONTACT_INFO"
                value="<?php if(isset($arrOneInfo['CONTACT_INFO']))
                echo $arrOneInfo['CONTACT_INFO']; ?>" /><br />
            </div>
            <div>
                <div class="tcol-md-6 in-line float-left">
                    <div for="firstname" class="label-short text-link">
                        <?php echo $arrTextTranslate['INFORMATION_EDIT_TEXT_004']; ?>
                    </div>
                    <div class="select-container">
                        <?php
                            //get all company
                            if(isset($arrOneInfo['COMPANY_NO'])){
                                $intCompanyNo = $arrOneInfo['COMPANY_NO'];
                            }else{
                                $intCompanyNo = $objUserInfo->intCompanyNo;
                            }
        
                            $strLanguageCondition = $objUserInfo->intLanguageType == 0
                            ? 'ABBREVIATIONS_JPN as ABBREVIATIONS'
                            : 'ABBREVIATIONS_ENG as ABBREVIATIONS';
                            
                            $arrCompanyList = fncSelectData(
                                "SELECT COMPANY_NO,
                                ".$strLanguageCondition."
                                FROM m_company
                                WHERE del_flag = 0
                                ORDER BY SORT_NO ASC",
                                [],
                                1,
                                false,
                                SCREEN_NAME
                            );
                            //get user admin_flag
                            $arrUserTmp = fncSelectOne(
                                "SELECT m_group.ADMIN_FLAG
                                FROM m_user
                                INNER JOIN m_company ON m_company.COMPANY_NO = m_user.COMPANY_NO
                                INNER JOIN m_group ON m_group.GROUP_NO = m_company.GROUP_NO
                                WHERE m_user.USER_NO=?",
                                [$objUserInfo->intUserNo],
                                SCREEN_NAME
                            );
                            if(is_array($arrUserTmp) && count($arrUserTmp)){
                                $blnAdminFlag = $arrUserTmp['ADMIN_FLAG'];
                            }else{
                                $blnAdminFlag = 0;
                            }
                        ?>
                        <select name="COMPANY_NO" <?php echo $objUserInfo->intMenuPerm !=1 ? 'disabled ' : ''; ?>>
                        	<option value=""
                        	<?php if($objUserInfo->intMenuPerm == 1 && $_POST['id'] == 0){
                        	    echo 'selected';
                        	}?>
							</option>
                            <?php foreach($arrCompanyList as $arrCompany){ ?>
                                <option value="<?php echo $arrCompany['COMPANY_NO']; ?>"
                                <?php if($objUserInfo->intMenuPerm != 1 || $_POST['id'] != 0){
                                     echo iff($intCompanyNo == $arrCompany['COMPANY_NO'], 'selected', '');
								}?>
                                >
                                    <?php echo fncHtmlSpecialChars($arrCompany['ABBREVIATIONS']); ?>
                                </option>
                            <?php } ?>
                        </select>
                    </div>
                </div>
                <div class="tcol-md-6 in-line float-left">
                    <div for="firstname" class="label-short text-link">
                        <?php echo $arrTextTranslate['INFORMATION_EDIT_TEXT_005']; ?>
                    </div>
                    <input type="text" class="tbtn" id="aj1" name="" value="" disabled/><br />
                </div>
            </div>

            <div for="firstname" class="label-short text-link">
                <?php echo $arrTextTranslate['INFORMATION_EDIT_TEXT_006']; ?>
            </div>
            <div class="select-container">
                <?php
                    
                    if(isset($arrOneInfo['INFO_CATEGORY_NO'])){
                        $intInfoCategoryNo = $arrOneInfo['INFO_CATEGORY_NO'];
                    }
                    
                    //get all info_category
                    $strLanguageCondition = $objUserInfo->intLanguageType == 0
                    ? 'INFO_CATEGORY_NAME_JPN as INFO_CATEGORY_NAME'
                    : 'INFO_CATEGORY_NAME_ENG as INFO_CATEGORY_NAME';

                    $arrInfoCategoryList = fncSelectData(
                        "SELECT INFO_CATEGORY_NO,
                        ".$strLanguageCondition."
                        FROM m_info_category
                        ORDER BY SORT_NO ASC",
                        [],
                        1,
                        false,
                        SCREEN_NAME
                    );
                ?>
                <select name="INFO_CATEGORY_NO">
                    <?php echo !isset($intInfoCategoryNo) ? '<option value=""></option>' : ''; ?>
                    <?php if(is_array($arrInfoCategoryList))
                    foreach($arrInfoCategoryList as $arrInfoCategory){ ?>
                        <option value="<?php echo $arrInfoCategory['INFO_CATEGORY_NO']; ?>"
                        <?php echo isset($intInfoCategoryNo)
                        ? iff(
                                $intInfoCategoryNo == $arrInfoCategory['INFO_CATEGORY_NO'],
                                'selected',
                                ''
                            )
                        : ''; ?>
                        >
                            <?php echo fncHtmlSpecialChars($arrInfoCategory['INFO_CATEGORY_NAME']);  ?>
                        </option>
                    <?php } ?>

                </select>
            </div>

            <p style="background-color:#4169e1">
                <legend>
                    <?php echo $arrTextTranslate['INFORMATION_EDIT_TEXT_007']; ?>
                </legend>
            </p>
            <table class="blueTable requestTable">
                <?php for($intLoop =1; $intLoop<=5; $intLoop++){ ?>
                    <tr>
                        <td rowspan="2" width="60px" align="center">
                            <div class="singe-line">
                                <?php echo $intLoop; ?>
                            </div>
                        </td>
                        <?php if(isset($arrOneInfo['TMP_FILE_NAME'.$intLoop])
                        && $arrOneInfo['TMP_FILE_NAME'.$intLoop] != ''){
                            $strPathTmp = SHARE_FOLDER . '/' . INFORMATION_ATTACHMENT_FOLDER . '/'
                            . $arrOneInfo['INFORMATION_NO'] . '/'.$intLoop.'/'
                            . $arrOneInfo['TMP_FILE_NAME'.$intLoop];
                        ?>
                        <td>
                            <div class="singe-line">
                                <div class="in-line">
                                    <a <?php echo 'href="#"'
                                    . (!file_exists(SHARE_FOLDER . '/' . INFORMATION_ATTACHMENT_FOLDER
                                    . '/' . $arrOneInfo['INFORMATION_NO'] . '/'.$intLoop.'/'
                                    . $arrOneInfo['TMP_FILE_NAME'.$intLoop])
                                    ? ' class="no-file fl-w-630 style_real_display"'
                                    : ' data-id="'.base64_encode($strPathTmp).'" class="download-file fl-w-630 style_real_display"')
                                    ?> class="link"><?php
                                        echo $arrOneInfo['TMP_FILE_NAME'.$intLoop];
                                    ?></a>
                                </div>

                                <div class="in-line col-right" style="width:132px;">
                                	<label for="chkDelete<?php echo $intLoop; ?>">
                                        <input type="checkbox" name="chkDelete<?php echo $intLoop; ?>"  id="chkDelete<?php echo $intLoop; ?>" class="chkDelete">
                                        <?php echo $arrTextTranslate['PUBLIC_TEXT_009']; ?>
                                        <input type="hidden" class="pf" value="<?php
                                        echo $arrOneInfo['PUBIC_FLAG'.$intLoop] == 1 ? "1" : "0"; ?>">
                                    </label>
                                </div>
                                <div style="clear:both;"></div>

                            </div>

                        </td>
                        <?php }else{

                            echo '<td style="display:none;">
                            <input type="checkbox" class="chkDelete">
                            <input type="hidden" class="pf" value="0">
                            </td>';


                        } ?>
                    </tr>
                    <tr>
                        <td>
                        <div class="singe-line">
                                <input type="file" style="width:80%" name="file<?php echo $intLoop; ?>"
                                class="file-upload" class="t-input" <?php
                                    echo isset($arrOneInfo['TMP_FILE_NAME'.$intLoop])
                                    && $arrOneInfo['TMP_FILE_NAME'.$intLoop]!='' ? 'disabled' : '';
                                ?> >
                                <?php if(isset($arrOneInfo['TMP_FILE_NAME'.$intLoop])
                                && $arrOneInfo['TMP_FILE_NAME'.$intLoop] != ''){ ?>
                                    <div class="in-line col-right" style="width:132px;">
                                        <label for="PUBIC_FLAG<?php echo $intLoop; ?>">
                                            <input type="checkbox" name="PUBIC_FLAG<?php echo $intLoop; ?>" id="PUBIC_FLAG<?php echo $intLoop; ?>" class="chkPublic"
                                            <?php echo $arrOneInfo['PUBIC_FLAG'.$intLoop] == 1 ? "checked" : ""; ?>>
                                            <?php echo $arrTextTranslate['PUBLIC_TEXT_017']; ?>
                                        </label>
                                    </div>
                                <?php }else{ ?>
                                    <div class="in-line col-right" style="display:none;width:132px;">
                                    	<label for="PUBIC_FLAG<?php echo $intLoop; ?>">
                                            <input type="checkbox" name="PUBIC_FLAG<?php echo $intLoop; ?>" id="PUBIC_FLAG<?php echo $intLoop; ?>" class="chkPublic">
                                            <?php echo $arrTextTranslate['PUBLIC_TEXT_017']; ?>
                                        </label>

                                    </div>
                                <?php } ?>
                            </div>
                        </td>
                    </tr>
                <?php } ?>

            </table>

            <div class="form-footer top-20">
                <div class="in-line">
                    <button type="submit" class="tbtn tbtn-defaut btn-save" >
                        <?php echo $arrTextTranslate['PUBLIC_BUTTON_005']; ?>
                    </button>
                </div>
                <div class="in-line text-right" style="float: right">
                    <button class="tbtn-cancel tbtn-defaut " id="close">
                        <?php echo $arrTextTranslate['PUBLIC_BUTTON_003']; ?>
                    </button>
                </div>
            </div>

        </div>
        </form>
    </div>
</div>
<div class="edit-error2" style="display:none;"></div>
<form action="information_edit_proc.php" method="post" id="formDownload">
    <input type="hidden" name="X-CSRF-TOKEN" value="<?php
            if(isset($_SESSION['csrf'])) echo $_SESSION['csrf'];
        ?>" />
    <input type="hidden" name="path" value="">
    <input type="hidden" name="mode" value="99">
</form>
<style>
    table.requestTable td, table.requestTable th {
        border: 1px solid #97999a;
        background: white;
    }
    .singe-line {
        border: none;
        padding: 15px;
        word-break: break-word;
    }
    fieldset{
        padding: 10px !important;
        border-color: threedface !important;
        border-image: initial !important;
        border-width: 2px;
    }
    input[type=file] {
        display: inline-block;
    }
    legend{
        color:white;
        padding: 0px 15px;
    }
    .label-short {
        width: 130px;

    }
    table.blueTable {
        width: 100%;
    }
    *{
        word-break: break-word;
        -ms-word-wrap: break-word;
    }
</style>
<script>
    var fileNotExistMessage = '<?php
    echo $arrTextTranslate['INFORMATION_EDIT_MSG_002'] ?>';
    <?php if(isset($arrOneInfo['INFORMATION_NO'])){ ?>
    $('#close').on('click', function(e){
        e.preventDefault();
        loadView("information_view.php", <?php if(isset($arrOneInfo['INFORMATION_NO']))
        echo $arrOneInfo['INFORMATION_NO']; ?>);
    });
    <?php }else{ ?>
    $('#close').on('click', function(e){
        e.preventDefault();
        loadPortalClose();
        $('#myModal').modal('hide');
    });

    <?php } ?>
    $('.no-file').on('click', function(e){
        e.preventDefault();
        $('.edit-error').html(fileNotExistMessage);
    });
    $("[name='tranChkBox']").on('click', function(e){
        if($(this).prop('checked') == true){
            $('.trans').attr('disabled', false);
            $('.tran-btn').prop('disabled', true);
        }else{
            $('.trans').attr('disabled', true);
            $('.tran-btn').prop('disabled', false);
        }
    });
    $("[name='COMPANY_NO']").change(function(){
        var companyNo = $(this).val();
        $.ajax({
			url: 'information_edit_proc.php',
			type: 'post',
			data: [
                {
                    name: 'X-CSRF-TOKEN',
                    value: '<?php if(isset($_SESSION['csrf'])) echo $_SESSION['csrf']; ?>'
                },
				{name: 'mode', value: 3},
				{name: 'company_no', value: companyNo}
            ],
            dataType: 'json',
			success: function(result){
				if(result == 900){
					alert('<?php echo $arrTextTranslate['PUBLIC_MSG_009'] ?>');
					window.location.href="login.php";
                    return;
				}
                $('#aj1').val(result[0]);
			}
		});
    });
    $("[name='COMPANY_NO']").change();
    $(".tran-btn").on('click', function(e){
        $('.edit-error').html('');
        $('.edit-error2').html('');
        e.preventDefault();
        $('.edit-error').html('');
        if($("[name='tranChkBox']").prop("checked") == true){
            return;
        }else{

            var titleTran = $(".fi-pink1 [name='titleSource[]']").val();
            var contentTran = $(".fi-pink1 [name='contentSource[]']").val();

            if((titleTran != '') || (contentTran != '')){
                return;
            }else{

                var data = [
                        {name: 'title', value: $(".fi-yellow1 [name='titleSource[]']").val()},
                        {name: 'content', value: $(".fi-yellow1 [name='contentSource[]']").val()},
                        {name: 'title1', value: $(".fi-pink1 [name='titleSource[]']").val()},
                        {name: 'content1', value: $(".fi-pink1 [name='contentSource[]']").val()},
                        {name: 'X-CSRF-TOKEN', value: $('meta[name="csrf-token"]').attr('content')},
                        {name: 'mode', value: 2},
                        {name: 'LANGUAGE_TYPE', value: $('[name="LANGUAGE_TYPE"]').val()}
                    ];
                $.ajax({
                    url: 'information_edit_proc.php',
                    type: 'post',
                    data: data,
                    success: function(result){
                    	if(result == 900){
    						alert('<?php echo $arrTextTranslate['PUBLIC_MSG_009'] ?>');
    						window.location.href="login.php";
    	                    return;
    					}
                        try{
                            result = JSON.parse(result);
                            $(".fi-pink1 [name='titleSource[]']").val(result[0]);
                            $(".fi-pink1 [name='contentSource[]']").val(result[1]);
                        }catch(err){
                            eval(result);
                        }
                    }
                });
            }
        }
    });
    var confirmAmazoneContinueMSG = '<?php
    echo $arrTextTranslate['PUBLIC_MSG_041'] ?>';
    var confirmMsg = '<?php echo $arrTextTranslate['INFORMATION_EDIT_MSG_003'] ?>';
    $('.btn-save').on('click', function(e){
        e.preventDefault();
        $('.edit-error').html('');
        $('.edit-error2').html('');

        var total_size = 0;
        $('input[type=file]').each(function(){
            if($(this).val()){
                var file = $(this).prop('files')[0];
                total_size = total_size + file.size;
            }
        });

        if( total_size > <?php echo TOTAL_FILE_SIZE ; ?>){
            $('.edit-error').html('<?php echo $arrTextTranslate['PUBLIC_MSG_019']; ?><br>');
            $('#myModal').animate({ scrollTop: 0 }, '10');
            return;
        }

        var intTranCheck = 0;
        if($("[name='tranChkBox']").prop("checked") == true){
        	intTranCheck = 1;
        }

            var data = [
                    {name: 'title', value: $(".fi-yellow1 [name='titleSource[]']").val()},
                    {name: 'content', value: $(".fi-yellow1 [name='contentSource[]']").val()},
                    {name: 'title1', value: $(".fi-pink1 [name='titleSource[]']").val()},
                    {name: 'content1', value: $(".fi-pink1 [name='contentSource[]']").val()},
                    {name: 'X-CSRF-TOKEN', value: $('meta[name="csrf-token"]').attr('content')},
                    {name: 'mode', value: 2},
                    {name: 'LANGUAGE_TYPE', value: $('[name="LANGUAGE_TYPE"]').val()},
                    {name: 'save', value: 1},
                    {name: 'manualTranslate', value: intTranCheck},
                    {name: 'INFORMATION_NO', value: $('[name="INFORMATION_NO"]').val()},
                    {name: 'CONFIRM_DATE', value: $('[name="CONFIRM_DATE"]').val()},
                    {name: 'CONTACT_INFO', value: $('[name="CONTACT_INFO"]').val()},
                    {name: 'INFO_CATEGORY_NO', value: $('[name="INFO_CATEGORY_NO"]').val()},
                    {name: 'COMPANY_NO', value: $('[name="COMPANY_NO"]').val()}
                ];

        	var strTitleTrans = $(".fi-pink1 [name='titleSource[]']").val().trim();
            var strContetTrans = $(".fi-pink1 [name='contentSource[]']").val().trim();

            var intAutoTran = 0;
            if(strTitleTrans == '' && strContetTrans == '' && intTranCheck == 0){
            	intAutoTran = 1;
            }

            $.ajax({
                url: 'information_edit_proc.php',
                type: 'post',
                data: data,
                success: function(result){
					if(result == 900){
						alert('<?php echo $arrTextTranslate['PUBLIC_MSG_009'] ?>');
						window.location.href="login.php";
	                    return;
					}
                    if(result == 1){
                        $("[name='auto_translation']").val(1);
                        t = false;
                    }else{
                        $("[name='auto_translation']").val(0);
                        if(result == 2){
                            t = confirm(confirmAmazoneContinueMSG);
                            if(!t) return;
                        }else{
                            if(strTitleTrans !='' || strContetTrans != ''){
                                eval(result);
                            }else{
                                try{
                                    result = JSON.parse(result);
                                    strTitleTrans = result[0];
                                    strContetTrans = result[1];
                                }catch(err){
                                	eval(result);
                                }

                            }
                            t=false;
                        }
                    }

                    var htmlErr = $('.edit-error').html();
                    if(htmlErr.trim() == ''){
                        if (!t) {
                            t2 = confirm(confirmMsg);
                            if(!t2){
                                return;
                            }
                        }



                        $(".fi-pink1 [name='titleSource[]']").prop('disabled', false);
                        $(".fi-pink1 [name='contentSource[]']").prop('disabled', false);
                        $(".fi-pink1 [name='titleSource[]']").val(strTitleTrans);
                        $(".fi-pink1 [name='contentSource[]']").val(strContetTrans);

                        var formElement = document.querySelector(".formEdit");
                        var formData = new FormData(formElement);
                        $(".fi-pink1 [name='titleSource[]']").prop('disabled', true);
                        $(".fi-pink1 [name='contentSource[]']").prop('disabled', true);
                        var url = $('.formEdit').attr('action');
                        var el = document.getElementById("file-upload");
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
                            $(".fi-pink1 [name='titleSource[]']").prop('disabled', false);
                            $(".fi-pink1 [name='contentSource[]']").prop('disabled', false);
                            formData = $('.formEdit').serializeArray();
                            $(".fi-pink1 [name='titleSource[]']").prop('disabled', true);
                            $(".fi-pink1 [name='contentSource[]']").prop('disabled', true);
                            formData.push({name: 'errFile', value: '1'});
                            $.ajax({
                                url: url,
                                type: 'post',
                                data: formData,
                                success: function(result){
                                    try{
                                    	if(result == 900){
                    						alert('<?php echo $arrTextTranslate['PUBLIC_MSG_009'] ?>');
                    						window.location.href="login.php";
                    	                    return;
                    					}
                                        if(intAutoTran == 1){
                                            $(".fi-pink1 [name='titleSource[]']").val('');
                                            $(".fi-pink1 [name='contentSource[]']").val('');
										}
                                        if(intTranCheck == 1){
                                        	$(".fi-pink1 [name='titleSource[]']").prop('disabled', false);
                                            $(".fi-pink1 [name='contentSource[]']").prop('disabled', false);
                                        }

                                        eval(result);
                                    }catch(err){

                                    }

                                    $('.edit-error').append($('.edit-error2').html());
                                    $('#myModal').animate({ scrollTop: 0 }, '10');
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
                                    console.log(result);
                                    if(result == 900){
                						alert('<?php echo $arrTextTranslate['PUBLIC_MSG_009'] ?>');
                						window.location.href="login.php";
                	                    return;
                					}
                                    if(result!=0 && result != 1){
                                    	if(intAutoTran == 1){
                                            $(".fi-pink1 [name='titleSource[]']").val('');
                                            $(".fi-pink1 [name='contentSource[]']").val('');
										}
                                        if(intTranCheck == 1){
                                        	$(".fi-pink1 [name='titleSource[]']").prop('disabled', false);
                                            $(".fi-pink1 [name='contentSource[]']").prop('disabled', false);
                                        }
                                        eval(result);
                                    }else{
                                        if(result == 1){
                                        	loadPortalClose();
                                            $('#myModal').modal('hide');
                                        }else{
                                            <?php if(isset($arrOneInfo['INFORMATION_NO'])){ ?>
                                            loadView("information_view.php", <?php echo $arrOneInfo['INFORMATION_NO']; ?>);
                                            <?php } ?>
                                        }
                                    }

                                }
                            });
                        }
                    }

                }
            });

    });

    $('.chkDelete').on('click', function(e){
        var index = $('.chkDelete').index(this);
        var t = $(".pf").eq(index).val();
        console.log(t);
        if($(this).prop('checked') == true){
            $('.file-upload').eq(index).attr('disabled', false);
            $('.chkPublic').eq(index).closest('div').css('display', 'none');
        }else{
            $('.file-upload').eq(index).attr('disabled', true);
            $('.file-upload').eq(index).val('');
            $('.chkPublic').eq(index).closest('div').css('display', 'inline-block');
            if(t==1 || t=='1'){
                $('.chkPublic').eq(index).prop('checked', true);
            }else{
                $('.chkPublic').eq(index).prop('checked', false);
            }
        }

    });
    $('.file-upload').change(function(e){
        var index = $('.file-upload').index(this);
        if($(this).val() != ''){
            $('.chkPublic').eq(index).closest('div').css('display', 'inline-block');
            $('.chkPublic').eq(index).prop('checked', false);
        }else{
            $('.chkPublic').eq(index).closest('div').css('display', 'none');
            $('.chkPublic').eq(index).prop('checked', false);
        }
    });

    function checkFileExist(name, file, listMsg) {
        var arrMsg = {
            'file1': '<?php
                        echo $arrTextTranslate['PUBLIC_MSG_011']
                        .$arrTextTranslate['PUBLIC_MSG_016']; ?>',
            'file2': '<?php
                        echo $arrTextTranslate['PUBLIC_MSG_012']
                        .$arrTextTranslate['PUBLIC_MSG_016']; ?>',
            'file3': '<?php
                        echo $arrTextTranslate['PUBLIC_MSG_013']
                        .$arrTextTranslate['PUBLIC_MSG_016']; ?>',
            'file4': '<?php
                        echo $arrTextTranslate['PUBLIC_MSG_014']
                        .$arrTextTranslate['PUBLIC_MSG_016']; ?>',
            'file5': '<?php
                        echo $arrTextTranslate['PUBLIC_MSG_015']
                        .$arrTextTranslate['PUBLIC_MSG_016']; ?>'
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
            url: 'information_edit_proc.php',
            type: 'POST',
            async: false,
            data: arr,
            success: function(result) {
                if($.trim(result) == 'window.location.href="login.php";') {
                    window.location.href="login.php";
                    return;
                }
                if(result == 900){
					alert('<?php echo $arrTextTranslate['PUBLIC_MSG_009'] ?>');
					window.location.href="login.php";
                    return;
				}
                if(result == 0) {
                    $('.edit-error').html('<?php
                        echo $arrTextTranslate['PUBLIC_MSG_016'];
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
    <?php
        if(isset($arrOneInfo['CORRECTION_FLAG'])
        && $arrOneInfo['CORRECTION_FLAG']==1){
    ?>

        $('[name="tranChkBox"]').click();
        $('.tran-btn').prop('disabled', true);

    <?php
        }
    ?>
    var clickTranBtn = 0;
    var confirmTranMsg = '<?php echo $arrTextTranslate['PUBLIC_MSG_020'] ?>';
    $('[name="tranChkBox"]').on('click', function(e){
        if(clickTranBtn == 0 && $('[name="tranChkBox"]').prop('checked') == true){
            var titleTran = $(".fi-pink1 [name='titleSource[]']").val();
            var contentTran = $(".fi-pink1 [name='contentSource[]']").val();
            if(titleTran != '' || contentTran != ''){
                alert(confirmTranMsg);
                clickTranBtn++;
            }
        }
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