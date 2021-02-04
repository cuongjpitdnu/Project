<?php
/*
 * @MstDist.php
 * 職種マスタモデルクラスファイル
 *
 * @create 2020/08/18 KBS K.Seto
 *
 * @update
 */

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Kyslik\ColumnSortable\Sortable;
/*
 * 職種マスタモデルクラス
 *
 * @create 2020/08/18 KBS K.Seto
 * @update
 */
class MstDist extends Model
{
	use Sortable;
	protected $table = 'mstDist'; // テーブル名
	public $sortableAs = ['fld1', 'fld2', 'fld3']; // ソート対象列別名一覧
	protected $primaryKey = 'Code'; // 主キー
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
	 * 略称列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/09/01　Chien
	 * @update
	 */
	public function fld3Sortable($query, $direction)
	{
		return $query->orderBy('Nick', $direction);
	}
}
