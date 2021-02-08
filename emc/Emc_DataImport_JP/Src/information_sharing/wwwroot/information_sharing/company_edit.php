<?php
    /*
	* @company_edit.php
	*
	* @create 2020/03/13 AKB Thang
	*/

    require_once('common/common.php');
    require_once('common/session_check.php');
    header('Content-type: text/html; charset=utf-8');
    header('X-FRAME-OPTIONS: DENY');
    //DB接続
    if(fncConnectDB() == false) {
        $_SESSION['LOGIN_ERROR'] = 'DB接続に失敗しました。';
        header('Location: login.php');
        exit;
    }
    //not login
    if(!isset($_SESSION['LOGINUSER_INFO'])) {
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
    //get user object
    $objUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);

    // get list text translate
    $arrTitle =  array(
        'COMPANY_EDIT_TEXT_001',
        'COMPANY_EDIT_TEXT_002',
        'COMPANY_EDIT_TEXT_003',
        'COMPANY_EDIT_TEXT_004',
        'COMPANY_EDIT_TEXT_005',
        'COMPANY_EDIT_TEXT_006',
        'COMPANY_EDIT_TEXT_007',
        'PUBLIC_BUTTON_003',
        'PUBLIC_BUTTON_013',
        'PUBLIC_MSG_009',
        'PUBLIC_MSG_049',
        'COMPANY_EDIT_MSG_014',
        'COMPANY_EDIT_MSG_001',
    );
    //defince constant
    define('SCREEN_NAME', '会社情報新規登録・編集画面');
    // get list text(header, title, msg) with languague_type of user logged
    $arrTextTranslate = getListTextTranslate(
        $arrTitle,
        $objUserInfo->intLanguageType
    );
    //redirect if dont have permission
    if($objUserInfo->intMenuPerm != 1) {
        echo "
        <script>
            alert('".$arrTextTranslate['PUBLIC_MSG_009']."');
            window.location.href = 'login.php';
        </script>
        ";
    }

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

    //write view log
    fncWriteLog(
        LogLevel['Info'],
        LogPattern['View'],
        SCREEN_NAME . ' 表示 (ユーザID = '.$objUserInfo->strUserID.')'
    );

    if(isset($_POST['id'])) {
        if($_POST['id'] != 0) {
            //process edit company
            $arrCompanyEdit = fncSelectOne(
                "SELECT * FROM m_company WHERE COMPANY_NO=? AND DEL_FLAG=?",
                [$_POST['id'], 0],
                SCREEN_NAME
            );
            if(!is_array($arrCompanyEdit) || count($arrCompanyEdit) == 0) {
                //company not exist, show error msg
?>
<script>
    alert('<?php echo $arrTextTranslate['COMPANY_EDIT_MSG_014']; ?>');
    $('#myModal').modal('hide');
</script>
<?php
            exit();
        }
    }
    //continue to edit company

    //get inst cats
    $strSQL = "SELECT INST_CATEGORY_NO,";
    $strSQL .= iff(
        $objUserInfo->intLanguageType,
        " INST_CATEGORY_NAME_ENG as INST_CATEGORY_NAME,",
        " INST_CATEGORY_NAME_JPN as INST_CATEGORY_NAME,"
    );
    $strSQL .= " INST_CATEGORY_NAME_ENG,";
    $strSQL .= " INST_CATEGORY_NAME_JPN,";
    $strSQL .= " SORT_NO";
    $strSQL .= " FROM m_inst_category ORDER BY SORT_NO ASC";

    $arrInstCategoryList = fncSelectData(
        $strSQL, [], 1, false, SCREEN_NAME
    );

    //get m_groups
    $strSQL = "SELECT GROUP_NO,";
    $strSQL .= iff(
        $objUserInfo->intLanguageType,
        " GROUP_NAME_ENG as GROUP_NAME,",
        " GROUP_NAME_JPN as GROUP_NAME,"
    );
    $strSQL .= " GROUP_NAME_ENG,";
    $strSQL .= " GROUP_NAME_JPN,";

    $strSQL .= " SORT_NO";
    $strSQL .= " FROM m_group ORDER BY SORT_NO ASC";

    $arrGroupList = fncSelectData($strSQL, [], 1, false, SCREEN_NAME);
?>
<div class="main-content">
    <div class="main-form">
        <div class="form-title">
            <?php echo $arrTextTranslate['COMPANY_EDIT_TEXT_001']; ?>
        </div>
        <div class="edit-error" style="color:red;"></div>
        <div class="form-body-90">
            <form class="formEdit" action="company_edit_proc.php" autocomplete="off">
            <input type="hidden" name="company_no" value="<?php
                if(isset($arrCompanyEdit['COMPANY_NO']))
                    echo $arrCompanyEdit['COMPANY_NO'];
            ?>">
            <input type="hidden" name="X-CSRF-TOKEN" value="<?php
                if(isset($_SESSION['csrf'])) echo $_SESSION['csrf'];
            ?>">
            <div class="label-short">
                <?php echo $arrTextTranslate['COMPANY_EDIT_TEXT_002']; ?>
            </div>
            <input type="text" name="company_name_jpn"
            class="t-input t-input-350" value="<?php
                if(isset($arrCompanyEdit['COMPANY_NAME_JPN']))
                    echo fncHtmlSpecialChars($arrCompanyEdit['COMPANY_NAME_JPN']);
            ?>"><br/>

            <div class="label-short">
                <?php echo $arrTextTranslate['COMPANY_EDIT_TEXT_003']; ?>
            </div>
            <input type="text" name="company_name_eng"
            class="t-input t-input-500" value="<?php
                if(isset($arrCompanyEdit['COMPANY_NAME_ENG']))
                    echo fncHtmlSpecialChars($arrCompanyEdit['COMPANY_NAME_ENG']);
            ?>"><br/>

            <div class="label-short">
                <?php echo $arrTextTranslate['COMPANY_EDIT_TEXT_004']; ?>
            </div>
            <input type="text" name="abbreviations_jpn" class="t-input t-input-350" value="<?php
                if(isset($arrCompanyEdit['ABBREVIATIONS_JPN']))
                    echo fncHtmlSpecialChars($arrCompanyEdit['ABBREVIATIONS_JPN']);
            ?>"><br/>
            <div class="label-short">
                <?php echo $arrTextTranslate['COMPANY_EDIT_TEXT_005']; ?>
            </div>
            <input type="text" name="abbreviations_eng" class="t-input t-input-500" value="<?php
                if(isset($arrCompanyEdit['ABBREVIATIONS_ENG']))
                    echo fncHtmlSpecialChars($arrCompanyEdit['ABBREVIATIONS_ENG']);
            ?>"><br/>

            <div class="label-short">
                <?php echo $arrTextTranslate['COMPANY_EDIT_TEXT_006']; ?>
            </div>
            <div class="select-container">
                <select name="inst_category_no">
                <?php echo (!isset($arrCompanyEdit['COMPANY_NO'])
                ? '<option value=""></option>' : ''); ?>">
                <?php
                    //show Inst Category List
                    if(isset($arrInstCategoryList))
                    foreach($arrInstCategoryList as $instCat){
                        echo '<option value="'.$instCat['INST_CATEGORY_NO'].'" '
                        .(isset($arrCompanyEdit['INST_CATEGORY_NO'])
                        && $arrCompanyEdit['INST_CATEGORY_NO'] == $instCat['INST_CATEGORY_NO']
                        ? 'selected' : '').'>'
                        .fncHtmlSpecialChars($instCat['INST_CATEGORY_NAME'])
                        .'</option>';
                    }
                ?>
                </select>
            </div><br/>

            <div class="label-short">
                <?php echo $arrTextTranslate['COMPANY_EDIT_TEXT_007']; ?>
            </div>
            <div class="select-container">
                <select name="group_no">
                <?php
                    echo (!isset($arrCompanyEdit['COMPANY_NO'])
                    ? '<option value=""></option>' : '');
                ?>
                <?php if(isset($arrGroupList)) foreach($arrGroupList as $arrGroup) {
                    echo '<option value="'.$arrGroup['GROUP_NO'].'" '
                    .(isset($arrCompanyEdit['GROUP_NO'])
                    && $arrCompanyEdit['GROUP_NO'] == $arrGroup['GROUP_NO']
                    ? 'selected' : '').'>'
                    .fncHtmlSpecialChars($arrGroup['GROUP_NAME'])
                    .'</option>';
                }?>
                </select>
            </div>

            <div class="form-footer top-20 text-right">
                <button type="submit" class="tbtn tbtn-defaut btn-save-edit" >
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
<style>
    .label-short{width:180px;}
</style>
<script>
    $('#close-setting').on('click', function(e) {
        setTimeout(function() {
            window.location.reload();
        }, 300);
    });
    var confirmMsg = '<?php echo $arrTextTranslate['COMPANY_EDIT_MSG_001']; ?>'
</script>
<?php
    } else {
        //id not exist
        echo "
        <script>
            alert('".PUBLIC_MSG_008_JPN."/".PUBLIC_MSG_008_ENG."');
            window.location.href = 'login.php';
        </script>
        ";
        exit();
    }
?>