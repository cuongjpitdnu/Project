-- -------------------------------------------------------------------
-- 生産管理システム 保存前一時小日程展開ログデータテーブル
--
--
-- Author T.Ito
-- Create 2020/11/13(Fri)
-- -------------------------------------------------------------------

DROP TABLE Cyn_Temp_LogData_Tenkai
/

CREATE TABLE Cyn_Temp_LogData_Tenkai(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    ProjectID     INT           NOT NULL, /* 主キー */
    OrderNo       NVARCHAR(10)  NOT NULL, /* 主キー */
    CKind         INT           NOT NULL, /* 主キー */
    PatternID     INT           NOT NULL,
    ID            INT           NOT NULL, /* 主キー */
    T_Name        NVARCHAR(50),
    T_BKumiku     NVARCHAR(1),
    Name          NVARCHAR(50),
    BKumiku       NVARCHAR(1),
    Kotei         NVARCHAR(2),
    KKumiku       NVARCHAR(1),
    Floor         NVARCHAR(10),
    N_Name        NVARCHAR(50),
    N_BKumiku     NVARCHAR(1),
    N_Kotei       NVARCHAR(2),
    N_KKumiku     NVARCHAR(1),
    Kumiku        NVARCHAR(1),
    DistCode      NCHAR(5),
    MacCode       NCHAR(8),
    Item          NCHAR(5)               DEFAULT '00000',
    BD_Code       NVARCHAR(5),
    BD_Code1      NVARCHAR(5),
    MngBData      DECIMAL(8,2)           DEFAULT 0,
    MngBData1     DECIMAL(8,2)           DEFAULT 0,
    BData1        DECIMAL(8,2)           DEFAULT 0,
    BData2        DECIMAL(8,2)           DEFAULT 0,
    BData3        DECIMAL(8,2)           DEFAULT 0,
    BData4        DECIMAL(8,2)           DEFAULT 0,
    BData5        DECIMAL(8,2)           DEFAULT 0,
    BData6        DECIMAL(8,2)           DEFAULT 0,
    BData7        DECIMAL(8,2)           DEFAULT 0,
    BData8        DECIMAL(8,2)           DEFAULT 0,
    BData9        DECIMAL(8,2)           DEFAULT 0,
    BData10       DECIMAL(8,2)           DEFAULT 0,
    BData11       DECIMAL(8,2)           DEFAULT 0,
    BData12       DECIMAL(8,2)           DEFAULT 0,
    BData13       DECIMAL(8,2)           DEFAULT 0,
    BData14       DECIMAL(8,2)           DEFAULT 0,
    BData15       DECIMAL(8,2)           DEFAULT 0,
    BData16       DECIMAL(8,2)           DEFAULT 0,
    BData17       DECIMAL(8,2)           DEFAULT 0,
    BData18       DECIMAL(8,2)           DEFAULT 0,
    BData19       DECIMAL(8,2)           DEFAULT 0,
    BData20       DECIMAL(8,2)           DEFAULT 0,
    BData21       DECIMAL(8,2)           DEFAULT 0,
    BData22       DECIMAL(8,2)           DEFAULT 0,
    BData23       DECIMAL(8,2)           DEFAULT 0,
    BData24       DECIMAL(8,2)           DEFAULT 0,
    BData25       DECIMAL(8,2)           DEFAULT 0,
    BData26       DECIMAL(8,2)           DEFAULT 0,
    BData27       DECIMAL(8,2)           DEFAULT 0,
    BData28       DECIMAL(8,2)           DEFAULT 0,
    BData29       DECIMAL(8,2)           DEFAULT 0,
    BData30       DECIMAL(8,2)           DEFAULT 0,
    BData31       DECIMAL(8,2)           DEFAULT 0,
    HC            DECIMAL(8,2)           DEFAULT 0,
    SDate         DATE,
    EDate         DATE,
    Days          INT,
    AMDFlag       INT,
    Log           NVARCHAR(500),
    PRIMARY KEY (ProjectID, OrderNo, CKind, ID))
/
