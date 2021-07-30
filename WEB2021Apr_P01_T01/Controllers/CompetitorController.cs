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
        public CompetitionScoreDAL scoreContext = new CompetitionScoreDAL();
        public CriteriaDAL criteriaContext = new CriteriaDAL();
        private JudgeDAL judgeContext = new JudgeDAL();
        // GET: CompetitorController
        public ActionResult Index()
        {
            int competitorId = (int)HttpContext.Session.GetInt32("userID");

            List<CompetitionSubmission> csList = csContext.CompetitorCompetitions(competitorId);

            List<CompetitorSubmissionViewModel> csVMList = MapToCompetitorSubmissionVM(csList);

            return View(csVMList);
        }

        public List<CompetitorSubmissionViewModel> MapToCompetitorSubmissionVM(List<CompetitionSubmission> csList)
        {
            int competitorId = (int)HttpContext.Session.GetInt32("userID");

            List<CompetitorSubmissionViewModel> csVMList = new List<CompetitorSubmissionViewModel>();


            foreach (CompetitionSubmission cs in csList)
            {
                List<Judge> judgeList = judgeContext.GetCompetitionJudges(cs.CompetitionId);
                cs.numofJudge = judgeList.Count();

                var criteriaList = criteriaContext.GetCompetitionCriteria(cs.CompetitionId);

                List<string> nameList = new List<string>();
                List<int> weightList = new List<int>();
                List<double> scoreList = new List<double>();

                foreach (var item in criteriaList)
                {
                    nameList.Add(item.CriteriaName);
                    weightList.Add(item.Weightage);
                }

                int i = 0;
                foreach (var score in scoreContext.GetCompetitiorScores(cs.CompetitionId, competitorId))
                {
                    // convert score to percentage out of the weightage score
                    double num = score.Score;
                    num /= 10;
                    num *= weightList[i];
                    scoreList.Add(num);
                    i++;
                }

                CompetitorSubmissionViewModel csVM = new CompetitorSubmissionViewModel
                {
                    CompetitionId = cs.CompetitionId,
                    //CompetitorId = cs.CompetitorId,
                    CompetitionName = cs.CompetitionName,
                    CriteriaName = nameList,
                    Score = scoreList,
                    VoteCount = cs.VoteCount,
                    Weightage = weightList,
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

            int competitorId = (int)HttpContext.Session.GetInt32("userID");

            csContext.JoinCompetition(id, competitorId);

            return RedirectToAction("Index"); // Need to add an object modal here to use in JoinCompetition View
        }

        // this method is used for viewing of competition details after they have joined the competition, used to submit file and appeal
        public ActionResult CompetitionSubmissionDetails(int id)
        {
            int competitorId = (int)HttpContext.Session.GetInt32("userID");

            CompetitorSubmissionViewModel csVM = csContext.GetCompetitorCompetitionDetails(id, competitorId);

            return View("JoinCompetition", csVM); // Need to add an object modal here to use in JoinCompetition View
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompetitionSubmissionDetails(CompetitorSubmissionViewModel csVM)
        {
            int competitorId = (int)HttpContext.Session.GetInt32("userID");
            csVM.CompetitorId = competitorId;

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
                    csContext.UploadFile(csVM);
                    ViewData["UploadColor"] = "lime";
                    ViewData["UploadMessage"] = "Upload Successful!";
                }
                catch (IOException)
                {
                    ViewData["UploadColor"] = "red";
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

        public ActionResult Appeal(int id)
        {
            int competitorId = (int)HttpContext.Session.GetInt32("userID");
            CompetitorSubmissionViewModel csVM = csContext.GetCompetitorCompetitionDetails(id, competitorId);
            return View("Appeal", csVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Appeal(CompetitorSubmissionViewModel csVM)
        {
            if (csVM.Appeal != null)
            {
                csContext.SubmitAppeal(csVM.Appeal, csVM.CompetitorId, csVM.CompetitionId);
            }
            return RedirectToAction("Index");
        }
    }
}
