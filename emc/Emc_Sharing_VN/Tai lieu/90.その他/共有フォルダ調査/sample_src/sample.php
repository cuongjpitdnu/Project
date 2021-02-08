<?php
$file_path = "\\\\10.10.10.10\\Scan\\";

touch($file_path.'test_'.date('YmdHis').'.txt');
//mkdir($file_path.'test');

exit;
?>