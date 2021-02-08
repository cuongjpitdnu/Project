<?php
/*
* @portal_function.php
*
* @create 2020/02/17 KBS Tam.nv
* @update
*/
const DISPLAY_NAME_PORTAL = 'ポータル画面';

require_once('common/common.php');
require_once('common/validate_user.php');

//define array translate text message
$arrMSG = [
    'PORTAL_TEXT_001',
    'PORTAL_TEXT_002',
    'PORTAL_TEXT_003',
    'PORTAL_TEXT_004',
    'PORTAL_TEXT_005',
    'PORTAL_TEXT_006',
    'PORTAL_TEXT_007',
    'PORTAL_TEXT_008',
    'PORTAL_TEXT_009',
    'PORTAL_TEXT_010',
    'PORTAL_TEXT_011',
    'PORTAL_TEXT_012',
    'PORTAL_TEXT_013',
    'PORTAL_TEXT_014',
    'PORTAL_TEXT_015',
    'PORTAL_TEXT_016',
    'PORTAL_TEXT_017',

    'PORTAL_DAILY_BUTTON_001',
    'PORTAL_DAILY_TEXT_001',
    'PUBLIC_BUTTON_001',
    'PORTAL_DAILY_TEXT_002',
    'PORTAL_DAILY_TEXT_003',
    'PORTAL_DAILY_TEXT_004',
    'PORTAL_DAILY_TEXT_005',
    'PORTAL_DAILY_TEXT_005',
    'PORTAL_DAILY_TEXT_005',
    'PORTAL_DAILY_TEXT_005',
    'PORTAL_DAILY_TEXT_005',
    'PORTAL_DAILY_TEXT_005',
    'PORTAL_DAILY_TEXT_006',
    'PORTAL_DAILY_TEXT_007',
    'PORTAL_DAILY_BUTTON_002',
    'PORTAL_DAILY_TEXT_008',
    'PORTAL_DAILY_TEXT_009',
    'PORTAL_DAILY_TEXT_010',
    'PORTAL_DAILY_TEXT_011',
    'PORTAL_DAILY_TEXT_012',
    'PORTAL_DAILY_TEXT_013',

    'PUBLIC_TEXT_001',
    'PUBLIC_TEXT_002',
    'PUBLIC_TEXT_003',
    'PUBLIC_TEXT_004',
    'PUBLIC_TEXT_005',
    'PUBLIC_TEXT_006',
    'PUBLIC_TEXT_007',
    'PUBLIC_TEXT_008',

    'PUBLIC_BUTTON_003',

    'PUBLIC_MSG_001',
    'PUBLIC_MSG_010',
    'PUBLIC_MSG_026',
    'PUBLIC_MSG_028',
    'PUBLIC_MSG_036',
    'PUBLIC_MSG_038',
    'PUBLIC_MSG_042',
    'PUBLIC_MSG_045',
    'PUBLIC_MSG_046',
    'PUBLIC_MSG_047',
    'PUBLIC_MSG_048',
    'USER_PERM_MSG_001',

    //Incident boad
    'PORTAL_INCIDENT_TEXT_001',
    'PORTAL_INCIDENT_TEXT_002',
    'PORTAL_INCIDENT_TEXT_003',
    'PORTAL_INCIDENT_TEXT_004',
    'PORTAL_INCIDENT_TEXT_005',
    'PORTAL_INCIDENT_TEXT_006',
    'PORTAL_INCIDENT_TEXT_007',
    'PORTAL_INCIDENT_TEXT_008',
    'PORTAL_INCIDENT_TEXT_009',
    'PORTAL_INCIDENT_TEXT_010',
    'PORTAL_INCIDENT_TEXT_011',
    'PORTAL_INCIDENT_TEXT_012',
    'PORTAL_INCIDENT_TEXT_013',
    'PORTAL_INCIDENT_TEXT_014',
    'PORTAL_INCIDENT_TEXT_015',
    'PORTAL_INCIDENT_TEXT_016',
    'PORTAL_INCIDENT_TEXT_017',
    'PORTAL_INCIDENT_TEXT_018',
    'PORTAL_INCIDENT_TEXT_019',
    'PORTAL_INCIDENT_TEXT_020',
    'PORTAL_INCIDENT_TEXT_021',

    'PORTAL_INCIDENT_MSG_001',
    'PORTAL_INCIDENT_MSG_002',
    'PORTAL_INCIDENT_MSG_003',
    'PORTAL_INCIDENT_MSG_004',

    'INFORMATION_EDIT_TEXT_005',
    'INFORMATION_EDIT_TEXT_006',

    //JCMG Boad
    'PORTAL_TEXT_017',
    'INCIDENT_CASE_EDIT_TEXT_003',

    'FHD_TITLE_INST',
    'FHD_COMPANY_INST',
    'FHD_TITLE_INFO',
    'FHD_COMPANY_INFO',

    'TITLE_INST',
    'COMPANY_INST',
    'TITLE_INFO',
    'COMPANY_INFO',

];
// Translate text folow by language Type
$arrText = getListTextTranslate($arrMSG,$objLoginUserInfo->intLanguageType);

