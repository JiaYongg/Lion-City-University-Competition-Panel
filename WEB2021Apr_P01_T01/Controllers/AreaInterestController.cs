using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB2021Apr_P01_T01.Models;
using WEB2021Apr_P01_T01.DAL;

namespace WEB2021Apr_P01_T01.Controllers
{
    public class AreaInterestController : Controller
    {
        private AoiDAL aoiContext = new AoiDAL();

        // GET: AreaInterestController
        public ActionResult Index()
        {
            string role = (string)HttpContext.Session.GetString("Role");
            if (role != "LCU Admin")
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                List<AreaInterest> aoiList = aoiContext.GetAreaInterests();

                return View(aoiList);
            }
        }

        //// GET: AreaInterestController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: AreaInterestController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: AreaInterestController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: AreaInterestController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: AreaInterestController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: AreaInterestController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: AreaInterestController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
