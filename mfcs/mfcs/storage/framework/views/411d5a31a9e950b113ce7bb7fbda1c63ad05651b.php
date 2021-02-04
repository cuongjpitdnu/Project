<?php $__env->startSection('content'); ?>

<script>
$(function(){
	$('[data-toggle="tooltip"]').tooltip()

	$('#selectdate').datepicker();

	$('#selectdate').change(function() {
		var val1 = $('#activeid').val();
		var val2 = $(this).val();
		var val3 = $('#sdate').val();
		$('#indicator').trigger('click');
		window.location.href = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/changedate?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>&val1=' + val1 + '&val2=' + val2 + '&val3=' + val3;
	});

	$('#orgtree')
	.on('activate_node.jstree', function (e, data) {
		var val1 = data.node.li_attr.item_id;
		$('#activeid').val(val1);
		var val3 = data.node.li_attr.s_date;
		$('#sdate').val(val3);
	}).jstree();

	$('li[name="orgitem"]').on('dblclick', function(){
		var val1 = $('#activeid').val();
		var val3 = $('#sdate').val();
		<?php
		if($menuInfo->IsReadOnly) {
			$action = 'show';
		}
		else {
			$action = 'edit';
		}
		?>
		$('#indicator').trigger('click');
		window.location.href = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/<?php echo e($action); ?>?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>&val1=' + val1 + '&val2=<?php echo e($baseDate); ?>' + '&val3=' + val3;
	});

	$('#show').on('click', function(){
		var val1 = $('#activeid').val();
		var val3 = $('#sdate').val();
		$('#indicator').trigger('click');
		window.location.href = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/show?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>&val1=' + val1 + '&val2=<?php echo e($baseDate); ?>' + '&val3=' + val3;
	});

	$('#create').on('click', function(){
		var val1 = $('#activeid').val();
		var val3 = $('#sdate').val();
		$('#indicator').trigger('click');
		window.location.href = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/create?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>&val1=' + val1 + '&val2=<?php echo e($baseDate); ?>' + '&val3=' + val3;
	});

	$('#edit').on('click', function(){
		var val1 = $('#activeid').val();
		var val3 = $('#sdate').val();
		$('#indicator').trigger('click');
		window.location.href = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/edit?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>&val1=' + val1 + '&val2=<?php echo e($baseDate); ?>' + '&val3=' + val3;
	});

	$('#delete').on('click', function(){
		var activeidValue = $('#activeid').val();
		if(activeidValue == ''){
			if('<?php echo e($errors->has("val1")); ?>' == false){
				$('#org_select_error').removeClass('d-none');
			}
			return;
		}
		var activeHasChild = $('#active_has_child').val();
		if (!window.confirm('<?php echo e(config("message.msg_cmn_if_001")); ?>')){
			return;
		}
		var hasChild = false;
		var hasNext = false;
		var checkDeleteJson = null;

		checkDeleteJson = {
			"cmn1": '<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>',
			"cmn2": '<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>',
			"val1": $('#activeid').val(),
			"val2": '<?php echo e($baseDate); ?>',
			"val3": $('#sdate').val(),
		};
		$.ajaxSetup({
			headers: {'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')}
		});
		$.ajax({
			type:'POST',
			url:'<?php echo e(url("/")); ?>/mst/org/checkdelete',
			dataType:'json',
			contentType: "application/json",
			data:JSON.stringify(checkDeleteJson),
			beforeSend : function(){
				$('#indicator').trigger('click');
			},
		}).done(function (response) {
			indicatorHide();
			var message = response.message;
			if(response.status == '<?php echo e(config("system_const.json_status_ng")); ?>'){
				hasNext = true;
			}
			if(response.has_child == '<?php echo e(config("system_const.json_return_true")); ?>'){
				hasChild = true;
			}
			if(message != null){
				window.alert(message);
				return window.location.href = '<?php echo e(url("/")); ?>/index';
			}
			var msg = null;
			if(hasChild == true && hasNext == false){
				msg = '<?php echo e(config("message.msg_org_if_001")); ?>'
			}
			else if(hasChild == false && hasNext == true){
				msg = '<?php echo e(config("message.msg_org_if_002")); ?>'
			}
			else if(hasChild == true && hasNext == true){
				msg = '<?php echo e(config("message.msg_org_if_003")); ?>'
			}
			if(msg != null){
				if (!window.confirm(msg)){
					return;
				}
			}
			var val1 = $('#activeid').val();
			var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/delete';
			$('#mainform').attr('action', url);
			$('#indicator').trigger('click');
			$('#mainform').submit();
		}).fail(function(xhr, status, error) {
			indicatorHide();
			window.location.href = '<?php echo e(url("/")); ?>/errors/500';
		});
	});
})

