<?php $__env->startSection('content'); ?>

<form action="" method="POST" id="mainform">
	<?php echo csrf_field(); ?>
	<div class="row ml-4">
		<div class="col-xs-12">
			<div class="row align-items-center">
				<div class="col-xs-1 text-left m-2 p-2 rounded border">
					■　物量定義 - 詳細
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
					<div class="info-circle"><i class="fas fa-info icon-small"></i></div> 取り込めなかった物量は次の通りです
					<table class="table">
						<tbody>
							<tr class="set-color">
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld1', '搭載名'));?></th>
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld2', '搭載組区'));?></th>
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld3', '中日程名'));?></th>
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld4', '中日程組区'));?></th>
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld5', '工程'));?></th>
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld6', '工程組区'));?></th>
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld7', '内容'));?></th>
							</tr>

							<?php $__currentLoopData = $rows; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $row): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
							<tr>
								<td class="real-space align-middle"><?php echo e($row['fld1']); ?></td>
								<td class="real-space align-middle"><?php echo e($row['fld2']); ?></td>
								<td class="real-space align-middle"><?php echo e($row['fld3']); ?></td>
								<td class="real-space align-middle"><?php echo e($row['fld4']); ?></td>
								<td class="real-space align-middle"><?php echo e($row['fld5']); ?></td>
								<td class="real-space align-middle"><?php echo e($row['fld6']); ?></td>
								<td class="real-space align-middle"><?php echo e($row['fld7']); ?></td>
							</tr>
							<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
						</tbody>
					</table>   
				</div>
			</div>
			<div class="row">
				<?php echo e($rows->appends(request()->query())->links()); ?>

			</div>
		</div>
	</div>
</form>
<?php $__env->stopSection(); ?>
<?php echo $__env->make('layouts/mainmenu/menu', \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/schem/bdata/result.blade.php ENDPATH**/ ?>