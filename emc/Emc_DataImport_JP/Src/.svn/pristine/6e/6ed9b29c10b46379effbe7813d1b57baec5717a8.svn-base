<?php
/*
* @category_detail_proc.php
*
* @create 2020/02/27 KBS Tam.nv
* @update
*/


require_once('common/common.php');
require_once('common/validate_user.php');
const DISPLAY_NAME = 'リンク情報一覧';

//define array translate text message
$arrMSG = [
    'PORTAL_DAILY_TEXT_009',
    'PUBLIC_BUTTON_003',
];

// Translate text folow by language Type
$arrText = getListTextTranslate($arrMSG,$objLoginUserInfo->intLanguageType);
$arrCategory = get_link_category_detail($_POST['id']);

/**
 * m_link_category get list link category
 *
 * @create 2020/02/17 KBS Tam.nv
 * @update
 * @param $intCateId category id
 * @return array category array
 */

function get_link_category_detail($intCateId){
    $strSQL = '';
    $strSQL .= ' SELECT *';
    $strSQL .= ' FROM m_link_category ';
    $strSQL .= ' LEFT JOIN t_link ';
    $strSQL .= ' ON m_link_category.LINK_CATEGORY_NO = t_link.LINK_CATEGORY_NO ';
    $strSQL .= ' WHERE m_link_category.LINK_CATEGORY_NO = :LINK_CATEGORY_NO';
    $strSQL .= ' ORDER BY t_link.SORT_NO ASC';
    $objStmt = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
    $objStmt->bindParam(':LINK_CATEGORY_NO', $intCateId);
    $strLogSql = DISPLAY_NAME.$strSQL;
    $strLogSql = str_replace(':LINK_CATEGORY_NO', $intCateId,$strLogSql);
    fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], $strLogSql);
    $objStmt->execute();
    $arrResult = $objStmt->fetchAll(PDO::FETCH_ASSOC);
    return $arrResult;
}


?>
