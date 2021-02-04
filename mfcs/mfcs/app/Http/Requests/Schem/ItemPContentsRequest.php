<?php
/*
 * @ItemPContentsRequest.php
 *
 * @create 2020/11/16 Chien
 *
 * @update
 */
namespace App\Http\Requests\Schem;

use Illuminate\Contracts\Validation\Validator;
use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Validation\ValidationException;
use App\Models\Cyn_BlockKukaku;
use App\Models\Cyn_Plan;
use App\Models\Cyn_C_BlockKukaku;
use App\Models\Cyn_C_Plan;

/*
 * ItemPContentsRequest class
 *
 * @create 2020/11/16 Chien
 *
 * @update
 *
 */
class ItemPContentsRequest extends FormRequest
{
	/**
	 * Determine if the user is authorized to make this request.
	 *
	 * @return bool
	 * @create 2020/11/16 Chien
	 *
	 * @update
	 */
	public function authorize() {
		return true;
	}

	/**
	 * Get the validation rules that apply to the request.
	 *
	 * @return array
	 * @create 2020/11/16 Chien
	 *
	 * @update
	 */
	public function rules() {
		return [
			'val301' => 'required|string|max:2',
			'val302' => 'required|string|max:1',
			'val303' => 'nullable|string|max:10',
			'val304' => 'nullable|string|max:5',
			'val305' => 'nullable|numeric|min:0',
			'val306' => 'nullable|numeric|min:0',
			'val307' => 'nullable|integer|min:1|max:2147483647',
			'val308' => 'nullable',
			'val309' => 'nullable',
			'val310' => 'nullable|string|max:4',
		];
	}

	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 * @create 2020/11/16 Chien
	 *
	 * @update
	 */
	public function attributes() {
		return [
			'val301' => '工程',
			'val302' => '工程組区',
			'val303' => '施工棟',
			'val304' => '物量コード',
			'val305' => '物量',
			'val306' => 'HC',
			'val307' => '工期',
			'val308' => '次工程',
			'val309' => '次工程組区',
			'val310' => '定盤',
		];
	}

