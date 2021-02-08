<?php
// -------------------------------------------------------------------------
//	function	:
//	create		: 2020/02/17 KBS Tamnv
//	update		:
// -------------------------------------------------------------------------

require_once('common/common.php');
require_once('common/validate_user.php');

$arrayIdInCompany = get_all_id_in_company($objLoginUserInfo->intUserNo);
$mapArrIds = array();
foreach ($arrayIdInCompany as $item){
    $mapArrIds[] = $item['USER_NO'];
}

$check24h = 0;
if($_POST){
    $inserData = insert($_POST,$objLoginUserInfo);
    if($inserData){
        $listQuery = get_query($objLoginUserInfo,$check24h);
        return $listQuery;
    }
    //ログ書き込み
}else{
    if(isset($_GET['typeOfTime'])){
        $check24h = $_GET['typeOfTime'];
    }

    $listQuery = get_query($objLoginUserInfo,$check24h);
}



function insert($data,$objLoginUserInfo){
    if($data['sleTran']=='ej'){
        $enTxt = $data['txtSource'];
        $jpTxt = $data['txtTarget'];
    }else{
        $enTxt = $data['txtTarget'];
        $jpTxt = $data['txtSource'];
    }
    $flag = 1;
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
    $date = date('Y-m-d H:i:s');
    $objStmt->bindParam(':QUERY_DATE', $date);
    $objStmt->bindParam(':QUERY_USER_NO', $objLoginUserInfo->intUserNo);
    $objStmt->bindParam(':CONTENT_JPN', $jpTxt);
    $objStmt->bindParam(':CONTENTS_ENG', $enTxt);
    $objStmt->bindParam(':LANGUAGE_TYPE', $objLoginUserInfo->intLanguageType);
    $objStmt->bindParam(':CORRECTION_FLAG',$flag,PDO::PARAM_INT);
    $strLogSql = $strSQL;
    $strLogSql = str_replace(':QUERY_DATE', date('Y-m-d H:i:s'),$strLogSql);
    $strLogSql = str_replace(':QUERY_USER_NO', $objLoginUserInfo->intUserNo,$strLogSql);
    $strLogSql = str_replace(':CONTENT_JPN', $jpTxt,$strLogSql);
    $strLogSql = str_replace(':CONTENTS_ENG', $enTxt,$strLogSql);
    $strLogSql = str_replace(':LANGUAGE_TYPE', $objLoginUserInfo->intLanguageType,$strLogSql);
    $strLogSql = str_replace(':CORRECTION_FLAG', $flag,$strLogSql);
    fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);
    return $objStmt->execute();
}

function get_query($objLoginUserInfo,$check24h){
    $strSQL = '';
    $strSQL .= ' SELECT * FROM t_query ';
    $strSQL .= ' WHERE';
    //$strSQL .= ' QUERY_USER_NO = :QUERY_USER_NO AND';
    if($check24h){
        $strSQL .= ' QUERY_DATE >= DATEADD(day, -1, GETDATE()) AND';
    }
    $strSQL .= ' LANGUAGE_TYPE = :LANGUAGE_TYPE ';
    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);

//    $objStmt->bindParam(':QUERY_USER_NO', $objLoginUserInfo->intUserNo);
    $objStmt->bindParam(':LANGUAGE_TYPE', $objLoginUserInfo->intLanguageType);
    $strLogSql = $strSQL;
//    $strLogSql = str_replace(':QUERY_USER_NO', $objLoginUserInfo->intUserNo,$strLogSql);
    $strLogSql = str_replace(':LANGUAGE_TYPE', $objLoginUserInfo->intLanguageType,$strLogSql);
    fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);
    $objStmt->execute();
    $arrResult = $objStmt->fetchAll(PDO::FETCH_ASSOC);
    return $arrResult;
}

function get_all_id_in_company($userId){
    $strSQL = '';
    $strSQL .= ' SELECT USER_NO FROM m_user ';
    $strSQL .= ' WHERE';
    $strSQL .= ' COMPANY_NO = (SELECT COMPANY_NO FROM m_user WHERE USER_NO = :USER_NO ) ';
    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);

    $objStmt->bindParam(':USER_NO', $userId);
    $strLogSql = $strSQL;
    $strLogSql = str_replace(':USER_NO', $userId,$strLogSql);
    fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);
    $objStmt->execute();
    $arrResult = $objStmt->fetchAll(PDO::FETCH_ASSOC);
    return $arrResult;
}


?>
