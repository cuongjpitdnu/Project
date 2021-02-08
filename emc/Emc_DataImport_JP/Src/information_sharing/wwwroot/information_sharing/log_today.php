<?php
/*
* @log_today.php
*
* @create 2020/04/06 KBS Tam.nv
* @update
*/


require_once('common/common.php');

header('Content-type: text/html; charset=utf-8');
header('X-FRAME-OPTIONS: DENY');

const DISPLAY_NAME = '当日ログ出力 ';

//DB connection
if(fncConnectDB() == false){
    fncWriteLog(LogLevel['Error'], LogPattern['Error'], 'DB接続に失敗しました。');
    exit;
}

fncWriteLog(LogLevel['Info'], LogPattern['View'], DISPLAY_NAME. "実行");
// check log folder exists
if(!TODAY_LOG_FOLDER){
    echo "<script>alert('当日分のログ出力先フォルダに値がありません。')</script>";
    exit();
}

// check path to log folder exists
$strFolderPath = SHARE_FOLDER_LOG.'/'.TODAY_LOG_FOLDER.'/';
if (!is_dir($strFolderPath)) {
    echo "<script>alert('当日分のログ出力先フォルダのパスが存在しません。')</script>";
    exit();
}
// query to get log in database
$arrLogToday = fncGetLogToday();
fncUploadFile($arrLogToday);

echo "<script>alert('当日分のメンテナンスログを保存しました。')</script>";
exit();

/**
 * t_log Get the entire log
 *
 * @create 2020/04/07 KBS Tam.nv
 * @update
 * @return array log data
 */

function fncGetLogToday()
{
    $dtmStartTime = date('Y-m-d');
    $dtmEndTime = date('Y-m-d H:i:s');
    $strSQL = '';
    $strSQL .= ' SELECT * FROM t_log ';
    $strSQL .= ' WHERE';
    $strSQL .= ' reg_date >= :START_DATE ';
    $strSQL .= ' AND reg_date <= :END_DATE ';
    $strSQL .= ' ORDER BY logid ASC ';
    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
    $objStmt->bindParam(':START_DATE', $dtmStartTime);
    $objStmt->bindParam(':END_DATE', $dtmEndTime);
    $strLogSql = DISPLAY_NAME . $strSQL;
    $strLogSql = str_replace(':START_DATE', $dtmStartTime, $strLogSql);
    $strLogSql = str_replace(':END_DATE', $dtmEndTime, $strLogSql);
    fncWriteLog(LogLevel['Info'], LogPattern['Sql'], $strLogSql);
    try {
        $objStmt->execute();
        return $objStmt->fetchAll(PDO::FETCH_ASSOC);
    }catch (Exception $e) {
        // other mysql exception (not duplicate key entry)
        fncWriteLog(LogLevel['Error'],
            LogPattern['Error'], DISPLAY_NAME.' '.$e->getMessage());
    }
}


/**
 * upload file to log folder
 *
 * @create 2020/04/07 KBS Tam.nv
 * @update
 * @param $arrLogToday array data log
 * @return
 */

function fncUploadFile($arrLogToday)
{
    $strFolderPath = SHARE_FOLDER_LOG.'/'.TODAY_LOG_FOLDER.'/';
    $strfileName = $strFolderPath.'/'.date('YmdHis').".txt";
    $fileLog = fopen($strfileName, "w") or die("Unable to open file!");
    $strLog = "";
    // check array data log, if null, return error message
    if($arrLogToday){
        foreach ($arrLogToday as $arrItem){
            $strLog .= $arrItem['CONTENT'].'';
        }
    }else{
        $strLog = LOG_TODAY_MSG_001;
    }

    try {
        fwrite($fileLog, $strLog);
        fclose($fileLog);
    }catch (Exception $e) {
        fncWriteLog(LogLevel['Error'],
            LogPattern['Error'], DISPLAY_NAME.' '.$e->getMessage());
    }

}


