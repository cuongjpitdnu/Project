<?php
    $intStatus = $_POST['status'];

    $status = $_POST['status'] == 0 ? "登録" : "完了";
    $incidentNo = $_POST['incident_no'];
    $incident_name = $_POST['incident_name'];
    $business_name = $_POST['business_name'];
    $occurrence_date= $_POST['occurrence_date'];
    $place_name= $_POST['place_name'];
    $place_name_eng= $_POST['place_name_eng'];
    $map_id= $_POST['map_id'];
    $place3_id= $_POST['place3_id'];
    $comp_date= $_POST['comp_date'];

?>


<html>
<body>

<table>
<tr>
		<td>ステータス</td>
		<td><?php echo $status?></td>
        </tr>

        <tr>
        <td>インシデントNo</td>
        <td><?php echo $incidentNo?></td>
        </tr>
		<?php if($intStatus == 1){ ?>
        <tr>
        <td>完了日時</td>
        <td><?php echo $comp_date?></td>
        </tr>
		<?php }?>

		<?php if($intStatus == 0){ ?>
        <tr id = business>
        <td>業務名</td>
        <td><?php echo $business_name?></td>
        </tr>

        <tr id ="incident">
        <td>インシデント件名</td>
        <td><?php echo $incident_name?></td>
        </tr>

        <tr id ="date">
        <td>発生日時</td>
        <td><?php echo $occurrence_date?></td>
        </tr>

        <tr id ="place_jpn">
        <td>場所名</td>
        <td><?php echo $place_name?></td>
        </tr>

        <tr id ="place_eng">
        <td>場所名（英語）</td>
        <td><?php echo $place_name_eng?></td>
        </tr>

        <tr id ="placeId">
        <td>場所3 ID</td>
        <td><?php echo $place3_id?></td>
        </tr>

        <tr id ="map">
        <td>地図ID</td>
        <td><?php echo $map_id?></td>
        </tr>
        <?php }?>

</table>

</body>
</html>