<?php
/*
 * @TimeTrackerCommon.php
 * TimeTrackerNX用WebAPI処理用ファイル
 *
 * @create 2020/06/01 KBS T.Nishida
 *
 * @update
 */

namespace App\Librarys;

use DB;
use App\Models\WorkItemIDList;
use App\Models\MstProject;
use Illuminate\Database\QueryException;
use DateTime;

/**
 * TimeTrackerNX用WebAPI処理クラス
 *
 * @create 2020/06/01　T.Nishida
 * @update
 */

class TimeTrackerCommon
{

	/**
	 * トークンを発行
	 *
	 * @param  string ユーザ
	 * @param  string パスワード
	 * @return 正常時：トークン文字列(string) エラー時:エラー内容(array)
	 *
	 * @create 2020/06/01　T.Nishida
	 * @update 2020/11/30 T.Nishida エラー時の処理を変更
	 */
	function getWebApiToken($userId, $password)
	{
		$retArr = '';
		$apiUrl = '/auth/token';

		//ログイン情報
		$json = '';
		$json .= '{"loginName":"' . $userId . '"';
		$json .= ',"password":"' . $password . '"';
		$json .= '}';

		//UTF8
		mb_http_output('utf-8');

		$optionArr = array(
			CURLOPT_URL => config('system_config.webapi_baseurl') . $apiUrl,
			CURLOPT_CUSTOMREQUEST => 'POST',
			CURLOPT_HTTPHEADER => array('Content-Type: application/json'),
			CURLOPT_POSTFIELDS => $json,
			CURLOPT_RETURNTRANSFER => true
		);
		//[CURLOPT_TIMEOUT] = 30;

		//WebApiから取得
		$ch = curl_init();
		curl_setopt_array($ch, $optionArr);

		$resJson = curl_exec($ch);
		$infoArr = curl_getinfo($ch);
		$errorNo = curl_errno($ch);

		//OK以外はエラー
		if (!$errorNo == CURLE_OK) {
			return '';
		}

		//JSONデコード
		$reeApi = json_decode($resJson, true);

		//ステータスコード
		switch ($infoArr['http_code']) {
			case 200:
				//OK
				if (!empty($reeApi['token'])) {
					$retArr = 'Authorization: Bearer ' . $reeApi['token'];
				}
				break;
			default:
				$retArr = self::getWebApiError($reeApi, $infoArr['http_code']);
				break;
		}

		return $retArr;
	}

	/**
	 * WebApiを実行
	 *
	 * @param  string トークン文字列
	 * @param  string メソッド（GET,POST,PUT,DELETE）
	 * @param  string API名
	 * @param  string クエリパラメータ
	 * @param  string リクエスト
	 * @param  boolean レスポンスをデコードして返すか（デバッグ用）
	 * @return array レスポンス
	 *
	 * @create 2020/06/01　T.Nishida
	 * @update
	 */
	function runWebApi(
		$token,
		$method,
		$api,
		$param = null,
		$request = null,
		$jsonDecode = true
	) {
		$retArr = '';
		$apiUrl = $api;

		//クエリパラメータが指定されているか
		if (!$param == null) {
			$apiUrl .= $param;
		}

		//渡すデータ
		$json = '';
		if (!$request == null) {
			$json = $request;
		}

		//UTF8
		mb_http_output('utf-8');

		$optionArr = array(
			CURLOPT_URL => config('system_config.webapi_baseurl') . $apiUrl,
			CURLOPT_CUSTOMREQUEST => $method,
			CURLOPT_HTTPHEADER => array('Content-Type: application/json', $token),
			CURLOPT_POSTFIELDS => $json,
			CURLOPT_RETURNTRANSFER => true
		);
		//$optionArr[CURLOPT_TIMEOUT] = 30;

		//WebApiから取得
		$ch = curl_init();
		curl_setopt_array($ch, $optionArr);

		$resJson = curl_exec($ch);
		$infoArr = curl_getinfo($ch);
		$errorNo = curl_errno($ch);

		//OK以外はエラー
		if ($errorNo !== CURLE_OK) {
			//詳しくエラーハンドリングしたい場合はerrorNoで確認
			//タイムアウトの場合はCURLE_OPERATION_TIMEDOUT
			return '';
		}

		//レスポンスをそのまま返すか？（デバッグ用）
		if ($jsonDecode == false) {
			return $resJson;
		}

		//JSONデコード
		$reeApi = json_decode($resJson, true);

		//ステータスコード
		switch ($infoArr['http_code']) {
			case 200:
				//OK
				$retArr = $reeApi;
				break;
			default:
				//エラー
				$retArr = self::getWebApiError($reeApi, $infoArr['http_code']);
				break;
		}

		return $retArr;
	}

	/**
	 * エラーを内容を取得
	 *
	 * @param  array WebApiから取得したデータ
	 * @param  integer ステータスコード
	 * @return array エラー内容
	 *
	 * @create 2020/06/01　T.Nishida
	 * @update 2020/10/13 T.Nishida errorcodeを返すように変更
	 * @update 2020/11/30 T.Nishida エラーメッセージの後ろに「timetracker_error_msg」を入れるように変更
	 */
	static function getWebApiError($retArrVal, $status)
	{
		$retArr = '';
		$code = '';

		if (isset($retArrVal['message'])) {

			if (isset($retArrVal['code'])) {
				$code = $retArrVal['code'];
			}

			$tmpArr = array(
				'error' => $status,
				'errorcode' => $code,
				'message' => $retArrVal['message'] . config('system_const.timetracker_error_msg')
			);
			$retArr = $tmpArr;
		} else {
			if (isset($retArrVal[0]['message'])) {

				if (isset($retArrVal[0]['code'])) {
					$code = $retArrVal[0]['code'];
				}

				$tmpArr = array(
					'error' => $status,
					'errorcode' => $code,
					'message' => $retArrVal[0]['message'] . config('system_const.timetracker_error_msg')
				);
				$retArr = $tmpArr;
			} else {
				switch ($status) {
					case 400:
						//無効なリクエスト
						$tmpArr = array(
							'error' => $status,
							'message' => '無効なリクエストです。' . config('system_const.timetracker_error_msg')
						);
						$retArr = $tmpArr;
						break;
					case 401:
						//ユーザー認証に失敗
						$tmpArr = array(
							'error' => $status,
							'message' => 'ユーザー認証に失敗しました。' . config('system_const.timetracker_error_msg')
						);
						$retArr = $tmpArr;
						break;
					case 500:
						//サーバー内で問題が発生
						$tmpArr = array(
							'error' => $status,
							'message' => 'サーバー内で問題が発生しました。' . config('system_const.timetracker_error_msg')
						);
						$retArr = $tmpArr;
						break;
					default:
						break;
				}
			}
		}

		return $retArr;
	}

	/**
	 * ワークアイテムID管理テーブル登録
	 *
	 * @param  integer ID
	 * @param  integer ワークアイテムID
	 * @return boolean 正常時：true エラー時：false
	 *
	 * @create 2020/10/08　S.Tanaka
	 * @update
	 */
	function registWorkItemID($id, $workItemId)
	{
		$exception = DB::transaction(function () use ($id, $workItemId) {
			try {
				WorkItemIDList::insert([
					'ID' => $id,
					'WorkItemID' => $workItemId,
				]);
			} catch (QueryException $e) {
				return $e;
			}
		});
		return is_null($exception) ? true : false;
	}

