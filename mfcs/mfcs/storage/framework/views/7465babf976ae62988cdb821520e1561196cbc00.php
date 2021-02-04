<?php $__env->startSection('content'); ?>
<script>
	$(function() {
		$('select[name=pageunit]').on('change', function(e) {
			$('#indicator').trigger('click');
			var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/';
			url += 'index?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
			url += '&page=1&pageunit=' + $(this).val();
			url += '&sort=<?php echo e(\Request::input('sort') ? \Request::input('sort') : 'fld1'); ?>';
			url += '&direction=<?php echo e(\Request::input('direction') ? \Request::input('direction') : 'asc'); ?>';
			window.location.href = url;
		});

		$('.create').on('click', function(e) {
			$('#indicator').trigger('click');
			var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/create';
			url += '?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
			url += '&page=<?php echo e(isset($request->page) ? $request->page : 1); ?>';
			url += '&pageunit=<?php echo e(isset($request->pageunit) ? $request->pageunit : 10); ?>';
			url += '&sort=<?php echo e(isset($request->sort) ? $request->sort : "fld1"); ?>';
			url += '&direction=<?php echo e(isset($request->direction) ? $request->direction : "asc"); ?>';
			window.location.href = url;
		});

		$('.edit').on('click', function(e) {
			$('#indicator').trigger('click');
			var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/edit';
			url += '?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
			url += '&page=<?php echo e(isset($request->page) ? $request->page : 1); ?>';
			url += '&pageunit=<?php echo e(isset($request->pageunit) ? $request->pageunit : 10); ?>';
			url += '&sort=<?php echo e(isset($request->sort) ? $request->sort : "fld1"); ?>';
			url += '&direction=<?php echo e(isset($request->direction) ? $request->direction : "asc"); ?>';
			url += '&val1=' + $(this).val();
			window.location.href = url;
		});

		$('.show').on('click', function(e) {
			$('#indicator').trigger('click');
			var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/show';
			url += '?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
			url += '&page=<?php echo e(isset($request->page) ? $request->page : 1); ?>';
			url += '&pageunit=<?php echo e(isset($request->pageunit) ? $request->pageunit : 10); ?>';
			url += '&sort=<?php echo e(isset($request->sort) ? $request->sort : "fld1"); ?>';
			url += '&direction=<?php echo e(isset($request->direction) ? $request->direction : "asc"); ?>';
			url += '&val1=' + $(this).val();
			window.location.href = url;
		});

		$('.delete').on('click', function(e) {
			if (window.confirm('<?php echo e(config("message.msg_cmn_if_001")); ?>')) {
				$('#indicator').trigger('click');
				$('input[name=fld1]').val($(this).val());
				$('input[name=fld4]').val($(this).attr('fld4'));
				$('#mainform').submit();
			}
		});
	});
</script>

<style>
	.table-row tr th,
	.table-row tr td { text-align: left; vertical-align: middle; }
	.table-row tr th.align-center,
	.table-row tr td.align-center { text-align: center; vertical-align: middle; }
</style>

