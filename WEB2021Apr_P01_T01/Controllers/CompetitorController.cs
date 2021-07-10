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
            var competitorId = HttpContext.Session.GetInt32("userID");

            List<CompetitionSubmission> csList = csContext.competitorCompetitions(Convert.ToInt32(competitorId));

            List<CompetitorSubmissionViewModel> csVMList = MapToCompetitorSubmissionVM(csList);

            return View(csVMList);
        }

        public List<CompetitorSubmissionViewModel> MapToCompetitorSubmissionVM(List<CompetitionSubmission> csList)
        {

            List<CompetitorSubmissionViewModel> csVMList = new List<CompetitorSubmissionViewModel>();
            foreach (CompetitionSubmission cs in csList)
            {
                List<Judge> judgeList = judgeContext.GetCompetitionJudges(cs.CompetitionId);
                cs.numofJudge = judgeList.Count();

                CompetitorSubmissionViewModel csVM = new CompetitorSubmissionViewModel
                {
                    CompetitionId = cs.CompetitionId,
                    CompetitionName = cs.CompetitionName,
                    StartDate = cs.StartDate,
                    EndDate = cs.EndDate,
                    ResultsReleaseDate = cs.ResultReleasedDate,
                    Appeal = cs.Appeal,
                    Ranking = cs.Ranking,
                    numOfJudge = cs.numofJudge,
                    durationLeft = (cs.StartDate - DateTime.Now).Days,
                    resultsReleaseDuration = (cs.ResultReleasedDate - cs.EndDate).Days,
                    FileUrl = cs.FileUrl
                };

                if (DateTime.Now < csVM.StartDate) // competition has not yet started
                {
                    csVM.Status = "NotStarted";
                    csVM.DateImage = "Timeleft.png";
                }
                else if (DateTime.Now >= csVM.StartDate && DateTime.Now < csVM.EndDate) // competition is on-going, duration left to submit file
                {
                    csVM.Status = "Ongoing";
                    csVM.DateImage = "Timeleft.png";
                }
                else
                {
                    csVM.Status = "Ended";
                    csVM.DateImage = "Timeleft.png";
                }

                if (csVM.ResultsReleaseDate >= DateTime.Now && csVM.Status == "Ended") // competition ended, results yet to release
                {
                    csVM.ResultRelease = false;
                    csVM.DateImage = "results.png";
                }
                else if (csVM.Status == "Ended" && csVM.ResultsReleaseDate <= DateTime.Now)
                {
                    csVM.ResultRelease = true;
                    csVM.DateImage = "cup.png";
                }

                csVMList.Add(csVM);
            }

            return csVMList;
        }
        
        public ActionResult JoinCompetition(int id) // Takes in the competitionId to join the competition
        {

            var competitorId = HttpContext.Session.GetInt32("userID");

            int affectRows = csContext.joinCompetition(id, Convert.ToInt32(competitorId));

            CompetitionSubmission cs = csContext.GetCompetitionDetails(id);

            return View("JoinCompetition", cs); // Need to add an object modal here to use in JoinCompetition View
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
