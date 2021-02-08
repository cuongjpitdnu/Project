<?php
/*
* @bulletin_board_mail.php
*
* @create 2020/04/06 KBS Tam.nv
* @update
*/

header('Content-type: text/html; charset=utf-8');
header('X-FRAME-OPTIONS: DENY');

$arrData = $_POST;
$_POST = array();


require_once('common/common.php');
const DISPLAY_NAME = 'アラート表示システム連携画面 ';
$arrResult = array();

//DB connection
if(fncConnectDB() == false){
    fncWriteLog(LogLevel['Error'], LogPattern['Error'], 'DB接続に失敗しました。');
    exit;
}
// check status, if status different 0,1 write error log
if(!isset($arrData['status']) || ($arrData['status'] !== '0' && $arrData['status'] !== '1' )){
    $strLog = DISPLAY_NAME.BULLETIN_BOARD_MAIL_MSG_001;
    fncWriteLog(LogLevel['Error'], LogPattern['Error'], $strLog);
}else{
    //Log start process
    fncWriteLog(LogLevel['Info'], LogPattern['View'],DISPLAY_NAME." 処理開始 ");    
    $intStatus = $arrData['status'];
    $arrError = validateInputBulMail($arrData);
    // check array error
    if($arrError){
        //Log validation error
        $strLog = DISPLAY_NAME.implode(' ',$arrError);
        fncWriteLog(LogLevel['Error'], LogPattern['Error'], $strLog);
    }else{
        $GLOBALS['g_dbaccess']->funcBeginTransaction();
        // check status number if type = 1 only update bulletin boad, type = 0 check update or insert.
        if($intStatus){
            fncUpdateBull($arrData);
        }else{
            
            //▼2020/05/27 KBS T.Masuda
            $strUpCheckSql = 'SELECT * FROM t_bulletin_board ';
            $strUpCheckSql.= 'INNER JOIN m_place_name ON m_place_name.place3_id =:place3_id ';
            $strUpCheckSql.= 'WHERE INCIDENT_NO = :INCIDENT_NO';
            
            $objCheck = $GLOBALS['g_dbaccess']->funcPrepare($strUpCheckSql);
            $objCheck->bindParam(':place3_id', $arrData['place3_id']);
            $objCheck->bindParam(':INCIDENT_NO', $arrData['incident_no']);

            $strLogSql = DISPLAY_NAME. $strUpCheckSql;
            $strLogSql = str_replace(':INCIDENT_NO', $arrData['incident_no'], $strLogSql);
            fncWriteLog(LogLevel['Info'], LogPattern['Sql'], $strLogSql);
            
            $objCheck->execute();
            $objCheckBull = $objCheck->fetchAll(PDO::FETCH_ASSOC);
            
            if($objCheckBull[0]['INCIDENT_NAME_JPN'] == $arrData['incident_name']
               && $objCheckBull[0]['BUSINESS_NAME'] == $arrData['business_name']
               && $objCheckBull[0]['PLACE3_ID'] == $arrData['place3_id']
               && $objCheckBull[0]['PLACE_NAME_JPN'] == $arrData['place_name']
               && $objCheckBull[0]['PLACE_NAME_ENG'] == $arrData['place_name_eng']
               && $objCheckBull[0]['MAP_ID'] == $arrData['map_id']
               && date('Y-m-d H:i:s',strtotime($objCheckBull[0]['OCCURRENCE_DATE'])) 
                   == date('Y-m-d H:i:s',strtotime($arrData['occurrence_date']))){
                
                fncWriteLog(LogLevel['Info'], LogPattern['View'],DISPLAY_NAME.
                            " データに変更がないため更新処理を行いませんでした。 ");
                $GLOBALS['g_dbaccess']->funcCommit();
                exit();
                
            }
            //▲2020/05/27 KBS T.Masuda
            
            
            $strInsertBull = fncInsertBull($arrData);
            //If insert bull success continue insert place
            if($strInsertBull){
                $strInsertPlace = fncInsertPlace($arrData);
            }
            //if insert place success get user and send email
            if(@$strInsertPlace){
                $arrUser = fncGetUserBoadMail();
                // if have user will send mail.
                if($arrUser){
                    fncSendmailToBull($arrUser);
                    fncWriteLog(LogLevel['Info'], LogPattern['Button'],DISPLAY_NAME.$strInsertBull);
                }
                $GLOBALS['g_dbaccess']->funcCommit();
            }

        }
    }

}

