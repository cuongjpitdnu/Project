<?php
/*
 * @mst.php
 * 共通マスタ関連ルーティング用ファイル
 *
 * @create 2020/07/09 KBS K.Yoshihara
 *
 * @update
 */

Route::get('/order/index', 'Mst\OrderController@index');
Route::get('/order/create', 'Mst\OrderController@create');
Route::get('/order/edit', 'Mst\OrderController@edit');
Route::get('/order/show', 'Mst\OrderController@show');
Route::post('/order/save', 'Mst\OrderController@save');


Route::get('/org/index', 'Mst\OrgController@index');
Route::get('/org/changedate', 'Mst\OrgController@changeDate');
Route::get('/org/show', 'Mst\OrgController@show');
Route::get('/org/create', 'Mst\OrgController@create');
Route::post('/org/save', 'Mst\OrgController@save');
Route::get('/org/edit', 'Mst\OrgController@edit');
Route::post('/org/delete', 'Mst\OrgController@delete');
Route::post('/org/checkdelete', 'Mst\OrgController@checkDelete');


Route::get('/bdata/index', 'Mst\BDataController@index');
Route::get('/bdata/create', 'Mst\BDataController@create');
Route::get('/bdata/edit', 'Mst\BDataController@edit');
Route::get('/bdata/show', 'Mst\BDataController@show');
Route::post('/bdata/save', 'Mst\BDataController@save');

Route::get('/floor/index', 'Mst\FloorController@index');
Route::get('/floor/create', 'Mst\FloorController@create');
Route::get('/floor/edit', 'Mst\FloorController@edit');
Route::get('/floor/show', 'Mst\FloorController@show');
Route::post('/floor/save', 'Mst\FloorController@save');

Route::get('/ability/index', 'Mst\AbilityController@index');
Route::get('/ability/create', 'Mst\AbilityController@create');
Route::get('/ability/edit', 'Mst\AbilityController@edit');
Route::get('/ability/show', 'Mst\AbilityController@show');
Route::post('/ability/save', 'Mst\AbilityController@save');
Route::post('/ability/checksave', 'Mst\AbilityController@checkSave');

Route::get('/member/index', 'Mst\MemberController@index');
Route::get('/member/changedate', 'Mst\MemberController@changeDate');
Route::get('/member/search', 'Mst\MemberController@search');
Route::get('/member/history', 'Mst\MemberController@history');
Route::post('/member/historydelete', 'Mst\MemberController@historydelete');
Route::get('/member/historycreate', 'Mst\MemberController@historycreate');
Route::get('/member/historyedit', 'Mst\MemberController@historyedit');
Route::get('/member/historyshow', 'Mst\MemberController@historyshow');
Route::post('/member/historysave', 'Mst\MemberController@historySave');
Route::get('/member/create', 'Mst\MemberController@create');
Route::get('/member/edit', 'Mst\MemberController@edit');
Route::get('/member/show', 'Mst\MemberController@show');
Route::post('/member/checkinsert', 'Mst\MemberController@checkInsert');
Route::post('/member/insert', 'Mst\MemberController@insert');
Route::post('/member/update', 'Mst\MemberController@update');

//Route::get('/member/index', 'Mst\MemberController@index');
//Route::get('/floor/index', 'Mst\FloorController@index');
//Route::get('/ability/index', 'Mst\AbilityController@index');
