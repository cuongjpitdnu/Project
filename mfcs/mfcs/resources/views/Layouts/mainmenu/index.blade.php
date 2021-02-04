@extends('layouts/mainmenu/menu')

@section('content')

<div class="row align-items-center">
	<div class="col-xs-1 text-left m-2 p-2 rounded border">
		■　お知らせ　■
	</div>
</div>

@if (session('menu_index_message'))
	<div class="row">
		<div class="col-xs-12">
			<div class="alert alert-danger">
				<ul>
					<li>{{ session('menu_index_message') }}</li>
				</ul>
			</div>
		</div>
	</div>
@endif

<div class="row pl-2">
	<div class="col-xs-12">
		@if (count($informations) == 0)
			表示するお知らせが有りません。
		@else
		<table class="table text-center">
			<tr>
				<th>@sortablelink('fld1', '更新日')</th>
				<th>@sortablelink('fld2', '内容')</th>
			</tr>
			@foreach($informations as $information)
			<tr>
				<td>{{ date('Y/m/d', strtotime($information['fld1'])) }}</td>
				<td class="text-left">{{ unEscapedLine($information['fld2']) }}</td>
			</tr>
			@endforeach
		</table>
		{{ $informations->appends(request()->query())->links() }}
		@endif
	</div>
</div>

@endsection
