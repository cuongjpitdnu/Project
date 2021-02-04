-- -------------------------------------------------------------------
-- 生産管理システム 検査種類マスタテーブル
--
--
-- Author T.Ito
-- Create 2020/12/18(Fri)
-- -------------------------------------------------------------------

DROP TABLE mstTest
/

CREATE TABLE mstTest(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    Code          INT           NOT NULL, /* 主キー */
    Name          NVARCHAR(40)  NOT NULL,
    Nick          NVARCHAR(40),
    PRIMARY KEY (Code))
/
