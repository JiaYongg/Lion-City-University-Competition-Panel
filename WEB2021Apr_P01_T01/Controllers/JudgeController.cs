using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB2021Apr_P01_T01.DAL;
using WEB2021Apr_P01_T01.Models;

namespace WEB2021Apr_P01_T01.Controllers
{
    public class JudgeController : Controller
    {
        
        public JudgeDAL judgeContext = new JudgeDAL();
        public CriteriaDAL criteriaContext = new CriteriaDAL();

        // Items needed for competition marking 
        public CompetitionScoreDAL scoreContext = new CompetitionScoreDAL();
        public CompetitionDAL compContext = new CompetitionDAL();
        public CompetitionSubmissionDAL submitContext = new CompetitionSubmissionDAL();

        public static int criteriaNo = 2;
        public static bool showCriteria = false;


        // GET: JudgeController/
        public ActionResult Index()
        {
            int judgeId = (int)HttpContext.Session.GetInt32("userID");
            List<Competition> competitionList = judgeContext.GetJudgesCompetition(judgeId);
            foreach (Competition competition in competitionList)
            {
                Console.WriteLine(competition.CompetitionName);
                HttpContext.Session.SetString("compName", competition.CompetitionName);
                HttpContext.Session.SetInt32("compId", competition.CompetitionId);
                ViewData["compName"] = competition.CompetitionName;
            }
            return View();
        }

        public ActionResult JudgeCompetitor()
        {
            int compId = (int)HttpContext.Session.GetInt32("compId");
            var compList = submitContext.GetCompetitionSubmissions(compId);
            var weightage = criteriaContext.GetCompetitionCriteria(compId);

            foreach (var item in compList)
            {
                int i = 0;
                double total = 0;

                foreach (var score in scoreContext.GetCompetitiorScores(compId, item.CompetitorId))
                {
                    total += weightage[i].Weightage * Convert.ToDouble(score.Score) / 10;
                    i++;
                }
                item.TotalMarks = total;
            }

            ViewData["Submit"] = compList;

            CompetitionSubmission competitionSubmission = submitContext.GetCompetitionDetails(compId); 

            return View(competitionSubmission);
        }

        public ActionResult competitorAppeal()
        {
            //ViewData["appeal"] = competitionSubmission.Appeal;
            return RedirectToAction("JudgeCompetitor");
        }
        // Action to display appeal
        public ActionResult Grade()
        {

            return View();
        }


        public ActionResult JudgeCriteria()
        {
            ViewData["compName"] = HttpContext.Session.GetString("compName");
            ViewData["ShowCriteria"] = showCriteria;
            ViewData["criteriaNo"] = criteriaNo;

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCriteria()
        {
            criteriaNo += 1;
            showCriteria = true;

            return RedirectToAction("JudgeCriteria");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCriteria()
        {
            criteriaNo -= 1;
            if (criteriaNo == 2)
            {
                // when criteria is back to 1 only do not display the html code for other criterias 
                showCriteria = false;
            }
            else
            {
                showCriteria = true;
            }
            if (criteriaNo == 1)
            {
                // When deleting the last criteria show this error
                criteriaNo += 1;
                TempData["criteriaError"] = "You cannot delete the last criteria!!";
            }

            return RedirectToAction("JudgeCriteria");
        }


        [HttpPost]
        public ActionResult Create(IFormCollection formData)
        {
            // Reminder to have an alert pop up upoon successfully creating the criteria(s)
            try
            {
                int total = 0;

                int compId = (int)HttpContext.Session.GetInt32("compId");
                var weightageList = formData["Weightage"].ToString().Split(',').Select(Int32.Parse).ToList();
                var nameList = formData["CriteriaName"].ToString().Split(',').ToList();

                bool emptyName = nameList.Contains("");

                var criteriaList = criteriaContext.GetAllCriteria();
                foreach (var item in criteriaList)
                {
                    // Check if the database already has criterias and the total weightage of the criterias = 100
                    if (item.CompetitionID == compId)
                    {
                        total += item.Weightage;
                    }
                }
                if (total == 100)
                {
                    TempData["criteriaError"] = "Sorry criteria for this competition has already been done.";
                    return RedirectToAction("JudgeCriteria");
                }
                else
                {
                    foreach (var item in weightageList)
                    {
                        total += item;
                    }
                    if (total == 100 && nameList.Contains("") == false)
                    {
                        // total weightage inputted is 100 and all criteria names are filled
                        for (int i = 0; i < weightageList.Count; i++)
                        {
                            criteriaContext.AddCriteria(compId, nameList[i], weightageList[i]);
                        }

                        showCriteria = false;
                        criteriaNo = 2;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        // Total weightage is 100 but some criteria names arent filled
                        if (emptyName == true && total == 100)
                        {
                            TempData["criteriaError"] = "Please fill up all values";
                        }
                        // weightage is not 100 
                        else
                        {
                            TempData["criteriaError"] = "Weightage must total up to 100%";
                        }

                        return RedirectToAction("JudgeCriteria");
                    }
                }
            }
            // Happens when a weightage is blank
            catch (FormatException e)
            {
                TempData["criteriaError"] = "Please fill up all values";
                return RedirectToAction("JudgeCriteria");
            }
            catch
            {
                TempData["criteriaError"] = "Please key in all values";
                return RedirectToAction("JudgeCriteria");
            }
        }

        public ActionResult Edit()
        {
            return View();
        }
    }
}
