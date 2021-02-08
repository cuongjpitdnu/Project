<?php
require_once ('incident_boad_ajax.php');

//▼2020/06/01 KBS S.Tasaki JCMGボード権限がない場合は取得データを表示しない。
if($objLoginUserInfo->intJcmgTabPerm != 1){
    $arrInfoData = array();
    $arrInfoNotReg = array();
}
//▲2020/06/01 KBS S.Tasaki

//▼2020/06/01 KBS S.Tasaki リクエストの改ざんによりデータが正常に取得できたかった場合の対策
if(!is_array($arrInsCategory)){
    $arrInsCategory = array();
}
//▲2020/06/01 KBS S.Tasaki

?>
<tr>
    <?php
    $arrInsId = array();
    foreach ($arrInsCategory as $intKey => $arrCat){
        $arrInsId[] = $arrCat['INST_CATEGORY_NO'];
        $intInsCateNo = $arrCat['INST_CATEGORY_NO'];
        echo '<td>';
        echo '<div class="col-ticker" id="col-ticker-'.$intInsCateNo.'">';
        echo '<div class="pink-ticker" id="pink-ticker-'.$intInsCateNo.'">';
        //if have data will loop data and show it
        if(@$arrInfoData[$intInsCateNo]){
            foreach ($arrInfoData[$intInsCateNo] as $intCatNo =>  $arrItem){
                echo '<div class="ticker clearfix">';
                echo '<div class="row-no-gutters clearfix">';
                echo '<div class="col-md-11 title title-incident"
                title="'.fncHtmlSpecialChars($arrCompany[$intCatNo]).'"
                data-fhd="'.($arrText['FHD_COMPANY_INST']).'" data-hd="'.($arrText['COMPANY_INST']).'"
                >'.fncHtmlSpecialChars($arrCompany[$intCatNo]).'</div>';
                echo '<div class="col-md-1 text-right"><a href="#" onclick="pin('.$intInsCateNo.','.$arrItem[0]['COMPANY_NO'].')">
                                <img src="img/sticker-pink.jpg"></a>
                                </div>';
                echo '</div>';
                echo '<div class="sub-title-incident clearfix"> <span class="col-md-6">'.$arrText['PORTAL_INCIDENT_TEXT_014'].'</span><span class="col-md-6 ">'.$arrText['PORTAL_INCIDENT_TEXT_015'].'</span></div>';
                echo '<div class="cont-ticker">';
                foreach ($arrItem as $arrInfoOfCom){
                    $strTitle = $arrInfoOfCom['TITLE'.$strEndLang];
                    //if not exist text, set text with other language type
                    if(!$strTitle){
                        $intUntra = 1;
                        // check language type
                        if(!$objLoginUserInfo->intLanguageType){
                            $strTitle = $arrInfoOfCom['TITLE_ENG'];
                        }else{
                            $strTitle = $arrInfoOfCom['TITLE_JPN'];
                        }
                    }
                    $dtmDateInfo = date('m/d H:i',strtotime($arrInfoOfCom['UP_DATE']));
                    echo '<div class="">';
                    echo '<span class="col-md-6 col-sm-6">'.$dtmDateInfo.'</span>';
                    echo '<span class="col-md-6 col-sm-6">';
                    echo '<a href="#" title="'.fncHtmlSpecialChars($strTitle).'"
                                            title="'.fncHtmlSpecialChars($strTitle).'"
                                            data-fhd="'.$arrText['FHD_TITLE_INST'].'" data-hd="'.$arrText['TITLE_INST'].'"
                                            onclick="loadView(\'information_view.php\','.$arrInfoOfCom['INFORMATION_NO'].')"
                                            class="tl-red-boad title-info">'.fncHtmlSpecialChars($strTitle).'</a>';
                    echo '</span>';
                    echo '</div>';
                }
                echo '</div>';
                echo '</div>';
            }
        }
        echo '</div>';

        echo '<div class="tickers" id="white-ticker-'.$intInsCateNo.'">';
        //if have data will loop data and show it
        if(@$arrInfoNotReg[$intInsCateNo]){
            foreach ($arrInfoNotReg[$intInsCateNo] as $intCatNo =>  $arrItem){
                echo '<div class="ticker">';
                echo '<div class="row-no-gutters clearfix">';
                echo '<div class="col-md-11 title title-incident"
                title="'.fncHtmlSpecialChars($arrCompany[$intCatNo]).'"                
                data-fhd="'.$arrText['FHD_COMPANY_INST'].'" data-hd="'.$arrText['COMPANY_INST'].'"
                >'.fncHtmlSpecialChars($arrCompany[$intCatNo]).'</div>';
                echo '<div class="col-md-1 text-right"><a href="#" class="pin-white" onclick="pin('.$intInsCateNo.','.$arrItem[0]['COMPANY_NO'].',1)">
                                <img src="img/sticker-white.jpg"></a></div></div>';
                echo '<div class="sub-title-incident clearfix"> <span class="col-md-6">'.$arrText['PORTAL_INCIDENT_TEXT_014'].'</span><span class="col-md-6">'.$arrText['PORTAL_INCIDENT_TEXT_015'].'</span></div>';
                echo '<div class="cont-ticker">';
                foreach ($arrItem as $arrInfoOfCom){
                    $strTitle = $arrInfoOfCom['TITLE'.$strEndLang];
                    //if not exist text, set text with other language type
                    if(!$strTitle){
                        $intUntra = 1;
                        if(!$objLoginUserInfo->intLanguageType){
                            $strTitle = $arrInfoOfCom['TITLE_ENG'];
                        }else{
                            $strTitle = $arrInfoOfCom['TITLE_JPN'];
                        }
                    }
                    $dtmDateInfo = date('m/d H:i',strtotime($arrInfoOfCom['UP_DATE']));

                    echo '<div class="">';
                    echo '<span class="col-md-6 col-sm-6">'.$dtmDateInfo.'</span>';
                    echo '<span class="col-md-6 col-sm-6"> <a href="#" 
                                            data-fhd="'.$arrText['FHD_TITLE_INST'].'" data-hd="'.$arrText['TITLE_INST'].'"
                                                title="'.fncHtmlSpecialChars($strTitle).'" onclick="loadView(\'information_view.php\','.$arrInfoOfCom['INFORMATION_NO'].')"
                                                class="tl-red-boad title-info">'.fncHtmlSpecialChars($strTitle).'</a></span>';
                    echo '</div>';
                }
                echo '</div>';
                echo '</div>';
            }
        }
        echo '</div>';
        echo '</div>';
        echo '</td>';
    }
    ?>
