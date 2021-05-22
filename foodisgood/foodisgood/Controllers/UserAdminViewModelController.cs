using foodisgood.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace foodisgood.Controllers
{
    public class UserAdminViewModelController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        // GET: EditUser

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        public ActionResult Edit(string email)
        {
            ApplicationUser appUser = new ApplicationUser();
            appUser = UserManager.FindByEmail(email);
            UserAdminViewModel user = new UserAdminViewModel();
            user.Location = appUser.Location;
            user.FirstName = appUser.FirstName;
            user.LastName = appUser.LastName;
            user.PhoneNumber = appUser.PhoneNumber;

            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(UserAdminViewModel model)
        {


            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var currentUser = UserManager.FindByEmail(model.Email);
            currentUser.FirstName = model.FirstName;
            currentUser.LastName = model.LastName;
            currentUser.PhoneNumber = model.PhoneNumber;
            currentUser.Location = model.Location;
            await UserManager.UpdateAsync(currentUser);
            var ctx = store.Context;
            ctx.SaveChanges();
            TempData["msg"] = "Profile Changes Saved !";
            return RedirectToAction("Index");
        }


        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            var offers = db.Offers.Where(o => o.UserID == id).ToList();
            var orders = db.Orders.Where(o => o.BuyerUserID == id).ToList();

            foreach (var offer in offers)
            {
                user.Offers.Remove(offer);
                db.Offers.Remove(offer);
            }
            foreach (var order in orders)
            {
                user.Orders.Remove(order);
                db.Orders.Remove(order);
            }
            db.Users.Remove(user);
            db.SaveChanges();
            return View("Index", db.Users.ToList());
        }
    }
}