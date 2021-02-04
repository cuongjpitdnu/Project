-- -------------------------------------------------------------------
-- 生産管理システム 小日程 - 休暇データテーブル
--
--
-- Author T.Ito
-- Create 2020/12/18(Fri)
-- -------------------------------------------------------------------

DROP TABLE S_Kyuka
/

CREATE TABLE S_Kyuka(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    WkDate        DATE          NOT NULL, /* 主キー */
    GroupID       INT           NOT NULL, /* 主キー */
    MemID         INT           NOT NULL, /* 主キー */
    KyukaID       INT           NOT NULL, /* 主キー */
    Hr            DECIMAL(8, 2) NOT NULL DEFAULT 0,
    PRIMARY KEY (WkDate, GroupID, MemID, KyukaID))
/
