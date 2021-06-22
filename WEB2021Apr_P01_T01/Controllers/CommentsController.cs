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
    public class CommentsController : Controller
    {
        private CommentsDAL commentsContext = new CommentsDAL();

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult AddComments(IFormCollection formData, int? competitionId)
        {
            string comments = formData["Comments"].ToString();

            if(ModelState.IsValid)
            {
                Comments cmmts = new Comments
                {
                    CompetitionID = (int)competitionId,
                    CommentDesc = comments,
                    DateTimePosted = DateTime.Now
                };

                commentsContext.AddComments(cmmts);
            }
            
            return RedirectToAction("Details", "Competition", new { id = competitionId });
        }
    }
}
