<?php
/*
 * @OutputOutputRequest.php
 *
 * @create 2020/10/02 Chien
 *
 * @update
 */
namespace App\Http\Requests\Schem;

use Illuminate\Contracts\Validation\Validator;
use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Validation\ValidationException;
/*
 * OutputOutputRequest class
 *
 * @create 2020/10/02 Chien
 *
 * @update
 *
 */
class OutputOutputRequest extends FormRequest
{
	/**
	 * Determine if the user is authorized to make this request.
	 *
	 * @return bool
	 * @create 2020/10/02 Chien
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
	 * @create 2020/10/02 Chien
	 *
	 * @update
	 */
	public function rules() {
		return [
			'val1' => 'required|integer',
			'val2' => 'required|integer|exists:mstProject,ID',
			'val3' => 'required|string|max:10|regex:/^[\x21-\x7E]+$/|exists:mstOrderNo,OrderNo',
			'val4' => 'required|boolean',
		];
	}

	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 * @create 2020/10/02 Chien
	 *
	 * @update
	 */
	public function attributes() {
		return [
			'val1' => '中日程区分',
			'val2' => '検討ケース',
			'val3' => 'オーダ',
			'val4' => '正式発行',
		];
	}

	/**
	 * Get the validator instance for the request.
	 *
	 * @return \Illuminate\Contracts\Validation\Validator
	 * @create 2020/10/02 Chien
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
	 * @create 2020/10/02 Chien
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
	}

	/**
	 * failedValidation
	 *
	 * @return mix RedirectResponse
	 * @create 2020/11/26 Chien
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
	 * @create 2020/11/26 Chien
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