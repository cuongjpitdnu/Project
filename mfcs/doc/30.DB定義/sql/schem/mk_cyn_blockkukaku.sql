-- -------------------------------------------------------------------
-- 生産管理システム ブロック/区画情報データテーブル
--
--
-- Author T.Ito
-- Create 2020/08/21(Fri)
-- Update 2020/12/03(Thu) 重量関連は8,1に桁数変更 by Toshinori Ito
-- -------------------------------------------------------------------

DROP TABLE Cyn_BlockKukaku
/

CREATE TABLE Cyn_BlockKukaku(
    Updated_at    DATETIME     NOT NULL DEFAULT GETDATE(),
    ProjectID     INT          NOT NULL, /* 主キー */
    OrderNo       NVARCHAR(10) NOT NULL, /* 主キー */
    WorkItemID    INT,
    CKind         INT          NOT NULL,
    T_Name        NVARCHAR(50) NOT NULL,
    T_BKumiku     NVARCHAR(1)  NOT NULL,
    No            INT          NOT NULL, /* 主キー */
    Name          NVARCHAR(50) NOT NULL,
    BKumiku       NVARCHAR(1)  NOT NULL,
    N_No          INT          NOT NULL DEFAULT 0,
    N_Name        NVARCHAR(50),
    N_BKumiku     NVARCHAR(1),
    Struct        NVARCHAR(2)           DEFAULT '5',
    Category      NVARCHAR(5),
    Width         DECIMAL(4, 1),
    Length        DECIMAL(4, 1),
    Height        DECIMAL(4, 1),
    Weight        DECIMAL(8, 1),
    Zu_No         NVARCHAR(6),
    KG_Weight     DECIMAL(8, 1)         DEFAULT 0,
    True_Weight   BIT          NOT NULL DEFAULT 0,
    Is_Magari     BIT          NOT NULL DEFAULT 0,
    Del_Date      DATE,
    O_CKind       INT,
    PRIMARY KEY (ProjectID, OrderNo, No))
/
