<?php
    /*
    * @request_edit.php
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
 * @param string $strFileNameCharError
 * @return
 */

$intTotalFileSize = 0;
define('FILE_NAME_LENGTH', 100);
define('TOTAL_FILE_SIZE', 10485760);
function fncValidateFile(
    $intFileNo, $strFileNotExist, $strFileNameError,
    $strFileNameTypeError, $strFileNameCharError=''
){
    if(isset($_FILES['file'.$intFileNo])&&$_FILES['file'.$intFileNo]['name'] != ''){
        //check file upload exist
        if(
            !isset($_FILES['file'.$intFileNo]['error'])
            || is_array($_FILES['file'.$intFileNo]['error'])
            || $_FILES['file'.$intFileNo]['size'] == 0
        ) {
            $_SESSION['REQUEST_EDIT_MSG_ERROR_INPUT'] .= $strFileNotExist .'<br>';
        }else{
            if(!file_exists($_FILES['file'.$intFileNo]['tmp_name'])){
                $_SESSION['REQUEST_EDIT_MSG_ERROR_INPUT'] .= $strFileNotExist .'<br>';
            }else{
                //check file length
                if(mb_strwidth($_FILES['file'.$intFileNo]['name']) > FILE_NAME_LENGTH ){
                    $_SESSION['REQUEST_EDIT_MSG_ERROR_INPUT'] .= $strFileNameError .'<br>';
                }
                //check file type
                if(!fncCheckFileType($_FILES['file'.$intFileNo]['name'], SCREEN_NAME)){
                    $_SESSION['REQUEST_EDIT_MSG_ERROR_INPUT'] .= $strFileNameTypeError .'<br>';
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
                    $_SESSION['REQUEST_EDIT_MSG_ERROR_INPUT'] .= $strFileNameCharError .'<br>';
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
$_SESSION['REQUEST_EDIT_MSG_ERROR_INPUT'] = '';
//constant
define('SCREEN_NAME', '依頼事項登録編集画面');
define('SCREEN_NAME_CREATE', '依頼事項登録編集画面 登録');
define('SCREEN_NAME_EDIT', '依頼事項登録編集画面 更新');
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
    'PUBLIC_MSG_001',
    'PUBLIC_MSG_009',
    'PUBLIC_MSG_004',
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
    'PUBLIC_MSG_019',
    'REQUEST_EDIT_MSG_001',
    'PUBLIC_MSG_051',

    //2020/04/27 T.Masuda 既に削除された依頼事項
    'REQUEST_EDIT_MSG_004',
    //2020/04/27 T.Masuda

);
// get list text(header, title, msg) with languague_type of user logged
$arrTextTranslate = getListTextTranslate(
    $arrTitle,
    $objUserInfo->intLanguageType
);


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
if($objUserInfo->intRequestRegPerm != 1 && $objUserInfo->intMenuPerm != 1){
    echo 900;
    exit();
}

$_SESSION['REQUEST_EDIT_ERROR'] = array();
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
define('LENGTH_TITLE_JPN_TO_ENG', 30);
define('LENGTH_TITLE_ENG_TO_JPN', 150);
define('LENGTH_CONTENT_JPN_TO_ENG', 1000);
define('LENGTH_CONTENT_ENG_TO_JPN', 5000);

//btn translate
if(isset($_POST['mode']) && $_POST['mode'] == 2){

    //2020/04/25 T.Masuda ボタンクリック時にログ出力
    if(isset($_POST['save'])){
        if($_POST['REQUEST_NO']!=''){
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
    //2020/04/25 T.Masuda

    $strErrMsg = '';

    //2020/04/25 T.Masuda 確認メッセージの前に入力チェック
    $intTranslate = $_POST['LANGUAGE_TYPE'] == 'ja' ? 0 : 1;

    if(!isset($_POST['save'])){
        //入力チェック
        $_SESSION['REQUEST_EDIT_ERROR']
            = array_merge($_SESSION['REQUEST_EDIT_ERROR'],
                fncTranInputCheck($_POST['title'], $_POST['content'],
                                  '','',$intTranslate,0,0,
                                  $arrTextTranslate));
    }else{
        //入力チェック
        $_SESSION['REQUEST_EDIT_ERROR']
            = array_merge($_SESSION['REQUEST_EDIT_ERROR'],
                fncTranInputCheck($_POST['title'], $_POST['content'],
                                  $_POST['title1'],$_POST['content1'],
                                  $intTranslate,$_POST['manualTranslate'],1,
                                  $arrTextTranslate));
    }

    if(count($_SESSION['REQUEST_EDIT_ERROR']) > 0) {
        foreach($_SESSION['REQUEST_EDIT_ERROR'] as $strError) {
            $strErrMsg .= $strError.'<br>';
        }
    }
    //2020/04/25 T.Masuda

    if(isset($strErrMsg) && $strErrMsg != ''){
        //show translation error message
        ?>
            $('.edit-error').html('<?php echo $strErrMsg; ?>');
            $('#myModal').animate({ scrollTop: 0 }, '10');
        <?php
    }else{
        //translation
        if($_POST['title1'] == '' && $_POST['content1'] == ''){
            //
            if($_POST['LANGUAGE_TYPE'] == 'en'){
                //english -> japansese
                $strTitleTranslated = is_array(tranAmazon($_POST['title'], 1))
                ? '' : tranAmazon($_POST['title'], 1);
                $strContentTranslated = is_array(tranAmazon($_POST['content'], 1))
                ? '' : tranAmazon($_POST['content'], 1) ;
            }else{
                //japanese -> english
                $strTitleTranslated = is_array(tranAmazon($_POST['title'], 0))
                ? '' : tranAmazon($_POST['title'], 0);
                $strContentTranslated = is_array(tranAmazon($_POST['content'], 0))
                ? '' : tranAmazon($_POST['content'], 0) ;
            }
            if(!($strTitleTranslated == '' ||  $strContentTranslated == '')){
                //return translation value
                echo json_encode([$strTitleTranslated, $strContentTranslated]);
            }else{
                //error translation
                if(!isset($_POST['save'])){
                    //show amazon translation errror warning
            ?>
                alert('<?php echo $arrTextTranslate['PUBLIC_MSG_010']; ?>');
            <?php
                }else{
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



//check input
if(isset($_POST['mode']) && $_POST['mode'] == 1){
if(isset($_POST['REQUEST_NO'])){
    if($_POST['REQUEST_NO']!=''){
        //get request information
        $arrOne = fncSelectOne(
            "SELECT * FROM t_request WHERE REQUEST_NO=?",
            [$_POST['REQUEST_NO']],
            SCREEN_NAME
        );
    }

    if(isset($arrOne) && (!is_array($arrOne) || count($arrOne)==0)){
        //2020/04/27 T.Masuda 既に削除された依頼事項の場合
        fncWriteLog(
        LogLevel['Error'],
        LogPattern['Error'],
        SCREEN_NAME . ' ' . $arrTextTranslate['PUBLIC_MSG_003']
        );
        echo "alert ('" .$arrTextTranslate['REQUEST_EDIT_MSG_004']."');";
        echo "loadPortalClose();";
        echo "$('#myModal').modal('hide');";
        exit();
        //2020/04/27 T.Masuda
    }else{
        //check content, title source
        $strTitle = isset($_POST['titleSource'])
        ? $_POST['titleSource'][0] : '';
        $strContent = isset($_POST['contentSource'])
        ? $_POST['contentSource'][0] : '';


        //check file 1-> 5
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
            //update file, check total file size on server
            for($i = 1; $i <= 5; $i++) {
                if(isset($arrOne['TMP_FILE_NAME'.$i])
                    && $arrOne['TMP_FILE_NAME'.$i] != '') {
                    if(!isset($_POST['chkDelete'.$i])) {
                        //add to total file size if the file is not deleted
                        $strFilePath = SHARE_FOLDER.'\\'.REQUEST_ATTACHMENT_FOLDER;
                        $strFilePath .= '\\'.$arrOne['REQUEST_NO']
                        .'\\'.$i.'\\'.$arrOne['TMP_FILE_NAME'.$i];
                        if(is_file($strFilePath)){
                            $intTotalFileSize += filesize($strFilePath);
                        }
                        $arrFileName[] = $arrOne['TMP_FILE_NAME'.$i];
                    }
                }
            }
        }
        //check total file size
        if($intTotalFileSize > TOTAL_FILE_SIZE){
            $_SESSION['REQUEST_EDIT_MSG_ERROR_INPUT']
            .= $arrTextTranslate['PUBLIC_MSG_019'].'<br>';
        }
        //check file name
        for($intCount = 1; $intCount <= 5; $intCount++){
            if(isset($_FILES['file'.$intCount]['name'])
            && $_FILES['file'.$intCount]['name']!=''){
                $arrFileName[] = $_FILES['file'.$intCount]['name'];
            }
        }
        //check dupliate file name
        if(fncDupArr($arrFileName)){
            $_SESSION['REQUEST_EDIT_MSG_ERROR_INPUT']
            .= $arrTextTranslate['PUBLIC_MSG_051'].'<br>';
        }
    }
}else{

}


if($_SESSION['REQUEST_EDIT_MSG_ERROR_INPUT'] != ''){
    //show error message
?>

$('.edit-error').html('<?php
echo $_SESSION['REQUEST_EDIT_MSG_ERROR_INPUT']; ?>');
$('#myModal').animate({ scrollTop: 0 }, '10');

<?php
}else{


    //process update
    if(isset($_POST['errFile'])){
        exit();
    }
    //get user list to send mail
    $arrUser = getDataMUserSendMail('REQUEST_CONTENTS_MAIL', SCREEN_NAME);
    if($arrUser == 0) {
        //can not get users to send mail, show error(stop insert or update)
        echo "$('.edit-error').html('".$arrTextTranslate['PUBLIC_MSG_001']."');";
        echo "$('#myModal').animate({ scrollTop: 0 }, '10');";
        fncWriteLog(
            LogLevel['Error'],
            LogPattern['Error'],
            SCREEN_NAME . ' ' . $arrTextTranslate['PUBLIC_MSG_001']
        );
        exit();
    }

    if($_POST['REQUEST_NO']!=''){
        //check incident case
        $arrIncidentOne = fncSelectOne(
            "SELECT INCIDENT_CASE_NO FROM t_incident_case
            WHERE INCIDENT_CASE_NO=?",
            [$arrOne['INCIDENT_CASE_NO']],
            SCREEN_NAME
        );
        if(!is_array($arrIncidentOne) || count($arrIncidentOne) == 0){
            //incident case not exist, show error
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
                $strPath = SHARE_FOLDER.'/'.REQUEST_ATTACHMENT_FOLDER
                .'/'.$arrOne['REQUEST_NO'].'/'.$intCount.'/';
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
            if(isset($_FILES['file'.$intCount])
            && $_FILES['file'.$intCount]['name'] != ''){
                $strPath = SHARE_FOLDER.'/'.REQUEST_ATTACHMENT_FOLDER
                .'/'.$arrOne['REQUEST_NO'].'/'.$intCount.'/';
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

        if($blnCheck){
            //call function to update request
            $objResult = fncProcessData(
                [
                    'REQUEST_NO' => $_POST['REQUEST_NO'],
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
                ],
                't_request',
                'REQUEST_NO=?',
                array($_POST['REQUEST_NO']),
                SCREEN_NAME
            );

            if(is_object($objResult) && $objResult->rowCount()>0){
                //update successfully
                $blnType = 0;
            }else{
                //update failed
                fncWriteLog(
                    LogLevel['Error'],
                    LogPattern['Error'],
                    SCREEN_NAME_EDIT . ' ' . $arrTextTranslate['PUBLIC_MSG_003']
                );
                //2020/04/27 T.Masuda 既に削除された依頼事項の場合
                //echo "$('.edit-error').html('".$arrTextTranslate['PUBLIC_MSG_003']."');";
                echo "alert '" .$arrTextTranslate['REQUEST_EDIT_MSG_004']."';";
                echo "loadPortalClose();";
                echo "$('#myModal').modal('hide');";
                //2020/04/27 T.Masuda
                exit();
            }
        } else {
            //upload file failed
            fncWriteLog(
                LogLevel['Error'],
                LogPattern['Error'],
                SCREEN_NAME_EDIT . ' ' . $arrTextTranslate['PUBLIC_MSG_003']
            );
            echo "$('.edit-error').html('".$arrTextTranslate['PUBLIC_MSG_003']."');";
            echo "$('#myModal').animate({ scrollTop: 0 }, '10');";
            exit();
        }
    }else{
        //add new
        $arrLastRequest = fncSelectOne(
            "SELECT NEXT VALUE FOR request_sequence as request_no",
            [],
            SCREEN_NAME
        );
        //get request_no
        if(is_array($arrLastRequest) && count($arrLastRequest)){
            $intRequestNo = $arrLastRequest['request_no'];
        }else{
            $intRequestNo = 1;
        }
        //upload
        $blnCheck = true;
        for($intCount = 1; $intCount<=5; $intCount++){
            //file 1-> 5
            if(isset($_FILES['file'.$intCount])
            && $_FILES['file'.$intCount]['name'] != ''){
                $strPath = SHARE_FOLDER.'/'.REQUEST_ATTACHMENT_FOLDER
                .'/'.$intRequestNo.'/'.$intCount.'/';
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
        //move file successfully, create
        if ($blnCheck) {
            //check incident case
            $arrIncidentOne = fncSelectOne(
                "SELECT INCIDENT_CASE_NO FROM t_incident_case
                WHERE INCIDENT_CASE_NO=?",
                [$_POST['INCIDENT_CASE_NO']],
                SCREEN_NAME
            );
            if(!is_array($arrIncidentOne) || count($arrIncidentOne) == 0){
                //incident case not exist, write log and show alert message
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
            //insert data
            $objResult = fncProcessData(
                [
                    'REQUEST_NO'   =>  $intRequestNo,
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
                ],
                't_request',
                '',
                [],
                SCREEN_NAME
            );
            if(is_object($objResult) && $objResult->rowCount()>0){
                //insert successfully
                $blnType = 1;

            }else{
                //insert not sucessfully, write log
                fncWriteLog(
                    LogLevel['Error'],
                    LogPattern['Error'],
                    SCREEN_NAME . ' ' . $arrTextTranslate['PUBLIC_MSG_002']
                );
                echo "$('.edit-error').html('".$arrTextTranslate['PUBLIC_MSG_002']."');";
                echo "$('#myModal').animate({ scrollTop: 0 }, '10');";
                exit();
            }
        }else{
            //upload failed, write log
            fncWriteLog(
                LogLevel['Error'],
                LogPattern['Error'],
                SCREEN_NAME . ' ' . $arrTextTranslate['PUBLIC_MSG_002']
            );
            echo "$('.edit-error').html('".$arrTextTranslate['PUBLIC_MSG_002']."');";
            echo "$('#myModal').animate({ scrollTop: 0 }, '10');";
            exit();
        }
    }


    // send mail

    if($arrUser == 0) {
        echo "$('.edit-error').html('".$arrTextTranslate['PUBLIC_MSG_001']."');";
        echo "$('#myModal').animate({ scrollTop: 0 }, '10');";
        fncWriteLog(
            LogLevel['Error'],
            LogPattern['Error'],
            SCREEN_NAME . ' ' . $arrTextTranslate['PUBLIC_MSG_001']
        );
    } else {
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
	            if($user['language_type'] == 0) {
	                if(count($arrTempMailJP) == 0) {
	                    array_push($arrTempMailJP, $user['mail_address']);
	                    $arrMail['jpn'][] = array(
	                        'USER_NAME' => $user['user_name'],
	                        'MAIL_ADDRESS' => $user['mail_address'],
	                    );
	                } else {
	                    if(!in_array($user['mail_address'], $arrTempMailJP)) {
	                        array_push($arrTempMailJP, $user['mail_address']);
	                        $arrMail['jpn'][] = array(
	                            'USER_NAME' => $user['user_name'],
	                            'MAIL_ADDRESS' => $user['mail_address'],
	                        );
	                    }
	                }
	            } else {
	                if(count($arrTempMailEN) == 0) {
	                    array_push($arrTempMailEN, $user['mail_address']);
	                    $arrMail['eng'][] = array(
	                        'USER_NAME' => $user['user_name'],
	                        'MAIL_ADDRESS' => $user['mail_address'],
	                    );
	                } else {
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
	            if($user['language_type'] == 0) {
	                if(count($arrTempExtMailJP) == 0) {
	                    array_push($arrTempExtMailJP, $user['mail_address']);
	                    $arrMail['jpn_ext'][] = array(
	                        'USER_NAME' => $user['user_name'],
	                        'MAIL_ADDRESS' => $user['mail_address'],
	                    );
	                } else {
	                    if(!in_array($user['mail_address'], $arrTempExtMailJP)) {
	                        array_push($arrTempExtMailJP, $user['mail_address']);
	                        $arrMail['jpn_ext'][] = array(
	                            'USER_NAME' => $user['user_name'],
	                            'MAIL_ADDRESS' => $user['mail_address'],
	                        );
	                    }
	                }
	            } else {
	                if(count($arrTempExtMailEN) == 0) {
	                    array_push($arrTempExtMailEN, $user['mail_address']);
	                    $arrMail['eng_ext'][] = array(
	                        'USER_NAME' => $user['user_name'],
	                        'MAIL_ADDRESS' => $user['mail_address'],
	                    );
	                } else {
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

        if(count($arrMail['eng']) > 0) {
            $arrMailSend['eng'] = array_chunk($arrMail['eng'], MAIL_SUBMIT_NUMBER);
        }
        
        //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
        if(count($arrMail['jpn_ext']) > 0) {
            $arrMailSend['jpn_ext'] = array_chunk($arrMail['jpn_ext'], MAIL_SUBMIT_NUMBER);
        }

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
            'jpn' => is_file('common/mail_temp_jpn.txt')
            ? nl2br(file_get_contents("common/mail_temp_jpn.txt")) : '',
            'eng' => is_file('common/mail_temp_eng.txt')
            ? nl2br(file_get_contents("common/mail_temp_eng.txt")) : '',
            'jpn_ext' => is_file('common/mail_temp_ext_jpn.txt')
            ? nl2br(file_get_contents("common/mail_temp_ext_jpn.txt")) : '',
            'eng_ext' => is_file('common/mail_temp_ext_eng.txt')
            ? nl2br(file_get_contents("common/mail_temp_ext_eng.txt")) : '',
        );
        //▲2020/06/11 KBS S.Tasaki

        if($arrBody['jpn'] != '') {
            $arrBody['jpn'] = str_replace ('%0%', date('m月d日H時i分'), $arrBody['jpn']);
            $arrBody['jpn'] = str_replace (
                '%1%',
                REQUEST_EDIT_TEXT_003_JPN,
                $arrBody['jpn']
            );
        }

        if($arrBody['eng'] != '') {
            $arrBody['eng'] = str_replace ('%0%', date('H:i, d M'), $arrBody['eng']);
            $arrBody['eng'] = str_replace (
                '%1%',
                REQUEST_EDIT_TEXT_003_ENG,
                $arrBody['eng']
            );
        }
        
        //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
        if($arrBody['jpn_ext'] != '') {
            $arrBody['jpn_ext'] = str_replace ('%0%', date('m月d日H時i分'), $arrBody['jpn_ext']);
            $arrBody['jpn_ext'] = str_replace (
                '%1%',
                REQUEST_EDIT_TEXT_003_JPN,
                $arrBody['jpn_ext']
            );
        }

        if($arrBody['eng_ext'] != '') {
            $arrBody['eng_ext'] = str_replace ('%0%', date('H:i, d M'), $arrBody['eng_ext']);
            $arrBody['eng_ext'] = str_replace (
                '%1%',
                REQUEST_EDIT_TEXT_003_ENG,
                $arrBody['eng_ext']
            );
        }
        //▲2020/06/11 KBS S.Tasaki

        $blnFlagHasBccMailJPN = false;
        $blnFlagHasBccMailENG = false;
        //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
        $blnFlagHasBccExtMailJPN = false;
        $blnFlagHasBccExtMailENG = false;
        //▲2020/06/11 KBS S.Tasaki

        if(count($arrMailSend['jpn']) > 0) {
            $blnFlagHasBccMailJPN = true;
        }

        if(count($arrMailSend['eng']) > 0) {
            $blnFlagHasBccMailENG = true;
        }
        
        //▼2020/06/11 KBS S.Tasaki 内部・外部の区分けを行うよう修正
        if(count($arrMailSend['jpn_ext']) > 0) {
            $blnFlagHasBccExtMailJPN = true;
        }

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
                
                if(count($arrListMail) > 0) {
                    foreach($arrListMail as $group) {
                        fncSendMail($group, $strSubjectSend, $strBodySend, '');
                    }
                }
            }
        }
        //▲2020/06/11 KBS S.Tasaki
        
        echo $blnType;
    }
}
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
        $arrErrExtension[] = strtolower($arrItem['ERR_EXTENSION']);
    }
    if(is_array($arrErrExtension) && count($arrErrExtension) > 0){
        //check file extenstion
        $arrFileNameTmp = explode('.', $strFileName);
        if(is_array($arrFileNameTmp) && count($arrFileNameTmp) >=2){
            $strFileExtension = strtolower($arrFileNameTmp[count($arrFileNameTmp)-1]);

            if(!in_array($strFileExtension, $arrErrExtension)){
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
