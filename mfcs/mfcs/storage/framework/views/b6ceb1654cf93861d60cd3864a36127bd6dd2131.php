<script>

$(function(){

	$('[data-toggle="tooltip"]').tooltip()

	$('#orgtree')
	.on('activate_node.jstree', function (e, data) {
		var selectedID = data.node.li_attr.item_id;
		$('#select_org_id').val(selectedID);
		var selectedName = data.node.li_attr.item_name;
		$('#select_org_name').val(selectedName);
	}).jstree();

	$('#select_org_ok').on('click', function(){
		var val2 = $('#select_org_id').val();
		$("input[name='val2']").val(val2);
		var orgName = $('#select_org_name').val();
		$('[name=orgname]').val(orgName);
		$('#org_select_dialog').modal('hide');
	});

	$('#clear').on('click', function(){
		$("input[name='val2']").val("<?php echo e(valueUrlEncode(0)); ?>");
		$('[name=orgname]').val("<?php echo e(config('system_const.org_null_name')); ?>");
	});

	$('#save').on('click', function(){
		$('#indicator').trigger('click');
		$('#mainform').submit();
	});

	$('#cancel').on('click', function(){
		$('#indicator').trigger('click');
		var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/index';
		url += '?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>';
		url += '&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
		url += '&page=<?php echo e($request->page); ?>';
		url += '&pageunit=<?php echo e($request->pageunit); ?>';
		url += '&sort=<?php echo e($request->sort); ?>';
		url += '&direction=<?php echo e($request->direction); ?>';
		window.location.href = url;
	});

	$('.selectdate').datepicker();
})

</script>

<input type="hidden" id="select_org_id" value="<?php echo e(valueUrlEncode(0)); ?>">
<input type="hidden" id="select_org_name" value="<?php echo e(config('system_const.org_null_name')); ?>">

<link rel="stylesheet" href="<?php echo e(url('/css/baseeditar.css')); ?>">

