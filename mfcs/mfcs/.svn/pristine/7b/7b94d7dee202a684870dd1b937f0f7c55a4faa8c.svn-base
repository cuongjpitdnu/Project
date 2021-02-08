<?php
$tops = $mstOrgCommon->getTopLvList();
?>

@if (!isset($tops) || count($tops) == 0)
	{{ config('message.msg_cmn_db_001') }}
@else
	<div id="orgtree">
		<?php
		$parents = null;
		if (isset($activeOrgID)){
			$parents = $mstOrgCommon->getPIDAll($activeOrgID);
		}
		?>
		@foreach($tops as $grpID)
		@include('Mst/Org/orgitem', ['mstOrgCommon' => $mstOrgCommon, 'grpID' => $grpID, 'activeOrgID' => $activeOrgID, 'parents' => $parents, 'folderOnly' => $folderOnly])
		@endforeach
	</div>
@endif
