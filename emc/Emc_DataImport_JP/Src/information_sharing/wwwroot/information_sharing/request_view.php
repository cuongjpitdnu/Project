<?php
    /*
     * @request_view.php
     *
     * @create 2020/03/23 AKB Thang
     */
    require_once('common/common.php');
    header('Content-type: text/html; charset=utf-8');
    header('X-FRAME-OPTIONS: DENY');

    //DB接続
    if(fncConnectDB() == false) {
        $_SESSION['LOGIN_ERROR'] = 'DB接続に失敗しました。';
        header('Location: login.php');
        exit;
    }
    //login check
    if(!isset($_SESSION['LOGINUSER_INFO'])) {
        echo "
        <script>
            alert('".PUBLIC_MSG_008_JPN . "/" . PUBLIC_MSG_008_ENG."');
            window.location.href = 'login.php';
        </script>
        ";

        exit();
    }
    //sesssion timeout check
    fncSessionTimeOutCheck();

    $_SESSION['errStr'] = '';
    //constant
    define('SCREEN_NAME', '依頼事項詳細表示画面');
    define('SCREEN_NAME_VIEW', '依頼事項詳細表示画面　表示');
    //get object login user
    $objUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);
    //log view
    fncWriteLog(
        LogLevel['Info'],
        LogPattern['View'] ,
        SCREEN_NAME_VIEW . ' (ユーザID = '.$objUserInfo->strUserID.') '
        .(isset($_SERVER['HTTP_REFERER']) ? $_SERVER['HTTP_REFERER'] : null)
    );

    // get list text translate
    $arrTitle =  array(
        'REQUEST_VIEW_TEXT_001',
        'REQUEST_VIEW_MSG_002',
        'PUBLIC_TEXT_001',
        'PUBLIC_TEXT_002',
        'PUBLIC_TEXT_003',
        'PUBLIC_TEXT_004',
        'PUBLIC_TEXT_005',
        'PUBLIC_BUTTON_002',
        'PUBLIC_BUTTON_003',
        'PUBLIC_MSG_009',
        'REQUEST_VIEW_MSG_001',
        'REQUEST_VIEW_TEXT_002',
        'PUBLIC_TEXT_017',
        'PUBLIC_MSG_049',
        'PUBLIC_MSG_016'
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
    
    //▼2020/05/27 KBS T.Masuda Jcmgタブ権限が無いユーザはログイン画面に遷移
    if($objUserInfo->intJcmgTabPerm != 1){
        echo '<script>alert("'.$arrTextTranslate['PUBLIC_MSG_009'].'");
                      window.location.href="login.php";</script>';
        exit;
    }
    //▲2020/05/27 KBS T.Masuda


    if(isset($_POST['id'])) {
        //check request exist
        $arrOneRequest = fncSelectOne(
            "SELECT t_request.* FROM t_request
            INNER JOIN t_incident_case
            ON t_incident_case.INCIDENT_CASE_NO = t_request.INCIDENT_CASE_NO
            WHERE t_request.REQUEST_NO = ? ",
            [$_POST['id']],
            SCREEN_NAME
        );
        if(!is_array($arrOneRequest) || count($arrOneRequest) == 0) {
            //log error
            fncWriteLog(
                LogLevel['Error'],
                LogPattern['Error'],
                SCREEN_NAME . ' ' . $arrTextTranslate['REQUEST_VIEW_MSG_002']
            );
            //request not exist
?>
<script>
    alert('<?php echo $arrTextTranslate['REQUEST_VIEW_MSG_002']; ?>');
    loadPortalClose();
    $('#myModal').modal('hide');
</script>
<?php
        exit();
    }

    //get user company from request
    $arrUserLogin = fncSelectOne(
        "SELECT COMPANY_NO FROM m_user
        INNER JOIN t_request ON t_request.REG_USER_NO=m_user.USER_NO
        WHERE REQUEST_NO=?",
        [$arrOneRequest['REQUEST_NO']],
        SCREEN_NAME
    );

    //2020/04/22 T.Masuda 依頼事項の言語タイプ取得
    $intReqestLang = $arrOneRequest['LANGUAGE_TYPE'];
    //2020/04/22 T.Masuda

    if(
        ($objUserInfo->intCompanyNo == $arrUserLogin['COMPANY_NO']
        && $objUserInfo->intRequestRegPerm)
        || $objUserInfo->intMenuPerm == 1
    ) {
        //show edit button
        $blnEditRequest = true;
    } else {
        //hide edit button
        $blnEditRequest = false;
    }
?>
<div class="main-content">
    <div class="main-form">
        <div class="form-title">
            <?php echo $arrTextTranslate['REQUEST_VIEW_TEXT_001']; ?>
        </div>
        <div class="edit-error" style="color:red;"></div>
        <div class="form-body">
            <?php if(!$arrOneRequest['CORRECTION_FLAG']) { ?>
            <div class="error-messeage">
                <div><?php echo PUBLIC_TEXT_001_JPN; ?></div>
                <div><?php echo PUBLIC_TEXT_001_ENG; ?></div>
            </div>
            <?php } ?>
            <?php if($blnEditRequest) { ?>
                <div class="col-right in-lineblock right-20">
                    <button type="button" class="tbtn tbtn-defaut load-modal"
                    href="request_edit.php" data-id="<?php echo $_POST['id'];?>" id="btn-edit">
                        <?php echo fncHtmlSpecialChars($arrTextTranslate['PUBLIC_BUTTON_002']) ?>
                    </button>
                </div>
            <?php } ?>

            <div class="cont-title">
                <div class="in-line"><?php
                    echo $arrTextTranslate['REQUEST_VIEW_TEXT_002'];
                ?></div>
                <div class="in-line "><?php
                    echo date('Y/m/d H:i', strtotime($arrOneRequest['REG_DATE']));
                ?></div>
            </div>

            <br/>

            <div>
                <p style="background-color:#4169e1">
                    <legend><?php
                        echo PUBLIC_TEXT_004_JPN; ?>/<?php
                        echo PUBLIC_TEXT_004_ENG;
                    ?></legend>
                </p>
                <div class="info-left">
                    <div class="line">
                        <div class="in-line tlabel">(<?php
                            echo PUBLIC_TEXT_002_JPN ?>/<?php
                            echo PUBLIC_TEXT_002_ENG
                        ?>)</div>
                        <div class="in-line text-input text-bold"><?php
                            if($intReqestLang == 0){
                                echo fncHtmlSpecialChars($arrOneRequest['TITLE_JPN']);
                            }else{
                                echo fncHtmlSpecialChars($arrOneRequest['TITLE_ENG']);
                            }
                        ?></div>
                    </div>
                    <div class="line">
                        <div class="in-line tlabel">(<?php
                            echo PUBLIC_TEXT_003_JPN ?>/<?php
                            echo PUBLIC_TEXT_003_ENG
                        ?>)</div>
                        <div class="in-line text-input text-bold"><?php
                            if($intReqestLang == 0){
                                echo fncHtmlSpecialChars($arrOneRequest['TITLE_ENG']);
                            }else{
                                echo fncHtmlSpecialChars($arrOneRequest['TITLE_JPN']);
                            }
                        ?></div>
                    </div>
                </div>
            </div>

            <p style="background-color:#4169e1">
                <legend><?php
                    echo PUBLIC_TEXT_005_JPN; ?>/<?php
                    echo PUBLIC_TEXT_005_ENG;
                ?></legend>
            </p>

            <div class="info">
                <div class="line">
                    <div >(<?php
                        echo PUBLIC_TEXT_002_JPN; ?>/<?php
                        echo PUBLIC_TEXT_002_ENG;
                    ?>)</div>
                    <div class="text-input text-bold"><?php
                        if($intReqestLang == 0){
                            echo fncHtmlSpecialChars($arrOneRequest['CONTENTS_JPN']);
                        }else{
                            echo fncHtmlSpecialChars($arrOneRequest['CONTENTS_ENG']);
                        }
                    ?></div>
                </div>
            </div>

            <div class="info">
                <div class="line">
                    <div >(<?php
                        echo PUBLIC_TEXT_003_JPN; ?>/<?php
                        echo PUBLIC_TEXT_003_ENG;
                    ?>)</div>
                    <div class="text-input text-bold"><?php
                        if($intReqestLang == 0){
                            echo fncHtmlSpecialChars($arrOneRequest['CONTENTS_ENG']);
                        }else{
                            echo fncHtmlSpecialChars($arrOneRequest['CONTENTS_JPN']);
                        }
                    ?></div>
                </div>
            </div>

            <br/>

            <p style="background-color:#4169e1">
                <legend><?php
                    echo REQUEST_VIEW_TEXT_003_JPN; ?>/<?php
                    echo REQUEST_VIEW_TEXT_003_ENG;
                ?></legend>
            </p>

            <div class="link info">
                <?php for($intLoop = 1; $intLoop <= 5; $intLoop++) { ?>
                    <?php if(isset($arrOneRequest['TMP_FILE_NAME'.$intLoop])
                    && trim($arrOneRequest['TMP_FILE_NAME'.$intLoop]) != '') {
                        $strPathTmp = SHARE_FOLDER . '/' . REQUEST_ATTACHMENT_FOLDER . '/'
                        . $arrOneRequest['REQUEST_NO'] . '/'.$intLoop.'/'
                        . $arrOneRequest['TMP_FILE_NAME'.$intLoop];
                    ?>
                    <div>
                        <a <?php echo 'href="#"'
                        . (!file_exists(SHARE_FOLDER . '/' . REQUEST_ATTACHMENT_FOLDER
                        . '/' . $arrOneRequest['REQUEST_NO'] . '/'.$intLoop.'/'
                        . $arrOneRequest['TMP_FILE_NAME'.$intLoop])
                        ? ' class="no-file"'
                        : '  data-id="'.base64_encode($strPathTmp).'" class="download-file"')
                        ?> ><?php
                            echo $arrOneRequest['TMP_FILE_NAME'.$intLoop];
                        ?></a><?php
                            if(isset($arrOneRequest['PUBIC_FLAG'.$intLoop])) {
                                if(trim($arrOneRequest['PUBIC_FLAG'.$intLoop]) == 1) {
                        ?>
                        &nbsp;&nbsp;<font size="2" color="red"><?php
                            echo $arrTextTranslate['PUBLIC_TEXT_017'];
                        ?></font>
                        <?php
                                }
                            }
                        ?>
                    </div>
                <?php } } ?>
            </div>

            <div class="form-footer  text-right top-20">
                <button type="submit" class="tbtn-cancel tbtn-defaut"
                id="close" data-dismiss="modal">
                    <?php echo $arrTextTranslate['PUBLIC_BUTTON_003'] ?>
                </button>
            </div>
        </div>
    </div>
</div>
<style>
    legend{
        color: white;
    }
    *{
        word-break: break-all;
        -ms-word-wrap: break-word;
    }
</style>
<form action="request_view_proc.php" method="post" id="formDownload">
    <input type="hidden" name="X-CSRF-TOKEN" value="<?php
            if(isset($_SESSION['csrf'])) echo $_SESSION['csrf'];
        ?>" />
    <input type="hidden" name="path" value="">
    <input type="hidden" name="mode" value="99">
</form>
<script>
    var fileNotExistMessage = '<?php
        echo $arrTextTranslate['REQUEST_VIEW_MSG_001'] ?>';
    $('.no-file').click(function(e) {
        e.preventDefault();
        $('.edit-error').html(fileNotExistMessage);
    });
    $('#close').click(function(){
        loadPortalClose();
    });
    $('.download-file').on('click', function(e) {
        e.preventDefault();
        $('.error').html('');
        var path = $(this).attr('data-id');
        var arr = [
            { name: 'X-CSRF-TOKEN',
                value: '<?php if(isset($_SESSION['csrf'])) echo $_SESSION['csrf']; ?>'},
            { name: 'file', value: $(this).attr('data-id') },
            { name: 'action', value: 'checkFile' }
        ];
        var blnCanDownload = false;
        $.ajax({
            url: 'request_view_proc.php',
            type: 'POST',
            async: false,
            data: arr,
            success: function(result) {

                if($.trim(result) == 'window.location.href="login.php";') {
                    window.location.href="login.php";
                    return;
                }
                if(result == 0) {
                    $('span.error').html('<div><?php
                        echo $arrTextTranslate['PUBLIC_MSG_016'];
                    ?></div>');
                } else {
                    blnCanDownload = true;
                }
                if(blnCanDownload) {
                    $('input[name=path]').val(path);

                    $('form#formDownload').submit();
                }
            },
            error: function(e) {
                console.log(e);
            }
        });

        return;
    });
</script>
<?php
    } else {
        echo "
        <script>
            alert('".PUBLIC_MSG_008_JPN."/".PUBLIC_MSG_008_ENG."');
            window.location.href = 'login.php';
        </script>
        ";
        exit();
    }
?>