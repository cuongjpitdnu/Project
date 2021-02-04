<?php $__env->startSection('content'); ?>
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();
	});
</script>
<style>
	.td-mw-160 {
		min-width: 160px !important;
	}
	.select-h-100 {
		height: 90px !important;
	}
	.select-h-204 {
		height: 204px !important;
	}
</style>
<div class="row ml-2 mr-2">
	<div class="col-md-12 col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　汎用集計表
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
		<form action="<?php echo e(url('/')); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/settings" method="POST" id="mainform" enctype="multipart/form-data">
			<?php echo csrf_field(); ?>
			<input type="hidden" id="kindid" name="cmn1" value="<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>" />
			<input type="hidden" id="menuid" name="cmn2" value="<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>" />
			
			<div class="row head-purple">
				<div class="col-xs-12">項目選択</div>
			</div>
			<div class="row ml-1">
				<div class="col-xs-12">
					<table class="table table-borderless mb-0">
						<tr>
							<td class="td-mw-160">出力項目選択：</td>
							<td class="align-middle">
								<select class="form-select select-h-100 val101" multiple name="val101">
									<option value="1">Val 1</option>
									<option value="2">Val 2</option>
									<option value="3">Val 3</option>
									<option value="4">Val 4</option>
								</select>
							</td>
							<td class="p-0 align-middle">
								<span class="col-xs-1 p-1">
								<?php echo $__env->make('layouts/error/item', ['name' => 'val101'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
								</span>
							</td>
						</tr>
					</table>
				</div>
			</div>

			<div class="row head-purple">
				<div class="col-xs-12">並び順選択</div>
			</div>
			<div class="row ml-1">
				<div class="col-xs-12">
					<table class="table table-borderless mb-0">
						<tr>
							<td rowspan="2" class="td-mw-160">集約キー項目選択：</td>
							<td rowspan="2" class="align-middle">
								<select class="form-select select-h-100 val101" multiple name="val101">
								</select>
							</td>
							<td class="align-middle pb-0">
								<button type="button" class="btn btn-outline-primary btn-sm">←</button>
							</td>
							<td rowspan="5" class="align-middle">
								<select class="form-select select-h-204 val101" multiple name="val101">
								</select>
							</td>
						</tr>
						<tr>
							<td class="align-middle pt-0">
								<button type="button" class="btn btn-outline-primary btn-sm">→</button>
							</td>
						</tr>
						<tr>
						</tr>
						<tr>
							<td rowspan="2" class="td-mw-160">集約キー項目選択：</td>
							<td rowspan="2" class="align-middle">
								<select class="form-select select-h-100 val101" multiple name="val101">
								</select>
							</td>
							<td class="align-middle pb-0">
								<button type="button" class="btn btn-outline-primary btn-sm">←</button>
							</td>
						</tr>
						<tr>
							<td class="align-middle pt-0">
								<button type="button" class="btn btn-outline-primary btn-sm">→</button>
							</td>
						</tr>

						
					</table>
				</div>
			</div>

			<div class="row head-purple">
				<div class="col-xs-12">小計・合計項目選択エリア</div>
			</div>
			<div class="row ml-1">
				<div class="col-xs-12">
					<table class="table table-borderless mb-0">
						<tr>
							<td class="td-mw-160">条件項目選択：</td>
							<td>
								<select class="form-select select-h-100 val101" multiple name="val101">

								</select>
							</td>
							<td class="p-0 align-middle">
								<span class="col-xs-1 p-1">
								<?php echo $__env->make('layouts/error/item', ['name' => 'val101'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
								</span>
							</td>
						</tr>
					</table>
				</div>
			</div>

			<div class="row head-purple">
				<div class="col-xs-12">条件選択エリア</div>
			</div>
			<div class="row ml-1">
				<div class="col-xs-12">
					<table class="table table-borderless mb-0">
						<tr>
							<td class="td-mw-160">条件項目選択：</td>
							<td>
								<select class="form-select select-h-100 val101" multiple name="val101">

								</select>
							</td>
							<td class="p-0 align-middle">
								<span class="col-xs-1 p-1">
								<?php echo $__env->make('layouts/error/item', ['name' => 'val101'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
								</span>
							</td>
						</tr>
					</table>
				</div>
			</div>

			
		</form>
		<div class="row">
			<div class="col-xs-1 p-1">
				<button type="button" id="save" class="<?php echo e(config('system_const.btn_color_ok')); ?>">
					<i class="<?php echo e(config('system_const.btn_img_ok')); ?>"></i><?php echo e(config('system_const.btn_char_ok')); ?>

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
<?php echo $__env->make('layouts/mainmenu/menu', \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/Report/summary/settings.blade.php ENDPATH**/ ?>