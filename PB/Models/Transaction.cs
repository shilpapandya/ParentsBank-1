using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ParentsBankProject.Models
{
    [CustomValidation(typeof(Transaction), "TransactionDateValidation")]
    public class Transaction
    {
        public int Id { get; set; }
        public virtual int AccountId { get; set; }
        public virtual Accounts Accounts { get; set; }


        public DateTime transactionDate { get; set; }
        [RegularExpression("(?!^0\\.00+$)^\\d{1,3}(?:,\\d{3})*\\.\\d*$", ErrorMessage = "The Transaction amount cannot be 0 or alphanumeric")]
        public decimal transactionAmount { get; set; }

        [Required]
        public string note { get; set; }

        public static ValidationResult TransactionDateValidation(Transaction t, ValidationContext context)
        {
            DateTime StartDate = new DateTime(2017, 1, 1, 0, 0, 0);
            DateTime EndDate = new DateTime(2017, 12, 31, 12, 0, 0);

            if (t == null)
            {
                return ValidationResult.Success;
            }
            int result = DateTime.Compare(t.transactionDate, StartDate);
            int result1 = DateTime.Compare(EndDate, t.transactionDate);
            if (result < 0)
            {
                return new ValidationResult("The transacation date should not be before than 1st Jan 2017");
            }
            if (result1 < 0)
            {
                return new ValidationResult("The transacation date should not be greater than 31st Dec 2017");
            }
            else

                return ValidationResult.Success;

        }

    }
}