/**
 * validate input data
 *
 * @create 2020/04/06 KBS Tam.nv
 * @update
 * @param $arrData Input data array
 * @return array errors array
 */

function validateInputBulMail($arrData){
    $arrError = array();
    $intNo = @$arrData['incident_no'];
    // check input is numeric
    if(!$intNo){
        $arrError[] = BULLETIN_BOARD_MAIL_MSG_002;
    }
    //check exists status
    if(!$arrData['status']){
        $strTilte = @$arrData['incident_name'];
        $strBuName = @$arrData['business_name'];
        $dtmOccDate = @$arrData['occurrence_date'];
        $strPlaceNameJpn = @$arrData['place_name'];
        $strPlaceNameEng = @$arrData['place_name_eng'];
        $intMapId = @$arrData['map_id'];
        $intPlaceId = @$arrData['place3_id'];

        // check input null
        if(!$strTilte){
            $arrError[] = BULLETIN_BOARD_MAIL_MSG_003;
        }
        // check input null
        if(!$strBuName){
            $arrError[] = BULLETIN_BOARD_MAIL_MSG_004;
        }
        // check input null and is date
        if(!validateDate($dtmOccDate)){
            $arrError[] = BULLETIN_BOARD_MAIL_MSG_005;
        }
        // check input null
        if(!$strPlaceNameJpn){
            $arrError[] = BULLETIN_BOARD_MAIL_MSG_006;
        }
        // check input null and is date
        
        //▼2020/05/27 KBS T.Mausda 場所名（英語）空白対応
        //if(!$strPlaceNameEng){
            //$arrError[] = BULLETIN_BOARD_MAIL_MSG_007;
        //}
        //▲2020/05/27 KBS T.Masuda
        
        // check input is numeric
        if(!is_numeric($intMapId)){
            $arrError[] = BULLETIN_BOARD_MAIL_MSG_008;
        }
        // check input is numeric
        if(!is_numeric($intPlaceId)){
            $arrError[] = BULLETIN_BOARD_MAIL_MSG_009;
        }
    }else{
        $dtmComDate = @$arrData['comp_date'];
        // check input is date
        if(!validateDate($dtmComDate)){
            $arrError[] = BULLETIN_BOARD_MAIL_MSG_010;
        }
    }

    return $arrError;
}


/**
 * validate data is date type
 *
 * @create 2020/04/06 KBS Tam.nv
 * @update
 * @param $dtmInputDate input
 * @param string $strFormat format
 * @return boolean true / false
 */
function validateDate($dtmInputDate, $strFormat = 'Y/m/d H:i:s')
{
    $dtmDate = DateTime::createFromFormat($strFormat, $dtmInputDate);
    // The Y ( 4 digits year ) returns TRUE for any integer with any number of digits so changing the comparison from == to === fixes the issue.
    return $dtmDate && $dtmDate->format($strFormat) === $dtmInputDate;
}

/**
 * t_bulletin_board Create a new bulletin record
 *
 * @create 2020/04/06 KBS Tam.nv
 * @update
 * @param $arrData input data
 * @return
 */
