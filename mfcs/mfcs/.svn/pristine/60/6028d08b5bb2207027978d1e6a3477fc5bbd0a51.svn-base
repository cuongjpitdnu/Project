@extends('layouts/mainmenu/menu')
@section('content')
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();
		var selectedValueVal2 = '{{ isset($request->val2) ? old('val2',$request->val2) : (is_null(old('val2')) ? valueUrlEncode(config('system_const.c_kind_chijyo')) : old('val2')) }}';
		const projects = fncJsonParse('{{ json_encode($projects) }}');
		binding(projects, selectedValueVal2, '{{ (old('val3',$request->val3)) }}');
		$('input[name=val2]').on('change', function() {
			$('select[name=val3]').empty();
			binding(projects, $(this).val(), '');
		});
		function binding(data, selectWhere, selected) {
			let arrUnique = [];
			$('select[name=val3]').empty();
			if(data.length > 0) {
				let flagHasValue = false;
				data.forEach(element => {
					var selectValue = (element.ID == selected) ? 'selected' : '';
					if(selectWhere == element.ListKind){
						if(arrUnique.length === 0) {
							flagHasValue = true;
							$('select[name=val3]').append(`<option value="" hidden></option><option value="${element.ID}" ${selectValue}>${convertHTML(element.ProjectName)}</option>`);
							arrUnique.push(element.ProjectName);
						} else {
							if(arrUnique.indexOf(element.ProjectName) === -1) {
								flagHasValue = true;
								$('select[name=val3]').append(`</option><option value="${element.ID}" ${selectValue}>${convertHTML(element.ProjectName)}</option>`);
								arrUnique.push(element.ProjectName);
							}
						}
					}
				});
				if(!flagHasValue) { $('select[name=val3]').append('<option value="" hidden></option>'); }
			} else {
				$('select[name=val3]').append('<option value=""></option>');
			}
		}
		$('#save').on('click', function(){
			$('#indicator').trigger('click');
			$('select[name=val3]').val($('select[name=val3]').val() == -1 ?  null : $('select[name=val3]') .val());
			$('#mainform').submit();
		});
	});
