<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();

		const dataVal6 = fncJsonParse('{{ json_encode($dataSelect['data_6_all']) }}');
		$('[name=val5]').on('change', function(e) {
			bindingSelect('val6', dataVal6, $(this).val());
		});

		$('#save').on('click', function() {
			$('#indicator').trigger('click');
			$('#mainform').submit();
		});

		$('#cancel').on('click', function() {
			$('#indicator').trigger('click');
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/manage';
			url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&page={{ $request->page }}';
			url += '&pageunit={{ $request->pageunit }}';
			url += '&sort={{ $request->sort }}';
			url += '&direction={{ $request->direction }}';
			url += '&val1={{ $request->val1 }}';
			window.location.href = url;
		});
	});

	function bindingSelect(name_input, data, ckind_filter) {
		$('#indicator').trigger('click');
		if(['val6'].indexOf(name_input) > -1) {
			let arrUnique = [];
			$('[name='+name_input+']').empty();
			if(data.length > 0) {
				let flagHasValue = false;
				$.each(data, function(i, e) {
					if(ckind_filter === e.CKind) {
						if(arrUnique.length === 0) {
							flagHasValue = true;
							$('[name=val6]').append(`<option value="${e.Code}">${convertHTML(e.Name)}</option>`);
							arrUnique.push(e.Code);
						} else {
							if(arrUnique.indexOf(e.Code) === -1) {
								flagHasValue = true;
								$('[name=val6]').append(`<option value="${e.Code}">${convertHTML(e.Name)}</option>`);
								arrUnique.push(e.Code);
							}
						}
					}
				});
				if(!flagHasValue) { $('[name='+name_input+']').append('<option value="{{ valueUrlEncode('') }}"></option>'); }
			} else {
				$('[name='+name_input+']').append('<option value="{{ valueUrlEncode('') }}"></option>');
			}
		}
		indicatorHide();
	}
</script>

<div class="row ml-2 mr-2">
	<div class="col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　展開パターンマスタ(@if($target === 'show')参照@elseif($target === 'create')登録@elseif($target === 'edit')編集@endif)
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
			<input type="hidden" name="val1" value="{{ old('val1', @$itemData['val1']) }}" />
			@if($target === 'edit')
			<input type="hidden" name="val2" value="{{ old('val2', @$itemData['val2']) }}" />
			<input type="hidden" name="val11" value="{{ valueUrlEncode(old('val11', @$itemData['val11'])) }}" />
			@endif

			<table class="table table-borderless">
				<tbody>
					<tr>
						<td class="align-middle">名称 *：</td>
						<td>
							<input type="text" name="val3" value="{{ old('val3', @$itemData['val3']) }}"
							{{ $target === "show" ? 'disabled="disabled"' : '' }} autocomplete="off" maxlength="40">
						</td>
						<td class="td-error">@include('layouts/error/item', ['name' => 'val3'])</td>
						<td class="align-middle">組区 *：</td>
						<td>
							<select name="val4" id="" {{ $target === "show" ? 'disabled="disabled"' : '' }}>
								@if ($target !== 'create')
								<option value=""></option>
								@endif
								@foreach ($dataSelect['data_4'] as $key => $item)
									<option value="{{ $key }}"
									{{ old('val4', @$itemData['val4']) === $key ? 'selected' : '' }}>{{ $item }}</option>
								@endforeach
							</select>
						</td>
						<td class="td-error">@include('layouts/error/item', ['name' => 'val4'])</td>
					</tr>
					<tr>
						<td class="align-middle">対象区分 *：</td>
						<td>
							<select name="val5" id="" {{ $target === "show" ? 'disabled="disabled"' : '' }}>
								@if ($target !== 'create')
								<option value=""></option>
								@endif
								@foreach ($dataSelect['data_5'] as $key => $item)
									<option value="{{ $key }}"
									{{ old('val5', @$itemData['val5']) === $key ? 'selected' : '' }}>{{ $item }}</option>
								@endforeach
							</select>
						</td>
						<td class="td-error">@include('layouts/error/item', ['name' => 'val5'])</td>
						<td class="align-middle">工程 *：</td>
						<td>
							<select name="val6" id="" {{ $target === "show" ? 'disabled="disabled"' : '' }}>
								@if ($target !== 'create')
								<option value=""></option>
								@endif
								@if(count($dataSelect['data_6']) > 0)
									@foreach ($dataSelect['data_6'] as $item)
										<option value="{{ $item['Code'] }}"
										{{ old('val6', @$itemData['val6']) === $item->Code ? 'selected' : '' }}>{{ $item->Name }}</option>
									@endforeach
								@endif
							</select>
						</td>
						<td class="td-error">@include('layouts/error/item', ['name' => 'val6'])</td>
					</tr>
					<tr>
						<td class="align-middle">工程組区 *：</td>
						<td>
							<select name="val7" id="" {{ $target === "show" ? 'disabled="disabled"' : '' }}>
								@if ($target !== 'create')
								<option value=""></option>
								@endif
								@foreach ($dataSelect['data_7'] as $key => $item)
									<option value="{{ $key }}"
									{{ old('val7', @$itemData['val7']) === $key ? 'selected' : '' }}>{{ $item }}</option>
								@endforeach
							</select>
						</td>
						<td class="td-error">@include('layouts/error/item', ['name' => 'val7'])</td>
						<td class="align-middle">施工棟 *：</td>
						<td>
							<select name="val8" id="" {{ $target === "show" ? 'disabled="disabled"' : '' }}>
								@if ($target !== 'create')
								<option value=""></option>
								@endif
								@if(count($dataSelect['data_8']) > 0)
									@foreach ($dataSelect['data_8'] as $item)
										<option value="{{ $item['Code'] }}"
										{{ old('val8', @$itemData['val8']) === $item['Code'] ? 'selected' : '' }}>{{ $item['Name'] }}</option>
									@endforeach
								@endif
							</select>
						</td>
						<td class="td-error">@include('layouts/error/item', ['name' => 'val8'])</td>
					</tr>
					<tr>
						<td class="align-middle">基準データ *：</td>
						<td>
							<select name="val9" id="" {{ $target === "show" ? 'disabled="disabled"' : '' }}>
								@if ($target !== 'create')
								<option value=""></option>
								@endif
								@foreach ($dataSelect['data_9'] as $key => $item)
									<option value="{{ $key }}"
									{{ old('val9', @$itemData['val9']) === $key ? 'selected' : '' }}>{{ $item }}</option>
								@endforeach
							</select>
						</td>
						<td class="td-error">@include('layouts/error/item', ['name' => 'val9'])</td>
						<td class="align-middle">
							<input type="checkbox" class="input-checkbox" name="val10" id="chk" value="{{ valueUrlEncode(1) }}"
							{{ (int)old('val10', @$itemData['val10']) === valueUrlEncode(1) ? 'checked' : '' }}
							{{ $target === "show" ? 'disabled="disabled"' : '' }}> 検討ケースに適用する
						</td>
						<td class="td-error">@include('layouts/error/item', ['name' => 'val10'])</td>
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