@extends('layouts/mainmenu/menu')
@section('content')
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();
		var dataVal1 = fncJsonParse('{{ json_encode($val1_All) }}');
		var dataVal2 = fncJsonParse('{{ json_encode($val2_All) }}');
		var dataVal3 = fncJsonParse('{{ json_encode($val3_All) }}');

		var val1 = $(".val1 option:selected").val();
		var val2Selected = '{{ isset($request->val2) ? $request->val2 : "" }}';
		bindingSelect('val2', dataVal2, val1, val2Selected);

		var val2 = $(".val2 option:selected").val();
		var val3Selected = '{{ isset($request->val3) ? $request->val3 : "" }}';
		bindingSelect('val3', dataVal3, val2, val3Selected);

		$('[name=val1]').on('change', function(e) {
			bindingSelect('val2', dataVal2, $(this).val());
			var val2 = $(".val2 option:selected").val();
			bindingSelect('val3', dataVal3, val2);
		});

		$('[name=val2]').on('change', function(e) {
			bindingSelect('val3', dataVal3, $(this).val());
		});

		$('#save').on('click', function(e) {
			$('#indicator').trigger('click');
			$('#mainform').submit();
		});

		$('.input-checkbox').off().click(function(){
			if($(this).prop('checked')){
				$('[name="'+$(this).attr('checkbox')+'"]').val('{{valueUrlEncode(1)}}');
			}else{
				$('[name="'+$(this).attr('checkbox')+'"]').val('{{valueUrlEncode(0)}}');
			}
		});

		$('.selectdate').datepicker();

		function bindingSelect(name_input, data, filterKey, itemSelected) {
			$('#indicator').trigger('click');
			if(['val2', 'val3'].indexOf(name_input) > -1) {
				let arrUnique = [];
				$('[name='+name_input+']').empty();
				if(data.length > 0) {
					let flagHasValue = false;
					if(name_input == 'val2') {
						$.each(data, function(i, e) {
							if(filterKey == e.SummaryType) {
								if(arrUnique.length == 0) {
									flagHasValue = true;
									if(e.val2 == itemSelected) {
										$('[name=val2]').append(`<option selected value="${e.val2}">${convertHTML(e.val2Name)}</option>`);
									}else {
										$('[name=val2]').append(`<option value="${e.val2}">${convertHTML(e.val2Name)}</option>`);
									}
									arrUnique.push(e.val2Name);
								} else {
									if(arrUnique.indexOf(e.val2Name) === -1) {
										flagHasValue = true;
										if(e.val2 == itemSelected) {
											$('[name=val2]').append(`<option selected value="${e.val2}">${convertHTML(e.val2Name)}</option>`);
										}else {
											$('[name=val2]').append(`<option value="${e.val2}">${convertHTML(e.val2Name)}</option>`);
										}
										arrUnique.push(e.val2Name);
									}
								}
							}
						});
						if(!flagHasValue) { $('[name='+name_input+']').append('<option value=""></option>'); }
					}
					else if(name_input == 'val3') {
						$.each(data, function(i, e) {
							if(filterKey == e.SummaryID) {
								if(arrUnique.length == 0) {
									flagHasValue = true;
									if(e.val3 == itemSelected) {
										$('[name=val3]').append(`<option selected value="${e.val3}">${convertHTML(e.val3Name)}</option>`);
									}else {
										$('[name=val3]').append(`<option value="${e.val3}">${convertHTML(e.val3Name)}</option>`);
									}
									arrUnique.push(e.val3Name);
								} else {
									if(arrUnique.indexOf(e.val3Name) === -1) {
										flagHasValue = true;
										if(e.val3 == itemSelected) {
											$('[name=val3]').append(`<option selected value="${e.val3}">${convertHTML(e.val3Name)}</option>`);
										}else {
											$('[name=val3]').append(`<option value="${e.val3}">${convertHTML(e.val3Name)}</option>`);
										}
										arrUnique.push(e.val3Name);
									}
								}
							}
						});
						if(!flagHasValue) { 
							$('[name='+name_input+']').append('<option value=""></option>'); 
							$('#checkbox_val5, #checkbox_val6').html('');
						}else {
							$('#checkbox_val5').html('').append(`
								<input type="checkbox" class="input-checkbox" {{ isset($request->val5) && $request->val5 == valueUrlEncode(config('system_const_report.summary_code_subtotal')) ? 'checked' : '' }} checkbox="val5"/> 
								{{ config('system_const_report.summary_name_subtotal') }}
								<input type="hidden" name="val5" value="{{ isset($request->val5) ? $request->val5 : valueUrlEncode(0) }}">
							`);
							$('#checkbox_val6').html('').append(`
								<input type="checkbox" class="input-checkbox" {{ isset($request->val6) && $request->val6 == valueUrlEncode(config('system_const_report.summary_code_total')) ? 'checked' : '' }} checkbox="val6"/> 
								{{ config("system_const_report.summary_name_total") }}
								<input type="hidden" name="val6" value="{{ isset($request->val6) ? $request->val6 : valueUrlEncode(0) }}">
							`); 

							$('.input-checkbox').off().click(function(){
								if($(this).prop('checked')){
									$('[name="'+$(this).attr('checkbox')+'"]').val('{{valueUrlEncode(1)}}');
								}else{
									$('[name="'+$(this).attr('checkbox')+'"]').val('{{valueUrlEncode(0)}}');
								}
							});
						}
					}
					
				}
				else 
				{
					if(name_input == 'val2') {
						$('[name='+name_input+']').append('<option value=""></option>');
					}

					if(name_input == 'val3') {
						$('#checkbox_val5, #checkbox_val6').html('');
					}
				}
			}
			indicatorHide();
		}
	});
