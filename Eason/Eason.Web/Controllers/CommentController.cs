using Eason.EntityFramework.Entities.News;
using Eason.EntityFramework.Repositories;
using Eason.Mvc;
using Eason.Web.Models;
using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Eason.Web.Controllers
{
    public class CommentController : EBaseController
    {
        private EasonRepository<ArticleComment, long> repository;
        public CommentController()
        {
            this.repository = new EasonRepository<ArticleComment, long>();
        }
        public async System.Threading.Tasks.Task<ActionResult> ListAsync(long? id, int? pageIndex, int? pageSize)
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
                var list = (await repository.GetAllListAsync((int)pageIndex, (int)pageSize, m => m.articleId == id && m.status == 0)).ToList();
                if (list != null && list.Count > 0)
                {
                    var data = Mapper.Map<IList<ArticleComment>, IList<CommentListModel>>(list);
                    result.Code = 0;
                    result.Message = string.Empty;
                    result.Data = data;
                    result.allpageNum = await repository.CountAsync(m => m.articleId == id && m.status == 0);
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

        [Authorize]
        public async System.Threading.Tasks.Task<ActionResult> AddAsync(long? id, string ccont, long? parentId)
        {
            var result = new ResultModel();

            if (id == null || id < 0)
            {
                result.Code = 1002;
                result.Message = " id ";
                return Jsonp(result, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(ccont))
            {
                result.Code = 1002;
                result.Message = " ccont ";
                return Jsonp(result, JsonRequestBehavior.AllowGet);
            }

            try
            {
                ArticleComment comment = new ArticleComment()
                {
                    articleId = (long)id,
                    contents = ccont,
                    creatorId = long.Parse((User.Identity as ClaimsIdentity).Claims.FirstOrDefault(m => m.Type == ClaimTypes.Sid).Value),
                    creationTime = DateTime.Now,
                    creatorName = User.Identity.Name,
                    imgUrl = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(m => m.Type == ClaimTypes.Uri).Value,
                    parentId = parentId ?? 0,
                    status = 2
                };
                await repository.InsertAsync(comment);
                var add = await repository.UnitOfWork.CommitAsync();
                if (add == 1)
                {
                    result.Code = 0;
                    result.Message = string.Empty;
                    result.Data = comment;
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
        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }
    }
}