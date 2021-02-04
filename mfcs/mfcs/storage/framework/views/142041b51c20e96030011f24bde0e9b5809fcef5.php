<?php if(isset($rule) || $errors->has($name)): ?>
<div <?php if(isset($id)): ?> id="<?php echo e($id); ?>"<?php endif; ?> class="<?php if(isset($hidden) && $hidden): ?><?php echo e('d-none'); ?> <?php endif; ?>" style="display:inline-block;">
<div class="icon-circle tooltip-main" data-toggle="tooltip" data-placement="right" data-html="true"
title="
		<?php if(isset($rule)): ?>
			<?php echo e(str_replace(':attribute', $ruleJpName, trans('validation.' . $rule))); ?>

		<?php endif; ?>
		<?php if(isset($name)): ?>
			<?php $__currentLoopData = $errors->get($name); $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $message): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
				<?php echo e($message); ?>

				<br />
			<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
		<?php endif; ?>
"
><i class="fas fa-exclamation text-danger icon-small"></i></div>
</div>
<?php endif; ?>
<?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/layouts/error/item.blade.php ENDPATH**/ ?>