using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjBank.Models;

namespace ProjBank.Controllers{
    [Authorize]
    public class AccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        List<Account> currentUser;
        bool isChild = false;
        bool isParent = false;
        // GET: Accounts
        public ActionResult Index()
        {
            if (isChild || isCurUserChild())
            {
                return RedirectToAction("Details", new { Id = currentUser.First().Id });
            }
            if (isParent || isCurUserParent())
            {
                return View(currentUser);
            }
            return View(currentUser);
        }
        public bool isCurUserChild(string n)
        {
            if (n == null)
            {
                n = User.Identity.Name;
            }
            List<Account> acRec = db.Accounts.AsNoTracking().Where(x => x.Recipient == n).ToList();
            if (acRec != null && acRec.Count > 0)
            {
                isChild = true;
                ViewBag.isChild = true;
                currentUser = acRec;
                return true;
            }
            return false;


            //  return View(db.Accounts.ToList());
        }
        public bool isCurUserChild()
        {
            return isCurUserChild(null);
        }
        public bool isCurUserParent(string n)
        {
            if (n == null)
            {
                n = User.Identity.Name;
            }
            List<Account> acPar = db.Accounts.AsNoTracking().Where(x => x.Owner == n).ToList();
            if (acPar != null && acPar.Count > 0)
            {
                isParent = true;
                ViewBag.isParent = true;
                currentUser = acPar;
                return true;
            }
            return false;
        }
        public bool isCurUserParent()
        {
            return isCurUserParent(null);
        }
        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
            if (account.IsOwnerOrRecipient(User.Identity.Name))
            {
                return View(account);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            return View(account);
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            if (isChild || isCurUserChild())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Owner,Recipient,name,OpenDate,InterestRate")] Account account)
        {
            if (ModelState.IsValid)
            {
                account.Owner = User.Identity.Name;
                account.OpenDate = DateTime.Now.Date;
                if (account.Owner.Equals(account.Recipient, StringComparison.OrdinalIgnoreCase))
                {
                    ViewBag.errorMessage = "Email ID of owner and child cannot be same";
                    return View(account);
                }
                // if user is receipent		
                if (account.IsRecipient(User.Identity.Name))
                {
                    ViewBag.errorMessage = "Receipient is already registered.";
                    return View(account);
                }
                if (isCurUserChild(account.Recipient))
                {
                    ViewBag.errorMessage = "Receipient is already registered as child.";
                    return View(account);
                }
                if (isCurUserParent(account.Recipient))
                {
                    ViewBag.errorMessage = "Receipient is already registered as Parent";
                    return View(account);
                }

                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(account);
        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (isParent || isCurUserParent())
            {
                Account account = db.Accounts.Find(id);
                if (account == null)
                {
                    return HttpNotFound();
                }
                if (account.IsOwner(User.Identity.Name))
                {
                    return View(account);
                }
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //  return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Owner,Recipient,name,accountBalance,OpenDate,InterestRate")] Account account)
        {
            if (ModelState.IsValid)
            {
                //Accounts accounts;		              
                if ((isParent || isCurUserParent()) && account.IsOwner(User.Identity.Name))
                {
                    // account.OpenDate = currentUser.FirstOrDefault().OpenDate; // DateTime.Now;
                    try
                    {
                        account.OpenDate = db.Accounts.AsNoTracking().Where(a => a.Id == account.Id).ToList()[0].OpenDate;
                    }
                    catch(Exception e)
                    {
                        account.OpenDate = DateTime.Now.Date;
                    }
                
               
                    db.Entry(account).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(account);
        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (isParent || isCurUserParent())
            {
                Account account = db.Accounts.Find(id);
                if (account.IsOwner(User.Identity.Name))
                {
                    if (account == null)
                    {
                        return HttpNotFound();

                    }
                    return View(account);
                }
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            if (isParent || isCurUserParent())
            {

                Account accounts = db.Accounts.Find(id);
                if (accounts.IsOwner(User.Identity.Name))
                {
                    if (accounts == null)
                    {
                        return HttpNotFound();
                    }
                    if (accounts.getTotalAmount != 0)
                    {
                        ViewBag.errorMessage = "Account cannot be deleted as it has balance";
                        return View("Delete");
                    }
                    db.Accounts.Remove(accounts);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }

        public bool hasBalance(int id)
        {
            if (db.Accounts.Find(id).getTotalAmount != 0)
            {
                return true;
            }
            return false;
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