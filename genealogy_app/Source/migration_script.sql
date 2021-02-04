-- ----------------------------------------------------------------------------
-- MySQL Workbench Migration
-- Migrated Schemata: giapha
-- Source Schemata: giapha
-- Created: Tue Sep 18 14:24:10 2018
-- Workbench Version: 8.0.12
-- ----------------------------------------------------------------------------

SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------------------------------------------------------
-- Schema giapha
-- ----------------------------------------------------------------------------
DROP SCHEMA IF EXISTS `giapha` ;
CREATE SCHEMA IF NOT EXISTS `giapha` ;

-- ----------------------------------------------------------------------------
-- Table giapha.M_FAMILY_HEAD
-- ----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS `giapha`.`M_FAMILY_HEAD` (
  `FHEAD_ID` INT NOT NULL,
  `MEMBER_ID` INT NULL,
  `PROFILE_DOC` VARCHAR(200) CHARACTER SET 'utf8mb4' NULL);

-- ----------------------------------------------------------------------------
-- Table giapha.M_FAMILY_IMAGE
-- ----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS `giapha`.`M_FAMILY_IMAGE` (
  `IMAGE_ID` INT NOT NULL,
  `IMAGE_TITLE` VARCHAR(50) CHARACTER SET 'utf8mb4' NULL,
  `IMAGE_DES` VARCHAR(200) CHARACTER SET 'utf8mb4' NULL,
  `IMAGE_NAME` VARCHAR(200) CHARACTER SET 'utf8mb4' NULL);

-- ----------------------------------------------------------------------------
-- Table giapha.M_FAMILY_INFO
-- ----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS `giapha`.`M_FAMILY_INFO` (
  `FAMILY_NAME` VARCHAR(100) CHARACTER SET 'utf8mb4' NULL,
  `FAMILY_HOMETOWN` VARCHAR(200) CHARACTER SET 'utf8mb4' NULL,
  `FAMILY_ANNIVERSARY` VARCHAR(200) CHARACTER SET 'utf8mb4' NULL);

-- ----------------------------------------------------------------------------
-- Table giapha.M_FAMILY_RELATION
-- ----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS `giapha`.`M_FAMILY_RELATION` (
  `RELID` INT NOT NULL,
  `RELNAME` VARCHAR(50) CHARACTER SET 'utf8mb4' NULL,
  `RELDES` VARCHAR(200) CHARACTER SET 'utf8mb4' NULL);

-- ----------------------------------------------------------------------------
-- Table giapha.M_NATIONALITY
-- ----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS `giapha`.`M_NATIONALITY` (
  `NAT_ID` INT NOT NULL,
  `NAT_NAME` VARCHAR(20) CHARACTER SET 'utf8mb4' NOT NULL);

-- ----------------------------------------------------------------------------
-- Table giapha.M_RELIGION
-- ----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS `giapha`.`M_RELIGION` (
  `REL_ID` INT NOT NULL,
  `REL_NAME` VARCHAR(20) CHARACTER SET 'utf8mb4' NOT NULL);

-- ----------------------------------------------------------------------------
-- Table giapha.M_ROOT
-- ----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS `giapha`.`M_ROOT` (
  `ROOT_ID` INT NOT NULL,
  `MEMBER_ID` INT NOT NULL);

-- ----------------------------------------------------------------------------
-- Table giapha.M_USER
-- ----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS `giapha`.`M_USER` (
  `USERID` INT NOT NULL,
  `USERNAME` VARCHAR(10) CHARACTER SET 'utf8mb4' NOT NULL,
  `PASS_WORD` VARCHAR(50) CHARACTER SET 'utf8mb4' NOT NULL,
  `LASTUPDATE` DATETIME(6) NOT NULL);

-- ----------------------------------------------------------------------------
-- Table giapha.T_FMEMBER_CAREER
-- ----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS `giapha`.`T_FMEMBER_CAREER` (
  `MEMBER_ID` INT NOT NULL,
  `CAREER_ID` INT NOT NULL,
  `CAREER_TYPE` INT NOT NULL,
  `START_DATE` DATETIME(6) NULL,
  `END_DATE` DATETIME(6) NULL,
  `OCCUPATION` VARCHAR(50) CHARACTER SET 'utf8mb4' NULL,
  `POSITION` VARCHAR(50) CHARACTER SET 'utf8mb4' NULL,
  `OFFICE_NAME` VARCHAR(50) CHARACTER SET 'utf8mb4' NOT NULL,
  `OFFICE_PLACE` VARCHAR(200) CHARACTER SET 'utf8mb4' NULL,
  `REMARK` LONGTEXT CHARACTER SET 'utf8mb4' NULL,
  `LASTUPDATE` DATETIME(6) NOT NULL,
  `START_DAY` DOUBLE NULL,
  `START_MON` DOUBLE NULL,
  `START_YEA` DOUBLE NULL,
  `END_DAY` DOUBLE NULL,
  `END_MON` DOUBLE NULL,
  `END_YEA` DOUBLE NULL);

