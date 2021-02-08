<?php
    /*
     * @user_mng_proc.php
     *
     * @create 2020/03/13 AKB Chien
     * @update
     */
    require_once('common/common.php');
    require_once('common/session_check.php');

    header('Content-type: text/html; charset=utf-8');
    header('X-FRAME-OPTIONS: DENY');

    // check timeout if direct access this file
    fncSessionTimeOutCheck();

    define('DISPLAY_TITLE', 'ユーザ管理画面');

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

    // check if ajax or export csv -> do something | access this file directly -> stop
    if(!(isset($_POST['mode']) && $_POST['mode'] == 1)
        && !(!empty($_SERVER['HTTP_X_REQUESTED_WITH'])
        && strtolower($_SERVER['HTTP_X_REQUESTED_WITH']) == 'xmlhttprequest')) {
        exit;
    }

    // ログインユーザ情報を取得
    $objLoginUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);

    $intLanguageType = $objLoginUserInfo->intLanguageType;

    /**
     * get data search
     *
     * @create 2020/03/13 AKB Chien
     * @update
     * @param string $strUserId
     * @param string $strCompanyName
     * @param string $strGroupNo
     * @param string $strUserName
     * @param integer $intLang
     * @param integer $intPage
     * @param boolean $blnPaging
     * @return array $arrResult
     */
    function fncGetMUser($strUserId = '', $strCompanyName = '', $strGroupNo = '',
                         $strUserName = '', $intLang, $intPage, $blnPaging = false) {
        try {
            $strSuffixes = ($intLang == 0) ? '_JPN' : '_ENG';
            $strCompanyNameSelect = 'company_name'.$strSuffixes;
            $strAbbreviationsSelect = 'abbreviations'.$strSuffixes;
            $strInstCategoryNameSelect = 'inst_category_name'.$strSuffixes;
            $strGroupNameSelect = 'group_name'.$strSuffixes;

            $strSQL = ' SELECT '
                    . '     mu.user_no, '
                    . '     mu.user_id, '
                    . '     mu.password, '
                    . '     FORMAT (mu.expiration_date_s, \'yyyy/M/d\') AS expiration_date_s, '
                    . '     FORMAT (mu.expiration_date_e, \'yyyy/M/d\') AS expiration_date_e, '
                    . $strCompanyNameSelect.' AS company_name, '
                    . $strAbbreviationsSelect.' AS abbreviations, '
                    . $strGroupNameSelect.' AS group_name, '
                    . '     mu.organization, '
                    . '     mu.user_name, '
                    . '     mu.tel, '
                    . '     mu.fax, '
                    . '     mu.mail_address, '
                    . '     mu.zip_code, '
                    . '     mu.address, '
                    . '     mu.remarks, '
                    . '     mu.language_type, '
                    . '     mu.up_date ';

            // if export csv
            if(!$blnPaging) {
                $strSQL .= ' , '.$strInstCategoryNameSelect.' AS inst_category_name ';
            }
            $strSQL .= ' FROM '
                    . '     m_user AS mu '
                    . '     INNER JOIN m_company AS mc ON mu.company_no = mc.company_no '
                    . '         AND mc.del_flag = 0 '
                    . '     INNER JOIN m_group AS mg ON mc.group_no = mg.group_no ';

            // if export csv
            if(!$blnPaging) {
                $strSQL .= ' INNER JOIN m_inst_category AS mic  ';
                $strSQL .= '    ON mic.inst_category_no = mc.inst_category_no ';
            }

            $arrCondition = array();
            $blnHaveAnd = false;
            // if user_id not empty
            if(trim($strUserId) != '') {
                $strSQL .= ' WHERE mu.user_id = ? ';
                $arrCondition[] = $strUserId;
                $blnHaveAnd = true;
            }
            // if company_name not empty
            if(trim($strCompanyName) != '') {
                if($blnHaveAnd) {
                    $strSQL .= ' AND ';
                } else {
                    $strSQL .= ' WHERE ';
                }
                // $strSQL .= ' AND mc.'.$strCompanyNameSelect.' LIKE ? ';
                $strSQL .= ' mc.'.$strCompanyNameSelect.' LIKE ? ESCAPE \'!\' ';
                $strCompanyName = str_replace('!', '!!', $strCompanyName);
                $strCompanyName = str_replace('%', '!%%', $strCompanyName);
                $strCompanyName = str_replace('[', '![', $strCompanyName);
                $strCompanyName = str_replace(']', '!]', $strCompanyName);
                $strCompanyName = str_replace('_', '!_', $strCompanyName);
                $arrCondition[] = '%'.$strCompanyName.'%';
                $blnHaveAnd = true;
            }
            // if group_no <> All
            if(trim($strGroupNo) != '' && trim($strGroupNo) != 'All') {
                if($blnHaveAnd) {
                    $strSQL .= ' AND ';
                } else {
                    $strSQL .= ' WHERE ';
                }
                $strSQL .= ' mc.group_no = ? ';
                $arrCondition[] = $strGroupNo;
                $blnHaveAnd = true;
            }
            // if user_name not empty
            if(trim($strUserName) != '') {
                if($blnHaveAnd) {
                    $strSQL .= ' AND ';
                } else {
                    $strSQL .= ' WHERE ';
                }
                $strSQL .= ' mu.user_name LIKE ? ESCAPE \'!\' ';
                $strUserName = str_replace('!', '!!', $strUserName);
                $strUserName = str_replace('%', '!%%', $strUserName);
                $strUserName = str_replace('[', '![', $strUserName);
                $strUserName = str_replace(']', '!]', $strUserName);
                $strUserName = str_replace('_', '!_', $strUserName);
                $arrCondition[] = $strUserName.'%';
                $blnHaveAnd = true;
            }

            $strSQL .= ' ORDER BY mu.reg_date DESC ';
            // execute SQL and get data
            $arrResult = fncSelectData($strSQL, $arrCondition,
                                        $intPage, $blnPaging, DISPLAY_TITLE);
            return $arrResult;
        } catch (\Exception $e) {
            // write log
            fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                $arrTextTranslate['PUBLIC_MSG_001']);
            $_SESSION['USER_MNG_ERROR'][] = $arrTextTranslate['PUBLIC_MSG_001'];
            return 0;
        }
    }

    $arrTitleMsg =  array(
        'USER_MNG_TEXT_006',
        'USER_MNG_TEXT_007',
        'USER_MNG_TEXT_008',
        'USER_MNG_TEXT_009',
        'USER_MNG_TEXT_010',
        'USER_MNG_TEXT_011',
        'PUBLIC_BUTTON_009',
        'USER_MNG_TEXT_012',
        'PUBLIC_BUTTON_001',
        'PUBLIC_BUTTON_011',
        'PUBLIC_BUTTON_015',
        'PUBLIC_TEXT_016',
        // get data grid = false
        'PUBLIC_MSG_001',
        // csv title
        'USER_MNG_TEXT_013',
        'USER_MNG_TEXT_014',
        'USER_MNG_TEXT_015',
        'USER_MNG_TEXT_016',
        'USER_MNG_TEXT_017',
        'USER_MNG_TEXT_018',
        'USER_MNG_TEXT_019',
        'USER_MNG_TEXT_020',
        'USER_MNG_TEXT_021',
        'USER_MNG_TEXT_022',
        'USER_MNG_TEXT_023',
        'USER_MNG_TEXT_024',
        'USER_MNG_TEXT_025',
        'USER_MNG_TEXT_026',
        'USER_MNG_TEXT_027',
        'USER_MNG_TEXT_028',
        'PUBLIC_TEXT_006',
        'PUBLIC_TEXT_007',
        // export data csv = false
        'PUBLIC_MSG_005',

        // 2020/03/30 AKB Chien - start - update document 2020/03/30
        'USER_MNG_TEXT_029',
        'USER_MNG_TEXT_030',
        'PUBLIC_MSG_049',
        // 2020/03/30 AKB Chien - end - update document 2020/03/30

        // 2020/04/20 AKB Chien - start - update document 2020/04/20
        'USER_MNG_TEXT_031',
        // 2020/04/20 AKB Chien - end - update document 2020/04/20
    );

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

    if(isset($_POST)) {
        if(isset($_POST['mode'])) {
            // export csv
            if($_POST['mode'] == 1) {
                $strUserIdSearch = $_SESSION['USER_MNG_SEARCH_USERID'];
                $strCompanyNameSearch = $_SESSION['USER_MNG_SEARCH_COMPANY'];
                $strGroupNoSearch = $_SESSION['USER_MNG_SEARCH_GROUP'];
                $strUserNameSearch = $_SESSION['USER_MNG_SEARCH_USER_NAME'];

                $strSuffixes = ($intLanguageType == 0) ? '_JPN' : '_ENG';
                $strGroupName = 'group_name'.$strSuffixes;

                if($strGroupNoSearch == '') {
                    $strGroupName = 'All';
                } else {
                    $strSQL = ' SELECT '
                            . '     group_no, '
                            . $strGroupName.' AS group_name '
                            . ' FROM '
                            . '     m_group '
                            . ' WHERE '
                            . '     m_group.group_no = ? ';
                    // get name of group to log
                    $arrOneGroup = fncSelectOne($strSQL, array($strGroupNoSearch), DISPLAY_TITLE);
                    if($arrOneGroup != 0 && count($arrOneGroup) > 0) {
                        $strGroupName = $arrOneGroup['group_name'];
                    } else {
                        $strGroupName = 'All';
                    }
                }

                // <CSV出力時>
                $strLog = DISPLAY_TITLE.'　CSV出力「ユーザID = '.$strUserIdSearch.',';
                $strLog .= '会社名 = '.$strCompanyNameSearch.',';
                $strLog .= 'グループ = '.$strGroupName.',';
                $strLog .= '担当者名 = '.$strUserNameSearch.'」';
                $strLog .= '(ユーザID = '.$objLoginUserInfo->strUserID.')';
                fncWriteLog(LogLevel['Info'], LogPattern['Button'], $strLog);
                // get list user
                $arrDataCSV = fncGetMUser($strUserIdSearch, $strCompanyNameSearch,
                                          $strGroupNoSearch, $strUserNameSearch, $intLanguageType, 1, false);
                $arrDataContent = array();

                // if has error msg
                if($arrDataCSV == 0) {
                    fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                        $arrTextTranslate['PUBLIC_MSG_005']);
                    $_SESSION['USER_MNG_ERROR'][] = $arrTextTranslate['PUBLIC_MSG_005'];
                    header('Location: user_mng.php');
                    exit;
                }

                // has data
                if($arrDataCSV != 0 && count($arrDataCSV) > 0) {
                    // prepare data to import csv file
                    foreach($arrDataCSV as $item) {
                        $strLanguageType = $arrTextTranslate['PUBLIC_TEXT_007'];
                        if($item['language_type'] == 0) {
                            $strLanguageType = $arrTextTranslate['PUBLIC_TEXT_006'];
                        }

                        $strAddress = '';
                        if(is_null($item['address']) || $item['address'] == '') {
                            $strAddress = '';
                        } else {
                            $strAddress = $item['address'];
                        }

                        $strRemarks = '';
                        if(is_null($item['remarks']) || $item['remarks'] == '') {
                            $strRemarks = '';
                        } else {
                            $strRemarks = $item['remarks'];
                        }

                        $strUpdate = '';
                        if(!is_null($item['up_date'])) {
                            $strUpdate = date_format(date_create($item['up_date']), 'Y/n/j');
                        }

                        // data in file csv
                        $arrDataContent[] = [
                            $item['user_id'],
                            $item['password'],
                            // 2020/04/20 AKB Chien - start - update document 2020/04/20
                            // $item['expiration_date_s'].'～'.$item['expiration_date_e'],
                            $item['expiration_date_s'],
                            $item['expiration_date_e'],
                            // 2020/04/20 AKB Chien - end - update document 2020/04/20
                            iff(is_null($item['company_name']), '', $item['company_name']),
                            iff(is_null($item['abbreviations']), '', $item['abbreviations']),
                            iff(is_null($item['inst_category_name']), '', $item['inst_category_name']),
                            iff(is_null($item['group_name']), '', $item['group_name']),
                            iff(is_null($item['organization']), '', $item['organization']),
                            iff(is_null($item['user_name']), '', $item['user_name']),
                            iff(is_null($item['tel']), '', $item['tel']),
                            iff(is_null($item['fax']), '', $item['fax']),
                            iff(is_null($item['mail_address']), '', $item['mail_address']),
                            iff(is_null($item['zip_code']), '', $item['zip_code']),
                            $strAddress,
                            $strRemarks,
                            $strLanguageType,
                            $strUpdate
                        ];
                    }

                    // header in csv file
                    $arrHeaderTitle = array([
                        $arrTextTranslate['USER_MNG_TEXT_013'],
                        $arrTextTranslate['USER_MNG_TEXT_014'],
                        $arrTextTranslate['USER_MNG_TEXT_015'],
                        // 2020/04/20 AKB Chien - start - update document 2020/04/20
                        $arrTextTranslate['USER_MNG_TEXT_031'],
                        // 2020/04/20 AKB Chien - end - update document 2020/04/20
                        $arrTextTranslate['USER_MNG_TEXT_016'],
                        $arrTextTranslate['USER_MNG_TEXT_017'],
                        $arrTextTranslate['USER_MNG_TEXT_018'],
                        $arrTextTranslate['USER_MNG_TEXT_019'],
                        $arrTextTranslate['USER_MNG_TEXT_020'],
                        $arrTextTranslate['USER_MNG_TEXT_021'],
                        $arrTextTranslate['USER_MNG_TEXT_022'],
                        $arrTextTranslate['USER_MNG_TEXT_023'],
                        $arrTextTranslate['USER_MNG_TEXT_024'],
                        $arrTextTranslate['USER_MNG_TEXT_025'],
                        $arrTextTranslate['USER_MNG_TEXT_026'],
                        $arrTextTranslate['USER_MNG_TEXT_027'],
                        $arrTextTranslate['USER_MNG_TEXT_028'],
                        $arrTextTranslate['USER_MNG_TEXT_030']
                    ]);

                    try {
                        // 「CSV出力」ボタンクリック後の処理
                        fncArray2Csv(array_merge($arrHeaderTitle, $arrDataContent),
                                                'ユーザ_'.date("Ymd-His").'.csv', 1);
                        return 1;
                    } catch (\Exception $e) {
                        // write log
                        fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                                        $arrTextTranslate['PUBLIC_MSG_005']);
                        $_SESSION['USER_MNG_ERROR'][] = $arrTextTranslate['PUBLIC_MSG_005'];
                        header('Location: user_mng.php');
                        exit;
                    }
                } else {
                    $_SESSION['USER_MNG_ERROR'][] = $arrTextTranslate['PUBLIC_MSG_005'];
                    header('Location: user_mng.php');
                    exit;
                }
            }
        }

        if(isset($_POST['loadList'])) {
            $strEvent = null;
            if(isset($_POST['event']) && trim($_POST['event']) != '') {
                $strEvent = trim($_POST['event']);
            }
            if(isset($_POST['page'])) $_SESSION['USER_MNG_PAGE'] = $_POST['page'];
            $GLOBALS['currentPage'] = $_SESSION['USER_MNG_PAGE'];

            $_SESSION['USER_MNG_ERROR'] = array();
            $strLog = '';
            $strLogPattern = '';
            if($strEvent == 0) {
            } else {
                $strLogPattern = 'Button';
                // save to session
                if(isset($_POST['userId'])) {
                    $_SESSION['USER_MNG_SEARCH_USERID'] = trim($_POST['userId']);
                }

                if(isset($_POST['companyName'])) {
                    $_SESSION['USER_MNG_SEARCH_COMPANY'] = trim($_POST['companyName']);
                }

                if(isset($_POST['groupNo'])) {
                    $_SESSION['USER_MNG_SEARCH_GROUP'] = trim($_POST['groupNo']);
                }

                if(isset($_POST['userName'])) {
                    $_SESSION['USER_MNG_SEARCH_USER_NAME'] = trim($_POST['userName']);
                }

                $strSuffixes = ($intLanguageType == 0) ? '_JPN' : '_ENG';
                $strGroupName = 'group_name'.$strSuffixes;

                if($_SESSION['USER_MNG_SEARCH_GROUP'] == '') {
                    $strGroupName = 'All';
                } else {
                    $strSQL = ' SELECT '
                            . '     group_no, '
                            . $strGroupName.' AS group_name '
                            . ' FROM '
                            . '     m_group '
                            . ' WHERE '
                            . '     m_group.group_no = ? ';
                    // get name of group to log
                    $arrOneGroup = fncSelectOne($strSQL, array($_SESSION['USER_MNG_SEARCH_GROUP']),
                                                DISPLAY_TITLE);
                    if($arrOneGroup != 0 && count($arrOneGroup) > 0) {
                        $strGroupName = $arrOneGroup['group_name'];
                    } else {
                        $strGroupName = 'All';
                    }
                }

                // <ユーザ情報検索時>
                if(isset($_POST['originalSearch']) && $_POST['originalSearch'] == 1) {
	                $strLog = DISPLAY_TITLE.' 検索を実施「ユーザID = '.$_SESSION['USER_MNG_SEARCH_USERID'].',';
	                $strLog .= '会社名 = '. $_SESSION['USER_MNG_SEARCH_COMPANY'].',';
	                $strLog .= 'グループ = '. $strGroupName.',';
	                $strLog .= '担当者名 = '.$_SESSION['USER_MNG_SEARCH_USER_NAME'].'」';
	                $strLog .= '(ユーザID = '.$objLoginUserInfo->strUserID.')';
	                fncWriteLog(LogLevel['Info'], LogPattern[$strLogPattern], $strLog);

	                $GLOBALS['currentPage'] = 1;
	                $_SESSION['USER_MNG_PAGE'] = 1;
	           }
            }

            // 下記条件にてユーザ情報を検索する
            $arrData = fncGetMUser($_SESSION['USER_MNG_SEARCH_USERID'],
                                   $_SESSION['USER_MNG_SEARCH_COMPANY'],
                                   $_SESSION['USER_MNG_SEARCH_GROUP'],
                                   $_SESSION['USER_MNG_SEARCH_USER_NAME'],
                                   $intLanguageType, $GLOBALS['currentPage'], true);
        ?>
            <table class="blueTable">
                <thead>
                    <tr>
                        <th class="text-th"><?php
                            echo $arrTextTranslate['USER_MNG_TEXT_006'];
                        ?></th>
                        <th class="text-th"><?php
                            echo $arrTextTranslate['USER_MNG_TEXT_007'];
                        ?></th>
                        <?php // 2020/04/20 AKB Chien - start - update document 2020/04/20 ?>
                        <th class="text-th"><?php
                            echo $arrTextTranslate['USER_MNG_TEXT_009'];
                        ?></th>
                        <th class="text-th"><?php
                            echo $arrTextTranslate['USER_MNG_TEXT_008'];
                        ?></th>
                        <?php // 2020/04/20 AKB Chien - end - update document 2020/04/20 ?>
                        <th class="text-th"><?php
                            echo $arrTextTranslate['USER_MNG_TEXT_010'];
                        ?></th>
                        <?php // 2020/03/30 AKB Chien - start - update document 2020/03/30 ?>
                        <th class="text-th"><?php
                            echo $arrTextTranslate['USER_MNG_TEXT_029'];
                        ?></th>
                        <?php // 2020/03/30 AKB Chien - end - update document 2020/03/30 ?>
                        <th class="text-th"><?php
                            echo $arrTextTranslate['USER_MNG_TEXT_011'];
                        ?></th>
                    </tr>
                </thead>
                <tfoot>
                    <tr>
                    	<td colspan="7">
                    		<table width="100%;">
                    			<tr>
                    				<td style="width:30%;">
                                        <?php
                    						echo str_replace('%1%', $GLOBALS['totalRecord'],
                    								$arrTextTranslate['PUBLIC_TEXT_016']);
                                        ?>
                    				</td>
                    				<td style="width:40%;text-align:center;">
                    					<div class="links in-line">
                    						<?php fncViewPagination('user_mng_proc.php'); ?>
                    					</div>
                    				</td>
                    				<td style="width:30%;">
                    					<div class="in-line" style="float: right">
			                                <form action="user_mng_proc.php" method="post" id="formCSV">
			                                    <input type="hidden" name="searchData" value="" />
			                                    <input type="hidden" name="X-CSRF-TOKEN" value="<?php echo $_POST['X-CSRF-TOKEN']; ?>" />
							                    <input type="hidden" name="mode" value="2" />
			                                    <button type="button" class="tbtn tbtn-defaut load-modal" href="user_edit.php" data-id="">
			                                    	<?php
			                                    		echo $arrTextTranslate['PUBLIC_BUTTON_001'];
			                                    	?>
			                                    </button>
			                                    <button type="button" class="tbtn tbtn-defaut btnOutput">
			                                    	<?php echo $arrTextTranslate['PUBLIC_BUTTON_011']; ?>
			                                    </button>
			                                    <button type="button" class="tbtn tbtn-defaut tbn-btn-return">
			                                    	<?php
			                                    		echo $arrTextTranslate['PUBLIC_BUTTON_015'];
			                                    	?>
			                                    </button>
			                                </form>
			                            </div>
                    				</td>
                    			</tr>
                    		</table>
                    	</td>
                    </tr>
                </tfoot>
                <tbody>
                    <?php
                        if($arrData != 0 && count($arrData) > 0) {
                            foreach ($arrData as $value) {
                    ?>
                        <tr>
                            <td class="text-center"><?php
                                echo fncHtmlSpecialChars($value['user_id']);
                            ?></td>
                            <td class="text-center">
                                <?php
                                    $strTextShow = fncHtmlSpecialChars($value['expiration_date_s']).'～';
                                    $strTextShow .= fncHtmlSpecialChars($value['expiration_date_e']);
                                    echo $strTextShow;
                                ?>
                            </td>
                            <?php // 2020/04/20 AKB Chien - start - update document 2020/04/20 ?>
                            <td class="text-center"><?php
                                echo fncHtmlSpecialChars($value['group_name']);
                            ?></td>
                            <td class="text-center"><?php
                                echo fncHtmlSpecialChars($value['company_name']);
                            ?></td>
                            <?php // 2020/04/20 AKB Chien - end - update document 2020/04/20 ?>
                            <td class="text-center"><?php
                                echo fncHtmlSpecialChars($value['user_name']);
                            ?></td>
                            <td class="text-center"><?php
                                if($value['up_date'] != '') {
                                    echo date_format(date_create($value['up_date']), 'Y/n/j');
                                }
                            ?></td>
                            <td class="text-center">
                                <button class="tbl-btn tbtn-green load-modal"
                                    data-id="<?php echo $value['user_no']; ?>" href="user_edit.php"><?php
                                    echo $arrTextTranslate['PUBLIC_BUTTON_009'];
                                ?></button>
                                <button class="tbl-btn tbtn-blue load-modal"
                                    data-id="<?php echo $value['user_no']; ?>" href="user_perm.php"><?php
                                    echo $arrTextTranslate['USER_MNG_TEXT_012'];
                                ?></button>
                            </td>
                        </tr>
                    <?php
                            }
                        }
                    ?>
                </tbody>
            </table>
            <script>
                $(function() {
<?php
            $arrErrorMsg = array();
            if(isset($_SESSION['USER_MNG_ERROR'])) {
                $arrErrorMsg = $_SESSION['USER_MNG_ERROR'];
            }
            // if has error
            if($arrData == 0) {
                fncWriteLog(LogLevel['Error'], LogPattern['Error'],
                            $arrTextTranslate['PUBLIC_MSG_001']);
                $arrErrorMsg[] = $arrTextTranslate['PUBLIC_MSG_001'];
            }
            $strHtml = '';
            if(count($arrErrorMsg) > 0) {
                $strHtml = '';
                foreach($arrErrorMsg as $value) {
                    $strHtml .= '<div>'.$value.'</div>';
                }
?>
                    $('.error').append('<?php echo $strHtml; ?>');
<?php       } ?>
                });
            </script>
<?php
        }
    }
?>