-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� �����P�[�X�R�s�[���O�e�[�u��
--
--
-- Author T.Ito
-- Create 2020/10/20(Tue)
-- -------------------------------------------------------------------

DROP TABLE Cyn_LogData_Copy
/

CREATE TABLE Cyn_LogData_Copy(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    HistoryID     INT           NOT NULL, /* ��L�[ */
    ID            INT           NOT NULL, /* ��L�[ */
    Name          NVARCHAR(50)  NOT NULL,
    BKumiku       NVARCHAR(1)   NOT NULL,
    Log           NVARCHAR(500),
    PRIMARY KEY (HistoryID, ID))
/
