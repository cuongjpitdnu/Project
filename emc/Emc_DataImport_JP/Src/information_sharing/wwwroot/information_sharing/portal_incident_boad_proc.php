<?php
/*
* @portal_incident_boad_proc.php
*
* @create 2020/02/17 KBS Tam.nv
* @update
*/

require_once('portal_function.php');

//check check box variable in sesson. if box true set check box is checked.
if(@$_SESSION['checkWithoutCom'] == 'true'){
    $strCheckWithoutCom = 'checked';
}

//get data inst category
$arrInsCategory = get_inst_cat();

//get data information category
$arrInfoCategory = get_info_cat();

//get data information pink pin - data registered
$arrInfos = get_t_information(NULL,$objLoginUserInfo,@$_SESSION['checkWithoutCom']);
//get data information white pin - data not register
$arrInfosNotReg = get_t_information(1,$objLoginUserInfo,@$_SESSION['checkWithoutCom']);

//get data information category
$arrInfosByCate = get_t_information(1,$objLoginUserInfo,@$_SESSION['checkWithoutCom'],1);
$arrInfoData = array();
$arrInfoByCateData = array();
$arrInfoNotReg = array();
$arrCompany = array();

foreach ($arrInfos as $data){
    $arrInfoData[$data['INST_CATEGORY_NO']][$data['COMPANY_NO']][] = $data;
    $arrCompany[$data['COMPANY_NO']] = $data['ABBREVIATIONS'.$strEndLang];
}

foreach ($arrInfosByCate as $data){
    $arrInfoByCateData[$data['INFO_CATEGORY_NO']][][] = $data;
    $arrCompany[$data['COMPANY_NO']] = $data['ABBREVIATIONS'.$strEndLang];
}


foreach ($arrInfosNotReg as $data){
    $arrInfoNotReg[$data['INST_CATEGORY_NO']][$data['COMPANY_NO']][] = $data;
    $arrCompany[$data['COMPANY_NO']]  = $data['ABBREVIATIONS'.$strEndLang];
}
?>
