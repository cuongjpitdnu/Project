<?php

namespace App\Providers;

use Illuminate\Foundation\Support\Providers\RouteServiceProvider as ServiceProvider;
use Illuminate\Support\Facades\Route;

class RouteServiceProvider extends ServiceProvider
{
	/**
	 * This namespace is applied to your controller routes.
	 *
	 * In addition, it is set as the URL generator's root namespace.
	 *
	 * @var string
	 */
	protected $namespace = 'App\Http\Controllers';

	/**
	 * The path to the "home" route for your application.
	 *
	 * @var string
	 */
	public const HOME = '/home';

	/**
	 * Define your route model bindings, pattern filters, etc.
	 *
	 * @return void
	 */
	public function boot()
	{
		//

		parent::boot();
	}

	/**
	 * Define the routes for the application.
	 *
	 * @return void
	 */
	public function map()
	{
		$this->mapApiRoutes();

		$this->mapWebRoutes();

		$this->mapMstRoutes();

		$this->mapSchetRoutes();

		$this->mapSchemRoutes();

		$this->mapSchesRoutes();

		$this->mapReportRoutes();

	}

	/**
	 * Define the "web" routes for the application.
	 *
	 * These routes all receive session state, CSRF protection, etc.
	 *
	 * @return void
	 */
	protected function mapWebRoutes()
	{
		Route::middleware('web')
			 ->namespace($this->namespace)
			 ->group(base_path('routes/web.php'));
	}

	/**
	 * Define the "api" routes for the application.
	 *
	 * These routes are typically stateless.
	 *
	 * @return void
	 */
	protected function mapApiRoutes()
	{
		Route::prefix('api')
			 ->middleware('api')
			 ->namespace($this->namespace)
			 ->group(base_path('routes/api.php'));
	}

	/**
	 * 共通マスタ画面用のルート
	 *
	 * @return void
	 *
	 * @create 2020/07/09　K.Yoshihara
	 * @update
	 */
	protected function mapMstRoutes()
	{
		Route::prefix('mst')
		->middleware('web')
		->namespace($this->namespace)
			->group(base_path('routes/mst.php'));
	}
	
	/**
	 * 搭載日程画面用のルート
	 *
	 * @return void
	 *
	 * @create 2020/07/09　K.Yoshihara
	 * @update
	 */
	protected function mapSchetRoutes()
	{
		Route::prefix('schet')
		->middleware('web')
		->namespace($this->namespace)
			->group(base_path('routes/schet.php'));
	}

	/**
	 * 中日程画面用のルート
	 *
	 * @return void
	 *
	 * @create 2020/07/09　K.Yoshihara
	 * @update
	 */
	protected function mapSchemRoutes()
	{
		Route::prefix('schem')
		->middleware('web')
		->namespace($this->namespace)
			->group(base_path('routes/schem.php'));
	}

	/**
	 * 小日程画面用のルート
	 *
	 * @return void
	 *
	 * @create 2020/07/09　K.Yoshihara
	 * @update
	 */
	protected function mapSchesRoutes()
	{
		Route::prefix('sches')
		->middleware('web')
		->namespace($this->namespace)
			->group(base_path('routes/sches.php'));
	}

	/**
	 * 帳票画面用のルート
	 *
	 * @return void
	 *
	 * @create 2020/07/09　K.Yoshihara
	 * @update
	 */
	protected function mapReportRoutes()
	{
		Route::prefix('report')
		->middleware('web')
		->namespace($this->namespace)
			->group(base_path('routes/report.php'));
	}

}
