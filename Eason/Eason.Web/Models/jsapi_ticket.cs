using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eason.Web.Models
{
    public class jsapi_ticket
    {
        public jsapi_ticket()
        {
            expires = DateTime.Now.AddHours(1.5);
        }
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public string ticket { get; set; }
        public string expires_in { get; set; }
        public DateTime expires { get; set; }
    }
}