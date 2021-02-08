<?php
/*
* @query_view_proc.php
*
* @create 2020/02/20 KBS Tam.nv
* @update
*/

require_once('common/common.php');
require_once('common/validate_user.php');
require_once('portal_function.php');

const DISPLAY_NAME = '問い合わせ画面';
const DISPLAY_NAME_PORTAL_IN_QUERY = 'ポータル画面';
const ACTION_NAME = '問い合わせ画面　投稿';

//define array translate text message
$arrMSG = [
    'PUBLIC_TEXT_001',
    'PUBLIC_TEXT_002',
    'PUBLIC_TEXT_003',
    'PUBLIC_TEXT_004',
    'PUBLIC_TEXT_005',
    'PUBLIC_TEXT_006',
    'PUBLIC_TEXT_007',
    'PUBLIC_TEXT_008',
    'PUBLIC_MSG_009',

    'PUBLIC_BUTTON_001',
    'PUBLIC_BUTTON_002',
    'PUBLIC_BUTTON_003',
    'PUBLIC_BUTTON_004',
    'PUBLIC_BUTTON_005',
    'QUERY_VIEW_TEXT_001',
    'USER_PERM_MSG_001',

    'QUERY_VIEW_MSG_001',
    'QUERY_VIEW_MSG_002',
    'QUERY_VIEW_MSG_003',
    'QUERY_VIEW_MSG_004',
    'PUBLIC_MSG_049',

];

if (!isset($arrText)) {
    $arrText = array();
}
// Translate text folow by language Type
$arrText = array_merge(
    getListTextTranslate($arrMSG, $objLoginUserInfo->intLanguageType),
    $arrText
);
// check permission
if (!$objLoginUserInfo->intQueryRegPerm
    && $_SERVER['REQUEST_URI'] == '/information_sharing/query_view.php') {
    $alertMsg = $arrText['PUBLIC_MSG_009'];
    echo "
        <script>
            alert('" . $alertMsg . "');
            window.location.href = 'login.php';
        </script>
    ";
}

// check request method, if method is GET, break.
if(empty($_POST)){
    // check language type to set message
    if(isset($objLoginUserInfo->intLanguageType)){
        $strMes = $arrText['PUBLIC_MSG_049'];
    }else{
        $strMes = ".PUBLIC_MSG_049_JPN."/".PUBLIC_MSG_049_ENG.";
    }
    echo "<script>alert('".$strMes."');
    history.back();</script>";
}
$intFlagQuery = @$_POST['intFlagQuery'];
$intFlagFromBtn = @$_POST['fromBtn'];
//flag check from click button show query view. It is action open query log view
if($intFlagFromBtn){
    $intFlagQuery = 1;
}

$intCheck24h = 0;
$intIsAjax = @$_POST['isAjax'];
// check method, only process action insert query.
if (@$_POST['action'] == 'insert_query') {
    $strViewLog = ACTION_NAME.'(ユーザID = ' . $objLoginUserInfo->strUserID . ') ';
    fncWriteLog(LogLevel['Info'], LogPattern['Button'], $strViewLog);
    // check permission
    if (!$objLoginUserInfo->intQueryRegPerm) {
            echo 900;
            exit();
        }
    //Validate input
    $arrValidate = validateInputText($_POST, $arrText, $objLoginUserInfo, 1);
    // check result data return. if result is array, it have error.
    if (is_array($arrValidate)) {
        echo json_encode($arrValidate);
    } else {
        // if input txtTarget is null, set value for input
        if(!$_POST['txtTarget']){
            $_POST['txtTarget'] = $arrValidate;
        }
        // validate ok, no error continue insert data to t_log table
        insert($_POST, $objLoginUserInfo);
        echo 1;
    }
} else {
    //check source run function to set DISPLAY NAME
    if($intFlagQuery){
        $strDisplayName = DISPLAY_NAME;
    }else{
        $strDisplayName = DISPLAY_NAME_PORTAL_IN_QUERY;
        $_SESSION["SES_TIME"] = date( "Y/m/d H:i:s", time() );
    }
    $strViewLog = $strDisplayName.'　画面表示(ユーザID = ' . $objLoginUserInfo->strUserID . ') ' .
        (isset($_SERVER['HTTP_REFERER']) ? $_SERVER['HTTP_REFERER'] : null);
    //check if form action show view query. write log start query screen
    if($intFlagFromBtn){
        fncWriteLog(LogLevel['Info'], LogPattern['View'], $strViewLog);
    }
    // check type of time. this variable set time to get data from t_log 24h or all
    if(isset($_POST['typeOfTime'])) {
        $intCheck24h = $_POST['typeOfTime'];
    }
    $arrQuery = get_query($intCheck24h, $intIsAjax,@$_POST['intFlagQuery'],@$_POST['intFlagChangeTime'],$intFlagFromBtn);
    //if data not null get name of user set it
    if (!empty($arrQuery)) {
        $arrUserName = getMapName($arrQuery, $strEndLang,@$_POST['intFlagQuery'],$intFlagFromBtn);
    }
}

