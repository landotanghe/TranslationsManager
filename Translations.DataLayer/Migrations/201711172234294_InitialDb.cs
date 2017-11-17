namespace Translations.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Translations",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Language = c.String(nullable: false, maxLength: 3),
                        Value = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                    })
                .PrimaryKey(t => new { t.Id, t.Language })
                .ForeignKey("dbo.Words", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Words",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sentences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false),
                        Source = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Translations", "Id", "dbo.Words");
            DropIndex("dbo.Translations", new[] { "Id" });
            DropTable("dbo.Sentences");
            DropTable("dbo.Words");
            DropTable("dbo.Translations");
        }
    }
}