// Get now time
$strUtcTime = date("m/d H:i", time() - date("Z"));
$strJstTime = date('m/d H:i');

// define array color for background title
$arrColorBgArr = ['red','brown','yellow','green','blue','white'];

/**
 * t_announce get list announce by datetime
 *
 * @create 2020/02/17 KBS Tam.nv
 * @update
 * @param $dtmDate 時間
 * @return array announce
 */
function get_annouce($dtmDate = ''){
    //If there is a time parameter, process it and include it in your SQL query
    if($dtmDate){
        $dtmStart =  date('Y-m-01', strtotime(str_replace('/', '-', $dtmDate)));
        $dtmEnd =  date('Y-m-t', strtotime(str_replace('/', '-', $dtmDate)));
        $dtmEnd =  $dtmEnd ." 23:59:59";
        $strSQLDate = '  AND ((up_date IS NULL AND reg_date BETWEEN :DATE_START1 AND :DATE_END1) OR (up_date IS NOT NULL AND up_date BETWEEN :DATE_START2 AND :DATE_END2))';
    }
    $strSQL = '';
    $strSQL .= ' SELECT ';
    $strSQL .= 't_announce.ANNOUNCE_NO,';
    $strSQL .= 't_announce.TITLE_JPN,';
    $strSQL .= 't_announce.TITLE_ENG,';
    $strSQL .= 't_announce.CONTENTS_JPN,';
    $strSQL .= 't_announce.CONTENTS_ENG,';
    $strSQL .= 't_announce.LANGUAGE_TYPE,';
    $strSQL .= 't_announce.UNTRANSLATED,';
    $strSQL .= 't_announce.COMP_DATE,';
    $strSQL .= 't_announce.DATA_TYPE,';
    $strSQL .= 'ISNULL(t_announce.UP_DATE,t_announce.REG_DATE) AS REG_DATE ';
    $strSQL .= 'FROM t_announce ';
    $strSQL .= ' WHERE';
    $strSQL .= ' COMP_DATE IS NULL';
    $strSQL .= @$strSQLDate;
    $strSQL .= ' ORDER BY REG_DATE DESC';
    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
    //if have time apply sql with timer parameter
    if($dtmDate){
        $objStmt->bindParam(':DATE_START1', $dtmStart);
        $objStmt->bindParam(':DATE_END1', $dtmEnd);
        $objStmt->bindParam(':DATE_START2', $dtmStart);
        $objStmt->bindParam(':DATE_END2', $dtmEnd);
    }
    $strLogSql = DISPLAY_NAME_PORTAL.$strSQL;
    //if have time apply sql with timer parameter
    if($dtmDate){
        $strLogSql = str_replace(':DATE_START1',$dtmStart,$strLogSql);
        $strLogSql = str_replace(':DATE_END1',$dtmEnd,$strLogSql);
        $strLogSql = str_replace(':DATE_START2',$dtmStart,$strLogSql);
        $strLogSql = str_replace(':DATE_END2',$dtmEnd,$strLogSql);
    }
    //write log sql
    fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);

    try {
        $objStmt->execute();
        $arrResult = $objStmt->fetchAll(PDO::FETCH_ASSOC);
    }catch (Exception $e) {
        // other mysql exception (not duplicate key entry)
        fncWriteLog(LogLevel['Error'], LogPattern['Error'], DISPLAY_NAME_PORTAL.' '.$e->getMessage());
    }
    return @$arrResult;
}

/**
 * t_announce translate announce if have record untranslated
 *
 * @create 2020/02/17 KBS Tam.nv
 * @update
 * @param $arrAnnoList array announce unstrslate
 * @param $objLoginUserInfo object user login
 * @return
 */
