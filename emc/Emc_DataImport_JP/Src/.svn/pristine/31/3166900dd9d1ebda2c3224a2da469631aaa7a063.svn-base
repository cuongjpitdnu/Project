<?php

    /*
     * @link_mng_proc.php
     *
     * @create 2020/03/13 AKB Thang
     */

    require_once('common/common.php');
    require_once('common/session_check.php');
    header('Content-type: text/html; charset=utf-8');
    header('X-FRAME-OPTIONS: DENY');
    //DB接続
    if(fncConnectDB() == false) {
        $_SESSION['LOGIN_ERROR'] = 'DB接続に失敗しました。';
        header('Location: login.php');
        exit;
    }

    $_SESSION['LINK_MNG_MSG_ERROR_INPUT'] = '';
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
    fncSessionTimeOutCheck();



    //constant
    define('SCREEN_NAME', 'リンク情報管理画面');
    $objUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);

    // get list text translate
    $arrTitle =  array(
        'LINK_MNG_TEXT_003',
        'LINK_MNG_TEXT_004',
        'LINK_MNG_TEXT_005',
        'LINK_MNG_TEXT_006',
        'PUBLIC_BUTTON_009',
        'PUBLIC_BUTTON_010',
        'PUBLIC_BUTTON_001',
        'PUBLIC_BUTTON_015',
        'PUBLIC_MSG_004',
        'LINK_MNG_MSG_002',
        'PUBLIC_MSG_001',
        'PUBLIC_TEXT_016'
    );

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
    //if request is not ajax -> stop
    if(
        !(!empty($_SERVER['HTTP_X_REQUESTED_WITH'])
        && strtolower($_SERVER['HTTP_X_REQUESTED_WITH']) == 'xmlhttprequest')
    )
    {
        exit;
    }
    //DB接続
    if(fncConnectDB() == false) {
        echo 0;
        exit();
    }

    //reset search data
    if(!isset($_POST['mode'])&&!isset($_POST['mov'])&&!isset($_POST['del'])) {
        $_SESSION['catNO'] = '';
    }
    //load list data
    if(isset($_POST['loadList'])) {
        //search by link category
        if(isset($_POST['catNO'])) {
            $_SESSION['catNO'] = $_POST['catNO'];
            if(trim($_SESSION['catNO']) != ''){
                //get link category
                $arrLinkCategory = fncSelectOne(
                    "SELECT LINK_CATEGORY_NAME_JPN, LINK_CATEGORY_NAME_ENG,"
                    .iff(
                        $objUserInfo->intLanguageType,
                        " LINK_CATEGORY_NAME_ENG as LINK_CATEGORY_NAME",
                        " LINK_CATEGORY_NAME_JPN as LINK_CATEGORY_NAME"
                    )
                    ." FROM m_link_category WHERE LINK_CATEGORY_NO=?",
                    [$_SESSION['catNO']],
                    SCREEN_NAME
                );
                if(is_array($arrLinkCategory) && count($arrLinkCategory) > 0){
                    //if link category is exist
                    $strLinkCategoryName = $arrLinkCategory['LINK_CATEGORY_NAME'];
                }else{
                    //link category not exist, store errror mesage to session
                    $_SESSION['LINK_MNG_MSG_ERROR_INPUT']
                    = fncHtmlSpecialChars($arrTextTranslate['PUBLIC_MSG_001']);
                }
            }else{
                $strLinkCategoryName = 'ALL';
            }
        }
        //log button search
        if(isset($_POST['originalSearch']) && $_POST['originalSearch'] == 1) {
            $strSearchQuery = '「カテゴリ = '.$strLinkCategoryName.'」';
            $strViewLog = SCREEN_NAME . '　検索を実施 '
            .$strSearchQuery
            .' (ユーザID = '.$objUserInfo->strUserID.')';
            fncWriteLog(LogLevel['Info'] , LogPattern['Button'], $strViewLog);
        }
    }
    //get links
    $arrCondition = [];

    $strSQL = "SELECT t_link.LINK_NO,";
    $strSQL .= " t_link.LINK_CATEGORY_NO, t_link.SORT_NO,";
    //get link name
    if($objUserInfo->intLanguageType) {
        //user language is English
        $strSQL .= ' t_link.LINK_NAME_ENG as LINK_NAME,';
        $strSQL .= ' m_link_category.LINK_CATEGORY_NAME_ENG as LINK_CATEGORY_NAME,';
    } else {
        //user language is Japanese
        $strSQL .= ' t_link.LINK_NAME_JPN as LINK_NAME,';
        $strSQL .= ' m_link_category.LINK_CATEGORY_NAME_JPN as LINK_CATEGORY_NAME,';
    }

    $strSQL .= ' t_link.URL';
    $strSQL .= ' FROM t_link';

    $strSQL .= ' INNER JOIN m_link_category';
    $strSQL .= ' ON m_link_category.LINK_CATEGORY_NO=t_link.LINK_CATEGORY_NO';
    //search by link category
    if(isset($_SESSION['catNO']) && $_SESSION['catNO'] != '') {
        $strSQL .= ' WHERE t_link.LINK_CATEGORY_NO=?';
        $arrCondition[] = $_SESSION['catNO'];
    }

    $strSQL .= ' ORDER BY m_link_category.SORT_NO ASC, t_link.SORT_NO ASC';


    //mode
    if(isset($_POST['mode'])) {
        //delete a link
        if($_POST['mode'] == 1) {
            try{
                //log button delete
                fncWriteLog(
                    LogLevel['Info'],
                    LogPattern['Button'],
                    SCREEN_NAME . ' 削除(ユーザID = '.$objUserInfo->strUserID.')'
                );

                //delete link sql query
                $strSQL = '';
                $strSQL .= 'DELETE FROM t_link WHERE LINK_NO=:LINK_NO';
                $query = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
                $query->bindParam(':LINK_NO', $_POST['id']);
                $query->execute();
                $strSqlLog = str_replace(':LINK_NO', $_POST['id'], $strSQL);
                //log sql
                fncWriteLog(LogLevel['Info'] , LogPattern['Sql'], SCREEN_NAME . ' '.$strSqlLog);



                if($query->rowCount() > 0) {
                    //delete query execute successfully, delete file and folder
                    $strFolderPath = SHARE_FOLDER.'/'.LINK_EDIT_FOLDER.'/'.$_POST['id'].'/1/';
                    if (file_exists($strFolderPath)) {
                        $files = glob($strFolderPath.'*'); // get all file names
                        foreach($files as $file){ // iterate files
                        if(is_file($file))
                            unlink($file); // delete file
                        }
                        //delete folder
                        rmdir($strFolderPath);
                        rmdir(SHARE_FOLDER.'/'.LINK_EDIT_FOLDER.'/'.$_POST['id'].'/');
                    }


                }
                echo 0;
                exit();
            }
            catch(\Exception $e){
                echo fncHtmlSpecialChars($arrTextTranslate['PUBLIC_MSG_004']);
                fncWriteLog(
                    LogLevel['Error'],
                    LogPattern['Error'],
                    SCREEN_NAME . ' ' .$arrTextTranslate['PUBLIC_MSG_004']
                );
                exit();
            }

        }
        //move link
        if($_POST['mode'] == 2) {
            //log move button
            fncWriteLog(
                LogLevel['Info'],
                LogPattern['Button'],
                SCREEN_NAME . '　ソート順を変更(ユーザID = '.$objUserInfo->strUserID.')'
            );

            //get current link
            $arrCurrentLink = fncSelectOne(
                "SELECT * FROM t_link WHERE LINK_NO=? AND SORT_NO=?",
                [$_POST['id'], $_POST['sort']],
                SCREEN_NAME
            );
            //get link up down
            $arrNextLink = fncSelectOne(
                "SELECT * FROM t_link WHERE LINK_NO=? AND SORT_NO=?",
                [$_POST['next'], $_POST['sortNext']],
                SCREEN_NAME
            );
            if(
                is_array($arrCurrentLink)
                && is_array($arrNextLink)
                && count($arrCurrentLink)
                && count($arrNextLink)
            ){
                //current link and next link is valid, process
                $GLOBALS['g_dbaccess']->funcBeginTransaction();
                $arrData = [];
                $arrData['SORT_NO'] = $arrCurrentLink['SORT_NO'];
                $objResult1 = fncProcessData(
                    $arrData,
                    't_link',
                    'LINK_NO=?',
                    [$arrNextLink['LINK_NO']],
                    SCREEN_NAME
                );
                //update $arrCurrentLink
                $arrData['SORT_NO'] = $arrNextLink['SORT_NO'];
                $objResult2 = fncProcessData(
                    $arrData,
                    't_link',
                    'LINK_NO=?',
                    [$arrCurrentLink['LINK_NO']],
                    SCREEN_NAME
                );
                if(!is_object($objResult1) || !is_object($objResult2)){
                    //update fail, roll back
                    $GLOBALS['g_dbaccess']->funcRollback();
                    echo fncHtmlSpecialChars($arrTextTranslate['LINK_MNG_MSG_002']);
                    fncWriteLog(
                        LogLevel['Error'],
                        LogPattern['Error'],
                        $arrTextTranslate['LINK_MNG_MSG_002']
                    );
                    exit();
                }
                if(!($objResult1->rowCount() && $objResult2->rowCount())) {
                    //update fail, roll back
                    $GLOBALS['g_dbaccess']->funcRollback();
                    echo fncHtmlSpecialChars($arrTextTranslate['LINK_MNG_MSG_002']);
                    fncWriteLog(
                        LogLevel['Error'],
                        LogPattern['Error'],
                        $arrTextTranslate['LINK_MNG_MSG_002']
                    );
                    exit();
                } else {
                    //everything is ok, commit
                    $GLOBALS['g_dbaccess']->funcCommit();

                    echo 0;
                    exit();
                }
            } else {
                //links are not valid
                echo $arrTextTranslate['LINK_MNG_MSG_002'];
                //log error
                fncWriteLog(
                    LogLevel['Error'],
                    LogPattern['Error'],
                    SCREEN_NAME . ' ' . $arrTextTranslate['LINK_MNG_MSG_002']
                );
                exit();
            }
        }
        exit();
    }

    if(
        isset($_SESSION['LINK_MNG_MSG_ERROR_INPUT'])
        && $_SESSION['LINK_MNG_MSG_ERROR_INPUT']!=''
    ) {
        //show error message
?>
<script>
    $(function() {
        var errStr = '<?php echo $_SESSION['LINK_MNG_MSG_ERROR_INPUT'] ?>';
        $('.error').html(errStr);
    });
</script>
<?php
    }else{
        //get array link list
        $arrLinkList = fncSelectData(
            $strSQL,
            $arrCondition,
            $GLOBALS['currentPage'],
            false,
            SCREEN_NAME);
        if(!is_array($arrLinkList)){
            //get data failed, show error
            echo "<script>";
            echo "$('.error').html('".$arrTextTranslate['PUBLIC_MSG_001']."');";
            echo "</script>";
        }

    }
