<div class="clearfix">
    <div class="in-line tcol-md-6" style="float: left">
        <div class="clearfix">
            <div class="in-line float-left title text-yellow">
                <div>関西エアポートからのお知らせ/</div>
                <div>Informattion</div>
            </div>
            <div class="in-line col-right">
                <input type="button" class="tbtn tbtn-none right-20" onclick="loadModal('announce_edit.html')" value="新規登録">
                <div class="select-container">
                    <select class="select-150 ">
                        <option>2020/01</option>
                        <option>2019/12</option>
                        <option>2019/11</option>
                        <option>2019/10</option>
                        <option>2019/09</option>
                        <option>2019/08</option>
                        <option>2019/07</option>
                        <option>2019/06</option>
                        <option>2019/05</option>
                        <option>2019/04</option>
                        <option>2019/03</option>
                        <option>2019/02</option>
                        <option>2019/01</option>
                    </select>
                </div>
            </div>
        </div>
        <div class="cont-table ">
            <div class="t-row">
                <div class="t-col-md-3">日時</div>
                <div class="t-col-md-3">タイトル</div>
                <div class="t-col-md-3">ファイル</div>
            </div>
            <div class="info-cont">
                <div class="t-row">
                    <div class="t-col-md-3">10/23 09:00</div>
                    <div class="t-col-md-3"><a href="#" id="btn1">計画停電について</a></div>
                    <div class="t-col-md-3">■</div>
                </div>

                <div class="t-row">
                    <div class="t-col-md-3">10/XX XX:XX 　</div>
                    <div class="t-col-md-3">〇〇〇</div>
                    <div class="t-col-md-3">-</div>
                </div>

                <div class="t-row">
                    <div class="t-col-md-3">10/XX XX:XX 　</div>
                    <div class="t-col-md-3">〇〇〇</div>
                    <div class="t-col-md-3">-</div>
                </div>
                <div class="t-row">
                    <div class="t-col-md-3">10/23 09:00</div>
                    <div class="t-col-md-3"><a href="#" id="btn1">計画停電について</a></div>
                    <div class="t-col-md-3">■</div>
                </div>

                <div class="t-row">
                    <div class="t-col-md-3">10/XX XX:XX 　</div>
                    <div class="t-col-md-3">〇〇〇</div>
                    <div class="t-col-md-3">-</div>
                </div>

                <div class="t-row">
                    <div class="t-col-md-3">10/XX XX:XX 　</div>
                    <div class="t-col-md-3">〇〇〇</div>
                    <div class="t-col-md-3">-</div>
                </div>
            </div>
        </div>
        <img src="img/kix.jpg" alt="" width="100%" class="top-20 map-img" >

        <div class="title text-yellow mar-tl-10">掲示板</div>

        <div class="cont-table">
            <div class="t-row">
                <div class="t-col-md-3">発生日時</div>
                <div class="t-col-md-3">業務名</div>
                <div class="t-col-md-3">インシデント件名</div>
            </div>
            <div class="t-body-table">
                <div class="t-row">
                    <div class="t-col-md-3">10/02 12:00</div>
                    <div class="t-col-md-3">急患等</div>
                    <div class="t-col-md-3"><a href="#" onclick="loadModal('bulletin_board_view.html')">急病者発生</a></div>
                </div>
                <div class="t-row">
                    <div class="t-col-md-3">10/02 10:40</div>
                    <div class="t-col-md-3">迷惑行為</div>
                    <div class="t-col-md-3"><a href="#" onclick="loadModal('bulletin_board_view.html')" >迷惑行為その１</a></div>
                </div>
                <div class="t-row">
                    <div class="t-col-md-3">10/02 09:35</div>
                    <div class="t-col-md-3">土木・道路</div>
                    <div class="t-col-md-3"><a href="#" onclick="loadModal('bulletin_board_view.html')" >路面剥離</a></div>
                </div>
                <div class="t-row">
                    <div class="t-col-md-3">10/02 09:35</div>
                </div>
                <div class="t-row">
                    <div class="t-col-md-3">10/02 09:35</div>
                </div>
                <div class="t-row">
                    <div class="t-col-md-3">10/02 09:35</div>
                </div>
                <div class="t-row">
                    <div class="t-col-md-3">10/02 09:35</div>
                </div>
                <div class="t-row">
                    <div class="t-col-md-3">10/02 09:35</div>
                </div>
                <div class="t-row">
                    <div class="t-col-md-3">10/02 09:35</div>
                </div>
            </div>
        </div>
    </div>

    <div class="in-line tcol-md-6 float-left">
        <div class="pad-left-20">
            <div class="title text-yellow">関係機関からの問い合わせ</div>
            <div class="chat-cont">
                <div class="mesgs"></div>
                <div class="chat-control mar-tl-10">
                    <div class="in-line mar-tl-10">
                        全表示 <input type="radio" name="gettime" value="0" checked>
                        <span class="pad-left-20">24時間表示</span><input type="radio" name="gettime" value="1">
                    </div>
                    <div class="in-line col-right">
                        <button class="tbtn tbtn-defaut" onclick="loadChat('query_view.php')">問い合わせ</button>
                    </div>
                </div>
            </div>

            <div class="title text-yellow mar-tl-10">各社へのリンク情報</div>

            <div class="cont-table">
                <div class="t-row">
                    <div class="t-col-md-3 title text-green">　気象/Weather</div>
                    <div class="t-col-md-3 title text-green">交通/Traffic</div>
                    <div class="t-col-md-3 title text-green">フライト/Flight info.</div>
                </div>
                <div class="t-body-table">
                    <div class="t-row">
                        <div class="t-col-md-3"><a href="http://google.com" target="_blank">飛行場実況気象(Metar)</a></div>
                        <div class="t-col-md-3"><a href="http://google.com" target="_blank">JR西日本</a></div>
                        <div class="t-col-md-3"><a href="http://google.com" target="_blank">本日のフライト情報</div>
                    </div>

                    <div class="t-row">
                        <div class="t-col-md-3"><a href="http://google.com" target="_blank">飛行場予報（TAF）</a></div>
                        <div class="t-col-md-3"><a href="http://google.com" target="_blank">南海電鉄</a></div>
                        <div class="t-col-md-3"></div>
                    </div>
                    <div class="t-row">
                        <div class="t-col-md-3"><a href="http://google.com" target="_blank">気象庁情報</a></div>
                        <div class="t-col-md-3"><a href="http://google.com" target="_blank">NEXCO</a></div>
                        <div class="t-col-md-3"></div>
                    </div>
                    <div class="t-row">
                        <div class="t-col-md-3">&nbsp;</div>
                        <div class="t-col-md-3"><a href="http://google.com" target="_blank">　南海バス</a></div>
                        <div class="t-col-md-3"></div>
                    </div>
                    <div class="t-row">
                        <div class="t-col-md-3">&nbsp;</div>
                        <div class="t-col-md-3"><a href="http://google.com" target="_blank">OMこうべ</a></div>
                        <div class="t-col-md-3"></div>
                    </div>
                    <div class="t-row">
                        <div class="t-col-md-3">&nbsp;</div>
                        <div class="t-col-md-3"><a href="http://google.com" target="_blank">関西空港交通</a></div>
                        <div class="t-col-md-3"></div>
                    </div>
                    <div class="t-row">
                        <div class="t-col-md-3">&nbsp;</div>
                        <div class="t-col-md-3"><a href="http://google.com" target="_blank">　南海バス</a></div>
                        <div class="t-col-md-3"></div>
                    </div>
                    <div class="t-row">
                        <div class="t-col-md-3">&nbsp;</div>
                        <div class="t-col-md-3"><a href="http://google.com" target="_blank">OMこうべ</a></div>
                        <div class="t-col-md-3"></div>
                    </div>
                    <div class="t-row">
                        <div class="t-col-md-3">&nbsp;</div>
                        <div class="t-col-md-3"><a href="http://google.com" target="_blank">関西空港交通</a></div>
                        <div class="t-col-md-3"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>


<script>
    $(".mesgs").load("msg_history.php");
</script>