@extends('layouts/mainmenu/menu')
@section('content')
<script>
$(function(){
	$('.toprev').on('click', function(){
		$('#indicator').trigger('click');
		var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/index';
		url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
		url +=  '&val1={{ $request -> val1 }}&val2={{ $request -> val2 }}&val3={{ $request -> val3 }}';
		url +=  '&val4={{ $request -> val4 }}&val5={{ $request -> val5 }}&pageunit={{ $request -> pageunit }}';
		window.location.href = url;
	});
	$('.toedit').on('click', function(){
		$('#indicator').trigger('click');
		var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/edit';
		url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
		url += '&page={{ $request->page }}&pageunit={{ $request->pageunit }}';
		url += '&sort={{ $request->sort }}';
		url += '&direction={{ $request->direction }}';
		url +=  '&val1={{ $request -> val1 }}&val2={{ $request -> val2 }}&val3={{ $request -> val3 }}';
		url +=  '&val4={{ $request -> val4 }}&val5={{ $request -> val5 }}';
		url +=  '&val101='+ $(this).val() +'&val102='+ $(this).attr("updatedat");
		window.location.href = url;
	});

	$('.toshow').on('click', function(){
		$('#indicator').trigger('click');
		var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/show';
		url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
		url += '&pageunit={{ $request->pageunit }}&page={{ $request->page }}';
		url += '&sort={{ $request->sort }}';
		url += '&direction={{ $request->direction }}';
		url +=  '&val1={{ $request -> val1 }}&val2={{ $request -> val2 }}&val3={{ $request -> val3 }}';
		url +=  '&val4={{ $request -> val4 }}&val5={{ $request -> val5 }}';
		url +=  '&val101='+ $(this).val();
		window.location.href = url;
	});

	$('.tohistory').on('click', function(){
		$('#indicator').trigger('click');
		var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/history';
		url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
		url += '&searchpage={{ $request->page }}&pageunit={{ $request->pageunit }}';
		url += '&searchsort={{ $request->sort }}';
		url += '&searchdirection={{ $request->direction }}';
		url +=  '&val1={{ $request -> val1 }}&val2={{ $request -> val2 }}&val3={{ $request -> val3 }}';
		url +=  '&val4={{ $request -> val4 }}&val5={{ $request -> val5 }}';
		url +=  '&val101='+ $(this).val();
		url += '&sort=fld1';
		url += '&direction=desc';
		window.location.href = url;
	});
});

</script>
<div class="row ml-4">
	<div class="col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　人員マスタ検索結果
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

		<div class="row">
			<div class="col-sm-12">
				<table class="table">
					<thead>
						<tr>
							<th class="text-center">@sortablelink('fld1', '社員番号')</th>
							<th class="text-center">@sortablelink('fld2', '名前')</th>
							<th class="text-center">@sortablelink('fld3', '所属')</th>
							<th class="text-center">@sortablelink('fld4', '会社名')</th>
							<th class="text-center"></th>
							<th class="text-center"></th>
						</tr>
					</thead>
					<tbody>
					@foreach($rows as $row)
					<tr>
					<td class="real-space align-middle text-right">{{ is_null($row->fld1) ? '' : $row->fld1 }}</td>
					<td class="real-space align-middle">{{ $row->fld2 }}</td>
					<td class="real-space align-middle">{{ empty($row->fld3) ? '' : $row->fld3 }}</td>
					<td class="real-space align-middle">{{ is_null($row->fld4) ? '' : $row->fld4 }}</td>
					<td class="align-middle text-center">
						@if (!$menuInfo->IsReadOnly)
							<button type="button" class="toedit {{ config('system_const.btn_color_rowedit') }}"
							value="{{ valueUrlEncode($row->ID) }}" updatedat="{{ valueUrlEncode($row->Updated_at) }}"  >
							@if (config('system_const.btn_img_rowedit')!='')<i class="{{ config('system_const.btn_img_rowedit') }}"></i>@endif
							{{ config('system_const.btn_char_rowedit') }}
							</button>
						@else
							<button type="button" class="toshow {{ config('system_const.btn_color_rowinfo') }}"
							value="{{ valueUrlEncode($row->ID) }}">
							@if (config('system_const.btn_img_rowinfo')!='')<i class="{{ config('system_const.btn_img_rowinfo') }}"></i>@endif
							{{ config('system_const.btn_char_rowinfo') }}
							</button>
						@endif
					</td>
					<td class="align-middle text-center">
						<button type="button" class="tohistory {{ config('system_const.btn_color_rowhistory') }}"
							value="{{ valueUrlEncode($row->ID) }}">
							@if (config('system_const.btn_img_rowhistory')!='')<i class="{{ config('system_const.btn_img_rowhistory') }}"></i>@endif
							{{ config('system_const.btn_char_rowhistory') }}
						</button>
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
