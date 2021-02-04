-- -------------------------------------------------------------------
-- 生産管理システム ユーザマスタテーブル
--
--
-- Author T.Nishida
-- Create 2020/07/06(Mon)
-- -------------------------------------------------------------------

DROP TABLE mstUser;


CREATE TABLE mstUser (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,UserID					NVARCHAR(20)	NOT NULL	
	,UserName				NVARCHAR(100)	NOT NULL	
	,SysKindID				NVARCHAR(100)				
	,SysMenuID_Update		NVARCHAR(200)				
	,SysMenuID_Read			NVARCHAR(200)				
	,PRIMARY KEY (UserID)
)
;


