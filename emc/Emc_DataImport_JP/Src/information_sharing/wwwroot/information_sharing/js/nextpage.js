function load_ajax(id){
    $.ajax({
        url : "announce_view.html",
        type : "GET",
        // type : "POST",
        dataType:"text",
        data : {
            textTitle : $('#title-source').val(),
            idChat : id,
        },
        success : function (result){
            $('#modal-body').html(result);
        }
    });
}

function loadModal(modal)
{
    $('#modal-body').html('');
    $('#myModal').modal({
            backdrop: 'static'
    });

    $.ajax({
        url : modal,
        //type : "GET",
        type : "POST",
        dataType:"text",
        data : {
            textTitle : $('#title-source').val(),
        },
        success : function (result){
            $('#modal-body').html(result);
        }
    });
}

function loadView(modal,id)
{
    $('#modal-body').html('');
    $('#myModal').modal({
            backdrop: 'static'
    });
    var csrf = $('#csrf').val();
    var data = [
        {name: 'id',value: id },
        {name: 'X-CSRF-TOKEN',value: csrf },
        {name: 'isAjax',value: 1 }
    ];

    $.ajax({
        url : modal,
        type : "POST",
        dataType:"text",
        data : data,
        success : function (result){
            $('#modal-body').html(result);
        }
    });
}

function loadViewBull(id,screen)
{
    $('#modal-body').html('');
    $('#myModal').modal({
            backdrop: 'static'
    });
    var csrf = $('#csrf').val();
    var data = [
        {name: 'id',value: id },
        {name: 'X-CSRF-TOKEN',value: csrf },
        {name: 'isAjax',value: 1 },
        {name: 'screen',value: screen }
    ];

    $.ajax({
        url : 'bulletin_board_view.php',
        type : "POST",
        dataType:"text",
        data : data,
        success : function (result){
            $('#modal-body').html(result);
        }
    });
}

function loadModalAnonView(id,type)
{
    $('#modal-body').html('');
    $('#myModal').modal({
            backdrop: 'static'
    });
    var csrf = $('#csrf').val();
    var data = [
        {name: 'id',value: id },
        {name: 'X-CSRF-TOKEN',value: csrf },
        {name: 'isAjax',value: 1 },
        {name: 'type',value: type}
    ];

    $.ajax({
        url : 'announce_view.php',
        type : "POST",
        dataType:"text",
        data : data,
        success : function (result){
            $('#modal-body').html(result);
        }
    });
}

function loadModalIncident(id)
{
    $('#modal-body').html('');
    $('#myModal').modal({
            backdrop: 'static'
    });
    var csrf = $('#csrf').val();
    var data = [
        {name: 'id',value: id },
        {name: 'X-CSRF-TOKEN',value: csrf },
        {name: 'isAjax',value: 1 },
        {name: 'screen',value: 'portal' }
    ];

    $.ajax({
        url : 'incident_case_view.php',
        type : "POST",
        dataType:"text",
        data : data,
        success : function (result){
            $('#modal-body').html(result);
        }
    });
}

function loadModalAnnonEdit(id,dataId)
{
    $('#modal-body').html('');
    $('#myModal').modal({
            backdrop: 'static'
    });
    var csrf = $('#csrf').val();
    var data = [
        {name: 'id',value: id },
        {name: 'dataId',value: dataId },
        {name: 'X-CSRF-TOKEN',value: csrf },
        {name: 'isAjax',value: 1 }
    ];

    $.ajax({
        url : 'announce_edit.php',
        type : "POST",
        dataType:"text",
        data : data,
        success : function (result){
            $('#modal-body').html(result);
        }
    });
}

function loadModalInfoEdit(id,dataId)
{
    $('#modal-body').html('');
    $('#myModal').modal({
            backdrop: 'static'
    });
    var csrf = $('#csrf').val();
    var data = [
        {name: 'id',value: id },
        {name: 'dataId',value: dataId },
        {name: 'X-CSRF-TOKEN',value: csrf },
        {name: 'isAjax',value: 1 }
    ];

    $.ajax({
        url : 'information_edit.php',
        type : "POST",
        dataType:"text",
        data : data,
        success : function (result){
            $('#modal-body').html(result);
        }
    });
}
function loadModalReqOnEdit(id,dataId)
{
    $('#modal-body').html('');
    $('#myModal').modal({
            backdrop: 'static'
    });
    var csrf = $('#csrf').val();
    var data = [
        {name: 'id',value: id },
        {name: 'dataId',value: dataId },
        {name: 'X-CSRF-TOKEN',value: csrf },
        {name: 'isAjax',value: 1 }
    ];

    $.ajax({
        url : 'request_edit.php',
        type : "POST",
        dataType:"text",
        data : data,
        success : function (result){
            $('#modal-body').html(result);
        }
    });
}

