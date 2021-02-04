<script>
$(function(){
	$('[data-toggle="tooltip"]').tooltip();

	$('#save').on('click', function(){
		$('#indicator').trigger('click');

		if($('select[name=tempVal107]').is(':disabled')) {
			$('input[name=val107]').val(0);
		} else {
			var tempValue = $('select[name=tempVal107]').val();
			$('input[name=val107]').val(tempValue);

		}

		$('input[name=val112]').val($('input[name=tempVal112]').val());
		$('input[name=val113]').val($('input[name=tempVal113]').val());

		$('#mainform').submit();
	});

	$('#cancel').on('click', function(){
		$('#indicator').trigger('click');
		window.location.href = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/index?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>&val1=<?php echo e($request->val1); ?>&val2=<?php echo e($request->val2); ?>&val3=<?php echo e($request->val3); ?>';
	});

	$('#orgtree')
	.on('activate_node.jstree', function (e, data) {
		var selectedID = data.node.li_attr.item_id;
		$('#select_parent_org_id').val(selectedID);
		var selectedName = data.node.li_attr.item_name;
		$('#select_parent_org_name').val(selectedName);
	}).jstree();

	$('#select_org_ok').on('click', function(){
		var val101 = $('#select_parent_org_id').val();
		$('#val101').val(val101);
		var orgName = $('#select_parent_org_name').val();
		$('[name=parent_org_name]').val(orgName);
		$('#org_select_dialog').modal('hide');
	});

	$('.clearorg').on('click', function(){
		$('[name=parent_org_name]').val("<?php echo e(config('system_const.org_root_name')); ?>");
		$('input[name="val101"]').val("<?php echo e(valueUrlEncode(0)); ?>");
	});

	$('.selectdate').datepicker();

	var arrKanren = fncJsonParse('<?php echo e(json_encode($arrKanren)); ?>');
	$('select[name=val105]').on('change', function () {
		$('select[name=tempVal107]').empty().append('<option value="0" selected="selected"></option>');
		if(this.value == 2){
			$('select[name="tempVal107"]').removeAttr('disabled', 'disabled');
			arrKanren.forEach(element => {
				var selectValue = (element.id == $('input[name=val107]').val()) ? 'selected' : '';
				$('select[name=tempVal107]').append('<option value="'+ element.id +'" '+ selectValue +'>'+  convertHTML(element.name) +'</option>');
			});
		}else{
			$('select[name="tempVal107"]').attr('disabled', 'disabled');
		}
	}).trigger('change');

	$('.input-checkbox').click(function(){
		if($(this).prop('checked')){
			$('[name="'+$(this).attr('checkbox')+'"]').val(1);
		}else{
			$('[name="'+$(this).attr('checkbox')+'"]').val(0);
		}
	});

})

</script>

