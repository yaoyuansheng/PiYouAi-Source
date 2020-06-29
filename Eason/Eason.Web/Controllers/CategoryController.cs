using Eason.EntityFramework.Entities.News;
using Eason.EntityFramework.Repositories;
using Eason.Mvc;
using Eason.Web.Models;
using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eason.Web.Controllers
{
    public class CategoryController : EBaseController
    {
        private EasonRepository<ArticleCategory, long> repository;
        public CategoryController()
        {
            this.repository = new EasonRepository<ArticleCategory, long>();
        }
        public async System.Threading.Tasks.Task<ActionResult> ItemsAsync(long? CategoryCode)
        {
            var result = new ResultModel();
            if (CategoryCode == null || CategoryCode < 0)
            {
                result.Code = 1002;
                result.Message = "CategoryCode  ";
                return Jsonp(result, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var lst = await repository.GetAllListAsync(m => m.parentId == CategoryCode && m.status == 0);
                if (lst != null && lst.Count() > 0)
                {
                    var data = lst;
                    result.Code = 0;
                    result.Message = string.Empty;
                    result.Data = data;
                    result.allpageNum = lst.Count();
                    return Jsonp(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result.Code = 1003;
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

        public JsonpResult ListAsync(int offset, int limit, string search)
        {
            IList<ArticleCategory> oldList;
            if (string.IsNullOrEmpty(search))
            {
                oldList = repository.GetAllList(offset, limit).ToList();
            }
            else
            {
                oldList = repository.GetAllList(offset, limit, m => m.title.Contains(search)).ToList();
            }
            //原始根
            var roots = oldList.Where(i => i.parentId == 0);
            //获取所有父亲
            var parents = oldList.GroupBy(m => m.parentId).Select(x => x.FirstOrDefault()).Where(i => i.parentId != 0).OrderBy(m => m.id).ToList();
            var newList = new List<ArticleCategory>();

            foreach (var item in roots)
            {
                parents.Add(item);
                // newList.Add(item);

            }
            parents = parents.OrderBy(m => m.parentId).ToList();
            for (int i = 0; i < parents.Count(); i++)
            {
                if (!newList.Contains(parents[i]))
                {
                    newList.Add(parents[i]);
                    GetChild(parents[i].id, oldList, newList);

                }
            }
            var total = repository.Count();
            return Jsonp(new { total = total, rows = newList }, JsonRequestBehavior.AllowGet);
        }
        private List<ArticleCategory> GetChild(long parentId, IList<ArticleCategory> oldList, List<ArticleCategory> NewList)
        {
            var child = oldList.Where(m => m.parentId == parentId).ToList();
            if (child != null && child.Count() > 0)
            {
                foreach (var item in child)
                {
                    NewList.Add(item);
                    oldList.Remove(item);
                    GetChild(item.id, oldList, NewList);
                }

            }
            return NewList;
        }

    }
}