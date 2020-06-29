using Eason.EntityFramework.Entities.Banner;
using Eason.EntityFramework.Entities.School;
using Eason.EntityFramework.Repositories;
using Eason.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Eason.Admin.Controllers
{
    public class CourseController : EBaseController
    {
        public CourseController()
        {
            repository = new EasonRepository<Course, long>();
        }
        private EasonRepository<Course, long> repository;
        // GET: Course
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List(int offset, int limit, string search)
        {

            IQueryable<Course> articles;
            if (string.IsNullOrEmpty(search))
            {
                articles = repository.GetAllList(offset, limit);
            }
            else
            {
                articles = repository.GetAllList(offset, limit, m => m.ktitle.Contains(search));
            }
            var total = repository.Count();
            return Jsonp(new { total = total, rows = articles }, JsonRequestBehavior.AllowGet);
        }  // GET: Users/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var user = await repository.FirstOrDefaultAsync(m => m.id == id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(Course user)
        {
            if (user != null)
            {
                user.creatorId = long.Parse((User.Identity as ClaimsIdentity).Claims.FirstOrDefault(m => m.Type == ClaimTypes.Sid).Value);
                user.creatorName = User.Identity.Name;
                user.creationTime = DateTime.Now;
            }
            if (ModelState.IsValid)
            {
                repository.Insert(user);
                repository.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var user = await repository.FirstOrDefaultAsync(m => m.id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(long id, string kpic, string kurl, string ktitle, string kpop, string kspic, string kteach, string kshi, string kmiao, string kmony, string ty)
        {

            var user = repository.FirstOrDefault(m => m.id == id);
            if (user == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    user.kpic = kpic;
                    user.ktitle = ktitle;
                    user.kpop = kpop;
                    user.kspic = kspic;
                    user.kteach = kteach;
                    user.kshi = kshi;
                    user.kmiao = kmiao;
                    user.kmony = kmony;
                    user.ty = ty;
                    user.kurl = kurl;
                    repository.Update(user);
                    repository.UnitOfWork.Commit();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.id))
                    {
                        return HttpNotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(user);
        }



        // POST: Users/Delete/5
        [ActionName("Delete")]

        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            var user = await repository.FirstOrDefaultAsync(m => m.id == id);
            repository.Delete(user);
            repository.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        private bool UserExists(long id)
        {
            return repository.Count(e => e.id == id) > 0;
        }
    }
}