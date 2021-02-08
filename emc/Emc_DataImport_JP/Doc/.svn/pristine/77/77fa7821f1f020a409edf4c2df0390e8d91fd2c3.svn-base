<?php

class mybinhex
{
    public function bin2hex($bindata)
    {
        $hexdata = '';
        $length = strlen($bindata);
        for($i=0; $i < $length; $i++) 
        {
            $hexdata .= sprintf("%02x", ord($bindata{$i}));
        }
        return $hexdata;
    }

    public function hex2bin($hexdata)
    {
        $bindata = '';
        $length = strlen($hexdata); 
        for ($i=0; $i < $length; $i += 2)
        {
            $bindata .= chr(hexdec(substr($hexdata, $i, 2)));
        }
        return $bindata;
    }
}

?>