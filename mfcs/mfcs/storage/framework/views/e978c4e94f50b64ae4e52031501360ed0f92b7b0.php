<?php $__env->startSection('content'); ?>
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();

		const dataVal2 = fncJsonParse('<?php echo e(json_encode($dataView['data_2_all'])); ?>');
		const dataVal3 = fncJsonParse('<?php echo e(json_encode($dataView['data_3_all'])); ?>');

		$('[name=val1]').on('change', function(e) {
			bindingSelect('val2', dataVal2, $(this).val(), '');
			bindingSelect('val3', dataVal3, $(this).val(), $('[name=val2]').val());
		});

		$('[name=val2]').on('change', function(e) {
			bindingSelect('val3', dataVal3, $('[name=val1]:checked').val(), $(this).val());
		});

		$('#output').on('click', function(e) {
			$('#area-error, table tr td:nth-child(2) div:last-child').html('');
			$('#indicator').trigger('click');
			$('[name=val4]').val(0);
			if($('[name=val4-view]').is(':checked')) {
				$('[name=val4]').val(1);
			}

			setCookie('export', 0, <?php echo e(config('system_const.timeout_time')); ?>);
			$('#mainform').submit();
			let interval = setInterval(function() {
				let checkCookie = getCookie("export");
				if(checkCookie == "") {
					indicatorHide();
					clearInterval(interval);
					var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/';
					url += 'index?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
					url += '&val1='+$('[name=val1]:checked').val();
					url += '&val2='+$('[name=val2]').val();
					url += '&val3='+$('[name=val3]').val();
					url += '&val4='+$('[name=val4]').val();
					url += '&err1=<?php echo e($msgTimeOut); ?>';
					window.location.href = url;
				}
				if(checkCookie == 1) {
					indicatorHide();
					clearInterval(interval);
				}
			}, 100);
		});
	});

	function bindingSelect(name_input, data, ckind_filter, project_filter) {
		$('#indicator').trigger('click');
		if(['val2', 'val3'].indexOf(name_input) > -1) {
			let arrUnique = [];
			$('[name='+name_input+']').empty();
			if(data.length > 0) {
				let flagHasValue = false;
				if(name_input == 'val3') {
					$.each(data, function(i, e) {
						if(ckind_filter === e.CKind && project_filter === e.ProjectID) {
							if(arrUnique.length === 0) {
								flagHasValue = true;
								$('[name=val3]').append(`<option value="${e.val3}">${convertHTML(e.val3Name)}</option>`);
								arrUnique.push(e.val3Name);
							} else {
								if(arrUnique.indexOf(e.val3Name) === -1) {
									flagHasValue = true;
									$('[name=val3]').append(`<option value="${e.val3}">${convertHTML(e.val3Name)}</option>`);
									arrUnique.push(e.val3Name);
								}
							}
						}
					});
				} else {
					$.each(data, function(i, e) {
						if(ckind_filter === e.ListKind) {
							if(arrUnique.length === 0) {
								flagHasValue = true;
								$('[name=val2]').append(`<option value="${e.val2}">${convertHTML(e.val2Name)}</option>`);
								arrUnique.push(e.val2Name);
							} else {
								if(arrUnique.indexOf(e.val2Name) === -1) {
									flagHasValue = true;
									$('[name=val2]').append(`<option value="${e.val2}">${convertHTML(e.val2Name)}</option>`);
									arrUnique.push(e.val2Name);
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

	function setCookie(cname, cvalue, minute) {
		let d = new Date();
		let exdays = (minute*60)/(24*60*60);
		d.setTime(d.getTime() + (exdays*24*60*60*1000));
		let expires = "expires=" + d.toGMTString();
		document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
	}

	function getCookie(cname) {
		var name = cname + "=";
		var decodedCookie = decodeURIComponent(document.cookie);
		var ca = decodedCookie.split(';');
		for(var i = 0; i < ca.length; i++) {
			var c = ca[i];
			while (c.charAt(0) == ' ') {
				c = c.substring(1);
			}
			if (c.indexOf(name) == 0) {
				return c.substring(name.length, c.length);
			}
		}
		return "";
	}
</script>

<div class="row ml-2 mr-2">
	<div class="col-md-12 col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　中日程表出力
			</div>
		</div>
		<?php if(isset($originalError) && count($originalError) > 0): ?>
		<div class="row">
			<div class="col-xs-12" id="area-error">
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

		<div class="row">
			<div class="col-xs-12">
				<form action="<?php echo e(url('/')); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/output" method="POST" id="mainform">
					<?php echo csrf_field(); ?>
					<input type="hidden" id="kindid" name="cmn1" value="<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>" />
					<input type="hidden" id="menuid" name="cmn2" value="<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>" />

					<table class="table table-borderless">
						<tr>
							<td>中日程区分：</td>
							<td>
								<label for="rdo1"><input type="radio" id="rdo1" name="val1"
									value="<?php echo e(valueUrlEncode(config('system_const.c_kind_chijyo'))); ?>"
									<?php echo e(old('val1', @$itemShow['val1']) === valueUrlEncode(config('system_const.c_kind_chijyo')) ?
									'checked' : ''); ?>> 地上中日程</label> /
								<label for="rdo2"><input type="radio" id="rdo2" name="val1"
									value="<?php echo e(valueUrlEncode(config('system_const.c_kind_gaigyo'))); ?>"
									<?php echo e(old('val1', @$itemShow['val1']) === valueUrlEncode(config('system_const.c_kind_gaigyo')) ?
									'checked' : ''); ?>> 外業中日程</label> /
								<label for="rdo3"><input type="radio" id="rdo3" name="val1"
									value="<?php echo e(valueUrlEncode(config('system_const.c_kind_giso'))); ?>"
									<?php echo e(old('val1', @$itemShow['val1']) === valueUrlEncode(config('system_const.c_kind_giso')) ?
									'checked' : ''); ?>> 艤装中日程</label>
								<?php echo $__env->make('layouts/error/item', ['name' => 'val1'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
							</td>
						</tr>
						<tr>
							<td>検討ケース：</td>
							<td>
								<select name="val2" id="">
									<?php if(count($dataView['data_2']) > 0): ?>
										<?php $__currentLoopData = $dataView['data_2']; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $item): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
											<option value=<?php echo e($item->val2); ?>

												<?php echo e(trim(old('val2', @$itemShow['val2'])) === trim($item->val2) ? 'selected': ''); ?>><?php echo e($item->val2Name); ?></option>
										<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
									<?php else: ?>
										<option value=""></option>
									<?php endif; ?>
								</select>
								<?php echo $__env->make('layouts/error/item', ['name' => 'val2'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
							</td>
						</tr>
						<tr>
							<td>オーダ：</td>
							<td>
								<select name="val3" id="">
									<?php if(count($dataView['data_3']) > 0): ?>
										<?php $__currentLoopData = $dataView['data_3']; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $item): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
											<option value=<?php echo e($item->val3); ?>

												<?php echo e(trim(old('val3', @$itemShow['val3'])) === trim($item->val3) ? 'selected': ''); ?>><?php echo e($item->val3Name); ?></option>
										<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
									<?php else: ?>
										<option value=""></option>
									<?php endif; ?>
								</select>
								<?php echo $__env->make('layouts/error/item', ['name' => 'val3'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
							</td>
						</tr>
						<?php if(!$menuInfo->IsReadOnly): ?>
						<tr>
							<td></td>
							<td>
								<label for="val4-view">
									<input type="checkbox" name="val4-view" id="val4-view"
										<?php echo e((int)old('val4', @$itemShow['val4']) === 1 ? 'checked' : ''); ?>

									> 正式発行
									<?php echo $__env->make('layouts/error/item', ['name' => 'val4'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
								</label>
								<input type="hidden" name="val4" value="" />
							</td>
						</tr>
						<?php endif; ?>
					</table>
				</form>
			</div>
		</div>

		<div class="row">
			<div class="col-xs-1 p-1">
				<button type="button" id="output" class="<?php echo e(config('system_const.btn_color_output')); ?>">
					<i class="<?php echo e(config('system_const.btn_img_output')); ?>"></i><?php echo e(config('system_const.btn_char_output')); ?>

				</button>
			</div>
		</div>
	</div>
</div>
<?php $__env->stopSection(); ?>
<?php echo $__env->make('layouts/mainmenu/menu', \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/Schem/Output/index.blade.php ENDPATH**/ ?>