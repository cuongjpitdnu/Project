<?php
/*
 * @schet.php
 * 搭載日程関連ルーティング用ファイル
 *
 * @create 2020/07/09 KBS K.Yoshihara
 *
 * @update
 */

Route::get('/index', 'Schet\topController@index');

Route::get('/Import/index', 'Schet\ImportController@index');
Route::post('/Import/import', 'Schet\ImportController@import');
Route::get('/Import/create', 'Schet\ImportController@create');
Route::post('/Import/save', 'Schet\ImportController@save');

Route::get('/case/index', 'Schet\CaseController@index');
Route::get('/case/create', 'Schet\CaseController@create');
Route::post('/case/newsave', 'Schet\CaseController@newsave');
Route::get('/case/copy', 'Schet\CaseController@copy');
Route::post('/case/copysave', 'Schet\CaseController@copysave');
Route::get('/case/delete', 'Schet\CaseController@delete');
Route::post('/case/deletesave', 'Schet\CaseController@deletesave');
Route::get('/case/apply', 'Schet\CaseController@apply');
