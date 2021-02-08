<?php
/*
* @password_chg_proc.php
*
* @create 2020/03/25 KBS Tam.nv
* @update
*/
const DISPLAY_NAME = 'パスワード変更画面';


require_once('common/common.php');

//check request method, if is GET method will reject
if(empty($_POST)){
    echo "<script>alert('".PUBLIC_MSG_049_JPN."/".PUBLIC_MSG_049_ENG."');
    history.back();</script>";
    exit();
}

//define array translate text message
$arrMSG = [
    'PUBLIC_MSG_003',

    'PASSWORD_CHG_TEXT_001',
    'PASSWORD_CHG_TEXT_002',
    'PASSWORD_CHG_TEXT_003',
    'PASSWORD_CHG_TEXT_004',
    'PASSWORD_CHG_TEXT_005',
    'PASSWORD_CHG_TEXT_006',

    'PASSWORD_CHG_MSG_002',
    'PASSWORD_CHG_MSG_003',
    'PASSWORD_CHG_MSG_004',
    'PASSWORD_CHG_MSG_005',
    'PASSWORD_CHG_MSG_006',
    'PASSWORD_CHG_MSG_007',
    'PASSWORD_CHG_MSG_008',
    'PASSWORD_CHG_MSG_009',
    'PASSWORD_CHG_MSG_010',
    'PASSWORD_CHG_MSG_011',
    'PUBLIC_BUTTON_017',
    'PUBLIC_BUTTON_018',

    'PASSWORD_CHG_MSG_001',
];
//Define notifications in account language
$strError = '';
// check if is post method to process
if(count($_POST) > 0){
    //DB connection
    if(fncConnectDB() == false){
        $_SESSION['LOGIN_ERROR'] = 'DB接続に失敗しました。';
        header('Location: login.php');
        exit;
    }
    // creat strCSrf to pass validate session input and csrf privacy
    $strCsrf = $_POST['X-CSRF-TOKEN'];
    //check user_id in session, if exists will destroy it
    if(@$_SESSION['user_id']){
        //Log
        $strViewLog = DISPLAY_NAME.' 更新 (ユーザID = '.$_SESSION['user_id'].') ';
        fncWriteLog(LogLevel['Info'] , LogPattern['Button'], $strViewLog);
        $_SESSION['user_id'] = NULL;
    }

    //get user data
    $arrUser = fncGetUserById($_POST['user_no']);

    $_SESSION['user_no'] = fncHtmlSpecialChars($_POST['user_no']);
    // if user exists process
    if($arrUser){
        // set text translate array data
        $arrText = getListTextTranslate($arrMSG,$arrUser[0]['LANGUAGE_TYPE']);
        // set text translate array data to session
        $_SESSION['arrText'] = $arrText;
        // check password not null
        if(!isset($_POST['txtPasword'])){
            //ログビュー
            $strViewLog = DISPLAY_NAME.' 表示 (ユーザID = '.$arrUser[0]['USER_ID'].') '.
                (isset($_SERVER['HTTP_REFERER']) ? $_SERVER['HTTP_REFERER'] : null);
            fncWriteLog(LogLevel['Info'] , LogPattern['View'], $strViewLog);
            $_SESSION['user_id'] = $arrUser[0]['USER_ID'];
        }
    }else{
        $arrText = $_SESSION['arrText'];
    }
    // check password not null, if null comeback with error
    if(isset($_POST['txtPasword'])){
        //$arrUser = fncGetUserById($_POST['user_no']);
        $strPassword = $_POST['txtPasword'];
        $strNewPassword = $_POST['txtNewPassword'];
        $strNewPassword2 = $_POST['txtNewPassword2'];
        // if exists user process validate input
        if($arrUser){
            //Password not entered
            if(mb_strlen($strPassword) == 0){
                @$_SESSION['PASSWORD_CHG_MSG'] .= $arrText['PASSWORD_CHG_MSG_002']. '<br>';
            }
            //new password not entered
            if(mb_strlen($strNewPassword) == 0){
                @$_SESSION['PASSWORD_CHG_MSG'] .= $arrText['PASSWORD_CHG_MSG_003']. '<br>';
            }
            //new password repeat not entered
            if(mb_strlen($strNewPassword2) == 0){
                @$_SESSION['PASSWORD_CHG_MSG'] .= $arrText['PASSWORD_CHG_MSG_007']. '<br>';
            }

            //validate new password charset
            if(!mb_detect_encoding($strNewPassword, 'ASCII', true) || (strpos($strNewPassword, ',') !== false )) {
                @$_SESSION['PASSWORD_CHG_MSG'] .= $arrText['PASSWORD_CHG_MSG_004']. '<br>';
            }

            //validate new password length
            if(mb_strlen($strNewPassword) < 12 || mb_strlen($strNewPassword) > 30){
                @$_SESSION['PASSWORD_CHG_MSG'] .= $arrText['PASSWORD_CHG_MSG_005']. '<br>';
            }
            //validate new password exists upper case
            //▼2020/05/29 KBS S.Tasaki 大文字のみの場合もエラーとなるよう修正
            $blnCheck = (preg_match('/[A-Z]+/', $strNewPassword) && preg_match('/[a-z]+/', $strNewPassword));
            if (!$blnCheck) {
                @$_SESSION['PASSWORD_CHG_MSG'] .= $arrText['PASSWORD_CHG_MSG_006']. '<br>';
            }
            //▲2020/05/29 KBS S.Tasaki

            //compare input old password with password in database
            if ($strPassword !== $arrUser[0]['PASSWORD']) {
                @$_SESSION['PASSWORD_CHG_MSG'] .= $arrText['PASSWORD_CHG_MSG_008']. '<br>';
            }
            //compare input new password with input old password
            if ($strNewPassword === $arrUser[0]['PASSWORD']) {
                @$_SESSION['PASSWORD_CHG_MSG'] .= $arrText['PASSWORD_CHG_MSG_009']. '<br>';
            }
            //compare input new password  with input new password repeat
            if ($strNewPassword !== $strNewPassword2) {
                @$_SESSION['PASSWORD_CHG_MSG'] .= $arrText['PASSWORD_CHG_MSG_010']. '<br>';
            }
        }else{
            //error not exists user
            @$_SESSION['PASSWORD_CHG_MSG'] .= $arrText['PUBLIC_MSG_003']. '<br>';
        }
        //check password expired date
        if(mb_strlen(@$_SESSION['PASSWORD_CHG_MSG']) !== 0){
            //▼2020/05/27 KBS T.Masuda エラーメッセージ初期化
            $_SESSION['PASSWORD_CHG_EVER_MSG'] = html_entity_decode($arrText['PASSWORD_CHG_TEXT_002']).'<br><br>';
            //▲2020/05/27 KBS T.Masuda
            
            @$_SESSION['PASSWORD_CHG_MSG'] = $_SESSION['PASSWORD_CHG_MSG'];
        }
        //if don't have error update new password and go to login page
        if(mb_strlen(@$_SESSION['PASSWORD_CHG_MSG']) == 0 ){

            $intResult = fncUpdateUserById($_POST['user_no'],$strNewPassword);
            // if update success redirect to login page, if have error comeback with error message
            if($intResult){
                //▼2020/06/01 KBS S.Tasaki 完了メッセージの表示を追加
                echo '<script>alert("'.$arrText['PASSWORD_CHG_MSG_011'].'");
                      window.location.href="login.php";</script>';
                //▲2020/06/01 KBS S.Tasaki
                exit;
            }else{
                @$_SESSION['PASSWORD_CHG_MSG'] .= $arrText['PUBLIC_MSG_003'];
            }
        }
    }else{
        //▼2020/05/27 KBS T.Masuda エラーメッセージ初期化
        $_SESSION['PASSWORD_CHG_EVER_MSG'] = html_entity_decode($arrText['PASSWORD_CHG_TEXT_002']).'<br><br>';
        //▲2020/05/27 KBS T.Masuda
        
        $_SESSION['PASSWORD_CHG_MSG'] = @$_SESSION['PASSWORD_CHG_MSG'];
    }
}


