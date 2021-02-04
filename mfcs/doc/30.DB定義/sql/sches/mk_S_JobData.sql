-- -------------------------------------------------------------------
-- 生産管理システム 小日程 - 工程情報データテーブル
--
--
-- Author T.Ito
-- Create 2020/11/13(Fri)
-- Update 2020/12/04(Fri) KeshiPattern,KeshiCode追加 by Toshinori Ito
-- -------------------------------------------------------------------

DROP TABLE S_JobData
/

CREATE TABLE S_JobData(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    ProjectID     INT           NOT NULL, /* 主キー */
    OrderNo       NVARCHAR(10)  NOT NULL DEFAULT '*', /* 主キー */
    WorkItemID    INT,
    ID            INT           NOT NULL, /* 主キー */
    Level_Job     INT           NOT NULL DEFAULT 0,
    PID           INT           NOT NULL DEFAULT 0,
    PID_Stn       INT           NOT NULL DEFAULT 0,
    BKumiku       NVARCHAR(1),
    KKumiku       NVARCHAR(1),
    KakoKumiku    NVARCHAR(1),
    DispName1     NVARCHAR(100) NOT NULL DEFAULT '*',
    DispName2     NVARCHAR(100),
    DispName3     NVARCHAR(100),
    CKind         INT,
    Kotei         NVARCHAR(2),
    AcFloor       NVARCHAR(10),
    AcMac         NCHAR(8),
    DistCode      NCHAR(5),
    BD_Code       NVARCHAR(5),
    MngBData      DECIMAL(8, 2),
    BData1        DECIMAL(8, 2),
    BData2        DECIMAL(8, 2),
    BData3        DECIMAL(8, 2),
    BData4        DECIMAL(8, 2),
    BData5        DECIMAL(8, 2),
    BData6        DECIMAL(8, 2),
    BData7        DECIMAL(8, 2),
    BData8        DECIMAL(8, 2),
    BData9        DECIMAL(8, 2),
    BData10       DECIMAL(8, 2),
    BData11       DECIMAL(8, 2),
    BData12       DECIMAL(8, 2),
    BData13       DECIMAL(8, 2),
    BData14       DECIMAL(8, 2),
    BData15       DECIMAL(8, 2),
    BData16       DECIMAL(8, 2),
    BData17       DECIMAL(8, 2),
    BData18       DECIMAL(8, 2),
    BData19       DECIMAL(8, 2),
    BData20       DECIMAL(8, 2),
    BData21       DECIMAL(8, 2),
    BData22       DECIMAL(8, 2),
    BData23       DECIMAL(8, 2),
    BData24       DECIMAL(8, 2),
    BData25       DECIMAL(8, 2),
    BData26       DECIMAL(8, 2),
    BData27       DECIMAL(8, 2),
    BData28       DECIMAL(8, 2),
    BData29       DECIMAL(8, 2),
    BData30       DECIMAL(8, 2),
    BData31       DECIMAL(8, 2),
    Item          NCHAR(5)      NOT NULL DEFAULT '00000',
    AcsDate       DATE,
    AceDate       DATE,
    PrsDate       DATE,
    PreDate       DATE,
    AcHr          DECIMAL(8, 2),
    PlStdHr       DECIMAL(8, 2),
    EpHr          DECIMAL(8, 2),
    HS            DECIMAL(8, 2),
    HK            DECIMAL(8, 2),
    PrRateNow     DECIMAL(7, 6) NOT NULL DEFAULT 0,
    WBSCode       NVARCHAR(24),
    N_BlockName   NVARCHAR(20),
    N_BKumiku     NVARCHAR(1),
    N_Kotei       NVARCHAR(2),
    N_KKumiku     NVARCHAR(1),
    IsOriginal    INT           NOT NULL DEFAULT 0,
    BaseID        INT           NOT NULL DEFAULT 0,
    KeshiPattern  INT,
    KeshiCode     INT           NOT NULL DEFAULT 0,
    PRIMARY KEY (ProjectID, OrderNo, ID))
/
