<?php
/*
 * @SchemCyn_mstKotei_STR_CEloquentRepository.php
 *
 * @create 2020/09/08 Chien
 *
 * @update
 */
namespace App\Repositories\SchemCyn_mstKotei_STR_C;

use App\Repositories\EloquentRepository;

/**
 * Class SchemCyn_mstKotei_STR_CEloquentRepository.
 * @create 2020/09/08 Chien
 * @update
 */
class SchemCyn_mstKotei_STR_CEloquentRepository extends EloquentRepository implements SchemCyn_mstKotei_STR_CRepositoryInterface
{
	/**
	 * get model
	 * @return string
	 * @create 2020/09/08 Chien
	 * @update
	 */
	public function getModel() {
		return \App\Models\Cyn_mstKotei_STR_C::class;
	}

	/**
	 * get primary key
	 * @return array
	 * @create 2020/09/08 Chien
	 * @update
	 */
	public function getPrimaryKey() {
		return ['CKind', 'Code', 'No'];
	}

	/**
	 * find object
	 * @return mixed
	 * @create 2020/09/08 Chien
	 * @update
	 */
	public function findWithMultiKey(...$value) {
		$result = $this->_model;
		foreach($this->_primaryKey as $key => $primaryKey) {
			if(isset($value[$key]) && is_string($value[$key])) {
				$result = $result->where($primaryKey, $value[$key]);
			}
		}
		// condition sort & groupBy
		$arrFieldGroupBySelect = array();
		if($value != null) {
			foreach($value as $item) {
				if(is_array($item)) {
					foreach($item as $field => $direction) {
						if(is_string($field)) {
							if($direction == 'groupBy') {
								$result = $result->groupBy($field);
								$arrFieldGroupBySelect[] = $field;
							} else {
								$result = $result->orderBy($field, $direction ? $direction : 'asc');
							}
						} else {
							$result = $result->orderBy($direction);
						}
					}
				}
			}
		}
		if(count($arrFieldGroupBySelect) > 0) {
			foreach($arrFieldGroupBySelect as $field) {
				$result = $result->select($field);
			}
		}
		$result = $result->distinct()->get();
		return $result;
	}
}
