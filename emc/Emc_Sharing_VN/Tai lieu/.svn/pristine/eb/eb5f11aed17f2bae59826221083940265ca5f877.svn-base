<html>
<body>
    <head>
    <script type="text/javascript">

    	window.onload = function(){
    		document.getElementById("cmpDate").style.visibility ="hidden";
    	}
		function fncCmb(){
			var cmbVal = document.test.status.selectedIndex


			if(document.test.status.options[cmbVal].value == 1){
				document.getElementById("cmpDate").style.visibility ="visible";
				document.getElementById("business").style.visibility  ="hidden";
				document.getElementById("incident").style.visibility  ="hidden";
				document.getElementById("date").style.visibility  ="hidden";
				document.getElementById("place_jpn").style.visibility  ="hidden";
				document.getElementById("place_eng").style.visibility  ="hidden";
				document.getElementById("placeId").style.visibility  ="hidden";
				document.getElementById("map").style.visibility  ="hidden";
			}else{
				document.getElementById("cmpDate").style.visibility ="hidden";
				document.getElementById("business").style.visibility ="visible";
				document.getElementById("incident").style.visibility ="visible";
				document.getElementById("date").style.visibility ="visible";
				document.getElementById("place_jpn").style.visibility ="visible";
				document.getElementById("place_eng").style.visibility ="visible";
				document.getElementById("placeId").style.visibility ="visible";
				document.getElementById("map").style.visibility ="visible";
			}
		}

		function fncView(){
			document.test.action = "alert_test_view.php";
			document.test.submit();
		}
	</script>

    </head>
	<form method="post" name = "test" onsubmit="return false;">
	<table>
		<tr>
		<td>ステータス</td>
		<td>
		<select name="status" onChange="fncCmb()">
            <option value="0">登録</option>
            <option value="1">完了</option>
        </select>
        </td>
        </tr>

        <tr>
        <td>インシデントNo</td>
        <td><input type="text" name ="incident_no"></td>
        </tr>

        <tr id = "cmpDate">
        <td>完了日時</td>
        <td><input type="text" name ="comp_date"></td>
        </tr>

        <tr id = "business">
        <td>業務名</td>
        <td><input type="text" name ="business_name"></td>
        </tr>

        <tr id ="incident">
        <td>インシデント件名</td>
        <td><input type="text" name ="incident_name"></td>
        </tr>

        <tr id ="date">
        <td>発生日時</td>
        <td><input type="text" name ="occurrence_date"></td>
        </tr>

        <tr id ="place_jpn">
        <td>場所名</td>
        <td><input type="text" name ="place_name"></td>
        </tr>

        <tr id ="place_eng">
        <td>場所名（英語）</td>
        <td><input type="text" name ="place_name_eng"></td>
        </tr>

        <tr id ="placeId">
        <td>場所3 ID</td>
        <td><input type="text" name ="place3_id"></td>
        </tr>

        <tr id ="map">
        <td>地図ID</td>
        <td><input type="text" name ="map_id"></td>
        </tr>


        <tr>
        <td></td>
        <td><input type="button" onclick="fncView();" value="表示画面へ"></td>
        </tr>
    </table>
	</form>


</body>
</html>