<script>
$(function(){
	<?php if(!$menuInfo->IsReadOnly): ?>
		var isHeartbeatNG = false;
		var heartbeatJson = {
				"sys_kind_id": '<?php echo e(valueUrlEncode($sysKindID)); ?>',
				"sys_menu_id": '<?php echo e(valueUrlEncode($sysMenuID)); ?>',
				"option_key": '<?php echo e(valueUrlEncode($optionKey)); ?>',
				};
		setInterval(function(){
			if (!isHeartbeatNG) {
				$.ajaxSetup({
					headers: {'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')}
				});
				$.ajax({
					type:'POST',
					url:'<?php echo e(url("/")); ?>/heartbeat',
					dataType:'json',
					contentType: "application/json",
					data:JSON.stringify(heartbeatJson),
					async : false,
				}).done(function (response,textStatus,jqXHR){
					if (response.status == '<?php echo e(config("system_const.json_status_ng")); ?>') {
						isHeartbeatNG = true;
						window.alert(response.message);
						$('#indicator').trigger('click');
						location.href = '<?php echo e(url("/")); ?>/index';
						return;
					}
				});
			}
		}, <?php echo e((int)config('system_const.lock_heart_beat_interval_sec') * 1000); ?>  );
	<?php endif; ?>
})
</script><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/layouts/heartbeat/heartbeat.blade.php ENDPATH**/ ?>