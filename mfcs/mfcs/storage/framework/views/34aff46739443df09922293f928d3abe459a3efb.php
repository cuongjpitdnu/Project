<!DOCTYPE html>
<html lang="ja">
	<head>
		<title>生産管理システム</title>
		<meta name="csrf-token" content="<?php echo e(csrf_token()); ?>">
		<link rel="stylesheet" href="<?php echo e(url('/css/bootstrap.css')); ?>">
		<link rel="stylesheet" href="<?php echo e(url('/css/base.css')); ?>">
		<link rel="stylesheet" href="<?php echo e(url('/css/jquery-ui.css')); ?>">
		<link rel="stylesheet" href="<?php echo e(url('/css/jquery-ui.structure.css')); ?>">
		<link rel="stylesheet" href="<?php echo e(url('/css/jquery-ui.theme.css')); ?>">
		<link rel="stylesheet" href="<?php echo e(url('/css/jstree/themes/default/style.css')); ?>">
		<link rel="stylesheet" href="<?php echo e(url('/css/jquery-ui.theme.css')); ?>">
		<link rel="stylesheet" href="<?php echo e(url('/css/h5.css')); ?>">
		<link rel="stylesheet" href="<?php echo e(url('/css/indicator.css')); ?>">
		<link rel="stylesheet" href="<?php echo e(url('/css/baseeditor.css')); ?>">
		<script src="<?php echo e(url('/js/jquery-3.5.1.js')); ?>"></script>
		<script src="<?php echo e(url('/js/jquery-ui.js')); ?>"></script>
		<script src="<?php echo e(url('/js/datepicker-ja.js')); ?>"></script>
		<script src="<?php echo e(url('/js/bootstrap.bundle.js')); ?>"></script>
		<script defer src="<?php echo e(url('/fontawesome/js/all.js')); ?>"></script>
		<script src="<?php echo e(url('/js/jstree.js')); ?>"></script>
		<script src="<?php echo e(url('/js/ejs-h5mod.js')); ?>"></script>
		<script src="<?php echo e(url('/js/h5.js')); ?>"></script>
		<script src="<?php echo e(url('/js/indicator-controller.js')); ?>"></script>
		<script src="<?php echo e(url('/js/common.js')); ?>"></script>
		<meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
		<script>window.onunload = function() {};window.onbeforeunload = function() {}; </script>
	</head>
	<body>
		<div id="indicator-target">
			<input type="button" class="d-none" id="indicator">
			<div class="container-fluid ops-main">
				<div class="sticky-top">
					<?php $sysURL = ''; ?>
					<div class="row maintitle border border-dark border-bottom-0 align-items-center">
						<div class="col-xs-3 text-nowrap text-left lead p-2 pr-4">
							<a class="btn-block font-weight-bold text-white" href="javascript:if(!indicatorVisible){$('#indicator').trigger('click');location.href='<?php echo e(url('/index')); ?>';}">生産管理システム</a>
						</div>
						<?php $__currentLoopData = $menuInfo->Kinds; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $sysKind): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
							<?php if(isset($menuInfo->KindID) && $sysKind['id'] == $menuInfo->KindID): ?>
								<?php
								$divclass = 'border-bottom border-info';
								$linkclass = 'syssel';
								$sysURL = $sysKind['url'];
								?>
							<?php else: ?>
								<?php
								$divclass = '';
								$linkclass = 'sysnormal';
								?>
							<?php endif; ?>
						<div class="col-xs-1 text-left p-2 <?php echo e($divclass); ?>">
							<a class="btn-block <?php echo e($linkclass); ?>" href="javascript:if(!indicatorVisible){$('#indicator').trigger('click');location.href='<?php echo e(url('/')); ?>/<?php echo e($sysKind['url']); ?>?cmn1=<?php echo e(valueUrlEncode($sysKind['id'])); ?>';}"><?php echo e($sysKind['sysnick']); ?></a>
						</div>
						<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
						<div class="col text-white text-nowrap text-right">
							<i class="fas fa-user"></i>
							<?php echo e($menuInfo->UserName); ?>

						</div>
					</div>
					<div class="row border border-dark bg-white align-items-center">
						<?php if(!isset($menuInfo->KindID)): ?>
							<div class="col-xs-1 text-left p-2">
								上部メニューよりシステムを選択してください。
							</div>
						<?php else: ?>
							<?php $menusCount = 0; ?>
							<?php $__currentLoopData = $menuInfo->Menus; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $sysMenu): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
								<?php if($sysMenu['syskindid'] != $menuInfo->KindID): ?>
									<?php continue; ?>;
								<?php endif; ?>
								<?php $menusCount++; ?>
								<?php if(isset($menuInfo->MenuID) && $sysMenu['id'] == $menuInfo->MenuID): ?>
									<?php
									$divclass = 'menucellsel';
									$linkclass = 'menulinksel';
									?>
								<?php else: ?>
									<?php
									$divclass = 'menucellnormal';
									$linkclass = 'menulinknormal';
									?>
								<?php endif; ?>
								<div class="col-xs-1 text-left p-2 <?php echo e($divclass); ?>">
								<a class="btn-block <?php echo e($linkclass); ?>" href="javascript:if(!indicatorVisible){$('#indicator').trigger('click');location.href='<?php echo e(url('/')); ?>/<?php echo e($sysURL); ?>/<?php echo e($sysMenu['url']); ?>/index?cmn1=<?php echo e(valueUrlEncode($sysMenu['syskindid'])); ?>&cmn2=<?php echo e(valueUrlEncode($sysMenu['id'])); ?>';}"><?php echo e($sysMenu['menunick']); ?></a>
								</div>
							<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
							<?php if($menusCount == 0): ?>
								<div class="col-xs-1 text-left p-2">
									ログインしているユーザーでは操作出来るメニューが有りません。
								</div>
							<?php endif; ?>
						<?php endif; ?>
					</div>
				</div>
				<?php echo $__env->yieldContent('content'); ?>
			</div>
		</div>
	</body>
</html>
<?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/layouts/mainmenu/menu.blade.php ENDPATH**/ ?>