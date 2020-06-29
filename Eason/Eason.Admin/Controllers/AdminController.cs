using Eason.Admin.Models;
using Eason.Application;
using Eason.Core.Users;
using Eason.EntityFramework.Entities.Authorization;
using Eason.EntityFramework.Repositories;
using Eason.Extension;
using Eason.Mvc;
using Estone.Application.Cas.Interfaces;
using ExpressMapper;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Eason.Admin.Controllers
{

    public class AdminController : EBaseController
    {
        private const string admin = "Administrots";
        IUserService userService;
        private UserRepository repository;
        public AdminController(UserRepository userRepository, IUserService userService)
        {
            this.repository = userRepository;
            this.userService = userService;
        }
        [Authorize]
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        [Authorize]
        public JsonpResult List(int offset, int limit, string search)
        {
            IQueryable<User> users;
            if (string.IsNullOrEmpty(search))
            {
                users = repository.GetAllList(offset, limit, m => m.isAdmin == 1);
            }
            else
            {
                users = repository.GetAllList(offset, limit, m => (m.name.Contains(search) || m.email.Contains(search) || m.telephone.Contains(search)) && m.isAdmin == 1);
            }
            var result = Mapper.Map<IQueryable<User>, IList<UserDto>>(users);
            var total = repository.Count();
            return Jsonp(new { total = total, rows = result }, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public async Task<JsonpResult> Delete(string strid)
        {
            if (string.IsNullOrEmpty(strid))
            {
                return Jsonp(new { success = true }, JsonRequestBehavior.AllowGet);

            }
            long[] ids = Array.ConvertAll(strid.Split(','), s => long.Parse(s));
            if (ids != null && ids.Length > 0)
            {
                var lst = repository.GetAll().WhereIn(m => m.id, ids);
                foreach (var item in lst)
                {
                    repository.Delete(item);
                }
                await repository.UnitOfWork.CommitAsync();
            }
            return Jsonp(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public async Task<JsonpResult> Active(string strid, int status)
        {
            if (string.IsNullOrEmpty(strid))
            {
                return Jsonp(new { success = true }, JsonRequestBehavior.AllowGet);

            }
            long[] ids = Array.ConvertAll(strid.Split(','), s => long.Parse(s));
            var lst = repository.GetAll().WhereIn(m => m.id, ids);
            foreach (var item in lst)
            {
                item.isActive = (byte)(status == 0 ? 0 : 1);
            }
            await repository.UnitOfWork.CommitAsync();
            return Jsonp(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }



        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create(UserDto model)
        {
            if (model == null)
            {
                ModelState.AddModelError("", "数据不允许为空!!");
            }
            if (string.IsNullOrEmpty(model.password))
            {
                ModelState.AddModelError("", "密码不允许为空!!");
            }
            if (string.IsNullOrEmpty(model.name))
            {
                ModelState.AddModelError("", "账户不允许!");
            }
            if (await repository.GetUserAsync(model.name) != null)
            {
                ModelState.AddModelError("", "账户：" + model.name + "已经被占用了!");
            }
            if (ModelState.IsValid)
            {
                User user = new User()
                {
                    name = model.name
                    ,
                    creationTime = DateTime.Now,
                    lastLoginTime = DateTime.Parse("2099-01-01"),
                    isActive = 0,
                    isDelete = 0,
                    password = model.password.Encrypt(),
                    telephone = model.telephone,
                    isAdmin = 1
                };

                repository.Insert(user);
                repository.UnitOfWork.Commit();
                var ttt =await repository.FirstOrDefaultAsync(m => m.isAdmin == 1 && m.name == model.name);
                if (ttt!=null)
                {
                    UserRole role = new UserRole();
                    role.roleId = 3;
                    role.userId = ttt.id;
                    role.status = 0;
                    role.creatorId = long.Parse((User.Identity as ClaimsIdentity).Claims.FirstOrDefault(m => m.Type == ClaimTypes.Sid).Value);
                    role.creatorName = User.Identity.Name;
                    role.creationTime = DateTime.Now;
                    var res = new EasonRepository<UserRole, long>();
                    res.Insert(role);
                    await res.UnitOfWork.CommitAsync();
                }
            
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize(Roles = "Administrots")]
        public async Task<ActionResult> Edit(long id)
        {
            var user = await repository.GetAsync(id);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        [Authorize(Roles = "Administrots")]
        public async Task<ActionResult> Edit(long id, string name, string email, string password, string telephone)
        {
            var user = await repository.GetAsync(id);
            if (user != null)
            {
                user.name = name;
                user.email = string.IsNullOrEmpty(email) ? user.email : email;
                user.password = string.IsNullOrEmpty(password) ? user.password : password.Encrypt();
                user.telephone = string.IsNullOrEmpty(telephone) ? user.telephone : telephone;
                var result = await repository.UpdateAsync(user);
                await repository.UnitOfWork.CommitAsync();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View(user);
        }


        private IAuthenticationManager AuthenticationManager { get { return HttpContext.GetOwinContext().Authentication; } }
        // GET: Login
        private string LoginErrorMessage(LoginResultType type)
        {
            switch (type)
            {
                case LoginResultType.Success:
                    return "登陆成功！";

                case LoginResultType.InvalidAccount:
                    return "用户名不正确！";
                case LoginResultType.InvalidPassword:
                    return "密码不正确！";

                case LoginResultType.UserIsNotActive:
                    return "用户没有激活！";
                case LoginResultType.LoginOut:
                    return "未知用户信息！";
                case LoginResultType.LockedOut:
                    return "用户被锁定！";
                default:
                    return "登陆失败！";
            }
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(LoginModel login)
        {
            if (login != null && ModelState.IsValid)
            {
                var result = await userService.GetLoginResultAsync(login.account, login.password);
                result.message = LoginErrorMessage(result.status);
                if (result.status == LoginResultType.Success)
                {
                    var permisson = await repository.GetUserPermissionsAsync(result.name);
                    if (permisson != null && permisson.roles.Contains(admin))
                    {
                        ClaimsIdentity _identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
                        _identity.AddClaim(new Claim(ClaimTypes.Name, result.name));
                        _identity.AddClaim(new Claim(ClaimTypes.Sid, result.userId.ToString()));
                        _identity.AddClaim(new Claim(ClaimTypes.Role, admin));
                        // _identity.AddClaim(new Claim(ClaimTypes.Name, result.name));
                        AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                        AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, _identity);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        result.message = LoginErrorMessage(LoginResultType.InvalidAccount);
                    }

                }
                return Content("<script>alert('" + result.message + "');window.location.href='/User/Login'</script>");
            }
            var errmessage = ModelState.AllModelStateErrors();
            return Content("<script>alert('" + errmessage.FirstOrDefault() + "');window.location.href='/User/Login'</script>");
            // return View();
        }


        public ActionResult Out()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login");
        }

    }
}