<?php if($target === 'create' || $target === 'edit'): ?>
	<?php echo $__env->make('layouts/heartbeat/heartbeat', ['sysKindID' => $menuInfo->KindID, 'sysMenuID' => $menuInfo->MenuID, 'optionKey' => config('system_const.lock_option_key_general')], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
<?php endif; ?>

<input type="hidden" id="select_parent_org_id" value="<?php echo e(valueUrlEncode(@$mstOrg['val101'])); ?>">
<input type="hidden" id="select_parent_org_name" value="<?php echo e($parentName); ?>">

<div class="row ml-4">
	<div class="col-xs-12">

		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　職制マスタ<?php if($target === 'show'): ?>参照<?php elseif($target === 'create'): ?>登録<?php elseif($target === 'edit'): ?>更新<?php endif; ?>
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

		<form action="<?php echo e(url('/')); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/save" method="POST" id="mainform">
			<?php echo csrf_field(); ?>
			<input type="hidden" name="method" value="<?php echo e($target); ?>">
			<input type="hidden" id="kindid" name="cmn1" value="<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>">
			<input type="hidden" id="menuid" name="cmn2" value="<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>">
			<input type="hidden" id="activeid" name="val1" value="<?php echo e($request->val1); ?>">
			<input type="hidden" id="basedate" name="val2" value="<?php echo e($request->val2); ?>">
			<input type="hidden" id="" name="val3" value="<?php echo e($request->val3); ?>">
			<input type="hidden" id="val101" name="val101" value="<?php echo e(valueUrlEncode(@$mstOrg['val101'])); ?>">

			<?php if($target === 'edit'): ?>
			<input type="hidden" id="" name="val112" value="<?php echo e((@$mstOrg['val112'])); ?>">
			<input type="hidden" id="" name="val113" value="<?php echo e((@$mstOrg['val113'])); ?>">
			<input type="hidden" id="" name="val114" value="<?php echo e(valueUrlEncode(@$mstOrg['val114'])); ?>">
			<input type="hidden" id="" name="val115" value="<?php echo e(valueUrlEncode(@$mstOrg['val115'])); ?>">
			<?php endif; ?>

			<table class="table table-borderless">
				<tbody>
				<tr>
					<td class="align-middle">親職制 *：</td>
					<td>
						<input type="text" <?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> name="parent_org_name" 
						value="<?php echo e(old('parent_org_name', $parentName)); ?>" readonly="" tabindex="-1" data-toggle="modal" data-target="#org_select_dialog">
						<input type="hidden" name="parent_org_name" value="<?php echo e(old('parent_org_name', $parentName)); ?>">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						<?php echo $__env->make('layouts/error/item', ['name' => 'val101'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
					<?php if($target === 'create' || $target === 'edit'): ?>
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
					<?php echo $__env->make('mst/org/select', ['mstOrgCommon' => $mstOrgCommon, 'activeOrgID' => $mstOrg['val101']], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
					<?php endif; ?>

					<td class="align-middle">職制コード：</td>
					<td>
						<input type="text" name="val102" value="<?php echo e(old('val102', @$mstOrg['val102'])); ?>"
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="6">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						<?php echo $__env->make('layouts/error/item', ['name' => 'val102'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">職制名 *：</td>
					<td>
						<input type="text" name="val103" value="<?php echo e(old('val103', @$mstOrg['val103'])); ?>"
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="50">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						<?php echo $__env->make('layouts/error/item', ['name' => 'val103'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
					<?php if($target === 'create' || $target === 'edit'): ?>
					<td></td>
					<td></td>
					<?php endif; ?>
					<td class="align-middle">略称 *：</td>
					<td>
						<input type="text" name="val104" value="<?php echo e(old('val104', @$mstOrg['val104'])); ?>"
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="50">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						<?php echo $__env->make('layouts/error/item', ['name' => 'val104'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">社内外 *：</td>
					<td>
						<select name="val105" <?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?>>
							<option value="1" <?php echo e((int)old('val105', @$mstOrg['val105']) === 1 ? 'selected' : ''); ?>>社内</option>
							<option value="2" <?php echo e((int)old('val105', @$mstOrg['val105']) === 2 ? 'selected' : ''); ?>>社外</option>
						</select>
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						<?php echo $__env->make('layouts/error/item', ['name' => 'val105'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
					<?php if($target === 'create' || $target === 'edit'): ?>
					<td></td>
					<td></td>
					<?php endif; ?>
					<td class="align-middle">部内外 *：</td>
					<td>
						<select name="val106" <?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?>>
							<option value="1" <?php echo e((int)old('val106', @$mstOrg['val106']) === 1 ? 'selected' : ''); ?>>部内</option>
							<option value="2" <?php echo e((int)old('val106', @$mstOrg['val106']) === 2 ? 'selected' : ''); ?>>部外</option>
						</select>
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						<?php echo $__env->make('layouts/error/item', ['name' => 'val106'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">請負会社：</td>
					<td>
						<select name="tempVal107" <?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?>>
							
						</select>
						<input type="hidden" id="" name="val107" value="<?php echo e(old('val107', @$mstOrg['val107'])); ?>" />
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						<?php echo $__env->make('layouts/error/item', ['name' => 'val107'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
					<?php if($target === 'create' || $target === 'edit'): ?>
					<td></td>
					<td></td>
					<?php endif; ?>
					<td class="align-middle">外注班タイプ：</td>
					<td>
						<select name="val108" <?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?>>
							<option value="0" <?php echo e((int)old('val108', @$mstOrg['val108']) === 0 ? 'selected' : ''); ?>>なし</option>
							<option value="1" <?php echo e((int)old('val108', @$mstOrg['val108']) === 1 ? 'selected' : ''); ?>>貸付</option>
							<option value="2" <?php echo e((int)old('val108', @$mstOrg['val108']) === 2 ? 'selected' : ''); ?>>一括</option>
							<option value="3" <?php echo e((int)old('val108', @$mstOrg['val108']) === 3 ? 'selected' : ''); ?>>○加</option>
						</select>
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						<?php echo $__env->make('layouts/error/item', ['name' => 'val108'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">仕入先コード：</td>
					<td>
						<input type="text" name="val109" value="<?php echo e(old('val109', @$mstOrg['val109'])); ?>"
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="20">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						<?php echo $__env->make('layouts/error/item', ['name' => 'val109'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
					<?php if($target === 'create' || $target === 'edit'): ?>
					<td></td>
					<td></td>
					<?php endif; ?>
					<td class="align-middle">フォルダフラグ：</td>
					<td>
						<input type="checkbox" class="input-checkbox" checkbox="val110"
						<?php echo e((int)old('val110', @$mstOrg['val110']) === 1 ? 'checked' : ''); ?>

						<?php echo e($target === "show" || $target === "edit" ? 'disabled="disabled"' : ''); ?>> フォルダ
						<input type="hidden" name="val110" value="<?php echo e(old('val110', @$mstOrg['val110']) ? 1 : 0); ?>">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						<?php echo $__env->make('layouts/error/item', ['name' => 'val110'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
				</tr>
				<tr>
					<td class="align-middle">表示順：</td>
					<td>
						<input type="text" class="text-right" name="val111" value="<?php echo e(old('val111', @$mstOrg['val111'])); ?>"
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
						<?php echo $__env->make('layouts/error/item', ['name' => 'val111'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
					<?php if($target === 'create' || $target === 'edit'): ?>
					<td></td>
					<td></td>
					<?php endif; ?>
					<?php if($target === 'show' || $target === 'edit'): ?>
					<td class="align-middle">適用期間：</td>
					<td>
						<input class="selectdate" type="text" name="tempVal112" value="<?php echo e(old('val112', @$mstOrg['val112'])); ?>"
						<?php echo e($target === "show" || $target === 'edit' ? 'disabled="disabled"' : ''); ?> autocomplete="off" style="float:left;width:78px;">
						<span style="display: inline-block ;width:14px; float:left; margin: 4px;">～</span>
						<input class="selectdate" type="text" name="tempVal113" value="<?php echo e(old('val113', @$mstOrg['val113'])); ?>"
						<?php echo e($target === "show" || $target === 'edit' ? 'disabled="disabled"' : ''); ?> autocomplete="off" style="float:left;width:78px;">
					</td>
					<?php endif; ?>
				</tr>
				</tbody>
			</table>

		</form>

		<div class="row">
			<?php if($target === 'create' || $target === 'edit'): ?>
			<div class="col-xs-1 p-1">
				<button type="button" id="save" class="<?php echo e(config('system_const.btn_color_save')); ?>"><i class="<?php echo e(config('system_const.btn_img_save')); ?>"></i><?php echo e(config('system_const.btn_char_save')); ?></button>
			</div>
			<?php endif; ?>
			<div class="col-xs-1 p-1">
				<button type="button" id="cancel" class="<?php echo e(config('system_const.btn_color_cancel')); ?>"><i class="<?php echo e(config('system_const.btn_img_cancel')); ?>"></i><?php echo e(config('system_const.btn_char_cancel')); ?></button>
			</div>
		</div>

	</div>
</div>
<?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/mst/org/contents.blade.php ENDPATH**/ ?>