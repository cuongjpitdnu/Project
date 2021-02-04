<?php $__env->startSection('content'); ?>
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();
		const dataVal2 = fncJsonParse('<?php echo e(json_encode($dataView['data_2_all'])); ?>');
		const dataVal3 = fncJsonParse('<?php echo e(json_encode($dataView['data_3_all'])); ?>');

		$('[name=val2]').on('change', function(e) {
			bindingSelect('val3', dataVal3, $(this).val());
		});

		// set disabled button,selector
		$('input:radio[name="val1"]').change(function() {
			var value = $("input:radio[name=val1]:checked").val();
			//export
			if (value == '<?php echo e(valueUrlEncode(1)); ?>') {
				$("#val5").val('');
				$(".val5").attr('disabled', true);
				$("[name=val6]").attr('disabled', true);
				$("#select").prop('disabled', true);
			}
			//import
			else if (value == '<?php echo e(valueUrlEncode(0)); ?>' ) {
				$(".val5").attr('disabled', false);
				$("[name=val6]").attr('disabled', false);
				$("#select").prop('disabled', false);
			} 
		});
		//button click file
		$('#select').click(function(){
			$('#val5').click();
		});

		$('#val5').change(function (e) {
			if(e.target.files.length == 0) {
				$('#filename').val("");
				return;
			}

			var fileName = e.target.files[0].name;
			$('#filename').val(fileName);
		});

		$('#save').on('click', function(e) {
			var value = $("input:radio[name=val1]:checked").val();
			//export
			if (value == '<?php echo e(valueUrlEncode(1)); ?>') {
				$('#indicator').trigger('click');
				setCookie('export', 0, <?php echo e(config('system_const.timeout_time')); ?>);
			}
			$('#mainform').submit();
			//export
			if (value == '<?php echo e(valueUrlEncode(1)); ?>') {
				let interval = setInterval(function() {
					let checkCookie = getCookie("export");
					if(checkCookie == "") {
						indicatorHide();
						clearInterval(interval);
						var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/';
						url += 'index?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
						url += '&val1='+$('[name=val1]:checked').val();
						url += '&val2='+$('[name=val3]').val();
						url += '&val3='+$('[name=val3]').val();
						url += '&val1='+$('[name=val4]:checked').val();
						url += '&val5='+$('[name=val5]').val();
						url += '&val6='+$('[name=val6]').val();
						url += '&err1=<?php echo e($msgTimeOut); ?>';
						window.location.href = url;
					}
					if(checkCookie == 1) {
						indicatorHide();
						clearInterval(interval);
					}
				}, 100);
			}
		});
	});

	function bindingSelect(name_input, data, project_filter) {
		$('#indicator').trigger('click');
		if(['val2', 'val3'].indexOf(name_input) > -1) {
			let arrUnique = [];
			$('[name='+name_input+']').empty();
			if(data.length > 0) {
				let flagHasValue = false;
				if(name_input == 'val2') {
					$.each(data, function(i, e) {
						if(arrUnique.length == 0) {
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
					});
				}
				else if(name_input == 'val3') {
					$.each(data, function(i, e) {
						if(project_filter == e.ProjectID) {
							if(arrUnique.length == 0) {
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
				}
				if(!flagHasValue) { $('[name='+name_input+']').append('<option value=""></option>'); }
			}
			else 
			{
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
				■　日程Import/Export
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
		<form action="<?php echo e(url('/')); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/execute" method="POST" id="mainform" enctype="multipart/form-data">
			<?php echo csrf_field(); ?>
			<input type="hidden" id="kindid" name="cmn1" value="<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>" />
			<input type="hidden" id="menuid" name="cmn2" value="<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>" />
			<div class="row head-purple">
				<div class="col-xs-12">条件選択</div>
			</div>

			<div class="row ml-1 mt-3">
				<div class="col-xs-12">
					<table class="table table-borderless mb-0">
						<tr>
							<td class="td-mw-108 align-middle">機能選択：</td>
							<td>
								<label>
									<input type="radio"  name="val1" class="val1-import" 
									value="<?php echo e(valueUrlEncode(config('system_const_schem.bd_val_import'))); ?>" 
									<?php echo e(trim(old('val1', @$itemShow['val1'])) === valueUrlEncode(config('system_const_schem.bd_val_import')) ? 'checked' : ''); ?>> 
									Import
								</label> /
								<label>
									<input type="radio"  name="val1" class="val1-export" 
									value="<?php echo e(valueUrlEncode(config('system_const_schem.bd_val_export'))); ?>" 
									<?php echo e(trim(old('val1', @$itemShow['val1'])) === valueUrlEncode(config('system_const_schem.bd_val_export')) ? 'checked' : ''); ?>> 
									Export
								</label>
							</td>
							<td class="p-0 align-middle">
								<span class="col-xs-1 p-1">
								<?php echo $__env->make('layouts/error/item', ['name' => 'val1'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
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
							<td class="td-mw-108 align-middle">検討ケース：</td>
							<td>
								<select name="val2" id="">
									<?php if(count($dataView['data_2_all']) > 0): ?>
										<?php $__currentLoopData = $dataView['data_2_all']; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $item): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
											<option value=<?php echo e($item->val2); ?>

												<?php echo e(trim(old('val2', @$itemShow['val2'])) === trim(($item->val2)) ? 'selected': ''); ?>>
												<?php echo e($item->val2Name); ?>

											</option>
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
						</tr>
						<tr>
							<td class="td-mw-108 align-middle">オーダ：</td>
							<td>
								<select name="val3" id="">
									<?php if(count($dataView['data_3']) > 0): ?>
										<?php $__currentLoopData = $dataView['data_3']; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $item): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
											<option value=<?php echo e($item->val3); ?>

												<?php echo e(trim(old('val3', @$itemShow['val3'])) === trim($item->val3) ? 'selected': ''); ?>>
												<?php echo e($item->val3Name); ?>

											</option>
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
					</table>
				</div>
			</div>
			<div class="row ml-1">
				<div class="col-xs-12">
					<table class="table table-borderless mt-0">
						<tr>
							<td class="td-mw-108 align-middle">データ区分：</td>
							<td>
								<label>
									<input type="radio"  name="val4"
									value="<?php echo e(valueUrlEncode(0)); ?>" 
									<?php echo e(trim(old('val4', @$itemShow['val4'])) === valueUrlEncode(0) ? 'checked' : ''); ?>> 
									中日程からの展開データ
								</label> /
								<label>
									<input type="radio"  name="val4"
									value="<?php echo e(valueUrlEncode(1)); ?>" 
									<?php echo e(trim(old('val4', @$itemShow['val4'])) === valueUrlEncode(1) ? 'checked' : ''); ?>> 
									オリジナルデータ
								</label>
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
			<div class="row head-purple">
				<div class="col-xs-12">ファイル選択</div>
			</div>
			<div class="row ml-1 mt-3">
				<div class="col-xs-12">
					<table class="table table-borderless">
						<?php if(!$menuInfo->IsReadOnly): ?>
							<tr>
								<td class="align-middle">
									<input type="file" name="val5" id="val5" class="val5 d-none" 
									<?php echo e(trim(old('val1', @$itemShow['val1'])) === valueUrlEncode(config('system_const_schem.bd_val_export')) ? 'disabled' : ''); ?>

									value="<?php echo e(@$itemData->val5); ?>" required="true">
									<input type="text" id="filename" class="val5 input-file-width"
									<?php echo e(trim(old('val1', @$itemShow['val1'])) === valueUrlEncode(config('system_const_schem.bd_val_export')) ? 'disabled' : ''); ?>

									value= "<?php echo e(@$itemData->filename); ?>" autocomplete="off" readonly />
								</td>

								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
									<?php echo $__env->make('layouts/error/item', ['name' => 'val5'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
									</span>
								</td>

								<td class="align-middle">
									<button type="button" name="select" id="select" 
									<?php echo e(trim(old('val1', @$itemShow['val1'])) === valueUrlEncode(config('system_const_schem.bd_val_export')) ? 'disabled' : ''); ?>

									class="<?php echo e(config('system_const.btn_color_file')); ?> val5">
										<i class="<?php echo e(config('system_const.btn_img_file')); ?>"></i>
										<?php echo e(config('system_const.btn_char_file')); ?>

									</button>
								</td>
							</tr>
						<?php endif; ?>
					</table>
				</div>
			</div>
			<div class="row head-purple">
				<div class="col-xs-12">ログ表示設定</div>
			</div>
			<div class="row ml-1 mt-3">
				<div class="col-xs-12">
					<table class="table table-borderless">
						<?php if(!$menuInfo->IsReadOnly): ?>
							<tr>
								<td class="td-mw-108 align-middle">表示件数：</td>
								<td>
									<select name="val6" class="pageunit-width"
									<?php echo e(trim(old('val1', @$itemShow['val1'])) === valueUrlEncode(config('system_const_schem.bd_val_export')) ? 'disabled' : ''); ?>>
										<option value="<?php echo e(valueUrlEncode(config('system_const.displayed_results_1'))); ?>" 
											<?php echo e(trim(old('val6',@$itemShow['val6'])) === trim(valueUrlEncode(config('system_const.displayed_results_1'))) ? 'selected' : ''); ?>>
											<?php echo e(config('system_const.displayed_results_1')); ?>

										</option>
										<option value="<?php echo e(valueUrlEncode(config('system_const.displayed_results_2'))); ?>" 
											<?php echo e(trim(old('val6',@$itemShow['val6'])) === trim(valueUrlEncode(config('system_const.displayed_results_2'))) ? 'selected' : ''); ?>>
											<?php echo e(config('system_const.displayed_results_2')); ?>

										</option>
										<option value="<?php echo e(valueUrlEncode(config('system_const.displayed_results_3'))); ?>" 
											<?php echo e(trim(old('val6',@$itemShow['val6'])) === trim(valueUrlEncode(config('system_const.displayed_results_3'))) ? 'selected' : ''); ?>>
											<?php echo e(config('system_const.displayed_results_3')); ?>

										</option>
									</select>
								</td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
									<?php echo $__env->make('layouts/error/item', ['name' => 'val6'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
									</span>
								</td>
								<td class="align-middle"> ※1ページあたり </td>
							</tr>
						<?php endif; ?>
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
<?php echo $__env->make('layouts/mainmenu/menu', \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/Sches/makenittei/index.blade.php ENDPATH**/ ?>