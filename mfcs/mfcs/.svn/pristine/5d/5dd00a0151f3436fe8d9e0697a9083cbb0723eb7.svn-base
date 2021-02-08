@extends('layouts/mainmenu/menu')
@section('content')
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();

		const dataVal302 = fncJsonParse('{{ json_encode($dataSelect['val302LoadAll']) }}');
		const dataVal303 = fncJsonParse('{{ json_encode($dataSelect['val303LoadAll']) }}');

		$('#save').on('click', function(e) {
			$('#indicator').trigger('click');
			$('#mainform').submit();
		});

		$('#cancel').on('click', function(e) {
			$('#indicator').trigger('click');
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/';
			url += 'index?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			window.location.href = url;
		});

		$('[name=val301]').on('change', function(e) {
			bindingData('val302', dataVal302, $(this).val(), '');
			bindingData('val303', dataVal303, $(this).val(), $('[name=val302]').val());
		});

		$('[name=val302]').on('change', function(e) {
			bindingData('val303', dataVal303, $('[name=val301]:checked').val(), $(this).val());
		});
	});

	function bindingData(name_input, data, ckind_filter, project_filter) {
		$('#indicator').trigger('click');
		if(['val302', 'val303'].indexOf(name_input) > -1) {
			let arrUnique = [];
			$('[name='+name_input+']').empty();
			if(data.length > 0) {
				let flagHasValue = false;
				if(name_input === 'val302') {
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
				if(name_input === 'val303') {
					$('[name='+name_input+']').append('<option value=""></option>');
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
				if(!flagHasValue) {
					if(name_input != 'val303') {
						$('[name='+name_input+']').append('<option value=""></option>');
					}
				}
			} else {
				if(name_input != 'val303') {
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
				■　検討ケース作成（検討ケース削除）
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
			<div class="col-xs-12">条件選択</div>
		</div>
		<form action="{{ url('/') }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/deletesave" method="POST" id="mainform">
			@csrf
			<input type="hidden" id="kindid" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}" />
			<input type="hidden" id="menuid" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}" />
			<div class="row mt-3">
				<div class="col-xs-12">
					<div class="row ml-3">
						<div class="col-xs-12">
							<table class="table table-borderless mb-0">
								<tbody>
									<tr>
										<td class="align-middle">中日程区分：</td>
										<td>
											<label class="mb-0" for="rdo1">
												<input type="radio" id="rdo1" name="val301" value="{{
												valueUrlEncode(config('system_const.c_kind_chijyo')) }}"
												{{ (old('val301', @$itemData['val301']) === valueUrlEncode(config('system_const.c_kind_chijyo'))) ? 'checked' : '' }}> {{
												config('system_const.c_name_chijyo') }}</label> /
											<label class="mb-0" for="rdo2">
												<input type="radio" id="rdo2" name="val301" value="{{
												valueUrlEncode(config('system_const.c_kind_gaigyo')) }}"
												{{ (old('val301', @$itemData['val301']) === valueUrlEncode(config('system_const.c_kind_gaigyo'))) ? 'checked' : '' }}> {{
												config('system_const.c_name_gaigyo') }}</label> /
											<label class="mb-0" for="rdo3">
												<input type="radio" id="rdo3" name="val301" value="{{
												valueUrlEncode(config('system_const.c_kind_giso')) }}"
												{{ (old('val301', @$itemData['val301']) === valueUrlEncode(config('system_const.c_kind_giso'))) ? 'checked' : '' }}> {{
												config('system_const.c_name_giso') }}</label>
										</td>
										<td class="p-0 align-middle">
											<span class="col-xs-1 p-1">
												@include('layouts/error/item', ['name' => 'val301'])
											</span>
										</td>
									</tr>
								</tbody>
							</table>
						</div>
					</div>
					<div class="row ml-3">
						<div class="col-xs-12">
							<table class="table table-borderless mb-0">
								<tbody>
									<tr>
										<td class="align-middle">検討ケース：</td>
										<td>
											<select name="val302" id="">
												@if(count($dataSelect['val302']) > 0)
													@foreach ($dataSelect['val302'] as $item)
														<option value={{ $item->ID }}
															{{ trim(old('val302', @$itemData['val302'])) === trim($item->ID) ? 'selected': '' }}>{{ $item->ProjectName }}</option>
													@endforeach
												@else
													<option value=""></option>
												@endif
											</select>
										</td>
										<td class="p-0 align-middle">
											<span class="col-xs-1 p-1">
												@include('layouts/error/item', ['name' => 'val302'])
											</span>
										</td>
										<td class="align-middle">オーダ：</td>
										<td>
											<select name="val303" id="">
												<option value=""></option>
												@if(count($dataSelect['val303']) > 0)
													@foreach ($dataSelect['val303'] as $item)
														<option value={{ $item->OrderNo }}
															{{ trim(old('val303', @$itemData['val303'])) === trim($item->OrderNo) ? 'selected': '' }}>{{ $item->NameShow }}</option>
													@endforeach
												@endif
											</select>
										</td>
										<td class="p-0 align-middle">
											<span class="col-xs-1 p-1">
												@include('layouts/error/item', ['name' => 'val303'])
											</span>
										</td>
									</tr>
								</tbody>
							</table>
						</div>
					</div>
				</div>
			</div>
		</form>
		<div class="row ml-1 mt-3">
			<div class="col-xs-1 p-1 ml-1">
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