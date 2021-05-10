using foodisgood.Models;
using System;
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
            reviewModel.userId = offer.UserID;
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
            Rewiew rewiew = new Rewiew();
            rewiew.UserID = id;
            rewiew.Text = text;
            rewiew.date = DateTime.Now;
            rewiew.note = Convert.ToInt32(note);
            db.Rewiews.Add(rewiew);
            db.SaveChanges();
            var rewiews = db.Rewiews.ToList();
            var userRewiews = rewiews.Where(x => x.UserID == id);
            reviewModel.rewiews = userRewiews;
            reviewModel.userId = id;
            return View("Rewiews", reviewModel);
        }
    }
}