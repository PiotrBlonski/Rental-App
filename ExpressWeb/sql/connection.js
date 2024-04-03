const mysql = require('mysql2');

var connection = mysql.createPool({
  host: "localhost",
  port: 3306,
  user: "root",
  password: "",
  database: "ICarus",
  multipleStatements: true
});

module.exports = connection