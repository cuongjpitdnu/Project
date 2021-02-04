<?php $__env->startSection('content'); ?>

<script>
$(function(){
	$('[data-toggle="tooltip"]').tooltip()

	$('#selectdate').datepicker();

	$('#selectdate').change(function() {
		var val1 = $(this).val();
		var val2 = $("#val2").val();
		var val3 = $('select[name=val3] option').filter(':selected').val();
		var val4 = encodeURIComponent($('input[name="val4"]').val());
		var val5 = encodeURIComponent($('input[name="val5"]').val());
		var pageunit = $('[name="pageunit"]').val();
		$('#indicator').trigger('click');
		var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/changedate?'+
		'cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>&val1=' 
		+ val1 + '&val2=' + val2 + '&val3=' + val3 + '&val4=' + val4 + '&val5=' + val5 + '&pageunit=' + pageunit;
		window.location.href = url;
	});

	
	$('.newItem').on('click', function(){
		$('#indicator').trigger('click');
		var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/create';
		url += '?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
		window.location.href = url;
	});
	
	$('.search').on('click', function(){
		var val1 = $('#selectdate').val();
		var val2 = $('input[name="val2"]').val();
		var val3 = $('select[name=val3] option').filter(':selected').val();
		var val4 = encodeURIComponent($('input[name="val4"]').val());
		var val5 = encodeURIComponent($('input[name="val5"]').val());
		var pageunit = $('[name="pageunit"]').val();

		$('#indicator').trigger('click');
		var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/search';
		url += '?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
		url +=  '&val1=' + val1 + '&val2=' + val2 + '&val3=' + val3 + '&val4=' + val4 + '&val5=' + val5 + '&pageunit=' + pageunit;
		url += '&sort=fld2';
		url += '&direction=asc';
		window.location.href = url;
	});

	$('#orgtree')
	.on('activate_node.jstree', function (e, data) {
		var selectedID = data.node.li_attr.item_id;
		$('#select_grp_id').val(selectedID);
		var selectedName = data.node.li_attr.item_name;
		$('#select_grp_name').val(selectedName);
	}).jstree();

	$('#select_org_ok').on('click', function(){
		var val2= $('#select_grp_id').val();
		$('input[name="val2"]').val(val2);
		var grpName = $('#select_grp_name').val();
		$('[name=groupname]').val(grpName);
		$('#org_select_dialog').modal('hide');
	});

	$('.clearorg').on('click', function(){
		$('[name=groupname]').val(null);
		$('input[name="val2"]').val(null);
	});


})

</script>

<input type="hidden" id="val2" name="val2" value="<?php echo e(old('val2', $grpID)); ?>">
<input type="hidden" id="select_grp_id" name="select_grp_id" value="<?php echo e(old('select_grp_id', $grpID)); ?>">
<input type="hidden" id="select_grp_name" name="select_grp_name" value="<?php echo e(old('select_grp_name', $grpName)); ?>">

