using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eason.Web.Models
{
    public class AccessToken
    {
        public AccessToken()
        {
            expires = DateTime.Now.AddHours(1.5);
        }
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public DateTime expires { get; set; }
    }
}