using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB2021Apr_P01_T01.DAL;
using WEB2021Apr_P01_T01.Models;

namespace WEB2021Apr_P01_T01.Controllers
{
    public class CompetitionController : Controller
    {
        private CompetitionDAL competitionContext = new CompetitionDAL();
        private AoiDAL aoiContext = new AoiDAL();
        private CriteriaDAL criteriaContext = new CriteriaDAL();
        private JudgeDAL judgeContext = new JudgeDAL();
        private CompetitionSubmissionDAL csContext = new CompetitionSubmissionDAL();
        private CommentsDAL commentsContext = new CommentsDAL();

        public ActionResult Index()
        {
            return View("Competition");
        }

        public ActionResult CurrentCompetitions()
        {
            List<Competition> competitionList = competitionContext.GetCurrentCompetitions();
            ViewData["PageTitle"] = "Ongoing & Future Competitions";
            ViewData["Subtitle"] = "Unleash your competitive spirits and participate in one of our competition now!";
            ViewData["BgImgUrl"] = "../images/10163.png";
            List<AreaInterest> aoiList = aoiContext.GetAreaInterests();
            ViewData["AreaInterest"] = aoiList;

            foreach (Competition comp in competitionList)
            {
                comp.AoiName = aoiContext.GetAoiName(comp.AoiId);
            }

            return View("CompetitionList", competitionList);
        }

        public ActionResult PastCompetitions()
        {
            List<Competition> pastCompetitionList = competitionContext.GetPastCompetitions();
            ViewData["PageTitle"] = "Past Competitions";
            ViewData["Subtitle"] = "View the past competition, its entries and the award winners.";
            ViewData["BgImgUrl"] = "../images/66732.png";
            List<AreaInterest> aoiList = aoiContext.GetAreaInterests();
            ViewData["AreaInterest"] = aoiList;

            foreach (Competition comp in pastCompetitionList)
            {
                comp.AoiName = aoiContext.GetAoiName(comp.AoiId);
            }
            return View("CompetitionList", pastCompetitionList);
        }

        public ActionResult CompetitionDetails(int id)
        {
            Competition competitionDetails = competitionContext.GetCompetitionDetails(id);
            List<CompetitionSubmission> csList = GetCompetitionSubmissions(id);
            List<Judge> juryList  = GetCompetitionJudges(id);
            List<Criteria> criteriaList = GetCompetitionCriterias(id);
            List<Comments> commentsList = GetCompetitionComments(id);

            CompetitionDetailsViewModel cdVM = MapToCompetitionDetailsVM(competitionDetails, csList, juryList, commentsList, criteriaList);

            if (competitionDetails == null)
            {
                RedirectToAction("Error", "Home");
            }

            if (competitionDetails.ResultsReleaseDate < DateTime.Now)
            {
                ViewData["ShowResults"] = true;
                ViewData["Rankings"] = GetRankings(id);
            }
            else
            {
                ViewData["ShowResults"] = false;
            }

            if (HttpContext.Session.GetString("Voted") == "true")
            {
                ViewData["DisableVote"] = true;
            }
            else
            {
                if (competitionDetails.StartDate >= DateTime.Now || competitionDetails.EndDate <= DateTime.Now)
                {
                    ViewData["DisableVote"] = true;
                }
                else
                {
                    ViewData["DisableVote"] = false;
                }
            }

            // Jia Yong's added codes

            if (competitionDetails.StartDate.Subtract(DateTime.Now).Days <= 3)
            {
                ViewData["Joinable"] = true;
            }
            else
            {
                ViewData["Joinable"] = false;
            }

            bool found = false;
            foreach (var item in csList)
            {
                if (HttpContext.Session.GetInt32("userID") == item.CompetitorId)
                {
                    found = true;
                    break;
                }
            }

            ViewData["IsJoined"] = found;
            ViewData["CompetitionSubmissionList"] = csList;

            return View(cdVM);
        }

