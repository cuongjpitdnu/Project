<?php
    /*
    * @information_edit_proc.php
    *
    * @create 2020/02/20 AKB Thang
    * @update
    */
require_once('common/common.php');
require_once('common/session_check.php');

header('Content-type: text/html; charset=utf-8');
header('X-FRAME-OPTIONS: DENY');

/**
 * validate
 * @create 2020/03/23 AKB Thang
 * @update
 * @param int $intFileNo
 * @param string $strFileNotExist
 * @param string $strFileNameError
 * @param string $strFileNameTypeError
 * @param array $arrOne
 * @return
 */
$intTotalFileSize = 0;
define('FILE_NAME_LENGTH', 100);
define('TOTAL_FILE_SIZE', 10485760);
function fncValidateFile(
    $intFileNo, $strFileNotExist, $strFileNameError,
    $strFileNameTypeError, $strFileNameCharError
){
    if(isset($_FILES['file'.$intFileNo])&&$_FILES['file'.$intFileNo]['name'] != ''){
        //check input file exist
        if(
            !isset($_FILES['file'.$intFileNo]['error'])
            || is_array($_FILES['file'.$intFileNo]['error'])
            || $_FILES['file'.$intFileNo]['size'] == 0
        ) {
            $_SESSION['INFORMATION_EDIT_MSG_ERROR_INPUT'] .= $strFileNotExist .'<br>';
        }else{
            if(!file_exists($_FILES['file'.$intFileNo]['tmp_name'])){
                //file not upload successfully
                $_SESSION['INFORMATION_EDIT_MSG_ERROR_INPUT'] .= $strFileNotExist .'<br>';
            }else{
                //file name exceed max length
                if(mb_strwidth($_FILES['file'.$intFileNo]['name']) > FILE_NAME_LENGTH ){
                    $_SESSION['INFORMATION_EDIT_MSG_ERROR_INPUT'] .= $strFileNameError .'<br>';
                }
                //check file type
                if(!fncCheckFileType($_FILES['file'.$intFileNo]['name'], SCREEN_NAME)){
                    $_SESSION['INFORMATION_EDIT_MSG_ERROR_INPUT'] .= $strFileNameTypeError .'<br>';
                }


                if(preg_match('/[\\\]/u',$_FILES['file'.$intFileNo]['name'])){
                    $_SESSION['REQUEST_EDIT_MSG_ERROR_INPUT'] .= $strFileNameCharError .'<br>';
                }

                //check special char
                $strRegExp = '/[\\/\:\*\?\"\<\>\|\&\#\%\']+|((COM|com)[0-9])';
                $strRegExp .= '|(AUX|aux)|(CON|con)|((LPT|lpt)[0-9])|(NUL|nul)|(PRN|prn)/';
                preg_match(
                    $strRegExp,
                    $_FILES['file'.$intFileNo]['name'],
                    $strFileNameErr
                );
                if(count($strFileNameErr) > 0) {
                    $_SESSION['INFORMATION_EDIT_MSG_ERROR_INPUT'] .= $strFileNameCharError .'<br>';
                }

                //total file size
                $GLOBALS['intTotalFileSize'] += $_FILES['file'.$intFileNo]['size'];
            }

        }
    }

}
/**
 * validate
 * @create 2020/04/17 AKB Thang
 * @update
 * @param array $arrVal
 * @return true false
 */
function fncDupArr($arrVal) {
    $arrDup = array();
    foreach ($arrVal as $val) {
        if (++$arrDup[$val] > 1) {
            return true;
        }
    }
    return false;
}
if(fncConnectDB() == false){
	$_SESSION['LOGIN_ERROR'] = 'DB接続に失敗しました。';
	header('Location: login.php');
	exit;
}
if(!isset($_SESSION['LOGINUSER_INFO'])){
    echo "
    <script>
    alert('".PUBLIC_MSG_008_JPN . "/" . PUBLIC_MSG_008_ENG."');
        window.location.href = 'login.php';
    </script>
    ";

	exit();
}
//create session to store error message
$_SESSION['INFORMATION_EDIT_MSG_ERROR_INPUT'] = '';
//define constant
define('SCREEN_NAME', '情報登録編集画面');
define('SCREEN_NAME_CREATE', '情報登録編集画面 登録');
define('SCREEN_NAME_EDIT', '情報登録編集画面 更新');

$objUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);



