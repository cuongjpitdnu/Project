<?php

namespace App\Providers;

use Illuminate\Support\ServiceProvider;

class AppServiceProvider extends ServiceProvider
{
	/**
	 * Register any application services.
	 *
	 * @return void
	 */
	public function register()
	{
		$this->app->singleton(
			\App\Repositories\MstOrderNo\MstOrderNoRepositoryInterface::class,
			\App\Repositories\MstOrderNo\MstOrderNoEloquentRepository::class
		);

		$this->app->singleton(
			\App\Repositories\MstBDCode\MstBDCodeRepositoryInterface::class,
			\App\Repositories\MstBDCode\MstBDCodeEloquentRepository::class
		);

		$this->app->singleton(
			\App\Repositories\MstFloor\MstFloorRepositoryInterface::class,
			\App\Repositories\MstFloor\MstFloorEloquentRepository::class
		);

		$this->app->singleton(
			\App\Repositories\SchemCyn_mstKotei_STR_P\SchemCyn_mstKotei_STR_PRepositoryInterface::class,
			\App\Repositories\SchemCyn_mstKotei_STR_P\SchemCyn_mstKotei_STR_PEloquentRepository::class
		);

		$this->app->singleton(
			\App\Repositories\SchemCyn_mstKotei_STR_C\SchemCyn_mstKotei_STR_CRepositoryInterface::class,
			\App\Repositories\SchemCyn_mstKotei_STR_C\SchemCyn_mstKotei_STR_CEloquentRepository::class
		);

		$this->app->singleton(
			\App\Repositories\Cyn_mstKotei\Cyn_mstKoteiRepositoryInterface::class,
			\App\Repositories\Cyn_mstKotei\Cyn_mstKoteiEloquentRepository::class
		);

		$this->app->singleton(
			\App\Repositories\MstDist\MstDistRepositoryInterface::class,
			\App\Repositories\MstDist\MstDistEloquentRepository::class
		);
	}

	/**
	 * Bootstrap any application services.
	 *
	 * @return void
	 */
	public function boot()
	{
		//
	}
}
