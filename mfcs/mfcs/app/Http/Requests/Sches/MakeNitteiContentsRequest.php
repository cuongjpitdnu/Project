<?php
/*
 * @MakeNitteiContentsRequest.php
 *
 * @create 2020/12/30 Cuong
 *
 * @update
 */
namespace App\Http\Requests\Sches;

use Illuminate\Foundation\Http\FormRequest;
/*
 * MakeNitteiContentsRequest class
 *
 * @create 2020/12/30 Cuong
 *
 * @update
 *
 */
class MakeNitteiContentsRequest extends FormRequest
{
	/**
	 * Determine if the user is authorized to make this request.
	 *
	 * @return bool
	 * @create 22020/12/30 Cuong
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
	 * @create 2020/12/30 Cuong
	 *
	 * @update
	 */
	public function rules()
	{
		$rules = [
			'val1' => 'required|numeric|in:'.config("system_const_schem.bd_val_import").','.config("system_const_schem.bd_val_export"),
			'val2' => 'required|numeric|exists:mstProject,ID',
			'val3' => 'required|regex:/^[\x21-\x7E]+$/|exists:mstOrderNo,OrderNo',
			'val4' => 'required|numeric|in:0,1',
		];

		//Import
		if($this->val1 == config("system_const_schem.bd_val_import")) {
			$rules['val5'] = 'required|file|mimes:xlsx,xls,xlsm|max: '.config('system_config.upload_file_size_max');
			$rules['val6'] = 'required|numeric';
		}
		
		return $rules;
	}

	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 * @create 2020/12/30 Cuong
	 *
	 * @update
	 */
	public function attributes() {
		return [
			'val1' => '機能選択',
			'val2' => '検討ケース',
			'val3' => 'オーダ',
			'val4' => 'データ区分',
			'val5' => 'ファイル選択',
			'val6' => '表示件数',
		];
	}

		/**
	 * Get the validator instance for the request.
	 *
	 * @return \Illuminate\Contracts\Validation\Validator
	 * @create 2020/12/02 Cuong
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
	 * @create 2020/12/02 Cuong
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

		if($this->request->has('val3')){
			$this->merge([
				'val3' => valueUrlDecode($this->request->get('val3'))
			]);
		}
		
		if($this->request->has('val4')){
			$this->merge([
				'val4' => valueUrlDecode($this->request->get('val4'))
			]);
		}

		if($this->request->has('val6')){
			$this->merge([
				'val6' => valueUrlDecode($this->request->get('val6'))
			]);
		}
		
	}
}
