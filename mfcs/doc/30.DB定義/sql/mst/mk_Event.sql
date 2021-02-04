-- -------------------------------------------------------------------
-- 生産管理システム 主要行事テーブル
--
--
-- Author T.Ito
-- Create 2020/12/18(Fri)
-- -------------------------------------------------------------------

DROP TABLE Event
/

CREATE TABLE Event(
    Updated_at       DATETIME      NOT NULL DEFAULT GETDATE(),
    EventDate        DATE          NOT NULL, /* 主キー */
    ProjectID        INT           NOT NULL, /* 主キー */
    OrderNo          NVARCHAR(10)  NOT NULL, /* 主キー */
    Name             NVARCHAR(40)  NOT NULL, /* 主キー */
    Nick             NVARCHAR(40),
    GroupID          INT           NOT NULL, /* 主キー */
    EndDate          DATE,
    ActEventDate     DATE,
    ActEndDate       DATE,
    ID               INT,
    BigMilestoneFlag INT,
    PRIMARY KEY (EventDate, ProjectID, OrderNo, Name, GroupID))
/