function tranInfo($arrAnnoList,$objLoginUserInfo){
    // check exists language type and count unstralate record. if not null continue translate .
    if(count($arrAnnoList)>0){
        $strTitle = '';
        $strContent = '';
        $arrIds = array();

        //convert array to list. multiple record translate one time
        foreach ($arrAnnoList as $arrItem){
            // if item untranslated add title to list
            if(!$arrItem['UNTRANSLATED']){
                if($strTitle){
                    $strTitle .= ' [KBSMARCODE] '.$arrItem['TITLE_JPN'];
                }else{
                    $strTitle .= $arrItem['TITLE_JPN'];
                }
                // if item untranslated add content to list
                if($strContent){
                    $strContent .= ' [KBSMARCODE] '.$arrItem['CONTENTS_JPN'];
                }else{
                    $strContent .= $arrItem['CONTENTS_JPN'];
                }
                $arrIds[] = $arrItem['ANNOUNCE_NO'];
            }
        }

        //count number data need transalte, if not null process call amazon service
        if(count($arrIds)){
            //transalte title
            $strTranTitle = tranAmazon($strTitle);
            
            if(is_array($strTranTitle) && $strTranTitle['error'] == 1){
            }else{
                //return array record, convert string to array with markcode.
                $arrTranTitle = explode(' [KBSMARCODE] ',$strTranTitle);
            }
            
            //transalte content
            $strTranContent = tranAmazon($strContent);
            
            if(is_array($strTranContent) && $strTranContent['error'] == 1){
            }else{
                //return array record, convert string to array with markcode.
                $arrTranContent = explode(' [KBSMARCODE] ',$strTranContent);
            }
            
            //update to data
            foreach ($arrIds as $intKey => $intId){
                $dtmDate = date('Y-m-d H:i:s');
                $strSQL = '';
                $strSQL .= ' UPDATE t_announce SET ';
                $strSQL .= ' TITLE_ENG = :TITLE_ENG, ';
                $strSQL .= ' CONTENTS_ENG = :CONTENTS_ENG, ';
                $strSQL .= ' UP_USER_NO = :UP_USER_NO, ';
                $strSQL .= ' UP_DATE = :UP_DATE, ';
                $strSQL .= ' UNTRANSLATED = 1 ';
                $strSQL .= ' WHERE ANNOUNCE_NO = :ANNOUNCE_NO; ';
                $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
                $objStmt->bindParam(':TITLE_ENG', $arrTranTitle[$intKey]);
                $objStmt->bindParam(':CONTENTS_ENG', $arrTranContent[$intKey]);
                $objStmt->bindParam(':UP_USER_NO', $objLoginUserInfo->intUserNo);
                $objStmt->bindParam(':UP_DATE', $dtmDate);
                $objStmt->bindParam(':ANNOUNCE_NO',$intId);
                $strLogSql = DISPLAY_NAME_PORTAL.$strSQL;
                $strLogSql = str_replace(':TITLE_ENG', $arrTranTitle[$intKey],$strLogSql);
                $strLogSql = str_replace(':CONTENTS_ENG', $arrTranContent[$intKey],$strLogSql);
                $strLogSql = str_replace(':ANNOUNCE_NO', $intId,$strLogSql);
                $strLogSql = str_replace(':UP_USER_NO',$objLoginUserInfo->intUserNo,$strLogSql);
                $strLogSql = str_replace(':UP_DATE', $dtmDate,$strLogSql);
                fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);
                $objStmt->execute();
            }
        }
    }
}

/**
 * t_bulletin translate bulletin
 *
 * @create 2020/02/17 KBS Tam.nv
 * @update
 * @param $arrList array data untranslated.
 * @param $objLoginUserInfo object user data
 * @return
 */
function tranBul($arrList,$objLoginUserInfo){
    // check exists language type and count unstralate record. if not null continue translate .
    if(count($arrList)>0){
        $strContent = '';
        $arrIds = array();

        // Convert the array of untranslated elements to a string so that it can be translated once.
        //After translation, return the same array according to the defined markup. Currently [KB SMARCODE].
        foreach ($arrList as $arrItem){
            //if item untranslated add to translate list
            if(!$arrItem['UNTRANSLATED']){
                if($strContent){
                    $strContent .= ' [KBSMARCODE] '.$arrItem['INCIDENT_NAME_JPN'];
                }else{
                    $strContent .= $arrItem['INCIDENT_NAME_JPN'];
                }
                $arrIds[] = $arrItem['BULLETIN_BOARD_NO'];
            }
        }
        // If there are not null, count the number of records converted and continue the conversion process.
        if(count($arrIds)){
            $strTranContent = tranAmazon($strContent);
            
            if(is_array($strTranContent) && $strTranContent['error'] == 1){
            }else{
                $arrTranContent = explode(' [KBSMARCODE] ',$strTranContent);
            }
            
            foreach ($arrIds as $intKey => $intId){
                $updated = date('Y-m-d H:i:s');
                $strSQL = '';
                $strSQL .= ' UPDATE t_bulletin_board SET ';
                $strSQL .= ' INCIDENT_NAME_ENG = :INCIDENT_NAME_ENG, ';
                $strSQL .= ' UP_USER_NO = :UP_USER_NO, ';
                $strSQL .= ' UP_DATE = :UP_DATE, ';
                $strSQL .= ' UNTRANSLATED = 1 ';
                $strSQL .= ' WHERE BULLETIN_BOARD_NO = :BULLETIN_BOARD_NO; ';
                $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
                $objStmt->bindParam(':INCIDENT_NAME_ENG', $arrTranContent[$intKey]);
                $objStmt->bindParam(':UP_USER_NO', $objLoginUserInfo->intUserNo);
                $objStmt->bindParam(':UP_DATE', $updated);
                $objStmt->bindParam(':BULLETIN_BOARD_NO',$intId);
                $strLogSql = DISPLAY_NAME_PORTAL.$strSQL;
                $strLogSql = str_replace(':CONTENTS_ENG',$arrTranContent[$intKey],$strLogSql);
                $strLogSql = str_replace(':UP_USER_NO',$objLoginUserInfo->intUserNo,$strLogSql);
                $strLogSql = str_replace(':UP_DATE',$updated,$strLogSql);
                $strLogSql = str_replace(':BULLETIN_BOARD_NO',$intId,$strLogSql);
                fncWriteLog(LogLevel['Info'] , LogPattern['Sql'],$strLogSql);
                try {
                    $objStmt->execute();
                }catch (Exception $e) {
                    // other mysql exception (not duplicate key entry)
                    fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                        DISPLAY_NAME_PORTAL.' '.$e->getMessage());
                }
            }
        }
    }
}