</script>

<?php echo $__env->make('layouts/heartbeat/heartbeat', ['sysKindID' => $menuInfo->KindID, 'sysMenuID' => $menuInfo->MenuID, 'optionKey' => config('system_const.lock_option_key_general')], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>

<input type="hidden" id="active_has_child" value="<?php echo e(valueUrlEncode($activeHasChild)); ?>">

<form action="<?php echo e(url('/')); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/index" method="POST" id="mainform">
	<?php echo csrf_field(); ?>
	<input type="hidden" id="kindid" name="cmn1" value="<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>">
	<input type="hidden" id="menuid" name="cmn2" value="<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>">
	<input type="hidden" id="activeid" name="val1" value="<?php echo e(valueUrlEncode($activeOrgID)); ?>">
	<input type="hidden" id="basedate" name="val2" value="<?php echo e($baseDate); ?>">
	<input type="hidden" id="sdate" name="val3" value="<?php echo e(valueUrlEncode($activeSDate)); ?>">
</form>

<div class="row ml-4">

	<div class="col-xs-12">

		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　職制マスタ
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

		<div class="row align-items-center">
			<div class="col-xs-3 m-1 pr-5 clearfix">
				<input id="selectdate" type="text" maxlength="10" size="14" value="<?php echo e(date('Y/m/d', strtotime($baseDate))); ?>">
				<?php echo $__env->make('layouts/error/item', ['name' => 'val2'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
			</div>
			<div class="col-xs-3 m-1 pr-5">

			</div>
			<?php if($menuInfo->IsReadOnly): ?>
			<div class="col-xs-1 m-1">
				<button id="show" type="button" class="<?php echo e(config('system_const.btn_color_info')); ?>"><?php if(config('system_const.btn_img_info')!=''): ?><i class="<?php echo e(config('system_const.btn_img_info')); ?>"></i><?php endif; ?><?php echo e(config('system_const.btn_char_info')); ?></button>
			</div>
			<?php else: ?>
			<div class="col-xs-1 m-1">
				<button id="create" type="button" class="<?php echo e(config('system_const.btn_color_new')); ?>"><?php if(config('system_const.btn_img_new')!=''): ?><i class="<?php echo e(config('system_const.btn_img_new')); ?>"></i><?php endif; ?><?php echo e(config('system_const.btn_char_new')); ?></button>
			</div>
			<div class="col-xs-1 m-1">
				<button id="edit" type="button" class="<?php echo e(config('system_const.btn_color_edit')); ?>"><?php if(config('system_const.btn_img_edit')!=''): ?><i class="<?php echo e(config('system_const.btn_img_edit')); ?>"></i><?php endif; ?><?php echo e(config('system_const.btn_char_edit')); ?></button>
			</div>
			<div class="col-xs-1 m-1">
				<button id="delete" type="button" class="<?php echo e(config('system_const.btn_color_delete')); ?>"><?php if(config('system_const.btn_img_delete')!=''): ?><i class="<?php echo e(config('system_const.btn_img_delete')); ?>"></i><?php endif; ?><?php echo e(config('system_const.btn_char_delete')); ?></button>
			</div>
			<?php endif; ?>
		</div>

		<div class="row">
			<div class="col border border-dark">

				<?php echo $__env->make('mst/org/tree', ['mstOrgCommon' => $mstOrgCommon, 'activeOrgID' => $activeOrgID, 'folderOnly' => false], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>

			</div>
			<?php echo $__env->make('layouts/error/item', ['name' => 'val1'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
			<?php echo $__env->make('layouts/error/item', ['id' => 'org_select_error', 'hidden' => true, 'rule' => 'required', 'ruleJpName' => '職制'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
		</div>

	</div>
</div>

<?php $__env->stopSection(); ?>

<?php echo $__env->make('layouts/mainmenu/menu', \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/mst/org/index.blade.php ENDPATH**/ ?>