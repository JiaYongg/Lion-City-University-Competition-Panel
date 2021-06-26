using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB2021Apr_P01_T01.DAL;
using WEB2021Apr_P01_T01.Models;

namespace WEB2021Apr_P01_T01.Controllers
{
    public class AdminController : Controller
    {
        public CompetitionDAL compyContext = new CompetitionDAL();

        // GET: AdminController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AdminController/Details/5
        public ActionResult CreateCompetition()
        {
            return View();
        }

        public ActionResult Create(IFormCollection formData)
        {
            string CompetitionName = formData["CompetitionName"];
            int AreaOfInterest = Convert.ToInt32(formData["AreaOfInterest"]);
            DateTime StartDate = Convert.ToDateTime(formData["StartDate"]);
            DateTime EndDate = Convert.ToDateTime(formData["EndDate"]);
            DateTime ResultsReleasedDate = Convert.ToDateTime(formData["ResultsReleasedDate"]);

            //criteriaContext.AddCriteria(compId, nameList[i], weightageList[i]);
            compyContext.AddCompetition(AreaOfInterest, CompetitionName, StartDate, EndDate, ResultsReleasedDate);
            return RedirectToAction("CreateCompetition");
        }

        // GET: AdminController/Create
        public ActionResult AreaOfInterest()
        {
            return View("AreaOfInterest");
        }

        

        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