/**
 * t_bulletin_board get list bulltein
 *
 * @create 2020/02/17 KBS Tam.nv
 * @update
 * @param
 * @return array bulletin
 */
function get_query_bulletin($strMapIds = NULL){
    $strSQL = '';
    $strSQL .= ' SELECT *';
    $strSQL .= ' FROM t_bulletin_board ';
    $strSQL .= ' INNER JOIN m_place_name ON';
    $strSQL .= ' t_bulletin_board.place3_id = m_place_name.place3_id';

    $strSQL .= ' LEFT JOIN m_business_name on';
    $strSQL .= ' m_business_name.BUSINESS_NAME_JPN = t_bulletin_board.BUSINESS_NAME';
    $strSQL .= ' WHERE COMP_DATE IS NULL ';
    // if have Map id will add to sql
    if($strMapIds){
        $strSQL .= ' AND m_place_name.map_id IN (' . implode(",", array_map('intval',$strMapIds)). ')';
    }
    $strSQL .= ' ORDER BY OCCURRENCE_DATE DESC';
    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);

    $strLogSql = DISPLAY_NAME_PORTAL.$strSQL;

    fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);
    try {
        $objStmt->execute();
        $arrResult = $objStmt->fetchAll(PDO::FETCH_ASSOC);
    }catch (Exception $e) {
        // other mysql exception (not duplicate key entry)
        fncWriteLog(LogLevel['Error'], LogPattern['Error'],
            DISPLAY_NAME_PORTAL.' '.$e->getMessage());
    }

    return @$arrResult;
}

/**
 * t_request get request data
 *
 * @create 2020/02/17 KBS Tam.nv
 * @update
 * @param
 * @return array request
 */
function get_request(){
    $strSQL = '';
    $strSQL .= ' SELECT t_request.*,';
    $strSQL .= ' m_company.ABBREVIATIONS_ENG,m_company.ABBREVIATIONS_JPN';
    $strSQL .= ' FROM t_request ';
    $strSQL .= ' INNER JOIN t_incident_case ON';
    $strSQL .= ' t_request.INCIDENT_CASE_NO = t_incident_case.INCIDENT_CASE_NO';
    $strSQL .= ' INNER JOIN m_user';
    $strSQL .= ' ON t_request.reg_user_no = m_user.user_no';
    $strSQL .= ' INNER JOIN m_company';
    $strSQL .= ' ON m_user.company_no = m_company.company_no';
    $strSQL .= ' WHERE t_incident_case.COMP_DATE IS NULL ';
    $strSQL .= ' ORDER BY t_request.REG_DATE DESC ';
    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
    $strLogSql = DISPLAY_NAME_PORTAL.$strSQL;
    fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);
    try {
        $objStmt->execute();
        $arrResult = $objStmt->fetchAll(PDO::FETCH_ASSOC);
    }catch (Exception $e) {
        // other mysql exception (not duplicate key entry)
        fncWriteLog(LogLevel['Error'], LogPattern['Error'],
            DISPLAY_NAME_PORTAL.' '.$e->getMessage());
    }
    return @$arrResult;
}

/**
 * t_bulletin_board get array list bulletin untranslated
 *
 * @create 2020/02/17 KBS Tam.nv
 * @update
 * @param
 * @return array array bulletin untranslated
 */
function bullUntran(){
    $unstra = 0;
    $strSQL = '';
    $strSQL .= ' SELECT *';
    $strSQL .= ' FROM t_bulletin_board ';
    $strSQL .= ' WHERE UNTRANSLATED = :UNTRANSLATED ';
    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
    $objStmt->bindParam(':UNTRANSLATED', $unstra);
    $strLogSql = DISPLAY_NAME_PORTAL.$strSQL;
    $strLogSql = str_replace(':UNTRANSLATED', 0,$strLogSql);
    fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);
    try {
        $objStmt->execute();
        $arrResult = $objStmt->fetchAll(PDO::FETCH_ASSOC);
    }catch (Exception $e) {
        // other mysql exception (not duplicate key entry)
        fncWriteLog(LogLevel['Error'], LogPattern['Error'],
            DISPLAY_NAME_PORTAL.' '.$e->getMessage());
    }
    return @$arrResult;
}

/**
 * 未翻訳のお知らせを取得
 *
 * @create 2020/05/08 KBS T.Masuda
 * @update
 * @param
 * @return $arrResult array 未翻訳のお知らせデータ
 */
