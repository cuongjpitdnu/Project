<?php
/*
 * @MstOrderNoRepositoryInterface.php
 *
 * @create 2020/08/03 Thang
 *
 * @update
 */
namespace App\Repositories\MstOrderNo;


/**
 * interface MstOrderNoRepositoryInterface
 * @create 2020/08/03 Thang
 *
 * @update
 */
interface MstOrderNoRepositoryInterface
{
	/**
	 * Get data with conditions
	 * @param array|string $conditions
	 * @return \Illuminate\Database\Eloquent\Collection|static[]
	 * @create 2020/08/03 Thang
	 * @update
	 */
	public function getData($conditions);
	/**
	 * Get one
	 * @param string $value
	 * @return mixed
	 * @create 2020/08/03 Thang
	 * @update
	 */
	public function find($value);
}
