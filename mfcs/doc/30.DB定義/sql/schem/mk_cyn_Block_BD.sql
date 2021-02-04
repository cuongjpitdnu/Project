-- -------------------------------------------------------------------
-- 生産管理システム 物量テーブル
--
--
-- Author T.Ito
-- Create 2020/10/20(Tue)
-- -------------------------------------------------------------------

DROP TABLE Cyn_Block_BD
/

CREATE TABLE Cyn_Block_BD(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    ProjectID     INT           NOT NULL, /* 主キー */
    OrderNo       NVARCHAR(10)  NOT NULL, /* 主キー */
    ID            INT           NOT NULL, /* 主キー */
    Name          NVARCHAR(50)  NOT NULL, /* 主キー */
    BKumiku       NVARCHAR(1)   NOT NULL, /* 主キー */
    Kotei         NVARCHAR(2)   NOT NULL, /* 主キー */
    KKumiku       NVARCHAR(1)   NOT NULL, /* 主キー */
    BD_Code       NVARCHAR(5)   NOT NULL,
    BData         DECIMAL(8, 2) NOT NULL DEFAULT 0,
    PRIMARY KEY (ProjectID, OrderNo, ID, Name, BKumiku, Kotei, KKumiku))
/