function loadChat(modal) {
    var intTimeReload = $('#reloadTime').val();
    var inLoadChatForm = setInterval('loadChatForm(1)',intTimeReload*1000);
    $('#modal-body').html('');
    $('#myModal').modal({
            backdrop: 'static'
    });

    var radioValue = $("input[name='gettime']:checked").val();
    var csrf = $('#csrf').val();
    var data = [
        {name: 'typeOfTime',value: radioValue },
        {name: 'X-CSRF-TOKEN',value: csrf },
        {name: 'fromBtn',value: 1 },
    ];

    $.ajax({
        url : modal,
        // type : "GET",
        type : "POST",
        dataType:"text",
        data : data,
        success : function (result){
            $('#modal-body').html(result);
            $('#modal-body').find('.msg_history').addClass('query_message');
            var msg = 'query_message';
            doResize(msg,52);
            $('.msg_history').stop().animate({
                scrollTop: $('.msg_history')[0].scrollHeight
            }, 200);
        }
    });

}


$( document ).ready(function() {
    loadInval();
    $('.selAnnounceTime').on('change', function() {
        var intId = $(this).attr('data-id');
        loadInfo(intId);
    });

    $("input[name='gettime']").on('change', function() {
        loadChatFormPortal(1);
    });

});
function loadInval() {
    var intTimeReload = $('#reloadTimeInfoBoad').val();
    var intTimeReloadQuery = $('#queryReloadTimePortal').val();
    var inLoadInfo = setInterval('loadInfo(1)',intTimeReload*1000);
    var inLoadBull = setInterval('loadBull()',intTimeReload*1000);
    var inLoadChatForm = setInterval('loadChatFormPortal()',intTimeReloadQuery*1000);
    var inLoadIncident = setInterval('loadIncident()',intTimeReload*1000);
    return [inLoadInfo,inLoadBull,inLoadChatForm,inLoadIncident];
}


function loadChatFormPortal(intFlagChangeTime) {
    var radioValue = $("input[name='gettime']:checked").val();
    var csrf = $('#csrf').val();
    var data = [
        {name: 'typeOfTime',value: radioValue },
        {name: 'X-CSRF-TOKEN',value: csrf },
        {name: 'isAjax',value: 1 },
        {name: 'intFlagChangeTime',value: intFlagChangeTime }
    ];

    $.ajax({
        url : 'msg_history_form.php',
        type : "POST",
        dataType:"text",
        data : data,
        success : function (result){
            if(intFlagChangeTime == 1){
                $('.msg_history').html(result);
                $('.msg_history').stop().animate({
                    scrollTop: $('.msg_history')[0].scrollHeight
                }, 10);
            }else{
                if(result.length > 2){
                    $('.msg_history').append(result);
                    $('.msg_history').stop().animate({
                        scrollTop: $('.msg_history')[0].scrollHeight
                    }, 10);
                }
            }
        }
    });
}

function loadInfo(intNumberTab) {
    var csrf = $('#csrf').val();
    var selectedDate = $("#selTimeAnnouce"+intNumberTab).children("option:selected").val();
    if(intNumberTab == 1){
        $("#selTimeAnnouce2").val(selectedDate);
    }else{
        $("#selTimeAnnouce1").val(selectedDate);
    }
    var data = [
        {name: 'X-CSRF-TOKEN',value: csrf },
        {name: 'action',value: 'load_info' },
        {name: 'dtmDateFilter',value: selectedDate }
    ];

    $.ajax({
        url : 'info_form_content.php',
        type : "POST",
        dataType:"text",
        data : data,
        success : function (result){
            if(result){
                $('.info-cont').html(result);
            }
        }
    });
}
function loadIncident() {
    var csrf = $('#csrf').val();
    var data = [
        {name: 'X-CSRF-TOKEN',value: csrf },
        {name: 'action',value: 'load-incident' },
    ];

    $.ajax({
        url : 'incident_boad_ajax.php',
        type : "POST",
        dataType:"JSON",
        data : data,
        success : function (result){
            // var statusContent2 = $('#content2').css('display');
            var statusTab2 = $('.el-tab-2').css('display');
            if(parseInt(result.intIncident) > 0){
                if(statusTab2 == 'none'){
                    alert(result.strNew);
                }
                $('.el-tab-2').show();
            }else{
            	var blnMapReflesh = false;
                if(statusTab2 != 'none'){
                	if(result.intJcmgTabPerm != '0'){
                    	alert(result.strDone);
                    	blnMapReflesh = true;
                    }
                }
                $("#tab1").prop("checked", true);
                $('.el-tab-2').hide();
                
                if(blnMapReflesh == true){
                	mapRefresh();
                }
            }

            if(result.intShowBtn == 1){
                $('#span-btnSearch').show();
            }else{
                $('#span-btnSearch').hide();
            }
            $('#timeJST').html(result.strJstTime);
            $('#timeUTC').html(result.strUtcTime);
        }
    });
}

