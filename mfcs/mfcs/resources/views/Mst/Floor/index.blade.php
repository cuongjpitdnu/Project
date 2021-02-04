@extends('layouts/mainmenu/menu')
@section('content')
<script>
$(function(){
	$('[name="pageunit"]').change(function(e){
		$('#indicator').trigger('click');
		var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/';
		url += 'index?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
		url += '&page=1&pageunit=' + $(this).val();
		url += '&sort={{ \Request::input('sort') ? \Request::input('sort') : '' }}';
		url += '&direction={{ \Request::input('direction') ? \Request::input('direction') : 'asc' }}';
		window.location.href = url;
	});

	$('.show').on('click', function(){
		var val1 = $(this).val();
		$('#indicator').trigger('click');
		var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/show';
		url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
		url += '&page={{ isset($request->page) ? $request->page : 1 }}';
		url += '&pageunit={{ isset($request->pageunit) ? $request->pageunit : config("system_const.displayed_results_1") }}';
		url += '&sort={{ isset($request->sort) ? $request->sort : "" }}';
		url += '&direction={{ isset($request->direction) ? $request->direction : "asc" }}';
		url += '&val1=' + val1;
		window.location.href = url;
	});

	$('.create').on('click', function(){
		$('#indicator').trigger('click');
		var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/create';
		url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
		url += '&page={{ isset($request->page) ? $request->page : 1 }}';
		url += '&pageunit={{ isset($request->pageunit) ? $request->pageunit : config("system_const.displayed_results_1") }}';
		url += '&sort={{ isset($request->sort) ? $request->sort : "" }}';
		url += '&direction={{ isset($request->direction) ? $request->direction : "asc" }}';
		window.location.href = url;
	});

	$('.edit').on('click', function(){
		var val1 = $(this).val();
		$('#indicator').trigger('click');
		var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/edit';
		url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
		url += '&page={{ isset($request->page) ? $request->page : 1 }}';
		url += '&pageunit={{ isset($request->pageunit) ? $request->pageunit : config("system_const.displayed_results_1") }}';
		url += '&sort={{ isset($request->sort) ? $request->sort : "" }}';
		url += '&direction={{ isset($request->direction) ? $request->direction : "asc" }}';
		url += '&val1=' + val1;
		window.location.href = url;
	});
});

</script>

<div class="row ml-4">
	<div class="col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　棟マスタ
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

		<div class="row align-items-center">
			@if (!$menuInfo->IsReadOnly)
			<div class="col-xs-1 m-3">
				<button type="button" class="create {{ config('system_const.btn_color_new') }}">
				@if (config('system_const.btn_img_new')!='')<i class="{{ config('system_const.btn_img_new') }}"></i>@endif
				{{ config('system_const.btn_char_new') }}
				</button>
			</div>
			@endif
		</div>

		<div class="row">
			<div class="col-sm-12">
				<table class="table">
					<thead>
						<tr>
							<th class="text-center">@sortablelink('fld1', 'コード')</th>
							<th class="text-center">@sortablelink('fld2', '名称')</th>
							<th class="text-center">@sortablelink('fld3', '略称')</th>
							<th class="text-center">@sortablelink('fld4', '表示順')</th>
							<th class="text-center">@sortablelink('fld5', '有効')</th>
							<th class="text-center"></th>
						</tr>
					</thead>
					<tbody>
					@foreach($floors as $floor)
					<tr>
						<td class="real-space align-middle">{{ $floor['fld1'] }}</td>
						<td class="real-space align-middle">{{ $floor['fld2'] }}</td>
						<td class="real-space align-middle">{{ $floor['fld3'] }}</td>
						<td class="real-space align-middle text-right">{{ $floor['fld4'] }}</td>
						<td class="align-middle">{{ $floor['fld5'] ? '有効' : '無効' }}</td>
						<td class="align-middle text-center">
							@if (!$menuInfo->IsReadOnly)
								<button type="button" class="edit {{ config('system_const.btn_color_rowedit') }}"
								value="{{ valueUrlEncode($floor['fld1']) }}">
								@if (config('system_const.btn_img_rowedit')!='')<i class="{{ config('system_const.btn_img_rowedit') }}"></i>@endif
								{{ config('system_const.btn_char_rowedit') }}
								</button>
							@else
								<button type="button" class="show {{ config('system_const.btn_color_rowinfo') }}"
								value="{{ valueUrlEncode($floor['fld1']) }}">
								@if (config('system_const.btn_img_rowinfo')!='')<i class="{{ config('system_const.btn_img_rowinfo') }}"></i>@endif
								{{ config('system_const.btn_char_rowinfo') }}
								</button>
							@endif
						</td>
					</tr>
					@endforeach
					</tbody>
				</table>
				{{ $floors->appends(request()->query())->links() }}
				<div class="pageunit float-left">
					表示件数
					<select name="pageunit" class="pageunit-width">
						<option value= {{ config('system_const.displayed_results_1') }} {{ (int)\Request::input('pageunit') === config('system_const.displayed_results_1') ? 'selected' : '' }}> {{ config('system_const.displayed_results_1') }} </option>
						<option value= {{ config('system_const.displayed_results_2') }} {{ (int)\Request::input('pageunit') === config('system_const.displayed_results_2') ? 'selected' : '' }}> {{ config('system_const.displayed_results_2') }} </option>
						<option value= {{ config('system_const.displayed_results_3') }} {{ (int)\Request::input('pageunit') === config('system_const.displayed_results_3') ? 'selected' : '' }}> {{ config('system_const.displayed_results_3') }} </option>
					</select>
				</div>
			</div>

		</div>

	</div>
</div>
@endsection
