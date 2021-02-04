<?php
/*
 * @BDataContentsRequest.php
 *
 * @create 2020/12/02 Cuong
 *
 * @update
 */
namespace App\Http\Requests\Schem;

use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Contracts\Validation\Validator;
use Illuminate\Validation\ValidationException;
/*
 * BDataContentsRequest class
 *
 * @create 2020/12/02 Cuong
 *
 * @update
 *
 */
class BDataContentsRequest extends FormRequest
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
		$rules = [
			'val1' => 'required|numeric|in:'.config("system_const.c_kind_chijyo").','.config("system_const.c_kind_gaigyo").','.config("system_const.c_kind_giso"),
			'val2' => 'required|numeric|in:'.config("system_const_schem.bd_val_import").','.config("system_const_schem.bd_val_export"),
			'val3' => 'required|numeric|exists:mstProject,ID',
			'val4' => 'required|regex:/^[\x21-\x7E]+$/|exists:mstOrderNo,OrderNo',
		];

		//Import
		if($this->val2 == config("system_const_schem.bd_val_import")) {
			$rules['val5'] = 'required|numeric|in:0,1';
			$rules['val6'] = 'required|file|mimes:xlsx,xls,xlsm|max: '.config('system_config.upload_file_size_max');
			$rules['val8'] = 'required|numeric';
		}
		
		return $rules;
	}

	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 * @create 2020/12/02 Cuong
	 *
	 * @update
	 */
	public function attributes() {
		return [
			'val1' => '中日程区分',
			'val2' => '機能選択',
			'val3' => 'ケース',
			'val4' => 'オーダ',
			'val5' => 'ログ出力',
			'val6' => 'ファイル選択',
			'val8' => '表示件数',
		];
	}
	//
	/**
	 * Get the validator instance for the request.
	 *
	 * @return \Illuminate\Contracts\Validation\Validator
	 * @create 2020/12/02 Cuong
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
	 * @create 2020/12/02 Cuong
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
		if($this->request->has('val8')){
			$this->merge([
				'val8' => valueUrlDecode($this->request->get('val8'))
			]);
		}
		
	}

	/**
	 * failedValidation
	 *
	 * @return mix RedirectResponse
	 * @create 2020/12/10 cuong
	 *
	 * @update
	 */
	protected function failedValidation(Validator $validator)
	{
		throw (new ValidationException($validator))
					->errorBag($this->errorBag)
					->redirectTo($this->getRedirectUrl());
	}
	/**
	 * get redirect url after failedValidation
	 *
	 * @return string
	 * @create 2020/12/10 cuong
	 *
	 * @update
	 */
	protected function getRedirectUrl()
	{
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
			if (strpos($key, 'val') !== false || $key == 'err1' || $key == 'ret1') {
				unset($query[$key]);
			}
		}
		$arrUrl = explode('?', $url->previous());

		$queryString = '?'.http_build_query($query);
		return $arrUrl[0].$queryString;
	}

}
