using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eason.Core
{
    public class SessionData
    {
        public long uid { get; set; }

        public string account { get; set; }

        public string password { get; set; }

        public int status { get; set; }


        public DateTime limitTime { get; set; }

        public DateTime loginTime { get; set; }
        public string ip { get; set; }

    }
}