function fncInsertBull($arrData)
{
    $intFlagError = 0;
    // check exists data
    $strSqlCheckExist = 'SELECT * FROM t_bulletin_board WHERE INCIDENT_NO = :INCIDENT_NO';
    $objStmtCheckExit = $GLOBALS['g_dbaccess']->funcPrepare($strSqlCheckExist);
    $objStmtCheckExit->bindParam(':INCIDENT_NO', $arrData['incident_no']);

    $strLogSql = DISPLAY_NAME. $strSqlCheckExist;
    $strLogSql = str_replace(':INCIDENT_NO', $arrData['incident_no'], $strLogSql);
    fncWriteLog(LogLevel['Info'], LogPattern['Sql'], $strLogSql);
    try {
        $objStmtCheckExit->execute();
        $objBulletin = $objStmtCheckExit->fetchAll(PDO::FETCH_ASSOC);
    }catch (Exception $e) {
        $GLOBALS['g_dbaccess']->funcRollback();
        fncWriteLog(LogLevel['Error'], LogPattern['Error'], DISPLAY_NAME.' '.$e->getMessage());
        $intFlagError = 1;
    }
    // if exists  data,process update else insert
    if(@$objBulletin){
        $strError = BULLETIN_BOARD_MAIL_MSG_014;
        $strAction = ' 更新しました';        
    }else{
        //insert data
        $strError = BULLETIN_BOARD_MAIL_MSG_013;
        $strAction = ' 登録しました';
        // get bulletin boad no
        $strSqlGetBullNo = 'SELECT NEXT VALUE FOR bulletin_board_sequence';
        $objStmtBullNo = $GLOBALS['g_dbaccess']->funcPrepare($strSqlGetBullNo);
        $strLogSql = DISPLAY_NAME. $strSqlGetBullNo;
        // check flag error, flag is false wil write log, if is true will process execute database
        if(!$intFlagError){
            fncWriteLog(LogLevel['Info'], LogPattern['Sql'], $strLogSql);
            try {
                $objStmtBullNo->execute();
                $arrNo = $objStmtBullNo->fetchAll(PDO::FETCH_ASSOC);
            }catch (Exception $e) {
                $GLOBALS['g_dbaccess']->funcRollback();
                fncWriteLog(LogLevel['Error'], LogPattern['Error'], DISPLAY_NAME.' '.$strError);
                $intFlagError = 1;
            }
        }
    }

    $strSQL = 'IF NOT EXISTS (SELECT * FROM t_bulletin_board WHERE INCIDENT_NO = :INCIDENT_NO)';
    $strSQL .= ' INSERT INTO t_bulletin_board (';
    $strSQL .= ' BULLETIN_BOARD_NO,';
    $strSQL .= ' INCIDENT_NO,';
    $strSQL .= ' OCCURRENCE_DATE,';
    $strSQL .= ' BUSINESS_NAME,';
    $strSQL .= ' PLACE3_ID,';
    $strSQL .= ' INCIDENT_NAME_JPN,';
    $strSQL .= ' INCIDENT_NAME_ENG,';
    $strSQL .= ' CORRECTION_FLAG,';
    $strSQL .= ' UNTRANSLATED,';
    $strSQL .= ' COMP_DATE,';
    $strSQL .= ' REG_DATE,';
    $strSQL .= ' UP_USER_NO,';
    $strSQL .= ' UP_DATE )';

    $strSQL .= ' VALUES(';
    $strSQL .= ' :BULLETIN_BOARD_NO,';
    $strSQL .= ' :INCIDENT_NO2,';
    $strSQL .= ' :OCCURRENCE_DATE,';
    $strSQL .= ' :BUSINESS_NAME,';
    $strSQL .= ' :PLACE3_ID,';
    $strSQL .= ' :INCIDENT_NAME_JPN,';
    $strSQL .= ' NULL,';
    $strSQL .= ' 0,';
    $strSQL .= ' 0,';
    $strSQL .= ' NULL,';
    $strSQL .= ' :REG_DATE, ';
    $strSQL .= ' NULL,';
    $strSQL .= ' NULL';
    $strSQL .= ' )';
    $strSQL .= ' ELSE';
    $strSQL .= ' UPDATE t_bulletin_board SET';
    $strSQL .= ' OCCURRENCE_DATE = :OCCURRENCE_DATE2,';
    $strSQL .= ' BUSINESS_NAME = :BUSINESS_NAME2,';
    $strSQL .= ' PLACE3_ID = :PLACE3_ID2,';
    $strSQL .= ' INCIDENT_NAME_JPN = :INCIDENT_NAME_JPN2,';

    $strSQL .= ' INCIDENT_NAME_ENG = ( CASE ';
    $strSQL .= ' WHEN INCIDENT_NAME_JPN <> :INCIDENT_NAME_PARAM THEN :NULL_DATA ELSE INCIDENT_NAME_ENG';
    $strSQL .= ' END), ';

    $strSQL .= ' CORRECTION_FLAG = ( CASE ';
    $strSQL .= ' WHEN INCIDENT_NAME_JPN <> :INCIDENT_NAME_PARAM2 THEN 0 ELSE CORRECTION_FLAG';
    $strSQL .= ' END), ';

    $strSQL .= ' UNTRANSLATED = ( CASE ';
    $strSQL .= ' WHEN INCIDENT_NAME_JPN <> :INCIDENT_NAME_PARAM3 THEN 0 ELSE UNTRANSLATED';
    $strSQL .= ' END), ';

    $strSQL .= ' UP_USER_NO = NULL,';
    $strSQL .= ' UP_DATE = :UP_DATE ';
    $strSQL .= ' WHERE INCIDENT_NO = :INCIDENT_NO3';
    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
    $objStmt->bindParam(':BULLETIN_BOARD_NO', $arrNo[0][""]);
    $objStmt->bindParam(':INCIDENT_NO', $arrData['incident_no']);
    $objStmt->bindParam(':INCIDENT_NO2', $arrData['incident_no']);
    $objStmt->bindParam(':INCIDENT_NO3', $arrData['incident_no']);
    $dtmOcDate = date('Y-m-d H:i:s',strtotime($arrData['occurrence_date']));
    $objStmt->bindParam(':OCCURRENCE_DATE', $dtmOcDate);
    $objStmt->bindParam(':OCCURRENCE_DATE2', $dtmOcDate);
    $objStmt->bindParam(':BUSINESS_NAME', $arrData['business_name']);
    $objStmt->bindParam(':BUSINESS_NAME2', $arrData['business_name']);
    $objStmt->bindParam(':PLACE3_ID', $arrData['place3_id']);
    $objStmt->bindParam(':PLACE3_ID2', $arrData['place3_id']);
    $objStmt->bindParam(':INCIDENT_NAME_PARAM3', $arrData['incident_name']);
    $objStmt->bindParam(':INCIDENT_NAME_PARAM2', $arrData['incident_name']);
    $objStmt->bindParam(':INCIDENT_NAME_PARAM', $arrData['incident_name']);
    $strNull = NULL;
    $objStmt->bindParam(':NULL_DATA', $strNull);
    $objStmt->bindParam(':INCIDENT_NAME_JPN', $arrData['incident_name']);
    $objStmt->bindParam(':INCIDENT_NAME_JPN2', $arrData['incident_name']);
    $dtmRegDate = date('Y-m-d H:i:s');
    $objStmt->bindParam(':REG_DATE', $dtmRegDate);
    $objStmt->bindParam(':UP_DATE', $dtmRegDate);
    $strLogSql = DISPLAY_NAME. $strSQL;
    $strLogSql = str_replace(':BULLETIN_BOARD_NO', $arrData['incident_no'], $strLogSql);
    $strLogSql = str_replace(':INCIDENT_NO2', $arrData['incident_no'], $strLogSql);
    $strLogSql = str_replace(':INCIDENT_NO3', $arrData['incident_no'], $strLogSql);
    $strLogSql = str_replace(':INCIDENT_NO', $arrData['incident_no'], $strLogSql);
    $strLogSql = str_replace(':OCCURRENCE_DATE2', $dtmOcDate, $strLogSql);
    $strLogSql = str_replace(':OCCURRENCE_DATE', $dtmOcDate, $strLogSql);
    $strLogSql = str_replace(':BUSINESS_NAME2', $arrData['business_name'],$strLogSql);
    $strLogSql = str_replace(':BUSINESS_NAME', $arrData['business_name'],$strLogSql);
    $strLogSql = str_replace(':PLACE3_ID2', $arrData['place3_id'], $strLogSql);
    $strLogSql = str_replace(':PLACE3_ID', $arrData['place3_id'], $strLogSql);
    $strLogSql = str_replace(':INCIDENT_NAME_PARAM', $arrData['incident_name'], $strLogSql);
    $strLogSql = str_replace(':NULL_DATA', $strNull, $strLogSql);
    $strLogSql = str_replace(':INCIDENT_NAME_JPN2', $arrData['incident_name'], $strLogSql);
    $strLogSql = str_replace(':INCIDENT_NAME_JPN',$arrData['incident_name'],$strLogSql);
    $strLogSql = str_replace(':REG_DATE', $dtmRegDate, $strLogSql);
    $strLogSql = str_replace(':UP_DATE', $dtmRegDate, $strLogSql);
    // check flag error, flag is false wil write log, if is true will process execute database
    if(!$intFlagError){
        fncWriteLog(LogLevel['Info'], LogPattern['Sql'], $strLogSql);
        try {
            $objStmt->execute();
            return $strAction;
        }catch (Exception $e) {
            $GLOBALS['g_dbaccess']->funcRollback();
            fncWriteLog(LogLevel['Error'], LogPattern['Error'], DISPLAY_NAME.' '.$strError);
        }
    }

}

