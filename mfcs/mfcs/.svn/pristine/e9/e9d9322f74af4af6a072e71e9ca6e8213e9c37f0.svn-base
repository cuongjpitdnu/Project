<?php
/*
* @Cyn_mstKotei.php
*
*
* @create 2020/09/03 Dung
*
* @update
*/

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Kyslik\ColumnSortable\Sortable;

/*
* 生産管理システムモデルクラス
*
* @create 2020/09/03 Dung
* @update
*/
class Cyn_mstKotei extends Model
{
	use Sortable;
	protected $table = 'Cyn_mstKotei'; // テーブル名
	public $sortableAs = ['fld1', 'fld2', 'fld3', 'fld4' ,'fld5']; // ソート対象列別名一覧
	protected $primaryKey = 'CKind'; // 主キー
	public $incrementing = false;  // 主キーが自動増分されない
	protected $keyType = 'string';  //主キーが文字型
	public $timestamps = true; //Set timestamp false
	const CREATED_AT = null;

	/**
	 * コード列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/09/03 Dung
	 * @update
	 */
	public function fld1Sortable($query, $direction)
	{
		return $query->orderBy('Code', $direction);
	}

	/**
	 * 名称列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/09/03 Dung
	 * @update
	 */
	public function fld2Sortable($query, $direction)
	{
		return $query->orderBy('Name', $direction);
	}

	/**
	 * 略称列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/09/03 Dung
	 * @update
	 */
	public function fld3Sortable($query, $direction)
	{
		return $query->orderBy('Nick', $direction);
	}

	/**
	 * 略称列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/09/03 Dung
	 * @update
	 */
	public function fld4Sortable($query, $direction)
	{
		return $query->orderBy('CKind', $direction);
	}

	/**
	 * 表示順列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/09/03 Dung
	 * @update
	 */
	public function fld5Sortable($query, $direction)
	{
		return $query->orderBy('DelFlag', $direction);
	}
}
