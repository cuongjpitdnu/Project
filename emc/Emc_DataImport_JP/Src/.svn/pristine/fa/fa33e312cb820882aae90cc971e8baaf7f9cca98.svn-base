<?php
    /*
     * @user_setting.php
     *
     * @create 2020/03/13 AKB Chien
     * @update
     */
    require_once('common/common.php');
    require_once('common/session_check.php');

    header('Content-type: text/html; charset=utf-8');
    header('X-FRAME-OPTIONS: DENY');

    define('DISPLAY_TITLE', 'ユーザ設定画面');

    // check connection
    if(fncConnectDB() == false) {
        $_SESSION['LOGIN_ERROR'] = 'DB接続に失敗しました。';
        header('Location: login.php');
        exit;
    }

    // Check if the user logged in or not
    if(!isset($_SESSION['LOGINUSER_INFO'])) {
        $strShow = '<script>alert("'.PUBLIC_MSG_008_JPN.' / '.PUBLIC_MSG_008_ENG.'");';
        $strShow .= ' window.location.href="login.php";</script> ';
        echo $strShow;
        exit;
    }

    // check timeout if direct access this file
    fncSessionTimeOutCheck();

    // ログインユーザ情報を取得
    $objLoginUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);

    $intLanguageType = $objLoginUserInfo->intLanguageType;
    $intUserNo = $objLoginUserInfo->intUserNo;

    $arrTitleMsg =  array(
        'USER_SETTING_TEXT_001',
        'USER_SETTING_TEXT_002',
        'USER_SETTING_TEXT_003',
        'USER_SETTING_TEXT_004',
        'USER_SETTING_TEXT_005',
        'USER_SETTING_TEXT_006',
        'USER_SETTING_TEXT_007',
        'USER_SETTING_TEXT_008',
        'USER_SETTING_TEXT_009',
        'USER_SETTING_TEXT_010',
        'USER_SETTING_TEXT_011',
        'USER_SETTING_TEXT_012',
        'PUBLIC_BUTTON_014',
        'PUBLIC_BUTTON_003',
        'USER_SETTING_MSG_001',

        // 2020/03/30 AKB Chien - start - update document 2020/03/30
        'PUBLIC_MSG_049'
        // 2020/03/30 AKB Chien - end - update document 2020/03/30
    );

    // write log when access this screen
    $strLog = DISPLAY_TITLE.'　画面表示(ユーザID = '.$objLoginUserInfo->strUserID.') ';
    $strLog .= isset($_SERVER['HTTP_REFERER']) ? $_SERVER['HTTP_REFERER'] : null;
    fncWriteLog(LogLevel['Info'], LogPattern['View'], $strLog);

    // get list text(header, title, msg) with languague_type of user logged
    $arrTextTranslate = getListTextTranslate($arrTitleMsg, $intLanguageType);

    // 2020/03/30 AKB Chien - start - update document 2020/03/30
    // GET通信にて遷移してきた場合、以下のメッセージをアラート表示し、遷移元画面に戻す。
    fncGetRequestCheck($arrTextTranslate);
    // 2020/03/30 AKB Chien - end - update document 2020/03/30

    // 2020/04/01 AKB Chien - start - update document 2020/04/01
    // GET通信にて遷移してきた場合、以下のメッセージをアラート表示し、遷移元画面に戻す。
    if(!isset($_SERVER['HTTP_REFERER'])) {
        echo '<script type="text/javascript">
                function goBack() {
                    history.go(-1);
                    return false;
                }
                alert("'.$arrTextTranslate['PUBLIC_MSG_049'].'");
                goBack();
            </script>';
        die();
    }
    // 2020/04/01 AKB Chien - end - update document 2020/04/01

    // 以下の条件でログインユーザの情報を取得し、表示項目に表示する。
    $arrResult = fncGetInfoUser($intUserNo, $intLanguageType);

    $arrRes = array(
        'USER_NO' => '',
        'USER_ID' => '',
        'COMPANY_NAME' => '',
        'ABBREVIATIONS' => '',
        'ADDRESS' => '',
        'ORGANIZATION' => '',
        'USER_NAME' => '',
        'MAIL_ADDRESS' => '',
        'TEL' => '',
        'FAX' => '',
        'PASSWORD' => '',
        'LANGUAGE_TYPE' => '',
    );
    // prepare data to show
    if($arrResult != 0 && count($arrResult) > 0) {
        $arrRes = array(
            'USER_NO' => @$arrResult[0]['USER_NO']
                            ? fncHtmlSpecialChars($arrResult[0]['USER_NO']) : '',
            'USER_ID' => @$arrResult[0]['USER_ID']
                            ? fncHtmlSpecialChars($arrResult[0]['USER_ID']) : '',
            'COMPANY_NAME' => @$arrResult[0]['COMPANY_NAME']
                            ? fncHtmlSpecialChars($arrResult[0]['COMPANY_NAME']) : '',
            'ABBREVIATIONS' => @$arrResult[0]['ABBREVIATIONS']
                            ? fncHtmlSpecialChars($arrResult[0]['ABBREVIATIONS']) : '',
            'ADDRESS' => @$arrResult[0]['ADDRESS']
                            ? fncHtmlSpecialChars($arrResult[0]['ADDRESS']) : '',
            'ORGANIZATION' => @$arrResult[0]['ORGANIZATION']
                            ? fncHtmlSpecialChars($arrResult[0]['ORGANIZATION']) : '',
            'USER_NAME' => @$arrResult[0]['USER_NAME']
                            ? fncHtmlSpecialChars($arrResult[0]['USER_NAME']) : '',
            'MAIL_ADDRESS' => @$arrResult[0]['MAIL_ADDRESS']
                            ? fncHtmlSpecialChars($arrResult[0]['MAIL_ADDRESS']) : '',
            'TEL' => @$arrResult[0]['TEL']
                            ? fncHtmlSpecialChars($arrResult[0]['TEL']) : '',
            'FAX' => @$arrResult[0]['FAX']
                            ? fncHtmlSpecialChars($arrResult[0]['FAX']) : '',
            'PASSWORD' => @$arrResult[0]['PASSWORD']
                            ? fncHtmlSpecialChars($arrResult[0]['PASSWORD']) : '',
            'LANGUAGE_TYPE' => @$arrResult[0]['LANGUAGE_TYPE']
                            ? fncHtmlSpecialChars($arrResult[0]['LANGUAGE_TYPE']) : '',
        );
    }

    /**
     * get data search
     *
     * @create 2020/03/13 AKB Chien
     * @update
     * @param integer $intUserNo    user_no of the user who is logged in
     * @param integer $intLang      languague type of the user who is logged in
     * @return array $arrResult     array result info of the user who is logged in
     */
    function fncGetInfoUser($intUserNo, $intLang) {
        try {
            $suffixes = ($intLang == 0) ? '_JPN' : '_ENG';
            $companyNameSelect = 'company_name'.$suffixes;
            $abbreviationsSelect = 'abbreviations'.$suffixes;

            $strSQL = ' SELECT  '
                    . '     mu.USER_NO, '
                    . '     mu.USER_ID, '
                    . '     mu.ADDRESS, '
                    . '     mu.ORGANIZATION, '
                    . '     mu.USER_NAME, '
                    . '     mu.MAIL_ADDRESS, '
                    . '     mu.USER_ID, '
                    . '     mu.TEL, '
                    . '     mu.FAX, '
                    . '     mu.PASSWORD, '
                    . '     mu.LANGUAGE_TYPE, '
                    . '     mc.'.$companyNameSelect.' AS COMPANY_NAME, '
                    . '     mc.'.$abbreviationsSelect.' AS ABBREVIATIONS '
                    . ' FROM '
                    . '     m_user AS mu '
                    . '     INNER JOIN m_company AS mc ON (mu.company_no = mc.company_no) '
                    . ' WHERE '
                    . '     mu.user_no = ? ';
            // execute SQL and get data
            $arrResult = fncSelectData($strSQL, array($intUserNo), 1, false, DISPLAY_TITLE);
            return $arrResult;
        } catch (\Exceoption $e) {
            // write log
            fncWriteLog(LogLevel['Error'], LogPattern['Error'], $e->getMessage());
            return 0;
        }
    }
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title><?php
        echo $arrTextTranslate['USER_SETTING_TEXT_001'];
    ?></title>
    <link rel="stylesheet" type="text/css" href="css/style.css">
    <style>
        #id-width {
            width: 125px;
        }
        <?php
            // if user is english-type -> fix mock
            if($intLanguageType == 1) {
                echo '.ip-span { margin: 6px 0 !important; } ';
                echo '#id-width { width: 135px !important; } ';
                echo '.pdl-15 { padding-left: 15px !important } ';
            }else{
                echo '.pdl-15 { padding-left: 10px !important } ';
            }
        ?>
    </style>
