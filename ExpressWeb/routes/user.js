require('dotenv').config()
const express = require('express')
const router = express.Router()
const jwt = require('jsonwebtoken');
const bodyParser = require('body-parser');
const bcrypt = require('bcrypt')
const connection = require('../sql/connection')
const multer = require('multer');
const path = require("path");
const urlencodedParser = bodyParser.urlencoded({ extended: false })
const fs = require('fs');

router.use(express.json());

router.get('/', authenticateToken, (req, res) => {
    connection.query(`SELECT id, name, lastname FROM Users WHERE login = '${req.user.name}';`, (error, results) => {
        if (error) return res.sendStatus(500);
        res.json({ name: results[0]['name'], lastname: results[0]['lastname'], id: results[0]['id'] })
    })
})

router.post('/changepassword', authenticateToken, (req, res) => {
    if(req.body.newPassword.length < 6)
        return res.status(400).send("New password is too weak")
    connection.query(`select * from users where users.login = '${req.user.name}';`, async (error) => {
        if (error) return res.sendStatus(500);
        const hashedPassword = await bcrypt.hash(req.body.newPassword, 10)
        connection.query(`UPDATE Users SET Password = '${hashedPassword}' WHERE login = '${req.user.name}';`, (error) => {
            if (error) return res.sendStatus(500)
            res.sendStatus(200)
        })
    })
})

router.post('/changedetails', authenticateToken, (req, res) => {
    connection.query(`select * from users where users.login = '${req.user.name}';`, async (error) => {
        if (error) return res.sendStatus(500);
        if (req.body.name == null || req.body.lastname == null) return res.status(400).send("Details cannot be empty")
        connection.query(`UPDATE Users SET Name = '${req.body.name}', LastName = '${req.body.lastname}' WHERE login = '${req.user.name}';`, (error) => {
            if (error) return res.sendStatus(500)
            res.sendStatus(200)
        })
    })
})

router.get('/cars', authenticateToken, (req, res) => {
    connection.query(`select cars.id, carrenthistory.id as "rentid", StartDate, cardetails.Brand, cardetails.Model, cardetails.Year from carrenthistory
    join users on userid = users.id
    join cars on carid = cars.id
    join cardetails on cars.detailid = cardetails.id
    where users.login = '${req.user.name}' and EndDate is NULL;`, (error, results) => {
        if (error) return res.sendStatus(500);
        res.json(results)
    })
})

router.post('/rent/:carid', authenticateToken, (req, res) => {
    connection.query(`select state from carstate join cars on cars.stateid = carstate.id where cars.id = '${req.params.carid}';`, (error, results) => {
        if (error) return res.sendStatus(500);
        if(results[0].state != "Available") return res.status(400).send("Car Unavailable")
        connection.query(`SELECT * FROM carrenthistory WHERE carid = '${req.params.carid}' and enddate = NULL;`, (error, results) => {
            if(results.length > 0) return res.status(400).send("Error while renting this car")
            connection.query(`SELECT id FROM Users WHERE login = '${req.user.name}';`, (error, results) => {
                if (error) return res.sendStatus(500)
                var date = new Date()
                var queries = [`INSERT INTO carrenthistory (startdate, userid, carid) VALUES ('${date.toISOString().slice(0, 19).replace('T', ' ')}', '${results[0].id}', '${req.params.carid}')`,
                            `UPDATE carstate join cars on carstate.id = cars.stateid SET state = 'Rented', locationid = NULL where cars.id = '${req.params.carid}'` ]
                connection.query(queries.join(';'),  (error) => {
                    if (error) return res.sendStatus(500)
                    res.status(200).send("Success")
                })
            })
        })
    })
})

router.get('/history', authenticateToken, (req, res) => {
    connection.query(`select cars.id, carrenthistory.id as "rentid", StartDate, EndDate, cardetails.Brand, cardetails.Model, cardetails.Year from carrenthistory
    join users on userid = users.id
    join cars on carid = cars.id
    join cardetails on cars.detailid = cardetails.id
    where users.login = '${req.user.name}' and EndDate is not NULL;`, (error, results) => {
        if (error) return res.sendStatus(500);
        res.json(results)
    })
})

