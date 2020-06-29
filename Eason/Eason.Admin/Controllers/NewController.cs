using Eason.Core.News;
using Eason.EntityFramework.Entities.News;
using Eason.EntityFramework.Repositories;
using Eason.Extension;
using Eason.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Eason.Admin.Controllers
{
    [Authorize(Roles = "Administrots")]
    public class NewController : EBaseController
    {
        private EasonRepository<Article, long> repository;
        public NewController()
        {
            this.repository = new EasonRepository<Article, long>();
        }
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]

        public async Task<ActionResult> Create(PublishArticle model)
        {
            if (model == null)
            {
                ModelState.AddModelError("", "数据不允许为空!!");
            }
            if (string.IsNullOrEmpty(model.title))
            {
                ModelState.AddModelError("", "标题不允许为空!!");
            }

            //if (string.IsNullOrEmpty(model.outLink))
            //{
            //    model.outLink = "http://www.instwin.com/act/upa/content.html";
            //}

            if (ModelState.IsValid)
            {
                var cate = new EasonRepository<ArticleCategory, long>();
                var ct = await cate.GetAsync(model.categoryId);
                Article art = new Article()
                {
                    title = model.title,
                    contents = model.contents,
                    creationTime = DateTime.Now,
                    status = model.status,
                    categoryId = model.categoryId,
                    //channelId = model.channelId,
                    //typeId = model.typeId,
                    creatorName = User.Identity.Name,
                    imageUrl = model.imageUrl,
                    videoUrl = model.videoUrl,
                    mTitle = model.mTitle,
                    desc = model.desc,
                    categoryName = ct.title,
                    outLink = model.outLink,
                    creatorId = long.Parse((User.Identity as ClaimsIdentity).Claims.FirstOrDefault(m => m.Type == ClaimTypes.Sid).Value)
                };
                await repository.InsertAsync(art);
                await repository.UnitOfWork.CommitAsync();
                return Jsonp(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return View(model);
        }
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var entity = await repository.GetAsync((long)id);
            if (entity != null)
            {
                return View(entity);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Edit(PublishArticle model)
        {
            if (model == null)
            {
                ModelState.AddModelError("", "数据不允许为空!!");
            }

            if (string.IsNullOrEmpty(model.title))
            {
                ModelState.AddModelError("", "标题不允许为空!!");
            }




            var art = await repository.GetAsync(model.id);
            var cate = new EasonRepository<ArticleCategory, long>();
            var ct = await cate.GetAsync(model.categoryId);
            if (art == null)
            {
                ModelState.AddModelError("", "该文章可能已经被删除!");
            }
            if (ModelState.IsValid)
            {


                art.title = model.title;
                art.contents = model.contents;
                art.creationTime = DateTime.Now;
                art.status = model.status;
                art.categoryId = model.categoryId;
                // art.typeId = model.typeId;
                art.creatorName = User.Identity.Name;
                art.imageUrl = model.imageUrl;
                art.videoUrl = model.videoUrl;
                art.mTitle = model.mTitle;
                art.desc = model.desc;
                art.outLink = model.outLink;
                art.categoryName = ct.title;
                art.creatorId = long.Parse((User.Identity as ClaimsIdentity).Claims.FirstOrDefault(m => m.Type == ClaimTypes.Sid).Value);
                await repository.UpdateAsync(art);
                await repository.UnitOfWork.CommitAsync();
                return Jsonp(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return View(model);
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

        public async Task<JsonpResult> List(int offset, int limit, string search, long categoryid)
        {
            IQueryable<Article> articles;
            var total = 0;
            if (string.IsNullOrEmpty(search))
            {
                if (categoryid != 0L)
                {
                    var allC = await GetAllCategoriesByIdAsync(categoryid);
                    articles = repository.GetAllList(offset, limit, m => m.categoryId, allC.ToArray());
                    total = repository.Count(m => m.categoryId, allC.ToArray());
                }
                else
                {
                    articles = repository.GetAllList(offset, limit);
                    total = repository.Count();
                }

            }
            else
            {
                if (categoryid != 0L)
                {
                    var allC = await GetAllCategoriesByIdAsync(categoryid);
                    articles = repository.GetAllList(offset, limit, m => m.title.Contains(search) || m.creatorName.Contains(search), m => m.categoryId, allC.ToArray());
                    total = repository.Count(m => m.title.Contains(search) || m.creatorName.Contains(search), m => m.categoryId, allC.ToArray());
                }
                else
                {
                    articles = repository.GetAllList(offset, limit, m => m.title.Contains(search) || m.creatorName.Contains(search));
                    total = repository.Count(m => m.title.Contains(search) || m.creatorName.Contains(search));

                }
            }
            //var result = Mapper.Map<IQueryable<User>, IList<UserDto>>(users);
          
            return Jsonp(new { total = total, rows = articles }, JsonRequestBehavior.AllowGet);
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