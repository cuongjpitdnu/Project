<?php
// -------------------------------------------------------------------------
//	function	: SQLServer用DBメソッド定義ファイル
//	create		: 2020/01/17 KBS S.Tasaki
//	update		: 
// -------------------------------------------------------------------------
// SQLServer用DBアクセスクラス
class sqlsrv_dbaccess{
	
	var $_host_name;		//ホスト名
	var $_port_no;			//ポート番号
	var $_database_name;	//データベース名
	var $_user_name;		//ユーザー名
	var $_user_pass;		//パスワード
	var $dbConn;				//コネクション
	
	//コンストラクタ
	function sqlsrv_dbaccess(){
		$this->_host_name = DB_HOSTNAME;
		$this->_database_name = DB_DBNAME;
		$this->_user_name = DB_USERNAME;
		$this->_user_pass = DB_PASSWORD;
	}
	
	//DB接続
	public function fncDbConnect(){
		try {
			$this->dbConn = new PDO('sqlsrv:Server=' .$this->_host_name .'; Database=' .$this->_database_name, $this->_user_name, $this->_user_pass);
			$this->dbConn->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
			
			return true;
		} catch( PDOException $e) {
			return false;
		}
	}
	
	//SQLステート作成
	public function funcPrepare($strSQL){
		return $this->dbConn->prepare($strSQL);
	}
	
	//トランザクション開始
	public function funcBeginTransaction(){
		$this->dbConn->beginTransaction();
	}
	
	//Commit
	public function funcCommit(){
		$this->dbConn->commit();
	}
	
	//Rollback
	public function funcRollback(){
		$this->dbConn->rollBack();
	}
	
	
}

?>