function postQuery()
{
    $('#er-source').hide();
    $('#er-character').hide();
    $('#er-target').hide();
    var source = $('#txt-source').val();
    var sleTran = $('#sle-tran').val();
    var ckeckMan = $('#ckeck-manunal').is(":checked");
    var target = $('#txt-target').val();
    var csrf = $('#csrf').val();
    var permError = $('#permError').val();
    var data = [
        {name: 'txtSource',value: source },
        {name: 'sleTran',value: sleTran },
        {name: 'ckeckMan',value: ckeckMan },
        {name: 'txtTarget',value: target },
        {name: 'action',value: 'insert_query'},
        {name: 'X-CSRF-TOKEN',value: csrf },
        {name: 'isAjax',value: 1 }
    ];
    $.ajax({
        url : 'query_view_proc.php',
        type : "POST",
        dataType:"JSON",
        data : data,
        success : function (result){
        	if(result == 900){
				alert(permError);
				window.location.href="login.php";
                return;
			}

            if(result == '1'){
                $('#txt-source').val('');
                $('#txt-target').val('');
                loadChatForm(1);
                $('.msg_history').stop().animate({
                    scrollTop: $('.msg_history')[0].scrollHeight
                }, 1);
            }else{
                $.each(result.error, function( index, value ) {
                    if(index == 'message'){
                        alert(value);
                    }else{
                        $('#'+index).html(value);
                        $('#'+index).show();
                    }
                });
            }
        }
    });
}

function loadChatForm(intFlagQuery) {
    var radioValue = $("input[name='gettime']:checked").val();
    var csrf = $('#csrf').val();
    var data = [
        {name: 'typeOfTime',value: radioValue },
        {name: 'X-CSRF-TOKEN',value: csrf },
        {name: 'isAjax',value: 1 },
        {name: 'intFlagQuery',value: intFlagQuery },
    ];
    $.ajax({
        url : 'msg_history_form.php',
        type : "POST",
        dataType:"text",
        data : data,
        success : function (result){
            if(result.length > 2){
                $('.msg_history').append(result);
                $('.msg_history').stop().animate({
                    scrollTop: $('.msg_history')[0].scrollHeight
                }, 1);
            }
        }
    });
}

// var reloadTime = $('#reloadTime').val();
// setInterval('loadChatForm()',reloadTime*1000);

$('#ckeck-manunal').change(function() {
    if(this.checked) {
        $('#txt-target').prop("disabled", false);
        $('#btn-tran').prop("disabled", true);
    }else{
        $('#txt-target').prop("disabled", true);
        $('#btn-tran').prop("disabled", false);
    }
});

$('#btn-tran').click(function() {
    $('#er-source').hide();
    $('#er-character').hide();
    $('#er-target').hide();
    var source = $('#txt-source').val();
    var sleTran = $('#sle-tran').val();
    var ckeckMan = $('#ckeck-manunal').is(":checked");
    var target = $('#txt-target').val();
    var csrf = $('#csrf').val();
    var permError = $('#permError').val();
    var data = [
        {name: 'txtSource',value: source },
        {name: 'sleTran',value: sleTran },
        {name: 'ckeckMan',value: ckeckMan },
        {name: 'txtTarget',value: target },
        {name: 'action',value: 'tran'},
        {name: 'X-CSRF-TOKEN',value: csrf },
        {name: 'istran',value: 1 }
    ];
    $.ajax({
        url : 'tran_proc.php',
        type : "POST",
        dataType:"JSON",
        data : data,
        success : function (result){
        	if(result == 900){
				alert(permError);
				window.location.href="login.php";
                return;
			}

            if(result.error){
                $.each(result.error, function( index, value ) {
                    if(index == 'message'){
                        alert(value);
                    }else{
                        $('#'+index).html(value);
                        $('#'+index).show();
                    }
                });
            }else{
                var obj = document.getElementById('txt-target');
                obj.value = result;
            }
        }
    });
});