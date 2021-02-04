<?php
/*
 * @ReCalcRequest.php
 *
 * @create 2020/11/24 Chien
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
 * ReCalcRequest class
 *
 * @create 2020/11/24 Chien
 *
 * @update
 *
 */
class ReCalcRequest extends FormRequest
{
	/**
	 * Determine if the user is authorized to make this request.
	 *
	 * @return bool
	 * @create 2020/11/24 Chien
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
	 * @create 2020/11/24 Chien
	 *
	 * @update
	 */
	public function rules() {
		$listVal1 = config('system_const.c_kind_chijyo').','.
				config('system_const.c_kind_gaigyo').','.
				config('system_const.c_kind_giso');
		$listVal2 = config('system_const.kumiku_code_kogumi').','.
				config('system_const.kumiku_code_naicyu').','.
				config('system_const.kumiku_code_kumicyu').','.
				config('system_const.kumiku_code_ogumi').','.
				config('system_const.kumiku_code_sogumi').','.
				config('system_const.kumiku_code_kyocyu');
		return [
			'val1' => 'required|numeric|in:'.$listVal1,
			'val2' => 'required|integer|exists:mstProject,ID',
			'val3' => 'required|string|regex:/^[\x21-\x7E]+$/|max:10|exists:mstOrderNo,OrderNo',
			'val4' => 'required|regex:/^[\x21-\x7E]+$/|exists:Cyn_mstKotei,Code',
			'val5' => 'required|in:'.$listVal2,
		];
	}

	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 * @create 2020/11/24 Chien
	 *
	 * @update
	 */
	public function attributes() {
		return [
			'val1' => '中日程区分',
			'val2' => '検討ケース',
			'val3' => 'オーダ',
			'val4' => '工程',
			'val5' => '工程組区',
		];
	}

	/**
	 * Get the validator instance for the request.
	 *
	 * @return \Illuminate\Contracts\Validation\Validator
	 * @create 2020/11/24 Chien
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
	 * @create 2020/11/24 Chien
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

		if($this->request->has('val5')) {
			$this->merge([
				'val5' => valueUrlDecode($this->request->get('val5'))
			]);
		}
	}

	/**
	 * failedValidation
	 *
	 * @return mix RedirectResponse
	 * @create 2020/11/24 Chien
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
	 * @create 2020/11/24 Chien
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