-- -------------------------------------------------------------------
-- 生産管理システム システムログテーブル
--
--
-- Author T.Nishida
-- Create 2020/07/14(Tue)
-- -------------------------------------------------------------------

DROP TABLE SystemLog;


CREATE TABLE SystemLog (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,ID						INT				NOT NULL	IDENTITY(1,1)
	,UserID					NVARCHAR(50)	
	,Action1				NVARCHAR(100)	
	,Action2				NVARCHAR(100)	
	,Action3				NVARCHAR(100)	
	,PRIMARY KEY (ID)
)
;


