
<?php
// -------------------------------------------------------------------------
//	function	: メッセージの定義
//	create		: 2020/02/06 KBS S.Tasaki
//	update		:
// -------------------------------------------------------------------------

//ログイン画面
define('LOGIN_MSG_001_JPN', 'UserIDを入力して下さい。');
define('LOGIN_MSG_001_ENG', 'Please enter your UserID.');
define('LOGIN_MSG_002_JPN', 'Passwordを入力して下さい。');
define('LOGIN_MSG_002_ENG', 'Please enter your Password.');
define('LOGIN_MSG_003_JPN', 'ユーザID, パスワードを確認してください。');
define('LOGIN_MSG_003_ENG', 'Check the user ID and password.');
define('LOGIN_MSG_004_JPN', '管理者に確認してください。');
define('LOGIN_MSG_004_ENG', 'Check with your administrator.');
define('LOGIN_MSG_005_JPN', '有効期限内ではないのでログインできません。');
define('LOGIN_MSG_005_ENG', 'You cannot log in because it is not within the expiration date.');
define('LOGIN_MSG_006_JPN', 'Password for Rakuninを入力して下さい。');
define('LOGIN_MSG_006_ENG', 'Enter Password for Rakunin.');
define('LOGIN_MSG_007_JPN', 'らく認に対象のユーザが登録されていません。');
define('LOGIN_MSG_007_ENG', 'The target user has not been registered.');
define('LOGIN_MSG_008_JPN', 'ユーザIDはロック中のため送信に失敗しました。');
define('LOGIN_MSG_008_ENG', 'Transmission failed because the user ID is locked');
define('LOGIN_MSG_009_JPN', 'ユーザIDに携帯電話番号が紐づけされていないため送信に失敗しました。');
define('LOGIN_MSG_009_ENG', 'Transmission failed because the mobile phone number is not linked to the user ID.');
define('LOGIN_MSG_010_JPN', 'ユーザIDにアプリケーションが紐づけされていないため送信に失敗しました。');
define('LOGIN_MSG_010_ENG', 'Transmission failed because the application is not linked to the user ID.');
define('LOGIN_MSG_011_JPN', 'ユーザIDにアプリケーション認証方式の種類「SMS認証」が設定されていないため送信に失敗しました。');
define('LOGIN_MSG_011_ENG', 'The transmission failed because the type of application authentication method "SMS authentication" is not set for the user ID.');
define('LOGIN_MSG_012_JPN', 'OTP認証パスワードの送信に失敗しました。');
define('LOGIN_MSG_012_ENG', 'Failed to send OTP authentication password.');
define('LOGIN_MSG_013_JPN', 'Password for Rakuninの値とOTP認証が一致しませんでした。');
define('LOGIN_MSG_013_ENG', 'Password for Rakunin value and OTP authentication did not match.');
define('LOGIN_MSG_014_JPN', 'システム管理者へ連絡してください。');
define('LOGIN_MSG_014_ENG', 'Contact your system administrator.');
define('LOGIN_MSG_015_JPN', '時間内にOTPの入力が成功しませんでした。');
define('LOGIN_MSG_015_ENG', 'OTP input was not successful in time.');
define('LOGIN_MSG_016_JPN', 'SMS認証コードの送信に失敗しました。時間をおいてから、もう一度お試しください。');
define('LOGIN_MSG_016_ENG', 'Failed to send SMS verification code. Please wait and try again.');


//ポータル画面（JCMGボード）
define('PORTAL_INCIDENT_MSG_001_JPN', 'JCMGボードが完了になりました。');
define('PORTAL_INCIDENT_MSG_001_ENG', 'The JCMG board is complete.');
define('PORTAL_INCIDENT_MSG_002_JPN', 'ピン止めに失敗しました。');
define('PORTAL_INCIDENT_MSG_002_ENG', 'Pinning failed.');
define('PORTAL_INCIDENT_MSG_003_JPN', 'ピン止めを外せませんでした。');
define('PORTAL_INCIDENT_MSG_003_ENG', 'The pinning could not be removed.');
define('PORTAL_INCIDENT_MSG_004_JPN', 'JCMGボードが公開されました。');
define('PORTAL_INCIDENT_MSG_004_ENG', 'The JCMG board has been released.');



//お知らせ表示画面
define('ANNOUNCE_VIEW_MSG_001_JPN', 'ファイルが見つかりませんでした。');
define('ANNOUNCE_VIEW_MSG_001_ENG', 'File not found.');
define('ANNOUNCE_VIEW_MSG_002_JPN', '既に削除されたお知らせ情報です。');
define('ANNOUNCE_VIEW_MSG_002_ENG', 'Announce information that has already been deleted.');



