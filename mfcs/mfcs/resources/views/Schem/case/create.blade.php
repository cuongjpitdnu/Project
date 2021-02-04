@extends('layouts/mainmenu/menu')
@section('content')
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();

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
	});
</script>

<div class="row ml-2 mr-2">
	<div class="col-md-12 col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　検討ケース作成（空の検討ケース作成）
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
		<div class="row ml-1">
			<div class="col-xs-12">
				<form action="{{ url('/') }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/newsave" method="POST" id="mainform">
					@csrf
					<input type="hidden" id="kindid" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}" />
					<input type="hidden" id="menuid" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}" />
					<div class="row mt-3">
						<div class="col-xs-12">
							<table class="table table-borderless mb-0 ml-2">
								<tbody>
									<tr>
										<td class="td-mw-108 align-middle">中日程区分：</td>
										<td>
											<label class="mb-0" for="rdo1">
												<input type="radio" id="rdo1" name="val101" value="{{
												valueUrlEncode(config('system_const.c_kind_chijyo')) }}"
												{{ ((int)old('val101', @$itemData['ListKind']) === config('system_const.c_kind_chijyo')) ? 'checked' : '' }}> {{
												config('system_const.c_name_chijyo') }}</label> /
											<label class="mb-0" for="rdo2">
												<input type="radio" id="rdo2" name="val101" value="{{
												valueUrlEncode(config('system_const.c_kind_gaigyo')) }}"
												{{ ((int)old('val101', @$itemData['ListKind']) === config('system_const.c_kind_gaigyo')) ? 'checked' : '' }}> {{
												config('system_const.c_name_gaigyo') }}</label> /
											<label class="mb-0" for="rdo3">
												<input type="radio" id="rdo3" name="val101" value="{{
												valueUrlEncode(config('system_const.c_kind_giso')) }}"
												{{ ((int)old('val101', @$itemData['ListKind']) === config('system_const.c_kind_giso')) ? 'checked' : '' }}> {{
												config('system_const.c_name_giso') }}</label>
										</td>
										<td class="p-0 align-middle">
											<span class="col-xs-1 p-1">
												@include('layouts/error/item', ['name' => 'val101'])
											</span>
										</td>
									</tr>
								</tbody>
							</table>
						</div>
					</div>
					<div class="row">
						<div class="col-xs-12">
							<table class="table table-borderless ml-2">
								<tbody>
									<tr>
										<td class="td-mw-108 align-middle">検討ケース：</td>
										<td>
											<input type="text" name="val102" value="{{ old('val102', @$itemData['ProjectName']) }}" maxlength="50" autocomplete="off">
										</td>
										<td class="p-0 align-middle">
											<span class="col-xs-1 p-1">
												@include('layouts/error/item', ['name' => 'val102'])
											</span>
										</td>
									</tr>
								</tbody>
							</table>
						</div>
					</div>

				</form>
			</div>
		</div>
		<div class="row ml-1">
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