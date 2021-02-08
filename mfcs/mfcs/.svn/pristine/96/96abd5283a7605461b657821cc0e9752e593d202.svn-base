<?php
/*
 * @OrgIndexDateRequest.php
 * 職制管理画面日付バリデーションファイル
 *
 * @create 2020/07/21 KBS K.Yoshihara
 *
 * @update 2020/11/09 KBS S.Tanaka 日付が2桁表記の場合のバリデーションエラーを追加
 */

namespace App\Http\Requests\Mst;

use Illuminate\Foundation\Http\FormRequest;
use App\Librarys\FuncCommon;

/*
 * 職制管理画面日付バリデーション
 *
 * @create 2020/07/21 KBS K.Yoshihara
 * @update 2020/11/09 KBS S.Tanaka 日付が2桁表記の場合のバリデーションエラーを追加
 */
class OrgIndexDateRequest extends FormRequest
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
	 * 独自処理を追加する
	 *
	 * @return array
	 * @create 2020/11/09 KBS S.Tanaka
	 * 
	 * @update 2020/11/09 KBS S.Tanaka 日付が2桁表記の場合のバリデーションエラーを追加
	 */
	public function withValidator($validator)
	{
		$baseDate = $this->input('val2');
		if(is_null($baseDate)){
			$baseDateResult = true;
			$baseDateMsg = null;
		}else{
			list($baseDateResult, $baseDateMsg) = FuncCommon::checkFormatDate($baseDate);
		}

		if(!$baseDateResult){
			$validator->after(function ($validator) use($baseDateMsg){
				$validator->errors()->add('val2', sprintf($baseDateMsg, '日付'));
			});
		}
	}

	/**
	 * Get the validation rules that apply to the request.
	 *
	 * @return array
	 */
	public function rules()
	{
		return [
			'val2' => 'required',
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
			'val2' => '日付',
		];
	}
}
