using Eason.Admin.Models;
using Eason.EntityFramework.Entities.News;
using Eason.EntityFramework.Repositories;
using Eason.Extension;
using Eason.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Eason.Admin.Controllers
{
    [Authorize]
    public class CategoryController : EBaseController
    {
        const string admin = "Administrots";
        public CategoryController()
        {
            easonRepository = new EasonRepository<ArticleCategory, long>();
        }
        private EasonRepository<ArticleCategory, long> easonRepository;
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

        public JsonpResult List(int offset, int limit, string search)
        {
            IList<ArticleCategory> oldList;
            if (string.IsNullOrEmpty(search))
            {
                oldList = easonRepository.GetAllList(offset, limit).ToList();
            }
            else
            {
                oldList = easonRepository.GetAllList(offset, limit, m => m.title.Contains(search)).ToList();
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



            var total = easonRepository.Count();
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
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var category = new ArticleCategory();
                category.title = model.title;
                category.sort = 99;
                category.status = 0;
                category.parentId = model.parentId;
                category.depth = model.parentId == 0 ? 0 : easonRepository.Get(model.parentId).depth + 1;
                category.creatorId = long.Parse((User.Identity as ClaimsIdentity).Claims.FirstOrDefault(m => m.Type == ClaimTypes.Sid).Value);
                category.creatorName = User.Identity.Name;
                category.creationTime = DateTime.Now;
                await easonRepository.InsertAsync(category);
                await easonRepository.UnitOfWork.CommitAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> Delete(string strid)
        {
            if (string.IsNullOrEmpty(strid))
            {
                return Jsonp(new { success = true }, JsonRequestBehavior.AllowGet);

            }
            long[] ids = Array.ConvertAll(strid.Split(','), s => long.Parse(s));
            if (ids != null && ids.Length > 0)
            {
                var children = easonRepository.GetAll().WhereIn(m => m.parentId, ids);
                if (children != null && children.Count() > 0)
                {
                    return Jsonp(new { notallow = true }, JsonRequestBehavior.AllowGet);
                }
                var lst = easonRepository.GetAll().WhereIn(m => m.id, ids);
                foreach (var item in lst)
                {
                    easonRepository.Delete(item);
                }

                await easonRepository.UnitOfWork.CommitAsync();
            }
            return Jsonp(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(CategoryModel model)
        {
            var entity = await easonRepository.GetAsync(model.id);
            if (entity != null && entity.title != model.title)
            {
                entity.title = model.title;
                await easonRepository.UpdateAsync(entity);
                await easonRepository.UnitOfWork.CommitAsync();
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var entity = await easonRepository.GetAsync((long)id);
            if (entity != null)
            {
                return View(entity);
            }

            return RedirectToAction("Index");
        }

    }
}