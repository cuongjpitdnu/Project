-- -------------------------------------------------------------------
-- 生産管理システム ワークアイテムID管理
--
--
-- Author T.Nishida
-- Create 2020/10/07(Wed)
-- -------------------------------------------------------------------

DROP TABLE WorkItemIDList;


CREATE TABLE WorkItemIDList (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,ID						BIGINT			NOT NULL
	,WorkItemID				INT				NOT NULL
	,PRIMARY KEY (ID)
)
;


