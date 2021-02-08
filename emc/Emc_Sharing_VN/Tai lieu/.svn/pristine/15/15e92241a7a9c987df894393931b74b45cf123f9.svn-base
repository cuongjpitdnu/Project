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
function edit(id)
{
    $.ajax({
        url : "announce_edit.html",
        type : "GET",
        dataType:"text",
        data : {
            idChat : id
        },
        success : function (result){
            $('#modal-body').html(result);
        }
    });
}


function loadModal(modal)
{
    $('#myModal').modal({
        backdrop: 'static'
    });

    $.ajax({
        url : modal,
        type : "GET",
        // type : "POST",
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
    $('#myModal').modal({
        backdrop: 'static'
    });

    $.ajax({
        url : modal,
        type : "GET",
        // type : "POST",
        dataType:"text",
        data : {
            id : id,
        },
        success : function (result){
            $('#modal-body').html(result);
        }
    });
}
function loadEdit(modal,id)
{
    $('#myModal').modal({
        backdrop: 'static'
    });

    $.ajax({
        url : modal,
        type : "GET",
        // type : "POST",
        dataType:"text",
        data : {
            id : id,
        },
        success : function (result){
            $('#modal-body').html(result);
        }
    });
}
function loadModalAnonView(id)
{
    $('#myModal').modal({
        backdrop: 'static'
    });

    $.ajax({
        url : 'announce_view.php',
        type : "GET",
        // type : "POST",
        dataType:"text",
        data : {
            annonId : id,
        },
        success : function (result){
            $('#modal-body').html(result);
        }
    });
}


function loadChat(modal) {
    $('#myModal').modal({
        backdrop: 'static'
    });

    var radioValue = $("input[name='gettime']:checked").val();
    $.ajax({
        url : modal,
        type : "GET",
        // type : "POST",
        dataType:"text",
        data : {
            typeOfTime : radioValue,
        },
        success : function (result){
            $('#modal-body').html(result);
        }
    });
}



