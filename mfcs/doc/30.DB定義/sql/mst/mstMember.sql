-- -------------------------------------------------------------------
-- 生産管理システム 人員マスタテーブル
--
--
-- Author T.Nishida
-- Create 2020/07/08(Wed)
-- -------------------------------------------------------------------

DROP TABLE mstMember;


CREATE TABLE mstMember (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,Up_User				NVARCHAR(20)				
	,ID						INT				NOT NULL	
	,Name					NVARCHAR(50)	NOT NULL	
	,Yomi					NVARCHAR(50)	NOT NULL	
	,Nick					NVARCHAR(50)	NOT NULL	
	,RetireDate				DATETIME					
	,PRIMARY KEY (ID)
)
;


