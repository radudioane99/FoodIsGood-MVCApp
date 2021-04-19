using foodisgood.Models;
using Microsoft.AspNet.Identity;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace foodisgood.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Offer);
            if (User.IsInRole("AppAdmin"))
            {
                return View("Index", orders.ToList());
            }
            else if (User.IsInRole("Customer"))
            {
                return View("IndexCustomer", orders.ToList());
            }
            else if (User.Identity.IsAuthenticated == false)
            {
                return View("IndexGuest", orders.ToList());
            }
            else
            {
                return View();
            }
        }

        public ActionResult MyOrders()
        {
            var id = User.Identity.GetUserId();
            id = id.ToString();
            var myOffers = db.Orders.Where(o => o.BuyerID.Equals(id));
            return View("MyOrders", myOffers.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("AppAdmin"))
            {
                return View("Details", order);
            }
            else if (User.IsInRole("Customer"))
            {
                return View("DetailsCustomer", order);
            }
            else
            {
                return View();
            }
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.OfferID = new SelectList(db.Offers, "ID", "Description");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,BuyerID,OfferID,DesiredQuantity")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OfferID = new SelectList(db.Offers, "ID", "Description", order.OfferID);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.OfferID = new SelectList(db.Offers, "ID", "Description", order.OfferID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,BuyerID,OfferID,DesiredQuantity")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OfferID = new SelectList(db.Offers, "ID", "Description", order.OfferID);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet, ActionName("PlaceOffer")]
        public ActionResult PlaceOffer(int? id)
        {
            OrderOffer model = new OrderOffer();
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                //Get data we need
                var seller = db.Users.Find(offer.UserID);
                if (seller != null)
                {
                    offer.User = seller;
                }
                Order order = new Order();
                order.OfferID = offer.ID;
                order.BuyerID = User.Identity.GetUserId();
                order.BuyerUser = db.Users.FirstOrDefault(x => x.Id == order.BuyerID);
                model.Order = order;
                model.Offer = offer;
                return View("CreateCustomer", model);
            }
        }

        [HttpPost, ActionName("PostOrderToOffer")]
        public ActionResult PostOrderToOffer([Bind(Include = "DesiredQuantity,OfferID,BuyerID")] Order order)
        {
            if (order.BuyerID != null && order.OfferID != 0 && order.DesiredQuantity != 0)
            {
                order.BuyerUser = db.Users.Find(order.BuyerID);
                order.Offer = db.Offers.Find(order.OfferID);
                order.Offer.Quantity = order.Offer.Quantity - order.DesiredQuantity;
                db.Entry(order.Offer).State = EntityState.Modified;
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
