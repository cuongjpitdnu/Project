@extends('layouts/mainmenu/menu')
@section('content')
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();

		const dataVal202 = fncJsonParse('{{ json_encode($dataSelect['val202LoadAll']) }}');
		const dataVal203 = fncJsonParse('{{ json_encode($dataSelect['val203LoadAll']) }}');
		const dataVal204 = fncJsonParse('{{ json_encode($dataSelect['val204LoadAll']) }}');
		const dataVal205 = fncJsonParse('{{ json_encode($dataSelect['val205LoadAll']) }}');

		$('#cancel').on('click', function(e) {
			$('#indicator').trigger('click');
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/';
			url += 'index?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			window.location.href = url;
		});

		$('[name=val201]').on('change', function(e) {
			bindingData('val202', dataVal202, $(this).val(), '');
			bindingData('val203', dataVal203, $(this).val(), $('[name=val202]').val());
			bindingData('val204', dataVal204, $(this).val(), '');
		});

		$('[name=val202]').on('change', function(e) {
			bindingData('val203', dataVal203, $('[name=val201]:checked').val(), $(this).val());
		});

		$('#save').on('click', function(e) {
			if (confirm("{{ config('message.msg_schem_case_005') }}")) {
				$('#indicator').trigger('click');
				$('#mainform').submit();
			}
		});
	});

	function bindingData(name_input, data, ckind_filter, project_filter) {
		$('#indicator').trigger('click');
		if(['val202', 'val203', 'val204', 'val205'].indexOf(name_input) > -1) {
			let arrUnique = [];
			$('[name='+name_input+']').empty();
			if(name_input === 'val202') {
				$('[name='+name_input+']').append('<option value="{{ valueUrlEncode(0) }}"></option>');
			}
			if(data.length > 0) {
				let flagHasValue = false;
				if(name_input === 'val202' || name_input === 'val204') {
					$.each(data, function(i, e) {
						if(ckind_filter === e.ListKind) {
							if(arrUnique.length === 0) {
								flagHasValue = true;
								$('[name='+name_input+']').append(`<option value="${e.ID}" selected>${convertHTML(e.ProjectName)}</option>`);
								arrUnique.push(e.ProjectName);
							} else {
								if(arrUnique.indexOf(e.ProjectName) === -1) {
									flagHasValue = true;
									$('[name='+name_input+']').append(`<option value="${e.ID}">${convertHTML(e.ProjectName)}</option>`);
									arrUnique.push(e.ProjectName);
								}
							}
						}
					});
				}
				if(name_input === 'val203') {
					$.each(data, function(i, e) {
						if(ckind_filter === e.CKind && project_filter === e.ProjectID) {
							if(arrUnique.length === 0) {
								flagHasValue = true;
								$('[name='+name_input+']').append(`<option value="${e.OrderNo}">${convertHTML(e.NameShow)}</option>`);
								arrUnique.push(e.NameShow);
							} else {
								if(arrUnique.indexOf(e.NameShow) === -1) {
									flagHasValue = true;
									$('[name='+name_input+']').append(`<option value="${e.OrderNo}">${convertHTML(e.NameShow)}</option>`);
									arrUnique.push(e.NameShow);
								}
							}
						}
					});
				}
				if(name_input === 'val205') {
					$.each(data, function(i, e) {
						if(ckind_filter === e.CKind) {
							if(arrUnique.length === 0) {
								flagHasValue = true;
								$('[name='+name_input+']').append(`<option value="${e.OrderNo}">${convertHTML(e.NameShow)}</option>`);
								arrUnique.push(e.NameShow);
							} else {
								if(arrUnique.indexOf(e.NameShow) === -1) {
									flagHasValue = true;
									$('[name='+name_input+']').append(`<option value="${e.OrderNo}">${convertHTML(e.NameShow)}</option>`);
									arrUnique.push(e.NameShow);
								}
							}
						}
					});
				}
				if(!flagHasValue) {
					if(name_input != 'val202') {
						$('[name='+name_input+']').append('<option value=""></option>');
					}
				}
			} else {
				if(name_input != 'val202') {
					$('[name='+name_input+']').append('<option value=""></option>');
				}
			}
		}
		indicatorHide();
	}
</script>

