@extends('errors/errmenu')
@section('content')

<div class="row align-items-center">
	<div class="col-xs-1 text-left m-2 p-2 rounded border">
		■　419 Original Error
	</div>
</div>

<div>
	<h3>エラーが発生した為、ページを表示できません</h3>
	<p>長時間操作が行われなかったか、不正な操作が行われたため、作業を続行出来ません。
	<br>
	恐れ入りますが、トップページに戻ってから操作をやり直してください。</p>
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
