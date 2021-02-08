<?php
    /*
     * @user_perm.php
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
    //If have not login yet, redirect to login page
    if(!isset($_SESSION['LOGINUSER_INFO'])){
        echo "
        <script>
            alert('".PUBLIC_MSG_008_JPN . "/" . PUBLIC_MSG_008_ENG."');
            window.location.href = 'login.php';
        </script>
        ";

        exit();
    }
    //session timeout check
    fncSessionTimeOutCheck();
    //define constant
    define('SCREEN_NAME', 'ユーザ別権限設定画面');
    define('SCREEN_NAME_VIEW', 'ユーザ別権限設定画面　画面表示');
    //create sesstion to store error message
    $_SESSION['USER_PERM_MSG_ERROR_INPUT'] = '';
    //get object user login
    $objUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);

    // get list text translate
    $arrTitle =  array(
        'USER_PERM_TEXT_001',
        'USER_PERM_TEXT_002',
        'USER_PERM_TEXT_003',
        'USER_PERM_TEXT_004',
        'USER_PERM_TEXT_005',
        'USER_PERM_TEXT_006',
        'USER_PERM_TEXT_007',
        'USER_PERM_TEXT_008',
        'USER_PERM_TEXT_009',
        'USER_PERM_TEXT_010',
        'USER_PERM_TEXT_011',
        'USER_PERM_TEXT_012',
        'USER_PERM_TEXT_013',
        'USER_PERM_TEXT_014',
        'USER_PERM_TEXT_015',
        'USER_PERM_TEXT_016',
        'USER_PERM_TEXT_017',
        'USER_PERM_TEXT_018',
        'USER_PERM_TEXT_019',
        'USER_PERM_TEXT_020',
        'USER_PERM_TEXT_021',
        'USER_PERM_MSG_001',
        'PUBLIC_BUTTON_003',
        'PUBLIC_BUTTON_013',
        'PUBLIC_MSG_009',
        'PUBLIC_MSG_049',
    );
    // get list text(header, title, msg) with languague_type of user logged
    $arrTextTranslate = getListTextTranslate($arrTitle,
        $objUserInfo->intLanguageType);

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
    //view log
    $strLog = SCREEN_NAME_VIEW . ' (ユーザID = '.$objUserInfo->strUserID.') ';
    $strLog .= isset($_SERVER['HTTP_REFERER']) ? $_SERVER['HTTP_REFERER'] : null;
    fncWriteLog(
        LogLevel['Info'],
        LogPattern['View'],
        $strLog
    );
    //redirect if dont have permission
    if($objUserInfo->intMenuPerm == 0) {
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
            $arrOneUserPerm = fncSelectOne("SELECT
            m_user.USER_NO,
            m_user.USER_ID,
            m_user.PERM_FLAG,
            m_user.ANNOUNCE_REG_PERM,
            m_user.JCMG_TAB_PERM,
            m_user.QUERY_REG_PERM,
            m_user.INCIDENT_CASE_REG_PERM,
            m_user.REQUEST_REG_PERM,
            m_user.INFORMATION_REG_PERM,
            m_user.MENU_PERM,
            m_user.ANNOUNCE_MAIL,
            m_user.BULLETIN_BOARD_MAIL,
            m_user.INCIDENT_CASE_MAIL,
            m_user.REQUEST_CONTENTS_MAIL,
            m_group.ADMIN_FLAG,
            ".iff(
                $objUserInfo->intLanguageType,
                " COMPANY_NAME_ENG as COMPANY_NAME",
                " COMPANY_NAME_JPN as COMPANY_NAME"
            )."
            FROM m_user
            INNER JOIN m_company ON m_user.COMPANY_NO = m_company.COMPANY_NO
            INNER JOIN m_group ON m_company.GROUP_NO = m_group.GROUP_NO
            WHERE USER_NO=?", [$_POST['id']], SCREEN_NAME
        );
    }
    //continue to edit user
?>
<div class="main-content">
    <div class="main-form">
        <div class="form-title"><?php echo $arrTextTranslate['USER_PERM_TEXT_001']; ?></div>
        <div class="edit-error" style="color:red;"></div>
        <div class="form-body">
            <form class="formEdit" action="user_perm_proc.php" name = "userPerm">
            <input type="hidden" name="mode" class="t-input t-input-250" value="1">
            <input type="hidden" name="user_no" class="t-input t-input-250" value="<?php
                if(isset($arrOneUserPerm['USER_NO'])) echo $arrOneUserPerm['USER_NO'];
            ?>">
            <input type="hidden" name="X-CSRF-TOKEN" value="<?php
                if(isset($_SESSION['csrf'])) echo $_SESSION['csrf'];
            ?>">
            <table class="blueTable">
                <thead>
                    <tr>
                        <th rowspan="3" align="center" class="text-center"><?php
                            echo html_entity_decode($arrTextTranslate['USER_PERM_TEXT_002']);
                        ?></th>
                        <th rowspan="3" align="center" class="text-center"><?php
                            echo html_entity_decode($arrTextTranslate['USER_PERM_TEXT_003']);
                        ?></th>
                        <th rowspan="1" align="center" class="text-center"><?php
                            echo html_entity_decode($arrTextTranslate['USER_PERM_TEXT_021']);
                        ?></th>
                        <th colspan="6" align="center" class="text-center"><?php
                            echo html_entity_decode($arrTextTranslate['USER_PERM_TEXT_004']);
                        ?></th>
                        <th colspan="5" align="center" class="text-center"><?php
                            echo html_entity_decode($arrTextTranslate['USER_PERM_TEXT_014']);
                        ?></th>
                    </tr>
                    <tr>
                        <th rowspan="2" align="center" class="text-center"><?php
                            echo html_entity_decode($arrTextTranslate['USER_PERM_TEXT_008']);
                        ?></th>
                        <th colspan="2" align="center" class="text-center"><?php
                            echo html_entity_decode($arrTextTranslate['USER_PERM_TEXT_005']);
                        ?></th>
                        <th colspan="3" align="center" class="text-center"><?php
                            echo html_entity_decode($arrTextTranslate['USER_PERM_TEXT_006']);
                        ?></th>
                        <th rowspan="2" align="center" class="text-center"><?php
                            echo html_entity_decode($arrTextTranslate['USER_PERM_TEXT_013']);
                        ?></th>
                        <th colspan="2" align="center" class="text-center"><?php
                            echo html_entity_decode($arrTextTranslate['USER_PERM_TEXT_015']);
                        ?></th>
                        <th colspan="2" align="center" class="text-center"><?php
                            echo html_entity_decode($arrTextTranslate['USER_PERM_TEXT_016']);
                        ?></th>
                    </tr>
                    <tr class="text-center">
                        <th align="center" class="text-center"><?php
                            echo html_entity_decode($arrTextTranslate['USER_PERM_TEXT_007']);
                        ?></th>
                        <th align="center" class="text-center"><?php
                            echo html_entity_decode($arrTextTranslate['USER_PERM_TEXT_009']);
                        ?></th>
                        <th align="center" class="text-center"><?php
                            echo html_entity_decode($arrTextTranslate['USER_PERM_TEXT_010']);
                        ?></th>
                        <th align="center" class="text-center"><?php
                            echo html_entity_decode($arrTextTranslate['USER_PERM_TEXT_011']);
                        ?></th>
                        <th align="center" class="text-center"><?php
                            echo html_entity_decode($arrTextTranslate['USER_PERM_TEXT_012']);
                        ?></th>
                        <th align="center" class="text-center"><?php
                            echo html_entity_decode($arrTextTranslate['USER_PERM_TEXT_017']);
                        ?></th>
                        <th align="center" class="text-center"><?php
                            echo html_entity_decode($arrTextTranslate['USER_PERM_TEXT_018']);
                        ?></th>
                        <th align="center" class="text-center"><?php
                            echo html_entity_decode($arrTextTranslate['USER_PERM_TEXT_019']);
                        ?></th>
                        <th align="center" class="text-center"><?php
                            echo html_entity_decode($arrTextTranslate['USER_PERM_TEXT_020']);
                        ?></th>

                    </tr>
                </thead>
                <tfoot>
                    <tr>
                        <td colspan="14" align="right">
                            <button type="submit" class="tbtn tbtn-defaut btn-save-edit"><?php
                                echo $arrTextTranslate['PUBLIC_BUTTON_013'];
                            ?></button>
                            <button type="submit" class="tbtn-cancel tbtn-defaut"
                            id="close-setting" data-dismiss="modal"><?php
                                echo $arrTextTranslate['PUBLIC_BUTTON_003'];
                            ?></button>
                        </td>
                    </tr>
                </tfoot>
                <tbody>
                    <tr>
                        <td class="text-center"><?php
                            echo fncHtmlSpecialChars($arrOneUserPerm['USER_ID']);
                        ?></td>
                        <td class="text-center"><?php
                            echo fncHtmlSpecialChars($arrOneUserPerm['COMPANY_NAME']);
                        ?></td>
                        <td align="center">
                            <input type="checkbox" name="JCMG_TAB_PERM"
                            <?php echo $arrOneUserPerm['JCMG_TAB_PERM'] ? 'checked': ''; ?>
                            >
                        </td>
                        <td align="center" <?php echo $arrOneUserPerm['ADMIN_FLAG']==0
                        ? 'style="background: #cecece;"' : ''; ?>>
                            <input type="checkbox" name="ANNOUNCE_REG_PERM"
                            <?php echo $arrOneUserPerm['ADMIN_FLAG']
                            ? (
                                $arrOneUserPerm['ANNOUNCE_REG_PERM'] ? 'checked': ''
                            )
                            : 'disabled style="display:none"' ; ?>
                            >
                        </td>

                        <td align="center">
                            <input type="checkbox" name="QUERY_REG_PERM"
                            <?php echo $arrOneUserPerm['QUERY_REG_PERM'] ? 'checked': ''; ?>
                            >
                        </td>
<?php
$strJcmgRegBackColor = '';
$strJcmgRegChecked = '';
$strJcmgRegDisplay = '';
if($arrOneUserPerm['JCMG_TAB_PERM'] == 0 || $arrOneUserPerm['ADMIN_FLAG']==0){
	$strJcmgRegBackColor = 'background: #cecece;';
	$strJcmgRegDisplay = 'display: none';
}else{
	if($arrOneUserPerm['INCIDENT_CASE_REG_PERM'] == 1){
		$strJcmgRegChecked = 'checked';
	}
}
?>
                        <td id="JCMG_REG_TD" align="center" style="<?php echo $strJcmgRegBackColor;?>">
                            <input type="checkbox" name="INCIDENT_CASE_REG_PERM" id="JCMG_REG" style="<?php echo $strJcmgRegDisplay;?>" <?php echo $strJcmgRegChecked;?>>
                        </td>
<?php
$strReqRegBackColor = '';
$strReqRegChecked = '';
$strReqRegDisplay = '';
if($arrOneUserPerm['JCMG_TAB_PERM'] == 0){
	$strReqRegBackColor = 'background: #cecece;';
	$strReqRegDisplay = 'display: none';
}else{
	if($arrOneUserPerm['REQUEST_REG_PERM'] == 1){
		$strReqRegChecked = 'checked';
	}
}
?>
                        <td id="REQ_REG_TD" align="center" style="<?php echo $strReqRegBackColor;?>">
                            <input type="checkbox" name="REQUEST_REG_PERM" id="REQ_REG" style="<?php echo $strReqRegDisplay;?>" <?php echo $strReqRegChecked;?>>
                        </td>
<?php
$strInfoRegBackColor = '';
$strInfoRegChecked = '';
$strInfoRegDisplay = '';
if($arrOneUserPerm['JCMG_TAB_PERM'] == 0){
	$strInfoRegBackColor = 'background: #cecece;';
	$strInfoRegDisplay = 'display: none';
}else{
	if($arrOneUserPerm['INFORMATION_REG_PERM'] == 1){
		$strInfoRegChecked = 'checked';
	}
}
?>
                        <td id="INFO_REG_TD" align="center" style="<?php echo $strInfoRegBackColor;?>">
                            <input type="checkbox" name="INFORMATION_REG_PERM" id="INFO_REG" style="<?php echo $strInfoRegDisplay;?>" <?php echo $strInfoRegChecked;?>>
                        </td>
                        <td align="center" <?php echo $arrOneUserPerm['ADMIN_FLAG']==0
                        ? 'style="background: #cecece;"' : ''; ?>>
                            <input type="checkbox" name="MENU_PERM"
                            <?php echo $arrOneUserPerm['ADMIN_FLAG']
                            ? (
                                $arrOneUserPerm['MENU_PERM'] ? 'checked': ''
                            )
                            : 'disabled style="display:none"' ; ?>
                            >
                        </td>
                        <td align="center">
                            <input type="checkbox" name="ANNOUNCE_MAIL"
                            <?php echo $arrOneUserPerm['ANNOUNCE_MAIL'] ? 'checked': ''; ?>
                            >
                        </td>
                        <td align="center">
                            <input type="checkbox" name="BULLETIN_BOARD_MAIL"
                            <?php echo $arrOneUserPerm['BULLETIN_BOARD_MAIL'] ? 'checked': ''; ?>
                            >
                        </td>
<?php
$strJcmgMailBackColor = '';
$strJcmgMailChecked = '';
$strJcmgMailDisplay = '';
if($arrOneUserPerm['JCMG_TAB_PERM'] == 0){
	$strJcmgMailBackColor = 'background: #cecece;';
	$strJcmgMailDisplay = 'display: none';
}else{
	if($arrOneUserPerm['INCIDENT_CASE_MAIL'] == 1){
		$strJcmgMailChecked = 'checked';
	}
}
?>
                        <td id="JCMG_MAIL_TD" align="center" style="<?php echo $strJcmgMailBackColor;?>">
                            <input type="checkbox" name="INCIDENT_CASE_MAIL" id="JCMG_MAIL" style="<?php echo $strJcmgMailDisplay;?>" <?php echo $strJcmgMailChecked;?>>
                        </td>
<?php
$strReqMailBackColor = '';
$strReqMailChecked = '';
$strReqMailDisplay = '';
if($arrOneUserPerm['JCMG_TAB_PERM'] == 0){
	$strReqMailBackColor = 'background: #cecece;';
	$strReqMailDisplay = 'display: none';
}else{
	if($arrOneUserPerm['REQUEST_CONTENTS_MAIL'] == 1){
		$strReqMailChecked = 'checked';
	}
}
?>
                        <td id="REQ_MAIL_TD" align="center" style="<?php echo $strReqMailBackColor;?>">
                            <input type="checkbox" name="REQUEST_CONTENTS_MAIL" id="REQ_MAIL" style="<?php echo $strReqMailDisplay;?>" <?php echo $strReqMailChecked;?>>
                        </td>
                    </tr>
                </tbody>
                </tr>
            </table>
            </form>
        </div>
    </div>
</div>
<style>
    .modal-lg {
        width: 1205px !important;
    }
</style>
<script>
    var confirmMsg = '<?php echo $arrTextTranslate['USER_PERM_MSG_001']; ?>'

    $('#close-setting').on('click', function(e) {
        setTimeout(function() {
            window.location.reload();
        }, 300);
    });

	$(function(){ $('input[name="JCMG_TAB_PERM"]').change(function() {
			if(! $(this).prop('checked')){
				$('#JCMG_REG').prop('checked', false);
				$('#REQ_REG').prop('checked', false);
				$('#INFO_REG').prop('checked', false);
				$('#JCMG_MAIL').prop('checked', false);
				$('#REQ_MAIL').prop('checked', false);
				
				$('#REQ_REG').hide();
				$('#REQ_REG_TD').css('background-color', '#cecece');
				$('#INFO_REG').hide();
				$('#INFO_REG_TD').css('background-color', '#cecece');
				$('#JCMG_MAIL').hide();
				$('#JCMG_MAIL_TD').css('background-color', '#cecece');
				$('#REQ_MAIL').hide();
				$('#REQ_MAIL_TD').css('background-color', '#cecece');
<?php
    if($arrOneUserPerm['ADMIN_FLAG']!=0){
?>
				$('#JCMG_REG').hide();
				$('#JCMG_REG_TD').css('background-color', '#cecece');
<?php
    }
?>

			}else{
				$('#REQ_REG').show();
				$('#REQ_REG_TD').css('background-color', '');
				$('#INFO_REG').show();
				$('#INFO_REG_TD').css('background-color', '');
				$('#JCMG_MAIL').show();
				$('#JCMG_MAIL_TD').css('background-color', '');
				$('#REQ_MAIL').show();
				$('#REQ_MAIL_TD').css('background-color', '');
<?php
    if($arrOneUserPerm['ADMIN_FLAG']!=0){
?>
				$('#JCMG_REG').show();
				$('#JCMG_REG_TD').css('background-color', '');
<?php
    }
?>
			}
		});
	});


</script>
<?php

    }else{
        //id not input
        echo "
        <script>
            alert('".PUBLIC_MSG_008_JPN."/".PUBLIC_MSG_008_ENG."');
            window.location.href = 'login.php';
        </script>
        ";
        exit();
    }
?>