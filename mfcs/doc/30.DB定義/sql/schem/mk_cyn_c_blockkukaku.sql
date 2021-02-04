-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� �u���b�N/�����(��������e�Ƃ������������x���f�[�^)�f�[�^�e�[�u��
--
--
-- Author T.Ito
-- Create 2020/09/09(Wed)
-- Update 2020/12/03(Thu) �d�ʊ֘A��8,1�Ɍ����ύX by Toshinori Ito
-- -------------------------------------------------------------------

DROP TABLE Cyn_C_BlockKukaku
/

CREATE TABLE Cyn_C_BlockKukaku(
    Updated_at    DATETIME     NOT NULL DEFAULT GETDATE(),
    ProjectID     INT          NOT NULL, /* ��L�[ */
    OrderNo       NVARCHAR(10) NOT NULL, /* ��L�[ */
    WorkItemID    INT,
    P_ProjectID   INT          NOT NULL,
    P_OrderNo     NVARCHAR(10) NOT NULL,
    P_CKind       INT          NOT NULL,
    P_No          INT          NOT NULL,
    T_Name        NVARCHAR(50) NOT NULL,
    T_BKumiku     NVARCHAR(1)  NOT NULL,
    CKind         INT          NOT NULL,
    No            INT          NOT NULL, /* ��L�[ */
    Name          NVARCHAR(50) NOT NULL, 
    BKumiku       NVARCHAR(1)  NOT NULL, 
    N_No          INT          NOT NULL DEFAULT 0,
    N_Name        NVARCHAR(50),
    N_BKumiku     NVARCHAR(1),
    Struct        NVARCHAR(2)           DEFAULT '5',
    Category      NVARCHAR(5),
    Width         DECIMAL(4, 1),
    Length        DECIMAL(4, 1),
    Height        DECIMAL(4, 1),
    Weight        DECIMAL(8, 1),
    Zu_No         NVARCHAR(6),
    KG_Weight     DECIMAL(8, 1)         DEFAULT 0,
    True_Weight   BIT          NOT NULL DEFAULT 0,
    Is_Magari     BIT          NOT NULL DEFAULT 0,
    Del_Date      DATE,
    PRIMARY KEY (ProjectID, OrderNo, No))
/
