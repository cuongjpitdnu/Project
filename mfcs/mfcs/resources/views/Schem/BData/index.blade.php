@extends('layouts/mainmenu/menu')
@section('content')
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();
		const dataVal3 = fncJsonParse('{{ json_encode($dataView['data_3_all']) }}');
		const dataVal4 = fncJsonParse('{{ json_encode($dataView['data_4_all']) }}');

		$('[name=val1]').on('change', function(e) {
			bindingSelect('val3', dataVal3, $(this).val(), '');
			bindingSelect('val4', dataVal4, $(this).val(), $('[name=val3]').val());
		});

		$('[name=val3]').on('change', function(e) {
			bindingSelect('val4', dataVal4, $('[name=val1]:checked').val(), $(this).val());
		});

		// set disabled button,selector
		$('input:radio[name="val2"]').change(function() {
			var value = $("input:radio[name=val2]:checked").val();
			//export
			if (value == '{{ valueUrlEncode(1) }}') {
				$('.input-checkbox').attr('disabled', true);
				$("#val6").val('');
				$(".val6").attr('disabled', true);
				$("[name=val8]").attr('disabled', true);
				$("#select").prop('disabled', true);
			}
			//import
			else if (value == '{{ valueUrlEncode(0) }}' ) {
				$('.input-checkbox').attr('disabled', false);
				$(".val6").attr('disabled', false);
				$("[name=val8]").attr('disabled', false);
				$("#select").prop('disabled', false);
			} 
		});
		//button click file
		$('#select').click(function(){
			$('#val6').click();
		});

		$('#val6').change(function (e) {
			if(e.target.files.length == 0) {
				$('#filename').val("");
				return;
			}

			var fileName = e.target.files[0].name;
			$('#filename').val(fileName);
		});

		$('#save').on('click', function(e) {
			$('#indicator').trigger('click');
			var value = $("input:radio[name=val2]:checked").val();
			//export
			if (value == '{{ valueUrlEncode(1) }}') {
				setCookie('export', 0, {{ config('system_const.timeout_time') }});
			}
			$('#mainform').submit();
			//export
			if (value == '{{ valueUrlEncode(1) }}') {
				let interval = setInterval(function() {
					let checkCookie = getCookie("export");
					if(checkCookie == "") {
						indicatorHide();
						clearInterval(interval);
						var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/';
						url += 'index?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
						url += '&val1='+$('[name=val1]:checked').val();
						url += '&val2='+$('[name=val2]:checked').val();
						url += '&val3='+$('[name=val3]').val();
						url += '&val4='+$('[name=val4]').val();
						url += '&val5='+$('[name=val5]').val();
						url += '&val8='+$('[name=val8]').val();
						url += '&err1={{ $msgTimeOut }}';
						window.location.href = url;
					}
					if(checkCookie == 1) {
						indicatorHide();
						clearInterval(interval);
					}
				}, 100);
			}
		});

		$('.input-checkbox').click(function(){
			if($(this).prop('checked')){
				$('[name="'+$(this).attr('checkbox')+'"]').val(1);
			}else{
				$('[name="'+$(this).attr('checkbox')+'"]').val(0);
			}
		});
	});

	function bindingSelect(name_input, data, ckind_filter, project_filter) {
		$('#indicator').trigger('click');
		if(['val3', 'val4'].indexOf(name_input) > -1) {
			let arrUnique = [];
			$('[name='+name_input+']').empty();
			if(data.length > 0) {
				let flagHasValue = false;
				if(name_input == 'val3') {
					$.each(data, function(i, e) {
						if(ckind_filter == e.ListKind) {
							if(arrUnique.length == 0) {
								flagHasValue = true;
								$('[name=val3]').append(`<option value="${e.val3}">${convertHTML(e.val3Name)}</option>`);
								arrUnique.push(e.val3Name);
							} else {
								if(arrUnique.indexOf(e.val3Name) === -1) {
									flagHasValue = true;
									$('[name=val3]').append(`<option value="${e.val3}">${convertHTML(e.val3Name)}</option>`);
									arrUnique.push(e.val3Name);
								}
							}
						}
					});
				}
				else if(name_input == 'val4') {
					$.each(data, function(i, e) {
						if(ckind_filter == e.CKind && project_filter == e.ProjectID) {
							if(arrUnique.length == 0) {
								flagHasValue = true;
								$('[name=val4]').append(`<option value="${e.val4}">${convertHTML(e.val4Name)}</option>`);
								arrUnique.push(e.val4Name);
							} else {
								if(arrUnique.indexOf(e.val4Name) === -1) {
									flagHasValue = true;
									$('[name=val4]').append(`<option value="${e.val4}">${convertHTML(e.val4Name)}</option>`);
									arrUnique.push(e.val4Name);
								}
							}
						}
					});
				}
				if(!flagHasValue) { $('[name='+name_input+']').append('<option value=""></option>'); }
			}
			else 
			{
				$('[name='+name_input+']').append('<option value=""></option>');
			}
		}
		indicatorHide();
	}

	function setCookie(cname, cvalue, minute) {
		let d = new Date();
		let exdays = (minute*60)/(24*60*60);
		d.setTime(d.getTime() + (exdays*24*60*60*1000));
		let expires = "expires=" + d.toGMTString();
		document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
	}

	function getCookie(cname) {
		var name = cname + "=";
		var decodedCookie = decodeURIComponent(document.cookie);
		var ca = decodedCookie.split(';');
		for(var i = 0; i < ca.length; i++) {
			var c = ca[i];
			while (c.charAt(0) == ' ') {
				c = c.substring(1);
			}
			if (c.indexOf(name) == 0) {
				return c.substring(name.length, c.length);
			}
		}
		return "";
	}
