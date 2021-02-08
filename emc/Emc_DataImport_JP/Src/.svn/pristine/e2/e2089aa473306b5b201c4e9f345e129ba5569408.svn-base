<?php
// -------------------------------------------------------------------------
//	function	: 定数の定義
//	create		: 2020/01/17 KBS S.Tasaki
//	update		:
// -------------------------------------------------------------------------

//ページタイトル
define('HEADER_PAGE_TITLE', '情報共有システム');

//接続先設定 0:マスタホスト、1:スレーブホスト
define('SERVER_SELECT', 0);

if(SERVER_SELECT == 0){
	//DB接続情報
	define('DB_HOSTNAME', '192.168.1.1\SQLEXPRESS');
	define('DB_DBNAME', '');
	define('DB_USERNAME', '');
	define('DB_PASSWORD', '');
	
	//共有フォルダ
	define('SHARE_FOLDER', '\\\\192.168.1.11\\Share');
	define('SHARE_FOLDER_ORGANIZE', '\\\\192.168.1.12\\Share');
	define('SHARE_FOLDER_LOG', '\\\\192.168.1.13\\Share');
	
}else if(SERVER_SELECT == 1){
	//DB接続情報
	define('DB_HOSTNAME', '192.168.1.2\SQLEXPRESS');
	define('DB_DBNAME', '');
	define('DB_USERNAME', '');
	define('DB_PASSWORD', '');
	
	//共有フォルダ
	define('SHARE_FOLDER', '\\\\192.168.1.21\\Share');
	define('SHARE_FOLDER_ORGANIZE', '\\\\192.168.1.22\\Share');
	define('SHARE_FOLDER_LOG', '\\\\192.168.1.23\\Share');
}else{
	//DB接続情報
	define('DB_HOSTNAME', '192.168.1.1\SQLEXPRESS');
	define('DB_DBNAME', '');
	define('DB_USERNAME', '');
	define('DB_PASSWORD', '');
	
	//共有フォルダ
	define('SHARE_FOLDER', '\\\\192.168.1.11\\Share');
	define('SHARE_FOLDER_ORGANIZE', '\\\\192.168.1.12\\Share');
	define('SHARE_FOLDER_LOG', '\\\\192.168.1.13\\Share');
}

//SMTPサーバ
define('MAIL_SMTP_HOST', '');
define('MAIL_SMTP_PORT', '');
define('MAIL_SMTP_USER', '');
define('MAIL_SMTP_PASS', '');
define('MAIL_FROM_ADDRESS', '');
define('MAIL_DESTINATION_ADMIN_ADDRESS', '');
define('MAIL_SUBMIT_NUMBER', 200);
define('MAIL_SUBMIT_TITLE_JPN', '【業務連絡】KIX情報共有システム');
define('MAIL_SUBMIT_TITLE_ENG', '【Announcement】KIX Information Sharing System');

//ログ出力
define('LOG_LOGIN_FLAG', 1);
define('LOG_DISPLAY_VIEW_FLAG', 1);
define('LOG_BUTTON_FLAG', 1);
define('LOG_SQL_FLAG', 0);

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
define('BANNER_VERTICAL_SIZE', '150');
define('BANNER_WIDTH_SIZE', '200');

//AWS
define('AWS_ACCESS_KEY', '');
define('AWS_SECRET_KEY', '');

//セッションタイムアウト時間
define('SESSION_TIMEOUT_TIME', 600);

//らく認
define('RAKUNIN_APP_ID', '');
define('RAKUNIN_APP_SECRET', '');
define('OTP_ERROR_MAILADDRESS', '');

//OTP有効期限(秒単位)
define('OTP_TIME_OUT', '60');

//当日分ログ出力先パス
define('TODAY_LOG_FOLDER', 'LOG_TODAY');

//ユーザID 最大サイズ
define('USER_ID_MAX', '20');

//FHD以上
//情報一覧、機関カテゴリ表示タイトル文字数
define('FHD_TITLE_INST_JPN', '9');
define('FHD_TITLE_INST_ENG', '14');
//情報一覧、機関カテゴリ表示会社略称名文字数
define('FHD_COMPANY_INST_JPN', '13');
define('FHD_COMPANY_INST_ENG', '18');
//情報一覧、情報カテゴリ表示タイトル文字数
define('FHD_TITLE_INFO_JPN', '6');
define('FHD_TITLE_INFO_ENG', '9');
//情報一覧、情報カテゴリ表示会社略称名文字数
define('FHD_COMPANY_INFO_JPN', '6');
define('FHD_COMPANY_INFO_ENG', '9');

//FHD以下
//情報一覧、機関カテゴリ表示タイトル文字数
define('TITLE_INST_JPN', '6');
define('TITLE_INST_ENG', '10');
//情報一覧、機関カテゴリ表示会社略称名文字数
define('COMPANY_INST_JPN', '10');
define('COMPANY_INST_ENG', '14');
//情報一覧、情報カテゴリ表示タイトル文字数
define('TITLE_INFO_JPN', '4');
define('TITLE_INFO_ENG', '6');
//情報一覧、情報カテゴリ表示会社略称名文字数
define('COMPANY_INFO_JPN', '4');
define('COMPANY_INFO_ENG', '9');

?>