//お知らせ登録編集画面
define('ANNOUNCE_EDIT_MSG_001_JPN', '既に削除されたお知らせ情報です。');
define('ANNOUNCE_EDIT_MSG_001_ENG', 'Announce information that has already been deleted.');
define('ANNOUNCE_EDIT_MSG_002_JPN', 'ファイルが見つかりませんでした。');
define('ANNOUNCE_EDIT_MSG_002_ENG', 'File not found.');
define('ANNOUNCE_EDIT_MSG_003_JPN', 'お知らせ情報を投稿します。よろしいですか？');
define('ANNOUNCE_EDIT_MSG_003_ENG', 'Post announcement information. Is it OK?');
define('ANNOUNCE_EDIT_MSG_004_JPN', 'タイトル（原文）を入力して下さい。');
define('ANNOUNCE_EDIT_MSG_004_ENG', 'Please enter the title (original text).');
define('ANNOUNCE_EDIT_MSG_005_JPN', 'タイトル（原文）の文字数が20を超えています。');
define('ANNOUNCE_EDIT_MSG_005_ENG', 'Title (original text) exceeds 20 characters.');
define('ANNOUNCE_EDIT_MSG_006_JPN', 'タイトル（原文）の文字数が100を超えています。');
define('ANNOUNCE_EDIT_MSG_006_ENG', 'Title (original text) exceeds 100 characters.');
define('ANNOUNCE_EDIT_MSG_007_JPN', 'タイトル（翻訳）を入力して下さい。');
define('ANNOUNCE_EDIT_MSG_007_ENG', 'Please enter a title (translation).');
define('ANNOUNCE_EDIT_MSG_008_JPN', 'タイトル（翻訳）の文字数が20を超えています。');
define('ANNOUNCE_EDIT_MSG_008_ENG', 'The title (translation) contains more than 20 characters.');
define('ANNOUNCE_EDIT_MSG_009_JPN', 'タイトル（翻訳）の文字数が100を超えています。');
define('ANNOUNCE_EDIT_MSG_009_ENG', 'The title (translation) contains more than 100 characters.');
define('ANNOUNCE_EDIT_MSG_010_JPN', '内容（原文）を入力して下さい。');
define('ANNOUNCE_EDIT_MSG_010_ENG', 'Please enter the content (original text).');
define('ANNOUNCE_EDIT_MSG_011_JPN', '内容（原文）の文字数が1000を超えています。');
define('ANNOUNCE_EDIT_MSG_011_ENG', 'The number of characters in the content (original text) exceeds 1000.');
define('ANNOUNCE_EDIT_MSG_012_JPN', '内容（原文）の文字数が5000を超えています。');
define('ANNOUNCE_EDIT_MSG_012_ENG', 'The number of characters in the content (original text) exceeds 5000.');
define('ANNOUNCE_EDIT_MSG_013_JPN', '内容（翻訳）を入力してください。');
define('ANNOUNCE_EDIT_MSG_013_ENG', 'Please enter the content (translation).');
define('ANNOUNCE_EDIT_MSG_014_JPN', '内容（翻訳）の文字数が1000を超えています。');
define('ANNOUNCE_EDIT_MSG_014_ENG', 'The number of characters in the content (translation) exceeds 1000.');
define('ANNOUNCE_EDIT_MSG_015_JPN', '内容（翻訳）の文字数が5000を超えています。');
define('ANNOUNCE_EDIT_MSG_015_ENG', 'The number of characters in the content (translation) exceeds 5000.');
define('ANNOUNCE_EDIT_MSG_016_JPN', 'タイトルと内容を入力してください。');
define('ANNOUNCE_EDIT_MSG_016_ENG', 'Please enter title and content.');
define('ANNOUNCE_EDIT_MSG_017_JPN', '完了にします。よろしいですか？');
define('ANNOUNCE_EDIT_MSG_017_ENG', 'To complete. Is it OK?');



//掲示板表示画面
define('BULLETIN_BOARD_VIEW_MSG_001_JPN', '既に削除された掲示板です。');
define('BULLETIN_BOARD_VIEW_MSG_001_ENG', 'A bulletin board that has already been deleted.');





//掲示板編集画面
define('BULLETIN_BOARD_EDIT_MSG_001_JPN', '正しくない処理が行われました。');
define('BULLETIN_BOARD_EDIT_MSG_001_ENG', 'Incorrect processing has been performed.');
define('BULLETIN_BOARD_EDIT_MSG_002_JPN', '掲示板を更新します。よろしいですか？');
define('BULLETIN_BOARD_EDIT_MSG_002_ENG', 'Update the bulletin board. Is it OK?');
define('BULLETIN_BOARD_EDIT_MSG_003_JPN', 'インシデント件名（翻訳）を入力してください。');
define('BULLETIN_BOARD_EDIT_MSG_003_ENG', 'Please enter the incident subject (translation).');
define('BULLETIN_BOARD_EDIT_MSG_004_JPN', 'インシデント件名（翻訳）の文字数が1000を超えています。');
define('BULLETIN_BOARD_EDIT_MSG_004_ENG', 'The number of characters in the incident subject (translation) exceeds 1000.');
define('BULLETIN_BOARD_EDIT_MSG_005_JPN', 'インシデント件数（翻訳）は半角英数字または特殊文字を入力してください。');
define('BULLETIN_BOARD_EDIT_MSG_005_ENG', 'Enter the number of incidents (translation) in single-byte alphanumeric characters or special characters.');
define('BULLETIN_BOARD_EDIT_MSG_006_JPN', 'インシデント件数（翻訳）に全角文字が含まれています。');
define('BULLETIN_BOARD_EDIT_MSG_006_ENG', 'Double-byte characters are included in the number of incidents (translation).');
define('BULLETIN_BOARD_EDIT_MSG_007_JPN', '既に削除された掲示板です。');
define('BULLETIN_BOARD_EDIT_MSG_007_ENG', 'A bulletin board that has already been deleted.');







//問い合わせ画面
define('QUERY_VIEW_MSG_001_JPN', '内容(原文)を入力してください。');
define('QUERY_VIEW_MSG_001_ENG', 'Please enter the content (original text).');
define('QUERY_VIEW_MSG_002_JPN', '内容(原文)の文字数が100を超えています。');
define('QUERY_VIEW_MSG_002_ENG', 'The number of characters in the content (original text) exceeds 200.');
define('QUERY_VIEW_MSG_003_JPN', '内容(翻訳)を入力してください。');
define('QUERY_VIEW_MSG_003_ENG', 'Please enter the content (translation).');
define('QUERY_VIEW_MSG_004_JPN', '内容(翻訳)の文字数が100を超えています。');
define('QUERY_VIEW_MSG_004_ENG', 'The number of characters in the content (translation) exceeds 200.');









//依頼事項登録編集画面
define('REQUEST_EDIT_MSG_001_JPN', '既に削除されたJCMG事案です。');
define('REQUEST_EDIT_MSG_001_ENG', 'Request item information that has already been deleted.');
define('REQUEST_EDIT_MSG_002_JPN', 'ファイルが見つかりませんでした。');
define('REQUEST_EDIT_MSG_002_ENG', 'File not found.');
define('REQUEST_EDIT_MSG_003_JPN', '依頼事項情報を投稿します。よろしいですか？');
define('REQUEST_EDIT_MSG_003_ENG', 'Post request information. Is it OK?');
define('REQUEST_EDIT_MSG_004_JPN', '既に削除された依頼事項情報です。');
define('REQUEST_EDIT_MSG_004_ENG', 'Request item information that has already been deleted.');


//依頼事項詳細表示画面
define('REQUEST_VIEW_MSG_001_JPN', 'ファイルが見つかりませんでした。');
define('REQUEST_VIEW_MSG_001_ENG', 'File not found.');
define('REQUEST_VIEW_MSG_002_JPN', '既に削除された依頼事項情報です。');
define('REQUEST_VIEW_MSG_002_ENG', 'Request item information that has already been deleted.');


