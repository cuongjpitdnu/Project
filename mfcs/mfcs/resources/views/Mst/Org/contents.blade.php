<script>
$(function(){
	$('[data-toggle="tooltip"]').tooltip();

	$('#save').on('click', function(){
		$('#indicator').trigger('click');

		if($('select[name=tempVal107]').is(':disabled')) {
			$('input[name=val107]').val(0);
		} else {
			var tempValue = $('select[name=tempVal107]').val();
			$('input[name=val107]').val(tempValue);

		}

		$('input[name=val112]').val($('input[name=tempVal112]').val());
		$('input[name=val113]').val($('input[name=tempVal113]').val());

		$('#mainform').submit();
	});

	$('#cancel').on('click', function(){
		$('#indicator').trigger('click');
		window.location.href = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/index?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}&val1={{ $request->val1 }}&val2={{ $request->val2 }}&val3={{ $request->val3 }}';
	});

	$('#orgtree')
	.on('activate_node.jstree', function (e, data) {
		var selectedID = data.node.li_attr.item_id;
		$('#select_parent_org_id').val(selectedID);
		var selectedName = data.node.li_attr.item_name;
		$('#select_parent_org_name').val(selectedName);
	}).jstree();

	$('#select_org_ok').on('click', function(){
		var val101 = $('#select_parent_org_id').val();
		$('#val101').val(val101);
		var orgName = $('#select_parent_org_name').val();
		$('[name=parent_org_name]').val(orgName);
		$('#org_select_dialog').modal('hide');
	});

	$('.clearorg').on('click', function(){
		$('[name=parent_org_name]').val("{{ config('system_const.org_root_name') }}");
		$('input[name="val101"]').val("{{ valueUrlEncode(0) }}");
	});

	$('.selectdate').datepicker();

	var arrKanren = fncJsonParse('{{ json_encode($arrKanren) }}');
	$('select[name=val105]').on('change', function () {
		$('select[name=tempVal107]').empty().append('<option value="0" selected="selected"></option>');
		if(this.value == 2){
			$('select[name="tempVal107"]').removeAttr('disabled', 'disabled');
			arrKanren.forEach(element => {
				var selectValue = (element.id == $('input[name=val107]').val()) ? 'selected' : '';
				$('select[name=tempVal107]').append('<option value="'+ element.id +'" '+ selectValue +'>'+  convertHTML(element.name) +'</option>');
			});
		}else{
			$('select[name="tempVal107"]').attr('disabled', 'disabled');
		}
	}).trigger('change');

	$('.input-checkbox').click(function(){
		if($(this).prop('checked')){
			$('[name="'+$(this).attr('checkbox')+'"]').val(1);
		}else{
			$('[name="'+$(this).attr('checkbox')+'"]').val(0);
		}
	});

})

</script>

@if($target === 'create' || $target === 'edit')
	@include('layouts/heartbeat/heartbeat', ['sysKindID' => $menuInfo->KindID, 'sysMenuID' => $menuInfo->MenuID, 'optionKey' => config('system_const.lock_option_key_general')])
@endif

<input type="hidden" id="select_parent_org_id" value="{{ valueUrlEncode(@$mstOrg['val101']) }}">
<input type="hidden" id="select_parent_org_name" value="{{ $parentName }}">

