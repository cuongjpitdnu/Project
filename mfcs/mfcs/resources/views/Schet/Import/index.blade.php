@extends('layouts/mainmenu/menu')

@section('content')

<script>
$(function(){
	$('[data-toggle="tooltip"]').tooltip();

	$('#select').click(function(){
		$('#val3').click();
	});

	$('#val3').change(function (e) {
		if(e.target.files.length == 0) {
			$('#filename').val("");
			return;
		}
		var fileName = e.target.files[0].name;
		$('#filename').val(fileName);
	});

	$('#ok').on('click', function(){
		if($('#val3').val() != "" ) {
			const fileSize = ($('#val3').prop('files')[0].size / 1024).toFixed(2); 
			if (fileSize > {{ config('system_config.upload_file_size_max') }} ) { 
				alert( "{{ config('message.msg_file_003') }}"); 
				return;
			} 
		}
		$('#indicator').trigger('click');
		$('#mainform').submit();
	});
})

</script>
	<div class="row ml-2 mr-2">
		<div class="col-md-12 col-xs-12">
			<div class="row align-items-center">
				<div class="col-xs-1 text-left m-2 p-2 rounded border">
					■　日程表取込
				</div>
			</div>
			@if (isset($originalError) && count($originalError) > 0)
			<div class="row">
				<div class="col-xs-12">
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

			<form action="{{ url('/') }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/import" method="POST" id="mainform" enctype="multipart/form-data">
				@csrf
				<input type="hidden" id="kindid" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}">
				<input type="hidden" id="menuid" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}">
				<div class="row head-purple">
					<div class="col-xs-12">条件選択</div>
				</div>
				<div class="row ml-1 mt-3">
					<div class="col-xs-12">
						<table class="table table-borderless">
							<tbody>
								<tr>
									<td class="td-mw-108 align-middle">検討ケース：</td>
									<td>
										<select name="val1" class="">
											@foreach ($projects as $project)
											<option value="{{ valueUrlEncode($project->ID) }}" {{ trim(valueUrlDecode(old('val1', @$request->val1))) === trim($project->ID) ? 'selected' : '' }}>
												{{ $project->ProjectName }}
											</option>
											@endforeach
										</select>
									</td>
									<td class="p-0 align-middle">
										<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val1'])
										</span>
									</td>
									<td class="td-mw-108 align-middle">オーダ：</td>
									<td>
										<select name="val2" class="">
											@foreach ($orders as $order)
											<option value="{{ valueUrlEncode($order->OrderNo) }}" {{ trim(valueUrlDecode(old('val2',@$request->val2))) === trim($order->OrderNo) ? 'selected' : '' }}>
												{{ $order->OrderNo }}
											</option>
											@endforeach
										</select>
									</td>
									<td class="p-0 align-middle">
										<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val2'])
										</span>
									</td>
								</tr>
							</tbody>
						</table>
					</div>
				</div>
				<div class="row head-purple">
					<div class="col-xs-12">ファイル選択</div>
				</div>
				<div class="row ml-1 mt-3">
					<div class="col-xs-12">
						<table class="table table-borderless">
							<tbody>
								<tr>
									<td class="align-middle">
										<input type="file" class="d-none" name="val3" id="val3" value="{{ @$request->val3 }}" required="true">
										<input type="text" class="input-file-width" name="filename" id="filename" value= "{{ @$request->filename }}" autocomplete="off" readonly />
									</td>
									<td class="p-0 align-middle">
										<span class="col-xs-1 p-1">
										@include('layouts/error/item', ['name' => 'val3'])
										</span>
									</td>
									<td class="align-middle">
										<button type="button" name="select" id="select" class="{{ config('system_const.btn_color_file') }}">
											<i class="{{ config('system_const.btn_img_file') }}"></i>
											{{ config('system_const.btn_char_file') }}
										</button>
									</td>
									
								</tr>
							</tbody>
						</table>
					</div>
				</div>
				<div class="row head-purple">
					<div class="col-xs-12">ログ表示</div>
				</div>
				<div class="row ml-1 mt-3">
					<div class="col-xs-12">
						<table class="table table-borderless">
							<tbody>
								<tr>
									<td class="td-mw-108 align-middle">表示件数：</td>
									<td>
										<select name="val5" class="pageunit pageunit-width">
											<option value="{{config('system_const.displayed_results_1')}}" {{ (int)old('val5',valueUrlDecode($request->val5)) === config('system_const.displayed_results_1') ? 'selected' : '' }}>
												{{ config('system_const.displayed_results_1') }}
											</option>
											<option value="{{ config('system_const.displayed_results_2') }}" {{ (int)old('val5',valueUrlDecode($request->val5)) === config('system_const.displayed_results_2') ? 'selected' : '' }}>
												{{ config('system_const.displayed_results_2') }}
											</option>
											<option value="{{ config('system_const.displayed_results_3') }}" {{ (int)old('val5',valueUrlDecode($request->val5)) === config('system_const.displayed_results_3') ? 'selected' : '' }}>
												{{ config('system_const.displayed_results_3') }}
											</option>
										</select>
										※1ページあたり
									</td>
								</tr>
							</tbody>
						</table>
					</div>
				</div>
			</form>

			<div class="row">
				<div class="col-xs-1 p-1 m-3">
					<button type="button" id="ok" class="{{ config('system_const.btn_color_ok') }}"><i class="{{ config('system_const.btn_img_ok') }}"></i>{{ config('system_const.btn_char_ok') }}</button>
				</div>
			</div>
		</div>
	</div>
@endsection
