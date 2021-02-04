<?php
/*
 * @MakeNitteiController.php
 * 工程定義管理画面コントローラーファイル
 *
 * @create 2020/12/30 Cuong
 *
 * @update
 */
namespace App\Http\Controllers\Sches;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Models\MstProject;
use App\Models\MstOrderNo;
use App\Models\S_JobData;
use App\Models\MstFloor;
use App\Models\MstDist;
use App\Models\MstBDCode;
use App\Models\MstMac;
use App\Models\S_History_Nittei;
use App\Models\S_Temp_LogData_Nittei;
use App\Models\S_LogData_Nittei;
use App\Http\Requests\Sches\MakeNitteiContentsRequest;
use App\Librarys\FuncCommon;
use App\Librarys\TimeTrackerCommon;
use App\Librarys\TimeTrackerFuncSches;
use PhpOffice\PhpSpreadsheet\IOFactory;
use PhpOffice\PhpSpreadsheet\Style\Border;
use PhpOffice\PhpSpreadsheet\Style\Font;
use PhpOffice\PhpSpreadsheet\Style\Fill;
use PhpOffice\PhpSpreadsheet\Shared\Date;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use PhpOffice\PhpSpreadsheet\Style\Style;
use PhpOffice\PhpSpreadsheet\Cell\Coordinate;
use PhpOffice\PhpSpreadsheet\Style\Protection;
use PhpOffice\PhpSpreadsheet\Cell\Cell;
use PhpOffice\PhpSpreadsheet\Exception as PhpSpreadsheetException;
use DB;
use Carbon\Carbon;
use Exception;
/*
 * MakeNitteiController class
 *
 * @create 2020/12/30 Cuong
 *
 * @update
 */
class MakeNitteiController extends Controller
{
	/**
	 * 日程取込画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/12/30 Cuong
	 * @update
	 */
	public function index(Request $request) {
		return $this->initialize($request);
	}

