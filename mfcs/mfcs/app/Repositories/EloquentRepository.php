<?php
/*
 * @EloquentRepository.php
 *
 * @create 2020/08/03 Thang
 *
 * @update
 */
namespace App\Repositories;

use App\Repositories\RepositoryInterface;
/**
 * Class EloquentRepository.
 * 
 * @create 2020/08/03 Thang
 *
 * @update
 */
abstract class EloquentRepository implements RepositoryInterface
{
	/**
	 * @var \Illuminate\Database\Eloquent\Model
	 */
	protected $_model;
	protected $_primaryKey;
	/**
	 * EloquentRepository constructor.
	 * @create 2020/08/03 Thang
	 * @update
	 */
	public function __construct()
	{
		$this->setModel();
		$this->setPrimaryKey();
	}

	/**
	 * get model
	 * @return string
	 * @create 2020/08/03 Thang
	 * @update
	 */
	abstract public function getModel();
	/**
	 * get primary key
	 * @return string
	 * @create 2020/08/03 Thang
	 * @update
	 */
	abstract public function getPrimaryKey();
	/**
	 * Set model
	 * @return mixed
	 * @create 2020/08/03 Thang
	 * @update
	 */
	public function setModel()
	{
		$this->_model = app()->make(
			$this->getModel()
		);
	}
	/**
	 * Set primary key
	 * @create 2020/08/03 Thang
	 * @update
	 */
	public function setPrimaryKey(){
		$this->_primaryKey = $this->getPrimaryKey();
	}
	/**
	 * Get All
	 * @return \Illuminate\Database\Eloquent\Collection|static[]
	 * @create 2020/08/03 Thang
	 * @update
	 */
	public function getAll()
	{
		return $this->_model->all();
	}

	/**
	 * Get data with conditions
	 * @param array|string $conditions
	 * @param int $paginate
	 * @param array $conditionArray
	 * @param string $select
	 * @return \Illuminate\Database\Eloquent\Collection|static[]
	 * @create 2020/08/03 Thang
	 * @update
	 */
	public function getData($conditions, $paginate = null, $select = null, $conditionArray = [])
	{
		$result = $this->_model;
		if(!is_null($select)){
			$result = $result->selectRaw($select);
		}
		if(is_array($conditions)){
			foreach($conditions as $key => $value){
				$result = $result->where($key, $value);
			}
		}else{
			$result = $result->whereRaw($conditions, $conditionArray);
		}
		$result = $result->sortable('fld1', 'asc');
		if(!is_null($paginate)){
			$result = $result->paginate($paginate);
		}else{
			$result = $result->get();
		}

		return $result;
	}

	/**
	 * Get one
	 * @param $id
	 * @return mixed
	 * @create 2020/08/03 Thang
	 * @update
	 */
	public function find($value)
	{
		$result = $this->_model->where($this->_primaryKey, $value)
			->firstOrFail();
		return $result;
	}

	/**
	 * Create
	 * @param array $attributes
	 * @return mixed
	 * @create 2020/08/03 Thang
	 * @update
	 */
	public function create(array $attributes)
	{
		return $this->_model->create($attributes);
	}

	/**
	 * Update
	 * @param $id
	 * @param array $attributes
	 * @return bool|mixed
	 * @create 2020/08/03 Thang
	 * @update
	 */
	public function update($id, array $attributes)
	{
		$result = $this->find($id);
		if ($result) {
			$result->update($attributes);
			return $result;
		}
		return false;
	}

	/**
	 * Delete
	 *
	 * @param $id
	 * @return bool
	 * @create 2020/08/03 Thang
	 * @update
	 */
	public function delete($id)
	{
		$result = $this->find($id);
		if ($result) {
			$result->delete();
			return true;
		}
		return false;
	}
}
