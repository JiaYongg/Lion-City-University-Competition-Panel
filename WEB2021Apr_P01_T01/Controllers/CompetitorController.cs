using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
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
                    //CompetitorId = cs.CompetitorId,
                    CompetitionName = cs.CompetitionName,
                    StartDate = cs.StartDate,
                    EndDate = cs.EndDate,
                    ResultsReleaseDate = cs.ResultReleasedDate,
                    Appeal = cs.Appeal,
                    Ranking = cs.Ranking,
                    numOfJudge = cs.numofJudge,
                    durationLeftToStart = (cs.StartDate - DateTime.Now).Days,
                    durationLeftToSubmit = (cs.EndDate - DateTime.Now).Days,
                    resultsReleaseDuration = (cs.ResultReleasedDate - DateTime.Now).Days,
                    FileUrl = cs.FileUrl,
                    //FileUploadDateTime = cs.FileUploadDateTime
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

            csContext.joinCompetition(id, Convert.ToInt32(competitorId));

            CompetitorSubmissionViewModel csVM = csContext.GetCompetitionDetails(id);

            return View(csVM); // Need to add an object modal here to use in JoinCompetition View
        }

        // this method is used for viewing of competition details after they have joined the competition, used to submit file and appeal
        public ActionResult CompetitionSubmissionDetails(int id)
        {
            var competitorId = HttpContext.Session.GetInt32("userID");

            CompetitorSubmissionViewModel csVM = csContext.GetCompetitionDetails(id);

            return View("JoinCompetition", csVM); // Need to add an object modal here to use in JoinCompetition View
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompetitionSubmissionDetails(CompetitorSubmissionViewModel csVM)
        {
            var competitorId = HttpContext.Session.GetInt32("userID");
            csVM.CompetitorId = (int)competitorId;

            if (csVM.fileToUpload != null && csVM.fileToUpload.Length > 0)
            {
                try
                {
                    string fileExt = Path.GetExtension(csVM.fileToUpload.FileName);
                    string uploadedFile = String.Format("File_{0}_{1}{2}", csVM.CompetitorId, csVM.CompetitionId, fileExt);
                    string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\competitionfiles", uploadedFile);

                    // Upload the file to server
                    using (var fileSteam = new FileStream(
                     savePath, FileMode.Create))
                    {
                        await csVM.fileToUpload.CopyToAsync(fileSteam);
                    }

                    csVM.FileUrl = uploadedFile;
                    csVM.FileUploadDateTime = DateTime.Now;
                    csContext.uploadFile(csVM);
                    ViewData["UploadMessage"] = "Upload Successful!";
                }
                catch (IOException)
                {
                    ViewData["UploadMessage"] = "Upload Failed!";
                    return View("JoinCompetition", csVM);
                }
                catch (Exception ex)
                {
                    ViewData["UploadMessage"] = ex.Message;
                    return View("JoinCompetition", csVM);
                }
            }
            return View("JoinCompetition", csVM);
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
