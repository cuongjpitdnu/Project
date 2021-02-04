-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� �H���v�f�}�X�^�e�[�u��
--
--
-- Author T.Ito
-- Create 2020/08/18(Tue)
-- -------------------------------------------------------------------

DROP TABLE Cyn_mstKotei
/

CREATE TABLE Cyn_mstKotei(
    Updated_at    DATETIME     NOT NULL DEFAULT GETDATE(),
    CKind         INT          NOT NULL, /* ��L�[ */
    Code          NVARCHAR(2)  NOT NULL, /* ��L�[ */
    Name          NVARCHAR(10) NOT NULL,
    Nick          NVARCHAR(10) NOT NULL,
    DelFlag       BIT          NOT NULL DEFAULT 0,
    OutFlag       INT          NOT NULL DEFAULT 0,
    PRIMARY KEY (CKind, Code))
/
