using System;

namespace Eason.Core.Users
{
    public class UserDto
    {
       
        public long id { get; set; }
        public string name { get; set; }

        public string password { get; set; }

        public string email { get; set; }

        public string imgUrl { get; set; }
        public string openid { get; set; }
        
        public string telephone { get; set; }
        public string isDelete { get; set; }
        public DateTime lastLoginTime { get; set; }
        public string isActive { get; set; }
        public DateTime creationTime { get; set; }

    }
}
