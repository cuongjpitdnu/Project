function postQuery()
{
    var tran = tranbeforeInsert();
    var source = $('#txt-source').val();
    var sleTran = $('#sle-tran').val();
    var ckeckMan = $('#ckeck-manunal').val();
    var target = $('#txt-target').val();

    if(tran == 1){
        $.ajax({
            url : 'msg_history_form.php',
            type : "POST",
            dataType:"text",
            data : {
                txtSource : source,
                sleTran : sleTran,
                ckeckMan : ckeckMan,
                txtTarget : target,
            },
            success : function (result){
                $('.msg_history').html(result);
            }
        });
    }
}


function loadChatForm() {
    var radioValue = $("input[name='gettime']:checked").val();
    $.ajax({
        url : 'msg_history_form.php',
        type : "GET",
        // type : "POST",
        dataType:"text",
        data : {
            typeOfTime : radioValue,
        },
        success : function (result){
            $('.msg_history').html(result);
        }
    });
}

setInterval('loadChatForm()',2000);

$('#ckeck-manunal').change(function() {
    if(this.checked) {
        $('#txt-target').prop("disabled", false);
    }else{
        $('#txt-target').prop("disabled", true);
    }
});

$( "#btn-tran" ).click(function() {
    translate();
});

function tranbeforeInsert() {
    if(!$('#ckeck-manunal').checked){
        console.log(translate());
        return translate();
    }else{
        return 1;
    }
}

function translate() {
    //AWS Access Key ID
    wkey1="AKIASVBKO6L4T3GB5VH6";

    //AWS Secret Access Key
    wkey2="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";

    //region
    wregion ="us-west-2";

    startTime1 = Date.now();

    a=document.getElementById( "txt-source" ).value ;

    AWS.config.region = wregion; // Region
    AWS.config.credentials = new AWS.Credentials(wkey1,wkey2);
    var translate = new AWS.Translate({region: AWS.config.region});
    var polly = new AWS.Polly();


    var p = $('#sle-tran').children("option:selected").val();

    //ì˙ñ{åÍÅ®âpåÍ
    if (p == 'je') {
        var params = {
            Text: a,
            SourceLanguageCode: "ja",
            TargetLanguageCode: "en"
        };
    }
    else {
        var params = {
            Text: a,
            SourceLanguageCode: "en",
            TargetLanguageCode: "ja"
        };
    }


    var tran = translate.translateText(params, function(err, data) {
        if (err) {
            console.log(err, err.stack);
            alert("Error calling Amazon Translate. " + err.message);
            return;
        }
        if (data) {
            var obj = document.getElementById('txt-target');
            obj.value = data.TranslatedText;
        }
    });
    if(tran.response.error == null){
        return 1;
    }else{
        return 0;
    }
}


//amazonñ|ñÛ






