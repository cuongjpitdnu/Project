<?php $__env->startSection('content'); ?>
<script>
    $(function() {
        $('[data-toggle="tooltip"]').tooltip();
        $('#save').on('click', function(){
            $('#indicator').trigger('click');
            $('#mainform').submit();
        });
        $('#cancel').on('click', function(){
            $('#indicator').trigger('click');
            var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/';
            url += 'index?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
            url += '&page=<?php echo e($request->page); ?>';
            url += '&pageunit=<?php echo e($request->pageunit); ?>';
            window.location.href = url;
        });
    });
</script>
<style>
	nav { float: left; margin-right: 30px; }
	.pageunit { line-height: 35px; }
	.table-row tr th,
	.table-row tr td { text-align: left; vertical-align: middle;}
	.table-row tr th.align-center,
	.table-row tr td.align-center { text-align: center; vertical-align: middle; }
	.wd-169 { width: 169px; height: 27px; }

	#fileInput {
		display: none;
	}
 
  .fa-stack { font-size: 0.5em; }
    i { vertical-align: middle; }
</style>
<form action="<?php echo e(url('/')); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/accept" method="POST" id="mainform">
	<?php echo csrf_field(); ?>
	<input type="hidden" id="kindid" name="cmn1" value="<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>">
	<input type="hidden" id="menuid" name="cmn2" value="<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>">
	<input type="hidden" id="" name="val1" value="<?php echo e($request->val1); ?>">
	<input type="hidden" id="" name="val2" value="<?php echo e($request->val2); ?>">
	<input type="hidden" id="" name="val3" value="<?php echo e($request->val3); ?>">
	<div class="row ml-4">
		<div class="col-xs-12">
			<div class="row align-items-center">
				<div class="col-xs-1 text-left m-2 p-2 rounded border">
					■搭載日程展開 - 詳細
				</div>
            </div>
            
			<?php if(isset($originalError) && count($originalError)): ?>
            <div class="row">
                <div class="col-xs-12">
                    <div class="alert alert-danger">
                        <ul>
                            <?php $__currentLoopData = $originalError; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $item): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
                            <li><?php echo e($item); ?></li>
                            <?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
                        </ul>
                    </div>
                </div>
            </div>
            <?php endif; ?>

			<div class="row">
                <div class="col-sm-12" style="padding-left:1rem !important;">
                    <span class="fa-stack" style="vertical-align: top;">
                        <i class="far fa-circle fa-stack-2x"></i>
                        <i class="fas fa-info fa-stack-1x"></i>
                    </span>
                    
                    現在の中日程との相違点です。よろしければ、保存ボタンを押してください。
					<table class="table table-row">
						<tbody>
							<tr class="set-color">
								<th><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld1', trans('schemImport.sortable.fld1')));?></th>
                                <th><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld2', trans('schemImport.sortable.fld2')));?></th>
                                <th><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld3', trans('schemImport.sortable.fld3')));?></th>
                                <th><?php echo \Kyslik\ColumnSortable\SortableLink::render(array ('fld4', trans('schemImport.sortable.fld4')));?></th>
                            </tr>
                            <?php $__currentLoopData = $rows; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $row): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
							<tr>
								<td><?php echo e($row->fld1); ?></td>
								<td><?php echo e($row->fld2); ?></td>
                                <td><?php echo e($row->fld3); ?></td>
                                <td><?php echo e($row->fld4); ?></td>
                            </tr>
                            <?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
						</tbody>
                    </table>   
				</div>
			</div>
			<div class="row">
                <div class="col-md-5">
                    <button type="button" id="save" class="<?php echo e(config('system_const.btn_color_save')); ?>">
                        <i class="<?php echo e(config('system_const.btn_img_save')); ?>"></i><?php echo e(config('system_const.btn_char_save')); ?>

                    </button>&emsp;
                    <button type="button" id="cancel" class="<?php echo e(config('system_const.btn_color_cancel')); ?>">
                        <i class="<?php echo e(config('system_const.btn_img_cancel')); ?>"></i><?php echo e(config('system_const.btn_char_cancel')); ?>

                    </button> 
                </div>
                <div class="col-md-7">
                    <div class="col-xs-1 p-1"> <?php echo e($rows->appends(request()->query())->links()); ?> </div>
                    <?php echo $__env->make('layouts/heartbeat/heartbeat', ['sysKindID' => $menuInfo->KindID, 'sysMenuID' => config(' system_const_schem.sys_menu_id_plan'),'optionKey' =>valueUrlDecode($request->val2)], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
                </div>
            </div>
		</div>
	</div>
</form>
<?php $__env->stopSection(); ?>
<?php echo $__env->make('layouts/mainmenu/menu', \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?><?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/Schem/Import/create.blade.php ENDPATH**/ ?>