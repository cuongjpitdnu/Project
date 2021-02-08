<script>

$(function(){

	$('[data-toggle="tooltip"]').tooltip()

	$('#orgtree')
	.on('activate_node.jstree', function (e, data) {
		var selectedID = data.node.li_attr.item_id;
		$('#select_org_id').val(selectedID);
		var selectedName = data.node.li_attr.item_name;
		$('#select_org_name').val(selectedName);
	}).jstree();

	$('#select_org_ok').on('click', function(){
		var val2 = $('#select_org_id').val();
		$("input[name='val2']").val(val2);
		var orgName = $('#select_org_name').val();
		$('[name=orgname]').val(orgName);
		$('#org_select_dialog').modal('hide');
	});

	$('#clear').on('click', function(){
		$("input[name='val2']").val("{{ valueUrlEncode(0) }}");
		$('[name=orgname]').val("{{ config('system_const.org_null_name') }}");
	});

	$('#save').on('click', function(){
		$('#indicator').trigger('click');
		$('#mainform').submit();
	});

	$('#cancel').on('click', function(){
		$('#indicator').trigger('click');
		var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/index';
		url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}';
		url += '&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
		url += '&page={{ $request->page }}';
		url += '&pageunit={{ $request->pageunit }}';
		url += '&sort={{ $request->sort }}';
		url += '&direction={{ $request->direction }}';
		window.location.href = url;
	});

	$('.selectdate').datepicker();
})

</script>

<input type="hidden" id="select_org_id" value="{{ valueUrlEncode(0) }}">
<input type="hidden" id="select_org_name" value="{{ config('system_const.org_null_name') }}">

