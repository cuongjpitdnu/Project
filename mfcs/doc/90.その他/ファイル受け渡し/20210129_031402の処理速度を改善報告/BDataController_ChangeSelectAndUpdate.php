<?php
	/*
	 * @BDataController.php
	 * Schem Bdata Controller file
	 *
	 * @create 2020/12/02 Cuong
	 *
	 *
	 * 
	 */
	namespace App\Http\Controllers\Schem;

	use App\Http\Controllers\Controller;
	use Illuminate\Http\Request;
	use App\Models\MstProject;
	use App\Models\MstOrderNo;
	use App\Models\Cyn_BlockKukaku;
	use App\Models\Cyn_Plan;
	use App\Models\Cyn_mstKotei;
	use App\Models\Cyn_Block_BD;
	use App\Models\MstBDCode;
	use App\Models\Cyn_C_BlockKukaku;
	use App\Models\Cyn_C_Plan;
	use App\Models\Cyn_Temp_Block_BD;
	use App\Models\Cyn_TosaiData;
	use App\Http\Requests\Schem\BDataContentsRequest;
	use PhpOffice\PhpSpreadsheet\IOFactory;
	use PhpOffice\PhpSpreadsheet\Style\Border;
	use PhpOffice\PhpSpreadsheet\Style\Font;
	use PhpOffice\PhpSpreadsheet\Style\Fill;
	use PhpOffice\PhpSpreadsheet\Shared\Date;
	use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
	use PhpOffice\PhpSpreadsheet\Style\Style;
	use Illuminate\Database\QueryException;
	use App\Librarys\FuncCommon;
	use Illuminate\Support\Facades\DB;
	use PhpOffice\PhpSpreadsheet\Cell\Coordinate;

	/*
	 * BDataController class
	 *
	 * @create 2020/12/02 Cuong
	 * @update
	 */
	class BDataController extends Controller
	{
		/**
		 * 日程取込条件設定画面
		 *
		 * @param Request 呼び出し元リクエストオブジェクト
		 * @return View ビュー
		 *
		 * @create 2020/12/02 Cuong
		 * @update
		*/
		public function index(Request $request)
		{
			return $this->initialize($request);
		}

		/**
		 * initial display processing
		 *
		 * @param Request
		 * @return View
		 *
		 * @create 2020/12/02 Cuong
		 * @update
		 */
		private function initialize(Request $request) {
			$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));
			// 戻り値のデータ型をチェック
			if ($this->isRedirectMenuInfo($menuInfo)) {
				// エラーが起きたのでリダイレクト
				return $menuInfo;
			}
			//initialize $originalError
			$originalError = [];
			if (isset($request->err1)) {
				$originalError[] = valueUrlDecode($request->err1);
			}
			// initialize $itemShow
			$itemShow = array(
				'val1' => isset($request->val1) ? valueUrlDecode($request->val1) :
							((trim(old('val1')) != '') ? valueUrlDecode(old('val1')) : 
							config('system_const.c_kind_chijyo')),
				'val2' => isset($request->val2) ? valueUrlDecode($request->val2) :
							((trim(old('val2')) != '') ? valueUrlDecode(old('val2')) :
							config('system_const_schem.bd_val_export')),
				'val3' => isset($request->val3) ? valueUrlDecode($request->val3) :
							((trim(old('val3')) != '') ? valueUrlDecode(old('val3')) : ''),
				'val4' => isset($request->val4) ? valueUrlDecode($request->val4) :
							((trim(old('val4')) != '') ? valueUrlDecode(old('val4')) : ''),
				'val5' => isset($request->val5) ? valueUrlDecode($request->val5) : 
							(trim(old('val5') != '') ? old('val5') : 1),
				'val6' => isset($request->val6) ? valueUrlDecode($request->val6) : 
							(trim(old('val6') != '') ? old('val6') : ''),
				'val8' => isset($request->val8) ? valueUrlDecode($request->val8) : 
							(trim(old('val8') != '') ? old('val8') : config('system_const.displayed_results_1')),
			);
			
			//get data val3
			$data_val3 = $this->getDataVal3($menuInfo, $itemShow['val1']);
			if(count($data_val3) > 0) {
				$arrUnique = array();
				foreach($data_val3 as $key => &$item) {
					if(count($arrUnique) == 0) {
						$arrUnique[] = $item->val3Name;
					} else {
						if(!in_array($item->val3Name, $arrUnique)) {
							$arrUnique[] = $item->val3Name;
						} else {
							unset($data_val3[$key]);
						}
					}
				}
			}

			$this->data['dataView']['data_3'] = $data_val3;
			$this->data['dataView']['data_3_all'] = $this->getDataVal3($menuInfo, '', true);
			$tempVal3 = ($itemShow['val3'] == config('system_const.project_listkind_tosai')) ?
						((count($data_val3) > 0) ? valueUrlDecode($data_val3->first()->val3) : config('system_const.project_listkind_tosai')) :
						$itemShow['val3'];
			$data_val4 = $this->getDataVal4($itemShow['val1'], $tempVal3);
			if(count($data_val4) > 0) {
				$arrUnique = array();
				foreach($data_val4 as $key => &$item) {
					if(count($arrUnique) == 0) {
						$arrUnique[] = $item->val4Name;
					} else {
						if(!in_array($item->val4Name, $arrUnique)) {
							$arrUnique[] = $item->val4Name;
						} else {
							unset($data_val4[$key]);
						}
					}
				}
			}

			// data 4 for val 4
			$this->data['dataView']['data_4'] = $data_val4;
			$this->data['dataView']['data_4_all'] = $this->getDataVal4('', '', true);
			
			$itemShow['val1'] = valueUrlEncode($itemShow['val1']);
			$itemShow['val2'] = valueUrlEncode($itemShow['val2']);
			$itemShow['val3'] = valueUrlEncode($itemShow['val3']);
			$itemShow['val4'] = valueUrlEncode($itemShow['val4']);
			$itemShow['val5'] = valueUrlEncode($itemShow['val5']);
			$itemShow['val6'] = valueUrlEncode($itemShow['val6']);
			$itemShow['val8'] = valueUrlEncode($itemShow['val8']);

			//request
			$this->data['menuInfo'] = $menuInfo;
			$this->data['request'] = $request;
			$this->data['originalError'] = $originalError;
			$this->data['itemShow'] = $itemShow;
			$this->data['msgTimeOut'] = valueUrlEncode(config('message.msg_cmn_err_002'));
			//return view with all data
			return view('Schem/BData/index', $this->data);
		}

		/**
		 * export excel and inport data  method
		 * @param Request BDataContentsRequest
		 * @return 
		 *
		 * @create 2020/12/02 Cuong
		 * @update
		 */
		public function execute(BDataContentsRequest  $request) {
			$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));
			// 戻り値のデータ型をチェック
			if ($this->isRedirectMenuInfo($menuInfo)) {
				// エラーが起きたのでリダイレクト
				return $menuInfo;
			}
			//validate form
			$validated = $request->validated();
			
			if($validated['val2'] == config("system_const_schem.bd_val_export")) {
				/* process export */
				$resExport = $this->export($menuInfo, $request, $validated);
				if($resExport != '') {
					return redirect($resExport);
				}
			}else {
				/* process import */
				$res = $this->import($menuInfo, $validated, $request);
				return redirect($res);
			}
		}

		/**
		 * export excel method
		 * @param menuInfo 
		 * @param request 
		 * @param validated 
		 * @return string
		 *
		 * @create 2020/12/02 Cuong
		 * @update 2020/12/29 Cuong update remove colunm style temp when count data < config('system_const_schem.bd_xl_initial_rows')
		 * @update 2020/01/11 Cuong update add and remove colunm style temp
		 */
		private function export($menuInfo, $request, $validated)
		{		
			$url = '';
			//Get data export from database.
			$data = $this->getDataExport($validated);

			// データが1件も取得できなかった場合
			if(count($data) == 0) {
				$url = url('/');
				$url .= '/' . $menuInfo->KindURL;
				$url .= '/' . $menuInfo->MenuURL;
				$url .= '/index';
				$url .= '?cmn1=' . $request->cmn1;
				$url .= '&cmn2=' . $request->cmn2;
				$url .= '&val1=' . valueUrlEncode($request->val1);
				$url .= '&val2=' . valueUrlEncode($request->val2);
				$url .= '&val3=' . valueUrlEncode($request->val3);
				$url .= '&val4=' . valueUrlEncode($request->val4);
				$url .= '&val5=' . valueUrlEncode($request->val5);
				$url .= '&val6=' . valueUrlEncode($request->val6);
				$url .= '&val8=' . valueUrlEncode($request->val8);
				$url .= '&err1=' . valueUrlEncode(config('message.msg_cmn_db_001'));
				return $url;
			}

			// 対象データのエクセル出力処理
			$arrayData = $this->processDataExport($data);
			// set $data null
			$data = null;

			// read file excel template
			$inputFileType = 'Xlsx';
			$inputFileName = config('system_const_schem.bd_export_template_path');
			if ($inputFileName != '') {
				$arrPath = explode('/', $inputFileName);
				$arrLength = count($arrPath);

				if ($arrLength > 2) {
					$inputFileName = public_path() . '\\' . $arrPath[$arrLength - 2] . '\\' . $arrPath[$arrLength - 1];
					// setting header
					$reader = IOFactory::createReader($inputFileType);
					$spreadsheet = $reader->load($inputFileName);
					$worksheet = $spreadsheet->getActiveSheet();
					$beginRow = config('system_const_schem.bd_xl_begin_row');
					$maxRow = count($arrayData) + (int)$beginRow - 1;

					$bd_xl_begin_col = config('system_const_schem.bd_xl_begin_col');
					$bd_xl_end_col = config('system_const_schem.bd_xl_end_col');
					$beginCol = Coordinate::stringFromColumnIndex($bd_xl_begin_col);
					$endCol = Coordinate::stringFromColumnIndex($bd_xl_end_col);

					/* set style excel */
					$maxRowTemp = config('system_const_schem.bd_xl_initial_rows') + (int)$beginRow - 1;
					// update remove colunm style temp when data record < config('system_const_schem.bd_xl_initial_rows')
					if($maxRow < $maxRowTemp) {
						$worksheet->removeRow($beginRow, $maxRowTemp - $maxRow);
					}
					// add colunm style temp when data record > config('system_const_schem.bd_xl_initial_rows')
					if($maxRow > $maxRowTemp) {
						$worksheet->insertNewRowBefore($maxRowTemp, $maxRow - $maxRowTemp);
					}

					// export data 
					$worksheet->fromArray($arrayData,NULL,$beginCol.$beginRow);
					$strFilename = $menuInfo->MenuNick;
					header("Content-Type: application/force-download");
					header("Content-Type: application/octet-stream");
					header("Content-Type: application/download");
					header('Content-Type:application/octet-stream; charset=Shift_JIS');
					header('Content-Type: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet');
					header('Content-Disposition: attachment; filename="' . $strFilename . '.xlsx"');
					header("Content-Transfer-Encoding: binary");
					header('Expires: 0');
					header('Cache-Control: no-cache, no-store');
					header('Pragma: no-cache');
					if (isset($_COOKIE['export']) && $_COOKIE['export'] == 0) {
						unset($_COOKIE['export']);
						setcookie("export", 1, -1, '/');
					}
					$writer = IOFactory::createWriter($spreadsheet, "Xlsx");
					ob_end_clean();
					flush();
					$writer->save("php://output");
				}
			}
		}

			/**
		 * display data method
		 * @param Request 
		 * @return 
		 *
		 * @create 2020/12/09 Cuong
		 * @update
		 */
		public function result(Request $request) {
			$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));
			// 戻り値のデータ型をチェック
			if ($this->isRedirectMenuInfo($menuInfo)) {
				// エラーが起きたのでリダイレクト
				return $menuInfo;
			}
			
			if(isset($request->val8) && in_array(valueUrlDecode($request->val8), [config('system_const.displayed_results_1'),
																	config('system_const.displayed_results_2'),
																	config('system_const.displayed_results_3')])){
				$pageunit = valueUrlDecode($request->val8);
			}else{
				$pageunit = config('system_const.displayed_results_1');
			}

			$sort = ['fld1','fld2','fld3','fld4','fld5','fld6','fld7'];
			if(isset($request->sort) && $request->sort != ''){
				$new = $request->sort;
				array_unshift($sort, $new);
				$sort = array_unique($sort);
			}

			$direction = (isset($request->direction) && $request->direction != '') ?  $request->direction : 'asc';

			$query = Cyn_Temp_Block_BD::select('Cyn_Temp_Block_BD.T_Name as fld1', 
													'Cyn_Temp_Block_BD.T_BKumiku as fld2', 
													'Cyn_Temp_Block_BD.Name as fld3', 
													'Cyn_Temp_Block_BD.BKumiku as fld4', 
													'Cyn_Temp_Block_BD.Kotei as fld5', 
													'Cyn_Temp_Block_BD.KKumiku as fld6', 
													'Cyn_Temp_Block_BD.Log as fld7', 
													'Cyn_mstKotei.Name')
								->where('Cyn_Temp_Block_BD.Ckind', '=', valueUrlDecode($request->val1))
								->leftJoin('Cyn_mstKotei', function($join) {
									$join->on('Cyn_mstKotei.Code', '=', 'Cyn_Temp_Block_BD.Kotei'); 
									$join->on('Cyn_mstKotei.CKind', '=', 'Cyn_Temp_Block_BD.CKind'); 
								})->get();
			
			$rows = $this->sortAndPagination($query, $sort, $direction, $pageunit, $request);

			$rows->getCollection()->transform(function ($value) {
				// format fld2
				$value['fld2'] = FuncCommon::getKumikuData($value['fld2'])[2];
	
				// format fld4
				$value['fld4'] = FuncCommon::getKumikuData($value['fld4'])[2];
	
				// format fld5
				$value['fld5'] = $value['fld5'] .config('system_const.code_name_separator').$value['Name'];
				
				// format fld6
				$value['fld6'] = FuncCommon::getKumikuData($value['fld6'])[2];
				return $value;
			});

			//ビューを表示
			return view('schem/bdata/result')->with([
				'request' => $request,
				'rows' => $rows,
				'menuInfo' => $menuInfo,
			]);
		}

		/**
		 * get data export method
		 * @param validated 
		 * @return 
		 *
		 * @create 2020/12/02 Cuong
		 * @update
		 */
		private function getDataExport($validated) {
			/* Get data */
			$data_1 = Cyn_TosaiData::select(
				'Cyn_BlockKukaku.T_Name',
				'Cyn_BlockKukaku.T_BKumiku',
				'Cyn_BlockKukaku.Name',
				'Cyn_BlockKukaku.BKumiku',

				'Cyn_Plan.Kotei',
				'Cyn_Plan.KKumiku',

				'Cyn_mstKotei.Name AS KoteiName',
				'Cyn_Block_BD.ID',
				'Cyn_Block_BD.BD_Code',
				'Cyn_Block_BD.BData',
				
				'mstBDCode.Name as BDName',
			)
			->selectRaw('\'1\' as GroupData')
			->join('Cyn_BlockKukaku', function($join) {
				$join->on('Cyn_TosaiData.ProjectID', '=', 'Cyn_BlockKukaku.ProjectID')
					->on('Cyn_TosaiData.OrderNo', '=', 'Cyn_BlockKukaku.OrderNo')
					->on('Cyn_TosaiData.CKind', '=', 'Cyn_BlockKukaku.CKind')
					->on('Cyn_TosaiData.Name', '=', 'Cyn_BlockKukaku.T_Name')
					->on('Cyn_TosaiData.BKumiku', '=', 'Cyn_BlockKukaku.T_BKumiku');
				})
			->join('Cyn_Plan', function($join) {
				$join->on('Cyn_BlockKukaku.ProjectID', '=', 'Cyn_Plan.ProjectID')
					->on('Cyn_BlockKukaku.OrderNo', '=', 'Cyn_Plan.OrderNo')
					->on('Cyn_BlockKukaku.No', '=', 'Cyn_Plan.No');
				})
			->leftJoin('Cyn_mstKotei', function($join) use($validated)  {
				$join->on('Cyn_mstKotei.Code', '=', 'Cyn_Plan.Kotei')
				->where('Cyn_mstKotei.DelFlag', '=', 0)
				->where('Cyn_mstKotei.CKind', '=', $validated['val1']);
			})
			->leftJoin('Cyn_Block_BD', function($join) {
				$join->on('Cyn_Block_BD.ProjectID', '=', 'Cyn_Plan.ProjectID')
					->on('Cyn_Block_BD.OrderNo', '=', 'Cyn_Plan.OrderNo')
					->on('Cyn_Block_BD.Kotei', '=', 'Cyn_Plan.Kotei')
					->on('Cyn_Block_BD.KKumiku', '=', 'Cyn_Plan.KKumiku')
					->on('Cyn_BlockKukaku.T_Name', '=', 'Cyn_Block_BD.T_Name')
					->on('Cyn_BlockKukaku.T_BKumiku', '=', 'Cyn_Block_BD.T_BKumiku')
					->on('Cyn_BlockKukaku.Name', '=', 'Cyn_Block_BD.Name')
					->on('Cyn_BlockKukaku.BKumiku', '=', 'Cyn_Block_BD.BKumiku');
				})
			->leftJoin('mstBDCode', function($join) { 
				$join->on('mstBDCode.Code', '=', 'Cyn_Block_BD.BD_Code')
				->where('mstBDCode.ViewFlag', '=', 1);
			})

			->where('Cyn_TosaiData.ProjectID', '=', $validated['val3'])
			->where('Cyn_TosaiData.OrderNo', '=', $validated['val4'])
			->where('Cyn_TosaiData.CKind', '=', $validated['val1'])
			->where('Cyn_TosaiData.WorkItemID', '!=', 0)
			->whereNull('Cyn_BlockKukaku.Del_Date')
			->whereNull('Cyn_Plan.Del_Date');
			
			$data_2 = Cyn_TosaiData::select(
				'Cyn_C_BlockKukaku.T_Name',
				'Cyn_C_BlockKukaku.T_BKumiku',
				'Cyn_C_BlockKukaku.Name',
				'Cyn_C_BlockKukaku.BKumiku',

				'Cyn_C_Plan.Kotei',
				'Cyn_C_Plan.KKumiku',

				'Cyn_mstKotei.Name AS KoteiName',
				'Cyn_Block_BD.ID',
				'Cyn_Block_BD.BD_Code',
				'Cyn_Block_BD.BData',
				
				'mstBDCode.Name as BDName',
			)
			->selectRaw('\'2\' as GroupData')
			->join('Cyn_BlockKukaku', function($join) {
				$join->on('Cyn_TosaiData.ProjectID', '=', 'Cyn_BlockKukaku.ProjectID')
					->on('Cyn_TosaiData.OrderNo', '=', 'Cyn_BlockKukaku.OrderNo')
					->on('Cyn_TosaiData.CKind', '=', 'Cyn_BlockKukaku.CKind')
					->on('Cyn_TosaiData.Name', '=', 'Cyn_BlockKukaku.T_Name')
					->on('Cyn_TosaiData.BKumiku', '=', 'Cyn_BlockKukaku.T_BKumiku');
				})
			->join('Cyn_C_BlockKukaku', function($join) {
				$join->on('Cyn_C_BlockKukaku.ProjectID', '=', 'Cyn_BlockKukaku.ProjectID')
					->on('Cyn_C_BlockKukaku.OrderNo', '=', 'Cyn_BlockKukaku.OrderNo')
					->on('Cyn_C_BlockKukaku.CKind', '=', 'Cyn_BlockKukaku.CKind')
					->on('Cyn_C_BlockKukaku.T_Name', '=', 'Cyn_BlockKukaku.Name')
					->on('Cyn_C_BlockKukaku.T_BKumiku', '=', 'Cyn_BlockKukaku.BKumiku');
				})
			->join('Cyn_C_Plan', function($join) {
				$join->on('Cyn_C_BlockKukaku.ProjectID', '=', 'Cyn_C_Plan.ProjectID')
					->on('Cyn_C_BlockKukaku.OrderNo', '=', 'Cyn_C_Plan.OrderNo')
					->on('Cyn_C_BlockKukaku.No', '=', 'Cyn_C_Plan.No');
				})
			->leftJoin('Cyn_mstKotei', function($join) use($validated)  {
				$join->on('Cyn_mstKotei.Code', '=', 'Cyn_C_Plan.Kotei')
				->where('Cyn_mstKotei.DelFlag', '=', 0)
				->where('Cyn_mstKotei.CKind', '=', $validated['val1']);
			})
			->leftJoin('Cyn_Block_BD', function($join) {
				$join->on('Cyn_Block_BD.ProjectID', '=', 'Cyn_C_Plan.ProjectID')
					->on('Cyn_Block_BD.OrderNo', '=', 'Cyn_C_Plan.OrderNo')
					->on('Cyn_Block_BD.Kotei', '=', 'Cyn_C_Plan.Kotei')
					->on('Cyn_Block_BD.KKumiku', '=', 'Cyn_C_Plan.KKumiku')
					->on('Cyn_C_BlockKukaku.T_Name', '=', 'Cyn_Block_BD.T_Name')
					->on('Cyn_C_BlockKukaku.T_BKumiku', '=', 'Cyn_Block_BD.T_BKumiku')
					->on('Cyn_C_BlockKukaku.Name', '=', 'Cyn_Block_BD.Name')
					->on('Cyn_C_BlockKukaku.BKumiku', '=', 'Cyn_Block_BD.BKumiku');
				})
			->leftJoin('mstBDCode', function($join) { 
				$join->on('mstBDCode.Code', '=', 'Cyn_Block_BD.BD_Code')
				->where('mstBDCode.ViewFlag', '=', 1);
			})
			->where('Cyn_TosaiData.ProjectID', '=', $validated['val3'])
			->where('Cyn_TosaiData.OrderNo', '=', $validated['val4'])
			->where('Cyn_TosaiData.CKind', '=', $validated['val1'])
			->where('Cyn_TosaiData.WorkItemID', '=', 0)
			->whereNull('Cyn_BlockKukaku.Del_Date')
			->whereNull('Cyn_C_BlockKukaku.Del_Date')
			->whereNull('Cyn_C_Plan.Del_Date');

			$data = $data_1->unionAll($data_2)
							->orderBy('T_Name', 'asc')
							->orderBy('T_BKumiku', 'asc')
							->orderBy('Name', 'asc')
							->orderBy('BKumiku', 'asc')
							->orderBy('Kotei', 'asc')
							->orderBy('KKumiku', 'asc')
							->orderBy('ID', 'asc')
							->get()->toArray();
			return $data;
		}

		/**
		 * process data before export method
		 * @param validated 
		 * @return array
		 *
		 * @create 2020/12/02 Cuong
		 * @update
		 */
		private function processDataExport($data) {
			$arrayData = array(); 	//出力リスト
			$arrDataLoopOld = array();
			$dataLoopOld = array();
			$bdBeginCol = config('system_const_schem.bd_xl_bd_begin_col'); 		//bdcode begin column
			$bdEndCol = config('system_const_schem.bd_xl_end_col');				//bdcode end column

			// Loop data get from database
			foreach($data as $key=>$item) {
				$groupName = $item['T_Name'].$item['T_BKumiku'].$item['Name'].$item['BKumiku'].$item['Kotei'].$item['KKumiku'];
				// has groupname in array data looped
				if(in_array($groupName, $arrDataLoopOld)) {
					// 「出力リスト」[ID]がnullではない場合
					if(!is_null($item['ID'])) {
						$colBDCode = (((int)$item['ID'] - 1) * 2) + (int)$bdBeginCol - 1;	//column index BDCode
						$colBData = (((int)$item['ID'] - 1) * 2) + (int)$bdBeginCol;		//column index BData
						for ($i=$bdBeginCol-1; $i < $bdEndCol; $i++) { 
							if($i == $colBDCode) {
								$dataLoopOld[$i] = $item['BD_Code'].config('system_const.code_name_separator').$item['BDName'];
							}elseif($i == $colBData){
								$dataLoopOld[$i] = $item['BData'];
							}else {
								if(isset($dataLoopOld[$i]) && is_null($dataLoopOld[$i])){
									$dataLoopOld[$i] = NULL;
								}
							}
						}
					}else {
						for ($i=$bdBeginCol-1; $i < $bdEndCol; $i++) { 
							$dataLoopOld[$i] = NULL;
						}
					}
				}else {
					// has not groupname in array data looped
					if(count($dataLoopOld) > 0) {
						array_push($arrayData,$dataLoopOld);
						$dataLoopOld = array();
						$dataLoopOld[0] = $item['T_Name'];
						$dataLoopOld[1] = FuncCommon::getKumikuData($item['T_BKumiku'])[2];
						$dataLoopOld[2] = $item['Name'];
						$dataLoopOld[3] = FuncCommon::getKumikuData($item['BKumiku'])[2];
						$dataLoopOld[4] = $item['Kotei'].config('system_const.code_name_separator').$item['KoteiName'];
						$dataLoopOld[5] = FuncCommon::getKumikuData($item['KKumiku'])[2];

						if(!is_null($item['ID'])) {
							$colBDCode = (((int)$item['ID'] - 1) * 2) + (int)$bdBeginCol - 1;
							$colBData = (((int)$item['ID'] - 1) * 2) + (int)$bdBeginCol;
							for ($i=$bdBeginCol-1; $i < $bdEndCol; $i++) { 
								if($i == $colBDCode) {
									$dataLoopOld[$i] = $item['BD_Code'].config('system_const.code_name_separator').$item['BDName'];
								}elseif($i == $colBData){
									$dataLoopOld[$i] = $item['BData'];
								}else {
									$dataLoopOld[$i] = NULL;
								}
								
							}
						}else {
							for ($i=$bdBeginCol-1; $i < $bdEndCol; $i++) { 
								$dataLoopOld[$i] = NULL;
							}
						}
					}else {
						$dataLoopOld[0] = $item['T_Name'];
						$dataLoopOld[1] = FuncCommon::getKumikuData($item['T_BKumiku'])[2];
						$dataLoopOld[2] = $item['Name'];
						$dataLoopOld[3] = FuncCommon::getKumikuData($item['BKumiku'])[2];
						$dataLoopOld[4] = $item['Kotei'].config('system_const.code_name_separator').$item['KoteiName'];
						$dataLoopOld[5] = FuncCommon::getKumikuData($item['KKumiku'])[2];

						if(!is_null($item['ID'])) {
							$colBDCode = (((int)$item['ID'] - 1) * 2) + (int)$bdBeginCol - 1;
							$colBData = (((int)$item['ID'] - 1) * 2) + (int)$bdBeginCol;
							for ($i=$bdBeginCol-1; $i < $bdEndCol; $i++) { 
								if($i == $colBDCode) {
									$dataLoopOld[$i] = $item['BD_Code'].config('system_const.code_name_separator').$item['BDName'];
								}elseif($i == $colBData) {
									$dataLoopOld[$i] = $item['BData'];
								}else {
									$dataLoopOld[$i] = NULL;
								}
								
							}
						}else {
							for ($i=$bdBeginCol-1; $i < $bdEndCol; $i++) { 
								$dataLoopOld[$i] = NULL;
							}
						}
					}
					array_push($arrDataLoopOld, $groupName);
				}
				
				if($key == (count($data) - 1)) {
					if(count($dataLoopOld) > 0 ) {
						array_push($arrayData,$dataLoopOld);
					}
				}
			}

			return $arrayData;
		}

		/**
		* import excel method
		* @param menuInfo 
		* @param validated 
		* @return 
		*
		* @create 2020/12/02 Cuong
		* @update 2021/01/14 Cuong	update condition when insert update data.
		*/
		private function import($menuInfo, $validated, $request)
		{	
			$startReadDataTime = microtime(true);

			// read data file excel
			$dataExcel = $this->readDataFileExcel($request);

			$caculaSearchData = microtime(true) - $startReadDataTime;
			if (isset($_COOKIE['ReadDataExcelFile_Time']) && $_COOKIE['ReadDataExcelFile_Time']) {
				unset($_COOKIE['ReadDataExcelFile_Time']);
				setcookie("ReadDataExcelFile_Time", $caculaSearchData, -1, '/');
			}else{
				setcookie("ReadDataExcelFile_Time", $caculaSearchData, -1, '/');
			}

			$startPrcessDataTime = microtime(true);

			// get all code of data bdcode.
			$listBDCode = MstBDCode::select('Code')->get()->toArray();

			if(count($dataExcel) > 0) {
				$listLog = array();					//list data log
				$listSave = array();				//list data save
				$listDelete = array();				//list data delete
				$errMessage = array();				//list data error message

				// 保存リスト作成処理 and 物量読込処理
				$this->processDataExcel($dataExcel, $listBDCode, $request, $listLog, $listSave, $listDelete, $errMessage);

				$caculaProcessData = microtime(true) - $startPrcessDataTime;
				if (isset($_COOKIE['ProcessDataExcel_Time']) && $_COOKIE['ProcessDataExcel_Time']) {
					unset($_COOKIE['ProcessDataExcel_Time']);
					setcookie("ProcessDataExcel_Time", $caculaProcessData, -1, '/');
				}else{
					setcookie("ProcessDataExcel_Time", $caculaProcessData, -1, '/');
				}

				// process Lock
				$resultProcessLock = $this->tryLock($menuInfo->KindID, config('system_const_schem.sys_menu_id_plan'), 
				$menuInfo->UserID, $menuInfo->SessionID, $request->val1, false);

				// 戻り値がnullではない場合
				if(!is_null($resultProcessLock)) {
					array_push($errMessage, $resultProcessLock);
					$urlErr = url('/');
					$urlErr .= '/' . $menuInfo->KindURL;
					$urlErr .= '/' . $menuInfo->MenuURL;
					$urlErr .= '/index';
					$urlErr .= '?cmn1=' . $request->cmn1;
					$urlErr .= '&cmn2=' . $request->cmn2;
					$urlErr .= '&val1=' . valueUrlEncode($request->val1);
					$urlErr .= '&val2=' . valueUrlEncode($request->val2);
					$urlErr .= '&val3=' . valueUrlEncode($request->val3);
					$urlErr .= '&val4=' . valueUrlEncode($request->val4);
					$urlErr .= '&val5=' . valueUrlEncode($request->val5);
					$urlErr .= '&val6=' . valueUrlEncode($request->val6);
					$urlErr .= '&val8=' . valueUrlEncode($request->val8);
					$urlErr .= '&err1=' . valueUrlEncode($resultProcessLock);
					return $urlErr;
				}
				$startInsertUpdateDataTime = microtime(true);

				/* insert update data to datatbase */
				try {
					//beginTransaction
					$dataCyn_Block_BD = Cyn_Block_BD::select('T_Name','T_BKumiku','Name','BKumiku','Kotei',
					'KKumiku','ID','BData', 'BD_Code')
					->selectRaw("(T_Name+'_'+T_BKumiku+'_'+Name+'_'+BKumiku+'_'+Kotei+'_'+KKumiku+'_'+CAST(Cyn_Block_BD.ID as VARCHAR)) as keyCheck")
					->where('ProjectID', '=', $validated['val3'])
					->where('OrderNo', '=', $validated['val4'])->get()->keyBy('keyCheck')->toArray();

					DB::transaction(function() use ($validated, $request, $listDelete, $listSave, $listLog, $dataCyn_Block_BD){
						if(count($listDelete) > 0) {
							// 削除処理「削除リスト
							foreach($listDelete as $delData) {
								$resulDel = Cyn_Block_BD::where('ProjectID', '=', $validated['val3'])
												->where('OrderNo', '=', $validated['val4'])
												->where('ID', '=', $delData['ID'])
												->where('T_Name', '=', $delData['T_Name'])
												->where('T_BKumiku', '=', $delData['T_BKumiku'])
												->where('Name', '=', $delData['Name'])
												->where('BKumiku', '=', $delData['BKumiku'])
												->where('Kotei', '=', $delData['Kotei'])
												->where('KKumiku', '=', $delData['KKumiku'])
												->delete();
							}
						}

						if(count($listSave) > 0) {
							$arrDataSaveInser = array();
							foreach($listSave as $data) {
								$keyCheck = $data['T_Name'].'_'.$data['T_BKumiku'].'_'.$data['Name']
										.'_'.$data['BKumiku'].'_'.$data['Kotei'].'_'.$data['KKumiku'].'_'.$data['ID'];
								if(isset($dataCyn_Block_BD[$keyCheck])) {	//data exists
									if($dataCyn_Block_BD[$keyCheck]['BD_Code'] != $data['BD_Code'] || 
										$dataCyn_Block_BD[$keyCheck]['BData'] != $this->toFloat($data['BData'])) {
										
										$result = Cyn_Block_BD::where('ProjectID', '=', $validated['val3'])
															->where('OrderNo', '=', $validated['val4'])
															->where('T_Name', '=', $data['T_Name'])
															->where('T_BKumiku', '=', $data['T_BKumiku'])
															->where('Name', '=', $data['Name'])
															->where('BKumiku', '=', $data['BKumiku'])
															->where('Kotei', '=', $data['Kotei'])
															->where('KKumiku', '=', $data['KKumiku'])
															->where('ID', '=', $data['ID'])
															->delete();


										$arrTempDataInser = array();
										$arrTempDataInser['ProjectID'] = $validated['val3'];
										$arrTempDataInser['OrderNo'] = $validated['val4'];
										$arrTempDataInser['T_Name'] = $data['T_Name'];
										$arrTempDataInser['T_BKumiku'] = $data['T_BKumiku'];
										$arrTempDataInser['Name'] = $data['Name'];
										$arrTempDataInser['BKumiku'] = $data['BKumiku'];
										$arrTempDataInser['Kotei'] = $data['Kotei'];
										$arrTempDataInser['KKumiku'] = $data['KKumiku'];
										$arrTempDataInser['ID'] = $data['ID'];
										$arrTempDataInser['BD_Code'] = $data['BD_Code'];
										$arrTempDataInser['BData'] = $this->toFloat($data['BData']);
										array_push($arrDataSaveInser, $arrTempDataInser);

									}
								}else {
									$arrTempDataInser = array();
									$arrTempDataInser['ProjectID'] = $validated['val3'];
									$arrTempDataInser['OrderNo'] = $validated['val4'];
									$arrTempDataInser['T_Name'] = $data['T_Name'];
									$arrTempDataInser['T_BKumiku'] = $data['T_BKumiku'];
									$arrTempDataInser['Name'] = $data['Name'];
									$arrTempDataInser['BKumiku'] = $data['BKumiku'];
									$arrTempDataInser['Kotei'] = $data['Kotei'];
									$arrTempDataInser['KKumiku'] = $data['KKumiku'];
									$arrTempDataInser['ID'] = $data['ID'];
									$arrTempDataInser['BD_Code'] = $data['BD_Code'];
									$arrTempDataInser['BData'] = $this->toFloat($data['BData']);
									array_push($arrDataSaveInser, $arrTempDataInser);
								}

								if($data['A_BD_Code'] == $data['BD_Code']) {
									$cyn_Plan['BData'] = $this->toFloat($data['BData']);
									$result = Cyn_Plan::where('ProjectID', '=', $validated['val3'])
														->where('OrderNo', '=', $validated['val4'])
														->where('No', '=', $data['A_No'])
														->where('KoteiNo', '=', $data['A_KoteiNo'])
														->update($cyn_Plan);
								}

								if($data['B_BD_Code'] == $data['BD_Code']) {
									$cyn_C_Plan['BData'] = $this->toFloat($data['BData']);
									$result = Cyn_C_Plan::where('ProjectID', '=', $validated['val3'])
														->where('OrderNo', '=', $validated['val4'])
														->where('No', '=', $data['B_No'])
														->where('KoteiNo', '=', $data['B_KoteiNo'])
														->update($cyn_C_Plan);
								}
							}

							if(count($arrDataSaveInser) > 0) {
								$arrChunk = array_chunk($arrDataSaveInser, 190);
								foreach ($arrChunk as $arrData) {
									Cyn_Block_BD::insert($arrData);
								}
							}
						}

						// 「ログ出力」(A5)=On
						if($request->val5 == 1) {
							$delTempBlockBD = Cyn_Temp_Block_BD::where('CKind','=', $validated['val1'])->delete();
							if(count($listLog) > 0) {
								$arrDataLog = array();
								$index = 1;
								foreach($listLog as $log) {
									$temBlockDB = array();
									$temBlockDB['CKind'] = $validated['val1'];
									$temBlockDB['ID'] = $index;
									$temBlockDB['T_Name'] = $log['T_Name'];
									$temBlockDB['T_BKumiku'] = $log['T_BKumiku'];
									$temBlockDB['Name'] = $log['Name'];
									$temBlockDB['BKumiku'] = $log['BKumiku'];
									$temBlockDB['Kotei'] = $log['Kotei'];
									$temBlockDB['KKumiku'] = $log['KKumiku'];
									$temBlockDB['Log'] = $log['Log'];

									array_push($arrDataLog, $temBlockDB);
									$index++;
								}

								if(count($arrDataLog) > 0) {
									$arrDataLogChunk = array_chunk($arrDataLog, 190);
									foreach ($arrDataLogChunk as $dataLog) {
										Cyn_Temp_Block_BD::insert($dataLog);
									}
								}
							}
						}
					});
				} finally {
					// process delete Lock
					$deleteLock = $this->deleteLock ($menuInfo->KindID, config('system_const_schem.sys_menu_id_plan'), 
					$menuInfo->SessionID, $request->val1);
				}

				$caculaInserUpdateData = microtime(true) - $startInsertUpdateDataTime;
				if (isset($_COOKIE['ProcessInserUpdateData_Time']) && $_COOKIE['ProcessInserUpdateData_Time']) {
					unset($_COOKIE['ProcessInserUpdateData_Time']);
					setcookie("ProcessInserUpdateData_Time", $caculaInserUpdateData, -1, '/');
				}else{
					setcookie("ProcessInserUpdateData_Time", $caculaInserUpdateData, -1, '/');
				}

				/* 下記をURLに連結して、GETで遷移する */
					//「ログ出力」(A5)=Offの場合
				if($request->val5 != 1) {
					$url = url('/');
					$url .= '/' . $menuInfo->KindURL;
					$url .= '/' . $menuInfo->MenuURL;
					$url .= '/index';
					$url .= '?cmn1=' . $request->cmn1;
					$url .= '&cmn2=' . $request->cmn2;
					$url .= '&val1=' . valueUrlEncode($request->val1);
					$url .= '&val2=' . valueUrlEncode($request->val2);
					$url .= '&val3=' . valueUrlEncode($request->val3);
					$url .= '&val4=' . valueUrlEncode($request->val4);
					$url .= '&val5=' . valueUrlEncode($request->val5);
					$url .= '&val6=' . valueUrlEncode($request->val6);
					$url .= '&val8=' . valueUrlEncode($request->val8);

					if(count($listLog) >= 1) {
						$url .= '&err1=' . valueUrlEncode(config('message.msg_schem_bd_006'));
					}elseif(count($listLog) == 0 && count($listSave) == 0) {
						$url .= '&err1=' . valueUrlEncode(config('message.msg_schem_bd_005'));
					}

					return $url;
				}else {
					//「ログ出力」(A5)=Onの合
					$url = url('/');
					$url .= '/' . $menuInfo->KindURL;
					$url .= '/' . $menuInfo->MenuURL;
					$url .= '/result';
					$url .= '?cmn1=' . $request->cmn1;
					$url .= '&cmn2=' . $request->cmn2;
					$url .= '&val1=' . valueUrlEncode($request->val1);
					$url .= '&val8=' . valueUrlEncode($request->val8);
					return $url;
				}
			}
		}

		/**
		* read data excel method
		* @param request 
		* @return array (data excel)
		*
		* @create 2020/12/02 Cuong
		* @update
		*/
		private function readDataFileExcel($request)
		{
			$file = $request->val6;
			$inputFileName = $_FILES['val6']['tmp_name'];
			try {
				/**  Identify the type of $inputFileName  **/
				$inputFileType = IOFactory::identify($inputFileName);
				/**  Create a new Reader of the type that has been identified  **/
				$reader = IOFactory::createReader($inputFileType);

				/**  Load $inputFileName to a Spreadsheet Object  **/
				$spreadsheet = $reader->load($inputFileName);
				$worksheet = $spreadsheet->getActiveSheet();

				// data read file from excel.
				$dataExcel = array();

				$beginRow = config('system_const_schem.bd_xl_begin_row');
				$highestRow = $worksheet->getHighestRow();
				$beginCol = config('system_const_schem.bd_xl_begin_col');
				$highestColumnIndex = config('system_const_schem.bd_xl_end_col');

				/* loop row excel */
				for ($row = $beginRow; $row <= $highestRow; ++$row) {
					$dataRowTemp = array();
					for ($col = $beginCol; $col <= $highestColumnIndex; ++$col) {
						$value = $worksheet->getCellByColumnAndRow($col, $row)->getValue();
						array_push($dataRowTemp, $value);
					}
					if(count($dataRowTemp) > 0) {
						array_push($dataExcel,$dataRowTemp);
					}
				}    

				return $dataExcel;

			} finally {
				// disconnect Worksheets
				$spreadsheet->disconnectWorksheets();
				$spreadsheet->garbageCollect();
			}
		}

		/**
		* read data excel method
		* @param menuInfo 
		* @param validated 
		* @return 
		*
		* @create 2020/12/02 Cuong
		* @update
		*/
		private function processDataExcel($dataExcel, $listBDCode, $request, &$listLog, &$listSave, &$listDelete, &$errMessage) {
			//validate form
			$validated = $request->validated();
			// Pattern A
			$arrA = Cyn_TosaiData::select(
				'Cyn_Plan.No AS A_No', 'Cyn_Plan.KoteiNo AS A_KoteiNo', 'Cyn_Plan.BD_Code AS A_BD_Code'
			)
			->selectRaw("(Cyn_TosaiData.Name+'_'+Cyn_TosaiData.BKumiku+'_'+Cyn_BlockKukaku.Name+'_'+Cyn_BlockKukaku.BKumiku+'_'+Cyn_Plan.Kotei+'_'+Cyn_Plan.KKumiku) as keyCheck")
			->join('Cyn_BlockKukaku', function($join) {
				$join->on('Cyn_TosaiData.ProjectID', '=', 'Cyn_BlockKukaku.ProjectID')
					->on('Cyn_TosaiData.OrderNo', '=', 'Cyn_BlockKukaku.OrderNo')
					->on('Cyn_TosaiData.CKind', '=', 'Cyn_BlockKukaku.CKind')
					->on('Cyn_TosaiData.Name', '=', 'Cyn_BlockKukaku.T_Name')
					->on('Cyn_TosaiData.BKumiku', '=', 'Cyn_BlockKukaku.T_BKumiku');
				})
			->join('Cyn_Plan', function($join) {
				$join->on('Cyn_BlockKukaku.ProjectID', '=', 'Cyn_Plan.ProjectID')
					->on('Cyn_BlockKukaku.OrderNo', '=', 'Cyn_Plan.OrderNo')
					->on('Cyn_BlockKukaku.No', '=', 'Cyn_Plan.No');
				})
			->where('Cyn_TosaiData.ProjectID', '=', $validated['val3'])
			->where('Cyn_TosaiData.OrderNo', '=', $validated['val4'])
			->where('Cyn_TosaiData.CKind', '=', $validated['val1'])
			->where('Cyn_TosaiData.WorkItemID', '!=', 0)
			->whereNull('Cyn_BlockKukaku.Del_Date')
			->whereNull('Cyn_Plan.Del_Date')
			->get()->keyBy('keyCheck')->toArray();

			// Pattern B
			$arrB = Cyn_TosaiData::select(
				'Cyn_C_Plan.No AS B_No', 'Cyn_C_Plan.KoteiNo AS B_KoteiNo', 'Cyn_C_Plan.BD_Code AS B_BD_Code'
			)
			->selectRaw("(Cyn_BlockKukaku.Name+'_'+Cyn_BlockKukaku.BKumiku+'_'+Cyn_C_BlockKukaku.Name+'_'+Cyn_C_BlockKukaku.BKumiku+'_'+Cyn_C_Plan.Kotei+'_'+Cyn_C_Plan.KKumiku) as keyCheck")
			->join('Cyn_BlockKukaku', function($join) {
				$join->on('Cyn_TosaiData.ProjectID', '=', 'Cyn_BlockKukaku.ProjectID')
					->on('Cyn_TosaiData.OrderNo', '=', 'Cyn_BlockKukaku.OrderNo')
					->on('Cyn_TosaiData.CKind', '=', 'Cyn_BlockKukaku.CKind')
					->on('Cyn_TosaiData.Name', '=', 'Cyn_BlockKukaku.T_Name')
					->on('Cyn_TosaiData.BKumiku', '=', 'Cyn_BlockKukaku.T_BKumiku');
				})
			->join('Cyn_C_BlockKukaku', function($join) {
				$join->on('Cyn_C_BlockKukaku.ProjectID', '=', 'Cyn_BlockKukaku.ProjectID')
					->on('Cyn_C_BlockKukaku.OrderNo', '=', 'Cyn_BlockKukaku.OrderNo')
					->on('Cyn_C_BlockKukaku.CKind', '=', 'Cyn_BlockKukaku.CKind')
					->on('Cyn_C_BlockKukaku.T_Name', '=', 'Cyn_BlockKukaku.Name')
					->on('Cyn_C_BlockKukaku.T_BKumiku', '=', 'Cyn_BlockKukaku.BKumiku');
				})
			->join('Cyn_C_Plan', function($join) {
				$join->on('Cyn_C_BlockKukaku.ProjectID', '=', 'Cyn_C_Plan.ProjectID')
					->on('Cyn_C_BlockKukaku.OrderNo', '=', 'Cyn_C_Plan.OrderNo')
					->on('Cyn_C_BlockKukaku.No', '=', 'Cyn_C_Plan.No');
				})
			->where('Cyn_TosaiData.ProjectID', '=', $validated['val3'])
			->where('Cyn_TosaiData.OrderNo', '=', $validated['val4'])
			->where('Cyn_TosaiData.WorkItemID', '=', 0)
			->where('Cyn_TosaiData.CKind', '=', $validated['val1'])

			->whereNull('Cyn_BlockKukaku.Del_Date')
			->whereNull('Cyn_C_BlockKukaku.Del_Date')
			->whereNull('Cyn_C_Plan.Del_Date')
			->get()->keyBy('keyCheck')->toArray();
				
			
			/* loop data excel */
			foreach($dataExcel as $data) {
				if (!is_null($data[0]) && !is_null($data[1]) && !is_null($data[2]) && !is_null($data[3]) && 
				!is_null($data[4]) && !is_null($data[5])) {
					
					$T_Name = $data[0];
					$T_BKumiku = FuncCommon::getSplitChar($data[1])[0];
					$Name = $data[2];
					$BKumiku = FuncCommon::getSplitChar($data[3])[0];
					$Kotei = FuncCommon::getSplitChar($data[4])[0];
					$KKumiku = FuncCommon::getSplitChar($data[5])[0];
					$keyCheck =  $T_Name.'_'.$T_BKumiku.'_'.$Name.'_'.$BKumiku.'_'.$Kotei.'_'.$KKumiku;
					// Pattern A
					if(isset($arrA[$keyCheck])) {
						$dataA = $arrA[$keyCheck];
					}else { 
						$dataA = array(); 
					}
					
					// Pattern B
					if(isset($arrB[$keyCheck])) {
						$dataB = $arrB[$keyCheck];
					}else { 
						$dataB = array(); 
					}

					/* data A and data B is null */
					if(count($dataA) == 0 && count($dataB) == 0) {
						// (A5)=On
						if ($request->val5 == 1) {
							$listLogTemp = array();
							$listLogTemp['T_Name'] = $T_Name;
							$listLogTemp['T_BKumiku'] = $T_BKumiku;
							$listLogTemp['Name'] = $Name;
							$listLogTemp['BKumiku'] = $BKumiku;
							$listLogTemp['Kotei'] = $Kotei;
							$listLogTemp['KKumiku'] = $KKumiku;
							$listLogTemp['Log'] = config('message.msg_schem_bd_001');
							array_push($listLog, $listLogTemp);
						}
					}else {
						$indexBegin = config('system_const_schem.bd_xl_bd_begin_col') - 1;
						$indexEnd = config('system_const_schem.bd_xl_end_col');
						$indexLoopCol = 1;

						for ($i = $indexBegin; $i < $indexEnd; $i += 2) { 
							if (!is_null($data[$i])) {
								$message = '';
								$isErr = false;
								$bdCode = FuncCommon::getSplitChar($data[$i])[0];
								$bd = $data[$i + 1];
								
								/* check validate value of cell BDCode */
								if(is_null($bdCode) || mb_strlen($bdCode) >= 6) {
									$message =  sprintf(config('message.msg_schem_bd_002'),$indexLoopCol);
									$isErr = true;
								}elseif(count($listBDCode) > 0) {
									$hasRecode = array_filter($listBDCode, function($item) use($bdCode){
										return $item['Code'] == $bdCode;
									});

									if(count($hasRecode) == 0) {
										$message = sprintf(config('message.msg_schem_bd_002'),$indexLoopCol);
										$isErr = true;
									}
								}

								/* check validate value of cell BData */
								if(!$isErr) {
									if(is_null($bd) || $bd < 0) {
										$message = sprintf(config('message.msg_schem_bd_003'),$indexLoopCol);
									}else{
										preg_match('/^(\d{1,6}(?:[\.\,]\d{1,2})?)$/', $bd, $matches);
										if(count($matches) == 0) { 
											$message = sprintf(config('message.msg_schem_bd_003'),$indexLoopCol);
										}
									}
								}
								
								if($message != '') {
									// 「ログ出力」(A5)=On
									if($request->val5 == 1) {
										$listLogTemp = array();
										$listLogTemp['T_Name'] = $T_Name;
										$listLogTemp['T_BKumiku'] = $T_BKumiku;
										$listLogTemp['Name'] = $Name;
										$listLogTemp['BKumiku'] = $BKumiku;
										$listLogTemp['Kotei'] = $Kotei;
										$listLogTemp['KKumiku'] = $KKumiku;
										$listLogTemp['Log'] = $message;
										array_push($listLog, $listLogTemp);
									}
								}else {
									$arraySaveTemp = array();
									$arraySaveTemp['T_Name'] = $T_Name;
									$arraySaveTemp['T_BKumiku'] = $T_BKumiku;
									$arraySaveTemp['Name'] = $Name;
									$arraySaveTemp['BKumiku'] = $BKumiku;
									$arraySaveTemp['Kotei'] = $Kotei;
									$arraySaveTemp['KKumiku'] = $KKumiku;
									$arraySaveTemp['ID'] = $indexLoopCol;
									$arraySaveTemp['BD_Code'] = $bdCode;
									$arraySaveTemp['BData'] = $bd;
									$arraySaveTemp['A_No'] = count($dataA) == 0 ? null : $dataA['A_No'];
									$arraySaveTemp['A_KoteiNo'] = count($dataA) == 0 ? null : $dataA['A_KoteiNo'];
									$arraySaveTemp['A_BD_Code'] = count($dataA) == 0 ? null : $dataA['A_BD_Code'];
									$arraySaveTemp['B_No'] = count($dataB) == 0 ? null : $dataB['B_No'];
									$arraySaveTemp['B_KoteiNo'] = count($dataB) == 0 ? null : $dataB['B_KoteiNo'];
									$arraySaveTemp['B_BD_Code'] = count($dataB) == 0 ? null : $dataB['B_BD_Code'];
									array_push($listSave, $arraySaveTemp);
								}
							}
							else {
								// 物量コード列に値が入っていない場合のみ
								$arrDelTemp = array();
								$arrDelTemp['T_Name'] = $T_Name;
								$arrDelTemp['T_BKumiku'] = $T_BKumiku;
								$arrDelTemp['Name'] = $Name;
								$arrDelTemp['BKumiku'] = $BKumiku;
								$arrDelTemp['Kotei'] = $Kotei;
								$arrDelTemp['KKumiku'] = $KKumiku;
								$arrDelTemp['ID'] = $indexLoopCol;
								array_push($listDelete, $arrDelTemp);
							}
							$indexLoopCol ++;
						}
					}
				}
			}
		}

		/**
		* get data value 3
		*
		* @param String $val3
		* @return Object mixed
		*
		* @create 2020/12/02 Cuong
		* @update
		*/
		private function getDataVal3($menuInfo, $value = '', $loadAll = false) {
			$data = MstProject::select('ID as val3', 'ProjectName as val3Name', 'ListKind')
								->where('SysKindID', '=', $menuInfo->KindID);
			$data = ($value !== '') ? $data->where('ListKind', '=', $value) : $data;
			$data = $data->orderBy('ProjectName')->get();

			if (count($data) > 0) {
				foreach ($data as &$row) {
					$row->val3 = valueUrlEncode($row->val3);
					$row->ListKind = valueUrlEncode($row->ListKind);
					$row->val3Name = ($loadAll) ? htmlentities($row->val3Name) : $row->val3Name;
				}
			}
			return $data;
		}

		/**
		* get data value 4
		*
		* @param String $val4
		* @return Object mixed
		*
		* @create 2020/12/02 Cuong
		* @update
		*/
		private function getDataVal4($val1 = 0, $val3 = '', $loadAll = false) {
			$data = mstOrderNo::select('mstOrderNo.OrderNo as val4', 'Cyn_TosaiData.CKind', 'Cyn_TosaiData.ProjectID')
									->join('Cyn_TosaiData', 'mstOrderNo.OrderNo', '=', 'Cyn_TosaiData.OrderNo');
			$data = ($val1 !== '') ? $data->where('Cyn_TosaiData.CKind', '=', $val1) : $data;
			$data = ($val3 !== '') ? $data->where('Cyn_TosaiData.ProjectID', '=', $val3) : $data;
			$data = $data->where('mstOrderNo.DispFlag', '=', 0)->orderBy('mstOrderNo.OrderNo')->distinct()->get();

			if (count($data) > 0) {
				foreach ($data as &$row) {
					$row->ProjectID = valueUrlEncode($row->ProjectID);
					$row->CKind = valueUrlEncode($row->CKind);
					$row->val4Name = ($loadAll) ? htmlentities($row->val4) : $row->val4;
					$row->val4 = valueUrlEncode($row->val4);
				}
			}

			return $data;
		}

		/**
		* parse to float
		*
		* @param mix num
		* @return 
		*
		* @create 2020/12/02 Cuong
		* @update
		*/
		private function toFloat($num) {
			$dotPos = strrpos($num, '.');
			$commaPos = strrpos($num, ',');
			$sep = (($dotPos > $commaPos) && $dotPos) ? $dotPos : 
				((($commaPos > $dotPos) && $commaPos) ? $commaPos : false);
			if (!$sep) {
				return floatval(preg_replace("/[^0-9]/", "", $num));
			} 

			return floatval(
				preg_replace("/[^0-9]/", "", substr($num, 0, $sep)) . '.' .
				preg_replace("/[^0-9]/", "", substr($num, $sep+1, strlen($num)))
			);
		}
	}
