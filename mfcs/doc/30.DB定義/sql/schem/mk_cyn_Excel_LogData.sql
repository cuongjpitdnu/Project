-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� Excel�捞���O�f�[�^�e�[�u��
--
--
-- Author T.Ito
-- Create 2020/08/24(Mon)
-- -------------------------------------------------------------------

DROP TABLE Cyn_Excel_LogData
/

CREATE TABLE Cyn_Excel_LogData(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    HistoryID     INT           NOT NULL, /* ��L�[ */
    ID            INT           NOT NULL, /* ��L�[ */
    Name          NVARCHAR(50),
    BKumiku       NVARCHAR(1),
    Kotei         NVARCHAR(2),
    KKumiku       NVARCHAR(1),
    SDate         DATE,
    Days          INT,
    LinkDays      INT,
    Floor         NVARCHAR(10),
    BD_Code       NVARCHAR(5),
    Log           NVARCHAR(500),
    PRIMARY KEY (HistoryID, ID))
/
