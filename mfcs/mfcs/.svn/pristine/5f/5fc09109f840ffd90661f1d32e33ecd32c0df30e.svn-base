<?php
/*
 * @ResponseHeaders.php
 * HTTPレスポンスヘッダ用ミドルウェア
 *
 * @create 2020/07/15 KBS T.Nishida
 *
 * @update
 */

namespace App\Http\Middleware;

use Closure;

class ResponseHeaders
{
    /**
     * Handle an incoming request.
     *
     * @param  \Illuminate\Http\Request  $request
     * @param  \Closure  $next
     * @return mixed
     */
    public function handle($request, Closure $next)
    {
		$response = $next($request);

		//クリックジャッキング対策
		//iframeを使用させない
		$response->headers->set('X-Frame-Options', 'deny');

		//XSS対策(文字コード脆弱性対応)
		$response->headers->set('Content-Type', 'text/html; charset=UTF-8');

		//キャッシュさせない(戻る・進むボタン対策)
		$response->headers->set("Cache-Control", "no-store, no-cache, must-revalidate, max-age=0, post-check=0, pre-check=0");
		$response->headers->set("Pragma", "no-cache");

		return $response;
    }
}
