using Eason.EntityFramework.Entities.Message;
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

    public class MessageController : EBaseController
    {
        public MessageController()
        {
            repository = new EasonRepository<TotalMessage, long>();
        }
        private EasonRepository<TotalMessage, long> repository;
        [Authorize(Roles = "Administrots")]
        // GET: TotalMessage
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Administrots")]
        public ActionResult List(int offset, int limit, string search)
        {

            IQueryable<TotalMessage> articles;
            if (string.IsNullOrEmpty(search))
            {
                articles = repository.GetAllList(offset, limit);
            }
            else
            {
                articles = repository.GetAllList(offset, limit, m => m.remark.Contains(search));
            }
            var total = repository.Count();
            return Jsonp(new { total = total, rows = articles }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administrots")]
        public async Task<int> remain()
        {
            var res = new EasonRepository<ShortMessage, long>();
            int result = 0;
            int cost = await res.CountAsync();
            var list = await repository.GetAllListAsync();
            foreach (var item in list)
            {
                result += item.count;
            }
            result = result - cost;
            return result;
        }
        [Authorize(Roles = "Administrots")]
        // GET: Users/Details/5
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
        public ActionResult Create(string tt)
        {
            if (tt == "D1E0765A-55CF-4571-AE18-BA36EA149320")
            {
                return View();
            }
            return RedirectToAction("Index");
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(TotalMessage user,string tt)
        {
            if (tt == "D1E0765A-55CF-4571-AE18-BA36EA149320")
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
        public ActionResult Edit(long id, int count, string remark)
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
                    user.count = count;
                    user.remark = remark;

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