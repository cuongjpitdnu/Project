<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();

		$('#save').on('click', function() {
			$('#indicator').trigger('click');
			$('#mainform').submit();
		});

		$('#cancel').on('click', function() {
			$('#indicator').trigger('click');
			let url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/';
			url += 'pmanage?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&bpage={{ $request->bpage }}';
			url += '&bsort={{ $request->bsort }}';
			url += '&bdirection={{ $request->bdirection }}';
			url += '&page={{ $request->page }}';
			url += '&sort={{ $request->sort }}';
			url += '&direction={{ $request->direction }}';
			url += '&val1={{ $request->val1 }}';
			url += '&val2={{ $request->val2 }}';
			url += '&val101={{ $request->val101 }}';
			url += '&val102={{ $request->val102 }}';
			url += '&val103={{ $request->val103 }}';
			url += '&val104={{ $request->val104 }}';
			url += '&val201={{ $request->val201 }}';
			window.location.href = url;
		});
	});
</script>

<div class="row ml-2 mr-2">
	<div class="col-md-12 col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　項目定義@if($target === 'show')(詳細・参照)@elseif($target === 'edit')(詳細・更新)@endif
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

		<form action="{{ url('/') }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/psave" method="POST" id="mainform">
			@csrf
			<input type="hidden" id="kindid" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}" />
			<input type="hidden" id="menuid" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}" />
			<input type="hidden" name="val1" value="{{ $request->val1 }}" />
			<input type="hidden" name="val2" value="{{ $request->val2 }}" />
			<input type="hidden" name="page" value="{{ $request->page }}" />
			<input type="hidden" name="sort" value="{{ $request->sort }}" />
			<input type="hidden" name="direction" value="{{ $request->direction }}" />
			<input type="hidden" name="val101" value="{{ $request->val101 }}" />
			<input type="hidden" name="val102" value="{{ $request->val102 }}" />
			<input type="hidden" name="val103" value="{{ $request->val103 }}" />
			<input type="hidden" name="val104" value="{{ $request->val104 }}" />
			<input type="hidden" name="val201" value="{{ $request->val201 }}" />
			<input type="hidden" name="val202" value="{{ $request->val202 }}" />

			<div class="row ml-1 mt-3">
				<div class="col-xs-12">
					<table class="table table-borderless">
						<tbody>
							<tr>
								<td class="align-middle">工程 *：</td>
								<td>
									<select name="val301" id=""
									{{ $target === "show" ? 'disabled="disabled"' : '' }}>
										@foreach($dataSelect['val301'] as $data)
											<option value="{{ $data->Code }}"
												{{ old('val301', @$itemData['Kotei']) === $data->Code ? 'selected' : '' }}>{{ $data->NameShow }}</option>
										@endforeach
									</select>
								</td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val301'])
									</span>
								</td>
								<td class="align-middle">工程組区 *：</td>
								<td>
									<select name="val302" id=""
									{{ $target === "show" ? 'disabled="disabled"' : '' }}>
										<option value=""></option>
										@foreach($dataSelect['val302'] as $key => $data)
											<option value="{{ valueUrlEncode($key) }}"
											{{ old('val302', @$itemData['KKumiku']) === valueUrlEncode($key) ? 'selected' : '' }}>{{ $data }}</option>
										@endforeach
									</select>
								</td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val302'])
									</span>
								</td>
							</tr>
							<tr>
								<td class="align-middle">施工棟：</td>
								<td>
									<select name="val303" id=""
									{{ $target === "show" ? 'disabled="disabled"' : '' }}>
										<option value=""></option>
										@foreach($dataSelect['val303'] as $data)
											<option value="{{ $data->Code }}"
												{{ old('val303', @$itemData['Floor']) === $data->Code ? 'selected' : '' }}>{{ $data->NameShow }}</option>
										@endforeach
									</select>
								</td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val303'])
									</span>
								</td>
								<td class="align-middle">物量コード：</td>
								<td>
									<select name="val304" id=""
									{{ $target === "show" ? 'disabled="disabled"' : '' }}>
										<option value=""></option>
										@foreach($dataSelect['val304'] as $data)
											<option value="{{ $data->Code }}"
												{{ old('val304', @$itemData['BD_Code']) === $data->Code ? 'selected' : '' }}>{{ $data->NameShow }}</option>
										@endforeach
									</select>
								</td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val304'])
									</span>
								</td>
							</tr>
							<tr>
								<td class="align-middle">物量：</td>
								<td><input type="text" name="val305" id="" maxlength="9" autocomplete="off"
									value="{{ old('val305', @$itemData['BData']) }}"
									{{ $target === "show" ? 'disabled="disabled"' : '' }}></td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val305'])
									</span>
								</td>
								<td class="align-middle">HC：</td>
								<td><input type="text" name="val306" id="" maxlength="9" autocomplete="off"
									value="{{ old('val306', @$itemData['HC']) }}"
									{{ $target === "show" ? 'disabled="disabled"' : '' }}></td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val306'])
									</span>
								</td>
							</tr>
							<tr>
								<td class="align-middle">工期：</td>
								<td><input type="text" name="val307" id="" maxlength="10" autocomplete="off"
									value="{{ old('val307', @$itemData['Days']) }}"
									{{ $target === "show" ? 'disabled="disabled"' : '' }}></td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val307'])
									</span>
								</td>
								<td class="align-middle">次工程：</td>
								<td>
									<select name="val308" id=""
									{{ $target === "show" ? 'disabled="disabled"' : '' }}>
										<option value=""></option>
										@foreach($dataSelect['val308'] as $data)
											<option value="{{ $data->Code }}"
												{{ old('val308', @$itemData['N_Kotei']) === $data->Code ? 'selected' : '' }}>{{ $data->NameShow }}</option>
										@endforeach
									</select>
								</td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val308'])
									</span>
								</td>
							</tr>
							<tr>
								<td class="align-middle">次工程組区：</td>
								<td>
									<select name="val309" id=""
									{{ $target === "show" ? 'disabled="disabled"' : '' }}>
										<option value=""></option>
										@foreach($dataSelect['val309'] as $data)
											<option value="{{ $data->KKumiku }}"
												{{ old('val309', @$itemData['N_KKumiku']) === $data->KKumiku ? 'selected' : '' }}>{{ $data->KKumikuShow }}</option>
										@endforeach
									</select>
								</td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val309'])
									</span>
								</td>
								<td class="align-middle">定盤：</td>
								<td><input type="text" name="val310" id="" maxlength="4" autocomplete="off"
									value="{{ old('val310', @$itemData['Jyoban']) }}"
									{{ $target === "show" ? 'disabled="disabled"' : '' }}></td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val310'])
									</span>
								</td>
							</tr>
						</tbody>
					</table>
				</div>
			</div>
		</form>

		<div class="row">
			@if($target === 'edit')
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