<?php
/**
 * @Cyn_Temp_Block_BD.php
 *
 *
 * @create 2020/12/08 Cuong
 *
 * @update
 */
namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Kyslik\ColumnSortable\Sortable;
/**
 * Cyn_Temp_Block_BD class
 *
 * @create 2020/12/08 Cuong
 * @update
 */
class Cyn_Temp_Block_BD extends Model
{
	use Sortable;
	protected $table = 'Cyn_Temp_Block_BD'; // テーブル名
	const CREATED_AT = null;
	public $sortableAs = ['fld1', 'fld2', 'fld3', 'fld4', 'fld5', 'fld6', 'fld7']; // ソート対象列別名一覧

	/**
	 * 搭載名ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/12/08　Cuong
	 * @update
	 */
	public function fld1Sortable($query, $direction)
	{
		return $query->orderBy('Cyn_Temp_Block_BD.T_Name', $direction);
	}

	/**
	 * 搭載組区ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/12/08　Cuong
	 * @update
	 */
	public function fld2Sortable($query, $direction)
	{
		return $query->orderBy('Cyn_Temp_Block_BD.T_BKumi', $direction);
	}

	/**
	 * 中日程名ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/12/08　Cuong
	 * @update
	 */
	public function fld3Sortable($query, $direction)
	{
		return $query->orderBy('Cyn_Temp_Block_BD.Name', $direction);
	}

	/**
	 * 中日程組区ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/12/08　Cuong
	 * @update
	 */
	public function fld4Sortable($query, $direction)
	{
		return $query->orderBy('Cyn_Temp_Block_BD.BKumiku', $direction);
	}

	/**
	 * 工程ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/12/08　Cuong
	 * @update
	 */
	public function fld5Sortable($query, $direction)
	{
		return $query->orderBy('Cyn_Temp_Block_BD.Kotei', $direction);
	}

	/**
	 * 工程組区ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/12/08　Cuong
	 * @update
	 */
	public function fld6Sortable($query, $direction)
	{
		return $query->orderBy('Cyn_Temp_Block_BD.KKumiku', $direction);
	}

	/**
	 * 内容ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/12/08　Cuong
	 * @update
	 */
	public function fld7Sortable($query, $direction)
	{
		return $query->orderBy('Cyn_Temp_Block_BD.Log', $direction);
	}

}
