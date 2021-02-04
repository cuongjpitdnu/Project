-- -------------------------------------------------------------------
-- 生産管理システム プロジェクトマスタ用ユニークキー
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



