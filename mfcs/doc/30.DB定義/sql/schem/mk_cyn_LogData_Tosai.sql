-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� ���ړ����捞���O�f�[�^�e�[�u��
--
--
-- Author T.Ito
-- Create 2020/08/24(Mon)
-- -------------------------------------------------------------------

DROP TABLE Cyn_LogData_Tosai
/

CREATE TABLE Cyn_LogData_Tosai(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    HistoryID     INT           NOT NULL, /* ��L�[ */
    ID            INT           NOT NULL, /* ��L�[ */
    BlockName     NVARCHAR(20)  NOT NULL,
    BKumiku       NVARCHAR(1)   NOT NULL,
    Gen           NVARCHAR(1)   NOT NULL,
    Log           NVARCHAR(500),
    TDate         DATE,
    KDate         DATE,
    SG_SDate      DATE,
    SG_EDate      DATE,
    NxtName       NVARCHAR(50),
    NxtBKumiku    NVARCHAR(1),
    PRIMARY KEY (HistoryID, ID))
/