/**
 * IDでユーザを取得
 *
 * @create 2020/03/25 KBS Tam.nv
 * @update
 * @param $intUserNo ユーザID
 * @return array ユーザアレイ
 */
function fncGetUserById($intUserNo){
    $strSQL = '';
    $strSQL .= ' SELECT *';
    $strSQL .= ' FROM m_user ';
    $strSQL .= ' WHERE USER_NO =:USER_NO ';
    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
    $objStmt->bindParam(':USER_NO', $intUserNo);
    $strLogSql = DISPLAY_NAME.$strSQL;
    $strLogSql = str_replace(':USER_NO', $intUserNo,$strLogSql);
    fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);
    try {
        $objStmt->execute();
        $arrResult = $objStmt->fetchAll(PDO::FETCH_ASSOC);
    }catch (Exception $e) {
        // other mysql exception (not duplicate key entry)
        fncWriteLog(LogLevel['Error'], LogPattern['Error'],
            DISPLAY_NAME.' '.$e->getMessage());
    }
    return @$arrResult;
}

/**
 * m_user update user by user no
 *
 * @create 2020/03/26 KBS Tam.nv
 * @update
 * @param $intUserNo user no
 * @param $strPassword new password
 * @return boolean true or null
 */
function fncUpdateUserById($intUserNo,$strPassword){
    $dtmUpdate = date('Y-m-d H:i:s');
    $strSQL = '';
    $strSQL .= ' UPDATE m_user';
    $strSQL .= ' SET PASSWORD = :PASSWORD,  ';
    $strSQL .= ' PASSWORD_UP_DATE = :PASSWORD_UP_DATE,  ';
    $strSQL .= ' UP_USER_NO = :UP_USER_NO,  ';
    $strSQL .= ' UP_DATE = :UP_DATE  ';
    $strSQL .= ' WHERE USER_NO =:USER_NO ';
    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
    $objStmt->bindParam(':USER_NO', $intUserNo);
    $objStmt->bindParam(':UP_USER_NO', $intUserNo);
    $objStmt->bindParam(':PASSWORD_UP_DATE', $dtmUpdate);
    $objStmt->bindParam(':UP_DATE', $dtmUpdate);
    $objStmt->bindParam(':PASSWORD', $strPassword);
    $strLogSql = DISPLAY_NAME.$strSQL;
    $strLogSql = str_replace(':USER_NO', $intUserNo,$strLogSql);
    $strLogSql = str_replace(':UP_USER_NO', $intUserNo,$strLogSql);
    $strLogSql = str_replace(':PASSWORD', $strPassword,$strLogSql);
    $strLogSql = str_replace(':UP_DATE', $dtmUpdate,$strLogSql);
    $strLogSql = str_replace(':PASSWORD_UP_DATE', $dtmUpdate,$strLogSql);
    fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);
    try {
        return $objStmt->execute();
    }catch (Exception $e) {
        // other mysql exception (not duplicate key entry)
        fncWriteLog(LogLevel['Error'], LogPattern['Error'],
            DISPLAY_NAME.' '.$e->getMessage());
    }
}
