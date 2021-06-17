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

        [HttpPost]
        public ActionResult login(IFormCollection formData)
        {
            // Read inputs from textboxes
            string loginEmail = formData["userEmail"].ToString();
            string password = formData["userPassword"].ToString();

            // Finding of judge email address && password, accessing database
            List<Judge> judgeList = judgeContext.GetAllJudge();
            foreach (Judge judge in judgeList)
            {
                if(loginEmail == judge.EmailAddr && password == judge.Password)
                {
                    HttpContext.Session.SetString("Name", judge.Name);
                    HttpContext.Session.SetString("Role", "LCU Judge");
                    // go to judge panel
                    return RedirectToAction("Judge");
                }
            }
            // Finding of competitor email address && password, accessing database
            List<Competitor> competitorList = competitorContext.GetAllCompetitor();
            foreach (Competitor competitor in competitorList)
            {
                if (loginEmail == competitor.EmailAddr && password == competitor.Password)
                {
                    HttpContext.Session.SetString("Name", competitor.Name);
                    HttpContext.Session.SetString("Role", "Competitor");
                    // go to competitor panel
                    return RedirectToAction("Competitor");
                }
            }

            // For admin login hard code.
            //Admin -- adminID and admin password below
            if (loginEmail == "admin@np.com" && password == "password1234")
            {
                // Admin login
                // Store user role as a string in session with the key “Role”
                HttpContext.Session.SetString("Role", "LCU Admin");

                // Redirect user to admin panel.
                return RedirectToAction("Admin");
            }
            else
            {
                // Store an error message in TempData for display at the index view
                TempData["Message"] = "Invalid Login Credentials!";

                // Redirect user back to the index view through an action
                return RedirectToAction("Index");
            }
        }
        // Find out a way to know if it is a admin, competitor or judge
        // user = admin, competitor or judge depending on who logged in
        public ActionResult user()
        {

        }
    }
}