function loadBull() {
    var csrf = $('#csrf').val();
    var data = [
        {name: 'X-CSRF-TOKEN',value: csrf },
        {name: 'action',value: 'load_bull' }
    ];

    $.ajax({
        url : 'bull_form.php',
        type : "POST",
        dataType:"text",
        data : data,
        success : function (result){
            if(result){
                $('#bull_form').html(result);
            }
        }
    });
}


function doResize(divId,pecent) {
    var xper = 1;
    var vph = $(window).height();
    // var vph = screen.height;

    if(100<vph && vph<200){
        xper = 3;
    }else if(200<vph && vph<300){
        xper = 2.9;
    }else if(300<vph && vph<350){
        xper = 2.8;
    }else if(350<=vph && vph<380){
        xper = 2.65;
    }else if(380<=vph && vph<400){
        xper = 2.5;
    }else if(400<=vph && vph<410){
        xper = 2.45;
    }else if(410<=vph && vph<420){
        xper = 2.4;
    }else if(420<=vph && vph<430){
        xper = 2.3;
    }else if(430<=vph && vph<440){
        xper = 2.2;
    }else if(440<=vph && vph<450){
        xper = 2;
    }else if(450<=vph && vph<470){
        xper = 1.9;
    }else if(470<=vph && vph<480){
        xper = 1.8;
    }else if(480<=vph && vph<500){
        xper = 1.7;
    }else if(500<=vph && vph<550){
        xper = 1.65;
    }else if(550<=vph && vph<600){
        xper = 1.5;
    }else if(600<=vph && vph<620){
        xper = 1.45;
    }else if(620<=vph && vph<650){
        xper = 1.35;
    }else if(650<=vph && vph<700){
        xper = 1.24;
    }else if(700<vph && vph<750){
        xper = 1.17;
    }else if(750<=vph && vph<767){
        xper = 1.16;
    }else if(768<=vph && vph<800){
        xper = 1.1;
    }else if(800<=vph && vph<830) {
        xper = 1.08;
    }else if(830<=vph && vph<850) {
        xper = 1.06;
    }else if(850<=vph && vph<950) {
        xper = 1.01;
    }else if(950<=vph && vph<1060) {
        xper = 1;
    }else{
        xper = 1;
    }

    heighDiv = vph*pecent/(100*xper);
    $('.'+divId).css({'height': heighDiv + 'px'});
}


function doResizeTable(divId,pecent) {
    var xper = 1;
    var vph = $(window).height();
    // var vph = screen.height;

    if(100<vph && vph<200){
        xper = 3;
    }else if(200<vph && vph<300){
        xper = 2.9;
    }else if(300<vph && vph<350){
        xper = 2.8;
    }else if(350<=vph && vph<380){
        xper = 2.65;
    }else if(380<=vph && vph<400){
        xper = 2.5;
    }else if(400<=vph && vph<410){
        xper = 2.45;
    }else if(410<=vph && vph<420){
        xper = 2.4;
    }else if(420<=vph && vph<430){
        xper = 2.3;
    }else if(430<=vph && vph<440){
        xper = 2.2;
    }else if(440<=vph && vph<450){
        xper = 2;
    }else if(450<=vph && vph<470){
        xper = 1.9;
    }else if(470<=vph && vph<480){
        xper = 1.8;
    }else if(480<=vph && vph<500){
        xper = 1.7;
    }else if(500<=vph && vph<550){
        xper = 2;
    }else if(550<=vph && vph<600){
        xper = 1.8;
    }else if(600<=vph && vph<620){
        xper = 1.7;
    }else if(620<=vph && vph<650){
        xper = 1.6;
    }else if(650<=vph && vph<700){
        xper = 1.4;
    }else if(700<vph && vph<750){
        xper = 1.3;
    }else if(750<=vph && vph<800){
        xper = 1.3;
    }else if(800<=vph && vph<830) {
        xper = 1.15;
    }else if(830<=vph && vph<850) {
        xper = 1.11;
    }else if(850<=vph && vph<900) {
        xper = 1.07;
    }else if(900<=vph && vph<950) {
        xper = 1.08;
    }else{
        xper = 1;
    }
// Chrome 1 - 79
/*    var isChrome = !!window.chrome && (!!window.chrome.webstore || !!window.chrome.runtime);
    if(isChrome == true){
        xper = xper*1.377;
    }*/

    heighDiv = vph*pecent/(100*xper);
    $('.'+divId).css({'height': heighDiv + 'px'});
}



