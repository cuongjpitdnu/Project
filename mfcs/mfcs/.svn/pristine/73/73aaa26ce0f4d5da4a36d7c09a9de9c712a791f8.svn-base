<?php
/*
 * @MstFloor.php
 * 棟マスタモデルクラスファイル
 *
 * @create 2020/08/18 KBS K.Seto
 *
 * @update 2020/08/31 Cuong set const CREATED_AT = null
 */

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Kyslik\ColumnSortable\Sortable;
/*
 * 棟マスタモデルクラス
 *
 * @create 2020/08/18 KBS K.Seto
 * @update 2020/08/20 AKB Cuong
 */
class MstFloor extends Model
{
	use Sortable;
	public $sortableAs = ['fld1', 'fld2', 'fld3', 'fld4', 'fld5']; // ソート対象列別名一覧
	protected $table = 'mstFloor'; // テーブル名
	protected $primaryKey = 'Code'; // 主キー
	public $incrementing = false;  // 主キーが自動増分されない
	protected $keyType = 'string';  //主キーが文字型
	public $timestamps = true; 
	const CREATED_AT = null;

	/**
	 * コード列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/08/18　K.Seto
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
	 * @create 2020/08/18　K.Seto
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
	 * @create 2020/08/18　K.Seto
	 * @update
	 */
	public function fld3Sortable($query, $direction)
	{
		return $query->orderBy('Nick', $direction);
	}

	/**
	 * 表示順列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/08/18　K.Seto
	 * @update
	 */
	public function fld4Sortable($query, $direction)
	{
		return $query->orderBy('SortNo', $direction);
	}

	/**
	 * 有効列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/08/18　K.Seto
	 * @update
	 */
	public function fld5Sortable($query, $direction)
	{
		return $query->orderBy('ViewFlag', $direction);
	}

}
