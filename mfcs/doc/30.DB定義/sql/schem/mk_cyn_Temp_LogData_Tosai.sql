-- -------------------------------------------------------------------
-- 生産管理システム 保存前一時搭載日程取込ログデータテーブル
--
--
-- Author T.Ito
-- Create 2020/08/28(Fri)
-- -------------------------------------------------------------------

DROP TABLE Cyn_Temp_LogData_Tosai
/

CREATE TABLE Cyn_Temp_LogData_Tosai(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    OrderNo       NVARCHAR(10)  NOT NULL, /* 主キー */
    CKind         INT           NOT NULL, /* 主キー */
    ID            INT           NOT NULL, /* 主キー */
    BlockName     NVARCHAR(20)  NOT NULL,
    BKumiku       NVARCHAR(1)   NOT NULL,
    Gen           NVARCHAR(1)   NOT NULL,
    Log           NVARCHAR(500),
    TDate         DATE,
    KDate         DATE,
    SG_SDate      DATE,
    SG_EDate      DATE,
    NxtName       NVARCHAR(50),
    NxtBKumiku    NVARCHAR(1),
    AMDFlag       INT,
    OldBlockName  NVARCHAR(20),
    PRIMARY KEY (OrderNo, CKind, ID))
/
