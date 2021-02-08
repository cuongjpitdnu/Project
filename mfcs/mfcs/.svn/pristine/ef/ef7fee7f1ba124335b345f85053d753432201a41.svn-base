<?php
/*
 * @MstBDCode.php
 * 物量マスタモデルクラスファイル
 *
 * @create 2020/07/29 KBS K.Seto
 *
 * @update 2020/08/31 Cuong set const CREATED_AT = null
 */

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Kyslik\ColumnSortable\Sortable;

/*
 * 物量マスタモデルクラス
 *
 * @create 2020/07/29 KBS K.Seto
 * @update 2020/08/31 Cuong
 */
class MstBDCode extends Model
{
	use Sortable;
	public $sortableAs = ['fld1', 'fld2', 'fld3', 'fld4']; // ソート対象列別名一覧
	protected $table = 'mstBDCode'; // テーブル名
	protected $primaryKey = 'Code'; // 主キー
	public $incrementing = false;  // 主キーが自動増分されない
	protected $keyType = 'string';  //主キーが文字型
	public $timestamps = true; //
	const CREATED_AT = null;

	/**
	 * コード列ソートメソッド
	 *
	 * @param 
	 * @return mix ソートクエリ
	 *
	 * @create 2020/07/29　K.Seto
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
	 * @create 2020/07/29　K.Seto
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
	 * @create 2020/07/29　K.Seto
	 * @update
	 */
	public function fld3Sortable($query, $direction)
	{
		return $query->orderBy('Nick', $direction);
	}

	/**
	 * 有効列ソートメソッド
	 *
	 * @param 
	 * @return mix ソートクエリ
	 *
	 * @create 2020/07/29　K.Seto
	 * @update
	 */
	public function fld4Sortable($query, $direction)
	{
		return $query->orderBy('ViewFlag', $direction);
	}
}
