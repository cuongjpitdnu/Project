-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� �ėp�W�v�\�o�͍���
--
--
-- Author T.Nishida
-- Create 2020/12/07(Mon)
-- -------------------------------------------------------------------

DROP TABLE R_SummaryItem;


CREATE TABLE R_SummaryItem (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,SummaryID				INT				NOT NULL
	,ID						INT				NOT NULL
	,ItemName				NVARCHAR(50)	NOT NULL
	,FieldName				NVARCHAR(30)	NOT NULL
	,SortNo					INT				NOT NULL	DEFAULT 0
	,PRIMARY KEY (SummaryID,ID)
)
;


