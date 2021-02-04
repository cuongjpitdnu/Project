-- -------------------------------------------------------------------
-- 生産管理システム 能力時間マスタテーブル
--
--
-- Author T.Nishida
-- Create 2020/07/08(Wed)
-- -------------------------------------------------------------------

DROP TABLE mstAbility;


CREATE TABLE mstAbility (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,Up_User				NVARCHAR(20)				
	,ID						INT				NOT NULL	
	,AbilityName			NVARCHAR(100)	NOT NULL	
	,GroupID				INT				NOT NULL	DEFAULT 0
	,FloorCode				NVARCHAR(10)				
	,DistCode				NCHAR(5)					
	,Sdate					DATETIME		NOT NULL	
	,Edate					DATETIME					
	,Hr						DECIMAL(8,2)				DEFAULT 0
	,PRIMARY KEY (ID)
)
;


