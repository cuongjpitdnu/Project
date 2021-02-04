<?php
/*
 * @AbilityController.php
 * 能力時間マスタコントローラーファイル
 *
 * @create 2020/08/18 KBS S.Tanaka
 *
 * @update 
 */

namespace App\Http\Controllers\Mst;

use App\Http\Controllers\Controller;
use App\Librarys\FuncCommon;
use App\Librarys\MstOrgCommon;
use App\Models\MstAbility;
use App\Models\MstDist;
use App\Models\MstFloor;
use App\Models\MstOrg;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use Illuminate\Database\QueryException;
use App\Http\Requests\Mst\AbilityContentsRequest;
use DateTime;

/*
 * 能力時間マスタコントローラー
 *
 * @create 2020/08/18 KBS S.Tanaka
 *
 * @update
 */
class AbilityController extends Controller
{
	/**
	 * GET 能力時間マスタトップ画面アクション
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/08/18 KBS S.Tanaka
	 * @update
	 */
	public function index(Request $request)
	{
		return $this->initialize($request);
	}

	/**
	 * 能力時間マスタトップ画面初期表示処理
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/08/18 KBS S.Tanaka
	 * @update
	 */
	private function initialize(Request $request)
	{

		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_all'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		//現在の日付取得
		$date = date("Y/m/d");

		if(isset($request->pageunit) && in_array($request->pageunit, [config('system_const.displayed_results_1'),
																	  config('system_const.displayed_results_2'),
																	  config('system_const.displayed_results_3')])){
			$pageunit = $request->pageunit;
		}else{
			$pageunit = config('system_const.displayed_results_1');
		}

		$direction = (isset($request->direction) && $request->direction != '') ?  $request->direction : 'asc';

		$sort = ['fld8'];
		if (isset($request->sort) && $request->sort != '') {
			$sort = [$request->sort, 'fld8'];
		}

		$query = MstAbility::select('mstAbility.AbilityName as fld1')
						->selectRaw('(
							case 
							when mstAbility.GroupID = 0 then \'*\' 
							else mstOrg.Name 
							end
							) as fld2,(
							case 
							when mstAbility.FloorCode is NULL then \'*\' 
							else mstFloor.Name 
							end
							) as fld3,(
							case 
							when mstAbility.DistCode is NULL then \'*\' 
							else mstDist.Name 
							end
							) as fld4'
						)
						->addselect(
							'mstAbility.Sdate as fld5',
							'mstAbility.Edate as fld6',
							'mstAbility.Hr as fld7',
							'mstAbility.ID as fld8',
						)
						->leftjoin('mstOrg', function ($join) use($date) {
							$join->on('mstAbility.GroupID', '=', 'mstOrg.ID')
								->where(function ($query1) use($date) {
									$query1->whereDate('mstOrg.Sdate', '<=', $date)
										->whereDate('mstOrg.Edate', '>=', $date)
										->orwhere(function($query2) use($date) {
											$query2->whereDate('mstOrg.Sdate', '<=', $date)
												->whereNull('mstOrg.Edate');
										});
								});
						})
						->leftjoin('mstFloor', 'mstAbility.FloorCode', '=', 'mstFloor.Code')
						->leftjoin('mstDist', 'mstAbility.DistCode', '=', 'mstDist.Code')
						->get();

		$abilities = $this->sortAndPagination($query, $sort, $direction, $pageunit, $request);

		$abilities->getCollection()->transform(function ($value) {
			//終了日のnull判定
			if(is_null($value['fld6'])){
				$value['fld6'] = '-';
			}
			//日付のフォーマット直し
			if($value['fld5'] != '-'){
				$sDate = new Datetime($value['fld5']);
				$value['fld5'] = $sDate->format('Y/m/d');
			}
			if($value['fld6'] != '-'){
				$eDate = new Datetime($value['fld6']);
				$value['fld6'] = $eDate->format('Y/m/d');
			}
			//工数のフォーマット直し
			$value['fld7'] = FuncCommon::formatDecToChar($value['fld7'], 2);
			return $value;
		});

