@extends('layouts/mainmenu/menu')
@section('content')
<script>
	$(function() {
		$('select[name=pageunit]').on('change', function(e) {
			$('#indicator').trigger('click');
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/';
			url += 'indexdetail?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&page=1&pageunit=' + $(this).val();
			url += '&sort={{ \Request::input('sort') ? \Request::input('sort') : 'fld1' }}';
			url += '&direction={{ \Request::input('direction') ? \Request::input('direction') : 'asc' }}';
			url += '&val1={{ $request->val1 }}';
			url += '&val3={{ $request->val3 }}';
			window.location.href = url;
		});

		$('.create').on('click', function(e) {
			$('#indicator').trigger('click');
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/createdetail';
			url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&page={{ isset($request->page) ? $request->page : 1 }}';
			url += '&pageunit={{ isset($request->pageunit) ? $request->pageunit : config('system_const.displayed_results_1') }}';
			url += '&sort={{ isset($request->sort) ? $request->sort : "fld1" }}';
			url += '&direction={{ isset($request->direction) ? $request->direction : "asc" }}'
			url += '&val1={{ $request->val1 }}';
			url += '&val3={{ $request->val3 }}';
			window.location.href = url;
		});

		$('.edit').on('click', function(e) {
			$('#indicator').trigger('click');
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/editdetail';
			url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&page={{ isset($request->page) ? $request->page : 1 }}';
			url += '&pageunit={{ isset($request->pageunit) ? $request->pageunit : config('system_const.displayed_results_1') }}';
			url += '&sort={{ isset($request->sort) ? $request->sort : "fld1" }}';
			url += '&direction={{ isset($request->direction) ? $request->direction : "asc" }}';
			url += '&val1={{ $request->val1 }}';
			url += '&val3={{ $request->val3 }}';
			url += '&val101='+$(this).attr('no');
			url += '&val102='+$(this).attr('updated_at');
			window.location.href = url;
		});

		$('.show').on('click', function(e) {
			$('#indicator').trigger('click');
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/showdetail';
			url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&page={{ isset($request->page) ? $request->page : 1 }}';
			url += '&pageunit={{ isset($request->pageunit) ? $request->pageunit : config('system_const.displayed_results_1') }}';
			url += '&sort={{ isset($request->sort) ? $request->sort : "fld1" }}';
			url += '&direction={{ isset($request->direction) ? $request->direction : "asc" }}';
			url += '&val1={{ $request->val1 }}';
			url += '&val3={{ $request->val3 }}';
			url += '&val101='+$(this).attr('no');
			window.location.href = url;
		});

		$('.delete').on('click', function(e) {
			if (!window.confirm('{{ config("message.msg_cmn_if_001") }}')) {
				return;
			}
			$('#indicator').trigger('click');
			$('input[name=val101]').val($(this).attr('no'));
			$('input[name=val102]').val($(this).attr('updated_at'));
			let pageRedirect = '{{ (count($rows) == 1) ? ((isset($request->page) && $request->page > 1) ? ($request->page - 1) : 1) : (isset($request->page) ? $request->page : 1) }}';
			$('input[name=page]').val(pageRedirect);
			$('#mainform').submit();
		});
	});
</script>

