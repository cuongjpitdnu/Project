<?php $__env->startSection('content'); ?>

<script>

$(function(){
	$('[name="pageunit"]').change(function(e){
		$('#indicator').trigger('click');
		var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/index';
		url += '?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>';
		url += '&page=1&pageunit=' + $(this).val();
		url += '&sort=<?php echo e($request->sort); ?>';
		url += '&direction=<?php echo e($request->direction); ?>';
		url += '&val1=<?php echo e($request->val1); ?>';
		url += '&val2=<?php echo e($request->val2); ?>';
		url += '&val3=<?php echo e($request->val3); ?>';
		url += '&val4=<?php echo e($request->val4); ?>';
		url += '&val5=<?php echo e($request->val5); ?>';
		window.location.href = url;
	});

	$('.back').on('click', function(){
		$('#indicator').trigger('click');
		var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>';
		url += '?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>';
		url += '&page=<?php echo e(isset($request->val2) ? $request->val2 : 1); ?>';
		url += '&pageunit=<?php echo e(isset($request->val3) ? $request->val3 : config('system_const.displayed_results_1')); ?>';
		url += '&sort=<?php echo e($request->val4); ?>';
		url += '&direction=<?php echo e($request->val5); ?>';
		window.location.href = url;
	});

})

</script>

<style>
	nav{
		float: left;
		margin-right: 30px;
	}
	.pageunit{
		line-height: 35px;
	}
</style>

<div class="row align-items-center">
	<div class="col-xs-1 text-left m-2 p-2 rounded border">
		■　搭載日程取込ログ　■
	</div>
</div>

<div class="row pl-2">
	<div class="col-xs-12">
		<div class="col-xs-1 m-1">
			<button type="button" class="back <?php echo e(config('system_const.btn_color_back')); ?>" 
				value="">
				<?php if(config('system_const.btn_img_back')!=''): ?>
				<i class="<?php echo e(config('system_const.btn_img_back')); ?>"></i>
				<?php endif; ?>
				<?php echo e(config('system_const.btn_char_back')); ?>

			</button>
		</div>
		<?php if(count($listDatas) == 0): ?>
			表示するログ情報が有りません。
		<?php else: ?>
			<table class="table text-center">
				<tr>
					<th><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld1', 'カテゴリ'));?></th>
					<th><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld2', 'ブロック名'));?></th>
					<th><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld3', '組区'));?></th>
					<th><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld4', 'ログ内容'));?></th>
				</tr>
				<?php $__currentLoopData = $listDatas; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $listData): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
				<tr>
					<td class="text-left"><?php echo e($listData->fld1); ?></td>
					<td class="text-left"><?php echo e($listData->fld2); ?></td>
					<td class="text-left"><?php echo e($listData->fld3); ?></td>
					<td class="text-left"><?php echo e($listData->fld4); ?></td>
				</tr>
				<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
			</table>
			<?php echo e($listDatas->appends(request()->query())->links()); ?>

			<div class="pageunit" style="float:left;">
				表示件数
				<select name="pageunit">
					<option value= <?php echo e(config('system_const.displayed_results_1')); ?> <?php echo e(\Request::input('pageunit') == config('system_const.displayed_results_1') ? 'selected' : ''); ?>> <?php echo e(config('system_const.displayed_results_1')); ?> </option>
					<option value= <?php echo e(config('system_const.displayed_results_2')); ?> <?php echo e(\Request::input('pageunit') == config('system_const.displayed_results_2') ? 'selected' : ''); ?>> <?php echo e(config('system_const.displayed_results_2')); ?> </option>
					<option value= <?php echo e(config('system_const.displayed_results_3')); ?> <?php echo e(\Request::input('pageunit') == config('system_const.displayed_results_3') ? 'selected' : ''); ?>> <?php echo e(config('system_const.displayed_results_3')); ?> </option>
				</select>
			</div>
		<?php endif; ?>
	</div>
</div>

<?php $__env->stopSection(); ?>

<?php echo $__env->make('layouts/mainmenu/menu', \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/Schet/index.blade.php ENDPATH**/ ?>