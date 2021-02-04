-- -------------------------------------------------------------------
-- 生産管理システム 小日程 - 作業指示テーブル
--
--
-- Author T.Ito
-- Create 2021/01/15(Fri)
-- -------------------------------------------------------------------

DROP TABLE S_ConJobMem
/

CREATE TABLE S_ConJobMem(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    Work_Date     DATE          NOT NULL, /* 主キー */
    GroupID       INT           NOT NULL DEFAULT 0, /* 主キー */
    Com_ID        INT           NOT NULL DEFAULT 0, /* 主キー */
    Mem_ID        INT           NOT NULL DEFAULT 0, /* 主キー */
    OrderNo       NVARCHAR(10),
    JobID         INT,
    KanID         INT,
    Item          NCHAR(5)               DEFAULT '00000',
    Bikou         NVARCHAR(40),
    IsKasei       BIT           NOT NULL DEFAULT 0,
    UkeGroupID    INT,
    PRIMARY KEY (Work_Date, GroupID, Com_ID, Mem_ID))
/
