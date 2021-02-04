@extends('layouts/mainmenu/menu')
@section('content')
<script>
	$(function() {
		$('select[name=pageunit]').on('change', function(e) {
			$('#indicator').trigger('click');
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/';
			url += 'index?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&page=1&pageunit=' + $(this).val();
			url += '&sort={{ \Request::input('sort') ? \Request::input('sort') : 'fld1' }}';
			url += '&direction={{ \Request::input('direction') ? \Request::input('direction') : 'asc' }}';
			window.location.href = url;
		});

		$('.create').on('click', function(e) {
			$('#indicator').trigger('click');
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/create';
			url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&page={{ isset($request->page) ? $request->page : 1 }}';
			url += '&pageunit={{ isset($request->pageunit) ? $request->pageunit : config('system_const.displayed_results_1') }}';
			url += '&sort={{ isset($request->sort) ? $request->sort : "fld1" }}';
			url += '&direction={{ isset($request->direction) ? $request->direction : "asc" }}';
			window.location.href = url;
		});

		$('.edit').on('click', function(e) {
			$('#indicator').trigger('click');
			var val1 = $(this).attr('fld1');
			var val4 = $(this).attr('fld4');

			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/edit';
			url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&page={{ isset($request->page) ? $request->page : 1 }}';
			url += '&pageunit={{ isset($request->pageunit) ? $request->pageunit : config('system_const.displayed_results_1') }}';
			url += '&sort={{ isset($request->sort) ? $request->sort : "fld1" }}';
			url += '&direction={{ isset($request->direction) ? $request->direction : "asc" }}';
			url += '&val1='+val1;
			url += '&val4='+val4;
			window.location.href = url;
		});

		$('.show').on('click', function(e) {
			$('#indicator').trigger('click');
			var val1 = $(this).attr('fld1');
			var val4 = $(this).attr('fld4');

			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/show';
			url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&page={{ isset($request->page) ? $request->page : 1 }}';
			url += '&pageunit={{ isset($request->pageunit) ? $request->pageunit : config('system_const.displayed_results_1') }}';
			url += '&sort={{ isset($request->sort) ? $request->sort : "fld1" }}';
			url += '&direction={{ isset($request->direction) ? $request->direction : "asc" }}';
			url += '&val1='+val1;
			url += '&val4='+val4;
			window.location.href = url;
		});
	});
</script>
<div class="row ml-4">
	<div class="col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　工程定義マスタ
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
				<button type="button" name="create" class="create {{ config('system_const.btn_color_new') }}">
				@if (config('system_const.btn_img_new')!='')
					<i class="{{ config('system_const.btn_img_new') }}"></i>
				@endif
					{{ config('system_const.btn_char_new') }}</button>
			</div>
			@endif
		</div>
		<div class="row">
			<div class="col-sm-12">
				<table class="table table-row">
					<thead>
						<th class="text-center">@sortablelink('fld1', trans('mstkotei.sortable.fld1'))</th>
						<th class="text-center">@sortablelink('fld2', trans('mstkotei.sortable.fld2'))</th>
						<th class="text-center">@sortablelink('fld3', trans('mstkotei.sortable.fld3'))</th>
						<th class="text-center">@sortablelink('fld4', trans('mstkotei.sortable.fld4'))</th>
						<th class="text-center">@sortablelink('fld5', trans('mstkotei.sortable.fld5'))</th>
						<th class="text-center"></th>
					</thead>
					<tbody>
						@foreach($rows as $row)
							<tr>
								<td class="real-space align-middle">{{ $row['fld1'] }}</td>
								<td class="real-space align-middle">{{ $row['fld2'] }}</td>
								<td class="real-space align-middle">{{ $row['fld3'] }}</td>	
								<td class="real-space align-middle">{{ (int)$row['fld4'] === config('system_const.c_kind_chijyo') ?
									config('system_const.c_name_chijyo') : ((int)$row['fld4'] === config('system_const.c_kind_gaigyo') ?
										config('system_const.c_name_gaigyo') : config('system_const.c_name_giso')) }}</td>

								<td class="text-center align-middle">{{ (int)$row['fld5'] === 0 ? '有効' : '無効' }}</td>
								<td class="text-center align-middle">
									@if (!$menuInfo->IsReadOnly)
										<button type="button" class="edit {{ config('system_const.btn_color_rowedit') }}"
											fld1="{{ valueUrlEncode($row['fld1']) }}" fld4="{{ valueUrlEncode($row['fld4']) }}">
											@if (config('system_const.btn_img_rowedit') !== '')
												<i class="{{ config('system_const.btn_img_rowedit') }}"></i>
											@endif
											{{ config('system_const.btn_char_rowedit') }}</button>
									@else
										<button type="button" class="show {{ config('system_const.btn_color_rowinfo') }}"
										fld1="{{ valueUrlEncode($row['fld1']) }}" fld4="{{ valueUrlEncode($row['fld4']) }}">
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
				{{ $rows->appends(\Request::except('page'))->render() }}
				<div class="pageunit float-left">
					表示件数
					<select name="pageunit" class="pageunit-width">
						<option value="{{config('system_const.displayed_results_1')}}"
							{{ (int)\Request::input('pageunit') === config('system_const.displayed_results_1') ? 'selected' : '' }}>{{
							config('system_const.displayed_results_1')
						}}</option>
						<option value="{{config('system_const.displayed_results_2')}}"
							{{ (int)\Request::input('pageunit') === config('system_const.displayed_results_2') ? 'selected' : '' }}>{{
							config('system_const.displayed_results_2')
						}}</option>
						<option value="{{config('system_const.displayed_results_3')}}"
							{{ (int)\Request::input('pageunit') === config('system_const.displayed_results_3') ? 'selected' : '' }}>{{
							config('system_const.displayed_results_3')
						}}</option>
					</select>
				</div>
			</div>
		</div>
	</div>
</div>
@endsection