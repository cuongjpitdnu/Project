<?php $__env->startSection('content'); ?>

<script>

$(function(){
	$('[name="pageunit"]').change(function(e){
		$('#indicator').trigger('click');
		var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>';
		url += '?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>';
		url += '&page=1&pageunit=' + $(this).val();
		url += '&sort=<?php echo e($request->sort); ?>';
		url += '&direction=<?php echo e($request->direction); ?>';
		window.location.href = url;
	});

	$('.show').on('click', function(){
		var val1 = $(this).val();
		$('#indicator').trigger('click');
		var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/index';
		url += '?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>';
		url += '&val1=' + val1;
		url += '&val2=<?php echo e(isset($request->page) ? $request->page : 1); ?>';
		url += '&val3=<?php echo e(isset($request->pageunit) ? $request->pageunit : config('system_const.displayed_results_1')); ?>';
		url += '&val4=<?php echo e($request->sort); ?>';
		url += '&val5=<?php echo e($request->direction); ?>';
		window.location.href = url;
	});

})

</script>

<div class="row align-items-center">
	<div class="col-xs-1 text-left m-2 p-2 rounded border">
		■　お知らせ　■
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

<div class="row pl-2">
	<div class="col-xs-12">
		<?php if(count($listDatas) == 0): ?>
			表示するお知らせが有りません。
		<?php else: ?>
			<table class="table text-center">
				<tr>
					<th><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld2', '取込日'));?></th>
					<th><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld4', '検討ケース'));?></th>
					<th><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld5', 'オーダ'));?></th>
					<th><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld6', '他日程との連携'));?></th>
					<th><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld7', '内容'));?></th>
					<th class="align-center"></th>
				</tr>
				<?php $__currentLoopData = $listDatas; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $listData): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
				<tr>
					<td><?php echo e(date('Y/m/d', strtotime($listData->fld2))); ?></td>
					<td class="text-left"><?php echo e($listData->fld4); ?></td>
					<td class="text-left"><?php echo e($listData->fld5); ?></td>
					<?php switch($listData->fld6):
						case (1): ?>
							<td class="text-left"><?php echo e('連携'); ?></td>
							<?php break; ?>
						<?php default: ?>
							<td class="text-left"></td>
					<?php endswitch; ?>
					<?php switch($listData->fld7):
						case (config('system_const_schet.schet_import_status_error')): ?>
							<td class="text-left"><?php echo e(config('message.msg_notice_schet_003')); ?></td>
							<?php break; ?>
						<?php case (config('system_const_schet.schet_import_status_running')): ?>
							<td class="text-left"><?php echo e(config('message.msg_notice_schet_002')); ?></td>
							<?php break; ?>
						<?php case (config('system_const_schet.schet_import_status_done')): ?>
							<td class="text-left"><?php echo e(config('message.msg_notice_schet_001')); ?></td>
							<?php break; ?>
						<?php default: ?>
							<td class="text-left"></td>
					<?php endswitch; ?>
					<td class="align-center">
						<button type="button" class="show <?php echo e(config('system_const.btn_color_rowdetail')); ?>" 
							value="<?php echo e(valueUrlEncode($listData->fld1)); ?>">
							<?php if(config('system_const.btn_img_rowdetail')!=''): ?>
							<i class="<?php echo e(config('system_const.btn_img_rowdetail')); ?>"></i>
							<?php endif; ?>
							<?php echo e(config('system_const.btn_char_rowdetail')); ?>

						</button>
					</td>
				</tr>
				<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
			</table>
			<?php echo e($listDatas->appends(request()->query())->links()); ?>

			
			<tr>
				<td>表示件数：</td>
				<td>
					<select name="pageunit" class="pageunit-width">
						<option value= <?php echo e(config('system_const.displayed_results_1')); ?> <?php echo e(\Request::input('pageunit') == config('system_const.displayed_results_1') ? 'selected' : ''); ?>> <?php echo e(config('system_const.displayed_results_1')); ?> </option>
						<option value= <?php echo e(config('system_const.displayed_results_2')); ?> <?php echo e(\Request::input('pageunit') == config('system_const.displayed_results_2') ? 'selected' : ''); ?>> <?php echo e(config('system_const.displayed_results_2')); ?> </option>
						<option value= <?php echo e(config('system_const.displayed_results_3')); ?> <?php echo e(\Request::input('pageunit') == config('system_const.displayed_results_3') ? 'selected' : ''); ?>> <?php echo e(config('system_const.displayed_results_3')); ?> </option>
					</select>
				</td>
			</tr>

		<?php endif; ?>
	</div>
</div>

<?php $__env->stopSection(); ?>

<?php echo $__env->make('layouts/mainmenu/menu', \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/layouts/submenu/schet.blade.php ENDPATH**/ ?>