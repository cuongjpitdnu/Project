-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� ���������e�[�u��
--
--
-- Author T.Ito
-- Create 2020/12/18(Fri)
-- -------------------------------------------------------------------

DROP TABLE TestDay
/

CREATE TABLE TestDay(
    Updated_at       DATETIME      NOT NULL DEFAULT GETDATE(),
    TestDate         DATE          NOT NULL, /* ��L�[ */
    ProjectID        INT           NOT NULL, /* ��L�[ */
    OrderNo          NVARCHAR(10)  NOT NULL, /* ��L�[ */
    JobID            INT           NOT NULL, /* ��L�[ */
    TestID           INT           NOT NULL, /* ��L�[ */
    Pass             Bit                    DEFAULT 0,
    PRIMARY KEY (TestDate, ProjectID, OrderNo, JobID, TestID))
/
