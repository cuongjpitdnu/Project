<?php
/*
 * @SummaryRequest.php
 *
 * @create 2020/02/01 Cuong
 *
 * @update
 */
namespace App\Http\Requests\Report;

use Illuminate\Contracts\Validation\Validator;
use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Validation\ValidationException;
use App\Librarys\FuncCommon;

use App\Models\R_SummaryTypeMst;
use App\Models\R_Summary;
use App\Models\R_SummaryCondition;
/*
 * SummaryRequest class
 *
 * @create 2020/02/01 Cuong
 *
 * @update
 *
 */
class SummaryRequest extends FormRequest
{
	/**
	 * Determine if the user is authorized to make this request.
	 *
	 * @return bool
	 */
	public function authorize()
	{
		return true;
	}

	/**
	 * Get the validation rules that apply to the request.
	 *
	 * @return array
	 */
	public function rules()
	{
		if($this->isMethod('post')) {
			$rules = [
				'val1' => 'required|numeric|exists:R_SummaryTypeMst,ID',
				'val2' => 'required|numeric|exists:R_Summary,ID',
				
			];
	
			if(isset($this->val3) && !is_null($this->val3)) {
				$rules['val3'] = 'numeric|exists:R_SummaryCondition,ID';
			}

			$code_subtotal = config('system_const_report.summary_code_subtotal');
			if(isset($this->val5) && !is_null($this->val5) &&  $this->val5 != 0) {
				$rules['val5'] = 'numeric|in:'.$code_subtotal;
			}
			$code_total = config('system_const_report.summary_code_total');
			if(isset($this->val6) && !is_null($this->val6) && $this->val6 != 0) {
				$rules['val6'] = 'numeric|in:'.$code_total;
			}


			return $rules;
		}
		return array();

	}
	/**
	 * 独自処理を追加する
	 *
	 * @return array
	 * @create 2020/02/01 Cuong
	 * 
	 * @update
	 */
	public function withValidator($validator)
	{
		if($this->isMethod('post')) {
			$dates = array();
			$dates['職制基準日（条件選択）'] = $this->input('val4');
			foreach($dates as $key=>$date){
				if(is_null($date)){
					$dateResult = true;
					$dateMsg = null;
				}else{
					list($dateResult, $dateMsg) = FuncCommon::checkFormatDate($date);
				}
	
				if(!$dateResult){
					$validator->after(function ($validator) use($dateMsg, $key){
						$validator->errors()->add('val4', sprintf($dateMsg, $key));
					});
				}
			}
		}
	}

	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 * @create 2020/02/01 Cuong
	 *
	 * @update
	 */
	public function attributes() {
		return [
			'val1' => '集計表タイプ',
			'val2' => '集計表名',
			'val3' => '条件項目選択',
			'val4' => '職制基準日（条件選択）',
			'val5' => '小計を出力',
			'val6' => '合計を出力',
		];
	}

	/**
	 * Get the validator instance for the request.
	 *
	 * @return \Illuminate\Contracts\Validation\Validator
	 * @create 2020/02/01 Cuong
	 *
	 * @update
	 */
	public function getValidatorInstance()
	{
		$this->prepareDataBeforeValidator();
		return parent::getValidatorInstance();
	}

	/**
	 * prepare value request.
	 *
	 * @return
	 * @create 2020/02/01 Cuong
	 *
	 * @update
	 */
	protected function prepareDataBeforeValidator() {
		if($this->request->has('val1')) {
			$this->merge([
				'val1' => valueUrlDecode($this->request->get('val1'))
			]);
		}
		if($this->request->has('val2')) {
			$this->merge([
				'val2' => valueUrlDecode($this->request->get('val2'))
			]);
		}
		if($this->request->has('val3')) {
			$this->merge([
				'val3' => valueUrlDecode($this->request->get('val3'))
			]);
		}
		if($this->request->has('val5')) {
			$this->merge([
				'val5' => valueUrlDecode($this->request->get('val5'))
			]);
		}
		if($this->request->has('val6')) {
			$this->merge([
				'val6' => valueUrlDecode($this->request->get('val6'))
			]);
		}
	
	}

	/**
	 * failedValidation
	 *
	 * @return mix RedirectResponse
	 * @create 2020/02/01 Cuong
	 *
	 * @update
	 */
	protected function failedValidation(Validator $validator)
	{
		throw (new ValidationException($validator))
					->errorBag($this->errorBag)
					->redirectTo($this->getRedirectUrl());
	}
}
