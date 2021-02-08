
<?php
// -------------------------------------------------------------------------
//	function	: ポータル画面 (仮)ページ
//	create		: 2020/01/17 KBS S.Tasaki
//	update		:
// -------------------------------------------------------------------------
require_once('common/validate_user.php');

?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>DailyBoard</title>
    <link rel="stylesheet" type="text/css" href="css/style.css">
    <link rel="stylesheet" type="text/css" href="css/tabs.css">
    <link rel="stylesheet" type="text/css" href="css/table-info.css">
    <link rel="stylesheet" type="text/css" href="css/menu.css">
    <link rel="stylesheet" href="css/bootstrap.min.css">
    <script type="text/javascript" src="js/jquery.min.js"></script>
    <script type="text/javascript" src="js/menu.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="js/nextpage.js"></script>
</head>
<body>
<div class="main-content">
    <div class="header-boad">
        <div class="in-line right-100"><img src="img/logo.jpg" alt=""></div>
        <div class="in-line">10/23 10:00 JST　　10/23 0:00UTC </div>
        <div class="in-line">
            <button class="tbtn tbtn-defaut mark-btn" onclick="loadModal('incident_case_edit.html')">インシデント事案登録</button>
        </div>
        <div class="dropdown">
            <button onclick="myFunction()" class="dropbtn">各種メニュー　▼</button>
            <div id="myDropdown" class="dropdown-content select-gray">
                <div class="t-outgroup">デイリーボード</div>
                <a href="announce_mng.html">お知らせ管理</a>
                <a href="bulletin_board_mng.html">掲示板管理</a>
                <a href="link_mng.html">リンク情報管理</a>
                <div class="t-outgroup">インシデントボード</div>
                <a href="incident_case_mng.html">インシデント事案管理</a>
                <div class="t-outgroup">会社情報</div>
                <a href="company_mng.html">会社情報管理</a>
                <div class="t-outgroup">ユーザ設定</div>
                <a href="#" onclick="loadModal('user_setting.html')" >ユーザ設定</a>
                <a href="user_mng.html">ユーザ管理</a>
            </div>
        </div>

        <div class="in-line col-right right-100">
            <div class="in-line">
                <div class="right-20">ユーザID:</div>
                <div class="right-20">会社名:</div>
                <div class="right-20">名前:</div>
            </div>
        </div>
    </div>


    <div class = "tabinator">
        <input type = "radio" id = "tab1" class="input-tab" name = "tabs" checked>
        <label for = "tab1" class="title">DailyBoard</label>
        <input type = "radio" id = "tab2" class="input-tab" name = "tabs">
        <label for = "tab2" class="title">IncidentBoard</label>
        <div id = "content1">
            <?php require_once('daily_boad.php'); ?>
        </div>
        <div id = "content2">
            <div class="incident_boad"></div>
            <?php require_once('incident_boad.php'); ?>
        </div>
    </div>
</div>

<!-- Modal HTML -->
<div id="myModal" class="modal fade">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div id="modal-body">

            </div>
        </div>
    </div>
</div>
<style>
    body{
        overflow: hidden;
    }
</style>


</body>
</html>