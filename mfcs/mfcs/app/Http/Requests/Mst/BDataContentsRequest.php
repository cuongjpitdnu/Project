<?php
/*
 * @OrderContentsRequest.php
 * 
 * @create 2020/08/06 Thang
 *
 * @update 2020/08/24 Cuong Edit code check remove val1
 */
namespace App\Http\Requests\Mst;

use Illuminate\Contracts\Validation\Validator;
use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Validation\ValidationException;
/*
 * BDataContentsRequest class
 * 
 * @create 2020/08/06 Thang
 *
 * @update 2020/11/25 Cuong update validate val1
 */
class BDataContentsRequest extends FormRequest
{
	/**
	 * Determine if the user is authorized to make this request.
	 *
	 * @return bool
	 * @create 2020/08/06 Thang
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
	 * @create 2020/08/06 Thang
	 *
	 * @update
	 */
	public function rules()
	{
		return [
			'val1' => 'required|string|regex:/^[0-9a-zA-Z]*$/|max:5',
			'val2' => 'nullable|string|max:50',
			'val3' => 'nullable|string|max:50',
			'val4' => 'required|boolean'
		];
	}
	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 * @create 2020/08/06 Thang
	 *
	 * @update
	 */
	public function attributes(){
		return [
			'val1' => 'コード',
			'val2' => '名称',
			'val3' => '略称',
			'val4' => '有効',
		];
	}
	/**
	 * failedValidation
	 *
	 * @return mix RedirectResponse
	 * @create 2020/08/10 Thang
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
	 * @create 2020/08/10 Thang
	 *
	 * @update 2020/08/24 Cuong Edit code check remove val1
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
		
		if(!(array_key_exists('val1', $query) && (array_key_exists('val5', $query) || !array_key_exists('val4', $query)))){
			//create case, so have to remove val1
			unset($query['val1']);
		}
		
		foreach($query as $key => $value) {
			if ((strpos($key, 'val') !== false && $key != 'val1') || $key == 'err1') {
				//remove val2 -> val5, err1
				unset($query[$key]);
			}
		}
		$arrUrl = explode('?', $url->previous());

		$queryString = '?'.http_build_query($query);
		return $arrUrl[0].$queryString;
	}
}