</tr>

<script>
    showTitle('title-info');
    showTitle('title-incident');

    function showTitle(className){
        $('.'+className).each(function(){
            // var widthSc = screen.width;
            var widthSc = screen.width;
            var heighSc = screen.height;

            var tit = this.title;
            var lengHd = $('.'+className).attr('data-hd');
            var lengFHD = $('.'+className).attr('data-fhd');

            if(widthSc >= 1920 && heighSc >= 1080){
                var strTit = tit.substring(0,lengFHD);
            }else{
                strTit = tit.substring(0,lengHd);
            }
            $(this).text(strTit);
        });
    }

    $( document ).ready(function() {
        setHeigh();
    });

    function setHeigh(){
        var colTicker = 'col-ticker';
        doResizeTable(colTicker,52);

        var winHeight = $(window).height();
        var ticker = 'cont-ticker';
        if(winHeight < 700){
            doResize(ticker,12);
        }else{
            doResize(ticker,7);
        }

        var arrIds = <?php echo json_encode($arrInsId); ?>;
        $.each(arrIds, function( index, value ) {
            var colTickerHeight  = $('.col-ticker').height();
            var colPinkTickerHeight  = $('#pink-ticker-'+value).height();
            if(colPinkTickerHeight > colTickerHeight){
                $('#pink-ticker-'+value).css({'height': colTickerHeight -2 + 'px'});
                $('#pink-ticker-'+value).css({'overflow': 'hidden'});
                $('#white-ticker-'+value).css({'height': 0 + 'px'});
            }else{
                var colWhiteTickerHeight  = colTickerHeight - colPinkTickerHeight -2 ;
                $('#white-ticker-'+value).css({'height': colWhiteTickerHeight + 'px'});
            }
        });
    }


    $(function () {
        $('[data-toggle="tooltip"]').tooltip();
    });


</script>