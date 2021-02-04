@extends('layouts/mainmenu/menu')
@section('content')
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();
		$('[name=val101], [name=val102]').on('change', function(i, e) {
			$('#indicator').trigger('click');
			let val102 = ($(this).attr('name') == 'val101') ? '' : $('[name=val102]').val();
			let url = '{{ url("/") }}/schem/item/manage';
			url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&val1={{ valueUrlEncode($request->val1) }}&val2={{ valueUrlEncode($request->val2) }}';
			url += '&val101='+$('[name=val101]').val();
			url += '&val102='+val102;
			window.location.href = url;
		});

		$('button[name=create]').on('click', function() {
			$('#indicator').trigger('click');
			let url = '{{ url("/") }}/schem/item/create';
			url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&page={{ isset($request->page) ? $request->page : 1 }}';
			url += '&sort={{ isset($request->sort) ? $request->sort : "" }}';
			url += '&direction={{ isset($request->direction) ? $request->direction : "asc" }}';
			url += '&val1={{ valueUrlEncode($request->val1) }}&val2={{ valueUrlEncode($request->val2) }}';
			url += '&val101='+$('[name=val101]').val();
			url += '&val102='+$('[name=val102]').val();
			url += '&val103='+$('[name=val103temp]').val();
			window.location.href = url;
		});

		$('.detail').on('click', function() {
			$('#indicator').trigger('click');
			let url = '{{ url("/") }}/schem/item/pmanage';
			url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&bpage={{ isset($request->page) ? $request->page : 1 }}';
			url += '&bsort={{ isset($request->sort) ? $request->sort : "" }}';
			url += '&bdirection={{ isset($request->direction) ? $request->direction : "asc" }}';
			url += '&val1={{ valueUrlEncode($request->val1) }}&val2={{ valueUrlEncode($request->val2) }}';
			url += '&val101='+$('[name=val101]').val();
			url += '&val102='+$('[name=val102]').val();
			url += '&val103='+$(this).attr('group');
			url += '&val104='+$(this).attr('no');
			window.location.href = url;
		});

		$('.edit').on('click', function() {
			$('#indicator').trigger('click');
			let url = '{{ url("/") }}/schem/item/edit';
			url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&page={{ isset($request->page) ? $request->page : 1 }}';
			url += '&sort={{ isset($request->sort) ? $request->sort : "" }}';
			url += '&direction={{ isset($request->direction) ? $request->direction : "asc" }}';
			url += '&val1={{ valueUrlEncode($request->val1) }}&val2={{ valueUrlEncode($request->val2) }}';
			url += '&val101='+$('[name=val101]').val();
			url += '&val102='+$('[name=val102]').val();
			url += '&val103='+$(this).attr('group');
			url += '&val104='+$(this).attr('no');
			window.location.href = url;
		});

		$('.show').on('click', function() {
			$('#indicator').trigger('click');
			let url = '{{ url("/") }}/schem/item/show';
			url += '?cmn1={{ valueUrlEncode($menuInfo->KindID) }}&cmn2={{ valueUrlEncode($menuInfo->MenuID) }}';
			url += '&page={{ isset($request->page) ? $request->page : 1 }}';
			url += '&sort={{ isset($request->sort) ? $request->sort : "" }}';
			url += '&direction={{ isset($request->direction) ? $request->direction : "asc" }}';
			url += '&val1={{ valueUrlEncode($request->val1) }}&val2={{ valueUrlEncode($request->val2) }}';
			url += '&val101='+$('[name=val101]').val();
			url += '&val102='+$('[name=val102]').val();
			url += '&val103='+$(this).attr('group');
			url += '&val104='+$(this).attr('no');
			window.location.href = url;
		});

		$('.delete').on('click', function() {
			if(confirm("{{ config('message.msg_cmn_if_001') }}")) {
				$('#indicator').trigger('click');
				$('[name=val103]').val($(this).attr('group'));
				$('[name=val104]').val($(this).attr('no'));
				let url = '{{ url("/") }}/schem/item/delete';
				let pageRedirect = '{{ (count($dataA4) == 1) ? (isset($request->page) ? ($request->page - 1) : 1) : (isset($request->page) ? $request->page : 1) }}';
				$('#form-control').attr('action', url);
				$('input[name=page]').val(pageRedirect);
				$('#form-control').submit();
			}
		});
	});
