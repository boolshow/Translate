﻿// See https://aka.ms/new-console-template for more information
using System.Net;

Console.WriteLine("Hello, World!");
ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;