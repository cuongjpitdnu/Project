<?php
/*
 * @RequestItemCreate.php
 *
 * @create 2020/11/09 Chien
 *
 * @update
 */
namespace App\Http\Requests\Schem;

use Illuminate\Contracts\Validation\Validator;
use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Validation\ValidationException;
use App\Models\Cyn_BlockKukaku;
use App\Models\Cyn_C_BlockKukaku;

/*
 * RequestItemCreate class
 *
 * @create 2020/11/09 Chien
 *
 * @update
 *
 */
class RequestItemCreate extends FormRequest
{
	/**
	 * Determine if the user is authorized to make this request.
	 *
	 * @return bool
	 * @create 2020/11/09 Chien
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
	 * @create 2020/11/09 Chien
	 *
	 * @update
	 */
	public function rules() {
		return [
			'val101' => 'required',
			'val102' => 'required',
		];
	}

	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 * @create 2020/11/09 Chien
	 *
	 * @update
	 */
	public function attributes() {
		return [
			'val101' => 'ケース',
			'val102' => 'オーダ',
		];
	}
}