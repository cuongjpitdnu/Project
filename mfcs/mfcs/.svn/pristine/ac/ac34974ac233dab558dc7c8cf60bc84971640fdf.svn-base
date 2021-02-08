<?php
/*
 * @CaseCreateRequest.php
 *
 * @create 2020/09/18 Chien
 *
 * @update
 */
namespace App\Http\Requests\Schet;

use Illuminate\Contracts\Validation\Validator;
use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Validation\ValidationException;
/*
 * CaseCreateRequest class
 *
 * @create 2020/09/18 Chien
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
			'val1' => 'required|string|max:50',
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
			'val1' => '検討ケース',
		];
	}

	/**
	 * failedValidation
	 *
	 * @return mix RedirectResponse
	 * @create 2020/10/28 Chien
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