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

        public ActionResult Index()
        {
            return View("Competition");
        }

        public ActionResult CurrentCompetitions()
        {
            List<Competition> competitionList = competitionContext.GetCurrentCompetitions();

            foreach (Competition comp in competitionList)
            {
                comp.AoiName = aoiContext.GetAoiName(comp.AoiId);
            }

            return View("CurrentCompetitions", competitionList);
        }

        public ActionResult PastCompetitions()
        {
            List<Competition> pastCompetitionList = competitionContext.GetPastCompetitions();
            foreach (Competition comp in pastCompetitionList)
            {
                comp.AoiName = aoiContext.GetAoiName(comp.AoiId);
            }
            return View("PastCompetitions", pastCompetitionList);
        }
    }
}
