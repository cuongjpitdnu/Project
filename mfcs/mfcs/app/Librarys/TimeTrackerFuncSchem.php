<?php
/*
 * @TimeTrackerFuncSchem.php
 * TimeTrackerNX処理用(中日程関係)ファイル
 *
 * @create 2020/07/27 KBS T.Nishida
 *
 * @update
 */

namespace App\Librarys;

/**
 * TimeTrackerNX処理用クラス
 *
 * @create 2020/07/27　T.Nishida
 * @update
 */

class TimeTrackerFuncSchem
{

	// サンプル。実際の実装時は仕様通りにしてください。サンプルコードはコメントにも残さず削除してください。
	static function insertKotei($projectID, $orderNo, $koteis, $calendar = null)
	{

		$returnArr = array();
		for ($i = 0; $i<count($koteis); $i++) {
			$newId['WorkItemID'] = $i.'1' ;
			$newId['WorkItemID_T'] = $i.'200' ;
			$newId['WorkItemID_K'] = $i.'300' ;
			$newId['WorkItemID_S'] = $i.'400' ;

			array_push($returnArr, $newId);
		}

		return $returnArr;
	}

	static function getOrderRoot($projectID, $orderNo, $tokenString = null) {
		return 999;
	}

	static function insertTosai($projectID, $orderNo, $arrParam = array()) {
		return array(
			'id' => (int)($arrParam['StatusTypeId'] + 1)
		);
	}

	static function insertBlock($projectID, $orderNo, $parentWorkItemID, $workItemID, $name, $bkumiku) {
		return (int)($parentWorkItemID * 10);
	}

	static function insertPlan($projectID, $orderNo, $parentWorkItemID, $workItemID = null, $sDate, $eDate, $name, $calendar = array()) {
		if(!is_null($workItemID)) {
			return (int)(($parentWorkItemID * 10) - 1);
		}
		return (int)($parentWorkItemID * 10);
	}

	static function updatePlan($projectID, $orderNo, $workItemID, $days, $name) {
		return null;
	}

	static function addCase($projectName, $param2, $cKind) {
		return null;
	}
}
?>
