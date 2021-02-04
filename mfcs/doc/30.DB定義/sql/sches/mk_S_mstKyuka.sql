-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� ������ - �x�Ƀ}�X�^�e�[�u��
--
--
-- Author T.Ito
-- Create 2020/12/18(Fri)
-- -------------------------------------------------------------------

DROP TABLE S_mstKyuka
/

CREATE TABLE S_mstKyuka(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    ID            INT           NOT NULL, /* ��L�[ */
    Name          NVARCHAR(40)  NOT NULL,
    Nick          NCHAR(6)      NOT NULL,
    Hr            DECIMAL(8, 2) NOT NULL DEFAULT 0,
    SortNo        INT           NOT NULL DEFAULT 0,
    PRIMARY KEY (ID))
/