//情報登録編集画面
define('INFORMATION_EDIT_MSG_001_JPN', '既に削除されたJCMG事案です。');
define('INFORMATION_EDIT_MSG_001_ENG', 'Information that has already been deleted.');
define('INFORMATION_EDIT_MSG_002_JPN', 'ファイルが見つかりませんでした。');
define('INFORMATION_EDIT_MSG_002_ENG', 'File not found.');
define('INFORMATION_EDIT_MSG_003_JPN', '情報を投稿します。よろしいですか？');
define('INFORMATION_EDIT_MSG_003_ENG', 'Post information. Is it OK?');
define('INFORMATION_EDIT_MSG_004_JPN', '本情報の確認時刻を入力して下さい。');
define('INFORMATION_EDIT_MSG_004_ENG', 'Enter the confirmation time of this information.');
define('INFORMATION_EDIT_MSG_005_JPN', '本情報の確認時刻には日時を入力して下さい。');
define('INFORMATION_EDIT_MSG_005_ENG', 'Enter the date and time for the confirmation time of this information.');
define('INFORMATION_EDIT_MSG_006_JPN', '本情報に関する問い合わせ先を入力して下さい。');
define('INFORMATION_EDIT_MSG_006_ENG', 'Enter the contact address for this information.');
define('INFORMATION_EDIT_MSG_007_JPN', '本情報に関する問い合わせ先は半角英数字または特殊文字を入力してください。');
define('INFORMATION_EDIT_MSG_007_ENG', 'For inquiries regarding this information, enter alphanumeric characters or special characters.');
define('INFORMATION_EDIT_MSG_008_JPN', '本情報に関する問い合わせ先の文字数が13を超えています。');
define('INFORMATION_EDIT_MSG_008_ENG', 'The number of characters for inquiries regarding this information exceeds 13 characters.');
define('INFORMATION_EDIT_MSG_009_JPN', '会社カテゴリが選択されていません。');
define('INFORMATION_EDIT_MSG_009_ENG', 'No company category has been selected.');
define('INFORMATION_EDIT_MSG_010_JPN', '情報カテゴリが選択されていません。');
define('INFORMATION_EDIT_MSG_010_ENG', 'No info category has been selected.');
define('INFORMATION_EDIT_MSG_011_JPN', '既に削除された情報です。');
define('INFORMATION_EDIT_MSG_011_ENG', 'Information that has already been deleted.');


//情報表示画面
define('INFORMATION_VIEW_MSG_001_JPN', 'ファイルが見つかりませんでした。');
define('INFORMATION_VIEW_MSG_001_ENG', 'File not found.');
define('INFORMATION_VIEW_MSG_002_JPN', '既に削除された情報です。');
define('INFORMATION_VIEW_MSG_002_ENG', 'Information that has already been deleted.');
define('INFORMATION_VIEW_MSG_003_JPN', '情報を削除します。よろしいですか？');
define('INFORMATION_VIEW_MSG_003_ENG', 'Delete information. Is it OK?');





//JCMG登録編集画面
define('INCIDENT_CASE_EDIT_MSG_001_JPN', '既に削除されたJCMG事案です。');
define('INCIDENT_CASE_EDIT_MSG_001_ENG', 'It is a JCMG case that has already been deleted.');
define('INCIDENT_CASE_EDIT_MSG_002_JPN', 'JCMG事案を投稿します。よろしいですか？');
define('INCIDENT_CASE_EDIT_MSG_002_ENG', 'Submit a JCMG case. Is it OK?');
define('INCIDENT_CASE_EDIT_MSG_003_JPN', '開始日に値がありません。');
define('INCIDENT_CASE_EDIT_MSG_003_ENG', 'Start date has no value.');
define('INCIDENT_CASE_EDIT_MSG_004_JPN', '開始日には日時を入力してください。');
define('INCIDENT_CASE_EDIT_MSG_004_ENG', 'Please enter a date and time for the start date.');
define('INCIDENT_CASE_EDIT_MSG_005_JPN', '完了していないJCMG事案があるため投稿できません。');
define('INCIDENT_CASE_EDIT_MSG_005_ENG', 'You cannot post because there is a JCMG case that has not been completed.');
define('INCIDENT_CASE_EDIT_MSG_006_JPN', '完了にします。よろしいですか？');
define('INCIDENT_CASE_EDIT_MSG_006_ENG', 'To complete. Is it OK?');
define('INCIDENT_CASE_EDIT_MSG_007_JPN', '既に完了したJCMG事案です。');
define('INCIDENT_CASE_EDIT_MSG_007_ENG', 'This is a completed JCMG case.');


//JCMG事案表示画面
define('INCIDENT_CASE_VIEW_MSG_001_JPN', '既に削除されたJCMG事案です。');
define('INCIDENT_CASE_VIEW_MSG_001_ENG', 'It is a JCMG case that has already been deleted.');











//ユーザ設定画面
define('USER_SETTING_MSG_001_JPN', 'ユーザ情報を更新します。よろしいですか？');
define('USER_SETTING_MSG_001_ENG', 'Update user information. Is it OK?');
define('USER_SETTING_MSG_002_JPN', '住所の文字数が100を超えています。');
define('USER_SETTING_MSG_002_ENG', 'The number of characters in the address exceeds 100 bytes.');
define('USER_SETTING_MSG_003_JPN', '組織の文字数が50を超えています。');
define('USER_SETTING_MSG_003_ENG', 'Organization has more than 50 characters.');
define('USER_SETTING_MSG_004_JPN', '名前の文字数が20を超えています。');
define('USER_SETTING_MSG_004_ENG', 'The name has more than 20 characters.');
define('USER_SETTING_MSG_005_JPN', 'メールに値がありません。');
define('USER_SETTING_MSG_005_ENG', 'Email has no value.');
define('USER_SETTING_MSG_006_JPN', 'メールは半角英数字または特殊文字を入力してください。');
define('USER_SETTING_MSG_006_ENG', 'Enter half-width alphanumeric characters or special characters for the mail.');
define('USER_SETTING_MSG_007_JPN', 'メールの文字数が50を超えています。');
define('USER_SETTING_MSG_007_ENG', 'The number of characters in the email exceeds 50.');
define('USER_SETTING_MSG_008_JPN', '電話は半角英数字または特殊文字を入力してください。');
define('USER_SETTING_MSG_008_ENG', 'Enter alphanumeric characters or special characters for the phone.');
define('USER_SETTING_MSG_009_JPN', '電話の文字数が20を超えています。');
define('USER_SETTING_MSG_009_ENG', 'Phone has more than 20 characters.');
define('USER_SETTING_MSG_010_JPN', 'FAXは半角英数字または特殊文字を入力してください。');
define('USER_SETTING_MSG_010_ENG', 'Enter one-byte alphanumeric characters or special characters for fax.');
define('USER_SETTING_MSG_011_JPN', 'FAXの文字数が20を超えています。');
define('USER_SETTING_MSG_011_ENG', 'The number of characters of the fax exceeds 20.');
define('USER_SETTING_MSG_012_JPN', 'パスワードに値がありません。');
define('USER_SETTING_MSG_012_ENG', 'Password has no value.');
define('USER_SETTING_MSG_013_JPN', 'パスワードは半角英数字または特殊文字を入力してください。');
define('USER_SETTING_MSG_013_ENG', 'Enter alphanumeric characters or special characters for the password.');
define('USER_SETTING_MSG_014_JPN', 'パスワードの文字数は、12以上および30以下で入力してください。');
define('USER_SETTING_MSG_014_ENG', 'Please enter a password of 12 or more and 30 or less.');
define('USER_SETTING_MSG_015_JPN', 'パスワードは大文字・小文字の両方を含んでください。');
define('USER_SETTING_MSG_015_ENG', 'The password must contain both uppercase and lowercase letters.');
define('USER_SETTING_MSG_016_JPN', '言語が選択されていません。');
define('USER_SETTING_MSG_016_ENG', 'No language selected.');







