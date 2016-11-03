namespace DevEvent.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMobileUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MobileUsers",
                c => new
                    {
                        sId = c.String(nullable: false, maxLength: 128),
                        CreatedTime = c.DateTimeOffset(nullable: false, precision: 7),
                        LastLoginTime = c.DateTimeOffset(precision: 7),
                        ProviderName = c.String(),
                    })
                .PrimaryKey(t => t.sId);
            
            CreateTable(
                "dbo.EventMobileUser",
                c => new
                    {
                        MobileUserId = c.String(nullable: false, maxLength: 128),
                        EventId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.MobileUserId, t.EventId })
                .ForeignKey("dbo.MobileUsers", t => t.MobileUserId, cascadeDelete: true)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.MobileUserId)
                .Index(t => t.EventId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventMobileUser", "EventId", "dbo.Events");
            DropForeignKey("dbo.EventMobileUser", "MobileUserId", "dbo.MobileUsers");
            DropIndex("dbo.EventMobileUser", new[] { "EventId" });
            DropIndex("dbo.EventMobileUser", new[] { "MobileUserId" });
            DropTable("dbo.EventMobileUser");
            DropTable("dbo.MobileUsers");
        }
    }
}
