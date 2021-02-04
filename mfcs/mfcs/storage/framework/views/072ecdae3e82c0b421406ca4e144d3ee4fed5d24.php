<?php $__env->startSection('content'); ?>

<script>
$(function(){
	$('[data-toggle="tooltip"]').tooltip();

	$('#select').click(function(){
		$('#val3').click();
	});

	$('#val3').change(function (e) {
		if(e.target.files.length == 0) {
			$('#filename').val("");
			return;
		}
		var fileName = e.target.files[0].name;
		$('#filename').val(fileName);
	});

	$('#ok').on('click', function(){
		if($('#val3').val() != "" ) {
			const fileSize = ($('#val3').prop('files')[0].size / 1024).toFixed(2); 
			if (fileSize > <?php echo e(config('system_config.upload_file_size_max')); ?> ) { 
				alert( "<?php echo e(config('message.msg_file_003')); ?>"); 
				return;
			} 
		}
		$('#indicator').trigger('click');
		$('#mainform').submit();
	});
})

</script>
	<div class="row ml-2 mr-2">
		<div class="col-md-12 col-xs-12">
			<div class="row align-items-center">
				<div class="col-xs-1 text-left m-2 p-2 rounded border">
					■　日程表取込
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

			<form action="<?php echo e(url('/')); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/import" method="POST" id="mainform" enctype="multipart/form-data">
				<?php echo csrf_field(); ?>
				<input type="hidden" id="kindid" name="cmn1" value="<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>">
				<input type="hidden" id="menuid" name="cmn2" value="<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>">
				<div class="row head-purple">
					<div class="col-xs-12">条件選択</div>
				</div>
				<div class="row ml-1 mt-3">
					<div class="col-xs-12">
						<table class="table table-borderless">
							<tbody>
								<tr>
									<td class="td-mw-108 align-middle">検討ケース：</td>
									<td>
										<select name="val1" class="">
											<?php $__currentLoopData = $projects; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $project): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
											<option value="<?php echo e(valueUrlEncode($project->ID)); ?>" <?php echo e(trim(valueUrlDecode(old('val1', @$request->val1))) === trim($project->ID) ? 'selected' : ''); ?>>
												<?php echo e($project->ProjectName); ?>

											</option>
											<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
										</select>
									</td>
									<td class="p-0 align-middle">
										<span class="col-xs-1 p-1">
										<?php echo $__env->make('layouts/error/item', ['name' => 'val1'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
										</span>
									</td>
									<td class="td-mw-108 align-middle">オーダ：</td>
									<td>
										<select name="val2" class="">
											<?php $__currentLoopData = $orders; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $order): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
											<option value="<?php echo e(valueUrlEncode($order->OrderNo)); ?>" <?php echo e(trim(valueUrlDecode(old('val2',@$request->val2))) === trim($order->OrderNo) ? 'selected' : ''); ?>>
												<?php echo e($order->OrderNo); ?>

											</option>
											<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
										</select>
									</td>
									<td class="p-0 align-middle">
										<span class="col-xs-1 p-1">
										<?php echo $__env->make('layouts/error/item', ['name' => 'val2'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
										</span>
									</td>
								</tr>
							</tbody>
						</table>
					</div>
				</div>
				<div class="row head-purple">
					<div class="col-xs-12">ファイル選択</div>
				</div>
				<div class="row ml-1 mt-3">
					<div class="col-xs-12">
						<table class="table table-borderless">
							<tbody>
								<tr>
									<td class="align-middle">
										<input type="file" class="d-none" name="val3" id="val3" value="<?php echo e(@$request->val3); ?>" required="true">
										<input type="text" class="input-file-width" name="filename" id="filename" value= "<?php echo e(@$request->filename); ?>" autocomplete="off" readonly />
									</td>
									<td class="p-0 align-middle">
										<span class="col-xs-1 p-1">
										<?php echo $__env->make('layouts/error/item', ['name' => 'val3'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
										</span>
									</td>
									<td class="align-middle">
										<button type="button" name="select" id="select" class="<?php echo e(config('system_const.btn_color_file')); ?>">
											<i class="<?php echo e(config('system_const.btn_img_file')); ?>"></i>
											<?php echo e(config('system_const.btn_char_file')); ?>

										</button>
									</td>
									
								</tr>
							</tbody>
						</table>
					</div>
				</div>
				<div class="row head-purple">
					<div class="col-xs-12">ログ表示</div>
				</div>
				<div class="row ml-1 mt-3">
					<div class="col-xs-12">
						<table class="table table-borderless">
							<tbody>
								<tr>
									<td class="td-mw-108 align-middle">表示件数：</td>
									<td>
										<select name="val5" class="pageunit pageunit-width">
											<option value="<?php echo e(config('system_const.displayed_results_1')); ?>" <?php echo e((int)old('val5',valueUrlDecode($request->val5)) === config('system_const.displayed_results_1') ? 'selected' : ''); ?>>
												<?php echo e(config('system_const.displayed_results_1')); ?>

											</option>
											<option value="<?php echo e(config('system_const.displayed_results_2')); ?>" <?php echo e((int)old('val5',valueUrlDecode($request->val5)) === config('system_const.displayed_results_2') ? 'selected' : ''); ?>>
												<?php echo e(config('system_const.displayed_results_2')); ?>

											</option>
											<option value="<?php echo e(config('system_const.displayed_results_3')); ?>" <?php echo e((int)old('val5',valueUrlDecode($request->val5)) === config('system_const.displayed_results_3') ? 'selected' : ''); ?>>
												<?php echo e(config('system_const.displayed_results_3')); ?>

											</option>
										</select>
										※1ページあたり
									</td>
								</tr>
							</tbody>
						</table>
					</div>
				</div>
			</form>

			<div class="row">
				<div class="col-xs-1 p-1 m-3">
					<button type="button" id="ok" class="<?php echo e(config('system_const.btn_color_ok')); ?>"><i class="<?php echo e(config('system_const.btn_img_ok')); ?>"></i><?php echo e(config('system_const.btn_char_ok')); ?></button>
				</div>
			</div>
		</div>
	</div>
<?php $__env->stopSection(); ?>

<?php echo $__env->make('layouts/mainmenu/menu', \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/Schet/Import/index.blade.php ENDPATH**/ ?>