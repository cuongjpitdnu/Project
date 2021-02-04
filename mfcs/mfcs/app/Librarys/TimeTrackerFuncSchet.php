<?php
/*
 * @TimeTrackerFuncSchet.php
 * TimeTrackerNX処理用(搭載日程関係)ファイル
 *
 * @create 2020/07/27 KBS T.Nishida
 *
 * @update
 */

namespace App\Librarys;

use DB;
use App\Models\WorkItemIDList;
use DateTime;

/**
 * TimeTrackerNX処理用クラス
 *
 * @create 2020/07/27　T.Nishida
 * @update
 */

class TimeTrackerFuncSchet
{

	/**
	 * プロジェクトを作成
	 *
	 * @param  string プロジェクト名
	 * @param  string プロジェクトコード
	 * @return 正常時：null(string) エラー時:エラーメッセージ(string) + timetracker_error_msg(config\system_const.php)
	 *
	 * @create 2020/10/06　S.Tanaka
	 * @update
	 */
	function addCase($projectName, $projectCode)
	{
		$timeTrackerCommon = new TimeTrackerCommon;

		$userID = config('system_config.webapi_username');
		$password = config('system_config.webapi_password');

		//トークン取得
		$token = $timeTrackerCommon->getWebApiToken($userID, $password);

		//トークン取得でエラーの場合
		if(isset($token['error'])){
			return $token['message'].config('system_const.timetracker_error_msg');
		}

		//変数初期化
		$userData = array();

		//WebApiを実行
		$userData = $timeTrackerCommon->runWebApi($token, 'GET', '/system/users');

		//WebApiを実行でエラーの場合
		if(isset($userData['error'])){
			return $userData['message'].config('system_const.timetracker_error_msg');
		}

		//変数初期化
		$managerId = null;

		//ユーザIDを設定
		$userID = config('system_config.webapi_username_tosai');

		//取得したユーザ一覧にuserIDがあるかどうか
		for($i=0; $i<count($userData['data']); $i++){
			if($userData['data'][$i]['loginName'] == $userID){
				$managerId = $userData['data'][$i]['id'];
				break;
			}
		}

		//ユーザ一覧にuserIDがなかった場合
		if(is_null($managerId)){
			return config('message.msg_timetracker_001').config('system_const.timetracker_error_msg');
		}

		//startDateとfinishDate作成
		$thisYM = date('Y-m');
		$startYM = date('Y-m-d', strtotime($thisYM . ' -' . config('system_const_timetracker.project_start_month') . ' month'));
		$finishYM = date('Y-m-d', strtotime($thisYM . ' +' . config('system_const_timetracker.project_finish_month') . ' month'));
		$startDate = date('Y-m-d', strtotime('first day of ' . $startYM));
		$finishDate = date('Y-m-d', strtotime('last day of ' . $finishYM));

		//JSON作成
		$json = '';
		$json .= '{"' . config('system_const_timetracker.project_name') . '":"' . $projectName . '"';
		$json .= ',"' . config('system_const_timetracker.project_code') . '":"' . $projectCode . '"';
		$json .= ',"' . config('system_const_timetracker.project_managerid') . '":"' . $managerId.'00' . '"';
		$json .= ',"' . config('system_const_timetracker.project_sdate') . '":"' . $startDate . '"';
		$json .= ',"' . config('system_const_timetracker.project_edate') . '":"' . $finishDate . '"}';

		//TimeTrackerNXのプロジェクトに追加
		$result = $timeTrackerCommon->runWebApi($token, 'POST', '/project/projects', '', $json);

		//エラーチェック
		if(isset($result['error'])){
			return $result['message'].config('system_const.timetracker_error_msg');
		}else{
			return null;
		}
	}

