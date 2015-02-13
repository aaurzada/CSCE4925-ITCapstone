using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SQLSolutions.Areas.Admin.Controllers
{
    public class CatalogController : Controller
    {
        // GET: Admin/Catalog
        public ActionResult Index()
        {
            return View();
        }
    }
}