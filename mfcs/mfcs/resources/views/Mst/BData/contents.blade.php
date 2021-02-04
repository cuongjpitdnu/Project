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
				■　物量マスタ@if($target === 'show')参照@elseif($target === 'create')登録@elseif($target === 'edit')更新@endif
			</div>
		</div>
		@if($errors->count() === 0)
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
			<input type="hidden" name="val5" value="{{ old('val5', @$bData['Updated_at']) }}">
			@endif
			<input type="hidden" name="method" value="{{ $target }}">

			<table class="table table-borderless">
				<tbody>
				<tr>
					<td class="align-middle">コード *：</td>
					<td>
						<input type="text" name="val1" value="{{ old('val1', @$bData['Code']) }}" 
						{{ $target === "show" || $target === "edit" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="5">
						@if($target === "edit")
						<input type="hidden" name="val1" value="{{ old('val1', @$bData['Code'])}}">
						@endif
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val1']) 
						</span>
					</td>
				</tr>
				<tr>
					<td class="align-middle">名称：</td>
					<td>
						<input type="text" name="val2" value="{{ old('val2', @$bData['Name']) }}" 
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
						<input type="text" name="val3" value="{{ old('val3', @$bData['Nick']) }}" 
						{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="50">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val3']) 
						</span>
					</td>
				</tr>
				<tr>
					<td class="align-middle">有効：</td>
					<td>
						<input type="checkbox" class="input-checkbox" checkbox="val4" 
						{{ ($target === "create" && empty(old('val4', @$bData['ViewFlag']))) 
						|| (int)old('val4', @$bData['ViewFlag']) === 1 ? 'checked' : '' }} {{ $target === "show" ? 'disabled="disabled"' : '' }}> 有効
						<input type="hidden" name="val4" value="{{ ($target === "create" && empty(old('val4', @$bData['ViewFlag']))) 
						|| old('val4', @$bData['ViewFlag']) ? 1 : 0 }}">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val4']) 
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