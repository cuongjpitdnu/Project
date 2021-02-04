<script>
$(function(){
	$('[data-toggle="tooltip"]').tooltip();
	$('#save').on('click', function(){
		$('#indicator').trigger('click');
		$('#mainform').submit();
	});

	$('#cancel').on('click', function(){
		$('#indicator').trigger('click');
		var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/';
		url += 'index?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
		url += '&page=<?php echo e($request->page); ?>';
		url += '&pageunit=<?php echo e($request->pageunit); ?>';
		url += '&sort=<?php echo e($request->sort); ?>';
		url += '&direction=<?php echo e($request->direction); ?>';
		window.location.href = url;
	});

	$('.selectdate').datepicker();
	$('.input-checkbox').click(function(){
		if($(this).prop('checked')){
			$('[name="'+$(this).attr('checkbox')+'"]').val(1);
		}else{
			$('[name="'+$(this).attr('checkbox')+'"]').val(0);
		}
	});
});

</script>

<div class="row ml-4">
	<div class="col-xs-12">

		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　オーダマスタ<?php if($target === 'show'): ?>参照<?php elseif($target === 'create'): ?>登録<?php elseif($target === 'edit'): ?>更新<?php endif; ?>
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
			<input type="hidden" id="kindid" name="cmn1" value="<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>">
			<input type="hidden" id="menuid" name="cmn2" value="<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>">
			<input type="hidden" name="page" value="<?php echo e($request->page); ?>">
			<input type="hidden" name="pageunit" value="<?php echo e($request->pageunit); ?>">
			<input type="hidden" name="sort" value="<?php echo e($request->sort); ?>">
			<input type="hidden" name="direction" value="<?php echo e($request->direction); ?>">
			<?php if($target === 'edit'): ?>
			<input type="hidden" name="val26" value="<?php echo e(old('val26', @$orderData['Updated_at'])); ?>">
			<?php endif; ?>
			<input type="hidden" name="method" value="<?php echo e($target); ?>">

			<table class="table table-borderless">
				<tbody>
				<tr>
					<td class="align-middle">オーダ *：</td>
					<td>
						<input type="text" name="val1" value="<?php echo e(old('val1', @$orderData['OrderNo'])); ?>"
						<?php echo e($target === "show" || $target === "edit" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="10">
						<?php if($target === "edit"): ?>
						<input type="hidden" name="val1" value="<?php echo e(old('val1', @$orderData['OrderNo'])); ?>">
						<?php endif; ?>
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val1'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
					<td class="align-middle">建造区分：</td>
					<td>
						<input type="text" name="val2" value="<?php echo e(old('val2', @$orderData['BLDDIST'])); ?>"
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="3">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val2'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">船級：</td>
					<td>
						<input type="text" name="val3" value="<?php echo e(old('val3', @$orderData['CLASS'])); ?>"
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="2">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val3'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
					<td class="align-middle">船種：</td>
					<td>
						<input type="text" name="val4" value="<?php echo e(old('val4', @$orderData['TYPE'])); ?>"
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="8">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val4'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">船型：</td>
					<td>
						<input type="text" name="val5" value="<?php echo e(old('val5', @$orderData['STYLE'])); ?>"
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="8">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val5'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
					<td class="align-middle">略称：</td>
					<td>
						<input type="text" name="val6" value="<?php echo e(old('val6', @$orderData['NAME'])); ?>"
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="8">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val6'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">Topマーキン：</td>
					<td>
						<input type="text" name="val7" value="<?php echo e(old('val7', @$orderData['TP_Date'])); ?>" class="selectdate"
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="10">
						<?php echo $__env->make('layouts/error/item', ['name' => 'val7'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val7'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
					<td class="align-middle">小組開始日：</td>
					<td>
						<input type="text" name="val8" value="<?php echo e(old('val8', @$orderData['KG_Date'])); ?>" class="selectdate"
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val8'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">大組開始日：</td>
					<td>
						<input type="text" name="val9" value="<?php echo e(old('val9', @$orderData['OG_Date'])); ?>" class="selectdate"
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val9'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
					<td class="align-middle">総組開始日：</td>
					<td>
						<input type="text" name="val10" value="<?php echo e(old('val10', @$orderData['SG_Date'])); ?>" class="selectdate"
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val10'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">搭載開始日：</td>
					<td>
						<input type="text" name="val11" value="<?php echo e(old('val11', @$orderData['LD_Date'])); ?>" class="selectdate"
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val11'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
					<td class="align-middle">船着工：</td>
					<td>
						<input type="text" name="val12" value="<?php echo e(old('val12', @$orderData['S_SDate'])); ?>" class="selectdate"
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val12'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">PE開始：</td>
					<td>
						<input type="text" name="val13" value="<?php echo e(old('val13', @$orderData['PE_SDate'])); ?>" class="selectdate"
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val13'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
					<td class="align-middle">シフト日：</td>
					<td>
						<input type="text" name="val14" value="<?php echo e(old('val14', @$orderData['ST_Date'])); ?>" class="selectdate"
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val14'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
				</tr>
				<tr>
					<td class="align-middle">進水：</td>
					<td>
						<input type="text" name="val15" value="<?php echo e(old('val15', @$orderData['L_Date'])); ?>" class="selectdate"
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val15'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
					<td class="align-middle">出渠日：</td>
					<td>
						<input type="text" name="val16" value="<?php echo e(old('val16', @$orderData['O_Date'])); ?>" class="selectdate"
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val16'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
				</tr>
				<tr>
					<td class="align-middle">離岸日：</td>
					<td>
						<input type="text" name="val17" value="<?php echo e(old('val17', @$orderData['PI_Date'])); ?>" class="selectdate"
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val17'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
					<td class="align-middle">引渡：</td>
					<td>
						<input type="text" name="val18" value="<?php echo e(old('val18', @$orderData['D_Date'])); ?>" class="selectdate"
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val18'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
				</tr>
				<tr>
					<td class="align-middle">作業長支援に取り込み可能：</td>
					<td>
						<input type="checkbox" class="input-checkbox" checkbox="val19"
						<?php echo e((int)old('val19', @$orderData['Sgts_Flag']) === 1 ? 'checked' : ''); ?>

						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?>> 取込可
						<input type="hidden" name="val19" value="<?php echo e(old('val19', @$orderData['Sgts_Flag']) ? 1 : 0); ?>">
						<?php echo $__env->make('layouts/error/item', ['name' => 'val19'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val19'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
					<td class="align-middle">ダミーオーダフラグ：</td>
					<td>
						<input type="checkbox" class="input-checkbox" checkbox="val20"
						<?php echo e((int)old('val20', @$orderData['Is_Dummy']) === 1 ? 'checked' : ''); ?>

						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?>> ダミーオーダ
						<input type="hidden" name="val20" value="<?php echo e(old('val20', @$orderData['Is_Dummy']) ? 1 : 0); ?>">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val20'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">艦艇フラグ：</td>
					<td>
						<select name="val21" <?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?>>
							<option value="0" <?php echo e((int)old('val21', @$orderData['Is_Kantei']) === 0 ? 'selected' : ''); ?>>艦艇ではない</option>
							<option value="1" <?php echo e(($target === "create" && empty(old('val21', @$orderData['Is_Kantei']))) 
							|| (int)old('val21', @$orderData['Is_Kantei']) === 1 ? 'selected' : ''); ?>>艦艇</option>
						</select>
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val21'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
					<td class="align-middle">非表示フラグ：</td>
					<td>
						<input type="checkbox" class="input-checkbox" checkbox="val22"
						<?php echo e((int)old('val22', @$orderData['DispFlag']) === 1 ? 'checked' : ''); ?>

						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?>> 非表示
						<input type="hidden" name="val22" value="<?php echo e(old('val22', @$orderData['DispFlag']) ? 1 : 0); ?>">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val22'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">WBSコード：</td>
					<td>
						<input type="text" name="val23" value="<?php echo e(old('val23', @$orderData['WBSCode'])); ?>"
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="3">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val23'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
					<td class="align-middle">前船オーダ：</td>
					<td>
						<input type="text" name="val24" value="<?php echo e(old('val24', @$orderData['PreOrderNo'])); ?>"
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val24'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">備考：</td>
					<td>
						<input type="text" name="val25" value="<?php echo e(old('val25', @$orderData['Note'])); ?>"
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="20">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val25'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
				</tr>
				</tbody>
			</table>
		</form>

		<div class="row">
			<?php if($target === 'create' || $target === 'edit'): ?>
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
</div>
<?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/mst/order/contents.blade.php ENDPATH**/ ?>