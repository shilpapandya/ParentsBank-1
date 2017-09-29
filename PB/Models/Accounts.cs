using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PB.Models
{
    public class Accounts
    {
        public virtual int Id { get; set; }
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Owner { get; set; }
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Recipient { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public decimal accountBalance { get; set; }

        [Editable(false)]
        public DateTime? OpenDate { get; set; }
        [Range(0, 100, ErrorMessage = "Interest Rate cannot less than 0 or greater than 100")]
        public int InterestRate { get; set; }

        public void YTDinterestEarned()
        {

        }
        /* public virtual List<Transactions> Transactions { get; set; }
        public Accounts()
        {
            Transactions = new List<Transactions>();
        }
        */
    }
}