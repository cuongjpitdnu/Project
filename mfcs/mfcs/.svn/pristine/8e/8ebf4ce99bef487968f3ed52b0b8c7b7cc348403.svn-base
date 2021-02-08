@extends('layouts/mainmenu/menu')
@section('content')
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip()

		$('#edit').on('click', function(e) {
			$('#indicator').trigger('click');
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/manage';
			url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&pageunit=' + $('[name=pageunit]').val();
			url += '&val1=' + $('input[name=val1]').val();
			window.location.href = url;
		});

		$('#orgtree')
		.on('activate_node.jstree', function (e, data) {
			var selectedID = data.node.li_attr.item_id;
			$('#select_org_id').val(selectedID);
			var selectedName = data.node.li_attr.item_name;
			$('#select_org_name').val(selectedName);
		}).jstree();

		$('#select_org_ok').on('click', function(){
			var val1= $('#select_org_id').val();
			$('input[name="val1"]').val(val1);
			var orgName = $('#select_org_name').val();
			$('[name=orgname]').val(orgName);
			$('#org_select_dialog').modal('hide');
		});
	});
</script>

<input type="hidden" name="val1" value="{{ old('val1', $val1) }}">
<input type="hidden" id="select_org_id" name="select_org_id" value="{{ old('select_org_id', $val1) }}">
<input type="hidden" id="select_org_name" name="select_org_name" value="{{ old('select_org_name', $orgName) }}">
<div class="row ml-2 mr-2">
	<div class="col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　展開パターンマスタ
			</div>
		</div>

		<div class="row align-items-center ml-1 mt-3">
			<table class="table table-borderless">
				<tr>
					<td class="align-middle">所有課係：</td>
					<td>
						<input type="text" name="orgname" value="{{ old('orgname', $orgName) }}"
							readonly="" tabindex="-1" data-toggle="modal" data-target="#org_select_dialog">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<button type="button" id="create" class="{{ config('system_const.btn_color_file') }}"
								data-toggle="modal" data-target="#org_select_dialog">
								<i class="{{ config('system_const.btn_img_file') }}"></i>{{ config('system_const.btn_char_file') }}
							</button>
						</span>
					</td>
					@include('mst/org/select', ['mstOrgCommon' => $mstOrgCommon, 'activeOrgID' => 0, 'folderOnly' => false ])
				</tr>
				<tr>
					<td class="align-middle">表示件数：</td>
					<td>
						<select class="pageunit pageunit-width" name="pageunit">
							<option value= {{ config('system_const.displayed_results_1') }}
								{{ (int)old('pageunit',\Request::input('pageunit')) === config('system_const.displayed_results_1') ? 'selected' : '' }}>
								{{ config('system_const.displayed_results_1') }} </option>
							<option value= {{ config('system_const.displayed_results_2') }}
								{{ (int)old('pageunit',\Request::input('pageunit')) === config('system_const.displayed_results_2') ? 'selected' : '' }}>
								{{ config('system_const.displayed_results_2') }} </option>
							<option value= {{ config('system_const.displayed_results_3') }}
								{{ (int)old('pageunit',\Request::input('pageunit')) === config('system_const.displayed_results_3') ? 'selected' : '' }}>
								{{ config('system_const.displayed_results_3') }} </option>
						</select>
						※1ページあたり
					</td>
				</tr>
			</table>
		</div>

		<div class="row ml-1">
			<div class="col-xs-1 p-1">
				<button type="button" id="edit" class="{{ config('system_const.btn_color_ok') }}">
					<i class="{{ config('system_const.btn_img_ok') }}"></i>{{ config('system_const.btn_char_ok') }}
				</button>
			</div>
		</div>
	</div>
</div>
@endsection