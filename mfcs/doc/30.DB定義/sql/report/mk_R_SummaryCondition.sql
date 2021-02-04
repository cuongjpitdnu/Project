-- -------------------------------------------------------------------
-- 生産管理システム 汎用集計表出力条件
--
--
-- Author T.Nishida
-- Create 2020/12/07(Mon)
-- -------------------------------------------------------------------

DROP TABLE R_SummaryCondition;


CREATE TABLE R_SummaryCondition (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,SummaryID				INT				NOT NULL
	,ID						INT				NOT NULL
	,ItemName				NVARCHAR(50)	NOT NULL
	,FieldName				NVARCHAR(30)	NOT NULL
	,DataType				INT				NOT NULL
	,SelectType				NVARCHAR(30)	NOT NULL
	,SelectCode				NVARCHAR(200)	
	,SelectData				NVARCHAR(200)	
	,RequiredFlag			BIT				NOT NULL	DEFAULT 0
	,SelectMulti			BIT				NOT NULL	DEFAULT 0
	,SortNo					INT				NOT NULL	DEFAULT 0
	,PRIMARY KEY (SummaryID,ID)
)
;


