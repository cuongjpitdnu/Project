<?php
	/*
	* @bulletin_board_mng.php
	*
	* @create 2020/03/13 AKB Thang
	*/

	require_once('common/common.php');
	require_once('common/session_check.php');

	header('Content-type: text/html; charset=utf-8');
	header('X-FRAME-OPTIONS: DENY');

	// if failed to connect DB, show alert then rediret to login page
	if(fncConnectDB() == false) {
		$_SESSION['LOGIN_ERROR'] = 'DB接続に失敗しました。';
		header('Location: login.php');
		exit;
	}

	// if login info does not existed, rediect to login page
	if(!isset($_SESSION['LOGINUSER_INFO'])) {
		echo "
		<script>
			alert('".PUBLIC_MSG_008_JPN . "/" . PUBLIC_MSG_008_ENG."');
			window.location.href = 'login.php';
		</script>
		";
		exit();
	}
	//get logged in user object
	$objUserInfo = unserialize($_SESSION['LOGINUSER_INFO']);
	//define constant
	define('SCREEN_NAME', '掲示板管理画面');

	// check page first time or refresh
	if (!isset($_SESSION["VISITS"])) {
	    $_SESSION["VISITS"] = 0;
	}
	$_SESSION["VISITS"] = $_SESSION["VISITS"] + 1;

	if ($_SESSION["VISITS"] > 1) {
	    //you refreshed the page!
	} else {
	    // init session of screen
	    $_SESSION['txtIncident'] = '';
	    $_SESSION['txtDateStart'] = '';
	    $_SESSION['txtDateEnd'] = '';
	    $_SESSION['chkDone'] = '';
	    $_SESSION['BULLETIN_BOARD_MNG_PAGE'] = 1;
	    //View log
	    $strViewLog = SCREEN_NAME . '　画面表示 (ユーザID = '
        .$objUserInfo->strUserID.') '
        .(isset($_SERVER['HTTP_REFERER']) ? $_SERVER['HTTP_REFERER'] : null);
        fncWriteLog(LogLevel['Info'] , LogPattern['View'], $strViewLog);
	}

	/* //not redirect after exporting csv, reset search
	if(!isset($_SESSION['BULLETIN_BOARD_MNG_REDIRECT'])) {
		$_SESSION['txtIncident'] = '';
		$_SESSION['txtDateStart'] = '';
		$_SESSION['txtDateEnd'] = '';
		$_SESSION['chkDone'] = '';
	} */

	$arrTitle =  array(
		'BULLETIN_BOARD_MNG_TEXT_001',
		'BULLETIN_BOARD_MNG_TEXT_002',
		'BULLETIN_BOARD_MNG_TEXT_003',
		'BULLETIN_BOARD_MNG_TEXT_004',
		'PUBLIC_BUTTON_006',
		'BULLETIN_BOARD_MNG_MSG_001',
		'PUBLIC_MSG_009',
		'PUBLIC_MSG_049',
		'PUBLIC_MSG_005',
		'PUBLIC_MSG_007',

	);

	// get list text(header, title, msg) with languague_type of user logged
	$arrTextTranslate = getListTextTranslate(
		$arrTitle,
		$objUserInfo->intLanguageType
	);

	//sesstion check
	fncSessionTimeOutCheck();

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

	//エラーメッセージ確認
	$strLoginError = '';

	// if have permission to access this screen
	if($objUserInfo->intMenuPerm != 1) {
		$alertMsg = $arrTextTranslate['PUBLIC_MSG_009'];
		echo "
		<script>
			alert('".$arrTextTranslate['PUBLIC_MSG_009']."');
			window.location.href = 'login.php';
		</script>
		";
	}


	/* if(!isset($_SESSION['BULLETIN_BOARD_MNG_REDIRECT'])) {
	    $_SESSION['BULLETIN_BOARD_MNG_PAGE'] = 1;
		//View log
		$strViewLog = SCREEN_NAME . '　画面表示 (ユーザID = '
		.$objUserInfo->strUserID.') '
		.(isset($_SERVER['HTTP_REFERER']) ? $_SERVER['HTTP_REFERER'] : null);
		fncWriteLog(LogLevel['Info'] , LogPattern['View'], $strViewLog);
	} */

?>
<!DOCTYPE html>
<html>
<head>
	<meta charset="UTF-8">
	<meta name="csrf-token" content="<?php echo $strCsrf; ?>">
	<title><?php echo $arrTextTranslate['BULLETIN_BOARD_MNG_TEXT_001']; ?></title>
	<link rel="stylesheet" type="text/css" href="css/style.css">
	<link rel="stylesheet" type="text/css" href="css/table.css">
	<link type="text/css"
	href="css/smoothness/jquery-ui-1.10.4.custom.css"
	rel="stylesheet" />
	<script type="text/javascript" src="js/jquery.min.js"></script>
	<script type="text/javascript" src="js/jquery-ui-1.10.4.custom.js"></script>
	<script type="text/javascript" src="js/jquery.ui.datepicker-ja.js"></script>

	<link rel="stylesheet" href="css/bootstrap.min.css">
	<script src="js/bootstrap.min.js"></script>
	<script src="js/common.js"></script>
</head>
<body>
<?php
	if($strLoginError != '') {
?>
<font color="red"><?php echo $strLoginError;?></font>
<?php
	}
