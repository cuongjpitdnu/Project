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
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/cancel';
			$('#mainform').attr('action', url);
			$('#indicator').trigger('click');
			$('#mainform').submit();
		});
	});
</script>
<form action="{{ url('/') }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/accept" method="POST" id="mainform">
	@csrf
	<input type="hidden" id="kindid" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}">
	<input type="hidden" id="menuid" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}">
	<input type="hidden" id="" name="val1" value="{{ $request->val1 }}">
	<input type="hidden" id="" name="val2" value="{{ $request->val2 }}">
	<input type="hidden" id="" name="val3" value="{{ $request->val3 }}">
	<input type="hidden" id="" name="val4" value="{{ $request->val4 }}">
	<div class="row ml-4">
		<div class="col-xs-12">
			<div class="row align-items-center">
				<div class="col-xs-1 text-left m-2 p-2 rounded border">
					■　搭載日程展開 - 詳細
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
					<div class="info-circle"><i class="fas fa-info icon-small"></i></div>

					現在の中日程との相違点です。よろしければ、保存ボタンを押してください。
					<table class="table">
						<tbody>
							<tr class="set-color">
								<th class="text-center">@sortablelink('fld1', trans('schemImport.sortable.fld1'))</th>
								<th class="text-center">@sortablelink('fld2', trans('schemImport.sortable.fld2'))</th>
								<th class="text-center">@sortablelink('fld3', trans('schemImport.sortable.fld3'))</th>
								<th class="text-center">@sortablelink('fld4', trans('schemImport.sortable.fld4'))</th>
							</tr>
							@foreach($rows as $row)
							<tr>
								<td class="real-space align-middle">{{ $row['fld1'] }}</td>
								<td class="real-space align-middle">{{ $row['fld2'] }}</td>
								<td class="real-space align-middle">{{ $row['fld3'] }}</td>
								<td class="real-space align-middle">{{ $row['fld4'] }}</td>
							</tr>
							@endforeach
						</tbody>
					</table>
				</div>
			</div>
			<div class="row">
				<div class="col-md-5">
					<button type="button" id="save" class="{{ config('system_const.btn_color_save') }}">
						<i class="{{ config('system_const.btn_img_save') }}"></i>{{ config('system_const.btn_char_save') }}
					</button>&emsp;
					<button type="button" id="cancel" class="{{ config('system_const.btn_color_cancel') }}">
						<i class="{{ config('system_const.btn_img_cancel') }}"></i>{{ config('system_const.btn_char_cancel') }}
					</button>
				</div>
				<div class="col-md-7">
					<div class="col-xs-1 p-1"> {{ $rows->appends(request()->query())->links() }} </div>
					@include('layouts/heartbeat/heartbeat', ['sysKindID' => $menuInfo->KindID,
								'sysMenuID' => config('system_const_schem.sys_menu_id_plan'),'optionKey' =>valueUrlDecode($request->val2)])
				</div>
			</div>
		</div>
	</div>
</form>
@endsection