<?php
/*
 * @FloorContentsRequest.php
 * 
 * @create 2020/08/20 Cuong
 *
 * @update
 */
namespace App\Http\Requests\Mst;

use Illuminate\Contracts\Validation\Validator;
use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Validation\ValidationException;

class FloorContentsRequest extends FormRequest
{	
	/**
	* Determine if the user is authorized to make this request.
	*
	* @return bool
	* @create 2020/08/20 Cuong
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
	 * @create 2020/08/20 Cuong
	 *
	 * @update 2020/11/25 Cuong update check validate val5, val6, val8
	 */
	public function rules()
	{
		return [
			'val1' => 'required|regex:/^[0-9a-zA-Z]*$/|max:10',
			'val2' => 'nullable|string|max:50',
			'val3' => 'nullable|string|max:50',
			'val4' => 'nullable|string|max:10',
			'val5' => 'required|integer|min:0|max:2147483647',
			'val6' => 'required|integer|min:0|max:2147483647',
			'val7' => 'nullable|regex:/^[0-9a-zA-Z]*$/|max:10',
			'val8' => 'required|integer|between:-2147483648,2147483647',
			'val9' => 'required|boolean'
		];
	}

	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 * @create 2020/08/20 Cuong
	 *
	 * @update
	 */
	public function attributes(){
		return [
			'val1' => 'コード',
			'val2' => '名称',
			'val3' => '略称',
			'val4' => '略称1',
			'val5' => '物量消化能力',
			'val6' => '保有HA',
			'val7' => '課係コード',
			'val8' => '表示順',
			'val9' => '表示フラグ',
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
	 * @create 2020/08/20 Cuong
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
		
		if(!(array_key_exists('val1', $query) && (array_key_exists('val10', $query) || !array_key_exists('val9', $query)))){
			//create case, so have to remove val1
			unset($query['val1']);
		}

		foreach($query as $key => $value) {
			if ((strpos($key, 'val') !== false && $key != 'val1') || $key == 'err1') {
				//remove val2 -> val26, err1
				unset($query[$key]);
			}
		}

		$arrUrl = explode('?', $url->previous());
		$queryString = '?'.http_build_query($query);
		return $arrUrl[0].$queryString;
	}
}