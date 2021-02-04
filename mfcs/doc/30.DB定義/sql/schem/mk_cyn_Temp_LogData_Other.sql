-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� �ۑ��O�ꎞ���敪�������捞���O�e�[�u��
--
--
-- Author T.Ito
-- Create 2020/10/20(Tue)
---Update 2020/10/27(Tue)
--        2020/10/29(Thu)
-- -------------------------------------------------------------------

DROP TABLE Cyn_Temp_LogData_Other
/

CREATE TABLE Cyn_Temp_LogData_Other(
    Updated_at         DATETIME      NOT NULL DEFAULT GETDATE(),
    ProjectID          INT           NOT NULL, /* ��L�[ */
    OrderNo            NVARCHAR(10)  NOT NULL, /* ��L�[ */
    CKind              INT           NOT NULL,
    ID                 INT           NOT NULL, /* ��L�[ */
    P_ProjectID        INT           NOT NULL,
    P_OrderNo          NVARCHAR(10)  NOT NULL,
    P_CKind            INT           NOT NULL,
    P_No               INT,
    Name               NVARCHAR(50)  NOT NULL,
    BKumiku            NVARCHAR(1)   NOT NULL,
    Log                NVARCHAR(500),
    KoteiNo            INT,
    Kotei              NVARCHAR(2),
    KKumiku            NVARCHAR(1),
    SDate              DATETIME,
    EDate              DATETIME,
    AMDFlag            INT,
    D_No               INT,
    D_Block_WorkItemID INT,
    D_KoteiNo          INT,
    D_Plan_WorkItemID  INT,
    PRIMARY KEY (ProjectID, OrderNo, ID))
/