?>

<table class="blueTable">
    <thead>
        <tr>
            <th><?php echo $arrTextTranslate['LINK_MNG_TEXT_003']; ?></th>
            <th><?php echo $arrTextTranslate['LINK_MNG_TEXT_004']; ?></th>
            <th><?php echo $arrTextTranslate['LINK_MNG_TEXT_005']; ?></th>
            <th><?php echo $arrTextTranslate['LINK_MNG_TEXT_006']; ?></th>
        </tr>
    </thead>
    <tfoot>
        <tr>
            <td>
                <?php
                    echo str_replace('%1%', isset($arrLinkList)
                    && is_array($arrLinkList)
                    ? count($arrLinkList) : 0,
                    $arrTextTranslate['PUBLIC_TEXT_016'])  ;
                ?>
            </td>
            <td colspan="3" align="right">

                <button type="submit" class="tbtn tbtn-defaut load-modal"
                href="link_edit.php" data-id="0">
                    <?php echo $arrTextTranslate['PUBLIC_BUTTON_001']; ?>
                </button>
                <button type="submit" class="tbtn tbtn-defaut"
                onclick="window.location.href='portal.php'">
                    <?php echo $arrTextTranslate['PUBLIC_BUTTON_015']; ?>
                </button>
            </td>
        </tr>
    </tfoot>
    <tbody>
        <?php
            //show link list
            if(isset($arrLinkList)
            && is_array($arrLinkList)) foreach($arrLinkList as $item) {
        ?>
        <tr>
            <td class="text-center">
                <?php echo fncHtmlSpecialChars($item['LINK_CATEGORY_NAME']); ?>
            </td>
            <td>
                <?php echo fncHtmlSpecialChars($item['LINK_NAME']); ?>
            </td>
            <td>
                <a href="<?php echo fncHtmlSpecialChars($item['URL']); ?>" target="_blank">
                    <?php echo fncHtmlSpecialChars($item['URL']); ?>
                </a>
            </td>
            <td class="text-center" style="position: relative">
                <div class="in-lineblock">
                    <button class="tbl-btn tbtn-green in-line load-modal"
                    href="link_edit.php" data-id="<?php echo $item['LINK_NO']; ?>">
                        <?php echo fncHtmlSpecialChars($arrTextTranslate['PUBLIC_BUTTON_009']); ?>
                    </button>
                    <button class="tbl-btn tbtn-red in-line btn-del"
                    data-id="<?php echo $item['LINK_NO'] ; ?>">
                        <?php echo fncHtmlSpecialChars($arrTextTranslate['PUBLIC_BUTTON_010']); ?>
                    </button>
                </div>
                <div class="in-lineblock btn-up-down">
                    <button class="tbtn-ud cat-<?php
                        echo $item['LINK_CATEGORY_NO'];
                    ?>-up up tbl-btn tbtn-blue in-line"
                    data-id="<?php echo $item['LINK_NO']; ?>" value="1"
                    data-class="cat-<?php echo $item['LINK_CATEGORY_NO'] ?>-up"
                    data-sort="<?php echo $item['SORT_NO'] ?>">
                    ↑
                    </button>
                    <button class="tbtn-ud cat-<?php
                        echo $item['LINK_CATEGORY_NO'];
                    ?>-down down tbl-btn tbtn-blue in-line"
                    data-id="<?php echo $item['LINK_NO']; ?>" value="-1"
                    data-class="cat-<?php echo $item['LINK_CATEGORY_NO'] ?>-down"
                    data-sort="<?php echo $item['SORT_NO'] ?>">
                    ↓
                    </button>
                </div>
            </td>
        </tr>
        <?php } ?>
    </tbody>
    </tr>
</table>