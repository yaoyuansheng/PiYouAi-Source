using Eason.EntityFramework.Entities.Activity;
using Eason.EntityFramework.Repositories;
using Eason.Mvc;
using Eason.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eason.Web.Controllers
{
    public class ActivityController : EBaseController
    {
        private EasonRepository<Forward, long> repository;
        public ActivityController()
        {
            this.repository = new EasonRepository<Forward, long>();
        }
        // GET: Acitity
        public async System.Threading.Tasks.Task<ActionResult> Index(string hdname)
        {
            var result = new ResultModel();
            if (string.IsNullOrEmpty(hdname))
            {
                result.Code = 1002;
                result.Message = " hdname ";
                return Jsonp(result, JsonRequestBehavior.AllowGet);
            }
        
            var entity = repository.FirstOrDefault(i => i.hdname == hdname);
            if (entity != null)
            {
                entity.hdnum += 1;
                await repository.UpdateAsync(entity);
                await repository.UnitOfWork.CommitAsync();
            }
            else
            {
                entity = new Forward();
                entity.hdname = hdname;
                entity.hdnum = 1;
                entity.creationTime = DateTime.Now;
                entity.creatorId = 0;
                entity.creatorName = "分享用户";
                entity.status = 0;
                repository.Insert(entity);
                await repository.UnitOfWork.CommitAsync();
            }
            result.Code = 0;
            result.Message = "success";
            return Jsonp(result, JsonRequestBehavior.AllowGet);
        }
    }
}