<script>
$(function(){
	$('[data-toggle="tooltip"]').tooltip();

	$('#save').on('click', function(){
		if( $('input[name=method]').val() == 'create'){
			var existsEqual = false;
			var checkInsertJson = null;

			checkInsertJson = {
				"cmn1": '{{ valueUrlEncode($menuInfo->KindID) }}',
				"cmn2": '{{ valueUrlEncode($menuInfo->MenuID) }}',
				"val401": $('input[name=val401]').val(),
			};
			$.ajaxSetup({
				headers: {'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')}
			});
			$.ajax({
				type:'POST',
				url:'{{ url("/") }}/mst/member/checkinsert',
				dataType:'json',
				contentType: "application/json",
				data:JSON.stringify(checkInsertJson),
				beforeSend : function(){
				$('#indicator').trigger('click');
				},
			}).done(function (response) {
				indicatorHide();
				var message = response.message;
				if(response.status == '{{ config("system_const.json_status_ng") }}'){
					existsEqual = true;
				}

				if(message != null){
					window.alert(message);
					return window.location.href = '{{ url("/") }}/index';
				}
				var msg = null;
				if(existsEqual == true){
					msg = '{{ config("message.msg_member_if_001") }}'
				}

				if(msg != null){
					if (!window.confirm(msg)){
						return;
					}
				}

				if($('select[name=tempVal303]').is(':disabled')) {
					$('input[name=val303]').val(0);
				} else {
					var tempValue = $('select[name=tempVal303]').val();
					$('input[name=val303]').val(tempValue);
				}

				if($('select[name=tempVal307]').is(':disabled')) {
					$('input[name=val307]').val(0);
				} else {
					var tempValue = $('select[name=tempVal307]').val();
					$('input[name=val307]').val(tempValue);
				}

				var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/insert';
				$('#mainform').attr('action', url);
				$('#indicator').trigger('click');
				$('#mainform').submit();

			}).fail(function(xhr, status, error) {
				indicatorHide();
				window.location.href = '{{ url("/") }}/errors/500';
			});
		}

		if( $('input[name=method]').val() == 'edit' ) {
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/update';
			$('#mainform').attr('action', url);
			$('#indicator').trigger('click');
			$('#mainform').submit();
		}
		
	});

	$('#cancel').on('click', function(){
		$('#indicator').trigger('click');
		var val4 = encodeURIComponent('{{ $request->val4 }}');
		var val5 = encodeURIComponent('{{ $request->val5 }}');

		if($('input[name=method]').val() == 'create') {
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/index';
		}
		if($('input[name=method]').val() == 'edit' || $('input[name=method]').val() == 'show') {
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/search';
		}
		url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
		if($('input[name=method]').val() == 'edit' || $('input[name=method]').val() == 'show') {
			url += '&pageunit={{ $request->pageunit }}';
			url += '&page={{ $request->page }}';
			url += '&sort={{ $request->sort }}';
			url += '&direction={{ $request->direction }}';
			url +=  '&val1={{ $request->val1 }}&val2={{ $request->val2 }}&val3={{ $request->val3 }}';
			url +=  '&val4={{ $request->val4 }}&val5={{ $request->val5 }}';
			url +=  '&val101={{ $request->val101 }}';
		}

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
		var val304 = $('#select_grp_id').val();
		$('#val304').val(val304);
		var orgName = $('#select_grp_name').val();
		$('[name=parent_org_name]').val(orgName);
		$('#org_select_dialog').modal('hide');
	});

	$('.clearorg').on('click', function(){
		$('[name=parent_org_name]').val("{{ config('system_const.org_null_name') }}");
		$('input[name="val304"]').val("{{ valueUrlEncode(0) }}");
	});
	
	$('.selectdate').datepicker();

	var arrKanren = fncJsonParse('{{ json_encode($arrKanren) }}');
	var arrVal307 = [
		{ id: 0, name: "" },
		{ id: 1, name: "貸付" },
		{ id: 2, name: "一括" },
		{ id: 3, name: "県外工" },
	];
	
	$('select[name=val302]').on('change', function () {
		$('select[name=tempVal303]').empty();
		$('select[name=tempVal307]').empty();
		if(this.value == 2){
			if( $('input[name=method]').val() != 'show') {
				$('select[name="tempVal303"]').removeAttr('disabled', 'disabled');
				$('select[name="tempVal307"]').removeAttr('disabled', 'disabled');
			}
			bindingDataSelectBox('select[name=tempVal303]', arrKanren, $('input[name=val303]').val());
			bindingDataSelectBox('select[name=tempVal307]', arrVal307, $('input[name=val307]').val());

		} else {
			$('select[name=tempVal303]').append('<option value="0" selected="selected"></option>');
			$('select[name="tempVal303"]').attr('disabled', 'disabled');

			$('select[name=tempVal307]').append('<option value="0" selected="selected"></option>');
			$('select[name="tempVal307"]').attr('disabled', 'disabled');
		}
	}).trigger('change');

	function bindingDataSelectBox(selectbox, data, selectedValue){
		data.forEach(element => {
			var selectValue = (element.id == selectedValue) ? 'selected' : '';
			$(selectbox).append('<option value="'+ element.id +'" '+ selectValue +'>'+  convertHTML(element.name) +'</option>');
		});
	}

})

