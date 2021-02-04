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
			@include('mst/org/tree', ['mstOrgCommon' => $mstOrgCommon, 'activeOrgID' => $activeOrgID, 'folderOnly' => $l_folderOnly])

			</div>
			<div class="modal-footer">
				<button type="button" id="select_org_ok" class="{{ config('system_const.btn_color_ok') }}">@if (config('system_const.btn_img_ok')!='')<i class="{{ config('system_const.btn_img_ok') }}"></i>@endif{{ config('system_const.btn_char_ok') }}</button>
				<button type="button" data-dismiss="modal" class="{{ config('system_const.btn_color_cancel') }}">@if (config('system_const.btn_img_cancel')!='')<i class="{{ config('system_const.btn_img_cancel') }}"></i>@endif{{ config('system_const.btn_char_cancel') }}</button>
			</div>
		</div>
	</div>
</div>