</head>
<body>
<div class="main-content">
    <div class="main-form">
        <div class="form-title"><?php
            echo $arrTextTranslate['USER_SETTING_TEXT_001']; ?></div>
        <div class="form-body-90">
            <div><font color="red" class="error-edit"></font></div>
            <form class="edit-form" action="" method="POST" autocomplete="off">
                <div class="label-short top-20" id="id-width" style="margin-left: 0;<?php
                    if($intLanguageType == 1) {
                        echo 'margin-right: 0;';
                } ?>"><?php
                    echo $arrTextTranslate['USER_SETTING_TEXT_002'];
                ?></div><span class="text-gray" name="lblID"><?php
                    echo $arrRes['USER_ID'];
                ?></span>
                <div class="mar-tl-10">
                    <div class="tcol-md-6 in-line float-left">
                        <div class="lb-left"><?php
                            echo $arrTextTranslate['USER_SETTING_TEXT_003'];
                        ?></div>
                        <span class="text-gray ip-span" name="lblCompany"><?php
                            echo $arrRes['COMPANY_NAME'];
                        ?></span>
                    </div>
                    <div class="tcol-md-6 in-line float-left">
                        <div class="lb-left"
                            style="margin-left: 0; width: 100% !important; height: 45px;"
                        ><?php
                                echo $arrTextTranslate['USER_SETTING_TEXT_004'];
                            ?><span class="text-gray pdl-15" name="lblAbbreviations"><?php
                                echo $arrRes['ABBREVIATIONS'];
                            ?></span>
                        </div>
                    </div>
                </div>

                <div class="lb-left"><?php
                    echo $arrTextTranslate['USER_SETTING_TEXT_005'];
                ?></div>
                <span class="ip-span">
                    <textarea class="t-input" cols="2" name="txtAddress"><?php
                        echo $arrRes['ADDRESS'];
                    ?></textarea>
                </span>

                <div class="lb-left"><?php
                    echo $arrTextTranslate['USER_SETTING_TEXT_006'];
                ?></div>
                <span class="ip-span">
                    <input type="text" class="t-input"
                        value="<?php echo $arrRes['ORGANIZATION']; ?>" name="txtOrganization">
                </span>

                <div class="lb-left"><?php
                    echo $arrTextTranslate['USER_SETTING_TEXT_007'];
                ?></div>
                <span class="ip-span"><input type="text" class="t-input"
                    value="<?php echo $arrRes['USER_NAME']; ?>" name="txtName">
                </span>

                <div class="lb-left"><?php
                        echo $arrTextTranslate['USER_SETTING_TEXT_008'];
                    ?> <span class="txt-red">※</span>
                </div>
                <span class="ip-span"><input type="text" class="t-input"
                    value="<?php echo $arrRes['MAIL_ADDRESS']; ?>" name="txtMail">
                </span>

                <div class="lb-left"><?php
                    echo $arrTextTranslate['USER_SETTING_TEXT_009'];
                ?></div>
                <span class="ip-span"><input type="text" class="t-input"
                    value="<?php echo $arrRes['TEL']; ?>" name="txtTel">
                </span>

                <div class="lb-left"><?php
                    echo $arrTextTranslate['USER_SETTING_TEXT_010'];
                ?></div>
                <span class="ip-span"><input type="text" class="t-input"
                    value="<?php echo $arrRes['FAX']; ?>" name="txtFax">
                </span>

                <div class="lb-left"><?php
                        echo $arrTextTranslate['USER_SETTING_TEXT_011'];
                    ?> <span class="txt-red">※</span>
                </div>
                <span class="ip-span"><input type="password" class="t-input"
                    value="<?php echo $arrRes['PASSWORD']; ?>" name="password">
                </span>

                <div class="lb-left"><?php
                        echo $arrTextTranslate['USER_SETTING_TEXT_012'];
                    ?> <span class="txt-red">※</span>
                </div>
                <span class="ip-span">
                    <span class="select-container">
                        <select name="cmbLanguage">
                            <option value="ja"
                                <?php
                                    // if user is jpn
                                    if($arrRes['LANGUAGE_TYPE'] == 0) {
                                        echo 'selected'; }
                                ?>>日本語</option>
                            <option value="en"
                                <?php
                                    // if user is eng
                                    if($arrRes['LANGUAGE_TYPE'] == 1) {
                                        echo 'selected'; }
                                ?>>English</option>
                        </select>
                    </span>
                </span>

                <div class="form-footer top-20 text-right">
                    <button type="button" class="tbtn tbtn-defaut" name="btnUpDate"><?php
                        echo $arrTextTranslate['PUBLIC_BUTTON_014'];
                    ?></button>
                    <button type="button" class="tbtn-cancel tbtn-defaut"
                        name="btnClose" id="close-setting" data-dismiss="modal"><?php
                        echo $arrTextTranslate['PUBLIC_BUTTON_003'];
                    ?></button>
                </div>
            </form>
        </div>
    </div>
