-- MySQL dump 10.13  Distrib 8.4.8, for Linux (x86_64)
--
-- Host: localhost    Database: mrcdb
-- ------------------------------------------------------
-- Server version       8.4.8

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__EFMigrationsHistory`
--

DROP TABLE IF EXISTS `__EFMigrationsHistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__EFMigrationsHistory`
--

LOCK TABLES `__EFMigrationsHistory` WRITE;
/*!40000 ALTER TABLE `__EFMigrationsHistory` DISABLE KEYS */;
INSERT INTO `__EFMigrationsHistory` VALUES
	('20260116191205_InitialCreate','8.0.22');
/*!40000 ALTER TABLE `__EFMigrationsHistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `accounting_movement`
--

DROP TABLE IF EXISTS `accounting_movement`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `accounting_movement` (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Description` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Amount` decimal(5,2) NOT NULL,
  `Date` date NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accounting_movement`
--

LOCK TABLES `accounting_movement` WRITE;
/*!40000 ALTER TABLE `accounting_movement` DISABLE KEYS */;
/*!40000 ALTER TABLE `accounting_movement` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `activity`
--

DROP TABLE IF EXISTS `activity`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `activity` (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Date` date NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `activity`
--

LOCK TABLES `activity` WRITE;
/*!40000 ALTER TABLE `activity` DISABLE KEYS */;
/*!40000 ALTER TABLE `activity` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `activity_stage`
--

DROP TABLE IF EXISTS `activity_stage`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `activity_stage` (
  `ActivityId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `StageId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `UserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `IsUserMain` tinyint(1) NOT NULL,
  `Notes` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`ActivityId`,`StageId`,`UserId`),
  KEY `IX_activity_stage_StageId` (`StageId`),
  KEY `IX_activity_stage_UserId` (`UserId`),
  CONSTRAINT `FK_ActivityStage_ActivityId_Activity_ID` FOREIGN KEY (`ActivityId`) REFERENCES `activity` (`ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_ActivityStage_StageId_Stage_ID` FOREIGN KEY (`StageId`) REFERENCES `stage` (`ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_ActivityStage_UserId_User_ID` FOREIGN KEY (`UserId`) REFERENCES `user` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `activity_stage`
--

LOCK TABLES `activity_stage` WRITE;
/*!40000 ALTER TABLE `activity_stage` DISABLE KEYS */;
/*!40000 ALTER TABLE `activity_stage` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `attendance`
--

DROP TABLE IF EXISTS `attendance`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `attendance` (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `UserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `PersonId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `IsAttendance` tinyint(1) NOT NULL,
  `Date` date NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `IX_attendance_PersonId` (`PersonId`),
  KEY `IX_attendance_UserId` (`UserId`),
  CONSTRAINT `FK_Attendance_PersonId_Person_ID` FOREIGN KEY (`PersonId`) REFERENCES `person` (`ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_Attendance_UserId_User_ID` FOREIGN KEY (`UserId`) REFERENCES `user` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `attendance`
--

LOCK TABLES `attendance` WRITE;
/*!40000 ALTER TABLE `attendance` DISABLE KEYS */;
/*!40000 ALTER TABLE `attendance` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `charge`
--

DROP TABLE IF EXISTS `charge`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `charge` (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Amount` decimal(5,2) NOT NULL,
  `Name` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `charge`
--

LOCK TABLES `charge` WRITE;
/*!40000 ALTER TABLE `charge` DISABLE KEYS */;
/*!40000 ALTER TABLE `charge` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `degree`
--

DROP TABLE IF EXISTS `degree`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `degree` (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Name` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `degree`
--

LOCK TABLES `degree` WRITE;
/*!40000 ALTER TABLE `degree` DISABLE KEYS */;
/*!40000 ALTER TABLE `degree` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `document`
--

DROP TABLE IF EXISTS `document`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `document` (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Name` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `document`
--

LOCK TABLES `document` WRITE;
/*!40000 ALTER TABLE `document` DISABLE KEYS */;
/*!40000 ALTER TABLE `document` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `parent`
--

DROP TABLE IF EXISTS `parent`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `parent` (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Name` varchar(80) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `NormalizedName` varchar(80) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `IsMasculine` tinyint(1) NOT NULL,
  `Phone` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `parent`
--

LOCK TABLES `parent` WRITE;
/*!40000 ALTER TABLE `parent` DISABLE KEYS */;
/*!40000 ALTER TABLE `parent` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `parent_person`
--

DROP TABLE IF EXISTS `parent_person`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `parent_person` (
  `ParentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `PersonId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `IsParent` tinyint(1) NOT NULL,
  PRIMARY KEY (`ParentId`,`PersonId`,`IsParent`),
  KEY `IX_parent_person_PersonId` (`PersonId`),
  CONSTRAINT `FK_ParentPerson_ParentId_Parent_ID` FOREIGN KEY (`ParentId`) REFERENCES `parent` (`ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_ParentPerson_PersonId_Person_ID` FOREIGN KEY (`PersonId`) REFERENCES `person` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `parent_person`
--

LOCK TABLES `parent_person` WRITE;
/*!40000 ALTER TABLE `parent_person` DISABLE KEYS */;
/*!40000 ALTER TABLE `parent_person` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `permission`
--

DROP TABLE IF EXISTS `permission`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `permission` (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Name` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `permission`
--

LOCK TABLES `permission` WRITE;
/*!40000 ALTER TABLE `permission` DISABLE KEYS */;
INSERT INTO `permission` VALUES
	('02b70407-c1d7-412f-bf8b-abb20b057708','Document.Write'),
	('10541623-89d2-4c99-9f13-4fab7c7dd1b4','Role.Read'),
	('1ca78d7a-3520-421b-8284-cce2c3ac9c2e','Permission.Read'),
	('6888801b-a53c-4d90-a63d-03b9e5a4b0ce','Permission.Write'),
	('74bca746-e066-44ba-bb5d-e85b177efce7','Sacrament.Delete'),
	('827feedf-dfca-4b8a-bc72-4725eb623425','Degree.Write'),
	('93c55742-19ef-477d-9437-cc88812a0178','Degree.Delete'),
	('941e8e93-7f83-4e4e-a12f-22ef57f9b233','Sacrament.Read'),
	('97730e91-395f-4a1f-922d-98e0e388110b','User.Read'),
	('a3f21364-381d-4ec4-8bdc-45b00e66eadf','User.Write'),
	('b794bcfe-43cc-4f46-8219-b63867719703','Document.Read'),
	('cdc9d742-8260-4317-a4c2-ac29d27f63de','Document.Delete'),
	('ce9f8627-cced-4216-bbab-eccb984dd7fd','Degree.Read'),
	('d13796c7-c7d3-47c4-a826-914c09de17be','Role.Write'),
	('fd719e16-8b15-47ab-916b-52189d431f26','Sacrament.Write');
/*!40000 ALTER TABLE `permission` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `person`
--

DROP TABLE IF EXISTS `person`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `person` (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Name` varchar(65) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `NormalizedName` varchar(65) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `IsMasculine` tinyint(1) NOT NULL,
  `IsSunday` tinyint(1) NOT NULL,
  `DOB` date NOT NULL,
  `RegistrationDate` date NOT NULL,
  `Parish` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Address` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Phone` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `LastDegreeId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `IX_person_LastDegreeId` (`LastDegreeId`),
  CONSTRAINT `FK_Person_DegreeId` FOREIGN KEY (`LastDegreeId`) REFERENCES `degree` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `person`
--

LOCK TABLES `person` WRITE;
/*!40000 ALTER TABLE `person` DISABLE KEYS */;
/*!40000 ALTER TABLE `person` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `person_charge`
--

DROP TABLE IF EXISTS `person_charge`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `person_charge` (
  `PersonId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ChargeId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  PRIMARY KEY (`PersonId`,`ChargeId`),
  KEY `IX_person_charge_ChargeId` (`ChargeId`),
  CONSTRAINT `FK_PersonCharge_ChargeId` FOREIGN KEY (`ChargeId`) REFERENCES `charge` (`ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_PersonCharge_PersonId` FOREIGN KEY (`PersonId`) REFERENCES `person` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `person_charge`
--

LOCK TABLES `person_charge` WRITE;
/*!40000 ALTER TABLE `person_charge` DISABLE KEYS */;
/*!40000 ALTER TABLE `person_charge` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `person_document`
--

DROP TABLE IF EXISTS `person_document`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `person_document` (
  `PersonId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DocumentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  PRIMARY KEY (`PersonId`,`DocumentId`),
  KEY `IX_person_document_DocumentId` (`DocumentId`),
  CONSTRAINT `FK_PersonDocument_DocumentId` FOREIGN KEY (`DocumentId`) REFERENCES `document` (`ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_PersonDocument_PersonId` FOREIGN KEY (`PersonId`) REFERENCES `person` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `person_document`
--

LOCK TABLES `person_document` WRITE;
/*!40000 ALTER TABLE `person_document` DISABLE KEYS */;
/*!40000 ALTER TABLE `person_document` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `person_sacrament`
--

DROP TABLE IF EXISTS `person_sacrament`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `person_sacrament` (
  `PersonId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `SacramentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  PRIMARY KEY (`PersonId`,`SacramentId`),
  KEY `IX_person_sacrament_SacramentId` (`SacramentId`),
  CONSTRAINT `FK_PersonSacrament_PersonId` FOREIGN KEY (`PersonId`) REFERENCES `person` (`ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_PersonSacrament_SacramentId` FOREIGN KEY (`SacramentId`) REFERENCES `sacrament` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `person_sacrament`
--

LOCK TABLES `person_sacrament` WRITE;
/*!40000 ALTER TABLE `person_sacrament` DISABLE KEYS */;
/*!40000 ALTER TABLE `person_sacrament` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `role`
--

DROP TABLE IF EXISTS `role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `role` (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Name` varchar(3) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `role`
--

LOCK TABLES `role` WRITE;
/*!40000 ALTER TABLE `role` DISABLE KEYS */;
INSERT INTO `role` VALUES
	('32aa8d94-3d20-49de-94eb-676fee70230f','adm'),
	('63509335-0f5a-44b2-8542-a1b5dee695f6','sys'),
	('ec9e1959-13cf-4858-aa1f-95f0432ba3a8','usr');
/*!40000 ALTER TABLE `role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `role_permission`
--

DROP TABLE IF EXISTS `role_permission`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `role_permission` (
  `RoleID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `PermissionID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  PRIMARY KEY (`RoleID`,`PermissionID`),
  KEY `IX_role_permission_PermissionID` (`PermissionID`),
  CONSTRAINT `FK_RolePermission_PermissionId` FOREIGN KEY (`PermissionID`) REFERENCES `permission` (`ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_RolePermission_RoleId` FOREIGN KEY (`RoleID`) REFERENCES `role` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `role_permission`
--

LOCK TABLES `role_permission` WRITE;
/*!40000 ALTER TABLE `role_permission` DISABLE KEYS */;
INSERT INTO `role_permission` VALUES
	('32aa8d94-3d20-49de-94eb-676fee70230f','02b70407-c1d7-412f-bf8b-abb20b057708'),
	('63509335-0f5a-44b2-8542-a1b5dee695f6','10541623-89d2-4c99-9f13-4fab7c7dd1b4'),
	('63509335-0f5a-44b2-8542-a1b5dee695f6','1ca78d7a-3520-421b-8284-cce2c3ac9c2e'),
	('63509335-0f5a-44b2-8542-a1b5dee695f6','6888801b-a53c-4d90-a63d-03b9e5a4b0ce'),
	('32aa8d94-3d20-49de-94eb-676fee70230f','74bca746-e066-44ba-bb5d-e85b177efce7'),
	('32aa8d94-3d20-49de-94eb-676fee70230f','827feedf-dfca-4b8a-bc72-4725eb623425'),
	('32aa8d94-3d20-49de-94eb-676fee70230f','93c55742-19ef-477d-9437-cc88812a0178'),
	('ec9e1959-13cf-4858-aa1f-95f0432ba3a8','941e8e93-7f83-4e4e-a12f-22ef57f9b233'),
	('ec9e1959-13cf-4858-aa1f-95f0432ba3a8','97730e91-395f-4a1f-922d-98e0e388110b'),
	('32aa8d94-3d20-49de-94eb-676fee70230f','a3f21364-381d-4ec4-8bdc-45b00e66eadf'),
	('ec9e1959-13cf-4858-aa1f-95f0432ba3a8','b794bcfe-43cc-4f46-8219-b63867719703'),
	('32aa8d94-3d20-49de-94eb-676fee70230f','cdc9d742-8260-4317-a4c2-ac29d27f63de'),
	('ec9e1959-13cf-4858-aa1f-95f0432ba3a8','ce9f8627-cced-4216-bbab-eccb984dd7fd'),
	('63509335-0f5a-44b2-8542-a1b5dee695f6','d13796c7-c7d3-47c4-a826-914c09de17be'),
	('32aa8d94-3d20-49de-94eb-676fee70230f','fd719e16-8b15-47ab-916b-52189d431f26');
/*!40000 ALTER TABLE `role_permission` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sacrament`
--

DROP TABLE IF EXISTS `sacrament`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sacrament` (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Name` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sacrament`
--

LOCK TABLES `sacrament` WRITE;
/*!40000 ALTER TABLE `sacrament` DISABLE KEYS */;
/*!40000 ALTER TABLE `sacrament` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `stage`
--

DROP TABLE IF EXISTS `stage`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `stage` (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `stage`
--

LOCK TABLES `stage` WRITE;
/*!40000 ALTER TABLE `stage` DISABLE KEYS */;
/*!40000 ALTER TABLE `stage` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user` (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Username` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `Password` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `LastPasswordUpdate` date NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES
	('e3d00323-99f0-40f0-8ed7-7f93a4ca37e2','Misha',1,'v1:lvsiB3s9+4tZquwJSNWKhhoULULxaDffv54fWpG3h3rEMvY=','2026-01-16');
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_role`
--

DROP TABLE IF EXISTS `user_role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_role` (
  `RoleID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `UserID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  PRIMARY KEY (`UserID`,`RoleID`),
  KEY `IX_user_role_RoleID` (`RoleID`),
  CONSTRAINT `FK_UserRole_RoleId` FOREIGN KEY (`RoleID`) REFERENCES `role` (`ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_UserRole_UserId` FOREIGN KEY (`UserID`) REFERENCES `user` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_role`
--

LOCK TABLES `user_role` WRITE;
/*!40000 ALTER TABLE `user_role` DISABLE KEYS */;
INSERT INTO `user_role` VALUES
	('32aa8d94-3d20-49de-94eb-676fee70230f','e3d00323-99f0-40f0-8ed7-7f93a4ca37e2'),
	('63509335-0f5a-44b2-8542-a1b5dee695f6','e3d00323-99f0-40f0-8ed7-7f93a4ca37e2'),
	('ec9e1959-13cf-4858-aa1f-95f0432ba3a8','e3d00323-99f0-40f0-8ed7-7f93a4ca37e2');
/*!40000 ALTER TABLE `user_role` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-02-05 17:16:38
