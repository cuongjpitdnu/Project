<div class="modal fade" id="org_select_dialog" tabindex="-1" role="dialog" aria-labelledby="Modal" aria-hidden="true">
	<div class="modal-dialog" role="document">
		<div class="modal-content">
			<div class="modal-header">
				<h5 class="modal-title" id="Modal">職制選択</h5>
				<button type="button" class="close" data-dismiss="modal" aria-label="閉じる">
					<span aria-hidden="true">&times;</span>
				</button>
			</div>
			<div class="modal-body">

			<?php
			$l_folderOnly = true;
			if (isset($folderOnly)) {
				$l_folderOnly = $folderOnly;
			}
			?>
			<?php echo $__env->make('mst/org/tree', ['mstOrgCommon' => $mstOrgCommon, 'activeOrgID' => $activeOrgID, 'folderOnly' => $l_folderOnly], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>

			</div>
			<div class="modal-footer">
				<button type="button" id="select_org_ok" class="<?php echo e(config('system_const.btn_color_ok')); ?>"><?php if(config('system_const.btn_img_ok')!=''): ?><i class="<?php echo e(config('system_const.btn_img_ok')); ?>"></i><?php endif; ?><?php echo e(config('system_const.btn_char_ok')); ?></button>
				<button type="button" data-dismiss="modal" class="<?php echo e(config('system_const.btn_color_cancel')); ?>"><?php if(config('system_const.btn_img_cancel')!=''): ?><i class="<?php echo e(config('system_const.btn_img_cancel')); ?>"></i><?php endif; ?><?php echo e(config('system_const.btn_char_cancel')); ?></button>
			</div>
		</div>
	</div>
</div><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/mst/org/select.blade.php ENDPATH**/ ?>