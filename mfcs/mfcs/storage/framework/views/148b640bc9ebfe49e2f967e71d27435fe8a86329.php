<?php $__env->startSection('content'); ?>
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();
		var dataVal1 = fncJsonParse('<?php echo e(json_encode($val1_All)); ?>');
		var dataVal2 = fncJsonParse('<?php echo e(json_encode($val2_All)); ?>');
		var dataVal3 = fncJsonParse('<?php echo e(json_encode($val3_All)); ?>');

		var val1 = $(".val1 option:selected").val();
		var val2Selected = '<?php echo e(isset($request->val2) ? $request->val2 : ""); ?>';
		bindingSelect('val2', dataVal2, val1, val2Selected);

		var val2 = $(".val2 option:selected").val();
		var val3Selected = '<?php echo e(isset($request->val3) ? $request->val3 : ""); ?>';
		bindingSelect('val3', dataVal3, val2, val3Selected);

		$('[name=val1]').on('change', function(e) {
			bindingSelect('val2', dataVal2, $(this).val());
			var val2 = $(".val2 option:selected").val();
			bindingSelect('val3', dataVal3, val2);
		});

		$('[name=val2]').on('change', function(e) {
			bindingSelect('val3', dataVal3, $(this).val());
		});

		$('#save').on('click', function(e) {
			$('#indicator').trigger('click');
			$('#mainform').submit();
		});

		$('.input-checkbox').off().click(function(){
			if($(this).prop('checked')){
				$('[name="'+$(this).attr('checkbox')+'"]').val('<?php echo e(valueUrlEncode(1)); ?>');
			}else{
				$('[name="'+$(this).attr('checkbox')+'"]').val('<?php echo e(valueUrlEncode(0)); ?>');
			}
		});

		$('.selectdate').datepicker();

		function bindingSelect(name_input, data, filterKey, itemSelected) {
			$('#indicator').trigger('click');
			if(['val2', 'val3'].indexOf(name_input) > -1) {
				let arrUnique = [];
				$('[name='+name_input+']').empty();
				if(data.length > 0) {
					let flagHasValue = false;
					if(name_input == 'val2') {
						$.each(data, function(i, e) {
							if(filterKey == e.SummaryType) {
								if(arrUnique.length == 0) {
									flagHasValue = true;
									if(e.val2 == itemSelected) {
										$('[name=val2]').append(`<option selected value="${e.val2}">${convertHTML(e.val2Name)}</option>`);
									}else {
										$('[name=val2]').append(`<option value="${e.val2}">${convertHTML(e.val2Name)}</option>`);
									}
									arrUnique.push(e.val2Name);
								} else {
									if(arrUnique.indexOf(e.val2Name) === -1) {
										flagHasValue = true;
										if(e.val2 == itemSelected) {
											$('[name=val2]').append(`<option selected value="${e.val2}">${convertHTML(e.val2Name)}</option>`);
										}else {
											$('[name=val2]').append(`<option value="${e.val2}">${convertHTML(e.val2Name)}</option>`);
										}
										arrUnique.push(e.val2Name);
									}
								}
							}
						});
						if(!flagHasValue) { $('[name='+name_input+']').append('<option value=""></option>'); }
					}
					else if(name_input == 'val3') {
						$.each(data, function(i, e) {
							if(filterKey == e.SummaryID) {
								if(arrUnique.length == 0) {
									flagHasValue = true;
									if(e.val3 == itemSelected) {
										$('[name=val3]').append(`<option selected value="${e.val3}">${convertHTML(e.val3Name)}</option>`);
									}else {
										$('[name=val3]').append(`<option value="${e.val3}">${convertHTML(e.val3Name)}</option>`);
									}
									arrUnique.push(e.val3Name);
								} else {
									if(arrUnique.indexOf(e.val3Name) === -1) {
										flagHasValue = true;
										if(e.val3 == itemSelected) {
											$('[name=val3]').append(`<option selected value="${e.val3}">${convertHTML(e.val3Name)}</option>`);
										}else {
											$('[name=val3]').append(`<option value="${e.val3}">${convertHTML(e.val3Name)}</option>`);
										}
										arrUnique.push(e.val3Name);
									}
								}
							}
						});
						if(!flagHasValue) { 
							$('[name='+name_input+']').append('<option value=""></option>'); 
							$('#checkbox_val5, #checkbox_val6').html('');
						}else {
							$('#checkbox_val5').html('').append(`
								<input type="checkbox" class="input-checkbox" <?php echo e(isset($request->val5) && $request->val5 == valueUrlEncode(config('system_const_report.summary_code_subtotal')) ? 'checked' : ''); ?> checkbox="val5"/> 
								<?php echo e(config('system_const_report.summary_name_subtotal')); ?>

								<input type="hidden" name="val5" value="<?php echo e(isset($request->val5) ? $request->val5 : valueUrlEncode(0)); ?>">
							`);
							$('#checkbox_val6').html('').append(`
								<input type="checkbox" class="input-checkbox" <?php echo e(isset($request->val6) && $request->val6 == valueUrlEncode(config('system_const_report.summary_code_total')) ? 'checked' : ''); ?> checkbox="val6"/> 
								<?php echo e(config("system_const_report.summary_name_total")); ?>

								<input type="hidden" name="val6" value="<?php echo e(isset($request->val6) ? $request->val6 : valueUrlEncode(0)); ?>">
							`); 

							$('.input-checkbox').off().click(function(){
								if($(this).prop('checked')){
									$('[name="'+$(this).attr('checkbox')+'"]').val('<?php echo e(valueUrlEncode(1)); ?>');
								}else{
									$('[name="'+$(this).attr('checkbox')+'"]').val('<?php echo e(valueUrlEncode(0)); ?>');
								}
							});
						}
					}
					
				}
				else 
				{
					if(name_input == 'val2') {
						$('[name='+name_input+']').append('<option value=""></option>');
					}

					if(name_input == 'val3') {
						$('#checkbox_val5, #checkbox_val6').html('');
					}
				}
			}
			indicatorHide();
		}
	});
