const express = require("express")
const app = express()
const https = require("https");
const fs = require("fs");

app.set('view engine', 'ejs')

const userRouter = require('./routes/user').router
const testsRouter = require('./routes/tests').router
const apiRouter = require('./routes/api').router

app.get('/', (req, res) => {
    res.render('index')
})

app.use('/user', userRouter)
app.use('/tests', testsRouter)
app.use('/api', apiRouter)

app.listen(3000)
/*https.createServer({
    key: fs.readFileSync("key.pem"),
    cert: fs.readFileSync("cert.pem"),
}, app)
    .listen(3000, () => {
        console.log('server is runing at port 3000')
    });*/