-- -------------------------------------------------------------------
-- 生産管理システム 小日程 - 個人別実績データテーブル
--
--
-- Author T.Ito
-- Create 2021/01/15(Fri)
-- -------------------------------------------------------------------

DROP TABLE S_ActInd
/

CREATE TABLE S_ActInd(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    OrderNo       NVARCHAR(10)  NOT NULL, /* 主キー */
    JobID         INT           NOT NULL, /* 主キー */
    WkDate        DATE          NOT NULL, /* 主キー */
    Mem_ID        INT           NOT NULL DEFAULT 0, /* 主キー */
    AcHr          DECIMAL(8,2)  NOT NULL DEFAULT 0,
    GroupID       INT                    DEFAULT 0,
    Com_ID        INT           NOT NULL DEFAULT 0, /* 主キー */
    PRIMARY KEY (OrderNo, JobID, WkDate, Mem_ID, Com_ID))
/
