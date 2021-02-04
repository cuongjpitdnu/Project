-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� ������ - �l�ʎ��уf�[�^�e�[�u��
--
--
-- Author T.Ito
-- Create 2021/01/15(Fri)
-- -------------------------------------------------------------------

DROP TABLE S_ActInd
/

CREATE TABLE S_ActInd(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    OrderNo       NVARCHAR(10)  NOT NULL, /* ��L�[ */
    JobID         INT           NOT NULL, /* ��L�[ */
    WkDate        DATE          NOT NULL, /* ��L�[ */
    Mem_ID        INT           NOT NULL DEFAULT 0, /* ��L�[ */
    AcHr          DECIMAL(8,2)  NOT NULL DEFAULT 0,
    GroupID       INT                    DEFAULT 0,
    Com_ID        INT           NOT NULL DEFAULT 0, /* ��L�[ */
    PRIMARY KEY (OrderNo, JobID, WkDate, Mem_ID, Com_ID))
/
