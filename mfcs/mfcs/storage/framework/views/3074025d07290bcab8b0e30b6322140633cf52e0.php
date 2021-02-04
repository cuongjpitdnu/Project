<?php $__env->startSection('content'); ?>
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();

		const val1Old = sessionStorage.getItem('val1') || '';
		const val2Old = sessionStorage.getItem('val2') || '';
		if(val1Old != '' && val1Old !== 'null') { bindingVal2(val1Old, val2Old); }

		$('#save').on('click', function(e) {
			$('#indicator').trigger('click');
			let val1Selected = ($('[name=val1]').val() == '<?php echo e(config('system_const.projectid_production')); ?>') ? '' : $('[name=val1]').val();
			let val3Selected = ($('[name=val3]').val() == '<?php echo e(config('system_const.projectid_production')); ?>') ? '' : $('[name=val3]').val();
			$('[name=val1]').val(val1Selected);
			$('[name=val3]').val(val3Selected);
			sessionStorage.setItem('val1', $('[name=val1]').val());
			sessionStorage.setItem('val2', $('[name=val2]').val());
			$('#mainform').submit();
		});

		$('#cancel').on('click', function(e) {
			$('#indicator').trigger('click');
			var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/';
			url += 'index?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
			window.location.href = url;
		});

		$('[name=val1]').on('change', function(e) {
			bindingVal2($(this).val(), '');
		});
	});

	function bindingVal2(value, selectedValue) {
		$.ajax({
			type: 'POST',
			url: '<?php echo e(url("/")); ?>/schet/case/getval2',
			dataType: 'json',
			headers: {'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')},
			contentType: 'application/json',
			data: JSON.stringify({'val1': value}),
			async : false,
		}).done(function (response) {
			$('[name=val2]').empty().append('<option value=""></option>');
			if(response != []) {
				if(response.length > 0) {
					$.each(response, function(i, e) {
						let seleted = (selectedValue == e.val2) ? 'selected' : '';
						$('[name=val2]').append(`<option value="${e.val2}" ${seleted}>${e.val2}</option>`);
					});
				}
			}
		}).fail(function(xhr, status, error) {
			window.location.href = '<?php echo e(url("/")); ?>/errors/500';
		}).always(function() {
			sessionStorage.clear();
		});
	}
</script>

<style>
	.wd-169 { width: 169px; }
	select { height: 27px; }
    table { margin-top: 16px; }
    table tr td { text-align: left; vertical-align: middle !important; border: none !important; }
</style>

<div class="row ml-2 mr-2">
	<div class="col-md-12 col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■検討ケース作成（コピー）
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
		<form action="<?php echo e(url('/')); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/copysave" method="POST" id="mainform">
			<?php echo csrf_field(); ?>
			<input type="hidden" id="kindid" name="cmn1" value="<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>" />
			<input type="hidden" id="menuid" name="cmn2" value="<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>" />
			<div class="row head-purple">
				<div class="col-xs-12">条件選択</div>
			</div>

			<div class="row ml-1">
				<div class="col-xs-12">
					<table class="table table-borderless">
						<tbody>
							<tr>
								<td>コピー元</td>
								<td>検討ケース：</td>
								<td>
									<select name="val1" id="" class="wd-169">
										<option value="<?php echo e(config('system_const.projectid_production')); ?>"></option>
										<?php $__currentLoopData = $dataView['data_1']; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $item): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
											<option value=<?php echo e($item->ID); ?>

												<?php echo e(trim(old('val1', @$itemShow['val1'])) == trim($item->ID) ? 'selected': ''); ?>><?php echo e($item->ProjectName); ?></option>
										<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
									</select>
									<?php echo $__env->make('layouts/error/item', ['name' => 'val1'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
								</td>
								<td>オーダ：</td>
								<td>
									<select name="val2" id="" class="wd-169">
										<option value=""></option>
										<?php $__currentLoopData = $dataView['data_3']; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $item): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
											<option value=<?php echo e($item->val2); ?>

												<?php echo e(trim(old('val2', @$itemShow['val2'])) == trim($item->val2) ? 'selected': ''); ?>><?php echo e($item->val2); ?></option>
										<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
									</select>
									<?php echo $__env->make('layouts/error/item', ['name' => 'val2'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
								</td>
							</tr>
							<tr>
								<td>コピー先</td>
								<td>検討ケース：</td>
								<td>
									<select name="val3" id="" class="wd-169">
										<option value="<?php echo e(config('system_const.projectid_production')); ?>"></option>
										<?php $__currentLoopData = $dataView['data_1']; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $item): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
											<option value=<?php echo e($item->ID); ?>

												<?php echo e(trim(old('val3', @$itemShow['val3'])) == trim($item->ID) ? 'selected': ''); ?>><?php echo e($item->ProjectName); ?></option>
										<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
									</select>
									<?php echo $__env->make('layouts/error/item', ['name' => 'val3'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
								</td>
								<td>オーダ：</td>
								<td>
									<select name="val4" id="" class="wd-169">
										<option value=""></option>
										<?php $__currentLoopData = $dataView['data_2']; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $item): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
											<option value=<?php echo e($item->OrderNo); ?>

												<?php echo e(trim(old('val4', @$itemShow['val4'])) == trim($item->OrderNo) ? 'selected': ''); ?>><?php echo e($item->OrderNo); ?></option>
										<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
									</select>
									<?php echo $__env->make('layouts/error/item', ['name' => 'val4'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
								</td>
							</tr>
						</tbody>
					</table>
				</div>
			</div>
			<div class="row head-purple">
				<div class="col-xs-12">手番シフト</div>
			</div>
			<div class="row ml-1">
				<div class="col-xs-12">
					<table class="table table-borderless">
						<tbody>
							<tr>
								<td>シフトする手番を入力：</td>
								<td>
									<input type="text" class="wd-169" name="val5" value="<?php echo e(old('val5', @$itemShow['val5'])); ?>" maxlength="6" />
									<?php echo $__env->make('layouts/error/item', ['name' => 'val5'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
								</td>
							</tr>
						</tbody>
					</table>
				</div>
			</div>
		</form>
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
<?php echo $__env->make('layouts/mainmenu/menu', \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/Schet/Case/copy.blade.php ENDPATH**/ ?>