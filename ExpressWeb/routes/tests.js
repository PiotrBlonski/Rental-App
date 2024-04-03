require('dotenv').config()
const express = require('express')
const router = express.Router()
const connection = require('../sql/connection')

router.post('/reset', (req, res) => {
    var queries = ["SET FOREIGN_KEY_CHECKS = 0",
        "delete from CarRentHistory",
        "alter table CarRentHistory AUTO_INCREMENT = 1",
        "delete from cars",
        "alter table Cars AUTO_INCREMENT = 1",
        "delete from CarDetails",
        "alter table CarDetails AUTO_INCREMENT = 1",
        "delete from CarState",
        "alter table CarState AUTO_INCREMENT = 1",
        "delete from CarIncidentHistory",
        "alter table CarIncidentHistory AUTO_INCREMENT = 1",
        "delete from Locations",
        "alter table Locations AUTO_INCREMENT = 1",
        "delete from Users",
        "alter table Users AUTO_INCREMENT = 1",
        "SET FOREIGN_KEY_CHECKS = 1"]
    connection.query(queries.join(';'))
    res.sendStatus(200)
})

router.post('/new', (req, res) => {
    var cars = [{ brand: "Toyota", model: "Corolla", year: 2022, price: 70 },
    { brand: "Toyota", model: "Yaris", year: 2018, price: 60 },
    { brand: "Kia", model: "Sportage", year: 2015, price: 45 },
    { brand: "Dacia", model: "Duster", year: 2017, price: 65 },
    { brand: "Skoda", model: "Octavia", year: 2010, price: 75 },
    { brand: "Toyota", model: "RAV4", year: 2023, price: 85 },
    { brand: "Hyundai", model: "Tucson", year: 2019, price: 74 },
    { brand: "Hyundai", model: "i30", year: 2016, price: 63 },
    { brand: "Toyota", model: "Yaris Cross", year: 2022, price: 68 },
    { brand: "Toyota", model: "C-HR", year: 2016, price: 85 },
    { brand: "Volkswagen", model: "T-Roc", year: 2017, price: 75 },
    { brand: "Skoda", model: "Fabia II", year: 2014, price: 40 },
    { brand: "Kia", model: "Ceed", year: 2016, price: 74 },
    { brand: "Dacia", model: "Sandero", year: 2015, price: 84 },
    { brand: "Skoda", model: "Kamiq", year: 2019, price: 46 }]
    for (let i = 0; i < cars.length; i++) {
        connection.query(`insert into cardetails (brand, model, year, price) values ('${cars[i].brand}', '${cars[i].model}', '${cars[i].year}', '${cars[i].price}');`)
    }
    for (let i = 0; i < 50; i++)
        connection.query(`insert into locations (name, cordinates) values ('Warsaw-${i}', '${52.264039 - Math.random() * 0.056583},${20.924660 + Math.random() * 0.255349}');`)

    for (let i = 0; i < 500; i++) {
        connection.query(`insert into carstate (state, locationid) values ('Available', '${RandomInt(50) + 1}');`)
        connection.query(`insert into cars (detailid, stateid) values ('${RandomInt(cars.length) + 1}', '${i + 1}');`)
    }
    for (let i = 0; i < 200; i++) {
        var incidents = [ "Crash", "Engine Fail", "Car Inspection", "Oil Change", "Replacement of the oxygen sensor", "Tyre repair" ]
        var day = RandomInt(25) + 1
        var month = RandomInt(12) + 1
        var enddate = new Date(`${month}/${day}/23`)
        var date = new Date(enddate.getTime() - Math.max(RandomInt(80000000000), 3000000000))
        connection.query(`insert into carincidenthistory (details, date, carid) values ('${incidents[RandomInt(incidents.length)]}', '${date.toISOString().slice(0, 19).replace('T', ' ')}', '${RandomInt(500) + 1}');`)
    }
    res.sendStatus(200)
})

router.post('/fill/:userid', (req, res) => {
    for (let i = 0; i < 15; i++) {
        var day = RandomInt(25) + 1
        var month = RandomInt(12) + 1
        var enddate = new Date(`${month}/${day}/23`)
        var date = new Date(enddate.getTime() - Math.max(RandomInt(50000000000), 1000000000))
        connection.query(`insert into carrenthistory (startdate, enddate, userid, carid) values ('${date.toISOString().slice(0, 19).replace('T', ' ')}', '${enddate.toISOString().slice(0, 19).replace('T', ' ')}', '${req.params.userid}', '${RandomInt(500) + 1}');`)
    }
    res.sendStatus(200)
})

router.get('/date', (req, res) => {
    var date = new Date().toISOString().slice(0, 19).replace('T', ' ');
    res.status(200).send(date);
})

function RandomInt(max) {
    return Math.floor(Math.random() * max);
}

module.exports = {
    router
}