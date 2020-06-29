using System.Threading.Tasks;
using System.Data.Entity;
using Eason.EntityFramework.Entities.Authorization;
using Eason.Core.Users;
using ExpressMapper;
using System;
using System.Linq;

namespace Eason.EntityFramework.Repositories
{
    public class UserRepository : EasonRepository<User, long>
    {

        public async Task<User> GetUserAsync(string name)
        {
            return await FirstOrDefaultAsync(i => i.name == name);
        }
        public async Task<User> GetUserByPhoneAsync(string phone)
        {
            return await FirstOrDefaultAsync(i => i.telephone == phone);
        }
        public async Task<UserPermissions> GetUserPermissionsAsync(string name)
        {
            var context = Context as EasonEntities;
            var userPerssions = new UserPermissions();
            userPerssions.username = name;
            userPerssions.roles = await (from u in context.Users
                                         join ur in context.UserRoles on u.id equals ur.user.id
                                         join r in context.Roles on ur.role.id equals r.id
                                         where u.name == name
                                         select r.name).ToArrayAsync();
            userPerssions.permissions = await (from r in context.Roles
                                               join pr in context.PermissionRoles on r.id equals pr.role.id
                                               join p in context.Permissions on pr.permissionId equals p.id
                                               where userPerssions.roles.Contains(r.name)
                                               select p.url).ToArrayAsync();
            return userPerssions;
        }
        /// <summary>
        /// add user as an asynchronous operation.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>Task&lt;UserDto&gt;.</returns>
        public async Task<RegisterUserResult> RegisterUserAsync(UserDto dto)
        {
            var result = new RegisterUserResult();
            if (dto == null)
            {
                result.status = RegisterUserResultType.InvalidAccount;
                return result;
            }
            var ex = await FirstOrDefaultAsync(i => i.name == dto.name  || (i.telephone == dto.telephone && !string.IsNullOrEmpty(i.telephone)));
            if (ex != null)
            {
                if (ex.name == dto.name)
                {
                    result.status = RegisterUserResultType.ExistAccount;
                    return result;
                }
                else
                {
                    result.status = RegisterUserResultType.ExistTelephone;
                    return result;
                }
            }
            var user = Mapper.Map<UserDto, User>(dto);
            var dbUser = await base.InsertAsync(user);
            await UnitOfWork.CommitAsync();

            if (dbUser != null)
            {
                result.status = RegisterUserResultType.Success;
                return result;
            }
            result.status = RegisterUserResultType.Fail;
            return result;
        }
    }
}
