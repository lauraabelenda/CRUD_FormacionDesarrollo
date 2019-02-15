using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationPrueba.ViewModels;

namespace WebApplicationPrueba.Controllers
{
    public class HomeController : Controller
    {
        
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
      
        public ActionResult Index()
        {
            log.Error("This could be an error");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            log.Debug("This could be an error");
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Departamento()
        {
            return RedirectToAction("/");
            
        }
    }
}