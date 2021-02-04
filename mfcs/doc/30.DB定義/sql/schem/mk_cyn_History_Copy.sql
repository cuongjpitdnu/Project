-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� �����P�[�X�R�s�[�����e�[�u��
--
--
-- Author T.Ito
-- Create 2020/10/20(Tue)
-- -------------------------------------------------------------------

DROP TABLE Cyn_History_Copy
/

CREATE TABLE Cyn_History_Copy(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    ID            INT           NOT NULL, /* ��L�[ */
    Import_User   NVARCHAR(20)  NOT NULL,
    Import_Date   DATETIME      NOT NULL,
    ProjectID_C   INT           NOT NULL,
    OrderNo_C     NVARCHAR(10)  NOT NULL,
    ProjectID_P   INT           NOT NULL,
    OrderNo_P     NVARCHAR(10)  NOT NULL,
    StatusFlag    INT           NOT NULL DEFAULT 0,
    PRIMARY KEY (ID))
/
