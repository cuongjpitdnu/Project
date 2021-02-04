@extends('errors/errmenu')
@section('content')

<div class="row align-items-center">
	<div class="col-xs-1 text-left m-2 p-2 rounded border">
		■　404 Not Found
	</div>
</div>

<div>
	<h3>ページが見つかりません</h3>
	<p>お探しのページは、移動または削除された可能性があります。</p>
</div>

<div>
	<p><img src="{{ url('/img/err001.png') }}"></p>
</div>

<button type="button" class="toppage {{ config('system_const.btn_color_toppage') }}" onclick="location.href='{{ url('index') }}'">
	@if (config('system_const.btn_img_toppage')!='')
	<i class="{{ config('system_const.btn_img_toppage') }}"></i>
	@endif
	{{ config('system_const.btn_char_toppage') }}
</button>

@endsection
