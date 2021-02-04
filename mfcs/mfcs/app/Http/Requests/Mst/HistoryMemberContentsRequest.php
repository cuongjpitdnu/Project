<?php
/*
 * @HistoryMemberContentsRequest.php
 * 
 * @create 2020/09/14 Cuong
 *
 * @update 2020/10/19 Cuong update check validate val301, val310
 * @update 2020/11/09 KBS S.Tanaka 日付が2桁表記の場合のバリデーションエラーを追加
 */
namespace App\Http\Requests\Mst;

use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Validation\ValidationException;
use Illuminate\Contracts\Validation\Validator;
use Illuminate\Validation\Rule;
use Illuminate\Database\Query\Builder;
use App\Librarys\FuncCommon;
use App\Models\MEMHist;
use App\Models\MstOrg;
use DB;
class HistoryMemberContentsRequest extends FormRequest
{
	/**
	* Determine if the user is authorized to make this request.
	*
	* @return bool
	* @create 2020/09/14 Cuong
	*
	* @update
	*/
	public function authorize()
	{
		return true;
	}

	/**
	 * Get the validation rules that apply to the request.
	 *
	 * @return array
	 * @create 2020/09/14 Cuong
	 *
	 * @update 2020/10/19 Cuong update check validate val301, val310
	 */
	public function rules()
	{
		return [
			'val301' => 'nullable|integer|min:0|max:2147483647',
			'val302' => 'required|numeric|between:-2147483648,2147483647',
			'val303' => 'nullable|numeric|between:-2147483648,2147483647',
			'val304' => 'nullable|numeric|between:-2147483648,2147483647',
			'val305' => 'required',
			'val306' => 'nullable',
			'val307' => 'nullable|numeric|between:-2147483648,2147483647',
			'val308' => 'required|string|max:5',
			'val309' => 'required|string|max:3',
			'val310' => 'nullable|integer|min:1|max:2147483647',
		];
	}

	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 * @create 2020/09/14 Cuong
	 *
	 * @update
	 */
	public function attributes(){
		return [
			'val301' => '社員番号',
			'val302' => '社内外フラグ',
			'val303' => '会社名',
			'val304' => '所属班',
			'val305' => '開始日',
			'val306' => '終了日',
			'val307' => '外注タイプ',
			'val308' => '職種',
			'val309' => 'プロパー',
			'val310' => '表示順',
		];
	}

	/**
	 * Get the validator instance for the request.
	 *
	 * @return \Illuminate\Contracts\Validation\Validator
	 * @create 2020/09/14 Cuong
	 *
	 * @update
	 */
	public function getValidatorInstance()
	{
		$this->decodeValRequest();
		return parent::getValidatorInstance();
	}

	/**
	 * decode request val304.
	 *
	 * @return array
	 * @create 2020/09/14 Cuong
	 *
	 * @update
	 */
	protected function decodeValRequest()
	{
		if($this->request->has('val304')){
			$this->merge([
				'val304' => valueUrlDecode($this->request->get('val304')),
			]);
		}
		if($this->request->has('old_val304')){
			$this->merge([
				'old_val304' => valueUrlDecode($this->request->get('old_val304')),
			]);
		}
	}

