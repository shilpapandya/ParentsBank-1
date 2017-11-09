using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjBank.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjBank.Controllers
{
    [Authorize]
    public class WishlistsController : Controller
    {
        
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Wishlists
        public ActionResult Index()
        {
            //var wishlists = db.Wishlists.Include(w => w.Account);
            //return View(wishlists.ToList());
            return View(db.Wishlists.Where(x => x.Account.Owner == User.Identity.Name || x.Account.Recipient == User.Identity.Name).ToList());
        }

        // GET: Wishlists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wishlist wishlist = db.Wishlists.Find(id);
            if (wishlist == null)
            {
                return HttpNotFound();
            }
            if (wishlist.Account.IsOwnerOrRecipient(User.Identity.Name))
            {
                return View(wishlist);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // GET: Wishlists/Create
        public ActionResult Create()
        {
            string currentlyLoggedInUsername = User.Identity.Name;
            var accounts = db.Accounts
                .Where(x => x.Owner == currentlyLoggedInUsername
                || x.Recipient == currentlyLoggedInUsername).ToList();
           // ViewBag.AccountId = new SelectList(accounts, "Id", "Owner");
            ViewBag.AccountId = new SelectList(accounts, "Id", "Recipient");
            return View();
        }

        // POST: Wishlists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AccountId,DateAdded,Cost,Description,Link,Purchased")] Wishlist wishlist)
        {
            wishlist.DateAdded = DateTime.Today;
            if (ModelState.IsValid)
            {
                db.Wishlists.Add(wishlist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            string currentlyLoggedInUsername = User.Identity.Name;
            var accounts = db.Accounts
                .Where(x => x.Owner == currentlyLoggedInUsername
                || x.Recipient == currentlyLoggedInUsername).ToList();
            ViewBag.AccountId = new SelectList(accounts, "Id", "Recipient",wishlist.AccountId);
            //ViewBag.AccountId = new SelectList(accounts, "Id", "Owner", wishlist.AccountId);
            return View(wishlist);
        }

        // GET: Wishlists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wishlist wishlist = db.Wishlists.Find(id);
            if (wishlist == null)
            {
                return HttpNotFound();
            }
            if (wishlist.Account.IsOwnerOrRecipient(User.Identity.Name))
            {
                // GET ONLY ACCOUNTS BELONGING TO THE CURRENT USER FOR THE DROPDOWN
                string currentlyLoggedInUsername = User.Identity.Name;
                var accounts = db.Accounts
                    .Where(x => x.Owner == currentlyLoggedInUsername
                    || x.Recipient == currentlyLoggedInUsername).ToList();
                ViewBag.AccountId = new SelectList(accounts, "Id", "Recipient",wishlist.AccountId);
               // ViewBag.AccountId = new SelectList(accounts, "Id", "Owner", wishlist.AccountId);
                return View(wishlist);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // POST: Wishlists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccountId,DateAdded,Cost,Description,Link,Purchased")] Wishlist wishlist)
        {
            wishlist.DateAdded = DateTime.Today;
            if (ModelState.IsValid)
            {
                db.Entry(wishlist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // GET ONLY ACCOUNTS BELONGING TO THE CURRENT USER FOR THE DROPDOWN
            string currentlyLoggedInUsername = User.Identity.Name;
            var accounts = db.Accounts
                .Where(x => x.Owner == currentlyLoggedInUsername
                || x.Recipient == currentlyLoggedInUsername).ToList();
            ViewBag.AccountId = new SelectList(accounts, "Id", "Recipient",wishlist.AccountId);
            //ViewBag.AccountId = new SelectList(accounts, "Id", "Owner", wishlist.AccountId);
            return View(wishlist);
        }

        // GET: Wishlists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wishlist wishlist = db.Wishlists.Find(id);
            if (wishlist == null)
            {
                return HttpNotFound();
            }
            // ASSUMING A BUSINESS RULE THAT ONLY LET'S OWNERS DELETE WishLists - THIS
            // CHECK ONLY LET OWNERS SEE THE DELETE VIEW
            if (wishlist.Account.IsOwner(User.Identity.Name))
            {
                return View(wishlist);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // POST: Wishlists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Wishlist wishlist = db.Wishlists.Find(id);


            // ASSUMING A BUSINESS RULE THAT ONLY OWNERS CAN DELETE WishLists
            // THIS CHECK ENSURES ONLY OWNERS CAN PERFORM THIS STEP
            if (wishlist != null && wishlist.Account.IsOwner(User.Identity.Name))
            {
                db.Wishlists.Remove(wishlist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult Search(string wishlistDesc, string wishlistPrice)
        {
            string description = wishlistDesc;
            decimal? price = null;
            if (!string.IsNullOrEmpty(wishlistPrice))
            {
                price = decimal.Parse(wishlistPrice);
            }
            List<Wishlist> results = null;
            List<Wishlist> validAccounts = db.Wishlists.Where(x => x.Account.Owner == User.Identity.Name || x.Account.Recipient == User.Identity.Name).ToList();
            if (!string.IsNullOrWhiteSpace(description))
            {
                if (results == null || results.Count() == 0)
                {
                    results = validAccounts.Where(s => s.Description.Contains(description)).ToList();
                }
                else
                {
                    results = results.Where(s => s.Description.Contains(description)).ToList();
                }
            }

            if (price != null)
            {
                if (results == null || results.Count() == 0)
                {
                    results = validAccounts.Where(s => s.Cost == price).ToList();
                }
                else
                {
                    results = results.Where(s => s.Cost == price).ToList();
                }
            }

            List<Wishlist> page1 = null;
            if (results != null)
            {
                page1 = results.ToList();

            }
            else
            {

               page1 = db.Wishlists.Where(x => x.Account.Owner == User.Identity.Name || x.Account.Recipient == User.Identity.Name).ToList();
            }
            return View("Index", page1);
            //TempData["list"] = page1;
            //return RedirectToAction("Index");

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
