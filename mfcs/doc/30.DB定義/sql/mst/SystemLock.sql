-- -------------------------------------------------------------------
-- 生産管理システム 排他ロックテーブル
--
--
-- Author T.Nishida
-- Create 2020/07/30(Thu)
-- Update 2020/08/17(Mon)
-- -------------------------------------------------------------------

DROP TABLE SystemLock;


CREATE TABLE SystemLock (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,SysKindID				INT				NOT NULL
	,SysMenuID				INT				NOT NULL
	,OptionKey				NVARCHAR(100)	NOT NULL
	,UserID					NVARCHAR(20)	
	,SessionID				NVARCHAR(40)	
	,BeginTime				DATETIME		NOT NULL
	,PRIMARY KEY (SysKindID, SysMenuID, OptionKey)
)
;


