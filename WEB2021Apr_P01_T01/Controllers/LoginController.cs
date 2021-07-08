using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB2021Apr_P01_T01.DAL;
using WEB2021Apr_P01_T01.Models;

namespace WEB2021Apr_P01_T01.Controllers
{
    public class LoginController : Controller
    {
        // Using DAL object
        private JudgeDAL judgeContext = new JudgeDAL();
        private CompetitorDAL competitorContext = new CompetitorDAL();

        // GET: LoginController
        public ActionResult Index()
        {
            return View("Login");
        }

        // GET: LoginController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(IFormCollection formData)
        {
            // Read inputs from textboxes
            string loginEmail = formData["Email"].ToString();
            string password = formData["Password"].ToString();

            // Finding of judge email address && password, accessing database
            List<Judge> judgeList = judgeContext.GetAllJudge();
            foreach (Judge judge in judgeList)
            {
                if (loginEmail == judge.EmailAddr && password == judge.Password)
                {
                    HttpContext.Session.SetString("Name", judge.Name);
                    HttpContext.Session.SetString("Role", "LCU Judge");
                    HttpContext.Session.SetInt32("userID", judge.JudgeId);
                    // go to judge panel
                    return RedirectToAction("Index","Judge");
                }
            }

            //Finding of competitor email address && password, accessing database
            List<Competitor> competitorList = competitorContext.GetAllCompetitor();
            foreach (Competitor competitor in competitorList)
            {
                if (loginEmail == competitor.EmailAddr && password == competitor.Password)
                {
                    HttpContext.Session.SetString("Name", competitor.Name);
                    HttpContext.Session.SetString("Role", "Competitor");
                    HttpContext.Session.SetInt32("userID", competitor.CompetitorId);
                    // go to competitor panel
                    return RedirectToAction("Index", "Competitor");
                }
            }

            // For admin login hard code.
            //Admin -- adminID and admin password below
            if (loginEmail == "admin1@lcu.edu.sg" && password == "p@55Admin")
            {
                // Admin login
                // Store user role as a string in session with the key “Role”
                HttpContext.Session.SetString("Role", "LCU Admin");

                // Redirect user to admin panel.
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                // Store an error message in TempData for display at the index view
                TempData["Message"] = "Invalid Login Credentials!";

                // Redirect user back to the index view through an action
                return RedirectToAction("Index");
            }
        }

        // GET: LoginController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoginController/Create
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

        // GET: LoginController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LoginController/Edit/5
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

        // GET: LoginController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LoginController/Delete/5
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
