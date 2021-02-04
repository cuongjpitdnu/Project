-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� �H����`�}�X�^�e�[�u��
--
--
-- Author T.Ito
-- Create 2020/08/18(Tue)
-- -------------------------------------------------------------------

DROP TABLE Cyn_mstKotei_STR_P
/

CREATE TABLE Cyn_mstKotei_STR_P(
    Updated_at    DATETIME     NOT NULL DEFAULT GETDATE(),
    CKind         INT          NOT NULL, /* ��L�[ */
    Code          NVARCHAR(2)  NOT NULL, /* ��L�[ */
    Name          NVARCHAR(20) NOT NULL,
    DelFlag       BIT          NOT NULL DEFAULT 0,
    PRIMARY KEY (CKind, Code))
/
