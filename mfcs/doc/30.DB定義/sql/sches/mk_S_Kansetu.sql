-- -------------------------------------------------------------------
-- 生産管理システム 小日程 - 間接データテーブル
--
--
-- Author T.Ito
-- Create 2020/12/18(Fri)
-- -------------------------------------------------------------------

DROP TABLE S_Kansetu
/

CREATE TABLE S_Kansetu(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    WkDate        DATE          NOT NULL, /* 主キー */
    GroupID       INT           NOT NULL, /* 主キー */
    Mem_ID        INT           NOT NULL DEFAULT 0, /* 主キー */
    Com_ID        INT           NOT NULL DEFAULT 0, /* 主キー */
    KanID         INT           NOT NULL, /* 主キー */
    OrderNo       NVARCHAR(10)  NOT NULL, /* 主キー */
    Item          NCHAR(5)      NOT NULL DEFAULT '00000', /* 主キー */
    Bikou         NVARCHAR(40),
    AcHr          DECIMAL(8,2)  NOT NULL DEFAULT 0,
    BData         DECIMAL(8,2)  NOT NULL DEFAULT 0,
    PRIMARY KEY (WkDate, GroupID, Mem_ID, Com_ID, KanID, OrderNo, Item))
/
