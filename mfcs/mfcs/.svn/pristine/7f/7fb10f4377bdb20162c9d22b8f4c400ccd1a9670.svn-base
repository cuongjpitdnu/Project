<?php
/*
 * @MstOrderNo.php
 * オーダマスタモデルクラスファイル
 *
 * @create 2020/07/29 KBS K.Seto
 *
 * @update 2020/08/31 Cuong set const CREATED_AT = null
 */

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Kyslik\ColumnSortable\Sortable;
/*
 * オーダマスタモデルクラス
 *
 * @create 2020/07/29 KBS K.Seto
 * @update
 */
class MstOrderNo extends Model
{
	use Sortable;
	public $sortableAs = ['fld1', 'fld2', 'fld3', 'fld4', 'fld5', 'fld6', 'fld7']; // ソート対象列別名一覧
	protected $table = 'mstOrderNo'; // テーブル名
	protected $primaryKey = 'OrderNo'; // 主キー
	public $incrementing = false;  // 主キーが自動増分されない
	protected $keyType = 'string';  //主キーが文字型
	public $timestamps = true; //
	const CREATED_AT = null;

	/**
	 * オーダ列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/07/29　K.Seto
	 * @update
	 */
	public function fld1Sortable($query, $direction)
	{
		return $query->orderBy('OrderNo', $direction);
	}

	/**
	 * 船種列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/07/29　K.Seto
	 * @update
	 */
	public function fld2Sortable($query, $direction)
	{
		return $query->orderBy('TYPE', $direction);
	}

	/**
	 * 船型列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/07/29　K.Seto
	 * @update
	 */
	public function fld3Sortable($query, $direction)
	{
		return $query->orderBy('STYLE', $direction);
	}

	/**
	 * マーキン列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/07/29　K.Seto
	 * @update
	 */
	public function fld4Sortable($query, $direction)
	{
		return $query->orderBy('TP_Date', $direction);
	}

	/**
	 * 進水列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/07/29　K.Seto
	 * @update
	 */
	public function fld5Sortable($query, $direction)
	{
		return $query->orderBy('L_Date', $direction);
	}

	/**
	 * 引渡列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/07/29　K.Seto
	 * @update
	 */
	public function fld6Sortable($query, $direction)
	{
		return $query->orderBy('D_Date', $direction);
	}

	/**
	 * 表示列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/07/29　K.Seto
	 * @update
	 */
	public function fld7Sortable($query, $direction)
	{
		return $query->orderBy('DispFlag', $direction);
	}

}
