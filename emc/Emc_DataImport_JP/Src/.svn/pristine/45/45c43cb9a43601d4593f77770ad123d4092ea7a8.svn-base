<?php
/*
* @portal_proc.php
*
* @create 2020/02/17 KBS Tam.nv
* @update
*/


require_once('portal_function.php');
//variable check load from query view to set display name in log
$_POST['load_fromQuery'] =1;
//thang
if(isset($_REQUEST['tab'])){
    $_SESSION['tab'] = $_REQUEST['tab'];
    exit();
}
//end thang
//flag check request from Ajax
$intAjax = 0;

//only process with request have action load_info, this is function auto load for announce content
if(@$_POST['action']=='load_info'){
    $_SESSION["SES_TIME"] = date( "Y/m/d H:i:s", time() );
    $dtmDate = $_POST['dtmDateFilter'];
    
    //2020/05/08 T.Masuda 未翻訳のお知らせを先に翻訳するように変更
    $arrAnnUntran = annUntran();
    //2020/05/08 T.Masuda
    
    //check and translate if have record untranslated
    tranInfo($arrAnnUntran,$objLoginUserInfo);
    
    $arrInfo = get_annouce($dtmDate);
    return $arrInfo;
}elseif(@$_POST['action']=='load_bull'){ //only process with request have action load_bull, this is function auto load for bulletin content
    $_SESSION["SES_TIME"] = date( "Y/m/d H:i:s", time() );
    $arrBullUntran = bullUntran();
    //check and translate if have record untranslated
    tranBul($arrBullUntran,$objLoginUserInfo);
    $arrBulletin = get_query_bulletin();
    $intAjax = 1;
    return $arrBulletin;
}elseif(@$_POST['action']=='load_bull_map'){ //only process with request have action load_bull_map, this is function auto load for bulletin map
    $_SESSION["SES_TIME"] = date( "Y/m/d H:i:s", time() );
    $strIdList = $_POST['arrLocationIds'];
    $arrIdList = explode(',',$strIdList);
    $arrBulletin = get_query_bulletin($arrIdList);
    //if have data will loop data
    if(!empty($arrBulletin)){
        //check if have 1 record show detail view, if have multiple record show list view
        if(count($arrBulletin) == 1){
            $_POST['id'] = $arrBulletin[0]['BULLETIN_BOARD_NO'];
            $_POST['screen'] = 'portal';
            include 'bulletin_board_view.php';
        }else{
            include 'bull_form_table.php';
        }
    }else{
        echo '';
    }
}else{
    //if don't have post action process as first time load page.
    $strViewLog = 'ポータル画面 画面表示 (ユーザID = '.$objLoginUserInfo->strUserID.') '.
        (isset($_SERVER['HTTP_REFERER']) ? $_SERVER['HTTP_REFERER'] : null);
    fncWriteLog(LogLevel['Info'] , LogPattern['View'], $strViewLog);
    
    fncSessionUp('ポータル画面');
    
    $objLoginUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);
    
    //2020/05/08 T.Masuda 未翻訳のお知らせを先に翻訳するように変更
    $arrAnnUntran = annUntran();
    //2020/05/08 T.Masuda
    tranInfo($arrAnnUntran,$objLoginUserInfo);
    $arrInfo = get_annouce();
    $arrBullUntran = bullUntran();
    tranBul($arrBullUntran,$objLoginUserInfo);
    $arrBulletin = get_query_bulletin();
    $intIncident = count(get_incident_case());
    // check permission view JMC boad
    if($objLoginUserInfo->intJcmgTabPerm == 0){
        $intIncident = 0;
    }
    
    $arrLinkCategory = get_link_category();
    // check permission register Incident if is 1 show button
    if(!$intIncident && $objLoginUserInfo->intIncidentCaseRegPerm){
        $intShowBtn = 1;
    }else{
        $intShowBtn = 0;
    }

}

?>
