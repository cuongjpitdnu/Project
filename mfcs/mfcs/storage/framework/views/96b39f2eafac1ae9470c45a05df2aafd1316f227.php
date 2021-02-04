<?php $__env->startSection('content'); ?>
<script>
$(function(){
	$('[name="pageunit"]').change(function(e){
		$('#indicator').trigger('click');
		var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/';
		url += 'index?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
		url += '&page=1&pageunit=' + $(this).val();
		url += '&sort=<?php echo e(\Request::input('sort') ? \Request::input('sort') : ''); ?>';
		url += '&direction=<?php echo e(\Request::input('direction') ? \Request::input('direction') : 'asc'); ?>';
		window.location.href = url;
	});

	$('.show').on('click', function(){
		var val1 = $(this).val();
		$('#indicator').trigger('click');
		var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/show';
		url += '?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
		url += '&page=<?php echo e(isset($request->page) ? $request->page : 1); ?>';
		url += '&pageunit=<?php echo e(isset($request->pageunit) ? $request->pageunit : config("system_const.displayed_results_1")); ?>';
		url += '&sort=<?php echo e(isset($request->sort) ? $request->sort : ""); ?>';
		url += '&direction=<?php echo e(isset($request->direction) ? $request->direction : "asc"); ?>';
		url += '&val1=' + val1;
		window.location.href = url;
	});

	$('.create').on('click', function(){
		$('#indicator').trigger('click');
		var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/create';
		url += '?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
		url += '&page=<?php echo e(isset($request->page) ? $request->page : 1); ?>';
		url += '&pageunit=<?php echo e(isset($request->pageunit) ? $request->pageunit : config("system_const.displayed_results_1")); ?>';
		url += '&sort=<?php echo e(isset($request->sort) ? $request->sort : ""); ?>';
		url += '&direction=<?php echo e(isset($request->direction) ? $request->direction : "asc"); ?>';
		window.location.href = url;
	});

	$('.edit').on('click', function(){
		var val1 = $(this).val();
		$('#indicator').trigger('click');
		var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/edit';
		url += '?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
		url += '&page=<?php echo e(isset($request->page) ? $request->page : 1); ?>';
		url += '&pageunit=<?php echo e(isset($request->pageunit) ? $request->pageunit : config("system_const.displayed_results_1")); ?>';
		url += '&sort=<?php echo e(isset($request->sort) ? $request->sort : ""); ?>';
		url += '&direction=<?php echo e(isset($request->direction) ? $request->direction : "asc"); ?>';
		url += '&val1=' + val1;
		window.location.href = url;
	});
});

</script>

<div class="row ml-4">
	<div class="col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　棟マスタ
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

		<div class="row align-items-center">
			<?php if(!$menuInfo->IsReadOnly): ?>
			<div class="col-xs-1 m-3">
				<button type="button" class="create <?php echo e(config('system_const.btn_color_new')); ?>">
				<?php if(config('system_const.btn_img_new')!=''): ?><i class="<?php echo e(config('system_const.btn_img_new')); ?>"></i><?php endif; ?>
				<?php echo e(config('system_const.btn_char_new')); ?>

				</button>
			</div>
			<?php endif; ?>
		</div>

		<div class="row">
			<div class="col-sm-12">
				<table class="table">
					<thead>
						<tr>
							<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld1', 'コード'));?></th>
							<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld2', '名称'));?></th>
							<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld3', '略称'));?></th>
							<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld4', '表示順'));?></th>
							<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld5', '有効'));?></th>
							<th class="text-center"></th>
						</tr>
					</thead>
					<tbody>
					<?php $__currentLoopData = $floors; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $floor): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
					<tr>
						<td class="real-space align-middle"><?php echo e($floor['fld1']); ?></td>
						<td class="real-space align-middle"><?php echo e($floor['fld2']); ?></td>
						<td class="real-space align-middle"><?php echo e($floor['fld3']); ?></td>
						<td class="real-space align-middle text-right"><?php echo e($floor['fld4']); ?></td>
						<td class="align-middle"><?php echo e($floor['fld5'] ? '有効' : '無効'); ?></td>
						<td class="align-middle text-center">
							<?php if(!$menuInfo->IsReadOnly): ?>
								<button type="button" class="edit <?php echo e(config('system_const.btn_color_rowedit')); ?>"
								value="<?php echo e(valueUrlEncode($floor['fld1'])); ?>">
								<?php if(config('system_const.btn_img_rowedit')!=''): ?><i class="<?php echo e(config('system_const.btn_img_rowedit')); ?>"></i><?php endif; ?>
								<?php echo e(config('system_const.btn_char_rowedit')); ?>

								</button>
							<?php else: ?>
								<button type="button" class="show <?php echo e(config('system_const.btn_color_rowinfo')); ?>"
								value="<?php echo e(valueUrlEncode($floor['fld1'])); ?>">
								<?php if(config('system_const.btn_img_rowinfo')!=''): ?><i class="<?php echo e(config('system_const.btn_img_rowinfo')); ?>"></i><?php endif; ?>
								<?php echo e(config('system_const.btn_char_rowinfo')); ?>

								</button>
							<?php endif; ?>
						</td>
					</tr>
					<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
					</tbody>
				</table>
				<?php echo e($floors->appends(request()->query())->links()); ?>

				<div class="pageunit float-left">
					表示件数
					<select name="pageunit" class="pageunit-width">
						<option value= <?php echo e(config('system_const.displayed_results_1')); ?> <?php echo e((int)\Request::input('pageunit') === config('system_const.displayed_results_1') ? 'selected' : ''); ?>> <?php echo e(config('system_const.displayed_results_1')); ?> </option>
						<option value= <?php echo e(config('system_const.displayed_results_2')); ?> <?php echo e((int)\Request::input('pageunit') === config('system_const.displayed_results_2') ? 'selected' : ''); ?>> <?php echo e(config('system_const.displayed_results_2')); ?> </option>
						<option value= <?php echo e(config('system_const.displayed_results_3')); ?> <?php echo e((int)\Request::input('pageunit') === config('system_const.displayed_results_3') ? 'selected' : ''); ?>> <?php echo e(config('system_const.displayed_results_3')); ?> </option>
					</select>
				</div>
			</div>

		</div>

	</div>
</div>
<?php $__env->stopSection(); ?>

<?php echo $__env->make('layouts/mainmenu/menu', \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/Mst/Floor/index.blade.php ENDPATH**/ ?>