-- -------------------------------------------------------------------
-- 生産管理システム 職種マスタテーブル
--
--
-- Author T.Nishida
-- Create 2020/09/11(Fri)
-- -------------------------------------------------------------------

DROP TABLE mstSyokusyu;


CREATE TABLE mstSyokusyu (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,Code					NCHAR(5)		NOT NULL	
	,Name					NVARCHAR(50)	NOT NULL	
	,Nick					NVARCHAR(50)	NOT NULL	
	,ForeColor				INT				NOT NULL	
	,BackColor				INT				NOT NULL	
	,PRIMARY KEY (Code)
)
;