	/**
	 * プロジェクトID取得
	 *
	 * @param  mix(int or string) IDまたはオーダ
	 * @param  boolean 本番フラグ
	 * @param  string トークン文字列（デフォルト null)
	 * @return array 正常時：[ProjectID]プロジェクトのID、[RootFolderID]ルートフォルダID エラー時：[error]エラーメッセージ
	 *
	 * @create 2020/10/08　S.Tanaka
	 * @update 2020/11/12　S.Tanaka　エラー時の返り値を「-1」からエラーメッセージに変更
	 */
	function getProjectID($idOrOrder, $productionFlag, $token = null)
	{
		if (!$productionFlag) {
			$result = MstProject::where('ID', $idOrOrder)
				->first();

			//取得できなかった場合
			if (!$result) {
				$array = array('error' => sprintf(config('message.msg_cmn_db_030'), '[mstProject]'));
				return $array;
			}

			//プロジェクトコード作成
			$projectCode = $result->SysKindID . '-' . $result->ListKind . '-' . $result->SerialNo;
		}

		if (is_null($token)) {
			$userID = config('system_config.webapi_username');
			$password = config('system_config.webapi_password');

			//トークン取得
			$token = $this->getWebApiToken($userID, $password);

			//トークン取得でエラーの場合
			if (isset($token['error'])) {
				$array = array('error' => $token['message']);
				return $array;
			}
		}

		$result = $this->runWebApi($token, 'GET', '/project/projects');

		if (isset($result['error'])) {
			$array = array('error' => $result['message']);
			return $array;
		}

		foreach ($result['data'] as $value) {
			if ($value['code'] == ($productionFlag ? $idOrOrder : $projectCode)) {
				$array = array(
					'ProjectID' => $value['id'],
					'RootFolderID' => $value['workItemRootFolderId']
				);
				return $array;
			}
		}

		$array = array('error' => config('message.msg_timetracker_007'));
		return $array;
	}

	/**
	 * カレンダー取得
	 *
	 * @param  integer プロジェクトID
	 * @param  string オーダ
	 * @param  string トークン文字列（デフォルト null)
	 * @return 正常時：カレンダー(array（連想配列）) エラー時:エラーメッセージ(string) + timetracker_error_msg(config\system_const.php)
	 *
	 * @create 2020/10/13　S.Tanaka
	 * @update 2020/10/26　S.Tanaka　引数にプロジェクトIDを追加し、そのIDからカレンダーを取得するように変更
	 * @update 2020/11/10　K.Yoshihara　引数に$orderNoを追加
	 * @update 2020/11/12　S.Tanaka　引数の$orderNoがnullでない場合は、その$orderNoからカレンダーを取得するように変更
	 */
	function getCalendar($projectId, $orderNo = null, $token = null)
	{
		return array();
		if (is_null($token)) {
			$userID = config('system_config.webapi_username');
			$password = config('system_config.webapi_password');

			//トークン取得
			$token = $this->getWebApiToken($userID, $password);

			//トークン取得でエラーの場合
			if (isset($token['error'])) {
				return $token['message'] . config('system_const.timetracker_error_msg');
			}
		}

		if (is_null($orderNo)) {
			$id = $projectId;
			$productionFlag = false;
		} else {
			$id = $orderNo;
			$productionFlag = true;
		}

		$result = $this->getProjectID($id, $productionFlag, $token);

		if (isset($result['error'])) {
			return $result['error'] . config('system_const.timetracker_error_msg');
		}

		$id = $result['ProjectID'];

		$result = $this->runWebApi($token, 'GET', '/project/projects/' . $id . '/workCalendar');

		if (isset($result['error'])) {
			return $result['message'] . config('system_const.timetracker_error_msg');
		}

		return $result;
	}

	/**
	 * 手番シフト
	 *
	 * @param  date シフトする日付
	 * @param  integer シフト数
	 * @param  array カレンダー
	 * @return 正常時：渡されたプロジェクトIDのカレンダー(date) エラー時：空文字
	 *
	 * @create 2020/10/14　S.Tanaka
	 * @update 2020/11/24　S.Tanaka　シフトする日付が休日の場合エラーとし、空文字を返す
	 */
	function shiftDate($shiftDate, $shiftNumber, $calendar)
	{
		return date('Y/m/d', strtotime($shiftDate . $shiftNumber . ' days'));
		//シフトする日付がセットされていない場合
		if (!$shiftDate) {
			return '';
		}

		//カレンダーがセットされていない場合
		if (!$calendar) {
			return '';
		}

		//シフト数が整数でない場合
		if (!preg_match('/^0$|^-?[1-9][0-9]*$/', $shiftNumber)) {
			return '';
		}

		//シフトする日付が存在する日付か
		$format = ['Y/m/d', 'Y-m-d'];
		for ($i = 0; $i < count($format); $i++) {
			$result = \date_parse_from_format($format[$i], $shiftDate);
			if (!$result['errors']) {
				if ($result['warnings']) {
					return '';
				}
				$errorFlag = false;
				break;
			}
			$errorFlag = true;
		}

		//シフトする日付のフォーマットが正しくない場合
		if ($errorFlag) {
			return '';
		}

		//シフト数は正の数か負の数か
		$synbol = ($shiftNumber < 0 ? '-' : '+');

		//シフトする日付が稼働日か
		$workDateFlag = $this->checkWorkDate($shiftDate, $calendar);

		//シフトする日付が休日の場合
		if (!$workDateFlag) {
			return '';
		}

		//シフトする日付が稼働日になるまで加算or減算し、シフト数が０になるまで加算or減算する
		while (!$workDateFlag or $shiftNumber != 0) {
			$objDateTime = new DateTime($shiftDate);

			$shiftDate = $objDateTime->modify($synbol . '1 day')->format('Y-m-d');

			$workDateFlag = $this->checkWorkDate($shiftDate, $calendar);

			if ($workDateFlag and $shiftNumber != 0) {
				$synbol == '+' ? $shiftNumber-- : $shiftNumber++;
			}
		}

		//返り値のフォーマット直し
		$objDateTime = new DateTime($shiftDate);
		$shiftDate = $objDateTime->format('Y/m/d');

		return $shiftDate;
	}

