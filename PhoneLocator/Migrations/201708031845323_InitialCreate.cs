namespace PhoneLocator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Updated = c.DateTime(nullable: true),
                        Latitude = c.Decimal(nullable: true),
                        Longitude = c.Decimal(nullable: true),
                        User = c.String(nullable: true),
                        Speed = c.Decimal(nullable: true),
                        Direction = c.Decimal(nullable: true),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Locations");
        }
    }
}
