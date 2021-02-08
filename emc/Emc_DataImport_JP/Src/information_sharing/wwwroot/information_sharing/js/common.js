
$(function(){
	// ************************************************************************************************************************************************************
	//	ログ出力
	// ************************************************************************************************************************************************************
	// -------------------------------------------------------------------------
	//	memo		: add csrf token to form before
	//	create		: 2020/02/13 AKB Thang
	//	update		:
	// ------
	var csrf = $('meta[name="csrf-token"]').attr('content');
	var inputToken = '<input type="hidden" name="X-CSRF-TOKEN" value="'+csrf+'">';
	$('form').append(inputToken);

	$(document).on('click', '.pagi', function(e) {
		e.preventDefault();
		if($(this).hasClass('disabled') || $(this).hasClass('active')) return;
		var page = $(this).attr('data');
		var url = $(this).attr('href');
		// var arr = $('.search-form').serializeArray();
		var arr = [{name: 'X-CSRF-TOKEN', value: csrf}, {name: 'loadList', value: 1}];;
		arr.push({ name: 'page', value: page });
		arr.push({ name: 'event', value: 1 });
		$.ajax({
			url: url,
			type: 'POST',
			data: arr,
			success: function(result){
				try{
					eval(result);
				}catch(err){
					$(".ajax-content").html(result);
				}

			}
		});
	});

	var originalSearch = 1;
	$(document).on('click', '.btn-del', function(e){
		e.preventDefault();
		t = confirm(confirmDelMsg);
		if(!t) return;
		var arr = [{name: 'X-CSRF-TOKEN', value: csrf}, {name: 'id', value: $(this).attr('data-id')}];
		var obj = {};
		obj.name = 'mode';
		obj.value = 1;
		arr.push(obj);
		$.ajax({
			url: ajaxUrl,
			type: 'post',
			data: arr,
			success: function(result){
				console.log(result);
				if(result != 0){
					try{
						eval(result);
					}
					catch(err){
						// console.log(result);
						$('.error').html(result);
					}
				}else{
					$('.error').html('');
				}
				var arr = [{name: 'X-CSRF-TOKEN', value: csrf}, {name: 'loadList', value: 1}];
				arr.push({name: 'del', value: 1});
				$.ajax({
					url: ajaxUrl,
					type: 'post',
					data: arr,
					success: function(result){
						console.log(result);
						try{
							eval(result);
						}
						catch(err){
							$(".ajax-content").html(result);

						}


					}
				});


			}
		});
	});

	$(document).on('click', '.btn-search', function(e){
		if(originalSearch == 1){
			$('.error').html('');
		}
		var html = $('.ajax-content').html();
		var arr = $('.search-form').serializeArray();
		arr.push({name: 'originalSearch', value: originalSearch});
		$.ajax({
			url: ajaxUrl,
			type: 'post',
			data: arr,
			success: function(result){
				try{
					eval(result);
				}
				catch(err){
					$(".ajax-content").html(result);

				}
				if($('.error').html().trim() != ''){
					$(".ajax-content").html(html);
				}

			}
		});

	});

	//csv output

	$(document).on('click', '.tbl-btn-csv', function(e){
		e.preventDefault();
		if($('.tbody-data').html().trim() == ''){
			$('.error').html(csvOutputFailed);
			return;
		}
		$('[name=mode]').val(2);
		var data = $('.search-form').serialize();
		$('[name=searchData]').val(data);
		$("#formCSV").submit();

	});
	//csv output del


	$(document).on('click', '.tbl-btn-csv-del', function(e){

		e.preventDefault();
		if($('.tbody-data').html().trim() == ''){
			return;
		}
		lockScreen();
		t=confirm(confirmMsg);

		if(!t){
			unlockScreen();
			return;
		}
		$('[name=mode]').val(3);

		$.ajax({
			url: ajaxUrl,
			type: 'post',
			data: $('#formCSV').serializeArray(),
			success:function(data){

				try{
					eval(data);
				}catch(err){

				}
				var arr = [{name: 'X-CSRF-TOKEN', value: csrf}, {name: 'loadList', value: 1}];
				arr.push({name: 'delCsv', value: 1});
				$.ajax({
					url: ajaxUrl,
					type: 'post',
					data: arr,
					success: function(result){
						console.log(result);
						try{
							unlockScreen();
							eval(result);
						}
						catch(err){
							$(".ajax-content").html(result);

						}
					}
				});

			}
		});


	});
	
	function lockScreen() {
		var element = document.createElement('div');
		element.id = "screenLock";
		element.style.height = '100%'; 
		element.style.left = '0px'; 
		element.style.position = 'fixed';
		element.style.top = '0px';
		element.style.width = '100%';
		element.style.zIndex = '9999';
		element.style.backgroundColor = 'gray';
		element.style.opacity = '0.3';
		
		var objBody = document.getElementsByTagName("body").item(0); 
		objBody.appendChild(element);
	}

	function unlockScreen() {
		$("#screenLock").remove();
	}

	$(document).on('click', '.tbn-btn-return', function(e){
		e.preventDefault();
		window.location.href = 'portal.php';
	});
	//load modal
	$(document).on('click', '.load-modal', function(e){
		e.preventDefault();
		var modalUrl = $(this).attr('href');

		var arr = [
			{name: 'X-CSRF-TOKEN', value: csrf},
			{name: 'id', value: $(this).attr('data-id')},
			{name: 'screen', value: $(this).attr('data-screen')}
		];
		$('#modal-body').html('');
		$('#myModal').modal({
				backdrop: 'static'
		});

		$.ajax({
			url : modalUrl,
			type : "POST",
			data : arr,
			success : function (result){
				$('#modal-body').html(result);
			}
		});
	});

	//load modal anounce_view
	$(document).on('click', '.load-modal-announce', function(e){
		e.preventDefault();
		var modalUrl = $(this).attr('href');

		var arr = [
			{name: 'X-CSRF-TOKEN', value: csrf},
			{name: 'id', value: $(this).attr('data-id')},
			{name: 'screen', value: $(this).attr('data-screen')},
			{name: 'type', value: $(this).attr('type')}
		];
		$('#modal-body').html('');
		$('#myModal').modal({
				backdrop: 'static'
		});

		$.ajax({
			url : modalUrl,
			type : "POST",
			data : arr,
			success : function (result){
				$('#modal-body').html(result);
			}
		});
	});


	//up down link
	$(document).on('click', '.tbtn-ud', function(){
		var move = $(this).val();
		var id = $(this).attr('data-id');
		var sort = $(this).attr('data-sort')
		var dataClass = $(this).attr('data-class');
		var index = $('.'+dataClass).index(this);
		var catLength = $('.'+dataClass).length;
		if(index == 0 && $(this).hasClass('up') || index == catLength-1 && $(this).hasClass('down'))

		return;

		if($(this).hasClass('up')){
			next = $('.'+dataClass).eq(index-1).attr('data-id');
			sortNext = $('.'+dataClass).eq(index-1).attr('data-sort');
		}
		else{
			next = $('.'+dataClass).eq(index+1).attr('data-id');
			sortNext = $('.'+dataClass).eq(index+1).attr('data-sort');
		}

		$.ajax({
			url: ajaxUrl,
			type: 'post',
			data: [
				{name: 'X-CSRF-TOKEN', value: csrf},
				{name: 'move', value: move},
				{name: 'id', value: id},
				{name: 'mode', value: 2},
				{name: 'next', value: next},
				{name: 'sort', value: sort},
				{name: 'sortNext', value: sortNext},
			],
			success: function(result){
				// console.log(result);
				if(result != 0){
					$('.error').html(result);
				}

				var arr = [{name: 'X-CSRF-TOKEN', value: csrf}, {name: 'loadList', value: 1}];
				arr.push({name: 'mov', value: 1});
				$.ajax({
					url: ajaxUrl,
					type: 'post',
					data: arr,
					success: function(result){
						// console.log(result);
						try{
							eval(result);
						}
						catch(err){
							$(".ajax-content").html(result);

						}


					}
				});


			}
		});
	});
	//save form
	$(document).on('click', '.btn-save-edit', function(e){
		e.preventDefault();
		$('.edit-error').html('');
        setTimeout(function(){
		t = confirm(confirmMsg);
		if(!t){
			return;
		}

		var formData = $('.formEdit').serializeArray();
		var url = $('.formEdit').attr('action');
		// console.log(url);

		$.ajax({
			url: url,
			type: 'post',
			data: formData,
			success: function(result){
				console.log(result);
				if(result!=0 && result != 1){
					eval(result);
				}else{
					window.location.reload();
					return;
					if(result == '0'){
						var currentPage = $('.pagi.active').attr('data');
						$('#myModal').modal('hide');

						originalSearch = 0;
						$('.btn-search').click();
						// var checkAjaxStop = false;
						// $(document).ajaxStop(function(){
						// 	if(!checkAjaxStop)
						// 	$('.pagi[data="'+currentPage+'"]').click();
						// 	$('.edit-error').html('');
						// 	$('#myModal').modal('hide');
						// 	checkAjaxStop = true;
						// });

						originalSearch = 1;
					}else{
						$('#myModal').modal('hide');
						originalSearch = 0;
						$('.btn-search').click();
						// var checkAjaxStop = false;
						// $(document).ajaxStop(function(){
						// 	if(!checkAjaxStop)
						// 	$('.pagi.last').click();
						// 	$('.edit-error').html('');
						// 	$('#myModal').modal('hide');
						// 	checkAjaxStop = true;
						// });

						originalSearch = 1;
					}

				}
			}
		});
		// console.log(formData);
        },15);
	});


	$(document).on('click', '.tabbs', function(e){
		var tabIndex = $('.tabbs').index(this);
		
		
		$.ajax({
			url: 'portal_proc.php',
			type: 'get',
			data: [{name: 'tab', value: tabIndex}],
			success: function(result){
				// console.log(result);
				
				if(tabIndex == 0){
					mapRefresh();
				}
				
			}
		});
	});

});
function downloadURI(uri, urlCheck, errMsg) {
	var csrf = $('meta[name="csrf-token"]').attr('content');
	$('.edit-error').html('');
	var res = uri.replace(/\s/g, '');
	var path = decodeURIComponent(escape(window.atob(res)));
	var arrPathFile = path.split("/");
	$.ajax({
		url: urlCheck,
		type: 'POST',
		data: [
			{name: 'action', value: 'checkFile'},
			{name: 'file', value: uri},
			{name: 'X-CSRF-TOKEN', value: csrf}
		],
		success: function(result){
			console.log(result);
			if(result != 0 && result !='0'){
				// if(!msieversion()){
				// 	var link = document.createElement("a");
				// 	link.download = '';
				// 	link.href = path;
				// 	link.click();
				// }else{
				// 	var c = document.getElementById("canvas1");
				// 	var ctx = c.getContext("2d");
				// 	var img = new Image();
				// 	img.src = path;
				// 	img.onload = function() {
				// 		c.width = img.width;
				// 		c.height = img.height;
				// 		ctx.drawImage(img, 0, 0);
				// 		window.navigator.msSaveBlob(c.msToBlob(), arrPathFile[arrPathFile.length-1]);
				// 		img.src = '';
				// 		ctx.clearRect(0, 0, c.width, c.height);
				// 	}
				// }
			}else{
				$('.edit-error').html(errMsg);
			}
		}
	});



}

function msieversion() {
	var ua = window.navigator.userAgent;
	var msie = ua.indexOf("MSIE ");
	if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {
		return true;
	} else {
		return false;
	}
	return false;
}
function loadRequests() {
    var csrf = $('#csrf').val();
    var data = [
        {name: 'X-CSRF-TOKEN',value: csrf },
    ];

    $.ajax({
        url : 'incident_request_form.php',
        type : "POST",
        dataType:"text",
        data : data,
        success : function (result){
            $('.info-request').html(result);
        }
    });
}


function loadPortalClose(){
    loadInfo(1);
    loadBull();
    loadChatFormPortal();
    loadIncident();
    loadInCidentCase();
    loadRequests();
    loadInfoInCident();
    getinfodata();
}


