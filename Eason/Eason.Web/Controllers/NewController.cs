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
    public class NewController : EBaseController
    {
        private EasonRepository<Article, long> repository;
        public NewController()
        {
            this.repository = new EasonRepository<Article, long>();
        }
        // GET: New
        public async System.Threading.Tasks.Task<ActionResult> IndexAsync(long? CategoryCode, int? pageIndex, int? pageSize)
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
                var allC = await GetAllCategoriesByIdAsync((long)CategoryCode);
                var lst = repository.GetAllList((int)pageIndex, (int)pageSize, m => m.status == 0, m => m.categoryId, allC.ToArray());
                EasonRepository<ArticleCategory, long> category = new EasonRepository<ArticleCategory, long>();
                if (lst != null && lst.Count() > 0)
                {
                    var data = Mapper.Map<IQueryable<Article>, IList<ArticleListModel>>(lst);
                    result.Code = 0;
                    result.Message = string.Empty;
                    result.Data = data;
                    result.allpageNum = repository.GetAllList(m => m.status == 0, m => m.categoryId, allC.ToArray()).Count();
                    result.ChildName = category.GetAll(m => m.parentId == CategoryCode);
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

        public async System.Threading.Tasks.Task<ActionResult> SearchAsync(string CCont, int? pageIndex, int? pageSize)
        {
            var result = new ResultModel();
            if (string.IsNullOrEmpty(CCont))
            {
                result.Code = 1002;
                result.Message = "CCont";
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
                var lst = await repository.GetAllListAsync((int)pageIndex, (int)pageSize, m => (m.creatorName.Contains(CCont) || m.title.Contains(CCont) || m.contents.Contains(CCont)) && m.status == 0);
                if (lst != null && lst.Count() > 0)
                {
                    var data = Mapper.Map<IQueryable<Article>, IList<ArticleListModel>>(lst);
                    result.Code = 0;
                    result.Message = string.Empty;
                    result.Data = data;
                    result.allpageNum = lst.Count();
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


        public async System.Threading.Tasks.Task<ActionResult> ItemAsync(long? id)
        {
            var result = new ResultModel();

            if (id == null || id < 0)
            {
                result.Code = 1002;
                result.Message = " id ";
                return Jsonp(result, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var item = await repository.SingleAsync(m => m.id == id && m.status == 0);
                if (item != null)
                {
                    item.readNum += 1;
                    await repository.UpdateAsync(item);
                    await repository.UnitOfWork.CommitAsync();
                    var data = Mapper.Map<Article, ArticleItemModel>(item);
                    result.Code = 0;
                    result.Message = string.Empty;
                    result.Data = data;
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


        public async System.Threading.Tasks.Task<ActionResult> AuthorAsync(long? id, int? pageIndex, int? pageSize)
        {
            var result = new ResultModel();

            if (id == null || id < 0)
            {
                result.Code = 1002;
                result.Message = " id ";
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
                var list = await repository.GetAllListAsync((int)pageIndex, (int)pageSize, m => m.creatorId == id && m.status == 0);
                if (list != null && list.Count() > 0)
                {
                    var data = Mapper.Map<IQueryable<Article>, IList<ArticleListModel>>(list);
                    result.Code = 0;
                    result.Message = string.Empty;
                    result.Data = data;
                    result.allpageNum = (await repository.GetAllListAsync(m => m.creatorId == id && m.status == 0)).Count();
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
        private List<long> GetChild(long parentId, IList<ArticleCategory> oldList, List<long> NewList)
        {
            var child = oldList.Where(m => m.parentId == parentId).ToList();
            if (child != null && child.Count() > 0)
            {
                foreach (var item in child)
                {
                    NewList.Add(item.id);
                    oldList.Remove(item);
                    GetChild(item.id, oldList, NewList);
                }

            }
            return NewList;
        }

        private async Task<IList<long>> GetAllCategoriesByIdAsync(long categoryid)
        {

            var service = new EasonRepository<ArticleCategory, long>();
            var oldList = await service.GetAllListAsync();
            var newList = new List<long>();
            if (!newList.Contains(categoryid))
            {
                newList.Add(categoryid);
                GetChild(categoryid, oldList, newList);

            }
            return newList;
        }


        public ActionResult Hot(long? CategoryCode, int? pageIndex, int? pageSize)
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

                var lst = repository.Context.Set<Article>().Where(m => m.categoryId == CategoryCode).OrderByDescending(m => m.readNum).Skip((int)pageIndex * (int)pageSize).Take((int)pageSize);
                if (lst != null && lst.Count() > 0)
                {
                    var data = Mapper.Map<IQueryable<Article>, IList<ArticleListModel>>(lst);
                    result.Code = 0;
                    result.Message = string.Empty;
                    result.Data = data;
                    result.allpageNum = repository.Context.Set<Article>().Where(m => m.categoryId == CategoryCode).OrderByDescending(m => m.readNum).Count();
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