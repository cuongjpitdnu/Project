-- -------------------------------------------------------------------
-- 生産管理システム 汎用帳票タイプマスタ
--
--
-- Author T.Nishida
-- Create 2020/12/07(Mon)
-- -------------------------------------------------------------------

DROP TABLE R_ReportTypeMst;


CREATE TABLE R_ReportTypeMst (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,ID						INT				NOT NULL
	,ReportTypeName			NVARCHAR(50)	NOT NULL
	,SortNo					INT				NOT NULL	DEFAULT 0
	,PRIMARY KEY (ID)
)
;