function annUntran(){
    $unstra = 0;
    $strSQL = '';
    $strSQL .= ' SELECT *';
    $strSQL .= ' FROM t_announce ';
    $strSQL .= ' WHERE UNTRANSLATED = :UNTRANSLATED ';
    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
    $objStmt->bindParam(':UNTRANSLATED', $unstra);
    $strLogSql = DISPLAY_NAME_PORTAL.$strSQL;
    $strLogSql = str_replace(':UNTRANSLATED', 0,$strLogSql);
    fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);
    try {
        $objStmt->execute();
        $arrResult = $objStmt->fetchAll(PDO::FETCH_ASSOC);
    }catch (Exception $e) {
        // other mysql exception (not duplicate key entry)
        fncWriteLog(LogLevel['Error'], LogPattern['Error'],
            DISPLAY_NAME_PORTAL.' '.$e->getMessage());
    }
    return @$arrResult;
}

// Incident boad function

/**
 * m_link_category get all link category
 *
 * @create 2020/03/04 KBS Tam.nv
 * @update
 * @param
 * @return array link category.
 */
function get_link_category(){
    $strSQL = '';
    $strSQL .= ' SELECT *';
    $strSQL .= ' FROM m_link_category ';
    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
    $strLogSql = DISPLAY_NAME_PORTAL.$strSQL;
    fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);
    try {
        $objStmt->execute();
        $arrResult = $objStmt->fetchAll(PDO::FETCH_ASSOC);
    }catch (Exception $e) {
        // other mysql exception (not duplicate key entry)
        fncWriteLog(LogLevel['Error'], LogPattern['Error'],
            DISPLAY_NAME_PORTAL.' '.$e->getMessage());
    }
    return @$arrResult;
}

/**
 * すべての事件を取得
 *
 * @create 2020/03/04 KBS Tam.nv
 * @update
 * @param
 * @return array すべてのインシデントケースを配列する
 */
function get_incident_case(){
    $strSQL = '';
    $strSQL .= ' SELECT *';
    $strSQL .= ' FROM t_incident_case ';
    $strSQL .= ' WHERE COMP_DATE IS NULL ';
    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
    $strLogSql = DISPLAY_NAME_PORTAL.$strSQL;
    fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);
    try {
        $objStmt->execute();
        $arrResult = $objStmt->fetchAll(PDO::FETCH_ASSOC);
    }catch (Exception $e) {
        // other mysql exception (not duplicate key entry)
        fncWriteLog(LogLevel['Error'], LogPattern['Error'],
            DISPLAY_NAME_PORTAL.' '.$e->getMessage());
        $_SESSION['SQL_GET_DATA_ERROR'] = 'PUBLIC_MSG_001';
    }
    return @$arrResult;
}

/**
 * m_inst_category get all inst category
 *
 * @create 2020/03/04 KBS Tam.nv
 * @update
 * @param
 * @return array inst category
 */
function get_inst_cat(){
    $strSQL = '';
    $strSQL .= ' SELECT *';
    $strSQL .= ' FROM m_inst_category ';
    $strSQL .= ' ORDER BY SORT_NO ASC ';
    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
    $strLogSql = DISPLAY_NAME_PORTAL.$strSQL;
    fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);
    try {
        $objStmt->execute();
        $arrResult = $objStmt->fetchAll(PDO::FETCH_ASSOC);
    }catch (Exception $e) {
        // other mysql exception (not duplicate key entry)
        fncWriteLog(LogLevel['Error'], LogPattern['Error'],
            DISPLAY_NAME_PORTAL.' '.$e->getMessage());
        $_SESSION['SQL_GET_DATA_ERROR'] = 'PUBLIC_MSG_001';
    }
    return @$arrResult;
}

/**
 * m_info_category get all info category
 *
 * @create 2020/03/04 KBS Tam.nv
 * @update
 * @param
 * @return array info category
 */
function get_info_cat(){
    $strSQL = '';
    $strSQL .= ' SELECT *';
    $strSQL .= ' FROM m_info_category ';
    $strSQL .= ' ORDER BY SORT_NO ASC ';
    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
    $strLogSql = DISPLAY_NAME_PORTAL.$strSQL;
    fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);
    try {
        $objStmt->execute();
        $arrResult = $objStmt->fetchAll(PDO::FETCH_ASSOC);
    }catch (Exception $e) {
        // other mysql exception (not duplicate key entry)
        fncWriteLog(LogLevel['Error'], LogPattern['Error'],
            DISPLAY_NAME_PORTAL.' '.$e->getMessage());
        $_SESSION['SQL_GET_DATA_ERROR'] = 'PUBLIC_MSG_001';
    }
    return @$arrResult;
}

/**
 * t_inst_company_sort get inst company sort by user bo, inst category no, company no
 *
 * @create 2020/03/04 KBS Tam.nv
 * @update
 * @param
 * @return array inst company sort
 */

