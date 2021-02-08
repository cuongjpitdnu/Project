@extends('layouts/mainmenu/menu')
@section('content')
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();
		$('#save').on('click', function(){
			$('#indicator').trigger('click');
			$('#mainform').submit();
		});
		$('#cancel').on('click', function(){
			$('#indicator').trigger('click');
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/';
			url += 'index?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&val1={{ $request->val1 }}';
			url += '&val2={{ $request->val2 }}';
			url += '&val5={{ $request->val5 }}';
			window.location.href = url;
		});
	});
</script>

<form action="{{ url('/') }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/save" method="POST" id="mainform">
	@csrf
	<input type="hidden" id="kindid" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}">
	<input type="hidden" id="menuid" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}">
	<input type="hidden" name="page" value="{{ $request->page }}" />
	<input type="hidden" name="sort" value="{{ $request->sort }}" />
	<input type="hidden" name="direction" value="{{ $request->direction }}" />
	<input type="hidden" id="" name="val1" value="{{ $request->val1 }}">
	<input type="hidden" id="" name="val2" value="{{ $request->val2 }}">
	<input type="hidden" id="" name="val5" value="{{ $request->val5 }}">
	<input type="hidden" id="" name="val6" value="{{ $request->val6 }}">
	<div class="row ml-4">
		<div class="col-xs-12">
			<div class="row align-items-center">
				<div class="col-xs-1 text-left m-2 p-2 rounded border">
					■　日程表取込登録
				</div>
			</div>
			
			@if (isset($originalError) && count($originalError))
			<div class="row">
				<div class="col-xs-12">
					<div class="alert alert-danger">
						<ul>
							@foreach ($originalError as $item)
							<li>{{ $item }}</li>
							@endforeach
						</ul>
					</div>
				</div>
			</div>
			@endif

			<div class="row">
				<div class="col-sm-12">
					<div class="info-circle"><i class="fas fa-info icon-small"></i></div>{{ count($rows) === 0 ? ' 現在の日程表との相違点はありませんでした。' : ' 現在の日程表との相違点です。よろしければ、保存ボタンを押してください。' }} 
					<table class="table table-row table-import">
						<tbody>
							<tr class="set-color">
								<th class="text-center">@sortablelink('fld1', '区分')</th>
								<th class="text-center">@sortablelink('fld2', 'ブロック名')</th>
								<th class="text-center">@sortablelink('fld3', '組区')</th>
								<th class="text-center">@sortablelink('fld4', '内容')</th>
							</tr>

							@foreach($rows as $row)
							<tr>
								<td class="real-space align-middle">{{ $row['fld1'] }}</td>
								<td class="real-space align-middle">{{ $row['fld2'] }}</td>
								<td class="real-space align-middle text-right">{{ $row['fld3'] }}</td>
								<td class="real-space align-middle">{{ $row['fld4'] }}</td>
							</tr>
							@endforeach
						</tbody>
					</table>   
				</div>
			</div>
			<div class="row">
				@if($datatype == 'ok')
				{{ $rows->appends(request()->query())->links() }}
				@endif
			</div>
			<div class="row">
				@if($datatype == 'ok' && count($rows) > 0)
				<div class="col-xs-1 p-1">
					<button type="button" id="save" class="{{ config('system_const.btn_color_save') }}">
						<i class="{{ config('system_const.btn_img_save') }}"></i>{{ config('system_const.btn_char_save') }}
					</button>
				</div>
				@endif
				<div class="col-xs-1 p-1">
					<button type="button" id="cancel" class="{{ config('system_const.btn_color_cancel') }}">
						<i class="{{ config('system_const.btn_img_cancel') }}"></i>{{ config('system_const.btn_char_cancel') }}
					</button> 
				</div>
			</div>
		</div>
	</div>
</form>
@endsection