-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� �ėp�W�v�\
--
--
-- Author T.Nishida
-- Create 2020/12/07(Mon)
-- -------------------------------------------------------------------

DROP TABLE R_Summary;


CREATE TABLE R_Summary (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,ID						INT				NOT NULL
	,SummaryType			INT				NOT NULL
	,SummaryName			NVARCHAR(50)	NOT NULL
	,TemplateFileName		NVARCHAR(30)	NOT NULL
	,SheetName				NVARCHAR(30)	NOT NULL
	,TitleRow				INT				NOT NULL
	,StartRow				INT				NOT NULL
	,StartCol				INT				NOT NULL
	,ViewName				NVARCHAR(30)	NOT NULL
	,PRIMARY KEY (ID)
)
;


