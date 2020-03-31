-- phpMyAdmin SQL Dump
-- version 3.5.1
-- http://www.phpmyadmin.net
--
-- Хост: 127.0.0.1
-- Время создания: Мар 31 2020 г., 12:23
-- Версия сервера: 5.5.25
-- Версия PHP: 5.3.13

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- База данных: `workers`
--

-- --------------------------------------------------------

--
-- Структура таблицы `plans`
--

CREATE TABLE IF NOT EXISTS `plans` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `WorkerId` int(11) NOT NULL,
  `Date` date NOT NULL,
  `StartOfDay` time NOT NULL,
  `EndOfDay` time NOT NULL,
  `Total` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `Id_Worker` (`WorkerId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Структура таблицы `sessions`
--

CREATE TABLE IF NOT EXISTS `sessions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `WorkerId` int(11) NOT NULL,
  `Token` varchar(40) NOT NULL,
  `ClientInfo` text NOT NULL,
  `Created` datetime NOT NULL,
  `LastUpdate` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `WorkerId` (`WorkerId`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Структура таблицы `statuses`
--

CREATE TABLE IF NOT EXISTS `statuses` (
  `Code` int(11) NOT NULL AUTO_INCREMENT,
  `Title` varchar(30) DEFAULT NULL,
  `Description` text,
  PRIMARY KEY (`Code`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=8 ;

--
-- Дамп данных таблицы `statuses`
--

INSERT INTO `statuses` (`Code`, `Title`, `Description`) VALUES
(1, 'Не установлен', 'Статус не был установлен (рабочий день не начался)'),
(2, 'На работе', 'Работник на рабочем месте'),
(3, 'На перерыве', 'Работник находится на перерыве'),
(4, 'В отпуске', 'В отпуске'),
(5, 'Рабочий день закончен', 'Работник отрабтал весь день'),
(6, 'На больничном', 'На больничном'),
(7, 'Командировка', 'В командировке');

-- --------------------------------------------------------

--
-- Структура таблицы `statuslogs`
--

CREATE TABLE IF NOT EXISTS `statuslogs` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `WorkerId` int(11) NOT NULL,
  `SetDate` date NOT NULL,
  `SetTime` time NOT NULL,
  `StatusCode` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `WorkerId` (`WorkerId`,`StatusCode`),
  KEY `StatusCode` (`StatusCode`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Структура таблицы `workers`
--

CREATE TABLE IF NOT EXISTS `workers` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(30) NOT NULL,
  `Surname` varchar(30) NOT NULL,
  `Patronymic` varchar(30) NOT NULL,
  `BirthDate` date NOT NULL,
  `Mail` varchar(60) DEFAULT NULL,
  `Position` varchar(30) NOT NULL,
  `Rate` int(11) NOT NULL,
  `AccessLevel` int(11) NOT NULL,
  `Login` varchar(30) NOT NULL,
  `Password` varchar(30) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Login` (`Login`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

--
-- Ограничения внешнего ключа сохраненных таблиц
--

--
-- Ограничения внешнего ключа таблицы `plans`
--
ALTER TABLE `plans`
  ADD CONSTRAINT `plans_ibfk_1` FOREIGN KEY (`WorkerId`) REFERENCES `workers` (`Id`);

--
-- Ограничения внешнего ключа таблицы `statuslogs`
--
ALTER TABLE `statuslogs`
  ADD CONSTRAINT `statuslogs_ibfk_1` FOREIGN KEY (`WorkerId`) REFERENCES `workers` (`Id`),
  ADD CONSTRAINT `statuslogs_ibfk_2` FOREIGN KEY (`StatusCode`) REFERENCES `statuses` (`Code`);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
