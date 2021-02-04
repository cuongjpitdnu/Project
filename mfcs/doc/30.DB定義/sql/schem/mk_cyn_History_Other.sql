-- -------------------------------------------------------------------
-- 生産管理システム 他区分中日程取込履歴テーブル
--
--
-- Author T.Ito
-- Create 2020/10/20(Tue)
-- -------------------------------------------------------------------

DROP TABLE Cyn_History_Other
/

CREATE TABLE Cyn_History_Other(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    ID            INT           NOT NULL, /* 主キー */
    Import_User   NVARCHAR(20)  NOT NULL,
    Import_Date   DATETIME      NOT NULL,
    ProjectID     INT           NOT NULL,
    OrderNo       NVARCHAR(10)  NOT NULL,
    CynProjectID  INT           NOT NULL,
    CynOrderNo    NVARCHAR(10)  NOT NULL,
    StatusFlag    INT           NOT NULL DEFAULT 0,
    PRIMARY KEY (ID))
/
