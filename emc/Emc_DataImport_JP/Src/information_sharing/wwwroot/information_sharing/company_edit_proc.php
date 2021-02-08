<?php
    /*
	* @company_edit_proc.php
	*
	* @create 2020/03/13 AKB Thang
	*/

    require_once('common/common.php');
    require_once('common/session_check.php');
    header('Content-type: text/html; charset=utf-8');
    header('X-FRAME-OPTIONS: DENY');
    //failed to connect database
    if(fncConnectDB() == false) {
        $_SESSION['LOGIN_ERROR'] = 'DB接続に失敗しました。';
        header('Location: login.php');
        exit;
    }
    //not login
    if(!isset($_SESSION['LOGINUSER_INFO'])) {
        echo "
        <script>
            alert('".PUBLIC_MSG_008_JPN . "/" . PUBLIC_MSG_008_ENG."');
            window.location.href = 'login.php';
        </script>
        ";
        exit();
    }
    //sesstion check
    fncSessionTimeOutCheck(1);
    //create session to store error message
    $_SESSION['COMPANY_EDIT_MSG_ERROR_INPUT'] = '';
    //constant
    define('SCREEN_NAME', '会社情報新規登録・編集画面');
    //get object user login
    $objUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);
    // get list text translate
    $arrTitle =  array(
        'COMPANY_EDIT_MSG_002',
        'COMPANY_EDIT_MSG_003',
        'COMPANY_EDIT_MSG_004',
        'COMPANY_EDIT_MSG_005',
        'COMPANY_EDIT_MSG_006',
        'COMPANY_EDIT_MSG_007',
        'COMPANY_EDIT_MSG_008',
        'COMPANY_EDIT_MSG_009',
        'COMPANY_EDIT_MSG_010',
        'COMPANY_EDIT_MSG_011',
        'COMPANY_EDIT_MSG_012',
        'COMPANY_EDIT_MSG_013',
        'PUBLIC_MSG_009',
        'PUBLIC_MSG_003',
        'PUBLIC_MSG_002',
        'COMPANY_EDIT_MSG_014',

        //2020/04/14 T.Masuda 重複チェック用のメッセージ
        'PUBLIC_MSG_001',
        'COMPANY_EDIT_MSG_015',
        'COMPANY_EDIT_MSG_016',
        'COMPANY_EDIT_MSG_017',
        'COMPANY_EDIT_MSG_018',
        //2020/04/14 T.Masuda
    );
    // get list text(header, title, msg) with languague_type of user logged
    $arrTextTranslate = getListTextTranslate(
        $arrTitle,
        $objUserInfo->intLanguageType
    );

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
    if($objUserInfo->intMenuPerm != 1) {
        echo "
        <script>
            alert('".$arrTextTranslate['PUBLIC_MSG_009']."');
            window.location.href = 'login.php';
        </script>
        ";
        exit();
    }

    //check input
    define('COMPANY_NAME_JPN_LENGTH', 20);
    define('COMPANY_NAME_ENG_LENGTH', 50);
    define('ABBREVIATIONS_JPN_LENGTH', 20);
    define('ABBREVIATIONS_ENG_LENGTH', 50);


    if(isset($_POST['company_no'])) {
        //company_no input
        if($_POST['company_no'] != ''){
            //log button edit
        	fncWriteLog(
                LogLevel['Info'],
                LogPattern['Button'],
                SCREEN_NAME . ' 更新 (ユーザID = '.$objUserInfo->strUserID.')'
            );

        }else{
            //log button register
        	fncWriteLog(
               LogLevel['Info'],
               LogPattern['Button'],
               SCREEN_NAME . ' 登録 (ユーザID = '.$objUserInfo->strUserID.')'
            );
        }
        //2020/04/14 T.Mausda 重複チェック追加
        $arrCompanyNo = [];

        $strDupliSql = 'SELECT COMPANY_NAME_JPN,
                               COMPANY_NAME_ENG,
                               ABBREVIATIONS_JPN,
                               ABBREVIATIONS_ENG
                        FROM m_company ';

        //編集時
        if($_POST['company_no'] != ''){
            $strDupliSql .= 'WHERE NOT company_no = ?';
            $arrCompanyNo[] = $_POST['company_no'];
        }

        $arrAllCompany = fncSelectData($strDupliSql, $arrCompanyNo, 1, false, SCREEN_NAME);

        //重複チェック用データ取得に失敗時
        if($arrAllCompany == 0){
            fncWriteLog(
                LogLevel['Error'],
                LogPattern['Error'],
                SCREEN_NAME .' ' . $arrTextTranslate['PUBLIC_MSG_001']
                );
            $_SESSION['COMPANY_EDIT_MSG_ERROR_INPUT']
            .= $arrTextTranslate['PUBLIC_MSG_001'].'<br>';
        }else{
        //2020/04/14 T.Mausda

            if(!isset($_POST['company_name_jpn'])
                || mb_strlen($_POST['company_name_jpn']) == 0) {
                $_SESSION['COMPANY_EDIT_MSG_ERROR_INPUT']
                .= $arrTextTranslate['COMPANY_EDIT_MSG_002'].'<br>';
            } else {
                //　文字数 > 20
                if(mb_strlen($_POST['company_name_jpn'])>COMPANY_NAME_JPN_LENGTH) {
                    $_SESSION['COMPANY_EDIT_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['COMPANY_EDIT_MSG_003'].'<br>';
                }
            }

            if(!isset($_POST['company_name_eng'])
                || mb_strlen($_POST['company_name_eng']) == 0) {
                $_SESSION['COMPANY_EDIT_MSG_ERROR_INPUT']
                .= $arrTextTranslate['COMPANY_EDIT_MSG_004'].'<br>';
            } else {
                //　文字数 > 50
                if(mb_strlen($_POST['company_name_eng'])>COMPANY_NAME_ENG_LENGTH) {
                    $_SESSION['COMPANY_EDIT_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['COMPANY_EDIT_MSG_005'].'<br>';
                }

                //　「半角英数字または特殊文字」以外
                if(!fncCheckEngText($_POST['company_name_eng'])) {
                    $_SESSION['COMPANY_EDIT_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['COMPANY_EDIT_MSG_006'].'<br>';
                }
            }

            if(!isset($_POST['abbreviations_jpn'])
                || mb_strlen($_POST['abbreviations_jpn']) == 0){
                $_SESSION['COMPANY_EDIT_MSG_ERROR_INPUT']
                .= $arrTextTranslate['COMPANY_EDIT_MSG_007'].'<br>';
            } else {
                //　文字数 >  4
                if(mb_strlen($_POST['abbreviations_jpn'])>ABBREVIATIONS_JPN_LENGTH) {
                    $_SESSION['COMPANY_EDIT_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['COMPANY_EDIT_MSG_008'].'<br>';
                }
            }

            if(!isset($_POST['abbreviations_eng'])
                || mb_strlen($_POST['abbreviations_eng']) == 0) {
                $_SESSION['COMPANY_EDIT_MSG_ERROR_INPUT']
                .= $arrTextTranslate['COMPANY_EDIT_MSG_009'].'<br>';
            } else {
                //　文字数 >  4
                if(mb_strlen($_POST['abbreviations_eng'])>ABBREVIATIONS_ENG_LENGTH) {
                    $_SESSION['COMPANY_EDIT_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['COMPANY_EDIT_MSG_010'].'<br>';
                }
                //　「半角英数字または特殊文字」以外
                if(!fncCheckEngText($_POST['abbreviations_eng'])) {
                    $_SESSION['COMPANY_EDIT_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['COMPANY_EDIT_MSG_011'].'<br>';
                }
            }
            //　inst categoryが存在しない場合
            if(!isset($_POST['inst_category_no']) || $_POST['inst_category_no'] == '') {
                $_SESSION['COMPANY_EDIT_MSG_ERROR_INPUT']
                .= $arrTextTranslate['COMPANY_EDIT_MSG_012'].'<br>';
            }

            if(!isset($_POST['group_no']) || $_POST['group_no'] == '') {
                $_SESSION['COMPANY_EDIT_MSG_ERROR_INPUT']
                .= $arrTextTranslate['COMPANY_EDIT_MSG_013'].'<br>';
            }



            //2020/04/14 T.Masuda 重複チェック
            foreach ($arrAllCompany as $company){
                //会社名（日本語）の重複チェック
                if(mb_strtolower($_POST['company_name_jpn'])
                    == mb_strtolower($company['COMPANY_NAME_JPN'])){
                    $_SESSION['COMPANY_EDIT_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['COMPANY_EDIT_MSG_015'].'<br>';
                }
                //会社名（英語）の重複チェック
                if(mb_strtolower($_POST['company_name_eng'])
                    == mb_strtolower($company['COMPANY_NAME_ENG'])){
                    $_SESSION['COMPANY_EDIT_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['COMPANY_EDIT_MSG_016'].'<br>';
                }
                //略称名（日本語）の重複チェック
                if(mb_strtolower($_POST['abbreviations_jpn'])
                    == mb_strtolower($company['ABBREVIATIONS_JPN'])){
                    $_SESSION['COMPANY_EDIT_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['COMPANY_EDIT_MSG_017'].'<br>';
                }
                //略称名（英語）の重複チェック
                if(mb_strtolower($_POST['abbreviations_eng'])
                    == mb_strtolower($company['ABBREVIATIONS_ENG'])){
                    $_SESSION['COMPANY_EDIT_MSG_ERROR_INPUT']
                    .= $arrTextTranslate['COMPANY_EDIT_MSG_018'].'<br>';
                }
            }
        }
            //2020/04/14 T.Masuda

    } else {
        $_SESSION['COMPANY_EDIT_MSG_ERROR_INPUT']
        .= $arrTextTranslate['COMPANY_EDIT_MSG_014'];
    }
    if($_SESSION['COMPANY_EDIT_MSG_ERROR_INPUT'] != '') {
    //show error message
?>
$('.edit-error').html('<?php echo $_SESSION['COMPANY_EDIT_MSG_ERROR_INPUT']; ?>');
<?php
        unset($_SESSION['COMPANY_EDIT_MSG_ERROR_INPUT']);
    } else {
        //process
        try {
            $arrInstCategory = fncSelectOne("SELECT INST_CATEGORY_NO
                FROM m_inst_category
                WHERE INST_CATEGORY_NO=?",
                [$_POST['inst_category_no']], SCREEN_NAME);
            $arrGroup = fncSelectOne("SELECT GROUP_NO
                FROM m_group
                WHERE GROUP_NO=?",
                [$_POST['group_no']], SCREEN_NAME);

            if($_POST['company_no'] != '') {
                //update company
                if(!$arrInstCategory || !$arrGroup) {
                    // inst category又はgroupデータが存在しない場合、ログを出力し、エラーメッセージを表示する
                    fncWriteLog(
                        LogLevel['Error'],
                        LogPattern['Error'],
                        $arrTextTranslate['PUBLIC_MSG_003']
                    );
                    echo "$('.edit-error').html('".$arrTextTranslate['PUBLIC_MSG_003']."');";
                    exit();
                }
                $objResult = fncProcessData(
                    [
                        'COMPANY_NO' => $_POST['company_no'],
                        'COMPANY_NAME_JPN'  => $_POST['company_name_jpn'],
                        'COMPANY_NAME_ENG'  =>  $_POST['company_name_eng'],
                        'ABBREVIATIONS_JPN'  => $_POST['abbreviations_jpn'],
                        'ABBREVIATIONS_ENG'  => $_POST['abbreviations_eng'],
                        'ABBREVIATIONS_ENG'  => $_POST['abbreviations_eng'],
                        'INST_CATEGORY_NO'  => $_POST['inst_category_no'],
                        'GROUP_NO'  => $_POST['group_no'],
                        'UP_USER_NO'    =>  $objUserInfo->intUserNo,
                        'UP_DATE'   => date('Y-m-d H:i:s')
                    ],
                    'm_company',
                    'COMPANY_NO=? AND DEL_FLAG=?',
                    array($_POST['company_no'], 0),
                    SCREEN_NAME
                );
                if(is_object($objResult) && $objResult->rowCount()>0){
                    //　会社情報を保存する処理にエラーがない場合、ログ出力する
                    echo 0;


                } else {
                    //　会社情報を保存する処理にエラーがある場合
                    fncWriteLog(
                        LogLevel['Error'],
                        LogPattern['Error'],
                        SCREEN_NAME .$arrTextTranslate['PUBLIC_MSG_003']
                    );
                    echo "alert('".$arrTextTranslate['COMPANY_EDIT_MSG_014']."'); ";
                    echo "$('#myModal').modal('hide'); ";
                    echo "setTimeout(function() {
                                window.location.reload();
                            }, 300);";
                }
            } else {

                //　新規登録処理
                if(!$arrInstCategory || !$arrGroup) {
                    //　inst category又はgroupデータが存在しない場合、ログを出力し、エラーメッセージを表示する
                    fncWriteLog(
                        LogLevel['Error'],
                        LogPattern['Error'],
                        SCREEN_NAME . $arrTextTranslate['PUBLIC_MSG_002']
                    );
                    echo "$('.edit-error').html('".$arrTextTranslate['PUBLIC_MSG_002']."');";
                    exit();
                }
                //get int company no
                $objLastCompany = fncSelectOne(
                    "SELECT COMPANY_NO FROM m_company ORDER BY COMPANY_NO DESC",
                    [0],
                    SCREEN_NAME
                );
                if($objLastCompany) {
                    $intCompanyNo = $objLastCompany['COMPANY_NO'] + 1;
                } else {
                    $intCompanyNo = 1;
                }
                //get int sort no
                $objLastSortCompany = fncSelectOne(
                    "SELECT SORT_NO FROM m_company WHERE DEL_FLAG=? ORDER BY SORT_NO DESC",
                    [0],
                    SCREEN_NAME
                );
                if($objLastSortCompany){
                    $intSortNo = $objLastSortCompany['SORT_NO'] + 1;
                }else{
                    $intSortNo = 1;
                }

                $objResult = fncProcessData([
                    'COMPANY_NO' => $intCompanyNo,
                    'COMPANY_NAME_JPN'  => $_POST['company_name_jpn'],
                    'COMPANY_NAME_ENG'  =>  $_POST['company_name_eng'],
                    'ABBREVIATIONS_JPN'  => $_POST['abbreviations_jpn'],
                    'ABBREVIATIONS_ENG'  => $_POST['abbreviations_eng'],
                    'ABBREVIATIONS_ENG'  => $_POST['abbreviations_eng'],
                    'INST_CATEGORY_NO'  => $_POST['inst_category_no'],
                    'GROUP_NO'  => $_POST['group_no'],
                    'SORT_NO'   =>  $intSortNo,
                    'DEL_FLAG'  =>  0,
                    'REG_USER_NO'=> $objUserInfo->intUserNo,
                    'REG_DATE'  => date('Y-m-d H:i:s')
                ], 'm_company', '', array(), SCREEN_NAME);
                if(is_object($objResult) && $objResult->rowCount()>0){
                    //　会社情報を保存する処理にエラーがある場合
                    echo 1;


                } else {
                    // echo $objResult;
                    //　会社情報を保存する処理にエラーがある場合
                    fncWriteLog(
                        LogLevel['Error'],
                        LogPattern['Error'],
                        SCREEN_NAME . $arrTextTranslate['PUBLIC_MSG_002']
                    );
                    echo "$('.edit-error').html('".$arrTextTranslate['PUBLIC_MSG_002']."');";
                }
            }
        } catch(\Exception $e) {
            fncWriteLog(
                LogLevel['Error'],
                LogPattern['Error'],
                $e->getMessage()
            );
            echo $e->getMessage();
        }
    }
?>