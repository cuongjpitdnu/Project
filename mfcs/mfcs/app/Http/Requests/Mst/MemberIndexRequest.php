<?php
/*
 * @MemberIndexRequest.php
 * 
 * @create 2020/09/01 Cuong
 *
 * @update 2020/09/14 Cuong
 * 
 * @update 2020/09/23 Cuong check validate val2
 * @update 2020/10/19 Cuong update check validate val4
 * @update 2020/11/09 KBS S.Tanaka 日付が2桁表記の場合のバリデーションエラーを追加
 */
namespace App\Http\Requests\Mst;

use Illuminate\Foundation\Http\FormRequest;
use App\Librarys\FuncCommon;
use App\Models\MstOrg;
use DB;

class MemberIndexRequest extends FormRequest
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
	 * @update 2020/10/19 Cuong Check val4 is integer
	 */
	public function rules()
	{
		return [
			'val1' => 'required',
			'val4' => 'nullable|integer|min:0|max:2147483647',
		];
	}

	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 * @create 2020/09/01 Cuong
	 *
	 * @update 2020/10/19 Cuong update name atrributes val4
	 */
	public function attributes(){
		return [
			'val1' => '基準日',
			'val2' => '所属',
			'val4' => '社員番号',
			'val5' => '氏名',
		];
	}

	/**
	 * 独自処理を追加する
	 *
	 * @return array
	 * @create 2020/09/14 Cuong
	 * 
	 * @update 2020/09/21 Cuong break loops when $flag = true
	 * @update 2020/09/23 Cuong check validate val2
	 * @update 2020/11/09 KBS S.Tanaka 日付が2桁表記の場合のバリデーションエラーを追加
	 */
	public function withValidator($validator)
	{
		$dateNow = DB::selectOne('SELECT CONVERT(DATE, getdate()) AS sysdate')->sysdate;
		$dateNow = str_replace('-', '/', $dateNow);
		$orgID = valueUrlDecode($this->val2);
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

		if(!is_null($orgID) && $baseDateResult){
			$val1 = $this->val1;
			$flag = false;
			$mstOrg = MstOrg::where('ID', '=', $orgID)
							->where('Sdate', '<=', $dateNow)
							->where(function($query) use($dateNow){
								$query->whereDate('Edate', '>=', $dateNow)
								->orWhereNull('Edate');
							})->first();
			if (is_null($mstOrg) || $mstOrg->FolderFlag == 1) {
				$flag = true;
			}
			
			$validator->after(function ($validator) use ($flag) {
				if ($flag) {
					$validator->errors()->add('val2', config('message.msg_member_if_002'));
				}
			});
		}

		// validate val5
		if(!is_null($this->val5) && $this->val5 != '' && !(bool) preg_match('/^[0-9a-zA-Z０-９ａ-ｚＡ-Ｚぁ-んァ-ンヴｧ-ﾝﾞﾟ　 一-龠]+$/',$this->val5)) {
			$validator->after(function ($validator) {
				$validator->errors()->add('val5', config('message.msg_validation_017'));
			});
		}
		return $validator;
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
	 * @create 2020/11/25 Cuong
	 *
	 * @update
	 */
	protected function failedValidation(\Illuminate\Contracts\Validation\Validator $validator)
	{
		$this->validator = $validator;
	}

}
