using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB2021Apr_P01_T01.DAL;
using WEB2021Apr_P01_T01.Models;

namespace WEB2021Apr_P01_T01.Controllers
{
    public class AdminController : Controller
    {
        public CompetitionDAL compyContext = new CompetitionDAL();
        private AoiDAL aoiContext = new AoiDAL();

        private List<SelectListItem> aoiList = new List<SelectListItem>();
        private List<AreaInterest> areaInterest = new List<AreaInterest>();


        public AdminController()
        {
            for (int i=1;i<=10;i++)
            {
                aoiList.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString(),
                });
            }
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
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

            compyContext.AddCompetition(competition, aoiID);
            return RedirectToAction("CreateCompetition");
        }


        // GET: AdminController/Create
        public ActionResult AreaOfInterest()
        {
            List<AreaInterest> aoiList = aoiContext.GetAreaInterests();
            List<string> aoiNames = new List<string>();
            foreach (var item in aoiList)
            {
                aoiNames.Add(item.Name);
            }

            ViewData["totalAOI"] = aoiList.Count();
            ViewData["aoiName"] = aoiNames;
            return View();
        }

        public ActionResult AddInterest(IFormCollection formData)
        {
            string aoiName = formData["aoiName"];

            compyContext.AddAreaInterest(aoiName);
            return RedirectToAction("AreaOfInterest");
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
