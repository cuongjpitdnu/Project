function load_ajax(id){
    $.ajax({
        url : "announce_edit.php",
        type : "POST",
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
    console.log(id);
    $.ajax({
        url : "announce_edit.php",
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

