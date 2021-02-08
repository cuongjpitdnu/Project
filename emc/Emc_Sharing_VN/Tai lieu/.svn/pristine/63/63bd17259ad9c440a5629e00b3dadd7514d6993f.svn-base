<html>
<head>
    <meta charset="UTF-8">
    <link rel="stylesheet" type="text/css" href="css/style.css">
    <script type="text/javascript" src="js/query_chat.js"></script>
    <script type="text/javascript" src="js/aws-sdk.min.js"></script>
    <style>
        .mesgs {
            border: 1px solid;
        }
    </style>
</head>
<body>
<div class="main-form">
    <div class="form-title">問い合わせ</div>
    <div class="form-body">
        <div class="mesgs" id="mesgs">
            <?php require_once('msg_history.php'); ?>
        </div>
        <div class=" top-20">内容/Contents (<span class="txt-red">原文/Original</span>)</div>
        <textarea name="txt-source" id="txt-source" class="form-control" rows="2" cols="50"></textarea>
        <div class="in-line tlabel"><input type="checkbox" name="manually" value="manual" id="ckeck-manunal" >翻訳を手入力する</div>

        <div class="in-line col-right">
            <div class="select-container">
                <select name="sle-tran" id="sle-tran">
                    <option value="je">日(原文)⇒英(翻訳)</option>
                    <option value="ej">Eng(Original)⇒Jpn(Translation)</option>
                </select>
            </div>
            <button type="button" class="tbtn tbtn-defaut pad-left-20" id="btn-tran">翻訳</button>
        </div>
        <div class="mar-tl-10">内容 (<span class="txt-red">翻訳</span>)</div>
        <textarea name="txt-target" id="txt-target"  class="form-control top-20" rows="2" cols="50" disabled></textarea>

        <div class="form-footer top-20">
            <div class="in-line">
                <button type="submit" class="tbtn tbtn-defaut " onclick="postQuery()">投稿</button>
            </div>
            <div class="in-line text-right" style="float: right">
                <button type="submit" class="tbtn-cancel tbtn-defaut " id="close" data-dismiss="modal">閉じる</button>
            </div>
        </div>
    </div>
</div>
</body>

</html>