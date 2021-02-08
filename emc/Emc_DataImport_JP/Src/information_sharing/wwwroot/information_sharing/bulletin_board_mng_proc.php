<?php

	/*
	* @bulletin_board_mng_proc.php
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
	//create Session for error message
	$_SESSION['BULLETIN_BOARD_MNG_ERROR_MSG'] = '';
	define('SCREEN_NAME', '掲示板管理画面');
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
	$objUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);



	// get list text translate
	$arrTitle =  array(
		'BULLETIN_BOARD_MNG_TEXT_005',
		'BULLETIN_BOARD_MNG_TEXT_006',
		'BULLETIN_BOARD_MNG_TEXT_007',
		'BULLETIN_BOARD_MNG_TEXT_008',
		'BULLETIN_BOARD_MNG_TEXT_009',
		'BULLETIN_BOARD_MNG_TEXT_010',
		'BULLETIN_BOARD_MNG_TEXT_012',
		'BULLETIN_BOARD_MNG_TEXT_013',
		'BULLETIN_BOARD_MNG_TEXT_014',
		'BULLETIN_BOARD_MNG_TEXT_015',
		'BULLETIN_BOARD_MNG_TEXT_016',
		'BULLETIN_BOARD_MNG_TEXT_017',
		'BULLETIN_BOARD_MNG_TEXT_018',
		'BULLETIN_BOARD_MNG_TEXT_019',
		'BULLETIN_BOARD_MNG_TEXT_020',
		'BULLETIN_BOARD_MNG_TEXT_021',
		'BULLETIN_BOARD_MNG_TEXT_022',
		'BULLETIN_BOARD_MNG_TEXT_023',
		'BULLETIN_BOARD_MNG_TEXT_024',
		'BULLETIN_BOARD_MNG_TEXT_025',
		'BULLETIN_BOARD_MNG_TEXT_026',
		'BULLETIN_BOARD_MNG_TEXT_027',
		'BULLETIN_BOARD_MNG_MSG_003',
		'BULLETIN_BOARD_MNG_MSG_002',
		'PUBLIC_BUTTON_010',
		'PUBLIC_BUTTON_007',
		'PUBLIC_BUTTON_011',
		'PUBLIC_BUTTON_012',
		'PUBLIC_BUTTON_015',
		'PUBLIC_MSG_004',
		'PUBLIC_MSG_007',
		'PUBLIC_MSG_001',
		'PUBLIC_MSG_005',
		'PUBLIC_MSG_006',
		'PUBLIC_TEXT_016',
		'PUBLIC_MSG_049',
		'BULLETIN_BOARD_MNG_TEXT_011'
	);

	// get list text(header, title, msg) with languague_type of user logged
	$arrTextTranslate = getListTextTranslate(
		$arrTitle,
		$objUserInfo->intLanguageType
	);


	//check get request
	fncGetRequestCheck($arrTextTranslate);
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
	//if request directly -> exit(), except when exporting csv
	if(
		!(!empty($_SERVER['HTTP_X_REQUESTED_WITH'])
		&& strtolower($_SERVER['HTTP_X_REQUESTED_WITH']) == 'xmlhttprequest')
	)
	{
		if(!(isset($_POST['mode']) && $_POST['mode'] == 2))
		exit;
	}
	//load list data

	// clear session info
	if(
		!isset($_POST['delCsv']) && !isset($_POST['del'])
		&& !isset($_POST['page']) && !isset($_POST['mode'])
		&& !isset($_SESSION['BULLETIN_BOARD_MNG_REDIRECT'])
	) {
		$_SESSION['txtIncident'] = '';
		$_SESSION['txtDateStart'] = '';
		$_SESSION['txtDateEnd'] = '';
		$_SESSION['chkDone'] = '';

	}

	//unset
	if(isset($_SESSION['BULLETIN_BOARD_MNG_REDIRECT'])) {
		unset($_SESSION['BULLETIN_BOARD_MNG_REDIRECT']);
	}

	//store search value to session
	if(isset($_POST['loadList']) || isset($_POST['chkDone'])) {

	    if(isset($_POST['page'])) $_SESSION['BULLETIN_BOARD_MNG_PAGE'] = $_POST['page'];
	    $GLOBALS['currentPage'] = $_SESSION['BULLETIN_BOARD_MNG_PAGE'];

		if(isset($_POST['txtIncident'])) {
			$_SESSION['txtIncident'] = $_POST['txtIncident'];
		}

		if(isset($_POST['txtDateStart'])) {
			$_SESSION['txtDateStart'] = $_POST['txtDateStart'];
		}

		if(isset($_POST['txtDateEnd'])) {
			$_SESSION['txtDateEnd'] = $_POST['txtDateEnd'];

		}
		if(isset($_POST['chkDone'])) {
			$_SESSION['chkDone'] = $_POST['chkDone'];
		}
	}

	$arrTextTranslate = getListTextTranslate(
		$arrTitle,
		$objUserInfo->intLanguageType
	);
	//sql query

	$arrCondition = [];
	$strSQL = '';
	$strSQL .= ' SELECT t_bulletin_board.BULLETIN_BOARD_NO,';
	$strSQL .= ' t_bulletin_board.OCCURRENCE_DATE,';
	$strSQL .= ' t_bulletin_board.UP_DATE,';
	$strSQL .= ' t_bulletin_board.CORRECTION_FLAG,';
	$strSQL .= ' t_bulletin_board.BUSINESS_NAME as BUSINESS_NAME_ORG,';
	//Get company name, incident name and place name by user language
	if($objUserInfo->intLanguageType) {
		$strSQL .= ' m_business_name.BUSINESS_NAME_ENG as BUSINESS_NAME,
		m_business_name.BUSINESS_NAME_ENG, m_business_name.BUSINESS_NAME_JPN,';
		$strSQL .= ' t_bulletin_board.INCIDENT_NAME_ENG as INCIDENT_NAME,
		t_bulletin_board.INCIDENT_NAME_ENG, t_bulletin_board.INCIDENT_NAME_JPN,';
		$strSQL .= ' m_place_name.PLACE_NAME_ENG as PLACE_NAME,
		m_place_name.PLACE_NAME_ENG, m_place_name.PLACE_NAME_JPN,';
	} else {
		$strSQL .= ' m_business_name.BUSINESS_NAME_JPN as BUSINESS_NAME,
		m_business_name.BUSINESS_NAME_ENG, m_business_name.BUSINESS_NAME_JPN,';
		$strSQL .= ' t_bulletin_board.INCIDENT_NAME_JPN as INCIDENT_NAME,
		t_bulletin_board.INCIDENT_NAME_ENG, t_bulletin_board.INCIDENT_NAME_JPN,';
		$strSQL .= ' m_place_name.PLACE_NAME_JPN as PLACE_NAME,
		m_place_name.PLACE_NAME_ENG, m_place_name.PLACE_NAME_JPN,';
	}
	$strSQL .= ' t_bulletin_board.CORRECTION_FLAG,';
	$strSQL .= ' t_bulletin_board.COMP_DATE,';
	//$strSQL .= ' t_bulletin_board.REG_DATE,';
	$strSQL .= ' t_bulletin_board.REG_DATE';
	//$strSQL .= ' m_user.USER_NAME,';
	//$strSQL .= ' m_user.DELETE_FLAG as user_delete_flag';

	$strSQL .= ' FROM t_bulletin_board ';
	//$strSQL .= ' FROM t_bulletin_board INNER JOIN m_user';
	//$strSQL .= ' ON t_bulletin_board.REG_USER_NO=m_user.USER_NO';

	$strSQL .= ' LEFT OUTER JOIN m_business_name';
	$strSQL .= ' ON t_bulletin_board.BUSINESS_NAME=
	m_business_name.BUSINESS_NAME_JPN';

	$strSQL .= ' LEFT OUTER JOIN m_place_name';
	$strSQL .= ' ON t_bulletin_board.place3_id=m_place_name.place3_id';
	//search by Incident name
	if(isset($_SESSION['txtIncident']) && $_SESSION['txtIncident'] != '') {
		if($objUserInfo->intLanguageType) {
			$strSQL .= ' WHERE INCIDENT_NAME_ENG  LIKE ? ESCAPE \'!\' ';
		} else {
			$strSQL .= ' WHERE INCIDENT_NAME_JPN LIKE ? ESCAPE \'!\' ';
		}
		$blnNext = 1;

		$strTmp = str_replace('!', '!!', $_SESSION['txtIncident']);
        $strTmp = str_replace('%', '!%%', $strTmp);

        $strTmp = str_replace('[', '![', $strTmp);
        $strTmp = str_replace(']', '!]', $strTmp);
        $strTmp = str_replace('_', '!_', $strTmp);
		$arrCondition[] = '%'.$strTmp.'%';
	}
	$blnCheckSDate = true;
	$blnCheckEDate = true;
	//search by Date Start
	if(isset($_SESSION['txtDateStart']) && $_SESSION['txtDateStart'] != '') {

		$blnCheckDate = checkFormatDateTimeInput($_SESSION['txtDateStart']);
		if($blnCheckDate) {
			if(isset($blnNext)) {
				$strSQL .= ' AND CAST(OCCURRENCE_DATE AS DATE) >= ?';
			} else {
				$strSQL .= ' WHERE CAST(OCCURRENCE_DATE AS DATE) >= ?';
			}
			$blnNext = 1;
			$arrCondition[] = $_SESSION['txtDateStart'];
		} else {
			$blnCheckSDate = false;
		}
	}

	//Search by Date end
	if(isset($_SESSION['txtDateEnd']) && $_SESSION['txtDateEnd'] != '') {
		$blnCheckDate = checkFormatDateTimeInput($_SESSION['txtDateEnd']);
		if($blnCheckDate) {
			if(isset($blnNext)) {
				$strSQL .= ' AND CAST(COMP_DATE AS DATE) <= ?';
			} else {
				$strSQL .= ' WHERE CAST(COMP_DATE AS DATE) <= ?';
			}
			$blnNext = 1;
			$arrCondition[] = $_SESSION['txtDateEnd'];
		} else {
			$blnCheckEDate = false;
		}
	}

	// Validate input of start date and end date
	if(!$blnCheckSDate || !$blnCheckEDate
	|| (!$blnCheckSDate && !$blnCheckEDate)) {
		$_SESSION['BULLETIN_BOARD_MNG_ERROR_MSG']
			.= $arrTextTranslate['BULLETIN_BOARD_MNG_MSG_002'].'<br>';
	}

	//Search by status
	if(isset($_SESSION['chkDone']) && $_SESSION['chkDone'] != '') {
		if(isset($blnNext)) {
			$strSQL .= ' AND COMP_DATE IS NOT NULL';
		} else {
			$strSQL .= ' WHERE COMP_DATE IS NOT NULL';
		}
	}
	//Start date must < End date
	if(
		$_SESSION['txtDateStart'] != ''
		&& $_SESSION['txtDateEnd'] != ''
	) {
		if(
			checkFormatDateTimeInput($_SESSION['txtDateStart'])
			&& checkFormatDateTimeInput($_SESSION['txtDateEnd'])
		) {
			$arrTmpDateStart = explode(' ', $_SESSION['txtDateStart']);
			$arrTmpDateEnd = explode(' ', $_SESSION['txtDateEnd']);
			//▼2020/06/08 KBS S.Tasaki 日付の比較を文字列型ではなく日付型にて比較するよう修正
			if(strtotime($arrTmpDateStart[0]) > strtotime($arrTmpDateEnd[0])) {
				$_SESSION['BULLETIN_BOARD_MNG_ERROR_MSG']
				.= $arrTextTranslate['BULLETIN_BOARD_MNG_MSG_003'].'<br>';
			}
			//▲2020/06/08 KBS S.Tasaki
		}
	}

	$strSQL .= ' ORDER BY t_bulletin_board.OCCURRENCE_DATE DESC';
	if($_SESSION['BULLETIN_BOARD_MNG_ERROR_MSG']=='') {
		// Save sql to session
		$_SESSION['strSQLBackup'] = $strSQL;
	}else{
		// Use lastest sql from session
		$strSQL = $_SESSION['strSQLBackup'];
	}
	//mode
	if(isset($_POST['mode'])) {
		//Process delete a bulletin board
		if($_POST['mode'] == 1) {
			try{
				//button delete log
				fncWriteLog(
					LogLevel['Info'] ,
					LogPattern['Button'],
					SCREEN_NAME . '　削除(ユーザID = '.$objUserInfo->strUserID.')'
				);

				$strSQL = '';
				$strSQL .= 'DELETE FROM t_bulletin_board
				WHERE BULLETIN_BOARD_NO=:BULLETIN_BOARD_NO';
				$query = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
				$query->bindParam(':BULLETIN_BOARD_NO', $_POST['id']);
				//sql log
				$strSqlLog = str_replace(':BULLETIN_BOARD_NO', $_POST['id'], $strSQL);
				fncWriteLog(
					LogLevel['Info'] ,
					LogPattern['Sql'],
					SCREEN_NAME . ' '.$strSqlLog
				);

				$query->execute();
				echo 0;
				exit();
			}catch(\Exception $e) {
				echo $arrTextTranslate['PUBLIC_MSG_004'];
				fncWriteLog(
					LogLevel['Error'],
					LogPattern['Error'],
					SCREEN_NAME . ' ' . $e->getMessage()
				);
				exit();
			}


		} else {
			if($_POST['mode'] == 2) {

			    //button log
			    $strSearchQuery = '「インシデント件名 = '.$_SESSION['txtIncident']
			    .',期間（開始）= '.$_SESSION['txtDateStart']
			    .',期間（終了） = '.$_SESSION['txtDateEnd']
			    .', 完了を表示 = '.iff($_SESSION['chkDone']!='',1,0).'」';
			    fncWriteLog(
			        LogLevel['Info'],
			        LogPattern['Button'],
			        SCREEN_NAME . '　CSV出力 '.$strSearchQuery.'(ユーザID = '.$objUserInfo->strUserID.')'
			        );
			}else if($_POST['mode'] == 3){
			    $strSearchQuery = '「インシデント件名 = '.$_SESSION['txtIncident']
			    .',期間（開始）= '.$_SESSION['txtDateStart']
	       	    .',期間（終了） = '.$_SESSION['txtDateEnd']
			    .', 完了を表示 = '.iff($_SESSION['chkDone']!='',1,0).'」';
			    //button log
			    fncWriteLog(
				LogLevel['Info'],
				LogPattern['Button'],
				SCREEN_NAME . '　一括削除 '.$strSearchQuery.'(ユーザID = '.$objUserInfo->strUserID.')'
			    );
			}

			// Get display data of list
			$arrTemp = fncSelectData(
				$strSQL, $arrCondition, $GLOBALS['currentPage'], false, SCREEN_NAME, false
			);

			$arr = [];
			//get data successfully, store data to csv format
			if(is_array($arrTemp))
			foreach($arrTemp as $item) {
				$arr[] = [
					$item['BUSINESS_NAME_JPN'] != '' ? $item['BUSINESS_NAME_JPN']
					: $item['BUSINESS_NAME_ORG'],
					$item['BUSINESS_NAME_ENG'],
					$item['INCIDENT_NAME_JPN'],
					$item['INCIDENT_NAME_ENG'],
					$item['PLACE_NAME_JPN'],
					$item['PLACE_NAME_ENG'],
					date("Y/m/d H:i", strtotime($item['OCCURRENCE_DATE'])),
					iff(
						is_null($item['COMP_DATE']),
						'',
						date("Y/m/d H:i", strtotime($item['COMP_DATE']))
					),
					!is_null($item['UP_DATE'])
					? date_format(date_create($item['UP_DATE']), 'Y/m/d H:i')
					: '',
					$item['CORRECTION_FLAG'] == 0
					? $arrTextTranslate['BULLETIN_BOARD_MNG_TEXT_023'] : '',
					!is_null($item['COMP_DATE'])
					? $arrTextTranslate['BULLETIN_BOARD_MNG_TEXT_027'] : ''
				];
			}
			//csv header
			$arrHeaderCsv = array([
				$arrTextTranslate['BULLETIN_BOARD_MNG_TEXT_012'],
				$arrTextTranslate['BULLETIN_BOARD_MNG_TEXT_013'],

				$arrTextTranslate['BULLETIN_BOARD_MNG_TEXT_016'],
				$arrTextTranslate['BULLETIN_BOARD_MNG_TEXT_017'],

				$arrTextTranslate['BULLETIN_BOARD_MNG_TEXT_014'],
				$arrTextTranslate['BULLETIN_BOARD_MNG_TEXT_015'],

				$arrTextTranslate['BULLETIN_BOARD_MNG_TEXT_024'],
				$arrTextTranslate['BULLETIN_BOARD_MNG_TEXT_025'],
				$arrTextTranslate['BULLETIN_BOARD_MNG_TEXT_019'],
				$arrTextTranslate['BULLETIN_BOARD_MNG_TEXT_022'],
				$arrTextTranslate['BULLETIN_BOARD_MNG_TEXT_026'],
			]);
			//export csv
			if($_POST['mode'] == 2) {



				//failed to get data, redirect back and show error
				if(!is_array($arrTemp) || !count($arrTemp)) {
					$_SESSION['BULLETIN_BOARD_MNG_ERROR'] = $arrTextTranslate['PUBLIC_MSG_005'];
					$_SESSION['BULLETIN_BOARD_MNG_REDIRECT'] = 1;
					fncWriteLog(
						LogLevel['Error'] ,
						LogPattern['Error'],
						$arrTextTranslate['PUBLIC_MSG_005']
					);
					header('Location: bulletin_board_mng.php');
				}
				$arr = array_merge($arrHeaderCsv, $arr);
				//call csv export function
				$blnCsvExportCheck = fncArray2Csv($arr, '掲示板_'.date("Ymd").'-'.date("His").'.csv', 1, null);

				//export csv successfully
				if($blnCsvExportCheck) {
					exit();
				} else {
					//failed to export csv, redirect back and show error
					$_SESSION['BULLETIN_BOARD_MNG_ERROR'] = $arrTextTranslate['PUBLIC_MSG_005'];
					$_SESSION['BULLETIN_BOARD_MNG_REDIRECT'] = 1;
					fncWriteLog(
						LogLevel['Error'] ,
						LogPattern['Error'],
						$arrTextTranslate['PUBLIC_MSG_005']
					);
					header('Location: bulletin_board_mng.php');
				}
			}
			//batched delete and store csv file
			if($_POST['mode'] == 3) {
				if(!is_array($arrTemp) || !count($arrTemp)) {
					echo "$('.error').html('".$arrTextTranslate['PUBLIC_MSG_007']."');";
					fncWriteLog(
						LogLevel['Error'],
						LogPattern['Error'],
						$arrTextTranslate['PUBLIC_MSG_007']
					);
					exit();
				}
				$dtmDatetime = date("Ymd").'-'.date("His");
				$filename = $dtmDatetime.'.csv';

				$arr = array_merge($arrHeaderCsv, $arr);

				$blnCsvExportCheck = fncArray2Csv(
					$arr, $filename, 0, SHARE_FOLDER_ORGANIZE .'/'
					.ORGANIZE_BULLETIN_BOARD_FOLDER . '/' .$dtmDatetime
				);
				if($blnCsvExportCheck) {
					//save csv successfully, delete data
					try{
						//begin transaction
						$GLOBALS['g_dbaccess']->funcBeginTransaction();
						//delete each item
						foreach($arrTemp as $item) {
							$strSQL = '';
							$strSQL .= 'DELETE FROM t_bulletin_board
							WHERE BULLETIN_BOARD_NO=:BULLETIN_BOARD_NO';
							$query = $GLOBALS['g_dbaccess']->funcPrepare($strSQL);
							$query->bindParam(':BULLETIN_BOARD_NO', $item['BULLETIN_BOARD_NO']);

							$query->execute();
							if($query->rowCount() > 0) {
								$strSqlLog = str_replace(
									':BULLETIN_BOARD_NO', $item['BULLETIN_BOARD_NO'], $strSQL
								);
								fncWriteLog(
									LogLevel['Info'],
									LogPattern['Sql'],
									SCREEN_NAME . ' '.$strSqlLog
								);
							} else {
								$GLOBALS['g_dbaccess']->funcRollback();
								echo "$('.error').html('".$arrTextTranslate['PUBLIC_MSG_007']."');";
								fncWriteLog(
									LogLevel['Error'],
									LogPattern['Error'],
									SCREEN_NAME . ' ' . $arrTextTranslate['PUBLIC_MSG_007']
								);
								exit();
							}

						}
						//no error occurred, commit
						$GLOBALS['g_dbaccess']->funcCommit();

					}catch(Exception $e) {
						$GLOBALS['g_dbaccess']->funcRollback();
						//error occurred, write log
						fncWriteLog(
							LogLevel['Error'],
							LogPattern['Error'],
							SCREEN_NAME . ' ' . $arrTextTranslate['PUBLIC_MSG_007']
						);
						//rollback

						//2020/4/14 T.Masude execコマンドでファイルの削除を行う

						//delete csv file
						//if(
						//	is_file(
						//		SHARE_FOLDER.'/'.ORGANIZE_BULLETIN_BOARD_FOLDER
						//		. '/' .$dtmDatetime. '/' . $filename
						//	)
						//) {
						//	unlink(
						//		SHARE_FOLDER.'/'.ORGANIZE_BULLETIN_BOARD_FOLDER
						//		. '/' .$dtmDatetime. '/' . $filename
						//	);
						//}
						//delete folder
						//if(
						//	is_dir(SHARE_FOLDER.'/'.ORGANIZE_BULLETIN_BOARD_FOLDER . '/' .$dtmDatetime)
						//) {
						//	rmdir(SHARE_FOLDER.'/'.ORGANIZE_BULLETIN_BOARD_FOLDER . '/' .$dtmDatetime);
						//}

						$strPath = str_replace("/", "\\", SHARE_FOLDER_ORGANIZE .'/'.ORGANIZE_BULLETIN_BOARD_FOLDER . '/' .$dtmDatetime);

						exec("rd /s /q " . $strPath);

						//2020/4/14 T.Masude
						header('Content-Type: text/html; charset=utf-8');
						header("Content-Transfer-Encoding: utf-8");
						echo "$('.error').html('".$arrTextTranslate['PUBLIC_MSG_007']."');";
						exit();
					}
					exit();
				} else {
					//failed to save csv, show error

					echo "$('.error').html('".$arrTextTranslate['PUBLIC_MSG_007']."');";
					fncWriteLog(
						LogLevel['Error'],
						LogPattern['Error'],
						SCREEN_NAME . ' ' . $arrTextTranslate['PUBLIC_MSG_007']
					);
					exit();
				}
				exit();
			}

		}
	}

	if($_SESSION['BULLETIN_BOARD_MNG_ERROR_MSG']!='') {
	//there is error, show
?>
<script>
	$(function() {
		$('.error').html('<?php echo $_SESSION['BULLETIN_BOARD_MNG_ERROR_MSG'] ?>');
	});
</script>
<?php
		unset($_SESSION['BULLETIN_BOARD_MNG_ERROR_MSG']);



	}

	if(isset($_POST['loadList'])) {
		if(isset($_POST['originalSearch']) && $_POST['originalSearch'] == 1) {
			//log button search
			$strSearchQuery = '「インシデント件名 = '.$_SESSION['txtIncident']
			.',期間（開始）= '.$_SESSION['txtDateStart']
			.',期間（終了） = '.$_SESSION['txtDateEnd']
			.',完了を表示 = '.iff($_SESSION['chkDone']!='',1,0).'」';
			$strViewLog = SCREEN_NAME . '　検索を実施 '
			.$strSearchQuery
			.' (ユーザID = '.$objUserInfo->strUserID.')';
			fncWriteLog(LogLevel['Info'] , LogPattern['Button'], $strViewLog);

			$GLOBALS['currentPage'] = 1;
			$_SESSION['BULLETIN_BOARD_MNG_PAGE'] = 1;
		}
	}

	//get bulletin board list
	$arrBulletinBoardList = fncSelectData(
		$strSQL, $arrCondition, $GLOBALS['currentPage'], true, SCREEN_NAME
	);

	if(is_array($arrBulletinBoardList)) {
		//get data successfully

	} else {
		//failed to get data
		echo "<script>";
		echo "$('.error').html('".$arrTextTranslate['PUBLIC_MSG_001']."');";
		echo "</script>";
	}

?>
<table class="blueTable">
    <thead>
    <tr>
        <th class="text-th">
			<?php echo $arrTextTranslate['BULLETIN_BOARD_MNG_TEXT_005']; ?>
		</th>
        <th class="text-th">
			<?php echo $arrTextTranslate['BULLETIN_BOARD_MNG_TEXT_006']; ?></th>
		<th class="text-th">
			<?php echo $arrTextTranslate['BULLETIN_BOARD_MNG_TEXT_007']; ?>
		</th>
        <th class="text-th">
			<?php echo $arrTextTranslate['BULLETIN_BOARD_MNG_TEXT_008']; ?>
		</th>
        <th class="text-th">
			<?php echo $arrTextTranslate['BULLETIN_BOARD_MNG_TEXT_020']; ?>
		</th>
        <th class="text-th">
			<?php echo $arrTextTranslate['BULLETIN_BOARD_MNG_TEXT_009']; ?>
		</th>
        <th class="text-th">
			<?php echo $arrTextTranslate['BULLETIN_BOARD_MNG_TEXT_010']; ?>
		</th>
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
							<?php fncViewPagination('bulletin_board_mng_proc.php'); ?>
						</div>
					</td>
					<td style="width:30%;">
						<div class="in-line" style="float: right">
							<form action="bulletin_board_mng_proc.php" method="post" id="formCSV">
							<input type="hidden" name="X-CSRF-TOKEN"
							value="<?php echo $_POST['X-CSRF-TOKEN']; ?>">
							<input type="hidden" name="mode" value="0">
							<button type="submit" class="tbtn tbtn-defaut tbl-btn-csv"
							data-dismiss="modal">
								<?php echo $arrTextTranslate['PUBLIC_BUTTON_011']; ?>
							</button>
							<button type="submit" class="tbtn tbtn-defaut tbl-btn-csv-del"
							id="close" disabled>
								<?php echo $arrTextTranslate['PUBLIC_BUTTON_012']; ?>
							</button>
							<button type="submit" class="tbtn tbtn-defaut tbn-btn-return">
								<?php echo $arrTextTranslate['PUBLIC_BUTTON_015']; ?>
							</button>
							</form>
			            </div>
					</td>
				</tr>
			</table>
		</td>
    </tr>
    </tfoot>
    <tbody class="tbody-data">
    <?php if(
		isset($arrBulletinBoardList) && is_array($arrBulletinBoardList)
	) foreach($arrBulletinBoardList as $item) { ?>
    <tr>
        <td class="text-center">
			<?php echo fncHtmlSpecialChars($item['BUSINESS_NAME'] == ''
			? $item['BUSINESS_NAME_ORG'] : $item['BUSINESS_NAME']); ?>
    	</td>
        <td class="text-center">
		<a href="bulletin_board_view.php" data-id="<?php
			echo $item['BULLETIN_BOARD_NO'];
			?>" class="load-modal" data-screen="bulletin_mng">
			<?php echo fncHtmlSpecialChars(
				$objUserInfo->intLanguageType == 0 ?
				(
					is_null($item['INCIDENT_NAME_JPN']) || $item['INCIDENT_NAME_JPN']==''
					? $item['INCIDENT_NAME_ENG']
					: $item['INCIDENT_NAME_JPN']
				)
				:
				(
					is_null($item['INCIDENT_NAME_ENG']) || $item['INCIDENT_NAME_ENG']==''
					? $item['INCIDENT_NAME_JPN']
					: $item['INCIDENT_NAME_ENG']
				)
			); ?>
		</a>
		</td>
        <td class="text-center">
			<?php echo (
				!is_null($item['COMP_DATE'])
				? date_format(date_create($item['OCCURRENCE_DATE']), 'Y/n/j H:i').' ～ '
				.date_format(date_create($item['COMP_DATE']), 'Y/n/j H:i')
				: date_format(date_create($item['OCCURRENCE_DATE']), 'Y/n/j H:i').' ～ '); ?>
		</td>
		<td class="text-center">
			<?php
				echo (
					!is_null($item['UP_DATE'])
					? date_format(date_create($item['UP_DATE']), 'Y/n/j H:i')
					: '');
			?>
		</td>

        <td class="text-center">
			<?php
				echo($item['CORRECTION_FLAG'] == 0 ?
				$arrTextTranslate['BULLETIN_BOARD_MNG_TEXT_021'] : '');
			?>
		</td>
		<td class="text-center">
			<?php
				echo (
					!is_null($item['COMP_DATE'])
					? $arrTextTranslate['BULLETIN_BOARD_MNG_TEXT_011']
					: '');
			?>
		</td>
        <td class="text-center">
            <div></div>
			<button class="tbl-btn tbtn-red btn-del" data-id="<?php
				echo $item['BULLETIN_BOARD_NO']; ?>">
				<?php echo fncHtmlSpecialChars($arrTextTranslate['PUBLIC_BUTTON_010']); ?>
			</button>
        </td>
    </tr>
	<?php } ?>

    </tbody>
    </tr>
</table>
<script>
	var numRecord = <?php echo $GLOBALS['totalRecord']; ?>;
	var confirmMsg = '<?php echo str_replace(
		'%0%', $GLOBALS['totalRecord'], $arrTextTranslate['PUBLIC_MSG_006']
	) ; ?>';
</script>

<script>
	var ajaxUrl = 'bulletin_board_mng_proc.php';
	$(function() {

		if($('[name="chkDone"]').prop('checked') == true) {
			$('.tbl-btn-csv-del').removeAttr('disabled');

		} else {
			$('.tbl-btn-csv-del').attr("disabled", true);

		}
	});

</script>
