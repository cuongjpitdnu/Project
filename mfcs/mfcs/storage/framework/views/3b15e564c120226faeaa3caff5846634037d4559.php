<?php $__env->startSection('content'); ?>

<script>

$(function(){

	$('[name="pageunit"]').change(function(e){
		$('#indicator').trigger('click');
		var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/index';
		url += '?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>';
		url += '&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
		url += '&page=1';
		url += '&pageunit=' + $(this).val();
		url += '&sort=<?php echo e(@\Request::input('sort')); ?>';
		url += '&direction=<?php echo e(@\Request::input('direction')); ?>';
		window.location.href = url;
	});

	$('#create').on('click', function(){
		$('#indicator').trigger('click');
		var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/create';
		url += '?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>';
		url += '&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
		url += '&page=<?php echo e(@$request->page); ?>';
		url += '&pageunit=<?php echo e(@$request->pageunit); ?>';
		url += '&sort=<?php echo e(@$request->sort); ?>';
		url += '&direction=<?php echo e(@$request->direction); ?>';
		window.location.href = url;
	});

	$('[id=edit]').on('click', function(){
		var val1 = $(this).val();
		$('#indicator').trigger('click');
		var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/edit';
		url += '?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>';
		url += '&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
		url += '&page=<?php echo e(@$request->page); ?>';
		url += '&pageunit=<?php echo e(@$request->pageunit); ?>';
		url += '&sort=<?php echo e(@$request->sort); ?>';
		url += '&direction=<?php echo e(@$request->direction); ?>';
		url += '&val1=' + val1;
		window.location.href = url;
	});

	$('[id=show]').on('click', function(){
		var val1 = $(this).val();
		$('#indicator').trigger('click');
		var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/show';
		url += '?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>';
		url += '&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
		url += '&page=<?php echo e(@$request->page); ?>';
		url += '&pageunit=<?php echo e(@$request->pageunit); ?>';
		url += '&sort=<?php echo e(@$request->sort); ?>';
		url += '&direction=<?php echo e(@$request->direction); ?>';
		url += '&val1=' + val1;
		window.location.href = url;
	});

})

</script>

<style>
	.table-ability th{
		text-align: center;
	}
</style>
<div class="row ml-4">

	<div class="col-xs-12">

		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　能力時間マスタ
			</div>
		</div>

		<?php if(!$menuInfo->IsReadOnly): ?>
		<div class="col-xs-1 m-1">
			<button id="create" type="button" class="<?php echo e(config('system_const.btn_color_new')); ?>"><?php if(config('system_const.btn_img_new')!=''): ?><i class="<?php echo e(config('system_const.btn_img_new')); ?>"></i><?php endif; ?><?php echo e(config('system_const.btn_char_new')); ?></button>
		</div>
		<?php endif; ?>
		<div class="row">
			<div class="col-sm-12" style="padding-left:1rem !important;">
				<table class="table table-ability">
					<thead>
						<tr>
							<th><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld1', '能力時間名称'));?></th>
							<th><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld2', '職制'));?></th>
							<th><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld3', '施工棟'));?></th>
							<th><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld4', '職種'));?></th>
							<th><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld5', '開始日'));?></th>
							<th><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld6', '終了日'));?></th>
							<th><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld7', '工数'));?></th>
							<th></th>
						</tr>
					</thead>
					<?php $__currentLoopData = $abilities; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $ability): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
					<tr>
						<td><?php echo e($ability['fld1']); ?></td>
						<td><?php echo e($ability['fld2']); ?></td>
						<td><?php echo e($ability['fld3']); ?></td>
						<td><?php echo e($ability['fld4']); ?></td>
						<td style="text-align: center"><?php echo e($ability['fld5']); ?></td>
						<td style="text-align: center"><?php echo e($ability['fld6']); ?></td>
						<td style="text-align: right"><?php echo e($ability['fld7']); ?></td>
						<td>
							<?php if($menuInfo->IsReadOnly): ?>
							<div class="col-xs-1 m-1">
								<button id="show" type="button" value="<?php echo e(valueUrlEncode($ability['fld8'])); ?>" class="<?php echo e(config('system_const.btn_color_rowinfo')); ?>"><?php if(config('system_const.btn_img_rowinfo')!=''): ?><i class="<?php echo e(config('system_const.btn_img_rowinfo')); ?>"></i><?php endif; ?><?php echo e(config('system_const.btn_char_rowinfo')); ?></button>
							</div>
							<?php else: ?>
							<div class="col-xs-1 m-1">
								<button id="edit" type="button" value="<?php echo e(valueUrlEncode($ability['fld8'])); ?>" class="<?php echo e(config('system_const.btn_color_rowedit')); ?>"><?php if(config('system_const.btn_img_rowedit')!=''): ?><i class="<?php echo e(config('system_const.btn_img_rowedit')); ?>"></i><?php endif; ?><?php echo e(config('system_const.btn_char_rowedit')); ?></button>
							</div>
							<?php endif; ?>
						</td>
					</tr>
					<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
				</table>
				<?php echo e($abilities->appends(request()->query())->links()); ?>

				<div class="pageunit" style="float:left;">
					表示件数
					<select name="pageunit" class="pageunit-width">
						<option value= <?php echo e(config('system_const.displayed_results_1')); ?> <?php echo e(\Request::input('pageunit') == config('system_const.displayed_results_1') ? 'selected' : ''); ?>> <?php echo e(config('system_const.displayed_results_1')); ?> </option>
						<option value= <?php echo e(config('system_const.displayed_results_2')); ?> <?php echo e(\Request::input('pageunit') == config('system_const.displayed_results_2') ? 'selected' : ''); ?>> <?php echo e(config('system_const.displayed_results_2')); ?> </option>
						<option value= <?php echo e(config('system_const.displayed_results_3')); ?> <?php echo e(\Request::input('pageunit') == config('system_const.displayed_results_3') ? 'selected' : ''); ?>> <?php echo e(config('system_const.displayed_results_3')); ?> </option>
					</select>
				</div>
			</div>
		</div>
	</div>
</div>

<?php $__env->stopSection(); ?>
<?php echo $__env->make('layouts/mainmenu/menu', \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/mst/ability/index.blade.php ENDPATH**/ ?>