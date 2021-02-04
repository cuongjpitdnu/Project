@extends('errors/errmenu')
@section('content')

<div class="row align-items-center">
	<div class="col-xs-1 text-left m-2 p-2 rounded border">
		■　500 Internal Server Error
	</div>
</div>

<div>
	<h3>エラーが発生した為、ページを表示できません</h3>
	<p>恐れ入りますが、システム管理者にエラー番号500をご連絡ください。</p>
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
