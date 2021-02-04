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
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/edit';
			url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&page={{ isset($request->page) ? $request->page : 1 }}';
			url += '&pageunit={{ isset($request->pageunit) ? $request->pageunit : config('system_const.displayed_results_1') }}';
			url += '&sort={{ isset($request->sort) ? $request->sort : "fld1" }}';
			url += '&direction={{ isset($request->direction) ? $request->direction : "asc" }}';
			url += '&val1=' + $(this).val();
			window.location.href = url;
		});

		$('.show').on('click', function(e) {
			$('#indicator').trigger('click');
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/show';
			url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&page={{ isset($request->page) ? $request->page : 1 }}';
			url += '&pageunit={{ isset($request->pageunit) ? $request->pageunit : config('system_const.displayed_results_1') }}';
			url += '&sort={{ isset($request->sort) ? $request->sort : "fld1" }}';
			url += '&direction={{ isset($request->direction) ? $request->direction : "asc" }}';
			url += '&val1=' + $(this).val();
			window.location.href = url;
		});

		$('.delete').on('click', function(e) {
			if (window.confirm('{{ config("message.msg_cmn_if_001") }}')) {
				$('#indicator').trigger('click');
				$('input[name=fld1]').val($(this).val());
				$('input[name=fld4]').val($(this).attr('fld4'));
				let pageRedirect = '{{ (count($rows) == 1) ? ((isset($request->page) && $request->page > 1) ? ($request->page - 1) : 1) : (isset($request->page) ? $request->page : 1) }}';
				$('input[name=page]').val(pageRedirect);
				$('#mainform').submit();
			}
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
				<table class="table">
					<thead>
						<th class="text-center">@sortablelink('fld1', trans('mstdist.sortable.fld1'))</th>
						<th class="text-center">@sortablelink('fld2', trans('mstdist.sortable.fld2'))</th>
						<th class="text-center">@sortablelink('fld3', trans('mstdist.sortable.fld3'))</th>
						<th class="text-center"></th>
						@if (!$menuInfo->IsReadOnly)
						<th class="text-center"></th>
						@endif
					</thead>
					<tbody>
						@foreach($rows as $row)
							<tr>
								<td class="align-middle real-space">{{ $row['fld1'] }}</td>
								<td class="align-middle real-space">{{ $row['fld2'] }}</td>
								<td class="align-middle real-space">{{ $row['fld3'] }}</td>
								<td class="align-middle text-center">
									@if (!$menuInfo->IsReadOnly)
										<button type="button" class="edit {{ config('system_const.btn_color_rowedit') }}"
											value="{{ valueUrlEncode($row['fld1']) }}" fld4="{{ valueUrlEncode($row['fld4']) }}">
											@if (config('system_const.btn_img_rowedit') !== '')
												<i class="{{ config('system_const.btn_img_rowedit') }}"></i>
											@endif
											{{ config('system_const.btn_char_rowedit') }}</button>
									@else
										<button type="button" class="show {{ config('system_const.btn_color_rowinfo') }}"
											value="{{ valueUrlEncode($row['fld1']) }}" fld4="{{ valueUrlEncode($row['fld4']) }}">
											@if (config('system_const.btn_img_rowinfo') !== '')
												<i class="{{ config('system_const.btn_img_rowinfo') }}"></i>
											@endif
											{{ config('system_const.btn_char_rowinfo') }}</button>
									@endif
								</td>
								@if (!$menuInfo->IsReadOnly)
								<td class="align-middle text-center">
									<button type="button" class="delete {{ config('system_const.btn_color_rowdelete') }}"
										value="{{ valueUrlEncode($row['fld1']) }}" fld4="{{ valueUrlEncode($row['fld4']) }}">
										@if (config('system_const.btn_img_delete') !== '')
											<i class="{{ config('system_const.btn_img_delete') }}"></i>
										@endif
										{{ config('system_const.btn_char_delete') }}</button>
									</td>
								@endif
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

		<form action="{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/delete" method="POST" id="mainform">
			@csrf
			<input type="hidden" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}" />
			<input type="hidden" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}" />
			<input type="hidden" name="page" value="{{ isset($request->page) ? $request->page : 1 }}" />
			<input type="hidden" name="pageunit" value="{{ isset($request->pageunit) ? $request->pageunit : config('system_const.displayed_results_1') }}" />
			<input type="hidden" name="sort" value="{{ isset($request->sort) ? $request->sort : "fld1" }}" />
			<input type="hidden" name="direction" value="{{ isset($request->direction) ? $request->direction : "asc" }}" />
			<input type="hidden" name="fld1" value="" />
			<input type="hidden" name="fld4" value="" />
		</form>
	</div>
</div>
@endsection