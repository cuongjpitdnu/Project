<?php
/*
 * @OutputController.php
 * 工程パターン管理画面コントローラーファイル
 *
 * @create 2020/10/02 Chien
 *
 * @update 2020/12/04
 */

namespace App\Http\Controllers\Schem;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Http\Requests\Schem\OutputOutputRequest;
use Illuminate\Support\Facades\DB;
use Illuminate\Database\QueryException;
use Illuminate\Pagination\LengthAwarePaginator;
use App\Librarys\FuncCommon;
use App\Librarys\MenuInfo;
use App\Librarys\TimeTrackerCommon;
use App\Models\MstProject;
use App\Models\MstOrderNo;
use App\Models\T_Tosai;
use App\Models\T_Sogumi;
use App\Models\T_Kyokyu;
use App\Models\Cyn_TosaiData;
use App\Models\Cyn_BlockKukaku;
use App\Models\Cyn_C_BlockKukaku;
use App\Models\Cyn_Plan;
use App\Models\Cyn_C_Plan;
use App\Models\Cyn_mstKotei;
use PhpOffice\PhpSpreadsheet\IOFactory;
use PhpOffice\PhpSpreadsheet\Shared\Date;
use PhpOffice\PhpSpreadsheet\Style\Border;
use PhpOffice\PhpSpreadsheet\Style\Font;
use PhpOffice\PhpSpreadsheet\Style\Fill;
use PhpOffice\PhpSpreadsheet\Style\Alignment;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use Exception;

/*
 * 船殻中日程表出力画面コントローラー
 *
 * @create 2020/10/02 Chien
 *
 * @update
 */

class OutputController extends Controller
{
	/**
	 * construct
	 * @param
	 * @return mixed
	 * @create 2020/10/02 Chien
	 * @update
	 */
	public function __construct() {
	}

	/**
	 * 船殻中日程表出力画面
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/10/02 Chien
	 * @update
	 */
	public function index(Request $request) {
		return $this->initialize($request);
	}

	/**
	 * init data to show page
	 *
	 * @param Request $request
	 * @return View ビュー
	 *
	 * @create 2020/10/02 Chien
	 * @update
	 */
	private function initialize(Request $request) {
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$this->data['menuInfo'] = $menuInfo;

		//initialize $originalError
		$originalError = [];
		$itemShow = array(
			'val1' => isset($request->val1) ? valueUrlDecode($request->val1) :
						((trim(old('val1')) != '') ? valueUrlDecode(old('val1')) : 0),
			'val2' => isset($request->val2) ? valueUrlDecode($request->val2) :
						((trim(old('val2')) != '') ? valueUrlDecode(old('val2')) : ''),
			'val3' => isset($request->val3) ? valueUrlDecode($request->val3) :
						((trim(old('val3')) != '') ? valueUrlDecode(old('val3')) : ''),
			'val4' => isset($request->val4) ? valueUrlDecode($request->val4) :
						((trim(old('val4')) != '') ? valueUrlDecode(old('val4')) : 0),
		);

		// data 2 for val 2
		$data2 = $this->getDataVal2($menuInfo, $itemShow['val1']);
		$this->data['dataView']['data_2'] = $data2;
		$this->data['dataView']['data_2_all'] = $this->getDataVal2($menuInfo, '', true);

		$tempVal2 = ($itemShow['val2'] == '') ?
			((count($data2) > 0) ? valueUrlDecode($data2->first()->val2) : $itemShow['val2']) : $itemShow['val2'];

		// data 3 for val 3
		$this->data['dataView']['data_3'] = $this->getDataVal3($itemShow['val1'], $tempVal2);
		$this->data['dataView']['data_3_all'] = $this->getDataVal3('', '', true);

		if (isset($request->err1)) {
			$originalError[] = valueUrlDecode($request->err1);
		}
		$itemShow['val1'] = valueUrlEncode($itemShow['val1']);
		$itemShow['val2'] = valueUrlEncode($tempVal2);
		$itemShow['val3'] = valueUrlEncode($itemShow['val3']);

		//request
		$this->data['request'] = $request;
		$this->data['originalError'] = $originalError;
		$this->data['itemShow'] = $itemShow;
		$this->data['msgTimeOut'] = valueUrlEncode(config('message.msg_cmn_err_002'));
		//return view with all data
		return view('Schem/Output/index', $this->data);
	}