<div class="row ml-4">
	<div class="col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　工程パターンマスタ(詳細)
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
				@if (config('system_const.btn_img_new') !== '')
					<i class="{{ config('system_const.btn_img_new') }}"></i>
				@endif
					{{ config('system_const.btn_char_new') }}</button>
			</div>
			@endif
			<div class="col-xs-3 text-left m-2 p-2">中日程区分：{{ (int)$parent->CKind === config('system_const.c_kind_chijyo') ?
				config('system_const.c_name_chijyo') : ((int)$parent->CKind === config('system_const.c_kind_gaigyo') ?
					config('system_const.c_name_gaigyo') : config('system_const.c_name_giso')) }}</div>
			<div class="col-xs-3 text-left m-2 p-2">パターンコード：{{ valueUrlDecode($request->val1) }}</div>
			<div class="col-xs-3 text-left m-2 p-2">名称：{{ $parent->Name }}</div>
		</div>

		<div class="row">
			<div class="col-sm-12">
				<table class="table table-row">
					<thead>
						<th class="text-center">@sortablelink('fld1', trans('mstkoteistrc.sortable.fld1'))</th>
						<th class="text-center">@sortablelink('fld2', trans('mstkoteistrc.sortable.fld2'))</th>
						<th class="text-center">@sortablelink('fld3', trans('mstkoteistrc.sortable.fld3'))</th>
						<th class="text-center">@sortablelink('fld4', trans('mstkoteistrc.sortable.fld4'))</th>
						<th class="text-center">@sortablelink('fld5', trans('mstkoteistrc.sortable.fld5'))</th>
						<th class="text-center">@sortablelink('fld6', trans('mstkoteistrc.sortable.fld6'))</th>
						<th class="text-center">@sortablelink('fld7', trans('mstkoteistrc.sortable.fld7'))</th>
						<th class="text-center"></th>
						<th class="text-center"></th>
					</thead>
					<tbody>
						@foreach($rows as $row)
							<tr>
								<td class="real-space align-middle">{{ $row['fld1'] }}</td>
								<td class="real-space align-middle">{{ $row['fld2'] }}</td>
								<td class="real-space text-right align-middle">{{ $row['fld3'] }}</td>
								<td class="real-space align-middle">{{ $row['fld4'] }}</td>
								<td class="real-space align-middle">{{ $row['fld5'] }}</td>
								<td class="real-space align-middle">{{ $row['fld6'] }}</td>
								<td class="real-space align-middle">{{ $row['fld7'] }}</td>
								<td class="text-center align-middle">
									@if (!$menuInfo->IsReadOnly)
										<button type="button" class="edit {{ config('system_const.btn_color_rowedit') }}"
											no="{{ valueUrlEncode($row['No']) }}" updated_at="{{ valueUrlEncode($row['Updated_at']) }}">
											@if (config('system_const.btn_img_rowedit') !== '')
												<i class="{{ config('system_const.btn_img_rowedit') }}"></i>
											@endif
											{{ config('system_const.btn_char_rowedit') }}</button>
									@else
										<button type="button" class="show {{ config('system_const.btn_color_rowinfo') }}"
												no="{{ valueUrlEncode($row['No']) }}">
											@if (config('system_const.btn_img_rowinfo') !== '')
												<i class="{{ config('system_const.btn_img_rowinfo') }}"></i>
											@endif
											{{ config('system_const.btn_char_rowinfo') }}</button>
									@endif
								</td>
								<td class="text-center align-middle">
									@if (!$menuInfo->IsReadOnly)
										<button type="button" class="delete {{ config('system_const.btn_color_rowdelete') }}"
												no="{{ valueUrlEncode($row['No']) }}" updated_at="{{ valueUrlEncode($row['Updated_at']) }}">
											@if (config('system_const.btn_img_delete') !== '')
												<i class="{{ config('system_const.btn_img_delete') }}"></i>
											@endif
											{{ config('system_const.btn_char_delete') }}</button>
									@endif
								</td>
							</tr>
						@endforeach
					</tbody>
				</table>
				{{ $rows->appends(request()->query())->links() }}
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

		<form action="{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/deletedetail" method="POST" id="mainform">
			@csrf
			<input type="hidden" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}" />
			<input type="hidden" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}" />
			<input type="hidden" name="page" value="{{ isset($request->page) ? $request->page : 1 }}" />
			<input type="hidden" name="pageunit" value="{{ isset($request->pageunit) ? $request->pageunit : 10 }}" />
			<input type="hidden" name="sort" value="{{ isset($request->sort) ? $request->sort : "fld1" }}" />
			<input type="hidden" name="direction" value="{{ isset($request->direction) ? $request->direction : "asc" }}" />
			<input type="hidden" name="val1" value="{{ $request->val1 }}" />
			<input type="hidden" name="val3" value="{{ $request->val3 }}" />
			<input type="hidden" name="val101" value="" />
			<input type="hidden" name="val102" value="" />
		</form>
	</div>
</div>
@endsection