-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� �ėp���[�o�͏���
--
--
-- Author T.Nishida
-- Create 2020/12/07(Mon)
-- -------------------------------------------------------------------

DROP TABLE R_ReportCondition;


CREATE TABLE R_ReportCondition (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,ReportID				INT				NOT NULL
	,ID						INT				NOT NULL
	,ItemName				NVARCHAR(50)	NOT NULL
	,DataType				INT				NOT NULL
	,SelectType				NVARCHAR(30)	NOT NULL
	,SelectCode				NVARCHAR(200)	
	,SelectData				NVARCHAR(200)	
	,RequiredFlag			BIT				NOT NULL	DEFAULT 0
	,SelectMulti			BIT				NOT NULL	DEFAULT 0
	,SortNo					INT				NOT NULL	DEFAULT 0
	,PRIMARY KEY (ReportID,ID)
)
;


