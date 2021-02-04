-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� ������ - ���X���уf�[�^�e�[�u��
--
--
-- Author T.Ito
-- Create 2020/12/07(Mon)
-- -------------------------------------------------------------------

DROP TABLE S_ActDaily
/

CREATE TABLE S_ActDaily(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    OrderNo       NVARCHAR(10)  NOT NULL, /* ��L�[ */
    JobID         INT           NOT NULL, /* ��L�[ */
    WkDate        DATE          NOT NULL, /* ��L�[ */
    AcHr          DECIMAL(8,2)  NOT NULL DEFAULT 0,
    S_BData       DECIMAL(8,2)  NOT NULL DEFAULT 0,
    S_HC          DECIMAL(8,2)  NOT NULL DEFAULT 0,
    Rate          DECIMAL(7,6)  NOT NULL DEFAULT 0,
    PRIMARY KEY (OrderNo, JobID, WkDate))
/