	/**
	 * init & prepare data to show
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/12/30 Cuong
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
						config('system_const_sches.nt_val_export')),
			'val2' => isset($request->val2) ? valueUrlDecode($request->val2) :
						((trim(old('val2')) != '') ? valueUrlDecode(old('val2')) : ''),
			'val3' => isset($request->val3) ? valueUrlDecode($request->val3) :
						((trim(old('val3')) != '') ? valueUrlDecode(old('val3')) : ''),
			'val4' => isset($request->val4) ? valueUrlDecode($request->val4) : 
						(trim(old('val4') != '') ? old('val4') : 1),
			'val5' => isset($request->val5) ? valueUrlDecode($request->val5) : 
						(trim(old('val5') != '') ? old('val5') : ''),
			'val6' => isset($request->val6) ? valueUrlDecode($request->val6) : 
						(trim(old('val6') != '') ? old('val6') : config('system_const.displayed_results_1')),
		);
		
		//get data val2
		$data_val2 = $this->getDataVal2($menuInfo);
		$this->data['dataView']['data_2_all'] = $data_val2;

		$tempVal3 = ($itemShow['val2'] == '') ?
			((count($data_val2) > 0) ? valueUrlDecode($data_val2->first()->val2) : $itemShow['val2']) : $itemShow['val2'];
		
		$data_val3 = $this->getDataVal3($tempVal3);
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

		// data 3 for val 3
		$this->data['dataView']['data_3'] = $data_val3;
		$this->data['dataView']['data_3_all'] = $this->getDataVal3('', true);
		
		$itemShow['val1'] = valueUrlEncode($itemShow['val1']);
		$itemShow['val2'] = valueUrlEncode($itemShow['val2']);
		$itemShow['val3'] = valueUrlEncode($itemShow['val3']);
		$itemShow['val4'] = valueUrlEncode($itemShow['val4']);
		$itemShow['val5'] = valueUrlEncode($itemShow['val5']);
		$itemShow['val6'] = valueUrlEncode($itemShow['val6']);

		//request
		$this->data['menuInfo'] = $menuInfo;
		$this->data['request'] = $request;
		$this->data['originalError'] = $originalError;
		$this->data['itemShow'] = $itemShow;
		$this->data['msgTimeOut'] = valueUrlEncode(config('message.msg_cmn_err_002'));
		//return view with all data
		return view('Sches/makenittei/index', $this->data);
	}

	/**
	 * export excel and inport data  method
	 * @param Request BDataContentsRequest
	 * @return
	 *
	 * @create 2020/12/02 Cuong
	 * @update
	 */
	public function execute(MakeNitteiContentsRequest  $request) {
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));
		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		
		if($request->val1 == config("system_const_schem.bd_val_export")) {
			/* process export */
			$resExport = $this->export($menuInfo, $request);
			if($resExport != '') {
				return redirect($resExport);
			}
		}else {
			/* process import */
			$res = $this->import($menuInfo, $request);
			return redirect($res);
		}
	}

	/**
	* get data value 2
	*
	* @param menuInfo
	* @return Object mixed
	*
	* @create 2020/12/30 Cuong
	* @update
	*/
	private function getDataVal2($menuInfo) {
		$data = MstProject::select('ID as val2', 'ProjectName as val2Name', 'ListKind')
							->where('SysKindID', '=', $menuInfo->KindID)->orderBy('ProjectName')->get();
		if (count($data) > 0) {
			foreach ($data as &$row) {
				$row->val2 = valueUrlEncode($row->val2);
				$row->ListKind = valueUrlEncode($row->ListKind);
				$row->val2Name = htmlentities($row->val2Name);
			}
		}
		return $data;
	}

	/**
	* get data value 3
	*
	* @param String $val2
	* @return Object mixed
	*
	* @create 2020/12/30 Cuong
	* @update
	*/
	private function getDataVal3($val2 = '', $loadAll = false) {
		$data = mstOrderNo::select('mstOrderNo.OrderNo as val3', 'S_JobData.ProjectID')
								->join('S_JobData', 'mstOrderNo.OrderNo', '=', 'S_JobData.OrderNo');
								
		$data = ($val2 !== '' && is_numeric($val2)) ? $data->where('S_JobData.ProjectID', '=', $val2) : $data;
		$data = $data->where('mstOrderNo.DispFlag', '=', 0)->orderBy('mstOrderNo.OrderNo')->distinct()->get();

		if (count($data) > 0) {
			foreach ($data as &$row) {
				$row->ProjectID = valueUrlEncode($row->ProjectID);
				$row->val3Name = ($loadAll) ? htmlentities($row->val3) : $row->val3;
				$row->val3 = valueUrlEncode($row->val3);
			}
		}
	
		return ($val2 !== '' && is_numeric($val2) || $loadAll) ? $data : array();
	}

	/**
	* export excel method
	* @param menuInfo 
	* @param request 
	* @return mixed
	*
	* @create 2020/12/30 Cuong
	* @update
	*/
	private function export($menuInfo, $request) {
		$timeNXCommon = new TimeTrackerCommon;	//object TimeTrackerCommon
		$projectID = $request->val2;			//projectID
		$order = $request->val3;				//order
		$val4 = $request->val4;					//データ区分
		$url = '';								//url

		$projectCalendar = $timeNXCommon->getCalendar($projectID);

		if(is_string($projectCalendar)) {
			// 排他ロック解除処理
			$this->deleteLock ($menuInfo->KindID, config('system_const_sches.syslock_menuid_sches'), 
							$menuInfo->SessionID, valueUrlDecode($request->val1));
			// 戻り値が文字列の場合
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
			$url .= '&err1=' . valueUrlEncode($projectCalendar);
			return $url;
		}

		// read file excel template
		$inputFileType = 'Xlsx';
		$inputFileName = config('system_const_sches.export_template_path');
		if ($inputFileName != '') {
			$arrPath = explode('/', $inputFileName);
			$arrLength = count($arrPath);

			if ($arrLength > 2) {
				$inputFileName = public_path() . '\\' . $arrPath[$arrLength - 2] . '\\' . $arrPath[$arrLength - 1];
				// setting header
				$reader = IOFactory::createReader($inputFileType);
				$spreadsheet = $reader->load($inputFileName);
				// process sheet master
				$dataMaster = array();
				$this->processSheetMaster($spreadsheet, $dataMaster);
				// process sheet nittei
				$this->processSheetNittei($spreadsheet, $dataMaster, $projectID, $order, $val4);

				$strFileName = $menuInfo->MenuNick;
				header("Content-Type: application/force-download");
				header("Content-Type: application/octet-stream");
				header("Content-Type: application/download");
				header('Content-Type:application/octet-stream; charset=Shift_JIS');
				header('Content-Type: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet');
				header('Content-Disposition: attachment; filename="' . $strFileName . '.xlsx"');
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
	* set cells values
	* @param array data 
	* @param worksheet 
	* @param int col 
	* @param int firstRow 
	* @param int endRow
	* @param int endRow
	*
	* @create 2020/12/30 Cuong
	* @update
	*/
	private function setCellValues($data, $worksheet, $col, $firstRow, $endRow, $tempRow) {
		if(count($data) > 0 && $endRow < $tempRow) {	//count row data < row template
			// style border
			$this->setBorderStyle($worksheet, $col . ($firstRow + 1), 'bottom', Border::BORDER_HAIR);
			// copy style cell to cells
			$worksheet->duplicateStyle($worksheet->getStyle($col . ($firstRow + 1)), $col . $firstRow . ':' . $col . $endRow);
			// style border
			$this->setBorderStyle($worksheet, $col . $endRow, 'bottom', Border::BORDER_THIN);

			// style border
			$this->setBorderStyle($worksheet, $col . $tempRow, 'outline', Border::BORDER_NONE);
		}
		if($endRow > $tempRow) {	//count row data > row template
			// style border
			$this->setBorderStyle($worksheet, $col . ($firstRow + 1), 'bottom', Border::BORDER_HAIR);
			// copy style cell to cells
			$worksheet->duplicateStyle($worksheet->getStyle($col . ($firstRow + 1)), $col . $firstRow . ':' . $col . $endRow);
			// style border
			$this->setBorderStyle($worksheet, $col . $endRow, 'bottom', Border::BORDER_THIN);
		}
		// export data 
		$worksheet->fromArray($data, NULL,$col . $firstRow);
	}

	/**
	* set style border cell method
	* @param worksheet
	* @param cell
	* @param position
	* @param borderStyle
	*
	* @create 2020/12/02 Cuong
	* @update
	*/
	private function setBorderStyle($worksheet, $cell, $position, $borderStyle) {
		$worksheet->getStyle($cell)->applyFromArray([
			'borders' => [
				$position => [
					'borderStyle' => $borderStyle,
					'color' => ['argb' => '000000'],
				],
			],
		]);
	}

	/**
	* process data export sheet master method
	* @param worksheet 
	* @param array &$dataMaster
	*
	* @create 2020/12/02 Cuong
	* @update
	*/
	private function processSheetMaster($spreadsheet, &$dataMaster) {
		// get sheet Master
		$worksheet = $spreadsheet->getSheetByName(config('system_const_sches.sches_mastersheet_name'));
		// array data column A
		$dataColA = [ 
						[FuncCommon::getKumikuData(config('system_const.kumiku_code_kogumi'))[2]],
						[FuncCommon::getKumikuData(config('system_const.kumiku_code_naicyu'))[2]],
						[FuncCommon::getKumikuData(config('system_const.kumiku_code_kumicyu'))[2]],
						[FuncCommon::getKumikuData(config('system_const.kumiku_code_ogumi'))[2]],
						[FuncCommon::getKumikuData(config('system_const.kumiku_code_sogumi'))[2]],
						[FuncCommon::getKumikuData(config('system_const.kumiku_code_kyocyu'))[2]],
					];
		
		$firstRow = config('system_const_sches.nt_xl_begin_row');	//begin row
		$tempRow = config('system_const_sches.nt_xl_initial_rows') + 1;
		// set value cells 
		$col = 'A';
		$endRow = count($dataColA) + 1;
		$this->setCellValues($dataColA, $worksheet, $col, $firstRow, $endRow, $tempRow);
		$dataMaster['colA'] = $dataColA;

		// array data column B
		$mstFloors = MstFloor::select('Code','Name')->where('ViewFlag','=',1)->orderBy('SortNo')->get();
		$dataColB = [];
		if(count($mstFloors) > 0) {
			foreach($mstFloors as $itemFloor) {
				array_push($dataColB,[$itemFloor->Code.config('system_const.code_name_separator').$itemFloor->Name]);
			};
		}
		// set value cells 
		$col = 'B';
		$endRow = count($dataColB) + 1;
		$this->setCellValues($dataColB, $worksheet, $col, $firstRow, $endRow, $tempRow);
		$dataMaster['colB'] = $dataColB;

		// array data column C
		$mstMacs = MstMac::select('Name')->selectRaw("LTRIM(RTRIM(Code)) as Code")
							->orderBy('Code')->orderBy('Name')->get();
		$dataColC = [];
		if(count($mstMacs) > 0) {
			foreach($mstMacs as $itemMac) {
				array_push($dataColC,[$itemMac->Code.config('system_const.code_name_separator').$itemMac->Name]);
			};
		}
		// set value cells 
		$col = 'C';
		$endRow = count($dataColC) + 1;
		$this->setCellValues($dataColC, $worksheet, $col, $firstRow, $endRow, $tempRow);
		$dataMaster['colC'] = $dataColC;

		// array data column D
		$mstDists = MstDist::select('Name')->selectRaw("LTRIM(RTRIM(Code)) as Code")
							->orderBy('Code')->orderBy('Name')->get();
		$dataColD = [];
		if(count($mstDists) > 0) {
			foreach($mstDists as $itemDist) {
				array_push($dataColD,[$itemDist->Code.config('system_const.code_name_separator').$itemDist->Name]);
			};
		}
		// set value cells 
		$col = 'D';
		$endRow = count($dataColD) + 1;
		$this->setCellValues($dataColD, $worksheet, $col, $firstRow, $endRow, $tempRow);
		$dataMaster['colD'] = $dataColD;

		// array data column E
		$mstBDCode = MstBDCode::select('Code','Name')->where('ViewFlag','=',1)
								->orderBy('Code')->orderBy('Name')->get();
		$dataColE = [];
		if(count($mstBDCode) > 0) {
			foreach($mstBDCode as $itemBD) {
				array_push($dataColE,[$itemBD->Code.config('system_const.code_name_separator').$itemBD->Name]);
			};
		}
		// set value cells 
		$col = 'E';
		$endRow = count($dataColE) + 1;
		$this->setCellValues($dataColE, $worksheet, $col, $firstRow, $endRow, $tempRow);
		$dataMaster['colE'] = $dataColE;

		// array data column F
		$dataColF = [ 
			[
				config('system_const_sches.keshicode_code_hr').
				config('system_const.code_name_separator').
				config('system_const_sches.keshicode_name_hr')
			],
			[
				config('system_const_sches.keshicode_code_bdata').
				config('system_const.code_name_separator').
				config('system_const_sches.keshicode_name_bdata')
			]
		];
		// set value cells 
		$col = 'F';
		$endRow = count($dataColF) + 1;
		$this->setCellValues($dataColF, $worksheet, $col, $firstRow, $endRow, $tempRow);
		$dataMaster['colF'] = $dataColF;

		// array data column G
		$dataColG = [ 
			[
				config('system_const_sches.keshipattern_code_keshikomi').
				config('system_const.code_name_separator').
				config('system_const_sches.keshipattern_name_keshikomi')
			],
			[
				config('system_const_sches.keshipattern_code_shintyoku').
				config('system_const.code_name_separator').
				config('system_const_sches.keshipattern_name_shintyoku')
			]
		];
		// set value cells 
		$col = 'G';
		$endRow = count($dataColG) + 1;
		$this->setCellValues($dataColG, $worksheet, $col, $firstRow, $endRow, $tempRow);
		$dataMaster['colG'] = $dataColG;
	}

	/**
	* process data export sheet Nittei method
	* @param worksheet 
	* @param array dataMaster
	* @param projectID
	* @param order
	* @param val4
	*
	* @create 2020/12/02 Cuong
	* @update
	*/
	private function processSheetNittei($spreadsheet, $dataMaster, $projectID, $order, $val4) {
		// TimeTrackerNXから工期情報を取得
		$query = S_JobData::select('WorkItemID')
								->where('S_JobData.ProjectID', '=', $projectID)
								->where('S_JobData.OrderNo', '=', $order)
								->where('S_JobData.Level_Job', '=', 3);
		if($val4 == 0) {
			$query->where('S_JobData.IsOriginal', '=', 0);
		}else {
			$query->where('S_JobData.IsOriginal', '=', 1);
		}

		$dataSJobData = $query->get()->toArray();

		if(count($dataSJobData) > 0) {
			$lstWorkItemID = array_column($dataSJobData, 'WorkItemID');
			$timeTrackerFuncSchem = new TimeTrackerCommon;
			$scheduleInformation = $timeTrackerFuncSchem->getKoteiRange($lstWorkItemID, false);
		}

		// get sheet Nittei
		$worksheet = $spreadsheet->getSheetByName(config('system_const_sches.sches_nitteisheet_name'));
		$queryDataNittei = S_JobData::select('S_JobData.WorkItemID','S_JobData.DispName1','S_JobData.DispName2'
							,'S_JobData.DispName3','S_JobData.KakoKumiku','S_JobData.AcFloor'
							,'S_JobData.BD_Code','S_JobData.MngBData','S_JobData.PlStdHr'
							,'S_JobData.EpHr','S_JobData.HS','S_JobData.HK','S_JobData.WBSCode'
							,'S_JobData.KeshiPattern','S_JobData.KeshiCode'
							,'MstFloor.Name  as FloorName', 'MstMac.Name  as MacName',
							'MstDist.Name  as DistName','MstBDCode.Name  as BDName')
							->selectRaw("LTRIM(RTRIM(S_JobData.Item)) as Item")
							->selectRaw("LTRIM(RTRIM(S_JobData.AcMac)) as AcMac")
							->selectRaw("LTRIM(RTRIM(S_JobData.DistCode)) as DistCode")
							->leftJoin('MstFloor ', function($join) {
								$join->on('S_JobData.AcFloor', '=', 'MstFloor.Code'); 
							})
							->leftJoin('MstMac', function($join) {
								$join->on('S_JobData .AcMac', '=', 'MstMac.Code'); 
							})
							->leftJoin('MstDist', function($join) {
								$join->on('S_JobData .DistCode', '=', 'MstDist.Code'); 
							})
							->leftJoin('MstBDCode', function($join) {
								$join->on('S_JobData .BD_Code', '=', 'MstBDCode.Code'); 
							})
							->where('S_JobData.ProjectID', '=', $projectID)
							->where('S_JobData.OrderNo', '=', $order)
							->where('S_JobData.Level_Job', '=', 3);
		
		if($val4 == 0) {
			$queryDataNittei->where('S_JobData.IsOriginal', '=', 0);
		}else {
			$queryDataNittei->where('S_JobData.IsOriginal', '=', 1);
		}
		$dataNittei = $queryDataNittei->orderBy('DispName1', 'asc')
									->orderBy('DispName2', 'asc')
									->orderBy('DispName3', 'asc')
									->get()->toArray();

		$dataNitteiExport = array();
		if(count($dataNittei) > 0) {
			foreach($dataNittei as $value) {
				$itemNittei = array();
				// colA
				$itemNittei[0] = NULL;
				// colB
				$itemNittei[1] = $value['DispName1'];
				// colC
				$itemNittei[2] = $value['DispName2'];
				// colD
				$itemNittei[3] = $value['DispName3'];
				// colE
				$kumikuData = FuncCommon::getKumikuData($value['KakoKumiku']);
				$itemNittei[4] = NULL;
				if(count($kumikuData) > 0) {
					if(in_array(array($kumikuData[2]), $dataMaster['colA'])) {
						$itemNittei[4] = $kumikuData[2];
					}
				}
				// colF
				$itemNittei[5] = NULL;
				$valueCol5 = $value['AcFloor'].config('system_const.code_name_separator').$value['FloorName'];
				if(in_array(array($valueCol5), $dataMaster['colB'])) {
					$itemNittei[5] = $valueCol5;
				}
				// colG
				$itemNittei[6] = NULL;
				$valueCol6 = $value['AcMac'].config('system_const.code_name_separator').$value['MacName'];
				if(in_array(array($valueCol6), $dataMaster['colC'])) {
					$itemNittei[6] = $valueCol6;
				}
				// colH
				$itemNittei[7] = NULL;
				$valueCol7 = $value['DistCode'].config('system_const.code_name_separator').$value['DistName'];
				if(in_array(array($valueCol7), $dataMaster['colD'])) {
					$itemNittei[7] = $valueCol7;
				}
				// colI
				$itemNittei[8] = NULL;
				$valueCol8 = $value['BD_Code'].config('system_const.code_name_separator').$value['BDName'];
				if(in_array(array($valueCol8), $dataMaster['colE'])) {
					$itemNittei[8] = $valueCol8;
				}
				// colJ
				$itemNittei[9] = NULL;
				$valueCol9 = NULL;
				if($value['KeshiCode'] == config('system_const_sches.keshicode_code_hr')) {
					$valueCol9 = config('system_const_sches.keshicode_code_hr')
								.config('system_const.code_name_separator')
								.config('system_const_sches.keshicode_name_hr');
				}
				if($value['KeshiCode'] == config('system_const_sches.keshicode_code_bdata')) {
					$valueCol9 = config('system_const_sches.keshicode_code_bdata')
								.config('system_const.code_name_separator')
								.config('system_const_sches.keshicode_name_bdata');
				}
				if(in_array(array($valueCol9), $dataMaster['colF'])) {
					$itemNittei[9] = $valueCol9;
				}
				// colK
				$itemNittei[10] = NULL;
				if($value['KeshiPattern'] == config('system_const_sches.keshipattern_code_keshikomi')) {
					$valueCol10 = config('system_const_sches.keshipattern_code_keshikomi')
								.config('system_const.code_name_separator')
								.config('system_const_sches.keshipattern_name_keshikomi');
				}
				if($value['KeshiPattern'] == config('system_const_sches.keshipattern_code_shintyoku')) {
					$valueCol10 = config('system_const_sches.keshipattern_code_shintyoku')
								.config('system_const.code_name_separator')
								.config('system_const_sches.keshipattern_name_shintyoku');
				}
				if(in_array(array($valueCol10), $dataMaster['colG'])) {
					$itemNittei[10] = $valueCol10;
				}
				// colL
				$itemNittei[11] = $value['MngBData'];
				// colM
				$workItemID = $value['WorkItemID'];
				$itemNittei[12] = $scheduleInformation[$workItemID]['plannedStartDate'];
				// colN
				$workItemID = $value['WorkItemID'];
				$itemNittei[13] = $scheduleInformation[$workItemID]['plannedFinishDate'];
				// colO
				$itemNittei[14] =  $value['Item'];
				// colP
				$itemNittei[15] =  $value['PlStdHr'];
				// colQ
				$itemNittei[16] =  $value['EpHr'];
				// colR
				$itemNittei[17] =  $value['HS'];
				// colS
				$itemNittei[18] =  $value['HK'];
				// colT
				$itemNittei[19] =  $value['WBSCode'];
				
				array_push($dataNitteiExport, $itemNittei);
			}
			// export data
			$beginCol = config('system_const_sches.nt_xl_begin_col');
			$beginRow = config('system_const_sches.nt_xl_begin_row');

			$maxRowTemp = config('system_const_sches.nt_xl_nittei_initial_rows') + 1;
			$maxRowData = count($dataNittei) + 1;
			// update remove colunm style temp when data record < config('system_const_schem.bd_xl_initial_rows')
			if($maxRowData < $maxRowTemp) {
				$worksheet->removeRow($beginRow, $maxRowTemp - $maxRowData);
			}
			// add colunm style temp when data record > config('system_const_schem.bd_xl_initial_rows')
			if($maxRowData > $maxRowTemp) {
				$worksheet->insertNewRowBefore($maxRowTemp, $maxRowData - $maxRowTemp);
			}
			$worksheet->fromArray($dataNitteiExport, NULL, $beginCol . $beginRow);

			// シートの保護を行う
			if($val4 == 0) {
				$worksheet->getProtection()->setSheet(true);
				$spreadsheet->getDefaultStyle()->getProtection()->setLocked(false);
				$maxRow = count($dataNittei) == 0 ? $maxRowTemp : $maxRowData;
				$worksheet->getStyle('L2:T'.$maxRow)
						->getProtection()->setLocked(\PhpOffice\PhpSpreadsheet\Style\Protection::PROTECTION_UNPROTECTED);
				
				$arrColProtection = array('A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K');
				foreach($arrColProtection as $colName) {
					$worksheet->getStyle($colName)
							->getProtection()->setLocked(\PhpOffice\PhpSpreadsheet\Style\Protection::PROTECTION_PROTECTED);
				}
			}
		}

	}

	/**
	* import excel method
	* @param menuInfo 
	* @param request 
	* @return string
	*
	* @create 2020/12/30 Cuong
	* @update
	*/
	private function import($menuInfo, $request) {
		// 排他ロックを行う
		$menuID = config('system_const_sches.syslock_menuid_sches');
		$this->tryLock ($menuInfo->KindID, $menuID, $menuInfo->UserID, $menuInfo->SessionID, $request->val1, true);

		// 検討ケースの実働期間を取得する
		$projectID = $request->val2;				//projectID
		$timeTrackerCommon = new TimeTrackerCommon;	//object TimeTrackerCommon
		$projectCalendar = $timeTrackerCommon->getCalendar($projectID);

		if(is_string($projectCalendar)) {
			// 排他ロック解除処理
			$this->deleteLock ($menuInfo->KindID, config('system_const_sches.syslock_menuid_sches'), 
							$menuInfo->SessionID, valueUrlDecode($request->val1));
			// 戻り値が文字列の場合
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
			$urlErr .= '&err1=' . valueUrlEncode($projectCalendar);
			return $urlErr;
		}

		// read data file excel
		$dataExcel = $this->readDataFileExcel($request, $menuInfo);
		if(is_string($dataExcel)) {
			return $dataExcel;
		}

		/* エクセルファイルの内容確認 */
		$this->checkContentsExcel($dataExcel, $request);

		/* 2.	初期表示 */
		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/create';
		$url .= '?cmn1=' . $request->cmn1;
		$url .= '&cmn2=' . $request->cmn2;
		$url .= '&val1=' . valueUrlEncode($request->val1);
		$url .= '&val2=' . valueUrlEncode($request->val2);
		$url .= '&val3=' . valueUrlEncode($request->val3);
		$url .= '&val4=' . valueUrlEncode($request->val4);
		$url .= '&val5=' . valueUrlEncode($request->val5);
		$url .= '&val6=' . valueUrlEncode($request->val6);
		return $url;
	}

	/**
	 * display data method
	 * @param Request 
	 * @return 
	 *
	 * @create 2021/01/14 Cuong
	 * @update
	 */
	public function create(Request $request) {
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));
		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		//error
		$originalError = array();
		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
		}
		$projectID = valueUrlDecode($request->val2);			//projectID
		$order = valueUrlDecode($request->val3);				//orderno

		if(isset($request->val6) && in_array(valueUrlDecode($request->val6), [config('system_const.displayed_results_1'),
																config('system_const.displayed_results_2'),
																config('system_const.displayed_results_3')])){
			$pageunit = valueUrlDecode($request->val6);
		}else{
			$pageunit = config('system_const.displayed_results_1');
		}

		$sort = ['ID'];
		if(isset($request->sort) && $request->sort != ''){
			$sort = [$request->sort, 'ID'];
		}

		$direction = (isset($request->direction) && $request->direction != '') ?  $request->direction : 'asc';

		$query = S_Temp_LogData_Nittei::select('S_Temp_LogData_Nittei.ID', 'S_Temp_LogData_Nittei.DispName1 as fld1'
						,'S_Temp_LogData_Nittei.DispName2 as fld2', 'S_Temp_LogData_Nittei.DispName3 as fld3'
						,'S_Temp_LogData_Nittei.Kumiku', 'S_Temp_LogData_Nittei.FloorCode', 'S_Temp_LogData_Nittei.MacCode'
						,'S_Temp_LogData_Nittei.DistCode', 'S_Temp_LogData_Nittei.BDCode', 'S_Temp_LogData_Nittei.MngBData as fld11'
						,'S_Temp_LogData_Nittei.SDate as fld12', 'S_Temp_LogData_Nittei.EDate as fld13'
						,'S_Temp_LogData_Nittei.PlStdHr as fld15'
						,'S_Temp_LogData_Nittei.EpHr as fld16','S_Temp_LogData_Nittei.HS as fld17'
						,'S_Temp_LogData_Nittei.HK as fld18', 'S_Temp_LogData_Nittei.WBSCode as fld19'
						,'S_Temp_LogData_Nittei.Log as fld20', 'S_Temp_LogData_Nittei.KeshiPattern', 'S_Temp_LogData_Nittei.KeshiCode'
						,'MstFloor.Name as FloorName', 'MstMac.Name as MacName', 'MstDist.Name as DistName'
						,'MstBDCode.Name as BDName')
						->selectRaw("LTRIM(RTRIM(S_Temp_LogData_Nittei.Item)) as fld14")
						->selectRaw("LTRIM(RTRIM(S_Temp_LogData_Nittei.MacCode)) as MacCode")
						->selectRaw("LTRIM(RTRIM(S_Temp_LogData_Nittei.DistCode)) as DistCode")
						->leftJoin('MstFloor ', function($join) {
							$join->on('MstFloor.Code', '=', 'S_Temp_LogData_Nittei.FloorCode'); 
						})
						->leftJoin('MstMac ', function($join) {
							$join->on('MstMac.Code', '=', 'S_Temp_LogData_Nittei.MacCode'); 
						})
						->leftJoin('MstDist  ', function($join) {
							$join->on('MstDist.Code', '=', 'S_Temp_LogData_Nittei.DistCode'); 
						})
						->leftJoin('MstBDCode   ', function($join) {
							$join->on('MstBDCode.Code', '=', 'S_Temp_LogData_Nittei.BDCode'); 
						})
						->where('S_Temp_LogData_Nittei.ProjectID', '=', $projectID)
						->where('S_Temp_LogData_Nittei.OrderNo', '=', $order)
						->orderBy('S_Temp_LogData_Nittei.ID', 'asc')
						->get();

		$rows = $this->sortAndPagination($query, $sort, $direction, $pageunit, $request);

		$rows->getCollection()->transform(function ($value) {
			// format fld4
			$value['fld4'] = is_null($value['Kumiku']) ? NULL : FuncCommon::getKumikuData($value['Kumiku'])[2];
			// format fld5
			$value['fld5'] = is_null($value['FloorCode']) ? NULL 
							: $value['FloorCode'] .config('system_const.code_name_separator').$value['FloorName'];
			// format fld6
			$value['fld6'] = is_null($value['MacCode']) ? NULL 
							: $value['MacCode'] .config('system_const.code_name_separator').$value['MacName'];
			// format fld7
			$value['fld7'] = is_null($value['DistCode']) ? NULL 
							: $value['DistCode'] .config('system_const.code_name_separator').$value['DistName'];
			// format fld8
			$value['fld8'] = is_null($value['BDCode']) ? NULL 
							: $value['BDCode'] .config('system_const.code_name_separator').$value['BDName'];
			// format fld9
			if($value['KeshiCode'] == config('system_const_sches.keshicode_code_hr')) {
				$value['fld9'] = config('system_const_sches.keshicode_code_hr')
								.config('system_const.code_name_separator')
								.config('system_const_sches.keshicode_name_hr');
			}
			if($value['KeshiCode'] == config('system_const_sches.keshicode_code_bdata')) {
				$value['fld9'] = config('system_const_sches.keshicode_code_bdata')
								.config('system_const.code_name_separator')
								.config('system_const_sches.keshicode_name_bdata');
			}
			// format fld10
			if($value['KeshiPattern'] == config('system_const_sches.keshipattern_code_keshikomi')) {
				$value['fld10'] = config('system_const_sches.keshipattern_code_keshikomi')
								.config('system_const.code_name_separator')
								.config('system_const_sches.keshipattern_name_keshikomi');
			}
			if($value['KeshiPattern'] == config('system_const_sches.keshipattern_code_shintyoku')) {
				$value['fld10'] = config('system_const_sches.keshipattern_code_shintyoku')
								.config('system_const.code_name_separator')
								.config('system_const_sches.keshipattern_name_shintyoku');
			}
			return $value;
		});

		//ビューを表示
		$this->data['menuInfo'] = $menuInfo;
		$this->data['request'] = $request;
		$this->data['rows'] = $rows;
		$this->data['originalError'] = $originalError;

		return view('sches/makenittei/create', $this->data);
	}

	/**
	 * click button 保存 method
	 * @param Request 
	 * @return 
	 *
	 * @create 2021/01/14 Cuong
	 * @update
	 */
	public function accept(Request $request) {
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));
		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$projectID = valueUrlDecode($request->val2);		//projectID
		$order = valueUrlDecode($request->val3);			//order
		$val4 = $request->val4;								//val4 データ区分

		// urlErr
		$urlErr = url('/');
		$urlErr .= '/' . $menuInfo->KindURL;
		$urlErr .= '/' . $menuInfo->MenuURL;
		$urlErr .= '/create';
		$urlErr .= '?cmn1=' . $request->cmn1;
		$urlErr .= '&cmn2=' . $request->cmn2;
		$urlErr .= '&val1=' . $request->val1;
		$urlErr .= '&val2=' . $request->val2;
		$urlErr .= '&val3=' . $request->val3;
		$urlErr .= '&val4=' . $request->val4;
		$urlErr .= '&val5=' . $request->val5;
		$urlErr .= '&val6=' . $request->val6;
		$isOK = false;
		try {
			// 日程取込履歴」データを登録する
			$maxID = S_History_Nittei::max('id');
			$historyID = is_null($maxID) ? 1 : $maxID + 1;
			$this->registerSHistoryNittei($historyID, $menuInfo, $projectID, $order);

			// 検討ケースの実働期間を取得する
			$timeTrackerCommon = new TimeTrackerCommon();	//object TimeTrackerCommon
			$projectCalendar = $timeTrackerCommon->getCalendar($projectID);
			if(is_string($projectCalendar)) {		//when get data from TimeTrackerNX error
				// 排他制御を解除する
				$urlErr .= '&err1=' . valueUrlEncode($projectCalendar);
				return redirect($urlErr);
			}

			/* 各マスタとの整合性を確認する。 */
			$res = $this->checkConsistencyMaster($projectID, $order, $urlErr);
			if(is_string($res)) {
				return redirect($res);
			}

			/* 登録済み搭載日程レベルデータ、中日程レベルデータの取得 */
			$registeredTosaiNitteiData = $this->getRegisteredTosaiNitteiData($projectID, $order, $val4);
			$registeredChyuNitteiData = $this->getRegisteredChyuNitteiData($projectID, $order, $val4);
			
			/* 工程情報データの登録 */
			$dataInsert = $this->getDataSTempLogNittei($projectID, $order);
			$id = 1;
			$timeTrackerFuncSches = new TimeTrackerFuncSches();
			$maxID = S_JobData::where('ProjectID', '=', $projectID)->where('OrderNo', '=', $order)->max('id');
			foreach($dataInsert as $item) {
				/* [AMDFlag]が「0」の場合（追加） */
				if($item->AMDFlag == 0) {
					$res = $this->processAMDFlagIs0($registeredTosaiNitteiData, $registeredChyuNitteiData, $item, $projectID, 
											$order, $maxID, $projectCalendar, $timeTrackerFuncSches, $urlErr);
					if(is_string($res)) {
						return redirect($res);
					}
				}

				/* [AMDFlag]が「1」の場合（変更） */
				if($item->AMDFlag == 1) {
					$res = $this->processAMDFlagIs1($item, $projectID, $order, $projectCalendar, $timeTrackerFuncSches, $urlErr);
					if(is_string($res)) {
						return redirect($res);
					}
				}
			
				/* [AMDFlag]が「2」の場合（削除） */
				if($item->AMDFlag == 2) {
					$res = $this->processAMDFlagIs2($item, $timeTrackerFuncSches, $urlErr);
					if(is_string($res)) {
						return redirect($res);
					}
				}

				/* 「日程取込ログ」データを登録する。 */
				$this->registerDataToSLogDataNittei($historyID, $id, $item);
			}
			// 「日程取込履歴」データを更新する。
			S_History_Nittei::where('ID', '=', $historyID)->update(array('StatusFlag'=> 1));
			$isOK = true;

		} finally {
			if(!$isOK) {
				S_History_Nittei::where('ID', '=', $historyID)->update(array('StatusFlag'=> -1));
			}
			// 排他ロック解除処理
			$this->deleteLock ($menuInfo->KindID, config('system_const_sches.syslock_menuid_sches'), 
							$menuInfo->SessionID, valueUrlDecode($request->val1));
		}
		
		// 画面遷移
		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/index';
		$url .= '?cmn1=' . $request->cmn1;
		$url .= '&cmn2=' . $request->cmn2;
		$url .= '&val1=' . $request->val1;
		$url .= '&val2=' . $request->val2;
		$url .= '&val3=' . $request->val3;
		$url .= '&val4=' . $request->val4;
		$url .= '&val5=' . $request->val5;
		$url .= '&val6=' . $request->val6;
		
		return redirect($url);
	}

	/**
	 * register S_History_Nittei method
	 * @param int historyID 
	 * @param menuInfo menuInfo 
	 * @param int projectID 
	 * @param string order 
	 * @return 
	 *
	 * @create 2021/01/14 Cuong
	 * @update
	 */
	private function registerSHistoryNittei($historyID, $menuInfo, $projectID, $order) {
		$dateNow = DB::selectOne('SELECT CONVERT(DATETIME, getdate()) AS sysdate')->sysdate;
		$sHistoryNittei = new S_History_Nittei;
		$sHistoryNittei->ID = $historyID;
		$sHistoryNittei->Import_User = $menuInfo->UserID;
		$sHistoryNittei->Import_Date = $dateNow;
		$sHistoryNittei->ProjectID = $projectID;
		$sHistoryNittei->OrderNo = $order;
		$sHistoryNittei->StatusFlag = 0;
		$sHistoryNittei->save();
	}

	/**
	 * check Consistency Master method
	 * @param int projectID 
	 * @param string order 
	 * @param string urlErr 
	 * @return 
	 *
	 * @create 2021/01/14 Cuong
	 * @update
	 */
	private function checkConsistencyMaster($projectID, $order, $urlErr) {
		// 「施工棟」の整合性チェック
		$temp_LogData_Nittei = S_Temp_LogData_Nittei::select('S_Temp_LogData_Nittei.ID')
							->where("ProjectID", "=", $projectID)
							->where("OrderNo", "=", $order)
							->whereNotNull("FloorCode ")
							->whereNotExists(function ($query) {
								$query->select('MstFloor.Code')
									->from('MstFloor')
									->whereRaw('MstFloor.Code = S_Temp_LogData_Nittei.FloorCode')
									->where('MstFloor.ViewFlag ', '=', 1);
							})->get();
		if(count($temp_LogData_Nittei) > 0) {
			$messErr = sprintf(config('message.msg_sches_makenittei_010'), '施工棟');
			$urlErr .= '&err1=' . valueUrlEncode($messErr);
			return $urlErr;
		}

		// 「装置」の整合性チェック
		$temp_LogData_Nittei = S_Temp_LogData_Nittei::select('S_Temp_LogData_Nittei.ID')
							->where("ProjectID", "=", $projectID)
							->where("OrderNo", "=", $order)
							->whereNotNull("MacCode")
							->whereNotExists(function ($query) {
							$query->select('MstMac.Code')
								->from('MstMac')
								->whereRaw('MstMac.Code = S_Temp_LogData_Nittei.MacCode');
							})->get();
		if(count($temp_LogData_Nittei) > 0) {
			$messErr = sprintf(config('message.msg_sches_makenittei_010'), '装置');
			$urlErr .= '&err1=' . valueUrlEncode($messErr);
			return $urlErr;
		}

		// 「職種」の整合性チェック
		$temp_LogData_Nittei = S_Temp_LogData_Nittei::select('S_Temp_LogData_Nittei.ID')
							->where("ProjectID", "=", $projectID)
							->where("OrderNo", "=", $order)
							->whereNotNull("DistCode")
							->whereNotExists(function ($query) {
								$query->select('MstDist.Code')
									->from('MstDist')
									->whereRaw('MstDist.Code = S_Temp_LogData_Nittei.DistCode');
							})->get();
		if(count($temp_LogData_Nittei) > 0) {
			$messErr = sprintf(config('message.msg_sches_makenittei_010'), '職種');
			$urlErr .= '&err1=' . valueUrlEncode($messErr);
			return $urlErr;
		}
		// 「管理物量コード」の整合性チェック
		$temp_LogData_Nittei = S_Temp_LogData_Nittei::select('S_Temp_LogData_Nittei.ID')
							->where("ProjectID", "=", $projectID)
							->where("OrderNo", "=", $order)
							->whereNotNull("BDCode")
							->whereNotExists(function ($query) {
								$query->select('MstBDCode.Code')
									->from('MstBDCode')
									->whereRaw('MstBDCode.Code = S_Temp_LogData_Nittei.BDCode')
									->where('MstBDCode.ViewFlag ', '=', 1);
							})->get();
		if(count($temp_LogData_Nittei) > 0) {
			$messErr = sprintf(config('message.msg_sches_makenittei_010'), '管理物量コード');
			$urlErr .= '&err1=' . valueUrlEncode($messErr);
			return $urlErr;
		}

		return 1;
	}

	/**
	 * get Data STempLogNittei method
	 * @param int projectID 
	 * @param string order 
	 * @return array
	 *
	 * @create 2021/01/14 Cuong
	 * @update
	 */
	private function getDataSTempLogNittei($projectID, $order) {
		$dataInsert = S_Temp_LogData_Nittei::select('S_Temp_LogData_Nittei.ID','S_Temp_LogData_Nittei.DispName1',
									'S_Temp_LogData_Nittei.DispName2', 'S_Temp_LogData_Nittei.DispName3', 
									'S_Temp_LogData_Nittei.Kumiku', 'S_Temp_LogData_Nittei.FloorCode', 
									'S_Temp_LogData_Nittei.MacCode', 'S_Temp_LogData_Nittei.DistCode', 
									'S_Temp_LogData_Nittei.BDCode', 'S_Temp_LogData_Nittei.MngBData', 
									'S_Temp_LogData_Nittei.SDate', 'S_Temp_LogData_Nittei.EDate', 
									'S_Temp_LogData_Nittei.Item', 'S_Temp_LogData_Nittei.PlStdHr', 
									'S_Temp_LogData_Nittei.EpHr', 'S_Temp_LogData_Nittei.HS', 'S_Temp_LogData_Nittei.HK',
									'S_Temp_LogData_Nittei.WBSCode', 'S_Temp_LogData_Nittei.AMDFlag', 
									'S_Temp_LogData_Nittei.KeshiPattern', 'S_Temp_LogData_Nittei.KeshiCode',
									'S_JobData.WorkItemID')
									->leftJoin('S_JobData', function($join) {
										$join->on('S_JobData.ProjectID', '=', 'S_Temp_LogData_Nittei.ProjectID')
											->on('S_JobData.OrderNo', '=', 'S_Temp_LogData_Nittei.OrderNo')
											->on('S_JobData.DispName1', '=', 'S_Temp_LogData_Nittei.DispName1')
											->on('S_JobData.DispName2', '=', 'S_Temp_LogData_Nittei.DispName2')
											->on('S_JobData.DispName3', '=', 'S_Temp_LogData_Nittei.DispName3');
									})
									->where('S_Temp_LogData_Nittei.ProjectID', '=', $projectID)
									->where('S_Temp_LogData_Nittei.OrderNo', '=', $order)
									->whereNotNull('S_Temp_LogData_Nittei.AMDFlag')
									->where('S_JobData.Level_Job', '=', 3)
									->orderBy('S_Temp_LogData_Nittei.ID')->get();
		return $dataInsert;
	}

	/**
	 * get Registered TosaiNittei Data method
	 * @param int projectID 
	 * @param string order 
	 * @return array
	 *
	 * @create 2021/01/14 Cuong
	 * @update
	 */
	private function getRegisteredTosaiNitteiData($projectID, $order, $val4) {
		// 搭載日程レベルデータの取得
		$query = S_JobData::select('ID', 'WorkItemID', 'DispName1', 'DispName2')
						->where("ProjectID", "=", $projectID)
						->where("OrderNo", "=", $order)
						->where("Level_Job ", "=", 1);
		if($val4 == 0) {
			$query->where('S_JobData.IsOriginal', '=', 0);
		}else {
			$query->where('S_JobData.IsOriginal', '=', 1);
		}

		$data = $query->orderBy('S_JobData.ID')->get();
		$registeredTosaiNitteiData = [];
		if(count($data) > 0) {
			foreach($data as $item) {
				$key = $item->DispName1;
				$registeredTosaiNitteiData[$key] = array('ID'=>$item->ID, 'WorkItemID'=>$item->WorkItemID);
			}
		}

		return $registeredTosaiNitteiData;
	}

	/**
	 * get Registered ChyuNittei Data method
	 * @param int projectID 
	 * @param string order 
	 * @return array
	 *
	 * @create 2021/01/14 Cuong
	 * @update
	 */
	private function getRegisteredChyuNitteiData($projectID, $order, $val4) {
		// 中日程レベルデータの取得
		$query = S_JobData::select('ID', 'WorkItemID', 'DispName1', 'DispName2')
							->where("ProjectID", "=", $projectID)
							->where("OrderNo", "=", $order)
							->where("Level_Job ", "=", 2);
		if($val4 == 0) {
			$query->where('S_JobData.IsOriginal', '=', 0);
		}else {
			$query->where('S_JobData.IsOriginal', '=', 1);
		}

		$data = $query->orderBy('S_JobData.ID')->get();
		$registeredChyuNitteiData = [];
		if(count($data) > 0) {
			foreach($data as $item) {
				$key = $item->DispName1."_". $item->DispName2;
				$registeredChyuNitteiData[$key] = array('ID'=>$item->ID, 'WorkItemID'=>$item->WorkItemID);
			}
		}

		return $registeredChyuNitteiData;
	}

	/**
	 * process data has AMDFlag = 0 method
	 * @param array registeredTosaiNitteiData 
	 * @param array registeredChyuNitteiData 
	 * @param array item 
	 * @param int projectID 
	 * @param string order 
	 * @param int maxID 
	 * @param int projectCalendar 
	 * @param TimeTracker timeTrackerFuncSches 
	 * @param string urlErr 
	 * @return mix
	 *
	 * @create 2021/01/14 Cuong
	 * @update
	 */
	private function processAMDFlagIs0($registeredTosaiNitteiData, $registeredChyuNitteiData, $item, $projectID, $order, 
									&$maxID, $projectCalendar, $timeTrackerFuncSches, $urlErr) {
		/* 搭載日程レベルデータの確認、作成 */
		if(isset($registeredTosaiNitteiData[$item->DispName1])) {	// すでに搭載日程レベルデータが存在するか確認
			// データが取得できた場合（すでに搭載日程レベルデータが存在した場合）
			$tosaiNitteiID = $registeredTosaiNitteiData[$item->DispName1]['ID'];
			$tosaiWorkItemID = $registeredTosaiNitteiData[$item->DispName1]['WorkItemID'];
		}else {
			$tosaiWorkItemID = $timeTrackerFuncSches->insertPlan($projectID, $order, null, null, null, 
															null, $item->DispName1, null);
			if(is_string($tosaiWorkItemID)) {
				$urlErr .= '&err1=' . valueUrlEncode($tosaiWorkItemID);
				return $urlErr;
			}
			// [S_JobData]に搭載日程レベルデータを追加する。
			$maxID = $maxID + 1;
			$sJobData = new S_JobData;
			$sJobData->ProjectID = $projectID;
			$sJobData->OrderNo = $order;
			$sJobData->WorkItemID = $tosaiWorkItemID;
			$sJobData->ID = $maxID;
			$sJobData->Level_Job = 1;
			$sJobData->PID = $maxID;
			$sJobData->PID_Stn = $maxID;
			$sJobData->KakoKumiku = null;
			if(!is_null($item->DispName1)) {
				$sJobData->DispName1 = $item->DispName1;
			}
			$sJobData->DispName2 = null;
			$sJobData->DispName3 = null;
			$sJobData->AcFloor = null;
			$sJobData->AcMac = null;
			$sJobData->DistCode = null;
			$sJobData->BD_Code = null;
			$sJobData->MngBData = null;
			$sJobData->PlStdHr = null;
			$sJobData->EpHr = null;
			$sJobData->HS = null;
			$sJobData->HK = null;
			$sJobData->WBSCode = null;
			$sJobData->PrRateNow = 0;
			$sJobData->IsOriginal = 1;
			$sJobData->BaseID = 0;
			$sJobData->KeshiPattern = null;
			$sJobData->KeshiCode = 0;
			$sJobData->save();
			$tosaiNitteiID = $maxID;
			$registeredTosaiNitteiData[$item->DispName1] = array('ID'=>$tosaiNitteiID, 'WorkItemID'=>$tosaiWorkItemID);
		}

		/* 中日程レベルデータの確認、作成 */
		if(isset($registeredChyuNitteiData[$item->DispName1."_". $item->DispName2])) {	// すでに中日程レベルデータが存在するか確認
			// データが取得できた場合（すでに中日程レベルデータが存在した場合）
			$chyuNitteiID = $registeredChyuNitteiData[$item->DispName1]['ID'];
			$chyuNitteiWorkItemID = $registeredChyuNitteiData[$item->DispName1]['WorkItemID'];
		}else {
			$chyuNitteiWorkItemID = $timeTrackerFuncSches->insertPlan($projectID, $order, $tosaiWorkItemID, null, null, 
															null, $item->DispName1, null);
			if(is_string($chyuNitteiWorkItemID)) {
				$urlErr .= '&err1=' . valueUrlEncode($chyuNitteiWorkItemID);
				return $urlErr;
			}
			// [S_JobData]に搭載日程レベルデータを追加する。
			$maxIDChyu = $maxID + 1;
			$sJobData = new S_JobData;
			$sJobData->ProjectID = $projectID;
			$sJobData->OrderNo = $order;
			$sJobData->WorkItemID = $chyuNitteiWorkItemID;
			$sJobData->ID = $maxIDChyu;
			$sJobData->Level_Job = 2;
			if(!is_null($tosaiNitteiID)) {
				$sJobData->PID = $tosaiNitteiID;
			}
			if(!is_null($tosaiNitteiID)) {
				$sJobData->PID_Stn = $tosaiNitteiID;
			}
			$sJobData->KakoKumiku = null;
			if(!is_null($item->DispName1)) {
				$sJobData->DispName1 = $item->DispName1;
			}
			$sJobData->DispName2 = $item->DispName2;
			$sJobData->DispName3 = null;
			$sJobData->AcFloor = null;
			$sJobData->AcMac = null;
			$sJobData->DistCode = null;
			$sJobData->BD_Code = null;
			$sJobData->MngBData = null;
			$sJobData->PlStdHr = null;
			$sJobData->EpHr = null;
			$sJobData->HS = null;
			$sJobData->HK = null;
			$sJobData->WBSCode = null;
			$sJobData->PrRateNow = 0;
			$sJobData->IsOriginal = 1;
			$sJobData->BaseID = 0;
			$sJobData->KeshiPattern = null;
			$sJobData->KeshiCode = 0;
			$sJobData->save();
			$chyuNitteiID = $maxIDChyu;
			$registeredChyuNitteiData[$item->DispName1."_". $item->DispName2] = array('ID'=>$chyuNitteiID, 
																				'WorkItemID'=>$chyuNitteiWorkItemID);
			$maxID = $maxIDChyu;
		}

		/* 小日程レベルデータの作成 */
		$display = $item->DispName1;
		if(!is_null($item->DispName2)) {
			$display = $display.'-'.$item->DispName2;
		}
		if(!is_null($item->DispName3)) {
			$display = $display.'-'.$item->DispName3;
		}
		$koNitteiWorkItemID = $timeTrackerFuncSches->insertPlan($projectID, $order, $chyuNitteiWorkItemID, null, 
													$item->SDate, $item->EDate, $display, $projectCalendar);
		if(is_string($koNitteiWorkItemID)) {
			$urlErr .= '&err1=' . valueUrlEncode($koNitteiWorkItemID);
			return $urlErr;
		}
		
		$maxIDKo = $maxID + 1;
		$sJobData = new S_JobData;
		$sJobData->ProjectID = $projectID;
		$sJobData->OrderNo = $order;
		$sJobData->WorkItemID = $koNitteiWorkItemID;
		$sJobData->ID = $maxIDKo;
		$sJobData->Level_Job = 3;
		if(!is_null($chyuNitteiID)) {
			$sJobData->PID = $chyuNitteiID;
		}
		if(!is_null($tosaiNitteiID)) {
			$sJobData->PID_Stn = $tosaiNitteiID;
		}
		$sJobData->KakoKumiku = $item->Kumiku;
		if(!is_null($item->DispName1)) {
			$sJobData->DispName1 = $item->DispName1;
		}
		$sJobData->DispName2 = $item->DispName2;
		$sJobData->DispName3 = $item->DispName3;
		$sJobData->AcFloor = $item->FloorCode;
		$sJobData->AcMac = $item->MacCode;
		$sJobData->DistCode = $item->DistCode;
		$sJobData->BD_Code = $item->BDCode;
		$sJobData->MngBData = $item->MngBData;
		if(!is_null($item->Item)) {
			$sJobData->Item = $item->Item;
		}
		$sJobData->PlStdHr = $item->PlStdHr;
		$sJobData->EpHr = $item->EpHr;
		$sJobData->HS = $item->HS;
		$sJobData->HK = $item->HK;
		$sJobData->WBSCode = $item->WBSCode;
		$sJobData->PrRateNow = 0;
		$sJobData->IsOriginal = 1;
		$sJobData->BaseID = 0;
		$sJobData->KeshiPattern = $item->KeshiPattern;
		if(!is_null($item->KeshiCode)) {
			$sJobData->KeshiCode = $item->KeshiCode;
		}
		$sJobData->save();
		$maxID = $maxIDKo;

		return 1;
	}

	/**
	 * process data has AMDFlag = 1 method
	 * @param array item 
	 * @param int projectID 
	 * @param string order 
	 * @param int projectCalendar 
	 * @param TimeTracker timeTrackerFuncSches 
	 * @param string urlErr 
	 * @return mix
	 *
	 * @create 2021/01/14 Cuong
	 * @update
	 */
	private function processAMDFlagIs1($item, $projectID, $order, $projectCalendar, $timeTrackerFuncSches, $urlErr) {
		// ①	小日程レベルデータの確認、更新
		$display = $item->DispName1;
		if(!is_null($item->DispName2)) {
			$display = $display.'-'.$item->DispName2;
		}
		if(!is_null($item->DispName3)) {
			$display = $display.'-'.$item->DispName3;
		}
		$resUpdatePlan = $timeTrackerFuncSches->updatePlan($projectID, $order, $item->WorkItemID, $item->SDate, 
																$item->EDate, $display, $projectCalendar);
		if(!is_null($resUpdatePlan)) {
			$urlErr .= '&err1=' . valueUrlEncode($resUpdatePlan);
			return $urlErr;
		}else {
			$jobData['KakoKumiku'] = $item->Kumiku;
			$jobData['AcFloor'] = $item->FloorCode;
			$jobData['AcMac'] = $item->MacCode;
			$jobData['DistCode'] = $item->DistCode;
			$jobData['BD_Code'] = $item->BDCode;
			$jobData['MngBData'] = $item->MngBData;
			$jobData['Item'] = $item->Item;
			$jobData['PlStdHr'] = $item->PlStdHr;
			$jobData['EpHr'] = $item->EpHr;
			$jobData['HS'] = $item->HS;
			$jobData['HK'] = $item->HK;
			$jobData['WBSCode'] = $item->WBSCode;
			$jobData['KeshiPattern'] = $item->KeshiPattern;
			$jobData['KeshiCode'] = $item->KeshiCode;
			//update S_JobData
			$resultUpdate = S_JobData::where('WorkItemID', '=', $item->WorkItemID)->update($jobData);
		}

		return 1;
	}

	/**
	 * process data has AMDFlag = 2 method
	 * @param array item 
	 * @param TimeTracker timeTrackerFuncSches 
	 * @param string urlErr 
	 * @return mix
	 *
	 * @create 2021/01/14 Cuong
	 * @update
	 */
	private function processAMDFlagIs2($item, $timeTrackerFuncSches, $urlErr) {
		// 小日程レベルデータの確認、削除
		$resDeletePlan = $timeTrackerFuncSches->deletePlan($item->WorkItemID);
		if(!is_null($resDeletePlan)) {
			$urlErr .= '&err1=' . valueUrlEncode($resDeletePlan);
			return $urlErr;
		}else {
			//delete S_JobData
			$resultDelete = S_JobData::where('WorkItemID', '=', $item->WorkItemID)->delete();
		}

		return 1;
	}

	/**
	 * register Data To table S_LogData_Nittei method
	 * @param int historyID 
	 * @param int id 
	 * @param array item 
	 * @return mix
	 *
	 * @create 2021/01/14 Cuong
	 * @update
	 */
	private function registerDataToSLogDataNittei($historyID, &$id, $item) {
		// [S_LogData_Nittei]テーブルにデータを登録する。
		$id ++;
		$objLogData = new S_LogData_Nittei;
		$objLogData->HistoryID = $historyID;
		$objLogData->ID = $id;
		$objLogData->DispName1 = $item->DispName1;
		$objLogData->DispName2 = $item->DispName2;
		$objLogData->DispName3 = $item->DispName3;
		$objLogData->Kumiku = $item->Kumiku;
		$objLogData->FloorCode = $item->FloorCode;
		$objLogData->MacCode = $item->MacCode;
		$objLogData->DistCode = $item->DistCode;
		$objLogData->BDCode = $item->BDCode;
		$objLogData->MngBData = $item->MngBData;
		$objLogData->SDate = $item->SDate;
		$objLogData->EDate = $item->EDate;
		$objLogData->Item = $item->Item;
		$objLogData->PlStdHr = $item->PlStdHr;
		$objLogData->EpHr = $item->EpHr;
		$objLogData->HS = $item->HS;
		$objLogData->HK = $item->HK;
		$objLogData->WBSCode = $item->WBSCode;
		$objLogData->Log = $item->Log;
		$objLogData->KeshiPattern = $item->KeshiPattern;
		$objLogData->KeshiCode = $item->KeshiCode;
		$objLogData->save();
	}

	/**
	 * click button キャンセル method
	 * @param Request 
	 * @return 
	 *
	 * @create 2021/01/14 Cuong
	 * @update
	 */
	public function cancel(Request $request) {
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));
		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$this->deleteLock ($menuInfo->KindID, config('system_const_sches.syslock_menuid_sches'), 
							$menuInfo->SessionID, valueUrlDecode($request->val1));
		// url
		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/index';
		$url .= '?cmn1=' . $request->cmn1;
		$url .= '&cmn2=' . $request->cmn2;
		$url .= '&val1=' . $request->val1;
		$url .= '&val2=' . $request->val2;
		$url .= '&val3=' . $request->val3;
		$url .= '&val4=' . $request->val4;
		$url .= '&val5=' . $request->val5;
		$url .= '&val6=' . $request->val6;
		
		return redirect($url);
	}

	/**
	* read data excel method
	* @param request 
	* @return menuInfo
	*
	* @create 2020/12/30 Cuong
	* @update
	*/
	private function readDataFileExcel($request, $menuInfo)
	{
		$dataExcel = array();
		$sheetNitteiName = config('system_const_sches.sches_nitteisheet_name');	//name sheet nittei
		$sheetMasterName = config('system_const_sches.sches_mastersheet_name');	//name sheet master

		$file = $request->val5;
		$extension = strtolower($file->getClientOriginalExtension()); //extension file uploaded
		$inputFileName = $_FILES['val5']['tmp_name'];
		try {
			/**  Identify the type of $inputFileName  **/
			$inputFileType = IOFactory::identify($inputFileName);
			/**  Create a new Reader of the type that has been identified  **/
			$reader = IOFactory::createReader($inputFileType);

			// エクセルファイルのシート名、ヘッダー確認
			$spreadsheet = $reader->load($inputFileName);
			$arrSheetName = $spreadsheet->getSheetNames(); //list sheet name

			// url
			$url = url('/');
			$url .= '/' . $menuInfo->KindURL;
			$url .= '/' . $menuInfo->MenuURL;
			$url .= '/index';
			$url .= '?cmn1=' . $request->cmn1;
			$url .= '&cmn2=' . $request->cmn2;
			for($i=1; $i<=6; $i++){
				$key = 'val'.$i;
				$url .= '&val'.$i.'=' . valueUrlEncode($request->$key);
			}

			/* シート名の確認 */
				// 「Nittei」シートが存在しない場合
			if(!in_array($sheetNitteiName, $arrSheetName)) {
				$originalError = sprintf(config('message.msg_sches_makenittei_001'), $sheetNitteiName);
				$url .= '&err1=' . valueUrlEncode($originalError);
				return $url;
			}
				// 「Master」シートが存在しない場合
			if(!in_array($sheetMasterName, $arrSheetName)) {
				$originalError = sprintf(config('message.msg_sches_makenittei_001'), $sheetMasterName);
				$url .= '&err1=' . valueUrlEncode($originalError);
				return $url;
			}

			/* ヘッダーの確認 */
				// 「Nittei」シート
			$worksheet = $spreadsheet->getSheetByName($sheetNitteiName);
			$headerSheetNittei = $this->checkSheetNittei($worksheet, $url);
			if(is_string($headerSheetNittei)) {
				return $headerSheetNittei;
			}
				// 「Master」シート
			$worksheet = $spreadsheet->getSheetByName($sheetMasterName);
			$resCheckSheetMaster = $this->checkSheetMaster($worksheet, $url);
			if(is_string($resCheckSheetMaster)) {
				return $resCheckSheetMaster;
			}

			// 一時データの削除
			$projectID = $request->val2;		//projectID
			$order = $request->val3;			//order
			$val4 = $request->val4;				//val4 データ区分
			// 一時データの削除
			$resulDel = S_Temp_LogData_Nittei::where('ProjectID', '=', $projectID)
								->where('OrderNo', '=', $order)
								->delete();
			
			/* エクセルファイルの一時読込 */
				// 「Nittei」シート
			$worksheet = $spreadsheet->getSheetByName($sheetNitteiName);
			$dataNitteiExcel = $this->readDataSheetNittei($worksheet);
				// 「Master」シート
			$worksheet = $spreadsheet->getSheetByName($sheetMasterName);
			$dataMasterExcel = $this->readDataSheetMaster($worksheet);

			$dataExcel[$sheetNitteiName] = $dataNitteiExcel;
			$dataExcel[$sheetMasterName] = $dataMasterExcel;
			$dataExcel['headerSheetNittei'] = $headerSheetNittei;

			return $dataExcel;
		} finally {
			// disconnect Worksheets
			$spreadsheet->disconnectWorksheets();
			$spreadsheet->garbageCollect();
		}
	}

	/**
	* ヘッダーの確認(シートNittei)
	* @param worksheet
	* @param string url
	* @return
	*
	* @create 2021/01/08 Cuong
	* @update
	*/
	private function checkSheetNittei($worksheet, $url) {
		// ヘッダーの確認
		$headerNittei = array();
		$headerRow = config('system_const_sches.nt_xl_header_row');				//header row begin
		$sheetNitteiName = config('system_const_sches.sches_nitteisheet_name');	//name sheet nittei
		$arrColName = array('A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 
							'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T');			//list column
		foreach($arrColName as $colName) {
			$cellValue = $worksheet->getCell($colName.$headerRow)->getValue();
			if($colName == 'A') {
				if($cellValue != '削除フラグ') {
					$originalError = sprintf(config('message.msg_sches_makenittei_002'),$sheetNitteiName);
					$url .= '&err1=' . valueUrlEncode($originalError);
					return $url;
				}
				$headerNittei['DeleteFlag'] = $cellValue;
			}
			if($colName == 'B') {
				if($cellValue != '名称1') {
					$originalError = sprintf(config('message.msg_sches_makenittei_002'),$sheetNitteiName);
					$url .= '&err1=' . valueUrlEncode($originalError);
					return $url;
				}
				$headerNittei['DispName1'] = $cellValue;
			}
			if($colName == 'C') {
				if($cellValue != '名称2') {
					$originalError = sprintf(config('message.msg_sches_makenittei_002'),$sheetNitteiName);
					$url .= '&err1=' . valueUrlEncode($originalError);
					return $url;
				}
				$headerNittei['DispName2'] = $cellValue;
			}
			if($colName == 'D') {
				if($cellValue != '名称3') {
					$originalError = sprintf(config('message.msg_sches_makenittei_002'),$sheetNitteiName);
					$url .= '&err1=' . valueUrlEncode($originalError);
					return $url;
				}
				$headerNittei['DispName3'] = $cellValue;
			}
			if($colName == 'E') {
				if($cellValue != '組区') {
					$originalError = sprintf(config('message.msg_sches_makenittei_002'),$sheetNitteiName);
					$url .= '&err1=' . valueUrlEncode($originalError);
					return $url;
				}
				$headerNittei['KakoKumiku'] = $cellValue;
			}
			if($colName == 'F') {
				if($cellValue != '施工棟') {
					$originalError = sprintf(config('message.msg_sches_makenittei_002'),$sheetNitteiName);
					$url .= '&err1=' . valueUrlEncode($originalError);
					return $url;
				}
				$headerNittei['AcFloor'] = $cellValue;
			}
			if($colName == 'G') {
				if($cellValue != '装置') {
					$originalError = sprintf(config('message.msg_sches_makenittei_002'),$sheetNitteiName);
					$url .= '&err1=' . valueUrlEncode($originalError);
					return $url;
				}
				$headerNittei['AcMac'] = $cellValue;
			}
			if($colName == 'H') {
				if($cellValue != '職種') {
					$originalError = sprintf(config('message.msg_sches_makenittei_002'),$sheetNitteiName);
					$url .= '&err1=' . valueUrlEncode($originalError);
					return $url;
				}
				$headerNittei['DistCode'] = $cellValue;
			}
			if($colName == 'I') {
				if($cellValue != '管理物量コード') {
					$originalError = sprintf(config('message.msg_sches_makenittei_002'),$sheetNitteiName);
					$url .= '&err1=' . valueUrlEncode($originalError);
					return $url;
				}
				$headerNittei['BD_Code'] = $cellValue;
			}
			if($colName == 'J') {
				if($cellValue != '消込管理') {
					$originalError = sprintf(config('message.msg_sches_makenittei_002'),$sheetNitteiName);
					$url .= '&err1=' . valueUrlEncode($originalError);
					return $url;
				}
				$headerNittei['KeshiCode'] = $cellValue;
			}
			if($colName == 'K') {
				if($cellValue != '消込方式') {
					$originalError = sprintf(config('message.msg_sches_makenittei_002'),$sheetNitteiName);
					$url .= '&err1=' . valueUrlEncode($originalError);
					return $url;
				}
				$headerNittei['KeshiPattern'] = $cellValue;
			}
			if($colName == 'L') {
				if($cellValue != '物量') {
					$originalError = sprintf(config('message.msg_sches_makenittei_002'),$sheetNitteiName);
					$url .= '&err1=' . valueUrlEncode($originalError);
					return $url;
				}
				$headerNittei['MngBData'] = $cellValue;
			}
			if($colName == 'M') {
				if($cellValue != '着工日') {
					$originalError = sprintf(config('message.msg_sches_makenittei_002'),$sheetNitteiName);
					$url .= '&err1=' . valueUrlEncode($originalError);
					return $url;
				}
				$headerNittei['sDate'] = $cellValue;
			}
			if($colName == 'N') {
				if($cellValue != '完工日') {
					$originalError = sprintf(config('message.msg_sches_makenittei_002'),$sheetNitteiName);
					$url .= '&err1=' . valueUrlEncode($originalError);
					return $url;
				}
				$headerNittei['eDate'] = $cellValue;
			}
			if($colName == 'O') {
				if($cellValue != 'アイテム') {
					$originalError = sprintf(config('message.msg_sches_makenittei_002'),$sheetNitteiName);
					$url .= '&err1=' . valueUrlEncode($originalError);
					return $url;
				}
				$headerNittei['Item'] = $cellValue;
			}
			if($colName == 'P') {
				if($cellValue != 'HC') {
					$originalError = sprintf(config('message.msg_sches_makenittei_002'),$sheetNitteiName);
					$url .= '&err1=' . valueUrlEncode($originalError);
					return $url;
				}
				$headerNittei['HC'] = $cellValue;
			}
			if($colName == 'Q') {
				if($cellValue != 'HJ') {
					$originalError = sprintf(config('message.msg_sches_makenittei_002'),$sheetNitteiName);
					$url .= '&err1=' . valueUrlEncode($originalError);
					return $url;
				}
				$headerNittei['HJ'] = $cellValue;
			}
			if($colName == 'R') {
				if($cellValue != 'HS') {
					$originalError = sprintf(config('message.msg_sches_makenittei_002'),$sheetNitteiName);
					$url .= '&err1=' . valueUrlEncode($originalError);
					return $url;
				}
				$headerNittei['HS'] = $cellValue;
			}
			if($colName == 'S') {
				if($cellValue != 'HK') {
					$originalError = sprintf(config('message.msg_sches_makenittei_002'),$sheetNitteiName);
					$url .= '&err1=' . valueUrlEncode($originalError);
					return $url;
				}
				$headerNittei['HK'] = $cellValue;
			}
			if($colName == 'T') {
				if($cellValue != 'WBSコード') {
					$originalError = sprintf(config('message.msg_sches_makenittei_002'),$sheetNitteiName);
					$url .= '&err1=' . valueUrlEncode($originalError);
					return $url;
				}
				$headerNittei['WBSCode'] = $cellValue;
			}
			
		}

		return $headerNittei;
	}
	
	/**
	* ヘッダーの確認(シートMaster)
	* @param worksheet 
	* @param string url 
	* @return
	*
	* @create 2021/01/08 Cuong
	* @update
	*/
	private function checkSheetMaster($worksheet, $url) {
		// ヘッダーの確認
			//Master
		$headerRow = config('system_const_sches.nt_xl_header_row');	//header row begin
		$sheetMasterName = config('system_const_sches.sches_mastersheet_name');	//name sheet nittei
		$arrColName = array('A', 'B', 'C', 'D', 'E', 'F', 'G');
		foreach($arrColName as $colName) {
				$cellValue = $worksheet->getCell($colName.$headerRow)->getValue();
				if($colName == 'A') {
					if($cellValue != '組区') {
						$originalError = sprintf(config('message.msg_sches_makenittei_002'),$sheetMasterName);
						$url .= '&err1=' . valueUrlEncode($originalError);
						return $url;
					}
				}
				if($colName == 'B') {
					if($cellValue != '施工棟') {
						$originalError = sprintf(config('message.msg_sches_makenittei_002'),$sheetMasterName);
					}
				}
				if($colName == 'C') {
					if($cellValue != '装置') {
						$originalError = sprintf(config('message.msg_sches_makenittei_002'),$sheetMasterName);
						$url .= '&err1=' . valueUrlEncode($originalError);
						return $url;
					}
				}
				if($colName == 'D') {
					if($cellValue != '職種') {
						$originalError = sprintf(config('message.msg_sches_makenittei_002'),$sheetMasterName);
						$url .= '&err1=' . valueUrlEncode($originalError);
						return $url;
					}
				}
				if($colName == 'E') {
					if($cellValue != '管理物量コード') {
						$originalError = sprintf(config('message.msg_sches_makenittei_002'),$sheetMasterName);
						$url .= '&err1=' . valueUrlEncode($originalError);
						return $url;
					}
				}
				if($colName == 'F') {
					if($cellValue != '消込管理') {
						$originalError = sprintf(config('message.msg_sches_makenittei_002'),$sheetMasterName);
						$url .= '&err1=' . valueUrlEncode($originalError);
						return $url;
					}
				}
				if($colName == 'G') {
					if($cellValue != '消込方式') {
						$originalError = sprintf(config('message.msg_sches_makenittei_002'),$sheetMasterName);
						$url .= '&err1=' . valueUrlEncode($originalError);
						return $url;
					}
				}
			
		}

		return 1;
	}

	/**
	* register import log
	* @param array dataExcel
	* @param int row
	* @param int id
	* @param string message
	* @param Request request
	* @return
	*
	* @create 2021/01/08 Cuong
	* @update
	*/
	private function registerImportLog($dataExcel, $headerNittei, $row, &$id, $message, $request) {
		$id++;
		$projectID = $request->val2;			//projectID
		$order = $request->val3;				//order
		$val4 = $request->val4;					//val4 データ区分

		/* 取込ログ登録 */
		if($message != '') {	// メモリ「$取込内容」に値が設定されている場合
			$obj = new S_Temp_LogData_Nittei;
			$obj->ProjectID = $projectID;
			$obj->OrderNo = $order;
			$obj->ID = $id;
			$obj->DispName1 = substr($dataExcel['DispName1'], 0 , 100);
			$obj->Log = $message;
			$obj->AMDFlag = NULL;
			$obj->save();
		}else {					// メモリ「$取込内容」に値が設定されていない場合
			if($dataExcel['DeleteFlag'] == config('system_const_sches.deleteflag')) {	// (ア)	「$エクセルデータ」[DeleteFlag]が「D」の場合
				$this->deleteFlagIsD($projectID, $order, $val4, $dataExcel, $headerNittei, $row, $id);
			}
			if(is_null($dataExcel['DeleteFlag'])) {		// $エクセルデータ」[DeleteFlag]がNULLの場合
				$this->deleteFlagIsNull($projectID, $order, $val4, $dataExcel, $headerNittei, $id);
			} 
		}
	}

	/**
	* when delete flag is "D"
	* @param int projectID
	* @param string order
	* @param array dataExcel
	* @param array headerNittei
	* @param int row
	* @param int id
	* @return
	*
	* @create 2021/01/08 Cuong
	* @update
	*/
	private function deleteFlagIsD($projectID, $order, $val4, $dataExcel, $headerNittei, $row, $id) {
		$obj = new S_Temp_LogData_Nittei;
		$obj->ProjectID = $projectID;
		$obj->OrderNo = $order;
		$obj->ID = $id;
		$obj->DispName1 = $dataExcel['DispName1'];
		$obj->DispName2 = $dataExcel['DispName2'];
		$obj->DispName3 = $dataExcel['DispName3'];
		$obj->Kumiku = is_null($dataExcel['KakoKumiku']) ? NULL 
						: FuncCommon::getSplitChar($dataExcel['KakoKumiku'])[0];
		$obj->FloorCode = is_null($dataExcel['AcFloor']) ? NULL 
						: FuncCommon::getSplitChar($dataExcel['AcFloor'])[0];
		$obj->MacCode = is_null($dataExcel['AcMac']) ? NULL 
						: FuncCommon::getSplitChar($dataExcel['AcMac'])[0];
		$obj->DistCode = is_null($dataExcel['DistCode']) ? NULL 
						: FuncCommon::getSplitChar($dataExcel['DistCode'])[0];
		$obj->BDCode = is_null($dataExcel['BD_Code']) ? NULL 
						: FuncCommon::getSplitChar($dataExcel['BD_Code'])[0];
		$obj->MngBData = $dataExcel['MngBData'];
		$obj->SDate = $dataExcel['sDate'];
		$obj->EDate = $dataExcel['eDate'];
		$obj->Item = $dataExcel['Item'];
		$obj->PlStdHr = $dataExcel['HC'];
		$obj->EpHr = $dataExcel['HJ'];
		$obj->HS = $dataExcel['HS'];
		$obj->HK = $dataExcel['HK'];
		$obj->WBSCode = $dataExcel['WBSCode'];
		$obj->Log = $val4 == 0 ? sprintf(config('message.msg_sches_makenittei_012'), $row) 
								: sprintf(config('message.msg_sches_makenittei_013'), $row);
		$obj->AMDFlag = $val4 == 0 ? NULL : 2;
		$obj->KeshiPattern = is_null($dataExcel['KeshiPattern']) ? NULL 
							: FuncCommon::getSplitChar($dataExcel['KeshiPattern'])[0];
		$obj->KeshiCode = is_null($dataExcel['KeshiCode']) ? NULL 
						: FuncCommon::getSplitChar($dataExcel['KeshiCode'])[0];
		$obj->save();
	}

	/**
	* when delete flag is null
	* @param int projectID
	* @param string order
	* @param array dataExcel
	* @param array headerNittei
	* @param int id
	* @return
	*
	* @create 2021/01/08 Cuong
	* @update
	*/
	private function deleteFlagIsNull($projectID, $order, $val4,$dataExcel, $headerNittei, &$id) {
		$query = S_JobData::select('S_JobData.WorkItemID','S_JobData.DispName1','S_JobData.DispName2'
								,'S_JobData.DispName3','S_JobData.KakoKumiku','S_JobData.AcFloor'
								,'S_JobData.BD_Code','S_JobData.MngBData','S_JobData.PlStdHr'
								,'S_JobData.EpHr','S_JobData.HS','S_JobData.HK','S_JobData.WBSCode'
								,'S_JobData.KeshiPattern','S_JobData.KeshiCode')
								->selectRaw("LTRIM(RTRIM(S_JobData.Item)) as Item")
								->selectRaw("LTRIM(RTRIM(S_JobData.AcMac)) as AcMac")
								->selectRaw("LTRIM(RTRIM(S_JobData.DistCode)) as DistCode")
								->where('S_JobData.ProjectID', '=', $projectID)
								->where('S_JobData.OrderNo', '=', $order)
								->where('S_JobData.Level_Job', '=', 3)
								->where('S_JobData.DispName1 ', '=', $dataExcel['DispName1']);

		if(is_null($dataExcel['DispName2'])) {
			$query->whereNull('S_JobData.DispName2');
		}else {
			$query->where('S_JobData.DispName2 ', '=', $dataExcel['DispName2']);
		}

		if(is_null($dataExcel['DispName3'])) {
			$query->whereNull('S_JobData.DispName3');
		}else {
			$query->where('S_JobData.DispName3 ', '=', $dataExcel['DispName3']);
		}

		if($val4 == 0) {
			$query->where('S_JobData.IsOriginal', '=', 0);
		}else {
			$query->where('S_JobData.IsOriginal', '=', 1);
		}
		
		$getSJobData = $query->get()->toArray();

		if(count($getSJobData) == 0) {	// データが取得できない場合
			$obj = new S_Temp_LogData_Nittei;
			$obj->ProjectID = $projectID;
			$obj->OrderNo = $order;
			$obj->ID = $id;
			$obj->DispName1 = $dataExcel['DispName1'];
			$obj->DispName2 = $dataExcel['DispName2'];
			$obj->DispName3 = $dataExcel['DispName3'];
			$obj->Kumiku = is_null($dataExcel['KakoKumiku']) ? NULL 
							: FuncCommon::getSplitChar($dataExcel['KakoKumiku'])[0];
			$obj->FloorCode = is_null($dataExcel['AcFloor']) ? NULL 
							: FuncCommon::getSplitChar($dataExcel['AcFloor'])[0];
			$obj->MacCode = is_null($dataExcel['AcMac']) ? NULL 
							: FuncCommon::getSplitChar($dataExcel['AcMac'])[0];
			$obj->DistCode = is_null($dataExcel['DistCode']) ? NULL 
							: FuncCommon::getSplitChar($dataExcel['DistCode'])[0];
			$obj->BDCode = is_null($dataExcel['BD_Code']) ? NULL 
							: FuncCommon::getSplitChar($dataExcel['BD_Code'])[0];
			$obj->MngBData = $dataExcel['MngBData'];
			$obj->SDate = $dataExcel['sDate'];
			$obj->EDate = $dataExcel['eDate'];
			$obj->Item = $dataExcel['Item'];
			$obj->PlStdHr = $dataExcel['HC'];
			$obj->EpHr = $dataExcel['HJ'];
			$obj->HS = $dataExcel['HS'];
			$obj->HK = $dataExcel['HK'];
			$obj->WBSCode = $dataExcel['WBSCode'];
			$display = $dataExcel['DispName1'];
			if(!is_null($dataExcel['DispName2'])) {
				$display.= '-'.$dataExcel['DispName2'];
			}
			if(!is_null($dataExcel['DispName3'])) {
				$display.= '-'.$dataExcel['DispName3'];
			}
			$obj->Log = $val4 == 0 ? sprintf(config('message.msg_sches_makenittei_014'), $display)
									: config('message.msg_sches_makenittei_015');
			$obj->AMDFlag = $val4 == 0 ? NULL : 0;
			$obj->KeshiPattern = is_null($dataExcel['KeshiPattern']) ? NULL 
								: FuncCommon::getSplitChar($dataExcel['KeshiPattern'])[0];
			$obj->KeshiCode = is_null($dataExcel['KeshiCode']) ? NULL 
								: FuncCommon::getSplitChar($dataExcel['KeshiCode'])[0];
			$obj->save();
		}elseif(count($getSJobData) == 1) {	// データが１件のみ取得できた場合
			$timeTrackerNX = new TimeTrackerCommon;
			$lstWorkItemID = array_column($getSJobData, 'WorkItemID');
			$resulTimeTK = $timeTrackerNX->getKoteiRange($lstWorkItemID, false);

			// 戻り値が配列の場合
			if(is_array($resulTimeTK)) {
				// 下記、項目対比表を元に値の比較を行う。
				$this->hasOneData($getSJobData, $resulTimeTK, $dataExcel, $headerNittei, $projectID, $order, $id);
			}
		}else {	// データが２件以上取得できた場合
			$obj = new S_Temp_LogData_Nittei;
			$obj->ProjectID = $projectID;
			$obj->OrderNo = $order;
			$obj->ID = $id;
			$obj->DispName1 = $dataExcel['DispName1'];
			$obj->Log = sprintf(config('message.msg_sches_makenittei_017'), $id);
			$obj->Log = NULL;
			$obj->save();
		}
	}
	/**
	* get data SJob has one data
	* @param array getSJobData
	* @param array resulTimeTK
	* @param array dataExcel
	* @param array headerNittei
	* @param int id
	* @return
	*
	* @create 2021/01/08 Cuong
	* @update
	*/
	private function hasOneData($getSJobData, $resulTimeTK, $dataExcel, $headerNittei, $projectID, $order, &$id) {
		foreach($getSJobData as $key=>$value) {
			$dataTimeTK = $resulTimeTK[$value['WorkItemID']];
			$isCheck = true;
			$mesLog = '';
			if($dataExcel['DispName1'] != $value['DispName1']) {
				$isCheck = false;
				$mesLog .= $headerNittei['DispName1'];
			}
			if($dataExcel['DispName2'] != $value['DispName2']) {
				$isCheck = false;
				empty($mesLog) ? $mesLog .= $headerNittei['DispName2'] 
							: $mesLog .= ','.$headerNittei['DispName2'];
			}
			if($dataExcel['DispName3'] != $value['DispName3']) {
				$isCheck = false;
				empty($mesLog) ? $mesLog .= $headerNittei['DispName3'] 
							: $mesLog .= ','.$headerNittei['DispName3'];
			}

			$dataExcel_KakoKumiku = is_null($dataExcel['KakoKumiku']) ? NULL 
								: FuncCommon::getSplitChar($dataExcel['KakoKumiku'])[0];
			if($dataExcel_KakoKumiku != $value['KakoKumiku']) {
				$isCheck = false;
				empty($mesLog) ? $mesLog .= $headerNittei['KakoKumiku'] 
							: $mesLog .= ','.$headerNittei['KakoKumiku'];
			}

			$dataExcel_AcFloor = is_null($dataExcel['AcFloor']) ? NULL 
								: FuncCommon::getSplitChar($dataExcel['AcFloor'])[0];
			if($dataExcel_AcFloor != $value['AcFloor']) {
				$isCheck = false;
				empty($mesLog) ? $mesLog .= $headerNittei['AcFloor'] 
							: $mesLog .= ','.$headerNittei['AcFloor'];
			}

			$dataExcel_AcMac = is_null($dataExcel['AcMac']) ? NULL 
								: FuncCommon::getSplitChar($dataExcel['AcMac'])[0];
			if($dataExcel_AcMac != $value['AcMac']) {
				$isCheck = false;
				empty($mesLog) ? $mesLog .= $headerNittei['AcMac'] 
							: $mesLog .= ','.$headerNittei['AcMac'];
			}

			$dataExcel_DistCode = is_null($dataExcel['DistCode']) ? NULL 
								: FuncCommon::getSplitChar($dataExcel['DistCode'])[0];
			if($dataExcel_DistCode != $value['DistCode']) {
				$isCheck = false;
				empty($mesLog) ? $mesLog .= $headerNittei['DistCode'] 
							: $mesLog .= ','.$headerNittei['DistCode'];
			}

			$dataExcel_BD_Code = is_null($dataExcel['BD_Code']) ? NULL 
								: FuncCommon::getSplitChar($dataExcel['BD_Code'])[0];
			if($dataExcel_BD_Code != $value['BD_Code']) {
				$isCheck = false;
				empty($mesLog) ? $mesLog .= $headerNittei['BD_Code'] 
							: $mesLog .= ','.$headerNittei['BD_Code'];
			}
			if($dataExcel['MngBData'] != $value['MngBData']) {
				$isCheck = false;
				empty($mesLog) ? $mesLog .= $headerNittei['MngBData'] 
							: $mesLog .= ','.$headerNittei['MngBData'];
			}
			if($dataExcel['sDate'] != $dataTimeTK['plannedStartDate']) {
				$isCheck = false;
				empty($mesLog) ? $mesLog .= $headerNittei['sDate'] 
							: $mesLog .= ','.$headerNittei['sDate'];
			}
			if($dataExcel['eDate'] != $dataTimeTK['plannedFinishDate']) {
				$isCheck = false;
				empty($mesLog) ? $mesLog .= $headerNittei['eDate'] 
							: $mesLog .= ','.$headerNittei['eDate'];
			}
			if($dataExcel['Item'] != $value['Item']) {
				$isCheck = false;
				empty($mesLog) ? $mesLog .= $headerNittei['Item'] 
							: $mesLog .= ','.$headerNittei['Item'];
			}
			if($dataExcel['HC'] != $value['PlStdHr']) {
				$isCheck = false;
				empty($mesLog) ? $mesLog .= $headerNittei['HC'] 
							: $mesLog .= ','.$headerNittei['HC'];
			}
			if($dataExcel['HJ'] != $value['EpHr']) {
				$isCheck = false;
				empty($mesLog) ? $mesLog .= $headerNittei['HJ'] 
							: $mesLog .= ','.$headerNittei['HJ'];
			}
			if($dataExcel['HS'] != $value['HS']) {
				$isCheck = false;
				empty($mesLog) ? $mesLog .= $headerNittei['HS'] 
							: $mesLog .= ','.$headerNittei['HS'];
			}
			if($dataExcel['HK'] != $value['HK']) {
				$isCheck = false;
				empty($mesLog) ? $mesLog .= $headerNittei['HK'] 
							: $mesLog .= ','.$headerNittei['HK'];
			}
			if($dataExcel['WBSCode'] != $value['WBSCode']) {
				$isCheck = false;
				empty($mesLog) ? $mesLog .= $headerNittei['WBSCode'] 
							: $mesLog .= ','.$headerNittei['WBSCode'];
			}

			$dataExcel_KeshiPattern = is_null($dataExcel['KeshiPattern']) ? NULL 
								: FuncCommon::getSplitChar($dataExcel['KeshiPattern'])[0];
			if($dataExcel_KeshiPattern != $value['KeshiPattern']) {
				$isCheck = false;
				empty($mesLog) ? $mesLog .= $headerNittei['KeshiPattern'] 
							: $mesLog .= ','.$headerNittei['KeshiPattern'];
			}

			$dataExcel_KeshiCode = is_null($dataExcel['KeshiCode']) ? NULL 
								: FuncCommon::getSplitChar($dataExcel['KeshiCode'])[0];
			if($dataExcel_KeshiCode != $value['KeshiCode']) {
				$isCheck = false;
				empty($mesLog) ? $mesLog .= $headerNittei['KeshiCode'] 
							: $mesLog .= ','.$headerNittei['KeshiCode'];
			}

			if(!$isCheck) {		// 値が一致しない項目が存在する場合
				$obj = new S_Temp_LogData_Nittei;
				$obj->ProjectID = $projectID;
				$obj->OrderNo = $order;
				$obj->ID = $id;
				$obj->DispName1 = $dataExcel['DispName1'];
				$obj->DispName2 = $dataExcel['DispName2'];
				$obj->DispName3 = $dataExcel['DispName3'];
				$obj->Kumiku = is_null($dataExcel['KakoKumiku']) ? NULL 
								: FuncCommon::getSplitChar($dataExcel['KakoKumiku'])[0];
				$obj->FloorCode = is_null($dataExcel['AcFloor']) ? NULL 
								: FuncCommon::getSplitChar($dataExcel['AcFloor'])[0];
				$obj->MacCode = is_null($dataExcel['AcMac']) ? NULL 
								: FuncCommon::getSplitChar($dataExcel['AcMac'])[0];
				$obj->DistCode = is_null($dataExcel['DistCode']) ? NULL 
								: FuncCommon::getSplitChar($dataExcel['DistCode'])[0];
				$obj->BDCode = is_null($dataExcel['BD_Code']) ? NULL 
								: FuncCommon::getSplitChar($dataExcel['BD_Code'])[0];
				$obj->MngBData = $dataExcel['MngBData'];
				$obj->SDate = $dataExcel['sDate'];
				$obj->EDate = $dataExcel['eDate'];
				$obj->Item = $dataExcel['Item'];
				$obj->PlStdHr = $dataExcel['HC'];
				$obj->EpHr = $dataExcel['HJ'];
				$obj->HS = $dataExcel['HS'];
				$obj->HK = $dataExcel['HK'];
				$obj->WBSCode = $dataExcel['WBSCode'];
				$obj->Log = sprintf(config('message.msg_sches_makenittei_016'), $mesLog);
				$obj->AMDFlag = 1;
				$obj->KeshiPattern = is_null($dataExcel['KeshiPattern']) ? NULL 
									: FuncCommon::getSplitChar($dataExcel['KeshiPattern'])[0];
				$obj->KeshiCode = is_null($dataExcel['KeshiCode']) ? NULL 
								: FuncCommon::getSplitChar($dataExcel['KeshiCode'])[0];
				$obj->save();
			}else {				// 値がすべて一致する場合
				$id--;
			}
		}
	}

	/**
	* function get data cell date
	*
	* @param mixed valDate
	* @return mixed
	*
	* @create 2020/01/12 Cuong
	* @update
	*/
	private function getSDate($valDate){
		if(is_null($valDate)) {
			return null;
		}

		if(is_numeric($valDate)) {
			return \PhpOffice\PhpSpreadsheet\Shared\Date::excelToDateTimeObject($valDate)->format('Y/m/d');
		}

		return $valDate;
	}

	/**
	* function check is date
	*
	* @param date
	* @return mixed
	*
	* @create 2020/01/12 Cuong
	* @update
	*/
	private function isRealDate($strDate) {
		$blnFlag = true;
		$arrTmpDate = explode('/', $strDate);
		if(count($arrTmpDate) == 3) {
			$arrCheckkHasHis = explode(' ', $arrTmpDate[2]);
			if(count($arrCheckkHasHis) == 1) {
				if(!ctype_digit($arrTmpDate[0]) || !ctype_digit($arrTmpDate[1]) || !ctype_digit($arrTmpDate[2])) {
					$blnFlag = false;
				} else {
					preg_match('/([1-9]\d{3}\/([1-9]|0[1-9]|1[0-2])\/([1-9]|0[1-9]|[12]\d|3[01]))/', $strDate, $arrCheckFormatDate);
					if(count($arrCheckFormatDate) == 0) {
						$blnFlag = false;
					} else {
						$blnCheckDate = checkdate($arrTmpDate[1], $arrTmpDate[2], $arrTmpDate[0]);
						if(!$blnCheckDate) {
							$blnFlag = false;
						}
					}
				}
			}
		} else {
			$blnFlag = false;
		}
		return $blnFlag;
	}

	/**
	* read data sheet nittei method
	*
	* @param worksheet
	* @return mixed
	*
	* @create 2020/01/12 Cuong
	* @update
	*/
	private function readDataSheetNittei($worksheet) {
		$beginRow = 2;
		$colB = 'B';
		$valCellColB = $worksheet->getCell($colB.$beginRow)->getValue();
		$rowIndex = $beginRow ;
		// read data sheet Nittei from excel.
		$dataNitteiExcel = array();
		while(!is_null($valCellColB)) {
			$tempData = array();
			$tempData['DeleteFlag'] = is_null($worksheet->getCell('A'.$rowIndex)->getValue()) ? NULL 
										: $worksheet->getCell('A'.$rowIndex)->getValue();
			$tempData['DispName1'] = is_null($worksheet->getCell('B'.$rowIndex)->getValue()) ? NULL 
										: $worksheet->getCell('B'.$rowIndex)->getValue();
			$tempData['DispName2'] = is_null($worksheet->getCell('C'.$rowIndex)->getValue()) ? NULL 
										: $worksheet->getCell('C'.$rowIndex)->getValue();
			$tempData['DispName3'] = is_null($worksheet->getCell('D'.$rowIndex)->getValue()) ? NULL 
										: $worksheet->getCell('D'.$rowIndex)->getValue();
			$tempData['KakoKumiku'] = is_null($worksheet->getCell('E'.$rowIndex)->getValue()) ? NULL 
										: $worksheet->getCell('E'.$rowIndex)->getValue();
			$tempData['AcFloor'] = is_null($worksheet->getCell('F'.$rowIndex)->getValue()) ? NULL 
										: $worksheet->getCell('F'.$rowIndex)->getValue();
			$tempData['AcMac'] = is_null($worksheet->getCell('G'.$rowIndex)->getValue()) ? NULL 
										: $worksheet->getCell('G'.$rowIndex)->getValue();
			$tempData['DistCode'] = is_null($worksheet->getCell('H'.$rowIndex)->getValue()) ? NULL 
										: $worksheet->getCell('H'.$rowIndex)->getValue();
			$tempData['BD_Code'] = is_null($worksheet->getCell('I'.$rowIndex)->getValue()) ? NULL 
										: $worksheet->getCell('I'.$rowIndex)->getValue();
			$tempData['KeshiCode'] = is_null($worksheet->getCell('J'.$rowIndex)->getValue()) ? NULL 
										: $worksheet->getCell('J'.$rowIndex)->getValue();
			$tempData['KeshiPattern'] = is_null($worksheet->getCell('K'.$rowIndex)->getValue()) ? NULL 
										: $worksheet->getCell('K'.$rowIndex)->getValue();
			$tempData['MngBData'] = is_null($worksheet->getCell('L'.$rowIndex)->getValue()) ? NULL 
										: $worksheet->getCell('L'.$rowIndex)->getValue();
			$tempData['sDate'] = is_null($worksheet->getCell('M'.$rowIndex)->getValue()) ? NULL 
										: $worksheet->getCell('M'.$rowIndex)->getValue();
			$tempData['sDateType'] = is_null($worksheet->getCell('M'.$rowIndex)->getValue()) ? NULL 
										: $worksheet->getCell('M'.$rowIndex);
			$tempData['eDate'] = is_null($worksheet->getCell('N'.$rowIndex)->getValue()) ? NULL 
										: $worksheet->getCell('N'.$rowIndex)->getValue();
			$tempData['Item'] = is_null($worksheet->getCell('O'.$rowIndex)->getValue()) ? NULL 
										: $worksheet->getCell('O'.$rowIndex)->getValue();
			$tempData['HC'] = is_null($worksheet->getCell('P'.$rowIndex)->getValue()) ? NULL 
										: $worksheet->getCell('P'.$rowIndex)->getValue();
			$tempData['HJ'] = is_null($worksheet->getCell('Q'.$rowIndex)->getValue()) ? NULL 
										: $worksheet->getCell('Q'.$rowIndex)->getValue();
			$tempData['HS'] = is_null($worksheet->getCell('R'.$rowIndex)->getValue()) ? NULL 
										: $worksheet->getCell('R'.$rowIndex)->getValue();
			$tempData['HK'] = is_null($worksheet->getCell('S'.$rowIndex)->getValue()) ? NULL 
										: $worksheet->getCell('S'.$rowIndex)->getValue();
			$tempData['WBSCode'] = is_null($worksheet->getCell('T'.$rowIndex)->getValue()) ? NULL 
										: $worksheet->getCell('T'.$rowIndex)->getValue();
			$dataNitteiExcel[$rowIndex] = $tempData;
			$rowIndex ++;
			$valCellColB = $worksheet->getCell($colB.$rowIndex)->getValue();
		}

		return $dataNitteiExcel;
	}

	/**
	* read data sheet master method
	*
	* @param worksheet
	* @return mixed
	*
	* @create 2020/01/12 Cuong
	* @update
	*/
	private function readDataSheetMaster($worksheet) {
		$dataMasterExcel = array();
		$arrColName = array('A', 'B', 'C', 'D', 'E', 'F', 'G');
		foreach($arrColName as $colName) {
			$dataCols = $this->readDataCol($worksheet, $colName);
			$dataMasterExcel[$colName] = $dataCols;
		}

		return $dataMasterExcel;
	}

	/**
	* function check is date
	*
	* @param worksheet
	* @param string colName
	* @return array
	*
	* @create 2020/01/12 Cuong
	* @update
	*/
	private function readDataCol($worksheet, $colName) {
		$beginRow = 2;
		$rowIndex = $beginRow ;
		$tempData = array();
		$valCell = $worksheet->getCell($colName.$beginRow)->getValue();
		while(!is_null($valCell)) {
			array_push($tempData, $valCell);
			$rowIndex ++;
			$valCell = $worksheet->getCell($colName.$rowIndex)->getValue();
		}

		return $tempData;
	}

	/**
	* check contents excel file method
	*
	* @param  array dataExcel
	* @param  Request request
	* @return mixed
	*
	* @create 2020/01/12 Cuong
	* @update
	*/
	private function checkContentsExcel($dataExcel, $request) {
		$dataNitteiExcel = $dataExcel[config('system_const_sches.sches_nitteisheet_name')];
		$headerSheetNittei = $dataExcel['headerSheetNittei'];
		if(count($dataNitteiExcel) > 0) {
			$id = 0;
			foreach($dataNitteiExcel as $row=>$item) {
				$messageLog = '';
				/* 削除フラグ */
				if(!is_null($item['DeleteFlag']) ) {	//「$エクセルデータ」[DeleteFlag]に値が設定されている場合
					if($this->dataDeleteFlagNotNull($dataExcel, $item, $headerSheetNittei, $row, $id, $request)) {
						continue;
					}
				}

				/* 必須チェック */
				if($this->needToCheck($dataExcel, $item, $headerSheetNittei, $row, $id, $request)) {
					continue;
				}

				/* 型の確認 */
				if($this->typeConfirm($dataExcel, $item, $headerSheetNittei, $row, $id, $request)) {
					continue;
				}

				/* 入力桁数の確認 */
				if($this->confirmInputLenght($dataExcel, $item, $headerSheetNittei, $row, $id, $request)) {
					continue;
				}

				/* マスタデータとの一致確認 */
				if($this->confirmWithMasterData($dataExcel, $item, $headerSheetNittei, $row, $id, $request)) {
					continue;
				}

				/* 重複の確認 */
				$groupKey = $item['DispName1'].$item['DispName2'].$item['DispName3'];
				$existGrName = array_filter($dataNitteiExcel, function($value) use($groupKey) {
					return $value['DispName1'].$value['DispName2'].$value['DispName3'] == $groupKey;
				});
				if(count($existGrName) > 1) {
					$mes = '';
					foreach($existGrName as $keyRow=>$val) {
						if($keyRow != $row) {
							if($mes != '') 
							{ 
								continue;	
							}else {
								$mes = $keyRow;
							}
						}
					}
					$messageLog = sprintf(config('message.msg_sches_makenittei_009'), $row, $mes);
					$this->registerImportLog($item, $headerSheetNittei, $row, $id, $messageLog, $request);
					continue;
				}

				$this->registerImportLog($item, $headerSheetNittei, $row, $id, $messageLog, $request);
			}
		}
	}

	/**
	* when data delete flag is not null method
	*
	* @param array dataExcel
	* @param array item
	* @param array headerSheetNittei
	* @param int row
	* @param int id
	* @param Request request
	* @return bool
	*
	* @create 2020/01/12 Cuong
	* @update
	*/
	private function dataDeleteFlagNotNull($dataExcel, $item, $headerSheetNittei, $row, &$id, $request) {
		$flagContinue = false;
		$projectID = $request->val2;			//projectID
		$order = $request->val3;				//order
		$val4 = $request->val4;					//データ区分

		if($item['DeleteFlag'] != config('system_const_sches.deleteflag')) {	//「$エクセルデータ」[DeleteFlag]が「D」以外の場合
			$messageLog = sprintf(config('message.msg_sches_makenittei_003'), $row, '削除フラグ');
			$this->registerImportLog($item, $headerSheetNittei, $row, $id, $messageLog, $request);
			return $flagContinue = true;
		}

		// 実績データの場合
		$messageLog = sprintf(config('message.msg_sches_makenittei_004'), $row);
		$query = S_JobData::where('S_JobData.ProjectID', '=', 0)
						->where('S_JobData.BaseID', '=', $projectID)
						->where('S_JobData.OrderNo', '=', $order)
						->where('S_JobData.Level_Job', '=', 3)
						->where('S_JobData.DispName1 ', '=', $item['DispName1']);

		if(is_null($item['DispName2'])) {
			$query->whereNull('S_JobData.DispName2');
		}else {
			$query->where('S_JobData.DispName2 ', '=', $item['DispName2']);
		}

		if(is_null($item['DispName3'])) {
			$query->whereNull('S_JobData.DispName3');
		}else {
			$query->where('S_JobData.DispName3 ', '=', $item['DispName3']);
		}
		$query->where(function($query1) {
			$query1->where('S_JobData.AcHr', '>', 0)
				->orWhere('S_JobData.PrRateNow', '>', 0);
		});
		if($val4 == 0) {
			$query->where('S_JobData.IsOriginal', '=', 0);
		}else {
			$query->where('S_JobData.IsOriginal', '=', 1);
		}
		$getSJobData = $query->get();

		if(count($getSJobData) > 0) {
			$this->registerImportLog($item, $headerSheetNittei, $row, $id, $messageLog, $request);
			return $flagContinue = true;
		}

		// 名称1,2,3の組合せが存在しない場合
		$messageLog = sprintf(config('message.msg_sches_makenittei_005'), $row);
		$query = S_JobData::where('S_JobData.ProjectID', '=', $projectID)
		->where('S_JobData.OrderNo', '=', $order)
		->where('S_JobData.Level_Job', '=', 3)
		->where('S_JobData.DispName1 ', '=', $item['DispName1']);

		if(is_null($item['DispName2'])) {
			$query->whereNull('S_JobData.DispName2');
		}else {
			$query->where('S_JobData.DispName2 ', '=', $item['DispName2']);
		}

		if(is_null($item['DispName3'])) {
			$query->whereNull('S_JobData.DispName3');
		}else {
			$query->where('S_JobData.DispName3 ', '=', $item['DispName3']);
		}

		if($val4 == 0) {
			$query->where('S_JobData.IsOriginal', '=', 0);
		}else {
			$query->where('S_JobData.IsOriginal', '=', 1);
		}
		$getSJobData = $query->get();

		if(count($getSJobData) > 0) {
			$this->registerImportLog($item, $headerSheetNittei, $row, $id, $messageLog, $request);
			return $flagContinue = true;
		}

		return $flagContinue;
	}

	/**
	* 必須チェック
	*
	* @param array dataExcel
	* @param array item
	* @param array headerSheetNittei
	* @param int row
	* @param int id
	* @param Request request
	* @return bool
	*
	* @create 2020/01/12 Cuong
	* @update
	*/
	private function needToCheck($dataExcel, $item, $headerSheetNittei, $row, &$id, $request) {
		// 「$エクセルデータ」[sDate]、[eDate]、[KeshiCode]、[KeshiPattern]のいずれか１つでもNULLの場合
		$flagContinue = false;
		$hasItemNull = false;
		$mes = '';
		if(is_null($item['sDate'])) {
			empty($mes) ? $mes .= $headerSheetNittei['sDate']
						: $mes .= ','.$headerSheetNittei['sDate'];
			$hasItemNull = true;
		}
		if(is_null($item['eDate'])) {
			empty($mes) ? $mes .= $headerSheetNittei['eDate']
						: $mes .= ','.$headerSheetNittei['eDate'];
			$hasItemNull = true;
		}
		if(is_null($item['KeshiCode'])) {
			empty($mes) ? $mes .= $headerSheetNittei['KeshiCode']
						: $mes .= ','.$headerSheetNittei['KeshiCode'];
			$hasItemNull = true;
		}
		if(is_null($item['KeshiPattern'])) {
			empty($mes) ? $mes .= $headerSheetNittei['KeshiPattern']
						: $mes .= ','.$headerSheetNittei['KeshiPattern'];
			$hasItemNull = true;
		}
		if($hasItemNull) {
			$messageLog = sprintf(config('message.msg_sches_makenittei_006'), $row, $mes);
			$this->registerImportLog($item, $headerSheetNittei, $row, $id, $messageLog, $request);
			return $flagContinue = true;
		}
		//「$エクセルデータ」[DispName2]がNULLかつ[DispName3]に値が設定されている場合
		if(is_null($item['DispName2']) && !is_null($item['DispName3'])) {
			$messageLog = sprintf(config('message.msg_sches_makenittei_011'), $row, $headerSheetNittei['DispName2']);
			$this->registerImportLog($item, $headerSheetNittei, $row, $id, $messageLog, $request);
			return $flagContinue = true;
		}

		return $flagContinue;
	}

	/**
	* 型の確認
	*
	* @param array dataExcel
	* @param array item
	* @param array headerSheetNittei
	* @param int row
	* @param int id
	* @param Request request
	* @return bool
	*
	* @create 2020/01/12 Cuong
	* @update
	*/
	private function typeConfirm($dataExcel, $item, $headerSheetNittei, $row, &$id, $request) {
		// 「$エクセルデータ」[MngBData]、[HC]、[HJ]、[HS]、[HK]が正数値以外の場合
		$flagContinue = false;
		$isNotPositive = false;
		$mes = '';
		if($item['MngBData'] < 0) {
			empty($mes) ? $mes .= $headerSheetNittei['MngBData']
						: $mes .= ','.$headerSheetNittei['MngBData'];
			$isNotPositive = true;
		}
		if($item['HC'] < 0) {
			empty($mes) ? $mes .= $headerSheetNittei['HC']
						: $mes .= ','.$headerSheetNittei['HC'];
			$isNotPositive = true;
		}
		if($item['HJ'] < 0) {
			empty($mes) ? $mes .= $headerSheetNittei['HJ']
						: $mes .= ','.$headerSheetNittei['HJ'];
			$isNotPositive = true;
		}
		if($item['HS'] < 0) {
			empty($mes) ? $mes .= $headerSheetNittei['HS']
						: $mes .= ','.$headerSheetNittei['HS'];
			$isNotPositive = true;
		}
		if($item['HK'] < 0) {
			empty($mes) ? $mes .= $headerSheetNittei['HK']
						: $mes .= ','.$headerSheetNittei['HK'];
			$isNotPositive = true;
		}
		if($isNotPositive) {
			$messageLog = sprintf(config('message.msg_sches_makenittei_007'), $row, $mes);
			$this->registerImportLog($item, $headerSheetNittei, $row, $id, $messageLog, $request);
			return $flagContinue = true;
		}

		// 「$エクセルデータ」[sDate]、[eDate]がNULLまたは日付形式以外の場合
		$sdate = $this->getSDate($item['sDate']);
		$edate = $this->getSDate($item['eDate']);
		$isDate = false;
		$mes = '';
		if(is_null($sdate) || !$this->isRealDate($sdate)) {
			empty($mes) ? $mes .= $headerSheetNittei['sDate']
						: $mes .= ','.$headerSheetNittei['sDate'];
			$isDate = true;
		}
		if(is_null($edate) || !$this->isRealDate($edate)) {
			empty($mes) ? $mes .= $headerSheetNittei['eDate']
						: $mes .= ','.$headerSheetNittei['eDate'];
			$isDate = true;
		}

		if($isDate) {
			$messageLog = sprintf(config('message.msg_sches_makenittei_011'), $row, $mes);
			$this->registerImportLog($item, $headerSheetNittei, $row, $id, $messageLog, $request);
			return $flagContinue = true;
		}

		//「$エクセルデータ」[Item]、[WBSCode]に半角英数記号以外が含まれている場合
		$isCheckType = false;
		$mes = '';
		if(!(bool) preg_match('/^[\x21-\x7E]+$/',$item['Item'])) {
			empty($mes) ? $mes .= $headerSheetNittei['Item']
						: $mes .= ','.$headerSheetNittei['Item'];
			$isCheckType = true;
		}

		if(!is_null($item['WBSCode']) && !(bool) preg_match('/^[\x21-\x7E]+$/',$item['WBSCode'])) {
			empty($mes) ? $mes .= $headerSheetNittei['WBSCode']
						: $mes .= ','.$headerSheetNittei['WBSCode'];
			$isCheckType = true;
		}
		if($isCheckType) {
			$messageLog = sprintf(config('message.msg_sches_makenittei_007'), $row, $mes);
			$this->registerImportLog($item, $headerSheetNittei, $row, $id, $messageLog, $request);
			return $flagContinue = true;
		}

		return $flagContinue;
	}

	/**
	* 入力桁数の確認
	*
	* @param array dataExcel
	* @param array item
	* @param array headerSheetNittei
	* @param int row
	* @param int id
	* @param Request request
	* @return bool
	*
	* @create 2020/01/12 Cuong
	* @update
	*/
	private function confirmInputLenght($dataExcel, $item, $headerSheetNittei, $row, &$id, $request) {
		$flagContinue = false;
		$isCheckLenghInput = false;
		$mes = '';
		if(!is_null($item['DispName1']) && mb_strlen($item['DispName1']) > 100) {
			empty($mes) ? $mes .= $headerSheetNittei['DispName1']
						: $mes .= ','.$headerSheetNittei['DispName1'];
			$isCheckLenghInput = true;
		}
		if(!is_null($item['DispName2']) && mb_strlen($item['DispName2']) > 100) {
			empty($mes) ? $mes .= $headerSheetNittei['DispName2']
						: $mes .= ','.$headerSheetNittei['DispName2'];
			$isCheckLenghInput = true;
		}
		if(!is_null($item['DispName3']) && mb_strlen($item['DispName3']) > 100) {
			empty($mes) ? $mes .= $headerSheetNittei['DispName3']
						: $mes .= ','.$headerSheetNittei['DispName3'];
			$isCheckLenghInput = true;
		}
		if(!is_null($item['Item']) && mb_strlen($item['Item']) > 5) {
			empty($mes) ? $mes .= $headerSheetNittei['Item']
						: $mes .= ','.$headerSheetNittei['Item'];
			$isCheckLenghInput = true;
		}
		if(!is_null($item['WBSCode']) && mb_strlen($item['WBSCode']) > 500) {
			empty($mes) ? $mes .= $headerSheetNittei['WBSCode']
						: $mes .= ','.$headerSheetNittei['WBSCode'];
			$isCheckLenghInput = true;
		}
		if($isCheckLenghInput) {
			$messageLog = sprintf(config('message.msg_sches_makenittei_008'), $row, $mes);
			$this->registerImportLog($item, $headerSheetNittei, $row, $id, $messageLog, $request);
			return $flagContinue = true;
		}

		return $flagContinue;
	}

	/**
	* マスタデータとの一致確認
	*
	* @param array dataExcel
	* @param array item
	* @param array headerSheetNittei
	* @param int row
	* @param int id
	* @param Request request
	* @return bool
	*
	* @create 2020/01/12 Cuong
	* @update
	*/
	private function confirmWithMasterData($dataExcel, $item, $headerSheetNittei, $row, &$id, $request) {
		$dataMasterExcel = $dataExcel[config('system_const_sches.sches_mastersheet_name')];
		$flagContinue = false;
		$isCheck = false;
		$mes = '';
		if(!is_null($item['KakoKumiku']) && !in_array($item['KakoKumiku'], $dataMasterExcel['A'])) {
			empty($mes) ? $mes .= $headerSheetNittei['KakoKumiku']
						: $mes .= ','.$headerSheetNittei['KakoKumiku'];
			$isCheck = true;
		}
		if(!is_null($item['AcFloor']) && !in_array($item['AcFloor'], $dataMasterExcel['B'])) {
			empty($mes) ? $mes .= $headerSheetNittei['AcFloor']
						: $mes .= ','.$headerSheetNittei['AcFloor'];
			$isCheck = true;
		}
		if(!is_null($item['AcMac']) && !in_array($item['AcMac'], $dataMasterExcel['C'])) {
			empty($mes) ? $mes .= $headerSheetNittei['AcMac']
						: $mes .= ','.$headerSheetNittei['AcMac'];
			$isCheck = true;
		}
		if(!is_null($item['DistCode']) && !in_array($item['DistCode'], $dataMasterExcel['D'])) {
			empty($mes) ? $mes .= $headerSheetNittei['DistCode']
						: $mes .= ','.$headerSheetNittei['DistCode'];
			$isCheck = true;
		}
		if(!is_null($item['BD_Code']) && !in_array($item['BD_Code'], $dataMasterExcel['E'])) {
			empty($mes) ? $mes .= $headerSheetNittei['BD_Code']
						: $mes .= ','.$headerSheetNittei['BD_Code'];
			$isCheck = true;
		}
		if(!is_null($item['KeshiCode']) && !in_array($item['KeshiCode'], $dataMasterExcel['F'])) {
			empty($mes) ? $mes .= $headerSheetNittei['KeshiCode']
						: $mes .= ','.$headerSheetNittei['KeshiCode'];
			$isCheck = true;
		}
		if(!is_null($item['KeshiPattern']) && !in_array($item['KeshiPattern'], $dataMasterExcel['G'])) {
			empty($mes) ? $mes .= $headerSheetNittei['KeshiPattern']
						: $mes .= ','.$headerSheetNittei['KeshiPattern'];
			$isCheck = true;
		}

		if($isCheck) {
			$messageLog = sprintf(config('message.msg_sches_makenittei_011'), $row, $mes);
			$this->registerImportLog($item, $headerSheetNittei, $row, $id, $messageLog, $request);
			return 	$flagContinue = true;
		}

		return $flagContinue;
	}
}
