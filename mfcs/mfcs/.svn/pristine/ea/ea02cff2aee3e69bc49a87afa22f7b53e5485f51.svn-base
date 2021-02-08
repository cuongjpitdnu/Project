@extends('layouts/mainmenu/menu')
@section('content')

<form action="" method="POST" id="mainform">
	@csrf
	<div class="row ml-4">
		<div class="col-xs-12">
			<div class="row align-items-center">
				<div class="col-xs-1 text-left m-2 p-2 rounded border">
					■　物量定義 - 詳細
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
					<div class="info-circle"><i class="fas fa-info icon-small"></i></div> 取り込めなかった物量は次の通りです
					<table class="table">
						<tbody>
							<tr class="set-color">
								<th class="text-center">@sortablelink('fld1', '搭載名')</th>
								<th class="text-center">@sortablelink('fld2', '搭載組区')</th>
								<th class="text-center">@sortablelink('fld3', '中日程名')</th>
								<th class="text-center">@sortablelink('fld4', '中日程組区')</th>
								<th class="text-center">@sortablelink('fld5', '工程')</th>
								<th class="text-center">@sortablelink('fld6', '工程組区')</th>
								<th class="text-center">@sortablelink('fld7', '内容')</th>
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
							</tr>
							@endforeach
						</tbody>
					</table>   
				</div>
			</div>
			<div class="row">
				{{ $rows->appends(request()->query())->links() }}
			</div>
		</div>
	</div>
</form>
@endsection