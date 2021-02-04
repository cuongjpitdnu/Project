-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� ������ - �v��}�X�^�e�[�u��
--
--
-- Author T.Ito
-- Create 2020/11/13(Fri)
-- Update 2020/12/04(Fri) �O���Q�ƃL�[�ǉ� by Toshinori Ito
-- -------------------------------------------------------------------

DROP TABLE S_mstDataPatternDetail
/

CREATE TABLE S_mstDataPatternDetail(
    Updated_at    DATETIME      NOT NULL DEFAULT GETDATE(),
    PID           INT           NOT NULL DEFAULT 0, /* ��L�[ */
    AF            NCHAR(1)      NOT NULL DEFAULT 'A',
    StartMargin   INT           NOT NULL DEFAULT 0,
    PlDateNo      INT           NOT NULL DEFAULT 0, /* ��L�[ */
    Days          INT           NOT NULL DEFAULT 1,
    EndMargin     INT           NOT NULL DEFAULT 0,
    PRIMARY KEY (PID, PlDateNo),
    CONSTRAINT FK_S_mstDataPatternDetail FOREIGN KEY(PID) 
    REFERENCES S_mstDataDetail(ID))
/
