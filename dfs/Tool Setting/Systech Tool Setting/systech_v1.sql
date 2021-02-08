/*
 Target Server Type    : MySQL
 Target Server Version : 50714
 File Encoding         : 65001

 Create Date		   : 02/10/2018 16:19:46
 Up Date			   : 25/10/2018 17:53:47
*/

DROP DATABASE IF EXISTS `systech`;
CREATE DATABASE `systech` CHARACTER SET utf8 COLLATE utf8_general_ci;
USE `systech`;

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for m_device
-- ----------------------------
DROP TABLE IF EXISTS `m_device`;
CREATE TABLE `m_device`  (
  `device_id` int(11) NOT NULL AUTO_INCREMENT,
  `device_name` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `ip_address` char(14) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `port` int(5) NOT NULL,
  `alarm_value` int(6) NOT NULL,
  `period` int(6) NOT NULL,
  `fail_level` int(6) NOT NULL,
  `samples` int(6) NOT NULL,
  `active` tinyint(4) NULL DEFAULT NULL,
  `device_type` int(1) NULL DEFAULT NULL,
  `userid` int(11) NULL DEFAULT NULL,
  `update_time` datetime(0) NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`device_id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 23 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

INSERT INTO `m_device` VALUES (1, 'Device 1', NULL, 10001, 500, 90, 700, 5, 1, 1, NULL, '2018-09-14 08:21:18');
INSERT INTO `m_device` VALUES (2, 'Device 2', NULL, 10001, 500, 90, 700, 5, 1, 2, NULL, '2018-09-14 08:21:20');
INSERT INTO `m_device` VALUES (3, 'Device 1', '192.168.0.1', 10001, 500, 90, 700, 5, 1, 1, 99, '2018-09-14 08:21:20');
INSERT INTO `m_device` VALUES (4, 'Device 2', '192.168.0.1', 10001, 500, 90, 700, 5, 1, 2, 99, '2018-09-14 08:21:20');

-- ----------------------------
-- Table structure for m_user
-- ----------------------------
DROP TABLE IF EXISTS `m_user`;
CREATE TABLE `m_user`  (
  `userid` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `password` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `fullname` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `email` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `role` int(1) NOT NULL COMMENT '0 - admin / 1 - user',
  `update_time` datetime(0) NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`userid`) USING BTREE,
  UNIQUE INDEX `username`(`username`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 13 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

INSERT INTO `m_user` VALUES (1, 'admin', 'admin', 'admin', NULL, 0, '2018-09-14 08:23:10');

-- ----------------------------
-- Table structure for tbl_measure
-- ----------------------------
DROP TABLE IF EXISTS `tbl_measure`;
CREATE TABLE `tbl_measure`  (
  `measure_id` int(11) NOT NULL AUTO_INCREMENT,
  `device_id` int(11) NOT NULL,
  `measure_type` int(1) NOT NULL,
  `alarm_value` int(6) NOT NULL,
  `period` int(6) NOT NULL,
  `fail_level` int(6) NOT NULL,
  `samples` int(6) NOT NULL,
  `start_time` datetime(0) NULL DEFAULT NULL,
  `end_time` datetime(0) NULL DEFAULT NULL,
  `result` int(1) NULL DEFAULT NULL,
  `userid` int(11) NULL DEFAULT NULL,
  `update_time` datetime(0) NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`measure_id`, `device_id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 26 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for tbl_measure_detail
-- ----------------------------
DROP TABLE IF EXISTS `tbl_measure_detail`;
CREATE TABLE `tbl_measure_detail`  (
  `detail_id` int(11) NOT NULL AUTO_INCREMENT,
  `measure_id` int(11) NOT NULL,
  `samples_time` datetime(3) NOT NULL,
  `device_id` int(11) NULL DEFAULT NULL,
  `actual_value` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `actual_max_value` int(11) NULL DEFAULT NULL,
  `actual_min_value` int(11) NULL DEFAULT NULL,
  `actual_delegate` int(11) NULL DEFAULT NULL,
  `result` int(1) NULL DEFAULT NULL,
  PRIMARY KEY (`detail_id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 6119 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for tbl_measure_detail_limit
-- ----------------------------
DROP TABLE IF EXISTS `tbl_measure_detail_limit`;
CREATE TABLE `tbl_measure_detail_limit`  (
  `limit_id` int(11) NOT NULL AUTO_INCREMENT,
  `measure_id` int(11) NOT NULL,
  `samples_time` datetime(3) NOT NULL,
  `device_id` int(11) NULL DEFAULT NULL,
  `actual_value` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `actual_max_value` int(11) NULL DEFAULT NULL,
  `actual_min_value` int(11) NULL DEFAULT NULL,
  `actual_delegate` int(11) NULL DEFAULT NULL,
  `result` int(1) NULL DEFAULT NULL,
  PRIMARY KEY (`limit_id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 116 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for tbl_measure_detail_raw
-- ----------------------------
DROP TABLE IF EXISTS `tbl_measure_detail_raw`;
CREATE TABLE `tbl_measure_detail_raw`  (
  `raw_id` int(11) NOT NULL AUTO_INCREMENT,
  `measure_id` int(11) NOT NULL,
  `samples_time` datetime(3) NOT NULL,
  `device_id` int(11) NULL DEFAULT NULL,
  `actual_value` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `actual_max_value` int(11) NULL DEFAULT NULL,
  `actual_min_value` int(11) NULL DEFAULT NULL,
  `actual_delegate` int(11) NULL DEFAULT NULL,
  `result` int(1) NULL DEFAULT NULL,
  PRIMARY KEY (`raw_id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 42157 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;
