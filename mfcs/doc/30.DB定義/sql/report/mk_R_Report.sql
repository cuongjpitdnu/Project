-- -------------------------------------------------------------------
-- 生産管理システム 汎用帳票
--
--
-- Author T.Nishida
-- Create 2020/12/07(Mon)
-- -------------------------------------------------------------------

DROP TABLE R_Report;


CREATE TABLE R_Report (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,ID						INT				NOT NULL
	,ReportType				INT				NOT NULL
	,ReportName				NVARCHAR(50)	NOT NULL
	,TemplateFileName		NVARCHAR(30)	NOT NULL
	,PRIMARY KEY (ID)
)
;


