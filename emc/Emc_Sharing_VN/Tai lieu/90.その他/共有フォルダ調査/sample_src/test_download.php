<?php

$file_path = "\\\\192.168.1.71\\Scan\\";
//$file_name = "あ.txt";
//$file_name = "b.png";
//$file_name = "c.jpg";
$file_name = "漢.pdf";

header('Content-Type: application/force-download');
//header('Content-disposition: attachment; filename="'.$file_name.'"');
header("Content-Disposition: attachment; filename*=UTF-8''" . rawurlencode($file_name));
readfile($file_path .$file_name);

exit;
?>