$arrayIdInCompany = get_all_id_in_company($objLoginUserInfo->intUserNo,$intFlagQuery,$intFlagFromBtn);
$mapArrIds = array();
foreach ($arrayIdInCompany as $item) {
    $mapArrIds[] = $item['USER_NO'];
}


/**
 * get name of user in query
 *
 * @create 2020/03/04 KBS Tam.nv
 * @update
 * @param $arrQuery array query data
 * @param $strEndLang Language suffix in which the user logs in
 * @return array ID Returns the shortened mapping data set in
 */
function getMapName($arrQuery, $strEndLang,$intFlagQuery,$intFlagFromBtn)
{
    $arrAllIds = array();
    $arrUserName = array();
    foreach ($arrQuery as $item) {
        $arrAllIds[$item['QUERY_USER_NO']] = $item['QUERY_USER_NO'];
    }

    $arrDataUserName = get_query_name_in_array($arrAllIds,$intFlagQuery,$intFlagFromBtn);
    foreach ($arrDataUserName as $us) {
        $arrUserName[$us['USER_NO']] = $us['ABBREVIATIONS' . $strEndLang];
    }
    return $arrUserName;
}

/**
 * t_query insert data to query table
 *
 * @create 2020/03/04 KBS Tam.nv
 * @update
 * @param $arrData stored data in the table includes
 * Includes text source, text target, translation from source to language
 * @param $objLoginUserInfo object user
 * @return
 */
function insert($arrData, $objLoginUserInfo)
{
    $intFlag = 1;
    if ($arrData['ckeckMan'] == 'false') {
        $intFlag = 0;
    }
    // check type translate , English to Japanese type = 1, Japanese to English type = 1
    if ($arrData['sleTran'] == 'ej') {
        $strEn = $arrData['txtSource'];
        $strJpn = $arrData['txtTarget'];
        $intLangType = 1;
    } else {
        $strEn = $arrData['txtTarget'];
        $strJpn = $arrData['txtSource'];
        $intLangType = 0;
    }
    $strSQL = '';
    $strSQL .= ' INSERT INTO t_query ';
    $strSQL .= ' VALUES(';
    $strSQL .= '(SELECT ISNULL(MAX( QUERY_NO ), 0)+ 1  FROM t_query),';
    $strSQL .= ':QUERY_DATE,';
    $strSQL .= ':QUERY_USER_NO,';
    $strSQL .= ':CONTENT_JPN,';
    $strSQL .= ':CONTENTS_ENG,';
    $strSQL .= ':LANGUAGE_TYPE,';
    $strSQL .= ':CORRECTION_FLAG';
    $strSQL .= ')';
    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
    $dtmDate = date('Y-m-d H:i:s');
    $objStmt->bindParam(':QUERY_DATE', $dtmDate);
    $objStmt->bindParam(':QUERY_USER_NO', $objLoginUserInfo->intUserNo);
    $objStmt->bindParam(':CONTENT_JPN', $strJpn);
    $objStmt->bindParam(':CONTENTS_ENG', $strEn);
    $objStmt->bindParam(':LANGUAGE_TYPE', $intLangType);
    $objStmt->bindParam(':CORRECTION_FLAG', $intFlag, PDO::PARAM_INT);
    $strLogSql = DISPLAY_NAME . $strSQL;
    $strLogSql = str_replace(':QUERY_DATE', date('Y-m-d H:i:s'), $strLogSql);
    $strLogSql = str_replace(':QUERY_USER_NO', $objLoginUserInfo->intUserNo, $strLogSql);
    $strLogSql = str_replace(':CONTENT_JPN', $strJpn, $strLogSql);
    $strLogSql = str_replace(':CONTENTS_ENG', $strEn, $strLogSql);
    $strLogSql = str_replace(':LANGUAGE_TYPE', $intLangType, $strLogSql);
    $strLogSql = str_replace(':CORRECTION_FLAG', $intFlag, $strLogSql);
    fncWriteLog(LogLevel['Info'], LogPattern['Sql'], $strLogSql);
    try {
        return $objStmt->execute();
    }catch (Exception $e) {
        fncWriteLog(LogLevel['Error'],
            LogPattern['Error'], DISPLAY_NAME.' '.$e->getMessage());
    }
}

/**
 * t_query get query data
 *
 * @create 2020/03/04 KBS Tam.nv
 * @update
 * @param $intCheck24h flag check gets data in 24 hours
 * @param $intIsAjax flag check from Ajax
 * @param $objLoginUserInfo object user login
 * @return array query list
 */
