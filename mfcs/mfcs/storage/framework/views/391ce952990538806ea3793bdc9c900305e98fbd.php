<script>
$(function(){
	$('[data-toggle="tooltip"]').tooltip();

	$('#save').on('click', function(){
		$('#indicator').trigger('click');

		if($('select[name=tempVal303]').is(':disabled')) {
			$('input[name=val303]').val(0);
		} else {
			var tempValue = $('select[name=tempVal303]').val();
			$('input[name=val303]').val(tempValue);
		}

		if($('select[name=tempVal307]').is(':disabled')) {
			$('input[name=val307]').val(0);
		} else {
			var tempValue = $('select[name=tempVal307]').val();
			$('input[name=val307]').val(tempValue);
		}

		$('#mainform').submit();
	});

	$('#cancel').on('click', function(){
		$('#indicator').trigger('click');
		var url = '<?php echo e(url("/")); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/history';
		url += '?cmn1=<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>&cmn2=<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>';
		url += '&pageunit=<?php echo e($request->pageunit); ?>&searchpage=<?php echo e($request->searchpage); ?>';
		url += '&searchsort=<?php echo e($request->searchsort); ?>';
		url += '&searchdirection=<?php echo e($request->searchdirection); ?>';
		url += '&page=<?php echo e($request->page); ?>';
		url += '&sort=<?php echo e($request->sort); ?>';
		url += '&direction=<?php echo e($request->direction); ?>';
		url +=  '&val1=<?php echo e($request->val1); ?>&val2=<?php echo e($request->val2); ?>&val3=<?php echo e($request->val3); ?>';
		url +=  '&val4=<?php echo e($request->val4); ?>&val5=<?php echo e($request->val5); ?>';
		url +=  '&val101=<?php echo e($request->val101); ?>';
		window.location.href = url;
	});

	$('#orgtree')
	.on('activate_node.jstree', function (e, data) {
		var selectedID = data.node.li_attr.item_id;
		$('#select_grp_id').val(selectedID);
		var selectedName = data.node.li_attr.item_name;
		$('#select_grp_name').val(selectedName);
	}).jstree();

	$('#select_org_ok').on('click', function(){
		var val304 = $('#select_grp_id').val();
		$('#val304').val(val304);
		var orgName = $('#select_grp_name').val();
		$('[name=parent_org_name]').val(orgName);
		$('#org_select_dialog').modal('hide');
	});

	$('.clearorg').on('click', function(){
		$('[name=parent_org_name]').val("<?php echo e(config('system_const.org_null_name')); ?>");
		$('input[name="val304"]').val("<?php echo e(valueUrlEncode(0)); ?>");
	});
	
	$('.selectdate').datepicker();

	var arrKanren = fncJsonParse('<?php echo e(json_encode($arrKanren)); ?>');
	var arrVal307 = [
		{ id: 0, name: "" },
		{ id: 1, name: "貸付" },
		{ id: 2, name: "一括" },
		{ id: 3, name: "県外工" },
	];
	
	$('select[name=val302]').on('change', function () {
		$('select[name=tempVal303]').empty();
		$('select[name=tempVal307]').empty();
		if(this.value == 2){
			if( $('input[name=method]').val() != 'show') {
				$('select[name="tempVal303"]').removeAttr('disabled', 'disabled');
				$('select[name="tempVal307"]').removeAttr('disabled', 'disabled');
			}
			bindingDataSelectBox('select[name=tempVal303]', arrKanren, $('input[name=val303]').val());
			bindingDataSelectBox('select[name=tempVal307]', arrVal307, $('input[name=val307]').val());

		} else {
			$('select[name=tempVal303]').append('<option value="0" selected="selected"></option>');
			$('select[name="tempVal303"]').attr('disabled', 'disabled');

			$('select[name=tempVal307]').append('<option value="0" selected="selected"></option>');
			$('select[name="tempVal307"]').attr('disabled', 'disabled');
		}
	}).trigger('change');

	function bindingDataSelectBox(selectbox, data, selectedValue){
		data.forEach(element => {
			var selectValue = (element.id == selectedValue) ? 'selected' : '';
			$(selectbox).append('<option value="'+ element.id +'" '+ selectValue +'>'+  element.name +'</option>');
		});
	}

})

</script>

