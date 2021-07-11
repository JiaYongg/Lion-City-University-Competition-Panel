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
    public class SignUpController : Controller
    {
        private SignUpDAL signUpContext = new SignUpDAL();
        private AoiDAL aoiContext = new AoiDAL();

        private Competitor competitor = new Competitor();
        private Judge judge = new Judge();

        private List<SelectListItem> salutationDropDown = new List<SelectListItem>();
        private List<string> salutation = new List<string> {"", "Mr", "Mrs", "Dr", "Mdm", "Ms" };

        private List<SelectListItem> aoiDropDown = new List<SelectListItem>();
        private List<string> userTypes = new List<string> {"Judge Registration", "Competitor Registration"};

        public SignUpController()
        {

            foreach (var salutationItem in salutation)
            {
                salutationDropDown.Add(
                new SelectListItem
                {
                    Value = salutationItem.ToString(),
                    Text = salutationItem.ToString()
                });
            }

            foreach(var item in aoiContext.GetAreaInterests())
            {
                aoiDropDown.Add(
                new SelectListItem
                {
                    Value = item.AreaInterestId.ToString(),
                    Text = item.Name.ToString()
                });
            }
        }
        // GET: SignUpController
        public ActionResult Index()
        {
            ViewData["UserType"] = userTypes;
            ViewData["Salutation"] = salutationDropDown;
            ViewData["Aoi"] = aoiDropDown;
            SignUp signup = new SignUp
            {
                // Set the default value to "Judge Registration" for the radio button
                userType = userTypes[0]
            };

            return View("Signup", signup);
        }

        // Form submission for Sign Up
        [HttpPost]
        public ActionResult SignUp(SignUp signUp, Judge judge, Competitor competitor)
        {

            ViewData["UserType"] = userTypes;
            ViewData["Salutation"] = salutation;

            string name = signUp.firstName + " " + signUp.lastName;
            string chosenSalutation = signUp.salutation;
            string chosenEmail = signUp.emailAddress;
            string password = signUp.password;

            if (signUpContext.IsCompetitorEmailExist(chosenEmail, competitor.CompetitorId) == true)
            {
                TempData["Error"] = "Email already exists.";
                return RedirectToAction("Index");
            }
            else if (signUpContext.IsJudgeEmailExist(chosenEmail, judge.JudgeId) == true)
            {
                TempData["Error"] = "Email already exists.";
                return RedirectToAction("Index");
            }
            else
            {
                // Email does not exist add to database
                int selection = userTypes.IndexOf(signUp.userType);

                if (userTypes[selection] == "Judge Registration")
                {
                    // if statement to check if the email contains @lcu.edu.sg
                    if (signUp.emailAddress.Contains("@lcu.edu.sg"))
                    {
                        int aoiid = signUp.AreaInterestId;
                        judge.JudgeId = signUpContext.AddJudge(name, chosenSalutation, aoiid, chosenEmail, password);
                        return RedirectToAction("Index", "Login");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                else // Competitor registration
                {
                    // adds competitor account to database
                    competitor.CompetitorId = signUpContext.AddCompetitor(name, chosenSalutation, chosenEmail, password);
                    return RedirectToAction("Index", "Login");
                }
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
