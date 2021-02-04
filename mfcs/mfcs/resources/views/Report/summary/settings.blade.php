@extends('layouts/mainmenu/menu')
@section('content')
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();
	});
</script>
<style>
	.td-mw-160 {
		min-width: 160px !important;
	}
	.select-h-100 {
		height: 90px !important;
	}
	.select-h-204 {
		height: 204px !important;
	}
</style>
<div class="row ml-2 mr-2">
	<div class="col-md-12 col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　汎用集計表
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
		<form action="{{ url('/') }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/settings" method="POST" id="mainform" enctype="multipart/form-data">
			@csrf
			<input type="hidden" id="kindid" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}" />
			<input type="hidden" id="menuid" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}" />
			
			<div class="row head-purple">
				<div class="col-xs-12">項目選択</div>
			</div>
			<div class="row ml-1">
				<div class="col-xs-12">
					<table class="table table-borderless mb-0">
						<tr>
							<td class="td-mw-160">出力項目選択：</td>
							<td class="align-middle">
								<select class="form-select select-h-100 val101" multiple name="val101">
									<option value="1">Val 1</option>
									<option value="2">Val 2</option>
									<option value="3">Val 3</option>
									<option value="4">Val 4</option>
								</select>
							</td>
							<td class="p-0 align-middle">
								<span class="col-xs-1 p-1">
								@include('layouts/error/item', ['name' => 'val101'])
								</span>
							</td>
						</tr>
					</table>
				</div>
			</div>

			<div class="row head-purple">
				<div class="col-xs-12">並び順選択</div>
			</div>
			<div class="row ml-1">
				<div class="col-xs-12">
					<table class="table table-borderless mb-0">
						<tr>
							<td rowspan="2" class="td-mw-160">集約キー項目選択：</td>
							<td rowspan="2" class="align-middle">
								<select class="form-select select-h-100 val101" multiple name="val101">
								</select>
							</td>
							<td class="align-middle pb-0">
								<button type="button" class="btn btn-outline-primary btn-sm">←</button>
							</td>
							<td rowspan="5" class="align-middle">
								<select class="form-select select-h-204 val101" multiple name="val101">
								</select>
							</td>
						</tr>
						<tr>
							<td class="align-middle pt-0">
								<button type="button" class="btn btn-outline-primary btn-sm">→</button>
							</td>
						</tr>
						<tr>
						</tr>
						<tr>
							<td rowspan="2" class="td-mw-160">集約キー項目選択：</td>
							<td rowspan="2" class="align-middle">
								<select class="form-select select-h-100 val101" multiple name="val101">
								</select>
							</td>
							<td class="align-middle pb-0">
								<button type="button" class="btn btn-outline-primary btn-sm">←</button>
							</td>
						</tr>
						<tr>
							<td class="align-middle pt-0">
								<button type="button" class="btn btn-outline-primary btn-sm">→</button>
							</td>
						</tr>

						{{-- <td class="p-0 align-middle">
							<span class="col-xs-1 p-1">
							@include('layouts/error/item', ['name' => 'val101'])
							</span>
						</td> --}}
					</table>
				</div>
			</div>

			<div class="row head-purple">
				<div class="col-xs-12">小計・合計項目選択エリア</div>
			</div>
			<div class="row ml-1">
				<div class="col-xs-12">
					<table class="table table-borderless mb-0">
						<tr>
							<td class="td-mw-160">条件項目選択：</td>
							<td>
								<select class="form-select select-h-100 val101" multiple name="val101">

								</select>
							</td>
							<td class="p-0 align-middle">
								<span class="col-xs-1 p-1">
								@include('layouts/error/item', ['name' => 'val101'])
								</span>
							</td>
						</tr>
					</table>
				</div>
			</div>

			<div class="row head-purple">
				<div class="col-xs-12">条件選択エリア</div>
			</div>
			<div class="row ml-1">
				<div class="col-xs-12">
					<table class="table table-borderless mb-0">
						<tr>
							<td class="td-mw-160">条件項目選択：</td>
							<td>
								<select class="form-select select-h-100 val101" multiple name="val101">

								</select>
							</td>
							<td class="p-0 align-middle">
								<span class="col-xs-1 p-1">
								@include('layouts/error/item', ['name' => 'val101'])
								</span>
							</td>
						</tr>
					</table>
				</div>
			</div>

			
		</form>
		<div class="row">
			<div class="col-xs-1 p-1">
				<button type="button" id="save" class="{{ config('system_const.btn_color_ok') }}">
					<i class="{{ config('system_const.btn_img_ok') }}"></i>{{ config('system_const.btn_char_ok') }}
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
@endsection