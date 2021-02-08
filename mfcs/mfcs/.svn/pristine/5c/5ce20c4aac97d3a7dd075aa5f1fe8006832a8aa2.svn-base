<?php
/*
* @Cyn_mstKotei_STR_C.php
*
*
* @create 2020/09/08 Chien
*
* @update
*/

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Kyslik\ColumnSortable\Sortable;

/*
* 生産管理システムモデルクラス
*
* @create 2020/09/08 Chien
* @update
*/
class Cyn_mstKotei_STR_C extends Model
{
	use Sortable;
	protected $table = 'Cyn_mstKotei_STR_C'; // テーブル名
	public $sortableAs = ['fld1', 'fld2', 'fld3', 'fld4', 'fld5', 'fld6', 'fld7']; // ソート対象列別名一覧
	protected $primaryKey = ['CKind', 'Code', 'No']; // 主キー
	public $incrementing = false;  // 主キーが自動増分されない
	protected $keyType = 'string';  //主キーが文字型
	public $timestamps = true; //Set timestamp false
	const CREATED_AT = null;

	/**
	 * 組区列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/09/08　Chien
	 * @update
	 */
	public function fld1Sortable($query, $direction)
	{
		return $query->orderBy('KKumiku', $direction);
	}

	/**
	 * 工程列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/09/08　Chien
	 * @update
	 */
	public function fld2Sortable($query, $direction)
	{
		return $query->orderBy('Kotei', $direction);
	}

	/**
	 * 工期列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/09/08　Chien
	 * @update
	 */
	public function fld3Sortable($query, $direction)
	{
		return $query->orderBy('Days', $direction);
	}

	/**
	 * 施工棟列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/09/08　Chien
	 * @update
	 */
	public function fld4Sortable($query, $direction)
	{
		return $query->orderBy('Floor', $direction);
	}

	/**
	 * 管理物量列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/09/08　Chien
	 * @update
	 */
	public function fld5Sortable($query, $direction)
	{
		return $query->orderBy('BD_Code', $direction);
	}

	/**
	 * 次組区列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/09/08　Chien
	 * @update
	 */
	public function fld6Sortable($query, $direction)
	{
		return $query->orderBy('N_KKumiku', $direction);
	}

	/**
	 * 次工程列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/09/08　Chien
	 * @update
	 */
	public function fld7Sortable($query, $direction)
	{
		return $query->orderBy('N_Kotei', $direction);
	}
}
