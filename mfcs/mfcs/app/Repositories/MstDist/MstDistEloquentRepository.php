<?php
/*
 * @MstDistEloquentRepository.php
 *
 * @create 2020/10/09 Chien
 *
 * @update
 */
namespace App\Repositories\MstDist;

use App\Repositories\EloquentRepository;

/**
 * Class MstDistEloquentRepository.
 * @create 2020/10/09 Chien
 * @update
 */
class MstDistEloquentRepository extends EloquentRepository implements MstDistRepositoryInterface
{
	/**
	 * get model
	 * @return string
	 * @create 2020/10/09 Chien
	 * @update
	 */
	public function getModel() {
		return \App\Models\MstDist::class;
	}

	/**
	 * get primary key
	 * @return array
	 * @create 2020/10/09 Chien
	 * @update
	 */
	public function getPrimaryKey() {
		return 'Code';
	}
}
