<?php
/*
 * @AbilityContentsRequest.php
 * 能力時間共通化画面バリデーションファイル
 *
 * @create 2020/08/27 KBS S.Tanaka
 *
 * @update 2020/11/06 KBS S.Tanaka 日付が2桁表記の場合のバリデーションエラーを追加
 */

namespace App\Http\Requests\Mst;

use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Contracts\Validation\Validator;
use Illuminate\Validation\ValidationException;
use Illuminate\Validation\Rule;
use App\Librarys\FuncCommon;

/*
 * 能力時間共通化画面バリデーション
 *
 * @create 2020/08/27 KBS S.Tanaka
 * @update 2020/11/06 KBS S.Tanaka 日付が2桁表記の場合のバリデーションエラーを追加
 */
class AbilityContentsRequest extends FormRequest
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
	 * @create 2020/09/04 KBS S.Tanaka
	 * 
	 * @update 2020/11/06 KBS S.Tanaka 日付が2桁表記の場合のバリデーションエラーを追加
	 */
	public function withValidator($validator)
	{
		$groupId = $this->input('val2');
		$floorCode = $this->input('val3');
		$distCode = $this->input('val4');
		$sDate = $this->input('val5');
		$eDate = $this->input('val6');
		$hr = $this->input('val7');

		if(is_null($sDate)){
			$sDateResult = true;
			$sDateMsg = null;
		}else{
			list($sDateResult, $sDateMsg) = FuncCommon::checkFormatDate($sDate);
		}
		
		if(is_null($eDate)){
			$eDate = '';
			$eDateResult = true;
			$eDateMsg = null;
		}else{
			list($eDateResult, $eDateMsg) = FuncCommon::checkFormatDate($eDate);
		}

		if($sDateResult && $eDateResult && !is_null($sDate)){
			$validator->addRules(['val6' => 'after:val5']);
		}else{
			if(!$sDateResult){
				$validator->after(function ($validator) use($sDateMsg){
					$validator->errors()->add('val5', sprintf($sDateMsg, '開始日'));
				});
			}
			if(!$eDateResult){
				$validator->after(function ($validator) use($eDateMsg){
					$validator->errors()->add('val6', sprintf($eDateMsg, '終了日'));
				});
			}
		}

		if(!$groupId && $floorCode == '' && $distCode == ''){
			for($num = 2; $num < 5; $num++){
				$validator->after(function ($validator) use($num){
					$validator->errors()->add('val'.$num, config("message.msg_validation_003"));
				});
			}
		}else{
			if($groupId || ($floorCode == '' && $distCode == '')){
				$val2 = array();
				
				if($sDateResult && $eDateResult && !is_null($sDate)){
					$val2 = [
						Rule::exists('mstOrg', 'ID')->where(function ($query) use($groupId, $sDate, $eDate) {
							$query->where(function ($query1) use($sDate, $eDate) {
								$query1->where(function ($query2) use($sDate, $eDate) {
									$query2->whereNotNull('mstOrg.Edate')
										->whereDate('mstOrg.Sdate', '<=', $sDate)
										->whereDate('mstOrg.Edate', '>=', $eDate);
								})
									->orwhere(function($query3) use($sDate) {
										$query3->whereNull('mstOrg.Edate')
											->whereDate('mstOrg.Sdate', '<=', $sDate);
									});
							})
							->where('mstOrg.ID', '=', $groupId);
						})
					];
					$validator->addRules(['val2' => $val2]);
				}
			}
			$validator->sometimes('val3', 'regex:/^[0-9a-zA-Z]*$/|exists:mstFloor,Code,ViewFlag,1|max:10', function ($input) {
				return $input->val3 != '' || (!$input->val2 && $input->val4 == '');
			});
			$validator->sometimes('val4', 'regex:/^[0-9a-zA-Z]*$/|exists:mstDist,Code|max:5', function ($input) {
				return $input->val4 != '' || (!$input->val2 && $input->val3 == '');
			});
		}

		//工数が正の数かつ整数6桁以内、小数2桁以内か
		if(!(preg_match('/^([1-9][0-9]{0,5}|0)(\.[0-9]{1,2})?$/', (double)$hr))){
			$validator->after(function ($validator) use($eDateMsg){
				$validator->errors()->add('val7', config("message.msg_validation_006"));
			});
		}
	}

	/**
	 * Get the validator instance for the request.
	 *
	 * @return \Illuminate\Contracts\Validation\Validator
	 * @create 2020/11/10 S.Tanaka
	 *
	 * @update
	 */
	public function getValidatorInstance()
	{
		$this->decodeValRequest();
		return parent::getValidatorInstance();
	}

	/**
	 * decode request val2.
	 *
	 * @return array
	 * @create 2020/11/10 S.Tanaka
	 *
	 * @update
	 */
	protected function decodeValRequest()
	{
		if($this->request->has('val2')){
			$this->merge([
				'val2' => valueUrlDecode($this->request->get('val2'))
			]);
		}
	}

	/**
	 * Get the validation rules that apply to the request.
	 *
	 * @return array
	 * @create 2020/08/27 KBS S.Tanaka
	 * 
	 * @update
	 */
	public function rules()
	{	

		return [
			'val1' => 'required|max:100',
			'val2' => 'numeric',
			'val5' => 'required',
			'val6' => 'nullable',
			'val7' => 'nullable|numeric',
		];
	}

	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 * @create 2020/08/27 KBS S.Tanaka
	 * 
	 * @update
	 */
	public function attributes()
	{
		return [
			'val1' => '能力時間名称',
			'val2' => '職制',
			'val3' => '施工棟',
			'val4' => '職種',
			'val5' => '開始日',
			'val6' => '終了日',
			'val7' => '工数',
		];
	}

	/**
	 * バリデーションエラーメッセージ
	 *
	 * @return array
	 * @create 2020/08/31 KBS S.Tanaka
	 * 
	 * @update
	 */
	public function messages() {
		return [
			'val2.exists' => config("message.msg_validation_002"),
			'val2.required' => config("message.msg_validation_003"),
			'val3.exists' => config("message.msg_validation_004"),
			'val3.required' => config("message.msg_validation_003"),
			'val4.exists' => config("message.msg_validation_005"),
			'val4.required' => config("message.msg_validation_003"),
		];
	}
	
	/**
	 * failedValidation
	 *
	 * @return mix RedirectResponse
	 * @create 2020/11/10 Cuong
	 *
	 * @update
	 */
	protected function failedValidation(Validator $validator) {
		throw (new ValidationException($validator))
					->errorBag($this->errorBag)
					->redirectTo($this->getRedirectUrl());
	}

	/**
	 * get redirect url after failedValidation
	 *
	 * @return string
	 * @create 2020/11/10 S.Tanaka
	 *
	 * @update
	 */
	protected function getRedirectUrl() {
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

		// create
		if(strpos($parseUrl['path'], '/create') !== false) {
			unset($query['val10']);
		}

		// edit
		foreach($query as $key => $value) {
			if (strpos($key, 'val') !== false || $key == 'err1') {
				if($key != 'val1'){
					if($key == 'val10') {
						$query['val1'] = $query['val10'];
					}
					unset($query[$key]);
				}
			}
		}

		$arrUrl = explode('?', $url->previous());
		$queryString = '?'.http_build_query($query);

		return $arrUrl[0].$queryString;
	}
}