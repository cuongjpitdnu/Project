<?php $__env->startSection('content'); ?>
<script>
$(function(){
	$('.toprev').on('click', function(){
		$('#indicator').trigger('click');
		var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/index';
		url += '?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
		url +=  '&val1=<?php echo e($request -> val1); ?>&val2=<?php echo e($request -> val2); ?>&val3=<?php echo e($request -> val3); ?>';
		url +=  '&val4=<?php echo e($request -> val4); ?>&val5=<?php echo e($request -> val5); ?>&pageunit=<?php echo e($request -> pageunit); ?>';
		window.location.href = url;
	});
	$('.toedit').on('click', function(){
		$('#indicator').trigger('click');
		var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/edit';
		url += '?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
		url += '&page=<?php echo e($request->page); ?>&pageunit=<?php echo e($request->pageunit); ?>';
		url += '&sort=<?php echo e($request->sort); ?>';
		url += '&direction=<?php echo e($request->direction); ?>';
		url +=  '&val1=<?php echo e($request -> val1); ?>&val2=<?php echo e($request -> val2); ?>&val3=<?php echo e($request -> val3); ?>';
		url +=  '&val4=<?php echo e($request -> val4); ?>&val5=<?php echo e($request -> val5); ?>';
		url +=  '&val101='+ $(this).val() +'&val102='+ $(this).attr("updatedat");
		window.location.href = url;
	});

	$('.toshow').on('click', function(){
		$('#indicator').trigger('click');
		var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/show';
		url += '?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
		url += '&pageunit=<?php echo e($request->pageunit); ?>&page=<?php echo e($request->page); ?>';
		url += '&sort=<?php echo e($request->sort); ?>';
		url += '&direction=<?php echo e($request->direction); ?>';
		url +=  '&val1=<?php echo e($request -> val1); ?>&val2=<?php echo e($request -> val2); ?>&val3=<?php echo e($request -> val3); ?>';
		url +=  '&val4=<?php echo e($request -> val4); ?>&val5=<?php echo e($request -> val5); ?>';
		url +=  '&val101='+ $(this).val();
		window.location.href = url;
	});

	$('.tohistory').on('click', function(){
		$('#indicator').trigger('click');
		var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/history';
		url += '?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
		url += '&searchpage=<?php echo e($request->page); ?>&pageunit=<?php echo e($request->pageunit); ?>';
		url += '&searchsort=<?php echo e($request->sort); ?>';
		url += '&searchdirection=<?php echo e($request->direction); ?>';
		url +=  '&val1=<?php echo e($request -> val1); ?>&val2=<?php echo e($request -> val2); ?>&val3=<?php echo e($request -> val3); ?>';
		url +=  '&val4=<?php echo e($request -> val4); ?>&val5=<?php echo e($request -> val5); ?>';
		url +=  '&val101='+ $(this).val();
		url += '&sort=fld1';
		url += '&direction=desc';
		window.location.href = url;
	});
});

</script>
<div class="row ml-4">
	<div class="col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　人員マスタ検索結果
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
			<div class="col-xs-1 m-3">
				<button type="button" class="toprev <?php echo e(config('system_const.btn_color_back')); ?>">
				<?php if(config('system_const.btn_img_back')!=''): ?><i class="<?php echo e(config('system_const.btn_img_back')); ?>"></i><?php endif; ?>
				<?php echo e(config('system_const.btn_char_back')); ?>

				</button>
			</div>
		</div>

		<div class="row">
			<div class="col-sm-12">
				<table class="table">
					<thead>
						<tr>
							<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld1', '社員番号'));?></th>
							<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld2', '名前'));?></th>
							<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld3', '所属'));?></th>
							<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld4', '会社名'));?></th>
							<th class="text-center"></th>
							<th class="text-center"></th>
						</tr>
					</thead>
					<tbody>
					<?php $__currentLoopData = $rows; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $row): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
					<tr>
					<td class="real-space align-middle text-right"><?php echo e(is_null($row->fld1) ? '' : $row->fld1); ?></td>
					<td class="real-space align-middle"><?php echo e($row->fld2); ?></td>
					<td class="real-space align-middle"><?php echo e(empty($row->fld3) ? '' : $row->fld3); ?></td>
					<td class="real-space align-middle"><?php echo e(is_null($row->fld4) ? '' : $row->fld4); ?></td>
					<td class="align-middle text-center">
						<?php if(!$menuInfo->IsReadOnly): ?>
							<button type="button" class="toedit <?php echo e(config('system_const.btn_color_rowedit')); ?>"
							value="<?php echo e(valueUrlEncode($row->ID)); ?>" updatedat="<?php echo e(valueUrlEncode($row->Updated_at)); ?>"  >
							<?php if(config('system_const.btn_img_rowedit')!=''): ?><i class="<?php echo e(config('system_const.btn_img_rowedit')); ?>"></i><?php endif; ?>
							<?php echo e(config('system_const.btn_char_rowedit')); ?>

							</button>
						<?php else: ?>
							<button type="button" class="toshow <?php echo e(config('system_const.btn_color_rowinfo')); ?>"
							value="<?php echo e(valueUrlEncode($row->ID)); ?>">
							<?php if(config('system_const.btn_img_rowinfo')!=''): ?><i class="<?php echo e(config('system_const.btn_img_rowinfo')); ?>"></i><?php endif; ?>
							<?php echo e(config('system_const.btn_char_rowinfo')); ?>

							</button>
						<?php endif; ?>
					</td>
					<td class="align-middle text-center">
						<button type="button" class="tohistory <?php echo e(config('system_const.btn_color_rowhistory')); ?>"
							value="<?php echo e(valueUrlEncode($row->ID)); ?>">
							<?php if(config('system_const.btn_img_rowhistory')!=''): ?><i class="<?php echo e(config('system_const.btn_img_rowhistory')); ?>"></i><?php endif; ?>
							<?php echo e(config('system_const.btn_char_rowhistory')); ?>

						</button>
					</td>
					</tr>
					<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
					</tbody>
				</table>
				<?php echo e($rows->appends(request()->query())->links()); ?>

			</div>

		</div>

	</div>
</div>
<?php $__env->stopSection(); ?>

<?php echo $__env->make('layouts/mainmenu/menu', \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/mst/member/search.blade.php ENDPATH**/ ?>