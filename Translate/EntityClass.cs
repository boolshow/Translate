using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Translate
{
    internal class EntityClass
    {
    }
    class Heade
    {
        public string Key { get; set; }
        public string Token { get; set; }
        public string IG { get; set; }
    }
    class Paramete
    {
        public string Messages { get; set; }
        public Paramete(string messages)
        {
            Messages = messages;
        }
    }
    class Edgbody
    {
        public string Text { get; set; }
    }
    class BaiduToken
    {
        public string Token { get; set; }
        public CookieContainer cookieContainer { get; set; }
    }
    class Browserheader
    {
        //ua: ua,
        //url: "https://fanyi.baidu.com/#" + from + "/" + to + "/" + encodeURIComponent(information),
        //platform: "Win32",
        //clientTs: new Date().getTime(),
        //version: "1.0.0.6"
        public string ua { get; set; }
        public string url { get; set; }
        public string platform { get; set; } = "Win32";
        public string clientTs { get; set; }
        public string version { get; set; } = "1.0.0.6";
    }
}
