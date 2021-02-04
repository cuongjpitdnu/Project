<?php
/*
 * @MstOrgCommon.php
 * 職制マスタ用ファイル
 *
 * @create 2020/06/01 KBS T.Nishida
 *
 * @update
 */

namespace App\Librarys;

use App\Models\MstOrg;

/**
 * 職制マスタを操作するクラス
 *
 * @create 2020/06/01　T.Nishida
 * @update
 */
class MstOrgCommon
{
	/**
	 * クラス内共通変数
	 */
	private $morgArr = array(); //取得した職制データ
	private $mgroupArr = array(); //階層化した職制データ

	private $mkeyToIndexHashArr = array(); //キー検索用

	private $mbaseDate = ""; //基準日

	/**
	 * コンストラクタ
	 *
	 * @param  date 基準日(省略すると実行日)
	 * @return
	 *
	 * @create 2020/06/01　T.Nishida
	 * @update
	 */
	function __construct($baseDate = null)
	{
		//基準日
		if ($baseDate == null) {
			$tmpDate = date("Y/m/d");
		}
		else{
			$tmpDate = $baseDate;
		}

		//データ取得
		$this->setOrg($tmpDate);
	}

	/**
	 * 職制データ取得
	 *
	 * @param  date 基準日
	 * @return
	 *
	 * @create 2020/06/01　T.Nishida
	 * @update
	 */
	function setOrg($baseDate)
	{
		//クラス内共通変数初期化
		$this->morgArr = array();
		$this->mgroupArr = array();
		$this->mkeyToIndexHashArr = array();
		$this->mbaseDate = $baseDate;

		//基準日がない場合処理しない
		if (!$baseDate) {
			return;
		}

		//基準日が日付でない場合処理しない
		try {
			list($Y, $m, $d) = explode('/', $baseDate);
			if (checkdate($m, $d, $Y) == false) {
				return;
			}
		} catch (\exception $e) {
			return;
		}

		//DB読込み
		$this->dbGetMstOrg($baseDate);

		//階層化
		$this->setGroupNest();
	}

	/**
	 * mstOrgテーブルから読込み
	 *
	 * @param  date 基準日
	 * @return
	 *
	 * @create 2020/06/01　T.Nishida
	 * @update
	 */
	private function dbGetMstOrg($baseDate)
	{
		//mstOrgテーブルから取得
		$mstOrgs = MstOrg::whereDate('SDate', '<=', $baseDate)
						->whereDate('EDate', '>=', $baseDate)
						->orwhere(function($query) use($baseDate) {
								$query->whereDate('SDate', '<=', $baseDate)
									  ->whereNull('EDate');
						})
						->orderByRaw('LV_No asc, SortNo asc')
						->get();

		$hashidx = 0;

		foreach ($mstOrgs as $mstOrg) {
			$tmpOrg = array(
				'id' => $mstOrg->ID,
				'pid' => $mstOrg->PID,
				'lv_no' => $mstOrg->LV_No,
				'sdate' => $mstOrg->Sdate,
				'edate' => $mstOrg->Edate,
				'name' => $mstOrg->Name,
				'nick' => $mstOrg->Nick,
				'syokuseicode' => $mstOrg->SyokuseiCode,
				'folderflag' => $mstOrg->FolderFlag,
				'outinflag' => $mstOrg->OutInFlag,
				'buoutinflag' => $mstOrg->BuOutInFlag,
				'outpid' => $mstOrg->OutPID,
				'outtype' => $mstOrg->OutType,
				'sortno' => $mstOrg->SortNo,
				'vendercode' => $mstOrg->VenderCode,
				'updated_at' => $mstOrg->Updated_at,
				'lvgrpidx' => array(),
				'lvgrpid' => array(),
			);
			array_push($this->morgArr, $tmpOrg);
			$this->mkeyToIndexHashArr[$tmpOrg['id']] = $hashidx;
			++$hashidx;
		}
	}

