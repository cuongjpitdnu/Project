-- -------------------------------------------------------------------
-- 生産管理システム 汎用帳票出力条件設定
--
--
-- Author T.Nishida
-- Create 2020/12/07(Mon)
-- -------------------------------------------------------------------

DROP TABLE R_ReportSetCondition;


CREATE TABLE R_ReportSetCondition (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,ReportID				INT				NOT NULL
	,ConditionID			INT				NOT NULL
	,DataID					INT				NOT NULL
	,FieldName				NVARCHAR(30)	NOT NULL
	,PRIMARY KEY (ReportID,ConditionID,DataID)
)
;


