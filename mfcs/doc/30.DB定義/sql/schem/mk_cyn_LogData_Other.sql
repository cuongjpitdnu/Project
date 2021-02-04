-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� ���敪�������捞���O�e�[�u��
--
--
-- Author T.Ito
-- Create 2020/10/20(Tue)
-- -------------------------------------------------------------------

DROP TABLE Cyn_LogData_Other
/

CREATE TABLE Cyn_LogData_Other(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    HistoryID     INT           NOT NULL, /* ��L�[ */
    ID            INT           NOT NULL, /* ��L�[ */
    P_CKind       INT           NOT NULL,
    P_No          INT           NOT NULL,
    P_Name        NVARCHAR(50)  NOT NULL,
    P_BKumiku     NVARCHAR(1)   NOT NULL,
    Log           NVARCHAR(500),
    P_SDate       DATETIME,
    P_EDate       DATETIME,
    PRIMARY KEY (HistoryID, ID))
/
