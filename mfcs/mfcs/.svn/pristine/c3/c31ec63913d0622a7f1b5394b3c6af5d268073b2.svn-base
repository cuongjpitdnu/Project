@extends('layouts/mainmenu/menu')
@section('content')
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip()

		$('.create').on('click', function(e) {
			$('#indicator').trigger('click');
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/createdetail';
			url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&page={{ isset($request->page) ? $request->page : 1 }}';
			url += '&pageunit={{ isset($request->pageunit) ? $request->pageunit : config('system_const.displayed_results_1') }}';
			url += '&sort={{ isset($request->sort) ? $request->sort : "ID" }}';
			url += '&direction={{ isset($request->direction) ? $request->direction : "asc" }}';
			url += '&val1={{ isset($request->val1) ? $request->val1 : valueUrlDecode(0) }}';
			url += '&val2={{ isset($request->val2) ? $request->val2 : valueUrlDecode(0) }}';
			window.location.href = url;
		});

		$('.detail').on('click', function(e) {
			$('#indicator').trigger('click');
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/{{ !$menuInfo->IsReadOnly ? "editpatterndetail" : "showpatterndetail" }}' ;
			url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&page={{ isset($request->page) ? $request->page : 1 }}';
			url += '&pageunit={{ isset($request->pageunit) ? $request->pageunit : config('system_const.displayed_results_1') }}';
			url += '&sort={{ isset($request->sort) ? $request->sort : "ID" }}';
			url += '&direction={{ isset($request->direction) ? $request->direction : "asc" }}';
			url += '&val1={{ isset($request->val1) ? $request->val1 : valueUrlDecode(0) }}';
			url += '&val2={{ isset($request->val2) ? $request->val2 : valueUrlDecode(0) }}';
			url += '&val3=' + $(this).val();
			window.location.href = url;
		});

		$('.edit').on('click', function(e) {
			$('#indicator').trigger('click');
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/editdetail';
			url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&page={{ isset($request->page) ? $request->page : 1 }}';
			url += '&pageunit={{ isset($request->pageunit) ? $request->pageunit : config('system_const.displayed_results_1') }}';
			url += '&sort={{ isset($request->sort) ? $request->sort : "ID" }}';
			url += '&direction={{ isset($request->direction) ? $request->direction : "asc" }}';
			url += '&val1={{ isset($request->val1) ? $request->val1 : valueUrlDecode(0) }}';
			url += '&val2={{ isset($request->val2) ? $request->val2 : valueUrlDecode(0) }}';
			url += '&val3=' + $(this).val();
			window.location.href = url;
		});

		$('.show').on('click', function(e) {
			$('#indicator').trigger('click');
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/showdetail';
			url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&page={{ isset($request->page) ? $request->page : 1 }}';
			url += '&pageunit={{ isset($request->pageunit) ? $request->pageunit : config('system_const.displayed_results_1') }}';
			url += '&sort={{ isset($request->sort) ? $request->sort : "ID" }}';
			url += '&direction={{ isset($request->direction) ? $request->direction : "asc" }}';
			url += '&val1={{ isset($request->val1) ? $request->val1 : valueUrlDecode(0) }}';
			url += '&val2={{ isset($request->val2) ? $request->val2 : valueUrlDecode(0) }}';
			url += '&val3=' + $(this).val();
			window.location.href = url;
		});

		$('.delete').on('click', function(e) {
			if (window.confirm('{{ config("message.msg_cmn_if_001") }}')) {
				$('#indicator').trigger('click');
				$('input[name=ID]').val($(this).val());
				$('input[name=Updated_at]').val($(this).attr('updatedAt'));
				let pageRedirect = '{{ (count($listPattern) == 1) ? ((isset($request->page) && $request->page > 1) ? ($request->page - 1) : 1) : (isset($request->page) ? $request->page : 1) }}';
				$('input[name=page]').val(pageRedirect);
				$('#mainform').submit();
			}
		});

		$('#cancel').on('click', function(e) {
			$('#indicator').trigger('click');
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/manage';
			url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&page={{ isset($request->val3) ? $request->val3 : 1 }}';
			url += '&pageunit={{ isset($request->val4) ? $request->val4 : config('system_const.displayed_results_1') }}';
			url += '&sort={{ isset($request->val5) ? $request->val5 : "ID" }}';
			url += '&direction={{ isset($request->val6) ? $request->val6 : "asc" }}';
			url += '&val1={{ isset($request->val1) ? $request->val1 : valueUrlDecode(0) }}';
			window.location.href = url;
		});

		$('[name="pageunit"]').change(function(e){
			$('#indicator').trigger('click');
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/pmanage';
			url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&page=1&pageunit=' + $(this).val();
			url += '&sort={{ \Request::input('sort') ? \Request::input('sort') : 'ID' }}';
			url += '&direction={{ \Request::input('direction') ? \Request::input('direction') : 'asc' }}';
			url += '&val1={{ \Request::input('val1') ? \Request::input('val1') : valueUrlEncode(0) }}';
			url += '&val2={{ \Request::input('val2') ? \Request::input('val2') : valueUrlEncode(0) }}';
			window.location.href = url;
		});
	});
</script>