//ユーザ管理画面
define('USER_MNG_MSG_001_JPN', 'ユーザ情報を削除します。よろしいですか？');
define('USER_MNG_MSG_001_ENG', 'Delete user information. Is it OK?');








//ユーザ新規登録・編集画面
define('USER_EDIT_MSG_001_JPN', 'ユーザ情報を登録します。よろしいですか？');
define('USER_EDIT_MSG_001_ENG', 'Register user information. Is it OK?');
define('USER_EDIT_MSG_002_JPN', 'ユーザIDに値がありません。');
define('USER_EDIT_MSG_002_ENG', 'User ID has no value.');
define('USER_EDIT_MSG_003_JPN', 'ユーザIDは半角英数字または特殊文字を入力してください。');
define('USER_EDIT_MSG_003_ENG', 'Enter a user ID using alphanumeric characters or special characters.');
define('USER_EDIT_MSG_004_JPN', 'ユーザIDの文字数が20を超えています。');
define('USER_EDIT_MSG_004_ENG', 'The number of characters of the user ID exceeds 20.');
define('USER_EDIT_MSG_005_JPN', 'ユーザIDは既に登録されています。');
define('USER_EDIT_MSG_005_ENG', 'User ID is already registered.');
define('USER_EDIT_MSG_006_JPN', 'パスワードに値がありません。');
define('USER_EDIT_MSG_006_ENG', 'Password has no value.');
define('USER_EDIT_MSG_007_JPN', 'パスワードは半角英数字または特殊文字を入力してください。');
define('USER_EDIT_MSG_007_ENG', 'Enter alphanumeric characters or special characters for the password.');
define('USER_EDIT_MSG_008_JPN', 'パスワードの文字数は、12以上および30以下で入力してください。');
define('USER_EDIT_MSG_008_ENG', 'Please enter a password of 12 or more and 30 or less.');
define('USER_EDIT_MSG_009_JPN', 'パスワードは大文字・小文字の両方を含んでください。');
define('USER_EDIT_MSG_009_ENG', 'The password must contain both uppercase and lowercase letters.');
define('USER_EDIT_MSG_010_JPN', '開始日に値がありません。');
define('USER_EDIT_MSG_010_ENG', 'Start date has no value.');
define('USER_EDIT_MSG_011_JPN', '開始日に日付を入力してください。');
define('USER_EDIT_MSG_011_ENG', 'Please enter a date for the start date.');
define('USER_EDIT_MSG_012_JPN', '終了日に値がありません。');
define('USER_EDIT_MSG_012_ENG', 'End date has no value.');
define('USER_EDIT_MSG_013_JPN', '終了日に日付を入力してください。');
define('USER_EDIT_MSG_013_ENG', 'Please enter a date for the end date.');
define('USER_EDIT_MSG_014_JPN', '有効期限の開始日は終了日よりも前の日付を入力してください。');
define('USER_EDIT_MSG_014_ENG', 'Expiration start date must be before end date.');
define('USER_EDIT_MSG_015_JPN', '言語が選択されていません。');
define('USER_EDIT_MSG_015_ENG', 'No language selected.');
define('USER_EDIT_MSG_016_JPN', '会社名が選択されていません。');
define('USER_EDIT_MSG_016_ENG', 'No company name has been selected.');
define('USER_EDIT_MSG_017_JPN', '組織の文字数が50を超えています。');
define('USER_EDIT_MSG_017_ENG', 'Organization has more than 50 characters.');
define('USER_EDIT_MSG_018_JPN', '担当者名の文字数が20を超えています。');
define('USER_EDIT_MSG_018_ENG', 'The name has more than 20 characters.');
define('USER_EDIT_MSG_019_JPN', 'メールに値がありません。');
define('USER_EDIT_MSG_019_ENG', 'Email has no value.');
define('USER_EDIT_MSG_020_JPN', 'メールは半角英数字または特殊文字を入力してください。');
define('USER_EDIT_MSG_020_ENG', 'Enter half-width alphanumeric characters or special characters for the mail.');
define('USER_EDIT_MSG_021_JPN', 'メールの文字数が50を超えています。');
define('USER_EDIT_MSG_021_ENG', 'The number of characters in the email exceeds 50.');
define('USER_EDIT_MSG_022_JPN', '電話は半角英数字または特殊文字を入力してください。');
define('USER_EDIT_MSG_022_ENG', 'Enter alphanumeric characters or special characters for the phone.');
define('USER_EDIT_MSG_023_JPN', '電話の文字数が20を超えています。');
define('USER_EDIT_MSG_023_ENG', 'Phone has more than 20 characters.');
define('USER_EDIT_MSG_024_JPN', 'FAXは半角英数字または特殊文字を入力してください。');
define('USER_EDIT_MSG_024_ENG', 'Enter one-byte alphanumeric characters or special characters for fax.');
define('USER_EDIT_MSG_025_JPN', 'FAXの文字数が20を超えています。');
define('USER_EDIT_MSG_025_ENG', 'The number of characters of the fax exceeds 20.');
define('USER_EDIT_MSG_026_JPN', '郵便番号は半角英数字または特殊文字を入力してください。');
define('USER_EDIT_MSG_026_ENG', 'Postal code must be alphanumeric or special characters.');
define('USER_EDIT_MSG_027_JPN', '郵便番号の文字数が8を超えています。');
define('USER_EDIT_MSG_027_ENG', 'Postal code contains more than 8 characters.');
define('USER_EDIT_MSG_028_JPN', '住所の文字数が100を超えています。');
define('USER_EDIT_MSG_028_ENG', 'The number of characters in the address exceeds 100 bytes.');
define('USER_EDIT_MSG_029_JPN', '備考の文字数が200を超えています。');
define('USER_EDIT_MSG_029_ENG', 'The number of characters in the remarks exceeds 200.');


