-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� ������ - �W���H���}�X�^�ڍ׃f�[�^�e�[�u��
--
--
-- Author T.Ito
-- Create 2020/11/13(Fri)
-- Update 2020/12/04(Fri) �O���Q�ƃL�[�ǉ�,KeshiPattern,KeshiCode�ǉ� by Toshinori Ito
-- -------------------------------------------------------------------

DROP TABLE S_mstDataDetail
/

CREATE TABLE S_mstDataDetail(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    ID            INT           NOT NULL DEFAULT 0, /* ��L�[ */
    PatternID     INT           NOT NULL DEFAULT 0, /* ��L�[ */
    Name          NVARCHAR(40)  NOT NULL,
    DistCode      NCHAR(5),
    AcMac         NCHAR(8),
    Item          NCHAR(5)               DEFAULT '00000',
    BD_Code       NVARCHAR(5),
    SortNo        INT           NOT NULL DEFAULT 0,
    Kumiku        NCHAR(1),
    KeshiPattern  INT           NOT NULL DEFAULT 0,
    KeshiCode     INT           NOT NULL DEFAULT 1,
    PRIMARY KEY (ID, PatternID),
    CONSTRAINT FK_S_mstDataDetail FOREIGN KEY(PatternID) 
    REFERENCES S_mstDataPattern(ID))
/
