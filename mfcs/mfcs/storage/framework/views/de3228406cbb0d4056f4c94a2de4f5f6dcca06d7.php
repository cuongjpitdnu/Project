<?php $__env->startSection('content'); ?>
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();

		$('#ok').on('click', function(e) {
			$('#indicator').trigger('click');
			let val1 = $('[name=val1]:checked').val();
			let val2 = $('[name=val2]').val();
			var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/manage';
			url += '?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
			url += '&val1='+val1;
			url += '&val2='+val2;

			window.location.href = url;
		});
	});
</script>

<style>
	table { margin-top: 16px; margin-left: 15px; }
	table tr td { text-align: left; vertical-align: middle !important; border: none !important; }
	table tr td label { margin-bottom: 0px; cursor: pointer; }
	table tr td label input[type=radio] { cursor: pointer; }
</style>

<div class="row ml-4">
	<div class="col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　項目定義
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

		<div class="row">
			<div class="col-xs-12">
				<table class="table table-borderless">
					<tbody>
						<tr>
							<td>中日程区分：</td>
							<td>
								<label for="rdo1"><input type="radio" id="rdo1" name="val1" value="<?php echo e(valueUrlEncode(config('system_const.c_kind_chijyo'))); ?>"
									<?php echo e((int)old('val1', @$itemShow['val1']) === config('system_const.c_kind_chijyo') ? 'checked' : ''); ?>> <?php echo e(config('system_const.c_name_chijyo')); ?></label> /
								<label for="rdo2"><input type="radio" id="rdo2" name="val1" value="<?php echo e(valueUrlEncode(config('system_const.c_kind_gaigyo'))); ?>"
									<?php echo e((int)old('val1', @$itemShow['val1']) === config('system_const.c_kind_gaigyo') ? 'checked' : ''); ?>> <?php echo e(config('system_const.c_name_gaigyo')); ?></label> /
								<label for="rdo3"><input type="radio" id="rdo3" name="val1" value="<?php echo e(valueUrlEncode(config('system_const.c_kind_giso'))); ?>"
									<?php echo e((int)old('val1', @$itemShow['val1']) === config('system_const.c_kind_giso') ? 'checked' : ''); ?>> <?php echo e(config('system_const.c_name_giso')); ?></label>
								<?php echo $__env->make('layouts/error/item', ['name' => 'val1'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
							</td>
						</tr>
						<tr>
							<td>表示件数：</td>
							<td>
								<select name="val2" class="pageunit-width">
									<option value="<?php echo e(valueUrlEncode(config('system_const.displayed_results_1'))); ?>"
										<?php echo e((int)old('val2', @$itemShow['val2']) === config('system_const.displayed_results_1') ? 'selected' : ''); ?>><?php echo e(config('system_const.displayed_results_1')); ?></option>
									<option value="<?php echo e(valueUrlEncode(config('system_const.displayed_results_2'))); ?>"
										<?php echo e((int)old('val2', @$itemShow['val2']) === config('system_const.displayed_results_2') ? 'selected' : ''); ?>><?php echo e(config('system_const.displayed_results_2')); ?></option>
									<option value="<?php echo e(valueUrlEncode(config('system_const.displayed_results_3'))); ?>"
										<?php echo e((int)old('val2', @$itemShow['val2']) === config('system_const.displayed_results_3') ? 'selected' : ''); ?>><?php echo e(config('system_const.displayed_results_3')); ?></option>
								</select> ※1ページあたり
								<?php echo $__env->make('layouts/error/item', ['name' => 'val2'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
							</td>
						</tr>
					</tbody>
				</table>
			</div>
		</div>

		<div class="row">
			<div class="col-xs-1 p-1">
				<button type="button" id="ok" class="<?php echo e(config('system_const.btn_color_ok')); ?>">
					<i class="<?php echo e(config('system_const.btn_img_ok')); ?>"></i><?php echo e(config('system_const.btn_char_ok')); ?></button>
			</div>
		</div>
	</div>
</div>
<?php $__env->stopSection(); ?>
<?php echo $__env->make('layouts/mainmenu/menu', \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/Schem/Item/index.blade.php ENDPATH**/ ?>