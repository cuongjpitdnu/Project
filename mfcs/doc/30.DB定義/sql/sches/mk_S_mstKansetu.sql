-- -------------------------------------------------------------------
-- 生産管理システム 小日程 - 間接コードマスタテーブル
--
--
-- Author T.Ito
-- Create 2020/12/18(Fri)
-- -------------------------------------------------------------------

DROP TABLE S_mstKansetu
/

CREATE TABLE S_mstKansetu(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    ID            INT           NOT NULL, /* 主キー */
    PID           INT           NOT NULL,
    Lv_No         INT           NOT NULL,
    Name          NVARCHAR(40)  NOT NULL,
    Nick          NVARCHAR(40)  NOT NULL,
    GroupID       INT           NOT NULL DEFAULT 0,
    OrderNo       NVARCHAR(10),
    Item          NCHAR(5)               DEFAULT '00000',
    DispFlag      INT           NOT NULL DEFAULT 0,
    TyokukaFlag   INT           NOT NULL DEFAULT 0,
    PRIMARY KEY (ID))
/
