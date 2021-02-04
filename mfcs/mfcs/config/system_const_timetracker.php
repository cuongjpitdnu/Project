<?php
/*
 * @system_const_timetracker.php
 * TimeTracker定数定義ファイル
 *
 *  編集する可能性があまりないものを定義
 *
 * @create 2020/09/24 KBS T.Nishida
 *
 * @update
 */

return [

	// TimeTrackerに表示する項目名
	'koteiname_schet_tosai' => '搭載', // 搭載日程レベル(搭載日程)
	'koteiname_schet_sogumi' => '総組立', // 搭載日程レベル(総組日程)
	'koteiname_schet_kyokyu' => '総組供給', // 搭載日程レベル(供給日程)
	'koteiname_schem_tosai' => '搭載日', // 中日程レベル(搭載日程)
	'koteiname_schem_sogumi' => '総組立', // 中日程レベル(総組日程)
	'koteiname_schem_kyokyu' => '総組供給日', // 中日程レベル(供給日程)

	//カレンダー
	'calendar_code' => 'calendar', //カレンダープロジェクトコード

	// プロジェクト開始・終了
	'project_start_month' => 12, //プロジェクトの開始日が何か月前か
	'project_finish_month' => 12, //プロジェクトの終了日が何か月後か
	'project_start_month_order' => 6, //オーダプロジェクトの開始日がマーキンから何か月前か
	'project_finish_month_order' => 6, //オーダプロジェクトの終了日が引渡日から何か月後か

	//runWebApi
	'runwebapi_max_number' => 50, //runWebApiに渡すデータの上限数

	//アイテムタイプID
	'itemtypeid_order' => '4', //オーダ
	'itemtypeid_tosai_root' => 5, //搭載日程本体
	'itemtypeid_tosai_parent' => '4', //搭載日程親
	'itemtypeid_tosai_child' => '3', //搭載日程子

	//プロジェクトリクエストボディ
	'project_name' => 'name', //プロジェクト名
	'project_code' => 'code', //プロジェクトコード
	'project_managerid' => 'managerId', //管理者ID
	'project_sdate' => 'plannedStartDate', //プロジェクト開始日
	'project_edate' => 'plannedFinishDate', //プロジェクト終了日

	//ワークアイテムリクエストボディ
	'workitem_move' => 'moveParentTo', //移動先親ワークアイテムID

	//ワークアイテムフィールド名
	'workitem_id' => 'Id', //ID
	'workitem_itemtypeid' => 'ItemTypeId', //アイテムタイプID
	'workitem_name' => 'Name', //名称
	'workitem_sdate' => 'PlannedStartDate', //開始日
	'workitem_edate' => 'PlannedFinishDate', //終了日
	'workitem_isdeleted' => 'IsDeleted', //削除されたワークアイテムか(trueなら削除されたワークアイテム、falseなら未削除のワークアイテム)
	'workitem_def_parentid' => 'ParentId', //カスタムではない本来の親ワークアイテムID
	'workitem_statustypeid' => 'StatusTypeId', //ステータスタイプID

	//ワークアイテムカスタムフィールド名
	'workitem_managedid' => 'C_WorkItemListID', //自分自身のワークアイテム管理ID
	'workitem_parentid' => 'C_WorkItemPID', //親ワークアイテムID
	'workitem_kumiku' => 'C_BKumiku', //組区
	'workitem_blockname' => 'C_BlockName', //ブロック名
	'workitem_orderno' => 'C_OrderNo', //オーダNo

	//エラーコード
	'errorcode_idnotfound' => 'IdNotFound', //指定したIDのデータが削除されている、あるいは見つからない時のエラーコード

];
