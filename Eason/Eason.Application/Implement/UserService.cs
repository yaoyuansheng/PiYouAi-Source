using Estone.Application.Cas.Interfaces;
using System;
using System.Threading.Tasks;
using Eason.EntityFramework.Repositories;
using Eason.Application;
using Eason.Core.Users;
using System.Linq.Expressions;
using Eason.EntityFramework.Entities.Authorization;
using ExpressMapper;
using System.Linq;
using System.Collections.Generic;

namespace Estone.Application.Cas.Implement
{
    /*
    password:只准许在本层处理，数据库中和应用层禁止对密码进行加密处理
    */

    /// <summary>
    /// Class LoginService.
    /// </summary>
    /// <seealso cref="Estone.Application.Cas.Interfaces.IUserService" />
    /// <seealso cref="System.IDisposable" />
    public class UserService : IUserService
    {
        public UserRepository userRepository { get; set; }
        public UserService(UserRepository _userRepository)
        {
            userRepository = _userRepository;
        }
        public async Task<LoginResult> GetLoginResultAsync(string account, string password)
        {
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
            {
                new ArgumentNullException("用户名或密码错误！");
            }
            LoginResult result = new LoginResult();
            var user = await userRepository.GetUserAsync(account);
            if (user == null)
            {
                result.status = LoginResultType.InvalidAccount;
                return result;
            }

            if (user.password != password.Encrypt())
            {
                result.status = LoginResultType.InvalidPassword;
                return result;
            }
            if (user.isActive != 0)
            {
                result.status = LoginResultType.UserIsNotActive;
                return result;
            }
            result.status = LoginResultType.Success;
            result.userId = user.id;
            result.name = user.name;
            result.imgUrl = user.imgUrl==null?string.Empty:user.imgUrl;
            return result;
        }

        public async Task<LoginResult> GetLoginResultAsync(string openid)
        {
          
            LoginResult result = new LoginResult();
            var user = await userRepository.FirstOrDefaultAsync(m=>m.openid==openid);
            if (user == null)
            {
                result.status = LoginResultType.InvalidAccount;
                return result;
            }
            if (user.isActive != 0)
            {
                result.status = LoginResultType.UserIsNotActive;
                return result;
            }
            result.status = LoginResultType.Success;
            result.userId = user.id;
            result.name = user.name;
            result.imgUrl = user.imgUrl == null ? string.Empty : user.imgUrl;
            return result;
        }
        public async Task<LoginResult> GetLoginResultByPhoneAsync(string phone, string password)
        {
            if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(password))
            {
                new ArgumentNullException("手机号或密码错误！");
            }
            LoginResult result = new LoginResult();
            var user = await userRepository.GetUserByPhoneAsync(phone);
            if (user == null)
            {
                result.status = LoginResultType.InvalidAccount;
                return result;
            }

            if (user.password != password.Encrypt())
            {
                result.status = LoginResultType.InvalidPassword;
                return result;
            }
            if (user.isActive != 0)
            {
                result.status = LoginResultType.UserIsNotActive;
                return result;
            }
            result.status = LoginResultType.Success;
            result.userId = user.id;
            result.name = user.name;
            result.imgUrl = user.imgUrl == null ? string.Empty : user.imgUrl;
            return result;
        }
        public async Task<RegisterUserResult> RegisterResultAsync(UserDto User)
        {
            if (User == null)
            {
                new ArgumentNullException("用户名或密码错误！");
            }
            User.password = User.password.Encrypt();

            var result = await userRepository.RegisterUserAsync(User);
            return result;
        }
        public IList<UserDto> GetList(int index, int page)
        {
            var result = userRepository.GetAllList(index, page);
            if (result != null && result.Count() > 0)
            {
                return Mapper.Map<IQueryable<User>, IList<UserDto>>(result);
            }
            return null;

        }
        public int Count()
        {
            return userRepository.Count();
        }
    }
}