using System;
using System.Data.Entity.ModelConfiguration;
using Translations.DataLayer.Dto;

namespace Translations.DataLayer.Configuration
{
    public class TranslatedWordConfiguration : EntityTypeConfiguration<TranslatedWord>
    {
        public TranslatedWordConfiguration()
        {
            ToTable("Translations");

            ConfigurePrimaryKey();

            ConfigureNavigationProperties();
            ConfigurePrimitiveProperties();
        }
        
        private void ConfigurePrimaryKey()
        {
            Property(translation => translation.LanguageIso3)
                .HasColumnName("Language")
                .HasMaxLength(3);
            HasKey(translation => new { translation.Id, translation.LanguageIso3 });
        }

        private void ConfigureNavigationProperties()
        {
            HasRequired(translation => translation.Word)
                .WithMany(word => word.Translations)
                .HasForeignKey(translation => translation.Id);
        }

        private void ConfigurePrimitiveProperties()
        {
            Property(translation => translation.Value).IsRequired().HasMaxLength(100);
            Property(translation => translation.Description).IsOptional();
        }
    }
}
