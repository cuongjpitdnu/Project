-- -------------------------------------------------------------------
-- 生産管理システム 総組ブロック
--
--
-- Author T.Nishida
-- Create 2020/09/04(Fri)
-- -------------------------------------------------------------------

DROP TABLE T_Sogumi;


CREATE TABLE T_Sogumi (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,ProjectID				INT				NOT NULL
	,OrderNo				NVARCHAR(10)	NOT NULL
	,BlockName				NVARCHAR(20)	NOT NULL
	,BlockKumiku			NVARCHAR(1)		NOT NULL
	,WorkItemID				INT				
	,PRIMARY KEY (ProjectID, OrderNo, BlockName, BlockKumiku)
)
;


