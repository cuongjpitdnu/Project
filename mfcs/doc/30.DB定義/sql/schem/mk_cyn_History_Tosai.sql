-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� ���ړ����捞�����f�[�^�e�[�u��
--
--
-- Author T.Ito
-- Create 2020/08/24(Mon)
-- -------------------------------------------------------------------

DROP TABLE Cyn_History_Tosai
/

CREATE TABLE Cyn_History_Tosai(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    ID            INT           NOT NULL, /* ��L�[ */
    Import_User   NVARCHAR(20)  NOT NULL,
    Import_Date   DATETIME      NOT NULL,
    ProjectID     INT           NOT NULL,
    OrderNo       NVARCHAR(10)  NOT NULL,
    CynProjectID  INT           NOT NULL,
    CynOrderNo    NVARCHAR(10)  NOT NULL,
    StatusFlag    INT           NOT NULL DEFAULT 0,
    PRIMARY KEY (ID))
/
