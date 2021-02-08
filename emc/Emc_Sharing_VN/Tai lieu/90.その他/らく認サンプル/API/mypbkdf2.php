<?php

class mypbkdf2
{
    /*
    * pbkdf2 key derivation function as defined by RSA's PKCS #5: rfc2898.txt
    * $algorithm - The hash algorithm to use.
    * $password - The password.
    * $salt - A salt that is unique to the password.
    * $count - Iteration count. Higher is better, but slower.
    * $key_length - The length of the derived key in bytes.
    * Returns: A $key_length-byte key derived from the password and salt.
    */
    public function pbkdf2($algorithm, $password, $salt, $count, $key_length)
    {
        $algorithm = strtolower($algorithm);
        if(!in_array($algorithm, hash_algos(), true))
        {
            trigger_error('PBKDF2 ERROR: Invalid hash algorithm.', E_USER_ERROR);
        }
        if($count <= 0 || $key_length <= 0)
        {
            trigger_error('PBKDF2 ERROR: Invalid parameters.', E_USER_ERROR);
        }
        if (function_exists("hash_pbkdf2"))
        {
            return hash_pbkdf2($algorithm, $password, $salt, $count, $key_length, true);
        }

        $hash_length = strlen(hash($algorithm, "", true));
        $block_count = ceil($key_length / $hash_length);

        $output = "";
        for($i = 1; $i <= $block_count; $i++)
        {
            //$i encoded as 4 bytes, big endian.
            $last = $salt . pack("N", $i);
            // first iteration
            $last = $xorsum = hash_hmac($algorithm, $last, $password, true);
            //perform the other $count - 1 iterations
            for ($j = 1; $j < $count; $j++)
            {
                $xorsum ^= ($last = hash_hmac($algorithm, $last, $password, true));
            }
            $output .= $xorsum;
        }

        return substr($output, 0, $key_length);
    }
}

?>
