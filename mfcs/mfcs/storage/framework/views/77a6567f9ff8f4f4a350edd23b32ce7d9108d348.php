<?php $__env->startSection('content'); ?>
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();
		$('#save').on('click', function(e) {
			$('#indicator').trigger('click');
			$('#mainform').submit();
		});

		$('#cancel').on('click', function(e) {
			$('#indicator').trigger('click');
			var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/';
			url += 'index?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
			window.location.href = url;
		});
	});
</script>

<style>
    /* table { margin-top: 16px; }
    table tr td { text-align: left; vertical-align: middle !important; border: none !important; } */
</style>
<link rel="stylesheet" href="<?php echo e(url('/css/baseeditar.css')); ?>">
<div class="row ml-2 mr-2">
	<div class="col-md-12 col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■検討ケース作成（空のケース作成）
			</div>
		</div>
		<?php if(isset($originalError) && count($originalError) > 0): ?>
		<div class="row">
			<div class="col-xs-12">
				<div class="alert alert-danger">
					<ul>
						<?php $__currentLoopData = $originalError; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $error): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
						<li><?php echo e($error); ?></li>
						<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
					</ul>
				</div>
			</div>
		</div>
		<?php endif; ?>
		<div class="row head-purple">
			<div class="col-xs-12">条件選択</div>
		</div>
		<div class="row ml-1">
			<div class="col-xs-12">
				<form action="<?php echo e(url('/')); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/newsave" method="POST" id="mainform">
					<?php echo csrf_field(); ?>
					<input type="hidden" id="kindid" name="cmn1" value="<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>" />
					<input type="hidden" id="menuid" name="cmn2" value="<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>" />
					<table class="table">
						<tbody>
							<tr>
								<td>検討ケース：</td>
								<td><input type="text" name="val1" value="<?php echo e(old('val1', $itemData['ProjectName'])); ?>" maxlength="50" autocomplete="off"></td>
								<td><?php echo $__env->make('layouts/error/item', ['name' => 'val1'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?></td>
							</tr>
						</tbody>
					</table>
				</form>
			</div>
		</div>
		<div class="row ml-1">
			<div class="col-xs-1 p-1">
				<button type="button" id="save" class="<?php echo e(config('system_const.btn_color_save')); ?>">
					<i class="<?php echo e(config('system_const.btn_img_save')); ?>"></i><?php echo e(config('system_const.btn_char_save')); ?>

				</button>
			</div>
			<div class="col-xs-1 p-1">
				<button type="button" id="cancel" class="<?php echo e(config('system_const.btn_color_cancel')); ?>">
					<i class="<?php echo e(config('system_const.btn_img_cancel')); ?>"></i><?php echo e(config('system_const.btn_char_cancel')); ?>

				</button>
			</div>
		</div>
    </div>
</div>
<?php $__env->stopSection(); ?>
<?php echo $__env->make('layouts/mainmenu/menu', \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/Schet/Case/create.blade.php ENDPATH**/ ?>