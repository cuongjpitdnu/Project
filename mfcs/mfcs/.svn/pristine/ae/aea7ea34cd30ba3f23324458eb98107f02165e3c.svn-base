<script>
$(function(){
	@if (!$menuInfo->IsReadOnly)
		var isHeartbeatNG = false;
		var heartbeatJson = {
				"sys_kind_id": '{{ valueUrlEncode($sysKindID) }}',
				"sys_menu_id": '{{ valueUrlEncode($sysMenuID) }}',
				"option_key": '{{ valueUrlEncode($optionKey) }}',
				};
		setInterval(function(){
			if (!isHeartbeatNG) {
				$.ajaxSetup({
					headers: {'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')}
				});
				$.ajax({
					type:'POST',
					url:'{{ url("/") }}/heartbeat',
					dataType:'json',
					contentType: "application/json",
					data:JSON.stringify(heartbeatJson),
					async : false,
				}).done(function (response,textStatus,jqXHR){
					if (response.status == '{{ config("system_const.json_status_ng") }}') {
						isHeartbeatNG = true;
						window.alert(response.message);
						$('#indicator').trigger('click');
						location.href = '{{ url("/") }}/index';
						return;
					}
				});
			}
		}, {{ (int)config('system_const.lock_heart_beat_interval_sec') * 1000 }}  );
	@endif
})
</script>