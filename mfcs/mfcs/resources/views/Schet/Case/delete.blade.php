@extends('layouts/mainmenu/menu')
@section('content')
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();

		const dataVal2 = fncJsonParse('{{ json_encode($dataView['data_3_all']) }}');

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

		$('[name=val1]').on('change', function(e) {
			$('#indicator').trigger('click');
			bindingVal2New(dataVal2, $(this).val());
		});
	});

	function bindingVal2New(data, target) {
		$('#indicator').trigger('click');
		let arrUnique = [];
		$('[name=val2]').empty().append('<option value=""></option>');
		if(data.length > 0) {
			$.each(data, function(i, e) {
				if(target === e.target) {
					if(arrUnique.length === 0) {
						$('[name=val2]').append(`<option value="${e.val2}">${convertHTML(e.val2Name)}</option>`);
						arrUnique.push(e.val2Name);
					} else {
						if(arrUnique.indexOf(e.val2Name) === -1) {
							$('[name=val2]').append(`<option value="${e.val2}">${convertHTML(e.val2Name)}</option>`);
							arrUnique.push(e.val2Name);
						}
					}
				}
			});
		}
		indicatorHide();
	}
</script>

<div class="row ml-2 mr-2">
	<div class="col-md-12 col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　検討ケース作成（データ削除）
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
				<form action="{{ url('/') }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/deletesave" method="POST" id="mainform">
					@csrf
					<input type="hidden" id="kindid" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}" />
					<input type="hidden" id="menuid" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}" />
					<table class="table table-borderless mt-3 mb-0">
						<tbody>
							<tr>
								<td class="align-middle">検討ケース：</td>
								<td>
									<select name="val1" id="">
										@if(count($dataView['data_1']) > 0)
											@foreach ($dataView['data_1'] as $item)
												<option value={{ $item->ID }}
													{{ trim(old('val1', @$itemShow['val1'])) === trim($item->ID) ? 'selected': '' }}>{{
														$item->ProjectName }}</option>
											@endforeach
										@endif
									</select>
								</td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val1'])
									</span>
								</td>
								<td class="align-middle">オーダ：</td>
								<td>
									<select name="val2" id="">
										<option value=""></option>
										@if(count($dataView['data_2']) > 0)
											@foreach ($dataView['data_2'] as $item)
												<option value={{ $item->val2 }}
													{{ trim(old('val2', @$itemShow['val2'])) === trim($item->val2) ? 'selected': '' }}>{{ $item->val2Name }}</option>
											@endforeach
										@endif
									</select>
								</td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val2'])
									</span>
								</td>
							</tr>
						</tbody>
					</table>
				</form>
			</div>
		</div>
		<div class="row ml-1 mt-3">
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