<div class="row ml-2 mr-2">
	<div class="col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　展開パターンマスタ(詳細一覧)
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
						<span class="col-xs-1 p-1 mr-1">
							<button type="button" id="select" class="create {{ config('system_const.btn_color_new') }}">
								<i class="{{ config('system_const.btn_img_new') }}"></i>{{ config('system_const.btn_char_new') }}
							</button>
						</span>
						@endif
						<span class="align-middle d-inline-block">
							<p class="m-0">所有課・係：{{ $val1 }}</p>
							<p class="m-0">パターンID：{{ valueUrlDecode($request->val2) }} <span class="ml-3">パターン名称：{{ $patternName }}</span></p>
						</span>
					</td>
				</tr>
			</table>
			<table class="table table-borderless">
				<thead>
					<tr>
						<th class="align-middle text-center">@sortablelink('ID', 'ID')</th>
						<th class="align-middle text-center">@sortablelink('Name', '名称')</th>
						<th class="align-middle text-center">@sortablelink('Kumiku', '組区')</th>
						<th class="align-middle text-center">@sortablelink('DistCode', '職種')</th>
						<th class="align-middle text-center">@sortablelink('AcMac', '装置')</th>
						<th class="align-middle text-center">@sortablelink('Item', 'アイテム')</th>
						<th class="align-middle text-center">@sortablelink('BD_Code', '管理物量')</th>
						<th class="align-middle text-center">@sortablelink('KeshiPattern', '消込方式')</th>
						<th class="align-middle text-center">@sortablelink('KeshiCode', '消込管理')</th>
						<th class="align-middle text-center">@sortablelink('SortNo', '表示順')</th>
						<th class="align-middle text-center"></th>
						<th class="align-middle text-center"></th>
						<th class="align-middle text-center"></th>
					</tr>
				</thead>
				<tbody>
				@foreach($listPattern as $pattern)
				<tr>
					<td class="text-right">{{ $pattern['ID'] }}</td>
					<td>{{ $pattern['Name'] }}</td>
					<td>{{ $pattern['Kumiku'] }}</td>
					<td>{{ $pattern['DistCode'] }}</td>
					<td>{{ $pattern['AcMac'] }}</td>
					<td>{{ $pattern['Item'] }}</td>
					<td>{{ $pattern['BD_Code'] }}</td>
					<td>{{ $pattern['KeshiPattern'] }}</td>
					<td>{{ $pattern['KeshiCode'] }}</td>
					<td class="text-right">{{ $pattern['SortNo'] }}</td>
					<td>
						<button type="button" class="detail {{ config('system_const.btn_color_rowdetail') }}"
						value="{{ valueUrlEncode($pattern['ID']) }}" updatedAt="{{ valueUrlEncode($pattern['Updated_at']) }}">
						@if (config('system_const.btn_img_rowdetail')!='')<i class="{{ config('system_const.btn_img_rowdetail') }}"></i>@endif
						{{ config('system_const.btn_char_rowdetail') }}
						</button>
					</td>
					<td>
						@if (!$menuInfo->IsReadOnly)
							<button type="button" class="edit {{ config('system_const.btn_color_rowedit') }}"
							value="{{ valueUrlEncode($pattern['ID']) }}" updatedAt="{{ valueUrlEncode($pattern['Updated_at']) }}">
							@if (config('system_const.btn_img_rowedit')!='')<i class="{{ config('system_const.btn_img_rowedit') }}"></i>@endif
							{{ config('system_const.btn_char_rowedit') }}
							</button>
						@else
							<button type="button" class="show {{ config('system_const.btn_color_rowinfo') }}"
							value="{{ valueUrlEncode($pattern['ID']) }}" updatedAt="{{ valueUrlEncode($pattern['Updated_at']) }}">
							@if (config('system_const.btn_img_rowinfo')!='')<i class="{{ config('system_const.btn_img_rowinfo') }}"></i>@endif
							{{ config('system_const.btn_char_rowinfo') }}
							</button>
						@endif
					</td>
					<td>
						@if (!$menuInfo->IsReadOnly)
							<button type="button" class="delete {{ config('system_const.btn_color_rowdelete') }}"
							value="{{ valueUrlEncode($pattern['ID']) }}" updatedAt="{{ valueUrlEncode($pattern['Updated_at']) }}">
							@if (config('system_const.btn_img_rowdelete')!='')<i class="{{ config('system_const.btn_img_rowdelete') }}"></i>@endif
							{{ config('system_const.btn_char_rowdelete') }}
							</button>
						@endif
					</td>
				</tr>
				@endforeach
				</tbody>
			</table>
			<div class="col-xs-1 p-1 mr-3 mb-3">
				<button type="button" id="cancel" class="{{ config('system_const.btn_color_cancel') }}">
					<i class="{{ config('system_const.btn_img_cancel') }}"></i>{{ config('system_const.btn_char_cancel') }}
				</button>
			</div>
			{{ $listPattern->appends(\Request::except('page'))->render() }}
			<div class="pageunit mb-3">
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

		<form action="{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/deletedetail" method="POST" id="mainform">
			@csrf
			<input type="hidden" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}" />
			<input type="hidden" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}" />
			<input type="hidden" name="page" value="{{ isset($request->page) ? $request->page : 1 }}" />
			<input type="hidden" name="pageunit" value="{{ isset($request->pageunit) ? $request->pageunit : config('system_const.displayed_results_1') }}" />
			<input type="hidden" name="sort" value="{{ isset($request->sort) ? $request->sort : "fld1" }}" />
			<input type="hidden" name="direction" value="{{ isset($request->direction) ? $request->direction : "asc" }}" />
			<input type="hidden" name="val1" value="{{ isset($request->val1) ? $request->val1 : valueUrlEncode(0) }}">
			<input type="hidden" name="val2" value="{{ isset($request->val2) ? $request->val2 : valueUrlEncode(0) }}">
			<input type="hidden" name="ID" value="" />
			<input type="hidden" name="Updated_at" value="" />
		</form>
	</div>
</div>
@endsection