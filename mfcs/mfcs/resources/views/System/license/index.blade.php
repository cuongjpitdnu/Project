@extends('layouts/mainmenu/menu')
@section('content')

<div class="row align-items-center">
	<div class="col-xs-1 text-left m-2 p-2 rounded border">
		■　ライセンス情報
	</div>
</div>

<div>
	<p>
	本製品は以下のソフトウェアを使用しています。（「著作権表示」「本許諾表示」はプログラム内に記載している為、省略）
	</p>
</div>

<div class="row pl-2">
	<div class="col-xs-12">

		<table class="table text-center">
			<tr>
				<th>ソフトウェア名</th>
				<th>ライセンス</th>
				<th>バージョン</th>
			</tr>
			<tr>
				<td class="text-left">Bootstrap</td>
				<td class="text-left">MIT License</td>
				<td class="text-left">4.3.1</td>
			</tr>
			<tr>
				<td class="text-left">Font Awesome</td>
				<td class="text-left">MIT License</td>
				<td class="text-left">5.13.1</td>
			</tr>
			<tr>
				<td class="text-left">hifive</td>
				<td class="text-left">Apache License 2.0</td>
				<td class="text-left">1.3.3</td>
			</tr>
			<tr>
				<td class="text-left">jQuery</td>
				<td class="text-left">MIT License</td>
				<td class="text-left">3.5.1</td>
			</tr>
			<tr>
				<td class="text-left">jQuery UI</td>
				<td class="text-left">MIT License</td>
				<td class="text-left">1.12.1</td>
			</tr>
			<tr>
				<td class="text-left">jsTree</td>
				<td class="text-left">MIT License</td>
				<td class="text-left">3.3.10</td>
			</tr>
		</table>

	</div>
</div>


@endsection