//ユーザ別権限設定画面
define('USER_PERM_MSG_001_JPN', 'ユーザ情報を登録します。よろしいですか？');
define('USER_PERM_MSG_001_ENG', 'Register user information. Is it OK?');




//お知らせ管理画面
define('ANNOUNCE_MNG_MSG_001_JPN', '完了にします。よろしいですか？');
define('ANNOUNCE_MNG_MSG_001_ENG', 'To complete. Is it OK?');
define('ANNOUNCE_MNG_MSG_002_JPN', 'お知らせを削除します。よろしいですか？');
define('ANNOUNCE_MNG_MSG_002_ENG', 'Delete announcement. Is it OK?');
define('ANNOUNCE_MNG_MSG_003_JPN', '期間には日付を入力してください。');
define('ANNOUNCE_MNG_MSG_003_ENG', 'Date must be a date');
define('ANNOUNCE_MNG_MSG_004_JPN', '期間開始日付は期間終了日付よりも前の日付を入力してください。');
define('ANNOUNCE_MNG_MSG_004_ENG', 'Period start date must be earlier than period end date.');
define('ANNOUNCE_MNG_MSG_005_JPN', '既に削除されたお知らせです。');
define('ANNOUNCE_MNG_MSG_005_ENG', 'It is a notification that has already been deleted.');





//掲示板管理画面
define('BULLETIN_BOARD_MNG_MSG_001_JPN', '掲示板を削除します。よろしいですか？');
define('BULLETIN_BOARD_MNG_MSG_001_ENG', 'Delete a bulletin board. Is it OK?');
define('BULLETIN_BOARD_MNG_MSG_002_JPN', '期間には日付を入力してください。');
define('BULLETIN_BOARD_MNG_MSG_002_ENG', 'Date must be a date');
define('BULLETIN_BOARD_MNG_MSG_003_JPN', '期間開始日付は期間終了日付よりも前の日付を入力してください。');
define('BULLETIN_BOARD_MNG_MSG_003_ENG', 'Period start date must be earlier than period end date');







//リンク情報管理画面
define('LINK_MNG_MSG_001_JPN', 'リンク情報を削除します。よろしいですか？');
define('LINK_MNG_MSG_001_ENG', 'Delete link information. Is it OK?');
define('LINK_MNG_MSG_002_JPN', 'リンク情報の並び替えに失敗しました。');
define('LINK_MNG_MSG_002_ENG', 'Sorting of link information failed.');


//リンク情報新規登録・編集画面
define('LINK_EDIT_MSG_001_JPN', 'リンク情報を登録します。よろしいですか？');
define('LINK_EDIT_MSG_001_ENG', 'Register the link information. Is it OK?');
define('LINK_EDIT_MSG_002_JPN', 'カテゴリを選択してください。');
define('LINK_EDIT_MSG_002_ENG', 'Please select a category.');
define('LINK_EDIT_MSG_003_JPN', 'リンク名を入力してください。');
define('LINK_EDIT_MSG_003_ENG', 'Please enter a link name.');
define('LINK_EDIT_MSG_004_JPN', 'リンク名の文字数が50を超えています。');
define('LINK_EDIT_MSG_004_ENG', 'The number of characters in the link name exceeds 50.');
define('LINK_EDIT_MSG_005_JPN', 'リンク名(翻訳)を入力してください。');
define('LINK_EDIT_MSG_005_ENG', 'Please enter a link name (translation).');
define('LINK_EDIT_MSG_006_JPN', 'リンク名(翻訳)の文字数が100を超えています。');
define('LINK_EDIT_MSG_006_ENG', 'The number of characters in the link name (translation) exceeds 100.');
define('LINK_EDIT_MSG_007_JPN', 'リンク先を入力してください。');
define('LINK_EDIT_MSG_007_ENG', 'Please enter a link.');
define('LINK_EDIT_MSG_008_JPN', 'リンク先は半角英数字または特殊文字を入力してください。');
define('LINK_EDIT_MSG_008_ENG', 'Enter alphanumeric characters or special characters for the link.');
define('LINK_EDIT_MSG_009_JPN', 'リンク先の文字数が1000を超えています。');
define('LINK_EDIT_MSG_009_ENG', 'The number of characters of the link destination exceeds 1000.');
define('LINK_EDIT_MSG_010_JPN', 'ファイルが存在しません。');
define('LINK_EDIT_MSG_010_ENG', 'File does not exist.');
define('LINK_EDIT_MSG_011_JPN', '拡張子が不適切です。画像ファイルを選択して下さい。');
define('LINK_EDIT_MSG_011_ENG', 'The extension is incorrect. Please select an image file.');
define('LINK_EDIT_MSG_012_JPN', 'ファイル名の文字数が100を超えています。');
define('LINK_EDIT_MSG_012_ENG', 'The number of characters in the file name exceeds 100.');
define('LINK_EDIT_MSG_013_JPN', 'ファイル画像の縦・横サイズが規定値を超えています。');
define('LINK_EDIT_MSG_013_ENG', 'The vertical and horizontal size of the file image exceeds the specified value.');
define('LINK_EDIT_MSG_014_JPN', '既に削除されたリンク情報です。');
define('LINK_EDIT_MSG_014_ENG', 'Link information that has already been deleted.');



