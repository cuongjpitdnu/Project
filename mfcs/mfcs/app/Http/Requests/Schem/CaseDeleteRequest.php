<?php
/*
 * @CaseCreateRequest.php
 *
 * @create 2020/12/16 Chien
 *
 * @update
 */
namespace App\Http\Requests\Schem;

use Illuminate\Contracts\Validation\Validator;
use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Validation\ValidationException;
use Illuminate\Validation\Rule;
use App\Models\MstProject;
use App\Models\MstOrderNo;

/*
 * CaseCreateRequest class
 *
 * @create 2020/12/16 Chien
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
	 * @create 2020/12/16 Chien
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
	 * @create 2020/12/16 Chien
	 *
	 * @update
	 */
	public function rules() {
		$list = config('system_const.c_kind_chijyo').','.
				config('system_const.c_kind_gaigyo').','.
				config('system_const.c_kind_giso');
		return [
			'val301' => 'required|numeric|in:'.$list,
			'val302' => 'required|integer|exists:mstProject,ID',
			'val303' => 'nullable|regex:/^[\x21-\x7E]+$/|max:10|exists:mstOrderNo,OrderNo',
		];
	}

	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 * @create 2020/12/16 Chien
	 *
	 * @update
	 */
	public function attributes() {
		return [
			'val301' => '中日程区分',
			'val302' => '検討ケース',
			'val303' => 'オーダ',
		];
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
			'val302.exists' => sprintf(config('message.msg_validation_010'), '検討ケース'),
			'val303.exists' => sprintf(config('message.msg_validation_010'), 'オーダ'),
		];
	}

	/**
	 * Get the validator instance for the request.
	 *
	 * @return \Illuminate\Contracts\Validation\Validator
	 * @create 2020/12/16 Chien
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
	 * @create 2020/12/16 Chien
	 *
	 * @update 2020/11/09 Chien
	 */
	protected function prepareDataBeforeValidator() {
		if($this->request->has('val301')) {
			$this->merge([
				'val301' => valueUrlDecode($this->request->get('val301'))
			]);
		}
		if($this->request->has('val302')) {
			$this->merge([
				'val302' => valueUrlDecode($this->request->get('val302'))
			]);
		}
		if($this->request->has('val303')) {
			$this->merge([
				'val303' => valueUrlDecode($this->request->get('val303'))
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