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
            if (seller != null)
            {
                offer.User = seller;
            }
            var rewiews = db.Rewiews.Where(x => x.UserID == seller.Id).ToList();

            return HttpNotFound();
        }
    }
}