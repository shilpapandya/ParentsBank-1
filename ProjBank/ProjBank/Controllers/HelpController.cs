using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjBank.Controllers
{
    public class HelpController : Controller
    {
        [ActionName("financial-resources")]
        public ActionResult Index()
        {
            return View();
        }
    }
}