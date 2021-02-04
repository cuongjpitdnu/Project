-- -------------------------------------------------------------------
-- 生産管理システム 職制マスタテーブル
--
--
-- Author T.Nishida
-- Create 2020/07/08(Wed)
-- -------------------------------------------------------------------

DROP TABLE mstOrg;


CREATE TABLE mstOrg (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,Up_User				NVARCHAR(20)				
	,ID						INT				NOT NULL	
	,PID					INT				NOT NULL	DEFAULT 0
	,LV_No					TINYINT			NOT NULL	DEFAULT 0
	,Sdate					DATETIME		NOT NULL	
	,Edate					DATETIME					
	,Name					NVARCHAR(50)	NOT NULL	
	,Nick					NVARCHAR(50)	NOT NULL	
	,SyokuseiCode			NCHAR(6)					
	,FolderFlag				BIT				NOT NULL	DEFAULT 1
	,OutInFlag				TINYINT			NOT NULL	DEFAULT 1
	,BuOutInFlag			TINYINT			NOT NULL	DEFAULT 1
	,OutPID					INT							DEFAULT 0
	,OutType				INT							DEFAULT 0
	,SortNo					INT				NOT NULL	DEFAULT 0
	,VenderCode				NVARCHAR(20)				
	,PRIMARY KEY (ID, Sdate)
)
;


