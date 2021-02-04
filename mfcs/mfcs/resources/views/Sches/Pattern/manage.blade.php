@extends('layouts/mainmenu/menu')
@section('content')
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip()

		$('.create').on('click', function(e) {
			$('#indicator').trigger('click');
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/create';
			url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&page={{ isset($request->page) ? $request->page : 1 }}';
			url += '&pageunit={{ isset($request->pageunit) ? $request->pageunit : config('system_const.displayed_results_1') }}';
			url += '&sort={{ isset($request->sort) ? $request->sort : "fld1" }}';
			url += '&direction={{ isset($request->direction) ? $request->direction : "asc" }}';
			url += '&val1={{ isset($request->val1) ? $request->val1 : valueUrlDecode(0) }}';
			window.location.href = url;
		});

		$('.detail').on('click', function(e) {
			$('#indicator').trigger('click');
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/pmanage';
			url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&val1={{ isset($request->val1) ? $request->val1 : valueUrlDecode(0) }}';
			url += '&val2=' + $(this).val();
			url += '&val3={{ valueUrlEncode(isset($request->page) ? $request->page : 1) }}';
			url += '&val4={{ valueUrlEncode(isset($request->pageunit) ? $request->pageunit : config('system_const.displayed_results_1')) }}';
			url += '&val5={{ valueUrlEncode(isset($request->sort) ? $request->sort : "fld1") }}';
			url += '&val6={{ valueUrlEncode(isset($request->direction) ? $request->direction : "asc") }}';
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
			url += '&val1={{ isset($request->val1) ? $request->val1 : valueUrlDecode(0) }}';
			url += '&val2=' + $(this).val();
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
			url += '&val1={{ isset($request->val1) ? $request->val1 : valueUrlDecode(0) }}';
			url += '&val2=' + $(this).val();
			window.location.href = url;
		});

		$('.delete').on('click', function(e) {
			if (window.confirm('{{ config("message.msg_cmn_if_001") }}')) {
				$('#indicator').trigger('click');
				$('input[name=fld1]').val($(this).val());
				$('input[name=fld8]').val($(this).attr('fld8'));
				let pageRedirect = '{{ (count($listPattern) == 1) ? ((isset($request->page) && $request->page > 1) ? ($request->page - 1) : 1) : (isset($request->page) ? $request->page : 1) }}';
				$('input[name=page]').val(pageRedirect);
				$('#mainform').submit();
			}
		});
	});
</script>

<div class="row ml-2 mr-2">
	<div class="col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　展開パターンマスタ(一覧)
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

		<div class="row align-items-center ml-1 mt-3">
			<table class="table table-borderless">
				<tr>
					<td class="p-0 align-middle">
						@if (!$menuInfo->IsReadOnly)
						<span class="col-xs-1 p-1">
							<button type="button" id="select" class="create {{ config('system_const.btn_color_new') }}">
								<i class="{{ config('system_const.btn_img_new') }}"></i>{{ config('system_const.btn_char_new') }}
							</button>
						</span>
						@endif
						<span class="align-middle">所有課・係：{{ $val1 }}</span>
					</td>
				</tr>
			</table>
			<table class="table table-borderless">
				<thead>
					<tr>
						<th class="align-middle text-center">@sortablelink('fld1', 'ID')</th>
						<th class="align-middle text-center">@sortablelink('fld2', '名称')</th>
						<th class="align-middle text-center">@sortablelink('fld3', '組区')</th>
						<th class="align-middle text-center">@sortablelink('fld4', '工程')</th>
						<th class="align-middle text-center">@sortablelink('fld5', '工程組区')</th>
						<th class="align-middle text-center">@sortablelink('fld5', '施工棟')</th>
						<th class="align-middle text-center">@sortablelink('fld5', '基準データ')</th>
						<th class="align-middle text-center"></th>
						<th class="align-middle text-center"></th>
						<th class="align-middle text-center"></th>
					</tr>
				</thead>
				<tbody>
				@foreach($listPattern as $pattern)
				<tr>
					<td class="text-right">{{ $pattern['fld1'] }}</td>
					<td>{{ $pattern['fld2'] }}</td>
					<td>{{ $pattern['fld3'] }}</td>
					<td>{{ $pattern['fld4'] }}</td>
					<td>{{ $pattern['fld5'] }}</td>
					<td>{{ $pattern['fld6'] }}</td>
					<td>{{ $pattern['fld7'] }}</td>
					<td>
						<button type="button" class="detail {{ config('system_const.btn_color_rowdetail') }}"
						value="{{ valueUrlEncode($pattern['fld1']) }}" fld8="{{ valueUrlEncode($pattern['fld8']) }}">
						@if (config('system_const.btn_img_rowdetail')!='')<i class="{{ config('system_const.btn_img_rowdetail') }}"></i>@endif
						{{ config('system_const.btn_char_rowdetail') }}
						</button>
					</td>
					<td>
						@if (!$menuInfo->IsReadOnly)
							<button type="button" class="edit {{ config('system_const.btn_color_rowedit') }}"
							value="{{ valueUrlEncode($pattern['fld1']) }}" fld8="{{ valueUrlEncode($pattern['fld8']) }}">
							@if (config('system_const.btn_img_rowedit')!='')<i class="{{ config('system_const.btn_img_rowedit') }}"></i>@endif
							{{ config('system_const.btn_char_rowedit') }}
							</button>
						@else
							<button type="button" class="show {{ config('system_const.btn_color_rowinfo') }}"
							value="{{ valueUrlEncode($pattern['fld1']) }}" fld8="{{ valueUrlEncode($pattern['fld8']) }}">
							@if (config('system_const.btn_img_rowinfo')!='')<i class="{{ config('system_const.btn_img_rowinfo') }}"></i>@endif
							{{ config('system_const.btn_char_rowinfo') }}
							</button>
						@endif
					</td>
					<td>
						@if (!$menuInfo->IsReadOnly)
							<button type="button" class="delete {{ config('system_const.btn_color_rowdelete') }}"
							value="{{ valueUrlEncode($pattern['fld1']) }}" fld8="{{ valueUrlEncode($pattern['fld8']) }}">
							@if (config('system_const.btn_img_rowdelete')!='')<i class="{{ config('system_const.btn_img_rowdelete') }}"></i>@endif
							{{ config('system_const.btn_char_rowdelete') }}
							</button>
						@endif
					</td>
				</tr>
				@endforeach
				</tbody>
			</table>
			{{ $listPattern->appends(request()->query())->links() }}
		</div>

		<form action="{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/delete" method="POST" id="mainform">
			@csrf
			<input type="hidden" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}" />
			<input type="hidden" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}" />
			<input type="hidden" name="page" value="{{ isset($request->page) ? $request->page : 1 }}" />
			<input type="hidden" name="pageunit" value="{{ isset($request->pageunit) ? $request->pageunit : config('system_const.displayed_results_1') }}" />
			<input type="hidden" name="sort" value="{{ isset($request->sort) ? $request->sort : "fld1" }}" />
			<input type="hidden" name="direction" value="{{ isset($request->direction) ? $request->direction : "asc" }}" />
			<input type="hidden" name="val1" value="{{ isset($request->val1) ? $request->val1 : valueUrlEncode(0) }}">
			<input type="hidden" name="fld1" value="" />
			<input type="hidden" name="fld8" value="" />
		</form>
	</div>
</div>
@endsection