-- ----------------------------------------------------------------------------
-- Table giapha.T_FMEMBER_CONTACT
-- ----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS `giapha`.`T_FMEMBER_CONTACT` (
  `MEMBER_ID` INT NOT NULL,
  `HOMETOWN` VARCHAR(200) CHARACTER SET 'utf8mb4' NULL,
  `HOME_ADD` VARCHAR(200) CHARACTER SET 'utf8mb4' NULL,
  `PHONENUM1` VARCHAR(50) CHARACTER SET 'utf8mb4' NULL,
  `PHONENUM2` VARCHAR(50) CHARACTER SET 'utf8mb4' NULL,
  `MAIL_ADD1` VARCHAR(100) CHARACTER SET 'utf8mb4' NULL,
  `MAIL_ADD2` VARCHAR(100) CHARACTER SET 'utf8mb4' NULL,
  `FAXNUM` VARCHAR(50) CHARACTER SET 'utf8mb4' NULL,
  `URL` VARCHAR(200) CHARACTER SET 'utf8mb4' NULL,
  `IMNICK` VARCHAR(50) CHARACTER SET 'utf8mb4' NULL,
  `REMARK` LONGTEXT CHARACTER SET 'utf8mb4' NULL,
  `LASTUPDATE` DATETIME(6) NOT NULL);

-- ----------------------------------------------------------------------------
-- Table giapha.T_FMEMBER_FACT
-- ----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS `giapha`.`T_FMEMBER_FACT` (
  `MEMBER_ID` INT NOT NULL,
  `FACT_ID` INT NOT NULL,
  `FACT_PLACE` VARCHAR(200) CHARACTER SET 'utf8mb4' NULL,
  `FACT_NAME` VARCHAR(200) CHARACTER SET 'utf8mb4' NOT NULL,
  `START_DATE` DATETIME(6) NULL,
  `END_DATE` DATETIME(6) NULL,
  `DESCRIPTION` VARCHAR(200) CHARACTER SET 'utf8mb4' NULL,
  `LASTUPDATE` DATETIME(6) NOT NULL,
  `START_DAY` DOUBLE NULL,
  `START_MON` DOUBLE NULL,
  `START_YEA` DOUBLE NULL,
  `END_DAY` DOUBLE NULL,
  `END_MON` DOUBLE NULL,
  `END_YEA` DOUBLE NULL);

-- ----------------------------------------------------------------------------
-- Table giapha.T_FMEMBER_MAIN
-- ----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS `giapha`.`T_FMEMBER_MAIN` (
  `MEMBER_ID` INT NOT NULL,
  `LAST_NAME` VARCHAR(100) CHARACTER SET 'utf8mb4' NOT NULL,
  `MIDDLE_NAME` VARCHAR(100) CHARACTER SET 'utf8mb4' NULL,
  `FIRST_NAME` VARCHAR(100) CHARACTER SET 'utf8mb4' NOT NULL,
  `ALIAS_NAME` VARCHAR(200) CHARACTER SET 'utf8mb4' NULL,
  `BIRTH_DAY` DATETIME(6) NULL,
  `GENDER` INT NULL,
  `HOMETOWN` VARCHAR(200) CHARACTER SET 'utf8mb4' NULL,
  `BIRTH_PLACE` VARCHAR(200) CHARACTER SET 'utf8mb4' NULL,
  `NATIONALITY` VARCHAR(50) CHARACTER SET 'utf8mb4' NULL,
  `RELIGION` VARCHAR(50) CHARACTER SET 'utf8mb4' NULL,
  `DECEASED` INT NULL,
  `DECEASED_DATE` DATETIME(6) NULL,
  `BURY_PLACE` VARCHAR(200) CHARACTER SET 'utf8mb4' NULL,
  `AVATAR_PATH` VARCHAR(200) CHARACTER SET 'utf8mb4' NULL,
  `FAMILY_ORDER` INT NULL,
  `REMARK` LONGTEXT CHARACTER SET 'utf8mb4' NULL,
  `LASTUPDATE` DATETIME(6) NOT NULL,
  `BIR_DAY` INT NULL,
  `BIR_MON` INT NULL,
  `BIR_YEA` INT NULL,
  `DEA_DAY` INT NULL,
  `DEA_MON` INT NULL,
  `DEA_YEA` INT NULL,
  `DEA_DAY_SUN` DOUBLE NULL,
  `DEA_MON_SUN` DOUBLE NULL,
  `DEA_YEA_SUN` DOUBLE NULL,
  `BIR_DAY_LUNAR` DOUBLE NULL,
  `BIR_MON_LUNAR` DOUBLE NULL,
  `BIR_YEA_LUNAR` DOUBLE NULL,
  `CAREER_TYPE` DOUBLE NULL,
  `EDUCATION_TYPE` DOUBLE NULL,
  `FACT_TYPE` DOUBLE NULL,
  `CAREER` LONGTEXT CHARACTER SET 'utf8mb4' NULL,
  `EDUCATION` LONGTEXT CHARACTER SET 'utf8mb4' NULL,
  `FACT` LONGTEXT CHARACTER SET 'utf8mb4' NULL,
  `LEVEL` DOUBLE NULL);

-- ----------------------------------------------------------------------------
-- Table giapha.T_FMEMBER_RELATION
-- ----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS `giapha`.`T_FMEMBER_RELATION` (
  `MEMBER_ID` INT NOT NULL,
  `REL_FMEMBER_ID` INT NOT NULL,
  `RELID` INT NULL,
  `ROLE_ORDER` DOUBLE NULL);
SET FOREIGN_KEY_CHECKS = 1;
