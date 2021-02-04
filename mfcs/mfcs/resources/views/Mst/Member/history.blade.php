@extends('layouts/mainmenu/menu')
@section('content')
<script>
$(function(){
	$('.toprev').on('click', function(){
		$('#indicator').trigger('click');
		var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/search';
		url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
		url += '&pageunit={{ $request->pageunit }}&page={{ $request->searchpage }}';
		url += '&sort={{ $request->searchsort }}';
		url += '&direction={{ $request->searchdirection }}';
		url +=  '&val1={{ $request -> val1 }}&val2={{ $request -> val2 }}&val3={{ $request -> val3 }}';
		url +=  '&val4={{ $request -> val4 }}&val5={{ $request -> val5 }}';
		window.location.href = url;
	});
	$('.tocreate').on('click', function(){
		$('#indicator').trigger('click');
		var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/historycreate';
		url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
		url += '&pageunit={{ $request->pageunit }}&searchpage={{ $request->searchpage }}';
		url += '&searchsort={{ $request->searchsort }}&searchdirection={{ $request->searchdirection }}';
		url += '&page={{ $request->page }}&sort={{ $request->sort }}&direction={{ $request->direction }}';
		url += '&val1={{ $request->val1 }}&val2={{ $request->val2 }}&val3={{ $request->val3 }}';
		url += '&val4={{ $request->val4 }}&val5={{ $request->val5 }}';
		url += '&val101= {{ $request->val101 }}';
		window.location.href = url;
	});
	$('.toedit').on('click', function(){
		$('#indicator').trigger('click');
		var val201 = $(this).attr('sdate');
		var val202 = $(this).attr('updatedat');
		var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/historyedit';
		url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
		url += '&pageunit={{ $request->pageunit }}&searchpage={{ $request->searchpage }}';
		url += '&searchsort={{ $request->searchsort }}&searchdirection={{ $request->searchdirection }}';
		url += '&page={{ $request->page }}&sort={{ $request->sort }}&direction={{ $request->direction }}';
		url +=  '&val1={{ $request->val1 }}&val2={{ $request->val2 }}&val3={{ $request->val3 }}';
		url +=  '&val4={{ $request->val4 }}&val5={{ $request->val5 }}';
		url +=  '&val101= {{ $request->val101 }}&val201=' + val201;
		url +=  '&val202=' + val202;
		window.location.href = url;
	});
	$('.toshow').on('click', function(){
		$('#indicator').trigger('click');
		var val201 = $(this).attr('sdate');
		var val202 = $(this).attr('updatedat');
		var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/historyshow';
		url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
		url += '&pageunit={{ $request->pageunit }}&searchpage={{ $request->searchpage }}';
		url += '&searchsort={{ $request->searchsort }}&searchdirection={{ $request->searchdirection }}';
		url += '&page={{ $request->page }}&sort={{ $request->sort }}&direction={{ $request->direction }}';
		url +=  '&val1={{ $request->val1 }}&val2={{ $request->val2 }}&val3={{ $request->val3 }}';
		url +=  '&val4={{ $request->val4 }}&val5={{ $request->val5 }}';
		url +=  '&val101= {{ $request->val101 }}&val201=' + val201;
		url +=  '&val202=' + val202;
		window.location.href = url;
	});

	$('.delete').on('click', function(){
		if (!window.confirm('{{ config("message.msg_cmn_if_001") }}')){
			return;
		}
		$('#indicator').trigger('click');
		var val201 = $(this).attr('sdate');
		var val202 = $(this).attr('updatedat');

		$('#val201').val(val201);
		$('#val202').val(val202);
		let pageRedirect = '{{ (count($rows) == 1) ? ((isset($request->page) && $request->page > 1) ? ($request->page - 1) : 1) : (isset($request->page) ? $request->page : 1) }}';
		$('input[name=page]').val(pageRedirect);
		$('#mainform').submit();
	});
});

</script>

<form action="{{ url('/') }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/historydelete" method="POST" id="mainform">
	@csrf
	<input type="hidden" id="kindid" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}">
	<input type="hidden" id="menuid" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}">
	<input type="hidden" id="" name="pageunit" value="{{ $request -> pageunit }}">
	<input type="hidden" id="" name="searchpage" value="{{ $request -> searchpage }}">
	<input type="hidden" id="" name="searchsort" value="{{ $request -> searchsort }}">
	<input type="hidden" id="" name="searchdirection" value="{{ $request -> searchdirection }}">
	<input type="hidden" id="" name="page" value="{{ $request -> page }}">
	<input type="hidden" id="" name="sort" value="{{ $request -> sort }}">
	<input type="hidden" id="" name="direction" value="{{ $request -> direction }}">
	<input type="hidden" id="" name="val1" value="{{ $request -> val1 }}">
	<input type="hidden" id="" name="val2" value="{{ $request -> val2 }}">
	<input type="hidden" id="" name="val3" value="{{ $request -> val3 }}">
	<input type="hidden" id="" name="val4" value="{{ $request -> val4 }}">
	<input type="hidden" id="" name="val5" value="{{ $request -> val5 }}">
	<input type="hidden" id="" name="val101" value="{{ $request -> val101 }}">
	<input type="hidden" id="val201" name="val201" value="">
	<input type="hidden" id="val202" name="val202" value="">
