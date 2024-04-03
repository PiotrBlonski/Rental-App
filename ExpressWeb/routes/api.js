require('dotenv').config()
const express = require('express')
const router = express.Router()
const fs = require('fs');
const connection = require('../sql/connection')
const path = require("path");
var absolutePath = path.resolve("images");

router.get('/image/cars/:brand/:model', (req, res) => {
    var path = `${absolutePath}\\cars\\${req.params.brand}\\${req.params.model}`;
    var defaultpath = `${absolutePath}\\cars`
    ReturnFile(path, defaultpath, res)
})

router.get('/image/user/:id', (req, res) => {
    connection.query(`SELECT COUNT(*) FROM Users;`, async (error, result) => {
        if (error || result[0]['COUNT(*)'] < req.params.id) return res.sendStatus(404)

        var path = `${absolutePath}\\users\\${req.params.id}`;
        var defaultpath = `${absolutePath}\\users`

        if (!fs.existsSync(path))
            fs.mkdirSync(absolutePath + "\\users\\" + result[0]['COUNT(*)'])

        ReturnFile(path, defaultpath, res)
    })
})

router.get('/locations', (req, res) => {
    connection.query(`select name, cordinates from locations;`, (error, results) => {
        if (error) return res.sendStatus(500);
        res.json(results)
    })
})

router.get('/locations/:name', (req, res) => {
    connection.query(`select cars.id, brand, model, year, price, carstate.state from cardetails
    join cars on cars.detailid = cardetails.id
    join carstate on cars.stateid = carstate.id
    join locations on carstate.locationid = locations.id
    where locations.name = '${req.params.name}' and carstate.state = 'Available';`, (error, results) => {
        if (error) return res.sendStatus(500);
        res.json(results)
    })
})

router.get('/cars/history/:id', (req, res) => {
    connection.query(`select details, date from carincidenthistory where carid = '${req.params.id}';`, (error, results) => {
        if (error) return res.sendStatus(500);
        res.json(results)
    })
})

function ReturnFile(path, defaultpath, res) {
    fs.open(path, 'r', (err, fd) => {
        if (err || !fs.existsSync(path))
            return res.sendStatus(404)

        var files = fs.readdirSync(path);
        if (files.length == 0) {
            files = fs.readdirSync(defaultpath + "\\default")
            return res.sendFile(defaultpath + "\\default\\" + files[0])
        }

        return res.sendFile(path + "\\" + files[0])
    });
}


module.exports = {
    router
}