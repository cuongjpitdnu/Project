-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� Excel�捞�����f�[�^�e�[�u��
--
--
-- Author T.Ito
-- Create 2020/08/24(Mon)
-- -------------------------------------------------------------------

DROP TABLE Cyn_Excel_History
/

CREATE TABLE Cyn_Excel_History(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    ID            INT           NOT NULL, /* ��L�[ */
    Import_User   NVARCHAR(20)  NOT NULL,
    Import_Date   DATETIME      NOT NULL,
    ProjectID     INT           NOT NULL,
    OrderNo       NVARCHAR(10)  NOT NULL,
    StatusFlag    INT           NOT NULL DEFAULT 0,
    PRIMARY KEY (ID))
/
