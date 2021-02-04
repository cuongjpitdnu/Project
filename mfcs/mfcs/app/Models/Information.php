<?php
/*
 * @MstSysKind.php
 * お知らせテーブルモデルクラスファイル
 *
 * @create 2020/07/09 KBS K.Yoshihara
 *
 * @update 2020/12/10 KBS K.Yoshihara 不安定ソート対策
 */

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Kyslik\ColumnSortable\Sortable;

/*
 * お知らせテーブルモデルクラス
 *
 * @create 2020/07/09 KBS K.Yoshihara
 * @update 2020/12/10 KBS K.Yoshihara 不安定ソート対策
 */
class Information extends Model
{
	use Sortable;
	public $sortableAs = ['fld1', 'fld2']; // ソート対象列別名一覧
	protected $table = 'Information'; // テーブル名

	/**
	 * 更新日列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/07/19 K.Yoshihara
	 * @update 2020/12/10 K.Yoshihara 不安定ソート対策
	 */
	public function fld1Sortable($query, $direction)
	{
		return $query->orderBy('Updated_at', $direction)->orderBy('Message', $direction);
	}

	/**
	 * 内容列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/07/19 K.Yoshihara
	 * @update 2020/12/10 K.Yoshihara 不安定ソート対策
	 */
	public function fld2Sortable($query, $direction)
	{
		return $query->orderBy('Message', $direction)->orderBy('Updated_at', $direction);
	}

}