	/**
	 * 独自処理を追加する
	 *
	 * @return
	 * @create 2020/09/16 Chien
	 *
	 * @update
	 */
	public function withValidator($validator) {
		$val1 = isset($this->val1) ? valueUrlDecode($this->val1) : '';
		$val101 = isset($this->val101) ? valueUrlDecode($this->val101) : '';
		$val102 = isset($this->val102) ? valueUrlDecode($this->val102) : '';
		$val103 = isset($this->val103) ? valueUrlDecode($this->val103) : '';
		$val104 = isset($this->val104) ? valueUrlDecode($this->val104) : '';
		$val201 = isset($this->val201) ? valueUrlDecode($this->val201) : '';
		$val202 = isset($this->val202) ? valueUrlDecode($this->val202) : '';
		$val301 = isset($this->val301) ? trim($this->val301) : '';
		$val302 = isset($this->val302) ? trim($this->val302) : '';
		$val303 = isset($this->val303) ? trim($this->val303) : '';
		$val304 = isset($this->val304) ? trim($this->val304) : '';
		$val305 = isset($this->val305) ? trim($this->val305) : '';
		$val306 = isset($this->val306) ? trim($this->val306) : '';
		$val308 = isset($this->val308) ? trim($this->val308) : '';
		$val309 = isset($this->val309) ? trim($this->val309) : '';
		$val310 = isset($this->val310) ? trim($this->val310) : '';

		// val301
		if($val301 != '' && $val302 != '') {
			$checkVal301_1 = null;
			$checkVal301_2 = null;
			if($val103 == 0) {
				$checkVal301_1 = Cyn_Plan::where('Cyn_Plan.ProjectID', '=', $val101)
										->where('Cyn_Plan.OrderNo', '=', $val102)
										->where('Cyn_Plan.No', '=', $val104)
										->where('Cyn_Plan.Kotei', '=', $val301)
										->where('Cyn_Plan.KKumiku', '=', $val302)
										->where('Cyn_Plan.KoteiNo', '<>', $val202)
										->whereNull('Cyn_Plan.Del_Date')->first();
				if ($checkVal301_1 == null) {
					// update rev5
					$queryCynPlan = Cyn_Plan::select('Cyn_Plan.Kotei', 'Cyn_Plan.KKumiku')
											->where('Cyn_Plan.ProjectID', '=', $val101)
											->where('Cyn_Plan.OrderNo', '=', $val102)
											->where('Cyn_Plan.No', '=', $val104)
											->where('Cyn_Plan.KoteiNo', '=', $val202)->first();
					if($queryCynPlan != null && ($val301 != $queryCynPlan->Kotei || $val302 != $queryCynPlan->KKumiku)) {
						$checkVal301_2 = Cyn_Plan::where('Cyn_Plan.ProjectID', '=', $val101)
													->where('Cyn_Plan.OrderNo', '=', $val102)
													->where('Cyn_Plan.No', '=', $val104)
													->where('Cyn_Plan.N_Kotei', '=', $queryCynPlan->Kotei)
													->where('Cyn_Plan.N_KKumiku', '=', $queryCynPlan->KKumiku)
													->where('Cyn_Plan.KoteiNo', '<>', $val202)
													->whereNull('Cyn_Plan.Del_Date')->first();
					}
				}
			}
			if($val103 == 1) {
				$checkVal301_1 = Cyn_C_Plan::where('Cyn_C_Plan.ProjectID', '=', $val101)
											->where('Cyn_C_Plan.OrderNo', '=', $val102)
											->where('Cyn_C_Plan.No', '=', $val104)
											->where('Cyn_C_Plan.Kotei', '=', $val301)
											->where('Cyn_C_Plan.KKumiku', '=', $val302)
											->where('Cyn_C_Plan.KoteiNo', '<>', $val202)
											->whereNull('Cyn_C_Plan.Del_Date')->first();
				if ($checkVal301_1 == null) {
					// update rev5
					$queryCynCPlan = Cyn_C_Plan::select('Cyn_C_Plan.Kotei', 'Cyn_C_Plan.KKumiku')
												->where('Cyn_C_Plan.ProjectID', '=', $val101)
												->where('Cyn_C_Plan.OrderNo', '=', $val102)
												->where('Cyn_C_Plan.No', '=', $val104)
												->where('Cyn_C_Plan.KoteiNo', '=', $val202)->first();
					if($queryCynCPlan != null && ($val301 != $queryCynCPlan->Kotei || $val302 != $queryCynCPlan->KKumiku)) {
						$checkVal301_2 = Cyn_C_Plan::where('Cyn_C_Plan.ProjectID', '=', $val101)
													->where('Cyn_C_Plan.OrderNo', '=', $val102)
													->where('Cyn_C_Plan.No', '=', $val104)
													->where('Cyn_C_Plan.N_Kotei', '=', $queryCynCPlan->Kotei)
													->where('Cyn_C_Plan.N_KKumiku', '=', $queryCynCPlan->KKumiku)
													->where('Cyn_C_Plan.KoteiNo', '<>', $val202)
													->whereNull('Cyn_C_Plan.Del_Date')->first();
					}
				}
			}
			if($checkVal301_1 != null) {
				$validator->after(function ($validator) {
					$validator->errors()->add('val301', config('message.msg_cmn_db_013'));
				});
			}
			if($checkVal301_2 != null) {
				$validator->after(function ($validator) {
					$validator->errors()->add('val301', config('message.msg_cmn_db_033'));
				});
			}
		}

		// val305
		if($val305 != '') {
			preg_match('/^(\d{1,6}(?:[\.\,]\d{1,2})?)$/', $val305, $matches);
			if(count($matches) == 0) {
				$validator->after(function ($validator) {
					$validator->errors()->add('val305', config('message.msg_validation_006'));
				});
			}
		}

		// val306
		if($val306 != '') {
			preg_match('/^(\d{1,6}(?:[\.\,]\d{1,2})?)$/', $val306, $matches);
			if(count($matches) == 0) {
				$validator->after(function ($validator) {
					$validator->errors()->add('val306', config('message.msg_validation_006'));
				});
			}
		}

		// val308
		if($val308 == '' && $val309 != '') {
			$validator->after(function ($validator) {
				$validator->errors()->add('val308', config('message.msg_cmn_db_012'));
			});
		}
		if($val308 != '' && $val309 != '') {
			// case 1
			if($val301 == $val308 && $val302 == $val309) {
				$validator->after(function ($validator) {
					$validator->errors()->add('val308', config('message.msg_cmn_db_014'));
				});
			}

			$dataCheckCase2 = null;
			$dataCheckCase3 = array();
			if($val103 == 0) {
				// Cyn_Plan
				$dataCheckCase2 = Cyn_Plan::where('ProjectID', '=', $val101)
									->where('OrderNo', '=', $val102)
									->where('No', '=', $val104)
									->where('Kotei', '=', $val308)
									->where('KKumiku', '=', $val309)
									->whereNull('Del_Date')->first();

				$dataCheckCase3 = Cyn_Plan::select(
										'Cyn_Plan.Kotei',
										'Cyn_Plan.KKumiku',
										'Cyn_Plan.N_Kotei',
										'Cyn_Plan.N_KKumiku',
									)
									->join('Cyn_Plan as tableB', function($join) {
										$join->on('Cyn_Plan.N_Kotei', '=', 'tableB.Kotei')
											->on('Cyn_Plan.N_KKumiku', '=', 'tableB.KKumiku');
									})
									->where('Cyn_Plan.ProjectID', '=', $val101)
									->where('Cyn_Plan.OrderNo', '=', $val102)
									->where('Cyn_Plan.No', '=', $val104)
									->whereNull('Cyn_Plan.Del_Date')->distinct()->get();
			}
			if($val103 == 1) {
				// Cyn_C_Plan
				$dataCheckCase2 = Cyn_C_Plan::where('ProjectID', '=', $val101)
									->where('OrderNo', '=', $val102)
									->where('No', '=', $val104)
									->where('Kotei', '=', $val308)
									->where('KKumiku', '=', $val309)
									->whereNull('Del_Date')->first();

				$dataCheckCase3 = Cyn_C_Plan::select(
										'Cyn_C_Plan.Kotei',
										'Cyn_C_Plan.KKumiku',
										'Cyn_C_Plan.N_Kotei',
										'Cyn_C_Plan.N_KKumiku',
									)
									->join('Cyn_C_Plan as tableB', function($join) {
										$join->on('Cyn_C_Plan.N_Kotei', '=', 'tableB.Kotei')
											->on('Cyn_C_Plan.N_KKumiku', '=', 'tableB.KKumiku');
									})
									->where('Cyn_C_Plan.ProjectID', '=', $val101)
									->where('Cyn_C_Plan.OrderNo', '=', $val102)
									->where('Cyn_C_Plan.No', '=', $val104)
									->whereNull('Cyn_C_Plan.Del_Date')->distinct()->get();
			}
			// case 2
			if($dataCheckCase2 == null) {
				$validator->after(function ($validator) {
					$validator->errors()->add('val308', config('message.msg_cmn_db_010'));
				});
			}

			// case 3
			if(count($dataCheckCase3) > 0) {
				$arrInDB = $dataCheckCase3->toArray();
				// faker data if in DB
				$arrInDB[] = array(
					'Kotei' => $val301,
					'KKumiku' => $val302,
					'N_Kotei' => $val308,
					'N_KKumiku' => $val309,
				);
				// check infinity loop
				$arrListCheck = array();
				if($this->checkInfinityLoop($arrInDB, 0, $arrListCheck, -1, 0)) {
					$validator->after(function ($validator) {
						$validator->errors()->add('val308', config('message.msg_validation_012'));
					});
				}
			}
		}

		// val309
		if($val308 != '' && $val309 == '') {
			$validator->after(function ($validator) {
				$validator->errors()->add('val309', config('message.msg_cmn_db_011'));
			});
		}
		if($val309 != '' && $val302 != '') {
			if($val302 > $val309) {
				$validator->after(function ($validator) {
					$validator->errors()->add('val309', config('message.msg_validation_008'));
				});
			}
		}

		return $validator;
	}

