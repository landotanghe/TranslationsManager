﻿using System.Data.Entity;
using Translations.DataLayer.Configuration;
using Translations.DataLayer.Dto;

namespace Translations.DataLayer
{
    internal class TranslationContext : DbContext
    {
        public DbSet<Word> Words { get; set; }
        public DbSet<TranslatedWord> TranslatedWords { get; set; }
        public DbSet<Quiz> Quizes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new SentenceConfiguration());
            modelBuilder.Configurations.Add(new TranslatedWordConfiguration());
            modelBuilder.Configurations.Add(new WordConfiguration());
            modelBuilder.Configurations.Add(new QuestionConfiguration());
            modelBuilder.Configurations.Add(new QuizConfiguration());
        }
    }
}
