-- -------------------------------------------------------------------
-- 生産管理システム 搭載日程取込ログ
--
--
-- Author T.Nishida
-- Create 2020/08/21(Fri)
-- -------------------------------------------------------------------

DROP TABLE T_ImportLog;


CREATE TABLE T_ImportLog (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,HistoryID				INT				NOT NULL
	,ID						INT				NOT NULL
	,Category				NVARCHAR(50)	NOT NULL
	,BlockName				NVARCHAR(20)	NOT NULL
	,BlockKumiku			NVARCHAR(1)		NOT NULL
	,Log					NVARCHAR(500)	
	,PRIMARY KEY (HistoryID, ID)
)
;


