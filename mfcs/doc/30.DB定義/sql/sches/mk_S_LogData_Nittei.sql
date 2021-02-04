-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� ������ - �����捞���O�e�[�u��
--
--
-- Author T.Ito
-- Create 2020/11/13(Fri)
-- -------------------------------------------------------------------

DROP TABLE S_LogData_Nittei
/

CREATE TABLE S_LogData_Nittei(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    HistoryID     INT           NOT NULL, /* ��L�[ */
    ID            INT           NOT NULL, /* ��L�[ */
    DispName1     NVARCHAR(100) NOT NULL,
    DispName2     NVARCHAR(100),
    DispName3     NVARCHAR(100),
    Kumiku        NVARCHAR(1),
    FloorCode     NVARCHAR(10),
    MacCode       NCHAR(8),
    DistCode      NCHAR(5),
    BDCode        NVARCHAR(5),
    MngBData      DECIMAL(8, 2),
    SDate         DATE,
    EDate         DATE,
    Item          NCHAR(5)               DEFAULT '00000',
    PlStdHr       DECIMAL(8, 2),
    EpHr          DECIMAL(8, 2),
    HS            DECIMAL(8, 2),
    HK            DECIMAL(8, 2),
    WBSCode       NVARCHAR(24),
    Log           NVARCHAR(500),
    KeshiPattern  INT                    DEFAULT 0,
    KeshiCode     INT                    DEFAULT 1,
    PRIMARY KEY (HistoryID, ID))
/