//JC<G事案管理画面
define('INCIDENT_CASE_MNG_MSG_001_JPN', '期間には日付を入力してください。');
define('INCIDENT_CASE_MNG_MSG_001_ENG', 'Date must be a date');
define('INCIDENT_CASE_MNG_MSG_002_JPN', '期間開始日付は期間終了日付よりも前の日付を入力してください。');
define('INCIDENT_CASE_MNG_MSG_002_ENG', 'Period start date must be earlier than period end date');
define('INCIDENT_CASE_MNG_MSG_003_JPN', '完了にします。よろしいですか？');
define('INCIDENT_CASE_MNG_MSG_003_ENG', 'To complete. Is it OK?');
define('INCIDENT_CASE_MNG_MSG_004_JPN', 'JCMG事案を削除します。よろしいですか？');
define('INCIDENT_CASE_MNG_MSG_004_ENG', 'Delete an JCMG case. Is it OK?');
define('INCIDENT_CASE_MNG_MSG_005_JPN', '完了を取り消します。よろしいですか？');
define('INCIDENT_CASE_MNG_MSG_005_ENG', 'Cancel completion. Is it OK?');
define('INCIDENT_CASE_MNG_MSG_006_JPN', '完了していないJCMG事案があるので完了を取り消せません。');
define('INCIDENT_CASE_MNG_MSG_006_ENG', 'The completion cannot be canceled because there is an JCMG that has not been completed.');
define('INCIDENT_CASE_MNG_MSG_007_JPN', '既に削除されたJCMG事案です。');
define('INCIDENT_CASE_MNG_MSG_007_ENG', 'It is a JCMG case that has already been deleted.');



//会社情報管理画面
define('COMPANY_MNG_MSG_001_JPN', '会社情報を削除します。よろしいですか？');
define('COMPANY_MNG_MSG_001_ENG', 'Delete company information. Is it OK?');



//会社情報新規登録・編集画面
define('COMPANY_EDIT_MSG_001_JPN', '会社情報を登録します。よろしいですか？');
define('COMPANY_EDIT_MSG_001_ENG', 'Register company information. Is it OK?');
define('COMPANY_EDIT_MSG_002_JPN', '会社名に値がありません。');
define('COMPANY_EDIT_MSG_002_ENG', 'Company name has no value');
define('COMPANY_EDIT_MSG_003_JPN', '会社名の文字数が20を超えています。');
define('COMPANY_EDIT_MSG_003_ENG', 'The number of characters in the company name exceeds 20.');
define('COMPANY_EDIT_MSG_004_JPN', '会社名（英語）に値がありません。');
define('COMPANY_EDIT_MSG_004_ENG', 'Company name (English) has no value.');
define('COMPANY_EDIT_MSG_005_JPN', '会社（英語）の文字数が50を超えています。');
define('COMPANY_EDIT_MSG_005_ENG', 'Company (English) has more than 50 characters.');
define('COMPANY_EDIT_MSG_006_JPN', '会社（英語）は半角英数字または特殊文字を入力してください。');
define('COMPANY_EDIT_MSG_006_ENG', 'Company (English), please enter alphanumeric characters or special characters.');
define('COMPANY_EDIT_MSG_007_JPN', '略称名に値がありません。');
define('COMPANY_EDIT_MSG_007_ENG', 'Abbreviated name has no value.');
define('COMPANY_EDIT_MSG_008_JPN', '略称名の文字数が20を超えています。');
define('COMPANY_EDIT_MSG_008_ENG', 'Abbreviated name exceeds 20 characters.');
define('COMPANY_EDIT_MSG_009_JPN', '略称名（英語）に値がありません。');
define('COMPANY_EDIT_MSG_009_ENG', 'Abbreviated name (English) has no value.');
define('COMPANY_EDIT_MSG_010_JPN', '略称名（英語）の文字数が50を超えています。');
define('COMPANY_EDIT_MSG_010_ENG', 'Abbreviated name (English) exceeds 50 characters.');
define('COMPANY_EDIT_MSG_011_JPN', '略称名（英語）は半角英数字または特殊文字を入力してください。');
define('COMPANY_EDIT_MSG_011_ENG', 'Please enter one-byte alphanumeric characters or special characters for the abbreviation name (English).');
define('COMPANY_EDIT_MSG_012_JPN', '機関カテゴリが選択されていません。');
define('COMPANY_EDIT_MSG_012_ENG', 'No institution category has been selected.');
define('COMPANY_EDIT_MSG_013_JPN', 'グループが選択されていません。');
define('COMPANY_EDIT_MSG_013_ENG', 'No groups have been selected.');
define('COMPANY_EDIT_MSG_014_JPN', '既に削除された会社情報です。');
define('COMPANY_EDIT_MSG_014_ENG', 'This is the company information that has already been deleted.');
define('COMPANY_EDIT_MSG_015_JPN', '会社名は既に登録されています。');
define('COMPANY_EDIT_MSG_015_ENG', 'Company name is already registered.');
define('COMPANY_EDIT_MSG_016_JPN', '会社名（英語）は既に登録されています。');
define('COMPANY_EDIT_MSG_016_ENG', 'Company name (English) is already registered.');
define('COMPANY_EDIT_MSG_017_JPN', '略称名は既に登録されています。');
define('COMPANY_EDIT_MSG_017_ENG', 'The short name has already been registered.');
define('COMPANY_EDIT_MSG_018_JPN', '略称名（英語）は既に登録されています。');
define('COMPANY_EDIT_MSG_018_ENG', 'The short name (English) has already been registered.');





//パスワード変更画面
define('PASSWORD_CHG_MSG_001_JPN', 'パスワードの変更を適用します。よろしいですか？');
define('PASSWORD_CHG_MSG_001_ENG', 'Apply password changes. Is it OK?');
define('PASSWORD_CHG_MSG_002_JPN', '現在のパスワードに値がありません。');
define('PASSWORD_CHG_MSG_002_ENG', 'The current password has no value.');
define('PASSWORD_CHG_MSG_003_JPN', '新しいパスワードに値がありません。');
define('PASSWORD_CHG_MSG_003_ENG', 'New password has no value.');
define('PASSWORD_CHG_MSG_004_JPN', '新しいパスワードは半角英数字または特殊文字を入力してください。');
define('PASSWORD_CHG_MSG_004_ENG', 'Enter a new password using alphanumeric characters or special characters.');
define('PASSWORD_CHG_MSG_005_JPN', '新しいパスワードの文字数は、12以上および30以下で入力してください。');
define('PASSWORD_CHG_MSG_005_ENG', 'Please enter a new password with at least 12 and no more than 30 characters.');
define('PASSWORD_CHG_MSG_006_JPN', '新しいパスワードは大文字・小文字の両方を含んでください。');
define('PASSWORD_CHG_MSG_006_ENG', 'The new password must contain both uppercase and lowercase letters.');
define('PASSWORD_CHG_MSG_007_JPN', '新しいパスワード(確認用)に値がありません。');
define('PASSWORD_CHG_MSG_007_ENG', 'New password (for confirmation) has no value.');
define('PASSWORD_CHG_MSG_008_JPN', 'ログインユーザのパスワードと現在のパスワードが一致しません。');
define('PASSWORD_CHG_MSG_008_ENG', 'The password of the login user does not match the current password.');
define('PASSWORD_CHG_MSG_009_JPN', 'ログインユーザのパスワードと新しいパスワードは異なるパスワードを入力してください。');
define('PASSWORD_CHG_MSG_009_ENG', 'Enter a different password for the login user password and the new password.');
define('PASSWORD_CHG_MSG_010_JPN', '新しいパスワードと確認用パスワードが一致しません。');
define('PASSWORD_CHG_MSG_010_ENG', 'New password and confirmation password do not match.');
define('PASSWORD_CHG_MSG_011_JPN', 'パスワードの変更が完了しました。');
define('PASSWORD_CHG_MSG_011_ENG', 'The password change is complete.');



