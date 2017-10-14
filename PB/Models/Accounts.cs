using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ParentsBankProject.Models
{
    public class Accounts
    {
        public int Id { get; set; }
        [Required]
        public string name { get; set; }
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Owner { get; set; }
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Recipient { get; set; }


        [Editable(false)]
        public DateTime OpenDate = DateTime.Today;
        [RegularExpression("^(?!0+(?:\\.0+)?$)\\d?\\d(?:\\.\\d+?)?$", ErrorMessage = "Interest Rate cannot less than 0 or greater than 100")]
        public int InterestRate { get; set; }
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
                return count;
                ;
            }
        }


        public DateTime? GetLastTransactionDate
        {
            get
            {

                if (Transactions != null && Transactions.Count() != 0)
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

            var myList = DailyTransaction.ToList();
            myList.Sort((pair1, pair2) => pair1.Key.CompareTo(pair2.Key));
            // DailyTransaction.Values.ToList().Sort();
            int numberofDays = 1;
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
                        numberofDays = DateTime.DaysInMonth(myList[i - 1].Key.Year, myList[i - 1].Key.Month) - myList[i - 1].Key.Date.Day;
                    }
                }

                accumulatedInterest = accumulatedInterest + principleAmount * (dailyInterest / 100) * numberofDays;
                if (month != myList[i].Key.Month)
                {
                    principleAmount = principleAmount + accumulatedInterest;
                    accumulatedInterest = accumulatedInterest + principleAmount * ((dailyInterest) / 100) * myList[i].Key.Day;
                }

                principleAmount = principleAmount + myList[i].Value;
                month = myList[i].Key.Month;


            }


            return principleAmount + accumulatedInterest;


        }
        public virtual List<Transaction> Transactions { get; set; }
        public Accounts()
        {
            Transactions = new List<Transaction>();
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