	/**
	 * 稼働日確認
	 *
	 * @param  date 日付
	 * @param  array(連想配列) カレンダー
	 * @return 正常時：稼働日ならtrue、稼働日でないならfalse(boolean) エラー時：エラーメッセージ(string)
	 *
	 * @create 2020/10/14　S.Tanaka
	 * @update 2020/11/18　S.Tanaka　private関数からpublic関数に変更、引数の日付のエラーチェック追加。
	 * @update 2020/12/11　S.Tanaka　日付チェックのフォーマットに'Y/m/d H:i:s.v'と'Y-m-d H:i:s.v'を追加
	 */
	function checkWorkDate($date, $calendar)
	{
		//日付が存在する日付か
		$format = ['Y/m/d', 'Y-m-d', 'Y/m/d H:i:s.v', 'Y-m-d H:i:s.v'];
		for ($i = 0; $i < count($format); $i++) {
			$result = \date_parse_from_format($format[$i], $date);
			if (!$result['errors']) {
				if ($result['warnings']) {
					return sprintf(config('message.msg_timetracker_011'), '日付');
				}
				$errorFlag = false;
				break;
			}
			$errorFlag = true;
		}

		//シフトする日付のフォーマットが正しくない場合
		if ($errorFlag) {
			return sprintf(config('message.msg_timetracker_011'), '日付の形式');
		}

		//日付フォーマット直し
		$objDateTime = new DateTime($date);
		$date = $objDateTime->format('Y-m-d');

		//日付が稼働日か確認
		if (isset($calendar['specifiedWorkDate'][$date])) {
			return $calendar['specifiedWorkDate'][$date];
		}

		//日付の曜日取得
		$dayOfWeek = strtolower($objDateTime->format('l'));

		return $calendar['workDayOfWeek'][$dayOfWeek];
	}

