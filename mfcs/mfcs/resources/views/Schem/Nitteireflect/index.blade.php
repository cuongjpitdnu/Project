@extends('layouts/mainmenu/menu')
@section('content')
<script>
$(function() {
	$('[data-toggle="tooltip"]').tooltip();

	const dataVal2 = fncJsonParse('{{ json_encode($dataView['data_2_all']) }}');
	const dataVal4 = fncJsonParse('{{ json_encode($dataView['data_4_all']) }}');

	bindingSelect('val2', dataVal2, $('[name=val1]').val(), '');
	bindingSelect('val4', dataVal4, $('[name=val1]').val(), $('[name=val3]:checked').val());

	$('[name=val1]').on('change', function(e) {
		bindingSelect('val2', dataVal2, $(this).val(), '');
		bindingSelect('val4', dataVal4, $(this).val(), $('[name=val3]:checked').val());
	});

	$('[name=val2]').on('change', function(e) {
		bindingSelect('val4', dataVal4, $(this).val(), $('[name=val3]:checked').val());
	});

	$('[name=val3]').on('change', function(e) {
		bindingSelect('val4', dataVal4, '', $(this).val());
	});

	$('#ok').on('click', function(e) {
		$('#indicator').trigger('click');
		$('#mainform').submit();
	});

});

function bindingSelect(name_input, data, project_filter, ckind_filter) {
	$('#indicator').trigger('click');
	if(['val2', 'val4'].indexOf(name_input) > -1) {
		let arrUnique = [];
		let arrCKind = [];
		$('[name='+name_input+']').empty();
		if(data.length > 0) {
			let flagHasValue = false;
			if(name_input == 'val4') {
				ckind_2_filter = $('option:selected', $('[name=val2]')).attr(`ckind`);
				if (typeof ckind_2_filter !== 'undefined' && ckind_2_filter !== '') {
					arrCKind = ckind_2_filter.split(',');
				}
				$.each(data, function(i, e) {
					if(ckind_filter === e.ListKind && arrCKind.indexOf(e.ListKind) >= 0) {
						if(arrUnique.length === 0) {
							flagHasValue = true;
							$('[name=val4]').append(`<option value="${e.val}">${convertHTML(e.valName)}</option>`);
							arrUnique.push(e.valName);
						} else {
							if(arrUnique.indexOf(e.valName) === -1) {
								flagHasValue = true;
								$('[name=val4]').append(`<option value="${e.val}">${convertHTML(e.valName)}</option>`);
								arrUnique.push(e.valName);
							}
						}
					}
				});
				$('[name=val4] option').each(function(){
					if ($(this).val() === "{{ trim(old('val4', @$itemShow['val4'])) }}") {
						$(this).attr('selected', 'selected');
					}
				});
			} else {
				$.each(data, function(i, e) {
					if(project_filter === e.ProjectID) {
						if(arrUnique.length === 0) {
							flagHasValue = true;
							$('[name=val2]').append(`<option value="${e.val2}" ckind="${e.CKind}">${convertHTML(e.NameShow)}</option>`);
							arrUnique.push(e.val2);
						} else {
							if(arrUnique.indexOf(e.val2) === -1) {
								flagHasValue = true;
								$('[name=val2]').append(`<option value="${e.val2}" ckind="${e.CKind}">${convertHTML(e.NameShow)}</option>`);
								arrUnique.push(e.val2);
							} else {
								$('[name=val2] option').each(function(){
									if ($(this).val() === e.val2) {
										$(this).attr('ckind', $(this).attr('ckind') + ',' + e.CKind);
									}
								});
							}
						}
					}
				});
				$('[name=val2] option').each(function(){
					if ($(this).val() === "{{ trim(old('val2', @$itemShow['val2'])) }}") {
						$(this).attr('selected', 'selected');
					}
				});
			}
			if(!flagHasValue) { $('[name='+name_input+']').append('<option value="{{ valueUrlEncode('') }}"></option>'); }
		} else {
			$('[name='+name_input+']').append('<option value="{{ valueUrlEncode('') }}"></option>');
		}
	}
	indicatorHide();
}
</script>

