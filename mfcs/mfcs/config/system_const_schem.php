<?php
/*
 * @system_const_schem.php
 * 中日程定数定義ファイル
 *
 *  編集する可能性があまりないものを定義
 *
 * @create 2020/09/16 KBS K.Yoshihara
 *
 * @update
 */

return [
	// 排他ロックテーブルSysMenuID
	'sys_menu_id_plan' => -4001, // 工程関連

	//中日程表テンプレートパス
	'output_template_path' => '../../../../public/excel/中日程表.xlsx',
	'export_template_path' => '../../../../public/excel/中日程表_export.xlsx',

	//中日程表
	'output_year_001' => '※西暦は',
	'output_year_002' => '網掛けの日は',
	'output_year_003' => '無しは',
	'output_year_004' => '太枠の日は',
	'output_year_005' => '太字の日は',
	'output_year_006' => '斜体字の日は',

	//中日程表出力フラグ
	'kotei_outflag_001' => 1, //マーキンとして出力
	'kotei_outflag_002' => 2, //本体工程着工日として出力

	// 他区分中日程からの取込 AMDFlag
	'amd_mod_plan' => 0, 		// 工程更新
	'amd_add_block' => 1, 		// ブロック追加
	'amd_add_plan' => 2, 		// 工程追加
	'amd_del_block' => 3, 		// ブロック削除
	'amd_del_plan' => 4, 		// 工程削除

	//状態フラグ
	'schem_import_status_running' => 0, //実行中
	'schem_import_status_error' => -1, //エラー
	'schem_import_status_done' => 1, //完了

	// 物量IO
	'bd_val_import' => 0, // Import
	'bd_val_export' => 1, // Export

	// 物量Import/Export実行結果
	'bd_result_error' => 1, // 失敗
	'bd_result_success' => 2, // 成功

	// 物量Excelテンプレートパス
	'bd_export_template_path' => '../../../../public/excel/物量.xlsx',

	// 物量Excel書式
	'bd_xl_begin_row' => 3, 			// 開始行番号
	'bd_xl_begin_col' => 1, 			// 開始列番号
	'bd_xl_end_col' => 68, 				// 終了列番号
	'bd_xl_t_name_col' => 1, 			// 搭載ブロック名列番号
	'bd_xl_t_bkumiku_col' => 2, 		// 搭載ブロック組区列番号
	'bd_xl_name_col' => 3, 				// ブロック名列番号
	'bd_xl_bkumiku_col' => 4, 			// 搭載ブロック組区列番号
	'bd_xl_kotei_col' => 5, 			// 工程コード列番号
	'bd_xl_kkumiku_col' => 6, 			// 工程組区列番号
	'bd_xl_bd_begin_col' => 7, 			// 物量開始列位置
	'bd_xl_initial_rows' => 3,          // 初期行数

	//TimeTrackerNX
	'case_project_name' => '中日程(%s)', //新規ケースプロジェクト名
];
