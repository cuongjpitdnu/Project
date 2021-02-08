<?php
	$mstItem = $mstOrgCommon->getDataFromID($grpID);
?>
@if (!isset($folderOnly) || !$folderOnly || $mstItem['folderflag'] == '1')
<ul>
	<?php
	$selected = 'false';
	if ($grpID == $activeOrgID) {
		$selected = 'true';
	}
	$opened = 'false';
	if (isset($activeOrgID) && isset($parents) && in_array($activeOrgID, $parents)) {
		$opened = 'true';
	}
	$icon = '';
	if ($mstItem['folderflag'] == '0') {
		$icon .= 'jstree-file';
	}
	?>
	<li name="orgitem" item_id="{{ valueUrlEncode($grpID) }}" item_name="{{ $mstItem['name'] }}" folder_flag="{{ $mstItem['folderflag'] }}" s_date="{{ valueUrlEncode($mstItem['sdate']) }}" full_name="{{ $mstOrgCommon->getFullName($grpID) }}" data-jstree='{"selected":{{ $selected }}, "opened":{{ $opened }}, "icon":"{{ $icon }}"}'>{{ $mstItem['name'] }}
		@foreach($mstOrgCommon->getChildID($grpID) as $childID)
			@include('Mst/Org/orgitem', ['mstOrgCommon' => $mstOrgCommon, 'grpID' => $childID])
		@endforeach
	</li>
</ul>
@endif
