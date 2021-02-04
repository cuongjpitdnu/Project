<!DOCTYPE html>
<html lang="ja">
	<head>
		<title>生産管理システム</title>
		<meta name="csrf-token" content="{{ csrf_token() }}">
		<link rel="stylesheet" href="{{ url('/css/bootstrap.css') }}">
		<link rel="stylesheet" href="{{ url('/css/base.css') }}">
		<link rel="stylesheet" href="{{ url('/css/jquery-ui.css') }}">
		<link rel="stylesheet" href="{{ url('/css/jquery-ui.structure.css') }}">
		<link rel="stylesheet" href="{{ url('/css/jquery-ui.theme.css') }}">
		<link rel="stylesheet" href="{{ url('/css/jstree/themes/default/style.css') }}">
		<link rel="stylesheet" href="{{ url('/css/jquery-ui.theme.css') }}">
		<link rel="stylesheet" href="{{ url('/css/h5.css') }}">
		<link rel="stylesheet" href="{{ url('/css/indicator.css') }}">
		<link rel="stylesheet" href="{{ url('/css/baseeditor.css') }}">
		<script src="{{ url('/js/jquery-3.5.1.js') }}"></script>
		<script src="{{ url('/js/jquery-ui.js') }}"></script>
		<script src="{{ url('/js/datepicker-ja.js') }}"></script>
		<script src="{{ url('/js/bootstrap.bundle.js') }}"></script>
		<script defer src="{{ url('/fontawesome/js/all.js') }}"></script>
		<script src="{{ url('/js/jstree.js') }}"></script>
		<script src="{{ url('/js/ejs-h5mod.js') }}"></script>
		<script src="{{ url('/js/h5.js') }}"></script>
		<script src="{{ url('/js/indicator-controller.js') }}"></script>
		<script src="{{ url('/js/common.js') }}"></script>
		<meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
		<script>window.onunload = function() {};window.onbeforeunload = function() {}; </script>{{-- Back Forward Cache 対策 --}}
	</head>
	<body>
		<div id="indicator-target">
			<input type="button" class="d-none" id="indicator">
			<div class="container-fluid ops-main">
				<div class="sticky-top">
					<?php $sysURL = ''; ?>
					<div class="row maintitle border border-dark border-bottom-0 align-items-center">
						<div class="col-xs-3 text-nowrap text-left lead p-2 pr-4">
							<a class="btn-block font-weight-bold text-white" href="javascript:if(!indicatorVisible){$('#indicator').trigger('click');location.href='{{ url('/index') }}';}">生産管理システム</a>
						</div>
						@foreach($menuInfo->Kinds as $sysKind)
							@if(isset($menuInfo->KindID) && $sysKind['id'] == $menuInfo->KindID)
								<?php
								$divclass = 'border-bottom border-info';
								$linkclass = 'syssel';
								$sysURL = $sysKind['url'];
								?>
							@else
								<?php
								$divclass = '';
								$linkclass = 'sysnormal';
								?>
							@endif
						<div class="col-xs-1 text-left p-2 {{ $divclass }}">
							<a class="btn-block {{ $linkclass }}" href="javascript:if(!indicatorVisible){$('#indicator').trigger('click');location.href='{{ url('/')}}/{{ $sysKind['url'] }}?cmn1={{ valueUrlEncode($sysKind['id']) }}';}">{{ $sysKind['sysnick'] }}</a>
						</div>
						@endforeach
						<div class="col text-white text-nowrap text-right">
							<i class="fas fa-user"></i>
							{{ $menuInfo->UserName }}
						</div>
					</div>
					<div class="row border border-dark bg-white align-items-center">
						@if(!isset($menuInfo->KindID))
							<div class="col-xs-1 text-left p-2">
								上部メニューよりシステムを選択してください。
							</div>
						@else
							<?php $menusCount = 0; ?>
							@foreach($menuInfo->Menus as $sysMenu)
								@if($sysMenu['syskindid'] != $menuInfo->KindID)
									@continue;
								@endif
								<?php $menusCount++; ?>
								@if(isset($menuInfo->MenuID) && $sysMenu['id'] == $menuInfo->MenuID)
									<?php
									$divclass = 'menucellsel';
									$linkclass = 'menulinksel';
									?>
								@else
									<?php
									$divclass = 'menucellnormal';
									$linkclass = 'menulinknormal';
									?>
								@endif
								<div class="col-xs-1 text-left p-2 {{ $divclass }}">
								<a class="btn-block {{ $linkclass }}" href="javascript:if(!indicatorVisible){$('#indicator').trigger('click');location.href='{{ url('/') }}/{{ $sysURL }}/{{ $sysMenu['url'] }}/index?cmn1={{ valueUrlEncode($sysMenu['syskindid']) }}&cmn2={{ valueUrlEncode($sysMenu['id']) }}';}">{{ $sysMenu['menunick'] }}</a>
								</div>
							@endforeach
							@if($menusCount == 0)
								<div class="col-xs-1 text-left p-2">
									ログインしているユーザーでは操作出来るメニューが有りません。
								</div>
							@endif
						@endif
					</div>
				</div>
				@yield('content')
			</div>
		</div>
	</body>
</html>