//アラート表示システム連携画面
define('BULLETIN_BOARD_MAIL_MSG_001', 'パラメータ「ステータス」の値が正しくありません。');
define('BULLETIN_BOARD_MAIL_MSG_002', 'パラメータ「インシデントNo」の値が正しくありません。');
define('BULLETIN_BOARD_MAIL_MSG_003', 'パラメータ「インシデント件名」の値が正しくありません。');
define('BULLETIN_BOARD_MAIL_MSG_004', 'パラメータ「業務名」の値が正しくありません。');
define('BULLETIN_BOARD_MAIL_MSG_005', 'パラメータ「発生日時」の値が正しくありません。');
define('BULLETIN_BOARD_MAIL_MSG_006', 'パラメータ「場所名」の値が正しくありません。');
define('BULLETIN_BOARD_MAIL_MSG_007', 'パラメータ「場所名（英語）」の値が正しくありません。');
define('BULLETIN_BOARD_MAIL_MSG_008', 'パラメータ「地図ID」の値が正しくありません。');
define('BULLETIN_BOARD_MAIL_MSG_009', 'パラメータ「場所3ID」の値が正しくありません。');
define('BULLETIN_BOARD_MAIL_MSG_010', 'パラメータ「完了日時」の値が正しくありません。');
define('BULLETIN_BOARD_MAIL_MSG_011', '掲示板メール通知対象者の取得に失敗しました。');
define('BULLETIN_BOARD_MAIL_MSG_012', '掲示板の完了に失敗しました。');
define('BULLETIN_BOARD_MAIL_MSG_013', '掲示板の登録に失敗しました。');
define('BULLETIN_BOARD_MAIL_MSG_014', '掲示板の更新に失敗しました。');
define('BULLETIN_BOARD_MAIL_MSG_015', '更新対象の掲示板が存在しません。');
define('BULLETIN_BOARD_MAIL_MSG_016', '完了対象の掲示板が存在しません。');


//当日ログ出力画面
define('LOG_TODAY_MSG_001', 'ログデータがありません。');




