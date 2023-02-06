'use strict';
console.log('Hello world');
const sign = require('./sign.js')
const tk = require('./Tk.js')
// ���� express ģ��
const express = require('express')
var bodyParser = require('body-parser')
// ���� cors �м��
const cors = require('cors');
// ���� express �ķ�����ʵ��
const app = express()
// ���� app.listen ������ָ���˿ںŲ�����web������
app.listen(3007, function () {
    console.log('api server running at http://127.0.0.1:3007')
})
// �� cors ע��Ϊȫ���м��
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