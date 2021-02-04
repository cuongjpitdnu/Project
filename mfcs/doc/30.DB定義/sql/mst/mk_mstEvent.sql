-- -------------------------------------------------------------------
-- 生産管理システム 主要行事名称マスタテーブル
--
--
-- Author T.Ito
-- Create 2020/12/18(Fri)
-- -------------------------------------------------------------------

DROP TABLE mstEvent
/

CREATE TABLE mstEvent(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    Name          NVARCHAR(40)  NOT NULL, /* 主キー */
    Nick          NVARCHAR(40),
    GroupID       INT           NOT NULL DEFAULT 0, /* 主キー */
    PRIMARY KEY (Name, GroupID))
/
