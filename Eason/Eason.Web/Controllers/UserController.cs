using Eason.Core.Users;
using Eason.Mvc;
using Eason.Web.Models;
using Estone.Application.Cas.Interfaces;
using ExpressMapper;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Eason.Web.Controllers
{
    public class UserController : Eason.Mvc.EBaseController
    {
        IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        private IAuthenticationManager AuthenticationManager { get { return HttpContext.GetOwinContext().Authentication; } }
        public ActionResult Index()
        {
            return View();
        }
        private string LoginErrorMessage(LoginResultType type)
        {
            switch (type)
            {
                case LoginResultType.Success:
                    return "登陆成功！";

                case LoginResultType.InvalidAccount:
                    return "手机号不正确！";
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
        public async Task<JsonpResult> Login(LoginModel login)
        {
            if (login != null && ModelState.IsValid)
            {
                var result = await userService.GetLoginResultByPhoneAsync(login.phone, login.password);
                result.message = LoginErrorMessage(result.status);
                if (result.status == LoginResultType.Success)
                {
                    ClaimsIdentity _identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
                    _identity.AddClaim(new Claim(ClaimTypes.Name, result.name ?? string.Empty));
                    _identity.AddClaim(new Claim(ClaimTypes.Uri, result.imgUrl));
                    _identity.AddClaim(new Claim(ClaimTypes.Sid, result.userId.ToString()));
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, _identity);
                }
                return Jsonp(result, JsonRequestBehavior.AllowGet);
            }
            var errmessage = ModelState.AllModelStateErrors();
            return Jsonp(new LoginResult() { status = LoginResultType.InvalidAccount, message = errmessage.FirstOrDefault() }, JsonRequestBehavior.AllowGet);
            // return View();
        }
        private string RegisterErrorMessage(RegisterUserResultType type)
        {
            switch (type)
            {
                case RegisterUserResultType.Success:
                    return "注册成功！";
                case RegisterUserResultType.InvalidAccount:
                    return "用户名不正确！";
                case RegisterUserResultType.InvalidPassword:
                    return "密码不正确！";

                case RegisterUserResultType.ExistAccount:
                    return "账户名重复！";
                case RegisterUserResultType.ExistEmail:
                    return "该邮箱已经被注册！";
                case RegisterUserResultType.ExistTelephone:
                    return "该号码已经被注册！";
                default:
                    return "注册失败！";
            }
        }
        public async Task<JsonpResult> Register(RegisterModel register)
        {
            if (register == null)
            {
                return Jsonp(new RegisterUserResult() { status = RegisterUserResultType.InvalidCode, message = "注册信息不正确" }, JsonRequestBehavior.AllowGet);
            }
            if (register.from != 1 && (register.code <= 0 || register.code >= 99999))
            {
                return Jsonp(new RegisterUserResult() { status = RegisterUserResultType.InvalidCode, message = "验证码不正确" }, JsonRequestBehavior.AllowGet);
            }
            if (ModelState.IsValid)
            {
                if (register.from != 1)
                {
                    var res = new EntityFramework.Repositories.EasonRepository<EntityFramework.Entities.Message.ShortMessage, long>();
                    var date = DateTime.Now.AddMinutes(-5);
                    var t = await res.FirstOrDefaultAsync(m => register.telephone == m.telephone && m.creationTime > date && m.status == 0);

                    if (t == null)
                    {
                        return Jsonp(new RegisterUserResult() { status = RegisterUserResultType.InvalidCode, message = "验证码不存在或已过期！" }, JsonRequestBehavior.AllowGet);
                    }
                    if (t.code != register.code)
                    {
                        return Jsonp(new RegisterUserResult() { status = RegisterUserResultType.InvalidCode, message = "验证码不存在或已过期！" }, JsonRequestBehavior.AllowGet);
                    }
                    if (register.telephone != t.telephone)
                    {
                        return Jsonp(new RegisterUserResult() { status = RegisterUserResultType.InvalidCode, message = "短信验证与提交手机号码不一致！" }, JsonRequestBehavior.AllowGet);

                    }
                }
                var user = Mapper.Map<RegisterModel, UserDto>(register);
                var result = await userService.RegisterResultAsync(user);
                result.message = RegisterErrorMessage(result.status);
                return Jsonp(result, JsonRequestBehavior.AllowGet);
            }
            var errmessage = ModelState.AllModelStateErrors();
            return Jsonp(new RegisterUserResult() { status = RegisterUserResultType.InvalidAccount, message = errmessage.FirstOrDefault() }, JsonRequestBehavior.AllowGet);
        }


        public JsonpResult LoginOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return Jsonp(new LoginResult() { status = LoginResultType.LoginOut }, JsonRequestBehavior.AllowGet);
            // return View();
        }




        public async Task<ActionResult> Wechat(string code, string backurl)
        {
            string weChatUrl = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid=wx89d90324312364fe&secret=cd094379b7a35359a7e6f2d5e614f3c6&code={0}&grant_type=authorization_code", code);
            var getresult = await Utility.HttpUtils.HttpClientAsync(weChatUrl, Utility.HttpMethod.GET, null);
            if (getresult.Contains("errcode"))
            {
                return Redirect(backurl);
            }
            var user = JsonConvert.DeserializeObject<WechatModel>(getresult);
            if (user == null)
            {
                return Redirect(backurl);
            }
            var result = await userService.GetLoginResultAsync(user.openid);

            if (result.status != LoginResultType.Success)
            {
                //if (user.scope.Contains("snsapi_userinfo"))
                //{
                string gi = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}", user.access_token, user.openid);
                var giresult = await Utility.HttpUtils.HttpClientAsync(gi, Utility.HttpMethod.GET, null);
                var inf = JsonConvert.DeserializeObject<WechatInfoModel>(giresult);
                if (inf != null)
                {
                    await Register(new RegisterModel { name = inf.nickname, password = user.openid, imgUrl = inf.headimgurl, openid = inf.openid, from = 1 });
                    result = await userService.GetLoginResultAsync(user.openid);
                }

            }
            ClaimsIdentity _identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
            _identity.AddClaim(new Claim(ClaimTypes.Name, result.name));
            _identity.AddClaim(new Claim(ClaimTypes.Uri, result.imgUrl));
            _identity.AddClaim(new Claim(ClaimTypes.Sid, result.userId.ToString()));
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, _identity);
            return Redirect(backurl);

        }

        [InterfaceAuth]
        public JsonpResult Info()
        {
            return Jsonp(new { login = true, name = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(m => m.Type == ClaimTypes.Name).Value, img = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(m => m.Type == ClaimTypes.Uri).Value }, JsonRequestBehavior.AllowGet);
        }
    }
}