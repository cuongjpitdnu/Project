<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();

		const dataVal205 = fncJsonParse('{{ json_encode($dataSelect['val205LoadAll']) }}');
		const dataVal206 = fncJsonParse('{{ json_encode($dataSelect['val206LoadAll']) }}');

		$('#save').on('click', function() {
			$('#indicator').trigger('click');
			$('[name=val201]').val($('[name=val201-view]').val());
			$('[name=val202]').val($('[name=val202-view]').val());
			$('[name=val203]').val($('[name=val203-view]').val());
			$('[name=val204]').val($('[name=val204-view]').val());
			$('#mainform').submit();
		});

		$('#cancel').on('click', function() {
			$('#indicator').trigger('click');
			let url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/';
			url += 'manage?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&page={{ $request->page }}';
			url += '&sort={{ $request->sort }}';
			url += '&direction={{ $request->direction }}';
			url += '&val1={{ $request->val1 }}';
			url += '&val2={{ $request->val2 }}';
			url += '&val101={{ $request->val101 }}';
			url += '&val102={{ $request->val102 }}';
			window.location.href = url;
		});

		$('[name=val201-view]').focusout(function(e) {
			bindingSelect('val205', dataVal205, $(this).val(), $('[name=val202-view]').val());
			bindingSelect('val206', dataVal206, $(this).val(), $('[name=val202-view]').val());
		});

		$('[name=val202-view]').change(function(e) {
			bindingSelect('val205', dataVal205, $('[name=val201-view]').val(), $(this).val());
			bindingSelect('val206', dataVal206, $('[name=val201-view]').val(), $(this).val());
		});
	});

	function bindingSelect(name_input, data, name_filter, bkumiku_filter) {
		$('#indicator').trigger('click');
		let arrUnique = [];
		if(name_input === 'val205') {
			$('[name=val205]').empty().append('<option value=""></option>');
			if(data.length > 0) {
				$.each(data, function(i, e) {
					if(name_filter === '' && bkumiku_filter === '') {
						if(arrUnique.length === 0) {
							$('[name=val205]').append(`<option value="${e.Name}">${convertHTML(e.NameShow)}</option>`);
							arrUnique.push(e.NameShow);
						} else {
							if(arrUnique.indexOf(e.NameShow) === -1) {
								$('[name=val205]').append(`<option value="${e.Name}">${convertHTML(e.NameShow)}</option>`);
								arrUnique.push(e.NameShow);
							}
						}
					} else {
						if((name_filter !== '' && bkumiku_filter !== '' && name_filter === e.T_Name && bkumiku_filter === e.T_BKumiku) ||
							(name_filter !== '' && name_filter === e.T_Name && bkumiku_filter === '') ||
							(bkumiku_filter !== '' && bkumiku_filter === e.T_BKumiku && name_filter === '')) {
							if(arrUnique.length === 0) {
								$('[name=val205]').append(`<option value="${e.Name}">${convertHTML(e.NameShow)}</option>`);
								arrUnique.push(e.NameShow);
							} else {
								if(arrUnique.indexOf(e.NameShow) === -1) {
									$('[name=val205]').append(`<option value="${e.Name}">${convertHTML(e.NameShow)}</option>`);
									arrUnique.push(e.NameShow);
								}
							}
						}
					}
				});
			} else { $('[name=val205]').append('<option value=""></option>'); }
		} else {
			$('[name=val206]').empty().append('<option value=""></option>');
			if(data.length > 0) {
				$.each(data, function(i, e) {
					if(name_filter === '' && bkumiku_filter === '') {
						if(arrUnique.length === 0) {
							$('[name=val206]').append(`<option value="${e.BKumiku}">${convertHTML(e.BKumikuName)}</option>`);
							arrUnique.push(e.BKumikuName);
						} else {
							if(arrUnique.indexOf(e.BKumikuName) === -1) {
								$('[name=val206]').append(`<option value="${e.BKumiku}">${convertHTML(e.BKumikuName)}</option>`);
								arrUnique.push(e.BKumikuName);
							}
						}
					} else {
						if((name_filter !== '' && bkumiku_filter !== '' && name_filter === e.T_Name && bkumiku_filter === e.T_BKumiku) ||
							(name_filter !== '' && name_filter === e.T_Name && bkumiku_filter === '') ||
							(bkumiku_filter !== '' && bkumiku_filter === e.T_BKumiku && name_filter === '')) {
							if(arrUnique.length === 0) {
								$('[name=val206]').append(`<option value="${e.BKumiku}">${convertHTML(e.BKumikuName)}</option>`);
								arrUnique.push(e.BKumikuName);
							} else {
								if(arrUnique.indexOf(e.BKumikuName) === -1) {
									$('[name=val206]').append(`<option value="${e.BKumiku}">${convertHTML(e.BKumikuName)}</option>`);
									arrUnique.push(e.BKumikuName);
								}
							}
						}
					}
				});
			} else { $('[name=val206]').append('<option value=""></option>'); }
		}
		indicatorHide();
	}