/**
 * t_bulletin_board update bulletin
 *
 * @create 2020/03/04 KBS Tam.nv
 * @update
 * @param $arrData input data
 * @return void
 */
function fncUpdateBull($arrData)
{
    $strSQL = '';
    $strSQL .= ' UPDATE t_bulletin_board SET ';
    $strSQL .= ' COMP_DATE = :comp_date,';
    $strSQL .= ' UP_USER_NO = NULL,';
    $strSQL .= ' UP_DATE = CURRENT_TIMESTAMP ';
    $strSQL .= ' WHERE INCIDENT_NO = :INCIDENT_NO';
    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
    $objStmt->bindParam(':INCIDENT_NO', $arrData['incident_no']);
    $objStmt->bindParam(':comp_date', $arrData['comp_date']);
    $strLogSql = DISPLAY_NAME. $strSQL;
    $strLogSql = str_replace(':INCIDENT_NO', $arrData['incident_no'], $strLogSql);
    $strLogSql = str_replace(':comp_date', $arrData['comp_date'], $strLogSql);
    fncWriteLog(LogLevel['Info'], LogPattern['Sql'], $strLogSql);
    try {
        $objStmt->execute();
        $count = $objStmt->rowCount();
        // if no data update will roll back and log message.
        if(!$count){
            $GLOBALS['g_dbaccess']->funcRollback();
            fncWriteLog(LogLevel['Error'], LogPattern['Error'],DISPLAY_NAME.BULLETIN_BOARD_MAIL_MSG_016);
        }else{
            $GLOBALS['g_dbaccess']->funcCommit();
            fncWriteLog(LogLevel['Info'], LogPattern['Button'],DISPLAY_NAME." 完了しました");
        }
    }catch (Exception $e) {
        $GLOBALS['g_dbaccess']->funcRollback();
        fncWriteLog(LogLevel['Error'],
            LogPattern['Error'], DISPLAY_NAME.' '.BULLETIN_BOARD_MAIL_MSG_012);
    }
}

