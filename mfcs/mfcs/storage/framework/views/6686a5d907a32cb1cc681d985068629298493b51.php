<?php
	$mstItem = $mstOrgCommon->getDataFromID($grpID);
?>
<?php if(!isset($folderOnly) || !$folderOnly || $mstItem['folderflag'] == '1'): ?>
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
	<li name="orgitem" item_id="<?php echo e(valueUrlEncode($grpID)); ?>" item_name="<?php echo e($mstItem['name']); ?>" folder_flag="<?php echo e($mstItem['folderflag']); ?>" s_date="<?php echo e(valueUrlEncode($mstItem['sdate'])); ?>" full_name="<?php echo e($mstOrgCommon->getFullName($grpID)); ?>" data-jstree='{"selected":<?php echo e($selected); ?>, "opened":<?php echo e($opened); ?>, "icon":"<?php echo e($icon); ?>"}'><?php echo e($mstItem['name']); ?>

		<?php $__currentLoopData = $mstOrgCommon->getChildID($grpID); $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $childID): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
			<?php echo $__env->make('Mst/Org/orgitem', ['mstOrgCommon' => $mstOrgCommon, 'grpID' => $childID], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
		<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
	</li>
</ul>
<?php endif; ?>
<?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/Mst/Org/orgitem.blade.php ENDPATH**/ ?>