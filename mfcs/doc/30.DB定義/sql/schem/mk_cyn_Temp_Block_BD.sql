-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� ���ʃ��O�p�ꎞ�e�[�u��
--
--
-- Author T.Ito
-- Create 2020/11/13(Fri)
-- -------------------------------------------------------------------

DROP TABLE Cyn_Temp_Block_BD
/

CREATE TABLE Cyn_Temp_Block_BD(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    CKind         INT           NOT NULL, /* ��L�[ */
    ID            INT           NOT NULL, /* ��L�[ */
    T_Name        NVARCHAR(50),
    T_BKumiku     NVARCHAR(1),
    Name          NVARCHAR(50),
    BKumiku       NVARCHAR(1),
    Kotei         NVARCHAR(2),
    KKumiku       NVARCHAR(1),
    Log           NVARCHAR(500),
    PRIMARY KEY (CKind, ID))
/
