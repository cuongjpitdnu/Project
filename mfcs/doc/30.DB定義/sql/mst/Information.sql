-- -------------------------------------------------------------------
-- 生産管理システム お知らせテーブル
--
--
-- Author T.Nishida
-- Create 2020/07/14(Tue)
-- -------------------------------------------------------------------

DROP TABLE Information;


CREATE TABLE Information (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,ID						INT				NOT NULL	IDENTITY(1,1)
	,Sdate					DATETIME		
	,Edate					DATETIME		
	,Message				NVARCHAR(500)	
	,PRIMARY KEY (ID)
)
;


