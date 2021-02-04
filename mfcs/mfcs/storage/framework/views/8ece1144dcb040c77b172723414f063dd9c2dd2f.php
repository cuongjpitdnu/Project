<script>
$(function(){
	$('[data-toggle="tooltip"]').tooltip();

	$('#save').on('click', function(){
		$('#indicator').trigger('click');
		$('#mainform').submit();
	});

	$('#cancel').on('click', function(){
		$('#indicator').trigger('click');
		var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/';
		url += 'index?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
		url += '&page=<?php echo e($request->page); ?>';
		url += '&pageunit=<?php echo e($request->pageunit); ?>';
		url += '&sort=<?php echo e($request->sort); ?>';
		url += '&direction=<?php echo e($request->direction); ?>';
		window.location.href = url;
	});

	$('.selectdate').datepicker();

	$('.input-checkbox').click(function(){
		if($(this).prop('checked')){
			$('[name="'+$(this).attr('checkbox')+'"]').val(1);
		}else{
			$('[name="'+$(this).attr('checkbox')+'"]').val(0);
		}
	});
})

</script>

<div class="row ml-4">
	<div class="col-xs-12">

		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　物量マスタ<?php if($target === 'show'): ?>参照<?php elseif($target === 'create'): ?>登録<?php elseif($target === 'edit'): ?>更新<?php endif; ?>
			</div>
		</div>
		<?php if($errors->count() === 0): ?>
		<?php if(isset($originalError) && count($originalError)): ?>
		<div class="row">
			<div class="col-xs-12">
				<div class="alert alert-danger">
					<ul>
						<?php $__currentLoopData = $originalError; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $item): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
						<li><?php echo e($item); ?></li>
						<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
					</ul>
				</div>
			</div>
		</div>
		<?php endif; ?>
		<?php endif; ?>
		<form action="<?php echo e(url('/')); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/save" method="POST" id="mainform">
			<?php echo csrf_field(); ?>
			<input type="hidden" id="kindid" name="cmn1" value="<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>">
			<input type="hidden" id="menuid" name="cmn2" value="<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>">
			<input type="hidden" name="page" value="<?php echo e($request->page); ?>">
			<input type="hidden" name="pageunit" value="<?php echo e($request->pageunit); ?>">
			<input type="hidden" name="sort" value="<?php echo e($request->sort); ?>">
			<input type="hidden" name="direction" value="<?php echo e($request->direction); ?>">
			<?php if($target === 'edit'): ?>
			<input type="hidden" name="val5" value="<?php echo e(old('val5', @$bData['Updated_at'])); ?>">
			<?php endif; ?>
			<input type="hidden" name="method" value="<?php echo e($target); ?>">

			<table class="table table-borderless">
				<tbody>
				<tr>
					<td class="align-middle">コード *：</td>
					<td>
						<input type="text" name="val1" value="<?php echo e(old('val1', @$bData['Code'])); ?>" 
						<?php echo e($target === "show" || $target === "edit" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="5">
						<?php if($target === "edit"): ?>
						<input type="hidden" name="val1" value="<?php echo e(old('val1', @$bData['Code'])); ?>">
						<?php endif; ?>
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val1'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?> 
						</span>
					</td>
				</tr>
				<tr>
					<td class="align-middle">名称：</td>
					<td>
						<input type="text" name="val2" value="<?php echo e(old('val2', @$bData['Name'])); ?>" 
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="50">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val2'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?> 
						</span>
					</td>
				</tr>
				<tr>
					<td class="align-middle">略称：</td>
					<td>
						<input type="text" name="val3" value="<?php echo e(old('val3', @$bData['Nick'])); ?>" 
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="50">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val3'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?> 
						</span>
					</td>
				</tr>
				<tr>
					<td class="align-middle">有効：</td>
					<td>
						<input type="checkbox" class="input-checkbox" checkbox="val4" 
						<?php echo e(($target === "create" && empty(old('val4', @$bData['ViewFlag']))) 
						|| (int)old('val4', @$bData['ViewFlag']) === 1 ? 'checked' : ''); ?> <?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?>> 有効
						<input type="hidden" name="val4" value="<?php echo e(($target === "create" && empty(old('val4', @$bData['ViewFlag']))) 
						|| old('val4', @$bData['ViewFlag']) ? 1 : 0); ?>">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val4'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?> 
						</span>
					</td>
				</tr>
				</tbody>
			</table>
		</form>

		<div class="row">
			<?php if($target === 'create' || $target === 'edit'): ?>
			<div class="col-xs-1 p-1">
				<button type="button" id="save" class="<?php echo e(config('system_const.btn_color_save')); ?>">
					<i class="<?php echo e(config('system_const.btn_img_save')); ?>"></i><?php echo e(config('system_const.btn_char_save')); ?>

				</button>
			</div>
			<?php endif; ?>
			<div class="col-xs-1 p-1">
				<button type="button" id="cancel" class="<?php echo e(config('system_const.btn_color_cancel')); ?>">
					<i class="<?php echo e(config('system_const.btn_img_cancel')); ?>"></i><?php echo e(config('system_const.btn_char_cancel')); ?>

				</button>
			</div>
		</div>

	</div>
</div><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/mst/bdata/contents.blade.php ENDPATH**/ ?>