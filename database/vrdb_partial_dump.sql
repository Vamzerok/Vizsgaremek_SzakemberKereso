-- phpMyAdmin SQL Dump
-- version 5.2.3
-- https://www.phpmyadmin.net/
--
-- Host: db
-- Generation Time: Apr 27, 2026 at 11:54 AM
-- Server version: 8.0.46
-- PHP Version: 8.3.26

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `vrdb`
--
CREATE DATABASE IF NOT EXISTS `vrdb` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;
USE `vrdb`;

-- --------------------------------------------------------

--
-- Table structure for table `experts`
--

CREATE TABLE `experts` (
  `user_id` int NOT NULL,
  `biography` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `company_email` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `company_phone_number` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `work_location_id` int NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `experts`
--

INSERT INTO `experts` (`user_id`, `biography`, `company_email`, `company_phone_number`, `work_location_id`) VALUES
(7, 'Mekk Elek vagyok, szombathelyi szakember. Kőműves, lakatos, kovács és kályhásmesteri munkákat vállalok. Harminc éve dolgozom ezen a területen. Az ügyfeleimnek, a munkáim eredményeiről, megosztott véleményen vannak, nem is értem miért.', 'mekk.elek@ezermester.hu', '+36 30 123 4567', 9),
(8, 'Csavaros Csaba vagyok. Vízvezeték-, gáz- és villanyszerelési munkákat vállalok. Gyors és megbízható munkavégzéssel.', 'csavaros.csaba@gmail.com', '+36 70 234 5678', 11),
(9, 'Kalapács Karcsi, kőműves és ács. Tapolcán és környékén vállalok kisebb nagyobb felújítási munkákat.', 'kalapacs.karcsi@ezermester.hu', '+36 20 345 6789', 15),
(10, 'Flexes Feri, székesfehérvári festő és burkoló. Minőségi belső és külső festési, illetve burkolási munkák.', 'flexes.feri@gmail.com', '+36 30 456 7890', 12),
(11, 'Csőtörő Csöpi debreceni vízszerelő. Gyors duguláselhárítás, csőjavítás, szerelvénycserék.', 'csotoro.csopi@vizman.hu', '+36 70 567 8901', 13),
(12, 'Szigszalagos Sanyi, zalahalápi tetőfedő és bútorasztalos. Megbízható munkavégzés kedvező áron.', 'szigszalagos.sanyi@gmail.com', '+36 20 678 9012', 14);

-- --------------------------------------------------------

--
-- Table structure for table `expert_specialties`
--

CREATE TABLE `expert_specialties` (
  `id` int NOT NULL,
  `expert_id` int NOT NULL,
  `occupation_id` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `expert_specialties`
--

INSERT INTO `expert_specialties` (`id`, `expert_id`, `occupation_id`) VALUES
(2, 7, 7321),
(3, 7, 7326),
(1, 7, 7511),
(4, 7, 7537),
(5, 8, 7521),
(6, 8, 7524),
(7, 9, 7511),
(8, 9, 7513),
(10, 10, 7534),
(9, 10, 7535),
(11, 11, 7521),
(13, 12, 7223),
(12, 12, 7532);

-- --------------------------------------------------------

--
-- Table structure for table `jobs`
--

CREATE TABLE `jobs` (
  `job_id` int NOT NULL,
  `location_id` int NOT NULL,
  `service_id` int NOT NULL,
  `initiating_user_id` int NOT NULL,
  `pricing_id` int DEFAULT NULL,
  `status` int NOT NULL,
  `rating` float DEFAULT NULL,
  `description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `title` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT '',
  `cancelled_from_status` int DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `jobs`
--

INSERT INTO `jobs` (`job_id`, `location_id`, `service_id`, `initiating_user_id`, `pricing_id`, `status`, `rating`, `description`, `title`, `cancelled_from_status`) VALUES
(1, 14, 16, 3, 22, 3, 4, 'A fürdőszobai lefolyó és a WC is eldugult, sürgős segítségre van szükség.', 'Dugulás a fürdőszobában', NULL),
(2, 8, 13, 3, 23, 2, NULL, 'A nappali falait szeretném kifesteni fehérre. A falak összterülete kb. 45 m2.', 'Nappali kifestése', NULL),
(3, 8, 15, 3, 24, 1, NULL, 'A fürdőszoba padlóját kell csempézni, a meglévő csempéket már felszedtem. Kb. 6 m^2-es területről beszélünk.', 'Fürdőszoba burkolása', NULL),
(4, 8, 6, 3, NULL, 0, NULL, 'A konyhai mosdó alatt lévő három sarokszelep tömít, ezeket cserélni kéne.', 'Három sarokszelep csere', NULL),
(5, 14, 19, 3, NULL, 4, NULL, 'A tető egy kisebb részén cserepek hiányoznak. Az eső teljesen szétáztatja a tetőszerkezetet. Gyors segítségre lenne szükségem.', 'Tető javítás', 0),
(6, 8, 1, 1, NULL, 0, NULL, 'A nappali falán több helyen repedés látható a vakolatban, kb. 3–4 repedés összesen.', 'Repedések a nappaliban', NULL),
(7, 9, 3, 2, 25, 1, NULL, 'A lakás főbejárati ajtajának zárbetétje elromlott, kulcsra már nem nyit.', 'Főbejárat zárbetét csere', NULL),
(8, 9, 5, 5, 26, 2, NULL, 'Régi falazott kandallónk felújíttatást igényel: a vakolat omlik, a tömítés megrepedt.', 'Kandalló felújítása', NULL),
(9, 13, 1, 4, 27, 3, 3, 'A garázs melletti külső fal vakolata több helyen lepergett, javítást igényel.', 'Garázsajtó melletti fal javítása', NULL),
(10, 13, 4, 6, 28, 4, NULL, 'A kerti kerítés kapuja nem nyílik megfelelően, néhány hegesztési ponton utánmunkálat szükséges.', 'Kerti kerítés javítás', 1),
(11, 12, 11, 1, NULL, 0, NULL, 'A ház mögötti kertben egy kb. 12 m^2-es fa teraszt szeretnék építtetni.', 'Fa terasz építése a kertbe', NULL),
(12, 10, 7, 2, 29, 3, 5, 'A régi vízórát cserélni kell, már jó ideje nem mér pontosan.', 'Vízóra csere', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `occupations`
--

CREATE TABLE `occupations` (
  `feor_id` int NOT NULL,
  `name` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `description` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `occupations`
--

INSERT INTO `occupations` (`feor_id`, `name`, `description`) VALUES
(7223, 'Bútorasztalos', ''),
(7321, 'Lakatos', ''),
(7326, 'Kovács', ''),
(7511, 'Kőműves', ''),
(7513, 'Ács', ''),
(7521, 'Vezeték- és csőhálózat-szerelő (víz, gáz, fűtés)', ''),
(7524, 'Épületvillamossági szerelő, villanyszerelő', ''),
(7532, 'Tetőfedő', ''),
(7534, 'Burkoló', ''),
(7535, 'Festő és mázoló', ''),
(7537, 'Kályha- és kandallóépítő', '');

-- --------------------------------------------------------

--
-- Table structure for table `pricing`
--

CREATE TABLE `pricing` (
  `pricing_id` int NOT NULL,
  `pricing_type` int NOT NULL,
  `fixed_price` decimal(18,2) NOT NULL,
  `unit_price` decimal(18,2) DEFAULT NULL,
  `unit_name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `pricing`
--

INSERT INTO `pricing` (`pricing_id`, `pricing_type`, `fixed_price`, `unit_price`, `unit_name`) VALUES
(1, 1, 15000.00, 2000.00, 'm^2'),
(2, 1, 25000.00, 8000.00, 'm^2'),
(3, 0, 8000.00, NULL, NULL),
(4, 1, 10000.00, 5000.00, 'm'),
(5, 0, 50000.00, NULL, NULL),
(6, 1, 2000.00, 1000.00, 'sarokszelep'),
(7, 0, 15000.00, NULL, NULL),
(8, 1, 8000.00, 4000.00, 'db'),
(9, 1, 2500.00, 800.00, 'db'),
(10, 1, 5000.00, 1500.00, 'm^2'),
(11, 1, 30000.00, 15000.00, 'm^2'),
(12, 0, 20000.00, NULL, NULL),
(13, 1, 10000.00, 1000.00, 'm^2'),
(14, 1, 15000.00, 1500.00, 'm^2'),
(15, 1, 5000.00, 2500.00, 'm^2'),
(16, 1, 5000.00, 2000.00, 'alkalom'),
(17, 0, 20000.00, NULL, NULL),
(18, 1, 8000.00, 3000.00, 'óra'),
(19, 1, 12000.00, 500.00, 'cserép'),
(20, 0, 20000.00, NULL, NULL),
(21, 1, 3000.00, 1000.00, 'db'),
(22, 0, 7000.00, NULL, NULL),
(23, 1, 10000.00, 1000.00, 'm^2'),
(24, 1, 5000.00, 2500.00, 'm^2'),
(25, 0, 8000.00, NULL, NULL),
(26, 0, 50000.00, NULL, NULL),
(27, 1, 15000.00, 2000.00, 'm2'),
(28, 1, 10000.00, 5000.00, 'óra'),
(29, 0, 15000.00, NULL, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `residential_addresses`
--

CREATE TABLE `residential_addresses` (
  `residential_address_id` int NOT NULL,
  `settlement_id` int NOT NULL,
  `street_name` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `building_number` int NOT NULL DEFAULT '0',
  `public_area_type` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `residential_addresses`
--

INSERT INTO `residential_addresses` (`residential_address_id`, `settlement_id`, `street_name`, `building_number`, `public_area_type`) VALUES
(1, 1, 'Deák Ferenc', 12, 'utca'),
(2, 1, 'Petőfi Sándor', 3, 'utca'),
(3, 2, 'Kossuth Lajos', 7, 'tér'),
(4, 3, 'Dózsa György', 21, 'út'),
(5, 4, 'Rákóczi Ferenc', 5, 'utca'),
(6, 5, 'Szabadság', 14, 'tér'),
(7, 9, 'Zrínyi Ilona', 12, 'utca'),
(8, 9, 'Kossuth Lajos', 5, 'utca'),
(9, 12, 'Ady Endre', 15, 'utca'),
(10, 13, 'Bem', 18, 'tér'),
(11, 14, 'Fő', 3, 'utca'),
(12, 15, 'Batsányi János', 2, 'utca'),
(13, 11, 'Petőfi Sándor', 8, 'utca'),
(14, 9, 'Arany János', 12, 'utca'),
(15, 12, 'Vár', 8, 'körút');

-- --------------------------------------------------------

--
-- Table structure for table `roles`
--

CREATE TABLE `roles` (
  `Id` int NOT NULL,
  `Name` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `roles`
--

INSERT INTO `roles` (`Id`, `Name`, `NormalizedName`, `ConcurrencyStamp`) VALUES
(1, 'Generic', 'GENERIC', NULL),
(2, 'Expert', 'EXPERT', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `services`
--

CREATE TABLE `services` (
  `service_id` int NOT NULL,
  `name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `description` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `expert_specialties_id` int NOT NULL,
  `pricing_id` int NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `services`
--

INSERT INTO `services` (`service_id`, `name`, `description`, `expert_specialties_id`, `pricing_id`) VALUES
(1, 'Falrepedések javítása', 'Vakolati és falrepedések javítása beltéren és kültéren. A pontos árajánlathoz kérem adja meg a javítandó felület közelítő területét m^2-ben.', 1, 1),
(2, 'Alapozó falazat készítése', 'Válaszfalak és alapozó falazatok elkészítése téglából vagy gázbetonból. Az anyagköltség külön számolódik. Kérem adja meg a tervezett falfelület területét m^2-ben.', 1, 2),
(3, 'Zárbetét csere', 'Ajtózárak és zárbetétek cseréje. Az ár tartalmazza a munkadíjat; az alkatrész ára külön számolódik.', 2, 3),
(4, 'Kapu- és kerítésjavítás', 'Fém kapuk, kerítések hegesztéssel elvégzett javítása, megerősítése. Kérem adja meg a javítandó kerítés hosszát méterben.', 2, 4),
(5, 'Kandalló felújítása', 'Meglévő kandallószerkezetek felújítása, tömítése, vakolása. Az ár a munkadíjat tartalmazza; anyagköltség külön megállapodás szerint.', 4, 5),
(6, 'Sarokszelep csere', 'Elöregedett sarokszelepek gyors cseréje. Kérem adja meg a cserélendő szelepek számát.', 5, 6),
(7, 'Vízóra csere', 'Elavult vízórák cseréje és hitelesítése. Tartalmazza a helyszíni felmérést és a tömítettség ellenőrzését.', 5, 7),
(8, 'Radiátor beüzemelés', 'Radiátorok bekötése, légtelenítése és nyomáspróba elvégzése. Kérem adja meg a beüzemelni kívánt radiátorok számát.', 5, 8),
(9, 'Konnektorcsere', 'Meglévő konnektorok cseréje, villamos folytonosság ellenőrzése. Kérem adja meg a cserélendő konnektorok számát.', 6, 9),
(10, 'Vakolat javítása', 'Beltéri és kültéri vakolathibák javítása. Kérem adja meg a javítandó felület közelítő területét m^2-ben.', 7, 10),
(11, 'Terasz építése', 'Fa szerkezetű terasz tervezése és megépítése. Az anyagköltség külön kalkulálandó. Kérem adja meg a tervezett terasz alapterületét m^2-ben.', 8, 11),
(12, 'Falépcső javítása', 'Nyikorgó, törött vagy korhadt lépcsőfokok javítása, cseréje. Tartalmazza a helyszíni felmérést és az elvégzett munkát.', 8, 12),
(13, 'Beltéri falak festése', 'Szobák és folyosók beltéri falainak festése diszperziós festékkel. Kérem adja meg a festendő falfelület összesített területét m^2-ben.', 9, 13),
(14, 'Homlokzatfestés', 'Épületek külső homlokzatának festése. Az alapozás ára külön megállapodás szerint. Kérem adja meg a homlokzatfelület közelítő területét m^2-ben.', 9, 14),
(15, 'Aljzatcsempe lerakása', 'Padlócsempe lerakása ragasztós technológiával, fugázással. Az anyagköltség külön számolódik. Kérem adja meg a burkolandó terület m^2-es méretét.', 10, 15),
(16, 'Duguláselhárítás', 'Lefolyó-, szennyvíz- és csatornavezeték-dugulások elhárítása. Ismételt beavatkozás esetén az egységár ismételten érvényes.', 11, 16),
(17, 'Mosdó/WC csere', 'Mosdó, WC-csésze cseréje. Tartalmazza a leszedést, az újbeüzemelést és a tömítettség ellenőrzését. A szerelvény ára külön számolódik.', 11, 17),
(18, 'Vízvezeték javítás', 'Csőtörések és szivárgások javítása. Kérem adja meg a becsült munkaidőt és a jelenség rövid leírását.', 11, 18),
(19, 'Tetőcserép csere', 'Törött, megrepedezett vagy eltolódott tetőcserepek cseréje. Az anyagköltség külön számítandó. Kérem adja meg a cserélendő cserepek becsült számát.', 12, 19),
(20, 'Szekrény javítás', 'Beépített és szabadon álló szekrények ajtajinak, fiókjainak javítása. Tartalmazza a helyszíni felmérést és az elvégzett munkát.', 13, 20),
(21, 'Polc felszerelése', 'Falra rögzített polcok felszerelése. Kérem adja meg a felszerelendő polcok számát.', 13, 21);

-- --------------------------------------------------------

--
-- Table structure for table `settlements`
--

CREATE TABLE `settlements` (
  `settlement_id` int NOT NULL,
  `postal_code` int NOT NULL,
  `name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `county_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `settlements`
--

INSERT INTO `settlements` (`settlement_id`, `postal_code`, `name`, `county_name`) VALUES
(1, 8127, 'Aba', 'Fejér'),
(2, 5241, 'Abádszalók', 'Jász-Nagykun-Szolnok'),
(3, 7678, 'Abaliget', 'Baranya'),
(4, 3261, 'Abasár', 'Heves'),
(5, 3882, 'Abaújalpár', 'Borsod-Abaúj-Zemplén'),
(6, 3882, 'Abaújkér', 'Borsod-Abaúj-Zemplén'),
(7, 3815, 'Abaújlak', 'Borsod-Abaúj-Zemplén'),
(8, 3881, 'Abaújszántó', 'Borsod-Abaúj-Zemplén'),
(9, 9700, 'Szombathely', 'Vas'),
(10, 9600, 'Sárvár', 'Vas'),
(11, 9798, 'Ják', 'Vas'),
(12, 8019, 'Székesfehérvár', 'Fejér'),
(13, 4042, 'Debrecen', 'Hajdú-Bihar'),
(14, 8308, 'Zalahaláp', 'Veszprém'),
(15, 8300, 'Tapolca', 'Veszprém');

-- --------------------------------------------------------

--
-- Table structure for table `time_intervals`
--

CREATE TABLE `time_intervals` (
  `time_interval_id` int NOT NULL,
  `date` date NOT NULL,
  `start_time` time(6) NOT NULL,
  `end_time` time(6) NOT NULL,
  `job_id` int NOT NULL,
  `type` int NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `time_intervals`
--

INSERT INTO `time_intervals` (`time_interval_id`, `date`, `start_time`, `end_time`, `job_id`, `type`) VALUES
(1, '2025-05-15', '08:00:00.000000', '12:00:00.000000', 4, 0),
(2, '2025-05-16', '08:00:00.000000', '12:00:00.000000', 4, 0),
(3, '2025-04-20', '10:00:00.000000', '16:00:00.000000', 5, 0),
(4, '2025-05-10', '09:00:00.000000', '17:00:00.000000', 6, 0),
(5, '2025-05-12', '09:00:00.000000', '17:00:00.000000', 6, 0),
(6, '2025-06-01', '07:00:00.000000', '16:00:00.000000', 11, 0),
(7, '2025-06-02', '07:00:00.000000', '16:00:00.000000', 11, 0),
(8, '2025-06-03', '07:00:00.000000', '16:00:00.000000', 11, 0),
(9, '2025-03-10', '08:00:00.000000', '12:00:00.000000', 1, 0),
(10, '2025-03-11', '09:00:00.000000', '17:00:00.000000', 1, 0),
(11, '2025-03-10', '09:00:00.000000', '11:00:00.000000', 1, 1),
(12, '2025-04-01', '07:00:00.000000', '15:00:00.000000', 2, 0),
(13, '2025-04-02', '07:00:00.000000', '15:00:00.000000', 2, 0),
(14, '2025-04-03', '07:00:00.000000', '15:00:00.000000', 2, 0),
(15, '2025-04-02', '08:00:00.000000', '14:00:00.000000', 2, 1),
(16, '2025-05-05', '08:00:00.000000', '17:00:00.000000', 3, 0),
(17, '2025-05-06', '08:00:00.000000', '17:00:00.000000', 3, 0),
(18, '2025-05-05', '09:00:00.000000', '13:00:00.000000', 3, 1),
(19, '2025-05-08', '08:00:00.000000', '12:00:00.000000', 7, 0),
(20, '2025-05-09', '08:00:00.000000', '12:00:00.000000', 7, 0),
(21, '2025-05-08', '09:00:00.000000', '11:00:00.000000', 7, 1),
(22, '2025-05-20', '08:00:00.000000', '14:00:00.000000', 8, 0),
(23, '2025-05-21', '08:00:00.000000', '14:00:00.000000', 8, 0),
(24, '2025-05-20', '09:00:00.000000', '13:00:00.000000', 8, 1),
(25, '2025-02-15', '08:00:00.000000', '17:00:00.000000', 9, 0),
(26, '2025-02-15', '09:00:00.000000', '15:00:00.000000', 9, 1),
(27, '2025-04-01', '10:00:00.000000', '16:00:00.000000', 10, 0),
(28, '2025-04-01', '11:00:00.000000', '15:00:00.000000', 10, 1),
(29, '2025-01-20', '08:00:00.000000', '12:00:00.000000', 12, 0),
(30, '2025-01-20', '09:00:00.000000', '11:00:00.000000', 12, 1);

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `Id` int NOT NULL,
  `FirstName` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `LastName` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `UserName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedUserName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Email` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedEmail` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `SecurityStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `PhoneNumber` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`Id`, `FirstName`, `LastName`, `UserName`, `NormalizedUserName`, `Email`, `NormalizedEmail`, `EmailConfirmed`, `PasswordHash`, `SecurityStamp`, `ConcurrencyStamp`, `PhoneNumber`, `PhoneNumberConfirmed`, `TwoFactorEnabled`, `LockoutEnd`, `LockoutEnabled`, `AccessFailedCount`) VALUES
(1, 'Gergő', 'Verhás', 'verhas.gergo@hbsz.edu.hu', 'VERHAS.GERGO@HBSZ.EDU.HU', 'verhas.gergo@hbsz.edu.hu', 'VERHAS.GERGO@HBSZ.EDU.HU', 0, 'AQAAAAIAAYagAAAAECo7rScMIILdbcDHm/tdcEqiAfPN9U/cdEIwm/RECpNh3Obzg/JS69sczJrACWDxmQ==', 'AAQO56RXBV5BFHPF2RNXM4BVLW6TLHTW', '8544e54a-97a0-4fce-96aa-28cbd43a7f59', NULL, 0, 0, NULL, 1, 0),
(2, 'Dávid', 'Varga', 'varga.david@hbsz.edu.hu', 'VARGA.DAVID@HBSZ.EDU.HU', 'varga.david@hbsz.edu.hu', 'VARGA.DAVID@HBSZ.EDU.HU', 0, 'AQAAAAIAAYagAAAAEOp/SPXsZXy269hgImuXTCGhZkbpqv91QpFVciKaEHYunrYfWH7MRyeToae52FYbbg==', 'QN2M53LGTU3NG6JLULKS3JJ3INTXIUEB', '5d71280d-b447-4a11-8ca0-5611955f571e', NULL, 0, 0, NULL, 1, 0),
(3, 'Bálint', 'Császár', 'csaszar.balint@hbsz.edu.hu', 'CSASZAR.BALINT@HBSZ.EDU.HU', 'csaszar.balint@hbsz.edu.hu', 'CSASZAR.BALINT@HBSZ.EDU.HU', 0, 'AQAAAAIAAYagAAAAEOQTXp89UVD6Mk57bhSqLuFQe/V0QBmGuyIYaOAnfT2zvcp2U2WD6I6eADBIfIHwyA==', 'PWL44WT6ZWXP2U65NVMLT6ZRDYASKW5J', '885316bf-49bb-456a-b903-253764667260', NULL, 0, 0, NULL, 1, 0),
(4, 'Elek', 'Teszt', 'teszt.elek@gmail.com', 'TESZT.ELEK@GMAIL.COM', 'teszt.elek@gmail.com', 'TESZT.ELEK@GMAIL.COM', 0, 'AQAAAAIAAYagAAAAECZAj3n62nEaMoRlPBo/M59bL4kbu5L8h44sy3nIoWV7UxJb4WgpQbKnHK2IVL5MBg==', 'B3TBMC2ISXTD3BXZVF3ZVVSOMXISRW3B', '798b074c-49ec-469d-9a88-039074f605a4', NULL, 0, 0, NULL, 1, 0),
(5, 'Péter', 'Horváth', 'horvath.peter@gmail.com', 'HORVATH.PETER@GMAIL.COM', 'horvath.peter@gmail.com', 'HORVATH.PETER@GMAIL.COM', 0, 'AQAAAAIAAYagAAAAELZONn/LfdXevf7jG2IVgfDXh9jseb74gS6X/wcUW1mydetc0WzY0Sjv+KgX8b+Zgg==', 'TX2AQQQ2QSFPGM7K4OQHRLLQVB6PYMYT', 'e81ae9e4-8329-4c3d-8b72-34f89076cb57', NULL, 0, 0, NULL, 1, 0),
(6, 'Andrea', 'Molnár', 'molnar.andrea@gmail.com', 'MOLNAR.ANDREA@GMAIL.COM', 'molnar.andrea@gmail.com', 'MOLNAR.ANDREA@GMAIL.COM', 0, 'AQAAAAIAAYagAAAAEGuVVndevRg7AzCJGmnjKSQqDgONYlAzdtvxOvOAKAQuvesoNBKZrb1pwOGLgmoXbw==', '2PMHL72FHIF3CCHWIV5CRXKQNRK6J6IG', 'f37ad7be-34a6-4953-99d8-eedf129975da', NULL, 0, 0, NULL, 1, 0),
(7, 'Elek', 'Mekk', 'mekk.elek@ezermester.hu', 'MEKK.ELEK@EZERMESTER.HU', 'mekk.elek@ezermester.hu', 'MEKK.ELEK@EZERMESTER.HU', 0, 'AQAAAAIAAYagAAAAEGZRhD+vlDxvPdhPdcwPakuN3mIT4CAiaCyWcSWrELB5oZxJExQ1ORYqYCpjfR6lUA==', 'CVSKLSBIQFEDFNUNQVXBA5RPP5O7S6ZD', 'f552eb30-60d0-4b36-9d2c-bbde078d4c27', NULL, 0, 0, NULL, 1, 0),
(8, 'Csaba', 'Csavaros', 'csavaros.csaba@gmail.com', 'CSAVAROS.CSABA@GMAIL.COM', 'csavaros.csaba@gmail.com', 'CSAVAROS.CSABA@GMAIL.COM', 0, 'AQAAAAIAAYagAAAAEMLfwwA/elt9LYfb5s3/47E4bfUjgco7nHrcyRUauJLnLmVtQh64MW0BZyeI4S5r+w==', 'KKFF2SGBGVLFJGO47DNT2SVVOUQCYUBP', '5e086483-4020-465f-9767-40322e146435', NULL, 0, 0, NULL, 1, 0),
(9, 'Karcsi', 'Kalapács', 'kalapacs.karcsi@ezermester.hu', 'KALAPACS.KARCSI@EZERMESTER.HU', 'kalapacs.karcsi@ezermester.hu', 'KALAPACS.KARCSI@EZERMESTER.HU', 0, 'AQAAAAIAAYagAAAAEFRdAoj9JTU/pSFRjHqkucXtlclCAzdFVDE1l3nnBEhmi6Gx3Q3uCNN00lN9ZekCeA==', 'OVGIPGJKJGAL2MHMJ5FFWTTLBOQK4CIM', '59268202-2bf9-41b8-89ea-aa7f5de56c8f', NULL, 0, 0, NULL, 1, 0),
(10, 'Feri', 'Flexes', 'flexes.feri@gmail.com', 'FLEXES.FERI@GMAIL.COM', 'flexes.feri@gmail.com', 'FLEXES.FERI@GMAIL.COM', 0, 'AQAAAAIAAYagAAAAEHFzJjUdJ8PWucxr59jkBDxReFIR4ZMXS/g5U+AqFm023SxWrNpvgd3m/7HLSd5RQQ==', 'Z2XLQ4TIKSAKNOZOZTWIDS7JD3UPXKFD', '3efb6ff6-72fb-4e1f-bb63-bf5c83f58a1e', NULL, 0, 0, NULL, 1, 0),
(11, 'Csöpi', 'Csőtörő', 'csotoro.csopi@vizman.hu', 'CSOTORO.CSOPI@VIZMAN.HU', 'csotoro.csopi@vizman.hu', 'CSOTORO.CSOPI@VIZMAN.HU', 0, 'AQAAAAIAAYagAAAAEKys1G64UlcnRGx4FxRLIIOnJKJar2KugY6ueHEi46jUHA23EQ05tVKVdxyg3wttTw==', '75OO3CPADR4Y52SC4OREXFM3QDWUR6FS', 'a690c4aa-4c5b-428b-9525-b11918e1c540', NULL, 0, 0, NULL, 1, 0),
(12, 'Sanyi', 'Szigszalagos', 'szigszalagos.sanyi@gmail.com', 'SZIGSZALAGOS.SANYI@GMAIL.COM', 'szigszalagos.sanyi@gmail.com', 'SZIGSZALAGOS.SANYI@GMAIL.COM', 0, 'AQAAAAIAAYagAAAAEP20lg/eQhbtGgGujOUNfO/hGxuOMufJaHe/+7AceDL3TwjHu6sNl2ufSLedJsyqpA==', 'L3CPGBTU7LSTKNO26QH24CA22IDGHHR3', '7181b57f-054e-49ef-8406-219480a1333c', NULL, 0, 0, NULL, 1, 0);

-- --------------------------------------------------------

--
-- Table structure for table `_role_claims`
--

CREATE TABLE `_role_claims` (
  `Id` int NOT NULL,
  `RoleId` int NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ClaimValue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Table structure for table `_user_claims`
--

CREATE TABLE `_user_claims` (
  `Id` int NOT NULL,
  `UserId` int NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ClaimValue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Table structure for table `_user_logins`
--

CREATE TABLE `_user_logins` (
  `LoginProvider` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProviderKey` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProviderDisplayName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `UserId` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Table structure for table `_user_roles`
--

CREATE TABLE `_user_roles` (
  `UserId` int NOT NULL,
  `RoleId` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `_user_roles`
--

INSERT INTO `_user_roles` (`UserId`, `RoleId`) VALUES
(1, 1),
(2, 1),
(3, 1),
(4, 1),
(5, 1),
(6, 1),
(7, 2),
(8, 2),
(9, 2),
(10, 2),
(11, 2),
(12, 2);

-- --------------------------------------------------------

--
-- Table structure for table `_user_tokens`
--

CREATE TABLE `_user_tokens` (
  `UserId` int NOT NULL,
  `LoginProvider` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Value` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Table structure for table `__EFMigrationsHistory`
--

CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `__EFMigrationsHistory`
--

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`) VALUES
('20260312202559_InitialCreate', '9.0.0'),
('20260316160409_UpdateExpertAddedStudies', '9.0.0'),
('20260316182731_RenamedExpertOccupaitonPlusMinorStuff', '9.0.0'),
('20260316185400_FixedStudyRelations', '9.0.0'),
('20260316205339_ServiceSupportsUnitBasedPricing', '9.0.0'),
('20260325170021_SeedMockData', '9.0.0'),
('20260329155240_JobStateMachine', '9.0.0'),
('20260404132059_AddingHBSZToTheDummyDatasetForFun', '9.0.0'),
('20260404190239_RemoveDurationAddJobDescription', '9.0.0'),
('20260404194940_AddJobTitle', '9.0.0'),
('20260411132935_AddExpertCompanyInfo', '9.0.0'),
('20260413154833_AddWorkLocationToExpert', '9.0.0'),
('20260414200438_AddJobCancelledFromStatus', '9.0.0'),
('20260420123228_MakeAddressBuildingNumberNullable', '9.0.0'),
('20260422164344_SeedDummyDataExpansion', '9.0.0'),
('20260422165700_RemoveCommentAndStudy', '9.0.0'),
('20260426164541_DumbAttributeTypesCleanup', '9.0.0');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `experts`
--
ALTER TABLE `experts`
  ADD PRIMARY KEY (`user_id`),
  ADD KEY `IX_experts_work_location_id` (`work_location_id`);

--
-- Indexes for table `expert_specialties`
--
ALTER TABLE `expert_specialties`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `IX_expert_specialties_expert_id_occupation_id` (`expert_id`,`occupation_id`),
  ADD KEY `IX_expert_specialties_occupation_id` (`occupation_id`);

--
-- Indexes for table `jobs`
--
ALTER TABLE `jobs`
  ADD PRIMARY KEY (`job_id`),
  ADD KEY `IX_jobs_pricing_id` (`pricing_id`),
  ADD KEY `IX_jobs_initiating_user_id` (`initiating_user_id`),
  ADD KEY `IX_jobs_location_id` (`location_id`),
  ADD KEY `IX_jobs_service_id` (`service_id`);

--
-- Indexes for table `occupations`
--
ALTER TABLE `occupations`
  ADD PRIMARY KEY (`feor_id`);

--
-- Indexes for table `pricing`
--
ALTER TABLE `pricing`
  ADD PRIMARY KEY (`pricing_id`);

--
-- Indexes for table `residential_addresses`
--
ALTER TABLE `residential_addresses`
  ADD PRIMARY KEY (`residential_address_id`),
  ADD KEY `IX_residential_addresses_settlement_id` (`settlement_id`);

--
-- Indexes for table `roles`
--
ALTER TABLE `roles`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `RoleNameIndex` (`NormalizedName`);

--
-- Indexes for table `services`
--
ALTER TABLE `services`
  ADD PRIMARY KEY (`service_id`),
  ADD KEY `IX_services_expert_specialties_id` (`expert_specialties_id`),
  ADD KEY `IX_services_pricing_id` (`pricing_id`);

--
-- Indexes for table `settlements`
--
ALTER TABLE `settlements`
  ADD PRIMARY KEY (`settlement_id`);

--
-- Indexes for table `time_intervals`
--
ALTER TABLE `time_intervals`
  ADD PRIMARY KEY (`time_interval_id`),
  ADD KEY `IX_time_intervals_job_id` (`job_id`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  ADD KEY `EmailIndex` (`NormalizedEmail`);

--
-- Indexes for table `_role_claims`
--
ALTER TABLE `_role_claims`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX__role_claims_RoleId` (`RoleId`);

--
-- Indexes for table `_user_claims`
--
ALTER TABLE `_user_claims`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX__user_claims_UserId` (`UserId`);

--
-- Indexes for table `_user_logins`
--
ALTER TABLE `_user_logins`
  ADD PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  ADD KEY `IX__user_logins_UserId` (`UserId`);

--
-- Indexes for table `_user_roles`
--
ALTER TABLE `_user_roles`
  ADD PRIMARY KEY (`UserId`,`RoleId`),
  ADD KEY `IX__user_roles_RoleId` (`RoleId`);

--
-- Indexes for table `_user_tokens`
--
ALTER TABLE `_user_tokens`
  ADD PRIMARY KEY (`UserId`,`LoginProvider`,`Name`);

--
-- Indexes for table `__EFMigrationsHistory`
--
ALTER TABLE `__EFMigrationsHistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `expert_specialties`
--
ALTER TABLE `expert_specialties`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT for table `jobs`
--
ALTER TABLE `jobs`
  MODIFY `job_id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT for table `occupations`
--
ALTER TABLE `occupations`
  MODIFY `feor_id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7538;

--
-- AUTO_INCREMENT for table `pricing`
--
ALTER TABLE `pricing`
  MODIFY `pricing_id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=30;

--
-- AUTO_INCREMENT for table `residential_addresses`
--
ALTER TABLE `residential_addresses`
  MODIFY `residential_address_id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT for table `roles`
--
ALTER TABLE `roles`
  MODIFY `Id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `services`
--
ALTER TABLE `services`
  MODIFY `service_id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=22;

--
-- AUTO_INCREMENT for table `settlements`
--
ALTER TABLE `settlements`
  MODIFY `settlement_id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT for table `time_intervals`
--
ALTER TABLE `time_intervals`
  MODIFY `time_interval_id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=31;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `Id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT for table `_role_claims`
--
ALTER TABLE `_role_claims`
  MODIFY `Id` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `_user_claims`
--
ALTER TABLE `_user_claims`
  MODIFY `Id` int NOT NULL AUTO_INCREMENT;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `experts`
--
ALTER TABLE `experts`
  ADD CONSTRAINT `FK_experts_settlements_work_location_id` FOREIGN KEY (`work_location_id`) REFERENCES `settlements` (`settlement_id`),
  ADD CONSTRAINT `FK_experts_users_user_id` FOREIGN KEY (`user_id`) REFERENCES `users` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `expert_specialties`
--
ALTER TABLE `expert_specialties`
  ADD CONSTRAINT `FK_expert_specialties_experts_expert_id` FOREIGN KEY (`expert_id`) REFERENCES `experts` (`user_id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_expert_specialties_occupations_occupation_id` FOREIGN KEY (`occupation_id`) REFERENCES `occupations` (`feor_id`) ON DELETE CASCADE;

--
-- Constraints for table `jobs`
--
ALTER TABLE `jobs`
  ADD CONSTRAINT `FK_jobs_pricing_pricing_id` FOREIGN KEY (`pricing_id`) REFERENCES `pricing` (`pricing_id`),
  ADD CONSTRAINT `FK_jobs_residential_addresses_location_id` FOREIGN KEY (`location_id`) REFERENCES `residential_addresses` (`residential_address_id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_jobs_services_service_id` FOREIGN KEY (`service_id`) REFERENCES `services` (`service_id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_jobs_users_initiating_user_id` FOREIGN KEY (`initiating_user_id`) REFERENCES `users` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `residential_addresses`
--
ALTER TABLE `residential_addresses`
  ADD CONSTRAINT `FK_residential_addresses_settlements_settlement_id` FOREIGN KEY (`settlement_id`) REFERENCES `settlements` (`settlement_id`) ON DELETE CASCADE;

--
-- Constraints for table `services`
--
ALTER TABLE `services`
  ADD CONSTRAINT `FK_services_expert_specialties_expert_specialties_id` FOREIGN KEY (`expert_specialties_id`) REFERENCES `expert_specialties` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_services_pricing_pricing_id` FOREIGN KEY (`pricing_id`) REFERENCES `pricing` (`pricing_id`) ON DELETE CASCADE;

--
-- Constraints for table `time_intervals`
--
ALTER TABLE `time_intervals`
  ADD CONSTRAINT `FK_time_intervals_jobs_job_id` FOREIGN KEY (`job_id`) REFERENCES `jobs` (`job_id`) ON DELETE CASCADE;

--
-- Constraints for table `_role_claims`
--
ALTER TABLE `_role_claims`
  ADD CONSTRAINT `FK__role_claims_roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `roles` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `_user_claims`
--
ALTER TABLE `_user_claims`
  ADD CONSTRAINT `FK__user_claims_users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `_user_logins`
--
ALTER TABLE `_user_logins`
  ADD CONSTRAINT `FK__user_logins_users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `_user_roles`
--
ALTER TABLE `_user_roles`
  ADD CONSTRAINT `FK__user_roles_roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `roles` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK__user_roles_users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `_user_tokens`
--
ALTER TABLE `_user_tokens`
  ADD CONSTRAINT `FK__user_tokens_users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