</script>

<div class="row ml-2 mr-2">
	<div class="col-md-12 col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　物量定義
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
		<form action="{{ url('/') }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/execute" method="POST" id="mainform" enctype="multipart/form-data">
			@csrf
			<input type="hidden" id="kindid" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}" />
			<input type="hidden" id="menuid" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}" />
			<div class="row head-purple">
				<div class="col-xs-12">条件選択</div>
			</div>
			<div class="row ml-1 mt-3">
				<div class="col-xs-12">
					<table class="table table-borderless mb-0">
						<tr>
							<td class="td-mw-108 align-middle">中日程区分：</td>
							<td>
								<label for="rdo1"><input type="radio" id="rdo1" name="val1" value="{{ valueUrlEncode(config('system_const.c_kind_chijyo')) }}"
									{{ old('val1', @$itemShow['val1']) === valueUrlEncode(config('system_const.c_kind_chijyo')) ? 'checked' : '' }}> 
										{{ config('system_const.c_name_chijyo') }} </label> /
								<label for="rdo2"><input type="radio" id="rdo2" name="val1" value="{{ valueUrlEncode(config('system_const.c_kind_gaigyo')) }}"
									{{ old('val1', @$itemShow['val1']) === valueUrlEncode(config('system_const.c_kind_gaigyo')) ? 'checked' : '' }}>
										{{ config('system_const.c_name_gaigyo') }} </label> /
								<label for="rdo3"><input type="radio" id="rdo3" name="val1" value="{{ valueUrlEncode(config('system_const.c_kind_giso')) }}"
									{{ old('val1', @$itemShow['val1']) === valueUrlEncode(config('system_const.c_kind_giso')) ? 'checked' : '' }}>
										{{ config('system_const.c_name_giso') }} </label>
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

			<div class="row ml-1">
				<div class="col-xs-12">
					<table class="table table-borderless">
						<tr>
							<td class="td-mw-108 align-middle">機能選択：</td>
							<td>
								<label>
									<input type="radio"  name="val2" class="val2-import" 
									value="{{ valueUrlEncode(config('system_const_schem.bd_val_import')) }}" 
									{{ trim(old('val2', @$itemShow['val2'])) === valueUrlEncode(config('system_const_schem.bd_val_import')) ? 'checked' : '' }}> 
									Import
								</label> /
								<label>
									<input type="radio"  name="val2" class="val2-export" 
									value="{{ valueUrlEncode(config('system_const_schem.bd_val_export')) }}" 
									{{ trim(old('val2', @$itemShow['val2'])) === valueUrlEncode(config('system_const_schem.bd_val_export')) ? 'checked' : '' }}> 
									Export
								</label>
							</td>
							<td class="p-0 align-middle">
								<span class="col-xs-1 p-1">
								@include('layouts/error/item', ['name' => 'val3'])
								</span>
							</td>
						</tr>
						<tr>
							<td class="td-mw-108 align-middle">ケース：</td>
							<td>
								<select name="val3" id="" class="">
									@if(count($dataView['data_3']) > 0)
										@foreach ($dataView['data_3'] as $item)
											<option value={{ $item->val3 }}
												{{ trim(old('val3', @$itemShow['val3'])) === trim(($item->val3)) ? 'selected': '' }}>
												{{ $item->val3Name }}
											</option>
										@endforeach
									@else
										<option value=""></option>
									@endif
								</select>	
							</td>
							<td class="p-0 align-middle">
								<span class="col-xs-1 p-1">
								@include('layouts/error/item', ['name' => 'val3'])
								</span>
							</td>
						</tr>
						<tr>
							<td class="td-mw-108 align-middle">オーダ：</td>
							<td>
								<select name="val4" id="" class="">
									@if(count($dataView['data_4']) > 0)
										@foreach ($dataView['data_4'] as $item)
											<option value={{ $item->val4 }}
												{{ trim(old('val4', @$itemShow['val4'])) === trim($item->val4) ? 'selected': '' }}>
												{{ $item->val4Name }}
											</option>
										@endforeach
									@else
										<option value=""></option>
									@endif
								</select>	
							</td>
							<td class="p-0 align-middle">
								<span class="col-xs-1 p-1">
								@include('layouts/error/item', ['name' => 'val4'])
								</span>
							</td>
						</tr>
						<tr>
							<td class="td-mw-108 align-middle">ログ出力：</td>
							<td>
								<input type="checkbox" class="input-checkbox" checkbox="val5" 
								{{ old('val5', @$itemShow['val5']) === valueUrlEncode(1) ? 'checked' : '' }} 
								{{ trim(old('val2', @$itemShow['val2'])) === valueUrlEncode(config('system_const_schem.bd_val_export'))  ? 'disabled' : ''}}>
								<input type="hidden" name="val5" value="{{ old('val5', @$itemShow['val5']) ? 1 : 0 }}">
							</td>
							<td class="p-0 align-middle">
								<span class="col-xs-1 p-1">
								@include('layouts/error/item', ['name' => 'val5'])
								</span>
							</td>
						</tr>
					</table>
				</div>
			</div>
			<div class="row head-purple">
				<div class="col-xs-12">ファイル選択</div>
			</div>
			<div class="row ml-1 mt-3">
				<div class="col-xs-12">
					<table class="table table-borderless">
						@if (!$menuInfo->IsReadOnly)
							<tr>
								<td class="align-middle">
									<input type="file" name="val6" id="val6" class="val6 d-none" 
									{{ trim(old('val2', @$itemShow['val2'])) === valueUrlEncode(config('system_const_schem.bd_val_export')) ? 'disabled' : '' }}
									value="{{ @$itemShow->val6 }}" required="true">
									<input type="text" name="filename" id="filename" class="val6 input-file-width"
									{{ trim(old('val2', @$itemShow['val2'])) === valueUrlEncode(config('system_const_schem.bd_val_export')) ? 'disabled' : '' }}
									value= "{{ @$itemShow->filename }}" autocomplete="off" readonly />
								</td>

								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
									@include('layouts/error/item', ['name' => 'val6'])
									</span>
								</td>

								<td class="align-middle">
									<button type="button" name="select" id="select" 
									{{ trim(old('val2', @$itemShow['val2'])) === valueUrlEncode(config('system_const_schem.bd_val_export')) ? 'disabled' : '' }}
									class="{{ config('system_const.btn_color_file') }} val6">
										<i class="{{ config('system_const.btn_img_file') }}"></i>
										{{ config('system_const.btn_char_file') }}
									</button>
								</td>
							</tr>
						@endif
					</table>
				</div>
			</div>
			<div class="row head-purple">
				<div class="col-xs-12">ログ表示設定</div>
			</div>
			<div class="row ml-1 mt-3">
				<div class="col-xs-12">
					<table class="table table-borderless">
						@if (!$menuInfo->IsReadOnly)
							<tr>
								<td class="td-mw-108 align-middle">表示件数：</td>
								<td>
									<select name="val8" class="pageunit-width"
									{{ trim(old('val2', @$itemShow['val2'])) === valueUrlEncode(config('system_const_schem.bd_val_export')) ? 'disabled' : '' }}>
										<option value="{{ valueUrlEncode(config('system_const.displayed_results_1')) }}" 
											{{ trim(old('val8',@$itemShow->val8)) === trim(valueUrlEncode(config('system_const.displayed_results_1'))) ? 'selected' : '' }}>
											{{ config('system_const.displayed_results_1') }}
										</option>
										<option value="{{valueUrlEncode(config('system_const.displayed_results_2')) }}" 
											{{ trim(old('val8',@$itemShow->val8)) === trim(valueUrlEncode(config('system_const.displayed_results_2'))) ? 'selected' : '' }}>
											{{config('system_const.displayed_results_2')}}
										</option>
										<option value="{{ valueUrlEncode(config('system_const.displayed_results_3')) }}" 
											{{ trim(old('val8',@$itemShow->val8)) === trim(valueUrlEncode(config('system_const.displayed_results_3'))) ? 'selected' : '' }}>
											{{ config('system_const.displayed_results_3') }}
										</option>
									</select>
								</td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
									@include('layouts/error/item', ['name' => 'val8'])
									</span>
								</td>
								<td > ※1ページあたり </td>
							</tr>
						@endif
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