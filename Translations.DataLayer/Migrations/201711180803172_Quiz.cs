namespace Translations.DataLayer.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Quiz : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Quizes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Score = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        QuizId = c.Int(nullable: false),
                        Number = c.Int(nullable: false),
                        IsCorrect = c.Boolean(nullable: false),
                        Attempts = c.Int(nullable: false),
                        Answer_Id = c.Int(),
                        Answer_LanguageIso3 = c.String(maxLength: 3),
                        Value_Id = c.Int(nullable: false),
                        Value_LanguageIso3 = c.String(nullable: false, maxLength: 3),
                    })
                .PrimaryKey(t => new { t.QuizId, t.Number })
                .ForeignKey("dbo.Translations", t => new { t.Answer_Id, t.Answer_LanguageIso3 })
                .ForeignKey("dbo.Quizes", t => t.QuizId, cascadeDelete: true)
                .ForeignKey("dbo.Translations", t => new { t.Value_Id, t.Value_LanguageIso3 }, cascadeDelete: true)
                .Index(t => t.QuizId)
                .Index(t => new { t.Answer_Id, t.Answer_LanguageIso3 })
                .Index(t => new { t.Value_Id, t.Value_LanguageIso3 });            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", new[] { "Value_Id", "Value_LanguageIso3" }, "dbo.Translations");
            DropForeignKey("dbo.Questions", "QuizId", "dbo.Quizes");
            DropForeignKey("dbo.Questions", new[] { "Answer_Id", "Answer_LanguageIso3" }, "dbo.Translations");
            DropIndex("dbo.Questions", new[] { "Value_Id", "Value_LanguageIso3" });
            DropIndex("dbo.Questions", new[] { "Answer_Id", "Answer_LanguageIso3" });
            DropIndex("dbo.Questions", new[] { "QuizId" });
            DropTable("dbo.Questions");
            DropTable("dbo.Quizes");
        }
    }
}
