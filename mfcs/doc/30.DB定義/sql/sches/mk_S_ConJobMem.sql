-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� ������ - ��Ǝw���e�[�u��
--
--
-- Author T.Ito
-- Create 2021/01/15(Fri)
-- -------------------------------------------------------------------

DROP TABLE S_ConJobMem
/

CREATE TABLE S_ConJobMem(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    Work_Date     DATE          NOT NULL, /* ��L�[ */
    GroupID       INT           NOT NULL DEFAULT 0, /* ��L�[ */
    Com_ID        INT           NOT NULL DEFAULT 0, /* ��L�[ */
    Mem_ID        INT           NOT NULL DEFAULT 0, /* ��L�[ */
    OrderNo       NVARCHAR(10),
    JobID         INT,
    KanID         INT,
    Item          NCHAR(5)               DEFAULT '00000',
    Bikou         NVARCHAR(40),
    IsKasei       BIT           NOT NULL DEFAULT 0,
    UkeGroupID    INT,
    PRIMARY KEY (Work_Date, GroupID, Com_ID, Mem_ID))
/
