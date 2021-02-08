<?php
/*
 * @FloorContentsRequest.php
 * 
 * @create 2020/08/27 Cuong
 *
 * @update
 */
namespace App\Http\Requests\Mst;

use Illuminate\Foundation\Http\FormRequest;

class OrgContentsRequest extends FormRequest
{
	/**
	* Determine if the user is authorized to make this request.
	*
	* @return bool
	* @create 2020/08/27 Cuong
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
	 * @create 2020/08/27 Cuong
	 *
	 * @update
	 */
	public function rules()
	{
		return [
			'val101' => 'required|numeric|between:-2147483648,2147483647',
			'val102' => 'nullable|string|max:6|regex:/^[0-9a-zA-Z]*$/',
			'val103' => 'required|string|max:50',
			'val104' => 'required|string|max:50',
			'val105' => 'required|numeric|between:-2147483648,2147483647',
			'val106' => 'required|numeric|between:-2147483648,2147483647',
			'val107' => 'required|numeric|between:-2147483648,2147483647',
			'val108' => 'required|numeric|between:-2147483648,2147483647',
			'val109' => 'nullable|string|max:20|regex:/^[0-9a-zA-Z]*$/',
			'val110' => 'required|boolean',
			'val111' => 'nullable|numeric|min:0|max:2147483647'
		];
	}

	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 * @create 2020/08/27 Cuong
	 *
	 * @update
	 */
	public function attributes(){
		return [
			'val101' => '親職制',
			'val102' => '職制コード',
			'val103' => '職制名',
			'val104' => '略称',
			'val105' => '社内外',
			'val106' => '部内外',
			'val107' => '請負会社',
			'val108' => '外注班タイプ',
			'val109' => '仕入先コード',
			'val110' => 'フォルダフラグ',
			'val111' => '表示順'
		];
	}

	/**
	 * Get the validator instance for the request.
	 *
	 * @return \Illuminate\Contracts\Validation\Validator
	 * @create 2020/08/27 Cuong
	 *
	 * @update
	 */
	public function getValidatorInstance()
	{
		$this->decodeValRequest();
		return parent::getValidatorInstance();
	}

	/**
	 * decode request val101.
	 *
	 * @return array
	 * @create 2020/08/27 Cuong
	 *
	 * @update
	 */
	protected function decodeValRequest()
	{
		if($this->request->has('val101')){
			$this->merge([
				'val101' => valueUrlDecode($this->request->get('val101'))
			]);
		}
	}
}
