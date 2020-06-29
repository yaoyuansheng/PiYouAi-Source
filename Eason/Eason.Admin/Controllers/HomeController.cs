using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eason.Admin.Controllers
{
    [Authorize(Roles = "Administrots")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
      
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }

    internal class Department
    {
        public string ID { get; set; }

        public string Name { get; set; }
        public string Level { get; set; }
        public string Desc { get; set; }
    }
}