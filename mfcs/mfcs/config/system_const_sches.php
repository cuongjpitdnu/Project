<?php
/*
 * @system_const_sches.php
 * 小日程定数定義ファイル
 *
 *  編集する可能性があまりないものを定義
 *
 * @create 2020/11/11 KBS R.Nakata
 *
 * @update
 */

return [
	//tryLock用メニューID
	'syslock_menuid_sches' => -5001,

	//小日程表テンプレートパス
	'export_template_path' => '../../../../public/excel/小日程表_export.xlsx',

	//消込管理
	'keshicode_code_hr' => 1,
	'keshicode_name_hr' => '工数',
	'keshicode_code_bdata' => 2,
	'keshicode_name_bdata' => '物量',

	//消込方法
	'keshipattern_code_keshikomi' => 0,
	'keshipattern_name_keshikomi' => '消込値',
	'keshipattern_code_shintyoku' => 1,
	'keshipattern_name_shintyoku' => '進捗率',

	//基準データ
	'basedata_code_cyn' => 0,
	'basedata_code_kukaku' => 1,
	'basedata_code_syoku' => 2,
	'basedata_code_kogumi' => 3,
	'basedata_code_nc' => 4,
	'basedata_code_kako_mage' => 5,
	'basedata_code_kako_setsudan' => 6,
	'basedata_name_cyn' => '中日程',
	'basedata_name_kukaku' => '区画',
	'basedata_name_syoku' => '職',
	'basedata_name_kogumi' => '小組',
	'basedata_name_nc' => 'NC',
	'basedata_name_kako_mage' => '加工(曲げ)',
	'basedata_name_kako_setsudan' => '加工(切断)',

	//基点種類
	'af_code_a' => 'A',
	'af_code_f' => 'F',
	'af_code_b' => 'B',
	'af_name_a' => '着工日基点',
	'af_name_f' => '完工日基点',
	'af_name_b' => '着工、完工日基点',

	//TimeTrackerNX
	'case_project_name' => '小日程(%s)', //新規ケースプロジェクト名

	//シート名
	'sches_mastersheet_name' => 'Master',
	'sches_nitteisheet_name' => 'Nittei',

	// 日程IO
	'nt_val_import' => 0, // Import
	'nt_val_export' => 1, // Export

	// 日程Excel書式
	'nt_xl_header_row' => 1, 				// 開始行番号ヘッダ
	'nt_xl_begin_row' => 2, 				// 開始行番号
	'nt_xl_begin_col' => 'A', 				// 開始列番号
	'nt_xl_initial_rows' => 2,				// 初期行数
	'nt_xl_nittei_initial_rows' => 3,		// 初期行数

	//DeleteFlag
	'deleteflag' => 'D',

];
