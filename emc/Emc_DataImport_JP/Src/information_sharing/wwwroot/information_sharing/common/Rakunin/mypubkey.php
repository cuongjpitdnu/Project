<?php
require_once('mypbkdf2.php');

class mypubkey
{
    function __construct()
    {
        $this->share_key = pack('H*', "6d3e227b686c21667c79" . 
                        "152e1133556064652c20" . 
                        "74686973206973207468" . 
                        "65203265336569366520" .  
                        "3b65793b206f6e207468" .  
                        "65207365727665722073" .  
                        "6964772c206974206973" .  
                        "207468652073656e6a2a" .  
                        "2c2f6c7323397326766e" .  
                        "6c2c7265636569766520" .  
                        "f2f2f2f2f2f2f2f2f2f2" . 
                        "5b606c7e207468652023" .  
                        "2b667f2e6c7e6e50"); 
        $this->iter_count = 10;
        $this->key_len = 16;
    }

    public function gen_pubkey($factor, &$pubkey)
    {
        $hash = sha1($this->share_key, true);
        //echo "sha1: " . bin2hex($hash) . '<br/>';
        //echo "factor: " . bin2hex($factor) . '<br/>';

        $mypbkdf2 = new mypbkdf2();
        $pubkey = $mypbkdf2->pbkdf2("SHA1", $hash, $factor, $this->iter_count, $this->key_len);
        //echo "pubkey: " . bin2hex($pubkey) .'<br/>';

        return 0;
    }
}

?>
