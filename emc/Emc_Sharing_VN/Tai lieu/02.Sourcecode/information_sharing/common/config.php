<?php
// -------------------------------------------------------------------------
//	function	: 定数の定義
//	create		: 2020/01/17 KBS S.Tasaki
//	update		:
// -------------------------------------------------------------------------

//ページタイトル
define('HEADER_PAGE_TITLE', '情報共有システム');

//DB接続情報
define('DB_HOSTNAME', 'localhost\sqlexpress');
define('DB_DBNAME', 'info_db');
define('DB_USERNAME', 'info_user');
define('DB_PASSWORD', 'info_pass');

//SMTPサーバ
define('MAIL_SMTP_HOST', '');
define('MAIL_SMTP_PORT', '');
define('MAIL_SMTP_USER', '');
define('MAIL_SMTP_PASS', '');

//ログ出力
define('LOG_FOLDER', 'C:\inetpub\wwwroot\information_sharing\log');
define('LOG_LOGIN_FLAG', 1);
define('LOG_DISPLAY_VIEW_FLAG', 1);
define('LOG_BUTTON_FLAG', 1);
define('LOG_SQL_FLAG', 1);

//管理画面1ページの表示件数
define('LIST_VIEW_NUM', 10);

//問い合わせ表示期間(日)
define('QUERY_VIEW_DAY', 30);

//問い合わせ更新周期(問い合わせ画面)(秒単位)
define('QUERY_RELOAD_TIME', 3);

//問い合わせ更新周期(ポータル画面)(秒単位)
define('QUERY_RELOAD_TIME_PORTAL', 10);

//ポータル更新周期(秒単位)
define('PORTAL_RELOAD_TIME', 60);

//パスワード有効期限(日)
define('PASSWORD_PERIOD_DAY', 90);

//共有フォルダ
define('SHARE_FOLDER', '');
define('SHARE_ID', '');
define('SHARE_PASSWORD', '');

//お知らせデータ情報添付ファイル保存フォルダ
define('ANNOUNCE_ATTACHMENT_FOLDER', '');

//関係機関への依頼事項添付ファイル保存フォルダ
define('REQUEST_ATTACHMENT_FOLDER', '');

//関係機関からの情報添付ファイル保存フォルダ
define('INFORMATION_ATTACHMENT_FOLDER', '');

//データ整理時お知らせデータ保存フォルダ
define('ORGANIZE_ANNOUNCE_FOLDER', '');

//データ整理時掲示板データ保存フォルダ
define('ORGANIZE_BULLETIN_BOARD_FOLDER', '');

//データ整理時インシデント事案データ保存フォルダ
define('ORGANIZE_INCIDENT_FOLDER', '');

//リンク情報新規登録編集 バナー画像保存フォルダ
define('LINK_EDIT_FOLDER', '');

//AWS
define('AWS_ACCESS_KEY', '');
define('AWS_SECRET_KEY', '');
?>