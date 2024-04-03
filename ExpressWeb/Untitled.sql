CREATE TABLE `CarRentHistory` (
  `startdate` date,
  `enddate` date,
  `userid` integer,
  `carid` integer,
  `paymentid` integer
);

CREATE TABLE `Cars` (
  `id` integer,
  `detailid` integer,
  `stateid` integer
);

CREATE TABLE `CarDetails` (
  `id` integer,
  `brand` varchar(255),
  `model` varchar(255),
  `year` integer
);

CREATE TABLE `CarState` (
  `id` integer,
  `state` varchar(255),
  `locationid` integer
);

CREATE TABLE `CarIncidentHistory` (
  `id` integer,
  `date` date,
  `userid` integer,
  `carid` integer
);

CREATE TABLE `Locations` (
  `id` integer,
  `name` varchar(255),
  `cordinates` varchar(255)
);

CREATE TABLE `Users` (
  `id` integer,
  `name` varchar(255),
  `lastname` varchar(255),
  `login` varchar(255),
  `password` varchar(255)
);

CREATE TABLE `Payments` (
  `id` integer,
  `date` date,
  `cardnumber` varchar(255)
);

ALTER TABLE `CarRentHistory` ADD FOREIGN KEY (`userid`) REFERENCES `Users` (`id`);

ALTER TABLE `CarRentHistory` ADD FOREIGN KEY (`carid`) REFERENCES `Cars` (`id`);

ALTER TABLE `CarRentHistory` ADD FOREIGN KEY (`paymentid`) REFERENCES `Payments` (`id`);

ALTER TABLE `CarIncidentHistory` ADD FOREIGN KEY (`userid`) REFERENCES `Users` (`id`);

ALTER TABLE `CarIncidentHistory` ADD FOREIGN KEY (`carid`) REFERENCES `Cars` (`id`);

ALTER TABLE `Locations` ADD FOREIGN KEY (`id`) REFERENCES `CarState` (`locationid`);

ALTER TABLE `CarState` ADD FOREIGN KEY (`id`) REFERENCES `Cars` (`stateid`);

ALTER TABLE `CarDetails` ADD FOREIGN KEY (`id`) REFERENCES `Cars` (`detailid`);
