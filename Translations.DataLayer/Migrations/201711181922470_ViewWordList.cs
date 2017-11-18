namespace Translations.DataLayer.Migrations
{
    using DbContexts;
    using System;
    using System.Data.Entity.Migrations;

    public partial class ViewWordList : DbMigration
    {
        public override void Up()
        {
            var script = @"
                CREATE VIEW dbo.WordList
                AS
                select w.Value as dutch, t.Value german from dbo.Translations t
                join dbo.Words w
                on w.id = t.Id
                where Language = 'deu'";
            Execute(script);
        }


        public override void Down()
        {
            Execute("DROP VIEW dbo.WordList");
        }

        private static void Execute(string script)
        {
            var context = new TranslationContext();
            context.Database.ExecuteSqlCommand(script);
        }
    }
}
