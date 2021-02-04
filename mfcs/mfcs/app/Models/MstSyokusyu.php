<?php
/*
 * @MstSyokusyu.php
 * 人員マスタモデルクラスファイル
 *
 * @create 2020/09/11 Cuong
 * @update
 */
namespace App\Models;

use Illuminate\Database\Eloquent\Model;
/*
 * 人員マスタモデルクラス
 *
 * @create 2020/09/11 Cuong
 * @update
 */
class MstSyokusyu extends Model
{
	protected $table = 'mstSyokusyu'; // テーブル名
	protected $primaryKey = 'Code'; // 主キー
	public $incrementing = false;  // 主キーが自動増分されない
	protected $keyType = 'string';  //主キーが文字型
}