	// サンプル。実際の実装時は仕様通りにしてください。サンプルコードはコメントにも残さず削除してください。
	static function getKoteiRange($workItemIDs, $isCalcDays = false, $calendar = null)
	{
		$ret = array();
			foreach ($workItemIDs as $item) {
			$newItem = array();
			switch ($item) {

				case 95001:
					$newItem = array('plannedStartDate' => '2020/06/05', 'plannedFinishDate' => '2020/06/05');
					break;

				case 95002:
					$newItem = array('plannedStartDate' => '2020/06/06', 'plannedFinishDate' => '2020/06/06');
					break;

				case 95003:
					$newItem = array('plannedStartDate' => '2020/06/07', 'plannedFinishDate' => '2020/06/07');
					break;

				case 95004:
					$newItem = array('plannedStartDate' => '2020/06/08', 'plannedFinishDate' => '2020/06/08');
					break;

				case 95005:
					$newItem = array('plannedStartDate' => '2020/06/09', 'plannedFinishDate' => '2020/06/09');
					break;

				case 95006:
					$newItem = array('plannedStartDate' => '2020/06/10', 'plannedFinishDate' => '2020/06/10');
					break;

				case 95007:
					$newItem = array('plannedStartDate' => '2020/06/11', 'plannedFinishDate' => '2020/06/11');
					break;

				case 95008:
					$newItem = array('plannedStartDate' => '2020/06/12', 'plannedFinishDate' => '2020/06/12');
					break;

				case 95009:
					$newItem = array('plannedStartDate' => '2020/06/13', 'plannedFinishDate' => '2020/06/13');
					break;

				case 95010:
					$newItem = array('plannedStartDate' => '2020/06/14', 'plannedFinishDate' => '2020/06/14');
					break;

				case 95011:
					$newItem = array('plannedStartDate' => '2020/06/15', 'plannedFinishDate' => '2020/06/15');
					break;

				case 95012:
					$newItem = array('plannedStartDate' => '2020/06/16', 'plannedFinishDate' => '2020/06/16');
					break;

				case 95013:
					$newItem = array('plannedStartDate' => '2020/06/17', 'plannedFinishDate' => '2020/06/17');
					break;

				case 95014:
					$newItem = array('plannedStartDate' => '2020/06/18', 'plannedFinishDate' => '2020/06/18');
					break;

				case 95015:
					$newItem = array('plannedStartDate' => '2020/06/19', 'plannedFinishDate' => '2020/06/19');
					break;

				case 95016:
					$newItem = array('plannedStartDate' => '2020/06/20', 'plannedFinishDate' => '2020/06/20');
					break;

				case 95017:
					$newItem = array('plannedStartDate' => '2020/06/21', 'plannedFinishDate' => '2020/06/21');
					break;

				case 95018:
					$newItem = array('plannedStartDate' => '2020/06/22', 'plannedFinishDate' => '2020/06/22');
					break;

				case 95019:
					$newItem = array('plannedStartDate' => '2020/06/23', 'plannedFinishDate' => '2020/06/23');
					break;

				case 95020:
					$newItem = array('plannedStartDate' => '2020/06/24', 'plannedFinishDate' => '2020/06/24');
					break;

				case 95021:
					$newItem = array('plannedStartDate' => '2020/06/25', 'plannedFinishDate' => '2020/06/25');
					break;

				case 95022:
					$newItem = array('plannedStartDate' => '2020/06/05', 'plannedFinishDate' => '2020/06/05');
					break;

				case 95023:
					$newItem = array('plannedStartDate' => '2020/06/06', 'plannedFinishDate' => '2020/06/06');
					break;

				case 95024:
					$newItem = array('plannedStartDate' => '2020/06/07', 'plannedFinishDate' => '2020/06/07');
					break;

				case 95025:
					$newItem = array('plannedStartDate' => '2020/06/08', 'plannedFinishDate' => '2020/06/08');
					break;

				case 95026:
					$newItem = array('plannedStartDate' => '2020/06/09', 'plannedFinishDate' => '2020/06/09');
					break;

				case 95027:
					$newItem = array('plannedStartDate' => '2020/06/10', 'plannedFinishDate' => '2020/06/10');
					break;

				case 95028:
					$newItem = array('plannedStartDate' => '2020/06/11', 'plannedFinishDate' => '2020/06/11');
					break;

				case 95029:
					$newItem = array('plannedStartDate' => '2020/06/12', 'plannedFinishDate' => '2020/06/12');
					break;

				case 95030:
					$newItem = array('plannedStartDate' => '2020/06/13', 'plannedFinishDate' => '2020/06/13');
					break;

				case 95031:
					$newItem = array('plannedStartDate' => '2020/06/14', 'plannedFinishDate' => '2020/06/14');
					break;

				case 95032:
					$newItem = array('plannedStartDate' => '2020/06/15', 'plannedFinishDate' => '2020/06/15');
					break;

				case 95033:
					$newItem = array('plannedStartDate' => '2020/06/16', 'plannedFinishDate' => '2020/06/16');
					break;

				case 95034:
					$newItem = array('plannedStartDate' => '2020/06/17', 'plannedFinishDate' => '2020/06/17');
					break;

				case 95035:
					$newItem = array('plannedStartDate' => '2020/06/18', 'plannedFinishDate' => '2020/06/18');
					break;

				case 95036:
					$newItem = array('plannedStartDate' => '2020/06/19', 'plannedFinishDate' => '2020/06/19');
					break;

				case 95037:
					$newItem = array('plannedStartDate' => '2020/06/20', 'plannedFinishDate' => '2020/06/20');
					break;

				case 95038:
					$newItem = array('plannedStartDate' => '2020/06/21', 'plannedFinishDate' => '2020/06/21');
					break;

				case 95039:
					$newItem = array('plannedStartDate' => '2020/06/22', 'plannedFinishDate' => '2020/06/22');
					break;

				case 95040:
					$newItem = array('plannedStartDate' => '2020/07/05', 'plannedFinishDate' => '2020/07/05');
					break;

				case 95041:
					$newItem = array('plannedStartDate' => '2020/07/06', 'plannedFinishDate' => '2020/07/06');
					break;

				case 95042:
					$newItem = array('plannedStartDate' => '2020/07/07', 'plannedFinishDate' => '2020/07/07');
					break;

				case 95043:
					$newItem = array('plannedStartDate' => '2020/07/08', 'plannedFinishDate' => '2020/07/08');
					break;

				case 95044:
					$newItem = array('plannedStartDate' => '2020/07/09', 'plannedFinishDate' => '2020/07/09');
					break;

				case 95045:
					$newItem = array('plannedStartDate' => '2020/07/10', 'plannedFinishDate' => '2020/07/10');
					break;

				case 95046:
					$newItem = array('plannedStartDate' => '2020/07/11', 'plannedFinishDate' => '2020/07/11');
					break;

				case 95047:
					$newItem = array('plannedStartDate' => '2020/07/12', 'plannedFinishDate' => '2020/07/12');
					break;

				case 95048:
					$newItem = array('plannedStartDate' => '2020/07/13', 'plannedFinishDate' => '2020/07/13');
					break;

				case 95049:
					$newItem = array('plannedStartDate' => '2020/07/14', 'plannedFinishDate' => '2020/07/14');
					break;

				case 95050:
					$newItem = array('plannedStartDate' => '2020/07/15', 'plannedFinishDate' => '2020/07/15');
					break;

				case 95051:
					$newItem = array('plannedStartDate' => '2020/07/16', 'plannedFinishDate' => '2020/07/16');
					break;

				case 95052:
					$newItem = array('plannedStartDate' => '2020/07/17', 'plannedFinishDate' => '2020/07/17');
					break;

				case 95053:
					$newItem = array('plannedStartDate' => '2020/07/18', 'plannedFinishDate' => '2020/07/18');
					break;

				case 95054:
					$newItem = array('plannedStartDate' => '2020/07/19', 'plannedFinishDate' => '2020/07/19');
					break;

				case 95055:
					$newItem = array('plannedStartDate' => '2020/07/20', 'plannedFinishDate' => '2020/07/20');
					break;

				case 95056:
					$newItem = array('plannedStartDate' => '2020/07/21', 'plannedFinishDate' => '2020/07/21');
					break;

				case 95057:
					$newItem = array('plannedStartDate' => '2020/07/22', 'plannedFinishDate' => '2020/07/22');
					break;

				case 95058:
					$newItem = array('plannedStartDate' => '2020/07/05', 'plannedFinishDate' => '2020/07/05');
					break;

				case 95059:
					$newItem = array('plannedStartDate' => '2020/07/06', 'plannedFinishDate' => '2020/07/06');
					break;

				case 95060:
					$newItem = array('plannedStartDate' => '2020/07/07', 'plannedFinishDate' => '2020/07/07');
					break;

				case 95061:
					$newItem = array('plannedStartDate' => '2020/07/08', 'plannedFinishDate' => '2020/07/08');
					break;

				case 95062:
					$newItem = array('plannedStartDate' => '2020/07/09', 'plannedFinishDate' => '2020/07/09');
					break;

				case 95063:
					$newItem = array('plannedStartDate' => '2020/07/10', 'plannedFinishDate' => '2020/07/10');
					break;

				case 95064:
					$newItem = array('plannedStartDate' => '2020/07/11', 'plannedFinishDate' => '2020/07/11');
					break;

				case 95065:
					$newItem = array('plannedStartDate' => '2020/07/12', 'plannedFinishDate' => '2020/07/12');
					break;

				case 95066:
					$newItem = array('plannedStartDate' => '2020/07/13', 'plannedFinishDate' => '2020/07/13');
					break;

				case 95067:
					$newItem = array('plannedStartDate' => '2020/07/14', 'plannedFinishDate' => '2020/07/14');
					break;

				case 95068:
					$newItem = array('plannedStartDate' => '2020/07/15', 'plannedFinishDate' => '2020/07/15');
					break;

				case 95069:
					$newItem = array('plannedStartDate' => '2020/07/16', 'plannedFinishDate' => '2020/07/16');
					break;

				case 95070:
					$newItem = array('plannedStartDate' => '2020/07/17', 'plannedFinishDate' => '2020/07/17');
					break;

				case 95071:
					$newItem = array('plannedStartDate' => '2020/07/18', 'plannedFinishDate' => '2020/07/18');
					break;

				case 95072:
					$newItem = array('plannedStartDate' => '2020/07/19', 'plannedFinishDate' => '2020/07/19');
					break;

				case 95073:
					$newItem = array('plannedStartDate' => '2020/07/20', 'plannedFinishDate' => '2020/07/20');
					break;

				case 95074:
					$newItem = array('plannedStartDate' => '2020/07/21', 'plannedFinishDate' => '2020/07/21');
					break;

				case 95075:
					$newItem = array('plannedStartDate' => '2020/07/22', 'plannedFinishDate' => '2020/07/22');
					break;

				case 95076:
					$newItem = array('plannedStartDate' => '2020/06/05', 'plannedFinishDate' => '2020/06/05');
					break;

				case 95077:
					$newItem = array('plannedStartDate' => '2020/06/06', 'plannedFinishDate' => '2020/06/06');
					break;

				case 95078:
					$newItem = array('plannedStartDate' => '2020/06/07', 'plannedFinishDate' => '2020/06/07');
					break;

				case 95079:
					$newItem = array('plannedStartDate' => '2020/06/08', 'plannedFinishDate' => '2020/06/08');
					break;

				case 95080:
					$newItem = array('plannedStartDate' => '2020/06/09', 'plannedFinishDate' => '2020/06/09');
					break;

				case 95081:
					$newItem = array('plannedStartDate' => '2020/06/10', 'plannedFinishDate' => '2020/06/10');
					break;

				case 95082:
					$newItem = array('plannedStartDate' => '2020/06/11', 'plannedFinishDate' => '2020/06/11');
					break;

				case 95083:
					$newItem = array('plannedStartDate' => '2020/06/12', 'plannedFinishDate' => '2020/06/12');
					break;

				case 95084:
					$newItem = array('plannedStartDate' => '2020/06/13', 'plannedFinishDate' => '2020/06/13');
					break;

				case 95085:
					$newItem = array('plannedStartDate' => '2020/06/14', 'plannedFinishDate' => '2020/06/14');
					break;

				case 95086:
					$newItem = array('plannedStartDate' => '2020/06/15', 'plannedFinishDate' => '2020/06/15');
					break;

				case 95087:
					$newItem = array('plannedStartDate' => '2020/06/16', 'plannedFinishDate' => '2020/06/16');
					break;

				case 95088:
					$newItem = array('plannedStartDate' => '2020/06/17', 'plannedFinishDate' => '2020/06/17');
					break;

				case 95089:
					$newItem = array('plannedStartDate' => '2020/06/18', 'plannedFinishDate' => '2020/06/18');
					break;

				case 95090:
					$newItem = array('plannedStartDate' => '2020/06/19', 'plannedFinishDate' => '2020/06/19');
					break;

				case 95091:
					$newItem = array('plannedStartDate' => '2020/06/20', 'plannedFinishDate' => '2020/06/20');
					break;

				case 95092:
					$newItem = array('plannedStartDate' => '2020/06/21', 'plannedFinishDate' => '2020/06/21');
					break;

				case 95093:
					$newItem = array('plannedStartDate' => '2020/06/22', 'plannedFinishDate' => '2020/06/22');
					break;

				case 95094:
					$newItem = array('plannedStartDate' => '2020/06/23', 'plannedFinishDate' => '2020/06/23');
					break;

				case 95095:
					$newItem = array('plannedStartDate' => '2020/06/24', 'plannedFinishDate' => '2020/06/24');
					break;

				case 95096:
					$newItem = array('plannedStartDate' => '2020/06/25', 'plannedFinishDate' => '2020/06/25');
					break;

				case 95097:
					$newItem = array('plannedStartDate' => '2020/07/05', 'plannedFinishDate' => '2020/07/05');
					break;

				case 95098:
					$newItem = array('plannedStartDate' => '2020/07/06', 'plannedFinishDate' => '2020/07/06');
					break;

				case 95099:
					$newItem = array('plannedStartDate' => '2020/07/07', 'plannedFinishDate' => '2020/07/07');
					break;

				case 95100:
					$newItem = array('plannedStartDate' => '2020/07/08', 'plannedFinishDate' => '2020/07/08');
					break;

				case 95101:
					$newItem = array('plannedStartDate' => '2020/07/09', 'plannedFinishDate' => '2020/07/09');
					break;

				case 95102:
					$newItem = array('plannedStartDate' => '2020/07/10', 'plannedFinishDate' => '2020/07/10');
					break;

				case 95103:
					$newItem = array('plannedStartDate' => '2020/07/11', 'plannedFinishDate' => '2020/07/11');
					break;

				case 95104:
					$newItem = array('plannedStartDate' => '2020/07/12', 'plannedFinishDate' => '2020/07/12');
					break;

				case 95105:
					$newItem = array('plannedStartDate' => '2020/07/13', 'plannedFinishDate' => '2020/07/13');
					break;

				case 95106:
					$newItem = array('plannedStartDate' => '2020/07/14', 'plannedFinishDate' => '2020/07/14');
					break;

				case 95107:
					$newItem = array('plannedStartDate' => '2020/07/15', 'plannedFinishDate' => '2020/07/15');
					break;

				case 95108:
					$newItem = array('plannedStartDate' => '2020/07/16', 'plannedFinishDate' => '2020/07/16');
					break;

				case 95109:
					$newItem = array('plannedStartDate' => '2020/07/17', 'plannedFinishDate' => '2020/07/17');
					break;

				case 95110:
					$newItem = array('plannedStartDate' => '2020/07/18', 'plannedFinishDate' => '2020/07/18');
					break;

				case 95111:
					$newItem = array('plannedStartDate' => '2020/07/19', 'plannedFinishDate' => '2020/07/19');
					break;

				case 95112:
					$newItem = array('plannedStartDate' => '2020/07/20', 'plannedFinishDate' => '2020/07/20');
					break;

				case 95113:
					$newItem = array('plannedStartDate' => '2020/07/21', 'plannedFinishDate' => '2020/07/21');
					break;

				case 95114:
					$newItem = array('plannedStartDate' => '2020/07/22', 'plannedFinishDate' => '2020/07/22');
					break;

				case 95201:
					$newItem = array('plannedStartDate' => '2020/05/05', 'plannedFinishDate' => '2020/05/05');
					break;

				case 95202:
					$newItem = array('plannedStartDate' => '2020/05/06', 'plannedFinishDate' => '2020/05/06');
					break;

				case 95203:
					$newItem = array('plannedStartDate' => '2020/05/07', 'plannedFinishDate' => '2020/05/07');
					break;

				case 95204:
					$newItem = array('plannedStartDate' => '2020/05/08', 'plannedFinishDate' => '2020/05/08');
					break;

				case 95205:
					$newItem = array('plannedStartDate' => '2020/05/09', 'plannedFinishDate' => '2020/05/09');
					break;

				case 95206:
					$newItem = array('plannedStartDate' => '2020/05/10', 'plannedFinishDate' => '2020/05/10');
					break;

				case 95207:
					$newItem = array('plannedStartDate' => '2020/05/11', 'plannedFinishDate' => '2020/05/11');
					break;

				case 95208:
					$newItem = array('plannedStartDate' => '2020/05/12', 'plannedFinishDate' => '2020/05/12');
					break;

				case 95209:
					$newItem = array('plannedStartDate' => '2020/05/13', 'plannedFinishDate' => '2020/05/13');
					break;

				case 95210:
					$newItem = array('plannedStartDate' => '2020/05/14', 'plannedFinishDate' => '2020/05/14');
					break;

				case 95211:
					$newItem = array('plannedStartDate' => '2020/05/15', 'plannedFinishDate' => '2020/05/15');
					break;

				case 95212:
					$newItem = array('plannedStartDate' => '2020/05/16', 'plannedFinishDate' => '2020/05/16');
					break;

				case 95213:
					$newItem = array('plannedStartDate' => '2020/05/17', 'plannedFinishDate' => '2020/05/17');
					break;

				case 95214:
					$newItem = array('plannedStartDate' => '2020/05/18', 'plannedFinishDate' => '2020/05/18');
					break;

				case 95215:
					$newItem = array('plannedStartDate' => '2020/05/19', 'plannedFinishDate' => '2020/05/19');
					break;

				case 95216:
					$newItem = array('plannedStartDate' => '2020/05/20', 'plannedFinishDate' => '2020/05/20');
					break;

				case 95217:
					$newItem = array('plannedStartDate' => '2020/05/21', 'plannedFinishDate' => '2020/05/21');
					break;

				case 95218:
					$newItem = array('plannedStartDate' => '2020/05/22', 'plannedFinishDate' => '2020/05/22');
					break;

				case 95219:
					$newItem = array('plannedStartDate' => '2020/07/05', 'plannedFinishDate' => '2020/07/05');
					break;

				case 95220:
					$newItem = array('plannedStartDate' => '2020/07/06', 'plannedFinishDate' => '2020/07/06');
					break;

				case 95221:
					$newItem = array('plannedStartDate' => '2020/07/07', 'plannedFinishDate' => '2020/07/07');
					break;

				case 95222:
					$newItem = array('plannedStartDate' => '2020/07/08', 'plannedFinishDate' => '2020/07/08');
					break;

				case 95223:
					$newItem = array('plannedStartDate' => '2020/07/09', 'plannedFinishDate' => '2020/07/09');
					break;

				case 95224:
					$newItem = array('plannedStartDate' => '2020/07/10', 'plannedFinishDate' => '2020/07/10');
					break;

				case 95225:
					$newItem = array('plannedStartDate' => '2020/07/11', 'plannedFinishDate' => '2020/07/11');
					break;

				case 95226:
					$newItem = array('plannedStartDate' => '2020/07/12', 'plannedFinishDate' => '2020/07/12');
					break;

				case 95227:
					$newItem = array('plannedStartDate' => '2020/07/13', 'plannedFinishDate' => '2020/07/13');
					break;

				case 95228:
					$newItem = array('plannedStartDate' => '2020/07/14', 'plannedFinishDate' => '2020/07/14');
					break;

				case 95229:
					$newItem = array('plannedStartDate' => '2020/07/15', 'plannedFinishDate' => '2020/07/15');
					break;

				case 95230:
					$newItem = array('plannedStartDate' => '2020/07/16', 'plannedFinishDate' => '2020/07/16');
					break;

				case 95231:
					$newItem = array('plannedStartDate' => '2020/07/17', 'plannedFinishDate' => '2020/07/17');
					break;

				case 95232:
					$newItem = array('plannedStartDate' => '2020/07/18', 'plannedFinishDate' => '2020/07/18');
					break;

				case 95233:
					$newItem = array('plannedStartDate' => '2020/07/19', 'plannedFinishDate' => '2020/07/19');
					break;

				case 95234:
					$newItem = array('plannedStartDate' => '2020/07/20', 'plannedFinishDate' => '2020/07/20');
					break;

				case 95235:
					$newItem = array('plannedStartDate' => '2020/07/21', 'plannedFinishDate' => '2020/07/21');
					break;

				case 95236:
					$newItem = array('plannedStartDate' => '2020/07/22', 'plannedFinishDate' => '2020/07/22');
					break;

				case 95237:
					$newItem = array('plannedStartDate' => '2020/07/23', 'plannedFinishDate' => '2020/07/23');
					break;

				case 95238:
					$newItem = array('plannedStartDate' => '2020/07/24', 'plannedFinishDate' => '2020/07/24');
					break;

				case 95239:
					$newItem = array('plannedStartDate' => '2020/07/25', 'plannedFinishDate' => '2020/07/25');
					break;

				case 95401:
					$newItem = array('plannedStartDate' => '2020/06/05', 'plannedFinishDate' => '2020/06/20');
					break;

				case 95402:
					$newItem = array('plannedStartDate' => '2020/06/06', 'plannedFinishDate' => '2020/06/21');
					break;

				case 95403:
					$newItem = array('plannedStartDate' => '2020/06/07', 'plannedFinishDate' => '2020/06/22');
					break;

				case 95404:
					$newItem = array('plannedStartDate' => '2020/06/08', 'plannedFinishDate' => '2020/06/23');
					break;

				case 95405:
					$newItem = array('plannedStartDate' => '2020/06/09', 'plannedFinishDate' => '2020/06/24');
					break;

				case 95406:
					$newItem = array('plannedStartDate' => '2020/06/10', 'plannedFinishDate' => '2020/06/25');
					break;

				case 95407:
					$newItem = array('plannedStartDate' => '2020/06/11', 'plannedFinishDate' => '2020/06/26');
					break;

				case 95408:
					$newItem = array('plannedStartDate' => '2020/06/12', 'plannedFinishDate' => '2020/06/27');
					break;

				case 95409:
					$newItem = array('plannedStartDate' => '2020/06/13', 'plannedFinishDate' => '2020/06/28');
					break;

				case 95410:
					$newItem = array('plannedStartDate' => '2020/06/14', 'plannedFinishDate' => '2020/06/29');
					break;

				case 95411:
					$newItem = array('plannedStartDate' => '2020/06/15', 'plannedFinishDate' => '2020/06/30');
					break;

				case 95412:
					$newItem = array('plannedStartDate' => '2020/06/16', 'plannedFinishDate' => '2020/07/01');
					break;

				case 95413:
					$newItem = array('plannedStartDate' => '2020/06/17', 'plannedFinishDate' => '2020/07/02');
					break;

				case 95414:
					$newItem = array('plannedStartDate' => '2020/06/18', 'plannedFinishDate' => '2020/07/03');
					break;

				case 95415:
					$newItem = array('plannedStartDate' => '2020/06/19', 'plannedFinishDate' => '2020/07/04');
					break;

				case 95416:
					$newItem = array('plannedStartDate' => '2020/06/20', 'plannedFinishDate' => '2020/07/05');
					break;

				case 95417:
					$newItem = array('plannedStartDate' => '2020/06/21', 'plannedFinishDate' => '2020/07/06');
					break;

				case 95418:
					$newItem = array('plannedStartDate' => '2020/06/22', 'plannedFinishDate' => '2020/07/07');
					break;

				case 95419:
					$newItem = array('plannedStartDate' => '2020/06/23', 'plannedFinishDate' => '2020/07/08');
					break;

				case 95420:
					$newItem = array('plannedStartDate' => '2020/06/24', 'plannedFinishDate' => '2020/07/09');
					break;

				case 95421:
					$newItem = array('plannedStartDate' => '2020/06/25', 'plannedFinishDate' => '2020/07/10');
					break;

				case 95422:
					$newItem = array('plannedStartDate' => '2020/06/04', 'plannedFinishDate' => '2020/06/20');
					break;

				case 95423:
					$newItem = array('plannedStartDate' => '2020/06/06', 'plannedFinishDate' => '2020/06/22');
					break;

				case 95424:
					$newItem = array('plannedStartDate' => '2020/06/07', 'plannedFinishDate' => '2020/06/23');
					break;

				case 95425:
					$newItem = array('plannedStartDate' => '2020/06/04', 'plannedFinishDate' => '2020/06/24');
					break;

				case 95426:
					$newItem = array('plannedStartDate' => '2020/06/08', 'plannedFinishDate' => '2020/06/24');
					break;

				case 95427:
					$newItem = array('plannedStartDate' => '2020/06/09', 'plannedFinishDate' => '2020/06/30');
					break;

				case 95428:
					$newItem = array('plannedStartDate' => '2020/06/10', 'plannedFinishDate' => '2020/06/26');
					break;

				case 95429:
					$newItem = array('plannedStartDate' => '2020/06/12', 'plannedFinishDate' => '2020/06/29');
					break;

				case 95430:
					$newItem = array('plannedStartDate' => '2020/06/14', 'plannedFinishDate' => '2020/06/21');
					break;

				case 95431:
					$newItem = array('plannedStartDate' => '2020/06/14', 'plannedFinishDate' => '2020/06/29');
					break;

				case 95432:
					$newItem = array('plannedStartDate' => '2020/06/14', 'plannedFinishDate' => '2020/06/29');
					break;

				case 95433:
					$newItem = array('plannedStartDate' => '2020/06/14', 'plannedFinishDate' => '2020/07/01');
					break;

				case 95434:
					$newItem = array('plannedStartDate' => '2020/06/14', 'plannedFinishDate' => '2020/07/08');
					break;

				case 95435:
					$newItem = array('plannedStartDate' => '2020/06/18', 'plannedFinishDate' => '2020/07/23');
					break;

				case 95436:
					$newItem = array('plannedStartDate' => '2020/06/19', 'plannedFinishDate' => '2020/07/03');
					break;

				case 95437:
					$newItem = array('plannedStartDate' => '2020/06/19', 'plannedFinishDate' => '2020/07/04');
					break;

				case 95438:
					$newItem = array('plannedStartDate' => '2020/06/21', 'plannedFinishDate' => '2020/07/05');
					break;

				case 95439:
					$newItem = array('plannedStartDate' => '2020/06/21', 'plannedFinishDate' => '2020/07/07');
					break;

				case 95440:
					$newItem = array('plannedStartDate' => '2020/06/05', 'plannedFinishDate' => '2020/06/20');
					break;

				case 95441:
					$newItem = array('plannedStartDate' => '2020/06/06', 'plannedFinishDate' => '2020/06/21');
					break;

				case 95442:
					$newItem = array('plannedStartDate' => '2020/06/07', 'plannedFinishDate' => '2020/06/22');
					break;

				case 95443:
					$newItem = array('plannedStartDate' => '2020/06/08', 'plannedFinishDate' => '2020/06/23');
					break;

				case 95444:
					$newItem = array('plannedStartDate' => '2020/06/09', 'plannedFinishDate' => '2020/06/24');
					break;

				case 95445:
					$newItem = array('plannedStartDate' => '2020/06/10', 'plannedFinishDate' => '2020/06/25');
					break;

				case 95446:
					$newItem = array('plannedStartDate' => '2020/06/11', 'plannedFinishDate' => '2020/06/26');
					break;

				case 95447:
					$newItem = array('plannedStartDate' => '2020/06/12', 'plannedFinishDate' => '2020/06/27');
					break;

				case 95448:
					$newItem = array('plannedStartDate' => '2020/06/13', 'plannedFinishDate' => '2020/06/28');
					break;

				case 95449:
					$newItem = array('plannedStartDate' => '2020/06/14', 'plannedFinishDate' => '2020/06/29');
					break;

				case 95450:
					$newItem = array('plannedStartDate' => '2020/06/15', 'plannedFinishDate' => '2020/06/30');
					break;

				case 95451:
					$newItem = array('plannedStartDate' => '2020/06/16', 'plannedFinishDate' => '2020/07/01');
					break;

				case 95452:
					$newItem = array('plannedStartDate' => '2020/06/17', 'plannedFinishDate' => '2020/07/02');
					break;

				case 95453:
					$newItem = array('plannedStartDate' => '2020/06/18', 'plannedFinishDate' => '2020/07/03');
					break;

				case 95454:
					$newItem = array('plannedStartDate' => '2020/06/19', 'plannedFinishDate' => '2020/07/04');
					break;

				case 95455:
					$newItem = array('plannedStartDate' => '2020/06/20', 'plannedFinishDate' => '2020/07/05');
					break;

				case 95456:
					$newItem = array('plannedStartDate' => '2020/06/21', 'plannedFinishDate' => '2020/07/06');
					break;

				case 95457:
					$newItem = array('plannedStartDate' => '2020/06/22', 'plannedFinishDate' => '2020/07/07');
					break;

				case 95458:
					$newItem = array('plannedStartDate' => '2020/06/04', 'plannedFinishDate' => '2020/06/20');
					break;

				case 95459:
					$newItem = array('plannedStartDate' => '2020/06/06', 'plannedFinishDate' => '2020/06/22');
					break;

				case 95460:
					$newItem = array('plannedStartDate' => '2020/06/07', 'plannedFinishDate' => '2020/06/23');
					break;

				case 95461:
					$newItem = array('plannedStartDate' => '2020/06/04', 'plannedFinishDate' => '2020/06/24');
					break;

				case 95462:
					$newItem = array('plannedStartDate' => '2020/06/08', 'plannedFinishDate' => '2020/06/24');
					break;

				case 95463:
					$newItem = array('plannedStartDate' => '2020/06/09', 'plannedFinishDate' => '2020/06/30');
					break;

				case 95464:
					$newItem = array('plannedStartDate' => '2020/06/10', 'plannedFinishDate' => '2020/06/26');
					break;

				case 95465:
					$newItem = array('plannedStartDate' => '2020/06/12', 'plannedFinishDate' => '2020/06/29');
					break;

				case 95466:
					$newItem = array('plannedStartDate' => '2020/06/14', 'plannedFinishDate' => '2020/06/21');
					break;

				case 95467:
					$newItem = array('plannedStartDate' => '2020/06/14', 'plannedFinishDate' => '2020/06/29');
					break;

				case 95468:
					$newItem = array('plannedStartDate' => '2020/06/14', 'plannedFinishDate' => '2020/06/29');
					break;

				case 95469:
					$newItem = array('plannedStartDate' => '2020/06/14', 'plannedFinishDate' => '2020/07/01');
					break;

				case 95470:
					$newItem = array('plannedStartDate' => '2020/06/14', 'plannedFinishDate' => '2020/07/08');
					break;

				case 95471:
					$newItem = array('plannedStartDate' => '2020/06/18', 'plannedFinishDate' => '2020/07/23');
					break;

				case 95472:
					$newItem = array('plannedStartDate' => '2020/06/19', 'plannedFinishDate' => '2020/07/03');
					break;

				case 95473:
					$newItem = array('plannedStartDate' => '2020/06/19', 'plannedFinishDate' => '2020/07/04');
					break;

				case 95474:
					$newItem = array('plannedStartDate' => '2020/06/21', 'plannedFinishDate' => '2020/07/05');
					break;

				case 95475:
					$newItem = array('plannedStartDate' => '2020/06/21', 'plannedFinishDate' => '2020/07/07');
					break;

				case 100103:
					$newItem = array('plannedStartDate' => '2017/06/22', 'plannedFinishDate' => '2020/07/10');
					break;

				default:
					$newItem = array('plannedStartDate' => '2018/11/09', 'plannedFinishDate' => '2018/11/13');
			}
			if ($isCalcDays) {
				$newItem['workDays'] = substr($item, -1);
			}
			$ret[$item] = $newItem;
		}
		return $ret;
	}

