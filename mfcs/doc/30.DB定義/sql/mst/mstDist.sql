-- -------------------------------------------------------------------
-- 生産管理システム 職種マスタテーブル
--
--
-- Author T.Nishida
-- Create 2020/07/08(Wed)
-- -------------------------------------------------------------------

DROP TABLE mstDist;


CREATE TABLE mstDist (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,Code					NCHAR(5)		NOT NULL	
	,Name					NVARCHAR(50)	NOT NULL	
	,Nick					NVARCHAR(50)	NOT NULL	
	,ForeColor				INT				NOT NULL	
	,BackColor				INT				NOT NULL	
	,BrushStyle				INT				NOT NULL	DEFAULT 0
	,PRIMARY KEY (Code)
)
;


