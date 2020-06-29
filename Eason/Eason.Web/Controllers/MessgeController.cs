using Aliyun.MNS;
using Aliyun.MNS.Model;
using Eason.Application.Implement;
using Eason.Application.Interfaces;
using Eason.EntityFramework.Entities.Authorization;
using Eason.EntityFramework.Entities.Message;
using Eason.EntityFramework.Repositories;
using Eason.Mvc;
using Eason.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Eason.Web.Controllers
{
    public class MessgeController : EBaseController
    {
        private EasonRepository<ShortMessage, long> repository;
        public MessgeController()
        {
            this.repository = new EasonRepository<ShortMessage, long>();
        }
        [RateLimit(Interval = 50L, CookieName = "est")]
        public async System.Threading.Tasks.Task<ActionResult> Index(string phone)
        {
            var result = new ResultModel();
            if (string.IsNullOrEmpty(phone))
            {
                result.Code = 1001;
                result.Message = " 手机号格式不正确 ";
                return Jsonp(result, JsonRequestBehavior.AllowGet);
            }

            if (!Utility.RegexUtils.IsPhone(phone))
            {
                result.Code = 1002;
                result.Message = " 手机号格式不正确 ";
                return Jsonp(result, JsonRequestBehavior.AllowGet);
            }
            var res = new EasonRepository<User, long>();
            if (res.Count(m => m.telephone == phone) > 0)
            {
                result.Code = 1003;
                result.Message = " 该手机号已经注册过了 ";
                return Jsonp(result, JsonRequestBehavior.AllowGet);

            }
            var date = DateTime.Now.AddMinutes(-1);
            var re = repository.FirstOrDefault(m => m.telephone == phone && m.creationTime > date);
            if (re != null)
            {
                result.Code = 1004;
                result.Message = " 请一分钟后再次发送 ";
                return Jsonp(result, JsonRequestBehavior.AllowGet);

            }
            date = DateTime.Now.AddMinutes(60);
            if (repository.Count(m => m.ip == UserHostAddress && m.creationTime > date) > 1000)
            {
                result.Code = 1005;
                result.Message = " 该IP一小时内发送的短信数量过多";
                return Jsonp(result, JsonRequestBehavior.AllowGet);

            }
            IMessageService service = new MessageService();
            try
            {
                var msg = new ShortMessage();
                msg.creatorId = 1;
                msg.creatorName = "注册用户";
                msg.creationTime = DateTime.Now;
                msg.telephone = phone;
                msg.code = new Random().Next(1000, 9999);
                msg.content = "验证码" + msg.code + "，您正在注册成为匹优爱用户，感谢您的支持！";
                msg.ip = UserHostAddress;
                await repository.InsertAsync(msg);
                await repository.UnitOfWork.CommitAsync();
                service.Register(msg.code.ToString(), msg.telephone);
                result.Code = 0;
                result.Message = string.Empty;
                result.Data = "发送成功！";
                return Jsonp(result, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                result.Code = 1001;
                result.Message = ex.Message;
                return Jsonp(result, JsonRequestBehavior.AllowGet);
            }

        }


    }
}