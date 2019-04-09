using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using static DataLogic.Entities.ApplicationUserEntity;
using DataLogic.Entities;

namespace DataLogic.Context
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

        public DbSet<BookingSystemEntity> BookingSystems { get; set; }
        public DbSet<BookingTableEntity> BookingTabels { get; set; }
        public DbSet<ArticleEntity> Articles { get; set; }
    }
}