?>
	<div class="main-content">
		<div class="main-form">
			<div class="form-title">
				<?php echo $arrTextTranslate['BULLETIN_BOARD_MNG_TEXT_001']; ?>
			</div>
			<?php if(isset($_SESSION['BULLETIN_BOARD_MNG_ERROR'])) {
				echo '<font color="red" class="error">'
				.$_SESSION['BULLETIN_BOARD_MNG_ERROR'].'</font>';
				unset($_SESSION['BULLETIN_BOARD_MNG_ERROR']);
			}else{
				echo '<font color="red" class="error"></font>';
			}
			?>
			<div class="form-body">
				<div class="cont-title">
					<form class="search-form" name="searchForm" id="searchForm" autocomplete="off">
						<input type="hidden" name="loadList" value="1">
						<div class=" in-line">
							<div class="label-short">
								<?php echo $arrTextTranslate['BULLETIN_BOARD_MNG_TEXT_002']; ?>
							</div>
							<input type="text" class="t-input t-input-125" name="txtIncident" value="<?php
								echo isset($_SESSION['txtIncident']) && $_SESSION['txtIncident']!=''
								? $_SESSION['txtIncident']
								: '';?>">
						</div>
						<div class="in-line">
							<div class="label-80" style="display: inline-block;">
								<?php echo $arrTextTranslate['BULLETIN_BOARD_MNG_TEXT_003']; ?>
							</div>
							<input class="t-input" id="date_start" name="txtDateStart" type="text"
							value="<?php echo (isset($_SESSION['txtDateStart'])
									&& $_SESSION['txtDateStart']!='' ?  $_SESSION['txtDateStart'] : '');
								?>"
							> ~
							<input class="t-input" id="date_end" name="txtDateEnd" type="text" value="<?php
									echo isset($_SESSION['txtDateEnd'])
									&& $_SESSION['txtDateEnd']!='' ? $_SESSION['txtDateEnd'] : '';
								?>"
							>
						</div>

						<div class="in-line">
							<label class="t-container">
								<?php echo $arrTextTranslate['BULLETIN_BOARD_MNG_TEXT_004']; ?>
								<input type="checkbox" name="chkDone"
									<?php
										echo iff(isset($_SESSION['chkDone'])
										&& $_SESSION['chkDone']!='', 'checked', '');
									?>
								>
								<span class="checkmark"></span>
							</label>
						</div>
						<div class="in-line col-right">
							<input type="button" class="tbtn tbtn-defaut btn-search" value="<?php
								echo $arrTextTranslate['PUBLIC_BUTTON_006']; ?>"
							>
						</div>
					</form>
				</div>
				<div class="ajax-content">

				</div>
			</div>
		</div>
	</div>
</body>
<!-- Modal HTML -->
<div id="myModal" class="modal fade">
	<div class="modal-dialog modal-lg">
		<div class="modal-content">
			<div id="modal-body">

			</div>
		</div>
	</div>
</div>


<script>
	var confirmDelMsg = '<?php
	echo $arrTextTranslate['BULLETIN_BOARD_MNG_MSG_001']; ?>';
	var csrf = '<?php echo $strCsrf; ?>';
	var ajaxUrl = 'bulletin_board_mng_proc.php';
	var csvOutputFailed = '<?php echo $arrTextTranslate['PUBLIC_MSG_005']; ?>';
	var batchDeleteFailed = '<?php echo $arrTextTranslate['PUBLIC_MSG_007']; ?>';
	$(function() {
		$('#date_start').datepicker({
			changeMonth: true,
			changeYear: true,
			showButtonPanel: true,
			dateFormat: 'yy/mm/dd'
		});

		$('#date_end').datepicker({
			changeMonth: true,
			changeYear: true,
			showButtonPanel: true,
			dateFormat: 'yy/mm/dd'
		});


		function loadPageNew() {
			$.ajax({
				url: ajaxUrl,
				type: 'post',
				data: [{name: 'X-CSRF-TOKEN', value: csrf}, {name: 'newLoad', value: '1'}],
				success: function(result) {
					$(".ajax-content").html(result);
				}
			});
		}

		function loadPage() {
			$('.error').html('');
			var html = $('.ajax-content').html();
			var arr = $('.search-form').serializeArray();
			arr.push({name: 'originalSearch', value: 0});

			$.ajax({
				url: ajaxUrl,
				type: 'post',
				data: arr,
				success: function(result){
					$(".ajax-content").html(result);
				}
			});
		}
<?php
if($_SESSION['VISITS'] > 1){
?>
		loadPage();
<?php
}else{
?>
		loadPageNew();
<?php
}
?>


		$('[name="chkDone"]').click
		$(document).on('click', '[name="chkDone"]', function(e) {
			checkDone();
		});
		function checkDone() {
			$('.error').html('');
			var html = $('.ajax-content').html();
			var arr = $('.search-form').serializeArray();
			arr.push({name: 'originalSearch', value: 1});

			$.ajax({
				url: ajaxUrl,
				type: 'post',
				data: arr,
				success: function(result){
					$(".ajax-content").html(result);

					if($('.error').html() != ''){
						$(".ajax-content").html(html);
						if($('[name="chkDone"]').prop('checked') == true) {
							$('[name="chkDone"]').prop('checked', false);
						}else{
							$('[name="chkDone"]').prop('checked', true);
						}
					}
					if($('[name="chkDone"]').prop('checked') == true) {
						$('.tbl-btn-csv-del').removeAttr('disabled');

					} else {
						$('.tbl-btn-csv-del').attr("disabled", true);

					}
				}
			});
		}
	});

</script>
</html>