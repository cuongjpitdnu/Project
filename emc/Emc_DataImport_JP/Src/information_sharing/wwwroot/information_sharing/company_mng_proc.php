<?php
    /*
     * @company_mng_proc.php
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
    //define constant
    define('SCREEN_NAME', '会社情報管理画面');

    $_SESSION['COMPANY_MNG_MSG_ERROR_INPUT'] = '';

    // if not login, redirect to login page
    if(!isset($_SESSION['LOGINUSER_INFO'])) {
        echo "
        <script>
            alert('".PUBLIC_MSG_008_JPN . "/" . PUBLIC_MSG_008_ENG."');
            window.location.href = 'login.php';
        </script>
        ";
        exit();
    }
    $objUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);

    // check timeout if direct access this file
    fncSessionTimeOutCheck(1);

    // get list text translate
    $arrTitle =  array(
        'COMPANY_MNG_TEXT_006',
        'COMPANY_MNG_TEXT_007',
        'COMPANY_MNG_TEXT_008',
        'COMPANY_MNG_TEXT_009',
        'COMPANY_MNG_TEXT_010',
        'PUBLIC_BUTTON_001',
        'PUBLIC_BUTTON_009',
        'PUBLIC_BUTTON_010',
        'PUBLIC_BUTTON_015',
        'PUBLIC_MSG_001',
        'PUBLIC_MSG_004',
        'PUBLIC_TEXT_016',
        'PUBLIC_MSG_049',

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

    //load list data
    if(!isset($_POST['page']) && !isset($_POST['mode']) && !isset($_POST['del'])) {
        $_SESSION['txtCompanyName'] = '';
        $_SESSION['txtAbbreviations'] = '';
        $_SESSION['cmbInstCategory'] = '';
        $_SESSION['cmbGroup'] = '';
    }

    //store search value to session
    if(isset($_POST['loadList'])) {
        //current page
        if(isset($_POST['page'])) $_SESSION['COMPANY_MNG_PAGE'] = $_POST['page'];
        $GLOBALS['currentPage'] = $_SESSION['COMPANY_MNG_PAGE'];
        //company name
        if(isset($_POST['txtCompanyName'])) {
            $_SESSION['txtCompanyName'] = $_POST['txtCompanyName'];
        }
        //Abbreviations
        if(isset($_POST['txtAbbreviations'])) {
            $_SESSION['txtAbbreviations'] = $_POST['txtAbbreviations'];

        }
        //Inst Category
        if(isset($_POST['cmbInstCategory'])) {
            $_SESSION['cmbInstCategory'] = $_POST['cmbInstCategory'];
            if(trim($_SESSION['cmbInstCategory']) != ''){
                //get inst Category
                $arrInstCategory = fncSelectOne(
                    "SELECT INST_CATEGORY_NAME_JPN, INST_CATEGORY_NAME_ENG,
                    ".iff(
                        $objUserInfo->intLanguageType,
                        " INST_CATEGORY_NAME_ENG as INST_CATEGORY_NAME",
                        " INST_CATEGORY_NAME_JPN as INST_CATEGORY_NAME"
                    )."
                    FROM m_inst_category WHERE INST_CATEGORY_NO=?",
                    [$_SESSION['cmbInstCategory']], SCREEN_NAME
                );
                if(is_array($arrInstCategory) && count($arrInstCategory) > 0){
                    //query successfull
                    $strInstCatName = $arrInstCategory['INST_CATEGORY_NAME'];
                }else{
                    //failed to get inst category
                    $_SESSION['COMPANY_MNG_MSG_ERROR_INPUT']
                    .= fncHtmlSpecialChars($arrTextTranslate['PUBLIC_MSG_001']);
                }
            }else{
                //select all
                $strInstCatName = 'ALL';
            }

        }
        //group
        if(isset($_POST['cmbGroup'])) {
            $_SESSION['cmbGroup'] = $_POST['cmbGroup'];
            if(trim($_SESSION['cmbGroup']) != ''){
                $arrGroup = fncSelectOne(
                    "SELECT GROUP_NAME_JPN, GROUP_NAME_ENG,
                    ".iff(
                        $objUserInfo->intLanguageType,
                        " GROUP_NAME_ENG as GROUP_NAME",
                        " GROUP_NAME_JPN as GROUP_NAME"
                    )."
                    FROM m_group WHERE GROUP_NO=?",
                    [$_SESSION['cmbGroup']],
                    SCREEN_NAME
                );
                if(is_array($arrGroup) && count($arrGroup)>0){
                    //group exist
                    $groupName = $arrGroup['GROUP_NAME'];
                }else{
                    //group not exist, store errror message to session
                    $_SESSION['COMPANY_MNG_MSG_ERROR_INPUT']
                    .= fncHtmlSpecialChars($arrTextTranslate['PUBLIC_MSG_001']);
                }
            }else{
                //select all
                $groupName = 'ALL';
            }

        }

       //log button search
       if(isset($_POST['originalSearch']) && $_POST['originalSearch'] == 1) {
          $searchQuery = '「会社名 = '.$_SESSION['txtCompanyName']
           .', 略称名 = '.$_SESSION['txtAbbreviations']
           .',機関カテゴリ = '.$strInstCatName
           .',グループ = '.$groupName.'」';
           $strViewLog = SCREEN_NAME . '　検索を実施 '.$searchQuery
           .' (ユーザID = '.$objUserInfo->strUserID.')';
           fncWriteLog(
              LogLevel['Info'],
              LogPattern['Button'],
             $strViewLog
           );

           $GLOBALS['currentPage'] = 1;
           $_SESSION['COMPANY_MNG_PAGE'] = 1;
         }

    }

    //get company
    $arrCondition = [];

    $strSQL = "SELECT m_company.COMPANY_NO,";

    if($objUserInfo->intLanguageType) {
        //login user is English
        $strSQL .= ' m_company.COMPANY_NAME_ENG as COMPANY_NAME,';
        $strSQL .= ' m_company.ABBREVIATIONS_ENG as ABBREVIATIONS,';
        $strSQL .= ' m_inst_category.INST_CATEGORY_NAME_ENG as INST_CATEGORY_NAME,';
        $strSQL .= ' m_group.GROUP_NAME_ENG as GROUP_NAME,';
    } else {
        //login user is japanese
        $strSQL .= ' m_company.COMPANY_NAME_JPN as COMPANY_NAME,';
        $strSQL .= ' m_company.ABBREVIATIONS_JPN as ABBREVIATIONS,';
        $strSQL .= ' m_inst_category.INST_CATEGORY_NAME_JPN as INST_CATEGORY_NAME,';
        $strSQL .= ' m_group.GROUP_NAME_JPN as GROUP_NAME,';
    }
    $strSQL .= ' m_company.INST_CATEGORY_NO, m_company.GROUP_NO';
    $strSQL .= ' FROM m_company';

    $strSQL .= ' INNER JOIN m_group
    ON m_group.GROUP_NO=m_company.GROUP_NO';
    $strSQL .= ' INNER JOIN m_inst_category
    ON m_company.INST_CATEGORY_NO=m_inst_category.INST_CATEGORY_NO';
    //search by company name
    if(isset($_SESSION['txtCompanyName']) && $_SESSION['txtCompanyName'] != '') {
        if($objUserInfo->intLanguageType) {
            $strSQL .= ' WHERE m_company.COMPANY_NAME_ENG LIKE ? ESCAPE \'!\' ';
        } else {
            $strSQL .= ' WHERE m_company.COMPANY_NAME_JPN LIKE ? ESCAPE \'!\' ';
        }
        $blnNext = 1;
        $strTmp = str_replace('!', '!!', $_SESSION['txtCompanyName']);
        $strTmp = str_replace('%', '!%%', $strTmp);

        $strTmp = str_replace('[', '![', $strTmp);
        $strTmp = str_replace(']', '!]', $strTmp);
        $strTmp = str_replace('_', '!_', $strTmp);
        $arrCondition[] = '%'.$strTmp.'%';
    }
    //search by ABBREVIATIONS name
    if(isset($_SESSION['txtAbbreviations']) && $_SESSION['txtAbbreviations'] != '') {
        if(!isset($blnNext)) {
            if($objUserInfo->intLanguageType) {
                $strSQL .= ' WHERE m_company.ABBREVIATIONS_ENG LIKE ? ESCAPE \'!\' ';
            } else {
                $strSQL .= ' WHERE m_company.ABBREVIATIONS_JPN LIKE ? ESCAPE \'!\' ';
            }
        } else {
            if($objUserInfo->intLanguageType) {
                $strSQL .= ' AND m_company.ABBREVIATIONS_ENG LIKE ? ESCAPE \'!\' ';
            } else {
                $strSQL .= ' AND m_company.ABBREVIATIONS_JPN LIKE ? ESCAPE \'!\' ';
            }
        }
        $blnNext = 1;
        $strTmp = str_replace('!', '!!', $_SESSION['txtAbbreviations']);
        $strTmp = str_replace('%', '!%%', $strTmp);

        $strTmp = str_replace('[', '![', $strTmp);
        $strTmp = str_replace(']', '!]', $strTmp);
        $strTmp = str_replace('_', '!_', $strTmp);
        $arrCondition[] = '%'.$strTmp.'%';

    }
    //search by Inst Category
    if(isset($_SESSION['cmbInstCategory']) && trim($_SESSION['cmbInstCategory']) != '') {
        if(!isset($blnNext)) {
            $strSQL .= ' WHERE m_company.INST_CATEGORY_NO=?';
        } else {
            $strSQL .= ' AND m_company.INST_CATEGORY_NO=?';
        }
        $blnNext = 1;
        $arrCondition[] = $_SESSION['cmbInstCategory'];
    }
    //search by group
    if(isset($_SESSION['cmbGroup']) && trim($_SESSION['cmbGroup']) != '') {
        if(!isset($blnNext)) {
            $strSQL .= ' WHERE m_company.GROUP_NO=?';
        } else {
            $strSQL .= ' AND m_company.GROUP_NO=?';
        }
        $blnNext = 1;
        $arrCondition[] = $_SESSION['cmbGroup'];
    }
    $strSQL .= ' AND m_company.DEL_FLAG=0';
    $strSQL .= ' ORDER BY m_company.SORT_NO ASC';

    //mode
    if(isset($_POST['mode'])) {
        //delete
        if($_POST['mode'] == 1) {
            try {
                //button log
                fncWriteLog(
                    LogLevel['Info'],
                    LogPattern['Button'],
                    SCREEN_NAME . ' 削除(ユーザID = '.$objUserInfo->strUserID.')'
                );
                $GLOBALS['g_dbaccess']->funcBeginTransaction();
                //update company del_flag
                $objResult = fncProcessData([
                    'DEL_FLAG' => 1
                ],
                'm_company',
                'COMPANY_NO=?',
                [$_POST['id']],
                SCREEN_NAME);
                if(!is_object($objResult) || $objResult->rowCount() == 0){
                    //rollback
                    $GLOBALS['g_dbaccess']->funcRollback();
                    echo $arrTextTranslate['PUBLIC_MSG_004'];
                    fncWriteLog(
                        LogLevel['Error'],
                        LogPattern['Error'],
                        $arrTextTranslate['PUBLIC_MSG_004']
                    );
                    exit();
                }
                //delete t_int_company_sort
                $strSQL = '';
                $strSQL .= 'DELETE FROM t_inst_company_sort WHERE COMPANY_NO=:COMPANY_NO';
                $query = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
                $query->bindParam(':COMPANY_NO', $_POST['id']);
                $strSqlLog = str_replace(':COMPANY_NO', $_POST['id'], $strSQL);
                //sql log
                fncWriteLog(
                    LogLevel['Info'],
                    LogPattern['Sql'],
                    SCREEN_NAME . ' '.$strSqlLog
                );

                $query->execute();

                $GLOBALS['g_dbaccess']->funcCommit();
                echo 0;
                exit();

            } catch(\Exception $e) {
                //rollback
                $GLOBALS['g_dbaccess']->funcRollback();
                echo $arrTextTranslate['PUBLIC_MSG_004'];
                fncWriteLog(
                    LogLevel['Error'],
                    LogPattern['Error'],
                    $arrTextTranslate['PUBLIC_MSG_004']
                );
                exit();
            }
        }
    }
    //show input error message
    if(
        isset($_SESSION['COMPANY_MNG_MSG_ERROR_INPUT'])
        && $_SESSION['COMPANY_MNG_MSG_ERROR_INPUT']!=''
    ) {
?>
<script>
    $(function() {
        var errStr = '<?php echo $_SESSION['COMPANY_MNG_MSG_ERROR_INPUT'] ?>';
        $('.error').html(errStr);
    });
</script>
<?php
    } else {
        //get company list current page
        $arrCompanyList = fncSelectData(
            $strSQL,
            $arrCondition,
            $GLOBALS['currentPage'],
            true,
            SCREEN_NAME
        );
        if(!is_array($arrCompanyList)){
            //failed to get data
            echo "<script>";
            echo "$('.error').html('".$arrTextTranslate['PUBLIC_MSG_001']."');";
            echo "</script>";
        }

    }
?>

<table class="blueTable">
    <thead>
        <tr>
            <th>
                <?php echo fncHtmlSpecialChars($arrTextTranslate['COMPANY_MNG_TEXT_006']); ?>
            </th>
            <th>
                <?php echo fncHtmlSpecialChars($arrTextTranslate['COMPANY_MNG_TEXT_007']); ?>
            </th>
            <th>
                <?php echo fncHtmlSpecialChars($arrTextTranslate['COMPANY_MNG_TEXT_008']); ?>
            </th>
            <th>
                <?php echo fncHtmlSpecialChars($arrTextTranslate['COMPANY_MNG_TEXT_009']); ?>
            </th>
            <th>
                <?php echo fncHtmlSpecialChars($arrTextTranslate['COMPANY_MNG_TEXT_010']); ?>
            </th>
        </tr>
    </thead>
    <tfoot>
        <tr>
            <td colspan="5">
                <table width="100%;">
                    <tr>
                        <td style="width:30%;">
                           <?php
			                    echo str_replace(
			                        '%1%',
			                        $GLOBALS['totalRecord'],
			                        $arrTextTranslate['PUBLIC_TEXT_016']
			                    );
			                ?>
                        </td>
                        <td style="width:40%;text-align:center;">
                            <div class="links in-line">
			                    <?php fncViewPagination('company_mng_proc.php'); ?>
			                </div>
                        </td>
                        <td style="width:30%;">
                             <div class="in-line" style="float: right">
			                    <button type="submit" class="tbtn tbtn-defaut load-modal"
			                    href="company_edit.php" data-id="0">
			                        <?php echo fncHtmlSpecialChars($arrTextTranslate['PUBLIC_BUTTON_001']); ?>
			                    </button>
			                    <button type="submit" class="tbtn tbtn-defaut"
			                    onclick="window.location.href='portal.php'">
			                        <?php echo fncHtmlSpecialChars($arrTextTranslate['PUBLIC_BUTTON_015']); ?>
			                    </button>
			                </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </tfoot>
    <tbody>
        <?php if(isset($arrCompanyList)
        && is_array($arrCompanyList)) foreach($arrCompanyList as $item) {
            //show list company
            ?>
        <tr>
            <td ><?php echo fncHtmlSpecialChars($item['COMPANY_NAME']); ?></td>
            <td ><?php echo fncHtmlSpecialChars($item['ABBREVIATIONS']); ?></td>
            <td class="text-center">
                <?php echo fncHtmlSpecialChars($item['INST_CATEGORY_NAME']); ?>
            </td>
            <td class="text-center">
                <?php echo fncHtmlSpecialChars($item['GROUP_NAME']); ?>
            </td>
            <td class="text-center">
                <button class="tbl-btn tbtn-green load-modal"
                href="company_edit.php" data-id="<?php echo $item['COMPANY_NO']; ?>">
                    <?php echo $arrTextTranslate['PUBLIC_BUTTON_009']; ?>
                </button>
                <button class="tbl-btn tbtn-red btn-del"
                data-id="<?php echo $item['COMPANY_NO'] ; ?>">
                    <?php echo $arrTextTranslate['PUBLIC_BUTTON_010']; ?>
                </button>
            </td>
        </tr>
    <?php } ?>
    </tbody>
    </tr>
</table>
