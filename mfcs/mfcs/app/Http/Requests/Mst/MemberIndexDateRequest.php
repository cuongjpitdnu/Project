<?php
/*
 * @MemberIndexDateRequest.php
 * 
 * @create 2020/09/01 Cuong
 *
 * @update 2020/11/09 KBS S.Tanaka 日付が2桁表記の場合のバリデーションエラーを追加
 */
namespace App\Http\Requests\Mst;

use Illuminate\Foundation\Http\FormRequest;
use App\Librarys\FuncCommon;

class MemberIndexDateRequest extends FormRequest
{
	/**
	* Determine if the user is authorized to make this request.
	*
	* @return bool
	* @create 2020/09/01 Cuong
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
	 * @create 2020/09/01 Cuong
	 *
	 * @update
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
	 * @create 2020/08/20 Cuong
	 *
	 * @update
	 */
	public function attributes(){
		return [
			'val1' => '基準日',
		];
	}

	/*
	* Validator instance updated on failedValidation
	*
	* @var \Illuminate\Contracts\Validation\Validator
	*/
	public $validator = null;	

	/**
	 * Overrid Handle a failed validation attempt.
	 * @param  \Illuminate\Contracts\Validation\Validator  $validator
	 * @return void
	 * @create 2020/09/01 Cuong
	 *
	 * @update
	 */
	protected function failedValidation(\Illuminate\Contracts\Validation\Validator $validator)
	{
		$this->validator = $validator;
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
		$baseDate = $this->input('val1');
		if(is_null($baseDate)){
			$baseDateResult = true;
			$baseDateMsg = null;
		}else{
			list($baseDateResult, $baseDateMsg) = FuncCommon::checkFormatDate($baseDate);
		}

		if(!$baseDateResult){
			$validator->after(function ($validator) use($baseDateMsg){
				$validator->errors()->add('val1', sprintf($baseDateMsg, '基準日'));
			});
		}
	}
}
	
