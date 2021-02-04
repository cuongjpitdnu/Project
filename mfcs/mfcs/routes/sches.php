<?php
/*
 * @sches.php
 * 小日程関連ルーティング用ファイル
 *
 * @create 2020/07/09 KBS K.Yoshihara
 *
 * @update
 */

Route::get('/Dist/index', 'Sches\DistController@index');
Route::post('/Dist/delete', 'Sches\DistController@delete');
Route::get('/Dist/create', 'Sches\DistController@create');
Route::get('/Dist/edit', 'Sches\DistController@edit');
Route::get('/Dist/show', 'Sches\DistController@show');
Route::post('/Dist/save', 'Sches\DistController@save');

Route::get('/pattern/index', 'Sches\PatternController@index');
Route::get('/pattern/manage', 'Sches\PatternController@manage');
Route::get('/pattern/create', 'Sches\PatternController@create');
Route::get('/pattern/edit', 'Sches\PatternController@edit');
Route::get('/pattern/show', 'Sches\PatternController@show');
Route::post('/pattern/save', 'Sches\PatternController@save');
Route::post('/pattern/delete', 'Sches\PatternController@delete');
Route::get('/pattern/pmanage', 'Sches\PatternController@pmanage');
Route::post('/pattern/deletedetail', 'Sches\PatternController@deletedetail');

Route::get('/makenittei/index', 'Sches\MakeNitteiController@index');
Route::post('/makenittei/execute', 'Sches\MakeNitteiController@execute');
Route::get('/makenittei/create', 'Sches\MakeNitteiController@create');
Route::post('/makenittei/accept', 'Sches\MakeNitteiController@accept');
Route::post('/makenittei/cancel', 'Sches\MakeNitteiController@cancel');