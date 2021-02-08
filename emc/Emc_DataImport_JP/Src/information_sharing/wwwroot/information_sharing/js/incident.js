$( document ).ready(function() {
    $('#content2').find('.info-cont').attr('id','info-cont-2');
    $('#content2').find('.selAnnounceTime').attr('id','selTimeAnnouce2');
    $('#content2').find('.selAnnounceTime').attr('data-id','2');
    loadInfo(2);

    $( "#tab2" ).click(function() {
        setHeigh();
    });
    $( "#tab1" ).click(function() {

    });

    loadIncidentInfo();
});

$(function () {
    $('[data-toggle="tooltip"]').tooltip();
});



function loadIncidentInfo() {
    var intTimeReload = $('#reloadTimeInfoBoad').val();
    var inLoadInfoIncident = setInterval('loadInfoInCident($(\'#without_com\').is(\':checked\'))',intTimeReload*1000);
    setInterval('loadInCidentCase()',intTimeReload*1000);
    setInterval('loadRequests()',intTimeReload*1000);
    setInterval('getinfodata($(\'#without_com\').is(\':checked\'))',intTimeReload*1000);
    return [inLoadInfoIncident];
}

function loadInfoInCident(checkWithoutCom) {
    var csrf = $('#csrf').val();
    var data = [
        {name: 'X-CSRF-TOKEN',value: csrf },
        {name: 'isAjax',value: 1 },
        {name: 'checkWithoutCom',value: checkWithoutCom },
        {name: 'action',value: 'load-info' }
    ];

    $.ajax({
        url : 'incident_boad_info_form.php',
        type : "POST",
        dataType:"text",
        data : data,
        success : function (result){
            $('#content-info').html(result);
        }
    });
}

function loadInCidentCase() {
    var csrf = $('#csrf').val();
    var data = [
        {name: 'X-CSRF-TOKEN',value: csrf },
    ];

    $.ajax({
        url : 'incident_case_form.php',
        type : "POST",
        dataType:"text",
        data : data,
        success : function (result){
            $('.incident-reg').html(result);
            var idIncident = $('.hideRegRequest').val();
            $('.btnRegInfo').attr('onclick','loadModalInfoEdit(0,'+idIncident+')');
            if($('.infoBtnView').val() == 0){
            	$('.btnRegInfo').hide();
            }else{
            	$('.btnRegInfo').show();
            }

        	if($('.queryBtnView').val() == 0){
            	$('.btn-chat').hide();
            }else{
            	$('.btn-chat').show();
            }

        	if($('.annoBtnView').val() == 0){
        		$('input[name="btnRegAnnounce"]').hide();
            }else{
            	$('input[name="btnRegAnnounce"]').show();
            }

        }
    });
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


function pin(intCatNo,intComNo,intType) { //Type = 1 => Insert, type  = 0 =>> delete
    var csrf = $('#csrf').val();
    var data = [
        {name: 'X-CSRF-TOKEN',value: csrf },
        {name: 'intCatNo',value: intCatNo },
        {name: 'intComNo',value: intComNo },
        {name: 'intType',value: intType },
        {name: 'action',value: 'pin-info' }
    ];

    $.ajax({
        url : 'incident_boad_ajax.php',
        type : "POST",
        dataType:"JSON",
        data : data,
        success : function (result){
            if(result.success){
                var checkWithoutCom = $('#without_com').is(':checked'); // this gives me null
                loadInfoInCident(checkWithoutCom);
            }
        }
    });
}


$(document).ready(function() {
    $('#without_com').change(function() {
        var checkWithoutCom = $(this).is(':checked'); // this gives me null
        getinfodata(checkWithoutCom);
        loadInfoInCident(checkWithoutCom);
    });

});

function getinfodata(checkWithoutCom) {
    var csrf = $('#csrf').val();
    var data = [
        {name: 'checkWithoutCom',value: checkWithoutCom },
        {name: 'X-CSRF-TOKEN',value: csrf },
        {name: 'isAjax',value: 1 },
        {name: 'action',value: 'long-form' }
    ];

    $.ajax({
        url : "incident_boad_info_long_form.php",
        type : "POST",
        dataType:"text",
        data : data,
        success : function (result){
            $('#long_form').html(result);
        }
    });
}

