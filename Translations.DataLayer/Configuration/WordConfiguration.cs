using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Translations.DataLayer.Dto;

namespace Translations.DataLayer.Configuration
{
    public class WordConfiguration : EntityTypeConfiguration<Word>
    {
        public WordConfiguration()
        {
            ConfigurePrimaryKey();

            ConfigureNavigationProperties();
            ConfigurePrimitiveProperties();
        }

        private void ConfigurePrimitiveProperties()
        {
            Property(word => word.Description).IsOptional();
        }

        private void ConfigureNavigationProperties()
        {
            HasMany(word => word.Translations)
                .WithRequired(translation => translation.Word)
                .HasForeignKey(translation => translation.Id);
        }

        private void ConfigurePrimaryKey()
        {
            Property(word => word.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasKey(word => word.Id);
        }
    }
}
