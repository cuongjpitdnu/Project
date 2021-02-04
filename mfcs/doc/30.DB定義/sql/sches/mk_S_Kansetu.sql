-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� ������ - �Ԑڃf�[�^�e�[�u��
--
--
-- Author T.Ito
-- Create 2020/12/18(Fri)
-- -------------------------------------------------------------------

DROP TABLE S_Kansetu
/

CREATE TABLE S_Kansetu(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    WkDate        DATE          NOT NULL, /* ��L�[ */
    GroupID       INT           NOT NULL, /* ��L�[ */
    Mem_ID        INT           NOT NULL DEFAULT 0, /* ��L�[ */
    Com_ID        INT           NOT NULL DEFAULT 0, /* ��L�[ */
    KanID         INT           NOT NULL, /* ��L�[ */
    OrderNo       NVARCHAR(10)  NOT NULL, /* ��L�[ */
    Item          NCHAR(5)      NOT NULL DEFAULT '00000', /* ��L�[ */
    Bikou         NVARCHAR(40),
    AcHr          DECIMAL(8,2)  NOT NULL DEFAULT 0,
    BData         DECIMAL(8,2)  NOT NULL DEFAULT 0,
    PRIMARY KEY (WkDate, GroupID, Mem_ID, Com_ID, KanID, OrderNo, Item))
/
