using Eason.EntityFramework.Entities.Banner;
using Eason.EntityFramework.Repositories;
using Eason.Mvc;
using Eason.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Eason.Web.Controllers
{
    public class BannerController : EBaseController
    {



        // GET: Banner
        public async System.Threading.Tasks.Task<ActionResult> Video(int? pageIndex, int? pageSize)
        {
            EasonRepository<VideoBanner, long> vdo = new EasonRepository<VideoBanner, long>();
            var result = new ResultModel();

           
            if (pageIndex == null || pageIndex < 0)
            {
                result.Code = 1002;
                result.Message = " pageIndex ";
                return Jsonp(result, JsonRequestBehavior.AllowGet);
            }
            if (pageSize == null || pageSize < 0)
            {
                result.Code = 1002;
                result.Message = " pageSize ";
                return Jsonp(result, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var list = (await vdo.GetAllListAsync((int)pageIndex, (int)pageSize, m => m.status == 0)).ToList();
                if (list != null && list.Count() > 0)
                {
                   
                    result.Code = 0;
                    result.Message = string.Empty;
                    result.Data = list;
                    result.allpageNum = await vdo.CountAsync(m => m.status == 0);
                    return Jsonp(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result.Code = 0;
                    result.Message = "not found";
                    return Jsonp(result, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                result.Code = 1001;
                result.Message = ex.Message;
                return Jsonp(result, JsonRequestBehavior.AllowGet);
            }

        }
        public async System.Threading.Tasks.Task<ActionResult> Info(int? pageIndex, int? pageSize)
        {
            EasonRepository<InfoBanner, long> inf = new EasonRepository<InfoBanner, long>();
            var result = new ResultModel();

            if (pageIndex == null || pageIndex < 0)
            {
                result.Code = 1002;
                result.Message = " pageIndex ";
                return Jsonp(result, JsonRequestBehavior.AllowGet);
            }
            if (pageSize == null || pageSize < 0)
            {
                result.Code = 1002;
                result.Message = " pageSize ";
                return Jsonp(result, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var list = (await inf.GetAllListAsync((int)pageIndex, (int)pageSize, m => m.status == 0)).ToList();
                if (list != null && list.Count() > 0)
                {
                    ;
                    result.Code = 0;
                    result.Message = string.Empty;
                    result.Data = list;
                    result.allpageNum = await inf.CountAsync(m => m.status == 0);
                    return Jsonp(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result.Code = 0;
                    result.Message = "not found";
                    return Jsonp(result, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                result.Code = 1001;
                result.Message = ex.Message;
                return Jsonp(result, JsonRequestBehavior.AllowGet);
            }

        }
        public async System.Threading.Tasks.Task<ActionResult> Index(int? pageIndex, int? pageSize)
        {
            EasonRepository<IndexBanner, long> idx = new EasonRepository<IndexBanner, long>(); ;
            var result = new ResultModel();

          
            if (pageIndex == null || pageIndex < 0)
            {
                result.Code = 1002;
                result.Message = " pageIndex ";
                return Jsonp(result, JsonRequestBehavior.AllowGet);
            }
            if (pageSize == null || pageSize < 0)
            {
                result.Code = 1002;
                result.Message = " pageSize ";
                return Jsonp(result, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var list = (await idx.GetAllListAsync((int)pageIndex, (int)pageSize, m => m.status == 0)).ToList();
                if (list != null && list.Count() > 0)
                {
                    ;
                    result.Code = 0;
                    result.Message = string.Empty;
                    result.Data = list;
                    result.allpageNum = await idx.CountAsync(m => m.status == 0);
                    return Jsonp(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result.Code = 0;
                    result.Message = "not found";
                    return Jsonp(result, JsonRequestBehavior.AllowGet);
                }

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