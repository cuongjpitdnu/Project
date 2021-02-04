<?php
/*
 * @MemberEditRequest.php
 * 
 * @create 2020/09/21 Cuong
 *
 * @update 2020/10/19 Cuong update check format datetime
 * @update 2020/11/09 KBS S.Tanaka 日付が2桁表記の場合のバリデーションエラーを追加
 * @update 2020/12/18 Cuong use library: ValidationException.
 */
namespace App\Http\Requests\Mst;

use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Contracts\Validation\Validator;
use Illuminate\Validation\ValidationException;
use App\Librarys\FuncCommon;

class MemberEditRequest extends FormRequest
{
	/**
	* Determine if the user is authorized to make this request.
	*
	* @return bool
	* @create 2020/09/21 Cuong
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
	* @create 2020/09/21 Cuong
	*
	* @update
	*/
	public function rules()
	{
		return [
			'val401' => 'required|string|max:50',
			'val402' => 'required|string|max:50',
			'val403' => 'required|string|max:50',
			'val404' => 'nullable',
		];
	}

	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 * @create 2020/09/21 Cuong
	 *
	 * @update
	 */
	public function attributes(){
		return [
			'val401' => '名前',
			'val402' => 'フリガナ',
			'val403' => '略称',
			'val404' => '定年退職日',
		];
	}

	/**
	 * 独自処理を追加する
	 *
	 * @return array
	 * @create 2020/11/09 KBS S.Tanaka
	 * 
	 * @update 
	 */
	public function withValidator($validator)
	{	
		$retireDate = $this->input('val404');
		
		if(is_null($retireDate)){
			$retireDateResult = true;
			$retireDateMsg = null;
		}else{
			list($retireDateResult, $retireDateMsg) = FuncCommon::checkFormatDate($retireDate);
		}

		if(!$retireDateResult){
			$validator->after(function ($validator) use($retireDateMsg){
				$validator->errors()->add('val404', sprintf($retireDateMsg, '定年退職日'));
			});
		}
	}

	/**
	 * failedValidation
	 *
	 * @return mix RedirectResponse
	 * @create 2020/11/27 Cuong
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
	 * @create 2020/11/27 Cuong
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

		foreach($query as $key => $value) {
			if (strpos($key, 'val') !== false || $key == 'err1') {
				if(!in_array($key, ['val1','val2','val3','val4','val5','val101','val102'])){
					unset($query[$key]);
				}
			}
		}

		$arrUrl = explode('?', $url->previous());
		$queryString = '?'.http_build_query($query);

		return $arrUrl[0].$queryString;
	}
}