</div>
</body>
<script>
    $(function() {
        var csrf = $('meta[name="csrf-token"]').attr('content');
        var inputToken = '<input type="hidden" name="X-CSRF-TOKEN" value="'+csrf+'">';
        $('form').append(inputToken);
    });

    $(document).ready(function() {
        $('#close-setting').on('click', function() {
            loadPortalClose();
            $('#myModal').modal('hide');
        });

        $('button[name=btnUpDate]').off().on('click', function(e) {
            e.preventDefault();
            $('.error-edit').html('');
            setTimeout(function(){
            if(confirm('<?php echo $arrTextTranslate['USER_SETTING_MSG_001']; ?>')) {
                var data = $('.edit-form').serializeArray();
                data.push({ name: 'mode', value: 1 });
                $.ajax({
                    url: 'user_setting_proc.php',
                    type: 'POST',
                    data: data,
                    success: function(result) {
                        if(result != '') {
                            if($.trim(result) == 'window.location.href="login.php";') {
                                window.location.href="login.php";
                                return;
                            }
                            var res = getData(result);
                            if(res['error'] != '') {
                                $('.error-edit').html(res['error']);
                                return;
                            }
                            if(res['success'] != '' && res['success'] == 1) {
                                $('#myModal').modal('hide');
                                window.location.reload();
                                return;
                            }
                        }
                    },
                    error: function(e) {
                        console.log(e);
                    }
                });
            }
        },15);
        });
    });

    function getData(str) {
        var res;
        try {
            res = JSON.parse(str);
        } catch (e) {
            res = str;
        }
        return res;
    }
</script>
</html>