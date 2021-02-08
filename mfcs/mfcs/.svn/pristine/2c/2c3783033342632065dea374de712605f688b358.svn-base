<?php
/*
 * @MstAbility.php
 * 能力時間マスタモデルクラスファイル
 *
 * @create 2020/08/18 KBS K.Seto
 *
 * @update
 */

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Kyslik\ColumnSortable\Sortable;
/*
 * 能力時間マスタモデルクラス
 *
 * @create 2020/08/18 KBS K.Seto
 * @update
 */
class MstAbility extends Model
{
	use Sortable;
	const CREATED_AT = null;
	public $sortableAs = ['fld1', 'fld2', 'fld3', 'fld4', 'fld5', 'fld6', 'fld7']; // ソート対象列別名一覧
	protected $table = 'mstAbility'; // テーブル名

	/**
	 * 能力時間名称列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/08/18　K.Seto
	 * @update 2020/12/09　S.Tanaka　第2キーに[mstAbility].[ID]を指定
	 */
	public function fld1Sortable($query, $direction)
	{
		return $query->orderBy('mstAbility.AbilityName', $direction)
						->orderBy('mstAbility.ID', $direction);
	}

	/**
	 * 職制列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/08/18　K.Seto
	 * @update 2020/12/09　S.Tanaka　第2キーに[mstAbility].[ID]を指定
	 */
	public function fld2Sortable($query, $direction)
	{
		return $query->orderBy('mstOrg.Name', $direction)
						->orderBy('mstAbility.ID', $direction);
	}

	/**
	 * 施工棟列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/08/18　K.Seto
	 * @update 2020/12/09　S.Tanaka　第2キーに[mstAbility].[ID]を指定
	 */
	public function fld3Sortable($query, $direction)
	{
		return $query->orderBy('mstFloor.Name', $direction)
						->orderBy('mstAbility.ID', $direction);
	}

	/**
	 * 職種列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/08/18　K.Seto
	 * @update 2020/12/09　S.Tanaka　第2キーに[mstAbility].[ID]を指定
	 */
	public function fld4Sortable($query, $direction)
	{
		return $query->orderBy('mstDist.Name', $direction)
						->orderBy('mstAbility.ID', $direction);
	}

	/**
	 * 開始日列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/08/18　K.Seto
	 * @update 2020/12/09　S.Tanaka　第2キーに[mstAbility].[ID]を指定
	 */
	public function fld5Sortable($query, $direction)
	{
		return $query->orderBy('mstAbility.Sdate', $direction)
						->orderBy('mstAbility.ID', $direction);
	}

	/**
	 * 終了日列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/08/18　K.Seto
	 * @update 2020/12/09　S.Tanaka　第2キーに[mstAbility].[ID]を指定
	 */
	public function fld6Sortable($query, $direction)
	{
		return $query->orderBy('mstAbility.Edate', $direction)
						->orderBy('mstAbility.ID', $direction);
	}

	/**
	 * 工数列ソートメソッド
	 *
	 * @param
	 * @return mix ソートクエリ
	 *
	 * @create 2020/08/18　K.Seto
	 * @update 2020/12/09　S.Tanaka　第2キーに[mstAbility].[ID]を指定
	 */
	public function fld7Sortable($query, $direction)
	{
		return $query->orderBy('mstAbility.Hr', $direction)
						->orderBy('mstAbility.ID', $direction);
	}

}