function fncInserInsComSort($intUserId,$intInstCateNo,$intComNo){
    $strViewLog = DISPLAY_NAME_PORTAL.' 登録 (ユーザID = ' .$intUserId . ') ';
    fncWriteLog(LogLevel['Info'], LogPattern['Button'], $strViewLog);
    $strSQL = '';
    $strSQL .= ' INSERT INTO t_inst_company_sort ';
    $strSQL .= ' SELECT';
    $strSQL .= ' :USER_NO,';
    $strSQL .= ' :INST_CATEGORY_NO,';
    $strSQL .= ' :COMPANY_NO,';
    $strSQL .= ' (SELECT ISNULL(MAX( SORT_NO ), 0)+ 1  FROM t_inst_company_sort WHERE USER_NO =:USER_NO3 AND INST_CATEGORY_NO =:INST_CATEGORY_NO3)';
    $strSQL .= ' WHERE NOT EXISTS (
    SELECT SORT_NO FROM t_inst_company_sort
    WHERE USER_NO =:USER_NO2 AND
    INST_CATEGORY_NO =:INST_CATEGORY_NO2 AND
    COMPANY_NO =:COMPANY_NO2
    )';
    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
    $objStmt->bindParam(':USER_NO', $intUserId);
    $objStmt->bindParam(':USER_NO2', $intUserId);
    $objStmt->bindParam(':USER_NO3', $intUserId);
    $objStmt->bindParam(':INST_CATEGORY_NO', $intInstCateNo);
    $objStmt->bindParam(':INST_CATEGORY_NO2', $intInstCateNo);
    $objStmt->bindParam(':INST_CATEGORY_NO3', $intInstCateNo);
    $objStmt->bindParam(':COMPANY_NO', $intComNo);
    $objStmt->bindParam(':COMPANY_NO2', $intComNo);

    $strLogSql = DISPLAY_NAME_PORTAL.$strSQL;
    $strLogSql = str_replace(':USER_NO', $intUserId, $strLogSql);
    $strLogSql = str_replace(':USER_NO2', $intUserId, $strLogSql);
    $strLogSql = str_replace(':INST_CATEGORY_NO', $intInstCateNo, $strLogSql);
    $strLogSql = str_replace(':INST_CATEGORY_NO2', $intInstCateNo, $strLogSql);
    $strLogSql = str_replace(':COMPANY_NO', $intComNo, $strLogSql);
    $strLogSql = str_replace(':COMPANY_NO2', $intComNo, $strLogSql);
    fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);
    try {
        return $objStmt->execute();
    }catch (Exception $e) {
        fncWriteLog(LogLevel['Error'], LogPattern['Error'],
            DISPLAY_NAME_PORTAL.' '.$e->getMessage());
        $_SESSION['SQL_GET_DATA_ERROR'] = 'PUBLIC_MSG_001';
    }
}

/**
 * t_inst_company_sort remove record inst company sort by inst category no and company no
 *
 * @create 2020/03/04 KBS Tam.nv
 * @update
 * @param
 * @return boolean true or NULL
 */

function fncDeleteInsComSort($intUserId,$intInstCateNo,$intComNo){
    $strViewLog = DISPLAY_NAME_PORTAL.' 削除 (ユーザID = ' .$intUserId . ') ';
    fncWriteLog(LogLevel['Info'], LogPattern['Button'], $strViewLog);

    $strSQL = '';
    $strSQL .= ' DELETE FROM t_inst_company_sort ';
    $strSQL .= ' WHERE USER_NO =:USER_NO AND
    INST_CATEGORY_NO =:INST_CATEGORY_NO AND
    COMPANY_NO =:COMPANY_NO';
    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
    $objStmt->bindParam(':USER_NO', $intUserId);
    $objStmt->bindParam(':INST_CATEGORY_NO', $intInstCateNo);
    $objStmt->bindParam(':COMPANY_NO', $intComNo);
    $strLogSql = DISPLAY_NAME_PORTAL.$strSQL;
    $strLogSql = str_replace(':USER_NO', $intUserId, $strLogSql);
    $strLogSql = str_replace(':INST_CATEGORY_NO', $intInstCateNo, $strLogSql);
    $strLogSql = str_replace(':COMPANY_NO', $intComNo, $strLogSql);
    fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);
    try {
        return $objStmt->execute();
    }catch (Exception $e) {
        fncWriteLog(LogLevel['Error'], LogPattern['Error'],
            DISPLAY_NAME_PORTAL.' '.$e->getMessage());
        $_SESSION['SQL_GET_DATA_ERROR'] = 'PUBLIC_MSG_001';
    }
}

/**
 * t_information get array infomation by user data
 *
 * @create 2020/03/04 KBS Tam.nv
 * @update
 * @param string $strOuter flag check in get data pink pin  or white pin.
 * @param $objLoginUserInfo object user login
 * @param $strCkWioutCom flag check get query infomation without other company
 * @param null $intFlagInfoCate flag check to get data by infomation
 * @return array data infomation
 */