<div class="row ml-4">
	<div class="col-xs-12">

		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　職制マスタ@if($target === 'show')参照@elseif($target === 'create')登録@elseif($target === 'edit')更新@endif
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

		<form action="{{ url('/') }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/save" method="POST" id="mainform">
			@csrf
			<input type="hidden" name="method" value="{{ $target }}">
			<input type="hidden" id="kindid" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}">
			<input type="hidden" id="menuid" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}">
			<input type="hidden" id="activeid" name="val1" value="{{ $request->val1 }}">
			<input type="hidden" id="basedate" name="val2" value="{{ $request->val2 }}">
			<input type="hidden" id="" name="val3" value="{{ $request->val3 }}">
			<input type="hidden" id="val101" name="val101" value="{{ valueUrlEncode(@$mstOrg['val101']) }}">

			@if ($target === 'edit')
			<input type="hidden" id="" name="val112" value="{{ (@$mstOrg['val112']) }}">
			<input type="hidden" id="" name="val113" value="{{ (@$mstOrg['val113']) }}">
			<input type="hidden" id="" name="val114" value="{{ valueUrlEncode(@$mstOrg['val114']) }}">
			<input type="hidden" id="" name="val115" value="{{ valueUrlEncode(@$mstOrg['val115']) }}">
			@endif

			<table class="table table-borderless">
				<tbody>
				<tr>
					<td class="align-middle">親職制 *：</td>
					<td>
						<input type="text" {{ $target === "show" ? 'disabled="disabled"' : '' }} name="parent_org_name" 
						value="{{ old('parent_org_name', $parentName) }}" readonly="" tabindex="-1" data-toggle="modal" data-target="#org_select_dialog">
						<input type="hidden" name="parent_org_name" value="{{ old('parent_org_name', $parentName) }}">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						@include('layouts/error/item', ['name' => 'val101'])
						</span>
					</td>
					@if ($target === 'create' || $target === 'edit')
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
					@include('mst/org/select', ['mstOrgCommon' => $mstOrgCommon, 'activeOrgID' => $mstOrg['val101']])
					@endif

					<td class="align-middle">職制コード：</td>
					<td>
						<input type="text" name="val102" value="{{ old('val102', @$mstOrg['val102']) }}"
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="6">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						@include('layouts/error/item', ['name' => 'val102'])
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">職制名 *：</td>
					<td>
						<input type="text" name="val103" value="{{ old('val103', @$mstOrg['val103']) }}"
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="50">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						@include('layouts/error/item', ['name' => 'val103'])
						</span>
					</td>
					@if ($target === 'create' || $target === 'edit')
					<td></td>
					<td></td>
					@endif
					<td class="align-middle">略称 *：</td>
					<td>
						<input type="text" name="val104" value="{{ old('val104', @$mstOrg['val104']) }}"
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="50">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						@include('layouts/error/item', ['name' => 'val104'])
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">社内外 *：</td>
					<td>
						<select name="val105" {{ $target === "show" ? 'disabled="disabled"' : '' }}>
							<option value="1" {{ (int)old('val105', @$mstOrg['val105']) === 1 ? 'selected' : '' }}>社内</option>
							<option value="2" {{ (int)old('val105', @$mstOrg['val105']) === 2 ? 'selected' : '' }}>社外</option>
						</select>
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						@include('layouts/error/item', ['name' => 'val105'])
						</span>
					</td>
					@if ($target === 'create' || $target === 'edit')
					<td></td>
					<td></td>
					@endif
					<td class="align-middle">部内外 *：</td>
					<td>
						<select name="val106" {{ $target === "show" ? 'disabled="disabled"' : '' }}>
							<option value="1" {{ (int)old('val106', @$mstOrg['val106']) === 1 ? 'selected' : '' }}>部内</option>
							<option value="2" {{ (int)old('val106', @$mstOrg['val106']) === 2 ? 'selected' : '' }}>部外</option>
						</select>
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						@include('layouts/error/item', ['name' => 'val106'])
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">請負会社：</td>
					<td>
						<select name="tempVal107" {{ $target === "show" ? 'disabled="disabled"' : '' }}>
							
						</select>
						<input type="hidden" id="" name="val107" value="{{ old('val107', @$mstOrg['val107']) }}" />
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						@include('layouts/error/item', ['name' => 'val107'])
						</span>
					</td>
					@if ($target === 'create' || $target === 'edit')
					<td></td>
					<td></td>
					@endif
					<td class="align-middle">外注班タイプ：</td>
					<td>
						<select name="val108" {{ $target === "show" ? 'disabled="disabled"' : '' }}>
							<option value="0" {{ (int)old('val108', @$mstOrg['val108']) === 0 ? 'selected' : '' }}>なし</option>
							<option value="1" {{ (int)old('val108', @$mstOrg['val108']) === 1 ? 'selected' : '' }}>貸付</option>
							<option value="2" {{ (int)old('val108', @$mstOrg['val108']) === 2 ? 'selected' : '' }}>一括</option>
							<option value="3" {{ (int)old('val108', @$mstOrg['val108']) === 3 ? 'selected' : '' }}>○加</option>
						</select>
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						@include('layouts/error/item', ['name' => 'val108'])
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">仕入先コード：</td>
					<td>
						<input type="text" name="val109" value="{{ old('val109', @$mstOrg['val109']) }}"
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="20">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						@include('layouts/error/item', ['name' => 'val109'])
						</span>
					</td>
					@if ($target === 'create' || $target === 'edit')
					<td></td>
					<td></td>
					@endif
					<td class="align-middle">フォルダフラグ：</td>
					<td>
						<input type="checkbox" class="input-checkbox" checkbox="val110"
						{{ (int)old('val110', @$mstOrg['val110']) === 1 ? 'checked' : '' }}
						{{ $target === "show" || $target === "edit" ? 'disabled="disabled"' : '' }}> フォルダ
						<input type="hidden" name="val110" value="{{ old('val110', @$mstOrg['val110']) ? 1 : 0 }}">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						@include('layouts/error/item', ['name' => 'val110'])
						</span>
					</td>
				</tr>
				<tr>
					<td class="align-middle">表示順：</td>
					<td>
						<input type="text" class="text-right" name="val111" value="{{ old('val111', @$mstOrg['val111']) }}"
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						@include('layouts/error/item', ['name' => 'val111'])
						</span>
					</td>
					@if ($target === 'create' || $target === 'edit')
					<td></td>
					<td></td>
					@endif
					@if($target === 'show' || $target === 'edit')
					<td class="align-middle">適用期間：</td>
					<td>
						<input class="selectdate" type="text" name="tempVal112" value="{{ old('val112', @$mstOrg['val112']) }}"
						{{ $target === "show" || $target === 'edit' ? 'disabled="disabled"' : '' }} autocomplete="off" style="float:left;width:78px;">
						<span style="display: inline-block ;width:14px; float:left; margin: 4px;">～</span>
						<input class="selectdate" type="text" name="tempVal113" value="{{ old('val113', @$mstOrg['val113']) }}"
						{{ $target === "show" || $target === 'edit' ? 'disabled="disabled"' : '' }} autocomplete="off" style="float:left;width:78px;">
					</td>
					@endif
				</tr>
				</tbody>
			</table>

		</form>

		<div class="row">
			@if($target === 'create' || $target === 'edit')
			<div class="col-xs-1 p-1">
				<button type="button" id="save" class="{{ config('system_const.btn_color_save') }}"><i class="{{ config('system_const.btn_img_save') }}"></i>{{ config('system_const.btn_char_save') }}</button>
			</div>
			@endif
			<div class="col-xs-1 p-1">
				<button type="button" id="cancel" class="{{ config('system_const.btn_color_cancel') }}"><i class="{{ config('system_const.btn_img_cancel') }}"></i>{{ config('system_const.btn_char_cancel') }}</button>
			</div>
		</div>

	</div>
</div>