/**
 * m_place_name Insert or Update place
 *
 * @create 2020/04/07 KBS Tam.nv
 * @update
 * @param $arrData input data
 * @return
 */
function fncInsertPlace($arrData)
{
    $strSqPlace = 'SELECT place3_id FROM m_place_name WHERE place3_id =:place3_id';
    $objPlace = $GLOBALS['g_dbaccess']->funcPrepare($strSqPlace);
    $objPlace->bindParam(':place3_id', $arrData['place3_id']);
    $strLogSql = DISPLAY_NAME. $strSqPlace;
    fncWriteLog(LogLevel['Info'], LogPattern['Sql'], $strLogSql);
    try {
        $objPlace->execute();
        $objPlaces = $objPlace->fetchAll(PDO::FETCH_ASSOC);
    }catch (Exception $e) {
        fncWriteLog(LogLevel['Error'], LogPattern['Error'], DISPLAY_NAME.' '.$e->getMessage());
    }
    // If exists data, process update,if not exists insert new
    if(@$objPlaces){
        $strError = PUBLIC_MSG_003_JPN;
        $strAction = ' 更新しました';
        $strSQL = '';
        $strSQL .= ' UPDATE m_place_name SET';
        $strSQL .= ' place_name_jpn = :place_name,';
        $strSQL .= ' place_name_eng = :place_name_eng,';
        $strSQL .= ' map_id = :map_id,';
        $strSQL .= ' up_date = :up_date';
        $strSQL .= ' WHERE place3_id =:place3_id';
        
    }else{
        $strSQL = '';
        $strSQL .= ' INSERT INTO m_place_name ';
        $strSQL .= ' SELECT';
        $strSQL .= ' (SELECT ISNULL(MAX( place_name_no ), 0)+ 1  FROM m_place_name),';
        $strSQL .= ' :place_name,';
        $strSQL .= ' :place_name_eng,';
        $strSQL .= ' :place3_id,';
        $strSQL .= ' :map_id,';
        $strSQL .= ' :up_date';
        $strError = PUBLIC_MSG_002_JPN;
        $strAction = ' 登録しました';
    }

    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
    $objStmt->bindParam(':place_name', $arrData['place_name']);
    $objStmt->bindParam(':place_name_eng', $arrData['place_name_eng']);
    $objStmt->bindParam(':place3_id', $arrData['place3_id']);
    $objStmt->bindParam(':map_id', $arrData['map_id']);
    $dtmUpdate = date('Y-m-d H:i:s');
    $objStmt->bindParam(':up_date', $dtmUpdate);
    $strLogSql = DISPLAY_NAME . $strSQL;
    $strLogSql = str_replace(
        ':place_name_eng',
        $arrData['place_name_eng'],
        $strLogSql
    );
    $strLogSql = str_replace(
        ':place_name',
        $arrData['place_name'],
        $strLogSql
    );
    $strLogSql = str_replace(':place3_id', $arrData['place3_id'], $strLogSql);
    $strLogSql = str_replace(':map_id', $arrData['map_id'], $strLogSql);
    $strLogSql = str_replace(':up_date', $dtmUpdate, $strLogSql);
    fncWriteLog(LogLevel['Info'], LogPattern['Sql'], $strLogSql);
    try {
        return $objStmt->execute();
    }catch (Exception $e) {
        $GLOBALS['g_dbaccess']->funcRollback();
        fncWriteLog(LogLevel['Error'], LogPattern['Error'], DISPLAY_NAME.' '.$strError);
    }
}

