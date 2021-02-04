-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� �v���W�F�N�g�}�X�^
--
--
-- Author T.Nishida
-- Create 2020/08/21(Fri)
-- -------------------------------------------------------------------

DROP TABLE mstProject;


CREATE TABLE mstProject (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,Up_User				NVARCHAR(20)	
	,ID						INT				NOT NULL
	,SysKindID				INT				NOT NULL
	,ListKind				INT				NOT NULL	DEFAULT 0
	,SerialNo				INT				NOT NULL
	,ProjectName			NVARCHAR(50)	NOT NULL
	,PRIMARY KEY (ID)
)
;


