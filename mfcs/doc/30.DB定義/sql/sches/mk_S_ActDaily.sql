-- -------------------------------------------------------------------
-- 生産管理システム 小日程 - 日々実績データテーブル
--
--
-- Author T.Ito
-- Create 2020/12/07(Mon)
-- -------------------------------------------------------------------

DROP TABLE S_ActDaily
/

CREATE TABLE S_ActDaily(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    OrderNo       NVARCHAR(10)  NOT NULL, /* 主キー */
    JobID         INT           NOT NULL, /* 主キー */
    WkDate        DATE          NOT NULL, /* 主キー */
    AcHr          DECIMAL(8,2)  NOT NULL DEFAULT 0,
    S_BData       DECIMAL(8,2)  NOT NULL DEFAULT 0,
    S_HC          DECIMAL(8,2)  NOT NULL DEFAULT 0,
    Rate          DECIMAL(7,6)  NOT NULL DEFAULT 0,
    PRIMARY KEY (OrderNo, JobID, WkDate))
/