	/**
	* check infinity loop data
	*
	* @return boolean true: has infinity data | false: hasn't infinity data
	* @create 2020/11/17 Chien
	*
	* @update
	*/
	private function checkInfinityLoop($data, $indexAI = 0, &$lstCheck, $nextIndex = -1, $startLine = 0) {
		if(!isset($data[$indexAI])) {
			return false;
		}
		$temp = ($nextIndex != -1) ? $data[$nextIndex] : $data[$indexAI];
		// have next -> find next
		if($temp['N_Kotei'] != '' && $temp['N_KKumiku'] != '') {
			foreach($data as $key => $row) {
				if($nextIndex != -1) {
					if($nextIndex == $key) {
						if($nextIndex == (count($data) - 1)) {
							return false;
						}
						continue;
					}
				} else {
					if($indexAI == $key) {
						continue;
					}
				}
				// find the next
				if($row['Kotei'] == $temp['N_Kotei'] && $row['KKumiku'] == $temp['N_KKumiku']) {
					// found the next
					if(count($lstCheck) == 0) {
						$lstCheck[$startLine][] = array($temp['Kotei'], $temp['KKumiku']);
						return $this->checkInfinityLoop($data, $indexAI, $lstCheck, $key, $startLine);
					} else {
						if(isset($lstCheck[$startLine])) {
							// check with first item in line -> infinity loop
							if($lstCheck[$startLine][0][0] == $temp['N_Kotei'] && $lstCheck[$startLine][0][1] == $temp['N_KKumiku']) {
								$lstCheck[$startLine][] = array($temp['Kotei'], $temp['KKumiku']);
								return true;
							} else {
								if(in_array(array($temp['Kotei'], $temp['KKumiku']), $lstCheck[$startLine])) {
									return true;
								}
								$lstCheck[$startLine][] = array($temp['Kotei'], $temp['KKumiku']);
								return $this->checkInfinityLoop($data, $indexAI, $lstCheck, $key, $startLine);
							}
						} else {
							return $this->checkInfinityLoop($data, ($indexAI + 1), $lstCheck, -1, ($startLine + 1));
						}
					}
				} else {
					if($key == (count($data) - 1)) {
						return false;
					}
					continue;
				}
			}
		} else {
			return $this->checkInfinityLoop($data, ($indexAI + 1), $lstCheck, -1, ($startLine + 1));
		}
	}

