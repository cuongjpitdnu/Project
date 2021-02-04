<?php
/*
 * @OutputRequest.php
 *
 * @create 2020/10/23 Dung
 *
 * @update
 */
namespace App\Http\Requests\Schem;

use Illuminate\Contracts\Validation\Validator;
use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Validation\ValidationException;
/*
 * OutputRequest class
 *
 * @create 2020/10/23 Dung
 *
 * @update
 *
 */
	class OutputRequest extends FormRequest
{
    /**
	 * Determine if the user is authorized to make this request.
	 *
	 * @return bool
	 * @create 2020/10/23 Dung
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
	 * @create 2020/10/23 Dung
	 *
	 * @update
	 */
	public function rules() {
		$rules = [
			'val1' => 'required|numeric|between:0,1,2',
            'val2' => 'required|numeric|between:0,1',
			'val3' => 'required|numeric|exists:mstProject,ID',
			'val4' => 'required|string|max:10|regex:/^[\x21-\x7E]+$/|exists:mstOrderNo,OrderNo',
            'val8' => 'required|numeric',
		];
		//Import
		if($this->val2 == 0) {
			
            $rules['val6'] = 'required|numeric|between:0,1';
			$rules['val7'] = 'required|file|max: '.config('system_config.upload_file_size_max');
			
		}
		//Exxport
		if($this->val2 == 1) {
			
			$rules['val5'] = 'required|regex:/^[\x21-\x7E]+$/|exists:Cyn_mstKotei_STR_P,Code';
			
		}

        return $rules;
    }

    /**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 * @create 2020/10/23 Dung
	 *
	 * @update
	 */
	public function attributes() {
        return [
			'val1' => '中日程区分',
			'val2' => '機能選択',
			'val3' => '検討ケース',
            'val4' => 'オーダー',
            'val5' => '工程パターン',
            'val6' => '日程計算方式',
            'val7' => 'ファイル選択',
            'val8' => '表示件数',
		];
	}

	/**
	 * 独自処理を追加する
	 *
     * @return array
	 * @create 2020/11/16 Dung
	 * 
	 * @update
     */
	public function withValidator($validator)
	{
		
		// Excel形式のファイル
		if($this->hasFile('val7')) {
			$file = $this->val3;
			$validator->after(function ($validator) use($file) {
				if($this->checkExcelFile($file->getClientOriginalExtension()) == false) {
					//return validator with error by file input name
					$validator->errors()->add('val7', config('message.msg_validation_011'));
				}
			});
		}
		return $validator;
	}
	/**
	 * 独自処理を追加する
	 *
     * @return array
	 * @create 2020/11/16 Dung
	 * 
	 * @update
     */
	public function checkExcelFile($file_ext){
		$valid=array(
			'xls','xlsx' // add your extensions here.
		);        
		return in_array($file_ext,$valid) ? true : false;
	}
}