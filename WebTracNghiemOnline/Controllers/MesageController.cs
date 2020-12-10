using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebTracNghiemOnline.Controllers
{
    public class MesageController : Controller
    {
        // GET: Mesage
        public ActionResult Mesage(double? Score)
        {
            ViewBag.Score = Score;
            return View();
        }
    }
}