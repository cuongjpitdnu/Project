-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� �ėp���[�o�͏����ݒ�
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


