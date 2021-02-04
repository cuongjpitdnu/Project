-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� �H���p�^�[���}�X�^�e�[�u��
--
--
-- Author T.Ito
-- Create 2020/08/18(Tue)
-- -------------------------------------------------------------------

DROP TABLE Cyn_mstKotei_STR_C
/

CREATE TABLE Cyn_mstKotei_STR_C(
    Updated_at    DATETIME     NOT NULL DEFAULT GETDATE(),
    CKind         INT          NOT NULL, /* ��L�[ */
    Code          NVARCHAR(2)  NOT NULL, /* ��L�[ */
    No            INT          NOT NULL, /* ��L�[ */
    Kotei         NVARCHAR(2)  NOT NULL,
    KKumiku       NVARCHAR(1)  NOT NULL,
    Days          INT          NOT NULL DEFAULT 1,
    Floor         NVARCHAR(10),
    BD_Code       NVARCHAR(5),
    N_No          INT          NOT NULL DEFAULT 0,
    N_Kotei       NVARCHAR(2),
    N_KKumiku     NVARCHAR(1),
    N_Link        INT          NOT NULL DEFAULT 0,
    PRIMARY KEY (CKind, Code, No))
/
