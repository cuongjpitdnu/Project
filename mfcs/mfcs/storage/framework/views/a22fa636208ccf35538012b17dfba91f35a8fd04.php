<?php $__env->startSection('content'); ?>
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();

		const dataVal2 = fncJsonParse('<?php echo e(json_encode($dataSelect['val2LoadAll'])); ?>');
		const dataVal3 = fncJsonParse('<?php echo e(json_encode($dataSelect['val3LoadAll'])); ?>');
		const dataVal4 = fncJsonParse('<?php echo e(json_encode($dataSelect['val4LoadAll'])); ?>');
		const dataVal5 = fncJsonParse('<?php echo e(json_encode($dataSelect['val5LoadAll'])); ?>');

		$('[name=val1]').on('change', function(e) {
			bindingValue('val2', dataVal2, $(this).val(), '', '', '');
			bindingValue('val3', dataVal3, $(this).val(), $('[name=val2]').val(), '', '');
			bindingValue('val4', dataVal4, $(this).val(), $('[name=val2]').val(), $('[name=val3]').val(), '');
			bindingValue('val5', dataVal5, $(this).val(), $('[name=val2]').val(), $('[name=val3]').val(), $('[name=val4]').val());
		});

		$('[name=val2]').on('change', function(e) {
			bindingValue('val3', dataVal3, $('[name=val1]:checked').val(), $(this).val(), '', '');
			bindingValue('val4', dataVal4, $('[name=val1]:checked').val(), $(this).val(), $('[name=val3]').val(), '');
			bindingValue('val5', dataVal5, $('[name=val1]:checked').val(), $(this).val(), $('[name=val3]').val(), $('[name=val4]').val());
		});

		$('[name=val3]').on('change', function(e) {
			bindingValue('val4', dataVal4, $('[name=val1]:checked').val(), $('[name=val2]').val(), $(this).val(), '');
			bindingValue('val5', dataVal5, $('[name=val1]:checked').val(), $('[name=val2]').val(), $(this).val(), $('[name=val4]').val());
		});

		$('[name=val4]').on('change', function(e) {
			bindingValue('val5', dataVal5, $('[name=val1]:checked').val(), $('[name=val2]').val(), $('[name=val3]').val(), $(this).val());
		});

		$('#ok').on('click', function(e) {
			$('#indicator').trigger('click');
			$('#mainform').submit();
		});
	});

	function bindingValue(name_input, data, ckind_filter, project_filter, order_filter, kotei_filter) {
		$('#indicator').trigger('click');
		if(['val2', 'val3', 'val4', 'val5'].indexOf(name_input) > -1) {
			let arrUnique = [];
			$('[name='+name_input+']').empty();
			if(data.length > 0) {
				let flagHasValue = false;
				if(name_input === 'val2') {
					$.each(data, function(i, e) {
						if(ckind_filter === e.ListKind) {
							if(arrUnique.length === 0) {
								flagHasValue = true;
								$('[name=val2]').append(`<option value="${e.ID}">${convertHTML(e.ProjectName)}</option>`);
								arrUnique.push(e.ProjectName);
							} else {
								if(arrUnique.indexOf(e.ProjectName) === -1) {
									flagHasValue = true;
									$('[name=val2]').append(`<option value="${e.ID}">${convertHTML(e.ProjectName)}</option>`);
									arrUnique.push(e.ProjectName);
								}
							}
						}
					});
				}
				if(name_input === 'val3') {
					$.each(data, function(i, e) {
						if(ckind_filter === e.CKind && project_filter === e.ProjectID) {
							if(arrUnique.length === 0) {
								flagHasValue = true;
								$('[name=val3]').append(`<option value="${e.OrderNo}">${convertHTML(e.NameShow)}</option>`);
								arrUnique.push(e.NameShow);
							} else {
								if(arrUnique.indexOf(e.NameShow) === -1) {
									flagHasValue = true;
									$('[name=val3]').append(`<option value="${e.OrderNo}">${convertHTML(e.NameShow)}</option>`);
									arrUnique.push(e.NameShow);
								}
							}
						}
					});
				}
				if(name_input === 'val4') {
					$.each(data, function(i, e) {
						if(ckind_filter === e.CKind && project_filter === e.ProjectID && order_filter === e.OrderNo) {
							if(arrUnique.length === 0) {
								flagHasValue = true;
								$('[name=val4]').append(`<option value="${e.Code}">${convertHTML(e.Name)}</option>`);
								arrUnique.push(e.Name);
							} else {
								if(arrUnique.indexOf(e.Name) === -1) {
									flagHasValue = true;
									$('[name=val4]').append(`<option value="${e.Code}">${convertHTML(e.Name)}</option>`);
									arrUnique.push(e.Name);
								}
							}
						}
					});
				}
				if(name_input === 'val5') {
					$.each(data, function(i, e) {
						if(ckind_filter === e.CKind && project_filter === e.ProjectID && order_filter === e.OrderNo && kotei_filter === e.Kotei) {
							if(arrUnique.length === 0) {
								flagHasValue = true;
								$('[name=val5]').append(`<option value="${e.KKumiku}">${convertHTML(e.NameShow)}</option>`);
								arrUnique.push(e.NameShow);
							} else {
								if(arrUnique.indexOf(e.NameShow) === -1) {
									flagHasValue = true;
									$('[name=val5]').append(`<option value="${e.KKumiku}">${convertHTML(e.NameShow)}</option>`);
									arrUnique.push(e.NameShow);
								}
							}
						}
					});
				}
				if(!flagHasValue) { $('[name='+name_input+']').append('<option value=""></option>'); }
			} else {
				$('[name='+name_input+']').append('<option value=""></option>');
			}
		}
		indicatorHide();
	}