	/**
	 * オーダを作成
	 *
	 * @param  integer プロジェクトID
	 * @param  string オーダ
	 * @return 正常時：オーダのワークアイテムID(integer) エラー時:エラーメッセージ(string) + timetracker_error_msg(config\system_const.php)
	 *
	 * @create 2020/10/07　S.Tanaka
	 * @update
	 */
	function createOrder($projectID, $order)
	{
		$timeTrackerCommon = new TimeTrackerCommon;

		$userID = config('system_config.webapi_username');
		$password = config('system_config.webapi_password');

		//トークン取得
		$token = $timeTrackerCommon->getWebApiToken($userID, $password);

		//トークン取得でエラーの場合
		if(isset($token['error'])){
			return $token['message'].config('system_const.timetracker_error_msg');
		}

		//プロジェクトIDを取得
		$result = $timeTrackerCommon->getProjectID($projectID, false, $token);

		if(isset($result['error'])){
			return config('message.msg_cmn_db_027').config('system_const.timetracker_error_msg');
		}

		//workItemRootFolderId取得
		$workItemRootFolderId = $result['RootFolderID'];

		//シーケンスのnextval取得
		$nextval = DB::select('select next value for seq_WorkItemIDList');
		$nextval = $nextval[0]->{""};

		//JSON作成
		$json = '';
		$json .= '{"fields":';
		$json .= '{"'.config('system_const_timetracker.workitem_itemtypeid').'":"'.config('system_const_timetracker.itemtypeid_order').'"';
		$json .= ',"'.config('system_const_timetracker.workitem_name').'":"'.$order.'"';
		$json .= ',"'.config('system_const_timetracker.workitem_managedid').'":"'.valueUrlEncode($nextval).'"}}';

		//API名が長いので変数にした
		$api = '/workitem/workitems/'.$workItemRootFolderId.'/subitems';

		//ワークアイテムの追加
		$result = $timeTrackerCommon->runWebApi($token, 'POST', $api, '', $json);

		if(isset($result['error'])){
			return $result['message'].config('system_const.timetracker_error_msg');
		}

		$id = $result["items"][0]["id"];

		$result = $timeTrackerCommon->registWorkItemID($nextval, $id);

		if(!$result){
			$timeTrackerCommon->runWebApi($token, 'DELETE', '/workitem/workitems/'.$id);
			return config('message.msg_cmn_db_016').config('system_const.timetracker_error_msg');
		}

		return $id;
	}

	/**
	 * 検討ケース（オーダ削除）
	 *
	 * @param  integer プロジェクトID
	 * @param  string オーダ
	 * @return 正常時：null エラー時:エラーメッセージ(string) + timetracker_error_msg(config\system_const.php)
	 *
	 * @create 2020/10/12　S.Tanaka
	 * @update
	 */
	function deleteOrder($projectID, $order)
	{
		$timeTrackerCommon = new TimeTrackerCommon;

		$userID = config('system_config.webapi_username');
		$password = config('system_config.webapi_password');

		//トークン取得
		$token = $timeTrackerCommon->getWebApiToken($userID, $password);

		//トークン取得でエラーの場合
		if(isset($token['error'])){
			return $token['message'].config('system_const.timetracker_error_msg');
		}

		//プロジェクトIDを取得
		$result = $timeTrackerCommon->getProjectID($projectID, false, $token);

		if(isset($result['error'])){
			return config('message.msg_cmn_db_027').config('system_const.timetracker_error_msg');
		}

		//workItemRootFolderId取得
		$workItemRootFolderId = $result['RootFolderID'];

		//API名が長いので変数にした
		$api = '/workitem/workitems/'.$workItemRootFolderId.'/subitems';

		$result = $timeTrackerCommon->runWebApi($token, 'GET', $api,'?fields=Id,Name,'
												.config('system_const_timetracker.workitem_managedid').'&depth=1');

		if(isset($result['error'])){
			return $result['message'].config('system_const.timetracker_error_msg');
		}

		//変数初期化
		$subItem = null;

		foreach($result[0]["fields"]["SubItems"] as $value){
			if($value["fields"]["Name"] == $order){
				$subItem = $value;
				break;
			}
		}

		//一致するデータがなかった場合
		if(is_null($subItem)){
			return null;
		}

		//長いので変数にした
		$workItemListID = valueUrlDecode($subItem["fields"][config('system_const_timetracker.workitem_managedid')]);

		$result = WorkItemIDList::where('ID', '=', $workItemListID)
								->get();

		//取得件数が0件だった場合
		if(count($result) == 0){
			return config('message.msg_cmn_db_027').config('system_const.timetracker_error_msg');
		}

		//一致しない場合
		if($subItem["fields"]["Id"] != $result[0]['WorkItemID']){
			return null;
		}

		$result = $timeTrackerCommon->deleteItem($subItem["fields"]["Id"], $token);

		return $result;
	}

