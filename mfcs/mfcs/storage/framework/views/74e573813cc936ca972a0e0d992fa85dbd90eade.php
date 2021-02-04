<?php $__env->startSection('content'); ?>
<script>
$(function(){
	$('.toprev').on('click', function(){
		$('#indicator').trigger('click');
		var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/search';
		url += '?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
		url += '&pageunit=<?php echo e($request->pageunit); ?>&page=<?php echo e($request->searchpage); ?>';
		url += '&sort=<?php echo e($request->searchsort); ?>';
		url += '&direction=<?php echo e($request->searchdirection); ?>';
		url +=  '&val1=<?php echo e($request -> val1); ?>&val2=<?php echo e($request -> val2); ?>&val3=<?php echo e($request -> val3); ?>';
		url +=  '&val4=<?php echo e($request -> val4); ?>&val5=<?php echo e($request -> val5); ?>';
		window.location.href = url;
	});
	$('.tocreate').on('click', function(){
		$('#indicator').trigger('click');
		var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/historycreate';
		url += '?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
		url += '&pageunit=<?php echo e($request->pageunit); ?>&searchpage=<?php echo e($request->searchpage); ?>';
		url += '&searchsort=<?php echo e($request->searchsort); ?>&searchdirection=<?php echo e($request->searchdirection); ?>';
		url += '&page=<?php echo e($request->page); ?>&sort=<?php echo e($request->sort); ?>&direction=<?php echo e($request->direction); ?>';
		url += '&val1=<?php echo e($request->val1); ?>&val2=<?php echo e($request->val2); ?>&val3=<?php echo e($request->val3); ?>';
		url += '&val4=<?php echo e($request->val4); ?>&val5=<?php echo e($request->val5); ?>';
		url += '&val101= <?php echo e($request->val101); ?>';
		window.location.href = url;
	});
	$('.toedit').on('click', function(){
		$('#indicator').trigger('click');
		var val201 = $(this).attr('sdate');
		var val202 = $(this).attr('updatedat');
		var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/historyedit';
		url += '?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
		url += '&pageunit=<?php echo e($request->pageunit); ?>&searchpage=<?php echo e($request->searchpage); ?>';
		url += '&searchsort=<?php echo e($request->searchsort); ?>&searchdirection=<?php echo e($request->searchdirection); ?>';
		url += '&page=<?php echo e($request->page); ?>&sort=<?php echo e($request->sort); ?>&direction=<?php echo e($request->direction); ?>';
		url +=  '&val1=<?php echo e($request->val1); ?>&val2=<?php echo e($request->val2); ?>&val3=<?php echo e($request->val3); ?>';
		url +=  '&val4=<?php echo e($request->val4); ?>&val5=<?php echo e($request->val5); ?>';
		url +=  '&val101= <?php echo e($request->val101); ?>&val201=' + val201;
		url +=  '&val202=' + val202;
		window.location.href = url;
	});
	$('.toshow').on('click', function(){
		$('#indicator').trigger('click');
		var val201 = $(this).attr('sdate');
		var val202 = $(this).attr('updatedat');
		var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/historyshow';
		url += '?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
		url += '&pageunit=<?php echo e($request->pageunit); ?>&searchpage=<?php echo e($request->searchpage); ?>';
		url += '&searchsort=<?php echo e($request->searchsort); ?>&searchdirection=<?php echo e($request->searchdirection); ?>';
		url += '&page=<?php echo e($request->page); ?>&sort=<?php echo e($request->sort); ?>&direction=<?php echo e($request->direction); ?>';
		url +=  '&val1=<?php echo e($request->val1); ?>&val2=<?php echo e($request->val2); ?>&val3=<?php echo e($request->val3); ?>';
		url +=  '&val4=<?php echo e($request->val4); ?>&val5=<?php echo e($request->val5); ?>';
		url +=  '&val101= <?php echo e($request->val101); ?>&val201=' + val201;
		url +=  '&val202=' + val202;
		window.location.href = url;
	});

	$('.delete').on('click', function(){
		if (!window.confirm('<?php echo e(config("message.msg_cmn_if_001")); ?>')){
			return;
		}
		$('#indicator').trigger('click');
		var val201 = $(this).attr('sdate');
		var val202 = $(this).attr('updatedat');

		$('#val201').val(val201);
		$('#val202').val(val202);
		let pageRedirect = '<?php echo e((count($rows) == 1) ? ((isset($request->page) && $request->page > 1) ? ($request->page - 1) : 1) : (isset($request->page) ? $request->page : 1)); ?>';
		$('input[name=page]').val(pageRedirect);
		$('#mainform').submit();
	});
});

</script>

