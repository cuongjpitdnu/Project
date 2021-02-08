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