</script>
<div class="row ml-2 mr-2">
	<div class="col-md-12 col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　汎用集計表
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
		<form action="{{ url('/') }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/settings" method="POST" id="mainform" enctype="multipart/form-data">
			@csrf
			<input type="hidden" id="kindid" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}" />
			<input type="hidden" id="menuid" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}" />

			<div class="row ml-1 mt-3">
				<div class="col-xs-12">
					<table class="table table-borderless mb-0">
						<tr>
							<td class="td-mw-108 align-middle">集計表タイプ：</td>
							<td>
								<select name="val1" class="val1" id="">
								@if (count($val1_All) > 0)
									@foreach ($val1_All as $item)
											<option {{ trim(old('val1', @$itemShow['val1'])) === trim($item->val1) ? 'selected' : '' }} value="{{$item->val1}}">
												{{$item->val1Name}}
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
						<tr>
							<td class="td-mw-108 align-middle">集計表名：</td>
							<td>
								<select name="val2" class="val2" id="">

								</select>
							</td>
							<td class="p-0 align-middle">
								<span class="col-xs-1 p-1">
								@include('layouts/error/item', ['name' => 'val2'])
								</span>
							</td>
						</tr>
						<tr>
							<td class="td-mw-108 align-middle">条件項目選択：</td>
							<td>
								<select class="form-select h-auto val3" multiple name="val3">

								</select>
							</td>
							<td class="p-0 align-middle">
								<span class="col-xs-1 p-1">
								@include('layouts/error/item', ['name' => 'val3'])
								</span>
							</td>
						</tr>
						<tr>
							<td class="td-mw-108 align-middle">職制基準日（条件選択）：</td>
							<td>
								<input type="text" name="val4" class="selectdate" maxlength="10" value="{{ old('val4', @$itemShow['val4']) }}"/>
							</td>
							<td class="p-0 align-middle">
								<span class="col-xs-1 p-1">
								@include('layouts/error/item', ['name' => 'val4'])
								</span>
							</td>
						</tr>
					</table>
				</div>
			</div>
			<div class="row ml-1">
				<div class="col-xs-12">
					<table class="table table-borderless mb-0">
						<tr>
							<td id="checkbox_val5">
								
							</td>
							<td class="p-0 align-middle">
								<span class="col-xs-1 p-1">
								@include('layouts/error/item', ['name' => 'val5'])
								</span>
							</td>
							<td id="checkbox_val6">
								
							</td>
							<td class="p-0 align-middle">
								<span class="col-xs-1 p-1">
								@include('layouts/error/item', ['name' => 'val6'])
								</span>
							</td>
						</tr>
					</table>
				</div>
			</div>
		</form>
		<div class="row">
			<div class="col-sm-12">
				<div class="col-xs-1 p-1">
					<button type="button" id="save" class="{{ config('system_const.btn_color_ok') }}"><i class="{{ config('system_const.btn_img_ok') }}"></i>{{ config('system_const.btn_char_ok') }}</button>
				</div>
			</div>
		</div>
	</div>
</div>
@endsection