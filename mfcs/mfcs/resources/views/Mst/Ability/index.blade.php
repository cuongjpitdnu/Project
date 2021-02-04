@extends('layouts/mainmenu/menu')
@section('content')

<script>

$(function(){

	$('[name="pageunit"]').change(function(e){
		$('#indicator').trigger('click');
		var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/index';
		url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}';
		url += '&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
		url += '&page=1';
		url += '&pageunit=' + $(this).val();
		url += '&sort={{ @\Request::input('sort') }}';
		url += '&direction={{ @\Request::input('direction') }}';
		window.location.href = url;
	});

	$('#create').on('click', function(){
		$('#indicator').trigger('click');
		var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/create';
		url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}';
		url += '&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
		url += '&page={{ @$request->page }}';
		url += '&pageunit={{ @$request->pageunit }}';
		url += '&sort={{ @$request->sort }}';
		url += '&direction={{ @$request->direction }}';
		window.location.href = url;
	});

	$('[id=edit]').on('click', function(){
		var val1 = $(this).val();
		$('#indicator').trigger('click');
		var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/edit';
		url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}';
		url += '&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
		url += '&page={{ @$request->page }}';
		url += '&pageunit={{ @$request->pageunit }}';
		url += '&sort={{ @$request->sort }}';
		url += '&direction={{ @$request->direction }}';
		url += '&val1=' + val1;
		window.location.href = url;
	});

	$('[id=show]').on('click', function(){
		var val1 = $(this).val();
		$('#indicator').trigger('click');
		var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/show';
		url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}';
		url += '&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
		url += '&page={{ @$request->page }}';
		url += '&pageunit={{ @$request->pageunit }}';
		url += '&sort={{ @$request->sort }}';
		url += '&direction={{ @$request->direction }}';
		url += '&val1=' + val1;
		window.location.href = url;
	});

})

</script>

<style>
	.table-ability th{
		text-align: center;
	}
</style>
<div class="row ml-4">

	<div class="col-xs-12">

		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　能力時間マスタ
			</div>
		</div>

		@if (!$menuInfo->IsReadOnly)
		<div class="col-xs-1 m-1">
			<button id="create" type="button" class="{{ config('system_const.btn_color_new') }}">@if (config('system_const.btn_img_new')!='')<i class="{{ config('system_const.btn_img_new') }}"></i>@endif{{ config('system_const.btn_char_new') }}</button>
		</div>
		@endif
		<div class="row">
			<div class="col-sm-12" style="padding-left:1rem !important;">
				<table class="table table-ability">
					<thead>
						<tr>
							<th>@sortablelink('fld1', '能力時間名称')</th>
							<th>@sortablelink('fld2', '職制')</th>
							<th>@sortablelink('fld3', '施工棟')</th>
							<th>@sortablelink('fld4', '職種')</th>
							<th>@sortablelink('fld5', '開始日')</th>
							<th>@sortablelink('fld6', '終了日')</th>
							<th>@sortablelink('fld7', '工数')</th>
							<th></th>
						</tr>
					</thead>
					@foreach($abilities as $ability)
					<tr>
						<td>{{ $ability['fld1'] }}</td>
						<td>{{ $ability['fld2'] }}</td>
						<td>{{ $ability['fld3'] }}</td>
						<td>{{ $ability['fld4'] }}</td>
						<td style="text-align: center">{{ $ability['fld5'] }}</td>
						<td style="text-align: center">{{ $ability['fld6'] }}</td>
						<td style="text-align: right">{{ $ability['fld7'] }}</td>
						<td>
							@if ($menuInfo->IsReadOnly)
							<div class="col-xs-1 m-1">
								<button id="show" type="button" value="{{ valueUrlEncode($ability['fld8']) }}" class="{{ config('system_const.btn_color_rowinfo') }}">@if (config('system_const.btn_img_rowinfo')!='')<i class="{{ config('system_const.btn_img_rowinfo') }}"></i>@endif{{ config('system_const.btn_char_rowinfo') }}</button>
							</div>
							@else
							<div class="col-xs-1 m-1">
								<button id="edit" type="button" value="{{ valueUrlEncode($ability['fld8']) }}" class="{{ config('system_const.btn_color_rowedit') }}">@if (config('system_const.btn_img_rowedit')!='')<i class="{{ config('system_const.btn_img_rowedit') }}"></i>@endif{{ config('system_const.btn_char_rowedit') }}</button>
							</div>
							@endif
						</td>
					</tr>
					@endforeach
				</table>
				{{ $abilities->appends(request()->query())->links() }}
				<div class="pageunit" style="float:left;">
					表示件数
					<select name="pageunit" class="pageunit-width">
						<option value= {{ config('system_const.displayed_results_1') }} {{ \Request::input('pageunit') == config('system_const.displayed_results_1') ? 'selected' : '' }}> {{ config('system_const.displayed_results_1') }} </option>
						<option value= {{ config('system_const.displayed_results_2') }} {{ \Request::input('pageunit') == config('system_const.displayed_results_2') ? 'selected' : '' }}> {{ config('system_const.displayed_results_2') }} </option>
						<option value= {{ config('system_const.displayed_results_3') }} {{ \Request::input('pageunit') == config('system_const.displayed_results_3') ? 'selected' : '' }}> {{ config('system_const.displayed_results_3') }} </option>
					</select>
				</div>
			</div>
		</div>
	</div>
</div>

@endsection