	/**
	 * 独自処理を追加する
	 *
	 * @return array
	 * @create 2020/09/14 Cuong
	 * 
	 * @update 2020/09/23 Cuong check validate val304
	 * @update 2020/11/09 KBS S.Tanaka 日付が2桁表記の場合のバリデーションエラーを追加
	 * @update 2020/11/25 upadate condition check validate val301,val304,val305,val306
	 * @update 2020/12/15 upadate condition check validate val301 (010607_Rev11)
	 * @update 2020/12/28 upadate condition check validate val301, val304 (010607_Rev12)
	 * @update 2021/01/05 upadate condition check validate val301, val304 (010607_Rev13)
	 */
	public function withValidator($validator)
	{
		$method = $this->input('method');
		$id = valueUrlDecode($this->val101);
		$val201 = valueUrlDecode($this->val201);
		$sDate = $this->input('val305');
		$eDate = $this->input('val306');

		// check validate val301
		if(!is_null($this->val301)) {
			$mstObjMemHist = MEMHist::where('MEM_ID', '!=', $id)
						->where('WorkerNo', '=', $this->val301)
						->get();

			if(count($mstObjMemHist) > 0) {
				$validator->after(function ($validator) {
					$validator->errors()->add('val301', config('message.msg_member_if_004'));
				});
			}
		}

		if(is_null($sDate)){
			$sDateResult = true;
			$sDateMsg = null;
		}else{
			list($sDateResult, $sDateMsg) = FuncCommon::checkFormatDate($sDate);
		}
		if(is_null($eDate)){
			$eDate = '';
			$eDateResult = true;
			$eDateMsg = null;
		}else{
			list($eDateResult, $eDateMsg) = FuncCommon::checkFormatDate($eDate);
		}

		if($sDateResult && $eDateResult && !is_null($sDate)) {
			$rules = ['val306' => 'after:val305'];
			$validator->addRules($rules);
		}else{
			if(!$sDateResult){
				$validator->after(function ($validator) use($sDateMsg){
					$validator->errors()->add('val305', sprintf($sDateMsg, '開始日'));
				});
			}
			if(!$eDateResult){
				$validator->after(function ($validator) use($eDateMsg){
					$validator->errors()->add('val306', sprintf($eDateMsg, '終了日'));
				});
			}
		}

		if( (!empty($sDate) && $sDateResult) && ( empty($eDate) || (!empty($eDate) && $eDateResult) )) {
			//check validate val304
			$orgID = $this->val304;
			if(!is_null($orgID) && $orgID != 0){
				$flag = false;
				$mstOrg = MstOrg::where('ID', '=', $orgID)
								->whereRaw('( SELECT MIN ( SDate ) FROM mstOrg WHERE ID = ? ) <= ? ', [$orgID,$sDate])
								->whereRaw('( ( SELECT MAX ( EDate ) FROM mstOrg WHERE ID = ? ) >= ? OR EDate IS NULL )', [$orgID,$eDate])
								->get()->toArray();

				if (count($mstOrg) == 0) {
					$validator->after(function ($validator) {
						$validator->errors()->add('val304', config('message.msg_member_if_003'));
					});
				}else {
					$arrSDate = array_column($mstOrg, 'Sdate');
					$maxSDate = max($arrSDate);

					$mstOrgMaxSDate = array_values(array_filter($mstOrg, function ($org) use ($maxSDate) {
						return ($org['Sdate'] == $maxSDate);
					}));

					if($mstOrgMaxSDate[0]['FolderFlag'] == 1) {
						$flag = true;
					}
					$validator->after(function ($validator) use ($flag) {
						if ($flag) {
							$validator->errors()->add('val304', config('message.msg_member_if_002'));
						}
					});
				}
			}
		}

		$result = array();
		if(!empty($sDate) && $sDateResult) {
			if ($this->input('method') == 'create') {
				if (is_null($eDate)) {
					$result = MEMHist::where(function ($query) use($id, $sDate) {
							$query->where('MEMHist.MEM_ID', '=', $id)
							->where(function($query1) use($sDate) {
								$query1->whereDate('MEMHist.Edate', '>=', $sDate)
									->orWhereNull('MEMHist.EDate');
							});
						})->get();
				}elseif($eDateResult) {
					$result = MEMHist::where(function ($query) use($id, $sDate, $eDate) {
							$query->where('MEMHist.MEM_ID', '=', $id)
								->where(function($query1) use($sDate, $eDate) {
									$query1->where(function($query2) use($sDate, $eDate) {
											$query2->where('MEMHist.Sdate', '<=', $sDate)
												->where(function($query3) use($sDate, $eDate) {
													$query3->whereDate('MEMHist.Edate', '>=', $sDate)
														->orWhereNull('MEMHist.EDate');
												});

								})
								->orWhere(function($query2) use($sDate, $eDate) {
									$query2->where('MEMHist.Sdate', '<=', $eDate)
										->where(function($query3) use($sDate, $eDate) {
											$query3->whereDate('MEMHist.Edate', '>=', $eDate)
												->orWhereNull('MEMHist.EDate');
										});

								})
								->orWhere(function($query2) use($sDate, $eDate) {
									$query2->whereDate('MEMHist.Sdate', '>=', $sDate)
									->whereDate('MEMHist.Edate', '<=', $eDate);
								});
							});
						})->get();
				}
			}

			if ($this->input('method') == 'edit') {
				if (is_null($eDate)) {
					$result = MEMHist::where(function ($query) use($id, $sDate, $eDate, $val201) {
							$query->where('MEMHist.MEM_ID', '=', $id)
							->where('MEMHist.Sdate', '!=', $val201)
							->where('MEMHist.Sdate', '<=', $sDate)
							->where(function($query1) use($sDate, $eDate) {
								$query1->whereDate('MEMHist.Edate', '>=', $sDate)
									->orWhereNull('MEMHist.EDate');
							});
						})->get();
				}elseif($eDateResult) {
					$result = MEMHist::where(function ($query) use($id, $sDate, $eDate, $val201) {
							$query->where('MEMHist.MEM_ID', '=', $id)
							->where('MEMHist.Sdate', '!=', $val201)
							->where(function($query1) use($sDate, $eDate) {
								$query1->where(function($query2) use($sDate, $eDate) {
										$query2->where('MEMHist.Sdate', '<=', $sDate)
											->where(function($query3) use($sDate, $eDate) {
												$query3->whereDate('MEMHist.Edate', '>=', $sDate)
													->orWhereNull('MEMHist.EDate');
											});

								})
								->orWhere(function($query2) use($sDate, $eDate) {
									$query2->where('MEMHist.Sdate', '<=', $eDate)
										->where(function($query3) use($sDate, $eDate) {
											$query3->whereDate('MEMHist.Edate', '>=', $eDate)
												->orWhereNull('MEMHist.EDate');
										});

								})
								->orWhere(function($query2) use($sDate, $eDate) {
									$query2->whereDate('MEMHist.Sdate', '>=', $sDate)
									->whereDate('MEMHist.Edate', '<=', $eDate);
								});
							});
						})->get();
				}
			}
			$validator->after(function ($validator) use ($result) {
				if (count($result) > 0) {
					$validator->errors()->add('val305', config('message.msg_cmn_db_005'));
				}
			});
		}

		return $validator;
	}

