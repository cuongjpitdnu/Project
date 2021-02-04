<?php $__env->startSection('content'); ?>
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();
		$('#save').on('click', function(e) {
			var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/accept';
			$('#mainform').attr('action', url);
			$('#indicator').trigger('click');
			$('#mainform').submit();
		});
		$('#cancel').on('click', function(e) {
			var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/cancel';
			$('#mainform').attr('action', url);
			$('#indicator').trigger('click');
			$('#mainform').submit();
		});
	});
</script>

<form action="" method="POST" id="mainform">
	<?php echo csrf_field(); ?>
	<input type="hidden" id="kindid" name="cmn1" value="<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>" />
	<input type="hidden" id="menuid" name="cmn2" value="<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>" />
	<input type="hidden" id="" name="val1" value="<?php echo e($request->val1); ?>" />
	<input type="hidden" id="" name="val2" value="<?php echo e($request->val2); ?>" />
	<input type="hidden" id="" name="val3" value="<?php echo e($request->val3); ?>" />
	<input type="hidden" id="" name="val4" value="<?php echo e($request->val4); ?>" />
	<input type="hidden" id="" name="val5" value="<?php echo e($request->val5); ?>" />
	<input type="hidden" id="" name="val6" value="<?php echo e($request->val6); ?>" />
	<div class="row ml-4">
		<div class="col-xs-12">
			<div class="row align-items-center">
				<div class="col-xs-1 text-left m-2 p-2 rounded border">
					■　日程Import - 詳細
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
					<table class="table table-row">
						<tbody>
							<tr class="set-color">
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld1', '名称1'));?></th>
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld2', '名称2'));?></th>
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld3', '名称3'));?></th>
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld4', '組区'));?></th>
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld5', '施工棟'));?></th>
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld6', '装置'));?></th>
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld7', '職種'));?></th>
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld8', '管理物量コード'));?></th>
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld9', '消込管理'));?></th>
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld10', '消込方式'));?></th>
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld11', '物量'));?></th>
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld12', '着工日'));?></th>
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld13', '完工日'));?></th>
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld14', 'アイテム'));?></th>
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld15', 'HC'));?></th>
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld16', 'HJ'));?></th>
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld17', 'HS'));?></th>
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld18', 'HK'));?></th>
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld19', 'WBSコード'));?></th>
								<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld20', '内容'));?></th>
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
								<td class="real-space align-middle"><?php echo e($row['fld8']); ?></td>
								<td class="real-space align-middle"><?php echo e($row['fld9']); ?></td>
								<td class="real-space align-middle"><?php echo e($row['fld10']); ?></td>
								<td class="real-space align-middle"><?php echo e($row['fld11']); ?></td>
								<td class="real-space align-middle"><?php echo e($row['fld12']); ?></td>
								<td class="real-space align-middle"><?php echo e($row['fld13']); ?></td>
								<td class="real-space align-middle"><?php echo e($row['fld14']); ?></td>
								<td class="real-space align-middle"><?php echo e($row['fld15']); ?></td>
								<td class="real-space align-middle"><?php echo e($row['fld16']); ?></td>
								<td class="real-space align-middle"><?php echo e($row['fld17']); ?></td>
								<td class="real-space align-middle"><?php echo e($row['fld18']); ?></td>
								<td class="real-space align-middle"><?php echo e($row['fld19']); ?></td>
								<td class="real-space align-middle"><?php echo e($row['fld20']); ?></td>
							</tr>
							<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
						</tbody>
					</table>   
				</div>
			</div>
			<div class="row">
				<?php echo e($rows->appends(request()->query())->links()); ?>

			</div>
			<div class="row">
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
</form>
<?php $__env->stopSection(); ?>
<?php echo $__env->make('layouts/mainmenu/menu', \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/sches/makenittei/create.blade.php ENDPATH**/ ?>