<?php
/*
 * @MstBDCodeEloquentRepository.php
 *
 * @create 2020/08/06 Thang
 *
 * @update
 */
namespace App\Repositories\MstBDCode;

use App\Repositories\EloquentRepository;

/**
 * Class MstBDCodeEloquentRepository.
 * @create 2020/08/06 Thang
 * @update
 */
class MstBDCodeEloquentRepository extends EloquentRepository implements MstBDCodeRepositoryInterface
{
	/**
	 * get model
	 * @return string
	 * @create 2020/08/03 Thang
	 * @update
	 */
	public function getModel(){
		return \App\Models\MstBDCode::class;
	}
	/**
	 * get primary key
	 * @return string
	 * @create 2020/08/03 Thang
	 * @update
	 */
	public function getPrimaryKey()
	{
		return 'Code';
	}
}
