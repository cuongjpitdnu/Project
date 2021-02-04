@extends('layouts/mainmenu/menu')

@section('content')

<script>
$(function(){
	$('[data-toggle="tooltip"]').tooltip()

	$('#selectdate').datepicker();

	$('#selectdate').change(function() {
		var val1 = $(this).val();
		var val2 = $("#val2").val();
		var val3 = $('select[name=val3] option').filter(':selected').val();
		var val4 = encodeURIComponent($('input[name="val4"]').val());
		var val5 = encodeURIComponent($('input[name="val5"]').val());
		var pageunit = $('[name="pageunit"]').val();
		$('#indicator').trigger('click');
		var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/changedate?'+
		'cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}&val1=' 
		+ val1 + '&val2=' + val2 + '&val3=' + val3 + '&val4=' + val4 + '&val5=' + val5 + '&pageunit=' + pageunit;
		window.location.href = url;
	});

	
	$('.newItem').on('click', function(){
		$('#indicator').trigger('click');
		var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/create';
		url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
		window.location.href = url;
	});
	
	$('.search').on('click', function(){
		var val1 = $('#selectdate').val();
		var val2 = $('input[name="val2"]').val();
		var val3 = $('select[name=val3] option').filter(':selected').val();
		var val4 = encodeURIComponent($('input[name="val4"]').val());
		var val5 = encodeURIComponent($('input[name="val5"]').val());
		var pageunit = $('[name="pageunit"]').val();

		$('#indicator').trigger('click');
		var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/search';
		url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
		url +=  '&val1=' + val1 + '&val2=' + val2 + '&val3=' + val3 + '&val4=' + val4 + '&val5=' + val5 + '&pageunit=' + pageunit;
		url += '&sort=fld2';
		url += '&direction=asc';
		window.location.href = url;
	});

	$('#orgtree')
	.on('activate_node.jstree', function (e, data) {
		var selectedID = data.node.li_attr.item_id;
		$('#select_grp_id').val(selectedID);
		var selectedName = data.node.li_attr.item_name;
		$('#select_grp_name').val(selectedName);
	}).jstree();

	$('#select_org_ok').on('click', function(){
		var val2= $('#select_grp_id').val();
		$('input[name="val2"]').val(val2);
		var grpName = $('#select_grp_name').val();
		$('[name=groupname]').val(grpName);
		$('#org_select_dialog').modal('hide');
	});

	$('.clearorg').on('click', function(){
		$('[name=groupname]').val(null);
		$('input[name="val2"]').val(null);
	});


})

</script>

<input type="hidden" id="val2" name="val2" value="{{ old('val2', $grpID) }}">
<input type="hidden" id="select_grp_id" name="select_grp_id" value="{{ old('select_grp_id', $grpID) }}">
<input type="hidden" id="select_grp_name" name="select_grp_name" value="{{ old('select_grp_name', $grpName) }}">

<div class="row ml-4">

	<div class="col-xs-12">

		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　人員マスタ
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
			<div class="col-xs-3 m-1 pr-5 clearfix">
				<button type="button" name="newItem" class="newItem {{ config('system_const.btn_color_new') }}">
				@if (config('system_const.btn_img_new')!='')<i class="{{ config('system_const.btn_img_new') }}"></i>@endif
				{{ config('system_const.btn_char_new') }}
				</button>
			</div>
			@endif
		</div>
		<div class="row align-items-center">
			<div class="col-xs-3 m-1 pr-5">
				<button type="button" name="search" class="search {{ config('system_const.btn_color_search') }}">
					@if (config('system_const.btn_img_search')!='')<i class="{{ config('system_const.btn_img_search') }}"></i>@endif
					{{ config('system_const.btn_char_search') }}
					</button>
			</div>
		</div>

		<div class="row align-items-center">
			<table class="table table-borderless">
				<tbody>
				<tr>
					<td class="align-middle">基準日：</td>
					<td>
						<input id="selectdate" type="text" maxlength="10" name="val1" size="14" value="{{ old('val1',$request->val1) }}"
						autocomplete="off">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val1'])
						</span>
					</td>
				</tr>
				
				<tr>
					<td class="align-middle">所属：</td>
					<td>
						<input type="text" name="groupname" value="{{ old('groupname', $grpName) }}" readonly="" tabindex="-1" data-toggle="modal" data-target="#org_select_dialog">
						<input type="hidden" name="groupname" value="{{ old('groupname', $grpName) }}">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val2'])
						</span>
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<button type="button" id="select" class="{{ config('system_const.btn_color_file') }}" data-toggle="modal" data-target="#org_select_dialog">
								<i class="{{ config('system_const.btn_img_file') }}"></i>{{ config('system_const.btn_char_file') }}
							</button>
						</span>
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<button type="button" name="clearorg" class="clearorg {{ config('system_const.btn_color_clear') }}">
								<i class="{{ config('system_const.btn_img_clear') }}"></i>{{ config('system_const.btn_char_clear') }}
							</button>
						</span>
					</td>
					@include('mst/org/select', ['mstOrgCommon' => $mstOrgCommon, 'activeOrgID' => $grpID, 'folderOnly' => false ])
				</tr>
	
				<tr>
					<td class="align-middle">外注会社：</td>
					<td>
						<select name="val3">
						<option value="{{valueUrlEncode(0)}}" ></option>
							@foreach ($arrKanrenID as $item)
							<option value="{{ valueUrlEncode($item['id']) }}" @if(valueUrlDecode(old('val3',$request->val3)) === $item['id']) selected @endif>{{ $item['name'] }}</option>
							@endforeach
						</select>
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val3'])
						</span>
					</td>
				</tr>
	
				<tr>
					<td class="align-middle">社員番号：</td>
					<td>
					<input type="text" name="val4" value="{{ old('val4',$request->val4) }}" class="text-right" autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val4'])
						</span>
					</td>
				</tr>
				<tr>
					<td class="align-middle">氏名：</td>
					<td>
						<input type="text" name="val5" value="{{ old('val5',$request->val5) }}" autocomplete="off" maxlength="50">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val5'])
						</span>
					</td>
				</tr>
				<tr>
					<td class="align-middle">表示件数：</td>
					<td>
						<select class="pageunit pageunit-width" name="pageunit">
							<option value= {{ valueUrlEncode(config('system_const.displayed_results_1')) }} {{ (int)valueUrlDecode(old('pageunit',\Request::input('pageunit'))) === config('system_const.displayed_results_1') ? 'selected' : '' }}> {{ config('system_const.displayed_results_1') }} </option>
							<option value= {{ valueUrlEncode(config('system_const.displayed_results_2')) }} {{ (int)valueUrlDecode(old('pageunit',\Request::input('pageunit'))) === config('system_const.displayed_results_2') ? 'selected' : '' }}> {{ config('system_const.displayed_results_2') }} </option>
							<option value= {{ valueUrlEncode(config('system_const.displayed_results_3')) }} {{ (int)valueUrlDecode(old('pageunit',\Request::input('pageunit'))) === config('system_const.displayed_results_3') ? 'selected' : '' }}> {{ config('system_const.displayed_results_3') }} </option>
						</select>
						※1ページあたり
					</td>
				</tr>
				</tbody>
			</table>
		</div>

	</div>
</div>

@endsection
