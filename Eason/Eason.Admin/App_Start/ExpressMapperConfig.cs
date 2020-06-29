using Eason.Core.Users;
using Eason.EntityFramework.Entities.Authorization;
using ExpressMapper;

namespace Eason.Admin
{
    public class ExpressMapperConfig
    {
        public static void RegisterMapper()
        {
            Mapper.Register<UserDto, User>();
            Mapper.Register<User, UserDto>().Member(m => m.isActive, n => n.isActive == 0 ? "正常" : "限制登陆");
        }
    }
}