function get_t_information(
    $strOuter = '',
    $objLoginUserInfo = NULL,
    $strCkWioutCom = NULL,
    $intFlagInfoCate = NULL
){
    $blnOuterFlag = false;
    // check get data for infomation, if not exists get data by infomation category
    if($intFlagInfoCate){
        $strOuter = ' ORDER BY UP_DATE DESC';
    }else{
        // check flag, if not exist get all data exist in t_inst_company_sort, this is data to show for white pink
        //if flag not null, get all data not exit in t_inst_company_sort, this is data show for pink pin
        if($strOuter){
            $strOuter = ' WHERE NOT EXISTS (SELECT t_inst_company_sort.COMPANY_NO';
            $strOuter .= ' FROM t_inst_company_sort';
            $strOuter .= ' WHERE t_information.COMPANY_NO = t_inst_company_sort.COMPANY_NO';
            $strOuter .= ' AND t_inst_company_sort.user_no = :USER_NO';
            $strOuter .= ' )';
            $strOuter .= ' ORDER BY  m_company.SORT_NO,UP_DATE DESC';
        }else{
            $blnOuterFlag = true;
            $strOuter = ' INNER JOIN t_inst_company_sort ON m_company.COMPANY_NO = t_inst_company_sort.COMPANY_NO　';
            $strOuter .= ' AND t_inst_company_sort.USER_NO = :USER_ID';
            $strOuter .= ' WHERE EXISTS (SELECT t_inst_company_sort.COMPANY_NO';
            $strOuter .= ' FROM t_inst_company_sort WHERE ';
            $strOuter .= ' t_information.COMPANY_NO = t_inst_company_sort.COMPANY_NO';
            $strOuter .= '  AND t_inst_company_sort.USER_NO = :USER_NO)';
            $strOuter .= ' ORDER BY t_inst_company_sort.SORT_NO DESC,UP_DATE DESC';
        }
    }
    // if flag = true get only data of company of user login
    if($strCkWioutCom == 'true'){
        $strWioutComStr = ' AND t_information.COMPANY_NO = :COMPANY_NO ';
    }

    // check get data for infomation, if not exists get data by infomation category
    if($intFlagInfoCate){
        $strSelectCateSql = ' INNER JOIN m_info_category ON';
        $strSelectCateSql .= ' t_information.info_category_no = m_info_category.info_category_no';
    }
    if($intFlagInfoCate){
        $strSelectCate = ' , m_info_category.info_category_no as INFO_CATEGORY_NO';
    }

    $strSQL = '';
    $strSQL .= ' SELECT';
    $strSQL .= ' t_information.INFORMATION_NO,';
    $strSQL .= ' t_information.INCIDENT_CASE_NO,';
    $strSQL .= ' ISNULL(t_information.UP_DATE,t_information.REG_DATE) AS UP_DATE,';
    $strSQL .= ' t_information.TITLE_JPN,';
    $strSQL .= ' t_information.REG_DATE,';
    $strSQL .= ' t_information.TITLE_ENG,m_company.INST_CATEGORY_NO,';
    $strSQL .= ' m_company.ABBREVIATIONS_JPN,m_company.ABBREVIATIONS_ENG,';
    $strSQL .= ' m_company.COMPANY_NO ';
    $strSQL .= @$strSelectCate;
    $strSQL .= ' FROM t_information ';
    $strSQL .= ' INNER JOIN t_incident_case ON ';
    $strSQL .= ' t_incident_case.INCIDENT_CASE_NO = t_information.INCIDENT_CASE_NO';
    $strSQL .= ' AND t_incident_case.COMP_DATE IS NULL';
    $strSQL .= @$strWioutComStr;
    $strSQL .= @$strSelectCateSql;
    $strSQL .= ' INNER JOIN m_company';
    $strSQL .= ' ON m_company.COMPANY_NO = t_information.COMPANY_NO';
    $strSQL .= ' AND m_company.DEL_FLAG = :DEL_FLAG';
    $strSQL .= $strOuter;
    //$strSQL .= ' ORDER BY  UP_DATE DESC';
    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
    // check get data for information, bind parameter follow by sql
    if(!$intFlagInfoCate){
        //check if exists user information will bind parameter by user no
        if($objLoginUserInfo){
            $objStmt->bindParam(':USER_NO',$objLoginUserInfo->intUserNo);
            //check if exists user information will bind parameter by user no
            if($blnOuterFlag){
                $objStmt->bindParam(':USER_ID',$objLoginUserInfo->intUserNo);
            }
        }
    }

    $intDelFlag = 0;
    $objStmt->bindParam(':DEL_FLAG',$intDelFlag);
    // bind parameter when exists flag check without other company
    if($strCkWioutCom == 'true'){
        $objStmt->bindParam(':COMPANY_NO',$objLoginUserInfo->intCompanyNo);
    }

    $strLogSql = DISPLAY_NAME_PORTAL.$strSQL;
    // bind parameter follow by sql
    if($objLoginUserInfo && !$intFlagInfoCate){
        $strLogSql = str_replace(':USER_NO', $objLoginUserInfo->intUserNo,$strLogSql);
        //check if exists user information will bind parameter by user no
        if($blnOuterFlag){
            $strLogSql = str_replace(':USER_ID', $objLoginUserInfo->intUserNo,$strLogSql);
        }
    }
    //replace parram bind data
    if($strCkWioutCom == 'true'){
        $strLogSql = str_replace(
            ':COMPANY_NO',
            $objLoginUserInfo->intCompanyNo,
            $strLogSql
        );
    }

    $strLogSql = str_replace(':DEL_FLAG', $intDelFlag,$strLogSql);

    fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);
    try {
        $objStmt->execute();
        $arrResult = $objStmt->fetchAll(PDO::FETCH_ASSOC);
    }catch (Exception $e) {

        // other mysql exception (not duplicate key entry)
        fncWriteLog(LogLevel['Error'], LogPattern['Error'],
            DISPLAY_NAME_PORTAL.' '.$e->getMessage());
        $_SESSION['SQL_GET_DATA_ERROR'] = 'PUBLIC_MSG_001';
    }
    return @$arrResult;
}

