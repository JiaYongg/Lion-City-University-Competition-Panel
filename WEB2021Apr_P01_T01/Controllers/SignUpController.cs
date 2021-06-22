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
    public class SignUpController : Controller
    {
        private SignUpDAL signUpContext = new SignUpDAL();
        private List<string> userTypes = new List<string> {"Judge Registration", "Competitor Registration"};

        // GET: SignUpController
        public ActionResult Index()
        {
            return View();
        }

        // When "Sign Up" button on nav bar is clicked
        public ActionResult SignUp()
        {
            ViewData["UserType"] = userTypes;
            SignUp signup = new SignUp
            {
                // Set the default value to "Judge Registration" for the radio button
                userType = userTypes[0]
            };
            return View(signup);
        }

        // Form submission for Sign Up
        [HttpPost]
        public ActionResult SignUp(SignUp signUp, Judge judge, Competitor competitor)
        {
            ViewData["UserType"] = userTypes;

            int selection = userTypes.IndexOf(signUp.userType);

            if (userTypes[selection] == "Judge Registration")
            {
                // if statement to check if the email contains @lcu.edu.sg
                if (signUp.emailAddress.Contains("@lcu.edu.sg"))
                {
                    //judge.JudgeId = signUpContext.AddJudge(judge); // <--- Eddie's part of code, Yet to implement
                    judge.JudgeId = signUpContext.AddJudge(judge);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(judge);
                }
            }
            else // Competitor registration
            {
                // adds competitor account to database
                competitor.CompetitorId = signUpContext.AddCompetitor(competitor);
                return RedirectToAction("Index");
            }

        }

        // GET: SignUpController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SignUpController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SignUpController/Create
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

        // GET: SignUpController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SignUpController/Edit/5
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

        // GET: SignUpController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SignUpController/Delete/5
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
