<?php
/*
 * @schem.php
 * 中日程関連ルーティング用ファイル
 *
 * @create 2020/07/09 KBS K.Yoshihara
 *
 * @update
 */

Route::get('/kotei/index', 'Schem\KoteiController@index');
Route::get('/kotei/create', 'Schem\KoteiController@create');
Route::get('/kotei/show', 'Schem\KoteiController@show');
Route::get('/kotei/edit', 'Schem\KoteiController@edit');
Route::post('/kotei/save', 'Schem\KoteiController@save');

Route::get('/pattern/index', 'Schem\PatternController@index');
Route::get('/pattern/show', 'Schem\PatternController@show');
Route::get('/pattern/create', 'Schem\PatternController@create');
Route::get('/pattern/edit', 'Schem\PatternController@edit');
Route::post('/pattern/save', 'Schem\PatternController@save');
Route::get('/pattern/indexdetail', 'Schem\PatternController@indexDetail');
Route::get('/pattern/createdetail', 'Schem\PatternController@createDetail');
Route::get('/pattern/editdetail', 'Schem\PatternController@editDetail');
Route::get('/pattern/showdetail', 'Schem\PatternController@showDetail');
Route::post('/pattern/deletedetail', 'Schem\PatternController@deleteDetail');
Route::post('/pattern/savedetail', 'Schem\PatternController@saveDetail');

Route::get('/import/index', 'Schem\ImportController@index');
Route::post('/import/import ','Schem\ImportController@import');
Route::get('/import/create ','Schem\ImportController@create');
Route::post('/import/accept ','Schem\ImportController@accept');
Route::post('/import/cancel ','Schem\ImportController@cancel');

//excel-Import
Route::get('/excelimport/index', 'Schem\ExcelImportController@index');
Route::post('/excelimport/getval3', 'Schem\ExcelImportController@ajaxGetVal4');
Route::post('/excelimport/output', 'Schem\ExcelImportController@excelImport');

Route::post('/excelimport/execute', 'Schem\ExcelImportController@execute');
Route::post('/excelimport/import', 'Schem\ExcelImportController@import');
Route::get('/excelimport/create', 'Schem\ExcelImportController@create');
Route::post('/excelimport/accept', 'Schem\ExcelImportController@accept');
Route::post('/excelimport/cancel ','Schem\ExcelImportController@cancel');

Route::get('/output/index', 'Schem\OutputController@index');
Route::post('/output/output', 'Schem\OutputController@output');

Route::get('/item/index', 'Schem\ItemController@index');
Route::get('/item/manage', 'Schem\ItemController@manage');
Route::post('/item/manage', 'Schem\ItemController@manage');
Route::post('/item/delete', 'Schem\ItemController@delete');
Route::get('/item/create', 'Schem\ItemController@create');
Route::get('/item/edit', 'Schem\ItemController@edit');
Route::get('/item/show', 'Schem\ItemController@show');
Route::post('/item/save', 'Schem\ItemController@save');
Route::get('/item/pmanage', 'Schem\ItemController@pmanage');
Route::get('/item/pedit', 'Schem\ItemController@pedit');
Route::get('/item/pshow', 'Schem\ItemController@pshow');
Route::post('/item/psave', 'Schem\ItemController@psave');

Route::get('/recalc/index', 'Schem\ReCalcController@index');
Route::post('/recalc/recalc', 'Schem\ReCalcController@recalc');

Route::get('/case/index', 'Schem\CaseController@index');
Route::get('/case/create', 'Schem\CaseController@create');
Route::post('/case/newsave', 'Schem\CaseController@newsave');
Route::get('/case/copy', 'Schem\CaseController@copy');
Route::post('/case/copysave', 'Schem\CaseController@copysave');
Route::get('/case/delete', 'Schem\CaseController@delete');
Route::post('/case/deletesave', 'Schem\CaseController@deletesave');

Route::get('/bdata/index', 'Schem\BDataController@index');
Route::post('/bdata/execute', 'Schem\BDataController@execute');
Route::get('/bdata/result', 'Schem\BDataController@result');

Route::get('/nitteireflect/index', 'Schem\NitteiReflectController@index');
Route::post('/nitteireflect/reflect', 'Schem\NitteiReflectController@reflect');