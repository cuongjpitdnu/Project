-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� ������ - ���ރf�[�^�e�[�u��
--
--
-- Author T.Ito
-- Create 2021/01/15(Fri)
-- -------------------------------------------------------------------

DROP TABLE S_Buzai
/

CREATE TABLE S_Buzai(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    ProjectID     INT           NOT NULL, /* ��L�[ */
    OrderNo       NVARCHAR(10)  NOT NULL, /* ��L�[ */
    JobID         INT           NOT NULL, /* ��L�[ */
    BuzaiName     NVARCHAR(14)  NOT NULL, /* ��L�[ */
    BData         DECIMAL(8, 2) NOT NULL DEFAULT 0,
    Weight        DECIMAL(8, 1) NOT NULL DEFAULT 0,
    List_Page     NVARCHAR(6),
    Keijyo        NVARCHAR(2),
    Length        INT,
    DoneFlag      NVARCHAR(1)            DEFAULT '0',
    DoneDate      DATE,
    AcHr          DECIMAL(8, 2) NOT NULL DEFAULT 0,
    PlStdHr       DECIMAL(8, 2) NOT NULL DEFAULT 0,
    Note          NVARCHAR(40),
    PRIMARY KEY (ProjectID, OrderNo, JobID, BuzaiName))
/
