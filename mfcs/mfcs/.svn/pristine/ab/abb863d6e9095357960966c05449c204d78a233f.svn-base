@extends('layouts/mainmenu/menu')

@section('content')

<script>
$(function(){
	$('[data-toggle="tooltip"]').tooltip()

	$('#selectdate').datepicker();

	$('#selectdate').change(function() {
		var val1 = $('#activeid').val();
		var val2 = $(this).val();
		var val3 = $('#sdate').val();
		$('#indicator').trigger('click');
		window.location.href = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/changedate?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}&val1=' + val1 + '&val2=' + val2 + '&val3=' + val3;
	});

	$('#orgtree')
	.on('activate_node.jstree', function (e, data) {
		var val1 = data.node.li_attr.item_id;
		$('#activeid').val(val1);
		var val3 = data.node.li_attr.s_date;
		$('#sdate').val(val3);
	}).jstree();

	$('li[name="orgitem"]').on('dblclick', function(){
		var val1 = $('#activeid').val();
		var val3 = $('#sdate').val();
		<?php
		if($menuInfo->IsReadOnly) {
			$action = 'show';
		}
		else {
			$action = 'edit';
		}
		?>
		$('#indicator').trigger('click');
		window.location.href = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/{{ $action }}?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}&val1=' + val1 + '&val2={{ $baseDate }}' + '&val3=' + val3;
	});

	$('#show').on('click', function(){
		var val1 = $('#activeid').val();
		var val3 = $('#sdate').val();
		$('#indicator').trigger('click');
		window.location.href = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/show?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}&val1=' + val1 + '&val2={{ $baseDate }}' + '&val3=' + val3;
	});

	$('#create').on('click', function(){
		var val1 = $('#activeid').val();
		var val3 = $('#sdate').val();
		$('#indicator').trigger('click');
		window.location.href = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/create?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}&val1=' + val1 + '&val2={{ $baseDate }}' + '&val3=' + val3;
	});

	$('#edit').on('click', function(){
		var val1 = $('#activeid').val();
		var val3 = $('#sdate').val();
		$('#indicator').trigger('click');
		window.location.href = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/edit?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}&val1=' + val1 + '&val2={{ $baseDate }}' + '&val3=' + val3;
	});

	$('#delete').on('click', function(){
		var activeidValue = $('#activeid').val();
		if(activeidValue == ''){
			if('{{ $errors->has("val1") }}' == false){
				$('#org_select_error').removeClass('d-none');
			}
			return;
		}
		var activeHasChild = $('#active_has_child').val();
		if (!window.confirm('{{ config("message.msg_cmn_if_001") }}')){
			return;
		}
		var hasChild = false;
		var hasNext = false;
		var checkDeleteJson = null;

		checkDeleteJson = {
			"cmn1": '{{ valueUrlEncode($menuInfo->KindID) }}',
			"cmn2": '{{ valueUrlEncode($menuInfo->MenuID) }}',
			"val1": $('#activeid').val(),
			"val2": '{{ $baseDate }}',
			"val3": $('#sdate').val(),
		};
		$.ajaxSetup({
			headers: {'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')}
		});
		$.ajax({
			type:'POST',
			url:'{{ url("/") }}/mst/org/checkdelete',
			dataType:'json',
			contentType: "application/json",
			data:JSON.stringify(checkDeleteJson),
			beforeSend : function(){
				$('#indicator').trigger('click');
			},
		}).done(function (response) {
			indicatorHide();
			var message = response.message;
			if(response.status == '{{ config("system_const.json_status_ng") }}'){
				hasNext = true;
			}
			if(response.has_child == '{{ config("system_const.json_return_true") }}'){
				hasChild = true;
			}
			if(message != null){
				window.alert(message);
				return window.location.href = '{{ url("/") }}/index';
			}
			var msg = null;
			if(hasChild == true && hasNext == false){
				msg = '{{ config("message.msg_org_if_001") }}'
			}
			else if(hasChild == false && hasNext == true){
				msg = '{{ config("message.msg_org_if_002") }}'
			}
			else if(hasChild == true && hasNext == true){
				msg = '{{ config("message.msg_org_if_003") }}'
			}
			if(msg != null){
				if (!window.confirm(msg)){
					return;
				}
			}
			var val1 = $('#activeid').val();
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/delete';
			$('#mainform').attr('action', url);
			$('#indicator').trigger('click');
			$('#mainform').submit();
		}).fail(function(xhr, status, error) {
			indicatorHide();
			window.location.href = '{{ url("/") }}/errors/500';
		});
	});
})

</script>

@include('layouts/heartbeat/heartbeat', ['sysKindID' => $menuInfo->KindID, 'sysMenuID' => $menuInfo->MenuID, 'optionKey' => config('system_const.lock_option_key_general')])

<input type="hidden" id="active_has_child" value="{{ valueUrlEncode($activeHasChild) }}">

<form action="{{ url('/') }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/index" method="POST" id="mainform">
	@csrf
	<input type="hidden" id="kindid" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}">
	<input type="hidden" id="menuid" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}">
	<input type="hidden" id="activeid" name="val1" value="{{ valueUrlEncode($activeOrgID) }}">
	<input type="hidden" id="basedate" name="val2" value="{{ $baseDate }}">
	<input type="hidden" id="sdate" name="val3" value="{{ valueUrlEncode($activeSDate) }}">
</form>

<div class="row ml-4">

	<div class="col-xs-12">

		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　職制マスタ
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
			<div class="col-xs-3 m-1 pr-5 clearfix">
				<input id="selectdate" type="text" maxlength="10" size="14" value="{{ date('Y/m/d', strtotime($baseDate)) }}">
				@include('layouts/error/item', ['name' => 'val2'])
			</div>
			<div class="col-xs-3 m-1 pr-5">

			</div>
			@if ($menuInfo->IsReadOnly)
			<div class="col-xs-1 m-1">
				<button id="show" type="button" class="{{ config('system_const.btn_color_info') }}">@if (config('system_const.btn_img_info')!='')<i class="{{ config('system_const.btn_img_info') }}"></i>@endif{{ config('system_const.btn_char_info') }}</button>
			</div>
			@else
			<div class="col-xs-1 m-1">
				<button id="create" type="button" class="{{ config('system_const.btn_color_new') }}">@if (config('system_const.btn_img_new')!='')<i class="{{ config('system_const.btn_img_new') }}"></i>@endif{{ config('system_const.btn_char_new') }}</button>
			</div>
			<div class="col-xs-1 m-1">
				<button id="edit" type="button" class="{{ config('system_const.btn_color_edit') }}">@if (config('system_const.btn_img_edit')!='')<i class="{{ config('system_const.btn_img_edit') }}"></i>@endif{{ config('system_const.btn_char_edit') }}</button>
			</div>
			<div class="col-xs-1 m-1">
				<button id="delete" type="button" class="{{ config('system_const.btn_color_delete') }}">@if (config('system_const.btn_img_delete')!='')<i class="{{ config('system_const.btn_img_delete') }}"></i>@endif{{ config('system_const.btn_char_delete') }}</button>
			</div>
			@endif
		</div>

		<div class="row">
			<div class="col border border-dark">

				@include('mst/org/tree', ['mstOrgCommon' => $mstOrgCommon, 'activeOrgID' => $activeOrgID, 'folderOnly' => false])

			</div>
			@include('layouts/error/item', ['name' => 'val1'])
			@include('layouts/error/item', ['id' => 'org_select_error', 'hidden' => true, 'rule' => 'required', 'ruleJpName' => '職制'])
		</div>

	</div>
</div>

@endsection
