<?php
/*
 * @ItemContentsRequest.php
 *
 * @create 2020/11/03 Chien
 *
 * @update
 */
namespace App\Http\Requests\Schem;

use Illuminate\Contracts\Validation\Validator;
use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Validation\ValidationException;
use App\Models\Cyn_BlockKukaku;
use App\Models\Cyn_C_BlockKukaku;

/*
 * ItemContentsRequest class
 *
 * @create 2020/11/03 Chien
 *
 * @update
 *
 */
class ItemContentsRequest extends FormRequest
{
	/**
	 * Determine if the user is authorized to make this request.
	 *
	 * @return bool
	 * @create 2020/11/03 Chien
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
	 * @create 2020/11/03 Chien
	 *
	 * @update
	 */
	public function rules() {
		return [
			'val201' => 'required|string|max:50',
			'val202' => 'required|string|max:1',
			'val203' => 'required|string|max:50',
			'val204' => 'required|string|max:1',
			'val205' => 'nullable',
			'val206' => 'nullable',
			'val207' => 'nullable|string|max:2|regex:/^[a-zA-Z0-9]*$/',
			'val208' => 'nullable|string|max:5|regex:/^[a-zA-Z0-9]*$/',
			'val209' => 'nullable|numeric|min:0',
			'val210' => 'nullable|numeric|min:0',
			'val211' => 'nullable|numeric|min:0',
			'val212' => 'nullable|numeric|min:0',
			'val213' => 'nullable|string|max:6|regex:/^[\x21-\x7E]+$/',
			'val214' => 'nullable|numeric|min:0',
			'val215' => 'nullable|in:0,1',
			'val216' => 'nullable|in:0,1',
		];
	}

	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 * @create 2020/11/03 Chien
	 *
	 * @update
	 */
	public function attributes() {
		return [
			'val201' => (valueUrlDecode($this->val1) == config('system_const.c_kind_giso')) ? '区画名' : 'ブロック名',
			'val202' => '組区',
			'val203' => (valueUrlDecode($this->val1) == config('system_const.c_kind_giso')) ? '区画名' : 'ブロック名',
			'val204' => '組区',
			'val205' => (valueUrlDecode($this->val1) == 2) ? '次区画名' : '次ブロック名',
			'val206' => '次組区',
			'val207' => '部位',
			'val208' => 'カテゴリー',
			'val209' => '代表幅',
			'val210' => '代表長',
			'val211' => '代表高',
			'val212' => '代表重量',
			'val213' => '工作図No',
			'val214' => '殻艤重量',
			'val215' => '重量確定',
			'val216' => '曲がり',
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
		$val101 = isset($this->val101) ? valueUrlDecode($this->val101) : '';
		$val102 = isset($this->val102) ? valueUrlDecode($this->val102) : '';
		$val103 = isset($this->val103) ? valueUrlDecode($this->val103) : '';
		$val104 = isset($this->val104) ? valueUrlDecode($this->val104) : '';
		$val201 = isset($this->val201) ? trim($this->val201) : '';
		$val202 = isset($this->val202) ? trim($this->val202) : '';
		$val203 = isset($this->val203) ? trim($this->val203) : '';
		$val204 = isset($this->val204) ? trim($this->val204) : '';
		$val205 = isset($this->val205) ? trim($this->val205) : '';
		$val206 = isset($this->val206) ? trim($this->val206) : '';
		$val209 = isset($this->val209) ? trim($this->val209) : '';
		$val210 = isset($this->val210) ? trim($this->val210) : '';
		$val211 = isset($this->val211) ? trim($this->val211) : '';
		$val212 = isset($this->val212) ? trim($this->val212) : '';
		$val214 = isset($this->val214) ? trim($this->val214) : '';

		// val203
		// Cyn_BlockKukaku
		$dataCheckVal203_1 = null;
		$dataCheckVal203_2 = null;
		if (trim($this->input('method')) == 'create') {
			if ($val103 == 0) {
				$dataCheckVal203_1 = Cyn_BlockKukaku::where('ProjectID', '=', $val101)
													->where('OrderNo', '=', $val102)
													->where('T_Name', '=', $val201)
													->where('T_BKumiku', '=', $val202)
													->where('Name', '=', $val203)
													->where('BKumiku', '=', $val204);
				// update rev12
				/* if (trim($this->input('method')) == 'edit') {
					$dataCheckVal203_1 = $dataCheckVal203_1->where('No', '<>', $val104);
				} */
				$dataCheckVal203_1 = $dataCheckVal203_1->whereNull('Del_Date')->first();

				// update rev12
				/* if (trim($this->input('method')) == 'edit' && $dataCheckVal203_1 == null) {
					// update rev10
					$queryCynBlock = Cyn_BlockKukaku::where('ProjectID', '=', $val101)
													->where('OrderNo', '=', $val102)
													->where('No', '=', $val104)->first();
					if ($queryCynBlock != null && ($val203 != $queryCynBlock->Name || $val204 != $queryCynBlock->BKumiku)) {
						$dataCheckVal203_2 = Cyn_BlockKukaku::where('ProjectID', '=', $val101)
															->where('OrderNo', '=', $val102)
															->where('T_Name', '=', $val201)
															->where('T_BKumiku', '=', $val202)
															->where('N_Name', '=', $queryCynBlock->Name)
															->where('N_BKumiku', '=', $queryCynBlock->BKumiku)
															->where('No', '<>', $val104)
															->whereNull('Del_Date')->first();
					}
				} */
			}
			// Cyn_C_BlockKukaku
			if ($val103 == 1) {
				$dataCheckVal203_1 = Cyn_C_BlockKukaku::where('ProjectID', '=', $val101)
														->where('OrderNo', '=', $val102)
														->where('T_Name', '=', $val201)
														->where('T_BKumiku', '=', $val202)
														->where('Name', '=', $val203)
														->where('BKumiku', '=', $val204);
				// update rev12
				/* if (trim($this->input('method')) == 'edit') {
					$dataCheckVal203_1 = $dataCheckVal203_1->where('No', '<>', $val104);
				} */
				$dataCheckVal203_1 = $dataCheckVal203_1->whereNull('Del_Date')->first();

				// update rev12
				/* if (trim($this->input('method')) == 'edit' && $dataCheckVal203_1 == null) {
					// update rev10
					$queryCynCBlock = Cyn_C_BlockKukaku::where('ProjectID', '=', $val101)
														->where('OrderNo', '=', $val102)
														->where('No', '=', $val104)->first();
					if ($queryCynCBlock != null && ($val203 != $queryCynCBlock->Name || $val204 != $queryCynCBlock->BKumiku)) {
						$dataCheckVal203_2 = Cyn_C_BlockKukaku::where('ProjectID', '=', $val101)
																->where('OrderNo', '=', $val102)
																->where('T_Name', '=', $val201)
																->where('T_BKumiku', '=', $val202)
																->where('N_Name', '=', $queryCynCBlock->Name)
																->where('N_BKumiku', '=', $queryCynCBlock->BKumiku)
																->where('No', '<>', $val104)
																->whereNull('Del_Date')->first();
					}
				} */
			}
			if ($dataCheckVal203_1 != null) {
				$msg = (valueUrlDecode($this->val1) == config('system_const.c_kind_giso')) ?
						config('message.msg_cmn_db_022') : config('message.msg_cmn_db_017');
				$validator->after(function ($validator) use ($msg) {
					$validator->errors()->add('val203', $msg);
				});
			}
		}
		// update rev12
		/* if ($dataCheckVal203_2 != null) {
			$msg = (valueUrlDecode($this->val1) == config('system_const.c_kind_giso')) ?
					config('message.msg_cmn_db_032') : config('message.msg_cmn_db_031');
			$validator->after(function ($validator) use ($msg) {
				$validator->errors()->add('val203', $msg);
			});
		} */

		// val205
		if ($val206 != '' && $val205 == '') {
			$msg = (valueUrlDecode($this->val1) == config('system_const.c_kind_giso')) ?
					config('message.msg_cmn_db_023') : config('message.msg_cmn_db_018');
			$validator->after(function ($validator) use ($msg) {
				$validator->errors()->add('val205', $msg);
			});
		}
		if ($val206 != '' && $val205 != '') {
			if ($val203 == $val205 && $val204 == $val206) {
				$msg = (valueUrlDecode($this->val1) == config('system_const.c_kind_giso')) ?
						config('message.msg_cmn_db_024') : config('message.msg_cmn_db_019');
				$validator->after(function ($validator) use ($msg) {
					$validator->errors()->add('val205', $msg);
				});
			} else {
				// check infinity loop
				$arrListCheck = array();
				$dataCheckLoop = array();
				// Cyn_BlockKukaku
				if ($val103 == 0) {
					$dataCheckLoop = Cyn_BlockKukaku::select(
										'Cyn_BlockKukaku.No',
										'Cyn_BlockKukaku.Name',
										'Cyn_BlockKukaku.BKumiku',
										'Cyn_BlockKukaku.N_Name',
										'Cyn_BlockKukaku.N_BKumiku',
									)->join('Cyn_BlockKukaku as tableB', function($join) {
										$join->on('Cyn_BlockKukaku.N_Name', '=', 'tableB.Name')
											->on('Cyn_BlockKukaku.N_BKumiku', '=', 'tableB.BKumiku');
									})
									->where('Cyn_BlockKukaku.ProjectID', '=', $val101)
									->where('Cyn_BlockKukaku.OrderNo', '=', $val102)
									->whereNull('Cyn_BlockKukaku.Del_Date')->distinct()->get();
				}
				// Cyn_C_BlockKukaku
				if ($val103 == 1) {
					$dataCheckLoop = Cyn_C_BlockKukaku::select(
										'Cyn_C_BlockKukaku.No',
										'Cyn_C_BlockKukaku.Name',
										'Cyn_C_BlockKukaku.BKumiku',
										'Cyn_C_BlockKukaku.N_Name',
										'Cyn_C_BlockKukaku.N_BKumiku',
									)->join('Cyn_C_BlockKukaku as tableB', function($join) {
										$join->on('Cyn_C_BlockKukaku.N_Name', '=', 'tableB.Name')
											->on('Cyn_C_BlockKukaku.N_BKumiku', '=', 'tableB.BKumiku');
									})
									->where('Cyn_C_BlockKukaku.ProjectID', '=', $val101)
									->where('Cyn_C_BlockKukaku.OrderNo', '=', $val102)
									->whereNull('Cyn_C_BlockKukaku.Del_Date')->distinct()->get();
				}
				if (count($dataCheckLoop) > 0) {
					$arrInDB = $dataCheckLoop->toArray();
					// faker data if in DB
					$arrInDB[] = array(
						'No' => '',
						'Name' => $val203,
						'BKumiku' => $val204,
						'N_Name' => $val205,
						'N_BKumiku' => $val206,
					);
					if ($this->checkInfinityLoop($arrInDB, 0, $arrListCheck, -1, 0)) {
						$msg = (valueUrlDecode($this->val1) == config('system_const.c_kind_giso')) ?
								config('message.msg_validation_013') : config('message.msg_validation_009');
						$validator->after(function ($validator) use ($msg) {
							$validator->errors()->add('val205', $msg);
						});
					}
				}

				$dataCheckVal205 = null;
				if ($val103 == 0) {
					$dataCheckVal205 = Cyn_BlockKukaku::where('ProjectID', '=', $val101)
														->where('OrderNo', '=', $val102)
														->where('T_Name', '=', $val201)
														->where('T_BKumiku', '=', $val202)
														->where('Name', '=', $val205)
														->where('BKumiku', '=', $val206)
														->whereNull('Del_Date')->first();
				}
				// Cyn_C_BlockKukaku
				if ($val103 == 1) {
					$dataCheckVal205 = Cyn_C_BlockKukaku::where('ProjectID', '=', $val101)
														->where('OrderNo', '=', $val102)
														->where('T_Name', '=', $val201)
														->where('T_BKumiku', '=', $val202)
														->where('Name', '=', $val205)
														->where('BKumiku', '=', $val206)
														->whereNull('Del_Date')->first();
				}
				if ($dataCheckVal205 == null) {
					$msg = (valueUrlDecode($this->val1) == config('system_const.c_kind_giso')) ?
							config('message.msg_cmn_db_025') : config('message.msg_cmn_db_020');
					$validator->after(function ($validator) use ($msg) {
						$validator->errors()->add('val205', $msg);
					});
				}
			}
		}

		// val206 ~ A6
		if ($val205 != '' && $val206 == '') {
			$msg = (valueUrlDecode($this->val1) == config('system_const.c_kind_giso')) ?
					config('message.msg_cmn_db_026') : config('message.msg_cmn_db_021');
			$validator->after(function ($validator) use ($msg) {
				$validator->errors()->add('val206', $msg);
			});
		}
		if ($val204 != '' && $val206 != '' && $val204 > $val206) {
			$validator->after(function ($validator) {
				$validator->errors()->add('val206', config('message.msg_validation_008'));
			});
		}

		// val209
		if ($val209 != '') {
			preg_match('/^(\d{1,3}(?:[\.]\d{1})?)$/', $val209, $matches);
			if (count($matches) == 0) {
				$validator->after(function ($validator) {
					$validator->errors()->add('val209', config('message.msg_validation_007'));
				});
			}
		}

		// val210
		if ($val210 != '') {
			preg_match('/^(\d{1,3}(?:[\.]\d{1})?)$/', $val210, $matches);
			if (count($matches) == 0) {
				$validator->after(function ($validator) {
					$validator->errors()->add('val210', config('message.msg_validation_007'));
				});
			}
		}

		// val211
		if ($val211 != '') {
			preg_match('/^(\d{1,3}(?:[\.]\d{1})?)$/', $val211, $matches);
			if (count($matches) == 0) {
				$validator->after(function ($validator) {
					$validator->errors()->add('val211', config('message.msg_validation_007'));
				});
			}
		}

		// val212
		if ($val212 != '') {
			preg_match('/^(\d{1,7}(?:[\.]\d{1})?)$/', $val212, $matches);
			if (count($matches) == 0) {
				$validator->after(function ($validator) {
					$validator->errors()->add('val212', config('message.msg_validation_016'));
				});
			}
		}

		// val214
		if ($val214 != '') {
			preg_match('/^(\d{1,7}(?:[\.]\d{1})?)$/', $val214, $matches);
			if (count($matches) == 0) {
				$validator->after(function ($validator) {
					$validator->errors()->add('val214', config('message.msg_validation_016'));
				});
			}
		}

		return $validator;
	}

	/**
	 * check infinity loop data
	 *
	 * @return boolean true: has infinity data | false: hasn't infinity data
	 * @create 2020/11/03 Chien
	 *
	 * @update
	 */
	private function checkInfinityLoop($data, $indexAI = 0, &$lstCheck, $nextIndex = -1, $startLine = 0) {
		if (!isset($data[$indexAI])) {
			return false;
		}
		$temp = ($nextIndex != -1) ? $data[$nextIndex] : $data[$indexAI];
		// have next -> find next
		if ($temp['N_Name'] != '' && $temp['N_BKumiku'] != '') {
			foreach($data as $key => $row) {
				if ($nextIndex != -1) {
					if ($nextIndex == $key) {
						if ($nextIndex == (count($data) - 1)) {
							return false;
						}
						continue;
					}
				} else {
					if ($indexAI == $key) {
						continue;
					}
				}
				// find the next
				if ($row['Name'] == $temp['N_Name'] && $row['BKumiku'] == $temp['N_BKumiku']) {
					// found the next
					if (count($lstCheck) == 0) {
						$lstCheck[$startLine][] = array($temp['Name'], $temp['BKumiku']);
						return $this->checkInfinityLoop($data, $indexAI, $lstCheck, $key, $startLine);
					} else {
						if (isset($lstCheck[$startLine])) {
							// check with first item in line -> infinity loop
							if ($lstCheck[$startLine][0][0] == $temp['N_Name'] && $lstCheck[$startLine][0][1] == $temp['N_BKumiku']) {
								$lstCheck[$startLine][] = array($temp['Name'], $temp['BKumiku']);
								return true;
							} else {
								if (in_array(array($temp['Name'], $temp['BKumiku']), $lstCheck[$startLine])) {
									return true;
								}
								$lstCheck[$startLine][] = array($temp['Name'], $temp['BKumiku']);
								return $this->checkInfinityLoop($data, $indexAI, $lstCheck, $key, $startLine);
							}
						} else {
							return $this->checkInfinityLoop($data, ($indexAI + 1), $lstCheck, -1, ($startLine + 1));
						}
					}
				} else {
					if ($key == (count($data) - 1)) {
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
	 * @create 2020/11/03 Chien
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
	 * @create 2020/11/03 Chien
	 *
	 * @update
	 */
	protected function decodeValRequest() {
		if ($this->request->has('val202')) {
			$this->merge([
				'val202' => valueUrlDecode($this->request->get('val202'))
			]);
		}

		if ($this->request->has('val204')) {
			$this->merge([
				'val204' => valueUrlDecode($this->request->get('val204'))
			]);
		}

		if ($this->request->has('val205')) {
			$this->merge([
				'val205' => valueUrlDecode($this->request->get('val205'))
			]);
		}

		if ($this->request->has('val206')) {
			$this->merge([
				'val206' => valueUrlDecode($this->request->get('val206'))
			]);
		}

		if ($this->request->has('val215')) {
			$this->merge([
				'val215' => valueUrlDecode($this->request->get('val215'))
			]);
		}

		if ($this->request->has('val216')) {
			$this->merge([
				'val216' => valueUrlDecode($this->request->get('val216'))
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
				if (!in_array($key, array('val1','val2','val101','val102','val103','val104'))) {
					unset($query[$key]);
				}
			}
		}

		$arrUrl = explode('?', $url->previous());
		$queryString = '?'.http_build_query($query);

		return $arrUrl[0].$queryString;
	}
}