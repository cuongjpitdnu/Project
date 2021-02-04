@extends('layouts/mainmenu/menu')
@section('content')
<script>
	$(function() {
		$('select[name=val201]').on('change', function(e) {
			$('#indicator').trigger('click');
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/';
			url += 'pmanage?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&bpage={{ $request->bpage }}';
			url += '&bpageunit={{ $request->bpageunit }}';
			url += '&bsort={{ $request->bsort }}';
			url += '&page=1';
			url += '&sort={{ \Request::input('sort') ? \Request::input('sort') : '' }}';
			url += '&direction={{ \Request::input('direction') ? \Request::input('direction') : 'asc' }}';
			url += '&val1={{ $request->val1 }}';
			url += '&val2={{ $request->val2 }}';
			url += '&val101={{ $request->val101 }}';
			url += '&val102={{ $request->val102 }}';
			url += '&val103={{ $request->val103 }}';
			url += '&val104={{ $request->val104 }}';
			url += '&val201='+$(this).val();
			window.location.href = url;
		});

		$('.edit').on('click', function(e) {
			$('#indicator').trigger('click');
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/';
			url += 'pedit?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&bpage={{ $request->bpage }}';
			url += '&bpageunit={{ $request->bpageunit }}';
			url += '&bsort={{ $request->bsort }}';
			url += '&page={{ \Request::input('page') ? \Request::input('page') : 1 }}';
			url += '&sort={{ \Request::input('sort') ? \Request::input('sort') : '' }}';
			url += '&direction={{ \Request::input('direction') ? \Request::input('direction') : 'asc' }}';
			url += '&val1={{ $request->val1 }}';
			url += '&val2={{ $request->val2 }}';
			url += '&val101={{ $request->val101 }}';
			url += '&val102={{ $request->val102 }}';
			url += '&val103={{ $request->val103 }}';
			url += '&val104={{ $request->val104 }}';
			url += '&val201='+$('select[name=val201]').val();
			url += '&val202='+$(this).attr('no');
			window.location.href = url;
		});

		$('.show').on('click', function(e) {
			$('#indicator').trigger('click');
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/';
			url += 'pshow?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&bpage={{ $request->bpage }}';
			url += '&bpageunit={{ $request->bpageunit }}';
			url += '&bsort={{ $request->bsort }}';
			url += '&page={{ \Request::input('page') ? \Request::input('page') : 1 }}';
			url += '&sort={{ \Request::input('sort') ? \Request::input('sort') : '' }}';
			url += '&direction={{ \Request::input('direction') ? \Request::input('direction') : 'asc' }}';
			url += '&val1={{ $request->val1 }}';
			url += '&val2={{ $request->val2 }}';
			url += '&val101={{ $request->val101 }}';
			url += '&val102={{ $request->val102 }}';
			url += '&val103={{ $request->val103 }}';
			url += '&val104={{ $request->val104 }}';
			url += '&val201='+$('select[name=val201]').val();
			url += '&val202='+$(this).attr('no');
			window.location.href = url;
		});

		$('#cancel').on('click', function() {
			$('#indicator').trigger('click');
			let url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/';
			url += 'manage?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&page={{ $request->bpage }}';
			url += '&sort={{ $request->bsort }}';
			url += '&direction={{ $request->bdirection }}';
			url += '&val1={{ $request->val1 }}';
			url += '&val2={{ $request->val2 }}';
			url += '&val101={{ $request->val101 }}';
			url += '&val102={{ $request->val102 }}';
			window.location.href = url;
		});
	});
</script>

