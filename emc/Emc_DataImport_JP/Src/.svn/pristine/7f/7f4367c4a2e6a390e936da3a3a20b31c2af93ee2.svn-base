<?php
    /*
     * @user_edit.php
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

	// if login info does not existed, redirect to login page
    if(!isset($_SESSION['LOGINUSER_INFO'])){
        echo "
        <script>
            alert('".PUBLIC_MSG_008_JPN . "/" . PUBLIC_MSG_008_ENG."');
            window.location.href = 'login.php';
        </script>
        ";

        exit();
    }
    //logout if session time out
    fncSessionTimeOutCheck();

    //constant
    define('DISPLAY_NAME', 'ユーザ登録編集画面');
    define('DISPLAY_NAME_VIEW', 'ユーザ登録編集画面 表示');


    $_SESSION['EDIT_USER_MSG_ERROR_INPUT'] = '';

    $objUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);

    fncWriteLog(
        LogLevel['Info'] ,
        LogPattern['View'] ,
        DISPLAY_NAME_VIEW . ' (ユーザID = '.$objUserInfo->strUserID.')'
    );

    // get list text translate
    $arrTitle =  array(
        'USER_EDIT_TEXT_001',
        'USER_EDIT_TEXT_002',
        'USER_EDIT_TEXT_003',
        'USER_EDIT_TEXT_004',
        'USER_EDIT_TEXT_005',
        'PUBLIC_TEXT_006',
        'PUBLIC_TEXT_007',
        'USER_EDIT_TEXT_006',
        'USER_EDIT_TEXT_007',
        'USER_EDIT_TEXT_008',
        'USER_EDIT_TEXT_009',
        'USER_EDIT_TEXT_010',
        'USER_EDIT_TEXT_011',
        'USER_EDIT_TEXT_012',
        'USER_EDIT_TEXT_013',
        'USER_EDIT_TEXT_014',
        'USER_EDIT_TEXT_015',
        'USER_EDIT_TEXT_016',
        'USER_EDIT_TEXT_017',
        'PUBLIC_BUTTON_003',
        'PUBLIC_BUTTON_013',
        'USER_EDIT_MSG_001',
        'PUBLIC_MSG_009',
        'PUBLIC_MSG_001',
        'PUBLIC_MSG_049'
    );

    // get list text(header, title, msg) with languague_type of user logged
    $arrTextTranslate = getListTextTranslate($arrTitle, $objUserInfo->intLanguageType);
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
        if($_POST['id'] != 0){
            //get edit user
            $arrOneUser = fncSelectOne(
                "SELECT * FROM m_user WHERE USER_NO=?",
                [$_POST['id']],
                DISPLAY_NAME
            );
            if(!$arrOneUser){
                //user not exist, return error msg

?>
    <script>
        alert('<?php echo $arrTextTranslate['PUBLIC_MSG_001']; ?>');
        $('#myModal').modal('hide');
    </script>


<?php
            exit();
        }
    }

    //continue to edit user

    //get company list
    $strSQL = "SELECT COMPANY_NO,";
    $strSQL .= iff($objUserInfo->intLanguageType,
        " COMPANY_NAME_ENG as COMPANY_NAME,",
        " COMPANY_NAME_JPN as COMPANY_NAME,");
    $strSQL .= " COMPANY_NAME_ENG,";
    $strSQL .= " COMPANY_NAME_JPN,";
    $strSQL .= " SORT_NO";
    $strSQL .= " FROM m_company WHERE DEL_FLAG=? ORDER BY SORT_NO ASC";

    $arrCompanyList = fncSelectData($strSQL, [0], 1, false, DISPLAY_NAME);
?>
<style>
    <?php if($objUserInfo->intLanguageType == 1) {
        //user is english, add style
        ?>
        .width-120 { width: 120px !important; }
    <?php } ?>
    
    .t-input-250 { width: 300px !important; }
    .t-input-60 { width: 100px !important; }
    .width-265 { width: 330px !important; }
    .modal-content { min-width: 1000px; !important; }
</style>
<div class="main-content" style="min-width: 1000px;">
    <div class="main-form">
        <div class="form-title">
            <?php echo $arrTextTranslate['USER_EDIT_TEXT_001']; ?>
        </div>
        <div class="edit-error" style="color:red;"></div>
        <div class="form-body">
            <form class="formEdit" action="user_edit_proc.php" autocomplete="off">
                <div class="clearfix">

                    <div class="in-line tcol-md-6 " style="float: left">

                        <div class="label-short">
                            <?php echo $arrTextTranslate['USER_EDIT_TEXT_002']; ?>
                            <span class="txt-red">※</span>
                        </div>
                        <input type="hidden" name="mode" class="t-input t-input-250" value="1">
                        <input type="hidden" name="user_no" class="t-input t-input-250" value="<?php
                            //user_no
                            if(isset($arrOneUser['USER_NO'])) echo $arrOneUser['USER_NO'];
                        ?>">
                        <input type="hidden" name="X-CSRF-TOKEN" value="<?php
                            //csrf token
                            if(isset($_SESSION['csrf'])) echo $_SESSION['csrf'];
                        ?>">
                        <input type="text"  name="user_id" class="t-input t-input-250" value="<?php
                            //user_id
                            if(isset($arrOneUser['USER_ID']))
                                echo fncHtmlSpecialChars($arrOneUser['USER_ID']); ?>"
                            <?php if(isset($arrOneUser['USER_NO']))
                                echo 'disabled'; ?>
                        ><br/>

                        <div class="label-short">
                            <?php echo $arrTextTranslate['USER_EDIT_TEXT_004']; ?>
                            <span class="txt-red">※</span>
                        </div>
                        <input type="text" name="expiration_date_s" class="t-input t-input-115"
                            id="date_start" value="<?php
                                if(isset($arrOneUser['EXPIRATION_DATE_S']))
                                    echo date_format(date_create($arrOneUser['EXPIRATION_DATE_S']), 'Y/m/d'); ?>"
                        > ~
                        <input type="text" name="expiration_date_e" class="t-input t-input-115"
                            id="date_end" value="<?php
                                if(isset($arrOneUser['EXPIRATION_DATE_E']))
                                    echo date_format(date_create($arrOneUser['EXPIRATION_DATE_E']), 'Y/m/d');
                            ?>"><br/>

                        <div class="label-short">
                            <?php echo $arrTextTranslate['USER_EDIT_TEXT_006']; ?>
                            <span class="txt-red">※</span>
                        </div>
                        <div class="select-container">
                            <select name="company_no">
                                <?php
                                    echo (!isset($arrOneUser['USER_NO']) ? '<option value=""></option>' : '');
                                ?>
                                <?php if(isset($arrCompanyList)) foreach($arrCompanyList as $arrCompany){?>
                                    <option value="<?php echo $arrCompany['COMPANY_NO']; ?>"
                                        <?php echo (isset($arrOneUser['COMPANY_NO'])
                                            && $arrOneUser['COMPANY_NO']==$arrCompany['COMPANY_NO'] ? 'selected' : ''); ?>>
                                        <?php echo fncHtmlSpecialChars($arrCompany['COMPANY_NAME']); ?>
                                    </option>
                                <?php } ?>
                            </select>
                        </div><br/>

                        <div class="label-short">
                            <?php echo $arrTextTranslate['USER_EDIT_TEXT_009']; ?>
                        </div>
                        <input type="text"  name="organization" class="t-input t-input-250" value="<?php
                            if(isset($arrOneUser['ORGANIZATION']))
                                echo fncHtmlSpecialChars($arrOneUser['ORGANIZATION']);
                            ?>"><br/>

                        <div class="label-short">
                            <?php echo $arrTextTranslate['USER_EDIT_TEXT_011']; ?>
                        </div>
                        <input type="text" name="user_name" class="t-input t-input-250" value="<?php
                            if(isset($arrOneUser['USER_NAME']))
                                echo fncHtmlSpecialChars($arrOneUser['USER_NAME']);
                        ?>"><br/>

                        <div class="label-short">
                            <?php echo $arrTextTranslate['USER_EDIT_TEXT_013']; ?>
                        </div>
                        <input type="text" name="tel" class="t-input t-input-250" value="<?php
                            if(isset($arrOneUser['TEL']))
                                echo fncHtmlSpecialChars($arrOneUser['TEL']);
                        ?>"><br/>
                    </div>

                    <div class="in-line tcol-md-6">
                        <div class="label-short">
                            <?php echo $arrTextTranslate['USER_EDIT_TEXT_003']; ?>
                            <span class="txt-red">※</span>
                        </div>
                        <input  name="password" type="password" class="t-input t-input-250 width-265" autocomplete="new-password" value="<?php
                            if(isset($arrOneUser['PASSWORD']))
                                echo fncHtmlSpecialChars($arrOneUser['PASSWORD']);
                        ?>"><br/>

                        <div class="cont-radio" style="margin: 0px !important;">
                            <div class="label-short">
                                <?php echo $arrTextTranslate['USER_EDIT_TEXT_005']; ?>
                                <span class="txt-red">※</span>
                            </div>
                            <label class="container-radio">
                                <?php echo $arrTextTranslate['PUBLIC_TEXT_006']; ?>
                                <input type="radio" name="language_type" value="0" <?php
                                    echo (isset($arrOneUser['LANGUAGE_TYPE'])
                                        && $arrOneUser['LANGUAGE_TYPE']==0 ? 'checked': '');
                                ?>>
                                <span class="checkmark checkmark-radio"></span>
                            </label>
                            <label class="container-radio">
                                <?php echo $arrTextTranslate['PUBLIC_TEXT_007']; ?>
                                <input type="radio" name="language_type" value="1" <?php
                                    echo (isset($arrOneUser['LANGUAGE_TYPE'])
                                    && $arrOneUser['LANGUAGE_TYPE']==1 ? 'checked' : '');
                                ?>>
                                <span class="checkmark checkmark-radio"></span>
                            </label><br/>
                        </div>
                        <?php
                            if(isset($arrOneUser['COMPANY_NO']))
                            $arrOneTmp = fncSelectOne("SELECT
                            ".iff($objUserInfo->intLanguageType,
                            ' m_company.ABBREVIATIONS_ENG as abbreviations,',
                            ' m_company.ABBREVIATIONS_JPN as abbreviations,')."
                            ".iff($objUserInfo->intLanguageType,
                            ' m_inst_category.INST_CATEGORY_NAME_ENG as INST_CATEGORY_NAME,',
                            ' m_inst_category.INST_CATEGORY_NAME_JPN as INST_CATEGORY_NAME,')."
                            ".iff($objUserInfo->intLanguageType,
                            ' m_group.GROUP_NAME_ENG as GROUP_NAME',
                            ' m_group.GROUP_NAME_JPN as GROUP_NAME')."
                            FROM m_company
                            INNER JOIN m_inst_category
                            ON m_inst_category.INST_CATEGORY_NO = m_company.INST_CATEGORY_NO
                            INNER JOIN m_group
                            ON m_group.GROUP_NO = m_company.GROUP_NO
                            WHERE COMPANY_NO=? AND DEL_FLAG=?"
                            , [$arrOneUser['COMPANY_NO'], 0],
                            DISPLAY_NAME
                        );

                        ?>



                        <div class="in-line">
                            <div class="label-short">
                                <?php echo $arrTextTranslate['USER_EDIT_TEXT_007']; ?>
                            </div>
                            <input type="text" class="t-input t-input-60" id="aj1" value="<?php
                                if(isset($arrOneTmp['abbreviations']))
                                    echo fncHtmlSpecialChars($arrOneTmp['abbreviations']);
                            ?>" disabled>
                        </div>

                        <div class="in-line">
                            <div class="label-100 width-120">
                                <?php echo $arrTextTranslate['USER_EDIT_TEXT_008']; ?>
                            </div>
                            <input type="text" class="t-input" id="aj2" value="<?php
                                if(isset($arrOneTmp['INST_CATEGORY_NAME']))
                                    echo fncHtmlSpecialChars($arrOneTmp['INST_CATEGORY_NAME']);
                            ?>" style="width:100px;" disabled><br/>
                        </div>


                        <div class="label-short">
                            <?php echo $arrTextTranslate['USER_EDIT_TEXT_010']; ?>
                            <span class="txt-red">※</span>
                        </div>


                        <input type="text" class="t-input t-input-250 width-265" id="aj3" value="<?php
                            if(isset($arrOneTmp['GROUP_NAME']))
                                echo fncHtmlSpecialChars($arrOneTmp['GROUP_NAME']);
                        ?>" disabled><br/>

                        <div class="label-short">
                            <?php echo $arrTextTranslate['USER_EDIT_TEXT_012']; ?>
                            <span class="txt-red">※</span>
                        </div>
                        <input type="text" name="mail_address" class="t-input t-input-250 width-265" value="<?php
                            if(isset($arrOneUser['MAIL_ADDRESS']))
                                echo fncHtmlSpecialChars($arrOneUser['MAIL_ADDRESS']);
                        ?>"><br/>

                        <div class="label-short">
                            <?php echo $arrTextTranslate['USER_EDIT_TEXT_014']; ?>
                        </div>
                        <input type="text" name="fax" class="t-input t-input-250 width-265" value="<?php
                            if(isset($arrOneUser['FAX']))
                                echo fncHtmlSpecialChars($arrOneUser['FAX']);
                        ?>"><br/>
                    </div>

                </div>


                <div class="label-short">
                    <?php echo $arrTextTranslate['USER_EDIT_TEXT_015']; ?>
                </div>
                <input type="text" name="zip_code" class="t-input" value="<?php
                    if(isset($arrOneUser['ZIP_CODE']))
                        echo fncHtmlSpecialChars($arrOneUser['ZIP_CODE']);
                ?>"><br/>

                <div>
                    <div class="label-short" style="float:left;width:124px;">
                        <?php echo $arrTextTranslate['USER_EDIT_TEXT_016']; ?>
                    </div>
                    <textarea class="t-input"  name="address" rows="2" style="width:820px"><?php
                        if(isset($arrOneUser['ADDRESS']))
                            echo fncHtmlSpecialChars($arrOneUser['ADDRESS']);
                    ?></textarea><br/>
                    <div class="label-short" style="float:left;width:124px;">
                        <?php echo $arrTextTranslate['USER_EDIT_TEXT_017']; ?>
                    </div>
                    <textarea class="t-input"  name="remarks" rows="2" style="width:820px"><?php
                        if(isset($arrOneUser['REMARKS']))
                            echo fncHtmlSpecialChars($arrOneUser['REMARKS']);
                    ?></textarea>
                </div>

                <div class="form-footer text-right right-20">
                    <button type="submit" class="tbtn-cancel tbtn-defaut btn-save-edit">
                        <?php echo $arrTextTranslate['PUBLIC_BUTTON_013']; ?>
                    </button>
                    <button type="submit" class="tbtn-cancel tbtn-defaut"
                        id="close-setting" data-dismiss="modal">
                        <?php echo $arrTextTranslate['PUBLIC_BUTTON_003']; ?>
                    </button>
                </div>
            </form>
        </div>
    </div>
</div>

<script>
    $('#date_start').datepicker({
        changeMonth: true,
        changeYear: true,
        showButtonPanel: true,
        dateFormat: 'yy/mm/dd'
    });

    $('#date_end').datepicker({
        changeMonth: true,
        changeYear: true,
        showButtonPanel: true,
        dateFormat: 'yy/mm/dd'
    });

    $('#close-setting').on('click', function(e) {
        setTimeout(function() {
            window.location.reload();
        }, 300);
    });

    $('[name=company_no]').change(function(){
        var companyNo = $(this).val();
        $.ajax({
			url: 'user_edit_proc.php',
			type: 'post',
			data: [
                {
                    name: 'X-CSRF-TOKEN',
                    value: '<?php if(isset($_SESSION['csrf'])) echo $_SESSION['csrf']; ?>'
                },
				{name: 'mode', value: 2},
				{name: 'company_no', value: companyNo}
            ],
            dataType: 'json',
			success: function(result){
                var arr = result;
                $('#aj1').val(arr[0]);
                $('#aj2').val(arr[1]);
                $('#aj3').val(arr[2]);


			}
		});
    });
</script>
<script>
    var confirmMsg = '<?php echo $arrTextTranslate['USER_EDIT_MSG_001']; ?>'
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