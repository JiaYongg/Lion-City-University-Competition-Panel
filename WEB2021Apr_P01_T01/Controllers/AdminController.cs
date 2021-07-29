using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB2021Apr_P01_T01.DAL;
using WEB2021Apr_P01_T01.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WEB2021Apr_P01_T01.Controllers
{
    public class AdminController : Controller
    {
        public CompetitionDAL compyContext = new CompetitionDAL();
        private AoiDAL aoiContext = new AoiDAL();
        private JudgeDAL judgeContext = new JudgeDAL();

        // GET: AdminController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AdminController/Details/5
        public ActionResult CreateCompetition()
        {
            ViewData["aoiList"] = GetAOI();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCompetition(Competition competition)
        {
            if (!ModelState.IsValid)
            {
                ModelStateEntry sDate = null;
                ModelStateEntry eDate = null;
                ModelStateEntry rDate = null;
                if (ModelState.TryGetValue("StartDate", out sDate))
                {
                    if (sDate != null && sDate.Errors.Count > 0)
                    {
                        ModelState.Remove("StartDate");
                        ModelState.AddModelError("StartDate", "Start Date is invalid");
                    }
                }

                if (ModelState.TryGetValue("EndDate", out eDate))
                {
                    if (eDate != null && eDate.Errors.Count > 0)
                    {
                        ModelState.Remove("EndDate");
                        ModelState.AddModelError("EndDate", "End Date is invalid");
                    }
                }

                if (ModelState.TryGetValue("ResultsReleaseDate", out rDate))
                {
                    if (rDate != null && rDate.Errors.Count > 0)
                    {
                        ModelState.Remove("ResultsReleaseDate");
                        ModelState.AddModelError("ResultsReleaseDate", "Result Release Date is invalid");
                    }
                }

                if (!competition.Validated)
                {
                    var validationResults = competition.Validate(new ValidationContext(competition, null, null));
                    foreach (var error in validationResults)
                    {
                        foreach (var memberName in error.MemberNames)
                        {
                            ModelState.AddModelError(memberName, error.ErrorMessage);
                        }
                    }
                }
                ViewData["aoiList"] = GetAOI();
                return View(competition);
            }
            if(compyContext.CheckCompetitionName(competition.CompetitionName))
            {
                ModelState.AddModelError("CompetitionName", "Competition already exists");
                ViewData["aoiList"] = GetAOI();
                return View(competition);
            }
            int aoiID = compyContext.GetAreaInterestID(competition.AoiName);
            int compId = compyContext.AddCompetition(competition, aoiID);

            return Redirect("~/Competition/CompetitionDetails/"+compId);
        }

        private List<SelectListItem> GetAOI()
        {
            List<SelectListItem> aoi = new List<SelectListItem>();

            List<AreaInterest> listOfAOI = aoiContext.GetAreaInterests();

            foreach (var item in listOfAOI)
            {
                aoi.Add(new SelectListItem
                {
                    Value = item.Name,
                    Text = item.Name
                });
            }

            return aoi;
        }

        // GET: AdminController/Create
        public ActionResult AreaOfInterest()
        {
            List<AreaInterest> aoiList = aoiContext.GetAreaInterests();
            List<string> aoiNames = new List<string>();
            List<int> aoiId = new List<int>();
            foreach (var item in aoiList)
            {
                aoiNames.Add(item.Name);
                aoiId.Add(item.AreaInterestId);
            }

            ViewData["totalAOI"] = aoiList.Count();
            ViewData["AoiName"] = aoiNames;
            ViewData["AoiId"] = aoiId;

            AreaInterest areaInterest = aoiContext.GetAoi();

            return View(areaInterest);
        }

        public ActionResult AddInterest(IFormCollection formData)
        {
            string aoiName = formData["AoiName"];

            aoiContext.AddAreaInterest(aoiName);
            return RedirectToAction("AreaOfInterest");
        }

        // GET: AdminController/Delete/5
        public ActionResult Delete()
        {
            return View();
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            aoiContext.Delete(id);
            return RedirectToAction("AreaOfInterest");
        }

        public ActionResult ViewJudges()
        {
            List<Judge> j = judgeContext.GetAllJudge();
            foreach (Judge item in j)
            {
                item.AreaInterestName = aoiContext.GetAoiName(item.AreaInterestId);
                item.competitionAssigned = judgeContext.GetJudgesFutureCompetition(item.JudgeId);
            }
            ViewData["futureComp"] = compyContext.GetFutureCompetitions();
            return View(j);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignJudgeToComp()
        {
            List<string> existList = new List<string>();
            List<string> compIds = Request.Query["cid"].ToString().Split('-').ToList();
            List<string> judgeIds = Request.Query["jid"].ToString().Split('-').ToList();
            foreach(string cid in compIds)
            {
                foreach(string jid in judgeIds)
                {
                    if(!judgeContext.AssignJudgeToComp(int.Parse(jid), int.Parse(cid)))
                    {
                        existList.Add(cid + "-" + jid);
                    }
                }
            }

            if(existList.Count == 0)
            {
                TempData["JavaScriptFunction"] = "assignJudgeSuccess('0');";
            }
            else
            {
                string str = "";
                foreach(string s in existList)
                {
                    str += s + ",";
                }
                str = str.Substring(0, str.Length - 1);

                TempData["JavaScriptFunction"] = string.Format("assignJudgeSuccess('{0}');", str);
            }
            return RedirectToAction("ViewJudges");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnassignJudgeFromComp()
        {
            string compId = Request.Query["cid"].ToString();
            string judgeId = Request.Query["jid"].ToString();

            judgeContext.UnassignJudgeFromComp(int.Parse(judgeId), int.Parse(compId));

            return RedirectToAction("ViewJudges");
        }
    }
}