</script>

<div class="row ml-2 mr-2">
	<div class="col-md-12 col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　中日程再計算
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

		<form action="<?php echo e(url('/')); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/recalc" method="POST" id="mainform">
			<?php echo csrf_field(); ?>
			<input type="hidden" id="kindid" name="cmn1" value="<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>" />
			<input type="hidden" id="menuid" name="cmn2" value="<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>" />
			<div class="row head-purple">
				<div class="col-xs-12">中日程選択</div>
			</div>

			<div class="row ml-1 mt-3">
				<div class="col-xs-12">
					<table class="table table-borderless mb-0">
						<tbody>
							<tr>
								<td>中日程区分：</td>
								<td>
									<label for="rdo1">
										<input type="radio" id="rdo1" name="val1" value="<?php echo e(valueUrlEncode(config('system_const.c_kind_chijyo'))); ?>"
										<?php echo e((old('val1', @$itemData['val1']) === valueUrlEncode(config('system_const.c_kind_chijyo'))) ? 'checked' : ''); ?>> <?php echo e(config('system_const.c_name_chijyo')); ?></label> /
									<label for="rdo2">
										<input type="radio" id="rdo2" name="val1" value="<?php echo e(valueUrlEncode(config('system_const.c_kind_gaigyo'))); ?>"
										<?php echo e((old('val1', @$itemData['val1']) === valueUrlEncode(config('system_const.c_kind_gaigyo'))) ? 'checked' : ''); ?>> <?php echo e(config('system_const.c_name_gaigyo')); ?></label> /
									<label for="rdo3">
										<input type="radio" id="rdo3" name="val1" value="<?php echo e(valueUrlEncode(config('system_const.c_kind_giso'))); ?>"
										<?php echo e((old('val1', @$itemData['val1']) === valueUrlEncode(config('system_const.c_kind_giso'))) ? 'checked' : ''); ?>> <?php echo e(config('system_const.c_name_giso')); ?></label>
								</td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										<?php echo $__env->make('layouts/error/item', ['name' => 'val1'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
									</span>
								</td>
							</tr>
						</tbody>
					</table>
				</div>
			</div>
			<div class="row ml-1">
				<div class="col-xs-12">
					<table class="table table-borderless">
						<tbody>
							<tr>
								<td class="align-middle">検討ケース：</td>
								<td>
									<select name="val2" id="">
										<?php if(count($dataSelect['val2']) > 0): ?>
											<?php $__currentLoopData = $dataSelect['val2']; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $item): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
												<option value=<?php echo e($item->ID); ?>

													<?php echo e(trim(old('val2', @$itemData['val2'])) === trim($item->ID) ? 'selected': ''); ?>><?php echo e($item->ProjectName); ?></option>
											<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
										<?php else: ?>
											<option value=""></option>
										<?php endif; ?>
									</select>
								</td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										<?php echo $__env->make('layouts/error/item', ['name' => 'val2'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
									</span>
								</td>
								<td class="td-mw-108 align-middle">オーダ：</td>
								<td>
									<select name="val3" id="">
										<?php if(count($dataSelect['val3']) > 0): ?>
											<?php $__currentLoopData = $dataSelect['val3']; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $item): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
												<option value=<?php echo e($item->OrderNo); ?>

													<?php echo e(trim(old('val3', @$itemData['val3'])) === trim($item->OrderNo) ? 'selected': ''); ?>><?php echo e($item->NameShow); ?></option>
											<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
										<?php else: ?>
											<option value=""></option>
										<?php endif; ?>
									</select>
								</td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										<?php echo $__env->make('layouts/error/item', ['name' => 'val3'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
									</span>
								</td>
							</tr>
						</tbody>
					</table>
				</div>
			</div>
			<div class="row head-purple">
				<div class="col-xs-12">キー工程選択</div>
			</div>
			<div class="row ml-1 mt-3">
				<div class="col-xs-12">
					<table class="table table-borderless">
						<tbody>
							<tr>
								<td class="td-mw-108 align-middle">工程：</td>
								<td>
									<select name="val4" id="">
										<?php if(count($dataSelect['val4']) > 0): ?>
											<?php $__currentLoopData = $dataSelect['val4']; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $item): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
												<option value=<?php echo e($item->Code); ?>

													<?php echo e(trim(old('val4', @$itemData['val4'])) === trim($item->Code) ? 'selected': ''); ?>><?php echo e($item->Name); ?></option>
											<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
										<?php else: ?>
											<option value=""></option>
										<?php endif; ?>
									</select>
								</td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										<?php echo $__env->make('layouts/error/item', ['name' => 'val4'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
									</span>
								</td>
								<td class="td-mw-108 align-middle">工程組区：</td>
								<td>
									<select name="val5" id="">
										<?php if(count($dataSelect['val5']) > 0): ?>
											<?php $__currentLoopData = $dataSelect['val5']; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $item): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
												<option value=<?php echo e($item->KKumiku); ?>

													<?php echo e(trim(old('val5', @$itemData['val5'])) === trim($item->KKumiku) ? 'selected': ''); ?>><?php echo e($item->NameShow); ?></option>
											<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
										<?php else: ?>
											<option value=""></option>
										<?php endif; ?>
									</select>
								</td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										<?php echo $__env->make('layouts/error/item', ['name' => 'val5'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
									</span>
								</td>
							</tr>
						</tbody>
					</table>
				</div>
			</div>
		</form>
		<div class="row ml-1">
			<div class="col-xs-1 p-1">
				<button type="button" id="ok" class="<?php echo e(config('system_const.btn_color_ok')); ?>">
					<i class="<?php echo e(config('system_const.btn_img_ok')); ?>"></i><?php echo e(config('system_const.btn_char_ok')); ?>

				</button>
			</div>
		</div>
	</div>
</div>

<?php $__env->stopSection(); ?>
<?php echo $__env->make('layouts/mainmenu/menu', \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/Schem/recalc/index.blade.php ENDPATH**/ ?>