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
    public class CompetitorController : Controller
    {
        private CompetitionSubmissionDAL csContext = new CompetitionSubmissionDAL();
        private CompetitionDAL compyContext = new CompetitionDAL();
        private JudgeDAL judgeContext = new JudgeDAL();
        // GET: CompetitorController
        public ActionResult Index()
        {
            ViewData["TryHarder"] = "Try harder next time!";

            var competitorId = HttpContext.Session.GetInt32("userID");

            List<CompetitionSubmission> cList = csContext.competitorCompetitions(Convert.ToInt32(competitorId));
            foreach(var item in cList)
            {
                List<Judge> judgeList = judgeContext.GetCompetitionJudges(item.CompetitionId);
                item.numofJudge = (int)judgeList.Count();
            }
            ViewData["CompetitorCompetitions"] = cList;

            return View();
        }
        
        public ActionResult JoinCompetition(int id)
        {

            // When user clicks "Join Competition" button, it uses the competition ID to get the list of CompetitionSubmission from the competition ID
            //List<CompetitionSubmission> csList = csContext.getCompetitionAndCompetitor(id);
            var competitorId = HttpContext.Session.GetInt32("userID");

            int affectRows = csContext.joinCompetition(id, Convert.ToInt32(competitorId));

            return View("JoinCompetition");
        }


        // GET: CompetitorController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CompetitorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CompetitorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: CompetitorController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CompetitorController/Edit/5
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

        // GET: CompetitorController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CompetitorController/Delete/5
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
