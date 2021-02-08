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



function doResize(divId,pecent) {
    var xper = 1;
    var vph = $(window).height();

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
    }else if(750<=vph && vph<800){
        xper = 1.11;
    }else if(800<=vph && vph<830) {
        xper = 1.08;
    }else if(830<=vph && vph<850) {
        xper = 1.06;
    }else if(850<=vph && vph<900) {
        xper = 1.04;
    }else if(900<=vph && vph<950) {
        xper = 1.02;
    }else{
        xper = 1;
    }

    heighDiv = vph*pecent/(100*xper);
    console.log('divId'+divId);
    console.log('xper'+xper);
    console.log('sum'+vph);
    console.log('div'+ heighDiv);
    $('.'+divId).css({'height': heighDiv + 'px'});
}


function doResizeTable(divId,pecent) {
    var xper = 1;
    var vph = $(window).height();

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
        xper = 1.5;
    }else if(700<vph && vph<750){
        xper = 1.3;
    }else if(750<=vph && vph<800){
        xper = 1.2;
    }else if(800<=vph && vph<830) {
        xper = 1.15;
    }else if(830<=vph && vph<850) {
        xper = 1.11;
    }else if(850<=vph && vph<900) {
        xper = 1.07;
    }else if(900<=vph && vph<950) {
        xper = 1.04;
    }else{
        xper = 1;
    }

    heighDiv = vph*pecent/(100*xper);
    console.log('divId'+divId);
    console.log('xper'+xper);
    console.log('sum'+vph);
    console.log('div'+ heighDiv);
    $('.'+divId).css({'height': heighDiv + 'px'});
}



