using Eason.Core.Users;
using Eason.EntityFramework.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Estone.Application.Cas.Interfaces
{
    public interface IUserService
    {
        UserRepository userRepository { get; set; }
        Task<LoginResult> GetLoginResultAsync(string account, string password);
        Task<LoginResult> GetLoginResultAsync(string openid);
        Task<LoginResult> GetLoginResultByPhoneAsync(string phone, string password);
        Task<RegisterUserResult> RegisterResultAsync(UserDto User);
        IList<UserDto> GetList(int index, int page);
        int Count();
    }
}