router.post('/pay/:rentid', authenticateToken, urlencodedParser, (req, res) => {
    connection.query(`select paymentid, users.login from carrenthistory join users on users.id = carrenthistory.userid where carrenthistory.id = '${req.params.rentid}';`, (error, results) => {
        if (error) return res.sendStatus(500);
        var rent = results[0]
        if (rent.paymentid != null || rent.login != req.user.name || req.body.Number?.length != 16 || req.body.CVC?.length != 3 || req.body.ExpireDate?.length != 7) 
            return res.sendStatus(400)
        connection.query(`insert into payments (CardNumber) values ('${req.body.Number}');`, (error, result, fields) => {
            if (error) return res.sendStatus(500);
            connection.query(`UPDATE carrenthistory SET paymentid = '${result.insertId}' WHERE id = '${req.params.rentid}';`,  (error) => {
                if (error) return res.sendStatus(500)
                res.sendStatus(200)
            })
        })
    })
})

router.get('/status/:rentid', authenticateToken, urlencodedParser, (req, res) => {
    connection.query(`select paymentid, users.login from carrenthistory join users on users.id = carrenthistory.userid where carrenthistory.id = '${req.params.rentid}';`, (error, results) => {
        if (error) return res.sendStatus(500);
        if (results[0].login != req.user.name) return res.sendStatus(403)
        if(results[0].paymentid == null) return res.status(200).send("Not Paid")
        else return res.status(200).send("Paid")
    })
})


router.post('/return/:rentid/:location', authenticateToken, (req, res) => {
    connection.query(`SELECT * from carrenthistory join users on carrenthistory.userid = users.id WHERE carrenthistory.id = '${req.params.rentid}' and users.login = '${req.user.name}';`, (error, results) => {
        if (error) return res.sendStatus(500)
        if (results.length == 0) return res.sendStatus(400)
        var CarId = results[0].carid
        connection.query(`SELECT * FROM locations WHERE name = '${req.params.location}';`, (error, results) => {
            if (error) return res.sendStatus(500)
            if (results.length == 0) return res.sendStatus(400)
            var location = results[0].id
            var date = new Date()
            var queries = [`UPDATE carrenthistory SET enddate = '${date.toISOString().slice(0, 19).replace('T', ' ')}' where id = '${req.params.rentid}'`,
            `UPDATE carstate join cars on carstate.id = cars.stateid SET state = 'Available', locationid = '${location}' where cars.id = '${CarId}'` ]
            connection.query(queries.join(';'),  (error) => {
                if (error) return res.sendStatus(500)
                if (results.affectedRows == 0) return res.status(400).send("Failed to return car")
                res.status(200).send("Success")
            })
        })
    })
})

router.post('/report/:rentid', authenticateToken, urlencodedParser, (req, res) => {
    connection.query(`SELECT * from carrenthistory join users on carrenthistory.userid = users.id WHERE carrenthistory.id = '${req.params.rentid}' and users.login = '${req.user.name}';`, (error, results) => {
        if (error) return res.sendStatus(500)
        if (results.length == 0) return res.sendStatus(400)
        var rent = results[0]
        var date = new Date();
        connection.query(`insert into carincidenthistory (details, date, carid, userid) values ('${req.body.Details}', '${date.toISOString().slice(0, 19).replace('T', ' ')}', '${rent.carid}', '${rent.userid}');`, (error, results) => {
            if (error) return res.sendStatus(500)
            res.status(200).send("Success")
        })
    })
})

