-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� �v���W�F�N�g�}�X�^�p���j�[�N�L�[
--
--
-- Author T.Nishida
-- Create 2020/09/01(Tue)
-- -------------------------------------------------------------------

ALTER TABLE mstProject DROP CONSTRAINT uk_mstProject_001
;


ALTER TABLE mstProject ADD CONSTRAINT uk_mstProject_001
 UNIQUE (SysKindID, ListKind, SerialNo)
;



