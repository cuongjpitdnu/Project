<?php $__env->startSection('content'); ?>
<script>
	$(function() {
		$('#ok').on('click', function(e) {
			$('#indicator').trigger('click');
			let valueChecked = $('input[name=val1]:checked').val();
			let page = (valueChecked == 0) ? 'create' : ((valueChecked == 1) ? 'copy' : ((valueChecked == 2) ? 'delete' : 'apply'));
			let url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/'+page;
			url += '?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
			window.location.href = url;
		});
	});
</script>

<style>
	#tbl-rdo { margin-top: 20px; }
	#tbl-rdo tr td { line-height: 30px; vertical-align: middle; }
</style>

<div class="row ml-2 mr-2">
	<div class="col-md-12 col-xs-12">
		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　検討ケース作成
			</div>
		</div>
		<div class="row head-purple">
			<div class="col-xs-12">条件選択</div>
		</div>
		<div class="row ml-2">
			<div class="col-xs-12">
				<table id="tbl-rdo">
					<tbody>
						<tr>
							<td>
								<label for="rdo-create">
									<input type="radio" name="val1" id="rdo-create" value="0" checked /> 空の検討ケースを作成
								</label>
							</td>
						</tr>
						<tr>
							<td>
								<label for="rdo-copy">
									<input type="radio" name="val1" id="rdo-copy" value="1" /> 既存の検討ケースからコピー
								</label>
							</td>
						</tr>
						<tr>
							<td>
								<label for="rdo-delete">
									<input type="radio" name="val1" id="rdo-delete" value="2" /> 検討ケースを削除
								</label>
							</td>
						</tr>
						<tr>
							<td>
								<label for="rdo-apply">
									<input type="radio" name="val1" id="rdo-apply" value="3" /> 本番に適用
								</label>
							</td>
						</tr>
					</tbody>
				</table>
			</div>
		</div>
		<div class="row ml-1">
			<div class="col-xs-1 p-1">
				<button type="button" id="ok" class="<?php echo e(config('system_const.btn_color_ok')); ?>">
					<i class="<?php echo e(config('system_const.btn_img_ok')); ?>"></i><?php echo e(config('system_const.btn_char_ok')); ?></button>
			</div>
		</div>
	</div>
</div>
<?php $__env->stopSection(); ?>
<?php echo $__env->make('layouts/mainmenu/menu', \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/Schem/case/index.blade.php ENDPATH**/ ?>