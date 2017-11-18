using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Translations.DataLayer.Dto;

namespace Translations.DataLayer.Configuration
{
    public class TranslatedSentenceConfiguration : EntityTypeConfiguration<TranslatedSentence>
    {
        public TranslatedSentenceConfiguration()
        {
            ConfigurePrimaryKey();

            ConfigureNavigationProperties();
            ConfigurePrimitiveProperties();
        }


        private void ConfigurePrimaryKey()
        {
            HasKey(sentence => sentence.Id);
            Property(sentence => sentence.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }

        private void ConfigureNavigationProperties()
        {
            HasMany(sentence => sentence.DemonstratedWords)
                .WithMany(word => word.Examples)
                .Map(cross =>
                {
                    cross.ToTable("Examples");
                    cross.MapLeftKey("SentenceId");
                    cross.MapRightKey("WordId", "LanguageIso3");
                });

        }

        private void ConfigurePrimitiveProperties()
        {
            Property(sentence => sentence.Value).IsRequired();
            Property(sentence => sentence.Source).IsRequired();
        }
    }
}
