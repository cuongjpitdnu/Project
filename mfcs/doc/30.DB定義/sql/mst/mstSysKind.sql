-- -------------------------------------------------------------------
-- 生産管理システム システム種類マスタテーブル
--
--
-- Author T.Nishida
-- Create 2020/07/06(Mon)
-- -------------------------------------------------------------------

DROP TABLE mstSysKind;


CREATE TABLE mstSysKind (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,ID						INT				NOT NULL	
	,SysName				NVARCHAR(50)	NOT NULL	
	,SysNick				NVARCHAR(50)	NOT NULL	
	,URL					NVARCHAR(20)	NOT NULL	
	,SortNo					INT				NOT NULL	
	,PRIMARY KEY (ID)
)
;


