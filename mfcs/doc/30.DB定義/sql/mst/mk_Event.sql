-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� ��v�s���e�[�u��
--
--
-- Author T.Ito
-- Create 2020/12/18(Fri)
-- -------------------------------------------------------------------

DROP TABLE Event
/

CREATE TABLE Event(
    Updated_at       DATETIME      NOT NULL DEFAULT GETDATE(),
    EventDate        DATE          NOT NULL, /* ��L�[ */
    ProjectID        INT           NOT NULL, /* ��L�[ */
    OrderNo          NVARCHAR(10)  NOT NULL, /* ��L�[ */
    Name             NVARCHAR(40)  NOT NULL, /* ��L�[ */
    Nick             NVARCHAR(40),
    GroupID          INT           NOT NULL, /* ��L�[ */
    EndDate          DATE,
    ActEventDate     DATE,
    ActEndDate       DATE,
    ID               INT,
    BigMilestoneFlag INT,
    PRIMARY KEY (EventDate, ProjectID, OrderNo, Name, GroupID))
/
