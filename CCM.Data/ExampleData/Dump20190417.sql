-- MySQL dump 10.13  Distrib 8.0.13, for Win64 (x86_64)
--
-- Host: localhost    Database: uccm
-- ------------------------------------------------------
-- Server version	5.7.23-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
 SET NAMES utf8 ;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `callhistories`
--

DROP TABLE IF EXISTS `callhistories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `callhistories` (
  `Id` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `CallId` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `SipCallId` longtext COLLATE utf8mb4_swedish_ci,
  `Started` datetime NOT NULL,
  `Ended` datetime NOT NULL,
  `DlgHashId` longtext COLLATE utf8mb4_swedish_ci,
  `DlgHashEnt` longtext COLLATE utf8mb4_swedish_ci,
  `ToTag` longtext COLLATE utf8mb4_swedish_ci,
  `FromTag` longtext COLLATE utf8mb4_swedish_ci,
  `FromId` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `FromSip` longtext COLLATE utf8mb4_swedish_ci,
  `FromUsername` longtext COLLATE utf8mb4_swedish_ci,
  `FromDisplayName` longtext COLLATE utf8mb4_swedish_ci,
  `FromComment` longtext COLLATE utf8mb4_swedish_ci,
  `FromLocationId` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `FromLocationName` longtext COLLATE utf8mb4_swedish_ci,
  `FromLocationComment` longtext COLLATE utf8mb4_swedish_ci,
  `FromLocationShortName` longtext COLLATE utf8mb4_swedish_ci,
  `FromCodecTypeId` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `FromCodecTypeName` longtext COLLATE utf8mb4_swedish_ci,
  `FromCodecTypeColor` longtext COLLATE utf8mb4_swedish_ci,
  `FromOwnerId` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `FromOwnerName` longtext COLLATE utf8mb4_swedish_ci,
  `FromRegionId` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `FromRegionName` longtext COLLATE utf8mb4_swedish_ci,
  `FromUserAgentHead` longtext COLLATE utf8mb4_swedish_ci,
  `ToId` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `ToSip` longtext COLLATE utf8mb4_swedish_ci,
  `ToUsername` longtext COLLATE utf8mb4_swedish_ci,
  `ToDisplayName` longtext COLLATE utf8mb4_swedish_ci,
  `ToComment` longtext COLLATE utf8mb4_swedish_ci,
  `ToLocationId` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `ToLocationName` longtext COLLATE utf8mb4_swedish_ci,
  `ToLocationComment` longtext COLLATE utf8mb4_swedish_ci,
  `ToLocationShortName` longtext COLLATE utf8mb4_swedish_ci,
  `ToCodecTypeId` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `ToCodecTypeName` longtext COLLATE utf8mb4_swedish_ci,
  `ToCodecTypeColor` longtext COLLATE utf8mb4_swedish_ci,
  `ToOwnerId` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `ToOwnerName` longtext COLLATE utf8mb4_swedish_ci,
  `ToRegionId` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `ToRegionName` longtext COLLATE utf8mb4_swedish_ci,
  `ToUserAgentHead` longtext COLLATE utf8mb4_swedish_ci,
  `IsPhoneCall` tinyint(4) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `CIX_CallHistories_Ended` (`Ended`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `callhistories`
--

LOCK TABLES `callhistories` WRITE;
/*!40000 ALTER TABLE `callhistories` DISABLE KEYS */;
/*!40000 ALTER TABLE `callhistories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `calls`
--