</form>
<div class="row ml-4">
	<div class="col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　人員マスタ履歴表示
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
			<div class="col-xs-1 m-3">
				<button type="button" class="toprev {{ config('system_const.btn_color_back') }}">
				@if (config('system_const.btn_img_back')!='')<i class="{{ config('system_const.btn_img_back') }}"></i>@endif
				{{ config('system_const.btn_char_back') }}
				</button>
			</div>
		</div>

		<div class="row align-items-center">
			@if (!$menuInfo->IsReadOnly)
			<div class="col-xs-1 mt-0 mr-0 mb-3 ml-3">
			<button type="button" name="tocreate" class="tocreate {{ config('system_const.btn_color_new') }}">
				@if (config('system_const.btn_img_new')!='')<i class="{{ config('system_const.btn_img_new') }}"></i>@endif
				{{ config('system_const.btn_char_new') }}
				</button>
			</div>
			@endif
			<div class="col-xs-1 mt-0 mr-3 mb-3 ml-3">
				<button type="button" class="{{ config('system_const.btn_color_readac') }}">
				@if (config('system_const.btn_img_readac')!='')<i class="{{ config('system_const.btn_img_readac') }}"></i>@endif
				{{ config('system_const.btn_char_readac') }}
				</button>
			</div>
			<span>名前: {{ $memberName }}</span>
		</div>

		<div class="row">
			<div class="col-sm-12">
				<table class="table">
					<thead>
						<tr>
							<th class="text-center">@sortablelink('fld1', '開始日')</th>
							<th class="text-center">@sortablelink('fld2', '終了日')</th>
							<th class="text-center">@sortablelink('fld3', '会社名')</th>
							<th class="text-center">@sortablelink('fld4', '所属')</th>
							<th class="text-center"></th>
							<th class="text-center"></th>
						</tr>
					</thead>
					<tbody>
					@foreach($rows as $row)
					<tr>
					<td class="real-space align-middle text-center">{{ Carbon\Carbon::parse($row->fld1)->format('Y/m/d') }}</td>
					<td class="real-space align-middle text-center">{{ is_null($row->fld2) ? '' : Carbon\Carbon::parse($row->fld2)->format('Y/m/d') }}</td>
					<td class="real-space align-middle">{{ is_null($row->fld3) ? '' : $row->fld3 }}</td>
					<td class="real-space align-middle">{{ empty($row->fld4) ? '' : $row->fld4 }}</td>
					<td class="align-middle text-center">
						@if (!$menuInfo->IsReadOnly)
							<button type="button" class="toedit {{ config('system_const.btn_color_rowedit') }}"
							value="" sdate="{{ valueUrlEncode($row->fld1 )}}"
							updatedat="{{ valueUrlEncode($row->Updated_at) }}">
							@if (config('system_const.btn_img_rowedit')!='')<i class="{{ config('system_const.btn_img_rowedit') }}"></i>@endif
							{{ config('system_const.btn_char_rowedit') }}
							</button>
						@else
							<button type="button" class="toshow {{ config('system_const.btn_color_rowinfo') }}"
							value="" sdate="{{ valueUrlEncode($row->fld1 )}}" updatedat="{{ valueUrlEncode($row->Updated_at) }}">
							@if (config('system_const.btn_img_rowinfo')!='')<i class="{{ config('system_const.btn_img_rowinfo') }}"></i>@endif
							{{ config('system_const.btn_char_rowinfo') }}
							</button>
						@endif
					</td>
					<td class="align-middle text-center">
						@if (!$menuInfo->IsReadOnly)
						<button type="button" class="delete {{ config('system_const.btn_color_rowdelete') }}"
							value="" sdate="{{ valueUrlEncode($row->fld1 )}}"
							updatedat="{{ valueUrlEncode($row->Updated_at) }}">
							@if (config('system_const.btn_img_rowdelete')!='')<i class="{{ config('system_const.btn_img_rowdelete') }}"></i>@endif
							{{ config('system_const.btn_char_rowdelete') }}
						</button>
						@endif
					</td>
					</tr>
					@endforeach
					</tbody>
				</table>
				{{ $rows->appends(request()->query())->links() }}
			</div>

		</div>

	</div>
</div>
@endsection
