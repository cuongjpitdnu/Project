-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� ������ - �����捞�����e�[�u��
--
--
-- Author T.Ito
-- Create 2020/11/13(Fri)
-- -------------------------------------------------------------------

DROP TABLE S_History_Nittei
/

CREATE TABLE S_History_Nittei(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    ID            INT           NOT NULL, /* ��L�[ */
    Import_User   NVARCHAR(20)  NOT NULL,
    Import_Date   DATETIME      NOT NULL,
    ProjectID     INT           NOT NULL,
    OrderNo       NVARCHAR(10)  NOT NULL,
    StatusFlag    INT           NOT NULL DEFAULT 0,
    PRIMARY KEY (ID))
/
