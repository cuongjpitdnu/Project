<?php
/*
 * @PatternContentsRequest.php
 *
 * @create 2021/01/29 Anh
 *
 * @update
 */
namespace App\Http\Requests\Sches;

use Illuminate\Contracts\Validation\Validator;
use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Validation\ValidationException;
use Illuminate\Validation\Rule;
use App\Models\Cyn_mstKotei;

/*
 * PatternContentsRequest class
 *
 * @create 2021/01/29 Anh
 *
 * @update
 *
 */
class PatternContentsRequest extends FormRequest
{
	/**
	 * Determine if the user is authorized to make this request.
	 *
	 * @return bool
	 *
	 * @create 2021/01/29 Anh
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
	 * @create 2021/01/29 Anh
	 *
	 * @update
	 */
	public function rules() {
		return [
			'val3' => 'required|string|max:40',
			'val4' => 'required|integer|max:9',
			'val5' => 'required|integer|max:9',
			// 'val6' => 'required|string|max:2|regex:/^[a-zA-Z0-9]*$/',
			'val7' => 'required|integer|max:9',
			'val8' => 'required|string|max:2|regex:/^[a-zA-Z0-9]*$/|exists:mstFloor,Code',
			'val9' => 'required|integer',
		];
	}

	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 *
	 * @create 2021/01/29 Anh
	 * @update
	 */
	public function attributes() {
		return [
			'val3' => '名称',
			'val4' => '組区',
			'val5' => '対象区分',
			'val6' => '工程',
			'val7' => '工程組区',
			'val8' => '施工棟',
			'val9' => '基準データ',
		];
	}

	/**
	 * 独自処理を追加する
	 *
	 * @return array
	 *
	 * @create 2021/01/29 Anh
	 * @update
	 */
	public function withValidator($validator)
	{
		$cKind = $this->input('val5');
		$code = $this->input('val6');

		$val6 = array();

		$val6 = [
			'required',
			Rule::exists('Cyn_mstKotei', 'Code')->where(function ($query) use($cKind, $code) {
				$query->where('Cyn_mstKotei.CKind', '=', $cKind)->where('Cyn_mstKotei.Code', '=', $code);
			}),
			'string',
			'max:2',
			'regex:/^[a-zA-Z0-9]*$/'
		];
		$validator->addRules(['val6' => $val6]);
	}


	/**
	 * Get the validator instance for the request.
	 *
	 * @return \Illuminate\Contracts\Validation\Validator
	 *
	 * @create 2021/01/29 Anh
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
	 * @create 2021/01/29 Anh
	 *
	 * @update
	 */
	protected function decodeValRequest() {
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

		if($this->request->has('val6')) {
			$this->merge([
				'val6' => valueUrlDecode($this->request->get('val6'))
			]);
		}

		if($this->request->has('val7')) {
			$this->merge([
				'val7' => valueUrlDecode($this->request->get('val7'))
			]);
		}

		if($this->request->has('val8')) {
			$this->merge([
				'val8' => valueUrlDecode($this->request->get('val8'))
			]);
		}

		if($this->request->has('val9')) {
			$this->merge([
				'val9' => valueUrlDecode($this->request->get('val9'))
			]);
		}
	}

	/**
	 * failedValidation
	 *
	 * @return mix RedirectResponse
	 *
	 * @create 2021/01/29 Anh
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
	 * @create 2021/01/29 Anh
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
			if ((strpos($key, 'val') !== false || $key == 'err1') && !in_array($key, ['val1', 'val2'])) {
				unset($query[$key]);
			}
		}

		$arrUrl = explode('?', $url->previous());
		$queryString = '?'.http_build_query($query);

		return $arrUrl[0].$queryString;
	}
}