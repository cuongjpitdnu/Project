<?php
/*
 * @NitteiReflectRequest.php
 *
 * @create 2021/01/18 Anh
 *
 * @update
 */
namespace App\Http\Requests\Schem;

use Illuminate\Contracts\Validation\Validator;
use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Validation\ValidationException;
use App\Models\MstProject;
use App\Models\MstOrderNo;
use App\Models\Cyn_mstKotei;

/*
 * NitteiReflectRequest class
 *
 * @create 2021/01/18 Anh
 *
 * @update
 *
 */
class NitteiReflectRequest extends FormRequest
{
	/**
	 * Determine if the user is authorized to make this request.
	 *
	 * @return bool
	 *
	 * @create 2021/01/18 Anh
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
	 *
	 * @create 2021/01/18 Anh
	 *
	 * @update
	 */
	public function rules() {
		$listVal1 = config('system_const.c_kind_chijyo').','.
				config('system_const.c_kind_gaigyo').','.
				config('system_const.c_kind_giso');
		return [
			'val1' => 'required|integer|exists:mstProject,ID',
			'val2' => 'required|regex:/^[\x21-\x7E]+$/|exists:mstOrderNo,OrderNo',
			'val3' => 'required|numeric|in:'.$listVal1,
			'val4' => 'required|integer|exists:mstProject,ID',
		];
	}

	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 *
	 * @create 2021/01/18 Anh
	 * @update
	 */
	public function attributes() {
		return [
			'val1' => '小日程・検討ケース',
			'val2' => 'オーダ',
			'val3' => '中日程区分',
			'val4' => '中日程・検討ケース',
		];
	}

	/**
	 * Get the validator instance for the request.
	 *
	 * @return \Illuminate\Contracts\Validation\Validator
	 *
	 * @create 2021/01/18 Anh
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
	 * @create 2021/01/18 Anh
	 *
	 * @update
	 */
	protected function decodeValRequest() {
		if($this->request->has('val1')) {
			$this->merge([
				'val1' => valueUrlDecode($this->request->get('val1'))
			]);
		}

		if($this->request->has('val2')) {
			$this->merge([
				'val2' => valueUrlDecode($this->request->get('val2'))
			]);
		}

		if($this->request->has('val3')) {
			$this->merge([
				'val3' => valueUrlDecode($this->request->get('val3'))
			]);
		}

		if($this->request->has('val4')) {
			$this->merge([
				'val4' => valueUrlDecode($this->request->get('val4'))
			]);
		}
	}

	/**
	 * failedValidation
	 *
	 * @return mix RedirectResponse
	 *
	 * @create 2021/01/18 Anh
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
	 *
	 * @create 2021/01/18 Anh
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