using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;




namespace ProjBank.Models
{
    [CustomValidation(typeof(Account), "InterestRateValudation")]
    public class Account
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
       
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Owner { get; set; }
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Recipient { get; set; }
        [Required]
        [Display(Name = "Account Name")]
        public string name { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Date Added")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [ReadOnly(true)]
        public DateTime OpenDate { get; set; }

        [Display(Name ="Interest Rate")]
        public decimal InterestRate { get; set; }

        public static ValidationResult InterestRateValudation(Account a, ValidationContext context)
        {
            if (a == null)
            {
                return ValidationResult.Success;
            }
            if (a.InterestRate <= 0 || a.InterestRate >= 100)
            {
                return new ValidationResult("The interest rate should be greater than 0 and less than 100");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
        public decimal getTotalAmount
        {
            get
            {
                decimal count = 0;
                if (Transactions != null)
                {
                    for (int i = 0; i < Transactions.Count(); i++)
                    {
                        count = count + Transactions[i].transactionAmount;
                    }
                }
                return count + interestAccrued;
                
            }
        }


        public DateTime? GetLastTransactionDate
        {
            get
            {

                if (Transactions != null && Transactions.Count()!=0)
                {
                    return Transactions[Transactions.Count() - 1].transactionDate;
                }
                else
                {
                    return null;
                }
            }
        }
         public decimal interestAccrued
        {
            get
            {
                return YTDinterestEarned();
            }
        }
        public decimal YTDinterestEarned()
        {
            decimal dailyInterest = (decimal)InterestRate / (decimal)365;
            decimal principleAmount = 0;
            Dictionary<DateTime, decimal> DailyTransaction = new Dictionary<DateTime, decimal>();
            if (Transactions == null || Transactions.Count() == 0)
            {
                return 0;
            }
            for (int i = 0; i < Transactions.Count(); i++)
            {
                if (Transactions[i].transactionDate < DateTime.Today)
                {
                    if (DailyTransaction != null && DailyTransaction.Keys.Contains(Transactions[i].transactionDate))
                    {
                        DailyTransaction[Transactions[i].transactionDate] = DailyTransaction[Transactions[i].transactionDate] + Transactions[i].transactionAmount;
                    }
                    else
                    {
                        DailyTransaction[Transactions[i].transactionDate] = Transactions[i].transactionAmount;
                    }

                }

            }
            decimal interestAccrued = DailyTransaction.Values.Sum();
            DailyTransaction.Add(DateTime.Today.Date, 0);
            var myList = DailyTransaction.ToList();

            myList.Sort((pair1, pair2) => pair1.Key.CompareTo(pair2.Key));

            // DailyTransaction.Values.ToList().Sort();
            int numberofDays = 0;
            decimal accumulatedInterest = 0;

            int month = myList[0].Key.Month;
            for (int i = 0; i < myList.Count(); i++)
            {
                if (i > 0)
                {
                    if (month == myList[i].Key.Month)
                    {
                        numberofDays = myList[i].Key.DayOfYear - myList[i - 1].Key.DayOfYear;
                    }
                    else
                    {
                        numberofDays = (DateTime.DaysInMonth(myList[i - 1].Key.Year, myList[i - 1].Key.Month) - myList[i - 1].Key.Date.Day) + 1;
                    }
                }

                if (month != myList[i].Key.Month)
                {
                    accumulatedInterest = accumulatedInterest + principleAmount * (dailyInterest / 100) * (numberofDays);
                }
                if (month != myList[i].Key.Month && i != myList.Count() - 1)
                {

                    principleAmount = principleAmount + accumulatedInterest;
                    accumulatedInterest = (principleAmount * ((dailyInterest) / 100) * (myList[i].Key.Day - 1));
                }

                principleAmount = principleAmount + myList[i].Value;
                month = myList[i].Key.Month;


            }
            decimal total = principleAmount + accumulatedInterest;
            return decimal.Round( total - interestAccrued,2 );

        }
        public virtual List<Transaction> Transactions { get; set; }
        public virtual List<Wishlist> Wishlist { get; set; }
        public Account()
        {
            Transactions = new List<Transaction>();
            Wishlist = new List<Models.Wishlist>();
        }

        public bool IsOwner(string currentUser)
        {
            // HELPER METHOD TO CHECK IF THE USER PASSED IN AS THE ARGUMENT
            // IS THE OWNER
            if (string.IsNullOrWhiteSpace(currentUser))
            {
                return false;
            }

            if (currentUser.ToUpper() == Owner.ToUpper())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsRecipient(string currentUser)
        {
            // HELPER METHOD TO CHECK IF THE USER PASSED IN AS THE ARGUMENT
            // IS THE Recipient
            if (string.IsNullOrWhiteSpace(currentUser))
            {
                return false;
            }

            if (currentUser.ToUpper() == Recipient.ToUpper())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

       

        public bool IsOwnerOrRecipient(string currentUser)
        {
            // HELPER METHOD TO CHECK IF THE USER PASSED IN AS THE ARGUMENT
            // IS THE OWNER OR THE Recipient
            return IsOwner(currentUser) || IsRecipient(currentUser);
        }
    }
}