// get list text translate
$arrTitle =  array(
    'PUBLIC_MSG_021',
    'PUBLIC_MSG_022',
    'PUBLIC_MSG_023',
    'PUBLIC_MSG_024',
    'PUBLIC_MSG_026',
    'PUBLIC_MSG_027',
    'PUBLIC_MSG_028',
    'PUBLIC_MSG_029',
    'PUBLIC_MSG_031',
    'PUBLIC_MSG_032',
    'PUBLIC_MSG_033',
    'PUBLIC_MSG_034',
    'PUBLIC_MSG_036',
    'PUBLIC_MSG_037',
    'PUBLIC_MSG_038',
    'PUBLIC_MSG_039',
    'PUBLIC_MSG_009',
    'PUBLIC_MSG_003',
    'PUBLIC_MSG_002',
    'PUBLIC_MSG_010',
    'PUBLIC_MSG_011',
    'PUBLIC_MSG_012',
    'PUBLIC_MSG_013',
    'PUBLIC_MSG_014',
    'PUBLIC_MSG_015',
    'PUBLIC_MSG_016',
    'PUBLIC_MSG_017',
    'PUBLIC_MSG_018',
    'PUBLIC_MSG_043',
    'PUBLIC_MSG_049',
    'PUBLIC_MSG_050',
    'INFORMATION_EDIT_MSG_004',
    'INFORMATION_EDIT_MSG_005',
    'INFORMATION_EDIT_MSG_006',
    'INFORMATION_EDIT_MSG_007',
    'INFORMATION_EDIT_MSG_008',
    'INFORMATION_EDIT_MSG_009',
    'INFORMATION_EDIT_MSG_010',
    'INFORMATION_EDIT_MSG_001',
    'PUBLIC_MSG_019',
    'PUBLIC_MSG_051',

    //2020/04/27 T.Masuda 既に削除された情報
    'INFORMATION_EDIT_MSG_011',
    //2020/04/27 T.Masuda

);
// get list text(header, title, msg) with languague_type of user logged
$arrTextTranslate = getListTextTranslate(
    $arrTitle,
    $objUserInfo->intLanguageType
);
//session check
fncSessionTimeOutCheck(1);
//check get request
fncGetRequestCheck($arrTextTranslate);
// check request directly
if(!isset($_SERVER['HTTP_REFERER'])){
    echo '<script type="text/javascript">
            function goBack() {
                // Check to see if override is needed here
                // If no override needed, call history.back()
                history.go(-1);
                return false;
            }
            alert("'.$arrTextTranslate['PUBLIC_MSG_049'].'");
            goBack();
        </script>';
    die();
}

//redirect if dont have permission
if($objUserInfo->intInformationRegPerm != 1 && $objUserInfo->intMenuPerm != 1){
    echo 900;
    exit();
}

$_SESSION['INFORMATION_EDIT_ERROR'] = array();

