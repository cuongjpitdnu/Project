-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� �ėp���[�o�̓f�[�^
--
--
-- Author T.Nishida
-- Create 2020/12/07(Mon)
-- -------------------------------------------------------------------

DROP TABLE R_ReportData;


CREATE TABLE R_ReportData (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,ReportID				INT				NOT NULL
	,ID						INT				NOT NULL
	,SheetName				NVARCHAR(30)	NOT NULL
	,TitleRow				INT				NOT NULL
	,StartRow				INT				NOT NULL
	,StartCol				INT				NOT NULL
	,ViewName				NVARCHAR(30)	NOT NULL
	,PRIMARY KEY (ReportID,ID)
)
;


