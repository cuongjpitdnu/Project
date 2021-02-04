-- -------------------------------------------------------------------
-- 生産管理システム システムメニューマスタテーブル
--
--
-- Author T.Nishida
-- Create 2020/07/06(Mon)
-- -------------------------------------------------------------------

DROP TABLE mstSysMenu;


CREATE TABLE mstSysMenu (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,ID						INT				NOT NULL	
	,SysKindID				INT				NOT NULL	
	,MenuName				NVARCHAR(50)	NOT NULL	
	,MenuNick				NVARCHAR(50)	NOT NULL	
	,URL					NVARCHAR(20)	NOT NULL	
	,SortNo					INT				NOT NULL	
	,PRIMARY KEY (ID)
)
;


