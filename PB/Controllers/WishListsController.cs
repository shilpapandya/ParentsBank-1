using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ParentsBankProject.Models;

namespace ParentsBankProject.Controllers
{
    public class WishListsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: WishLists
        public ActionResult Index()
        {
            var WishLists = db.WishLists.Include(w => w.Account);
            return View(WishLists.ToList());
        }

        // GET: WishLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WishList WishList = db.WishLists.Find(id);
            if (WishList == null)
            {
                return HttpNotFound();
            }
            if (WishList.Account.IsOwnerOrRecipient (User.Identity.Name))
            {
                return View(WishList);
            }
            else
            {
                return HttpNotFound();
            }
            return View(WishList);
        }

        // GET: WishLists/Create
        public ActionResult Create()
        {
            // GET ONLY ACCOUNTS BELONGING TO THE CURRENT USER FOR THE DROPDOWN
            string currentlyLoggedInUsername = User.Identity.Name;
            var accounts = db.Accounts
                .Where(x => x.Owner == currentlyLoggedInUsername
                || x.Recipient == currentlyLoggedInUsername).ToList();

            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Recipient");
            return View();
        }

        // POST: WishLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AccountId,DateAdded,Cost,Description,Link,Purchased")] WishList WishList)
        {
            if (ModelState.IsValid)
            {
                db.WishLists.Add(WishList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // GET ONLY ACCOUNTS BELONGING TO THE CURRENT USER FOR THE DROPDOWN
            string currentlyLoggedInUsername = User.Identity.Name;
            var accounts = db.Accounts
                .Where(x => x.Owner == currentlyLoggedInUsername
                || x.Recipient == currentlyLoggedInUsername).ToList();

            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Recipient", WishList.AccountId);
            return View(WishList);
        }

        // GET: WishLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WishList WishList = db.WishLists.Find(id);
            if (WishList == null)
            {
                return HttpNotFound();
            }

            // ONLY SHOW THE EDIT FOR IF THE USER HAS PERMISSION TO THE USER
            if (WishList.Account.IsOwnerOrRecipient(User.Identity.Name))
            {
                // GET ONLY ACCOUNTS BELONGING TO THE CURRENT USER FOR THE DROPDOWN
                string currentlyLoggedInUsername = User.Identity.Name;
                var accounts = db.Accounts
                    .Where(x => x.Owner == currentlyLoggedInUsername
                    || x.Recipient == currentlyLoggedInUsername).ToList();

                ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Recipient", WishList.AccountId);
                return View(WishList);
            }
            else
            {
                return HttpNotFound();
            }

        }

        // POST: WishLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccountId,DateAdded,Cost,Description,Link,Purchased")] WishList WishList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(WishList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // GET ONLY ACCOUNTS BELONGING TO THE CURRENT USER FOR THE DROPDOWN
            string currentlyLoggedInUsername = User.Identity.Name;
            var accounts = db.Accounts
                .Where(x => x.Owner == currentlyLoggedInUsername
                || x.Recipient == currentlyLoggedInUsername).ToList();

            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Recipient", WishList.AccountId);
            return View(WishList);
        }

        // GET: WishLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WishList WishList = db.WishLists.Find(id);
            if (WishList == null)
            {
                return HttpNotFound();
            }
            // ASSUMING A BUSINESS RULE THAT ONLY LET'S OWNERS DELETE WishLists - THIS
            // CHECK ONLY LET OWNERS SEE THE DELETE VIEW
            if (WishList.Account.IsOwner(User.Identity.Name))
            {
                return View(WishList);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // POST: WishLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WishList WishList = db.WishLists.Find(id);

            // ASSUMING A BUSINESS RULE THAT ONLY OWNERS CAN DELETE WishLists
            // THIS CHECK ENSURES ONLY OWNERS CAN PERFORM THIS STEP
            if (WishList != null && WishList.Account.IsOwner(User.Identity.Name))
            {
                db.WishLists.Remove(WishList);
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
