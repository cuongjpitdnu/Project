-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� ���ʃe�[�u��
--
--
-- Author T.Ito
-- Create 2020/10/20(Tue)
-- -------------------------------------------------------------------

DROP TABLE Cyn_Block_BD
/

CREATE TABLE Cyn_Block_BD(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    ProjectID     INT           NOT NULL, /* ��L�[ */
    OrderNo       NVARCHAR(10)  NOT NULL, /* ��L�[ */
    ID            INT           NOT NULL, /* ��L�[ */
    Name          NVARCHAR(50)  NOT NULL, /* ��L�[ */
    BKumiku       NVARCHAR(1)   NOT NULL, /* ��L�[ */
    Kotei         NVARCHAR(2)   NOT NULL, /* ��L�[ */
    KKumiku       NVARCHAR(1)   NOT NULL, /* ��L�[ */
    BD_Code       NVARCHAR(5)   NOT NULL,
    BData         DECIMAL(8, 2) NOT NULL DEFAULT 0,
    PRIMARY KEY (ProjectID, OrderNo, ID, Name, BKumiku, Kotei, KKumiku))
/
