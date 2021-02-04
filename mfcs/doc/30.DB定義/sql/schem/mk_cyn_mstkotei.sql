-- -------------------------------------------------------------------
-- 生産管理システム 工程要素マスタテーブル
--
--
-- Author T.Ito
-- Create 2020/08/18(Tue)
-- -------------------------------------------------------------------

DROP TABLE Cyn_mstKotei
/

CREATE TABLE Cyn_mstKotei(
    Updated_at    DATETIME     NOT NULL DEFAULT GETDATE(),
    CKind         INT          NOT NULL, /* 主キー */
    Code          NVARCHAR(2)  NOT NULL, /* 主キー */
    Name          NVARCHAR(10) NOT NULL,
    Nick          NVARCHAR(10) NOT NULL,
    DelFlag       BIT          NOT NULL DEFAULT 0,
    OutFlag       INT          NOT NULL DEFAULT 0,
    PRIMARY KEY (CKind, Code))
/
