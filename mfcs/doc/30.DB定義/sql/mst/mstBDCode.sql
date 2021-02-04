-- -------------------------------------------------------------------
-- 生産管理システム 物量マスタテーブル
--
--
-- Author T.Nishida
-- Create 2020/07/08(Wed)
-- -------------------------------------------------------------------

DROP TABLE mstBDCode;


CREATE TABLE mstBDCode (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,Up_User				NVARCHAR(20)				
	,Code					NVARCHAR(5)		NOT NULL	
	,Name					NVARCHAR(50)				
	,Nick					NVARCHAR(50)				
	,ViewFlag				BIT				NOT NULL	DEFAULT 1
	,PRIMARY KEY (Code)
)
;


