-- -------------------------------------------------------------------
-- 生産管理システム 小日程 - 標準工程マスタテーブル
--
--
-- Author T.Ito
-- Create 2020/11/13(Fri)
-- -------------------------------------------------------------------

DROP TABLE S_mstDataPattern
/

CREATE TABLE S_mstDataPattern(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    ID            INT           NOT NULL DEFAULT 0, /* 主キー */
    Name          NVARCHAR(40)  NOT NULL,
    BaseData      INT           NOT NULL,
    GroupID       INT           NOT NULL DEFAULT 0,
    CKind         INT           NOT NULL,
    BKumiku       NVARCHAR(1)   NOT NULL,
    Kotei         NVARCHAR(2)   NOT NULL,
    KKumiku       NVARCHAR(1)   NOT NULL,
    Floor         NVARCHAR(10)  NOT NULL,
    PRIMARY KEY (ID))
/
