<?php
/*
 * @CaseDeleteRequest.php
 *
 * @create 2020/09/18 Chien
 *
 * @update
 */
namespace App\Http\Requests\Schet;

use Illuminate\Contracts\Validation\Validator;
use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Validation\ValidationException;
use App\Models\MstProject;
use App\Models\MstOrderNo;
/*
 * CaseDeleteRequest class
 *
 * @create 2020/09/18 Chien
 *
 * @update
 *
 */
class CaseDeleteRequest extends FormRequest
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
			'val1' => 'required|integer|exists:mstProject,ID',
			'val2' => 'nullable|regex:/^[\x21-\x7E]+$/|max:10',
		];;
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
			'val2' => 'オーダ',
		];
	}

	/**
	 * 独自処理を追加する
	 *
	 * @return
	 * @create 2020/10/28 Chien
	 *
	 * @update 2020/11/10 Chien
	 */
	public function withValidator($validator) {
		if(trim($this->val2) != '' || trim($this->val2) != null) {
			$checkData = MstOrderNo::where('OrderNo', '=', $this->val2)->first();
			if($checkData == null) {
				$validator->after(function ($validator) {
					$validator->errors()->add('val2', sprintf(config('message.msg_validation_010'), 'オーダ'));
				});
			}
		}
		return $validator;
	}

	/**
	 * バリデーションエラーメッセージ
	 *
	 * @return array
	 * @create 2020/11/09 Chien
	 *
	 * @update
	 */
	public function messages() {
		return [
			'val1.exists' => sprintf(config('message.msg_validation_010'), 'オーダ'),
		];
	}

	/**
	 * Get the validator instance for the request.
	 *
	 * @return \Illuminate\Contracts\Validation\Validator
	 * @create 2020/10/28 Chien
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
	 * @create 2020/10/28 Chien
	 *
	 * @update
	 */
	protected function prepareDataBeforeValidator() {
		if($this->request->has('val1')){
			$this->merge([
				'val1' => valueUrlDecode($this->request->get('val1'))
			]);
		} else {
			$this->merge(['val1' => '']);
		}

		if($this->request->has('val2')) {
			$this->merge([
				'val2' => valueUrlDecode($this->request->get('val2'))
			]);
		} else {
			$this->merge(['val2' => '']);
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