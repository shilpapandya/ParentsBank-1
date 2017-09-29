using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PB.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public virtual Accounts AccountId { get; set; }

        public virtual Accounts Account { get; set; }

        public DateTime TransactionDate { get; set; }


        public decimal Amount { get; set; }


        public String Note { get; set; }


    }
}