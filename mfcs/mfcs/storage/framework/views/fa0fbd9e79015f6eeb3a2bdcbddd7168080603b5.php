<?php $__env->startSection('content'); ?>

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
	<p><img src="<?php echo e(url('/img/err001.png')); ?>"></p>
</div>

<button type="button" class="toppage <?php echo e(config('system_const.btn_color_toppage')); ?>" onclick="location.href='<?php echo e(url('index')); ?>'">
	<?php if(config('system_const.btn_img_toppage')!=''): ?>
	<i class="<?php echo e(config('system_const.btn_img_toppage')); ?>"></i>
	<?php endif; ?>
	<?php echo e(config('system_const.btn_char_toppage')); ?>

</button>

<?php $__env->stopSection(); ?>

<?php echo $__env->make('errors/errmenu', \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/errors/404.blade.php ENDPATH**/ ?>