-- -------------------------------------------------------------------
-- 生産管理システム 汎用帳票出力項目
--
--
-- Author T.Nishida
-- Create 2020/12/07(Mon)
-- -------------------------------------------------------------------

DROP TABLE R_ReportItem;


CREATE TABLE R_ReportItem (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,ReportID				INT				NOT NULL
	,DataID					INT				NOT NULL
	,ID						INT				NOT NULL
	,ItemName				NVARCHAR(50)	NOT NULL
	,FieldName				NVARCHAR(30)	NOT NULL
	,SortNo					INT				NOT NULL	DEFAULT 0
	,PRIMARY KEY (ReportID,DataID,ID)
)
;


