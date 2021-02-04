-- -------------------------------------------------------------------
-- ê∂éYä«óùÉVÉXÉeÉÄ ìãç⁄ì˙íˆéÊçûóöó
--
--
-- Author T.Nishida
-- Create 2020/08/21(Fri)
-- -------------------------------------------------------------------

DROP TABLE T_ImportHistory;


CREATE TABLE T_ImportHistory (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,ID						INT				NOT NULL
	,Import_User			NVARCHAR(20)	
	,Import_Date			DATETIME		NOT NULL
	,ProjectID				INT				NOT NULL
	,OrderNo				NVARCHAR(10)	NOT NULL
	,LinkFlag				BIT				NOT NULL
	,StatusFlag				INT							DEFAULT 0
	,PRIMARY KEY (ID)
)
;


