-- -------------------------------------------------------------------
-- 生産管理システム 小日程 - 原単マスタテーブル
--
--
-- Author T.Ito
-- Create 2020/12/07(Mon)
-- -------------------------------------------------------------------

DROP TABLE S_mstGentan
/

CREATE TABLE S_mstGentan(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    GroupID       INT           NOT NULL DEFAULT 0, /* 主キー */
    OrderNo       NVARCHAR(10)  NOT NULL DEFAULT '_', /* 主キー */
    Struct        NCHAR(4)      NOT NULL DEFAULT '*', /* 主キー */
    Category      NCHAR(6)      NOT NULL DEFAULT '*', /* 主キー */
    Kumiku        NCHAR(1)      NOT NULL DEFAULT '*', /* 主キー */
    AcFloor       NCHAR(10)     NOT NULL DEFAULT '*', /* 主キー */
    AcMac         NCHAR(8)      NOT NULL DEFAULT '*', /* 主キー */
    DistCode      NCHAR(5)      NOT NULL DEFAULT '*', /* 主キー */
    Gentan        DECIMAL(4, 3) NOT NULL DEFAULT 0,
    PRIMARY KEY (GroupID, OrderNo, Struct, Category, Kumiku, AcFloor, AcMac, DistCode))
/