<div class="row ml-4">
	<div class="col-xs-12">

		<div class="row align-items-center">
			<div class="col-xs-1 text-left m-2 p-2 rounded border">
				■　人員マスタ履歴<?php if($target === 'show'): ?>参照<?php elseif($target === 'create'): ?>登録<?php elseif($target === 'edit'): ?>更新<?php endif; ?>
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
		
		<form action="<?php echo e(url('/')); ?>/<?php echo e($menuInfo->KindURL); ?>/<?php echo e($menuInfo->MenuURL); ?>/historysave" method="POST" id="mainform">
			<?php echo csrf_field(); ?>
			<input type="hidden" name="method" value="<?php echo e($target); ?>">
			<input type="hidden" id="kindid" name="cmn1" value="<?php echo e(valueUrlEncode($menuInfo->KindID)); ?>">
			<input type="hidden" id="menuid" name="cmn2" value="<?php echo e(valueUrlEncode($menuInfo->MenuID)); ?>">
			<input type="hidden" id="pageunit" name="pageunit" value="<?php echo e($request->pageunit); ?>">
			<input type="hidden" id="searchpage" name="searchpage" value="<?php echo e($request->searchpage); ?>">
			<input type="hidden" id="searchsort" name="searchsort" value="<?php echo e($request->searchsort); ?>">
			<input type="hidden" id="searchdirection" name="searchdirection" value="<?php echo e($request->searchdirection); ?>">
			<input type="hidden" id="page" name="page" value="<?php echo e($request->page); ?>">
			<input type="hidden" id="sort" name="sort" value="<?php echo e($request->sort); ?>">
			<input type="hidden" id="direction" name="direction" value="<?php echo e($request->direction); ?>">
			<input type="hidden" id="val1" name="val1" value="<?php echo e($request->val1); ?>">
			<input type="hidden" id="val2" name="val2" value="<?php echo e($request->val2); ?>">
			<input type="hidden" id="val3" name="val3" value="<?php echo e($request->val3); ?>">
			<input type="hidden" id="val4" name="val4" value="<?php echo e($request->val4); ?>">
			<input type="hidden" id="val5" name="val5" value="<?php echo e($request->val5); ?>">
			<input type="hidden" id="val101" name="val101" value="<?php echo e($request->val101); ?>">
			<input type="hidden" id="val201" name="val201" value="<?php echo e($request->val201); ?>">
			<input type="hidden" id="val202" name="val202" value="<?php echo e($request->val202); ?>">
			<input type="hidden" id="val304" name="val304" value="<?php echo e((old('val304', valueUrlEncode(@$memHist['val304'])))); ?>">
			<input type="hidden" id="select_grp_id" name="select_grp_id" value="<?php echo e((old('val304', valueUrlEncode(@$memHist['val304'])))); ?>">
			<input type="hidden" id="select_grp_name" name="select_grp_name" value="<?php echo e(old('select_grp_name', $grpName)); ?>">

			<table class="table table-borderless">
				<tbody>
				<tr>
					<td class="align-middle">社員番号：</td>
					<td>
					<input type="text" name="val301" value="<?php echo e(old('val301', @$memHist['val301'])); ?>" class="text-right" 
					<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val301'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
					<td class="align-middle">社内外フラグ *：</td>
					<td>
						<select name="val302" <?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?>>
							<option value="1" <?php echo e((int)old('val302',@$memHist['val302']) === 1 ? 'selected' : ''); ?> >社内</option>
							<option value="2" <?php echo e((int)old('val302',@$memHist['val302']) === 2 ? 'selected' : ''); ?>>社外</option>
						</select>
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val302'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
				</tr>
				
				<tr>
					<td class="align-middle">会社名：</td>
					<td>
						<select name="tempVal303" <?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?>></select>
						<input type="hidden" id="" name="val303" value="<?php echo e(old('val303', @$memHist['val303'])); ?>" />
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val303'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
					<td class="align-middle">所属班：</td>
					<td> 
						<input type="text" <?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> name="parent_org_name" 
						value="<?php echo e(old('parent_org_name', $grpName)); ?>" readonly="" tabindex="-1" data-toggle="modal" data-target="#org_select_dialog">
						<input type="hidden" name="parent_org_name" value="<?php echo e(old('parent_org_name', $grpName)); ?>">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val304'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
					<?php if($target === 'create' || $target === 'edit'): ?>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<button type="button" id="select" class="<?php echo e(config('system_const.btn_color_file')); ?>" data-toggle="modal" data-target="#org_select_dialog">
								<i class="<?php echo e(config('system_const.btn_img_file')); ?>"></i><?php echo e(config('system_const.btn_char_file')); ?>

							</button>
						</span>
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<button type="button" name="clearorg" class="clearorg <?php echo e(config('system_const.btn_color_clear')); ?>">
								<i class="<?php echo e(config('system_const.btn_img_clear')); ?>"></i><?php echo e(config('system_const.btn_char_clear')); ?>

							</button>
						</span>
					</td>
					<?php echo $__env->make('mst/org/select', ['mstOrgCommon' => $mstOrgCommon, 'activeOrgID' => $memHist['val304'], 'folderOnly' => false], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
					<?php endif; ?>
				</tr>

				<tr>
					<td class="align-middle">開始日 *：</td>
					<td>
						<input id="" type="text" maxlength="10" name="val305" size="14" value="<?php echo e(old('val305', @$memHist['val305'])); ?>" 
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> class="selectdate" autocomplete="off">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val305'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
					<td class="align-middle">終了日：</td>
					<td>
						<input id="" type="text" maxlength="10" name="val306" size="14" value="<?php echo e(old('val306', @$memHist['val306'])); ?>" 
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> class="selectdate" autocomplete="off">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val306'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">職種 *：</td>
					<td>
						<select name="val308" <?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?>>
							<option value=""></option>
							<?php $__currentLoopData = $mstSyokusyu; $__env->addLoop($__currentLoopData); foreach($__currentLoopData as $item): $__env->incrementLoopIndices(); $loop = $__env->getLastLoop(); ?>
								<option value="<?php echo e($item->Code); ?>" <?php echo e(trim(old('val308', @$memHist['val308'])) === trim($item->Code) ? 'selected' : ''); ?>>
									<?php echo e($item->Name); ?>

								</option>
							<?php endforeach; $__env->popLoop(); $loop = $__env->getLastLoop(); ?>
						</select>
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val308'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
					<td class="align-middle">外注班タイプ：</td>
					<td>
						<select name="tempVal307" <?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?>>
							
						</select>
						<input type="hidden" id="" name="val307" value="<?php echo e(old('val307', @$memHist['val307'])); ?>" />
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val307'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
				</tr>

				<tr>
					<td class="align-middle">プロパー *：</td>
					<td>
						<select name="val309" <?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?>>
							<option value="0" <?php echo e(trim(old('val309', @$memHist['val309'])) === '0' ? 'selected' : ''); ?>>その他</option>
							<option value="1" <?php echo e(($target === "create" || trim(old('val309', @$memHist['val309']))) === '1' ? 'selected' : ''); ?>>直雇</option>
							<option value="2" <?php echo e(trim(old('val309', @$memHist['val309'])) === '2' ? 'selected' : ''); ?>>外国人研修生</option>
							<option value="3" <?php echo e(trim(old('val309', @$memHist['val309'])) === '3' ? 'selected' : ''); ?>>請負</option>
							<option value="4" <?php echo e(trim(old('val309', @$memHist['val309'])) === '4' ? 'selected' : ''); ?>>派遣</option>
							<option value="5" <?php echo e(trim(old('val309', @$memHist['val309'])) === '5' ? 'selected' : ''); ?>>再雇用</option>
							<option value="6" <?php echo e(trim(old('val309', @$memHist['val309'])) === '6' ? 'selected' : ''); ?>>パート</option>
							<option value="7" <?php echo e(trim(old('val309', @$memHist['val309'])) === '7' ? 'selected' : ''); ?>>加工外注</option>
						</select>
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val309'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
					<td class="align-middle">表示順：</td>
					<td>
						<input type="text" name="val310" value="<?php echo e(old('val310', @$memHist['val310'])); ?>" class="text-right"
						<?php echo e($target === "show" ? 'disabled="disabled"' : ''); ?> autocomplete="off" maxlength="10">
					</td>
					<td class="p-0 align-middle">
						<span class="col-xs-1 p-1">
							<?php echo $__env->make('layouts/error/item', ['name' => 'val310'], \Illuminate\Support\Arr::except(get_defined_vars(), ['__data', '__path']))->render(); ?>
						</span>
					</td>
				</tr>
				</tbody>
			</table>

		</form>

		<div class="row">
			<?php if($target === 'create' || $target === 'edit'): ?>
			<div class="col-xs-1 p-1">
				<button type="button" id="save" class="<?php echo e(config('system_const.btn_color_save')); ?>"><i class="<?php echo e(config('system_const.btn_img_save')); ?>"></i><?php echo e(config('system_const.btn_char_save')); ?></button>
			</div>
			<?php endif; ?>
			<div class="col-xs-1 p-1">
				<button type="button" id="cancel" class="<?php echo e(config('system_const.btn_color_cancel')); ?>"><i class="<?php echo e(config('system_const.btn_img_cancel')); ?>"></i><?php echo e(config('system_const.btn_char_cancel')); ?></button>
			</div>
		</div>

	</div>
</div>
<?php /**PATH C:\Xamp\htdocs\mfcs\resources\views/mst/member/historycontents.blade.php ENDPATH**/ ?>