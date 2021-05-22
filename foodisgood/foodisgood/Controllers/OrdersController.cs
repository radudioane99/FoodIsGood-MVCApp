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
        [Authorize(Roles = "AppAdmin")]
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
            var myOffers = db.Orders.Where(o => o.BuyerUserID.Equals(id));
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
        public ActionResult Create([Bind(Include = "ID,BuyerUserID,OfferID,DesiredQuantity,Accepted")] Order order)
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
        public ActionResult Edit([Bind(Include = "ID,DesiredQuantity,Accepted")] Order order)
        {
            Order initialOrder = db.Orders.AsNoTracking().Single(o => o.ID.Equals(order.ID));
            order.BuyerUserID = initialOrder.BuyerUserID;
            order.OfferID = initialOrder.OfferID;
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

        // Dioane kill yourself pls!
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
                //Get *the* data we need // Dioane burn that Cambridge Certificate pls!
                var seller = db.Users.Find(offer.UserID);
                if (seller != null)
                {
                    offer.User = seller;
                }
                Order order = new Order();
                order.OfferID = offer.ID;
                order.BuyerUserID = User.Identity.GetUserId();
                order.BuyerUser = db.Users.FirstOrDefault(x => x.Id == order.BuyerUserID);
                model.Order = order;
                model.Offer = offer;
                return View("CreateCustomer", model);
            }
        }

        //--------------------------------------------------------------
        [HttpPost, ActionName("PostOrderToOffer")]
        public ActionResult PostOrderToOffer([Bind(Include = "DesiredQuantity,OfferID,BuyerUserID,Accepted")] Order order)
        {
            order.Offer = db.Offers.Find(order.OfferID);
            if (order.BuyerUserID != null && order.OfferID != 0 && order.DesiredQuantity != 0 && order.DesiredQuantity <= order.Offer.Quantity)
            {
                order.BuyerUser = db.Users.Find(order.BuyerUserID);
                db.Entry(order.Offer).State = EntityState.Modified;
                db.Orders.Add(order);

                // Domsa -- the desired quantity is substracted when placing an order
                order.Offer.Quantity = order.Offer.Quantity - order.DesiredQuantity;

                /*
                if (order.Offer.Quantity == 0)
                {
                    db.Offers.Remove(order.Offer);
                }
                */

                db.SaveChanges();
                return RedirectToAction("MyOrders");
            }
            else if (order.DesiredQuantity > order.Offer.Quantity) // Domsa
            {
                // Recreating the model such that we can "refresh" the page
                OrderOffer model = new OrderOffer();
                model.Order = order;
                model.Offer = order.Offer;
                // Adds the error message to the ViewBag -- Checked in View
                ViewBag.MyErrorMessage = "Quantity is too big!";
                // "Refresh" the page (calls the page with the same model as parameter)
                return View("CreateCustomer", model);
            }
            else
            {
                return HttpNotFound();
            }

        }

        public ActionResult AcceptOrder(int? id)
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

        [HttpPost, ActionName("AcceptOrder")]
        public ActionResult AcceptOrderConfirm(int id)
        {
            Order order = db.Orders.Find(id);
            if (order.Accepted != true)
            {
                order.Accepted = true;
                // order.Offer.Quantity = order.Offer.Quantity - order.DesiredQuantity;
                db.Entry(order.Offer).State = EntityState.Modified;
                db.SaveChanges();
                var offerId = order.OfferID;
                var offerOrders = db.Orders.Where(o => o.OfferID == offerId);              
                return View("OrdersOfMyOffer", offerOrders.ToList());
            }
            else
            {
                var offerId = order.OfferID;
                var offerOrders = db.Orders.Where(o => o.OfferID == offerId);
                return View("OrdersOfMyOffer", offerOrders.ToList());
            }
        }

        public ActionResult CompleteOrder(int? id)
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

        [HttpPost, ActionName("CompleteOrder")]
        public ActionResult CompleteOrderConfirm(int id)
        {
            Order order = db.Orders.Find(id);
            var offerId = order.OfferID;
            var offerOrders = db.Orders.Where(o => o.OfferID == offerId);
            if (order.Accepted == true && offerOrders.Count() > 1) // if the offer has other orders we delete the current one and return the View to the others
            {
                db.Orders.Remove(order);
                db.SaveChanges();
                offerOrders = db.Orders.Where(o => o.OfferID == offerId);
                return View("OrdersOfMyOffer", offerOrders.ToList());
            }
            else if (order.Accepted == true && offerOrders.Count() == 1) // if there are no other orders we delete the current order AND offer and return to Index + POPUP
            {
                db.Offers.Remove(order.Offer);
                db.Orders.Remove(order);
                db.SaveChanges();
                // TO DO: add pop-up notification
                return View("MyOrders");
            }
            else if (order.Accepted == false) // if the order is not accepted return the same page and show notification
            {
                // TO DO: add pop-up notification
                return View(order);
            }
            return HttpNotFound();
        }

        public ActionResult DeleteOrder(int? id)
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

        [HttpPost, ActionName("DeleteOrder")]
        public ActionResult DeleteOrderConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            order.Offer.Quantity += order.DesiredQuantity;
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
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
