namespace Translations.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WordValue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Words", "Value", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Words", "Value");
        }
    }
}