	// サンプル。実際の実装時は仕様通りにしてください。サンプルコードはコメントにも残さず削除してください。
	static function getWorkDays($projectID, $orderNo, $koteis)
	{
		$ret = array();
		foreach ($koteis as $item) {
			$sDate = new DateTime($item['sDate']);
			$eDate = new DateTime($item['eDate']);
			$diff = $sDate->diff($eDate);
			$days = $diff->days + 1;
			array_push($ret, array('sDate' => $item['sDate'], 'eDate' => $item['eDate'], 'workDays' => $days));
		}
		return $ret;
	}

	/**
	 * オーダルート取得
	 *
	 * @param  integer プロジェクトID
	 * @param  string オーダNo
	 * @param  string トークン文字列（デフォルト null)
	 * @return 正常時：オーダルートのWorkItemID(int) エラー時：エラーメッセージ(string) + timetracker_error_msg(config\system_const.php)
	 *
	 * @create 2020/11/12　S.Tanaka
	 * @update
	 */
	function getOrderRoot($projectID, $orderNo, $token = null)
	{
		return 65;
		if (is_null($token)) {
			$userID = config('system_config.webapi_username');
			$password = config('system_config.webapi_password');

			//トークン取得
			$token = $this->getWebApiToken($userID, $password);

			//トークン取得でエラーの場合
			if (isset($token['error'])) {
				return $token['message'] . config('system_const.timetracker_error_msg');
			}
		}

		$result = $this->getProjectID($projectID, false, $token);

		if (isset($result['error'])) {
			return $result['error'] . config('system_const.timetracker_error_msg');
		}

		$rootFolderId = $result["RootFolderID"];

		$parameters = '?fields=Id,Name,' . config('system_const_timetracker.workitem_orderno');
		$parameters .= ',' . config('system_const_timetracker.workitem_managedid') . '&depth=1';

		$result = $this->runWebApi($token, 'GET', '/workitem/workitems/' . $rootFolderId . '/subItems', $parameters);

		if (isset($result['error'])) {
			return $result['message'] . config('system_const.timetracker_error_msg');
		}

		//配列の初期化
		$subItems = [];

		foreach ($result[0]["fields"]["SubItems"] as $value) {
			if (valueUrlDecode($value["fields"][config('system_const_timetracker.workitem_orderno')]) == $orderNo) {
				$subItems[] = $value;
			}
		}

		if (!empty($subItems)) {
			//変数の初期化
			$subItem = null;

			foreach ($subItems as $value) {
				$managedId = valueUrlDecode($value["fields"][config('system_const_timetracker.workitem_managedid')]);

				$result = WorkItemIDList::where('ID', $managedId)
					->get();

				if (count($result) != 0) {
					$subItem = $value;
					break;
				}
			}

			if (is_null($subItem)) {
				return config('message.msg_timetracker_010') . config('system_const.timetracker_error_msg');
			}

			$subItemId = $subItem["fields"]["Id"];
			$subItemOrderNo = valueUrlDecode($value["fields"][config('system_const_timetracker.workitem_orderno')]);
			$subItemName = $subItem["fields"]["Name"];

			if ($subItemId == $result[0]->WorkItemID) {
				if ($subItemName == $subItemOrderNo) {
					return (int)$subItemId;
				} else {
					$json = '';
					$json .= '{"fields":';
					$json .= '{"' . config('system_const_timetracker.workitem_name') . '":"';
					$json .= $orderNo . '"}}';

					$result = $this->runWebApi($token, 'PUT', '/workitem/workitems/' . $subItemId, '', $json);

					if (isset($result['error'])) {
						return $result['message'] . config('system_const.timetracker_error_msg');
					}

					return (int)$subItemId;
				}
			}
		}

		//シーケンスのnextval取得
		$nextval = DB::select('select next value for seq_WorkItemIDList');
		$nextval = $nextval[0]->{""};

		$json = '';
		$json .= '{"fields":';
		$json .= '{"' . config('system_const_timetracker.workitem_itemtypeid') . '":"';
		$json .= config('system_const_timetracker.itemtypeid_order') . '"';
		$json .= ',"' . config('system_const_timetracker.workitem_name') . '":"';
		$json .= $orderNo . '"';
		$json .= ',"' . config('system_const_timetracker.workitem_managedid') . '":"';
		$json .= valueUrlEncode($nextval) . '"';
		$json .= ',"' . config('system_const_timetracker.workitem_parentid') . '":"';
		$json .= valueUrlEncode($rootFolderId) . '"';
		$json .= ',"' . config('system_const_timetracker.workitem_orderno') . '":"';
		$json .= valueUrlEncode($orderNo) . '"}}';

		$api = '/workitem/workitems/' . $rootFolderId . '/subItems';

		$result = $this->runWebApi($token, 'POST', $api, '', $json);

		if (isset($result['error'])) {
			return $result['message'] . config('system_const.timetracker_error_msg');
		}

		$id = $result["items"][0]["id"];

		$result = $this->registWorkItemID($nextval, $id);

		if (!$result) {
			$this->runWebApi($token, 'DELETE', '/workitem/workitems/' . $id);

			return config('message.msg_cmn_db_029') . config('system_const.timetracker_error_msg');
		}

		return (int)$id;
	}

