<?php $__env->startSection('content'); ?>
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();
			const dataVal3 = fncJsonParse('<?php echo e(json_encode($dataView['data_3_all'])); ?>');
			const dataVal4 = fncJsonParse('<?php echo e(json_encode($dataView['data_4_all'])); ?>');
			const dataVal5 = fncJsonParse('<?php echo e(json_encode($dataView['data_5_all'])); ?>');
		$('[name=val1]').on('change', function(e) {
			bindingSelect('val3', dataVal3, $(this).val(), '');
			bindingSelect('val4', dataVal4, $(this).val(), $('[name=val3]').val());
			bindingSelect('val5', dataVal5, $(this).val(), '' , '');
		});

		$('[name=val3]').on('change', function(e) {
			bindingSelect('val4', dataVal4, $('[name=val1]:checked').val(), $(this).val());
		});
		// set disabled button,selector
		$('input:radio[name="val2"]').change(function() {
			var value = $("input:radio[name=val2]:checked").val();
			//export
			if (value == '<?php echo e(valueUrlEncode(1)); ?>') {
				$("#val7").val('');
				$(".val6").attr('disabled', true);
				
				$(".val7").attr('disabled', true);
				$(".val7").val('');
				$("#select").prop('disabled', true);
				$('select[name="val5"]').attr('disabled', false);
			}
			//import
			else if (value == '<?php echo e(valueUrlEncode(0)); ?>' ) {
				$(".val6").attr('disabled', false);
				$(".val7").attr('disabled', false);
				$(".val5").val('');
				$("#select").prop('disabled', false);
				$('select[name="val5"]').attr('disabled', true);
			}
		});
		//button click file
		$('#select').click(function(){
			$('#val7').click();
		});
		$('#val7').change(function (e) {
			if(e.target.files.length == 0) {
				$('#filename').val("");
				return;
			}
			var fileName = e.target.files[0].name;
			$('#filename').val(fileName);
		});

		$('#save').on('click', function(e) {
			$('#area-error, table tr td:nth-child(2) div:last-child').html('');
			$('#indicator').trigger('click');
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
					url += '&val2='+$('[name=val2]:checked').val();
					url += '&val3='+$('[name=val3]').val();
					url += '&val4='+$('[name=val4]').val();
					url += '&val5='+$('[name=val5]').val();
					url += '&val8='+$('[name=val8]').val();
					url += '&err1=<?php echo e($msgTimeOut); ?>';
					window.location.href = url;
				}
				if(checkCookie == 1) {
					indicatorHide();
					clearInterval(interval);
				}
			}, 100);
		});

		$('.input-checkbox').click(function(){
			if($(this).prop('checked')){
				$('[name="'+$(this).attr('checkbox')+'"]').val(1);
			}else{
				$('[name="'+$(this).attr('checkbox')+'"]').val(0);
			}
		});
	});
	function bindingSelect(name_input, data, ckind_filter, project_filter) {
		$('#indicator').trigger('click');
		if(['val3', 'val4','val5'].indexOf(name_input) > -1) {
			let arrUnique = [];
			$('[name='+name_input+']').empty();
			if(data.length > 0) {
				let flagHasValue = false;
				if(name_input == 'val3') {
					$.each(data, function(i, e) {
						if(ckind_filter == e.ListKind) {
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
				else if(name_input == 'val4') {
					$.each(data, function(i, e) {
						if(ckind_filter == e.CKind && project_filter == e.ProjectID) {
							if(arrUnique.length == 0) {
								flagHasValue = true;
								$('[name=val4]').append(`<option value="${e.val4}">${convertHTML(e.val4Name)}</option>`);
								arrUnique.push(e.val4Name);
							} else {
								if(arrUnique.indexOf(e.val4Name) === -1) {
									flagHasValue = true;
									$('[name=val4]').append(`<option value="${e.val4}">${convertHTML(e.val4Name)}</option>`);
									arrUnique.push(e.val4Name);
								}
							}
						}
					});
				}
				if(name_input == 'val5'){
					$.each(data, function(i, e) {
						if(ckind_filter == e.CKind) {
							if(arrUnique.length == 0) {
								flagHasValue = true;
								$('[name=val5]').append(`<option value="${e.Code}">${convertHTML(e.val5Name)}</option>`);
								arrUnique.push(e.val5Name);
							} else {
								if(arrUnique.indexOf(e.val5Name) === -1) {
									flagHasValue = true;
									$('[name=val5]').append(`<option value="${e.Code}">${convertHTML(e.val5Name)}</option>`);
									arrUnique.push(e.val5Name);
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
		<form action="<?php echo e(url('/')); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/execute"
			 method="POST" id="mainform" enctype="multipart/form-data">
			<?php echo csrf_field(); ?>
			<input type="hidden" id="kindid" name="cmn1" value="<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>" />
			<input type="hidden" id="menuid" name="cmn2" value="<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>" />
			<div class="row head-purple">
				<div class="col-xs-12">条件選択</div>
			</div>

			<div class="row ml-1 mt-3">
				<div class="col-xs-12">
					<table class="table table-borderless">
						<tr>
							<td class="td-mw-108 align-middle">中日程区分：</td>
							<td>
								<label>
									<input type="radio" name="val1"
										   value="<?php echo e(valueUrlEncode(0)); ?>"<?php echo e(old('val1', @$itemShow['val1'])
										   === valueUrlEncode(config('system_const.c_kind_chijyo'))
											? 'checked' : ''); ?>> <?php echo e(config('system_const.c_name_chijyo')); ?>

								</label> /
								<label>
									<input type="radio" name="val1"
											value="<?php echo e(valueUrlEncode(1)); ?>" <?php echo e(old('val1', @$itemShow['val1'])
											=== valueUrlEncode(config('system_const.c_kind_gaigyo')) ? 'checked' : ''); ?>>
											<?php echo e(config('system_const.c_name_gaigyo')); ?>

								</label> /
								<label>
									<input type="radio" name="val1"
										   value="<?php echo e(valueUrlEncode(2)); ?>" <?php echo e(old('val1', @$itemShow['val1'])
										   === valueUrlEncode(config('system_const.c_kind_giso')) ? 'checked' : ''); ?>>
											<?php echo e(config('system_const.c_name_giso')); ?>

								</label>
							</td>
							<td class="p-0 align-middle">
								<span class="col-xs-1 p-1">
								<?php echo $__env->make('layouts/error/item', ['name' => 'val2'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
								</span>
							</td>
						</tr>
					
						<tr>
							<td class="td-mw-108 align-middle">機能選択：</td>
							<td>
								<label>
									<input type="radio"  name="val2" class="val2-import"
										  value="<?php echo e(valueUrlEncode(0)); ?>"  <?php echo e(trim(old('val2', @$itemShow['val2']))
										  === valueUrlEncode(0) ? 'checked' : ''); ?>>
									Import
								</label> /
								<label>
									<input type="radio" name="val2" class="val2-export"
									value="<?php echo e(valueUrlEncode(1)); ?>" <?php echo e(trim(old('val2', @$itemShow['val2']))
									=== valueUrlEncode(1) ? 'checked' : ''); ?>>
									Export
								</label>
								<?php echo $__env->make('layouts/error/item', ['name' => 'val2'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
							</td>
							<td class="p-0 align-middle">
								<span class="col-xs-1 p-1">
								<?php echo $__env->make('layouts/error/item', ['name' => 'val2'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
								</span>
							</td>
						</tr>
						<tr>
							<td class="td-mw-108 align-middle">検討ケース：</td>
							<td>
								<select name="val3">
									<?php if(count($dataView['data_3']) > 0): ?>
										<?php $__currentLoopData = $dataView['data_3']; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $item): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
											<option value=<?php echo e($item->val3); ?> <?php echo e(trim(old('val3', @$itemShow['val3']))
											        === trim(($item->val3)) ? 'selected': ''); ?>> <?php echo e($item->val3Name); ?>

											</option>
										<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
									<?php else: ?>
										<option value=""></option>
									<?php endif; ?>
								</select>
							</td>
							<td>
								<span class="col-xs-1 p-1">
								<?php echo $__env->make('layouts/error/item', ['name' => 'val3'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
								</span>
							</td>
							<td class="td-mw-108 align-middle">オーダ：</td>
							<td>
								<select name="val4">
									<?php if(count($dataView['data_4']) > 0): ?>
										<?php $__currentLoopData = $dataView['data_4']; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $item): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
											<option value=<?php echo e($item->val4); ?>

											<?php echo e(trim(old('val4', @$itemShow['val4'])) === trim($item->val4) ? 'selected': ''); ?>>
											<?php echo e($item->val4Name); ?>

											</option>
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
						</tr>
						<tr>
							<td class="td-mw-108 align-middle">工程パターン：</td>
							<td>
								<select name="val5" <?php echo e(((old('val2', @$itemShow['val2']) !== '' && old('val2',
											@$itemShow['val2']) === valueUrlEncode(0))
											|| old('val2', @$itemShow['val2']) === '') ? 'disabled' : ''); ?>>
									<?php if(count($dataView['data_5']) > 0): ?>
										<?php $__currentLoopData = $dataView['data_5']; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $item): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
										<option value=<?php echo e($item->Code); ?>

											<?php echo e(trim(old('val5', @$itemShow['val5'])) === trim($item->Code) ?
												'selected': ''); ?>>
											<?php echo e($item->val5Name); ?>

											</option>
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
					
						<tr>
							<td class="td-mw-108 align-middle">日程計算方式：</td>
							<td>
								<label>
									<input type="radio" name="val6" class="val6"
									value="<?php echo e(valueUrlEncode(0)); ?>"
									<?php echo e(((old('val2', @$itemShow['val2']) != ''
									&& old('val2', @$itemShow['val2']) == valueUrlEncode(1))
									|| old('val2', @$itemShow['val2']) == '') ? 'disabled' : ''); ?>

									<?php echo e(old('val6', @$itemShow['val6']) == valueUrlEncode(0) ? 'checked' : ''); ?>> 
									着工日・完工日を使用
								</label> /
								<label>
									<input type="radio" name="val6" class="val6"
									value="<?php echo e(valueUrlEncode(1)); ?>" 
									<?php echo e(((old('val2', @$itemShow['val2']) != ''
									&& old('val2', @$itemShow['val2']) == valueUrlEncode(1))
									|| old('val2', @$itemShow['val2']) == '') ? 'disabled' : ''); ?>

									<?php echo e(old('val6', @$itemShow['val6']) == valueUrlEncode(1) ? 'checked' : ''); ?>>
									工期・リンク日数を使用
								</label>
								
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
			<div class="row head-purple">
				<div class="col-xs-12">ファイル選択</div>
			</div>
			<div class="row ml-1 mt-3">
				<div class="col-xs-12">
					<table class="table table-borderless">
						<?php if(!$menuInfo->IsReadOnly): ?>
						<tr>
							<td class="td-mw-108 align-middle">
								<input type="file" name="val7" id="val7" class="val7 d-none"
								<?php echo e(trim(old('val2', @$itemShow['val2'])) === valueUrlEncode(1) ? 'disabled' : ''); ?>

								value="<?php echo e(@$itemData->val7); ?>" required="true">
								<input type="text" name="filename" id="filename" class="val7 input-file-width"
								<?php echo e(trim(old('val2', @$itemShow['val2'])) === valueUrlEncode(1) ? 'disabled' : ''); ?>

								value= "<?php echo e(@$itemData->filename); ?>" autocomplete="off" readonly />
							</td>

							<td>
								<span class="col-xs-1 p-1">
								<?php echo $__env->make('layouts/error/item', ['name' => 'val7'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
								</span>
							</td>
							<td class="p-0 align-middle">
								<button type="button" name="select" id="select"
								<?php echo e(trim(old('val2', @$itemShow['val2'])) === valueUrlEncode(1) ? 'disabled' : ''); ?>

								class="<?php echo e(config('system_const.btn_color_file')); ?> val7">
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
									<select name="val8" class="wd-168 pageunit-width">
										<option value="<?php echo e(valueUrlEncode(config('system_const.displayed_results_1'))); ?>"
											<?php echo e(trim(old('val8',@$itemShow['val8'])) ===
											trim(valueUrlEncode(config('system_const.displayed_results_1'))) ? 'selected' : ''); ?>>
											<?php echo e(config('system_const.displayed_results_1')); ?>

										</option>
										<option value="<?php echo e(valueUrlEncode(config('system_const.displayed_results_2'))); ?>"
											<?php echo e(trim(old('val8',@$itemShow['val8'])) ===
											trim(valueUrlEncode(config('system_const.displayed_results_2'))) ? 'selected' : ''); ?>>
											<?php echo e(config('system_const.displayed_results_2')); ?>

										</option>
										<option value="<?php echo e(valueUrlEncode(config('system_const.displayed_results_3'))); ?>"
											<?php echo e(trim(old('val8',@$itemShow['val8'])) ===
											trim(valueUrlEncode(config('system_const.displayed_results_3'))) ? 'selected' : ''); ?>>
											<?php echo e(config('system_const.displayed_results_3')); ?>

										</option>
									</select>
								</td>
								<td class="p-0 align-middle">
									<span class="col-xs-1 p-1">
										<?php echo $__env->make('layouts/error/item', ['name' => 'val8'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
									</span>
								</td>
								<td> ※1ページあたり</td>
							</tr>
						<?php endif; ?>
					</table>
				</div>
			</div>
		</form>
		<div class="row">
			<div class="col-sm-12">
				<div class="col-xs-1 p-1">
					<button type="button" id="save" class="<?php echo e(config('system_const.btn_color_ok')); ?>">
						<i class="<?php echo e(config('system_const.btn_img_ok')); ?>"></i><?php echo e(config('system_const.btn_char_ok')); ?>

					</button>
				</div>
			</div>
		</div>
	</div>
</div>
<?php $__env->stopSection(); ?>
<?php echo $__env->make('layouts/mainmenu/menu', \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/Schem/ExcelImport/index.blade.php ENDPATH**/ ?>