/**
 * Get list users to send emails
 *
 * @create 2020/04/07 KBS Tam.nv
 * @update
 * @return array users
 */
function fncGetUserBoadMail()
{
    $dtmDateStart = date('Y-m-d', strtotime(' +1 day'));
    $dtmDateEnd = date('Y-m-d');
    $strSQL = '';
    //▼2020/06/11 KBS S.Tasaki 関空ユーザ識別のためにSQL文を変更
    $strSQL .= ' SELECT mu.user_no ';
    $strSQL .= '      , mu.mail_address ';
    $strSQL .= '      , mu.user_name ';
    $strSQL .= '      , mu.language_type ';
    $strSQL .= '      , mg.admin_flag ';
    $strSQL .= '   FROM m_user mu';
    $strSQL .= '  INNER JOIN m_company mc ON mu.company_no = mc.company_no ';
    $strSQL .= '  INNER JOIN m_group mg ON mc.group_no = mg.group_no ';
    $strSQL .= '  WHERE ';
    $strSQL .= '        mu.bulletin_board_mail = 1 ';
    $strSQL .= '    AND mu.expiration_date_s < :START_DATE ';
    $strSQL .= '    AND mu.expiration_date_E >= :END_DATE ';
    
    /*
    $strSQL .= ' SELECT USER_NO,mail_address,user_name,language_type FROM m_user ';
    $strSQL .= ' WHERE';
    $strSQL .= ' bulletin_board_mail = 1 ';
    $strSQL .= ' AND expiration_date_s < :START_DATE ';
    $strSQL .= ' AND expiration_date_E >= :END_DATE ';
    */
    //▲2020/06/11 KBS S.Tasaki
    
    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
    $objStmt->bindParam(':START_DATE', $dtmDateStart);
    $objStmt->bindParam(':END_DATE', $dtmDateEnd);
    $strLogSql = DISPLAY_NAME . $strSQL;
    $strLogSql = str_replace(':START_DATE', $dtmDateStart, $strLogSql);
    $strLogSql = str_replace(':END_DATE', $dtmDateEnd, $strLogSql);
    fncWriteLog(LogLevel['Info'], LogPattern['Sql'], $strLogSql);
    try {
        $objStmt->execute();
        return $objStmt->fetchAll(PDO::FETCH_ASSOC);
    }catch (Exception $e) {
        $GLOBALS['g_dbaccess']->funcRollback();
        fncWriteLog(LogLevel['Error'],
            LogPattern['Error'], DISPLAY_NAME.' '.BULLETIN_BOARD_MAIL_MSG_011);
    }
}

/**
 * send email to array user
 *
 * @create 2020/04/07 KBS Tam.nv
 * @update
 * @param $arrUser user array
 * @return void
 */
