-- -------------------------------------------------------------------
-- 生産管理システム 人員履歴データテーブル
--
--
-- Author T.Nishida
-- Create 2020/07/08(Wed)
-- -------------------------------------------------------------------

DROP TABLE MEMHist;


CREATE TABLE MEMHist (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,Up_User				NVARCHAR(20)				
	,MEM_ID					INT				NOT NULL	
	,Sdate					DATETIME		NOT NULL	
	,Edate					DATETIME					
	,WorkerNo				INT							DEFAULT 0
	,GroupID				INT							DEFAULT 0
	,SortNo					INT							
	,DistCode				NCHAR(5)		NOT NULL	
	,OutInFlag				TINYINT			NOT NULL	DEFAULT 0
	,COM_ID					INT							DEFAULT 0
	,OutType				TINYINT						DEFAULT 0
	,Is_Proper				NVARCHAR(3)		NOT NULL	DEFAULT 1
	,PRIMARY KEY (MEM_ID, Sdate)
)
;


