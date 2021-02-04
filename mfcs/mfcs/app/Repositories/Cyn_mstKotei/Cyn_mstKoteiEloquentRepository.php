<?php
/*
 * @Cyn_mstKoteiEloquentRepository.php
 *
 * @create 2020/09/03 Dung
 *
 * @update
 */
namespace App\Repositories\Cyn_mstKotei;

use App\Repositories\EloquentRepository;

/**
 * Class Cyn_mstKoteiEloquentRepository.
 * @create 2020/09/03 Dung
 * @update
 */
class Cyn_mstKoteiEloquentRepository extends EloquentRepository implements Cyn_mstKoteiRepositoryInterface
{
	/**
	 * get model
	 * @return string
	 * @create 2020/09/03 Dung
	 * @update
	 */
	public function getModel() {
		return \App\Models\Cyn_mstKotei::class;
	}

	/**
	 * get primary key
	 * @return string
	 * @create 2020/09/03 Dung
	 * @update
	 */
	public function getPrimaryKey() {
		return ['Code', 'CKind'];
	}
	/**
	 * find object
	 * @return mixed
	 * @create 2020/09/03 Dung
	 * @update
	 */
	public function findWithMultiKey(...$value) {
		$result = $this->_model;
		foreach($this->_primaryKey as $key => $primaryKey) {
			if(isset($value[$key])) {
				$result = $result->where($primaryKey, $value[$key]);
			}
		}
		$result = $result->first();
		return $result;
	}
}
