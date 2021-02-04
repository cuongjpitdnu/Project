@extends('layouts/mainmenu/menu')
@section('content')
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();
		$('#save').on('click', function(e) {
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/accept';
			$('#mainform').attr('action', url);
			$('#indicator').trigger('click');
			$('#mainform').submit();
		});
		$('#cancel').on('click', function(e) {
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/cancel';
			$('#mainform').attr('action', url);
			$('#indicator').trigger('click');
			$('#mainform').submit();
		});
	});
</script>

<form action="" method="POST" id="mainform">
	@csrf
	<input type="hidden" id="kindid" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}" />
	<input type="hidden" id="menuid" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}" />
	<input type="hidden" id="" name="val1" value="{{ $request->val1 }}" />
	<input type="hidden" id="" name="val2" value="{{ $request->val2 }}" />
	<input type="hidden" id="" name="val3" value="{{ $request->val3 }}" />
	<input type="hidden" id="" name="val4" value="{{ $request->val4 }}" />
	<input type="hidden" id="" name="val5" value="{{ $request->val5 }}" />
	<input type="hidden" id="" name="val6" value="{{ $request->val6 }}" />
	<div class="row ml-4">
		<div class="col-xs-12">
			<div class="row align-items-center">
				<div class="col-xs-1 text-left m-2 p-2 rounded border">
					■　日程Import - 詳細
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

			<div class="row">
				<div class="col-sm-12">
					<div class="info-circle"><i class="fas fa-info icon-small"></i></div> 現在の小日程との相違点です。よろしければ、保存ボタンを押してください。
					<table class="table table-row">
						<tbody>
							<tr class="set-color">
								<th class="text-center">@sortablelink('fld1', '名称1')</th>
								<th class="text-center">@sortablelink('fld2', '名称2')</th>
								<th class="text-center">@sortablelink('fld3', '名称3')</th>
								<th class="text-center">@sortablelink('fld4', '組区')</th>
								<th class="text-center">@sortablelink('fld5', '施工棟')</th>
								<th class="text-center">@sortablelink('fld6', '装置')</th>
								<th class="text-center">@sortablelink('fld7', '職種')</th>
								<th class="text-center">@sortablelink('fld8', '管理物量コード')</th>
								<th class="text-center">@sortablelink('fld9', '消込管理')</th>
								<th class="text-center">@sortablelink('fld10', '消込方式')</th>
								<th class="text-center">@sortablelink('fld11', '物量')</th>
								<th class="text-center">@sortablelink('fld12', '着工日')</th>
								<th class="text-center">@sortablelink('fld13', '完工日')</th>
								<th class="text-center">@sortablelink('fld14', 'アイテム')</th>
								<th class="text-center">@sortablelink('fld15', 'HC')</th>
								<th class="text-center">@sortablelink('fld16', 'HJ')</th>
								<th class="text-center">@sortablelink('fld17', 'HS')</th>
								<th class="text-center">@sortablelink('fld18', 'HK')</th>
								<th class="text-center">@sortablelink('fld19', 'WBSコード')</th>
								<th class="text-center">@sortablelink('fld20', '内容')</th>
							</tr>

							@foreach($rows as $row)
							<tr>
								<td class="real-space align-middle">{{ $row['fld1'] }}</td>
								<td class="real-space align-middle">{{ $row['fld2'] }}</td>
								<td class="real-space align-middle">{{ $row['fld3'] }}</td>
								<td class="real-space align-middle">{{ $row['fld4'] }}</td>
								<td class="real-space align-middle">{{ $row['fld5'] }}</td>
								<td class="real-space align-middle">{{ $row['fld6'] }}</td>
								<td class="real-space align-middle">{{ $row['fld7'] }}</td>
								<td class="real-space align-middle">{{ $row['fld8'] }}</td>
								<td class="real-space align-middle">{{ $row['fld9'] }}</td>
								<td class="real-space align-middle">{{ $row['fld10'] }}</td>
								<td class="real-space align-middle">{{ $row['fld11'] }}</td>
								<td class="real-space align-middle">{{ $row['fld12'] }}</td>
								<td class="real-space align-middle">{{ $row['fld13'] }}</td>
								<td class="real-space align-middle">{{ $row['fld14'] }}</td>
								<td class="real-space align-middle">{{ $row['fld15'] }}</td>
								<td class="real-space align-middle">{{ $row['fld16'] }}</td>
								<td class="real-space align-middle">{{ $row['fld17'] }}</td>
								<td class="real-space align-middle">{{ $row['fld18'] }}</td>
								<td class="real-space align-middle">{{ $row['fld19'] }}</td>
								<td class="real-space align-middle">{{ $row['fld20'] }}</td>
							</tr>
							@endforeach
						</tbody>
					</table>   
				</div>
			</div>
			<div class="row">
				{{ $rows->appends(request()->query())->links() }}
			</div>
			<div class="row">
				<div class="col-xs-1 p-1">
					<button type="button" id="save" class="{{ config('system_const.btn_color_save') }}">
					<i class="{{ config('system_const.btn_img_save') }}"></i>{{ config('system_const.btn_char_save') }}
					</button>
				</div>
				<div class="col-xs-1 p-1">
					<button type="button" id="cancel" class="{{ config('system_const.btn_color_cancel') }}">
					<i class="{{ config('system_const.btn_img_cancel') }}"></i>{{ config('system_const.btn_char_cancel') }}
					</button>
				</div>
			</div>
		</div>
	</div>
</form>
@endsection