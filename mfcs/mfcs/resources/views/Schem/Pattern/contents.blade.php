<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();
		$('#save').on('click', function() {
			$('#indicator').trigger('click');
			$('input[name=val101]').val($('input[name=val101-view]').val());
			$('input[name=val103]').val($('select[name=val103-view]').val());
			$('input[name=val104]').val(0);
			if(!$('.input-checkbox').is(':checked')) {
				$('input[name=val104]').val(1);
			}
			$('#mainform').submit();
		});

		$('#cancel').on('click', function() {
			$('#indicator').trigger('click');
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/';
			url += 'index?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&page={{ $request->page }}';
			url += '&pageunit={{ $request->pageunit }}';
			url += '&sort={{ $request->sort }}';
			url += '&direction={{ $request->direction }}';
			window.location.href = url;
		});
	});
</script>

<div class="row ml-4">
	<div class="col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　工程パターンマスタ@if($target === 'show')参照@elseif($target === 'create')登録@elseif($target === 'edit')更新@endif
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
			<input type="hidden" name="method" value="{{ $target }}" />
			<input type="hidden" id="kindid" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}" />
			<input type="hidden" id="menuid" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}" />
			<input type="hidden" name="page" value="{{ $request->page }}" />
			<input type="hidden" name="pageunit" value="{{ $request->pageunit }}" />
			<input type="hidden" name="sort" value="{{ $request->sort }}" />
			<input type="hidden" name="direction" value="{{ $request->direction }}" />

			@if($target === 'edit')
			<input type="hidden" name="val105" value="{{ valueUrlEncode(old('val105', @$itemData['val105'])) }}">
			@endif
			<input type="hidden" name="val101" value="" />
			<input type="hidden" name="val103" value="" />

			<table class="table table-borderless">
				<tbody>
					<tr>
						<td class="align-middle">コード *：</td>
						<td>
							<input type="text" name="val101-view" value="{{ old('val101', @$itemData['val101']) }}"
							{{ $target === "show" || $target === "edit" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="2">
						</td>
						<td class="p-0 align-middle">
							<span class="col-xs-1 p-1">
								@include('layouts/error/item', ['name' => 'val101'])
							</span>
						</td>
					</tr>
					<tr>
						<td class="align-middle">名称 *：</td>
						<td>
							<input type="text" name="val102" value="{{ old('val102', @$itemData['val102']) }}"
							{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="20">
						</td>
						<td class="p-0 align-middle">
							<span class="col-xs-1 p-1">
								@include('layouts/error/item', ['name' => 'val102'])
							</span>
						</td>
					</tr>
					<tr>
						<td class="align-middle">中日程区分 *：</td>
						<td>
							<select name="val103-view" {{ in_array($target, ["show", "edit"]) ? 'disabled="disabled"' : '' }}>
								<option value="" hidden></option>
								<option value="{{ valueUrlEncode(config('system_const.c_kind_chijyo')) }}"
									{{ !is_null(old('val103', @$itemData['val103'])) &&
										(int)old('val103', @$itemData['val103']) === config('system_const.c_kind_chijyo') ? 'selected' : '' }}>{{
									config('system_const.c_name_chijyo')
								}}</option>
								<option value="{{ valueUrlEncode(config('system_const.c_kind_gaigyo')) }}"
									{{ !is_null(old('val103', @$itemData['val103'])) &&
										(int)old('val103', @$itemData['val103']) === config('system_const.c_kind_gaigyo') ? 'selected' : '' }}>{{
									config('system_const.c_name_gaigyo')
								}}</option>
								<option value="{{ valueUrlEncode(config('system_const.c_kind_giso')) }}"
									{{ !is_null(old('val103', @$itemData['val103'])) &&
										(int)old('val103', @$itemData['val103']) === config('system_const.c_kind_giso') ? 'selected' : '' }}>{{
									config('system_const.c_name_giso')
								}}</option>
							</select>
						</td>
						<td class="p-0 align-middle">
							<span class="col-xs-1 p-1">
								@include('layouts/error/item', ['name' => 'val103'])
							</span>
						</td>
					</tr>
					<tr>
						<td class="align-middle">有効：</td>
						<td>
							<label for="chk">
								<input type="checkbox" class="input-checkbox" checkbox="val104" id="chk"
								{{ $target === "show" ? 'disabled="disabled"' : '' }}
								{{ (int)old('val104', @$itemData['val104']) === 0 ? 'checked' : '' }}
								> 有効
							</label>
							<input type="hidden" name="val104" value="{{ old('val104', @$itemData['val104']) }}">
						</td>
						<td class="p-0 align-middle">
							<span class="col-xs-1 p-1">
								@include('layouts/error/item', ['name' => 'val104'])
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