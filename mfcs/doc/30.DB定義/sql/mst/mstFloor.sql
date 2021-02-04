-- -------------------------------------------------------------------
-- 生産管理システム 棟マスタテーブル
--
--
-- Author T.Nishida
-- Create 2020/07/08(Wed)
-- -------------------------------------------------------------------

DROP TABLE mstFloor;


CREATE TABLE mstFloor (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,Up_User				NVARCHAR(20)				
	,Code					NVARCHAR(10)	NOT NULL	
	,Name					NVARCHAR(50)				
	,Nick					NVARCHAR(50)				
	,Nick1					NVARCHAR(10)				
	,BD_P_D					INT				NOT NULL	DEFAULT 0
	,HA_P_D					INT				NOT NULL	DEFAULT 0
	,OwnerGroup				NVARCHAR(10)				
	,SortNo					INT				NOT NULL	DEFAULT 0
	,ViewFlag				BIT				NOT NULL	DEFAULT 1
	,PRIMARY KEY (Code)
)
;