<div class="row ml-2 mr-2">
	<div class="col-md-12 col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　検討ケース作成（コピー）
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

		<form action="{{ url('/') }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/copysave" method="POST" id="mainform">
			@csrf
			<input type="hidden" id="kindid" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}" />
			<input type="hidden" id="menuid" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}" />
			<div class="row head-purple">
				<div class="col-xs-12">条件選択</div>
			</div>

			<div class="row mt-3">
				<div class="col-xs-12">
					<table class="table table-borderless ml-3 mb-0">
						<tbody>
							<tr>
								<td class="align-middle">中日程区分：</td>
								<td>
									<label class="mb-0" for="rdo1">
										<input type="radio" id="rdo1" name="val201" value="{{
										valueUrlEncode(config('system_const.c_kind_chijyo')) }}"
										{{ (old('val201', @$itemData['val201']) === valueUrlEncode(config('system_const.c_kind_chijyo'))) ? 'checked' : '' }}> {{
										config('system_const.c_name_chijyo') }}</label> /
									<label class="mb-0" for="rdo2">
										<input type="radio" id="rdo2" name="val201" value="{{
										valueUrlEncode(config('system_const.c_kind_gaigyo')) }}"
										{{ (old('val201', @$itemData['val201']) === valueUrlEncode(config('system_const.c_kind_gaigyo'))) ? 'checked' : '' }}> {{
										config('system_const.c_name_gaigyo') }}</label> /
									<label class="mb-0" for="rdo3">
										<input type="radio" id="rdo3" name="val201" value="{{
										valueUrlEncode(config('system_const.c_kind_giso')) }}"
										{{ (old('val201', @$itemData['val201']) === valueUrlEncode(config('system_const.c_kind_giso'))) ? 'checked' : '' }}> {{
										config('system_const.c_name_giso') }}</label>
								</td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val201'])
									</span>
								</td>
							</tr>
						</tbody>
					</table>
				</div>
			</div>
			<div class="row">
				<div class="col-xs-12">
					<table class="table table-borderless ml-3">
						<tbody>
							<tr>
								<td class="align-middle">コピー元</td>
								<td class="align-middle">検討ケース：</td>
								<td>
									<select name="val202" id="">
										<option value="{{ valueUrlEncode(0) }}"></option>
										@if(count($dataSelect['val202']) > 0)
											@foreach ($dataSelect['val202'] as $item)
												<option value={{ $item->ID }}
													{{ trim(old('val202', @$itemData['val202'])) === trim($item->ID) ? 'selected': '' }}>{{ $item->ProjectName }}</option>
											@endforeach
										@endif
									</select>
								</td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val202'])
									</span>
								</td>
								<td class="td-mw-108 align-middle">オーダ：</td>
								<td>
									<select name="val203" id="">
										@if(count($dataSelect['val203']) > 0)
											@foreach ($dataSelect['val203'] as $item)
												<option value={{ $item->OrderNo }}
													{{ trim(old('val203', @$itemData['val203'])) === trim($item->OrderNo) ? 'selected': '' }}>{{ $item->NameShow }}</option>
											@endforeach
										@else
											<option value=""></option>
										@endif
									</select>
								</td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val203'])
									</span>
								</td>
							</tr>
							<tr>
								<td class="align-middle">コピー先</td>
								<td class="align-middle">検討ケース：</td>
								<td>
									<select name="val204" id="">
										@if(count($dataSelect['val204']) > 0)
											@foreach ($dataSelect['val204'] as $item)
												<option value={{ $item->ID }}
													{{ trim(old('val204', @$itemData['val204'])) === trim($item->ID) ? 'selected': '' }}>{{ $item->ProjectName }}</option>
											@endforeach
										@else
											<option value=""></option>
										@endif
									</select>
								</td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val204'])
									</span>
								</td>
								<td class="td-mw-108 align-middle">オーダ：</td>
								<td>
									<select name="val205" id="">
										@if(count($dataSelect['val205']) > 0)
											@foreach ($dataSelect['val205'] as $item)
												<option value={{ $item->OrderNo }}
													{{ trim(old('val205', @$itemData['val205'])) === trim($item->OrderNo) ? 'selected': '' }}>{{ $item->NameShow }}</option>
											@endforeach
										@else
											<option value=""></option>
										@endif
									</select>
								</td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val205'])
									</span>
								</td>
							</tr>
						</tbody>
					</table>
				</div>
			</div>
			<div class="row head-purple">
				<div class="col-xs-12">手番シフト</div>
			</div>
			<div class="row mt-3">
				<div class="col-xs-12">
					<table class="table table-borderless ml-3">
						<tbody>
							<tr>
								{{-- <td class="align-middle">シフト：</td> --}}
								<td class="align-middle">シフトする手番を入力：</td>	{{-- update rev6 --}}
								<td>
									<input type="text" name="val206" value="{{ old('val206', @$itemData['val206']) }}" maxlength="6" autocomplete="off" />
								</td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val206'])
									</span>
								</td>
								{{-- <td class="align-middle">手番シフトする</td> --}} {{-- update rev6 --}}
							</tr>
						</tbody>
					</table>
				</div>
			</div>
		</form>
		<div class="row ml-2">
			<div class="col-xs-1 p-1">
				<button type="button" id="save" class="{{ config('system_const.btn_color_save') }}">
					<i class="{{ config('system_const.btn_img_save') }}"></i>{{ config('system_const.btn_char_save') }}
				</button>
			</div>
			<div class="col-xs-1 p-1">
				<button type="button" id="cancel" class="{{ config('system_const.btn_color_cancel') }}">
					<i class="{{ config('system_const.btn_img_cancel') }}"></i>{{ config('system_const.btn_char_cancel') }}
				</button>
			</div>
		</div>
	</div>
</div>
@endsection