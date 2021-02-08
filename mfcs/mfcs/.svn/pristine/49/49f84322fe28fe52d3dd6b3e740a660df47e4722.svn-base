<?php
/*
 * @OrderContentsRequest.php
 * 
 * @create 2020/08/04 Thang
 *
 * @update 2020/08/24 Cuong Edit code check remove val1
 * @update 2020/10/27 Cuong Update check validate val21,val23,val24
 */
namespace App\Http\Requests\Mst;

use Illuminate\Contracts\Validation\Validator;
use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Validation\ValidationException;
use App\Librarys\FuncCommon;

/*
 * OrderContentsRequest class
 * 
 * @create 2020/08/04 Thang
 *
 * @update 2020/08/17 add redirect url function
 *  
 */
class OrderContentsRequest extends FormRequest
{
	/**
	 * Determine if the user is authorized to make this request.
	 *
	 * @return bool
	 * @create 2020/08/04 Thang
	 *
	 * @update
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
		$dates = array();
		$num = 7;
		$dates['Topマーキン'] = $this->input('val7');
		$dates['小組開始日'] = $this->input('val8');
		$dates['大組開始日'] = $this->input('val9');
		$dates['総組開始日'] = $this->input('val10');
		$dates['搭載開始日'] = $this->input('val11');
		$dates['船着工'] = $this->input('val12');
		$dates['PE開始'] = $this->input('val13');
		$dates['シフト日'] = $this->input('val14');
		$dates['進水'] = $this->input('val15');
		$dates['出渠日'] = $this->input('val16');
		$dates['離岸日'] = $this->input('val17');
		$dates['引渡'] = $this->input('val18');

		foreach($dates as $key=>$date){
			if(is_null($date)){
				$dateResult = true;
				$dateMsg = null;
			}else{
				list($dateResult, $dateMsg) = FuncCommon::checkFormatDate($date);
			}

			if(!$dateResult){
				$validator->after(function ($validator) use($dateMsg, $key, $num){
					$validator->errors()->add('val'.$num, sprintf($dateMsg, $key));
				});
			}
			$num++;
		}
	}

	/**
	 * Get the validation rules that apply to the request.
	 *
	 * @return array
	 * @create 2020/08/04 Thang
	 *
	 * @update 2020/10/27 Cuong Update check validate val21,val23,val24
	 * @update 2020/11/25 Cuong Update check validate val24: 半角英数記号が入力されている
	 */
	public function rules()
	{
		return [
			'val1' => 'required|regex:/^[\x21-\x7E]+$/|max:10',
			'val2' => 'nullable|alpha_num|max:3',
			'val3' => 'nullable|alpha_num|max:2',
			'val4' => 'nullable|alpha_num|max:8',
			'val5' => 'nullable|alpha_num|max:8',
			'val6' => 'nullable|string|max:8',
			'val7' => 'nullable',
			'val8' => 'nullable',
			'val9' => 'nullable',
			'val10' => 'nullable',
			'val11' => 'nullable',
			'val12' => 'nullable',
			'val13' => 'nullable',
			'val14' => 'nullable',
			'val15' => 'nullable',
			'val16' => 'nullable',
			'val17' => 'nullable',
			'val18' => 'nullable',
			'val19' => 'required|boolean',
			'val20' => 'required|boolean',
			'val21' => 'required|numeric',
			'val22' => 'required|boolean',
			'val23' => 'nullable|max:3|regex:/^[0-9a-zA-Z]*$/',
			'val24' => 'nullable|max:10|regex:/^[\x21-\x7E]+$/',
			'val25' => 'nullable|string|max:20',
		];
	}
	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 * @create 2020/08/04 Thang
	 *
	 * @update
	 */
	public function attributes()
	{
		return [
			'val1' => 'オーダ',
			'val2' => '建造区分',
			'val3' => '船級',
			'val4' => '船種',
			'val5' => '船型',
			'val6' => '略称',
			'val7' => 'Topマーキン',
			'val8' => '小組開始日',
			'val9' => '大組開始日',
			'val10' => '総組開始日',
			'val11' => '搭載開始日',
			'val12' => '船着工',
			'val13' => 'PE開始',
			'val14' => 'シフト日',
			'val15' => '進水',
			'val16' => '出渠日',
			'val17' => '離岸日',
			'val18' => '引渡',
			'val19' => '作業長支援に取り込み可能',
			'val20' => 'ダミーオーダフラグ',
			'val21' => '艦艇フラグ',
			'val22' => '非表示フラグ',
			'val23' => 'WBSコード',
			'val24' => '前船オーダ',
			'val25' => '備考'
		];
	}
	/**
	 * failedValidation
	 *
	 * @return mix RedirectResponse
	 * @create 2020/08/10 Thang
	 *
	 * @update
	 */
	protected function failedValidation(Validator $validator)
	{
		throw (new ValidationException($validator))
					->errorBag($this->errorBag)
					->redirectTo($this->getRedirectUrl());
	}
	/**
	 * get redirect url after failedValidation
	 *
	 * @return string
	 * @create 2020/08/10 Thang
	 *
	 * @update 2020/08/24 Cuong Edit code check remove val1
	 */
	protected function getRedirectUrl()
	{
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
		
		if(!(array_key_exists('val1', $query) && (array_key_exists('val26', $query) || !array_key_exists('val25', $query)))){
			//create case, so have to remove val1
			unset($query['val1']);
		}

		foreach($query as $key => $value) {
			if ((strpos($key, 'val') !== false && $key != 'val1') || $key == 'err1') {
				//remove val2 -> val26, err1
				unset($query[$key]);
			}
		}
		$arrUrl = explode('?', $url->previous());

		$queryString = '?'.http_build_query($query);
		return $arrUrl[0].$queryString;
	}
}
