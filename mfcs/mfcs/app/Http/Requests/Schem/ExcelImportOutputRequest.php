<?php
/*
 * @ExcelImportOutputRequest.php
 *
 * @create 2020/11/23 Dung
 *
 * @update
 */
namespace App\Http\Requests\Schem;

use Illuminate\Contracts\Validation\Validator;
use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Validation\ValidationException;
use App\Models\Cyn_mstKotei_STR_P;
/*
 * ExcelImportOutputRequest class
 *
 * @create 2020/11/23 Dung
 *
 * @update
 *
 */
	class ExcelImportOutputRequest extends FormRequest
{
	/**
	 * Determine if the user is authorized to make this request.
	 *
	 * @return bool
	 * @create 2020/10/23 Dung
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
	 * @create 2020/11/23 Dung
	 *
	 * @update
	 */
	public function rules() {
		$listVal1 = config('system_const.c_kind_chijyo').','.
				config('system_const.c_kind_gaigyo').','.
				config('system_const.c_kind_giso');
		$rules = [
			'val1' => 'required|integer|in:'.$listVal1,
			'val2' => 'required|numeric|in:'.config("system_const_schem.bd_val_import").','.config("system_const_schem.bd_val_export"),
			'val3' => 'required|numeric|exists:mstProject,ID',
			'val4' => 'required|string|regex:/^[\x21-\x7E]+$/|exists:mstOrderNo,OrderNo',
			'val8' => 'required|numeric',
		];
		//Import
		if($this->val2 != false && $this->val2 == config("system_const_schem.bd_val_import")) {
			$rules['val6'] = 'required|numeric|between:0,1';
			$rules['val7'] = 'required|file|max: '.config('system_config.upload_file_size_max');
		}
		// //Exxport
		if($this->val2 != false && $this->val2 == config("system_const_schem.bd_val_export")) { 
			$rules['val5'] = 'required|regex:/^[\x21-\x7E]+$/|exists:Cyn_mstKotei_STR_P,Code';
		}
		return $rules;
	}

	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 * @create 2020/11/23 Dung
	 *
	 * @update
	 */
	public function attributes() {
		return [
			'val1' => '中日程区分',
			'val2' => '機能選択',
			'val3' => '検討ケース',
			'val4' => 'オーダ',
			'val5' => '工程パターン',
			'val6' => '日程計算方式',
			'val7' => 'ファイル選択',
			'val8' => '表示件数',
		];
	}
	//
	/**
	 * Get the validator instance for the request.
	 *
	 * @return \Illuminate\Contracts\Validation\Validator
	 * @create 2020/11/23 Dung
	 *
	 * @update
	 */
	public function getValidatorInstance()
	{
		$this->decodeValRequest();
		return parent::getValidatorInstance();
	}

	/**
	 * decode request.
	 *
	 * @return array
	 * @create 2020/11/23 Dung
	 *
	 * @update
	 */
	protected function decodeValRequest() {
		if($this->request->has('val1')){
			$this->merge([
				'val1' => valueUrlDecode($this->request->get('val1'))
			]);
		}

		if($this->request->has('val2')){
			$this->merge([
				'val2' => valueUrlDecode($this->request->get('val2'))
			]);
		}

		if($this->request->has('val3')){
			$this->merge([
				'val3' => valueUrlDecode($this->request->get('val3'))
			]);
		}
		if($this->request->has('val4')){
			$this->merge([
				'val4' => valueUrlDecode($this->request->get('val4'))
			]);
		}
		if($this->request->has('val5')){
			$this->merge([
				'val5' => valueUrlDecode($this->request->get('val5'))
			]);
		}
		if($this->request->has('val6')){
			$this->merge([
				'val6' => valueUrlDecode($this->request->get('val6'))
			]);
		}
		if($this->request->has('val8')){
			$this->merge([
				'val8' => valueUrlDecode($this->request->get('val8'))
			]);
		}

	}
	//

	/**
	 * 独自処理を追加する
	 *
	 * @return array
	 * @create 2020/11/23 Dung
	 *
	 * @update
	 */
	public function withValidator($validator)
	{
		// Excel形式のファイル
		if($this->hasFile('val7')) {
			$file = $this->val7;
			$validator->after(function ($validator) use($file) {
				if($this->checkExcelFile($file->getClientOriginalExtension()) == false) {
					//return validator with error by file input name
					$validator->errors()->add('val7', config('message.msg_validation_011'));
				}
			});
		}
		return $validator;
	}
	/**
	 * 独自処理を追加する
	 *
	 * @return array
	 * @create 2020/12/02 Dung
	 *
	 * @update
	 */
	public function checkExcelFile($file_ext){
		$valid=array(
			'xls','xlsx','xlsm' // add your extensions here.
		);
		return in_array($file_ext,$valid) ? true : false;
	}

	/**
	 * failedValidation
	 *
	 * @return mix RedirectResponse
	 * @create 2020/11/26 Dung
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
	 * @create 2020/11/26 Dung
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
				unset($query[$key]);
			}
		}

		$arrUrl = explode('?', $url->previous());
		$queryString = '?'.http_build_query($query);

		return $arrUrl[0].$queryString;
	}
}