/**
 * validate input data for query screen
 *
 * @create 2020/03/04 KBS Tam.nv
 * @update
 * @param $arrData array input data
 * @param $arrText array input text transalte
 * @param $objLoginUserInfo object user login
 * @return array/text if have error return array, if not have error return string translated
 */
function validateInputText($arrData,$arrText,$objLoginUserInfo, $intType){
    $arrError = array();
    $intMaxLengSource = 200;
    $intMaxLengTarget = 1000;
    $intTypeTran = 0;
    //if data translate from english to Japanese set type translate = 1 setup max leng for source and target.
    if($arrData['sleTran']=='ej'){
        $intMaxLengSource = 1000;
        $intMaxLengTarget = 200;
        $intTypeTran = 1;
        if(!mb_detect_encoding($arrData['txtSource'], 'ASCII', true)) {
            $arrError['er-character'] =  $arrText['PUBLIC_MSG_028'];
        }
    }

    //check input source not null
    if(!$arrData['txtSource']){
        //if txtSource is null add to array error and show in div error source at view.
        $arrError['er-source'] = $arrText['PUBLIC_MSG_026'];
    }else{
        //if not null continue check type translate.
        //check type of translate and set message error
        if($intTypeTran){
            $errSouce = $arrText['PUBLIC_MSG_046'];
        }else{
            $errSouce = $arrText['PUBLIC_MSG_045'];
        }
        // check txtSouce max leng
        if(mb_strlen($arrData['txtSource'])>$intMaxLengSource){
            $arrError['er-source'] = $errSouce;
        }
    }
    // if source input is english check input charset code
    if($intTypeTran == 0 && $arrData['txtTarget']){
        //▼2020/05/29 KBS S.Tasaki 半角英数チェックから全角ダブルクォーテーションを除外
        $strCheckTarget = $arrData['txtTarget'];
        $strCheckTarget = str_replace('“', '', $strCheckTarget);
        $strCheckTarget = str_replace('”', '', $strCheckTarget);
        if(!mb_detect_encoding($strCheckTarget, 'ASCII', true)) {
            $arrError['er-character'] =  $arrText['PUBLIC_MSG_038'];
        }
        //▲2020/05/29 KBS S.Tasaki
    }
    // check target input, if it not null continue chekc type of translate
    if(!$arrData['txtTarget'] && $arrData['ckeckMan'] =='true'){
        $arrError['er-target'] = $arrText['PUBLIC_MSG_036'];
    }else{
        //check type of translate and set message error
        if($intTypeTran){
            $errMaxTarget = $arrText['PUBLIC_MSG_047'];
        }else{
            $errMaxTarget = $arrText['PUBLIC_MSG_048'];
        }
        //▼2020/05/27 KBS T.Masuda 翻訳欄の文字数チェックを行わない
        // check max length of target
        //if(mb_strlen($arrData['txtTarget'])>$intMaxLengTarget){
        //    $arrError['er-target'] = $errMaxTarget;
        //}
        //▲2020/05/27 KBS T.Masuda
    }
    // if don't have error get service translate from amazon AWS. if have error return array error.
    if(empty($arrError)){
        $strTran = tranAmazon($arrData['txtSource'],$intTypeTran);
        // if translate error return array data, if success return text translate result
        if(is_array($strTran)){
            //translate error
            $arrError['message'] = $arrText['PUBLIC_MSG_010'];
            $arrResult['error'] = $arrError;
            //▼2020/06/04 KBS S.Tasaki 投稿時の自動翻訳の場合、空文字にて登録を実施する。
            if($intType == 1){
                return '';
            }else{
                return $arrResult;
            }
            //▲2020/06/04 KBS S.Tasaki
            
        }else{
            //translate success
            return $strTran;
        }
    }else{
        $arrResult['error'] = $arrError;
        return $arrResult;
    }
}

