<?php
/*
 * @ItemIndexContentsRequest.php
 *
 * @create 2020/10/23 Chien
 *
 * @update
 */
namespace App\Http\Requests\Schem;

use Illuminate\Contracts\Validation\Validator;
use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Validation\ValidationException;
/*
 * ItemIndexContentsRequest class
 *
 * @create 2020/10/23 Chien
 *
 * @update
 *
 */
class ItemIndexContentsRequest extends FormRequest
{
	/**
	 * Determine if the user is authorized to make this request.
	 *
	 * @return bool
	 * @create 2020/10/23 Chien
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
	 * @create 2020/10/23 Chien
	 *
	 * @update
	 */
	public function rules() {
		$list = config('system_const.c_kind_chijyo').','.
				config('system_const.c_kind_gaigyo').','.
				config('system_const.c_kind_giso');
		return [
			'val1' => 'required|numeric|in:'.$list,
			'val2' => 'required|numeric',
		];
	}

	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 * @create 2020/10/23 Chien
	 *
	 * @update
	 */
	public function attributes() {
		return [
			'val1' => '中日程区分',
			'val2' => '表示件数',
		];
	}

	/**
	 * Get the validator instance for the request.
	 *
	 * @return \Illuminate\Contracts\Validation\Validator
	 * @create 2020/10/23 Chien
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
	 * @create 2020/10/23 Chien
	 *
	 * @update
	 */
	protected function decodeValRequest() {
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

		if($this->request->has('val101')){
			$this->merge([
				'val101' => valueUrlDecode($this->request->get('val101'))
			]);
		}

		if($this->request->has('val102')){
			$this->merge([
				'val102' => valueUrlDecode($this->request->get('val102'))
			]);
		}
	}
}