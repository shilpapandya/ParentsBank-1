using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjBank.Models
{
    [CustomValidation(typeof(Transaction), "TransactionDateValidation")]
    [CustomValidation(typeof(Transaction), "TransactionAmountValidation")]
    public class Transaction
    {
        public int Id { get; set; }
        public virtual int AccountId { get; set; }
        public virtual Account Accounts { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Transaction Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime transactionDate { get; set; }
       
        [Display(Name = "Transaction Amount")]
        public decimal transactionAmount { get; set; }


        [Required]
        [Display(Name ="Note")]
        public string note { get; set; }

        public static ValidationResult TransactionAmountValidation(Transaction t, ValidationContext context)
        {
            if (t == null)
            {
                return ValidationResult.Success;
            }
            if (t.transactionAmount == 0)
            {
                return new ValidationResult("The transaction amount cannot be zero");
            }
            else
            {
                return ValidationResult.Success;
            }

        }
        public static ValidationResult TransactionDateValidation(Transaction t, ValidationContext context)
        {
            DateTime StartDate = new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0);
            DateTime EndDate = DateTime.Today; //new DateTime(DateTime.Now.Year, 12, 31, 12, 0, 0);

            if (t == null)
            {
                return ValidationResult.Success;
            }
            int result = DateTime.Compare(t.transactionDate, StartDate);
            int result1= DateTime.Compare(EndDate, t.transactionDate );
            if (result < 0)
            {
                return new ValidationResult("The transaction date should not be before than 1st Jan 2017");
            }
            if (result1 < 0)
            {
                return new ValidationResult("The transaction date should not be greater than today");
            }
            else
            {
                return ValidationResult.Success;
            }

        }

    }
}