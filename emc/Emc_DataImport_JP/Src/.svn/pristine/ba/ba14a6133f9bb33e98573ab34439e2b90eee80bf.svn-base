<?php
/*
* @incident_boad_ajax.php
*
* @create 2020/02/20 KBS Tam.nv
* @update
*/
    require_once('portal_function.php');
    //check variable check box get data without other company
    if(@$_POST['checkWithoutCom']){
        $strChkWithoutCom = $_POST['checkWithoutCom'];
        $_SESSION['checkWithoutCom'] = $strChkWithoutCom;
    }else{
        $strChkWithoutCom = @$_SESSION['checkWithoutCom'];
    }
    // process with only action log-form to auto load information category
    if(@$_POST['action']=='long-form'){
        $arrInfoCategory = get_info_cat();
        $arrInfosByCate = get_t_information(1,$objLoginUserInfo,$strChkWithoutCom,1);
        $arrInfoByCateData = array();
        foreach ($arrInfosByCate as $data){
            $arrInfoByCateData[$data['INFO_CATEGORY_NO']][][] = $data;
            $arrCompany[$data['COMPANY_NO']]  = $data['ABBREVIATIONS'.$strEndLang];
        }
    }
    // process with only action load-info to auto load info by inst category
    if(@$_POST['action']=='load-info'){
        $arrInsCategory = get_inst_cat();
        $arrInfoCategory = get_info_cat();
        $arrInfos = get_t_information(NULL,$objLoginUserInfo,$strChkWithoutCom);
        $arrInfosNotReg = get_t_information(1,$objLoginUserInfo,$strChkWithoutCom);
        $arrInfoData = array();
        $arrInfoNotReg = array();
        $arrCompany = array();
        foreach ($arrInfos as $data){
            $arrInfoData[$data['INST_CATEGORY_NO']][$data['COMPANY_NO']][] = $data;
            $arrCompany[$data['COMPANY_NO']] = $data['ABBREVIATIONS'.$strEndLang];
        }

        foreach ($arrInfosNotReg as $data){
            $arrInfoNotReg[$data['INST_CATEGORY_NO']][$data['COMPANY_NO']][] = $data;
            $arrCompany[$data['COMPANY_NO']]  = $data['ABBREVIATIONS'.$strEndLang];
        }
    }
    // process with only action pin-info when pin or unpin data
    if(@$_POST['action']=='pin-info'){
        $arrResult = array();
        // check type to insert or delete record in table t_inst_company_sort
        if($_POST['intType']){
            $strResult = fncInserInsComSort($objLoginUserInfo->intUserNo,$_POST['intCatNo'],$_POST['intComNo']);
        }else{
            $strResult = fncDeleteInsComSort($objLoginUserInfo->intUserNo,$_POST['intCatNo'],$_POST['intComNo']);
        }
        //check result to return message
        if($strResult){
            $arrResult['success'] = 1;
        }else{
            $arrResult['error'] = 1;
        }
        echo json_encode($arrResult);
    }
    // process with only action load-incident to auto reload incident list
    if(@$_POST['action']=='load-incident'){
        $intIncident = count(get_incident_case());
        $intIncidentJcmg = $intIncident;
        //check permission show data
        if($objLoginUserInfo->intJcmgTabPerm == 0){
	        $intIncident = 0;
	    }
        $arrResult['intIncident'] = $intIncident;

        $arrResult['intJcmgTabPerm'] = $objLoginUserInfo->intJcmgTabPerm;

        $arrResult['strDone'] = $arrText['PORTAL_INCIDENT_MSG_001'];
        $arrResult['strNew'] = $arrText['PORTAL_INCIDENT_MSG_004'];
        //check permission
        if($intIncidentJcmg == 0 && $objLoginUserInfo->intIncidentCaseRegPerm){
            $intShowBtn = 1;
        }else{
            $intShowBtn = 0;
        }

        $arrResult['intShowBtn'] = $intShowBtn;
        $arrResult['strUtcTime'] = $strUtcTime;
        $arrResult['strJstTime'] = $strJstTime;
        echo json_encode($arrResult);
    }
?>

