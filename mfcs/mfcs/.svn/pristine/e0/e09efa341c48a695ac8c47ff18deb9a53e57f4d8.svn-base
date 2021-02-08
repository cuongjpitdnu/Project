<?php
/*
 * @web.php
 * その他処理関連ルーティング用ファイル
 *
 *  下記に分類出来ない場合のみこのファイルに記述してください。
 *   mst.php           共通マスタ
 *   schet.php         搭載日程
 *   schem.php         中日程
 *   sches.php         小日程
 *   report.php        帳票
 *
 * @create 2020/07/09 KBS K.Yoshihara
 *
 * @update
 */

/*
|--------------------------------------------------------------------------
| Web Routes
|--------------------------------------------------------------------------
|
| Here is where you can register web routes for your application. These
| routes are loaded by the RouteServiceProvider within a group which
| contains the "web" middleware group. Now create something great!
|
*/


Route::get('/index', 'Layouts\MenuController@main');

Route::get('/system', 'Layouts\MenuController@sub');
Route::get('/system/version/index', 'System\VersionController@index');
Route::get('/system/license/index', 'System\LicenseController@index');

Route::get('/mst', 'Layouts\MenuController@sub');
Route::get('/schet', 'Layouts\MenuController@sub');
Route::get('/schem', 'Layouts\MenuController@sub');
Route::get('/sches', 'Layouts\MenuController@sub');
Route::get('/report', 'Layouts\MenuController@sub');
Route::get('/errors/getusererror', function () {return view('errors/getusererror');});
Route::get('/errors/unregistereduser', function () {return view('errors/unregistereduser');});
Route::get('/errors/500', function () {return view('errors/500');});
Route::post('/heartbeat', 'Controller@heartBeat');

