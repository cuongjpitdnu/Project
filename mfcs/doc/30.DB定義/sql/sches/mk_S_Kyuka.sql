-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� ������ - �x�Ƀf�[�^�e�[�u��
--
--
-- Author T.Ito
-- Create 2020/12/18(Fri)
-- -------------------------------------------------------------------

DROP TABLE S_Kyuka
/

CREATE TABLE S_Kyuka(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    WkDate        DATE          NOT NULL, /* ��L�[ */
    GroupID       INT           NOT NULL, /* ��L�[ */
    MemID         INT           NOT NULL, /* ��L�[ */
    KyukaID       INT           NOT NULL, /* ��L�[ */
    Hr            DECIMAL(8, 2) NOT NULL DEFAULT 0,
    PRIMARY KEY (WkDate, GroupID, MemID, KyukaID))
/
