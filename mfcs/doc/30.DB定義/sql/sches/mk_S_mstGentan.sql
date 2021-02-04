-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� ������ - ���P�}�X�^�e�[�u��
--
--
-- Author T.Ito
-- Create 2020/12/07(Mon)
-- -------------------------------------------------------------------

DROP TABLE S_mstGentan
/

CREATE TABLE S_mstGentan(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    GroupID       INT           NOT NULL DEFAULT 0, /* ��L�[ */
    OrderNo       NVARCHAR(10)  NOT NULL DEFAULT '_', /* ��L�[ */
    Struct        NCHAR(4)      NOT NULL DEFAULT '*', /* ��L�[ */
    Category      NCHAR(6)      NOT NULL DEFAULT '*', /* ��L�[ */
    Kumiku        NCHAR(1)      NOT NULL DEFAULT '*', /* ��L�[ */
    AcFloor       NCHAR(10)     NOT NULL DEFAULT '*', /* ��L�[ */
    AcMac         NCHAR(8)      NOT NULL DEFAULT '*', /* ��L�[ */
    DistCode      NCHAR(5)      NOT NULL DEFAULT '*', /* ��L�[ */
    Gentan        DECIMAL(4, 3) NOT NULL DEFAULT 0,
    PRIMARY KEY (GroupID, OrderNo, Struct, Category, Kumiku, AcFloor, AcMac, DistCode))
/
