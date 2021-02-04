<?php
/*
 * @MenuInfo.php
 * メニュー表示情報クラスファイル
 *
 * @create 2020/07/09 KBS K.Yoshihara
 *
 * @update
 */

namespace App\Librarys;

/**
 * メニュー表示情報クラス
 *
 * @create 2020/07/09　K.Yoshihara
 * @update
 */
class MenuInfo
{
	public $SessionID; // セッションID
	public $UserID; // ユーザーID
	public $GuestID; // ゲストユーザーの場合のWindowsログインユーザーID
	public $UserName; // ユーザー名
	public $Kinds; // システム種類マスタレコードの配列
	public $Menus; // システムメニューマスタレコードの配列
	public $IsReadOnly; // メニューに書き込み権限が有るか
	public $KindID; // システム種類ID
	public $KindURL; // システムURL
	public $MenuID; // システムメニューマスタID
	public $MenuURL; // メニューURL
	public $MenuNick; // メニュー略称
}