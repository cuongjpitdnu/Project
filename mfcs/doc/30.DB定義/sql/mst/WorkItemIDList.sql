-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� ���[�N�A�C�e��ID�Ǘ�
--
--
-- Author T.Nishida
-- Create 2020/10/07(Wed)
-- -------------------------------------------------------------------

DROP TABLE WorkItemIDList;


CREATE TABLE WorkItemIDList (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,ID						BIGINT			NOT NULL
	,WorkItemID				INT				NOT NULL
	,PRIMARY KEY (ID)
)
;


