-- -------------------------------------------------------------------
-- 生産管理システム 搭載日程取込データ
--
--
-- Author T.Nishida
-- Create 2020/09/04(Fri)
-- -------------------------------------------------------------------

DROP TABLE T_ImportData;


CREATE TABLE T_ImportData (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,ID						INT				NOT NULL
	,ImportID				INT				NOT NULL
	,UserID					NVARCHAR(20)	NOT NULL
	,BlockNick				NVARCHAR(20)	
	,BlockName				NVARCHAR(20)	
	,BlockKumiku			NVARCHAR(1)		NOT NULL
	,K_BlockNick			NVARCHAR(20)	
	,K_BlockName			NVARCHAR(20)	
	,K_BlockKumiku			NVARCHAR(1)		
	,Kind					TINYINT			NOT NULL
	,SDate					DATETIME		
	,EDate					DATETIME		
	,SDate_P				DATETIME		
	,EDate_P				DATETIME		
	,SDate_S				DATETIME		
	,EDate_S				DATETIME		
	,SDate_C				DATETIME		
	,EDate_C				DATETIME		
	,WorkItemID_P			INT				
	,WorkItemID_S			INT				
	,WorkItemID_C			INT				
	,K_BlockName_P			NVARCHAR(20)	
	,K_BlockKumiku_P		NVARCHAR(1)		
	,K_BlockName_S			NVARCHAR(20)	
	,K_BlockKumiku_S		NVARCHAR(1)		
	,K_BlockName_C			NVARCHAR(20)	
	,K_BlockKumiku_C		NVARCHAR(1)		
	,ImportFlag				INT				NOT NULL
	,ModifyFlag				NVARCHAR(10)	
	,Message				NVARCHAR(500)	
	,Row					INT				
	,Col					INT				
	,PRIMARY KEY (ID, ImportID)
)
;