</script>

<div class="row ml-2 mr-2">
	<div class="col-md-12 col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　項目定義@if($target === 'show')(参照)@elseif($target === 'create')(登録)@elseif($target === 'edit')(更新)@endif
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
			<input type="hidden" name="method" value="{{ $target }}" />
			<input type="hidden" name="val201" value="" />
			<input type="hidden" name="val202" value="" />
			<input type="hidden" name="val203" value="" />
			<input type="hidden" name="val204" value="" />

			<div class="row head-purple">
				<div class="col-xs-12">{{ ((int)valueUrlDecode($request->val103) === 1) ? '搭載日程レベルデータ' : '搭載日程データ' }}</div>
			</div>

			<div class="row ml-1 mt-3">
				<div class="col-xs-12">
					<table class="table table-borderless">
						<tbody>
							<tr>
								<td class="align-middle">{{ (int)valueUrlDecode($request->val1) === config('system_const.c_kind_giso') ? '区画名' : 'ブロック名' }} *：</td>
								<td><input type="text" name="val201-view" id="" maxlength="50"
									value="{{ old('val201', @$itemData['T_Name']) }}" autocomplete="off"
									{{ in_array($target, array('show', 'edit')) ? 'disabled="disabled"' : '' }}></td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val201'])
									</span>
								</td>
								<td class="td-mw-108 align-middle">組区 *：</td>
								<td>
									<select name="val202-view" id=""
									{{ in_array($target, array('show', 'edit')) ? 'disabled="disabled"' : '' }}>
										<option value="" hidden></option>
										@foreach($dataSelect['val202'] as $key => $data)
											<option value="{{ valueUrlEncode($key) }}"
											{{ old('val202', @$itemData['T_BKumiku']) === valueUrlEncode($key) ? 'selected' : '' }}>{{ $data }}</option>
										@endforeach
									</select>
								</td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val202'])
									</span>
								</td>
							</tr>
						</tbody>
					</table>
				</div>
			</div>

			<div class="row head-purple">
				<div class="col-xs-12">中日程データ</div>
			</div>

			<div class="row ml-1 mt-3">
				<div class="col-xs-12">
					<table class="table table-borderless">
						<tbody>
							<tr>
								<td class="align-middle">{{ (int)valueUrlDecode($request->val1) === config('system_const.c_kind_giso') ? '区画名' : 'ブロック名' }} *：</td>
								<td><input type="text" name="val203-view" id="" maxlength="50" autocomplete="off"
									value="{{ old('val203', @$itemData['Name']) }}"
									{{ in_array($target, ["edit", "show"]) ? 'disabled="disabled"' : '' }} /></td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val203'])
									</span>
								</td>
								<td class="align-middle">組区 *：</td>
								<td>
									<select name="val204-view" id=""
									{{ in_array($target, ["edit", "show"]) ? 'disabled="disabled"' : '' }}>
										<option value="" hidden></option>
										@foreach($dataSelect['val204'] as $key => $data)
											<option value="{{ valueUrlEncode($key) }}"
											{{ old('val204', @$itemData['BKumiku']) === valueUrlEncode($key) ? 'selected' : '' }}>{{ $data }}</option>
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
								<td class="align-middle">{{ (int)valueUrlDecode($request->val1) === 2 ? '次区画名：' : '次ブロック名：' }}</td>
								<td>
									<select name="val205" id=""
									{{ $target === "show" ? 'disabled="disabled"' : '' }}>
									<option value=""></option>
									@if(count($dataSelect['val205']) > 0)
										@foreach ($dataSelect['val205'] as $item)
											<option class="real-space" value="{{ $item->Name }}"
											{{ old('val205', @$itemData['N_Name']) === $item->Name ? 'selected' : '' }}>{{ $item->NameShow }}</option>
										@endforeach
									@endif
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
									@if(count($dataSelect['val206']) > 0)
										@foreach ($dataSelect['val206'] as $item)
											<option value="{{ $item->BKumiku }}"
											{{ old('val206', @$itemData['N_BKumiku']) === $item->BKumiku ? 'selected' : '' }}>{{ $item->BKumikuName }}</option>
										@endforeach
									@endif
									</select>
								</td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val206'])
									</span>
								</td>
							</tr>
							<tr>
								<td class="align-middle">部位：</td>
								<td><input type="text" name="val207" id="" maxlength="2" autocomplete="off"
									value="{{ old('val207', @$itemData['Struct']) }}"
									{{ $target === "show" ? 'disabled="disabled"' : '' }}></td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val207'])
									</span>
								</td>
								<td class="align-middle">カテゴリー：</td>
								<td><input type="text" name="val208" id="" maxlength="5" autocomplete="off"
									value="{{ old('val208', @$itemData['Category']) }}"
									{{ $target === "show" ? 'disabled="disabled"' : '' }}></td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val208'])
									</span>
								</td>
							</tr>
							<tr>
								<td class="align-middle">代表幅：</td>
								<td><input type="text" name="val209" id="" maxlength="5" autocomplete="off"
									value="{{ old('val209', @$itemData['Width']) }}"
									{{ $target === "show" ? 'disabled="disabled"' : '' }}></td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val209'])
									</span>
								</td>
								<td class="align-middle">代表長：</td>
								<td><input type="text" name="val210" id="" maxlength="5" autocomplete="off"
									value="{{ old('val210', @$itemData['Length']) }}"
									{{ $target === "show" ? 'disabled="disabled"' : '' }}></td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val210'])
									</span>
								</td>
							</tr>
							<tr>
								<td class="align-middle">代表高：</td>
								<td><input type="text" name="val211" id="" maxlength="5" autocomplete="off"
									value="{{ old('val211', @$itemData['Height']) }}"
									{{ $target === "show" ? 'disabled="disabled"' : '' }}></td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val211'])
									</span>
								</td>
								<td class="align-middle">代表重量：</td>
								<td><input type="text" name="val212" id="" maxlength="9" autocomplete="off"
									value="{{ old('val212', @$itemData['Weight']) }}"
									{{ $target === "show" ? 'disabled="disabled"' : '' }}></td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val212'])
									</span>
								</td>
							</tr>
							<tr>
								<td class="align-middle">工作図No：</td>
								<td><input type="text" name="val213" id="" maxlength="6" autocomplete="off"
									value="{{ old('val213', @$itemData['Zu_No']) }}"
									{{ $target === "show" ? 'disabled="disabled"' : '' }}></td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val213'])
									</span>
								</td>
								<td class="align-middle">殻艤重量：</td>
								<td><input type="text" name="val214" id="" maxlength="9" autocomplete="off"
									value="{{ old('val214', @$itemData['KG_Weight']) }}"
									{{ $target === "show" ? 'disabled="disabled"' : '' }}></td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val214'])
									</span>
								</td>
							</tr>
							<tr>
								<td class="align-middle">重量確定：</td>
								<td>
									<select name="val215" id=""
									{{ $target === "show" ? 'disabled="disabled"' : '' }}>
										<option value="{{ valueUrlEncode(0) }}"
										{{ (int)old('val215', @$itemData['True_Weight']) === 0 ? 'selected' : '' }}>0：未確定</option>
										<option value="{{ valueUrlEncode(1) }}"
										{{ (int)old('val215', @$itemData['True_Weight']) === 1 ? 'selected' : '' }}>1：確定</option>
									</select>
								</td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val215'])
									</span>
								</td>
								<td class="align-middle">曲がり：</td>
								<td>
									<select name="val216" id=""
									{{ $target === "show" ? 'disabled="disabled"' : '' }}>
										<option value="{{ valueUrlEncode(0) }}"
										{{ (int)old('val216', @$itemData['Is_Magari']) === 0 ? 'selected' : '' }}>0：曲がりなし</option>
										<option value="{{ valueUrlEncode(1) }}"
										{{ (int)old('val216', @$itemData['Is_Magari']) === 1 ? 'selected' : '' }}>1：曲がり</option>
									</select>
								</td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val216'])
									</span>
								</td>
							</tr>
						</tbody>
					</table>
				</div>
			</div>
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