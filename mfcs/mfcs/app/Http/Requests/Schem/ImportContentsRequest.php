<?php
/*
 * @ImportContentsRequest.php
 *
 * @create 2020/09/14 Dung
 *
 */
namespace App\Http\Requests\Schem;

use Illuminate\Contracts\Validation\Validator;
use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Validation\ValidationException;
use App\Models\MstProject;
use App\Models\MstOrderNo;
use Illuminate\Validation\Rule;

/*
 * ImportContentsRequest class
 *
 * @create 2020/09/14 Dung
 *
 * @update
 */
class ImportContentsRequest extends FormRequest
{
	/**
	 * Determine if the user is authorized to make this request.
	 *
	 * @return bool
	 * @create 2020/09/14 Dung
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
	 * @create 2020/09/14 Dung
	 *
	 * @update
	 */
	public function rules()
	{
		return [
			'val1' => 'required|regex:/^[\x21-\x7E]+$/|exists:mstOrderNo,OrderNo',
			'val2' => 'required|numeric',
			'val3' => 'required|numeric|exists:mstProject,ID',
			'val4' => 'required|numeric',
		];
	}
	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 * @create 2020/09/14 Dung
	 *
	 * @update
	 */
	public function attributes(){
		return [
			'val1' => 'オーダ',
			'val2' => '中日程区分',
			'val3' => '検討ケース',
			'val4' => '表示件数'
		];
	}

	/**
	 * failedValidation
	 *
	 * @return mix RedirectResponse
	 * @create 2020/09/14 Dung
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
	 * @create 2020/09/21 Dung
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

		// create
		if(strpos($parseUrl['path'], '/import') !== false) {
			unset($query['val1']);
			unset($query['val4']);
		}

		// edit
		foreach($query as $key => $value) {
			if (strpos($key, 'val') !== false || $key == 'err1') {
				if(!($key == 'val1' || $key == 'val2' || $key == 'val3' || $key == 'val4')) {
					unset($query[$key]);
				}
			}
		}

		$arrUrl = explode('?', $url->previous());
		$queryString = '?'.http_build_query($query);

		return $arrUrl[0].$queryString;
	}

	/**
	 * 独自処理を追加する
	 *
	 * @return
	 * @create 2020/09/14 Dung
	 *
	 * @update
	 */
	public function withValidator($validator) {
		if($this->val3 == -1) {
			$rules = ['val3' => 'required'];
			$validator->addRules($rules);
		}
	}

	/**
	 * Get the validator instance for the request.
	 *
	 * @return \Illuminate\Contracts\Validation\Validator
	 * @create 2020/08/27 Cuong
	 *
	 * @update
	 */
	public function getValidatorInstance()
	{
		$this->decodeValRequest();
		return parent::getValidatorInstance();
	}

	/**
	 * decode request val101.
	 *
	 * @return array
	 * @create 2020/09/21 Dung
	 *
	 * @update 2021/15/1 Dung
	 */
	protected function decodeValRequest()
	{
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
	}

}
