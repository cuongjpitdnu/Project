-- -------------------------------------------------------------------
-- ���Y�Ǘ��V�X�e�� �\�͎��ԃ}�X�^�p���j�[�N�L�[
--
--
-- Author T.Nishida
-- Create 2020/09/01(Tue)
-- -------------------------------------------------------------------

ALTER TABLE mstAbility DROP CONSTRAINT uk_mstAbility_001
;


ALTER TABLE mstAbility ADD CONSTRAINT uk_mstAbility_001
 UNIQUE (GroupID, FloorCode, DistCode, Sdate)
 ;