<div class="row ml-4">
	<div class="col-xs-12">

		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　能力時間マスタ@if($target == 'show')参照@elseif($target == 'create')登録@elseif($target == 'edit')更新@endif
			</div>
		</div>

		<div id="error">
		</div>

		@if (isset($originalError) && count($originalError))
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
		
		<form action="{{ url('/') }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/save" method="POST" id="mainform">
			@csrf
			<input type="hidden" name="method" value="{{ $target }}">
			<input type="hidden" id="kindid" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}">
			<input type="hidden" id="menuid" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}">
			<input type="hidden" name="page" value="{{ $request->page }}">
			<input type="hidden" name="pageunit" value="{{ $request->pageunit }}">
			<input type="hidden" name="sort" value="{{ $request->sort }}">
			<input type="hidden" name="direction" value="{{ $request->direction }}">
			<input type="hidden" name="val2" value="{{ old('val2', valueUrlEncode(@$abilityData['GroupID'])) }}">
			@if ($target == 'edit')
			<input type="hidden" name="val8" value="{{ old('val8', $abilityData['Updated_at']) }}">
			<input type="hidden" name="val10" value="{{ valueUrlEncode($abilityData['ID']) }}">
			@endif

			<table class="table table-borderless">
				<tbody>
					<tr>
						<td>能力時間名称 *：</td>
						<td>
							<input type="text" name="val1" maxlength="100" value="{{ old('val1', @$abilityData['AbilityName']) }}" {{ $target == "show" || $target == "edit" ? 'disabled="disabled"' : '' }} autocomplete="off">
							@if($target == "edit")
							<input type="hidden" name="val1" value="{{ old('val1', @$abilityData['AbilityName']) }}">
							@endif
						</td>
						<td style="padding:0px; vertical-align: middle;">
							<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val1'])
							</span>
						</td>

						<td>職制：</td>
						<td>
							<input type="text" name="orgname" value="{{ old('orgname', @$abilityData['GroupName']) }}" {{ $target == "show" || $target == "edit" ? 'disabled="disabled"' : '' }} tabindex="-1" data-toggle="modal" data-target="#org_select_dialog">
							<input type="hidden" name="orgname" value="{{ old('orgname', @$abilityData['GroupName']) }}">
						</td>
						<td style="padding:0px; vertical-align: middle;">
							<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val2'])
							</span>
						</td>

						@if ($target == 'create')
						<td style="padding:0px; vertical-align: middle;">
						<span class="col-xs-1 p-1">
							<button type="button" id="select" class="{{ config('system_const.btn_color_file') }}" data-toggle="modal" data-target="#org_select_dialog">
								<i class="{{ config('system_const.btn_img_file') }}"></i>{{ config('system_const.btn_char_file') }}
							</button>
						</span>
						</td>
						<td style="padding:0px; vertical-align: middle;">
						<span class="col-xs-1 p-1">
							<button type="button" id="clear" class="{{ config('system_const.btn_color_clear') }}">
								<i class="{{ config('system_const.btn_img_clear') }}"></i>{{ config('system_const.btn_char_clear') }}
							</button>
						</span>
						</td>
						@include('mst/org/select', ['mstOrgCommon' => $mstOrgCommon, 'activeOrgID' => 0])
						@endif
					</tr>
					<tr>
						<td>施工棟：</td>
						<td>
							@if ($target == 'create' || $target == 'edit')
							<select name="val3" {{ $target == "show" || $target == "edit" ? 'disabled="disabled"' : '' }}>
								@if ($target == 'create')
								<option value="" selected></option>
								@foreach ($Floor as $value)
								<option value="{{ $value['Code'] }}" {{ (is_null(old('val3')) ? @$abilityData['FloorCode'] : old('val3')) == $value['Code'] ? 'selected' : '' }}>{{ $value['Name'] }}</option>
								@endforeach
								@endif

								@if ($target == 'edit')
								<option value="{{ $abilityData['FloorCode'] }}">{{ $abilityData['FloorName'] }}</option>
								@endif
							</select>
							@if($target == "edit")
							<input type="hidden" name="val3" value="{{ old('val3', @$abilityData['FloorCode']) }}">
							@endif

							@elseif($target == 'show')
							<input type="text" name="val3" value="{{ $abilityData['FloorCode'] }}" disabled="disabled">
							@endif
						</td>
						<td style="padding:0px; vertical-align: middle;">
							<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val3'])
							</span>
						</td>

						<td>職種：</td>
						<td>
							@if ($target == 'create' || $target == 'edit')
							<select name="val4" {{ $target == "show" || $target == "edit" ? 'disabled="disabled"' : '' }}>
								@if ($target == 'create')
								<option value="" selected></option>
								@foreach ($Dist as $value)
								<option value="{{ $value['Code'] }}" {{ (is_null(old('val4')) ? @$abilityData['DistCode'] : old('val4').(mb_strlen(old('val4')) <= 5 ? str_repeat(' ', 5 - mb_strlen(old('val4'))) : '')) == $value['Code'] ? 'selected' : '' }}>{{ $value['Name'] }}</option>
								@endforeach
								@endif

								@if ($target == 'edit')
								<option value="{{ $abilityData['DistCode'] }}">{{ $abilityData['DistName'] }}</option>
								@endif
							</select>
							@if($target == "edit")
							<input type="hidden" name="val4" value="{{ old('val4', @$abilityData['DistCode']) }}">
							@endif

							@elseif($target == 'show')
							<input type="text" name="val4" value="{{ $abilityData['DistCode'] }}" disabled="disabled">
							@endif
						</td>
						<td style="padding:0px; vertical-align: middle;">
							<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val4'])
							</span>
						</td>
					</tr>
					<tr>
						<td>開始日 *：</td>
						<td>
							<input type="text" name="val5" maxlength="10" value="{{ old('val5', @$abilityData['Sdate']) }}" {{ $target == "show" ? 'disabled="disabled"' : '' }} class="selectdate" autocomplete="off">
						</td>
						<td style="padding:0px; vertical-align: middle;">
							<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val5'])
							</span>
						</td>

						<td>終了日：</td>
						<td>
							<input type="text" name="val6" maxlength="10" value="{{ old('val6', @$abilityData['Edate']) }}" {{ $target == "show" ? 'disabled="disabled"' : '' }} class="selectdate" autocomplete="off">
						</td>
						<td style="padding:0px; vertical-align: middle;">
							<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val6'])
							</span>
						</td>
					</tr>
					<tr>
						<td>工数：</td>
						<td>
							<input type="text" name="val7" maxlength="9" value="{{ old('val7', @$abilityData['Hr']) }}" {{ $target == "show" ? 'disabled="disabled"' : '' }} autocomplete="off">
						</td>
						<td style="padding:0px; vertical-align: middle;">
							<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val7'])
							</span>
						</td>
					</tr>
				</tbody>
			</table>
		</form>

		<div class="row">
			@if($target == 'create' || $target == 'edit')
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