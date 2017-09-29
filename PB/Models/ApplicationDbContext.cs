using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PB.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<PB.Models.Accounts> Accounts { get; set; }

        public System.Data.Entity.DbSet<PB.Models.Transaction> Transactions { get; set; }

        public System.Data.Entity.DbSet<PB.Models.WishList> WishLists { get; set; }
    }
}