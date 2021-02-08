<script>
$(function(){
	$('[data-toggle="tooltip"]').tooltip();
	$('#save').on('click', function(){
		$('#indicator').trigger('click');
		$('#mainform').submit();
	});

	$('#cancel').on('click', function(){
		$('#indicator').trigger('click');
		var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/';
		url += 'index?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
		url += '&page={{ $request->page }}';
		url += '&pageunit={{ $request->pageunit }}';
		url += '&sort={{ $request->sort }}';
		url += '&direction={{ $request->direction }}';
		window.location.href = url;
	});

	$('.selectdate').datepicker();
	$('.input-checkbox').click(function(){
		if($(this).prop('checked')){
			$('[name="'+$(this).attr('checkbox')+'"]').val(1);
		}else{
			$('[name="'+$(this).attr('checkbox')+'"]').val(0);
		}
	});
});

</script>

<div class="row ml-4">
	<div class="col-xs-12">

		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　オーダマスタ@if($target === 'show')参照@elseif($target === 'create')登録@elseif($target === 'edit')更新@endif
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
			<input type="hidden" id="kindid" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}">
			<input type="hidden" id="menuid" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}">
			<input type="hidden" name="page" value="{{ $request->page }}">
			<input type="hidden" name="pageunit" value="{{ $request->pageunit }}">
			<input type="hidden" name="sort" value="{{ $request->sort }}">
			<input type="hidden" name="direction" value="{{ $request->direction }}">
			@if($target === 'edit')
			<input type="hidden" name="val26" value="{{ old('val26', @$orderData['Updated_at']) }}">
			@endif
			<input type="hidden" name="method" value="{{ $target }}">

			<table class="table table-borderless">
				<tbody>
				<tr>
					<td class="align-middle">オーダ *：</td>
					<td>
						<input type="text" name="val1" value="{{ old('val1', @$orderData['OrderNo']) }}"
						{{ $target === "show" || $target === "edit" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="10">
						@if($target === "edit")
						<input type="hidden" name="val1" value="{{ old('val1', @$orderData['OrderNo'])}}">
						@endif
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val1'])
						</span>
					</td>
					<td class="align-middle">建造区分：</td>
					<td>
						<input type="text" name="val2" value="{{ old('val2', @$orderData['BLDDIST']) }}"
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="3">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val2'])
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">船級：</td>
					<td>
						<input type="text" name="val3" value="{{ old('val3', @$orderData['CLASS']) }}"
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="2">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val3'])
						</span>
					</td>
					<td class="align-middle">船種：</td>
					<td>
						<input type="text" name="val4" value="{{ old('val4', @$orderData['TYPE']) }}"
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="8">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val4'])
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">船型：</td>
					<td>
						<input type="text" name="val5" value="{{ old('val5', @$orderData['STYLE']) }}"
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="8">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val5'])
						</span>
					</td>
					<td class="align-middle">略称：</td>
					<td>
						<input type="text" name="val6" value="{{ old('val6', @$orderData['NAME']) }}"
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="8">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val6'])
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">Topマーキン：</td>
					<td>
						<input type="text" name="val7" value="{{ old('val7', @$orderData['TP_Date']) }}" class="selectdate"
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="10">
						@include('layouts/error/item', ['name' => 'val7'])
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val7'])
						</span>
					</td>
					<td class="align-middle">小組開始日：</td>
					<td>
						<input type="text" name="val8" value="{{ old('val8', @$orderData['KG_Date']) }}" class="selectdate"
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val8'])
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">大組開始日：</td>
					<td>
						<input type="text" name="val9" value="{{ old('val9', @$orderData['OG_Date']) }}" class="selectdate"
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val9'])
						</span>
					</td>
					<td class="align-middle">総組開始日：</td>
					<td>
						<input type="text" name="val10" value="{{ old('val10', @$orderData['SG_Date']) }}" class="selectdate"
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val10'])
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">搭載開始日：</td>
					<td>
						<input type="text" name="val11" value="{{ old('val11', @$orderData['LD_Date']) }}" class="selectdate"
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val11'])
						</span>
					</td>
					<td class="align-middle">船着工：</td>
					<td>
						<input type="text" name="val12" value="{{ old('val12', @$orderData['S_SDate']) }}" class="selectdate"
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val12'])
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">PE開始：</td>
					<td>
						<input type="text" name="val13" value="{{ old('val13', @$orderData['PE_SDate']) }}" class="selectdate"
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val13'])
						</span>
					</td>
					<td class="align-middle">シフト日：</td>
					<td>
						<input type="text" name="val14" value="{{ old('val14', @$orderData['ST_Date']) }}" class="selectdate"
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val14'])
						</span>
					</td>
				</tr>
				<tr>
					<td class="align-middle">進水：</td>
					<td>
						<input type="text" name="val15" value="{{ old('val15', @$orderData['L_Date']) }}" class="selectdate"
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val15'])
						</span>
					</td>
					<td class="align-middle">出渠日：</td>
					<td>
						<input type="text" name="val16" value="{{ old('val16', @$orderData['O_Date']) }}" class="selectdate"
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val16'])
						</span>
					</td>
				</tr>
				<tr>
					<td class="align-middle">離岸日：</td>
					<td>
						<input type="text" name="val17" value="{{ old('val17', @$orderData['PI_Date']) }}" class="selectdate"
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val17'])
						</span>
					</td>
					<td class="align-middle">引渡：</td>
					<td>
						<input type="text" name="val18" value="{{ old('val18', @$orderData['D_Date']) }}" class="selectdate"
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val18'])
						</span>
					</td>
				</tr>
				<tr>
					<td class="align-middle">作業長支援に取り込み可能：</td>
					<td>
						<input type="checkbox" class="input-checkbox" checkbox="val19"
						{{ (int)old('val19', @$orderData['Sgts_Flag']) === 1 ? 'checked' : '' }}
						{{ $target === "show" ? 'disabled="disabled"' : '' }}> 取込可
						<input type="hidden" name="val19" value="{{ old('val19', @$orderData['Sgts_Flag']) ? 1 : 0 }}">
						@include('layouts/error/item', ['name' => 'val19'])
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val19'])
						</span>
					</td>
					<td class="align-middle">ダミーオーダフラグ：</td>
					<td>
						<input type="checkbox" class="input-checkbox" checkbox="val20"
						{{ (int)old('val20', @$orderData['Is_Dummy']) === 1 ? 'checked' : '' }}
						{{ $target === "show" ? 'disabled="disabled"' : '' }}> ダミーオーダ
						<input type="hidden" name="val20" value="{{ old('val20', @$orderData['Is_Dummy']) ? 1 : 0 }}">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val20'])
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">艦艇フラグ：</td>
					<td>
						<select name="val21" {{ $target === "show" ? 'disabled="disabled"' : '' }}>
							<option value="0" {{ (int)old('val21', @$orderData['Is_Kantei']) === 0 ? 'selected' : '' }}>艦艇ではない</option>
							<option value="1" {{ ($target === "create" && empty(old('val21', @$orderData['Is_Kantei']))) 
							|| (int)old('val21', @$orderData['Is_Kantei']) === 1 ? 'selected' : '' }}>艦艇</option>
						</select>
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val21'])
						</span>
					</td>
					<td class="align-middle">非表示フラグ：</td>
					<td>
						<input type="checkbox" class="input-checkbox" checkbox="val22"
						{{ (int)old('val22', @$orderData['DispFlag']) === 1 ? 'checked' : '' }}
						{{ $target === "show" ? 'disabled="disabled"' : '' }}> 非表示
						<input type="hidden" name="val22" value="{{ old('val22', @$orderData['DispFlag']) ? 1 : 0 }}">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val22'])
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">WBSコード：</td>
					<td>
						<input type="text" name="val23" value="{{ old('val23', @$orderData['WBSCode']) }}"
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="3">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val23'])
						</span>
					</td>
					<td class="align-middle">前船オーダ：</td>
					<td>
						<input type="text" name="val24" value="{{ old('val24', @$orderData['PreOrderNo']) }}"
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val24'])
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">備考：</td>
					<td>
						<input type="text" name="val25" value="{{ old('val25', @$orderData['Note']) }}"
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="20">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val25'])
						</span>
					</td>
				</tr>
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
