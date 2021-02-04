-- -------------------------------------------------------------------
-- 生産管理システム 検査日程テーブル
--
--
-- Author T.Ito
-- Create 2020/12/18(Fri)
-- -------------------------------------------------------------------

DROP TABLE TestDay
/

CREATE TABLE TestDay(
    Updated_at       DATETIME      NOT NULL DEFAULT GETDATE(),
    TestDate         DATE          NOT NULL, /* 主キー */
    ProjectID        INT           NOT NULL, /* 主キー */
    OrderNo          NVARCHAR(10)  NOT NULL, /* 主キー */
    JobID            INT           NOT NULL, /* 主キー */
    TestID           INT           NOT NULL, /* 主キー */
    Pass             Bit                    DEFAULT 0,
    PRIMARY KEY (TestDate, ProjectID, OrderNo, JobID, TestID))
/