</script>

<div class="row ml-4">
	<div class="col-xs-12">

		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　人員マスタ@if($target === 'show')参照@elseif($target === 'create')登録@elseif($target === 'edit')更新@endif
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
		
		<form action="{{ url('/') }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/historysave" method="POST" id="mainform">
			@csrf
			<input type="hidden" name="method" value="{{ $target }}">
			<input type="hidden" id="kindid" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}">
			<input type="hidden" id="menuid" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}">
			@if($target === 'edit')
				<input type="hidden" id="pageunit" name="pageunit" value="{{ $request->pageunit }}">
				<input type="hidden" id="page" name="page" value="{{ $request->page }}">
				<input type="hidden" id="sort" name="sort" value="{{ $request->sort }}">
				<input type="hidden" id="direction" name="direction" value="{{ $request->direction }}">
				<input type="hidden" id="val1" name="val1" value="{{ $request->val1 }}">
				<input type="hidden" id="val2" name="val2" value="{{ $request->val2 }}">
				<input type="hidden" id="val3" name="val3" value="{{ $request->val3 }}">
				<input type="hidden" id="val4" name="val4" value="{{ $request->val4 }}">
				<input type="hidden" id="val5" name="val5" value="{{ $request->val5 }}">
				<input type="hidden" id="val101" name="val101" value="{{ $request->val101 }}">
				<input type="hidden" id="val102" name="val102" value="{{ $request->val102 }}">
			@endif
			@if($target === 'create')
				<input type="hidden" id="val304" name="val304" value="{{ old('val304', valueUrlEncode(@$memHist['val304'])) }}">
				<input type="hidden" id="select_grp_id" name="select_grp_id" value="{{ old('val304', valueUrlEncode(@$memHist['val304'])) }}">
				<input type="hidden" id="select_grp_name" name="select_grp_name" value="{{ old('select_grp_name', $grpName) }}">
			@endif

			<table class="table table-borderless">
				<tbody>
				<tr>
					<td class="align-middle">名前 *：</td>
					<td>
						<input type="text" name="val401" value="{{ old('val401', @$mstMember ['val401']) }}"
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="50">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						@include('layouts/error/item', ['name' => 'val401'])
						</span>
					</td>
					<td class="align-middle">フリガナ *：</td>
					<td>
						<input type="text" name="val402" value="{{ old('val402', @$mstMember ['val402']) }}"
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="50">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						@include('layouts/error/item', ['name' => 'val402'])
						</span>
					</td>
				</tr>
				<tr>
					<td class="align-middle">略称 *：</td>
					<td>
						<input type="text" name="val403" value="{{ old('val403', @$mstMember ['val403']) }}"
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="50">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						@include('layouts/error/item', ['name' => 'val403'])
						</span>
					</td>
					<td class="align-middle">定年退職日：</td>
					<td>
						<input id="" type="text" maxlength="10" name="val404" size="14" value="{{ old('val404', @$mstMember ['val404']) }}" 
						{{ $target === "show" ? 'disabled="disabled"' : '' }} class="selectdate" autocomplete="off">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						@include('layouts/error/item', ['name' => 'val404'])
						</span>
					</td>
				</tr>
			@if($target === 'create')
				<tr>
					<td class="align-middle">社員番号：</td>
					<td>
					<input type="text" name="val301" value="{{ old('val301', @$memHist['val301']) }}" class="text-right" 
					{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						@include('layouts/error/item', ['name' => 'val301'])
						</span>
					</td>
					<td class="align-middle">社内外フラグ *：</td>
					<td>
						<select name="val302" {{ $target === "show" ? 'disabled="disabled"' : '' }}>
							<option value="1" {{ (int)old('val302',@$memHist['val302']) === 1 ? 'selected' : '' }}>社内</option>
							<option value="2" {{ (int)old('val302',@$memHist['val302']) === 2 ? 'selected' : '' }}>社外</option>
						</select>
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						@include('layouts/error/item', ['name' => 'val302'])
						</span>
					</td>
				</tr>
				
				<tr>
					<td class="align-middle">会社名：</td>
					<td>
						<select name="tempVal303" {{ $target === "show" ? 'disabled="disabled"' : '' }}></select>
						<input type="hidden" id="" name="val303" value="{{ old('val303', @$memHist['val303']) }}" />
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						@include('layouts/error/item', ['name' => 'val303'])
						</span>
					</td>
					<td class="align-middle">所属班：</td>
					<td> 
						<input type="text" {{ $target === "show" ? 'disabled="disabled"' : '' }} name="parent_org_name" 
						value="{{ old('parent_org_name', $grpName) }}" readonly="" tabindex="-1" data-toggle="modal" data-target="#org_select_dialog">
						<input type="hidden" name="parent_org_name" value="{{ old('parent_org_name', $grpName) }}">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						@include('layouts/error/item', ['name' => 'val304'])
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
					@include('mst/org/select', ['mstOrgCommon' => $mstOrgCommon, 'activeOrgID' => $memHist['val304'], 'folderOnly' => false])
					@endif
				</tr>

				<tr>
					<td class="align-middle">開始日 *：</td>
					<td>
						<input id="" type="text" maxlength="10" name="val305" size="14" value="{{ old('val305', @$memHist['val305']) }}" 
						{{ $target === "show" ? 'disabled="disabled"' : '' }} class="selectdate" autocomplete="off">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						@include('layouts/error/item', ['name' => 'val305'])
						</span>
					</td>
					<td class="align-middle">終了日：</td>
					<td>
						<input id="" type="text" maxlength="10" name="val306" size="14" value="{{ old('val306', @$memHist['val306']) }}" 
						{{ $target === "show" ? 'disabled="disabled"' : '' }} class="selectdate" autocomplete="off">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						@include('layouts/error/item', ['name' => 'val306'])
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">職種 *：</td>
					<td>
						<select name="val308" {{ $target === "show" ? 'disabled="disabled"' : '' }}>
							<option value=""></option>
							@foreach ($mstSyokusyu as $item)
								<option value="{{ $item->Code }}" {{ trim(old('val308', @$memHist['val308'])) == trim($item->Code) ? 'selected' : '' }}>
									{{ $item->Name }}
								</option>
							@endforeach
						</select>
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						@include('layouts/error/item', ['name' => 'val308'])
						</span>
					</td>
					<td class="align-middle">外注班タイプ：</td>
					<td>
						<select name="tempVal307" {{ $target === "show" ? 'disabled="disabled"' : '' }}>
							
						</select>
						<input type="hidden" id="" name="val307" value="{{ old('val307', @$memHist['val307']) }}" />
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						@include('layouts/error/item', ['name' => 'val307'])
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">プロパー *：</td>
					<td>
						<select name="val309" {{ $target === "show" ? 'disabled="disabled"' : '' }}>
							<option value="0" {{ trim(old('val309', @$memHist['val309'])) === '0' ? 'selected' : '' }}>その他</option>
							<option value="1" {{ ($target === "create" || trim(old('val309', @$memHist['val309']))) === '1' ? 'selected' : '' }}>直雇</option>
							<option value="2" {{ trim(old('val309', @$memHist['val309'])) === '2' ? 'selected' : '' }}>外国人研修生</option>
							<option value="3" {{ trim(old('val309', @$memHist['val309'])) === '3' ? 'selected' : '' }}>請負</option>
							<option value="4" {{ trim(old('val309', @$memHist['val309'])) === '4' ? 'selected' : '' }}>派遣</option>
							<option value="5" {{ trim(old('val309', @$memHist['val309'])) === '5' ? 'selected' : '' }}>再雇用</option>
							<option value="6" {{ trim(old('val309', @$memHist['val309'])) === '6' ? 'selected' : '' }}>パート</option>
							<option value="7" {{ trim(old('val309', @$memHist['val309'])) === '7' ? 'selected' : '' }}>加工外注</option>
						</select>
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						@include('layouts/error/item', ['name' => 'val309'])
						</span>
					</td>
				</tr>
			@endif
				</tbody>
			</table>

		</form>

		<div class="row">
			@if($target === 'create' || $target === 'edit')
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