router.post('/avatar', authenticateToken, multer({ dest: path.resolve("images\\users\\") }).single('file'), (req, res) => {
    connection.query(`SELECT id FROM Users WHERE login = '${req.user.name}';`, (error, results) => {
        if (error) fs.rm(req.file.path, () => { return res.sendStatus(500) })

        var userfolder = req.file.destination + "\\" + results[0].id
        fs.rename(req.file.destination + "\\" + req.file.filename, userfolder + "\\profilepicture" + req.file.originalname.substring(req.file.originalname.lastIndexOf('.')), (error) => {
            if (error)
                fs.rm(req.file.path, () => { return res.sendStatus(500) })

            res.sendStatus(200)
        })
    })
})

router.post('/refreshtoken', (req, res) => {
    const refreshToken = req.body.token
    if (refreshToken == null) return res.sendStatus(401)

    connection.query(`SELECT * FROM Users WHERE RefreshToken = '${refreshToken}';`, (error, results) => {
        if (error || results.length == 0) return res.sendStatus(403)

        jwt.verify(refreshToken, process.env.REFRESH_TOKEN_SECRET, (error, user) => {
            if (error) return res.sendStatus(403)

            const accessToken = generateAccessToken({ name: user.name })
            res.send(accessToken)
        })
    })
})

router.post('/register', urlencodedParser, checkDataStrength, (req, res) => {
    connection.query(`SELECT * FROM Users WHERE login = '${req.body.login}';`, async (error, results) => {
        if (error) return res.sendStatus(500)
        if (results.length > 0) return res.status(400).send("Login exists")

        const hashedPassword = await bcrypt.hash(req.body.password, 10)

        connection.query(`INSERT INTO Users (name, lastname, login, password) VALUES ('${req.body.name}', '${req.body.lastname}', '${req.body.login}', '${hashedPassword}');`, async (error) => {
            if (error) return res.status(400).send("Failed to register new user")
            return res.status(200).send("User Registered")
        })
    });
})

router.post('/login', urlencodedParser, (req, res) => {
    connection.query(`SELECT * FROM Users WHERE login = '${req.body.login}';`, async (error, results) => {
        if (error) return res.sendStatus(500)
        if (results.length == 0 || !await bcrypt.compare(req.body.password, results[0].password)) return res.status(401).send("Incorrect login or password")

        const user = { name: req.body.login }
        const accessToken = generateAccessToken(user)
        const refreshToken = jwt.sign(user, process.env.REFRESH_TOKEN_SECRET)

        connection.query(`UPDATE Users SET RefreshToken = '${refreshToken}' WHERE login = '${req.body.login}';`,  (error) => {
            if (error) return res.sendStatus(500)
            res.json({ accesstoken: accessToken, refreshtoken: refreshToken })
        })
    })
})

router.post('/logout', authenticateToken, (req, res) => {
    connection.query(`UPDATE Users SET RefreshToken = NULL WHERE login = '${req.user.name}';`,  (error) => {
        if (error) return res.sendStatus(500)
        res.status(200).send("User logged out")
    })
})

function generateAccessToken(user) {
    return accessToken = jwt.sign(user, process.env.ACCESS_TOKEN_SECRET, { expiresIn: '10m' });
}

function checkDataStrength(req, res, next) {
    if (req.body.login == null || req.body.login.length < 4) return res.status(400).send("Login must be larger than 3 characters")
    if (req.body.password == null || req.body.password.length < 6) return res.status(400).send("Password must be larger than 5 characters")
    next()
}

function authenticateToken(req, res, next) {
    const authHeader = req.headers['authorization']
    const token = authHeader && authHeader.split(' ')[1]
    if (token == null) return res.sendStatus(401)

    jwt.verify(token, process.env.ACCESS_TOKEN_SECRET, (error, user) => {
        if (error) return res.sendStatus(403)
        connection.query(`SELECT RefreshToken FROM Users WHERE Login = '${user.name}';`, (error, results) => {
            if(error) return res.sendStatus(500)
            if(results[0].RefreshToken == null) return res.sendStatus(401)
            req.user = user
            next()
        })
    })
}

module.exports = {
    authenticateToken,
    router
}