<?php
/*
 * @T_ImportLog.php
 * 搭載日程取込ログテーブルモデルクラスファイル
 *
 * @create 2020/08/21 T.Nishida
 *
 * @update
 */

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Kyslik\ColumnSortable\Sortable;

/*
 * 搭載日程取込ログテーブルモデルクラス
 *
 * @create 2020/08/21 T.Nishida
 * @update
 */
class T_ImportLog extends Model
{
	use Sortable;
	public $sortableAs = ['fld1', 'fld2', 'fld3', 'fld4']; // ソート対象列別名一覧
	protected $table = 'T_ImportLog'; // テーブル名
	const CREATED_AT = null;
	/**
	 * カテゴリ列ソートメソッド
	 *
	 * @param 
	 * @return mix ソートクエリ
	 *
	 * @create 2020/08/27 T.Nishida
	 * @update
	 */
	public function fld1Sortable($query, $direction)
	{
		return $query->orderBy('Category', $direction);
	}

	/**
	 * ブロック名列ソートメソッド
	 *
	 * @param 
	 * @return mix ソートクエリ
	 *
	 * @create 2020/08/27 T.Nishida
	 * @update
	 */
	public function fld2Sortable($query, $direction)
	{
		return $query->orderBy('BlockName', $direction);
	}

	/**
	 * ブロック組区列ソートメソッド
	 *
	 * @param 
	 * @return mix ソートクエリ
	 *
	 * @create 2020/08/27 T.Nishida
	 * @update
	 */
	public function fld3Sortable($query, $direction)
	{
		return $query->orderBy('BlockKumiku', $direction);
	}

	/**
	 * ログ内容列ソートメソッド
	 *
	 * @param 
	 * @return mix ソートクエリ
	 *
	 * @create 2020/08/27 T.Nishida
	 * @update
	 */
	public function fld4Sortable($query, $direction)
	{
		return $query->orderBy('Log', $direction);
	}
}
