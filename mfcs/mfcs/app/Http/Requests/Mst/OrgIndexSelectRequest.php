<?php
/*
 * @OrgIndexSelectRequest.php
 * 職制管理画面職制選択バリデーションファイル
 *
 * @create 2020/07/21 KBS K.Yoshihara
 *
 * @update
 */

namespace App\Http\Requests\Mst;

use Illuminate\Foundation\Http\FormRequest;

/*
 * 職制管理画面職制選択バリデーション
 *
 * @create 2020/07/21 KBS K.Yoshihara
 * @update
 */
class OrgIndexSelectRequest extends FormRequest
{
	/**
	 * Determine if the user is authorized to make this request.
	 *
	 * @return bool
	 */
	public function authorize()
	{
		return true;
	}

	/**
	 * Get the validation rules that apply to the request.
	 *
	 * @return array
	 */
	public function rules()
	{
		return [
			'val1' => 'required',
		];
	}

	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 */
	public function attributes()
	{
		// \resources\lang\ja\validation.php にも書けるが、このチェックでしか使わないようなものはここに書く。
		return [
			'val1' => '職制',
		];
	}

}