	/* load ajax when change val1 */
	public function ajaxGetVal2(Request $request) {
		$val1 = isset($request->val1) ? valueUrlDecode($request->val1) : '';
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return json_encode(array());
		}
		return json_encode($this->getDataVal2($menuInfo, $val1));
	}

	/**
	 * get data value 2
	 *
	 * @param String $val1
	 * @return Object mixed
	 *
	 * @create 2020/10/02 Chien
	 * @update
	 */
	private function getDataVal2($menuInfo, $value = '', $loadAll = false) {
		$data = MstProject::select('ID as val2', 'ProjectName as val2Name', 'ListKind')
			->where('SysKindID', '=', $menuInfo->KindID);
		$data = ($value !== '') ? $data->where('ListKind', '=', $value) : $data;
		$data = $data->orderBy('ProjectName')->get();

		if (count($data) > 0) {
			foreach ($data as &$row) {
				$row->val2 = valueUrlEncode($row->val2);
				$row->ListKind = valueUrlEncode($row->ListKind);
				$row->val2Name = ($loadAll) ? htmlentities($row->val2Name) : $row->val2Name;
			}
		}
		return $data;
	}

	/* load ajax when change projectId at val2 */
	public function ajaxGetVal3(Request $request) {
		$val1 = isset($request->val1) ? valueUrlDecode($request->val1) : 0;
		$val2 = isset($request->val2) ? valueUrlDecode($request->val2) : '';
		return json_encode($this->getDataVal3($val1, $val2));
	}

	/**
	 * get data value 3
	 *
	 * @param String $val2
	 * @return Object mixed
	 *
	 * @create 2020/10/02 Chien
	 * @update
	 */
	private function getDataVal3($val1 = 0, $val2 = '', $loadAll = false) {
		$data = Cyn_BlockKukaku::select('Cyn_BlockKukaku.OrderNo as val3', 'Cyn_BlockKukaku.CKind', 'Cyn_BlockKukaku.ProjectID')
								->join('mstOrderNo', 'Cyn_BlockKukaku.OrderNo', '=', 'mstOrderNo.OrderNo');
		$data = ($val1 !== '') ? $data->where('Cyn_BlockKukaku.CKind', '=', $val1) : $data;
		$data = ($val2 !== '') ? $data->where('Cyn_BlockKukaku.ProjectID', '=', $val2) : $data;
		$data = $data->where('mstOrderNo.DispFlag', '=', 0)->orderBy('Cyn_BlockKukaku.OrderNo')->distinct()->get();

		if (count($data) > 0) {
			foreach ($data as &$row) {
				$row->val3Name = ($loadAll) ? htmlentities($row->val3) : $row->val3;
				$row->ProjectID = valueUrlEncode($row->ProjectID);
				$row->CKind = valueUrlEncode($row->CKind);
				$row->val3 = valueUrlEncode($row->val3);
			}
		}

		return $data;
	}

	/**
	 * POST export file excel + update data (optional)
	 *
	 * @param OutputOutputRequest 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/10/02 Chien
	 * @update 2020/12/04 Chien
	 */
	public function output(OutputOutputRequest $request) {
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}
		//validate form
		$validated = $request->validated();

		// data 1
		$data_1 = Cyn_TosaiData::select(
									'Cyn_Plan.B_PlSDate',
									'Cyn_Plan.B_SG_Date',
									'Cyn_Plan.B_T_Date',
									'Cyn_Plan.Kotei',
									'Cyn_Plan.KKumiku',
									'Cyn_Plan.WorkItemID',
									'Cyn_Plan.Del_Date as DDChild',

									'Cyn_Plan.ProjectID as CPProjectID',
									'Cyn_Plan.OrderNo as CPOrderNo',
									'Cyn_Plan.No as CPNo',
									'Cyn_Plan.KoteiNo as CPKoteiNo',

									'Cyn_BlockKukaku.Del_Date',
									'Cyn_BlockKukaku.Name',
									'Cyn_BlockKukaku.N_No',
									'Cyn_BlockKukaku.Zu_No',
									'Cyn_BlockKukaku.BKumiku',

									'Cyn_BlockKukaku.ProjectID as CBProjectID',
									'Cyn_BlockKukaku.OrderNo as CBOrderNo',
									'Cyn_BlockKukaku.No as CBNo',

									'Cyn_TosaiData.T_Date'
								)
								->selectRaw('\'1\' as GroupData')
								->join('Cyn_BlockKukaku', function ($join) {
									$join->on('Cyn_TosaiData.ProjectID', '=', 'Cyn_BlockKukaku.ProjectID')
										->on('Cyn_TosaiData.OrderNo', '=', 'Cyn_BlockKukaku.OrderNo')
										->on('Cyn_TosaiData.Name', '=', 'Cyn_BlockKukaku.T_Name')
										->on('Cyn_TosaiData.BKumiku', '=', 'Cyn_BlockKukaku.T_BKumiku');
								})
								->join('Cyn_Plan', function ($join) {
									$join->on('Cyn_BlockKukaku.ProjectID', '=', 'Cyn_Plan.ProjectID')
										->on('Cyn_BlockKukaku.OrderNo', '=', 'Cyn_Plan.OrderNo')
										->on('Cyn_BlockKukaku.No', '=', 'Cyn_Plan.No');
								})
								->where('Cyn_TosaiData.ProjectID', '=', $validated['val2'])
								->where('Cyn_TosaiData.OrderNo', '=', $validated['val3'])
								->where('Cyn_TosaiData.CKind', '=', $validated['val1'])
								->where('Cyn_TosaiData.WorkItemID', '>', 0);

		// data 2
		$data_2 = Cyn_TosaiData::select(
									'Cyn_C_Plan.B_PlSDate',
									'Cyn_C_Plan.B_SG_Date',
									'Cyn_C_Plan.B_T_Date',
									'Cyn_C_Plan.Kotei',
									'Cyn_C_Plan.KKumiku',
									'Cyn_C_Plan.WorkItemID',
									'Cyn_C_Plan.Del_Date as DDChild',

									'Cyn_C_Plan.ProjectID as CPProjectID',
									'Cyn_C_Plan.OrderNo as CPOrderNo',
									'Cyn_C_Plan.No as CPNo',
									'Cyn_C_Plan.KoteiNo as CPKoteiNo',

									'Cyn_C_BlockKukaku.Del_Date',
									'Cyn_C_BlockKukaku.Name',
									'Cyn_C_BlockKukaku.N_No',
									'Cyn_C_BlockKukaku.Zu_No',
									'Cyn_C_BlockKukaku.BKumiku',

									'Cyn_C_BlockKukaku.ProjectID as CBProjectID',
									'Cyn_C_BlockKukaku.OrderNo as CBOrderNo',
									'Cyn_C_BlockKukaku.No as CBNo',

									'Cyn_TosaiData.T_Date',
								)
								->selectRaw('\'2\' as GroupData')
								->join('Cyn_BlockKukaku', function ($join) {
									$join->on('Cyn_TosaiData.ProjectID', '=', 'Cyn_BlockKukaku.ProjectID')
										->on('Cyn_TosaiData.OrderNo', '=', 'Cyn_BlockKukaku.OrderNo')
										->on('Cyn_TosaiData.Name', '=', 'Cyn_BlockKukaku.T_Name')
										->on('Cyn_TosaiData.BKumiku', '=', 'Cyn_BlockKukaku.T_BKumiku');
								})
								->join('Cyn_C_BlockKukaku', function ($join) {
									$join->on('Cyn_BlockKukaku.ProjectID', '=', 'Cyn_C_BlockKukaku.ProjectID')
										->on('Cyn_BlockKukaku.OrderNo', '=', 'Cyn_C_BlockKukaku.OrderNo')
										->on('Cyn_BlockKukaku.CKind', '=', 'Cyn_C_BlockKukaku.CKind')
										->on('Cyn_BlockKukaku.Name', '=', 'Cyn_C_BlockKukaku.T_Name')
										->on('Cyn_BlockKukaku.BKumiku', '=', 'Cyn_C_BlockKukaku.T_BKumiku');
								})
								->join('Cyn_C_Plan', function ($join) {
									$join->on('Cyn_C_BlockKukaku.ProjectID', '=', 'Cyn_C_Plan.ProjectID')
										->on('Cyn_C_BlockKukaku.OrderNo', '=', 'Cyn_C_Plan.OrderNo')
										->on('Cyn_C_BlockKukaku.No', '=', 'Cyn_C_Plan.No');
								})
								->where('Cyn_BlockKukaku.ProjectID', '=', $validated['val2'])
								->where('Cyn_BlockKukaku.OrderNo', '=', $validated['val3'])
								->where('Cyn_BlockKukaku.CKind', '=', $validated['val1'])
								->where('Cyn_TosaiData.WorkItemID', '=', 0);	// update rev8

		// initial url
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

		$data = $data_1->union($data_2)->orderBy('Name')->get();

		if (count($data) == 0) {
			// has error
			$url .= '&err1=' . valueUrlEncode(config('message.msg_cmn_db_001'));
			return redirect($url);
		}

		// read file excel template
		$inputFileType = 'Xlsx';
		$inputFileName = config('system_const_schem.output_template_path');

		if ($inputFileName != '') {
			$arrPath = explode('/', $inputFileName);
			$arrLength = count($arrPath);

			if ($arrLength > 2) {
				$inputFileName = public_path() . '\\' . $arrPath[$arrLength - 2] . '\\' . $arrPath[$arrLength - 1];

				if (!file_exists($inputFileName)) {
					// has error
					$url .= '&err1=' . valueUrlEncode(config('message.msg_cmn_db_001'));
					return redirect($url);
				}

				$msgTimeTrackerFailure = '';
				$TimeTrackerCommon = new TimeTrackerCommon();

				// update rev7 - getCalendar
				$dataGetCalendar = $TimeTrackerCommon->getCalendar($validated['val2']);	// update rev8
				if ($dataGetCalendar != '' && is_string($dataGetCalendar)) {
					// error
					$url .= '&err1=' . valueUrlEncode($dataGetCalendar);
					return redirect($url);
				}

				$reader = IOFactory::createReader($inputFileType);
				$spreadsheet = $reader->load($inputFileName);
				$worksheet = $spreadsheet->getActiveSheet();

				// set date Q2
				$worksheet->setCellValue('Q2', '発行日：' . date('Y/m/d'));

				// set オーダ A2
				$worksheet->setCellValue('A2', 'オーダ' . $validated['val3']);

				// array list export
				$arrListExport = array();
				$arrBeforeMergeRow = array();

				// design đata
				$arrDelDate = array();
				$arrRangeYear = array();
				$groupBlockName = array();
				$arrDataLastNNo = array();	// fix bug 293
				foreach ($data as &$item) {
					$dataTemp = array();
					$tempOutFlag1 = Cyn_mstKotei::where('Code', '=', $item->Kotei)
												->where('CKind', '=', $validated['val1'])
												->where('OutFlag', '=', config('system_const_schem.kotei_outflag_001'))->first();
					$tempOutFlag2 = Cyn_mstKotei::where('Code', '=', $item->Kotei)
												->where('CKind', '=', $validated['val1'])
												->where('OutFlag', '=', config('system_const_schem.kotei_outflag_002'))->first();

					$dataTemp['key'] = sprintf(
						'%s_%s_%s/%s_%s_%s_%s',
						$item->CBProjectID,
						$item->CBOrderNo,
						$item->CBNo,
						$item->CPProjectID,
						$item->CPOrderNo,
						$item->CPNo,
						$item->CPKoteiNo,
					);

					// fix bug 293
					$dataTemp['keyParent'] = sprintf(
						'%s_%s_%s',
						$item->CBProjectID,
						$item->CBOrderNo,
						$item->CBNo,
					);
					if (!isset($arrDataLastNNo[$dataTemp['keyParent']])) {
						$arrDataLastNNo[$dataTemp['keyParent']] = array();
					}

					$dataTemp['GroupData'] = $item->GroupData;

					$dataTemp['OutFlag'] = ($tempOutFlag1 != null) ? 1 : (($tempOutFlag2 != null) ? 2 : '');

					$dataTemp['Kotei'] = $item->Kotei;

					$dataTemp['KKumiku'] = $item->KKumiku;

					$dataTemp['No'] = $item->CPNo;
					$dataTemp['KoteiNo'] = $item->CPKoteiNo;

					if (!in_array($item->Del_Date, $arrDelDate) && ($item->Del_Date != null || $item->Del_Date != '')) {
						$arrDelDate[] = $item->Del_Date;
					}

					$dataTemp['DDChild'] = $item->DDChild;

					// col B
					$dataTemp['colB'] = '';
					if ($item->B_PlSDate == '' && $item->B_SGDate == '' && $item->B_T_Date == '') {
						$dataTemp['colB'] = '☆';
					}
					if(!empty($item->Del_Date)) {
						$dataTemp['colB'] = '×';
					}

					// update rev8
					$dataTemp['flagHasDataWrong'] = ($item->B_PlSDate == '' && $item->B_SGDate == '' &&
													$item->B_T_Date == '' && !empty($item->Del_Date)) ? 1 : -1;

					$dataTemp['Group6Char'] = mb_substr($item->Name, 0, 6);

					// col C
					$dataTemp['colC'] = mb_substr($item->Name, 0, 12);
					if (mb_strlen($item->Name) >= 13 && !in_array(mb_substr($item->Name, 12, 1), array('P', 'S', 'C'))) {
						$dataTemp['colC'] = $item->Name;
					}

					// col D - Gen
					$dataTemp['colD'] = '';
					if (mb_strlen($item->Name) >= 13 && in_array(mb_substr($item->Name, 12, 1), array('P', 'S', 'C'))) {
						$dataTemp['colD'] = mb_substr($item->Name, 12, 1);
					}

					// store group follow with block name
					if (!in_array(array($dataTemp['colC'], $dataTemp['colD']), $groupBlockName)) {
						$groupBlockName[] = array($dataTemp['colC'], $dataTemp['colD']);
					}

					// col E
					$dataTemp['colE'] = '';
					$dataTemp['timeColE'] = '';
					if (in_array($item->KKumiku, array(
						config('system_const.kumiku_code_kogumi'),
						config('system_const.kumiku_code_naicyu')
					)) && $item->B_PlSDate != '' && $tempOutFlag1 != null) {
						$dataTemp['colE'] = Date::PHPToExcel(date("Y-m-d", strtotime($item->B_PlSDate)));
						$dataTemp['timeColE'] = $item->B_PlSDate;
					}

					// col F
					$dataTemp['colF'] = '';
					$dataTemp['timeColF'] = '';
					if (in_array($item->KKumiku, array(
						config('system_const.kumiku_code_kogumi'),
						config('system_const.kumiku_code_naicyu')
					)) && $item->WorkItemID != '' && $tempOutFlag1 != null) {
						$dataGetKoteiRange = $TimeTrackerCommon->getKoteiRange(array($item->WorkItemID));
						// update rev6 - 2020/10/21
						if (is_string($dataGetKoteiRange) && !empty($dataGetKoteiRange)) {
							$msgTimeTrackerFailure = $dataGetKoteiRange;
							break;
						}
						if (is_array($dataGetKoteiRange) && isset($dataGetKoteiRange[$item->WorkItemID]) &&
							is_numeric(strtotime($dataGetKoteiRange[$item->WorkItemID]['plannedStartDate']))
							&& empty($item->Del_Date) && empty($item->DDChild)) {
							$dataTemp['colF'] = Date::PHPToExcel(date("Y-m-d", strtotime($dataGetKoteiRange[$item->WorkItemID]['plannedStartDate'])));
							$dataTemp['timeColF'] = $dataGetKoteiRange[$item->WorkItemID]['plannedStartDate'];
						}
					}

					// col G
					$dataTemp['colG'] = '';
					if ($dataTemp['colE'] != '' && $dataTemp['colF'] != '') {
						$dataGetDiffDate = $TimeTrackerCommon->getDateDiff($dataGetCalendar, $dataTemp['timeColE'], $dataTemp['timeColF']);
						if ($dataGetDiffDate != '') {
							if (is_string($dataGetDiffDate)) {
								$msgTimeTrackerFailure = $dataGetDiffDate;
								break;
							}
							if (is_int($dataGetDiffDate)) {
								$dataTemp['colG'] = $dataGetDiffDate;
							}
						}
					}

					// col H
					$dataTemp['colH'] = '';
					$dataTemp['timeColH'] = '';
					if (in_array($item->KKumiku, array(
						config('system_const.kumiku_code_ogumi'),
						config('system_const.kumiku_code_kumicyu')
					)) && $item->B_PlSDate != '' && $tempOutFlag1 != null) {
						$dataTemp['colH'] = Date::PHPToExcel(date("Y-m-d", strtotime($item->B_PlSDate)));
						$dataTemp['timeColH'] = $item->B_PlSDate;
					}

					// col I
					$dataTemp['colI'] = '';
					$dataTemp['timeColI'] = '';
					if (in_array($item->KKumiku, array(
						config('system_const.kumiku_code_ogumi'),
						config('system_const.kumiku_code_kumicyu')
					)) && $item->WorkItemID != '' && $tempOutFlag1 != null) {
						$dataGetKoteiRange = $TimeTrackerCommon->getKoteiRange(array($item->WorkItemID));
						// update rev6 - 2020/10/21
						if (is_string($dataGetKoteiRange) && !empty($dataGetKoteiRange)) {
							$msgTimeTrackerFailure = $dataGetKoteiRange;
							break;
						}
						if (is_array($dataGetKoteiRange) && isset($dataGetKoteiRange[$item->WorkItemID]) &&
							is_numeric(strtotime($dataGetKoteiRange[$item->WorkItemID]['plannedStartDate']))
							&& empty($item->Del_Date) && empty($item->DDChild)) {
							$dataTemp['colI'] = Date::PHPToExcel(date("Y-m-d", strtotime($dataGetKoteiRange[$item->WorkItemID]['plannedStartDate'])));
							$dataTemp['timeColI'] = $dataGetKoteiRange[$item->WorkItemID]['plannedStartDate'];
						}
					}

					// col J
					$dataTemp['colJ'] = '';
					if ($dataTemp['colH'] != '' && $dataTemp['colI'] != '') {
						$dataGetDiffDate = $TimeTrackerCommon->getDateDiff($dataGetCalendar, $dataTemp['timeColH'], $dataTemp['timeColI']);
						if ($dataGetDiffDate != '') {
							if (is_string($dataGetDiffDate)) {
								$msgTimeTrackerFailure = $dataGetDiffDate;
								break;
							}
							if (is_int($dataGetDiffDate)) {
								$dataTemp['colJ'] = $dataGetDiffDate;
							}
						}
					}

					// col K
					$dataTemp['colK'] = '';
					$dataTemp['timeColK'] = '';
					if (in_array($item->KKumiku, array(
						config('system_const.kumiku_code_kogumi'),
						config('system_const.kumiku_code_naicyu')
					)) && $item->B_PlSDate != '' && $tempOutFlag2 != null) {
						$dataTemp['colK'] = Date::PHPToExcel(date("Y-m-d", strtotime($item->B_PlSDate)));
						$dataTemp['timeColK'] = $item->B_PlSDate;
					}

					// col L
					$dataTemp['colL'] = '';
					$dataTemp['timeColL'] = '';
					if (in_array($item->KKumiku, array(
						config('system_const.kumiku_code_kogumi'),
						config('system_const.kumiku_code_naicyu')
					)) && $item->WorkItemID != '' && $tempOutFlag2 != null) {
						$dataGetKoteiRange = $TimeTrackerCommon->getKoteiRange(array($item->WorkItemID));
						// update rev6 - 2020/10/21
						if (is_string($dataGetKoteiRange) && !empty($dataGetKoteiRange)) {
							$msgTimeTrackerFailure = $dataGetKoteiRange;
							break;
						}
						if (is_array($dataGetKoteiRange) && isset($dataGetKoteiRange[$item->WorkItemID]) &&
							is_numeric(strtotime($dataGetKoteiRange[$item->WorkItemID]['plannedStartDate']))
							&& empty($item->Del_Date) && empty($item->DDChild)) {
							$dataTemp['colL'] = Date::PHPToExcel(date("Y-m-d", strtotime($dataGetKoteiRange[$item->WorkItemID]['plannedStartDate'])));
							$dataTemp['timeColL'] = $dataGetKoteiRange[$item->WorkItemID]['plannedStartDate'];
						}
					}

					// col M
					$dataTemp['colM'] = '';
					$dataTemp['timeColM'] = '';
					if (in_array($item->KKumiku, array(
						config('system_const.kumiku_code_ogumi'),
						config('system_const.kumiku_code_kumicyu')
					)) && $item->B_PlSDate != '' && $tempOutFlag2 != null) {
						$dataTemp['colM'] = Date::PHPToExcel(date("Y-m-d", strtotime($item->B_PlSDate)));
						$dataTemp['timeColM'] = $item->B_PlSDate;
					}

					// col N
					$dataTemp['colN'] = '';
					$dataTemp['timeColN'] = '';
					if (in_array($item->KKumiku, array(
						config('system_const.kumiku_code_ogumi'),
						config('system_const.kumiku_code_kumicyu')
					)) && $item->WorkItemID != '' && $tempOutFlag2 != null) {
						$dataGetKoteiRange = $TimeTrackerCommon->getKoteiRange(array($item->WorkItemID));
						// update rev6 - 2020/10/21
						if (is_string($dataGetKoteiRange) && !empty($dataGetKoteiRange)) {
							$msgTimeTrackerFailure = $dataGetKoteiRange;
							break;
						}
						if (is_array($dataGetKoteiRange) && isset($dataGetKoteiRange[$item->WorkItemID]) &&
							is_numeric(strtotime($dataGetKoteiRange[$item->WorkItemID]['plannedStartDate']))
							&& empty($item->Del_Date) && empty($item->DDChild)) {
							$dataTemp['colN'] = Date::PHPToExcel(date("Y-m-d", strtotime($dataGetKoteiRange[$item->WorkItemID]['plannedStartDate'])));
							$dataTemp['timeColN'] = $dataGetKoteiRange[$item->WorkItemID]['plannedStartDate'];
						}
					}

					// col O
					$dataTemp['colO'] = '';
					$dataTemp['timeColO'] = '';

					// col P
					$dataTemp['colP'] = '';
					$dataTemp['timeColP'] = '';
					$dataTemp['DDChildColP'] = '';
					if ($item->N_No != '') {
						$temp = $this->getLastNext($item->GroupData, $validated, $item->N_No);
						if ($temp != null) {
							$tempPlan = array();
							if ($item->GroupData == 1) {
								$tempPlan = Cyn_Plan::where('ProjectID', '=', $validated['val2'])
													->where('OrderNo', '=', $validated['val3'])
													->where('No', '=', $temp->No)
													->get();
							} else {
								$tempPlan = Cyn_C_Plan::where('ProjectID', '=', $validated['val2'])
													->where('OrderNo', '=', $validated['val3'])
													->where('No', '=', $temp->No)
													->get();
							}
							$tempPlan = $tempPlan->toArray();
							if (count($tempPlan) > 0) {
								$minDate = '';
								$minBSGDate = '';
								$minDDChild = '';
								foreach ($tempPlan as $itemPlan) {
									$tempCynMstKotei = Cyn_mstKotei::where('Code', '=', $itemPlan['Kotei'])
																	->where('CKind', '=', $validated['val1'])
																	->where('OutFlag', '=', 2)->first();
									if ($tempCynMstKotei != null) {
										$dataGetKoteiRange = $TimeTrackerCommon->getKoteiRange(array($itemPlan['WorkItemID']));
										// update rev6 - 2020/10/21
										if (is_string($dataGetKoteiRange) && !empty($dataGetKoteiRange)) {
											$msgTimeTrackerFailure = $dataGetKoteiRange;
											break;
										}
										if (is_array($dataGetKoteiRange) && isset($dataGetKoteiRange[$itemPlan['WorkItemID']])) {
											$date = $dataGetKoteiRange[$itemPlan['WorkItemID']]['plannedStartDate'];
											if (is_numeric(strtotime($date))) {
												if ($minDate == '') {
													$minDate = $date;
													$minBSGDate = $itemPlan['B_SG_Date'];	// update rev9
													$minDDChild = $itemPlan['Del_Date'];
													// fix bug 293
													$arrDataLastNNo[$dataTemp['keyParent']] = array(
														'projectID' => $itemPlan['ProjectID'],
														'orderNo' => $itemPlan['OrderNo'],
														'no' => $itemPlan['No'],
														'koteiNo' => $itemPlan['KoteiNo'],
													);
												} else {
													if (strtotime($minDate) > strtotime($date)) {
														$minDate = $date;
														$minBSGDate = $itemPlan['B_SG_Date'];	// update rev9
														$minDDChild = $itemPlan['Del_Date'];
														// fix bug 293
														$arrDataLastNNo[$dataTemp['keyParent']][] = array(
															'projectID' => $itemPlan['ProjectID'],
															'orderNo' => $itemPlan['OrderNo'],
															'no' => $itemPlan['No'],
															'koteiNo' => $itemPlan['KoteiNo'],
														);
													}
												}
											}
										}
									}
								}

								// update rev6 - 2020/10/21
								if ($msgTimeTrackerFailure != '') {
									break;
								}
								if (empty($item->Del_Date)) {
									if ($minDate != '') {
										$dataTemp['colP'] = Date::PHPToExcel(date("Y-m-d", strtotime($minDate)));
										$dataTemp['timeColP'] = $minDate;
										$dataTemp['DDChildColP'] = $minDDChild;
									}
									if ($minBSGDate != '') {
										// set value for col O
										$dataTemp['colO'] = Date::PHPToExcel(date("Y-m-d", strtotime($minBSGDate)));
										$dataTemp['timeColO'] = $minBSGDate;
									}
								}
							}
						}
					}

					// col Q
					$dataTemp['colQ'] = ($item->B_T_Date != '') ? Date::PHPToExcel(date("Y-m-d", strtotime($item->B_T_Date))) : '';
					$dataTemp['timeColQ'] = $item->B_T_Date;

					// col R
					$dataTemp['colR'] = ($item->T_Date != '') ? Date::PHPToExcel(date("Y-m-d", strtotime($item->T_Date))) : '';
					$dataTemp['timeColR'] = $item->T_Date;

					// col S
					$dataTemp['colS'] = $item->Zu_No;

					$arrBeforeMergeRow[$dataTemp['Group6Char']][] = $dataTemp;
				}

				// update Rev6 - 2020/10/21
				if ($msgTimeTrackerFailure != '') {
					$url .= '&err1=' . valueUrlEncode($msgTimeTrackerFailure);
					// redirect screen output
					return redirect($url);
				}

				$newArrBeforeMergeRow = array();
				$test = array();
				foreach ($arrBeforeMergeRow as $keyGroup => &$group) {
					if (count($group) > 1) {
						$groupTemp = array();
						$arrCountAllRowDel = array();
						foreach ($groupBlockName as $keyBlock => &$block) {
							foreach ($group as $k => &$row) {
								if (array($row['colC'], $row['colD']) == $block) {
									$groupTemp[$keyBlock][] = $row;
									$test[$keyBlock][] = $row;	// $groupTemp ~ $test
									unset($group[$k]);	// increment performance
								}
							}
						}
						if (count($groupTemp) > 0) {
							foreach ($groupTemp as $newBlock) {
								if (count($newBlock) > 1) {
									// clone
									$temp = $newBlock[0];
									$countTextStarColB = 0;
									$countDataWrong = 0;
									$countUniqueRowInGroup = array();
									foreach ($newBlock as $item) {
										if (!isset($countUniqueRowInGroup[$keyGroup.'_'.$item['No'].'_'.$item['KKumiku'].'_'.$item['OutFlag']])) {
											$countUniqueRowInGroup[$keyGroup.'_'.$item['No'].'_'.$item['KKumiku'].'_'.$item['OutFlag']] = 1;
										} else {
											$countUniqueRowInGroup[$keyGroup.'_'.$item['No'].'_'.$item['KKumiku'].'_'.$item['OutFlag']] += 1;
										}
										// B
										if (!empty($item['colB'])) {
											if ($item['colB'] == '×') {
												$temp['colB'] = $item['colB'];
											}
											if ($item['colB'] == '☆') {
												$countTextStarColB++;
											}
										}

										if ($item['flagHasDataWrong'] != -1) {
											$countDataWrong++;
										}

										if ($temp['colB'] != '×') {
											// H
											if (empty($temp['colH']) && !empty($item['colH'])) {
												$temp['colH'] = $item['colH'];
												$temp['timeColH'] = $item['timeColH'];
											}
											// compare col I get min
											if ($item['DDChild'] != '' && in_array($item['KKumiku'], array(
												config('system_const.kumiku_code_ogumi'),
												config('system_const.kumiku_code_kumicyu'),
											)) && $item['OutFlag'] == config('system_const_schem.kotei_outflag_001')) {
												$arrCountAllRowDel[$keyGroup.'_'.$item['No']][$item['KKumiku']][$item['OutFlag']]['colI'][] = '※';
											} else {
												if (empty($temp['timeColI'])) {
													$temp['colI'] = $item['colI'];
													$temp['timeColI'] = $item['timeColI'];
												} else {
													if (!empty($item['timeColI']) && strtotime($temp['timeColI']) > strtotime($item['timeColI'])) {
														$temp['colI'] = $item['colI'];
														$temp['timeColI'] = $item['timeColI'];
													}
												}
											}
											// J
											if (empty($temp['colJ']) && !empty($item['colJ'])) {
												if ($temp['colH'] != '' && $temp['colH'] != '※' && $temp['colI'] != '' && $temp['colI'] != '※') {
													$temp['colJ'] = $item['colJ'];
												}
											}

											// M
											if (empty($temp['colM']) && !empty($item['colM'])) {
												$temp['colM'] = $item['colM'];
												$temp['timeColM'] = $item['timeColM'];
											}
											// compare col N get min
											if ($item['DDChild'] != '' && in_array($item['KKumiku'], array(
												config('system_const.kumiku_code_ogumi'),
												config('system_const.kumiku_code_kumicyu'),
											)) && $item['OutFlag'] == config('system_const_schem.kotei_outflag_002')) {
												$arrCountAllRowDel[$keyGroup.'_'.$item['No']][$item['KKumiku']][$item['OutFlag']]['colN'][] = '※';
											} else {
												if (empty($temp['timeColN'])) {
													$temp['colN'] = $item['colN'];
													$temp['timeColN'] = $item['timeColN'];
												} else {
													if (!empty($item['timeColN']) && strtotime($temp['timeColN']) > strtotime($item['timeColN'])) {
														$temp['colN'] = $item['colN'];
														$temp['timeColN'] = $item['timeColN'];
													}
												}
											}

											// E
											if (empty($temp['colE']) && !empty($item['colE'])) {
												$temp['colE'] = $item['colE'];
												$temp['timeColE'] = $item['timeColE'];
											}
											// compare col F get min
											if ($item['DDChild'] != '' && in_array($item['KKumiku'], array(
												config('system_const.kumiku_code_kogumi'),
												config('system_const.kumiku_code_naicyu'),
											)) && $item['OutFlag'] == config('system_const_schem.kotei_outflag_001')) {
												$arrCountAllRowDel[$keyGroup.'_'.$item['No']][$item['KKumiku']][$item['OutFlag']]['colF'][] = '※';
											} else {
												if (empty($temp['timeColF'])) {
													$temp['colF'] = $item['colF'];
													$temp['timeColF'] = $item['timeColF'];
												} else {
													if (!empty($item['timeColF']) && strtotime($temp['timeColF']) > strtotime($item['timeColF'])) {
														$temp['colF'] = $item['colF'];
														$temp['timeColF'] = $item['timeColF'];
													}
												}
											}
											// G
											if (empty($temp['colG']) && !empty($item['colG'])) {
												if ($temp['colE'] != '' && $temp['colE'] != '※' && $temp['colF'] != '' && $temp['colF'] != '※') {
													$temp['colG'] = $item['colG'];
												}
											}

											// K
											if (empty($temp['colK']) && !empty($item['colK'])) {
												$temp['colK'] = $item['colK'];
												$temp['timeColK'] = $item['timeColK'];
											}
											// compare col L get min
											if ($item['DDChild'] != '' && in_array($item['KKumiku'], array(
												config('system_const.kumiku_code_kogumi'),
												config('system_const.kumiku_code_naicyu'),
											)) && $item['OutFlag'] == config('system_const_schem.kotei_outflag_002')) {
												$arrCountAllRowDel[$keyGroup.'_'.$item['No']][$item['KKumiku']][$item['OutFlag']]['colL'][] = '※';
											} else {
												if (empty($temp['timeColL'])) {
													$temp['colL'] = $item['colL'];
													$temp['timeColL'] = $item['timeColL'];
												} else {
													if (!empty($item['timeColL']) && strtotime($temp['timeColL']) > strtotime($item['timeColL'])) {
														$temp['colL'] = $item['colL'];
														$temp['timeColL'] = $item['timeColL'];
													}
												}
											}

											// O
											if (empty($temp['colO']) && !empty($item['colO'])) {
												$temp['colO'] = $item['colO'];
												$temp['timeColO'] = $item['timeColO'];
											}
											// P
											if (empty($temp['colP']) && !empty($item['colP'])) {
												if ($item['DDChild'] != '' && in_array($item['DDChildColP'], array(
													config('system_const.kumiku_code_sogumi')
												))) {
													$arrCountAllRowDel[$keyGroup.'_'.$item['No']][$item['KKumiku']][$item['OutFlag']]['colP'][] = '※';
												} else {
													if (empty($temp['timeColP'])) {
														$temp['colP'] = $item['colP'];
														$temp['timeColP'] = $item['timeColP'];
													} else {
														if (!empty($item['timeColP']) && strtotime($temp['timeColP']) > strtotime($item['timeColP'])) {
															$temp['colP'] = $item['colP'];
															$temp['timeColP'] = $item['timeColP'];
														}
													}
												}
											}
											// Q
											if (empty($temp['colQ']) && !empty($item['colQ'])) {
												$temp['colQ'] = $item['colQ'];
												$temp['timeColQ'] = $item['timeColQ'];
											}
											// R
											if (empty($temp['colR']) && !empty($item['colR'])) {
												$temp['colR'] = $item['colR'];
												$temp['timeColR'] = $item['timeColR'];
											}
											// S
											if (empty($temp['colS']) && !empty($item['colS'])) {
												if ($item['DDChild'] != '') {
													$temp['colS'] = '';
												} else {
													$temp['colS'] = $item['colS'];
												}
											}
										} else {
											$temp['colE'] = '※';
											$temp['timeColE'] = '';
											$temp['colF'] = '※';
											$temp['timeColF'] = '';
											$temp['colH'] = '※';
											$temp['timeColH'] = '';
											$temp['colI'] = '※';
											$temp['timeColI'] = '';
											$temp['colK'] = '※';
											$temp['timeColK'] = '';
											$temp['colL'] = '※';
											$temp['timeColL'] = '';
											$temp['colM'] = '※';
											$temp['timeColM'] = '';
											$temp['colN'] = '※';
											$temp['timeColN'] = '';
											$temp['colO'] = '※';
											$temp['timeColO'] = '';
											$temp['colP'] = '※';
											$temp['timeColP'] = '';
											$temp['colQ'] = '※';
											$temp['timeColQ'] = '';
											$temp['colR'] = '※';
											$temp['timeColR'] = '';
										}
									}
									if (count($countUniqueRowInGroup) > 0 && count($arrCountAllRowDel) > 0) {
										foreach ($arrCountAllRowDel as $groupNo => $arr) {
											if (count($arr) > 0) {
												foreach ($arr as $kKumiku => $arrOutFlagCol) {
													if (count($arrOutFlagCol) > 0) {
														foreach ($arrOutFlagCol as $outFlag => $countDel) {
															$key = $groupNo.'_'.$kKumiku.'_'.$outFlag;
															if (isset($countUniqueRowInGroup[$key])) {
																foreach ($countDel as $col => $value) {
																	if ($countUniqueRowInGroup[$key] == count($value)) {
																		// set ※
																		$temp[$col] = '※';
																		$temp['time'.ucfirst($col)] = '';
																	}
																}
															}
														}
													}
												}
											}
										}
									}
									// update rev8
									$temp['flagHasDataWrong'] = ($countDataWrong == count($newBlock)) ? 1 : -1;
									if ($countTextStarColB == count($newBlock)) {
										$temp['colB'] = '☆';
									} else {
										// fix bug #258
										if ($temp['colB'] != '' && $temp['colB'] != '×') {
											$temp['colB'] = '';
										}
									}
									$newArrBeforeMergeRow[$keyGroup][] = $temp;
								} else {
									foreach ($newBlock as $row) {
										$newArrBeforeMergeRow[$keyGroup][] = $row;
									}
								}
							}
						}
					} else {
						foreach ($group as $row) {
							$newArrBeforeMergeRow[$keyGroup][] = $row;
						}
					}
				}

				$listNoKotei = array();
				$arrMergeRowBeforeRemoveFailData = array();
				// merge row
				foreach ($newArrBeforeMergeRow as $group) {
					// check group have >= 2 to merge row
					if (count($group) > 1) {
						$arrOnlyKeyNotDuplicate = array();
						foreach ($group as $key => &$row) {
							$flagHasProcess = false;

							foreach ($group as $key_copy => &$row_copy) {
								if ($key == $key_copy) {
									continue;
								}

								if ($row['colC'] == $row_copy['colC'] &&
									$row['colE'] == $row_copy['colE'] &&
									$row['colF'] == $row_copy['colF'] &&
									$row['colH'] == $row_copy['colH'] &&
									$row['colI'] == $row_copy['colI'] &&
									$row['colK'] == $row_copy['colK'] &&
									$row['colL'] == $row_copy['colL'] &&
									$row['colM'] == $row_copy['colM'] &&
									$row['colN'] == $row_copy['colN'] &&
									$row['colO'] == $row_copy['colO'] &&
									$row['colP'] == $row_copy['colP'] &&
									$row['colQ'] == $row_copy['colQ'] &&
									$row['colR'] == $row_copy['colR']
								) {
									if (($row['colD'] == 'P' && $row_copy['colD'] == 'S') ||
										($row['colD'] == 'S' && $row_copy['colD'] == 'P')
									) {
										if (count($arrOnlyKeyNotDuplicate) == 0) {
											$arrOnlyKeyNotDuplicate[] = $row['key'];
											$arrOnlyKeyNotDuplicate[] = $row_copy['key'];
											$row['colD'] = 'P/S';
											$row['merge'] = $row['key'] . '-----' . $row_copy['key'];
											// $arrListExport[] = $row;
											$arrMergeRowBeforeRemoveFailData[$row['Group6Char']][] = $row;
											$row['colF'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colF'] : '※') : '※';
											$row['colI'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colI'] : '※') : '※';
											$row['colL'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colL'] : '※') : '※';
											$row['colN'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colN'] : '※') : '※';
											$row['colP'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colP'] : '※') : '※';
											$row['colR'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colR'] : '※') : '※';
											$listNoKotei[$row['GroupData']][] = $row;
											$row_copy['colF'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colF'] : '※') : '※';
											$row_copy['colI'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colI'] : '※') : '※';
											$row_copy['colL'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colL'] : '※') : '※';
											$row_copy['colN'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colN'] : '※') : '※';
											$row_copy['colP'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colP'] : '※') : '※';
											$row_copy['colR'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colR'] : '※') : '※';
											$listNoKotei[$row_copy['GroupData']][] = $row_copy;
											// $this->getListYear($row, $arrRangeYear);
											$flagHasProcess = true;
										} else {
											if (!in_array($row['key'], $arrOnlyKeyNotDuplicate) && !in_array($row_copy['key'], $arrOnlyKeyNotDuplicate)) {
												$arrOnlyKeyNotDuplicate[] = $row['key'];
												$arrOnlyKeyNotDuplicate[] = $row_copy['key'];
												$row['colD'] = 'P/S';
												$row['merge'] = $row['key'] . '-----' . $row_copy['key'];
												// $arrListExport[] = $row;
												$arrMergeRowBeforeRemoveFailData[$row['Group6Char']][] = $row;
												$row['colF'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colF'] : '※') : '※';
												$row['colI'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colI'] : '※') : '※';
												$row['colL'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colL'] : '※') : '※';
												$row['colN'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colN'] : '※') : '※';
												$row['colP'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colP'] : '※') : '※';
												$row['colR'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colR'] : '※') : '※';
												$listNoKotei[$row['GroupData']][] = $row;
												$row_copy['colF'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colF'] : '※') : '※';
												$row_copy['colI'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colI'] : '※') : '※';
												$row_copy['colL'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colL'] : '※') : '※';
												$row_copy['colN'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colN'] : '※') : '※';
												$row_copy['colP'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colP'] : '※') : '※';
												$row_copy['colR'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colR'] : '※') : '※';
												$listNoKotei[$row_copy['GroupData']][] = $row_copy;
												// $this->getListYear($row, $arrRangeYear);
												$flagHasProcess = true;
											}
										}
									}
									if (($row['colD'] == 'P' && $row_copy['colD'] == 'C') ||
										($row['colD'] == 'C' && $row_copy['colD'] == 'P')
									) {
										if (count($arrOnlyKeyNotDuplicate) == 0) {
											$arrOnlyKeyNotDuplicate[] = $row['key'];
											$arrOnlyKeyNotDuplicate[] = $row_copy['key'];
											$row['colD'] = 'P/C';
											$row['merge'] = $row['key'] . '-----' . $row_copy['key'];
											// $arrListExport[] = $row;
											$arrMergeRowBeforeRemoveFailData[$row['Group6Char']][] = $row;
											$row['colF'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colF'] : '※') : '※';
											$row['colI'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colI'] : '※') : '※';
											$row['colL'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colL'] : '※') : '※';
											$row['colN'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colN'] : '※') : '※';
											$row['colP'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colP'] : '※') : '※';
											$row['colR'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colR'] : '※') : '※';
											$listNoKotei[$row['GroupData']][] = $row;
											$row_copy['colF'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colF'] : '※') : '※';
											$row_copy['colI'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colI'] : '※') : '※';
											$row_copy['colL'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colL'] : '※') : '※';
											$row_copy['colN'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colN'] : '※') : '※';
											$row_copy['colP'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colP'] : '※') : '※';
											$row_copy['colR'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colR'] : '※') : '※';
											$listNoKotei[$row_copy['GroupData']][] = $row_copy;
											// $this->getListYear($row, $arrRangeYear);
											$flagHasProcess = true;
										} else {
											if (!in_array($row['key'], $arrOnlyKeyNotDuplicate) && !in_array($row_copy['key'], $arrOnlyKeyNotDuplicate)) {
												$arrOnlyKeyNotDuplicate[] = $row['key'];
												$arrOnlyKeyNotDuplicate[] = $row_copy['key'];
												$row['colD'] = 'P/C';
												$row['merge'] = $row['key'] . '-----' . $row_copy['key'];
												// $arrListExport[] = $row;
												$arrMergeRowBeforeRemoveFailData[$row['Group6Char']][] = $row;
												$row['colF'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colF'] : '※') : '※';
												$row['colI'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colI'] : '※') : '※';
												$row['colL'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colL'] : '※') : '※';
												$row['colN'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colN'] : '※') : '※';
												$row['colP'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colP'] : '※') : '※';
												$row['colR'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colR'] : '※') : '※';
												$listNoKotei[$row['GroupData']][] = $row;
												$row_copy['colF'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colF'] : '※') : '※';
												$row_copy['colI'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colI'] : '※') : '※';
												$row_copy['colL'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colL'] : '※') : '※';
												$row_copy['colN'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colN'] : '※') : '※';
												$row_copy['colP'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colP'] : '※') : '※';
												$row_copy['colR'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colR'] : '※') : '※';
												$listNoKotei[$row_copy['GroupData']][] = $row_copy;
												// $this->getListYear($row, $arrRangeYear);
												$flagHasProcess = true;
											}
										}
									}
									if (($row['colD'] == 'S' && $row_copy['colD'] == 'C') ||
										($row['colD'] == 'C' && $row_copy['colD'] == 'S')
									) {
										if (count($arrOnlyKeyNotDuplicate) == 0) {
											$arrOnlyKeyNotDuplicate[] = $row['key'];
											$arrOnlyKeyNotDuplicate[] = $row_copy['key'];
											$row['colD'] = 'C/S';
											$row['merge'] = $row['key'] . '-----' . $row_copy['key'];
											// $arrListExport[] = $row;
											$arrMergeRowBeforeRemoveFailData[$row['Group6Char']][] = $row;
											$row['colF'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colF'] : '※') : '※';
											$row['colI'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colI'] : '※') : '※';
											$row['colL'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colL'] : '※') : '※';
											$row['colN'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colN'] : '※') : '※';
											$row['colP'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colP'] : '※') : '※';
											$row['colR'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colR'] : '※') : '※';
											$listNoKotei[$row['GroupData']][] = $row;
											$row_copy['colF'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colF'] : '※') : '※';
											$row_copy['colI'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colI'] : '※') : '※';
											$row_copy['colL'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colL'] : '※') : '※';
											$row_copy['colN'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colN'] : '※') : '※';
											$row_copy['colP'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colP'] : '※') : '※';
											$row_copy['colR'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colR'] : '※') : '※';
											$listNoKotei[$row_copy['GroupData']][] = $row_copy;
											// $this->getListYear($row, $arrRangeYear);
											$flagHasProcess = true;
										} else {
											if (!in_array($row['key'], $arrOnlyKeyNotDuplicate) && !in_array($row_copy['key'], $arrOnlyKeyNotDuplicate)) {
												$arrOnlyKeyNotDuplicate[] = $row['key'];
												$arrOnlyKeyNotDuplicate[] = $row_copy['key'];
												$row['colD'] = 'C/S';
												$row['merge'] = $row['key'] . '-----' . $row_copy['key'];
												// $arrListExport[] = $row;
												$arrMergeRowBeforeRemoveFailData[$row['Group6Char']][] = $row;
												$row['colF'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colF'] : '※') : '※';
												$row['colI'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colI'] : '※') : '※';
												$row['colL'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colL'] : '※') : '※';
												$row['colN'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colN'] : '※') : '※';
												$row['colP'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colP'] : '※') : '※';
												$row['colR'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colR'] : '※') : '※';
												$listNoKotei[$row['GroupData']][] = $row;
												$row_copy['colF'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colF'] : '※') : '※';
												$row_copy['colI'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colI'] : '※') : '※';
												$row_copy['colL'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colL'] : '※') : '※';
												$row_copy['colN'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colN'] : '※') : '※';
												$row_copy['colP'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colP'] : '※') : '※';
												$row_copy['colR'] = ($row_copy['colB'] != '×') ? (($row_copy['DDChild'] == '') ? $row_copy['colR'] : '※') : '※';
												$listNoKotei[$row_copy['GroupData']][] = $row_copy;
												// $this->getListYear($row, $arrRangeYear);
												$flagHasProcess = true;
											}
										}
									}
								}
							}

							if (!$flagHasProcess && !in_array($row['key'], $arrOnlyKeyNotDuplicate)) {
								// $arrListExport[] = $row;
								$arrMergeRowBeforeRemoveFailData[$row['Group6Char']][] = $row;
								$row['colF'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colF'] : '※') : '※';
								$row['colI'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colI'] : '※') : '※';
								$row['colL'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colL'] : '※') : '※';
								$row['colN'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colN'] : '※') : '※';
								$row['colP'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colP'] : '※') : '※';
								$row['colR'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colR'] : '※') : '※';
								$listNoKotei[$row['GroupData']][] = $row;
								// $this->getListYear($row, $arrRangeYear);
							}
						}
					} else {
						foreach ($group as &$row) {
							// $arrListExport[] = $row;
							$arrMergeRowBeforeRemoveFailData[$row['Group6Char']][] = $row;
							$row['colF'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colF'] : '※') : '※';
							$row['colI'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colI'] : '※') : '※';
							$row['colL'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colL'] : '※') : '※';
							$row['colN'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colN'] : '※') : '※';
							$row['colP'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colP'] : '※') : '※';
							$row['colR'] = ($row['colB'] != '×') ? (($row['DDChild'] == '') ? $row['colR'] : '※') : '※';
							$listNoKotei[$row['GroupData']][] = $row;
							// $this->getListYear($row, $arrRangeYear);
						}
					}
				}

				// update rev8
				$arrListExport = array();
				$msgTimeTrackerFailure = '';
				if (count($arrMergeRowBeforeRemoveFailData) > 0) {
					foreach ($arrMergeRowBeforeRemoveFailData as $group6Char => &$list) {
						if (count($list) > 0) {
							$countError = 0;
							$countDataNothingER = 0;
							foreach ($list as $keyIndex => &$itemList) {
								if ($itemList['flagHasDataWrong'] === 1) {
									$countError += 1;
								}

								$itemList['colE'] = ($itemList['colB'] != '×') ? $itemList['colE'] : '※';
								$itemList['colF'] = ($itemList['colB'] != '×' ) ? (($itemList['colF'] != $itemList['colE']) ? $itemList['colF'] : '') : '※';
								// re-calculate colG
								if ($itemList['colE'] != '' && $itemList['colE'] != '※' && $itemList['colF'] != '' && $itemList['colF'] != '※') {
									$dataGetDiffDate = $TimeTrackerCommon->getDateDiff($dataGetCalendar, $itemList['timeColE'], $itemList['timeColF']);
									if ($dataGetDiffDate != '') {
										if (is_string($dataGetDiffDate)) {
											$msgTimeTrackerFailure = $dataGetDiffDate;
											break 2;
										}
										if (is_int($dataGetDiffDate) && $dataGetDiffDate != 0) {
											$itemList['colG'] = $dataGetDiffDate;
										}
									}
								} else {
									$itemList['colG'] = '';
								}

								$itemList['colH'] = ($itemList['colB'] != '×') ? $itemList['colH'] : '※';
								$itemList['colI'] = ($itemList['colB'] != '×') ? (($itemList['colI'] != $itemList['colH']) ? $itemList['colI'] : '') : '※';
								// re-calculate colJ
								if ($itemList['colH'] != '' && $itemList['colH'] != '※' && $itemList['colI'] != '' && $itemList['colI'] != '※') {
									$dataGetDiffDate = $TimeTrackerCommon->getDateDiff($dataGetCalendar, $itemList['timeColH'], $itemList['timeColI']);
									if ($dataGetDiffDate != '') {
										if (is_string($dataGetDiffDate)) {
											$msgTimeTrackerFailure = $dataGetDiffDate;
											break 2;
										}
										if (is_int($dataGetDiffDate) && $dataGetDiffDate != 0) {
											$itemList['colJ'] = $dataGetDiffDate;
										}
									}
								} else {
									$itemList['colJ'] = '';
								}

								$itemList['colK'] = ($itemList['colB'] != '×') ? $itemList['colK'] : '※';
								$itemList['colL'] = ($itemList['colB'] != '×') ? (($itemList['colL'] != $itemList['colK']) ? $itemList['colL'] : '') : '※';

								$itemList['colM'] = ($itemList['colB'] != '×') ? $itemList['colM'] : '※';
								$itemList['colN'] = ($itemList['colB'] != '×') ? (($itemList['colN'] != $itemList['colM']) ? $itemList['colN'] : '') : '※';

								$itemList['colO'] = ($itemList['colB'] != '×') ? $itemList['colO'] : '※';
								$itemList['colP'] = ($itemList['colB'] != '×') ? (($itemList['colP'] != $itemList['colO']) ? $itemList['colP'] : '') : '※';

								$itemList['colQ'] = ($itemList['colB'] != '×') ? $itemList['colQ'] : '※';
								$itemList['colR'] = ($itemList['colB'] != '×') ? (($itemList['colR'] != $itemList['colQ']) ? $itemList['colR'] : '') : '※';

								if (
									$itemList['colE'] == '' &&
									$itemList['colF'] == '' &&
									$itemList['colG'] == '' &&
									$itemList['colH'] == '' &&
									$itemList['colI'] == '' &&
									$itemList['colJ'] == '' &&
									$itemList['colK'] == '' &&
									$itemList['colL'] == '' &&
									$itemList['colM'] == '' &&
									$itemList['colN'] == '' &&
									$itemList['colO'] == '' &&
									$itemList['colP'] == '' &&
									$itemList['colQ'] == '' &&
									$itemList['colR'] == ''
								) {
									// ※E~R列に出力するデータがなかった場合はそのブロックは出力しない。
									$countDataNothingER += 1;
								}
							}
							if ($countError === count($list) || $countDataNothingER === count($list)) {
								unset($arrMergeRowBeforeRemoveFailData[$group6Char]);
								// fix bug 293
								foreach ($list as $keyIndex => $itemList) {
									if (isset($arrDataLastNNo[$itemList['keyParent']])) {
										unset($arrDataLastNNo[$itemList['keyParent']]);
									}
								}
							} else {
								foreach ($list as $keyIndex => &$itemList) {
									$arrListExport[] = $itemList;
									$this->getListYear($itemList, $arrRangeYear);
								}
							}
						}
					}
				}

				if ($msgTimeTrackerFailure != '') {
					$url .= '&err1=' . valueUrlEncode($msgTimeTrackerFailure);
					// redirect screen output
					return redirect($url);
				}

				if (count($arrListExport) > 0) {
					// sort by column C
					$arrColC = array_column($arrListExport, 'colC');
					array_multisort($arrColC, SORT_ASC, $arrListExport);

					$listYearStyle = array();
					if (count($arrRangeYear) > 0) {
						$arrTempCount = array();
						foreach ($arrRangeYear as $year => $list) {
							$arrTempCount[$year] = count($list);
						}
						// sort asc
						asort($arrTempCount);

						// find group year of quantity
						$arrYearQuantity = array();
						foreach ($arrTempCount as $year => $quantity) {
							$arrYearQuantity[$quantity][] = $year;
						}
						$arrTempCount = array();
						foreach ($arrYearQuantity as &$arrYear) {
							sort($arrYear);
							array_push($arrTempCount, ...$arrYear);
						}

						$arrRangeYear = array();
						foreach ($arrTempCount as $year) {
							$arrRangeYear[] = $year;
						}
						$totalYear = count($arrRangeYear);

						$template = '';
						// in 1 year
						if ($totalYear == 1) {
							$template = config('system_const_schem.output_year_001') . $arrRangeYear[0] . '年';
						}
						// pass through only 2 years
						if ($totalYear == 2) {
							$template = '※' . config('system_const_schem.output_year_002') . $arrRangeYear[0] . '年。' .
								config('system_const_schem.output_year_003') . $arrRangeYear[1] . '年';
							$listYearStyle[$arrRangeYear[0]] = 'dot';
						}
						// pass through only 3 years
						if ($totalYear == 3) {
							$template = '※' . config('system_const_schem.output_year_004') . $arrRangeYear[0] . '年、' .
								config('system_const_schem.output_year_002') . $arrRangeYear[1] . '年、' .
								config('system_const_schem.output_year_003') . $arrRangeYear[2] . '年';
							$listYearStyle[$arrRangeYear[1]] = 'dot';
							$listYearStyle[$arrRangeYear[0]] = 'border';
						}
						// pass through only 4 years
						if ($totalYear == 4) {
							$template = '※' . config('system_const_schem.output_year_004') . $arrRangeYear[0] . '年、' .
								config('system_const_schem.output_year_002') . $arrRangeYear[1] . '年、' .
								config('system_const_schem.output_year_005') . $arrRangeYear[2] . '年、' .
								config('system_const_schem.output_year_003') . $arrRangeYear[3] . '年';
							$listYearStyle[$arrRangeYear[1]] = 'dot';
							$listYearStyle[$arrRangeYear[0]] = 'border';
							$listYearStyle[$arrRangeYear[2]] = 'bold';
						}
						// pass through over 4 years
						if ($totalYear >= 5) {
							$template = '※' . config('system_const_schem.output_year_004') . $arrRangeYear[0] . '年、' .
								config('system_const_schem.output_year_002') . $arrRangeYear[1] . '年、' .
								config('system_const_schem.output_year_005') . $arrRangeYear[2] . '年、' .
								config('system_const_schem.output_year_006') . $arrRangeYear[3] . '年、' .
								config('system_const_schem.output_year_003') . $arrRangeYear[4] . '年';
							$listYearStyle[$arrRangeYear[1]] = 'dot';
							$listYearStyle[$arrRangeYear[0]] = 'border';
							$listYearStyle[$arrRangeYear[2]] = 'bold';
							$listYearStyle[$arrRangeYear[3]] = 'italic';
						}

						if ($template != '') {
							// set date Q2
							$worksheet->setCellValue('J1', $template);
						}
					}

					$baseRow = 6;
					$rowIncrement = $baseRow;
					$firstRow = $baseRow;
					$blockName = '';
					$arrRangeBorder = array();
					$arrRowStrikeThrough = array();
					$arrStyleCellYear = array(
						'dot' => array(),
						'border' => array(),
						'bold' => array(),
						'italic' => array(),
					);
					$arrayExportExcel = array();
					foreach ($arrListExport as &$row) {
						$arrayExportExcel[] = array(
							$row['colB'],
							$row['colC'],
							$row['colD'],
							$row['colE'],
							$row['colF'],
							$row['colG'],
							$row['colH'],
							$row['colI'],
							$row['colJ'],
							$row['colK'],
							$row['colL'],
							$row['colM'],
							$row['colN'],
							$row['colO'],
							$row['colP'],
							$row['colQ'],
							$row['colR'],
							$row['colS'],
						);

						if ($row['colB'] == '×') {
							$arrRowStrikeThrough[] = 'C' . $rowIncrement . ':' . 'D' . $rowIncrement;
							$arrRowStrikeThrough[] = 'S' . $rowIncrement;
						} else {
							if (($row['colE'] != '' && $row['colE'] != '※') && $row['colF'] == '※') {
								$arrRowStrikeThrough[] = 'E' . $rowIncrement;
							}
							if (($row['colH'] != '' && $row['colH'] != '※') && $row['colI'] == '※') {
								$arrRowStrikeThrough[] = 'H' . $rowIncrement;
							}
							if (($row['colK'] != '' && $row['colK'] != '※') && $row['colL'] == '※') {
								$arrRowStrikeThrough[] = 'K' . $rowIncrement;
							}
							if (($row['colM'] != '' && $row['colM'] != '※') && $row['colN'] == '※') {
								$arrRowStrikeThrough[] = 'M' . $rowIncrement;
							}
							if (($row['colO'] != '' && $row['colO'] != '※') && $row['colP'] == '※') {
								$arrRowStrikeThrough[] = 'O' . $rowIncrement;
							}
							// update rev7
							// if (($row['colQ'] != '' && $row['colQ'] != '※') && $row['colR'] == '※') {
							// 	$arrRowStrikeThrough[] = 'Q' . $rowIncrement;
							// }
						}

						$this->createListStyleYear($row['colB'], $row['DDChild'], $row['colE'], $row['timeColE'], 'E'.$rowIncrement,
																					$listYearStyle, $arrStyleCellYear);
						$this->createListStyleYear($row['colB'], $row['DDChild'], $row['colF'], $row['timeColF'], 'F'.$rowIncrement,
																					$listYearStyle, $arrStyleCellYear);
						$this->createListStyleYear($row['colB'], $row['DDChild'], $row['colH'], $row['timeColH'], 'H'.$rowIncrement,
																					$listYearStyle, $arrStyleCellYear);
						$this->createListStyleYear($row['colB'], $row['DDChild'], $row['colI'], $row['timeColI'], 'I'.$rowIncrement,
																					$listYearStyle, $arrStyleCellYear);
						$this->createListStyleYear($row['colB'], $row['DDChild'], $row['colK'], $row['timeColK'], 'K'.$rowIncrement,
																					$listYearStyle, $arrStyleCellYear);
						$this->createListStyleYear($row['colB'], $row['DDChild'], $row['colL'], $row['timeColL'], 'L'.$rowIncrement,
																					$listYearStyle, $arrStyleCellYear);
						$this->createListStyleYear($row['colB'], $row['DDChild'], $row['colM'], $row['timeColM'], 'M'.$rowIncrement,
																					$listYearStyle, $arrStyleCellYear);
						$this->createListStyleYear($row['colB'], $row['DDChild'], $row['colN'], $row['timeColN'], 'N'.$rowIncrement,
																					$listYearStyle, $arrStyleCellYear);
						$this->createListStyleYear($row['colB'], $row['DDChild'], $row['colO'], $row['timeColO'], 'O'.$rowIncrement,
																					$listYearStyle, $arrStyleCellYear);
						$this->createListStyleYear($row['colB'], $row['DDChild'], $row['colP'], $row['timeColP'], 'P'.$rowIncrement,
																					$listYearStyle, $arrStyleCellYear);
						$this->createListStyleYear($row['colB'], $row['DDChild'], $row['colQ'], $row['timeColQ'], 'Q'.$rowIncrement,
																					$listYearStyle, $arrStyleCellYear);
						$this->createListStyleYear($row['colB'], $row['DDChild'], $row['colR'], $row['timeColR'], 'R'.$rowIncrement,
																					$listYearStyle, $arrStyleCellYear);

						if ($blockName == '') {
							$blockName = $row['Group6Char'];
						} else {
							if ($row['Group6Char'] != $blockName) {
								$arrRangeBorder[] = 'A' . $baseRow . ':' . 'S' . ($rowIncrement - 1);
								$baseRow = $rowIncrement;
								$blockName = $row['Group6Char'];
							}
						}

						$rowIncrement++;
					}

					if ($msgTimeTrackerFailure != '') {
						$url .= '&err1=' . valueUrlEncode($msgTimeTrackerFailure);
						// redirect screen output
						return redirect($url);
					}

					// fill data to Excel
					$worksheet->fromArray($arrayExportExcel, NULL, 'B'.$firstRow);

					// last block
					$arrRangeBorder[] = 'A' . $baseRow . ':' . 'S' . ($rowIncrement - 1);
					$endRow = $rowIncrement - 1;

					// set style font + border
					if (count($arrRangeBorder) > 0) {
						// update rev8
						$worksheet->getStyle('A' . $firstRow . ':' . 'S' . $endRow)
									->getAlignment()
									->setShrinkToFit(true);	// update rev8;

						foreach ($arrRangeBorder as $range) {
							$worksheet->getStyle($range)->applyFromArray([
								'borders' => [
									'outline' => [
										'borderStyle' => Border::BORDER_THIN,
										'color' => ['argb' => '000000'],
									],
								],
							]);
						}

						// set format M/D
						$arrCellDateFormat = array('E', 'F', 'H', 'I', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R');
						foreach ($arrCellDateFormat as $col) {
							$worksheet->getStyle($col . $firstRow . ':' . $col . $endRow)
									->getNumberFormat()
									->setFormatCode('M/D');
						}

						$arrRangeColSolid = array(
							array('A', 'A'),	// col A
							array('B', 'B'),	// col B
							array('C', 'D'),	// col C + D
							array('E', 'G'),	// col E + F + G
							array('H', 'J'),	// col H + I + J
							array('K', 'L'),	// col K + L
							array('M', 'N'),	// col M + N
							array('O', 'P'),	// col O + P
							array('Q', 'R'),	// col Q + R
						);
						foreach ($arrRangeColSolid as $range) {
							$worksheet->getStyle($range[0] . $firstRow . ':' . $range[1] . $endRow)->applyFromArray([
								'borders' => [
									'outline' => [
										'borderStyle' => Border::BORDER_THIN,
										'color' => ['argb' => '000000'],
									],
								],
							]);
						}

						$arrColHair = array('E', 'F', 'H', 'I', 'K', 'M', 'O', 'Q');
						foreach ($arrColHair as $col) {
							$worksheet->getStyle($col . $firstRow . ':' . $col . $endRow)->applyFromArray([
								'borders' => [
									'right' => [
										'borderStyle' => Border::BORDER_HAIR,
										'color' => ['argb' => '000000'],
									],
								],
							]);
						}

						// set strikethrough
						if (count($arrRowStrikeThrough) > 0) {
							foreach ($arrRowStrikeThrough as $range) {
								$worksheet->getStyle($range)->applyFromArray([
									'font' => [
										'strikethrough' => true
									],
								]);
							}
						}

						// set style for Year cell
						if (count($arrStyleCellYear['dot']) > 0) {
							foreach ($arrStyleCellYear['dot'] as $cell) {
								$worksheet->getStyle($cell)->applyFromArray([
									'fill' => [
										'fillType' => \PhpOffice\PhpSpreadsheet\Style\Fill::FILL_PATTERN_GRAY0625,
										'startColor' => [
											'argb' => '000000',
										],
										'endColor' => [
											'argb' => 'FFFFFFFF',
										],
									],
								]);
							}
						}
						if (count($arrStyleCellYear['border']) > 0) {
							foreach ($arrStyleCellYear['border'] as $cell) {
								$worksheet->getStyle($cell)->applyFromArray([
									'borders' => [
										'outline' => [
											'borderStyle' => Border::BORDER_THICK,
											'color' => ['argb' => '000000'],
										],
									],
								]);
							}
						}
						if (count($arrStyleCellYear['bold']) > 0) {
							foreach ($arrStyleCellYear['bold'] as $cell) {
								$worksheet->getStyle($cell)->applyFromArray([
									'font' => [
										'bold' => true
									],
								]);
							}
						}
						if (count($arrStyleCellYear['italic']) > 0) {
							foreach ($arrStyleCellYear['italic'] as $cell) {
								$worksheet->getStyle($cell)->applyFromArray([
									'font' => [
										'italic' => true
									],
								]);
							}
						}
					}
				}

				// if 正式発行 checked -> update Cyn_Plan || Cyn_C_Plan
				if ($validated['val4'] == 1) {
					try {
						DB::transaction(function () use ($listNoKotei, $arrDataLastNNo, $validated) {
							foreach ($listNoKotei as $group => $rows) {
								foreach ($rows as $row) {
									$tempCynMstKotei = null;
									$objTemp = array();

									// fix bug 293
									if (in_array($row['KKumiku'], array(
										config('system_const.kumiku_code_kogumi'),
										config('system_const.kumiku_code_naicyu')
									))) {
										if ($row['OutFlag'] == config('system_const_schem.kotei_outflag_001') &&
											$row['colF'] != '※') {
											if (trim($row['timeColF']) != '') {
												$objTemp['B_PlSDate'] = $row['timeColF'];
											} else {
												$objTemp['B_PlSDate'] = null;
											}
										}
										if ($row['OutFlag'] == config('system_const_schem.kotei_outflag_002') &&
											$row['colL'] != '※') {
											if (trim($row['timeColL']) != '') {
												$objTemp['B_PlSDate'] = $row['timeColL'];
											} else {
												$objTemp['B_PlSDate'] = null;
											}
										}
									}
									if (in_array($row['KKumiku'], array(
										config('system_const.kumiku_code_ogumi'),
										config('system_const.kumiku_code_kumicyu')
									))) {
										if ($row['OutFlag'] == config('system_const_schem.kotei_outflag_001') &&
											$row['colI'] != '※') {
											if (trim($row['timeColI']) != '') {
												$objTemp['B_PlSDate'] = $row['timeColI'];
											} else {
												$objTemp['B_PlSDate'] = null;
											}
										}
										if ($row['OutFlag'] == config('system_const_schem.kotei_outflag_002') &&
											$row['colN'] != '※') {
											if (trim($row['timeColN']) != '') {
												$objTemp['B_PlSDate'] = $row['timeColN'];
											} else {
												$objTemp['B_PlSDate'] = null;
											}
										}
									}
									if ($row['colP'] != '※') {
										if (trim($row['timeColP']) != '') {
											$objTemp['B_SG_Date'] = $row['timeColP'];
										} else {
											$objTemp['B_SG_Date'] = null;
										}
									}
									if ($row['colR'] != '※') {
										if (trim($row['timeColR']) != '') {
											$objTemp['B_T_Date'] = $row['timeColR'];
										} else {
											$objTemp['B_T_Date'] = null;
										}
									}

									if (count($objTemp) > 0) {
										if ($row['GroupData'] == 1) {
											$result = Cyn_Plan::where('ProjectID', $validated['val2'])
																->Where('OrderNo', $validated['val3'])
																->Where('No', $row['No'])
																->Where('KoteiNo', $row['KoteiNo'])
																->update($objTemp);
											// fix bug 293
											if (isset($arrDataLastNNo[$row['keyParent']])) {
												$result = Cyn_Plan::where('ProjectID', $arrDataLastNNo[$row['keyParent']]['projectID'])
																	->Where('OrderNo', $arrDataLastNNo[$row['keyParent']]['orderNo'])
																	->Where('No', $arrDataLastNNo[$row['keyParent']]['no'])
																	->Where('KoteiNo', $arrDataLastNNo[$row['keyParent']]['koteiNo'])
																	->update($objTemp);
											}
										}
										if ($row['GroupData'] == 2) {
											$result = Cyn_C_Plan::where('ProjectID', $validated['val2'])
																->Where('OrderNo', $validated['val3'])
																->Where('No', $row['No'])
																->Where('KoteiNo', $row['KoteiNo'])
																->update($objTemp);
											// fix bug 293
											if (isset($arrDataLastNNo[$row['keyParent']])) {
												$result = Cyn_C_Plan::where('ProjectID', $arrDataLastNNo[$row['keyParent']]['projectID'])
																	->Where('OrderNo', $arrDataLastNNo[$row['keyParent']]['orderNo'])
																	->Where('No', $arrDataLastNNo[$row['keyParent']]['no'])
																	->Where('KoteiNo', $arrDataLastNNo[$row['keyParent']]['koteiNo'])
																	->update($objTemp);
											}
										}
									}

									// Cyn_Plan
									if ($row['GroupData'] == 1) {
										if ($row['colB'] == '×') {
											// 以下の条件と表の内容で[Cyn_ BlockKukaku]または[Cyn_C_ BlockKukaku]のデータを削除する。
											$deleteObj = Cyn_BlockKukaku::where('ProjectID', '=', $validated['val2'])
																		->where('OrderNo', '=', $validated['val3'])
																		->where('No', '=', $row['No'])
																		->whereNotNull('Del_Date')
																		->delete();
										}

										// 以下の条件と表の内容で[Cyn_Plan]または[Cyn_C_Plan]のデータを削除する。
										$findData = Cyn_Plan::select('Cyn_mstKotei.OutFlag', 'Cyn_Plan.No', 'Cyn_Plan.KoteiNo', 'Cyn_Plan.KKumiku')
															->join('Cyn_mstKotei', 'Cyn_mstKotei.Code', '=', 'Cyn_Plan.Kotei')
															->join('Cyn_BlockKukaku', 'Cyn_mstKotei.CKind', '=', 'Cyn_BlockKukaku.CKind')
															->where('Cyn_Plan.ProjectID', '=', $validated['val2'])
															->where('Cyn_Plan.OrderNo', '=', $validated['val3'])
															->where('Cyn_mstKotei.CKind', '=', $validated['val1'])
															->where('Cyn_Plan.No', '=', $row['No'])
															->whereNotNull('Cyn_Plan.Del_Date')->get();
										if (count($findData) > 0) {
											foreach ($findData as $rowData) {
												if (in_array($rowData->KKumiku, array(
													config('system_const.kumiku_code_kogumi'),
													config('system_const.kumiku_code_naicyu'),
													config('system_const.kumiku_code_ogumi'),
													config('system_const.kumiku_code_kumicyu'),
												)) && in_array($rowData->OutFlag, array(
													config('system_const_schem.kotei_outflag_001'),
													config('system_const_schem.kotei_outflag_002'),
												))) {
													$deleteObj = Cyn_Plan::where('ProjectID', '=', $validated['val2'])
																		->where('OrderNo', '=', $validated['val3'])
																		->where('No', '=', $rowData->No)
																		->where('KoteiNo', '=', $rowData->KoteiNo)
																		->whereNotNull('Del_Date')
																		->delete();
												}
											}
										}
									}
									// Cyn_C_Plan
									if ($row['GroupData'] == 2) {
										if ($row['colB'] == '×') {
											// 以下の条件と表の内容で[Cyn_ BlockKukaku]または[Cyn_C_ BlockKukaku]のデータを削除する。
											$deleteObj = Cyn_C_BlockKukaku::where('ProjectID', '=', $validated['val2'])
																			->where('OrderNo', '=', $validated['val3'])
																			->where('No', '=', $row['No'])
																			->whereNotNull('Del_Date')
																			->delete();
										}

										// 以下の条件と表の内容で[Cyn_Plan]または[Cyn_C_Plan]のデータを削除する。
										$findData = Cyn_C_Plan::select('Cyn_mstKotei.OutFlag', 'Cyn_C_Plan.No', 'Cyn_C_Plan.KoteiNo', 'Cyn_C_Plan.KKumiku')
															->join('Cyn_mstKotei', 'Cyn_mstKotei.Code', '=', 'Cyn_C_Plan.Kotei')
															->join('Cyn_C_BlockKukaku', 'Cyn_mstKotei.CKind', '=', 'Cyn_C_BlockKukaku.CKind')
															->where('Cyn_C_Plan.ProjectID', '=', $validated['val2'])
															->where('Cyn_C_Plan.OrderNo', '=', $validated['val3'])
															->where('Cyn_mstKotei.CKind', '=', $validated['val1'])
															->where('Cyn_C_Plan.No', '=', $row['No'])
															->whereNotNull('Cyn_C_Plan.Del_Date')->get();
										if (count($findData) > 0) {
											foreach ($findData as $rowData) {
												if (in_array($rowData->KKumiku, array(
													config('system_const.kumiku_code_kogumi'),
													config('system_const.kumiku_code_naicyu'),
													config('system_const.kumiku_code_ogumi'),
													config('system_const.kumiku_code_kumicyu'),
												)) && in_array($rowData->OutFlag, array(
													config('system_const_schem.kotei_outflag_001'),
													config('system_const_schem.kotei_outflag_002'),
												))) {
													$deleteObj = Cyn_C_Plan::where('ProjectID', '=', $validated['val2'])
																		->where('OrderNo', '=', $validated['val3'])
																		->where('No', '=', $rowData->No)
																		->where('KoteiNo', '=', $rowData->KoteiNo)
																		->whereNotNull('Del_Date')
																		->delete();
												}
											}
										}
									}
								}
							}

							// update rev9 - 以下の内容で[mstOrderNo]を更新する。
							DB::statement("UPDATE mstOrderNo SET mstOrderNo.OutPutDate = GETDATE() WHERE mstOrderNo.OrderNo = ?", array(
								$validated['val3']
							)); // fix bug 294
						});
					} catch (Exception $e) {
						$url .= '&err1=' . valueUrlEncode(config('message.msg_cmn_db_016'));
						// redirect screen output
						return redirect($url);
					}
				}

				// QA #69
				if (count($arrListExport) == 0) {
					// has error
					$url .= '&err1=' . valueUrlEncode(config('message.msg_cmn_db_001'));
					return redirect($url);
				}

				$strFileName = $menuInfo->MenuNick;	// update rev 7
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
	 * get detail of first next no can found
	 *
	 * @param String $type
	 * @param Array $param
	 * @param Integer $nextNo
	 * @return
	 *
	 * @create 2021/01/06 Chien
	 * @update
	 */
	private function getLastNext($type, $param, $nextNo) {
		$temp = null;
		if ($type == 1) {
			$temp = Cyn_BlockKukaku::where('CKind', '=', $param['val1'])
									->where('ProjectID', '=', $param['val2'])
									->where('OrderNo', '=', $param['val3'])
									->where('No', '=', $nextNo)
									->first();
		} else {
			$temp = Cyn_C_BlockKukaku::where('CKind', '=', $param['val1'])
									->where('ProjectID', '=', $param['val2'])
									->where('OrderNo', '=', $param['val3'])
									->where('No', '=', $nextNo)
									->first();
		}
		if ($temp != null) {
			if ($temp->BKumiku != config('system_const.kumiku_code_sogumi')) {
				if ($temp->N_No == 0 || $temp->N_No == null) {
					$temp = null;
				} else {
					$temp = $this->getLastNext($type, $param, $temp->N_No);
				}
			}
		}
		return $temp;
	}

	/**
	 * prepare list year to bind cell J1
	 *
	 * @param Array $row
	 * @param Array &$arrRangeYear (reference)
	 * @return
	 *
	 * @create 2020/12/08 Chien
	 * @update
	 */
	private function getListYear($row, &$arrRangeYear) {
		$listCol = array('E', 'F', 'H', 'I', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R');
		if ($row['colB'] != '×' && $row['DDChild'] == '') {
			foreach ($listCol as $col) {
				if (!empty($row['timeCol' . $col])) {
					$arrRangeYear[date('Y', strtotime($row['timeCol' . $col]))][] = date('Y', strtotime($row['timeCol' . $col]));
				}
			}
		}
	}

	/**
	 * create list style for each cell in list Year appear in J1
	 *
	 * @param String $checkFather
	 * @param String $checkChild
	 * @param String $dataYear
	 * @param String $cell
	 * @param Array $listYearStyle
	 * @param Array &$arrStyleCellYear (reference)
	 * @return
	 *
	 * @create 2020/12/08 Chien
	 * @update
	 */
	private function createListStyleYear($checkFather, $checkChild, $dataCellRoot, $dataYear, $cell, $listYearStyle, &$arrStyleCellYear) {
		if ($checkFather != '×' && $checkChild == '' && $dataCellRoot != '' && $dataYear != '') {
			$year = date('Y', strtotime($dataYear));
			if (isset($listYearStyle[$year])) {
				$arrStyleCellYear[$listYearStyle[$year]][] = $cell;
			}
		}
	}
}
