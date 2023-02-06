'use strict';
console.log('Hello world');
const sign = require('./sign.js')
const tk = require('./Tk.js')
// 导入 express 模块
const express = require('express')
var bodyParser = require('body-parser')
// 导入 cors 中间件
const cors = require('cors');
// 创建 express 的服务器实例
const app = express()
// 调用 app.listen 方法，指定端口号并启动web服务器
app.listen(3007, function () {
    console.log('api server running at http://127.0.0.1:3007')
})
// 将 cors 注册为全局中间件
app.use(cors())
app.use(bodyParser.json())
app.use(express.urlencoded({ extended: true }))

app.post('/regSign', async (req, res) => {
    const data = req.body
    try {
        const string = sign.sign(data.Messages)
        console.log(string)
        res.json(string)
    } catch (e) {
        console.log(e)
        res.json(e)
    }
})
app.post('/gettk', async (req, res) => {
    const data = req.body
    try {
        const string = tk.tk(data.Messages)
        console.log(string)
        res.json(string)
    } catch (e) {
        console.log(e)
        res.json(e)
    }
})