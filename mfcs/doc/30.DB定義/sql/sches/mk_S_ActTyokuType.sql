-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� ������ - ���ړ��̓e�[�u��
--
--
-- Author T.Ito
-- Create 2021/01/15(Fri)
-- -------------------------------------------------------------------

DROP TABLE S_ActTyokuType
/

CREATE TABLE S_ActTyokuType(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    WkDate        DATE          NOT NULL, /* ��L�[ */
    GroupID       INT           NOT NULL, /* ��L�[ */
    TyokuType     INT           NOT NULL DEFAULT 0,
    PRIMARY KEY (WkDate, GroupID))
/
