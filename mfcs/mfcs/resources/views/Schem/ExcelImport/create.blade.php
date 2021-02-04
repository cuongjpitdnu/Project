@extends('layouts/mainmenu/menu')
@section('content')
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();
		$('#save').on('click', function(){
			$('#indicator').trigger('click');
			$('#mainform').submit();
		});
		$('#cancel').on('click', function(){
			var url = '{{ url("/") }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/cancel';
			$('#mainform').attr('action', url);
			$('#indicator').trigger('click');
			$('#mainform').submit();
		});
	});
	
</script>
<form action="{{ url('/') }}/{{ $menuInfo->KindURL }}/{{ $menuInfo->MenuURL }}/accept" method="POST" id="mainform">
	@csrf
	<input type="hidden" id="kindid" name="cmn1" value="{{ valueUrlEncode($menuInfo->KindID) }}">
	<input type="hidden" id="menuid" name="cmn2" value="{{ valueUrlEncode($menuInfo->MenuID) }}">
	<input type="hidden" id="" name="val1" value="{{ $request->val1 }}">
	<input type="hidden" id="" name="val2" value="{{ $request->val2 }}">
	<input type="hidden" id="" name="val3" value="{{ $request->val3 }}">
	<input type="hidden" id="" name="val4" value="{{ $request->val4 }}">
	<input type="hidden" id="" name="val6" value="{{ $request->val6}}">
	<input type="hidden" id="" name="val8" value="{{ $request->val8}}">
	
	<div class="row ml-4">
		<div class="col-xs-12">
			<div class="row align-items-center">
				<div class="col-xs-1 text-left m-2 p-2 rounded border">
					■　日程Import-詳細
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
                <div class="col-sm-12" >
                    <div class="info-circle"><i class="fas fa-info icon-small"></i></div>
                    現在の地上中日程との相違点です。よろしければ、保存btnを押してください。
					<table class="table table-row">
						<tbody>
							<tr class="set-color">
                                <th class="align-center">@sortablelink('fld1', 'ブロック名')</th>
                                <th class="align-center">@sortablelink('fld2', '組区')</th>
                                <th class="align-center">@sortablelink('fld3', '舷')</th>
                                <th class="align-center">@sortablelink('fld4', '工程組区')</th>
                                <th class="align-center">@sortablelink('fld5', '工程')</th>
                                <th class="align-center">@sortablelink('fld6', '内容')</th>
                            </tr>
                            @foreach($rows as $row)
							<tr>
								<td class="real-space">{{ $row['fld1'] }}</td>
								<td class="real-space">{{ $row['fld2'] }}</td>
                                <td class="real-space">{{ $row['fld3'] }}</td>
                                <td class="real-space">{{ $row['fld4'] }}</td>
                                <td class="real-space">{{ $row['fld5'] }}</td>
                                <td class="real-space">{{ $row['fld6'] }}</td> 
                            </tr>
                            @endforeach
						</tbody>
                    </table>
				</div>
            </div>
            <div class="row">
				<div class="col-md-5">
					<button type="button" id="save" class="{{ config('system_const.btn_color_save') }}">
						<i class="{{ config('system_const.btn_img_save') }}"></i>{{ config('system_const.btn_char_save') }}
					</button>&emsp;
					<button type="button" id="cancel" class="{{ config('system_const.btn_color_cancel') }}">
						<i class="{{ config('system_const.btn_img_cancel') }}"></i>{{ config('system_const.btn_char_cancel') }}
					</button>
				</div>
				<div class="col-md-7">
					<div class="col-xs-1 p-1"> {{ $rows->appends(request()->query())->links() }} </div>
				</div>
            </div>
		</div>
	</div>
</form>
@endsection