	/**
	* Get the validator instance for the request.
	*
	* @return \Illuminate\Contracts\Validation\Validator
	* @create 2020/11/17 Chien
	*
	* @update
	*/
	public function getValidatorInstance() {
		$this->decodeValRequest();
		return parent::getValidatorInstance();
	}

	/**
	* decode request.
	*
	* @return array
	* @create 2020/11/17 Chien
	*
	* @update
	*/
	protected function decodeValRequest() {
		if($this->request->has('val301')) {
			$this->merge([
				'val301' => valueUrlDecode($this->request->get('val301'))
			]);
		}
		if($this->request->has('val302')) {
			$this->merge([
				'val302' => valueUrlDecode($this->request->get('val302'))
			]);
		}
		if($this->request->has('val303')) {
			$this->merge([
				'val303' => valueUrlDecode($this->request->get('val303'))
			]);
		}
		if($this->request->has('val304')) {
			$this->merge([
				'val304' => valueUrlDecode($this->request->get('val304'))
			]);
		}
		if($this->request->has('val308')) {
			$this->merge([
				'val308' => valueUrlDecode($this->request->get('val308'))
			]);
		}
		if($this->request->has('val309')) {
			$this->merge([
				'val309' => valueUrlDecode($this->request->get('val309'))
			]);
		}
	}

	/**
	* failedValidation
	*
	* @return mix RedirectResponse
	* @create 2020/11/12 Chien
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
	* @create 2020/11/12 Chien
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

		// edit
		foreach($query as $key => $value) {
			if (strpos($key, 'val') !== false || $key == 'err1') {
				if(!in_array($key, array('val1','val2','val101','val102','val103','val104','val201','val202'))) {
					unset($query[$key]);
				}
			}
		}

		$arrUrl = explode('?', $url->previous());
		$queryString = '?'.http_build_query($query);

		return $arrUrl[0].$queryString;
	}
}