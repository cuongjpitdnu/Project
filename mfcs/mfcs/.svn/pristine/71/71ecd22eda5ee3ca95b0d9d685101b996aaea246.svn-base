<?php
/*
* @Cyn_mstKotei_STR_P.php
*
*
* @create 2020/09/01 Chien
*
* @update
*/

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Kyslik\ColumnSortable\Sortable;

/*
* 生産管理システムモデルクラス
*
* @create 2020/09/01 Chien
* @update
*/
class Cyn_mstKotei_STR_P extends Model
{
	use Sortable;
	protected $table = 'Cyn_mstKotei_STR_P'; // テーブル名
	public $sortableAs = ['fld1', 'fld2', 'fld3', 'fld4']; // ソート対象列別名一覧
	protected $primaryKey = ['CKind', 'Code']; // 主キー
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
	 * @create 2020/09/01　Chien
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
	 * @create 2020/09/01　Chien
	 * @update
	 */
	public function fld2Sortable($query, $direction)
	{
		return $query->orderBy('Name', $direction);
	}

	/**
	 * 区分列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/09/01　Chien
	 * @update
	 */
	public function fld3Sortable($query, $direction)
	{
		return $query->orderBy('CKind', $direction);
	}

	/**
	 * 有効列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/09/01　Chien
	 * @update
	 */
	public function fld4Sortable($query, $direction)
	{
		return $query->orderBy('DelFlag', $direction);
	}
}
