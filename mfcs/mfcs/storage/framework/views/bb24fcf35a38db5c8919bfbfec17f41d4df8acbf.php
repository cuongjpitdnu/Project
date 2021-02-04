<?php $__env->startSection('content'); ?>
<script>
	$(function() {
		$('[data-toggle="tooltip"]').tooltip();
		var selectedValueVal2 = '<?php echo e(isset($request->val2) ? valueUrlDecode(old('val2',$request->val2)) : (is_null(old('val2')) ? config('system_const.c_kind_chijyo') : old('val2'))); ?>';
		const projects = fncJsonParse('<?php echo e(json_encode($projects)); ?>');
		binding(projects, selectedValueVal2, '<?php echo e((old('val3',$request->val3))); ?>');
		$('input[name=val2]').on('change', function() {
			$('select[name=val3]').empty();
			binding(projects, $(this).val(), '');
		});
		function binding(data, selectWhere, selected) {
			
			data.forEach(element => {
				var selectValue = (element.ID == selected) ? 'selected' : '';
				if(selectWhere == element.ListKind){
					$('select[name=val3]').append(`</option><option value="${element.ID}" ${selectValue}>${convertHTML(element.ProjectName)}</option>`);
				}
			});
		}
		$('#save').on('click', function(){
			$('#indicator').trigger('click');
			$('select[name=val3]').val($('select[name=val3]').val() == -1 ?  null : $('select[name=val3]') .val());
			$('#mainform').submit();
		});
	});
</script>

<style>
	.wd-30{ margin-left: 30px; }
	.wd-15{ margin-left: 15px; }
</style>
<form action="<?php echo e(url('/')); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/import" method="POST" id="mainform">
	<?php echo csrf_field(); ?>
	<input type="hidden" id="kindid" name="cmn1" value="<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>">
	<input type="hidden" id="menuid" name="cmn2" value="<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>">
	<input type="hidden" id="selectedValueVal2" value="<?php echo e(old('val2',$request->val2)); ?>">
	<div class="row ml-2 mr-2">
		<div class="col-md-12 col-xs-12">
			<div class="row align-items-center">
				<div class="col-xs-1 text-left m-2 p-2 rounded border">
					■　搭載日程展開
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
			<div class="row head-purple">
				<div class="col-xs-12">搭載日程選択</div>
			</div>
			<div class="row ml-1">
				<div class="col-xs-12">
					<table class="table table-borderless">
						<tbody>
							<tr>
								<td>オーダ：</td>
								<td>
									<select name="val1" class="wd-30">
										<?php if(count($orders) > 0): ?>
											<?php $__currentLoopData = $orders; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $value): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
												<option value="<?php echo e(valueUrlEncode($value->OrderNo)); ?>" 
													<?php echo e(trim(valueUrlDecode(old('val1',@$request->val1)))
													 === trim($value->OrderNo) ? 'selected' : ''); ?>>
													<?php echo e($value->OrderNo); ?>

												</option>
											<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
										<?php else: ?>
											<option value=""></option>
										<?php endif; ?>
									</select>					
									<?php echo $__env->make('layouts/error/item', ['name' => 'val1'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
								</td>
							</tr>
						</tbody>
					</table>
				</div>
			</div>
			<div class="row head-purple">
				<div class="col-xs-12">中日程選択</div>
			</div>
			<div class="row ml-1">
				<div class="col-xs-12">
					<table class="table table-borderless">
						<tbody>
							<tr>
								<td>中日程区分 : </td>
								<td>
									<label>
										<input type="radio" id="rdo1" name="val2" 
											value="<?php echo e(config('system_const.c_kind_chijyo')); ?>" 
											<?php echo e(trim(valueUrlDecode(old('val2',@$itemShow['val2']))) 
											=== trim(config('system_const.c_kind_chijyo')) ? 'checked' : ''); ?>>
											<?php echo e(config('system_const.c_name_chijyo')); ?>

									</label> /
									<label>
										<input type="radio" id="rdo2" name="val2" 
											value="<?php echo e(config('system_const.c_kind_gaigyo')); ?>" 
											<?php echo e(trim(valueUrlDecode(old('val2',@$itemShow['val2'])))
											=== trim(config('system_const.c_kind_gaigyo')) ? 'checked' : ''); ?>>
											<?php echo e(config('system_const.c_name_gaigyo')); ?>

									</label> /
									<label>
										<input type="radio" id="rdo3" name="val2" 
											value=" <?php echo e(config('system_const.c_kind_giso')); ?>" 
											<?php echo e(trim(valueUrlDecode(old('val2',@$itemShow['val2'])))
											=== trim(config('system_const.c_kind_giso')) ? 'checked' : ''); ?>> 
											<?php echo e(config('system_const.c_name_giso')); ?>

									</label>

									
									
									<?php echo $__env->make('layouts/error/item', ['name' => 'val2'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
								</td>
							</tr>
						
							<tr>
								<td>検討ケース：</td>
								<td>
									<select name="val3" class="selectedValueVal2"></select>
									<?php echo $__env->make('layouts/error/item', ['name' => 'val3'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
								</td>
							</tr>
						</tbody>
					</table>
				</div>
			</div>
			<div class="row head-purple">
				<div class="col-xs-12">ログ表示設定</div>
			</div>
			<div class="row ml-1">
				<div class="col-xs-12">
					<table class="table table-borderless">
						<tbody>
							<tr>
								<td>表示件数：</td>
								<td>
									<select name="val4" class="wd-15 pageunit-width">
										<option value="<?php echo e(config('system_const.displayed_results_1')); ?>" 
											<?php echo e(trim(valueUrlDecode(old('val4',@$request->val4))) 
											=== trim(config('system_const.displayed_results_1')) ? 'selected' : ''); ?>>
											<?php echo e(config('system_const.displayed_results_1')); ?>

										</option>
										<option value="<?php echo e(config('system_const.displayed_results_2')); ?>" 
											<?php echo e(trim(valueUrlDecode(old('val4',@$request->val4))) 
											=== trim(config('system_const.displayed_results_2')) ? 'selected' : ''); ?>>
											<?php echo e(config('system_const.displayed_results_2')); ?>

										</option>
										<option value="<?php echo e(config('system_const.displayed_results_3')); ?>" 
											<?php echo e(trim(valueUrlDecode(old('val4',@$request->val4))) 
											=== trim(config('system_const.displayed_results_3')) ? 'selected' : ''); ?>>
											<?php echo e(config('system_const.displayed_results_3')); ?>

										</option>
									</select>
									※1ページあたり
									<?php echo $__env->make('layouts/error/item', ['name' => 'val4'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
								</td>
							</tr>
						</tbody>
					</table>
				</div>
			</div>
			<div class="row">
				<div class="col-sm-12" style="padding-left:1rem !important;">
					<div class="col-xs-1 p-1">
						<button type="button" id="save" class="<?php echo e(config('system_const.btn_color_ok')); ?>">
							<i class="<?php echo e(config('system_const.btn_img_ok')); ?>"></i>
							<?php echo e(config('system_const.btn_char_ok')); ?>

						</button>
					</div>
				</div>
			</div>
		</div>
	</div>
</form>
<?php $__env->stopSection(); ?>
<?php echo $__env->make('layouts/mainmenu/menu', \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/Schem/Import/index.blade.php ENDPATH**/ ?>