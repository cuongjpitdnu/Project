-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� ��v�s�����̃}�X�^�e�[�u��
--
--
-- Author T.Ito
-- Create 2020/12/18(Fri)
-- -------------------------------------------------------------------

DROP TABLE mstEvent
/

CREATE TABLE mstEvent(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    Name          NVARCHAR(40)  NOT NULL, /* ��L�[ */
    Nick          NVARCHAR(40),
    GroupID       INT           NOT NULL DEFAULT 0, /* ��L�[ */
    PRIMARY KEY (Name, GroupID))
/