<div class="row ml-4">
	<div class="col-xs-12">

		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　能力時間マスタ<?php if($target == 'show'): ?>参照<?php elseif($target == 'create'): ?>登録<?php elseif($target == 'edit'): ?>更新<?php endif; ?>
			</div>
		</div>

		<div id="error">
		</div>

		<?php if(isset($originalError) && count($originalError)): ?>
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
		
		<form action="<?php echo e(url('/')); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/save" method="POST" id="mainform">
			<?php echo csrf_field(); ?>
			<input type="hidden" name="method" value="<?php echo e($target); ?>">
			<input type="hidden" id="kindid" name="cmn1" value="<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>">
			<input type="hidden" id="menuid" name="cmn2" value="<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>">
			<input type="hidden" name="page" value="<?php echo e($request->page); ?>">
			<input type="hidden" name="pageunit" value="<?php echo e($request->pageunit); ?>">
			<input type="hidden" name="sort" value="<?php echo e($request->sort); ?>">
			<input type="hidden" name="direction" value="<?php echo e($request->direction); ?>">
			<input type="hidden" name="val2" value="<?php echo e(old('val2', valueUrlEncode(@$abilityData['GroupID']))); ?>">
			<?php if($target == 'edit'): ?>
			<input type="hidden" name="val8" value="<?php echo e(old('val8', $abilityData['Updated_at'])); ?>">
			<input type="hidden" name="val10" value="<?php echo e(valueUrlEncode($abilityData['ID'])); ?>">
			<?php endif; ?>

			<table class="table table-borderless">
				<tbody>
					<tr>
						<td>能力時間名称*：</td>
						<td>
							<input type="text" name="val1" maxlength="100" value="<?php echo e(old('val1', @$abilityData['AbilityName'])); ?>" <?php echo e($target == "show" || $target == "edit" ? 'disabled="disabled"' : ''); ?> autocomplete="off">
							<?php if($target == "edit"): ?>
							<input type="hidden" name="val1" value="<?php echo e(old('val1', @$abilityData['AbilityName'])); ?>">
							<?php endif; ?>
						</td>
						<td style="padding:0px; vertical-align: middle;">
							<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val1'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
							</span>
						</td>

						<td>職制：</td>
						<td>
							<input type="text" name="orgname" value="<?php echo e(old('orgname', @$abilityData['GroupName'])); ?>" readonly tabindex="-1" data-toggle="modal" data-target="#org_select_dialog">
							<input type="hidden" name="orgname" value="<?php echo e(old('orgname', @$abilityData['GroupName'])); ?>">
						</td>
						<td style="padding:0px; vertical-align: middle;">
							<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val2'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
							</span>
						</td>

						<?php if($target == 'create'): ?>
						<td style="padding:0px; vertical-align: middle;">
						<span class="col-xs-1 p-1">
							<button type="button" id="select" class="<?php echo e(config('system_const.btn_color_file')); ?>" data-toggle="modal" data-target="#org_select_dialog">
								<i class="<?php echo e(config('system_const.btn_img_file')); ?>"></i><?php echo e(config('system_const.btn_char_file')); ?>

							</button>
						</span>
						</td>
						<td style="padding:0px; vertical-align: middle;">
						<span class="col-xs-1 p-1">
							<button type="button" id="clear" class="<?php echo e(config('system_const.btn_color_clear')); ?>">
								<i class="<?php echo e(config('system_const.btn_img_clear')); ?>"></i><?php echo e(config('system_const.btn_char_clear')); ?>

							</button>
						</span>
						</td>
						<?php echo $__env->make('mst/org/select', ['mstOrgCommon' => $mstOrgCommon, 'activeOrgID' => 0], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						<?php endif; ?>
					</tr>
					<tr>
						<td>施工棟：</td>
						<td>
							<?php if($target == 'create' || $target == 'edit'): ?>
							<select name="val3" <?php echo e($target == "show" || $target == "edit" ? 'disabled="disabled"' : ''); ?>>
								<?php if($target == 'create'): ?>
								<option value="" selected></option>
								<?php $__currentLoopData = $Floor; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $value): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
								<option value="<?php echo e($value['Code']); ?>" <?php echo e((is_null(old('val3')) ? @$abilityData['FloorCode'] : old('val3')) == $value['Code'] ? 'selected' : ''); ?>><?php echo e($value['Name']); ?></option>
								<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
								<?php endif; ?>

								<?php if($target == 'edit'): ?>
								<option value="<?php echo e($abilityData['FloorCode']); ?>"><?php echo e($abilityData['FloorName']); ?></option>
								<?php endif; ?>
							</select>
							<?php if($target == "edit"): ?>
							<input type="hidden" name="val3" value="<?php echo e(old('val3', @$abilityData['FloorCode'])); ?>">
							<?php endif; ?>

							<?php elseif($target == 'show'): ?>
							<input type="text" name="val3" value="<?php echo e($abilityData['FloorCode']); ?>" disabled="disabled">
							<?php endif; ?>
						</td>
						<td style="padding:0px; vertical-align: middle;">
							<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val3'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
							</span>
						</td>

						<td>職種：</td>
						<td>
							<?php if($target == 'create' || $target == 'edit'): ?>
							<select name="val4" <?php echo e($target == "show" || $target == "edit" ? 'disabled="disabled"' : ''); ?>>
								<?php if($target == 'create'): ?>
								<option value="" selected></option>
								<?php $__currentLoopData = $Dist; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $value): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
								<option value="<?php echo e($value['Code']); ?>" <?php echo e((is_null(old('val4')) ? @$abilityData['DistCode'] : old('val4').(mb_strlen(old('val4')) <= 5 ? str_repeat(' ', 5 - mb_strlen(old('val4'))) : '')) == $value['Code'] ? 'selected' : ''); ?>><?php echo e($value['Name']); ?></option>
								<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
								<?php endif; ?>

								<?php if($target == 'edit'): ?>
								<option value="<?php echo e($abilityData['DistCode']); ?>"><?php echo e($abilityData['DistName']); ?></option>
								<?php endif; ?>
							</select>
							<?php if($target == "edit"): ?>
							<input type="hidden" name="val4" value="<?php echo e(old('val4', @$abilityData['DistCode'])); ?>">
							<?php endif; ?>

							<?php elseif($target == 'show'): ?>
							<input type="text" name="val4" value="<?php echo e($abilityData['DistCode']); ?>" disabled="disabled">
							<?php endif; ?>
						</td>
						<td style="padding:0px; vertical-align: middle;">
							<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val4'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
							</span>
						</td>
					</tr>
					<tr>
						<td>開始日*：</td>
						<td>
							<input type="text" name="val5" maxlength="10" value="<?php echo e(old('val5', @$abilityData['Sdate'])); ?>" <?php echo e($target == "show" ? 'disabled="disabled"' : ''); ?> class="selectdate" autocomplete="off">
						</td>
						<td style="padding:0px; vertical-align: middle;">
							<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val5'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
							</span>
						</td>

						<td>終了日：</td>
						<td>
							<input type="text" name="val6" maxlength="10" value="<?php echo e(old('val6', @$abilityData['Edate'])); ?>" <?php echo e($target == "show" ? 'disabled="disabled"' : ''); ?> class="selectdate" autocomplete="off">
						</td>
						<td style="padding:0px; vertical-align: middle;">
							<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val6'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
							</span>
						</td>
					</tr>
					<tr>
						<td>工数：</td>
						<td>
							<input type="text" name="val7" maxlength="9" value="<?php echo e(old('val7', @$abilityData['Hr'])); ?>" <?php echo e($target == "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off">
						</td>
						<td style="padding:0px; vertical-align: middle;">
							<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val7'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
							</span>
						</td>
					</tr>
				</tbody>
			</table>
		</form>

		<div class="row">
			<?php if($target == 'create' || $target == 'edit'): ?>
			<div class="col-xs-1 p-1">
				<button type="button" id="save" class="<?php echo e(config('system_const.btn_color_save')); ?>">
					<i class="<?php echo e(config('system_const.btn_img_save')); ?>"></i><?php echo e(config('system_const.btn_char_save')); ?>

				</button>
			</div>
			<?php endif; ?>
			<div class="col-xs-1 p-1">
				<button type="button" id="cancel" class="<?php echo e(config('system_const.btn_color_cancel')); ?>">
					<i class="<?php echo e(config('system_const.btn_img_cancel')); ?>"></i><?php echo e(config('system_const.btn_char_cancel')); ?>

				</button>
			</div>
		</div>
	</div>
</div><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/mst/ability/contents.blade.php ENDPATH**/ ?>