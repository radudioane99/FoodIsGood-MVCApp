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
            string ID = User.Identity.GetUserId();
            var users = db.Users.Where(u => u.Id != ID).ToList();
            return View(users);
        }

        public ActionResult Edit(string id)
        {
            ApplicationUser appUser = new ApplicationUser();
            appUser = UserManager.FindById(id);
            UserAdminViewModel viewUser = new UserAdminViewModel();
            viewUser.Id = appUser.Id;
            viewUser.Location = appUser.Location;
            viewUser.FirstName = appUser.FirstName;
            viewUser.LastName = appUser.LastName;
            viewUser.PhoneNumber = appUser.PhoneNumber;
            viewUser.Email = appUser.Email;

            return View(viewUser);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(UserAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var currentUser = UserManager.FindById(model.Id);
            currentUser.FirstName = model.FirstName;
            currentUser.LastName = model.LastName;
            currentUser.PhoneNumber = model.PhoneNumber;
            currentUser.Location = model.Location;
            currentUser.Email = model.Email;
            currentUser.UserName = model.Email;
            await UserManager.UpdateAsync(currentUser);
            var ctx = store.Context;
            ctx.SaveChanges();
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
            return View("Index");
        }
    }
}