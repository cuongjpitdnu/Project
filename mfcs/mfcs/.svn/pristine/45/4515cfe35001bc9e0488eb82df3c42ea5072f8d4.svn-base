<?php
/*
 * @helpers.php
 * Bladeヘルパー共通関数ファイル
 *
 * @create 2020/07/09 KBS K.Yoshihara
 *
 * @update 2020/07/21 KBS K.Yoshihara valueUrlEncode valueUrlDecode 追加
 */

use Illuminate\Support\HtmlString;

if ( ! function_exists('unEscapedLine')) {
	/**
	 * 改行をエスケープしないエスケープ処理
	 *
	 * @param  string html
	 * @return HtmlString 改行をエスケープしていないhtml
	 *
	 * @create 2020/07/08　K.Yoshihara
	 * @update
	 */
	function unEscapedLine(string $string): HtmlString
	{
		return new HtmlString(nl2br(e($string)));
	}
}

if ( ! function_exists('valueUrlEncode')) {
	/**
	 * 暗号化処理
	 *
	 * @param  string 暗号化したい文字列
	 * @return string 暗号化した値
	 *
	 * @create 2020/07/21　K.Yoshihara
	 * @update
	 */
	function valueUrlEncode($string)
	{
		if (!isset($string)){
			return null;
		}

		// 暗号化
		return base64_encode(openssl_encrypt($string, 'AES-128-ECB', 'mfcskey4111'));
	}
}

if ( ! function_exists('valueUrlDecode')) {
	/**
	 * 複合化処理
	 *
	 * @param  string 値
	 * @return string 複合化した値
	 *
	 * @create 2020/07/21　K.Yoshihara
	 * @update
	 */
	function valueUrlDecode($string)
	{
		if (!isset($string)){
			return null;
		}

		// 複合化
		return openssl_decrypt(base64_decode($string), 'AES-128-ECB', 'mfcskey4111');
	}
}
