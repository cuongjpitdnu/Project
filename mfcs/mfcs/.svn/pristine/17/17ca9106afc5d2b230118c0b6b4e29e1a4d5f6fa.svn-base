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
class CaseCopyRequest extends FormRequest
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
		$rules = [
			'val201' => 'required|numeric|in:'.$list,
			'val202' => 'nullable|integer',
			'val203' => 'required|regex:/^[\x21-\x7E]+$/|max:10|exists:mstOrderNo,OrderNo',
			'val204' => 'required|integer|exists:mstProject,ID',
			'val205' => 'required|regex:/^[\x21-\x7E]+$/|max:10|exists:mstOrderNo,OrderNo',
			'val206' => 'required|integer',
		];
		if($this->val206 != '' && mb_strlen(abs($this->val206)) > 5) {
			$rules['val206'] = 'required|string|max:5';
		}
		return $rules;
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
			'val201' => '中日程区分',
			'val202' => 'コピー元検討ケース',
			'val203' => 'コピー元オーダ',
			'val204' => 'コピー先検討ケース',
			'val205' => 'コピー先オーダ',
			'val206' => '手番シフト',
		];
	}

	/**
	 * 独自処理を追加する
	 *
	 * @return
	 * @create 2021/01/21 Chien
	 *
	 * @update
	 */
	public function withValidator($validator) {
		if(trim($this->val202) != '' && $this->val202 != config('system_const.projectid_production')) {
			$checkData = MstProject::where('ID', '=', $this->val202)->first();
			if($checkData == null) {
				$validator->after(function ($validator) {
					$validator->errors()->add('val202', sprintf(config('message.msg_validation_010'), '検討ケース'));
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
			'val203.exists' => sprintf(config('message.msg_validation_010'), 'コピー元オーダ'),
			'val204.exists' => sprintf(config('message.msg_validation_010'), 'コピー先検討ケース'),
			'val205.exists' => sprintf(config('message.msg_validation_010'), 'コピー先オーダ'),
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
		if($this->request->has('val201')) {
			$this->merge([
				'val201' => valueUrlDecode($this->request->get('val201'))
			]);
		}
		if($this->request->has('val202')) {
			$this->merge([
				'val202' => valueUrlDecode($this->request->get('val202'))
			]);
		}
		if($this->request->has('val203')) {
			$this->merge([
				'val203' => valueUrlDecode($this->request->get('val203'))
			]);
		}
		if($this->request->has('val204')) {
			$this->merge([
				'val204' => valueUrlDecode($this->request->get('val204'))
			]);
		}
		if($this->request->has('val205')) {
			$this->merge([
				'val205' => valueUrlDecode($this->request->get('val205'))
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