//共通メッセージ
define('PUBLIC_MSG_001_JPN', 'データ取得に失敗しました。');
define('PUBLIC_MSG_001_ENG', 'Data acquisition failed');
define('PUBLIC_MSG_002_JPN', 'データ登録に失敗しました。');
define('PUBLIC_MSG_002_ENG', 'Data registration failed');
define('PUBLIC_MSG_003_JPN', 'データ更新に失敗しました。');
define('PUBLIC_MSG_003_ENG', 'Data update failed');
define('PUBLIC_MSG_004_JPN', 'データ削除に失敗しました。');
define('PUBLIC_MSG_004_ENG', 'Data deletion failed');
define('PUBLIC_MSG_005_JPN', 'CSV出力に失敗しました。');
define('PUBLIC_MSG_005_ENG', 'CSV output failed.');
define('PUBLIC_MSG_006_JPN', '%0%件、データ削除を行いますか？');
define('PUBLIC_MSG_006_ENG', 'Do you want to delete %0% data?');
define('PUBLIC_MSG_007_JPN', '一括削除に失敗しました。');
define('PUBLIC_MSG_007_ENG', 'Batch deletion failed.');
define('PUBLIC_MSG_008_JPN', 'ログインが行われていません。');
define('PUBLIC_MSG_008_ENG', 'You are not logged in.');
define('PUBLIC_MSG_009_JPN', 'セッション情報が正しくありません。');
define('PUBLIC_MSG_009_ENG', 'Session information is incorrect.');
define('PUBLIC_MSG_010_JPN', '現在、Amazon翻訳サービスは利用できませんので、のちほど、再登録してください');
define('PUBLIC_MSG_010_ENG', 'Currently, Amazon translation service is not available, please re-register later');
define('PUBLIC_MSG_011_JPN', '【添付ファイル1】');
define('PUBLIC_MSG_011_ENG', '[Attached file 1]');
define('PUBLIC_MSG_012_JPN', '【添付ファイル2】');
define('PUBLIC_MSG_012_ENG', '[Attached file 2]');
define('PUBLIC_MSG_013_JPN', '【添付ファイル3】');
define('PUBLIC_MSG_013_ENG', '[Attached file 3]');
define('PUBLIC_MSG_014_JPN', '【添付ファイル4】');
define('PUBLIC_MSG_014_ENG', '[Attached file 4]');
define('PUBLIC_MSG_015_JPN', '【添付ファイル5】');
define('PUBLIC_MSG_015_ENG', '[Attached file 5]');
define('PUBLIC_MSG_016_JPN', 'ファイルが存在しません。');
define('PUBLIC_MSG_016_ENG', 'File does not exist.');
define('PUBLIC_MSG_017_JPN', '拡張子が不適切です。');
define('PUBLIC_MSG_017_ENG', 'The extension is incorrect.');
define('PUBLIC_MSG_018_JPN', 'ファイル名の文字数が100を超えています。');
define('PUBLIC_MSG_018_ENG', 'The number of characters in the file name exceeds 100.');
define('PUBLIC_MSG_019_JPN', '添付ファイルの合計容量が10MBを超えています。');
define('PUBLIC_MSG_019_ENG', 'The total size of the attached file exceeds 10MB.');
define('PUBLIC_MSG_020_JPN', '自動翻訳の注意喚起メッセージが表示されなくなります。');
define('PUBLIC_MSG_020_ENG', 'The warning message for automatic translation will not be displayed.');
define('PUBLIC_MSG_021_JPN', 'タイトル（原文）を入力して下さい。');
define('PUBLIC_MSG_021_ENG', 'Please enter the title (original text).');
define('PUBLIC_MSG_022_JPN', 'タイトル（原文）の文字数が30を超えています。');
define('PUBLIC_MSG_022_ENG', 'Title (original text) exceeds 30 characters.');
define('PUBLIC_MSG_023_JPN', 'タイトル（原文）は半角英数字または特殊文字を入力してください。');
define('PUBLIC_MSG_023_ENG', 'Enter the title (original text) in single-byte alphanumeric characters or special characters.');
define('PUBLIC_MSG_024_JPN', 'タイトル（原文）の文字数が150を超えています。');
define('PUBLIC_MSG_024_ENG', 'Title (original text) exceeds 150 characters.');
define('PUBLIC_MSG_025_JPN', 'タイトル（原文）に全角文字が含まれています。');
define('PUBLIC_MSG_025_ENG', 'The title (original text) contains double-byte characters.');
define('PUBLIC_MSG_026_JPN', '内容（原文）を入力して下さい。');
define('PUBLIC_MSG_026_ENG', 'Please enter the content (original text).');
define('PUBLIC_MSG_027_JPN', '内容（原文）の文字数が1000を超えています。');
define('PUBLIC_MSG_027_ENG', 'The number of characters in the content (original text) exceeds 1000.');
define('PUBLIC_MSG_028_JPN', '内容（原文）は半角英数字または特殊文字を入力してください。');
define('PUBLIC_MSG_028_ENG', 'Please enter half-width alphanumeric characters or special characters for the content (original text).');
define('PUBLIC_MSG_029_JPN', '内容（原文）の文字数が5000を超えています。');
define('PUBLIC_MSG_029_ENG', 'The number of characters in the content (original text) exceeds 5000.');
define('PUBLIC_MSG_030_JPN', '内容（原文）に全角文字が含まれています。');
define('PUBLIC_MSG_030_ENG', 'The content (original text) contains double-byte characters.');
define('PUBLIC_MSG_031_JPN', 'タイトル（翻訳）を入力して下さい。');
define('PUBLIC_MSG_031_ENG', 'Please enter a title (translation).');
define('PUBLIC_MSG_032_JPN', 'タイトル（翻訳）の文字数が30を超えています。');
define('PUBLIC_MSG_032_ENG', 'The title (translation) contains more than 30 characters.');
define('PUBLIC_MSG_033_JPN', 'タイトル（翻訳）は半角英数字または特殊文字を入力してください。');
define('PUBLIC_MSG_033_ENG', 'Please enter half-width alphanumeric characters or special characters for the title (translation).');
define('PUBLIC_MSG_034_JPN', 'タイトル（翻訳）の文字数が150を超えています。');
define('PUBLIC_MSG_034_ENG', 'The title (translation) contains more than 150 characters.');
define('PUBLIC_MSG_035_JPN', 'タイトル（翻訳）に全角文字が含まれています。');
define('PUBLIC_MSG_035_ENG', 'The title (translation) contains double-byte characters.');
define('PUBLIC_MSG_036_JPN', '内容（翻訳）を入力してください。');
define('PUBLIC_MSG_036_ENG', 'Please enter the content (translation).');
define('PUBLIC_MSG_037_JPN', '内容（翻訳）の文字数が1000を超えています。');
define('PUBLIC_MSG_037_ENG', 'The number of characters in the content (translation) exceeds 1000.');
define('PUBLIC_MSG_038_JPN', '内容（翻訳）は半角英数字または特殊文字を入力してください。');
define('PUBLIC_MSG_038_ENG', 'Please enter half-width alphanumeric characters or special characters for the content (translation).');
define('PUBLIC_MSG_039_JPN', '内容（翻訳）の文字数が5000を超えています。');
define('PUBLIC_MSG_039_ENG', 'The number of characters in the content (translation) exceeds 5000.');
define('PUBLIC_MSG_040_JPN', '内容（翻訳）に全角文字が含まれています。');
define('PUBLIC_MSG_040_ENG', 'The content (translation) contains double-byte characters.');
define('PUBLIC_MSG_041_JPN', '現在、Amazon翻訳サービスが利用できませんが、投稿を行いますか？');
define('PUBLIC_MSG_041_ENG', 'Currently, Amazon translation service is not available, do you want to post?');
define('PUBLIC_MSG_042_JPN', '翻訳結果はありません。');
define('PUBLIC_MSG_042_ENG', 'There is no translation result.');
define('PUBLIC_MSG_043_JPN', '既にJCMG事案は削除されています。');
define('PUBLIC_MSG_043_ENG', 'The JCMG case has already been deleted.');
define('PUBLIC_MSG_044_JPN', 'セッションがタイムアウトしました。');
define('PUBLIC_MSG_044_ENG', 'Your session has timed out.');
define('PUBLIC_MSG_045_JPN', '内容（原文）の文字数が200を超えています。');
define('PUBLIC_MSG_045_ENG', 'The number of characters in the content (original text) exceeds 200.');
define('PUBLIC_MSG_046_JPN', '内容（原文）の文字数が1000を超えています。');
define('PUBLIC_MSG_046_ENG', 'The number of characters in the content (original text) exceeds 1000.');
define('PUBLIC_MSG_047_JPN', '内容（翻訳）の文字数が200を超えています。');
define('PUBLIC_MSG_047_ENG', 'The number of characters in the content (translation) exceeds 200.');
define('PUBLIC_MSG_048_JPN', '内容（翻訳）の文字数が1000を超えています。');
define('PUBLIC_MSG_048_ENG', 'The number of characters in the content (translation) exceeds 1000.');
define('PUBLIC_MSG_049_JPN', '正しくない処理が行われました。');
define('PUBLIC_MSG_049_ENG', 'Incorrect processing has been performed.');
define('PUBLIC_MSG_050_JPN', 'ファイル名に使用できない文字が含まれています。');
define('PUBLIC_MSG_050_ENG', 'The file name contains characters that cannot be used.');
define('PUBLIC_MSG_051_JPN', 'ファイル名と拡張子が重複したファイルがあります。');
define('PUBLIC_MSG_051_ENG', 'Some files have the same file name and extension.');
?>