function get_query($intCheck24h, $intIsAjax = 0,$intFlagQuery = 1,$intFlagChangeTime = 0,$intFlagFromBtn)
{

    //check max id to get update query
    // The first time load data, get from id = 0, when auto reload, only get update record.
    if ($intIsAjax && !$intFlagChangeTime) {
        $intMaxQueryId = isset($_SESSION["maxQueryId"]) ? $_SESSION["maxQueryId"] : 0;
    } else {
        $intMaxQueryId = 0;
        $_SESSION["maxQueryId"] = 0;
    }
    $strSQL = '';
    $strSQL .= ' SELECT * FROM t_query ';
    $strSQL .= ' WHERE';
    if ($intCheck24h) {
        $strSQL .= ' QUERY_DATE >= DATEADD(day, -1, GETDATE()) AND';
    } else {
        $strSQL .= ' QUERY_DATE >= DATEADD(day, -' .QUERY_VIEW_DAY . ', GETDATE()) AND';
    }

    $strSQL .= ' QUERY_NO > :QUERY_NO_MAX';
    //$strSQL .= ' ORDER BY query_date ASC';
    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
    $objStmt->bindParam(':QUERY_NO_MAX', $intMaxQueryId);
    // check source of request to set DISPLAY NAME, from portal or from query view.
    if($intFlagQuery || $intFlagFromBtn){
        $trDisplayName = DISPLAY_NAME;
    }else{
        $trDisplayName = DISPLAY_NAME_PORTAL_IN_QUERY;
    }
    $strLogSql = $trDisplayName . $strSQL;
    $strLogSql = str_replace(':QUERY_NO_MAX', $intMaxQueryId, $strLogSql);
    fncWriteLog(LogLevel['Info'], LogPattern['Sql'], $strLogSql);
    try {
        $objStmt->execute();
        $arrResult = $objStmt->fetchAll(PDO::FETCH_ASSOC);
    }catch (Exception $e) {
        // other mysql exception (not duplicate key entry)
        fncWriteLog(LogLevel['Error'],
            LogPattern['Error'], $trDisplayName.' '.$e->getMessage());
    }
    $_SESSION["maxQueryId"] =
        count($arrResult) ? end($arrResult)['QUERY_NO'] : $intMaxQueryId;
    return $arrResult;
}

/**
 * m_user Get all user_no same company
 *
 * @create 2020/03/04 KBS Tam.nv
 * @update
 * @param $intUserId user_no
 * @return array user_no same company
 */
function get_all_id_in_company($intUserId,$intFlagQuery,$intFlagFromBtn)
{
    $strSQL = '';
    $strSQL .= ' SELECT USER_NO FROM m_user ';
    $strSQL .= ' WHERE';
    $strSQL .= ' COMPANY_NO = (SELECT COMPANY_NO FROM m_user
                WHERE USER_NO =:USER_NO)';
    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
    $objStmt->bindParam(':USER_NO', $intUserId);
    // check flag to set display name write to t_log.
    if($intFlagQuery){
        $trDisplayName = DISPLAY_NAME;
    }else{
        $trDisplayName = DISPLAY_NAME_PORTAL_IN_QUERY;
    }
    $strLogSql = $trDisplayName . $strSQL;
    $strLogSql = str_replace(':USER_NO', $intUserId, $strLogSql);
    // if from portal.php open query view write log, if from query view, no write.
    if($intFlagFromBtn){
        fncWriteLog(LogLevel['Info'], LogPattern['Sql'], $strLogSql);
    }

    try {
        $objStmt->execute();
        $arrResult = $objStmt->fetchAll(PDO::FETCH_ASSOC);
    }catch (Exception $e) {
        // other mysql exception (not duplicate key entry)
        fncWriteLog(LogLevel['Error'],
            LogPattern['Error'], DISPLAY_NAME.' '.$e->getMessage());
    }
    return @$arrResult;
}

/**
 * m_user Get all user information including company information for each user ID list
 *
 * @create 2020/03/04 KBS Tam.nv
 * @update
 * @param $intUserId user_no or user_no list
 * @param $intFlagQuery flag check action from query
 * @return array user with company information.
 */
function get_query_name_in_array($intUserId,$intFlagQuery,$intFlagFromBtn)
{
    $strSQL = '';
    $strSQL .= ' SELECT USER_NO,USER_NAME,ABBREVIATIONS_ENG,ABBREVIATIONS_JPN';
    $strSQL .= ' FROM m_user';
    $strSQL .= ' INNER JOIN m_company ON m_company.COMPANY_NO = m_user.COMPANY_NO';
    $strSQL .= ' WHERE';
    $strSQL .= ' USER_NO IN (' . implode(",", array_map('intval',$intUserId)). ')';
    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
    //flag check source request from portal or query view
    if($intFlagQuery || $intFlagFromBtn){
        $trDisplayName = DISPLAY_NAME;
    }else{
        $trDisplayName = DISPLAY_NAME_PORTAL_IN_QUERY;
    }
    $strLogSql = $trDisplayName . $strSQL;
    //flag to check from action open query view. if open view, write log.
    if($intFlagFromBtn){
        fncWriteLog(LogLevel['Info'], LogPattern['Sql'], $strLogSql);
    }
    try {
        $objStmt->execute();
        $arrResult = $objStmt->fetchAll(PDO::FETCH_ASSOC);
    }catch (Exception $e) {
        // other mysql exception (not duplicate key entry)
        fncWriteLog(LogLevel['Error'],
            LogPattern['Error'], $trDisplayName.' '.$e->getMessage());
    }
    return @$arrResult;
}