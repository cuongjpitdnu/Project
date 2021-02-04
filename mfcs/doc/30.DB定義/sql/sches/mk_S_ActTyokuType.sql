-- -------------------------------------------------------------------
-- 生産管理システム 小日程 - 直接入力テーブル
--
--
-- Author T.Ito
-- Create 2021/01/15(Fri)
-- -------------------------------------------------------------------

DROP TABLE S_ActTyokuType
/

CREATE TABLE S_ActTyokuType(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    WkDate        DATE          NOT NULL, /* 主キー */
    GroupID       INT           NOT NULL, /* 主キー */
    TyokuType     INT           NOT NULL DEFAULT 0,
    PRIMARY KEY (WkDate, GroupID))
/
