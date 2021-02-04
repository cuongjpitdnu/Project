@extends('layouts/mainmenu/menu')
@section('content')
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();

		const dataVal2 = fncJsonParse('{{ json_encode($dataView['data_2_all']) }}');
		const dataVal3 = fncJsonParse('{{ json_encode($dataView['data_3_all']) }}');

		$('[name=val1]').on('change', function(e) {
			bindingSelect('val2', dataVal2, $(this).val(), '');
			bindingSelect('val3', dataVal3, $(this).val(), $('[name=val2]').val());
		});

		$('[name=val2]').on('change', function(e) {
			bindingSelect('val3', dataVal3, $('[name=val1]:checked').val(), $(this).val());
		});

		$('#output').on('click', function(e) {
			$('#area-error, table tr td:nth-child(2) div:last-child').html('');
			$('#indicator').trigger('click');
			$('[name=val4]').val(0);
			if($('[name=val4-view]').is(':checked')) {
				$('[name=val4]').val(1);
			}

			setCookie('export', 0, {{ config('system_const.timeout_time') }});
			$('#mainform').submit();
			let interval = setInterval(function() {
				let checkCookie = getCookie("export");
				if(checkCookie == "") {
					indicatorHide();
					clearInterval(interval);
					var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/';
					url += 'index?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
					url += '&val1='+$('[name=val1]:checked').val();
					url += '&val2='+$('[name=val2]').val();
					url += '&val3='+$('[name=val3]').val();
					url += '&val4='+$('[name=val4]').val();
					url += '&err1={{ $msgTimeOut }}';
					window.location.href = url;
				}
				if(checkCookie == 1) {
					indicatorHide();
					clearInterval(interval);
				}
			}, 100);
		});
	});

	function bindingSelect(name_input, data, ckind_filter, project_filter) {
		$('#indicator').trigger('click');
		if(['val2', 'val3'].indexOf(name_input) > -1) {
			let arrUnique = [];
			$('[name='+name_input+']').empty();
			if(data.length > 0) {
				let flagHasValue = false;
				if(name_input == 'val3') {
					$.each(data, function(i, e) {
						if(ckind_filter === e.CKind && project_filter === e.ProjectID) {
							if(arrUnique.length === 0) {
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
				} else {
					$.each(data, function(i, e) {
						if(ckind_filter === e.ListKind) {
							if(arrUnique.length === 0) {
								flagHasValue = true;
								$('[name=val2]').append(`<option value="${e.val2}">${convertHTML(e.val2Name)}</option>`);
								arrUnique.push(e.val2Name);
							} else {
								if(arrUnique.indexOf(e.val2Name) === -1) {
									flagHasValue = true;
									$('[name=val2]').append(`<option value="${e.val2}">${convertHTML(e.val2Name)}</option>`);
									arrUnique.push(e.val2Name);
								}
							}
						}
					});
				}
				if(!flagHasValue) { $('[name='+name_input+']').append('<option value=""></option>'); }
			} else {
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
				■　中日程表出力
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

		<div class="row ml-1">
			<div class="col-xs-12">
				<form action="{{ url('/') }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/output" method="POST" id="mainform">
					@csrf
					<input type="hidden" id="kindid" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}" />
					<input type="hidden" id="menuid" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}" />

					<div class="row">
						<div class="col-xs-12">
							<table class="table table-borderless mb-0">
								<tr>
									<td class="align-middle">中日程区分：</td>
									<td>
										<label class="mb-0" for="rdo1"><input type="radio" id="rdo1" name="val1"
											value="{{ valueUrlEncode(config('system_const.c_kind_chijyo')) }}"
											{{ old('val1', @$itemShow['val1']) === valueUrlEncode(config('system_const.c_kind_chijyo')) ?
											'checked' : '' }}> 地上中日程</label> /
										<label class="mb-0" for="rdo2"><input type="radio" id="rdo2" name="val1"
											value="{{ valueUrlEncode(config('system_const.c_kind_gaigyo')) }}"
											{{ old('val1', @$itemShow['val1']) === valueUrlEncode(config('system_const.c_kind_gaigyo')) ?
											'checked' : '' }}> 外業中日程</label> /
										<label class="mb-0" for="rdo3"><input type="radio" id="rdo3" name="val1"
											value="{{ valueUrlEncode(config('system_const.c_kind_giso')) }}"
											{{ old('val1', @$itemShow['val1']) === valueUrlEncode(config('system_const.c_kind_giso')) ?
											'checked' : '' }}> 艤装中日程</label>
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
					<div class="row">
						<div class="col-xs-12">
							<table class="table table-borderless">
								<tr>
									<td class="align-middle">検討ケース：</td>
									<td>
										<select name="val2" id="">
											@if(count($dataView['data_2']) > 0)
												@foreach ($dataView['data_2'] as $item)
													<option value={{ $item->val2 }}
														{{ trim(old('val2', @$itemShow['val2'])) === trim($item->val2) ? 'selected': '' }}>{{
															$item->val2Name }}</option>
												@endforeach
											@else
												<option value=""></option>
											@endif
										</select>
									</td>
									<td class="p-0 align-middle">
										<span class="col-xs-1 p-1">
											@include('layouts/error/item', ['name' => 'val2'])
										</span>
									</td>
								</tr>
								<tr>
									<td class="align-middle">オーダ：</td>
									<td>
										<select name="val3" id="">
											@if(count($dataView['data_3']) > 0)
												@foreach ($dataView['data_3'] as $item)
													<option value={{ $item->val3 }}
														{{ trim(old('val3', @$itemShow['val3'])) === trim($item->val3) ? 'selected': '' }}>{{ $item->val3Name }}</option>
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
								@if (!$menuInfo->IsReadOnly)
								<tr>
									<td></td>
									<td>
										<label for="val4-view">
											<input type="checkbox" name="val4-view" id="val4-view"
												{{ (int)old('val4', @$itemShow['val4']) === 1 ? 'checked' : '' }}
											> 正式発行
										</label>
										<input type="hidden" name="val4" value="" />
									</td>
									<td class="p-0 align-middle">
										<span class="col-xs-1 p-1">
											@include('layouts/error/item', ['name' => 'val4'])
										</span>
									</td>
								</tr>
								@endif
							</table>
						</div>
					</div>
				</form>
			</div>
		</div>

		<div class="row">
			<div class="col-xs-1 p-1">
				<button type="button" id="output" class="{{ config('system_const.btn_color_output') }}">
					<i class="{{ config('system_const.btn_img_output') }}"></i>{{ config('system_const.btn_char_output') }}
				</button>
			</div>
		</div>
	</div>
</div>
@endsection