<div class="row ml-4">
	<div class="col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　項目定義(詳細)
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

		<div class="row align-items-center">
			<div class="col-xs-3 text-left m-2 p-2">{{
				($request->get('val1') === valueUrlEncode(config('system_const.c_kind_giso'))) ? '区画名' : 'ブロック名'
			}}：<span class="real-space">{{ $itemData['Name'] }}</span></div>
			<div class="col-xs-3 text-left m-2 p-2">組区：{{ $itemData['BKumiku'] }}</div>
		</div>

		<div class="row mt-2">
			<div class="col-xs-12">
				<table class="table ml-2" id="tbl-content">
					<thead>
						<tr>
							<th class="text-center">@sortablelink('KKumiku', '組区')</th>
							<th class="text-center">@sortablelink('Kotei', '工程')</th>
							<th class="text-center">@sortablelink('Floor', '施工棟')</th>
							<th class="text-center">@sortablelink('BD_Code', '物量コード')</th>
							<th class="text-center">@sortablelink('BData', '物量')</th>
							<th class="text-center">@sortablelink('HC', 'HC')</th>
							<th class="text-center">@sortablelink('Days', '工期')</th>
							<th class="text-center">@sortablelink('N_KKumiku', '次組区')</th>
							<th class="text-center">@sortablelink('N_Kotei', '次工程')</th>
							<th class="text-center">@sortablelink('Jyoban', '定盤')</th>
							<th class="text-center"></th>
						</tr>
					</thead>
					<tbody>
						@foreach($dataA3 as $row)
							<tr>
								<td class="align-middle real-space">{{ $row['KKumiku'] }}</td>
								<td class="align-middle real-space">{{ $row['Kotei'] }}</td>
								<td class="align-middle real-space">{{ $row['Floor'] }}</td>
								<td class="align-middle real-space">{{ $row['BD_Code'] }}</td>
								<td class="align-middle real-space text-right">{{ $row['BData'] }}</td>
								<td class="align-middle real-space text-right">{{ $row['HC'] }}</td>
								<td class="align-middle real-space text-right">{{ $row['Days'] }}</td>
								<td class="align-middle real-space">{{ $row['N_KKumiku'] }}</td>
								<td class="align-middle real-space">{{ $row['N_Kotei'] }}</td>
								<td class="align-middle real-space">{{ $row['Jyoban'] }}</td>
								<td class="align-middle text-center">
									@if (!$menuInfo->IsReadOnly)
										<button type="button" class="edit {{ config('system_const.btn_color_rowedit') }}"
											no="{{ valueUrlEncode($row['KoteiNo']) }}">
											@if (config('system_const.btn_img_rowedit') !== '')
												<i class="{{ config('system_const.btn_img_rowedit') }}"></i>
											@endif
											{{ config('system_const.btn_char_rowedit') }}</button>
									@else
										<button type="button" class="show {{ config('system_const.btn_color_rowinfo') }}"
											no="{{ valueUrlEncode($row['KoteiNo']) }}">
											@if (config('system_const.btn_img_rowinfo') !== '')
												<i class="{{ config('system_const.btn_img_rowinfo') }}"></i>
											@endif
											{{ config('system_const.btn_char_rowinfo') }}</button>
									@endif
								</td>
							</tr>
						@endforeach
					</tbody>
				</table>
				<div class="row ml-2 mr-4 float-left">
					<div class="col-xs-1">
						<button type="button" id="cancel" class="{{ config('system_const.btn_color_cancel') }}">
							<i class="{{ config('system_const.btn_img_cancel') }}"></i>{{ config('system_const.btn_char_cancel') }}
						</button>
					</div>
				</div>
				{{ $dataA3->appends(request()->query())->links() }}
				<div class="pageunit float-left">
					表示件数
					<select name="val201" class="pageunit-width">
						<option value="{{valueUrlEncode(config('system_const.displayed_results_1'))}}"
							{{ \Request::input('val201') === valueUrlEncode(config('system_const.displayed_results_1')) ? 'selected' : '' }}>{{
							config('system_const.displayed_results_1')
						}}</option>
						<option value="{{valueUrlEncode(config('system_const.displayed_results_2'))}}"
							{{ \Request::input('val201') === valueUrlEncode(config('system_const.displayed_results_2')) ? 'selected' : '' }}>{{
							config('system_const.displayed_results_2')
						}}</option>
						<option value="{{valueUrlEncode(config('system_const.displayed_results_3'))}}"
							{{ \Request::input('val201') === valueUrlEncode(config('system_const.displayed_results_3')) ? 'selected' : '' }}>{{
							config('system_const.displayed_results_3')
						}}</option>
					</select>
				</div>
			</div>
		</div>
	</div>
</div>
@endsection