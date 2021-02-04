<?php
/*
 * @PatternDetailContentsRequest.php
 *
 * @create 2020/09/15 Chien
 *
 * @update
 */
namespace App\Http\Requests\Schem;

use Illuminate\Contracts\Validation\Validator;
use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Validation\ValidationException;
use App\Models\Cyn_mstKotei_STR_C;
use App\Models\Cyn_mstKotei;
use App\Models\MstFloor;
use App\Models\MstBDCode;
/*
 * PatternDetailContentsRequest class
 *
 * @create 2020/09/15 Chien
 *
 * @update
 *
 */
class PatternDetailContentsRequest extends FormRequest
{
	/**
	 * Determine if the user is authorized to make this request.
	 *
	 * @return bool
	 * @create 2020/09/15 Chien
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
	 * @create 2020/09/15 Chien
	 *
	 * @update 2020/10/16 Chien
	 */
	public function rules() {
		return [
			'val201' => 'required',
			'val202' => 'nullable|integer|min:1',
			'val203' => 'required',
			'val204' => 'nullable',
			'val205' => 'nullable',
			'val206' => 'nullable',
			'val207' => 'nullable|integer',
			'val208' => 'nullable',
		];
	}

	/**
	 * Set custom attributes for validator errors.
	 *
	 * @return array
	 * @create 2020/09/15 Chien
	 *
	 * @update
	 */
	public function attributes() {
		return [
			'val201' => '組区',
			'val202' => '工期',
			'val203' => '工程',
			'val204' => '施工棟',
			'val205' => '管理物量',
			'val206' => '次組区',
			'val207' => 'リンク日数',
			'val208' => '次工程',
		];
	}

	/**
	 * 独自処理を追加する
	 *
	 * @return
	 * @create 2020/09/16 Chien
	 *
	 * @update
	 */
	public function withValidator($validator) {
		$cKind = valueUrlDecode($this->val3);
		$code = valueUrlDecode($this->val1);
		$no = valueUrlDecode($this->val101);
		$updatedAt = valueUrlDecode($this->val102);

		// val201
		if(trim($this->val201) != '') {
			$checkData = Cyn_mstKotei_STR_C::query()
							->where('CKind', '=', $cKind)
							->where('Code', '=', $code)
							->where('KKumiku', '=', $this->val201)
							->where('Kotei', '=', $this->val203);

			if(trim($this->input('method')) == 'edit') {
				$checkData = $checkData->where('No', '<>', $no);
			}

			$checkData = $checkData->first();

			if($checkData != null) {
				$validator->after(function ($validator) {
					$validator->errors()->add('val201', config('message.msg_cmn_db_013'));
				});
			}
		}

		// val206
		if(trim($this->val206) == '' && trim($this->val208) != '') {
			$validator->after(function ($validator) {
				$validator->errors()->add('val206', config('message.msg_cmn_db_011'));
			});
		}
		if(trim($this->val206) != '' && trim($this->val208) != '') {
			if(trim($this->val201) === trim($this->val206) && trim($this->val203) === trim($this->val208)) {
				$validator->after(function ($validator) {
					$validator->errors()->add('val206', config('message.msg_cmn_db_014'));
				});
			}
			$checkData = Cyn_mstKotei_STR_C::query()
							->where('CKind', '=', $cKind)
							->where('Code', '=', $code)
							->where('KKumiku', '=', $this->val206)
							->where('Kotei', '=', $this->val208);

			// if(trim($this->input('method')) == 'edit') {
			// 	$checkData = $checkData->where('No', '<>', $no);
			// }
			$checkData = $checkData->first();

			if($checkData == null) {
				$validator->after(function ($validator) {
					$validator->errors()->add('val206', config('message.msg_cmn_db_010'));
				});
			}
		}

		// val208
		if(trim($this->val206) != '' && trim($this->val208) == '') {
			$validator->after(function ($validator) {
				$validator->errors()->add('val208', config('message.msg_cmn_db_012'));
			});
		}

		return $validator;
	}

	/**
	 * failedValidation
	 *
	 * @return mix RedirectResponse
	 * @create 2020/08/20 Cuong
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
	 * @create 2020/08/20 Cuong
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
			// unset($query['val101']);
			// unset($query['val103']);
		}

		// edit
		foreach($query as $key => $value) {
			if (strpos($key, 'val') !== false || $key == 'err1') {
				if(!($key == 'val1' || $key == 'val3' || $key == 'val101' || $key == 'val102')) {
					unset($query[$key]);
				}
			}
		}

		$arrUrl = explode('?', $url->previous());
		$queryString = '?'.http_build_query($query);

		return $arrUrl[0].$queryString;
	}
}