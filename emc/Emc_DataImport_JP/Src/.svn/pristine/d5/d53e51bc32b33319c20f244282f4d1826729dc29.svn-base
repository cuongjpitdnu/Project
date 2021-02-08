<?php
    /*
     * @user_edit_proc.php
     *
     * @create 2020/03/13 AKB Thang
     */

    require_once('common/common.php');
    require_once('common/session_check.php');

    header('Content-type: text/html; charset=utf-8');
    header('X-FRAME-OPTIONS: DENY');

    //constant
    define('DISPLAY_NAME', 'ユーザ登録編集画面');//screen name
    define('DISPLAY_NAME_NEW_REGISTER', 'ユーザ登録編集画面 登録');//screen name + Register
    define('DISPLAY_NAME_UPDATE', 'ユーザ登録編集画面 更新');//screen name + update
    //database connect failed
    if(fncConnectDB() == false){
        $_SESSION['LOGIN_ERROR'] = 'DB接続に失敗しました。';
        header('Location: login.php');
        exit;
    }
    if(!isset($_SESSION['LOGINUSER_INFO'])){
        //not login
        echo "
        <script>
        alert('".PUBLIC_MSG_008_JPN . "/" . PUBLIC_MSG_008_ENG."');
            window.location.href = 'login.php';
        </script>
        ";

        exit();
    }
    //sesstion time out check
    fncSessionTimeOutCheck(1);
    //error message input
    $_SESSION['EDIT_USER_MSG_ERROR_INPUT'] = '';

    $objUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);

    // get list text translate
    $arrTitle =  array(
        'USER_EDIT_MSG_002',
        'USER_EDIT_MSG_003',
        'USER_EDIT_MSG_004',
        'USER_EDIT_MSG_005',
        'USER_EDIT_MSG_006',
        'USER_EDIT_MSG_007',
        'USER_EDIT_MSG_008',
        'USER_EDIT_MSG_009',
        'USER_EDIT_MSG_010',
        'USER_EDIT_MSG_011',
        'USER_EDIT_MSG_012',
        'USER_EDIT_MSG_013',
        'USER_EDIT_MSG_014',
        'USER_EDIT_MSG_015',
        'USER_EDIT_MSG_016',
        'USER_EDIT_MSG_017',
        'USER_EDIT_MSG_018',
        'USER_EDIT_MSG_019',
        'USER_EDIT_MSG_020',
        'USER_EDIT_MSG_021',
        'USER_EDIT_MSG_022',
        'USER_EDIT_MSG_023',
        'USER_EDIT_MSG_024',
        'USER_EDIT_MSG_025',
        'USER_EDIT_MSG_026',
        'USER_EDIT_MSG_027',
        'USER_EDIT_MSG_028',
        'USER_EDIT_MSG_029',
        'PUBLIC_MSG_009',
        'PUBLIC_MSG_003',
        'PUBLIC_MSG_002',
        'PUBLIC_MSG_049'

    );

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
    if($objUserInfo->intMenuPerm != 1){
        echo "
        <script>
            alert('".$arrTextTranslate['PUBLIC_MSG_009']."');
            window.location.href = 'login.php';
        </script>
        ";
    }
    
    //company change
    if(isset($_POST['mode']) && $_POST['mode'] == 2){
        //get company info
        $arrOneCompany = fncSelectOne(
            "SELECT * FROM m_company WHERE COMPANY_NO=? AND DEL_FLAG=?",
            [$_POST['company_no'], 0],
            DISPLAY_NAME
        );
        //company exist
        if($arrOneCompany){
            //get abbreviations, inst_category_name, group_name
            $arrTempOne = fncSelectOne(
                "SELECT
                ".iff(
                    $objUserInfo->intLanguageType,
                    ' m_company.ABBREVIATIONS_ENG as abbreviations,',
                    ' m_company.ABBREVIATIONS_JPN as abbreviations,'
                )."
                ".iff(
                    $objUserInfo->intLanguageType,
                    ' m_inst_category.INST_CATEGORY_NAME_ENG as INST_CATEGORY_NAME,',
                    ' m_inst_category.INST_CATEGORY_NAME_JPN as INST_CATEGORY_NAME,'
                )."
                ".iff(
                    $objUserInfo->intLanguageType,
                    ' m_group.GROUP_NAME_ENG as GROUP_NAME',
                    ' m_group.GROUP_NAME_JPN as GROUP_NAME'
                )."
                FROM m_company
                INNER JOIN m_inst_category
                ON m_inst_category.INST_CATEGORY_NO = m_company.INST_CATEGORY_NO
                INNER JOIN m_group
                ON m_group.GROUP_NO = m_company.GROUP_NO
                WHERE COMPANY_NO=? AND DEL_FLAG=?",
                [$arrOneCompany['COMPANY_NO'], 0],
                DISPLAY_NAME
            );

            if(is_array($arrTempOne)){
                //select query is successull, return data
                echo json_encode(
                    [$arrTempOne['abbreviations'],
                    $arrTempOne['INST_CATEGORY_NAME'],
                    $arrTempOne['GROUP_NAME']]
                ) ;
            }else{
                //return empty value
                echo json_encode([[],[],[]]);
            }

        }else{
            //return empty value
            echo json_encode([[],[],[]]);
        }
        exit();
    }




    //check input
    if(isset($_POST['mode']) && $_POST['mode'] == 1){
    if(isset($_POST['user_no'])){
        if($_POST['user_no']!=''){
            //log update button
            fncWriteLog(
                LogLevel['Info'],
                LogPattern['Button'],
                DISPLAY_NAME_UPDATE. ' (ユーザID = '.$objUserInfo->strUserID.')'
            );

            //get edit user
            $arrOneUser = fncSelectOne(
                "SELECT USER_NO, PASSWORD, PASSWORD_UP_DATE, PERM_FLAG,
                ANNOUNCE_REG_PERM, JCMG_TAB_PERM, INCIDENT_CASE_REG_PERM, MENU_PERM
                FROM m_user
                WHERE USER_NO=?",
                [$_POST['user_no']],
                DISPLAY_NAME
            );
            //get user Admin_flag
            $arrGroup = fncSelectOne(
                "SELECT ADMIN_FLAG FROM m_group
                INNER JOIN m_company ON m_company.GROUP_NO = m_group.GROUP_NO
                WHERE m_company.COMPANY_NO=? AND m_company.DEL_FLAG=?",
                [$_POST['company_no'], 0],
                DISPLAY_NAME
            );
            if((!is_array($arrGroup) || count($arrGroup) == 0)){
                //sql query failed, store error message to sesssion
                $_SESSION['EDIT_USER_MSG_ERROR_INPUT'] 
                .= $arrTextTranslate['PUBLIC_MSG_003'].'<br>';
            }
        }else{
            //log register button
            fncWriteLog(
                LogLevel['Info'],
                LogPattern['Button'],
                DISPLAY_NAME_NEW_REGISTER . ' (ユーザID = '.$objUserInfo->strUserID.')'
            );


            $arrOneUser = false;
        }


        if((!is_array($arrOneUser) || count($arrOneUser) == 0) 
        && $_POST['user_no'] != ''){
            //edit user not exist
            $_SESSION['EDIT_USER_MSG_ERROR_INPUT'] 
            .= $arrTextTranslate['PUBLIC_MSG_003'].'<br>';

        }else{
            if($_POST['user_no'] == ''){
                //check user_id
                if(!isset($_POST['user_id']) || mb_strwidth($_POST['user_id'])==0){
                    $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['USER_EDIT_MSG_002'].'<br>';
                }else{
                    //number of characters > USER_ID_MAX
                    if(mb_strwidth($_POST['user_id'])>USER_ID_MAX){
                        $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                        .= $arrTextTranslate['USER_EDIT_MSG_004'].'<br>';
                    }

                }
                if(!fncCheckEngText($_POST['user_id'])){
                    //user_id include fullsize characters
                    $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['USER_EDIT_MSG_003'].'<br>';
                }
                
                //get an user with same user_id
                $arrOneUser = fncSelectOne(
                    "SELECT USER_NO from m_user WHERE USER_ID=?",
                    [$_POST['user_id']],
                    DISPLAY_NAME);
                if($arrOneUser){
                    //user_id exist -> return false;
                    $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['USER_EDIT_MSG_005'].'<br>';
                }
            }
            if(
                !isset($_POST['password'])
                || mb_strwidth($_POST['password'])==0
            ){
                //password not input
                $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                .= $arrTextTranslate['USER_EDIT_MSG_006'].'<br>';
            }
            else
            {
                //number of characters > 30
                if(
                    mb_strwidth($_POST['password'])>30
                    || mb_strwidth($_POST['password'])<12
                )
                {
                    $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['USER_EDIT_MSG_008'].'<br>';
                }
                /**
                 * fncCheckEngText2
                 *
                 * @create 2020/02/21 AKB Thang
                 * @update
                 * @param string $strText
                 * @return boolean true: , false:
                 */
                function fncCheckEngText2($strText){
                    //check ',' exist
                    $intPosition = strpos($strText, ',');
                    if($intPosition !== false){
                    	return false;
                    }

                    return (strlen($strText) != strlen(utf8_decode($strText))) ? false : true;

                }
                //check half size and special char
                if(!fncCheckEngText2($_POST['password'])){
                    $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['USER_EDIT_MSG_007'].'<br>';
                }
                //must have lower case and upper case

                $check = (preg_match('/[A-Z]+/', $_POST['password'])
                    && preg_match('/[a-z]+/', $_POST['password']));
                if(!$check){
                    $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['USER_EDIT_MSG_009'].'<br>';
                }
            }

            if(
                !isset($_POST['expiration_date_s'])
                || mb_strwidth($_POST['expiration_date_s'])==0
            )
            {
                //expiration_date_s not input
                $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                .= $arrTextTranslate['USER_EDIT_MSG_010'].'<br>';
            }
            else
            {
                //check date
                $arrTmpDate = explode('/', $_POST['expiration_date_s']);
                if(
                    isset($arrTmpDate[0])
                    && isset($arrTmpDate[1])
                    && isset($arrTmpDate[2])
                    && is_numeric($arrTmpDate[0])
                    && is_numeric($arrTmpDate[1])
                    && is_numeric($arrTmpDate[2])
                ){
                    $blnCheckDate = checkdate($arrTmpDate[1], $arrTmpDate[2], $arrTmpDate[0]);
                }else{
                    $blnCheckDate = false;
                }

                if(!$blnCheckDate){
                    //date format not valid
                    $_SESSION['EDIT_USER_MSG_ERROR_INPUT'] 
                    .= $arrTextTranslate['USER_EDIT_MSG_011'].'<br>';
                }

            }

            if(!isset($_POST['expiration_date_e']) 
            || mb_strwidth($_POST['expiration_date_e'])==0){
                //expiration_date_e not input
                $_SESSION['EDIT_USER_MSG_ERROR_INPUT'] 
                .= $arrTextTranslate['USER_EDIT_MSG_012'].'<br>';
            }else{
                //check date
                $arrTmpDate = explode('/', $_POST['expiration_date_e']);
                if(
                    isset($arrTmpDate[0])
                    && isset($arrTmpDate[1])
                    && isset($arrTmpDate[2])
                    && is_numeric($arrTmpDate[0])
                    && is_numeric($arrTmpDate[1])
                    && is_numeric($arrTmpDate[2])
                ){
                    $blnCheckDate = checkdate($arrTmpDate[1], $arrTmpDate[2], $arrTmpDate[0]);
                }else{
                    $blnCheckDate = false;
                }

                if(!$blnCheckDate){
                    //date format not valid
                    $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['USER_EDIT_MSG_013'].'<br>';
                }

                //▼2020/06/08 KBS S.Tasaki 日付の比較を文字列型ではなく日付型にて比較するよう修正
                //date start > date end
                $strSDatePost = trim($_POST['expiration_date_s']);
                $strEDatePost = trim($_POST['expiration_date_e']);
                
                if(strtotime($strSDatePost) > strtotime($strEDatePost)) {
                    $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['USER_EDIT_MSG_014'].'<br>';
                }
                //▲2020/06/08 KBS S.Tasaki

            }
            if(!isset($_POST['language_type']) || $_POST['language_type']==''){
                //language type not input
                $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                .= $arrTextTranslate['USER_EDIT_MSG_015'].'<br>';
            }

            if(!isset($_POST['company_no']) || $_POST['company_no']==''){
                //company not selected
                $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                .= $arrTextTranslate['USER_EDIT_MSG_016'].'<br>';
            }

            if(!isset($_POST['organization'])){
                //organization not input
                $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                .= $arrTextTranslate['USER_EDIT_MSG_017'].'<br>';
            }else{
                //number of characters > 50
                if(mb_strwidth($_POST['organization'])>50){
                    $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['USER_EDIT_MSG_017'].'<br>';
                }

            }

            if(!isset($_POST['user_name'])){
                //username not input
                $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                .= $arrTextTranslate['USER_EDIT_MSG_018'].'<br>';
            }else{
                //number of characters > 20
                if(mb_strwidth($_POST['user_name'])>20){
                    $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['USER_EDIT_MSG_018'].'<br>';
                }

            }

            if(!isset($_POST['mail_address'])  || $_POST['mail_address']==''){
                //maild address not input
                $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                .= $arrTextTranslate['USER_EDIT_MSG_019'].'<br>';
            }else{
                //number of characters > 50
                if(mb_strwidth($_POST['mail_address'])>50){
                    $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['USER_EDIT_MSG_021'].'<br>';
                }
                //check half size and special char
                if(!fncCheckEngText($_POST['mail_address'])){
                    $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['USER_EDIT_MSG_020'].'<br>';
                }

            }

            if(!isset($_POST['tel'])){
                //tel not input
                $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                .= $arrTextTranslate['USER_EDIT_MSG_022'].'<br>';
            }else{
                //number of characters > 20
                if(mb_strwidth($_POST['tel'])>20){
                    $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['USER_EDIT_MSG_023'].'<br>';
                }
                //▼2020/06/05 KBS S.Tasaki 電話番号は数値または「-」のみの入力とする。
                if($_POST['tel'] != ''){
                    if(!preg_match('/^[-0-9]+$/', $_POST['tel'])){
                        $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                        .= $arrTextTranslate['USER_EDIT_MSG_022'].'<br>';
                    }
                }
                //▲2020/06/05 KBS S.Tasaki

            }

            if(!isset($_POST['fax'])){
                //fax not input
                $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                .= $arrTextTranslate['USER_EDIT_MSG_024'].'<br>';
            }else{
                //number of characters > 20
                if(mb_strwidth($_POST['fax'])>20){
                    $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['USER_EDIT_MSG_025'].'<br>';
                }
                //▼2020/06/05 KBS S.Tasaki FAXは数値または「-」のみの入力とする。
                if($_POST['fax'] != ''){
                    if(!preg_match('/^[-0-9]+$/', $_POST['fax'])){
                        $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                        .= $arrTextTranslate['USER_EDIT_MSG_024'].'<br>';
                    }
                }
                //▲2020/06/05 KBS S.Tasaki

            }

            if(!isset($_POST['zip_code'])){
                //zip code not input
                $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                .= $arrTextTranslate['USER_EDIT_MSG_026'].'<br>';
            }else{
                //number of characters > 8
                if(mb_strwidth($_POST['zip_code'])>8){
                    $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['USER_EDIT_MSG_027'].'<br>';
                }
                //▼2020/06/05 KBS S.Tasaki 郵便番号は「999-9999」形式にて、数値または「-」のみの入力とする。
                if($_POST['zip_code'] != ''){
                    if(!preg_match('/^([0-9]{3})(-[0-9]{4})?$/i', $_POST['zip_code'])){
                        $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                        .= $arrTextTranslate['USER_EDIT_MSG_026'].'<br>';
                    }
                }
                //▲2020/06/05 KBS S.Tasaki

            }

            if(!isset($_POST['address'])){
                //address not input
                $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                .= $arrTextTranslate['USER_EDIT_MSG_028'].'<br>';
            }else{
                //number of characters > 100
                if(mb_strwidth($_POST['address'])>100){
                    $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['USER_EDIT_MSG_028'].'<br>';
                }

            }

            if(!isset($_POST['remarks'])){
                //remarks not input
                $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                .= $arrTextTranslate['USER_EDIT_MSG_029'].'<br>';
            }else{
                //number of characters > 200
                if(mb_strwidth($_POST['remarks'])>200){
                    $_SESSION['EDIT_USER_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['USER_EDIT_MSG_029'].'<br>';
                }

            }

        }
    }


    if($_SESSION['EDIT_USER_MSG_ERROR_INPUT'] != ''){
        //show error message
?>
$('.edit-error').html('<?php echo $_SESSION['EDIT_USER_MSG_ERROR_INPUT']; ?>');
<?php
        }else{

            //get company
            $arrOneCompany = fncSelectOne(
                "SELECT COMPANY_NO
                FROM m_company
                WHERE COMPANY_NO=?",
                [$_POST['company_no']],
                DISPLAY_NAME
            );


            if($_POST['user_no']!=''){
                //update user
                //if company does not exist, show error
                if(!is_array($arrOneCompany) || count($arrOneCompany) == 0){
                    //add error log
                    fncWriteLog(
                        LogLevel['Error'] ,
                        LogPattern['Error'],
                        DISPLAY_NAME_NEW_REGISTER . ' ' . $arrTextTranslate['PUBLIC_MSG_003']
                    );
                    echo "$('.edit-error').html('".$arrTextTranslate['PUBLIC_MSG_003']."');";
                    exit();
                }

                if($arrOneUser['PERM_FLAG'] == 1 && $arrGroup['ADMIN_FLAG'] != 1){
                	$objResult = fncProcessData(
	                    [
	                        'PASSWORD'  =>  $_POST['password'],
	                        'PASSWORD_UP_DATE'  =>  iff(
	                            $_POST['password'] == $arrOneUser['PASSWORD'],
	                            $arrOneUser['PASSWORD_UP_DATE'],
	                            date('Y-m-d H:i:s')
	                        ),
	                        'EXPIRATION_DATE_S'  => $_POST['expiration_date_s'],
	                        'EXPIRATION_DATE_E'  => $_POST['expiration_date_e'],
	                        'LANGUAGE_TYPE'  => (isset($_POST['language_type'])
	                        && $_POST['language_type']!='0' ? 1 : 0),
	                        'COMPANY_NO'  => $_POST['company_no'],
	                        'ORGANIZATION'  => $_POST['organization'],
	                        'USER_NAME'  => $_POST['user_name'],
	                        'MAIL_ADDRESS'  => $_POST['mail_address'],
	                        'TEL'  => $_POST['tel'],
	                        'FAX'  => $_POST['fax'],
	                        'ZIP_CODE'  => $_POST['zip_code'],
	                        'ADDRESS'  => $_POST['address'],
	                        'REMARKS'  => $_POST['remarks'],
	                        'ANNOUNCE_REG_PERM' =>0,
	                        'INCIDENT_CASE_REG_PERM' => 0,
	                        'MENU_PERM' => 0,
	                        'UP_USER_NO'    =>  $objUserInfo->intUserNo,
	                        'UP_DATE'   => date('Y-m-d H:i:s')
	                    ],
	                    'm_user',
	                    'USER_NO=?',
	                    array($_POST['user_no']),
	                    DISPLAY_NAME
	                );

                }else{
                	$objResult = fncProcessData(
	                    [
	                        'PASSWORD'  =>  $_POST['password'],
	                        'PASSWORD_UP_DATE'  =>  iff(
	                            $_POST['password'] == $arrOneUser['PASSWORD'],
	                            $arrOneUser['PASSWORD_UP_DATE'],
	                            date('Y-m-d H:i:s')
	                        ),
	                        'EXPIRATION_DATE_S'  => $_POST['expiration_date_s'],
	                        'EXPIRATION_DATE_E'  => $_POST['expiration_date_e'],
	                        'LANGUAGE_TYPE'  => (isset($_POST['language_type'])
	                        && $_POST['language_type']!='0' ? 1 : 0),
	                        'COMPANY_NO'  => $_POST['company_no'],
	                        'ORGANIZATION'  => $_POST['organization'],
	                        'USER_NAME'  => $_POST['user_name'],
	                        'MAIL_ADDRESS'  => $_POST['mail_address'],
	                        'TEL'  => $_POST['tel'],
	                        'FAX'  => $_POST['fax'],
	                        'ZIP_CODE'  => $_POST['zip_code'],
	                        'ADDRESS'  => $_POST['address'],
	                        'REMARKS'  => $_POST['remarks'],
	                        'UP_USER_NO'    =>  $objUserInfo->intUserNo,
	                        'UP_DATE'   => date('Y-m-d H:i:s')
	                    ],
	                    'm_user',
	                    'USER_NO=?',
	                    array($_POST['user_no']),
	                    DISPLAY_NAME
	                );
                }


                if(is_object($objResult) && $objResult->rowCount()>0){
                    //update successfully
                    echo 0;

                }else{
                    //update not successfully, log error
                    fncWriteLog(
                        LogLevel['Error'],
                        LogPattern['Error'],
                        DISPLAY_NAME . ' ' . $arrTextTranslate['PUBLIC_MSG_003']
                    );
                    echo "$('.edit-error').html('".$arrTextTranslate['PUBLIC_MSG_003']."');";
                }


            }else{
                //if company does not exist, show error
                if(!is_array($arrOneCompany) || count($arrOneCompany) == 0){

                    //log errror
                    fncWriteLog(
                        LogLevel['Error'],
                        LogPattern['Error'],
                        DISPLAY_NAME . ' ' . $arrTextTranslate['PUBLIC_MSG_002']
                    );
                    echo "$('.edit-error').html('".$arrTextTranslate['PUBLIC_MSG_002']."');";
                    exit();
                }
                //get max user_no
                $arrLastUser = fncSelectOne(
                    "SELECT * FROM m_user ORDER BY USER_NO DESC",
                    [],
                    DISPLAY_NAME
                );
                if(is_array($arrLastUser) && count($arrLastUser)){
                    //user_no = max user_no + 1
                    $intUserNo = $arrLastUser['USER_NO'] + 1;
                }else{
                    //max user_no not exist, user_no = 1
                    $intUserNo = 1;
                }
                //insert user
                $objResult = fncProcessData(
                    [
                        'USER_NO' => $intUserNo,
                        'USER_ID'  => $_POST['user_id'],
                        'PASSWORD'  =>  $_POST['password'],
                        'PASSWORD_UP_DATE'  =>  date('Y-m-d H:i:s'),
                        'EXPIRATION_DATE_S'  => $_POST['expiration_date_s'],
                        'EXPIRATION_DATE_E'  => $_POST['expiration_date_e'],
                        'LANGUAGE_TYPE'  => (isset($_POST['language_type'])
                        && $_POST['language_type']!='0' ? 1 : 0),
                        'COMPANY_NO'  => $_POST['company_no'],
                        'ORGANIZATION'  => $_POST['organization'],
                        // 'GROUP_NO'  => $arrOneCompany['GROUP_NO'],
                        'USER_NAME'  => $_POST['user_name'],
                        'MAIL_ADDRESS'  => $_POST['mail_address'],
                        'TEL'  => $_POST['tel'],
                        'FAX'  => $_POST['fax'],
                        'ZIP_CODE'  => $_POST['zip_code'],
                        'ADDRESS'  => $_POST['address'],
                        'REMARKS'  => $_POST['remarks'],
                        'REG_USER_NO'  => $objUserInfo->intUserNo,

                        'PERM_FLAG' =>  0,
                        'REG_DATE'  =>  date('Y-m-d H:i:s'),

                        // 2020/04/14 T.Masuda 新規登録時権限を無効にして登録
                        'JCMG_TAB_PERM' => 0,
                        'ANNOUNCE_REG_PERM' => 0,
                        'QUERY_REG_PERM' => 0,
                        'INCIDENT_CASE_REG_PERM' => 0,
                        'REQUEST_REG_PERM' => 0,
                        'INFORMATION_REG_PERM' => 0,
                        'MENU_PERM' => 0,
                        'ANNOUNCE_MAIL' => 0,
                        'BULLETIN_BOARD_MAIL' => 0,
                        'INCIDENT_CASE_MAIL' => 0,
                        'REQUEST_CONTENTS_MAIL' => 0,
                        // 2020/04/14 T.Masuda
                    ],
                    'm_user',
                    '',
                    [],
                    DISPLAY_NAME
                );
                if(is_object($objResult) && $objResult->rowCount()>0){
                    //insert user success
                    echo 1;

                }else{
                    //insert not success, log error
                    fncWriteLog(
                        LogLevel['Error'],
                        LogPattern['Error'],
                        DISPLAY_NAME . ' ' . $arrTextTranslate['PUBLIC_MSG_002']
                    );
                    echo "$('.edit-error').html('".$arrTextTranslate['PUBLIC_MSG_002']."');";
                }
            }
        }
    }
?>