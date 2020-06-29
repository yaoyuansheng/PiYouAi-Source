using Eason.EntityFramework.Entities.News;
using Eason.EntityFramework.Repositories;
using Eason.Mvc;
using Eason.Web.Models;
using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Eason.Web.Controllers
{

    public class HomeController : EBaseController
    {
        private EasonRepository<Article, long> repository;
        public HomeController()
        {
            this.repository = new EasonRepository<Article, long>();
        }
        public ActionResult Index()
        {
            return Redirect("http://www.piyouai.com/web1/index.html");
        }
        public async Task<ActionResult> Banner(long? CategoryCode, int? pageIndex, int? pageSize)
        {
            var result = new ResultModel();
            if (CategoryCode == null || CategoryCode < 0)
            {
                result.Code = 1002;
                result.Message = "CategoryCode  ";
                return Jsonp(result, JsonRequestBehavior.AllowGet);
            }
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
                var lst = await repository.GetAllListAsync((int)pageIndex, (int)pageSize, m => m.categoryId == CategoryCode && m.status == 0);
                var data = Mapper.Map<IQueryable<Article>, IList<BannerModel>>(lst);
                result.Code = 0;
                result.Message = string.Empty;
                result.Data = data;
                result.allpageNum = lst.Count();
                return Jsonp(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result.Code = 1001;
                result.Message = ex.Message;
                return Jsonp(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult About()
        {
            ViewBag.Message = User.Identity.Name;

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}