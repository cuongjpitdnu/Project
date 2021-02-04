-- -------------------------------------------------------------------
-- 生産管理システム 他区分中日程取込ログテーブル
--
--
-- Author T.Ito
-- Create 2020/10/20(Tue)
-- -------------------------------------------------------------------

DROP TABLE Cyn_LogData_Other
/

CREATE TABLE Cyn_LogData_Other(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    HistoryID     INT           NOT NULL, /* 主キー */
    ID            INT           NOT NULL, /* 主キー */
    P_CKind       INT           NOT NULL,
    P_No          INT           NOT NULL,
    P_Name        NVARCHAR(50)  NOT NULL,
    P_BKumiku     NVARCHAR(1)   NOT NULL,
    Log           NVARCHAR(500),
    P_SDate       DATETIME,
    P_EDate       DATETIME,
    PRIMARY KEY (HistoryID, ID))
/
