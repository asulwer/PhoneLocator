using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PhoneLocator.Models
{
    public class PhoneLocatorContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public PhoneLocatorContext() : base("name=PhoneLocatorContext")
        {
        }

        public System.Data.Entity.DbSet<PhoneLocator.Models.Location> Locations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Location>().Property(x => x.Latitude).HasPrecision(10, 7);
            modelBuilder.Entity<Location>().Property(x => x.Longitude).HasPrecision(10, 7);
            modelBuilder.Entity<Location>().Property(x => x.Speed).HasPrecision(10, 7);
            modelBuilder.Entity<Location>().Property(x => x.Heading).HasPrecision(10, 7);
        }
    }
}