<div class="row ml-2 mr-2">
	<div class="col-md-12 col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　小日程反映
			</div>
		</div>
		@if (isset($originalError) && count($originalError) > 0)
		<div class="row">
			<div class="col-xs-12" id="area-error">
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

		<form action="{{ url('/') }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/reflect" method="POST" id="mainform">
			@csrf
			<input type="hidden" id="kindid" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}" />
			<input type="hidden" id="menuid" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}" />
			<div class="row head-purple">
				<div class="col-xs-12">小日程反映</div>
			</div>
			<div class="row ml-1 mt-3">
				<div class="col-xs-12">
					<table class="table table-borderless">
						<tr>
							<td class="align-middle">検討ケース：</td>
							<td>
								<select name="val1" id="">
									@if(count($dataView['data_1']) > 0)
										@foreach ($dataView['data_1'] as $item)
											<option value={{ $item->val }}
												{{ trim(old('val1', @$itemShow['val1'])) === trim($item->val) ? 'selected': '' }}>{{
													$item->valName }}</option>
										@endforeach
									@else
										<option value="{{ valueUrlEncode('') }}"></option>
									@endif
								</select>
								@include('layouts/error/item', ['name' => 'val1'])
							</td>
							<td class="align-middle">オーダ：</td>
							<td>
								<select name="val2" id="">
									@if(count($dataView['data_2']) > 0)
										@foreach ($dataView['data_2'] as $item)
											<option value={{ $item->val2 }} ckind={{ $item->CKind }}
												{{ trim(old('val2', @$itemShow['val2'])) === trim($item->val2) ? 'selected': '' }}>{{ $item->NameShow }}</option>
										@endforeach
									@else
										<option value="{{ valueUrlEncode('') }}"></option>
									@endif
								</select>
								@include('layouts/error/item', ['name' => 'val2'])
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
							<td class="align-middle">中日程区分：</td>
							<td>
								<label class="mb-0 align-middle" for="rdo1"><input type="radio" id="rdo1" name="val3"
									value="{{ valueUrlEncode(config('system_const.c_kind_chijyo')) }}"
									{{ old('val3', @$itemShow['val3']) === valueUrlEncode(config('system_const.c_kind_chijyo')) ?
									'checked' : '' }}> {{ config('system_const.c_name_chijyo') }}</label> /
								<label class="mb-0 align-middle" for="rdo2"><input type="radio" id="rdo2" name="val3"
									value="{{ valueUrlEncode(config('system_const.c_kind_gaigyo')) }}"
									{{ old('val3', @$itemShow['val3']) === valueUrlEncode(config('system_const.c_kind_gaigyo')) ?
									'checked' : '' }}> {{ config('system_const.c_name_gaigyo') }}</label> /
								<label class="mb-0 align-middle" for="rdo3"><input type="radio" id="rdo3" name="val3"
									value="{{ valueUrlEncode(config('system_const.c_kind_giso')) }}"
									{{ old('val3', @$itemShow['val3']) === valueUrlEncode(config('system_const.c_kind_giso')) ?
									'checked' : '' }}> {{ config('system_const.c_name_giso') }}</label>
								@include('layouts/error/item', ['name' => 'val3'])
							</td>
						</tr>
						<tr>
							<td class="align-middle">検討ケース：</td>
							<td>
								<select name="val4" id="">
									@if(count($dataView['data_4']) > 0)
										@foreach ($dataView['data_4'] as $item)
											<option value={{ $item->val }}
												{{ trim(old('val4', @$itemShow['val4'])) === trim($item->val) ? 'selected': '' }}>{{ $item->valName }}</option>
										@endforeach
									@else
										<option value="{{ valueUrlEncode('') }}"></option>
									@endif
								</select>
								@include('layouts/error/item', ['name' => 'val4'])
							</td>
						</tr>
					</table>
				</div>
			</div>
		</form>

		<div class="row ml-1">
			<div class="col-xs-1 p-1">
				<button type="button" id="ok" class="{{ config('system_const.btn_color_ok') }}">
					<i class="{{ config('system_const.btn_img_ok') }}"></i>{{ config('system_const.btn_char_ok') }}
				</button>
			</div>
		</div>
	</div>
</div>
@endsection