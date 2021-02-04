-- -------------------------------------------------------------------
-- 生産管理システム 保存前一時Excel取込ログ表示用データテーブル
--
--
-- Author T.Ito
-- Create 2020/09/24(Thu)
-- -------------------------------------------------------------------

DROP TABLE Cyn_Temp_Excel_LogDataVis
/

CREATE TABLE Cyn_Temp_Excel_LogDataVis(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    ProjectID     INT           NOT NULL, /* 主キー */
    OrderNo       NVARCHAR(10)  NOT NULL, /* 主キー */
    CKind         INT           NOT NULL, /* 主キー */
    PID           INT           NOT NULL,
    ID            INT           NOT NULL, /* 主キー */
    Log           NVARCHAR(500),
    T_Name        NVARCHAR(50),
    T_BKumiku     NVARCHAR(1),
    Name          NVARCHAR(50),
    BKumiku       NVARCHAR(1),
    Gen           NVARCHAR(1),
    Kotei         NVARCHAR(2),
    KKumiku       NVARCHAR(1),
    SDate         DATE,
    EDate         DATE,
    Days          INT,
    LinkDays      INT,
    Floor         NVARCHAR(10),
    BD_Code       NVARCHAR(5),
    PRIMARY KEY (ProjectID, OrderNo, CKind, ID))
/
