namespace Translations.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TranslatedSentences : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TranslatedSentences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false),
                        Translation = c.String(),
                        TranslationLanguageIso3 = c.String(),
                        Source = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Examples",
                c => new
                    {
                        SentenceId = c.Int(nullable: false),
                        WordId = c.Int(nullable: false),
                        LanguageIso3 = c.String(nullable: false, maxLength: 3),
                    })
                .PrimaryKey(t => new { t.SentenceId, t.WordId, t.LanguageIso3 })
                .ForeignKey("dbo.TranslatedSentences", t => t.SentenceId, cascadeDelete: true)
                .ForeignKey("dbo.Translations", t => new { t.WordId, t.LanguageIso3 }, cascadeDelete: true)
                .Index(t => t.SentenceId)
                .Index(t => new { t.WordId, t.LanguageIso3 });
            
            DropTable("dbo.Sentences");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Sentences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false),
                        Source = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Examples", new[] { "WordId", "LanguageIso3" }, "dbo.Translations");
            DropForeignKey("dbo.Examples", "SentenceId", "dbo.TranslatedSentences");
            DropIndex("dbo.Examples", new[] { "WordId", "LanguageIso3" });
            DropIndex("dbo.Examples", new[] { "SentenceId" });
            DropTable("dbo.Examples");
            DropTable("dbo.TranslatedSentences");
        }
    }
}