	/**
	 * 職制を階層化する
	 *
	 * @param
	 * @return
	 *
	 * @create 2020/06/01　T.Nishida
	 * @update
	 */
	private function setGroupNest()
	{
		//データがなければ抜ける
		if (count($this->morgArr) == 0) {
			return;
		}

		//先にレベル毎の配列を作る
		for($i=0; $i<$this->morgArr[count($this->morgArr)-1]['lv_no']; ++$i) {
			$tmpArr = array('lv_no' => $i+1,
							'grplist' => array()
							);
			array_push($this->mgroupArr, $tmpArr);
		}

		for($i=0; $i<count($this->morgArr); ++$i) {

			$levelIdx = $this->morgArr[$i]['lv_no']-1;

			//配列へ
			$tmpArr = array('syokuseicode' => $this->morgArr[$i]['syokuseicode'],
							'id' => $this->morgArr[$i]['id'],
							'name' => $this->morgArr[$i]['name'],
							'orgidx' => $i,
							'childid' => array(),
							'childidx' => array()
							);
			array_push($this->mgroupArr[$levelIdx]['grplist'], $tmpArr);

			//レベルごとの情報をセット
			for($j=0; $j<$levelIdx; ++$j) {
				array_push($this->morgArr[$i]['lvgrpidx'], '-1');
				array_push($this->morgArr[$i]['lvgrpid'], '-1');
			}
			array_push($this->morgArr[$i]['lvgrpidx'],
						count($this->mgroupArr[$levelIdx]['grplist'])-1);
			array_push($this->morgArr[$i]['lvgrpid'],
						$this->morgArr[$i]['id']);

			//親を探す
			if ($levelIdx > 0){
				for($j=0; $j<count($this->mgroupArr[$levelIdx-1]['grplist']); ++$j) {
					if ($this->mgroupArr[$levelIdx-1]['grplist'][$j]['id'] == $this->morgArr[$i]['pid']) {

						//子の情報を入れる
						array_push($this->mgroupArr[$levelIdx-1]['grplist'][$j]['childid'],
									$this->morgArr[$i]['id']);
						array_push($this->mgroupArr[$levelIdx-1]['grplist'][$j]['childidx'],
									count($this->mgroupArr[$levelIdx]['grplist'])-1);

						//レベルごとの情報に親の情報をセット
						$pidx = $this->mgroupArr[$levelIdx-1]['grplist'][$j]['orgidx'];
						for($k=0; $k<$levelIdx; ++$k) {
							$this->morgArr[$i]['lvgrpidx'][$k] = $this->morgArr[$pidx]['lvgrpidx'][$k];
							$this->morgArr[$i]['lvgrpid'][$k] = $this->morgArr[$pidx]['lvgrpid'][$k];
						}

						break;
					}
				}
			}

		}
	}

	/**
	 * 親の職制IDを全て返す
	 *
	 * @param  integer 職制ID
	 * @return array  親の職制ID配列
	 *
	 * @create 2020/06/01　T.Nishida
	 * @update
	 */
	function getPIDAll($grpId)
	{
		$pidArr = array();

		//配列のキーがあるか調べる
		if (!array_key_exists($grpId, $this->mkeyToIndexHashArr)) {
			return;
		}

		//インデックスを取得
		$myIdx = $this->mkeyToIndexHashArr[$grpId];

		for($i=$this->morgArr[$myIdx]['lv_no']-1; $i>0; --$i) {
			array_push($pidArr, $this->morgArr[$myIdx]['lvgrpid'][$i-1]);
		}

		return $pidArr;
	}

	/**
	 * 子の職制IDを返す
	 *
	 * @param  integer 職制ID
	 * @return array  子の職制ID配列
	 *
	 * @create 2020/06/01　T.Nishida
	 * @update
	 */
	function getChildID($grpId)
	{
		$cidArr = array();

		//配列のキーがあるか調べる
		if (!array_key_exists($grpId, $this->mkeyToIndexHashArr)) {
			return;
		}

		//インデックスを取得
		$myIdx = $this->mkeyToIndexHashArr[$grpId];

		//各インデックスを取得
		$levelIdx = $this->morgArr[$myIdx]['lv_no']-1;
		$grpListIdx = $this->morgArr[$myIdx]['lvgrpidx'][$levelIdx];

		for($i=0; $i<count($this->mgroupArr[$levelIdx]['grplist'][$grpListIdx]['childid']); ++$i) {
			array_push($cidArr, $this->mgroupArr[$levelIdx]['grplist'][$grpListIdx]['childid'][$i]);
		}

		return $cidArr;
	}

	/**
	 * 最上位レベルのリストを返す
	 *
	 * @param
	 * @return array 最上位レベルのID配列
	 *
	 * @create 2020/06/01　T.Nishida
	 * @update
	 */
	function getTopLvList()
	{
		$topIdArr = array();

		//データがなければ抜ける
		if (count($this->morgArr) == 0) {
			return;
		}

		for($i=0; $i<count($this->mgroupArr[0]['grplist']); ++$i) {
			array_push($topIdArr, $this->mgroupArr[0]['grplist'][$i]['id']);
		}

		return $topIdArr;
	}

