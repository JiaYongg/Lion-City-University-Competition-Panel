﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public CompetitionDAL compContext = new CompetitionDAL();
        public JudgeDAL judgeContext = new JudgeDAL();
        public CriteriaDAL criteriaContext = new CriteriaDAL();

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
            }
            return View();
        }

        public ActionResult JudgeCompetitor()
        {
            return View();
        }


        public ActionResult JudgeCriteria()
        {
            ViewData["compName"] = HttpContext.Session.GetString("compName");
            ViewData["ShowCriteria"] = showCriteria;
            ViewData["criteriaNo"] = criteriaNo;
            Console.WriteLine(criteriaNo);
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
                showCriteria = false;
            }
            else
            {
                showCriteria = true;
            }
            if (criteriaNo == 1)
            {
                criteriaNo += 1;
                TempData["criteriaError"] = "You cannot delete the last criteria!!";
            }

            return RedirectToAction("JudgeCriteria");
        }


        [HttpPost]
        public ActionResult Create(IFormCollection formData)
        {
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
                        if (emptyName == true && total == 100)
                        {
                            TempData["criteriaError"] = "Please fill up all values";
                        }
                        else
                        {
                            TempData["criteriaError"] = "Weightage must total up to 100%";
                        }

                        return RedirectToAction("JudgeCriteria");
                    }
                }
            }
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
    }
}
