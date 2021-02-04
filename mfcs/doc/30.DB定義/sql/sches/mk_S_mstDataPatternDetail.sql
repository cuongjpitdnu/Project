-- -------------------------------------------------------------------
-- 生産管理システム 小日程 - 計画マスタテーブル
--
--
-- Author T.Ito
-- Create 2020/11/13(Fri)
-- Update 2020/12/04(Fri) 外部参照キー追加 by Toshinori Ito
-- -------------------------------------------------------------------

DROP TABLE S_mstDataPatternDetail
/

CREATE TABLE S_mstDataPatternDetail(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    PID           INT           NOT NULL DEFAULT 0, /* 主キー */
    AF            NCHAR(1)      NOT NULL DEFAULT 'A',
    StartMargin   INT           NOT NULL DEFAULT 0,
    PlDateNo      INT           NOT NULL DEFAULT 0, /* 主キー */
    Days          INT           NOT NULL DEFAULT 1,
    EndMargin     INT           NOT NULL DEFAULT 0,
    PRIMARY KEY (PID, PlDateNo),
    CONSTRAINT FK_S_mstDataPatternDetail FOREIGN KEY(PID) 
    REFERENCES S_mstDataDetail(ID))
/
