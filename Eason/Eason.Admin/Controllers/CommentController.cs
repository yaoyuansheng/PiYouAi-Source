using Eason.EntityFramework.Entities.News;
using Eason.EntityFramework.Repositories;
using Eason.Extension;
using Eason.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Eason.Admin.Controllers
{
    public class CommentController : EBaseController
    {
        public CommentController()
        {
            repository = new EasonRepository<ArticleComment, long>();
        }
        private EasonRepository<ArticleComment, long> repository;
        // GET: Common
        public ActionResult Index(long? id)
        {
            ViewBag.id = id;
            return View();
        }



        public ActionResult List(int offset, int limit, string search, long? id)
        {
            if (id == null)
            {
                return Jsonp(new { total = 0, rows = 0 }, JsonRequestBehavior.AllowGet);
            }
            IQueryable<ArticleComment> articles;
            if (string.IsNullOrEmpty(search))
            {
                articles = repository.GetAllList(offset, limit, m => m.articleId == id);
            }
            else
            {
                articles = repository.GetAllList(offset, limit, m => (m.contents.Contains(search) || m.creatorName.Contains(search)) && m.articleId == id);
            }
            var total = repository.Count();
            return Jsonp(new { total = total, rows = articles }, JsonRequestBehavior.AllowGet);
        }

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
        public async Task<JsonpResult> Active(string strid, byte status)
        {
            if (string.IsNullOrEmpty(strid))
            {
                return Jsonp(new { success = true }, JsonRequestBehavior.AllowGet);

            }
            long[] ids = Array.ConvertAll(strid.Split(','), s => long.Parse(s));
            var lst = repository.GetAll().WhereIn(m => m.id, ids);
            foreach (var item in lst)
            {
                item.status = status;
            }
            await repository.UnitOfWork.CommitAsync();
            return Jsonp(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}