<?php
/*
 * @system_const.php
 * 共通定数定義ファイル
 *
 *  編集する可能性があまりないものを定義
 *
 * @create 2020/07/09 KBS K.Yoshihara
 *
 * @update
 */

return [

	// ユーザー
	'guest_user' => 'guest', // guestユーザー名

	// 画面上に表示する汎用的な文字列
	'null_display_value' => '-', // null値

	// システムログへ出力する動作1
	'syslog_action1_login' => 'LOGIN', // ログイン
	'syslog_action1_menu' => 'MENU', // メニュー

	// システムログへ出力する動作2
	'syslog_action2_registered' => '登録ユーザ', // DBに登録されているユーザ
	'syslog_action2_unregistered' => '未登録ユーザ', // DBに登録されていないユーザ
	'syslog_action2_getusererror' => 'ユーザID取得失敗', // ユーザID取得失敗
	'syslog_action2_sort' => 'ソート', // ソート

	// JSON戻り値
	'json_status_ok' => 'OK', // 成功時
	'json_status_ng' => 'NG', // 失敗時
	'json_return_true' => 'true', // 真偽値の真
	'json_return_false' => 'false', // 真偽値の偽

	// 権限
	'authority_all' => 0, // 無制限
	'authority_readonly' => 1, // 閲覧
	'authority_editable' => 2, // 編集

	// 排他ロック関連
	'lock_heart_beat_interval_sec' => 60, // 生存通知間隔(秒)
	'lock_heart_beat_lifetime_sec' => 180, // 生存通知の有効期間(秒)
	'lock_open_lifetime_sec' => 3600, // ページを開いてから何秒まで生存通知を有効にするか
	'lock_option_key_general' => '0', // ロックテーブルのオプションキーデフォルト

	// 記号などの文字列
	'code_name_separator' => '：', // コードと名称の区切り記号

	// 職制関連
	'org_root_name' => '親無し', // ルート職制の名称
	'org_null_name' => '---', // 職制未選択の名称

	// ボタンの設定
	// 参照
	'btn_color_info' => 'btn btn-outline-primary',		// 色
	'btn_img_info' => '',      							// 画像
	'btn_char_info' => '参照',                        	// 文字
	// テーブル行内の参照
	'btn_color_rowinfo' => 'btn btn-outline-primary btn-sm',	// 色
	'btn_img_rowinfo' => '',               				// 画像
	'btn_char_rowinfo' => '参照',                       // 文字
	// 編集
	'btn_color_edit' => 'btn btn-outline-primary',      // 色
	'btn_img_edit' => '',                  				// 画像
	'btn_char_edit' => '編集',                        	// 文字
	// テーブル行内の編集
	'btn_color_rowedit' => 'btn btn-outline-primary btn-sm',   // 色
	'btn_img_rowedit' => '',               				// 画像
	'btn_char_rowedit' => '編集',                       // 文字
	// 削除
	'btn_color_delete' => 'btn btn-outline-primary',    // 色
	'btn_img_delete' => '',               				// 画像
	'btn_char_delete' => '削除',                      	// 文字
	// テーブル行内の削除
	'btn_color_rowdelete' => 'btn btn-outline-primary btn-sm', // 色
	'btn_img_rowdelete' => '',            				// 画像
	'btn_char_rowdelete' => '削除',                     // 文字
	// 新規
	'btn_color_new' => 'btn btn-outline-primary',       // 色
	'btn_img_new' => '',                   				// 画像
	'btn_char_new' => '新規',                         	// 文字
	// 検索
	'btn_color_search' => 'btn btn-outline-primary',    // 色
	'btn_img_search' => '',              				// 画像
	'btn_char_search' => '検索',                      	// 文字
	// 保存
	'btn_color_save' => 'btn btn-outline-primary',      // 色
	'btn_img_save' => '',                  				// 画像
	'btn_char_save' => '保存',                        	// 文字
	// OK
	'btn_color_ok' => 'btn btn-outline-primary',        // 色
	'btn_img_ok' => '',                   				// 画像
	'btn_char_ok' => 'OK',                            	// 文字
	// キャンセル
	'btn_color_cancel' => 'btn btn-outline-primary',    // 色
	'btn_img_cancel' => '',               				// 画像
	'btn_char_cancel' => 'キャンセル',                 	// 文字
	// 戻る
	'btn_color_back' => 'btn btn-outline-primary',      // 色
	'btn_img_back' => '',            					// 画像
	'btn_char_back' => '戻る',                          // 文字
	// ファイルを選択
	'btn_color_file' => 'btn btn-outline-primary',      // 色
	'btn_img_file' => '',            					// 画像
	'btn_char_file' => '選択',                          // 文字
	// クリア
	'btn_color_clear' => 'btn btn-outline-primary',     // 色
	'btn_img_clear' => '',            					// 画像
	'btn_char_clear' => 'クリア',                       // 文字
	// 履歴
	'btn_color_history' => 'btn btn-outline-primary',   // 色
	'btn_img_history' => '',            				// 画像
	'btn_char_history' => '履歴',                       // 文字
	// テーブル行内の履歴
	'btn_color_rowhistory' => 'btn btn-outline-primary btn-sm',   // 色
	'btn_img_rowhistory' => '',            				// 画像
	'btn_char_rowhistory' => '履歴',                       // 文字
	// 実績反映
	'btn_color_readac' => 'btn btn-outline-primary',    // 色
	'btn_img_readac' => '',            				    // 画像
	'btn_char_readac' => '実績反映',                    // 文字
	// テーブル行内の詳細
	'btn_color_rowdetail' => 'btn btn-outline-primary btn-sm', // 色
	'btn_img_rowdetail' => '',            			    // 画像
	'btn_char_rowdetail' => '詳細',                     // 文字
	// 出力
	'btn_color_output' => 'btn btn-outline-primary',    // 色
	'btn_img_output' => '',            				    // 画像
	'btn_char_output' => '出力',                    // 文字
	// 次へ
	'btn_color_next' => 'btn btn-outline-primary',    // 色
	'btn_img_next' => '',            				    // 画像
	'btn_char_next' => '次へ',                    // 文字
	// トップページへ戻る
	'btn_color_toppage' => 'btn btn-outline-primary',    // 色
	'btn_img_toppage' => '',            				    // 画像
	'btn_char_toppage' => 'トップページへ戻る',                    // 文字

	//TimeTrackerNXWebApi関連
	'timetracker_error_msg' => '[TimeTrackerNXwebApi]',	//TimeTrackerNXWebApiのエラーメッセージの文末につける

	//表示件数の選択肢
	'displayed_results_1' => 10,
	'displayed_results_2' => 20,
	'displayed_results_3' => 30,

	// 組区 区切り記号「：」は code_name_separator を使用する
	// (3：枠中は、現在では使用されていない為、欠番。)
	'kumiku_code_kogumi' => '1',     // 1：小組
	'kumiku_code_naicyu' => '2',     // 2：内中
	'kumiku_code_kumicyu' => '4',    // 4：組中
	'kumiku_code_ogumi' => '5',      // 5：大組
	'kumiku_code_sogumi' => '6',     // 6：総組
	'kumiku_code_kyocyu' => '7',     // 7：渠中
	'kumiku_name_kogumi' => '小組',  // 1：小組
	'kumiku_name_naicyu' => '内中',  // 2：内中
	'kumiku_name_kumicyu' => '組中', // 4：組中
	'kumiku_name_ogumi' => '大組',   // 5：大組
	'kumiku_name_sogumi' => '総組',  // 6：総組
	'kumiku_name_kyocyu' => '渠中',  // 7：渠中

	// 中日程区分
	'c_kind_chijyo' => 0,				// 地上中日程
	'c_kind_gaigyo' => 1,				// 外業中日程
	'c_kind_giso' => 2,					// 艤装中日程
	'c_name_chijyo' => '地上中日程', 	 // 地上中日程
	'c_name_gaigyo' => '外業中日程',	 // 外業中日程
	'c_name_giso' => '艤装中日程',		 // 艤装中日程

	//項目種類
	'project_listkind_tosai' => 0, //搭載日程
	'project_listkind_schem_chijyo' => 0, //中日程(地上)
	'project_listkind_schem_gaigyo' => 1, //中日程(外業)
	'project_listkind_schem_giso' => 2, //中日程(艤装)

	//プロジェクトID
	'projectid_production' => 0, //本番用

	// SQL
	'seq_start_with' => 1, // シーケンスの開始値
	'seq_option' => 'no cache', // シーケンスのオプション
	'sql_max_in' => 1000, // SQLのIN句に入れるデータの上限

	//タイムアウト
	'timeout_time' => 360, //タイムアウト時間(分)

	//取込タイムアウト
	'import_timeout_minute' => 60 //取込タイムアウト時間

];