<div class="row ml-4">
	<div class="col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　工程定義マスタ
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
				<button type="button" name="create" class="create <?php echo e(config('system_const.btn_color_new')); ?>">
				<?php if(config('system_const.btn_img_new')!=''): ?>
					<i class="<?php echo e(config('system_const.btn_img_new')); ?>"></i>
				<?php endif; ?>
					<?php echo e(config('system_const.btn_char_new')); ?></button>
			</div>
			<?php endif; ?>
		</div>
		<div class="row">
			<div class="col-sm-12" style="padding-left:1rem !important;">
				<table class="table table-row">
					<thead>
						<th class="align-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld1', trans('mstdist.sortable.fld1')));?></th>
						<th class="align-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld2', trans('mstdist.sortable.fld2')));?></th>
						<th class="align-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld3', trans('mstdist.sortable.fld3')));?></th>
						<th class="align-center"></th>
						<?php if(!$menuInfo->IsReadOnly): ?>
						<th class="align-center"></th>
						<?php endif; ?>
					</thead>
					<tbody>
						<?php $__currentLoopData = $rows; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $row): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
							<tr>
								<td class="real-space"><?php echo e($row['fld1']); ?></td>
								<td class="real-space"><?php echo e($row['fld2']); ?></td>
								<td class="real-space"><?php echo e($row['fld3']); ?></td>
								<td class="align-center">
									<?php if(!$menuInfo->IsReadOnly): ?>
										<button type="button" class="edit <?php echo e(config('system_const.btn_color_rowedit')); ?>"
											value="<?php echo e(valueUrlEncode($row['fld1'])); ?>" fld4="<?php echo e(valueUrlEncode($row['fld4'])); ?>">
											<?php if(config('system_const.btn_img_rowedit') !== ''): ?>
												<i class="<?php echo e(config('system_const.btn_img_rowedit')); ?>"></i>
											<?php endif; ?>
											<?php echo e(config('system_const.btn_char_rowedit')); ?></button>
									<?php else: ?>
										<button type="button" class="show <?php echo e(config('system_const.btn_color_rowinfo')); ?>"
											value="<?php echo e(valueUrlEncode($row['fld1'])); ?>" fld4="<?php echo e(valueUrlEncode($row['fld4'])); ?>">
											<?php if(config('system_const.btn_img_rowinfo') !== ''): ?>
												<i class="<?php echo e(config('system_const.btn_img_rowinfo')); ?>"></i>
											<?php endif; ?>
											<?php echo e(config('system_const.btn_char_rowinfo')); ?></button>
									<?php endif; ?>
								</td>
								<?php if(!$menuInfo->IsReadOnly): ?>
								<td class="align-center">
									<button type="button" class="delete <?php echo e(config('system_const.btn_color_delete')); ?> btn-sm"
										value="<?php echo e(valueUrlEncode($row['fld1'])); ?>" fld4="<?php echo e(valueUrlEncode($row['fld4'])); ?>">
										<?php if(config('system_const.btn_img_delete') !== ''): ?>
											<i class="<?php echo e(config('system_const.btn_img_delete')); ?>"></i>
										<?php endif; ?>
										<?php echo e(config('system_const.btn_char_delete')); ?></button>
									</td>
								<?php endif; ?>
							</tr>
						<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
					</tbody>
				</table>
				<?php echo e($rows->appends(request()->query())->links()); ?>

				<div class="pageunit" style="float:left;">
					表示件数
					<select name="pageunit" class="pageunit-width">
						<option value="<?php echo e(config('system_const.displayed_results_1')); ?>"
							<?php echo e((int)\Request::input('pageunit') === config('system_const.displayed_results_1') ? 'selected' : ''); ?>><?php echo e(config('system_const.displayed_results_1')); ?></option>
						<option value="<?php echo e(config('system_const.displayed_results_2')); ?>"
							<?php echo e((int)\Request::input('pageunit') === config('system_const.displayed_results_2') ? 'selected' : ''); ?>><?php echo e(config('system_const.displayed_results_2')); ?></option>
						<option value="<?php echo e(config('system_const.displayed_results_3')); ?>"
							<?php echo e((int)\Request::input('pageunit') === config('system_const.displayed_results_3') ? 'selected' : ''); ?>><?php echo e(config('system_const.displayed_results_3')); ?></option>
					</select>
				</div>
			</div>
		</div>

		<form action="<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/delete" method="POST" id="mainform">
			<?php echo csrf_field(); ?>
			<input type="hidden" name="cmn1" value="<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>" />
			<input type="hidden" name="cmn2" value="<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>" />
			<input type="hidden" name="page" value="<?php echo e(isset($request->page) ? $request->page : 1); ?>" />
			<input type="hidden" name="pageunit" value="<?php echo e(isset($request->pageunit) ? $request->pageunit : 10); ?>" />
			<input type="hidden" name="sort" value="<?php echo e(isset($request->sort) ? $request->sort : "fld1"); ?>" />
			<input type="hidden" name="direction" value="<?php echo e(isset($request->direction) ? $request->direction : "asc"); ?>" />
			<input type="hidden" name="fld1" value="" />
			<input type="hidden" name="fld4" value="" />
		</form>
	</div>
</div>
<?php $__env->stopSection(); ?>
<?php echo $__env->make('layouts/mainmenu/menu', \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/Sches/Distr/index.blade.php ENDPATH**/ ?>