//check file download
if(isset($_POST)) {
    if(isset($_POST['action'])) {
        // ①のファイルが存在するか確認する。
        if($_POST['action'] == 'checkFile') {

            $strFilePath = base64_decode($_POST['file']);
            // check file exist
            if(is_file($strFilePath)) {
                echo base64_encode($strFilePath);
            } else {
                echo 0;
            }
        }
    }

    if(isset($_POST['mode'])) {
        if($_POST['mode'] == 99) {
            $strFilePath = base64_decode($_POST['path']);
            //call function download file
            fncDownloadFile($strFilePath);
        }
    }
}
//define validate contstant
define('CONTACT_INFO_TEXT_LENGTH', 13);
define('LENGTH_TITLE_JPN_TO_ENG', 30);
define('LENGTH_TITLE_ENG_TO_JPN', 150);
define('LENGTH_CONTENT_JPN_TO_ENG', 1000);
define('LENGTH_CONTENT_ENG_TO_JPN', 5000);
//btn translate
if(isset($_POST['mode']) && $_POST['mode'] == 2){

    //2020/04/27 T.Masuda ボタンクリック時にログ出力
    if(isset($_POST['save'])){
        if($_POST['INFORMATION_NO']!=''){
            //log edit button
            fncWriteLog(
                LogLevel['Info'],
                LogPattern['Button'],
                SCREEN_NAME_EDIT . ' (ユーザID = '.$objUserInfo->strUserID.')'
                );
        }else{
            //log insert button
            fncWriteLog(
                LogLevel['Info'],
                LogPattern['Button'],
                SCREEN_NAME_CREATE . ' (ユーザID = '.$objUserInfo->strUserID.')'
                );
        }
    }
    //2020/04/27 T.Masuda
    $strErrMsg = '';

    //2020/04/27 T.Masuda 確認メッセージの前に入力チェック
    $intTranslate = $_POST['LANGUAGE_TYPE'] == 'ja' ? 0 : 1;

    if(!isset($_POST['save'])){
        //入力チェック
        $_SESSION['INFORMATION_EDIT_ERROR']
        = array_merge($_SESSION['INFORMATION_EDIT_ERROR'],
            fncTranInputCheck($_POST['title'], $_POST['content'],
                              '','',$intTranslate,0,0,
                              $arrTextTranslate));
    }else{
        //入力チェック
        $_SESSION['INFORMATION_EDIT_ERROR']
        = array_merge($_SESSION['INFORMATION_EDIT_ERROR'],
            fncTranInputCheck($_POST['title'], $_POST['content'],
                              $_POST['title1'],$_POST['content1'],
                              $intTranslate,$_POST['manualTranslate'],1,
                              $arrTextTranslate));
    }

    if(count($_SESSION['INFORMATION_EDIT_ERROR']) > 0) {
        foreach($_SESSION['INFORMATION_EDIT_ERROR'] as $strError) {
            $strErrMsg .= $strError.'<br>';
        }
    }

    if(isset($_POST['save'])){
        //date check
        if(!isset($_POST['CONFIRM_DATE']) || $_POST['CONFIRM_DATE'] == ''){
            $strErrMsg.= $arrTextTranslate['INFORMATION_EDIT_MSG_004'].'<br>';
        }else{
            if(!checkFormatDateTimeInput2($_POST['CONFIRM_DATE'])){
                $strErrMsg.= $arrTextTranslate['INFORMATION_EDIT_MSG_005'].'<br>';
            }
        }
        //contact check
        if(!isset($_POST['CONTACT_INFO']) || $_POST['CONTACT_INFO'] == ''){
            $strErrMsg.= $arrTextTranslate['INFORMATION_EDIT_MSG_006'].'<br>';
        }else{
            //check half size
            if(!fncCheckEngText($_POST['CONTACT_INFO'])){
                $strErrMsg.= $arrTextTranslate['INFORMATION_EDIT_MSG_007'].'<br>';
            }
            //check length
            if(mb_strwidth($_POST['CONTACT_INFO']) > CONTACT_INFO_TEXT_LENGTH){
                $strErrMsg.= $arrTextTranslate['INFORMATION_EDIT_MSG_008'].'<br>';
            }
        }

        //check company
        if(isset($_POST['COMPANY_NO']) && $_POST['COMPANY_NO']==''){
            //not input
            $strErrMsg.= $arrTextTranslate['INFORMATION_EDIT_MSG_009'].'<br>';
        }
        else if(isset($_POST['COMPANY_NO']) && $_POST['COMPANY_NO']!=''){
            $arrCompanyTmp = fncSelectOne(
                "SELECT COMPANY_NO FROM m_company WHERE COMPANY_NO=? AND DEL_FLAG=?",
                [$_POST['COMPANY_NO'], 0],
                SCREEN_NAME
                );
            if(!is_array($arrCompanyTmp) || count($arrCompanyTmp) == 0){
                //company not exist
                $strErrMsg.= $arrTextTranslate['INFORMATION_EDIT_MSG_009'].'<br>';
            }
        }

        //check inst category
        if(!isset($_POST['INFO_CATEGORY_NO']) || $_POST['INFO_CATEGORY_NO']==''){
            //not input
            $strErrMsg.= $arrTextTranslate['INFORMATION_EDIT_MSG_010'].'<br>';
        }
    }
    //2020/04/27 T.Masuda

    if(isset($strErrMsg) && $strErrMsg != ''){
        //show error message
        ?>
            $('.edit-error').html('<?php echo $strErrMsg; ?>');
            $('#myModal').animate({ scrollTop: 0 }, '10');
        <?php
    }else{
        //title translated and content translated are null, apply translation
        if($_POST['title1'] == '' && $_POST['content1'] == ''){
            if($_POST['LANGUAGE_TYPE'] == 'en'){
                //english -> japansese
                $titleTranslated = is_array(tranAmazon($_POST['title'], 1))
                ? '' : tranAmazon($_POST['title'], 1);
                $contentTranslated = is_array(tranAmazon($_POST['content'], 1))
                ? '' : tranAmazon($_POST['content'], 1) ;
            }else{
                //japanese -> english
                $titleTranslated = is_array(tranAmazon($_POST['title'], 0))
                ? '' : tranAmazon($_POST['title'], 0);
                $contentTranslated = is_array(tranAmazon($_POST['content'], 0))
                ? '' : tranAmazon($_POST['content'], 0) ;
            }
            if(!($titleTranslated == '' ||  $contentTranslated == '')){
                //title translated and content translated(affter translation) are not null
                echo json_encode([$titleTranslated, $contentTranslated]);
            }else{
                if(!isset($_POST['save'])){
                    //if post button is pressed
            ?>
                alert('<?php echo $arrTextTranslate['PUBLIC_MSG_010']; ?>');
            <?php
                }else{
                    //if translation button is pressed
                    echo 2;
                }
            }
        }else{
            //autotranslation off
            echo 1;
        }
    }
    exit();
}
//company change