DROP TABLE IF EXISTS `calls`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `calls` (
  `Id` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `SipCallID` longtext COLLATE utf8mb4_swedish_ci,
  `DlgHashId` longtext COLLATE utf8mb4_swedish_ci,
  `DlgHashEnt` longtext COLLATE utf8mb4_swedish_ci,
  `FromId` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `FromUsername` longtext COLLATE utf8mb4_swedish_ci,
  `FromDisplayName` varchar(200) COLLATE utf8mb4_swedish_ci DEFAULT NULL,
  `ToId` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `ToUsername` longtext COLLATE utf8mb4_swedish_ci,
  `ToDisplayName` varchar(200) COLLATE utf8mb4_swedish_ci DEFAULT NULL,
  `Started` datetime NOT NULL,
  `Updated` datetime NOT NULL,
  `Closed` tinyint(4) NOT NULL,
  `State` int(11) DEFAULT NULL,
  `ToTag` longtext COLLATE utf8mb4_swedish_ci,
  `FromTag` longtext COLLATE utf8mb4_swedish_ci,
  `IsPhoneCall` tinyint(4) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Closed` (`Closed`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `calls`
--

LOCK TABLES `calls` WRITE;
/*!40000 ALTER TABLE `calls` DISABLE KEYS */;
INSERT INTO `calls` VALUES ('01563703-f728-427c-ad19-3e12afd006c4','98MToIWW3kETbtg3sHDibPM1mPjLSuWT','1060','2446','0324949f-42ed-499e-af3c-8dc1b9697ef1','studio-pool-001@sipdomain.com','Studio pool 1','136D9F77-CA80-49D3-BC27-02B77EE3B644','john.doe@sipdomain.com','John Doe','2019-02-11 17:06:59','2019-02-11 17:06:59',0,0,'<null>','.jEnQGVuZTiH1spqxqwodXt3SwutsC84',0),('11d55837-0942-4032-ad9e-4ac56355b63d','HoAUpjWKlZqFOOq23Ap2T5fk.lqPGUQt','4376','1631','21ABB1AD-38B6-4211-9098-7F6906752F5E','ob-portable-001@sipdomain.com','OB Portable 1','27CD88DC-5FBC-439F-B470-BFEEE91852F3','studio-pool-002@sipdomain.com','Studio pool 2','2019-03-11 17:06:59','2019-03-11 17:06:59',0,0,'i51TTgEN5Hk9jThG-bnQmpj.5ld5ONg3','1L6vjZliAiaBP7m.vfh4BElhjL7jJZlW',0),('194c74fe-99ae-416d-a072-d294ba094588','-I.lzsaUUEVtZqTPAxLxFtBUTRiPlRDL','7318','2393','4DDBDBEB-89E7-4232-BEA4-748B4D5E7B37','dev-001@sipdomain.com','DEV 001','4e6f8ed4-1d22-46f4-a81a-d6dbc8f581ac','pis.rartsiger@sipdomain.com','Pis Rartsiger','2019-03-11 16:06:59','2019-03-11 16:06:59',0,0,'<null>','ZDNREt36Jw8rprr0obdLW4szC5M2b7pO',0),('7cc8324f-9076-4e88-b873-c9d944b1193c','Rp0h9faDDxap1oWiQuM2Zdp9R88DNZj-','1240','3796','6821859E-53AC-4A3A-A14D-C63495F56655','facility-001@sipdomain.com','Facility 1','00000000-0000-0000-0000-000000000000','020304050','020304050','2019-03-15 12:06:59','2019-03-15 12:06:59',0,0,'as168fd8f7','XJX.vbm5ugNEFD13tj.IgqQN.3L5gVJH',1),('bce5624d-d8cf-486b-b85a-08e08254b0a4','Rp0h9faDDxap1oWiQuM2Zdp9R88DNZj-','1240','3796','7722B5B4-2089-4A0F-9BE9-57B6C89F5312','block-line@sipdomain.com','Block Connection','00000000-0000-0000-0000-000000000000','0700128752','0700128752','2019-03-15 12:06:59','2019-03-15 12:06:59',0,0,'<null>','XJX.vbm5ugNEFD13tj.IgqQN.3L5gVJH',1),('c5d01b65-11df-4ec2-84c1-19e354ef4342','scXSCBr.GhjASSXHi7zWfFMpKkhBMLwI','3327','2341','9ec856fa-cc47-4031-8882-9a897fb2770c','local-department@sipdomain.com','Local Department','00000000-0000-0000-0000-000000000000','08562712312','08562712312','2019-02-15 12:06:59','2019-02-15 12:06:59',0,0,'<null>','RxpfiYjMVhKpTuPh-N8qgdz0E4FWsh6c',1),('f49990ad-10a2-4bae-b85b-fced14f33123','Wb6lOHdCvSPXVbaiurX9gRueKSCbqZpo','2727','1581','6d19425f-6a23-404f-9854-3f9dc55bde3b','backpack-001@sipdomain.com','Backpack 1','974A8AA1-DA5F-4148-BAB0-FBC2BD70B868','tim.johnsson@@sipdomain.com','Tim Johnsson','2019-03-10 10:06:59','2019-03-10 10:06:59',0,0,'fx4wJKrbA55vQsz2Qs4hLdmGO1cLjq2U','kfelgXH.pZfrckItICHNwlxJPQqEmL2B',0),('fa04bfc1-5c16-46e4-8988-150f0d22049d','q58NoXa20zcXzxHBEEHV0Vy','699','3157','A4F083D7-327F-4490-A19D-474845E06946','lisa.fredriksen@sipdomain.com','Lisa Fredriksen','AE3EE31E-9B5C-49F2-9624-F90EB41CDD8B','mike.golden@sipdomain.com','Mike Golden','2019-03-12 10:06:59','2019-03-12 10:06:59',0,0,'OM7WgnlKBhfSHWFynXp3RPltIsLTAw7w','4mX3Hec79H22j',0),('ff1d5bf6-e1dc-404c-b90a-bdd26b941e21','3wAjka3YFcY0qsHF0s3oHE6hKJ3vDICd','2249','971','BDEB2094-A9B1-49CB-8722-5557C89AFC1C','miranda.stewart@sipdomain.com','Miranda Stewart','D029EAD8-2D71-4545-90A0-7CEE5F85E46A','yang.weng@sipdomain.com','Yang Weng','2019-02-12 10:06:59','2019-02-12 10:06:59',0,0,'<null>','V9iCZFAOUOdDNX-zRJ.DN0WV5yQ0LBvm',0);
/*!40000 ALTER TABLE `calls` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ccmusers`
--

DROP TABLE IF EXISTS `ccmusers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `ccmusers` (
  `Id` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `UserName` longtext COLLATE utf8mb4_swedish_ci,
  `FirstName` longtext COLLATE utf8mb4_swedish_ci,
  `LastName` longtext COLLATE utf8mb4_swedish_ci,
  `Comment` longtext COLLATE utf8mb4_swedish_ci,
  `PasswordHash` longtext COLLATE utf8mb4_swedish_ci,
  `Salt` longtext COLLATE utf8mb4_swedish_ci,
  `CreatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `CreatedOn` datetime NOT NULL,
  `UpdatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `UpdatedOn` datetime NOT NULL,
  `Role_Id` char(36) COLLATE utf8mb4_swedish_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_CcmUsers_Role_RoleId` (`Role_Id`),
  CONSTRAINT `FK_CcmUsers_Roles_RoleId` FOREIGN KEY (`Role_Id`) REFERENCES `roles` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ccmusers`
--

LOCK TABLES `ccmusers` WRITE;
/*!40000 ALTER TABLE `ccmusers` DISABLE KEYS */;
INSERT INTO `ccmusers` VALUES ('318c0f75-81dc-4455-8808-de53fca5fb34','discovery','Discovery','Test','Access account','kq+3HYdWGXLlrKVtb3Jprg==','CyIGvYj4S+cKfBV3PakHKP9zYaQ=','root','2019-04-15 15:51:44','root','2019-02-03 21:00:54',NULL),('C0E480FC-BD0C-4779-8324-85DD709B7FBE','root','root','Administrator',NULL,'kq+3HYdWGXLlrKVtb3Jprg==','NSVoZlkkdSFnclFCZEhzZyMmXiQ=','root','2019-04-15 15:51:44','root','2018-04-25 07:10:09','93F8370B-3321-42CB-A7BF-856AA8E6446E');
/*!40000 ALTER TABLE `ccmusers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cities`
--

DROP TABLE IF EXISTS `cities`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `cities` (
  `Id` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `Name` longtext COLLATE utf8mb4_swedish_ci,
  `CreatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `CreatedOn` datetime NOT NULL,
  `UpdatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `UpdatedOn` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cities`
--

LOCK TABLES `cities` WRITE;
/*!40000 ALTER TABLE `cities` DISABLE KEYS */;
INSERT INTO `cities` VALUES ('03C9DD9D-1A55-4CB4-861A-C837EBDAC859','Berlin','root','2015-04-29 21:57:28','root','2015-12-17 12:58:12'),('15C2F344-551D-4621-B5F0-C6F74F99D35D','Paris','root','2017-06-14 13:15:26','root','2017-06-14 13:15:26'),('91CB9B00-E4B7-4C5B-9603-51792B2623B0','Oslo','root','2015-04-28 12:17:24','root','2015-11-12 14:28:58'),('9588E6AE-01C6-49ED-9DF5-8AAF57D8DDFD','Stockholm','root','2014-11-19 12:54:41','root','2015-05-05 10:45:42'),('C999C4A4-F848-497D-8024-9AFD0CF87352','Helsinki','root','2014-10-22 11:14:12','root','2014-10-22 11:14:12'),('D1DF8E13-86BE-4950-A169-B36A3F16D7E9','New York','root','2015-04-01 19:53:06','root','2015-04-01 19:53:06'),('F614CA9A-B8DD-4CFE-8855-1988C8DDBB38','Los Angeles','root','2015-07-08 14:25:26','root','2016-02-08 09:08:14');
/*!40000 ALTER TABLE `cities` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `codecpresets`
--

DROP TABLE IF EXISTS `codecpresets`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `codecpresets` (
  `Id` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `Name` longtext COLLATE utf8mb4_swedish_ci,
  `CreatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `CreatedOn` datetime NOT NULL,
  `UpdatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `UpdatedOn` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `codecpresets`
--

LOCK TABLES `codecpresets` WRITE;
/*!40000 ALTER TABLE `codecpresets` DISABLE KEYS */;
INSERT INTO `codecpresets` VALUES ('076F058C-11BF-416C-84C0-90E43C6F9B6B','Preset 1','root','2015-04-29 08:59:11','root','2015-04-29 08:59:11'),('2789E7A3-CBF6-4434-A1C5-7EF72D718B0A','Preset 7','root','2016-09-22 17:33:43','root','2016-09-22 17:33:43'),('6A6D3983-665C-4E87-BD98-2FBF24C953A0','Preset 6','root','2016-09-22 17:33:38','root','2016-09-22 17:33:38'),('82B6D5C3-79A9-4CD7-B3C7-EE96D9CA1922','Preset 4','root','2016-09-22 17:33:30','root','2016-09-22 17:33:30'),('9B098C2E-06AC-4546-9CBE-377BB3A30555','Preset 5','root','2016-09-22 17:33:35','root','2016-09-22 17:33:35'),('C3B8AD85-0E26-41A9-8125-4448A77431B6','Preset 2','root','2015-04-29 08:59:15','root','2015-04-29 08:59:15'),('D8C7DC2F-79AB-434C-BE03-F2F3830D0717','Preset 3','root','2016-09-22 17:33:18','root','2016-09-22 17:33:18'),('F225F656-3DFA-4A7A-A876-9B35C60F177B','Preset 8','root','2016-09-22 17:33:55','root','2016-09-22 17:33:55');
/*!40000 ALTER TABLE `codecpresets` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `codecpresetuseragents`
--

DROP TABLE IF EXISTS `codecpresetuseragents`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `codecpresetuseragents` (
  `CodecPreset_Id` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `UserAgent_Id` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  PRIMARY KEY (`CodecPreset_Id`,`UserAgent_Id`),
  KEY `IX_CodecPreset_CodecPresetId` (`CodecPreset_Id`),
  KEY `IX_UserAgent_UserAgentId` (`UserAgent_Id`),
  CONSTRAINT `FK_CodecPresetUserAgents_CodecPresetId` FOREIGN KEY (`CodecPreset_Id`) REFERENCES `codecpresets` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_CodecPresetUserAgents_UserAgentId` FOREIGN KEY (`UserAgent_Id`) REFERENCES `useragents` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `codecpresetuseragents`
--

LOCK TABLES `codecpresetuseragents` WRITE;
/*!40000 ALTER TABLE `codecpresetuseragents` DISABLE KEYS */;
INSERT INTO `codecpresetuseragents` VALUES ('076F058C-11BF-416C-84C0-90E43C6F9B6B','6FE0FD55-CAFB-4D57-8818-8A289511C4B5'),('076F058C-11BF-416C-84C0-90E43C6F9B6B','834EA3A6-1F25-4F4B-961F-97ECFE351BB8'),('2789E7A3-CBF6-4434-A1C5-7EF72D718B0A','6FE0FD55-CAFB-4D57-8818-8A289511C4B5'),('2789E7A3-CBF6-4434-A1C5-7EF72D718B0A','834EA3A6-1F25-4F4B-961F-97ECFE351BB8'),('6A6D3983-665C-4E87-BD98-2FBF24C953A0','6FE0FD55-CAFB-4D57-8818-8A289511C4B5'),('6A6D3983-665C-4E87-BD98-2FBF24C953A0','834EA3A6-1F25-4F4B-961F-97ECFE351BB8'),('82B6D5C3-79A9-4CD7-B3C7-EE96D9CA1922','6FE0FD55-CAFB-4D57-8818-8A289511C4B5'),('82B6D5C3-79A9-4CD7-B3C7-EE96D9CA1922','834EA3A6-1F25-4F4B-961F-97ECFE351BB8'),('9B098C2E-06AC-4546-9CBE-377BB3A30555','6FE0FD55-CAFB-4D57-8818-8A289511C4B5'),('9B098C2E-06AC-4546-9CBE-377BB3A30555','834EA3A6-1F25-4F4B-961F-97ECFE351BB8'),('C3B8AD85-0E26-41A9-8125-4448A77431B6','6FE0FD55-CAFB-4D57-8818-8A289511C4B5'),('C3B8AD85-0E26-41A9-8125-4448A77431B6','834EA3A6-1F25-4F4B-961F-97ECFE351BB8'),('D8C7DC2F-79AB-434C-BE03-F2F3830D0717','6FE0FD55-CAFB-4D57-8818-8A289511C4B5'),('D8C7DC2F-79AB-434C-BE03-F2F3830D0717','834EA3A6-1F25-4F4B-961F-97ECFE351BB8'),('F225F656-3DFA-4A7A-A876-9B35C60F177B','6FE0FD55-CAFB-4D57-8818-8A289511C4B5'),('F225F656-3DFA-4A7A-A876-9B35C60F177B','834EA3A6-1F25-4F4B-961F-97ECFE351BB8');
/*!40000 ALTER TABLE `codecpresetuseragents` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `codectypes`
--

DROP TABLE IF EXISTS `codectypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `codectypes` (
  `Id` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `Name` longtext COLLATE utf8mb4_swedish_ci,
  `Color` longtext COLLATE utf8mb4_swedish_ci,
  `CreatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `CreatedOn` datetime NOT NULL,
  `UpdatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `UpdatedOn` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `codectypes`
--

LOCK TABLES `codectypes` WRITE;
/*!40000 ALTER TABLE `codectypes` DISABLE KEYS */;
INSERT INTO `codectypes` VALUES ('1488C1A6-B6C2-498E-9E85-8F80CFE4DD0C','Backpack','#854f4f','root','2015-07-09 07:20:37','root','2017-01-31 14:36:09'),('176B09ED-38EE-49AD-9FCD-922AC93CA71D','External','#ffc700','root','2014-09-16 12:12:10','root','2014-11-19 09:11:53'),('2FDB03E3-5E28-4640-A609-1B2098C8819C','Virtual','#a73cd9','root','2017-07-28 09:04:04','root','2017-07-28 09:04:04'),('531C49AF-4ED1-4E32-851F-482748B1259F','Studio','#00abff','root','2014-09-16 12:11:38','root','2017-01-31 14:34:02'),('63A48BAC-1C58-4E84-9BC3-6DD8494B45D5','Development','#e74c3c','root','2014-09-16 12:12:51','root','2014-11-19 09:12:38'),('675EBEC4-2F0A-46CE-880C-FD0B50BC7CA3','Pool-codecs','#d1a229','root','2017-07-28 09:08:33','root','2017-07-28 09:08:33'),('794B9AE5-FEF6-4232-BF85-DBB9DAD23F96','National service','#000000','root','2017-03-09 12:30:42','root','2017-03-09 12:40:45'),('7AE664E7-551D-4ED5-A394-BD9BBB3CC33F','Trucks','#e53ce7','root','2014-10-22 06:47:35','root','2014-11-19 09:12:03'),('96b76f87-2216-4561-9b74-609554b4deaf','Correspondents','#565656','root','2017-11-09 09:12:47','root','2017-11-09 09:13:06'),('9EA80A94-C092-4FBD-8858-9B5DA446F0ED','Phones','#9685b3','root','2017-08-10 11:54:07','root','2017-08-10 11:54:07'),('9FBE2AA7-0080-45A4-9B50-07A12C12ED8A','Personal','#ffff00','root','2014-09-16 12:12:33','root','2014-09-16 12:12:33'),('a4f254a9-1c6f-4c30-b0dc-6f4179693206','Intercom','#b27810','root','2018-01-22 12:11:24','root','2018-01-22 12:11:37'),('F50604D9-3442-452A-B600-28D8D1E2F056','Portable','#00ff75','root','2014-09-16 12:10:07','root','2017-01-31 14:35:55');
/*!40000 ALTER TABLE `codectypes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `filters`
--

DROP TABLE IF EXISTS `filters`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `filters` (
  `Id` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `Name` longtext COLLATE utf8mb4_swedish_ci,
  `PropertyName` longtext COLLATE utf8mb4_swedish_ci,
  `Type` longtext COLLATE utf8mb4_swedish_ci,
  `FilteringName` longtext COLLATE utf8mb4_swedish_ci,
  `CreatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `CreatedOn` datetime NOT NULL,
  `UpdatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `UpdatedOn` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `filters`
--

LOCK TABLES `filters` WRITE;
/*!40000 ALTER TABLE `filters` DISABLE KEYS */;
INSERT INTO `filters` VALUES ('26B43F3C-47B4-40C1-AFEE-0853DADDF557','Codec type','Name','CodecTypes','CodecTypeName','root','2015-11-24 14:42:38','root','2019-02-06 01:55:28'),('ED60C2B3-50B8-419F-83A9-5E91EB366F79','Region','Name','Regions','RegionName','root','2014-09-16 12:25:24','root','2019-12-16 12:24:33');
/*!40000 ALTER TABLE `filters` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `locations`
--

DROP TABLE IF EXISTS `locations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `locations` (
  `Id` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `Name` longtext COLLATE utf8mb4_swedish_ci,
  `ShortName` longtext COLLATE utf8mb4_swedish_ci,
  `Comment` longtext COLLATE utf8mb4_swedish_ci,
  `Net_Address_v4` varchar(50) COLLATE utf8mb4_swedish_ci DEFAULT NULL,
  `Cidr` tinyint(4) DEFAULT NULL,
  `Net_Address_v6` varchar(200) COLLATE utf8mb4_swedish_ci DEFAULT NULL,
  `Cidr_v6` tinyint(4) DEFAULT NULL,
  `CarrierConnectionId` longtext COLLATE utf8mb4_swedish_ci,
  `CreatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `CreatedOn` datetime NOT NULL,
  `UpdatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `UpdatedOn` datetime NOT NULL,
  `City_Id` char(36) COLLATE utf8mb4_swedish_ci DEFAULT NULL,
  `Region_Id` char(36) COLLATE utf8mb4_swedish_ci DEFAULT NULL,
  `ProfileGroup_Id` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_City_CityId` (`City_Id`),
  KEY `IX_Region_RegionId` (`Region_Id`),
  KEY `fk_locations_profilegroups1_idx` (`ProfileGroup_Id`),
  CONSTRAINT `FK_City_CityId` FOREIGN KEY (`City_Id`) REFERENCES `cities` (`Id`),
  CONSTRAINT `FK_Region_RegionId` FOREIGN KEY (`Region_Id`) REFERENCES `regions` (`Id`),
  CONSTRAINT `fk_locations_profilegroups1` FOREIGN KEY (`ProfileGroup_Id`) REFERENCES `profilegroups` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `locations`
--

LOCK TABLES `locations` WRITE;
/*!40000 ALTER TABLE `locations` DISABLE KEYS */;
INSERT INTO `locations` VALUES ('0A10C58F-EF64-455D-B907-3C1273FAF6C1','Office Department 1','Office1',NULL,'121.19.127.192',28,NULL,NULL,NULL,'root','2019-01-06 09:49:53','root','2019-02-06 01:49:53','9588E6AE-01C6-49ED-9DF5-8AAF57D8DDFD','015d0eea-83d0-457e-b931-3f40fb38d0c8','632a109c-6365-4e92-bbeb-bf876a321905'),('1FCA4F89-C062-42CB-B4ED-2D4451164312','Arena Fotboll','ArenaF',NULL,'134.25.24.96',28,NULL,NULL,NULL,'root','2019-01-06 09:49:53','root','2019-02-06 01:41:47','F614CA9A-B8DD-4CFE-8855-1988C8DDBB38','7cd0bca7-fe2e-4a75-a8da-74a13e770d5a','72c94c45-d876-4209-a361-20612289fd32'),('5F22E14A-793D-4B5F-91B5-B57C3B99F599','Company A','CmpyA',NULL,'211.15.0.0',16,NULL,NULL,NULL,'root','2019-01-06 09:49:53','root','2019-02-06 01:51:01','9588e6ae-01c6-49ed-9df5-8aaf57d8ddfd','a1132218-3e57-4471-93a3-0122fbcf8793','60d7b004-762f-4580-a027-852d660d8aaf'),('67E2541A-7870-4CF9-A4F1-6E97B951168A','Deep forest','forest',NULL,'77.220.0.0',16,NULL,NULL,NULL,'root','2019-01-06 09:49:53','root','2019-02-06 01:51:38',NULL,'11173c20-a2ee-4fc2-89a5-017e7a2b2b77','E530AD41-72E5-479C-8E47-9BAF9176A2B4'),('79672557-6CBA-471C-82DB-E16AD5EA1D59','Office Department 2','Office2',NULL,'11.230.110.0',24,NULL,NULL,NULL,'root','2019-01-06 09:49:53','root','2019-02-06 01:50:32','F614CA9A-B8DD-4CFE-8855-1988C8DDBB38','c930ab2b-545f-4760-8891-1c8d56933084','632a109c-6365-4e92-bbeb-bf876a321905'),('88E3D0DE-9CCE-4BD4-B7AF-7EC20E7EF429','Unspecified','?',NULL,'0.0.0.0',0,NULL,NULL,NULL,'root','2019-01-06 09:49:53','root','2019-02-06 01:35:34',NULL,'5f91805d-8841-4d28-b02e-9c5fa3eab450','E530AD41-72E5-479C-8E47-9BAF9176A2B4');
/*!40000 ALTER TABLE `locations` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `logs`
--

DROP TABLE IF EXISTS `logs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `logs` (
  `Id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Date` datetime(6) DEFAULT NULL,
  `Level` varchar(50) COLLATE utf8mb4_swedish_ci DEFAULT NULL,
  `LevelValue` int(11) DEFAULT NULL,
  `Message` longtext COLLATE utf8mb4_swedish_ci,
  `Callsite` varchar(512) COLLATE utf8mb4_swedish_ci DEFAULT NULL,
  `Exception` varchar(512) COLLATE utf8mb4_swedish_ci DEFAULT NULL,
  `Application` varchar(64) COLLATE utf8mb4_swedish_ci DEFAULT NULL,
  `ActivityId` char(36) COLLATE utf8mb4_swedish_ci DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `logs`
--

LOCK TABLES `logs` WRITE;
/*!40000 ALTER TABLE `logs` DISABLE KEYS */;
/*!40000 ALTER TABLE `logs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `metatypes`
--

DROP TABLE IF EXISTS `metatypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `metatypes` (
  `Id` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `Name` longtext COLLATE utf8mb4_swedish_ci,
  `PropertyName` longtext COLLATE utf8mb4_swedish_ci,
  `Type` longtext COLLATE utf8mb4_swedish_ci,
  `FullPropertyName` longtext COLLATE utf8mb4_swedish_ci,
  `CreatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `CreatedOn` datetime NOT NULL,
  `UpdatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `UpdatedOn` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `metatypes`
--

LOCK TABLES `metatypes` WRITE;
/*!40000 ALTER TABLE `metatypes` DISABLE KEYS */;
INSERT INTO `metatypes` VALUES ('0A524DB7-041E-405D-AB07-55F97E67B9E6','ShortLocation','ShortName','CCM.Data.Entities.Location','Location.ShortName','root','2019-03-04 10:17:47','root','2019-03-04 10:17:47'),('49D336F1-222F-42B9-8893-66A64B711730','Owner','Name','CCM.Data.Entities.Owner','User.Owner.Name','root','2019-03-04 10:17:47','root','2019-03-04 10:17:47'),('6965B48F-33A0-435C-A3F0-DE033822AAF7','IPAddress','IP','CCM.Data.Entities.RegisteredSip','IP','root','2019-03-04 10:17:47','root','2019-03-04 10:17:47'),('69CA75B3-EFBC-4C66-94D2-8B8E0BA79FFE','Location','Name','CCM.Data.Entities.Location','Location.Name','root','2019-03-04 10:17:47','root','2019-03-04 10:17:47'),('77E9D7A9-0161-4A64-954D-DDD4B131F09D','UserAgentHeader','UserAgentHead','CCM.Data.Entities.RegisteredSip','UserAgentHead','root','2019-03-04 10:17:47','root','2019-03-04 10:17:47');
/*!40000 ALTER TABLE `metatypes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `owners`
--

DROP TABLE IF EXISTS `owners`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `owners` (
  `Id` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `Name` longtext COLLATE utf8mb4_swedish_ci,
  `CreatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `CreatedOn` datetime NOT NULL,
  `UpdatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `UpdatedOn` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `owners`
--

LOCK TABLES `owners` WRITE;
/*!40000 ALTER TABLE `owners` DISABLE KEYS */;
INSERT INTO `owners` VALUES ('58790BE0-5607-44A7-9D9F-D763A67891BB','Company','root','2019-03-04 10:17:47','root','2019-03-04 10:17:47'),('F26D4889-11AF-4242-BE87-59034E6A575F','Development','root','2019-03-04 10:17:47','root','2019-03-04 10:17:47');
/*!40000 ALTER TABLE `owners` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `profilegroupprofileorders`
--

DROP TABLE IF EXISTS `profilegroupprofileorders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `profilegroupprofileorders` (
  `Profile_Id` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `ProfileGroup_Id` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `SortIndex` int(11) NOT NULL,
  PRIMARY KEY (`Profile_Id`,`ProfileGroup_Id`),
  KEY `fk_ProfileGroupProfileOrders_profilegroups1_idx` (`ProfileGroup_Id`),
  KEY `fk_ProfileGroupProfileOrders_profile_idx` (`Profile_Id`),
  CONSTRAINT `fk_ProfileGroupProfileOrders_profilegroups1` FOREIGN KEY (`ProfileGroup_Id`) REFERENCES `profilegroups` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `fk_ProfileGroupProfileOrders_profiles1` FOREIGN KEY (`Profile_Id`) REFERENCES `profiles` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `profilegroupprofileorders`
--

LOCK TABLES `profilegroupprofileorders` WRITE;
/*!40000 ALTER TABLE `profilegroupprofileorders` DISABLE KEYS */;
INSERT INTO `profilegroupprofileorders` VALUES ('1D765BF5-8FD6-49AE-8D76-5E00564B7552','60d7b004-762f-4580-a027-852d660d8aaf',6),('1D765BF5-8FD6-49AE-8D76-5E00564B7552','632a109c-6365-4e92-bbeb-bf876a321905',6),('1D765BF5-8FD6-49AE-8D76-5E00564B7552','714b88fc-2fc2-467e-a013-4f0d0aa47757',1),('1D765BF5-8FD6-49AE-8D76-5E00564B7552','72c94c45-d876-4209-a361-20612289fd32',6),('1D765BF5-8FD6-49AE-8D76-5E00564B7552','7d842c78-6d8c-4cc5-912f-9ed51c828090',5),('1D765BF5-8FD6-49AE-8D76-5E00564B7552','822928b9-8c47-46d2-8ca7-3e7e7b352933',5),('348A6B99-1201-46CE-98E8-E84553EF0493','60d7b004-762f-4580-a027-852d660d8aaf',7),('348A6B99-1201-46CE-98E8-E84553EF0493','632a109c-6365-4e92-bbeb-bf876a321905',7),('348A6B99-1201-46CE-98E8-E84553EF0493','714b88fc-2fc2-467e-a013-4f0d0aa47757',0),('348A6B99-1201-46CE-98E8-E84553EF0493','72c94c45-d876-4209-a361-20612289fd32',7),('348A6B99-1201-46CE-98E8-E84553EF0493','7d842c78-6d8c-4cc5-912f-9ed51c828090',6),('348A6B99-1201-46CE-98E8-E84553EF0493','822928b9-8c47-46d2-8ca7-3e7e7b352933',6),('348a6b99-1201-46ce-98e8-e84553ef0493','d97c4db1-095e-4dc9-96b6-45dd2d6a7e0c',1),('3f80bd4b-25c2-436e-a801-e2d30da0f775','632a109c-6365-4e92-bbeb-bf876a321905',9),('3f80bd4b-25c2-436e-a801-e2d30da0f775','d97c4db1-095e-4dc9-96b6-45dd2d6a7e0c',0),('48A00ADD-4561-49C8-819F-199EFAB11AAB','28c44bae-296f-40b4-affb-277796494567',0),('48A00ADD-4561-49C8-819F-199EFAB11AAB','60d7b004-762f-4580-a027-852d660d8aaf',2),('48A00ADD-4561-49C8-819F-199EFAB11AAB','632a109c-6365-4e92-bbeb-bf876a321905',3),('48A00ADD-4561-49C8-819F-199EFAB11AAB','714b88fc-2fc2-467e-a013-4f0d0aa47757',2),('48A00ADD-4561-49C8-819F-199EFAB11AAB','72c94c45-d876-4209-a361-20612289fd32',2),('48A00ADD-4561-49C8-819F-199EFAB11AAB','7d842c78-6d8c-4cc5-912f-9ed51c828090',2),('48A00ADD-4561-49C8-819F-199EFAB11AAB','822928b9-8c47-46d2-8ca7-3e7e7b352933',3),('48a00add-4561-49c8-819f-199efab11aab','d97c4db1-095e-4dc9-96b6-45dd2d6a7e0c',2),('48A00ADD-4561-49C8-819F-199EFAB11AAB','E530AD41-72E5-479C-8E47-9BAF9176A2B4',1),('706937C9-300F-46EB-A629-DF7A8293FDEE','632a109c-6365-4e92-bbeb-bf876a321905',0),('85A43515-4F55-4503-9697-A2A553EB4B49','28c44bae-296f-40b4-affb-277796494567',1),('85A43515-4F55-4503-9697-A2A553EB4B49','60d7b004-762f-4580-a027-852d660d8aaf',3),('85A43515-4F55-4503-9697-A2A553EB4B49','632a109c-6365-4e92-bbeb-bf876a321905',2),('85A43515-4F55-4503-9697-A2A553EB4B49','72c94c45-d876-4209-a361-20612289fd32',3),('85A43515-4F55-4503-9697-A2A553EB4B49','7d842c78-6d8c-4cc5-912f-9ed51c828090',1),('85a43515-4f55-4503-9697-a2a553eb4b49','d97c4db1-095e-4dc9-96b6-45dd2d6a7e0c',3),('85A43515-4F55-4503-9697-A2A553EB4B49','E530AD41-72E5-479C-8E47-9BAF9176A2B4',2),('aad8aefd-afc6-4a03-ac5e-d8ae77e857b9','d97c4db1-095e-4dc9-96b6-45dd2d6a7e0c',6),('B2DAD012-5746-47FC-A1C9-768A83D7D3FF','28c44bae-296f-40b4-affb-277796494567',3),('B2DAD012-5746-47FC-A1C9-768A83D7D3FF','60d7b004-762f-4580-a027-852d660d8aaf',9),('B2DAD012-5746-47FC-A1C9-768A83D7D3FF','632a109c-6365-4e92-bbeb-bf876a321905',8),('B2DAD012-5746-47FC-A1C9-768A83D7D3FF','714b88fc-2fc2-467e-a013-4f0d0aa47757',6),('B2DAD012-5746-47FC-A1C9-768A83D7D3FF','72c94c45-d876-4209-a361-20612289fd32',9),('B2DAD012-5746-47FC-A1C9-768A83D7D3FF','7d842c78-6d8c-4cc5-912f-9ed51c828090',8),('B2DAD012-5746-47FC-A1C9-768A83D7D3FF','822928b9-8c47-46d2-8ca7-3e7e7b352933',7),('b2dad012-5746-47fc-a1c9-768a83d7d3ff','d97c4db1-095e-4dc9-96b6-45dd2d6a7e0c',4),('B2DAD012-5746-47FC-A1C9-768A83D7D3FF','E530AD41-72E5-479C-8E47-9BAF9176A2B4',4),('d54f1365-67a9-40bc-a69d-da6b59ce8723','d97c4db1-095e-4dc9-96b6-45dd2d6a7e0c',5),('F4E80C17-D198-4B74-832E-B08F893C6A76','60d7b004-762f-4580-a027-852d660d8aaf',1),('F4E80C17-D198-4B74-832E-B08F893C6A76','632a109c-6365-4e92-bbeb-bf876a321905',1),('F4E80C17-D198-4B74-832E-B08F893C6A76','72c94c45-d876-4209-a361-20612289fd32',1),('F4E80C17-D198-4B74-832E-B08F893C6A76','7d842c78-6d8c-4cc5-912f-9ed51c828090',0);
/*!40000 ALTER TABLE `profilegroupprofileorders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `profilegroups`
--

DROP TABLE IF EXISTS `profilegroups`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `profilegroups` (
  `Id` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `Name` varchar(64) COLLATE utf8mb4_swedish_ci NOT NULL,
  `Description` longtext COLLATE utf8mb4_swedish_ci,
  `CreatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `CreatedOn` datetime(3) NOT NULL,
  `UpdatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `UpdatedOn` datetime(3) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_ProfileGroup_Id_idx` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `profilegroups`
--

LOCK TABLES `profilegroups` WRITE;
/*!40000 ALTER TABLE `profilegroups` DISABLE KEYS */;
INSERT INTO `profilegroups` VALUES ('28c44bae-296f-40b4-affb-277796494567','Wi-Fi',NULL,'root','2019-03-04 10:17:47.000','root','2019-03-04 10:17:47.000'),('60d7b004-762f-4580-a027-852d660d8aaf','Local Office',NULL,'root','2019-03-04 10:17:47.000','root','2019-03-04 10:17:47.000'),('632a109c-6365-4e92-bbeb-bf876a321905','Studio',NULL,'root','2019-03-04 10:17:47.000','root','2019-03-04 10:17:47.000'),('714b88fc-2fc2-467e-a013-4f0d0aa47757','Satellite ',NULL,'root','2019-03-04 10:17:47.000','root','2019-03-04 10:17:47.000'),('72c94c45-d876-4209-a361-20612289fd32','Sport',NULL,'root','2019-03-04 10:17:47.000','root','2019-03-04 10:17:47.000'),('7d842c78-6d8c-4cc5-912f-9ed51c828090','Unspecified internal',NULL,'root','2019-03-04 10:17:47.000','root','2019-03-04 10:17:47.000'),('822928b9-8c47-46d2-8ca7-3e7e7b352933','Portable',NULL,'root','2019-03-04 10:17:47.000','root','2019-03-04 10:17:47.000'),('d97c4db1-095e-4dc9-96b6-45dd2d6a7e0c','Backpack',NULL,'root','2019-03-04 10:17:47.000','root','2019-03-04 10:17:47.000'),('E530AD41-72E5-479C-8E47-9BAF9176A2B4','Unspecified',NULL,'root','2019-03-04 10:17:47.000','root','2019-03-04 10:17:47.000');
/*!40000 ALTER TABLE `profilegroups` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `profiles`
--

DROP TABLE IF EXISTS `profiles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `profiles` (
  `Id` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `Name` varchar(64) COLLATE utf8mb4_swedish_ci NOT NULL,
  `Description` longtext COLLATE utf8mb4_swedish_ci,
  `Sdp` longtext COLLATE utf8mb4_swedish_ci NOT NULL,
  `CreatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `CreatedOn` datetime NOT NULL,
  `UpdatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `UpdatedOn` datetime NOT NULL,
  `SortIndex` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `profiles`
--

LOCK TABLES `profiles` WRITE;
/*!40000 ALTER TABLE `profiles` DISABLE KEYS */;
INSERT INTO `profiles` VALUES ('1D765BF5-8FD6-49AE-8D76-5E00564B7552','Vehicle SAT Stereo','ENH APTx, 24 bit, 576 kbps stereo, FEC 1/6 - original','v=0\no=dev-02 3599992693 3599992693 IN IP4 example.com\ns=Fordon Stereo\nc=IN IP4 203.0.113.12\nt=0 0\nm=audio 5004 RTP/AVP 96 97\na=rtpmap:96 aptx/48000/2\na=fmtp:96 variant=enhanced; bitresolution=24\na=rtpmap:97 parityfec/32000\na=fmtp:97 5006 IN IP4 203.0.113.12\na=sendrecv\na=ptime:10\na=ebuacip:protp 97 ratio=6\na=ebuacip:jb 0\na=ebuacip:jbdef 0 fixed 90\na=ebuacip:plength 96 10','root','2019-02-06 01:27:57','root','2019-02-06 01:27:57',3),('348A6B99-1201-46CE-98E8-E84553EF0493','Vehicle SAT Mono','ENH APTx, 24 bit, 192 kbps mono, FEC 1/6 - original','v=0\no=dev-02 3599993974 3599993974 IN IP4 example.com\ns=Vehicle\nc=IN IP4 203.0.113.12\nt=0 0\nm=audio 5004 RTP/AVP 96 97\na=rtpmap:96 aptx/32000/1\na=fmtp:96 variant=enhanced; bitresolution=24\na=rtpmap:97 parityfec/32000\na=fmtp:97 5006 IN IP4 203.0.113.12\na=sendrecv\na=ptime:17\na=ebuacip:protp 97 ratio=6\na=ebuacip:jb 0\na=ebuacip:jbdef 0 fixed 80\na=ebuacip:plength 96 17','root','2019-02-06 01:27:57','root','2019-02-06 01:27:57',2),('3F80BD4B-25C2-436E-A801-E2D30DA0F775','Backpack Stereo','OPUS 128 kbps CBR - original','v=0\no=dev-02 3599993476 3599993476 IN IP4 example.com\ns=Portable Mono\nc=IN IP4 203.0.113.12\nt=0 0\nm=audio 5004 RTP/AVP 96\na=rtpmap:96 opus/48000/2\na=fmtp:96 maxaveragebitrate=64000;stereo=1;sprop-stereo=1;cbr=1\na=sendrecv\na=ptime:20\na=ebuacip:jb 0\na=ebuacip:jbdef 0 fixed 220\na=ebuacip:plength 96 20','root','2019-02-06 01:27:57','root','2019-02-06 01:27:57',18),('4693a5f6-fe27-4b28-a609-0a04db05bdc8','Fordon SAT Stereo','OPUS 288 kbps CBR - original','v=0\no=dev-02 3599993476 3599993476 IN IP4 example.com\ns=Vehicle Stereo\nc=IN IP4 203.0.113.12\nt=0 0\nm=audio 5004 RTP/AVP 96\na=rtpmap:96 opus/48000/2\na=fmtp:96 stereo=1; sprop-stereo=1;maxaveragebitrate=288000;cbr=1\na=sendrecv\na=ptime:20\na=ebuacip:jb 0\na=ebuacip:jbdef 0 fixed 150\na=ebuacip:plength 96 20','root','2019-02-06 01:27:57','root','2019-02-06 01:27:57',21),('48A00ADD-4561-49C8-819F-199EFAB11AAB','Internet Mono','OPUS 64 kbps mono, FEC inband - original','v=0\r\no=mtu-02 3599993476 3599993476 IN IP4 example.com\r\ns=Internet Mono\r\nc=IN IP4 203.0.113.12\r\nt=0 0\r\nm=audio 5004 RTP/AVP 96\r\na=rtpmap:96 opus/48000/2\r\na=fmtp:96 stereo=0; sprop-stereo=0;maxaveragebitrate=64000;cbr=1;useinbandfec=1\r\na=sendrecv\r\na=ptime:20\r\na=ebuacip:jb 0\r\na=ebuacip:jbdef 0 fixed 350\r\na=ebuacip:plength 96 20','root','2019-02-06 01:27:57','root','2019-02-06 01:27:57',4),('4D5F1210-7073-48AC-8248-AEA3F48A64C3','Linkbox Stereo','APTX 384 kbps','v=0\no=dev-02 3599993476 3599993476 IN IP4 example.com\ns=Portable Mono\nc=IN IP4 203.0.113.12\nt=0 0\nm=audio 5004 RTP/AVP 96\na=rtpmap:96 opus/48000/2\na=fmtp:96 maxaveragebitrate=128000;stereo=1;sprop-stereo=1;cbr=1\na=sendrecv\na=ptime:20\na=ebuacip:jb 0\na=ebuacip:jbdef 0 fixed 220\na=ebuacip:plength 96 20','root','2019-02-06 01:27:57','root','2019-02-06 01:27:57',20),('706937C9-300F-46EB-A629-DF7A8293FDEE','Studio','PCM 24/48 stereo - original','v=0\r\no=default 3599993476 3599993476 IN IP4 example.com\r\ns=Studio\r\nc=IN IP4 203.0.113.12\r\nt=0 0\r\nm=audio 5004 RTP/AVP 96\r\na=rtpmap:96 L24/48000/2\r\na=sendrecv\r\na=ptime:5\r\na=ebuacip:jb 0\r\na=ebuacip:jbdef 0 fixed 6\r\na=ebuacip:plength 96 5','root','2019-02-06 01:27:57','root','2019-02-06 01:27:57',0),('85A43515-4F55-4503-9697-A2A553EB4B49','Internet Stereo','OPUS 256 kbps stereo, FEC inband - original','v=0\no=default 3599993476 3599993476 IN IP4 example.com\ns=Internet Stereo\nc=IN IP4 203.0.113.12\nt=0 0\nm=audio 5004 RTP/AVP 96\na=rtpmap:96 opus/48000/2\na=fmtp:96 maxaveragebitrate=256000;cbr=1;stereo=1;sprop-stereo=1\na=sendrecv\na=ptime:20\na=ebuacip:jb 0\na=ebuacip:jbdef 0 fixed 400\na=ebuacip:plength 96 20','root','2019-02-06 01:27:57','root','2019-02-06 01:27:57',19),('AAD8AEFD-AFC6-4A03-AC5E-D8AE77E857B9','Abroad Stereo','OPUS 192kbps Stereo CBR - original','v=0\no=default 3599993476 3599993476 IN IP4 example.com\ns=Outside Stereo\nc=IN IP4 203.0.113.12\nt=0 0\nm=audio 5004 RTP/AVP 96\na=rtpmap:96 opus/48000/2\na=fmtp:96 stereo=1; sprop-stereo=1;maxaveragebitrate=128000;cbr=1\na=sendrecv\na=ptime:20\na=ebuacip:jb 0\na=ebuacip:jbdef 0 fixed 220\na=ebuacip:plength 96 20','root','2019-02-06 01:27:57','root','2019-02-06 01:27:57',17),('B2DAD012-5746-47FC-A1C9-768A83D7D3FF','Telephone','OPUS 64 kbps mono, G.722, G.711 - original','v=0\no=default 3599993974 3599993974 IN IP4 example.com\ns=Phone\nc=IN IP4 203.0.113.12\nt=0 0\nm=audio 5004 RTP/AVP 96 9 8\na=rtpmap:96 opus/48000/2\na=fmtp:96 stereo=0; sprop-stereo=0;maxaveragebitrate=64000;cbr=1;useinbandfec=1\na=rtpmap:9 G722/8000\na=rtpmap:8 PCMA/8000\na=sendrecv\na=ptime:20\na=ebuacip:jb 0\na=ebuacip:jbdef 0 fixed 40\na=ebuacip:plength 96 20\na=ebuacip:plength 9 20\na=ebuacip:plength 8 20','root','2019-02-06 01:27:57','root','2019-02-06 01:27:57',6),('c7bf4633-04e9-4030-bbf6-15ac3b9d780d','Fordon SAT Mono','OPUS 144 kbps CBR - original','v=0\no=default 3599993476 3599993476 IN IP4 example.com\ns=Vehicle Mono Opus\nc=IN IP4 203.0.113.12\nt=0 0\nm=audio 5004 RTP/AVP 96\na=rtpmap:96 opus/48000/2\na=fmtp:96 stereo=0; sprop-stereo=0;maxaveragebitrate=144000;cbr=1\na=sendrecv\na=ptime:20\na=ebuacip:jb 0\na=ebuacip:jbdef 0 fixed 100\na=ebuacip:plength 96 20','root','2019-02-06 01:27:57','root','2019-02-06 01:27:57',22),('d54f1365-67a9-40bc-a69d-da6b59ce8723','Abroad Mono','OPUS 48kbps Mono - original','v=0\no=default 3599993476 3599993476 IN IP4 example.com\ns=Internet Mono\nc=IN IP4 203.0.113.12\nt=0 0\nm=audio 5004 RTP/AVP 96\na=rtpmap:96 opus/48000/2\na=fmtp:96 stereo=0; sprop-stereo=0;maxaveragebitrate=48000;cbr=1;useinbandfec=1\na=sendrecv\na=ptime:20\na=ebuacip:jb 0\na=ebuacip:jbdef 0 fixed 300\na=ebuacip:plength 96 20','root','2019-02-06 01:27:57','root','2019-02-06 01:27:57',23),('F4E80C17-D198-4B74-832E-B08F893C6A76','Arenas','ENH APTx, 24 bit, 576 kbps stereo - original','v=0\no=default 3599992693 3599992693 IN IP4 example.com\ns=Sport\nc=IN IP4 203.0.113.12\nt=0 0\nm=audio 5004 RTP/AVP 96\na=rtpmap:96 aptx/48000/2\na=fmtp:96 variant=enhanced; bitresolution=24\na=ptime:10\na=sendrecv\na=ebuacip:plength 96 10\na=ebuacip:jb 0\na=ebuacip:jbdef 0 fixed 80','root','2019-02-06 01:27:57','root','2019-02-06 01:27:57',1);
/*!40000 ALTER TABLE `profiles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `regions`
--

DROP TABLE IF EXISTS `regions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `regions` (
  `Id` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `Name` longtext COLLATE utf8mb4_swedish_ci,
  `CreatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `CreatedOn` datetime NOT NULL,
  `UpdatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `UpdatedOn` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `regions`
--

LOCK TABLES `regions` WRITE;
/*!40000 ALTER TABLE `regions` DISABLE KEYS */;
INSERT INTO `regions` VALUES ('015D0EEA-83D0-457E-B931-3F40FB38D0C8','Russia','root','2019-02-06 01:27:57','root','2019-04-17 13:34:57'),('11173C20-A2EE-4FC2-89A5-017E7A2B2B77','Europe','root','2019-02-06 01:27:57','root','2019-04-17 13:34:51'),('13F5CE95-7D35-4246-BED6-3961560C23A8','Warszawa','root','2019-02-06 01:27:57','root','2019-02-06 01:27:57'),('214822FF-8D90-45F3-A6B8-789FF87707D1','Unspecified','root','2019-02-06 01:27:57','root','2019-02-06 01:27:57'),('5B3B1DEF-85CC-464B-B64C-11C7D4DF4466','Amsterdam','root','2019-02-06 01:27:57','root','2019-02-06 01:27:57'),('5F91805D-8841-4D28-B02E-9C5FA3EAB450','Other','root','2019-02-06 01:27:57','root','2019-04-17 13:35:05'),('7cd0bca7-fe2e-4a75-a8da-74a13e770d5a','Frankfurt','root','2019-02-06 01:27:57','root','2019-04-17 13:36:00'),('A0282620-3262-464B-80E6-95535A38E858','Stockholm','root','2019-02-06 01:27:57','root','2019-02-06 01:27:57'),('A1132218-3E57-4471-93A3-0122FBCF8793','Milano','root','2019-02-06 01:27:57','root','2019-04-17 13:35:23'),('BD2A7668-E4DF-4515-9D36-6A30D7273764','Istanbul','root','2019-02-06 01:27:57','root','2019-02-06 01:27:57'),('C930AB2B-545F-4760-8891-1C8D56933084','Kairo','root','2019-02-06 01:27:57','root','2019-04-17 13:35:14'),('CBF5F7B9-9784-44E5-92BD-C073CF402543','Madrid','root','2019-02-06 01:27:57','root','2019-02-06 01:27:57'),('CF611ACA-B941-4736-AA07-D459A65A473E','International','root','2019-02-06 01:27:57','root','2019-04-17 13:34:41'),('CFC7C231-5D80-48E6-8B92-95FC8EDF53D1','Geneve','root','2019-02-06 01:27:57','root','2019-02-06 01:27:57'),('F5E3B1BD-44F6-4875-8E7E-479A418D2923','Prag','root','2019-02-06 01:27:57','root','2019-02-06 01:27:57');
/*!40000 ALTER TABLE `regions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `registeredsips`
--

DROP TABLE IF EXISTS `registeredsips`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `registeredsips` (
  `Id` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `SIP` varchar(128) COLLATE utf8mb4_swedish_ci NOT NULL,
  `UserAgentHead` longtext COLLATE utf8mb4_swedish_ci,
  `Username` longtext COLLATE utf8mb4_swedish_ci,
  `DisplayName` longtext COLLATE utf8mb4_swedish_ci,
  `IP` longtext COLLATE utf8mb4_swedish_ci,
  `Port` int(11) NOT NULL,
  `RegistrationType` tinyint(4) NOT NULL DEFAULT '0',
  `ServerTimeStamp` bigint(20) DEFAULT NULL,
  `Updated` datetime NOT NULL,
  `Expires` int(11) NOT NULL,
  `IsActive` tinyint(4) NOT NULL DEFAULT '0',
  `UserAgentId` char(36) COLLATE utf8mb4_swedish_ci DEFAULT NULL,
  `Location_LocationId` char(36) COLLATE utf8mb4_swedish_ci DEFAULT NULL,
  `User_UserId` char(36) COLLATE utf8mb4_swedish_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_UserAgentId` (`UserAgentId`),
  KEY `IX_Location_LocationId` (`Location_LocationId`),
  KEY `IX_User_UserId` (`User_UserId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `registeredsips`
--

LOCK TABLES `registeredsips` WRITE;
/*!40000 ALTER TABLE `registeredsips` DISABLE KEYS */;
INSERT INTO `registeredsips` VALUES ('02fc8613-6946-4235-bf5a-22477a8b3ef7','pbx@sipdomain.com','Asterisk PBX 11.13.1','pbx@sipdomain.com','PBX Service','10.121.194.213',5080,0,1647565332,'2020-01-20 15:15:49',97,0,'391f4e31-9f6e-41c5-a787-1cb0f63035ef','88e3d0de-9cce-4bd4-b7af-7ec20e7ef429','01217de1-9f81-4e67-bfee-97df400c7fa1'),('0324949f-42ed-499e-af3c-8dc1b9697ef1','studio-pool-001@sipdomain.com','Nerea/h2.2.6','studio-pool-001@sipdomain.com','Studio pool 1','11.15.154.86',5060,0,1842805239,'2020-01-20 15:15:49',58,0,'4e53c5be-748e-4fd0-9f4b-3461379cf1b4','5f22e14a-793d-4b5f-91b5-b57c3b99f599','01814726-0A9E-4071-8B26-D05DB331EAA9'),('136D9F77-CA80-49D3-BC27-02B77EE3B644','john.doe@sipdomain.com','Blink Pro 4.5.1 (MacOSX)','john.doe@sipdomain.com','John Doe','211.15.160.52',5090,0,1505200163,'2020-01-20 15:15:49',124,0,'28C8D173-67EE-40A4-8850-A28133A388C2','5F22E14A-793D-4B5F-91B5-B57C3B99F599','01DA6EF7-FE8B-4D59-9B45-8DAF843F45AA'),('21ABB1AD-38B6-4211-9098-7F6906752F5E','ob-portable-001@sipdomain.com','Quantum/3.5.0a','ob-portable-001@sipdomain.com','OB Portable 1','138.128.94.34',5060,0,1460636422,'2020-01-20 15:15:49',0,0,'6FE0FD55-CAFB-4D57-8818-8A289511C4B5','5F22E14A-793D-4B5F-91B5-B57C3B99F599','042b9cd3-e2fe-4055-846f-4ea8df1f9a61'),('27CD88DC-5FBC-439F-B470-BFEEE91852F3','studio-pool-002@sipdomain.com','QuantumST/3.5.0a','studio-pool-002@sipdomain.com','Studio pool 2','121.19.127.234',5060,0,1458657572,'2020-01-20 15:15:49',0,0,'834EA3A6-1F25-4F4B-961F-97ECFE351BB8','0A10C58F-EF64-455D-B907-3C1273FAF6C1','05F57AD6-16D0-421B-9618-36A1CC799EC3'),('4DDBDBEB-89E7-4232-BEA4-748B4D5E7B37','dev-001@sipdomain.com','ProntoNetLCv6.8.4','dev-001@sipdomain.com','DEV 001','159.203.47.235',1148,0,1457544803,'2020-01-20 15:15:49',0,0,'4444026F-3854-4AAB-95DF-E182D5EB5B36','88E3D0DE-9CCE-4BD4-B7AF-7EC20E7EF429','063F5B48-6A59-4632-8CF8-0BC93EB44FFA'),('4e6f8ed4-1d22-46f4-a81a-d6dbc8f581ac','pis.rartsiger@sipdomain.com','snom710/8.7.5.35','pis.rartsiger@sipdomain.com','Pis Rartsiger','11.230.110.132',32768,0,1545220316,'2020-01-20 15:15:49',124,0,'ba5b9883-ed3e-46c5-b34b-9f6fb5edcc63','79672557-6cba-471c-82db-e16ad5ea1d59','0758335C-9F9F-46DA-A227-2FEE087D73EC'),('6821859E-53AC-4A3A-A14D-C63495F56655','facility-001@sipdomain.com','ME-UMAC-C/5.18','facility-001@sipdomain.com','Facility 1','121.15.24.106',5060,0,1498030497,'2020-01-20 15:15:49',60,0,'C086F4A5-EBE9-46FC-AF6A-78ADF0389F26','1FCA4F89-C062-42CB-B4ED-2D4451164312','083d21ec-9fdd-434c-9e7b-4611f7ec9e6b'),('6d19425f-6a23-404f-9854-3f9dc55bde3b','backpack-001@sipdomain.com','baresip v0.5.10 (arm6/linux)','backpack-001@sipdomain.com','Backpack 1','11.29.194.136',5060,0,1547565332,'2020-01-20 15:15:49',58,0,'a3e19e66-736b-4a49-86a7-6985ba997b43','5f22e14a-793d-4b5f-91b5-b57c3b99f599','09375200-7204-421f-9d12-eca29d75e166'),('7722B5B4-2089-4A0F-9BE9-57B6C89F5312','block-line@sipdomain.com','AETA/ScoopTeam aoip-v2.00.0012','block-line@sipdomain.com','Block Connection','8.30.194.202',19914,0,1496056496,'2020-01-20 15:15:49',88,0,NULL,'88E3D0DE-9CCE-4BD4-B7AF-7EC20E7EF429','0A420FD3-13D9-4001-8F2F-B605F966FF70'),('86c2eb72-2094-4741-9b32-28c03d66181c','traffic-report@sipdomain.com','Bria 4 release 4.7.0 stamp 83078','traffic-report@sipdomain.com','Traffic report','50.23.161.172',55720,0,1527604307,'2020-01-20 15:15:49',124,0,'f310f519-08c7-499a-a102-c02b7a35485c','5f22e14a-793d-4b5f-91b5-b57c3b99f599','0a6d003c-fb20-4422-8ed7-af292a7100ef'),('974A8AA1-DA5F-4148-BAB0-FBC2BD70B868','tim.johnsson@@sipdomain.com','tSIP 0.01.41.00','tim.johnsson@sipdomain.com','Tim Johnsson','123.5.65.190',61664,0,1459004730,'2020-01-20 15:15:49',-1,0,NULL,'88E3D0DE-9CCE-4BD4-B7AF-7EC20E7EF429','0AB7C5E2-8AAA-4BFE-B20B-B812BB5F9439'),('9ec856fa-cc47-4031-8882-9a897fb2770c','local-department@sipdomain.com','ME-UMAC2-M/1.38','local-department@sipdomain.com','Local Department','209.45.255.122',49483,0,1547130528,'2020-01-20 15:15:49',25,0,'ee394836-e09b-4cc5-91a6-0589befc61cf','88e3d0de-9cce-4bd4-b7af-7ec20e7ef429','0ACC8882-E7A2-4712-ADF0-8D50D8E99609'),('A4F083D7-327F-4490-A19D-474845E06946','lisa.fredriksen@sipdomain.com','LUCI Live SR_3.1.11_iPhone8.1_(iOS_10.3)','lisa.fredriksen@sipdomain.com','Lisa Fredriksen','77.220.40.116',31369,0,1491570054,'2020-01-20 15:15:49',0,0,'F4950560-BF03-4C01-81C5-435C53587238','67E2541A-7870-4CF9-A4F1-6E97B951168A','0B387A2B-3767-4829-9458-7B1FDA6E4210'),('AE3EE31E-9B5C-49F2-9624-F90EB41CDD8B','mike.golden@sipdomain.com','MicroSIP/3.15.1','mike.golden@sipdomain.com','Mike Golden','130.101.32.162',62471,0,1491029728,'2020-01-20 15:15:49',0,0,NULL,'88E3D0DE-9CCE-4BD4-B7AF-7EC20E7EF429','0D7F7ED5-2C04-451D-821D-24532995CD99'),('BDEB2094-A9B1-49CB-8722-5557C89AFC1C','miranda.stewart@sipdomain.com','Comrex SIP','miranda.stewart@sipdomain.com','Miranda Stewart','129.77.137.161',5060,0,1459519384,'2020-01-20 15:15:49',0,0,'A2118E5F-B0E1-4B31-BB43-02379801F0E0','88E3D0DE-9CCE-4BD4-B7AF-7EC20E7EF429','0dc9a4e1-4487-4392-b4d7-f96053b82f3a'),('D029EAD8-2D71-4545-90A0-7CEE5F85E46A','yang.weng@sipdomain.com','Linphone/3.9.1 (belle-sip/1.4.2)','yang.weng@sipdomain.com','Yang Weng','82.65.161.37',5060,0,1484752648,'2020-01-20 15:15:49',0,0,'18B9E420-7918-44B9-B7E2-B4698E6035BD','5F22E14A-793D-4B5F-91B5-B57C3B99F599','0e55a76e-ca12-4455-9327-31c9761a6f96');
/*!40000 ALTER TABLE `registeredsips` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `roles`
--

DROP TABLE IF EXISTS `roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `roles` (
  `Id` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `Name` longtext COLLATE utf8mb4_swedish_ci,
  `CreatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `CreatedOn` datetime NOT NULL,
  `UpdatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `UpdatedOn` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `roles`
--

LOCK TABLES `roles` WRITE;
/*!40000 ALTER TABLE `roles` DISABLE KEYS */;
INSERT INTO `roles` VALUES ('0C5F9098-A09A-44B1-8603-2DEC46E8E7EF','Remote','root','2019-02-13 16:50:00','root','2019-02-13 16:50:00'),('88783AD0-F83F-40F0-AB46-F12938DFE68A','AccountManager','root','2019-02-13 16:50:00','root','2019-02-13 16:50:00'),('93F8370B-3321-42CB-A7BF-856AA8E6446E','Admin','root','2019-02-13 16:50:00','root','2019-02-13 16:50:00');
/*!40000 ALTER TABLE `roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `settings`
--

DROP TABLE IF EXISTS `settings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `settings` (
  `Id` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `Name` longtext COLLATE utf8mb4_swedish_ci,
  `Value` longtext COLLATE utf8mb4_swedish_ci,
  `Description` longtext COLLATE utf8mb4_swedish_ci,
  `CreatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `CreatedOn` datetime NOT NULL,
  `UpdatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `UpdatedOn` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `settings`
--

LOCK TABLES `settings` WRITE;
/*!40000 ALTER TABLE `settings` DISABLE KEYS */;
INSERT INTO `settings` VALUES ('3EF6CC4F-4343-4CD9-9FF3-04447118AC65','CodecControlActive','true','Codec control on/off ','root','2019-02-13 16:50:00','root','2019-02-13 16:50:00'),('A2570519-60D4-49B8-A32A-4FDC7B194BB1','SIPDomain','sipdomain.com','The SIP domain','root','2019-02-13 16:50:00','root','2019-02-13 16:50:00'),('C2413728-20F1-4443-B13F-9F26D7DCE25D','UseSipEvent','true','Recieve Kamailio-messages in JSON-format','root','2019-02-13 16:50:00','root','2019-02-13 16:50:00'),('D676D91D-B1CF-4F48-B983-1BD9112962D0','MaxRegistrationAge','130','Time in seconds before SIP registration is obsolete','root','2019-02-13 16:50:00','root','2019-02-13 16:50:00'),('EFF88C41-3DDB-4434-A76F-C2BAD39811B0','LatestCallCount','30','Number of closed calls to show','root','2019-02-13 16:50:00','root','2019-02-13 16:50:00'),('F21A6278-4A7C-4FE2-8871-D5BE6FFD7358','UseOldKamailioEvent','false','Recieve Kamailio-messages in old format','root','2019-02-13 16:50:00','root','2019-02-13 16:50:00');
/*!40000 ALTER TABLE `settings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sipaccounts`
--

DROP TABLE IF EXISTS `sipaccounts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `sipaccounts` (
  `Id` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `UserName` longtext COLLATE utf8mb4_swedish_ci,
  `DisplayName` longtext COLLATE utf8mb4_swedish_ci,
  `Comment` longtext COLLATE utf8mb4_swedish_ci,
  `ExtensionNumber` longtext COLLATE utf8mb4_swedish_ci,
  `AccountType` int(11) NOT NULL DEFAULT '0',
  `IsPoolCodec` tinyint(4) NOT NULL DEFAULT '0',
  `AccountLocked` tinyint(4) NOT NULL DEFAULT '0',
  `Password` longtext COLLATE utf8mb4_swedish_ci,
  `CreatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `CreatedOn` datetime NOT NULL,
  `UpdatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `UpdatedOn` datetime NOT NULL,
  `CodecType_Id` char(36) COLLATE utf8mb4_swedish_ci DEFAULT NULL,
  `Owner_Id` char(36) COLLATE utf8mb4_swedish_ci DEFAULT NULL,
  `Region_Id` char(36) COLLATE utf8mb4_swedish_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_SipAccounts_CodecTypeId` (`CodecType_Id`),
  KEY `IX_SipAccounts_OwnerId` (`Owner_Id`),
  KEY `IX_SipAccounts_RegionId` (`Region_Id`),
  CONSTRAINT `FK_SipAccounts_CodecTypeId` FOREIGN KEY (`CodecType_Id`) REFERENCES `codectypes` (`Id`),
  CONSTRAINT `FK_SipAccounts_OwnerId` FOREIGN KEY (`Owner_Id`) REFERENCES `owners` (`Id`),
  CONSTRAINT `FK_SipAccounts_RegionId` FOREIGN KEY (`Region_Id`) REFERENCES `regions` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sipaccounts`
--

LOCK TABLES `sipaccounts` WRITE;
/*!40000 ALTER TABLE `sipaccounts` DISABLE KEYS */;
INSERT INTO `sipaccounts` VALUES ('01217de1-9f81-4e67-bfee-97df400c7fa1','pbx@sipdomain.com','PBX Service',NULL,NULL,0,0,0,'t3aC1yJ9PJY9Rms','root','2018-03-13 15:05:57','root','2019-04-17 13:29:44','2fdb03e3-5e28-4640-a609-1b2098c8819c','58790be0-5607-44a7-9d9f-d763a67891bb',NULL),('01814726-0A9E-4071-8B26-D05DB331EAA9','studio-pool-001@sipdomain.com','Studio pool 1',NULL,NULL,0,1,0,'y5x%!nH7Zb!kObY','root','2016-04-29 07:43:14','root','2019-04-17 13:31:20','531c49af-4ed1-4e32-851f-482748b1259f','58790BE0-5607-44A7-9D9F-D763A67891BB',NULL),('01DA6EF7-FE8B-4D59-9B45-8DAF843F45AA','john.doe@sipdomain.com','John Doe',NULL,NULL,0,0,0,'hUh3g1Zexqw62o2','root','1900-01-01 00:00:00','root','2019-04-17 13:29:05','9fbe2aa7-0080-45a4-9b50-07a12c12ed8a','58790BE0-5607-44A7-9D9F-D763A67891BB',NULL),('042b9cd3-e2fe-4055-846f-4ea8df1f9a61','ob-portable-001@sipdomain.com','OB Portable 1',NULL,NULL,0,0,0,'QpysXRPhO8a931F','root','2018-01-22 12:14:44','root','2019-04-17 13:29:30','f50604d9-3442-452a-b600-28d8d1e2f056','58790be0-5607-44a7-9d9f-d763a67891bb',NULL),('05F57AD6-16D0-421B-9618-36A1CC799EC3','studio-pool-002@sipdomain.com','Studio pool 2',NULL,NULL,0,1,0,'fzph3i8sgrMIdth','root','2015-09-21 13:01:58','root','2019-04-17 13:31:23','531C49AF-4ED1-4E32-851F-482748B1259F','58790BE0-5607-44A7-9D9F-D763A67891BB',NULL),('063F5B48-6A59-4632-8CF8-0BC93EB44FFA','dev-001@sipdomain.com','DEV 001',NULL,NULL,0,0,0,'GZSkL1yw6v7q#yj','root','2015-09-25 08:26:15','root','2019-04-17 13:31:13','63a48bac-1c58-4e84-9bc3-6dd8494b45d5','58790BE0-5607-44A7-9D9F-D763A67891BB',NULL),('0758335C-9F9F-46DA-A227-2FEE087D73EC','pis.rartsiger@sipdomain.com','Pis Rartsiger',NULL,NULL,0,0,0,'US!Ew4qPjqSIcVr','root','2015-06-01 10:28:25','root','2019-04-17 13:31:16','9FBE2AA7-0080-45A4-9B50-07A12C12ED8A','58790BE0-5607-44A7-9D9F-D763A67891BB',NULL),('083d21ec-9fdd-434c-9e7b-4611f7ec9e6b','facility-001@sipdomain.com','Facility 1',NULL,NULL,0,0,0,'yd9VrmhQ7lsjbfV','root','2018-04-25 07:14:50','root','2019-04-17 13:28:59','176b09ed-38ee-49ad-9fcd-922ac93ca71d','58790BE0-5607-44A7-9D9F-D763A67891BB',NULL),('09375200-7204-421f-9d12-eca29d75e166','backpack-001@sipdomain.com','Backpack 1',NULL,NULL,0,0,0,'ziHP18M7cIEzjNZ','root','2018-08-10 09:00:29','root','2019-04-17 13:30:57','63a48bac-1c58-4e84-9bc3-6dd8494b45d5','58790be0-5607-44a7-9d9f-d763a67891bb',NULL),('0A420FD3-13D9-4001-8F2F-B605F966FF70','block-line@sipdomain.com','Block Connection',NULL,NULL,0,0,0,'6p!zOgPQpEXETHz','root','2017-07-28 08:55:28','root','2019-04-17 13:31:09','531c49af-4ed1-4e32-851f-482748b1259f','58790BE0-5607-44A7-9D9F-D763A67891BB',NULL),('0a6d003c-fb20-4422-8ed7-af292a7100ef','traffic-report@sipdomain.com','Traffic report',NULL,NULL,0,0,0,'iebn1H2bU3!4Sgd','root','2018-06-14 09:39:07','root','2019-04-17 13:31:26','2fdb03e3-5e28-4640-a609-1b2098c8819c','58790be0-5607-44a7-9d9f-d763a67891bb',NULL),('0AB7C5E2-8AAA-4BFE-B20B-B812BB5F9439','tim.johnsson@sipdomain.com','Tim Johnsson',NULL,NULL,0,0,0,'t7X86fOV7cm#z6g','root','2014-09-30 13:42:14','root','2019-04-17 13:41:09','9FBE2AA7-0080-45A4-9B50-07A12C12ED8A','58790BE0-5607-44A7-9D9F-D763A67891BB',NULL),('0ACC8882-E7A2-4712-ADF0-8D50D8E99609','local-department@sipdomain.com','Local Department',NULL,NULL,0,0,0,'7RosQgKbbtc37c3','root','2017-08-10 14:05:14','root','2019-04-17 13:30:53','176b09ed-38ee-49ad-9fcd-922ac93ca71d','58790BE0-5607-44A7-9D9F-D763A67891BB',NULL),('0B387A2B-3767-4829-9458-7B1FDA6E4210','lisa.fredriksen@sipdomain.com','Lisa Fredriksen',NULL,NULL,0,0,0,'SqDPGP1CyyU7FOS','root','2015-05-21 13:13:36','root','2019-04-17 13:31:00','F50604D9-3442-452A-B600-28D8D1E2F056','58790BE0-5607-44A7-9D9F-D763A67891BB',NULL),('0D7F7ED5-2C04-451D-821D-24532995CD99','mike.golden@sipdomain.com','Mike Golden',NULL,NULL,0,0,0,'KVc4RgYz6I6Xf36','root','2016-01-19 13:11:29','root','2019-04-17 13:29:13','9fbe2aa7-0080-45a4-9b50-07a12c12ed8a','58790BE0-5607-44A7-9D9F-D763A67891BB',NULL),('0dc9a4e1-4487-4392-b4d7-f96053b82f3a','miranda.stewart@sipdomain.com','Miranda Stewart',NULL,NULL,0,0,0,'nCPwOqCId08%ab2','root','2018-04-23 11:39:53','root','2019-04-17 13:29:33','9ea80a94-c092-4fbd-8858-9b5da446f0ed','58790be0-5607-44a7-9d9f-d763a67891bb',NULL),('0e55a76e-ca12-4455-9327-31c9761a6f96','yang.weng@sipdomain.com','Yang Weng',NULL,NULL,0,0,0,'f8HD!oqqAr0yogx','root','2018-04-25 07:14:50','root','2019-04-17 13:29:26','f50604d9-3442-452a-b600-28d8d1e2f056','58790BE0-5607-44A7-9D9F-D763A67891BB',NULL),('c5d987a6-d18d-4c01-aeba-24c728a4ed9b','rogsantestaliasusername@contrib.sr.se','Roger Test',NULL,NULL,0,0,0,'Nm4w%L5H%AF99Qh','root','2019-02-04 22:13:09','root','2019-04-17 13:29:51','531c49af-4ed1-4e32-851f-482748b1259f','58790BE0-5607-44A7-9D9F-D763A67891BB','11173c20-a2ee-4fc2-89a5-017e7a2b2b77');
/*!40000 ALTER TABLE `sipaccounts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `studios`
--

DROP TABLE IF EXISTS `studios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `studios` (
  `Id` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `Name` longtext COLLATE utf8mb4_swedish_ci NOT NULL,
  `CodecSipAddress` longtext COLLATE utf8mb4_swedish_ci,
  `CameraAddress` longtext COLLATE utf8mb4_swedish_ci,
  `CameraActive` tinyint(4) NOT NULL,
  `CameraUsername` longtext COLLATE utf8mb4_swedish_ci,
  `CameraPassword` longtext COLLATE utf8mb4_swedish_ci,
  `CameraVideoUrl` longtext COLLATE utf8mb4_swedish_ci,
  `CameraImageUrl` longtext COLLATE utf8mb4_swedish_ci,
  `CameraPlayAudioUrl` longtext COLLATE utf8mb4_swedish_ci,
  `AudioClipNames` longtext COLLATE utf8mb4_swedish_ci,
  `InfoText` longtext COLLATE utf8mb4_swedish_ci,
  `MoreInfoUrl` longtext COLLATE utf8mb4_swedish_ci,
  `NrOfAudioInputs` int(11) NOT NULL,
  `AudioInputNames` longtext COLLATE utf8mb4_swedish_ci,
  `AudioInputDefaultGain` int(11) NOT NULL,
  `NrOfGpos` int(11) NOT NULL,
  `GpoNames` longtext COLLATE utf8mb4_swedish_ci,
  `InactivityTimeout` int(11) NOT NULL,
  `CreatedBy` longtext COLLATE utf8mb4_swedish_ci NOT NULL,
  `CreatedOn` datetime NOT NULL,
  `UpdatedBy` longtext COLLATE utf8mb4_swedish_ci NOT NULL,
  `UpdatedOn` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `studios`
--

LOCK TABLES `studios` WRITE;
/*!40000 ALTER TABLE `studios` DISABLE KEYS */;
INSERT INTO `studios` VALUES ('2DF5686C-7097-4DBD-8BD6-1D0D5B428FA7','Studio Test','studio-entrance-sthlm@contrib.sr.se','kamera.mycompany.se',0,NULL,NULL,'/mjpg/video.mjpg?camera=1&resolution=640x480&fps=25&compression=30','/axis-cgi/jpg/image.cgi?camera=1','/axis-cgi/playclip.cgi','Welcome instructions, Ask them to call','To see the camera use Chrome or Firefox. Always read the manual before first use.\r\n','http://www.mycompany.se/manual.pdf',3,'Red, Blue, Yellow',-30,1,'Green light',15,'root','2017-02-12 21:30:31','root','2019-02-06 01:53:56');
/*!40000 ALTER TABLE `studios` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `useragentprofileorders`
--

DROP TABLE IF EXISTS `useragentprofileorders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `useragentprofileorders` (
  `UserAgentId` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `ProfileId` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `SortIndex` int(11) NOT NULL,
  PRIMARY KEY (`UserAgentId`,`ProfileId`),
  KEY `IX_ProfileId` (`ProfileId`),
  KEY `IX_UserAgentId` (`UserAgentId`),
  CONSTRAINT `FK_dbo.UserAgentProfileOrders_dbo.Profiles_ProfileId` FOREIGN KEY (`ProfileId`) REFERENCES `profiles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_dbo.UserAgentProfileOrders_dbo.UserAgents_UserAgentId` FOREIGN KEY (`UserAgentId`) REFERENCES `useragents` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `useragentprofileorders`
--

LOCK TABLES `useragentprofileorders` WRITE;
/*!40000 ALTER TABLE `useragentprofileorders` DISABLE KEYS */;
INSERT INTO `useragentprofileorders` VALUES ('12c6d8c1-0ac0-46f3-b6fd-53ded04ab388','48a00add-4561-49c8-819f-199efab11aab',0),('12c6d8c1-0ac0-46f3-b6fd-53ded04ab388','b2dad012-5746-47fc-a1c9-768a83d7d3ff',1),('18B9E420-7918-44B9-B7E2-B4698E6035BD','48A00ADD-4561-49C8-819F-199EFAB11AAB',0),('18B9E420-7918-44B9-B7E2-B4698E6035BD','B2DAD012-5746-47FC-A1C9-768A83D7D3FF',1),('1973A2C9-F7F2-43B3-A4F4-795502ADEDAF','48A00ADD-4561-49C8-819F-199EFAB11AAB',0),('2576E6D7-F490-4712-B07D-871DFC140A66','1D765BF5-8FD6-49AE-8D76-5E00564B7552',3),('2576E6D7-F490-4712-B07D-871DFC140A66','348A6B99-1201-46CE-98E8-E84553EF0493',2),('2576E6D7-F490-4712-B07D-871DFC140A66','48A00ADD-4561-49C8-819F-199EFAB11AAB',4),('2576E6D7-F490-4712-B07D-871DFC140A66','706937C9-300F-46EB-A629-DF7A8293FDEE',0),('2576E6D7-F490-4712-B07D-871DFC140A66','B2DAD012-5746-47FC-A1C9-768A83D7D3FF',6),('2576E6D7-F490-4712-B07D-871DFC140A66','F4E80C17-D198-4B74-832E-B08F893C6A76',1),('28C8D173-67EE-40A4-8850-A28133A388C2','B2DAD012-5746-47FC-A1C9-768A83D7D3FF',0),('311dfec3-e460-4a45-84a8-26e866b04101','48a00add-4561-49c8-819f-199efab11aab',0),('311dfec3-e460-4a45-84a8-26e866b04101','b2dad012-5746-47fc-a1c9-768a83d7d3ff',1),('312B9589-10F4-4BDB-92EF-B3623A70A8B8','1D765BF5-8FD6-49AE-8D76-5E00564B7552',4),('312B9589-10F4-4BDB-92EF-B3623A70A8B8','348A6B99-1201-46CE-98E8-E84553EF0493',3),('312B9589-10F4-4BDB-92EF-B3623A70A8B8','48A00ADD-4561-49C8-819F-199EFAB11AAB',5),('312B9589-10F4-4BDB-92EF-B3623A70A8B8','4D5F1210-7073-48AC-8248-AEA3F48A64C3',14),('312B9589-10F4-4BDB-92EF-B3623A70A8B8','706937C9-300F-46EB-A629-DF7A8293FDEE',1),('312B9589-10F4-4BDB-92EF-B3623A70A8B8','85A43515-4F55-4503-9697-A2A553EB4B49',13),('312B9589-10F4-4BDB-92EF-B3623A70A8B8','AAD8AEFD-AFC6-4A03-AC5E-D8AE77E857B9',12),('312B9589-10F4-4BDB-92EF-B3623A70A8B8','B2DAD012-5746-47FC-A1C9-768A83D7D3FF',7),('312B9589-10F4-4BDB-92EF-B3623A70A8B8','F4E80C17-D198-4B74-832E-B08F893C6A76',2),('33D9F8EA-A1B7-4A98-9BE7-8655CCF7AED7','1D765BF5-8FD6-49AE-8D76-5E00564B7552',2),('33D9F8EA-A1B7-4A98-9BE7-8655CCF7AED7','348A6B99-1201-46CE-98E8-E84553EF0493',1),('33D9F8EA-A1B7-4A98-9BE7-8655CCF7AED7','706937C9-300F-46EB-A629-DF7A8293FDEE',3),('33D9F8EA-A1B7-4A98-9BE7-8655CCF7AED7','B2DAD012-5746-47FC-A1C9-768A83D7D3FF',4),('33D9F8EA-A1B7-4A98-9BE7-8655CCF7AED7','F4E80C17-D198-4B74-832E-B08F893C6A76',0),('391F4E31-9F6E-41C5-A787-1CB0F63035EF','B2DAD012-5746-47FC-A1C9-768A83D7D3FF',0),('4444026F-3854-4AAB-95DF-E182D5EB5B36','1D765BF5-8FD6-49AE-8D76-5E00564B7552',3),('4444026F-3854-4AAB-95DF-E182D5EB5B36','348A6B99-1201-46CE-98E8-E84553EF0493',2),('4444026F-3854-4AAB-95DF-E182D5EB5B36','706937C9-300F-46EB-A629-DF7A8293FDEE',0),('4444026F-3854-4AAB-95DF-E182D5EB5B36','F4E80C17-D198-4B74-832E-B08F893C6A76',1),('4982FD0A-21CB-4BE0-A96F-A6CB3CC7F3D7','1D765BF5-8FD6-49AE-8D76-5E00564B7552',3),('4982FD0A-21CB-4BE0-A96F-A6CB3CC7F3D7','348A6B99-1201-46CE-98E8-E84553EF0493',2),('4982FD0A-21CB-4BE0-A96F-A6CB3CC7F3D7','48A00ADD-4561-49C8-819F-199EFAB11AAB',4),('4982FD0A-21CB-4BE0-A96F-A6CB3CC7F3D7','4D5F1210-7073-48AC-8248-AEA3F48A64C3',12),('4982FD0A-21CB-4BE0-A96F-A6CB3CC7F3D7','706937C9-300F-46EB-A629-DF7A8293FDEE',0),('4982FD0A-21CB-4BE0-A96F-A6CB3CC7F3D7','85A43515-4F55-4503-9697-A2A553EB4B49',9),('4982FD0A-21CB-4BE0-A96F-A6CB3CC7F3D7','AAD8AEFD-AFC6-4A03-AC5E-D8AE77E857B9',10),('4982FD0A-21CB-4BE0-A96F-A6CB3CC7F3D7','B2DAD012-5746-47FC-A1C9-768A83D7D3FF',11),('4982FD0A-21CB-4BE0-A96F-A6CB3CC7F3D7','F4E80C17-D198-4B74-832E-B08F893C6A76',1),('4E53C5BE-748E-4FD0-9F4B-3461379CF1B4','1D765BF5-8FD6-49AE-8D76-5E00564B7552',4),('4E53C5BE-748E-4FD0-9F4B-3461379CF1B4','348A6B99-1201-46CE-98E8-E84553EF0493',3),('4E53C5BE-748E-4FD0-9F4B-3461379CF1B4','3F80BD4B-25C2-436E-A801-E2D30DA0F775',13),('4E53C5BE-748E-4FD0-9F4B-3461379CF1B4','48A00ADD-4561-49C8-819F-199EFAB11AAB',5),('4E53C5BE-748E-4FD0-9F4B-3461379CF1B4','706937C9-300F-46EB-A629-DF7A8293FDEE',0),('4e53c5be-748e-4fd0-9f4b-3461379cf1b4','85a43515-4f55-4503-9697-a2a553eb4b49',2),('4E53C5BE-748E-4FD0-9F4B-3461379CF1B4','B2DAD012-5746-47FC-A1C9-768A83D7D3FF',7),('4E53C5BE-748E-4FD0-9F4B-3461379CF1B4','F4E80C17-D198-4B74-832E-B08F893C6A76',1),('50fd3e73-a4fd-48e7-9613-88369aeb866f','1d765bf5-8fd6-49ae-8d76-5e00564b7552',3),('50fd3e73-a4fd-48e7-9613-88369aeb866f','348a6b99-1201-46ce-98e8-e84553ef0493',2),('50fd3e73-a4fd-48e7-9613-88369aeb866f','3f80bd4b-25c2-436e-a801-e2d30da0f775',13),('50fd3e73-a4fd-48e7-9613-88369aeb866f','4693a5f6-fe27-4b28-a609-0a04db05bdc8',16),('50fd3e73-a4fd-48e7-9613-88369aeb866f','48a00add-4561-49c8-819f-199efab11aab',4),('50fd3e73-a4fd-48e7-9613-88369aeb866f','4d5f1210-7073-48ac-8248-aea3f48a64c3',15),('50fd3e73-a4fd-48e7-9613-88369aeb866f','706937c9-300f-46eb-a629-df7a8293fdee',0),('50fd3e73-a4fd-48e7-9613-88369aeb866f','85a43515-4f55-4503-9697-a2a553eb4b49',14),('50fd3e73-a4fd-48e7-9613-88369aeb866f','aad8aefd-afc6-4a03-ac5e-d8ae77e857b9',12),('50fd3e73-a4fd-48e7-9613-88369aeb866f','b2dad012-5746-47fc-a1c9-768a83d7d3ff',6),('50fd3e73-a4fd-48e7-9613-88369aeb866f','c7bf4633-04e9-4030-bbf6-15ac3b9d780d',17),('50fd3e73-a4fd-48e7-9613-88369aeb866f','d54f1365-67a9-40bc-a69d-da6b59ce8723',18),('50fd3e73-a4fd-48e7-9613-88369aeb866f','f4e80c17-d198-4b74-832e-b08f893c6a76',1),('6FE0FD55-CAFB-4D57-8818-8A289511C4B5','1D765BF5-8FD6-49AE-8D76-5E00564B7552',4),('6FE0FD55-CAFB-4D57-8818-8A289511C4B5','348A6B99-1201-46CE-98E8-E84553EF0493',6),('6FE0FD55-CAFB-4D57-8818-8A289511C4B5','3F80BD4B-25C2-436E-A801-E2D30DA0F775',14),('6FE0FD55-CAFB-4D57-8818-8A289511C4B5','48A00ADD-4561-49C8-819F-199EFAB11AAB',8),('6FE0FD55-CAFB-4D57-8818-8A289511C4B5','706937C9-300F-46EB-A629-DF7A8293FDEE',0),('6FE0FD55-CAFB-4D57-8818-8A289511C4B5','AAD8AEFD-AFC6-4A03-AC5E-D8AE77E857B9',2),('6FE0FD55-CAFB-4D57-8818-8A289511C4B5','B2DAD012-5746-47FC-A1C9-768A83D7D3FF',9),('6FE0FD55-CAFB-4D57-8818-8A289511C4B5','F4E80C17-D198-4B74-832E-B08F893C6A76',1),('7A3AD9E5-0A73-4CC2-8AD0-D32DC8728E35','B2DAD012-5746-47FC-A1C9-768A83D7D3FF',0),('834EA3A6-1F25-4F4B-961F-97ECFE351BB8','1D765BF5-8FD6-49AE-8D76-5E00564B7552',2),('834EA3A6-1F25-4F4B-961F-97ECFE351BB8','348A6B99-1201-46CE-98E8-E84553EF0493',1),('834EA3A6-1F25-4F4B-961F-97ECFE351BB8','3F80BD4B-25C2-436E-A801-E2D30DA0F775',12),('834EA3A6-1F25-4F4B-961F-97ECFE351BB8','48A00ADD-4561-49C8-819F-199EFAB11AAB',4),('834EA3A6-1F25-4F4B-961F-97ECFE351BB8','4D5F1210-7073-48AC-8248-AEA3F48A64C3',13),('834EA3A6-1F25-4F4B-961F-97ECFE351BB8','706937C9-300F-46EB-A629-DF7A8293FDEE',3),('834EA3A6-1F25-4F4B-961F-97ECFE351BB8','B2DAD012-5746-47FC-A1C9-768A83D7D3FF',6),('834EA3A6-1F25-4F4B-961F-97ECFE351BB8','F4E80C17-D198-4B74-832E-B08F893C6A76',0),('839C9288-A16A-4F23-A177-B9A7B206CEE7','48A00ADD-4561-49C8-819F-199EFAB11AAB',0),('839C9288-A16A-4F23-A177-B9A7B206CEE7','B2DAD012-5746-47FC-A1C9-768A83D7D3FF',1),('897C5865-15C8-4D6C-99C2-FD017910BA8B','48A00ADD-4561-49C8-819F-199EFAB11AAB',0),('975384BD-049E-43AB-BE18-B035D9F0AAA9','48A00ADD-4561-49C8-819F-199EFAB11AAB',0),('9A41CFD4-6410-4EE5-B301-2647AEFDBBF9','706937C9-300F-46EB-A629-DF7A8293FDEE',1),('9A41CFD4-6410-4EE5-B301-2647AEFDBBF9','B2DAD012-5746-47FC-A1C9-768A83D7D3FF',2),('9A41CFD4-6410-4EE5-B301-2647AEFDBBF9','F4E80C17-D198-4B74-832E-B08F893C6A76',0),('A2118E5F-B0E1-4B31-BB43-02379801F0E0','B2DAD012-5746-47FC-A1C9-768A83D7D3FF',0),('a3e19e66-736b-4a49-86a7-6985ba997b43','3f80bd4b-25c2-436e-a801-e2d30da0f775',0),('A3E19E66-736B-4A49-86A7-6985BA997B43','48A00ADD-4561-49C8-819F-199EFAB11AAB',1),('AE689972-DE5E-4D70-B6A3-84D2C72043C2','1D765BF5-8FD6-49AE-8D76-5E00564B7552',4),('AE689972-DE5E-4D70-B6A3-84D2C72043C2','348A6B99-1201-46CE-98E8-E84553EF0493',2),('AE689972-DE5E-4D70-B6A3-84D2C72043C2','48A00ADD-4561-49C8-819F-199EFAB11AAB',5),('AE689972-DE5E-4D70-B6A3-84D2C72043C2','706937C9-300F-46EB-A629-DF7A8293FDEE',0),('AE689972-DE5E-4D70-B6A3-84D2C72043C2','B2DAD012-5746-47FC-A1C9-768A83D7D3FF',6),('AE689972-DE5E-4D70-B6A3-84D2C72043C2','F4E80C17-D198-4B74-832E-B08F893C6A76',1),('BA5B9883-ED3E-46C5-B34B-9F6FB5EDCC63','348A6B99-1201-46CE-98E8-E84553EF0493',3),('BA5B9883-ED3E-46C5-B34B-9F6FB5EDCC63','706937C9-300F-46EB-A629-DF7A8293FDEE',1),('BA5B9883-ED3E-46C5-B34B-9F6FB5EDCC63','B2DAD012-5746-47FC-A1C9-768A83D7D3FF',0),('BA5B9883-ED3E-46C5-B34B-9F6FB5EDCC63','F4E80C17-D198-4B74-832E-B08F893C6A76',2),('BAD688AF-A272-41F8-965D-C0D461DB1ACE','B2DAD012-5746-47FC-A1C9-768A83D7D3FF',0),('C086F4A5-EBE9-46FC-AF6A-78ADF0389F26','48A00ADD-4561-49C8-819F-199EFAB11AAB',1),('C086F4A5-EBE9-46FC-AF6A-78ADF0389F26','85A43515-4F55-4503-9697-A2A553EB4B49',0),('C086F4A5-EBE9-46FC-AF6A-78ADF0389F26','B2DAD012-5746-47FC-A1C9-768A83D7D3FF',2),('C185DC38-40EF-4C8F-8739-8230150E6785','48A00ADD-4561-49C8-819F-199EFAB11AAB',0),('C185DC38-40EF-4C8F-8739-8230150E6785','B2DAD012-5746-47FC-A1C9-768A83D7D3FF',2),('C91415F0-0F2F-41C3-8FB1-976FA5C0A08A','3F80BD4B-25C2-436E-A801-E2D30DA0F775',3),('C91415F0-0F2F-41C3-8FB1-976FA5C0A08A','48A00ADD-4561-49C8-819F-199EFAB11AAB',1),('C91415F0-0F2F-41C3-8FB1-976FA5C0A08A','B2DAD012-5746-47FC-A1C9-768A83D7D3FF',4),('D1A93E03-1B04-482D-B24A-F7BEB09C0090','1D765BF5-8FD6-49AE-8D76-5E00564B7552',2),('D1A93E03-1B04-482D-B24A-F7BEB09C0090','348A6B99-1201-46CE-98E8-E84553EF0493',1),('D1A93E03-1B04-482D-B24A-F7BEB09C0090','706937C9-300F-46EB-A629-DF7A8293FDEE',3),('D1A93E03-1B04-482D-B24A-F7BEB09C0090','B2DAD012-5746-47FC-A1C9-768A83D7D3FF',4),('D1A93E03-1B04-482D-B24A-F7BEB09C0090','F4E80C17-D198-4B74-832E-B08F893C6A76',0),('d620f852-4b36-410a-a3a1-69f463767619','48a00add-4561-49c8-819f-199efab11aab',0),('d620f852-4b36-410a-a3a1-69f463767619','85a43515-4f55-4503-9697-a2a553eb4b49',2),('d620f852-4b36-410a-a3a1-69f463767619','b2dad012-5746-47fc-a1c9-768a83d7d3ff',1),('D974D577-1921-4410-BE22-D2C7E7663B06','1D765BF5-8FD6-49AE-8D76-5E00564B7552',3),('D974D577-1921-4410-BE22-D2C7E7663B06','348A6B99-1201-46CE-98E8-E84553EF0493',2),('D974D577-1921-4410-BE22-D2C7E7663B06','48A00ADD-4561-49C8-819F-199EFAB11AAB',4),('D974D577-1921-4410-BE22-D2C7E7663B06','706937C9-300F-46EB-A629-DF7A8293FDEE',0),('D974D577-1921-4410-BE22-D2C7E7663B06','B2DAD012-5746-47FC-A1C9-768A83D7D3FF',6),('D974D577-1921-4410-BE22-D2C7E7663B06','F4E80C17-D198-4B74-832E-B08F893C6A76',1),('e7b19bd0-4646-4c6a-af04-4cf085ac576f','48a00add-4561-49c8-819f-199efab11aab',1),('e7b19bd0-4646-4c6a-af04-4cf085ac576f','85a43515-4f55-4503-9697-a2a553eb4b49',0),('EE394836-E09B-4CC5-91A6-0589BEFC61CF','1D765BF5-8FD6-49AE-8D76-5E00564B7552',4),('EE394836-E09B-4CC5-91A6-0589BEFC61CF','348A6B99-1201-46CE-98E8-E84553EF0493',5),('EE394836-E09B-4CC5-91A6-0589BEFC61CF','3F80BD4B-25C2-436E-A801-E2D30DA0F775',14),('EE394836-E09B-4CC5-91A6-0589BEFC61CF','48A00ADD-4561-49C8-819F-199EFAB11AAB',8),('EE394836-E09B-4CC5-91A6-0589BEFC61CF','4D5F1210-7073-48AC-8248-AEA3F48A64C3',15),('EE394836-E09B-4CC5-91A6-0589BEFC61CF','706937C9-300F-46EB-A629-DF7A8293FDEE',0),('EE394836-E09B-4CC5-91A6-0589BEFC61CF','85A43515-4F55-4503-9697-A2A553EB4B49',7),('EE394836-E09B-4CC5-91A6-0589BEFC61CF','AAD8AEFD-AFC6-4A03-AC5E-D8AE77E857B9',13),('EE394836-E09B-4CC5-91A6-0589BEFC61CF','B2DAD012-5746-47FC-A1C9-768A83D7D3FF',9),('EE394836-E09B-4CC5-91A6-0589BEFC61CF','F4E80C17-D198-4B74-832E-B08F893C6A76',1),('F310F519-08C7-499A-A102-C02B7A35485C','48A00ADD-4561-49C8-819F-199EFAB11AAB',0),('F310F519-08C7-499A-A102-C02B7A35485C','B2DAD012-5746-47FC-A1C9-768A83D7D3FF',1),('F4950560-BF03-4C01-81C5-435C53587238','48A00ADD-4561-49C8-819F-199EFAB11AAB',1),('F4950560-BF03-4C01-81C5-435C53587238','B2DAD012-5746-47FC-A1C9-768A83D7D3FF',0),('FB74B158-4C1A-4653-8253-3107C8B38ADD','B2DAD012-5746-47FC-A1C9-768A83D7D3FF',0),('FDF667DF-38EA-48FD-98AB-01DC0D1AECCA','1D765BF5-8FD6-49AE-8D76-5E00564B7552',2),('FDF667DF-38EA-48FD-98AB-01DC0D1AECCA','348A6B99-1201-46CE-98E8-E84553EF0493',1),('FDF667DF-38EA-48FD-98AB-01DC0D1AECCA','706937C9-300F-46EB-A629-DF7A8293FDEE',3),('FDF667DF-38EA-48FD-98AB-01DC0D1AECCA','B2DAD012-5746-47FC-A1C9-768A83D7D3FF',4),('FDF667DF-38EA-48FD-98AB-01DC0D1AECCA','F4E80C17-D198-4B74-832E-B08F893C6A76',0);
/*!40000 ALTER TABLE `useragentprofileorders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `useragents`
--

DROP TABLE IF EXISTS `useragents`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `useragents` (
  `Id` char(36) COLLATE utf8mb4_swedish_ci NOT NULL,
  `Name` longtext COLLATE utf8mb4_swedish_ci,
  `Identifier` longtext COLLATE utf8mb4_swedish_ci,
  `MatchType` int(11) NOT NULL,
  `Image` longtext COLLATE utf8mb4_swedish_ci,
  `UserInterfaceLink` longtext COLLATE utf8mb4_swedish_ci,
  `Ax` tinyint(4) NOT NULL,
  `Width` int(11) NOT NULL,
  `Height` int(11) NOT NULL,
  `CreatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `CreatedOn` datetime NOT NULL,
  `UpdatedBy` longtext COLLATE utf8mb4_swedish_ci,
  `UpdatedOn` datetime NOT NULL,
  `Api` longtext COLLATE utf8mb4_swedish_ci,
  `Lines` int(11) NOT NULL,
  `Inputs` int(11) NOT NULL,
  `NrOfGpos` int(11) NOT NULL DEFAULT '0',
  `MaxInputDb` int(11) NOT NULL,
  `MinInputDb` int(11) NOT NULL,
  `Comment` longtext COLLATE utf8mb4_swedish_ci,
  `InputGainStep` int(11) NOT NULL,
  `GpoNames` longtext COLLATE utf8mb4_swedish_ci,
  `UserInterfaceIsOpen` tinyint(4) NOT NULL,
  `UseScrollbars` tinyint(4) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `useragents`
--

LOCK TABLES `useragents` WRITE;
/*!40000 ALTER TABLE `useragents` DISABLE KEYS */;
INSERT INTO `useragents` VALUES ('12c6d8c1-0ac0-46f3-b6fd-53ded04ab388','Janus WebRTC','Janus',0,'telefonare.png','http://[host]/',0,0,0,'root','2017-12-19 10:08:15','root','2018-03-28 14:23:56',NULL,0,0,0,0,0,'',3,NULL,1,0),('18B9E420-7918-44B9-B7E2-B4698E6035BD','Linphone','Linphone',0,'linphone-icon.png',NULL,0,0,0,'root','2014-09-29 12:29:49','root','2015-09-08 08:55:25',NULL,0,0,0,0,0,NULL,0,NULL,0,0),('1973A2C9-F7F2-43B3-A4F4-795502ADEDAF','BareSip Client v0.4.14','baresip v0.4.14',1,'baresip-s-icon.png','http://[host]/',0,0,0,'root','2015-09-09 11:52:39','root','2016-01-19 13:44:45',NULL,0,0,0,0,0,NULL,3,NULL,0,0),('2576E6D7-F490-4712-B07D-871DFC140A66','Quntum ST W3','QuantumW3/02.1.3',1,'icon-prodys-quantum_ST.png',NULL,1,0,0,'root','2017-03-14 16:11:13','root','2017-07-12 08:11:42','IkusNet',2,2,0,0,0,NULL,3,NULL,0,0),('28C8D173-67EE-40A4-8850-A28133A388C2','Blink','Blink',0,'blink-logo-text@2x.png',NULL,0,0,0,'root','2017-07-25 09:06:54','root','2017-07-25 09:08:17',NULL,0,0,0,0,0,'',3,NULL,0,0),('311dfec3-e460-4a45-84a8-26e866b04101','Zoiper','Z 5',0,'zoiper-logo_thumb.png',NULL,0,0,0,'root','2018-01-22 10:11:27','root','2018-01-22 10:13:06',NULL,0,0,0,0,0,'',3,NULL,0,0),('312B9589-10F4-4BDB-92EF-B3623A70A8B8','UMAC2-C','ME-UMAC2-C',1,'UMAC-CII.png',NULL,0,0,0,'root','2016-04-11 09:31:01','root','2017-12-15 12:48:00','Umac',0,0,0,0,0,NULL,3,NULL,0,0),('33D9F8EA-A1B7-4A98-9BE7-8655CCF7AED7','Nomada','Nomada',0,'Nomada-icon.png','http://[host]/',1,620,1000,'root','2014-09-29 12:32:58','root','2014-09-29 12:32:58',NULL,0,0,0,0,0,NULL,0,NULL,0,0),('391F4E31-9F6E-41C5-A787-1CB0F63035EF','Asterisk PBX','Asterisk PBX',0,'asteriskpbx-icon.png',NULL,0,0,0,'root','2014-09-29 12:27:09','root','2014-09-29 12:27:09',NULL,0,0,0,0,0,NULL,0,NULL,0,0),('4444026F-3854-4AAB-95DF-E182D5EB5B36','ProntoNet','ProntoNet',0,'Prontonet-icon.png','http://[host]/',1,620,1000,'root','2014-09-29 12:35:04','root','2017-03-02 11:50:08',NULL,0,0,0,0,0,'',0,NULL,0,0),('4982FD0A-21CB-4BE0-A96F-A6CB3CC7F3D7','Quantum ST+','QuantumST+',1,'icon-prodys-quantum_ST_Plus.png','http://[host]',0,494,287,'root','2017-06-20 12:27:04','root','2018-03-15 12:45:13',NULL,0,0,0,0,0,NULL,3,NULL,0,1),('4E53C5BE-748E-4FD0-9F4B-3461379CF1B4','QuantumOne','Nerea/',0,'icon-prodys-quantum.png','http://[host]/',0,550,350,'root','2017-05-30 11:40:12','root','2018-10-30 14:55:34','IkusNet',2,0,0,0,0,NULL,3,NULL,0,0),('50fd3e73-a4fd-48e7-9613-88369aeb866f','MicroSIP','MicroSIP',1,NULL,NULL,0,0,0,'root','2018-03-19 13:39:15','root','2018-03-19 13:39:15',NULL,0,0,0,0,0,NULL,3,NULL,0,0),('6FE0FD55-CAFB-4D57-8818-8A289511C4B5','Quantum','Quantum/3',1,'icon-prodys-quantum.png','http://[host]/',1,494,287,'root','2014-09-29 12:36:07','root','2018-10-30 10:26:24','IkusNet',2,5,2,0,-100,NULL,0,'Ut 0, Ut 1, Ut 2, Ut 3',0,0),('7A3AD9E5-0A73-4CC2-8AD0-D32DC8728E35','WebRTC','webrtc',0,'webrtc-icon-90x15.png','',0,0,0,'root','2014-09-29 12:40:41','root','2016-09-08 12:04:10','',0,0,0,0,0,'',0,'',0,0),('834EA3A6-1F25-4F4B-961F-97ECFE351BB8','Quantum ST','QuantumST/',1,'QuantumST_Transp.png','http://[host]/',1,494,287,'root','2014-09-29 12:36:49','root','2018-09-21 13:04:31','IkusNet',1,1,0,0,-100,NULL,0,NULL,0,0),('839C9288-A16A-4F23-A177-B9A7B206CEE7','Acrobits','Acrobits',0,NULL,NULL,0,0,0,'root','2017-07-05 09:12:32','root','2017-07-05 09:13:02',NULL,0,0,0,0,0,NULL,0,NULL,0,0),('897C5865-15C8-4D6C-99C2-FD017910BA8B','Baresip Client v0.4.17','baresip v0.4.17',0,'baresip-s-icon.png','http://[host]/',0,0,0,'root','2016-04-20 08:45:09','root','2016-04-28 10:57:01',NULL,0,0,0,0,0,NULL,3,NULL,0,0),('975384BD-049E-43AB-BE18-B035D9F0AAA9','Baresip Client v0.4.15','baresip v0.4.15',0,'baresip-s-icon.png','http://[host]/',0,0,0,'root','2015-10-16 10:55:02','root','2016-01-19 13:44:34',NULL,0,0,0,0,0,NULL,3,NULL,1,1),('9A41CFD4-6410-4EE5-B301-2647AEFDBBF9','Mayah','MAYAH',0,'mayah-icon.png',NULL,0,0,0,'root','2014-09-29 12:30:35','root','2015-06-15 17:20:16',NULL,0,0,0,0,0,NULL,0,NULL,0,0),('A2118E5F-B0E1-4B31-BB43-02379801F0E0','Comrex','Comrex',0,'comrex-icon.png',NULL,0,0,0,'root','2014-09-29 12:29:26','root','2014-09-29 12:29:26',NULL,0,0,0,0,0,NULL,0,NULL,0,0),('A3E19E66-736B-4A49-86A7-6985BA997B43','BareSip Client v0.5.10','baresip v0.5.10',1,'Aloe-Project-Logo.png','http://[host]/',0,850,500,'root','2016-10-17 13:05:40','root','2018-11-21 13:33:19','BaresipRest',0,1,0,100,0,NULL,3,NULL,1,0),('AE689972-DE5E-4D70-B6A3-84D2C72043C2','UMAC','ME-UMAC-M/',1,'me-umac-m-icon.png',NULL,0,0,0,'root','2014-09-29 12:38:23','root','2017-06-12 15:52:11','Umac',0,0,0,0,0,NULL,0,NULL,0,0),('BA5B9883-ED3E-46C5-B34B-9F6FB5EDCC63','Snom','snom',0,'snom-icon.png','http://[host]/',0,0,0,'root','2014-09-29 12:37:28','root','2015-06-03 07:13:17',NULL,0,0,0,0,0,NULL,0,NULL,0,0),('BAD688AF-A272-41F8-965D-C0D461DB1ACE','Mayah Mobi','Mobi',0,'mobi-icon.png',NULL,0,0,0,'root','2014-09-29 12:31:08','root','2014-09-29 12:41:47',NULL,0,0,0,0,0,NULL,0,NULL,0,0),('C086F4A5-EBE9-46FC-AF6A-78ADF0389F26','UMAC-C','ME-UMAC-C/',0,'me-umac-c-icon.png',NULL,0,0,0,'root','2014-09-29 12:39:03','root','2017-06-13 14:35:44','Umac',0,0,0,0,0,NULL,0,NULL,0,0),('C185DC38-40EF-4C8F-8739-8230150E6785','PCNet','PCNet',0,'prodys-pcnet-icon-2.png',NULL,0,0,0,'root','2014-09-29 12:34:13','root','2014-10-01 13:52:43',NULL,0,0,0,0,0,NULL,0,NULL,0,0),('C65FC719-238B-46B0-B5EA-4692E96453FD','IP-hybrid','IP-hybrid',0,'iphybridalfa.png','http://[host]/',0,0,0,'root','2017-08-10 16:29:40','root','2017-08-14 10:39:32',NULL,0,0,0,0,0,'',3,NULL,0,0),('C91415F0-0F2F-41C3-8FB1-976FA5C0A08A','Baresip Client','baresip',1,'baresip-s-icon.png','http://[host]/',0,850,460,'root','2015-03-03 13:37:31','root','2018-03-29 19:12:28','BaresipRest',0,1,0,100,0,NULL,3,NULL,1,0),('D1A93E03-1B04-482D-B24A-F7BEB09C0090','Uppgradera firmware','Nomada IP XL v6.6.8',0,'uppgraderafw-icon-95x10.gif',NULL,0,0,0,'root','2014-09-29 12:39:47','root','2014-09-29 12:40:09',NULL,0,0,0,0,0,NULL,0,NULL,0,0),('d620f852-4b36-410a-a3a1-69f463767619','Luci Live MAC ','LuciLiveClient_2.5.9_MAC',0,'LuciLive-logo.png',NULL,0,0,0,'root','2018-01-31 13:52:58','root','2018-01-31 13:53:29',NULL,0,0,0,0,0,NULL,3,NULL,0,0),('D974D577-1921-4410-BE22-D2C7E7663B06','ME-UMAC2 - C','ME-UMAC2-C/1.3',1,'UMAC-CII.png',NULL,0,0,0,'root','2016-04-21 17:50:16','root','2016-11-08 12:00:17','Umac',0,0,0,0,0,NULL,3,NULL,0,0),('e7b19bd0-4646-4c6a-af04-4cf085ac576f','Luci Live Windows','LuciLiveClient',0,'LuciLive-logo.png',NULL,0,0,0,'root','2018-03-19 13:28:51','root','2018-03-19 14:42:18',NULL,0,0,0,0,0,NULL,3,NULL,0,0),('EE394836-E09B-4CC5-91A6-0589BEFC61CF','UmacMK2','ME-UMAC2-M',1,'me-umac-m2-icon.png',NULL,0,0,0,'root','2015-05-20 10:06:45','root','2017-12-15 09:00:20','Umac',0,0,0,0,0,NULL,3,NULL,0,0),('F310F519-08C7-499A-A102-C02B7A35485C','Bria','Bria',0,'bria.png','http://[host]/',0,0,0,'root','2014-09-29 12:29:02','root','2018-03-07 14:20:33',NULL,0,0,0,0,0,NULL,0,NULL,0,0),('F4950560-BF03-4C01-81C5-435C53587238','LuciLive','LUCI',0,'LuciLive-logo.png',NULL,0,0,0,'root','2014-09-29 12:30:08','root','2018-01-31 13:48:35',NULL,0,0,0,0,0,NULL,0,NULL,0,0),('FB74B158-4C1A-4653-8253-3107C8B38ADD','Media5-fone','Media5-fone',0,'media5-icon.png',NULL,0,0,0,'root','2014-09-29 12:31:33','root','2014-09-29 12:41:57',NULL,0,0,0,0,0,NULL,0,NULL,0,0),('FDF667DF-38EA-48FD-98AB-01DC0D1AECCA','Nereus','Nereus',0,'Nereus-icon.png','http://[host]/',1,620,1000,'root','2014-09-29 12:32:05','root','2017-07-06 12:29:55',NULL,0,0,0,0,0,NULL,0,NULL,0,0);
/*!40000 ALTER TABLE `useragents` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-04-17 16:32:18
