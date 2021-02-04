-- -------------------------------------------------------------------
-- 生産管理システム 工程情報(中日程を親とした中日程レベルデータ)データテーブル
--
--
-- Author T.Ito
-- Create 2020/09/09(Wed)
-- -------------------------------------------------------------------

DROP TABLE Cyn_C_Plan
/

CREATE TABLE Cyn_C_Plan(
    Updated_at    DATETIME     NOT NULL DEFAULT GETDATE(),
    ProjectID     INT          NOT NULL, /* 主キー */
    OrderNo       NVARCHAR(10) NOT NULL, /* 主キー */
    WorkItemID    INT,
    No            INT          NOT NULL, /* 主キー */
    KoteiNo       INT          NOT NULL, /* 主キー */
    Kotei         NVARCHAR(2)  NOT NULL,
    KKumiku       NVARCHAR(1)  NOT NULL,
    Floor         NVARCHAR(10),
    BD_Code       NVARCHAR(5),
    BData         DECIMAL(8, 2)         DEFAULT 0,
    HC            DECIMAL(8, 2)         DEFAULT 0,
    Days          INT          NOT NULL DEFAULT 1,
    N_KoteiNo     INT          NOT NULL DEFAULT 0,
    N_Kotei       NVARCHAR(2),
    N_KKumiku     NVARCHAR(1),
    N_Link        INT          NOT NULL DEFAULT 0,
    Del_Date      DATE,
    B_PlSDate     DATE,
    B_SG_Date     DATE,
    B_T_Date      DATE,
    Jyoban        NVARCHAR(4),
    PRIMARY KEY (ProjectID, OrderNo, No, KoteiNo))
/
