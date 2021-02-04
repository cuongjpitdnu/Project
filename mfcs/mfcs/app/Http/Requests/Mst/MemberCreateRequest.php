<?php
/*
 * @MemberCreateRequest.php
 * 
 * @create 2020/09/21 Cuong
 *
 * @update 2020/10/19 Cuong update check format datetime
 * @update 2020/11/09 KBS S.Tanaka 日付が2桁表記の場合のバリデーションエラーを追加
 */
namespace App\Http\Requests\Mst;

use Illuminate\Foundation\Http\FormRequest;
use App\Librarys\FuncCommon;
use App\Models\MstOrg;
use App\Models\MEMHist;
use DB;

class MemberCreateRequest extends FormRequest
{
	/**
	* Determine if the user is authorized to make this request.
	*
	* @return bool
	* @create 2020/09/21 Cuong
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
	* @create 2020/09/21 Cuong
	*
	* @update
	*/
	public function rules()
	{
		return [
			'val401' => 'required|string|max:50',
			'val402' => 'required|string|max:50',
			'val403' => 'required|string|max:50',
			'val404' => 'nullable',
			'val301' => 'nullable|integer|min:0|max:2147483647',
			'val302' => 'required|numeric|between:-2147483648,2147483647',
			'val303' => 'nullable|numeric|between:-2147483648,2147483647',
			'val304' => 'nullable|numeric|between:-2147483648,2147483647',
			'val305' => 'required',
			'val306' => 'nullable',
			'val307' => 'nullable|numeric|between:-2147483648,2147483647',
			'val308' => 'required|string|max:5',
			'val309' => 'required|string|max:3',
		];
	}

	/**
	 * Get the validator instance for the request.
	 *
	 * @return \Illuminate\Contracts\Validation\Validator
	 * @create 2020/09/21 Cuong
	 *
	 * @update
	 */
	public function getValidatorInstance()
	{
		$this->decodeValRequest();
		return parent::getValidatorInstance();
	}

	/**
	 * decode request val304.
	 *
	 * @return array
	 * @create 2020/09/21 Cuong
	 *
	 * @update
	 */
	protected function decodeValRequest()
	{
		if($this->request->has('val304')){
			$this->merge([
				'val304' => valueUrlDecode($this->request->get('val304'))
			]);
		}
	}

	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 * @create 2020/09/21 Cuong
	 *
	 * @update
	 */
	public function attributes(){
		return [
			'val401' => '名前',
			'val402' => 'フリガナ',
			'val403' => '略称',
			'val404' => '定年退職日',
			'val301' => '社員番号',
			'val302' => '社内外フラグ',
			'val303' => '会社名',
			'val304' => '所属班',
			'val305' => '開始日',
			'val306' => '終了日',
			'val307' => '外注タイプ',
			'val308' => '職種',
			'val309' => 'プロパー',
		];
	}

	/**
	 * 独自処理を追加する
	 *
	 * @return array
	 * @create 2020/09/21 Cuong
	 * 
	 * @update 2020/09/23 Cuong check validate val304
	 * @update 2020/11/09 KBS S.Tanaka 日付が2桁表記の場合のバリデーションエラーを追加
	 * @update 2020/11/25 update check validate val301
	 * @update 2020/01/27 update check validate val301
	 */
	public function withValidator($validator)
	{	
		$dateNow = DB::selectOne('SELECT CONVERT(DATE, getdate()) AS sysdate')->sysdate;
		$dateNow = str_replace('-', '/', $dateNow);
		$retireDate = $this->input('val404');
		$sDate = $this->input('val305');
		$eDate = $this->input('val306');
		
		// check validate val301
		if(!is_null($this->val301) && is_int($this->val301)) {
			$mstObjMemHist = MEMHist::where('WorkerNo', '=', $this->val301)->get();
			if(count($mstObjMemHist) > 0) {
				$validator->after(function ($validator) {
					$validator->errors()->add('val301', config('message.msg_member_if_004'));
				});
			}
		}
		
		if(is_null($retireDate)){
			$retireDateResult = true;
			$retireDateMsg = null;
		}else{
			list($retireDateResult, $retireDateMsg) = FuncCommon::checkFormatDate($retireDate);
		}

		if(is_null($sDate)){
			$sDateResult = true;
			$sDateMsg = null;
		}else{
			list($sDateResult, $sDateMsg) = FuncCommon::checkFormatDate($sDate);
		}

		if(is_null($eDate)){
			$eDateResult = true;
			$eDateMsg = null;
		}else{
			list($eDateResult, $eDateMsg) = FuncCommon::checkFormatDate($eDate);
		}

		if(!$retireDateResult){
			$validator->after(function ($validator) use($retireDateMsg){
				$validator->errors()->add('val404', sprintf($retireDateMsg, '定年退職日'));
			});
		}

		if($sDateResult && $eDateResult && !is_null($sDate)) {
			$rules = ['val306' => 'after:val305'];
			$validator->addRules($rules);
		}else{
			if(!$sDateResult){
				$validator->after(function ($validator) use($sDateMsg){
					$validator->errors()->add('val305', sprintf($sDateMsg, '開始日'));
				});
			}
			if(!$eDateResult){
				$validator->after(function ($validator) use($eDateMsg){
					$validator->errors()->add('val306', sprintf($eDateMsg, '終了日'));
				});
			}
		}
		
			//check validate val304
		if($sDateResult && $eDateResult && !is_null($sDate)) {
			$orgID = $this->val304;
			if(!is_null($orgID)){
				$flag = false;
				$mstOrg = MstOrg::where('ID', '=', $orgID)
								->whereRaw('( SELECT MIN ( SDate ) FROM mstOrg WHERE ID = ? ) <= ? ', [$orgID,$sDate])
								->whereRaw('( ( SELECT MAX ( EDate ) FROM mstOrg WHERE ID = ? ) >= ? OR EDate IS NULL )', [$orgID,$eDate])
								->get()->toArray();

				if (count($mstOrg) == 0) {
					$validator->after(function ($validator) {
						$validator->errors()->add('val304', config('message.msg_member_if_003'));
					});
				}else {
					$arrSDate = array_column($mstOrg, 'Sdate');
					$maxSDate = max($arrSDate);

					$mstOrgMaxSDate = array_values(array_filter($mstOrg, function ($org) use ($maxSDate) {
						return ($org['Sdate'] == $maxSDate);
					}));

					if($mstOrgMaxSDate[0]['FolderFlag'] == 1) {
						$flag = true;
					}
					$validator->after(function ($validator) use ($flag) {
						if ($flag) {
							$validator->errors()->add('val304', config('message.msg_member_if_002'));
						}
					});
				}
			}
		}
	}
}