		//ビューを表示
		return view('mst/ability/index')->with([
			'request' => $request,
			'abilities' => $abilities,
			'menuInfo' => $menuInfo,
		]);
	}

	/**
	 * GET 能力時間マスタ新規ボタンアクション
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/08/24　S.Tanaka
	 * @update
	 */
	public function create(Request $request)
	{
		
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$abilityData = array();
		$originalError = array();
		
		$abilityData['GroupName'] = config('system_const.org_null_name');
		$abilityData['GroupID'] = '0';

		if(isset($request->err1)){
			for($i=1; $i<=8; $i++){
				$request->{"val".$i} = valueUrlDecode($request->{"val".$i});
			}
			$originalError[] = valueUrlDecode($request->err1);

			$abilityData['AbilityName'] = $request->val1;
			$abilityData['GroupID'] = $request->val2;
			$abilityData['FloorCode'] = $request->val3;
			$abilityData['DistCode'] = $request->val4;
			$sDate = new DateTime($request->val5);
			$abilityData['Sdate'] = $sDate->format('Y/m/d');
			if(is_null($request->val6)){
				$abilityData['Edate'] = '';
			}else{
				$eDate = new DateTime($request->val6); 
				$abilityData['Edate'] = $eDate->format('Y/m/d');
			}
			$abilityData['Hr'] = $request->val7;
			$abilityData['GroupName'] = $request->val8;
		}

		$Floor = MstFloor::select('Name','Code')
						->where('ViewFlag', '!=', '0')
						->orderBy('SortNo', 'asc')
						->get();

		$Dist = MstDist::select('Name','Code')
						->orderBy('Code', 'asc')
						->get();

		$mstOrgCommon = new mstOrgCommon();

		$menuid = valueUrlDecode($request->cmn2);
		$kindid = valueUrlDecode($request->cmn1);

		return view('mst/ability/create')->with([
			'menuInfo' => $menuInfo,
			'abilityData' => $abilityData,
			'request' => $request,
			'originalError' => $originalError,
			'Floor' => $Floor,
			'Dist' => $Dist,
			'mstOrgCommon' => $mstOrgCommon,
		]);
	}

	/**
	 * GET 能力時間マスタ編集ボタンアクション
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/08/24　S.Tanaka
	 * @update
	 */
	public function edit(Request $request)
	{
		
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		//現在の日付取得
		$date = date("Y/m/d");

		$abilityData = array();
		$originalError = array();

		if(isset($request->err1)){
			for($i=1; $i<=10; $i++){
				$request->{"val".$i} = valueUrlDecode($request->{"val".$i});
			}
			$originalError[] = valueUrlDecode($request->err1);

			$abilityData['AbilityName'] = $request->val1;
			$abilityData['GroupID'] = $request->val2;
			$abilityData['FloorCode'] = $request->val3;
			$abilityData['DistCode'] = $request->val4;
			$sDate = new DateTime($request->val5);
			$abilityData['Sdate'] = $sDate->format('Y/m/d');
			if(is_null($request->val6)){
				$abilityData['Edate'] = '';
			}else{
				$eDate = new DateTime($request->val6); 
				$abilityData['Edate'] = $eDate->format('Y/m/d');
			}
			$abilityData['Hr'] = $request->val7;
			$abilityData['Updated_at'] = $request->val8;
			$abilityData['GroupName'] = $request->val9;
			$abilityData['ID'] = $request->val10;
			$floorName = MstFloor::select('Name')
								->where('Code', '=', $request->val3)
								->get();
			$abilityData['FloorName'] = (isset($floorName[0]->Name) ? $floorName[0]->Name : '');
			$distName = MstDist::select('Name')
							->where('Code', '=', $request->val4)
							->get();
			$abilityData['DistName'] = (isset($distName[0]->Name) ? $distName[0]->Name : '');
		}else{
			$id = valueUrlDecode($request->val1);
			
			$data = MstAbility::select('mstAbility.AbilityName as fld1',
									   'mstAbility.GroupID as fld2',
									   'mstOrg.Name as fld3',
									   'mstFloor.Name as fld4',
									   'mstDist.Name as fld5',
									   'mstAbility.Sdate as fld6',
									   'mstAbility.Edate as fld7',
									   'mstAbility.Hr as fld8',
									   'mstAbility.ID as fld9',
									   'mstAbility.Updated_at as fld10',
									   'mstAbility.FloorCode as fld11',
									   'mstAbility.DistCode as fld12',
									   )
							->leftjoin('mstOrg', function ($join) use($date) {
								$join->on('mstAbility.GroupID', '=', 'mstOrg.ID')
									->where(function ($query1) use($date) {
										$query1->whereDate('mstOrg.Sdate', '<=', $date)
											->whereDate('mstOrg.Edate', '>=', $date)
											->orwhere(function($query2) use($date) {
												$query2->whereDate('mstOrg.Sdate', '<=', $date)
													->whereNull('mstOrg.Edate');
											});
									});
							})
							->leftjoin('mstFloor', 'mstAbility.FloorCode', '=', 'mstFloor.Code')
							->leftjoin('mstDist', 'mstAbility.DistCode', '=', 'mstDist.Code')
							->where('mstAbility.ID', '=', $id)
							->get();
						
			$abilityData['AbilityName'] = $data[0]->fld1;
			$abilityData['GroupID'] = $data[0]->fld2;
			$abilityData['FloorCode'] = $data[0]->fld11;
			$abilityData['DistCode'] = $data[0]->fld12;
			$sDate = new DateTime($data[0]->fld6);
			$abilityData['Sdate'] = $sDate->format('Y/m/d');
			if(is_null($data[0]->fld7)){
				$abilityData['Edate'] = '';
			}else{
				$eDate = new DateTime($data[0]->fld7); 
				$abilityData['Edate'] = $eDate->format('Y/m/d');
			}
			//工数のフォーマット直し
			if(!is_null($data[0]->fld8)){
				$abilityData['Hr'] = FuncCommon::formatDecToText($data[0]->fld8);
			}else{
				$abilityData['Hr'] = null;
			}
			$abilityData['ID'] = $data[0]->fld9;
			$abilityData['GroupName'] = is_null($data[0]->fld3)?config('system_const.org_null_name'):$data[0]->fld3;
			$abilityData['FloorName'] = is_null($data[0]->fld4)?'':$data[0]->fld4;
			$abilityData['DistName'] = is_null($data[0]->fld5)?'':$data[0]->fld5;
			$abilityData['Updated_at'] = $data[0]->fld10;
		}

		$Floor = MstFloor::select()
						->where('ViewFlag', '!=', '0')
						->orderBy('SortNo', 'asc')
						->get();

		$Dist = MstDist::select()
						->orderBy('Code', 'asc')
						->get();

		return view('mst/ability/edit')->with([
			'menuInfo' => $menuInfo,
			'abilityData' => $abilityData,
			'request' => $request,
			'originalError' => $originalError,
			'Floor' => $Floor,
			'Dist' => $Dist,
		]);
	}

	/**
	 * GET 能力時間マスタ参照ボタンアクション
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/08/24　S.Tanaka
	 * @update
	 */
	public function show(Request $request)
	{
		
		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_readonly'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		$id = valueUrlDecode($request->val1);

		//現在の日付取得
		$date = date("Y/m/d");

		$data = MstAbility::select('mstAbility.AbilityName as fld1',
								   'mstOrg.Name as fld2',
								   'mstFloor.Name as fld3',
								   'mstDist.Name as fld4',
								   'mstAbility.Sdate as fld5',
								   'mstAbility.Edate as fld6',
								   'mstAbility.Hr as fld7',
								   )
						->leftjoin('mstOrg', function ($join) use($date) {
							$join->on('mstAbility.GroupID', '=', 'mstOrg.ID')
								->where(function ($query1) use($date) {
									$query1->whereDate('mstOrg.Sdate', '<=', $date)
										->whereDate('mstOrg.Edate', '>=', $date)
										->orwhere(function($query2) use($date) {
											$query2->whereDate('mstOrg.Sdate', '<=', $date)
												->whereNull('mstOrg.Edate');
										});
								});
						})
						->leftjoin('mstFloor', 'mstAbility.FloorCode', '=', 'mstFloor.Code')
						->leftjoin('mstDist', 'mstAbility.DistCode', '=', 'mstDist.Code')
						->where('mstAbility.ID', '=', $id)
						->get();
						
		$abilityData['AbilityName'] = $data[0]->fld1;
		$abilityData['GroupName'] = is_null($data[0]->fld2)?config('system_const.org_null_name'):$data[0]->fld2;
		$abilityData['FloorCode'] = $data[0]->fld3;
		$abilityData['DistCode'] = $data[0]->fld4;
		$sDate = new DateTime($data[0]->fld5);
		$abilityData['Sdate'] = $sDate->format('Y/m/d');
		if(is_null($data[0]->fld6)){
			$abilityData['Edate'] = '';
		}else{
			$eDate = new DateTime($data[0]->fld6); 
			$abilityData['Edate'] = $eDate->format('Y/m/d');
		}
		//工数のフォーマット直し
		if(!is_null($data[0]->fld7)){
			$abilityData['Hr'] = FuncCommon::formatDecToText($data[0]->fld7);
		}else{
			$abilityData['Hr'] = null;
		}

		return view('mst/ability/show')->with([
			'menuInfo' => $menuInfo,
			'abilityData' => $abilityData,
			'request' => $request,
		]);
	}

	/**
	 * 能力時間マスタ保存前確認処理
	 *
	 * @param Request 呼び出し元リクエストオブジェクト
	 * @param mix MenuInfo RedirectResponse
	 * @return 正常時 true(boolean)、エラー時 リダイレクト先URL(string)
	 *
	 * @create 2020/08/26　S.Tanaka
	 * @update 2020/11/10　S.Tanaka　バリデーション前からバリデーション後の処理に変更
	 */
	private function checkSave($request, $menuInfo)
	{
		$method = $request->method;
		$groupId = $request->val2;
		$floorCode = $request->val3;
		if(!is_null($request->val4) && mb_strlen($request->val4) <= 5){
			$distCode = $request->val4. str_repeat(' ', 5 - mb_strlen($request->val4));
		}else{
			$distCode = $request->val4;
		}
		$sDate = $request->val5;
		$eDate = $request->val6;
		
		if($method == 'edit'){
			$id = valueUrlDecode($request->val10);

			$existsData = MstAbility::where('GroupID', '=', $groupId)
									->where('FloorCode', '=', $floorCode)
									->where('DistCode', '=', $distCode)
									->where('ID', '!=', $id)
									->where(function ($query1) use($sDate, $eDate) {
										$query1->where(function ($query2) use($sDate, $eDate) {
													$query2->whereNotNull('Edate')
														->whereDate('Edate', '>=', $sDate)
														->when($eDate != '', function ($query3) use($eDate) {
															$query3->whereDate('Sdate', '<=', $eDate);
														});
												})
												->orwhere(function($query4) use($sDate, $eDate) {
													$query4->whereNull('Edate')
														->when($eDate != '',function($query5) use($eDate) {
															$query5->whereDate('Sdate', '<=', $eDate);
														});
												});
									})
									->exists();
		}else{
			$existsData = MstAbility::where('GroupID', '=', $groupId)
									->where('FloorCode', '=', $floorCode)
									->where('DistCode', '=', $distCode)
									->where(function ($query1) use($sDate, $eDate) {
										$query1->where(function ($query2) use($sDate, $eDate) {
													$query2->whereNotNull('Edate')
														->whereDate('Edate', '>=', $sDate)
														->when($eDate != '', function ($query3) use($eDate) {
															$query3->whereDate('Sdate', '<=', $eDate);
														});
												})
												->orwhere(function($query4) use($sDate, $eDate) {
													$query4->whereNull('Edate')
														->when($eDate != '',function($query5) use($eDate) {
															$query5->whereDate('Sdate', '<=', $eDate);
														});
												});
									})
									->exists();
		}

		if($existsData){
			$originalError = config('message.msg_cmn_db_007');
			$url = url('/');
			$url .= '/' . $menuInfo->KindURL;
			$url .= '/' . $menuInfo->MenuURL;
			$url .= '/' . $method;
			$url .= '?cmn1=' . $request->cmn1;
			$url .= '&cmn2=' . $request->cmn2;
			$url .= '&page=' . $request->page;
			$url .= '&pageunit=' . $request->pageunit;
			$url .= '&sort=' . $request->sort;
			$url .= '&direction=' . $request->direction;
			for($i=1; $i<=7; $i++){
				$key = 'val'.$i;
				$url .= '&val'.$i.'=' . valueUrlEncode($request->$key);
			}
			if($method == 'create'){
				$url .= '&val8=' . valueUrlEncode($request->orgname);
			}elseif($method == 'edit'){
				$url .= '&val8=' . valueUrlEncode($request->val8);
				$url .= '&val9=' . valueUrlEncode($request->orgname);
				$url .= '&val10=' . $request->val10;
			}
			$url .= '&err1=' . valueUrlEncode($originalError);

			return $url;
		}

		return true;
	}

	/**
	 * POST 能力時間マスタ保存ボタンアクション
	 *
	 * @param AbilityContentsRequest 呼び出し元リクエストオブジェクト
	 * @return View ビュー
	 *
	 * @create 2020/08/28　S.Tanaka
	 * @update
	 */
	public function save(AbilityContentsRequest $request)
	{

		// 初期処理
		$menuInfo = $this->checkLogin($request, config('system_const.authority_editable'));

		// 戻り値のデータ型をチェック
		if ($this->isRedirectMenuInfo($menuInfo)) {
			// エラーが起きたのでリダイレクト
			return $menuInfo;
		}

		//保存前確認処理
		$checkSaveResult = $this->checkSave($request, $menuInfo);
		if($checkSaveResult !== true){
			return redirect($checkSaveResult);
		}

		if($request->method == 'create'){
			$ability = new MstAbility;
			try {
				DB::transaction(function () use ($ability, $menuInfo, $request, &$result) {
					$ability['AbilityName'] = $request['val1'];
					$ability['GroupID'] = (is_null($request['val2']) ? '0' : $request['val2']);
					$ability['FloorCode'] = $request['val3'];
					$ability['DistCode'] = $request['val4'];
					$ability['Sdate'] = $request['val5'];
					$ability['Edate'] = $request['val6'];
					$ability['Hr'] = (is_null($request['val7']) ? '0' : $request['val7']);
					$ability['Up_User'] = $menuInfo->UserID;
					$value = DB::select('select NEXT VALUE FOR seq_mstAbility');
					$value = array_values((array) $value[0]);
					$ability['ID'] = $value[0];
					$ability->save();
				});
			} catch (QueryException $e) {
				if ($request->method == 'create' && $e->getCode() == '23000'){
					$originalError = config('message.msg_cmn_db_006');
					$url = url('/');
					$url .= '/' . $menuInfo->KindURL;
					$url .= '/' . $menuInfo->MenuURL;
					$url .= '/create';
					$url .= '?cmn1=' . $request->cmn1;
					$url .= '&cmn2=' . $request->cmn2;
					$url .= '&page=' . $request->page;
					$url .= '&pageunit=' . $request->pageunit;
					$url .= '&sort=' . $request->sort;
					$url .= '&direction=' . $request->direction;
					for($i=1; $i<=7; $i++){
						$key = 'val'.$i;
						$url .= '&val'.$i.'=' . valueUrlEncode($request->$key);
					}
					$url .= '&val8=' . valueUrlEncode($request->orgname);
					$url .= '&err1=' . valueUrlEncode($originalError);
					return redirect($url);
				}
				throw $e;
			}
		}elseif($request->method == 'edit'){
			try {
				$ability['AbilityName'] = $request['val1'];
				$ability['GroupID'] = $request['val2'];
				$ability['FloorCode'] = $request['val3'];
				$ability['DistCode'] = $request['val4'];
				$ability['Sdate'] = $request['val5'];
				$ability['Edate'] = $request['val6'];
				$ability['Hr'] = (is_null($request['val7']) ? '0' : $request['val7']);
				$ability['Up_User'] = $menuInfo->UserID;

				$request->val10 = valueUrlDecode($request->val10);
				$result = MstAbility::where('ID', '=', $request->val10)
									->where('Updated_at', '=', $request->val8)
									->update($ability);
				if(!$result){
					$originalError = config('message.msg_cmn_db_002');
					$url = url('/');
					$url .= '/' . $menuInfo->KindURL;
					$url .= '/' . $menuInfo->MenuURL;
					$url .= '/edit';
					$url .= '?cmn1=' . $request->cmn1;
					$url .= '&cmn2=' . $request->cmn2;
					$url .= '&page=' . $request->page;
					$url .= '&pageunit=' . $request->pageunit;
					$url .= '&sort=' . $request->sort;
					$url .= '&direction=' . $request->direction;
					for($i=1; $i<=7; $i++){
						$key = 'val'.$i;
						$url .= '&val'.$i.'=' . valueUrlEncode($request->$key);
					}
					$url .= '&val8=' . valueUrlEncode($request->val8);
					$url .= '&val9=' . valueUrlEncode($request->orgname);
					$url .= '&val10=' . valueUrlEncode($request->val10);
					$url .= '&err1=' . valueUrlEncode($originalError);

					return redirect($url);
				}
			} catch (QueryException $e) {
				if($request->method == 'edit' && $e->getCode() == '23000'){
					$originalError = config('message.msg_cmn_db_006');
					$url = url('/');
					$url .= '/' . $menuInfo->KindURL;
					$url .= '/' . $menuInfo->MenuURL;
					$url .= '/edit';
					$url .= '?cmn1=' . $request->cmn1;
					$url .= '&cmn2=' . $request->cmn2;
					$url .= '&page=' . $request->page;
					$url .= '&pageunit=' . $request->pageunit;
					$url .= '&sort=' . $request->sort;
					$url .= '&direction=' . $request->direction;
					for($i=1; $i<=7; $i++){
						$key = 'val'.$i;
						$url .= '&val'.$i.'=' . valueUrlEncode($request->$key);
					}
					$url .= '&val8=' . valueUrlEncode($request->val8);
					$url .= '&val9=' . valueUrlEncode($request->orgname);
					$url .= '&val10=' . valueUrlEncode($request->val10);
					$url .= '&err1=' . valueUrlEncode($originalError);

					return redirect($url);
				}
				throw $e;
			}
		}

		$url = url('/');
		$url .= '/' . $menuInfo->KindURL;
		$url .= '/' . $menuInfo->MenuURL;
		$url .= '/index';
		$url .= '?cmn1=' . $request->cmn1;
		$url .= '&cmn2=' . $request->cmn2;
		$url .= '&page=' . $request->page;
		$url .= '&pageunit=' . $request->pageunit;
		$url .= '&sort=' . $request->sort;
		$url .= '&direction=' . $request->direction;

		return redirect($url);
	}
}