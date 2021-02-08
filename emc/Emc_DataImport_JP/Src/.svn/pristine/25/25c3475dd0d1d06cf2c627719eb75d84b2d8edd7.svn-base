<?php
    /*
     * @user_perm_proc.php
     *
     * @create 2020/03/13 AKB Thang
     */
    require_once('common/common.php');
    require_once('common/session_check.php');
    header('Content-type: text/html; charset=utf-8');
    header('X-FRAME-OPTIONS: DENY');
    //database connect failed
    if(fncConnectDB() == false) {

        $_SESSION['LOGIN_ERROR'] = 'DB接続に失敗しました。';
        header('Location: login.php');
        exit;
    }
    //user not login
    if(!isset($_SESSION['LOGINUSER_INFO'])) {
        echo "
        <script>
        alert('".PUBLIC_MSG_008_JPN . "/" . PUBLIC_MSG_008_ENG."');
            window.location.href = 'login.php';
        </script>
        ";
        exit();
    }
    //session error message
    $_SESSION['USER_PERM_MSG_ERROR_INPUT'] = '';
    //define constant
    define('SCREEN_NAME', 'ユーザ別権限設定画面');
    define('SCREEN_NAME_EDIT', 'ユーザ別権限設定画面 更新');
    //object user login
    $objUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);

    //sesstion check
    fncSessionTimeOutCheck(1);
    // get list text translate
    $arrTitle =  array(
        'PUBLIC_MSG_009',
        'PUBLIC_MSG_049',
        'PUBLIC_MSG_003',
    );
    
    // get list text(header, title, msg) with languague_type of user logged
    $arrTextTranslate = getListTextTranslate($arrTitle,
        $objUserInfo->intLanguageType);

    //check get request
    fncGetRequestCheck($arrTextTranslate);
    // check request directly
    if(!isset($_SERVER['HTTP_REFERER'])) {
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
    //check permsission
    if($objUserInfo->intMenuPerm != 1) {
        echo "
        <script>
            alert('".$arrTextTranslate['PUBLIC_MSG_009']."');
            window.location.href = 'login.php';
        </script>
        ";
    }


    //check input
    if(isset($_POST['mode']) && $_POST['mode'] == 1) {
    if(isset($_POST['user_no'])) {
        //log button edit
        fncWriteLog(
            LogLevel['Info'],
            LogPattern['Button'],
            SCREEN_NAME_EDIT . ' (ユーザID = '.$objUserInfo->strUserID.')'
        );
        //get user admin_flag
        $arrGroup = fncSelectOne("SELECT m_group.ADMIN_FLAG
            FROM m_user
            INNER JOIN m_company ON m_user.COMPANY_NO = m_company.COMPANY_NO
            INNER JOIN m_group ON m_company.GROUP_NO = m_group.GROUP_NO
            WHERE m_user.USER_NO = ?
            ", [$_POST['user_no']],
            SCREEN_NAME
        );

        if(!$arrGroup && $_POST['user_no'] != '') {
            //sql querry error
            $_SESSION['USER_PERM_MSG_ERROR_INPUT'] .= $arrTextTranslate['PUBLIC_MSG_003'];
            $_SESSION['USER_PERM_MSG_ERROR_INPUT'] .= '<br>';
        }
    } else {
        //user_no not input
        $_SESSION['USER_PERM_MSG_ERROR_INPUT'] .= $arrTextTranslate['PUBLIC_MSG_003'];
        $_SESSION['USER_PERM_MSG_ERROR_INPUT'] .= '<br>';
    }


    if($_SESSION['USER_PERM_MSG_ERROR_INPUT'] != '') {
        //show error message
?>
$('.edit-error').html('<?php echo $_SESSION['USER_PERM_MSG_ERROR_INPUT']; ?>');
<?php
        } else {
            //process update

            if($_POST['user_no']!='') {
                //update user permission
                $objResult = fncProcessData([
                    'ANNOUNCE_REG_PERM' => iff($arrGroup['ADMIN_FLAG'],
                    isset($_POST['ANNOUNCE_REG_PERM']) ? 1 : 0, 0),
                    'JCMG_TAB_PERM' => isset($_POST['JCMG_TAB_PERM']) ? 1 : 0,
                    'QUERY_REG_PERM' => isset($_POST['QUERY_REG_PERM']) ? 1 : 0,
                    'INCIDENT_CASE_REG_PERM' => iff($arrGroup['ADMIN_FLAG'],
                    isset($_POST['INCIDENT_CASE_REG_PERM']) ? 1 : 0, 0),
                    'REQUEST_REG_PERM' => isset($_POST['REQUEST_REG_PERM']) ? 1 : 0,
                    'INFORMATION_REG_PERM' => isset($_POST['INFORMATION_REG_PERM']) ? 1 : 0,
                    'MENU_PERM' => iff($arrGroup['ADMIN_FLAG'],
                    isset($_POST['MENU_PERM']) ? 1 : 0, 0),
                    'ANNOUNCE_MAIL' => isset($_POST['ANNOUNCE_MAIL']) ? 1 : 0,
                    'BULLETIN_BOARD_MAIL' => isset($_POST['BULLETIN_BOARD_MAIL']) ? 1 : 0,
                    'INCIDENT_CASE_MAIL' => isset($_POST['INCIDENT_CASE_MAIL']) ? 1 : 0,
                    'REQUEST_CONTENTS_MAIL' => isset($_POST['REQUEST_CONTENTS_MAIL']) ? 1 : 0,
                    'PERM_FLAG' =>  1,
                    'UP_USER_NO'    =>  $objUserInfo->intUserNo,
                    'UP_DATE'   => date('Y-m-d H:i:s')
                ],
                'm_user',
                'USER_NO=?',
                array($_POST['user_no']),
                SCREEN_NAME);

                if(is_object($objResult) && $objResult->rowCount()>0) {
                    //update successfully
                    echo 0;

                } else {
                    //update failed, write error log
                    fncWriteLog(
                        LogLevel['Error'],
                        LogPattern['Error'],
                        SCREEN_NAME . ' ' . $arrTextTranslate['PUBLIC_MSG_003']
                    );
                    echo "$('.edit-error').html('".$arrTextTranslate['PUBLIC_MSG_003']."');";
                }
            }
        }
    }
?>