if(isset($_POST['mode']) && $_POST['mode'] == 3){
    //get arr company
    $arrOneCompany = fncSelectOne(
        "SELECT * FROM m_company WHERE COMPANY_NO=? AND DEL_FLAG=?",
        [$_POST['company_no'], 0],
        SCREEN_NAME
    );
    if($arrOneCompany){
        //if the company is exist, get data to return
        $arrTempOne = fncSelectOne(
            "SELECT

            ".iff(
                $objUserInfo->intLanguageType,
                ' m_inst_category.INST_CATEGORY_NAME_ENG as INST_CATEGORY_NAME',
                ' m_inst_category.INST_CATEGORY_NAME_JPN as INST_CATEGORY_NAME'
            )."

            FROM m_company
            INNER JOIN m_inst_category
            ON m_inst_category.INST_CATEGORY_NO = m_company.INST_CATEGORY_NO
            WHERE COMPANY_NO=?",
            [$arrOneCompany['COMPANY_NO']],
            SCREEN_NAME
        );

        if(is_array($arrTempOne)){
            //get the data successfully
            echo json_encode(
                [$arrTempOne['INST_CATEGORY_NAME']]
            ) ;
        }else{
            //get data not successfully
            echo json_encode([[]]);
        }

    }else{
        //get the company not successfully
        echo json_encode([[]]);
    }
    exit();
}

