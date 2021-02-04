-- -------------------------------------------------------------------
-- 生産管理システム オーダマスタテーブル
--
--
-- Author T.Nishida
-- Create 2020/07/08(Wed)
-- -------------------------------------------------------------------

DROP TABLE mstOrderNo;


CREATE TABLE mstOrderNo (
	Updated_at				DATETIME		NOT NULL	DEFAULT GETDATE()
	,Up_User				NVARCHAR(20)				
	,OrderNo				NVARCHAR(10)	NOT NULL	
	,BLDDIST				NCHAR(3)					
	,CLASS					NCHAR(2)					
	,TYPE					NCHAR(8)					
	,STYLE					NCHAR(8)					
	,NAME					NCHAR(8)					
	,TP_Date				DATETIME					
	,KG_Date				DATETIME					
	,OG_Date				DATETIME					
	,SG_Date				DATETIME					
	,LD_Date				DATETIME					
	,S_SDate				DATETIME					
	,PE_SDate				DATETIME					
	,ST_Date				DATETIME					
	,L_Date					DATETIME					
	,O_Date					DATETIME					
	,PI_Date				DATETIME					
	,D_Date					DATETIME					
	,OutPutDate				DATETIME					
	,ImportDate				DATETIME					
	,Sgts_Flag				NVARCHAR(1)		NOT NULL	DEFAULT 0
	,Exp_Flag				NVARCHAR(1)		NOT NULL	DEFAULT 0
	,Last_Exp_Date			DATETIME					
	,Exp_Res				NVARCHAR(1)					
	,Is_Dummy				NVARCHAR(1)		NOT NULL	DEFAULT 0
	,Is_Kantei				TINYINT						DEFAULT 0
	,DispFlag				BIT				NOT NULL	DEFAULT 0
	,ViewColor				INT							
	,DrawPattern			INT				NOT NULL	DEFAULT 0
	,WBSCode				NCHAR(3)					
	,PreOrderNo				NVARCHAR(10)				
	,Note					NVARCHAR(20)				
	,PRIMARY KEY (OrderNo)
)
;


