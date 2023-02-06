using System;
using System.Collections.Generic;
using System.Linq;
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
}
