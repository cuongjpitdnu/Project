<?php
/*
 * @RepositoryInterface.php
 * Interface class
 *
 * @create 2020/08/03 Thang
 *
 * @update
 */
namespace App\Repositories;

/**
 * Class RepositoryInterface.
 * @create 2020/08/03 Thang
 *
 * @update
 */
interface RepositoryInterface
{
	/**
	 * Get all
	 * @return \Illuminate\Database\Eloquent\Collection|static[]
	 * @create 2020/08/03 Thang
	 * @update
	 */
	public function getAll();

	/**
	 * Get with conditions
	 * @param string || array $conditions
	 * @param int $paginate
	 * @return \Illuminate\Database\Eloquent\Collection|static[]
	 * @create 2020/08/03 Thang
	 * @update
	 */
	public function getData($conditions);

	/**
	 * Get one
	 * @param $id
	 * @return mixed
	 * @create 2020/08/03 Thang
	 * @update
	 */
	public function find($value);

	/**
	 * Create
	 * @param array $attributes
	 * @return mixed
	 * @create 2020/08/03 Thang
	 * @update
	 */
	public function create(array $attributes);

	/**
	 * Update
	 * @param $id
	 * @param array $attributes
	 * @return mixed
	 * @create 2020/08/03 Thang
	 * @update
	 */
	public function update($id, array $attributes);

	/**
	 * Delete
	 * @param $id
	 * @return mixed
	 * @create 2020/08/03 Thang
	 * @update
	 */
	public function delete($id);
}