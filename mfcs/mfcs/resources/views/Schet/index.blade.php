@extends('layouts/mainmenu/menu')

@section('content')

<script>

$(function(){
	$('[name="pageunit"]').change(function(e){
		$('#indicator').trigger('click');
		var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/index';
		url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}';
		url += '&page=1&pageunit=' + $(this).val();
		url += '&sort={{ $request->sort }}';
		url += '&direction={{ $request->direction }}';
		url += '&val1={{ $request->val1 }}';
		url += '&val2={{ $request->val2 }}';
		url += '&val3={{ $request->val3 }}';
		url += '&val4={{ $request->val4 }}';
		url += '&val5={{ $request->val5 }}';
		window.location.href = url;
	});

	$('.back').on('click', function(){
		$('#indicator').trigger('click');
		var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}';
		url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}';
		url += '&page={{ isset($request->val2) ? $request->val2 : 1 }}';
		url += '&pageunit={{ isset($request->val3) ? $request->val3 : config('system_const.displayed_results_1') }}';
		url += '&sort={{ $request->val4 }}';
		url += '&direction={{ $request->val5 }}';
		window.location.href = url;
	});

})

</script>

<div class="row align-items-center">
	<div class="col-xs-1 text-left m-2 p-2 rounded border">
		■　搭載日程取込ログ　■
	</div>
</div>

<div class="row pl-2">
	<div class="col-xs-12">
		<div class="col-xs-1 m-1">
			<button type="button" class="back {{ config('system_const.btn_color_back') }}" 
				value="">
				@if (config('system_const.btn_img_back')!='')
				<i class="{{ config('system_const.btn_img_back') }}"></i>
				@endif
				{{ config('system_const.btn_char_back') }}
			</button>
		</div>
		@if (count($listDatas) == 0)
			表示するログ情報が有りません。
		@else
			<table class="table text-center">
				<tr>
					<th>@sortablelink('fld1', 'カテゴリ')</th>
					<th>@sortablelink('fld2', 'ブロック名')</th>
					<th>@sortablelink('fld3', '組区')</th>
					<th>@sortablelink('fld4', 'ログ内容')</th>
				</tr>
				@foreach($listDatas as $listData)
				<tr>
					<td class="text-left">{{ $listData->fld1 }}</td>
					<td class="text-left">{{ $listData->fld2 }}</td>
					<td class="text-left">{{ $listData->fld3 }}</td>
					<td class="text-left">{{ $listData->fld4 }}</td>
				</tr>
				@endforeach
			</table>
			{{ $listDatas->appends(request()->query())->links() }}
			<div class="pageunit" style="float:left;">
				表示件数
				<select name="pageunit" class="pageunit-width">
					<option value= {{ config('system_const.displayed_results_1') }} {{ \Request::input('pageunit') == config('system_const.displayed_results_1') ? 'selected' : '' }}> {{ config('system_const.displayed_results_1') }} </option>
					<option value= {{ config('system_const.displayed_results_2') }} {{ \Request::input('pageunit') == config('system_const.displayed_results_2') ? 'selected' : '' }}> {{ config('system_const.displayed_results_2') }} </option>
					<option value= {{ config('system_const.displayed_results_3') }} {{ \Request::input('pageunit') == config('system_const.displayed_results_3') ? 'selected' : '' }}> {{ config('system_const.displayed_results_3') }} </option>
				</select>
			</div>
		@endif
	</div>
</div>

@endsection
