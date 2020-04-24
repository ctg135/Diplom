-- phpMyAdmin SQL Dump
-- version 3.5.1
-- http://www.phpmyadmin.net
--
-- Хост: 127.0.0.1
-- Время создания: Апр 24 2020 г., 18:47
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
-- Структура таблицы `accesslevels`
--

CREATE TABLE IF NOT EXISTS `accesslevels` (
  `Id` int(11) NOT NULL,
  `Name` varchar(30) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `accesslevels`
--

INSERT INTO `accesslevels` (`Id`, `Name`) VALUES
(1, 'Администратор'),
(2, 'Начальник'),
(3, 'Руководитель'),
(4, 'Работник');

-- --------------------------------------------------------

--
-- Структура таблицы `daytypes`
--

CREATE TABLE IF NOT EXISTS `daytypes` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Description` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=6 ;

--
-- Дамп данных таблицы `daytypes`
--

INSERT INTO `daytypes` (`Id`, `Description`) VALUES
(1, 'Рабочий день'),
(2, 'Выходной'),
(3, 'Больничный'),
(4, 'Отпуск'),
(5, 'Не определён');

-- --------------------------------------------------------

--
-- Структура таблицы `department`
--

CREATE TABLE IF NOT EXISTS `department` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=3 ;

--
-- Дамп данных таблицы `department`
--

INSERT INTO `department` (`Id`, `Name`) VALUES
(1, 'Программистический'),
(2, 'Сварщический');

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
  `DayType` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `Id_Worker` (`WorkerId`),
  KEY `Total` (`DayType`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=3 ;

--
-- Дамп данных таблицы `plans`
--

INSERT INTO `plans` (`Id`, `WorkerId`, `Date`, `StartOfDay`, `EndOfDay`, `DayType`) VALUES
(1, 3, '2020-04-17', '08:00:00', '15:00:00', 1),
(2, 5, '2020-04-17', '09:30:00', '16:30:00', 3);

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Структура таблицы `statuses`
--

CREATE TABLE IF NOT EXISTS `statuses` (
  `Code` int(11) NOT NULL AUTO_INCREMENT,
  `Title` varchar(30) DEFAULT NULL,
  `Description` text,
  PRIMARY KEY (`Code`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=7 ;

--
-- Дамп данных таблицы `statuses`
--

INSERT INTO `statuses` (`Code`, `Title`, `Description`) VALUES
(1, 'Не установлен', 'Статус не был установлен (рабочий день не начался)'),
(2, 'На работе', 'Работник на рабочем месте'),
(3, 'На перерыве', 'Работник находится на перерыве'),
(4, 'В отпуске', 'В отпуске'),
(5, 'Рабочий день закончен', 'Работник отрабтал весь день'),
(6, 'На больничном', 'На больничном');

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
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=2 ;

--
-- Дамп данных таблицы `statuslogs`
--

INSERT INTO `statuslogs` (`Id`, `WorkerId`, `SetDate`, `SetTime`, `StatusCode`) VALUES
(1, 3, '2020-04-17', '08:30:00', 2);

-- --------------------------------------------------------

--
-- Структура таблицы `tasks`
--

CREATE TABLE IF NOT EXISTS `tasks` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Description` varchar(30) NOT NULL,
  `SetWorker` int(11) NOT NULL,
  `Stage` int(11) NOT NULL,
  `SetterWorker` int(11) NOT NULL,
  `Created` datetime NOT NULL,
  `Finished` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `SetWorker` (`SetWorker`,`Stage`,`SetterWorker`),
  KEY `SetterWorker` (`SetterWorker`),
  KEY `Stage` (`Stage`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=3 ;

--
-- Дамп данных таблицы `tasks`
--

INSERT INTO `tasks` (`Id`, `Description`, `SetWorker`, `Stage`, `SetterWorker`, `Created`, `Finished`) VALUES
(1, 'Диплом крафт', 3, 1, 4, '0000-00-00 00:00:00', NULL),
(2, 'Диплом', 3, 1, 3, '2020-04-17 10:32:22', NULL);

-- --------------------------------------------------------

--
-- Структура таблицы `taskstage`
--

CREATE TABLE IF NOT EXISTS `taskstage` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Description` varchar(30) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=4 ;

--
-- Дамп данных таблицы `taskstage`
--

INSERT INTO `taskstage` (`Id`, `Description`) VALUES
(1, 'Ожидает принятия'),
(2, 'Принят к выполнению'),
(3, 'Выполнено');

-- --------------------------------------------------------

--
-- Структура таблицы `workers`
--

CREATE TABLE IF NOT EXISTS `workers` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(30) NOT NULL,
  `Surname` varchar(30) NOT NULL,
  `Patronymic` varchar(30) NOT NULL,
  `Department` int(11) NOT NULL,
  `Position` varchar(30) NOT NULL,
  `AccessLevel` int(11) NOT NULL,
  `Login` varchar(30) NOT NULL,
  `Password` varchar(40) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Login` (`Login`),
  KEY `department` (`Department`),
  KEY `AccessLevel` (`AccessLevel`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=9 ;

--
-- Дамп данных таблицы `workers`
--

INSERT INTO `workers` (`Id`, `Name`, `Surname`, `Patronymic`, `Department`, `Position`, `AccessLevel`, `Login`, `Password`) VALUES
(3, 'Василий', 'Салабаев', 'Сергеевич', 1, 'пРОГЕР', 2, 'salabaev', 'hash'),
(4, 'Никита', 'Лабутин', 'Тостур', 1, 'Лаборант', 3, 'toster', 'hash'),
(5, 'Артем', 'Федкеевич', 'Святославочи', 1, 'Охранник', 4, 'vergas', 'hash'),
(6, 'Валерий', 'Вергасов', 'вадимвоч', 2, 'Диктор', 2, 'valret', 'hash'),
(7, 'Эдуард', 'Михайлов', 'jaifjsdjksdf', 2, 'Директор', 3, 'eduk', 'hash'),
(8, 'Iop', 'Poi', 'Lopa', 2, 'loiolj', 4, 'loi', 'hash');

--
-- Ограничения внешнего ключа сохраненных таблиц
--

--
-- Ограничения внешнего ключа таблицы `plans`
--
ALTER TABLE `plans`
  ADD CONSTRAINT `plans_ibfk_1` FOREIGN KEY (`WorkerId`) REFERENCES `workers` (`Id`),
  ADD CONSTRAINT `plans_ibfk_2` FOREIGN KEY (`DayType`) REFERENCES `daytypes` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Ограничения внешнего ключа таблицы `sessions`
--
ALTER TABLE `sessions`
  ADD CONSTRAINT `sessions_ibfk_1` FOREIGN KEY (`WorkerId`) REFERENCES `workers` (`Id`);

--
-- Ограничения внешнего ключа таблицы `statuslogs`
--
ALTER TABLE `statuslogs`
  ADD CONSTRAINT `statuslogs_ibfk_1` FOREIGN KEY (`WorkerId`) REFERENCES `workers` (`Id`),
  ADD CONSTRAINT `statuslogs_ibfk_2` FOREIGN KEY (`StatusCode`) REFERENCES `statuses` (`Code`);

--
-- Ограничения внешнего ключа таблицы `tasks`
--
ALTER TABLE `tasks`
  ADD CONSTRAINT `tasks_ibfk_3` FOREIGN KEY (`SetterWorker`) REFERENCES `workers` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `tasks_ibfk_1` FOREIGN KEY (`Stage`) REFERENCES `taskstage` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `tasks_ibfk_2` FOREIGN KEY (`SetWorker`) REFERENCES `workers` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Ограничения внешнего ключа таблицы `workers`
--
ALTER TABLE `workers`
  ADD CONSTRAINT `workers_ibfk_1` FOREIGN KEY (`Department`) REFERENCES `department` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `workers_ibfk_2` FOREIGN KEY (`AccessLevel`) REFERENCES `accesslevels` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
