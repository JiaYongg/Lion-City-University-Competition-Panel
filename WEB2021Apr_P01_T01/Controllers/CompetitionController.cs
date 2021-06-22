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

            return View("CurrentCompetitions", competitionList);
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
            return View("PastCompetitions", pastCompetitionList);
        }

        public ActionResult Details(int id)
        {
            ViewData["JudgeList"] = GetCompetitionJudges(id);
            ViewData["CriteriaList"] = GetCompetitionCriterias(id);
            ViewData["CompetitionSubmissionList"] = GetCompetitionSubmissions(id);
            ViewData["Comments"] = GetCompetitionComments(id);

            Competition competitionDetails = competitionContext.GetCompetitionDetails(id);
            if (competitionDetails == null)
            {
                RedirectToAction("Error", "Home");
            }

            if (competitionDetails.ResultsReleaseDate < DateTime.Now)
            {
                ViewData["ShowResults"] = "true";
                ViewData["Rankings"] = GetRankings(id);
            }
            else
            {
                ViewData["ShowResults"] = "false";
            }

            return View(competitionDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComments(IFormCollection formData, int? competitionId)
        {
            string comments = formData["Comments"].ToString();

            Comments cmmts = new Comments
            {
                CompetitionID = (int) competitionId,
                CommentDesc = comments,
                DateTimePosted = DateTime.Now
            };
            
            commentsContext.AddComments(cmmts);
            return RedirectToAction("Details", new { id = competitionId });
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
    }
}