</script>

<div class="row ml-4">
	<div class="col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　項目定義
			</div>
		</div>
		@if (isset($originalError) && count($originalError) > 0)
		<div class="row">
			<div class="col-xs-12" id="area-error">
				<div class="alert alert-danger">
					<ul>
						@foreach ($originalError as $error)
						<li>{{ $error }}</li>
						@endforeach
					</ul>
				</div>
			</div>
		</div>
		@endif

		{{-- A1 --}}
		<div class="row align-items-center">
			<div class="col-xs-12 text-left m-2 p-2">中日程区分：{{
				((int)$request->get('val1') === config('system_const.c_kind_chijyo')) ? config('system_const.c_name_chijyo') :
				(((int)$request->get('val1') === config('system_const.c_kind_gaigyo')) ? config('system_const.c_name_gaigyo') :
				(((int)$request->get('val1') === config('system_const.c_kind_giso') ? config('system_const.c_name_giso') : '')))
			}}</div>
		</div>
		<form action="" method="POST" id="form-control">
			@csrf
			<input type="hidden" id="kindid" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}" />
			<input type="hidden" id="menuid" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}" />
			<input type="hidden" name="val1" value="{{ valueUrlEncode($request->val1) }}" />
			<input type="hidden" name="val2" value="{{ valueUrlEncode($request->val2) }}" />
			<input type="hidden" name="page" value="{{ isset($request->page) ? $request->page : 1 }}" />
			<input type="hidden" name="sort" value="{{ isset($request->sort) ? $request->sort : "" }}" />
			<input type="hidden" name="direction" value="{{ isset($request->direction) ? $request->direction : "asc" }}" />
			<input type="hidden" name="val103" value="" />
			<input type="hidden" name="val104" value="" />
			<input type="hidden" name="val103temp" value="{{ valueUrlEncode($val103) }}" />

			<div class="row align-items-center">
				<div class="col-xs-3 text-left ml-2 pl-2">ケース：
					<select name="val101" id="" class="">
						@if(count($selectVal101) > 0)
							@foreach($selectVal101 as $item)
							<option value="{{ $item->ID }}"
								{{ (old('val101', valueUrlEncode(@$request->val101)) === $item->ID) ? 'selected' : '' }}>{{ $item->ProjectName }}</option>
							@endforeach
						@else
						<option value=""></option>
						@endif
					</select>
					<span class="col-xs-1 p-1">
						@include('layouts/error/item', ['name' => 'val101'])
					</span>
				</div>
				<div class="col-xs-3 text-left ml-2 pl-2">オーダ：
					<select name="val102" id="" class="">
						@if(count($selectVal102) > 0)
							@foreach($selectVal102 as $item)
							<option value="{{ $item->val102 }}"
								{{ old('val101', valueUrlEncode(@$request->val102)) === $item->val102 ? 'selected' : '' }}>{{ $item->val102Name }}</option>
							@endforeach
						@else
							<option value=""></option>
						@endif
					</select>
					<span class="col-xs-1 p-1">
						@include('layouts/error/item', ['name' => 'val102'])
					</span>
				</div>
				@if (!$menuInfo->IsReadOnly)
				<div class="col-xs-3 ml-3">
					<button type="button" name="create" class="create {{ config('system_const.btn_color_new') }}">
						@if (config('system_const.btn_img_new') !== '')
						<i class="{{ config('system_const.btn_img_new') }}"></i>
						@endif
						{{ config('system_const.btn_char_new') }}</button>
				</div>
				@endif
			</div>

			<div class="row mt-2">
				<div class="col-xs-12">
					<table class="table ml-2" id="tbl-content">
						<thead>
							<tr>
								<th class="text-center">@sortablelink('Name', (((int)$request->get('val1') === config('system_const.c_kind_giso'))
																				? '区画名' : 'ブロック'))</th>
								<th class="text-center">@sortablelink('BKumiku', '組区')</th>
								<th class="text-center">@sortablelink('N_Name', (((int)$request->get('val1') === config('system_const.c_kind_giso'))
																				? '次区画名' : '次ブロック'))</th>
								<th class="text-center">@sortablelink('N_BKumiku', '次組区')</th>
								<th class="text-center">@sortablelink('Struct', '部位')</th>
								<th class="text-center">@sortablelink('Category', 'カテゴリー')</th>
								<th class="text-center">@sortablelink('Width', '代表幅')</th>
								<th class="text-center">@sortablelink('Length', '代表長')</th>
								<th class="text-center">@sortablelink('Height', '代表高')</th>
								<th class="text-center">@sortablelink('Weight', '代表重量')</th>
								<th class="text-center">@sortablelink('Zu_No', '工作図No')</th>
								<th class="text-center">@sortablelink('KG_Weight', '殻艤重量')</th>
								<th class="text-center">@sortablelink('True_Weight', '重量確定')</th>
								<th class="text-center">@sortablelink('Is_Magari', '曲がり')</th>
								<th class="text-center"></th>
								<th class="text-center"></th>
								<th class="text-center"></th>
							</tr>
						</thead>
						<tbody>
							@foreach($dataA4 as $row)
							<tr>
								<td class="align-middle real-space">{{ $row['Name'] }}</td>
								<td class="align-middle real-space">{{ $row['BKumiku'] }}</td>
								<td class="align-middle real-space">{{ $row['N_Name'] }}</td>
								<td class="align-middle real-space">{{ $row['N_BKumiku'] }}</td>
								<td class="align-middle real-space">{{ $row['Struct'] }}</td>
								<td class="align-middle real-space">{{ $row['Category'] }}</td>
								<td class="align-middle text-right real-space">{{ $row['Width'] }}</td>
								<td class="align-middle text-right real-space">{{ $row['Length'] }}</td>
								<td class="align-middle text-right real-space">{{ $row['Height'] }}</td>
								<td class="align-middle text-right real-space">{{ $row['Weight'] }}</td>
								<td class="align-middle real-space">{{ $row['Zu_No'] }}</td>
								<td class="align-middle text-right real-space">{{ $row['KG_Weight'] }}</td>
								<td class="align-middle real-space">{{ $row['True_Weight'] }}</td>
								<td class="align-middle real-space">{{ $row['Is_Magari'] }}</td>
								<td class="align-middle text-center">
									<button type="button" class="detail {{ config('system_const.btn_color_rowdetail') }}"
										no="{{ $row['No'] }}" group="{{ $row['GroupData'] }}">
										@if (config('system_const.btn_img_rowdetail') != '')
											<i class="{{ config('system_const.btn_img_rowdetail') }}"></i>
										@endif
										{{ config('system_const.btn_char_rowdetail') }}</button>
								</td>
								<td class="align-middle text-center">
									@if (!$menuInfo->IsReadOnly)
										<button type="button" class="edit {{ config('system_const.btn_color_rowedit') }}"
											no="{{ $row['No'] }}" group="{{ $row['GroupData'] }}">
											@if (config('system_const.btn_img_rowedit') != '')
												<i class="{{ config('system_const.btn_img_rowedit') }}"></i>
											@endif
											{{ config('system_const.btn_char_rowedit') }}</button>
									@else
										<button type="button" class="show {{ config('system_const.btn_color_rowinfo') }}"
											no="{{ $row['No'] }}" group="{{ $row['GroupData'] }}">
											@if (config('system_const.btn_img_rowinfo') != '')
												<i class="{{ config('system_const.btn_img_rowinfo') }}"></i>
											@endif
											{{ config('system_const.btn_char_rowinfo') }}</button>
									@endif
								</td>
								<td class="align-middle text-center">
									@if (!$menuInfo->IsReadOnly)
									<button type="button" class="delete {{ config('system_const.btn_color_rowdelete') }}"
										no="{{ $row['No'] }}" group="{{ $row['GroupData'] }}">
										@if (config('system_const.btn_img_rowdelete') != '')
											<i class="{{ config('system_const.btn_img_rowdelete') }}"></i>
										@endif
										{{ config('system_const.btn_char_rowdelete') }}</button>
									@endif
								</td>
							</tr>
							@endforeach
						</tbody>
					</table>
					{{ $dataA4->appends(request()->query())->links() }}
				</div>
			</div>
		</form>
	</div>
</div>
@endsection