//press post button, check input
if(isset($_POST['mode']) && $_POST['mode'] == 1){
    if(isset($_POST['INFORMATION_NO'])){
        if($_POST['INFORMATION_NO']!=''){
            //get information data
            $arrOne = fncSelectOne(
                "SELECT * FROM t_information WHERE INFORMATION_NO=?",
                [$_POST['INFORMATION_NO']],
                SCREEN_NAME
            );

        }

        if(isset($arrOne) && (!is_array($arrOne) || count($arrOne)==0)){
            //2020/04/27 T.Masuda 既に削除された情報の場合
            //update information is  not exist

            fncWriteLog(
                LogLevel['Error'],
                LogPattern['Error'],
                SCREEN_NAME . ' ' . $arrTextTranslate['PUBLIC_MSG_003']
                );
            echo "alert ('" .$arrTextTranslate['INFORMATION_EDIT_MSG_011']."');";
            echo "loadPortalClose();";
            echo "$('#myModal').modal('hide');";
            exit();
            //2020/04/27 T.Masuda
        }else{
            //check file
            for($intCount = 1; $intCount <= 5; $intCount++){
                fncValidateFile(
                    $intCount,
                    $arrTextTranslate['PUBLIC_MSG_01'.$intCount]
                    . ' ' . $arrTextTranslate['PUBLIC_MSG_016'],
                    $arrTextTranslate['PUBLIC_MSG_01'.$intCount]
                    . ' ' . $arrTextTranslate['PUBLIC_MSG_018'],
                    $arrTextTranslate['PUBLIC_MSG_01'.$intCount]
                    . ' ' . $arrTextTranslate['PUBLIC_MSG_017'],
                    $arrTextTranslate['PUBLIC_MSG_01'.$intCount]
                    . ' ' . $arrTextTranslate['PUBLIC_MSG_050']
                );
            }
            $arrFileName = [];
            if(isset($arrOne) && count($arrOne) > 0){
                //get total file size on server
                for($i = 1; $i <= 5; $i++) {
                    //check file 1 -> 5
                    if(isset($arrOne['TMP_FILE_NAME'.$i])
                        && $arrOne['TMP_FILE_NAME'.$i] != '') {
                        if(!isset($_POST['chkDelete'.$i])) {
                            //, if file exist and is not deleted,  add file size to total size
                            $strFilePath = SHARE_FOLDER.'\\'.INFORMATION_ATTACHMENT_FOLDER;
                            $strFilePath .= '\\'.$arrOne['INFORMATION_NO']
                            .'\\'.$i.'\\'.$arrOne['TMP_FILE_NAME'.$i];
                            if(is_file($strFilePath)){
                                //file exist
                                $intTotalFileSize += filesize($strFilePath);
                            }
                            $arrFileName[] = $arrOne['TMP_FILE_NAME'.$i];
                        }
                    }
                }
            }

            //check total file size
            if($intTotalFileSize > TOTAL_FILE_SIZE){
                $_SESSION['INFORMATION_EDIT_MSG_ERROR_INPUT']
                .= $arrTextTranslate['PUBLIC_MSG_019'].'<br>';
            }
            //check file name
            for($intCount = 1; $intCount <= 5; $intCount++){
                if(isset($_FILES['file'.$intCount]['name'])
                && $_FILES['file'.$intCount]['name']!=''){
                    $arrFileName[] = $_FILES['file'.$intCount]['name'];
                }
            }
            if(fncDupArr($arrFileName)){
                $_SESSION['INFORMATION_EDIT_MSG_ERROR_INPUT']
                .= $arrTextTranslate['PUBLIC_MSG_051'].'<br>';
            }
        }
    }
    else{
        exit();
    }

if($_SESSION['INFORMATION_EDIT_MSG_ERROR_INPUT'] != ''){
    //show error message
?>

$('.edit-error').html('<?php
echo $_SESSION['INFORMATION_EDIT_MSG_ERROR_INPUT']; ?>');
$('#myModal').animate({ scrollTop: 0 }, '10');

<?php
}else{

    if(isset($_POST['errFile'])){
        //file upload error, exit
        exit();
    }
    //get admin flag of login user
    $arrUserLoginTmp = fncSelectOne(
        "SELECT ADMIN_FLAG FROM m_user
        INNER JOIN m_company ON m_company.COMPANY_NO = m_user.COMPANY_NO
        INNER JOIN m_group ON m_group.GROUP_NO = m_company.GROUP_NO
        WHERE USER_NO=? AND m_company.DEL_FLAG=?",
        [$objUserInfo->intUserNo, 0],
        SCREEN_NAME
    );
    if($_POST['INFORMATION_NO']!=''){
        //process update
        //check incident case
        $arrIncidentOne = fncSelectOne(
            "SELECT INCIDENT_CASE_NO FROM t_incident_case
            WHERE INCIDENT_CASE_NO=?",
            [$arrOne['INCIDENT_CASE_NO']],
            SCREEN_NAME
        );
        if(!is_array($arrIncidentOne) || count($arrIncidentOne) == 0){
            //incident case is not exist, write log and alert
            fncWriteLog(
                LogLevel['Error'],
                LogPattern['Error'],
                SCREEN_NAME.' ' . $arrTextTranslate['PUBLIC_MSG_043']
            );
            echo "alert('".$arrTextTranslate['PUBLIC_MSG_043']."');";
            echo "loadPortalClose();";
            echo "$('#myModal').modal('hide');";
            exit();
        }
        //delete folder
        for($intCount = 1; $intCount<=5; $intCount++){
            if(isset($_POST['chkDelete'.$intCount])){
                $strPath = SHARE_FOLDER.'/'.INFORMATION_ATTACHMENT_FOLDER
                .'/'.$arrOne['INFORMATION_NO'].'/'.$intCount.'/';
                //delete folder
                if(is_dir($strPath)){
                    $files = glob($strPath.'*'); // get all file names
                    foreach($files as $file){ // iterate files
                    if(is_file($file))
                        unlink($file); // delete file
                    }
                    rmdir($strPath);

                }
            }

        }
        $blnCheck = true;
        for($intCount = 1; $intCount<=5; $intCount++){
            //upload file
            if(isset($_FILES['file'.$intCount])
            && $_FILES['file'.$intCount]['name'] != ''){
                //file $intCount exist
                $strPath = SHARE_FOLDER.'/'.INFORMATION_ATTACHMENT_FOLDER
                .'/'.$arrOne['INFORMATION_NO'].'/'.$intCount.'/';
                if (!file_exists($strPath)) {
                    //create folder if not exist
                    mkdir($strPath, 0777, true);
                }
                $files = glob($strPath.'*'); // get all file names
                foreach($files as $file){ // iterate files
                if(is_file($file))
                    unlink($file); // delete file
                }
                if(
                    !move_uploaded_file(
                        $_FILES["file".$intCount]["tmp_name"]
                        ,$strPath.basename($_FILES['file'.$intCount]['name'])
                    )
                ){
                    //
                    $blnCheck = false;
                    break;
                }
            }
        }

        if($blnCheck){
            //file upload successfully, update data
            if(!is_array($arrUserLoginTmp) || count($arrUserLoginTmp) == 0){
                fncWriteLog(
                    LogLevel['Error'],
                    LogPattern['Error'],
                    SCREEN_NAME_EDIT . ' ' . $arrTextTranslate['PUBLIC_MSG_003']
                );
                echo "$('.edit-error').html('".$arrTextTranslate['PUBLIC_MSG_003']."');";
                echo "$('#myModal').animate({ scrollTop: 0 }, '10');";
                exit();
            }
            //update information
            $objResult = fncProcessData(
                [
                    'INFORMATION_NO' => $_POST['INFORMATION_NO'],
                    'TITLE_JPN' => ($_POST['LANGUAGE_TYPE'] == 'ja'
                    ? $_POST['titleSource'][0] : $_POST['titleSource'][1]),
                    'TITLE_ENG' => ($_POST['LANGUAGE_TYPE'] == 'ja'
                    ? $_POST['titleSource'][1] : $_POST['titleSource'][0]),
                    'CONTENTS_JPN' => ($_POST['LANGUAGE_TYPE'] == 'ja'
                    ? $_POST['contentSource'][0] : $_POST['contentSource'][1]),
                    'CONTENTS_ENG' => ($_POST['LANGUAGE_TYPE'] == 'ja'
                    ? $_POST['contentSource'][1] : $_POST['contentSource'][0]),
                    'LANGUAGE_TYPE' =>  ($_POST['LANGUAGE_TYPE'] == 'ja' ? 0 : 1),
                    'CORRECTION_FLAG' => (isset($_POST['tranChkBox']) ? 1 : 0),
                    'TMP_FILE_NAME1' => isset($_FILES['file1']['name'])
                    ? $_FILES['file1']['name'] : $arrOne['TMP_FILE_NAME1'],
                    'TMP_FILE_NAME2' => isset($_FILES['file2']['name'])
                    ? $_FILES['file2']['name'] : $arrOne['TMP_FILE_NAME2'],
                    'TMP_FILE_NAME3' => isset($_FILES['file3']['name'])
                    ? $_FILES['file3']['name'] : $arrOne['TMP_FILE_NAME3'],
                    'TMP_FILE_NAME4' => isset($_FILES['file4']['name'])
                    ? $_FILES['file4']['name'] : $arrOne['TMP_FILE_NAME4'],
                    'TMP_FILE_NAME5' => isset($_FILES['file5']['name'])
                    ? $_FILES['file5']['name'] : $arrOne['TMP_FILE_NAME5'],
                    'UP_USER_NO'    =>  $objUserInfo->intUserNo,
                    'UP_DATE'   => date('Y-m-d H:i:s'),
                    'PUBIC_FLAG1' => (isset($_POST['PUBIC_FLAG1']) ? 1 : 0),
                    'PUBIC_FLAG2' => (isset($_POST['PUBIC_FLAG2']) ? 1 : 0),
                    'PUBIC_FLAG3' => (isset($_POST['PUBIC_FLAG3']) ? 1 : 0),
                    'PUBIC_FLAG4' => (isset($_POST['PUBIC_FLAG4']) ? 1 : 0),
                    'PUBIC_FLAG5' => (isset($_POST['PUBIC_FLAG5']) ? 1 : 0),
                    'CONFIRM_DATE' => $_POST['CONFIRM_DATE'],
                    'CONTACT_INFO' => $_POST['CONTACT_INFO'],
                    'INFO_CATEGORY_NO' => $_POST['INFO_CATEGORY_NO'],
                    'COMPANY_NO' => isset($_POST['COMPANY_NO'])
                    ? $_POST['COMPANY_NO'] : $arrOne['COMPANY_NO'],
                ],
                't_information',
                'INFORMATION_NO=?',
                array($_POST['INFORMATION_NO']),
                SCREEN_NAME
            );

            if(is_object($objResult) && $objResult->rowCount()>0){
                //update successfully
                echo 0;


            }else{
                //update failed, write log and show error message
                fncWriteLog(
                    LogLevel['Error'],
                    LogPattern['Error'],
                    SCREEN_NAME . ' ' . $arrTextTranslate['PUBLIC_MSG_003']
                );
                echo "alert ('" .$arrTextTranslate['INFORMATION_EDIT_MSG_011']."');";
                echo "loadPortalClose();";
                echo "$('#myModal').modal('hide');";
                exit();
            }
        } else {
            //file upload failed, log error and show message
            fncWriteLog(
                LogLevel['Error'],
                LogPattern['Error'],
                SCREEN_NAME . ' ' . $arrTextTranslate['PUBLIC_MSG_003']
            );
            echo "$('.edit-error').html('".$arrTextTranslate['PUBLIC_MSG_003']."');";
            echo "$('#myModal').animate({ scrollTop: 0 }, '10');";
        }



    }else{
        //add new
        //check incident case
        $arrIncidentOne = fncSelectOne(
            "SELECT INCIDENT_CASE_NO FROM t_incident_case
            WHERE INCIDENT_CASE_NO=? AND COMP_DATE IS NULL",
            [$_POST['INCIDENT_CASE_NO']],
            SCREEN_NAME
        );
        if(!is_array($arrIncidentOne) || count($arrIncidentOne) == 0){
            //incident case not exist
            fncWriteLog(
                LogLevel['Error'],
                LogPattern['Error'],
                SCREEN_NAME.' ' . $arrTextTranslate['PUBLIC_MSG_043']
            );
            echo "alert('".$arrTextTranslate['PUBLIC_MSG_043']."');";
            echo "loadPortalClose();";
            echo "$('#myModal').modal('hide');";
            exit();
        }
        //get information_no
        $arrLastInformation = fncSelectOne(
            "SELECT NEXT VALUE FOR information_sequence as information_no",
            [],
            SCREEN_NAME
        );

        if(is_array($arrLastInformation) && count($arrLastInformation)){
            $intInformationNo = $arrLastInformation['information_no'];
        }else{
            $intInformationNo = 1;
        }

        //upload
        $blnCheck = true;
        for($intCount = 1; $intCount<=5; $intCount++){
            $strPath = SHARE_FOLDER.'/'.INFORMATION_ATTACHMENT_FOLDER
            .'/'.$intInformationNo.'/'.$intCount.'/';

            //upload file
            if(isset($_FILES['file'.$intCount])
            && $_FILES['file'.$intCount]['name'] != ''){

                if (!file_exists($strPath)) {
                    mkdir($strPath, 0777, true);
                }
                $files = glob($strPath.'*'); // get all file names
                foreach($files as $file){ // iterate files
                if(is_file($file))
                    unlink($file); // delete file
                }
                if(
                    !move_uploaded_file(
                        $_FILES["file".$intCount]["tmp_name"]
                        ,$strPath.basename($_FILES['file'.$intCount]['name'])
                    )
                ){
                    $blnCheck = false;
                    break;
                }
            }
        }
        //move file successfully, insert data
        if ($blnCheck) {


            if(!is_array($arrUserLoginTmp) || count($arrUserLoginTmp) == 0){
                //get admin_flag failed, log error and show message
                fncWriteLog(
                    LogLevel['Error'],
                    LogPattern['Error'],
                    SCREEN_NAME_EDIT . ' ' . $arrTextTranslate['PUBLIC_MSG_003']
                );
                echo "$('.edit-error').html('".$arrTextTranslate['PUBLIC_MSG_003']."');";
                echo "$('#myModal').animate({ scrollTop: 0 }, '10');";
                exit();
            }
            //insert data
            $objResult = fncProcessData(
                [
                    'INFORMATION_NO'   =>  $intInformationNo,
                    'INCIDENT_CASE_NO' => $_POST['INCIDENT_CASE_NO'],
                    'TITLE_JPN' => ($_POST['LANGUAGE_TYPE'] == 'ja'
                    ? $_POST['titleSource'][0] : $_POST['titleSource'][1]),
                    'TITLE_ENG' => ($_POST['LANGUAGE_TYPE'] == 'ja'
                    ? $_POST['titleSource'][1] : $_POST['titleSource'][0]),
                    'CONTENTS_JPN' => ($_POST['LANGUAGE_TYPE'] == 'ja'
                    ? $_POST['contentSource'][0] : $_POST['contentSource'][1]),
                    'CONTENTS_ENG' => ($_POST['LANGUAGE_TYPE'] == 'ja'
                    ? $_POST['contentSource'][1] : $_POST['contentSource'][0]),
                    'LANGUAGE_TYPE' =>  ($_POST['LANGUAGE_TYPE'] == 'ja' ? 0 : 1),
                    'CORRECTION_FLAG' => (isset($_POST['tranChkBox']) ? 1 : 0),
                    'TMP_FILE_NAME1' => $_FILES['file1']['name'] != ''
                    ? $_FILES['file1']['name'] : null,
                    'TMP_FILE_NAME2' => $_FILES['file2']['name'] != ''
                    ? $_FILES['file2']['name'] : null,
                    'TMP_FILE_NAME3' => $_FILES['file3']['name'] != ''
                    ? $_FILES['file3']['name'] : null,
                    'TMP_FILE_NAME4' => $_FILES['file4']['name'] != ''
                    ? $_FILES['file4']['name'] : null,
                    'TMP_FILE_NAME5' => $_FILES['file5']['name'] != ''
                    ? $_FILES['file5']['name'] : null,
                    'REG_USER_NO'=> $objUserInfo->intUserNo,
                    'REG_DATE'  => date('Y-m-d H:i:s'),
                    'PUBIC_FLAG1' => (isset($_POST['PUBIC_FLAG1']) ? 1 : 0),
                    'PUBIC_FLAG2' => (isset($_POST['PUBIC_FLAG2']) ? 1 : 0),
                    'PUBIC_FLAG3' => (isset($_POST['PUBIC_FLAG3']) ? 1 : 0),
                    'PUBIC_FLAG4' => (isset($_POST['PUBIC_FLAG4']) ? 1 : 0),
                    'PUBIC_FLAG5' => (isset($_POST['PUBIC_FLAG5']) ? 1 : 0),
                    'CONFIRM_DATE' => $_POST['CONFIRM_DATE'],
                    'CONTACT_INFO' => $_POST['CONTACT_INFO'],
                    'INFO_CATEGORY_NO' => $_POST['INFO_CATEGORY_NO'],
                    'COMPANY_NO' => isset($_POST['COMPANY_NO'])
                    ? $_POST['COMPANY_NO'] : $objUserInfo->intCompanyNo,
                ],
                't_information',
                '',
                [],
                SCREEN_NAME
            );
            if(is_object($objResult) && $objResult->rowCount()>0){
                //insert successfully
                echo 1;

            }else{
                //insert failed, log error and show message
                fncWriteLog(
                    LogLevel['Error'],
                    LogPattern['Error'],
                    SCREEN_NAME_CREATE . ' ' . $arrTextTranslate['PUBLIC_MSG_002']
                );
                echo "$('.edit-error').html('".$arrTextTranslate['PUBLIC_MSG_002']."');";
                echo "$('#myModal').animate({ scrollTop: 0 }, '10');";
            }
        }else{
            //upload failed, log error and show message
            fncWriteLog(
                LogLevel['Error'],
                LogPattern['Error'],
                SCREEN_NAME_CREATE . ' ' . $arrTextTranslate['PUBLIC_MSG_002']
            );
            echo "$('.edit-error').html('".$arrTextTranslate['PUBLIC_MSG_002']."');";
            echo "$('#myModal').animate({ scrollTop: 0 }, '10');";
        }
    }

}
}
/**
 * check format datetime input (Ymd Hi or Ymd)
 *
 * @create 2020/03/13 AKB Thang
 * @update
 * @param string $strDate
 * @return boolean true: , false:
 */
function checkFormatDateTimeInput2($strDate) {
    $blnFlag = true;
    $arrTmpDate = explode('/', $strDate);
    if(count($arrTmpDate) == 3) {
        $arrCheckkHasHis = explode(' ', $arrTmpDate[2]);
        if(count($arrCheckkHasHis) == 1) {
            $blnFlag = false;
        } else {
            if(!ctype_digit($arrTmpDate[0])
            || !ctype_digit($arrTmpDate[1])
            || !ctype_digit($arrCheckkHasHis[0])) {
                //y m d must be nummeric
                $blnFlag = false;
            } else {
                //$strDate must be matched format 2020/02/01 or 2020/2/1
                preg_match(
                    '/([1-9]\d{3}\/([1-9]|0[1-9]|1[0-2])\/([1-9]|0[1-9]|[12]\d|3[01]))/',
                    $strDate,
                    $arrCheckFormatDate
                );
                if(count($arrCheckFormatDate) == 0) {
                    $blnFlag = false;
                }
                $blnCheckDate = checkdate($arrTmpDate[1], $arrCheckkHasHis[0], $arrTmpDate[0]);
                if(!$blnCheckDate) {
                    //the date is not valid date
                    $blnFlag = false;
                } else {
                    $strHis = $arrCheckkHasHis[1];
                    if(trim($strHis) != '') {

                        //check hour minute format: H:i
                        preg_match(
                            '/^(([0-1]?[0-9])|([2][0-3])):([0-5]?[0-9])?$/',
                            $strHis,
                            $arrCheckHis
                        );
                        //format input is H:i:s exactly (has 1 ":")
                        if(substr_count($strHis, ':') != 1) {
                            $blnFlag = false;
                        }
                        if(count($arrCheckHis) == 0) {
                            $blnFlag = false;
                        }
                    }
                }
            }
        }
    } else {
        $blnFlag = false;
    }
    return $blnFlag;
}
/**
 * check file type
 *
 * @create 2020/04/14 AKB Thang
 * @update
 * @param string $strFileName
 * @param string $strScreenName
 * @return boolean true: , false:
 */
function fncCheckFileType($strFileName, $strScreenName){
    //get list of err extension
    $arrTmp = fncSelectData(
        "SELECT ERR_EXTENSION FROM m_err_extension",
        [],
        1,
        false,
        $strScreenName
    );
    $arrErrExtension = [];
    foreach($arrTmp as $arrItem){
        //add list of extenstion that not allowed to array
        $arrErrExtension[] = strtolower($arrItem['ERR_EXTENSION']);
    }
    if(is_array($arrErrExtension) && count($arrErrExtension) > 0){
        //check file extenstion
        $arrFileNameTmp = explode('.', $strFileName);
        //
        if(is_array($arrFileNameTmp) && count($arrFileNameTmp) >=2){

            $strFileExtension = strtolower($arrFileNameTmp[count($arrFileNameTmp)-1]);

            if(!in_array($strFileExtension, $arrErrExtension)){
                //file extension is not in the array
                return true;
            }else{
                return false;
            }
        }else{
            return false;
        }
    }else{
        return true;
    }

}