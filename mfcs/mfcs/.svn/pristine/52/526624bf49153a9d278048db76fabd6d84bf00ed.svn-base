@extends('layouts/mainmenu/menu')
@section('content')
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();

		$('#ok').on('click', function(e) {
			$('#indicator').trigger('click');
			let val1 = $('[name=val1]:checked').val();
			let val2 = $('[name=val2]').val();
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/manage';
			url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&val1='+val1;
			url += '&val2='+val2;

			window.location.href = url;
		});
	});
</script>

<div class="row ml-4">
	<div class="col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　項目定義
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

		<div class="row">
			<div class="col-xs-12">
				<table class="table table-borderless">
					<tbody>
						<tr>
							<td class="align-middle">中日程区分：</td>
							<td>
								<label class="mb-0" for="rdo1"><input type="radio" id="rdo1" name="val1" value="{{ valueUrlEncode(config('system_const.c_kind_chijyo')) }}"
									{{ (int)old('val1', @$itemShow['val1']) === config('system_const.c_kind_chijyo') ? 'checked' : '' }}> {{
										config('system_const.c_name_chijyo') }}</label> /
								<label class="mb-0" for="rdo2"><input type="radio" id="rdo2" name="val1" value="{{ valueUrlEncode(config('system_const.c_kind_gaigyo')) }}"
									{{ (int)old('val1', @$itemShow['val1']) === config('system_const.c_kind_gaigyo') ? 'checked' : '' }}> {{
										config('system_const.c_name_gaigyo') }}</label> /
								<label class="mb-0" for="rdo3"><input type="radio" id="rdo3" name="val1" value="{{ valueUrlEncode(config('system_const.c_kind_giso')) }}"
									{{ (int)old('val1', @$itemShow['val1']) === config('system_const.c_kind_giso') ? 'checked' : '' }}> {{
										config('system_const.c_name_giso') }}</label>
								@include('layouts/error/item', ['name' => 'val1'])
							</td>
						</tr>
						<tr>
							<td class="align-middle">表示件数：</td>
							<td>
								<select name="val2" class="pageunit-width">
									<option value="{{ valueUrlEncode(config('system_const.displayed_results_1')) }}"
										{{ (int)old('val2', @$itemShow['val2']) === config('system_const.displayed_results_1') ? 'selected' : '' }}>{{
										config('system_const.displayed_results_1')
									}}</option>
									<option value="{{ valueUrlEncode(config('system_const.displayed_results_2')) }}"
										{{ (int)old('val2', @$itemShow['val2']) === config('system_const.displayed_results_2') ? 'selected' : '' }}>{{
										config('system_const.displayed_results_2')
									}}</option>
									<option value="{{ valueUrlEncode(config('system_const.displayed_results_3')) }}"
										{{ (int)old('val2', @$itemShow['val2']) === config('system_const.displayed_results_3') ? 'selected' : '' }}>{{
										config('system_const.displayed_results_3')
									}}</option>
								</select> ※1ページあたり
								@include('layouts/error/item', ['name' => 'val2'])
							</td>
						</tr>
					</tbody>
				</table>
			</div>
		</div>

		<div class="row">
			<div class="col-xs-1 p-1">
				<button type="button" id="ok" class="{{ config('system_const.btn_color_ok') }}">
					<i class="{{ config('system_const.btn_img_ok') }}"></i>{{ config('system_const.btn_char_ok') }}</button>
			</div>
		</div>
	</div>
</div>
@endsection