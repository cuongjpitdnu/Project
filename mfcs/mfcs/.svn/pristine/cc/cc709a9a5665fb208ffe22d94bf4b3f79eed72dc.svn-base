@extends('layouts/mainmenu/menu')
@section('content')
<script>
	$(function() {
		$('#ok').on('click', function(e) {
			$('#indicator').trigger('click');
			let valueChecked = $('input[name=val1]:checked').val();
			let page = (valueChecked == 0) ? 'create' : ((valueChecked == 1) ? 'copy' : ((valueChecked == 2) ? 'delete' : 'apply'));
			let url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/'+page;
			url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			window.location.href = url;
		});
	});
</script>

<div class="row ml-2 mr-2">
	<div class="col-md-12 col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　検討ケース作成
			</div>
		</div>
		<div class="row head-purple">
			<div class="col-xs-12">条件選択</div>
		</div>
		<div class="row ml-3 mt-3">
			<div class="col-xs-12">
				<table id="tbl-rdo">
					<tbody>
						<tr>
							<td>
								<label for="rdo-create" class="mb-3">
									<input type="radio" name="val1" id="rdo-create" value="0" checked /> 空の検討ケースを作成
								</label>
							</td>
						</tr>
						<tr>
							<td>
								<label for="rdo-copy" class="mb-3">
									<input type="radio" name="val1" id="rdo-copy" value="1" /> 既存の検討ケースからコピー
								</label>
							</td>
						</tr>
						<tr>
							<td>
								<label for="rdo-delete" class="mb-3">
									<input type="radio" name="val1" id="rdo-delete" value="2" /> 検討ケースを削除
								</label>
							</td>
						</tr>
						<tr>
							<td>
								<label for="rdo-apply" class="mb-3">
									<input type="radio" name="val1" id="rdo-apply" value="3" /> 本番に適用
								</label>
							</td>
						</tr>
					</tbody>
				</table>
			</div>
		</div>
		<div class="row ml-1">
			<div class="col-xs-1 p-1 ml-2">
				<button type="button" id="ok" class="{{ config('system_const.btn_color_ok') }}">
					<i class="{{ config('system_const.btn_img_ok') }}"></i>{{ config('system_const.btn_char_ok') }}</button>
			</div>
		</div>
	</div>
</div>
@endsection