<?php
/*
 * @T_ImportHistory.php
 * 搭載日程取込履歴テーブルモデルクラスファイル
 *
 * @create 2020/08/21 T.Nishida
 *
 * @update
 */

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Kyslik\ColumnSortable\Sortable;

/*
 * 搭載日程取込履歴テーブルモデルクラス
 *
 * @create 2020/08/21 T.Nishida
 * @update
 */
class T_ImportHistory extends Model
{
	use Sortable;
	public $sortableAs = ['fld1', 'fld2', 'fld3', 'fld4', 'fld5', 'fld6', 'fld7']; // ソート対象列別名一覧
	protected $table = 'T_ImportHistory'; // テーブル名
	const CREATED_AT = null;
	/**
	 * 取込日列ソートメソッド
	 *
	 * @param 
	 * @return mix ソートクエリ
	 *
	 * @create 2020/08/25 T.Nishida
	 * @update
	 */
	public function fld2Sortable($query, $direction)
	{
		return $query->orderBy('A.Import_Date', $direction);
	}

	/**
	 * プロジェクト名列ソートメソッド
	 *
	 * @param 
	 * @return mix ソートクエリ
	 *
	 * @create 2020/08/25 T.Nishida
	 * @update
	 */
	public function fld4Sortable($query, $direction)
	{
		return $query->orderBy('B.ProjectName', $direction);
	}

	/**
	 * オーダ列ソートメソッド
	 *
	 * @param 
	 * @return mix ソートクエリ
	 *
	 * @create 2020/08/25 T.Nishida
	 * @update
	 */
	public function fld5Sortable($query, $direction)
	{
		return $query->orderBy('A.OrderNo', $direction);
	}

	/**
	 * 日程連携フラグ列ソートメソッド
	 *
	 * @param 
	 * @return mix ソートクエリ
	 *
	 * @create 2020/08/25 T.Nishida
	 * @update
	 */
	public function fld6Sortable($query, $direction)
	{
		return $query->orderBy('A.LinkFlag', $direction);
	}

	/**
	 * 状態フラグ列ソートメソッド
	 *
	 * @param 
	 * @return mix ソートクエリ
	 *
	 * @create 2020/08/25 T.Nishida
	 * @update
	 */
	public function fld7Sortable($query, $direction)
	{
		return $query->orderBy('A.StatusFlag', $direction);
	}
}
