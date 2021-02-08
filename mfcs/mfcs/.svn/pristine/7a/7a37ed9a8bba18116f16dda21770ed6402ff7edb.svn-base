<?php
/*
 * @PatternContentsRequest.php
 *
 * @create 2020/09/04 Chien
 *
 * @update
 */
namespace App\Http\Requests\Schem;

use Illuminate\Contracts\Validation\Validator;
use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Validation\ValidationException;
/*
 * PatternContentsRequest class
 *
 * @create 2020/09/04 Chien
 *
 * @update
 *
 */
class PatternContentsRequest extends FormRequest
{
	/**
	 * Determine if the user is authorized to make this request.
	 *
	 * @return bool
	 * @create 2020/09/04 Chien
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
	 * @create 2020/09/04 Chien
	 *
	 * @update
	 */
	public function rules() {
		return [
			'val101' => 'required|regex:/^[0-9a-zA-Z]*$/|max:2',
			'val102' => 'required|string|max:20',
			'val103' => 'required|string|max:1',
			'val104' => 'required|boolean',
		];
	}

	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 * @create 2020/09/04 Chien
	 *
	 * @update
	 */
	public function attributes() {
		return [
			'val101' => 'コード',
			'val102' => '名称',
			'val103' => '中日程区分',
			'val104' => '有効',
		];
	}

	/**
	 * failedValidation
	 *
	 * @return mix RedirectResponse
	 * @create 2020/08/20 Cuong
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
	 * @create 2020/08/20 Cuong
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
		if(strpos($parseUrl['path'], '/create') !== false) {
			unset($query['val101']);
			unset($query['val103']);
		}

		// edit
		foreach($query as $key => $value) {
			if (strpos($key, 'val') !== false || $key == 'err1') {
				if(!($key == 'val1' || $key == 'val3' || $key == 'val101' || $key == 'val103')) {
					unset($query[$key]);
				}
			}
		}

		$arrUrl = explode('?', $url->previous());
		$queryString = '?'.http_build_query($query);

		return $arrUrl[0].$queryString;
	}

	/**
	* Get the validator instance for the request.
	*
	* @return \Illuminate\Contracts\Validation\Validator
	* @create 2020/12/31 Chien
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
	* @create 2020/12/31 Chien
	*
	* @update
	*/
	protected function decodeValRequest() {
		if($this->request->has('val103')) {
			$this->merge([
				'val103' => valueUrlDecode($this->request->get('val103'))
			]);
		}
	}
}