using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB2021Apr_P01_T01.DAL;
using Microsoft.AspNetCore.Http;

namespace WEB2021Apr_P01_T01.Controllers
{
    public class JudgeController : Controller
    {
        public CompetitionDAL compContext = new CompetitionDAL();
        public JudgeDAL judgeContext = new JudgeDAL();

        int criteriaNo = 1;
        
        // GET: JudgeController
        public ActionResult Index()
        {
            //int judgeId = (int)HttpContext.Session.GetInt32("userID");
            //ViewData["compName"] = judgeContext.GetJudgesCompetition(judgeId);
            return View();
        }

        public ActionResult JudgeCompetitor()
        {
            return View();
        }

        public ActionResult JudgeCriteria()
        {
            ViewData["criteriaNo"] = criteriaNo;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCriteria()
        {
            ViewData["criteriaNo"] = criteriaNo + 1;
            return View();
        }

        
    }
}
