<?php
/*
 * @CaseCreateRequest.php
 *
 * @create 2020/11/20 Chien
 *
 * @update
 */
namespace App\Http\Requests\Schem;

use Illuminate\Contracts\Validation\Validator;
use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Validation\ValidationException;
/*
 * CaseCreateRequest class
 *
 * @create 2020/11/20 Chien
 *
 * @update
 *
 */
class CaseCreateRequest extends FormRequest
{
	/**
	 * Determine if the user is authorized to make this request.
	 *
	 * @return bool
	 * @create 2020/11/20 Chien
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
	 * @create 2020/11/20 Chien
	 *
	 * @update
	 */
	public function rules() {
		$list = config('system_const.c_kind_chijyo').','.
				config('system_const.c_kind_gaigyo').','.
				config('system_const.c_kind_giso');
		return [
			'val101' => 'required|numeric|in:'.$list,
			'val102' => 'required|string|max:50',
		];
	}

	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 * @create 2020/11/20 Chien
	 *
	 * @update
	 */
	public function attributes() {
		return [
			'val101' => '中日程区分',
			'val102' => '検討ケース',
		];
	}

	/**
	 * Get the validator instance for the request.
	 *
	 * @return \Illuminate\Contracts\Validation\Validator
	 * @create 2020/11/20 Chien
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
	 * @create 2020/11/20 Chien
	 *
	 * @update 2020/11/09 Chien
	 */
	protected function prepareDataBeforeValidator() {
		if($this->request->has('val101')) {
			$this->merge([
				'val101' => valueUrlDecode($this->request->get('val101'))
			]);
		}
	}

	/**
	 * failedValidation
	 *
	 * @return mix RedirectResponse
	 * @create 2020/10/28 Chien
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
	 * @create 2020/10/28 Chien
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