	/**
	 * 検討ケース（手番シフト）
	 *
	 * @param  integer ワークアイテムID
	 * @param  integer シフト数
	 * @param  array カレンダー
	 * @return 正常時：[sDate]シフト後の開始日　[eDate]シフト後の終了日(array(date)) エラー時:エラーメッセージ(string) + timetracker_error_msg(config\system_const.php)
	 *
	 * @create 2020/10/13　S.Tanaka
	 * @update
	 */
	function getShiftDate($workItemId, $shiftNumber, $calendar)
	{
		$timeTrackerCommon = new TimeTrackerCommon;

		$userID = config('system_config.webapi_username');
		$password = config('system_config.webapi_password');

		//トークン取得
		$token = $timeTrackerCommon->getWebApiToken($userID, $password);

		//トークン取得でエラーの場合
		if(isset($token['error'])){
			return $token['message'].config('system_const.timetracker_error_msg');
		}

		//ワークアイテム取得
		$result = $timeTrackerCommon->runWebApi($token, 'GET', '/workitem/workitems/', $workItemId);

		if(isset($result['error'])){
			return $result['message'].config('system_const.timetracker_error_msg');
		}

		//削除されているワークアイテムの場合
		if($result[0]["fields"][config('system_const_timetracker.workitem_isdeleted')]){
			return config('message.msg_timetracker_005').config('system_const.timetracker_error_msg');
		}

		//開始日、終了日取得
		$plannedStartDate = $result[0]["fields"][config('system_const_timetracker.workitem_sdate')];
		$plannedFinishDate = $result[0]["fields"][config('system_const_timetracker.workitem_edate')];

		//開始日、終了日のフォーマット直し
		$objDateTime = new DateTime($plannedStartDate);
		$plannedStartDate = $objDateTime->format('Y/m/d');
		$objDateTime = new DateTime($plannedFinishDate);
		$plannedFinishDate = $objDateTime->format('Y/m/d');

		//開始日シフト
		$sDate = $timeTrackerCommon->shiftDate($plannedStartDate, $shiftNumber, $calendar);

		if($sDate == ''){
			return config('message.msg_timetracker_002').config('system_const.timetracker_error_msg');
		}

		//終了日シフト
		$eDate = $timeTrackerCommon->shiftDate($plannedFinishDate, $shiftNumber, $calendar);

		if($eDate == ''){
			return config('message.msg_timetracker_002').config('system_const.timetracker_error_msg');
		}

		return array('sDate' => $sDate, 'eDate' => $eDate);
	}

	/**
	 * 日程表取込機能(日程取得)
	 *
	 * @param  array ワークアイテムID
	 * @return 正常時：[sdate]開始日　[edate]終了日　[errflag]-1(array(連想配列の配列)) エラー時:エラーメッセージ(string)
	 *
	 * @create 2020/10/20　S.Tanaka
	 * @update
	 */
	function getWorkItemDate($arrWorkItemId)
	{
		return array(
			'0' => array(
				'errflag' => 0,
				'sdate' => '2020/10/19',
				'edate' => '2020/09/15',
			),
			'1' => array(
				'errflag' => 0,
				'sdate' => '2020/09/20',
				'edate' => '2020/09/24',
			),
			'2' => array(
				// 'errflag' => -1,
				'errflag' => 0,
				'sdate' => '2020/09/23',
				'edate' => '2020/09/24',
			),
			'3' => array(
				'errflag' => 0,
				'sdate' => '2020/09/24',
				'edate' => '2020/09/28',
			),
			// '4' => array(
			// 	'errflag' => -1,
			// ),
			'4' => array(
				'errflag' => 0,
				'sdate' => '2020/09/25',
				'edate' => '2020/09/05',
			),
			'5' => array(
				'errflag' => 0,
				'sdate' => '2020/09/26',
				'edate' => '2020/09/06',
			),

			'6' => array(
                // 'errflag' => -1,
				'errflag' => 0,
				'sdate' => '2020/09/27',
				'edate' => '2020/09/07',
			),
			'7' => array(
				'errflag' => 0,
				'sdate' => '2020/08/28',
				'edate' => '2020/09/08',
			),
			'8' => array(
				'errflag' => 0,
				'sdate' => '2020/09/29',
				'edate' => '2020/09/09',
			),
			'9' => array(
				'errflag' => 0,
				'sdate' => '2020/09/10',
				'edate' => '2020/09/10',
			),
			'10' => array(
				'errflag' => 0,
				'sdate' => '2020/09/11',
				'edate' => '2020/09/11',
			),
			'11' => array(
				'errflag' => 0,
				'sdate' => '2020/09/12',
				'edate' => '2020/09/12',
			),
			'12' => array(
                'errflag' => -1,
				'errflag' => 0,
				'sdate' => '2020/09/13',
				'edate' => '2020/09/13',
			),
			'13' => array(
				'errflag' => 0,
				'sdate' => '2020/09/14',
				'edate' => '2020/09/14',
			),
			'14' => array(
				'errflag' => 0,
				'sdate' => '2020/09/15',
				'edate' => '2020/09/15',
			),
			'15' => array(
				'errflag' => 0,
				'sdate' => '2020/09/16',
				'edate' => '2020/09/16',
			),
			'16' => array(
				'errflag' => 0,
				'sdate' => '2020/09/17',
				'edate' => '2020/09/17',
			),
			'17' => array(
				'errflag' => 0,
				'sdate' => '2020/09/18',
				'edate' => '2020/09/18',
			),
			'18' => array(
				'errflag' => 0,
				'sdate' => '2020/09/19',
				'edate' => '2020/09/19',
			),
			'19' => array(
				'errflag' => 0,
				'sdate' => '2020/09/20',
				'edate' => '2020/09/20',
			),
			'20' => array(
				'errflag' => 0,
				'sdate' => '2020/09/21',
				'edate' => '2020/09/21',
			)
		);
		// return array(
		// 	'0' => array(
		// 		'errflag' => 0,
		// 		'sdate' => '2020/09/11',
		// 		'edate' => '2020/09/11',
		// 	),
		// 	'1' => array(
		// 		'errflag' => 0,
		// 		'sdate' => '2020/09/12',
		// 		'edate' => '2020/09/12',
		// 	),
		// 	'2' => array(
		// 		'errflag' => 0,
		// 		'sdate' => '2020/09/13',
		// 		'edate' => '2020/09/13',
		// 	),
		// 	'3' => array(
		// 		'errflag' => -1,
		// 	),
		// 	'4' => array(
		// 		'errflag' => 0,
		// 		'sdate' => '2020/09/05',
		// 		'edate' => '2020/09/05',
		// 	)
		// );
	}