<form action="<?php echo e(url('/')); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/historydelete" method="POST" id="mainform">
	<?php echo csrf_field(); ?>
	<input type="hidden" id="kindid" name="cmn1" value="<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>">
	<input type="hidden" id="menuid" name="cmn2" value="<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>">
	<input type="hidden" id="" name="pageunit" value="<?php echo e($request -> pageunit); ?>">
	<input type="hidden" id="" name="searchpage" value="<?php echo e($request -> searchpage); ?>">
	<input type="hidden" id="" name="searchsort" value="<?php echo e($request -> searchsort); ?>">
	<input type="hidden" id="" name="searchdirection" value="<?php echo e($request -> searchdirection); ?>">
	<input type="hidden" id="" name="page" value="<?php echo e($request -> page); ?>">
	<input type="hidden" id="" name="sort" value="<?php echo e($request -> sort); ?>">
	<input type="hidden" id="" name="direction" value="<?php echo e($request -> direction); ?>">
	<input type="hidden" id="" name="val1" value="<?php echo e($request -> val1); ?>">
	<input type="hidden" id="" name="val2" value="<?php echo e($request -> val2); ?>">
	<input type="hidden" id="" name="val3" value="<?php echo e($request -> val3); ?>">
	<input type="hidden" id="" name="val4" value="<?php echo e($request -> val4); ?>">
	<input type="hidden" id="" name="val5" value="<?php echo e($request -> val5); ?>">
	<input type="hidden" id="" name="val101" value="<?php echo e($request -> val101); ?>">
	<input type="hidden" id="val201" name="val201" value="">
	<input type="hidden" id="val202" name="val202" value="">
</form>
<div class="row ml-4">
	<div class="col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　人員マスタ履歴表示
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

		<div class="row align-items-center">
			<?php if(!$menuInfo->IsReadOnly): ?>
			<div class="col-xs-1 mt-0 mr-0 mb-3 ml-3">
			<button type="button" name="tocreate" class="tocreate <?php echo e(config('system_const.btn_color_new')); ?>">
				<?php if(config('system_const.btn_img_new')!=''): ?><i class="<?php echo e(config('system_const.btn_img_new')); ?>"></i><?php endif; ?>
				<?php echo e(config('system_const.btn_char_new')); ?>

				</button>
			</div>
			<?php endif; ?>
			<div class="col-xs-1 mt-0 mr-3 mb-3 ml-3">
				<button type="button" class="<?php echo e(config('system_const.btn_color_readac')); ?>">
				<?php if(config('system_const.btn_img_readac')!=''): ?><i class="<?php echo e(config('system_const.btn_img_readac')); ?>"></i><?php endif; ?>
				<?php echo e(config('system_const.btn_char_readac')); ?>

				</button>
			</div>
			<span>名前: <?php echo e($memberName); ?></span>
		</div>

		<div class="row">
			<div class="col-sm-12">
				<table class="table">
					<thead>
						<tr>
							<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld1', '開始日'));?></th>
							<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld2', '終了日'));?></th>
							<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld3', '会社名'));?></th>
							<th class="text-center"><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld4', '所属'));?></th>
							<th class="text-center"></th>
							<th class="text-center"></th>
						</tr>
					</thead>
					<tbody>
					<?php $__currentLoopData = $rows; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $row): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
					<tr>
					<td class="real-space align-middle text-center"><?php echo e(Carbon\Carbon::parse($row->fld1)->format('Y/m/d')); ?></td>
					<td class="real-space align-middle text-center"><?php echo e(is_null($row->fld2) ? '' : Carbon\Carbon::parse($row->fld2)->format('Y/m/d')); ?></td>
					<td class="real-space align-middle"><?php echo e(is_null($row->fld3) ? '' : $row->fld3); ?></td>
					<td class="real-space align-middle"><?php echo e(empty($row->fld4) ? '' : $row->fld4); ?></td>
					<td class="align-middle text-center">
						<?php if(!$menuInfo->IsReadOnly): ?>
							<button type="button" class="toedit <?php echo e(config('system_const.btn_color_rowedit')); ?>"
							value="" sdate="<?php echo e(valueUrlEncode($row->fld1 )); ?>"
							updatedat="<?php echo e(valueUrlEncode($row->Updated_at)); ?>">
							<?php if(config('system_const.btn_img_rowedit')!=''): ?><i class="<?php echo e(config('system_const.btn_img_rowedit')); ?>"></i><?php endif; ?>
							<?php echo e(config('system_const.btn_char_rowedit')); ?>

							</button>
						<?php else: ?>
							<button type="button" class="toshow <?php echo e(config('system_const.btn_color_rowinfo')); ?>"
							value="" sdate="<?php echo e(valueUrlEncode($row->fld1 )); ?>" updatedat="<?php echo e(valueUrlEncode($row->Updated_at)); ?>">
							<?php if(config('system_const.btn_img_rowinfo')!=''): ?><i class="<?php echo e(config('system_const.btn_img_rowinfo')); ?>"></i><?php endif; ?>
							<?php echo e(config('system_const.btn_char_rowinfo')); ?>

							</button>
						<?php endif; ?>
					</td>
					<td class="align-middle text-center">
						<?php if(!$menuInfo->IsReadOnly): ?>
						<button type="button" class="delete <?php echo e(config('system_const.btn_color_rowdelete')); ?>"
							value="" sdate="<?php echo e(valueUrlEncode($row->fld1 )); ?>"
							updatedat="<?php echo e(valueUrlEncode($row->Updated_at)); ?>">
							<?php if(config('system_const.btn_img_rowdelete')!=''): ?><i class="<?php echo e(config('system_const.btn_img_rowdelete')); ?>"></i><?php endif; ?>
							<?php echo e(config('system_const.btn_char_rowdelete')); ?>

						</button>
						<?php endif; ?>
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

<?php echo $__env->make('layouts/mainmenu/menu', \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/mst/member/history.blade.php ENDPATH**/ ?>