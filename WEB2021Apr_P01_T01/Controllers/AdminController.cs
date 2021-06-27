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
            string competitionName = formData["CompetitionName"];
            string areaOfInterest = formData["AreaOfInterest"];
            DateTime startDate = Convert.ToDateTime(formData["StartDate"]);
            DateTime endDate = Convert.ToDateTime(formData["EndDate"]);
            DateTime resultsReleasedDate = Convert.ToDateTime(formData["ResultsReleasedDate"]);

            Competition competition = new Competition
            {
                CompetitionName = competitionName,
                AoiName = areaOfInterest,
                StartDate = startDate,
                EndDate = endDate,
                ResultsReleaseDate = resultsReleasedDate
            };

            int aoiID = compyContext.AddAreaInterest(competition.AoiName);

            //criteriaContext.AddCriteria(compId, nameList[i], weightageList[i]);
            compyContext.AddCompetition(competition, aoiID);
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