</script>
<form action="{{ url('/') }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/import" method="POST" id="mainform">
	@csrf
	<input type="hidden" id="kindid" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}">
	<input type="hidden" id="menuid" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}">
	<input type="hidden" id="selectedValueVal2" value="{{old('val2',$request->val2)}}">
	<div class="row ml-2 mr-2">
		<div class="col-md-12 col-xs-12">
			<div class="row align-items-center">
				<div class="col-xs-1 text-left m-2 p-2 rounded border">
					■　搭載日程展開
				</div>
			</div>
			@if (isset($originalError) && count($originalError) > 0)
			<div class="row">
				<div class="col-xs-12">
					<div class="alert alert-danger">
						<ul>
							@foreach ($originalError as $error)
							<li>{{ $error }}</li>
							@endforeach
						</ul>
					</div>
				</div>
			</div>
			@endif
			<div class="row head-purple">
				<div class="col-xs-12">搭載日程選択</div>
			</div>
			<div class="row ml-1 mt-3">
				<div class="col-xs-12">
					<table class="table table-borderless mb-0">
						<tr>
							<td class="td-mw-108 align-middle">オーダ：</td>
							<td>
								<select name="val1">
									@if(count($orders) > 0)
										@foreach ($orders as $value)
											<option value="{{ valueUrlEncode($value->OrderNo) }}" 
												{{ trim(valueUrlDecode(old('val1',@$request->val1)))
													=== trim($value->OrderNo) ? 'selected' : '' }}>
												{{ $value->OrderNo }}
											</option>
										@endforeach
									@else
										<option value=""></option>
									@endif
								</select>					
							</td>
							<td class="p-0 align-middle">
								<span class="col-xs-1 p-1">
								@include('layouts/error/item', ['name' => 'val1'])
								</span>
							</td>
						</tr>
					</table>
				</div>
			</div>
			<div class="row head-purple">
				<div class="col-xs-12">中日程選択</div>
			</div>
			<div class="row ml-1">
				<div class="col-xs-12">
					<table class="table table-borderless">
						<tr>
							<td class="td-mw-108 align-middle">中日程区分 : </td>
							<td>
								<label>
									<input type="radio" id="rdo1" name="val2" 
										value="{{ valueUrlEncode(config('system_const.c_kind_chijyo')) }}" 
										{{ (int)valueUrlDecode(old('val2',@$itemShow['val2']))
										=== config('system_const.c_kind_chijyo') ? 'checked' : '' }}>
										{{ config('system_const.c_name_chijyo') }}
								</label> /
								<label>
									<input type="radio" id="rdo2" name="val2" 
										value="{{ valueUrlEncode(config('system_const.c_kind_gaigyo')) }}" 
										{{ (int)valueUrlDecode(old('val2',@$itemShow['val2']))
										=== config('system_const.c_kind_gaigyo') ? 'checked' : '' }}>
										{{ config('system_const.c_name_gaigyo') }}
								</label> /
								<label>
									<input type="radio" id="rdo3" name="val2" 
										value=" {{ valueUrlEncode(config('system_const.c_kind_giso')) }}" 
										{{  (int)valueUrlDecode(old('val2',@$itemShow['val2']))
										=== config('system_const.c_kind_giso') ? 'checked' : '' }}> 
										{{ config('system_const.c_name_giso') }}
								</label>
							</td>
							<td class="p-0 align-middle">
								<span class="col-xs-1 p-1">
									@include('layouts/error/item', ['name' => 'val2'])
								</span>
							</td>
						</tr>
						<tr>
							<td class="td-mw-108 align-middle">検討ケース：</td>
							<td>
								<select name="val3" class="selectedValueVal2"></select>
							</td>
							<td class="p-0 align-middle">
								<span class="col-xs-1 p-1">
									@include('layouts/error/item', ['name' => 'val3'])
								</span>
							</td>
						</tr>
					</table>
				</div>
			</div>
			<div class="row head-purple">
				<div class="col-xs-12">ログ表示設定</div>
			</div>
			<div class="row ml-1 mt-3">
				<div class="col-xs-12">
					<table class="table table-borderless">
						<tbody>
							<tr>
								<td class="td-mw-108 align-middle">表示件数：</td>
								<td>
									<select name="val4" class="wd-15 pageunit-width">
										<option value="{{ valueUrlEncode(config('system_const.displayed_results_1')) }}" 
											{{ (int)valueUrlDecode(old('val4',@$itemShow['val4']))
											=== config('system_const.displayed_results_1') ? 'selected' : '' }}>
											{{ config('system_const.displayed_results_1') }}
										</option>
										<option value="{{ valueUrlEncode(config('system_const.displayed_results_2')) }}" 
											{{ (int)valueUrlDecode(old('val4',@$itemShow['val4']))
											=== config('system_const.displayed_results_2') ? 'selected' : '' }}>
											{{config('system_const.displayed_results_2')}}
										</option>
										<option value="{{ valueUrlEncode(config('system_const.displayed_results_3')) }}" 
											{{ (int)valueUrlDecode(old('val4',@$itemShow['val4']))
											=== config('system_const.displayed_results_3') ? 'selected' : '' }}>
											{{ config('system_const.displayed_results_3') }}
										</option>
									</select>
									
								</td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val4'])
									</span>
								</td>
								<td> ※1ページあたり</td>
							</tr>
						</tbody>
					</table>
				</div>
			</div>
			<div class="row">
				<div class="col-sm-12">
					<div class="col-xs-1 p-1">
						<button type="button" id="save" class="{{ config('system_const.btn_color_ok') }}">
							<i class="{{ config('system_const.btn_img_ok') }}"></i>
							{{ config('system_const.btn_char_ok') }}
						</button>
					</div>
				</div>
			</div>
		</div>
	</div>
</form>
@endsection