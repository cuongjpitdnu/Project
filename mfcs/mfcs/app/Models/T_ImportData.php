<?php
/*
 * @T_ImportData.php
 * file Model T_ImportData 
 *
 * @create 2020/09/24 Cuong
 *
 * @update 
 */
namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Kyslik\ColumnSortable\Sortable;
/**
 * class Model T_ImportData 
 *
 * @create 2020/09/24 Cuong
 * @update
 */
class T_ImportData extends Model
{
	use Sortable;
	public $sortableAs = ['fld1', 'fld2', 'fld3', 'fld4']; // ソート対象列別名一覧
	protected $table = 'T_ImportData'; // テーブル名
	const CREATED_AT = null; // 作成日フィールドがないことを指定

	/**
	 * Kind column sort method
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/10/29　Cuong
	 * @update
	 */
	public function fld1Sortable($query, $direction)
	{
		return $query->orderBy('Kind', $direction);
	}

	/**
	 * BlockName column sort method
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/10/29　Cuong
	 * @update
	 */
	public function fld2Sortable($query, $direction)
	{
		return $query->orderBy('BlockName', $direction);
	}

	/**
	 * BlockKumiku column sort method
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/10/29　Cuong
	 * @update
	 */
	public function fld3Sortable($query, $direction)
	{
		return $query->orderBy('BlockKumiku', $direction);
	}

	/**
	 * Message column sort method
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/10/29　Cuong
	 * @update
	 */
	public function fld4Sortable($query, $direction)
	{
		return $query->orderBy('Message', $direction);
	}
}
