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
	})
	
</script>

<div class="row ml-4">
	<div class="col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　棟マスタ@if($target === 'show')参照@elseif($target === 'create')登録@elseif($target === 'edit')更新@endif
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
			@if($target == 'edit')
			<input type="hidden" name="val10" value="{{ old('val10', @$floorData['Updated_at']) }}">
			@endif
			<input type="hidden" name="method" value="{{ $target }}">

			<table class="table table-borderless">
				<tbody>
				<tr>
					<td class="align-middle">コード *：</td>
					<td>
						<input type="text" name="val1" value="{{ old('val1', @$floorData['Code']) }}" 
						{{  $target === "show" || $target === "edit" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="10">
						@if($target === "edit")
						<input type="hidden" name="val1" value="{{ old('val1', @$floorData['Code'])}}">
						@endif
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val1'])
						</span>
					</td>
					<td class="align-middle">名称：</td>
					<td>
						<input type="text" name="val2" value="{{ old('val2', @$floorData['Name']) }}" 
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="50">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val2'])
						</span>
					</td>
				</tr>
				<tr>
					<td class="align-middle">略称：</td>
					<td>
						<input type="text" name="val3" value="{{ old('val3', @$floorData['Nick']) }}" 
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="50">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val3'])
						</span>
					</td>
					<td class="align-middle">略称1：</td>
					<td>
						<input type="text" name="val4" value="{{ old('val4', @$floorData['Nick1']) }}" 
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val4'])
						</span>
					</td>
				</tr>
				<tr>
					<td class="align-middle">物量消化能力 *：</td>
					<td>
						<input type="text" name="val5" value="{{ ($target === "create" && empty(old('val5', @$floorData['BD_P_D']))) ?
						0 : old('val5', @$floorData['BD_P_D']) }}" 
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="11">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val5'])
						</span>
					</td>
					<td class="align-middle">保有HA *：</td>
					<td>
						<input type="text" name="val6" value="{{($target === "create" && empty(old('val6', @$floorData['HA_P_D']))) ?
						0 : old('val6', @$floorData['HA_P_D']) }}" 
						{{ $target == "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="11">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val6'])
						</span>
					</td>
				</tr>
				<tr>
					<td class="align-middle">課係コード: </td>
					<td>
						<input type="text" name="val7" value="{{ old('val7', @$floorData['OwnerGroup']) }}" 
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val7'])
						</span>
					</td>
					<td class="align-middle">表示順 *：</td>
					<td>
						<input type="text" name="val8" value="{{ ($target === "create" && empty(old('val8', @$floorData['SortNo']))) ? 0 : old('val8', @$floorData['SortNo']) }}" 
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="11">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val8'])
						</span>
					</td>
				</tr>
				<tr>
					<td class="align-middle">有効：</td>
					<td>
						<input type="checkbox" class="input-checkbox" checkbox="val9" 
						{{ ($target === "create" && empty(old('val9', @$floorData['ViewFlag']))) 
						|| (int)old('val9', @$floorData['ViewFlag']) === 1 ? 'checked' : '' }} {{ $target === "show" ? 'disabled="disabled"' : '' }}> 有効
						<input type="hidden" name="val9" value="{{ ($target === "create" && empty(old('val9', @$floorData['ViewFlag']))) 
						|| old('val9', @$floorData['ViewFlag']) ? 1 : 0 }}">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val9'])
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