<div class="row ml-4">

	<div class="col-xs-12">

		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　人員マスタ
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
			<div class="col-xs-3 m-1 pr-5 clearfix">
				<button type="button" name="newItem" class="newItem <?php echo e(config('system_const.btn_color_new')); ?>">
				<?php if(config('system_const.btn_img_new')!=''): ?><i class="<?php echo e(config('system_const.btn_img_new')); ?>"></i><?php endif; ?>
				<?php echo e(config('system_const.btn_char_new')); ?>

				</button>
			</div>
			<?php endif; ?>
		</div>
		<div class="row align-items-center">
			<div class="col-xs-3 m-1 pr-5">
				<button type="button" name="search" class="search <?php echo e(config('system_const.btn_color_search')); ?>">
					<?php if(config('system_const.btn_img_search')!=''): ?><i class="<?php echo e(config('system_const.btn_img_search')); ?>"></i><?php endif; ?>
					<?php echo e(config('system_const.btn_char_search')); ?>

					</button>
			</div>
		</div>

		<div class="row align-items-center">
			<table class="table table-borderless">
				<tbody>
				<tr>
					<td class="align-middle">基準日：</td>
					<td>
						<input id="selectdate" type="text" maxlength="10" name="val1" size="14" value="<?php echo e(old('val1',$request->val1)); ?>"
						autocomplete="off">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val1'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
				</tr>
				
				<tr>
					<td class="align-middle">所属：</td>
					<td>
						<input type="text" name="groupname" value="<?php echo e(old('groupname', $grpName)); ?>" readonly="" tabindex="-1" data-toggle="modal" data-target="#org_select_dialog">
						<input type="hidden" name="groupname" value="<?php echo e(old('groupname', $grpName)); ?>">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val2'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<button type="button" id="select" class="<?php echo e(config('system_const.btn_color_file')); ?>" data-toggle="modal" data-target="#org_select_dialog">
								<i class="<?php echo e(config('system_const.btn_img_file')); ?>"></i><?php echo e(config('system_const.btn_char_file')); ?>

							</button>
						</span>
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<button type="button" name="clearorg" class="clearorg <?php echo e(config('system_const.btn_color_clear')); ?>">
								<i class="<?php echo e(config('system_const.btn_img_clear')); ?>"></i><?php echo e(config('system_const.btn_char_clear')); ?>

							</button>
						</span>
					</td>
					<?php echo $__env->make('mst/org/select', ['mstOrgCommon' => $mstOrgCommon, 'activeOrgID' => $grpID, 'folderOnly' => false ], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
				</tr>
	
				<tr>
					<td class="align-middle">外注会社：</td>
					<td>
						<select name="val3">
						<option value="<?php echo e(valueUrlEncode(0)); ?>" ></option>
							<?php $__currentLoopData = $arrKanrenID; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $item): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
							<option value="<?php echo e(valueUrlEncode($item['id'])); ?>" <?php if(valueUrlDecode(old('val3',$request->val3)) === $item['id']): ?> selected <?php endif; ?>><?php echo e($item['name']); ?></option>
							<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
						</select>
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val3'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
				</tr>
	
				<tr>
					<td class="align-middle">社員番号：</td>
					<td>
					<input type="text" name="val4" value="<?php echo e(old('val4',$request->val4)); ?>" class="text-right" autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val4'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
				</tr>
				<tr>
					<td class="align-middle">氏名：</td>
					<td>
						<input type="text" name="val5" value="<?php echo e(old('val5',$request->val5)); ?>" autocomplete="off" maxlength="50">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val5'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
				</tr>
				<tr>
					<td class="align-middle">表示件数：</td>
					<td>
						<select class="pageunit pageunit-width" name="pageunit">
							<option value= <?php echo e(valueUrlEncode(config('system_const.displayed_results_1'))); ?> <?php echo e((int)valueUrlDecode(old('pageunit',\Request::input('pageunit'))) === config('system_const.displayed_results_1') ? 'selected' : ''); ?>> <?php echo e(config('system_const.displayed_results_1')); ?> </option>
							<option value= <?php echo e(valueUrlEncode(config('system_const.displayed_results_2'))); ?> <?php echo e((int)valueUrlDecode(old('pageunit',\Request::input('pageunit'))) === config('system_const.displayed_results_2') ? 'selected' : ''); ?>> <?php echo e(config('system_const.displayed_results_2')); ?> </option>
							<option value= <?php echo e(valueUrlEncode(config('system_const.displayed_results_3'))); ?> <?php echo e((int)valueUrlDecode(old('pageunit',\Request::input('pageunit'))) === config('system_const.displayed_results_3') ? 'selected' : ''); ?>> <?php echo e(config('system_const.displayed_results_3')); ?> </option>
						</select>
						※1ページあたり
					</td>
				</tr>
				</tbody>
			</table>
		</div>

	</div>
</div>

<?php $__env->stopSection(); ?>

<?php echo $__env->make('layouts/mainmenu/menu', \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/mst/member/index.blade.php ENDPATH**/ ?>