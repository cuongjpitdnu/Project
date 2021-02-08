<?php
/*
 * @MstFloorEloquentRepository.php
 *
 * @create 2020/08/20 Cuong
 *
 * @update
 */
namespace App\Repositories\MstFloor;

use App\Repositories\EloquentRepository;

/**
 * Class MstFloorEloquentRepository.
 * @create 2020/08/20 Cuong
 * @update
 */
class MstFloorEloquentRepository extends EloquentRepository implements MstFloorRepositoryInterface
{
	/**
	 * get model
	 * @return string
	 * @create 2020/08/20 Cuong
	 * @update
	 */
	public function getModel(){
		return \App\Models\MstFloor::class;
	}
	/**
	 * get primary key
	 * @return string
	 * @create 2020/08/20 Cuong
	 * @update
	 */
	public function getPrimaryKey()
	{
		return 'Code';
	}
	/**
	 * get data
	 * @return string
	 * @create 2020/08/20 Cuong
	 * @update 2020/09/01 Cuong if $request->sort empty then orderBy  SortNo and Code
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
		
		$request = app()->make('Illuminate\Http\Request');
		if(!$request->has('sort') || empty($request->sort) ){
			$result = $result->orderBy('SortNo', 'asc')
				->orderBy('Code', 'asc');
		}else{
			$result = $result->sortable();
		}

		if(!is_null($paginate)){
			$result = $result->paginate($paginate);
		}else{
			$result = $result->get();
		}

		return $result;
	}
}
