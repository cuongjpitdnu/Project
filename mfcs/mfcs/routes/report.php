<?php
/*
 * @report.php
 * 帳票関連ルーティング用ファイル
 *
 * @create 2020/07/09 KBS K.Yoshihara
 *
 * @update
 */

Route::get('/summary/index', 'Report\SummaryController@index');
Route::post('/summary/settings', 'Report\SummaryController@settings');
Route::get('/summary/settings', 'Report\SummaryController@settings');