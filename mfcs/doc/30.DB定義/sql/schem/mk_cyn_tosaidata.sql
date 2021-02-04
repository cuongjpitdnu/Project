-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� ���ړ������x���f�[�^�e�[�u��
--
--
-- Author T.Ito
-- Create 2020/08/21(Fri)
-- -------------------------------------------------------------------

DROP TABLE Cyn_TosaiData
/

CREATE TABLE Cyn_TosaiData(
    Updated_at    DATETIME     NOT NULL DEFAULT GETDATE(),
    ProjectID     INT          NOT NULL, /* ��L�[ */
    OrderNo       NVARCHAR(10) NOT NULL, /* ��L�[ */
    CKind         INT          NOT NULL,
    WorkItemID    INT,
    Name          NVARCHAR(50) NOT NULL, /* ��L�[ */
    BKumiku       NVARCHAR(1)  NOT NULL, /* ��L�[ */
    IsOriginal    BIT          NOT NULL DEFAULT 0,
    T_Date        DATE,
    SG_Date       DATE,
    PlSDate       DATE,
    SG_Days       INT,
    NxtName       NVARCHAR(50),
    NxtBKumiku    NVARCHAR(1),
    WorkItemID_T  INT,
    WorkItemID_K  INT,
    WorkItemID_S  INT,
    PRIMARY KEY (ProjectID, OrderNo, Name, BKumiku))
/
