using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PB.Models
{
    public class WishList
    {
        public int Id { get; set; }

        public virtual string AccountId { get; set; }

        public virtual Accounts Account { get; set; }

        public DateTime DateAdded { get; set; }


        public decimal Cost { get; set; }


        public String Description { get; set; }
        public String Link { get; set; }
        public bool Purchased { get; set; }
    }
}