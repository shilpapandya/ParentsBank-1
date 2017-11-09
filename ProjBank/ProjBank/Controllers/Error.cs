using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjBank.Controllers
{
    public class Error : Controller
    {
        [ActionName("PageNotFound")]
        public ActionResult Index()
        {
            return View();
        }
    }
}