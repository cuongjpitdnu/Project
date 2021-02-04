-- -------------------------------------------------------------------
-- 生産管理システム TimeTrackerNXAPI実行情報
--
--
-- Author T.Nishida
-- Create 2020/12/24(Thu)
-- -------------------------------------------------------------------

DROP TABLE TimeTrackerRunApiInfo;


CREATE TABLE TimeTrackerRunApiInfo (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,UserID					NVARCHAR(50)	NOT NULL
)
;