function fncSendmailToBull($arrUser){
    // send mail
    //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
    $arrMail = array(
        'jpn' => array(),
        'eng' => array(),
        'jpn_ext' => array(),
        'eng_ext' => array(),
    );
    //▲2020/06/11 KBS S.Tasaki
    

    $arrTempMailJP = array();
    $arrTempMailEN = array();
    //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
    $arrTempExtMailJP = array();
    $arrTempExtMailEN = array();
    //▲2020/06/11 KBS S.Tasaki
    foreach($arrUser as $user) {
        //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
        if($user['admin_flag'] == 1){
	        // check language type and set body follow by language
	        if($user['language_type'] == 0) {
	            //check number element in template email, if is null will create new, if exists will add more
	            if(count($arrTempMailJP) == 0) {
	                array_push($arrTempMailJP, $user['mail_address']);
	                $arrMail['jpn'][] = array(
	                    'USER_NAME' => $user['user_name'],
	                    'MAIL_ADDRESS' => $user['mail_address'],
	                );
	            } else {
	                //check user mail address in template email, if is null will create new
	                if(!in_array($user['mail_address'], $arrTempMailJP)) {
	                    array_push($arrTempMailJP, $user['mail_address']);
	                    $arrMail['jpn'][] = array(
	                        'USER_NAME' => $user['user_name'],
	                        'MAIL_ADDRESS' => $user['mail_address'],
	                    );
	                }
	            }
	        } else {
	            //check number element in template email, if is null will create new, if exists will add more
	            if(count($arrTempMailEN) == 0) {
	                array_push($arrTempMailEN, $user['mail_address']);
	                $arrMail['eng'][] = array(
	                    'USER_NAME' => $user['user_name'],
	                    'MAIL_ADDRESS' => $user['mail_address'],
	                );
	            } else {
	                //check user mail address in template email, if is null will create new
	                if(!in_array($user['mail_address'], $arrTempMailEN)) {
	                    array_push($arrTempMailEN, $user['mail_address']);
	                    $arrMail['eng'][] = array(
	                        'USER_NAME' => $user['user_name'],
	                        'MAIL_ADDRESS' => $user['mail_address'],
	                    );
	                }
	            }
	        }
        }else{
	        // check language type and set body follow by language
	        if($user['language_type'] == 0) {
	            //check number element in template email, if is null will create new, if exists will add more
	            if(count($arrTempExtMailJP) == 0) {
	                array_push($arrTempExtMailJP, $user['mail_address']);
	                $arrMail['jpn_ext'][] = array(
	                    'USER_NAME' => $user['user_name'],
	                    'MAIL_ADDRESS' => $user['mail_address'],
	                );
	            } else {
	                //check user mail address in template email, if is null will create new
	                if(!in_array($user['mail_address'], $arrTempExtMailJP)) {
	                    array_push($arrTempExtMailJP, $user['mail_address']);
	                    $arrMail['jpn_ext'][] = array(
	                        'USER_NAME' => $user['user_name'],
	                        'MAIL_ADDRESS' => $user['mail_address'],
	                    );
	                }
	            }
	        } else {
	            //check number element in template email, if is null will create new, if exists will add more
	            if(count($arrTempExtMailEN) == 0) {
	                array_push($arrTempExtMailEN, $user['mail_address']);
	                $arrMail['eng_ext'][] = array(
	                    'USER_NAME' => $user['user_name'],
	                    'MAIL_ADDRESS' => $user['mail_address'],
	                );
	            } else {
	                //check user mail address in template email, if is null will create new
	                if(!in_array($user['mail_address'], $arrTempExtMailEN)) {
	                    array_push($arrTempExtMailEN, $user['mail_address']);
	                    $arrMail['eng_ext'][] = array(
	                        'USER_NAME' => $user['user_name'],
	                        'MAIL_ADDRESS' => $user['mail_address'],
	                    );
	                }
	            }
	        }
        }
        //▲2020/06/11 KBS S.Tasaki
        
    }

    //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
    $arrMailSend = array(
        'jpn' => array(),
        'eng' => array(),
        'jpn_ext' => array(),
        'eng_ext' => array(),
    );
    //▲2020/06/11 KBS S.Tasaki

    // divide array mail with MAIL_SUBMIT_NUMBER
    if(count($arrMail['jpn']) > 0) {
        $arrMailSend['jpn'] = array_chunk($arrMail['jpn'], MAIL_SUBMIT_NUMBER);
    }
    // count array English mail
    if(count($arrMail['eng']) > 0) {
        $arrMailSend['eng'] = array_chunk($arrMail['eng'], MAIL_SUBMIT_NUMBER);
    }
    
    //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
    // divide array mail with MAIL_SUBMIT_NUMBER
    if(count($arrMail['jpn_ext']) > 0) {
        $arrMailSend['jpn_ext'] = array_chunk($arrMail['jpn_ext'], MAIL_SUBMIT_NUMBER);
    }
    // count array English mail
    if(count($arrMail['eng_ext']) > 0) {
        $arrMailSend['eng_ext'] = array_chunk($arrMail['eng_ext'], MAIL_SUBMIT_NUMBER);
    }
    //▲2020/06/11 KBS S.Tasaki
    
    $arrSubject = array(
        'jpn' => MAIL_SUBMIT_TITLE_JPN,
        'eng' => MAIL_SUBMIT_TITLE_ENG,
    );

    //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
    $arrBody = array(
        'jpn' => is_file('common/mail_temp_jpn.txt') ?
            nl2br(file_get_contents("common/mail_temp_jpn.txt")) : '',
        'eng' => is_file('common/mail_temp_eng.txt') ?
            nl2br(file_get_contents("common/mail_temp_eng.txt")) : '',
        'jpn_ext' => is_file('common/mail_temp_ext_jpn.txt') ?
            nl2br(file_get_contents("common/mail_temp_ext_jpn.txt")) : '',
        'eng_ext' => is_file('common/mail_temp_ext_eng.txt') ?
            nl2br(file_get_contents("common/mail_temp_ext_eng.txt")) : '',
    );
    //▲2020/06/11 KBS S.Tasaki
    
    // check mail body, if not null will replace data in some element define in body
    if($arrBody['jpn'] != '') {
        $arrBody['jpn'] = str_replace ('%0%', date('m月d日H時i分'), $arrBody['jpn']);
        $arrBody['jpn'] = str_replace('%1%',BULLETIN_BOARD_MAIL_TEXT_001_JPN,$arrBody['jpn']);
    }
    // check mail body, if not null will replace data in some element define in body
    if($arrBody['eng'] != '') {
        $arrBody['eng'] = str_replace ('%0%', date('H:i, d M'), $arrBody['eng']);
        $arrBody['eng'] = str_replace('%1%',BULLETIN_BOARD_MAIL_TEXT_001_ENG,$arrBody['eng']);
    }
    
    //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
    // check mail body, if not null will replace data in some element define in body
    if($arrBody['jpn_ext'] != '') {
        $arrBody['jpn_ext'] = str_replace ('%0%', date('m月d日H時i分'), $arrBody['jpn_ext']);
        $arrBody['jpn_ext'] = str_replace('%1%',BULLETIN_BOARD_MAIL_TEXT_001_JPN,$arrBody['jpn_ext']);
    }
    // check mail body, if not null will replace data in some element define in body
    if($arrBody['eng_ext'] != '') {
        $arrBody['eng_ext'] = str_replace ('%0%', date('H:i, d M'), $arrBody['eng_ext']);
        $arrBody['eng_ext'] = str_replace('%1%',BULLETIN_BOARD_MAIL_TEXT_001_ENG,$arrBody['eng_ext']);
    }
    //▲2020/06/11 KBS S.Tasaki
    
    $blnFlagHasBccMailJPN = false;
    $blnFlagHasBccMailENG = false;
    //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
    $blnFlagHasBccExtMailJPN = false;
    $blnFlagHasBccExtMailENG = false;
    //▲2020/06/11 KBS S.Tasaki
    
    // count array email prepare, if have data, flag true, mail data will send when flag true
    if(count($arrMailSend['jpn']) > 0) {
        $blnFlagHasBccMailJPN = true;
    }
    // count array email prepare, if have data, flag true, mail data will send when flag true
    if(count($arrMailSend['eng']) > 0) {
        $blnFlagHasBccMailENG = true;
    }
    
    //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
    // count array email prepare, if have data, flag true, mail data will send when flag true
    if(count($arrMailSend['jpn_ext']) > 0) {
        $blnFlagHasBccExtMailJPN = true;
    }
    // count array email prepare, if have data, flag true, mail data will send when flag true
    if(count($arrMailSend['eng_ext']) > 0) {
        $blnFlagHasBccExtMailENG = true;
    }
    //▲2020/06/11 KBS S.Tasaki
    
    //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
    if(!$blnFlagHasBccMailJPN && !$blnFlagHasBccMailENG && !$blnFlagHasBccExtMailJPN && !$blnFlagHasBccExtMailENG) {
        fncSendMail(array(), $arrSubject['jpn'], $arrBody['jpn'], '');
    } else {
        foreach($arrMailSend as $strLang => $arrListMail) {
            $strSubjectSend = '';
            $strBodySend = '';
            if($strLang == 'jpn'){
                $strSubjectSend = $arrSubject['jpn'];
                $strBodySend = $arrBody['jpn'];
            }else if($strLang == 'eng'){
                $strSubjectSend = $arrSubject['eng'];
                $strBodySend = $arrBody['eng'];
            }else if($strLang == 'jpn_ext'){
                $strSubjectSend = $arrSubject['jpn'];
                $strBodySend = $arrBody['jpn_ext'];
            }else{
                $strSubjectSend = $arrSubject['eng'];
                $strBodySend = $arrBody['eng_ext'];
            }
            
            // check list email not null
            if(count($arrListMail) > 0) {
                foreach($arrListMail as $group) {
                    fncSendMail($group, $strSubjectSend, $strBodySend, '');
                }
            }
        }
    }
}
