<?php
/*
* @tran_proc.php
*
* @create 2020/02/18 KBS Tam.nv
* @update
*/

require_once('common/common.php');
require_once('common/validate_user.php');
require_once('portal_function.php');
//only process with post method have action tran
if(@$_POST['action'] == 'tran'){
    // check variable manual and txtTarget, if exist them will stop process
    if($_POST['ckeckMan'] =='true' || $_POST['txtTarget']){

    }else{
        // check permission
        if (!$objLoginUserInfo->intQueryRegPerm) {
            echo 900;
            exit();
        }
        // action Ajax translate text.
        $arrValidate = validateInputText($_POST,$arrText,$objLoginUserInfo, 0);
        echo json_encode($arrValidate);
    }
}
