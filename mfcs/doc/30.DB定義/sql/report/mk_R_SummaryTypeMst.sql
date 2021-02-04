-- -------------------------------------------------------------------
-- 生産管理システム 汎用集計表タイプマスタ
--
--
-- Author T.Nishida
-- Create 2020/12/07(Mon)
-- -------------------------------------------------------------------

DROP TABLE R_SummaryTypeMst;


CREATE TABLE R_SummaryTypeMst (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,ID						INT				NOT NULL
	,SummaryTypeName		NVARCHAR(50)	NOT NULL
	,SortNo					INT				NOT NULL	DEFAULT 0
	,PRIMARY KEY (ID)
)
;


