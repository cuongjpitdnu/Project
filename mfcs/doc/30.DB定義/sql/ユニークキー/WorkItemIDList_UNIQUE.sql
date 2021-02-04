-- -------------------------------------------------------------------
-- 生産管理システム プロジェクトマスタ用ユニークキー
--
--
-- Author T.Nishida
-- Create 2020/09/01(Tue)
-- -------------------------------------------------------------------

ALTER TABLE WorkItemIDList DROP CONSTRAINT uk_WorkItemIDList_001
;


ALTER TABLE WorkItemIDList ADD CONSTRAINT uk_WorkItemIDList_001
 UNIQUE (WorkItemID)
;



