using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjBank.Models;
using System.Web.UI;

namespace ProjBank.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
      
        // GET: Transactions
        public ActionResult Index()
        {
            //var transactions = db.Transactions.Include(t => t.Accounts);
            //return View(transactions.ToList());
            return View(db.Transactions.Where(x => x.Accounts.Owner == User.Identity.Name || x.Accounts.Recipient == User.Identity.Name).ToList().OrderBy(o => o.transactionDate).ToList());
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
           
            if (db.Accounts.AsNoTracking().Where(x => x.Recipient == User.Identity.Name).ToList().Count() > 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
           
            ViewBag.AccountId = new SelectList(db.Accounts.Where(p => p.Owner == User.Identity.Name), "Id", "Recipient");
            // ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Owner");
            return View();
            
           
            
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AccountId,transactionDate,transactionAmount,note")] Transaction transaction)
        {
            if (db.Accounts.AsNoTracking().Where(x => x.Recipient == User.Identity.Name).ToList().Count() > 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                /*     decimal accBalance = 0;
                     for(int i = 0; i < db.Transactions.Count(); i++)
                     {
                         List<Transaction> Transactions = new List<Transaction>();
                         Transactions = db.Transactions.ToList();
                         if(Transactions[i].AccountId == transaction.AccountId)

                         {
                             accBalance = accBalance + Transactions[i].transactionAmount;
                         }

                     } */
                //decimal accBalance = transaction.Accounts.getTotalAmount;
                List<Transaction> orderedTransaction = db.Accounts.Find(transaction.AccountId).Transactions.ToList();
                decimal totalAmt = 0;
                for (int i = 0; i < orderedTransaction.Count(); i++)
                {
                    if (DateTime.Compare(orderedTransaction[i].transactionDate, transaction.transactionDate) <= 0)
                    {
                        totalAmt = totalAmt + orderedTransaction[i].transactionAmount + db.Accounts.Find(transaction.AccountId).interestAccrued; 
                    }
                }
                if (Math.Abs(transaction.transactionAmount) > Decimal.Round( totalAmt,2 ) && transaction.transactionAmount < 0)
                {

                    ViewBag.errorMessage = "Debit amount cannot be less than account balance";
                    ViewBag.AccountId = new SelectList(db.Accounts.Where(p => p.Owner == User.Identity.Name), "Id", "Recipient", transaction.AccountId);
                    //  ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Owner", transaction.AccountId);
                    return View(transaction);
                }
                decimal accBalance = db.Accounts.Find(transaction.AccountId).getTotalAmount;
                if (Math.Abs(transaction.transactionAmount) > accBalance && transaction.transactionAmount<0)
                {
                    ViewBag.errorMessage = "Debit amount cannot be greater than account balance";
                    ViewBag.AccountId = new SelectList(db.Accounts.Where(p => p.Owner == User.Identity.Name), "Id", "Recipient");
                  //  ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Owner", transaction.AccountId);
                    return View();
             
                }
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(db.Accounts.Where(p => p.Owner == User.Identity.Name), "Id", "Recipient", transaction.AccountId);
            //ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Owner", transaction.AccountId);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            if (db.Accounts.Find(transaction.AccountId).Owner == User.Identity.Name)
            {
                ViewBag.AccountId = new SelectList(db.Accounts.Where(p => p.Owner == User.Identity.Name), "Id", "Recipient", transaction.AccountId);
                //ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Owner", transaction.AccountId);
                return View(transaction);
            }
            else
            {
                return HttpNotFound();
            }
           
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccountId,transactionDate,transactionAmount,note")] Transaction transaction)
        {
            if (db.Accounts.Find(transaction.AccountId).Owner == User.Identity.Name) { 
                if (ModelState.IsValid)
                {
                    db.Entry(transaction).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                List<Transaction> orderedTransaction = db.Accounts.Find(transaction.AccountId).Transactions.ToList();
                decimal totalAmt = 0;
                for (int i = 0; i < orderedTransaction.Count(); i++)
                {
                    if (DateTime.Compare(orderedTransaction[i].transactionDate, transaction.transactionDate) <= 0)
                    {
                        totalAmt = totalAmt + orderedTransaction[i].transactionAmount + db.Accounts.Find(transaction.AccountId).interestAccrued;
                    }
                }
                if (Math.Abs(transaction.transactionAmount) > Decimal.Round( totalAmt,2 ) && transaction.transactionAmount < 0)
                {

                    ViewBag.errorMessage = "Debit amount cannot be less than account balance";
                    ViewBag.AccountId = new SelectList(db.Accounts.Where(p => p.Owner == User.Identity.Name), "Id", "Recipient", transaction.AccountId);
                    //  ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Owner", transaction.AccountId);
                    return View(transaction);
                }
                decimal accBalance = transaction.Accounts.getTotalAmount;
                if (Math.Abs(transaction.transactionAmount) > accBalance && transaction.transactionAmount < 0)
                {
                    ViewBag.errorMessage = "Debit amount cannot be greater than account balance";
                    ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Recipient");
                    return View();

                }
                ViewBag.AccountId = new SelectList(db.Accounts.Where(p => p.Owner == User.Identity.Name), "Id", "Recipient", transaction.AccountId);
                //ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Owner", transaction.AccountId);
                return View(transaction);

            } else
            {
                return HttpNotFound();
            }
                
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            if (db.Accounts.Find(transaction.AccountId).Owner == User.Identity.Name)
            {
                return View(transaction);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            if (db.Accounts.Find(transaction.AccountId).Owner == User.Identity.Name)
            {
                List<Transaction> orderedTransaction = db.Accounts.Find(transaction.AccountId).Transactions.ToList();
                decimal totalAmt = 0;
                for (int i = 0; i < orderedTransaction.Count(); i++)
                {
                    if (DateTime.Compare(orderedTransaction[i].transactionDate, transaction.transactionDate) <= 0)
                    {
                        totalAmt = totalAmt + orderedTransaction[i].transactionAmount + db.Accounts.Find(transaction.AccountId).interestAccrued;
                    }
                }
                if (Math.Abs(transaction.transactionAmount) > Decimal.Round( totalAmt,2 ) && transaction.transactionAmount < 0)
                {

                    ViewBag.errorMessage = "Debit amount cannot be less than account balance";
                    ViewBag.AccountId = new SelectList(db.Accounts.Where(p => p.Owner == User.Identity.Name), "Id", "Recipient", transaction.AccountId);
                    //  ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Owner", transaction.AccountId);
                    return View(transaction);
                }
                decimal accBalance = transaction.Accounts.getTotalAmount;
                if (Math.Abs(transaction.transactionAmount) > accBalance && transaction.transactionAmount < 0)
                {
                    ViewBag.errorMessage = "Cannot delete as remaining balance will be less than 0";
                    
                    return View("Delete");

                }
                db.Transactions.Remove(transaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            } else
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
