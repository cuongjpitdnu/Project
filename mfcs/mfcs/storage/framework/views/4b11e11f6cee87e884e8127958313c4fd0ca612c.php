<?php $__env->startSection('content'); ?>
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();
		$('#save').on('click', function(){
			$('#indicator').trigger('click');
			$('#mainform').submit();
		});
		$('#cancel').on('click', function(){
			$('#indicator').trigger('click');
			var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/';
			url += 'index?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
			url += '&val1=<?php echo e($request->val1); ?>';
			url += '&val2=<?php echo e($request->val2); ?>';
			url += '&val5=<?php echo e($request->val5); ?>';
			window.location.href = url;
		});
	});
</script>

<form action="<?php echo e(url('/')); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/save" method="POST" id="mainform">
	<?php echo csrf_field(); ?>
	<input type="hidden" id="kindid" name="cmn1" value="<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>">
	<input type="hidden" id="menuid" name="cmn2" value="<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>">
	<input type="hidden" name="page" value="<?php echo e($request->page); ?>" />
	<input type="hidden" name="sort" value="<?php echo e($request->sort); ?>" />
	<input type="hidden" name="direction" value="<?php echo e($request->direction); ?>" />
	<input type="hidden" id="" name="val1" value="<?php echo e($request->val1); ?>">
	<input type="hidden" id="" name="val2" value="<?php echo e($request->val2); ?>">
	<input type="hidden" id="" name="val5" value="<?php echo e($request->val5); ?>">
	<input type="hidden" id="" name="val6" value="<?php echo e($request->val6); ?>">
	<div class="row ml-4">
		<div class="col-xs-12">
			<div class="row align-items-center">
				<div class="col-xs-1 text-left m-2 p-2 rounded border">
					■　日程表取込登録
				</div>
			</div>
			
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

			<div class="row">
				<div class="col-sm-12">
					<div class="info-circle"><i class="fas fa-info icon-small"></i></div><?php echo e(count($rows) === 0 ? ' 現在の日程表との相違点はありませんでした。' : ' 現在の日程表との相違点です。よろしければ、保存ボタンを押してください。'); ?> 
					<table class="table table-row table-import">
						<tbody>
							<tr class="set-color">
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld1', '区分'));?></th>
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld2', 'ブロック名'));?></th>
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld3', '組区'));?></th>
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld4', '内容'));?></th>
							</tr>

							<?php $__currentLoopData = $rows; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $row): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
							<tr>
								<td class="real-space align-middle"><?php echo e($row['fld1']); ?></td>
								<td class="real-space align-middle"><?php echo e($row['fld2']); ?></td>
								<td class="real-space align-middle text-right"><?php echo e($row['fld3']); ?></td>
								<td class="real-space align-middle"><?php echo e($row['fld4']); ?></td>
							</tr>
							<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
						</tbody>
					</table>   
				</div>
			</div>
			<div class="row">
				<?php if($datatype == 'ok'): ?>
				<?php echo e($rows->appends(request()->query())->links()); ?>

				<?php endif; ?>
			</div>
			<div class="row">
				<?php if($datatype == 'ok' && count($rows) > 0): ?>
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
	</div>
</form>
<?php $__env->stopSection(); ?>
<?php echo $__env->make('layouts/mainmenu/menu', \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/Schet/Import/create.blade.php ENDPATH**/ ?>