	/**
	 * 職制名を取得
	 *
	 * @param  integer 職制ID
	 * @return string 職制名
	 *
	 * @create 2020/06/01　T.Nishida
	 * @update
	 */
	function getGrpName($grpId)
	{
		$grpName = '---';

		//配列のキーがあるか調べる
		if (!array_key_exists($grpId, $this->mkeyToIndexHashArr)) {
			return $grpName;
		}

		//インデックスを取得
		$myIdx = $this->mkeyToIndexHashArr[$grpId];

		//名称を取得
		$grpName = $this->morgArr[$myIdx]['name'];

		return $grpName;
	}

	/**
	 * 職制略称を取得
	 *
	 * @param  integer 職制ID
	 * @return string 職制略称
	 *
	 * @create 2020/06/01　T.Nishida
	 * @update
	 */
	function getGrpNick($grpId)
	{
		$grpName = '---';

		//配列のキーがあるか調べる
		if (!array_key_exists($grpId, $this->mkeyToIndexHashArr)) {
			return $grpName;
		}

		//インデックスを取得
		$myIdx = $this->mkeyToIndexHashArr[$grpId];

		//略称を取得
		$grpName = $this->morgArr[$myIdx]['nick'];

		return $grpName;
	}

	/**
	 * 親の職制IDを返す
	 *
	 * @param  integer 職制ID
	 * @return integer 親のID
	 *
	 * @create 2020/06/01　T.Nishida
	 * @update
	 */
	function getPID($grpId)
	{
		$pid = -1;

		//配列のキーがあるか調べる
		if (!array_key_exists($grpId, $this->mkeyToIndexHashArr)) {
			return $pid;
		}

		//インデックスを取得
		$myIdx = $this->mkeyToIndexHashArr[$grpId];

		//親IDを取得
		$pid = $this->morgArr[$myIdx]['pid'];

		return $pid;
	}

	/**
	 * レベルを返す
	 *
	 * @param  integer 職制ID
	 * @return integer レベル
	 *
	 * @create 2020/06/01　T.Nishida
	 * @update
	 */
	function getLevel($grpId)
	{
		$level = -1;

		//配列のキーがあるか調べる
		if (!array_key_exists($grpId, $this->mkeyToIndexHashArr)) {
			return $level;
		}

		//インデックスを取得
		$myIdx = $this->mkeyToIndexHashArr[$grpId];

		//レベルを取得
		$level = $this->morgArr[$myIdx]['lv_no'];

		return $level;
	}

	/**
	 * 職制のフル名称を返す
	 *
	 * @param  integer 職制ID
	 * @return string フル名称
	 *
	 * @create 2020/06/01　T.Nishida
	 * @update
	 */
	function getFullName($grpId)
	{
		$fullName = '';

		//配列のキーがあるか調べる
		if (!array_key_exists($grpId, $this->mkeyToIndexHashArr)) {
			return $fullName;
		}

		//インデックスを取得
		$myIdx = $this->mkeyToIndexHashArr[$grpId];

		for($i=0; $i<$this->morgArr[$myIdx]['lv_no']; ++$i){
			$lvIdx = $this->morgArr[$myIdx]['lvgrpidx'][$i];
			$fullName .= $this->mgroupArr[$i]['grplist'][$lvIdx]['name'];
		}

		return $fullName;
	}

	/**
	 * 指定IDのデータを返す
	 *
	 * @param  integer 職制ID
	 * @return array データ
	 *
	 * @create 2020/06/01　T.Nishida
	 * @update
	 */
	function getDataFromID($grpId)
	{
		//配列のキーがあるか調べる
		if (!array_key_exists($grpId, $this->mkeyToIndexHashArr)) {
			return;
		}

		//インデックスを取得
		$myIdx = $this->mkeyToIndexHashArr[$grpId];

		//配列を返す
		return  $this->morgArr[$myIdx];
	}

	/**
	 * 関連会社一覧の配列を返す
	 *
	 * @param
	 * @return array 関連会社一覧
	 *
	 * @create 2020/07/22　T.Nishida
	 * @update 2020/08/11 T.Nishida 名称も取得するように変更
	 */
	function getKanrenID()
	{
		$kanrenDatas = array();

		for($i=0; $i<count($this->morgArr); ++$i) {
			if ($this->morgArr[$i]['outinflag'] == 2) {
				if ($this->morgArr[$i]['buoutinflag'] == 2) {
					if ($this->morgArr[$i]['folderflag'] != 1) {
						$kanrenData = array(
							'id' => $this->morgArr[$i]['id'],
							'name' => $this->morgArr[$i]['name'],
						);
						array_push($kanrenDatas, $kanrenData);
					}
				}
			}
		}

		//配列を返す
		return  $kanrenDatas;
	}
}
?>