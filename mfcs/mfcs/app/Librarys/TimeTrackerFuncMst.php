<?php
/*
 * @TimeTrackerFuncMst.php
 * TimeTrackerNX処理用(マスタ関係)ファイル
 *
 * @create 2020/07/27 KBS T.Nishida
 *
 * @update 
 */

namespace App\Librarys;
use DateTime;

/**
 * TimeTrackerNX処理用クラス
 *
 * @create 2020/07/27　T.Nishida
 * @update
 */

class TimeTrackerFuncMst
{
	
	/**
	 * 新規オーダをTimeTrackerNXのプロジェクトに追加
	 *
	 * @param  string プロジェクト名
	 * @param  string プロジェクトコード
	 * @param  string プロジェクト開始日
	 * @param  string プロジェクト終了日
	 * @return 正常時：null(string) エラー時:エラーメッセージ(string)
	 * 
	 * @create 2020/07/31  S.Tanaka
	 * @update 2020/10/26  S.Tanaka 開始日と終了日の設定の変更
	 */
	function mstOrder($projectName, $projectCode, $startDate = null, $finishDate = null)
	{
		$timeTrackerCommon = new TimeTrackerCommon;

		$userID = config('system_config.webapi_username');
		$password = config('system_config.webapi_password');

		//トークン取得
		$token = $timeTrackerCommon->getWebApiToken($userID, $password);

		//トークン取得でエラーの場合
		if(isset($token['error'])){
			return $token['message'];
		}

		//変数初期化
		$userData = array();

		//WebApiを実行
		$userData = $timeTrackerCommon->runWebApi($token, 'GET', '/system/users');

		//WebApiを実行でエラーの場合
		if(isset($userData['error'])){
			return $userData['message'];
		}

		//変数初期化
		$managerId = null;

		//取得したユーザ一覧にuserIDがあるかどうか
		for($i=0; $i<count($userData['data']); $i++){
			if($userData['data'][$i]['loginName'] == $userID){
				$managerId = $userData['data'][$i]['id'];
				break;
			}
		}

		//ユーザ一覧にuserIDがなかった場合
		if(is_null($managerId)){
			return sprintf(config('message.msg_timetracker_001'), 'システム').config('system_const.timetracker_error_msg');
		}

		$objSDateTime = new DateTime($startDate);
		$objEDateTime = new DateTime($finishDate);

		//開始日と終了日の比較
		$synbol = $objSDateTime->diff($objEDateTime)->format('%R');

		//開始日と終了日の両方とも入力されているかつ開始日が終了日より後の日付の場合
		if(!is_null($startDate) && !is_null($finishDate) && $synbol === '-'){
			return config('message.msg_order_err_001').config('system_const.timetracker_error_msg');
		}

		//開始日の設定
		$sDate = $objSDateTime->modify('first day of this months');
		
		if(is_null($startDate)){
			$startDate = $sDate->modify('-'. config('system_const_timetracker.project_start_month') .' months');
		}else{
			$startDate = $sDate->modify('-'. config('system_const_timetracker.project_start_month_order') .' months');
		}

		//終了日の設定
		$eDate = $objEDateTime->modify('first day of this months');

		if(is_null($finishDate)){
			$eDate = $eDate->modify('+'. config('system_const_timetracker.project_finish_month') .' months');
		}else{
			$eDate = $eDate->modify('+'. config('system_const_timetracker.project_finish_month_order') .' months');
		}
		$finishDate = $eDate->modify('last day of this months');

		//設定後の開始日と設定後の終了日の比較
		$synbol = $startDate->diff($finishDate)->format('%R');
		
		//設定後の開始日が設定後の終了日より後の日付の場合
		if($synbol === '-'){
			$objSDateTime = new DateTime();
			$objEDateTime = new DateTime();
			$sDate = $objSDateTime->modify('first day of this months');
			$eDate = $objEDateTime->modify('first day of this months');
			$startDate = $sDate->modify('-'. config('system_const_timetracker.project_start_month') .' months');
			$eDate = $eDate->modify('+'. config('system_const_timetracker.project_finish_month') .' months');
			$finishDate = $eDate->modify('last day of this months');
		}

		//フォーマット設定
		$startDate = $startDate->format('Y-m-d');
		$finishDate = $finishDate->format('Y-m-d');

		//エスケープ処理
		$projectName = addslashes($projectName);
		$projectCode = addslashes($projectCode);
		
		//JSON作成
		$json = '';
		$json .= '{"' . config('system_const_timetracker.project_name') . '":"' . $projectName . '"';
		$json .= ',"' . config('system_const_timetracker.project_code') . '":"' . $projectCode . '"';
		$json .= ',"' . config('system_const_timetracker.project_managerid') . '":"' . $managerId . '"';
		$json .= ',"' . config('system_const_timetracker.project_sdate') . '":"' . $startDate . '"';
		$json .= ',"' . config('system_const_timetracker.project_edate') . '":"' . $finishDate . '"}';

		//TimeTrackerNXのプロジェクトに追加
		$result = $timeTrackerCommon->runWebApi($token, 'POST', '/project/projects', '', $json);

		//エラーチェック
		if(isset($result['error'])){
			return $result['message'];
		}else{
			return null;
		}
	}
}
?>
