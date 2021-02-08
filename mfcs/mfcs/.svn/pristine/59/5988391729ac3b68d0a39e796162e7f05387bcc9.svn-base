<?php
/**
 * @Cyn_Temp_LogData_Tosai.php
 *
 *
 * @create 2020/09/24 Dung
 *
 * @update
 */
namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Kyslik\ColumnSortable\Sortable;
/**
 * 生産管理システムモデルクラス
 *
 * @create 2020/09/24 Dung
 * @update
 */
class Cyn_Temp_LogData_Tosai extends Model {
	use Sortable;
	protected $table = 'Cyn_Temp_LogData_Tosai'; // テーブル名
	public $sortableAs = ['fld1', 'fld2', 'fld3', 'fld4'];
	const CREATED_AT = null;
	/**
	 * コード列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/09/24 Dung
	 * @update
	 */
	public function fld1Sortable($query, $direction)
	{
		return $query->orderBy('BlockName', $direction);
	}

	/**
	 * 名称列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/09/24 Dung
	 * @update
	 */
	public function fld2Sortable($query, $direction)
	{
		return $query->orderBy('BKumiku', $direction);
	}

	/**
	 * 略称列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/09/24 Dung
	 * @update
	 */
	public function fld3Sortable($query, $direction)
	{
		return $query->orderBy('Gen', $direction);
	}

	/**
	 * 略称列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/09/24 Dung
	 * @update
	 */
	public function fld4Sortable($query, $direction)
	{
		return $query->orderBy('Log', $direction);
	}
}
