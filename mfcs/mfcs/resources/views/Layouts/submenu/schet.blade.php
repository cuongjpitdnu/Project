@extends('layouts/mainmenu/menu')

@section('content')

<script>

$(function(){
	$('[name="pageunit"]').change(function(e){
		$('#indicator').trigger('click');
		var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}';
		url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}';
		url += '&page=1&pageunit=' + $(this).val();
		url += '&sort={{ $request->sort }}';
		url += '&direction={{ $request->direction }}';
		window.location.href = url;
	});

	$('.show').on('click', function(){
		var val1 = $(this).val();
		$('#indicator').trigger('click');
		var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/index';
		url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}';
		url += '&val1=' + val1;
		url += '&val2={{ isset($request->page) ? $request->page : 1 }}';
		url += '&val3={{ isset($request->pageunit) ? $request->pageunit : config('system_const.displayed_results_1') }}';
		url += '&val4={{ $request->sort }}';
		url += '&val5={{ $request->direction }}';
		window.location.href = url;
	});

})

</script>

<div class="row align-items-center">
	<div class="col-xs-1 text-left m-2 p-2 rounded border">
		■　お知らせ　■
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

<div class="row pl-2">
	<div class="col-xs-12">
		@if (count($listDatas) == 0)
			表示するお知らせが有りません。
		@else
			<table class="table text-center">
				<tr>
					<th>@sortablelink('fld2', '取込日')</th>
					<th>@sortablelink('fld4', '検討ケース')</th>
					<th>@sortablelink('fld5', 'オーダ')</th>
					<th>@sortablelink('fld6', '他日程との連携')</th>
					<th>@sortablelink('fld7', '内容')</th>
					<th class="align-center"></th>
				</tr>
				@foreach($listDatas as $listData)
				<tr>
					<td>{{ date('Y/m/d', strtotime($listData->fld2)) }}</td>
					<td class="text-left">{{ $listData->fld4 }}</td>
					<td class="text-left">{{ $listData->fld5 }}</td>
					@switch($listData->fld6)
						@case(1)
							<td class="text-left">{{ '連携' }}</td>
							@break
						@default
							<td class="text-left"></td>
					@endswitch
					@switch($listData->fld7)
						@case(config('system_const_schet.schet_import_status_error'))
							<td class="text-left">{{ config('message.msg_notice_schet_003') }}</td>
							@break
						@case(config('system_const_schet.schet_import_status_running'))
							<td class="text-left">{{ config('message.msg_notice_schet_002') }}</td>
							@break
						@case(config('system_const_schet.schet_import_status_done'))
							<td class="text-left">{{ config('message.msg_notice_schet_001') }}</td>
							@break
						@default
							<td class="text-left"></td>
					@endswitch
					<td class="align-center">
						<button type="button" class="show {{ config('system_const.btn_color_rowdetail') }}" 
							value="{{ valueUrlEncode($listData->fld1) }}">
							@if (config('system_const.btn_img_rowdetail')!='')
							<i class="{{ config('system_const.btn_img_rowdetail') }}"></i>
							@endif
							{{ config('system_const.btn_char_rowdetail') }}
						</button>
					</td>
				</tr>
				@endforeach
			</table>
			{{ $listDatas->appends(request()->query())->links() }}
			
			<tr>
				<td>表示件数：</td>
				<td>
					<select name="pageunit" class="pageunit-width">
						<option value= {{ config('system_const.displayed_results_1') }} {{ \Request::input('pageunit') == config('system_const.displayed_results_1') ? 'selected' : '' }}> {{ config('system_const.displayed_results_1') }} </option>
						<option value= {{ config('system_const.displayed_results_2') }} {{ \Request::input('pageunit') == config('system_const.displayed_results_2') ? 'selected' : '' }}> {{ config('system_const.displayed_results_2') }} </option>
						<option value= {{ config('system_const.displayed_results_3') }} {{ \Request::input('pageunit') == config('system_const.displayed_results_3') ? 'selected' : '' }}> {{ config('system_const.displayed_results_3') }} </option>
					</select>
				</td>
			</tr>

		@endif
	</div>
</div>

@endsection
