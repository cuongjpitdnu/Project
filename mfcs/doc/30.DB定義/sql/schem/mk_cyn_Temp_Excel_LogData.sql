-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� �ۑ��O�ꎞExcel�捞���O�f�[�^�e�[�u��
--
--
-- Author T.Ito
-- Create 2020/09/24(Thu)
-- Update 2020/10/06(Tue)
-- -------------------------------------------------------------------

DROP TABLE Cyn_Temp_Excel_LogData
/

CREATE TABLE Cyn_Temp_Excel_LogData(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    ProjectID     INT           NOT NULL, /* ��L�[ */
    OrderNo       NVARCHAR(10)  NOT NULL, /* ��L�[ */
    CKind         INT           NOT NULL, /* ��L�[ */
    WorkItemID    INT,
    ID            INT           NOT NULL, /* ��L�[ */
    Log           NVARCHAR(500),
    T_Name        NVARCHAR(50),
    T_BKumiku     NVARCHAR(1),
    Name          NVARCHAR(50),
    BKumiku       NVARCHAR(1),
    Gen           NVARCHAR(1),
    Kotei         NVARCHAR(2),
    KKumiku       NVARCHAR(1),
    SDate         DATE,
    EDate         DATE,
    Days          INT,
    LinkDays      INT,
    Floor         NVARCHAR(10),
    BD_Code       NVARCHAR(5),
    AMDFlag       INT,
    PRIMARY KEY (ProjectID, OrderNo, CKind, ID))
/
