using foodisgood.Models;
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
            Offer offer = db.Offers.Find(id);
            var seller = db.Users.Find(offer.UserID);
            var rewiews = db.Rewiews.ToList();
            var userRewiews = rewiews.Select(x => x.UserID == offer.UserID);
            return View("Rewiews", rewiews);
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            string text = form["Text"];
            string id = form["Id"];
            Rewiew rewiew = new Rewiew();
            rewiew.UserID = id;
            rewiew.Text = text;
            db.Rewiews.Add(rewiew);
            db.SaveChanges();
            var rewiews = db.Rewiews.ToList();
            var userRewiews = rewiews.Select(x => x.UserID == id);
            return View("Rewiews", rewiews);
        }
    }
}