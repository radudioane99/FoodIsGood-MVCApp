using foodisgood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace foodisgood.Controllers
{
    [AllowAnonymous]
    public class RewiewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [HttpGet, ActionName("GetSellerRewiews")]
        public ActionResult GetSellerRewiews(int? id)
        {
            ReviewModel reviewModel = new ReviewModel();
            Offer offer = db.Offers.Find(id);
            var seller = db.Users.Find(offer.UserID);
            var userReviewed = db.Users.Where(x => x.Id.Equals(offer.UserID)).FirstOrDefault();
            reviewModel.userId = offer.UserID;
            reviewModel.PersonFirstname = userReviewed.FirstName;
            reviewModel.PersonLastname = userReviewed.LastName;
            reviewModel.StarsAverage = this.GetStarsAverage(offer.UserID);
            if (offer.UserID != null)
            {
                var rewiews = db.Rewiews.ToList();
                var userRewiews = rewiews.Where(x => x.UserID == offer.UserID);
                reviewModel.rewiews = userRewiews;
                return View("Rewiews", reviewModel);
            }
            else
            {
                return View("Rewiews", null);
            }

        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            ReviewModel reviewModel = new ReviewModel();
            string text = form["Text"];
            string id = form["Id"];
            string note = form["Note"];
            var user = db.Users.Where(x => x.Email.Equals(this.User.Identity.Name)).FirstOrDefault();
            var userReviewed = db.Users.Where(x => x.Id.Equals(id)).FirstOrDefault();
            Rewiew rewiew = new Rewiew();
            rewiew.UserID = id;
            rewiew.Text = text;
            rewiew.date = DateTime.Now;
            rewiew.note = Convert.ToInt32(note);
            rewiew.UserReviewer = user.Id;
            rewiew.ReviewerFirstname = user.FirstName;
            rewiew.ReviewerLastname = user.LastName;
            db.Rewiews.Add(rewiew);
            db.SaveChanges();
            var rewiews = db.Rewiews.ToList();
            var userRewiews = rewiews.Where(x => x.UserID == id);
            reviewModel.rewiews = userRewiews;
            reviewModel.userId = id;
            reviewModel.PersonFirstname = userReviewed.FirstName;
            reviewModel.PersonLastname = userReviewed.LastName;
            return View("Rewiews", reviewModel);
        }

        private string GetStarsAverage(string usrId)
        {
            List<Rewiew> userReviews = this.db.Rewiews.Where(x => x.UserID.Equals(usrId)).ToList();
            float sum = 0;
            int contor = 0;
            foreach (Rewiew rewiew in userReviews)
            {
                contor++;
                sum = sum + rewiew.note;
            }
            float average = (float)sum / contor;
            return average.ToString("0.00");
        }
    }
}