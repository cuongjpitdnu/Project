<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();
		$('#save').on('click', function() {
			$('#indicator').trigger('click');
			$('#mainform').submit();
		});

		$('#cancel').on('click', function() {
			$('#indicator').trigger('click');
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/';
			url += 'indexdetail?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&page={{ $request->page }}';
			url += '&pageunit={{ $request->pageunit }}';
			url += '&sort={{ $request->sort }}';
			url += '&direction={{ $request->direction }}';
			url += '&val1={{ $request->val1 }}';
			url += '&val3={{ $request->val3 }}';
			window.location.href = url;
		});
	});
</script>

<div class="row ml-4">
	<div class="col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　工程パターンマスタ(詳細)@if($target === 'show')参照@elseif($target === 'create')登録@elseif($target === 'edit')更新@endif
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

		<form action="{{ url('/') }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/savedetail" method="POST" id="mainform">
			@csrf
			<input type="hidden" id="kindid" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}" />
			<input type="hidden" id="menuid" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}" />
			<input type="hidden" name="page" value="{{ $request->page }}" />
			<input type="hidden" name="pageunit" value="{{ $request->pageunit }}" />
			<input type="hidden" name="sort" value="{{ $request->sort }}" />
			<input type="hidden" name="direction" value="{{ $request->direction }}" />
			<input type="hidden" name="val1" value="{{ $request->val1 }}" />
			<input type="hidden" name="val3" value="{{ $request->val3 }}" />
			@if($target === 'edit')
			<input type="hidden" name="val101" value="{{ $request->val101 }}" />
			<input type="hidden" name="val102" value="{{ old('val102', $request->val102) }}" />
			@endif
			<input type="hidden" name="method" value="{{ $target }}" />

			<table class="table table-borderless">
				<tbody>
					<tr>
						<td class="align-middle">組区 *：</td>
						<td>
							<select name="val201" id=""
								{{ $target === "show" ? 'disabled="disabled"' : '' }}>
								<option value=""></option>
								@foreach($dataSelect['val201'] as $key => $data)
									<option value="{{ $key }}"
										{{ old('val201', trim(@$itemData['val201'])) === trim($key) ? 'selected': '' }}>{{ $data }}</option>
								@endforeach
							</select>
						</td>
						<td class="p-0 align-middle">
							<span class="col-xs-1 p-1">
								@include('layouts/error/item', ['name' => 'val201'])
							</span>
						</td>
						<td class="align-middle">工期：</td>
						<td>
							<input type="text" name="val202" value="{{ old('val202', @$itemData['val202']) }}"
							{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="5">
						</td>
						<td class="p-0 align-middle">
							<span class="col-xs-1 p-1">
								@include('layouts/error/item', ['name' => 'val202'])
							</span>
						</td>
					</tr>

					<tr>
						<td class="align-middle">工程 *：</td>
						<td>
							<select name="val203" id=""
								{{ $target === "show" ? 'disabled="disabled"' : '' }}>
								<option value=""></option>
								@foreach($dataSelect['val203'] as $data)
									<option value="{{ trim($data['Code']) }}"
										{{ old('val203', trim(@$itemData['val203'])) === trim($data['Code']) ? 'selected': '' }}>{{ $data['Name'] }}</option>
								@endforeach
							</select>
						</td>
						<td class="p-0 align-middle">
							<span class="col-xs-1 p-1">
								@include('layouts/error/item', ['name' => 'val203'])
							</span>
						</td>
						<td class="align-middle">施工棟：</td>
						<td>
							<select name="val204" id=""
								{{ $target === "show" ? 'disabled="disabled"' : '' }}>
								<option value=""></option>
								@foreach($dataSelect['val204'] as $data)
									<option value="{{ trim($data['Code']) }}"
										{{ old('val204', trim(@$itemData['val204'])) === trim($data['Code']) ? 'selected': '' }}>{{ $data['Name'] }}</option>
								@endforeach
							</select>
						</td>
						<td class="p-0 align-middle">
							<span class="col-xs-1 p-1">
								@include('layouts/error/item', ['name' => 'val204'])
							</span>
						</td>
					</tr>

					<tr>
						<td class="align-middle">管理物量：</td>
						<td>
							<select name="val205" id=""
								{{ $target === "show" ? 'disabled="disabled"' : '' }}>
								<option value=""></option>
								@foreach($dataSelect['val205'] as $data)
									<option value="{{ trim($data['Code']) }}"
										{{ old('val205', trim(@$itemData['val205'])) === trim($data['Code']) ? 'selected': '' }}>{{ $data['Name'] }}</option>
								@endforeach
							</select>
						</td>
						<td class="p-0 align-middle">
							<span class="col-xs-1 p-1">
								@include('layouts/error/item', ['name' => 'val205'])
							</span>
						</td>
						<td class="align-middle">次組区：</td>
						<td>
							<select name="val206" id=""
								{{ $target === "show" ? 'disabled="disabled"' : '' }}>
								<option value=""></option>
								@foreach($dataSelect['val206'] as $data)
									<option value="{{ $data['KKumiku'] }}"
										{{ old('val206', trim(@$itemData['val206'])) === trim($data['KKumiku']) ? 'selected': '' }}>{{ $data['Name'] }}</option>
								@endforeach
							</select>
						</td>
						<td class="p-0 align-middle">
							<span class="col-xs-1 p-1">
								@include('layouts/error/item', ['name' => 'val206'])
							</span>
						</td>
					</tr>

					<tr>
						<td class="align-middle">リンク日数：</td>
						<td>
							<input type="text" name="val207" value="{{ old('val207', @$itemData['val207']) }}"
							{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="5">
						</td>
						<td class="p-0 align-middle">
							<span class="col-xs-1 p-1">
								@include('layouts/error/item', ['name' => 'val207'])
							</span>
						</td>
						<td class="align-middle">次工程：</td>
						<td>
							<select name="val208" id=""
								{{ $target === "show" ? 'disabled="disabled"' : '' }}>
								<option value=""></option>
								@foreach($dataSelect['val208'] as $data)
									<option value="{{ trim($data['Code']) }}"
										{{ old('val208', trim(@$itemData['val208'])) === trim($data['Code']) ? 'selected': '' }}>{{ $data['Name'] }}</option>
								@endforeach
							</select>
						</td>
						<td class="p-0 align-middle">
							<span class="col-xs-1 p-1">
								@include('layouts/error/item', ['name' => 'val208'])
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