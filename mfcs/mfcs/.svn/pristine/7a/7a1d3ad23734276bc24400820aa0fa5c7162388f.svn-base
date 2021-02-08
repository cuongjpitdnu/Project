<?php
/*
 * @SchemCyn_mstKotei_STR_PEloquentRepository.php
 *
 * @create 2020/09/01 Chien
 *
 * @update
 */
namespace App\Repositories\SchemCyn_mstKotei_STR_P;

use App\Repositories\EloquentRepository;

/**
 * Class SchemCyn_mstKotei_STR_PEloquentRepository.
 * @create 2020/09/01 Chien
 * @update
 */
class SchemCyn_mstKotei_STR_PEloquentRepository extends EloquentRepository implements SchemCyn_mstKotei_STR_PRepositoryInterface
{
	/**
	 * get model
	 * @return string
	 * @create 2020/09/01 Chien
	 * @update
	 */
	public function getModel() {
		return \App\Models\Cyn_mstKotei_STR_P::class;
	}

	/**
	 * get primary key
	 * @return array
	 * @create 2020/09/01 Chien
	 * @update
	 */
	public function getPrimaryKey() {
		return ['CKind', 'Code'];
	}

	/**
	 * find object
	 * @return mixed
	 * @create 2020/09/01 Chien
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
