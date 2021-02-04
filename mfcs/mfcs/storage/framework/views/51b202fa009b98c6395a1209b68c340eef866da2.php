<?php
$tops = $mstOrgCommon->getTopLvList();
?>

<?php if(!isset($tops) || count($tops) == 0): ?>
	<?php echo e(config('message.msg_cmn_db_001')); ?>

<?php else: ?>
	<div id="orgtree">
		<?php
		$parents = null;
		if (isset($activeOrgID)){
			$parents = $mstOrgCommon->getPIDAll($activeOrgID);
		}
		?>
		<?php $__currentLoopData = $tops; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $grpID): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
		<?php echo $__env->make('Mst/Org/orgitem', ['mstOrgCommon' => $mstOrgCommon, 'grpID' => $grpID, 'activeOrgID' => $activeOrgID, 'parents' => $parents, 'folderOnly' => $folderOnly], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
		<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
	</div>
<?php endif; ?>
<?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/mst/org/tree.blade.php ENDPATH**/ ?>