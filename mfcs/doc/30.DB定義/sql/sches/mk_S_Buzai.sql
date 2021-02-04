-- -------------------------------------------------------------------
-- 生産管理システム 小日程 - 部材データテーブル
--
--
-- Author T.Ito
-- Create 2021/01/15(Fri)
-- -------------------------------------------------------------------

DROP TABLE S_Buzai
/

CREATE TABLE S_Buzai(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    ProjectID     INT           NOT NULL, /* 主キー */
    OrderNo       NVARCHAR(10)  NOT NULL, /* 主キー */
    JobID         INT           NOT NULL, /* 主キー */
    BuzaiName     NVARCHAR(14)  NOT NULL, /* 主キー */
    BData         DECIMAL(8, 2) NOT NULL DEFAULT 0,
    Weight        DECIMAL(8, 1) NOT NULL DEFAULT 0,
    List_Page     NVARCHAR(6),
    Keijyo        NVARCHAR(2),
    Length        INT,
    DoneFlag      NVARCHAR(1)            DEFAULT '0',
    DoneDate      DATE,
    AcHr          DECIMAL(8, 2) NOT NULL DEFAULT 0,
    PlStdHr       DECIMAL(8, 2) NOT NULL DEFAULT 0,
    Note          NVARCHAR(40),
    PRIMARY KEY (ProjectID, OrderNo, JobID, BuzaiName))
/