</script>
<div class="row ml-2 mr-2">
	<div class="col-md-12 col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　汎用集計表
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
		<form action="<?php echo e(url('/')); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/settings" method="POST" id="mainform" enctype="multipart/form-data">
			<?php echo csrf_field(); ?>
			<input type="hidden" id="kindid" name="cmn1" value="<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>" />
			<input type="hidden" id="menuid" name="cmn2" value="<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>" />

			<div class="row ml-1 mt-3">
				<div class="col-xs-12">
					<table class="table table-borderless mb-0">
						<tr>
							<td class="td-mw-108 align-middle">集計表タイプ：</td>
							<td>
								<select name="val1" class="val1" id="">
								<?php if(count($val1_All) > 0): ?>
									<?php $__currentLoopData = $val1_All; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $item): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
											<option <?php echo e(trim(old('val1', @$itemShow['val1'])) === trim($item->val1) ? 'selected' : ''); ?> value="<?php echo e($item->val1); ?>">
												<?php echo e($item->val1Name); ?>

											</option>
									<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
								<?php else: ?>
									<option value=""></option>
								<?php endif; ?>
								</select>
							</td>
							<td class="p-0 align-middle">
								<span class="col-xs-1 p-1">
								<?php echo $__env->make('layouts/error/item', ['name' => 'val1'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
								</span>
							</td>
						</tr>
						<tr>
							<td class="td-mw-108 align-middle">集計表名：</td>
							<td>
								<select name="val2" class="val2" id="">

								</select>
							</td>
							<td class="p-0 align-middle">
								<span class="col-xs-1 p-1">
								<?php echo $__env->make('layouts/error/item', ['name' => 'val2'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
								</span>
							</td>
						</tr>
						<tr>
							<td class="td-mw-108 align-middle">条件項目選択：</td>
							<td>
								<select class="form-select h-auto val3" multiple name="val3">

								</select>
							</td>
							<td class="p-0 align-middle">
								<span class="col-xs-1 p-1">
								<?php echo $__env->make('layouts/error/item', ['name' => 'val3'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
								</span>
							</td>
						</tr>
						<tr>
							<td class="td-mw-108 align-middle">職制基準日（条件選択）：</td>
							<td>
								<input type="text" name="val4" class="selectdate" maxlength="10" value="<?php echo e(old('val4', @$itemShow['val4'])); ?>"/>
							</td>
							<td class="p-0 align-middle">
								<span class="col-xs-1 p-1">
								<?php echo $__env->make('layouts/error/item', ['name' => 'val4'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
								</span>
							</td>
						</tr>
					</table>
				</div>
			</div>
			<div class="row ml-1">
				<div class="col-xs-12">
					<table class="table table-borderless mb-0">
						<tr>
							<td id="checkbox_val5">
								
							</td>
							<td class="p-0 align-middle">
								<span class="col-xs-1 p-1">
								<?php echo $__env->make('layouts/error/item', ['name' => 'val5'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
								</span>
							</td>
							<td id="checkbox_val6">
								
							</td>
							<td class="p-0 align-middle">
								<span class="col-xs-1 p-1">
								<?php echo $__env->make('layouts/error/item', ['name' => 'val6'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
								</span>
							</td>
						</tr>
					</table>
				</div>
			</div>
		</form>
		<div class="row">
			<div class="col-sm-12">
				<div class="col-xs-1 p-1">
					<button type="button" id="save" class="<?php echo e(config('system_const.btn_color_ok')); ?>"><i class="<?php echo e(config('system_const.btn_img_ok')); ?>"></i><?php echo e(config('system_const.btn_char_ok')); ?></button>
				</div>
			</div>
		</div>
	</div>
</div>
<?php $__env->stopSection(); ?>
<?php echo $__env->make('layouts/mainmenu/menu', \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/Report/summary/index.blade.php ENDPATH**/ ?>