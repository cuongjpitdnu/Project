<?php $__env->startSection('content'); ?>

<div class="row align-items-center">
	<div class="col-xs-1 text-left m-2 p-2 rounded border">
		■　お知らせ　■
	</div>
</div>

<?php if(session('menu_index_message')): ?>
	<div class="row">
		<div class="col-xs-12">
			<div class="alert alert-danger">
				<ul>
					<li><?php echo e(session('menu_index_message')); ?></li>
				</ul>
			</div>
		</div>
	</div>
<?php endif; ?>

<div class="row pl-2">
	<div class="col-xs-12">
		<?php if(count($informations) == 0): ?>
			表示するお知らせが有りません。
		<?php else: ?>
		<table class="table text-center">
			<tr>
				<th><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld1', '更新日'));?></th>
				<th><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld2', '内容'));?></th>
			</tr>
			<?php $__currentLoopData = $informations; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $information): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
			<tr>
				<td><?php echo e(date('Y/m/d', strtotime($information['fld1']))); ?></td>
				<td class="text-left"><?php echo e(unEscapedLine($information['fld2'])); ?></td>
			</tr>
			<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
		</table>
		<?php echo e($informations->appends(request()->query())->links()); ?>

		<?php endif; ?>
	</div>
</div>

<?php $__env->stopSection(); ?>

<?php echo $__env->make('layouts/mainmenu/menu', \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/layouts/mainmenu/index.blade.php ENDPATH**/ ?>