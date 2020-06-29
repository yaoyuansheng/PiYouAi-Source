using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eason.Core.Users
{
    public class UserPermissions
    {
        public long userid { get; set; }
        public string username { get; set; }
        public string[] roles { get; set; }
        public string[] permissions { get; set; }
    }
}
