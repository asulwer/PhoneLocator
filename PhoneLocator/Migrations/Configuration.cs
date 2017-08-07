namespace PhoneLocator.Migrations
{
    using PhoneLocator.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PhoneLocator.Models.PhoneLocatorContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "PhoneLocator.Models.PhoneLocatorContext";
        }

        protected override void Seed(PhoneLocator.Models.PhoneLocatorContext context)
        {
            context.Locations.AddOrUpdate(new Location()
            {
                Updated = DateTime.Now,
                Latitude = 41.8009269m,
                Longitude = -88.3464032m,
                User = "aaron",
                Speed = 0,
                Heading = 0
            });
        }
    }
}