	/**
	 * ワークアイテム削除
	 *
	 * @param  array ワークアイテムIDの配列
	 * @param  string トークン文字列（デフォルト null)
	 * @return 正常時：null エラー時：エラーメッセージ(string) + timetracker_error_msg(config\system_const.php)
	 *
	 * @create 2020/10/19　S.Tanaka
	 * @update
	 */
	function deleteItem($arrWorkItemId, $token = null)
	{
		return null;
		if (is_null($token)) {
			$userID = config('system_config.webapi_username');
			$password = config('system_config.webapi_password');

			//トークン取得
			$token = $this->getWebApiToken($userID, $password);

			//トークン取得でエラーの場合
			if (isset($token['error'])) {
				return $token['message'] . config('system_const.timetracker_error_msg');
			}
		}

		foreach ($arrWorkItemId as $workItemId) {
			$result = $this->runWebApi($token, 'DELETE', '/workitem/workitems/' . $workItemId);

			if (isset($result['error'])) {
				return $result['message'] . config('system_const.timetracker_error_msg');
			}
		}

		return null;
	}

	/**
	 * カレンダー確認
	 *
	 * @param  array カレンダー1
	 * @param  array カレンダー2
	 * @return 正常時：null エラー時：エラーメッセージ(string)
	 *
	 * @create 2020/10/19　S.Tanaka
	 * @update
	 */
	function checkCalendar($firstCalendar, $secondCalendar)
	{
		if ($firstCalendar === $secondCalendar) {
			return null;
		}

		return config('message.msg_timetracker_008');
	}