	/**
	 * 日程表取込機能(工程作成)
	 *
	 * @param  integer プロジェクトID
	 * @param  string オーダNo
	 * @param  array(連想配列) 作成する工程の親ID(parentflag=trueの場合は作成する親の親ID)
	 * 						  作成する工程のブロック名
	 * 						  作成する工程の組区
	 * 						  作成する工程の表示名
	 * 						  作成する工程の開始日
	 * 						  作成する工程の終了日
	 * 						  親を作成するかのフラグ（trueなら親を作成）
	 * @return 正常時：(作成した親ワークアイテムID(parentflag=trueの時のみ))　作成した工程のワークアイテムID(連想配列) エラー時:エラーメッセージ(string)
	 *
	 * @create 2020/10/20　S.Tanaka
	 * @update 2020/10/23  S.Tanaka　開始日と終了日が休日によりズレて登録されていた場合、無効にしてエラーを返すように修正(ペンディング)
	 */
	function insertKotei($projectID, $orderNo, $arrKoteiItem)
	{
		// $timeTrackerCommon = new TimeTrackerCommon;

		// $userID = config('system_config.webapi_username');
		// $password = config('system_config.webapi_password');

		// //トークン取得
		// $token = $timeTrackerCommon->getWebApiToken($userID, $password);

		// //トークン取得でエラーの場合
		// if(isset($token['error'])){
		// 	return $token['message'].config('system_const.timetracker_error_msg');
		// }

		// //親職制作成処理
		// if($arrKoteiItem['parentflag']){
		// 	$nextval = DB::select('select next value for seq_WorkItemIDList');
		// 	$nextval = $nextval[0]->{""};

		// 	//JSON作成
		// 	$json = '';
		// 	$json .= '{"fields":';
		// 	$json .= '{"'.config('system_const_timetracker.workitem_itemtypeid').'":"';
		// 	$json .= config('system_const_timetracker.itemtypeid_tosai_parent').'"';
		// 	$json .= ',"'.config('system_const_timetracker.workitem_name').'":"';
		// 	$json .= $arrKoteiItem['blockname'].'"';
		// 	$json .= ',"'.config('system_const_timetracker.workitem_managedid').'":"';
		// 	$json .= valueUrlEncode($nextval).'"';
		// 	$json .= ',"'.config('system_const_timetracker.workitem_parentid').'":"';
		// 	$json .= valueUrlEncode($arrKoteiItem['parentid']).'"';
		// 	$json .= ',"'.config('system_const_timetracker.workitem_kumiku').'":"';
		// 	$json .= $arrKoteiItem['kumiku'].'"}}';

		// 	//API名が長かったので変数にした
		// 	$api = '/workitem/workitems/'.$arrKoteiItem['parentid'].'/subitems';

		// 	$result = $timeTrackerCommon->runWebApi($token, 'POST', $api, '', $json);

		// 	if(isset($result['error'])){
		// 		return $result['message'].config('system_const.timetracker_error_msg');
		// 	}

		// 	$workItemId = $result["items"][0]["id"];

		// 	$result = $timeTrackerCommon->registWorkItemID($nextval, $workItemId);

		// 	if(!$result){
		// 		$result = $timeTrackerCommon->runWebApi($token, 'DELETE', '/workitem/workitems/'.$workItemId);

		// 		if(isset($result['error'])){
		// 			return $result['message'].config('system_const.timetracker_error_msg');
		// 		}

		// 		return config('message.msg_cmn_db_016').config('system_const.timetracker_error_msg');
		// 	}
		// 	$parentWorkItemId = $workItemId;
		// }

		// $nextval = DB::select('select next value for seq_WorkItemIDList');
		// $nextval = $nextval[0]->{""};

		// //JSON作成
		// $json = '';
		// $json .= '{"fields":';
		// $json .= '{"'.config('system_const_timetracker.workitem_itemtypeid').'":"';
		// $json .= config('system_const_timetracker.itemtypeid_tosai_child').'"';
		// $json .= ',"'.config('system_const_timetracker.workitem_name').'":"';
		// $json .= $arrKoteiItem['name'].'"';
		// $json .= ',"'.config('system_const_timetracker.workitem_sdate').'":"';
		// $json .= $arrKoteiItem['sdate'].'"';
		// $json .= ',"'.config('system_const_timetracker.workitem_edate').'":"';
		// $json .= $arrKoteiItem['edate'].'"';
		// $json .= ',"'.config('system_const_timetracker.workitem_managedid').'":"';
		// $json .= valueUrlEncode($nextval).'"';
		// $json .= ',"'.config('system_const_timetracker.workitem_parentid').'":"';
		// $json .= valueUrlEncode(($arrKoteiItem['parentflag'] ? $parentWorkItemId : $arrKoteiItem['parentid'])).'"';
		// $json .= ',"'.config('system_const_timetracker.workitem_blockname').'":"';
		// $json .= $arrKoteiItem['blockname'].'"';
		// $json .= ',"'.config('system_const_timetracker.workitem_kumiku').'":"';
		// $json .= $arrKoteiItem['kumiku'].'"}}';

		// //API名が長いので変数にした
		// $api = '/workitem/workitems/';
		// $api .= ($arrKoteiItem['parentflag'] ? $parentWorkItemId : $arrKoteiItem['parentid']).'/subitems';

		// $result = $timeTrackerCommon->runWebApi($token, 'POST', $api, '', $json);

		// if(isset($result['error'])){
		// 	if($arrKoteiItem['parentflag']){
		// 		$result2 = $timeTrackerCommon->runWebApi($token, 'DELETE', '/workitem/workitems/'.$parentWorkItemId);

		// 		if(isset($result2['error'])){
		// 			return $result2['message'].config('system_const.timetracker_error_msg');
		// 		}
		// 	}

		// 	return $result['message'].config('system_const.timetracker_error_msg');
		// }

		// $workItemId = $result["items"][0]["id"];

		// $result = $timeTrackerCommon->registWorkItemID($nextval, $workItemId);

		// if(!$result){
		// 	//API名が長いので変数にした
		// 	$api = '/workitem/workitems/'.($arrKoteiItem['parentflag'] ? $parentWorkItemId : $workItemId);

		// 	$result = $timeTrackerCommon->runWebApi($token, 'DELETE', $api);

		// 	if(isset($result['error'])){
		// 		return $result['message'].config('system_const.timetracker_error_msg');
		// 	}

		// 	return config('message.msg_cmn_db_016').config('system_const.timetracker_error_msg');
		// }

		// //配列の初期化
		// $array = array();

		// if($arrKoteiItem['parentflag']){
		// 	$array['parentid'] = $parentWorkItemId;
		// }

		// $array['workItemid'] = $workItemId;

		$array = array('parentid' => 0, 'workitemid'=> 1);
        return $array;
        // return 'Error TimeTracker InsertKotei Test!';
	}

	function updateKotei($projectID, $orderNo, $arrKoteiItem){
		$array = array('parentid' => 0, 'workitemid'=> 1);
        return $array;
        // return 'Error TimeTracker UpdateKotei Test!';
	}
	function deleteKotei($array) {
		return 1;
	}
}
?>
