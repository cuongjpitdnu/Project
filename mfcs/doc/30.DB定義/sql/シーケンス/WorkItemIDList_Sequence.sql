-- -------------------------------------------------------------------
-- 生産管理システム ワークアイテムID管理用シーケンス
--
--
-- Author T.Nishida
-- Create 2020/10/07(Wed)
-- -------------------------------------------------------------------

DROP SEQUENCE seq_WorkItemIDList;


CREATE SEQUENCE seq_WorkItemIDList
    START WITH 1
    MINVALUE 1
    NO CYCLE
    NO CACHE
;


