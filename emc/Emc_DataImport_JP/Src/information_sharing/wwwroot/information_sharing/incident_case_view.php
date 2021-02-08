<?php
/*
 * @インシデント事案表示画面
 *
 * @create 2020/03/23 KBS T.Masuda
 * @update
 */
    require_once('common/common.php');

    header('Content-type: text/html; charset=utf-8');
    header('X-FRAME-OPTIONS: DENY');

    if(fncConnectDB() == false) {
        $_SESSION['LOGIN_ERROR'] = 'DB接続に失敗しました。';
        header('Location: login.php');
        exit;
    }
    
    //ログインしていない場合、ログイン画面を表示する
    if(!isset($_SESSION['LOGINUSER_INFO'])) {
        echo '<script>alert("'.PUBLIC_MSG_008_JPN.' / '.PUBLIC_MSG_008_ENG.'");
                           window.location.href="login.php";</script>';
        exit;
    }

    //ログインユーザ情報を取得
    $objLoginUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);

    //ログインユーザの言語タイプ
    $intLanguageType = $objLoginUserInfo->intLanguageType;

    //表示テキスト・メッセージ
    $arrTitleMsg =  array(
        'PUBLIC_MSG_049',

        'INCIDENT_CASE_VIEW_TEXT_001',
        'INCIDENT_CASE_VIEW_TEXT_002',
        'PUBLIC_BUTTON_002',
        'PUBLIC_BUTTON_003',

        'PUBLIC_TEXT_010',
        'PUBLIC_MSG_020',
        'PUBLIC_MSG_001',

        'INCIDENT_CASE_VIEW_MSG_001',
        
        //▼2020/05/27 KBS T.Masuda 権限がないユーザへのアラートメッセージ
        'PUBLIC_MSG_009',
        //▲2020/05/27 KBS T.Masuda
    );

    //言語タイプに応じたテキスト・メッセージ
    $arrTextTranslate = getListTextTranslate($arrTitleMsg, $intLanguageType);

    //URLを直接指定した場合
    if($_SERVER['REQUEST_METHOD'] == 'GET'){
        echo '<script>alert("'.$arrTextTranslate['PUBLIC_MSG_049'].'");
                       history.back();</script>';
        exit;
    }
    
    //▼2020/05/27 KBS T.Masuda Jcmgタブ権限が無いユーザはログイン画面に遷移
    if($objLoginUserInfo->intJcmgTabPerm != 1){
        echo '<script>alert("'.$arrTextTranslate['PUBLIC_MSG_009'].'");
                      window.location.href="login.php";</script>';
        exit;
    }
    //▲2020/05/27 KBS T.Masuda

    fncSessionTimeOutCheck();

    const SCREEN_NAME = 'JCMG事案表示画面';

    //VIEWログ内容
    $strLogView = SCREEN_NAME.'　表示'.
        '(ユーザID = '.$objLoginUserInfo->strUserID.') '.
        (isset($_SERVER['HTTP_REFERER']) ? $_SERVER['HTTP_REFERER'] : null);

    //表示ログ登録
    fncWriteLog(LogLevel['Info'], LogPattern['View'],$strLogView );

    //遷移元画面
    $strScreenRef = $_POST['screen'];

    //表示インシデントNo
    $intIncidentNo = @$_POST['id'] ? $_POST['id'] : '';

    //表示インシデント事案情報
    $arrDataIncident = fncGetInfoIncident($intIncidentNo, SCREEN_NAME);

    //データ取得失敗時
    if($arrDataIncident == 0 ){
        $strAlert = '<script>alert("'.$arrTextTranslate['PUBLIC_MSG_001'].'");
                            setTimeout(function() {';
        if($strScreenRef == 'portal'){
            $strAlert.="window.location.href = 'portal_move.php'";
        }else{
            $strAlert.='window.location.reload();';
        }
        $strAlert.= '}, 300);';
        $strAlert.= "$('#myModal').modal('hide');</script>";
        echo $strAlert;
        exit;
    }

    //インシデント情報格納
    $arrRes = array(
        'S_DATE' => '',
        'TITLE_ORIGINAL' => '',
        'TITLE_TRANSLATE' => '',
        'CONTENTS_ORIGINAL' => '',
        'CONTENTS_TRANSLATE' => '',
        'LANGUAGE_TYPE' => '',
        'CORRECTION_FLAG' => '',
    );
    //インシデント事案情報を格納
    if($arrDataIncident != 0 && count($arrDataIncident) > 0) {
        //タイトル（原文）
        $strTitleOriginal = '';
        //タイトル（翻訳）
        $strTitleTranslate = '';
        //内容（原文）
        $strContentsOriginal = '';
        //内容（翻訳）
        $strContentsTranslate = '';
        //開始日
        $dtmStartDate = New DateTime($arrDataIncident[0]['S_DATE']);
        //言語タイプが日本語か英語
        if($arrDataIncident[0]['LANGUAGE_TYPE'] == 0) {
            $strTitleOriginal = @$arrDataIncident[0]['TITLE_JPN']
                                 ? fncHtmlSpecialChars(trim($arrDataIncident[0]['TITLE_JPN'])) : '';
            $strTitleTranslate = @$arrDataIncident[0]['TITLE_ENG']
                                  ? fncHtmlSpecialChars(trim($arrDataIncident[0]['TITLE_ENG'])) : '';
            $strContentsOriginal = @$arrDataIncident[0]['CONTENTS_JPN']
                                    ? fncHtmlSpecialChars(trim($arrDataIncident[0]['CONTENTS_JPN'])) : '';
            $strContentsTranslate = @$arrDataIncident[0]['CONTENTS_ENG']
                                     ? fncHtmlSpecialChars(trim($arrDataIncident[0]['CONTENTS_ENG'])) : '';
        } else {
            $strTitleOriginal = @$arrDataIncident[0]['TITLE_ENG']
                                 ? fncHtmlSpecialChars(trim($arrDataIncident[0]['TITLE_ENG'])) : '';
            $strTitleTranslate = @$arrDataIncident[0]['TITLE_JPN']
                                  ? fncHtmlSpecialChars(trim($arrDataIncident[0]['TITLE_JPN'])) : '';
            $strContentsOriginal = @$arrDataIncident[0]['CONTENTS_ENG']
                                    ? fncHtmlSpecialChars(trim($arrDataIncident[0]['CONTENTS_ENG'])) : '';
            $strContentsTranslate = @$arrDataIncident[0]['CONTENTS_JPN']
                                     ? fncHtmlSpecialChars(trim($arrDataIncident[0]['CONTENTS_JPN'])) : '';
        }
        
        //表示するJCMGデータを配列に格納
        $arrRes = array(
            'S_DATE' => $dtmStartDate->format('Y/m/d H:i'),
            'TITLE_ORIGINAL' => $strTitleOriginal,
            'TITLE_TRANSLATE' => $strTitleTranslate,
            'CONTENTS_ORIGINAL' => $strContentsOriginal,
            'CONTENTS_TRANSLATE' => $strContentsTranslate,
            'LANGUAGE_TYPE' => $arrDataIncident[0]['LANGUAGE_TYPE'],
            'CORRECTION_FLAG' => $arrDataIncident[0]['CORRECTION_FLAG'],
        );

    //削除されたインシデント事案だった場合、アラート表示し、画面を閉じる
    }else if(count($arrDataIncident) == 0){
        //デイリーボードを表示する
        $_SESSION['tab'] = -1;

        $strAlert ='<script>alert("'.$arrTextTranslate['INCIDENT_CASE_VIEW_MSG_001'].'");
                            setTimeout(function() {';
        //遷移元がポータル画面の場合
        if($strScreenRef == 'portal'){
            $strAlert.="window.location.href = 'portal_move.php'";
        }else{
            $strAlert.='window.location.reload();';
        }
        $strAlert.= '}, 300);';
        $strAlert.= "$('#myModal').modal('hide');</script>";
        echo $strAlert;
        exit;
    }



?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="csrf-token" content="<?php echo (isset($_POST['X-CSRF-TOKEN']) ? $_POST['X-CSRF-TOKEN'] : ''); ?>">
    <title><?php echo $arrTextTranslate['INCIDENT_CASE_VIEW_TEXT_001']; ?></title>
    <link rel="stylesheet" type="text/css" href="css/style.css">
</head>


<body>
    <div class="main-content">
        <div class="main-form">
            <div class="form-title"><?php echo $arrTextTranslate['INCIDENT_CASE_VIEW_TEXT_001']; ?></div>
            <div class="form-body">
            <?php if($arrRes['CORRECTION_FLAG'] == 0){?>
                <div class="error-messeage">
                    <div><?php echo fncHtmlSpecialChars(PUBLIC_TEXT_001_JPN); ?></div>
                    <div><?php echo fncHtmlSpecialChars(PUBLIC_TEXT_001_ENG); ?></div>
                </div>
            <?php } ?>
                <div class="cont-title">
                    <div class="in-lineblock col-left">
                        <div class="in-line"><?php echo $arrTextTranslate['INCIDENT_CASE_VIEW_TEXT_002']; ?></div>
                        <div class="in-line"><?php echo $arrRes['S_DATE'];?></div>
                    </div>

              <?php if($objLoginUserInfo->intIncidentCaseRegPerm == 1){?>
                    <div class="col-right in-lineblock right-20">
                         <button type="button" class="tbtn tbtn-defaut load-modal" href="incident_case_edit.php" data-id="<?php echo $intIncidentNo?>" data-screen="<?php echo $strScreenRef; ?>" id="btn-edit"><?php echo $arrTextTranslate['PUBLIC_BUTTON_002']; ?></button>
                    </div>
              <?php } ?>
                </div>
                <br>

                <div>
                    <p style="background-color:#4169e1">
                    <legend><?php echo fncHtmlSpecialChars(PUBLIC_TEXT_004_JPN).'/'.fncHtmlSpecialChars(PUBLIC_TEXT_004_ENG); ?></legend>
                    </p>
                    <div class="info-left">
                        <div class="line">
                            <div class="in-line tlabel">(<?php echo fncHtmlSpecialChars(PUBLIC_TEXT_002_JPN).'/'.fncHtmlSpecialChars(PUBLIC_TEXT_002_ENG); ?>)</div>
                            <div class="in-line text-input text-bold" style="word-break: break-all;"><?php echo fncBreakAll($arrRes['TITLE_ORIGINAL']);?></div>
                        </div>
                        <div class="line">
                            <div class="in-line tlabel">(<?php echo fncHtmlSpecialChars(PUBLIC_TEXT_003_JPN).'/'.fncHtmlSpecialChars(PUBLIC_TEXT_003_ENG); ?>)</div>
                            <div class="in-line text-input text-bold" style="word-break: break-all;"><?php echo fncBreakAll($arrRes['TITLE_TRANSLATE']);?></div>
                        </div>
                    </div>
                </div>

                <p style="background-color:#4169e1">
                <legend><?php echo fncHtmlSpecialChars(PUBLIC_TEXT_005_JPN).'/'.fncHtmlSpecialChars(PUBLIC_TEXT_005_ENG); ?></legend>
                </p>
                <div class="info">
                    <div class="line">
                        <div >(<?php echo fncHtmlSpecialChars(PUBLIC_TEXT_002_JPN).'/'.fncHtmlSpecialChars(PUBLIC_TEXT_002_ENG); ?>)</div>
                        <div class="text-input text-bold " style="word-break: break-all;">
                            <?php echo fncBreakAll($arrRes['CONTENTS_ORIGINAL']);?>
                        </div>
                    </div>
                </div>

                <div class="info">
                    <div class="line">
                        <div >(<?php echo fncHtmlSpecialChars(PUBLIC_TEXT_003_JPN).'/'.fncHtmlSpecialChars(PUBLIC_TEXT_003_ENG); ?>)</div>
                        <div class="text-input text-bold" style="word-break: break-all;">
                            <?php echo fncBreakAll($arrRes['CONTENTS_TRANSLATE']) ;?>
                        </div>
                    </div>
                </div>

                <div class="form-footer text-right right-20">
                    <button type="submit" class="tbtn-cancel tbtn-defaut " name="btnClose" id="close" data-dismiss="modal"><?php echo $arrTextTranslate['PUBLIC_BUTTON_003']; ?></button>
                </div>
            </div>
        </div>
    </div>
    <script>
        $('button[name=btnClose]').off().on('click', function(e) {
            e.preventDefault();

			if('<?php echo $strScreenRef?>' == 'portal'){
                
                $('#myModal').modal('hide');
                loadPortalClose();
			}else{
				setTimeout(function() {
	                window.location.reload();
	            }, 300);
	            $('#myModal').modal('hide');
			}
        });

    </script>
    </body>
</html>