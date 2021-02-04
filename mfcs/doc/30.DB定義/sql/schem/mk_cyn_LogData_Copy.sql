-- -------------------------------------------------------------------
-- 生産管理システム 検討ケースコピーログテーブル
--
--
-- Author T.Ito
-- Create 2020/10/20(Tue)
-- -------------------------------------------------------------------

DROP TABLE Cyn_LogData_Copy
/

CREATE TABLE Cyn_LogData_Copy(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    HistoryID     INT           NOT NULL, /* 主キー */
    ID            INT           NOT NULL, /* 主キー */
    Name          NVARCHAR(50)  NOT NULL,
    BKumiku       NVARCHAR(1)   NOT NULL,
    Log           NVARCHAR(500),
    PRIMARY KEY (HistoryID, ID))
/
