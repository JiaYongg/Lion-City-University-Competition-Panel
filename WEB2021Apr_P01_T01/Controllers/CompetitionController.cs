﻿using Microsoft.AspNetCore.Mvc;
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
            Competition competitionDetails = competitionContext.GetCompetitionDetails(id);
            if (competitionDetails == null)
            {
                RedirectToAction("Error", "Home");
            }
            return View(competitionDetails);
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
    }
}