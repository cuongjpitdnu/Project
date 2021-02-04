<?php
/*
 * @DistContentsRequest.php
 *
 * @create 2020/10/09 Chien
 *
 * @update
 */
namespace App\Http\Requests\Sches;

use Illuminate\Contracts\Validation\Validator;
use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Validation\ValidationException;
use Illuminate\Validation\Rule;
/*
 * DistContentsRequest class
 *
 * @create 2020/10/09 Chien
 *
 * @update
 *
 */
class DistContentsRequest extends FormRequest
{
	/**
	 * Determine if the user is authorized to make this request.
	 *
	 * @return bool
	 * @create 2020/10/09 Chien
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
	 * @create 2020/10/09 Chien
	 *
	 * @update
	 */
	public function rules() {
		$val1 = $this->request->get('val1');
		$check = $this->request->get('check');

		// initial
		$rules = [
			'val1' => [
				'required',
				'regex:/^[0-9a-zA-Z]*$/',
				'max:5',
			],
			'val2' => [
				'required',
				'string',
				'max:50',
			],
			'val3' => 'required|string|max:50',
		];

		// create
		if($check == 1) {
			$rules['val2'][] = Rule::unique('mstDist', 'Name');
		}
		// edit
		if($check == 2) {
			$rules['val2'][] = Rule::unique('mstDist', 'Name')->ignore($this->val1, 'Code');
		}

		return $rules;
	}

	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 * @create 2020/10/09 Chien
	 *
	 * @update
	 */
	public function attributes() {
		return [
			'val1' => 'コード',
			'val2' => '名称',
			'val3' => '略称',
		];
	}

	/**
	 * failedValidation
	 *
	 * @return mix RedirectResponse
	 * @create 2020/10/09 Cuong
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
	 * @create 2020/10/09 Chien
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
			unset($query['val1']);
		}

		// edit
		foreach($query as $key => $value) {
			if (strpos($key, 'val') !== false || $key == 'err1') {
				if(!($key == 'val1')) {
					unset($query[$key]);
				}
			}
		}

		$arrUrl = explode('?', $url->previous());
		$queryString = '?'.http_build_query($query);

		return $arrUrl[0].$queryString;
	}
}