	static function getChildWorkItem($workItemID)
	{
		$res = array();
		for($i = 0; $i < 100; $i++) {
			$res[$i] = array(
				'itemTypeId' => $i,
				'statusTypeId'=> ($i+1),
				'name' => 'Name_'.$i,
				'plannedStartDate' => '2018/11/09',
				'plannedFinishDate' => '2018/11/13'
			);
		}
		return $res;
		/* return array(
			28549637 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_28549637', 'plannedStartDate' => '2018/11/09', 'plannedFinishDate' => '2018/11/13'),
			48267395 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_48267395', 'plannedStartDate' => '2019/06/30', 'plannedFinishDate' => '2020/02/19'),
			49326857 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_49326857', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			13 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_13', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			65 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_65', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97301 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97301', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97303 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97303', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97302 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97302', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97304 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97304', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97305 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97305', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97226 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97226', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97227 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97227', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97228 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97228', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97229 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97229', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97230 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97230', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97301 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97301', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97302 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97302', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97303 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97303', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97304 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97304', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97305 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97305', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97306 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97306', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97307 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97307', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97308 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97308', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97309 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97309', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97310 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97310', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97311 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97311', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97312 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97312', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97313 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97313', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97314 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97314', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97315 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97315', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97326 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97326', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97327 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97327', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97328 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97328', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97329 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97329', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97330 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97330', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97332 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97332', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97333 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97333', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97335 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97335', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97336 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97336', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
			97339 => array('itemTypeId' => '145', 'statusTypeId'=> '156', 'name' => 'Name_97339', 'plannedStartDate' => '2020/08/09', 'plannedFinishDate' => '2020/09/14'),
		); */
	}

	static function getShiftDate($date, $number, $array)
	{
		return date('Y/m/d', strtotime($date . $number . ' days'));
	}

	public function getDateDiff($projectID, $bDate, $date)
	{
		$before = new \DateTime(date('Y-m-d', strtotime($bDate)));
		$after = new \DateTime(date('Y-m-d', strtotime($date)));
		$interval = date_diff($before, $after);
		return (int)$interval->format('%R%a');
	}
}