        [HttpPost]
        public ActionResult CompetitionDetails(CompetitionDetailsViewModel cdVM, int id)
        {
            Competition competitionDetails = competitionContext.GetCompetitionDetails(id);
            cdVM.JudgeList = GetCompetitionJudges(id);
            cdVM.CriteriaList = GetCompetitionCriterias(id);
            cdVM.CommentsList = GetCompetitionComments(id);
            cdVM.SubmissionList = GetCompetitionSubmissions(id);

            cdVM.AoiName = competitionDetails.AoiName;
            cdVM.CompetitionId = id;
            cdVM.CompetitionName = competitionDetails.CompetitionName;
            cdVM.StartDate = competitionDetails.StartDate;
            cdVM.EndDate = competitionDetails.EndDate;
            cdVM.ResultsReleaseDate = competitionDetails.ResultsReleaseDate;

            if (competitionDetails == null)
            {
                RedirectToAction("Error", "Home");
            }

            if (competitionDetails.ResultsReleaseDate < DateTime.Now)
            {
                ViewData["ShowResults"] = true;
                ViewData["Rankings"] = GetRankings(id);
            }
            else
            {
                ViewData["ShowResults"] = false;
            }

            if (HttpContext.Session.GetString("Voted") == "true")
            {
                ViewData["DisableVote"] = true;
            }
            else
            {
                if (competitionDetails.StartDate >= DateTime.Now || competitionDetails.EndDate <= DateTime.Now)
                {
                    ViewData["DisableVote"] = true;
                }
                else
                {
                    ViewData["DisableVote"] = false;
                }
            }

            // Jia Yong's added codes

            bool found = false;
            foreach (var item in cdVM.SubmissionList)
            {
                if (HttpContext.Session.GetInt32("userID") == item.CompetitorId)
                {
                    found = true;
                    break;
                }
            }

            ViewData["IsJoined"] = found;
            ViewData["CompetitionSubmissionList"] = cdVM.SubmissionList;

            // Jia Yong's added codes

            if (competitionDetails.StartDate.Subtract(DateTime.Now).Days <= 3)
            {
                ViewData["Joinable"] = true;
            }
            else
            {
                ViewData["Joinable"] = false;
            }

            if (!ModelState.IsValid)
            {
                return View(cdVM);
            }

            Comments cmmts = new Comments
            {
                CommentDesc = cdVM.CommentDesc,
                DateTimePosted = DateTime.Now,
                CompetitionID = cdVM.CompetitionId
            };

            commentsContext.AddComments(cmmts);

            return RedirectToAction("CompetitionDetails", "Competition", cdVM.CompetitionId, "Comments");
        }

        public CompetitionDetailsViewModel MapToCompetitionDetailsVM(Competition comp, List<CompetitionSubmission> csList, List<Judge> judgeList, List<Comments> commentsList, List<Criteria> criteriaList)
        {
            CompetitionDetailsViewModel cdVM = new CompetitionDetailsViewModel
            {
                CompetitionId = comp.CompetitionId,
                CompetitionName = comp.CompetitionName,
                StartDate = comp.StartDate,
                EndDate = comp.EndDate,
                ResultsReleaseDate = comp.ResultsReleaseDate,
                AoiName = comp.AoiName,
                JudgeList = judgeList,
                CriteriaList = criteriaList,
                SubmissionList = csList,
                CommentsList = commentsList
            };

            return cdVM;
        }

        [HttpPost]
        public ActionResult Vote(int? competitorId, int? competitionId)
        {
            if (HttpContext.Session.GetString("Voted") != "true")
            {
                AddVoteCount((int)competitorId, (int)competitionId);
                HttpContext.Session.SetString("Voted", "true");
            }

            return RedirectToAction("CompetitionDetails", new { id = competitionId });
        }

        private List<Criteria> GetCompetitionCriterias(int id)
        {
            // Get the list of criteria objects for a specific competition
            List<Criteria> criteriaList = criteriaContext.GetCompetitionCriteria(id);
            return criteriaList;
        }

        private List<Judge> GetCompetitionJudges(int id)
        {
            // Get the list of judge objects for a specific competition
            List<Judge> judgeList = judgeContext.GetCompetitionJudges(id);
            return judgeList;
        }

        private List<CompetitionSubmission> GetCompetitionSubmissions(int id)
        {
            // Get the list of competition submissions for a specific competition
            List<CompetitionSubmission> csList = csContext.GetCompetitionSubmissions(id);
            return csList;
        }

        private List<Comments> GetCompetitionComments(int id)
        {
            List<Comments> commentsList = commentsContext.GetCompetitionComments(id);
            return commentsList;
        }

        private List<CompetitionSubmission> GetRankings(int id)
        {
            List<CompetitionSubmission> rankList = csContext.GetTopThree(id);
            return rankList;
        }

        private int AddVoteCount(int competitorId, int competitionId)
        {
            int count = csContext.Vote(competitorId, competitionId);
            return count;
        }
    }
}
