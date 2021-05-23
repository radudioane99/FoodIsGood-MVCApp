using foodisgood.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace foodisgood.Controllers
{
    [AllowAnonymous]
    public class OffersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Offers
        public ActionResult Index(string sortOrder, string searchString)
        {
            // Show expired offers!
            var expiredOffers = db.Offers.Where(o => DateTime.Now > o.EndTime && o.Expired == false).ToList();
            if (expiredOffers.Any())
            {
                foreach (Offer offer in expiredOffers)
                {
                    offer.Expired = true;
                    db.Entry(offer).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            // Done deleting.

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";
            ViewBag.QuantitySortParm = sortOrder == "Quantity" ? "quantity_desc" : "Quantity";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.EndDateSortParm = sortOrder == "EndDate" ? "endDate_desc" : "EndDate";

            var offers = from s in db.Offers select s;
            if (User.IsInRole("Customer"))
            {
                offers = from s in db.Offers where s.Expired == false select s;
            } 

            if (!String.IsNullOrEmpty(searchString))
            {
                offers = offers.Where(s => s.Name.Contains(searchString));
            }

            offers = offers.Where(s => s.Quantity > 0);

            switch (sortOrder)
            {
                case "price_desc":
                    offers = offers.OrderByDescending(s => s.PriceUnit);
                    break;
                case "Price":
                    offers = offers.OrderBy(s => s.PriceUnit);
                    break;
                case "quantity_desc":
                    offers = offers.OrderByDescending(s => s.Quantity);
                    break;
                case "Quantity":
                    offers = offers.OrderBy(s => s.Quantity);
                    break;
                case "name_desc":
                    offers = offers.OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    offers = offers.OrderBy(s => s.CreateTime);
                    break;
                case "date_desc":
                    offers = offers.OrderByDescending(s => s.CreateTime);
                    break;
                case "EndDate":
                    offers = offers.OrderBy(s => s.EndTime);
                    break;
                case "endDate_desc":
                    offers = offers.OrderByDescending(s => s.EndTime);
                    break;
                default:
                    offers = offers.OrderBy(s => s.Name);
                    break;
            }


            if (User.IsInRole("AppAdmin"))
            {
                return View("Index", offers.ToList());
            }
            else if (User.IsInRole("Customer"))
            {
                return View("IndexCustomer", offers.ToList());
            }
            else if (User.Identity.IsAuthenticated == false)
            {
                return View("IndexGuest", offers.ToList());
            }
            else
            {
                return View(offers.ToList());
            }
        }

        public ActionResult MyOffers()
        {
            var id = User.Identity.GetUserId();
            id = id.ToString();
            var myOffers = db.Offers.Where(o => o.UserID.Equals(id));
            return View("MyOffers", myOffers.ToList());
        }


        // GET: Offers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole("AppAdmin"))
            {
                return View("Details", offer);
            }
            else if (User.IsInRole("Customer"))
            {
                return View("DetailsCustomer", offer);
            }
            else if (User.Identity.IsAuthenticated == false)
            {
                return View("DetailsGuest", offer);
            }
            else
            {
                return View();
            }
        }


        // GET: Offers/Create
        public ActionResult Create()
        {
            //ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");
            if (User.IsInRole("AppAdmin"))
            {
                ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");
                return View();
            }
            else
            {
                ViewBag.ProductID = new SelectList(db.Products.OrderBy(p => p.Name), "ID", "Name");
                return View();
            }
        }


        // POST: Offers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PriceUnit,Quantity,CreateTime,EndTime,Expired,Description,ProductID,UserID, Name")] Offer offer)
        {
            if (User.IsInRole("Customer"))
            {
                // ViewBag.ProductID = new SelectList(db.Products.Where(p => p.Name == "Tomatoes"), "ID", "Name");
                offer.UserID = User.Identity.GetUserId();
                offer.CreateTime = DateTime.Now.Date;
            }
            if (ModelState.IsValid)
            {
                db.Offers.Add(offer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", offer.ProductID);
            return View(offer);
        }


        // GET: Offers/Create
        public ActionResult CreateCustomer()
        {
            //ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");
            if (User.IsInRole("AppAdmin"))
            {
                ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");
                return View();
            }
            else
            {
                ViewBag.ProductID = new SelectList(db.Products.OrderBy(p => p.Name), "ID", "Name");
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCustomer([Bind(Include = "ID,PriceUnit,Quantity,CreateTime,EndTime,Expired,Description,ProductID,UserID, Name")] Offer offer)
        {
            if (User.IsInRole("Customer"))
            {
                // ViewBag.ProductID = new SelectList(db.Products.Where(p => p.Name == "Tomatoes"), "ID", "Name");
                offer.UserID = User.Identity.GetUserId();
                offer.CreateTime = DateTime.Now;
            }
            if (ModelState.IsValid)
            {
                db.Offers.Add(offer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", offer.ProductID);
            return View("CreateCustomer", offer);
        }


        // GET: Offers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", offer.ProductID);
            return View(offer);
        }

        // POST: Offers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PriceUnit,Quantity,CreateTime,EndTime,Expired,Description,Name")] Offer offer)
        {
            Offer initialOffer = db.Offers.AsNoTracking().Single(o => o.ID.Equals(offer.ID));
            offer.ProductID = initialOffer.ProductID;
            offer.UserID = initialOffer.UserID;
            if (ModelState.IsValid)
            {
                db.Entry(offer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", offer.ProductID);
            return View(offer);
        }

        public ActionResult EditCustomer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", offer.ProductID);
            return View(offer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //ID,Quantity,EndTime,Description
        //ID,PriceUnit,Quantity,CreateTime,EndTime,Expired,Description,ProductID, UserID, Name
        public ActionResult EditCustomer([Bind(Include = "ID,Quantity,EndTime,Description,Name")] Offer offer)
        {
            Offer initialOffer = db.Offers.AsNoTracking().Single(o => o.ID.Equals(offer.ID));
            offer.PriceUnit = initialOffer.PriceUnit;
            offer.CreateTime = initialOffer.CreateTime;
            offer.Expired = initialOffer.Expired;
            offer.ProductID = initialOffer.ProductID;
            offer.UserID = initialOffer.UserID;

            if (ModelState.IsValid)
            {
                db.Entry(offer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MyOffers");
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", offer.ProductID);
            return View(offer);
        }



        // GET: Offers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
        }

        // POST: Offers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Offer offer = db.Offers.Find(id);
            db.Offers.Remove(offer);
            db.SaveChanges();
            string id_string = id.ToString();
            var myOffers = db.Offers.Where(o => o.UserID.Equals(id_string));
            return RedirectToAction("MyOffers", myOffers.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult SeeOrders(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            var offerOrders = db.Orders.Where(o => o.OfferID == id);
            return View("~/Views/Orders/OrdersOfMyOffer.cshtml",offerOrders.ToList());
        }
    }
}