	/**
	* failedValidation
	*
	* @return mix RedirectResponse
	* @create 2020/11/27 Cuong
	*
	* @update
	*/
	protected function failedValidation(Validator $validator) {

		throw (new ValidationException($validator))
					->errorBag($this->errorBag)
					->redirectTo($this->getRedirectUrl());
	}

	/**
	* get redirect url after failedValidation
	*
	* @return string
	* @create 2020/11/27 Cuong
	*
	* @update
	*/
	protected function getRedirectUrl() {
		$url = $this->redirector->getUrlGenerator();

		if ($this->redirect) {
			return $url->to($this->redirect);
		} elseif ($this->redirectRoute) {
			return $url->route($this->redirectRoute);
		} elseif ($this->redirectAction) {
			return $url->action($this->redirectAction);
		}
		$parseUrl = parse_url($url->previous());
		parse_str($parseUrl['query'], $query);

		foreach($query as $key => $value) {
			if (strpos($key, 'val') !== false || $key == 'err1') {
				if(!in_array($key, ['val1','val2','val3','val4','val5','val101','val201','val202'])){
					unset($query[$key]);
				}
			}
		}

		$arrUrl = explode('?', $url->previous());
		$queryString = '?'.http_build_query($query);

		return $arrUrl[0].$queryString;
	}
}
