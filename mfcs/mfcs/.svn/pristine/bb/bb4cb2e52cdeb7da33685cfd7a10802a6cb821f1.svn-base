<?php
/*
 * @ImportContentsRequest.php
 * 
 * @create 2020/09/24 Cuong
 *
 * @update 2020/11/21 Cuong	update check validate
 */
namespace App\Http\Requests\Schet;

use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Contracts\Validation\Validator;
use Illuminate\Validation\ValidationException;
use App\Models\MstProject;
use App\Models\MstOrderNo;
class ImportContentsRequest extends FormRequest
{
	/**
	* Determine if the user is authorized to make this request.
	*
	* @return bool
	* @create 2020/09/24 Cuong
	*
	* @update 2020/11/21 Cuong	update check validate
	*/
	public function authorize()
	{
		return true;
	}

	/**
	 * Get the validation rules that apply to the request.
	 *
	 * @return array
	 * @create 2020/09/24 Cuong
	 *
	 * @update 2020/11/21 Cuong
	 */
	public function rules()
	{
		return [
			'val1' => 'required|integer|exists:mstProject,ID',
			'val2' => 'required|regex:/^[\x21-\x7E]+$/||max:10|exists:mstOrderNo,OrderNo',
			'val3' => 'required|file|mimes:xlsx,xls|max: '.config('system_config.upload_file_size_max'),
			'val5' => 'required|integer',
		];
	}

	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 * @create 2020/09/24 Cuong
	 *
	 * @update
	 */
	public function attributes(){
		return [
			'val1' => '検討ケース',
			'val2' => 'オーダ',
			'val3' => 'ファイル選択',
			'val5' => '表示件数',
		];
	}

	/**
	 * バリデーションエラーメッセージ
	 *
	 * @return array
	 * @create 2020/11/21 Cuong
	 *
	 * @update
	 */
	public function messages() {
		return [
			'val1.exists' => sprintf(config('message.msg_validation_010'), '検討ケース'),
			'val2.exists' => sprintf(config('message.msg_validation_010'), 'オーダ'),
		];
	}

	/**
	 * Get the validator instance for the request.
	 *
	 * @return \Illuminate\Contracts\Validation\Validator
	 * @create 2020/09/24 Cuong
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
	 * @create 2020/09/24 Cuong
	 *
	 * @update
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
	}

	/**
	 * failedValidation
	 *
	 * @return mix RedirectResponse
	 * @create 2020/11/19 cuong
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
	 * @create 2020/11/19 cuong
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
			if (strpos($key, 'val') !== false || $key == 'err1') {
				unset($query[$key]);
			}
		}
		$arrUrl = explode('?', $url->previous());

		$queryString = '?'.http_build_query($query);
		return $arrUrl[0].$queryString;
	}
}
